using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Web.Hosting;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Activation;
using System.Security.Cryptography;
using System.Xml.Xsl;
using System.IO;
using System.Web;

[ServiceBehavior(Namespace = "https://test.paxfacturacion.com.mx:454")]
// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "wcfRecepcion" in code, svc and config file together.
public class wcfRecepcion : IwcfRecepcion
{
    private int errorCode;
    private TimbreFiscalDigital gNodoTimbre;
    private clsOperacionTimbradoSellado gTimbrado;
    private clsInicioSesionUsuario datosUsuario;
    private clsValCertificado gCertificado;
    private clsHSMComunicacion gHSM;
    protected DataTable dtCreditos;

    /*  Códigos de error
     * 
      '''  0   - Sin errores
      ''' 100 - El archivo de texto esta mal formado
      ''' 177 - El certificado no se encuentra en la lista de régimen fiscal
      ''' 179 - El RFC del Certificado no corresponde al del comprobante
      ''' 200 - El certificado está fuera de su periodo de validez
      ''' 333 - El XML no cumple con el esquema de hacienda
      ''' 406 - El nombre de documento no corresponde a ningúno del sistema
      ''' 472 - El comprobante ya está timbrado
      ''' 504 - La fecha del comprobante esta fuera de periodo
      ''' 511 - El sello no corresponde a los datos del comprobante
      ''' 570 - No se pudó recuperar el certificado del comprobante
      ''' 592 - El certificado no es de tipo CSD
      ''' 622 - El servicio no esta disponible -- NOTA: este error es para el público, la causa real es: No se pudo recuperar la llave y el certificado del PAC
      ''' 645 - El comprobante no contiene un sello de emisor
      ''' 799 - Faltan datos del comprobante
      ''' 817 - No se pudo generar el sello del PAC
      ''' 999 - Error durante el registro del comprobante
      '''  90  - Los datos del usuario son requeridos.
      '''  91  - El usuario esta en estado pendiente.
      '''  92  - El usuario esta en estado bloqueado.
      '''  93  - El usuario esta en estado expirado.
      '''  94  - El usuario esta en estado por cambiar contraseña.
      '''  95  - Usuario inexistente.
      '''  96  - Usuario o contraseña incorrecta.
    */


    public string fnEnviarTXT(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion, string sOrigen)
    {
        System.IO.StringReader lector = new System.IO.StringReader(psComprobante);
        string linea = string.Empty;
        string[] seccion = null;
        string[] atributos = null;
        string[] atributosAduana = null;
        int pnId_Usuario = 0;
        string sXmlDocument = string.Empty;
        string sRetAutentication = string.Empty;
        string sRequest = string.Empty;
        string sResponse = string.Empty;


        //Recupera datos del emisor.
        clsOperacionCuenta gDAL = new clsOperacionCuenta();
        clsConfiguracionCreditos css;
        SqlDataReader sdrInfo = null;
        DataTable certificado = new DataTable();
        string resValidacion = string.Empty;
        string sCoriginalEmisor = String.Empty;
        string sCoriginalTimbre = string.Empty;
        string sello = string.Empty;
        string numeroCertificado = string.Empty;

        XmlNode impuestos = null;
        XmlNode padre = null;
        XmlNode padreConcepto = null;
        XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
        XmlDocument xDocumento = new XmlDocument();

        if (sOrigen == "conGT" || sOrigen == "conGE")
        {
            string sServicio = string.Empty;
            switch (sOrigen)
            {
                case "conGT": //Generación y Timbrado
                    sServicio = "GT";
                    break;
                case "conGE": //Generación + Timbrado
                    sServicio = "GE";
                    break;
            }
            sXmlDocument = fnEnviarXML(psComprobante, psTipoDocumento, pnId_Estructura, sNombre, sContraseña, sVersion, sServicio);
            return sXmlDocument;
        }

        if (sOrigen == "conT" || sOrigen == "wsdlT")
        {

            return "Este servicio es para Generación y timbrado.";
        }

        if (sVersion == "3.0")
        {
            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xDocumento = new XmlDocument(nsm.NameTable);
            xDocumento.CreateXmlDeclaration("1.0", "UTF-8", "no");
            xDocumento.AppendChild(xDocumento.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));
        }

        if (sVersion == "3.2")
        {
            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xDocumento = new XmlDocument(nsm.NameTable);
            xDocumento.CreateXmlDeclaration("1.0", "UTF-8", "no");
            xDocumento.AppendChild(xDocumento.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));
        }


        sRetAutentication = fnAutentication(sNombre, sContraseña, ref pnId_Usuario, ref datosUsuario);

        //*******************************************************Insertar Request en tabla de acuses
        sRequest = clsComun.fnRequestRecepcion(psComprobante, psTipoDocumento, pnId_Estructura.ToString(), sNombre, sContraseña);
        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sRequest, DateTime.Now, "0", "Request", "E", string.Empty);
        //*******************************************************Insertar Request en tabla de acuses


        string[] est = sRetAutentication.Split('-');
        if (est.Length > 2)
        {
            sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
        }
        else
        {
            sRetAutentication = est[0].Trim();
        }

        if (sRetAutentication == "0")
        {

            if (pnId_Estructura == 0)
                pnId_Estructura = Convert.ToInt32(fnRecuperaMatriz(pnId_Usuario));

            //Revisar los creditos disponibles.
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso3 - " + "Revisa Creditos Disponibles.");
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');


            if (dtCreditos.Rows.Count > 0)
            {
                css = new clsConfiguracionCreditos();
                double dCostCred = css.fnRecuperaPrecioServicio(1);
                double creditos = 0;
                creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                if (creditos == 0)
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    //ViewState["dtCreditos"] = dtCreditos;
                    double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                    if (creditos2 == 0)
                    {
                        return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }

                    if (creditos2 < dCostCred)
                    {
                        return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }
                }
                else
                {
                    if (creditos < dCostCred)
                    {
                        return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }
                }

            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                //ViewState["dtCreditos"] = dtCreditos;
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                css = new clsConfiguracionCreditos();
                double dCostCred = css.fnRecuperaPrecioServicio(4);

                if (creditos == 0)
                {
                    return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                }

                if (creditos < dCostCred)
                {
                    return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                }
            }

            if (sVersion == "3.0")
            {
                while (true)
                {
                    linea = lector.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                        break;

                    seccion = linea.Split('?');

                    try
                    {
                        atributos = seccion[1].Split('|');

                        if (seccion[0] == "im" || seccion[0] == "it" || seccion[0] == "ir")
                        {
                            if (impuestos == null)
                            {
                                if (seccion[0] == "im")
                                    impuestos = xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Impuestos", atributos));
                                else
                                    impuestos = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3"));

                            }
                        }

                        switch (seccion[0])
                        {
                            case "co":
                                fnCrearElementoRoot(xDocumento, atributos);
                                break;
                            case "re":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Emisor", atributos));
                                break;
                            case "de":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "DomicilioFiscal", atributos));
                                break;
                            case "ee":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "ExpedidoEn", atributos));
                                break;
                            case "rr":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Receptor", atributos));
                                break;
                            case "dr":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Domicilio", atributos));
                                break;
                            case "cc":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);
                                if (padre == null)
                                    padre = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));
                                break;
                            case "ir":
                                padre = impuestos.SelectSingleNode("cfdi:Retenciones", nsm);
                                if (padre == null)
                                    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Retencion", atributos));
                                break;
                            case "it":
                                padre = impuestos.SelectSingleNode("cfdi:Traslados", nsm);
                                if (padre == null)
                                    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Traslado", atributos));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El archivo de texto esta mal formado", pnId_Usuario);
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"); //El archivo de texto esta mal formado
                    }
                }

            }

            if (sVersion == "3.2")
            {
                while (true)
                {
                    linea = lector.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                        break;

                    seccion = linea.Split('?');

                    try
                    {
                        atributos = seccion[1].Split('|');

                        //Revisa datos aduanales
                        if (atributos[0].Contains("cantidad"))
                        {
                            atributosAduana = null;
                            if (seccion.Length > 2)
                            {
                                atributosAduana = seccion[2].Split('|');
                            }
                        }

                        if (seccion[0] == "im" || seccion[0] == "it" || seccion[0] == "ir")
                        {
                            if (impuestos == null)
                            {
                                if (seccion[0] == "im")
                                    impuestos = xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Impuestos", atributos));
                                else
                                    impuestos = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3"));

                            }
                        }

                        switch (seccion[0])
                        {
                            case "co":
                                fnCrearElementoRoot32(xDocumento, atributos);
                                break;
                            case "re":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Emisor", atributos));
                                break;
                            case "de":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "DomicilioFiscal", atributos));
                                break;
                            case "ee":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "ExpedidoEn", atributos));
                                break;
                            case "rf":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "RegimenFiscal", atributos));
                                break;
                            case "rr":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Receptor", atributos));
                                break;
                            case "dr":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Domicilio", atributos));
                                break;
                            case "cc":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);
                                if (padre == null)
                                {
                                    padre = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3"));

                                    if (atributosAduana != null)
                                    {
                                        padreConcepto = xDocumento.DocumentElement.LastChild.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));
                                        //padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsm);
                                        padreConcepto.AppendChild(fnCrearElemento(xDocumento, "InformacionAduanera", atributosAduana));
                                    }
                                    else
                                    {
                                        padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));
                                    }
                                }
                                else
                                {
                                    if (atributosAduana != null)
                                    {
                                        padreConcepto = xDocumento.DocumentElement.LastChild.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));
                                        //padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsm);
                                        padreConcepto.AppendChild(fnCrearElemento(xDocumento, "InformacionAduanera", atributosAduana));
                                    }
                                    else
                                    {
                                        padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));
                                    }
                                }
                                break;
                            case "ir":
                                padre = impuestos.SelectSingleNode("cfdi:Retenciones", nsm);
                                if (padre == null)
                                    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Retencion", atributos));
                                break;
                            case "it":
                                padre = impuestos.SelectSingleNode("cfdi:Traslados", nsm);
                                if (padre == null)
                                    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Traslado", atributos));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El archivo de texto esta mal formado", pnId_Usuario);
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"); //El archivo de texto esta mal formado
                    }
                }

            }


            try
            {
                ////Según version se obtiene datos fiscales

                sdrInfo = gDAL.fnObtenerDatosFiscalesSuc(pnId_Estructura);

                if (sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read())
                {
                    //Obtener datos del emisor, llave, certificado, y password
                    certificado = clsTimbradoFuncionalidad.ObtenerCertificado(Convert.ToInt32(sdrInfo["id_rfc"].ToString()));
                }


                byte[] bLlave = (byte[])certificado.Rows[0]["key"];
                byte[] bCertificado = (byte[])certificado.Rows[0]["certificado"];
                string sPassword = certificado.Rows[0]["password"].ToString();


                ////Preparamos los objetos de manejo tanto de la llave como del certificado
                gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                gCertificado = new clsValCertificado(bCertificado);


                X509Certificate2 certEmisor = new X509Certificate2();
                certEmisor.Import(Utilerias.Encriptacion.DES3.Desencriptar(bCertificado));

                //Cerificado para agregar al XML
                string sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());

                //Numero del certificado
                numeroCertificado = gCertificado.ObtenerNoCertificado();



                ////Generamos la cadena original
                XPathNavigator navNodoTimbre = xDocumento.CreateNavigator();
                //sCoriginalEmisor = gTimbrado.fnConstruirCadenaTimbrado(navNodoTimbre, scadena); //"cadenaoriginal_3_2"); 

                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                switch (sVersion)
                {
                    case "3.0":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(CaOri.V30));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(navNodoTimbre, args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCoriginalEmisor = srDll.ReadToEnd();


                        break;
                    case "3.2":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(CaOri.V32));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(navNodoTimbre, args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCoriginalEmisor = srDll.ReadToEnd();

                        break;
                }


                //Genera sello de la cadena original
                clsNodoTimbre creadorTimbre = new clsNodoTimbre();
                switch (sVersion)
                {
                    case "3.0":
                        sello = gTimbrado.fnGenerarSello(sCoriginalEmisor, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1, true);
                        break;
                    case "3.2":
                        sello = gTimbrado.fnGenerarSello(sCoriginalEmisor, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1, true);
                        break;
                }

                //Asignar los valores de certificado,numero de certificado y sello.
                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocumento.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).SetValue(numeroCertificado);
                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);
                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sello);



            }
            catch (Exception)
            {
                return "No se pudo generar el sello del emisor, revise la versión solicitada.";
            }


            lector.Close();
            //Mandamos el XML a revision y firmado
            //fnActualizaCreditos(dtCreditos);
            sXmlDocument = fnEnviarXML(xDocumento.DocumentElement.OuterXml, psTipoDocumento, pnId_Estructura, sNombre, sContraseña, sVersion, "GT");

        }
        else
        {
            //*******************************************************Insertar Response en tabla de acuses
            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sRetAutentication, "Response", "E", string.Empty);
            //*******************************************************Insertar Response en tabla de acuses
            sXmlDocument = clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
        }

        return sXmlDocument;
    }

    public string fnEnviarXML(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion, string sServicio)
    {

        int pnId_Usuario = 0;
        string sXmlDocument = string.Empty;
        string sRetAutentication = string.Empty;
        string sCadenaOriginal = string.Empty;
        string sRetornoSAT = string.Empty;
        string sRequest = string.Empty;
        string sResponse = string.Empty;
        string sCadenaOriginalEmisor = string.Empty;
        string scadena = string.Empty;
        string sesquema = string.Empty;
        clsConfiguracionCreditos css;

        switch (sVersion)
        {
            case "3.0":
                //scadena = "cadenaoriginal_3_0";
                sesquema = "esquema_v3";
                break;
            case "3.2":
                //scadena = "cadenaoriginal_3_2";
                sesquema = "esquema_v3_2";
                break;
        }

        //Revisar version
        if (sVersion == "3.0")
        {
            return "999 - Por disposición oficial estos comprobantes ya no se timbran.";
        }

        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(wcfRecepcion.AcceptAllCertificatePolicy);


        sRetAutentication = fnAutentication(sNombre, sContraseña, ref pnId_Usuario, ref datosUsuario);
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso1 - " + "Verifica el usuario.");

        try
        {
            //*******************************************************Insertar Request en tabla de acuses
            sRequest = clsComun.fnRequestRecepcion(psComprobante, psTipoDocumento, pnId_Estructura.ToString(), sNombre, sContraseña);
            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sRequest, DateTime.Now, "0", "Request", "E", string.Empty);
            //*******************************************************Insertar Request en tabla de acuses
        }
        catch
        {
            //*******************************************************Insertar Response en tabla de acuses
            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
            //*******************************************************Insertar Response en tabla de acuses
            return clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"); //El archivo de texto esta mal formado
        }



        string[] est = sRetAutentication.Split('-');
        if (est.Length > 2)
        {
            sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
        }
        else
        {
            sRetAutentication = est[0].Trim();
        }
        if (sRetAutentication == "0")
        {
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso2 - " + "Recupera Estructura.");
            if (pnId_Estructura == 0)
                pnId_Estructura = Convert.ToInt32(fnRecuperaMatriz(pnId_Usuario));

            int pnId_tipo_documento;

            //Revisar los creditos disponibles.
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso3 - " + "Revisa Creditos Disponibles.");
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');

            int nServicio = 0;
            switch (sServicio)
            {
                case "GT": //Generación y Timbrado
                    nServicio = 4;
                    break;
                case "GE": //Generación + Envio
                    nServicio = 10;
                    break;
            }
            if (dtCreditos.Rows.Count > 0)
            {
                css = new clsConfiguracionCreditos();
                double dCostCred = css.fnRecuperaPrecioServicio(nServicio);
                double creditos = 0;
                creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                if (creditos == 0)
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    //ViewState["dtCreditos"] = dtCreditos;
                    double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                    if (creditos2 == 0)
                    {
                        return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }

                    if (creditos2 < dCostCred)
                    {
                        return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }
                }
                else
                {
                    if (creditos < dCostCred)
                    {
                        return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }
                }

            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                //ViewState["dtCreditos"] = dtCreditos;
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                css = new clsConfiguracionCreditos();
                double dCostCred = css.fnRecuperaPrecioServicio(nServicio);

                if (creditos == 0)
                {
                    return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                }

                if (creditos < dCostCred)
                {
                    return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                }
            }

            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso4 - " + "Recupera Tipo Documento.");
                pnId_tipo_documento = clsTimbradoFuncionalidad.fnBuscarTipoDocumento(psTipoDocumento);

            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El nombre de documento no corresponde a ningúno del sistema", pnId_Usuario);
                //return "406"; //El nombre de documento no corresponde a ningúno del sistema
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("406", "Consulta"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "406", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("406", "Consulta");
            }

            //Recupera datos del emisor.
            clsValCertificado vValidadorCertificado = null;
            XmlDocument xDocTimbrado = new XmlDocument();
            XmlDocument docNodoTimbre;
            gNodoTimbre = new TimbreFiscalDigital();
            string sRFCEmisor = string.Empty;
            string sSello = string.Empty;
            string sNoCertificado = string.Empty;
            string sRFCReceptor = string.Empty;

            DateTime dFechaComprobante;
            byte[] bLlave;
            byte[] bCertificado;
            string sPassword = string.Empty;
            int retVal;
            errorCode = 0;
            DataTable tblFechas = new DataTable();
            string noCertificadoPAC = string.Empty;
            string selloPAC = string.Empty;

            //clsPistasAuditoria.fnGenerarPistasAuditoria(pnId_Usuario, DateTime.Now, "wsRecepcionDocumentos" + "|" + "fnEnviarXML" + "|" + "Recepción de un comprobante" + DateTime.Now.ToString());

            //Verificamos que el XML cumpla con el esquema de SAT
            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso5 - " + "Verifica el esquema.");
                errorCode = fnValidate(psComprobante, sesquema); //"esquema_v3");

                if (errorCode != 0)
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("333", "Consulta"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "333", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return clsComun.fnRecuperaErrorSAT("333", "Consulta");
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El XML no cumple con el esquema de hacienda", pnId_Usuario);
                //return "333";
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("333", "Consulta"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "333", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("333", "Consulta");
            }

            //Recuperamos el certificado a partir del XML del comprobante
            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso6 - " + "Recupera Certificado.");
                vValidadorCertificado = fnRecuperarCertificado(psComprobante);

                if (vValidadorCertificado == null)
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("570", "Timbrado"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "570", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return clsComun.fnRecuperaErrorSAT("570", "Timbrado");
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudó recuperar el certificado del comprobante", pnId_Usuario);
                // return "570";
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("570", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "570", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("570", "Timbrado");
            }

            //Verificamos que el certificado del comprobante se de tipo CSD
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso7 - " + "Verifica certificado sea CSD.");
            if (!vValidadorCertificado.fnEsCSD())
            {
                clsErrorLog.fnNuevaEntrada(new Exception("El certificado no es de tipo CSD"), clsErrorLog.TipoErroresLog.Datos, "El certificado no es de tipo CSD", pnId_Usuario);
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("306", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "306", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("306", "Timbrado y Cancelación"); //El certificado no es de tipo CSD
            }

            //Certificado no expedido por el SAT
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso8 - " + "Verifica certificado no apocrifo");
            if (!vValidadorCertificado.fnCSD308())
            {
                clsErrorLog.fnNuevaEntrada(new Exception("Certificado no expedido por el SAT"), clsErrorLog.TipoErroresLog.Datos, "Certificado no expedido por el SAT", pnId_Usuario);
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("308", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "308", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("308", "Timbrado y Cancelación"); //El certificado no es de tipo CSD
            }


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try
            {

                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso9 - " + "Recupera datos de XML");
                xDocTimbrado.LoadXml(psComprobante);
                sRFCEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
                dFechaComprobante = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;
                sSello = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value;
                sNoCertificado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value;
                sRFCReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;



            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "Faltan datos del comprobante", pnId_Usuario);
                //return "799";//Faltan datos del comprobante
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("N - 504", "Recepción"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "N - 504", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("N - 504", "Recepción");

            }

            //Verificar que no contenga adenda el comprobante.
            try
            {
                if (xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante) != null)
                {
                    if (xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante).LocalName != string.Empty)
                    {
                        return "510 - No esta permitido enviar adendas en el comprobante.";
                    }
                }

            }
            catch
            {
                return "510 - No esta permitido enviar adendas en el comprobante.";
            }

            try
            {
                //Validar el RFC del receptor 18/06/2012 cambios en la recepcion del sat.
                if (!clsComun.fnValidaExpresion(sRFCReceptor, @"[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                {
                    return "509 - Verifique el RFC del receptor.";
                }

            }
            catch (Exception)
            {
                return "509 - Verifique el RFC del receptor.";
            }

            //verificamos si el comprobante ya contiene un nodo timbre
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso10 - " + "Revisar Timbre Previo.");
            XmlNode aux = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante);

            if (aux != null)
            {
                // return "472"; //El comprobante ya está timbrado
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("307", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "307", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("307", "Timbrado y Cancelación");
            }

            //Generar la cadena del emisor con los conceptos del XML
            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso11 - " + "Crea cadena de original emisor.");
                //gTimbrado = new clsOperacionTimbradoSellado();
                //sCadenaOriginalEmisor = gTimbrado.fnConstruirCadenaTimbrado(xDocTimbrado.CreateNavigator(), scadena);

                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                switch (sVersion)
                {
                    case "3.0":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(CaOri.V30));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(xDocTimbrado.CreateNavigator(), args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCadenaOriginalEmisor = srDll.ReadToEnd();


                        break;
                    case "3.2":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(CaOri.V32));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(xDocTimbrado.CreateNavigator(), args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCadenaOriginalEmisor = srDll.ReadToEnd();

                        break;
                }

            }
            catch (Exception)
            {
                return clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación");
            }

            try
            {
                //303------Preparamos los objetos de manejo tanto de la llave como del certificado---303
                //gTimbrado = new clsOperacionTimbradoSellado();
                //Validamos el sello de emisor
                //string sCOEmisor = gTimbrado.fnConstruirCadenaTimbrado(xDocTimbrado.CreateNavigator(), scadena);//"cadenaoriginal_3_0");
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso12 - " + "Revisar que corresponda la candena del emisor.");
                if (string.IsNullOrEmpty(sSello) || !(sCadenaOriginalEmisor.Contains(sRFCEmisor)))
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "303", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación");
                }
            }
            catch (Exception)
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "303", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación");

            }

            //Certificado revocado o caduco R o C
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso13 - " + "Revisa caducidad del certificado.");
            if (!fnVerificarCaducidadCertificado(sNoCertificado, "R", "C", ref vValidadorCertificado))
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("304", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "304", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("304", "Timbrado y Cancelación");
            }

            try
            {
                //La fecha de emisión no esta dentro de la vigencia del CSD del Emisor
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso14 - " + "Revisar vigencia del certificado.");
                if (!vValidadorCertificado.ComprobarFechas() || !fnRecuperaFechaLCO(sNoCertificado, "A", ref vValidadorCertificado))
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "305", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
                }
            }
            catch (Exception)
            {
                return clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
            }

            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso15 - " + "Revisar Fecha del comprobante no sea menor a 2011.");
            if (!clsComun.fnRevisarFechaNoPosterior(dFechaComprobante))
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("403", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "403", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("403", "Timbrado");
            }

            //RFC del emisor no se encuentra en el régimen de contribuyentes 402
            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso16 - " + "Revisar en el LCO.");
                string rfcComprobante = vValidadorCertificado.VerificarExistenciaCertificado();
                if (sRFCEmisor != rfcComprobante)
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("402", "Timbrado"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "402", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return clsComun.fnRecuperaErrorSAT("402", "Timbrado");
                }
            }
            catch
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("402", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "402", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("402", "Timbrado");
            }

            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso17 - " + "Revisar fecha dentro de 72 horas.");
            if (!vValidadorCertificado.fnFechaContraPeriodoValidez(dFechaComprobante))
            {
                // return "504"; //La fecha del comprobante esta fuera de periodo
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("401", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "401", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("401", "Timbrado");
            }

            //Revisamos que esl sello del emisor sea valido.
            try
            {
                //Preparamos los objetos de manejo tanto de la llave como del certificado
                gTimbrado = new clsOperacionTimbradoSellado();
                // *************Comentado por limitar rendimiento al HSM*************************************************
                //Validamos el sello de emisor
                //string sCadenaOriginalEmisor = gTimbrado.fnConstruirCadenaTimbrado(xDocTimbrado.CreateNavigator(), scadena);//"cadenaoriginal_3_0");
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso18 - " + "Revisar sello del emisor.");
                if (!vValidadorCertificado.fnVerificarSello(sCadenaOriginalEmisor, sSello))
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "302", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación");
                }
                // *************Comentado por limitar rendimiento al HSM*************************************************
            }
            catch (Exception)
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "302", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación");
            }


            try
            {
                //Preparamos los objetos de manejo tanto de la llave como del certificado
                //clsOperacionTimbradoSellado gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                //clsValCertificado gCertificado = new clsValCertificado(bCertificado);
                //clsOperacionTimbradoSellado gTimbrado = new clsOperacionTimbradoSellado();

                // *************Comentado por limitar rendimiento al HSM*************************************************
                //Validamos el sello de emisor
                //string sCadenaOriginalEmisor = gTimbrado.fnConstruirCadenaTimbrado(xDocTimbrado.CreateNavigator(), "cadenaoriginal_3_0");
                //if (!vValidadorCertificado.fnVerificarSello(sCadenaOriginalEmisor, sSello))
                //    return "511"; //El sello no corresponde a los datos del comprobante
                // *************Comentado por limitar rendimiento al HSM*************************************************


                //Llenamos los datos del nodo timbre
                gNodoTimbre.UUID = Guid.NewGuid().ToString();
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime(DateTime.Now.ToString("s"));
                gNodoTimbre.selloCFD = sSello;

                //********Obtienen el numero del sertificado del HSM********************

                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso19 - " + "Usando el HSM.");
                if (clsComun.ObtenerParamentro("TipoTimbrado") == "HSM")
                {

                    //Servicio HSM3
                    wslServicioPAC gServicio = new wslServicioPAC();

                    try
                    {
                        gHSM = new clsHSMComunicacion();
                        gTimbrado = new clsOperacionTimbradoSellado();
                        noCertificadoPAC = gHSM.fnObtenerNumeroCertificado(gHSM.fnHSMLogin());
                        gNodoTimbre.noCertificadoSAT = noCertificadoPAC;
                        gHSM.fnHSMLogOut();

                        //Si no se Recupero el Certificado de HSM1 o HSM2
                        if (gNodoTimbre.noCertificadoSAT == string.Empty)
                        {
                            bLlave = gServicio.HSM3_KEY();
                            bCertificado = gServicio.HSM3_CER();
                            sPassword = gServicio.HSM3_PAS();

                            gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                            gCertificado = new clsValCertificado(bCertificado);
                            noCertificadoPAC = gCertificado.ObtenerNoCertificado();
                            gNodoTimbre.noCertificadoSAT = noCertificadoPAC;
                        }
                    }
                    catch (Exception)
                    {
                        //Si falla HSM1 o HSM2
                        bLlave = gServicio.HSM3_KEY();
                        bCertificado = gServicio.HSM3_CER();
                        sPassword = gServicio.HSM3_PAS();

                        gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                        gCertificado = new clsValCertificado(bCertificado);
                        noCertificadoPAC = gCertificado.ObtenerNoCertificado();
                        gNodoTimbre.noCertificadoSAT = noCertificadoPAC;
                    }
                }
                else
                {

                    try
                    {
                        ////Preparamos los objetos de manejo tanto de la llave como del certificado
                        wslServicioPAC gServicio = new wslServicioPAC();
                        bLlave = gServicio.ObtenerLlavePAC();
                        bCertificado = gServicio.ObtenerCertificado();
                        sPassword = gServicio.ObtenerPassword();

                        gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                        gCertificado = new clsValCertificado(bCertificado);
                        noCertificadoPAC = gCertificado.ObtenerNoCertificado();
                        gNodoTimbre.noCertificadoSAT = noCertificadoPAC;

                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso20 - " + "Recueprando informacion.");

                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudo recuperar la llave y el certificado del PAC", pnId_Usuario);
                        //return "622";//No se pudo recuperar la llave y el certificado del PAC
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("622", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "622", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return clsComun.fnRecuperaErrorSAT("622", "Timbrado y Cancelación");
                    }

                }

                //********Obtienen el numero del sertificado del HSM********************

                //Generamos el primer XML necesario para generar la cadena original
                docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                //Generamos la cadena original
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso21 - " + "Genera cadena origianl de TFD.");
                XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();
                //sCadenaOriginal = gTimbrado.fnConstruirCadenaTimbrado(navNodoTimbre, "cadenaoriginal_TFD_1_0");

                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                // Load the type of the class.
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(Timbrado.V3.TFDXSLT));

                ms = new MemoryStream();
                args = new XsltArgumentList();

                xslt.Transform(navNodoTimbre, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sCadenaOriginal = srDll.ReadToEnd();


                //Generamos el sello del SAT, se lo agregamos al objeto y generamos el XML final
                //***********Genera la firma con el HSM*************************************
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso22 - " + "Genera sello SAT.");
                if (clsComun.ObtenerParamentro("TipoTimbrado") == "HSM")
                {

                    try
                    {
                        selloPAC = gHSM.fnFirmar(sCadenaOriginal, gHSM.fnHSMLogin());
                        gNodoTimbre.selloSAT = selloPAC;
                        gHSM.fnHSMLogOut();

                        if (gNodoTimbre.selloSAT == string.Empty)
                        {
                            selloPAC = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                            gNodoTimbre.selloSAT = selloPAC;
                        }
                    }
                    catch (Exception)
                    {
                        selloPAC = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                        gNodoTimbre.selloSAT = selloPAC;
                    }

                }
                else
                {
                    selloPAC = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                    gNodoTimbre.selloSAT = selloPAC;
                    //gNodoTimbre.selloSAT = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("sello prueba SAT")); 
                }

                //Validamos
                if (!vValidadorCertificado.fnVerificarSelloPAC(sCadenaOriginal, selloPAC, noCertificadoPAC))
                {
                    return "320 - Verifique el comprobante, reintente de nuevo.";
                }

                //xDocTimbrado = gTimbrado.fnGenerarXML(gNodoTimbre);// Reescribe el documento. No Borrar.

                //***********Genera la firma con el HSM*************************************
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso23 - " + "Cargando timbre al documento.");
                docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                XmlNode Complemento = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsmComprobante);
                if (Complemento == null)
                    Complemento = xDocTimbrado.CreateElement("cfdi", "Complemento", nsmComprobante.LookupNamespace("cfdi"));

                Complemento.InnerXml = docNodoTimbre.DocumentElement.OuterXml + Complemento.InnerXml;

                xDocTimbrado.DocumentElement.AppendChild(Complemento);



                //Eliminamos la llave y el certificado
                gTimbrado.fnDestruirLlave();
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudo generar el sello del PAC", pnId_Usuario);
                //return "817"; //No se pudo generar el sello del PAC
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("817", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "817", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return clsComun.fnRecuperaErrorSAT("817", "Timbrado");
            }

            string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        //Buscar el HASH del comprobante de la cadena original del emisor.
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso24 - " + "Busca el HAS del emisor.");

                        string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginalEmisor).ToUpper();
                        string Comprobante = clsTimbradoFuncionalidad.fnBuscarHashCompXML(datosUsuario.id_usuario, HASHEmisor, "Emisor");
                        if (Comprobante != "0")
                        {
                            XmlDocument hasComprobante = new XmlDocument();
                            hasComprobante.LoadXml(Comprobante);
                            //return "98";
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            //return clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                            return hasComprobante.OuterXml;
                        }

                        //string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginalEmisor).ToUpper();
                        //if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHEmisor, "Emisor"))
                        //{
                        //    //return "98";
                        //    //*******************************************************Insertar Response en tabla de acuses
                        //    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                        //    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                        //    //*******************************************************Insertar Response en tabla de acuses
                        //    return clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                        //}

                        //Buscar el HASH del comprobante de la cadena original del timbre fiscal.
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso25 - " + "Busca el HAS del TFD.");
                        string HASHTimbreFiscal = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();
                        if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbreFiscal, "Timbre"))
                        {
                            //return "98";
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                        }


                        SqlCommand cmd = new SqlCommand("usp_Timbrado_InsertaComprobanteAll_Ins", con);
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("sXML", xDocTimbrado.DocumentElement.OuterXml);
                        cmd.Parameters.AddWithValue("nId_tipo_documento", pnId_tipo_documento);
                        cmd.Parameters.AddWithValue("cEstatus", "P");
                        cmd.Parameters.AddWithValue("dFecha_Documento", DateTime.Now);
                        cmd.Parameters.AddWithValue("nId_estructura", pnId_Estructura);
                        cmd.Parameters.AddWithValue("nId_usuario_timbrado", pnId_Usuario);
                        cmd.Parameters.AddWithValue("nSerie", "N/A");
                        cmd.Parameters.AddWithValue("sOrigen", "R");
                        cmd.Parameters.AddWithValue("sHash", HASHTimbreFiscal);
                        cmd.Parameters.AddWithValue("sDatos", HASHEmisor);


                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso26 - " + "Inserta Comprobante.");
                        retVal = Convert.ToInt32(cmd.ExecuteScalar());
                        fnActualizaCreditos(dtCreditos, sServicio);

                        tran.Commit();



                        if (retVal == 0)
                        {
                            //return "999";
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
                        }
                        else
                        {
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso27 - " + "Recupera XML para regresarlo al cliente.");
                            sXmlDocument = xDocTimbrado.OuterXml;

                            //Enviar comprobante al SAT --Comentado para mejorar el rendimento de entrega de comprobante.
                            //clsEnvioSAT enviarSAT = new clsEnvioSAT();
                            //sRetornoSAT = enviarSAT.fnEnviarBloqueCfdi(HASH, pnId_Usuario, xDocTimbrado, retVal, datosUsuario);




                            //*******************************************************Insertar Response en tabla de acuses
                            string uuid = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, uuid.ToUpper(), sXmlDocument, DateTime.Now, "0", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                        }



                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante", pnId_Usuario);
                        //return "999"; //Error durante el registro del comprobante
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
                    }
                }

            }


        }
        else
        {
            //*******************************************************Insertar Response en tabla de acuses
            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sRetAutentication, "Response", "E", string.Empty);
            //*******************************************************Insertar Response en tabla de acuses
            sXmlDocument = clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
        }


        return sXmlDocument;
    }

    //*****************************************Metodo General de Seguros******************************************************

    /*  Códigos de errors
  * 
   '''  0   - Sin errores
   ''' 100 - El archivo de texto esta mal formado
   ''' 177 - El certificado no se encuentra en la lista de régimen fiscal
   ''' 179 - El RFC del Certificado no corresponde al del comprobante
   ''' 200 - El certificado está fuera de su periodo de validez
   ''' 333 - El XML no cumple con el esquema de hacienda
   ''' 406 - El nombre de documento no corresponde a ningúno del sistema
   ''' 472 - El comprobante ya está timbrado
   ''' 504 - La fecha del comprobante esta fuera de periodo
   ''' 511 - El sello no corresponde a los datos del comprobante
   ''' 570 - No se pudó recuperar el certificado del comprobante
   ''' 592 - El certificado no es de tipo CSD
   ''' 622 - El servicio no esta disponible -- NOTA: este error es para el público, la causa real es: No se pudo recuperar la llave y el certificado del PAC
   ''' 645 - El comprobante no contiene un sello de emisor
   ''' 799 - Faltan datos del comprobante
   ''' 817 - No se pudo generar el sello del PAC
   ''' 999 - Error durante el registro del comprobante
   '''  90  - Los datos del usuario son requeridos.
   '''  91  - El usuario esta en estado pendiente.
   '''  92  - El usuario esta en estado bloqueado.
   '''  93  - El usuario esta en estado expirado.
   '''  94  - El usuario esta en estado por cambiar contraseña.
   '''  95  - Usuario inexistente.
   '''  96  - Usuario o contraseña incorrecta.
 */

    public object[] fnEnviarTXTGeneralSeguros(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion, string sOrigen)
    {
        System.IO.StringReader lector = new System.IO.StringReader(psComprobante);
        string linea = string.Empty;
        string[] seccion = null;
        string[] atributos = null;
        int pnId_Usuario = 0;
        object[] sXmlDocument = new object[9];
        string sRetAutentication = string.Empty;
        string sRequest = string.Empty;
        string sResponse = string.Empty;
        //Parametro para generar PDF
        bool benviaPdf = false;
        //Recupera datos del emisor.
        clsOperacionCuenta gDAL = new clsOperacionCuenta();
        clsConfiguracionCreditos css;
        SqlDataReader sdrInfo = null;
        DataTable certificado = new DataTable();
        string resValidacion = string.Empty;
        string sCoriginalEmisor = String.Empty;
        string sCoriginalTimbre = string.Empty;
        string sello = string.Empty;
        string numeroCertificado = string.Empty;

        XmlNode impuestos = null;
        XmlNode padre = null;
        XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
        XmlDocument xDocumento = new XmlDocument();

        if (sOrigen == "conGT" || sOrigen == "conGE")
        {
            string sServicio = string.Empty;
            switch (sOrigen)
            {
                case "conGT": //Generación y Timbrado
                    sServicio = "GT";
                    break;
                case "conGE": //Generación + Timbrado
                    sServicio = "GE";
                    break;
            }
            sXmlDocument = fnEnviarXMLGeneralSeguros(psComprobante, psTipoDocumento, pnId_Estructura, sNombre, sContraseña, sVersion, sServicio, false);
            return sXmlDocument;
        }

        if (sOrigen == "conT" || sOrigen == "wsdlT")
        {
            sXmlDocument[0] = "Este servicio es para Generación y timbrado.";
            return sXmlDocument;
        }

        if (sVersion == "3.0")
        {
            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xDocumento = new XmlDocument(nsm.NameTable);
            xDocumento.CreateXmlDeclaration("1.0", "UTF-8", "no");
            xDocumento.AppendChild(xDocumento.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));
        }

        if (sVersion == "3.2")
        {
            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xDocumento = new XmlDocument(nsm.NameTable);
            xDocumento.CreateXmlDeclaration("1.0", "UTF-8", "no");
            xDocumento.AppendChild(xDocumento.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));
        }


        sRetAutentication = fnAutentication(sNombre, sContraseña, ref pnId_Usuario, ref datosUsuario);

        //*******************************************************Insertar Request en tabla de acuses
        sRequest = clsComun.fnRequestRecepcion(psComprobante, psTipoDocumento, pnId_Estructura.ToString(), sNombre, sContraseña);
        //clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sRequest, DateTime.Now, "0", "Request", "E", string.Empty);
        //*******************************************************Insertar Request en tabla de acuses


        string[] est = sRetAutentication.Split('-');
        if (est.Length > 2)
        {
            sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
        }
        else
        {
            sRetAutentication = est[0].Trim();
        }

        if (sRetAutentication == "0")
        {

            if (pnId_Estructura == 0)
                pnId_Estructura = Convert.ToInt32(fnRecuperaMatriz(pnId_Usuario));

            //Revisar los creditos disponibles.
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso3 - " + "Revisa Creditos Disponibles.");
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');


            if (dtCreditos.Rows.Count > 0)
            {
                css = new clsConfiguracionCreditos();
                double dCostCred = css.fnRecuperaPrecioServicio(1);
                double creditos = 0;
                creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                if (creditos == 0)
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    //ViewState["dtCreditos"] = dtCreditos;
                    double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                    if (creditos2 == 0)
                    {
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                        return sXmlDocument;
                    }

                    if (creditos2 < dCostCred)
                    {
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                        return sXmlDocument;
                    }
                }
                else
                {
                    if (creditos < dCostCred)
                    {
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                        return sXmlDocument;
                    }
                }

            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                //ViewState["dtCreditos"] = dtCreditos;
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                css = new clsConfiguracionCreditos();
                double dCostCred = css.fnRecuperaPrecioServicio(4);

                if (creditos == 0)
                {
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    return sXmlDocument;
                }

                if (creditos < dCostCred)
                {
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    return sXmlDocument;
                }
            }

            if (sVersion == "3.0")
            {
                while (true)
                {
                    linea = lector.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                        break;

                    seccion = linea.Split('?');

                    try
                    {
                        atributos = seccion[1].Split('|');

                        if (seccion[0] == "im" || seccion[0] == "it" || seccion[0] == "ir")
                        {
                            if (impuestos == null)
                            {
                                if (seccion[0] == "im")
                                    impuestos = xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Impuestos", atributos));
                                else
                                    impuestos = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3"));

                            }
                        }

                        switch (seccion[0])
                        {
                            case "co":
                                fnCrearElementoRoot(xDocumento, atributos);
                                break;
                            case "re":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Emisor", atributos));
                                break;
                            case "de":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "DomicilioFiscal", atributos));
                                break;
                            case "ee":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "ExpedidoEn", atributos));
                                break;
                            case "rr":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Receptor", atributos));
                                break;
                            case "dr":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Domicilio", atributos));
                                break;
                            case "cc":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);
                                if (padre == null)
                                    padre = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));
                                break;
                            case "ir":
                                padre = impuestos.SelectSingleNode("cfdi:Retenciones", nsm);
                                if (padre == null)
                                    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Retencion", atributos));
                                break;
                            case "it":
                                padre = impuestos.SelectSingleNode("cfdi:Traslados", nsm);
                                if (padre == null)
                                    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Traslado", atributos));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El archivo de texto esta mal formado", pnId_Usuario);
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación");
                        return sXmlDocument; //El archivo de texto esta mal formado
                    }
                }

            }

            if (sVersion == "3.2")
            {
                while (true)
                {
                    linea = lector.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                        break;

                    seccion = linea.Split('?');

                    try
                    {
                        atributos = seccion[1].Split('|');

                        if (seccion[0] == "im" || seccion[0] == "it" || seccion[0] == "ir")
                        {
                            if (impuestos == null)
                            {
                                if (seccion[0] == "im")
                                    impuestos = xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Impuestos", atributos));
                                else
                                    impuestos = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3"));

                            }
                        }

                        switch (seccion[0])
                        {
                            case "co":
                                fnCrearElementoRoot32(xDocumento, atributos);
                                break;
                            case "re":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Emisor", atributos));
                                break;
                            case "de":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "DomicilioFiscal", atributos));
                                break;
                            case "ee":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "ExpedidoEn", atributos));
                                break;
                            case "rf":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "RegimenFiscal", atributos));
                                break;
                            case "rr":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Receptor", atributos));
                                break;
                            case "dr":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Domicilio", atributos));
                                break;
                            case "cc":
                                padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);
                                if (padre == null)
                                    padre = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));
                                break;
                            case "ir":
                                padre = impuestos.SelectSingleNode("cfdi:Retenciones", nsm);
                                if (padre == null)
                                    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Retencion", atributos));
                                break;
                            case "it":
                                padre = impuestos.SelectSingleNode("cfdi:Traslados", nsm);
                                if (padre == null)
                                    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3"));
                                padre.AppendChild(fnCrearElemento(xDocumento, "Traslado", atributos));
                                break;
                        }

                        if (seccion[0] == "ad")
                        {
                            foreach (string a in atributos)
                            {
                                string[] valores = a.Split('@');
                                if (valores[0] == "enviarPDF")
                                    benviaPdf = Convert.ToBoolean(valores[1]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El archivo de texto esta mal formado", pnId_Usuario);
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación");
                        return sXmlDocument; //El archivo de texto esta mal formado
                    }
                }

            }


            try
            {
                ////Según version se obtiene datos fiscales

                sdrInfo = gDAL.fnObtenerDatosFiscalesSuc(pnId_Estructura);

                if (sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read())
                {
                    //Obtener datos del emisor, llave, certificado, y password
                    certificado = clsTimbradoFuncionalidad.ObtenerCertificado(Convert.ToInt32(sdrInfo["id_rfc"].ToString()));
                }


                byte[] bLlave = (byte[])certificado.Rows[0]["key"];
                byte[] bCertificado = (byte[])certificado.Rows[0]["certificado"];
                string sPassword = certificado.Rows[0]["password"].ToString();


                ////Preparamos los objetos de manejo tanto de la llave como del certificado
                gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                gCertificado = new clsValCertificado(bCertificado);


                X509Certificate2 certEmisor = new X509Certificate2();
                certEmisor.Import(Utilerias.Encriptacion.DES3.Desencriptar(bCertificado));

                //Cerificado para agregar al XML
                string sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());

                //Numero del certificado
                numeroCertificado = gCertificado.ObtenerNoCertificado();



                ////Generamos la cadena original
                XPathNavigator navNodoTimbre = xDocumento.CreateNavigator();
                //sCoriginalEmisor = gTimbrado.fnConstruirCadenaTimbrado(navNodoTimbre, scadena); //"cadenaoriginal_3_2"); 

                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                switch (sVersion)
                {
                    case "3.0":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(CaOri.V30));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(navNodoTimbre, args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCoriginalEmisor = srDll.ReadToEnd();


                        break;
                    case "3.2":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(CaOri.V32));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(navNodoTimbre, args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCoriginalEmisor = srDll.ReadToEnd();

                        break;
                }


                //Genera sello de la cadena original
                clsNodoTimbre creadorTimbre = new clsNodoTimbre();
                switch (sVersion)
                {
                    case "3.0":
                        sello = gTimbrado.fnGenerarSello(sCoriginalEmisor, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1, true);
                        break;
                    case "3.2":
                        sello = gTimbrado.fnGenerarSello(sCoriginalEmisor, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1, true);
                        break;
                }

                //Asignar los valores de certificado,numero de certificado y sello.
                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocumento.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).SetValue(numeroCertificado);
                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);
                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sello);



            }
            catch (Exception)
            {
                sXmlDocument[0] = "No se pudo generar el sello del emisor, revise la versión solicitada.";
                return sXmlDocument;
            }


            lector.Close();
            //Mandamos el XML a revision y firmado
            //fnActualizaCreditos(dtCreditos);
            sXmlDocument = fnEnviarXMLGeneralSeguros(xDocumento.DocumentElement.OuterXml, psTipoDocumento, pnId_Estructura, sNombre, sContraseña, sVersion, "GT", benviaPdf);

        }
        else
        {
            //*******************************************************Insertar Response en tabla de acuses
            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sRetAutentication, "Response", "E", string.Empty);
            //*******************************************************Insertar Response en tabla de acuses
            sXmlDocument[0] = clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
        }

        return sXmlDocument;
    }

    public object[] fnEnviarXMLGeneralSeguros(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion, string sServicio, bool benviaPdf)
    {

        int pnId_Usuario = 0;
        object[] sXmlDocument = new object[9];
        string sRetAutentication = string.Empty;
        string sCadenaOriginal = string.Empty;
        string sRetornoSAT = string.Empty;
        string sRequest = string.Empty;
        string sResponse = string.Empty;
        string sCadenaOriginalEmisor = string.Empty;
        string scadena = string.Empty;
        string sesquema = string.Empty;
        clsConfiguracionCreditos css;

        switch (sVersion)
        {
            case "3.0":
                //scadena = "cadenaoriginal_3_0";
                sesquema = "esquema_v3";
                break;
            case "3.2":
                //scadena = "cadenaoriginal_3_2";
                sesquema = "esquema_v3_2";
                break;
        }

        //Revisar version
        if (sVersion == "3.0")
        {
            sXmlDocument[0] = "999 - Por disposición oficial estos comprobantes ya no se timbran.";
            return sXmlDocument;
        }

        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(wcfRecepcionASMX.AcceptAllCertificatePolicy);

        //Verifica el usuario.

        sRetAutentication = fnAutentication(sNombre, sContraseña, ref pnId_Usuario, ref datosUsuario);
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso1 - " + "Verifica el usuario.");

        try
        {
            //*******************************************************Insertar Request en tabla de acuses
            sRequest = clsComun.fnRequestRecepcion(psComprobante, psTipoDocumento, pnId_Estructura.ToString(), sNombre, sContraseña);
            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sRequest, DateTime.Now, "0", "Request", "E", string.Empty);
            //*******************************************************Insertar Request en tabla de acuses
        }
        catch
        {
            //*******************************************************Insertar Response en tabla de acuses
            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
            //*******************************************************Insertar Response en tabla de acuses
            sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación");
            return sXmlDocument; //El archivo de texto esta mal formado
        }



        string[] est = sRetAutentication.Split('-');
        if (est.Length > 2)
        {
            sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
        }
        else
        {
            sRetAutentication = est[0].Trim();
        }
        if (sRetAutentication == "0")
        {
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso2 - " + "Recupera Estructura.");
            if (pnId_Estructura == 0)
                pnId_Estructura = Convert.ToInt32(fnRecuperaMatriz(pnId_Usuario));

            int pnId_tipo_documento;

            //Revisar los creditos disponibles.
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso3 - " + "Revisa Creditos Disponibles.");
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');

            int nServicio = 0;
            switch (sServicio)
            {
                case "GT": //Generación y Timbrado
                    nServicio = 4;
                    break;
                case "GE": //Generación + Envio
                    nServicio = 10;
                    break;
            }
            if (dtCreditos.Rows.Count > 0)
            {
                css = new clsConfiguracionCreditos();

                double dCostCred = css.fnRecuperaPrecioServicio(nServicio);
                double creditos = 0;
                creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                if (creditos == 0)
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    //ViewState["dtCreditos"] = dtCreditos;
                    double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                    if (creditos2 == 0)
                    {
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                        return sXmlDocument;
                    }

                    if (creditos2 < dCostCred)
                    {
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                        return sXmlDocument;
                    }
                }
                else
                {
                    if (creditos < dCostCred)
                    {
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                        return sXmlDocument;
                    }
                }

            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                //ViewState["dtCreditos"] = dtCreditos;
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                css = new clsConfiguracionCreditos();
                double dCostCred = css.fnRecuperaPrecioServicio(nServicio);

                if (creditos == 0)
                {
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    return sXmlDocument;
                }

                if (creditos < dCostCred)
                {
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    return sXmlDocument;
                }
            }

            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso4 - " + "Recupera Tipo Documento.");
                pnId_tipo_documento = clsTimbradoFuncionalidad.fnBuscarTipoDocumento(psTipoDocumento);

            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El nombre de documento no corresponde a ningúno del sistema", pnId_Usuario);
                //return "406"; //El nombre de documento no corresponde a ningúno del sistema
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("406", "Consulta"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "406", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("406", "Consulta");
                return sXmlDocument;
            }

            //Recupera datos del emisor.
            clsValCertificado vValidadorCertificado = null;
            XmlDocument xDocTimbrado = new XmlDocument();
            XmlDocument docNodoTimbre;
            gNodoTimbre = new TimbreFiscalDigital();
            string sRFCEmisor = string.Empty;
            string sSello = string.Empty;
            string sNoCertificado = string.Empty;
            string sRFCReceptor = string.Empty;

            DateTime dFechaComprobante;
            byte[] bLlave;
            byte[] bCertificado;
            string sPassword = string.Empty;
            int retVal;
            errorCode = 0;
            DataTable tblFechas = new DataTable();
            string noCertificadoPAC = string.Empty;
            string selloPAC = string.Empty;


            //Verificamos que el XML cumpla con el esquema de SAT
            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso5 - " + "Verifica el esquema.");
                errorCode = fnValidate(psComprobante, sesquema);//"esquema_v3");
                if (errorCode != 0)
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("333", "Consulta"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "333", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("333", "Consulta");
                    return sXmlDocument;
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El XML no cumple con el esquema de hacienda", pnId_Usuario);
                //return "333";
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("333", "Consulta"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "333", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("333", "Consulta");
                return sXmlDocument;
            }

            //Recuperamos el certificado a partir del XML del comprobante
            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso6 - " + "Recupera Certificado.");
                vValidadorCertificado = fnRecuperarCertificado(psComprobante);

                if (vValidadorCertificado == null)
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("570", "Timbrado"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "570", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("570", "Timbrado");
                    return sXmlDocument;
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudó recuperar el certificado del comprobante", pnId_Usuario);
                // return "570";
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("570", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "570", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("570", "Timbrado");
                return sXmlDocument;
            }

            //Verificamos que el certificado del comprobante se de tipo CSD
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso7 - " + "Verifica certificado sea CSD.");
            if (!vValidadorCertificado.fnEsCSD())
            {
                clsErrorLog.fnNuevaEntrada(new Exception("El certificado no es de tipo CSD"), clsErrorLog.TipoErroresLog.Datos, "El certificado no es de tipo CSD", pnId_Usuario);
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("306", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "306", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("306", "Timbrado y Cancelación");
                return sXmlDocument; //El certificado no es de tipo CSD
            }

            //Certificado no expedido por el SAT
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso8 - " + "Verifica certificado no apocrifo");
            if (!vValidadorCertificado.fnCSD308())
            {
                clsErrorLog.fnNuevaEntrada(new Exception("Certificado no expedido por el SAT"), clsErrorLog.TipoErroresLog.Datos, "Certificado no expedido por el SAT", pnId_Usuario);
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("308", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "308", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("308", "Timbrado y Cancelación");
                return sXmlDocument; //El certificado no es de tipo CSD
            }


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso9 - " + "Recupera datos de XML");
                xDocTimbrado.LoadXml(psComprobante);
                sRFCEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
                dFechaComprobante = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;
                sSello = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value;
                sNoCertificado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value;
                sRFCReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;


            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "Faltan datos del comprobante", pnId_Usuario);
                //return "799";//Faltan datos del comprobante
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("N - 504", "Recepción"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "N - 504", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("N - 504", "Recepción");
                return sXmlDocument;

            }

            //Verificar que no contenga adenda el comprobante.
            try
            {
                if (xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante) != null)
                {
                    if (xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante).LocalName != string.Empty)
                    {
                        sXmlDocument[0] = "510 - No esta permitido enviar adendas en el comprobante.";
                        return sXmlDocument;
                    }
                }

            }
            catch
            {
                sXmlDocument[0] = "510 - No esta permitido enviar adendas en el comprobante.";
                return sXmlDocument;
            }

            try
            {
                //Validar el RFC del receptor 18/06/2012 cambios en la recepcion del sat.
                if (!clsComun.fnValidaExpresion(sRFCReceptor, @"[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                {
                    sXmlDocument[0] = "509 - Verifique el RFC del receptor.";
                    return sXmlDocument;
                }

            }
            catch (Exception)
            {
                sXmlDocument[0] = "509 - Verifique el RFC del receptor.";
                return sXmlDocument;
            }

            //verificamos si el comprobante ya contiene un nodo timbre
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso10 - " + "Revisar Timbre Previo.");
            XmlNode aux = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante);

            if (aux != null)
            {
                // return "472"; //El comprobante ya está timbrado
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("307", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "307", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("307", "Timbrado y Cancelación");
                return sXmlDocument;
            }

            //Generar la cadena del emisor con los conceptos del XML
            try
            {
                //gTimbrado = new clsOperacionTimbradoSellado();
                ////clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso11 - " + "Crea cadena de original emisor.");
                //sCadenaOriginalEmisor = gTimbrado.fnConstruirCadenaTimbrado(xDocTimbrado.CreateNavigator(), scadena);

                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                switch (sVersion)
                {
                    case "3.0":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(CaOri.V30));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(xDocTimbrado.CreateNavigator(), args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCadenaOriginalEmisor = srDll.ReadToEnd();


                        break;
                    case "3.2":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(CaOri.V32));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(xDocTimbrado.CreateNavigator(), args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCadenaOriginalEmisor = srDll.ReadToEnd();

                        break;
                }

            }
            catch (Exception)
            {
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación");
                return sXmlDocument;
            }

            try
            {
                //303------Preparamos los objetos de manejo tanto de la llave como del certificado---303
                //gTimbrado = new clsOperacionTimbradoSellado();

                //Validamos el sello de emisor
                //string sCOEmisor = gTimbrado.fnConstruirCadenaTimbrado(xDocTimbrado.CreateNavigator(), scadena);
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso12 - " + "Revisar que corresponda la candena del emisor.");
                if (string.IsNullOrEmpty(sSello) || !(sCadenaOriginalEmisor.Contains(sRFCEmisor)))
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "303", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación");
                    return sXmlDocument;
                }
            }
            catch (Exception)
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "303", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación");
                return sXmlDocument;

            }

            //Certificado revocado o caduco R o C
            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso13 - " + "Revisa caducidad del certificado.");
            if (!fnVerificarCaducidadCertificado(sNoCertificado, "R", "C", ref vValidadorCertificado))
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("304", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "304", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("304", "Timbrado y Cancelación");
                return sXmlDocument;
            }


            try
            {
                //La fecha de emisión no esta dentro de la vigencia del CSD del Emisor
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso14 - " + "Revisar vigencia del certificado.");
                if (!vValidadorCertificado.ComprobarFechas() || !fnRecuperaFechaLCO(sNoCertificado, "A", ref vValidadorCertificado))
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "305", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
                    return sXmlDocument;
                }
            }
            catch (Exception)
            {
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
                return sXmlDocument;
            }

            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso15 - " + "Revisar Fecha del comprobante no sea menor a 2011.");
            if (!clsComun.fnRevisarFechaNoPosterior(dFechaComprobante))
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("403", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "403", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("403", "Timbrado");
                return sXmlDocument;
            }

            //RFC del emisor no se encuentra en el régimen de contribuyentes 402
            try
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso16 - " + "Revisar en el LCO.");
                string rfcComprobante = vValidadorCertificado.VerificarExistenciaCertificado();
                if (sRFCEmisor != rfcComprobante)
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("402", "Timbrado"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "402", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("402", "Timbrado");
                    return sXmlDocument;
                }
            }
            catch
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("402", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "402", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("402", "Timbrado");
                return sXmlDocument;
            }

            //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso17 - " + "Revisar fecha dentro de 72 horas.");
            if (!vValidadorCertificado.fnFechaContraPeriodoValidez(dFechaComprobante))
            {
                // return "504"; //La fecha del comprobante esta fuera de periodo
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("401", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "401", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("401", "Timbrado");
                return sXmlDocument;
            }

            //Revisamos que esl sello del emisor sea valido.
            try
            {
                //Preparamos los objetos de manejo tanto de la llave como del certificado
                gTimbrado = new clsOperacionTimbradoSellado();
                // *************Comentado por limitar rendimiento al HSM*************************************************
                //Validamos el sello de emisor
                //string sCadenaOriginalEmisor = gTimbrado.fnConstruirCadenaTimbrado(xDocTimbrado.CreateNavigator(), scadena);//"cadenaoriginal_3_0");
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso18 - " + "Revisar sello del emisor.");
                if (!vValidadorCertificado.fnVerificarSello(sCadenaOriginalEmisor, sSello))
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "302", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación");
                    return sXmlDocument;
                }
                // *************Comentado por limitar rendimiento al HSM*************************************************
            }
            catch (Exception)
            {
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "302", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación");
                return sXmlDocument;
            }


            try
            {
                //Preparamos los objetos de manejo tanto de la llave como del certificado
                //clsOperacionTimbradoSellado gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                //clsValCertificado gCertificado = new clsValCertificado(bCertificado);
                //clsOperacionTimbradoSellado gTimbrado = new clsOperacionTimbradoSellado();

                // *************Comentado por limitar rendimiento al HSM*************************************************
                //Validamos el sello de emisor
                //string sCadenaOriginalEmisor = gTimbrado.fnConstruirCadenaTimbrado(xDocTimbrado.CreateNavigator(), "cadenaoriginal_3_0");
                //if (!vValidadorCertificado.fnVerificarSello(sCadenaOriginalEmisor, sSello))
                //    return "511"; //El sello no corresponde a los datos del comprobante
                // *************Comentado por limitar rendimiento al HSM*************************************************


                //Llenamos los datos del nodo timbre
                gNodoTimbre.UUID = Guid.NewGuid().ToString();
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime(DateTime.Now.ToString("s"));
                gNodoTimbre.selloCFD = sSello;

                //********Obtienen el numero del sertificado del HSM********************

                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso19 - " + "Usando el HSM.");
                if (clsComun.ObtenerParamentro("TipoTimbrado") == "HSM")
                {

                    //Servicio HSM3
                    wslServicioPAC gServicio = new wslServicioPAC();

                    try
                    {
                        gHSM = new clsHSMComunicacion();
                        gTimbrado = new clsOperacionTimbradoSellado();
                        noCertificadoPAC = gHSM.fnObtenerNumeroCertificado(gHSM.fnHSMLogin());
                        gNodoTimbre.noCertificadoSAT = noCertificadoPAC;
                        gHSM.fnHSMLogOut();

                        //Si no se Recupero el Certificado de HSM1 o HSM2
                        if (gNodoTimbre.noCertificadoSAT == string.Empty)
                        {
                            bLlave = gServicio.HSM3_KEY();
                            bCertificado = gServicio.HSM3_CER();
                            sPassword = gServicio.HSM3_PAS();

                            gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                            gCertificado = new clsValCertificado(bCertificado);
                            noCertificadoPAC = gCertificado.ObtenerNoCertificado();
                            gNodoTimbre.noCertificadoSAT = noCertificadoPAC;
                        }
                    }
                    catch (Exception)
                    {
                        //Si falla HSM1 o HSM2
                        bLlave = gServicio.HSM3_KEY();
                        bCertificado = gServicio.HSM3_CER();
                        sPassword = gServicio.HSM3_PAS();

                        gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                        gCertificado = new clsValCertificado(bCertificado);
                        noCertificadoPAC = gCertificado.ObtenerNoCertificado();
                        gNodoTimbre.noCertificadoSAT = noCertificadoPAC;
                    }
                }
                else
                {

                    try
                    {
                        ////Preparamos los objetos de manejo tanto de la llave como del certificado
                        wslServicioPAC gServicio = new wslServicioPAC();
                        bLlave = gServicio.ObtenerLlavePAC();
                        bCertificado = gServicio.ObtenerCertificado();
                        sPassword = gServicio.ObtenerPassword();

                        gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                        gCertificado = new clsValCertificado(bCertificado);
                        noCertificadoPAC = gCertificado.ObtenerNoCertificado();
                        gNodoTimbre.noCertificadoSAT = noCertificadoPAC;

                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso20 - " + "Recueprando informacion.");
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudo recuperar la llave y el certificado del PAC", pnId_Usuario);
                        //return "622";//No se pudo recuperar la llave y el certificado del PAC
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("622", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "622", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("622", "Timbrado y Cancelación");
                        return sXmlDocument;
                    }

                }

                //********Obtienen el numero del sertificado del HSM********************

                //Generamos el primer XML necesario para generar la cadena original
                docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                //Generamos la cadena original
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso21 - " + "Genera cadena origianl de TFD.");
                XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();
                //sCadenaOriginal = gTimbrado.fnConstruirCadenaTimbrado(navNodoTimbre, "cadenaoriginal_TFD_1_0");

                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                // Load the type of the class.
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(Timbrado.V3.TFDXSLT));

                ms = new MemoryStream();
                args = new XsltArgumentList();

                xslt.Transform(navNodoTimbre, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sCadenaOriginal = srDll.ReadToEnd();


                //Generamos el sello del SAT, se lo agregamos al objeto y generamos el XML final
                //***********Genera la firma con el HSM*************************************

                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso22 - " + "Genera sello SAT.");
                if (clsComun.ObtenerParamentro("TipoTimbrado") == "HSM")
                {

                    try
                    {
                        selloPAC = gHSM.fnFirmar(sCadenaOriginal, gHSM.fnHSMLogin());
                        gNodoTimbre.selloSAT = selloPAC;
                        gHSM.fnHSMLogOut();

                        if (gNodoTimbre.selloSAT == string.Empty)
                        {
                            selloPAC = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                            gNodoTimbre.selloSAT = selloPAC;
                        }
                    }
                    catch (Exception)
                    {
                        selloPAC = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                        gNodoTimbre.selloSAT = selloPAC;
                    }

                }
                else
                {
                    selloPAC = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                    gNodoTimbre.selloSAT = selloPAC;
                    //gNodoTimbre.selloSAT = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("sello prueba SAT")); 
                }

                //Validamos
                if (!vValidadorCertificado.fnVerificarSelloPAC(sCadenaOriginal, selloPAC, noCertificadoPAC))
                {
                    sXmlDocument[0] = "320 - Verifique el comprobante, reintente de nuevo.";
                    return sXmlDocument;
                }

                //xDocTimbrado = gTimbrado.fnGenerarXML(gNodoTimbre);// Reescribe el documento. No Borrar.

                //***********Genera la firma con el HSM*************************************

                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso23 - " + "Cargando timbre al documento.");

                docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                XmlNode Complemento = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsmComprobante);
                if (Complemento == null)
                    Complemento = xDocTimbrado.CreateElement("cfdi", "Complemento", nsmComprobante.LookupNamespace("cfdi"));

                Complemento.InnerXml = docNodoTimbre.DocumentElement.OuterXml + Complemento.InnerXml;

                xDocTimbrado.DocumentElement.AppendChild(Complemento);


                //Eliminamos la llave y el certificado
                gTimbrado.fnDestruirLlave();
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudo generar el sello del PAC", pnId_Usuario);
                //return "817"; //No se pudo generar el sello del PAC
                //*******************************************************Insertar Response en tabla de acuses
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("817", "Timbrado"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "817", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("817", "Timbrado");
                return sXmlDocument;
            }

            string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {

                        //Buscar el HASH del comprobante de la cadena original del emisor.
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso24 - " + "Busca el HAS del emisor.");

                        string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginalEmisor).ToUpper();
                        string Comprobante = clsTimbradoFuncionalidad.fnBuscarHashCompXML(datosUsuario.id_usuario, HASHEmisor, "Emisor");
                        if (Comprobante != "0")
                        {
                            XmlDocument hasComprobante = new XmlDocument();
                            hasComprobante.LoadXml(Comprobante);
                            //return "98";
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            //return clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                            //*******************************************************Insertar Response en tabla de acuses

                            //***********************************************************************************************
                            //Variables de retorno de xml
                            string fechaTimb, sFolioFiscal, sNoCert, sNoCertificadoSAT, sSelloCFDI, sSelloSAT, sFechaCertificacion;
                            fechaTimb = sFolioFiscal = sNoCert = sNoCertificadoSAT = sSelloCFDI = sSelloSAT = sFechaCertificacion = string.Empty;
                            DateTime fechaTimbrado = DateTime.MinValue;
                            //Si el xml fue timbrado
                            XmlNamespaceManager nsmComprobanteT = new XmlNamespaceManager(hasComprobante.NameTable);
                            nsmComprobanteT.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                            nsmComprobanteT.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                            XPathNavigator navEncabezado = hasComprobante.CreateNavigator();
                            try { sFolioFiscal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteT).Value; }
                            catch { }
                            try { sNoCert = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value; }
                            catch { }
                            try { sNoCertificadoSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
                            catch { }
                            try { fechaTimb = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value; }
                            catch { }
                            try { sSelloCFDI = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value; } //Sello Emisor
                            catch { }
                            try { sSelloSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value; }
                            catch { }

                            try { sCadenaOriginal = fnConstruirCadenaTimbrado(hasComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0"); }
                            catch { }

                            if (!string.IsNullOrEmpty(fechaTimb))
                                fechaTimbrado = Convert.ToDateTime(fechaTimb);


                            sFechaCertificacion = fechaTimbrado.ToString("s");
                            // ref string sCadenaOriginalSAT

                            sXmlDocument[0] = hasComprobante.OuterXml;
                            sXmlDocument[1] = sFolioFiscal;
                            sXmlDocument[2] = sNoCert;
                            sXmlDocument[3] = sNoCertificadoSAT;
                            sXmlDocument[4] = sFechaCertificacion;
                            sXmlDocument[5] = sSelloCFDI;
                            sXmlDocument[6] = sSelloSAT;
                            sXmlDocument[7] = sCadenaOriginal;
                            byte[] imgCodigo = { };
                            if (benviaPdf == true)
                            {
                                try
                                {
                                    XmlDocument xXmlPdf = new XmlDocument();
                                    xXmlPdf.LoadXml(hasComprobante.OuterXml);

                                    clsPlantillaLista PlantillaLista = new clsPlantillaLista();
                                    string sRutaPDF = clsComun.ObtenerParamentro("RutaDocXmlZips") + "\\" + sFolioFiscal + ".pdf";
                                    PlantillaLista.fnCrearPLantillaEnvioGS(xXmlPdf, "Logo", "0", "Factura", sRutaPDF, 0, 0, "Green");

                                    byte[] bt = (byte[])System.IO.File.ReadAllBytes(sRutaPDF);

                                    sXmlDocument[8] = bt;
                                }
                                catch (Exception ex)
                                {
                                    sXmlDocument[8] = "PDF no generado - Error:" + ex.Message;
                                }
                            }
                            return sXmlDocument;
                        }

                        //string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginalEmisor).ToUpper();
                        //if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHEmisor,"Emisor"))
                        //{
                        //    //return "98";
                        //    //*******************************************************Insertar Response en tabla de acuses
                        //    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                        //    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                        //    //*******************************************************Insertar Response en tabla de acuses
                        //    return clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                        //}

                        //Buscar el HASH del comprobante de la cadena original del timbre fiscal.
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso25 - " + "Busca el HAS del TFD.");
                        string HASHTimbreFiscal = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();
                        if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbreFiscal, "Timbre"))
                        {
                            //return "98";
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                            return sXmlDocument;
                        }



                        SqlCommand cmd = new SqlCommand("usp_Timbrado_InsertaComprobanteAll_Ins", con);
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("sXML", xDocTimbrado.DocumentElement.OuterXml);
                        cmd.Parameters.AddWithValue("nId_tipo_documento", pnId_tipo_documento);
                        cmd.Parameters.AddWithValue("cEstatus", "P");
                        cmd.Parameters.AddWithValue("dFecha_Documento", DateTime.Now);
                        cmd.Parameters.AddWithValue("nId_estructura", pnId_Estructura);
                        cmd.Parameters.AddWithValue("nId_usuario_timbrado", pnId_Usuario);
                        cmd.Parameters.AddWithValue("nSerie", "N/A");
                        cmd.Parameters.AddWithValue("sOrigen", "R");
                        cmd.Parameters.AddWithValue("sHash", HASHTimbreFiscal);
                        cmd.Parameters.AddWithValue("sDatos", HASHEmisor);

                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso26 - " + "Inserta Comprobante.");
                        retVal = Convert.ToInt32(cmd.ExecuteScalar());
                        fnActualizaCreditos(dtCreditos, sServicio);

                        tran.Commit();



                        if (retVal == 0)
                        {
                            //return "999";
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("999", "Timbrado");
                            return sXmlDocument;
                        }
                        else
                        {

                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso27 - " + "Recupera XML para regresarlo al cliente.");
                            //***********************************************************************************************
                            //Variables de retorno de xml
                            string fechaTimb, sFolioFiscal, sNoCert, sNoCertificadoSAT, sSelloCFDI, sSelloSAT, sFechaCertificacion;
                            fechaTimb = sFolioFiscal = sNoCert = sNoCertificadoSAT = sSelloCFDI = sSelloSAT = sFechaCertificacion = string.Empty;
                            DateTime fechaTimbrado = DateTime.MinValue;
                            //Si el xml fue timbrado
                            XmlNamespaceManager nsmComprobanteT = new XmlNamespaceManager(xDocTimbrado.NameTable);
                            nsmComprobanteT.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                            nsmComprobanteT.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                            XPathNavigator navEncabezado = xDocTimbrado.CreateNavigator();
                            try { sFolioFiscal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteT).Value; }
                            catch { }
                            try { sNoCert = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value; }
                            catch { }
                            try { sNoCertificadoSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
                            catch { }
                            try { fechaTimb = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value; }
                            catch { }
                            try { sSelloCFDI = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value; } //Sello Emisor
                            catch { }
                            try { sSelloSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value; }
                            catch { }

                            try { sCadenaOriginal = fnConstruirCadenaTimbrado(xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0"); }
                            catch { }

                            if (!string.IsNullOrEmpty(fechaTimb))
                                fechaTimbrado = Convert.ToDateTime(fechaTimb);


                            sFechaCertificacion = fechaTimbrado.ToString("s");
                            // ref string sCadenaOriginalSAT

                            sXmlDocument[0] = xDocTimbrado.OuterXml;
                            sXmlDocument[1] = sFolioFiscal;
                            sXmlDocument[2] = sNoCert;
                            sXmlDocument[3] = sNoCertificadoSAT;
                            sXmlDocument[4] = sFechaCertificacion;
                            sXmlDocument[5] = sSelloCFDI;
                            sXmlDocument[6] = sSelloSAT;
                            sXmlDocument[7] = sCadenaOriginal;

                            if (benviaPdf == true)
                            {
                                try
                                {
                                    XmlDocument xXmlPdf = new XmlDocument();
                                    xXmlPdf.LoadXml(xDocTimbrado.OuterXml);
                                    byte[] imgCodigo = { };
                                    clsPlantillaLista PlantillaLista = new clsPlantillaLista();
                                    string sRutaPDF = clsComun.ObtenerParamentro("RutaDocXmlZips") + "\\" + sFolioFiscal + ".pdf";
                                    PlantillaLista.fnCrearPLantillaEnvioGS(xXmlPdf, "Logo", "0", "Factura", sRutaPDF, 0, 0, "Green");

                                    byte[] bt = (byte[])System.IO.File.ReadAllBytes(sRutaPDF);

                                    sXmlDocument[8] = bt;
                                }
                                catch (Exception ex)
                                {
                                    sXmlDocument[8] = "PDF no generado - Error:" + ex.Message;
                                }
                            }
                            //Enviar comprobante al SAT --Comentado para mejorar el rendimento de entrega de comprobante.
                            //clsEnvioSAT enviarSAT = new clsEnvioSAT();
                            //sRetornoSAT = enviarSAT.fnEnviarBloqueCfdi(HASH, pnId_Usuario, xDocTimbrado, retVal, datosUsuario);

                            //*******************************************************Insertar Response en tabla de acuses
                            string uuid = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, uuid.ToUpper(), xDocTimbrado.OuterXml, DateTime.Now, "0", "Response", "E", string.Empty);
                        }


                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante", pnId_Usuario);
                        //return "999"; //Error durante el registro del comprobante
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        sXmlDocument[0] = clsComun.fnRecuperaErrorSAT("999", "Timbrado");
                        return sXmlDocument;
                    }
                }
            }

        }
        else
        {
            //*******************************************************Insertar Response en tabla de acuses
            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sRetAutentication, "Response", "E", string.Empty);
            //*******************************************************Insertar Response en tabla de acuses
            sXmlDocument[0] = clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
        }


        return sXmlDocument;
    }

    # region MetodosInternos

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
    {
        string sCadenaOriginal = string.Empty;

        try
        {
            MemoryStream ms = new MemoryStream();
            XslCompiledTransform trans = new XslCompiledTransform();
            trans.Load(XmlReader.Create(new StringReader(clsComunPDF.ObtenerParamentro(psNombreArchivoXSLT))));
            XsltArgumentList args = new XsltArgumentList();
            trans.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            sCadenaOriginal = sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            clsErrorLogPDF.fnNuevaEntrada(ex, clsErrorLogPDF.TipoErroresLog.Datos);
        }

        return sCadenaOriginal;
    }

    private void fnCrearElementoRoot(XmlDocument pxDoc, string[] pasAtributos)
    {
        clsOperacionConsulta OC = new clsOperacionConsulta();
        XmlAttribute xAttr;
        foreach (string a in pasAtributos)
        {
            string[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = OC.reemplaza(valores[1]);
            pxDoc.DocumentElement.Attributes.Append(xAttr);
        }
        xAttr = pxDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
        xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv3.xsd";
        pxDoc.DocumentElement.Attributes.Append(xAttr);
    }

    private void fnCrearElementoRoot32(XmlDocument pxDoc, string[] pasAtributos)
    {
        clsOperacionConsulta OC = new clsOperacionConsulta();
        XmlAttribute xAttr;
        foreach (string a in pasAtributos)
        {
            string[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = OC.reemplaza(valores[1]);
            pxDoc.DocumentElement.Attributes.Append(xAttr);
        }
        xAttr = pxDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
        xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
        pxDoc.DocumentElement.Attributes.Append(xAttr);
    }

    private XmlElement fnCrearElemento(XmlDocument pxDoc, string psElemento, string[] pasAtributos)
    {
        clsOperacionConsulta OC = new clsOperacionConsulta();
        XmlAttribute xAttr;
        XmlElement elemento = pxDoc.CreateElement("cfdi", psElemento, "http://www.sat.gob.mx/cfd/3");
        foreach (string a in pasAtributos)
        {
            string[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = OC.reemplaza(valores[1]);
            elemento.Attributes.Append(xAttr);
        }
        return elemento;
    }

    private clsValCertificado fnRecuperarCertificado(string psComprobante)
    {
        //recuperamos el certificado del comprobante
        XmlDocument xDocTimbrado = new XmlDocument();
        xDocTimbrado.LoadXml(psComprobante);

        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

        string sCertificadoBase64 = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).Value;
        if (string.IsNullOrEmpty(sCertificadoBase64))
        {
            errorCode = 570; //No se pudó recuperar el certificado del comprobante
            return null;
        }

        return new clsValCertificado(Convert.FromBase64String(sCertificadoBase64));
    }

    private int ErrorsCount;
    private bool Ignorar;

    /// <summary>
    /// Obtiene los errores de validacion del esquema.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ValidationHandler(object sender, ValidationEventArgs args)
    {
        if (!Ignorar)
            ErrorsCount++;
    }

    public int fnValidate(string psXmlDocument, string psNombreEsquema)
    {
        //recuperamos el esquema de la base de datos
        string esquema = clsComun.ObtenerParamentro(psNombreEsquema);

        // Declare local objects
        XmlTextReader tr = null;
        XmlSchemaCollection xsc = null;
        XmlValidatingReader vr = null;

        // Text reader object
        tr = new XmlTextReader(new System.IO.StringReader(esquema));
        xsc = new XmlSchemaCollection();
        xsc.Add(null, tr);

        // XML validator object

        vr = new XmlValidatingReader(psXmlDocument,
                        XmlNodeType.Document, null);

        vr.Schemas.Add(xsc);

        // Add validation event handler

        vr.ValidationType = ValidationType.Schema;
        vr.ValidationEventHandler +=
                    new ValidationEventHandler(ValidationHandler);

        ErrorsCount = 0;
        Ignorar = false;

        // Validate XML data
        while (vr.Read())
        {
            if (vr.Name.Contains("Complemento") || vr.Name.Contains("Addenda"))
            {
                Ignorar = true;
                vr.Skip();
                Ignorar = false;
            }
        }

        vr.Close();

        if (ErrorsCount != 0)
            return 333;
        else
            return 0;
    }

    /// <summary>
    /// Validar datos del usario.
    /// </summary>
    /// <param name="sNombre"></param>
    /// <param name="sContraseña"></param>
    /// <returns></returns>
    private string fnAutentication(string sNombre, string sContraseña, ref int nId_usuario, ref clsInicioSesionUsuario datosUsuario)
    {
        string sVarNombre = sNombre.Trim();
        string sVarContraseña = sContraseña.Trim();
        clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
        datosUsuario = new clsInicioSesionUsuario();

        DataTable tabla = new DataTable();
        DataTable tablaRFC = new DataTable();

        int nId_contribuyente;
        //int nId_usuario;
        char cEstatus;
        string sEmail;
        string sRetorno = string.Empty;

        if (string.IsNullOrEmpty(sVarNombre) || string.IsNullOrEmpty(sVarContraseña))
        {
            return clsComun.fnRecuperaErrorSAT("90", "Seguridad");
        }

        //LLena datos de usuario.
        tabla = busquedaUsuario.buscarUsuario(sVarNombre);
        tablaRFC = busquedaUsuario.buscarUsuarioRFC(sVarNombre);

        if (tabla.Rows.Count > 0)
        {
            try
            {
                sVarContraseña = Utilerias.Encriptacion.Base64.DesencriptarBase64(sVarContraseña);
            }
            catch (Exception)
            {
                return clsComun.fnRecuperaErrorSAT("96", "Seguridad");
            }

            //Revisa contraseña
            if (Utilerias.Encriptacion.Classica.Desencriptar(sVarContraseña) == Utilerias.Encriptacion.Classica.Desencriptar(tabla.Rows[0]["password"].ToString()))
            {
                //Guardar datos en clase Usuario
                nId_contribuyente = Convert.ToInt32(tabla.Rows[0]["id_contribuyente"]);
                nId_usuario = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
                cEstatus = Convert.ToChar(tabla.Rows[0]["estatus"]);
                sEmail = Convert.ToString(tabla.Rows[0]["email"]);

                datosUsuario.id_contribuyente = nId_contribuyente;
                datosUsuario.id_usuario = nId_usuario;
                datosUsuario.estatus = cEstatus;
                datosUsuario.email = sEmail;

                if (tablaRFC.Rows.Count > 0)
                {
                    datosUsuario.version = Convert.ToString(tablaRFC.Rows[0]["version"]);
                }


                //Revisar el estatus del usuario.
                switch (cEstatus)
                {

                    case 'P':
                        sRetorno = clsComun.fnRecuperaErrorSAT("91", "Seguridad");
                        break;
                    case 'A':
                        sRetorno = clsComun.fnRecuperaErrorSAT("0", "Todos"); ;
                        break;
                    case 'B':
                        sRetorno = clsComun.fnRecuperaErrorSAT("92", "Seguridad");
                        break;
                    case 'E':
                        sRetorno = clsComun.fnRecuperaErrorSAT("93", "Seguridad");
                        break;
                    case 'C':
                        sRetorno = clsComun.fnRecuperaErrorSAT("94", "Seguridad");
                        break;
                }
            }
            else
            {
                sRetorno = clsComun.fnRecuperaErrorSAT("96", "Seguridad");
            }
        }
        else
        {
            sRetorno = clsComun.fnRecuperaErrorSAT("95", "Seguridad");
        }

        return sRetorno;
    }

    /// <summary>
    /// Recupera el Id de la matriz del usuario.
    /// </summary>
    /// <param name="pnId_Usuario"></param>
    /// <returns></returns>
    private string fnRecuperaMatriz(int pnId_Usuario)
    {
        try
        {
            string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
            {
                SqlCommand cmd = new SqlCommand("usp_Con_Obtener_Matriz_Sel", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("nId_Usuario", pnId_Usuario);

                con.Open();
                string retVal = cmd.ExecuteScalar().ToString();
                con.Close();

                if (retVal == "0")
                    //return "999";
                    return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
                else
                    return retVal;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante", pnId_Usuario);
            //return "999"; //Error durante el registro del comprobante
            return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
        }
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    /// <summary>
    /// Regresa la validacion de las fechas en el LCO
    /// </summary>
    /// <param name="sNoCertificado"></param>
    /// <param name="estatus"></param>
    /// <returns></returns>
    private bool fnRecuperaFechaLCO(string sNoCertificado, string estatus, ref clsValCertificado vValidadorCertificado)
    {
        DataTable fechas = vValidadorCertificado.RevisaExistenciaCertificadoFechas(sNoCertificado, estatus);

        if (Convert.ToDateTime(fechas.Rows[0]["fecha_inicio"].ToString()).CompareTo(DateTime.Today) > 0
           || Convert.ToDateTime(fechas.Rows[0]["fecha_final"].ToString()).CompareTo(DateTime.Today) < 0)
            return false;

        return true;
    }

    /// <summary>
    /// Verifica si el certificado esta revocado o caduco
    /// </summary>
    private bool fnVerificarCaducidadCertificado(string sNoCertificado, string estatusR, string estatusC, ref clsValCertificado vValidadorCertificado)
    {
        string rfcCaduco = vValidadorCertificado.RevisaCaducidadCertificado(sNoCertificado, estatusR, estatusC);

        if (!string.IsNullOrEmpty(rfcCaduco))
            return false;

        return true;
    }

    /// <summary>
    /// Actauliza los creditos disponibles.
    /// </summary>
    private int fnActualizaCreditos(DataTable dtCreditos, string sServicio)
    {
        DataTable tlbCreditos = new DataTable();
        int idCredito = 0;
        int idEstructura = 0;
        double Creditos = 0;
        int nRetorno = 0;

        tlbCreditos = dtCreditos;

        if (tlbCreditos.Rows.Count > 0)
        {

            idCredito = Convert.ToInt32(tlbCreditos.Rows[0]["id_creditos"]);
            idEstructura = Convert.ToInt32(tlbCreditos.Rows[0]["id_estructura"]);
            Creditos = Convert.ToDouble(tlbCreditos.Rows[0]["creditos"]);

            nRetorno = clsTimbradoFuncionalidad.fnActualizarCreditos(idCredito, idEstructura, Creditos, sServicio);

            clsTimbradoFuncionalidad.fnActualizarCreditosHistorico(idCredito, idEstructura, Creditos);
        }

        return nRetorno;

    }

    # endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Net;
using System.Net.Security;
using System.Xml.Schema;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;

/// <summary>
/// 
/// </summary>
[WebService(Namespace = "https://test.paxfacturacion.com.mx:454")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class wcfValidaASMX : System.Web.Services.WebService 
{

    private int errorCode;
    private TimbreFiscalDigital gNodoTimbre;
    private int ErrorsCount;
    private bool Ignorar;
    private clsOperacionTimbradoSellado gTimbrado;
    private clsInicioSesionUsuario datosUsuario;
    private clsValCertificado gCertificado;
    private clsHSMComunicacion gHSM;
    protected DataTable dtCreditos;

    /*  Códigos de error
     '''   0   - Sin errores
     '''   100 - El archivo de texto esta mal formado
       ''' 177 - El certificado no se encuentra en la lista de régimen fiscal
      '''  179 - El RFC del Certificado no corresponde al del comprobante
      ''  200 - El certificado está fuera de su periodo de validez
      '''  333 - El XML no cumple con el esquema de hacienda
      '''  406 - El nombre de documento no corresponde a ningúno del sistema
      '''  472 - El comprobante ya está timbrado
     '''   504 - La fecha del comprobante esta fuera de periodo
        511 - El sello no corresponde a los datos del comprobante
     '''   570 - No se pudó recuperar el certificado del comprobante
     '''   592 - El certificado no es de tipo CSD
    '''    645 - El comprobante no contiene un sello de emisor
       ''' 799 - Faltan datos del comprobante
        999 - Error durante el registro del comprobante
    */

    [WebMethod]
    public string fnValidaXML(string psComprobante, string sNombre, string sContraseña, string sVersion)
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
        string sSerie = string.Empty;
        string sFolio = string.Empty;
        string sUUID = string.Empty;
        clsConfiguracionCreditos css;

        switch (sVersion)
        {
            case "3.0":
                scadena = "cadenaoriginal_3_0";
                sesquema = "esquema_v3";
                break;
            case "3.2":
                scadena = "cadenaoriginal_3_2";
                sesquema = "esquema_v3_2";
                break;
        }



        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(wcfRecepcionASMX.AcceptAllCertificatePolicy);

        //Verifica el usuario.

        sRetAutentication = fnAutentication(sNombre, sContraseña, ref pnId_Usuario, ref datosUsuario);
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso1 - " + "Verifica el usuario.");



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


            //Revisar los creditos disponibles.
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
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                css = new clsConfiguracionCreditos();
                double dCostCred = css.fnRecuperaPrecioServicio(5);

                if (creditos == 0)
                {
                    return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                }

                if (creditos < dCostCred)
                {
                    return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                }
            }


            //Recupera datos del emisor.
            clsValCertificado vValidadorCertificado = null;
            XmlDocument xDocTimbrado = new XmlDocument();
            XmlDocument docNodoTimbre;
            gNodoTimbre = new TimbreFiscalDigital();
            string sRFCEmisor = string.Empty;
            string sRFCReceptor = string.Empty;
            string sSello = string.Empty;
            string sNoCertificado = string.Empty;
            DateTime dFechaComprobante;

            string sPassword = string.Empty;
            errorCode = 0;
            DataTable tblFechas = new DataTable();

            fnActualizaCreditos(dtCreditos);

            //Se elimina BOM en caso de que lo contenga
            if (psComprobante.Contains("<?xml"))
            {
                int pos = psComprobante.IndexOf("<?xml");
                if (pos > 0)
                {
                    psComprobante = psComprobante.Substring(pos, psComprobante.Length - pos);
                }
            }

            //Verificamos que el XML cumpla con el esquema de SAT
            try
            {
                errorCode = fnValidate(psComprobante, sesquema);//"esquema_v3");
                if (errorCode != 0)
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, null, null, null, null, sVersion, "333");
                    //333 - El XML no cumple con el esquema de hacienda
                    return clsComun.fnRecuperaErrorSAT("333", "Consulta");
                }
            }
            catch (Exception)
            {
                //333 - El XML no cumple con el esquema de hacienda
                return clsComun.fnRecuperaErrorSAT("333", "Consulta");
            }

            //Recuperamos el certificado a partir del XML del comprobante
            try
            {
                vValidadorCertificado = fnRecuperarCertificado(psComprobante);

                if (vValidadorCertificado == null)
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, null, null, null, null, sVersion, "570");
                    //570 - No se pudo recuperar el certificado del comprobante
                    return clsComun.fnRecuperaErrorSAT("570", "Timbrado");
                }
            }
            catch (Exception)
            {
                //570 - No se pudo recuperar el certificado del comprobante
                return clsComun.fnRecuperaErrorSAT("570", "Timbrado");
            }

            try
            {
                //Verificamos que el certificado del comprobante se de tipo CSD
                if (!vValidadorCertificado.fnEsCSD())
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, null, null, null, null, sVersion, "306");
                    //306 - El certificado no es de tipo CSD.
                    return clsComun.fnRecuperaErrorSAT("306", "Timbrado y Cancelación"); //El certificado no es de tipo CSD
                }
            }
            catch (Exception)
            {
                //306 - El certificado no es de tipo CSD.
                return clsComun.fnRecuperaErrorSAT("306", "Timbrado y Cancelación");
            }

            try
            {
                //Certificado no expedido por el SAT
                if (!vValidadorCertificado.fnCSD308())
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, null, null, null, null, sVersion, "308");
                    //308 - Certificado no expedido por el SAT.
                    return clsComun.fnRecuperaErrorSAT("308", "Timbrado y Cancelación"); //El certificado no es de tipo CSD
                }
            }
            catch (Exception)
            {
                //308 - Certificado no expedido por el SAT.
                return clsComun.fnRecuperaErrorSAT("308", "Timbrado y Cancelación");
            }

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try
            {
                xDocTimbrado.LoadXml(psComprobante);
                sRFCEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
                sRFCReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;
                dFechaComprobante = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;
                sSello = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value;
                sNoCertificado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value;
                try
                {
                    sSerie = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value;
                }
                catch
                {
                    sSerie = null;
                }

                try
                {
                    sFolio = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value;
                }
                catch
                {
                    sFolio = null;
                }

                try
                {
                    sUUID = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                }
                catch
                {
                    //"312 - El comprobante no contiene un Timbre Fiscal, esto es requerido en CFDI."
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "312");
                    return clsComun.fnRecuperaErrorSAT("312", "Validacion");
                }


            }
            catch (Exception)
            {
                //504 - La estructura del comprobante recibido no es válida.
                clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "504");
                return clsComun.fnRecuperaErrorSAT("504", "Validacion");

            }

            //Verificar el rfc del certificado contra el que esta agregado
            string[] datosCertificado = { };
            datosCertificado = vValidadorCertificado.Certificado.Subject.Split(',');
            if (datosCertificado.Length > 0)
            {
                for (int count = 0; count < datosCertificado.Length; count++)
                {
                    if (datosCertificado[count].Contains("OID.2.5.4.45"))
                    {
                        string[] rfc = { };
                        rfc = datosCertificado[count].Split('=');
                        try
                        {
                            if (!rfc[1].Contains(sRFCEmisor))
                            {
                                //179 - El RFC del Certificado no corresponde al del comprobante
                                return clsComun.fnRecuperaErrorSAT("179", "Validacion");
                            }
                        }
                        catch
                        {
                            //179 - El RFC del Certificado no corresponde al del comprobante
                            return clsComun.fnRecuperaErrorSAT("179", "Validacion");
                        }
                    }
                }
            }

            //Verifica que el certificado reportado se el mismo al certificado usado.
            try
            {
                if (sNoCertificado != vValidadorCertificado.ObtenerNoCertificado())
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "311");
                    //311 - Certificado reportado no corresponde a certificado usado.
                    return clsComun.fnRecuperaErrorSAT("311", "Validacion");
                }

            }
            catch (Exception)
            {
                //311 - Certificado reportado no corresponde a certificado usado.
                return clsComun.fnRecuperaErrorSAT("311", "Validacion");
            }

            //Generar la cadena del emisor con los conceptos del XML
            try
            {
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
                //310 - No se puede validar por problemas al generar la cadena.
                return clsComun.fnRecuperaErrorSAT("310", "Validacion");
            }

            try
            {
                //Verificamos que exista el sello
                if (string.IsNullOrEmpty(sSello))
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "309");
                    //309 - No se puede validar por falta de sello.
                    return clsComun.fnRecuperaErrorSAT("309", "Timbrado y Cancelación");
                }
            }
            catch (Exception)
            {
                //309 - No se puede validar por falta de sello.
                return clsComun.fnRecuperaErrorSAT("309", "Timbrado y Cancelación");
            }

            ////Certificado revocado o caduco R o C
            //if (!fnVerificarCaducidadCertificado(sNoCertificado, "R", "C", ref vValidadorCertificado))
            //{
            //    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "304");
            //    return clsComun.fnRecuperaErrorSAT("304", "Timbrado y Cancelación");
            //}

            try
            {
                //La fecha de emisión no esta dentro de la vigencia del CSD del Emisor
                if (!vValidadorCertificado.ComprobarFechasV(dFechaComprobante))
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "305");
                    //305 - La fecha de emisión no está dentro de la vigencia del CSD del Emisor.
                    return clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
                }
            }
            catch (Exception)
            {
                //305 - La fecha de emisión no está dentro de la vigencia del CSD del Emisor.
                return clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
            }

            if (!clsComun.fnRevisarFechaNoPosterior(dFechaComprobante))
            {
                clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "403");
                //403 - Que la fecha de emisión sea posterior al 01 de Enero 2011.
                return clsComun.fnRecuperaErrorSAT("403", "Timbrado");
            }

            ////RFC del emisor no se encuentra en el régimen de contribuyentes 402
            //try
            //{
            //    string rfcComprobante = vValidadorCertificado.VerificarExistenciaCertificado();
            //    if (sRFCEmisor != rfcComprobante)
            //    {
            //        clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "402");
            //        return clsComun.fnRecuperaErrorSAT("402", "Timbrado");
            //    }
            //}
            //catch
            //{
            //    return clsComun.fnRecuperaErrorSAT("402", "Timbrado");
            //}


            //Revisamos que esl sello del emisor sea valido.
            try
            {
                //Preparamos los objetos de manejo tanto de la llave como del certificado
                gTimbrado = new clsOperacionTimbradoSellado();
                // *************Comentado por limitar rendimiento al HSM*************************************************
                //Validamos el sello de emisor
                if (!vValidadorCertificado.fnVerificarSello(sCadenaOriginalEmisor, sSello))
                {
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "302", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "302");
                    //302 - Sello mal formado o inválido.
                    return clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación");
                }
                // *************Comentado por limitar rendimiento al HSM*************************************************
            }
            catch (Exception)
            {
                //302 - Sello mal formado o inválido.
                return clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación");
            }


            //Revisamos que esl sello del timbre sea valido.
            try
            {
                //Preparamos los objetos de manejo tanto de la llave como del certificado
                gTimbrado = new clsOperacionTimbradoSellado();


                //Asignar los valores de certificado,numero de certificado y sello.
                XmlNamespaceManager nsmComprobanteTimbre = new XmlNamespaceManager(xDocTimbrado.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

                //Llenamos los datos del nodo timbre
                gNodoTimbre.UUID = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime(xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value);
                gNodoTimbre.noCertificadoSAT = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value;
                gNodoTimbre.selloCFD = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloCFD", nsmComprobante).Value;
                gNodoTimbre.selloSAT = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value;

                string selloSAT = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value;
                string noCertificadoPAC = gNodoTimbre.noCertificadoSAT = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value;

                //Generamos el primer XML necesario para generar la cadena original
                docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                //Generamos la cadena original
                XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();
                sCadenaOriginal = gTimbrado.fnConstruirCadenaTimbrado(navNodoTimbre, "cadenaoriginal_TFD_1_0");



                if (!vValidadorCertificado.fnVerificarExistenciaCertificadoPAC(noCertificadoPAC))
                {
                    DownloadFTP("D:\\CertificadosPAC\\" + noCertificadoPAC,
                        "ftp://ftp2.sat.gob.mx/Certificados/FEA/" + noCertificadoPAC.Substring(0, 6) + "/" + noCertificadoPAC.Substring(6, 6)
                        + "/" + noCertificadoPAC.Substring(12, 2) + "/" + noCertificadoPAC.Substring(14, 2) + "/" + noCertificadoPAC.Substring(16, 2) + "/" + noCertificadoPAC + ".cer",
                        "anonymous", "");
                }

                //Validamos
                if (!vValidadorCertificado.fnVerificarSelloPAC(sCadenaOriginal, selloSAT, noCertificadoPAC))
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "302");
                    return "302 - Sello del SAT Invalido o no se encuentra registrado el certificado.";
                }
                // *************Comentado por limitar rendimiento al HSM*************************************************
            }
            catch (Exception)
            {
                return "302 - Sello del SAT Invalido o no se encuentra registrado el certificado.";
            }


            try
            {

                //fnActualizaCreditos(dtCreditos);
                clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "0");
                sXmlDocument = clsComun.fnRecuperaErrorSAT("0", "Todos");

            }
            catch (Exception)
            {
                clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, sUUID, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "999");
                return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
            }

        }
        else
        {
            sXmlDocument = clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
        }


        return sXmlDocument;
    }


    [WebMethod]
    public string fnValidaCFD(string psComprobante, string sNombre, string sContraseña, string sVersion)
    {
        string sXmlDocument = string.Empty;
        string sRetAutentication = string.Empty;
        string sCadenaOriginal = string.Empty;
        string sNoCertificado = string.Empty;
        int pnId_Usuario = 0;
        string sesquema = string.Empty;
        clsConfiguracionCreditos css;

        switch (sVersion)
        {
            case "2.0":

                sesquema = "esquema_v2";
                break;

            case "2.2":

                sesquema = "Esquema_v2_2";
                break;
        }

        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(wcfRecepcionASMX.AcceptAllCertificatePolicy);

        //Verifica el usuario.
        sRetAutentication = fnAutentication(sNombre, sContraseña, ref pnId_Usuario, ref datosUsuario);
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso1 - " + "Verifica el usuario.");

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


            //Revisar los creditos disponibles.
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
                        //97 - No hay créditos disponibles.
                        return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }

                    if (creditos2 < dCostCred)
                    {
                        //97 - No hay créditos disponibles.
                        return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }
                }
                else
                {
                    if (creditos < dCostCred)
                    {
                        //97 - No hay créditos disponibles.
                        return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }
                }

            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                css = new clsConfiguracionCreditos();
                double dCostCred = css.fnRecuperaPrecioServicio(5);

                if (creditos == 0)
                {
                    //97 - No hay créditos disponibles.
                    return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                }

                if (creditos < dCostCred)
                {
                    //97 - No hay créditos disponibles.
                    return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                }
            }


            //Recupera datos del emisor.
            clsValCertificado vValidadorCertificado = null;
            XmlDocument xDocTo = new XmlDocument();
            string sRFCEmisor = string.Empty;
            string sRFCReceptor = string.Empty;
            string sSerie = string.Empty;
            string sFolio = string.Empty;
            string sSello = string.Empty;
            DateTime dFechaComprobante;

            string sPassword = string.Empty;
            errorCode = 0;

            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, "wcfValidaCFDI" + "|" + "fnEnviarXML" + "|" + "Valida un comprobante" + DateTime.Now.ToString());

            fnActualizaCreditos(dtCreditos);

            //Se elimina BOM en caso de que lo contenga
            if (psComprobante.Contains("<?xml"))
            {
                int pos = psComprobante.IndexOf("<?xml");
                if (pos > 0)
                {
                    psComprobante = psComprobante.Substring(pos, psComprobante.Length - pos);
                }
            }

            //Verificamos que el XML cumpla con el esquema de SAT
            try
            {

                errorCode = fnValidate(psComprobante, sesquema);
                if (errorCode != 0)
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, null, null, null, null, sVersion, "333");
                    //333 - El XML no cumple con el esquema de hacienda
                    return clsComun.fnRecuperaErrorSAT("333", "Consulta");
                }
            }
            catch (Exception)
            {
                //333 - El XML no cumple con el esquema de hacienda
                return clsComun.fnRecuperaErrorSAT("333", "Consulta");
            }



            //Recuperamos el certificado a partir del XML del comprobante
            try
            {
                vValidadorCertificado = fnRecuperarCertificadov2(psComprobante);

                if (vValidadorCertificado == null)
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, null, null, null, null, sVersion, "570");
                    //570 - No se pudo recuperar el certificado del comprobante
                    return clsComun.fnRecuperaErrorSAT("570", "Timbrado");
                }
            }
            catch (Exception)
            {
                //570 - No se pudo recuperar el certificado del comprobante
                return clsComun.fnRecuperaErrorSAT("570", "Timbrado");
            }


            //Verificamos que el certificado del comprobante se de tipo CSD
            if (!vValidadorCertificado.fnEsCSD())
            {
                //El certificado no es de tipo CSD
                clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, null, null, null, null, sVersion, "306");
                //306 - El certificado no es de tipo CSD.
                return clsComun.fnRecuperaErrorSAT("306", "Timbrado y Cancelación");
            }

            //Certificado no expedido por el SAT
            if (!vValidadorCertificado.fnCSD308())
            {
                clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, null, null, null, null, sVersion, "380");
                //308 - Certificado no expedido por el SAT.
                return clsComun.fnRecuperaErrorSAT("308", "Timbrado y Cancelación"); //El certificado no es de tipo CSD
            }

            //Recuperar datos del XML
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTo.NameTable);
            nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");

            try
            {
                xDocTo.LoadXml(psComprobante);
                sRFCEmisor = xDocTo.CreateNavigator().SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@rfc", nsmComprobante).Value;
                sRFCReceptor = xDocTo.CreateNavigator().SelectSingleNode("/cfd:Comprobante/cfd:Receptor/@rfc", nsmComprobante).Value;
                dFechaComprobante = xDocTo.CreateNavigator().SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;
                sSello = xDocTo.CreateNavigator().SelectSingleNode("/cfd:Comprobante/@sello", nsmComprobante).Value;
                sNoCertificado = xDocTo.CreateNavigator().SelectSingleNode("/cfd:Comprobante/@noCertificado", nsmComprobante).Value;
                try
                {
                    sSerie = xDocTo.CreateNavigator().SelectSingleNode("/cfd:Comprobante/@serie", nsmComprobante).Value;
                }
                catch
                {
                    sSerie = null;
                }
                try
                {
                    sFolio = xDocTo.CreateNavigator().SelectSingleNode("/cfd:Comprobante/@folio", nsmComprobante).Value;
                }
                catch
                {
                    sFolio = null;
                }
            }
            catch (Exception)
            {
                //Faltan datos del comprobante
                clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "504");
                //504 - La estructura del comprobante recibido no es válida.
                return clsComun.fnRecuperaErrorSAT("504", "Validacion");
            }

            //Verifica que el certificado reportado se el mismo al certificado usado.
            try
            {
                if (sNoCertificado != vValidadorCertificado.ObtenerNoCertificado())
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "311");
                    //311 - Certificado reportado no corresponde a certificado usado.
                    return clsComun.fnRecuperaErrorSAT("311", "Validacion");
                }

            }
            catch (Exception)
            {
                //311 - Certificado reportado no corresponde a certificado usado.
                return clsComun.fnRecuperaErrorSAT("311", "Validacion");
            }

            //Leer el sellos del XML
            try
            {
                if (string.IsNullOrEmpty(sSello))
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "309");
                    //309 - No se puede validar por falta de sello.
                    return clsComun.fnRecuperaErrorSAT("309", "Validacion");
                }
            }
            catch (Exception)
            {
                //309 - No se puede validar por falta de sello.
                return clsComun.fnRecuperaErrorSAT("309", "Validacion");
            }

            //Verificamos que el certificado sea válido y pertenezca al RFC de emisor que viene en el comprobante
            try
            {

                string rfcComprobante = vValidadorCertificado.VerificarExistenciaCertificadoV2();
                if (string.IsNullOrEmpty(rfcComprobante) || sRFCEmisor != rfcComprobante)
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "179");
                    //179 - El RFC del Certificado no corresponde al del comprobante
                    return clsComun.fnRecuperaErrorSAT("179", "Timbrado y Cancelación");
                }
            }
            catch (Exception)
            {
                //179 - El RFC del Certificado no corresponde al del comprobante
                return clsComun.fnRecuperaErrorSAT("179", "Timbrado y Cancelación");
            }

            try
            {
                //Verificamos que el periodo de validez del certificado este vigente
                if (!vValidadorCertificado.ComprobarFechasV2(dFechaComprobante))
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "305");
                    //305 - La fecha de emisión no está dentro de la vigencia del CSD del Emisor.
                    return clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
                }
            }
            catch (Exception)
            {
                //305 - La fecha de emisión no está dentro de la vigencia del CSD del Emisor.
                return clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
            }


            //Generar la cadena del emisor con los conceptos del XML
            try
            {

                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                switch (sVersion)
                {
                    case "2.0":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(cadenaoriginal_2_0));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(xDocTo.CreateNavigator(), args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCadenaOriginal = srDll.ReadToEnd();


                        break;
                    case "2.2":

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(cadenaoriginal_2_2));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();

                        xslt.Transform(xDocTo.CreateNavigator(), args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCadenaOriginal = srDll.ReadToEnd();

                        break;
                }

            }
            catch (Exception)
            {
                //310 - No se puede validar por problemas al generar la cadena.
                return clsComun.fnRecuperaErrorSAT("310", "Validacion");
            }

            try
            {
                //Validamos el sello de emisor
                if (!vValidadorCertificado.fnVerificarSello(sCadenaOriginal, sSello))
                {
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "302");
                    //302 - Sello mal formado o inválido.
                    return clsComun.fnRecuperaErrorSAT("302", "Timbrado");
                }
            }
            catch (Exception)
            {
                //302 - Sello mal formado o inválido.
                return clsComun.fnRecuperaErrorSAT("302", "Timbrado");
            }

            //try
            //{
            //    if (!fnVerificarCaducidadCertificadoCSD(sNoCertificado, "R", "C", ref vValidadorCertificado))
            //    {
            //        clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "Caducidad");
            //        return clsComun.fnRecuperaErrorSAT("304", "Timbrado y Cancelación");
            //    }
            //}
            //catch (Exception)
            //{
            //    return clsComun.fnRecuperaErrorSAT("304", "Timbrado y Cancelación");
            //}

            try
            {

                if (errorCode == 0)
                {
                    //fnActualizaCreditos(dtCreditos);
                    clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "0");
                    sXmlDocument = clsComun.fnRecuperaErrorSAT("0", "Todos");
                }
                else
                    sXmlDocument = errorCode.ToString();

            }
            catch (Exception)
            {
                clsPistasAuditoria.fnInsertaValidacion(pnId_Usuario, DateTime.Now, null, sSerie, sFolio, sRFCEmisor, sRFCReceptor, sVersion, "999");
                return "999 - No se pudo verificar el comprobante.";
            }
        }
        else
        {
            sXmlDocument = clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
        }

        return sXmlDocument;

    }

    [WebMethod]
    public object[] fnValidarPAX001(string psComprobante, string sNombre, string sContraseña)
    {
        object[] xmlDatos = new object[5];

        //Se elimina BOM en caso de que lo contenga
        if (psComprobante.Contains("<?xml"))
        {
            int pos = psComprobante.IndexOf("<?xml");
            if (pos > 0)
            {
                psComprobante = psComprobante.Substring(pos, psComprobante.Length - pos);
            }
        }

        XmlDocument xDocumento = new XmlDocument();
        xDocumento.LoadXml(psComprobante);

        XmlElement xElemento = (XmlElement)xDocumento.GetElementsByTagName("cfdi:Comprobante")[0];

        if (xElemento == null)
            xElemento = (XmlElement)xDocumento.GetElementsByTagName("cfd:Comprobante")[0];

        if (xElemento == null)
            xElemento = (XmlElement)xDocumento.GetElementsByTagName("Comprobante")[0];

        string sVersion = string.Empty;
        if (xElemento != null)
            sVersion = xElemento.Attributes["version"].Value;
        else
            xmlDatos[0] = "XML mal formado - No se puede obtener la version";

        //Verifica version del XML
        if (sVersion == "2.0" || sVersion == "2.2")
        {
            //Invoca funcion para validar xml 
            xmlDatos[0] = fnValidaCFD(psComprobante, sNombre, sContraseña, sVersion);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(psComprobante);

            //Valida tipo de error.
            string[] sValida = xmlDatos[0].ToString().Split('-');
            if (fnValidaError(sValida[0].ToString().Trim()) == true)
            {
                //Si el xml esta bien formado se retorna valores requeridos
                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDoc.NameTable);
                nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");

                try { xmlDatos[1] = xDoc.CreateNavigator().SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@rfc", nsmComprobante).Value; } //RFC Emisor
                catch { xmlDatos[2] = null; }
                try { xmlDatos[2] = xDoc.CreateNavigator().SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).ValueAsDateTime; } //Fecha de Emision
                catch { xmlDatos[2] = null; }
                try { xmlDatos[3] = xDoc.CreateNavigator().SelectSingleNode("/cfd:Comprobante/@total", nsmComprobante).Value; } //Importe total
                catch { xmlDatos[3] = null; }
                try
                {
                    //IVA
                    XPathNavigator navComprobante = xDoc.CreateNavigator();
                    XPathNodeIterator navDetalles = navComprobante.Select("/cfd:Comprobante/cfd:Impuestos/cfd:Traslados/cfd:Traslado", nsmComprobante);

                    string sIva = string.Empty;
                    double dTOTIVA = 0;
                    //Se recorre nodos de traslado y se obtiene el total de IVA
                    while (navDetalles.MoveNext())
                    {
                        XPathNavigator Detalle = navDetalles.Current;
                        sIva = Detalle.SelectSingleNode("@impuesto", nsmComprobante).Value;

                        if (sIva.ToUpper() == "IVA")
                        {
                            try { dTOTIVA += Convert.ToDouble(Detalle.SelectSingleNode("@importe", nsmComprobante).Value); }
                            catch { dTOTIVA += 0; }
                        }

                        xmlDatos[4] = dTOTIVA; //Total IVA
                    }
                }
                catch { xmlDatos[4] = null; }
            }
        }
        else if (sVersion == "3.0" || sVersion == "3.2")
        {
            xmlDatos[0] = fnValidaXML(psComprobante, sNombre, sContraseña, sVersion);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(psComprobante);

            //Valida tipo de error.
            string[] sValida = xmlDatos[0].ToString().Split('-');
            if (fnValidaError(sValida[0].ToString().Trim()) == true)
            {
                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDoc.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

                try { xmlDatos[1] = xDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value; } //RFC Emisor
                catch { xmlDatos[1] = null; }
                try { xmlDatos[2] = xDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime; } //Fecha de Emision
                catch { xmlDatos[2] = null; }
                try { xmlDatos[3] = xDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value; } //Importe total
                catch { xmlDatos[3] = null; }

                try
                {
                    //IVA
                    XPathNavigator navComprobante = xDoc.CreateNavigator();
                    XPathNodeIterator navDetalles = navComprobante.Select("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado", nsmComprobante);

                    string sIva = string.Empty;
                    double dTOTIVA = 0;
                    //Se recorre nodos de traslado y se obtiene el total de IVA
                    while (navDetalles.MoveNext())
                    {
                        XPathNavigator Detalle = navDetalles.Current;
                        sIva = Detalle.SelectSingleNode("@impuesto", nsmComprobante).Value;

                        if (sIva.ToUpper() == "IVA")
                        {
                            try { dTOTIVA += Convert.ToDouble(Detalle.SelectSingleNode("@importe", nsmComprobante).Value); }
                            catch { dTOTIVA += 0; }
                        }
                    }

                    xmlDatos[4] = dTOTIVA; //Total IVA
                }
                catch { xmlDatos[4] = null; }
            }
        }

        return xmlDatos;
    }


    # region MetodosPrivados

    /// <summary>
    /// Valida el error y regresa un true si es diferente a 97, 90, 95, 96, 91, 92, 93, 94
    /// </summary>
    /// <param name="sNoError"></param>
    /// <returns>Regresa un valor booleano</returns>
    private bool fnValidaError(string sNoError)
    {
        bool bValida = false;
        //Que sea diferente al error por usuario o creditos
        if (sNoError != "97" && sNoError != "90" && sNoError != "95" && sNoError != "95" && sNoError != "96" && sNoError != "91" && sNoError != "92" && sNoError != "93" && sNoError != "94")
            bValida = true;

        return bValida;
    }
    private clsValCertificado fnRecuperarCertificadov2(string psComprobante)
    {
        //recuperamos el certificado del comprobante
        XmlDocument xDocTimbrado = new XmlDocument();
        xDocTimbrado.LoadXml(psComprobante);

        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
        nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");

        string sCertificadoBase64 = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfd:Comprobante/@certificado", nsmComprobante).Value;
        if (string.IsNullOrEmpty(sCertificadoBase64))
        {
            errorCode = 570; //No se pudó recuperar el certificado del comprobante
            return null;
        }

        return new clsValCertificado(Convert.FromBase64String(sCertificadoBase64));
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
        if (!string.IsNullOrEmpty(sNoCertificado) || sNoCertificado != "")
        {
        string rfcCaduco = vValidadorCertificado.RevisaCaducidadCertificado(sNoCertificado, estatusR, estatusC);

        if (!string.IsNullOrEmpty(rfcCaduco))
            return false;

        return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Verifica si el certificado esta revocado o caduco
    /// </summary>
    private bool fnVerificarCaducidadCertificadoCSD(string sNoCertificado, string estatusR, string estatusC, ref clsValCertificado vValidadorCertificado)
    {
        if (!string.IsNullOrEmpty(sNoCertificado) || sNoCertificado != "")
        {
            string rfcCaduco = vValidadorCertificado.RevisaCaducidadCertificadoCSD(sNoCertificado, estatusR, estatusC);

            if (!string.IsNullOrEmpty(rfcCaduco) || rfcCaduco != "")
                return false;

            return true;
        }
        else
        {
            return false;
        }
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
        XmlDocument document = new XmlDocument();
        document.LoadXml(psXmlDocument);

        if (psNombreEsquema == "esquema_v2" || psNombreEsquema == "Esquema_v2_2")
        {

            //Se lee nodo addenda
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(document.NameTable);
            nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");

            XPathNavigator navComprobante = document.CreateNavigator();
            XPathNodeIterator navAddenda = navComprobante.Select(String.Format("/{0}:Comprobante/{0}:Addenda", "cfd"), nsmComprobante);

            //Si contiene addenda
            if (navAddenda.Count > 0)
            {
                XmlNode AddendaNode = document.SelectSingleNode(String.Format("/{0}:Comprobante/{0}:Addenda", "cfd"), nsmComprobante);
                AddendaNode.ParentNode.RemoveChild(AddendaNode);

                if (psNombreEsquema == "Esquema_v2_2")
                {
                    if (document.DocumentElement.Attributes["xsi:schemaLocation"].Value != "http://www.sat.gob.mx/cfd/2 http://www.sat.gob.mx/sitio_internet/cfd/2/cfdv22.xsd")
                    {
                        document.DocumentElement.Attributes["xsi:schemaLocation"].Value = "http://www.sat.gob.mx/cfd/2 http://www.sat.gob.mx/sitio_internet/cfd/2/cfdv22.xsd";
                    }

                }

                if (psNombreEsquema == "esquema_v2")
                {

                    if (document.DocumentElement.Attributes["xsi:schemaLocation"].Value != "http://www.sat.gob.mx/cfd/2 http://www.sat.gob.mx/sitio_internet/cfd/2/cfdv2.xsd")
                    {
                        document.DocumentElement.Attributes["xsi:schemaLocation"].Value = "http://www.sat.gob.mx/cfd/2 http://www.sat.gob.mx/sitio_internet/cfd/2/cfdv2.xsd";
                    }

                }

            }

            psXmlDocument = document.InnerXml;
        }

        if (psNombreEsquema == "esquema_v3" || psNombreEsquema == "esquema_v3_2")
        {

            //Se lee nodo addenda
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(document.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            XPathNavigator navComprobante = document.CreateNavigator();
            XPathNodeIterator navAddenda = navComprobante.Select("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante);

            //Si contiene addenda
            if (navAddenda.Count > 0)
            {
                XmlNode AddendaNode = document.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante);
                AddendaNode.ParentNode.RemoveChild(AddendaNode);

                if (psNombreEsquema == "esquema_v3_2")
                {

                    if (document.DocumentElement.Attributes["xsi:schemaLocation"].Value != "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd")
                    {
                        document.DocumentElement.Attributes["xsi:schemaLocation"].Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
                    }

                }

                if (psNombreEsquema == "esquema_v3")
                {

                    if (document.DocumentElement.Attributes["xsi:schemaLocation"].Value != "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv3.xsd")
                    {
                        document.DocumentElement.Attributes["xsi:schemaLocation"].Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv3.xsd";
                    }

                }
            }

            psXmlDocument = document.InnerXml;
        }



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
                    return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
                else
                    return retVal;
            }
        }
        catch (Exception)
        {
            //Error durante el registro del comprobante
            return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
        }
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
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
    /// Actauliza los creditos disponibles.
    /// </summary>
    private int fnActualizaCreditos(DataTable dtCreditos)
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

            nRetorno = clsTimbradoFuncionalidad.fnActualizarCreditos(idCredito, idEstructura, Creditos, "V");
            clsTimbradoFuncionalidad.fnActualizarCreditosHistorico(idCredito, idEstructura, Creditos);
        }

        return nRetorno;

    }

    public Boolean DownloadFTP(string LocalDirectory, string RemoteFile, string Login, string Password)
    {
        try
        {
            // Creo el objeto ftp 

            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(RemoteFile);

            // Fijo las credenciales, usuario y contraseña

            ftp.Credentials = new NetworkCredential(Login, Password);

            // Le digo que no mantenga la conexión activa al terminar.

            ftp.KeepAlive = false;

            // Indicamos que la operación es descargar un archivo...

            ftp.Method = WebRequestMethods.Ftp.DownloadFile;


            // … en modo binario … (podria ser como ASCII)

            ftp.UseBinary = true;


            // Desactivo cualquier posible proxy http.

            // Ojo pues de saltar este paso podría usar 

            // un proxy configurado en iexplorer

            ftp.Proxy = null;

            // Activar si se dispone de SSL

            ftp.EnableSsl = false;

            // Configuro el buffer a 2 KBytes

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;

            LocalDirectory = Path.Combine(LocalDirectory, Path.GetFileName(RemoteFile));
            Boolean Retorno = false;




            using (Stream strm = ftp.GetResponse().GetResponseStream())
            {
                //DesencriptacionLCO.clsLog.fnEscribirEntrada("Se inició la descarga del archivo [" + RemoteFile + "]", DesencriptacionLCO.TipoLOG.Aplicacion);
                // Leer del buffer 2kb cada vez  

                contentLen = strm.Read(buff, 0, buffLength);

                FileInfo file = new FileInfo(LocalDirectory);

                if (!Directory.Exists(file.DirectoryName))
                {
                    DirectoryInfo dir = Directory.CreateDirectory(file.DirectoryName);
                }

                using (FileStream fs = new FileStream(LocalDirectory, FileMode.Create, FileAccess.Write, FileShare.None))
                    // mientras haya datos en el buffer...

                    while (contentLen != 0)
                    {


                        // escribir en el stream del fichero
                        //el contenido del stream de conexión

                        fs.Write(buff, 0, contentLen);

                        contentLen = strm.Read(buff, 0, buffLength);
                        Retorno = true;
                    }

            }
            return Retorno;

        }
        catch (WebException ex)
        {
            //No se encuentra el archivo o no está disponible
            //Se inicia el timer para intentar descargar nuevamente el archivo
            //DesencriptacionLCO.clsLog.fnEscribirEntrada("Ha ocurrido un error al descargar el archivo " + RemoteFile + ", se eliminarán los archivos creados y se intentará descargar el archivo nuevamente -  FDesencriptarLCO.DownloadFTP():\r\n " + ex.Message, DesencriptacionLCO.TipoLOG.Error);
            //string DateNow = DateTime.Now.ToString("yyyy-MM-dd");
            //File.Delete(Settings.Default.rutaLCOFTP + "LCODesencriptado.txt");
            //File.Delete(Settings.Default.rutaLCOFTP + "LCO_" + DateNow + ".XML");

            //DesencriptacionLCO.clsLog.fnEscribirEntrada("Se intentará nuevamente la descarga del archivo LCO [" + RemoteFile + "]", DesencriptacionLCO.TipoLOG.Aplicacion);
            //timerVerificaArchivo.Enabled = true; //Se inicia timer para intentar nuevamente la descarga del archivo

            return false;
        }
        catch (Exception ex)
        {
            //DesencriptacionLCO.clsLog.fnEscribirEntrada("Ha ocurrido un error en la descarga vía FTP, no se intentará descargar el archivo nuevamente pero el servicio seguirá funcionando normalmente - FDesencriptarLCO.DownloadFTP():\r\n " + ex.Message, DesencriptacionLCO.TipoLOG.Error);
            //string DateNow = DateTime.Now.ToString("yyyy-MM-dd");
            //File.Delete(Settings.Default.rutaLCOFTP + "LCODesencriptado.txt");
            //File.Delete(Settings.Default.rutaLCOFTP + "LCO_" + DateNow + ".XML");
            ////FDesencriptarLCO Desencriptar = new FDesencriptarLCO();
            ////Desencriptar.Desencriptar3();

            ////Enviar correo para indicar que ocurrió un problema al descargar  el archivo LCO
            //fnEnviaCorreoDescarga(Settings.Default.rutaLCOFTP + "LCO_" + DateNow + ".XML", Settings.Default.FTP + "LCO_" + DateNow + ".XML", false);
            //timerVerificaArchivo.Enabled = false; //Se desactiva timer

            return false;
        }
    }
  

    #   endregion

    
}

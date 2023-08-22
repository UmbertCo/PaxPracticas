using ConectorPDFSharp.Properties;
using ConectorPDFSharp.wcfRecpcionASMX;
using ConectorPDFSharp.wsLicenciaASMX;
using Microsoft.Win32;
using OpenSSL_Lib;
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Serialization;
using ConectorPDFSharp.wcfRecpcionASMX;
using ConectorPDFSharp.wsLicenciaASMX;

public class clsTimbradoGeneracion
{

    #region Variables Privadas

    private byte[] gLlave;
    private byte[] gLlavePAC;
    private string gsPassword;
    private string _sSerie;
    private string _sFolio;

    #endregion

    #region Propiedades Públicas

    /// <summary>
    /// Retorna o establece el arreglo de bytes del archivo key
    /// </summary>
    public byte[] LlavePrivada
    {
        get { return gLlave; }
        set { gLlave = value; }
    }

    /// <summary>
    /// Retorna o establece el arreglo de bytes del archivo key
    /// </summary>
    public byte[] LlavePrivadaPAC
    {
        get { return gLlavePAC; }
        set { gLlavePAC = value; }
    }

    /// <summary>
    /// Retorna o establece el password de la llave privada
    /// </summary>
    public string Password
    {
        get { return gsPassword; }
        set { gsPassword = value; }
    }

    public string gsSerie
    {
        get
        {
            return _sSerie;
        }
        set
        {
            _sSerie = value;
        }
    }

    public string gsFolio
    {
        get
        {
            return _sFolio;
        }
        set
        {
            _sFolio = value;
        }
    }

    //Ambos parametros para generar el nombre del xml y el pdf
    public string sRFC = string.Empty;
    public string sUUID = string.Empty;

    public string sListaCorreos = string.Empty;
    StringBuilder sbAdenda = new StringBuilder(); // Para la generacion del correo, utilizado en fnGenerarComprobante

    #endregion

    private bool bValida { get; set; }

    //Servicios
    private wcfRecepcionASMXSoapClient wsRecepcionT = new wcfRecepcionASMXSoapClient();

    //Seguridad    
    private static string xsd_validacion;
    private static string xsd_error_code;
    X509Certificate2 certEmisor = new X509Certificate2();
    OpenSSL_Lib.cSello cSello;

    public clsTimbradoGeneracion()
    {
        DateTime Fecha = DateTime.Today;
        try
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsTimbradoGeneracion.AcceptAllCertificatePolicy);

            fnGenerarLlave();
        }
        catch (Exception ex)
        {
            clsLog.EscribirLog("Error al generar las llaves para el sello. " + ex.Message);
        }



    }

    public void fnCrearPlantillaEnvio(XmlDocument pxComprobante, string psTipoDocumento, string sRuta, string sNombreTxt)
    {
        if (!(sRuta == string.Empty))
        {
            clsPDFSharpNomina nomina = new clsPDFSharpNomina(pxComprobante);

            if (!string.IsNullOrEmpty(psTipoDocumento))
                nomina.fnGenerarPDF(sRuta, sNombreTxt);

            nomina = null;
        }
    }

    public void fnTimbradoGeneracion()
    {
        string[] Files = null;
        string RutaXMLDocs = Settings.Default.RutaDocs;
        string filtro = "*.txt";
        Files = Directory.GetFiles(RutaXMLDocs, filtro);

        foreach (string archivo in Files)
        {
            char[] cCad = { '-' };
            string[] sCad;
            int nBandera = 0;
            System.IO.StringReader lectorVersion;
            string noCertificado = string.Empty;
            string sNombreTXT = Path.GetFileNameWithoutExtension(archivo);
            string sText = string.Empty;
            string sXMLGenerado = string.Empty;
            string sXML = string.Empty;
            XmlDocument xDocumento = new XmlDocument();

            while ((clsLog.fnWaitForFile(archivo) == false))
            {
                //Se hace pato un rato.(Se espera a que se desbloquee el Archivo)
            }

            using (Stream stream = File.Open(archivo.ToString(), FileMode.Open))
            {
                StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                sText = sr.ReadToEnd();
                lectorVersion = new System.IO.StringReader(sText);
            }

            try
            {
                string sCorreo = string.Empty; ;

                sXMLGenerado = fnGenerarComprobante(sText, sNombreTXT, ref sCorreo);
                if (sXMLGenerado.Equals(string.Empty))
                {
                    File.Copy(archivo, Settings.Default.RutaDocInv + sNombreTXT + " - " + "Invalido" + " - " + String.Format("{0:dd-MM-yyyy}", DateTime.Now) + ".txt", true);
                    File.Delete(archivo);
                    continue;
                }

                xDocumento.LoadXml(sXMLGenerado);
                System.Threading.Thread.Sleep(100);

                try
                {
                    sXML = wsRecepcionT.fnEnviarXML(xDocumento.OuterXml, "Recibo de Nomina", 0, Settings.Default.Usuario, Settings.Default.Password, "3.2");
                }
                catch (Exception ex)
                {
                    sXML = "Error al momento de timbrar el comprobante" + " - " + ex.Message;
                }



                //Se valida el tipo de respuesta
                sCad = sXML.Split(cCad);
                if (sCad.Length <= 3)
                {
                    nBandera = 1; //Se indica que el comprobante no fue timbrado
                    //En caso de marcar error se graba un log
                    clsLog.EscribirLog(sXML + " - " + sNombreTXT);
                }

                //Si el documento fue timbrado se guarda el XML en ruta definida
                if (nBandera == 0)
                {
                    //Se obtiene el xml
                    XmlDocument xXML = new XmlDocument();
                    xXML.LoadXml(sXML);
                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xXML.NameTable);
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                    XPathNavigator navEncabezado = xXML.CreateNavigator();

                    var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaDocZips, sNombreTXT);

                    File.WriteAllText(rutaAbsolutaAcuse, xXML.OuterXml);
                    string sRutaPDF = Settings.Default.RutaPDF + sNombreTXT + ".pdf";

                    try
                    {
                        //Generar PDF
                        fnCrearPlantillaEnvio(xXML, "Nomina", sRutaPDF, sNombreTXT);
                    }
                    catch (Exception ex)
                    {
                        clsLog.EscribirLog("No se pudo generar la representación impresa del CFDI - " + sRutaPDF + ex.Message);
                        clsLog.EscribirLog(rutaAbsolutaAcuse);
                    }

                    File.Copy(archivo, Settings.Default.RutaTxtGen + sNombreTXT + " - " + String.Format("{0:dd-MM-yyyy}", DateTime.Now) + ".txt", true);
                    File.Delete(archivo);
                  
                }
                else
                {
                    if (sXML.Contains("Error al momento de timbrar el comprobante"))
                    {
                        File.Copy(archivo, Settings.Default.RutaDesconexion + sNombreTXT + ".txt", true);
                        File.Delete(archivo);
                    }
                    else
                    {
                        File.Copy(archivo, Settings.Default.RutaDocInv + sNombreTXT + " - " + "Invalido" + " - " + String.Format("{0:dd-MM-yyyy}", DateTime.Now) + ".txt", true);
                        File.Delete(archivo);
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Ha ocurrido un error al generar el comprobante - " + ex.Message);
                File.Copy(archivo, Settings.Default.RutaDocInv + sNombreTXT + " - " + "Invalido" + " - " + String.Format("{0:dd-MM-yyyy}", DateTime.Now) + ".txt", true);
                File.Delete(archivo);
            }
        }
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    /// <summary>
    /// Revisa que la fecha del certificado sea valida
    /// </summary>
    /// <returns></returns>
    private bool fnComprobarFechas()
    {
        bool bResultado = true;

        if (certEmisor.NotBefore.CompareTo(DateTime.Today) > 0 || certEmisor.NotAfter.CompareTo(DateTime.Today) < 0)
            return false;

        return bResultado;
    }

    /// <summary>
    /// Función que contruye la cadena original
    /// </summary>
    /// <param name="xml">Documento</param>
    /// <param name="psNombreArchivoXSLT">Nombre del archivo de tranformación</param>
    /// <returns></returns>
    private string fnConstruirCadenaTimbrado(IXPathNavigable xml)
    {
        string sCadenaOriginal = string.Empty;
        MemoryStream ms;
        StreamReader srDll;
        XsltArgumentList args;
        XslCompiledTransform xslt;
        try
        {
            xslt = new XslCompiledTransform();
            xslt.Load(typeof(CaOri.V32));
            ms = new MemoryStream();
            args = new XsltArgumentList();
            xslt.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            srDll = new StreamReader(ms);
            sCadenaOriginal = srDll.ReadToEnd();
        }
        catch (Exception ex)
        {
            clsLog.EscribirLog("Error al generar la cadena original. " + ex.Message);
            throw new Exception(DateTime.Now + " " + "Error al generar la cadena original." + " " + ex.Message);

        }
        return sCadenaOriginal;
    }

    /// <summary>
    /// Función que revisa que el certificado no sea apocrifo
    /// </summary>
    /// <returns></returns>
    private bool fnCSD308()
    {
        bool bRetorno = false;
        try
        {
            if (certEmisor.IssuerName.Name.Contains("A.C. del Servicio de Administración Tributaria"))
                bRetorno = true;
            else
                bRetorno = false;
        }
        catch (Exception)
        {

        }
        return bRetorno;
    }

    /// <summary>
    /// Función que genera las llaves para la generación del sello
    /// </summary>
    private void fnGenerarLlave()
    {
        //Obtener la Llave Privada del Emisor
        string[] FileKey = null;
        string RutaKey = (String)Settings.Default.RutaCertificados + "\\";
        string filtroKey = "*.key";
        FileKey = Directory.GetFiles(RutaKey, filtroKey);

        foreach (string filekey in FileKey)
        {
            Stream streamkey = File.Open(filekey.ToString(), FileMode.Open);
            StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
            using (BinaryReader br = new BinaryReader(streamkey))
            {
                gLlave = br.ReadBytes(Convert.ToInt32(streamkey.Length));
            }
        }



        //Obtener el Password del Certificado Emisor
        string[] FilePwd = null;
        string RutaPwd = (String)Settings.Default.RutaCertificados + "\\";
        string filtroPwd = "*.txt";
        FilePwd = Directory.GetFiles(RutaPwd, filtroPwd);

        foreach (string filePwd in FilePwd)
        {
            using (Stream streamPwd = File.Open(filePwd.ToString(), FileMode.Open))
            {
                StreamReader srPwd = new StreamReader(streamPwd, System.Text.Encoding.UTF8, true);
                gsPassword = srPwd.ReadToEnd();
            }
        }



        //Obtener el Certificado del Emisor
        string[] FilesCer = null;
        string RutaCert = (String)Settings.Default.RutaCertificados + "\\";
        string filtroCert = "*.cer";
        FilesCer = Directory.GetFiles(RutaCert, filtroCert);

        foreach (string filecer in FilesCer)
        {
            certEmisor.Import(filecer);
        }


        //Llave del Emisor
        //*****************************
        //Se cambiaria el metodo de sellado por OpenSSL
        cSello = new cSello(FileKey[0], FilePwd[0], Settings.Default.RutaCertificados + @"\");
        //
        //*****************************

    }

    /// <summary>
    /// Función que genera el comprobante
    /// </summary>
    /// <param name="sLayout">Layout</param>
    /// <returns></returns>
    private string fnGenerarComprobante(string sLayout, string sNombreLayout, ref string sCorreo)
    {

        int x = 0;

        int nBandera = 0;
        DateTime Fecha = DateTime.Today;
        string sCadenaOriginalEmisor = String.Empty;
        string linea = string.Empty;
        string lineaVersion = string.Empty;
        string noCertificado = string.Empty;
        string numeroCertificado = string.Empty;
        string sSello = string.Empty;

        string[] atributosVersionSeccion1 = null;
        string[] seccionVersion = null;

        System.IO.StringReader lectorVersion;
        XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
        XPathNavigator navNodoTimbre;
        XmlDocument xDocumento = new XmlDocument();


        Comprobante Cfd = new Comprobante();

        ComprobanteEmisor CEmisor = new ComprobanteEmisor();
        CEmisor.DomicilioFiscal = new t_UbicacionFiscal();
        CEmisor.ExpedidoEn = new t_Ubicacion();

        ComprobanteEmisorRegimenFiscal CERegimen = new ComprobanteEmisorRegimenFiscal();
        List<ComprobanteEmisorRegimenFiscal> ListRegimen = new List<ComprobanteEmisorRegimenFiscal>();

        ComprobanteReceptor CReceptor = new ComprobanteReceptor();
        CReceptor.Domicilio = new t_Ubicacion();

        List<ComprobanteConcepto> ListConcepto = new List<ComprobanteConcepto>();
        ComprobanteConcepto CConcepto = new ComprobanteConcepto();

        ComprobanteImpuestos CImpuestos = new ComprobanteImpuestos();
        ComprobanteImpuestosRetencion impuestosRetencion = new ComprobanteImpuestosRetencion();
        List<ComprobanteImpuestosRetencion> listaImpRetencion = new List<ComprobanteImpuestosRetencion>();


        Nomina CompNomina = new Nomina();

        NominaPercepciones nomPercepciones = new NominaPercepciones();
        List<NominaPercepcionesPercepcion> listaPercepciones = new List<NominaPercepcionesPercepcion>();

        NominaDeducciones nomDeducciones = new NominaDeducciones();
        List<NominaDeduccionesDeduccion> listaDeducciones = new List<NominaDeduccionesDeduccion>();

        List<NominaIncapacidad> listaIncapcidad = new List<NominaIncapacidad>();
        List<NominaHorasExtra> listaHorasExtra = new List<NominaHorasExtra>();

        string sCertificado = string.Empty;
        try
        {

            sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());
            byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
            numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();
        }
        catch { }


        try
        {
            lectorVersion = new System.IO.StringReader(sLayout);
            while (true)
            {
                lineaVersion = lectorVersion.ReadLine();
                if (string.IsNullOrEmpty(lineaVersion))
                    break;

                seccionVersion = lineaVersion.Split('?');

                try
                {

                    atributosVersionSeccion1 = seccionVersion[1].Split('|');

                    switch (seccionVersion[0])
                    {
                        case "co":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("version"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.version = valores[1];
                                }
                                if (arreglo.Contains("serie"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.serie = valores[1];
                                }
                                if (arreglo.Contains("folio"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.folio = valores[1];
                                }
                                if (arreglo.Split('@')[0].Equals("fecha"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.fecha = Convert.ToDateTime(valores[1]);
                                }


                                if (arreglo.Contains("formaDePago"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.formaDePago = valores[1];
                                }
                                if (arreglo.Contains("noCertificado"))
                                {
                                    Cfd.noCertificado = numeroCertificado;
                                }
                                if (arreglo.Contains("certificado"))
                                {
                                    Cfd.certificado = sCertificado;
                                }

                                if (arreglo.Contains("condicionesDePago"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.condicionesDePago = valores[1];
                                }

                                if (arreglo.Contains("subTotal"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.subTotal = Convert.ToDecimal(valores[1]);
                                }
                                if (arreglo.Contains("descuento"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.descuento = Convert.ToDecimal(valores[1]);
                                    Cfd.descuentoSpecified = true;
                                }
                                if (arreglo.Contains("motivoDescuento"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.motivoDescuento = valores[1];
                                }

                                if (arreglo.Contains("TipoCambio"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.TipoCambio = valores[1];
                                }

                                if (arreglo.Contains("Moneda"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.Moneda = valores[1];
                                }
                                if (arreglo.Contains("total"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.total = Convert.ToDecimal(valores[1]);
                                }
                                if (arreglo.Contains("tipoDeComprobante"))
                                {
                                    Cfd.tipoDeComprobante = ComprobanteTipoDeComprobante.egreso;
                                }
                                if (arreglo.Contains("metodoDePago"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.metodoDePago = valores[1];
                                }

                                if (arreglo.Contains("LugarExpedicion"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.LugarExpedicion = valores[1];
                                }

                                if (arreglo.Contains("NumCtaPago"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Cfd.NumCtaPago = valores[1];
                                }


                            }

                            break;

                        case "re":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("rfc"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.rfc = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("nombre"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.nombre = fnReplaceCaracters(valores[1]);
                                }

                            }

                            break;

                        case "de":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("calle"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.calle = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("noExterior"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.noExterior = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("noInterior"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.noInterior = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("colonia"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.colonia = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("localidad"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.localidad = fnReplaceCaracters(valores[1]);
                                }

                                if (arreglo.Contains("referencia"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.referencia = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("municipio"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.municipio = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("estado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.estado = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("pais"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.pais = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("codigoPostal"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.DomicilioFiscal.codigoPostal = fnReplaceCaracters(valores[1]);
                                }
                            }

                            break;

                        case "ee":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("calle"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.calle = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("noExterior"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.noExterior = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("noInterior"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.noInterior = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("colonia"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.colonia = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("localidad"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.localidad = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("referencia"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.referencia = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("municipio"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.municipio = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("estado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.estado = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("pais"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.pais = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("codigoPostal"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CEmisor.ExpedidoEn.codigoPostal = fnReplaceCaracters(valores[1]);
                                }
                            }

                            break;


                        case "rf":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("Regimen"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CERegimen.Regimen = fnReplaceCaracters(valores[1]);
                                    ListRegimen.Add(CERegimen);
                                }
                            }

                            break;


                        case "rr":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("rfc"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.rfc = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("nombre"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.nombre = fnReplaceCaracters(valores[1]);
                                }

                            }

                            break;

                        case "dr":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("calle"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.calle = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("noExterior"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.noExterior = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("noInterior"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.noInterior = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("colonia"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.colonia = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("localidad"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.localidad = fnReplaceCaracters(valores[1]);
                                }

                                if (arreglo.Contains("referencia"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.referencia = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("municipio"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.municipio = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("estado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.estado = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("pais"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.pais = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("codigoPostal"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CReceptor.Domicilio.codigoPostal = fnReplaceCaracters(valores[1]);
                                }

                            }

                            break;
                        case "cc":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("cantidad"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CConcepto.cantidad = Convert.ToDecimal(valores[1]);
                                }
                                if (arreglo.Contains("unidad"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CConcepto.unidad = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("noIdentificacion"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CConcepto.noIdentificacion = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("descripcion"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CConcepto.descripcion = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("valorUnitario"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CConcepto.valorUnitario = Convert.ToDecimal(valores[1]);
                                }
                                if (arreglo.Contains("importe"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CConcepto.importe = Convert.ToDecimal(valores[1]);
                                }
                            }
                            ListConcepto.Add(CConcepto);
                            break;

                        case "im":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("totalImpuestosRetenidos"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CImpuestos.totalImpuestosRetenidos = Convert.ToDecimal(valores[1]);
                                }
                            }

                            break;

                        case "ir":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("impuesto"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    impuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto.ISR;
                                }
                                if (arreglo.Contains("importe"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    impuestosRetencion.importe = Convert.ToDecimal(valores[1]);
                                }
                            }

                            listaImpRetencion.Add(impuestosRetencion);

                            break;

                        case "nom":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("Version"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.Version = valores[1];
                                }
                                if (arreglo.Contains("RegistroPatronal"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.RegistroPatronal = valores[1];
                                }
                                if (arreglo.Contains("NumEmpleado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.NumEmpleado = valores[1];
                                }
                                if (arreglo.Contains("CURP"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.CURP = valores[1];
                                }
                                if (arreglo.Contains("TipoRegimen"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.TipoRegimen = Convert.ToInt32(valores[1]);
                                }
                                if (arreglo.Contains("NumSeguridadSocial"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.NumSeguridadSocial = valores[1];
                                }
                                if (arreglo.Contains("FechaPago"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.FechaPago = Convert.ToDateTime(valores[1]);
                                }
                                if (arreglo.Contains("FechaInicialPago"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.FechaInicialPago = Convert.ToDateTime(valores[1]);
                                }
                                if (arreglo.Contains("FechaFinalPago"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.FechaFinalPago = Convert.ToDateTime(valores[1]);
                                }
                                if (arreglo.Contains("NumDiasPagados"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.NumDiasPagados = Convert.ToDecimal(valores[1]);
                                }
                                if (arreglo.Contains("Departamento"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.Departamento = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("CLABE"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.CLABE = valores[1];
                                }
                                if (arreglo.Contains("Banco"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.Banco = fnReplaceCaracters(valores[1]);
                                    CompNomina.BancoSpecified = true;
                                }
                                if (arreglo.Contains("FechaInicioRelLaboral"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.FechaInicioRelLaboral = Convert.ToDateTime(valores[1]);
                                    CompNomina.FechaInicioRelLaboralSpecified = true;
                                }
                                if (arreglo.Contains("Antiguedad"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.Antiguedad = Convert.ToInt32(valores[1]);
                                    CompNomina.AntiguedadSpecified = true;
                                }
                                if (arreglo.Split('@')[0].Equals("Puesto"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.Puesto = fnReplaceCaracters(valores[1]);

                                }
                                if (arreglo.Contains("TipoContrato"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.TipoContrato = valores[1];
                                }
                                if (arreglo.Contains("TipoJornada"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.TipoJornada = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("PeriodicidadPago"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.PeriodicidadPago = fnReplaceCaracters(valores[1]);
                                }
                                if (arreglo.Contains("SalarioBaseCotApor"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.SalarioBaseCotApor = Convert.ToDecimal(valores[1]);
                                    CompNomina.SalarioBaseCotAporSpecified = true;
                                }
                                if (arreglo.Contains("RiesgoPuesto"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.RiesgoPuesto = Convert.ToInt32(valores[1]);
                                    CompNomina.RiesgoPuestoSpecified = true;
                                }
                                if (arreglo.Contains("SalarioDiarioIntegrado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    CompNomina.SalarioDiarioIntegrado = Convert.ToDecimal(valores[1]);
                                    CompNomina.SalarioDiarioIntegradoSpecified = true;
                                }
                            }

                            break;

                        case "percs":

                            foreach (string arreglo in atributosVersionSeccion1)
                            {

                                if (arreglo.Contains("TotalGravado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    nomPercepciones.TotalGravado = Convert.ToDecimal(valores[1]);
                                }
                                if (arreglo.Contains("TotalExento"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    nomPercepciones.TotalExento = Convert.ToDecimal(valores[1]);
                                }
                            }

                            break;

                        case "per":

                            NominaPercepcionesPercepcion Percepcion = new NominaPercepcionesPercepcion();

                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("TipoPercepcion"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Percepcion.TipoPercepcion = valores[1];
                                }
                                if (arreglo.Contains("Clave"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Percepcion.Clave = valores[1];
                                }
                                if (arreglo.Contains("Concepto"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Percepcion.Concepto = valores[1];
                                }
                                if (arreglo.Contains("ImporteGravado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Percepcion.ImporteGravado = Convert.ToDecimal(valores[1]);
                                }
                                if (arreglo.Contains("ImporteExento"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Percepcion.ImporteExento = Convert.ToDecimal(valores[1]);
                                }
                            }

                            listaPercepciones.Add(Percepcion);

                            break;

                        case "deducs":

                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("TotalGravado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    nomDeducciones.TotalGravado = Convert.ToDecimal(valores[1]);
                                }
                                if (arreglo.Contains("TotalExento"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    nomDeducciones.TotalExento = Convert.ToDecimal(valores[1]);
                                }
                            }

                            break;

                        case "dedu":

                            NominaDeduccionesDeduccion Deduccion = new NominaDeduccionesDeduccion();

                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("TipoDeduccion"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Deduccion.TipoDeduccion = valores[1];
                                }
                                if (arreglo.Contains("Clave"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Deduccion.Clave = valores[1];
                                }
                                if (arreglo.Contains("Concepto"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Deduccion.Concepto = valores[1];
                                }
                                if (arreglo.Contains("ImporteGravado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Deduccion.ImporteGravado = Convert.ToDecimal(valores[1]);
                                }

                                if (arreglo.Contains("ImporteExento"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Deduccion.ImporteExento = Convert.ToDecimal(valores[1]);
                                }
                            }

                            listaDeducciones.Add(Deduccion);

                            break;


                        case "inca":

                            NominaIncapacidad Incapacidad = new NominaIncapacidad();

                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("DiasIncapacidad"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Incapacidad.DiasIncapacidad = Convert.ToDecimal(valores[1]);
                                }
                                if (arreglo.Contains("TipoIncapacidad"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Incapacidad.TipoIncapacidad = Convert.ToInt32(valores[1]);
                                }
                                if (arreglo.Contains("Descuento"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    Incapacidad.Descuento = Convert.ToDecimal(valores[1]);
                                }
                            }

                            listaIncapcidad.Add(Incapacidad);

                            break;

                        case "hora":

                            NominaHorasExtra HoraExtra = new NominaHorasExtra();

                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("Dias"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    HoraExtra.Dias = Convert.ToInt32(valores[1]);
                                }
                                if (arreglo.Contains("TipoHoras"))
                                {
                                    string[] valores = arreglo.Split('@');

                                    switch (valores[1].ToString())
                                    {
                                        case "Dobles":
                                            HoraExtra.TipoHoras = NominaHorasExtraTipoHoras.Dobles;
                                            break;
                                        case "Triples":
                                            HoraExtra.TipoHoras = NominaHorasExtraTipoHoras.Triples;
                                            break;
                                        default:
                                            break;
                                    }

                                }
                                if (arreglo.Contains("HorasExtra"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    HoraExtra.HorasExtra = Convert.ToInt32(valores[1]);
                                }

                                if (arreglo.Contains("ImportePagado"))
                                {
                                    string[] valores = arreglo.Split('@');
                                    HoraExtra.ImportePagado = Convert.ToDecimal(valores[1]);
                                }
                            }


                            listaHorasExtra.Add(HoraExtra);

                            break;

                        case "ad":
                            sListaCorreos = "";
                            try
                            {
                                string[] primerCorreo;
                                for (int i = 0; i < atributosVersionSeccion1.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        primerCorreo = atributosVersionSeccion1[i].Split('@'); ;
                                        string sPrimerCorreo = primerCorreo[1] + "@" + primerCorreo[2];
                                        sListaCorreos += sPrimerCorreo;
                                    }
                                    else { sListaCorreos += "," + atributosVersionSeccion1[i]; }
                                    sbAdenda.Append("<Documento>");
                                    sbAdenda.Append("<DatosEmail>");
                                    sbAdenda.Append("<correo>");
                                    sbAdenda.Append(sListaCorreos);
                                    sbAdenda.Append("</correo>");
                                    sbAdenda.Append("</DatosEmail>");
                                    sbAdenda.Append("</Documento>");
                                }

                            }
                            catch
                            {
                                clsLog.EscribirLog("Algo salio mal al obtener lista de correos " + sListaCorreos);
                            }
                            break;

                    }
                }
                catch (Exception ex)
                {
                    nBandera = 1;
                    clsLog.EscribirLog("Error Layout - " + ex.Message);
                    return string.Empty;
                }
            }

            lectorVersion.Close();

            if (!nBandera.Equals(0))
            {
                return string.Empty;
            }

            //Revisa el modo de ejecución
            if (Settings.Default.Modo.Equals("P"))
            {
                //Valida certificado sea vigente
                if (!fnComprobarFechas())
                {
                    clsLog.EscribirLog("El certificado está fuera de fecha");
                    return string.Empty;
                }

                //Valida certificado
                if (!fnCSD308())
                {
                    clsLog.EscribirLog("308 - Certificado no expedido por el SAT");
                    return string.Empty;
                }
            }



            if (listaImpRetencion.Count > 0)
            {
                CImpuestos.Retenciones = listaImpRetencion.ToArray();
                CImpuestos.totalImpuestosRetenidosSpecified = true;
                CImpuestos.totalImpuestosRetenidos = CImpuestos.totalImpuestosRetenidos;
            }

            if (listaPercepciones.Count > 0)
            {
                nomPercepciones.Percepcion = listaPercepciones.ToArray();
                CompNomina.Percepciones = nomPercepciones;
            }

            if (listaDeducciones.Count > 0)
            {
                nomDeducciones.Deduccion = listaDeducciones.ToArray();
                CompNomina.Deducciones = nomDeducciones;
            }

            if (listaIncapcidad.Count > 0)
            {
                CompNomina.Incapacidades = listaIncapcidad.ToArray();
            }

            if (listaHorasExtra.Count > 0)
            {
                CompNomina.HorasExtras = listaHorasExtra.ToArray();
            }

            if (ListConcepto.Count > 0)
            {
                Cfd.Conceptos = ListConcepto.ToArray();
            }

            if (ListRegimen.Count > 0)
            {
                CEmisor.RegimenFiscal = ListRegimen.ToArray();
            }

            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);

            XmlDocument xmlComplNomina = new XmlDocument();
            XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
            sns.Add("nomina", "http://www.sat.gob.mx/nomina");

            XmlSerializer serializador = new XmlSerializer(typeof(Nomina));
            serializador.Serialize(sw, CompNomina, sns);


            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            xmlComplNomina.LoadXml(sr.ReadToEnd());

            XmlElement xeComplNomina = xmlComplNomina.DocumentElement;

            ComprobanteComplemento complNomina = new ComprobanteComplemento();
            XmlElement[] axeComplNomina = new XmlElement[] { xeComplNomina };
            complNomina.Any = axeComplNomina;


            Cfd.Complemento = complNomina;
            Cfd.Emisor = CEmisor;
            Cfd.Receptor = CReceptor;
            Cfd.Impuestos = CImpuestos;

            //-----------------------------------------------------------------------------------------------------------------------

            string tNameSpace = "nomina" + "|" + "http://www.sat.gob.mx/nomina" + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina11.xsd";
            xDocumento = fnGenerarXML32(Cfd, tNameSpace);

            navNodoTimbre = xDocumento.CreateNavigator();
            sCadenaOriginalEmisor = fnConstruirCadenaTimbrado(navNodoTimbre);
            cSello.sCadenaOriginal = sCadenaOriginalEmisor;
            Cfd.sello = cSello.sSello;


            //Valida sello
            if (!fnVerificarSello(sCadenaOriginalEmisor, Cfd.sello))
            {
                clsLog.EscribirLog("Sello Incorrecto");
                return string.Empty;
            }


            xDocumento = fnGenerarXML32(Cfd, tNameSpace);


        }
        catch (Exception ex)
        {
            clsLog.EscribirLog(sNombreLayout + " - " + ex.Message);
            return string.Empty;
        }
        return xDocumento.OuterXml;
    }

    /// <summary>
    /// Delegado para la validación del xml con el esquema
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void SchemaValidationHandler(object sender, ValidationEventArgs e)
    {

        switch (e.Severity)
        {
            case XmlSeverityType.Error:

                if (!e.Message.Contains("TimbreFiscalDigital"))
                {
                    xsd_error_code = xsd_error_code + e.Message;
                }

                break;
        }
    }

    /// <summary>
    /// Función que se encarga de validar el xml con el esquema
    /// </summary>
    /// <param name="xXmlSalida">Comprobante</param>
    /// <returns></returns>
    private bool fnValidarEsquema(XmlDocument xXmlSalida)
    {
        bool bResultado = false;
        try
        {
            xsd_validacion = string.Empty;
            xsd_error_code = string.Empty;
            xsd_validacion = fnValidate(xXmlSalida);//"esquema_v3");
            if (xsd_validacion != string.Empty && xsd_validacion != null)
            {
                throw new System.ArgumentException("333 - " + xsd_validacion, "valida esquema");
            }
            bResultado = true;
        }
        catch (Exception)
        {
            throw new System.ArgumentException("Archivo XML Incorrecto " + xsd_validacion, "Archivo no Cumple Con Esquema");
        }
        return bResultado;
    }

    /// <summary>
    /// Función que valida el comprobante contra el esquema
    /// </summary>
    /// <param name="psXmlDocument">Comprobante</param>
    /// <returns></returns>
    public string fnValidate(XmlDocument psXmlDocument)
    {
        string retorno = string.Empty;
        try
        {
            ValidationEventHandler validation = new ValidationEventHandler(SchemaValidationHandler);
            psXmlDocument.Validate(validation);

            if (xsd_error_code != string.Empty)
            {
                retorno = xsd_error_code;
            }
        }
        catch (Exception)
        {
            return "Revise el esquema del XML y reintente de nuevo.";
        }
        return retorno;
    }

    /// <summary>
    /// Comprueba que el sello del comprobante refleje los datos de la cadena original
    /// </summary>
    /// <param name="psCadenaOriginal">Cadena original del comprobante</param>
    /// <returns>Booleano indicando si la cadena original corresponde al sello</returns>
    public bool fnVerificarSello(string psCadenaOriginal, string psSello)
    {
        RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certEmisor.PublicKey.Key);
        try
        {
            //Verificamos que el certificado obtenga el mismo resultado que el del sello
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(psCadenaOriginal));
            bool exito = publica.VerifyHash(
                    hash,
                    "sha1",
                    Convert.FromBase64String(psSello));

            return exito;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Anexo 20 Eliminar en la reglas de estructura.
    /// </summary>
    /// <param name="varRep"></param>
    /// <returns></returns>
    public static string fnReplaceCaracters(string varRep)
    {
        string sReplace = string.Empty;

        if (varRep.Contains('&'))
        {
            varRep.Replace("&", "&amp;");
        }

        if (varRep.Contains('<'))
        {
            varRep.Replace("<", "&lt;");
        }

        if (varRep.Contains('>'))
        {
            varRep.Replace(">", "&gt;");
        }

        if (varRep.Contains("'"))
        {
            varRep.Replace("'", "&apos;");
        }

        if (varRep.Contains("\""))
        {
            varRep.Replace("\"", "&quot;");
        }

        sReplace = varRep;
        return sReplace;
    }

    /// <summary>
    /// Genera el XML en base a la estructura que contiene los datos version 3.2
    /// </summary>
    /// <param name="datos">Estructura que contiene los datos</param>
    /// <returns>XmlDocument con los datos del objeto Comprobante</returns>
    public XmlDocument fnGenerarXML32(Comprobante datos, string tNamespace)
    {
        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
        XmlDocument xXml = new XmlDocument();
        XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
        sns.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
        string[] pspace = { "" };
        if (!(tNamespace == null))
        {
            pspace = tNamespace.Split('|');
            if (pspace.Length > 1)
            {
                sns.Add(pspace[0], pspace[1]);
            }
        }
        XmlSerializer serializador = new XmlSerializer(typeof(Comprobante));
        try
        {
            serializador.Serialize(sw, datos, sns);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);

            xXml.LoadXml(sr.ReadToEnd());
            if (!(tNamespace == null))
            {
                XmlAttribute att = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                att.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd" + " " + pspace[1] + " " + pspace[2];
                xXml.DocumentElement.SetAttributeNode(att);
            }
            else
            {
                XmlAttribute att = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                att.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
                xXml.DocumentElement.SetAttributeNode(att);

            }


            return xXml;
        }
        catch (Exception ex)
        {
            clsLog.EscribirLog("fnGenerarXML32 - " + ex.Message);
            return xXml;
        }
    }


}


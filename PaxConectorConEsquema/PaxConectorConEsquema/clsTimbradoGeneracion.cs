using cfdv32;
using PaxConectorConEsquema.Properties;
using PaxConectorConEsquema.wcfRecepcionASMX;
using PaxConectorConEsquema.wsLicenciaASMX;
using Microsoft.Win32;
using OpenSSL_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Text.RegularExpressions;

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

    #endregion

    private bool bValida { get; set; }
    DataTable tblComplementos;
    XmlNamespaceManager nsmComprobante = null;
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
            clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "Error al generar las llaves para el sello. Asegurese de que las llaves esten en la carpeta de certificados y que la contraseña sea la correcta. " + ex.Message);
        }
    }

    public void fnTimbradoGeneracion()
    {
        string[] Files = null;
        string RutaXMLDocs = Settings.Default.rutaDocs;
        string filtro = "*.txt";
        Files = Directory.GetFiles(RutaXMLDocs, filtro);
        DateTime Fecha = DateTime.Today;

        foreach (string archivo in Files)
        {
            fnRecuperarEsquemas();
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

            while ((fnWaitForFile(archivo) == false))
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
                sXMLGenerado = fnGenerarComprobante(sText, sNombreTXT);
                if (sXMLGenerado.Equals(string.Empty))
                {
                    //Si el txt es invalido
                    //Copia el archivo txt invalido a otra carpeta
                    File.Copy(archivo, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(archivo);
                    continue;
                    //return;
                }

                //xDocumento.LoadXml(sXMLGenerado);

                ///////RETIRAR ADDENDA///////
                XmlDocument xDocSinAddenda = new XmlDocument(); //Sera enviado al servicio
                xDocSinAddenda.LoadXml(sXMLGenerado);

                //Quitamos el nodo de la addenda del XMLDocument
                XmlNamespaceManager nsmCom = new XmlNamespaceManager(xDocSinAddenda.NameTable);
                nsmCom.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmCom.AddNamespace("fomadd", "http://www.ford.com.mx/cfdi/addenda");
                nsmCom.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                bool bAddenda = true;
                bool Validacion = true;

                String xAddenda = "";
                try
                {
                    xAddenda = xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmCom).OuterXml;

                    XmlDocument AddendaFOB = new XmlDocument();
                    AddendaFOB.LoadXml(xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmCom).InnerXml);


                    XmlNamespaceManager nsmComAd = new XmlNamespaceManager(AddendaFOB.NameTable);
                    nsmComAd.AddNamespace("fomadd", "http://www.ford.com.mx/cfdi/addenda");

                    string Ad = AddendaFOB.OuterXml;

                    

                    xsd_validacion = string.Empty;
                    xsd_error_code = string.Empty;
                    xsd_validacion = fnValidate(AddendaFOB, 1);//"esquema_v3");
                    if (xsd_validacion != string.Empty && xsd_validacion != null)
                    {
                        Validacion = false;
                        throw new System.ArgumentException("333 - " + xsd_validacion, "valida esquema.");
                    }

                    xAddenda = xAddenda.Replace(" xmlns:cfdi=\"http://www.sat.gob.mx/cfd/3\"",string.Empty);
                    xAddenda = xAddenda.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", string.Empty);

                    xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmCom).DeleteSelf();
                }
                catch
                {
                    bAddenda = false;

                }
                string postData = xDocSinAddenda.OuterXml;
                ////////////////////////////


                System.Threading.Thread.Sleep(500);

                try 
                {
                    //Se manda el xml a timbrar
                    if (Validacion == true)
                    {
                        sXML = wsRecepcionT.fnEnviarXML(xDocSinAddenda.OuterXml, Settings.Default.tipodocto, 0, Settings.Default.usuario, Settings.Default.password, "3.2");
                    }
                    else
                    {
                        sXML = "Error en addenda";
                    }
                }
                catch (Exception ex)
                {
                    sXML = "Error al momento de timbrar el comprobante." + " " + ex.Message;
                    
                }


                //Se valida el tipo de respuesta
                sCad = sXML.Split(cCad);
                if (sCad.Length <= 2)
                {
                    nBandera = 1; //Se indica que el comprobante no fue timbrado
                    //En caso de marcar error se graba un log
                    
                    if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                        clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + sXML + ", Nombre txt: " + sNombreTXT);
                    else
                        clsLog.Escribir(Settings.Default.LogError + "LogErrorSinTimbrar" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + sXML + ", Nombre txt: " + sNombreTXT);
                }

                //Si el documento fue timbrado se guarda el XML en ruta definida
                if (nBandera == 0)
                {
                    //Se obtiene el xml
                    XmlDocument xXML = new XmlDocument();
                    try
                    {
                        xXML.LoadXml(sXML);
                    }
                    catch (Exception ex)
                    {
                        //Si al cargar el comprobante marcar error
                        clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + sXML + ", Nombre txt: " + sNombreTXT + ". Respuesta: " + xXML + " " + ex.Message);
                        //Copia el archivo txt invalido a otra carpeta
                        File.Copy(archivo, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                        //Elimina el archivo txt
                        File.Delete(archivo);
                        continue;
                    }
                    //Se vuelve a agregar la addenda si existe
                    if (bAddenda)
                        xXML.CreateNavigator().SelectSingleNode("cfdi:Comprobante", nsmCom).AppendChild(xAddenda);
                    ////////////////////////////////////////////
                    XPathNavigator navEncabezado = xXML.CreateNavigator();

                    //Se obtiene el UUID
                    //try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                    //catch { snombreDoc = Guid.NewGuid().ToString(); }

                    //Se guarda XML en ruta especificada 
                    //xXML.Save(Settings.Default.RutaDocZips + sNombreTXT + ".xml");

                    var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaDocZips, sNombreTXT);
                    //AccesoDisco.GuardarArchivoTexto(rutaAbsolutaAcuse, xXML.OuterXml);
                    File.WriteAllText(rutaAbsolutaAcuse, xXML.OuterXml);

                    try
                    {
                        //Generar PDF
                        string sRutaPDF = Settings.Default.RutaDocZips;
                        fnCrearPlantillaEnvio(xXML, Settings.Default.tipodocto, sRutaPDF + sNombreTXT + ".pdf");
                    }
                    catch (Exception ex)
                    {
                        clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "No se pudo generar la representación impresa del CFDI. " + ex.Message);
                    }

                    //Se guarda log de comprobantes timbrados
                    clsLog.Escribir(Settings.Default.LogTimbrados + "LogTimbrados", DateTime.Now + ", Nombre txt: " + sNombreTXT);

                    //Copia el archivo txt timbrado a otra carpeta
                    File.Copy(archivo, Settings.Default.rutaTXTGen + sNombreTXT + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(archivo);
                }
                else
                {
                    if (sXML.Contains("Error al momento de timbrar el comprobante"))
                    {
                        //Si el txt es invalido y fue por una problema en la llamada al servicio
                        //Copia el archivo txt invalido a otra carpeta
                        File.Copy(archivo, Settings.Default.rutaDesconexion + sNombreTXT + ".txt", true);
                        //Elimina el archivo txt
                        File.Delete(archivo);
                    }
                    else
                    {
                        //Si el txt es invalido
                        //Copia el archivo txt invalido a otra carpeta
                        File.Copy(archivo, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                        //Elimina el archivo txt
                        File.Delete(archivo);
                    }
                }
            }
            catch(Exception ex)
            {
                if(!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                    clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "Ha ocurrido un error al generar el comprobante. " + ex.Message);
                else
                    clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "Ha ocurrido un error al generar el comprobante. " + ex.Message);

                //Si el txt es invalido
                //Copia el archivo txt invalido a otra carpeta
                File.Copy(archivo, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                //Elimina el archivo txt
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
    private string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
    {
        string sCadenaOriginal = string.Empty;
        MemoryStream ms;
        StreamReader srDll;
        XsltArgumentList args;
        XslCompiledTransform xslt;
        try
        {
            xslt = new XslCompiledTransform();
            xslt.Load(typeof(CaOri.V3210));
            ms = new MemoryStream();
            args = new XsltArgumentList();
            xslt.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            srDll = new StreamReader(ms);
            sCadenaOriginal = srDll.ReadToEnd();
        }
        catch (Exception ex)
        {
            throw new Exception(DateTime.Now + " " + "Error al generar la cadena original." + " " + ex.Message);
            //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message);
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
    /// Crear elementos Raiz del Documento en Version 3.0
    /// </summary>
    /// <param name="pxDocumento">Documento</param>
    /// <param name="psElemento">Elemento</param>
    /// <param name="pasAtributos">Atributos</param>
    /// <returns></returns>
    private XmlElement fnCrearElemento(XmlDocument pxDocumento, string psElemento, string[] pasAtributos)
    {
        XmlAttribute xAttr;
        XmlElement elemento = pxDocumento.CreateElement("cfdi", psElemento, "http://www.sat.gob.mx/cfd/3");
        foreach (string a in pasAtributos)
        {
            string[] valores = a.Split('@');
            xAttr = pxDocumento.CreateAttribute(valores[0]);
            xAttr.Value = valores[1];
            elemento.Attributes.Append(xAttr);
        }
        return elemento;
    }

    /// <summary>
    /// Crear elementos Raiz del Documento en Version 3.2
    /// </summary>
    /// <param name="pxDocumento">Documento</param>
    /// <param name="pasAtributos">Atributos</param>
    private void fnCrearElementoRoot32(XmlDocument pxDocumento, string[] pasAtributos)
    {
        XmlAttribute xAttr;
        foreach (string a in pasAtributos)
        {
            string[] valores = a.Split('@');
            xAttr = pxDocumento.CreateAttribute(valores[0]);
            xAttr.Value = valores[1];
            pxDocumento.DocumentElement.Attributes.Append(xAttr);
        }
        xAttr = pxDocumento.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
        xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
        pxDocumento.DocumentElement.Attributes.Append(xAttr);
    }

    /// <summary>
    /// Crea archivo pdf segun plantilla configurada para su posterior envio de correo
    /// </summary>
    /// <param name="pxComprobante"></param>
    /// <param name="psTipoDocumento"></param>
    /// <param name="sRuta"></param>
    /// 

    //***********************************Para crear el pdf, descomentar en caso de ser necesario**********************************
    public void fnCrearPlantillaEnvio(XmlDocument pxComprobante, string psTipoDocumento, string sRuta)
    {
        if (!(sRuta == string.Empty))
        {
              clsPlantillaConector pdf = new clsPlantillaConector(pxComprobante);

            if (!string.IsNullOrEmpty(psTipoDocumento))
                pdf.TipoDocumento = psTipoDocumento.ToUpper();
            pdf.fnGenerarPDFSave(sRuta, "Black");
        }
    }
    //******************************************************************************************************************************/

    /// <summary>
    /// Función que genera las llaves para la generación del sello
    /// </summary>
    private void fnGenerarLlave()
    {
        //Obtener la Llave Privada del Emisor
        string[] FileKey = null;
        string RutaKey = (String)Settings.Default.rutaCertificados + "\\";
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
        //Obtener la Llave Privada del Emisor

        //Obtener el Password del Certificado Emisor
        string[] FilePwd = null;
        string RutaPwd = (String)Settings.Default.rutaCertificados + "\\";
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
        //Obtener el Password del Certificado Emisor

        //Obtener el Certificado del Emisor
        string[] FilesCer = null;
        string RutaCert = (String)Settings.Default.rutaCertificados + "\\";
        string filtroCert = "*.cer";
        FilesCer = Directory.GetFiles(RutaCert, filtroCert);

        foreach (string filecer in FilesCer)
        {
            certEmisor.Import(filecer);
        }
        //Obtener el Certificado del Emisor
        //Llave del Emisor
        //*****************************
        //Se cambiaria el metodo de sellado por OpenSSL
        cSello = new cSello(FileKey[0], FilePwd[0], Settings.Default.rutaCertificados + @"\");
        //
        //*****************************
        //Llave del Emisor
    }

    /// <summary>
    /// Función que genera el comprobante
    /// </summary>
    /// <param name="sLayout">Layout</param>
    /// <returns></returns>
    private string fnGenerarComprobante(string sLayout, string sNombreLayout)
    {
        int nBandera = 0;
        DateTime Fecha = DateTime.Today;
        string sCadenaOriginalEmisor = String.Empty;
        string linea = string.Empty;
        string lineaVersion = string.Empty;
        string noCertificado = string.Empty;
        string numeroCertificado = string.Empty;
        string sSello = string.Empty;
        string[] atributos = null;
        string[] atributosAduana = null;
        string[] seccion = null;
        string[] atributosVersionSeccion1 = null;
        string[] seccionVersion = null;
        StringReader lector;
        System.IO.StringReader lectorVersion;
        XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
        XPathNavigator navNodoTimbre;
        XmlDocument xDocumento = new XmlDocument();
        XmlDocument xXmlSalida;
        XmlNode impuestos = null;
        XmlNode padre = null;
        XmlNode padreConcepto = null;
        List<string[]> listaAddenda = new List<string[]>();

        Comprobante cComprobante = new Comprobante();
        ComprobanteEmisor cComprobanteEmisor = new ComprobanteEmisor();
        cComprobanteEmisor.DomicilioFiscal = new t_UbicacionFiscal();
        cComprobanteEmisor.ExpedidoEn = new t_Ubicacion();
        ComprobanteEmisorRegimenFiscal cComprobanteEmisorRegimenFiscal = new ComprobanteEmisorRegimenFiscal();
        List<ComprobanteEmisorRegimenFiscal> lComprobanteEmisorRegimenFiscal = new List<ComprobanteEmisorRegimenFiscal>();
        ComprobanteReceptor cComprobanteReceptor = new ComprobanteReceptor();
        cComprobanteReceptor.Domicilio = new t_Ubicacion();
        ComprobanteConcepto cComprobanteConcepto = new ComprobanteConcepto();
        List<ComprobanteConcepto> lComprobanteConcepto = new List<ComprobanteConcepto>();
        ComprobanteImpuestos cComprobanteImpuestos = new ComprobanteImpuestos();
        ComprobanteImpuestosRetencion cComprobanteImpuestosRetencion = new ComprobanteImpuestosRetencion();
        List<ComprobanteImpuestosRetencion> lComprobanteImpuestosRetencion = new List<ComprobanteImpuestosRetencion>();
        ComprobanteImpuestosTraslado cComprobanteImpuestosTraslado = new ComprobanteImpuestosTraslado();
        List<ComprobanteImpuestosTraslado> lComprobanteImpuestosTraslado = new List<ComprobanteImpuestosTraslado>();

        try
        {
            lector = new System.IO.StringReader(sLayout);

            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("fomadd", "http://www.ford.com.mx/cfdi/addenda");
            nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            
            xDocumento = new XmlDocument(nsm.NameTable);
            xDocumento.CreateXmlDeclaration("1.0", "UTF-8", "no");
            xDocumento.AppendChild(xDocumento.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));

            while (true)
            {
                linea = lector.ReadLine();
                if (string.IsNullOrEmpty(linea))
                    break;

                seccion = linea.Split('?');

                try
                {
                    atributos = seccion[1].Split('|');
                    foreach (string arreglo in atributos)
                    {
                        string[] elementos = arreglo.Split('@');
                        switch (seccion[0])
                        {
                            case "co":
                                switch (elementos[0])
                                {
                                    case "version":
                                        cComprobante.version = elementos[1];
                                        continue;
                                    case "serie":
                                        cComprobante.serie = elementos[1];
                                        continue;
                                    case "folio":
                                        cComprobante.folio = elementos[1];
                                        continue;
                                    case "fecha":
                                        cComprobante.fecha = DateTime.Parse(elementos[1]);
                                        continue;
                                    case "sello":
                                        cComprobante.sello = elementos[1];
                                        continue;
                                    case "formaDePago":
                                        cComprobante.formaDePago = elementos[1];
                                        continue;
                                    case "noCertificado":
                                        cComprobante.noCertificado = elementos[1];
                                        continue;
                                    case "certificado":
                                        cComprobante.certificado = elementos[1];
                                        continue;
                                    case "subTotal":
                                        cComprobante.subTotal = decimal.Parse(elementos[1]);
                                        continue;
                                    case "Moneda":
                                        cComprobante.Moneda = elementos[1];
                                        continue;
                                    case "total":
                                        cComprobante.total = decimal.Parse(elementos[1]);
                                        continue;
                                    case "tipoDeComprobante":
                                        switch (elementos[1])
                                        {
                                            case "ingreso":
                                                cComprobante.tipoDeComprobante = ComprobanteTipoDeComprobante.ingreso;
                                                continue;
                                            case "egreso":
                                                cComprobante.tipoDeComprobante = ComprobanteTipoDeComprobante.egreso;
                                                continue;
                                            case "traslado":
                                                cComprobante.tipoDeComprobante = ComprobanteTipoDeComprobante.traslado;
                                                continue;
                                        }
                                        continue;
                                    case "LugarExpedicion":
                                        cComprobante.LugarExpedicion = elementos[1];
                                        continue;
                                    case "metodoDePago":
                                        cComprobante.metodoDePago = elementos[1];
                                        continue;
                                }
                                break;
                            case "re":
                                switch (elementos[0])
                                {
                                    case "rfc":
                                        cComprobanteEmisor.rfc = elementos[1];
                                        continue;
                                    case "nombre":
                                        cComprobanteEmisor.nombre = elementos[1];
                                        continue;
                                }
                                break;
                            case "de":
                                switch (elementos[0])
                                {
                                    case "calle":
                                        cComprobanteEmisor.DomicilioFiscal.calle = elementos[1];
                                        continue;
                                    case "noExterior":
                                        cComprobanteEmisor.DomicilioFiscal.noExterior = elementos[1];
                                        continue;
                                    case "noInterior":
                                        cComprobanteEmisor.DomicilioFiscal.noInterior = elementos[1];
                                        continue;
                                    case "colonia":
                                        cComprobanteEmisor.DomicilioFiscal.colonia = elementos[1];
                                        continue;
                                    case "localidad":
                                        cComprobanteEmisor.DomicilioFiscal.localidad = elementos[1];
                                        continue;
                                    case "municipio":
                                        cComprobanteEmisor.DomicilioFiscal.municipio = elementos[1];
                                        continue;
                                    case "estado":
                                        cComprobanteEmisor.DomicilioFiscal.estado = elementos[1];
                                        continue;
                                    case "pais":
                                        cComprobanteEmisor.DomicilioFiscal.pais = elementos[1];
                                        continue;
                                    case "codigoPostal":
                                        cComprobanteEmisor.DomicilioFiscal.codigoPostal = elementos[1];
                                        continue;
                                }
                                break;
                            case "ee":
                                switch (elementos[0])
                                {
                                    case "calle":
                                        cComprobanteEmisor.ExpedidoEn.calle = elementos[1];
                                        continue;
                                    case "noExterior":
                                        cComprobanteEmisor.ExpedidoEn.noExterior = elementos[1];
                                        continue;
                                    case "noInterior":
                                        cComprobanteEmisor.ExpedidoEn.noInterior = elementos[1];
                                        continue;
                                    case "colonia":
                                        cComprobanteEmisor.ExpedidoEn.colonia = elementos[1];
                                        continue;
                                    case "localidad":
                                        cComprobanteEmisor.ExpedidoEn.localidad = elementos[1];
                                        continue;
                                    case "municipio":
                                        cComprobanteEmisor.ExpedidoEn.municipio = elementos[1];
                                        continue;
                                    case "estado":
                                        cComprobanteEmisor.ExpedidoEn.estado = elementos[1];
                                        continue;
                                    case "pais":
                                        cComprobanteEmisor.ExpedidoEn.pais = elementos[1];
                                        continue;
                                    case "codigoPostal":
                                        cComprobanteEmisor.ExpedidoEn.codigoPostal = elementos[1];
                                        continue;
                                }
                                break;
                            case "rf":
                                switch (elementos[0])
                                {
                                    case "Regimen":
                                        cComprobanteEmisorRegimenFiscal.Regimen = elementos[1];
                                        lComprobanteEmisorRegimenFiscal.Add(cComprobanteEmisorRegimenFiscal);
                                        continue;
                                }
                                break;
                            case "rr":
                                switch (elementos[0])
                                {
                                    case "rfc":
                                        cComprobanteReceptor.rfc = elementos[1];
                                        continue;
                                    case "nombre":
                                        cComprobanteReceptor.nombre = elementos[1];
                                        continue;
                                }
                                break;
                            case "dr":
                                switch (elementos[0])
                                {
                                    case "calle":
                                        cComprobanteReceptor.Domicilio.calle = elementos[1];
                                        continue;
                                    case "noExterior":
                                        cComprobanteReceptor.Domicilio.noExterior = elementos[1];
                                        continue;
                                    case "noInterior":
                                        cComprobanteReceptor.Domicilio.noInterior = elementos[1];
                                        continue;
                                    case "colonia":
                                        cComprobanteReceptor.Domicilio.colonia = elementos[1];
                                        continue;
                                    case "localidad":
                                        cComprobanteReceptor.Domicilio.localidad = elementos[1];
                                        continue;
                                    case "municipio":
                                        cComprobanteReceptor.Domicilio.municipio = elementos[1];
                                        continue;
                                    case "estado":
                                        cComprobanteReceptor.Domicilio.estado = elementos[1];
                                        continue;
                                    case "pais":
                                        cComprobanteReceptor.Domicilio.pais = elementos[1];
                                        continue;
                                    case "codigoPostal":
                                        cComprobanteReceptor.Domicilio.codigoPostal = elementos[1];
                                        continue;
                                }
                                break;
                            case "cc":
                                switch (elementos[0])
                                {
                                    case "cantidad":
                                    cComprobanteConcepto.cantidad = decimal.Parse(elementos[1]);
                                    continue;
                                    case "unidad":
                                    cComprobanteConcepto.unidad = elementos[1];
                                continue;
                                    case "noIdentificacion":
                                    cComprobanteConcepto.noIdentificacion = elementos[1];
                                continue;
                                    case "descripcion":
                                    cComprobanteConcepto.descripcion = elementos[1];
                                continue;
                                    case "valorUnitario":
                                    cComprobanteConcepto.valorUnitario = decimal.Parse(elementos[1]);
                                continue;
                                    case "importe":
                                    cComprobanteConcepto.importe = decimal.Parse(elementos[1]);
                                    continue;
                                }

                                   // lComprobanteConcepto.Add(cComprobanteConcepto);
                                break;
                            case "im":
                                switch (elementos[0])
                                {
                                    case "totalImpuestosTrasladados":
                                        cComprobanteImpuestos.totalImpuestosTrasladados = decimal.Parse(elementos[1]);
                                        continue;
                                }
                                break;
                            case "ir":
                                switch (elementos[0])
                                {
                                    case "impuesto":
                                            switch (elementos[1])
                                            {
                                                case "ISR":
                                                    cComprobanteImpuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto.ISR;
                                                    continue;
                                                case "IVA":
                                                    cComprobanteImpuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto.IVA;
                                                    continue;
                                        }
                                            continue;
                                    case "importe":
                                            cComprobanteImpuestosRetencion.importe = decimal.Parse(elementos[1]);
                                            continue;
                                }

                                break;
                            case "it":
                                switch (elementos[0])
                                {
                                    case "impuesto":
                                        switch (elementos[1])
                                        {
                                            case "IVA":
                                                cComprobanteImpuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.IVA;
                                                break;
                                            case "IEPS":
                                                cComprobanteImpuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.IEPS;
                                                break;
                                        }
                                        continue;

                                    case "tasa":
                                        cComprobanteImpuestosTraslado.tasa = decimal.Parse(elementos[1]);
                                        continue;
                                    case "importe":
                                        cComprobanteImpuestosTraslado.importe = decimal.Parse(elementos[1]);
                                        continue;
                                }
                                break;
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    nBandera = 1;
                    //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + sNombreLayout + " " + "El archivo de texto esta mal formado" + ex.Message);
                    if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                        clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + sNombreLayout + " " + "El archivo de texto esta mal formado." + " " + ex.Message);
                    else
                        clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + sNombreLayout + " " + "El archivo de texto esta mal formado." + " " + ex.Message);

                }
            }

            lector.Close();
            //Termina el ciclo para generar el XML

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
                    //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "El certificado está fuera de fecha");
                    if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                        clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "El certificado está fuera de fecha");
                    else
                        clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "El certificado está fuera de fecha");

                    return string.Empty;
                }

                //Valida certificado
                if (!fnCSD308())
                {
                    //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "308 - Certificado no expedido por el SAT");
                    if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                        clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "308 - Certificado no expedido por el SAT");
                    else
                        clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "308 - Certificado no expedido por el SAT");

                    return string.Empty;
                }
            }
            //Cerificado para agregar al XML
            string sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());
            //Numero del certificado
            byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
            numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();
            noCertificado = cComprobante.noCertificado;
            gsSerie = cComprobante.serie;
            gsFolio = cComprobante.folio;

            if (!noCertificado.Equals(numeroCertificado))
            {
                //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "El documento " + sNombreLayout + " no contiene o es incorrecto el número de certificado");
                if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                    clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "El documento " + sNombreLayout + " no contiene o es incorrecto el número de certificado");
                else
                    clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "El documento " + sNombreLayout + " no contiene o es incorrecto el número de certificado");

                return string.Empty;
            }
            lComprobanteConcepto.Add(cComprobanteConcepto);
            lComprobanteImpuestosRetencion.Add(cComprobanteImpuestosRetencion);
            lComprobanteImpuestosTraslado.Add(cComprobanteImpuestosTraslado);
            if (lComprobanteImpuestosRetencion.Count > 0)
            {
                cComprobanteImpuestos.Retenciones = lComprobanteImpuestosRetencion.ToArray();
                cComprobanteImpuestos.totalImpuestosRetenidosSpecified = true;
                cComprobanteImpuestos.totalImpuestosRetenidos = cComprobanteImpuestos.totalImpuestosRetenidos;
            }
            if (lComprobanteImpuestosTraslado.Count > 0)
            {
                cComprobanteImpuestos.Traslados = lComprobanteImpuestosTraslado.ToArray();
                cComprobanteImpuestos.totalImpuestosTrasladadosSpecified = true;
                cComprobanteImpuestos.totalImpuestosTrasladados = cComprobanteImpuestos.totalImpuestosTrasladados;
            }
            if (lComprobanteConcepto.Count > 0)
            {
                cComprobante.Conceptos = lComprobanteConcepto.ToArray();
            }
            if (lComprobanteEmisorRegimenFiscal.Count > 0)
            {
                cComprobanteEmisor.RegimenFiscal = lComprobanteEmisorRegimenFiscal.ToArray();
            }

            string tNamespace = string.Empty;

            cComprobante.Emisor = cComprobanteEmisor;
            cComprobante.Receptor = cComprobanteReceptor;
            cComprobante.Impuestos = cComprobanteImpuestos;

            xDocumento = fnGenerarXML32(cComprobante, tNamespace);

            string scadena = "cadenaoriginal_3_2";
            navNodoTimbre = xDocumento.CreateNavigator();
            sCadenaOriginalEmisor = fnConstruirCadenaTimbrado(navNodoTimbre, scadena);
            cSello.sCadenaOriginal = sCadenaOriginalEmisor;
            sSello = cSello.sSello;
            //sSello = fnGenerarSello(sCadenaOriginalEmisor);

            //Valida sello
            if (!fnVerificarSello(sCadenaOriginalEmisor, sSello))
            {
                //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Sello incorrecto");
                if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                    clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "Sello incorrecto");
                else
                    clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "Sello incorrecto");

                return string.Empty;
            }

            if (nBandera == 0)
            {
                //Asignar los valores de certificado,numero de certificado y sello.
                nsmComprobante = new XmlNamespaceManager(xDocumento.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).SetValue(numeroCertificado);
                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);
                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sSello);
            }

            
            xsd_validacion = string.Empty;
            xsd_error_code = string.Empty;
            xsd_validacion = fnValidate(xDocumento , 0);//"esquema_v3");
            if (xsd_validacion != string.Empty && xsd_validacion != null)
            {
                throw new System.ArgumentException("333 - " + xsd_validacion, "valida esquema.");
            }

            //if (!fnValidarEsquema(xDocumento))
            //{
            //    clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + sNombreLayout + " " + "Archivo no Cumple Con Esquema " + xsd_validacion);
            //    return string.Empty;
            //}
        }
        catch (Exception ex)
        {
            if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "Archivo: " + sNombreLayout + " " + ex.Message);
            else
                clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "Archivo: " + sNombreLayout + " " + ex.Message);

            return string.Empty;
        }

        fnAgregarAddenda(ref xDocumento, listaAddenda, ref nsm);

        return xDocumento.OuterXml;
    }

    /// <summary>
    /// Procedimiento que Genera el Sello del Emisor
    /// </summary>
    /// <param name="psCadenaOriginal">Cadena Original</param>
    /// <returns></returns>
    public static string fnGenerarSello(string psCadenaOriginal)
    {
        string sello = string.Empty;
        try
        {
            //sello = rsa.SignStringENC(psCadenaOriginal, sAlgoritmo);


            //clsLog.Escribir(Settings.Default.LogError + "conteo", "0");
            //StreamReader sr = new StreamReader(Settings.Default.LogError + @"\conteo.txt");
            //int line = Convert.ToInt16(sr.ReadToEnd()) + 1;
            //sr.Close();
            //clsLog.Escribir(Settings.Default.LogError + "conteo", line.ToString());
        }
        catch (Exception)
        {
            return null;
        }
        return sello;
    }

    /// <summary>
    /// Método que se encarga de incrementar o sumar el importe de los impuestos de traslado en el atributo de totalImpuestosTrasladados
    /// </summary>
    /// <param name="pxDocumento">Comprobante</param>
    /// <param name="nodoImpuestos">Nodo de impuestos</param>
    /// <param name="paAtributos">Atributos de impuesto de traslado</param>
    private void fnIncrementarTotalImpuestosTrasladados(XmlDocument pxDocumento, XmlNode nodoImpuestos, string[] paAtributos)
    {
        XmlNamespaceManager nsmComprobanteImpuestos;
        nsmComprobanteImpuestos = new XmlNamespaceManager(pxDocumento.NameTable);
        nsmComprobanteImpuestos.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

        string sTotalImpuestosTrasladados = string.Empty;
        try { sTotalImpuestosTrasladados = nodoImpuestos.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosTrasladados", nsmComprobanteImpuestos).Value; }
        catch { }

        XmlAttribute xAttr;
        foreach (string a in paAtributos)
        {
            string[] valores = a.Split('@');
            if (valores[0].Equals("importe"))
            {
                if (sTotalImpuestosTrasladados.Equals(string.Empty))
                {
                    xAttr = pxDocumento.CreateAttribute("totalImpuestosTrasladados");
                    xAttr.Value = valores[1];
                    nodoImpuestos.Attributes.Append(xAttr);
                }
                else
                {
                    CultureInfo languaje = new CultureInfo("es-Mx");
                    double dTotalImpuestosTransladados = Convert.ToDouble(sTotalImpuestosTrasladados, languaje) + Convert.ToDouble(valores[1], languaje);
                    nodoImpuestos.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosTrasladados", nsmComprobanteImpuestos).SetValue(dTotalImpuestosTransladados.ToString("F6", languaje));
                }
            }
        }
    }

    /// <summary>
    /// Método que se encarga de incrementar o sumar el importe de los impuestos retenidos en el atributo de totalImpuestosRetenidos
    /// </summary>
    /// <param name="pxDocumento">Comprobante</param>
    /// <param name="nodoImpuestos">Nodo de impuestos</param>
    /// <param name="paAtributos">Atributos de impuesto de retencion</param>
    private void fnIncrementarTotalImpuestosRetenidos(XmlDocument pxDocumento, XmlNode nodoImpuestos, string[] paAtributos)
    {
        XmlNamespaceManager nsmComprobanteImpuestos;
        nsmComprobanteImpuestos = new XmlNamespaceManager(pxDocumento.NameTable);
        nsmComprobanteImpuestos.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

        string sTotalImpuestosTrasladados = string.Empty;
        try { sTotalImpuestosTrasladados = pxDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosRetenidos", nsmComprobanteImpuestos).Value; }
        catch { }

        XmlAttribute xAttr;
        foreach (string a in paAtributos)
        {
            string[] valores = a.Split('@');
            if (valores[0].Equals("importe"))
            {
                if (sTotalImpuestosTrasladados.Equals(string.Empty))
                {
                    xAttr = pxDocumento.CreateAttribute("totalImpuestosRetenidos");
                    xAttr.Value = valores[1];
                    nodoImpuestos.Attributes.Append(xAttr);
                }
                else
                {
                    CultureInfo languaje = new CultureInfo("es-Mx");
                    double dTotalImpuestosTransladados = Convert.ToDouble(sTotalImpuestosTrasladados, languaje) + Convert.ToDouble(valores[1], languaje);
                    nodoImpuestos.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosRetenidos", nsmComprobanteImpuestos).SetValue(dTotalImpuestosTransladados.ToString("F6",languaje));
                }
            }
        }
    }

    /// <summary>
    /// Recupera el namespace de un esquema en especifico
    /// </summary>
    /// <param name="nombre">Nombre del esquema</param>
    /// <returns></returns>
    private static string fnRecuperaNamespace(string nombre)
    {
        string retorno = string.Empty;

        nombre = nombre.Replace(Resources.origenEsquemas + @"\", "").Replace(".xsd", "");

        switch (nombre)
        {
            case "esquema_v3_2":
                retorno = "http://www.sat.gob.mx/cfd/3"; break;
            case "xsd_detallista_3_2":
                retorno = "http://www.sat.gob.mx/detallista"; break;
            case "xsd_divisas_3_2":
                retorno = "http://www.sat.gob.mx/divisas"; break;
            case "xsd_donatarias_3_2":
                retorno = "http://www.sat.gob.mx/donat"; break;
            case "xsd_ecb_3_2":
                retorno = "http://www.sat.gob.mx/ecb"; break;
            case "xsd_ecc_3_2":
                retorno = "http://www.sat.gob.mx/ecc"; break;
            case "xsd_iedu_3_2":
                retorno = "http://www.sat.gob.mx/iedu"; break;
            case "xsd_implocal_3_2":
                retorno = "http://www.sat.gob.mx/implocal"; break;
            case "xsd_leyfisc_3_2":
                retorno = "http://www.sat.gob.mx/leyendasFiscales"; break;
            case "xsd_pfic_3_2":
                retorno = "http://www.sat.gob.mx/pfic"; break;
            case "xsd_psgcfdsp_3_2":
                retorno = "http://www.sat.gob.mx/psgcfdsp"; break;
            case "xsd_turista_3_2":
                retorno = "http://www.sat.gob.mx/TuristaPasajeroExtranjero"; break;
            case "xsd_ventavehi_3_2":
                retorno = "http://www.sat.gob.mx/ventavehiculos"; break;
        }
        return retorno;
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
    /// Función que valida el comprobante contra el esquema
    /// </summary>
    /// <param name="psXmlDocument">Comprobante</param>
    /// <returns></returns>
    public string fnValidate(XmlDocument psXmlDocument, int sel)
    {
        string retorno = string.Empty;
        try
        {
            XmlTextReader tr;
            foreach (DataRow row in tblComplementos.Rows)
            {
                
                if (sel == 0)
                {
                    if (row["esquema"].ToString() == "http://www.sat.gob.mx/cfd/3")
                    {
                        tr = new XmlTextReader(new System.IO.StringReader(row["Valor"].ToString()));
                        psXmlDocument.Schemas.Add(row["esquema"].ToString(), tr);
                    }
                    
                }
                else
                {
                    if (row["esquema"].ToString() == "http://www.ford.com.mx/cfdi/addenda")
                    {
                        tr = new XmlTextReader(new System.IO.StringReader(row["Valor"].ToString()));
                        psXmlDocument.Schemas.Add(row["esquema"].ToString(), tr);
                    }
                }
            }

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
    /// Función que espera a que el archivo pueda ser leido para ser procesado.
    /// </summary>
    /// <param name="fullPath">Dirección completa</param>
    /// <returns></returns>
    bool fnWaitForFile(string fullPath)
    {
        int numTries = 0;
        while (true)
        {
            ++numTries;
            try
            {
                using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                {
                    fs.ReadByte();
                    break;
                }
            }
            catch (Exception)
            {
                if (numTries > 10)
                {
                    throw new Exception("El archivo no se puede procesar porque otro proceso lo esta utilizando: " + fullPath);
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
        return true;
    }

    // Funcion que agrega la addenda al documento xml <param name="xXML"></param>
    public void fnAgregarAddenda(ref XmlDocument xXML, List<string[]> listaAddenda, ref XmlNamespaceManager nsm)
    {
        DateTime Fecha = DateTime.Today;
        // Agregamos la addenda al XML
        if (listaAddenda.Count != 0)
        {
            try
            {

                XmlNode nodoRaiz = xXML.DocumentElement;

                XmlNode nodoAddenda = xXML.CreateElement("cfdi", "Addenda", "http://www.sat.gob.mx/cfd/3");
                nodoRaiz.AppendChild(nodoAddenda);

                XmlNode nodoFOMADD = xXML.CreateElement("fomadd", "addenda", "http://www.ford.com.mx/cfdi/addenda");

                //Agregar schemalocation//////////
                XmlAttribute idAttribute = xXML.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                idAttribute.Value = "http://www.ford.com.mx/cfdi/addenda http://www.ford.com.mx/cfdi/addenda/cfdiAddendaFord_1_0.xsd";
                nodoFOMADD.Attributes.Append(idAttribute);
                ///////////////////////////////////

                nodoAddenda.AppendChild(nodoFOMADD);

                XmlNode nodo = xXML.CreateElement("fomadd", "FOMASN", "http://www.ford.com.mx/cfdi/addenda");
                foreach (string[] arregloAddenda in listaAddenda)
                {
                    for (int i = 0; i < arregloAddenda.Length; i++)
                    {
                        string[] attrValor = arregloAddenda[i].Split('@');
                        if (attrValor[0] == "version")
                        {
                            XmlAttribute verAttribute = xXML.CreateAttribute("version"); ;
                            verAttribute.Value = attrValor[1];
                            nodo.Attributes.Append(verAttribute);

                            nodoFOMADD.AppendChild(nodo);
                        }
                        else if(attrValor[0] == "GSDB")
                        {
                            XmlElement GSDB = xXML.CreateElement("fomadd", "GSDB", "http://www.ford.com.mx/cfdi/addenda");

                            Regex regex = new Regex("^[A-Za-z0-9]{4,5}$");
                            Match match = regex.Match(attrValor[1]);
                            if (match.Success)
                            {
                                GSDB.InnerText = attrValor[1];
                            }
                            else
                            {
                                if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                                    clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, "Error: El valor ingresado no es alfanumerico o no cumple con las longitudes determinadas (GSDB) :  (" + attrValor[1] + ")");
                                else
                                    clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, "Error: El valor ingresado no es alfanumerico o no cumple con las longitudes determinadas (GSDB) :  (" + attrValor[1] + ")");
                            }

                            
                            nodo.AppendChild(GSDB);
                        }
                        else if(attrValor[0] == "ASN")
                        {
                            XmlElement ASN = xXML.CreateElement("fomadd", "ASN", "http://www.ford.com.mx/cfdi/addenda");

                            Regex regex = new Regex("^[0-9]{1,11}$");
                            Match match = regex.Match(attrValor[1]);
                            if (match.Success)
                            {
                                ASN.InnerText = attrValor[1];
                            }
                            else
                            {
                                if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                                    clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, "Error: El valor ingresado no es numerico o no cumple con las longitudes determinadas (ASN) :  (" + attrValor[1] + ")");
                                else
                                    clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, "Error: El valor ingresado no es numerico o no cumple con las longitudes determinadas (ASN) :  (" + attrValor[1] + ")");
                            }
                            
                            nodo.AppendChild(ASN);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, ex.Message + " Error al agregar addenda al comprobante.  " + " " + DateTime.Now);
            }
        }
    }

    //Recupera esquemas
    public void fnRecuperarEsquemas()
    {
        DateTime Fecha = DateTime.Today;
        tblComplementos = new DataTable();
        tblComplementos.Columns.Add("Valor");
        tblComplementos.Columns.Add("esquema");

        DataRow row = tblComplementos.NewRow();
        row["Valor"] = Resources.esquema32;
        row["esquema"] = Resources.esquema_v3_2;// fnRecuperaNamespace(esquema);
        tblComplementos.Rows.Add(row);

        DataRow row2 = tblComplementos.NewRow();

        //Buscar esquema ford//
        string sText = "";
        string[] Files = null;
        string RutaEsquema = Settings.Default.rutaEsquemaFord;
        string filtro = "*.xsd";
        Files = Directory.GetFiles(RutaEsquema, filtro);
        foreach (string archivo in Files)
        {
            using (Stream stream = File.Open(archivo.ToString(), FileMode.Open))
            {
                StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                sText = sr.ReadToEnd();
                StringReader lectorVersion = new System.IO.StringReader(sText);
            }
        }
        /////////////////////
        if (sText == "" || sText == null)
        {
            clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, "No se encontro ningun esquema en la carpeta de esquemas");
        }
        row2["Valor"] = sText;
        row2["esquema"] = Resources.esquemaford;// fnRecuperaNamespace(esquema);
        tblComplementos.Rows.Add(row2);
    }
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
            /*if (!(tNamespace == null))
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

            }*/

            return xXml;
        }
        catch (Exception ex)
        {
            clsLog.Escribir(Settings.Default.LogError, "fnGenerarXML32 - " + ex.Message);
            return xXml;
        }
    }

}


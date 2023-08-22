using PaxConectorFoxconn;
using PaxConectorFoxconn.Properties;
using PaxConectorFoxconn.wsLicenciaASMX;
using PaxConectorFoxconn.wcfRecepcionASMX;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using UtileriasXML.Adendas;

public partial class MyFileSystemWatcher : FileSystemWatcher
{
    #region Variables Privadas

    private byte[] gLlave;
    private byte[] gLlavePAC;
    private string gsPassword;

    #endregion

    private DateTime Fecha = DateTime.Today;
    private bool bValida { get; set; }
    DataTable tblComplementos;
    XmlNamespaceManager nsmComprobante = null;
    ////Servicios
    private wsLicenciaASMXSoapClient wsLicencia = new wsLicenciaASMXSoapClient();
    private wcfRecepcionASMXSoapClient wsRecepcionT = new wcfRecepcionASMXSoapClient();
    ////Licencia
    private RegistryKey clave;
    ////Seguridad    
    private static byte[] gbLlave;
    private static MemoryStream ms;
    private static Chilkat.Rsa rsa;
    private static string sAlgoritmo;
    private static Chilkat.Rsa rsaPAC;
    private static string xsd_validacion;
    private static string xsd_error_code;
    private static Chilkat.PrivateKey key;
    private static Chilkat.PrivateKey pem;
    private static string noCertificadoPAC;
    X509Certificate2 certEmisor = new X509Certificate2();
    AdendaFoxconn aAdenda;
  

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

    #endregion

    /// <summary>
    /// Tipo de algoritmo a usar
    /// </summary>
    public enum AlgoritmoSellado
    {
        MD5,
        SHA1
    }

    public MyFileSystemWatcher()
    {
        Init();
    }

    public MyFileSystemWatcher(string container)
    {
        Init();
    }

    public MyFileSystemWatcher(String inDirectoryPath, string inFilter)
        : base(inDirectoryPath, inFilter)
    {
        Init();
    }

    private void Init()
    {
        Path = Settings.Default.RutaDocs;
        InternalBufferSize = (8192 * 4);
        IncludeSubdirectories = false;
        NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
        Created += new FileSystemEventHandler(Watcher_Created);
        EnableRaisingEvents = true;

        bValida = fnValidaLicenciaLlavePC();
        fnGenerarLlave();

        tblComplementos = new DataTable();
        tblComplementos.Columns.Add("Valor");
        tblComplementos.Columns.Add("esquema");

        DataRow row = tblComplementos.NewRow();
        row["Valor"] = Settings.Default.esquema32;
        row["esquema"] = Resources.esquema_v3_2;// fnRecuperaNamespace(esquema);
        tblComplementos.Rows.Add(row);
    }

    ///// <summary>
    ///// Coloca un bloqueo en el archivo ZIP y espera a que se termine su transmision por la red
    ///// </summary>
    ///// <param name="fullPath"></param>
    ///// <returns></returns>
    //bool WaitForFile(string fullPath)
    //{
    //    int numTries = 0;
    //    while (true)
    //    {
    //        ++numTries;
    //        try
    //        {
    //            using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
    //            {
    //                fs.ReadByte();
    //                break;
    //            }
    //        }
    //        catch (Exception)
    //        {
    //            if (numTries > 10)
    //            {
    //                return false;
    //            }
    //            System.Threading.Thread.Sleep(1000);
    //        }
    //    }
    //    return true;
    //}

    public void Watcher_Created(object sender, FileSystemEventArgs e)
    {
        int nBandera;
        string sTXT, sXML, snombreDoc, sXMLGenerado;
        string[] sCad;
        char[] cCad = { '-' };
        XmlDocument xDocumento = new XmlDocument();
        aAdenda = new AdendaFoxconn();

        try
        {
            //fnGenerarLlave();

            tblComplementos = new DataTable();
            tblComplementos.Columns.Add("Valor");
            tblComplementos.Columns.Add("esquema");

            var listaEsquemas = AccesoDisco.RecuperaListaArchivos(Resources.origenEsquemas);
            foreach (string esquema in listaEsquemas)
            {
                DataRow row = tblComplementos.NewRow();
                Stream archivo = File.Open(esquema, FileMode.Open);
                StreamReader sr = new StreamReader(archivo);

                row["Valor"] = sr.ReadToEnd();
                row["esquema"] = fnRecuperaNamespace(esquema);
                tblComplementos.Rows.Add(row);
                archivo.Close();
            }

            if (!bValida)
                return;

            if (!Settings.Default.TipoServicio.Equals("GT"))
                return;

            sTXT = sXML = snombreDoc = sXMLGenerado = string.Empty;

            if (System.IO.Path.GetFileName(e.FullPath) == String.Empty)
            {
                return;
            }

            //while ((WaitForFile(e.FullPath) == false))
            //{
            //    //Se hace pato un rato.(Se espera a que se desbloquee el Archivo)
            //}

            using (FileStream fsSource = new FileStream(e.FullPath, FileMode.Open))
            {
                nBandera = 0;
                string sNombreTXT = System.IO.Path.GetFileNameWithoutExtension(e.Name);
                StreamReader srTXT = new StreamReader(fsSource);
                sTXT = srTXT.ReadToEnd();

                srTXT.Close();
                fsSource.Close();

                sXMLGenerado = fnGenerarComprobante(sTXT, sNombreTXT);

                if (sXMLGenerado.Equals(string.Empty))
                {
                    clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Comprobante no generado");
                    //Si el txt es invalido
                    //Copia el archivo txt invalido a otra carpeta
                    File.Copy(e.FullPath, Settings.Default.RutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(e.FullPath);
                    return;
                }

                xDocumento.LoadXml(sXMLGenerado);

                System.Threading.Thread.Sleep(100);

                //Se manda el xml a timbrar
                sXML = wsRecepcionT.fnEnviarXML(xDocumento.OuterXml, Settings.Default.TipoDocto, 0, Settings.Default.Usuario, Settings.Default.Password, "3.2");

                //Se valida el tipo de respuesta
                sCad = sXML.Split(cCad);
                if (sCad.Length <= 2)
                {
                    nBandera = 1; //Se indica que el comprobante no fue timbrado
                    //En caso de marcar error se graba un log
                    clsLog.Escribir(Settings.Default.LogError + "LogErrorSinTimbrar", DateTime.Now + " " + sXML + ", Nombre txt: " + sNombreTXT);
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
                    //Se obtiene el UUID
                    try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                    catch { snombreDoc = Guid.NewGuid().ToString(); }

                    //Se guarda XML en ruta especificada 
                    xXML.Save(Settings.Default.RutaDocZips + snombreDoc + ".xml");

                    try
                    {
                        //Generar PDF
                        string sRutaPDF = Settings.Default.RutaDocZips;
                        fnCrearPlantillaEnvio(xXML, Settings.Default.TipoDocto, sRutaPDF + snombreDoc + ".pdf");
                    }
                    catch (Exception ex)
                    {
                        clsLog.Escribir(Settings.Default.LogError + "LogErrorSinTimbrar", DateTime.Now + " " + ex.Message);
                    }

                    //Se guarda log de comprobantes timbrados
                    clsLog.Escribir(Settings.Default.LogTimbrados + "LogTimbrados", DateTime.Now + ", Nombre txt: " + sNombreTXT);

                    //Copia el archivo txt timbrado a otra carpeta
                    File.Copy(e.FullPath, Settings.Default.RutaTXTGen + sNombreTXT + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(e.FullPath);
                }
                else
                {
                    //Si el txt es invalido
                    //Copia el archivo txt invalido a otra carpeta
                    File.Copy(e.FullPath, Settings.Default.RutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(e.FullPath);
                }
            }
        }
        catch (Exception ex)
        {
            clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message);
        }
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
            clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message);
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
    public void fnCrearPlantillaEnvio(XmlDocument pxComprobante, string psTipoDocumento, string sRuta)
    {
        if (!(sRuta == string.Empty))
        {
            //clsPlantillaFoxconn pdf = new clsPlantillaFoxconn(pxComprobante);

            //if (!string.IsNullOrEmpty(psTipoDocumento))
            //    pdf.TipoDocumento = psTipoDocumento.ToUpper();
            //pdf.fnGenerarPDFSave(sRuta, "Black");
        }
    }

    /// <summary>
    /// Función que genera las llaves para la generación del sello
    /// </summary>
    private void fnGenerarLlave()
    {
        //Obtener la Llave Privada del Emisor
        string[] FileKey = null;
        string RutaKey = Settings.Default.RutaCertificados + "\\";
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
        string RutaPwd = Settings.Default.RutaCertificados + "\\";
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
        string RutaCert = (String)Settings.Default.RutaCertificados + "\\";
        string filtroCert = "*.cer";
        FilesCer = Directory.GetFiles(RutaCert, filtroCert);

        foreach (string filecer in FilesCer)
        {
            certEmisor.Import(filecer);
        }
        //Obtener el Certificado del Emisor

        //Llave del Emisor
        //*****************************
        noCertificadoPAC = Resources.NoCertificado;
        key = new Chilkat.PrivateKey();
        key.LoadPkcs8Encrypted(gLlave, Resources.Contraseña);
        pem = new Chilkat.PrivateKey();
        pem.LoadPem(key.GetPkcs8Pem());
        string pkeyXml = pem.GetXml();
        //*****************************
        rsa = new Chilkat.Rsa();
        //*****************************
        bool bSuccess;
        bSuccess = rsa.UnlockComponent("INTERMRSA_78UJEvED0IwK");
        bSuccess = rsa.GenerateKey(1024);
        //*****************************
        rsa.LittleEndian = false;
        rsa.EncodingMode = "base64";
        rsa.Charset = "utf-8";
        rsa.ImportPrivateKey(pkeyXml);
        sAlgoritmo = "sha-1";
        //*****************************
        //Llave del Emisor

        //Llave del PAC
        //*****************************
        key = new Chilkat.PrivateKey();
        key.LoadPkcs8Encrypted(gbLlave, gsPassword);
        pem = new Chilkat.PrivateKey();
        pem.LoadPem(key.GetPkcs8Pem());
        pkeyXml = pem.GetXml();
        //*****************************
        rsaPAC = new Chilkat.Rsa();
        //*****************************
        bSuccess = rsaPAC.UnlockComponent("INTERMRSA_78UJEvED0IwK");
        bSuccess = rsaPAC.GenerateKey(1024);
        //*****************************
        rsaPAC.LittleEndian = false;
        rsaPAC.EncodingMode = "base64";
        rsaPAC.Charset = "utf-8";
        rsaPAC.ImportPrivateKey(pkeyXml);
        sAlgoritmo = "sha-1";
        //*****************************
        //Llave del PAC
    }

    /// <summary>
    /// Función que genera el comprobante
    /// </summary>
    /// <param name="sLayout">Layout</param>
    /// <returns></returns>
    private string fnGenerarComprobante(string sLayout, string sNombreLayout)
    {
        int nBandera = 0;
        string sCadenaOriginalEmisor = String.Empty;
        string linea = string.Empty;
        string lineaVersion = string.Empty;
        string noCertificado = string.Empty;
        string numeroCertificado = string.Empty;
        string sSello = string.Empty;
        string sAdenda = string.Empty;
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
        XmlNode impuestos = null;
        XmlNode padre = null;
        XmlNode padreConcepto = null;
        

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
                                if (arreglo.Contains("noCertificado"))
                                {
                                    string[] snoCert = arreglo.Split('@');
                                    noCertificado = snoCert[1];
                                }
                            }

                            break;
                    }
                }
                catch
                {
                    nBandera = 1;
                    return string.Empty;
                }
            }
            lectorVersion.Close();

            lector = new System.IO.StringReader(sLayout);

            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
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

                        default:
                            aAdenda.fnAgregarNodo(new string[]{seccion[0],seccion[1]});
                            break;
                    }
                }
                catch (Exception ex)
                {
                    nBandera = 1;
                    clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + sNombreLayout + " " + "El archivo de texto esta mal formado" + ex.Message);
                }
            }



            lector.Close();
            //Termina el ciclo para generar el XML

            if (!aAdenda.bAdendaBienFormada) 
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + aAdenda.sMensajeError);

                return "";
            
            }

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
                    clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "El certificado está fuera de fecha");
                    return string.Empty;
                }

                //Valida certificado
                if (!fnCSD308())
                {
                    clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "308 - Certificado no expedido por el SAT");
                    return string.Empty;
                }
            }

            //Cerificado para agregar al XML
            string sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());
            //Numero del certificado
            byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
            numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();

            if (!noCertificado.Equals(numeroCertificado))
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "El documento " + sNombreLayout + " no contiene o es incorrecto el número de certificado");
                return string.Empty;
            }

            string scadena = "cadenaoriginal_3_2";
            navNodoTimbre = xDocumento.CreateNavigator();
            sCadenaOriginalEmisor = fnConstruirCadenaTimbrado(navNodoTimbre, scadena);
            sSello = fnGenerarSello(sCadenaOriginalEmisor);

            //Valida sello
            if (!fnVerificarSello(sCadenaOriginalEmisor, sSello))
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Sello incorrecto");
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

            if (!fnValidarEsquema(xDocumento))
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + sNombreLayout + " " + "Archivo no Cumple Con Esquema " + xsd_validacion);
                return string.Empty;
            }
        }
        catch (Exception ex)
        {
            clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message + " " + sNombreLayout);
            return string.Empty;
        }



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
            sello = rsa.SignStringENC(psCadenaOriginal, sAlgoritmo);
        }
        catch (Exception)
        {
            return null;
        }
        return sello;
    }

    /// <summary>
    /// Obtiene el numero serial de la pc local
    /// </summary>
    /// <returns></returns>
    private string fnObtenerSerialNumber()
    {
        string sSerialNumber = string.Empty;
        ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
        scope.Connect();
        ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

        foreach (PropertyData propData in wmiClass.Properties)
        {
            if (propData.Name == "SerialNumber")
            {
                sSerialNumber = Convert.ToString(propData.Value);
            }
        }

        sSerialNumber = Utilerias.Encriptacion.Base64.EncriptarBase64(sSerialNumber);

        return sSerialNumber;
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
    /// Valida registro licencia
    /// </summary>
    /// <returns></returns>
    private bool fnValidaLicenciaLlavePC()
    {
        bool bValida = false;
        string Cadena = string.Empty;
        string sSerialNumber = string.Empty;
        try
        {
            //Se inicializan las variables en el registro para utilizarlas en el servicio.
            //Obtiene llave PC
            sSerialNumber = fnObtenerSerialNumber();
            //Se busca registro anterior en PC
            this.clave = Registry.CurrentUser.OpenSubKey(@"Software\DDNSCDMON\" + Settings.Default.Estatus, true); //PRODUCCION
            if (this.clave == null)
            {
                //Se obtiene el usuario el cual contiene licencia
                string usuario = Settings.Default.Usuario;
                string Origen = Settings.Default.Origen;
                //Por primera vez verifica licencias suficientes del usuario en caso de contener se registra en base de datos
                Cadena = wsLicencia.fnObtenerLlaveVersionUsuario(usuario, sSerialNumber, Origen, DateTime.Today);
                Cadena = Utilerias.Encriptacion.Base64.DesencriptarBase64(Cadena);
                string[] cad = Cadena.Split('|');

                int Validez = Convert.ToInt32(cad[4]);
                //Si contiene suficientes licencias
                if (Validez == 1)
                {
                    //Se guarda llave en registro de PC
                    this.clave = Registry.CurrentUser.CreateSubKey(@"Software\DDNSCDMON\PRUEBAS");
                    this.clave.SetValue("LLAVE", sSerialNumber);
                    this.clave.Close();
                    this.clave = null;
                }
                else
                {
                    throw new Exception("Licencias insuficientes, contacte a su proveedor");
                }
            }
            else
            {
                //Se obtiene la llave registrada en PC
                string sLlaveRegistro = this.clave.GetValue("LLAVE").ToString();
                string sLlaveDesen = Utilerias.Encriptacion.Base64.DesencriptarBase64(sLlaveRegistro);
                //Se valida que la llave registrada sea igual a la llave PC la cual contiene el sistema
                if (Utilerias.Encriptacion.Base64.DesencriptarBase64(sSerialNumber) != Utilerias.Encriptacion.Base64.DesencriptarBase64(sLlaveRegistro))
                {
                    throw new Exception("La licencia no coincide con la registrada");
                }
            }
            bValida = true;
        }
        catch (Exception ex)
        {
            clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message);
        }
        return bValida;
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
            XmlTextReader tr;
            foreach (DataRow row in tblComplementos.Rows)
            {
                tr = new XmlTextReader(new System.IO.StringReader(row["Valor"].ToString()));
                psXmlDocument.Schemas.Add(row["esquema"].ToString(), tr);
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
}

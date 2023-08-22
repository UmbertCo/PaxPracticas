using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Win32;
using PaxConectorRetenciones.wsLicenciaASMX;
using PaxConectorRetenciones.wcfRecepcionasmx;
using PaxConectorRetenciones.wcfValidaASMX;
using OpenSSL_Lib;
using PaxConectorRetenciones.Properties;
using System.Management;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Schema;
using Root.Reports;
using System.Drawing;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Reflection;

namespace PaxConectorRetenciones
{
    public partial class MyFileSystemWatcher : FileSystemWatcher
    {
        #region Variables Privadas

        private byte[] gLlave;
        private byte[] gLlavePAC;
        private string gsPassword;

        #endregion

        //private DateTime Fecha = DateTime.Today;
        private bool bValida { get; set; }
        DataTable tblComplementos;
        XmlNamespaceManager nsmRetenciones = null;
        //Servicios
        private wsLicenciaASMXSoapClient wsLicencia = new wsLicenciaASMXSoapClient();
        private wcfRecepcionASMXSoapClient wsRecepcionT = new wcfRecepcionASMXSoapClient();
        private wcfValidaASMXSoapClient servicioXML = new wcfValidaASMXSoapClient();
        //Licencia
        private RegistryKey clave;
        //Seguridad    
        private static string xsd_validacion;
        private static string xsd_error_code;
        private int TipoComprobante;
        X509Certificate2 certEmisor = new X509Certificate2();
        OpenSSL_Lib.cSello cSello;
        Funciones FN;

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
            Debugger.Launch();
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

        private void Init()//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            try
            {
                bValida = fnValidaLicenciaLlavePC();

                if (bValida == true)
                {


                    Path = Settings.Default.rutaDocs;
                    InternalBufferSize = (8192 * 4);
                    IncludeSubdirectories = false;
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
                    Created += new FileSystemEventHandler(Watcher_Created);
                    EnableRaisingEvents = true;

                    fnGenerarLlave();

                    /**************CREAR DIRECTORIO*****************/
                    string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                    AppPath = AppPath.Replace("file:\\", "");

                    string path = AppPath + "\\esquemas";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);

                    }

                    if (Directory.GetFiles(path).Length < 25)
                    {
                        CrearEsquemas();
                    }

                    tblComplementos = new DataTable();
                    tblComplementos.Columns.Add("Valor");
                    tblComplementos.Columns.Add("esquema");

                    DataRow row = tblComplementos.NewRow();
                    /* string rutaEsquemaRetenciones = AppDomain.CurrentDomain.BaseDirectory + "\\esquemas\\retencionpagov1.xsd";

                     XmlDocument xDocument = new XmlDocument();
                     xDocument.Load(rutaEsquemaRetenciones);
                
                     row["Valor"] = xDocument.OuterXml; */

                    row["Valor"] = Resources.retencionpagoV1;
                    row["esquema"] = Resources.retenciones;// fnRecuperaNamespace(esquema);
                    tblComplementos.Rows.Add(row);
                }
                else
                {
                    clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Error al iniciar el servicio. No hay licencias validas ");
                }
            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Error al iniciar el servicio. " + ex);
            }
        }

        public void CrearEsquemas()
        {
            /****GENERAR CARPETA DE ESQUEMAS***/

            System.IO.Stream stream = new MemoryStream(Resources.esquemas);

            ZipFile zip = new ZipFile(stream);

            //Assembly.GetExecutingAssembly().Location

            string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            AppPath = AppPath.Replace("file:\\", "");

            string path = AppPath + "\\esquemas";

            foreach (ZipEntry zipEntry in zip)
            {
                if (zipEntry.IsFile)
                {
                    /*using (var fs = new FileStream(path + "\\" + zipEntry.Name, FileMode.Create, FileAccess.Write))
                        zip.GetInputStream(zipEntry).CopyTo(fs);*/

                    Stream zipStream = zip.GetInputStream(zipEntry);
                    byte[] buffer = new byte[4096];

                    using (FileStream streamWriter = File.Create(path + "\\" + zipEntry.Name))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }     
        }

        private void fnGenerarLlave()////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            try
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
            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Faltan elementos del certificado." + ex.Message);
            }

        }
        private bool fnValidaLicenciaLlavePC()/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

                    string usuario = Settings.Default.usuario;
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

                        this.clave = Registry.CurrentUser.CreateSubKey(@"Software\DDNSCDMON\"+ Settings.Default.Estatus);
                        this.clave.SetValue("LLAVE", sSerialNumber);
                        this.clave.Close();
                        this.clave = null;
                        bValida = true;
                    }
                    else
                    {
                        bValida = false;
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
                        bValida = false;
                        throw new Exception("La licencia no coincide con la registrada");
                    }
                    else
                    {
                        bValida = true;
                    }
                } 
            }
            catch (Exception ex)
            {
                bValida = false;
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Error al validar la licencia. " + ex.Message);
            }
            return bValida;
        }

        private string fnObtenerSerialNumber()////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

        public void Watcher_Created(object sender, FileSystemEventArgs e)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            int nBandera;
            DateTime Fecha = DateTime.Today;
            string sTXT, sXML, snombreDoc, sXMLGenerado;
            string[] sCad;
            char[] cCad = { '-' };
            XmlDocument xDocumento = new XmlDocument();
            try
            {
                //Genera Llaves PEM

                fnGenerarLlave();

                if (!bValida)
                    return;

                if (!Settings.Default.TipoServicio.Equals("GT"))
                    return;

                sTXT = sXML = snombreDoc = sXMLGenerado = string.Empty;

                if (System.IO.Path.GetFileName(e.FullPath) == String.Empty)
                {
                    return;
                }

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
                        //Si el txt es invalido
                        //Copia el archivo txt invalido a otra carpeta
                    
                        File.Copy(e.FullPath, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);


                        //Elimina el archivo txt

                        File.Delete(e.FullPath);
                        return;
                    }

                    xDocumento.LoadXml(sXMLGenerado);

                    System.Threading.Thread.Sleep(100);

                    //Se manda el xml a timbrar


                    //sXML = wsRecepcionT.fnEnviarXML(xDocumento.OuterXml, Settings.Default.tipodocto, 0, Settings.Default.usuario, Settings.Default.password, "3.2");
                    sXML = wsRecepcionT.fnEnviarXML(xDocumento.OuterXml, TipoComprobante, 0, Settings.Default.usuario, Settings.Default.password, "1.0");


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
                        XmlNamespaceManager nsmRetenciones = new XmlNamespaceManager(xXML.NameTable);
                        nsmRetenciones.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                        nsmRetenciones.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
                        nsmRetenciones.AddNamespace("arrendamientoenfideicomiso", "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso");
                        nsmRetenciones.AddNamespace("dividendos", "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos");
                        nsmRetenciones.AddNamespace("enajenaciondeacciones", "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones");
                        nsmRetenciones.AddNamespace("fideicomisonoempresarial", "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial");
                        nsmRetenciones.AddNamespace("intereses", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses");
                        nsmRetenciones.AddNamespace("intereseshipotecarios", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios");
                        nsmRetenciones.AddNamespace("operacionesconderivados", "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados");
                        nsmRetenciones.AddNamespace("pagosaextranjeros", "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros");
                        nsmRetenciones.AddNamespace("planesderetiro", "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro");
                        nsmRetenciones.AddNamespace("premios", "http://www.sat.gob.mx/esquemas/retencionpago/1/premios");
                        nsmRetenciones.AddNamespace("sectorfinanciero", "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero");
                        nsmRetenciones.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                        XPathNavigator navEncabezado = xXML.CreateNavigator();


                        var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", Settings.Default.RutaDocZips, sNombreTXT);

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
                        File.Copy(e.FullPath, Settings.Default.rutaTXTGen + sNombreTXT + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                        //Elimina el archivo txt
                        File.Delete(e.FullPath);
                    }
                    else
                    {
                        //Si el txt es invalido
                        //Copia el archivo txt invalido a otra carpeta
                        File.Copy(e.FullPath, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                        //Elimina el archivo txt
                        File.Delete(e.FullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Ha ocurrido un error al generar el comprobante. " + ex.Message);
            }
        }

        private string fnGenerarComprobante(string sLayout, string sNombreLayout)//////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            int nBandera = 0;
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
            XmlNode padre = null;
            XmlNode val = null;

            string xsd_valEna = string.Empty;
            string xsd_valDiv = string.Empty;
            string xsd_valInt = string.Empty;
            string xsd_valFid = string.Empty;
            string xsd_valPag = string.Empty;
            string xsd_valPre = string.Empty;
            string xsd_valOp = string.Empty;
            string xsd_valSec = string.Empty;

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
                            case "ret":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("NumCert"))
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

                nsm.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
                nsm.AddNamespace("arrendamientoenfideicomiso", "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso");
                nsm.AddNamespace("dividendos", "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos");
                nsm.AddNamespace("enajenaciondeacciones", "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones");
                nsm.AddNamespace("fideicomisonoempresarial", "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial");
                nsm.AddNamespace("intereses", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses");
                nsm.AddNamespace("intereseshipotecarios", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios");
                nsm.AddNamespace("operacionesconderivados", "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados");
                nsm.AddNamespace("pagosaextranjeros", "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros");
                nsm.AddNamespace("planesderetiro", "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro");
                nsm.AddNamespace("premios", "http://www.sat.gob.mx/esquemas/retencionpago/1/premios");
                nsm.AddNamespace("sectorfinanciero", "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero");
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");


                xDocumento = new XmlDocument(nsm.NameTable);
                xDocumento.CreateXmlDeclaration("1.0", "UTF-8", "no");
                xDocumento.AppendChild(xDocumento.CreateElement("retenciones", "Retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1"));

                int acum = 0;
                string Denom = "";
                string Tipo = "";

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

                        switch (seccion[0])
                        {
                            case "ret":
                                fnCrearElementoRoot32(xDocumento, atributos);
                                break;
                            case "re":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Emisor", atributos, ""));
                                break;
                            case "rr":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Receptor", atributos, ""));
                                break;
                            case "rn":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Receptor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Nacional", atributos, ""));
                                break;
                            case "rx":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Receptor", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Extranjero", atributos, ""));
                                break;
                            case "rp":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Periodo", atributos, ""));
                                break;
                            case "rt":
                                xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Totales", atributos, ""));
                                break;
                            case "ri":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Totales", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "ImpRetenidos", atributos, ""));
                                break;
                            case "comp":

                                xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("retenciones", "Complemento", "http://www.sat.gob.mx/esquemas/retencionpago/1"));


                                foreach (string a in atributos)
                                {
                                    string[] valores = a.Split('@');
                                    Denom = valores[0];
                                    Tipo = valores[1];
                                }

                                fnAgregarNamespacesExtra(xDocumento, Tipo, nsm);

                                //VALIDAR ESTRUCTURA BASICA
                                xsd_validacion = string.Empty;
                                xsd_validacion = fnValidate(xDocumento);
                                break;

                            //Enajenacion acciones
                            case "ena":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "EnajenaciondeAcciones", atributos, "enajenaciondeacciones"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/enajenaciondeacciones:EnajenaciondeAcciones", nsm);
                                xsd_valEna = Validador(val.OuterXml, "enajenaciondeacciones");
                                break;

                            //Dividendos

                            case "div":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Dividendos", atributos, "dividendos"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/dividendos:Dividendos", nsm);
                                xsd_valDiv = Validador(val.OuterXml, "dividendos");
                                break;
                            case "dividOutil":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/dividendos:Dividendos", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "DividOUtil", atributos, "dividendos"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/dividendos:Dividendos", nsm);
                                xsd_valDiv = Validador(val.OuterXml, "dividendos");
                                break;
                            case "rem":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/dividendos:Dividendos", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Remanente", atributos, "dividendos"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/dividendos:Dividendos", nsm);
                                xsd_valDiv = Validador(val.OuterXml, "dividendos");
                                break;

                            //Intereses

                            case "int":

                                if (Tipo == "intereses")
                                {
                                    padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Intereses", atributos, "intereses"));

                                    val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/intereses:Intereses", nsm);
                                    xsd_valInt = Validador(val.OuterXml, "intereses");

                                }
                                else if (Tipo == "intereseshipoteca")
                                {
                                    padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Intereseshipotecarios", atributos, "intereseshipotecarios"));

                                    val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/intereseshipotecarios:Intereseshipotecarios", nsm);
                                    xsd_valInt = Validador(val.OuterXml, "intereseshipotecarios");
                                }

                                break;

                            //Arrendamiento Fideicomiso

                            case "fid":
                                if (Tipo == "arrendafideicomiso")
                                {
                                    padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Arrendamientoenfideicomiso", atributos, "arrendamientoenfideicomiso"));

                                    val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/arrendamientoenfideicomiso:Arrendamientoenfideicomiso", nsm);
                                    xsd_valFid = Validador(val.OuterXml, "arrendamientoenfideicomiso");

                                }
                                else if (Tipo == "fidenoempresarial")
                                {
                                    padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Fideicomisonoempresarial", atributos, "fideicomisonoempresarial"));

                                    val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                    xsd_valFid = Validador(val.OuterXml, "fideicomisonoempresarial");
                                }
                                break;

                            //Pago Extranjeros

                            case "pag":
                                if (Tipo == "pagosextranjeros")
                                {
                                    padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Pagosaextranjeros", atributos, "pagosaextranjeros"));

                                    val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/pagosaextranjeros:Pagosaextranjeros", nsm);
                                    xsd_valPag = Validador(val.OuterXml, "pagosaextranjeros");
                                }
                                else if (Tipo == "planesretiro")
                                {

                                    padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Planesderetiro", atributos, "planesderetiro"));

                                    val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/planesderetiro:Planesderetiro", nsm);
                                    xsd_valPag = Validador(val.OuterXml, "planesderetiro");
                                }

                                break;
                            case "nobe":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/pagosaextranjeros:Pagosaextranjeros", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "NoBeneficiario", atributos, "pagosaextranjeros"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/pagosaextranjeros:Pagosaextranjeros", nsm);
                                xsd_valPag = Validador(val.OuterXml, "pagosaextranjeros");
                                break;
                            case "bene":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/pagosaextranjeros:Pagosaextranjeros", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Beneficiario", atributos, "pagosaextranjeros"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/pagosaextranjeros:Pagosaextranjeros", nsm);
                                xsd_valPag = Validador(val.OuterXml, "pagosaextranjeros");
                                break;

                            //Premios

                            case "pre":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Premios", atributos, "premios"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/premios:Premios", nsm);
                                xsd_valPre = Validador(val.OuterXml, "premios");
                                break;

                            //Fideicomiso Noempresarial

                            /*case "fid":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Fideicomisonoempresarial", atributos, "fideicomisonoempresarial"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                xsd_validacion += Validador(val.OuterXml, "fideicomisonoempresarial");
                                break;*/
                            case "ing":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "IngresosOEntradas", atributos, "fideicomisonoempresarial"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                xsd_valFid = Validador(val.OuterXml, "fideicomisonoempresarial");
                                break;
                            case "inting":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial/fideicomisonoempresarial:IngresosOEntradas", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "IntegracIngresos", atributos, "fideicomisonoempresarial"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                xsd_valFid = Validador(val.OuterXml, "fideicomisonoempresarial");
                                break;
                            case "ded":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "DeduccOSalidas", atributos, "fideicomisonoempresarial"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                xsd_valFid = Validador(val.OuterXml, "fideicomisonoempresarial");
                                break;
                            case "intded":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial/fideicomisonoempresarial:DeduccOSalidas", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "IntegracEgresos", atributos, "fideicomisonoempresarial"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                xsd_valFid = Validador(val.OuterXml, "fideicomisonoempresarial");
                                break;
                            case "reten":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "RetEfectFideicomiso", atributos, "fideicomisonoempresarial"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsm);
                                xsd_valFid = Validador(val.OuterXml, "fideicomisonoempresarial");
                                break;

                            //Planes de retiro

                            /*case "pagr":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Planesderetiro", atributos, "planesderetiro"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/planesderetiro:Planesderetiro", nsm);
                                xsd_validacion += Validador(val.OuterXml, "planesderetiro");
                                break;*/

                            //Intereses Hipotecarios

                            /*case "inth":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Intereseshipotecarios", atributos, "intereseshipotecarios"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/intereseshipotecarios:Intereseshipotecarios", nsm);
                                xsd_validacion += Validador(val.OuterXml, "intereseshipotecarios");
                                break;*/

                            //Operaciones con derivados

                            case "op":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "Operacionesconderivados", atributos, "operacionesconderivados"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/operacionesconderivados:Operacionesconderivados", nsm);
                                xsd_valOp = Validador(val.OuterXml, "operacionesconderivados");
                                break;

                            //Sector Financiero

                            case "sec":
                                padre = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento", nsm);
                                padre.AppendChild(fnCrearElemento(xDocumento, "SectorFinanciero", atributos, "sectorfinanciero"));

                                val = xDocumento.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/sectorfinanciero:SectorFinanciero", nsm);
                                xsd_valSec = Validador(val.OuterXml, "sectorfinanciero");
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
                cSello.sCadenaOriginal = sCadenaOriginalEmisor;

                sSello = cSello.sSello;

                //Valida sello
                if (!fnVerificarSello(sCadenaOriginalEmisor, sSello))
                {
                    clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Sello incorrecto");
                    return string.Empty;
                }

                if (nBandera == 0)
                {
                    //Asignar los valores de certificado,numero de certificado y sello.
                    nsmRetenciones = new XmlNamespaceManager(xDocumento.NameTable);
                    nsmRetenciones.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
                    xDocumento.CreateNavigator().SelectSingleNode("/retenciones:Retenciones/@NumCert", nsmRetenciones).SetValue(numeroCertificado);
                    xDocumento.CreateNavigator().SelectSingleNode("/retenciones:Retenciones/@Cert", nsmRetenciones).SetValue(sCertificado);
                    xDocumento.CreateNavigator().SelectSingleNode("/retenciones:Retenciones/@Sello", nsmRetenciones).SetValue(sSello);
                }


                //VALIDAR COMPLEMENTOS?
                //xsd_validacion = fnValidate(xDocumento);
                xsd_validacion = xsd_valEna + xsd_valDiv + xsd_valInt + xsd_valFid + xsd_valPag + xsd_valPre + xsd_valOp + xsd_valSec;

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
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + " " + ex.Message + "Archivo: " + sNombreLayout);
                return string.Empty;
            }
            return xDocumento.OuterXml;
        }

        private void fnCrearElementoRoot32(XmlDocument pxDocumento, string[] pasAtributos)/////////////////////////////////////////////////////////////////////////////////////////////////
        {
            XmlAttribute xAttr;
            foreach (string a in pasAtributos)
            {
                string[] valores = a.Split('@');
                xAttr = pxDocumento.CreateAttribute(valores[0]);
                xAttr.Value = valores[1];
                if (valores[0] == "CveRetenc")
                {
                    TipoComprobante = Convert.ToInt32(valores[1]);
                }
                pxDocumento.DocumentElement.Attributes.Append(xAttr);
            }
            xAttr = pxDocumento.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xAttr.Value = "http://www.sat.gob.mx/esquemas/retencionpago/1 http://www.sat.gob.mx/esquemas/retencionpago/1/retencionpagov1.xsd ";
            pxDocumento.DocumentElement.Attributes.Append(xAttr);
           
        }

        private void fnAgregarNamespacesExtra(XmlDocument pxDocumento, string nombre, XmlNamespaceManager nsm)
        {
            XmlNode raiz = null;
            raiz = pxDocumento.SelectSingleNode("/retenciones:Retenciones", nsm);
            try
            {
                
                string Namespace;

                switch (nombre)
                {

                    case "enajenacionacciones":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones/enajenaciondeacciones.xsd";
                        break;
                    case "dividendos":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos/dividendos.xsd";
                        break;
                    case "intereses":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses http://www.sat.gob.mx/esquemas/retencionpago/1/intereses/intereses.xsd ";
                        break;
                    case "arrendafideicomiso":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso/arrendamientoenfideicomiso.xsd";
                        break;
                    case "pagosextranjeros":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros/pagosaextranjeros.xsd";
                        break;
                    case "premios":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/premios http://www.sat.gob.mx/esquemas/retencionpago/1/premios/premios.xsd";
                        break;
                    case "fidenoempresarial":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial/fideicomisonoempresarial.xsd";
                        break;
                    case "planesretiro":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro/planesderetiro.xsd";
                        break;
                    case "intereseshipoteca":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios/intereseshipotecarios.xsd";
                        break;
                    case "derivados":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados/operacionesconderivados.xsd";
                        break;
                    case "sectorfinanciero":
                        Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero/sectorfinanciero.xsd";
                        break;
                    default:
                        Namespace = "";
                        break;
                }
                if (pxDocumento != null)
                {

                    XmlAttribute idAttribute = raiz.Attributes["xsi:schemaLocation"];
                    if (idAttribute != null)
                    {

                        idAttribute.Value = "http://www.sat.gob.mx/esquemas/retencionpago/1 http://www.sat.gob.mx/esquemas/retencionpago/1/retencionpagov1.xsd " + Namespace;
                        pxDocumento.DocumentElement.Attributes.Append(idAttribute);
                        
                    }
                    /*XmlAttribute xAttr;
                    xAttr = pxDocumento.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    xAttr.Value = "http://www.sat.gob.mx/esquemas/retencionpago/1 http://www.sat.gob.mx/esquemas/retencionpago/1/retencionpagov1.xsd " + Namespace;
                    pxDocumento.DocumentElement.Attributes.Append(xAttr);*/
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           

        }

        private XmlElement fnCrearElemento(XmlDocument pxDocumento, string psElemento, string[] pasAtributos, string nombre)//////////////////////////////////////////////////////////////
        {
            XmlAttribute xAttr;
            string Namespace = "";
            if (nombre == "" || nombre == string.Empty)
	        {
                nombre = "retenciones";
	        }
            switch (nombre)
            {
                case "":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1";
                    break;
                case "enajenaciondeacciones":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones";
                    break;
                case "dividendos":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos"; 
                    break;
                case "intereses":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses";
                    break;
                case "arrendamientoenfideicomiso":
                    Namespace ="http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso";
                    break;
                case "pagosaextranjeros":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros";
                    break;
                case "premios":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/premios";
                    break;
                case "fideicomisonoempresarial":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial";
                    break;
                case "planesderetiro":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro"; 
                    break;
                case "intereseshipotecarios":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios"; 
                    break;
                case "operacionesconderivados":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados";
                    break;
                case "sectorfinanciero":
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero";
                    break;
                default:
                    Namespace = "http://www.sat.gob.mx/esquemas/retencionpago/1";
                    break;
            }
            
            XmlElement elemento = pxDocumento.CreateElement(nombre, psElemento, Namespace);
            foreach (string a in pasAtributos)
            {
                string[] valores = a.Split('@');
                xAttr = pxDocumento.CreateAttribute(valores[0]);
                xAttr.Value = valores[1];
                elemento.Attributes.Append(xAttr);
            }
            return elemento;
        }

        private bool fnComprobarFechas()/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            bool bResultado = true;

            if (certEmisor.NotBefore.CompareTo(DateTime.Today) > 0 || certEmisor.NotAfter.CompareTo(DateTime.Today) < 0)
                return false;

            return bResultado;
        }
        private bool fnCSD308()///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
        private string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)///////////////////////////////////////////////////////////////////////////////////
        {
            string sCadenaOriginal = string.Empty;
            MemoryStream ms;
            StreamReader srDll;
            XsltArgumentList args;
            XslCompiledTransform xslt;
            try
            {
                /*StringReader srt = new StringReader(Settings.Default.cadenaoriginal_RET);
                XmlReader xrt = XmlReader.Create(srt);*/

                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOriRet.V10));
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
        public bool fnVerificarSello(string psCadenaOriginal, string psSello)//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certEmisor.PublicKey.Key);
            try
            {
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
            catch (Exception)
            {
                return false;
            }
        }
        public string fnValidate(XmlDocument psXmlDocument)///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            string retorno = string.Empty;
            try
            {
                XmlTextReader tr;
                foreach (DataRow row in tblComplementos.Rows)
                {
                    tr = new XmlTextReader(new System.IO.StringReader(row["Valor"].ToString()));
                    psXmlDocument.Schemas.Add(row["esquema"].ToString(), tr);//APUNTAR A ESQUEMA LOCAL

                }

                ValidationEventHandler validation = new ValidationEventHandler(SchemaValidationHandler);
                psXmlDocument.Validate(validation);

                if (xsd_error_code != string.Empty)
                {
                    retorno = xsd_error_code;
                }
            }
            catch (Exception ex)
            {
                string cosa = ex.ToString();
                return "Revise el esquema del XML y reintente de nuevo.";
                
            }
            return retorno;
        }
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
        public string Validador(string XML, string Clasificacion)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                AppPath = AppPath.Replace("file:\\", "");

                string path = AppPath + "\\esquemas\\";
                settings.Schemas.Add(null, XmlReader.Create(path + Clasificacion + ".xsd"));

                StringReader srt = new StringReader(XML);
                XmlReader Reader = XmlReader.Create(srt, settings);

                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.Load(Reader);
                Reader.Close();
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public void fnCrearPlantillaEnvio(XmlDocument pxComprobante, string psTipoDocumento, string sRuta)
        {


            FN = new Funciones();
 
            try
            {
                Report PDFA = FN.fnGenerarPDF(Color.FromArgb(99, 0, 0), pxComprobante);
                PDFA.Save(sRuta);
            }
            catch (Exception ex)
            {

                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message);
            }
        }
    }
}

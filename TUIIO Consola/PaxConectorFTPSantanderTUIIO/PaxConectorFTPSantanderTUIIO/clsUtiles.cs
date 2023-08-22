using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using PAXConectorFTPGTCFDI33.Properties;
using ICSharpCode.SharpZipLib.Zip;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PAXConectorFTPGTCFDI33
{
    delegate void delDetener(Int32 nCantidadHilos);


    public class clsUtiles
    {
        public static string sHash { get; set; }
        static X509Certificate2 certEmisor = new X509Certificate2();
        public static string[] arregloAddenda; //Arreglo que guardara los atributos y valores de la addenda

        public clsUtiles()
        {
             InitializeCulture();
        }

        public void InitializeCulture()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
        }

        /// <summary>
        /// Método que se encarga de cargar el certificado el emisor
        /// </summary>
        public static void fnAgregarCertificado()
        {
            string[] FilesCer = null;
            string RutaCert = Settings.Default.RutaCertificado;
            string filtroCert = "*.cer";
            try
            {
                FilesCer = Directory.GetFiles(RutaCert, filtroCert);

                foreach (string filecer in FilesCer)
                {
                    certEmisor.Import(filecer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar el certificado del emisor." + ex.Message);
            }
        }

        /// <summary>
        /// Metodo que se encarga de agregar un archivo a un Zip
        /// </summary>
        /// <param name="zStream"></param>
        /// <param name="relativePath"></param>
        /// <param name="file"></param>
        public static void fnAñadirFicheroaZip(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream, string relativePath, string file)
        {
            byte[] buffer = new byte[4096];
            string fileRelativePath = (relativePath.Length > 1 ? relativePath : string.Empty)
                                      + System.IO.Path.GetFileName(file);
            ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(fileRelativePath);

            zStream.PutNextEntry(entry); 
            using (FileStream fs = File.OpenRead(file))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }  
        }

        /// <summary>
        /// Función que contruye la cadena original
        /// </summary>
        /// <param name="xml">Documento</param>
        /// <returns></returns>
        private static string fnConstruirCadenaTimbrado(IXPathNavigable xml)
        {
            string sCadenaOriginal = string.Empty;
            MemoryStream ms;
            StreamReader srDll;
            XsltArgumentList args;
            XslCompiledTransform xslt;
            try
            {
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOri.V40));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                sCadenaOriginal = srDll.ReadToEnd();
            }
            catch (Exception ex)
            {
                clsLog.WriteLine(ex.Message + " Error al generar la cadena original.  " + " " + DateTime.Now);
                clsLogRespaldo.WriteLine(ex.Message + " Error al generar la cadena original.  " + " " + DateTime.Now);
                throw new System.OperationCanceledException();
            }
            return sCadenaOriginal;
        }

        /// <summary>
        /// Sobrecarga que realize para poder escribir en el log de xml en caso de error
        /// </summary>
        /// <param name="psCadenaOriginal"></param>
        /// <param name="sNombreArchivoZip"></param>
        /// <param name="sNombreXML"></param>
        /// <returns></returns>
        private static bool fnExisteComprobante(string psCadenaOriginal)
        {
            bool bResultado = false;
            string nComprobante = string.Empty;
            string sHashEmisor = string.Empty;
            try
            {
                sHash = string.Empty;
                sHashEmisor = fnGetHASH(psCadenaOriginal);
                nComprobante = fnObtenerHashComprobante(sHashEmisor);
                if (!nComprobante.Equals("0"))
                {
                    bResultado = true;
                }
                sHash = sHashEmisor;
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException("No pudo combrobar la existencia del comprobante." + " " + ex.Message);
            }
            return bResultado;
        }

        /// <summary>
        /// Método que se encarga de generar el sello del comprobante
        /// </summary>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <returns></returns>
        private static string fnGenerarSello(string psCadenaOriginal)
        {
            string sResultado = string.Empty;
            try
            {
                File.WriteAllText(Settings.Default.RutaSello + "bin.txt", psCadenaOriginal);
                string _sNombreArchivoBin = Settings.Default.RutaSello + "BIN_SIGNATURE.txt";

                ProcessStartInfo psiEjecucion
                                            = new ProcessStartInfo(Settings.Default.RutaOpenSSL, "dgst -sha256 -sign \"" +
                                                Settings.Default.RutaPEM + "\" -out \"" + _sNombreArchivoBin + "\" \"" +
                                                 Settings.Default.RutaSello + "bin.txt");

                psiEjecucion.CreateNoWindow = true;
                psiEjecucion.UseShellExecute = false;
                psiEjecucion.RedirectStandardError = true;

                // Inicia el proceso ya inicializado y espera a que termine su ejecucion
                Process pProceso = Process.Start(psiEjecucion);
                pProceso.WaitForExit();

                if (pProceso.ExitCode != 0) throw new Exception("No se pudo crear el archivo BIN: " + _sNombreArchivoBin);
                pProceso.Dispose();

                psiEjecucion
                    = new ProcessStartInfo(Settings.Default.RutaOpenSSL, "enc -base64 -in \""
                        + _sNombreArchivoBin + "\" -out \"" +
                        Settings.Default.RutaSello + "sello.txt" + "\"");

                psiEjecucion.CreateNoWindow = true;
                psiEjecucion.UseShellExecute = false;
                psiEjecucion.RedirectStandardError = true;
                // Inicia el proceso ya inicializado y espera a que termine su ejecucion
                pProceso = Process.Start(psiEjecucion);
                pProceso.WaitForExit();

                if (pProceso.ExitCode != 0) throw new Exception("No se pudo crear el archivo Sello: " + Settings.Default.RutaSello + "sello.txt");
                pProceso.Dispose();

                string fileSello = File.ReadAllText(Settings.Default.RutaSello + "sello.txt");
                string _sSello = "";
                foreach (string elemento in fileSello.Split('\n'))
                    _sSello += elemento.Trim().TrimStart().TrimEnd();

                sResultado = _sSello;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el sello. " + ex.Message);
            }
            finally
            {
                if (File.Exists(Settings.Default.RutaSello + "bin.txt"))
                    File.Delete(Settings.Default.RutaSello + "bin.txt");
                if (File.Exists(Settings.Default.RutaSello + "BIN_SIGNATURE.txt"))
                    File.Delete(Settings.Default.RutaSello + "BIN_SIGNATURE.txt");
                if (File.Exists(Settings.Default.RutaSello + "sello.txt"))
                    File.Delete(Settings.Default.RutaSello + "sello.txt");
            }
            return sResultado;
        }

        /// <summary>
        /// Función que se encarga de recibir el Layout de texto y convertirlo en un documento de tipo XML
        /// </summary>
        /// <param name="psLayout">Layout</param>
        /// <param name="psNombre">Nombre del documento</param>
        /// <returns></returns>
        private static XmlDocument fnGenerarXML(string psLayout, string psNombre)
        {

            int x = 0;
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

                //Revisa el modo de ejecución
                if (Settings.Default.Modo.Equals("P"))
                {
                    //Valida certificado sea vigente
                    if (!fnComprobarFechas())
                    {
                        clsLog.WriteLine("El certificado está fuera de fecha");
                        throw (new Exception("El certificado está fuera de fecha"));
                    }

                    //Valida certificado
                    //if (!fnCSD308())
                    //{
                    //    clsLog.WriteLine("308 - Certificado no expedido por el SAT");
                    //    throw (new Exception("308 - Certificado no expedido por el SAT"));
                    //}
                }


                MemoryStream ms = new MemoryStream();
                StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);

                XmlDocument xmlComplNomina = new XmlDocument();
                XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
                nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                //Se agrego Exportacion='01' -> PAGOS20
                string sNodoComprobante = "<cfdi:Comprobante Moneda='XXX' LugarExpedicion='" + Settings.Default.LugarExpedicion + "' TipoDeComprobante='P' Exportacion='01' Total='0' SubTotal='0'  Certificado='' NoCertificado=''  Sello='' Fecha='" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "' Version='4.0' xsi:schemaLocation='http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd http://www.sat.gob.mx/Pagos20 http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos20.xsd' xmlns:pago20='http://www.sat.gob.mx/Pagos20'  xmlns:cfdi='http://www.sat.gob.mx/cfd/4' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>" + psLayout + "</cfdi:Comprobante>";





                //psLayout = psLayout.Substring(psLayout.IndexOf("<cfdi:Comprobante "), "<cfdi:Comprobante ".Length) 
                //    + new string (psLayout.Contains("xmlns:xsi")?"".ToArray():"xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'".ToArray())
                //    + new string(psLayout.Contains("xmlns:cfdi")?"".ToArray():"xmlns:cfdi='http://www.sat.gob.mx/cfd/3'".ToArray())
                //    + "xsi:schemaLocation='http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd'"
                //    + psLayout.Substring(psLayout.IndexOf("<cfdi:Comprobante ") + "<cfdi:Comprobante ".Length);

                
                 xDocumento.LoadXml(sNodoComprobante);

                //xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:cfdi="http://www.sat.gob.mx/cfd/3">
                ///xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante").CreateAttribute("xsi", "schemaLocation", "", "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd http://www.sat.gob.mx/Pagos http://www.sat.gob.mx/sitio_internet/cfd/Pagos/Pagos10.xsd");

                try
                {
                    xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@Rfc", nsm).SetValue(fnObtenerRfcEmisorDeCertificado(certEmisor));
                }
                catch { }

                try
                {
                    xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Certificado", nsm).SetValue(sCertificado);
                }
                catch { }



                try
                {
                    xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@NoCertificado", nsm).SetValue(numeroCertificado);
                }
                catch { }

                //Busca si tiene atributos de DomicilioFiscalReceptor, RegimenFiscalReceptory si no lo agrega
                XmlNode receptorNode = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor", nsm);
                XmlAttribute DomicilioFiscalReceptorAttr = receptorNode.Attributes["DomicilioFiscalReceptor"];
                XmlAttribute RegimenFiscalReceptorAttr = receptorNode.Attributes["RegimenFiscalReceptor"];
                XmlAttribute RFCAttr = receptorNode.Attributes["Rfc"];
                XmlAttribute UsoCFDIAttr = receptorNode.Attributes["UsoCFDI"];


                if (string.IsNullOrEmpty(DomicilioFiscalReceptorAttr?.Value) || string.IsNullOrEmpty(RegimenFiscalReceptorAttr?.Value))
                {
                    // El atributo está vacío, lo agregamos con un valor específico
                    RFCAttr.Value = "XAXX010101000";
                    DomicilioFiscalReceptorAttr.Value = "01219";
                    RegimenFiscalReceptorAttr.Value = "616";
                    UsoCFDIAttr.Value = "CP01";
                }

                navNodoTimbre = xDocumento.CreateNavigator();
                sCadenaOriginalEmisor = fnConstruirCadenaTimbrado(navNodoTimbre);

                sSello = fnGenerarSello(sCadenaOriginalEmisor);






                try
                {
                    xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Sello", nsm).SetValue(sSello);
                }
                catch { }

                try
                {
                    xDocumento.CreateNavigator()
                        .SelectSingleNode("/cfdi:Comprobante", nsm).AppendChild("<cfdi:Addenda><ad cadenaOriginal='"
                        + sCadenaOriginalEmisor + "'></ad></cfdi:Addenda>");


                }

                catch { }






                //xDocumento = fnGenerarXML32(Cfd, tNameSpace);

                //xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante", nsm).AppendChild("<cfdi:Addenda><ad cadenaOriginal='" + sCadenaOriginalEmisor + "'></ad></cfdi:Addenda>");



            }
            catch (Exception ex)
            {
                clsLog.WriteLine(psNombre + " - " + ex.Message);
                throw (new Exception(psNombre + " - " + ex.Message));
            }
            return xDocumento;
        }

        /// Función que revisa que el certificado no sea apocrifo
        /// </summary>
        /// <returns></returns>
        private static bool fnCSD308()
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
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool fnComprobarFechas()
        {
            bool bResultado = true;

            if (certEmisor.NotBefore.CompareTo(DateTime.Today) > 0 || certEmisor.NotAfter.CompareTo(DateTime.Today) < 0)
                return false;

            return bResultado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pcertEmisor"></param>
        /// <returns></returns>
        public static string fnObtenerRfcEmisorDeCertificado(X509Certificate2 pcertEmisor)
        {
            try
            {
                string[] cert = pcertEmisor.SubjectName.Decode(X500DistinguishedNameFlags.UseNewLines).Split('\n');

                string[] rfc = cert[3].Split('=');

                string sRfc = string.Empty;

                foreach (string sParteCert in cert)
                {
                    if (fnValidaExpresion(sParteCert, @"(OID)(\.\d+)+"))
                    {
                        if (fnValidaExpresion(sParteCert, @"(?<rfc>[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A])"))
                        {

                            return fnSeleccionarExpresion(sParteCert, "(?<rfc>[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A])", "rfc");
                        }
                    }



                }


            }
            catch (CryptographicException ex)
            {
                clsLog.WriteLine("Error al obtener Rfc Emisor: (" + DateTime.Now + "):" + ex.Message);


            }
            catch (Exception ex)
            {
                clsLog.WriteLine("Error al obtener Rfc Emisor: (" + DateTime.Now + "):" + ex.Message);
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sValor"></param>
        /// <param name="expresion"></param>
        /// <param name="psNombreGrupo"></param>
        /// <returns></returns>
        public static string fnSeleccionarExpresion(string sValor, string expresion, string psNombreGrupo)
        {


            if (Regex.IsMatch(sValor, expresion))
            {
                Match mBusqueda = Regex.Match(sValor, expresion);

                return mBusqueda.Groups[psNombreGrupo].Value;



            }

            return "";
        }

        /// <summary>
        /// Encargado de validar si la expreson es verdadera
        /// </summary>
        /// <param name="sValor">valor a evaluar</param>
        /// <param name="expresion">expresion regular</param>
        /// <returns>retorna si es verdadero</returns>
        public static bool fnValidaExpresion(string sValor, string expresion)
        {
            bool bRetorno = false;

            if (Regex.IsMatch(sValor, expresion))
            {
                bRetorno = true;
            }

            return bRetorno;
        }

        /// <summary>
        /// Función que se encarga de generar el Hash de una cadena
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string fnGetHASH(string text)
        {
            byte[] hashValue;
            byte[] message = Encoding.UTF8.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        /// <summary>
        /// Sobrecarga que realize para poder escribir en el log de xml en caso de error
        /// </summary>
        /// <param name="psHashEmisor"></param>
        /// <param name="sNombreArchivoZip"></param>
        /// <param name="sNombreXML"></param>
        /// <returns></returns>
        private static string fnObtenerHashComprobante(string psHashEmisor)
        {
            string nRetorno = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
                {
                    conexion.Open();
                    // Se busca el comprobante 
                    using (SqlCommand comando = new SqlCommand("usp_mpac40_Comprobantes_Datos_BuscaHASH_XML_Sel", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@sHash", psHashEmisor);
                        nRetorno = Convert.ToString(comando.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new OperationCanceledException("No fue posible buscar el Hash del comprobante.");
            }
            return nRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRutaArchivo"></param>
        public static void fnProcesoPrincipal(String sRutaArchivo)
        {
            String sNombre = Path.GetFileName(sRutaArchivo);

        

            if (Settings.Default.LogFlush)
            {
                XmlDocument xdEntradaArchivo = new XmlDocument();
                xdEntradaArchivo.LoadXml("<EntradaArchivo nombre='' tamaño=''  hora='" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "'></EntradaArchivo>");

                xdEntradaArchivo.CreateNavigator().SelectSingleNode("/EntradaArchivo/@nombre").SetValue(sNombre);

                clsLog.fnFlush(xdEntradaArchivo.OuterXml);

                xdEntradaArchivo = null;
            }

            //Inicia las Variables de monitoreo de Repeticion cada vez que se genera el evento created

            List<String> lArchivosRegistrados = null;
            Hashtable htTablaRepetidos = null;

            clsExtensionMethod.htHilosRepetidos.Clear();
            clsExtensionMethod.xdHilos = new XmlDocument();
            clsExtensionMethod.nHilos = 0;
            clsExtensionMethod.lArchivosProcesados = new List<string>();

            lArchivosRegistrados = new List<string>();
            htTablaRepetidos = new Hashtable();

            clsExtensionMethod.xdHilos.LoadXml("<Hilos Archivo='' CantidadHilos =''></Hilos>");

            //Log.WriteLine("Archivo Agregado " + sRutaArchivo + " "+ DateTime.Now);
            while ((fnWaitForFile(sRutaArchivo) == false))
            {
                //Espera a que el archivo sea escrito totalmente en el disco duro.
            }


            if (Settings.Default.LogFlush)
            {
                XmlDocument xdSalidaWFF = new XmlDocument();

                xdSalidaWFF.LoadXml("<SalidaWFF nombre=''  hora='" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "'></SalidaWFF>");

                xdSalidaWFF.CreateNavigator().SelectSingleNode("/SalidaWFF/@nombre").SetValue(sNombre);

                clsLog.fnFlush(xdSalidaWFF.OuterXml);

                xdSalidaWFF = null;
            }

            bool bErrorEntradaCopia = false;

            try
            {
                if (!Directory.Exists(Settings.Default.FolderCopia))
                    Directory.CreateDirectory(Settings.Default.FolderCopia);

                File.Copy(sRutaArchivo, Settings.Default.FolderCopia + System.IO.Path.DirectorySeparatorChar + sNombre);
            }
            catch (IOException ex)
            {
                bErrorEntradaCopia = true;

                if (File.Exists(Settings.Default.FolderCopia + System.IO.Path.DirectorySeparatorChar + sNombre))
                {
                    File.Copy(sRutaArchivo, Settings.Default.FolderCopia + System.IO.Path.DirectorySeparatorChar + sNombre + "PAX " + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss_fff") + ".zip", true);
                }
                else
                {
                    throw new Exception("Error en Entrada Copia: " + ex.Message);
                }
            }

            catch (Exception ex)
            {
                bErrorEntradaCopia = true;

                try { File.Delete(sRutaArchivo); }
                catch
                {
                    clsLog.WriteLine("Error al Copiar: (" + DateTime.Now + "):" + ex.Message);
                    clsLogRespaldo.WriteLine("Error al Copiar: (" + DateTime.Now + "):" + ex.Message);
                    return;
                }

                clsLog.WriteLine("Error al Copiar: (" + DateTime.Now + "):" + ex.Message);
                clsLogRespaldo.WriteLine("Error al Copiar: (" + DateTime.Now + "):" + ex.Message);
                return;
            }


            if (!bErrorEntradaCopia)
            {
                clsLog.WriteLine("Archivo Agregado " + sRutaArchivo + " " + DateTime.Now);
                clsLogRespaldo.WriteLine("Archivo Agregado " + sRutaArchivo + " " + DateTime.Now);
            }

            if (System.IO.Path.GetFileName(sRutaArchivo) == String.Empty)
            {
                //Ignorar carpeta nueva.
            }
            else
            {
                try
                {


                    int nRetorno;
                    int nExisteHash;
                    List<clsCfdi> list = new List<clsCfdi>();
                    using (FileStream fsSource = new FileStream(sRutaArchivo, FileMode.Open))
                    {
                        Stream strXml = null;
                        Stream strZip = fsSource;
                        ZipFile archivoZip = new ZipFile(strZip);

                      

                        XmlDocument xmlDocGeneraTimbre = new XmlDocument();
                        string Usuario = Properties.Settings.Default.Usuario;
                        using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
                        {
                            conexion.Open();
                            using (SqlTransaction tran = conexion.BeginTransaction())
                            {
                               

                                try
                                {
                                    string hashzip = fnGetHASH(sNombre).ToString();
                                    using (SqlCommand command = new SqlCommand("usp_mpac40_Zips_Datos_BuscaHASH_Sel", conexion))
                                    {
                                        command.Transaction = tran;
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("sHASH", hashzip);
                                        nExisteHash = Convert.ToInt32(command.ExecuteScalar());
                                    }

                                    

                                    if (nExisteHash == 0)
                                    {
                                        using (SqlCommand command = new SqlCommand("usp_mpac40_Zip_Datos_Ins", conexion))
                                        {
                                            command.Transaction = tran;
                                            command.CommandType = CommandType.StoredProcedure;
                                            command.Parameters.AddWithValue("snombre_zip", sNombre);
                                            command.Parameters.AddWithValue("shash_zip", hashzip);
                                            command.Parameters.AddWithValue("sfecha_recepcion", DateTime.Now);
                                            command.Parameters.AddWithValue("sfecha_salida", DateTime.Now);
                                            command.Parameters.AddWithValue("sEstatus", "P");
                                            command.Parameters.AddWithValue("sUsuario", Usuario);
                                            nRetorno = Convert.ToInt32(command.ExecuteScalar());
                                        }
                                    }
                                    else
                                    {
                                        throw new System.OperationCanceledException();
                                    }
                                    tran.Commit();

                                    
                                }
                                catch (OperationCanceledException)
                                {
                                    throw new System.OperationCanceledException("El Archivo ZIP " + sNombre + " ya se proceso previamente.");
                                }
                                catch (Exception ex)
                                {
                                    throw new System.OperationCanceledException("El Archivo ZIP " + sNombre + " no se pudo almacenar en la Base de Datos ó contiene carpetas. " + ex.Message);
                                }
                            }
                        }

                        


                        bool bPrimerArchivo = true;

                        if (archivoZip.Count == 0)
                        {
                            throw new OperationCanceledException("El Archivo " + sNombre + " esta vacio ");
                        }

                        foreach (ZipEntry zipEntry in archivoZip)
                        {
                            if (Settings.Default.LogFlush)
                            {
                                if (bPrimerArchivo)
                                {
                                    XmlDocument PropiedadesArchivo = new XmlDocument();
                                    PropiedadesArchivo.LoadXml("<PropiedadesArchivo nombre='' tamañoArchivo='' Cantidad='' hora='" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "'></PropiedadesArchivo>");

                                    PropiedadesArchivo.CreateNavigator().SelectSingleNode("/PropiedadesArchivo/@nombre").SetValue(sNombre);
                                    PropiedadesArchivo.CreateNavigator().SelectSingleNode("/PropiedadesArchivo/@tamañoArchivo").SetValue(zipEntry.Size.ToString());
                                    PropiedadesArchivo.CreateNavigator().SelectSingleNode("/PropiedadesArchivo/@Cantidad").SetValue(archivoZip.Count.ToString());

                                    clsLog.fnFlush(PropiedadesArchivo.OuterXml);

                                    PropiedadesArchivo = null;

                                    bPrimerArchivo = false;
                                }
                            }

                            try
                            {
                                if (!zipEntry.IsFile)
                                {
                                    throw new OperationCanceledException();
                                }

                                if (zipEntry.IsDirectory)
                                {
                                    throw new OperationCanceledException();
                                }

                                if (zipEntry.IsCrypted)
                                {
                                    throw new OperationCanceledException();
                                }


                                String entryFileName = zipEntry.Name;

                                #region Agregado por Hector Portillo 2014-05-06
                                //Para cada entrada del Zip verifica que los nombres de archivo sean iguales. En caso de serlo lo agrega a la HashTable de archivos repetidos
                                //y los enumera para luego imprimirlos en el Log

                                try
                                {
                                    htTablaRepetidos.Add(entryFileName, 0);

                                }
                                catch
                                {
                                    int nAux = int.Parse(htTablaRepetidos[entryFileName].ToString());

                                    nAux++;

                                    htTablaRepetidos[entryFileName] = nAux;

                                    if (Settings.Default.LogFlush)
                                    {
                                        clsLog.fnFlush(DateTime.Now + " Zip " + sNombre + " contiene archivos repetidos");


                                        clsLog.WriteLine(DateTime.Now + " Zip " + sNombre + " contiene archivos repetidos");
                                        clsLogRespaldo.WriteLine(DateTime.Now + " Zip " + sNombre + " contiene archivos repetidos");
                                        File.Copy(Properties.Settings.Default.MonitorFolder + @"\" + sNombre, Properties.Settings.Default.FolderIncorrectos + @"\" + sNombre, true);
                                        File.Delete(Properties.Settings.Default.MonitorFolder + @"\" + sNombre);

                                    }
                                    return;
                                }

                                #endregion

                                byte[] buffer = new byte[4096];
                                byte[] pdf = new byte[0];
                                byte[] xml = new byte[0];
                                string sText = string.Empty;

                                if (System.IO.Path.GetExtension(zipEntry.Name).ToLower() == ".xml")
                                {
                                 

                                    try
                                    {
                                        //--------------------------------------------------------------------
                                        strXml = (Stream)archivoZip.GetInputStream(zipEntry);
                                        //--------------------------------------------------------------------
                                        StreamReader reader = new StreamReader(strXml);
                                        sText = reader.ReadToEnd();
                                        reader.Close();
                                        //--------------------------------------------------------------------

                                        XmlDocument xDocumento = new XmlDocument();
                                        xDocumento = fnGenerarXML(sText, zipEntry.Name);

                                        if (Settings.Default.ValidacionHash)
                                        {
                                            string sCadenaOriginal = fnConstruirCadenaTimbrado(xDocumento.CreateNavigator());

                                            //Se revisa que el comprobante no exista en la BD local
                                            if (fnExisteComprobante(sCadenaOriginal))
                                            {
                                                clsLog.WriteLine("Comprobante existente " + Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name + " " + DateTime.Now);
                                                clsLogRespaldo.WriteLine("Comprobante existente " + Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name + " " + DateTime.Now);
                                                File.WriteAllText(Settings.Default.FolderIncorrectos + @"\" + "Error_" + zipEntry.Name, "<Response><Description>Ya existe un comprobante registrado.</Description></Response>");
                                                File.WriteAllText(Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name, xDocumento.OuterXml);
                                                continue;
                                            }
                                        }

                                        clsCfdi cfdi = new clsCfdi();
                                        cfdi.XML = xDocumento.InnerXml;
                                        cfdi.FileName = System.IO.Path.GetFileNameWithoutExtension(zipEntry.Name);
                                        cfdi.HashEmisor = sHash;

                                        list.Add(cfdi);
                                      
                                        //--------------------------------------------------------------------
                                    }
                                    catch (Exception ex)
                                    {
                                        if (strXml.Length == 0) return;
                                        clsLog.WriteLine("Archivo Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name + " " + DateTime.Now + " EXCEPTION: " + ex);
                                        clsLogRespaldo.WriteLine("Archivo Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name + " " + DateTime.Now + " EXCEPTION: " + ex);
                                        // Create a FileStream object to write a stream to a file
                                        File.WriteAllText(Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name , sText);
                                    }
                                }
                                else
                                {
                                    clsLog.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name + " " + DateTime.Now + " No es XML");
                                    clsLogRespaldo.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name + " " + DateTime.Now + " No es XML");
                                }
                            }
                            catch (OperationCanceledException)
                            {
                                throw new OperationCanceledException();
                            }
                            catch (Exception ex)
                            {
                                clsLog.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name + " " + DateTime.Now + " " + ex.Message);
                                clsLogRespaldo.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + zipEntry.Name + " " + DateTime.Now + " " + ex.Message);
                            }
                        }
                    }


                   
                    clsExtensionMethod.zip = null;
                    clsExtensionMethod.zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(Properties.Settings.Default.FolderSalida + @"\" + sNombre));
                    clsExtensionMethod.zip.SetLevel(6);

                    int nHilos = Convert.ToInt32(Properties.Settings.Default.Hilos);

                    clsColeccionCFDI[] CFDIs = clsExtensionMethod.fnParticion1(list, nHilos);

                    if (CFDIs.Length < nHilos) nHilos = CFDIs.Length;

                    var threads = new Thread[nHilos];
                    int i = 0;


                    delDetener fnDetener = new delDetener(delegate(int nCantidadHilos)
                    {
                        while (clsExtensionMethod.nHilos < nCantidadHilos)
                        {
                            Thread.Sleep(1000);
                        }
                    });

                    #region Agregado por Hector Portillo 2014-09-11
                    for (int nHilo = 0; nHilo < CFDIs.Length; nHilo++)
                    {
                        Worker workerObject = new Worker();

                        threads[nHilo] = new Thread(new ParameterizedThreadStart(workerObject.DoWork));

                        threads[nHilo].Name = "Hilo " + nHilo;

                        threads[nHilo].Start(new Object[] { CFDIs[nHilo].Collecion, nRetorno });

                        Thread.Sleep(5);

                        i++;
                    }
                    #endregion


                   
                    if (Settings.Default.LogFlush)
                    {
                        XmlDocument xdEntradaProceso = new XmlDocument();
                        xdEntradaProceso.LoadXml("<EntradaProceso nombre=''  hora='" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "'></EntradaProceso>");
                        xdEntradaProceso.CreateNavigator().SelectSingleNode("/EntradaProceso/@nombre").SetValue(sNombre);
                        clsLog.fnFlush(xdEntradaProceso.OuterXml);
                        xdEntradaProceso = null;
                    }

                    fnDetener.Invoke(nHilos);

                    if (Settings.Default.LogFlush)
                    {
                        XmlDocument xdSalidaProceso = new XmlDocument();
                        xdSalidaProceso.LoadXml("<SalidaProceso nombre=''  hora='" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "'></SalidaProceso>");
                        xdSalidaProceso.CreateNavigator().SelectSingleNode("/SalidaProceso/@nombre").SetValue(sNombre);
                        clsLog.fnFlush(xdSalidaProceso.OuterXml);
                        xdSalidaProceso = null;
                    }

                    string sRes = clsExtensionMethod.fnComparaListas(list, clsExtensionMethod.lArchivosProcesados, sNombre);

                    

                    if (!sRes.Equals(""))
                    {
                        clsLog.WriteLine(DateTime.Now + " " + sRes);
                        clsLogRespaldo.WriteLine(DateTime.Now + " " + sRes);
                    }

                    CFDIs = null;

                    clsExtensionMethod.zip.Finish();
                    clsExtensionMethod.zip.Close();

                    

                    using (FileStream fsSource = new FileStream(Properties.Settings.Default.FolderSalida + @"\" + sNombre, FileMode.Open))
                    {
                        Stream strZip = fsSource;
                        ZipFile archivoZip = new ZipFile(strZip);
                        long size = archivoZip.Count;

                        archivoZip.Close();

                        if (size == 0)
                        {
                            File.Delete(Properties.Settings.Default.FolderSalida + @"\" + sNombre);
                        }
                        else
                        {
                            File.Copy(Properties.Settings.Default.FolderSalida + @"\" + sNombre, Properties.Settings.Default.FolderRespaldo + @"\" + sNombre);
                            File.Copy(Properties.Settings.Default.MonitorFolder + @"\" + sNombre, Properties.Settings.Default.FolderProcesados + @"\" + sNombre);
                        }

                        strZip.Dispose();
                    }

                    using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
                    {
                        try
                        {
                            conexion.Open();
                            using (SqlCommand command = new SqlCommand("usp_mpac40_Zip_Datos_Upd", conexion))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("nIdZip", nRetorno);
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            clsLog.WriteLine("No se pudo actualizar la fecha de Salida del Archivo ZIP " + sNombre + " " + DateTime.Now);
                            clsLogRespaldo.WriteLine("No se pudo actualizar la fecha de Salida del Archivo ZIP " + sNombre + " " + DateTime.Now);
                            if (Settings.Default.LogFlush)
                            {
                                clsLog.fnFlush(ex.Message);
                            }
                        }
                    }

                    clsLog.WriteLine("Archivo Finalizado " + sRutaArchivo + " " + DateTime.Now);
                    clsLogRespaldo.WriteLine("Archivo Finalizado " + sRutaArchivo + " " + DateTime.Now);
                    //File.Copy(Properties.Settings.Default.folderSalida + @"\" + inArgs.Name, Properties.Settings.Default.folderRespaldo + @"\" + sNombre);
                    File.Delete(Properties.Settings.Default.MonitorFolder + @"\" + sNombre);
                }
                catch (OperationCanceledException ex)
                {
                    clsLog.WriteLine(ex.Message + DateTime.Now);
                    clsLogRespaldo.WriteLine(ex.Message + DateTime.Now);
                    File.Copy(Properties.Settings.Default.MonitorFolder + @"\" + sNombre, Properties.Settings.Default.FolderIncorrectos + @"\" + sNombre, true);
                    File.Delete(Properties.Settings.Default.MonitorFolder + @"\" + sNombre);
                }

                catch (Exception ex)
                {
                    clsLog.WriteLine(ex.Message + " El Archivo " + sNombre + " no es ZIP ó esta dañado." + DateTime.Now);
                    clsLogRespaldo.WriteLine(ex.Message + " El Archivo " + sNombre + " no es ZIP ó esta dañado." + DateTime.Now);
                    File.Delete(Properties.Settings.Default.MonitorFolder + @"\" + sNombre);
                }
            }

            if (Settings.Default.LogFlush)
            {
                XmlDocument xdSalidaArchivo = new XmlDocument();
                xdSalidaArchivo.LoadXml("<SalidaArchivo nombre=''  hora='" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "'></SalidaArchivo>");
                xdSalidaArchivo.CreateNavigator().SelectSingleNode("/SalidaArchivo/@nombre").SetValue(sNombre);
                clsLog.fnFlush(xdSalidaArchivo.OuterXml);
                xdSalidaArchivo = null;
            }
        }

        /// <summary>
        /// Anexo 20 Eliminar en la reglas de estructura.
        /// </summary>
        /// <param name="varRep">Valor</param>
        /// <returns></returns>
        private static string fnReplaceCaracters(string varRep)
        {
            string sReplace = string.Empty;

            if (varRep.Equals('&'))
            {
                varRep.Replace("&", "&amp;");
            }

            if (varRep.Equals('<'))
            {
                varRep.Replace("<", "&lt;");
            }

            if (varRep.Equals('>'))
            {
                varRep.Replace(">", "&gt;");
            }

            if (varRep.Equals("'"))
            {
                varRep.Replace("'", "&apos;");
            }

            if (varRep.Equals("\""))
            {
                varRep.Replace("\"", "&quot;");
            }

            sReplace = varRep;
            return sReplace;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDocumento"></param>
        /// <returns></returns>
        public static XmlDocument fnResellar(XmlDocument xDocumento)
        {
            XPathNavigator navNodoTimbre = xDocumento.CreateNavigator();

            String sCadenaOriginal = String.Empty;

            sCadenaOriginal = fnConstruirCadenaTimbrado(navNodoTimbre);

            //if (Settings.Default.ValidacionHash)
            //{
            //    if (fnExisteComprobante(sCadenaOriginal))
            //    {
            //        return xDocumento;
            //    }
            //}

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocumento.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");

            String sSello = fnGenerarSello(sCadenaOriginal);

            xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Sello", nsmComprobante).SetValue(sSello);

            return xDocumento;
        }


        /// <summary>
        /// Función que se encarga de obtener el siguiente archivo en la ruta de entrada
        /// </summary>
        /// <returns></returns>
        public static string fnSiguienteArchivo()
        {
            string sRes = "";
            try
            {
                //Solo obtener archivos ZIP
                sRes = Directory.EnumerateFiles(Settings.Default.MonitorFolder, "*.zip").ToList<string>()[0];
            }
            catch (IOException ex) { }
            catch
            {

            }
            return sRes;
        }

        /// <summary>
        /// Comprueba que el sello del comprobante refleje los datos de la cadena original
        /// </summary>
        /// <param name="psCadenaOriginal">Cadena original del comprobante</param>
        /// <returns>Booleano indicando si la cadena original corresponde al sello</returns>
        private static bool fnVerificarSello(string psCadenaOriginal, string psSello)
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
        /// Metodo que se encarga de indicar cuando un archivo puede ser procesado en la carpeta de entrada
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static bool fnWaitForFile(string fullPath)
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
                        return false;
                    }
                    System.Threading.Thread.Sleep(500);
                }
            } return true;
        }
    }
}

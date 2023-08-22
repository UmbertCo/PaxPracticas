using SolucionPruebas.Presentacion;
using SolucionPruebas.Presentacion.Servicios;
using SolucionPruebas.WindowsService.EnviadorComprobantes.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SolucionPruebas.WindowsService.EnviadorComprobantes
{
    /// <summary>
    /// Clase de CFDI, que contiene información individual de cada unos de los Layouts de texto en el archivo ZIP
    /// </summary>
    public class cfdi
    {
        public string XML { get; set; }
        public string Layout { get; set; }
        public string FileName { get; set; }
    }

    public class clsEnviarComprobantes
    {
        private Presentacion.Servicios.ServicioLocal.ServiceClient SDServicioLocal;
        private Presentacion.Servicios.wsRecepcionDesarrollo.wcfRecepcionASMXSoapClient SDRecepcionDesarrollo;
        
        //private List<XmlDocument> ListaComprobantes;
        //private volatile bool _shouldStop;

        private byte[] gLlave;
        private string gsPassword;

        public void fnEnviarComprobantes()
        {
            string sCadenaOriginal;
            string sSello;
            string sArchivo;
            string sResultado;
            XmlDocument xdComprobante;
            XmlDocument xdComprobanteTimbrado;
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            OpenSSL_Lib.cSello cSello;
            try
            {
                clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + "Iniciar ");

                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsEnviarComprobantes.AcceptAllCertificatePolicy);

                List<string> lArchivos = new List<string>();
                lArchivos = Directory.GetFiles(Settings.Default.RutaArchivos).ToList();

                //string[] FileKey = null;
                //string RutaKey = Settings.Default.RutaLlave;
                //string filtroKey = "*.key";
                //FileKey = Directory.GetFiles(RutaKey, filtroKey);
                //foreach (string filekey in FileKey)
                //{
                //    Stream streamkey = File.Open(filekey.ToString(), FileMode.Open);
                //    StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
                //    using (BinaryReader br = new BinaryReader(streamkey))
                //    {
                //        gLlave = br.ReadBytes(Convert.ToInt32(streamkey.Length));
                //    }
                //}

                ////Obtener el Password del Certificado Emisor
                //string[] FilePwd = null;
                //string RutaPwd = (String)Settings.Default.RutaLlave + "\\";
                //string filtroPwd = "*.txt";
                //FilePwd = Directory.GetFiles(RutaPwd, filtroPwd);

                //foreach (string filePwd in FilePwd)
                //{
                //    using (Stream streamPwd = File.Open(filePwd.ToString(), FileMode.Open))
                //    {
                //        StreamReader srPwd = new StreamReader(streamPwd, System.Text.Encoding.UTF8, true);
                //        gsPassword = srPwd.ReadToEnd();
                //    }
                //}

                //cSello = new OpenSSL_Lib.cSello(FileKey[0], FilePwd[0], Settings.Default.RutaPfx);

                clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + "Obteniendo archivos: " + lArchivos.Count());


                List<cfdi> list = new List<cfdi>();
                List<XmlDocument> ListaDocumento = new List<XmlDocument>();

                foreach (string sRutaArchivo in lArchivos)
                {
                    sArchivo = string.Empty;
                    sResultado = string.Empty;

                    Stream stArchivo = File.Open(sRutaArchivo, FileMode.Open);
                    StreamReader srArchivo = new StreamReader(stArchivo);
                    sArchivo = srArchivo.ReadToEnd();
                    srArchivo.Close();

                    xdComprobante = new XmlDocument();
                    xdComprobanteTimbrado = new XmlDocument();
                    nsm = new XmlNamespaceManager(xdComprobante.NameTable);
                    nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                    nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                    xdComprobante.LoadXml(sArchivo);

                    ////Removemos la declaracion del Xml en caso de que exista
                    //foreach (XmlNode xnNodo in xdComprobante)
                    //{
                    //    if (xnNodo.NodeType.Equals(XmlNodeType.XmlDeclaration))
                    //        xdComprobante.RemoveChild(xnNodo);
                    //}

                    ////Removemos el nodo del timbra fiscal en caso de que exista
                    //foreach (XmlNode xnNodo in xdComprobante.FirstChild.ChildNodes)
                    //{
                    //    if (xnNodo.Name.Equals("cfdi:Complemento"))
                    //    {
                    //        foreach (XmlNode xnNodoComplemento in xnNodo)
                    //        {
                    //            if (xnNodoComplemento.Name.Equals("tfd:TimbreFiscalDigital"))
                    //            {
                    //                xnNodo.RemoveChild(xnNodoComplemento);
                    //            }
                    //        }
                    //    }
                    //}

                    //xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsm).SetValue(DateTime.Now.ToString("s"));                 

                    //try
                    //{
                    //    sCadenaOriginal = fnConstruirCadenaTimbrado(xdComprobante.CreateNavigator());
                    //    cSello.sCadenaOriginal = sCadenaOriginal;
                    //    sSello = cSello.sSello;
                    //}
                    //catch (Exception ex)
                    //{
                    //    clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + "Error al generar el sello: " + ex.Message);
                    //    continue;
                    //}

                    //sSello = sSello.Replace("\n", "");
                    //xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsm).SetValue(sSello);

                    cfdi cfdi = new cfdi();
                    cfdi.XML = xdComprobante.InnerXml;
                    cfdi.Layout = xdComprobante.InnerXml;
                    cfdi.FileName = sRutaArchivo;
                    list.Add(cfdi);

                    ListaDocumento.Add(xdComprobante);

                    File.Delete(sRutaArchivo);

                    clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + "Archivo agregado: " + sRutaArchivo);
                }

                clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + "Iniciando!!");

                int div = list.Count() / Convert.ToInt32(5);
                if (div == 0) div = 1;

                IEnumerable<List<cfdi>> list1 = ExtensionMethod.Partition<cfdi>(list, div);

                var threads = new Thread[list1.Count()];
                int i = 0;
                foreach (List<cfdi> list2 in list1)
                {
                    Envio workerEnvio = new Envio();
                    threads[i] = new Thread(() => workerEnvio.fnEnviarComprobante(list2));
                    threads[i].Start();
                    Thread.Sleep(5);
                    i++;
                }

                foreach (Thread thread in threads)
                {
                    thread.Join();
                }

                //Envio cEnvioComprobantes = new Envio(ListaDocumento);
                //Thread tHilo = new Thread(new ThreadStart(cEnvioComprobantes.fnEnviarComprobante));

                //tHilo.Start();
                //tHilo.Join();

                clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + "Terminado!!");
            }
            catch (Exception ex)
            {
                clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + ex.Message);
            }
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
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
                throw new Exception(DateTime.Now + " " + "Error al generar la cadena original." + " " + ex.Message);
                //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message);
            }
            return sCadenaOriginal;
        }        
    }

    public class Envio
    {
        static readonly object _object = new object();
        //private Presentacion.Servicios.wcfRecepcionDP.wcfRecepcionASMXSoapClient SDRecepcionDP;
        //private Presentacion.Servicios.RecepcionDP2.wcfRecepcionASMXSoapClient SDRecepcionDP2;
        private Presentacion.Servicios.wsRecepcionDesarrollo.wcfRecepcionASMXSoapClient SDRecepcionDesarrollo;

        private List<XmlDocument> ListaComprobantes;
        private volatile bool _shouldStop;

        public Envio()
        {

        }

        public Envio(List<XmlDocument> ListaDocumentos)
        {
            this.ListaComprobantes = ListaDocumentos;
        }

        public void fnEnviarComprobante()
        {
            string sResultado = string.Empty;
            try
            {
                foreach (XmlDocument xdComprobante in ListaComprobantes)
                {
                    XmlDocument xdDocumentoTimbrado = new XmlDocument();
                    //SDRecepcionDP2 = ProxyLocator.ObtenerServicioTimbradoDP2();
                    //sResultado = SDRecepcionDP2.fnEnviarXML(xdComprobante.InnerXml, "Factura", 0, "ismael.hidalgo", "Z2/CpcODw4BtworChMOrw4AU77++N8ObQjpiPEXvv7vvvqrvvozvv7sV77yd776a77+I77+Q", "3.2");

                    string sNombreArchivo = String.Format("{0}\\{1}.xml", Settings.Default.RutaSalida, System.IO.Path.GetFileNameWithoutExtension(xdComprobante.Name));

                    try
                    {
                        xdDocumentoTimbrado.LoadXml(sResultado);
                        xdDocumentoTimbrado.Save(sNombreArchivo);
                    }
                    catch (Exception ex)
                    {
                        clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + ex.Message + " - " + sResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + ex.Message + " - " + sResultado);
            }
        }

        public void fnEnviarComprobante(List<cfdi> list)
        {
            while (!_shouldStop)
            {
                foreach (cfdi prime in list)
                {
                    string sResultado = string.Empty;
                    try
                    {
                        lock (_object)
                        {
                            XmlDocument xdDocumentoTimbrado = new XmlDocument();
                            SDRecepcionDesarrollo = ProxyLocator.ObtenerServicioTimbradoDesarrollo();
                            sResultado = SDRecepcionDesarrollo.fnEnviarXML(prime.XML, "Factura", 0, "ismael.hidalgo", "ecKBxITDmsObw6h/w7jDmMKw77+nGXhUflHCnX07W++9vu++qUzvvoTvvoPvv67vvpDvvobvvaE=", "3.2");

                            string sNombreArchivo = String.Format("{0}\\{1}.xml", Settings.Default.RutaSalida, System.IO.Path.GetFileNameWithoutExtension(prime.FileName));
                            try
                            {
                                xdDocumentoTimbrado.LoadXml(sResultado);
                                xdDocumentoTimbrado.Save(sNombreArchivo);
                            }
                            catch (Exception ex)
                            {
                                clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + ex.Message + " - " + sResultado);
                            }
                        }
                    }
                    catch (FaultException ex)
                    {
                        clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + ex.Message + " - " + sResultado);
                    }
                    catch (Exception ex)
                    {
                        clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + ex.Message + " - " + sResultado);
                    }

                }
                _shouldStop = true;
            }
        }
    }

    public static class ExtensionMethod
    {
        public static IEnumerable<List<cfdi>> Partition<cfdi>(this IList<cfdi> source, Int32 size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (Double)size); i++)
                yield return new List<cfdi>(source.Skip(size * i).Take(size));
        }

        public static ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip;
        public static MemoryStream bos;
        public static int nHilos;
    }
}




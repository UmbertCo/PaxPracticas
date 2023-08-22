using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using SolucionPruebas.WindowsService.GeneradorComprobantesHilos.Properties;
using SolucionPruebas.Presentacion.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SolucionPruebas.WindowsService.GeneradorComprobantesHilos
{
    public class clsGenerarComprobantes
    {
        public clsGenerarComprobantes()
        { 
        
        }

        public void fnGenerar()
        {
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            try
            {
                List<string> lArchivos = new List<string>();
                lArchivos = Directory.GetFiles(Settings.Default.RutaArchivos).ToList();

                clsLog.Escribir(Settings.Default.LogError + "Log" + DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year, DateTime.Now + " " + "Generando Comprobantes");

                string[] asArregloArchivos = Settings.Default.ArregloArchivos.Split(',');
                List<int> lnNumeroArchivos = new List<int>();
                foreach (string sNumeroArchivo in asArregloArchivos)
                {
                    lnNumeroArchivos.Add(Convert.ToInt32(sNumeroArchivo));
                }

                foreach (int nNumeroArchivos in lnNumeroArchivos)
                {
                    //bool bSeguir = true;
                    string sCarpeta = nNumeroArchivos.ToString() + "-" + Guid.NewGuid().ToString("N");

                    clsExtensionMethod.zip = null;
                    clsExtensionMethod.zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(Settings.Default.RutaSalida + @"\" + sCarpeta + ".zip"));
                    clsExtensionMethod.zip.SetLevel(6);

                    GenerarComprobantes cGenerarLayouts = new GenerarComprobantes(lArchivos, nNumeroArchivos, Settings.Default.DiasDiferencia);
                    Thread hilo = new Thread(new ThreadStart(cGenerarLayouts.fnGenerarComprobante));
                    hilo.Start();
                    hilo.Join();

                    using (FileStream fsSource = new FileStream(Settings.Default.RutaSalida + sCarpeta + ".zip", FileMode.Open))
                    {
                        Stream strZip = fsSource;
                        ZipFile archivoZip = new ZipFile(strZip);
                        long size = archivoZip.Count;
                        archivoZip.Close();

                        if (size == 0)
                        {
                            File.Delete(Settings.Default.RutaSalida + sCarpeta + ".zip");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + ex.Message);
            }
        }
    }

    public class GenerarComprobantes
    {
        List<string> gaArchivos;
        int gnNumeroRegistros;
        private Presentacion.Servicios.ServicioLocal.ServiceClient SDServicioLocal;
        private static Chilkat.Rsa rsa;
        private static Chilkat.PrivateKey key;
        private static Chilkat.PrivateKey pem;
        private static DateTime gdFecha;
        private static int gnContadorFecha = 0;
        //private static  

        public GenerarComprobantes(List<string> paArchivos, int pnNumeroRegistros, int pnDiasDiferencia)
        {
            gaArchivos = paArchivos;
            gnNumeroRegistros = pnNumeroRegistros;
            gdFecha = DateTime.Now.AddDays(pnDiasDiferencia);

            byte[] bLlavePrivada = null;
            Stream streamkey = File.Open(Settings.Default.RutaLlave, FileMode.Open);
            StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
            using (BinaryReader br = new BinaryReader(streamkey))
            {
                bLlavePrivada = br.ReadBytes(Convert.ToInt32(streamkey.Length));
            }

            key = new Chilkat.PrivateKey();
            key.LoadPkcs8Encrypted(bLlavePrivada, Settings.Default.Password);
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
        }

        public void fnGenerarComprobante()
        {
            bool bSeguir = true;
            int nContador = 0;
            try
            {
                while (bSeguir)
                {
                    foreach (string sRutaArchivo in gaArchivos)
                    {
                        string sCadenaOriginal = string.Empty;
                        string sSello = string.Empty;
                        string sArchivo = string.Empty;
                        XmlDocument xdComprobante;
                        XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());

                        Stream stArchivo = File.Open(sRutaArchivo, FileMode.Open);
                        StreamReader srArchivo = new StreamReader(stArchivo);
                        sArchivo = srArchivo.ReadToEnd();
                        srArchivo.Close();

                        xdComprobante = new XmlDocument();
                        nsm = new XmlNamespaceManager(xdComprobante.NameTable);
                        nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                        nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                        nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                        xdComprobante.LoadXml(sArchivo);

                        //Removemos la declaracion del Xml en caso de que exista
                        foreach (XmlNode xnNodo in xdComprobante)
                        {
                            if (xnNodo.NodeType.Equals(XmlNodeType.XmlDeclaration))
                                xdComprobante.RemoveChild(xnNodo);
                        }

                        //Removemos el nodo del timbra fiscal en caso de que exista
                        foreach (XmlNode xnNodo in xdComprobante.FirstChild.ChildNodes)
                        {
                            if (xnNodo.Name.Equals("cfdi:Complemento"))
                            {
                                foreach (XmlNode xnNodoComplemento in xnNodo)
                                {
                                    if (xnNodoComplemento.Name.Equals("tfd:TimbreFiscalDigital"))
                                    {
                                        xnNodo.RemoveChild(xnNodoComplemento);
                                    }
                                }
                            }
                        }

                        if (Settings.Default.Version.Equals("3.2"))
                            xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsm).SetValue(gdFecha.AddSeconds(gnContadorFecha).ToString("s"));
                        else
                            xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Fecha", nsm).SetValue(gdFecha.AddSeconds(gnContadorFecha).ToString("s"));

                        

                        //SDServicioLocal = Presentacion.Servicios.ProxyLocator.ObtenerServicioLocal();
                        //sCadenaOriginal = SDServicioLocal.fnAplicarHojaTransformacion(xdComprobante.InnerXml);

                        MemoryStream ms;
                        StreamReader srDll;
                        XsltArgumentList args;
                        XslCompiledTransform xslt;
                        XPathNavigator navNodoTimbre = null;

                        navNodoTimbre = xdComprobante.CreateNavigator();

                        xslt = new XslCompiledTransform();

                        if(Settings.Default.Version.Equals("3.2"))
                            xslt.Load(typeof(CaOri.V32));
                        else
                            xslt.Load(typeof(CaOri.V33));

                        ms = new MemoryStream();
                        args = new XsltArgumentList();
                        xslt.Transform(navNodoTimbre, args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);
                        sCadenaOriginal = srDll.ReadToEnd();

                        sSello = rsa.SignStringENC(sCadenaOriginal, Settings.Default.AlgoritmoDigestion);

                        if (Settings.Default.Version.Equals("3.2"))
                            xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsm).SetValue(sSello);
                        else
                            xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Sello", nsm).SetValue(sSello);



                        string sNombreArchivo = String.Format("{0}\\{1}.xml", Settings.Default.RutaSalida, nContador.ToString() + "-" + Guid.NewGuid().ToString("N"));
                        //string sNombreArchivo = String.Format("{0}\\{1}.xml", Settings.Default.RutaSalida, Path.GetFileNameWithoutExtension(sRutaArchivo));

                        xdComprobante.Save(sNombreArchivo);
                        fnAñadirFicheroaZip(clsExtensionMethod.zip, "/", sNombreArchivo);
                        File.Delete(sNombreArchivo);

                        nContador += 1;
                        gnContadorFecha += 1; 

                        if (nContador.Equals(gnNumeroRegistros))
                        {
                            bSeguir = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + ex.Message);
            }
            finally
            {
                clsExtensionMethod.zip.Finish();
                clsExtensionMethod.zip.Close();
            }
        }

        /// <summary>
        /// Función que agrega un archivo individual a un fichero ZIP que esta en memoria
        /// </summary>
        /// <param name="zStream">Stream</param>
        /// <param name="psRelativePath">Ruta relativa</param>
        /// <param name="psFile">Nombre del archivo</param>
        public static void fnAñadirFicheroaZip(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream, string psRelativePath, string psFile)
        {
            byte[] buffer = new byte[4096];
            string fileRelativePath = (psRelativePath.Length > 1 ? psRelativePath : string.Empty)
                                      + System.IO.Path.GetFileName(psFile);
            ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(fileRelativePath);
            zStream.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(psFile))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }

        public static void fnAñadirFicheroaZip(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream, string psFile)
        {
            // comprimir los ficheros del array en el zip indicado
            // si crearAuto = True, zipfile será el directorio en el que se guardará
            // y se generará automáticamente el nombre con la fecha y hora actual
            Crc32 objCrc32 = new Crc32();
            //
            string strFile = null;
            strFile = psFile;
            FileStream strmFile = File.OpenRead(strFile);
            byte[] abyBuffer = new byte[Convert.ToInt32(strmFile.Length - 1) + 1];
            //
            strmFile.Read(abyBuffer, 0, abyBuffer.Length);
            //
            //------------------------------------------------------------------
            // para guardar sólo el nombre del fichero
            // esto sólo se debe hacer si no se procesan directorios
            // que puedan contener nombres repetidos
            string sFile = Path.GetFileName(strFile);
            ZipEntry theEntry = new ZipEntry(sFile);
            //------------------------------------------------------------------
            // guardar la fecha y hora de la última modificación
            FileInfo fi = new FileInfo(strFile);
            theEntry.DateTime = fi.LastWriteTime;
            theEntry.Size = strmFile.Length;
            strmFile.Close();
            objCrc32.Reset();
            objCrc32.Update(abyBuffer);
            theEntry.Crc = objCrc32.Value;
            zStream.PutNextEntry(theEntry);
            zStream.Write(abyBuffer, 0, abyBuffer.Length);
        }
    }
}

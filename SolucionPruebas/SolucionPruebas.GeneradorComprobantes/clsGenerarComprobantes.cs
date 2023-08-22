using ICSharpCode.SharpZipLib.Zip;
using SolucionPruebas.GeneradorComprobantes.Properties;
using SolucionPruebas.Presentacion.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SolucionPruebas.GeneradorComprobantes
{
    public class clsGenerarComprobantes
    {
        public clsGenerarComprobantes()
        { 
        
        }

        private Presentacion.Servicios.ServicioLocal.ServiceClient SDServicioLocal;

        public void fnGenerar()
        {
            DateTime dFecha;
            int nDiasDiferencia = 0;
            int nContadorFecha = 0;
            string sCadenaOriginal;
            string sSello;
            string sArchivo;
            XmlDocument xdComprobante;
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            try
            {
                List<string> lArchivos = new List<string>();
                lArchivos = Directory.GetFiles(Settings.Default.RutaArchivos).ToList();
                nDiasDiferencia = Settings.Default.DiasDiferencia;
                dFecha = DateTime.Now.AddDays(nDiasDiferencia);

                clsLog.Escribir(Settings.Default.LogError + "Log" + DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year, DateTime.Now + " " + "Generando Comprobantes");

                string[] asArregloArchivos = Settings.Default.ArregloArchivos.Split(',');
                List<int> lnNumeroArchivos = new List<int>();
                foreach (string sNumeroArchivo in asArregloArchivos)
                {
                    lnNumeroArchivos.Add(Convert.ToInt32(sNumeroArchivo));
                }

                foreach (int nNumeroArchivos in lnNumeroArchivos)
                {
                    bool bSeguir = true;
                    string sCarpeta = nNumeroArchivos.ToString() + "-" + Guid.NewGuid().ToString("N");

                    clsExtensionMethod.zip = null;
                    clsExtensionMethod.zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(Settings.Default.RutaSalida + @"\" + sCarpeta + ".zip"));
                    clsExtensionMethod.zip.SetLevel(6);

                    int nContador = 0;
    
                    try
                    {
                        while (bSeguir)
                        {
                            foreach (string sRutaArchivo in lArchivos)
                            {
                                sArchivo = string.Empty;

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

                                xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsm).SetValue(dFecha.AddSeconds(nContadorFecha).ToString("s"));

                                SDServicioLocal = Presentacion.Servicios.ProxyLocator.ObtenerServicioLocal();
                                sCadenaOriginal = SDServicioLocal.fnAplicarHojaTransformacion(xdComprobante.InnerXml);

                                Stream streamkey = File.Open(Settings.Default.RutaLlave, FileMode.Open);
                                StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
                                byte[] bLlavePrivada = null;
                                using (BinaryReader br = new BinaryReader(streamkey))
                                {
                                    bLlavePrivada = br.ReadBytes(Convert.ToInt32(streamkey.Length));
                                }

                                sSello = SDServicioLocal.fnGenerarSello(Settings.Default.RutaPfx, sCadenaOriginal, bLlavePrivada, Settings.Default.Password);
                                sSello = sSello.Replace("\n", "");
                                xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsm).SetValue(sSello);

                                string sNombreArchivo = String.Format("{0}\\{1}.xml", Settings.Default.RutaSalida, nContador.ToString() + "-" + Guid.NewGuid().ToString("N"));
                                xdComprobante.Save(sNombreArchivo);
                                fnAñadirFicheroaZip(clsExtensionMethod.zip, "/", sNombreArchivo);
                                File.Delete(sNombreArchivo);

                                nContador += 1;
                                nContadorFecha += 1;

                                if (nContador.Equals(nNumeroArchivos))
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

                    using (FileStream fsSource = new FileStream(Properties.Settings.Default.RutaSalida + sCarpeta + ".zip", FileMode.Open))
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
    }
}

using ICSharpCode.SharpZipLib.Zip;
using PAXConectorFTPGTCFDI33.CancelacionASMX;
using PAXConectorFTPGTCFDI33.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace PAXConectorFTPGTCFDI33
{
    class MyFileSystemWatcherCancelacion : FileSystemWatcher
    {
        private wcfCancelaASMXSoapClient wsCancela = new wcfCancelaASMXSoapClient();
        ZipOutputStream zosArchivoError;

        public MyFileSystemWatcherCancelacion()
        {
            Init();
        }

        private void Init()
        {
            try
            {
                Path = Settings.Default.RutaCancelacionEntrada;
                InternalBufferSize = (8192 * 4);
                IncludeSubdirectories = false;
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
                Created += new FileSystemEventHandler(WatcherCancelacion_Created);
                EnableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                clsLogCancelacion.WriteLine("Error al momento de Iniciar el Servicio de Cancelación. " + " " + DateTime.Now + " " + ex.Message);
                clsLogRespaldoCancelacion.WriteLine("Error al momento de Iniciar el Servicio de Cancelación. " + " " + DateTime.Now + " " + ex.Message);
            }
        }

        void WatcherCancelacion_Created(object sender, FileSystemEventArgs e)
        {
            DateTime Fecha = DateTime.Today;
            string sLineasValidas = "";
            string sLineasErroneas = "";
            try
            {
                while ((fnWaitForFile(e.FullPath) == false))
                {
                    //Se hace pato un rato.(Se espera a que se desbloquee el Archivo)
                }

                clsLogCancelacion.WriteLine(DateTime.Now + " " + "Archivo Agregado " + e.FullPath);
                clsLogRespaldoCancelacion.WriteLine(DateTime.Now + " " + "Archivo Agregado " + e.FullPath);

                if (System.IO.Path.GetFileName(e.FullPath) == String.Empty)
                {
                    return;
                }

                // Se crea el zip de Errores
                zosArchivoError = new ZipOutputStream(File.Create(Settings.Default.RutaCancelacionErrores + System.IO.Path.GetFileNameWithoutExtension(e.Name) + ".zip"));
                zosArchivoError.SetLevel(6);

                // Creamos el zip donde van a estar contenidos los recibos de los UUI´s cancelados
                clsExtensionMethod.zip = null;
                clsExtensionMethod.zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(Settings.Default.RutaCancelacionSalida + @"\" + System.IO.Path.GetFileNameWithoutExtension(e.FullPath) + ".zip"));
                clsExtensionMethod.zip.SetLevel(6);

                // Se respalda el archivo dejado en la carpeta de entrada
                //File.Copy(e.FullPath, Settings.Default.RutaEntradaRespaldo + e.Name, true);

                // Inicializamos 
                string sCancelados = DateTime.Now + " Cancelados en " + System.IO.Path.GetFileName(e.FullPath) + ":";
                bool bCancelados = false;
                bool bRechazados = false;
                string sRechazados = DateTime.Now + " Rechazados en " + System.IO.Path.GetFileName(e.FullPath) + ":";

                try
                {
                    if (System.IO.Path.GetExtension(e.Name).ToLower() != ".zip")
                    {
                        throw new OperationCanceledException("El documento no es un archivo comprimido (zip)" + e.FullPath);
                    }

                    using (FileStream fsSource = new FileStream(e.FullPath, FileMode.Open))
                    {
                        Stream strZip = fsSource;
                        Stream sLayout = null;
                        ZipFile archivoZip = new ZipFile(strZip);

                        foreach (ZipEntry zipEntry in archivoZip)
                        {
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
                                byte[] buffer = new byte[4096];

                                // Se verifica que el archivo contenido es un txt
                                if (System.IO.Path.GetExtension(zipEntry.Name).ToLower() != ".txt")
                                {
                                    clsLogCancelacion.WriteLine(DateTime.Now + "Archivo Incorrecto " + zipEntry.Name);
                                    clsLogRespaldoCancelacion.WriteLine(DateTime.Now + "Archivo Incorrecto " + zipEntry.Name);

                                    using (var stream = new MemoryStream())
                                    {
                                        archivoZip.GetInputStream(zipEntry).CopyTo(stream);
                                        File.WriteAllBytes(Settings.Default.RutaCancelacionErrores + zipEntry.Name, stream.ToArray());
                                    }
                                    fnAñadirFicheroaZip(zosArchivoError, "/", Settings.Default.RutaCancelacionErrores + zipEntry.Name);
                                    File.Delete(Settings.Default.RutaCancelacionErrores + zipEntry.Name);

                                    continue;
                                }

                                try
                                {
                                    sLayout = (Stream)archivoZip.GetInputStream(zipEntry);

                                    StreamReader reader = new StreamReader(sLayout);
                                    while (true)
                                    {
                                        string linea = reader.ReadLine();
                                        if (string.IsNullOrEmpty(linea))
                                            break;

                                        string[] datos = linea.Split('|');

                                        if (!datos.Length.Equals(4))
                                        {
                                            // Se crea el acuse de cancelación en la ruta de errores
                                            File.WriteAllText(Settings.Default.RutaCancelacionErrores + linea + ".txt", "La estructura de la linea esta mal formada: " + linea);
                                            // Se agrega el archivo del acuse de cancelación a la carpeta comprimida
                                            fnAñadirFicheroaZip(zosArchivoError, "/", Settings.Default.RutaCancelacionErrores + linea + ".txt");
                                            // Se elimna el acuse de cancelación en la ruta de respaldo despues de haberse agregado al archivo ZIP
                                            File.Delete(Settings.Default.RutaCancelacionErrores + linea + ".txt");

                                            sLineasErroneas += System.Environment.NewLine + linea;
                                            sRechazados += System.Environment.NewLine + linea;
                                            bRechazados = true;
                                            continue;
                                        }

                                        CancelacionASMX.ArrayOfString uuid = new CancelacionASMX.ArrayOfString();
                                        uuid.Add(datos[0]);
                                        string rfc = datos[1];
                                        ArrayOfString rfcReceptor = new ArrayOfString { datos[2] };
                                        ArrayOfString total = new ArrayOfString { datos[3] };
                                        string usuario = Settings.Default.Usuario;
                                        string password = Settings.Default.Password;
                                        XmlDocument xResultado = new XmlDocument();
                                        try
                                        {
                                            string uuidEstatus = string.Empty;
                                            string uuidDescripcion = string.Empty;
                                            string uuidFecha = string.Empty;
                                            string resultado = wsCancela.fnCancelarXML(uuid, rfc, rfcReceptor, total, usuario, password);

                                            try
                                            {
                                                try { xResultado.LoadXml(resultado); }
                                                catch { xResultado.LoadXml(fnFormarErrorXML(resultado, datos[0])); }

                                                File.WriteAllText(Settings.Default.RutaCancelacionRespaldo + datos[0] + ".xml", xResultado.OuterXml);

                                                XmlElement xFolios = (XmlElement)xResultado.GetElementsByTagName("Folios")[0];
                                                uuidEstatus = xFolios.GetElementsByTagName("UUIDEstatus")[0].InnerText.Trim();
                                                uuidDescripcion = xFolios.GetElementsByTagName("UUIDdescripcion")[0].InnerText.Trim();
                                                uuidFecha = xFolios.GetElementsByTagName("UUIDfecha")[0].InnerText.Replace(" - ", "").Trim();
                                            }
                                            catch (Exception ex)
                                            {
                                                uuidDescripcion = resultado;
                                                sLineasErroneas += linea + System.Environment.NewLine;
                                            }

                                            lock (clsExtensionMethod.zip)
                                            {
                                                if (fnBuscaClaves(uuidEstatus, "/Cancelado/Clave", "Clave")) //Factura Cancelada
                                                {
                                                    if (uuidEstatus == "201")
                                                    {
                                                        sCancelados += "\n" + uuid[0];
                                                        bCancelados = true;
                                                        sLineasValidas += System.Environment.NewLine + linea;

                                                        File.WriteAllText(Settings.Default.RutaCancelacionSalida + datos[0] + ".xml", xResultado.OuterXml);
                                                    }
                                                    else
                                                    {
                                                        sCancelados += System.Environment.NewLine + " [Cancelado Anteriormente] " + uuid[0];
                                                        bCancelados = true;
                                                        sLineasValidas += System.Environment.NewLine + linea;
                                                    }
                                                }
                                                else
                                                {
                                                    sRechazados += System.Environment.NewLine + uuid[0] + " " + uuidDescripcion;
                                                    bRechazados = true;
                                                    sLineasErroneas += System.Environment.NewLine + linea;

                                                    if (string.IsNullOrEmpty(xResultado.OuterXml))
                                                    {
                                                        // Se crea el acuse de cancelación en la ruta de errores
                                                        File.WriteAllText(Settings.Default.RutaCancelacionErrores + datos[0] + ".txt", resultado);
                                                        // Se agrega el archivo del acuse de cancelación a la carpeta comprimida
                                                        fnAñadirFicheroaZip(zosArchivoError, "/", Properties.Settings.Default.RutaCancelacionErrores + datos[0] + ".txt");
                                                        // Se elimna el acuse de cancelación en la ruta de respaldo despues de haberse agregado al archivo ZIP
                                                        File.Delete(Settings.Default.RutaCancelacionErrores + datos[0] + ".txt");
                                                    }
                                                    else
                                                    {
                                                        // Se crea el acuse de cancelación en la ruta de errores
                                                        File.WriteAllText(Settings.Default.RutaCancelacionErrores + datos[0] + ".xml", xResultado.OuterXml);
                                                        // Se agrega el archivo del acuse de cancelación a la carpeta comprimida
                                                        fnAñadirFicheroaZip(zosArchivoError, "/", Properties.Settings.Default.RutaCancelacionErrores + datos[0] + ".xml");
                                                        // Se elimna el acuse de cancelación en la ruta de respaldo despues de haberse agregado al archivo ZIP
                                                        File.Delete(Settings.Default.RutaCancelacionErrores + datos[0] + ".xml");
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception(ex.Message);
                                        }

                                        if (File.Exists(Settings.Default.RutaCancelacionSalida + datos[0] + ".xml"))
                                        {
                                            fnAñadirFicheroaZip(clsExtensionMethod.zip, "/", Settings.Default.RutaCancelacionSalida + datos[0] + ".xml");
                                            File.Delete(Settings.Default.RutaCancelacionSalida + datos[0] + ".xml");
                                        }
                                    }

                                    if (bCancelados)
                                    {
                                        clsLogCancelacion.WriteLine(sCancelados);
                                        clsLogRespaldoCancelacion.WriteLine(sCancelados);
                                    }
                                    if (bRechazados)
                                    {
                                        clsLogCancelacion.WriteLine(sRechazados);
                                        clsLogRespaldoCancelacion.WriteLine(sRechazados);
                                    }

                                    //try
                                    //{
                                    //    if (bCancelados)
                                    //    {
                                    //        clsLog.Escribir(Settings.Default.RutaDocsCancelados + "Cancelados", sLineasValidas);
                                    //    }
                                    //    if (bRechazados)
                                    //    {
                                    //        clsLog.Escribir(Settings.Default.RutaDocsCancelacionError + "Rechazados", sLineasErroneas);
                                    //    }

                                    //    //File.Delete(e.FullPath);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    clsLog.Escribir(Settings.Default.RutaCancelacionLog + "LogError", "Se ha generado un error " + e.FullPath + " " + DateTime.Now + " " + ex.Message);
                                    //    LogRespaldo.WriteLine("Se ha generado un error " + e.FullPath + " " + DateTime.Now + " " + ex.Message);
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    clsLogCancelacion.WriteLine(DateTime.Now + " Archivo de texto Incorrecto " + Settings.Default.RutaCancelacionLog + zipEntry.Name + " " + "EXCEPTION: " + ex.Message);
                                    clsLogRespaldoCancelacion.WriteLine(DateTime.Now + " Archivo de texto Incorrecto " + Settings.Default.RutaCancelacionLogRespaldo + zipEntry.Name + " " + "EXCEPTION: " + ex.Message);

                                    // Se crea el acuse de cancelación en la ruta de errores
                                    File.WriteAllText(Settings.Default.RutaCancelacionErrores + "Error_" + System.IO.Path.GetFileNameWithoutExtension(zipEntry.Name) + ".txt", DateTime.Now + " Archivo de texto Incorrecto " + zipEntry.Name + " " + "EXCEPTION: " + ex.Message);

                                    // Se guarda el excel en la carpeta de Incorrectos
                                    using (var stream = new MemoryStream())
                                    {
                                        archivoZip.GetInputStream(zipEntry).CopyTo(stream);
                                        File.WriteAllBytes(Settings.Default.RutaCancelacionErrores + zipEntry.Name, stream.ToArray());
                                    }

                                    // Se agrega el archivo del acuse de cancelación a la carpeta comprimida
                                    fnAñadirFicheroaZip(zosArchivoError, "/", Settings.Default.RutaCancelacionErrores + zipEntry.Name);
                                    fnAñadirFicheroaZip(zosArchivoError, "/", Settings.Default.RutaCancelacionErrores + "Error_" + System.IO.Path.GetFileNameWithoutExtension(zipEntry.Name) + ".txt");
                                    // Se elimna el acuse de cancelación en la ruta de respaldo despues de haberse agregado al archivo ZIP
                                    File.Delete(Settings.Default.RutaCancelacionErrores + "Error_" + zipEntry.Name);
                                    File.Delete(Settings.Default.RutaCancelacionErrores + zipEntry.Name);
                                }
                            }
                            catch (OperationCanceledException)
                            {
                                throw new OperationCanceledException();
                            }
                            catch (Exception)
                            {
                                clsLogCancelacion.WriteLine(DateTime.Now + "Archivo Incorrecto " + zipEntry.Name);
                                clsLogRespaldoCancelacion.WriteLine(DateTime.Now + "Archivo Incorrecto " + zipEntry.Name);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsLogCancelacion.WriteLine(DateTime.Now + " Error al cancelar. " + e.FullPath + " " + ex.Message);
                    clsLogRespaldoCancelacion.WriteLine(DateTime.Now + " Error al cancelar. " + e.FullPath + " " + ex.Message);

                    File.Copy(e.FullPath, Settings.Default.RutaCancelacionErrores + e.Name, true);
                    fnAñadirFicheroaZip(zosArchivoError, "/", Settings.Default.RutaCancelacionErrores + e.Name);
                    File.Delete(Settings.Default.RutaCancelacionErrores + e.Name);
                }
                finally
                {
                    // Cerramos el zip donde estan contenidos los UUID´s que se cancelaron correctamente
                    clsExtensionMethod.zip.Finish();
                    clsExtensionMethod.zip.Close();

                    // Cerramos el zip donde estan contenidos los errores
                    zosArchivoError.Finish();
                    zosArchivoError.Close();

                    // Eliminamos el archivo origen
                    File.Delete(e.FullPath);
                }

                // Se respalda el archivo de la salida de los UUID´s cancelados
                using (FileStream fsSource = new FileStream(Properties.Settings.Default.RutaCancelacionSalida + System.IO.Path.GetFileNameWithoutExtension(e.Name) + ".zip", FileMode.Open))
                {
                    Stream strZip = fsSource;
                    ZipFile archivoZip = new ZipFile(strZip);
                    long size = archivoZip.Count;
                    archivoZip.Close();

                    if (size == 0)
                    {
                        File.Delete(Settings.Default.RutaCancelacionSalida + System.IO.Path.GetFileNameWithoutExtension(e.Name) + ".zip");
                    }
                    //else
                    //{
                    //    File.Copy(Settings.Default.RutaCancelacionSalida + e.Name, Settings.Default.RutaCancelacionRespaldo + e.Name);
                    //}
                }

                // Borramos el zip que se genero en la carpeta de Errores en dado caso que todos los UUID's se hayan cancelado correctamente
                using (FileStream fsSource = new FileStream(Properties.Settings.Default.RutaCancelacionErrores + System.IO.Path.GetFileNameWithoutExtension(e.Name) + ".zip", FileMode.Open))
                {
                    Stream strZip = fsSource;
                    ZipFile archivoZip = new ZipFile(strZip);
                    long size = archivoZip.Count;
                    archivoZip.Close();

                    if (size == 0)
                    {
                        File.Delete(Settings.Default.RutaCancelacionErrores + System.IO.Path.GetFileNameWithoutExtension(e.Name) + ".zip");
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogCancelacion.WriteLine(DateTime.Now + " Error al cancelar. " + e.FullPath + " " + ex.Message);
                clsLogRespaldoCancelacion.WriteLine(DateTime.Now + " Error al cancelar. " + e.FullPath + " " + ex.Message);
            }
            finally
            {
                clsLogCancelacion.WriteLine(DateTime.Now + " " + "Archivo Finalizado " + e.FullPath);
                clsLogRespaldoCancelacion.WriteLine(DateTime.Now + " " + "Archivo Finalizado " + e.FullPath);
            }
        }

        /// <summary>
        /// Función que agrega un archivo individual a un fichero ZIP que esta en memoria
        /// </summary>
        /// <param name="zStream">Stream</param>
        /// <param name="psRelativePath">Ruta relativa</param>
        /// <param name="psFile">Nombre del archivo</param>
        private static void fnAñadirFicheroaZip(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream, string psRelativePath, string psFile)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="psClave"></param>
        /// <param name="psRuta"></param>
        /// <param name="psAtributo"></param>
        /// <returns></returns>
        /// <cmmi>
        /// <requerimiento>REQ9</requerimiento>
        /// </cmmi>
        private static bool fnBuscaClaves(string psClave, string psRuta, string psAtributo)
        {
            bool bRetorno = false;
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + "Codigos.xml"));

                XmlNamespaceManager nsmClaves = new XmlNamespaceManager(xml.NameTable);

                //XmlNodeList xnlConcepto = xdClaves.SelectNodes("/c:ListaConceptos/c:Conceptos/c:Concepto[@Concepto='" + psClave + "']", nsmClaves);
                XmlNodeList xnlConcepto = xml.SelectNodes(psRuta + "[@" + psAtributo + "='" + psClave + "']", nsmClaves);

                if (xnlConcepto.Count > 0)
                    bRetorno = true;
            }
            catch (Exception ex)
            {
                //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnBuscaClaves", "webConsultaCFDI");
            }
            return bRetorno;
        }

        /// <summary>
        /// Función que se encarga de generar un xml en base al mensaje de error que nos devuelve el web service de timbrado
        /// </summary>
        /// <param name="psRespuesta">Respuesta</param>
        /// <param name="psUUID">UUID</param>
        /// <returns></returns>
        private static string fnFormarErrorXML(string psRespuesta, string psUUID)
        {
            StringBuilder sbError = new StringBuilder();
            try
            {
                string[] asError = psRespuesta.Split('-');
                sbError.Append("<Cancelacion>");
                sbError.Append("<Folios>");

                sbError.Append("<UUIDEstatus>");
                sbError.Append(asError[0]);
                sbError.Append("</UUIDEstatus>");

                sbError.Append("<UUIDdescripcion>");
                sbError.Append(asError[1]);
                sbError.Append("</UUIDdescripcion>");

                sbError.Append("<UUIDfecha>");
                sbError.Append(DateTime.Now.ToString("s"));
                sbError.Append("</UUIDfecha>");

                sbError.Append("</Folios>");
                sbError.Append("</Cancelacion>");
            }
            catch (Exception ex)
            {

            }
            return sbError.ToString();
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
                        throw new Exception("No se pudo Utilizar el archivo por que otro proceso lo esta utilizando");
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            }
            return true;
        }
    }
}

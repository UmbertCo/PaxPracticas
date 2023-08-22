using PAXConectorFTPGTCFDI33.Properties;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

namespace PAXConectorFTPGTCFDI33
{
    public class Worker
    {
        private volatile bool _shouldStop;

        public void DoWork(object pObj)
        {
            Object[] objArr = (Object[])pObj;

            clsCfdi[] list = (clsCfdi[])objArr[0];
            int nRetorno = (int)objArr[1];

            try
            {
                List<clsCfdi> lRehacer = new List<clsCfdi>();
                while (!_shouldStop)
                {
                    foreach (clsCfdi prime in list)
                    {
                        try
                        {
                            string responseFromServer = string.Empty;

                      

                            XmlDocument xDocSinAddenda = new XmlDocument();

                            xDocSinAddenda.LoadXml(prime.XML);

                            if (xDocSinAddenda.FirstChild.NodeType != XmlNodeType.XmlDeclaration)
                            {
                                xDocSinAddenda.InsertBefore(xDocSinAddenda.CreateXmlDeclaration("1.0", "utf-8", null), xDocSinAddenda.DocumentElement);
                            }

                            xDocSinAddenda.LoadXml(xDocSinAddenda.OuterXml);

                            //Quitamos el nodo de la addenda del XMLDocument
                            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocSinAddenda.NameTable);
                            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                            nsmComprobante.AddNamespace("nomina12", "http://www.sat.gob.mx/nomina12");
                            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                            bool bAddenda = true;

                            String xAddenda = "";
                            try
                            {
                                xAddenda = xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmComprobante).OuterXml;
                                xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmComprobante).DeleteSelf();
                                //xAddenda = xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:c/cfdi:Addenda", nsmComprobante).OuterXml;
                               //xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmComprobante).DeleteSelf();
                            }
                            catch
                            {
                                bAddenda = false;

                            }

                            string postData = xDocSinAddenda.InnerXml;

                            Exception exMultipac = null;
                            
                            //responseFromServer = MultiPAC.Integracion.Comercio_Digital.Timbrado(xDocSinAddenda.OuterXml, ConfigurationManager.AppSettings["URIMultiPAC"], ConfigurationManager.AppSettings["HOSTMultiPAC"], ConfigurationManager.AppSettings["TipoMultiPAC"], ConfigurationManager.AppSettings["UsuarioMultiPAC"], ConfigurationManager.AppSettings["PWDMultiPAC"], ref exMultipac);
                            responseFromServer = MultiPAC.Integracion.Comercio_Digital.Timbrado(xDocSinAddenda.OuterXml, Settings.Default.URIMultiPAC, Settings.Default.HOSTMultiPAC, Settings.Default.TipoMultiPAC, Settings.Default.UsuarioMultiPAC, Settings.Default.PWDMultiPAC, ref exMultipac);
                            //responseFromServer = MultiPAC.Integracion.Comercio_Digital.Timbrado(xDocSinAddenda.OuterXml, Settings.Default.URIMultiPAC, Settings.Default.HOSTMultiPAC, Settings.Default.TipoMultiPAC, Settings.Default.UsuarioMultiPAC, Settings.Default.PWDMultiPAC, ref exMultipac);
                            //responseFromServer = MultiPAC.Integracion.Comercio_Digital.Timbrado(postData, "https://pruebas.comercio-digital.mx/timbre4/timbrarV5", "pruebas.comercio-digital.mx", "XML", "AAA010101AAA", "PWD", ref exMultipac);
                            
                            string response = "Hizo la peticion";



                            //string response = "xDocSinAddenda" + xDocSinAddenda.OuterXml + " URIMultiPAC" + ConfigurationManager.AppSettings["URIMultiPAC"] + " HOSTMultiPAC " + ConfigurationManager.AppSettings["HOSTMultiPAC"] +  "TipoMultiPAC " + ConfigurationManager.AppSettings["TipoMultiPAC"]+ "UsuarioMultiPAC " +  ConfigurationManager.AppSettings["UsuarioMultiPAC"] + "PWDMultiPAC " + ConfigurationManager.AppSettings["PWDMultiPAC"] + "FIN ";
                            //string response = "xDocSinAddenda" + xDocSinAddenda.OuterXml + " URIMultiPAC" + Settings.Default.URIMultiPAC + " HOSTMultiPAC " + Settings.Default.HOSTMultiPAC + "TipoMultiPAC " + Settings.Default.TipoMultiPAC + "UsuarioMultiPAC " + Settings.Default.UsuarioMultiPAC  + "PWDMultiPAC " + Settings.Default.PWDMultiPAC;
                            response = "postData" + postData + " URIMultiPAC " + Settings.Default.URIMultiPAC + " HOSTMultiPAC " + Settings.Default.HOSTMultiPAC + "TipoMultiPAC " + Settings.Default.TipoMultiPAC + "UsuarioMultiPAC " + Settings.Default.UsuarioMultiPAC + "PWDMultiPAC " + Settings.Default.PWDMultiPAC;

                            if (exMultipac != null)
                            {
                                if (fnErrorRfcReceptor(responseFromServer))
                                {
                                    lock (clsExtensionMethod.sRutaRFCCambio)
                                    {
                                        string sMoneda = string.Empty;
                                        XmlDocument xdDocNuevo = new XmlDocument();

                                        xdDocNuevo.LoadXml(prime.XML);
                                        xdDocNuevo.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Rfc", nsmComprobante).SetValue("XAXX010101000");
                                        //Actualizado con CFDI 4.0
                                        xdDocNuevo.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@DomicilioFiscalReceptor", nsmComprobante).SetValue("01219");
                                        xdDocNuevo.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@RegimenFiscalReceptor", nsmComprobante).SetValue("616");
                                        xdDocNuevo.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@UsoCFDI", nsmComprobante).SetValue("CP01");

                                        



                                        xdDocNuevo = clsUtiles.fnResellar(xdDocNuevo);

                                        prime.XML = xdDocNuevo.OuterXml;

                                        lRehacer.Add(prime);


                                    }


                                }
                                else
                                {
                                    clsLog.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + prime.FileName + ".xml" + " " + DateTime.Now);
                                    clsLogRespaldo.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + prime.FileName + ".xml" + " " + DateTime.Now);

                                    File.WriteAllText(Properties.Settings.Default.FolderIncorrectos + @"\" + prime.FileName + ".xml", prime.XML, UTF8Encoding.UTF8);
                                    File.WriteAllText(Properties.Settings.Default.FolderIncorrectos + @"\" + "Error_" + prime.FileName + ".xml", responseFromServer + "request : " + response, UTF8Encoding.UTF8);

                                    if (Settings.Default.LogFlush)
                                    {
                                        clsLog.fnFlush("<HiloErr Hilo='" + Thread.CurrentThread.Name + "' ArchivoXML='" + prime.FileName + "'>" + responseFromServer + "///" + "</HiloErr>");
                                    }
                                }

                                continue;

                            }

                            if (!responseFromServer.Contains("<cfdi:Comprobante"))
                            {

                                if (fnErrorRfcReceptor(responseFromServer))
                                {
                                    lock (clsExtensionMethod.sRutaRFCCambio)
                                    {
                                        string sMoneda = string.Empty;
                                        XmlDocument xdDocNuevo = new XmlDocument();

                                        xdDocNuevo.LoadXml(prime.XML);
                                        xdDocNuevo.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@Rfc", nsmComprobante).SetValue("XAXX010101000");

                                        xdDocNuevo = clsUtiles.fnResellar(xdDocNuevo);

                                        prime.XML = xdDocNuevo.OuterXml;

                                        lRehacer.Add(prime);


                                    }

                                  
                                }
                                else
                                {
                                    clsLog.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + prime.FileName + ".xml" + " " + DateTime.Now);
                                    clsLogRespaldo.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + prime.FileName + ".xml" + " " + DateTime.Now);

                                    File.WriteAllText(Properties.Settings.Default.FolderIncorrectos + @"\" + prime.FileName + ".xml", prime.XML, UTF8Encoding.UTF8);
                                    File.WriteAllText(Properties.Settings.Default.FolderIncorrectos + @"\" + "ErrorDEV_" + prime.FileName + ".xml", responseFromServer + "request : " + response, UTF8Encoding.UTF8);

                                    if (Settings.Default.LogFlush)
                                    {
                                        clsLog.fnFlush("<HiloErr Hilo='" + Thread.CurrentThread.Name + "' ArchivoXML='" + prime.FileName + "'>" + responseFromServer + "///" + "</HiloErr>");
                                    }
                                }

                                continue;
                            }

                         



                            XmlDocument xDocumentXML = new XmlDocument();
                                xDocumentXML.LoadXml(responseFromServer);

                                XmlDocument xDonConAddenda = new XmlDocument();
                                xDonConAddenda.LoadXml(xDocumentXML.OuterXml);


                                XmlNamespaceManager namespaceMan = new XmlNamespaceManager(xDonConAddenda.NameTable);
                                namespaceMan.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                                namespaceMan.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                                nsmComprobante.AddNamespace("nomina12", "http://www.sat.gob.mx/nomina12");

                                if (bAddenda)
                                    xDonConAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante", namespaceMan).AppendChild(xAddenda);
                                //xDonConAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante", namespaceMan).AppendChild(xAddenda);

                                try
                                {
                                    lock (clsExtensionMethod.zip)
                                    {
                                        try
                                        {
                                            clsExtensionMethod.htHilosRepetidos.Add(prime.FileName, "0");
                                        }
                                        catch
                                        {
                                            int aux = int.Parse(clsExtensionMethod.htHilosRepetidos[prime.FileName].ToString());
                                            clsExtensionMethod.htHilosRepetidos[prime.FileName] = aux.ToString();
                                            continue;
                                        }

                                        using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(Settings.Default.Conexion)))
                                        {
                                            conexion.Open();
                                            using (SqlTransaction tran = conexion.BeginTransaction())
                                            {
                                                try
                                                {


                                                    StreamWriter sr4 = new StreamWriter(Properties.Settings.Default.FolderSalida + @"\" + prime.FileName + ".xml");
                                                    sr4.WriteLine(xDonConAddenda.OuterXml);
                                                    sr4.Close();



                                                    clsUtiles.fnAñadirFicheroaZip(clsExtensionMethod.zip, "/", Properties.Settings.Default.FolderSalida + @"\" + prime.FileName + ".xml");


                                                    try
                                                    {
                                                        File.Delete(Settings.Default.FolderSalida + @"\" + prime.FileName + ".xml");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        clsLog.WriteLine("Se ha generado un error al momento de generar el PDF " + @"\" + prime.FileName + ".xml" + " " + ex.Message + " " + DateTime.Now);
                                                        clsLogRespaldo.WriteLine("Se ha generado un error al momento de generar el PDF " + @"\" + prime.FileName + ".xml" + " " + ex.Message + " " + DateTime.Now);
                                                    }

                                                responseFromServer = responseFromServer.Replace(string.Concat("<?xml version=", "\"1.0\"", " encoding=", "\"UTF-8\"", "?>"), "").Replace(string.Concat("<?xml version=", "\"1.0\"", " encoding=", "\"utf-8\"", "?>"), "").Trim();

                                                using (SqlCommand command = new SqlCommand("usp_mpac40_Comprobante_Datos_Ins", conexion))
                                                    {
                                                        command.Transaction = tran;
                                                        command.CommandType = CommandType.StoredProcedure;
                                                        command.Parameters.AddWithValue("sXml", responseFromServer);
                                                        command.Parameters.AddWithValue("sOrigen", Properties.Settings.Default.MonitorFolder);
                                                        command.Parameters.AddWithValue("sNombre_archivo", prime.FileName + ".xml");
                                                        command.Parameters.AddWithValue("sEstatus", "P");
                                                        command.Parameters.AddWithValue("nIdZip", nRetorno);
                                                        command.Parameters.AddWithValue("nIdPac", Settings.Default.IdPac);

                                                    if (Settings.Default.ValidacionHash)
                                                            command.Parameters.AddWithValue("sDatos", prime.HashEmisor);

                                                        command.ExecuteNonQuery();
                                                    }

                                                    tran.Commit();
                                                }
                                                catch (Exception ex)
                                                {
                                                    tran.Rollback();

                                                    StreamWriter sr4 = new StreamWriter(Properties.Settings.Default.FolderPendientes + @"\" + prime.FileName + ".xml", true);
                                                    sr4.WriteLine(responseFromServer);
                                                    sr4.Close();

                                                    if (Settings.Default.LogFlush)
                                                    {
                                                        clsLog.fnFlush("<HiloErr Hilo='" + Thread.CurrentThread.Name + "' ArchivoXML='" + prime.FileName + "'>" + ex.Message + "///" + ex.StackTrace + "</HiloErr>");

                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    clsLog.WriteLine(ex + " " + DateTime.Now);
                                    clsLogRespaldo.WriteLine(ex + " " + DateTime.Now);

                                    StreamWriter sr4 = new StreamWriter(Properties.Settings.Default.FolderPendientes + @"\" + prime.FileName + ".xml", true);
                                    sr4.WriteLine(responseFromServer);
                                    sr4.Close();

                                    clsLog.WriteLine("Archivo XML Pendiente " + Properties.Settings.Default.FolderPendientes + @"\" + prime.FileName + ".xml" + " " + DateTime.Now);
                                    clsLogRespaldo.WriteLine("Archivo XML Pendiente " + Properties.Settings.Default.FolderPendientes + @"\" + prime.FileName + ".xml" + " " + DateTime.Now);

                                    if (Settings.Default.LogFlush)
                                    {
                                        clsLog.fnFlush("<HiloErr Hilo='" + Thread.CurrentThread.Name + "' ArchivoXML='" + prime.FileName + "'>" + ex.Message + "///" + ex.StackTrace + "</HiloErr>");
                                    }
                                }
                           
                        }
                        catch (Exception ex)
                        {
                            clsLog.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + prime.FileName + ".xml" + " " + DateTime.Now + " " + ex.Message);
                            clsLogRespaldo.WriteLine("Archivo XML Incorrecto " + Properties.Settings.Default.FolderIncorrectos + @"\" + prime.FileName + ".xml" + " " + DateTime.Now + " " + ex.Message);

                            File.WriteAllText(Properties.Settings.Default.FolderIncorrectos + @"\" + prime.FileName + ".xml", prime.XML, UTF8Encoding.UTF8);

                            if (Settings.Default.LogFlush)
                            {
                                clsLog.fnFlush("<HiloErr Hilo='" + Thread.CurrentThread.Name + "' ArchivoXML='" + prime.FileName + "'>" + ex.Message + "///" + ex.StackTrace + "</HiloErr>");
                            }
                        }
                        finally
                        {
                            lock (clsExtensionMethod.lArchivosProcesados)
                            {
                                clsExtensionMethod.lArchivosProcesados.Add(prime.FileName);
                            }
                        }

                    }

                    if (lRehacer.Count > 0)
                    {
                        list = lRehacer.ToArray();

                        lRehacer.Clear();
                    }
                    else
                    {
                        _shouldStop = true;
                    }

                }

                list = null;
            }
            catch (Exception ex)
            {
                if (Settings.Default.LogFlush)
                {
                    clsLog.fnFlush("Error Critico '" + ex + "' Hilo:'" + Thread.CurrentThread.Name + "///" + ex.StackTrace + "'");
                }
            }
            clsExtensionMethod.nHilos++;
        }



        /// <summary>
        /// Obtiene el codigo de error de la respuesta del servicio
        /// </summary>
        /// <param name="sResponseFromServer"></param>
        /// <returns></returns>
        private static bool fnErrorRfcReceptor(string sResponseFromServer)
        {
            string sCodigo = string.Empty;
            string sDescripcion = string.Empty;
            try
            {
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.LoadXml(sResponseFromServer);
                //XmlNode xmlNodo = xmlDoc.SelectSingleNode("/Response/Error/Code");
                //sCodigo = xmlNodo.InnerText;

                //XmlNode xmlNodoDescripcion = xmlDoc.SelectSingleNode("/Response/Error/Description");
                //sDescripcion = xmlNodoDescripcion.InnerText;

                //if (sCodigo.Trim().Equals("CFDI33132"))
                //{
                //    return true;
                //}

                if (sResponseFromServer.Trim().Contains("CFDI33132"))
                {
                    return true;
                }

                if (sResponseFromServer.Trim().Contains("CFDI40143"))
                {
                    return true;
                }

            }
            catch { }
            return false;
        }
    }
}

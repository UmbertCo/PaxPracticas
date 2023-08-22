using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace PAXRecuperacionSOAPBateriaGT
{
    class clsTimbrado
    {
        clsRecuperacionSOAP RecuperacionSOAP = new clsRecuperacionSOAP();

        public void fnTimbrado()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsTimbrado.AcceptAllCertificatePolicy);

                string sXML, snombreDoc, sVersion, sUsuario, sPassword, sOrigen, psTipoDocumento;
                DateTime Fecha = DateTime.Today;
                char[] cCad = { '-' };
                string[] sCad;

                sVersion = sUsuario = sPassword = sOrigen = psTipoDocumento = string.Empty;

                sVersion = "3.2";
                sUsuario = "paxgeneracion";
                sPassword = "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB";
                psTipoDocumento = "Factura";
                sOrigen = "GT";

                string[] files = Directory.GetFiles(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["rutaArchivos"].ToString(), "*.txt");
                //Se recorre los xml contenidos en el directorio
                foreach (string sXMLs in files)
                {
                    wsGeneracionTimbradoWS.wcfRecepcionASMXSoapClient facturacion = new wsGeneracionTimbradoWS.wcfRecepcionASMXSoapClient();
                    wsGeneracionTimbradoSVCWS.IwcfRecepcionClient facturacionSVC = new wsGeneracionTimbradoSVCWS.IwcfRecepcionClient();

                    var requestInterceptor = new InspectorBehavior();
                    facturacion.Endpoint.Behaviors.Add(requestInterceptor);
                    //facturacionAPEL.Endpoint.Behaviors.Add(requestInterceptor);
                    facturacionSVC.Endpoint.Behaviors.Add(requestInterceptor);

                    Stream stream = File.Open(sXMLs.ToString(), FileMode.Open);
                    string sNombreXML = Path.GetFileNameWithoutExtension(sXMLs);
                    RecuperacionSOAP.recuperaNombre(sNombreXML);
                    string sLayout = string.Empty;

                    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                    StringBuilder sw = new StringBuilder();

                    sLayout = sr.ReadToEnd();
                    sr.Close();                    
                    
                    sXML = string.Empty;
                    
                    try
                    {
                        //sXML = facturacion.fnEnviarTXT(sLayout, psTipoDocumento, 0, sUsuario, sPassword, sVersion, sOrigen);

                        wsGeneracionTimbradoWS.ArrayOfAnyType asXML = facturacion.fnEnviarTXTPAX001(sLayout, psTipoDocumento, 0, sUsuario, sPassword, sVersion, sOrigen);
                        sXML = asXML[1].ToString();

                        //sXML = facturacionSVC.fnEnviarTXT(sLayout, psTipoDocumento, 0, sUsuario, sPassword, sVersion, sOrigen);
                    }
                    catch (Exception ex)
                    {
                        clsLog.EscribirLog("Error al timbrar  - " + ex.Message);
                    }


                    try
                    {
                        //Se valida el tipo de respuesta
                        sCad = sXML.Split(cCad);
                        if (sCad.Length <= 2)
                        {
                            //En caso de marcar error se graba un log
                            string path = (String)PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["LogError"] + "LogErrorSinTimbrar" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                            if (!File.Exists(path))
                            {
                                StreamWriter sr4 = new StreamWriter(path);
                                sr4.WriteLine(DateTime.Now + " " + sXML + ", Nombre XML: " + sNombreXML);
                                sr4.Close();
                            }
                            else
                            {
                                System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                                sw4.WriteLine(DateTime.Now + " " + sXML + ", Nombre XML: " + sNombreXML);
                                sw4.Close();
                            }
                            //Guarda XML invalido
                            //xmlfinal.Save(PaxRecuperacionSOAPBateria.Properties.Settings.Default["RutaXMLInv"].ToString() + sNombreXML + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".xml");
                            //Elimina el archivo xml
                            //File.Delete(sXMLs);
                        }
                        else
                        {
                            //Si el xml fue timbrado con exito
                            XmlDocument XMLTimbrado = new XmlDocument();

                            XMLTimbrado.LoadXml(sXML);

                            XmlNamespaceManager nsmComprobanteT = new XmlNamespaceManager(XMLTimbrado.NameTable);
                            nsmComprobanteT.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                            nsmComprobanteT.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                            XPathNavigator navEncabezado = XMLTimbrado.CreateNavigator();
                            try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteT).Value; }
                            catch { snombreDoc = Guid.NewGuid().ToString(); }

                            //Se guarda XML en ruta especificada 
                            //XMLTimbrado.Save(PaxRecuperacionSOAPBateria.Properties.Settings.Default["RutaTimbrado"].ToString() + snombreDoc + ".xml");
                            RecuperacionSOAP.obtenerUUID(XMLTimbrado);

                        }
                    }
                    catch (Exception ex)
                    {
                        clsLog.EscribirLog("Error al intentar el resultado  - " + ex.Message);
                    }


                    try
                    {
                        using (Stream s = System.IO.File.Open(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString() + "trace.log",
                                              FileMode.Open,
                                              FileAccess.Read,
                                              FileShare.ReadWrite))
                        {
                            string sArchivo = StreamToString(s);
                            string[] inicio;
                            string[] fin;

                            inicio = sArchivo.Split('{');
                            fin = inicio[1].Split('}');
                            string encabezado = fin[0];

                            encabezado = encabezado.Trim();
                            RecuperacionSOAP.recuperaRequestHeader(encabezado);

                            s.Close();
                            s.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        clsLog.EscribirLog("Error al intentar leer trace.log - " + ex.Message);
                    }

                    RecuperacionSOAP.escribirSOAP();
                    facturacion.Close();

                    //RecuperacionSOAP.creaXMLRequest();
                    //RecuperacionSOAP.creaXMLResponse();

                    //RecuperacionSOAP.insertarAcuseRequest();
                    //RecuperacionSOAP.insertarAcuseResponse();

                    TraceSource ts = new TraceSource("System.Net");
                    ts.Close();

                    TraceSource ts2 = new TraceSource("System.Net.Sockets");
                    ts2.Listeners[1].Close();
                    ts2.Close();


                    if (File.Exists(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString() + "trace.log"))
                        File.Delete(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString() + "trace.log");
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                try
                {
                    using (Stream s = System.IO.File.Open(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString() + "trace.log",
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.ReadWrite))
                    {
                        string sArchivo = StreamToString(s);
                        string[] inicio;
                        string[] fin;

                        inicio = sArchivo.Split('{');
                        fin = inicio[1].Split('}');
                        string encabezado = fin[0];

                        encabezado = encabezado.Trim();
                        RecuperacionSOAP.recuperaRequestHeader(encabezado);

                        s.Close();
                        s.Dispose();
                    }
                }
                catch (Exception e)
                {
                    clsLog.EscribirLog("Error al intentar leer trace.log - " + e.Message);
                }

                RecuperacionSOAP.escribirSOAP();
                //RecuperacionSOAP.creaXMLRequest();
                //RecuperacionSOAP.creaXMLResponse();

                //RecuperacionSOAP.insertarAcuseRequest();
                //RecuperacionSOAP.insertarAcuseResponse();

                TraceSource ts = new TraceSource("System.Net");
                ts.Close();

                TraceSource ts2 = new TraceSource("System.Net.Sockets");
                ts2.Listeners[1].Close();
                ts2.Close();


                if (File.Exists(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString() + "trace.log"))
                    File.Delete(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString() + "trace.log");

                try
                {
                    DateTime Fecha = DateTime.Today;
                    string path = (String)PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                    if (!File.Exists(path))
                    {
                        StreamWriter sr4 = new StreamWriter(path);
                        sr4.WriteLine(DateTime.Now + " " + ex.Message);
                        sr4.Close();
                    }
                    else
                    {
                        System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                        sw4.WriteLine(DateTime.Now + " " + ex.Message);
                        sw4.Close();
                    }
                }
                catch
                {
                }
            }
            clsRecuperacionSOAP.limpiarVariables();
        }

        public static string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}

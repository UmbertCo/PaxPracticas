using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath;


public class clsTimbrado
{
    public ConectorPAXGenericoPDF.wsRecepcionTASMX.wcfRecepcionASMX wsRecepcionT = new ConectorPAXGenericoPDF.wsRecepcionTASMX.wcfRecepcionASMX();

    public void fnTimbrado()
    {
        try
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsTimbrado.AcceptAllCertificatePolicy);

            string sXML, snombreDoc, sVersion;
            DateTime Fecha = DateTime.Today;
            string psTipoDocumento = ConectorPAXGenericoPDF.Properties.Settings.Default["tipodocto"].ToString();
            char[] cCad = { '-' };
            string[] sCad;
            //Se obtiene lista de txt almacenados en carpeta para timbrar
            string[] files = Directory.GetFiles(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocs"].ToString(), "*.xml");
            //Se recorre los xml contenidos en el directorio
            foreach (string sXMLs in files)
            {

                Stream stream = File.Open(sXMLs.ToString(), FileMode.Open);
                string sNombreXML = Path.GetFileNameWithoutExtension(sXMLs);
                StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                StringBuilder sw = new StringBuilder();

                while (!(sr.EndOfStream))
                {
                    string linea = sr.ReadLine();
                    char[] Arreglo = { };

                    if (linea.Contains("<?xml"))
                    {
                        int pos = linea.IndexOf("<?xml");
                        if (pos > -1)
                        {
                            linea = linea.Substring(pos, linea.Length - pos);
                        }
                    }
                    //  linea = reemplaza(linea);
                    sw.Append(linea);
                }

                XmlDocument xmlfinal = new XmlDocument();
                xmlfinal.XmlResolver = null;
                sr.Close();

                try
                {
                    xmlfinal.LoadXml(sw.ToString());
                }
                catch (Exception ex)
                {

                    try
                    {
                        string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                        if (!File.Exists(path))
                        {
                            StreamWriter sr2 = new StreamWriter(path);
                            sr2.WriteLine(DateTime.Now + " " + "Problemas en la recepción de facturas", "El XML contiene caracteres especiales BOM" + ", nombre del comprobante " + sXMLs);
                            sr2.Close();
                        }
                        else
                        {
                            System.IO.StreamWriter sw2 = new System.IO.StreamWriter(path, true);
                            sw2.WriteLine(DateTime.Now + " " + ex.Message);
                            sw2.Close();
                        }
                        //Guarda XML invalido
                        xmlfinal.Save(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreXML + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".xml");
                        //Elimina el archivo xml
                        File.Delete(sXMLs);
                        return;
                    }
                    catch
                    {
                    }
                }

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlfinal.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                sVersion = xmlfinal.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;

                sXML = wsRecepcionT.fnEnviarXML(xmlfinal.OuterXml, psTipoDocumento, 0, ConectorPAXGenericoPDF.Properties.Settings.Default["usuario"].ToString(),
                       ConectorPAXGenericoPDF.Properties.Settings.Default["password"].ToString(), sVersion);

                //Se valida el tipo de respuesta
                sCad = sXML.Split(cCad);
                if (sCad.Length <= 2)
                {
                    //En caso de marcar error se graba un log
                    string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogErrorSinTimbrar" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

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
                    xmlfinal.Save(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreXML + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".xml");
                    //Elimina el archivo xml
                    File.Delete(sXMLs);
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
                    XMLTimbrado.Save(ConectorPAXGenericoPDF.Properties.Settings.Default["RutaDocZips"].ToString() + snombreDoc + ".xml");

                    try
                    {
                        //Generar PDF
                        string sRutaPDF = ConectorPAXGenericoPDF.Properties.Settings.Default["RutaDocZips"].ToString();

                        fnCrearPLantillaEnvio(XMLTimbrado, psTipoDocumento, sRutaPDF + snombreDoc + ".pdf");
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            DateTime Fechaex = DateTime.Today;
                            string pathex = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";

                            if (!File.Exists(pathex))
                            {
                                StreamWriter sr4 = new StreamWriter(pathex);
                                sr4.WriteLine(DateTime.Now + " " + ex.Message);
                                sr4.Close();
                            }
                            else
                            {
                                System.IO.StreamWriter sw4 = new System.IO.StreamWriter(pathex, true);
                                sw4.WriteLine(DateTime.Now + " " + ex.Message);
                                sw4.Close();
                            }
                        }
                        catch
                        {
                        }
                    }

                    //Se guarda log de comprobantes timbrados
                    string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogTimbrados"] + "LogTimbrados" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                    if (!File.Exists(path))
                    {
                        StreamWriter sr4 = new StreamWriter(path);
                        sr4.WriteLine(DateTime.Now + ", Nombre XML: " + sNombreXML);
                        sr4.Close();
                    }
                    else
                    {
                        System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                        sw4.WriteLine(DateTime.Now + ", Nombre XML: " + sNombreXML);
                        sw4.Close();
                    }                    
                    //Elimina el archivo xml
                    File.Delete(sXMLs);
                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                DateTime Fecha = DateTime.Today;
                string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

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
    }

    /// <summary>
    /// Crea archivo pdf segun plantilla configurada para su posterior envio de correo
    /// </summary>
    /// <param name="pxComprobante"></param>
    /// <param name="psTipoDocumento"></param>
    /// <param name="sRuta"></param>
    public void fnCrearPLantillaEnvio(XmlDocument pxComprobante, string psTipoDocumento, string sRuta)
    {

        if (!(sRuta == string.Empty))
        {
            clsOperacionConsultaPdf pdf = new clsOperacionConsultaPdf(pxComprobante);

            if (!string.IsNullOrEmpty(psTipoDocumento))
                pdf.TipoDocumento = psTipoDocumento.ToUpper();
            pdf.fnGenerarPDFSave(sRuta, "Black");

        }
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}


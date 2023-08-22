using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

namespace ConectorPAXGenericoPDF
{
    class Reimpresion
    {
        public void fnEnviarTXT()
        {
            bool NOaddenda = false;
            string status = string.Empty;
            bool impreso = false;
            string printerName = "";
            string correo = "";
            string[] Files = null;
            string RutaXMLDocs = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["Reimpresion"];
            string filtro = "*.xml";
            Files = Directory.GetFiles(RutaXMLDocs, filtro);
            DateTime Fecha = DateTime.Today;

            foreach (string archivo in Files)
            {
                System.IO.StringReader lectorVersion;
                System.IO.StringReader lector;
                string noCertificado = string.Empty;
                string sNombreTXT = Path.GetFileNameWithoutExtension(archivo);
                string text = string.Empty;
                string tipodocumento = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["tipodocto"];

                using (Stream stream = File.Open(archivo.ToString(), FileMode.Open))
                {
                    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                    text = sr.ReadToEnd();
                    lector = new System.IO.StringReader(text);
                    lectorVersion = new System.IO.StringReader(text);
                }

                

                XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
                nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                XmlDocument XMLTimbrado = new XmlDocument(nsm.NameTable);
                XMLTimbrado.LoadXml(text);

                try
                {

                    XmlNode padre = null;
                    padre = XMLTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/impresora", nsm);
                    printerName = padre.InnerText;

                    padre = XMLTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/correo", nsm);
                    correo = padre.InnerText;
                }
                catch (Exception ex)
                {
                    string pathI = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogErrorSinImprimir" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                    clsLog.Escribir(pathI, DateTime.Now + " " + "Este documento no contiene addenda");
                    NOaddenda = true;
                }

                string snombreDoc = string.Empty;
                XmlNamespaceManager nsmComprobanteT = new XmlNamespaceManager(XMLTimbrado.NameTable);
                nsmComprobanteT.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobanteT.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                XPathNavigator navEncabezado = XMLTimbrado.CreateNavigator();
                try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteT).Value; }
                catch { snombreDoc = Guid.NewGuid().ToString(); }

                string sRutaXML = ConectorPAXGenericoPDF.Properties.Settings.Default["RutaDocZips"].ToString() + snombreDoc + ".xml";
                XMLTimbrado.Save(sRutaXML);
                //Ruta PDF
                string sRutaPDF = ConectorPAXGenericoPDF.Properties.Settings.Default["rutaPDF"].ToString() + snombreDoc + ".pdf";
                try
                {
                    //Generar PDF
                    clsPlantillaLista PlantillaLista = new clsPlantillaLista();

                    PlantillaLista.fnCrearPLantillaEnvio(XMLTimbrado, tipodocumento, sRutaPDF);

                    PrinterSettings printer = new PrinterSettings();
                    printer.PrinterName = printerName;

                    if (printer.IsValid)
                    {
                        ExecuteCommandBAT(sRutaPDF);
                        string Filename = sRutaPDF + ".ps";
                        if (File.Exists(Filename))
                        {
                            //Print the file to the printer.
                            RawPrinterHelper.SendFileToPrinter(printer.PrinterName, Filename);
                            string nombreArchivo = Path.GetFileName(Filename);

                            impreso = RawPrinterHelper.GetJobs(printerName, nombreArchivo);
                            clsOperaciones.fnActualizarImpresion(snombreDoc, impreso);
                            File.Delete(Filename);
                        }
                    }
                    else
                    {
                        if (printerName != "" && NOaddenda == false)
                        {
                            string pathI = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogErrorSinImprimir" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                            clsLog.Escribir(pathI, DateTime.Now + " " + ", Nombre PDF: " + sNombreTXT + " asegurese de que la impresora " + printerName + " este conectada");
                        }

                    }

                    /*clsEnvioCorreoDocs EnvioCorreoDocs = new clsEnvioCorreoDocs();
                    string sMensaje = string.Empty;
                    sMensaje = "<table>";
                    sMensaje = sMensaje + "<tr><td>Estimado Cliente. <br /><br />Se adjunta por medio de este correo electrónico su Comprobante Fiscal Digital por Internet.<br />"+ status +"</td></tr>";
                    sMensaje = sMensaje + "</table>";
                    string sMailTo = correo;
                    bool sEnvio = false;
                    //////////////////////////////////////////////////////////////////////////////
                    //Envia documento
                    sEnvio = EnvioCorreoDocs.fnPdfEnvioCorreoSinZIP(XMLTimbrado, sRutaPDF, sMailTo, "Comprobantes", sMensaje, sRutaXML, sNombreTXT, "");
                    //Se verifica si el correo fue enviado
                    if (sEnvio == false)
                    {
                        try
                        {
                            if (!File.Exists((String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt"))
                            {
                                StreamWriter sr4 = new StreamWriter((String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt");
                                sr4.WriteLine(DateTime.Now + " No se puede enviar el documento - " + sNombreTXT + " - al correo - " + sMailTo);
                                sr4.Close();
                            }
                            else
                            {
                                System.IO.StreamWriter sw4 = new System.IO.StreamWriter((String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                                sw4.WriteLine(DateTime.Now + " No se puede enviar el documento - " + sNombreTXT + " - al correo - " + sMailTo);
                                sw4.Close();
                            }
                        }
                        catch
                        {
                        }
                    }*/
                    File.Delete(archivo);
                }
                catch (Exception ex)
                {
                    try
                    {
                        DateTime Fechaex = DateTime.Today;
                        string pathex = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";
                        clsLog.Escribir(pathex, DateTime.Now + " " + ex.Message);
                    }
                    catch
                    {
                    }
                }
            }

            
        }

        private void ExecuteCommandBAT(string file)
        {
            string outputO = "";
            string errorO = "";
            string ExitCode = "";

            try
            {
                int exitCode;
                ProcessStartInfo processInfo;
                Process process;

                processInfo = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "lib\\pdf2ps.bat");
                processInfo.Verb = "runas";
                processInfo.Arguments = file + " " + file + ".ps";
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                // *** Redirect the output ***
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;

                process = Process.Start(processInfo);
                process.WaitForExit();

                // *** Read the streams ***
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                exitCode = process.ExitCode;

                outputO = "output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output);
                errorO = "error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error);
                ExitCode = "ExitCode: " + exitCode.ToString();

                process.Close();


            }
            catch (Exception ex)
            {
                DateTime Fecha = DateTime.Today;
                string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                if (!File.Exists(path))
                {
                    StreamWriter sr4 = new StreamWriter(path);
                    sr4.WriteLine(DateTime.Now + " " + ex.Message);
                    sr4.WriteLine(DateTime.Now + " " + outputO);
                    sr4.WriteLine(DateTime.Now + " " + errorO);
                    sr4.WriteLine(DateTime.Now + " " + ExitCode);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                    sw4.WriteLine(DateTime.Now + " " + ex.Message);
                    sw4.WriteLine(DateTime.Now + " " + outputO);
                    sw4.WriteLine(DateTime.Now + " " + errorO);
                    sw4.WriteLine(DateTime.Now + " " + ExitCode);
                    sw4.Close();
                }
            }
        }



    }
}

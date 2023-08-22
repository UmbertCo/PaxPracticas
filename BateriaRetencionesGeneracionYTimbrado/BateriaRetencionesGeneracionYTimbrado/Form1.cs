using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace BateriaRetencionesGeneracionYTimbrado
{
    public partial class Form1 : Form
    {
        ////public static string rutaDocs = "C:\\ConectorRetenciones\\XML\\Docs\\";
        //validación
        public static string rutaDocs = @"C:\PAXRegeneracionRetenciones\Archivos Generados\";
        
        //public static string rutaDocs = System.AppDomain.CurrentDomain.BaseDirectory + "\\XML\\Docs\\";
        //public static string rutaXML = System.AppDomain.CurrentDomain.BaseDirectory + "\\XML\\Generado\\";
        //public static string rutaXML = "C:\\ConectorRetenciones\\XML\\Generado\\";

        //validación
        public static string rutaXML=@"C:\PAXRegeneracionRetenciones\Archivos validados\";
       // public static string rutaXML = System.IO.Directory.GetCurrentDirectory();

        //validación
        public static string rutaErrores = @"C:\PAXRegeneracionRetenciones\Errores\";
        //public static string rutaErrores =  "C:\\ConectorRetenciones\\XML\\Errores\\";
        //public static string rutaErrores = System.AppDomain.CurrentDomain.BaseDirectory + "\\XML\\Errores\\";

        //validación
        public static string rutaDocInv = @"C:\PAXRegeneracionRetenciones\DocumentosInvalidos\";
        //public static string rutaDocInv ="C:\\ConectorRetenciones\\XML\\DocumentosInvalidos\\";
        //public static string rutaDocInv = System.AppDomain.CurrentDomain.BaseDirectory + "\\XML\\DocumentosInvalidos\\";

        wcfRecepcionasmx.wcfRecepcionASMXSoapClient WCF = new wcfRecepcionasmx.wcfRecepcionASMXSoapClient();
        //ws valida
        wfcValidacionasmx.wcfValidaASMXSoapClient WFCValida = new wfcValidacionasmx.wcfValidaASMXSoapClient();



        FilesManager FM = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //FM = new FilesManager();
            
            if (Temp1.Enabled == true)
            {
                Temp1.Enabled = false;
            }
            else
            {
                Temp1.Enabled = true;
            }
        }

        private void Temp1_Tick(object sender, EventArgs e)
        {
            Temp1.Stop();

            DirectoryInfo d = new DirectoryInfo(rutaDocs);//Assuming Test is your Folder
            //FileInfo[] Files = d.GetFiles("*.txt");
            FileInfo[] Files = d.GetFiles("*.xml");
            
            if (Files != null)
            {
                string sTXT = "";
                string sXML = "";

                foreach (FileInfo file in Files)
                {
                    try
                    {
                        using (FileStream fsSource = new FileStream(file.FullName, FileMode.Open))
                        {
                            string sNombreTXT = System.IO.Path.GetFileNameWithoutExtension(file.Name);
                            StreamReader srTXT = new StreamReader(fsSource);
                            sTXT = srTXT.ReadToEnd();
                            srTXT.Close();

                            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(delegate(object senderE, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                            {
                                return true;
                            });

                            //string Response = WCF.fnEnviarTXT(sTXT, 25, 0, "ws_retenciones", "wrfCv8SVxITEscO8w43DhsSCxLBZcsKbwr/Em8OtwoJNwqTDo++9uO+/oO+/pREB77+Z776J772l", "1.0");

                            string Response = WFCValida.fnValidaXML(sXML, "ws_retenciones", "wrfCv8SVxITEscO8w43DhsSCxLBZcsKbwr/Em8OtwoJNwqTDo++9uO+/oO+/pREB77+Z776J772l", "1.0");

                            char[] cCad = { '-' };
                            string[] sCad;
                            sCad = Response.Split(cCad);

                            int n;
                            bool isNumeric = int.TryParse(sCad[0], out n);

                            if (sCad.Length <= 5 || isNumeric == true )
                            {
                                File.Delete(file.FullName);
                                throw new Exception("Invalido " + Response);
                            }
                            else
                            {
                                XmlDocument xXML = new XmlDocument();
                                xXML.LoadXml(Response);
                                /* XmlNamespaceManager nsmRetenciones = new XmlNamespaceManager(xXML.NameTable);
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
                                 nsmRetenciones.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");*/
                                xXML.Save(rutaXML + file.Name+".xml");

                                File.Delete(file.FullName);
                            }
                        }


                        Temp1.Start();
                    }
                    catch (Exception ex)
                    {
                        //Temp1.Enabled = false;
                        if (!ex.Message.Contains("Invalido"))
                        {
                            Temp1.Enabled = false;
                        }
                        else if (ex.Message.Contains("Invalido"))
                        {
                            //string path = rutaDocInv +"\\"+ file.Name;
                            string path = rutaDocInv + file.Name;
                            if (!File.Exists(path))
                            {
                                File.Create(path).Dispose();
                                StreamWriter tw = new StreamWriter(path);
                                tw.Write(sTXT);
                                tw.Close();
                                tw.Dispose();
                            }
                            else if (File.Exists(path))
                            {
                                StreamWriter tw = new StreamWriter(path, true);
                                tw.Write(sTXT);
                                tw.Close();
                                tw.Dispose();
                            }
                        }
                        Log("Archivo: "+ file.Name +" Hubo un error " + ex.Message);
                        
                    }
                }
            }
            else
            {
                Temp1.Enabled = false;
                MessageBox.Show("No hay archivos");
                Log("No hay archivos");
            }

           
        }



        public static void Log(string e)
        {
            string path = rutaErrores + @"\LOG.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine("" + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
            else if (File.Exists(path))
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("M" + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
        }

        


    }
}

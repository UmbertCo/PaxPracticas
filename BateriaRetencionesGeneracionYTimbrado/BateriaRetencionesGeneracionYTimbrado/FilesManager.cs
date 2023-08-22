using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Windows.Forms;

namespace BateriaRetencionesGeneracionYTimbrado
{
    class FilesManager
    {
        
        public static string rutaDocs = System.AppDomain.CurrentDomain.BaseDirectory + "//XML//Docs";
        public static string rutaXML = System.AppDomain.CurrentDomain.BaseDirectory + "//XML//Generado";
        public static string rutaErrores = System.AppDomain.CurrentDomain.BaseDirectory + "//XML//Errores";
        static System.Timers.Timer aTimer = new System.Timers.Timer();

        public FilesManager()
        {
           
            aTimer.Start();
            aTimer.Interval = 10000;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
                DirectoryInfo d = new DirectoryInfo(rutaDocs);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.txt");
                if (Files != null)
                {
                    try
                    {

                        foreach (FileInfo file in Files)
                        {
                            string sTXT;

                            using (FileStream fsSource = new FileStream(file.FullName, FileMode.Open))
                            {
                                string sNombreTXT = System.IO.Path.GetFileNameWithoutExtension(file.Name);
                                StreamReader srTXT = new StreamReader(fsSource);
                                sTXT = srTXT.ReadToEnd();
                                srTXT.Close();

                                //sXML = wsRecepcionT.fnEnviarXML(xDocumento.OuterXml, TipoComprobante, 0, Settings.Default.usuario, Settings.Default.password, "1.0");
                                  string response = "";
                                  if (response == "")
                                  {
                                      aTimer.Enabled = false;
                                      throw new Exception("Test Exception");
                                  }

                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        aTimer.Enabled = false;
                        Log("Error " + ex);
                        
                    }
                }
                else
                {

                }

            
        }

        public static void Log(string e)
        {
            string path = rutaErrores + "\\LOG.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine("Error " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
            else if (File.Exists(path))
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("MError " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
        }
    }
}

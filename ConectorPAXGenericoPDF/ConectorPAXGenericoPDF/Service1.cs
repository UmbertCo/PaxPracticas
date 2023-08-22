using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using System.IO;
using System.Configuration;
using ConectorPAXGenericoPDF.Properties;
using System.Reflection;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;


namespace ConectorPAXGenericoPDF
{
    public partial class Service1 : ServiceBase
    {
        //private System.Threading.Timer oTimer;
        private System.Timers.Timer Timer;
        public Service1()
        {
            InitializeComponent();
            
            Timer = new System.Timers.Timer();

        }

        protected override void OnStart(string[] args)
        {
            /*Debugger.Launch();
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();*/

            //Crea recursos de Ghostscript
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\lib";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!Directory.EnumerateFiles(path).Any())
            {
                fnCrearGhostscript();
            }
            if (Directory.GetFiles(path).Length < 252)
            {
                fnCrearGhostscript();
            }

            //Timer para el control del tiempo entre llamadas. Milisegundos donde 1 segundo son 1000 ms.
            Timer = new System.Timers.Timer();
            //Timer.Interval = 5000;
            Timer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
            Timer.Enabled = true;

            //TimerCallback oCallback = new TimerCallback(OnTimedEvent);
            //oTimer = new System.Threading.Timer(oCallback, null, Convert.ToInt32(Settings.Default.intervalo), Convert.ToInt32(Settings.Default.intervalo));
        }

        void myTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int timeToGo;
            //Revisar el tiempo para iniciar el proceso
            timeToGo = Convert.ToInt32(Settings.Default.intervalo);
            Timer.Interval = timeToGo;

            
            fnIniciarTimbrado();

        }

        protected override void OnStop()
        {
            //oTimer.Dispose();
            Timer.Enabled = false;
        }

        //private bool TimerRunning = false;

        public void fnCrearGhostscript()
        {
            /****GENERAR CARPETA DE ESQUEMAS***/

            System.IO.Stream stream = new MemoryStream(Resources.lib);

            ZipFile zip = new ZipFile(stream);

            //Assembly.GetExecutingAssembly().Location

            string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            AppPath = AppPath.Replace("file:\\", "");

            string path = AppPath;

            foreach (ZipEntry zipEntry in zip)
            {
                if (zipEntry.IsFile)
                {
                    /*using (var fs = new FileStream(path + "\\" + zipEntry.Name, FileMode.Create, FileAccess.Write))
                        zip.GetInputStream(zipEntry).CopyTo(fs);*/

                    Stream zipStream = zip.GetInputStream(zipEntry);
                    byte[] buffer = new byte[4096];

                    using (FileStream streamWriter = File.Create(path + "\\" + zipEntry.Name))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
        }

        private void fnIniciarTimbrado() //private void OnTimedEvent(object state)
        {
            //if (TimerRunning)
            //{
            //    return;
            //}
            
            //TimerRunning = true;
            try
            {
                Timer.Enabled = false;
                //Verifica si existen carpetas en el cual se almacena los archivos correspondientes
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["RutaDocZips"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["RutaDocZips"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["LogTimbrados"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["LogTimbrados"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocs"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocs"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaTXTGen"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaTXTGen"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaCertificados"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaCertificados"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["imagenes"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["imagenes"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocCancelados"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocCancelados"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["LogCancelacion"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["LogCancelacion"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["Reimpresion"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["Reimpresion"].ToString());
                if (!(Directory.Exists(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaPDF"].ToString())))
                    Directory.CreateDirectory(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaPDF"].ToString());
                
                //Crea recursos de Imagenes
                string ruta = ConectorPAXGenericoPDF.Properties.Settings.Default["imagenes"].ToString();
                string fichero = ruta + "logo_pax.png";
                if (!File.Exists(fichero))
                {
                    Properties.Resources.logo_pax.Save(fichero);

                }

                clsOperacionTimbrado OperacionTimbrado = new clsOperacionTimbrado();
                OperacionTimbrado.fnGeneracionTimbrado(ConectorPAXGenericoPDF.Properties.Settings.Default["TipoServicio"].ToString());
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
            finally
            {
                //TimerRunning = false;
                Timer.Enabled = true;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using wsGeneraXml.Properties;

namespace wsGeneraXml
{
    public partial class GeneraXml : ServiceBase
    {
        

        public GeneraXml()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();
            //Debugger.Launch();
            //Timer para el control del tiempo entre llamadas. Milisegundos donde 1 segundo son 1000 ms.
            if (!(Directory.Exists(Properties.Settings.Default.rutaDocs)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaDocs);
            if (!(Directory.Exists(Properties.Settings.Default.LogError)))
                Directory.CreateDirectory(Properties.Settings.Default.LogError);
            if (!(Directory.Exists(Properties.Settings.Default.LogTimbrados)))
                Directory.CreateDirectory(Properties.Settings.Default.LogTimbrados);
            if (!(Directory.Exists(Properties.Settings.Default.rutaDocs)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaDocs);
            if (!(Directory.Exists(Properties.Settings.Default.rutaTXTGen)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaTXTGen);
            if (!(Directory.Exists(Properties.Settings.Default.rutaDocInv)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaDocInv);
            if (!(Directory.Exists(Properties.Settings.Default.rutaCertificados)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaCertificados);
            if (!(Directory.Exists(Properties.Settings.Default.rutaDesconexion)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaDesconexion);

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Settings.Default.rutaDocs;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.txt*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }
        protected override void OnStop()
        {
            //Timer.Enabled = false;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            fnIniciarTimbrado();
        }

        void myTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //int timeToGo;
            //Revisar el tiempo para iniciar el proceso
            //timeToGo = Convert.ToInt32(Settings.Default.intervalo);
            //Timer.Interval = timeToGo;

            fnIniciarTimbrado();
        }

        /// <summary>
        /// Función que revisa si las direcciones donde se van almacenar los archivos y los logs se encuentran creados
        /// </summary>
        private void fnIniciarTimbrado()
        {
            clsGenerar cGenerar = new clsGenerar();
            DateTime Fecha = DateTime.Today;
            try
            {
                //Timer.Enabled = false;
                //Verifica si existen carpetas en el cual se almacena los archivos correspondientes
                //if (!(Directory.Exists(Properties.Settings.Default.RutaDocZips)))
                //    Directory.CreateDirectory(Properties.Settings.Default.RutaDocZips);
                //if (!(Directory.Exists(Properties.Settings.Default.LogError)))
                //    Directory.CreateDirectory(Properties.Settings.Default.LogError);
                //if (!(Directory.Exists(Properties.Settings.Default.LogTimbrados)))
                //    Directory.CreateDirectory(Properties.Settings.Default.LogTimbrados);
                //if (!(Directory.Exists(Properties.Settings.Default.rutaDocs)))
                //    Directory.CreateDirectory(Properties.Settings.Default.rutaDocs);
                //if (!(Directory.Exists(Properties.Settings.Default.rutaTXTGen)))
                //    Directory.CreateDirectory(Properties.Settings.Default.rutaTXTGen);
                //if (!(Directory.Exists(Properties.Settings.Default.rutaDocInv)))
                //    Directory.CreateDirectory(Properties.Settings.Default.rutaDocInv);
                //if (!(Directory.Exists(Properties.Settings.Default.rutaCertificados)))
                //    Directory.CreateDirectory(Properties.Settings.Default.rutaCertificados);
                //if (!(Directory.Exists(Properties.Settings.Default.rutaDesconexion)))
                //    Directory.CreateDirectory(Properties.Settings.Default.rutaDesconexion);
                //if (!(Directory.Exists(Properties.Settings.Default.rutaEsquemaFord)))
                //    Directory.CreateDirectory(Properties.Settings.Default.rutaEsquemaFord);

                cGenerar.fnComenzarGeneracion(Settings.Default.TipoServicio);

            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + ex.Message);
            }
            finally
            {
                //Timer.Enabled = true;
            }
        }
    }
}

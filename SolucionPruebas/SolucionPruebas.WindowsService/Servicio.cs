using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SolucionPruebas.WindowsService
{
    public partial class Servicio : ServiceBase
    {
        private DateTime Fecha = DateTime.Today;

        public Servicio()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();

            fnIniciarTimbrado();

            MyFileSystemWatcher.Path = Properties.Settings.Default.WatchPath;
            MyFileSystemWatcher.IncludeSubdirectories = false;
            MyFileSystemWatcher.Filter = ".txt";
            MyFileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            MyFileSystemWatcher.EnableRaisingEvents = true;
            MyFileSystemWatcher.Created += new FileSystemEventHandler(Watcher_Created);
        }

        protected override void OnStop()
        {
        }

        public void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                System.IO.File.Copy(e.FullPath, Properties.Settings.Default.LocationPath + e.Name);
            }
            catch (Exception ex)
            {
                Log.Instance.LogPath = (String)Properties.Settings.Default["LogError"];
                Log.Instance.LogFileName = "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                Log.WriteLine(DateTime.Now + " " + ex.Message);
            }
        }

        /// <summary>
        /// Función que revisa si las direcciones donde se van almacenar los archivos y los logs se encuentran creados
        /// </summary>
        private void fnIniciarTimbrado()
        {
            try
            {
                //Verifica si existen carpetas en el cual se almacena los archivos correspondientes
                if (!(Directory.Exists(Properties.Settings.Default.WatchPath)))
                    Directory.CreateDirectory(Properties.Settings.Default.WatchPath);
                if (!(Directory.Exists(Properties.Settings.Default.LocationPath)))
                    Directory.CreateDirectory(Properties.Settings.Default.LocationPath);
                if (!(Directory.Exists(Properties.Settings.Default.LogError)))
                    Directory.CreateDirectory(Properties.Settings.Default.LogError);
            }
            catch (Exception ex)
            {
                Log.Instance.LogPath = (String)Properties.Settings.Default["LogError"];
                Log.Instance.LogFileName = "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                Log.WriteLine(DateTime.Now + " " + ex.Message);
                //clsLog.EscribirError((String)Settings.Default["LogError"] + "LogError", DateTime.Now + " " + ex.Message);
            }
            finally
            {

            }
        }

        private void MyFileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                System.IO.File.Copy(e.FullPath, Properties.Settings.Default.LocationPath + e.Name);
            }
            catch (Exception ex)
            {
                Log.Instance.LogPath = (String)Properties.Settings.Default["LogError"];
                Log.Instance.LogFileName = "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                Log.WriteLine(DateTime.Now + " " + ex.Message);
            }
        }
    }
}

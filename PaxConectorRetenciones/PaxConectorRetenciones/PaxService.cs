using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using PaxConectorRetenciones.Properties;
using System.Reflection;

namespace PaxConectorRetenciones
{
    public partial class PaxService : ServiceBase
    {

        protected FileSystemWatcher Watcher;
        public PaxService()
        {
            InitializeComponent();
           
            
        }

        protected override void OnStart(string[] args)
        {
            //Debugger.Launch();
            fnIniciarTimbrado();
            Watcher = new MyFileSystemWatcher(Settings.Default.rutaDocs);
            /*Debugger.Launch();
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();*/
           // Watcher = new MyFileSystemWatcher(Settings.Default.rutaDocs);
        }

        protected override void OnStop()
        {
        }

        private void fnIniciarTimbrado()
        {
            try
            {
                //Verifica si existen carpetas en el cual se almacena los archivos correspondientes
                if (!(Directory.Exists(Properties.Settings.Default.RutaDocZips)))
                    Directory.CreateDirectory(Properties.Settings.Default.RutaDocZips);
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
                if (!(Directory.Exists(Properties.Settings.Default.imagenes)))
                    Directory.CreateDirectory(Properties.Settings.Default.imagenes);
                if (!File.Exists(Properties.Settings.Default.imagenes + "\\logo_pax.png"))
                {
                    Properties.Resources.logo_pax.Save(Properties.Settings.Default.imagenes + "\\logo_pax.png");

                }
                //if (!(Directory.Exists(Resources.origenEsquemas)))
                //    Directory.CreateDirectory(Resources.origenEsquemas);
            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message);
            }
            finally
            {

            }
        }
    }
}

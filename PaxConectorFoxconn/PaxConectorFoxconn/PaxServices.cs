using PaxConectorFoxconn.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace PaxConectorFoxconn
{
    public partial class PaxServices : ServiceBase
    {
        private DateTime Fecha = DateTime.Today;
        protected FileSystemWatcher Watcher;

        public PaxServices()
        {

            InitializeComponent();

            fnIniciarTimbrado();
            Watcher = new MyFileSystemWatcher(Settings.Default.RutaDocs);
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
        /// <summary>
        /// Función que revisa si las direcciones donde se van almacenar los archivos y los logs se encuentran creados
        /// </summary>
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
                if (!(Directory.Exists(Properties.Settings.Default.RutaDocs)))
                    Directory.CreateDirectory(Properties.Settings.Default.RutaDocs);
                if (!(Directory.Exists(Properties.Settings.Default.RutaTXTGen)))
                    Directory.CreateDirectory(Properties.Settings.Default.RutaTXTGen);
                if (!(Directory.Exists(Properties.Settings.Default.RutaDocInv)))
                    Directory.CreateDirectory(Properties.Settings.Default.RutaDocInv);
                if (!(Directory.Exists(Properties.Settings.Default.RutaCertificados)))
                    Directory.CreateDirectory(Properties.Settings.Default.RutaCertificados);
                if (!(Directory.Exists(Resources.origenEsquemas)))
                    Directory.CreateDirectory(Resources.origenEsquemas);
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

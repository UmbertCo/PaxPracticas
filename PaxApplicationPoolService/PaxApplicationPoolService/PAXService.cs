using System;
using PaxApplicationPoolService.Properties;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using System.Reflection;

namespace PaxApplicationPoolService
{
    public partial class PAXService : ServiceBase
    {
        private System.Timers.Timer Timer = null;

        public PAXService()
        {
            InitializeComponent();
            Timer = new System.Timers.Timer();
        }

        public void Start()
        {
            OnStart(null);
        }

        void myTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int timeToGo;
            //Revisar el tiempo para iniciar el proceso
            timeToGo = Convert.ToInt32(Settings.Default.Intervalo);
            Timer.Interval = timeToGo;
            fnIniciarChequeo();
        }

        protected override void OnStart(string[] args)
        {
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();

            //tomamos el directorio donde está siendo ejecutado el servicio
            String dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, "ListaAppPool.xml");

            /**********************CREANDO LISTA DE APPLICATION POOL POR REVISAR******************************/
            //esta lista se carga a partir de un archivo xml ubicado en Resource1.resx
            //para agregar elementos a esta lista se debe modificar el archivo xml

            XmlDocument xmlAppPool = new XmlDocument();
            try
            {
                xmlAppPool.Load(path);
            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Error al cargar XML: " + ex);
            }

            XmlNodeList xApplications_pools = xmlAppPool.GetElementsByTagName("applications_pools");
            XmlNodeList xApp_pool = ((XmlElement)xApplications_pools[0]).GetElementsByTagName("app_pool");

            foreach (XmlElement app_pool in xApp_pool)
            {
                clsLog.EscribirLog("Elemento agregado a la lista: " + app_pool.InnerText);
            }
            /*************************FIN CREACIÓN LISTA APPLICATION POOL******************************/

            Timer = new System.Timers.Timer();
            Timer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
            Timer.Enabled = true;
        }

        protected override void OnStop()
        {
            Timer.Enabled = false;
        }

        public void fnIniciarChequeo()
        {
            Timer.Enabled = false;
            try
            {
                if (!(Directory.Exists(Settings.Default.LogError)))
                    Directory.CreateDirectory(Settings.Default.LogError);
                ReiniciarAppPool.ChecarEstatusIIS();
            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Error al iniciar servicio método ChecarEstatusIIS: "+ex);
            }
            finally
            {
                Timer.Enabled = true;
            }
        }
    }
}

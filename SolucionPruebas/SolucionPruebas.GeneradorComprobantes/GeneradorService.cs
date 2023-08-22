using SolucionPruebas.GeneradorComprobantes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SolucionPruebas.GeneradorComprobantes
{
    public partial class GeneradorService : ServiceBase
    {
        public GeneradorService()
        {
            InitializeComponent();
        }

        private System.Timers.Timer Timer;
        clsGenerarComprobantes cGenerarComprobantes = new clsGenerarComprobantes();

        protected override void OnStart(string[] args)
        {
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();

            //Timer para el control del tiempo entre llamadas. Milisegundos donde 1 segundo son 1000 ms.
            Timer = new System.Timers.Timer();
            //Timer.Interval = 5000;
            Timer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
            Timer.Enabled = true;
        }

        protected override void OnStop()
        {
            Timer.Enabled = false;
        }

        void myTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int timeToGo;
            //Revisar el tiempo para iniciar el proceso
            timeToGo = Convert.ToInt32(Settings.Default.Intervalo);
            Timer.Interval = timeToGo;

            fnGeneracion();
        }

        /// <summary>
        /// Función que revisa si las direcciones donde se van almacenar los archivos y los logs se encuentran creados
        /// </summary>
        private void fnGeneracion()
        {
            DateTime Fecha = DateTime.Today;
            try
            {
                Timer.Enabled = false;
                //Verifica si existen carpetas en el cual se almacena los archivos correspondientes

                cGenerarComprobantes.fnGenerar();

            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message);
            }
            finally
            {
                Timer.Enabled = true;
            }
        }
    }
}

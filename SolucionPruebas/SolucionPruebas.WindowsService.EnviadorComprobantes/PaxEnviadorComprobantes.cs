using SolucionPruebas.WindowsService.EnviadorComprobantes.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SolucionPruebas.WindowsService.EnviadorComprobantes
{
    public partial class PaxEnviadorComprobantes : ServiceBase
    {
        private System.Timers.Timer MyTimer;

        public PaxEnviadorComprobantes()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            //if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();

            //Timer para el control del tiempo entre llamadas. Milisegundos donde 1 segundo son 1000 ms.
            MyTimer = new System.Timers.Timer();
            //Timer.Interval = 5000;
            MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
            MyTimer.Enabled = true;
        }
        protected override void OnStop()
        {
            MyTimer.Enabled = false;
            clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    "Servicio Detenido " + DateTime.Now);
        }
        void myTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int timeToGo;
            //Revisar el tiempo para iniciar el proceso
            timeToGo = Convert.ToInt32(Settings.Default.Intervalo);
            MyTimer.Interval = timeToGo;

            fnEnviarComprobantes();
        }

        private void fnEnviarComprobantes()
        {
            DateTime Fecha = DateTime.Today;
            try
            {
                MyTimer.Enabled = false;

                clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    "Envios iniciada " + DateTime.Now);

                clsLog.fnEscribir(Settings.Default.LogError + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, DateTime.Now + " " + "Iniciar ");

                clsEnviarComprobantes cEnviarComprobantes = new clsEnviarComprobantes();
                cEnviarComprobantes.fnEnviarComprobantes();

                clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    "Envios finalizada " + DateTime.Now);
            }
            catch (Exception ex)
            {
                clsLog.fnEscribir(Settings.Default.LogError + "LogError" + DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year, DateTime.Now + " " + ex.Message);
            }
            finally
            {
                MyTimer.Enabled = true;
            }
        }
    }
}

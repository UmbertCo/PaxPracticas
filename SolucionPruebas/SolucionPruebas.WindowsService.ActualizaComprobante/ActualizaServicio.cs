using SolucionPruebas.WindowsService.ActualizaComprobante.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace SolucionPruebas.WindowsService.ActualizaComprobante
{
    public partial class ActualizaServicio : ServiceBase
    {
        private System.Threading.Timer Timer;
        private System.Timers.Timer MyTimer;

        public ActualizaServicio()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();

            //SetTimerValue();

            //Timer para el control del tiempo entre llamadas. Milisegundos donde 1 segundo son 1000 ms.
            MyTimer = new System.Timers.Timer();
            //Timer.Interval = 5000;
            MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
            MyTimer.Enabled = true;
        }
        protected override void OnStop()
        {
            //Timer.Dispose();
            MyTimer.Enabled = false;
            clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    "Servicio Detenido " + DateTime.Now);

           
        }

        private void SetTimerValue()
        {

            DateTime requiredTime = DateTime.Today.AddHours(Convert.ToInt32(Settings.Default.Hora)).AddMinutes(Convert.ToInt32(Settings.Default.Minutos));
            clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    "Iniciando " + DateTime.Now);

            if (DateTime.Now > requiredTime)
            {
                requiredTime = requiredTime.AddDays(1);
            }

            Timer = new Timer(new TimerCallback(TimerAction));
            Timer.Change((int)(requiredTime - DateTime.Now).TotalMilliseconds, Timeout.Infinite);
        }

        private void TimerAction(object e)
        {
            try
            {
                clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    "Actualización iniciada " + DateTime.Now);

                clsActualizacionComprobantes.fnActualizarComprobantes();

                clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    "Actualización finalizada " + DateTime.Now);
            }
            catch (Exception ex)
            {
                clsLog.fnEscribir(Settings.Default.LogError + "LogError" + DateTime.Today.Day + "-" + DateTime.Today.Month + "-" + DateTime.Today.Year, DateTime.Now + " " + ex.Message);
            }
            SetTimerValue();
        }

        void myTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int timeToGo;
            //Revisar el tiempo para iniciar el proceso
            timeToGo = Convert.ToInt32(Settings.Default.Intervalo);
            MyTimer.Interval = timeToGo;

            fnIniciarActualizacion();
        }

        /// <summary>
        /// Función que revisa si las direcciones donde se van almacenar los archivos y los logs se encuentran creados
        /// </summary>
        private void fnIniciarActualizacion()
        {
            DateTime Fecha = DateTime.Today;
            try
            {
                MyTimer.Enabled = false;

                clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    "Actualización iniciada " + DateTime.Now);

                clsActualizacionComprobantes.fnActualizarComprobantes();

                clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    "Actualización finalizada " + DateTime.Now);
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

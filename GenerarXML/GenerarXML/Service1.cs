using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace GenerarXML
{
    public partial class GenerarXML : ServiceBase
    {
        private Timer timer1 = null;

        public GenerarXML()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = 10 * 1000; //cada 10 segundos
            this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
            LeerEscribirFicherosINI.WriteErrorLog("Servicio Windows de Prueba iniciado");
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            LeerEscribirFicherosINI.LeerEsciribirFichero();
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
            LeerEscribirFicherosINI.WriteErrorLog("Servicio Windows de Prueba se ha detenido");
        }
    }
}

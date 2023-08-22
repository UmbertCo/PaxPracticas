using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace wsPracticaTeamFoundation
{
	partial class Service1: ServiceBase
	{
        private Timer tmrContador;
        private ClsEntrada cEntrada = new ClsEntrada();
		public Service1()
		{
			InitializeComponent();
            tmrContador = new Timer();
		}
        void EEHElapsed(object sender, ElapsedEventArgs e)
        {
            cEntrada.buscarEntrada();
        }

		protected override void OnStart(string[] args)
		{
            if (args.GetLength(0) > 0 && args[0].Equals("DEBUG"))
                System.Diagnostics.Debugger.Launch();
            tmrContador.Interval = 15000;
            tmrContador.Elapsed += new ElapsedEventHandler(EEHElapsed);
            tmrContador.Enabled = true;
		}

		protected override void OnStop()
		{
            tmrContador.Enabled = false;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace SolucionPruebas.WindowsService.ActualizaComprobante
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new ActualizaServicio() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using ConectorPDFSharp;

namespace ConectorPDFSharp
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
				new PAXService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}

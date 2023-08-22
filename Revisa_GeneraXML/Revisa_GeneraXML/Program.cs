using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;

namespace Revisa_GeneraXML
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
				new Revisa_Genera() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}

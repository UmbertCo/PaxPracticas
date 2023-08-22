using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace GeneradorPruebasSantander
{
    class Program
    {
        public static void fnGeneracionPruebas(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { @"" };

                args[0] = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                clsGeneracionPaquetes.fnGenerarPrueba(args[0]);

            }
            else if (args.Length == 7)
            {


                clsGeneracionPaquetes.fnGenerarPrueba(args[0], args[1]
                                                     , Convert.ToInt32(args[2]), Convert.ToInt32(args[3])
                                                     , Convert.ToInt32(args[4]), Convert.ToBoolean(args[5])
                                                     , Convert.ToDouble(args[6]));


            }

            else if (args.Length == 8)
            {
                bool bDebug = Convert.ToBoolean(args[7]);

                if (bDebug)
                    Debugger.Launch();

                clsGeneracionPaquetes.fnGenerarPrueba(args[0], args[1]
                                                     , Convert.ToInt32(args[2]), Convert.ToInt32(args[3])
                                                     , Convert.ToInt32(args[4]), Convert.ToBoolean(args[5])
                                                     , Convert.ToDouble(args[6]));


            }

        }

        static void Main(string[] args)
        {
           

            fnGeneracionPruebas(args);
        }
    }
}

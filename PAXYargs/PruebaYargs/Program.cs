using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using PAXYargs;

namespace PruebaYargs
{
    class Program
    {
        static void Main(string[] args)
        {

            Debugger.Launch();

            Yargs yargs = new Yargs(args);

            yargs.Opcion("code", new cOpcion { alias="c",def="a ningun lugar",descripcion="enviar es un parametro", demandaropcion=true,tipo=typeof(string) });

            yargs.Opcion("param", new cOpcion { def = "a ningun lugar", descripcion = "enviar es un parametro", demandaropcion = true, tipo = typeof(string) });

            var argv = yargs.argv;

            //yargs.fnGenerarHelp();
        }
    }
}

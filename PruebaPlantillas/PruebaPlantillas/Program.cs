using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Root.Reports;
using System.Xml;
using System.Threading;
using System.Drawing;
using System.Diagnostics;

namespace PruebaPlantillas
{
    class Program
    {
        static void Main(string[] args)
        {



          ImplementadorPlantilla Plantilla =  ImplementacionPlantillas.PlantillaBeatriz();


          try
          {
              Plantilla.ObtenerPlantilla().Save("Plantilla.pdf");

              RT.ViewPDF("Plantilla.pdf");

          }
          catch 
          {

              try
              {
                  Process[] procesos = Process.GetProcessesByName("AcroRd32");

                  foreach (Process proceso in procesos)
                  {

                      proceso.Kill();

                  }


              }
              catch { }

              Thread.Sleep(3000);

              Plantilla.ObtenerPlantilla().Save("Plantilla.pdf");

              RT.ViewPDF("Plantilla.pdf");

             
          
          }



        }
    }
}

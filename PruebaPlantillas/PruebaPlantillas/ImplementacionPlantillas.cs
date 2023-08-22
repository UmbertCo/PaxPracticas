using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Root.Reports;
using System.Xml;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using PlantillaBeatriz;

namespace PruebaPlantillas
{
    class ImplementacionPlantillas
    {

        public static ImplementadorPlantilla PlantillaBeatriz()
        {
            //476 idContribuyente -- 192.168.3.106 
            //344 RFC 

            XmlDocument xdoc = new XmlDocument();

            xdoc.Load(System.Configuration.ConfigurationManager.AppSettings["XmlBeatriz"].ToString());

            clsPlantillaBeatriz plPlantilla = new clsPlantillaBeatriz(xdoc);

            plPlantilla.fnGenerarPDF(476, 344,"Black");

            return plPlantilla;
        
        }

    }
}

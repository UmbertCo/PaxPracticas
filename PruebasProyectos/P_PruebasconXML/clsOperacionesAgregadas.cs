using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;

namespace P_PruebasconXML
{
   public static class clsOperacionesAgregadas
    {
       public static bool fnFechaDentroRango(this XElement xeFecha,  XElement xeHora, string sFechaInit, string sFechaFin)
        {
            DateTime dtFecha;// = DateTime.ParseExact(xeFecha.Value,"MM/dd/yyyy",CultureInfo.InvariantCulture);
            DateTime.TryParseExact(xeFecha.Value + " " + xeHora.Value, "M/dd/yyyy HH:mm:ss", null,
                                   DateTimeStyles.None, out dtFecha);
            DateTime dtFechaInit = DateTime.Parse(sFechaInit);
            DateTime dtFechaFin = DateTime.Parse(sFechaFin);

            if (dtFecha.Ticks >= dtFechaInit.Ticks && dtFecha.Ticks <= dtFechaFin.Ticks)
                return true;

            return false;

        }

       public static bool fnFechaDentroRango(this XAttribute xeFecha, string sFechaInit, string sFechaFin) 
       {

           DateTime dtFecha;// = DateTime.ParseExact(xeFecha.Value,"MM/dd/yyyy",CultureInfo.InvariantCulture);
           DateTime.TryParseExact(xeFecha.Value, "dd-MM-yyyy HH:mm:ss", null,
                                  DateTimeStyles.None, out dtFecha);
           DateTime dtFechaInit = DateTime.Parse(sFechaInit);
           DateTime dtFechaFin = DateTime.Parse(sFechaFin);

           if (dtFecha.Ticks >= dtFechaInit.Ticks && dtFecha.Ticks <= dtFechaFin.Ticks)
               return true;

           return false;
       
       }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SolucionPruebas.WindowsService.ActualizaComprobante
{
    public static class clsLog
    {
        public static void fnEscribir(string psRuta, string psMensajeError)
        {
            DateTime Fecha = DateTime.Today;
            string sPathex;
            try
            {
                sPathex = psRuta + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                if (!File.Exists(sPathex))
                {
                    StreamWriter sr4 = new StreamWriter(sPathex);
                    sr4.WriteLine(psMensajeError);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(sPathex, true);
                    sw4.WriteLine(psMensajeError);
                    sw4.Close();
                }
            }
            catch (Exception ex)
            {
                fnEscribirEventViewer(ex.Message, psMensajeError);
            }
        }
        public static void fnEscribirEventViewer(string psMensaje, string psMensajeOriginal)
        {
            if (!EventLog.SourceExists("PAXCFDIUpdate"))
            {
                EventLog.CreateEventSource("PAXCFDIUpdate", "PAX CFDI Update");
            }

            EventLog PaxEventLog = new EventLog();
            PaxEventLog.Source = "PAXCFDIUpdate";
            PaxEventLog.WriteEntry(psMensaje);
        }
    }
}

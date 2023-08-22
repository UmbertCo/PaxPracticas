using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using wsPracticaTeamFoundation.Properties;

namespace wsPracticaTeamFundation
{
    public class clsLog
    {
        public static void Escribir(string sMensajeError)
        {
            DateTime fecha = DateTime.Today;
            string sPathex;
            try
            {
                sPathex = Settings.Default.rutaLog + fecha.ToString() +".txt";
                if (!File.Exists(sPathex))
                {
                    StreamWriter sr4 = new StreamWriter(sPathex);
                    sr4.WriteLine(sMensajeError);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(sPathex, true);
                    sw4.WriteLine(sMensajeError);
                    sw4.Close();
                }
            }
            catch (Exception ex)
            {
                EventLogPax(ex.Message, sMensajeError);
            }
        }

        private static void EventLogPax(string sMensajeError, string psMensajeOriginal)
        {
            DateTime fecha = DateTime.Today;
            string sPathex;
            try
            {
                sPathex = Settings.Default.rutaLog + fecha.ToString() + ".txt";
                if (!File.Exists(sPathex))
                {
                    StreamWriter sr4 = new StreamWriter(sPathex);
                    sr4.WriteLine(sMensajeError + " " + DateTime.Now);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(sPathex, true);
                    sw4.WriteLine(sMensajeError + " Mensaje original: " + psMensajeOriginal);
                    sw4.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
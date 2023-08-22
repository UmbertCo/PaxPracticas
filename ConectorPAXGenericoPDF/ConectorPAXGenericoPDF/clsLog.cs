using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConectorPAXGenericoPDF
{
    class clsLog
    {
        /// <summary>
        /// Función que se encarga de escribir en los logs
        /// </summary>
        /// <param name="sRuta">Ruta del Log</param>
        /// <param name="sMensajeError">Mensaje de error</param>
        public static void Escribir(string sRuta, string sMensajeError)
        {
            DateTime Fecha = DateTime.Today;
            try
            {
                if (!File.Exists(sRuta))
                {
                    StreamWriter sr4 = new StreamWriter(sRuta);
                    sr4.WriteLine(sMensajeError);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(sRuta, true);
                    sw4.WriteLine(sMensajeError);
                    sw4.Close();
                }
            }
            catch (Exception ex)
            {
                EventLogPax(ex.Message);
            }
        }

        private static void EventLogPax(string sMensajeError)
        {
            string sPathex;
            try
            {
                sPathex = @"C:\PaxLog" + ".txt";
                if (!File.Exists("C:"))
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

            }
        }
    }
}

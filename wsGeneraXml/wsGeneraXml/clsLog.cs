using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace wsGeneraXml
{
    public class clsLog
    {
        public static void Escribir(string sRuta, string sMensajeError)
        {
            string sPathex;
            try
            {
                sPathex = sRuta + ".txt";
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
            string sPathex;
            try
            {
                sPathex = @"C:\PaxLog" + ".txt";
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
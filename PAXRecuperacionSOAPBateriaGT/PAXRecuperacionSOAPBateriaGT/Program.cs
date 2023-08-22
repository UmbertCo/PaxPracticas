using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PAXRecuperacionSOAPBateriaGT
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Verifica si existen carpetas en el cual se almacena los archivos correspondientes
                if (!(Directory.Exists(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString())))
                    Directory.CreateDirectory(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString());

                if (!(Directory.Exists(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["LogError"].ToString())))
                    Directory.CreateDirectory(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["LogError"].ToString());

                eliminarTrace();

            }
            catch (Exception ex)
            {
                clsLog.EscribirLog("Error al crear directorio - " + ex);
            }

            fnIniciarTimbrado();
            eliminarTrace();
        }

        public static void eliminarTrace()
        {
            try
            {
                TraceSource t = new TraceSource("System.Net");
                t.Close();

                TraceSource ts2 = new TraceSource("System.Net.Sockets");
                ts2.Listeners[1].Close();
                ts2.Close();

                if (File.Exists(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString() + "trace.log"))
                    File.Delete(PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["RutaSOAP"].ToString() + "trace.log");
            }
            catch
            {
            }
        }

        private static void fnIniciarTimbrado()
        {
            try
            {
                clsOperacionTimbrado OperacionTimbrado = new clsOperacionTimbrado();
                OperacionTimbrado.fnGeneracionTimbrado();
            }
            catch (Exception ex)
            {
                try
                {
                    DateTime Fecha = DateTime.Today;
                    string path = (String)PAXRecuperacionSOAPBateriaGT.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                    if (!File.Exists(path))
                    {
                        StreamWriter sr4 = new StreamWriter(path);
                        sr4.WriteLine(DateTime.Now + " " + ex.Message);
                        sr4.Close();
                    }
                    else
                    {
                        System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                        sw4.WriteLine(DateTime.Now + " " + ex.Message);
                        sw4.Close();
                    }
                }
                catch
                {
                }
            }
        }
    }
}

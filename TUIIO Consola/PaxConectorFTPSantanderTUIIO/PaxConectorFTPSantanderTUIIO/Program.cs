using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
//using del modo "consola"
using PAXConectorFTPGTCFDI33.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace PAXConectorFTPGTCFDI33
{
    static class Program
    {
        //Consola
        public static System.Timers.Timer tTemporizador;
        public static DateTime tInicial;
        public static DateTime tEvento;
        public static TimeSpan tTrasncurridoAsignado;
        public static FileSystemWatcher WatcherCancelacion;

        static void  tTemporizador_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tTemporizador.Enabled = false;

            tInicial = DateTime.Now;

            try
            {
                if (tInicial > tEvento)
                {


                    clsPrincipal.fnPrincipal();


                }
            }
            catch (Exception ex)
            {
                clsLog.fnFlush(ex.Message);
                clsLog.fnFlush(ex.StackTrace);

            }

            tTemporizador.Enabled = true;

        }



        public static void fnInitTemporizador()
        {
            tInicial = DateTime.Now;
            tEvento = DateTime.Now.AddSeconds(1);

            try
            {
                tTrasncurridoAsignado = Settings.Default.Lapso;
            }
            catch
            {
                tTrasncurridoAsignado = new TimeSpan(0, 0, 30);
            }
            finally
            {
                tEvento.Add(tTrasncurridoAsignado);
            }

            //Consola
            clsPrincipal.fnPrincipal();

            tTemporizador = new System.Timers.Timer();
            tTemporizador.Elapsed += new System.Timers.ElapsedEventHandler(tTemporizador_Elapsed);
            tTemporizador.Interval = 100;
            tTemporizador.Enabled = true;
        }



        //Fin codigo consola

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            int iDebug = 1;

            if (iDebug == 1)
            {


                try
                {
                    fnInitTemporizador();

                    WatcherCancelacion = new MyFileSystemWatcherCancelacion();

                    string PathToFolder = Properties.Settings.Default.MonitorFolder;

                    var dateAndTime = DateTime.Now;
                    int year = dateAndTime.Year;
                    int month = dateAndTime.Month;
                    int day = dateAndTime.Day;

                    string monthName = dateAndTime.ToString("MMMM", CultureInfo.InvariantCulture);

                    clsLog.Instance.LogPath = Properties.Settings.Default.LogFolder;
                    clsLogRespaldo.Instance.LogPath = Properties.Settings.Default.LogRespaldo + @"\" + year + @"\" + monthName + @"\" + day;
                    clsLog.Instance.LogFileName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                    clsLogRespaldo.Instance.LogFileName = "LogRespa" + System.Reflection.Assembly.GetEntryAssembly().GetName().Name;

                    clsLogCancelacion.Instance.LogPath = Properties.Settings.Default.RutaCancelacionLog;
                    clsLogRespaldoCancelacion.Instance.LogPath = Properties.Settings.Default.RutaCancelacionLogRespaldo + @"\" + year + @"\" + monthName + @"\" + day;
                    clsLogCancelacion.Instance.LogFileName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name + "Cancel";
                    clsLogRespaldoCancelacion.Instance.LogFileName = "LogRespa" + System.Reflection.Assembly.GetEntryAssembly().GetName().Name + "Cancel";

                }
                catch (Exception ex)
                {

                    clsLog.WriteLine("No se pudo iniciar el servicio " + DateTime.Now + ex.Message);
                    clsLogRespaldo.WriteLine("No se pudo iniciar el servicio " + DateTime.Now + ex.Message);
                }






                // Código adicional que desea que se ejecute cuando se inicia la aplicación de consola
                Console.WriteLine("Presione cualquier tecla para salir...");
                //
                Console.Read();
            }
            else //Servicio
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new PaxService()
                };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PAXConectorFTPGTCFDI33;

namespace PAXConectorFTPGTCFDI33
{

    public class clsLogCancelacion
    {
        public static clsLogCancelacion Instance = new clsLogCancelacion();

        private clsLogCancelacion()
        {
            LogFileName = "Example";
            LogFileExtension = ".log";
        }

        public StreamWriter Writer { get; set; }

        public string LogPath { get; set; }

        public string LogFileName { get; set; }

        public string LogFileExtension { get; set; }

        public string LogFile { get { return LogFileName + LogFileExtension; } }

        public string LogFullPath { get { return Path.Combine(LogPath, LogFile); } }

        public bool LogExists { get { return File.Exists(LogFullPath); } }

        /// <summary>
        /// Método que se encarga de escribir una nueva línea en el Log
        /// </summary>
        /// <param name="inLogMessage">Nuevo mensaje a escribir</param>
        public void WriteLineToLog(String inLogMessage)
        {
            WriteToLog(inLogMessage + Environment.NewLine);
        }

        /// <summary>
        /// Método que se encarga de escribir una nueva línea en el Log
        /// </summary>
        /// <param name="inLogMessage">Mensaje a escribir</param>
        public void WriteToLog(String inLogMessage)
        {
            try
            {
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                if (Writer == null)
                {
                    Writer = new StreamWriter(LogFullPath, true);
                }

                Writer.Write(inLogMessage);
                Writer.Flush();
            }
            catch (Exception ex)
            {
                EventLogPax(ex.Message, inLogMessage);
            }
        }

        /// <summary>
        /// Método que se encarga de actualizar la ruta de la carpeta del Log con la fecha
        /// </summary>
        public static void WriteLine(String inLogMessage)
        {
            clsLogCancelacion.Instance.WriteLineToLog(inLogMessage);
        }

        /// <summary>
        /// Método que se encarga de escribir en el Log
        /// </summary>
        /// <param name="inLogMessage">Mensaje a escribir</param>
        public static void Write(String inLogMessage)
        {
            clsLogCancelacion.Instance.WriteToLog(inLogMessage);
        }

        private static void EventLogPax(string sMensajeError, string psMensajeOriginal)
        {
            string sPathex;
            try
            {
                sPathex = @"C:\PaxLogCancelacion" + ".txt";
                if (!File.Exists(sPathex))
                {
                    StreamWriter sr4 = new StreamWriter(sPathex);
                    sr4.WriteLine(sMensajeError + " " + DateTime.Now);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(sPathex, true);
                    sw4.WriteLine(sMensajeError + " Error original: " + psMensajeOriginal);
                    sw4.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}


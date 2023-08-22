using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PAXConectorFTPGTCFDI33.Properties;

namespace PAXConectorFTPGTCFDI33
{
    public class clsLogRespaldoCancelacion
    {
        public static clsLogRespaldoCancelacion Instance = new clsLogRespaldoCancelacion();

        private clsLogRespaldoCancelacion()
        {
            LogFileName = "Example";
            LogFileExtension = ".log";
        }

        public StreamWriter Writer { get; set; }

        public string LogPath { get; set; }

        public DateTime Fecha { get; set; }

        public string LogFileName { get; set; }

        public string LogFileExtension { get; set; }

        public string LogFile { get { return LogFileName + LogFileExtension; } }

        public string LogFullPath { get { return Path.Combine(LogPath, LogFile); } }

        public bool LogExists { get { return File.Exists(LogFullPath); } }

        /// <summary>
        /// Método que se encarga de escribir una nueva línea en el Log de respaldo
        /// </summary>
        /// <param name="inLogMessage">Nuevo mensaje a escribir</param>
        public void WriteLineToLog(String inLogMessage)
        {
            WriteToLog(inLogMessage + Environment.NewLine);
        }

        /// <summary>
        /// Método que se encarga de escribir una nueva línea en el Log de respaldo
        /// </summary>
        /// <param name="inLogMessage">Mensaje a escribir</param>
        public void WriteToLog(String inLogMessage)
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

        /// <summary>
        /// Método que se encarga de escribir en el Log de respaldo una nueva línea
        /// </summary>
        /// <param name="inLogMessage">Mensaje</param>
        public static void WriteLine(String inLogMessage)
        {
            ActualizaFolder();
            clsLogRespaldoCancelacion.Instance.WriteLineToLog(inLogMessage);
        }

        /// <summary>
        /// Método que se encarga de actualizar la ruta de la carpeta del Log de respaldo con la fecha
        /// </summary>
        public static void ActualizaFolder()
        {
            if (clsLogRespaldoCancelacion.Instance.Fecha != DateTime.Today)
            {
                var dateAndTime = DateTime.Now;
                int year = dateAndTime.Year;
                int month = dateAndTime.Month;
                int day = dateAndTime.Day;
                string monthName = dateAndTime.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture);
               clsLogRespaldoCancelacion.Instance.LogPath = Properties.Settings.Default.RutaCancelacionLogRespaldo + @"\" + year + @"\" + monthName + @"\" + day;
            }
        }

        /// <summary>
        /// Método que se encarga de escribir en el Log de respaldo
        /// </summary>
        /// <param name="inLogMessage">Mensaje a escribir</param>
        public static void Write(String inLogMessage)
        {
            ActualizaFolder();
            clsLogRespaldoCancelacion.Instance.WriteToLog(inLogMessage);
        }
    }
}

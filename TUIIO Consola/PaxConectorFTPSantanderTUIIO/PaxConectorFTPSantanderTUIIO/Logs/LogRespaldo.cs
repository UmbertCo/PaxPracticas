using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace PAXConectorFTPGTCFDI33
{
    class clsLogRespaldo
    {
        public static clsLogRespaldo Instance = new clsLogRespaldo();

        private clsLogRespaldo()
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

        public void WriteLineToLog(String inLogMessage)
        {
            WriteToLog(inLogMessage + Environment.NewLine);
        }

        public void WriteToLog(String inLogMessage)
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }

            using (StreamWriter swLog = new StreamWriter(LogFullPath, true))
            {
                swLog.Write(inLogMessage);
                swLog.Flush();
            }
        }

        public static void WriteLine(String inLogMessage)
        {
            ActualizaFolder();
            clsLogRespaldo.Instance.WriteLineToLog(inLogMessage);
        }

        public static void Write(String inLogMessage)
        {
            ActualizaFolder();
            clsLogRespaldo.Instance.WriteToLog(inLogMessage);
        }

        public static void ActualizaFolder()
        {
            if (clsLogRespaldo.Instance.Fecha != DateTime.Today)
            {
                var dateAndTime = DateTime.Now;
                int year = dateAndTime.Year;
                int month = dateAndTime.Month;
                int day = dateAndTime.Day;
                string monthName = dateAndTime.ToString("MMMM", CultureInfo.InvariantCulture);
                clsLogRespaldo.Instance.LogPath = Properties.Settings.Default.LogRespaldo + @"\" + year + @"\" + monthName + @"\" + day;
            }
        }
    }
}


using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Reflection;

namespace PAXConectorFTPGTCFDI33
{
    public class clsLog
    {
        public static clsLog Instance = new clsLog();

        private clsLog()
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

            if (Writer == null)
            {
                Writer = new StreamWriter(LogFullPath, true);
            }

            Writer.Write(inLogMessage);
            Writer.Flush();
        }

        public static void WriteLine(String inLogMessage)
        {
            clsLog.Instance.WriteLineToLog(inLogMessage);
        }

        public static void Write(String inLogMessage)
        {
            clsLog.Instance.WriteToLog(inLogMessage);
        }

        public static void fnFlush(string sOut)
        {
            string sruta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "Log.xml";
            XmlDocument xdDoc = new XmlDocument();
            try
            {
                xdDoc.Load(sruta);
            }
            catch (FileNotFoundException ex)
            {
                xdDoc.LoadXml("<LogOut></LogOut>");
            }
            catch
            {
                return;
            }

            XPathNavigator xNavegador = null;
            try
            {
                xNavegador = xdDoc.CreateNavigator().SelectSingleNode("/LogOut/Entrada[@fechaCreacion='" + DateTime.Now.ToString("dd-MM-yyyy HH:mm") + "']");

                try
                {
                    xNavegador.AppendChild(sOut);
                }
                catch
                {
                    xNavegador.SetValue(sOut);
                }
                xdDoc.Save(sruta);
                return;
            }
            catch (NullReferenceException ex)
            {
                try
                {
                    XmlDocument xdEntrada = new XmlDocument();

                    xdEntrada.LoadXml("<Entrada fechaCreacion='" + DateTime.Now.ToString("dd-MM-yyyy HH:mm") + "'></Entrada>");

                    xNavegador = xdDoc.CreateNavigator().SelectSingleNode("/LogOut");

                    try
                    {
                        xdEntrada.CreateNavigator().SelectSingleNode("/Entrada").AppendChild(sOut);
                    }
                    catch
                    {
                        xdEntrada.CreateNavigator().SelectSingleNode("/Entrada").SetValue(sOut);
                    }

                    xNavegador.AppendChild(xdEntrada.InnerXml);

                    xdDoc.Save(sruta);
                }
                catch { return; }
            }
            catch { return; }
        }
    }
}

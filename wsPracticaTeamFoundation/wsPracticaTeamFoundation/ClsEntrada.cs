using ICSharpCode;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;
using wsPracticaTeamFoundation.Properties;

namespace wsPracticaTeamFoundation
{
    class ClsEntrada
    {
        public void buscarEntrada()
        {
            verificarCarpetas();

            clsValidacion cValidacion = new clsValidacion();
            List<string> xXmlList = new List<string>();
            string pathTemp = Settings.Default.rutaTemporal;
            string pathXml = Settings.Default.rutaXml;
            string pathZip = Settings.Default.rutaEntrada;
            string[] sFiles = null;
            string sFiltro = "*.zip";
            StringWriter swXml = new StringWriter();
            XmlDocument xDocument = new XmlDocument();
            XmlTextWriter xtwXml = new XmlTextWriter(swXml);

            sFiles = Directory.GetFiles(pathZip, sFiltro);
            foreach (string archivo in sFiles)
            {
                FastZip fZip = new FastZip();
                string sDocument = string.Empty;
                string sFileName = string.Empty;
                string[] sFilesXml = null;
                string sFiltroXml = ".xml";
                fZip.ExtractZip(archivo, pathXml, sFiltroXml);
                sFileName = System.IO.Path.GetFileNameWithoutExtension(archivo);
                sFilesXml = Directory.GetFiles(pathXml + sFileName);
                foreach (string xmlFile in sFilesXml)
                {
                    xDocument.Load(xmlFile);
                    xDocument.WriteTo(xtwXml);
                    sDocument = swXml.ToString();
                    xXmlList.Add(sDocument);
                }
            }
            cValidacion.fnValidacion(xXmlList);
            try
            {
                Directory.Delete(pathTemp, true);
            }
            catch(Exception e) { }
        }
        public void verificarCarpetas()
        {
            if (!(Directory.Exists(Properties.Settings.Default.rutaEntrada)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaEntrada);
            if (!(Directory.Exists(Properties.Settings.Default.rutaErrores)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaErrores);
            if (!(Directory.Exists(Properties.Settings.Default.rutaLog)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaLog);
            if (!(Directory.Exists(Properties.Settings.Default.rutaSalida)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaSalida);
            if (!(Directory.Exists(Properties.Settings.Default.rutaTemporal)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaTemporal);
            if (!(Directory.Exists(Properties.Settings.Default.rutaXml)))
                Directory.CreateDirectory(Properties.Settings.Default.rutaXml);
        }
    }
}

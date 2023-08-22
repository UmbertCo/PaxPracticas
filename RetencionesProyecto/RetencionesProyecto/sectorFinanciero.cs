﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.XPath;
using System.Xml;
using RetencionesProyecto.Properties;
using System.IO;

namespace RetencionesProyecto
{
    class sectorFinanciero
    {
    
        CultureInfo languaje;

        public string Version { get; set; }
        public string IdFideicom { get; set; }
        public string NomFideicom { get; set; }
        public string DescripFideicom { get; set; }
  
     

        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public sectorFinanciero(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {

                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                IdFideicom = navDetalle.SelectSingleNode("@IdFideicom", nsmComprobante).Value;
                NomFideicom = navDetalle.SelectSingleNode("@NomFideicom", nsmComprobante).Value;
                DescripFideicom = navDetalle.SelectSingleNode("@DescripFideicom", nsmComprobante).Value;

            }
            catch(Exception ex) 
            {
                Log("No se encontro un dato requerido" + ex.ToString());
            }
        }

        public static void Log(string e)
        {
            string path = Settings.Default.LogError + "LOG.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine("Error " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
            else if (File.Exists(path))
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("MError " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
        }
    }
}

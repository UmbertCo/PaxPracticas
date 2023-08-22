using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.XPath;
using System.IO;
using System.Xml.Xsl;
using System.Xml;
using Timbrado;
using RetencionesProyecto.Properties;

namespace RetencionesProyecto
{
    public class Transformacion
    {

        /// <summary>
        /// Transforma un valor a su representación porcentual
        /// </summary>
        /// <param name="valor">cadena con el valor a ser transformado</param>
        /// <returns>Cadena con el nuevo valor porcentual</returns>
        public static string ruta = Settings.Default.LogError;
        public static string rutaXSLT = System.AppDomain.CurrentDomain.BaseDirectory;

        public static void Log(string e)
        {
            string path = ruta + "\\LOG.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine("Error " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
            }
            else if (File.Exists(path))
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("MError " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
            }
        }

        public static string fnFormatoPorcentaje(string valor)
        {
            CultureInfo languaje;
            languaje = new CultureInfo("es-Mx");

            try
            {
                //return string.Format("{0:F2}%", Convert.ToDouble(valor));
                return Convert.ToDouble(valor, languaje).ToString("F2", languaje) + "%";
            }
            catch
            {
                return valor;
            }
        }

        /// <summary>
        /// Transforma un valor a su representación de formato monetario
        /// </summary>
        /// <param name="valor">cadena con el valor a ser transformado</param>
        /// <returns>Cadena con el nuevo valor en formato monetario</returns>
        public static string fnFormatoCurrency(string valor)
        {
            CultureInfo languaje;
            languaje = new CultureInfo("es-Mx");

            //return string.Format("{0:c2}", Convert.ToDouble(valor));
            return Convert.ToDouble(valor, languaje).ToString("c2", languaje);
        }

        /// <summary>
        /// Formatea el valor a dos decimales
        /// </summary>
        /// <param name="valor">cadena con el valor a ser transformado</param>
        /// <returns>Cadena con el nuevo valor en formato monetario</returns>
        public static string fnFormatoRedondeo(string valor)
        {
            CultureInfo languaje;
            languaje = new CultureInfo("es-Mx");

            //return string.Format("{0:n2}", Convert.ToDouble(valor));
            return Convert.ToDouble(valor, languaje).ToString("n2", languaje);
        }

        /// <summary>
        /// Contruye la cadena original a partir de un XML de CFDI
        /// </summary>
        /// <param name="xml">Objeto navegador del XML</param>
        /// <returns>Retorna la cadena original</returns>
        public static string fnConstruirCadenaTimbrado(IXPathNavigable xml)
        {
            string sCadenaOriginal = string.Empty;

            try
            {
                MemoryStream ms = new MemoryStream();
                XslCompiledTransform trans = new XslCompiledTransform();
                trans.Load(XmlReader.Create(new StringReader(Resources.cadenaoriginal_TFD_1_0)));
                XsltArgumentList args = new XsltArgumentList();
                trans.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                sCadenaOriginal = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return sCadenaOriginal;
        }
    }

       
}

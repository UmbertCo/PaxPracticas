using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.XPath;
using System.Xml;
using PaxConectorRetenciones.Properties;
using System.IO;

namespace PaxConectorRetenciones
{
    class planesDeRetiro
    {
        CultureInfo languaje;

        public string Version { get; set; }
        public string EntidadFederativa { get; set; }
        public string SistemaFinanc { get; set; }
        public string HuboRetirosAnioInmAntPer { get; set; }
        public string HuboRetirosAnioInmAnt { get; set; }

        public string sMontTotAportAnioInmAnterior;
        public string MontTotAportAnioInmAnterior
        {
            get { return sMontTotAportAnioInmAnterior; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotAportAnioInmAnterior = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }


        private string sMontIntRealesDevengAniooInmAnt;
        public string MontIntRealesDevengAniooInmAnt
        {
            get { return sMontIntRealesDevengAniooInmAnt; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sMontIntRealesDevengAniooInmAnt = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        private string sMontTotRetiradoAnioInmAntPer;
        public string MontTotRetiradoAnioInmAntPer
        {
            get { return sMontTotRetiradoAnioInmAntPer; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotRetiradoAnioInmAntPer = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        private string sMontTotExentRetiradoAnioInmAnt;
        public string MontTotExentRetiradoAnioInmAnt
        {
            get { return sMontTotExentRetiradoAnioInmAnt; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotRetiradoAnioInmAntPer = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        private string sMMontTotExedenteAnioInmAnt;
        public string MMontTotExedenteAnioInmAnt
        {
            get { return sMMontTotExedenteAnioInmAnt; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotRetiradoAnioInmAntPer = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }
        private string sMontTotRetiradoAnioInmAnt;
        public string MontTotRetiradoAnioInmAnt
        {
            get { return sMontTotRetiradoAnioInmAnt; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotRetiradoAnioInmAnt = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }


        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public planesDeRetiro(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {

                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                SistemaFinanc = navDetalle.SelectSingleNode("@SistemaFinanc", nsmComprobante).Value;
                try { MontTotAportAnioInmAnterior = navDetalle.SelectSingleNode("@MontTotAportAnioInmAnterior", nsmComprobante).Value; }catch { }
                MontIntRealesDevengAniooInmAnt = navDetalle.SelectSingleNode("@MontIntRealesDevengAniooInmAnt", nsmComprobante).Value;
                HuboRetirosAnioInmAntPer = navDetalle.SelectSingleNode("@HuboRetirosAnioInmAntPer", nsmComprobante).Value;
                try { MontTotRetiradoAnioInmAntPer = navDetalle.SelectSingleNode("@MontTotRetiradoAnioInmAntPer", nsmComprobante).Value; }catch { }
                try {MontTotExentRetiradoAnioInmAnt = navDetalle.SelectSingleNode("@MontTotExentRetiradoAnioInmAnt", nsmComprobante).Value;}catch { }
                try {MMontTotExedenteAnioInmAnt = navDetalle.SelectSingleNode("@MMontTotExedenteAnioInmAnt", nsmComprobante).Value;}catch { }
                HuboRetirosAnioInmAnt = navDetalle.SelectSingleNode("@HuboRetirosAnioInmAnt", nsmComprobante).Value;
                try { MontTotRetiradoAnioInmAnt = navDetalle.SelectSingleNode("@MontTotRetiradoAnioInmAnt", nsmComprobante).Value; }catch { }

            }
            catch (Exception ex)
            {
                Log("No se encontro un dato requerido" + ex.ToString());
            }
        }
        public static void Log(string e)
        {
            DateTime Fecha = DateTime.Today;
            string path = Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
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

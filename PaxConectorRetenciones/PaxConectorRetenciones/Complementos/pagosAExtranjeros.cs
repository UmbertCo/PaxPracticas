using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Globalization;
using PaxConectorRetenciones.Properties;
using System.IO;

namespace PaxConectorRetenciones
{
    class pagosAExtranjeros
    {
             CultureInfo languaje;

        public string Version { get; set; }

        //Es beneficiaro del pago
        public string EsBenefEfectDelCobro { get; set; }

        //No Beneficiario

        //Pais de Residencia
        public string PaisDeResidParaEfecFisc { get; set; }

        //Concepto de Pago
        public string ConceptoPago { get; set; }

        //Descripcion del Concepto de Pago
        public string DescripcionConcepto { get; set; }

        //Beneficiario

        public string RFC { get; set; }

        public string CURP { get; set; }

        public string NomDenRazSocB { get; set; }

        public string ConceptoPagoBen { get; set; }

        public string DescripcionConceptoBen { get; set; }





        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public pagosAExtranjeros(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {
                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                EsBenefEfectDelCobro = navDetalle.SelectSingleNode("@EsBenefEfectDelCobro", nsmComprobante).Value;
                PaisDeResidParaEfecFisc = navDetalle.SelectSingleNode("pagosaextranjeros:NoBeneficiario/@PaisDeResidParaEfecFisc", nsmComprobante).Value;
                ConceptoPago = navDetalle.SelectSingleNode("pagosaextranjeros:NoBeneficiario/@ConceptoPago", nsmComprobante).Value;
                DescripcionConcepto = navDetalle.SelectSingleNode("pagosaextranjeros:NoBeneficiario/@DescripcionConcepto", nsmComprobante).Value;
                RFC = navDetalle.SelectSingleNode("pagosaextranjeros:Beneficiario/@RFC", nsmComprobante).Value;
                CURP = navDetalle.SelectSingleNode("pagosaextranjeros:Beneficiario/@CURP", nsmComprobante).Value;
                NomDenRazSocB = navDetalle.SelectSingleNode("pagosaextranjeros:Beneficiario/@NomDenRazSocB", nsmComprobante).Value;
                ConceptoPagoBen = navDetalle.SelectSingleNode("pagosaextranjeros:Beneficiario/@ConceptoPago", nsmComprobante).Value;
                DescripcionConceptoBen = navDetalle.SelectSingleNode("pagosaextranjeros:Beneficiario/@DescripcionConcepto", nsmComprobante).Value;

               
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

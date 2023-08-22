using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;

namespace RetencionesProyecto
{
    public class Impuesto
    {
        public string Nombre { get; set; }
        public string Tasa { get; set; }
        public string Importe { get; set; }

        //Esta propiedad retorna el texto del renglon a mostrar en el PDF
        public string TextoImpuesto
        {
            get
            {
                return Nombre + " " + Tasa + " " + Importe;
            }
        }

        /// <summary>
        /// Crea una nueva instancia de Impuesto
        /// </summary>
        /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
        /// <param name="nsmComprobante"></param>
        public Impuesto(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
        {
            Nombre = navImpuesto.SelectSingleNode("@impuesto", nsmComprobante).Value;
            if (Nombre != "IEPS")
            {
                try { Tasa = navImpuesto.SelectSingleNode("@tasa", nsmComprobante).Value; }
                catch { Tasa = "Retención"; }
                Importe = navImpuesto.SelectSingleNode("@importe", nsmComprobante).Value;
            }
        }

    }
}

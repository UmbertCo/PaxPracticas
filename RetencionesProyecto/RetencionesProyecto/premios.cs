using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.XPath;
using System.Xml;

namespace RetencionesProyecto
{
    class premios
    {
        CultureInfo languaje;

        public string Version { get; set; }
        public string EntidadFederativa { get; set; }
     

      

        public string sMontTotPagoExent;
        public string MontTotPagoExent
        {
            get { return sMontTotPagoExent; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotPagoExent = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }


        private string sMontTotPagoGrav;
        public string MontTotPagoGrav
        {
            get { return sMontTotPagoGrav; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotPagoGrav = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        private string sMontTotPago;
        public string MontTotPago
        {
            get { return sMontTotPago; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotPago = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }



        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public premios(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {

                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                EntidadFederativa = navDetalle.SelectSingleNode("@EntidadFederativa", nsmComprobante).Value;
                MontTotPago = navDetalle.SelectSingleNode("@MontTotPago", nsmComprobante).Value;
                MontTotPagoGrav = navDetalle.SelectSingleNode("@MontTotPagoGrav", nsmComprobante).Value;
                MontTotPagoExent = navDetalle.SelectSingleNode("@MontTotPagoExent", nsmComprobante).Value;
               
            }
            catch { }
        }
    }
}

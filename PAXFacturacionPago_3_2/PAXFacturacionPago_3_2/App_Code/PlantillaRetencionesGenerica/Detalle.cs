using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Globalization;

    public class Detalle
    {
        CultureInfo languaje;

        public string Impuesto { get; set; }

        public string TipoPagoRet { get; set; }

        private string sBaseRet;
        public string BaseRet
        {
            get { return sBaseRet; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
                sBaseRet = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        private string sMontoRet;
        public string MontoRet
        {
            get { return sMontoRet; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontoRet = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }



        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public Detalle(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {

                MontoRet = navDetalle.SelectSingleNode("@montoRet", nsmComprobante).Value;
                TipoPagoRet = navDetalle.SelectSingleNode("@TipoPagoRet", nsmComprobante).Value;
                try { BaseRet = navDetalle.SelectSingleNode("@BaseRet", nsmComprobante).Value; }
                catch { BaseRet = string.Empty; }
                try { Impuesto = navDetalle.SelectSingleNode("@Impuesto", nsmComprobante).Value; }
                catch { Impuesto = string.Empty; }
            }
            catch(Exception ex) { clsErrorLogPDFRet.fnNuevaEntrada(ex, clsErrorLogPDFRet.TipoErroresLog.Referencia); }
        }

    }


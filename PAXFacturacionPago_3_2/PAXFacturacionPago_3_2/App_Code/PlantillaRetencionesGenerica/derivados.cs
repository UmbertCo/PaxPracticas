using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.XPath;
using System.Xml;
using System.IO;

    class derivados
    {
    
        CultureInfo languaje;

        public string Version { get; set; }


        private string sMontGanAcum;
        public string MontGanAcum
        {
            get { return sMontGanAcum; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontGanAcum = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }
        private string sMontPerdDed;
        public string MontPerdDed
        {
            get { return sMontPerdDed; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontPerdDed = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }


        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public derivados(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {

                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                MontGanAcum = navDetalle.SelectSingleNode("@MontGanAcum", nsmComprobante).Value;
                MontPerdDed = navDetalle.SelectSingleNode("@MontPerdDed", nsmComprobante).Value;
          
            }
            catch (Exception ex)
            {
                clsErrorLogPDFRet.fnNuevaEntrada(ex, clsErrorLogPDFRet.TipoErroresLog.Referencia);
            }
        }

   
    }


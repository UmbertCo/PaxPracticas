using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Globalization;
using System.IO;


    class enajenacionDeAcciones
    {
            
        CultureInfo languaje;

        public string Version { get; set; }

        //Tipo de contrato
        public string ContratoIntermediacion { get; set; }


        //Ganancia
        public string sGanancia;
        public string Ganancia
        {
            get { return sGanancia; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sGanancia = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Perdida
        public string sPerdida;
        public string Perdida
        {
            get { return sPerdida; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sPerdida = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }


        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public enajenacionDeAcciones(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {
                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                ContratoIntermediacion = navDetalle.SelectSingleNode("@ContratoIntermediacion", nsmComprobante).Value;
                Ganancia = navDetalle.SelectSingleNode("@Ganancia", nsmComprobante).Value;
                Perdida = navDetalle.SelectSingleNode("@Perdida", nsmComprobante).Value;
               
            }
            catch (Exception ex)
            {
                clsErrorLogPDFRet.fnNuevaEntrada(ex, clsErrorLogPDFRet.TipoErroresLog.Referencia);
            }
        }


    }


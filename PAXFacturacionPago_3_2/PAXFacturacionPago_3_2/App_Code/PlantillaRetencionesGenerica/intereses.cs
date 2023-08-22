using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Globalization;
using System.IO;


    class intereses
    {
        CultureInfo languaje;

        public string Version { get; set; }

        //Sistema Financiero
        public string SistFinanciero { get; set; }

        //Retiro de Intereses
        public string RetiroAORESRetInt { get; set; }

        //Operaciones Financieras Derivadas
        public string OperFinancDerivad { get; set; }

        //Importe de Interes Nominal
        public string sMontIntNominal;
        public string MontIntNominal
        {
            get { return sMontIntNominal; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontIntNominal = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Monto de Intereses Reales
        public string sMontIntReal;
        public string MontIntReal
        {
            get { return sMontIntReal; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontIntReal = Convert.ToDouble(value, languaje).ToString("c", languaje);
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
        public intereses(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {
                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                SistFinanciero = navDetalle.SelectSingleNode("@SistFinanciero", nsmComprobante).Value;
                RetiroAORESRetInt = navDetalle.SelectSingleNode("@RetiroAORESRetInt", nsmComprobante).Value;
                OperFinancDerivad = navDetalle.SelectSingleNode("@OperFinancDerivad ", nsmComprobante).Value;
                MontIntNominal = navDetalle.SelectSingleNode("@MontIntNominal", nsmComprobante).Value;
                MontIntReal = navDetalle.SelectSingleNode("@MontIntReal", nsmComprobante).Value;
                Perdida = navDetalle.SelectSingleNode("@Perdida", nsmComprobante).Value;
               
            }
            catch (Exception ex)
            {
                clsErrorLogPDFRet.fnNuevaEntrada(ex, clsErrorLogPDFRet.TipoErroresLog.Referencia);  
            }
        }


    }


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;
using System.Globalization;
using System.IO;


    class dividendos
    {
        
        CultureInfo languaje;

        public string Version { get; set; }

        //Tipo
        public string CveTipDivOUtil { get; set; }

        //Importe de dividendo o utilidad
        public string sMontISRAcredRetMexico;
        public string MontISRAcredRetMexico
        {
            get { return sMontISRAcredRetMexico; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontISRAcredRetMexico = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Importe o retencion del dividendo o utilidad en el extranjero
        public string sMontISRAcredRetExtranjero;
        public string MontISRAcredRetExtranjero
        {
            get { return sMontISRAcredRetExtranjero; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontISRAcredRetExtranjero = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Monto de retencion en el extrenajero sobre dividendos
        public string sMontRetExtDivExt;
        public string MontRetExtDivExt
        {
            get { return sMontRetExtDivExt; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontRetExtDivExt = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Tipo de sociedad de dividendo distribuido
        public string TipoSocDistrDiv { get; set; }
    
        //Monto de ISR acreditable Nacional
        public string sMontISRAcredNal;
        public string MontISRAcredNal
        {
            get { return sMontISRAcredNal; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontISRAcredNal = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Monto de dividendo acomulable nacional
        public string sMontDivAcumNal;
        public string MontDivAcumNal
        {
            get { return sMontDivAcumNal; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontDivAcumNal = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Monto del dividendo acomulable extranjero
        public string sMontDivAcumExt;
        public string MontDivAcumExt
        {
            get { return sMontDivAcumExt; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontDivAcumExt = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Proporcional Remanente
        public string sProporcionRem;
        public string ProporcionRem
        {
            get { return sProporcionRem; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sProporcionRem = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public dividendos(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {

                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                CveTipDivOUtil = navDetalle.SelectSingleNode("dividendos:DividOUtil/@CveTipDivOUtil", nsmComprobante).Value;
                MontISRAcredRetMexico = navDetalle.SelectSingleNode("dividendos:DividOUtil/@MontISRAcredRetMexico", nsmComprobante).Value;
                MontISRAcredRetExtranjero = navDetalle.SelectSingleNode("dividendos:DividOUtil/@MontISRAcredRetExtranjero", nsmComprobante).Value;
                MontRetExtDivExt = navDetalle.SelectSingleNode("dividendos:DividOUtil/@MontRetExtDivExt", nsmComprobante).Value;
                TipoSocDistrDiv = navDetalle.SelectSingleNode("dividendos:DividOUtil/@TipoSocDistrDiv", nsmComprobante).Value;
                MontISRAcredNal = navDetalle.SelectSingleNode("dividendos:DividOUtil/@MontISRAcredNal", nsmComprobante).Value;
                MontDivAcumNal = navDetalle.SelectSingleNode("dividendos:DividOUtil/@MontDivAcumNal", nsmComprobante).Value;
                MontDivAcumExt = navDetalle.SelectSingleNode("dividendos:DividOUtil/@MontDivAcumExt", nsmComprobante).Value;
                ProporcionRem = navDetalle.SelectSingleNode("dividendos:Remanente/@ProporcionRem", nsmComprobante).Value;

               
            }
            catch (Exception ex)
            {
                clsErrorLogPDFRet.fnNuevaEntrada(ex, clsErrorLogPDFRet.TipoErroresLog.Referencia);
            }
        }

   
    }


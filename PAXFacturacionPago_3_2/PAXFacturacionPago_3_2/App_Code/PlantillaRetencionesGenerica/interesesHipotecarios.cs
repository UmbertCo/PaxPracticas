using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Xml.XPath;
using System.Xml;
using System.IO;


    class interesesHipotecarios
    {
        CultureInfo languaje;

        public string Version { get; set; }

        //Credito Otorgado por institucion financiera
        public string CreditoDeInstFinanc { get; set; }


        //Saldo Insoluto
        public string sSaldoInsoluto;
        public string SaldoInsoluto
        {
            get { return sSaldoInsoluto; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sSaldoInsoluto = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Proporcional deducible del credito
        public string sPropDeducDelCredit;
        public string PropDeducDelCredit
        {
            get { return sPropDeducDelCredit; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sPropDeducDelCredit = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Monto total de intereses nominales
        public string sMontTotIntNominalesDev;
        public string MontTotIntNominalesDev
        {
            get { return sMontTotIntNominalesDev; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotIntNominalesDev = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        //Monto total de intereses nominales devengados y pagados
        public string sMontTotNominalesDevYPag;
        public string MontTotNominalesDevYPag
        {
            get { return sMontTotNominalesDevYPag; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotNominalesDevYPag = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }


        public string sMontTotIntRealPagDeduc;
        public string MontTotIntRealPagDeduc
        {
            get { return sMontTotIntRealPagDeduc; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sMontTotIntRealPagDeduc = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        public string sNumContrato;
        public string NumContrato
        {
            get { return sNumContrato; }
            set
            {
                languaje = new CultureInfo("es-Mx");

                //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
                sNumContrato = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }


        /// <summary>
        /// Crea una nueva instancia de Detalle
        /// </summary>
        /// <param name="navDetalle">Navegador con los datos del concepto</param>
        /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
        public interesesHipotecarios(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
        {
            try
            {
                Version = navDetalle.SelectSingleNode("@Version", nsmComprobante).Value;
                CreditoDeInstFinanc = navDetalle.SelectSingleNode("@CreditoDeInstFinanc", nsmComprobante).Value;
                SaldoInsoluto = navDetalle.SelectSingleNode("@SaldoInsoluto", nsmComprobante).Value;
                try {PropDeducDelCredit = navDetalle.SelectSingleNode("@PropDeducDelCredit", nsmComprobante).Value; }catch { }
                try {MontTotIntNominalesDev = navDetalle.SelectSingleNode("@MontTotIntNominalesDev", nsmComprobante).Value;}catch { }
                try {MontTotNominalesDevYPag = navDetalle.SelectSingleNode("@MontTotNominalesDevYPag", nsmComprobante).Value;}catch { }
                try {MontTotIntRealPagDeduc = navDetalle.SelectSingleNode("@MontTotIntRealPagDeduc", nsmComprobante).Value;}catch { }
                try { NumContrato = navDetalle.SelectSingleNode("@NumContrato", nsmComprobante).Value; }catch { }

               
            }
            catch (Exception ex)
            {
                clsErrorLogPDFRet.fnNuevaEntrada(ex, clsErrorLogPDFRet.TipoErroresLog.Referencia);
            }
        }

      
    }


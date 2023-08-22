using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Globalization;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.XPath;

namespace ClassDevExpress
{
    public partial class rptPlantillaGenerica : DevExpress.XtraReports.UI.XtraReport
    {
        public rptPlantillaGenerica()
        {
            InitializeComponent();
        }
        private void xrBarCodeQRCFDI_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRBarCode BarCodeQRCFDI = (XRBarCode)sender;
            string sCadenaCodigo = string.Empty;

            sCadenaCodigo = "?re=" + ((XRTableCell)FindControl("CellEmisorRFC", true)).Text.Replace("RFC:", "").Trim()
                          + "&rr=" + ((XRTableCell)FindControl("CellReceptorRFC", true)).Text.Replace("RFC:", "").Trim()
                          + "&tt=" + string.Format("{0:0000000000.000000}", Convert.ToDouble(GetCurrentColumnValue("total").ToString()))
                          + "&id=" + ((XRTableCell)FindControl("CellUUID", true)).Text.Trim();
            (sender as XRBarCode).Text = sCadenaCodigo;
        }

        private void CellMetodoDePago_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            (sender as XRTableCell).Text = fnComparaMetodoPago(GetCurrentColumnValue("metodoDePago").ToString()); 
        }

        /// Compara metodos de pago con su clave y obtiene su descripcion
        /// </summary>
        /// <returns>string</returns>
        public string fnComparaMetodoPago(string psMetodo)
        {
            string sDescripcionOut = string.Empty;

            try
            {
                string[] sMetodos;
                if (psMetodo.Contains(","))
                {
                    sMetodos = psMetodo.Split(',');
                }
                else
                {
                    sMetodos = new string[] { psMetodo };
                }

                foreach (string sMetodo in sMetodos)
                {
                    XmlDocument xmlMetodos = new XmlDocument();

                    xmlMetodos.Load(AppDomain.CurrentDomain.BaseDirectory + "MetodoPago.xml");

                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlMetodos.NameTable);
                    nsmComprobante.AddNamespace("mp", "mp:ListaMetodosPago");

                    XPathNavigator navComprobante = xmlMetodos.CreateNavigator();
                    XPathNodeIterator navDetalles = navComprobante.Select("/mp:ListaMetodosPago/mp:MetodosPago", nsmComprobante);

                    while (navDetalles.MoveNext())
                    {
                        XPathNavigator nodenavigator = navDetalles.Current;

                        if (nodenavigator.HasChildren)//Si contiene nodo hijo
                        {
                            XPathNodeIterator navDetallesMetodos = nodenavigator.Select("mp:MetodoPago", nsmComprobante);

                            while (navDetallesMetodos.MoveNext())
                            {
                                XPathNavigator navDetallesPago = navDetallesMetodos.Current;

                                string Clave = navDetallesPago.SelectSingleNode("@Clave", nsmComprobante).Value;
                                string Descripcion = navDetallesPago.SelectSingleNode("@Descripcion", nsmComprobante).Value;
                                string Estatus = navDetallesPago.SelectSingleNode("@Estatus", nsmComprobante).Value;

                                if (Estatus.Equals("A"))
                                {
                                    if (sMetodo.Equals(Clave))
                                    {
                                        if (sDescripcionOut.Equals(string.Empty))
                                        {
                                            sDescripcionOut = Clave + " " + Descripcion;
                                        }
                                        else
                                        {
                                            sDescripcionOut = sDescripcionOut + ", " + Clave + " " + Descripcion;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                if (sDescripcionOut.Equals(string.Empty))
                {
                    return psMetodo;
                }
            }
            catch (System.Exception)
            {
                sDescripcionOut = string.Empty;
            }
            return sDescripcionOut;
        }

        private string fnTextoImporte(string psValor, string psMoneda)
        {
            CultureInfo languaje;
            Numalet parser = new Numalet();
            parser.LetraCapital = true;

            switch (psMoneda)
            {
                case "MXN":
                    parser.TipoMoneda = Numalet.Moneda.Peso;
                    break;
                case "USD":
                    parser.TipoMoneda = Numalet.Moneda.Dolar;
                    break;
                case "XEU":
                    parser.TipoMoneda = Numalet.Moneda.Euro;
                    break;
                case "EUR":
                    parser.TipoMoneda = Numalet.Moneda.Euro;
                    break;
            }

            languaje = new CultureInfo("es-Mx");

            return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
        }

        private void CellCantidadLetra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CeldaTotal = (XRTableCell)sender;

            if (CeldaTotal.Text != "")
            {
                (sender as XRTableCell).Text = fnTextoImporte(CeldaTotal.Text, GetCurrentColumnValue("Moneda").ToString()).ToUpper();
            }
        }

        private void xrSubRepImpLocalesTras_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrSubRepImpLocalesTras.ReportSource.Report.XmlDataPath = this.XmlDataPath;
            ((XRSubreport)sender).ReportSource.Parameters["pColor"].Value = Parameters["pColor"].Value;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string sMoneda = GetCurrentColumnValue("Moneda").ToString();
            string sFormatString = "{0:" + (sMoneda == "EUR" || sMoneda == "XEU" ? "€ " : "$ ") + "#,###.00}";

            CellSubTotalValue.DataBindings["Text"].FormatString = sFormatString;
            LabelTotalValue.DataBindings["Text"].FormatString = sFormatString;
            CellDescuentoValue.DataBindings["Text"].FormatString = sFormatString;
            CellIVAValue.DataBindings["Text"].FormatString = sFormatString;
            CellIEPSValue.DataBindings["Text"].FormatString = sFormatString;
            CellRetencionISRValue.DataBindings["Text"].FormatString = sFormatString;
            CellRetencionIVAValue.DataBindings["Text"].FormatString = sFormatString;
            CellValorUnitario.DataBindings["Text"].FormatString = sFormatString;
            CellImporte.DataBindings["Text"].FormatString = sFormatString;
        }

        private void xrSubRepImpLocalesRet_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrSubRepImpLocalesRet.ReportSource.Report.XmlDataPath = this.XmlDataPath;
            ((XRSubreport)sender).ReportSource.Parameters["pColor"].Value = Parameters["pColor"].Value;
        }

        private void CellIVATit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CeldaIvaTitulo = (XRTableCell)sender;


            if (CeldaIvaTitulo.Text != "")
            {
                (sender as XRTableCell).Text = "IVA " + Convert.ToDouble(CeldaIvaTitulo.Text).ToString("F2") + "%";
            }
        }

        private void CellIEPSTit_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CeldaIEPSTitulo = (XRTableCell)sender;

            if (CeldaIEPSTitulo.Text != "")
            {
                (sender as XRTableCell).Text = "IEPS " + Convert.ToDouble(CeldaIEPSTitulo.Text).ToString("F2") + "%";
            }
        }

        private void xrLineFooterConceptos_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (e.PageIndex == e.PageCount - 1)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void rptPlantillaGenerica_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            pnlHeader.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            tblConceptoHeader.BackColor = Color.FromName(Parameters["pColor"].Value.ToString());
            xrCrossBandLineLeft.ForeColor = Color.FromName(Parameters["pColor"].Value.ToString());
            xrCrossBandLineRight.ForeColor = Color.FromName(Parameters["pColor"].Value.ToString());
            xrLineFooterConceptos.ForeColor = Color.FromName(Parameters["pColor"].Value.ToString());
            xrtblCellCadenaOriginal.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellSelloTit.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellTitUUID.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellUUID.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellTipoDoc.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            xrBarCodeQRCFDI.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellTotalLetraTit.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellSubTotalTit.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellSubTotalValue.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellCantidadLetra.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellFormaDePagoTit.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellFormaDePagoVal.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellMetodoDePagoTit.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellMetodoDePago.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellNumeroDeCuentaTit.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            CellNumCtaPago.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            xrTotalesPlace.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            LabelTotalTit.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            LabelTotalValue.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
            xrCrossBandLine1.ForeColor = Color.FromName(Parameters["pColor"].Value.ToString());
            if (Parameters["pRutaXMLTerceros"].Value.ToString() == string.Empty)
            {
                xrSubreportTerceros.Visible = false;
            }
            else
            {
                xrSubreportTerceros.ReportSource.Report.XmlDataPath = Parameters["pRutaXMLTerceros"].Value.ToString();
                xrSubreportTerceros.ReportSource.Parameters["pMoneda"].Value = Parameters["pRutaXMLTerceros"].Value.ToString();
            }


        }                                                                                              
    }
}


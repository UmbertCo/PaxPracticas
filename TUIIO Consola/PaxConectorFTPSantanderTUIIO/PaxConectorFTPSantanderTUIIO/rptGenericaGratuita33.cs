using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Threading;
using System.Xml.XPath;
using System.Xml;
using System.Globalization;

namespace PAXConectorFTPGTCFDI33
{
    public partial class rptGenericaGratuita33 : DevExpress.XtraReports.UI.XtraReport
    {
        public rptGenericaGratuita33()
        {
            InitializeComponent();
            InitializeCulture();
        }

        public void InitializeCulture()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
        }

        private void xrBarCodeQRGratuita_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRBarCode BarCodeQRCFDI = (XRBarCode)sender;
            string sCadenaCodigo = string.Empty;

            sCadenaCodigo = Parameters["pLinkSAT"].Value.ToString()
                            + "&id=" + ((XRTableCell)FindControl("CellUUID", true)).Text.Trim()
                            + "?re=" + ((XRTableCell)FindControl("CellRFCEmisor", true)).Text.Replace("RFC:", "").Trim()
                            + "&rr=" + ((XRTableCell)FindControl("CellRFCReceptor", true)).Text.Replace("RFC:", "").Trim()
                            + "&tt=" + Convert.ToDouble(GetCurrentColumnValue("Total").ToString())
                            + "&fe=" + System.Text.RegularExpressions.Regex.Match(((XRTableCell)FindControl("CellSelloDigitalEmisor", true)).Text.Trim(), @"(.{8})\s*$").ToString();

            (sender as XRBarCode).Text = sCadenaCodigo;
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string sMoneda = GetCurrentColumnValue("Moneda").ToString();
            string sFormatString = "{0:" + (sMoneda == "EUR" || sMoneda == "XEU" ? "€ " : "$ ") + "#,##0.00}";

            CellImpSubtotal.DataBindings["Text"].FormatString = sFormatString;
            CellImpTotal.DataBindings["Text"].FormatString = sFormatString;
            CellImpIvaTraslado.DataBindings["Text"].FormatString = sFormatString;
            CellImpIepsTraslado.DataBindings["Text"].FormatString = sFormatString;
            CellImpIsrRetencion.DataBindings["Text"].FormatString = sFormatString;
            CellImpIvaRetencion.DataBindings["Text"].FormatString = sFormatString;
            xrImporte.DataBindings["Text"].FormatString = sFormatString;
            xrPrecioUnitario.DataBindings["Text"].FormatString = sFormatString;
            xrCantidad.DataBindings["Text"].FormatString = "{0:#,##0.00}";
        }

        private void CellTotalconLetra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CellTotalLetra = (XRTableCell)sender;

            if (CellTotalLetra.Text != "")
            {
                (sender as XRTableCell).Text = fnTextoImporte(CellTotalLetra.Text, GetCurrentColumnValue("Moneda").ToString()).ToUpper();
            }
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

        private void CellMetodoPago_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CeldaMetodoPago = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(CeldaMetodoPago.Text))
            {
                (sender as XRTableCell).Text = fnCompareValue("MetodoPago.xml", "mp:ListaMetodosPago", "/mp:ListaMetodosPago/mp:MetodosPago", "mp:MetodoPago", CeldaMetodoPago.Text);
            }
        }

        private void CellRegimenEmisor_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CellRegimenEmisor = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(CellRegimenEmisor.Text))
            {
                (sender as XRTableCell).Text = fnCompareValue("RegimenFiscal.xml", "mp:ListaRegimenesFiscales", "/mp:ListaRegimenesFiscales/mp:RegimenesFiscales", "mp:RegimenFiscal", CellRegimenEmisor.Text);
            }
        }

        private void CellFormaPago_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CellFormaPago = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(CellFormaPago.Text))
            {
                (sender as XRTableCell).Text = fnCompareValue("FormaPago.xml", "mp:ListaFormasPago", "/mp:ListaFormasPago/mp:FormasPago", "mp:FormaPago", CellFormaPago.Text);
            }
        }

        private void CellUsoCFDI_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CellUsoCFDI = (XRTableCell)sender;
            if (CellUsoCFDI.Text != "") 
            {  
                if (!string.IsNullOrEmpty(CellUsoCFDI.Text))
                {
                    (sender as XRTableCell).Text = "Uso CFDI: " + fnCompareValue("UsoCFDI.xml", "mp:ListaUsosCFDI", "/mp:ListaUsosCFDI/mp:UsosCFDI", "mp:UsoCFDI", CellUsoCFDI.Text);
                }
            } 
        }

        private void CellTipoRelacion_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CellTipoRelacion = (XRTableCell)sender;
            if (!string.IsNullOrEmpty(CellTipoRelacion.Text))
            {
                if (!string.IsNullOrEmpty(CellTipoRelacion.Text))
                {
                    (sender as XRTableCell).Text = fnCompareValue("TipoRelacion.xml", "mp:ListaTiposRelacion", "/mp:ListaTiposRelacion/mp:TiposRelacion", "mp:TipoRelacion", CellTipoRelacion.Text);
                }
            }
        }

        public string fnCompareValue(string psNameXML, string psNamespace, string sFatherNodeName, string sSonNodeName, string psValue)
        {
            string sDescripcionOut = string.Empty;

            try
            {
                XmlDocument xmlCatalogo = new XmlDocument();
                xmlCatalogo.Load(AppDomain.CurrentDomain.BaseDirectory + @"\XML\" + psNameXML);

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlCatalogo.NameTable);
                nsmComprobante.AddNamespace("mp", psNamespace);

                XPathNavigator navComprobante = xmlCatalogo.CreateNavigator();
                XPathNodeIterator navDetalles = navComprobante.Select(sFatherNodeName, nsmComprobante);

                while (navDetalles.MoveNext())
                {
                    XPathNavigator nodenavigator = navDetalles.Current; 
                    if (nodenavigator.HasChildren)
                    {
                        XPathNodeIterator navDetallesFather = nodenavigator.Select(sSonNodeName, nsmComprobante);
                        while (navDetallesFather.MoveNext())
                        {
                            XPathNavigator navDetalle = navDetallesFather.Current;
                            string Clave = navDetalle.SelectSingleNode("@Clave", nsmComprobante).Value;
                            string Descripcion = navDetalle.SelectSingleNode("@Descripcion", nsmComprobante).Value;
                            string Estatus = navDetalle.SelectSingleNode("@Estatus", nsmComprobante).Value;
                            if (Estatus.Equals("A"))
                            {
                                if (psValue.Equals(Clave))
                                {
                                    sDescripcionOut = Clave + "(" + Descripcion + ")";
                                }
                            }
                        } 
                    }
                } 

                if (sDescripcionOut.Equals(string.Empty))
                {
                    return psValue;
                }
            }
            catch (System.Exception)
            {
                sDescripcionOut = string.Empty;
            }
            return sDescripcionOut;
        }

        private void xrLine1_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (e.PageIndex == e.PageCount - 1)
                e.Cancel = true;
            else
                e.Cancel = false;
        }
    }
}

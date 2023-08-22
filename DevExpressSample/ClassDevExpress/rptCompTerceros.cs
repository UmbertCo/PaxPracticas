using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace ClassDevExpress
{
    public partial class rptCompTerceros : DevExpress.XtraReports.UI.XtraReport
    {
        public rptCompTerceros()
        {
            InitializeComponent();
        }

  



        private void DetailReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string sMoneda = Parameters["pMoneda"].Value.ToString();
            string sFormatString = "{0:" + (sMoneda == "EUR" || sMoneda == "XEU" ? "€ " : "$ ") + "#,###.00}";

            CellIVAValue.DataBindings["Text"].FormatString = sFormatString;
            CellIEPSValue.DataBindings["Text"].FormatString = sFormatString;
            CellRetencionISRValue.DataBindings["Text"].FormatString = sFormatString;
            CellRetencionIVAValue.DataBindings["Text"].FormatString = sFormatString;
        }

        private void CellAcumulado_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string sRowIVARet = CellRetencionIVATit.Text.Trim() + " " + CellRetencionIVAValue.Text.Trim();
            string sRowISRRet = CellRetencionISRTit.Text.Trim() + " " + CellRetencionISRValue.Text.Trim();
            string sRowIVA = (CellIVATit.Text.Trim() != "" ? "IVA Tras " + Convert.ToDouble(CellIVATit.Text.Trim()).ToString("F2") + "%:" : "") + " " + CellIVAValue.Text.Trim();
            string sRowIEPS = (CellIEPSTit.Text.Trim() != "" ? "IEPS " + Convert.ToDouble(CellIEPSTit.Text.Trim()).ToString("F2") + "%:" : "") + " " + CellIEPSValue.Text.Trim();
 
            (sender as XRTableCell).Text = sRowIVARet.Trim() + " " + sRowISRRet.Trim() + " " + sRowIVA.Trim() + " " + sRowIEPS.Trim();
            (sender as XRTableCell).Text = (sender as XRTableCell).Text.Trim();
        }

        private void tblImpLocalTra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            RowIvaTraslados.Visible =  false;
            RowIEPSTraslados.Visible =  false;
            RowISRRetenciones.Visible =  false;
            RowIVARetenciones.Visible = false;
        }

    }
}

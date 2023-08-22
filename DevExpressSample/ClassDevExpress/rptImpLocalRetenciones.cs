using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace ClassDevExpress
{
    public partial class rptImpLocalRetenciones : DevExpress.XtraReports.UI.XtraReport
    {
        public rptImpLocalRetenciones()
        {
            InitializeComponent();
        }

        private void DetailReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {  
            string sMoneda = GetCurrentColumnValue("Moneda").ToString();
            string sFormatString = "{0:" + (sMoneda == "EUR" || sMoneda == "XEU" ? "€ " : "$ ") + "#,###.00}";   
            CellImpLocalRetValue.DataBindings["Text"].FormatString = sFormatString;

        }

        private void rptImpLocalRetenciones_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            CellImpLocalRetTit.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
        }

    }
}

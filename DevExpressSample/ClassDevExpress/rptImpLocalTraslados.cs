using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace ClassDevExpress
{
    public partial class rptImpLocalTraslados : DevExpress.XtraReports.UI.XtraReport
    {
        public rptImpLocalTraslados()
        {
            InitializeComponent();
        }

        private void DetailReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string sMoneda = GetCurrentColumnValue("Moneda").ToString();
            string sFormatString = "{0:" + (sMoneda == "EUR" || sMoneda == "XEU" ? "€ " : "$ ") + "#,###.00}";  
            CellImpLocalTraValue.DataBindings["Text"].FormatString = sFormatString;   
        }

        private void rptImpLocalTraslados_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            CellImpLocalTraTit.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
        }

    }
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Globalization;
using System.Text;


namespace DevExpressSample
{
    public partial class rptProfecoFactura : DevExpress.XtraReports.UI.XtraReport
    {
        public rptProfecoFactura()
        {
            InitializeComponent();
            //xrTableCell49.DataBindings["Text"].FormatString = "{0:€ #,###.00}";
        }

        private void xrtblCellCantidadLetra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CeldaTotal = (XRTableCell)sender;
            if (CeldaTotal.Text != "")
            {
                (sender as XRTableCell).Text = fnTextoImporte(CeldaTotal.Text, "MXN").ToUpper();
            }
        }

        private string fnTextoImporte(string psValor, string psMoneda)
        {
            CultureInfo languaje;
            NumaletNomina parser = new NumaletNomina();
            parser.LetraCapital = true;

            switch (psMoneda)
            {
                case "MXN":
                    parser.TipoMoneda = NumaletNomina.Moneda.Peso;
                    break;
                case "USD":
                    parser.TipoMoneda = NumaletNomina.Moneda.Dolar;
                    break;
                case "XEU":
                    parser.TipoMoneda = NumaletNomina.Moneda.Euro;
                    break;
            }

            languaje = new CultureInfo("es-Mx");

            return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
        }
    }


}

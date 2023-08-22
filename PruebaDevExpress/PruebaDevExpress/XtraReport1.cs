using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;

using System.Drawing;
using System.Drawing.Imaging;

namespace PruebaDevExpress
{
    public partial class XtraReport1 : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReport1()
        {
            InitializeComponent();
        }

        private void XtraReport1_DesignerLoaded(object sender, DevExpress.XtraReports.UserDesigner.DesignerLoadedEventArgs e)
        {
           // DataTable dt = new DataTable();

           // dt.Columns.Add("asd");
           // dt.Columns.Add("asd2");

           // dt.Rows.Add("asd", "asd");
           // dt.Rows.Add("asd", "asd");

           //tblDetalleFactura.

            
        }

        private void cfFecha_GetValue(object sender, GetValueEventArgs e)
        {
            XRLabel lbl = sender as XRLabel;

            lbl.Text = "asd";
        }

        private void xrtblCellCantidadLetra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell CeldaTotal = (XRTableCell)sender;
            if (CeldaTotal.Text != "")
            {
                (sender as XRTableCell).Text = clsComun.fnTextoImporte(CeldaTotal.Text, "MXN").ToUpper();
            }
        }


        //private void xrBarCodeQRProfeco_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    XRBarCode QR = (sender as XRBarCode);

        //    QR.Text = clsComun.GenerarCodigoBidimensional(this.XmlDataPath);
        //}

        //private void xrTableCell14_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    XRTableCell CeldaTotal = (XRTableCell)sender;
        //    if (CeldaTotal.Text != "")
        //    {
        //        XmlDocument xdDoc = new XmlDocument();

        //        xdDoc.Load(this.XmlDataPath);

        //        (sender as XRTableCell).Text = clsComun.fnConstruirCadenaTimbrado(xdDoc);
        //    }
        //}

    }
}

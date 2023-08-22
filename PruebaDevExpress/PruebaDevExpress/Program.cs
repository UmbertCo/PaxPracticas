using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.Xml;
using System.Reflection;
using System.IO;


namespace PruebaDevExpress
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            

            OpenFileDialog ofd = new OpenFileDialog();
            String sCadenaOriginal = "";
            XmlDocument xdDoc = new XmlDocument();
            XtraReport1 xrReporte = new XtraReport1();
            PdfExportOptions pdfOptions = xrReporte.ExportOptions.Pdf;

            pdfOptions.Compressed = true;
            pdfOptions.ConvertImagesToJpeg = false;
            pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
            pdfOptions.DocumentOptions.Title = "CFDI";

            ofd.Filter = "(*.xml)|*.xml";

            if(ofd.ShowDialog()!= DialogResult.OK)
                return ;

            

            xrReporte.XmlDataPath = ofd.FileName;

            

            xdDoc.Load(ofd.FileName);


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdDoc.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try { sCadenaOriginal = "|" + clsComun.fnConstruirCadenaTimbrado(xdDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator()) + "||"; }
            catch { }

            XRTableCell xrtblCellCadenaOriginal = (XRTableCell)xrReporte.FindControl("xrtCadOri", true);
            xrtblCellCadenaOriginal.Text = sCadenaOriginal;

            XRBarCode QRCodeProfeco = (XRBarCode)xrReporte.FindControl("xrBarCodeQRProfeco", true);
            QRCodeProfeco.Text = clsComun.GenerarCodigoBidimensional(xdDoc);




            xrReporte.ExportToPdf(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "PruebaXml.pdf");

            

        

            
        
        }


    }
}

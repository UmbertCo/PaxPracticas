using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

namespace XtraReportsPractica
{
    public partial class Form1 : Form
    {

        XmlDocument xmlUnion = new XmlDocument();  

        public Form1()
        {
            InitializeComponent();
        }

        public static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
        {
            string sCadenaOriginal = string.Empty;
            try
            {
                MemoryStream ms = new MemoryStream();
                XslCompiledTransform trans = new XslCompiledTransform();
                trans.Load(typeof(Timbrado.V3.TFDXSLT));
                XsltArgumentList args = new XsltArgumentList();
                trans.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                sCadenaOriginal = sr.ReadToEnd();
            }
            catch (Exception)
            {
                //LOGO DE ERROR
            }

            return sCadenaOriginal;
        }

        private string GenerarCodigoBidimensional()
        {
            XmlNamespaceManager nsm = new XmlNamespaceManager(xmlUnion.NameTable);
            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navCodigo = xmlUnion.CreateNavigator();

            string sCadenaCodigo = "?re=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsm).Value
                                + "&rr=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsm).Value
                                + "&tt=" + string.Format("{0:0000000000.000000}", navCodigo.SelectSingleNode("/cfdi:Comprobante/@total", nsm).ValueAsDouble)
                                + "&id=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value.ToUpper();

            if (sCadenaCodigo.Length < 93 || sCadenaCodigo.Length > 95)
                throw new Exception("Los datos para la cadena del código CBB no cumplen con la especificación de hacienda");

            return sCadenaCodigo;
        }

        private void btnMostrarReporte_Click(object sender, EventArgs e)
        {
            XtraReport1 report = new XtraReport1();
            xmlUnion.Load("C:\\Archivos\\PRACTICA XTRA REPORTS\\P11.xml");
            report.XmlDataPath = "C:\\Archivos\\PRACTICA XTRA REPORTS\\P11.xml";

            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

            pdfOptions.Compressed = true;
            pdfOptions.ConvertImagesToJpeg = false;

            string sCadenaOriginal = string.Empty;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlUnion.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xmlUnion.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
            catch { }

            XRTableCell xrtblCellCadenaOriginal = (XRTableCell)report.FindControl("xrTableCellCadenaOriginal", true);
            xrtblCellCadenaOriginal.Text = sCadenaOriginal;

            XRBarCode QRCodeProfeco = (XRBarCode)report.FindControl("xrBarCodeQR", true);
            QRCodeProfeco.Text = GenerarCodigoBidimensional();

            pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
            pdfOptions.DocumentOptions.Title = "CFDI";

            report.ExportToPdf("C:\\Archivos\\PRACTICA XTRA REPORTS\\P11.pdf");
            report.ShowPreview();
        }
    }
}

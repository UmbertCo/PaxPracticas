using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Xml.Xsl;

namespace PracticaReporte
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        XmlDocument xComprobante = new XmlDocument();

        private void button1_Click(object sender, EventArgs e)
        {

            if (ofdAbrirXml.ShowDialog() == DialogResult.OK)
            {
                rptNomina reporte = new rptNomina();
                xComprobante.Load(ofdAbrirXml.FileName);
                reporte.XmlDataPath = ofdAbrirXml.FileName.ToString();
                PdfExportOptions pdfOptions = reporte.ExportOptions.Pdf;

                pdfOptions.Compressed = true;
                pdfOptions.ConvertImagesToJpeg = false;

                string sCadenaOriginal = string.Empty;

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xComprobante.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
                catch { }

                XRTableCell tblCellCadenaTimbre = (XRTableCell)reporte.FindControl("xrtblCellCadenaOriginal", true);
                tblCellCadenaTimbre.Text = sCadenaOriginal;

                XRBarCode QRCode = (XRBarCode)reporte.FindControl("xrBarCodeQRProfeco", true);
                QRCode.Text = GenerarCodigoBidimensional();

                pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
                pdfOptions.DocumentOptions.Title = "CFDI";

                reporte.ExportToPdf(System.IO.Directory.GetParent(ofdAbrirXml.FileName.ToString()).Parent.FullName + @"\Nomina.pdf");
                reporte.ShowPreview();
            }
        }

        public static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
        {
            string sCadenaOriginal = string.Empty;
            try
            {
                MemoryStream ms = new MemoryStream();
                XslCompiledTransform trans = new XslCompiledTransform();
                trans.Load(typeof(TimFis));
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
            XmlNamespaceManager nsm = new XmlNamespaceManager(xComprobante.NameTable);
            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navCodigo = xComprobante.CreateNavigator();

            string sCadenaCodigo = "?re=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsm).Value
                                + "&rr=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsm).Value
                                + "&tt=" + string.Format("{0:0000000000.000000}", navCodigo.SelectSingleNode("/cfdi:Comprobante/@total", nsm).ValueAsDouble)
                                + "&id=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value.ToUpper();

            if (sCadenaCodigo.Length < 93 || sCadenaCodigo.Length > 95)
                throw new Exception("Los datos para la cadena del código CBB no cumplen con la especificación de hacienda");

            return sCadenaCodigo;
        }
    }
}

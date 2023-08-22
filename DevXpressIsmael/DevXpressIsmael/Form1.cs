using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Windows.Forms;

namespace DevXpressIsmael
{
    public partial class frmEjemploDevXpress : Form
    {
        XmlDocument xdComprobante = new XmlDocument();

        public frmEjemploDevXpress()
        {
            InitializeComponent();
        }

        private void btnVerPDF_Click(object sender, EventArgs e)
        {
            XtraReport1 xrEjemplo = new XtraReport1();
            PdfExportOptions peoOpciones = new PdfExportOptions();
            string sCadenaOriginal = string.Empty;
            try
            {
                OpenFileDialog ofdComprobante = new OpenFileDialog();
                ofdComprobante.Filter = "XML Files|*.xml";
                //ofdComprobante.DefaultExt = "*.xml";
                ofdComprobante.AddExtension = true;                
                ofdComprobante.ShowDialog();

                if (string.IsNullOrEmpty(ofdComprobante.FileName))
                    return;

                xdComprobante.Load(ofdComprobante.FileName);

                xrEjemplo.DataSource = xdComprobante;
                //xrEjemplo.XmlDataPath = ofdComprobante.FileName;

                peoOpciones.Compressed = true;
                peoOpciones.ConvertImagesToJpeg = false;

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdComprobante.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
                catch { }

                XRTableCell xrtblCellCadenaOriginal = (XRTableCell)xrEjemplo.FindControl("xrtblCellCadenaOriginal", true);
                xrtblCellCadenaOriginal.Text = sCadenaOriginal;


                XRBarCode QRCodeProfeco = (XRBarCode)xrEjemplo.FindControl("xrBarCodeQRProfeco", true);
                QRCodeProfeco.Text = fnGenerarCodigoBidimensional();

                peoOpciones.DocumentOptions.Author = "CORPUS Facturacion";
                peoOpciones.DocumentOptions.Title = "CFDI";

                xrEjemplo.ExportToPdf(Path.GetDirectoryName(ofdComprobante.FileName) + "" + Path.GetFileNameWithoutExtension(ofdComprobante.FileName) + ".pdf");
                xrEjemplo.ShowPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
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

        private string fnGenerarCodigoBidimensional()
        {
            XmlNamespaceManager nsm = new XmlNamespaceManager(xdComprobante.NameTable);
            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navCodigo = xdComprobante.CreateNavigator();

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

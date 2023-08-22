using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
//using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using DevExpress.XtraCharts;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml.Xsl;

namespace DevExpressSample
{
    public partial class frmDevExpressReporting : Form
    {
        XmlDocument xmlUnion = new XmlDocument();           

        public frmDevExpressReporting()
        {
            InitializeComponent();
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

        //********************************************************************************************************************************************//

        private void btnCC_Click(object sender, EventArgs e)
        {
            rptCC report = new rptCC();
            xmlUnion.Load(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\T_CC.xml");
            report.XmlDataPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\T_CC.xml";

            string sCadenaOriginal = string.Empty;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlUnion.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xmlUnion.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
            catch { }

            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

            pdfOptions.Compressed = true;
            pdfOptions.ConvertImagesToJpeg = false;

            XRLabel lblCadenaTimbre = (XRLabel)report.FindControl("xrlblCadenaOriginalTimbre", true);

            lblCadenaTimbre.Text = sCadenaOriginal;

            XRBarCode QRCode = (XRBarCode)report.FindControl("xrBarCodeQR", true);
            QRCode.Text = GenerarCodigoBidimensional();

            XRChart chart = (XRChart)report.FindControl("xrChartUnion", true);

            Series series = new Series("SeriesMovimientos", ViewType.Bar);
            chart.Series.Add(series);
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            series.ShowInLegend = false;
            series.DataSource = CreateChartDataEC();
            series.ArgumentScaleType = ScaleType.Qualitative;
            series.ArgumentDataMember = "Saldos";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            series.View.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(140)))), ((int)(((byte)(241)))));
            ((SideBySideBarSeriesView)series.View).FillStyle.FillMode = FillMode.Solid;

            XRPictureBox LogoBanner = (XRPictureBox)report.FindControl("xrpicBanner", true);
            LogoBanner.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\encabezado.png"; ;

            XRPictureBox LogoPie = (XRPictureBox)report.FindControl("xrpicPie", true);
            LogoPie.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\pie.png"; ;


            XRPictureBox Logo = (XRPictureBox)report.FindControl("xrpicLogo", true);
            XRPictureBox Logo2 = (XRPictureBox)report.FindControl("xrpicAkalaHeader", true);

            Logo.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\AkalaLogo.png";
            Logo2.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\AkalaLogo.png";

            pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
            pdfOptions.DocumentOptions.Title = "CFDI";

            report.ExportToPdf(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\TestCC.pdf");
            report.ShowPreview();

        }

        private DataTable CreateChartDataEC()
        {
            string[] strTitles = { "Saldo Inicial", "Abonos (+)", "Retiros (-)", "Comisiones (-)", "Intereses a favor (+)", "Otros Cargos (-)", "Saldo Final" };
            string[] strAttribute = { "GraficoSaldoInicial", "GraficoAbonos", "GraficoRetiros", "GraficoComisiones", "GraficoInteresesFavor", "GraficoOtrosCargos", "GraficoSaldoFinal" };
            string[] strAttributeTitles = { "SaldoAnterior", "Abonos", "Cargos", "ComisionesCobradas", "InteresPagados", "ISRRetenido", "SaldoCierre" };

            double[] yValues = { 0, 0, 0, 0, 0, 0, 0 };

            double[] xValues = { 0, 0, 0, 0, 0, 0, 0 };

            DataTable table = new DataTable("tblSaldos");

            table.Columns.Add("Saldos", typeof(string));
            table.Columns.Add("Value", typeof(double));

            int i = 0;

            foreach (string value in strAttribute)
            {
                yValues[i] = Convert.ToDouble(ValueAttributeEC(value));

                if (strAttributeTitles[i] != "ISRRetenido")
                {
                    xValues[i] = Convert.ToDouble(ValueAttributeEC(strAttributeTitles[i]));
                }
                else
                {
                    xValues[i] = Convert.ToDouble(ValueAttributeEC(strAttributeTitles[i])) + Convert.ToDouble(ValueAttributeEC("IVAComisiones"));
                }

                DataRow row = null;
                row = table.NewRow();
                row["Saldos"] = string.Concat(strTitles[i], Environment.NewLine, Environment.NewLine, yValues[i].ToString("n2", CultureInfo.CreateSpecificCulture("es-Mx")), "%", Environment.NewLine, Environment.NewLine, xValues[i].ToString("c2", CultureInfo.CreateSpecificCulture("es-Mx")));

                row["Value"] = yValues[i];
                table.Rows.Add(row);
                i++;
            }

            return table;
        }

        private string ValueAttributeEC(string strAttribute)
        {
            string strValue = "0";

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlUnion.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = xmlUnion.CreateNavigator();

            try { strValue = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@" + strAttribute, nsmComprobante).Value; }
            catch { }

            return strValue;
        }

        //********************************************************************************************************************************************//

        private void btnPR_Click(object sender, EventArgs e)
        {
            rptPR report = new rptPR();
            xmlUnion.Load(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\T_PR.xml");
            report.XmlDataPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\T_PR.xml";

            string sCadenaOriginal = string.Empty;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlUnion.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xmlUnion.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
            catch { }

            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

            pdfOptions.Compressed = true;
             pdfOptions.ConvertImagesToJpeg = false;

            XRLabel lblCadenaTimbre = (XRLabel)report.FindControl("xrlblCadenaOriginalTimbre", true);

            lblCadenaTimbre.Text = sCadenaOriginal;

            XRBarCode QRCode = (XRBarCode)report.FindControl("xrBarCodeQR", true);
            QRCode.Text = GenerarCodigoBidimensional();

            XRChart chart = (XRChart)report.FindControl("xrChartUnion", true);


            Series series = new Series("SeriesMovimientos", ViewType.Bar);
            chart.Series.Add(series);
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            series.ShowInLegend = false;
            series.DataSource = CreateChartDataEP();
            series.ArgumentScaleType = ScaleType.Qualitative;
            series.ArgumentDataMember = "Saldos";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            series.View.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(140)))), ((int)(((byte)(241)))));
            ((SideBySideBarSeriesView)series.View).FillStyle.FillMode = FillMode.Solid;


            XRPictureBox LogoBanner = (XRPictureBox)report.FindControl("xrpicBanner", true);
            LogoBanner.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\encabezado.png"; ;

            XRPictureBox LogoPie = (XRPictureBox)report.FindControl("xrpicPie", true);
            LogoPie.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\pie.png"; ;


            XRPictureBox Logo = (XRPictureBox)report.FindControl("xrpicLogo", true);
            XRPictureBox Logo2 = (XRPictureBox)report.FindControl("xrpicAkalaHeader", true);

            Logo.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\AkalaLogo.png";
            Logo2.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\AkalaLogo.png";

            pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
            pdfOptions.DocumentOptions.Title = "CFDI";

            report.ExportToPdf(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\TestPR.pdf");
            report.ShowPreview();

        }

        private DataTable CreateChartDataEP()
        {
            string[] strTitles = { "Abono", "Cargos", "Comisiones", "I.V.A", "Intereses", "Moratorios", "Saldo" };
            string[] strAttribute = { "GraficoAbono", "GraficoCargo", "GraficoComision", "GraficoIVA", "GraficoInteres", "GraficoMora", "GraficoSaldo" };
            string[] strAttributeTitles = { "TotalAbonos", "TotalCargos", "TotalComisiones", "TotalIVA", "TotalIntereses", "TotalMoratorios", "Saldo" };

            double[] yValues = { 0, 0, 0, 0, 0, 0, 0 };

            double[] xValues = { 0, 0, 0, 0, 0, 0, 0 };

            DataTable table = new DataTable("tblSaldos");

            table.Columns.Add("Saldos", typeof(string));
            table.Columns.Add("Value", typeof(double));

            int i = 0;

            foreach (string value in strAttribute)
            {
                yValues[i] = Convert.ToDouble(ValueAttributeEP(value));
                xValues[i] = Convert.ToDouble(ValueAttributeEP(strAttributeTitles[i]));

                DataRow row = null;
                row = table.NewRow();

                row["Saldos"] = string.Concat(strTitles[i], Environment.NewLine, Environment.NewLine, yValues[i].ToString("n2", CultureInfo.CreateSpecificCulture("es-Mx")), "%", Environment.NewLine, Environment.NewLine, xValues[i].ToString("c2", CultureInfo.CreateSpecificCulture("es-Mx")));
                row["Value"] = yValues[i];
                table.Rows.Add(row);
                i++;
            }

            return table;
        }

        private string ValueAttributeEP(string strAttribute)
        {
            string strValue = "0";

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlUnion.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = xmlUnion.CreateNavigator();

            try { strValue = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EP/@" + strAttribute, nsmComprobante).Value; }
            catch { }

            return strValue;
        }

        //********************************************************************************************************************************************//

        private void btnDP_Click(object sender, EventArgs e)
        {
            rptDP report = new rptDP();
            xmlUnion.Load(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\T_DP.xml");
            report.XmlDataPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\T_DP.xml";

            string sCadenaOriginal = string.Empty;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlUnion.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xmlUnion.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
            catch { }

            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

            pdfOptions.Compressed = true;
            pdfOptions.ConvertImagesToJpeg = false;

            XRLabel lblCadenaTimbre = (XRLabel)report.FindControl("xrlblCadenaOriginalTimbre", true);

            lblCadenaTimbre.Text = sCadenaOriginal;

            XRBarCode QRCode = (XRBarCode)report.FindControl("xrBarCodeQR", true);
            QRCode.Text = GenerarCodigoBidimensional();

            XRChart chart = (XRChart)report.FindControl("xrChartUnion", true);


            Series series = new Series("SeriesMovimientos", ViewType.Bar);
            chart.Series.Add(series);
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            series.ShowInLegend = false;
            series.DataSource = CreateChartDataED();
            series.ArgumentScaleType = ScaleType.Qualitative;
            series.ArgumentDataMember = "Saldos";
            series.ValueScaleType = ScaleType.Numerical;
            series.ValueDataMembers.AddRange(new string[] { "Value" });
            series.View.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(140)))), ((int)(((byte)(241)))));
            ((SideBySideBarSeriesView)series.View).FillStyle.FillMode = FillMode.Solid;


            XRPictureBox LogoBanner = (XRPictureBox)report.FindControl("xrpicBanner", true);
            LogoBanner.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\encabezado.png"; ;

            XRPictureBox LogoPie = (XRPictureBox)report.FindControl("xrpicPie", true);
            LogoPie.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\pie.png"; ;


            XRPictureBox Logo = (XRPictureBox)report.FindControl("xrpicLogo", true);
            XRPictureBox Logo2 = (XRPictureBox)report.FindControl("xrpicAkalaHeader", true);

            Logo.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\AkalaLogo.png";
            Logo2.ImageUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\AkalaLogo.png";

            pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
            pdfOptions.DocumentOptions.Title = "CFDI";

            report.ExportToPdf(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\UnionProgreso\TestDP.pdf");
            report.ShowPreview();
        }

        private DataTable CreateChartDataED()
        {
            string[] strTitles = { "Saldo Anterior", "Abonos (+)", "Retiros (-)", "Comisiones (-)", "Intereses (+)", "Otros Cargos (-)", "Saldo Final" };
            string[] strAttribute = { "GraficoSaldoAnterior", "GraficoAbonos", "GraficoRetiros", "GraficoComisiones", "GraficoIntereses", "GraficoOtrosCargos", "GraficoSaldoFinal" };
            string[] strAttributeTitles = { "SaldoDisponible", "Abonos", "Cargos", "ComisionesCobradas", "InteresPagado", "IVAComisiones", "SaldoFinal" };

            double[] yValues = { 0, 0, 0, 0, 0, 0, 0 };

            double[] xValues = { 0, 0, 0, 0, 0, 0, 0 };

            DataTable table = new DataTable("tblSaldos");

            table.Columns.Add("Saldos", typeof(string));
            table.Columns.Add("Value", typeof(double));

            int i = 0;

            foreach (string value in strAttribute)
            {
                yValues[i] = Convert.ToDouble(ValueAttributeED(value));
                xValues[i] = Convert.ToDouble(ValueAttributeED(strAttributeTitles[i]));

                DataRow row = null;
                row = table.NewRow();

                row["Saldos"] = string.Concat(strTitles[i], Environment.NewLine, Environment.NewLine, yValues[i].ToString("n2", CultureInfo.CreateSpecificCulture("es-Mx")), "%", Environment.NewLine, Environment.NewLine, xValues[i].ToString("c2", CultureInfo.CreateSpecificCulture("es-Mx")));
                row["Value"] = yValues[i];
                table.Rows.Add(row);
                i++;
            }

            return table;
        }

        private string ValueAttributeED(string strAttribute)
        {
            string strValue = "0";

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlUnion.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = xmlUnion.CreateNavigator();

            try { strValue = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/ED/@" + strAttribute, nsmComprobante).Value; }
            catch { }

            return strValue;
        }

        //********************************************************************************************************************************************//

        private void btnProfecoNomina_Click(object sender, EventArgs e)
        {
            rptProfecoNomina report = new rptProfecoNomina();
            xmlUnion.Load(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\ProfecoNomina\Nomina.xml");
            report.XmlDataPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\ProfecoNomina\Nomina.xml";

            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

            pdfOptions.Compressed = true;
            pdfOptions.ConvertImagesToJpeg = false;

            string sCadenaOriginal = string.Empty;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlUnion.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xmlUnion.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
            catch { }

            XRTableCell tblCellCadenaTimbre = (XRTableCell)report.FindControl("xrtblCellCadenaOriginal", true);
            tblCellCadenaTimbre.Text = sCadenaOriginal;
            
            XRBarCode QRCodeProfeco = (XRBarCode)report.FindControl("xrBarCodeQRProfeco", true);
            QRCodeProfeco.Text = GenerarCodigoBidimensional();

            pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
            pdfOptions.DocumentOptions.Title = "CFDI";

            report.ExportToPdf(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\ProfecoNomina\Nomina.pdf");
            report.ShowPreview();
        }

        //********************************************************************************************************************************************//

        private void btnProfecoFactura_Click(object sender, EventArgs e)
        {
            rptProfecoFactura report = new rptProfecoFactura();
            xmlUnion.Load(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\ProfecoFactura\UnConcepto.xml");
            report.XmlDataPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\ProfecoFactura\UnConcepto.xml";

            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

            pdfOptions.Compressed = true;
            pdfOptions.ConvertImagesToJpeg = false;

            string sCadenaOriginal = string.Empty;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlUnion.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xmlUnion.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
            catch { }

            XRTableCell xrtblCellCadenaOriginal = (XRTableCell)report.FindControl("xrtblCellCadenaOriginal", true);
            xrtblCellCadenaOriginal.Text = sCadenaOriginal;

            XRBarCode QRCodeProfeco = (XRBarCode)report.FindControl("xrBarCodeQRProfeco", true);
            QRCodeProfeco.Text = GenerarCodigoBidimensional();

            pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
            pdfOptions.DocumentOptions.Title = "CFDI";

            report.ExportToPdf(System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.FullName + @"\ProfecoFactura\Factura.pdf");
            report.ShowPreview();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace PAXPlantillaNomina12CFDI33
{
    public partial class Preview : System.Web.UI.Page
    {
        rptNomina12 report = new rptNomina12();

        protected void Page_Load(object sender, EventArgs e)
        {

            XmlDocument xmlReport = new XmlDocument();

            xmlReport.Load(HttpRuntime.AppDomainAppPath + "/XML/NOMINA12CDF33.xml");

            report.XmlDataPath = HttpRuntime.AppDomainAppPath + "/XML/NOMINA12CDF33.xml";

            report.Parameters["pColor"].Value = "Navy";
            report.Parameters["pLinkSAT"].Value = "";

            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

            pdfOptions.Compressed = true;
            pdfOptions.ConvertImagesToJpeg = false;

            string sCadenaOriginal = string.Empty;
            string sUUID = string.Empty;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlReport.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xmlReport.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
            catch { }
            try { sUUID = xmlReport.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
            catch { }


            XRTableCell xrtblCellCadenaOriginal = (XRTableCell)report.FindControl("xrtblCellCadenaOriginal", true);
            xrtblCellCadenaOriginal.Text = sCadenaOriginal;


            pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
            pdfOptions.DocumentOptions.Title = "CFDI";

            report.Name = sUUID;
            Session["report"] = report;

            lnkPDF.NavigateUrl = "~/ReportOutput.aspx/" + sUUID;
        }


        public static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
        {
            string sCadenaOriginal = string.Empty;
            try
            {
                MemoryStream ms = new MemoryStream();
                XslCompiledTransform trans = new XslCompiledTransform();
                trans.Load(typeof(Timbrado.V3.TFD11XSLT));
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
    }
}
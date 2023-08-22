using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraPrinting;
using System.Xml;
using DevExpress.XtraReports.UI;
using System.Xml.XPath;
using System.IO;
using System.Xml.Xsl;
using System.Drawing.Printing;
using System.Net.Mail;
using System.Xml.Linq;
using System.Diagnostics;

public partial class _Default : System.Web.UI.Page
{
    XmlDocument xmlReport = new XmlDocument();

    protected void Page_Load(object sender, EventArgs e)
    {

        //ClassDevExpress.rptPlantillaGenerica report = new ClassDevExpress.rptPlantillaGenerica();
        //ClassDevExpress.rptPlantillaLogo report = new ClassDevExpress.rptPlantillaLogo();
        ClassDevExpress.rptNomina report = new ClassDevExpress.rptNomina();
        xmlReport.Load(HttpRuntime.AppDomainAppPath + "/XML/Nomina.xml");

        report.XmlDataPath = HttpRuntime.AppDomainAppPath + "/XML/Nomina.xml";

        report.Parameters["pTipoDocumento"].Value = "Recibo";
        report.Parameters["pColor"].Value = "";
        //report.Parameters["pRutaXMLTerceros"].Value = HttpRuntime.AppDomainAppPath + @"XML\SoloTerceros.xml";
        //report.Parameters["pRutaXMLTerceros"].Value = string.Empty;
                                                             
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

    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            // Create a new report.                
            ClassDevExpress.rptNomina report = new ClassDevExpress.rptNomina();

            xmlReport.Load(HttpRuntime.AppDomainAppPath + "/XML/Nomina.xml");
            report.XmlDataPath = HttpRuntime.AppDomainAppPath + "/XML/Nomina.xml";

            report.Parameters["pTipoDocumento"].Value = "Recibo";
            report.Parameters["pColor"].Value = "Navy";

            PdfExportOptions pdfOptions = report.ExportOptions.Pdf;
            EmailOptions mailOptions = report.ExportOptions.Email;
           

            pdfOptions.Compressed = true;
            pdfOptions.ConvertImagesToJpeg = false;

            mailOptions.RecipientAddress ="cesar.negrete@paxfacturacion.com";
            mailOptions.RecipientName = "Cesar Negrete Villa";
  

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


            MemoryStream mem = new MemoryStream();
            report.ExportToPdf(mem); 

            mem.Seek(0, System.IO.SeekOrigin.Begin);
            Attachment att = new Attachment(mem, report.Name + ".pdf", "application/pdf");

            MailMessage mail = new MailMessage();
            mail.Attachments.Add(att);

            mail.From = new MailAddress("someone@somewhere.com", "Someone");
            mail.To.Add(new MailAddress(mailOptions.RecipientAddress, mailOptions.RecipientName));  

            mail.Subject = report.ExportOptions.Email.Subject;
            mail.Body = "This is a test e-mail message sent by an application.";


            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new System.Net.NetworkCredential("noreply@corpuscfdi.com.mx", "F4cturax10n");
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mail);

            mem.Close();
            mem.Dispose();
            smtp.Dispose();
            att.Dispose();
        }
        catch (Exception ex)
        {

        }
    }
}
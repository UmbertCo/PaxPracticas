using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraReports.UI;
using System.IO;

public partial class ReportOutput : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        XtraReport report = Session["report"] as XtraReport;
        MemoryStream stream = new MemoryStream();
        this.Title = report.Name;  
 

        Response.Clear();
        report.ExportToPdf(stream);

        Response.ContentType = "application/pdf" ;
        Response.AddHeader("Accept-Header", stream.Length.ToString());
        Response.AddHeader("Content-Disposition","Inline" + "; filename=" + report.Name + ".pdf");
        Response.AddHeader("Content-Length", stream.Length.ToString());
        Response.BinaryWrite(stream.ToArray());
        Response.End();
    }
}
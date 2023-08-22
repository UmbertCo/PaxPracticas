using System;
using System.Web;
using System.Data;

public partial class Consultas_webDescargaPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int nIdComprobante = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["idcomprobante"]));
            fnDescargarPDF(nIdComprobante);
        }
        catch (Exception ex)
        {

        }
    }

    private void fnDescargarPDF(int nIdComprobante)
    {
        DataTable dtComprobante = new clsOperacionComprobantes().fnObtenerArchivoPdf(nIdComprobante);
        string sVersion = dtComprobante.Rows[0]["version"].ToString();
        string sNombreArchivo = "prueba";

        byte[] bPdf = dtComprobante.Rows[0]["pdf"] as byte[];
        if (sVersion.StartsWith("3"))
        {
            sNombreArchivo = dtComprobante.Rows[0]["uuid"].ToString();
        }
        else if (sVersion.StartsWith("2"))
        {
            sNombreArchivo =
                dtComprobante.Rows[0]["serie"].ToString() +
                dtComprobante.Rows[0]["folio"].ToString() +
                dtComprobante.Rows[0]["rfc_emisor"].ToString();
        }
        else
        {
            sNombreArchivo = "Factura" + new DateTime().ToString("dd-MM-yyyy");
        }
        Response.ClearHeaders();
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + sNombreArchivo + ".pdf");
        Response.AddHeader("Content-Length", bPdf.Length.ToString());
        //Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        Response.BinaryWrite(bPdf);
        Response.OutputStream.Flush();
        //Response.Write(comprobante.OuterXml);
        Response.End();
    }
}
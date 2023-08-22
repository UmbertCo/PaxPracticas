using System;
using System.Web;
using System.Data;
using System.Xml;

public partial class Consultas_webDescargarXML : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int nIdComprobante = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["idcomprobante"]));
            fnDescargarXML(nIdComprobante);
        }
        catch (Exception ex)
        {

        }
    }
    private void fnDescargarXML(int nIdComprobante)
    {
        DataTable dtComprobante = new clsOperacionComprobantes().fnObtenerArchivoXml(nIdComprobante);
        string sVersion = dtComprobante.Rows[0]["version"].ToString();
        string sNombreArchivo = string.Empty;
        XmlDocument xXml = new XmlDocument();
        xXml.LoadXml(dtComprobante.Rows[0]["xml"].ToString());
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
        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "text/xml";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + sNombreArchivo + ".xml");
        Response.Write(xXml.InnerXml);
        //Response.Write(comprobante.OuterXml);
        Response.End();
    }
}
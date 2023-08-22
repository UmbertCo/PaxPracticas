using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;

namespace PAXRecepcionProveedores.Consultas
{
    public partial class webDescargarXML : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int nIdComprobante = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["idcomprobante"]));
                int nTipoComprobante = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["tipocomprobante"]));
                fnDescargarXML(nIdComprobante, nTipoComprobante);
            }
            catch (Exception ex)
            {

            }

        }

        private void fnDescargarXML(int nIdComprobante, int nTipoComprobante)
        {
            DataTable dtComprobante = new DataTable();
            //Verifica que tipo de comprobante es para saber de donde llamarlo
            switch (nTipoComprobante)
            {
                    //El número 0 es para los comprobantes del proveedor
                case 0: dtComprobante = new clsOperacionComprobantes().fnObtenerArchivoXml(nIdComprobante);
                    break;
                    //El número 1 es para los comprobantes para el cliente (queda pendiente ver de donde se va a obtener el comprobante)
                case 1:
                    dtComprobante = new clsOperacionComprobantes().fnObtenerArchivoXml(nIdComprobante);
                    break;
            }
            if (dtComprobante.Rows.Count > 0)
            {
                string sVersion = dtComprobante.Rows[0]["version"].ToString();
                string sNombreArchivo = string.Empty;
                XmlDocument xXml = new XmlDocument();
                xXml.LoadXml(dtComprobante.Rows[0]["xml"].ToString());
                if (!string.IsNullOrEmpty(dtComprobante.Rows[0]["uuid"].ToString()))
                {
                    sNombreArchivo = dtComprobante.Rows[0]["uuid"].ToString();
                }
                else
                {
                    sNombreArchivo =
                        dtComprobante.Rows[0]["serie"].ToString() +
                        dtComprobante.Rows[0]["folio"].ToString() +
                        dtComprobante.Rows[0]["rfc_emisor"].ToString();
                }
                if (string.IsNullOrEmpty(sNombreArchivo))
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Xml;

namespace PAXRecepcionProveedores.Consultas
{
    public partial class webDescargaPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                int nIdComprobante = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["idcomprobante"]));
                int nTipoComprobante = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["tipocomprobante"]));
                int nTipoPlantilla = Convert.ToInt32(HttpUtility.UrlDecode(Request.QueryString["tipoplantilla"]));
                fnDescargarPDF(nIdComprobante, nTipoComprobante, nTipoPlantilla);
            }
            catch (Exception ex)
            {

            }
        }

        private void fnDescargarPDF(int nIdComprobante, int nTipoComprobante, int nTipoPlantilla)
        {
            DataTable dtComprobante = new DataTable();
            bool bPlantilla = false;
            
            //Verifica que tipo de comprobante es para saber de donde llamarlo
            
                    //El número 0 es para los comprobantes del proveedor
               if(nTipoComprobante ==0){
                   dtComprobante = new clsOperacionComprobantes().fnObtenerArchivoPdf(nIdComprobante);
               } else{
                    //Si es mayor a 1 es para los comprobantes para el cliente (queda pendiente ver de donde se van a tomar los archivos)

                   //Si el tipo de plantilla es 1, baja el pdf de la base de datos, si es mayor a 1, obtiene la plantilla correspondiente
                   if (nTipoPlantilla == 1)
                   {
                       dtComprobante = new clsOperacionComprobantes().fnObtenerArchivoPdf(nIdComprobante);
                   }
                   else
                   {
                       dtComprobante = new clsOperacionComprobantes().fnObtenerArchivoXml(nIdComprobante);
                       bPlantilla = true;
                   }
                   
               }
            
            if (dtComprobante.Rows.Count > 0)
            {
                if (!bPlantilla)
                {
                    string sVersion = dtComprobante.Rows[0]["version"].ToString();
                    string sNombreArchivo = "prueba";

                    byte[] bPdf = dtComprobante.Rows[0]["pdf"] as byte[];
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
                else
                {
                    try
                    {
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(dtComprobante.Rows[0]["xml"].ToString());
                        clsPlantillaLista pdf = new clsPlantillaLista();
                        pdf.fnObtenerPlantilla(xml, nTipoPlantilla, this,nIdComprobante);
                    }
                    catch (System.Xml.XmlException ex)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
                    }
                    catch (Root.Reports.ReportException ex)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
                    }
                    catch (Exception ex)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
                    }
                }
            }
        }
        public void Page_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError().GetBaseException();

            if (!string.IsNullOrEmpty(objErr.Message))
            {
                Server.ClearError();
                Response.Redirect("~/webGlobalError.aspx");
            }

        }
    }
}
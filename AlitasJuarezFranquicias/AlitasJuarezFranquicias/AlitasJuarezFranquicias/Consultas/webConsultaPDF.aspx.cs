using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using System.Xml;

public partial class webConsultaPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            try
            {
                ///Valores Obtenidos de la url
                ///*************************************************************************
                string sIdCfd = HttpUtility.UrlDecode(Request.QueryString["idcfd"]);
                string sTipoDocumento = clsComun.ObtenerParamentro("TipoComprobante");
                ///*************************************************************************
                fnGenerarPDF(0, sIdCfd, sTipoDocumento,1, "Black");

            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
            }
        }
    }

    /// <summary>
    /// Genera Documento PDF del comprobante
    /// </summary>
    /// <param name="pnIdContribuyente"></param>
    /// <param name="psIdCfd"></param>
    /// <param name="psTipoDocumento"></param>
    /// <param name="sId_rfc"></param>
    /// <param name="scolor"></param>
    private void fnGenerarPDF(int pnIdContribuyente, string psIdCfd, string psTipoDocumento, int sId_rfc, string scolor)
    {
        clsOperacionConsulta gDAL = new clsOperacionConsulta();
        clsPlantillaLista pdf;

        try
        {
            XmlDocument XmlDoc = new XmlDocument();
            pdf = new clsPlantillaLista();
            string plantilla = "Logo"; //PlantillaC.fnRecuperaPlantillaNombre(2); //
            XmlDoc = gDAL.fnObtenerComprobanteXML(pnIdContribuyente, psIdCfd);
            pdf.fnObtenerPLantilla(XmlDoc, plantilla, psIdCfd, psTipoDocumento, this, string.Empty, pnIdContribuyente, sId_rfc, "Black");

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
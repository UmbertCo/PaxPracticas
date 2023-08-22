using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Xml;
using System.Text;

public partial class Timbrado_Addendas_webAddendaAcerosyEquiposMineros : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            btnCerrar.Attributes.Add("onclick", "window.close();");
            //Ocultamos los días de crédito
            lblDiasdeCredito.Attributes.Add("style", "visibility:hidden");
            ddlDiasdeCredito.Attributes.Add("style", "visibility:hidden");
            ddlCondPago.Attributes.Add("onChange", "return fnMostrar();");
        }
    }


    /// <summary>
    /// Loads the language specific resources
    /// </summary>
    protected override void InitializeCulture()
    {
        if (Session["Culture"] != null)
        {
            string lang = Session["Culture"].ToString();
            if ((lang != null) || (lang != ""))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
            }
        }
        else
        {
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }

    //private void fnLimpiarCampos()
    //{
    //    ddlMonedaArt.SelectedValue = "MXN";
    //    txtNoComp.Text = string.Empty;
    //    txtNoProv.Text = string.Empty;
    //    txtOrdCom.Text = string.Empty;
    //    Session.Remove("lblDetSubtotal");
    //    Session.Remove("lblTotalVal");
    //}

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            XmlDocument xmladenda = new XmlDocument();
            string sArmXML = fnArmarXML();
            if (!(string.IsNullOrEmpty(sArmXML)))
            {
                xmladenda.LoadXml(sArmXML);
                Session["AddendaGenerada"] += xmladenda.OuterXml;
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                btnGuardar.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    private string fnArmarXML()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<Documento>");
        sb.Append("<FacturaAcerosyEquiposMineros>");

        sb.Append("<numOrden>");
        sb.Append(txtNumIOrden.Text);
        sb.Append("</numOrden>");
        sb.Append("<condPago>");
        sb.Append(ddlCondPago.SelectedValue);
        if (ddlCondPago.SelectedIndex == 1)
        {
            sb.Append("<diasCredito>");
            sb.Append(ddlDiasdeCredito.SelectedValue);
            sb.Append("</diasCredito>");
        }
        sb.Append("</condPago>");
        sb.Append("<diaVencimiento>");
        sb.Append(txtFechaVencimiento.Text);
        sb.Append("</diaVencimiento>");

        sb.Append("</FacturaAcerosyEquiposMineros>");
        sb.Append("</Documento>");

        return sb.ToString();
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;

public partial class Timbrado_webBusquedaReceptores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnConsulta_Click(object sender, EventArgs e)
    {
        fnTraerReceptores();
    }

    /// <summary>
    /// realiza la búsqueda de los receptores disponibles
    /// </summary>
    private void fnTraerReceptores()
    {
        clsTimbradoFuncionalidad gDAL = new clsTimbradoFuncionalidad();

        try
        {
            string sIdEstructura = Session["identificadorReceptor"].ToString();

            gdvReceptores.DataSource = gDAL.fnLlenarGridReceptores(sIdEstructura, txtRFC.Text, txtRazonSocial.Text);
            gdvReceptores.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void gdvReceptores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvReceptores.PageIndex = e.NewPageIndex;
        fnTraerReceptores();
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Timbrado/webTimbradoGeneracion.aspx");
    }
    protected void hpkSeleccion_Click(object sender, EventArgs e)
    {
        int index = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;

        Session.Add("busquedaReceptor", gdvReceptores.DataKeys[index].Values["id_rfc_receptor"].ToString()
            + ":" + gdvReceptores.DataKeys[index].Values["rfc_receptor"].ToString()
            + ":" + gdvReceptores.DataKeys[index].Values["nombre_receptor"].ToString());

        Response.Redirect("~/Timbrado/webTimbradoGeneracion.aspx");
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
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
            }
        }
        else
        {
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }
}
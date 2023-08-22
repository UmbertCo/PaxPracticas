using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pantallas_Problemas_webBusquedaIncidentes : System.Web.UI.Page
{
    private clsBusquedaIncidentes gDAL;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
        if (!IsPostBack)
        {
            txtFechaIni.Text = DateTime.Today.ToString("dd/MM/yyyy");
            txtFechaFin.Text = DateTime.Today.ToString("dd/MM/yyyy");
        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        fnRealizarBusqueda();
    }

    private void fnRealizarBusqueda()
    {
        gDAL = new clsBusquedaIncidentes();

        try
        {
            gdvIncidentes.DataSource = gDAL.fnObtenerIncidentes(ddlEstatus.SelectedValue, Convert.ToDateTime(txtFechaIni.Text),
                                                                Convert.ToDateTime(txtFechaFin.Text), ddlUrgencia.SelectedValue,
                                                                ddlImpacto.SelectedValue, txtPalabrasClave.Text);
            gdvIncidentes.DataBind();
        }
        catch(Exception ex)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;

public partial class Pantallas_Usuarios_webUsuariosPerfiles : System.Web.UI.Page
{
    private clsUsuarios gDAL;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
        if (!IsPostBack)
        {
            gDAL = new clsUsuarios();
            //manda llamar el metodo que llena el grid con los perfiles
            fnCargaPerfiles();
            fnCargaTipoIncidencias();
        }
    }

    private void fnCargaTipoIncidencias()
    {
        try
        {
            gDAL = new clsUsuarios();
            ddltipoinc.DataSource = gDAL.fnCargarCatalogoTipoIncidencias();
            ddltipoinc.DataTextField = "tipo_incidente";
            ddltipoinc.DataValueField = "id_tipo_incidente";
            ddltipoinc.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaTipoIncidencias", "webUsuariosPerfiles.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaTipoIncidencias", "webUsuariosPerfiles.aspx.cs");
        }
    }

    private void fnCargaPerfiles()
    {
        try
        {
            gDAL = new clsUsuarios();
            gdvPerfiles.DataSource = gDAL.fnLlenaPerfiles();
            gdvPerfiles.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaPerfiles", "webUsuariosPerfiles.aspx.cs");
            gdvPerfiles.DataSource = null;
            gdvPerfiles.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaPerfiles", "webUsuariosPerfiles.aspx.cs");
        }
    }
    protected void gdvPerfiles_SelectedIndexChanged(object sender, EventArgs e)
    {
        //obtener los valores del usuario seleccionado en el grid
        GridViewRow gvrFila = (GridViewRow)gdvPerfiles.SelectedRow;

        Session["psIdPerfil"] = Convert.ToInt32(gdvPerfiles.SelectedDataKey.Value);

        txtPerfil.Text = ((Label)gvrFila.FindControl("lblperfil")).Text;
        btnNuevo.Enabled = true;
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        gDAL = new clsUsuarios();
        gDAL.fnActualizaPerfil(Convert.ToInt32(Session["psIdPerfil"]), txtPerfil.Text);
        btnNuevo.Enabled = true;
    }
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        txtPerfil.Text = String.Empty;
        Session["psIdPerfil"] = 0;
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
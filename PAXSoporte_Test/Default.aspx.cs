using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Data;
using System.Web.Security;
using System.Web.Services;

public partial class webOperacionPrincipal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
    }
    /// <summary>
    /// Loads the language specific resources
    /// </summary>
    protected override void InitializeCulture()
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
        else
        {
            DataTable tabla = new DataTable();
            clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();
            clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
            datosUsuario = clsComun.fnUsuarioEnSesion();

            //Recupera datos de BD
            tabla = busquedaUsuario.fnBuscarUsuario(datosUsuario.userName);
            char sEstadoActual = datosUsuario.estatus;

            if (sEstadoActual != 'A')
            {
                Session["objUsuario"] = null;
                Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
            }
        }
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

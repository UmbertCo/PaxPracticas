using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;

public partial class Account_ChangePassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx");
        }

        //Generacion de Objetos.
        clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
        char sEstadoActual;

        //Asignacion de variables.
        string sRedirecciona = "~/InicioSesion/webInicioSesionLogin.aspx";
        sEstadoActual = datosUsuario.estatus;

        //Revisar el estado Actual
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "Revisar" + "|" + "Revisar el estado Actual del usuario.");
        switch (sEstadoActual)
        {
            case 'P':
                usrGlobalPwd.sRedireccion = sRedirecciona; 
                break;
            case 'B':
                usrGlobalPwd.sRedireccion = sRedirecciona; 
                break;
            case 'E':
                usrGlobalPwd.sRedireccion = sRedirecciona; 
                break;
            case 'C':
                usrGlobalPwd.sRedireccion = sRedirecciona;
                break;

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

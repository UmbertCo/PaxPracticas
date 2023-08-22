using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Security.Principal;

using System.Threading;
using System.Globalization;

public partial class webOperacionPrincipal : System.Web.UI.Page
{
    //clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Extract the forms authentication cookie
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];
        FormsAuthenticationTicket authTicket = null;

        if (null == authCookie)
        {
            return;
        }
        else
        {
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                return;
            }

            if (null == authTicket)
            {
                return;
            }

            // Recupera los roles
            string[] roles = authTicket.UserData.Split(new char[] { '|' });

            // Crea objeto de identidad
            FormsIdentity id = new FormsIdentity(authTicket);

            // This principal will flow throughout the request.
            GenericPrincipal principal = new GenericPrincipal(id, roles);
            // Attach the new principal object to the current HttpContext object
            Context.User = principal;

            IPrincipal p = HttpContext.Current.User;

            if (p.IsInRole("Administrador") && p.IsInRole("Timbrar") && p.IsInRole("Consultas"))
            {
                //lblPermisos.Text = authTicket.UserData;
            }
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
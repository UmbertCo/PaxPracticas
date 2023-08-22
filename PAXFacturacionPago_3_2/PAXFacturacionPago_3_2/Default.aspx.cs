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
    clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Extract the forms authentication cookie
        string cookieName = FormsAuthentication.FormsCookieName;
        HttpCookie authCookie = Context.Request.Cookies[cookieName];
        FormsAuthenticationTicket authTicket = null;

        if (null == authCookie)
        {
            //Entraria a este segmento cuando este en debug
            if (Session["UsuarioActivo"] != null)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgSesionActiva, Resources.resCorpusCFDIEs.varContribuyente);
            }

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
                lblPermisos.Text = authTicket.UserData;
            }

            if (Session["UsuarioActivo"] != null)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgSesionActiva, Resources.resCorpusCFDIEs.varContribuyente);
            }
        }        
    }
    protected void imgDescarga_Click(object sender, ImageClickEventArgs e)
    {
        string filename = "Manual de Usuario.pdf";
        fnDescargaArchivo(filename);
    }
    protected void imgDescargaNomina_Click(object sender, ImageClickEventArgs e)
    {
        string filename = "Manual de Usuario_Nomina.pdf";
        fnDescargaArchivo(filename);
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

    /// <summary>
    /// Funcion encargada de descargar los archivos del servidor.
    /// </summary>
    /// <param name="filename"></param>
    private void fnDescargaArchivo(string filename)
    {
        try
        {
            if (!String.IsNullOrEmpty(filename))
            {
                String dlDir = @"Manuales/";
                String path = Server.MapPath(dlDir + filename).Replace(@"\InicioSesion", "");

                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);

                if (toDownload.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                    Response.AddHeader("Content-Length", toDownload.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(path);
                    Response.End();
                }
            }
        }
        catch (ThreadAbortException)
        {
            //No se registra algun error por la descarga del manual de usuario
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    
}
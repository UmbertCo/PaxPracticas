using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Threading;
using System.Globalization;

public partial class Operacion_Cuenta_webOperacionCuentaBaja : System.Web.UI.Page
{
    private clsOperacionCuenta gDAL;

    protected void Page_Load(object sender, EventArgs e)
    {
         fnEstablecerDialogoModal();
         btnPassBaja.Focus();
    }

    /// <summary>
    /// Creae el script requerido para la confirmación de baja en el idioma correcto
    /// </summary>
    private void fnEstablecerDialogoModal()
    {
        try
        {
            string sScript = "function bajaUsuario(){ return confirm('" + Resources.resCorpusCFDIEs.varConfirmacionBaja + "' );  }";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "fnBajaUsu", sScript, true);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
        }
    }

    protected void btnPassBajaModal_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtPassBaja.Text))
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valConfirmaNueva);
            return;
        }

        gDAL = new clsOperacionCuenta();

        try
        {
            //si se tuvo éxito terminamos su sesión
            if (gDAL.fnConstruirCuerpoCorreo(txtPassBaja.Text))
            {
                clsComun.fnNuevaPistaAuditoria(
                    "webGlobalCuenta",
                    "btnPassBaja",
                    "Se envió solicitud de baja para la cuenta del usuario " + clsComun.fnUsuarioEnSesion().userName +" "+clsComun.fnUsuarioEnSesion().email
                    );

                //Session.Abandon();
                //FormsAuthentication.SignOut();
                //Response.Redirect("~/InicioSesion/webInicioSesionLogin.aspx");

                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNotificaciónCorreo);
            }
            else
                throw new Exception("No se pudo dar de baja la cuenta del usuario");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBajaUsu);
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
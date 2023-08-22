using System;
using System.Web;
using System.Threading;
using System.Globalization;

public partial class InicioSesion_webInicioSesionRecupera : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeCulture();
        }
    }
    protected void btnRecuperaCuenta_Click(object sender, EventArgs e)
    {
        //Asignacion de variables.
        string sOrigen = string.Empty;
        string strMensaje = string.Empty;
        string sPassword = string.Empty;

        sOrigen = this.Page.Title;

        //Preparar envio de correo
        clsGeneraEMAIL sendEmail = new clsGeneraEMAIL();
        clsGeneraLlaves llaves = new clsGeneraLlaves();
        clsInicioSesionSolicitudReg registro = new clsInicioSesionSolicitudReg();


        if (string.IsNullOrEmpty(txtUsuario.Text.Trim()) || string.IsNullOrEmpty(txtCorreo.Text.Trim()))
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }


        if (!clsComun.fnValidaExpresion(txtCorreo.Text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtCorreo);
            return;
        }

        sPassword = GeneradorPassword.GetPassword();

        clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "GenerarLlavesContribuyentes" + "|" + "Crea la clave provisional del usuario.");

        //Buscar Clave existente
        clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "buscarClaveExistente" + "|" + "Revisa existencia del usuario.");
        if (registro.buscarClaveExistente(txtUsuario.Text.Trim(), txtCorreo.Text.Trim()) != 0)
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "actualizaContraseña" + "|" + "Actualiza la contraseña del usaurio." + "|" + txtUsuario.Text + "|" + txtCorreo.Text);
            if (registro.actualizaContraseña(txtUsuario.Text.Trim(), txtCorreo.Text.Trim(), Utilerias.Encriptacion.Base64.EncriptarBase64(sPassword)))
            {
                //Generar mensaje a enviar.
                strMensaje = "<table>";
                strMensaje = strMensaje + "<tr><td><b>Al Contribuyente:</b></td><td>Se le ha enviado un correo para la recuperación de la contraseña.</td></tr>";
                strMensaje = strMensaje + "<tr><td><b>Usuario:</b></td><td>" + txtUsuario.Text + "</td></tr>";
                strMensaje = strMensaje + "<tr><td><b>Contraseña temporal:</b></td><td>" + sPassword + "</td></tr>";
                strMensaje = strMensaje + "</table>";

                //Enviar correo.
                clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "EnviarCorreo" + "|" + "Envia correo de cambio de contraseña.");
                if (sendEmail.EnviarCorreo(txtCorreo.Text, "Recuperación de contraseña", strMensaje + " " + clsComun.ObtenerParamentro("urlHostCosto")))
                {
                    //Actualiza la fecha de ingreso
                    registro.actualizaFechaIngreso(0, txtUsuario.Text.Trim(), txtCorreo.Text.Trim());
                    //Actualizo el estado del usuario a cambiar password.
                    registro.actualizaEstadoActual(txtUsuario.Text.Trim(), txtCorreo.Text.Trim(), 'C');
                    Response.Redirect("webInicioSesionCorrecto.aspx?tpResult=Recupera");
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCorreo, Resources.resCorpusCFDIEs.varContribuyente);
                }
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoResCuenta, Resources.resCorpusCFDIEs.varContribuyente);
            }
        }
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoUsuCorreo, Resources.resCorpusCFDIEs.varContribuyente);
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
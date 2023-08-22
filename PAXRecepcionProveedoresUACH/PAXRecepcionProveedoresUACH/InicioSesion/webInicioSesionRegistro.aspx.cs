using System;
using System.Web.UI;
using System.Threading;
using System.Globalization;

public partial class InicioSesion_webInicioSesionRegistro : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeCulture();
            txtNombre.Focus();
        }
    }

    protected void btnCrearCuenta_Click(object sender, EventArgs e)
    {
        //Asignacion de variables.
        string sOrigen = string.Empty;
        string number_server_side = (string)Session[csAntiBot.SESSION_CAPTCHA];
        string strMensaje = string.Empty;
        string sPassword = string.Empty;

        sOrigen = this.Page.Title;

        string stxtNombre = string.Empty;
        string stxtUsuario = string.Empty;
        string stxtCorreo = string.Empty;

        stxtNombre = txtNombre.Text;
        stxtUsuario = txtUsuario.Text;
        stxtCorreo = txtCorreo.Text;

        System.Threading.Thread.Sleep(1000);

        //Verificar capcha y contrato seleccionado.
        if (number_server_side == txtNumero.Text)
        {

            if (!clsComun.fnValidaExpresion(stxtUsuario, @"(?=^.{8,}$).*$"))
            {
                clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtUsuario);
                return;
            }

            if (!clsComun.fnValidaExpresion(stxtCorreo, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtCorreo);
                return;
            }

            if (chkAceptar.Checked == true)
            {
                //Preparar envio de correo
                clsGeneraEMAIL sendEmail = new clsGeneraEMAIL();
                clsGeneraLlaves llaves = new clsGeneraLlaves();
                clsInicioSesionSolicitudReg registro = new clsInicioSesionSolicitudReg();

                sPassword = GeneradorPassword.GetPassword();

                clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "GenerarLlavesContribuyentes" + "|" + "Crea la contraseña al nuevo contribuyente.");

                //Buscar Clave existente
                clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "buscarClaveExistente" + "|" + "Revisa que no exista el usuario.");
                if (registro.buscarUsuario(stxtUsuario.Trim()).Rows.Count == 0)
                {

                    //Guardar valores en BD
                    clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "solicitudRegistroContribuyente" + "|" + "Registra el nuevo contribuyente" + "|" + stxtNombre + "|" + stxtUsuario);
                    if (registro.solicitudRegistroContribuyente(stxtNombre, stxtUsuario, stxtCorreo, Utilerias.Encriptacion.Classica.Encriptar(sPassword), 'C'))
                    {
                        //Generar mensaje a enviar.
                        strMensaje = "<table>";
                        strMensaje = strMensaje + "<tr><td><b>Al Contribuyente:</b></td><td>Se le ha enviado un correo para continuar con el registro, presione el link que se muestra a continuación.</td></tr>";
                        strMensaje = strMensaje + "<tr><td><b>Usuario:</b></td><td>" + stxtUsuario + "</td></tr>";
                        strMensaje = strMensaje + "<tr><td><b>Contraseña temporal:</b></td><td>" + sPassword + "</td></tr>";
                        strMensaje = strMensaje + "</table>";

                        //Enviar correo.
                        clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "EnviarCorreo" + "|" + "Envia correo a contribuyente" + "|" + stxtCorreo);
                        if (sendEmail.EnviarCorreo(stxtCorreo, Resources.resCorpusCFDIEs.msgRegistroCon, strMensaje + " " + clsComun.ObtenerParamentro("urlHostCosto")))
                        {
                            Response.Redirect("webInicioSesionCorrecto.aspx?tpResult=" + sOrigen);
                        }
                        else
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCorreo, Resources.resCorpusCFDIEs.varContribuyente);
                        }

                    }
                    else
                    {

                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);

                    }
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);
                }
            }
        }
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgMalCaptcha, Resources.resCorpusCFDIEs.varContribuyente);
        }
    }

    protected void chkAceptar_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAceptar.Checked == true)
        {
            btnCrearCuenta.Enabled = true;
        }
        else
        {
            btnCrearCuenta.Enabled = false;
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

    protected void bntRecarga_Click(object sender, ImageClickEventArgs e)
    {
        ImageCaptcha.ImageUrl = "~/captcha.ashx";
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
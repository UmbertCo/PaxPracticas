using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InicioSesion_webInicioSesionLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblIncioSesion.Text = Resources.resCorpusCFDIEs.lblInicioSesion;
            lblBienvenida.Text = Resources.resCorpusCFDIEs.lblBienvenida;
            lblProveedor.Text = Resources.resCorpusCFDIEs.lblProveedor;

            txtUserName.Focus();
            ViewState.Add("nIntentos", 0);

            Form.DefaultButton = btnEntrar.UniqueID;
        }
    }
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            clsErrorLog.fnNuevaEntrada(objErr, clsErrorLog.TipoErroresLog.Datos, "Page_Error", "webInicioSesionLogin");
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx", false);
        }
    }
    protected void btnAviso_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "myModalAviso", "$('#divModalAviso').modal('hide');", true);
        upModalAviso.Update();

        //mpeAvisos.Hide();
    }
    protected void btnEntrar_Click(object sender, EventArgs e)
    {
        //Creear Objetos 
        clsInicioSesionUsuario cInicioSesionUsuario = new clsInicioSesionUsuario();
        clsInicioSesionSolicitudReg cInicioSesionSolicitudReg = new clsInicioSesionSolicitudReg();
        clsConfiguracionPlantilla cConfiguracionPlantilla = new clsConfiguracionPlantilla();
        clsGeneraEMAIL cGeneraEMAIL = new clsGeneraEMAIL();
        DataTable dtUsuario = new DataTable();
        DataTable dtRFC = new DataTable();
        DataTable dtModulos = new DataTable();

        TextBox userName = new TextBox();
        TextBox Password = new TextBox();

        //Asigancion de variables.
        string sRedireccionaReg = "~/InicioSesion/webInicioSesionRegDatos.aspx";
        string sRedireccionaPWD = "~/InicioSesion/webInicioSesionCambiarPWD.aspx";
        string strMensaje = string.Empty;
        string sClientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        string sTimeOut = "10";

        if (string.IsNullOrEmpty(sClientIP))
        {
            sClientIP = Request.ServerVariables["REMOTE_ADDR"];
        }

        //Tiempo de sesion por variable.
        if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["SesionTimeout"]))
        {
            sTimeOut = System.Configuration.ConfigurationManager.AppSettings["SesionTimeout"];
        }

        userName = txtUserName;
        Password = txtPassword;

        if (string.IsNullOrEmpty(userName.Text.Trim()) || string.IsNullOrEmpty(Password.Text.Trim()))
        {
            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varValidacionError, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
            return;
        }

        //LLena datos de usuario.
        dtUsuario = cInicioSesionSolicitudReg.fnBuscarUsuario(userName.Text.Trim());
        dtRFC = cInicioSesionSolicitudReg.fnBuscarUsuarioRFC(userName.Text.Trim());
        dtModulos = cInicioSesionSolicitudReg.fnRecuperaModulosUsuario(userName.Text.Trim());
        Session.Add("objModulos", dtModulos);

        if (dtUsuario.Rows.Count == 0)
        {
            Thread.Sleep(10000);
        }

        if (dtUsuario.Rows.Count > 0)
        {

            int idusuario = Convert.ToInt32(dtUsuario.Rows[0]["id_usuario"]);
            char estatus = Convert.ToChar(dtUsuario.Rows[0]["estatus"]);
            string email = Convert.ToString(dtUsuario.Rows[0]["email"]);
            char origen = Convert.ToChar(dtUsuario.Rows[0]["sistema_origen"]);
            int idEstructura = cConfiguracionPlantilla.fnRecuperaEstructura(idusuario);
            DataTable Plantillas = new DataTable();
            Plantillas = cConfiguracionPlantilla.fnObtieneConfiguracionPlantilla(idEstructura);

            int nPlantilla = 0;
            //nPlantilla = cConfiguracionPlantilla.fnRecuperaPlantillaRecursiva(idEstructura);
            if (origen != 'G')
            {
                clsPistasAuditoria.fnGenerarPistasAuditoria(cInicioSesionUsuario.nIdUsuario, DateTime.Now, this.Title + "|" + sClientIP + "|" + "buscarUsuario" + "|" + userName.Text.Trim() + "|" + "El usuario no corresponde al sistema designado.");
                clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varValidacionError, Resources.resCorpusCFDIEs.varContribuyente, "small", "Error.png");
                return;

            }

            //Revisa contraseña           
            if (Password.Text == PAXCrypto.CryptoAES.DesencriptaAES((byte[])dtUsuario.Rows[0]["password"]))
            {
                //Guardar datos en clase Usuario
                cInicioSesionUsuario.nIdContribuyente = Convert.ToInt32(dtUsuario.Rows[0]["id_contribuyente"]);
                cInicioSesionUsuario.nIdUsuario = Convert.ToInt32(dtUsuario.Rows[0]["id_usuario"]);
                cInicioSesionUsuario.sEstatus = Convert.ToChar(dtUsuario.Rows[0]["estatus"]);
                cInicioSesionUsuario.sUserName = userName.Text.Trim();
                cInicioSesionUsuario.sEmail = Convert.ToString(dtUsuario.Rows[0]["email"]);
                cInicioSesionUsuario.sSistemaOrigen = Convert.ToChar(dtUsuario.Rows[0]["sistema_origen"]);
                if (Plantillas.Rows.Count > 0)
                {
                    cInicioSesionUsuario.nPlantilla = Convert.ToInt32(Plantillas.Rows[0]["id_plantilla"]);
                    cInicioSesionUsuario.sColor = Convert.ToString(Plantillas.Rows[0]["color"]);
                }
                else if (nPlantilla != 0)
                {
                    cInicioSesionUsuario.nPlantilla = nPlantilla;

                }
                if (dtRFC.Rows.Count > 0)
                {
                    cInicioSesionUsuario.nIdRfc = Convert.ToInt32(dtRFC.Rows[0]["id_rfc"]);
                    cInicioSesionUsuario.sRfc = Convert.ToString(dtRFC.Rows[0]["rfc"]);
                    cInicioSesionUsuario.sVersion = "3.3";
                }
                else
                {
                    cInicioSesionSolicitudReg.fnActualizaEstadoActual(userName.Text.Trim(), string.Empty, 'P');
                }

                //Actualizar los datos del usuario.
                clsPistasAuditoria.fnGenerarPistasAuditoria(cInicioSesionUsuario.nIdUsuario, DateTime.Now, this.Title + "|" + "buscarUsuario" + "|" + "Recupera datos del usuario.");

                //Revisar el estatus del usuario.
                switch (estatus)
                {

                    case 'P':

                        cInicioSesionUsuario.Actualizar();
                        Response.Redirect(sRedireccionaReg, false);

                        break;
                    case 'A':

                        //Revisa usuarios conectados
                        bool bUsuarioLog = fnLogin(userName.Text.Trim());
                        if (!bUsuarioLog)
                        {
                            clsComun.fnMessage(this, string.Format(Resources.resCorpusCFDIEs.varUsuLinea, sTimeOut), Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
                            return;
                        }

                        cInicioSesionUsuario.Actualizar();

                        //Actualiza la fecha de ingreso
                        string roles = fnGetRoles();
                        cInicioSesionSolicitudReg.fnActualizaFechaIngreso(cInicioSesionUsuario.nIdUsuario, string.Empty, string.Empty);
                        clsPistasAuditoria.fnGenerarPistasAuditoria(cInicioSesionUsuario.nIdUsuario, DateTime.Now, this.Title + "|" + "actualizaFechaIngreso" + "|" + "Actualiza la fecha de ingreso." + "|" + "ultima_entrada=" + DateTime.Now);
                        clsPistasAuditoria.fnGenerarPistasAuditoria(cInicioSesionUsuario.nIdUsuario, DateTime.Now, this.Title + "|" + sClientIP + "|" + "Ip Origen del Usuario");

                        //Crear autentificacion del usuario logeado


                        FormsAuthenticationTicket authTicket = new
                        FormsAuthenticationTicket(1,                          // version
                                                    userName.Text.Trim(),       // username
                                                    DateTime.Now,               // creation
                                                    DateTime.Now.AddMinutes(Convert.ToDouble(sTimeOut)), // Expiration
                                                    false,                      // Persistent
                                                    roles);                     // Userdata

                        // Now encrypt the ticket.
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        // Create a cookie and add the encrypted ticket to the
                        // cookie as data.

                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        authCookie.Secure = true;

                        // Add the cookie to the outgoing cookies collection.
                        Response.Cookies.Add(authCookie);
                        Page.Session.Timeout = Convert.ToInt32(sTimeOut);

                        // Redirect the user to the originally requested page
                        clsPistasAuditoria.fnGenerarPistasAuditoria(cInicioSesionUsuario.nIdUsuario, DateTime.Now, this.Title + "|" + "FormsAuthentication" + "|" + "Redireciona a pagina de bienvenida, acceso exitoso.");

                        string cookieName = FormsAuthentication.FormsCookieName.ToString();
                        HttpCookie MyCookie = Context.Request.Cookies[cookieName];

                        if (MyCookie.Secure)
                        {
                            Response.Redirect(FormsAuthentication.GetRedirectUrl(userName.Text.Trim(), false), false);
                        }

                        break;
                    case 'B':
                        //Revisa si esta en estado de inactivo el usuario
                        clsPistasAuditoria.fnGenerarPistasAuditoria(cInicioSesionUsuario.nIdUsuario, DateTime.Now, this.Title + "|" + "RevisaInactividad" + "|" + "Revisa si esta en estado de inactivo el usuario");
                        if (cInicioSesionSolicitudReg.fnRevisaInactividad(cInicioSesionUsuario.nIdUsuario))
                        {
                            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msg30diasInactividad, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
                        }
                        else
                        {
                            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msgUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
                        }
                        break;
                    case 'E':

                        cInicioSesionUsuario.Actualizar();
                        Response.Redirect(sRedireccionaPWD, false);

                        break;
                    case 'C':

                        cInicioSesionUsuario.Actualizar();
                        Response.Redirect(sRedireccionaPWD, false);

                        break;
                }

            }
            else
            {
                if (estatus == 'B')
                {
                    clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msgUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
                }
                else
                {
                    //Guardar los intentos de ingreso al sistema.
                    ViewState["nIntentos"] = (int)ViewState["nIntentos"] + 1;
                    clsPistasAuditoria.fnGenerarPistasAuditoria(cInicioSesionUsuario.nIdUsuario, DateTime.Now, this.Title + "|" + "RevisaIntentos" + "|" + "Revisa los intentos de ingreso al sistema.");
                    if ((int)ViewState["nIntentos"] == 3)
                    {
                        //Actualiza el estado del usuario.
                        ViewState["nIntentos"] = 0;
                        cInicioSesionSolicitudReg.fnActualizaEstadoActual(userName.Text.Trim(), string.Empty, 'B');

                        //Generar mensaje a enviar.
                        strMensaje = Resources.resCorpusCFDIEs.mailBloqueo;

                        //Enviar correo.
                        clsPistasAuditoria.fnGenerarPistasAuditoria(idusuario, DateTime.Now, this.Title + "|" + "EnviarCorreo" + "|" + "Envia correo por bloqueo 3 intentos.");
                        if (cGeneraEMAIL.EnviarCorreo(email, Resources.resCorpusCFDIEs.msgBloquearCon, strMensaje + " " + clsComun.fnObtenerParametro("urlHostCosto")))
                        {
                            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msgSeUsuarioBloqueado, Resources.resCorpusCFDIEs.TitInfo, "small", "Info.png");
                        }
                        else
                        {
                            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msgNoCorreo, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
                        }

                    }
                    else
                    {
                        clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varUsuPassInv, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
                    }
                }
            }

        }
        else
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "Usuario Inexistente" + "|" + "El usuario ingresado no exisite en la BD." + "|" + userName.Text.Trim() + "|" + Password.Text.Trim() + "|" + sClientIP);
            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varUsuPassInv, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
        }
    }
    //protected void btnBloqueo_Click(object sender, EventArgs e)
    //{
    //    clsInicioSesionSolicitudReg cInicioSesionSolicitudReg = new clsInicioSesionSolicitudReg();
    //    DataTable dtUsuario = new DataTable();

    //    string sNumeroCaptcha = (string)Session[csAntiBot.SESSION_CAPTCHA];

    //    if (!sNumeroCaptcha.Equals(txtNumero.Text))
    //    {
    //        clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msgMalCaptcha, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
    //        return;
    //    }

    //    //LLena datos de usuario.
    //    dtUsuario = cInicioSesionSolicitudReg.fnBuscarUsuario(txtUserNameBloq.Text.Trim());
    //    Form.DefaultButton = btnEntrar.UniqueID;


    //    if (dtUsuario.Rows.Count > 0)
    //    {
    //        int idusuario = Convert.ToInt32(dtUsuario.Rows[0]["id_usuario"]);
    //        char estatus = Convert.ToChar(dtUsuario.Rows[0]["estatus"]);
    //        string email = Convert.ToString(dtUsuario.Rows[0]["email"]);

    //        //Revisa contraseña
    //        //if (txtPasswordBloq.Text == Utilerias.Encriptacion.Classica.Desencriptar(tabla.Rows[0]["password"].ToString()))            
    //        if (txtPasswordBloq.Text == PAXCrypto.CryptoAES.DesencriptaAES((byte[])dtUsuario.Rows[0]["password"]))
    //        {

    //            System.Collections.Generic.List<string> d = Application["UsersLoggedIn"] as System.Collections.Generic.List<string>;
    //            if (d != null)
    //            {
    //                lock (d)
    //                {
    //                    if (d.Contains(txtUserName.Text) || d.Contains(txtUserNameBloq.Text))
    //                    {
    //                        // User is already logged in!!!
    //                        if (!string.IsNullOrEmpty(txtUserName.Text))
    //                            d.Remove(txtUserName.Text);
    //                        if (!string.IsNullOrEmpty(txtUserNameBloq.Text))
    //                            d.Remove(txtUserNameBloq.Text);
    //                        clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varBloqUsu, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");

    //                    }
    //                    else
    //                    {
    //                        clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varBloqNoUsu, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varUsuPassInv, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
    //        }
    //    }
    //    else
    //    {
    //        clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varUsuPassInv, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
    //    }

    //    txtUserNameBloq.Text = string.Empty;
    //    txtPasswordBloq.Text = string.Empty;
    //}
    protected void btnRecuperaCuenta_Click(object sender, EventArgs e)
    {
        //Asignacion de variables. //REVISAR EL ESTATUS DEL USUARIO Y REVISAR EL RFC LIGA AL USUARIO SI ES DIFERENTE DE P
        string sOrigen = string.Empty;
        string strMensaje = string.Empty;
        string sPassword = string.Empty;
        char cEstatus = 'C';
        sOrigen = this.Page.Title;
        string sNumeroCaptcha = (string)Session[csAntiBot.SESSION_CAPTCHA];

        //Preparar envio de correo
        clsGeneraEMAIL cGeneraEMAIL = new clsGeneraEMAIL();
        clsGeneraLlaves cGeneraLlaves = new clsGeneraLlaves();
        clsInicioSesionSolicitudReg cInicioSesionSolicitudReg = new clsInicioSesionSolicitudReg();

        if (!sNumeroCaptcha.Equals(txtNumeroRec.Text))
        {
            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msgMalCaptcha, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
            return;
        }

        if (string.IsNullOrEmpty(txtUsuario.Text.Trim()) || string.IsNullOrEmpty(txtCorreo.Text.Trim()))
        {
            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varValidacionError, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
            return;
        }


        if (!clsComun.fnValidaExpresion(txtCorreo.Text, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
        {
            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.txtCorreo, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
            return;
        }

        sPassword = GeneradorPassword.GetPassword();

        clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "GenerarLlavesContribuyentes" + "|" + "Crea la clave provisional del usuario.");

        //Buscar Clave existente
        clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "buscarClaveExistente" + "|" + "Revisa existencia del usuario.");
        if (cInicioSesionSolicitudReg.fnBuscarClaveExistente(txtUsuario.Text.Trim(), txtCorreo.Text.Trim()) != 0)
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "actualizaContraseña" + "|" + "Actualiza la contraseña del usaurio." + "|" + txtUsuario.Text + "|" + txtCorreo.Text);
            DataTable dtEstatusUsuario = cInicioSesionSolicitudReg.fnBuscarUsuario(txtUsuario.Text.Trim());

            if (cInicioSesionSolicitudReg.fnBuscarUsuarioRFC(txtUsuario.Text.Trim()).Rows.Count.Equals(0))
            {
                cEstatus = 'P';
            }
            else
            {
                cEstatus = 'C';
            }

            if (cInicioSesionSolicitudReg.fnActualizaContraseña(txtUsuario.Text.Trim(), txtCorreo.Text.Trim(), PAXCrypto.CryptoAES.EncriptaAES(sPassword)))
            {
                //Generar mensaje a enviar.
                strMensaje = "<table>";
                strMensaje = strMensaje + "<tr><td><b>Al Contribuyente:</b></td><td>Se le ha enviado un correo para la recuperación de la contraseña, presione el link que se muestra a continuación.</td></tr>";
                strMensaje = strMensaje + "<tr><td><b>Usuario:</b></td><td>" + txtUsuario.Text + "</td></tr>";
                strMensaje = strMensaje + "<tr><td><b>Contraseña temporal:</b></td><td>" + sPassword + "</td></tr>";
                strMensaje = strMensaje + "</table>";

                //Enviar correo.
                clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "EnviarCorreo" + "|" + "Envia correo de cambio de contraseña.");
                if (cGeneraEMAIL.EnviarCorreo(txtCorreo.Text, Resources.resCorpusCFDIEs.msgRecuperaCon, strMensaje + " " + clsComun.fnObtenerParametro("urlHost")))
                {
                    //Actualiza la fecha de ingreso
                    cInicioSesionSolicitudReg.fnActualizaFechaIngreso(0, txtUsuario.Text.Trim(), txtCorreo.Text.Trim());
                    //Actualizo el estado del usuario a cambiar password.
                    cInicioSesionSolicitudReg.fnActualizaEstadoActual(txtUsuario.Text.Trim(), txtCorreo.Text.Trim(), cEstatus);
                    Response.Redirect("webInicioSesionCorrecto.aspx?tpResult=" + "Recupera");
                }
                else
                {
                    clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msgNoCorreo, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
                }
            }
            else
            {
                clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msgNoResCuenta, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
            }
        }
        else
        {
            clsComun.fnMessage(this, Resources.resCorpusCFDIEs.msgNoUsuCorreo, Resources.resCorpusCFDIEs.varContribuyente, "small", "Info.png");
        }
    }

    protected void btnCanRec_Click(object sender, EventArgs e)
    {
        txtUsuario.Text = string.Empty;
        txtCorreo.Text = string.Empty;
        txtNumeroRec.Text = string.Empty;
    }

    protected void lnkRecMsg_Click(object sender, EventArgs e)
    {
        txtUsuario.Text = string.Empty;
        txtCorreo.Text = string.Empty;
        txtNumeroRec.Text = string.Empty;

        //modalBlock.Show();
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "myModalRecupera", "$('#divModalRecupera').modal();", true);

        //upModalRecupera.Update();
        Form.DefaultButton = btnCrearCuenta.UniqueID;
    }
    protected void imgDescarga_Click(object sender, ImageClickEventArgs e)
    {
        string filename = "Manual de Usuario.pdf";
        fnDescargaArchivo(filename);
    }
    protected void bntRecarga_Click(object sender, EventArgs e)
    {

    }
    protected void bntRecargaRec_Click(object sender, EventArgs e)
    {

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
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnDescargaArchivo", "InicioSesion_webInicioSesionLogin");
        }
    }

    /// <summary>
    /// Recupera el rol de usuario.
    /// </summary>
    /// <returns></returns>
    private string fnGetRoles()
    {
        return "Administrador|Timbrar|Consultas";
    }

    /// <summary>
    /// funcion encargada de revisar el usuario por session.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    protected bool fnLogin(string userName)
    {
        System.Collections.Generic.List<string> d = Application["UsersLoggedIn"] as System.Collections.Generic.List<string>;
        if (d != null)
        {
            lock (d)
            {
                if (d.Contains(userName))
                {
                    // User is already logged in!!!
                    return false;
                }
                d.Add(userName);
            }
        }
        Session["UserLoggedIn"] = userName;
        return true;
    }

    /// <summary>
    /// Función que se encarga de mostrar la ventana de Aviso
    /// </summary>
    private void fnMostrarAviso()
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "myModalAviso", "$('#divModalAviso').modal();", true);
        upModalAviso.Update();
    }
}
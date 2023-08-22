using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Web.Security;
using System.Globalization;

public partial class Account_Login : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                txtUsuario.Focus();
                ViewState.Add("nIntentos", 0);

                Form.DefaultButton = btnLogin.UniqueID;
            }
        }
        catch (Exception ex)
        { clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion); }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        //Creear Objetos
        clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();
        clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
        //clsConfiguracionPlantilla Plantilla = new clsConfiguracionPlantilla();
        clsGeneraEMAIL sendEmail = new clsGeneraEMAIL();
        DataTable tabla = new DataTable();
        DataTable tablaRFC = new DataTable();
        DataTable tablaModulos = new DataTable();

        TextBox userName = new TextBox();
        TextBox Password = new TextBox();

        //Asigancion de variables.
        string sRedireccionaPWD = "~/Cuenta/webInicioSesionCambiarPWD.aspx";
        string strMensaje = string.Empty;
        string sClientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(sClientIP))
        {
            sClientIP = Request.ServerVariables["REMOTE_ADDR"];
        }
        userName = txtUsuario;
        Password = txtPassword;

        if (string.IsNullOrEmpty(userName.Text.Trim()) || string.IsNullOrEmpty(Password.Text.Trim()))
        {
            lblErrorLog.Text = Resources.resCorpusCFDIEs.varValidacionError;
            mpeErrorLog.Show();
            //clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        //LLena datos de usuario.
        tabla = busquedaUsuario.buscarUsuario(userName.Text.Trim());
        tablaRFC = busquedaUsuario.buscarUsuarioRFC(userName.Text.Trim());
        tablaModulos = busquedaUsuario.fnRecuperaModulosUsuario(userName.Text.Trim());
        Session.Add("objModulos", tablaModulos);

        //if (tabla.Rows.Count == 0)
        //{
        //    Thread.Sleep(10000);
        //}

        if (tabla.Rows.Count > 0)
        {

            int idusuario = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
            char estatus = Convert.ToChar(tabla.Rows[0]["estatus"]);
            string email = Convert.ToString(tabla.Rows[0]["email"]); 
            char origen = Convert.ToChar(tabla.Rows[0]["sistema_origen"]);
            //int idEstructura = Plantilla.fnRecuperaEstructura(idusuario);
            //DataTable Plantillas = new DataTable();
            //Plantillas = Plantilla.fnObtieneConfiguracionPlantilla(idEstructura);

            //Revisa contraseña
            if (Password.Text == Utilerias.Encriptacion.Base64.DesencriptarBase64(tabla.Rows[0]["password"].ToString()))
            {


                //Guardar datos en clase Usuario
                datosUsuario.id_contribuyente = Convert.ToInt32(tabla.Rows[0]["id_contribuyente"]);
                datosUsuario.id_usuario = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
                datosUsuario.estatus = Convert.ToChar(tabla.Rows[0]["estatus"]);
                datosUsuario.userName = userName.Text.Trim();
                datosUsuario.email = Convert.ToString(tabla.Rows[0]["email"]);
                datosUsuario.sistema_origen = Convert.ToChar(tabla.Rows[0]["sistema_origen"]);
                //if (Plantillas.Rows.Count > 0)
                //{
                //    datosUsuario.plantilla = Convert.ToInt32(Plantillas.Rows[0]["id_plantilla"]);
                //    datosUsuario.color = Convert.ToString(Plantillas.Rows[0]["color"]);
                //}
                if (tablaRFC.Rows.Count > 0)
                {
                    datosUsuario.id_rfc = Convert.ToInt32(tablaRFC.Rows[0]["id_rfc"]);
                    datosUsuario.rfc = Convert.ToString(tablaRFC.Rows[0]["rfc"]);
                    datosUsuario.version = Convert.ToString(tablaRFC.Rows[0]["version"]);
                }
                datosUsuario.Actualizar();

                //Actualizar los datos del usuario.
                tabla = busquedaUsuario.buscarUsuario(userName.Text.Trim());
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "buscarUsuario" + "|" + "Recupera datos del usuario.");
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "password" + "|" + "Revisa contraseña.");

                //Revisar el estatus del usuario.
                switch (estatus)
                {

                    case 'P':

                        Response.Redirect(sRedireccionaPWD,false);

                        break;
                    case 'A':

                        //Revisa usuarios conectados
                        bool bUsuarioLog = fnLogin(userName.Text.Trim());
                        if (!bUsuarioLog)
                        {
                            lblAviso.Text = Resources.resCorpusCFDIEs.varUsuLinea;
                            mpeAvisos.Show();
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varUsuLinea, Resources.resCorpusCFDIEs.varContribuyente);
                            return;
                        }

                        //Actualiza la fecha de ingreso
                        string roles = fnGetRoles();
                        busquedaUsuario.actualizaFechaIngreso(datosUsuario.id_usuario, string.Empty, string.Empty);
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "actualizaFechaIngreso" + "|" + "Actualiza la fecha de ingreso." + "|" + "ultima_entrada=" + DateTime.Now);
                        //Crear autentificacion del usuario logeado
                        FormsAuthenticationTicket authTicket = new
                        FormsAuthenticationTicket(1,                          // version
                                                    userName.Text.Trim(),       // username
                                                    DateTime.Now,               // creation
                                                    DateTime.Now.AddMinutes(20), // Expiration
                                                    false,                      // Persistent
                                                    roles);                     // Userdata

                        // Now encrypt the ticket.
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        // Create a cookie and add the encrypted ticket to the
                        // cookie as data.

                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        authCookie.Secure = true; 
                        //authCookie.Path = "/";
                        //authCookie.HttpOnly = true;

                        // Add the cookie to the outgoing cookies collection.
                        Response.Cookies.Add(authCookie);
                        //Response.Cookies["sqlAuthCookie"].Expires = DateTime.Now.AddMinutes(5); 

                        Page.Session.Timeout = 20;

                        // Redirect the user to the originally requested page
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "FormsAuthentication" + "|" + "Redireciona a pagina de bienvenida.");
                        //Response.Redirect("~/Account/Bienvenida.aspx");
                        Response.Redirect(FormsAuthentication.GetRedirectUrl(userName.Text.Trim(), false), false);

                        break;
                    case 'B':
                        //Revisa si esta en estado de inactivo el usuario
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "RevisaInactividad" + "|" + "Revisa si esta en estado de inactivo el usuario");
                        if (busquedaUsuario.RevisaInactividad(datosUsuario.id_usuario))
                        {
                            lblAviso.Text = Resources.resCorpusCFDIEs.msg30diasInactividad;
                            mpeAvisos.Show();
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msg30diasInactividad, Resources.resCorpusCFDIEs.varContribuyente);
                        }
                        else
                        {
                            lblAviso.Text = Resources.resCorpusCFDIEs.msgUsuarioBloqueado;
                            mpeAvisos.Show();
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente);
                        }
                        break;
                    case 'E':

                        Response.Redirect(sRedireccionaPWD);

                        break;
                    case 'C':

                        Response.Redirect(sRedireccionaPWD);

                        break;
                    case 'M':

                        Response.Redirect(sRedireccionaPWD);
                        
                        break;
                }

            }
            else
            {
                if (estatus == 'B')
                {
                    lblAviso.Text = Resources.resCorpusCFDIEs.msgUsuarioBloqueado;
                    mpeAvisos.Show();
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente);
                }
                else
                {
                    //Guardar los intentos de ingreso al sistema.
                    ViewState["nIntentos"] = (int)ViewState["nIntentos"] + 1;
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "RevisaIntentos" + "|" + "Revisa los intentos de ingreso al sistema.");
                    if ((int)ViewState["nIntentos"] == 3)
                    {
                        //Actualiza el estado del usuario.
                        ViewState["nIntentos"] = 0;
                        busquedaUsuario.actualizaEstadoActual(userName.Text.Trim(), string.Empty, 'B');

                        //Generar mensaje a enviar.
                        strMensaje = Resources.resCorpusCFDIEs.mailBloqueo;

                        //Enviar correo.
                        clsPistasAuditoria.fnGenerarPistasAuditoria(idusuario, DateTime.Now, this.Title + "|" + "EnviarCorreo" + "|" + "Envia correo por bloqueo 3 intentos.");
                        if (sendEmail.EnviarCorreo(email, Resources.resCorpusCFDIEs.msgBloquearCon, strMensaje + " " + clsComun.ObtenerParamentro("urlHostCosto")))
                        {
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgSeUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente);
                            lblAviso.Text = Resources.resCorpusCFDIEs.msgSeUsuarioBloqueado;
                            mpeAvisos.Show();
                        }
                        else
                        {
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCorreo, Resources.resCorpusCFDIEs.varContribuyente);
                            lblAviso.Text = Resources.resCorpusCFDIEs.msgNoCorreo;
                            mpeAvisos.Show();
                        }

                    }
                    else
                    {
                        lblAviso.Text = Resources.resCorpusCFDIEs.msgPassIncorrecto +"<br/>"+"Intentos: "+ViewState["nIntentos"]+ "/3";
                        mpeAvisos.Show();
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgPassIncorrecto, Resources.resCorpusCFDIEs.varContribuyente);
                    }
                }
            }

        }
        else
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "Usuario Incorrecto" + "|" + "El usuario no coincide con el del registro" + "|" + userName.Text.Trim() + "|" + Password.Text.Trim() + "|" + sClientIP);

            lblAviso.Text = Resources.resCorpusCFDIEs.msgUsuNoExiste;
            mpeAvisos.Show();
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgUsuNoExiste, Resources.resCorpusCFDIEs.varContribuyente);
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
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
            }
        }
        else
        {
            string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
        }
    }

    protected void imgDescarga_Click(object sender, ImageClickEventArgs e)
    {
        string filename = "Manual de Usuario.pdf";
        fnDescargaArchivo(filename);
        //Response.Redirect("~/About.aspx", true);
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
                String path = Server.MapPath(dlDir + filename).Replace(@"\Account", "");

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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void lnkDesbloq_Click(object sender, EventArgs e)
    {
        modalBlock.Show();
        Form.DefaultButton = btnBloqueo.UniqueID;
    }
    protected void btnBloqueo_Click(object sender, EventArgs e)
    {
        clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
        DataTable tabla = new DataTable();

        //LLena datos de usuario.
        tabla = busquedaUsuario.buscarUsuario(txtUserNameBloq.Text.Trim());
        Form.DefaultButton = btnLogin.UniqueID;


        if (tabla.Rows.Count > 0)
        {
            int idusuario = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
            char estatus = Convert.ToChar(tabla.Rows[0]["estatus"]);
            string email = Convert.ToString(tabla.Rows[0]["email"]);

            //Revisa contraseña
            if (txtPasswordBloq.Text == Utilerias.Encriptacion.Base64.DesencriptarBase64(tabla.Rows[0]["password"].ToString()))
            {

                System.Collections.Generic.List<string> d = Application["UsersLoggedIn"] as System.Collections.Generic.List<string>;
                if (d != null)
                {
                    lock (d)
                    {
                        if (txtUsuario.Text == txtUserNameBloq.Text)
                        {
                            // User is already logged in!!!
                            if (!string.IsNullOrEmpty(txtUsuario.Text))
                                d.Remove(txtUsuario.Text);
                            if (!string.IsNullOrEmpty(txtUserNameBloq.Text))
                                d.Remove(txtUserNameBloq.Text);
                            //string sUsuario = txtUserNameBloq.Text;
                            //string sPassword = tabla.Rows[0]["password"].ToString();
                            //if (clsComun.fnDesbloquear(sUsuario, sPassword) > 0)
                            //{
                            lblAviso.Text = Resources.resCorpusCFDIEs.varBloqUsu;
                            mpeAvisos.Show();
                            //}
                        }
                        else
                        {
                            lblAviso.Text = Resources.resCorpusCFDIEs.varBloqNoUsu;
                            mpeAvisos.Show();
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBloqNoUsu, Resources.resCorpusCFDIEs.varContribuyente);
                        }
                    }
                }
            }
            else
            {
                lblAviso.Text = Resources.resCorpusCFDIEs.msgPassIncorrecto;
                mpeAvisos.Show();
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgPassIncorrecto, Resources.resCorpusCFDIEs.varContribuyente);
            }
        }
        else
        {
            lblAviso.Text = Resources.resCorpusCFDIEs.msgUsuNoExiste;
            mpeAvisos.Show();
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgUsuNoExiste, Resources.resCorpusCFDIEs.varContribuyente);
        }

        txtUserNameBloq.Text = string.Empty;
        txtPasswordBloq.Text = string.Empty;
    }
    protected void btnCanBloq_Click(object sender, EventArgs e)
    {
        txtUserNameBloq.Text = string.Empty;
        txtPasswordBloq.Text = string.Empty;
    }
}

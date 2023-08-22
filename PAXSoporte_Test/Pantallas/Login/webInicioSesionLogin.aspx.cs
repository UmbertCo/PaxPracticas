using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Globalization;
using System.Threading;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblIncioSesion.Text = Resources.resCorpusCFDIEs.lblInicioSesion;
            lblBienvenida.Text = Resources.resCorpusCFDIEs.lblBienvenida;
            //lblProveedor.Text = Resources.resCorpusCFDIEs.lblProveedor;

            txtUserName.Focus();
            ViewState.Add("nIntentos", 0);

        }
    }
    protected void btnEntrar_Click(object sender, EventArgs e)
    {
        //Creear Objetos
        clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();
        clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
        clsGeneraEMAIL sendEmail = new clsGeneraEMAIL();
        DataTable tabla = new DataTable();

        TextBox userName = new TextBox();
        TextBox Password = new TextBox();

        //Asigancion de variables.
        string sRedireccionaReg = "webInicioSesionRegDatos.aspx";
        string sRedireccionaPWD = "webInicioSesionCambiarPWD.aspx";
        string strMensaje = string.Empty;
        string sClientIP=Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(sClientIP))
        {
            sClientIP = Request.ServerVariables["REMOTE_ADDR"];
        }
        userName = txtUserName;
        Password = txtPassword;

        if (string.IsNullOrEmpty(userName.Text.Trim()) || string.IsNullOrEmpty(Password.Text.Trim()))
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        //LLena datos de usuario.
        tabla = busquedaUsuario.fnBuscarUsuario(userName.Text.Trim());

        if (tabla.Rows.Count > 0)
        {
            int idusuario = Convert.ToInt32(tabla.Rows[0]["id_usuario_soporte"]);
            char estatus = Convert.ToChar(tabla.Rows[0]["estatus"]);
            string email = Convert.ToString(tabla.Rows[0]["email"]);


            //Revisa contraseña
            if (Password.Text == PAXCrypto.CryptoAES.DesencriptaAES((byte[])tabla.Rows[0]["password"]))
            {
                //Guardar datos en clase Usuario
                //datosUsuario.id_contribuyente = Convert.ToInt32(tabla.Rows[0]["id_contribuyente"]);
                datosUsuario.id_usuario = Convert.ToInt32(tabla.Rows[0]["id_usuario_soporte"]);
                datosUsuario.estatus = Convert.ToChar(tabla.Rows[0]["estatus"]);
                datosUsuario.userName = userName.Text.Trim();
                datosUsuario.email = Convert.ToString(tabla.Rows[0]["email"]);

                datosUsuario.Actualizar();

                //Actualizar los datos del usuario.
                tabla = busquedaUsuario.fnBuscarUsuario(userName.Text.Trim());
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "buscarUsuario" + "|" + "Recupera datos del usuario.");
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "password" + "|" + "Revisa contraseña.");

                //Revisar el estatus del usuario.
                switch (estatus)
                {

                    case 'P':

                        Response.Redirect(sRedireccionaReg);

                        break;
                    case 'A':

                        //Actualiza la fecha de ingreso
                        string roles = fnGetRoles();
                        busquedaUsuario.fnActualizaFechaIngreso(datosUsuario.id_usuario, string.Empty, string.Empty);
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "actualizaFechaIngreso" + "|" + "Actualiza la fecha de ingreso." + "|" + "ultima_entrada=" + DateTime.Now);
                        //Crear autentificacion del usuario logeado
                        FormsAuthenticationTicket authTicket = new
                        FormsAuthenticationTicket(1,                          // version
                                                    userName.Text.Trim(),       // username
                                                    DateTime.Now,               // creation
                                                    DateTime.Now.AddMinutes(10), // Expiration
                                                    false,                      // Persistent
                                                    roles);                     // Userdata

                        // Now encrypt the ticket.
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        // Create a cookie and add the encrypted ticket to the
                        // cookie as data.

                        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                        // Add the cookie to the outgoing cookies collection.
                        Response.Cookies.Add(authCookie);

                        Page.Session.Timeout = 10;

                        // Redirect the user to the originally requested page
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "FormsAuthentication" + "|" + "Redireciona a pagina de bienvenida.");
                        Response.Redirect(FormsAuthentication.GetRedirectUrl(userName.Text.Trim(), false));

                        break;
                    case 'B':

                        string roles2 = fnGetRoles();
                        busquedaUsuario.fnActualizaFechaIngreso(datosUsuario.id_usuario, string.Empty, string.Empty);
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "actualizaFechaIngreso" + "|" + "Actualiza la fecha de ingreso." + "|" + "ultima_entrada=" + DateTime.Now);
                        //Crear autentificacion del usuario logeado
                        FormsAuthenticationTicket authTicket2 = new
                        FormsAuthenticationTicket(1,                          // version
                                                    userName.Text.Trim(),       // username
                                                    DateTime.Now,               // creation
                                                    DateTime.Now.AddMinutes(10), // Expiration
                                                    false,                      // Persistent
                                                    roles2);                     // Userdata

                        // Now encrypt the ticket.
                        string encryptedTicket2 = FormsAuthentication.Encrypt(authTicket2);
                        // Create a cookie and add the encrypted ticket to the
                        // cookie as data.

                        HttpCookie authCookie2 = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket2);

                        // Add the cookie to the outgoing cookies collection.
                        Response.Cookies.Add(authCookie2);

                        Page.Session.Timeout = 10;

                        // Redirect the user to the originally requested page
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "FormsAuthentication" + "|" + "Redireciona a pagina de cambio de password.");
                        Response.Redirect(sRedireccionaPWD);
                        break;
                    case 'E':

                        Response.Redirect(sRedireccionaPWD);

                        break;
                    case 'C':

                        Response.Redirect(sRedireccionaPWD);

                        break;
                }

            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgPassIncorrecto, Resources.resCorpusCFDIEs.varContribuyente);
                //if (estatus == 'B')
                //{
                //   // clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente);
                //}
                //else
                //{
                //    //Guardar los intentos de ingreso al sistema.
                //    //ViewState["nIntentos"] = (int)ViewState["nIntentos"] + 1;
                //    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "RevisaIntentos" + "|" + "Revisa los intentos de ingreso al sistema.");
                //    //if ((int)ViewState["nIntentos"] == 3)
                //    //{
                //        //Actualiza el estado del usuario.
                //        ////ViewState["nIntentos"] = 0;
                //        //busquedaUsuario.actualizaEstadoActual(userName.Text.Trim(), string.Empty, 'B');

                //        ////Generar mensaje a enviar.
                //        //strMensaje = Resources.resCorpusCFDIEs.mailBloqueo;

                //        ////Enviar correo.
                //        //string ruta = null;
                //        //clsPistasAuditoria.fnGenerarPistasAuditoria(idusuario, DateTime.Now, this.Title + "|" + "EnviarCorreo" + "|" + "Envia correo por bloqueo 3 intentos.");
                //        //if (sendEmail.EnviarCorreo(email, Resources.resCorpusCFDIEs.msgBloquearCon, strMensaje + " " + clsComun.fnObtenerParamentro("urlHost") + "CORPUSCFDI/InicioSesion/webInicioSesionLogin.aspx", ruta))
                //        //{
                //        //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgSeUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente);
                //        //}
                //        //else
                //        //{
                //        //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgSeUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente);
                //        //}

                //    //}
                //    //else
                //    //{
                      
                //    //}
                //}
            }

        }
        else
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "Usuario Inexistente" + "|" + "El usuario ingresado no exisite en la BD." + "|" + userName.Text.Trim() + "|" + Password.Text.Trim() + "|" + sClientIP);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgUsuNoExiste, Resources.resCorpusCFDIEs.varContribuyente);
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

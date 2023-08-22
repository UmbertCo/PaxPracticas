using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Web.Security;
using System.Data;

namespace PAXRecepcionProveedores.InicioSesion
{
    public partial class webInicioSesionLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblIncioSesion.Text = Resources.resCorpusCFDIEs.lblInicioSesion;
                    lblBienvenida.Text = Resources.resCorpusCFDIEs.lblBienvenida;
                    //lblProveedor.Text = Resources.resCorpusCFDIEs.lblProveedor;

                    txtUserName.Focus();
                    ViewState.Add("nIntentos", 0);

                    Form.DefaultButton = btnEntrar.UniqueID;
                    if (Session["altaProveedor"] != null)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAltaProveedor);
                        Session.Remove("altaProveedor");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/webGlobalError.aspx");
            }
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            //Creear Objetos
            clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();
            clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
            clsGeneraEMAIL sendEmail = new clsGeneraEMAIL();
            DataTable tabla = new DataTable();
            DataTable tablaRFC = new DataTable();
            DataTable tablaModulos = new DataTable();

            TextBox userName = new TextBox();
            TextBox Password = new TextBox();

            //Asigancion de variables.
            string sRedireccionaReg = "webInicioSesionRegDatos.aspx";
            string sRedireccionaPWD = "~/InicioSesion/webInicioSesionCambiarPWD.aspx";
            string strMensaje = string.Empty;
            string sClientIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
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
            tabla = busquedaUsuario.buscarUsuario(userName.Text.Trim());
            //tablaRFC = busquedaUsuario.buscarUsuarioRFC(userName.Text.Trim());
            

            if (tabla.Rows.Count == 0)
            {
                Thread.Sleep(10000);
            }

            if (tabla.Rows.Count > 0)
            {

                int idusuario = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
                char estatus = Convert.ToChar(tabla.Rows[0]["estatus"]);
                string email = Convert.ToString(tabla.Rows[0]["email"]);
                char origen = Convert.ToChar(tabla.Rows[0]["sistema_origen"]);
                DataTable Plantillas = new DataTable();

                //Revisa contraseña
                if (Password.Text == Utilerias.Encriptacion.Base64.DesencriptarBase64(tabla.Rows[0]["password"].ToString()))
                {
                    tablaModulos = busquedaUsuario.fnRecuperaModulosUsuario(userName.Text);
                    Session.Add("objModulos", tablaModulos);
                    //Guardar datos en clase Usuario
                    //datosUsuario.id_contribuyente = Convert.ToInt32(tabla.Rows[0]["id_contribuyente"]);
                    datosUsuario.id_usuario = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
                    datosUsuario.estatus = Convert.ToChar(tabla.Rows[0]["estatus"]);
                    datosUsuario.userName = Convert.ToString(tabla.Rows[0]["clave_usuario"]);
                    datosUsuario.email = Convert.ToString(tabla.Rows[0]["email"]);
                    datosUsuario.sistema_origen = Convert.ToChar(tabla.Rows[0]["sistema_origen"]);
                    datosUsuario.Id_perfil = Convert.ToInt32(tabla.Rows[0]["id_perfil"].ToString());
                    
                    //if (tablaRFC.Rows.Count > 0)
                    //{
                    //    datosUsuario.id_rfc = Convert.ToInt32(tablaRFC.Rows[0]["id_rfc"]);
                    //    datosUsuario.rfc = Convert.ToString(tablaRFC.Rows[0]["rfc"]);
                    //    datosUsuario.version = Convert.ToString(tablaRFC.Rows[0]["version"]);
                    //}
                    datosUsuario.Actualizar();

                    //Actualizar los datos del usuario.
                    tabla = busquedaUsuario.buscarUsuario(userName.Text.Trim());
                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "buscarUsuario" + "|" + "Recupera datos del usuario.");
                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "password" + "|" + "Revisa contraseña.");

                    //Revisar el estatus del usuario.
                    switch (estatus)
                    {

                        case 'P':

                            Response.Redirect(sRedireccionaReg);

                            break;
                        case 'A':

                            //Revisa usuarios conectados
                            bool bUsuarioLog = fnLogin(userName.Text.Trim());
                            if (!bUsuarioLog)
                            {
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varUsuLinea, Resources.resCorpusCFDIEs.varContribuyente);
                                return;
                            }

                            //Actualiza la fecha de ingreso
                            string roles = fnGetRoles();
                            busquedaUsuario.actualizaFechaIngreso(datosUsuario.id_usuario, string.Empty, string.Empty);
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "actualizaFechaIngreso" + "|" + "Actualiza la fecha de ingreso." + "|" + "ultima_entrada=" + DateTime.Now);
                            //Crear autentificacion del usuario logeado
                            FormsAuthenticationTicket authTicket = new
                            FormsAuthenticationTicket(1,                          // version
                                                        datosUsuario.userName,       // username
                                                        DateTime.Now,               // creation
                                                        DateTime.Now.AddMinutes(20), // Expiration
                                                        false,                      // Persistent
                                                        roles);                     // Userdata

                            // Now encrypt the ticket.
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            // Create a cookie and add the encrypted ticket to the
                            // cookie as data.

                            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                            //authCookie.Path = "/";
                            //authCookie.HttpOnly = true;

                            // Add the cookie to the outgoing cookies collection.
                            Response.Cookies.Add(authCookie);
                            //Response.Cookies["sqlAuthCookie"].Expires = DateTime.Now.AddMinutes(5); 

                            Page.Session.Timeout = 20;

                            // Redirect the user to the originally requested page
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "FormsAuthentication" + "|" + "Redireciona a pagina de bienvenida.");
                            Response.Redirect(FormsAuthentication.GetRedirectUrl(userName.Text.Trim(), false));

                            break;
                        case 'B':
                            //Revisa si esta en estado de inactivo el usuario
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "RevisaInactividad" + "|" + "Revisa si esta en estado de inactivo el usuario");
                            if (busquedaUsuario.RevisaInactividad(datosUsuario.id_usuario))
                            {
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msg30diasInactividad, Resources.resCorpusCFDIEs.varContribuyente);
                            }
                            else
                            {
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente);
                            }
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
                    if (estatus == 'B')
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente);
                    }
                    else
                    {
                        //Guardar los intentos de ingreso al sistema.
                        ViewState["nIntentos"] = (int)ViewState["nIntentos"] + 1;
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "RevisaIntentos" + "|" + "Revisa los intentos de ingreso al sistema.");
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
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgSeUsuarioBloqueado, Resources.resCorpusCFDIEs.varContribuyente);
                            }
                            else
                            {
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCorreo, Resources.resCorpusCFDIEs.varContribuyente);
                            }

                        }
                        else
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgPassIncorrecto, Resources.resCorpusCFDIEs.varContribuyente);
                        }
                    }
                }

            }
            else
            {
                clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "Usuario Inexistente" + "|" + "El usuario ingresado no exisite en la BD." + "|" + userName.Text.Trim() + "|" + Password.Text.Trim() + "|" + sClientIP);
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgUsuNoExiste, Resources.resCorpusCFDIEs.varContribuyente);
            }
            
        }

        protected void btnBloqueo_Click(object sender, EventArgs e)
        {
            clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
            DataTable tabla = new DataTable();

            //LLena datos de usuario.
            tabla = busquedaUsuario.buscarUsuario(txtUserNameBloq.Text.Trim());
            Form.DefaultButton = btnEntrar.UniqueID;


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
                            if (d.Contains(txtUserName.Text) || d.Contains(txtUserNameBloq.Text))
                            {
                                // User is already logged in!!!
                                if (!string.IsNullOrEmpty(txtUserName.Text))
                                    d.Remove(txtUserName.Text);
                                if (!string.IsNullOrEmpty(txtUserNameBloq.Text))
                                    d.Remove(txtUserNameBloq.Text);
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBloqUsu, Resources.resCorpusCFDIEs.varContribuyente);

                            }
                            else
                            {
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBloqNoUsu, Resources.resCorpusCFDIEs.varContribuyente);
                            }
                        }
                    }
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgPassIncorrecto, Resources.resCorpusCFDIEs.varContribuyente);
                }
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgPassIncorrecto, Resources.resCorpusCFDIEs.varContribuyente);
            }

            txtUserNameBloq.Text = string.Empty;
            txtPasswordBloq.Text = string.Empty;
        }

        protected void btnCanBloq_Click(object sender, EventArgs e)
        {
            txtUserNameBloq.Text = string.Empty;
            txtPasswordBloq.Text = string.Empty;
        }

        protected void lnkDesbloq_Click(object sender, EventArgs e)
        {
            modalBlock.Show();
            Form.DefaultButton = btnBloqueo.UniqueID;
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
                    else
                    {
                        clsComun.fnMostrarMensaje(this, "Manual en proceso");
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            }
        }
        protected void imgDescarga_Click(object sender, ImageClickEventArgs e)
        {
            string filename = "Manual de Usuario.pdf";
            fnDescargaArchivo(filename);
        }
    }
}
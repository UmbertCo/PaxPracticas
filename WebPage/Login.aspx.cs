using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using System.Data;
using System.Web.Security;


namespace WebPage
{
    public partial class WebForm6 : System.Web.UI.Page
    {

        Logica negocio = new Logica();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.FindControl("lblmenu").Visible = false;
            Master.FindControl("menuBorder").Visible = false;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("NuevoUsuario.aspx");
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //variables
            #region

            bool bEstatus = false;
            string sUsuario = string.Empty;
            string sPass = string.Empty;

            #endregion

            sUsuario = txtUsuario.Text;
            sPass = txtContraseña.Text;

            if (sUsuario == string.Empty)
            {

                Response.Write("Ingresar Usuario");
                return;
            }
            if (sPass == string.Empty)
            {
                Response.Write("Ingresar Contraseña");
                return;
            }

            DataTable tabla = negocio.fnVericaUsuarioExiste(sUsuario, sPass);

            if (tabla.Rows.Count > 0)
            {
                if (txtContraseña.Text == Utilerias.Encriptacion.Classica.Desencriptar(tabla.Rows[0]["contrasena"].ToString()))
                {
                    try
                    {

                        DataTable dtTabladePersonal = negocio.fnMostraTabladelPersonal(sUsuario);

                        Session["idPersonal"] = dtTabladePersonal.Rows[0]["idPersonal"];
                        Session["Nombre"] = dtTabladePersonal.Rows[0]["nombre"].ToString();
                        Session["typo"] = dtTabladePersonal.Rows[0]["typo"].ToString();
                        Session["idUsuario"] = dtTabladePersonal.Rows[0]["idUsuario"].ToString();
                        Session["ApPaterno"] = dtTabladePersonal.Rows[0]["ApPaterno"].ToString();
                        bEstatus = Convert.ToBoolean(dtTabladePersonal.Rows[0]["estatus"]);

                        if (bEstatus == true)
                        {
                            txtUsuario.Text = "";
                            txtUsuario.Focus();
                            string nombre = Session["Nombre"].ToString();


                            FormsAuthenticationTicket authTicket = new
                            FormsAuthenticationTicket(1,                          // version
                                 nombre,                   // username
                                 DateTime.Now,               // creation
                                 DateTime.Now.AddMinutes(20), // Expiration
                                 false,                      // Persistent
                                 "Admin");                     // Userdata

                            // Now encrypt the ticket.
                            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                            // Create a cookie and add the encrypted ticket to the
                            // cookie as data.

                            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                            // Add the cookie to the outgoing cookies collection.
                            Response.Cookies.Add(authCookie);

                            Page.Session.Timeout = 20;

                            // Redirect the user to the originally requested page
                            Response.Redirect(FormsAuthentication.GetRedirectUrl(nombre, false));

                        }
                        else
                        {
                            //mensaje 
                        }

                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex);
                    }

                }
                else
                {
                    lblerror.Visible = true;
                }
            }
        }
    }
}

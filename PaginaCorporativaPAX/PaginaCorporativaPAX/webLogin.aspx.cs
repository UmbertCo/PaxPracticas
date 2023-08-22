using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Globalization;

namespace PaginaComercial
{
    public partial class webLogin: System.Web.UI.Page
    {
        private PagComercial gPag;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            string componente = Request.QueryString["componente"];
            Label5.Visible = false;
            gPag = new PagComercial();
            DataTable dtUsuario = gPag.fnObtieneUsuarioSoporte(txtUserName.Text);

            // Agregar validacion para que revise si regreso datos de usuario
            if (dtUsuario.Rows.Count > 0)
            {
                if (txtPassword.Text == Utilerias.Encriptacion.Classica.Desencriptar(dtUsuario.Rows[0]["password"].ToString()))
                {
                    DataTable dtLogin = gPag.fnObtieneUsuarioSoporteLogin(Convert.ToInt32(dtUsuario.Rows[0]["id_usuario_soporte"]), componente);
                    if (dtLogin.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon in dtLogin.Rows)
                        {
                            switch (Convert.ToString(Renglon["componente"]))
                            {
                                case "webNoticias.aspx":
                                    
                                    Label5.Visible = false;
                                    Response.Redirect("webNoticias.aspx");
                                    break;

                                case "webPreguntasRegistro.aspx":
                                   
                                    Label5.Visible = false;
                                    Response.Redirect("webPreguntasRegistro.aspx");
                                    break;
                            }

                        }
                    }
                    else
                    {
                        Common.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblPasswordError, string.Empty);
                    }

                }
                else
                {
                    Common.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblPasswordError, string.Empty);
                }
            }
            else
            {
                Common.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblPasswordError, string.Empty);
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
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;

public partial class Pantallas_Login_webRecuperacionPassword : System.Web.UI.Page
{
    private clsUsuarios gDAL;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
         try
        {
            if (txtUsuario.Text != "" && txtCorreo.Text != "")
            {
                clsGeneraEMAIL email = new clsGeneraEMAIL();
                gDAL = new clsUsuarios();
                string psPass = gDAL.fnObtenerContraseniaUsuario(txtUsuario.Text, txtCorreo.Text);
                if (psPass != null)
                {
                    string sPassword = GeneradorPassword.GetPassword();
                    string strMensaje = "<table>";
                    strMensaje = strMensaje + "<tr><td><b>Estimado usuario del sistema de mesa de ayuda</b></td><td>Se le ha enviado un correo para la recuperación de la contraseña</td></tr>";
                    strMensaje = strMensaje + "<tr><td><b>Usuario:</b></td><td>" + txtUsuario.Text + "</td></tr>";
                    strMensaje = strMensaje + "<tr><td><b>Contraseña temporal:</b></td><td>" + sPassword + "</td></tr>";
                    strMensaje = strMensaje + "</table>";
                    gDAL.fnActualizaPasswordUsuario(txtUsuario.Text, sPassword,"C");
                    email.fnEnviarCorreoAtencionIncidencia(txtCorreo.Text, "Correo de recuperación de contraseña", strMensaje, clsComun.fnObtenerParamentro("emailAppFrom"));
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEnvioCorreo);
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoUsuCorreo); 
                }
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblRecuperaDatos);
            }
        
        }
         catch (SqlException ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Button1_Click", "webRecuperacionPassword.aspx.cs");
         }
         catch (Exception ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "Button1_Click", "webRecuperacionPassword.aspx.cs");
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
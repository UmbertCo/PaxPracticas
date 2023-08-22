using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;

public partial class WebControls_usrGlobalPwd : System.Web.UI.UserControl
{
    clsUsuarios gDAL = new clsUsuarios();
    public string sRedireccion     { get; set; }
    public bool   bTerminarSesion  { get; set; }
    public string TextoBoton
    {
        get { return btnValidar.Text; }
        set { btnValidar.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Crear Objetos
            clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();
            clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
            gDAL = new clsUsuarios();
            DataTable tabla = new DataTable();

            //Recuperar estructura de sesion
            datosUsuario = clsComun.fnUsuarioEnSesion();
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "fnUsuarioEnSesion" + "|" + "Recuperar estructura de sesion.");

            txtContraseniaAnterior.Focus();

        }
    }
    protected void btnValidar_Click(object sender, EventArgs e)
    {
        //Crear Objetos
        sRedireccion = "~/Pantallas/Login/webInicioSesionLogin.aspx";
        gDAL = new clsUsuarios();
        clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();
        clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
        DataTable tabla = new DataTable();

        char sEstadoActual;
        string sPassword;


        if (string.IsNullOrEmpty(txtContraseniaAnterior.Text.Trim()) ||
            string.IsNullOrEmpty(txtContraseniaNueva.Text.Trim()) ||
            string.IsNullOrEmpty(txtConfirmaNueva.Text.Trim()))
        {
            clsComun.fnMostrarError(this.Page, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }


        if (!clsComun.fnValidaExpresion(txtContraseniaNueva.Text, @"(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"))
        {
            clsComun.fnMostrarError(this.Page, Resources.resCorpusCFDIEs.valContraseniaNueva);
            return;
        }

        if (!clsComun.fnValidaExpresion(txtConfirmaNueva.Text, @"(?=^.{8,}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z]).*$"))
        {
            clsComun.fnMostrarError(this.Page, Resources.resCorpusCFDIEs.valContraseniaNueva);
            return;
        }



        //Recuperar estructura de sesion
        datosUsuario = clsComun.fnUsuarioEnSesion();

        //Recupera datos de BD
        tabla = busquedaUsuario.fnBuscarUsuario(datosUsuario.userName);
        sEstadoActual = datosUsuario.estatus;

        if (tabla.Rows.Count > 0)
        {
            //Recupera y desencripta la contraseña del usuario.
            if (txtContraseniaAnterior.Text == PAXCrypto.CryptoAES.DesencriptaAES((byte[])tabla.Rows[0]["password"]))
            {

                //Revisa que no sea igual en nombre de usuario a contraseña.
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "RevisarNOmbre" + "|" + "Revisa que no sea igual en nombre de usuario a contraseña.");
                if (datosUsuario.userName != txtConfirmaNueva.Text.Trim())
                {

                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "actualizaContraseña" + "|" + "Actualiza contraseña del usuario en BD.");

                   string psStaturs = "A";
                   bool psConfirmacion = false;
                   psConfirmacion = gDAL.fnActualizaPasswordUsuario(datosUsuario.userName, txtConfirmaNueva.Text.Trim(), psStaturs);
                   try
                   {
                       Response.Redirect(sRedireccion);
                   }
                   catch
                   {
                   }
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoPassUsu, Resources.resCorpusCFDIEs.varContribuyente);  
                   
                }
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoPassCon, Resources.resCorpusCFDIEs.varContribuyente); 
 
               
            }


        }

    }
}
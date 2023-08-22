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
    public string sRedireccion { get; set; }
    public bool bTerminarSesion { get; set; }
    public string TextoBoton
    {
        get { return btnValidar.Text; }
        set { btnValidar.Text = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //Crear Objetos
                clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();
                clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
                DataTable tabla = new DataTable();

                //Recuperar estructura de sesion
                datosUsuario = clsComun.fnUsuarioEnSesion();
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "fnUsuarioEnSesion" + "|" + "Recuperar estructura de sesion.");


                //Recupera datos de BD
                tabla = busquedaUsuario.buscarUsuario(datosUsuario.userName);
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "buscarUsuario" + "|" + "Recuperar los datos del usuario." + "|" + datosUsuario.userName);


                txtContraseniaAnterior.Focus();

                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "RevisaExpiracion" + "|" + "Revisa que no este expirado el usaurio." + "|" + datosUsuario.id_usuario);
                if (busquedaUsuario.RevisaExpiracion(datosUsuario.id_usuario))
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgExpira, Resources.resCorpusCFDIEs.varContribuyente);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/webGlobalError.aspx");
        }
    }

    protected void btnValidar_Click(object sender, EventArgs e)
    {
        //Crear Objetos
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
        tabla = busquedaUsuario.buscarUsuario(datosUsuario.userName);
        sEstadoActual = datosUsuario.estatus;

        if (tabla.Rows.Count > 0)
        {
            //Recupera y desencripta la contraseña del usuario.
            if (txtContraseniaAnterior.Text == Utilerias.Encriptacion.Base64.DesencriptarBase64(tabla.Rows[0]["password"].ToString()))
            {
                //Encripta la nueva contraseña.
                sPassword = Utilerias.Encriptacion.Base64.EncriptarBase64(txtConfirmaNueva.Text.Trim());

                //Revisa que no sea igual en nombre de usuario a contraseña.
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "RevisarNOmbre" + "|" + "Revisa que no sea igual en nombre de usuario a contraseña.");
                if (datosUsuario.userName != txtConfirmaNueva.Text.Trim())
                {
                    //Revisar que minimo no este la contraseña en tres ocasiones
                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "RevisaPassRepetido" + "|" + "Revisar que minimo no este la contraseña en tres ocasiones.");
                    if (!busquedaUsuario.RevisaPassRepetido(datosUsuario.id_usuario, sPassword))
                    {
                        //Actualiza contraseña del usuario en BD
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "actualizaContraseña" + "|" + "Actualiza contraseña del usuario en BD.");
                        if (busquedaUsuario.actualizaContraseña(datosUsuario.userName, datosUsuario.email, sPassword))
                        {
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "RevisaEstatus" + "|" + "Revisa el estado actual, para activarlo.");
                            switch (sEstadoActual)
                            {
                                case 'P':

                                    if (busquedaUsuario.actualizaEstadoActual(datosUsuario.userName, datosUsuario.email, 'A'))
                                    {
                                        if (sRedireccion != string.Empty)
                                        {
                                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "actualizaContraseñaCaducidad" + "|" + "Actualiza la contraseña del usuario.");
                                            busquedaUsuario.actualizaContraseñaCaducidad(datosUsuario.id_usuario, 'A', sPassword);
                                            datosUsuario.Actualizar();
                                            Response.Redirect(sRedireccion);
                                        }
                                    }

                                    break;
                                case 'C':

                                    if (busquedaUsuario.actualizaEstadoActual(datosUsuario.userName, datosUsuario.email, 'A'))
                                    {
                                        if (sRedireccion != string.Empty)
                                        {
                                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "actualizaContraseñaCaducidad" + "|" + "Actualiza la contraseña del usuario.");
                                            busquedaUsuario.actualizaContraseñaCaducidad(datosUsuario.id_usuario, 'B', sPassword);
                                            datosUsuario.Actualizar();
                                            Session.Abandon();
                                            FormsAuthentication.SignOut();
                                            Response.Redirect(sRedireccion);
                                        }
                                    }

                                    break;

                                case 'E':

                                    if (busquedaUsuario.actualizaEstadoActual(datosUsuario.userName, datosUsuario.email, 'A'))
                                    {
                                        if (sRedireccion != string.Empty)
                                        {
                                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "actualizaContraseñaCaducidad" + "|" + "Actualiza la contraseña del usuario.");
                                            busquedaUsuario.actualizaContraseñaCaducidad(datosUsuario.id_usuario, 'B', sPassword);
                                            //Actualiza la fecha de cambio
                                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "actualizaFechaCambio" + "|" + "Actualiza la fecha de cambio del usuario.");
                                            busquedaUsuario.actualizaFechaCambio(datosUsuario.id_usuario, string.Empty, string.Empty);
                                            datosUsuario.Actualizar();
                                            Response.Redirect(sRedireccion);
                                        }
                                    }

                                    break;

                                case 'A':

                                    //if (bTerminarSesion)
                                    //{
                                    if (sRedireccion != string.Empty)
                                    {
                                        busquedaUsuario.actualizaContraseñaCaducidad(datosUsuario.id_usuario, 'B', sPassword);
                                        //Actualiza la fecha de cambio
                                        busquedaUsuario.actualizaFechaCambio(datosUsuario.id_usuario, string.Empty, string.Empty);
                                        datosUsuario.Actualizar();
                                        Session.Abandon();
                                        FormsAuthentication.SignOut();
                                        Response.Redirect(sRedireccion);
                                    }
                                    //    }

                                    break;
                            }
                        }
                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgPassUsado, Resources.resCorpusCFDIEs.varContribuyente);
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
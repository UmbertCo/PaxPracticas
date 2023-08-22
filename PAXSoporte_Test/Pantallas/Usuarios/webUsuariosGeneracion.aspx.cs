using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Threading;
using System.Globalization;

public partial class Pantallas_Usuarios_webUsuariosGeneracion : System.Web.UI.Page
{
    private clsUsuarios gDAL;
    private clsIncidencias gINS;
    private clsBusquedaIncidentes gPRO;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
        if (!IsPostBack)
        { 
            gDAL = new clsUsuarios ();
            //manda llamar el metodo que llena el grid con los usuarios de soporte
            fnCargarUsuarios();
            fnCargaTipoIncidencias();
           // fnCargaPerfiles();
        }

    }

    private void fnCargaPerfiles()
    {       
        try             
        {
            gDAL = new clsUsuarios();
            ddlPerfil.DataSource = gDAL.fnLlenaPerfiles();
            ddlPerfil.DataTextField = "desc_perfil";
            ddlPerfil.DataValueField = "id_perfil";
            ddlPerfil.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaPerfiles", "webUsuariosGeneracion.aspx.cs");
            //gdvUsuarios.DataSource = null;
            //gdvUsuarios.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaPerfiles", "webUsuariosGeneracion.aspx.cs");
        }
    }

    private void fnCargarUsuarios()
    {
        gDAL = new clsUsuarios();

        try
        {
            gdvUsuarios.DataSource = gDAL.fnLlenarGridUsuariosSoporte();
            gdvUsuarios.DataBind();
            gdvUsuarios.Enabled = true;
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargarUsuarios", "webUsuariosGeneracion.aspx.cs");
            gdvUsuarios.DataSource = null;
            gdvUsuarios.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargarUsuarios", "webUsuariosGeneracion.aspx.cs");
        }
    }

    private void fnCargaTipoIncidencias()
    {
        try
        {
            gDAL = new clsUsuarios();
            ddlIncidencia.DataSource = gDAL.fnCargarCatalogoTipoIncidencias();
            ddlIncidencia.DataTextField = "tipo_incidente";
            ddlIncidencia.DataValueField="id_tipo_incidente";
            ddlIncidencia.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaTipoIncidencias", "webUsuariosGeneracion.aspx.cs");
            //gdvUsuarios.DataSource = null;
            //gdvUsuarios.DataBind();           
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaTipoIncidencias", "webUsuariosGeneracion.aspx.cs");
        }
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
        //guarda la informacion del usuario        
    {
        string stxtcorreo = txtCorreo.Text;
        if (string.IsNullOrEmpty(txtNombre.Text)
           || string.IsNullOrEmpty(txtCorreo.Text)
           || string.IsNullOrEmpty(txtUsuario.Text)
           || ddlIncidencia.SelectedValue == null
           )
        {

            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }
        else
        {
            
            if (!clsComun.fnValidaExpresion(stxtcorreo, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.txtCorreo);
                return;
            }
                try
                {
                    gDAL = new clsUsuarios();
                    
                        string status = "A";

                        int psIncidencia = Convert.ToInt32(ddlIncidencia.SelectedValue);
                        //int psIdPerfil = Convert.ToInt32(ddlPerfil.SelectedValue);
                        int psUsuarioId;
                        //

                        //

                        if ((Session["psIdUsuario"] != null))
                        {
                             DataTable Usu = gDAL.fnObtenerUsuariobyUsuario(txtUsuario.Text);

                          
                            string sPassword = GeneradorPassword.GetPassword();
                            byte[] psPass = PAXCrypto.CryptoAES.EncriptaAES(sPassword);
                            psUsuarioId = Convert.ToInt32(Session["psIdUsuario"]);
                            gDAL.fnEliminaRelacionUsuarioTipoIncidente(psUsuarioId);
                            gDAL.fnGuardaInformacionUsuarioSoporte(psUsuarioId, txtNombre.Text, PAXCrypto.CryptoAES.EncriptaAES(txtCorreo.Text), psPass, status, psIncidencia, txtUsuario.Text);


                            foreach (ListItem seleccion in ddlIncidencia.Items)
                            {
                                if (seleccion.Selected == true)
                                {
                                    gDAL.fnInsertaRelacionUsuarioTipoIncidente(psUsuarioId, Convert.ToInt32(seleccion.Value));
                                }
                      

                            // gDAL.fnInsertaRelacionPerfilTipoIncidente(psIdPerfil, psIncidencia);
                            // gDAL.fnActualizaRelacionUsuarioPerfil(psIdPerfil, psUsuarioId);
                             }
                            fnCargarUsuarios();
                            fnLimpiaCampos();
                            btnGuardar.Enabled = false;
                            fnLimpiarCheckboxList();
                        }

                        else
                        {
                            DataTable Usu = gDAL.fnObtenerUsuariobyUsuario(txtUsuario.Text);

                    if (Usu.Rows.Count == 0)
                    {
                            psUsuarioId = 0;
                            //
                            string sPassword = GeneradorPassword.GetPassword();
                            byte[] psPass = PAXCrypto.CryptoAES.EncriptaAES(sPassword);
                            clsGeneraEMAIL email = new clsGeneraEMAIL();

                            string strMensaje = "<table>";
                            strMensaje = strMensaje + "<tr><td><b>Estimado usuario del sistema de mesa de ayuda</b></td><td>Se le ha enviado un correo para la recuperación de la contraseña</td></tr>";
                            strMensaje = strMensaje + "<tr><td><b>Usuario:</b></td><td>" + txtUsuario.Text + "</td></tr>";
                            strMensaje = strMensaje + "<tr><td><b>Contraseña temporal:</b></td><td>" + sPassword + "</td></tr>";
                            strMensaje = strMensaje + "</table>";
                            //
                            psUsuarioId = gDAL.fnGuardaInformacionUsuarioSoporte(psUsuarioId, txtNombre.Text, PAXCrypto.CryptoAES.EncriptaAES(txtCorreo.Text), psPass, status, psIncidencia, txtUsuario.Text);
                            gDAL.fnActualizaPasswordUsuario(txtUsuario.Text, sPassword, "C");
                            email.fnEnviarCorreoAtencionIncidencia(txtCorreo.Text, "Correo de recuperación de contraseña", strMensaje, clsComun.fnObtenerParamentro("emailAppFrom"));

                            //gDAL.fnInsertaRelacionPerfilTipoIncidente(psIdPerfil, psIncidencia);
                            //gDAL.fnInsertaRelacionUsuarioPerfil(psIdPerfil, psUsuarioId);
                            foreach (ListItem seleccion in ddlIncidencia.Items)
                            {
                                if (seleccion.Selected == true)
                                {
                                    gDAL.fnInsertaRelacionUsuarioTipoIncidente(psUsuarioId, Convert.ToInt32(seleccion.Value));
                                }
                            }
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEnvioCorreo);
                            fnCargarUsuarios();
                            //fnLimpiaCampos();
                            fnDesbloqueaTextos(1);
                        }                        
                             else
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarErrorUsuNue);  
                    }

                    }
                   
                    
                }
                   
                catch (Exception ex)
                {
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnGuardar_Click", "webUsuariosGeneracion.aspx.cs");
                }
         
        }
    }
    protected void gdvUsuarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvUsuarios.PageIndex = e.NewPageIndex;
        fnCargarUsuarios();
    }
    protected void gdvUsuarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //ScriptManager SM = ScriptManager.GetCurrent(this);
        //SM.RegisterPostBackControl(gdvUsuarios);

        //cancelamos la acción por defecto
        e.Cancel = false;
        gDAL = new clsUsuarios();
        gINS = new clsIncidencias();
        gPRO = new clsBusquedaIncidentes();
        try
        {
            //Obtenemos el ID de la fila seleccionada
            int psid_usuario_soporte = Convert.ToInt32(e.Keys["id_usuario_soporte"].ToString());
            DataTable dtInc = null;
            DataTable dtPro = null;
            dtInc = gINS.fnObtieneTicketsIncidenciaporUsuario(psid_usuario_soporte);
            dtPro = gPRO.fnObtieneTicketsProblemaporUsuario(psid_usuario_soporte);
            if (dtInc.Rows.Count == 0)
            {
                if (dtPro.Rows.Count == 0)
                {
                    gDAL.fnBajaUsuarioSoporte(psid_usuario_soporte);
                    fnCargarUsuarios();
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblMsjPro);  
                    //modalCreditos.Show();
                }                
            }
            else
            {
                //modalCreditos.Show();
               clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblMsjInc);  
            }
            
        }       
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "gdvUsuarios_RowDeleting", "webUsuariosGeneracion.aspx.cs");
        }
    }
    protected void gdvUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
         gDAL = new clsUsuarios();
        //obtener los valores del usuario seleccionado en el grid
        GridViewRow gvrFila = (GridViewRow)gdvUsuarios.SelectedRow;       
        Session["psIdUsuario"] = Convert.ToInt32(gdvUsuarios.SelectedDataKey.Value);
        DataTable dsTipos = null;
        fnLimpiarCheckboxList();
        dsTipos = gDAL.fnObtenerTipoIncidentesporUsuario(Convert.ToInt32(Session["psIdUsuario"]));
     
        foreach (DataRow renglon in dsTipos.Rows)
        {
            for (int i = 0; i < ddlIncidencia.Items.Count; i++)
            {
                int tipoinc = Convert.ToInt32(renglon["id_tipo_incidente"]);
                if (tipoinc == Convert.ToInt32(ddlIncidencia.Items[i].Value))
                {
                    ddlIncidencia.Items[i].Selected = true;
                    break;
                }
            }
        }
      
        ddlPerfil.SelectedIndex = ddlPerfil.Items.IndexOf(ddlPerfil.Items.FindByValue("lblidperfil"));
        txtCorreo.Text = ((Label)gvrFila.FindControl("lblEmail")).Text;
        txtNombre.Text = ((Label)gvrFila.FindControl("lblnombreusuario")).Text;
        txtUsuario.Text = ((Label)gvrFila.FindControl("lblusuariosopg")).Text;
        txtUsuario.ReadOnly = true;
        btnCambiarPwd.Enabled = true;
        fnDesbloqueaTextos(0);
        btnGuardar.Enabled = true;
    }
    private void fnLimpiarCheckboxList()
    {
        for (int i = 0; i < ddlIncidencia.Items.Count; i++)
        {
            ddlIncidencia.Items[i].Selected = false;
        }
    }
    private void fnLimpiaCampos()
    {
     txtCorreo.Text = string.Empty;
     txtNombre.Text= string.Empty;
     txtUsuario.Text= string.Empty;
     txtUsuario.ReadOnly = false;
     txtPassword.Text= string.Empty;
     txtConfirmaPwd.Text = string.Empty;
     ddlIncidencia.SelectedIndex = 1;
    }


    protected void btnNuevoUsuario_Click(object sender, EventArgs e)
    {
        Session["psIdUsuario"] = null;
        fnLimpiaCampos();
        fnLimpiarCheckboxList();
        btnGuardar.Enabled = true;
        fnDesbloqueaTextos(0);
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string style = @"<style> .text { mso-number-format:\@; } </script> ";
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=CatalgoUsuarios" + DateTime.Today + ".xls");
        Response.ContentType = "application/excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        gdvUsuarios.RenderControl(htw); //donde gdvUsuarios es el nombre del gridview
        // Style is added dynamically
        Response.Write(style);
        Response.Write(sw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btnValidar_Click(object sender, EventArgs e)
    {
        //Crear Objetos
        int psIdUsua = Convert.ToInt32(Session["psIdUsuario"]);
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
        //datosUsuario = clsComun.fnUsuarioEnSesion();

        //Recupera datos de BD
        tabla = busquedaUsuario.fnBuscarUsuario(txtUsuario.Text);
        sEstadoActual = 'A';

        if (tabla.Rows.Count > 0)
        {
            //Recupera y desencripta la contraseña del usuario.
            if (txtContraseniaAnterior.Text == PAXCrypto.CryptoAES.DesencriptaAES((byte[])tabla.Rows[0]["password"]))
            {
                //Encripta la nueva contraseña.
                //sPassword=Utilerias.Encriptacion.Classica.Encriptar(txtConfirmaNueva.Text.Trim());

                //Revisa que no sea igual en nombre de usuario a contraseña.
                clsPistasAuditoria.fnGenerarPistasAuditoria(psIdUsua, DateTime.Now, "usrGlobalPwd" + "|" + "RevisarNOmbre" + "|" + "Revisa que no sea igual en nombre de usuario a contraseña.");
                if (txtUsuario.Text != txtConfirmaNueva.Text.Trim())
                {
                    //Revisar que minimo no este la contraseña en tres ocasiones
                   // clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "RevisaPassRepetido" + "|" + "Revisar que minimo no este la contraseña en tres ocasiones.");
                    //if (!busquedaUsuario.RevisaPassRepetido(datosUsuario.id_usuario, sPassword))
                    //{
                    //    //Actualiza contraseña del usuario en BD
                   
                    clsPistasAuditoria.fnGenerarPistasAuditoria(psIdUsua, DateTime.Now, "usrGlobalPwd" + "|" + "actualizaContraseña" + "|" + "Actualiza contraseña del usuario en BD.");
                       // if ()
                    string psStaturs = "A";
                   bool psConfirmacion = false;
                   psConfirmacion = gDAL.fnActualizaPasswordUsuario(txtUsuario.Text, txtConfirmaNueva.Text.Trim(), psStaturs);
                   //Response.Redirect(sRedireccion);
               
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
    protected void btnCambiarPwd_Click(object sender, EventArgs e)
    {
        mpePanel.Show();
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

    public void fnDesbloqueaTextos(int bloqueo)
    {
        if (bloqueo == 0)
        {
            txtNombre.Enabled = true;
            txtCorreo.Enabled = true;
            txtUsuario.Enabled = true;
        }
        else
        {
            txtNombre.Enabled = false;
            txtCorreo.Enabled = false;
            txtUsuario.Enabled = false;
        }
    }
  
protected void  btnAcepCreditos_Click(object sender, EventArgs e)
{

}
protected void gdvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
{

}
}
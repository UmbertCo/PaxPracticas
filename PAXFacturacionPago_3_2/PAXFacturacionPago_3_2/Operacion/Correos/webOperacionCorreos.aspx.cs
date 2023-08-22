using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;

public partial class Operacion_Correos_webOperacionCorreos : System.Web.UI.Page
{
     private clsOperacionCorreos gDAL;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        clsComun.fnPonerTitulo(this);
        if (!IsPostBack)
        {
            fnCargaCorreos();
        }
    }
    protected void btnNuevoUsuario_Click(object sender, EventArgs e)
    {
        Session["psIdCorreo"] = null;
        fnLimpiaCampos();
        fnLimpiarCheckboxList();
        btnGuardar.Enabled = true;
        fnDesbloqueaTextos(0);
    }

    private void fnLimpiaCampos()
    {
        txtCorreo.Text = string.Empty;
        ddlEstatus.SelectedIndex = 1;
    }
    protected void btnCambiarPwd_Click(object sender, EventArgs e)
    {
        mpePanel.Show();
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
         string stxtcorreo = txtCorreo.Text;
         if (string.IsNullOrEmpty(txtCorreo.Text)
            || ddlEstatus.SelectedValue == null
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
         }

         try
         {
             gDAL = new clsOperacionCorreos();
             DataTable RFCTable = new DataTable();
             RFCTable = gDAL.ObtenerInfoContribuyenteRFC(txtRFC.Text);

             //string Password = Utilerias.Encriptacion.Classica.Desencriptar(txtPass.Text);             
             string Password = PAXCrypto.CryptoAES.DesencriptaAES64(txtPass.Text);
            
             if ((Session["psIdCorreo"] != null))
             {
                  
                 string psIdCorreo = Convert.ToString(Session["psIdCorreo"]);
                 
                
                 foreach (DataRow renglon in RFCTable.Rows)
                 {
                     int idContribuyente = Convert.ToInt32(renglon["id_contribuyente"]);
                     gDAL.fnCorreoInsUpd(psIdCorreo, txtCorreo.Text, null, ddlEstatus.SelectedValue, idContribuyente);
                     fnCargaCorreos();
                 }

             }
             else
             {
                 foreach (DataRow renglon in RFCTable.Rows)
                 {
                     int idContribuyente = Convert.ToInt32(renglon["id_contribuyente"]);
                     gDAL.fnCorreoInsUpd(null, txtCorreo.Text, Password, ddlEstatus.SelectedValue, idContribuyente);
                     fnCargaCorreos();
                 }
             }
         }
         catch(Exception ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
         }
    }

    private void fnLimpiarCheckboxList()
    {
        for (int i = 0; i < ddlEstatus.Items.Count; i++)
        {
            ddlEstatus.Items[i].Selected = false;
        }
    }

    public void fnDesbloqueaTextos(int bloqueo)
    {
        if (bloqueo == 0)
        {            
            txtCorreo.Enabled = true;         
        }
        else
        {            
            txtCorreo.Enabled = false;            
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string style = @"<style> .text { mso-number-format:\@; } </script> ";
        Response.ClearContent();
        Response.AddHeader("content-disposition", "attachment; filename=CatalgoUsuarios" + DateTime.Today + ".xls");
        Response.ContentType = "application/excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        gdvCorreos.RenderControl(htw); //donde gdvUsuarios es el nombre del gridview
        // Style is added dynamically
        Response.Write(style);
        Response.Write(sw.ToString());
        Response.End();
    }
    protected void btnValidar_Click(object sender, EventArgs e)
    {
        //Crear Objetos
        int psIdCorreo = Convert.ToInt32(Session["psIdCorreo"]);         
        gDAL = new clsOperacionCorreos();
   
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
        tabla = gDAL.ObtenerInfoCorreo(psIdCorreo); 
        sEstadoActual = 'A';

        if (tabla.Rows.Count > 0)
        {
            //Recupera y desencripta la contraseña del usuario.
            //if (txtContraseniaAnterior.Text == Utilerias.Encriptacion.Classica.Desencriptar(tabla.Rows[0]["password"].ToString()))
            if (txtContraseniaAnterior.Text == PAXCrypto.CryptoAES.DesencriptaAES((byte[])tabla.Rows[0]["password"]))
            {
                //Encripta la nueva contraseña.
                //sPassword=Utilerias.Encriptacion.Classica.Encriptar(txtConfirmaNueva.Text.Trim());

                //Revisa que no sea igual en nombre de usuario a contraseña.
                clsPistasAuditoria.fnGenerarPistasAuditoria(psIdCorreo, DateTime.Now, "usrGlobalPwd" + "|" + "RevisarCorreo" + "|" + "Revisa que no sea igual el correo a la contraseña.");
                if (txtCorreo.Text != txtConfirmaNueva.Text.Trim())
                {
                    //Revisar que minimo no este la contraseña en tres ocasiones
                    // clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "usrGlobalPwd" + "|" + "RevisaPassRepetido" + "|" + "Revisar que minimo no este la contraseña en tres ocasiones.");
                    //if (!busquedaUsuario.RevisaPassRepetido(datosUsuario.id_usuario, sPassword))
                    //{
                    //    //Actualiza contraseña del usuario en BD

                    clsPistasAuditoria.fnGenerarPistasAuditoria(psIdCorreo, DateTime.Now, "usrGlobalPwd" + "|" + "actualizaContraseña" + "|" + "Actualiza contraseña del correo del usuario en BD.");
                    // if ()
                    string psStaturs = "A";
                    bool psConfirmacion = false;
                    psConfirmacion = gDAL.fnActualizaPasswordUsuario(psIdCorreo, txtConfirmaNueva.Text.Trim(), psStaturs);
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

    private void fnCargaCorreos()
    {
        try
    {
        gDAL = new clsOperacionCorreos();
        gdvCorreos.DataSource = gDAL.fnBuscaCorreoConf();
        gdvCorreos.DataBind();
        gdvCorreos.Enabled = true;
    }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            gdvCorreos.DataSource = null;
            gdvCorreos.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void gdvCorreos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gdvCorreos.PageIndex = e.NewPageIndex;
            fnCargaCorreos();
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void gdvCorreos_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gdvCorreos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            gDAL = new clsOperacionCorreos();
            //cancelamos la acción por defecto
            e.Cancel = false;
            //Obtenemos el ID de la fila seleccionada
            int psidCorreo = Convert.ToInt32(e.Keys["idCorreo"].ToString());
            gDAL.fnBajaCorreo(psidCorreo);
            fnCargaCorreos();

        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

    }
    protected void gdvCorreos_SelectedIndexChanged(object sender, EventArgs e)
    {
         gDAL = new clsOperacionCorreos();
        GridViewRow gvrFila = (GridViewRow)gdvCorreos.SelectedRow;
        Session["psIdCorreo"] = Convert.ToInt32(gdvCorreos.SelectedDataKey.Value);
        DataTable dtInfo = new DataTable();
        dtInfo = gDAL.ObtenerInfoCorreo(Convert.ToInt32(gdvCorreos.SelectedDataKey.Value));
        foreach (DataRow renglon in dtInfo.Rows)
        {
            DataTable Contribuyente = new DataTable();
            Contribuyente = gDAL.ObtenerInfoCorreoContribuyente(Convert.ToInt32(renglon["id_usuario"]));
            txtCorreo.Text = Convert.ToString(renglon["CorreoElectronico"]);
            txtRFC.Text = Convert.ToString(Contribuyente.Rows[0]["rfc"]);
            ddlEstatus.SelectedValue = Convert.ToString(renglon["Estatus"]);
            btnGuardar.Enabled = true;
            btnCambiarPwd.Enabled = true;
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
}
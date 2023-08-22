using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Globalization;

public partial class Operacion_RFC_webOperacionRfc : System.Web.UI.Page
{
    clsInicioSesionUsuario datosUsuario;
    private clsOperacionRFC  gRFC;
    private clsOperacionCuenta gDAL;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Account/Login.aspx");
        }
        else
        {
            string sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            if (!new clsOperacionUsuarios().fnObtenerPermisoPagina(sPagina))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varPermisos);
                Response.Redirect("~/Default.aspx");
            }

        }
        //DataTable tblVersiones = new DataTable();
        try
        {
            if (!IsPostBack)
            {
                datosUsuario = clsComun.fnUsuarioEnSesion();
                fnCargaDatos();
                //fnCargarSucursales();
                //fnCargaUsuarios(datosUsuario.id_usuario, Convert.ToInt32(ddlSucursales.SelectedValue));
                /*tblVersiones = clsInicioSesionRegistroDatos.fnRecuperaVersionesVigentes("");
                int id_version = Convert.ToInt32(tblVersiones.Rows[0]["id_version"]);
                drpVersion.DataSource = tblVersiones;
                drpVersion.DataBind();
                drpVersion.Enabled = false;*/
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    //public void fnCargaUsuarios(int pidUsuario, int pIdSucursales)
    //{
    //    datosUsuario = clsComun.fnUsuarioEnSesion();
    //    gRFC = new clsOperacionRFC();
    //    DataTable dtUsuarios = new DataTable();
    //    dtUsuarios = gRFC.fnObtieneUsuariosporPadre(pidUsuario, pIdSucursales);
    //    ddlUsuarios.DataSource = dtUsuarios;
    //    ddlUsuarios.DataBind();
      
    //}

    public void fnCargaDatos()
    {
        //datosUsuario = clsComun.fnUsuarioEnSesion();
        gRFC = new clsOperacionRFC();
        DataTable dtRFC = new DataTable();
        dtRFC = gRFC.fnObtenerRFCs();
        gdvRFCs.DataSource = dtRFC;
        gdvRFCs.DataBind();
      
    }

    protected void btnGuardarActualizar_Click(object sender, EventArgs e)
    {

        try
        {
            System.Threading.Thread.Sleep(1000);

            if (string.IsNullOrEmpty(txtNombre.Text)
          || (string.IsNullOrEmpty(txtRFC.Text))
          )
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError);
                return;
            }

            gRFC = new clsOperacionRFC();
            clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
            byte[] Logo = { };

            if ((Session["idRFC"] == null || Convert.ToInt32(Session["idRFC"]) == 0))
            {

                gDAL = new clsOperacionCuenta();
                clsValCertificado vValidadorCertificado = null;
                string resValidacion = string.Empty;

                //Verificamos que el archivo de llave privada sea un .key
                if (Path.GetExtension(fupKey.FileName).ToLower() != ".key")
                {
                    lblAviso.Text = Resources.resCorpusCFDIEs.valExtensionKey;
                    mpeAvisos.Show();
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valExtensionKey);
                    return;
                }
                //verificamos que el archivo de certificado público sea un .cer
                if (Path.GetExtension(fupCer.FileName).ToLower() != ".cer")
                {
                    lblAviso.Text = Resources.resCorpusCFDIEs.valExtensionCer;
                    mpeAvisos.Show();
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valExtensionCer);
                    return;
                }
                else
                {
                    try
                    {
                        //realizamos las validaciones de SAT sobre el archivo
                        vValidadorCertificado = new clsValCertificado(fupCer.FileBytes);
                        vValidadorCertificado.LlavePrivada = new clsOperacionTimbradoSellado(fupKey.FileBytes, txtPass.Text);
                        resValidacion = vValidadorCertificado.ValidarCertificado(txtRFC.Text);

                        //if (!string.IsNullOrEmpty(resValidacion))
                        //{
                        //    clsComun.fnMostrarMensaje(this, resValidacion);
                        //    return;
                        //}
                        //else
                        //{
                        datosUsuario = clsComun.fnUsuarioEnSesion();
                        gRFC = new clsOperacionRFC();
                        int idRFC = gRFC.fnInsertaRFC(txtRFC.Text, "A", txtNombre.Text, 2);
                        Session["idRFC"] = idRFC;
                        if (idRFC != 0)
                        {
                            //Una vez validados ambos archivos y su password los guardamos en la BD
                            int retVal = gDAL.fnGuardarCertificadosCobro(idRFC.ToString(), vValidadorCertificado);

                            if (retVal != 0)
                            {
                                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);
                                lblAviso.Text = Resources.resCorpusCFDIEs.varAlta;
                                mpeAvisos.Show();

                                clsComun.fnNuevaPistaAuditoria(
                                    "webGlobalCuenta",
                                    "fnGuardarCertificados",
                                    "Se modificaron los certificados para el RFC con ID " + txtRFC.Text
                                    );
                                fnCargaDatos();

                            }
                            else
                                throw new Exception(Resources.resCorpusCFDIEs.varNoAcReg);// "No se actualizó registro alguno.");
                        }
                            lblAviso.Text = Resources.resCorpusCFDIEs.varAlta;
                            mpeAvisos.Show();

                        //}
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                        lblErrorLog.Text = Resources.resCorpusCFDIEs.varErrorCambio;
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
                        mpeErrorLog.Show();
                    }

                }

            }
            else
            {
                int idRFC = Convert.ToInt32(Session["idRFC"]);
                datosUsuario = clsComun.fnUsuarioEnSesion();
                gRFC = new clsOperacionRFC();

               
                gRFC.fnInsertaRFC(txtRFC.Text, "A", txtNombre.Text, 2);

                if (fupCer.FileName != "" && fupKey.FileName != "" && txtPass.Text != "")
                {
                    gDAL = new clsOperacionCuenta();
                    clsValCertificado vValidadorCertificado = null;
                    string resValidacion = string.Empty;

                    //Verificamos que el archivo de llave privada sea un .key
                    if (Path.GetExtension(fupKey.FileName).ToLower() != ".key")
                    {
                        lblAviso.Text = Resources.resCorpusCFDIEs.valExtensionKey;
                        mpeAvisos.Show();
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valExtensionKey);
                        return;
                    }
                    //verificamos que el archivo de certificado público sea un .cer
                    if (Path.GetExtension(fupCer.FileName).ToLower() != ".cer")
                    {
                        lblAviso.Text = Resources.resCorpusCFDIEs.valExtensionCer;
                        mpeAvisos.Show();
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valExtensionCer);
                        return;
                    }
                    else
                    {
                        try
                        {
                            //realizamos las validaciones de SAT sobre el archivo
                            vValidadorCertificado = new clsValCertificado(fupCer.FileBytes);
                            vValidadorCertificado.LlavePrivada = new clsOperacionTimbradoSellado(fupKey.FileBytes, txtPass.Text);
                            resValidacion = vValidadorCertificado.ValidarCertificado(txtRFC.Text);

                            //if (!string.IsNullOrEmpty(resValidacion))
                            //{
                            //    clsComun.fnMostrarMensaje(this, resValidacion);
                            //    return;
                            //}
                        }
                        catch (Exception ex)
                        {
                            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                        }

                        //Una vez validados ambos archivos y su password los guardamos en la BD
                        try
                        {
                            int retVal = gDAL.fnGuardarCertificadosCobro(idRFC.ToString(), vValidadorCertificado);

                            if (retVal != 0)
                            {
                                lblAviso.Text = Resources.resCorpusCFDIEs.varCambio;
                                mpeAvisos.Show();
                                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);
                                clsComun.fnNuevaPistaAuditoria(
                                    "webGlobalCuenta",
                                    "fnGuardarCertificados",
                                    "Se modificaron los certificados para el RFC con ID " + txtRFC.Text
                                    );
                                fnCargaDatos();

                            }
                            else
                                throw new Exception(Resources.resCorpusCFDIEs.varNoAcReg);// "No se actualizó registro alguno.");
                        }
                        catch (SqlException ex)
                        {
                            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
                            lblErrorLog.Text = Resources.resCorpusCFDIEs.varErrorCambio;
                            mpeErrorLog.Show();
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
                        }
                        catch (Exception ex)
                        {
                            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                            lblErrorLog.Text = Resources.resCorpusCFDIEs.varErrorCambio;
                            mpeErrorLog.Show();
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
                        }

                    }
                }
                fnCargaDatos();
                valCer.ValidationGroup = "RegisterUserValidationGroup";
                rfvCer.ValidationGroup = "RegisterUserValidationGroup";
                rfvKey.ValidationGroup = "RegisterUserValidationGroup";
                regxKey.ValidationGroup = "RegisterUserValidationGroup";
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);
                lblAviso.Text = Resources.resCorpusCFDIEs.varCambio;
                mpeAvisos.Show();
            }

            fnCargaDatos();
            gdvRFCs.SelectedIndex = -1;
            btnNuevoUsuario_Click(sender, e);

            txtRFC.Text = String.Empty;
            txtNombre.Text = String.Empty;
            Session["idRFC"] = null;
            txtRFC.Enabled = false;
            txtNombre.Enabled = false;
            fupCer.Enabled = false;
            fupKey.Enabled = false;
            btnGuardarActualizar.Visible = false;
            txtPass.Enabled = false;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
   
    protected void btnNuevoUsuario_Click(object sender, EventArgs e)
    {
        txtRFC.Text = String.Empty;
        txtNombre.Text = String.Empty;
        Session["idRFC"] = null;
        txtRFC.Enabled = true;
        txtNombre.Enabled = true;
        fupCer.Enabled = true;
        fupKey.Enabled = true;
        //drpVersion.Enabled = true;
       // fupLogo.Enabled = true;
        txtPass.Enabled = true;
        //ddlSucursales.Enabled = true;
        btnGuardarActualizar.Visible = true;
        //ddlUsuarios.Enabled = true;

        gdvRFCs.SelectedIndex = -1;
    }

    protected void gdvRFCs_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {           
            GridViewRow gvrFila = (GridViewRow)gdvRFCs.SelectedRow;
            Session["idRFC"] = Convert.ToInt32(gdvRFCs.SelectedDataKey.Value);
         
            txtRFC.Text = ((Label)gvrFila.FindControl("lblrcf")).Text;
            txtNombre.Text = ((Label)gvrFila.FindControl("lblrazonsocial")).Text;
            //drpVersion.SelectedValue = ((Label)gvrFila.FindControl("lblidversion")).Text;
           // ddlSucursales.SelectedValue = ((Label)gvrFila.FindControl("lblidestructura")).Text;
           // ddlSucursales_SelectedIndexChanged(sender, e);
           // ddlUsuarios.SelectedValue = ((Label)gvrFila.FindControl("lblidcontribuyente")).Text;
           // ddlUsuarios.Enabled = false;
            //string lblidpadre = ((Label)gvrFila.FindControl("lblidpadre")).Text;
            //if (lblidpadre == "" || lblidpadre == String.Empty)
            //{
            //    txtRFC.Enabled = false;
            //    txtNombre.Enabled = false;
            //    fupCer.Enabled = false;
            //    fupKey.Enabled = false;
               
            //    //drpVersion.Enabled = false;
            //   // ddlSucursales.Enabled = false;
            //    txtPass.Enabled = false;
            //  //  ddlUsuarios.Enabled = false;
            //  //  fupLogo.Enabled = true;
            //}
            //else
            //{
                txtNombre.Enabled = true;
                fupCer.Enabled = true;
                fupKey.Enabled = true;
              
                //drpVersion.Enabled = true;
               // ddlSucursales.Enabled = false;
                txtPass.Enabled = true;
               // ddlUsuarios.Enabled = false;
               // fupLogo.Enabled = true;
            //}
         
            btnGuardarActualizar.Visible = true;
            valCer.ValidationGroup = "Edicion";
            rfvCer.ValidationGroup = "Edicion";
            rfvKey.ValidationGroup = "Edicion";
            regxKey.ValidationGroup = "Edicion";
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void gdvRFCs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    gRFC = new clsOperacionRFC();
        //    Label IdRFC = (Label)e.Row.FindControl("lblidrfc");
        //    MemoryStream ms = new MemoryStream(gRFC.fnObtenerImagenRFC(Convert.ToInt32(IdRFC.Text)));
        //    if (ms.Length > 0)
        //    {
        //        e.Row.Cells[1].Enabled = false;               
        //    }
        //}   
    }

    protected void gdvRFCs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gRFC = new clsOperacionRFC();
        int nIdRFC = Convert.ToInt32(e.Keys["id_rfc"].ToString());
        string sRfc = e.Keys["rfc"].ToString();
       
        if ( gRFC.fnVerificarRfcCfd(nIdRFC))
        {
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblEliminarRFC);
            lblAviso.Text = Resources.resCorpusCFDIEs.lblEliminarRFC;
            mpeAvisos.Show();
        }
        else
        {
            gRFC.fnEliminaRFCbyId(nIdRFC);
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);
            lblAviso.Text = Resources.resCorpusCFDIEs.varBaja;
            mpeAvisos.Show();
            fnCargaDatos();
        }
        
        
    }

    ///// <summary>
    ///// Trae la lista de sucursales activas asignadas al usuario y las carga en el GridView
    ///// </summary>
    //private void fnCargarSucursales()
    //{
    //    try
    //    {
    //        DataTable tblEstructura = new DataTable();
    //        datosUsuario = clsComun.fnUsuarioEnSesion();
    //        tblEstructura = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(false);
    //        ddlSucursales.DataSource = tblEstructura;
    //        ViewState["tblEstructura"] = tblEstructura;
    //        ddlSucursales.DataBind();
    //    }
    //    catch (SqlException ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

    //    }
    //    catch
    //    {
    //        //referencia nula
    //    }
    //}

    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        txtRFC.Text = String.Empty;
        txtNombre.Text = String.Empty;
        Session["idRFC"] = null;
        txtRFC.Enabled = false;
        txtNombre.Enabled = false;
        fupCer.Enabled = false;
        fupKey.Enabled = false;
        //fupLogo.Enabled = false;
        //ddlUsuarios.Enabled = false;
        //ddlSucursales.Enabled = false;
        btnGuardarActualizar.Visible = false;
        //drpVersion.Enabled = false;
        fnCargaDatos();
        gdvRFCs.SelectedIndex = -1;
        txtPass.Enabled = false;
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

    protected void gdvRFCs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            gRFC = new clsOperacionRFC();
            ScriptManager SM = ScriptManager.GetCurrent(this);
            SM.RegisterPostBackControl(gdvRFCs);
            if (e.CommandName == "img")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridView gdvComprobante = (GridView)e.CommandSource;
                GridViewRow row = gdvComprobante.Rows[index];
                Label IdRFC = (Label)row.Cells[6].Controls[1];
                MemoryStream ms = new MemoryStream(gRFC.fnObtenerImagenRFC(Convert.ToInt32(IdRFC.Text)));
                if (ms.Length > 0)
                {
                    ms.Position = 0;
                    this.EnableViewState = false;
                    byte[] fileData = null;
                    fileData = new byte[ms.Length + 1];
                    long bytesRead = ms.Read(fileData, 0, Convert.ToInt32(ms.Length));
                    Response.ContentType = "image/jpeg";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + Guid.NewGuid().ToString() + ".jpg");
                    Response.Expires = 0;
                    Response.Buffer = true;
                    Response.Clear();
                    Response.BinaryWrite(fileData);
                    Response.End();
                }
                else
                {
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarSinLogo);
                    lblAviso.Text = Resources.resCorpusCFDIEs.VarSinLogo;
                    mpeAvisos.Show();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
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
}
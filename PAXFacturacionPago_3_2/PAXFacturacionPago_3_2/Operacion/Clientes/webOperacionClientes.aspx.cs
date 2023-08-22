using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Pantalla para el alta, baja y cambio de los datos de los receptores
/// </summary>
public partial class Operacion_Clientes_webOperacionClientes : System.Web.UI.Page
{
    private clsOperacionClientes gDAL;
    clsInicioSesionUsuario datosUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            clsComun.fnPonerTitulo(this);

            fnCargarSucursales();
            fnCargarReceptores("nombre_receptor"," ASC");

        }
    }

    /// <summary>
    /// Trae la lista de receptores activos del contribuyente
    /// </summary>
    public void fnCargarReceptores(string sOrdenar, string sDireccion)
    {
        gDAL = new clsOperacionClientes();

        try
        {
            DataTable dtReceptores = gDAL.fnLlenarReceptores(ddlSucursales.SelectedValue);
            dtReceptores.DefaultView.Sort = sOrdenar + sDireccion;
            gdvReceptores.DataSource = dtReceptores;
            gdvReceptores.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            gdvReceptores.DataSource = null;
            gdvReceptores.DataBind();
        }
    }

    /// <summary>
    /// Vacía todos los controles del formulario
    /// </summary>
    private void fnLimpiarPantalla(Panel pPnl)
    {
        try
        {
            clsComunCatalogo.fnLimpiarFormulario(pPnl);
        }
        finally
        {
            btnCancelarRfc.Visible = false;
            hdIdRfc.Value = string.Empty;
            gdvReceptores.SelectedIndex = -1;
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        gDAL = new clsOperacionClientes();

        //Validaciones
        if (string.IsNullOrEmpty(txtRFC.Text) || string.IsNullOrEmpty(txtRazonSocial.Text))
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }
        else if (!clsComun.fnValidaExpresion(txtRFC.Text, @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        try
        {
            int retVal = gDAL.fnGuardarReceptorCobro(hdIdRfc.Value, ddlSucursales.SelectedValue, txtRFC.Text, txtRazonSocial.Text, PAXCrypto.CryptoAES.EncriptaAES(txtCorreo.Text));

            if (retVal != 0)
            {


                if (string.IsNullOrEmpty(hdIdRfc.Value))
                {
                    // clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                    Response.Redirect("webOperacionClienteSuc.aspx?gis=" + retVal + "&nrs=" + txtRazonSocial.Text);
                    clsComun.fnNuevaPistaAuditoria(
                        "webOperacionClientes",
                        "fnGuardarReceptor",
                        "Se agregó un nuevo receptor con los datos",
                        txtRFC.Text,
                        txtRazonSocial.Text,
                        ddlSucursales.SelectedItem.Text,
                        txtCorreo.Text
                        );

                    fnCargarReceptores("nombre_receptor", " ASC");
                    fnLimpiarPantalla(pnlRFC);
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);

                    clsComun.fnNuevaPistaAuditoria(
                        "webOperacionClientes",
                        "fnGuardarReceptor",
                        "Se modifico al receptor con ID " + hdIdRfc.Value + " con los datos",
                        txtRFC.Text,
                        txtRazonSocial.Text,
                        ddlSucursales.SelectedItem.Text,
                        txtCorreo.Text
                        );

                    fnCargarReceptores("nombre_receptor", " ASC");
                    fnLimpiarPantalla(pnlRFC);
                }
            }
            else
                throw new Exception("No se agregó ningún registro");
        }
        catch (ThreadAbortException)
        { 
            //No se registra algun error por la redirección a la ventana de Sucursal del Cliente
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            if (string.IsNullOrEmpty(hdIdRfc.Value))
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            if (string.IsNullOrEmpty(hdIdRfc.Value))
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
        }
    }

    /// <summary>
    /// Trae la lista de sucursales activas asignadas al usuario y las carga en el drop
    /// </summary>
    private void fnCargarSucursales()
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            ddlSucursales.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true);
            ddlSucursales.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSucursales.DataSource = null;
            ddlSucursales.DataBind();
        }
        catch
        {
            //referencia nula
        }
    }

    
    protected void btnCancelarRfc_Click(object sender, EventArgs e)
    {
        fnLimpiarPantalla(pnlRFC);
    }

    
    protected void gdvReceptores_SelectedIndexChanged(object sender, EventArgs e)
    {
        gDAL = new clsOperacionClientes();
        string sIdRfc;

        try
        {
            sIdRfc = gdvReceptores.SelectedDataKey.Value.ToString();
            DataTable sdrLector = gDAL.fnEditarReceptor(sIdRfc);

            hdIdRfc.Value = sdrLector.Rows[0]["id_rfc_receptor"].ToString();
            txtRFC.Text = sdrLector.Rows[0]["rfc_receptor"].ToString();
            txtRazonSocial.Text = sdrLector.Rows[0]["nombre_receptor"].ToString();
            txtCorreo.Text = sdrLector.Rows[0]["correo"].ToString();
            ddlSucursales.SelectedValue = sdrLector.Rows[0]["id_estructura"].ToString();

            btnCancelarRfc.Visible = true;

            /*Asi estaba originalmente cuando el metodo devolvia un SqlDataReader
             * while (sdrLector.Read())
            {
                hdIdRfc.Value = sdrLector["id_rfc_receptor"].ToString();
                txtRFC.Text = sdrLector["rfc_receptor"].ToString();
                txtRazonSocial.Text = sdrLector["nombre_receptor"].ToString();
                txtCorreo.Text = sdrLector["correo"].ToString();
                ddlSucursales.SelectedValue = sdrLector["id_estructura"].ToString();

                btnCancelarRfc.Visible = true;
            }
             */
        }
        catch (Exception ex)
        {
            fnLimpiarPantalla(pnlRFC);
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void gdvReceptores_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gDAL = new clsOperacionClientes();

        try
        {
            string sIdRfc = Convert.ToString(e.Keys["id_rfc_receptor"]);
            int retVal = gDAL.fnEliminarReceptor(sIdRfc);

            if (retVal != 0)
            {
                if (hdIdRfc.Value == sIdRfc)
                    fnLimpiarPantalla(pnlRFC);

                fnCargarReceptores("nombre_receptor", " ASC");
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionCliente",
                    "fnEliminarReceptor",
                    "Se elimina al Receptor con ID " + sIdRfc
                    );
            }
            else
                throw new Exception("No se eliminó ningún registro");
        }
        catch (Exception ex)
        {
            fnLimpiarPantalla(pnlRFC);
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
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
    protected void gdvReceptores_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvReceptores.PageIndex = e.NewPageIndex;
        fnCargarReceptores("nombre_receptor", " ASC");
    }
    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarReceptores("nombre_receptor", " ASC");
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

    protected void gdvReceptores_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sOrden = e.SortExpression;
        string sDireccion = string.Empty;
        if (SortDirection == SortDirection.Ascending)
        {

            SortDirection = SortDirection.Descending;

            sDireccion = " DESC";

        }

        else
        {

            SortDirection = SortDirection.Ascending;

            sDireccion = " ASC";

        }

        fnCargarReceptores(sOrden, sDireccion);
    }
    public SortDirection SortDirection
    {

        get
        {

            if (ViewState["SortDirection"] == null)
            {

                ViewState["SortDirection"] = SortDirection.Ascending;

            }

            return (SortDirection)ViewState["SortDirection"];

        }

        set
        {

            ViewState["SortDirection"] = value;

        }

    }
}

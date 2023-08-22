using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Data;

public partial class Operacion_Clientes_webOperacionClienteSuc : System.Web.UI.Page
{
    private clsOperacionClientes gDAL;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gDAL = new clsOperacionClientes();

            txtReceptor.Text = HttpUtility.UrlDecode(Request.QueryString["nrs"]);
            string sIdReceptor = Request.QueryString["gis"];

            //verificamos que el ID en la url pertenesca a un receptor del usuario logueado
            //de lo contrario mandamos a la pantalla de error
            if (!gDAL.fnVerificarPropiedad(sIdReceptor))
                throw new Exception("Violación de seguridad por modificación de valores en URL");

            fnCargarSucursalesReceptor("calle"," ASC");
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
    protected void btnActualizarDomicilio_Click(object sender, EventArgs e)
    {
        //Validaciones
        if (string.IsNullOrEmpty(txtSucursal.Text)
            || string.IsNullOrEmpty(txtPais.Text)
            || string.IsNullOrEmpty(txtEstado.Text)
            || string.IsNullOrEmpty(txtMunicipio.Text)
            || string.IsNullOrEmpty(txtCalle.Text)
            || (!string.IsNullOrEmpty(txtCodigoPostal.Text) && !clsComun.fnIsInt(txtCodigoPostal.Text))
            )
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        gDAL = new clsOperacionClientes();
        string sIdRfc = Request.QueryString["gis"];

        try
        {
            int retVal = gDAL.fnGuardarSucursal(
                hdIdEstructura.Value,
                sIdRfc,
                txtSucursal.Text,
                PAXCrypto.CryptoAES.EncriptaAES(txtCalle.Text),
                (string.IsNullOrEmpty(txtNoExterior.Text) ? null : PAXCrypto.CryptoAES.EncriptaAES(txtNoExterior.Text)),
                (string.IsNullOrEmpty(txtNoInterior.Text) ? null : PAXCrypto.CryptoAES.EncriptaAES(txtNoInterior.Text)),
                (string.IsNullOrEmpty(txtColonia.Text) ? null : PAXCrypto.CryptoAES.EncriptaAES(txtColonia.Text)),
                (string.IsNullOrEmpty(txtCodigoPostal.Text) ? null : PAXCrypto.CryptoAES.EncriptaAES(string.Format("{0:00000}", Convert.ToInt32(txtCodigoPostal.Text)))),
                //PAXCrypto.CryptoAES.EncriptaAES(txtCodigoPostal.Text),
                (string.IsNullOrEmpty(txtLocalidad.Text) ? null : PAXCrypto.CryptoAES.EncriptaAES(txtLocalidad.Text)),
                PAXCrypto.CryptoAES.EncriptaAES(txtMunicipio.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtEstado.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtPais.Text));

            if (retVal != 0)
            {
                fnCargarSucursalesReceptor("calle", " ASC");
                if (string.IsNullOrEmpty(hdIdEstructura.Value))
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);

                    clsComun.fnNuevaPistaAuditoria(
                        "webOperacionClientes",
                        "fnGuardarSucursal",
                        "Se agregó una nueva sucursal con los datos",
                        sIdRfc,
                        txtSucursal.Text,
                        txtCalle.Text,
                        txtNoExterior.Text,
                        txtNoInterior.Text,
                        txtColonia.Text,
                        txtCodigoPostal.Text,
                        txtLocalidad.Text,
                        txtMunicipio.Text,
                        txtEstado.Text,
                        txtPais.Text
                        );
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);

                    clsComun.fnNuevaPistaAuditoria(
                        "webOperacionClientes",
                        "fnGuardarSucursal",
                        "Se modificó sucursal con ID " + hdIdEstructura.Value + " con los datos",
                        sIdRfc,
                        txtSucursal.Text,
                        txtCalle.Text,
                        txtNoExterior.Text,
                        txtNoInterior.Text,
                        txtColonia.Text,
                        txtCodigoPostal.Text,
                        txtLocalidad.Text,
                        txtMunicipio.Text,
                        txtEstado.Text,
                        txtPais.Text
                        );
                }
            }
            else
                throw new Exception("No se actualizó registro alguno.");
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            if (string.IsNullOrEmpty(hdIdEstructura.Value))
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            if (string.IsNullOrEmpty(hdIdEstructura.Value))
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
        }
        finally
        {
            fnLimpiarPantalla(pnlFormulario);
        }
    }
    protected void gdvSucursales_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //cancelamos la acción por defecto
        e.Cancel = false;
        gDAL = new clsOperacionClientes();

        try
        {
            //Obtenemos el ID de la fila seleccionada
            string id_estructura =  e.Keys["id_estructura"].ToString();

            int retVal = gDAL.fnEliminarSucursalReceptor(id_estructura);

            if (retVal != 0)
            {
                //Si se estaba editando la estructura, entonces limpiamos los datos del formulario
                if (!string.IsNullOrEmpty(hdIdEstructura.Value) && hdIdEstructura.Value.Equals(id_estructura))
                    fnLimpiarPantalla(pnlFormulario);

                fnCargarSucursalesReceptor("calle", " ASC");
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);

                clsComun.fnNuevaPistaAuditoria(
                        "webOperacionClientes",
                        "fnEliminarSucursalReceptor",
                        "Se dió de baja a la sucursal con ID " + id_estructura
                        );
            }
            else
                throw new Exception("No se realizó la baja");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
        }
    }
    protected void gdvSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrFila = (GridViewRow)gdvSucursales.SelectedRow;

        try
        {
            hdIdEstructura.Value = gdvSucursales.SelectedDataKey.Value.ToString();

            clsComunCatalogo.fnAsignarValorFila(gvrFila, pnlFormulario);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        btnCancelar.Visible = true;
    }
    protected void gdvSucursales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvSucursales.PageIndex = e.NewPageIndex;
        fnCargarSucursalesReceptor("calle", " ASC");
    }
    protected void gdvSucursales_Sorting(object sender, GridViewSortEventArgs e)
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

        //fnCargarReceptores(sOrden, sDireccion);
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        fnLimpiarPantalla(pnlFormulario);
    }
    protected void btnRegresar_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Operacion/Clientes/webOperacionClientes.aspx");
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
    /// Trae la lista de sucursales activas asignadas al receptor y las carga en el GridView
    /// </summary>
    private void fnCargarSucursalesReceptor(string sOrden, string sDireccion)
    {
        gDAL = new clsOperacionClientes();

        try
        {
            string sIdRfc = Request.QueryString["gis"];
            DataTable dtSucursales = gDAL.fnLlenarGridSucursalesReceptores(sIdRfc);
            dtSucursales.DefaultView.Sort = sOrden + sDireccion;
            gdvSucursales.DataSource = dtSucursales;
            gdvSucursales.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            gdvSucursales.DataSource = null;
            gdvSucursales.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    /// <summary>
    /// Vacía todos los controles del formulario
    /// </summary>
    private void fnLimpiarPantalla(Panel pPnl)
    {
        try
        {
            string receptor = txtReceptor.Text;
            clsComunCatalogo.fnLimpiarFormulario(pPnl);
            txtReceptor.Text = receptor;
        }
        finally
        {
            btnCancelar.Visible = false;
            hdIdEstructura.Value = string.Empty;
            gdvSucursales.SelectedIndex = -1;
        }
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
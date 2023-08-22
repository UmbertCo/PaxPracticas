using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Operacion_SeriesFolios_webOperacionSeriesFolios : System.Web.UI.Page
{
    //private clsOperacionSeriesFolios gDAL;
    private clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        clsComun.fnPonerTitulo(this);

        //if (!IsPostBack)
        //{
        //    try
        //    {
        //        fnCargarTiposDocumentos();
        //        fnCargarSucursales();
        //        fnCargarSeries();
        //    }
        //    catch (Exception ex)
        //    {
        //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Page_Load", "webOperacionSeriesFolios");
        //    }
        //}
    }
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            clsErrorLog.fnNuevaEntrada(objErr, clsErrorLog.TipoErroresLog.Datos, "Page_Error", "webOperacionSeriesFolios");
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx");
        }
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        //Validaciones
        //if (string.IsNullOrEmpty(txtSerie.Text)
        //    || string.IsNullOrEmpty(txtFolio.Text)
        //    || !clsComun.fnIsInt(txtFolio.Text)
        //    )
        //{
        //    clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
        //    return;
        //}

        //gDAL = new clsOperacionSeriesFolios();

        //try
        //{
        //    int retVal = gDAL.fnAgregarSerie(ddlSucursales.SelectedValue, ddlTipoDocumento.SelectedValue, txtSerie.Text, txtFolio.Text);

        //    if (retVal != 0)
        //    {
        //        fnCargarSeries();
        //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);

        //        clsComun.fnNuevaPistaAuditoria(
        //            "webOperacionSeriesFolios",
        //            "fnAgregarSerie",
        //            "Se agregó una nueva serie con los datos",
        //            ddlSucursales.SelectedItem.Text,
        //            ddlTipoDocumento.SelectedItem.Text,
        //            txtSerie.Text,
        //            txtFolio.Text
        //            );
        //    }
        //    else
        //        throw new Exception("No se inserto ningún registro");
        //}
        //catch (SqlException ex)
        //{
        //    //El código 5020 significa que la serie ya existe y esta activa para 
        //    // esta misma convinación de estructura - documento
        //    if (ex.Message == "5020")
        //    {
        //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorSerieYaAsignada);
        //    }
        //    else
        //    {
        //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
        //}
    }
    protected void gdvSeries_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //Desactiva la accion por defecto
        //e.Cancel = false;
        //gDAL = new clsOperacionSeriesFolios();

        //try
        //{
        //    object id_serie = gdvSeries.DataKeys[e.RowIndex].Values["id_serie"];
        //    object serie = e.Values["serie"];

        //    int retVal = gDAL.fnEliminarSerie(id_serie, serie, ddlTipoDocumento.SelectedValue, Convert.ToInt32(ddlSucursales.SelectedValue));

        //    if (retVal != 0)
        //    {
        //        fnCargarSeries();
        //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);

        //        clsComun.fnNuevaPistaAuditoria(
        //            "webOperacionSeriesFolios",
        //            "fnEliminarSerie",
        //            "Se dió de baja la serie con ID " + id_serie
        //            );
        //    }
        //    else
        //        throw new Exception("No se elimino ningún registro");
        //}
        //catch (SqlException ex)
        //{
        //    //El código 5030 significa que la serie ya esta en uso por lo que no puede ser borrada
        //    if (ex.Message == "5030")
        //    {
        //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorSerieEnUso);
        //    }
        //    else
        //    {
        //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
        //}
    }
    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarSeries();
    }
    protected void ddlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarSeries();
    }
    protected void gdvSeries_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gdvSeries.PageIndex = e.NewPageIndex;
        //fnCargarSeries();
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
    /// Trae la lista de sucursales activas asignadas al usuario y las carga en el GridView
    /// </summary>
    private void fnCargarSucursales()
    {
        //try
        //{
        //    ddlSucursales.DataSource = clsComun.fnLlenarDropSucursales(true);
        //    ddlSucursales.DataBind();
        //}
        //catch (SqlException ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    ddlSucursales.DataSource = null;
        //    ddlSucursales.DataBind();
        //}
        //catch
        //{
        //    //referencia nula
        //}
    }

    /// <summary>
    /// Llena el drop de selección para el tipo de documento al que se le asignará el impuesto
    /// </summary>
    private void fnCargarTiposDocumentos()
    {
        //gDAL = new clsOperacionSeriesFolios();

        //try
        //{
        //    ddlTipoDocumento.DataSource = gDAL.fnObtenerTiposDocumentos();
        //    ddlTipoDocumento.DataBind();
        //}
        //catch (SqlException ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //}
    }

    /// <summary>
    /// Método que se encargade cargar las series relacionadas a la estructura y el documento
    /// </summary>
    private void fnCargarSeries()
    {
        //gDAL = new clsOperacionSeriesFolios();

        //try
        //{
        //    datosUsuario = clsComun.fnUsuarioEnSesion();
        //    gdvSeries.DataSource = gDAL.fnObtenerSeries(ddlSucursales.SelectedValue, ddlTipoDocumento.SelectedValue, datosUsuario.id_usuario.ToString());
        //    gdvSeries.DataBind();
        //}
        //catch (SqlException ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        //}
    } 
}
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
/// Pantalla para la configuración de las tasas de impuesto para cada tipo de documento.
/// Los documentos que queden configurados serán los únicos que podrán ser utilizados para realizar factura
/// </summary>
public partial class Operacion_DocumentosImpuestos_webOperacionDocImpuestos : System.Web.UI.Page
{
    private clsOperacionDocImpuestos gDAL;

    protected void Page_Load(object sender, EventArgs e)
    {
        clsComun.fnPonerTitulo(this);

        if (!IsPostBack)
        {
            fnCargarTiposDocumentos();
            fnCargarEfectos();
            fnCargarImpuestos();
            fnCargarAsignados();
        }

        gdvImpuestos.SelectedIndex = -1;
    }

    /// <summary>
    /// Carga todos los impuestos asignados al contribuyente dentro del grid view
    /// </summary>
    private void fnCargarAsignados()
    {
        gDAL = new clsOperacionDocImpuestos();

        try
        {
            DataTable dtAsignados = gDAL.fnObtenerDocumentosAsignados();

            gdvImpuestos.DataSource = dtAsignados;
            gdvImpuestos.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
        catch
        {
            //Sin acción
        }
    }

    /// <summary>
    /// Llena el drop de selección de efecto del impuesto
    /// </summary>
    private void fnCargarEfectos()
    {
        gDAL = new clsOperacionDocImpuestos();
        ddlEfecto.DataSource = gDAL.fnCargarEfectos();
        ddlEfecto.DataBind();
    }

    /// <summary>
    /// Llena el drop de selección del tipo de impuesto a asignar
    /// </summary>
    private void fnCargarImpuestos()
    {
        gDAL = new clsOperacionDocImpuestos();

        try
        {
            ddlImpuesto.DataSource = gDAL.fnCargarImpuestos(ddlEfecto.SelectedValue);
            ddlImpuesto.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    /// <summary>
    /// Llena el drop de selección para el tipo de documento al que se le asignará el impuesto
    /// </summary>
    private void fnCargarTiposDocumentos()
    {
        gDAL = new clsOperacionDocImpuestos();

        try
        {
            ddlTipoDocumento.DataSource = gDAL.fnCargarTiposDocumento();
            ddlTipoDocumento.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {

        gDAL = new clsOperacionDocImpuestos();

        try
        {
            int retVal = gDAL.fnAgregarImpuesto(ddlTipoDocumento.SelectedValue, ddlImpuesto.SelectedValue);

            if (retVal != 0)
            {
                fnCargarAsignados();
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionDocImpuestos",
                    "fnAgregarImpuesto",
                    "Se agregó una nueva combinación Documento-Impuesto con los datos",
                    ddlTipoDocumento.SelectedItem.Text,
                    ddlImpuesto.SelectedItem.Text
                    );
            }
            else
                throw new Exception("No se Inserto ningún registro");
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
        }
    }

    protected void gdvImpuestos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //Desactivamos el evento por defecto
        e.Cancel = false;

        gDAL = new clsOperacionDocImpuestos();

        try
        {
            object nIdTipoDocumento = gdvImpuestos.DataKeys[e.RowIndex].Values["id_tipo_documento"];
            object nIdImpuesto = gdvImpuestos.DataKeys[e.RowIndex].Values["id_impuesto"];

            int retVal = gDAL.fnEliminarImpuesto( nIdTipoDocumento, nIdImpuesto);

            if (retVal != 0)
            {
                fnCargarAsignados();
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionDocImpuestos",
                    "fnEliminarImpuesto",
                    "Se dió de baja a la combinación Documento-Impuesto con los IDs " + nIdImpuesto + " y " + nIdTipoDocumento
                    );
            }
            else
                throw new Exception("No se eliminó ningún registro");
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
        }
    }

    protected void ddlEfecto_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarImpuestos();
    }
    protected void gdvImpuestos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvImpuestos.PageIndex = e.NewPageIndex;
        fnCargarAsignados();
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
    protected void gdvImpuestos_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow fila = gdvImpuestos.SelectedRow;

        string documento = HttpUtility.HtmlDecode(fila.Cells[1].Text);
        string impuesto = HttpUtility.HtmlDecode(fila.Cells[2].Text);
        string efecto = HttpUtility.HtmlDecode(fila.Cells[3].Text);
        string tasa = HttpUtility.HtmlDecode(fila.Cells[4].Text);

        ddlTipoDocumento.SelectedIndex = ddlTipoDocumento.Items.IndexOf(ddlTipoDocumento.Items.FindByText(documento));
        ddlEfecto.SelectedIndex = ddlEfecto.Items.IndexOf(ddlEfecto.Items.FindByText(efecto));
        fnCargarImpuestos();
        ddlImpuesto.SelectedIndex = ddlImpuesto.Items.IndexOf(ddlImpuesto.Items.FindByText(impuesto));
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
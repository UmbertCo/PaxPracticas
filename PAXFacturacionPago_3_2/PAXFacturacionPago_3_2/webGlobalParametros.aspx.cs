using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;

public partial class webGlobalParametros : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fnCargarGrid();
        }
    }
    /// <summary>
    /// Trae la lista de sucursales activas asignadas al usuario y las carga en el GridView
    /// </summary>
    private void fnCargarGrid()
    {
        try
        {
            gdvDatos.DataSource = clsComun.LlenarParametros();
            gdvDatos.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            gdvDatos.DataSource = null;
            gdvDatos.DataBind();
        }
        catch
        {
            //referencia nula
        }
    }
    /// <summary>
    /// Vacía todos los controles del formulario
    /// </summary>
    private void fnLimpiarPantalla()
    {
        try
        {
            clsComunCatalogo.fnLimpiarFormulario(pnlFormulario);
            ddlEstatus.SelectedIndex = 0;
        }
        finally
        {
            btnCancelar.Visible = false;
            hdIdEstructura.Value = string.Empty;
            gdvDatos.SelectedIndex = -1;
        }
    }
    protected void btnGuardarActualizar_Click(object sender, EventArgs e)
    {
        //validaciones
        if (string.IsNullOrEmpty(txtParametro.Text)
            || string.IsNullOrEmpty(txtValor.Text)
            )
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        clsComun gDAL = new clsComun();

        try
        {
            int retVal = gDAL.fnGuardarParametro(
                hdIdEstructura.Value,
                txtParametro.Text,
                txtValor.Text,
                ddlEstatus.SelectedValue);

            if (retVal != 0)
            {
                fnCargarGrid();
                fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), true);
            }
            else
                throw new Exception("No se actualizó registro alguno.");
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), false);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), false);
        }
        finally
        {
            fnLimpiarPantalla();
        }

        txtParametro.Enabled = true;
    }
    /// <summary>
    /// Seleccionar el tipo de mensaje a mostrar
    /// </summary>
    /// <param name="pbNuevo">Es nuevo registro</param>
    /// <param name="pbExito">Fue exitosa la acción</param>
    private void fnSeleccionarMensaje(bool pbNuevo, bool pbExito)
    {
        if (pbExito)
        {
            if (pbNuevo)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);

                clsComun.fnNuevaPistaAuditoria(
                    "webGlobalParametros",
                    "fnGuardarParametro",
                    "Se agregó una nueva parametro con los datos",
                    txtParametro.Text,
                    txtValor.Text,
                    ddlEstatus.SelectedValue
                    );
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);

                clsComun.fnNuevaPistaAuditoria(
                    "webGlobalParametros",
                    "fnGuardarParametro",
                    "Se modificó el parametro con ID " + hdIdEstructura.Value + " con los datos",
                    txtParametro.Text,
                    txtValor.Text,
                    ddlEstatus.SelectedValue
                    );
            }
        }
        else
        {
            if (pbNuevo)
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        fnLimpiarPantalla();
        txtParametro.Enabled = true;
    }

    protected void gdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvDatos.PageIndex = e.NewPageIndex;
        fnCargarGrid();
    }
    protected void gdvDatos_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        gdvDatos.DataSource = null;
        gdvDatos.DataBind(); 
    }
    protected void gdvDatos_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrFila = (GridViewRow)gdvDatos.SelectedRow;

        try
        {
            hdIdEstructura.Value = gdvDatos.SelectedDataKey.Value.ToString();

            clsComunCatalogo.fnAsignarValorFila(gvrFila, pnlFormulario);

            ddlEstatus.SelectedItem.Text = ((Label)gvrFila.FindControl("lblEstatus")).Text;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        txtParametro.Enabled = false;
        btnCancelar.Visible = true;
    }
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            Server.ClearError();
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
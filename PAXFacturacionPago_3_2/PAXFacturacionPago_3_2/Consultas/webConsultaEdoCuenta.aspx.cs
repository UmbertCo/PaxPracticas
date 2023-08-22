using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.Threading;
using System.Globalization;
using System.IO.Compression;
using System.IO;
using System.Collections;
using Ionic.Zip;
using ICSharpCode.SharpZipLib.Zip;
using Root.Reports;
using System.Web.UI.HtmlControls;
using System.Xml.XPath;
using System.ServiceModel.Channels;
using System.Net;
public partial class Consultas_webConsultaEdoCuenta : System.Web.UI.Page
{
    private clsOperacionConsulta gDAL;
    private clsOperacionDistribuidores gOp;
    private clsInicioSesionUsuario DatosUsuario;
    protected DataTable dtCreditos;
    private DataTable TablaDet;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Establecemos el filtro de fechas para el día d ehoy
            txtFechaFin_CalendarExtender.SelectedDate = DateTime.Now;
            txtFechaIni_CalendarExtender.SelectedDate = DateTime.Now;
            //ViewState["id_EdoCuenta"] = string.Empty;

            //Se verifica si el usuario es distribuidor
            gOp = new clsOperacionDistribuidores();
            DataTable tblDistribuidor = new DataTable();
            tblDistribuidor = gOp.fnObtieneDistribuidoresporidUsuario(clsComun.fnUsuarioEnSesion().id_usuario);
            if (tblDistribuidor.Rows.Count > 0)
            {
                gDAL = new clsOperacionConsulta();
                lblCliAfi.Visible = true;
                ddlCliAfi.Visible = true;
                ddlCliAfi.DataSource = gDAL.fnObtenerUsuarioDist(Convert.ToInt32(tblDistribuidor.Rows[0]["id_distribuidor"].ToString()));
                ddlCliAfi.DataBind();
            }
            else
            {
                lblCliAfi.Visible = false;
                ddlCliAfi.Visible = false;
            }
            fnActualizarLblCreditos();
        }
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        fnObtenerDatos();
    }

    private void fnObtenerDatos()
    {
        gDAL = new clsOperacionConsulta();

        try
        {
            //Se verifica si el usuario es distribuidor
            gOp = new clsOperacionDistribuidores();
            gDAL = new clsOperacionConsulta();
            DataTable tblDistribuidor = new DataTable();
            tblDistribuidor = gOp.fnObtieneDistribuidoresporidUsuario(clsComun.fnUsuarioEnSesion().id_usuario);
            if (tblDistribuidor.Rows.Count > 0)
            {
                gdvCreditos.DataSource = gDAL.fnObtenerEdoCuentaDist(Convert.ToDateTime(txtFechaIni.Text),
                                                                     Convert.ToDateTime(txtFechaFin.Text), 
                                                                     Convert.ToInt32(tblDistribuidor.Rows[0]["id_distribuidor"]),
                                                                     Convert.ToInt32(ddlCliAfi.SelectedValue));
                gdvCreditos.Columns[0].Visible = true;
            }
            else
            {
                gdvCreditos.DataSource = gDAL.fnObtenerEdoCuenta(Convert.ToDateTime(txtFechaIni.Text),
                                                                 Convert.ToDateTime(txtFechaFin.Text));
                gdvCreditos.Columns[0].Visible = false;
            }
            gdvCreditos.DataBind();
            ViewState["dtEdoCuenta"] = gdvCreditos.DataSource;

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

    /// <summary>
    /// Actualizar etiqueta de Creditos.
    /// </summary>
    private void fnActualizarLblCreditos()
    {
        DatosUsuario = clsComun.fnUsuarioEnSesion();
        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(DatosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
        ViewState["dtCreditos"] = dtCreditos;
        if (dtCreditos.Rows.Count > 0)
        {
            if (dtCreditos.Rows[0]["creditos"].ToString() == "0.00")
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                if (creditos == 0)
                {
                    lblCredValor.Text = "0";
                }
                else
                {
                    lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                }

            }
            else
            {
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
            }
        }
        else
        {
            clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
            if (creditos == 0)
            {
                lblCredValor.Text = "0";
            }
            else
            {
                lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
            }
        }
        
    }

    /// <summary>
    /// Revisa que existan creditos.
    /// </summary>
    private double fnRevisaCreditos()
    {
        double retorno = 0;
        double credit = 0;
        //Revisar los creditos disponibles.
        DatosUsuario = clsComun.fnUsuarioEnSesion();
        dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(DatosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');
        ViewState["dtCreditos"] = dtCreditos;
        if (dtCreditos.Rows.Count > 0)
        {
            double creditos = 0;
            creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
            if (creditos == 0)
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
                ViewState["dtCreditos"] = dtCreditos;
                double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                credit = creditos2;
                if (creditos2 == 0)
                {
                    modalCreditos.Show();
                }
            }

        }
        else
        {
            clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(DatosUsuario.id_usuario);
            ViewState["dtCreditos"] = dtCreditos;
            double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
            credit = creditos;
            if (creditos == 0)
            {
                modalCreditos.Show();
            }
        }

        retorno = credit;//dtCreditos.Rows.Count;

        return retorno;
    }
    protected void gdvCreditos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvCreditos.PageIndex = e.NewPageIndex;
        fnObtenerDatos();
    }


    private void fnObtieneDetalle(DateTime dFecIni, int nId_Usuario)
    {
        DateTime dFecFin = dFecIni;
        gDAL = new clsOperacionConsulta();

        try
        {
            gdvDetalle.DataSource = gDAL.fnObtenerDetalleEdoCuenta(dFecIni,
                                                                   dFecFin, nId_Usuario);
            gdvDetalle.DataBind();
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
    protected void gdvCreditos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int nUsuario = 0;
            //ViewState["id_EdoCuenta"] = gdvCreditos.SelectedDataKey.Value;

            TablaDet = (DataTable)ViewState["dtEdoCuenta"];

            DateTime dFecha = Convert.ToDateTime(TablaDet.Rows[gdvCreditos.SelectedRow.DataItemIndex]["Fecha"].ToString());
            //Se verifica si el usuario es distribuidor
            gOp = new clsOperacionDistribuidores();
            DataTable tblDistribuidor = new DataTable();
            tblDistribuidor = gOp.fnObtieneDistribuidoresporidUsuario(clsComun.fnUsuarioEnSesion().id_usuario);
            if (tblDistribuidor.Rows.Count > 0)
            {
                nUsuario = Convert.ToInt32(TablaDet.Rows[gdvCreditos.SelectedRow.DataItemIndex]["id_usuario_timbrado"].ToString());
            }
            else
            {
                nUsuario = clsComun.fnUsuarioEnSesion().id_usuario;
            }
            gdvDetalle.PageIndex = 0;
            fnObtieneDetalle(dFecha, nUsuario);
            modalDetalle.Show();
        }
        catch
        {

        }
    }
    protected void btnAcepDetalle_Click(object sender, EventArgs e)
    {
        gdvDetalle.DataSource = string.Empty;
        gdvDetalle.DataBind();
        modalDetalle.Hide();
    }
    protected void gdvDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvDetalle.PageIndex = e.NewPageIndex;

        int nUsuario = 0;
        //ViewState["id_EdoCuenta"] = gdvCreditos.SelectedDataKey.Value;

        TablaDet = (DataTable)ViewState["dtEdoCuenta"];
        DateTime dFecha = Convert.ToDateTime(TablaDet.Rows[gdvCreditos.SelectedRow.DataItemIndex]["Fecha"].ToString());
        //Se verifica si el usuario es distribuidor
        gOp = new clsOperacionDistribuidores();
        DataTable tblDistribuidor = new DataTable();
        tblDistribuidor = gOp.fnObtieneDistribuidoresporidUsuario(clsComun.fnUsuarioEnSesion().id_usuario);
        if (tblDistribuidor.Rows.Count > 0)
        {
            nUsuario = Convert.ToInt32(TablaDet.Rows[gdvCreditos.SelectedRow.DataItemIndex]["Cliente"].ToString());
        }
        else
        {
            nUsuario = clsComun.fnUsuarioEnSesion().id_usuario;
        }
        fnObtieneDetalle(dFecha, nUsuario);
        modalDetalle.Show();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;

public partial class Consultas_webReporteCreditosHistorico : System.Web.UI.Page
{
    private clsInicioSesionUsuario datosUsuario;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fnObtieneUsuarios();
            txtFechaInicio.Text = Convert.ToString(DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year);
            txtFechaFin.Text = Convert.ToString(DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year);
        }
    }
    protected void ddlUsuario_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fnCargaSucursales(Convert.ToInt32(ddlUsuario.SelectedValue));
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    public void fnObtieneUsuarios()
    {
        clsOperacionDistribuidores distribuidores = new clsOperacionDistribuidores();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        DataTable dist1 = distribuidores.fnObtieneDistribuidoresporidUsuario(datosUsuario.id_usuario);
        if (dist1.Rows.Count > 0)
        {
            DataTable UsuariosDist = distribuidores.fnObtieneUsuariosporDistribuidor(Convert.ToInt32(dist1.Rows[0]["id_distribuidor"]));
            ddlUsuario.DataSource = UsuariosDist;
            ddlUsuario.DataValueField = "id_usuario";
            ddlUsuario.DataTextField = "clave_usuario";


            DataTable DRUser = distribuidores.fnObtenerDatosUsuario();

            DataRow drFila = UsuariosDist.NewRow();
            drFila["id_usuario"] = datosUsuario.id_usuario;
            drFila["clave_usuario"] = DRUser.Rows[0]["clave_usuario"].ToString();

            UsuariosDist.Rows.InsertAt(drFila, 0); 

            ddlUsuario.DataBind();
            fnCargaSucursales(Convert.ToInt32(ddlUsuario.SelectedValue));
        }
        else
        {
            clsOperacionUsuarios usuarios = new clsOperacionUsuarios();
            DataTable DRUser = distribuidores.fnObtenerDatosUsuario();
            ddlUsuario.Items.Add(DRUser.Rows[0]["clave_usuario"].ToString());
            ddlUsuario.Items[0].Value = Convert.ToString(datosUsuario.id_usuario);
            fnCargaSucursales(Convert.ToInt32(ddlUsuario.Items[0].Value));

        }
    }

    public void fnCargaSucursales(int Id_usuario)
    {

        clsConfiguracionCreditos creditos = new clsConfiguracionCreditos();
        DataTable dtSucursales = new DataTable();
        dtSucursales = creditos.fnObtenerSucursalesCreditosAcumulados(Id_usuario);
        ddlSucursales.DataSource = dtSucursales;
        ddlSucursales.DataValueField = "id_estructura";
        ddlSucursales.DataTextField = "nombre";
        ddlSucursales.DataBind();
    }
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        try
        {
            fnRealizarConsulta();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void btnExportar_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)(ViewState["ExportarExcel"]);

        DataTable dtNew = new DataTable();
        //DataColumn columna1 = new DataColumn();
        //columna1.DataType = System.Type.GetType("System.String");
        //columna1.AllowDBNull = true;
        //columna1.Caption = "origen";
        //columna1.ColumnName = "origen";
        //columna1.DefaultValue = null;
        //dtNew.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.String");
        columna2.AllowDBNull = true;
        columna2.Caption = "origen";
        columna2.ColumnName = "origen";
        columna2.DefaultValue = null;
        dtNew.Columns.Add(columna2);
        
        //DataColumn columna3 = new DataColumn();
        //columna3.DataType = System.Type.GetType("System.Int32");
        //columna3.AllowDBNull = true;
        //columna3.Caption = "id_tipodocumento";
        //columna3.ColumnName = "id_tipodocumento";
        //columna3.DefaultValue = null;
        //dtNew.Columns.Add(columna3);

        DataColumn columna4 = new DataColumn();
        columna4.DataType = System.Type.GetType("System.String");
        columna4.AllowDBNull = true;
        columna4.Caption = "documento";
        columna4.ColumnName = "documento";
        columna4.DefaultValue = null;
        dtNew.Columns.Add(columna4);

        DataColumn columna5 = new DataColumn();
        columna5.DataType = System.Type.GetType("System.String");
        columna5.AllowDBNull = true;
        columna5.Caption = "UUID";
        columna5.ColumnName = "UUID";
        columna5.DefaultValue = null;
        dtNew.Columns.Add(columna5);

        DataColumn columna6 = new DataColumn();
        columna6.DataType = System.Type.GetType("System.String");
        columna6.AllowDBNull = true;
        columna6.Caption = "serie";
        columna6.ColumnName = "serie";
        columna6.DefaultValue = null;
        dtNew.Columns.Add(columna6);

        DataColumn columna7 = new DataColumn();
        columna7.DataType = System.Type.GetType("System.String");
        columna7.AllowDBNull = true;
        columna7.Caption = "folio";
        columna7.ColumnName = "folio";
        columna7.DefaultValue = null;
        dtNew.Columns.Add(columna7);

        //DataColumn columna8 = new DataColumn();
        //columna8.DataType = System.Type.GetType("System.String");
        //columna8.AllowDBNull = true;
        //columna8.Caption = "estatus";
        //columna8.ColumnName = "estatus";
        //columna8.DefaultValue = null;
        //dtNew.Columns.Add(columna8);

        DataColumn columna9 = new DataColumn();
        columna9.DataType = System.Type.GetType("System.String");
        columna9.AllowDBNull = true;
        columna9.Caption = "estatus";
        columna9.ColumnName = "estatus";
        columna9.DefaultValue = null;
        dtNew.Columns.Add(columna9);

        //DataColumn columna10 = new DataColumn();
        //columna10.DataType = System.Type.GetType("System.Int32");
        //columna10.AllowDBNull = true;
        //columna10.Caption = "id_estructura";
        //columna10.ColumnName = "id_estructura";
        //columna10.DefaultValue = null;
        //dtNew.Columns.Add(columna10);

        DataColumn columna11 = new DataColumn();
        columna11.DataType = System.Type.GetType("System.String");
        columna11.AllowDBNull = true;
        columna11.Caption = "sucursal";
        columna11.ColumnName = "sucursal";
        columna11.DefaultValue = null;
        dtNew.Columns.Add(columna11);

        //DataColumn columna12 = new DataColumn();
        //columna12.DataType = System.Type.GetType("System.Int32");
        //columna12.AllowDBNull = true;
        //columna12.Caption = "id_creditos";
        //columna12.ColumnName = "id_creditos";
        //columna12.DefaultValue = null;
        //dtNew.Columns.Add(columna12);
      
        DataColumn columna13 = new DataColumn();
        columna13.DataType = System.Type.GetType("System.Double");
        columna13.AllowDBNull = true;
        columna13.Caption = "cargo";
        columna13.ColumnName = "cargo";
        columna13.DefaultValue = null;
        dtNew.Columns.Add(columna13);

        DataColumn columna14 = new DataColumn();
        columna14.DataType = System.Type.GetType("System.Double");
        columna14.AllowDBNull = true;
        columna14.Caption = "Abono";
        columna14.ColumnName = "Abono";
        columna14.DefaultValue = null;
        dtNew.Columns.Add(columna14);

        DataColumn columna15 = new DataColumn();
        columna15.DataType = System.Type.GetType("System.Double");
        columna15.AllowDBNull = true;
        columna15.Caption = "creditos restantes";
        columna15.ColumnName = "creditos restantes";
        columna15.DefaultValue = null;
        dtNew.Columns.Add(columna15);

        DataColumn columna16 = new DataColumn();
        columna16.DataType = System.Type.GetType("System.DateTime");
        columna16.AllowDBNull = true;
        columna16.Caption = "fecha"; 
        columna16.ColumnName = "fecha";
        columna16.DefaultValue = null;
        dtNew.Columns.Add(columna16);

        foreach (DataRow renglon in dt.Rows)
        {
            DataRow nuevo = dtNew.NewRow();
            nuevo["origen"] = renglon["descripcion_origen"];
            nuevo["documento"] = renglon["documento"];
            nuevo["UUID"] = renglon["UUID"];
            nuevo["serie"] = renglon["serie"];
            nuevo["folio"] = renglon["folio"];
            nuevo["estatus"] = renglon["descripcion_estatus"];
            nuevo["sucursal"] = renglon["estructura"];
            nuevo["cargo"] = renglon["cargo"];
            nuevo["Abono"] = renglon["Abono"];
            nuevo["creditos restantes"] = renglon["creditos_restantes"];
            nuevo["fecha"] = renglon["fecha"];
            dtNew.Rows.Add(nuevo);
        }
        dataTableAExcel(dtNew);
    }

    private void dataTableAExcel(DataTable tabla)
    {
        ScriptManager SM = ScriptManager.GetCurrent(this);
        SM.RegisterPostBackControl(btnExportar);
        if (tabla.Rows.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            System.Web.UI.Page pagina = new System.Web.UI.Page();
            HtmlForm form = new HtmlForm();
            GridView dg = new GridView();
            dg.EnableViewState = false;
            dg.DataSource = tabla;
            dg.DataBind();
            pagina.EnableEventValidation = false;
            pagina.DesignerInitialize();
            pagina.Controls.Add(form);
            form.Controls.Add(dg);
            pagina.RenderControl(htw);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Guid.NewGuid().ToString() + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
    }

    private bool fnRealizarConsulta()
    {
        bool condicion = false;
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            clsConfiguracionCreditos creditos = new clsConfiguracionCreditos();
            DataTable dtAcumulado = new DataTable();
            string FechaInicio = string.Empty;
            string FechaFin = string.Empty;
            int nIdEstructura = 0;

            if (txtFechaInicio.Text != "")
            {
                FechaInicio = txtFechaInicio.Text;
            }
            if (txtFechaFin.Text != "")
            {
                FechaFin = txtFechaFin.Text;
            }
        
            nIdEstructura = Convert.ToInt32(ddlSucursales.SelectedValue);

            dtAcumulado = creditos.fnObtieneReporteHistorico(nIdEstructura, FechaInicio, FechaFin);
            gdvHistorico.DataSource = dtAcumulado;
            gdvHistorico.DataBind();

            if (gdvHistorico.Rows.Count > 0)
            {
                ViewState["ExportarExcel"] = dtAcumulado;

                btnExportar.Visible = true;


            }
            else
            {
                btnExportar.Visible = false;

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
        return condicion;
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
    protected void gdvHistorico_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvHistorico.PageIndex = e.NewPageIndex;
        bool condicion = fnRealizarConsulta();
    }
}
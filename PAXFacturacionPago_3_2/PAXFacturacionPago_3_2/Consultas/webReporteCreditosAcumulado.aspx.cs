using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Globalization;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Consultas_webReporteCreditosAcumulado : System.Web.UI.Page
{
    private clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fnObtieneUsuarios();
        }
    }

    protected void gdvAcumulado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvAcumulado.PageIndex = e.NewPageIndex;
        bool condicion = fnRealizarConsulta();
        //if (condicion == true)
        //{
        //    cbPaginado.Checked = true;
        //}
        //else
        //{
        //    cbPaginado.Checked = false;
        //}
    }
    //protected void cbSeleccionar_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (cbSeleccionar.Checked)
    //    {
    //        foreach (GridViewRow renglon in gdvAcumulado.Rows)
    //        {
    //            CheckBox CbCan;

    //            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
    //            CbCan.Checked = true;

    //        }
    //    }
    //    else
    //    {
    //        foreach (GridViewRow renglon in gdvAcumulado.Rows)
    //        {
    //            CheckBox CbCan;

    //            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
    //            CbCan.Checked = false;

    //        }
    //    }
    //}
    //protected void cbPaginado_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (cbPaginado.Checked)
    //    {
    //        gdvAcumulado.AllowPaging = true;
    //        gdvAcumulado.PageSize = 10;
    //        Panel234.ScrollBars = ScrollBars.None;
    //        bool condicion = fnRealizarConsulta();
    //        if (condicion == true)
    //        {
    //            cbPaginado.Checked = true;
    //        }
    //        else
    //        {
    //            cbPaginado.Checked = false;
    //        }
    //    }
    //    else
    //    {
    //        gdvAcumulado.AllowPaging = false;
    //        Panel234.ScrollBars = ScrollBars.Auto;
    //        fnRealizarConsulta();

    //    }
    //}

    private bool fnRealizarConsulta()
    {
        bool condicion = false;
            try
            {
                datosUsuario = clsComun.fnUsuarioEnSesion();
                clsConfiguracionCreditos creditos = new clsConfiguracionCreditos();
                DataTable dtAcumulado = new DataTable();
                string FechaCompra = string.Empty;
                string FechaVigencia = string.Empty;
                double Precio = 0;
                int nIdEstructura = 0;

                if (txtFechaCompra.Text != "")
                {
                    FechaCompra = txtFechaCompra.Text; 
                }
                if (txtFechaVigencia.Text != "")
                {
                    FechaVigencia = txtFechaVigencia.Text;
                }
                if (txtPrecio.Text != "")
                {
                    Precio = Convert.ToDouble(txtPrecio.Text);
                }
               
                    nIdEstructura = Convert.ToInt32(ddlSucursales.SelectedValue);
                
                dtAcumulado = creditos.fnObtieneReporteAcumulado(Convert.ToInt32(ddlUsuario.SelectedValue) , nIdEstructura, FechaCompra, FechaVigencia, Precio);
                gdvAcumulado.DataSource = dtAcumulado;
                gdvAcumulado.DataBind();

                if (gdvAcumulado.Rows.Count > 0)
                {
                    ViewState["ExportarExcel"] = dtAcumulado;
   
                  //  cbSeleccionar.Visible = true;
                   // cbPaginado.Visible = true;
                    btnExportar.Visible = true;


                }
                else
                {
                  //  cbSeleccionar.Visible = false;
                  //  cbPaginado.Visible = false;
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
    protected void btnExportar_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)(ViewState["ExportarExcel"]);

        DataTable dtNew = new DataTable();
        DataColumn columna1 = new DataColumn();
        columna1.DataType = System.Type.GetType("System.String");
        columna1.AllowDBNull = true;
        columna1.Caption = "clave_usuario";
        columna1.ColumnName = "clave_usuario";
        columna1.DefaultValue = null;
        dtNew.Columns.Add(columna1);

        DataColumn columna2 = new DataColumn();
        columna2.DataType = System.Type.GetType("System.Double");
        columna2.AllowDBNull = true;
        columna2.Caption = "creditos";
        columna2.ColumnName = "creditos";
        columna2.DefaultValue = null;
        dtNew.Columns.Add(columna2);

        DataColumn columna3 = new DataColumn();
        columna3.DataType = System.Type.GetType("System.Double");
        columna3.AllowDBNull = true;
        columna3.Caption = "precio_unitario";
        columna3.ColumnName = "precio_unitario";
        columna3.DefaultValue = null;
        dtNew.Columns.Add(columna3);
        
        DataColumn columna4 = new DataColumn();
        columna4.DataType = System.Type.GetType("System.DateTime");
        columna4.AllowDBNull = true;
        columna4.Caption = "fecha_compra";
        columna4.ColumnName = "fecha_compra";
        columna4.DefaultValue = null;
        dtNew.Columns.Add(columna4);

        DataColumn columna5 = new DataColumn();
        columna5.DataType = System.Type.GetType("System.DateTime");
        columna5.AllowDBNull = true;
        columna5.Caption = "fecha_vigencia";
        columna5.ColumnName = "fecha_vigencia";
        columna5.DefaultValue = null;
        dtNew.Columns.Add(columna5);

        foreach (DataRow renglon in dt.Rows)
        {
            DataRow nuevo = dtNew.NewRow();
            nuevo["clave_usuario"] = renglon["clave_usuario"];
            nuevo["creditos"] = renglon["creditos"];
            nuevo["precio_unitario"] = renglon["precio_unitario"];
            nuevo["fecha_compra"] = renglon["fecha_compra"];
            nuevo["fecha_vigencia"] = renglon["fecha_vigencia"];
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
        if(dist1.Rows.Count >0 )
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
}
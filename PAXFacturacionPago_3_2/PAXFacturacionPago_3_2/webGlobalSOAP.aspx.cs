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

public partial class webGlobalSOAP : System.Web.UI.Page
{
    clsInicioSesionUsuario datosUsuario;
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    DataTable tblPista;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            fnCargarUUID();

            //Establecemos el filtro de fechas para el día d ehoy
            txtFechaIniSO_Calendarextender.SelectedDate = DateTime.Now;
            txtFechaFinSO_Calendarextender.SelectedDate = DateTime.Now;

        }
    }
    protected void btnConsultaSO_Click(object sender, EventArgs e)
    {
        string uuid;
        string codigo;
        string FechaIni = string.Empty;
        string FechaFin = string.Empty;
        string Origen = string.Empty; 

        DateTime startingDate = Convert.ToDateTime(txtFechaIniSO.Text);
        DateTime endingDate = Convert.ToDateTime(txtFechaFinSO.Text);

        FechaIni = startingDate.ToString("yyyyMMdd");
        FechaFin = endingDate.ToString("yyyyMMdd");

        if (ddlUsuarioSO.SelectedItem.Text == "Todos")
            uuid = string.Empty;
        else
            uuid = ddlUsuarioSO.SelectedItem.Text;

        if (ddlCodigo.SelectedItem.Text == "Todos")
            codigo = string.Empty;
        else
            codigo = ddlCodigo.SelectedItem.Text;

        datosUsuario = clsComun.fnUsuarioEnSesion();

        if (ddlOrigen.SelectedValue == "Envio")
        {
            Origen = "E";
        }
        else
        {
            Origen = "C";
        }
           

        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "fnCargarSOAP" + "|" + "Consulta los SOAP." + "| Usuario: " + datosUsuario.userName + "| Fecha: " + DateTime.Now);
        fnCargarSOAP(uuid, ddlTipo.SelectedValue, FechaIni, FechaFin, Origen, ddlOrigenAcuses.SelectedValue, codigo);
    }

    /// <summary>
    /// Recupera la lista de UUID
    /// </summary>
    private void fnCargarUUID()
    {
        try
        {
            ddlUsuarioSO.DataSource = clsComun.fnObtenerUUID(ddlTipo.SelectedValue,ddlOrigenAcuses.SelectedValue);
            ddlUsuarioSO.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlUsuarioSO.DataSource = null;
            ddlUsuarioSO.DataBind();
        }
    }

    /// <summary>
    /// Carga la consulta al GRID
    /// </summary>
    /// <param name="psUUID"></param>
    /// <param name="psSOAP"></param>
    /// <param name="sFechaIni"></param>
    /// <param name="sFechaFin"></param>
    private void fnCargarSOAP(string psUUID, string psSOAP,
                              string sFechaIni, string sFechaFin,string sOrigen,string sEfecto,string sCodigo)
    {
        try
        {
            DataTable tblentryType = new DataTable();

            tblentryType = clsComun.fnObtenerSOAP(datosUsuario.id_contribuyente.ToString(), psUUID, psSOAP, sFechaIni, sFechaFin, sOrigen, sEfecto, sCodigo);
            ViewState["ExportarExcel"] = tblentryType;
            if (tblentryType.Rows.Count > 0)
            {
                btnExportar.Visible = true;
            }
            else
            {
                btnExportar.Visible = false;
            }

            if (!string.IsNullOrEmpty(sFechaIni) && !string.IsNullOrEmpty(sFechaFin))
            {
                ViewState["tblPistas"] = tblentryType;
                grvDatosSO.DataSource = tblentryType;
                grvDatosSO.DataBind();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            grvDatosSO.DataSource = null;
            grvDatosSO.DataBind();
        }
    }

    protected void grvDatosSO_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        } 
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        //  You can cache the DataTable for improving performance
        tblPista = (DataTable)ViewState["tblPistas"];

        DataView dv = new DataView(tblPista);
        dv.Sort = sortExpression + direction;

        grvDatosSO.DataSource = dv;
        grvDatosSO.DataBind();
    }
    protected void btnExportar_Click(object sender, EventArgs e)
    {

        int bander = 0;
        foreach (GridViewRow renglon in grvDatosSO.Rows)
        {
            CheckBox CbCan;

            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

            //Seleccionado y activo
            if (CbCan.Checked == true)
            {
                bander = 1;
                string objeto = HttpUtility.HtmlDecode(renglon.Cells[1].Text);
                
                dataTableAExcel(objeto, ddlTipo.SelectedValue);  
                return;
            }

        }
        if (bander != 1)
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varSelSOAP); 
        }


    }
    /// <summary>
    /// Exporta el registro seleccionado.
    /// </summary>
    /// <param name="objeto"></param>
    /// <param name="tipo"></param>
    private void dataTableAExcel(string objeto,string tipo)
    {
        ScriptManager SM = ScriptManager.GetCurrent(this);
        SM.RegisterPostBackControl(btnExportar);
        if (!string.IsNullOrEmpty(objeto))
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(objeto);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.text";
            Response.AddHeader("Content-Disposition", "attachment;filename="+ tipo +"-"+ DateTime.Now.ToString("s") + ".txt");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
    }


    protected void ddlOrigenAcuses_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarUUID();
    }
}
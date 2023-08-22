using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.Data;
using System.IO;

using System.Text;
using System.Configuration;
using System.Threading;
using System.Globalization;

public partial class Pantallas_Problemas_webConsultaProblemas : System.Web.UI.Page
{
    string SeleccioneUnValor = "(Seleccione Opcion)";
    clsInicioSesionUsuario datosUsuario;
    private clsBusquedaIncidentes gPRO;
    private clsIncidencias gINS;
    private clsUsuarios gDAL;
    private UtilityBarControl[] ListControls;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
        if (!IsPostBack)
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            fnCargarProblemas();
            fnObtieneUsuariosSoporte();
            fnCargaTipoProblemas();
            fncargaTicketProblemas();
        }
    }

    protected void AgregaOpcionSeleccione(System.Object sender, System.EventArgs e)
    {
        ((DropDownList)sender).Items.Insert(0, new ListItem(SeleccioneUnValor, string.Empty));
    }

    private void fncargaTicketProblemas()
    {
        try
        {
            gPRO = new clsBusquedaIncidentes();
            ddlticket.DataSource = gPRO.fnObtieneTicketProblema();
            ddlticket.DataTextField = "ticket_problema";
            ddlticket.DataValueField = "ticket_problema";
            ddlticket.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fncargaTicketProblemas", "webConsultaProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fncargaTicketProblemas", "webConsultaProblemas.aspx.cs");
        }
    }

    private void fnCargarProblemas()
    {
        gPRO = new clsBusquedaIncidentes();

        try
        {
            DataTable ds = new DataTable();
            ds = gPRO.fnObtieneProblemasporUsuario(datosUsuario.id_usuario);
            Session["dsgrid"] = ds.DefaultView;
            gdvIncidencias.DataSource = ds;
            gdvIncidencias.DataBind();
            Session["dsIncidencias"] = ds;
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargarProblemas", "webConsultaProblemas.aspx.cs");
            gdvIncidencias.DataSource = null;
            gdvIncidencias.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargarProblemas", "webConsultaProblemas.aspx.cs");
        }
    }

    private void fnObtieneUsuariosSoporte()
    {
        try
        {
            gINS = new clsIncidencias();
            ddlusuariosop.DataSource = gINS.fnObtieneUsuariosSoporte();
            ddlusuariosop.DataTextField = "usuario";
            ddlusuariosop.DataValueField = "id_usuario_soporte";
            ddlusuariosop.DataBind();
            ddlusuariosop.SelectedValue = Convert.ToString(datosUsuario.id_usuario);

        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnObtieneUsuariosSoporte", "webConsultaProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnObtieneUsuariosSoporte", "webConsultaProblemas.aspx.cs");
        }
    }

    private void fnCargaTipoProblemas()
    {
        try
        {
            gDAL = new clsUsuarios();
            ddltipoinc.DataSource = gDAL.fnCargarCatalogoTipoIncidencias();
            ddltipoinc.DataTextField = "tipo_incidente";
            ddltipoinc.DataValueField = "id_tipo_incidente";
            ddltipoinc.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaTipoProblemas", "webConsultaProblemas.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaTipoProblemas", "webConsultaProblemas.aspx.cs");
        }
    }

    protected void btnNuevoProblema_Click(object sender, EventArgs e)
    {
        Response.Redirect("webCatalogoProblemas.aspx");
        //pintargrid
    }
    protected void gdvIncidencias_SelectedIndexChanged1(object sender, EventArgs e)
    {
        gINS = new clsIncidencias();
        GridViewRow gvrFila = (GridViewRow)gdvIncidencias.SelectedRow;
        int psProblema = 0;
        psProblema = Convert.ToInt32(gdvIncidencias.SelectedDataKey.Value);
        Session["psIncidenteProblema"] = psProblema;

        Response.Redirect("webAtencionProblemas.aspx");
    }


    //Funcion para colorear los renglones del grid segun el nivel de urgencia de la incidencia
    public void fnpintarGrid()
    {
    
    
        foreach (GridViewRow renglon in gdvIncidencias.Rows)
        {
            Label Estatus;
            Estatus = ((Label)renglon.FindControl("lblestatus"));

            /* Label ps_idtipo;
             ps_idtipo = ((Label)renglon.FindControl("lblincidente"));
            ddltipoinc.SelectedValue = ps_idtipo.Text;*/
           
            Label tot;
            tot = ((Label)renglon.FindControl("lblurgencia"));

            string urgencia = tot.Text;

            if (urgencia == "A")
            {
                if (Estatus.Text != "C")
                {
                    renglon.Cells[6].BackColor = Color.FromName("#FF3300");// Color.FromName("#CC0000");
                }
               
            }
            if (urgencia == "M")
            {
                if (Estatus.Text != "C")
                {
                    renglon.Cells[6].BackColor = Color.FromName("#FFFF99");
                }
            }
            if (urgencia == "B")
            {
                //e.Row.BackColor = Color.FromName("#ffc7ce");
            }
            tot.Visible = false;

            Label horas;
            horas = ((Label)renglon.FindControl("lblTiempoR"));
       
            
                DateTime hora = Convert.ToDateTime(horas.Text);
                DateTime  minimo = Convert.ToDateTime("02:00:00");
                DateTime medio = Convert.ToDateTime("06:00:00");
                DateTime maximo = Convert.ToDateTime("08:00:00");
               


            if (hora.Hour <= minimo.Hour)
            {
                if (Estatus.Text == "C")
                {
                    horas.Text = "00:00:00";
                    renglon.Cells[1].BackColor = Color.FromName("#00CC66");
                }
                else
                {
                    renglon.Cells[7].BackColor = Color.FromName("#FF3300");
                    renglon.Cells[1].BackColor = Color.FromName("#FFFF99");
                }

                // Color.FromName("#CC0000");
            }
            if (hora.Hour > minimo.Hour && hora.Hour <= medio.Hour)
            {
                if (Estatus.Text == "C")
                {
                    horas.Text = "00:00:00";
                    renglon.Cells[1].BackColor = Color.FromName("#00CC66");
                }
                else
                {

                    renglon.Cells[7].BackColor = Color.FromName("#FFFF99");
                    renglon.Cells[1].BackColor = Color.FromName("#FFFF99");

                }
                //if estatus = C 
                // horas.Text = "Cerrado"
            }

            if (hora.Hour == maximo.Hour)
            {
                if (Estatus.Text == "C")
                {
                    horas.Text = "00:00:00";
                    renglon.Cells[1].BackColor = Color.FromName("#00CC66");
                }
                else
                {
                    renglon.Cells[1].BackColor = Color.FromName("#FFFF99");
                }
            }

            if (hora.Hour > minimo.Hour && hora.Hour < maximo.Hour)
            {
                if (Estatus.Text == "C")
                {
                    horas.Text = "00:00:00";
                    renglon.Cells[1].BackColor = Color.FromName("#00CC66");
                }
            }
            tot.Visible = false;
        }
    }
    protected void gdvIncidencias_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        fnpintarGrid();
    }
    protected void ddltipoinc_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void ddlusuariosop_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void btnBuscarB_Click(object sender, EventArgs e)
    {
        try
        {
            fnCargaGridFiltros();
            btnExcel.Enabled = true;
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnBuscarB_Click", "webConsultaProblemas.aspx.cs");

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnBuscarB_Click", "webConsultaProblemas.aspx.cs");
        }
    }


    private void fnCargaGridFiltros()
    {
     
        gPRO = new clsBusquedaIncidentes();
        string psestatusb = null,
            pstipoincb = null,
            psidusuariosopb = null,
             psFechastring2 = null,
            psFechastring = null,
            psticket = null;

        if (ddlestatus.SelectedValue != "N")
        {
            psestatusb = ddlestatus.SelectedValue;
        }

        if (ddltipoinc.SelectedValue != null)
        {
            pstipoincb = ddltipoinc.SelectedValue;
        }

        if (txtfechareg.Text != "")
        {
            DateTime fechacut = Convert.ToDateTime(txtfechareg.Text);
            psFechastring = fechacut.ToString("yyyyMMdd");

        }

        if (txtfechareg2.Text != "")
        {
            DateTime fechacut2 = Convert.ToDateTime(txtfechareg2.Text);
            psFechastring2 = fechacut2.ToString("yyyyMMdd");

        }

        if (ddlusuariosop.SelectedValue != null)
        {
            psidusuariosopb = ddlusuariosop.SelectedValue;
        }

        if (ddlticket.SelectedValue != null)
        {
            psticket = ddlticket.SelectedValue;
        }

        DataTable ds = new DataTable();
        ds = gPRO.fnGetProblemasbyFiltros(psestatusb, pstipoincb, psFechastring, psidusuariosopb, psFechastring2, psticket);
        Session["dsgrid"] = ds.DefaultView;
        gdvIncidencias.DataSource = ds;
        gdvIncidencias.DataBind();
        Session["dsIncidencias"] = ds;
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
            {
                ViewState["sortDirection"] = SortDirection.Ascending;
            }

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        DataView compro = new DataView();
        compro = ((DataView)Session["dsgrid"]);
        compro.Sort = sortExpression + " " + direction;
        gdvIncidencias.DataSource = compro;
        gdvIncidencias.DataBind();
        Session["dsIncidencias"] = compro;
        gdvIncidencias.Visible = true;

        foreach (GridViewRow renglon in gdvIncidencias.Rows)
        {
            
        }
    }
    protected void gdvIncidencias_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;


        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;

            SortGridView(sortExpression, "DESC");
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;

            SortGridView(sortExpression, "ASC");

        }
    }
    protected void Timer2_Tick(object sender, EventArgs e)
    {
        fnCargarProblemas();
        fnpintarGrid();
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        //fnPintardatatable();
        if (File.Exists(ExcelFullName))
        {
            File.Delete(ExcelFullName);
        }
        Panel PanelToExport = new Panel();

        Table TableToExport = new Table();
        TableRow Row = new TableRow();
        TableCell Cell = new TableCell();

        TableRow Row1 = new TableRow();
        TableRow Row2 = new TableRow();
        TableRow Row3 = new TableRow();
        TableRow Row4 = new TableRow();
        TableRow Row5 = new TableRow();
        TableCell Cell1 = new TableCell();
        TableCell Cell2 = new TableCell();
        TableCell Cell3 = new TableCell();
        TableCell Cell4 = new TableCell();
        TableCell Cell5 = new TableCell();
        if (ddlusuariosop.SelectedItem.Text != "(Seleccione Opcion)")
        {
            Cell1.Text = "Usuario: " + ddlusuariosop.SelectedItem.Text;
            Row1.Cells.Add(Cell1);
            TableToExport.Rows.Add(Row1);
        }
        else
        {
            Cell1.Text = "Usuario: Todos";
            Row1.Cells.Add(Cell1);
            TableToExport.Rows.Add(Row1);
        }
        if (ddltipoinc.SelectedItem.Text != "(Seleccione Opcion)")
        {
            Cell2.Text = " Tipo Problema: " + ddltipoinc.SelectedItem.Text;
            Row2.Cells.Add(Cell2);
            TableToExport.Rows.Add(Row2);
        }
        else
        {
            Cell2.Text = " Tipo Problema: Todos";
            Row2.Cells.Add(Cell2);
            TableToExport.Rows.Add(Row2);
        }
        if (ddlestatus.SelectedItem.Text != "(Seleccione Opcion)")
        {
            Cell3.Text = " Estatus: " + ddlestatus.SelectedItem.Text;
            Row3.Cells.Add(Cell3);
            TableToExport.Rows.Add(Row3);
        }
        else
        {
            Cell3.Text = " Estatus: Todos";
            Row3.Cells.Add(Cell3);
            TableToExport.Rows.Add(Row3);
        }
        if (ddlticket.SelectedItem.Text != "(Seleccione Opcion)")
        {
            Cell4.Text = " Ticket: " + ddlticket.SelectedItem.Text;
            Row4.Cells.Add(Cell4);
            TableToExport.Rows.Add(Row4);

        }
        else
        {
            Cell4.Text = " Ticket: Todos";
            Row4.Cells.Add(Cell4);
            TableToExport.Rows.Add(Row4);
        }
        if (txtfechareg.Text != "" && txtfechareg2.Text != "")
        {
            Cell5.Text = " Fecha Inicial: " + txtfechareg.Text + "| Fecha Final: " + txtfechareg2.Text;
            Row5.Cells.Add(Cell5);
            TableToExport.Rows.Add(Row5);
        }
        else
        {
            Cell5.Text = " Fecha Inicial: Todas" + "| Fecha Final: Todas";
            Row5.Cells.Add(Cell5);
            TableToExport.Rows.Add(Row5);
        }

        Cell = new TableCell();
        Row.Cells.Add(Cell);

        ExcelInsertGridView(Cell, gdvIncidencias, null);
        Row.Cells.Add(Cell);
        TableToExport.Rows.Add(Row);
        PanelToExport.Controls.Add(TableToExport);
        PanelToExport.Controls.Add(new LiteralControl("<br />"));


        Response.Clear();
        Response.Charset = "UTF-8";
        Response.ContentEncoding = Encoding.Default;
        Response.AddHeader("content-disposition", "attachment; filename=" + ExcelName + ExcelExtension);
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
        PanelToExport.RenderControl(htmlTextWriter);
        Response.Write(stringWriter.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        mpePanel.Show();
        fnpintarGrid();
    }

    #region General Properties
    private string TemporalFolder
    {
        get
        {
            return Server.MapPath("~/TemporalFiles/");
        }
    }
    public int FontSize
    {
        get
        {
            return (int)(ViewState["FontSize"] ?? 8);
        }
        set
        {
            ViewState["FontSize"] = value;
        }
    }
    public int BorderWidth
    {
        get
        {
            return (int)(ViewState["BorderWidth"] ?? 1);
        }
        set
        {
            ViewState["BorderWidth"] = value;
        }
    }
    #endregion

    #region Excel Properties
    public string ExcelName
    {
        get
        {
            string text = (string)ViewState["ExcelName"];
            if (text != null)
                return text;
            else
                return "Consulta de Problemas";
        }
        set
        {
            ViewState["ExcelName"] = value;
        }
    }
    private string ExcelExtension
    {
        get
        {
            return ".xls";
        }
    }
    private string ExcelFullName
    {
        get
        {
            return clsComun.fnObtenerParamentro("RutaExcel") + ExcelName;
        }
    }
    #endregion


    private void ExcelInsertGridView(TableCell CellToExport, GridView GridViewControl, string Size)
    {
        fnpintarGrid();
        if (GridViewControl.Visible == false)
        {
            return;
        }
        Table myTable = new Table();
        myTable.CellSpacing = 0;
        TableRow myRow;
        TableCell myCell;
        GridViewRow Row;
        TableCell Cell;
        HyperLink HyperLinkControl;
        System.Web.UI.WebControls.Label LabelControl;
        System.Web.UI.WebControls.TextBox TextBoxControl;
        LinkButton LinkButtonControl;
        GridView SubGridViewControl;
        int LastColumnSpan = 0;
       // DataTable ds = fnPintardatatable();
        //Fill Tittle
        int Columns = 0;
        for (int i = 0; i < GridViewControl.Columns.Count; i++)
        {
            if (GridViewControl.Columns[i].Visible == true)
            {
                Columns = Columns + 1;
            }
        }
        myRow = new TableRow();
        if ((Size == "ToRightW") || (Size == "ToDownW"))
        {
            myCell = new TableCell();
            if (GridViewControl.FooterRow.Visible == true)
            {
                myCell.RowSpan = GridViewControl.Rows.Count + 3;
            }
            else
            {
                myCell.RowSpan = GridViewControl.Rows.Count + 2;
            }
            myRow.Cells.Add(myCell);
        }
        myCell = new TableCell();
        myCell.ColumnSpan = Columns;
        myCell.Font.Name = "Arial";
        myCell.Font.Size = FontSize + 2;
        myCell.Font.Bold = true;
        myCell.BorderWidth = BorderWidth;
        myCell.ForeColor = System.Drawing.Color.Navy;
        myCell.BackColor = System.Drawing.Color.LightBlue;
        myCell.HorizontalAlign = HorizontalAlign.Center;
        if (GridViewControl.Caption != "")
        {
            myCell.Text = GridViewControl.Caption;
            myRow.Cells.Add(myCell);
            myTable.Rows.Add(myRow);
        }
        //Fill Headers
        myRow = new TableRow();
        if (((Size == "ToRightW") || (Size == "ToDownW")) && (GridViewControl.Caption == ""))
        {
            myCell = new TableCell();
            if (GridViewControl.FooterRow.Visible == true)
            {
                myCell.RowSpan = GridViewControl.Rows.Count + 2;
            }
            else
            {
                myCell.RowSpan = GridViewControl.Rows.Count + 1;
            }
            myRow.Cells.Add(myCell);
        }
        for (int i = 0; i < GridViewControl.Columns.Count; i++)
        {
            if ((GridViewControl.Columns[i].Visible == true) && (LastColumnSpan <= 0) && (GridViewControl.Rows.Count > 0))
            //if ((LastColumnSpan <= 0) && (GridViewControl.Rows.Count > 0))
            {
                if (GridViewControl.HeaderRow.HasControls())
                {
                    myCell = new TableCell();
                    myCell.Font.Name = "Arial";
                    myCell.Font.Size = FontSize;
                    myCell.Font.Bold = true;
                    myCell.BorderWidth = BorderWidth;
                    myCell.ForeColor = System.Drawing.Color.Navy;
                    myCell.BackColor = System.Drawing.Color.LightBlue;
                    myCell.ColumnSpan = GridViewControl.HeaderRow.Cells[i].ColumnSpan;
                    LastColumnSpan = myCell.ColumnSpan;
                    if (GridViewControl.HeaderRow.Cells[i].HasControls())
                    {
                        foreach (Control myControl in GridViewControl.HeaderRow.Cells[i].Controls)
                        {
                            if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.HyperLink"))
                            {
                                HyperLinkControl = (HyperLink)myControl;
                                myCell.Text = HyperLinkControl.Text;
                            }
                            else if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                            {
                                LabelControl = (System.Web.UI.WebControls.Label)myControl;
                                myCell.Text = LabelControl.Text;
                            }
                            else if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.DataControlLinkButton"))
                            {
                                LinkButtonControl = (LinkButton)myControl;
                                myCell.Text = LinkButtonControl.Text;
                            }
                        }
                    }
                    else
                    {
                        myCell.Text = GridViewControl.HeaderRow.Cells[i].Text;
                    }
                    myRow.Cells.Add(myCell);
                }
            }
            LastColumnSpan = LastColumnSpan - 1;
        }
        myTable.Rows.Add(myRow);
        //Fill Data
        LastColumnSpan = 0;
        for (int i = 0; i < GridViewControl.Rows.Count; i++)
        {
            myRow = new TableRow();
            Row = GridViewControl.Rows[i];
            for (int j = 0; j < GridViewControl.Columns.Count; j++)
            {
                if ((GridViewControl.Columns[j].Visible == true) && (LastColumnSpan <= 0) && (GridViewControl.Rows.Count > 0))
                //if ((LastColumnSpan <= 0) && (GridViewControl.Rows.Count > 0))
                {
                    Cell = Row.Cells[j];
                    myCell = new TableCell();
                    myCell.Font.Name = "Arial";
                    myCell.Font.Size = FontSize;
                    myCell.BorderWidth = BorderWidth;
                    if (GridViewControl.Columns[j] == GridViewControl.Columns[1])
                    {
                        //myCell.BackColor = ds.Rows[i].;
                    }
                    myCell.ColumnSpan = Row.Cells[j].ColumnSpan;
                    LastColumnSpan = myCell.ColumnSpan;
                    if (!Cell.ForeColor.IsEmpty)
                    {
                        myCell.ForeColor = Cell.ForeColor;
                    }
                    else
                    {
                        if (!Row.ForeColor.IsEmpty)
                        {
                            myCell.ForeColor = Row.ForeColor;
                        }
                    }
                    if (!Cell.BackColor.IsEmpty)
                    {
                        myCell.BackColor = Cell.BackColor;
                    }
                    else
                    {
                        if (!Row.BackColor.IsEmpty)
                        {
                            myCell.BackColor = Row.BackColor;
                        }
                    }
                    if (Row.Cells[j].HasControls())
                    {
                        foreach (Control myControl in Row.Cells[j].Controls)
                        {
                            if (myControl.Visible)
                            {
                                if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.HyperLink"))
                                {
                                    HyperLinkControl = (HyperLink)myControl;
                                    if (!HyperLinkControl.ForeColor.IsEmpty)
                                    {
                                        myCell.ForeColor = HyperLinkControl.ForeColor;
                                    }
                                    myCell.Text = HyperLinkControl.Text;
                                }
                                else if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                                {
                                    LabelControl = (System.Web.UI.WebControls.Label)myControl;
                                    if (!LabelControl.ForeColor.IsEmpty)
                                    {
                                        myCell.ForeColor = LabelControl.ForeColor;
                                    }
                                    myCell.Text = LabelControl.Text;
                                }
                                else if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                                {
                                    TextBoxControl = (System.Web.UI.WebControls.TextBox)myControl;
                                    myCell.Text = myCell.Text + "[" + TextBoxControl.Text + "]";
                                }
                                else if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.DataControlLinkButton"))
                                {
                                    LinkButtonControl = (LinkButton)myControl;
                                    myCell.Text = LinkButtonControl.Text;
                                }
                                else if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.LinkButton"))
                                {
                                    LinkButtonControl = (LinkButton)myControl;
                                    myCell.Text = LinkButtonControl.Text;
                                }
                                else if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.GridView"))
                                {
                                    SubGridViewControl = (GridView)myControl;
                                    ExcelInsertGridView(myCell, SubGridViewControl, "Short");
                                }
                            }
                        }
                    }
                    else
                    {
                        myCell.Text = Row.Cells[j].Text;
                    }
                    myRow.Cells.Add(myCell);
                }
                LastColumnSpan = LastColumnSpan - 1;
            }
            myTable.Rows.Add(myRow);
        }
        //Fill Footer
        DataBoundLiteralControl dataBoundLiteralControl;
        if (GridViewControl.FooterRow != null)
        {
            if (GridViewControl.FooterRow.Visible == true)
            {
                myRow = new TableRow();
                for (int i = 0; i < GridViewControl.FooterRow.Cells.Count; i++)
                {
                    if ((GridViewControl.FooterRow.Cells[i].Visible == true) && (GridViewControl.Rows.Count > 0) && (GridViewControl.Columns[i].Visible == true))
                    {
                        myCell = new TableCell();
                        myCell.Font.Name = "Arial";
                        myCell.Font.Size = FontSize;
                        myCell.Font.Bold = true;
                        myCell.BorderWidth = BorderWidth;
                        myCell.ForeColor = System.Drawing.Color.Navy;
                        myCell.BackColor = System.Drawing.Color.LightBlue;
                        myCell.ColumnSpan = GridViewControl.FooterRow.Cells[i].ColumnSpan;
                        if (GridViewControl.FooterRow.Cells[i].HasControls())
                        {
                            foreach (Control myControl in GridViewControl.FooterRow.Cells[i].Controls)
                            {
                                if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.HyperLink"))
                                {
                                    HyperLinkControl = (HyperLink)myControl;
                                    myCell.Text = HyperLinkControl.Text;
                                }
                                else if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.Label"))
                                {
                                    LabelControl = (System.Web.UI.WebControls.Label)myControl;
                                    myCell.Text = LabelControl.Text;
                                }
                                else if (myControl.GetType().ToString().Equals("System.Web.UI.WebControls.DataControlLinkButton"))
                                {
                                    LinkButtonControl = (LinkButton)myControl;
                                    myCell.Text = LinkButtonControl.Text;
                                }
                                else if (myControl.GetType().ToString().Equals("System.Web.UI.DataBoundLiteralControl"))
                                {
                                    dataBoundLiteralControl = (DataBoundLiteralControl)myControl;
                                    myCell.Text = dataBoundLiteralControl.Text;
                                }
                            }
                        }
                        else
                        {
                            myCell.Text = GridViewControl.FooterRow.Cells[i].Text;
                        }
                        myRow.Cells.Add(myCell);
                    }
                }
                myTable.Rows.Add(myRow);
            }
        }
        CellToExport.Controls.Add(myTable);
    }


    #region Other Properties and Methods
    //---
    public string HeaderName
    {
        get
        {
            string text = (string)ViewState["HeaderName"];
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            ViewState["HeaderName"] = value;
        }
    }
    public string ReportName
    {
        get
        {
            string text = (string)ViewState["ReportName"];
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            ViewState["ReportName"] = value;
        }
    }
    public string StartingDate
    {
        get
        {
            string text = (string)ViewState["StartingDate"];
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            ViewState["StartingDate"] = value;
        }
    }
    public string EndingDate
    {
        get
        {
            string text = (string)ViewState["EndingDate"];
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            ViewState["EndingDate"] = value;
        }
    }
    public string Shift
    {
        get
        {
            string text = (string)ViewState["Shift"];
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            ViewState["Shift"] = value;
        }
    }
    public string User
    {
        get
        {
            string text = (string)ViewState["User"];
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            ViewState["User"] = value;
        }
    }
    //---
    //---
    public string Structure = string.Empty;
    public string FacilityId = string.Empty;
    public string ItemId = string.Empty;
    public string LblStructure
    {
        get
        {
            string text = (string)ViewState["LblStructure"];
            if (text != null)
                return text;
            else
                return string.Empty;
        }
        set
        {
            ViewState["LblStructure"] = value;
        }
    }
    //---
    //---



    //---
    //---
    private string DateFormat(string Date)
    {
        string stringDate = "";
        DateTime date = new DateTime();
        if (DateTime.TryParse(Date, out date))
        {
            stringDate = String.Format("{0,2:00}/{1,2:00}/{2,2:00}", date.Day, date.Month, date.Year);
        }
        else
        {
            stringDate = Date;
            //stringDate = "0/0/0";
        }
        return stringDate;
    }
    //---
    #endregion

    private class UtilityBarControl
    {
        private string _Type;
        public string Type
        {
            get
            {
                return _Type;
            }
        }
        private Label _LabelControl;
        public Label LabelControl
        {
            get
            {
                return _LabelControl;
            }
        }
        //private Chart _ChartControl;
        //public Chart ChartControl
        //{
        //    get
        //    {
        //        return _ChartControl;
        //    }
        //}
        private GridView _GridViewControl;
        public GridView GridViewControl
        {
            get
            {
                return _GridViewControl;
            }
        }
        public string _Size;
        public string Size
        {
            get
            {
                return _Size;
            }
        }
        private string _TextControl;
        public string TextControl
        {
            get
            {
                return _TextControl;
            }
        }
        public UtilityBarControl(Label LabelControl)
        {
            _Type = "Label";
            _LabelControl = LabelControl;
            _Size = "NewLine";
        }
        public UtilityBarControl(Label LabelControl, string Size)
        {
            _Type = "Label";
            _LabelControl = LabelControl;
            _Size = Size;
        }
        //public UtilityBarControl(Chart ChartControl, string Size)
        //{
        //    _Type = "Chart";
        //    _ChartControl = ChartControl;
        //    _Size = Size;
        //}
        public UtilityBarControl(GridView GridViewControl, string Size)
        {
            _Type = "GridView";
            _GridViewControl = GridViewControl;
            _Size = Size;
        }
        public UtilityBarControl(string TextControl)
        {
            _Type = "Text";
            _TextControl = TextControl;
            _Size = "NoSize";
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
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
        }
    }
}
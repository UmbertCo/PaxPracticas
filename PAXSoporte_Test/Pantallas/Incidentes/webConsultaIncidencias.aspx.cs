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
using System.Threading;
using System.Globalization;
public partial class Pantallas_Incidentes_webConsultaIncidencias : System.Web.UI.Page
{
    clsInicioSesionUsuario datosUsuario;
    private clsBusquedaIncidentes gPRO;
    private clsIncidencias gINS;
    private clsUsuarios gDAL;
    string  SeleccioneUnValor = "(Seleccione Opcion)";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["objUsuario"] == null)
        {
            Response.Redirect("~/Pantallas/Login/webInicioSesionLogin.aspx");
        }
        datosUsuario = clsComun.fnUsuarioEnSesion();
        if (!IsPostBack)            
        {
            
            fnCargarIncidencias();
              fnObtieneUsuariosSoporte();
              fnCargaTipoIncidencias();
              fncargaTicketIncidencias();
              fncargaTicketProblema();
        }
        
    }
    protected void AgregaOpcionSeleccione(System.Object sender, System.EventArgs e)
    {
        ((DropDownList)sender).Items.Insert(0, new ListItem(SeleccioneUnValor, string.Empty));
    }
    private void fncargaTicketProblema()
    {
           gINS = new clsIncidencias();
           txtticketpro.DataSource = gINS.fnObtieneTicketProblemadeIncidencia();
           txtticketpro.DataTextField = "ticket_problema";
           txtticketpro.DataValueField = "ticket_problema";
           txtticketpro.DataBind();
       
    }
    private void fncargaTicketIncidencias()
    {
         try
        {
            gINS = new clsIncidencias();
            ddlticket.DataSource = gINS.fnObtieneTicketIncidencia();
            ddlticket.DataTextField = "ticket";
            ddlticket.DataValueField = "ticket";
            ddlticket.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fncargaTicketIncidencias", "webConsultaIncidencias.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fncargaTicketIncidencias", "webConsultaIncidencias.aspx.cs");
        }
    }
    private void fnCargarIncidencias()
    {
        gINS = new clsIncidencias();

        try
        {                      
            DataTable ds = new DataTable();
            ds = gINS.fnObtieneIncidenciasporUsuario(datosUsuario.id_usuario);
            Session["dsgrid"] = ds.DefaultView;
            gdvIncidencias.DataSource = ds;
            gdvIncidencias.DataBind();
            
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargarIncidencias", "webConsultaIncidencias.aspx.cs");
            gdvIncidencias.DataSource = null;
            gdvIncidencias.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargarIncidencias", "webConsultaIncidencias.aspx.cs");
        }
    }
    protected void gdvIncidencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvIncidencias.PageIndex = e.NewPageIndex;
        fnCargarIncidencias();
    }

    protected void gdvIncidencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        //obtener los valores del usuario seleccionado en el grid
        GridViewRow gvrFila = (GridViewRow)gdvIncidencias.SelectedRow;
        Session["psTicket"] = ((Label)gvrFila.FindControl("lblTicket")).Text;
    }
   


    protected void gdvIncidencias_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        fnpintarGrid();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Attributes.Add("class", "text");
        }
    }
    protected void btnProblema_Click(object sender, EventArgs e)
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            int bandera = 0;
            int contador = 0;
            int banderaproblema = 0;
            string psDescripcion = null;
            List<string> tickets = new List<string>();
            foreach (GridViewRow renglon in gdvIncidencias.Rows)
            {
                
                CheckBox CbProb;

                //string[] tickets = null;
                CbProb = (CheckBox)(renglon.FindControl("cbProblema"));
                

                if (CbProb.Checked)
                {
                    Label pTipoInc;
                    LinkButton pTicketProbelma;
                    pTicketProbelma = ((LinkButton)renglon.FindControl("lblticketproblema"));
                    if (string.IsNullOrEmpty(pTicketProbelma.Text))
                    {
                        Label pidIncidencia, pDesc, pTicketI;
                        pidIncidencia = ((Label)renglon.FindControl("lblidincidencia"));
                        pDesc = ((Label)renglon.FindControl("lbldescripcion"));
                        pTicketI = ((Label)renglon.FindControl("lblticket"));
                        psDescripcion += "Ticket Incidencia: " + pTicketI.Text + " \n" + " Descripción: " + pDesc.Text + " " + " \n" + " \n";
                        string final;
                        final = pidIncidencia.Text;
                        tickets.Add(final);
                        contador += 1;
                        bandera = 1;
                        pTipoInc = ((Label)renglon.FindControl("lblincidente"));
                        Session["lbltipoinc"] = pTipoInc.Text;
                    }
                    else
                    {
                        banderaproblema = 1;
                    }                              
                }
            }

          
            if (bandera == 0)
            {
                if (banderaproblema == 1)
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblTicketProblemaGenerado);
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblSinSeleccionarGrid);
                    fnpintarGrid();
                }
            }
               
            else
            {

                        gINS = new clsIncidencias();
                        gPRO = new clsBusquedaIncidentes();

                        DataTable Tabla = new DataTable("usuarios");
                        Tabla.Columns.Add("idusuario", typeof(Int32));
                        Tabla.Columns.Add("carga", typeof(Int32));

                        string sTicket = clsGeneraLlaves.GenerarTicket();
                        int pstipo = Convert.ToInt32(Session["lbltipoinc"]);
                        DataSet dsUsuario = gPRO.fnObtieneUsuarioSoporte(pstipo);
                        foreach (DataRow renglon in dsUsuario.Tables[0].Rows)
                       
                        {
                            DataTable dsCarga = new DataTable();
                            dsCarga = gPRO.fnObtieneProblemasporUsuario(Convert.ToInt32(renglon["id_usuario_soporte"]));
                            DataRow nuevo;
                            nuevo = Tabla.NewRow();
                            nuevo["idusuario"] = Convert.ToInt32(renglon["id_usuario_soporte"]);
                            nuevo["carga"] = Convert.ToInt32(dsCarga.Rows.Count);
                            Tabla.Rows.Add(nuevo);
                        }
                        DataTable dtOrdenado = FiltrarDataTable(Tabla, "", "carga ASC");

                        int psIdPromebla = gPRO.fnProblemasIns(sTicket, datosUsuario.id_usuario, pstipo, Convert.ToInt32(dtOrdenado.Rows[0]["idusuario"]), psDescripcion);
                            foreach (string incidencia in tickets)
                            {
                                int idIncPro = 0;
                                idIncPro = Convert.ToInt32(incidencia);
                                gINS.fnActualizaTicketProblemaenIncidencias(idIncPro, sTicket);
                                gPRO.fnProblemasInsRel(idIncPro, psIdPromebla);
                            }                                               
                        
                                           
                    fnCargarIncidencias();
                    fnpintarGrid();
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblConfirmaProblema);
               
                //} }
                  

            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnProblema_Click", "webConsultaIncidencias.aspx.cs");
          
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnProblema_Click", "webConsultaIncidencias.aspx.cs");
        }
    }

    public DataTable FiltrarDataTable(DataTable dt, string filtro, string orden)
    {
        DataRow[] rows;
        DataTable dtNew;

        try
        {
            dtNew = dt.Clone();
            rows = dt.Select(filtro, orden);

            Array.ForEach(rows, dtNew.ImportRow);

            return dtNew;
        }
        catch (Exception ex)
        {
            throw new Exception(String.Format("FiltrarDataTable - {0} - {1}", ex.Source, ex.Message));
        }
    }

    //Funcion para colorear los renglones del grid segun el nivel de urgencia de la incidencia
    public void fnpintarGrid()
    {
        foreach (GridViewRow renglon in gdvIncidencias.Rows)
        {
            Label Estatus;
            Estatus = ((Label)renglon.FindControl("lblestatus"));

            /*Label ps_idtipo;
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
                DateTime minimo = Convert.ToDateTime("02:00:00");
                DateTime medio = Convert.ToDateTime("06:00:00");
                DateTime maximo = Convert.ToDateTime("08:00:00");

                LinkButton Ticketp;
                Ticketp = ((LinkButton)renglon.FindControl("lblticketproblema"));
           
                if (hora.Hour <= minimo.Hour)
                {
                    if (Estatus.Text == "C")
                    {
                        horas.Text = "00:00:00";
                        renglon.Cells[1].BackColor = Color.FromName("#00CC66");
                        if (Ticketp.Text != "")
                        {
                            renglon.Cells[10].BackColor = Color.FromName("#00CC66");
                        }
                    }
                    else
                    {
                        renglon.Cells[7].BackColor = Color.FromName("#FF3300");
                        renglon.Cells[1].BackColor = Color.FromName("#FFFF99");
                        if (Ticketp.Text != "")
                        {
                            renglon.Cells[10].BackColor = Color.FromName("#FFFF99");
                        }
                    }

                    // Color.FromName("#CC0000");
                }
                if (hora.Hour > minimo.Hour && hora.Hour <= medio.Hour)
                {
                    if (Estatus.Text == "C")
                    {
                        horas.Text = "00:00:00";
                        renglon.Cells[1].BackColor = Color.FromName("#00CC66");
                        if (Ticketp.Text != "")
                        {
                            renglon.Cells[10].BackColor = Color.FromName("#00CC66");
                        }
                    }
                    else
                    {
                       
                            renglon.Cells[7].BackColor = Color.FromName("#FFFF99");
                            renglon.Cells[1].BackColor = Color.FromName("#FFFF99");
                            if (Ticketp.Text != "")
                            {
                                renglon.Cells[10].BackColor = Color.FromName("#FFFF99");
                            }                                  
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
                        if (Ticketp.Text != "")
                        {
                            renglon.Cells[10].BackColor = Color.FromName("#00CC66");
                        }
                    }
                    else
                    {
                        renglon.Cells[1].BackColor = Color.FromName("#FFFF99");
                        if (Ticketp.Text != "")
                        {
                            renglon.Cells[10].BackColor = Color.FromName("#FFFF99");
                        }
                    }
                }

                if (hora.Hour > minimo.Hour && hora.Hour < maximo.Hour)
                {
                    if (Estatus.Text == "C")
                    {
                        horas.Text = "00:00:00";
                        renglon.Cells[1].BackColor = Color.FromName("#00CC66");
                        if (Ticketp.Text != "")
                        {
                            renglon.Cells[10].BackColor = Color.FromName("#00CC66");
                        }
                    }
                    else
                    {
                        renglon.Cells[1].BackColor = Color.FromName("#FFFF99");
                        if (Ticketp.Text != "")
                        {
                            renglon.Cells[10].BackColor = Color.FromName("#FFFF99");
                        }
                    }
                }
               
                tot.Visible = false;

        }
    }
    protected void btnNuevaIncidencia_Click1(object sender, EventArgs e)
    {
        Response.Redirect("webCatalogoIncidencias.aspx");
        fnpintarGrid();
    }
    protected void gdvIncidencias_SelectedIndexChanged1(object sender, EventArgs e)
    {
        gINS = new clsIncidencias();
        GridViewRow gvrFila = (GridViewRow)gdvIncidencias.SelectedRow;
        int psIncidente = 0;
        psIncidente = Convert.ToInt32(gdvIncidencias.SelectedDataKey.Value);
        Session["psIncidenteIncidencia"] = psIncidente;
       
        Response.Redirect("webAtencionIncidencias.aspx");
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "btnBuscarB_Click", "webConsultaIncidencias.aspx.cs");

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "btnBuscarB_Click", "webConsultaIncidencias.aspx.cs");
        }
    }
  
    private void fnCargaTipoIncidencias()
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnCargaTipoIncidencias", "webConsultaIncidencias.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnCargaTipoIncidencias", "webConsultaIncidencias.aspx.cs");
        }
    }

    private void fnObtieneUsuariosSoporte()
    {
        try
        {
        gINS = new clsIncidencias();
        ddlusuariosop .DataSource = gINS.fnObtieneUsuariosSoporte();
        ddlusuariosop.DataTextField = "usuario";
        ddlusuariosop.DataValueField = "id_usuario_soporte";
        ddlusuariosop.DataBind();
        ddlusuariosop.SelectedValue = Convert.ToString(datosUsuario.id_usuario);

        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnObtieneUsuariosSoporte", "webConsultaIncidencias.aspx.cs");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnObtieneUsuariosSoporte", "webConsultaIncidencias.aspx.cs");
        }
    }

    private void fnCargaGridFiltros()
    {
        gINS = new clsIncidencias();

        string psestatusb = null,
            pstipoincb = null,          
            psidusuariosopb = null,
            psticketprob = null,
             psFechastring2 = null,
            psFechastring = null,
           psticket = null;

        DateTime psfecharegb;
        
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

        if (txtticketpro.SelectedValue  != null)
        {
            psticketprob = txtticketpro.SelectedValue;
        }

        if (ddlticket.SelectedValue != null)
        {
            psticket = ddlticket.SelectedValue;
        }


        DataTable ds = new DataTable();
        ds = gINS.fnGetIncidenciasbyFiltros(psestatusb, pstipoincb, psFechastring, psidusuariosopb, psticketprob, psFechastring2, psticket);
        Session["dsgrid"] = ds.DefaultView;
        gdvIncidencias.DataSource = ds;
        gdvIncidencias.DataBind();  
    }
    protected void ddlusuariosop_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void ddltipoinc_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void gdvIncidencias_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Problema")
        {
            Object Llave = e.CommandSource;
            LinkButton boton = ((LinkButton)Llave);
            gINS = new clsIncidencias();
           int pIdProblemas =  gINS.fnObtieneIdProblemabyTicket(boton.Text);
            Session["psIncidenteProblema"] = pIdProblemas;
            Response.Redirect("~/Pantallas/Problemas/webAtencionProblemas.aspx");
        }
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
        compro =  ((DataView)Session["dsgrid"]);
        compro.Sort = sortExpression + " " + direction;
        gdvIncidencias.DataSource = compro;
        gdvIncidencias.DataBind();
        gdvIncidencias.Visible = true;
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
        fnCargarIncidencias();
        fnpintarGrid();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
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
        TableRow Row6 = new TableRow();
        TableCell Cell1 = new TableCell();
        TableCell Cell2 = new TableCell();
        TableCell Cell3 = new TableCell();
        TableCell Cell4 = new TableCell();
        TableCell Cell5 = new TableCell();
        TableCell Cell6 = new TableCell();
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
        if (txtticketpro.Text != "")
        {
            Cell5.Text = " Ticket Problema: " + txtticketpro.Text;
            Row5.Cells.Add(Cell5);
            TableToExport.Rows.Add(Row5);
        }
        else
        {
            Cell5.Text = " Ticket Problema: Todos";
            Row5.Cells.Add(Cell5);
            TableToExport.Rows.Add(Row5);
        }
        if (txtfechareg.Text != "" && txtfechareg2.Text != "")
        {
            Cell6.Text = " Fecha Inicial: " + txtfechareg.Text + "| Fecha Final: " + txtfechareg2.Text;
            Row6.Cells.Add(Cell6);
            TableToExport.Rows.Add(Row6);
        }
        else
        {
            Cell6.Text = " Fecha Inicial: Todas" + "| Fecha Final: Todas";
            Row6.Cells.Add(Cell6);
            TableToExport.Rows.Add(Row6);
        }
        Cell = new TableCell();
        Row.Cells.Add(Cell);

        ExcelInsertGridView(Cell, gdvIncidencias, null);
        Row.Cells.Add(Cell);
        TableToExport.Rows.Add(Row);
        PanelToExport.Controls.Add(TableToExport);
        PanelToExport.Controls.Add(new LiteralControl("<br />"));


        Response.Clear();
        //Response.ContentEncoding = Encoding.UTF7;
        //Response.Charset = "UTF-7";

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
    protected void txtticketpro_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
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
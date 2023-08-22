using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Data;

public partial class webGlobalPistas : System.Web.UI.Page
{
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    DataTable tblPista;
    DataSet tblPistaBD;
    DataSet tblPistaSO;
    clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            fnCargarUsuarios();
            //fnCargarPistas(null, null, txtFechaIni.Text, txtFechaFin.Text);

            //Establecemos el filtro de fechas para el día d ehoy
            txtFechaFin_CalendarExtender.SelectedDate = DateTime.Now;
            txtFechaIni_CalendarExtender.SelectedDate = DateTime.Now;

            txtFechaIniBd_Calendarextender.SelectedDate = DateTime.Now;
            txtFechaFinBd_Calendarextender.SelectedDate = DateTime.Now;

            txtFechaIniSO_Calendarextender.SelectedDate = DateTime.Now;
            txtFechaFinSO_Calendarextender.SelectedDate = DateTime.Now;


            //fnCargarPistasBD(ddlUsuario.SelectedValue, txtConsulta.Text, txtConsulta2.Text, 
            //    txtConsulta3.Text, txtFechaIniBd.Text, txtFechaFinBd.Text);

            //fnCargarPistasSO(ddlUsuarioSO.SelectedValue, txtConsultaSO.Text, txtConsulta2SO.Text,
            //             txtConsulta3SO.Text, txtFechaIniSO.Text, txtFechaFinSO.Text, ddlTipo.SelectedValue, ddlSource.SelectedValue, ddlInstancia.SelectedValue );

            TabContainer.ActiveTabIndex= 0; 
 
        }
    }
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        string FechaIni = string.Empty;
        string FechaFin = string.Empty;

        DateTime startingDate = Convert.ToDateTime(txtFechaIni.Text);
        DateTime endingDate = Convert.ToDateTime(txtFechaFin.Text);

        FechaIni = startingDate.ToString("yyyyMMdd");
        FechaFin = endingDate.ToString("yyyyMMdd");

        datosUsuario = clsComun.fnUsuarioEnSesion();

        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "fnCargarPistas" + "|" + "Consulta las pistas de auditoria del aplicativo." + "| Usuario: " + datosUsuario.userName + "| Fecha: " + DateTime.Now);
        fnCargarPistas(ddlReceptor.SelectedValue, txtAccion.Text, txtAccion2.Text, txtAccion3.Text, FechaIni, FechaFin);
    }
    /// <summary>
    /// Carga los usuarios activos para usarlos en los filtros
    /// </summary>
    private void fnCargarUsuarios()
    {
        try
        {
            ddlReceptor.DataSource = clsComun.fnObtenerUsuarios();
            ddlReceptor.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlReceptor.DataSource = null;
            ddlReceptor.DataBind();
        }
    }


    /// <summary>
    /// Carga la lista de pistas
    /// </summary>
    private void fnCargarPistas(string psUsuario, string psAccion,
                                string psAccion2, string psAccion3,
                                string sFechaReg, string sFechaReg2)
    {
        try
        {
            tblPista = clsComun.fnObtenerPistas(psUsuario, psAccion, psAccion2, psAccion3, sFechaReg, sFechaReg2,"1");
            ViewState["tblPistas"] = tblPista;
            gdvComprobantes.DataSource = tblPista;
            gdvComprobantes.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            gdvComprobantes.DataSource = null;
            gdvComprobantes.DataBind();
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


    //Sorting Grid 
    protected void gdvComprobantes_Sorting(object sender, GridViewSortEventArgs e)
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

        gdvComprobantes.DataSource = dv;
        gdvComprobantes.DataBind();
    }

    protected void btnConsultaBD_Click(object sender, EventArgs e)
    {
        string usuario;

        string FechaIni = string.Empty;
        string FechaFin = string.Empty;

        DateTime startingDate = Convert.ToDateTime(txtFechaIniBd.Text);
        DateTime endingDate = Convert.ToDateTime(txtFechaFinBd.Text);

        FechaIni = startingDate.ToString("yyyyMMdd");
        FechaFin = endingDate.ToString("yyyyMMdd");

        if (ddlUsuario.SelectedItem.Text == "Todos")
            usuario = string.Empty;
        else
            usuario = ddlUsuario.SelectedItem.Text;

        datosUsuario = clsComun.fnUsuarioEnSesion();

        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "fnCargarPistasBD" + "|" + "Consulta las pistas de auditoria de Base de Datos." + "| Usuario: " + datosUsuario.userName + "| Fecha: " + DateTime.Now);
        fnCargarPistasBD(usuario, txtConsulta.Text, txtConsulta2.Text, txtConsulta3.Text, FechaIni, FechaFin);
    }

    /// <summary>
    /// Carga la lista de pistas de BD
    /// </summary>
    private void fnCargarPistasBD(string psUsuario, string psAccion,
                                string psAccion2, string psAccion3,
                                string sFechaReg, string sFechaReg2)
    {
        try
        {
            DataTable tblUsuario = new DataTable();
            DataTable tblInstancia = new DataTable();
            DataTable tblArchivo = new DataTable();
            DataTable tblDatos = new DataTable();


            tblPistaBD = clsComun.fnObtenerPistasBD(psUsuario, psAccion, psAccion2, psAccion3, sFechaReg, sFechaReg2);


            tblUsuario = (DataTable)tblPistaBD.Tables["Table"];
            tblInstancia = (DataTable)tblPistaBD.Tables["Table1"];
            tblArchivo = (DataTable)tblPistaBD.Tables["Table2"];
            tblDatos = (DataTable)tblPistaBD.Tables["Table3"];

            ddlUsuario.DataSource = tblUsuario;
            ddlUsuario.DataBind();

            lblInstanciaVal.Text = tblInstancia.Rows[0]["Instancia"].ToString();

            lblArchivoVal.Text = tblArchivo.Rows[0]["Archivo"].ToString();

            if (!string.IsNullOrEmpty(sFechaReg) && !string.IsNullOrEmpty(sFechaReg2))
            {
                ViewState["tblPistasBD"] = tblDatos;
                grvDatosBd.DataSource = tblDatos;
                grvDatosBd.DataBind();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            grvDatosBd.DataSource = null;
            grvDatosBd.DataBind();
        }
    }

    //Sorting Grid 
    protected void grvDatosBd_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridViewBd(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridViewBd(sortExpression, ASCENDING);
        }
    }

    private void SortGridViewBd(string sortExpression, string direction)
    {
        //  You can cache the DataTable for improving performance
        tblPista = (DataTable)ViewState["tblPistasBD"];

        DataView dv = new DataView(tblPista);
        dv.Sort = sortExpression + direction;

        grvDatosBd.DataSource = dv;
        grvDatosBd.DataBind();
    }
    protected void btnConsultaSO_Click(object sender, EventArgs e)
    {
        string usuario;
        string entrytype;
        string source;

        string FechaIni = string.Empty;
        string FechaFin = string.Empty;

        DateTime startingDate = Convert.ToDateTime(txtFechaIniSO.Text);
        DateTime endingDate = Convert.ToDateTime(txtFechaFinSO.Text);

        FechaIni = startingDate.ToString("yyyyMMdd");
        FechaFin = endingDate.ToString("yyyyMMdd");

        if (ddlUsuarioSO.SelectedItem.Text == "Todos")
            usuario = string.Empty;
        else
            usuario = ddlUsuarioSO.SelectedItem.Text;

        if (ddlTipo.SelectedItem.Text == "Todos")
            entrytype = string.Empty;
        else
            entrytype = ddlTipo.SelectedItem.Text;

        if (ddlSource.SelectedItem.Text == "Todos")
            source = string.Empty;
        else
            source = ddlSource.SelectedItem.Text;

        datosUsuario = clsComun.fnUsuarioEnSesion();

        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "fnCargarPistasSO" + "|" + "Consulta las pistas de auditoria del Sistema Operativo." + "| Usuario: " + datosUsuario.userName + "| Fecha: " + DateTime.Now);
        fnCargarPistasSO(usuario, txtConsultaSO.Text, txtConsulta2SO.Text,
                         txtConsulta3SO.Text, FechaIni, FechaFin, entrytype, source, ddlInstancia.SelectedItem.Text);
    }

    /// <summary>
    /// Carga la lista de pistas de SO
    /// </summary>
    private void fnCargarPistasSO(string psUsuario, string psAccion,
                                string psAccion2, string psAccion3,
                                string sFechaReg, string sFechaReg2,
                                string sEntryType,string sSource,
                                string sHost)
    {
        try
        {
            DataTable tblentryType = new DataTable();
            DataTable tblMachineName = new DataTable();
            DataTable tblUsuarios = new DataTable();
            DataTable tblDatos = new DataTable();
            DataTable tblSource = new DataTable();


            tblPistaSO = clsComun.fnObtenerPistasSO(psUsuario, psAccion, psAccion2, 
                                                    psAccion3, sFechaReg, sFechaReg2,
                                                    sEntryType,sSource,sHost);


            tblentryType = (DataTable)tblPistaSO.Tables["Table"];
            tblMachineName = (DataTable)tblPistaSO.Tables["Table1"];
            tblUsuarios = (DataTable)tblPistaSO.Tables["Table2"];
            tblSource = (DataTable)tblPistaSO.Tables["Table3"];
            tblDatos= (DataTable)tblPistaSO.Tables["Table4"];

            ddlUsuarioSO.DataSource = tblUsuarios;
            ddlUsuarioSO.DataBind();

            ddlTipo.DataSource = tblentryType;
            ddlTipo.DataBind();

            ddlSource.DataSource = tblSource;
            ddlSource.DataBind();

            //lblInstanciaValSO.Text = tblMachineName.Rows[0]["MachineName"].ToString();

            //lblArchivoVal.Text = tblArchivo.Rows[0]["Archivo"].ToString();

            if (!string.IsNullOrEmpty(sFechaReg) && !string.IsNullOrEmpty(sFechaReg2))
            {
                ViewState["tblPistasSO"] = tblDatos;
                grvDatosSO.DataSource = tblDatos;
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

    //Sorting Grid SO
    protected void grvDatosSO_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridViewSO(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridViewSO(sortExpression, ASCENDING);
        }
    }

    private void SortGridViewSO(string sortExpression, string direction)
    {
        //  You can cache the DataTable for improving performance
        tblPista = (DataTable)ViewState["tblPistasSO"];

        DataView dv = new DataView(tblPista);
        dv.Sort = sortExpression + direction;

        grvDatosSO.DataSource = dv;
        grvDatosSO.DataBind();
    }
}
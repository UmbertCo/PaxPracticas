using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Drawing;


public partial class webGlobalMonitoreo : System.Web.UI.Page
{
    private clsMonitoreo gINS;
    clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {


            //Establecemos el filtro de fechas para el día de hoy
            txtFechaFin_CalendarExtender.SelectedDate = DateTime.Now;
            txtFechaIni_CalendarExtender.SelectedDate = DateTime.Now;

            txtFechaIniMem_CalendarExtender.SelectedDate = DateTime.Now;
            txtFechaFinMem_CalendarExtender.SelectedDate = DateTime.Now;

            txtFechaIniAlm_CalendarExtender.SelectedDate = DateTime.Now;
            txtFechaFinAlm_CalendarExtender.SelectedDate = DateTime.Now;

            txtFechaIniRed_CalendarExtender.SelectedDate = DateTime.Now;
            txtFechaFinRed_CalendarExtender.SelectedDate = DateTime.Now;

            txtFechaIniCon_CalendarExtender.SelectedDate = DateTime.Now;
            txtFechaFinCon_CalendarExtender.SelectedDate = DateTime.Now;

            txtFechaIniUsu_CalendarExtender.SelectedDate = DateTime.Now;
            txtFechaFinUsu_CalendarExtender.SelectedDate = DateTime.Now;

            //Tab Activo
            TabContainer.ActiveTabIndex = 0;
            chkLista.Items[8].Selected = true;
            //Llenar combo usuarios.
            gINS = new clsMonitoreo();
            ddlUsuarios.DataSource =gINS.fnRecuperUsuariosMonitoreo();
            ddlUsuarios.DataBind();
            fnRecuperaExistentes();
        }

        //Registrar botones para eventos
        ScriptManager SM = ScriptManager.GetCurrent(this);
        SM.RegisterPostBackControl(btnGuardar);
        SM.RegisterPostBackControl(btnGuardarMem);
        SM.RegisterPostBackControl(btnGuardaAlm);
        SM.RegisterPostBackControl(btnRed);
        SM.RegisterPostBackControl(btnExcel);
        SM.RegisterPostBackControl(btnConusltar);

        

    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlUbicacion.SelectedValue) & !string.IsNullOrEmpty(ddlDispositivo.SelectedValue)
            & !string.IsNullOrEmpty(ddlOrigen.SelectedValue) & !string.IsNullOrEmpty(txtPromedio.Text)
            & !string.IsNullOrEmpty(txtMaximo.Text) & !string.IsNullOrEmpty(txtCapCpu.Text)
            & !string.IsNullOrEmpty(txtFechaIni.Text) & !string.IsNullOrEmpty(txtFechaFin.Text)
            )
        {

            //Manda llamar la funcion para insertar
            if (fnGuardarRegistroMonitoreo(ddlUbicacion.SelectedValue, ddlDispositivo.SelectedValue, ddlOrigen.SelectedValue,
                                       txtPromedio.Text, txtMaximo.Text, txtCapCpu.Text, Convert.ToDateTime(txtFechaIni.Text),
                                       Convert.ToDateTime(txtFechaFin.Text), fuCpu))
            {
                txtPromedio.Text=string.Empty; 
                txtMaximo.Text=string.Empty;
                txtCapCpu.Text = string.Empty;
                fnRecuperaExistentes();
            }


        }
    }
    protected void btnGuardarMem_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlUbMemoria.SelectedValue) & !string.IsNullOrEmpty(ddlDisMemoria.SelectedValue)
            & !string.IsNullOrEmpty(ddlOrMemoria.SelectedValue) & !string.IsNullOrEmpty(txtPromemoria.Text)
            & !string.IsNullOrEmpty(txtMaxMem.Text) & !string.IsNullOrEmpty(txtCapMem.Text)
            & !string.IsNullOrEmpty(txtFechaIniMem.Text) & !string.IsNullOrEmpty(txtFechaFinMem.Text)
            )
        {

            //Manda llamar la funcion para insertar
            if (fnGuardarRegistroMonitoreo(ddlUbMemoria.SelectedValue, ddlDisMemoria.SelectedValue, ddlOrMemoria.SelectedValue,
                                        txtPromemoria.Text, txtMaxMem.Text, txtCapMem.Text, Convert.ToDateTime(txtFechaIniMem.Text),
                                        Convert.ToDateTime(txtFechaFinMem.Text), fuCpuMem))
            {
                txtPromemoria.Text = string.Empty;
                txtMaxMem.Text = string.Empty;
                txtCapMem.Text = string.Empty;
                fnRecuperaExistentes();
            }

        }
    }
    protected void btnGuardaAlm_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlUbicacionAlmacen.SelectedValue) & !string.IsNullOrEmpty(ddlDispositivoAlm.SelectedValue)
            & !string.IsNullOrEmpty(ddlOrigenAlm.SelectedValue) & !string.IsNullOrEmpty(txtPromedioAlm.Text)
            & !string.IsNullOrEmpty(txtMaximoAlm.Text) & !string.IsNullOrEmpty(txtCapAlm.Text)
            & !string.IsNullOrEmpty(txtFechaIniAlm.Text) & !string.IsNullOrEmpty(txtFechaFinAlm.Text)
            )
        {
            
            //Manda llamar la funcion para insertar
            if (fnGuardarRegistroMonitoreo(ddlUbicacionAlmacen.SelectedValue, ddlDispositivoAlm.SelectedValue, ddlOrigenAlm.SelectedValue,
                                        txtPromedioAlm.Text, txtMaximoAlm.Text, txtCapAlm.Text, Convert.ToDateTime(txtFechaIniAlm.Text),
                                        Convert.ToDateTime(txtFechaFinAlm.Text), fuAlm))
            {
                txtPromedioAlm.Text = string.Empty;
                txtMaximoAlm.Text = string.Empty;
                txtCapAlm.Text = string.Empty;
                fnRecuperaExistentes();
            }

        }
    }
    protected void btnRed_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlUbicacionRed.SelectedValue) & !string.IsNullOrEmpty(ddlDispositivoRed.SelectedValue)
            & !string.IsNullOrEmpty(ddlOrigenRed.SelectedValue) & !string.IsNullOrEmpty(txtPromedioRed.Text)
            & !string.IsNullOrEmpty(txtMaximoRed.Text) & !string.IsNullOrEmpty(txtRedCap.Text)
            & !string.IsNullOrEmpty(txtFechaIniRed.Text) & !string.IsNullOrEmpty(txtFechaFinRed.Text)
            )
        {

            //Manda llamar la funcion para insertar
            if (fnGuardarRegistroMonitoreo(ddlUbicacionRed.SelectedValue, ddlDispositivoRed.SelectedValue, ddlOrigenRed.SelectedValue,
                                        txtPromedioRed.Text, txtMaximoRed.Text, txtRedCap.Text, Convert.ToDateTime(txtFechaIniRed.Text),
                                        Convert.ToDateTime(txtFechaFinRed.Text), fuAlm))
            {
                txtPromedioRed.Text = string.Empty;
                txtMaximoRed.Text = string.Empty;
                txtRedCap.Text = string.Empty;
                fnRecuperaExistentes();
            }

        }
    }
    protected void btnConusltar_Click(object sender, EventArgs e)
    {
        DataTable tabla = new DataTable();
        string dispositivo = string.Empty;
        string ubicacion = ddlUbicacionCon.SelectedValue;
        string usuario = ddlUsuarios.SelectedItem.Value;

        //Revisa dispositivos seleccionados
        foreach (ListItem items in chkLista.Items)
        {
            if (items.Selected == true)
            {
                dispositivo = dispositivo + "," + items.Text;
            }
        }
        
        if(dispositivo.Length >0)
            dispositivo = dispositivo.Substring(1); 

        //Dar formato a fecha
        string FechaIni = string.Empty;
        string FechaFin = string.Empty;

        DateTime startingDate = Convert.ToDateTime(txtFechaIniCon.Text);
        DateTime endingDate = Convert.ToDateTime(txtFechaFinCon.Text);

        FechaIni = startingDate.ToString("yyyyMMdd");
        FechaFin = endingDate.ToString("yyyyMMdd");

        ViewState["dispositivo"] = dispositivo;
        if (dispositivo == "TODOS")
        {
            dispositivo=string.Empty;
        }

        if (usuario == "-1" && ddlUsuarios.SelectedItem.Text == "TODOS")
        {
            usuario = string.Empty;
        }

        //Recupera consulta
        gINS = new clsMonitoreo();
        tabla = gINS.fnConsultaMonitoreo(ubicacion,dispositivo,FechaIni,FechaFin,usuario);
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "Monitoreo Consulta." + "|" + ubicacion + "|" + dispositivo + "|" + FechaIni+"|"+FechaFin+"|"+usuario);


        gdvRegistros.DataSource = tabla;
        ViewState["tablaConsulta"] = tabla;
        gdvRegistros.DataBind();

        if (tabla.Rows.Count > 0)
        {
            btnExcel.Enabled = true;
        }
        else
        {
            btnExcel.Enabled = false;
        }

        ViewState["ubicacion"] = ubicacion;
        ViewState["usuario"] = usuario;
        ViewState["FechaIni"] = FechaIni;
        ViewState["FechaFin"] = FechaFin;
 
    }

    /// <summary>
    /// Funcion encargada de registrar monitoreos
    /// </summary>
    /// <param name="sddlUbicacion"></param>
    /// <param name="sddlDispositivo"></param>
    /// <param name="sddlOrigen"></param>
    /// <param name="stxtPromedio"></param>
    /// <param name="stxtMaximo"></param>
    /// <param name="stxtCapacidad"></param>
    /// <param name="stxtFechaIni"></param>
    /// <param name="stxtFechaFin"></param>
    /// <param name="adjuntar"></param>
    /// <returns></returns>
    private bool fnGuardarRegistroMonitoreo(string sddlUbicacion,string sddlDispositivo,string sddlOrigen,
                                            string stxtPromedio, string stxtMaximo,string stxtCapacidad,
                                            DateTime stxtFechaIni, DateTime stxtFechaFin, FileUpload adjuntar)
    {

        gINS = new clsMonitoreo();
        bool Archivo = false;
        int archivoEnBytes = 0;
        bool psarchivo = false;
        byte[] pArchivo;
        bool retorno = false;

        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();

            if (adjuntar.FileName.ToString() != "")
            {
                //Verifica la extencion del archivo
                Archivo = gINS.fnverificaarchivo(adjuntar.FileName);
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "verificar la extencion del archivo." + "|" + sddlUbicacion + "|" + adjuntar.FileName);
                if (Archivo == true)
                {
                    pArchivo = adjuntar.FileBytes;
                    archivoEnBytes = (Convert.ToInt32(pArchivo.Length) / 1024);

                    //Verifica el tamaño del archivo
                    psarchivo = gINS.fnVerificaTamanioMax(archivoEnBytes);
                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "verificar el tamaño del archivo." + "|" + sddlUbicacion + "|" + archivoEnBytes);
                    if (psarchivo == true)
                    {
                        //Inserta el registro
                        gINS.fnGuardarMonitoreo(sddlUbicacion, sddlDispositivo, sddlOrigen, stxtPromedio, stxtMaximo,
                                    stxtCapacidad, adjuntar.FileBytes, stxtFechaIni, stxtFechaFin,
                                    datosUsuario.id_usuario, DateTime.Now, 'A');

                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "Monitoreo Registrado." + "|" + sddlUbicacion);

                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                        retorno = true;
                    }
                    else
                    {
                        retorno = false;
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorTamanio);
                    }

                }
                else
                {
                    retorno = false;
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorArchivo + " " + clsComun.ObtenerParamentro("Extensiones"));
                }
            }
            else
            {
                retorno = true;
                //Inserta el registro
                gINS.fnGuardarMonitoreo(sddlUbicacion, sddlDispositivo, sddlOrigen, stxtPromedio, stxtMaximo,
                                        stxtCapacidad, adjuntar.FileBytes, stxtFechaIni, stxtFechaFin,
                                        datosUsuario.id_usuario, DateTime.Now, 'A');

                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "Monitoreo Registrado." + "|" + sddlUbicacion);


                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
            }
        }
        catch (Exception ex)
        {
            retorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
        }


        return retorno;
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
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            DataTable tableConsulta = new DataTable();
            
            //Configurar GRID
            GridView grid = new GridView();
            string[] key = { "id_Monitoreo" };
            
            grid.ID="gdvRegistros";
            grid.AutoGenerateColumns =false;
            grid.BackColor = System.Drawing.Color.White;
            grid.BorderColor = System.Drawing.ColorTranslator.FromHtml("#336666");
            grid.BorderStyle = BorderStyle.Double;
            grid.BorderWidth = Unit.Pixel(3);
            grid.CellPadding = 4;
            grid.DataKeyNames = key;
            grid.GridLines = GridLines.Horizontal;
            grid.Width = Unit.Pixel(900);

            //Llenar columnas grid
            foreach (DataRow item in fnCrearTabla().Rows)
            {
                BoundField field = new BoundField();
                field.DataField = item["DataField"].ToString();
                field.HeaderText = item["HeaderText"].ToString();
                grid.Columns.Add(field);
            }
            grid.FooterStyle.BackColor =System.Drawing.Color.White;
            grid.FooterStyle.ForeColor =System.Drawing.ColorTranslator.FromHtml("#336666");
            grid.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#336666");
            grid.HeaderStyle.ForeColor = System.Drawing.Color.White;
            grid.HeaderStyle.Font.Bold = true;
            grid.HeaderStyle.Width = Unit.Pixel(10);

            //LLenar datos Grid
            tableConsulta = (DataTable)ViewState["tablaConsulta"];
            grid.DataSource = tableConsulta;
            grid.DataBind();



            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gdvRegistros.EnableViewState = false;

            page.EnableEventValidation = false;
            page.DesignerInitialize();
            
            //Crear Encabezado del reporte
            Table TableToExport = new Table();
            TableRow Row = new TableRow();
            TableCell Cell = new TableCell();
            TableRow Row1 = new TableRow();
            TableCell Cell1 = new TableCell();
            TableRow Row2 = new TableRow();
            TableCell Cell2 = new TableCell();
            TableRow Row3 = new TableRow();
            TableCell Cell3 = new TableCell();

            Cell.Text = Resources.resCorpusCFDIEs.varUbicacion + ": " + ViewState["ubicacion"];
            Row.Cells.Add(Cell);
            TableToExport.Rows.Add(Row);

            Cell1.Text = Resources.resCorpusCFDIEs.varDispositivo + ": " + ViewState["dispositivo"];
            Row1.Cells.Add(Cell1);
            TableToExport.Rows.Add(Row1);

            Cell2.Text = Resources.resCorpusCFDIEs.lblPistasUsuario + ": " + ViewState["usuario"];
            Row2.Cells.Add(Cell2);
            TableToExport.Rows.Add(Row2);

            Cell3.Text = Resources.resCorpusCFDIEs.lblFechaIni + ": " + ViewState["FechaIni"] +" "+ Resources.resCorpusCFDIEs.lblFechaFin + ": " + ViewState["FechaFin"];
            Row3.Cells.Add(Cell3);
            TableToExport.Rows.Add(Row3);


            page.Controls.Add(TableToExport);
            page.Controls.Add(form);

            form.Controls.Add(grid);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment; filename=consulta_monitoreo" + DateTime.Today + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void chkLista_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Eventos para el checkbox de dispositivos
        if (chkLista.Items[8].Selected)
        {
            chkLista.Items[0].Selected = false;
            chkLista.Items[1].Selected = false;
            chkLista.Items[2].Selected = false;
            chkLista.Items[3].Selected = false;
            chkLista.Items[4].Selected = false;
            chkLista.Items[5].Selected = false;
            chkLista.Items[6].Selected = false;
            chkLista.Items[7].Selected = false;
            chkLista.Items[8].Selected = true;

        }
        else
        {
            if (chkLista.Items[0].Selected)
            {
                chkLista.Items[8].Selected = false;
            }
            if (chkLista.Items[1].Selected)
            {
                chkLista.Items[8].Selected = false;
            }
            if (chkLista.Items[2].Selected)
            {
                chkLista.Items[8].Selected = false;
            }
            if (chkLista.Items[3].Selected)
            {
                chkLista.Items[8].Selected = false;
            }
            if (chkLista.Items[4].Selected)
            {
                chkLista.Items[8].Selected = false;
            }
            if (chkLista.Items[5].Selected)
            {
                chkLista.Items[8].Selected = false;
            }
            if (chkLista.Items[6].Selected)
            {
                chkLista.Items[8].Selected = false;
            }
            if (chkLista.Items[7].Selected)
            {
                chkLista.Items[8].Selected = false;
            }
        }

    }

    /// <summary>
    /// encargada de redireccionar on error
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx");
        }

    }

    /// <summary>
    /// Funcion encargada de generar la infromacion de las columnas
    /// </summary>
    /// <returns></returns>
    private DataTable fnCrearTabla()
    {
        DataTable tabla= new DataTable();
        tabla.Columns.Add("DataField");
        tabla.Columns.Add("HeaderText");

        DataRow registro = tabla.NewRow();
        registro["DataField"] = "ubicacion";
        registro["HeaderText"] = Resources.resCorpusCFDIEs.varUbicacion;
        tabla.Rows.Add(registro);

        DataRow registro2 = tabla.NewRow();
        registro2["DataField"] = "tipo_dispositivo";
        registro2["HeaderText"] = Resources.resCorpusCFDIEs.varDispositivo;
        tabla.Rows.Add(registro2);

        DataRow registro3 = tabla.NewRow();
        registro3["DataField"] = "origen";
        registro3["HeaderText"] = Resources.resCorpusCFDIEs.varOrigen;
        tabla.Rows.Add(registro3);

        DataRow registro4 = tabla.NewRow();
        registro4["DataField"] = "fecha_ini";
        registro4["HeaderText"] = Resources.resCorpusCFDIEs.lblFechaIni;
        tabla.Rows.Add(registro4);

        DataRow registro5 = tabla.NewRow();
        registro5["DataField"] = "fecha_fin";
        registro5["HeaderText"] = Resources.resCorpusCFDIEs.lblFechaFin;
        tabla.Rows.Add(registro5);

        DataRow registro6 = tabla.NewRow();
        registro6["DataField"] = "promedio";
        registro6["HeaderText"] = Resources.resCorpusCFDIEs.varPromedio;
        tabla.Rows.Add(registro6);

        DataRow registro7 = tabla.NewRow();
        registro7["DataField"] = "maximo";
        registro7["HeaderText"] = Resources.resCorpusCFDIEs.varMaximo;
        tabla.Rows.Add(registro7);

        DataRow registro8 = tabla.NewRow();
        registro8["DataField"] = "capacidad";
        registro8["HeaderText"] = Resources.resCorpusCFDIEs.varCapacidad;
        tabla.Rows.Add(registro8);

        DataRow registro10 = tabla.NewRow();
        registro10["DataField"] = "clave_usuario";
        registro10["HeaderText"] = Resources.resCorpusCFDIEs.lblPistasUsuario;
        tabla.Rows.Add(registro10);

        DataRow registro11 = tabla.NewRow();
        registro11["DataField"] = "fecha_captura";
        registro11["HeaderText"] = Resources.resCorpusCFDIEs.lblFecha;
        tabla.Rows.Add(registro11);

        return tabla; 
    }

    private void fnRecuperaExistentes()
    {
        clsMonitoreo monitoreo = new clsMonitoreo();
        DataSet tblExistentes = new DataSet("tblExistentes");

        int nTActive = TabContainer.ActiveTabIndex;
        string sUbicacion = string.Empty;
        string sDispositivo = string.Empty;

        switch (nTActive)
        {
            case 0:
                sUbicacion = ddlUbicacion.SelectedValue;
                sDispositivo = ddlDispositivo.SelectedValue;
                break;

            case 1:
                sUbicacion = ddlUbMemoria.SelectedValue;
                sDispositivo = ddlDisMemoria.SelectedValue;
                break;

            case 2:
                sUbicacion = ddlUbicacionAlmacen.SelectedValue;
                sDispositivo = ddlDispositivoAlm.SelectedValue;
                break;

            case 3:
                sUbicacion = ddlUbicacionRed.SelectedValue;
                sDispositivo = ddlDispositivoRed.SelectedValue;
                break;
        }


        tblExistentes = monitoreo.fnRecuperaExistentes(sUbicacion, sDispositivo);

        if (tblExistentes.Tables[0].Rows.Count > 0)
        {
            if (tblExistentes.Tables[0].Rows[0]["CPU"].ToString() == "1")
            {
                btnGuardar.Enabled = false;
                lblMsgCPU.Text = Resources.resCorpusCFDIEs.varMonReg;
                lblMsgCPU.Visible = true;
            }
            else
            {
                btnGuardar.Enabled = true;
                lblMsgCPU.Text = "";
                lblMsgCPU.Visible = false;
            }
        }

        if (tblExistentes.Tables[1].Rows.Count > 0)
        {
            if (tblExistentes.Tables[1].Rows[0]["MEMORIA"].ToString() == "1")
            {
                btnGuardarMem.Enabled = false;
                lblMsgMemoria.Text = Resources.resCorpusCFDIEs.varMonReg;
                lblMsgMemoria.Visible = true;
            }
            else
            {
                btnGuardarMem.Enabled = true;
                lblMsgMemoria.Text = "";
                lblMsgMemoria.Visible = false;
            }
        }

        if (tblExistentes.Tables[2].Rows.Count > 0 || tblExistentes.Tables[3].Rows.Count > 0)
        {
            if (tblExistentes.Tables[2].Rows[0]["Red Publica"].ToString() == "1" || tblExistentes.Tables[3].Rows[0]["Red Privada"].ToString() == "1")
            {
                btnRed.Enabled = false;
                lblMsgRed.Text = Resources.resCorpusCFDIEs.varMonReg;
                lblMsgRed.Visible = true;
                
            }
            else
            {
                btnRed.Enabled = true;
                lblMsgRed.Text = "";
                lblMsgRed.Visible = false;
            }
        }

        if (tblExistentes.Tables[4].Rows.Count > 0)
        {
            if (tblExistentes.Tables[4].Rows[0]["C"].ToString() == "1"
                || tblExistentes.Tables[5].Rows[0]["D"].ToString() == "1"
                || tblExistentes.Tables[6].Rows[0]["SAN"].ToString() == "1"
                || tblExistentes.Tables[7].Rows[0]["NAS"].ToString() == "1")
            {
                btnGuardaAlm.Enabled = false;
                lblMsgAlm.Text = Resources.resCorpusCFDIEs.varMonReg;
                lblMsgAlm.Visible = true;
            }
            else
            {
                btnGuardaAlm.Enabled = true;
                lblMsgAlm.Text = "";
                lblMsgAlm.Visible = false;
            }
        }

    }
    protected void ddlUbicacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnRecuperaExistentes();
    }
    protected void ddlUbMemoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnRecuperaExistentes();
    }
    protected void ddlUbicacionAlmacen_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnRecuperaExistentes();
    }
    protected void ddlUbicacionRed_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnRecuperaExistentes();
    }
    protected void ddlDispositivoRed_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnRecuperaExistentes();
    }
    protected void ddlDispositivoAlm_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnRecuperaExistentes();
    }
    protected void TabContainer_ActiveTabChanged(object sender, EventArgs e)
    {
        fnRecuperaExistentes();
    }
    protected void btnMonitoreo_Click(object sender, EventArgs e)
    {
        DataSet tblAlarmas = new DataSet();
        DataTable tblMostrarAlarmas = new DataTable();
        gINS = new clsMonitoreo();
        string strMensaje = string.Empty;

        tblMostrarAlarmas.Columns.Add("Ubicacion");
        tblMostrarAlarmas.Columns.Add("Alarma"); 

        tblAlarmas=gINS.fnRecuperaAlarmas();

        tblMostrarAlarmas=fnRecuperaAlarmas(tblAlarmas);

        if (tblMostrarAlarmas.Rows.Count > 0)
        {
            lblDetalle.Text = Resources.resCorpusCFDIEs.varSiAlarmas;
            lblDetalle.ForeColor = Color.Red;
             
            strMensaje = "<table>";
            strMensaje = strMensaje + "<tr><td><b>Se ha disparado la alarma al pasar el " + clsComun.ObtenerParamentro("capacidadPOR") +"% de configuración por dispositivo.</b></td></tr>";
            strMensaje = strMensaje + "<tr><td><b>Se presenta la ubicación y el dispositivo, para mayor información, consulte el reporte.</b></td></tr>";
            strMensaje = strMensaje + "<tr><td></td></tr>";
            strMensaje = strMensaje + "<tr><td></td></tr>";
            strMensaje = strMensaje + "<tr><td></td></tr>";

            foreach (DataRow item in tblMostrarAlarmas.Rows)
            {
                strMensaje = strMensaje + "<tr><td>"+item["Ubicacion"]+"</td></tr>";
            }

            strMensaje = strMensaje + "</table>";

            varDetAlarmas.InnerHtml = strMensaje;

            modalAlarmas.Show();
        }
        else
        {
            lblDetalle.Text = Resources.resCorpusCFDIEs.varNoAlarmas;
            varDetAlarmas.InnerHtml = string.Empty;
            lblDetalle.ForeColor = Color.Black;
                        
            modalAlarmas.Show();
        }
    }

    /// <summary>
    /// Recuepra la lista de alarmas
    /// </summary>
    /// <param name="tblAlarmas"></param>
    /// <returns></returns>
    private DataTable fnRecuperaAlarmas(DataSet tblAlarmas)
    {
        DataTable tblMostrarAlarmas = new DataTable();

        tblMostrarAlarmas.Columns.Add("Ubicacion");
        tblMostrarAlarmas.Columns.Add("Alarma"); 

        if (tblAlarmas.Tables[0].Rows.Count > 0)
        {
            //CPU
            if (tblAlarmas.Tables[0].Rows[0]["CPU_Apolo"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("CPU_Apolo", "1");
            }
            if (tblAlarmas.Tables[0].Rows[0]["CPU_Discovery"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("CPU_Discovery", "1");
            }
            //MEMORIA
            if (tblAlarmas.Tables[1].Rows[0]["MEM_Apolo"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("MEM_Apolo", "1");
            }
            if (tblAlarmas.Tables[1].Rows[0]["MEM_Discovery"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("MEM_Discovery", "1");
            }
            //RED
            if (tblAlarmas.Tables[2].Rows[0]["RED_Publica_Apolo"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("RED_Publica_Apolo", "1");
            }
            if (tblAlarmas.Tables[2].Rows[0]["RED_Publica_Discovery"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("RED_Publica_Discovery", "1");
            }
            if (tblAlarmas.Tables[2].Rows[0]["RED_Privada_Apolo"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("RED_Privada_Apolo", "1");
            }
            if (tblAlarmas.Tables[2].Rows[0]["RED_Privada_Discovery"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("RED_Privada_Discovery", "1");
            }
            //ALMACENAMIENTO
            if (tblAlarmas.Tables[3].Rows[0]["C_Apolo"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("C_Apolo", "1");
            }
            if (tblAlarmas.Tables[3].Rows[0]["C_Discovery"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("C_Discovery", "1");
            }
            if (tblAlarmas.Tables[3].Rows[0]["D_Apolo"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("D_Apolo", "1");
            }
            if (tblAlarmas.Tables[3].Rows[0]["D_Discovery"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("D_Discovery", "1");
            }
            if (tblAlarmas.Tables[3].Rows[0]["SAN_Apolo"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("SAN_Apolo", "1");
            }
            if (tblAlarmas.Tables[3].Rows[0]["SAN_Discovery"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("SAN_Discovery", "1");
            }
            if (tblAlarmas.Tables[3].Rows[0]["NAS_Apolo"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("NAS_Apolo", "1");
            }
            if (tblAlarmas.Tables[3].Rows[0]["NAS_Discovery"].ToString() == "1")
            {
                tblMostrarAlarmas.Rows.Add("NAS_Discovery", "1");
            }
        }

        return tblMostrarAlarmas;
    }
    protected void btnAcepAlarms_Click(object sender, EventArgs e)
    {

    }
}
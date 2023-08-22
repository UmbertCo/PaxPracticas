using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utilerias.SQL;
using System.Text;
using System.Threading;
using System.Globalization;

public partial class Operacion_Comprobantes_webConsultaComprobantes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["esProveedor"] = false;
                clsInicioSesionUsuario sesUsuario = clsComun.fnUsuarioEnSesion();
                clsOperacionUsuarios oOpUsuarios = new clsOperacionUsuarios();
                //SqlDataReader sdrInfo = gOp.fnObtenerDatosUsuario();
                int nId_Usuario = sesUsuario.id_usuario;
                if (nId_Usuario > 0)
                {
                    DataTable tblModulosPerfil = oOpUsuarios.fnSeleccionaModulosHijo(sesUsuario.Id_perfil, true);
                    string[] urlActual = Request.Url.AbsolutePath.Split('/');
                    int encontrado = tblModulosPerfil.AsEnumerable().Where(t => t.Field<string>("modulo").Contains(urlActual[urlActual.Length - 1])).Count();
                    if (encontrado < 1)
                        Response.Redirect("~/Default.aspx", false);

                    if (new clsOperacionUsuarios().fnVerificarUsuarioPerfil(sesUsuario.id_usuario, "P"))
                    {
                        ViewState["esProveedor"] = true;
                    }
                }
                //fnLlenaEmisores(dsFiltros.Tables[0]);
                //fnLlenaReceptores(dsFiltros.Tables[1]);
                fnLlenaVersiones();
                //fnLlenaUsuarios(dsFiltros.Tables[3]);
                fnLlenaFechas();
                fnLlenarSucursales();
                ViewState["dtStatus"] = new clsOperacionComprobantes().fnObtenerStatus();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            Response.Redirect("~/webGlobalError.aspx",false);
        }
    }
    private void fnLlenarSucursales()
    {
        DataTable dtSucursales = new DataTable();
        clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
        if (usuarioActivo.Id_perfil == clsComun.fnObtenerIdPerfil("proveedor"))
        {
            dtSucursales = new clsOperacionProveedores().fnObtenerSucursalesProveedorUsuario(usuarioActivo.id_usuario);

        }
        else
        {
            dtSucursales = new clsOperacionSucursales().fnObtenerSucursalesUsuario(usuarioActivo.id_usuario);
        }
        gvSucursales.DataSource = dtSucursales;
        gvSucursales.DataBind();
        gvSucursales.Visible = true;
    }
    private void fnLlenaFechas()
    {
        txtFechaInicio.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
        txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }



    //private void fnLlenaUsuarios(DataTable dtUsuarios)
    //{
    //    ddlUsuario.DataSource = dtUsuarios;
    //    ddlUsuario.DataValueField = "id_usuario";
    //    ddlUsuario.DataTextField = "clave_usuario";
    //    ddlUsuario.DataBind();
    //    ddlUsuario.Items.Add("--");
    //    ddlUsuario.SelectedIndex = ddlUsuario.Items.Count - 1;
    //}

    private void fnLlenaVersiones()
    {

        string versiones = clsComun.ObtenerParamentro("versiones");

        string[] values = versiones.Split(',');

        foreach (string version in values)
        {
            ddlVersion.Items.Add(new ListItem(version));
        }

        //ddlVersion.Items.Add(new ListItem("3.2"));
        //ddlVersion.Items.Add(new ListItem("2.2"));
        ddlVersion.SelectedIndex = 0;
    }

    //private void fnLlenaEmisores(DataTable dtEmisores)
    //{
    //    ddlEmisor.DataSource = dtEmisores;
    //    ddlEmisor.DataValueField = "rfc_emisor";
    //    ddlEmisor.DataTextField = "nombre_emisor";
    //    ddlEmisor.DataBind();
    //    ddlEmisor.Items.Add("--");
    //    ddlEmisor.SelectedIndex = ddlEmisor.Items.Count - 1;
    //}

    //private void fnLlenaReceptores(DataTable dtReceptores)
    //{
    //    ddlReceptor.DataSource = dtReceptores;
    //    ddlReceptor.DataValueField = "rfc_receptor";
    //    ddlReceptor.DataTextField = "nombre_receptor";
    //    ddlReceptor.DataBind();
    //    ddlReceptor.Items.Add("--");
    //    ddlReceptor.SelectedIndex = ddlReceptor.Items.Count - 1;
    //}

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        try
        {
            btnAnterior.Visible = false;
            btnAnterior.Visible = false;
            fnBuscarComprobantes(1);
            ViewState["paginado"] = 1;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    private void fnBuscarComprobantes(int nPagina)
    {
        //Verifica que la fecha inicial sea menor que la final
        cbSeleccionar.Checked = false;
        DataTable dtResultado = new DataTable("dtResultadoComprobantes");
        if (DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) >
            DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varFechaMayor);
        else
        {
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            int nTipo = 0;
            string sRfcEmisor = string.Empty;
            string sRfcReceptor = string.Empty;
            string sVersion = string.Empty;
            string sLista = null;
            sRfcEmisor = txtRfcEmisor.Text;
            sRfcReceptor = txtRfcReceptor.Text;

            if (!ddlVersion.SelectedValue.Equals("Todas"))
            {
                //sVersion = ddlVersion.SelectedValue;
                sVersion = ddlVersion.SelectedValue.Substring(0, 3);
            }
            else
            {
                sVersion = null;
            }

            //if (!ddlVersion.SelectedValue.Equals("--"))
            //    sVersion = ddlVersion.SelectedValue;

            foreach (GridViewRow renglon in gvSucursales.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                if (CbCan.Checked)
                {
                    Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                    if (sLista != null)
                    {
                        sLista = sLista + "," + sIdSucursal.Text;
                    }
                    else
                    {
                        sLista = sIdSucursal.Text;
                    }
                }
            }


            nTipo = Convert.ToInt32(rbTipoBusqueda.SelectedValue);
            //realiza la busqueda
            int nLinPag = Convert.ToInt32(ddlLineasPagina.SelectedItem.ToString());
            if (Convert.ToBoolean(ViewState["esProveedor"]))
            {
                dtResultado = new clsOperacionComprobantes().fnBuscarComprobantesProveedorPag(DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), sRfcEmisor, sRfcReceptor, sVersion, usuarioActivo.id_usuario, nTipo, sLista, nPagina, nLinPag);
                //gvResultado.AutoGenerateEditButton = false;
                fnCambiarStatusCultura(dtResultado);
                gvResultado.AutoGenerateColumns = false;
                gvResultado.DataSource = dtResultado;
                gvResultado.DataBind();
                gvResultado.Columns[0].Visible = false;
                //TemplateField tfEdit = (TemplateField)gvResultado.Columns[0];
                //tfEdit.Visible = false;
                
            }
            else
            {
                dtResultado = new clsOperacionComprobantes().fnBuscarComprobantesPag(DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), sRfcEmisor, sRfcReceptor, sVersion, usuarioActivo.id_usuario, nTipo, sLista, nPagina, nLinPag);
                fnCambiarStatusCultura(dtResultado);
                gvResultado.AutoGenerateColumns = false;
                gvResultado.DataSource = dtResultado;
                gvResultado.DataBind();
                gvResultado.Columns[0].Visible = true;
                
            }
            

            if (gvResultado.Rows.Count > 0)
            {
                btnDescargar.Visible = true;
                btnExportar.Visible = true;
                cbSeleccionar.Visible = true;
            }
            else
            {
                btnDescargar.Visible = false;
                cbSeleccionar.Visible = false;
                btnExportar.Visible = false;
            }


            if (gvResultado.Rows.Count < nLinPag)
                btnSiguiente.Visible = false;
            else
                btnSiguiente.Visible = true;
            // fnSeleccionarStatus();

        }
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        Session["dtConsultaExc"] = null;
        DataTable dtConsulta = fnRealizarConsultaAsincrona();
        Session["dtConsultaExc"] = dtConsulta;
        Session["bProveedor"] = ViewState["esProveedor"];
        ScriptManager.RegisterStartupScript(this, this.GetType(), "newWindow",
                                                        String.Format("<script>window.open('webDescargaConsulta.aspx','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>", "webDescargaConsulta.aspx"), false);
        //ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('webDescargaConsulta.aspx','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>", "webDescargaConsulta.aspx"));
    }

    public DataTable fnRealizarConsultaAsincrona()
    {
        try
        {
            DataTable dtNew = new DataTable();
            DataColumn columna1 = new DataColumn();
            columna1.DataType = System.Type.GetType("System.DateTime");
            columna1.AllowDBNull = true;
            columna1.Caption = "dDesde";
            columna1.ColumnName = "dDesde";
            columna1.DefaultValue = null;
            dtNew.Columns.Add(columna1);

            DataColumn columna2 = new DataColumn();
            columna2.DataType = System.Type.GetType("System.DateTime");
            columna2.AllowDBNull = true;
            columna2.Caption = "dHasta";
            columna2.ColumnName = "dHasta";
            columna2.DefaultValue = null;
            dtNew.Columns.Add(columna2);


            DataColumn columna3 = new DataColumn();
            columna3.DataType = System.Type.GetType("System.String");
            columna3.AllowDBNull = true;
            columna3.Caption = "sRfcEmisor";
            columna3.ColumnName = "sRfcEmisor";
            columna3.DefaultValue = null;
            dtNew.Columns.Add(columna3);


            DataColumn columna4 = new DataColumn();
            columna4.DataType = System.Type.GetType("System.String");
            columna4.AllowDBNull = true;
            columna4.Caption = "sRfcReceptor";
            columna4.ColumnName = "sRfcReceptor";
            columna4.DefaultValue = 0;
            dtNew.Columns.Add(columna4);


            DataColumn columna5 = new DataColumn();
            columna5.DataType = System.Type.GetType("System.String");
            columna5.AllowDBNull = true;
            columna5.Caption = "sVersion";
            columna5.ColumnName = "sVersion";
            columna5.DefaultValue = "0";
            dtNew.Columns.Add(columna5);

            DataColumn columna6 = new DataColumn();
            columna6.DataType = System.Type.GetType("System.Int32");
            columna6.AllowDBNull = true;
            columna6.Caption = "nUsuario";
            columna6.ColumnName = "nUsuario";
            columna6.DefaultValue = "0";
            dtNew.Columns.Add(columna6);

            DataColumn columna7 = new DataColumn();
            columna7.DataType = System.Type.GetType("System.Int32");
            columna7.AllowDBNull = true;
            columna7.Caption = "nTipo";
            columna7.ColumnName = "nTipo";
            columna7.DefaultValue = "0";
            dtNew.Columns.Add(columna7);

            DataColumn columna8 = new DataColumn();
            columna8.DataType = System.Type.GetType("System.String");
            columna8.AllowDBNull = true;
            columna8.Caption = "sLista";
            columna8.ColumnName = "sLista";
            columna8.DefaultValue = "0";
            dtNew.Columns.Add(columna8);

           

            DataRow RenNuevo = dtNew.NewRow();

            string sVersion = string.Empty;
            if (!ddlVersion.SelectedValue.Equals("Todas"))
            {
                //sVersion = ddlVersion.SelectedValue;
                sVersion = ddlVersion.SelectedValue.Substring(0, 3);
            }
            else
            {
                sVersion = null;
            }

            //if (!ddlVersion.SelectedValue.Equals("--"))
            //    sVersion = ddlVersion.SelectedValue;
            string sLista = string.Empty;
            foreach (GridViewRow renglon in gvSucursales.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                if (CbCan.Checked)
                {
                    Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                    if (sLista != null)
                    {
                        sLista = sLista + "," + sIdSucursal.Text;
                    }
                    else
                    {
                        sLista = sIdSucursal.Text;
                    }
                }
            }


            int nTipo = Convert.ToInt32(rbTipoBusqueda.SelectedValue);

            RenNuevo["nUsuario"] = clsComun.fnUsuarioEnSesion().id_usuario;

            RenNuevo["dDesde"] = DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            RenNuevo["dHasta"] = DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            RenNuevo["sRfcEmisor"] = txtRfcEmisor.Text;
            RenNuevo["sRfcReceptor"] = txtRfcReceptor.Text;
            RenNuevo["sVersion"] = sVersion;
            RenNuevo["nTipo"] = nTipo;
            RenNuevo["sLista"] = sLista;
            

            dtNew.Rows.Add(RenNuevo);

            return dtNew;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return null;
        }
    }
    protected void ddlEmisor_SelectedIndexChanged(object sender, EventArgs e)
    {
        //lMensajeError.Text = "Evento IndexChange";
    }

    protected void gvResultado_PageIndexChanged(object sender, EventArgs e)
    {
    }

    protected void gvResultado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //fnBuscarComprobantes();
        //gvResultado.PageIndex = e.NewPageIndex;
        //gvResultado.DataBind();
    }

    //protected void ddlStatus_Load(object sender, EventArgs e)
    //{
    //    DropDownList ddlStatus = (DropDownList)sender;
    //    ddlStatus.DataSource = (DataTable) ViewState["dtStatus"];
    //    ddlStatus.DataTextField = "nombre";
    //    ddlStatus.DataValueField = "id_status";
    //    ddlStatus.DataBind();

    //}

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

        DropDownList ddlStatus = (DropDownList)sender;
        GridViewRow gvrRes = ((GridViewRow)ddlStatus.NamingContainer);
        TextBox txtFechaPago = (TextBox)gvrRes.FindControl("txtFechaPago");
        Image img = (Image)gvrRes.FindControl("imgIni");
        int nIdStatus = Convert.ToInt32(ddlStatus.SelectedValue);
        //Si el estatus es 'Pagado', activa el selector de fecha de pago
        if (nIdStatus == 5 && ddlStatus.Enabled)
        {
            txtFechaPago.Enabled = true;
            txtFechaPago.Text = DateTime.Now.ToString("dd/MM/yyyy");
            img.Visible = true;
        }
        else
        {
            txtFechaPago.Enabled = false;
            txtFechaPago.Text = "";
            img.Visible = false;
        }
        //int nIdComprobante =Convert.ToInt32(((DataRowView)gvrRes.DataItem)["id_estructura"]);// Convert.ToInt32(gvrRes["id_estructura"]);
        //new clsOperacionComprobantes().fnCambiarStatus(nIdComprobante, nIdStatus);

    }

    //protected void gvResultado_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    DropDownList ddlStatus =e.Row.FindControl("ddlStatus") as DropDownList;
    //    ddlStatus.SelectedValue = DataBinder.Eval(e.Row.DataItem, "status").ToString();//((DataTable)gvResultado.DataSource).Rows[e.Row.RowIndex]["status"].ToString();
    //}

    //private void fnSeleccionarStatus()
    //{
    //    for (int i = 0; i < gvResultado.Rows.Count; i++ )
    //    {
    //        DataRow dr = ((DataTable)gvResultado.DataSource).Rows[i];
    //        DropDownList ddlStatus = gvResultado.Rows[i].FindControl("ddlStatus") as DropDownList;
    //        ddlStatus.SelectedValue = dr["status"].ToString();
    //    }
    //}

    protected void gvResultado_RowEditing(object sender, GridViewEditEventArgs e)
    {

        gvResultado.EditIndex = e.NewEditIndex;
        int nPagina = Convert.ToInt32(ViewState["paginado"]);
        fnBuscarComprobantes(nPagina);

        DropDownList ddlStatus = gvResultado.Rows[e.NewEditIndex].FindControl("ddlStatus") as DropDownList;
        ddlStatus.DataSource = fnObtenerStatus();//(DataTable)ViewState["dtStatus"];
        ddlStatus.DataTextField = "nombre";
        ddlStatus.DataValueField = "id_status";
        ddlStatus.DataBind();
        TextBox txtFechaPago = gvResultado.Rows[e.NewEditIndex].FindControl("txtFechaPago") as TextBox;
        string sFechaPago = ((DataTable)gvResultado.DataSource).Rows[e.NewEditIndex]["fecha_pago"].ToString();
        if (!string.IsNullOrEmpty(sFechaPago))
            txtFechaPago.Text = Convert.ToDateTime(sFechaPago).ToString("dd/MM/yyyy");

        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(((DataTable)gvResultado.DataSource).Rows[e.NewEditIndex]["status"].ToString()));
        if (ddlStatus.SelectedValue.ToString() == "3")
        {
            //Si el estatus es 'Pagado', deshabilita la selección de estatus.
            ddlStatus.Enabled = false;
        }
        else if (ddlStatus.SelectedValue.ToString() == "5")
        {

            txtFechaPago.Enabled = true;
            imgFin.Visible = true;

        }
        if (!((CheckBox)gvResultado.Rows[e.NewEditIndex].FindControl("chbValido")).Checked)
        {
            //Si el comprobante es inválido, elimina los estatus de 'Pagado', 'Contabilizado' y 'Programado'
            ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("3"));
            ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("4"));
            ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("5"));
        }
        ViewState["id_comprobante"] = Convert.ToInt32(((DataTable)gvResultado.DataSource).Rows[e.NewEditIndex]["id_comprobante"]);
        btnAnterior.Enabled = false;
        btnSiguiente.Enabled = false;
    }

    private DataTable fnObtenerStatus()
    {
        DataTable dtStatus = (DataTable)ViewState["dtStatus"];
        foreach (DataRow drStatus in dtStatus.Rows)
        {
            drStatus["nombre"] = fnStatusCultura(drStatus["nombre"].ToString());

        }
        return dtStatus;
    }

    private DataTable fnCambiarStatusCultura(DataTable dtResultado)
    {
        foreach (DataRow drResultado in dtResultado.Rows)
        {
            drResultado["status"] = fnStatusCultura(drResultado["status"].ToString());

        }
        return dtResultado;
    }
    private string fnStatusCultura(string sStatus)
    {

        switch (sStatus)
        {
            case "Recibida":
                sStatus = Resources.resCorpusCFDIEs.varRecibida;
                break;
            case "Rechazada":
                sStatus = Resources.resCorpusCFDIEs.varRechazada;
                break;
            case "Pagada":
                sStatus = Resources.resCorpusCFDIEs.varPagada;
                break;
            case "Contabilizada":
                sStatus = Resources.resCorpusCFDIEs.varContabilizada;
                break;
            case "Programada":
                sStatus = Resources.resCorpusCFDIEs.varProgramada;
                break;
            default:
                break;
        }
        return sStatus;
    }
    protected void gvResultado_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DropDownList ddlStatus = gvResultado.Rows[e.RowIndex].FindControl("ddlStatus") as DropDownList;
        TextBox txtFechaPago = gvResultado.Rows[e.RowIndex].FindControl("txtFechaPago") as TextBox;
        int nIdStatus = Convert.ToInt32(ddlStatus.SelectedValue);
        int nIdComprobante = Convert.ToInt32(ViewState["id_comprobante"]);
        string sFechaPago = string.Empty;
        if (nIdStatus == 5)
        {
            //Si el estatus es 'Programado', obtiene la fecha
            sFechaPago = txtFechaPago.Text;
        }
        new clsOperacionComprobantes().fnCambiarStatus(nIdComprobante, nIdStatus, sFechaPago);
        if (nIdStatus != 1)
        {
            //Si el estatus no es 'Recibido', envia un acuse por correo
            new clsEnviaAcusePago().fnEnviarAcuse(nIdComprobante);
        }
        ViewState.Remove("id_comprobante");
        gvResultado.EditIndex = -1;
        int nPagina = Convert.ToInt32(ViewState["paginado"]);
        fnBuscarComprobantes(nPagina);
        btnAnterior.Enabled = true;
        btnSiguiente.Enabled = true;
    }

    protected void gvResultado_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ViewState.Remove("id_comprobante");
        gvResultado.EditIndex = -1;
        int nPagina = Convert.ToInt32(ViewState["paginado"]);
        fnBuscarComprobantes(nPagina);
        btnAnterior.Enabled = true;
        btnSiguiente.Enabled = true;
    }

    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        int pagina = Convert.ToInt32(ViewState["paginado"]);
        pagina -= 1;

        if (pagina > 0)
        {
            ViewState["paginado"] = pagina;
            //fnRealizarConsultaPaginado(pagina, 2);
            fnBuscarComprobantes(pagina);
            pagina -= 1;
            if (pagina == 0)
                btnAnterior.Visible = false;
            else
                btnAnterior.Visible = true;
        }
        else
        {
            btnAnterior.Visible = false;
        }

    }

    protected void btnSiguiente_Click(object sender, EventArgs e)
    {
        int pagina = Convert.ToInt32(ViewState["paginado"]);
        pagina += 1;
        ViewState["paginado"] = pagina;
        //fnRealizarConsultaPaginado(pagina, 1);
        fnBuscarComprobantes(pagina);
        if (pagina > 1)
            btnAnterior.Visible = true;
        else
            btnAnterior.Visible = false;
        //pagina += 1;
        //bool bPaginas = fnVerificarPaginas(pagina);
        //if (bPaginas == true)
        //    btnSiguiente.Visible = true;
        //else
        //    btnSiguiente.Visible = false;
        // btnAnterior.Visible = true;
    }
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

    protected void btnDescargar_Click(object sender, EventArgs e)
    {
        try
        {
            //Pasar parámetros de consulta para la descarga masiva de comprobantes
            //Receptor|Estatus|Sucursal|Documentos|Series|Folio inicio|Folio fin|Fecha inicio|Fecha fin|Usuario

            string sLista = null;
            int nTipo;
            int esProveedor;
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            string sListaComprobantes = null;
            foreach (GridViewRow renglon in gvResultado.Rows)
            {
                CheckBox CbCan;
                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                if (CbCan.Checked)
                {
                    Label sIdComprobante = ((Label)renglon.FindControl("lblidcomprobante"));
                    if (sListaComprobantes != null)
                    {
                        sListaComprobantes = sListaComprobantes + "," + sIdComprobante.Text;
                    }
                    else
                    {
                        sListaComprobantes = sIdComprobante.Text;
                    }
                }
            }
            //if (!string.IsNullOrEmpty(sListaComprobantes))
            //{
                foreach (GridViewRow renglon in gvSucursales.Rows)
                {
                    CheckBox CbCan;

                    CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                    if (CbCan.Checked)
                    {
                        Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                        if (sLista != null)
                        {
                            sLista = sLista + "," + sIdSucursal.Text;
                        }
                        else
                        {
                            sLista = sIdSucursal.Text;
                        }
                    }
                }



                if (rbTipoBusqueda.SelectedValue == "0")
                {
                    nTipo = 0;
                }
                else
                {
                    nTipo = 1;
                }

                if (Convert.ToBoolean(ViewState["esProveedor"]))
                {
                    esProveedor = 1;
                }
                else
                {
                    esProveedor = 0;
                }

                string sVersion = ddlVersion.SelectedValue.Substring(0, 3);

                string sParametros = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
                                                Convert.ToDateTime(txtFechaInicio.Text),
                                                Convert.ToDateTime(txtFechaFin.Text),
                                                txtRfcEmisor.Text,
                                                txtRfcReceptor.Text,
                                                ddlVersion.SelectedValue,
                                                usuarioActivo.id_usuario,
                                                nTipo,
                                                sLista,
                                                sListaComprobantes
                                                );

                //Se encriptan los parámetros
                string sParamEncriptados = Utilerias.Encriptacion.Base64.EncriptarBase64(sParametros);


                ScriptManager.RegisterStartupScript(this, this.GetType(), "descargarComprobantes",
                                                            String.Format("<script>window.open('{0}?p={1}&f={2}','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                                                                           "webDescargarComprobantes.aspx", sParamEncriptados, esProveedor), false);
            //}
            //else
            //{
            //    clsComun.fnMostrarMensaje(this, "Debe seleccionar al menos un comprobante");
            //}

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    protected void cbSeleccionar_CheckedChanged(object sender, EventArgs e)
    {

        if (cbSeleccionar.Checked)
        {
            foreach (GridViewRow renglon in gvResultado.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                CbCan.Checked = true;

            }
        }
        else
        {
            foreach (GridViewRow renglon in gvResultado.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                CbCan.Checked = false;

            }
        }

    }
}
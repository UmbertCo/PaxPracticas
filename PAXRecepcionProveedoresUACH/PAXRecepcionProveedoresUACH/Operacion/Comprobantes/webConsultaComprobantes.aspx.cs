using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
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
                InitializeCulture();
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
                        Response.Redirect("~/Default.aspx");

                    if (new clsOperacionUsuarios().fnVerificarUsuarioPerfil(sesUsuario.id_usuario, "proveedor"))
                    {
                        ViewState["esProveedor"] = true;
                        fnLlenarSucursales();
                    }
                }

                //fnLlenaEmisores(dsFiltros.Tables[0]);
                //fnLlenaReceptores(dsFiltros.Tables[1]);
                fnLlenaVersiones();
                //fnCargarEmpresas();
                fnLlenarSucursales();
                //fnLlenaUsuarios(dsFiltros.Tables[3]);
                fnLlenaFechas();
                fnLLenarRegistros();
                //ddlTamañoPagina.Visible = false;
                //cbSeleccionar.Visible = false;
                //lblTamañoPagina.Visible = false;
                //btnDescargar.Visible = false;
                //fnCargarEstatus();
                //ViewState["dtStatus"] = new clsOperacionComprobantes().fnObtenerStatus();
                //ViewState["dtStatus"] = new clsOperacionUsuarios().fnObtenerEstatusUsuario(nId_Usuario);
            }
        }
        catch(Exception ex)
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

    private void fnLlenaFechas()
    {
        txtFechaInicio.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
        txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    private void fnLLenarRegistros()
    {
        ddlTamañoPagina.Items.Add(new ListItem("10"));
        ddlTamañoPagina.Items.Add(new ListItem("20"));
        ddlTamañoPagina.Items.Add(new ListItem("30"));
        ddlTamañoPagina.Items.Add(new ListItem("40"));
        ddlTamañoPagina.Items.Add(new ListItem("50"));
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
        //ddlVersion.Items.Add(new ListItem("3.0"));
        //ddlVersion.Items.Add(new ListItem("2.0"));
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
            btnSiguiente.Visible = false;
            ddlTamañoPagina.Visible = true;
            cbSeleccionar.Visible = true;
            lblTamañoPagina.Visible = true;
            fnBuscarComprobantes(1);
            ViewState["paginado"] = 1;
     
        }
        catch (Exception ex)
        {
            gvResultado.DataSource = new DataTable();
            gvResultado.DataBind();
        }
    }

    private void fnBuscarComprobantes(int nPagina)
    {
        //Verifica que la fecha inicial sea menor que la final
        DataTable dtResultado = new DataTable("dtResultadoComprobantes");
        int nTipo;
        if (DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            lMensajeError.Text = "La fecha final no debe ser mayor a la inicial";
        else
        {
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();

            string sRfcEmisor = string.Empty;
            string sRfcReceptor = string.Empty;
            string sVersion = string.Empty;
            sRfcEmisor = txtRfcEmisor.Text;
            sRfcReceptor = txtRfcReceptor.Text;
            string sLista = null;

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

            if (!ddlVersion.SelectedValue.Equals("Todas"))
            {
                //sVersion = ddlVersion.SelectedValue;
                sVersion = ddlVersion.SelectedValue.Substring(0, 4);
            }
            else
            {
                sVersion = null;
            }


            //realiza la busqueda
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

                dtResultado = new clsOperacionComprobantes().fnBuscarComprobantesProveedorPag(DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), sRfcEmisor, sRfcReceptor, sVersion, usuarioActivo.id_usuario, nTipo, sLista, nPagina, Convert.ToInt32(ddlTamañoPagina.SelectedValue));
                gvResultado.Columns[0].Visible = false;
                //gvResultado.AutoGenerateEditButton = false;        
            }
            else
            {
                dtResultado = new clsOperacionComprobantes().fnBuscarComprobantesPag(DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), sRfcEmisor, sRfcReceptor, sVersion, usuarioActivo.id_usuario, nTipo, sLista, nPagina, Convert.ToInt32(ddlTamañoPagina.SelectedValue));
                DataTable dtStatus = (DataTable)ViewState["dtStatus"];
                if (dtStatus == null || dtStatus.Rows.Count <= 0)
                {
                    gvResultado.Columns[0].Visible = true;
                    //gvResultado.AutoGenerateEditButton = false;
                    //gvResultado.FindControl("EditButton").Visible = false;    
                }
            }
            gvResultado.AutoGenerateColumns = false;
            gvResultado.DataSource = dtResultado;
            gvResultado.DataBind();

            if (gvResultado.Rows.Count > 0)
            {
                btnDescargar.Visible = true;
            }
            else
            {
                btnDescargar.Visible = false;
            }

            
            if ( gvResultado.Rows.Count < 10)
                btnSiguiente.Visible = false;
            else
                btnSiguiente.Visible = true;
            
            // fnSeleccionarStatus();

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
        if ((nIdStatus == 5 || nIdStatus == 6)&& ddlStatus.Enabled)
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
        gvResultado.Columns[5].Visible = false;
        gvResultado.Columns[6].Visible = false;
        gvResultado.Columns[7].Visible = false;
        gvResultado.Columns[8].Visible = false;
        gvResultado.EditIndex = e.NewEditIndex;
        int nNumPag = Convert.ToInt32(ViewState["paginado"]);
        fnBuscarComprobantes(nNumPag);
        Image img = (Image)gvResultado.Rows[e.NewEditIndex].FindControl("imgIni");
        img.Visible = false;
        DropDownList ddlStatus = gvResultado.Rows[e.NewEditIndex].FindControl("ddlStatus") as DropDownList;
        ddlStatus.DataSource = (DataTable)ViewState["dtStatus"];
        ddlStatus.DataTextField = "nombre";
        ddlStatus.DataValueField = "id_status";
        ddlStatus.DataBind();
        if (!((CheckBox)gvResultado.Rows[e.NewEditIndex].FindControl("chbValido")).Checked)
        {
            //Si el comprobante es inválido, elimina los estatus de 'Pagado', 'Contabilizado' y 'Programado'
            ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("3"));
            ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("4"));
            ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("5"));
        }

        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(((DataTable)gvResultado.DataSource).Rows[e.NewEditIndex]["status"].ToString()));
        if (ddlStatus.SelectedValue.ToString() == "3")
        {
            //Si el estatus es 'Pagado', deshabilita la selección de estatus.
            ddlStatus.Enabled = false;
        }
        else if (ddlStatus.SelectedValue.ToString() == "5" || ddlStatus.SelectedValue.ToString() == "6")
        {
            TextBox txtFechaPago = gvResultado.Rows[e.NewEditIndex].FindControl("txtFechaPago") as TextBox;
            txtFechaPago.Enabled = true;
            img.Visible = true;
        }
        
        ViewState["id_comprobante"] = Convert.ToInt32(((DataTable)gvResultado.DataSource).Rows[e.NewEditIndex]["id_comprobante"]);
    }

    protected void gvResultado_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DropDownList ddlStatus = gvResultado.Rows[e.RowIndex].FindControl("ddlStatus") as DropDownList;
        TextBox txtFechaPago = gvResultado.Rows[e.RowIndex].FindControl("txtFechaPago") as TextBox;
        int nIdStatus = Convert.ToInt32(ddlStatus.SelectedValue);
        int nIdComprobante = Convert.ToInt32(ViewState["id_comprobante"]);
        string sFechaPago = string.Empty;
        if (nIdStatus == 5 || nIdStatus == 6)
        {
            //Si el estatus es 'Programado' o 'Cheque Emitido', obtiene la fecha
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
        int nNumPag = Convert.ToInt32(ViewState["paginado"]);
        fnBuscarComprobantes(nNumPag);
    }

    protected void gvResultado_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvResultado.Columns[5].Visible = true;
        gvResultado.Columns[6].Visible = true;
        gvResultado.Columns[7].Visible = true;
        gvResultado.Columns[8].Visible = true;
        ViewState.Remove("id_comprobante");
        gvResultado.EditIndex = -1;
        int nNumPag = Convert.ToInt32(ViewState["paginado"]);
        fnBuscarComprobantes(nNumPag);
    }

    
    protected void GrvModulos_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GrvModulos_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    private void fnCargarEmpresas()
    {
        try
        {
            DataTable dtSucursales = new DataTable();
            clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
            dtSucursales = new clsOperacionSucursales().fnObtenerSucursalesEmpresas();
            gvSucursales.DataSource = dtSucursales;
            gvSucursales.DataBind();
            gvSucursales.Visible = true;
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

        }
        catch (Exception)
        {
           
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
            //dtSucursales = new clsOperacionProveedores().fnObtenerSucursalesUsuario(usuarioActivo.id_usuario);
        }
        gvSucursales.DataSource = dtSucursales;
        gvSucursales.DataBind();
        gvSucursales.Visible = true;
    }
    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        int pagina = Convert.ToInt32(ViewState["paginado"]);
        pagina -= 1;

        if (pagina > 0)
        {
            ViewState["paginado"] = pagina;
            fnBuscarComprobantes(pagina);
            //fnRealizarConsultaPaginado(pagina, 2);
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
        //if (gvResultado.Rows.Count > 0)
        //    btnSiguiente.Visible = true;
        //else
        //    btnSiguiente.Visible = false;
    }

    protected void btnSiguiente_Click(object sender, EventArgs e)
    {
        int pagina = Convert.ToInt32(ViewState["paginado"]);
        pagina += 1;
        ViewState["paginado"] = pagina;
        fnBuscarComprobantes(pagina);
        //fnRealizarConsultaPaginado(pagina, 1);
        pagina += 1;
        //bool bPaginas = fnVerificarPaginas(pagina);
        //if (bPaginas == true)
        //if(gvResultado.Rows.Count>0)
        //    btnSiguiente.Visible = true;
        //else
        //    btnSiguiente.Visible = false;
        // btnAnterior.Visible = true;
        if (pagina > 1)
            btnAnterior.Visible = true;
        else
            btnAnterior.Visible = false;
    }

    /// <summary>
    /// selecciona todas las filas del gridview
    /// </summary>
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

    /// <summary>
    /// comprime en un zip todos los xml y pdf contenidos en las filas seleccionadas del grid
    /// </summary>
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

            string sVersion = ddlVersion.SelectedValue.Substring(0,3);

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
                                                                       "webDescargaComprobantes.aspx", sParamEncriptados,esProveedor), false);

       
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        
    }


}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Operacion_Empleados_webOperacionNominaEmpleados : System.Web.UI.Page
{
    clsInicioSesionUsuario datosUsuario;
    private string SeleccioneUnValor = Resources.resCorpusCFDIEs.varSeleccione;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                fnCargarPeriodos();
                fnCargarBanco();
                fnCargarEstatus();
                fnCargarNumeroRegistrosPorVista();
                fnCargarSucursales();
                fnCargarRiesgoPuesto();
                fnCargarTipoRegimen();

                hdId_Empleado.Value = "0";
                ViewState["IdTipoPeriodo"] = 0;
                ddlSucursales_SelectedIndexChanged(sender, e);
                ddlPeriodos.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            clsErrorLog.fnNuevaEntrada(objErr, clsErrorLog.TipoErroresLog.BaseDatos);
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx");
        }
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
        bool bResultado = false;
        try
        {
            string sRFC = txtRFC.Text;
            string sNombre = txtNombre.Text;
            string sRegistroPatronal = txtRegistroPatronal.Text;
            string sNumeroEmpleado = txtNumeroEmpleado.Text;
            string sCURP = txtCURP.Text;
            int nTipoRegimen = Convert.ToInt32(ddlTipoRegimen.SelectedValue);
            string sNumeroSeguridadSocial = txtNumeroSeguridadSocial.Text;
            string sDepartamento = txtDepartamento.Text;
            string sCLABE = txtCLABE.Text;
            int nBanco = Convert.ToInt32(ddlBanco.SelectedValue);
            DateTime dFechaInicioRelacionLaboral = (txtFechaInicioRelacionLaboral.Text.Equals(string.Empty) ? Convert.ToDateTime("01-01-1900") : Convert.ToDateTime(txtFechaInicioRelacionLaboral.Text));
            string sPuesto = txtPuesto.Text;
            string sTipoContrato = txtTipoContrato.Text;
            string sTipoJornada = txtTipoJornada.Text;
            double nSalarioBase = (txtSalarioBase.Text.Equals(string.Empty) ? 0 : Convert.ToDouble(txtSalarioBase.Text));
            double nSalarioDiarioIntegrado = (txtSalarioDiarioIntegrado.Text.Equals(string.Empty) ? 0 : Convert.ToDouble(txtSalarioDiarioIntegrado.Text));
            int nRiesgoPuesto = Convert.ToInt32(ddlRiesgoPuesto.SelectedValue);
            string sCorreo = txtCorreo.Text;
            int nId_Estructura = Convert.ToInt32(ddlSucursales.SelectedValue);
            char sEstatus = Convert.ToChar(ddlEstatus.SelectedValue);
            int nId_Periodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            if (!string.IsNullOrEmpty(sCLABE) && !sCLABE.Length.Equals(18))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valClabe);
                return;
            }

            if (nId_Periodo.Equals(0) && !ViewState["IdTipoPeriodo"].ToString().Equals("-1"))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valPeriodo);
                return;
            }

            if (!fnRegistrarPeriodoEstructura(nId_Estructura, nId_Periodo))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorGuardarEmpleado);
                return;
            }

            if (Convert.ToInt32(hdId_Empleado.Value).Equals(0))
            {
                bResultado = cEmpleados.fnIngresarEmpleado(sNombre, PAXCrypto.CryptoAES.EncriptaAES(sCorreo), PAXCrypto.CryptoAES.EncriptaAES(sRegistroPatronal),
                    nRiesgoPuesto, sNumeroEmpleado, PAXCrypto.CryptoAES.EncriptaAES(sCURP), sRFC, nBanco,
                    PAXCrypto.CryptoAES.EncriptaAES(sCLABE), nTipoRegimen, PAXCrypto.CryptoAES.EncriptaAES(sNumeroSeguridadSocial), sDepartamento, sPuesto, sTipoContrato, sTipoJornada,
                    dFechaInicioRelacionLaboral, PAXCrypto.CryptoAES.EncriptaAES(Convert.ToString(nSalarioBase)), PAXCrypto.CryptoAES.EncriptaAES(Convert.ToString(nSalarioDiarioIntegrado)), nId_Estructura, sEstatus);

                if (!bResultado)
                    return;

                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
            }
            else
            {
                bResultado = cEmpleados.fnActualizarEmpleado(Convert.ToInt32(hdId_Empleado.Value), sNombre, PAXCrypto.CryptoAES.EncriptaAES(sCorreo),
                    PAXCrypto.CryptoAES.EncriptaAES(sRegistroPatronal), nRiesgoPuesto,
                    sNumeroEmpleado, PAXCrypto.CryptoAES.EncriptaAES(sCURP), sRFC, nBanco, PAXCrypto.CryptoAES.EncriptaAES(sCLABE), nTipoRegimen, 
                    PAXCrypto.CryptoAES.EncriptaAES(sNumeroSeguridadSocial), sDepartamento, sPuesto, sTipoContrato, sTipoJornada, dFechaInicioRelacionLaboral,
                    PAXCrypto.CryptoAES.EncriptaAES(Convert.ToString(nSalarioBase)), PAXCrypto.CryptoAES.EncriptaAES(Convert.ToString(nSalarioDiarioIntegrado)), nId_Estructura, sEstatus);

                if (!bResultado)
                    return;

                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);
            }

            hdId_Empleado.Value = "0";
            fnLimpiarPantalla(pnlDatosEmpleado);
            btnConsultar_Click(sender, e);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            if (Convert.ToInt32(hdId_Empleado.Value).Equals(0))
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            if (Convert.ToInt32(hdId_Empleado.Value).Equals(0))
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
        }
    }
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        fnHabilitarDeshabiliarControles(true);
        ddlEstatus.Enabled = false;
        ddlEstatus.SelectedValue = "A";
        btnGuardar.Enabled = true;
        hdId_Empleado.Value = "0";
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        fnHabilitarDeshabiliarControles(false);
        fnLimpiarPantalla(pnlDatosEmpleado);
        btnNuevo.Enabled = true;
        btnGuardar.Enabled = false;
        hdId_Empleado.Value = "0";
        gdvEmpleados.SelectedIndex = -1;
        ddlPeriodos.Enabled = false;
    }
    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtRfc = new DataTable();
        DataTable dtPeriodosSucursales = new DataTable();

        clsOperacionRFC cOperacionRFC = new clsOperacionRFC();
        dtRfc = cOperacionRFC.fnObtieneRFCsEstructura(Convert.ToInt32(ddlSucursales.SelectedValue));

        if (dtRfc.Rows.Count <= 0)
        {
            ViewState["IdTipoPeriodo"] = -1;
            ddlPeriodos.SelectedValue = "0";
            ddlPeriodos.Enabled = false;
            return;
        }

        ddlPeriodos.Enabled = true;
        clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();
        dtPeriodosSucursales = cTimbradoNomina.LlenarDropPeriodos(Convert.ToInt32(ddlSucursales.SelectedValue));

        if (dtPeriodosSucursales.Rows.Count <= 0)
        {
            ViewState["IdTipoPeriodo"] = 0;
            return;
        }

        ddlPeriodos.SelectedValue = dtPeriodosSucursales.Rows[0]["IdTipoPeriodo"].ToString();
        ViewState["IdTipoPeriodo"] = ddlPeriodos.SelectedValue;

    }
    protected void gdvEmpleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtEmpleado = new DataTable();
        try
        {
            if (gdvEmpleados.SelectedIndex < 0)
                return;

            GridViewRow fila = gdvEmpleados.SelectedRow;

            clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
            hdId_Empleado.Value = gdvEmpleados.SelectedDataKey.Value.ToString();

            dtEmpleado = cEmpleados.fnExisteEmpleado(Convert.ToInt32(hdId_Empleado.Value));

            fnLimpiarPantalla(pnlDatosEmpleado);
            if (dtEmpleado.Rows.Count.Equals(0))
                return;

            txtRFC.Text = dtEmpleado.Rows[0]["RFC"].ToString();
            txtNombre.Text = dtEmpleado.Rows[0]["Nombre"].ToString();
            txtRegistroPatronal.Text = dtEmpleado.Rows[0]["RegistroPatronal"].ToString();
            txtNumeroEmpleado.Text = dtEmpleado.Rows[0]["NumEmpleado"].ToString();
            txtCURP.Text = dtEmpleado.Rows[0]["CURP"].ToString();
            ddlTipoRegimen.SelectedValue = dtEmpleado.Rows[0]["TipoRegimen"].ToString();
            txtNumeroSeguridadSocial.Text = dtEmpleado.Rows[0]["NumSeguridadSocial"].ToString();
            txtDepartamento.Text = dtEmpleado.Rows[0]["Departamento"].ToString();
            txtCLABE.Text = dtEmpleado.Rows[0]["CLABE"].ToString();
            ddlBanco.SelectedValue = (dtEmpleado.Rows[0]["Banco"].ToString().Equals(string.Empty) ? "0" : dtEmpleado.Rows[0]["Banco"].ToString());
            txtFechaInicioRelacionLaboral.Text = (dtEmpleado.Rows[0]["FechaInicioRelLaboral"].ToString().Equals(string.Empty) ? string.Empty : Convert.ToDateTime(dtEmpleado.Rows[0]["FechaInicioRelLaboral"].ToString()).ToShortDateString());
            txtPuesto.Text = dtEmpleado.Rows[0]["Puesto"].ToString();
            txtTipoContrato.Text = dtEmpleado.Rows[0]["TipoContrato"].ToString();
            txtTipoJornada.Text = dtEmpleado.Rows[0]["TipoJornada"].ToString();
            txtSalarioBase.Text = dtEmpleado.Rows[0]["SalarioBaseCotApor"].ToString();
            txtSalarioDiarioIntegrado.Text = dtEmpleado.Rows[0]["SalarioDiarioIntegrado"].ToString();
            ddlRiesgoPuesto.SelectedValue = (dtEmpleado.Rows[0]["RiesgoPuesto"].ToString().Equals(string.Empty) ? "0" : dtEmpleado.Rows[0]["RiesgoPuesto"].ToString());
            txtCorreo.Text = dtEmpleado.Rows[0]["Correo"].ToString();
            ddlSucursales.SelectedValue = dtEmpleado.Rows[0]["id_estructura"].ToString();
            ddlEstatus.SelectedValue = dtEmpleado.Rows[0]["Estatus"].ToString();
            
            btnNuevo.Enabled = false;
            btnGuardar.Enabled = true;
            fnHabilitarDeshabiliarControles(true);
            ddlSucursales_SelectedIndexChanged(sender, e);

            if (ViewState["IdTipoPeriodo"].ToString() != "-1")
                ddlPeriodos.Enabled = true;
            else
                ddlPeriodos.Enabled = false;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }
    protected void gdvEmpleados_Sorting(object sender, GridViewSortEventArgs e)
    {
        //Retrieve the table from the session object.
        DataTable dt = (DataTable)ViewState["dtEmpleados"];
        if (dt != null)
        {
            //Sort the data.
            dt.DefaultView.Sort = e.SortExpression + " " + fnGetSortDirection(e.SortExpression);
            gdvEmpleados.DataSource = ViewState["dtEmpleados"];
            gdvEmpleados.DataBind();
        }
    }
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        ViewState["paginado"] = 1;

        fnLimpiarPantalla(pnlDatosEmpleado);
        fnHabilitarDeshabiliarControles(false);
        btnGuardar.Enabled = false;
        btnNuevo.Enabled = true;

        fnBuscarEmpleados(Convert.ToInt32(ViewState["paginado"]), Convert.ToInt32(ddlNumeroRegistros.SelectedValue));

        btnAnterior.Visible = false;
        btnSiguiente.Visible = false;
        lblNumeroRegistros.Visible = false;
        ddlNumeroRegistros.Visible = false;

        if (fnVerificarPagina(2, Convert.ToInt32(ddlNumeroRegistros.SelectedValue)))
        {
            btnSiguiente.Visible = true;
        }

        if (!gdvEmpleados.Rows.Count.Equals(0))
        {
            lblNumeroRegistros.Visible = true;
            ddlNumeroRegistros.Visible = true;
        }
    }
    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        int pagina = Convert.ToInt32(ViewState["paginado"]);
        pagina -= 1;

        btnAnterior.Visible = false;

        if (pagina > 0)
        {
            ViewState["paginado"] = pagina;
            fnBuscarEmpleados(pagina, Convert.ToInt32(ddlNumeroRegistros.SelectedValue));

            btnSiguiente.Visible = true;

            if (pagina > 1)
                btnAnterior.Visible = true;
        }
    }
    protected void btnSiguiente_Click(object sender, EventArgs e)
    {
        int nPagina = Convert.ToInt32(ViewState["paginado"]);
        nPagina += 1;
        ViewState["paginado"] = nPagina;
        fnBuscarEmpleados(nPagina, Convert.ToInt32(ddlNumeroRegistros.SelectedValue));

        btnAnterior.Visible = true;
        btnSiguiente.Visible = false;

        nPagina += 1;
        if (fnVerificarPagina(nPagina, Convert.ToInt32(ddlNumeroRegistros.SelectedValue)))
            btnSiguiente.Visible = true;
    }
    protected void ddlNumeroRegistros_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnConsultar_Click(sender, e);
    }
    protected void ddlRiesgoPuesto_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void ddlBanco_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void ddlPeriodos_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void ddlTipoRegimen_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void AgregaOpcionSeleccione(System.Object sender, System.EventArgs e)
    {
        ((DropDownList)sender).Items.Insert(0, new ListItem(SeleccioneUnValor, "0"));
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

    /// <summary>
    /// Metódo que se encarga de buscar los empleados
    /// <param name="pnPagina"></param>
    /// <param name="pnResultadosPorPagina"></param>
    /// </summary>
    public void fnBuscarEmpleados(int pnPagina, int pnResultadosPorPagina)
    {
        DataTable dtEmpleados = new DataTable();
        try
        {
            clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
            dtEmpleados = cEmpleados.fnBuscarEmpleados(txtNumeroEmpleadoBusqueda.Text, txtRFCBusqueda.Text, txtNombreBusqueda.Text, Convert.ToInt32(ddlSucursalBusqueda.SelectedValue), pnPagina, pnResultadosPorPagina);
            gdvEmpleados.DataSource = dtEmpleados;
            gdvEmpleados.DataBind();

            ViewState["dtEmpleados"] = dtEmpleados;
            gdvEmpleados.SelectedIndex = -1;            
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    /// <summary>
    /// Metódo que se encarga cargar los estatus
    /// </summary>
    private void fnCargarEstatus()
    {
        DataTable dtEstatus = new DataTable();
        dtEstatus.Columns.Add("Estatus", typeof(string));
        dtEstatus.Columns.Add("Descripcion", typeof(string));

        dtEstatus.Rows.Add("A", "Activo");
        dtEstatus.Rows.Add("I", "Inactivo");

        ddlEstatus.DataSource = dtEstatus;
        ddlEstatus.DataBind();
    }

    /// <summary>
    /// Metódo que se encarga de cargar los bancos
    /// </summary>
    private void fnCargarBanco()
    {
        try
        {
            clsOperacionBanco cBancos = new clsOperacionBanco();
            ddlBanco.DataSource = cBancos.fnListarBancos();
            ddlBanco.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlBanco.DataSource = null;
            ddlBanco.DataBind();
        }
    }

    /// <summary>
    /// Función que carga el numero de registros que va contener el grid por página.
    /// </summary>
    private void fnCargarNumeroRegistrosPorVista()
    {
        ddlNumeroRegistros.DataSource = new int[] { 5, 10, 15, 20 };
        ddlNumeroRegistros.DataBind();
    }

    /// <summary>
    /// Método que se encarga de cargar los períodos disponibles
    /// </summary>
    private void fnCargarPeriodos()
    {
        clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();
        try
        {
            ddlPeriodos.DataSource = cTimbradoNomina.LlenarDropPeriodos();
            ddlPeriodos.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlPeriodos.DataSource = null;
            ddlPeriodos.DataBind();
        }
    }

    /// <summary>
    /// Trae la lista de sucursales activas asignadas al usuario y las carga en el drop
    /// </summary>
    private void fnCargarSucursales()
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            ddlSucursales.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true);
            ddlSucursales.DataBind();

            ddlSucursalBusqueda.DataSource = ddlSucursales.DataSource;
            ddlSucursalBusqueda.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlSucursales.DataSource = null;
            ddlSucursales.DataBind();
        }
    }

    /// <summary>
    /// Metódo que se encarga de cargar los riesgos de puesto
    /// </summary>
    private void fnCargarRiesgoPuesto()
    {
        try
        {
            clsOperacionRiesgoPuesto cRiesgoPuesto = new clsOperacionRiesgoPuesto();
            ddlRiesgoPuesto.DataSource = cRiesgoPuesto.fnListarTiposRiesgosPuesto();
            ddlRiesgoPuesto.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlRiesgoPuesto.DataSource = null;
            ddlRiesgoPuesto.DataBind();
        }
    }

    /// <summary>
    /// Metódo que se encarga de cargar los tipos de regimenes
    /// </summary>
    private void fnCargarTipoRegimen()
    {
        try
        {
            clsOperacionTipoRegimen cTipoREgimen = new clsOperacionTipoRegimen();
            ddlTipoRegimen.DataSource = cTipoREgimen.fnListarTiposRegimenes();
            ddlTipoRegimen.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlTipoRegimen.DataSource = null;
            ddlTipoRegimen.DataBind();
        }
    }

    /// <summary>
    /// Función que se encarga de obtener el ordenamiento
    /// </summary>
    /// <param name="column">Columna a ordernar</param>
    /// <returns></returns>
    private string fnGetSortDirection(string column)
    {
        // By default, set the sort direction to ascending.
        string sortDirection = "ASC";

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection"] = sortDirection;
        ViewState["SortExpression"] = column;

        return sortDirection;
    }

    /// <summary>
    /// Función que se encarga de habilitar o deshabilitar los controles del formulario
    /// </summary>
    /// <param name="pbAccion"></param>
    private void fnHabilitarDeshabiliarControles(bool pbAccion)
    {
        //Datos del empleado
        txtRFC.Enabled = pbAccion;
        txtNombre.Enabled = pbAccion;
        txtRegistroPatronal.Enabled = pbAccion;
        txtNumeroEmpleado.Enabled = pbAccion;
        txtCURP.Enabled = pbAccion;
        ddlTipoRegimen.Enabled = pbAccion;
        txtNumeroSeguridadSocial.Enabled = pbAccion;
        txtDepartamento.Enabled = pbAccion;
        txtCLABE.Enabled = pbAccion;
        ddlBanco.Enabled = pbAccion;
        txtFechaInicioRelacionLaboral.Enabled = pbAccion;
        txtPuesto.Enabled = pbAccion;
        txtTipoContrato.Enabled = pbAccion;
        txtTipoJornada.Enabled = pbAccion;
        txtSalarioBase.Enabled = pbAccion;
        txtSalarioDiarioIntegrado.Enabled = pbAccion;
        ddlRiesgoPuesto.Enabled = pbAccion;
        txtCorreo.Enabled = pbAccion;
        ddlSucursales.Enabled = pbAccion;
        ddlEstatus.Enabled = pbAccion;
        ddlPeriodos.Enabled = pbAccion;
    }

    /// <summary>
    /// Vacía todos los controles del formulario
    /// </summary>
    private void fnLimpiarPantalla(Panel pPnl)
    {
        try
        {
            clsComunCatalogo.fnLimpiarFormulario(pPnl);
        }
        finally
        {
            //hdId_Empleado.Value = "0";
            ddlTipoRegimen.SelectedIndex = -1;
            ddlBanco.SelectedIndex = -1;
            ddlRiesgoPuesto.SelectedIndex = -1;
            ddlSucursales.SelectedIndex = -1;
            ddlSucursales_SelectedIndexChanged(new object(), new EventArgs());
            ddlEstatus.SelectedIndex = -1;
        }
    }

    /// <summary>
    /// Función que se encarga de registrar el periodo de una estructura especifica
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <param name="pnId_Periodo">ID del Periodo</param>
    /// <returns></returns>
    private bool fnRegistrarPeriodoEstructura(int pnId_Estructura, int pnId_Periodo)
    {
        bool bResultado = false;
        clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
        try
        {
            if(Convert.ToInt32(ViewState["IdTipoPeriodo"]).Equals(0) && pnId_Periodo > 0)
                bResultado = cEmpleados.fnIngresarSucursalPeriodo(pnId_Estructura, pnId_Periodo);
            else if(Convert.ToInt32(ViewState["IdTipoPeriodo"]) > 0 && !Convert.ToInt32(ViewState["IdTipoPeriodo"]).Equals(pnId_Periodo))
                bResultado = cEmpleados.fnActualizarSucursalPeriodo(pnId_Estructura, pnId_Periodo);
            else
                bResultado = true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        return bResultado;
    }

    /// <summary>
    /// Metódo que se encarga de verificar si la siguiente página tiene registros
    /// </summary>
    /// <param name="pnPagina">Página a ver</param>
    /// <param name="pnNumeroRegistros">Número de registros a ver</param>
    /// <returns></returns>
    private bool fnVerificarPagina(int pnPagina, int pnNumeroRegistros)
    {
        DataTable dtEmpleados = new DataTable();
        bool condicion = false;
        try
        {
            clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
            dtEmpleados = cEmpleados.fnBuscarEmpleados(txtNumeroEmpleadoBusqueda.Text, txtRFCBusqueda.Text, txtNombreBusqueda.Text, Convert.ToInt32(ddlSucursalBusqueda.SelectedValue), pnPagina, pnNumeroRegistros);

            if (dtEmpleados.Rows.Count > 0)
                condicion = true;

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
}
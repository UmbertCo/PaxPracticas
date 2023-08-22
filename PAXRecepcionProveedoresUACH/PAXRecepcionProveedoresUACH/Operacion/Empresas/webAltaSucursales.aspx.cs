using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Globalization;

public partial class Operacion_Empresas_webAltaSucursales : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeCulture();
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


            }
            fnObtenerEmpresas();
            fnObtenerSucursales();
            fnCargarPaises();
            fnCargarEstados();
            fnCargarMunicipios();
            fnActivarCampos(false);
            btnNCancelar.Enabled = false;
        }
    }

    /// <summary>
    /// Obtiene las empresas correspondientes al usuario
    /// </summary>
    private void fnObtenerEmpresas()
    {
        ddlEmpresas.Items.Clear();
        clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
        DataTable dtEmpresas = new clsOperacionSucursales().fnObtenerEmpresasUsuario(usuarioActivo.id_usuario);
        if (dtEmpresas != null && dtEmpresas.Rows.Count > 0)
        {
            ddlEmpresas.DataSource = dtEmpresas;
            ddlEmpresas.DataTextField = "razon_social";
            ddlEmpresas.DataValueField = "id_empresa";
            ddlEmpresas.DataBind();
            ddlEmpresas.Enabled = true;
            btnEditar.Enabled = true;
            btnBorrar.Enabled = true;
        }
        else
        {
            ddlEmpresas.Items.Add(new ListItem(Resources.resCorpusCFDIEs.lblRegistreEmpresa, "0"));
            ddlEmpresas.DataBind();
            ddlEmpresas.Enabled = false;
            btnNuevo.Enabled = false;
        }
        fnObtenerSucursales();
    }
    protected void btnGuardarEmpresa_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtNombre.Text)
            || string.IsNullOrEmpty(txtCalle.Text)
            || string.IsNullOrEmpty(txtColonia.Text))
        {
            //clsComun.fnMostrarMensaje(this, "Faltan datos");
            return;
        }
        //Verifica que tenga al menos un correo
        if (lbEmailsAcuses.Items.Count <= 0)
        {
            //clsComun.fnMostrarMensaje(this, "Debe agregar al menos un correo");
            txtCorreo.Focus();
            return;
        }
        //Verifica el código postal
        if (!string.IsNullOrEmpty(txtCodigoPostal.Text))
        {
            try
            {
                Convert.ToInt32(txtCodigoPostal.Text);
            }
            catch (Exception ex)
            {
                //clsComun.fnMostrarMensaje(this, "El Código postal debe ser numérico");
                txtCodigoPostal.Focus();
                return;
            }
            if (txtCodigoPostal.Text.Length != 5)
            {
                //clsComun.fnMostrarMensaje(this, "El Código postal debe ser de 5 dígitos");
                txtCodigoPostal.Focus();
                return;
            }
        }
        try
        {

            int nIdSucursal = 0;
            if (ViewState["id_sucursal"] != null)
            {
                nIdSucursal = Convert.ToInt32(ViewState["id_sucursal"]);
            }
            //Verifica que esté una empresa seleccionada
            int nIdEmpresa = Convert.ToInt32(ddlEmpresas.SelectedValue);
            if (nIdEmpresa <= 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblAltaEmpresa);
                return;
            }
            int nIdMunicipio = Convert.ToInt32(ddlMunicipio.SelectedValue);
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            //Guarda la sucursal
            nIdSucursal = new clsOperacionSucursales().fnGuardarSucursal(nIdSucursal, nIdEmpresa, usuarioActivo.id_usuario,
               txtNombre.Text, nIdMunicipio, txtLocalidad.Text, txtColonia.Text, txtCalle.Text, txtNoExterior.Text, txtNoInterior.Text,
               txtCodigoPostal.Text, chbUnica.Checked);
            if (nIdSucursal > 0)
            {
                //Agrega los correos, borrando los anteriores
                new clsOperacionSucursales().fnBorrarCorreos(nIdSucursal);
                if (!new clsOperacionSucursales().fnGuardarCorreos(lbEmailsAcuses.Items, nIdSucursal))
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblCorreosGuardarFallo);
                }

            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblFacultadGuardada);
            fnActivarCampos(false);
            fnLimpiarCampos();
            fnObtenerSucursales();
            btnNCancelar.Enabled = false;
            btnEditar.Enabled = true;
            btnBorrar.Enabled = true;
            ddlEmpresas.Enabled = true;
            ddlSucursales.Enabled = true;
            btnNuevo.Enabled = true;
            ViewState.Remove("id_sucursal");
        }

    }
    /// <summary>
    /// Agrega un correo a la lista
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAgregarMail_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtCorreo.Text))
        {
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }
        string sCorreo = txtCorreo.Text;
        if (!clsComun.fnValidaExpresion(sCorreo, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
        {
            //clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtCorreo);
            return;
        }

        foreach (ListItem item in lbEmailsAcuses.Items)
        {
            if (item.Text.Equals(sCorreo))
            {
                return;
            }
        }
        lbEmailsAcuses.Items.Add(sCorreo);
        lbEmailsAcuses.DataBind();
        txtCorreo.Text = "";
    }

    protected void btnQuitarMail_Click(object sender, EventArgs e)
    {
        if (lbEmailsAcuses.SelectedIndex >= 0)
        {
            lbEmailsAcuses.Items.Remove(lbEmailsAcuses.SelectedItem);
            lbEmailsAcuses.DataBind();

        }
    }
    /// <summary>
    /// Método encargado de llenar el drop de los paises
    /// </summary>
    private void fnCargarPaises()
    {
        ddlPais.DataSource = clsComun.fnLlenarDropPaises();
        ddlPais.DataValueField = "id_pais";
        ddlPais.DataTextField = "nombre";
        ddlPais.DataBind();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los estados
    /// </summary>
    /// <param name="psIdEstado"></param>
    private void fnCargarEstados()
    {
        ddlEstado.DataSource = clsComun.fnLlenarDropEstados(ddlPais.SelectedValue);
        ddlEstado.DataValueField = "id_estado";
        ddlEstado.DataTextField = "nombre";
        ddlEstado.DataBind();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los municipios
    /// </summary>
    /// <param name="psIdPais"></param>
    private void fnCargarMunicipios()
    {
        ddlMunicipio.DataSource = clsComun.fnLlenarDropMunicipios(ddlEstado.SelectedValue);
        ddlMunicipio.DataValueField = "id_municipio";
        ddlMunicipio.DataTextField = "nombre";
        ddlMunicipio.DataBind();
    }

    /// <summary>
    /// Activa y desactiva los campos para los datos de la empresa.
    /// </summary>
    /// <param name="activo"></param>
    private void fnActivarCampos(bool activo)
    {
        txtCalle.Enabled = activo;
        ddlPais.Enabled = activo;
        ddlEstado.Enabled = activo;
        ddlMunicipio.Enabled = activo;
        txtLocalidad.Enabled = activo;
        txtColonia.Enabled = activo;
        txtNoExterior.Enabled = activo;
        txtNoInterior.Enabled = activo;
        txtCodigoPostal.Enabled = activo;
        txtCorreo.Enabled = activo;
        txtNombre.Enabled = activo;
        lbEmailsAcuses.Enabled = activo;
        btnAgregarMail.Enabled = activo;
        btnQuitarMail.Enabled = activo;
        btnGuardarEmpresa.Enabled = activo;
        chbUnica.Enabled = activo;
    }

    /// <summary>
    /// Limpia los datos de los campos de la empresa
    /// </summary>
    private void fnLimpiarCampos()
    {
        txtCalle.Text = "";
        ddlPais.SelectedIndex = 0;
        ddlEstado.SelectedIndex = 0;
        ddlMunicipio.SelectedIndex = 0;
        txtLocalidad.Text = "";
        txtColonia.Text = "";
        txtNoExterior.Text = "";
        txtNoInterior.Text = "";
        txtCodigoPostal.Text = "";
        txtCorreo.Text = "";
        txtNombre.Text = "";
        chbUnica.Checked = false;
        lbEmailsAcuses.Items.Clear();
        lbEmailsAcuses.DataBind();
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        fnActivarCampos(true);
        fnLimpiarCampos();
        btnBorrar.Enabled = false;
        btnEditar.Enabled = false;
        btnNuevo.Enabled = false;
        ddlEmpresas.Enabled = false;
        ddlSucursales.Enabled = false;
        btnNCancelar.Enabled = true;
    }

    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        fnActivarCampos(false);
        fnLimpiarCampos();
        btnEditar.Enabled = true;
        btnBorrar.Enabled = true;
        btnNuevo.Enabled = true;
        ddlEmpresas.Enabled = true;
        ddlSucursales.Enabled = true;
        btnNCancelar.Enabled = false;
        ViewState.Remove("id_sucursal");
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        int nIdSucursal = Convert.ToInt32(ddlSucursales.SelectedValue);
        if (nIdSucursal > 0)
        {
            DataTable dtSucursales = new clsOperacionSucursales().fnObtenerSucursal(nIdSucursal);
            if (dtSucursales.Rows.Count > 0)
            {
                ViewState["id_sucursal"] = nIdSucursal;
                ddlEmpresas.Enabled = false;
                ddlSucursales.Enabled = false;
                txtNombre.Text = dtSucursales.Rows[0]["nombre"].ToString();
                txtColonia.Text = dtSucursales.Rows[0]["colonia"].ToString();
                txtCalle.Text = dtSucursales.Rows[0]["calle"].ToString();
                txtLocalidad.Text = dtSucursales.Rows[0]["localidad"].ToString();
                txtNoExterior.Text = dtSucursales.Rows[0]["no_exterior"].ToString();
                txtNoInterior.Text = dtSucursales.Rows[0]["no_interior"].ToString();
                txtCodigoPostal.Text = dtSucursales.Rows[0]["codigo_postal"].ToString();
                chbUnica.Checked = Convert.ToBoolean(dtSucursales.Rows[0]["factura_unica_mes"].ToString());
                ddlPais.SelectedIndex = ddlPais.Items.IndexOf(ddlPais.Items.FindByText(dtSucursales.Rows[0]["pais"].ToString()));
                fnCargarEstados();
                ddlEstado.SelectedIndex = ddlEstado.Items.IndexOf(ddlEstado.Items.FindByText(dtSucursales.Rows[0]["estado"].ToString()));
                fnCargarMunicipios();
                ddlMunicipio.SelectedIndex = ddlMunicipio.Items.IndexOf(ddlMunicipio.Items.FindByText(dtSucursales.Rows[0]["municipio"].ToString()));
                fnObtenerCorreos(nIdSucursal);
                fnActivarCampos(true);
                btnNuevo.Enabled = false;
                btnEditar.Enabled = false;
                btnBorrar.Enabled = false;
                btnNCancelar.Enabled = true;
            }

        }
    }
    /// <summary>
    /// Obtiene los correos de la sucursal
    /// </summary>
    /// <param name="nIdSucursal"></param>
    private void fnObtenerCorreos(int nIdSucursal)
    {
        DataTable dtCorreos = new clsOperacionSucursales().fnObtenerCorreosSucursal(nIdSucursal);
        if (dtCorreos.Rows.Count > 0)
        {
            lbEmailsAcuses.DataSource = dtCorreos;
            lbEmailsAcuses.DataValueField = "correo";
            lbEmailsAcuses.DataBind();
        }
    }
    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        int nIdSucursal = Convert.ToInt32(ddlSucursales.SelectedValue);
        if (nIdSucursal > 0)
        {

            int res = new clsOperacionSucursales().fnEliminarSucursal(nIdSucursal);
            if (res > 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblFacultadEliminar);
                fnObtenerSucursales();

            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblFacultadLigada);
            }
        }
    }

    protected void ddlEmpresas_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnObtenerSucursales();
    }
    /// <summary>
    /// Obtiene las sucursales de la empresa seleccionada
    /// </summary>
    private void fnObtenerSucursales()
    {
        ddlSucursales.Items.Clear();
        int nIdEmpresa = Convert.ToInt32(ddlEmpresas.SelectedValue);
        if (nIdEmpresa > 0)
        {
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            DataTable dtSucursales = new clsOperacionSucursales().fnObtenerSucursales(nIdEmpresa, usuarioActivo.id_usuario);
            if (dtSucursales.Rows.Count > 0)
            {
                ddlSucursales.DataSource = dtSucursales;
                ddlSucursales.DataValueField = "id_sucursal";
                ddlSucursales.DataTextField = "nombre";
                ddlSucursales.DataBind();

            }
            else
            {
                ddlSucursales.Items.Add(new ListItem(Resources.resCorpusCFDIEs.lblFacultadesNoAsignadas, "0"));
                ddlSucursales.DataBind();
                ddlSucursales.Enabled = false;
                btnEditar.Enabled = false;
                btnBorrar.Enabled = false;
            }
        }
    }

    protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarMunicipios();
    }

    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarEstados();
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
}
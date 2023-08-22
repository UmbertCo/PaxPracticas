using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class Operacion_Proveedores_webOperacionProveedores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            InitializeCulture();
            clsInicioSesionUsuario sesUsuario = clsComun.fnUsuarioEnSesion();
            clsOperacionUsuarios oOpUsuarios = new clsOperacionUsuarios();
            //SqlDataReader sdrInfo = gOp.fnObtenerDatosUsuario();
            fnActivarUsuarioCaptura(false);
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
                    btnNuevo.Visible = false;
                    btnBorrar.Visible = false;
                }
            }
            fnLlenarProveedores();
            fnCargarPaises();
            fnCargarEstados();
            fnCargarMunicipios();
            fnCargarEmpresas();
            fnActivarCampos(false);
            btnNCancelar.Enabled = false;
            ViewState["editar"] = 0;
        }

    }

    private void fnActivarUsuarioCaptura(bool activo)
    {
        txtUsuario.Visible = activo;
        txtPassword.Visible = activo;
        txtConfirmarPassword.Visible = activo;
        regxNueva.Enabled = activo;
        PasswordRequired.Enabled = activo;
        regExPassword.Enabled = activo;
        ConfirmPasswordRequired.Enabled = activo;
        PasswordCompare.Enabled = activo;
        lblUsuario.Visible = activo;
        lblPassword.Visible = activo;
        lblConfirmarPassword.Visible = activo;
        
        ltrDatosUsuario.Visible = activo;
    }


    private void fnActivarCampos(bool activo)
    {
        txtNombre.Enabled = activo;
        txtContacto.Enabled = activo;
        txtCalle.Enabled = activo;
        txtLocalidad.Enabled = activo;
        txtColonia.Enabled = activo;
        txtNoExterior.Enabled = activo;
        txtNoInterior.Enabled = activo;
        txtCodigoPostal.Enabled = activo;
        txtCorreo.Enabled = activo;
        txtTelefono.Enabled = activo;
        ddlPais.Enabled = activo;
        ddlEstado.Enabled = activo;
        ddlMunicipio.Enabled = activo;
        txtUsuario.Enabled = activo;
        txtPassword.Enabled = activo;
        txtConfirmarPassword.Enabled = activo;
        gvSucursales.Enabled = activo;
    }

    private void fnLimpiarCampos()
    {
        txtNombre.Text = "";
        txtContacto.Text = "";
        txtCalle.Text = "";
        txtLocalidad.Text = "";
        txtColonia.Text = "";
        txtNoExterior.Text = "";
        txtNoInterior.Text = "";
        txtCodigoPostal.Text = "";
        txtCorreo.Text = "";
        txtTelefono.Text = "";
        ddlPais.SelectedIndex = 0;
        ddlEstado.SelectedIndex = 0;
        ddlMunicipio.SelectedIndex = 0;
        txtUsuario.Text = "";
        txtPassword.Text = "";
        txtConfirmarPassword.Text = "";
        foreach (GridViewRow renglon in gvSucursales.Rows)
        {
            CheckBox CbCan;

            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

            CbCan.Checked = false;
        }
    }
    protected void gvProveedores_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void gvProveedores_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void fnLlenarProveedores()
    {

        DataTable dtProveedores = new DataTable();
        clsInicioSesionUsuario sesUsuario = clsComun.fnUsuarioEnSesion();
        if (!new clsOperacionUsuarios().fnVerificarUsuarioPerfil(sesUsuario.id_usuario, "proveedor"))
        {
            dtProveedores = new clsOperacionProveedores().fnObtenerProveedores();
        }
        else
        {
            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            dtProveedores = new clsOperacionProveedores().fnObtenerProveedor(usuarioActivo.id_usuario);
        }
        gvProveedores.DataSource = dtProveedores;
        gvProveedores.DataBind();
    }

    protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarMunicipios();
    }

    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarEstados();
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
        catch (Exception ex)
        {
            //Error genérico

        }

    }

    protected void btnGuardarEmpresa_Click(object sender, EventArgs e)
    {
        
        if (string.IsNullOrEmpty(txtNombre.Text)
            || string.IsNullOrEmpty(txtContacto.Text)
            || string.IsNullOrEmpty(txtCalle.Text)
            || string.IsNullOrEmpty(txtColonia.Text)

            )
        {
            clsComun.fnMostrarMensaje(this, "Faltan datos");
            return;
        }

        if (!string.IsNullOrEmpty(txtCodigoPostal.Text))
        {
            try
            {
                Convert.ToInt32(txtCodigoPostal.Text);
            }
            catch (Exception ex)
            {
                clsComun.fnMostrarMensaje(this, "El Código postal debe ser numérico");
                txtCodigoPostal.Focus();
                return;
            }
            if (txtCodigoPostal.Text.Length != 5)
            {
                clsComun.fnMostrarMensaje(this, "El Código postal debe ser de 5 dígitos");
                txtCodigoPostal.Focus();
                return;
            }
        }

        string stxtUsuario = string.Empty;
        string sPassword = string.Empty;
        string sConfirmarPassword = string.Empty;

        if (Convert.ToBoolean(ViewState["altaUsuario"]) == true)
        {
            stxtUsuario = txtUsuario.Text.Trim();
            sPassword = txtPassword.Text.Trim();
            sConfirmarPassword = txtConfirmarPassword.Text.Trim();
            if (txtUsuario.Enabled)
            {
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    if (sPassword == sConfirmarPassword)
                    {
                        //Validacion del lado del servidor
                        if (!clsComun.fnValidaExpresion(stxtUsuario, @"(?=^.{8,}$).*$"))
                        {
                            //clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtUsuario);
                            //return;
                        }
                    }
                    else
                    {
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valConNewConf);
                        //return;
                    }
                }
                else
                {
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valConfirmaNueva);
                    //return;
                }
                if (new clsInicioSesionSolicitudReg().buscarClaveExistente(stxtUsuario) > 0)
                {
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);
                    //return;
                }
            }
        }

        int nIdMunicipio = Convert.ToInt32(ddlMunicipio.SelectedValue);


        int editar = Convert.ToInt32(ViewState["editar"]);
        if (editar == 0)
        {

            if (new clsOperacionProveedores().fnNombreExiste(txtNombre.Text) > 0)
            {
                clsComun.fnMostrarMensaje(this, "El proveedor ya está registrado");
                return;
            }
            try
            {
                int nIdPerfil = clsComun.fnObtenerIdPerfil("proveedor");
                int nIdUsuario = 0;
                if (nIdPerfil > 0)
                {
                    if (Convert.ToBoolean(ViewState["altaUsuario"]) == true)
                    {
                        nIdUsuario = new clsOperacionUsuarios().fnRegistroContribuyente(stxtUsuario, txtCorreo.Text, Utilerias.Encriptacion.Base64.EncriptarBase64(sPassword), "1", "C", nIdPerfil, 0, "A");
                    }
                    if (nIdUsuario > 0)
                    {
                        int nIdProveedor = new clsOperacionProveedores().fnGuardarProveedor(txtNombre.Text, txtContacto.Text, nIdMunicipio, txtLocalidad.Text,
                        txtColonia.Text, txtCalle.Text, txtNoExterior.Text, txtNoInterior.Text, txtCodigoPostal.Text, txtCorreo.Text, txtTelefono.Text,
                        nIdUsuario);

                        if (nIdProveedor > 0)
                        {
                            foreach (GridViewRow renglon in gvSucursales.Rows)
                            {
                                CheckBox CbCan;

                                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                                if (CbCan.Checked)
                                {
                                    Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                                    int nIdSucursal = Convert.ToInt32(sIdSucursal.Text);
                                    new clsOperacionProveedores().fnProveedorSucursalRel(nIdProveedor, nIdSucursal);
                                }
                            }
                            //StringBuilder strMensaje = new StringBuilder();
                            //strMensaje.Append("<table>");
                            //strMensaje.Append("<tr><td colspan='2'><b>A nuestro proveedor: " + txtNombre.Text + "</b></td></tr>");
                            //strMensaje.Append("<tr><td colspan='2'>Se ha registrado en nuestro portal.</td></tr>");
                            //strMensaje.Append("<tr><td colspan='2'>Para acompletar el registro, inicie sesión con los siguientes datos.</td></tr>");
                            //strMensaje.Append("<tr><td><b>Usuario:</b></td><td>" + txtUsuario.Text + "</td></tr>");
                            //strMensaje.Append("<tr><td><b>Contraseña temporal:</b></td><td>" + sPassword + "</td></tr>");
                            //strMensaje.Append("</table>");
                            ////clsComun.fnMostrarMensaje(this, "Ahora podrá ingresar con sus datos.");
                            //new clsGeneraEMAIL().EnviarCorreo(txtCorreo.Text, "Registro de proveedor.", strMensaje.ToString());
                            fnActivarCampos(false);
                            fnLimpiarCampos();
                            fnActivarBotones(true);
                            gvProveedores.Enabled = true;
                            fnLlenarProveedores();
                            clsComun.fnMostrarMensaje(this, "Proveedor guardado con éxito");
                        }
                        else
                        {
                            clsComun.fnMostrarMensaje(this, "No se pudo guardar el proveedor");
                        }

                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, "No se pudo guardar el usuario");
                    }

                }
                else
                {
                    clsComun.fnMostrarMensaje(this, "No se encontró el perfil 'Proveedor'");
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {


            }
        }
        else if (editar == 1)
        {

            try
            {
                GridViewRow gvrProveedor = gvProveedores.Rows[gvProveedores.SelectedIndex];
                int nIdProveedor = Convert.ToInt32(((Label)gvrProveedor.FindControl("lblGvIdProveedor")).Text);
                if (nIdProveedor > 0)
                {
                    //Verifica que el nombre de proveedor no exista
                    int nIdProvNombre = new clsOperacionProveedores().fnNombreExiste(txtNombre.Text);
                    if (nIdProvNombre > 0)
                    {
                        if (nIdProvNombre != nIdProveedor)
                        {
                            clsComun.fnMostrarMensaje(this, "El proveedor ya está registrado");
                            return;
                        }
                    }
                    nIdProveedor = new clsOperacionProveedores().fnEditarProveedor(nIdProveedor, txtNombre.Text, txtContacto.Text, nIdMunicipio, txtLocalidad.Text,
                            txtColonia.Text, txtCalle.Text, txtNoExterior.Text, txtNoInterior.Text, txtCodigoPostal.Text, txtCorreo.Text, txtTelefono.Text);
                    if (nIdProveedor > 0)
                    {
                        new clsOperacionProveedores().fnEliminarProveedorSucursalRel(nIdProveedor);
                        foreach (GridViewRow renglon in gvSucursales.Rows)
                        {
                            CheckBox CbCan;

                            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                            if (CbCan.Checked)
                            {
                                Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                                int nIdSucursal = Convert.ToInt32(sIdSucursal.Text);
                                new clsOperacionProveedores().fnProveedorSucursalRel(nIdProveedor, nIdSucursal);
                            }
                        }
                        fnActivarCampos(false);
                        fnLimpiarCampos();
                        fnActivarBotones(true);
                        gvProveedores.Enabled = true;
                        fnLlenarProveedores();
                        clsComun.fnMostrarMensaje(this, "Datos Actualizados");
                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, "No se pudo editar el proveedor");
                    }
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, "No está seleccionado ningún proveedor");
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                ViewState["editar"] = 0;
            }
        }
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        ViewState["altaUsuario"] = true;
        fnActivarBotones(false);
        gvProveedores.Enabled = false;
        fnLimpiarCampos();
        fnActivarCampos(true);
        txtNombre.Focus();
        fnActivarUsuarioCaptura(true);
   
    }

    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        try
        {
            int nIdProveedor = Convert.ToInt32(((Label)gvProveedores.SelectedRow.FindControl("lblGvIdProveedor")).Text);
            new clsOperacionProveedores().fnEliminarProveedor(nIdProveedor);
            fnLlenarProveedores();
            clsComun.fnMostrarMensaje(this, "Proveedor Eliminado");
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        ViewState["altaUsuario"] = false;
        if (gvProveedores.SelectedIndex >= 0)
        {
            fnActivarUsuarioCaptura(false);
            
            int nIdProveedor = Convert.ToInt32(((Label)gvProveedores.SelectedRow.FindControl("lblGvIdProveedor")).Text);
            DataTable dtProvs = new clsOperacionProveedores().fnObtenerDatosProveedor(nIdProveedor);
            DataRow drProveedor = dtProvs.Rows[0];

            txtNombre.Text = drProveedor["nombre"].ToString();
            txtContacto.Text = drProveedor["contacto"].ToString();
            txtLocalidad.Text = drProveedor["localidad"].ToString();
            txtColonia.Text = drProveedor["colonia"].ToString();
            txtCalle.Text = drProveedor["calle"].ToString();
            txtNoExterior.Text = drProveedor["no_exterior"].ToString();
            txtNoInterior.Text = drProveedor["no_interior"].ToString();
            txtCodigoPostal.Text = drProveedor["codigo_postal"].ToString();
            txtCorreo.Text = drProveedor["email"].ToString();
            txtTelefono.Text = drProveedor["telefono"].ToString();
           

            ddlPais.SelectedIndex = ddlPais.Items.IndexOf(ddlPais.Items.FindByText(drProveedor["pais"].ToString()));
            ddlPais_SelectedIndexChanged(null, null);
            ddlEstado.SelectedIndex = ddlEstado.Items.IndexOf(ddlEstado.Items.FindByText(drProveedor["estado"].ToString()));
            ddlEstado_SelectedIndexChanged(null, null);
            ddlMunicipio.SelectedIndex = ddlMunicipio.Items.IndexOf(ddlMunicipio.Items.FindByText(drProveedor["municipio"].ToString()));

            DataTable dtSucursales = new clsOperacionProveedores().fnObtenerSucursalesProveedor(nIdProveedor);

            foreach (GridViewRow renglon in gvSucursales.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                CbCan.Checked = false;
                Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                DataRow[] GridMod;
                GridMod = dtSucursales.Select("id_sucursal= " + Convert.ToInt32(sIdSucursal.Text));
                if (!(GridMod.GetUpperBound(0) == -1))
                {
                    for (int t = 0; t <= GridMod.GetUpperBound(0); t++)
                    {
                        for (int j = 0; j <= GridMod[t].ItemArray.GetUpperBound(0); j++)
                        {
                            CbCan.Checked = true;
                        }
                    }

                }
            }
            fnActivarCampos(true);
            fnActivarBotones(false);
           
           // txtUsuario.Enabled = false;
           // txtPassword.Enabled = false;
           // txtConfirmarPassword.Enabled = false;
            gvProveedores.Enabled = false;
            ViewState["editar"] = 1;
        }
    }

    private void fnActivarBotones(bool activo)
    {
        btnNuevo.Enabled = activo;
        btnEditar.Enabled = activo;
        btnBorrar.Enabled = activo;
        btnNCancelar.Enabled = !activo;
    }
    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        fnActivarUsuarioCaptura(false);
        fnActivarBotones(true);
        fnActivarCampos(false);
        fnLimpiarCampos();
        gvProveedores.Enabled = true;
    }
}
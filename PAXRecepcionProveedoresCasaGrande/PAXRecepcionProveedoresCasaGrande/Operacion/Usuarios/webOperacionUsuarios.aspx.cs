using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Globalization;
using System.Web.UI.HtmlControls;

public partial class Operacion_Usuarios_webOperacionUsuarios : System.Web.UI.Page
{
    private clsOperacionUsuarios gOp;
    DataTable tblEstrucutra;
    TreeView tree;
    public string SeleccioneUnValor = "(" + Resources.resCorpusCFDIEs.varSeleccionePerfil + ")";
    clsInicioSesionUsuario datosUsuario;
    public static string scrollPos = String.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            scrollPos = ((HtmlInputHidden)scrollPos2).ClientID.ToString();
            if (!IsPostBack)
            {
                tblEstrucutra = new DataTable();
                fnCargaPerfiles();
                clsComun.fnPonerTitulo(this);
                fnCargarEmpresas();
                ViewState["Editar"] = 0;
                btnBorrar.OnClientClick = "return confirm('" + Resources.resCorpusCFDIEs.varBorrarNodo + "');";
                fnCargarModulos();

                gOp = new clsOperacionUsuarios();

                try
                {
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
                        ViewState["idEstructuraPrincipal"] = nId_Usuario;
                        //tblEstrucutra = gOp.fnObtenerEstructura(nId_Usuario.ToString());
                        ViewState["paginado"] = 1;
                        tblEstrucutra = fnObtenerTablaUsuarios(1);
                        fnCrearNodos(0, null, tblEstrucutra);
                        trvEstructura.Nodes[0].Selected = true;
                        lblSelVal.Text = trvEstructura.Nodes[0].Text;
                        hdnSel.Value = trvEstructura.Nodes[0].Text;
                        hdnValuePath.Value = trvEstructura.Nodes[0].ValuePath;
                        hdnSelVal.Value = trvEstructura.Nodes[0].Value;

                    }


                    //txtNombre.Enabled = false;
                    txtCorreo.Enabled = false;
                    txtUsuario.Enabled = false;
                    ddlPerfil.Enabled = false;
                    txtPassword.Enabled = false;
                    txtConfirmarPassword.Enabled = false;
                    //ddlSucursales.Enabled = false;
                    GrvModulos.Enabled = false;
                    txtNombreNodo.Enabled = false;
                    btnAgregar.Enabled = false;
                    gvSucursales.Enabled = false;
                }
                catch (SqlException ex)
                {
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                }
                catch (Exception ex)
                {
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                }
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/webGlobalError.aspx");
        }
    }

    /// <summary>
    /// Llena el grid que contiene los modulos para asignarlos al nuevo perfil
    /// </summary>
    private void fnCargarModulos()
    {

        try
        {
            DataTable dtModulos = new clsOperacionUsuarios().fnObtenerModulos();
            gvModulosNuevoPerfil.DataSource = dtModulos;
            gvModulosNuevoPerfil.DataBind();
            gvModulosNuevoPerfil.Visible = true;

            //foreach (GridViewRow renglon in gvModulosNuevoPerfil.Rows)
            //{
            //    CheckBox CbCan;
            //    CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
            //    CbCan.Checked = true;

            //}
        }
        catch (Exception ex)
        {
            gvModulosNuevoPerfil.DataSource = null;
            gvModulosNuevoPerfil.DataBind();
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }


    private void fnCrearNodos(int indicePadre, TreeNode nodePadre, DataTable tblEstructura)
    {
        // Crear un DataView con los Nodos que dependen del Nodo padre pasado como parámetro.
        DataView dataViewHijos = new DataView(tblEstructura);
        dataViewHijos.RowFilter = tblEstructura.Columns["id_usuario_alta"].ColumnName + " = " + indicePadre;

        // Agregar al TreeView los nodos Hijos que se han obtenido en el DataView.
        foreach (DataRowView dataRowCurrent in dataViewHijos)
        {
            TreeNode nuevoNodo = new TreeNode();
            nuevoNodo.Text = dataRowCurrent["clave_usuario"].ToString().Trim();
            nuevoNodo.Value = dataRowCurrent["id_usuario"].ToString().Trim();

            // si el parámetro nodoPadre es nulo es porque es la primera llamada, son los Nodos
            // del primer nivel que no dependen de otro nodo.
            if (nodePadre == null)
            {
                ViewState["nodoPadre"] = dataRowCurrent["id_usuario"].ToString().Trim();
                trvEstructura.Nodes.Add(nuevoNodo);
            }
            // se añade el nuevo nodo al nodo padre.
            else
            {
                //nuevoNodo.ShowCheckBox = true;
                nodePadre.ChildNodes.Add(nuevoNodo);
            }

            // Llamada recurrente al mismo método para agregar los Hijos del Nodo recién agregado.

            fnCrearNodos(Int32.Parse(dataRowCurrent["id_usuario"].ToString()), nuevoNodo, tblEstructura);
        }
    }

    private void fnCargaPerfiles()
    {
        try
        {
            clsOperacionUsuarios gUsu = new clsOperacionUsuarios();
            DataTable dtPerfiles = new DataTable();
            dtPerfiles = gUsu.fnCargaPerfiles();
            ddlPerfil.DataSource = dtPerfiles;
            ddlPerfil.DataTextField = "desc_perfil";
            ddlPerfil.DataValueField = "id_perfil";
            ddlPerfil.DataBind();
            ddlPerfilEditar.DataSource = dtPerfiles;
            ddlPerfilEditar.DataTextField = "desc_perfil";
            ddlPerfilEditar.DataValueField = "id_perfil";
            ddlPerfilEditar.DataBind();

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    private void fnCargarSucursales(int nIdEmpresa)
    {
        try
        {
            //DataTable tblEstructura = new DataTable();
            //datosUsuario = clsComun.fnUsuarioEnSesion();
            //tblEstructura = fnObtenerSucursales(nIdEmpresa);
            //ddlSucursales.DataSource = tblEstructura;
            //ddlSucursales.DataValueField = "id_sucursal";
            //ddlSucursales.DataTextField = "nombre";
            //ViewState["tblEstructura"] = tblEstructura;
            //ddlSucursales.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

        }
        catch
        {
            //referencia nula
        }
    }

    private void fnCargarEmpresas()
    {
        try
        {
            DataTable dtSucursales = new DataTable();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            dtSucursales = new clsOperacionSucursales().fnObtenerSucursalesEmpresas();
            gvSucursales.DataSource = dtSucursales;
            gvSucursales.DataBind();
            gvSucursales.Visible = true;
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

        }
        catch
        {
            //referencia nula
        }
    }



    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        //Buscar Clave existente
        clsInicioSesionSolicitudReg registro = new clsInicioSesionSolicitudReg();

        if (registro.buscarUsuario(txtNombreNodo.Text).Rows.Count == 0)
        {

            if (!string.IsNullOrEmpty(txtNombreNodo.Text) && !string.IsNullOrEmpty(trvEstructura.Nodes[0].Text))
            {
                btnNuevo.Enabled = false;
                btnBorrar.Enabled = false;
                btnEditar.Enabled = false;
                //btnCancelar.Enabled = true;
                trvEstructura.Enabled = false;
                //txtNombre.Enabled = true;
                txtCorreo.Enabled = true;
                txtUsuario.Enabled = true;
                ddlPerfil.Enabled = true;
                gvSucursales.Enabled = true;
                txtPassword.Enabled = true;
                txtConfirmarPassword.Enabled = true;
                //ddlSucursales.Enabled = true;
                GrvModulos.Enabled = true;
                txtNombreNodo.Enabled = false;
                btnAgregar.Enabled = false;
                rblUsuarios.Enabled = false;
                btnGuardarActualizar.Enabled = true;
                btnAnterior.Enabled = false;
                btnSiguiente.Enabled = false;
                btnBuscarUsuarios.Enabled = false;
                txtBuscarUsuarios.Enabled = false;
                ddlLineasPagina.Enabled = false;
                //Crea nodos Hijos

                List<TreeNode> lista = Find(trvEstructura.Nodes[0].Text);

                foreach (TreeNode item in lista)
                {

                    if (item.ValuePath == trvEstructura.Nodes[0].Value)
                    {

                        item.ChildNodes.Add(new TreeNode(txtNombreNodo.Text));
                        txtUsuario.Text = txtNombreNodo.Text;
                        trvEstructura.ExpandAll();
                        fnBorrarNodoSeleccionado();
                        btnBorrar.Enabled = false;
                        btnEditar.Enabled = false;
                        btnAgregar.Enabled = false;
                        trvEstructura.Enabled = false;
                        gvSucursales.Enabled = true;
                        ViewState["Editar"] = 0;
                        return;
                    }
                }
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAgregaNodoUsuarios);
            }
        }
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);
        }
    }

    private void fnBorrarNodoSeleccionado()
    {
        lblSelVal.Text = Resources.resCorpusCFDIEs.lblNinguno;
        lblProvUsuario.Text = Resources.resCorpusCFDIEs.lblNinguno;
        hdnSel.Value = string.Empty;
        hdnValuePath.Value = string.Empty;
        txtNombreNodo.Text = string.Empty;
    }

    private List<TreeNode> Find(string param)
    {
        List<TreeNode> list = new List<TreeNode>();

        foreach (TreeNode item in trvEstructura.Nodes)
        {
            if (item.Text.Contains(param))
                list.Add(item);

            if (item.ChildNodes.Count > 0)
                list.AddRange(FindChild(item.ChildNodes, param));
        }

        return list;
    }
    private List<TreeNode> FindChild(TreeNodeCollection nodes, string param)
    {
        List<TreeNode> list = new List<TreeNode>();

        foreach (TreeNode item in nodes)
        {
            if (item.Text.Contains(param))
                list.Add(item);

            if (item.ChildNodes.Count > 0)
                list.AddRange(FindChild(item.ChildNodes, param));
        }

        return list;
    }

    protected void btnGuardarActualizar_Click(object sender, EventArgs e)
    {
        //validaciones
        if (string.IsNullOrEmpty(txtUsuario.Text)
            //|| string.IsNullOrEmpty(txtNombre.Text)
            || string.IsNullOrEmpty(txtCorreo.Text)
            //|| string.IsNullOrEmpty(ddlPerfil.SelectedValue)
            )
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        //Asignacion de variables
        string sOrigen = string.Empty;
        string strMensaje = string.Empty;
        //string stxtNombre = string.Empty;
        string stxtUsuario = string.Empty;
        string stxtCorreo = string.Empty;
        string sPassword = string.Empty;
        string sConfirmarPassword = string.Empty;
        int id_perfil = 0;

        //stxtNombre = txtNombre.Text;
        stxtUsuario = txtUsuario.Text;
        stxtCorreo = txtCorreo.Text;
        sPassword = txtPassword.Text;
        sConfirmarPassword = txtConfirmarPassword.Text;

        //id_sucursal = Convert.ToInt32(ddlSucursales.SelectedValue);

        try
        {

            //Validacion del lado del servidor
            if (!clsComun.fnValidaExpresion(stxtUsuario, @"(?=^.{8,}$).*$"))
            {
                clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtUsuario);
                return;
            }

            if (!clsComun.fnValidaExpresion(stxtCorreo, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtCorreo);
                return;
            }



            //Preparar envio de correo
            clsGeneraEMAIL sendEmail = new clsGeneraEMAIL();
            clsGeneraLlaves llaves = new clsGeneraLlaves();
            clsInicioSesionSolicitudReg registro = new clsInicioSesionSolicitudReg();
            gOp = new clsOperacionUsuarios();



            //Obtenemos el ID de la fila seleccionada
            string id_estructura = hdnSelVal.Value;

            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "GenerarLlavesContribuyentes" + "|" + "Crea la contraseña al nuevo contribuyente.");

            int Editar = Convert.ToInt32(ViewState["Editar"]);
            if (Editar == 0)
            {
                if (!string.IsNullOrEmpty(ddlPerfil.SelectedValue))
                    id_perfil = Convert.ToInt32(ddlPerfil.SelectedValue);
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    if (sPassword == sConfirmarPassword)
                    {
                        //Validacion del lado del servidor
                        if (!clsComun.fnValidaExpresion(stxtUsuario, @"(?=^.{8,}$).*$"))
                        {
                            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtUsuario);
                            return;
                        }
                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valConNewConf);
                        return;
                    }
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valConfirmaNueva);
                    return;
                }
                if (id_perfil <= 0)
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varSeleccionePerfil);
                    return;
                }
                //Buscar Clave existente
                clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "buscarClaveExistente" + "|" + "Revisa que no exista el usuario.");
                if (registro.buscarClaveExistente(stxtUsuario.Trim()) == 0)
                {
                    //Guardar valores en BD
                    clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "solicitudRegistroContribuyente" + "|" + "Registra el nuevo contribuyente" + "|" + stxtUsuario);
                    sPassword = txtPassword.Text;

                    int idUsuario = gOp.fnRegistroContribuyente(stxtUsuario, stxtCorreo, Utilerias.Encriptacion.Base64.EncriptarBase64(sPassword), trvEstructura.Nodes[0].Value, "C", id_perfil, 0, "A");

                    if (idUsuario != 0)
                    {

                        datosUsuario = clsComun.fnUsuarioEnSesion();




                        foreach (GridViewRow renglon in gvSucursales.Rows)
                        {
                            CheckBox CbCan;

                            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                            if (CbCan.Checked)
                            {
                                Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                                int nIdSucursal = Convert.ToInt32(sIdSucursal.Text);
                                gOp.fnInsertaRelacionUsuarioSucursal(idUsuario, nIdSucursal);
                            }
                        }



                        fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), true);

                        fnBorrarNodoSeleccionado();
                        //tblEstrucutra = gOp.fnObtenerEstructura(ViewState["idEstructuraPrincipal"].ToString());
                        int pagina = Convert.ToInt32(ViewState["paginado"]);
                        tblEstrucutra = fnObtenerTablaUsuarios(pagina);
                        trvEstructura.Nodes.Clear();
                        fnCrearNodos(0, null, tblEstrucutra);
                        trvEstructura.ExpandAll();



                        fnCargaPerfiles();
                        GrvModulos.DataSource = null;
                        GrvModulos.DataBind();
                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);
                    }
                }
                else
                {

                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);

                }
            }
            else
            {
                //Obtenemos el ID de la fila seleccionada
                int idUsuario = Convert.ToInt32(hdnSelVal.Value);
                gOp.fnEliminarModulosUsuario(idUsuario);
                gOp.fnEliminarSucursalUsuario(idUsuario);
                int nIdPerfil = 0;
                if (ddlPerfil.Visible)
                {
                    nIdPerfil = Convert.ToInt32(ddlPerfil.SelectedValue);
                }
                gOp.fnActualizaUsuarioInfo(idUsuario, stxtUsuario, stxtCorreo, sPassword, nIdPerfil);


                foreach (GridViewRow renglon in gvSucursales.Rows)
                {
                    CheckBox CbCan;

                    CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                    if (CbCan.Checked)
                    {
                        Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                        int nIdSucursal = Convert.ToInt32(sIdSucursal.Text);
                        gOp.fnInsertaRelacionUsuarioSucursal(idUsuario, nIdSucursal);
                    }
                }
            }

            btnBorrar.Enabled = true;
            btnEditar.Enabled = true;
            trvEstructura.Enabled = true;
            txtNombreNodo.Enabled = false;
            btnAgregar.Enabled = false;
            fnLimpiarPantalla();
            btnNuevo.Enabled = true;
            trvEstructura.Enabled = true;
            btnGuardarActualizar.Enabled = false;
            rfvPassword.Enabled = true;
            rfvConfirmarPassword.Enabled = true;
            ddlLineasPagina.Enabled = true;
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), false);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), false);
        }
        finally
        {

        }
    }

    /// <summary>
    /// Vacía todos los controles del formulario
    /// </summary>
    private void fnLimpiarPantalla()
    {
        try
        {
            //txtNombre.Enabled = false;
            txtCorreo.Enabled = false;
            txtUsuario.Enabled = false;
            ddlPerfil.SelectedIndex = 0;
            ddlPerfil_SelectedIndexChanged(null, null);
            ddlPerfil.Enabled = false;
            txtPassword.Enabled = false;
            txtConfirmarPassword.Enabled = false;
            btnAnterior.Enabled = true;
            btnSiguiente.Enabled = true;
            btnBuscarUsuarios.Enabled = true;
            txtBuscarUsuarios.Enabled = true;
            //ddlSucursales.Enabled = false;
            GrvModulos.Enabled = false;
            gvSucursales.Enabled = false;
            ddlPerfil.Visible = true;
            ddlPerfilProveedor.Visible = false;
            GrvModulos.Enabled = true;
            txtUsuario.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtConfirmarPassword.Text = string.Empty;
            clsComunCatalogo.fnLimpiarFormulario(pnlFormulario);
            foreach (GridViewRow renglon in gvSucursales.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                CbCan.Checked = false;
            }
            //ddlPais.SelectedIndex = 0;
            //ddlPais_SelectedIndexChanged(ddlPais, null);
            //ddlEstado.SelectedIndex = 0;
        }
        finally
        {
            //btnCancelar.Visible = false;
            hdIdEstructura.Value = string.Empty;
            //gdvSucursales.SelectedIndex = -1;
        }
    }

    private void fnSeleccionarMensaje(bool pbNuevo, bool pbExito)
    {
        if (pbExito)
        {
            if (pbNuevo)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionUsuarios",
                    "fnGuardarUsuario",
                    "Se agrego un usuario nuevo con los datos:",
                    //txtNombre.Text,
                     txtUsuario.Text,
                     txtCorreo.Text
                    );
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionUsuarios",
                    "fnGuardarUsuario",
                    "Se modificó el usuario con id: " + hdIdEstructura.Value + " con los datos:",
                    //txtNombre.Text,
                     txtUsuario.Text,
                     txtCorreo.Text
                    );
            }
        }
        else
        {
            if (pbNuevo)
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
        }
    }

    protected void trvEstructura_SelectedNodeChanged(object sender, EventArgs e)
    {
        tree = new TreeView();
        tree = (TreeView)sender;
        string sNombreProveedor =
            new clsOperacionProveedores().fnObtenerProveedorUsuario(Convert.ToInt32(tree.SelectedNode.Value));
        if (!string.IsNullOrEmpty(sNombreProveedor))
        {
            lblProvUsuario.Text = sNombreProveedor;
        }
        else
        {
            lblProvUsuario.Text = Resources.resCorpusCFDIEs.lblNinguno;
        }
        lblSelVal.Text = tree.SelectedNode.Text;
        hdnSel.Value = tree.SelectedNode.Text;
        hdnSelVal.Value = tree.SelectedNode.Value;
        hdnValuePath.Value = tree.SelectedNode.ValuePath;
        tree.ForeColor = Color.Red;
        txtNombreNodo.Enabled = false;
        btnAgregar.Enabled = false;
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        btnNCancelar_Click(sender, e);
        txtNombreNodo.Enabled = true;
        btnAgregar.Enabled = true;
        btnNuevo.Enabled = false;
        btnEditar.Enabled = false;
        btnBorrar.Enabled = false;
        trvEstructura.Enabled = false;
    }

    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnValuePath.Value != string.Empty)
            {

                int nfacGenerados = 0;
                List<TreeNode> lisParent = Find(hdnSel.Value);

                foreach (TreeNode item in lisParent)
                {
                    if (item.Value == ViewState["nodoPadre"].ToString())
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBoNodPrinc);
                        return;
                    }
                    else
                    {
                        try
                        {
                            gOp = new clsOperacionUsuarios();
                            //Obtenemos el ID de la fila seleccionada
                            int id_estructura = Convert.ToInt32(hdnSelVal.Value);

                            nfacGenerados = gOp.fnObtenerComprobantesUsuario(id_estructura);
                            if (nfacGenerados > 0)
                            {
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNoAgregar);
                                return;
                            }
                            bool retVal = false;

                            gOp = new clsOperacionUsuarios();
                            DataTable dtUsuario = new DataTable();
                            int idUsuario = Convert.ToInt32(trvEstructura.SelectedNode.Value);
                            dtUsuario = gOp.fnObtenerInfoBasicaUsuario(idUsuario);
                            //
                            clsInicioSesionSolicitudReg reg = new clsInicioSesionSolicitudReg();
                            retVal = reg.actualizaEstadoActual(Convert.ToString(dtUsuario.Rows[0]["clave_usuario"]), Convert.ToString(dtUsuario.Rows[0]["email"]), 'B');

                            if (retVal != false)
                            {
                                //Si se estaba editando la estructura, entonces limpiamos los datos del formulario
                                if (!string.IsNullOrEmpty(hdIdEstructura.Value) && hdIdEstructura.Value.Equals(id_estructura))
                                    fnLimpiarPantalla();

                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);

                                clsComun.fnNuevaPistaAuditoria(
                                    "webOperacionUsuarios",
                                    "btnBorrar_Click",
                                    "Se dió de baja el usuario con ID " + id_estructura
                                    );

                                RemoveNodeRecurrently(trvEstructura.Nodes, hdnValuePath.Value);
                                fnBorrarNodoSeleccionado();
                                fnLimpiarPantalla();
                            }
                            else
                                throw new Exception(Resources.resCorpusCFDIEs.varBajaFallida);
                        }
                        catch (Exception ex)
                        {
                            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
                        }
                    }
                }
                txtNombreNodo.Enabled = false;
                btnAgregar.Enabled = false;
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAgregaNodoUsuarios);
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        try
        {
            fnCargaPerfiles();
            GrvModulos.DataSource = null;
            GrvModulos.DataBind();

            if (hdnValuePath.Value != string.Empty)
            {
                btnNuevo.Enabled = false;
                btnBorrar.Enabled = false;
                btnEditar.Enabled = false;
                trvEstructura.Enabled = false;
                btnAnterior.Enabled = false;
                btnSiguiente.Enabled = false;
                btnBuscarUsuarios.Enabled = false;
                txtBuscarUsuarios.Enabled = false;
                rfvPassword.Enabled = false;
                rfvConfirmarPassword.Enabled = false;
                int nfacGenerados = 0;
                List<TreeNode> lisParent = Find(hdnSel.Value);

                foreach (TreeNode item in lisParent)
                {
                    if (item.Value == ViewState["nodoPadre"].ToString())
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEdNodPrinc);
                        btnNCancelar_Click(sender, e);
                        return;
                    }


                    gOp = new clsOperacionUsuarios();
                    //Obtenemos el ID de la fila seleccionada

                    int id_estructura = Convert.ToInt32(hdnSelVal.Value);
                    nfacGenerados = gOp.fnObtenerComprobantesUsuario(id_estructura);
                    if (nfacGenerados <= 0)
                    {
                        //txtNombre.Enabled = true;
                        txtCorreo.Enabled = true;
                        txtUsuario.Enabled = true;
                        ddlPerfil.Enabled = true;
                        //txtPassword.Enabled = true;
                        //txtConfirmarPassword.Enabled = true;
                        gvSucursales.Enabled = true;
                        //ddlSucursales.Enabled = true;
                        GrvModulos.Enabled = true;
                        rblUsuarios.Enabled = false;

                    }
                    else
                    {
                        //txtNombre.Enabled = false;
                        txtCorreo.Enabled = true;
                        txtUsuario.Enabled = false;
                        ddlPerfil.Enabled = false;
                        txtPassword.Enabled = false;
                        txtConfirmarPassword.Enabled = false;
                        gvSucursales.Enabled = true;
                        //ddlSucursales.Enabled = false;
                        GrvModulos.Enabled = false;
                        rblUsuarios.Enabled = false;
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarCorreoGeneracion);
                    }


                    if (!string.IsNullOrEmpty(hdnSel.Value))
                    {
                        DataTable tblEstructura = new DataTable();
                        //DataRow[] rowEstructura;
                        //tblEstructura = (DataTable)ViewState["tblEstructura"];
                        //rowEstructura = tblEstructura.Select("id_estructura=" + trvEstructura.SelectedNode.Value);
                        ViewState["Editar"] = 1;
                        try
                        {

                            gOp = new clsOperacionUsuarios();
                            DataTable dtUsuario = new DataTable();
                            int idUsuario = Convert.ToInt32(trvEstructura.SelectedNode.Value);
                            dtUsuario = gOp.fnObtenerInfoUsuario(idUsuario);
                            //txtNombre.Text = Convert.ToString(dtUsuario.Rows[0]["nombre"]);
                            txtUsuario.Text = Convert.ToString(dtUsuario.Rows[0]["clave_usuario"]);
                            txtCorreo.Text = Convert.ToString(dtUsuario.Rows[0]["email"]);
                            //ddlSucursales.SelectedValue = Convert.ToString(dtUsuario.Rows[0]["id_estructura"]);
                            ViewState["id_contribuyente"] = Convert.ToInt32(dtUsuario.Rows[0]["id_usuario"]);
                            if (!new clsOperacionUsuarios().fnVerificarUsuarioPerfil(idUsuario, "P"))
                            {
                                ddlPerfil.SelectedValue = Convert.ToString(dtUsuario.Rows[0]["id_perfil"]);
                                ddlPerfil_SelectedIndexChanged(sender, null);
                            }
                            else
                            {
                                ddlPerfil.Visible = false;
                                ddlPerfilProveedor.Visible = true;
                                gvSucursales.Enabled = false;
                                GrvModulos.Enabled = false;

                            }



                            DataTable dtSucursalesUsuario = new clsOperacionSucursales().fnObtenerSucursalesUsuario(id_estructura);

                            foreach (GridViewRow renglon in gvSucursales.Rows)
                            {
                                CheckBox CbCan;

                                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                                CbCan.Checked = false;
                                Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                                DataRow[] GridMod;
                                GridMod = dtSucursalesUsuario.Select("id_sucursal= " + Convert.ToInt32(sIdSucursal.Text));
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
                                //btnCancelar.Visible = true;
                            }
                            btnAgregar.Enabled = false;
                            txtNombreNodo.Enabled = false;
                            btnGuardarActualizar.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                        }
                    }
                    else
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAgregaNodoUsuarios);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            gOp = new clsOperacionUsuarios();
            fnBorrarNodoSeleccionado();
            //tblEstrucutra = gOp.fnObtenerEstructura(ViewState["idEstructuraPrincipal"].ToString());
            int pagina = Convert.ToInt32(ViewState["paginado"]);
            tblEstrucutra = fnObtenerTablaUsuarios(pagina);
            trvEstructura.Nodes.Clear();
            fnCrearNodos(0, null, tblEstrucutra);
            trvEstructura.ExpandAll();

            fnLimpiarPantalla();
            btnBorrar.Enabled = true;
            btnEditar.Enabled = true;
            //btnAgregar.Enabled = true;
            if (rblUsuarios.SelectedIndex != 2)
            {
                btnNuevo.Enabled = true;
            }
            trvEstructura.Enabled = true;
            rblUsuarios.Enabled = true;
            GrvModulos.DataSource = null;
            GrvModulos.DataBind();
            ddlPerfil.SelectedValue = "";
            ddlLineasPagina.Enabled = true;
            //ddlSucursales.SelectedIndex = 0;
            btnAgregar.Enabled = false;
            txtNombreNodo.Enabled = false;
            btnGuardarActualizar.Enabled = false;
            rfvPassword.Enabled = true;
            rfvConfirmarPassword.Enabled = true;
            ddlLineasPagina.Enabled = true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        fnLimpiarPantalla();
    }

    protected void ddlPerfil_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }

    protected void AgregaOpcionSeleccione(System.Object sender, System.EventArgs e)
    {
        ((DropDownList)sender).Items.Insert(0, new ListItem(SeleccioneUnValor, string.Empty));
    }

    protected void ddlPerfil_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gOp = new clsOperacionUsuarios();
            DataTable dtModulosHijo = new DataTable();
            dtModulosHijo = gOp.fnSeleccionaModulosHijo(Convert.ToInt32(ddlPerfil.SelectedValue));

            GrvModulos.DataSource = dtModulosHijo;
            GrvModulos.DataBind();
            GrvModulos.Visible = true;


        }
        catch (Exception ex)
        {
            GrvModulos.DataSource = null;
            GrvModulos.DataBind();
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void GrvModulos_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void GrvModulos_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void ddlMatriz_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarSucursales(Convert.ToInt32(((DropDownList)sender).SelectedValue));
    }

    /// <summary>
    /// Trae la lista de sucursales activas asignadas al usuario y las carga en el GridView
    /// </summary>
    //private void fnCargarSucursales()
    //{
    //    try
    //    {
    //        DataTable tblEstructura = new DataTable();
    //        datosUsuario = clsComun.fnUsuarioEnSesion();
    //        tblEstructura = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true);
    //        ddlSucursales.DataSource = tblEstructura;
    //        ViewState["tblEstructura"] = tblEstructura;
    //        ddlSucursales.DataBind();
    //    }
    //    catch (SqlException ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

    //    }
    //    catch
    //    {
    //        //referencia nula
    //    }
    //}

    //protected void btnGuardaEmpresa_Click(object sender, EventArgs e)
    //{
    //    //if (!string.IsNullOrWhiteSpace(txtNombreEmpresa.Text))
    //    //{
    //    //    if (gOp == null)
    //    //        gOp = new clsOperacionUsuarios();

    //    //    gOp.fnInsertaSucursal(txtNombreEmpresa.Text);
    //    //    fnCargarEmpresas();
    //    //}
    //}

    //protected void btnGuardaSucursal_Click(object sender, EventArgs e)
    //{
    //    //if (!string.IsNullOrWhiteSpace(txtNombreSucursal.Text))
    //    //{
    //    //    if (gOp == null)
    //    //        gOp = new clsOperacionUsuarios();

    //    //    gOp.fnInsertaSucursal(txtNombreSucursal.Text, Convert.ToInt32(ddlMatriz.SelectedValue));
    //    //    fnCargarSucursales(Convert.ToInt32(ddlMatriz.SelectedValue));
    //    //}
    //}

    private void RemoveNodeRecurrently(TreeNodeCollection childNodeCollection, string text)
    {
        foreach (TreeNode childNode in childNodeCollection)
        {
            if (childNode.ChildNodes.Count > 0)
                RemoveNodeRecurrently(childNode.ChildNodes, text);

            if (childNode.ValuePath == text)
            {
                TreeNode parentNode = childNode.Parent;
                parentNode.ChildNodes.Remove(childNode);

                break;
            }
        }
    }

    protected void btnGuardarNuevoPerfil_Click(object sender, EventArgs e)
    {

        List<int> nIdModulosSel = new List<int>();
        foreach (GridViewRow renglon in gvModulosNuevoPerfil.Rows)
        {
            CheckBox CbCan;

            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

            if (CbCan.Checked)
            {
                Label sIdModulo = ((Label)renglon.FindControl("lblidmodulo"));
                nIdModulosSel.Add(Convert.ToInt32(sIdModulo.Text));
            }
        }
        if (nIdModulosSel.Count > 0)
        {
            if (Convert.ToBoolean(ViewState["NuevoPerfil"]))
            {
                if (!string.IsNullOrEmpty(txtNuevoPerfil.Text))
                {
                    int res = new clsOperacionUsuarios().fnGuardaNuevoPerfil(txtNuevoPerfil.Text, nIdModulosSel);
                    if (res > 0)
                    {
                        fnCargaPerfiles();
                        txtNuevoPerfil.Text = string.Empty;
                        fnCargarModulos();
                    }
                    else
                    {
                        lblMensajePerfil.Text = Resources.resCorpusCFDIEs.lblPerfilExiste;
                    }
                }

                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEscribaPerfil);
                }
            }
            else
            {
                int nIdPerfil = Convert.ToInt32(ddlPerfilEditar.SelectedValue);
                new clsOperacionUsuarios().fnEditarPerfil(nIdPerfil, nIdModulosSel);
                fnCargaPerfiles();
                txtNuevoPerfil.Text = string.Empty;
                fnCargarModulos();
            }
            btnNuevoPerfil.Enabled = true;
            btnEditarPerfil.Enabled = true;
            btnBorrarPerfil.Enabled = true;
            btnGuardarNuevoPerfil.Enabled = false;
            txtNuevoPerfil.Enabled = false;
            txtNuevoPerfil.Text = string.Empty;
            ddlPerfilEditar.Enabled = true;
            gvModulosNuevoPerfil.Enabled = false;
            foreach (GridViewRow renglon in gvModulosNuevoPerfil.Rows)
            {
                CheckBox CbCan;
                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                CbCan.Checked = false;
            }
        }
        
            mdlPerfiles.Show();
    }
    private DataTable fnObtenerTablaUsuarios(int nPagina)
    {
        int nNumPagina = Convert.ToInt32(ddlLineasPagina.SelectedItem.ToString());
        DataTable dtResultado = new DataTable();
        clsInicioSesionUsuario sesUsuario = clsComun.fnUsuarioEnSesion();
        int nId_Usuario = sesUsuario.id_usuario;
        gOp = new clsOperacionUsuarios();
        switch (rblUsuarios.SelectedIndex)
        {
            //case 0:
            //    {
            //        dtResultado = gOp.fnObtenerEstructuraPag(nId_Usuario.ToString(), nPagina, nNumPagina, txtBuscarUsuarios.Text);
            //        btnNuevo.Enabled = true;
            //        break;
            //    }
            case 0: //case 1:
                {
                    dtResultado = gOp.fnObtenerEstructuraProveedorPag(nId_Usuario.ToString(), false, nPagina, nNumPagina, txtBuscarUsuarios.Text);
                    btnNuevo.Enabled = true;
                    break;
                }
            //case 2:
            //    {
            //        dtResultado = gOp.fnObtenerEstructuraProveedorPag(nId_Usuario.ToString(), true, nPagina, nNumPagina, txtBuscarUsuarios.Text);
            //        btnNuevo.Enabled = false;
            //        break;
            //    }
        }
        if (nPagina <= 1)
        {
            btnAnterior.Visible = false;
        }
        else
        {
            btnAnterior.Visible = true;
        }
        if (dtResultado.Rows.Count < nNumPagina)
        {
            btnSiguiente.Visible = false;
        }
        else
        {
            btnSiguiente.Visible = true;
        }
        return dtResultado;
    }
    protected void rblUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clsInicioSesionUsuario sesUsuario = clsComun.fnUsuarioEnSesion();
            int nId_Usuario = sesUsuario.id_usuario;
            tblEstrucutra = new DataTable();
            ViewState["paginado"] = 1;
            tblEstrucutra = fnObtenerTablaUsuarios(1);

            trvEstructura.Nodes.Clear();
            fnCrearNodos(0, null, tblEstrucutra);
            //trvEstructura.Nodes[0].Selected = true;
            trvEstructura.ExpandAll();
            btnAnterior.Visible = false;

        }
        catch (Exception ex)
        {

        }
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

    protected void btnCancelarPerfil_Click(object sender, EventArgs e)
    {
        bool mostrar = btnGuardarNuevoPerfil.Enabled;
        txtNuevoPerfil.Text = string.Empty;
        ddlPerfilEditar.SelectedIndex = 0;
        txtNuevoPerfil.Enabled = false;
        ddlPerfilEditar.Enabled = true;
        btnNuevoPerfil.Enabled = true;
        btnEditarPerfil.Enabled = true;
        btnBorrarPerfil.Enabled = true;
        btnGuardarNuevoPerfil.Enabled = false;
        lblMensajePerfil.Text = string.Empty;
        foreach (GridViewRow renglon in gvModulosNuevoPerfil.Rows)
        {
            CheckBox CbCan;
            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
            CbCan.Checked = false;
        }
        gvModulosNuevoPerfil.Enabled = false;
        if (mostrar)
        {
            mdlPerfiles.Show();
        }
        else
        {
            mdlPerfiles.Hide();
        }
    }

    protected void btnNuevoPerfil_Click(object sender, EventArgs e)
    {
        lblMensajePerfil.Text = string.Empty;
        btnEditarPerfil.Enabled = false;
        btnBorrarPerfil.Enabled = false;
        btnNuevoPerfil.Enabled = false;
        txtNuevoPerfil.Text = string.Empty;
        //fnCargaPerfiles();
        foreach (GridViewRow renglon in gvModulosNuevoPerfil.Rows)
        {
            CheckBox CbCan;
            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
            CbCan.Checked = false;
        }
        txtNuevoPerfil.Enabled = true;
        ViewState["NuevoPerfil"] = true;
        //ddlPerfil.Enabled = true;
        //gvGrvModulos.Enabled = true;
        gvModulosNuevoPerfil.Enabled = true;
        ddlPerfilEditar.Enabled = false;
        btnGuardarNuevoPerfil.Enabled = true;
        //fnCargarModulos();
        mdlPerfiles.Show();
    }

    protected void btnEditarPerfil_Click(object sender, EventArgs e)
    {

        lblMensajePerfil.Text = string.Empty;
        btnNuevoPerfil.Enabled = false;
        btnBorrarPerfil.Enabled = false;
        ddlPerfilEditar.Enabled = false;
        ViewState["NuevoPerfil"] = false;
        txtNuevoPerfil.Enabled = false;
        gvModulosNuevoPerfil.Enabled = true;
        btnGuardarNuevoPerfil.Enabled = true;
        gOp = new clsOperacionUsuarios();
        DataTable dtModulosHijo = new DataTable();
        dtModulosHijo = gOp.fnSeleccionaModulosHijo(Convert.ToInt32(ddlPerfilEditar.SelectedValue));

        //GrvModulos.DataSource = dtModulosHijo;
        //GrvModulos.DataBind();
        //GrvModulos.Visible = true;

        foreach (GridViewRow renglon in gvModulosNuevoPerfil.Rows)
        {
            CheckBox CbCan;
            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
            CbCan.Checked = false;

            Label sIdModulo = ((Label)renglon.FindControl("lblidmodulo"));
            DataRow[] GridMod;
            //GridMod = dtModulosHijo.Select();
            GridMod = dtModulosHijo.Select("id_modulo= " + Convert.ToInt32(sIdModulo.Text));
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
        mdlPerfiles.Show();

    }

    protected void btnBorrarPerfil_Click(object sender, EventArgs e)
    {

        try
        {
            int nIdPerfil = Convert.ToInt32(ddlPerfilEditar.SelectedValue);
            int nUsuarios = new clsOperacionUsuarios().fnObtenerUsuariosPerfil(nIdPerfil);

            if (nUsuarios > 0)
            {
                //linkModal_ModalPopupExtender.Show();
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarEliminarPerfil);
                lblMensajePerfil.Text = Resources.resCorpusCFDIEs.VarEliminarPerfil;
                mdlPerfiles.Show();
            }
            else
            {
                new clsOperacionUsuarios().fnEliminarPerfilModulo(nIdPerfil);
                new clsOperacionUsuarios().fnEliminaPerfil(nIdPerfil);
                fnCargaPerfiles();
                //linkModal_ModalPopupExtender.Show();
            }

        }
        catch (Exception ex)
        {
            //linkModal_ModalPopupExtender.Show();
        }

        mdlPerfiles.Show();
    }

    protected void btnBuscarUsuarios_Click(object sender, EventArgs e)
    {

        ViewState["paginado"] = 1;
        tblEstrucutra = fnObtenerTablaUsuarios(1);

        trvEstructura.Nodes.Clear();
        fnCrearNodos(0, null, tblEstrucutra);
        trvEstructura.ExpandAll();

    }
    protected void btnAnterior_Click(object sender, EventArgs e)
    {
        int pagina = Convert.ToInt32(ViewState["paginado"]);
        pagina -= 1;

        if (pagina > 0)
        {
            ViewState["paginado"] = pagina;
            //fnRealizarConsultaPaginado(pagina, 2);
            tblEstrucutra = fnObtenerTablaUsuarios(pagina);

            trvEstructura.Nodes.Clear();
            fnCrearNodos(0, null, tblEstrucutra);
            trvEstructura.ExpandAll();
            pagina -= 1;

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
        tblEstrucutra = fnObtenerTablaUsuarios(pagina);

        trvEstructura.Nodes.Clear();
        fnCrearNodos(0, null, tblEstrucutra);
        trvEstructura.ExpandAll();

    }

    //protected void ddlPerfilEditar_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    gOp = new clsOperacionUsuarios();
    //    DataTable dtModulosHijo = new DataTable();
    //    dtModulosHijo = gOp.fnSeleccionaModulosHijo(Convert.ToInt32(ddlPerfilEditar.SelectedValue));

    //    //GrvModulos.DataSource = dtModulosHijo;
    //    //GrvModulos.DataBind();
    //    //GrvModulos.Visible = true;

    //    foreach (GridViewRow renglon in gvModulosNuevoPerfil.Rows)
    //    {
    //        CheckBox CbCan;
    //        CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
    //        CbCan.Checked = false;

    //        Label sIdModulo = ((Label)renglon.FindControl("lblidmodulo"));
    //        DataRow[] GridMod;
    //        //GridMod = dtModulosHijo.Select();
    //        GridMod = dtModulosHijo.Select("id_modulo= " + Convert.ToInt32(sIdModulo.Text));
    //        if (!(GridMod.GetUpperBound(0) == -1))
    //        {
    //            for (int t = 0; t <= GridMod.GetUpperBound(0); t++)
    //            {
    //                for (int j = 0; j <= GridMod[t].ItemArray.GetUpperBound(0); j++)
    //                {
    //                    CbCan.Checked = true;
    //                }
    //            }

    //        }
    //    }
    //    mdlPerfiles.Show();
    //}
}
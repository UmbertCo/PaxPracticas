using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Pantalla para el alta, baja y cambio de los datos de sucursales para el emisor
/// </summary>
public partial class Operacion_Sucursales_webOperacionSucursales : System.Web.UI.Page
{
    private clsOperacionSucursales gDAL;
    private clsOperacionCuenta gOp;
    DataTable tblEstrucutra, tblSucursalUsu;
    clsInicioSesionUsuario datosUsuario;
    TreeView tree;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tblEstrucutra = new DataTable();

            clsComun.fnPonerTitulo(this);

            fnCargarPaises();
            fnCargarEstados(ddlPais.SelectedValue);
            //fnCargarSucursalesBorradas();

            fnCargarSucursales();

            txtNombreNodo.Enabled = false;
            btnAgregar.Enabled = false;
            fDesHabilitarDatosFiscales();

            btnBorrar.OnClientClick = "return confirm('" + Resources.resCorpusCFDIEs.varBorrarNodo + "');";

            gOp = new clsOperacionCuenta();
            gDAL = new clsOperacionSucursales();

            try
            {
                DataTable sdrInfo = gOp.fnObtenerDatosFiscales();

                if (sdrInfo != null && sdrInfo.Rows.Count > 0)
                {
                    ViewState["idEstructuraPrincipal"] = sdrInfo.Rows[0]["id_estructura"].ToString();
                    tblEstrucutra = gDAL.fnObtenerEstructura(sdrInfo.Rows[0]["id_estructura"].ToString());
                    fnCrearNodos(0, null, tblEstrucutra, Convert.ToInt32(sdrInfo.Rows[0]["id_estructura"].ToString()));
                    //trvEstructura.Nodes[0].Selected = true;
                    //trvEstructura.Nodes[0].SelectAction = TreeNodeSelectAction.None;
                    //lblSelVal.Text = trvEstructura.Nodes[0].Text;
                    //hdnSel.Value = trvEstructura.Nodes[0].Text;
                    //hdnValuePath.Value = trvEstructura.Nodes[0].ValuePath;
                    //hdnSelVal.Value = trvEstructura.Nodes[0].Value;
                    sender = trvEstructura;
                    trvEstructura_SelectedNodeChanged(sender, e);
                }
                txtNombreNodo.Enabled = false;
                btnAgregar.Enabled = false;
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

    /// <summary>
    /// Trae la lista de sucursales activas asignadas al usuario y las carga en el GridView
    /// </summary>
    private void fnCargarSucursales()
    {
        try
        {
            DataTable tblEstructura = new DataTable();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            tblEstructura = clsComun.fnLlenarDropSucursales(false); //clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); 
            gdvSucursales.DataSource = tblEstructura;
            ViewState["tblEstructura"] = tblEstructura;
            gdvSucursales.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            gdvSucursales.DataSource = null;
            gdvSucursales.DataBind();
        }
        catch
        {
            //referencia nula
        }
    }

    /// <summary>
    /// Método encargado de llenar el drop de los paises
    /// </summary>
    private void fnCargarPaises()
    {
        ddlPais.DataSource = clsComun.fnLlenarDropPaises();
        ddlPais.DataBind();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los estados
    /// </summary>
    /// <param name="psIdPais"></param>
    private void fnCargarEstados(string psIdPais)
    {
        ddlEstado.DataSource = clsComun.fnLlenarDropEstados(psIdPais);
        ddlEstado.DataBind();
    }

    protected void grvDetalles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        gdvSucursales.DataSource = null;
        gdvSucursales.DataBind();
    }
    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sIdPais = (sender as DropDownList).SelectedValue;
        fnCargarEstados(sIdPais);
    }
    protected void btnGuardarActualizar_Click(object sender, EventArgs e)
    {
        //validaciones
        if (string.IsNullOrEmpty(txtSucursal.Text)
            || string.IsNullOrEmpty(txtMunicipio.Text)
            || string.IsNullOrEmpty(txtCalle.Text)
            || string.IsNullOrEmpty(txtCodigoPostal.Text)
            || !clsComun.fnIsInt(txtCodigoPostal.Text)
            || txtCodigoPostal.Text.Length > 5
            )
        {
            clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        gDAL = new clsOperacionSucursales();

        try
        {
            int retVal = gDAL.fnGuardarSucursal(
                hdIdEstructura.Value,
                txtSucursal.Text,
                PAXCrypto.CryptoAES.EncriptaAES(txtCalle.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtNoExterior.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtNoInterior.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtColonia.Text),
                txtReferencia.Text,
                PAXCrypto.CryptoAES.EncriptaAES(string.Format("{0:00000}", Convert.ToInt32(txtCodigoPostal.Text))),
                //PAXCrypto.CryptoAES.EncriptaAES(txtCodigoPostal.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtLocalidad.Text),
                PAXCrypto.CryptoAES.EncriptaAES(txtMunicipio.Text),
                ddlEstado.SelectedValue,
                hdnSelVal.Value);

            if (retVal != 0)
            {
                clsConfiguracionPlantilla conf = new clsConfiguracionPlantilla();

                if (hdIdEstructura.Value == "")
                    conf.fnActualizaPlantilla(0, 1, retVal, "Black", 0);

                fnCargarSucursales();
                fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), true);

                fnBorrarNodoSeleccionado();
                tblEstrucutra = gDAL.fnObtenerEstructura(ViewState["idEstructuraPrincipal"].ToString());
                trvEstructura.Nodes.Clear();
                fnCrearNodos(0, null, tblEstrucutra, Convert.ToInt32(ViewState["idEstructuraPrincipal"]));
                trvEstructura.ExpandAll();
                btnBorrar.Enabled = true;
                btnEditar.Enabled = true;
                btnAgregar.Enabled = true;
                trvEstructura.Enabled = true;
                txtNombreNodo.Enabled = true;
                sender = trvEstructura;
                trvEstructura_SelectedNodeChanged(sender, e);
                fDesHabilitarDatosFiscales();
            }
            else
                throw new Exception(Resources.resCorpusCFDIEs.varNoAcReg);

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
            fnLimpiarPantalla();
            txtNombreNodo.Enabled = false;
            btnAgregar.Enabled = false;
        }
    }

    /// <summary>
    /// Seleccionar el tipo de mensaje a mostrar
    /// </summary>
    /// <param name="pbNuevo">Es nuevo registro</param>
    /// <param name="pbExito">Fue exitosa la acción</param>
    private void fnSeleccionarMensaje(bool pbNuevo, bool pbExito)
    {
        if (pbExito)
        {
            if (pbNuevo)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionSucursales",
                    "fnGuardarSucursal",
                    "Se agregó una nueva sucursal con los datos",
                    txtSucursal.Text,
                    txtCalle.Text,
                    txtNoExterior.Text,
                    txtNoInterior.Text,
                    txtColonia.Text,
                    txtReferencia.Text,
                    txtCodigoPostal.Text,
                    txtLocalidad.Text,
                    txtMunicipio.Text,
                    ddlEstado.SelectedItem.Text
                    );
            }
            else
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionSucursales",
                    "fnGuardarSucursal",
                    "Se modificó la sucursal con ID " + hdIdEstructura.Value + " con los datos",
                    txtSucursal.Text,
                    txtCalle.Text,
                    txtNoExterior.Text,
                    txtNoInterior.Text,
                    txtColonia.Text,
                    txtReferencia.Text,
                    txtCodigoPostal.Text,
                    txtLocalidad.Text,
                    txtMunicipio.Text,
                    ddlEstado.SelectedItem.Text
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

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        btnBorrar.Enabled = true;
        btnEditar.Enabled = true;
        btnAgregar.Enabled = true;
        trvEstructura.Enabled = true;
        fnLimpiarPantalla();
    }

    /// <summary>
    /// Vacía todos los controles del formulario
    /// </summary>
    private void fnLimpiarPantalla()
    {
        try
        {
            clsComunCatalogo.fnLimpiarFormulario(pnlFormulario);
            ddlPais.SelectedIndex = 0;
            ddlPais_SelectedIndexChanged(ddlPais, null);
            ddlEstado.SelectedIndex = 0;
        }
        finally
        {
            btnCancelar.Visible = false;
            hdIdEstructura.Value = string.Empty;
            gdvSucursales.SelectedIndex = -1;
        }
    }
    protected void gdvSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrFila = (GridViewRow)gdvSucursales.SelectedRow;

        try
        {
            hdIdEstructura.Value = gdvSucursales.SelectedDataKey.Value.ToString();

            clsComunCatalogo.fnAsignarValorFila(gvrFila, pnlFormulario);

            ddlPais_SelectedIndexChanged(ddlPais, null);
            ddlEstado.SelectedItem.Text = ((Label)gvrFila.FindControl("lblEstado")).Text;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        //btnCancelar.Visible = true;
    }
    protected void gdvSucursales_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //cancelamos la acción por defecto
        e.Cancel = false;
        gDAL = new clsOperacionSucursales();

        try
        {
            //Obtenemos el ID de la fila seleccionada
            string id_estructura = e.Keys["id_estructura"].ToString();

            int retVal = gDAL.fnEliminarSucursal(id_estructura);

            if (retVal != 0)
            {
                //Si se estaba editando la estructura, entonces limpiamos los datos del formulario
                if (!string.IsNullOrEmpty(hdIdEstructura.Value) && hdIdEstructura.Value.Equals(id_estructura))
                    fnLimpiarPantalla();

                fnCargarSucursales();
                fnCargarSucursalesBorradas();
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionSucursales",
                    "fnEliminarSucursal",
                    "Se dió de baja la sucursal con ID " + id_estructura
                    );
            }
            else
                throw new Exception("No se realizó la baja");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
        }

    }
    private void fnCargarSucursalesBorradas()
    {
        gDAL = new clsOperacionSucursales();
        DataTable dtSucursalesBorradas = new DataTable();

        try
        {
            dtSucursalesBorradas = gDAL.fnObtenerSucursalesBorradas();

            if (dtSucursalesBorradas.Rows.Count > 0)
            {
                ddlSucursalesBorradas.DataSource = dtSucursalesBorradas;
                ddlSucursalesBorradas.DataBind();
                //pnlBorrados.Visible = true;
            }
            else
            {
                pnlBorrados.Visible = false;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }
    protected void btnReactivar_Click(object sender, EventArgs e)
    {
        gDAL = new clsOperacionSucursales();

        try
        {
            int retVal = gDAL.fnActualizarSucursalBorrada(ddlSucursalesBorradas.SelectedValue);

            if (retVal != 0)
            {
                fnCargarSucursales();
                fnCargarSucursalesBorradas();

                tblEstrucutra = gDAL.fnObtenerEstructura(ViewState["idEstructuraPrincipal"].ToString());
                trvEstructura.Nodes.Clear();
                fnCrearNodos(0, null, tblEstrucutra, Convert.ToInt32(ViewState["idEstructuraPrincipal"]));
                trvEstructura.ExpandAll();

                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void gdvSucursales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvSucursales.PageIndex = e.NewPageIndex;
        fnCargarSucursales();
    }

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


    /// <summary>
    /// Crea la lista de nodos reucperada de la estructura.
    /// </summary>
    /// <param name="indicePadre"></param>
    /// <param name="nodePadre"></param>
    /// <param name="tblEstructura"></param>
    private void fnCrearNodos(int indicePadre, TreeNode nodePadre, DataTable tblEstructura, int nid_estructura)
    {
        //Se obtiene las sucursales que pertenecen al usuario
        datosUsuario = clsComun.fnUsuarioEnSesion();
        tblSucursalUsu = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); 
        //Se crea un DataView de las sucursales pertenecientes al usuario
        DataView dvSucursales = new DataView(tblSucursalUsu);

        // Crear un DataView con los Nodos que dependen del Nodo padre pasado como parámetro.
        DataView dataViewHijos = new DataView(tblEstructura);
        dataViewHijos.RowFilter = tblEstructura.Columns["id_padre"].ColumnName + " = " + indicePadre;

        // Agregar al TreeView los nodos Hijos que se han obtenido en el DataView.
        foreach (DataRowView dataRowCurrent in dataViewHijos)
        {
            TreeNode nuevoNodo = new TreeNode();
            nuevoNodo.Text = dataRowCurrent["nombre"].ToString().Trim();
            nuevoNodo.Value = dataRowCurrent["id_estructura"].ToString().Trim();
            dvSucursales.RowFilter = "id_estructura = " + nuevoNodo.Value; //SE FILTRA LA ESTRUCTURA PARA REVISAR SI PUEDE MODIFICAR EL NODO CORRESPONDIENTE

            if (Convert.ToInt32(nuevoNodo.Value) == nid_estructura)
            {
                nuevoNodo.Selected = true;
                lblSelVal.Text = nuevoNodo.Text;
                hdnSel.Value = nuevoNodo.Text;
                hdnValuePath.Value = nuevoNodo.ValuePath;
                hdnSelVal.Value = nuevoNodo.Value;
            }
            if (lblSelVal.Text != "Ninguno")
                nid_estructura = 0;
             //si el parámetro nodoPadre es nulo es porque es la primera llamada, son los Nodos
            // del primer nivel que no dependen de otro nodo.
            if (nodePadre == null)
            {
                ViewState["nodoPadre"] = dataRowCurrent["id_estructura"].ToString().Trim();
                if (dvSucursales.Count == 0) //SI NO CONTIENE RESULTADOS NO PUEDE MODIFICAR NODO
                    nuevoNodo.SelectAction = TreeNodeSelectAction.None;
                trvEstructura.Nodes.Add(nuevoNodo);
            }
            // se añade el nuevo nodo al nodo padre.
            else
            {
                //nuevoNodo.ShowCheckBox = true;
                if (dvSucursales.Count == 0) //SI NO CONTIENE RESULTADOS NO PUEDE MODIFICAR NODO
                    nuevoNodo.SelectAction = TreeNodeSelectAction.None; 
                nodePadre.ChildNodes.Add(nuevoNodo);

            }

            // Llamada recurrente al mismo método para agregar los Hijos del Nodo recién agregado.
            fnCrearNodos(Int32.Parse(dataRowCurrent["id_estructura"].ToString()), nuevoNodo, tblEstructura, nid_estructura);
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(txtNombreNodo.Text) && !string.IsNullOrEmpty(hdnSel.Value))
        {

            //Crea nodos Hijos

            List<TreeNode> lista = Find(hdnSel.Value);

            foreach (TreeNode item in lista)
            {

                if (item.ValuePath == hdnValuePath.Value)
                {
                    item.ChildNodes.Add(new TreeNode(txtNombreNodo.Text));
                    txtSucursal.Text = txtNombreNodo.Text;
                    trvEstructura.ExpandAll();
                    fnBorrarNodoSeleccionado();
                    btnBorrar.Enabled = false;
                    btnEditar.Enabled = false;
                    btnAgregar.Enabled = false;
                    trvEstructura.Enabled = false;
                    fHabilitarDatosFiscales();
                    return;
                }
            }
         
        }
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAgregaNodo);
        }


    }

    protected void btnBorrar_Click(object sender, EventArgs e)
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

                    gDAL = new clsOperacionSucursales();

                    try
                    {
                        //Obtenemos el ID de la fila seleccionada
                        string id_estructura = hdnSelVal.Value;

                        nfacGenerados = gDAL.fnBuscarGenerados(id_estructura);
                        if (nfacGenerados > 0)
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNoAgregar);
                            return;
                        }

                        int retVal = gDAL.fnEliminarSucursal(id_estructura);

                        if (retVal != 0)
                        {
                            //Si se estaba editando la estructura, entonces limpiamos los datos del formulario
                            if (!string.IsNullOrEmpty(hdIdEstructura.Value) && hdIdEstructura.Value.Equals(id_estructura))
                                fnLimpiarPantalla();

                            fnCargarSucursales();
                            fnCargarSucursalesBorradas();
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);

                            clsComun.fnNuevaPistaAuditoria(
                                "webOperacionSucursales",
                                "fnEliminarSucursal",
                                "Se dió de baja la sucursal con ID " + id_estructura
                                );

                            RemoveNodeRecurrently(trvEstructura.Nodes, hdnValuePath.Value);
                            fnBorrarNodoSeleccionado();
                            fDesHabilitarDatosFiscales();
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
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNodoNoSel);
        }
    }

    #region BusquedaRecursiva
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
    # endregion

    /// <summary>
    /// Elimina el nodo seleccionado
    /// </summary>
    /// <param name="childNodeCollection"></param>
    /// <param name="text"></param>
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
    protected void trvEstructura_SelectedNodeChanged(object sender, EventArgs e)
    {
        //Recuperar nodo seleccionado

        tree = new TreeView();
        tree = (TreeView)sender;

        lblSelVal.Text = tree.SelectedNode.Text;
        hdnSel.Value = tree.SelectedNode.Text;
        hdnSelVal.Value = tree.SelectedNode.Value;
        hdnValuePath.Value = tree.SelectedNode.ValuePath;

        tree.ForeColor = Color.Red;
        txtNombreNodo.Enabled = false;
        btnAgregar.Enabled = false;
    }
    protected void btnEditar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnSel.Value))
        {
            DataTable tblEstructura = new DataTable();
            DataRow[] rowEstructura;
            tblEstructura = (DataTable)ViewState["tblEstructura"];
       try
            {
                rowEstructura = tblEstructura.Select("id_estructura=" + trvEstructura.SelectedNode.Value);
     
                if (rowEstructura.Length != 0)
                {
                    hdIdEstructura.Value = trvEstructura.SelectedNode.Value;

                    txtSucursal.Text = rowEstructura[0]["nombre"].ToString();
                    txtMunicipio.Text = rowEstructura[0]["municipio"].ToString();
                    txtLocalidad.Text = rowEstructura[0]["localidad"].ToString();
                    txtCalle.Text = rowEstructura[0]["calle"].ToString();
                    txtNoExterior.Text = rowEstructura[0]["numero_exterior"].ToString();
                    txtNoInterior.Text = rowEstructura[0]["numero_interior"].ToString();
                    txtColonia.Text = rowEstructura[0]["colonia"].ToString();
                    txtReferencia.Text = rowEstructura[0]["referencia"].ToString();
                    txtCodigoPostal.Text = rowEstructura[0]["codigo_postal"].ToString();
                    ddlEstado.SelectedIndex = ddlEstado.Items.IndexOf(ddlEstado.Items.FindByText(rowEstructura[0]["estado"].ToString()));
                    ddlPais.SelectedIndex = ddlPais.SelectedIndex = ddlPais.Items.IndexOf(ddlPais.Items.FindByText(rowEstructura[0]["pais"].ToString()));

                    ddlPais_SelectedIndexChanged(ddlPais, null);
                    fnBorrarNodoSeleccionado();

                    //btnCancelar.Visible = true;
                    btnAgregar.Enabled = false;
                    txtNombreNodo.Enabled = false;
                    fHabilitarDatosFiscales();
                }
                else
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEditarNodo);
                    return;
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            }


        }
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNodoNoSel);
        }
    }

    /// <summary>
    /// Seleccionar nodo
    /// </summary>
    private void fnBorrarNodoSeleccionado()
    {
        lblSelVal.Text = Resources.resCorpusCFDIEs.lblNinguno;
        hdnSel.Value = string.Empty;
        hdnValuePath.Value = string.Empty;
        txtNombreNodo.Text = string.Empty;
    }
    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            gDAL = new clsOperacionSucursales();
            fnBorrarNodoSeleccionado();
            tblEstrucutra = gDAL.fnObtenerEstructura(ViewState["idEstructuraPrincipal"].ToString());
            trvEstructura.Nodes.Clear();
            fnCrearNodos(0, null, tblEstrucutra, Convert.ToInt32(ViewState["idEstructuraPrincipal"]));
            trvEstructura.ExpandAll();
            btnBorrar.Enabled = true;
            btnEditar.Enabled = true;
            btnAgregar.Enabled = true;
            trvEstructura.Enabled = true;
            sender = trvEstructura;
            trvEstructura_SelectedNodeChanged(sender, e);
            fnLimpiarPantalla();
            btnAgregar.Enabled = false;
            txtNombreNodo.Enabled = false;
            fDesHabilitarDatosFiscales();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        fnLimpiarPantalla();
        fDesHabilitarDatosFiscales();
        txtNombreNodo.Enabled = true;
        btnAgregar.Enabled = true;
    }
    private void fDesHabilitarDatosFiscales()
    {
        txtSucursal.Enabled = false;
        ddlPais.Enabled = false;
        ddlEstado.Enabled = false;
        txtMunicipio.Enabled = false;
        txtLocalidad.Enabled = false;
        txtCalle.Enabled = false;
        txtNoExterior.Enabled = false;
        txtNoInterior.Enabled = false;
        txtColonia.Enabled = false;
        txtReferencia.Enabled = false;
        txtCodigoPostal.Enabled = false;
        btnGuardarActualizar.Enabled = false;
    }

    private void fHabilitarDatosFiscales()
    {
        txtSucursal.Enabled = true;
        ddlPais.Enabled = true;
        ddlEstado.Enabled = true;
        txtMunicipio.Enabled = true;
        txtLocalidad.Enabled = true;
        txtCalle.Enabled = true;
        txtNoExterior.Enabled = true;
        txtNoInterior.Enabled = true;
        txtColonia.Enabled = true;
        txtReferencia.Enabled = true;
        txtCodigoPostal.Enabled = true;
        btnGuardarActualizar.Enabled = true;
    }
}

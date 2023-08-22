using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Drawing;
using System.IO;

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
        try
        {
            if (Session["objUsuario"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                string sPagina = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
                if (!new clsOperacionUsuarios().fnObtenerPermisoPagina(sPagina))
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varPermisos);
                    Response.Redirect("~/Default.aspx");
                }

            }
            if (!IsPostBack)
            {

                tblEstrucutra = new DataTable();
                //fnCargarSeries();
                ////fnCargarFolio();
                fnCargarPaises();
                fnCargarEstados(ddlPais.SelectedValue);
                //fnCargarRfcs();
                //txtNombreNodo.Enabled = false;
                //btnAgregar.Enabled = false;
                fDesHabilitarDatosFiscales();
                immImagenMostrar.Visible = false;
                //btnBorrar.OnClientClick = "return confirm('" + Resources.resCorpusCFDIEs.varBorrarNodo + "');";

                gOp = new clsOperacionCuenta();
                gDAL = new clsOperacionSucursales();
                clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
                try
                {
                    tblEstrucutra = gDAL.fnObtenerEstructuraUsuario(usuarioActivo.id_usuario);
                    //fnCrearNodos(tblEstrucutra);
                    fnCrearNodos(0, null, tblEstrucutra, 0);
                    ViewState["tblEstructura"] = tblEstrucutra;

                    sender = trvEstructura;
                    //trvEstructura_SelectedNodeChanged(sender, e);
                    //txtNombreNodo.Enabled = false;
                    //btnAgregar.Enabled = false;
                    btnNuevo.Enabled = false;
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
        { clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion); }
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

    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sIdPais = (sender as DropDownList).SelectedValue;
        fnCargarEstados(sIdPais);
    }

    protected void btnGuardarActualizar_Click(object sender, EventArgs e)
    {
        gDAL = new clsOperacionSucursales();
        string imagen = string.Empty;
        byte[] imagenBytes = new byte[0];

        string sNoTienda =  txtNumTienda.Text;
        string sSucursal = txtSucursal.Text;
        string scalle = txtCalle.Text;
        string snumero_exterior = txtNoExterior.Text;
        string snumero_interior = txtNoInterior.Text;
        string scolonia = txtColonia.Text;
        string sreferencia = string.Empty;
        string slocalidad = txtLocalidad.Text;
        string smunicipio = txtMunicipio.Text;
        string snid_estado = ddlEstado.SelectedValue;
        string scodigo_postal = txtCodigoPostal.Text;

        //validaciones
        if (string.IsNullOrEmpty(txtSucursal.Text)
            || string.IsNullOrEmpty(txtMunicipio.Text)
            || string.IsNullOrEmpty(txtCalle.Text)
            || string.IsNullOrEmpty(txtCodigoPostal.Text)
            || !clsComun.fnIsInt(txtCodigoPostal.Text)
            || txtCodigoPostal.Text.Length > 5
            //|| string.IsNullOrEmpty(txtNombreMostrar.Text)
            //|| Convert.ToInt32(ddlRfc.SelectedItem.Value) <= 0
            //|| Convert.ToInt32(txtIdSucursal.Text) <= 0
            || string.IsNullOrEmpty(txtNumTienda.Text)
            )
        {
            //clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.varValidacionError);
            lblErrorLog.Text = Resources.resCorpusCFDIEs.varValidacionError;
            mpeErrorLog.Show();
            return;
        }

        if (fupLogo.HasFile)
        {
            if (Path.GetExtension(fupLogo.FileName).ToLower() != ".jpg")
            {
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
                lblAviso.Text = Resources.resCorpusCFDIEs.VarLogo;
                mpeAvisos.Show();
                return;
            }

            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
            System.Web.HttpPostedFile mifichero = fupLogo.PostedFile;
            double dTamañoArchivo = mifichero.ContentLength / 1024;
            System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupLogo.FileContent);
            if (dTamañoArchivo > psMaximo)
            {
                //El tamaño máximo del logo es de 1MB
                //clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
                lblAviso.Text = Resources.resCorpusCFDIEs.VarErrorLogo;
                mpeAvisos.Show();
                return;
            }

            imagenBytes = fupLogo.FileBytes;
        }

        if (!string.IsNullOrEmpty(hdIdEstructura.Value))
        {
            int idEstrucutura = Convert.ToInt32(ViewState["SelNodo"]);
            ViewState.Remove("SelNodo");

            //Evalua que el numero de Sucursal no este asignado a otra Sucursal
            int sIdEstExistente = new clsOperacionSucursales().fnObtenerIdEstructura(sNoTienda);
            if (sIdEstExistente > 0)
            {
                if (idEstrucutura != sIdEstExistente)
                {
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNumTiendaExiste);
                    lblAviso.Text = Resources.resCorpusCFDIEs.varNumTiendaExiste;
                    mpeAvisos.Show();
                    return;
                }
            }

            int serie = 0;
            //Insertamos relacion para la serie y folio
            if ((ddlSerie.SelectedValue == "N/A"))
            {
                serie = 0;
            }
            else
            {
                serie = Convert.ToInt32(ddlSerie.SelectedValue);
            }

            clsComun.fnAgregarRelacionSerieFolio(Convert.ToInt32(hdIdEstructura.Value), serie);
        }
        else
        {
            //Evalua que el numero de Sucursal no este asignado a otra Sucursal
            int sIdEstExistente = new clsOperacionSucursales().fnObtenerIdEstructura(sNoTienda);
            if (sIdEstExistente > 0)
            {
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNumTiendaExiste);
                lblAviso.Text = Resources.resCorpusCFDIEs.varNumTiendaExiste;
                mpeAvisos.Show();
                return;
            }
        }

        try
        {
            int retVal = gDAL.fnGuardarSucursal(sNoTienda,
                string.Empty,
                hdIdEstructura.Value,
                sSucursal,
                scalle,
                snumero_exterior,
                snumero_interior,
                scolonia,
                sreferencia,
                scodigo_postal,
                slocalidad,
                smunicipio,
                snid_estado,
                hdnSelVal.Value);

            if (retVal != 0)
            {
                //Agregamos la imagen a la surcusal
                gDAL.fnAgregarImagen(retVal, imagenBytes);

                //fnCargarSucursales();
                ViewState.Remove("addSucursal");
                ViewState.Remove("updNodoPrincipal");
                fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), true);
                clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
                fnBorrarNodoSeleccionado();
                tblEstrucutra = gDAL.fnObtenerEstructuraUsuario(usuarioActivo.id_usuario);
                ViewState["tblEstructura"] = tblEstrucutra;
                //fnCrearNodos(0, null, tblEstrucutra, 0);
                fnLimpiarPantalla();
                btnBorrar.Enabled = true;
                btnNuevo.Enabled = true;
                btnEditar.Enabled = true;
                //btnAgregar.Enabled = true;
                trvEstructura.Enabled = true;
                //txtNombreNodo.Enabled = true;
                sender = trvEstructura;
                //trvEstructura_SelectedNodeChanged(sender, e);
                fDesHabilitarDatosFiscales();

                lblVerNombreMostrar.Visible = false; ;
                lblVerNombreMostrar.Text = string.Empty;
                immImagenMostrar.Visible = false;
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

            //lblRfc.Text = Resources.resCorpusCFDIEs.lblRFC;
            lblSelVal.Text = Resources.resCorpusCFDIEs.lblNinguno;
            fnLimpiarPantalla();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            tblEstrucutra = gDAL.fnObtenerEstructuraUsuario(datosUsuario.id_usuario);
            trvEstructura.Nodes.Clear();
            fnCrearNodos(0, null, tblEstrucutra, 0);
            trvEstructura.ExpandAll();
            //btnNuevo.Text = Resources.resCorpusCFDIEs.lblNuevoCorreo;
            hdnValuePath.Value = string.Empty;
            tblSeriesFolios.Visible = false;

            //txtNombreNodo.Enabled = false;
            //btnAgregar.Enabled = false;
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
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                lblAviso.Text = Resources.resCorpusCFDIEs.varAlta;
                mpeAvisos.Show();
                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionSucursales",
                    "fnGuardarSucursal",
                    "Se agregó una nueva sucursal con los datos",
                    txtNumTienda,
                    //txtIdSucursal,
                    //txtNombreMostrar,
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
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);
                lblAviso.Text = Resources.resCorpusCFDIEs.varCambio;
                mpeAvisos.Show();

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
            {
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
                lblErrorLog.Text = Resources.resCorpusCFDIEs.varErrorAlta;
                mpeErrorLog.Show();
            }
            else
            {
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
                lblErrorLog.Text = Resources.resCorpusCFDIEs.varErrorCambio;
                mpeErrorLog.Show();
            }
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        btnBorrar.Enabled = true;
        btnEditar.Enabled = true;
        //btnAgregar.Enabled = true;
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
            lblVerNombreMostrar.Visible = false; ;
            lblVerNombreMostrar.Text = string.Empty;
            immImagenMostrar.Visible = false;
            clsComunCatalogo.fnLimpiarFormulario(pnlFormulario);
            ddlPais.SelectedIndex = 0;
            ddlPais_SelectedIndexChanged(ddlPais, null);
            ddlEstado.SelectedIndex = 0;
            //lblRfc.Text = Resources.resCorpusCFDIEs.lblRFC;
            //txtTienda.Text = string.Empty;
            //txtIdSucursal.Enabled = true;
            //txtIdSucursal.Text = string.Empty;
            //btnCancelarTienda.Visible = true;
            //btnAgregarTienda.Text = Resources.resCorpusCFDIEs.lblAgregar;
            
        }
        finally
        {
            btnCancelar.Visible = false;
            hdIdEstructura.Value = string.Empty;
            //gdvSucursales.SelectedIndex = -1;
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
                //if (dvSucursales.Count == 0) //SI NO CONTIENE RESULTADOS NO PUEDE MODIFICAR NODO
                //    nuevoNodo.SelectAction = TreeNodeSelectAction.None;
                trvEstructura.Nodes.Add(nuevoNodo);
            }
            // se añade el nuevo nodo al nodo padre.
            else
            {
                //nuevoNodo.ShowCheckBox = true;
                //if (dvSucursales.Count == 0) //SI NO CONTIENE RESULTADOS NO PUEDE MODIFICAR NODO
                //    nuevoNodo.SelectAction = TreeNodeSelectAction.None; 
                nodePadre.ChildNodes.Add(nuevoNodo);

            }

            // Llamada recurrente al mismo método para agregar los Hijos del Nodo recién agregado.
            fnCrearNodos(Int32.Parse(dataRowCurrent["id_estructura"].ToString()), nuevoNodo, tblEstructura, nid_estructura);
        }
    }

    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        if (hdnValuePath.Value != string.Empty)
        {
            ////int nfacGenerados = 0;
            List<TreeNode> lisParent = Find(hdnSel.Value);

            //if (lisParent[0].ChildNodes.Count > 0)
            //{
            //    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblEliminarSucursales);// "Debe eliminar las Sucursales");// Resources.resCorpusCFDIEs.varNodoNoSel);
            //    lblAviso.Text = Resources.resCorpusCFDIEs.lblEliminarSucursales;
            //    mpeAvisos.Show();
            //    return;
            //}

            if (!string.IsNullOrEmpty(Convert.ToString(ViewState["updNodoPrincipal"])))//Si variable se edita el nodo principal
            {
                lblAviso.Text = Resources.resCorpusCFDIEs.varEditarNodo;
                mpeAvisos.Show();
                return;
            }

            #region comentado 02/04/2013

            foreach (TreeNode item in lisParent)
            {
                gDAL = new clsOperacionSucursales();
                try
                {
                    //Obtenemos el ID de la fila seleccionada
                    string id_estructura = hdnSelVal.Value;
                    //Validamos que la Sucursal no cuente con comprobantes
                    int nfacGenerados = gDAL.fnBuscarGenerados(id_estructura);
                    if (nfacGenerados > 0)
                    {
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNoAgregar);
                        lblAviso.Text = Resources.resCorpusCFDIEs.varNoAgregar;
                        mpeAvisos.Show();
                        return;
                    }

                    //Checamos que la estructura no esta ligada a un usuario
                    if (clsComun.fnValidarEstructura(Convert.ToInt32(id_estructura)) == true)
                    {
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varSucursalComprobante);
                        lblAviso.Text = Resources.resCorpusCFDIEs.vrUsuarioAsignado;
                        mpeAvisos.Show();
                        return;
                    }

                    ///Checamos que la estructura no haya realiza ticket´s
                    if (clsComun.fnValidaEstructuraTicket(Convert.ToInt32(id_estructura)) > 0)
                    {
                        lblAviso.Text = Resources.resCorpusCFDIEs.varSucursalTicket;
                        mpeAvisos.Show();
                        return;
                    }

                    int retVal = gDAL.fnEliminarSucursal(id_estructura);

                    if (retVal != 0)
                    {
                        //Si se estaba editando la estructura, entonces limpiamos los datos del formulario
                        if (!string.IsNullOrEmpty(hdIdEstructura.Value) && hdIdEstructura.Value.Equals(id_estructura))
                            fnLimpiarPantalla();

                        //fnCargarSucursales();
                        //fnCargarSucursalesBorradas();
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);
                        lblAviso.Text = Resources.resCorpusCFDIEs.varBaja;
                        mpeAvisos.Show();

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
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
                    lblErrorLog.Text = Resources.resCorpusCFDIEs.varErrorBaja;
                    mpeErrorLog.Show();
                }
            }

            //txtNombreNodo.Enabled = false;
            //btnAgregar.Enabled = false;
            #endregion

            lblVerNombreMostrar.Visible = false;
            immImagenMostrar.Visible = false;
            //btnNuevo.Text = Resources.resCorpusCFDIEs.lblNuevoCorreo;
        }
        else
        {
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNodoNoSel);
            lblAviso.Text = Resources.resCorpusCFDIEs.varNodoNoSel;
            mpeAvisos.Show();
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
                if (parentNode != null)
                    parentNode.ChildNodes.Remove(childNode);
                else
                    childNodeCollection.Remove(childNode);
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
        int nid_estructura = Convert.ToInt32(hdnSelVal.Value);
        DataTable dtMenu = clsComun.fnObtenerMenu(nid_estructura);
        if (dtMenu != null && dtMenu.Rows.Count > 0)
        {
            lblVerNombreMostrar.Text = dtMenu.Rows[0]["Descripcion"].ToString();
            immImagenMostrar.Visible = true;
            lblVerNombreMostrar.Visible = true;
            if (!string.IsNullOrEmpty(dtMenu.Rows[0]["Url"].ToString()))
                immImagenMostrar.ImageUrl = "~/Imagen.aspx?estructura=" + Convert.ToInt32(hdnSelVal.Value)+"&Ubic=1";
            else
                immImagenMostrar.ImageUrl = clsComun.ObtenerParamentro("NoImagen");
            btnEditar.Enabled = true;
            ViewState["updNodoPrincipal"] = "0";
            btnNuevo.Enabled = true;
            ViewState["addSucursal"] = "1";
            lblNodoSel.Text = Resources.resCorpusCFDIEs.lblTienda;
        }
        else
        {
            ViewState.Remove("updNodoPrincipal"); 

            lblVerNombreMostrar.Visible = true;
            lblVerNombreMostrar.Text = tree.SelectedNode.Text;
            immImagenMostrar.Visible = true;
            //Mostramos la imagen
            immImagenMostrar.ImageUrl = "~/Imagen.aspx?estructura=" + Convert.ToInt32(hdnSelVal.Value) + "&Ubic=1";

            //btnNuevo.Text = Resources.resCorpusCFDIEs.lblNuevoCorreo;
            ViewState.Remove("addSucursal");
            lblNodoSel.Text = Resources.resCorpusCFDIEs.lblSucursal;
            btnEditar.Enabled = true;
            btnNuevo.Enabled = false;
            //lblVerNombreMostrar.Text = "<" + Resources.resCorpusCFDIEs.lblVerNombreCapturar + ">";
            //immImagenMostrar.Visible = false;
        }

        tree.ForeColor = Color.Red;
        fnCargarSeries();
        fnCargarFolio();
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Convert.ToString(ViewState["updNodoPrincipal"])))//Si variable se edita el nodo principal
        {
            if (!string.IsNullOrEmpty(hdnSel.Value))
            {
                lblAviso.Text = Resources.resCorpusCFDIEs.varEditarNodo;
                mpeAvisos.Show();
                return;
                ////obtenemos la inf del nodo
                //DataTable dtAuxuliar = clsComun.fnObtnerNodoRaiz(Convert.ToInt32(hdnSelVal.Value));
                //txtTienda.Text = dtAuxuliar.Rows[0]["nombre"].ToString();
                //txtIdSucursal.Text = dtAuxuliar.Rows[0]["id_estructura_cobro"].ToString();
                //txtIdSucursal.Enabled = false;
                ////btnCancelarTienda.Visible = false;
                //btnAgregarTienda.Text = Resources.resCorpusCFDIEs.btnActualizar;
                //mpeNewTienda.Show();
            }
        }

        if (!string.IsNullOrEmpty(hdnSel.Value))
        {
            int nidEstructura = Convert.ToInt32(hdnSelVal.Value);
            DataTable tblEstructura = new DataTable();
            DataRow[] rowEstructura;
            tblEstructura =(DataTable)ViewState["tblEstructura"];
            
            try
            {
                rowEstructura = tblEstructura.Select("id_estructura=" + trvEstructura.SelectedNode.Value);
                string sRfc = string.Empty;// rowEstructura[0]["rfc"].ToString();
                String sIdEstructura = rowEstructura[0]["id_estructura"].ToString();
                gDAL = new clsOperacionSucursales();
                DataTable tblEstructuraSeleccionada = gDAL.fnObtenerSucursal(sIdEstructura);
                rowEstructura = tblEstructuraSeleccionada.Select("id_estructura=" + trvEstructura.SelectedNode.Value);
                
                if (rowEstructura.Length != 0)
                {
                    DataTable sdrDomicilio = gDAL.fnObtenerTablaDomicilioSuc(Convert.ToInt32(sIdEstructura));

                    hdIdEstructura.Value = trvEstructura.SelectedNode.Value;
                    txtNumTienda.Text = rowEstructura[0]["num_tienda"].ToString();
                    //txtIdSucursal.Text = rowEstructura[0]["id_estructura_cobro"].ToString();
                    txtSucursal.Text = rowEstructura[0]["nombre"].ToString();
                    txtMunicipio.Text = sdrDomicilio.Rows[0]["municipio"].ToString();
                    txtLocalidad.Text = sdrDomicilio.Rows[0]["localidad"].ToString();
                    txtCalle.Text = sdrDomicilio.Rows[0]["calle"].ToString();
                    txtNoExterior.Text = sdrDomicilio.Rows[0]["numero_exterior"].ToString();
                    txtNoInterior.Text = sdrDomicilio.Rows[0]["numero_interior"].ToString();
                    txtColonia.Text = sdrDomicilio.Rows[0]["colonia"].ToString();
                    txtReferencia.Text = sdrDomicilio.Rows[0]["referencia"].ToString();
                    txtCodigoPostal.Text = sdrDomicilio.Rows[0]["codigo_postal"].ToString();
                    
                    ddlEstado.SelectedIndex = ddlEstado.Items.IndexOf(ddlEstado.Items.FindByText(sdrDomicilio.Rows[0]["estado"].ToString()));
                    ddlPais.SelectedIndex = ddlPais.SelectedIndex = ddlPais.Items.IndexOf(ddlPais.Items.FindByText(sdrDomicilio.Rows[0]["pais"].ToString()));

                    //int rfcIndex = ddlRfc.Items.IndexOf(ddlRfc.Items.FindByText(sRfc));
                    fHabilitarDatosFiscales();

                    //Validamos que la Sucursal no cuente con comprobantes
                    int nfacGenerados = gDAL.fnBuscarGenerados(sIdEstructura);
                    if (nfacGenerados > 0)
                    {
                        txtNumTienda.Enabled = false;
                        //ddlRfc.Enabled = false;
                    }
                    //Obtenemos la serie y folio 
                    DataTable dtAxuliarSerie = clsComun.fnObtenerSerieFolio(nidEstructura,null);

                    if (dtAxuliarSerie.Rows.Count > 0)
                    {
                        ddlSerie.SelectedIndex = ddlSerie.Items.IndexOf(ddlSerie.Items.FindByText(dtAxuliarSerie.Rows[0]["serie"].ToString()));
                        int idserie = Convert.ToInt32(ddlSerie.SelectedValue);
                        fnCargarFolio();
                    }
                    //else
                    //{
                    //    //if (rfcIndex >= 0)
                    //    //{
                    //    //    ddlRfc.SelectedIndex = ddlRfc.Items.IndexOf(ddlRfc.Items.FindByText(sRfc));
                    //    //    int idRfc = Convert.ToInt32(ddlRfc.SelectedValue);
                    //    //    if (new clsOperacionRFC().fnVerificarRfcCfd(idRfc))
                    //    //    {
                    //    //        ddlRfc.Enabled = false;
                    //    //    }
                    //    //}
                    //    //else
                    //    //{
                    //    //    ddlRfc.SelectedIndex = 0;
                    //    //    lblRfc.Text = lblRfc.Text + " (" + Resources.resCorpusCFDIEs.lblRfcEliminado + ")";
                    //    //}
                    //}
                   

                    ddlPais_SelectedIndexChanged(ddlPais, null);
                    //fnBorrarNodoSeleccionado();
                                     
                    btnNuevo.Enabled = false;
                    btnEditar.Enabled = false;
                    btnBorrar.Enabled = false;
                    ViewState["SelNodo"] = trvEstructura.SelectedNode.Value;
                    trvEstructura.Enabled = false;
                    
                }   
                else
                {
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEditarNodo);
                    lblAviso.Text = Resources.resCorpusCFDIEs.varEditarNodo;
                    mpeAvisos.Show();
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
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNodoNoSel);
            lblAviso.Text = Resources.resCorpusCFDIEs.varNodoNoSel;
            mpeAvisos.Show();
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
        //txtNombreNodo.Text = string.Empty;
    }

    /// <summary>
    /// Recupera las series por sucursal y tipo de documento.
    /// </summary>
    private void fnCargarSeries()
    {
        try
        {
            string nSucursal = string.Empty;

            if (!string.IsNullOrEmpty(hdnSelVal.Value))
                nSucursal = hdnSelVal.Value;

            DataTable table = new DataTable();

            table = clsTimbradoFuncionalidad.LlenarDropSeries(nSucursal, 1);// Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]));
            if (table.Rows.Count > 0)
            {
                DataRow fila = table.NewRow();
                fila[0] = 0;
                fila[1] = "N/A";
                table.Rows.Add(fila);
                DataView dtv = new DataView(table);
                dtv.Sort = "id_serie";
                ddlSerie.DataSource = dtv;
                tblSeriesFolios.Visible = true;
            }
            else
            {
                ddlSerie.Items.Clear();
                ddlSerie.Items.Add("N/A");
                tblSeriesFolios.Visible = false;
            }

            ddlSerie.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSerie.DataSource = null;
            ddlSerie.DataBind();
        }
        catch (Exception ex)
        {
            //referencia nula
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            ddlSerie.DataSource = null;
            ddlSerie.DataBind();
        }
    }

    /// <summary>
    /// Recupera los folios por tipo de sucursal y documento.
    /// </summary>
    private void fnCargarFolio()
    {
        try
        {
            string nSucursal = string.Empty;

            if (!string.IsNullOrEmpty(hdnSelVal.Value))
                nSucursal = hdnSelVal.Value;
            
            DataTable table = new DataTable();
            table = clsTimbradoFuncionalidad.LlenarDropSeries(nSucursal, 1);// Convert.ToInt32(ddlTipoDoc.SelectedValue.Split('|')[0]));
            if (ddlSerie.SelectedValue != "0")
            {
                //txtFolio.Text = table.Rows[ddlSerie.SelectedIndex]["folio"].ToString(); /* 16 - 02 - 2013 */
                DataRow[] row = table.Select("id_serie=" + ddlSerie.SelectedValue);
                txtFolio.Text = row[0]["folio"].ToString();
            }
            else
            {
                txtFolio.Text = "0";
            }
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            txtFolio.Text = "0";
        }
        catch (Exception ex)
        {
            //referencia nula
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            txtFolio.Text = "0";
        }
    }

    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            gDAL = new clsOperacionSucursales();
            fnBorrarNodoSeleccionado();
            tblEstrucutra = gDAL.fnObtenerEstructuraUsuario(datosUsuario.id_usuario);
            trvEstructura.Nodes.Clear();
            fnCrearNodos(0, null, tblEstrucutra, 0);
            trvEstructura.ExpandAll();
            btnBorrar.Enabled = true;
            btnEditar.Enabled = true;
            btnNuevo.Enabled = true;
            //btnAgregar.Enabled = true;
            trvEstructura.Enabled = true;
            sender = trvEstructura;
            //trvEstructura_SelectedNodeChanged(sender, e);
            fnLimpiarPantalla();
            //btnAgregar.Enabled = false;
            //txtNombreNodo.Enabled = false;
            fDesHabilitarDatosFiscales();
            immImagenMostrar.Visible = false;
            lblVerNombreMostrar.Visible = false;
            tblSeriesFolios.Visible = false;
            //btnNuevo.Text = Resources.resCorpusCFDIEs.lblNuevoCorreo;
            ViewState.Remove("addSucursal");
            ViewState.Remove("updNodoPrincipal");
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(Convert.ToString(ViewState["addSucursal"])))
        {
            trvEstructura.Enabled = false;
            fnLimpiarPantalla();
            fHabilitarDatosFiscales();
            //txtNombreNodo.Enabled = true;
            //btnAgregar.Enabled = true;
            immImagenMostrar.Visible = false;
            lblVerNombreMostrar.Visible = false;
            lblSelVal.Text = hdnSel.Value;// Resources.resCorpusCFDIEs.lblNinguno;
            ViewState.Remove("addSucursal");
            btnEditar.Enabled = false;
            btnBorrar.Enabled = false;
            btnNuevo.Enabled = false;
        }
        else
        {
            lblAviso.Text = "Debe de seleccionar una franquicia para agregar sucursales";
            mpeAvisos.Show();
        }
    }

    private void fDesHabilitarDatosFiscales()
    {
        ddlSerie.Enabled = false;
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
        //txtNombreMostrar.Enabled = false;
        fupLogo.Enabled = false;
        //ddlRfc.Enabled = false;
       // txtIdSucursal.Enabled = false;
        txtNumTienda.Enabled = false;
    }

    private void fHabilitarDatosFiscales()
    {
        ddlSerie.Enabled = true;
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
        //txtNombreMostrar.Enabled = true;
        fupLogo.Enabled = true;
        //ddlRfc.Enabled = true;
        //txtIdSucursal.Enabled = true;
        txtNumTienda.Enabled = true;
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

    //protected void btnAgregarTienda_Click(object sender, EventArgs e)
    //{
    //    gDAL = new clsOperacionSucursales();
    //    datosUsuario = clsComun.fnUsuarioEnSesion();
    //    string imagen = string.Empty;
    //    DataTable dtMenu = null;
    //    byte[] imagenBytes = new byte[0];
    //    byte[] imagenBytesTicket = new byte[0];
    //    string sIndentificador = Convert.ToString(ViewState["updNodoPrincipal"]);
        
    //    if (!string.IsNullOrEmpty(sIndentificador))//Convert.ToString(ViewState["updNodoPrincipal"])))//Si variable se edita el nodo principal
    //    {
    //        if (fupimgPlantillaLogo.HasFile)
    //        {
    //            if (Path.GetExtension(fupimgPlantillaLogo.FileName).ToLower() != ".jpg")
    //            {
    //                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
    //                lblAviso.Text = Resources.resCorpusCFDIEs.VarLogo;
    //                mpeAvisos.Show();
    //                return;
    //            }

    //            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
    //            System.Web.HttpPostedFile mifichero = fupimgPlantillaLogo.PostedFile;
    //            double dTamañoArchivo = mifichero.ContentLength / 1024;
    //            System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupimgPlantillaLogo.FileContent);
    //            if (dTamañoArchivo > psMaximo)
    //            {
    //                //El tamaño máximo del logo es de 1MB
    //                //clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
    //                lblAviso.Text = Resources.resCorpusCFDIEs.VarErrorLogo;
    //                mpeAvisos.Show();
    //                return;
    //            }

    //            imagenBytes = fupimgPlantillaLogo.FileBytes;
    //        }

    //        if (fupImgTicket.HasFile)
    //        {
    //            if (Path.GetExtension(fupImgTicket.FileName).ToLower() != ".jpg")
    //            {
    //                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
    //                lblAviso.Text = Resources.resCorpusCFDIEs.VarLogo;
    //                mpeAvisos.Show();
    //                return;
    //            }

    //            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
    //            System.Web.HttpPostedFile mifichero = fupImgTicket.PostedFile;
    //            double dTamañoArchivo = mifichero.ContentLength / 1024;
    //            System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupImgTicket.FileContent);
    //            if (dTamañoArchivo > psMaximo)
    //            {
    //                //El tamaño máximo del logo es de 1MB
    //                //clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
    //                lblAviso.Text = Resources.resCorpusCFDIEs.VarErrorLogo;
    //                mpeAvisos.Show();

    //                return;
    //            }

    //            imagenBytesTicket = fupImgTicket.FileBytes;
    //        }

    //        //gDAL.fnActualizaTienda(txtTienda.Text, Convert.ToInt32(hdnSelVal.Value), imagenBytes, imagenBytesTicket);
    //        //Actualizar Regla de Negocio
    //        //if (!string.IsNullOrEmpty(hdIdEstructura.Value)) //insertamos la relacion de la regla de negocio
    //        //    gDAL.fnGuardarRelacionRegla(hdIdEstructura.Value, ddlReglaNegocio.SelectedValue, null);
    //        ViewState.Remove("updNodoPrincipal");
    //    }
    //    else
    //    {

    //        //int nRel = gDAL.fnGuardarTienda(txtTienda.Text, Convert.ToInt32(txtIdSucursal.Text));
    //        //insertamos la relacion del usuario y la estructura
    //        //gDAL.fnGuardarRelUsuarioEstructura(datosUsuario.id_usuario, nRel);
    //        //Insertamos la regla de Negocio
    //       // gDAL.fnGuardarRelacionRegla(null, ddlReglaNegocio.SelectedValue, Convert.ToString(nRel));
    //        //dtMenu = clsComun.fnObtenerMenu(nRel);
    //        if (fupimgPlantillaLogo.HasFile)
    //        {
    //            if (Path.GetExtension(fupimgPlantillaLogo.FileName).ToLower() != ".jpg")
    //            {
    //                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
    //                lblAviso.Text = Resources.resCorpusCFDIEs.VarLogo;
    //                mpeAvisos.Show();
    //                return;
    //            }

    //            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
    //            System.Web.HttpPostedFile mifichero = fupimgPlantillaLogo.PostedFile;
    //            double dTamañoArchivo = mifichero.ContentLength / 1024;
    //            System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupimgPlantillaLogo.FileContent);
    //            if (dTamañoArchivo > psMaximo)
    //            {
    //                //El tamaño máximo del logo es de 1MB
    //                //clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
    //                lblAviso.Text = Resources.resCorpusCFDIEs.VarErrorLogo;
    //                mpeAvisos.Show();
    //                return;
    //            }

    //            imagenBytes = fupimgPlantillaLogo.FileBytes;
    //        }

    //        if (fupImgTicket.HasFile)
    //        {
    //            if (Path.GetExtension(fupImgTicket.FileName).ToLower() != ".jpg")
    //            {
    //                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
    //                lblAviso.Text = Resources.resCorpusCFDIEs.VarLogo;
    //                mpeAvisos.Show();
    //                return;
    //            }

    //            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
    //            System.Web.HttpPostedFile mifichero = fupImgTicket.PostedFile;
    //            double dTamañoArchivo = mifichero.ContentLength / 1024;
    //            System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupImgTicket.FileContent);
    //            if (dTamañoArchivo > psMaximo)
    //            {
    //                //El tamaño máximo del logo es de 1MB
    //                //clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
    //                lblAviso.Text = Resources.resCorpusCFDIEs.VarErrorLogo;
    //                mpeAvisos.Show();
    //                return;
    //            }

    //            imagenBytesTicket = fupImgTicket.FileBytes;
    //        }

    //        //Se guarda el menú en la tabla
    //        //clsComun.fnAgregarMenuLateral(txtTienda.Text, imagenBytes, nRel, imagenBytesTicket);

    //    }//Fin Condicion Guardar Edidtar

    //    fnSeleccionarMensaje(string.IsNullOrEmpty(sIndentificador), true);
    //    fnLimpiarPantalla();
    //    fDesHabilitarDatosFiscales();
    //    immImagenMostrar.Visible = false;
    //    lblVerNombreMostrar.Visible = false;
    //    btnNuevo.Text = Resources.resCorpusCFDIEs.lblNuevoCorreo;
    //    btnNuevo.Enabled = true;
    //    btnBorrar.Enabled = true;
    //    btnEditar.Enabled = true;
    //    trvEstructura.Enabled = true;
    //    tblEstrucutra = gDAL.fnObtenerEstructuraUsuario(datosUsuario.id_usuario);
    //    trvEstructura.Nodes.Clear();
    //    fnCrearNodos(0, null, tblEstrucutra, 0);
    //    trvEstructura.ExpandAll();
    //    ViewState.Remove("addSucursal");
    //    //btnAgregarTienda.Text = Resources.resCorpusCFDIEs.lblAgregar;
    //}

    //protected void mpeNewTienda_Load(object sender, EventArgs e)
    //{
    //    if(!IsPostBack)
    //        fnCargarReglaNegocio();
    //}

    //protected void ddlReglaNegocio_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    mpeNewTienda.Show();
    //}

    protected void btnCancelarTienda_Click(object sender, EventArgs e)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        gDAL = new clsOperacionSucursales();
        fnBorrarNodoSeleccionado();
        tblEstrucutra = gDAL.fnObtenerEstructuraUsuario(datosUsuario.id_usuario);
        trvEstructura.Nodes.Clear();
        fnCrearNodos(0, null, tblEstrucutra, 0);
        trvEstructura.ExpandAll();
        btnBorrar.Enabled = true;
        btnEditar.Enabled = true;
        btnNuevo.Enabled = true;
        //btnAgregar.Enabled = true;
        trvEstructura.Enabled = true;
        sender = trvEstructura;
        //trvEstructura_SelectedNodeChanged(sender, e);
        fnLimpiarPantalla();
        //btnAgregar.Enabled = false;
        //txtNombreNodo.Enabled = false;
        fDesHabilitarDatosFiscales();
        immImagenMostrar.Visible = false;
        lblVerNombreMostrar.Visible = false;
        //btnNuevo.Text = Resources.resCorpusCFDIEs.lblNuevoCorreo;
        ViewState.Remove("addSucursal");
        ViewState.Remove("updNodoPrincipal");
    }
    protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarFolio();
    }
}

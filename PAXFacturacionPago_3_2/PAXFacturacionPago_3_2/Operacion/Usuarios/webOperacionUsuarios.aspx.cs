using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

public partial class Operacion_Usuarios_webOperacionUsuarios : System.Web.UI.Page
{
    private clsOperacionUsuarios gOp;
    DataTable tblEstrucutra;
    TreeView tree;
    public string SeleccioneUnValor = "(Seleccione perfil)";
    clsInicioSesionUsuario datosUsuario;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            tblEstrucutra = new DataTable();
            fnCargaPerfiles();
            clsComun.fnPonerTitulo(this);
            fnCargarSucursales();
            ViewState["Editar"] = 0;
            btnBorrar.OnClientClick = "return confirm('" + Resources.resCorpusCFDIEs.varBorrarNodo + "');";

            gOp = new clsOperacionUsuarios();

            try
            {
                DataTable sdrInfo = gOp.fnObtenerDatosUsuario();

                if (sdrInfo != null && sdrInfo.Rows.Count > 0)
                {
                    ViewState["idEstructuraPrincipal"] = sdrInfo.Rows[0]["id_usuario"].ToString();
                    tblEstrucutra = gOp.fnObtenerEstructura(sdrInfo.Rows[0]["id_usuario"].ToString());
                    fnCrearNodos(0, null, tblEstrucutra);
                    trvEstructura.Nodes[0].Selected = true;
                    lblSelVal.Text = trvEstructura.Nodes[0].Text;
                    hdnSel.Value = trvEstructura.Nodes[0].Text;
                    hdnValuePath.Value = trvEstructura.Nodes[0].ValuePath;
                    hdnSelVal.Value = trvEstructura.Nodes[0].Value;
                }

                txtNombre.Enabled = false;
                txtCorreo.Enabled = false;
                txtUsuario.Enabled = false;
                ddlPerfil.Enabled = false;
                ddlSucursales.Enabled = false;
                GrvModulos.Enabled = false;
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
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        //Buscar Clave existente
        clsInicioSesionSolicitudReg registro = new clsInicioSesionSolicitudReg();

        if (registro.buscarUsuario(txtNombreNodo.Text).Rows.Count == 0)
        {

            if (!string.IsNullOrEmpty(txtNombreNodo.Text) && !string.IsNullOrEmpty(trvEstructura.Nodes[0].Text))
            {

                txtNombre.Enabled = true;
                txtCorreo.Enabled = true;
                //txtUsuario.Enabled = true;
                ddlPerfil.Enabled = true;
                ddlSucursales.Enabled = true;
                GrvModulos.Enabled = true;
                txtNombreNodo.Enabled = false;
                btnAgregar.Enabled = false;
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
                            //
                            DataTable tblEstructura = new DataTable();
                            DataRow[] rowEstructura;
                            tblEstructura = (DataTable)ViewState["tblEstructura"];
                            rowEstructura = tblEstructura.Select("id_estructura=" + trvEstructura.SelectedNode.Value);
                            gOp = new clsOperacionUsuarios();
                            DataTable dtUsuario = new DataTable();
                            int idUsuario = Convert.ToInt32(trvEstructura.SelectedNode.Value);
                            dtUsuario = gOp.fnObtenerInfoBasicaUsuario(idUsuario);                    
                            //
                            clsInicioSesionSolicitudReg reg = new clsInicioSesionSolicitudReg();
                            retVal =  reg.actualizaEstadoActual(Convert.ToString(dtUsuario.Rows[0]["clave_usuario"]), Convert.ToString(dtUsuario.Rows[0]["email"]), 'B');
                             
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
            fnCargarSucursales();
            fnCargaPerfiles();
            GrvModulos.DataSource = null;
            GrvModulos.DataBind();

            if (hdnValuePath.Value != string.Empty)
            {

                int nfacGenerados = 0;
                List<TreeNode> lisParent = Find(hdnSel.Value);

            foreach (TreeNode item in lisParent)
            {  
            if (item.Value == ViewState["nodoPadre"].ToString())
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEdNodPrinc);
                return;
            }

            
            gOp = new clsOperacionUsuarios();
            //Obtenemos el ID de la fila seleccionada
            
            int id_estructura = Convert.ToInt32(hdnSelVal.Value);
               nfacGenerados = gOp.fnObtenerComprobantesUsuario(id_estructura);
               if (nfacGenerados <= 0)
               {
                   txtNombre.Enabled = true;
                   txtCorreo.Enabled = true;
                   //txtUsuario.Enabled = true;
                   ddlPerfil.Enabled = true;
                   ddlSucursales.Enabled = true;
                   GrvModulos.Enabled = true;

               }
               else
               {
                   txtNombre.Enabled = false;
                   txtCorreo.Enabled = true;
                   txtUsuario.Enabled = false;
                   ddlPerfil.Enabled = false;
                   ddlSucursales.Enabled = false;
                   GrvModulos.Enabled = false;
                   clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarCorreoGeneracion);
               }
               

            if (!string.IsNullOrEmpty(hdnSel.Value))
            {
                DataTable tblEstructura = new DataTable();
                DataRow[] rowEstructura;
                tblEstructura = (DataTable)ViewState["tblEstructura"];
                rowEstructura = tblEstructura.Select("id_estructura=" + trvEstructura.SelectedNode.Value);
                ViewState["Editar"] = 1;
                try
                {

                        gOp = new clsOperacionUsuarios();
                        DataTable dtUsuario = new DataTable();
                        int idUsuario = Convert.ToInt32(trvEstructura.SelectedNode.Value);
                        dtUsuario = gOp.fnObtenerInfoUsuario(idUsuario);
                        txtNombre.Text = Convert.ToString(dtUsuario.Rows[0]["nombre"]);
                        txtUsuario.Text = Convert.ToString(dtUsuario.Rows[0]["clave_usuario"]);
                        txtCorreo.Text = Convert.ToString(dtUsuario.Rows[0]["email"]);
                        ddlSucursales.SelectedValue = Convert.ToString(dtUsuario.Rows[0]["id_estructura"]);
                        ViewState["id_contribuyente"] = Convert.ToInt32(dtUsuario.Rows[0]["id_contribuyente"]); 
                        ddlPerfil.SelectedValue = Convert.ToString(dtUsuario.Rows[0]["id_perfil"]);
                        ddlPerfil_SelectedIndexChanged(sender, null);
                        
                        foreach (GridViewRow renglon in GrvModulos.Rows)
                        {
                            CheckBox CbCan;

                            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                            CbCan.Checked = false;
                            Label sIdModulo = ((Label)renglon.FindControl("lblidmodulo"));
                            DataRow[] GridMod;
                            GridMod = dtUsuario.Select("id_modulo= " + Convert.ToInt32(sIdModulo.Text));
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
            gOp = new  clsOperacionUsuarios();
            fnBorrarNodoSeleccionado();
            tblEstrucutra = gOp.fnObtenerEstructura(ViewState["idEstructuraPrincipal"].ToString());
            trvEstructura.Nodes.Clear();
            fnCrearNodos(0, null, tblEstrucutra);
            trvEstructura.ExpandAll();
            fnLimpiarPantalla();
            btnBorrar.Enabled = true;
            btnEditar.Enabled = true;
            btnAgregar.Enabled = true;
            trvEstructura.Enabled = true;
            GrvModulos.DataSource = null;
            GrvModulos.DataBind();
            ddlPerfil.SelectedValue = "";
            ddlSucursales.SelectedIndex = 0;
            btnAgregar.Enabled = false;
            txtNombreNodo.Enabled = false;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }
    protected void btnGuardarActualizar_Click(object sender, EventArgs e)
    {
        //validaciones
        if (string.IsNullOrEmpty(txtUsuario.Text)            
            || string.IsNullOrEmpty(txtNombre.Text)           
            || string.IsNullOrEmpty(txtCorreo.Text)
            || string.IsNullOrEmpty(ddlPerfil.SelectedValue)
            )
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }

        //Asignacion de variables
        string sOrigen = string.Empty;
        string strMensaje = string.Empty;
        string stxtNombre = string.Empty;
        string stxtUsuario = string.Empty;
        string stxtCorreo = string.Empty;
        string sPassword = string.Empty;

        stxtNombre = txtNombre.Text;
        stxtUsuario = txtUsuario.Text;
        stxtCorreo = txtCorreo.Text;

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

            sPassword = GeneradorPassword.GetPassword();

            //Obtenemos el ID de la fila seleccionada
            string id_estructura = hdnSelVal.Value;

            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "GenerarLlavesContribuyentes" + "|" + "Crea la contraseña al nuevo contribuyente.");

            int Editar = Convert.ToInt32(ViewState["Editar"]);
            if (Editar == 0)
            {
            //Buscar Clave existente
            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "buscarClaveExistente" + "|" + "Revisa que no exista el usuario.");
            if (registro.buscarClaveExistente(stxtUsuario.Trim(), stxtCorreo.Trim()) == 0)
            {

           

                //Guardar valores en BD
                clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "solicitudRegistroContribuyente" + "|" + "Registra el nuevo contribuyente" + "|" + stxtNombre + "|" + stxtUsuario);


                //int idUsuario = gOp.fnRegistroContribuyente(stxtNombre, stxtUsuario, stxtCorreo, Utilerias.Encriptacion.Classica.Encriptar(sPassword), trvEstructura.Nodes[0].Value, "C");
                int idUsuario = gOp.fnRegistroContribuyente(stxtNombre, stxtUsuario, PAXCrypto.CryptoAES.EncriptaAES(stxtCorreo), PAXCrypto.CryptoAES.EncriptaAES(sPassword), trvEstructura.Nodes[0].Value, "C");

                    if (idUsuario != 0)
                    {
                        //Generar mensaje a enviar.
                        strMensaje = "<table>";
                        strMensaje = strMensaje + "<tr><td><b>Al Contribuyente:</b></td><td>Se le ha enviado un correo para continuar con el registro, presione el link que se muestra a continuación.</td></tr>";
                        strMensaje = strMensaje + "<tr><td><b>Usuario:</b></td><td>" + stxtUsuario + "</td></tr>";
                        strMensaje = strMensaje + "<tr><td><b>Contraseña temporal:</b></td><td>" + sPassword + "</td></tr>";
                        strMensaje = strMensaje + "</table>";

                        gOp.fnInsertaRelacionUsuarioEstructura(idUsuario, Convert.ToInt32(ddlSucursales.SelectedValue));
                        datosUsuario = clsComun.fnUsuarioEnSesion();
                       

                        foreach (GridViewRow renglon in GrvModulos.Rows)
                        {
                            CheckBox CbCan;

                            CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                            if (CbCan.Checked)
                            {
                                Label sIdModulo = ((Label)renglon.FindControl("lblidmodulo"));
                                int pIdModulo = Convert.ToInt32(sIdModulo.Text);
                                gOp.fnInsertaRelacionPerfilUsuarioModulo(idUsuario, Convert.ToInt32(ddlPerfil.SelectedValue), pIdModulo);
                                int pIdModuloPadre = gOp.fnSeleccionaModulosPadre(pIdModulo);
                                gOp.fnInsertaRelacionPerfilUsuarioModulo(idUsuario, Convert.ToInt32(ddlPerfil.SelectedValue), pIdModuloPadre);
                            }
                        }

                        //Enviar correo.
                        clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "EnviarCorreo" + "|" + "Envia correo a contribuyente" + "|" + stxtCorreo);
                        if (sendEmail.EnviarCorreo(stxtCorreo, Resources.resCorpusCFDIEs.msgRegistroCon, strMensaje + " " + clsComun.ObtenerParamentro("urlHostCosto")))
                        {
                            //Response.Redirect("webInicioSesionCorrecto.aspx?tpResult=" + sOrigen);

                            fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), true);

                            fnBorrarNodoSeleccionado();
                            tblEstrucutra = gOp.fnObtenerEstructura(ViewState["idEstructuraPrincipal"].ToString());
                            trvEstructura.Nodes.Clear();
                            fnCrearNodos(0, null, tblEstrucutra);
                            trvEstructura.ExpandAll();

                        }
                        else
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCorreo, Resources.resCorpusCFDIEs.varContribuyente);
                        }

                        fnCargarSucursales();
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
                    gOp.fnEliminarEstructuraUsuario(idUsuario);
                    gOp.fnInsertaRelacionUsuarioEstructura(idUsuario, Convert.ToInt32(ddlSucursales.SelectedValue));
                    int nId_Contribuyente = Convert.ToInt32(ViewState["id_contribuyente"]);
                    gOp.fnActualizaUsuarioInfo(idUsuario, stxtNombre, nId_Contribuyente,stxtUsuario, PAXCrypto.CryptoAES.EncriptaAES(stxtCorreo));
                    foreach (GridViewRow renglon in GrvModulos.Rows)
                    {
                        CheckBox CbCan;

                        CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                        if (CbCan.Checked)
                        {
                            Label sIdModulo = ((Label)renglon.FindControl("lblidmodulo"));
                            int pIdModulo = Convert.ToInt32(sIdModulo.Text);
                            gOp.fnInsertaRelacionPerfilUsuarioModulo(idUsuario, Convert.ToInt32(ddlPerfil.SelectedValue), pIdModulo);

                            int pIdModuloPadre = gOp.fnSeleccionaModulosPadre(pIdModulo);
                            gOp.fnInsertaRelacionPerfilUsuarioModulo(idUsuario, Convert.ToInt32(ddlPerfil.SelectedValue), pIdModuloPadre);

                            fnSeleccionarMensaje(string.IsNullOrEmpty(hdIdEstructura.Value), true);

                            fnBorrarNodoSeleccionado();
                            tblEstrucutra = gOp.fnObtenerEstructura(ViewState["idEstructuraPrincipal"].ToString());
                            trvEstructura.Nodes.Clear();
                            fnCrearNodos(0, null, tblEstrucutra);
                            trvEstructura.ExpandAll();
                        }
                    }
                }

            btnBorrar.Enabled = true;
            btnEditar.Enabled = true;
            trvEstructura.Enabled = true;
            txtNombreNodo.Enabled = false;
            btnAgregar.Enabled = false;
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
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        fnLimpiarPantalla();
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
        btnBorrar.Enabled = true;
        btnEditar.Enabled = true;
        btnAgregar.Enabled = true;
        trvEstructura.Enabled = true;

        btnCancelar_Click(sender, e);
        GrvModulos.DataSource = null;
        GrvModulos.DataBind();
        txtNombreNodo.Enabled = false;
        btnAgregar.Enabled = false;
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

    private void fnBorrarNodoSeleccionado()
    {
        lblSelVal.Text = Resources.resCorpusCFDIEs.lblNinguno;
        hdnSel.Value = string.Empty;
        hdnValuePath.Value = string.Empty;
        txtNombreNodo.Text = string.Empty;
    }

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

    /// <summary>
    /// Vacía todos los controles del formulario
    /// </summary>
    private void fnLimpiarPantalla()
    {
        try
        {
            txtNombre.Enabled = false;
            txtCorreo.Enabled = false;
            txtUsuario.Enabled = false;
            ddlPerfil.Enabled = false;
            ddlSucursales.Enabled = false;
            GrvModulos.Enabled = false;

            clsComunCatalogo.fnLimpiarFormulario(pnlFormulario);
            //ddlPais.SelectedIndex = 0;
            //ddlPais_SelectedIndexChanged(ddlPais, null);
            //ddlEstado.SelectedIndex = 0;
        }
        finally
        {
            btnCancelar.Visible = false;
            hdIdEstructura.Value = string.Empty;
            //gdvSucursales.SelectedIndex = -1;
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
                    "webOperacionUsuarios",
                    "fnGuardarUsuario",
                    "Se agrego un usuario nuevo con los datos:",
                     txtNombre.Text,
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
                     txtNombre.Text,
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

    /// <summary>
    /// Trae la lista de sucursales activas asignadas al usuario y las carga en el GridView
    /// </summary>
    private void fnCargarSucursales()
    {
        try
        {
            DataTable tblEstructura = new DataTable();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            tblEstructura = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true);
            ddlSucursales.DataSource = tblEstructura;
            ViewState["tblEstructura"] = tblEstructura;
            ddlSucursales.DataBind();
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

    private void fnCargaPerfiles()
    {
        try
        {
            clsOperacionUsuarios gUsu = new clsOperacionUsuarios();
            DataTable dtPerfiles = new DataTable();
            dtPerfiles = gUsu.fnCargaPerfiles();
            ddlPerfil.DataSource = dtPerfiles;
            ddlPerfil.DataBind();      
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }
    protected void GrvModulos_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void GrvModulos_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
  

    protected void AgregaOpcionSeleccione(System.Object sender, System.EventArgs e)
    {
        ((DropDownList)sender).Items.Insert(0, new ListItem(SeleccioneUnValor, string.Empty));
    }
 
    protected void ddlPerfil_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
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
           
            foreach (GridViewRow renglon in GrvModulos.Rows)
            {
                CheckBox CbCan;
                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                CbCan.Checked = true;
                
            }
        }
        catch (Exception ex)
        {
            GrvModulos.DataSource = null;
            GrvModulos.DataBind();
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        btnNCancelar_Click(sender, e);
        txtNombreNodo.Enabled = true;
        btnAgregar.Enabled = true;
    }
}

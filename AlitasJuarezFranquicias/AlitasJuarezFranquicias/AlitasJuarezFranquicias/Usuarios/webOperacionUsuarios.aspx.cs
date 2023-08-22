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
    public string sSelSucursal = "(Seleccione Sucursal)";
    clsInicioSesionUsuario datosUsuario;
    string relSuc;

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
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varPermisos);
                    lblAviso.Text = Resources.resCorpusCFDIEs.varPermisos;
                    mpeAvisos.Show();
                    Response.Redirect("~/Default.aspx");
                }

            }
            if (!IsPostBack)
            {

                tblEstrucutra = new DataTable();
                fnCargaPerfiles();
                //clsComun.fnPonerTitulo(this);
                fnCargarSucursales();
                //fnCargarSucursales();
                ViewState["Editar"] = 0;
                //btnBorrar.OnClientClick = "return confirm('" + Resources.resCorpusCFDIEs.varBorrarNodo + "');";

                gOp = new clsOperacionUsuarios();

                try
                {
                    //SqlDataReader sdrInfo = gOp.fnObtenerDatosUsuario();
                    clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
                    //if (sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read())   
                    //{
                    ViewState["idEstructuraPrincipal"] = usuarioActivo.id_usuario.ToString();
                    ViewState["relacionSucursales"] = string.Empty;
                    string relSuc = string.Empty;
                    tblEstrucutra = gOp.fnObtenerEstructura(usuarioActivo.id_usuario.ToString());
                    fnCrearNodos(0, null, tblEstrucutra);
                    trvEstructura.Nodes[0].Selected = true;
                    lblSelVal.Text = trvEstructura.Nodes[0].Text;
                    hdnSel.Value = trvEstructura.Nodes[0].Text;
                    hdnValuePath.Value = trvEstructura.Nodes[0].ValuePath;
                    hdnSelVal.Value = trvEstructura.Nodes[0].Value;
                    //}
                    txtNombre.Enabled = false;
                    txtPassword.Enabled = false;
                    txtConfirmarPassword.Enabled = false;
                    txtUsuario.Enabled = false;
                    ddlPerfil.Enabled = false;
                    ddlSucursales.Enabled = false;
                    txtCorreo.Enabled = false;
                    txtTelefono.Enabled = false;
                    //ddlSucursales.Enabled = false;
                    grvSucursales.Enabled = false;
                    GrvModulos.Enabled = false;
                    txtNombreNodo.Enabled = false;
                    btnAgregar.Enabled = false;
                    chkCambioContrasena.Visible = false;
                    imgFlecha.Visible = false;
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
            else
            {
                //imgFlecha.Visible = false;
            }
          
        }
        catch (Exception ex)
        { clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion); }
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
                btnCancelar.Enabled = true;
                txtNombre.Enabled = true;
                txtPassword.Enabled = true;
                txtConfirmarPassword.Enabled = true;
                txtCorreo.Enabled = true;
                txtTelefono.Enabled = true;
                txtUsuario.Enabled = true;
                ddlPerfil.Enabled = true;
                //ddlSucursales.Enabled = true;
                //ddlSucursales.Enabled = true;
                //grvSucursales.Enabled = true;
                GrvModulos.Enabled = true;
                txtNombreNodo.Enabled = false;
                btnAgregar.Enabled = false;

                NewPasswordRequired.ValidationGroup = "RegisterUserValidationGroup";
                NewPasswordRequired.ToolTip = Resources.resCorpusCFDIEs.lblContraseniaRequerida;
                ConfirmNewPasswordRequired.ValidationGroup = "RegisterUserValidationGroup";

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
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAgregaNodoUsuarios);
                lblAviso.Text = Resources.resCorpusCFDIEs.varAgregaNodoUsuarios;
                mpeAvisos.Show();
            }
        }
        else
        {
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);
            lblAviso.Text = Resources.resCorpusCFDIEs.msgClaveExistente;
            mpeAvisos.Show();
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
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBoNodPrinc);
                        lblAviso.Text = Resources.resCorpusCFDIEs.varBoNodPrinc;
                        mpeAvisos.Show();
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
                                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNoAgregar);
                                lblAviso.Text = Resources.resCorpusCFDIEs.varNoAgregar;
                                mpeAvisos.Show();
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
                              
                                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varBaja);
                                lblAviso.Text = Resources.resCorpusCFDIEs.varBaja;
                                mpeAvisos.Show();

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
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
                            lblErrorLog.Text = Resources.resCorpusCFDIEs.varErrorBaja;
                            mpeErrorLog.Show();
                        }
                    }
                }
                txtNombreNodo.Enabled = false;
                btnAgregar.Enabled = false;
            }
            else
            {
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAgregaNodoUsuarios);
                lblAviso.Text = Resources.resCorpusCFDIEs.varAgregaNodoUsuarios;
                mpeAvisos.Show();
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
            clsInicioSesionSolicitudReg registro = new clsInicioSesionSolicitudReg();
            string stxtUsuario = string.Empty;
            string stxtCorreo = string.Empty;
            stxtUsuario = txtUsuario.Text;
            stxtCorreo = txtCorreo.Text;
            if (registro.buscarClaveExistente(stxtUsuario.Trim(), stxtCorreo.Trim()) == 0)
            {
                btnNuevo.Enabled = false;
                btnBorrar.Enabled = false;
                btnEditar.Enabled = false;
                btnCancelar.Enabled = true;
                ddlSucursales.Enabled = false;
                ddlPerfil.Enabled = false;
                grvSucursales.Enabled = false;
                GrvModulos.Enabled = false;
                trvEstructura.Enabled = true;
                NewPasswordRequired.ValidationGroup = null;
                ConfirmNewPasswordRequired.ValidationGroup = null;
            }
            else
            {
                ddlSucursales.Enabled = true;
                trvEstructura.Enabled = true;
                ddlPerfil.Enabled = true;
                grvSucursales.Enabled = true;
                GrvModulos.Enabled = true;
       

            }

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
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varEdNodPrinc);
                lblAviso.Text = Resources.resCorpusCFDIEs.varEdNodPrinc;
                mpeAvisos.Show();
                return;
            }

           

            gOp = new clsOperacionUsuarios();
            //Obtenemos el ID de la fila seleccionada
            
            int id_estructura = Convert.ToInt32(hdnSelVal.Value);
            //fnCargarSucursales(id_estructura);
               nfacGenerados = gOp.fnObtenerComprobantesUsuario(id_estructura);
               if (nfacGenerados <= 0)
               {
                   txtNombre.Enabled = true;
                   txtUsuario.Enabled = true;
                   txtTelefono.Enabled = true;
                   txtCorreo.Enabled = true;
                   ddlPerfil.Enabled = true;
                   ddlSucursales.Enabled = true;
                   grvSucursales.Enabled = true;
                   GrvModulos.Enabled = true;
                   chkCambioContrasena.Visible = true;
                   chkCambioContrasena.Enabled = true;
                   pChkCambio.Visible = true;
                        

               }
               else
               {
                   txtNombre.Enabled = false;
                   
                   txtUsuario.Enabled = false;
                   txtTelefono.Enabled = false;
                   txtCorreo.Enabled = false;
                   ddlPerfil.Enabled = false;
                   ddlSucursales.Enabled = false;
                   GrvModulos.Enabled = false;
                   grvSucursales.Enabled = false;
                   chkCambioContrasena.Visible = false;
                   //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarCorreoGeneracion);
                   lblAviso.Text = Resources.resCorpusCFDIEs.VarCorreoGeneracion;
                   mpeAvisos.Show();
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
                        txtTelefono.Text = Convert.ToString(dtUsuario.Rows[0]["telefono"]);
                        ViewState["id_contribuyente"] = Convert.ToInt32(dtUsuario.Rows[0]["id_contribuyente"]); 
                        ddlPerfil.SelectedValue = Convert.ToString(dtUsuario.Rows[0]["id_perfil"]);
                        ddlPerfil_SelectedIndexChanged(sender, null);
                        //DataTable dtIdEstructuras = gOp.fnObtenerIdsEstructuraUsuario(idUsuario);

                        //foreach (GridViewRow renglon in grvSucursales.Rows)
                        //{

                        //    CheckBox cbCan;
                        //    cbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                        //    cbCan.Checked = false;
                        //    Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                        //    int nIdSucursal = Convert.ToInt32(sIdSucursal.Text);
                        //    foreach (DataRow row in dtIdEstructuras.Rows)
                        //    {
                        //        int idEstructura = Convert.ToInt32(row["id_estructura"]);
                        //        if (idEstructura == nIdSucursal)
                        //        {
                        //            cbCan.Checked = true;

                        //        }
                        //    }
                        //}

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
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAgregaNodoUsuarios);
                lblAviso.Text = Resources.resCorpusCFDIEs.varAgregaNodoUsuarios;
                mpeAvisos.Show();
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
            trvEstructura.Enabled = true;
            fnCrearNodos(0, null, tblEstrucutra);
            trvEstructura.ExpandAll();
            fnLimpiarPantalla();
            btnNuevo.Enabled = true;
            btnBorrar.Enabled = true;
            btnEditar.Enabled = true;
            btnAgregar.Enabled = true;
            trvEstructura.Enabled = true;
            GrvModulos.DataSource = null;
            GrvModulos.DataBind();
            grvSucursales.DataSource = null;
            grvSucursales.DataBind();
            ddlPerfil.SelectedValue = "";
            //ddlSucursales.SelectedIndex = 0;
            grvSucursales.Enabled = false;
            
            btnAgregar.Enabled = false;
            txtNombreNodo.Enabled = false;
            chkCambioContrasena.Visible = false;
            pChkCambio.Visible = false;
            chkCambioContrasena.Checked = false;

            hdnSel.Value = string.Empty;
            hdnSelVal.Value = string.Empty;
            hdnValuePath.Value = string.Empty;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    protected void btnGuardarActualizar_Click(object sender, EventArgs e)
    {

        clsInicioSesionSolicitudReg registro = new clsInicioSesionSolicitudReg();
        int Editar = Convert.ToInt32(ViewState["Editar"]);
        gOp = new clsOperacionUsuarios();
        //validaciones
        if (string.IsNullOrEmpty(txtUsuario.Text)            
            || string.IsNullOrEmpty(txtNombre.Text)
            
            || string.IsNullOrEmpty(ddlPerfil.SelectedValue)
            )
        {
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError);
            lblAviso.Text = Resources.resCorpusCFDIEs.varValidacionError;
            mpeAvisos.Show();
            return;
        }

       
        

        //Asignacion de variables
        string sOrigen = string.Empty;
        //string strMensaje = string.Empty;
        string stxtNombre = string.Empty;
        string stxtUsuario = string.Empty;


        string stxtCorreo = string.Empty;
        string sPassword = string.Empty;
        string sConfirmarPassword = string.Empty;
        string stxtTelefono = string.Empty;

        stxtNombre = txtNombre.Text;
        stxtUsuario = txtUsuario.Text;
        stxtCorreo = txtCorreo.Text;
        stxtTelefono = txtTelefono.Text;
        sPassword = txtPassword.Text;
        sConfirmarPassword = txtConfirmarPassword.Text;

        if (Editar == 1)
        {
            DataTable dtSucursales = new DataTable();
            int id_usuario = registro.buscarUsuarioExistente(stxtUsuario.Trim(), stxtCorreo.Trim());
            dtSucursales = gOp.fnObtenerSucursalesUsuario(id_usuario);
            if (dtSucursales.Rows.Count == 0)
            {
                lblAviso.Text = Resources.resCorpusCFDIEs.lblAsignarSucursal;
                mpeAvisos.Show();
                return;
            }
        }

        try
        {
            if (!clsComun.fnValidaExpresion(stxtCorreo, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                //clsComun.fnMostrarError(this, Resources.resCorpusCFDIEs.txtCorreo);
                lblErrorLog.Text = Resources.resCorpusCFDIEs.txtCorreo;
                mpeErrorLog.Show();
                return;
            }

            int cont = 0;
            foreach (GridViewRow renglon in GrvModulos.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                if (CbCan.Checked)
                {
                    cont++;
                }
            }
            if (cont == 0)
            {
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAsignarModulo);
                lblAviso.Text = Resources.resCorpusCFDIEs.varAsignarModulo;
                mpeAvisos.Show();
                return;
            }
                
                registro = new clsInicioSesionSolicitudReg();
                

                
                if (Editar == 0)
                {
                    if( !string.IsNullOrEmpty(txtPassword.Text))
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
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valConNewConf);
                            lblAviso.Text = Resources.resCorpusCFDIEs.valConNewConf;
                            mpeAvisos.Show();
                            return;
                        }
                    }
                    else
                    {
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valConfirmaNueva);
                        lblAviso.Text = Resources.resCorpusCFDIEs.valConfirmaNueva;
                        mpeAvisos.Show();
                        return;
                    }
                    //Buscar Clave existente
                    clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "buscarClaveExistente" + "|" + "Revisa que no exista el usuario.");
                    if (registro.buscarClaveExistente(stxtUsuario.Trim(), stxtCorreo.Trim()) == 0)
                    {
                        //Guardar valores en BD
                        clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, this.Title + "|" + "solicitudRegistroContribuyente" + "|" + "Registra el nuevo contribuyente" + "|" + stxtNombre + "|" + stxtUsuario);
                        int idUsuario = gOp.fnRegistroContribuyente(stxtNombre, stxtUsuario, stxtCorreo, Utilerias.Encriptacion.Base64.EncriptarBase64(sPassword), trvEstructura.Nodes[0].Value, "P",stxtTelefono);
                        if (idUsuario != 0)
                        {
                            datosUsuario = clsComun.fnUsuarioEnSesion();

                            foreach (GridViewRow renglon in grvSucursales.Rows)
                            {
                                CheckBox CbCan;

                                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                                if (CbCan.Checked)
                                {
                                    Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                                    int pIdSucursal = Convert.ToInt32(sIdSucursal.Text);
                                    //gOp.fnInsertaRelacionUsuarioEstructura(idUsuario, pIdSucursal);
                                }
                            }

                            DataTable dtSucursales = (DataTable)ViewState["tblSucursalesUsuario"];


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

                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varUsuarioRegistrado);
                            lblAviso.Text = Resources.resCorpusCFDIEs.varUsuarioRegistrado;
                           
                            txtPassword.Enabled = false;
                            txtConfirmarPassword.Enabled = false;
                            NewPasswordRequired.ValidationGroup = null;
                            ConfirmNewPasswordRequired.ValidationGroup = null;
                           
                            imgFlecha.Visible = true;
                            trvEstructura.Enabled = true;
 
                            mpeAvisos.Show();
                            fnCargarSucursales();
                            fnCargaPerfiles();
                            GrvModulos.DataSource = null;
                            GrvModulos.DataBind();
                            grvSucursales.DataSource = null;
                            grvSucursales.DataBind();
                            fnCargarSucursales();
                        }
                        else
                        {
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);
                            lblAviso.Text = Resources.resCorpusCFDIEs.msgClaveExistente;
                            mpeAvisos.Show();
                        }
                    }
                    else
                    {
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varContribuyente);
                        lblAviso.Text = Resources.resCorpusCFDIEs.msgClaveExistente;
                        mpeAvisos.Show();
                    }
                }
                else
                {

                    //Obtenemos el ID de la fila seleccionada
                    int idUsuario = Convert.ToInt32(hdnSelVal.Value);
                    int nId_Contribuyente = Convert.ToInt32(ViewState["id_contribuyente"]);
                    if(!string.IsNullOrEmpty(sPassword))
                        sPassword = Utilerias.Encriptacion.Base64.EncriptarBase64(txtPassword.Text);

                    gOp.fnActualizaUsuarioInfo(idUsuario, stxtNombre, nId_Contribuyente, stxtUsuario, stxtCorreo, stxtTelefono,sPassword);
                    //Se eliminan las relaciones con módulos y estructuras para insertar las nuevas.
                    gOp.fnEliminarModulosUsuario(idUsuario);
                    //gOp.fnEliminarEstructuraUsuario(idUsuario);
                    
                    foreach (GridViewRow renglon in grvSucursales.Rows)
                    {
                        CheckBox CbCan;

                        CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));

                        if (CbCan.Checked)
                        {
                            
                            Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                            int pIdSucursal = Convert.ToInt32(sIdSucursal.Text);
                            //gOp.fnInsertaRelacionUsuarioEstructura(idUsuario, pIdSucursal);
                        }
                    }

                    string relSuc = ViewState["relacionSucursales"].ToString();
                    DataTable dtSucursales = (DataTable)ViewState["tblSucursalesUsuario"];
                    
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
                            
                        }
                    }
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgClaveExistente, Resources.resCorpusCFDIEs.varUsuarioRegistrado);
                    lblAviso.Text = Resources.resCorpusCFDIEs.varCambio;
                    imgFlecha.Visible = false;
                    mpeAvisos.Show();
                }
                datosUsuario = clsComun.fnUsuarioEnSesion();
                tblEstrucutra = gOp.fnObtenerEstructura(datosUsuario.id_usuario.ToString());
               
                trvEstructura.Nodes.Clear();
                fnCrearNodos(0, null, tblEstrucutra);
                trvEstructura.ExpandAll();
                btnBorrar.Enabled = true;
                btnNuevo.Enabled = true;
                
                txtNombreNodo.Enabled = false;
                btnAgregar.Enabled = false;
                chkCambioContrasena.Enabled = false;
                chkCambioContrasena.Checked = false;
                if (Editar == 1)
                {
                    fnLimpiarPantalla();
                    ddlPerfil.SelectedValue = "";
                    trvEstructura.Enabled = true;
                    btnEditar.Enabled = true;
                    GrvModulos.DataSource = null;
                    GrvModulos.DataBind();
                    pChkCambio.Visible = false;
                    grvSucursales.DataSource = null;
                    grvSucursales.DataBind();
                    
                }
                else
                {
                    TreeNodeCollection nodes = trvEstructura.Nodes[0].ChildNodes;
                    trvEstructura.Nodes[0].ChildNodes[nodes.Count - 1].Select();
                    //lblSelVal.Text = trvEstructura.SelectedNode.Text;
                    //hdnSel.Value = trvEstructura.SelectedNode.Text;
                    //hdnSelVal.Value = trvEstructura.SelectedNode.Value;
                    //hdnValuePath.Value = trvEstructura.SelectedNode.ValuePath;
                    trvEstructura_SelectedNodeChanged(trvEstructura, e);
                    btnEditar_Click(sender, e);

                }

                fnCargarSucursales();

                //Response.Redirect("~/Usuarios/webOperacionUsuarios.aspx", true);
            
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
        int idUsuario = Convert.ToInt32(tree.SelectedNode.Value);
        gOp = new clsOperacionUsuarios();
        DataTable dtIdEstructuras = gOp.fnObtenerIdsEstructuraUsuario(idUsuario);

        foreach (GridViewRow renglon in grvSucursales.Rows)
        {
            CheckBox cbCan;
            cbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
            cbCan.Checked = false;
            Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
            int nIdSucursal = Convert.ToInt32(sIdSucursal.Text);
            foreach (DataRow row in dtIdEstructuras.Rows)
            {
                int idEstructura = Convert.ToInt32(row["id_estructura"]);
                if (idEstructura == nIdSucursal)
                {
                    cbCan.Checked = true;
                    break;
                }
            }
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
            txtConfirmarPassword.Enabled =false;
            txtPassword.Enabled = false;
            txtUsuario.Enabled = false;
            ddlPerfil.Enabled = false;
            ddlSucursales.Enabled = false;
            txtNombre.Text = "";
            txtUsuario.Text = "";
            txtPassword.Text = "";
            txtConfirmarPassword.Text = "";
            txtTelefono.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Enabled = false;
            grvSucursales.Enabled = false;
            GrvModulos.Enabled = false;
            grvSucursales.Enabled = false;
            foreach (GridViewRow renglon in grvSucursales.Rows)
            {

                CheckBox cbCan;
                cbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                cbCan.Checked = false;
            }
            //clsComunCatalogo.fnLimpiarFormulario(pnlFormulario);
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
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                lblAviso.Text = Resources.resCorpusCFDIEs.varAlta;
                mpeAvisos.Show();

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionUsuarios",
                    "fnGuardarUsuario",
                    "Se agrego un usuario nuevo con los datos:",
                     txtNombre.Text,
                     txtUsuario.Text,
                     //txtCorreo.Text
                     ""
                    );
            }
            else
            {
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);
                lblAviso.Text = Resources.resCorpusCFDIEs.varCambio;
                mpeAvisos.Show();

                clsComun.fnNuevaPistaAuditoria(
                    "webOperacionUsuarios",
                    "fnGuardarUsuario",
                    "Se modificó el usuario con id: " + hdIdEstructura.Value + " con los datos:",
                     txtNombre.Text,
                     txtUsuario.Text,
                     //txtCorreo.Text
                     ""
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
    //        grvSucursales.DataSource = tblEstructura;
    //        ViewState["tblEstructura"] = tblEstructura;
    //        grvSucursales.DataBind();
    //    }
    //    catch (SqlException ex)
    //    {
    //        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

    //    }
      
    //} 

    private void fnCargarSucursales()
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            DataTable dtAuxiliar = clsComun.fnLlenarSucursales(datosUsuario.id_usuario, 0);
            ddlSucursales.DataSource = dtAuxiliar;
            ViewState["tblEstructura"] = dtAuxiliar;
            ddlSucursales.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
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
        trvEstructura.Enabled = false;
        txtNombreNodo.Enabled = true;
        btnAgregar.Enabled = true;
    }

    protected void chkCambioContrasena_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCambioContrasena.Checked == true)
        {
            txtPassword.Enabled = true;
            txtConfirmarPassword.Enabled = true;
            NewPasswordRequired.ValidationGroup = "RegisterUserValidationGroup";
            NewPasswordRequired.ToolTip = Resources.resCorpusCFDIEs.valContraseniaNueva;
            ConfirmNewPasswordRequired.ValidationGroup = "RegisterUserValidationGroup";
        }
        else
        {
            txtPassword.Enabled = false;
            txtConfirmarPassword.Enabled = false;
            NewPasswordRequired.ValidationGroup = null;
            ConfirmNewPasswordRequired.ValidationGroup = null;
        }
    }

    protected void ddlSucursales_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccioneSucursal(sender, e);
    }

    protected void AgregaOpcionSeleccioneSucursal(System.Object sender, System.EventArgs e)
    {
        ((DropDownList)sender).Items.Insert(0, new ListItem(sSelSucursal, string.Empty));
    }

    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        //llenamos el gridSucursales
        datosUsuario = clsComun.fnUsuarioEnSesion();
        gOp = new clsOperacionUsuarios();

        if (Convert.ToInt32(ddlSucursales.SelectedIndex) > 0)
        {
            grvSucursales.DataSource = clsComun.fnLlenarSucursales(datosUsuario.id_usuario, Convert.ToInt32(ddlSucursales.SelectedValue));
            grvSucursales.DataBind();
            if (!string.IsNullOrEmpty(hdnSelVal.Value))
            {
                int idUsuario = Convert.ToInt32(trvEstructura.SelectedNode.Value);
                DataTable dtIdEstructuras = gOp.fnObtenerIdsEstructuraUsuario(idUsuario);
                foreach (GridViewRow renglon in grvSucursales.Rows)
                {
                    CheckBox cbCan;
                    cbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                    cbCan.Checked = false;
                    Label sIdSucursal = ((Label)renglon.FindControl("lblidsucursal"));
                    int nIdSucursal = Convert.ToInt32(sIdSucursal.Text);
                    foreach (DataRow row in dtIdEstructuras.Rows)
                    {
                        int idEstructura = Convert.ToInt32(row["id_estructura"]);
                        if (idEstructura == nIdSucursal)
                        {
                            cbCan.Checked = true;
                        }
                    }
                }
            }
        }
        else 
        {
            grvSucursales.DataSource = null;
            grvSucursales.DataBind();
        }
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

    protected void cbSeleccion_OnCheckedChanged(object sender, EventArgs e)
    {
        clsInicioSesionSolicitudReg registro = new clsInicioSesionSolicitudReg();
        
        string stxtUsuario = string.Empty;
        string stxtCorreo = string.Empty;
        stxtUsuario = txtUsuario.Text;
        stxtCorreo = txtCorreo.Text;
        int id_usuario;
        int pIdSucursal;
        gOp = new clsOperacionUsuarios();
        
        if (registro.buscarClaveExistente(stxtUsuario.Trim(), stxtCorreo.Trim()) == 1)
        {
             //clsComun.fnUsuarioEnSesion();
            
            id_usuario = registro.buscarUsuarioExistente(stxtUsuario.Trim(), stxtCorreo.Trim());
         
            //id_usuario = registro.buscarClaveExistente(stxtUsuario.Trim(), stxtCorreo.Trim());
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);

            var data = grvSucursales.DataKeys[row.RowIndex];
            Label sIdSucursal = (Label)grvSucursales.Rows[row.RowIndex].FindControl("lblidsucursal");
            pIdSucursal = Convert.ToInt32(sIdSucursal.Text);
            if (((CheckBox)sender).Checked)
            {
               
                gOp.fnInsertaRelacionUsuarioEstructura(id_usuario, pIdSucursal);
               
            }
            else
            {
                gOp.fnEliminarEstructuraUsuarioSucursal(id_usuario, pIdSucursal);
            }
        }
    }
}

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
using System.IO;

public partial class Cuenta_WebOperacionDatosFiscales : System.Web.UI.Page
{
    private clsOperacionSucursales gDAL;
    
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
                txtNombreNodo.Enabled = false;
                btnAgregar.Enabled = false;
                btnActualizarDomicilio.Enabled = false;
                fnCargarPaises();
                fnCargarEstados(ddlPais.SelectedValue);
                fnCargarReglaNegocio();
                fDesHabilitarDatosFiscales();
                fnCargarRfcs();

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
        }catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    private void fnLimpiar()
    {
        txtRazonSocial.Text = string.Empty;
        txtNombreNodo.Text = string.Empty;
        txtIdSucursal.Text = string.Empty;
        //txtRazonSocial.Text = string.Empty;
        txtFranquicia.Text = string.Empty;
        ddlPais.SelectedIndex = 0;
        ddlPais_SelectedIndexChanged(ddlPais, null);
        ddlEstado.SelectedIndex = 0;
        txtMunicipio.Text = string.Empty;
        txtLocalidad.Text = string.Empty;
        txtRegimenFiscal.Text = string.Empty;
        txtCalle.Text = string.Empty;
        txtNoExterior.Text = string.Empty;
        txtNoInterior.Text = string.Empty;
        txtColonia.Text = string.Empty;
        txtCodigoPostal.Text = string.Empty;
        txtReferencia.Text = string.Empty;
        ddlRfc.SelectedIndex = 0;
        ddlReglaNegocio.SelectedIndex = 0;
        lblRfc.Text = Resources.resCorpusCFDIEs.lblRFC;
        lblRegla.Text = Resources.resCorpusCFDIEs.lblReglaNe;

        hdnSelVal.Value = string.Empty;
        hdnValuePath.Value = string.Empty;
    }

    private void fDesHabilitarDatosFiscales()
    {
        txtIdSucursal.Enabled = false;
        //txtRazonSocial.Enabled = false;
        txtFranquicia.Enabled = false;
        ddlPais.Enabled = false;
        ddlEstado.Enabled = false;
        txtMunicipio.Enabled = false;
        txtLocalidad.Enabled = false;
        txtRegimenFiscal.Enabled = false;
        txtCalle.Enabled = false;
        txtNoExterior.Enabled = false;
        txtNoInterior.Enabled = false;
        txtColonia.Enabled = false;
        txtCodigoPostal.Enabled = false;
        fupLogo.Enabled = false;
        ddlRfc.Enabled = false;
        ddlReglaNegocio.Enabled = false;
        fupImgTicket.Enabled = false;
        txtReferencia.Enabled = false;
    }

    private void fHabilitarDatosFiscales()
    {
        txtIdSucursal.Enabled = true;
        //txtRazonSocial.Enabled = true;
        txtFranquicia.Enabled = true;
        ddlPais.Enabled = true;
        ddlEstado.Enabled = true;
        txtMunicipio.Enabled = true;
        txtLocalidad.Enabled = true;
        txtRegimenFiscal.Enabled = true;
        txtCalle.Enabled = true;
        txtNoExterior.Enabled = true;
        txtNoInterior.Enabled = true;
        txtColonia.Enabled = true;
        txtCodigoPostal.Enabled = true;
        fupLogo.Enabled = true;
        ddlRfc.Enabled = true;
        ddlReglaNegocio.Enabled = true;
        fupImgTicket.Enabled = true;
        txtReferencia.Enabled = true;
    }

    /// <summary>
    /// Método encargado de llenar el drop de los países
    /// </summary>
    private void fnCargarPaises()
    {
        ddlPais.DataSource = clsComun.fnLlenarDropPaises();
        ddlPais.DataBind();

        //ddlPais0.DataSource = clsComun.fnLlenarDropPaises();
        //ddlPais0.DataBind();
    }

    /// <summary>
    /// Método encargado de llenar el drop de los estados
    /// </summary>
    /// <param name="psIdPais"></param>
    private void fnCargarEstados(string psIdPais)
    {
        ddlEstado.DataSource = clsComun.fnLlenarDropEstados(psIdPais);
        ddlEstado.DataBind();

        //ddlEstado0.DataSource = clsComun.fnLlenarDropEstados(psIdPais);
        //ddlEstado0.DataBind();
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

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        txtNombreNodo.Enabled = true;
        btnAgregar.Enabled = true;
        trvEstructura.Enabled = false;
        btnBorrar.Enabled = false;
        btnEditar.Enabled = false;
    }

    protected void btnNCancelar_Click(object sender, EventArgs e)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        gDAL = new clsOperacionSucursales();
        fnBorrarNodoSeleccionado();
        tblEstrucutra = gDAL.fnObtenerEstructuraUsuario(datosUsuario.id_usuario);
        trvEstructura.Nodes.Clear();
        fnCrearNodos(0, null, tblEstrucutra, 0);
        trvEstructura.ExpandAll();
        sender = trvEstructura;

        fnLimpiar();
        txtNombreNodo.Text = string.Empty;
        fDesHabilitarDatosFiscales();
        trvEstructura.Enabled = true;
        btnNuevo.Enabled = true;
        btnBorrar.Enabled = true;
        btnEditar.Enabled = true;
        btnActualizarDomicilio.Enabled = false;
        fnBorrarNodoSeleccionado();
        txtNombreNodo.Enabled = false;
        btnAgregar.Enabled = false;
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
            //else // se añade el nuevo nodo al nodo padre.
            //{
            //    //nuevoNodo.ShowCheckBox = true;
            //    //if (dvSucursales.Count == 0) //SI NO CONTIENE RESULTADOS NO PUEDE MODIFICAR NODO
            //    //    nuevoNodo.SelectAction = TreeNodeSelectAction.None; 
            //    nodePadre.ChildNodes.Add(nuevoNodo);

            //}

            // Llamada recurrente al mismo método para agregar los Hijos del Nodo recién agregado.
            fnCrearNodos(Int32.Parse(dataRowCurrent["id_estructura"].ToString()), nuevoNodo, tblEstructura, nid_estructura);
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        txtNombreNodo.Enabled = false;
        fHabilitarDatosFiscales();
        txtIdSucursal.Focus();
        txtFranquicia.Text = txtNombreNodo.Text;
        txtFranquicia.Enabled = false;
        btnActualizarDomicilio.Enabled = true;
        trvEstructura.Enabled = false;
        btnNuevo.Enabled = false;
        btnBorrar.Enabled = false;
        btnEditar.Enabled = false;

        DataTable dtRfcs = new clsOperacionRFC().fnObtenerRFCs();
        if (dtRfcs.Rows.Count > 0)
        {
            txtRazonSocial.Text = dtRfcs.Rows[0]["razon_social"].ToString();
            txtRazonSocial.Enabled = false;
        }
        else
        {
            ListItem lstRfc = new ListItem("<" + Resources.resCorpusCFDIEs.varAgregueRfc + ">", "0");
            ddlRfc.Items.Add(lstRfc);
            ddlRfc.DataBind();
        }

    }

    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sIdPais = (sender as DropDownList).SelectedValue;
        fnCargarEstados(sIdPais);
    }

    protected void btnActualizarDomicilio_Click(object sender, EventArgs e)
    {
        gDAL = new clsOperacionSucursales();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        string imagen = string.Empty;
        DataTable dtMenu = null;
        byte[] imagenBytes = new byte[0];
        byte[] imagenBytesTicket = new byte[0];
        int nRel = 0;
        string sFranquicia = txtFranquicia.Text;
        string sEstructuraCobro = txtIdSucursal.Text;

        string scalle = txtCalle.Text;
        string snumero_exterior = txtNoExterior.Text;
        string snumero_interior = txtNoInterior.Text;
        string scolonia = txtColonia.Text;
        string sreferencia = txtReferencia.Text;
        string slocalidad = txtLocalidad.Text;
        string smunicipio = txtMunicipio.Text;
        int    nid_estado =Convert.ToInt32( ddlEstado.SelectedValue);
        string scodigo_postal = txtCodigoPostal.Text;
        string sregimen_fiscal = txtRegimenFiscal.Text;

        /*Validaciones*/
        if(string.IsNullOrEmpty(scalle)
            ||string.IsNullOrEmpty(smunicipio)
            ||string.IsNullOrEmpty(scodigo_postal))
        {
            lblAviso.Text = "Faltan Campos Obligatorios";
            mpeAvisos.Show();
            return;
        }
    /*@calle			NVARCHAR(50),        
	  @numero_exterior  NVARCHAR(50)= NULL,
	  @numero_interior	NVARCHAR(50)= NULL,
	  @colonia			NVARCHAR(50)= NULL,
	  @referencia		NVARCHAR(50)= NULL,
	  @localidad		NVARCHAR(50)= NULL,
	  @municipio		NVARCHAR(50),        
	  @id_estado		NVARCHAR(50),        
      @codigo_postal	NVARCHAR(50),        
      @regimen_fiscal	NVARCHAR(50)= NULL*/

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

        if (fupImgTicket.HasFile)
        {
            if (Path.GetExtension(fupImgTicket.FileName).ToLower() != ".jpg")
            {
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.VarLogo);
                lblAviso.Text = Resources.resCorpusCFDIEs.VarLogo;
                mpeAvisos.Show();
                return;
            }

            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFileLogo"));
            System.Web.HttpPostedFile mifichero = fupImgTicket.PostedFile;
            double dTamañoArchivo = mifichero.ContentLength / 1024;
            System.Drawing.Image dImg = System.Drawing.Image.FromStream(fupImgTicket.FileContent);
            if (dTamañoArchivo > psMaximo)
            {
                //El tamaño máximo del logo es de 1MB
                //clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.VarErrorLogo));
                lblAviso.Text = Resources.resCorpusCFDIEs.VarErrorLogo;
                mpeAvisos.Show();

                return;
            }

            imagenBytesTicket = fupImgTicket.FileBytes;
        }

        if (!string.IsNullOrEmpty(hdnSelVal.Value))//Actualizamos los datos fiscales
        {

            gDAL.fnActualizaTienda(sFranquicia, Convert.ToInt32(hdnSelVal.Value), imagenBytes, imagenBytesTicket,
                                    scalle, snumero_exterior, snumero_interior, scolonia, sreferencia, slocalidad, smunicipio, nid_estado, scodigo_postal, sregimen_fiscal);
        }
        else //insertamos nuevo
        {
            try
            {
                nRel = gDAL.fnGuardarTienda(sFranquicia, Convert.ToInt32(sEstructuraCobro),
                                            scalle, snumero_exterior, snumero_interior, scolonia, sreferencia, slocalidad, smunicipio, nid_estado, scodigo_postal, sregimen_fiscal);
                //insertamos la relacion del usuario y la estructura
                gDAL.fnGuardarRelUsuarioEstructura(datosUsuario.id_usuario, nRel);

                //Se guarda la relación entre el RFC y la estructura
                clsOperacionRFC opRfc = new clsOperacionRFC();

                int idRfc = Convert.ToInt32(ddlRfc.SelectedValue);
                opRfc.fnAgregarRelacionRfcEstructura(idRfc, nRel);

                //dtMenu = clsComun.fnObtenerMenu(nRel);

                //Se guarda el menú en la tabla
                clsComun.fnAgregarMenuLateral(sFranquicia, imagenBytes, nRel, imagenBytesTicket);
            }
            catch (Exception ex)
            {
                fnLimpiar();
                fDesHabilitarDatosFiscales();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                fnSeleccionarMensaje(string.IsNullOrEmpty(hdnSelVal.Value), false);
            }
        }

        //Actualizar Regla de Negocio
        if (!string.IsNullOrEmpty(hdnSelVal.Value)) //insertamos la relacion de la regla de negocio
            gDAL.fnGuardarRelacionRegla(hdnSelVal.Value, ddlReglaNegocio.SelectedValue, null);
        else//Insertamos la regla de Negocio
            gDAL.fnGuardarRelacionRegla(null, ddlReglaNegocio.SelectedValue, Convert.ToString(nRel));

        fDesHabilitarDatosFiscales();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        tblEstrucutra = gDAL.fnObtenerEstructuraUsuario(datosUsuario.id_usuario);
        trvEstructura.Nodes.Clear();
        fnCrearNodos(0, null, tblEstrucutra, 0);
        trvEstructura.ExpandAll();
        trvEstructura.Enabled = true;
        btnNuevo.Enabled = true;
        lblSelVal.Text = Resources.resCorpusCFDIEs.lblNinguno;

        fnSeleccionarMensaje(string.IsNullOrEmpty(hdnSelVal.Value), true);
        fnLimpiar();
        
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
        btnNuevo.Enabled = false;
        btnBorrar.Enabled = true;
        btnEditar.Enabled = true;
        btnAgregar.Enabled = false;
        btnActualizarDomicilio.Enabled = false;
    }

    protected void btnEditar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnSelVal.Value))
        {
            int nidEstructura = Convert.ToInt32(hdnSelVal.Value);
            DataTable tblEstructura = new DataTable();
            DataRow[] rowEstructura;
            tblEstructura = (DataTable)ViewState["tblEstructura"];

            try
            {
                rowEstructura = tblEstructura.Select("id_estructura=" + trvEstructura.SelectedNode.Value);
                string sRfc = rowEstructura[0]["rfc"].ToString();
                DataTable dtRfcs = new clsOperacionRFC().fnObtenerRFCs();

                if (dtRfcs.Rows.Count > 0)
                {
                    txtRazonSocial.Text = rowEstructura[0]["razon_social"].ToString();
                    txtRazonSocial.Enabled = false;
                }
                else
                {
                    ListItem lstRfc = new ListItem("<" + Resources.resCorpusCFDIEs.varAgregueRfc + ">", "0");
                    ddlRfc.Items.Add(lstRfc);
                    ddlRfc.DataBind();
                }

                string sReglaDeNegocio = string.Empty;
                String sIdEstructura = rowEstructura[0]["id_estructura"].ToString();
                gDAL = new clsOperacionSucursales();
                DataTable tblEstructuraSeleccionada = gDAL.fnObtenerSucursal(sIdEstructura);
                rowEstructura = tblEstructuraSeleccionada.Select("id_estructura=" + trvEstructura.SelectedNode.Value);

                

                if (rowEstructura.Length != 0)
                {
                    DataTable sdrDomicilio = gDAL.fnObtenerTablaDomicilioSuc(Convert.ToInt32(sIdEstructura));

                    //txtNumTienda.Text = rowEstructura[0]["num_tienda"].ToString();
                    txtIdSucursal.Text = rowEstructura[0]["id_estructura_cobro"].ToString();
                    txtFranquicia.Text = rowEstructura[0]["nombre"].ToString();
                    txtMunicipio.Text = sdrDomicilio.Rows[0]["municipio"].ToString();
                    txtLocalidad.Text = sdrDomicilio.Rows[0]["localidad"].ToString();
                    txtCalle.Text = sdrDomicilio.Rows[0]["calle"].ToString();
                    txtNoExterior.Text = sdrDomicilio.Rows[0]["numero_exterior"].ToString();
                    txtNoInterior.Text = sdrDomicilio.Rows[0]["numero_interior"].ToString();
                    txtColonia.Text = sdrDomicilio.Rows[0]["colonia"].ToString();
                    //txtReferencia.Text = sdrDomicilio.Rows[0]["referencia"].ToString();
                    txtCodigoPostal.Text = sdrDomicilio.Rows[0]["codigo_postal"].ToString();
                    txtRegimenFiscal.Text = sdrDomicilio.Rows[0]["regimen_fiscal"].ToString();
                    txtReferencia.Text = sdrDomicilio.Rows[0]["referencia"].ToString();

                    ddlEstado.SelectedIndex = ddlEstado.Items.IndexOf(ddlEstado.Items.FindByText(sdrDomicilio.Rows[0]["estado"].ToString()));
                    ddlPais.SelectedIndex = ddlPais.SelectedIndex = ddlPais.Items.IndexOf(ddlPais.Items.FindByText(sdrDomicilio.Rows[0]["pais"].ToString()));

                    int rfcIndex = ddlRfc.Items.IndexOf(ddlRfc.Items.FindByText(sRfc));
                    int nRegla = ddlReglaNegocio.Items.IndexOf(ddlReglaNegocio.Items.FindByText(sReglaDeNegocio));

                    fHabilitarDatosFiscales();

                    DataRow[] dtRow =  tblEstructura.Select("id_padre=" + sIdEstructura);

                    int nCondicion = 0;

                    for (int c = 0; c < dtRow.Length; c++)
                    {
                        string sid_estructura = dtRow[c]["id_estructura"].ToString();
                        //Validamos que la Sucursal no cuente con comprobantes
                        int nfacGenerados = gDAL.fnBuscarGenerados(sid_estructura);
                        if (nfacGenerados > 0)
                        {
                            nCondicion++;
                        }
                    }
                    //int nfacGenerados = gDAL.fnBuscarGenerados(sid_estructura);
                    if (nCondicion>0)
                    {
                        //txtNumTienda.Enabled = false;
                        ddlRfc.Enabled = false;
                    }
                    else
                    {
                        if (rfcIndex >= 0)
                        {
                            ddlRfc.SelectedIndex = ddlRfc.Items.IndexOf(ddlRfc.Items.FindByText(sRfc));
                            int idRfc = Convert.ToInt32(ddlRfc.SelectedValue);
                            if (new clsOperacionRFC().fnVerificarRfcCfd(idRfc))
                            {
                                ddlRfc.Enabled = false;
                            }
                        }
                        else
                        {
                            ddlRfc.SelectedIndex = 0;
                            //lblRfc.Text = lblRfc.Text + " (" + Resources.resFranquiciasEs.lblRfcEliminado + ")";
                            
                        }
                    }


                    if (nRegla >= 0)
                    {
                        ddlReglaNegocio.SelectedIndex= ddlReglaNegocio.Items.IndexOf(ddlReglaNegocio.Items.FindByText(sReglaDeNegocio));
                        ////ddlRfc.SelectedIndex = ddlRfc.Items.IndexOf(ddlRfc.Items.FindByText(sRfc));
                        //int idRfc = Convert.ToInt32(ddlRfc.SelectedValue);
                        //if (new clsOperacionRFC().fnVerificarRfcCfd(idRfc))
                        //{
                        //    ddlRfc.Enabled = false;
                        //}
                    }
                    else
                    {
                        ddlReglaNegocio.SelectedIndex = 0;
                        //lblRegla.Text = lblRegla.Text +" (" +"Se cambiará por el seleccionado" + ") ";
                        //ddlRfc.SelectedIndex = 0;
                        //lblRfc.Text = lblRfc.Text + " (" + Resources.resCorpusCFDIEs.lblRfcEliminado + ")";
                    }

                    ddlPais_SelectedIndexChanged(ddlPais, null);
                    //fnBorrarNodoSeleccionado();

                    btnNuevo.Enabled = false;
                    btnEditar.Enabled = false;
                    btnBorrar.Enabled = false;
                    trvEstructura.Enabled = false;
                    btnActualizarDomicilio.Enabled = true;

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
            lblAviso.Text = Resources.resCorpusCFDIEs.varAgregaNodo;
            mpeAvisos.Show();
        }
    }

    /// <summary>
    /// Obtiene las reglas de Negocio
    /// </summary>
    private void fnCargarReglaNegocio()
    {
        DataTable dtRfcs = clsComun.llenarRegla();
        if (dtRfcs.Rows.Count > 0)
        {
            ddlReglaNegocio.DataSource = dtRfcs;
            ddlReglaNegocio.DataBind();
        }
        else
        {
            ListItem lstRfc = new ListItem("<" + "Agregar regla de Negocio" + ">", "0");//Resources.resCorpusCFDIEs.varAgregueRfc+
            ddlReglaNegocio.Items.Add(lstRfc);
            ddlReglaNegocio.DataBind();
        }
    }

    /// <summary>
    /// Obtiene los RFC's registrados
    /// </summary>
    private void fnCargarRfcs()
    {
        DataTable dtRfcs = new clsOperacionRFC().fnObtenerRFCs();
        if (dtRfcs.Rows.Count > 0)
        {
            ddlRfc.DataSource = dtRfcs;
            ddlRfc.DataBind();
        }
        else
        {
            ListItem lstRfc = new ListItem("<" + Resources.resCorpusCFDIEs.varAgregueRfc + ">", "0");
            ddlRfc.Items.Add(lstRfc);
            ddlRfc.DataBind();
        }
    }

    /// <summary>
    /// Seleccionar nodo
    /// </summary>
    private void fnBorrarNodoSeleccionado()
    {
        lblSelVal.Text = Resources.resCorpusCFDIEs.lblNinguno;
        hdnSel.Value = string.Empty;
        hdnSelVal.Value = string.Empty;
        hdnValuePath.Value = string.Empty;
        //txtNombreNodo.Text = string.Empty;
    }
    protected void btnBorrar_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(hdnSelVal.Value))
        {
            int indicePadre =Convert.ToInt32(hdnSelVal.Value);
            gDAL = new clsOperacionSucursales();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            tblEstrucutra = gDAL.fnObtenerEstructuraUsuario(datosUsuario.id_usuario);

            // Crear un DataView con los Nodos que dependen del Nodo padre pasado como parámetro.
            DataView dataViewHijos = new DataView(tblEstrucutra);
            dataViewHijos.RowFilter = tblEstrucutra.Columns["id_padre"].ColumnName + " = " + indicePadre;

            if (dataViewHijos.Count > 0)
            {
                lblAviso.Text = Resources.resCorpusCFDIEs.lblEliminarSucursales;
                mpeAvisos.Show();
                return;
            }

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
                    ////Si se estaba editando la estructura, entonces limpiamos los datos del formulario
                    //if (!string.IsNullOrEmpty(hdIdEstructura.Value) && hdIdEstructura.Value.Equals(id_estructura))
                    //    fnLimpiarPantalla();

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
                lblAviso.Text = Resources.resCorpusCFDIEs.varErrorBaja;
                mpeAvisos.Show();
            }

        }
        else
        {
            lblAviso.Text ="Debe seleccionar almenos una nodo";
            mpeAvisos.Show();
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
                    "Se agregó una nuevi nodo principal con los datos",
                    //txtIdSucursal,
                    //txtNombreMostrar,
                    txtFranquicia.Text,
                    txtCalle.Text,
                    txtNoExterior.Text,
                    txtNoInterior.Text,
                    txtColonia.Text,
                    //txtReferencia.Text,
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
                    "Se modificó la sucursal con ID " + hdnSelVal.Value + " con los datos",
                    txtFranquicia.Text,
                    txtCalle.Text,
                    txtNoExterior.Text,
                    txtNoInterior.Text,
                    txtColonia.Text,
                    //txtReferencia.Text,
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
                lblAviso.Text = Resources.resCorpusCFDIEs.varErrorAlta;
                mpeAvisos.Show();
            }
            else
            {
                //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
                lblAviso.Text = Resources.resCorpusCFDIEs.varErrorCambio;
                mpeAvisos.Show();
            }
        }
    }

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

    protected void ddlRfc_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtRfcs = new clsOperacionRFC().fnObtenerRFCs();
        if (dtRfcs.Rows.Count > 0)
        {
            //txtRazonSocial.Text = dtRfcs.Rows[0]["razon_social"].ToString();
            DataRow[] result = dtRfcs.Select("RFC = '" + ddlRfc.SelectedItem.Text + "'");
            txtRazonSocial.Text = result[0].ItemArray[2].ToString();
            txtRazonSocial.Enabled = false;
        }
        else
        {
            ListItem lstRfc = new ListItem("<" + Resources.resCorpusCFDIEs.varAgregueRfc + ">", "0");
            ddlRfc.Items.Add(lstRfc);
            ddlRfc.DataBind();
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
}
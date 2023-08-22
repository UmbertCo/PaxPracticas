using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utilerias.SQL;
using System.Text;
using PAXRecepcionProveedores.App_Code.GeneradorE_MAIL;
using System.Threading;
using System.Globalization;

namespace PAXRecepcionProveedores.Operacion.Comprobantes
{
    public partial class webConsultaComprobantes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
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
                            Response.Redirect("~/Default.aspx",false);

                        if (new clsOperacionUsuarios().fnVerificarUsuarioPerfil(sesUsuario.id_usuario, "P"))
                        {
                            ViewState["esProveedor"] = true;
                        }
                    }
                    //fnLlenaEmisores(dsFiltros.Tables[0]);
                    //fnLlenaReceptores(dsFiltros.Tables[1]);
                    fnLlenaVersiones();
                    //fnLlenaUsuarios(dsFiltros.Tables[3]);
                    fnLlenaFechas();
                    fnLlenarSucursales();
                    ViewState["dtStatus"] = new clsOperacionComprobantes().fnObtenerStatus();
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/webGlobalError.aspx");
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
                dtSucursales = new clsOperacionSucursales().fnObtenerSucursalesUsuario(usuarioActivo.id_usuario);
            }
            gvSucursales.DataSource = dtSucursales;
            gvSucursales.DataBind();
            gvSucursales.Visible = true;
        }
        private void fnLlenaFechas()
        {
            txtFechaInicio.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
            
            ddlVersion.Items.Add(new ListItem("3.2"));
            ddlVersion.Items.Add(new ListItem("2.2"));
            ddlVersion.SelectedIndex =0;
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
                btnAnterior.Visible = false;
                fnBuscarComprobantes(1);
                ViewState["paginado"] = 1;
            }
            catch (Exception ex)
            {
            }
        }

        private void fnBuscarComprobantes(int nPagina)
        {
            //Verifica que la fecha inicial sea menor que la final
            DataTable dtResultado = new DataTable("dtResultadoComprobantes");
            if (DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture) >
                DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varFechaMayor);
            else
            {
                clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
                int nTipo = 0;
                string sRfcEmisor = string.Empty;
                string sRfcReceptor = string.Empty;
                string sVersion = string.Empty;
                string sLista = null;
                sRfcEmisor = txtRfcEmisor.Text;
                sRfcReceptor = txtRfcReceptor.Text;
                if (!ddlVersion.SelectedValue.Equals("--"))
                    sVersion = ddlVersion.SelectedValue;

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


                nTipo = Convert.ToInt32(rbTipoBusqueda.SelectedValue);
                //realiza la busqueda
                int nLinPag = Convert.ToInt32(ddlLineasPagina.SelectedItem.ToString());
                if (Convert.ToBoolean(ViewState["esProveedor"]))
                {
                    dtResultado = new clsOperacionComprobantes().fnBuscarComprobantesProveedorPag(DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), sRfcEmisor, sRfcReceptor, sVersion, usuarioActivo.id_usuario, nTipo, sLista, nPagina, nLinPag);
                    gvResultado.AutoGenerateEditButton = false;
                }
                else
                {
                    dtResultado = new clsOperacionComprobantes().fnBuscarComprobantesPag(DateTime.ParseExact(txtFechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(txtFechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), sRfcEmisor, sRfcReceptor, sVersion, usuarioActivo.id_usuario, nTipo, sLista, nPagina, nLinPag);
                }
                fnCambiarStatusCultura(dtResultado);
                gvResultado.AutoGenerateColumns = false;
                gvResultado.DataSource = dtResultado;
                gvResultado.DataBind();

                if (gvResultado.Rows.Count < nLinPag)
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
            if (nIdStatus == 5 && ddlStatus.Enabled)
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
            
            gvResultado.EditIndex = e.NewEditIndex;
            int nPagina = Convert.ToInt32(ViewState["paginado"]);
            fnBuscarComprobantes(nPagina);

            DropDownList ddlStatus = gvResultado.Rows[e.NewEditIndex].FindControl("ddlStatus") as DropDownList;
            ddlStatus.DataSource = fnObtenerStatus();//(DataTable)ViewState["dtStatus"];
            ddlStatus.DataTextField = "nombre";
            ddlStatus.DataValueField = "id_status";
            ddlStatus.DataBind();
            TextBox txtFechaPago = gvResultado.Rows[e.NewEditIndex].FindControl("txtFechaPago") as TextBox;
            string sFechaPago = ((DataTable)gvResultado.DataSource).Rows[e.NewEditIndex]["fecha_pago"].ToString();
            if(!string.IsNullOrEmpty(sFechaPago))
                txtFechaPago.Text = Convert.ToDateTime(sFechaPago).ToString("dd/MM/yyyy");
           
            ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(((DataTable)gvResultado.DataSource).Rows[e.NewEditIndex]["status"].ToString()));
            if (ddlStatus.SelectedValue.ToString() == "3")
            {
                //Si el estatus es 'Pagado', deshabilita la selección de estatus.
                ddlStatus.Enabled = false;
            }
            else if (ddlStatus.SelectedValue.ToString() == "5")
            {
                
                txtFechaPago.Enabled = true;
                imgFin.Visible = true;

            }
            if (!((CheckBox)gvResultado.Rows[e.NewEditIndex].FindControl("chbValido")).Checked)
            {
                //Si el comprobante es inválido, elimina los estatus de 'Pagado', 'Contabilizado' y 'Programado'
                ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("3"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("4"));
                ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("5"));
            }
            ViewState["id_comprobante"] = Convert.ToInt32(((DataTable)gvResultado.DataSource).Rows[e.NewEditIndex]["id_comprobante"]);
            btnAnterior.Enabled = false;
            btnSiguiente.Enabled = false;
        }

        private DataTable fnObtenerStatus()
        {
            DataTable dtStatus = (DataTable)ViewState["dtStatus"];
            foreach(DataRow drStatus in dtStatus.Rows)
            {
                drStatus["nombre"] = fnStatusCultura(drStatus["nombre"].ToString());
               
            }
            return dtStatus;
        }

        private DataTable fnCambiarStatusCultura(DataTable dtResultado)
        {
            foreach (DataRow drResultado in dtResultado.Rows)
            {
                drResultado["status"] = fnStatusCultura(drResultado["status"].ToString());

            }
            return dtResultado;
        }
        private string fnStatusCultura(string sStatus)
        {

            switch (sStatus)
            {
                case "Recibida":
                    sStatus = Resources.resCorpusCFDIEs.varRecibida;
                    break;
                case "Rechazada":
                    sStatus = Resources.resCorpusCFDIEs.varRechazada;
                    break;
                case "Pagada":
                    sStatus = Resources.resCorpusCFDIEs.varPagada;
                    break;
                case "Contabilizada":
                    sStatus = Resources.resCorpusCFDIEs.varContabilizada;
                    break;
                case "Programada":
                    sStatus = Resources.resCorpusCFDIEs.varProgramada;
                    break;
                default:
                    break;
            }
            return sStatus;
        }
        protected void gvResultado_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DropDownList ddlStatus = gvResultado.Rows[e.RowIndex].FindControl("ddlStatus") as DropDownList;
            TextBox txtFechaPago = gvResultado.Rows[e.RowIndex].FindControl("txtFechaPago") as TextBox;
            int nIdStatus = Convert.ToInt32(ddlStatus.SelectedValue);
            int nIdComprobante = Convert.ToInt32(ViewState["id_comprobante"]);
            string sFechaPago = string.Empty;
            if (nIdStatus == 5)
            {
                //Si el estatus es 'Programado', obtiene la fecha
                sFechaPago = txtFechaPago.Text;
            }
            new clsOperacionComprobantes().fnCambiarStatus(nIdComprobante, nIdStatus,sFechaPago);
            if (nIdStatus != 1)
            {
                //Si el estatus no es 'Recibido', envia un acuse por correo
                new clsEnviaAcusePago().fnEnviarAcuse(nIdComprobante);
            }
            ViewState.Remove("id_comprobante");
            gvResultado.EditIndex = -1;
            int nPagina = Convert.ToInt32(ViewState["paginado"]);
            fnBuscarComprobantes(nPagina);
            btnAnterior.Enabled = true;
            btnSiguiente.Enabled = true;
        }

        protected void gvResultado_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ViewState.Remove("id_comprobante");
            gvResultado.EditIndex = -1;
            int nPagina = Convert.ToInt32(ViewState["paginado"]);
            fnBuscarComprobantes(nPagina);
            btnAnterior.Enabled = true;
            btnSiguiente.Enabled = true;
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            int pagina = Convert.ToInt32(ViewState["paginado"]);
            pagina -= 1;

            if (pagina > 0)
            {
                ViewState["paginado"] = pagina;
                //fnRealizarConsultaPaginado(pagina, 2);
                fnBuscarComprobantes(pagina);
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

        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            int pagina = Convert.ToInt32(ViewState["paginado"]);
            pagina += 1;
            ViewState["paginado"] = pagina;
            //fnRealizarConsultaPaginado(pagina, 1);
            fnBuscarComprobantes(pagina);
            if (pagina > 1)
                btnAnterior.Visible = true;
            else
                btnAnterior.Visible = false;
            //pagina += 1;
            //bool bPaginas = fnVerificarPaginas(pagina);
            //if (bPaginas == true)
            //    btnSiguiente.Visible = true;
            //else
            //    btnSiguiente.Visible = false;
            // btnAnterior.Visible = true;
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
}
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Globalization;
public partial class Operacion_Distribuidores_webOperacionAsignaCreditosDistribuidores : System.Web.UI.Page
{
    private clsOperacionDistribuidores gOd;
    clsInicioSesionUsuario datosUsuario;
    private static DataSet creditos;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
          
            if (!IsPostBack)
            {
                btnGuardar.OnClientClick = "return confirm('" + Resources.resCorpusCFDIEs.varConfirmarGuardarDistribuidor + "');";

                TabContainer1.ActiveTabIndex = 0;

                datosUsuario = clsComun.fnUsuarioEnSesion();
                clsOperacionUsuarios gUs = new clsOperacionUsuarios();
                DataTable dtUs = gUs.fnObtenerInfoUsuario(datosUsuario.id_usuario);
                int IdEstructura = Convert.ToInt32(dtUs.Rows[0]["id_estructura"]);
                fnCargaCreditosDistribuidor(datosUsuario.id_usuario, IdEstructura);
                gOd = new clsOperacionDistribuidores();
                DataTable tblDistribuidor = new DataTable();
                tblDistribuidor = gOd.fnObtieneDistribuidoresporidUsuario(datosUsuario.id_usuario);
                if (tblDistribuidor.Rows.Count > 0)
                {
                    int pidDistribuidor = Convert.ToInt32(tblDistribuidor.Rows[0]["id_distribuidor"]);
                    ViewState["id_distribuidor"] = pidDistribuidor;
                    fnObtieneUsuariosporDistribuidor(pidDistribuidor, null, null, null);
                    lblCodDis.Text = tblDistribuidor.Rows[0]["numero_dist"].ToString();
                }
                else
                    lblCodDis.Text = "--";
            }
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {

        try
        {

            gOd = new clsOperacionDistribuidores();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            clsOperacionUsuarios gUs = new clsOperacionUsuarios();
            DataTable dtUs = gUs.fnObtenerInfoUsuario(datosUsuario.id_usuario);
            int pIdEstructuraDist = Convert.ToInt32(dtUs.Rows[0]["id_estructura"]);

            double CreditosDistribuidor = gOd.fnObtieneCreditosDistribuidor(datosUsuario.id_usuario, pIdEstructuraDist);

            double CreditosAnt = Convert.ToDouble(ViewState["creditos"]);
            //Verificamos si el tipo de acceso es libre o restringido

            
            if (ddlAcceso.SelectedValue == "R")
            {
                foreach (GridViewRow renglon in grdServiciosAsig.Rows)
                {
                    TextBox tbCalculo = (TextBox)renglon.Cells[3].Controls[1];
                    if (tbCalculo.Text != "")
                    {
                        TextBox tbPrecioUnit = (TextBox)renglon.Cells[4].Controls[1];
                        Label tbCreditos = (Label)renglon.Cells[5].Controls[1];
                        int tbServicio = Convert.ToInt32(renglon.Cells[0].Text);
                        double sPrecio = Convert.ToDouble(tbPrecioUnit.Text);
                        double Creditos = Convert.ToDouble(tbCreditos.Text);
                        if (sPrecio > 0 && Creditos > 0)
                        {
                            if (CreditosDistribuidor > 0)
                            {

                                if (CreditosDistribuidor >= Creditos)
                                {

                                    int idUsuario = Convert.ToInt32(ViewState["idUsuario"]);
                                    int idEstructura = Convert.ToInt32(ViewState["idEstructura"]);

                                    DataTable tblDistribuidor = new DataTable();
                                    tblDistribuidor = gOd.fnObtieneDistribuidoresporidUsuario(datosUsuario.id_usuario);
                                    if (tblDistribuidor.Rows.Count > 0)
                                    {
                                        //int pidDistribuidor = Convert.ToInt32(tblDistribuidor.Rows[0]["id_distribuidor"]);
                                        int pidDistribuidor = Convert.ToInt32(ViewState["id_distribuidor"].ToString());
                                        gOd.fnActualizaCreditosUsuariodeDistribuidor(datosUsuario.id_usuario, idUsuario, Creditos, idEstructura, CreditosAnt, "R", pidDistribuidor, pIdEstructuraDist, sPrecio, Convert.ToString(tbServicio));

                                        fnCargaCreditosDistribuidor(datosUsuario.id_usuario, pIdEstructuraDist);
                                        fnObtieneUsuariosporDistribuidor(pidDistribuidor, null, null, null);
                                    }

                                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);

                                }
                                else
                                {
                                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varInsCred);
                                    return;
                                }

                            }
                            else
                            {
                                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblMsgCreditos);
                                return;
                            }
                        }
                        else
                        {
                            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblMsgCreditosNeg);
                            return;
                        }


                    }
                }
            }

            else
            {
                //Actualiza el usuario cuando es el estatus es de tipo libre de creditos
                DataTable tblDistribuidor = new DataTable();
                tblDistribuidor = gOd.fnObtieneDistribuidoresporidUsuario(datosUsuario.id_usuario);
                int idUsuario = Convert.ToInt32(ViewState["idUsuario"]);
                int idEstructuraUs = Convert.ToInt32(ViewState["idEstructura"]);
                if (tblDistribuidor.Rows.Count > 0)
                {
                    int pidDistribuidor = Convert.ToInt32(tblDistribuidor.Rows[0]["id_distribuidor"]);
                    gOd.fnActualizaUsuariodeDistribuidor(pidDistribuidor, idUsuario, CreditosAnt, datosUsuario.id_usuario, "L", idEstructuraUs, pIdEstructuraDist);
                    fnObtieneUsuariosporDistribuidor(pidDistribuidor, null, null, null);
                }
                fnCargaCreditosDistribuidor(datosUsuario.id_usuario, pIdEstructuraDist);
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);
            }
            fnLimpiaCampos();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void btnCancelarRfc_Click(object sender, EventArgs e)
    {
        fnLimpiaCampos();
        for (int i = 0; i < cbServicios.Items.Count; i++)
        {
            cbServicios.Items[i].Selected = false;
        }
    }
    public void fnCargaCreditosDistribuidor(int idUsuario, int idEstructura)
    {
        try
        {
        gOd = new clsOperacionDistribuidores();
        double CreditosDisp = 0;
        CreditosDisp = gOd.fnObtieneCreditosDistribuidor(idUsuario, idEstructura);
        lbCreditosDisponibles.Text = CreditosDisp.ToString("N0");
        
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

    }
    public void fnLimpiaCampos()
    {
        try
        {
             txtUsuario.Text = String.Empty;
             txtemail.Text = String.Empty;
             txtcreditos.Text = String.Empty;          
             txtcreditos.Enabled = false;             
             ddlAcceso.SelectedValue = "R";
             //Borrar Sesiones y ViewStates!!!
             ViewState["idUsuario"] = null;
             ViewState["creditos"] = null;
             ViewState["idEstructura"] = null;
             btnCancelarRfc.Visible = false;
             btnGuardar.Visible = false;
             gdvClientes.SelectedIndex = -1;
             grdServiciosAsig.DataSource = null;
             grdServiciosAsig.Visible = false;
            
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    public void fnCargaCreditosUsuario(int idUsuario, int idEstructura)
    {
        try
        {
            gOd = new clsOperacionDistribuidores();
            DataTable dtCreditosUser = new DataTable();
            dtCreditosUser =gOd.fnObtieneCreditosUsuarioDistribuidor(idUsuario, idEstructura);
            txtcreditos.Text = Convert.ToString(dtCreditosUser.Rows[0]["creditos"]);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

    }
    public void fnObtieneUsuariosporDistribuidor(int pidDistribuidor, string sClave_usuario, string sEmail, string sAcceso )
    {

        try
        {
            gOd = new clsOperacionDistribuidores();
            DataTable dtClientes = new DataTable();
            dtClientes = gOd.fnObtieneClientesporDistribuidor(pidDistribuidor, sClave_usuario, sEmail, sAcceso);
            gdvClientes.DataSource = dtClientes;
            gdvClientes.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

    }
    protected void gdvClientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            gOd = new clsOperacionDistribuidores();
            GridViewRow gvrFila = (GridViewRow)gdvClientes.SelectedRow;
            ViewState["idUsuario"] = ((Label)gvrFila.FindControl("lblidusuario")).Text;
            ViewState["idEstructura"] = ((Label)gvrFila.FindControl("lblidestructura")).Text;
            int idUsuario = Convert.ToInt32(((Label)gvrFila.FindControl("lblidusuario")).Text);
            int idEstructura = Convert.ToInt32(((Label)gvrFila.FindControl("lblidestructura")).Text);
            string Acceso = ((Label)gvrFila.FindControl("lblaccesousuario")).Text;

            if (Acceso == "R")
            {
                lblUsuario3.Visible = true;
                txtcreditos.Visible = true;
                grdServiciosAsig.Visible = true;

            }
            else
            {
                lblUsuario3.Visible = false;
                txtcreditos.Visible = false;
                grdServiciosAsig.Visible = false;

            }

            ViewState["creditos"] = Convert.ToDouble(gvrFila.Cells[6].Text);//Convert.ToDouble(((Label)gvrFila.FindControl("lblclicred")).Text);
            ddlAcceso.SelectedValue = ((Label)gvrFila.FindControl("lblaccesousuario")).Text;
            txtUsuario.Text = ((Label)gvrFila.FindControl("lblclaveusuario")).Text;
            txtemail.Text = ((Label)gvrFila.FindControl("lblemailusuario")).Text;
            btnCancelarRfc.Visible = true;
            btnGuardar.Visible = true;

            fnRecuperaServicios();
            btnActualiza.Visible = true;

            creditos = fnRecuperaCreditosusuario(txtUsuario.Text);


            DataTable tblUsuario = new DataTable();
            DataTable tblCreditos = new DataTable();
            DataTable tblServicios = new DataTable();

            tblUsuario = creditos.Tables[0];
            tblCreditos = creditos.Tables[1];
            tblServicios = creditos.Tables[2];
           
            
            grdServiciosAsig.DataSource = tblServicios;
            grdServiciosAsig.DataBind();

            for (int i = 0; i < cbServicios.Items.Count; i++)
            {
                cbServicios.Items[i].Selected = false;
            }

            for (int i = 0; i < cbServicios.Items.Count; i++)
            {
                foreach (DataRow renglon in tblServicios.Rows)
                {
                    string Descripcion = Convert.ToString(renglon["descripcion"]);
                    string Servicio = Convert.ToString(cbServicios.Items[i].Text);
                    if (Descripcion == Servicio)
                    {
                        cbServicios.Items[i].Selected = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void gdvClientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gOd = new clsOperacionDistribuidores();
            DataTable tblDistribuidor = new DataTable();
            datosUsuario = clsComun.fnUsuarioEnSesion();
            tblDistribuidor = gOd.fnObtieneDistribuidoresporidUsuario(datosUsuario.id_usuario);
            if (tblDistribuidor.Rows.Count > 0)
            {
                gdvClientes.PageIndex = e.NewPageIndex; 
                //int pidDistribuidor = Convert.ToInt32(tblDistribuidor.Rows[0]["id_distribuidor"]);
                int pidDistribuidor = Convert.ToInt32(ViewState["id_distribuidor"].ToString());
                fnObtieneUsuariosporDistribuidor(pidDistribuidor, null, null, null);
            }
                
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void gdvClientes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void ddlAcceso_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAcceso.SelectedValue == "L")
            {

                lblUsuario3.Visible = false;
                txtcreditos.Visible = false;
                grdServiciosAsig.Visible = false;
                
            }
            else
            {
                lblUsuario3.Visible = true;
                txtcreditos.Visible = true;
                grdServiciosAsig.Visible = true;

            }
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void LnkCancelar_Click(object sender, EventArgs e)
    {
        try
        {
            string Cancelar = ((LinkButton)sender).CommandArgument;
            foreach (GridViewRow renglon in gdvClientes.Rows)
            {
                Label idUsuario = (Label)(renglon.FindControl("lblidusuario"));
                Label idEstructura = (Label)(renglon.FindControl("lblidestructura"));
                if (Cancelar == idUsuario.Text)
                {

                    gOd = new clsOperacionDistribuidores();
                    datosUsuario = clsComun.fnUsuarioEnSesion();

                    GridViewRow gvrFila = (GridViewRow)gdvClientes.SelectedRow;
                    DataTable tblcredituser = new DataTable();

                    tblcredituser= gOd.fnObtieneCreditosUsuarioDistribuidor(Convert.ToInt32(idUsuario.Text), Convert.ToInt32(idEstructura.Text));
                    double creditos = Convert.ToDouble(tblcredituser.Rows[0]["creditos"]);
                    DataTable tblDistribuidor = new DataTable();
                    tblDistribuidor = gOd.fnObtieneDistribuidoresporidUsuario(datosUsuario.id_usuario);
                    if (tblDistribuidor.Rows.Count > 0)
                    {
                        //int pidDistribuidor = Convert.ToInt32(tblDistribuidor.Rows[0]["id_distribuidor"]);
                        int pidDistribuidor = Convert.ToInt32(ViewState["id_distribuidor"].ToString());
                        gOd.fnBajaUsuariodeDistribuidor(pidDistribuidor, Convert.ToInt32(idUsuario.Text), creditos, datosUsuario.id_usuario);
                        fnObtieneUsuariosporDistribuidor(pidDistribuidor, null, null, null);
                        fnLimpiaCampos();
                        datosUsuario = clsComun.fnUsuarioEnSesion();
                        clsOperacionUsuarios gUs = new clsOperacionUsuarios();
                        DataTable dtUs = gUs.fnObtenerInfoUsuario(datosUsuario.id_usuario);
                        int IdEstructura = Convert.ToInt32(dtUs.Rows[0]["id_estructura"]);
                        fnCargaCreditosDistribuidor(datosUsuario.id_usuario, IdEstructura);
                    }
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void gdvClientes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
     
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbutton = (LinkButton)e.Row.FindControl("LnkCancelar");
            //lbutton.Attributes.Add("onClick", "return fnValidaConfirmar()");

         // lbutton.Attributes.Add("Onclick", "javascript:return " + Resources.resCorpusCFDIEs.lblEliminarDist + "')");
          lbutton.OnClientClick = "return confirm('" + Resources.resCorpusCFDIEs.lblEliminarDist + "');";
        //    lbutton.Attributes.Add("OnClick", "javascript:return " + "function confirmation() {" + "var agree = confirm('" + Resources.resCorpusCFDIEs.lblEliminarDist + " if (agree) return true; else return false; }')"); 
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
    protected void btnActualiza_Click(object sender, EventArgs e)
    {
        try
        {

            DataSet creditos2 = new DataSet();
            DataTable tblUsuario = new DataTable();
            DataTable tblCreditos = new DataTable();
            DataTable tblServicios = new DataTable();

            creditos2 = fnRecuperaCreditosusuario(txtUsuario.Text);

            tblUsuario = creditos2.Tables[0];
            tblCreditos = creditos2.Tables[1];
            tblServicios = creditos2.Tables[2];

            clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();

            int idEstructura = Convert.ToInt32(tblUsuario.Rows[0]["estructura"]);
            string Servicios = string.Empty;


            for (int i = 0; i < cbServicios.Items.Count; i++)
            {
                if (cbServicios.Items[i].Selected == true)
                {
                    if (Servicios == string.Empty)
                        Servicios = cbServicios.Items[i].Value;
                    else
                        Servicios = Servicios + "," + cbServicios.Items[i].Value;
                }
            }
            if (Servicios == string.Empty)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varServicios);
                return;
            }

            Creditos.fnActualizaServicios(idEstructura, Servicios);
            gdvClientes_SelectedIndexChanged(sender, e);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    private DataSet fnRecuperaCreditosusuario(string clave_usuario)
    {

        DataSet creditos = new DataSet();
        clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();
        try
        {
            creditos = Creditos.fnRecuperaCreditos(clave_usuario);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        return creditos;
    }
    public void fnRecuperaServicios()
    {
        try
        {
            DataSet creditos = new DataSet();
            clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();
            DataTable dtServicios = Creditos.fnObtieneServicios();
            cbServicios.DataSource = dtServicios;
            cbServicios.DataTextField = "descripcion";
            cbServicios.DataValueField = "id_servicios";
            cbServicios.DataBind();

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    public void fnCalcularCreditos()
    {
        try
        {
            clsConfiguracionCreditos Creditos = new clsConfiguracionCreditos();
            double Precio = 0;
            double Operacion = 0;
            string Servicios = string.Empty;
            txtcreditos.Text = "";
            foreach (GridViewRow renglon in grdServiciosAsig.Rows)
            {

                TextBox tbCalculo = (TextBox)renglon.Cells[3].Controls[1];
                //GridViewRow row = ((TextBox)sender).Parent.Parent as GridViewRow;
                Label tbImporte = (Label)renglon.Cells[5].Controls[1]; ;
                int pIdServicio = Convert.ToInt32(renglon.Cells[0].Text);
                Precio = Creditos.fnRecuperaPrecioServicio(pIdServicio);
                if (tbCalculo.Text != "")
                {

                    Operacion = Convert.ToDouble(tbCalculo.Text) * Precio;
                    tbImporte.Text = Operacion.ToString();
                    if (txtcreditos.Text != "")
                    {
                        double creditos = Convert.ToDouble(txtcreditos.Text);
                        double calculo = creditos + Operacion;
                        txtcreditos.Text = Convert.ToString(calculo);
                    }
                    else
                    {
                        txtcreditos.Text = Convert.ToString(Operacion);
                    }
                    lblUsuario3.Visible = true;
                    txtcreditos.Visible = true;

                    TextBox tbPrecioUnitario = (TextBox)renglon.Cells[4].Controls[1];

                    tbPrecioUnitario.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void txtPrecio_TextChanged(object sender, EventArgs e)
    {
        fnCalcularCreditos();
    }
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx", false);
        }

    }
    protected void tbImporte_TextChanged(object sender, EventArgs e)
    {
        fnCalcularCreditos();
    }
    public void fnValidaConfirmar()
    {
        string sb = string.Empty;
        sb = "<script type=\"text/javascript\">";
        sb = sb + "  function confirmation()  ";
        sb = sb + " { ";
        sb = sb + "  var agree = confirm('" + Resources.resCorpusCFDIEs.lblEliminarDist + "');";
        sb = sb + "  if (agree)";
        sb = sb + " return true;";
        sb = sb + " else";
        sb = sb + " return false;";
        sb = sb + " } ";
        sb = sb + " </script> ";

        //lbutton.Attributes.Add("onclick", "javascript:return " + "function confirmation() {" + "var agree = confirm('" + Resources.resCorpusCFDIEs.lblEliminarDist + " if (agree) return true; else return false; }')"); 
    }
    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        string sClave_usuario = string.Empty;
        string sEmail = string.Empty;
        string sAcceso=string.Empty;


        int pidDistribuidor = Convert.ToInt32(ViewState["id_distribuidor"].ToString());
        sClave_usuario=txtUsuarioFiltro.Text;
        sEmail=txtCorreoFiltro.Text;
        sAcceso=ddlAccesoFiltro.SelectedItem.Value;

        if (sAcceso == "T")
            sAcceso = null;

        fnObtieneUsuariosporDistribuidor(pidDistribuidor,sClave_usuario,sEmail,sAcceso);
    }
}
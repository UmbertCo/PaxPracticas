using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Resources;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Serialization;
using System.Drawing;


public partial class Timbrado_webTimbradoGeneracionNomina : System.Web.UI.Page
{

    private clsInicioSesionUsuario datosUsuario;
    private clsOperacionCuenta gDAL  = new clsOperacionCuenta();
    private clsOperacionSucursales gOpeSuc = new clsOperacionSucursales();
    private clsEnvioCorreoDocs cEd;
    private clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();

    private clsConfiguracionCreditos cCc;
    protected DataTable dtCreditos;

    private clsOperacionTimbradoSellado gTimbrado;
    private clsValCertificado gCertificado;

    private string UltimaNominaNoPagada = Resources.resCorpusCFDIEs.varUltimaNominaNoPagada;

   # region PageEvents

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                datosUsuario = clsComun.fnUsuarioEnSesion();
                if (datosUsuario == null)
                    return;

                fnCargarSucursalesFiscal(datosUsuario.id_usuario);
                fnCargarPeriodos();
                fnCargarNominasPagadas();

                if (Session["IdPago"] != null)
                    Session.Remove("IdPago");

                Session["Id_Nomina"] = "0";

                DataTable sdrInfo = gDAL.fnObtenerDatosFiscales();   
                
                if (sdrInfo != null && sdrInfo.Rows.Count > 0)
                {
                    //txtRegimenfiscal.Text = sdrInfo["regimen_fiscal"].ToString();
                    ViewState["rfc_Emisor"] = sdrInfo.Rows[0]["rfc"].ToString();
                    ViewState["razonSocial_Emisor"] = sdrInfo.Rows[0]["razon_social"].ToString();
                }

                ViewState["GuidPathXMLsGen"] = Guid.NewGuid().ToString();
                ViewState["GuidPathZIPsGen"] = Guid.NewGuid().ToString();

                ViewState["nombreDoc"] = string.Empty;
                ViewState["retornoInsert"] = string.Empty;

                txtFechaPago.Text = DateTime.Now.ToShortDateString();

                //Revisar los creditos disponibles.
                cCc = new clsConfiguracionCreditos(); 

                double dCostCred = 0;
                dCostCred = cCc.fnRecuperaPrecioServicio(Convert.ToInt32(clsEnumeraciones.Servicios.Nomina)); //Precio servicio Generación + Timbrado

                dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');
                ViewState["dtCreditos"] = dtCreditos;

                if (dtCreditos.Rows.Count > 0) //Valida créditos de usuario
                {
                    double creditos = 0;
                    creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                    if (creditos == 0)
                    {
                        clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                        dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                        ViewState["dtCreditos"] = dtCreditos;
                        double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                        if (creditos2 == 0)
                        {
                            lblDetailMsg.Visible = false;
                            lblCosCre.Visible = false;
                            lblHeaderMsg.Visible = true;
                            modalCreditos.Show();
                        }
                        else
                        {
                            //Se valida que tenga créditos suficientes
                            if (creditos2 < dCostCred)
                            {
                                lblDetailMsg.Visible = false;
                                lblCosCre.Visible = true;
                                lblHeaderMsg.Visible = false;
                                modalCreditos.Show();
                            }
                        }
                    }
                }
                else //Valida créditos de distribuidor
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    ViewState["dtCreditos"] = dtCreditos;
                    double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                    if (creditos == 0)
                    {
                        lblDetailMsg.Visible = false;
                        lblCosCre.Visible = false;
                        lblHeaderMsg.Visible = true;
                        modalCreditos.Show();
                    }
                    else
                    {
                        //Se valida que tenga créditos suficientes
                        if (creditos < dCostCred)
                        {
                            lblDetailMsg.Visible = false;
                            lblCosCre.Visible = true;
                            lblHeaderMsg.Visible = false;
                            modalCreditos.Show();
                        }
                    }
                }

                fnActualizarLblCreditos();

                DataSet dsCreditos = fnRecuperaCreditosusuario(datosUsuario.userName);

                DataTable tblServicios = new DataTable();
                tblServicios = dsCreditos.Tables[2];
                bool TieneGeneracionNómina = false;
                foreach (DataRow renglon in tblServicios.Rows)
                {

                    string sDescripcion = Convert.ToString(renglon["descripcion"]);
                    if (sDescripcion == "Nómina")
                    {
                        TieneGeneracionNómina = true;
                    }
                }

                if (TieneGeneracionNómina == false)
                {
                    lblDetailMsg.Visible = true;
                    lblCosCre.Visible = false;
                    lblHeaderMsg.Visible = false;
                    modalCreditos.Show();
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        public void Page_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError().GetBaseException();

            if (!string.IsNullOrEmpty(objErr.Message))
            {
                clsErrorLog.fnNuevaEntrada(objErr, clsErrorLog.TipoErroresLog.Datos);
                Server.ClearError();
                Response.Redirect("~/webGlobalError.aspx", false);
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
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
                }
            }
            else
            {
                string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            }
        }

    #endregion

   # region Mail

       protected void btnAceptarCor_Click(object sender, EventArgs e)
        {
            string sMensajeEnvio = string.Empty;
            foreach (GridViewRow Row in gdvPagosNomina.Rows)
            {
                string retornoInsert = string.Empty;
                Literal ltlIdCfd = (Literal)(Row.FindControl("ltlIdCfd"));
                retornoInsert = ltlIdCfd.Text; //idCFDI

                if (retornoInsert != "")
                { 
                    try
                    {
                        string snombreDoc = "General";
                        bool bEnvio = false;                       

                        string sMensaje = string.Empty;
                        sMensaje = "<table>";
                        sMensaje = sMensaje + "<tr><td>" + txtCorreoMsj.Text.Replace("\n", @"<br />") + "</td><td></td></tr>";
                        sMensaje = sMensaje + "</table>";

                        if (sMensaje == string.Empty)
                            sMensaje = "Comprobantes PAX Facturación";

                        string sCC = txtCorreoCC.Text;
                        datosUsuario = clsComun.fnUsuarioEnSesion();

                        if (datosUsuario.email != string.Empty)
                        {
                            txtCorreoEmisor.Text = datosUsuario.email;
                        }

                        string sMailTo = txtCorreoEmisor.Text;

                        cEd = new clsEnvioCorreoDocs();

                        string sCorCli = gdvPagosNomina.DataKeys[Row.RowIndex]["Correo"].ToString();

                        if (!string.IsNullOrEmpty(sCorCli))
                            sMailTo += "," + sCorCli;

                        string sCorCC = string.Empty;
                        if (txtCorreoCC.Text != string.Empty)
                            sCorCC = txtCorreoCC.Text; 

                        Literal ltlXML = (Literal)(Row.FindControl("ltlXML"));
                        string sXML = ltlXML.Text;

                        XmlDocument xDocTimbrado = new XmlDocument();
                        xDocTimbrado.LoadXml(sXML);

                        if (sMensaje == string.Empty)
                            sMensaje = "Comprobantes PAX Facturación";

                        Guid Gid;
                        Gid = Guid.NewGuid();

                        try
                        {
                            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
                            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                            XPathNavigator navEncabezado = xDocTimbrado.CreateNavigator();
                            try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                            catch { snombreDoc = Gid.ToString(); }

                        }
                        catch (Exception ex)
                        {
                            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                        }                   


                        //Verifica si se envia el comprobante en ZIP o no.
                        if (rdlArchivo.SelectedIndex == 1)
                        {

                            bEnvio = cEd.fnPdfEnvioCorreo("Nomina", retornoInsert, "Recibo de Nomina",
                                                clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLsGen"] + "\\" + snombreDoc + ".pdf",
                                                datosUsuario.id_contribuyente, datosUsuario.id_rfc, datosUsuario.color, sMailTo,
                                                "PAXFacturacion", sMensaje, Convert.ToString(ViewState["GuidPathZIPsGen"]),
                                                Convert.ToString(ViewState["GuidPathXMLsGen"]), snombreDoc, sCC);
                        }
                        else
                        {
                            bEnvio = cEd.fnPdfEnvioCorreoSinZIP("Nomina", retornoInsert, "Recibo de Nomina",
                                                clsComun.ObtenerParamentro("RutaDocXmlZips") + ViewState["GuidPathXMLsGen"] + "\\" + snombreDoc + ".pdf",
                                                datosUsuario.id_contribuyente, datosUsuario.id_rfc, datosUsuario.color, sMailTo,
                                                "PAXFacturacion", sMensaje, Convert.ToString(ViewState["GuidPathZIPsGen"]),
                                                Convert.ToString(ViewState["GuidPathXMLsGen"]), snombreDoc, sCC);
                        }

                        if (!bEnvio)
                            sMensajeEnvio += "(" + sMailTo + " " + snombreDoc + ") ";
                           

                        txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;
                        txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;

                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                    }
                }

            }

            if (!string.IsNullOrEmpty(sMensajeEnvio))
            {
                lblAvisoCorreoNoEnviado.Text = Resources.resCorpusCFDIEs.msgErrorEnvioCorreo + "" + sMensajeEnvio;
                lblAvisoCorreoNoEnviado.Visible = true;
            }
            else
            {
                lblAvisoCorreoNoEnviado.Text = string.Empty;
                lblAvisoCorreoNoEnviado.Visible = false;
            }

            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgEnvioCorreo, Resources.resCorpusCFDIEs.varContribuyente);
            mpeEnvioCorreo.Hide();
            cbSeleccionar.Checked = false;
            cbSeleccionar.Visible = false;
            btnConsultar_Click(sender, e);
        }

       protected void btnCancelarCor_Click(object sender, EventArgs e)
       {
            ViewState["nombreDoc"] = string.Empty;
            ViewState["retornoInsert"] = string.Empty;

            cbSeleccionar.Checked = false;
            cbSeleccionar.Visible = false;
            btnConsultar_Click(sender, e);
       }

   #endregion

   # region Creditos

        protected void btnAcepCreditos_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx", false);
        }


        private bool fnActualizarLblCreditos()
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
            cCc = new clsConfiguracionCreditos();
            bool bRespuesta = true;
            double dCostCred = cCc.fnRecuperaPrecioServicio(Convert.ToInt32(clsEnumeraciones.Servicios.Nomina)); //Precio servicio generación + timbrado
            if (dtCreditos.Rows.Count > 0)
            {
                double TCreditos = 0;
                TCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                if (TCreditos == 0)
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                    if (creditos == 0)
                    {
                        lblCredValor.Text = "0";
                        lblHeaderMsg.Visible = false;
                        lblCosCre.Visible = false;
                        lblHeaderMsg.Visible = true;
                        modalCreditos.Show();
                        bRespuesta = false;
                    }
                    else
                    {
                        double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                        lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                        //Se valida que tenga créditos suficientes
                        if (dCreditos < dCostCred)
                        {
                            lblHeaderMsg.Visible = false;
                            lblCosCre.Visible = true;
                            lblHeaderMsg.Visible = false;
                            modalCreditos.Show();
                            bRespuesta = false;
                        }
                    }
                }
                else
                {
                    double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                    lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                    //Se valida que tenga créditos suficientes
                    if (dCreditos < dCostCred)
                    {
                        lblDetailMsg.Visible = false;
                        lblCosCre.Visible = true;
                        lblHeaderMsg.Visible = false;
                        modalCreditos.Show();
                        bRespuesta = false;
                    }
                }
            }
            else
            {
                clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                if (creditos == 0)
                {
                    lblCredValor.Text = "0";
                    lblDetailMsg.Visible = false;
                    lblCosCre.Visible = false;
                    lblHeaderMsg.Visible = true;
                    modalCreditos.Show();
                    bRespuesta = false;
                }
                else
                {
                    double dCreditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                    lblCredValor.Text = dtCreditos.Rows[0]["creditos"].ToString();
                    //Se valida que tenga créditos suficientes
                    if (dCreditos < dCostCred)
                    {
                        lblDetailMsg.Visible = false;
                        lblCosCre.Visible = true;
                        lblHeaderMsg.Visible = false;
                        modalCreditos.Show();
                        bRespuesta = false;
                    }
                }
            }

            return bRespuesta;
        }

        private int fnCheckNoCreditoVSNoNominaCheck()
        {
            int nResultado = 0;
            int iNumTimbrar = 0;
            double dTotalCreditosTimbrar = 0;
            try
            {
                double dCostCred = cCc.fnRecuperaPrecioServicio(Convert.ToInt32(clsEnumeraciones.Servicios.Nomina)); //Precio servicio Generación + Timbrado

                dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');

                foreach (GridViewRow Row in gdvPagosNomina.Rows)
                {
                    CheckBox CbTimbrado = (CheckBox)(Row.FindControl("cbTimbrado"));
                    CheckBox CbCan = (CheckBox)(Row.FindControl("chkSeleccion"));
                    if (CbCan.Checked && !CbTimbrado.Checked)
                    {
                       iNumTimbrar +=1;
                    }
                }

                dTotalCreditosTimbrar = iNumTimbrar * dCostCred;

                if (dTotalCreditosTimbrar > Convert.ToDouble(dtCreditos.Rows[0]["creditos"]))
                    nResultado = Convert.ToInt32(Math.Truncate(Convert.ToDouble(dtCreditos.Rows[0]["creditos"]) / dCostCred));
            }
            catch(Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            }
            return nResultado;
        }

        /// <summary>
        /// Actauliza los creditos disponibles.
        /// </summary>
        private void fnActualizaCreditos()
        {
            DataTable tlbCreditos = new DataTable();
            int idCredito = 0;
            int idEstructura = 0;
            double Creditos = 0;
            int nRetorno = 0;

            tlbCreditos = (DataTable)ViewState["dtCreditos"];

            if (tlbCreditos.Rows.Count > 0)
            {

                idCredito = Convert.ToInt32(tlbCreditos.Rows[0]["id_creditos"]);
                idEstructura = Convert.ToInt32(tlbCreditos.Rows[0]["id_estructura"]);
                Creditos = Convert.ToDouble(tlbCreditos.Rows[0]["creditos"]);

                nRetorno = clsTimbradoFuncionalidad.fnActualizarCreditos(idCredito, idEstructura, Creditos, "N");

                clsTimbradoFuncionalidad.fnActualizarCreditosHistorico(idCredito, idEstructura, Creditos);
                datosUsuario = clsComun.fnUsuarioEnSesion();
                dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), string.Empty, 'A', 'S');
                if (dtCreditos.Rows.Count > 0)
                {
                    fnActualizarLblCreditos();
                }
                else
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                    if (creditos > 0)
                    {
                        fnActualizarLblCreditos();
                    }
                }
            }


        }

    #endregion

   # region FuncionesCargaControles

        /// <summary>
        /// Trae la lista filtrada de las sucursales fiscales.
        /// </summary>
        private void fnCargarSucursalesFiscal(int IdUsuario)
        {
            try
            {
                ddlSucursal.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(IdUsuario); //clsComun.LlenarDropSucursales(true); //
                ddlSucursal.DataBind();
            }

            catch (SqlException ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
                ddlSucursal.DataSource = null;
                ddlSucursal.DataBind();
            }
            catch
            {
                //referencia nula
            }
        }

        /// <summary>
        /// Método que se encarga de cargar los períodos disponibles
        /// </summary>
        private void fnCargarPeriodos()
        {
            clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();
            try
            {
                ddlPeriodos.DataSource = cTimbradoNomina.LlenarDropPeriodos();
                ddlPeriodos.DataBind();
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                ddlPeriodos.DataSource = null;
                ddlPeriodos.DataBind();
            }
        }

        /// <summary>
        /// Método que se encarga de consultar las nóminas pagadas de los ultimos 3 meses
        /// </summary>
        private void fnCargarNominasPagadas()
        {
            try
            {
                clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();
                ddlNominasPagadas.DataSource = cTimbradoNomina.fnConsultaNominasPagadas(Convert.ToInt32(ddlSucursal.SelectedValue),
                                            Convert.ToInt32(ddlPeriodos.SelectedValue));
                ddlNominasPagadas.DataBind();
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                ddlNominasPagadas.DataSource = null;
                ddlNominasPagadas.DataBind();
            }
        }

        /// <summary>
        /// Método que se encarga de llenar el combo de Nominas pagadas con la ultima nómina sin timbrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fnAgregaOpcionSeleccione(System.Object sender, System.EventArgs e)
        {
            ((DropDownList)sender).Items.Insert(0, new ListItem(UltimaNominaNoPagada, "0"));
        }

    #endregion

    protected void hpEditar_Click(object sender, EventArgs e)
    {
        LinkButton btnVer = (LinkButton)sender;
        GridViewRow gvrRegistro = (GridViewRow)btnVer.NamingContainer;

        Session["IdPago"] = ((LinkButton)sender).CommandArgument;
        Session["Id_Nomina"] = gdvPagosNomina.DataKeys[gvrRegistro.DataItemIndex]["Id_Nomina"].ToString();
        Response.Redirect("~/Timbrado/webTimbradoRegistroNomina.aspx");
    }

    protected void hpVer_Click(object sender, EventArgs e)
    {
        LinkButton btnVer = (LinkButton)sender;
        GridViewRow gvrRegistro = (GridViewRow)btnVer.NamingContainer;

        Session["IdPago"] = ((LinkButton)sender).CommandArgument;
        Session["Id_Nomina"] = gdvPagosNomina.DataKeys[gvrRegistro.DataItemIndex]["Id_Nomina"].ToString();
        Response.Redirect("~/Timbrado/webTimbradoRegistroNomina.aspx");
    }

    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        try
        {
            btnGenerar.Enabled = false;
            btnCrear.Enabled = false;

            if (!ddlNominasPagadas.SelectedValue.Equals("0"))
            {
                gdvPagosNomina.DataSource = cTimbradoNomina.fnBusquedaLastNomina(Convert.ToInt32(ddlSucursal.SelectedValue), ddlEstatus.SelectedValue,
                                            Convert.ToInt32(ddlPeriodos.SelectedValue), ddlNominasPagadas.SelectedValue);

                gdvPagosNomina.DataBind();

                btnGenerar.Enabled = true;
            }
            else
            {
                gdvPagosNomina.DataSource = cTimbradoNomina.fnBusquedaNominaSinTimbrar(Convert.ToInt32(ddlSucursal.SelectedValue));
                gdvPagosNomina.DataBind();

                btnCrear.Enabled = true;
            }
            
            cbSeleccionar.Visible = Convert.ToBoolean(gdvPagosNomina.Rows.Count);
            cbSeleccionar.Checked = false;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }    
    }

    protected void btnCrear_Click(object sender, EventArgs e)
    {
        //Revisar si se ha seleccionado algun registro del Grid
        int nCantMarcados = gdvPagosNomina.Rows.Cast<GridViewRow>().Where(x => ((CheckBox)x.FindControl("chkSeleccion")).Checked).Count();
        if (nCantMarcados.Equals(0))
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgSeleccioneRegistrosTimbrado, Resources.resCorpusCFDIEs.varContribuyente);
            return;
        }

        mpeConfirmacionTimbrar.Show();        
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        bool nBandera = true;
        DateTime dFechaPago;
        DateTime dFechaInicioPago;
        DateTime dFechaFinPago;
        decimal nNumeroDiasPagados = 0;
        int nId_Sucursal = 0;
        int nId_Periodo = 0;
        string sEmpleadosError = string.Empty;
        string sEmpleados = string.Empty;
        string sGenerados = string.Empty;
        try
        {
            //Revisar si se ha seleccionado algun registro del Grid
            int nCantMarcados = gdvPagosNomina.Rows.Cast<GridViewRow>().Where(x => ((CheckBox)x.FindControl("chkSeleccion")).Checked).Count();
            if (nCantMarcados.Equals(0))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgSeleccioneRegistrosGeneracion, Resources.resCorpusCFDIEs.varContribuyente);
                return;
            }

            dFechaPago = Convert.ToDateTime("01/01/1990");
            dFechaInicioPago = Convert.ToDateTime("01/01/1990");
            dFechaFinPago = Convert.ToDateTime("01/01/1990");
            nId_Sucursal = Convert.ToInt32(ddlSucursal.SelectedValue);
            nNumeroDiasPagados = 0;
            nId_Periodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            foreach (GridViewRow renglon in gdvPagosNomina.Rows)
            {
                int nId_Pago_Nomina = Convert.ToInt32(gdvPagosNomina.DataKeys[renglon.RowIndex].Values["Id_PagoNomina"].ToString());
                int nId_Nomina = Convert.ToInt32(gdvPagosNomina.DataKeys[renglon.RowIndex].Values["Id_Nomina"].ToString());
                int nId_Empleado = Convert.ToInt32(gdvPagosNomina.DataKeys[renglon.RowIndex].Values["Id_Empleado"].ToString());

                // Modificado 10/06/2014 Ismael Hidalgo
                // Si esta habilitado el control de Generación de Nómina es porque es una nómina timbrado o pagada,
                // por lo que no es necesario revisar si esta timbrada
                CheckBox chkSeleccion = (CheckBox)renglon.FindControl("chkSeleccion");
                try
                {
                    sEmpleados = gdvPagosNomina.Rows[renglon.RowIndex].Cells[2].Text;

                    if (chkSeleccion.Checked)
                    {
                        clsTimbradoNomina cTimgbradoNomina = new clsTimbradoNomina();

                        // Verificamos si el empleado tiene un registro de pago activo sin pagar
                        if (cTimbradoNomina.fnExistePagoNominaPorIdEstructuraIdEmpleado(nId_Sucursal, nId_Empleado))
                        {
                            sEmpleadosError += sEmpleados + ",";
                            continue;
                            //throw new Exception(Resources.resCorpusCFDIEs.varErrorEmpleadoNominaExistente);
                        }
                       
                        nBandera = cTimgbradoNomina.fnGenerarNomina(nId_Pago_Nomina, nId_Sucursal, nId_Periodo,
                                        dFechaPago, dFechaInicioPago, dFechaFinPago, nNumeroDiasPagados);

                        if (nBandera)
                            sGenerados += sEmpleados + ",";

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Ocurrio un error a la hora de Generar una nueva nómina. Empleado {0}, Nomina {1}, Pago {2}. {3}", sEmpleados, nId_Nomina, nId_Pago_Nomina, ex.Message));
                    //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                    //sEmpleadosError +=  sEmpleados + ",";
                }
            }

            if (string.IsNullOrEmpty(sEmpleadosError))
            {
                fnCargarNominasPagadas();
                btnConsultar_Click(sender, e);
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
            }
            else
            {
                Exception myException = new Exception(Resources.resCorpusCFDIEs.varErrorEmpleadoNominaExistente + ":" + sEmpleadosError.Substring(0, sEmpleadosError.Length - 1)) { Source = "webTimbradoGeneracionNomina.aspx|btnGenerar_Click" };
                clsErrorLog.fnNuevaEntrada(myException, clsErrorLog.TipoErroresLog.Datos);
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorGeneracion + " " + sEmpleadosError.Substring(0, sEmpleadosError.Length - 1));
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);
        }
        finally 
        {
            Session.Remove("Id_Nomina");
        }
    }

    protected void btnAceptarConfirmacionTimbrado_Click(object sender, EventArgs e)
    {
        System.Globalization.Calendar oCalendario;
        clsTimbradoFuncionalidad timbrar = new clsTimbradoFuncionalidad();
        clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
        clsValCertificado cer = null;
        DataTable certificado = new DataTable();
        DateTime dFechaPago;
        DateTime dFechaInicioPago;
        DateTime dFechaFinPago;
        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        decimal nNumeroDiasPagados = 0;
        string sClaveNomina = string.Empty;
        string sRFCEmisor = string.Empty;
        string resValidacion = string.Empty;
        string sCadenaOriginal = string.Empty;
        string sCOriginal = String.Empty;
        string sRequest = string.Empty;
        string selloPAC = string.Empty;
        string noCertificadoPAC = string.Empty;
        string sUUID = string.Empty;
        string sSerie = string.Empty;
        string sFolio = string.Empty;
        string sMensajeError = string.Empty;
        int retornoInsert = 0;
        int nId_Nomina = 0;
        int nId_Sucursal = 0;
        int nId_Periodo = 0;
        bool bEntro = false;
        string sTimbrado = string.Empty;

        XmlDocument xDocumento = new XmlDocument();

        if (!fnActualizarLblCreditos())
            return;

        if (gdvPagosNomina.Rows.Count == 0)
            return;

        int nRegistrosTimbrar = fnCheckNoCreditoVSNoNominaCheck();

        if (!nRegistrosTimbrar.Equals(0))
        {
            clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.varCreditosInsuficientesNomina, nRegistrosTimbrar));
            return;
        }

        DataTable sdrInfo = gDAL.fnObtenerDatosFiscalesSuc(Convert.ToInt32(gdvPagosNomina.DataKeys[0]["id_estructura"]));

        if (sdrInfo != null && sdrInfo.Rows.Count > 0)
        {
            certificado = clsTimbradoFuncionalidad.ObtenerCertificado(Convert.ToInt32(sdrInfo.Rows[0]["id_rfc"].ToString()));
            if (clsTimbradoFuncionalidad.fnReplaceCaracters(sdrInfo.Rows[0]["rfc"].ToString()) != string.Empty)
                sRFCEmisor = clsTimbradoFuncionalidad.fnReplaceCaracters(sdrInfo.Rows[0]["rfc"].ToString());
            if (certificado == null || certificado.Rows.Count < 1)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoRecuperoCert);
                return;
            }
        }

        byte[] bLlave = (byte[])certificado.Rows[0]["key"];
        byte[] bCertificado = (byte[])certificado.Rows[0]["certificado"];
        byte[] sPassword = (byte[])certificado.Rows[0]["password"];

        cer = new clsValCertificado(bCertificado);

        if (sRFCEmisor != cer.fnVerificarExistenciaCertificado())
            resValidacion = Resources.resCorpusCFDIEs.valEmisionCer;
        if (!cer.fnComprobarFechas())
            resValidacion = Resources.resCorpusCFDIEs.valFechaCer;
        if (!cer.fnCSD308())
            resValidacion = Resources.resCorpusCFDIEs.varCSD308;
        if (cer.fnEsFiel())
            resValidacion = Resources.resCorpusCFDIEs.valFIELCerFis;

        if (!string.IsNullOrEmpty(resValidacion))
        {
            clsComun.fnMostrarMensaje(this, resValidacion);
            return;
        }

        
        fnCertProximoVencer((certificado.Rows[0]["fecha_termino"] != null) ? certificado.Rows[0]["fecha_termino"].ToString() : string.Empty);
        gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
        gCertificado = new clsValCertificado(bCertificado);

        datosUsuario = clsComun.fnUsuarioEnSesion();

        foreach (GridViewRow Row in gdvPagosNomina.Rows)
        {
            string sRetornoSAT = string.Empty;
            CheckBox CbTimbrado = (CheckBox)(Row.FindControl("cbTimbrado"));
            CheckBox CbCan = (CheckBox)(Row.FindControl("chkSeleccion"));
            TextBox txtNumeroDiasPagados = (TextBox)(Row.FindControl("txtNumeroDiasPagados"));
            string sEmpleado = string.Empty;

            #region Agregado 10/06/2014. Estos datos se van a tomar en base a los objetos en la captura, no en el Grid

            oCalendario = dfi.Calendar;
            
            dFechaPago = Convert.ToDateTime(txtFechaPago.Text);
            dFechaInicioPago = Convert.ToDateTime(txtFechaInicialPago.Text);
            dFechaFinPago = Convert.ToDateTime(txtFechaFinalPago.Text);
            nId_Sucursal = Convert.ToInt32(ddlSucursal.SelectedValue);
            
            nId_Periodo = Convert.ToInt32(ddlPeriodos.SelectedValue);

            sClaveNomina = DateTime.Now.ToShortDateString() + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + "-" + oCalendario.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString() + "-" + nId_Sucursal.ToString();

            #endregion

            if (CbCan.Checked && !CbTimbrado.Checked)
            {
                if (string.IsNullOrEmpty(txtNumeroDiasPagados.Text))
                {
                    //sMensajeError += "Registro número de días capturado :" + sEmpleado + ",";
                    clsComun.fnMostrarMensaje(this, "No hay un número de días capturado en un registro seleccionado.", Resources.resCorpusCFDIEs.varContribuyente);
                    continue;
                }

                nNumeroDiasPagados = Convert.ToDecimal(txtNumeroDiasPagados.Text);

                if (nNumeroDiasPagados <= 0)
                {
                    //sMensajeError += "Registro número de días capturado :" + sEmpleado + ",";
                    clsComun.fnMostrarMensaje(this, "Número de días negativo en un registro seleccionado.", Resources.resCorpusCFDIEs.varContribuyente);
                    continue;
                }

                bEntro = true;
                try
                {
                    # region Declaracion de Objectos

                    DataTable dtEmpleado = new DataTable();
                    DataTable dtNomina = new DataTable();
                    DataTable dtPagoNomina = new DataTable();
                    DataTable dtPercepcionesDeducciones = new DataTable();
                    DataTable dtHorasExtra = new DataTable();
                    DataTable dtIncapacidades = new DataTable();
                    DataTable dtComprobantePagoNomina = new DataTable();

                    Comprobante Cfd = new Comprobante();

                    ComprobanteEmisor CEmisor = new ComprobanteEmisor();
                    CEmisor.DomicilioFiscal = new t_UbicacionFiscal();

                    ComprobanteEmisorRegimenFiscal CERegimen = new ComprobanteEmisorRegimenFiscal();
                    List<ComprobanteEmisorRegimenFiscal> ListRegimen = new List<ComprobanteEmisorRegimenFiscal>();

                    ComprobanteReceptor CReceptor = new ComprobanteReceptor();

                    List<ComprobanteConcepto> ListConcepto = new List<ComprobanteConcepto>();
                    ComprobanteConcepto CConcepto = new ComprobanteConcepto();

                    ComprobanteImpuestos CImpuestos = new ComprobanteImpuestos();
                    ComprobanteImpuestosRetencion impuestosRetencion = new ComprobanteImpuestosRetencion();
                    List<ComprobanteImpuestosRetencion> listaImpRetencion = new List<ComprobanteImpuestosRetencion>();

                    Nomina CompNomina = new Nomina();

                    NominaPercepciones nomPercepciones = new NominaPercepciones();
                    List<NominaPercepcionesPercepcion> listaPercepciones = new List<NominaPercepcionesPercepcion>();

                    NominaDeducciones nomDeducciones = new NominaDeducciones();
                    List<NominaDeduccionesDeduccion> listaDeducciones = new List<NominaDeduccionesDeduccion>();

                    List<NominaIncapacidad> listaIncapcidad = new List<NominaIncapacidad>();
                    List<NominaHorasExtra> listaHorasExtra = new List<NominaHorasExtra>();

                    # endregion

                    # region Llenado de Objectos

                    byte[] certificadoBinario;
                    //certificadoBinario = Utilerias.Encriptacion.DES3.Desencriptar(bCertificado);                    
                    certificadoBinario = (bCertificado);

                    dtEmpleado = cEmpleados.fnExisteEmpleado(Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_Empleado"]));
                    dtPagoNomina = cTimbradoNomina.fnObtenerPagoNomina(Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_PagoNomina"]));
                    dtNomina = cTimbradoNomina.fnObtenerNomina(Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_PagoNomina"]));
                    dtPercepcionesDeducciones = cTimbradoNomina.fnObtenerPercepcionesDeducciones(Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_PagoNomina"]));
                    dtHorasExtra = cTimbradoNomina.fnObtenerHorasExtra(Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_PagoNomina"]));
                    dtIncapacidades = cTimbradoNomina.fnObtenerIncapacidades(Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_PagoNomina"]));
                    dtComprobantePagoNomina = cTimbradoNomina.fnObtenerComprobantePagoNomina(Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_PagoNomina"]));

                    # endregion

                    # region Asignacion de Datos Comprobante y Nomina

                    // Nodo Combrobante

                    Cfd.version = datosUsuario.version;
                    Cfd.fecha = Convert.ToDateTime(DateTime.Now.ToString("s"));
                    Cfd.tipoDeComprobante = ComprobanteTipoDeComprobante.egreso;
                    Cfd.metodoDePago = fnReplaceCaracters(dtComprobantePagoNomina.Rows[0]["metodo_pago"].ToString());
                    Cfd.formaDePago = fnReplaceCaracters(dtComprobantePagoNomina.Rows[0]["forma_pago"].ToString());
                    if (dtComprobantePagoNomina.Rows[0]["numero_cuenta"].ToString() != "") { Cfd.NumCtaPago = fnReplaceCaracters(dtComprobantePagoNomina.Rows[0]["numero_cuenta"].ToString()); }

                    Cfd.noCertificado = cer.ObtenerNoCertificado();
                    Cfd.certificado = Convert.ToBase64String(certificadoBinario);

                    if (dtComprobantePagoNomina.Rows[0]["moneda"].ToString() != "")
                    {
                        Cfd.Moneda = dtComprobantePagoNomina.Rows[0]["moneda"].ToString();
                        if (Cfd.Moneda != "MXN")
                        {
                            Cfd.TipoCambio = dtComprobantePagoNomina.Rows[0]["tipo_cambio"].ToString();
                        }
                    }

                    Cfd.LugarExpedicion = fnReplaceCaracters(dtComprobantePagoNomina.Rows[0]["lugar_expedicion"].ToString());


                    sdrInfo = gDAL.fnObtenerDatosFiscales();
                    DataTable sdrInfoFis = gDAL.fnObtenerDatosFiscalesSuc(Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["id_estructura"])); //Obtiene rfc, razon social
                    DataTable sdrInfoSuc = gOpeSuc.fnObtenerDomicilioSuc(Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["id_estructura"])); //Obtiene direccion fiscal de la sucursal

                    // Nodo Emisor

                    if (sdrInfoFis != null && sdrInfoFis.Rows.Count > 0 &&  sdrInfo != null && sdrInfo.Rows.Count > 0  && sdrInfoSuc != null && sdrInfoSuc.Rows.Count > 0 )
                    {
                        CEmisor.rfc = fnReplaceCaracters(sdrInfoFis.Rows[0]["rfc"].ToString());

                        if (sdrInfoFis.Rows[0]["razon_social"].ToString() != "") { CEmisor.nombre = fnReplaceCaracters(sdrInfoFis.Rows[0]["razon_social"].ToString()); }

                        // Nodo Domicilio Fiscal Emisor 

                        if (sdrInfoSuc.Rows[0]["numero_exterior"].ToString() != "") { CEmisor.DomicilioFiscal.noExterior = fnReplaceCaracters(sdrInfoSuc.Rows[0]["numero_exterior"].ToString()); }
                        if (sdrInfoSuc.Rows[0]["numero_interior"].ToString() != "") { CEmisor.DomicilioFiscal.noInterior = fnReplaceCaracters(sdrInfoSuc.Rows[0]["numero_interior"].ToString()); }
                        if (sdrInfoSuc.Rows[0]["colonia"].ToString() != "") { CEmisor.DomicilioFiscal.colonia = fnReplaceCaracters(sdrInfoSuc.Rows[0]["colonia"].ToString()); }
                        if (sdrInfoSuc.Rows[0]["localidad"].ToString() != "") { CEmisor.DomicilioFiscal.localidad = fnReplaceCaracters(sdrInfoSuc.Rows[0]["localidad"].ToString()); }
                        if (sdrInfoSuc.Rows[0]["referencia"].ToString() != "") { CEmisor.DomicilioFiscal.referencia = fnReplaceCaracters(sdrInfoSuc.Rows[0]["referencia"].ToString()); }

                        CEmisor.DomicilioFiscal.pais = fnReplaceCaracters(sdrInfo.Rows[0]["pais"].ToString());
                        CEmisor.DomicilioFiscal.estado = fnReplaceCaracters(sdrInfoSuc.Rows[0]["estado"].ToString());
                        CEmisor.DomicilioFiscal.municipio = fnReplaceCaracters(sdrInfoSuc.Rows[0]["municipio"].ToString());
                        CEmisor.DomicilioFiscal.calle = fnReplaceCaracters(sdrInfoSuc.Rows[0]["calle"].ToString());
                        CEmisor.DomicilioFiscal.codigoPostal = fnReplaceCaracters(sdrInfoSuc.Rows[0]["codigo_postal"].ToString());

                        // Nodo Regimen Fiscal Emisor           

                        CERegimen.Regimen = dtComprobantePagoNomina.Rows[0]["regimen_fiscal"].ToString();
                        ListRegimen.Add(CERegimen);
                    }


                    // Nodo Expedido En

                    if (dtComprobantePagoNomina.Rows[0]["id_estructura_expedido"] != DBNull.Value)
                    {
                        t_Ubicacion ExpedidoEnField = new t_Ubicacion();

                        DataTable sdrDomicilio = gOpeSuc.fnObtenerDomicilioSuc(Convert.ToInt32(dtComprobantePagoNomina.Rows[0]["id_estructura_expedido"]));

                        if (sdrDomicilio != null && sdrDomicilio.Rows.Count > 0)
                        {
                            ExpedidoEnField.pais = fnReplaceCaracters(sdrInfo.Rows[0]["pais"].ToString());
                            if (sdrDomicilio.Rows[0]["codigo_postal"].ToString() != "") { ExpedidoEnField.codigoPostal = fnReplaceCaracters(sdrDomicilio.Rows[0]["codigo_postal"].ToString()); }
                            if (sdrDomicilio.Rows[0]["numero_exterior"].ToString() != "") { ExpedidoEnField.noExterior = fnReplaceCaracters(sdrDomicilio.Rows[0]["numero_exterior"].ToString()); }
                            if (sdrDomicilio.Rows[0]["numero_interior"].ToString() != "") { ExpedidoEnField.noInterior = fnReplaceCaracters(sdrDomicilio.Rows[0]["numero_interior"].ToString()); }
                            if (sdrDomicilio.Rows[0]["colonia"].ToString() != "") { ExpedidoEnField.colonia = fnReplaceCaracters(sdrDomicilio.Rows[0]["colonia"].ToString()); }
                            if (sdrDomicilio.Rows[0]["estado"].ToString() != "") { ExpedidoEnField.estado = fnReplaceCaracters(sdrDomicilio.Rows[0]["estado"].ToString()); }
                            if (sdrDomicilio.Rows[0]["calle"].ToString() != "") { ExpedidoEnField.calle = fnReplaceCaracters(sdrDomicilio.Rows[0]["calle"].ToString()); }
                            if (sdrDomicilio.Rows[0]["municipio"].ToString() != "") { ExpedidoEnField.municipio = fnReplaceCaracters(sdrDomicilio.Rows[0]["municipio"].ToString()); }
                            if (sdrDomicilio.Rows[0]["localidad"].ToString() != "") { ExpedidoEnField.localidad = fnReplaceCaracters(sdrDomicilio.Rows[0]["localidad"].ToString()); }
                            if (sdrDomicilio.Rows[0]["referencia"].ToString() != "") { ExpedidoEnField.referencia = fnReplaceCaracters(sdrDomicilio.Rows[0]["referencia"].ToString()); }
                        }
                        CEmisor.ExpedidoEn = ExpedidoEnField;
                    }


                    // Nodo Receptor

                    CReceptor.rfc = fnReplaceCaracters(dtEmpleado.Rows[0]["RFC"].ToString());
                    if (dtEmpleado.Rows[0]["Nombre"].ToString() != "") { CReceptor.nombre = fnReplaceCaracters(dtEmpleado.Rows[0]["Nombre"].ToString()); }



                    // Nodo Nomina

                    CompNomina.Version = "1.1";

                    if (dtEmpleado.Rows[0]["RegistroPatronal"].ToString() != "") { CompNomina.RegistroPatronal = dtEmpleado.Rows[0]["RegistroPatronal"].ToString(); }
                    if (dtEmpleado.Rows[0]["NumSeguridadSocial"].ToString() != "") { CompNomina.NumSeguridadSocial = dtEmpleado.Rows[0]["NumSeguridadSocial"].ToString(); }
                    if (dtEmpleado.Rows[0]["Departamento"].ToString() != "") { CompNomina.Departamento = dtEmpleado.Rows[0]["Departamento"].ToString(); }
                    if (dtEmpleado.Rows[0]["CLABE"].ToString() != "") { CompNomina.CLABE = dtEmpleado.Rows[0]["CLABE"].ToString(); }
                    if (dtEmpleado.Rows[0]["Banco"] != DBNull.Value) { CompNomina.Banco = dtEmpleado.Rows[0]["Banco"].ToString().PadLeft(3, '0'); CompNomina.BancoSpecified = true; }
                    if (dtEmpleado.Rows[0]["FechaInicioRelLaboral"] != DBNull.Value) { CompNomina.FechaInicioRelLaboral = Convert.ToDateTime(dtEmpleado.Rows[0]["FechaInicioRelLaboral"]); CompNomina.FechaInicioRelLaboralSpecified = true; }
                    if (dtPagoNomina.Rows[0]["Antiguedad"] != DBNull.Value) { CompNomina.Antiguedad = Convert.ToInt32(dtPagoNomina.Rows[0]["Antiguedad"]); CompNomina.AntiguedadSpecified = true; }
                    if (dtEmpleado.Rows[0]["Puesto"].ToString() != "") { CompNomina.Puesto = dtEmpleado.Rows[0]["Puesto"].ToString(); }
                    if (dtEmpleado.Rows[0]["TipoContrato"].ToString() != "") { CompNomina.TipoContrato = dtEmpleado.Rows[0]["TipoContrato"].ToString(); }
                    if (dtEmpleado.Rows[0]["TipoJornada"].ToString() != "") { CompNomina.TipoJornada = dtEmpleado.Rows[0]["TipoJornada"].ToString(); }
                    if (dtEmpleado.Rows[0]["SalarioBaseCotApor"] != DBNull.Value) { CompNomina.SalarioBaseCotApor = Convert.ToDecimal(dtEmpleado.Rows[0]["SalarioBaseCotApor"]); CompNomina.SalarioBaseCotAporSpecified = true; }
                    if (dtEmpleado.Rows[0]["RiesgoPuesto"] != DBNull.Value) { CompNomina.RiesgoPuesto = Convert.ToInt32(dtEmpleado.Rows[0]["RiesgoPuesto"]); CompNomina.RiesgoPuestoSpecified = true; }
                    if (dtEmpleado.Rows[0]["SalarioDiarioIntegrado"] != DBNull.Value) { CompNomina.SalarioDiarioIntegrado = Convert.ToDecimal(dtEmpleado.Rows[0]["SalarioDiarioIntegrado"]); CompNomina.SalarioDiarioIntegradoSpecified = true; }

                    CompNomina.NumEmpleado = dtEmpleado.Rows[0]["NumEmpleado"].ToString();
                    CompNomina.CURP = dtEmpleado.Rows[0]["CURP"].ToString();
                    CompNomina.TipoRegimen = Convert.ToInt32(dtEmpleado.Rows[0]["TipoRegimen"]);

                    sEmpleado = CompNomina.NumEmpleado;

                    #region Modificado 10/06/2014 Ismael Hidalgo. Estos datos no se van a tomar del datatable, excepto el de numero de días pagados, pero este se va obtener de un TextBox
                    //CompNomina.FechaPago = Convert.ToDateTime(dtPagoNomina.Rows[0]["FechaPago"]);
                    //CompNomina.FechaInicialPago = Convert.ToDateTime(dtPagoNomina.Rows[0]["FechaInicialPago"]);
                    //CompNomina.FechaFinalPago = Convert.ToDateTime(dtPagoNomina.Rows[0]["FechaFinalPago"]);
                    //CompNomina.NumDiasPagados = Convert.ToDecimal(dtPagoNomina.Rows[0]["NumDiasPagados"]);
                    #endregion

                    CompNomina.FechaPago = dFechaPago;
                    CompNomina.FechaInicialPago = dFechaInicioPago;
                    CompNomina.FechaFinalPago = dFechaFinPago;
                    CompNomina.NumDiasPagados = nNumeroDiasPagados;

                    #region  Modificado 10/06/2014 Ismael Hidalgo. El periodo se va tomar del objeto, no del grid
                    //CompNomina.PeriodicidadPago = gdvPagosNomina.DataKeys[Row.RowIndex]["Periodo"].ToString();
                    #endregion

                    CompNomina.PeriodicidadPago = ddlPeriodos.SelectedItem.Text;

                    // Nodo Percepciones

                    DataView dvPercepciones = new DataView(dtPercepcionesDeducciones);
                    dvPercepciones.RowFilter = "Id_Tipo= " + ((int)clsEnumeraciones.TiposDetalleNomina.Percepcion).ToString();

                    DataTable dtPercepciones = dvPercepciones.ToTable();

                    // Nodo Percepciones

                    if (dtPercepciones.Rows.Count != 0)
                    {
                        nomPercepciones.TotalGravado = Convert.ToDecimal(dtPercepciones.Compute("Sum(ImporteGravado)", "Id_Tipo =" + ((int)clsEnumeraciones.TiposDetalleNomina.Percepcion).ToString()));
                        nomPercepciones.TotalExento = Convert.ToDecimal(dtPercepciones.Compute("Sum(ImporteExento)", "Id_Tipo = " + ((int)clsEnumeraciones.TiposDetalleNomina.Percepcion).ToString()));
                    }


                    // Nodos Percepcion

                    foreach (DataRow renglonDetalle in dtPercepciones.Rows)
                    {
                        NominaPercepcionesPercepcion Percepcion = new NominaPercepcionesPercepcion();
                        Percepcion.TipoPercepcion = renglonDetalle["ClavePercepcion"].ToString().PadLeft(3, '0'); 
                        Percepcion.Clave = renglonDetalle["Clave"].ToString();
                        Percepcion.Concepto = renglonDetalle["Concepto"].ToString();
                        Percepcion.ImporteGravado = Convert.ToDecimal(renglonDetalle["ImporteGravado"]);
                        Percepcion.ImporteExento = Convert.ToDecimal(renglonDetalle["ImporteExento"]);
                        listaPercepciones.Add(Percepcion);
                    }

                    //Nodo Deducciones

                    DataView dvDeducciones = new DataView(dtPercepcionesDeducciones);
                    dvDeducciones.RowFilter = "Id_Tipo= " + ((int)clsEnumeraciones.TiposDetalleNomina.Deduccion).ToString();

                    DataTable dtDeducciones = dvDeducciones.ToTable();

                    if (dtDeducciones.Rows.Count != 0)
                    {
                        nomDeducciones.TotalGravado = Convert.ToDecimal(dtDeducciones.Compute("Sum(ImporteGravado)", "Id_Tipo =" + ((int)clsEnumeraciones.TiposDetalleNomina.Deduccion).ToString()));
                        nomDeducciones.TotalExento = Convert.ToDecimal(dtDeducciones.Compute("Sum(ImporteExento)", "Id_Tipo = " + ((int)clsEnumeraciones.TiposDetalleNomina.Deduccion).ToString()));

                    }
                    //Nodos Deduccion

                    foreach (DataRow renglonDetalle in dtDeducciones.Rows)
                    {
                        NominaDeduccionesDeduccion Deduccion = new NominaDeduccionesDeduccion();
                        Deduccion.TipoDeduccion = renglonDetalle["ClavePercepcion"].ToString().PadLeft(3, '0');
                        Deduccion.Clave = renglonDetalle["Clave"].ToString();
                        Deduccion.Concepto = renglonDetalle["Concepto"].ToString();
                        Deduccion.ImporteGravado = Convert.ToDecimal(renglonDetalle["ImporteGravado"]);
                        Deduccion.ImporteExento = Convert.ToDecimal(renglonDetalle["ImporteExento"]);
                        listaDeducciones.Add(Deduccion);
                    }

                    // Nodos HorasExtra

                    foreach (DataRow renglonDetalle in dtHorasExtra.Rows)
                    {
                        NominaHorasExtra HoraExtra = new NominaHorasExtra();
                        HoraExtra.Dias = Convert.ToInt32(renglonDetalle["Dias"]);
                        switch (renglonDetalle["TipoHoras"].ToString())
                        {
                            case "Dobles":
                                HoraExtra.TipoHoras = NominaHorasExtraTipoHoras.Dobles;
                                break;
                            case "Triples":
                                HoraExtra.TipoHoras = NominaHorasExtraTipoHoras.Triples;
                                break;
                            default:
                                break;
                        }
                        HoraExtra.HorasExtra = Convert.ToInt32(renglonDetalle["HorasExtra"]);
                        HoraExtra.ImportePagado = Convert.ToDecimal(renglonDetalle["ImportePagado"]);
                        listaHorasExtra.Add(HoraExtra);
                    }

                    // Nodos Incapacidad

                    foreach (DataRow renglonDetalle in dtIncapacidades.Rows)
                    {
                        NominaIncapacidad Incapacidad = new NominaIncapacidad();
                        Incapacidad.DiasIncapacidad = Convert.ToDecimal(renglonDetalle["DiasIncapacidad"]);
                        Incapacidad.TipoIncapacidad = Convert.ToInt32(renglonDetalle["Tipo"]);
                        Incapacidad.Descuento = Convert.ToDecimal(renglonDetalle["Descuento"]);
                        listaIncapcidad.Add(Incapacidad);
                    }

                    string sTotalGravadoSinISR = string.Empty;
                    string sTotalExentoSinISR = string.Empty;
                    string sTotalGravadoConISR = string.Empty;
                    string sTotalExentoConISR = string.Empty;
                    string sFiltro = string.Empty;

                    decimal nISR = 0;

                    sFiltro = "Id_Tipo = " + ((int)clsEnumeraciones.TiposDetalleNomina.Deduccion).ToString() + " And Id_TipoPercepDedu <> " + ((int)clsEnumeraciones.TiposDeduccionesPercepciones.ISR).ToString();

                    sTotalGravadoSinISR = dtDeducciones.Compute("Sum(ImporteGravado)", sFiltro).ToString();
                    sTotalExentoSinISR = dtDeducciones.Compute("Sum(ImporteExento)", sFiltro).ToString();

                    if (string.IsNullOrEmpty(sTotalGravadoSinISR))
                        sTotalGravadoSinISR = "0";

                    if (string.IsNullOrEmpty(sTotalExentoSinISR))
                        sTotalExentoSinISR = "0";

                    sFiltro = "Id_Tipo = " + ((int)clsEnumeraciones.TiposDetalleNomina.Deduccion).ToString() +
                                                " And Id_TipoPercepDedu = " + ((int)clsEnumeraciones.TiposDeduccionesPercepciones.ISR).ToString();

                    sTotalGravadoConISR = dtDeducciones.Compute("Sum(ImporteGravado)", sFiltro).ToString();
                    sTotalExentoConISR = dtDeducciones.Compute("Sum(ImporteExento)", sFiltro).ToString();

                    if (string.IsNullOrEmpty(sTotalGravadoConISR))
                        sTotalGravadoConISR = "0";

                    if (string.IsNullOrEmpty(sTotalExentoConISR))
                        sTotalExentoConISR = "0";


                    nISR = Convert.ToDecimal(sTotalGravadoConISR) + Convert.ToDecimal(sTotalExentoConISR);

                    Cfd.subTotal = nomPercepciones.TotalGravado + nomPercepciones.TotalExento;

                    if ((Convert.ToDecimal(sTotalGravadoSinISR) + Convert.ToDecimal(sTotalExentoSinISR)) != 0)
                    {
                        Cfd.descuento = Convert.ToDecimal(sTotalGravadoSinISR) + Convert.ToDecimal(sTotalExentoSinISR);
                        Cfd.descuentoSpecified = true;
                        Cfd.motivoDescuento = "Deducciones nómina";
                    }


                    Cfd.total = Convert.ToDecimal(dtPagoNomina.Rows[0]["Total"]);

                    // Nodo Concepto

                    CConcepto.cantidad = Convert.ToDecimal(1.00);
                    CConcepto.unidad = "Servicio";
                    CConcepto.descripcion = "PAGO DE NÓMINA";
                    CConcepto.valorUnitario = Cfd.subTotal;
                    CConcepto.importe = Cfd.subTotal;

                    ListConcepto.Add(CConcepto);

                    if (nISR != 0)
                    {

                        // Nodo Impuestos

                        CImpuestos.totalImpuestosRetenidos = nISR;

                        // Nodo Retencion
                        impuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto.ISR;
                        impuestosRetencion.importe = nISR;
                        listaImpRetencion.Add(impuestosRetencion);

                    }



                    // Agregar Listas

                    if (listaImpRetencion.Count > 0)
                    {
                        CImpuestos.Retenciones = listaImpRetencion.ToArray();
                        CImpuestos.totalImpuestosRetenidosSpecified = true;
                        CImpuestos.totalImpuestosRetenidos = CImpuestos.totalImpuestosRetenidos;
                    }

                    if (listaPercepciones.Count > 0)
                    {
                        nomPercepciones.Percepcion = listaPercepciones.ToArray();
                        CompNomina.Percepciones = nomPercepciones;
                    }

                    if (listaDeducciones.Count > 0)
                    {
                        nomDeducciones.Deduccion = listaDeducciones.ToArray();
                        CompNomina.Deducciones = nomDeducciones;
                    }

                    if (listaIncapcidad.Count > 0)
                    {
                        CompNomina.Incapacidades = listaIncapcidad.ToArray();
                    }

                    if (listaHorasExtra.Count > 0)
                    {
                        CompNomina.HorasExtras = listaHorasExtra.ToArray();
                    }

                    if (ListConcepto.Count > 0)
                    {
                        Cfd.Conceptos = ListConcepto.ToArray();
                    }

                    if (ListRegimen.Count > 0)
                    {
                        CEmisor.RegimenFiscal = ListRegimen.ToArray();
                    }

                    # endregion

                    # region  Generar Nodo Nomina y Asignacion Nodos Hijos al Comprobante

                    MemoryStream ms = new MemoryStream();
                    StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);

                    XmlDocument xmlComplNomina = new XmlDocument();
                    XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
                    sns.Add("nomina", "http://www.sat.gob.mx/nomina");

                    XmlSerializer serializador = new XmlSerializer(typeof(Nomina));
                    serializador.Serialize(sw, CompNomina, sns);

                    ms.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(ms);
                    xmlComplNomina.LoadXml(sr.ReadToEnd());

                    XmlElement xeComplNomina = xmlComplNomina.DocumentElement;

                    ComprobanteComplemento complNomina = new ComprobanteComplemento();
                    XmlElement[] axeComplNomina = new XmlElement[] { xeComplNomina };
                    complNomina.Any = axeComplNomina;


                    Cfd.Complemento = complNomina;
                    Cfd.Emisor = CEmisor;
                    Cfd.Receptor = CReceptor;
                    Cfd.Impuestos = CImpuestos;

                    string tNameSpace = "nomina" + "|" + "http://www.sat.gob.mx/nomina" + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina11.xsd";
                    xDocumento = gTimbrado.fnGenerarXML32(Cfd, tNameSpace);

                    # endregion

                    # region Generar Cadena Original y Sello Emisor

                    XPathNavigator navNodoTimbre = xDocumento.CreateNavigator();
                    sCadenaOriginal = fnConstruirCadenaTimbrado(navNodoTimbre);
                    sRequest = clsComun.fnRequestRecepcion(sCadenaOriginal, "Recibo de Nomina", gdvPagosNomina.DataKeys[0]["id_estructura"].ToString(), datosUsuario.userName, "");
                    if (clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sRequest, DateTime.Now, "0", "Request", "E", string.Empty) == 0)
                    {
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "No se inserto el Request.");
                    }

                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "9.-fnConstruirCadenaTimbrado" + "|" + "Generamos la cadena original.");

                    clsNodoTimbre creadorTimbre = new clsNodoTimbre();
                    string sSello = string.Empty;

                    sSello = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1, true);
                    Cfd.sello = sSello;

                    //Validar sello del emisor
                    if (!cer.fnVerificarSello(sCadenaOriginal, sSello))
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado + " " + "HSM", Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }

                    xDocumento = gTimbrado.fnGenerarXML32(Cfd, tNameSpace);

                    # endregion

                    # region Generar Nodo Timbre

                    creadorTimbre.GenerarNodoTimbre32(Cfd, ref sCOriginal, axeComplNomina[0].OuterXml, null);

                    int x = 1;

                    //Revisar datos del HSM

                    if (Cfd.Complemento.Any[x].Attributes["UUID"].Value == string.Empty ||
                    Cfd.Complemento.Any[x].Attributes["FechaTimbrado"].Value == string.Empty ||
                    Cfd.Complemento.Any[x].Attributes["selloCFD"].Value == string.Empty ||
                    Cfd.Complemento.Any[x].Attributes["noCertificadoSAT"].Value == string.Empty ||
                    Cfd.Complemento.Any[x].Attributes["selloSAT"].Value == string.Empty)
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCertificado + " " + "HSM", Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }
                    else
                    {
                        selloPAC = Cfd.Complemento.Any[x].Attributes["selloSAT"].Value;
                        noCertificadoPAC = Cfd.Complemento.Any[x].Attributes["noCertificadoSAT"].Value;
                        sUUID = Cfd.Complemento.Any[x].Attributes["UUID"].Value;
                    }

                    //Validamos sello del PAC
                    if (!clsValCertificado.fnVerificarSelloPAC(sCOriginal, selloPAC, noCertificadoPAC))
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado + " " + "HSM", Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }

                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "10.-GenerarNodoTimbre" + "|" + "Generamos el sello.");

                    xDocumento = gTimbrado.fnGenerarXML32(Cfd, tNameSpace);

                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "11.-GenerarNodoTimbre" + "|" + "Generamos el sello del SAT, se lo agregamos al objeto y generamos el XML final.");


                    # endregion

                    # region Generar Insertar Combrobante

                    nId_Nomina = cTimbradoNomina.fnExisteNominaPorIdEstructuraIdPeriodoClaveNomina(nId_Sucursal, nId_Periodo, sClaveNomina);

                    //if (!nId_Nomina.Equals(0))
                    //{
                    //    if (cTimbradoNomina.fnExistePagoNominaPorIdEmpleadoIdNomina(nId_Nomina, Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_Empleado"])))
                    //    {
                    //        clsComun.fnMostrarMensaje(this, string.Format(Resources.resCorpusCFDIEs.valNominaEmpleadoPeriodo, sClaveNomina, gdvPagosNomina.Rows[Row.RowIndex].Cells[5].Text), Resources.resCorpusCFDIEs.varContribuyente);
                    //        return;
                    //    }
                    //}


                    //Generar el Hash para revisar si no hay uno existente en la BD
                    string HASHTimbre = clsEnvioSAT.GetHASH(sCOriginal).ToUpper();
                    string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();

                    if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHEmisor, "Emisor"))
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }


                    if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbre, "Timbre"))
                    {
                        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                        return;
                    }

                    if (!clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbre, "Timbre"))
                    {


                        XmlNamespaceManager nsmComprobante2 = new XmlNamespaceManager(xDocumento.NameTable);
                        nsmComprobante2.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                        nsmComprobante2.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                        DateTime dFecha_Timbrado = new DateTime();
                        string sNombre_Emisor = "";
                        string sRfcReceptor = "";
                        string sNombre_Receptor = "";
                        DateTime dFecha_Emision = new DateTime();
                        Decimal nTotal = 0;
                        string sMoneda = "";

                        try { dFecha_Timbrado = Convert.ToDateTime(xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante2).Value); }
                        catch { }
                        try { sNombre_Emisor = xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante2).Value; }
                        catch { }
                        try { sRfcReceptor = xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante2).Value; }
                        catch { }
                        try { sNombre_Receptor = xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante2).Value; }
                        catch { }
                        try { dFecha_Emision = Convert.ToDateTime(xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante2).Value); }
                        catch { }
                        try { nTotal = Convert.ToDecimal(xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante2).Value); }
                        catch { }
                        try { sMoneda = xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante2).Value; }
                        catch { }


                        retornoInsert = cTimbradoNomina.fnInsertarComprobante(
                            //xDocumento.DocumentElement.OuterXml,
                            PAXCrypto.CryptoAES.EncriptaAES(xDocumento.DocumentElement.OuterXml),
                            (int)clsEnumeraciones.TiposDocumentos.ReciboNomina,
                            'G',
                            DateTime.Now,
                            Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["id_estructura"]),
                            datosUsuario.id_usuario,
                            sSerie,
                            "C",
                            HASHTimbre,
                            HASHEmisor,
                            nId_Nomina,
                            sClaveNomina,
                            Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_PagoNomina"]),
                            nId_Sucursal,
                            nId_Periodo,
                            dFechaPago,
                            dFechaInicioPago,
                            dFechaFinPago,
                            nNumeroDiasPagados,
                             sUUID,
                            dFecha_Timbrado,
                            sRFCEmisor,
                            sNombre_Emisor,
                            sRfcReceptor,
                            sNombre_Receptor,
                            dFecha_Emision,
                            sSerie,
                            sFolio,
                            //nTotal,
                            PAXCrypto.CryptoAES.EncriptaAES(Convert.ToString(nTotal)),
                            sMoneda
                            );

                        if (retornoInsert > 0)
                        {
                            sTimbrado = sEmpleado + ",";
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "13.-fnInsertarComprobante" + "|" + "Insertado correctamente: " + retornoInsert + " UUID: " + sUUID.ToUpper());
                            //Inserta acuse PAC y creditos al guardar el comprobante.
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, sUUID.ToUpper(), xDocumento.DocumentElement.OuterXml, DateTime.Now, "0", "Response", "E", string.Empty);

                            // Se actualiza el registro del pago
                            // Modificación 10/06/2014 Ismael Hidalgo
                            // Se pasan los datos de fecha de pago, fecha inicio de pago, el numero de días trabajados, el periodo para actualizar el registro de pago al empleado
                            //cTimbradoNomina.fnActualizaIdCfdNomina(nId_Nomina, sClaveNomina, Convert.ToInt32(gdvPagosNomina.DataKeys[Row.RowIndex]["Id_PagoNomina"]), 
                            //    retornoInsert, nId_Sucursal, nId_Periodo, dFechaPago, dFechaInicioPago, dFechaFinPago, nNumeroDiasPagados);

                            //Enviar comprobante al SAT
                            //clsEnvioSAT enviarSAT = new clsEnvioSAT();
                            //sRetornoSAT = enviarSAT.fnEnviarBloqueCfdi(HASHEmisor, datosUsuario.id_usuario, xDocumento, retornoInsert, datosUsuario);

                            Literal ltlXML = (Literal)(Row.FindControl("ltlXML"));
                            ltlXML.Text = xDocumento.DocumentElement.OuterXml;
                            Literal ltlIdCfd = (Literal)(Row.FindControl("ltlIdCfd"));
                            ltlIdCfd.Text = retornoInsert.ToString();

                            fnActualizaCreditos();
                        }
                        else
                        {
                            sMensajeError += sEmpleado + ",";
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "13A.-fnInsertarComprobante" + "|" + "Error al insertar el comprobante:" + Resources.resCorpusCFDIEs.msgNoCompGenerado);
                            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                        }
                    }
                    else
                    {
                        sMensajeError += sEmpleado + ",";
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "13B.-fnInsertarComprobante" + "|" + "Error al insertar el comprobante:" + Resources.resCorpusCFDIEs.msgNoCompGenerado);
                        //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                    }                   

                    # endregion
                }
                catch (Exception ex)
                {
                    sMensajeError += sEmpleado + ",";
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "13C.-fnInsertarComprobante" + "|" + "Error al insertar el comprobante:" + Resources.resCorpusCFDIEs.msgNoCompGenerado);
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado, Resources.resCorpusCFDIEs.varContribuyente);
                }
            }
        }



        if (!(datosUsuario.email == string.Empty))
        {
            txtCorreoEmisor.Text = datosUsuario.email;
        }

        if (!string.IsNullOrEmpty(sMensajeError) && bEntro)
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msgNoCompGenerado + " " + sMensajeError.Replace(sMensajeError.Substring(sMensajeError.Length - 1, 1), ""), Resources.resCorpusCFDIEs.varContribuyente);
        }

        if (!string.IsNullOrEmpty(sTimbrado))
        {
            txtFechaPago.Text = string.Empty;
            txtFechaInicialPago.Text = string.Empty;
            txtFechaFinalPago.Text = string.Empty;

            fnCargarNominasPagadas();

            txtCorreoCliente.BorderColor = System.Drawing.Color.Empty;
            txtCorreoCC.BorderColor = System.Drawing.Color.Empty;
            txtCorreoMsj.Text = Resources.resCorpusCFDIEs.varMensajeCorreo + "\n" + "\n" + ViewState["razonSocial_Emisor"] + "\n" + "RFC:" + "\n" + ViewState["rfc_Emisor"];
            mpeEnvioCorreo.Show();
        }
    }

    protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarNominasPagadas();

        gdvPagosNomina.DataSource = null;
        gdvPagosNomina.DataBind();
        cbSeleccionar.Visible = false;
        cbSeleccionar.Checked = false;
    }

    protected void ddlPeriodos_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarNominasPagadas();
    }

    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCargarNominasPagadas();
    }

    protected void ddlNominasPagadas_DataBound(object sender, EventArgs e)
    {
        fnAgregaOpcionSeleccione(sender, e);
    }

    /// <summary>
    /// Función que contruye la cadena original
    /// </summary>
    /// <param name="xml">Documento</param>
    /// <param name="psNombreArchivoXSLT">Nombre del archivo de tranformación</param>
    /// <returns></returns>
    private string fnConstruirCadenaTimbrado(IXPathNavigable xml)
    {
        string sCadenaOriginal = string.Empty;
        MemoryStream ms;
        StreamReader srDll;
        XsltArgumentList args;
        XslCompiledTransform xslt;
        try
        {
            xslt = new XslCompiledTransform();
            xslt.Load(typeof(CaOri.V32));
            ms = new MemoryStream();
            args = new XsltArgumentList();
            xslt.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            srDll = new StreamReader(ms);
            sCadenaOriginal = srDll.ReadToEnd();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        return sCadenaOriginal;
    }

    /// <summary>
    /// Función que se encarga de recuperar los créditos del usuario
    /// </summary>
    /// <param name="clave_usuario">Clave del Usuario</param>
    /// <returns></returns>
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

    /// <summary>
    /// Anexo 20 Eliminar en la reglas de estructura.
    /// </summary>
    /// <param name="varRep"></param>
    /// <returns></returns>
    public static string fnReplaceCaracters(string varRep)
    {
        string sReplace = string.Empty;

        if (varRep.Contains('&'))
        {
            varRep.Replace("&", "&amp;");
        }

        if (varRep.Contains('<'))
        {
            varRep.Replace("<", "&lt;");
        }

        if (varRep.Contains('>'))
        {
            varRep.Replace(">", "&gt;");
        }

        if (varRep.Contains("'"))
        {
            varRep.Replace("'", "&apos;");
        }

        if (varRep.Contains("\""))
        {
            varRep.Replace("\"", "&quot;");
        }

        sReplace = varRep;
        return sReplace;
    }

    /* Modificación 28 - 12 - 2012 Josel Moreno Se agregó función para verificar si el certificado está próximo a vencer */
    private void fnCertProximoVencer(string sFechaTermino)
    {
        if (!String.IsNullOrEmpty(sFechaTermino))
        {
            try
            {
                DateTime dFechaTermino = DateTime.Parse(sFechaTermino);
                DateTime dFechaActual = DateTime.Now;
                TimeSpan dDiferencia = new TimeSpan();
                dDiferencia = dFechaTermino - dFechaActual;

                int nDifDias = dDiferencia.Days;   //Diferencia en días 

                if (nDifDias >= 0 && nDifDias <= 30)
                {
                    string sMensaje = string.Empty;

                    if (nDifDias == 0)
                    {
                        if (dFechaTermino.Day == dFechaActual.Day)
                            sMensaje = Resources.resCorpusCFDIEs.msgCaducCertHoy;
                        else
                            sMensaje = Resources.resCorpusCFDIEs.msgCaducCert1Dia;
                    }
                    else
                    {
                        sMensaje = String.Format(Resources.resCorpusCFDIEs.msgCaducCertDias, nDifDias);
                    }

                    clsComun.fnMostrarMensaje(this, sMensaje);
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "Ocurrió un error al verificar si el certificado está próximo a vencer - Fecha de caducidad de certificado: " + sFechaTermino + " - PAX_FacturacionPago_3_2.Timbrado.webTimbradoGeneraciona.fnCertProximoVencer()", 0);
            }
        }
    }

    protected void cbSeleccionar_CheckedChanged(object sender, EventArgs e){
        foreach (GridViewRow renglon in gdvPagosNomina.Rows)
        {
            CheckBox CbCan = (CheckBox)(renglon.FindControl("chkSeleccion"));
            CbCan.Checked = cbSeleccionar.Checked;
        }      
    }

    decimal total = 0M;

    protected void gdvPagosNomina_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal dTotal;
            decimal value;
            Label lblTotal = (Label)e.Row.FindControl("lblTotal");
            bool ok = decimal.TryParse(lblTotal.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out value);
            dTotal = value;
            total += dTotal;
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotalSum = (Label)e.Row.FindControl("lblTotalSum");
            lblTotalSum.Text = total.ToString("c6");
        }
    }
    protected void gdvPagosNomina_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();
        try
        {
            int nId_Pago_Nomina = Convert.ToInt32(gdvPagosNomina.DataKeys[e.RowIndex].Values["Id_PagoNomina"].ToString());
            int nId_Nomina = Convert.ToInt32(gdvPagosNomina.DataKeys[e.RowIndex].Values["Id_Nomina"].ToString());

            if (!cTimbradoNomina.fnActualizarNominaEstatusInactivo(nId_Nomina, nId_Pago_Nomina, gdvPagosNomina.Rows.Count))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCambio);
                return;
            }

            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varCambio);

            btnConsultar_Click(sender, e);
        }
        catch (Exception ex)
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorBaja);
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string sIDS = "";

            foreach (GridViewRow Row in gdvPagosNomina.Rows) 
            {

                sIDS += "" + gdvPagosNomina.DataKeys[Row.RowIndex]["Id_PagoNomina"].ToString() + ",";
            }

            sIDS = sIDS.Substring(0, sIDS.Length - 1);

            //sIDS = Utilerias.Encriptacion.Base64.EncriptarBase64(sIDS);            
            sIDS = PAXCrypto.CryptoAES.EncriptarAES64(sIDS);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "../Consultas/webDescargaConsultaGeneracion.aspx",
                                                        String.Format("<script>window.open('{0}?p={1}','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                                                                       "../Consultas/webDescargaConsultaGeneracion.aspx", sIDS), false);

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
}

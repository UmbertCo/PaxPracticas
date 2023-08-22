using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Timbrado_webTimbradoRegistroNomina : System.Web.UI.Page
{
    clsInicioSesionUsuario datosUsuario;
    protected DataTable dtCreditos;
    private string SeleccioneUnValor = Resources.resCorpusCFDIEs.varSeleccione;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            if (datosUsuario == null)
                return;

            fnCargarSucursalesExpedidoEn();
            fnCargarSucursalesFiscal();
            fnCargarPaisesLugarExpedicion();
            fnCargarEstadosLugarExpedicion(ddlPaisLugExp.SelectedValue);
            ddlMetodoPago_SelectedIndexChanged(sender, e);
            ddlSucursalesFis_SelectedIndexChanged(sender, e);

            clsOperacionCuenta cOperacionCuenta = new clsOperacionCuenta();

            DataTable sdrInfo = cOperacionCuenta.fnObtenerDatosFiscales();

            if (sdrInfo != null && sdrInfo.Rows.Count > 0)
            {
                txtRegimenfiscal.Text = sdrInfo.Rows[0]["regimen_fiscal"].ToString();
                ViewState["rfc_Emisor"] = sdrInfo.Rows[0]["rfc"].ToString();
                ViewState["razonSocial_Emisor"] = sdrInfo.Rows[0]["razon_social"].ToString();
            }

            if (!(ddlSucursales.SelectedValue == ""))
                ddlSucursales_SelectedIndexChanged(sender, e);

            //txtFechaPago.Text = DateTime.Now.ToShortDateString();  Se modifico 05/06/2014. Ya no van a ser necesarias, se van a capturar en la Generación antes de Timbrar
            cbAgrExpEn.Checked = false; //Esconder panel expedido en
            cbAgrExpEn_CheckedChanged(sender, e);

            fnCargarTipos();
            fnCargarTipoDeduccionPercepcion(Convert.ToInt32(ddlTipo.SelectedValue));
            fnOcultarPanelConceptosIncapacidades();
            fnOcultarPanelConceptosHorasExtra();
            fnCargarTiposIncapacidades();

            fnInicializaConceptosNomina();
            ViewState["Id_Empleado"] = "0";
            //Session["Id_Nomina"] = "0";
            Session["IdPagoNomina"] = "0";

            if (Session["IdPago"] != null)
            {
                btnGenerarNomina.Visible = true;
                fnCargarDatosNominaGeneracion(Convert.ToInt32(Session["IdPago"]));
            }
        }
    }
    public void Page_Error(object sender, EventArgs e)
    {
        Exception objErr = Server.GetLastError().GetBaseException();

        if (!string.IsNullOrEmpty(objErr.Message))
        {
            clsErrorLog.fnNuevaEntrada(objErr, clsErrorLog.TipoErroresLog.Datos);
            Server.ClearError();
            Response.Redirect("~/webGlobalError.aspx");
        }
    }
    protected void ddlSucursalesFis_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSucursalesFis.SelectedValue.Equals(string.Empty))
            return;

        fnObtenerDomicilioSucursalFiscal(Convert.ToInt32(ddlSucursalesFis.SelectedValue));
        fnLimpiarDatosEmpleado();
        txtAntiguedad.Text = string.Empty;
        /* Se modifico 05/06/2014. Ya no van a ser necesarias, se van a capturar en la Generación antes de Timbrar
        txtFechaPago.Text = DateTime.Now.ToShortDateString();
        txtFechaInicialPago.Text = string.Empty;
        txtFechaFinalPago.Text = string.Empty;
        txtNumeroDiasPagos.Text = string.Empty;*/
    }
    protected void cbAgrExpEn_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAgrExpEn.Checked)
        {
            cpeExpedidoEn.Collapsed = false;
            cpeExpedidoEn.ClientState = "false";
            cpeExpedidoEn.ExpandControlID = pnlExtender.ID;
            cpeExpedidoEn.CollapseControlID = pnlExtender.ID;

            rfvSucursales.EnableClientScript = true;
            pnlExpedidoEn.Attributes.Add("style", "visibility:show;");
        }
        else
        {
            cpeExpedidoEn.Collapsed = true;
            cpeExpedidoEn.ClientState = "true";
            cpeExpedidoEn.ExpandControlID = "";
            cpeExpedidoEn.CollapseControlID = "";

            rfvSucursales.EnableClientScript = false;

            pnlExpedidoEn.Attributes.Add("style", "visibility:hidden;");
        }
    }
    protected void ddlSucursales_SelectedIndexChanged(object sender, EventArgs e)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ddlSucursalesExpEn" + "|" + "Selecciona un tipo de sucursal expedido en." + "|" + ddlSucursales.SelectedValue);
        fnObtieneDomicilioSucursalExpedidoEn(Convert.ToInt32(ddlSucursales.SelectedValue));     
    }
    protected void ddlPaisLugExp_SelectedIndexChanged(object sender, EventArgs e)
    {
        string sId_Pais = (sender as DropDownList).SelectedValue;
        fnCargarEstadosLugarExpedicion(sId_Pais);
    }
    protected void ddlMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMetodoPago.SelectedItem.Text == "No Aplica")
        {
            txtNumeroCuenta.Text = "No Aplica";
            txtNumeroCuenta.Enabled = false;
        }
        else
        {
            txtNumeroCuenta.Text = string.Empty;
        }

        if (txtNumeroCuenta.Text != "No Aplica")
            txtNumeroCuenta.Enabled = true;

        if (ddlMetodoPago.SelectedItem.Text == "Efectivo" || ddlMetodoPago.SelectedItem.Text == "No identificado")
            txtNumeroCuenta.Enabled = false;
    }
    protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
    {
        fnCambiarTipoMoneda();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, this.Title + "|" + "ddlMoneda" + "|" + "Selecciona un tipo de moneda." + "|" + ddlMoneda.SelectedValue);
    }
    protected void btnConsulta_Click(object sender, EventArgs e)
    {
        fnBuscarEmpleados();
        linkModal_ModalPopupExtender.Show();
    }
    protected void btnCancelarModal_Click(object sender, EventArgs e)
    {
        gdvEmpleados.DataSource = null;
        gdvEmpleados.DataBind();
        linkModal_ModalPopupExtender.Hide();
    }
    protected void gdvEmpleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gdvEmpleados.PageIndex = e.NewPageIndex;
        fnBuscarEmpleados();
        linkModal_ModalPopupExtender.Show();
    }
    protected void gdvEmpleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtEmpleado = new DataTable();
        DateTime dtFechaInicioRelacionLaboral;
        try
        {
            fnLimpiarDatosEmpleado();

            if (gdvEmpleados.SelectedIndex < 0)
                return;

            string sId_Empleado = gdvEmpleados.SelectedDataKey.Values["Id_Empleado"].ToString();
            string sTipoRegimen = gdvEmpleados.SelectedDataKey.Values["TipoRegimen"].ToString();
            string sBanco = gdvEmpleados.SelectedDataKey.Values["Banco"].ToString();
            string sRiesgoPuesto = gdvEmpleados.SelectedDataKey.Values["RiesgoPuesto"].ToString();

            ViewState["Id_Empleado"] = sId_Empleado;

            gdvEmpleados.DataSource = null;
            gdvEmpleados.DataBind();

            clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
            dtEmpleado = cEmpleados.fnExisteEmpleado(Convert.ToInt32(sId_Empleado));

            if (dtEmpleado.Rows.Count.Equals(0))
                return;

            lblRfcEmpleadoDescripion.Text = dtEmpleado.Rows[0]["RFC"].ToString();
            txtNombreEmpleado.Text = dtEmpleado.Rows[0]["Nombre"].ToString();
            lblRegistroPatronalDescripion.Text = dtEmpleado.Rows[0]["RegistroPatronal"].ToString();
            lblNumeroEmpleadoDescripion.Text = dtEmpleado.Rows[0]["NumEmpleado"].ToString();
            lblCURPDescripion.Text = dtEmpleado.Rows[0]["CURP"].ToString();
            lblNumeroSeguridadSocialDescripion.Text = dtEmpleado.Rows[0]["NumSeguridadSocial"].ToString();
            lblDepartamentoDescripion.Text = dtEmpleado.Rows[0]["Departamento"].ToString();
            lblCLABEDescripion.Text = dtEmpleado.Rows[0]["CLABE"].ToString();
            lblFechaInicioRelacionLaboralDescripion.Text = (dtEmpleado.Rows[0]["FechaInicioRelLaboral"].ToString().Equals(string.Empty) ? string.Empty : Convert.ToDateTime(dtEmpleado.Rows[0]["FechaInicioRelLaboral"].ToString()).ToShortDateString());
            lblPuestoDescripion.Text = dtEmpleado.Rows[0]["Puesto"].ToString();
            lblTipoContratoDescripion.Text = dtEmpleado.Rows[0]["TipoContrato"].ToString();
            lblTipoJornadaDescripion.Text = dtEmpleado.Rows[0]["TipoJornada"].ToString();
            lblSalarioBaseDescripion.Text = dtEmpleado.Rows[0]["SalarioBaseCotApor"].ToString();
            lblSalarioDiarioIntegradoDescripion.Text = dtEmpleado.Rows[0]["SalarioDiarioIntegrado"].ToString();

            lblTipoRegimenDescripion.Text = sTipoRegimen;
            lblBancoDescripion.Text = sBanco;
            lblRiesgoPuestoDescripion.Text = sRiesgoPuesto;

            if (!string.IsNullOrEmpty(lblFechaInicioRelacionLaboralDescripion.Text))
            {
                dtFechaInicioRelacionLaboral = Convert.ToDateTime(lblFechaInicioRelacionLaboralDescripion.Text);
                TimeSpan tsFechaDiferencia = DateTime.Now - dtFechaInicioRelacionLaboral;
                int nSemanas = tsFechaDiferencia.Days / 7;
                txtAntiguedad.Text = nSemanas.ToString();
            }            
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }
    protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!hfId_Registro.Value.Equals("0"))
        {
            DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];
            DataRow Renglon = dtDetalle.Rows.Find(hfId_Registro.Value);

            if (Convert.ToInt32(Renglon["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                Convert.ToInt32(Renglon["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
            {
                if (gdvHorasExtra.Rows.Count > 0)
                {
                    ddlTipo.SelectedValue = Renglon["Id_Tipo"].ToString();
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varHorasExtraPercepcionesCapturadas);
                    return;
                }
            }

            if (Convert.ToInt32(Renglon["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                Convert.ToInt32(Renglon["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
            {
                if (gdvIncapacidades.Rows.Count > 0)
                {
                    ddlTipo.SelectedValue = Renglon["Id_Tipo"].ToString();
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varIncapacidadesIncapacidadesCapturadas);
                    return;
                }
            }
        }


        fnCargarTipoDeduccionPercepcion(Convert.ToInt32(ddlTipo.SelectedValue));
        txtClaveConcepto.Text = string.Empty;
        txtConcepto.Text = string.Empty;
        txtImporteGravado.Text = "0";
        txtImporteExento.Text = "0";

        ddlTipoDeduccionPercepcion.Focus();
    }
    protected void ddlTipoDeduccionPercepcion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!hfId_Registro.Value.Equals("0"))
        {
            DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];
            DataRow Renglon = dtDetalle.Rows.Find(hfId_Registro.Value);

            if (Convert.ToInt32(Renglon["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                Convert.ToInt32(Renglon["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
            {
                if (gdvHorasExtra.Rows.Count > 0)
                {
                    ddlTipoDeduccionPercepcion.SelectedValue = Renglon["Id_TipoPercepDedu"].ToString();
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varHorasExtraPercepcionesCapturadas);
                    return;
                }
            }

            if (Convert.ToInt32(Renglon["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                Convert.ToInt32(Renglon["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
            {
                if (gdvIncapacidades.Rows.Count > 0)
                {
                    ddlTipoDeduccionPercepcion.SelectedValue = Renglon["Id_TipoPercepDedu"].ToString();
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varIncapacidadesIncapacidadesCapturadas);
                    return;
                }
            }
        }

        if (ddlTipoDeduccionPercepcion.SelectedValue.Equals("0"))
        {
            txtClaveConcepto.Text = string.Empty;
            txtConcepto.Text = string.Empty;
            txtImporteGravado.Text = "0";
            txtImporteExento.Text = "0";
            return;
        }

        string[] asTipoPercepcion = ddlTipoDeduccionPercepcion.SelectedItem.Text.Split('-');
        
        txtClaveConcepto.Text = asTipoPercepcion[0].Trim();
        txtConcepto.Text = asTipoPercepcion[1].Trim();

        txtClaveConcepto.Focus();
    }
    protected void ddlTipoDeduccionPercepcion_DataBound(object sender, EventArgs e)
    {
        AgregaOpcionSeleccione(sender, e);
    }
    protected void btnAgregarDetalle_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];

            hfId_RegistroHoraExtra.Value = "0";
            hfId_RegistroIncapacidad.Value = "0";

            if (Convert.ToInt32(ddlTipo.SelectedValue).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                    Convert.ToInt32(ddlTipoDeduccionPercepcion.SelectedValue).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra) &&
                gdvHorasExtra.Rows.Count <= 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varHorasExtraPercepcionesInexistentes);
                fnExpandirPanelHorasExtra();
                txtDiasHorasExtra.Focus();
                return;
            }

            if (Convert.ToInt32(ddlTipo.SelectedValue).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                Convert.ToInt32(ddlTipoDeduccionPercepcion.SelectedValue).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad) &&
                gdvIncapacidades.Rows.Count <= 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varIncapacidadesDeduccionesInexistentes);
                fnExpadirPanelIncapacidades();
                txtDiasIncapacidad.Focus();
                return;
            }

            if (hfId_Registro.Value.Equals("0"))
            {
                DataRow Renglon = dtDetalle.NewRow();
                Renglon["Id_Tipo"] = Convert.ToInt32(ddlTipo.SelectedValue);
                Renglon["Tipo"] = ddlTipo.SelectedItem.Text;
                Renglon["Id_TipoPercepDedu"] = Convert.ToInt32(ddlTipoDeduccionPercepcion.SelectedValue);
                Renglon["Clave"] = txtClaveConcepto.Text;
                Renglon["Concepto"] = txtConcepto.Text;
                Renglon["ImporteGravado"] = Convert.ToDouble(txtImporteGravado.Text);
                Renglon["ImporteExento"] = Convert.ToDouble(txtImporteExento.Text);

                dtDetalle.Rows.Add(Renglon);

                fnActualizarIncapacidades(Convert.ToInt32(dtDetalle.Rows[dtDetalle.Rows.Count - 1][0].ToString()));
                fnActualizarHorasExtra(Convert.ToInt32(dtDetalle.Rows[dtDetalle.Rows.Count - 1][0].ToString()));
            }
            else
            {
                DataRow Renglon = dtDetalle.Rows.Find(hfId_Registro.Value);

                if (Renglon == null)
                    return;

                Renglon["Id_Tipo"] = Convert.ToInt32(ddlTipo.SelectedValue);
                Renglon["Tipo"] = ddlTipo.SelectedItem.Text;
                Renglon["Id_TipoPercepDedu"] = Convert.ToInt32(ddlTipoDeduccionPercepcion.SelectedValue);
                Renglon["Clave"] = txtClaveConcepto.Text;
                Renglon["Concepto"] = txtConcepto.Text;
                Renglon["ImporteGravado"] = Convert.ToDouble(txtImporteGravado.Text);
                Renglon["ImporteExento"] = Convert.ToDouble(txtImporteExento.Text);
            }

            ddlTipoDeduccionPercepcion.SelectedValue = "0";
            txtClaveConcepto.Text = string.Empty;
            txtConcepto.Text = string.Empty;
            txtImporteGravado.Text = "0";
            txtImporteExento.Text = "0";
            hfId_Registro.Value = "0";

            fnOcultarPanelConceptosHorasExtra();
            fnOcultarPanelConceptosIncapacidades();

            gdvHorasExtra.DataSource = null;
            gdvHorasExtra.DataBind();

            gdvIncapacidades.DataSource = null;
            gdvIncapacidades.DataBind();

            ViewState["dtDetalle"] = dtDetalle;

            fnCargarConceptosPercepciones();
            fnCargarConceptosDeducciones();
            fnCalcularTotalPercepciones();            
            fnCalcularTotalDeduccion();

            fnCalcularTotales();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError, Resources.resCorpusCFDIEs.varContribuyente);
        }
    }
    protected void btnCancelarDetalle_Click(object sender, EventArgs e)
    {
        ddlTipoDeduccionPercepcion.SelectedValue = "0";
        txtClaveConcepto.Text = string.Empty;
        txtConcepto.Text = string.Empty;
        txtImporteGravado.Text = "0";
        txtImporteExento.Text = "0";
        hfId_Registro.Value = "0";

        gdvIncapacidades.DataSource = null;
        gdvIncapacidades.DataBind();

        gdvHorasExtra.DataSource = null;
        gdvHorasExtra.DataBind();

        gdvConceptosPercepciones.SelectedIndex = - 1;
        gdvConceptosDeducciones.SelectedIndex = -1;
    }
    protected void btnAgregarHorasExtra_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtHorasExtra = (DataTable)ViewState["dtHorasExtra"];

            if (hfId_Hora_Extra.Value.Equals("0"))
            {
                DataRow Renglon = dtHorasExtra.NewRow();
                if (hfId_RegistroHoraExtra.Value.Equals("0"))
                    Renglon["Id_PercepDedu"] = "0";
                else
                    Renglon["Id_PercepDedu"] = Convert.ToInt32(hfId_RegistroHoraExtra.Value);

                Renglon["Dias"] = Convert.ToInt32(txtDiasHorasExtra.Text);
                Renglon["TipoHoras"] = ddlTipoHorasExtra.SelectedValue;
                Renglon["HorasExtra"] = Convert.ToInt32(txtHorasExtra.Text);
                Renglon["ImportePagado"] = Convert.ToDouble(txtImportePagadoHorasExtra.Text);

                dtHorasExtra.Rows.Add(Renglon);
            }
            else 
            {
                DataRow Renglon = dtHorasExtra.Rows.Find(hfId_Hora_Extra.Value);

                if (Renglon == null)
                    return;

                Renglon["Id_PercepDedu"] = Convert.ToInt32(hfId_RegistroHoraExtra.Value);
                Renglon["Dias"] = Convert.ToInt32(txtDiasHorasExtra.Text);
                Renglon["TipoHoras"] = ddlTipoHorasExtra.SelectedValue;
                Renglon["HorasExtra"] = Convert.ToInt32(txtHorasExtra.Text);
                Renglon["ImportePagado"] = Convert.ToDouble(txtImportePagadoHorasExtra.Text);
            }

            txtDiasHorasExtra.Text = string.Empty;
            ddlTipoHorasExtra.SelectedIndex = -1;
            txtHorasExtra.Text = string.Empty;
            txtImportePagadoHorasExtra.Text = "0";
            hfId_Hora_Extra.Value = "0";

            ViewState["dtHorasExtra"] = dtHorasExtra;
            fnCargarHorasExtra();
            gdvHorasExtra.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError, Resources.resCorpusCFDIEs.varContribuyente);
        }
    }
    protected void btnCancelarIncapacidad_Click(object sender, EventArgs e)
    {
        ddlTipoIncapacidad.SelectedIndex = -1;
        txtDiasIncapacidad.Text = string.Empty;
        txtDescuentoIncapacidad.Text = string.Empty;
        hfId_Incapacidad.Value = "0";        
    }
    protected void btnAgregarIncapacidad_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dtIncapacidades = (DataTable)ViewState["dtIncapacidades"];

            if (hfId_Incapacidad.Value.Equals("0"))
            {
                DataRow Renglon = dtIncapacidades.NewRow();
                if (hfId_RegistroHoraExtra.Value.Equals("0"))
                    Renglon["Id_PercepcionDeduccion"] = "0";
                else
                    Renglon["Id_PercepcionDeduccion"] = Convert.ToInt32(hfId_RegistroIncapacidad.Value);

                Renglon["Tipo"] = Convert.ToInt32(ddlTipoIncapacidad.SelectedValue);
                Renglon["DiasIncapacidad"] = Convert.ToDouble(txtDiasIncapacidad.Text);
                Renglon["TipoIncapacidad"] = ddlTipoIncapacidad.SelectedItem.Text;
                Renglon["Descuento"] = Convert.ToDouble(txtDescuentoIncapacidad.Text);

                dtIncapacidades.Rows.Add(Renglon);
            }
            else
            {
                DataRow Renglon = dtIncapacidades.Rows.Find(hfId_Incapacidad.Value);

                if (Renglon == null)
                    return;

                Renglon["Id_PercepcionDeduccion"] = Convert.ToInt32(hfId_RegistroIncapacidad.Value);
                Renglon["Tipo"] = Convert.ToInt32(ddlTipoIncapacidad.SelectedValue);
                Renglon["DiasIncapacidad"] = Convert.ToDouble(txtDiasIncapacidad.Text);
                Renglon["TipoIncapacidad"] = ddlTipoIncapacidad.SelectedItem.Text;
                Renglon["Descuento"] = Convert.ToDouble(txtDescuentoIncapacidad.Text);
            }

            ddlTipoIncapacidad.SelectedIndex = -1;
            txtDiasIncapacidad.Text = string.Empty;
            txtDescuentoIncapacidad.Text = "0";
            hfId_Incapacidad.Value = "0";

            ViewState["dtIncapacidades"] = dtIncapacidades;           
            fnCargarIncapacidades();
            gdvIncapacidades.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError, Resources.resCorpusCFDIEs.varContribuyente);
        }
    }
    protected void btnCancelarHoraExtra_Click(object sender, EventArgs e)
    {
        txtDiasHorasExtra.Text = string.Empty;
        ddlTipoHorasExtra.SelectedIndex = -1;
        txtHorasExtra.Text = string.Empty;
        txtImportePagadoHorasExtra.Text = string.Empty;
        hfId_Hora_Extra.Value = "0";
    }
    protected void gdvConceptosPercepciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];
        int nId_Registro = Convert.ToInt32(gdvConceptosPercepciones.DataKeys[e.RowIndex].Values["Id_PercepDedu"].ToString());
        DataRow Renglon = dtDetalle.Rows.Find(nId_Registro);

        if (Convert.ToInt32(Renglon["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                Convert.ToInt32(Renglon["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
        {
            if (gdvHorasExtra.Rows.Count > 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varHorasExtraPercepcionesCapturadas);
                return;
            }
        }

        dtDetalle.Rows.Remove(Renglon);

        ViewState["dtDetalle"] = dtDetalle;

        ddlTipoDeduccionPercepcion.SelectedValue = "0";
        txtClaveConcepto.Text = string.Empty;
        txtConcepto.Text = string.Empty;
        txtImporteGravado.Text = "0";
        txtImporteExento.Text = "0";
        hfId_Registro.Value = "0";

        fnCargarConceptosPercepciones();
        fnCalcularTotalPercepciones();
        fnCalcularTotales();
        hfId_Registro.Value = "0";
    }
    protected void gdvConceptosPercepciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hfId_Registro.Value = gdvConceptosPercepciones.SelectedDataKey.Values["Id_PercepDedu"].ToString();

            ddlTipo.SelectedValue = gdvConceptosPercepciones.SelectedDataKey.Values["Id_Tipo"].ToString();
            fnCargarTipoDeduccionPercepcion(Convert.ToInt32(ddlTipo.SelectedValue));
            ddlTipoDeduccionPercepcion.SelectedValue = gdvConceptosPercepciones.SelectedDataKey.Values["Id_TipoPercepDedu"].ToString();
            txtClaveConcepto.Text = gdvConceptosPercepciones.SelectedRow.Cells[2].Text;
            txtConcepto.Text = HttpUtility.HtmlDecode(gdvConceptosPercepciones.SelectedRow.Cells[3].Text);
            txtImporteGravado.Text = gdvConceptosPercepciones.SelectedRow.Cells[4].Text;
            txtImporteExento.Text = gdvConceptosPercepciones.SelectedRow.Cells[5].Text;

            gdvHorasExtra.DataSource = null;
            gdvHorasExtra.DataBind();

            gdvIncapacidades.DataSource = null;
            gdvIncapacidades.DataBind();

            fnOcultarPanelConceptosHorasExtra();
            fnOcultarPanelConceptosIncapacidades();

            if (Convert.ToInt32(ddlTipo.SelectedValue).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                        Convert.ToInt32(ddlTipoDeduccionPercepcion.SelectedValue).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
            {
                hfId_RegistroHoraExtra.Value = hfId_Registro.Value;
                fnCargarHorasExtra();
                fnExpandirPanelHorasExtra();
            }

            gdvConceptosDeducciones.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void gdvConceptosDeducciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hfId_Registro.Value = gdvConceptosDeducciones.SelectedDataKey.Values["Id_PercepDedu"].ToString();

            ddlTipo.SelectedValue = gdvConceptosDeducciones.SelectedDataKey.Values["Id_Tipo"].ToString();
            fnCargarTipoDeduccionPercepcion(Convert.ToInt32(ddlTipo.SelectedValue));
            ddlTipoDeduccionPercepcion.SelectedValue = gdvConceptosDeducciones.SelectedDataKey.Values["Id_TipoPercepDedu"].ToString();
            txtClaveConcepto.Text = gdvConceptosDeducciones.SelectedRow.Cells[2].Text;
            txtConcepto.Text = HttpUtility.HtmlDecode(gdvConceptosDeducciones.SelectedRow.Cells[3].Text);
            txtImporteGravado.Text = gdvConceptosDeducciones.SelectedRow.Cells[4].Text;
            txtImporteExento.Text = gdvConceptosDeducciones.SelectedRow.Cells[5].Text;

            gdvHorasExtra.DataSource = null;
            gdvHorasExtra.DataBind();

            gdvIncapacidades.DataSource = null;
            gdvIncapacidades.DataBind();

            fnOcultarPanelConceptosHorasExtra();
            fnOcultarPanelConceptosIncapacidades();

            if (Convert.ToInt32(ddlTipo.SelectedValue).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                Convert.ToInt32(ddlTipoDeduccionPercepcion.SelectedValue).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
            {
                hfId_RegistroIncapacidad.Value = hfId_Registro.Value;
                fnCargarIncapacidades();
                fnExpadirPanelIncapacidades();
            }

            gdvConceptosPercepciones.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }
    protected void gdvConceptosDeducciones_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];
        int nId_Registro = Convert.ToInt32(gdvConceptosDeducciones.DataKeys[e.RowIndex].Values["Id_PercepDedu"].ToString());
        DataRow Renglon = dtDetalle.Rows.Find(nId_Registro);

        if (Convert.ToInt32(Renglon["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                Convert.ToInt32(Renglon["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
        {
            if (gdvIncapacidades.Rows.Count > 0)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varIncapacidadesIncapacidadesCapturadas);
                return;
            }
        }

        dtDetalle.Rows.Remove(Renglon);

        ViewState["dtDetalle"] = dtDetalle;

        ddlTipoDeduccionPercepcion.SelectedValue = "0";
        txtClaveConcepto.Text = string.Empty;
        txtConcepto.Text = string.Empty;
        txtImporteGravado.Text = "0";
        txtImporteExento.Text = "0";
        hfId_Registro.Value = "0";

        fnCargarConceptosDeducciones();
        fnCalcularTotalDeduccion();
        fnCalcularTotales();
        hfId_Registro.Value = "0";
    }
    protected void gdvIncapacidades_SelectedIndexChanged(object sender, EventArgs e)
    {
        //hfId_Incapacidad.Value = gdvIncapacidades.SelectedDataKey.Values["Id_PercepcionDeduccion"].ToString();
        hfId_Incapacidad.Value = gdvIncapacidades.SelectedDataKey.Values["Id_Incapacidad"].ToString();

        ddlTipoIncapacidad.SelectedValue = gdvIncapacidades.SelectedDataKey.Values["Id_Incapacidad"].ToString();
        txtDiasIncapacidad.Text = gdvIncapacidades.SelectedRow.Cells[2].Text;
        txtDescuentoIncapacidad.Text = gdvIncapacidades.SelectedRow.Cells[4].Text;
    }
    protected void gdvIncapacidades_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        DataTable dtIncapacidades = (DataTable)ViewState["dtIncapacidades"];
        dtIncapacidades.Rows.RemoveAt(e.RowIndex);

        ViewState["dtIncapacidades"] = dtIncapacidades;

        ddlTipoIncapacidad.SelectedIndex = -1;
        txtDiasIncapacidad.Text = string.Empty;
        txtDescuentoIncapacidad.Text = string.Empty;
        hfId_Incapacidad.Value = "0";

        gdvIncapacidades.DataSource = dtIncapacidades;
        gdvIncapacidades.DataBind();
    }
    protected void gdvHorasExtra_SelectedIndexChanged(object sender, EventArgs e)
    {
        hfId_Hora_Extra.Value = gdvHorasExtra.SelectedDataKey.Value.ToString();

        txtDiasHorasExtra.Text = gdvHorasExtra.SelectedDataKey.Value.ToString();
        ddlTipoHorasExtra.SelectedValue = gdvHorasExtra.SelectedRow.Cells[3].Text;
        txtHorasExtra.Text = gdvHorasExtra.SelectedRow.Cells[4].Text;
        txtImportePagadoHorasExtra.Text = gdvHorasExtra.SelectedRow.Cells[5].Text;
    }
    protected void gdvHorasExtra_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        DataTable dtHorasExtra = (DataTable)ViewState["dtHorasExtra"];
        dtHorasExtra.Rows.RemoveAt(e.RowIndex);

        ViewState["dtHorasExtra"] = dtHorasExtra;

        txtDiasHorasExtra.Text = string.Empty;
        ddlTipoHorasExtra.SelectedIndex = -1;
        txtHorasExtra.Text = string.Empty;
        txtImportePagadoHorasExtra.Text = string.Empty;
        hfId_Hora_Extra.Value = "0";

        gdvHorasExtra.DataSource = dtHorasExtra;
        gdvHorasExtra.DataBind();
    }
    protected void AgregaOpcionSeleccione(System.Object sender, System.EventArgs e)
    {
        ((DropDownList)sender).Items.Insert(0, new ListItem(SeleccioneUnValor, "0"));
    }
    protected void btnCrear_Click(object sender, EventArgs e)
    {
        bool bResultado = false;
        clsOperacionCuenta cOperacionConsulta = new clsOperacionCuenta();
        clsOperacionRFC cOperacionRFC = new clsOperacionRFC();
        DateTime dFechaPago;
        DateTime dFechaInicialPago;
        DateTime dFechaFinalPago;
        DataTable dtPeriodos = new DataTable();
        DataTable dtCertificado = new DataTable();
        DataTable dtCertificadoEstructrura = new DataTable();
        decimal nNumeroDiasPagados = 0;
        decimal nTotal = 0;
        int nId_Empleado = 0;
        int nId_Estructuctura = 0;
        int nId_Estructura_Expedido = 0;
        int nId_Periodo = 0;        
        int nAntiguedad = 0;
        //int nUltimaNomina = 0;
        //int nId_Rfc = 0;
        string sForma_Pago = string.Empty;
        string sMetodo_Pago = string.Empty;
        string sMoneda = string.Empty;
        string sRegimen_Fiscal = string.Empty;
        string sLugar_Expedicion = string.Empty;
        string sNumero_Cuenta = string.Empty;
        string sTipo_Cambio = string.Empty;
        //SqlDataReader sdrInfo = null;
        try
        {
            if (!fnValidarCapturaHorasExtra())
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varHorasExtraPercepcionesInexistentes);
                return;
            }

            if (!fnValidarCapturaIncapacidades())
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varIncapacidadesDeduccionesInexistentes);
                return;
            }

            if (gdvConceptosPercepciones.Rows.Count <= 0)
            {
                //Muestra un msj de aviso 
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varConceptoPercepcionesInexistente, Resources.resCorpusCFDIEs.varContribuyente);
                return;
            }

            if (Convert.ToDecimal(lblDetSubtotal.Text.Replace("$", "").Replace(",", "")) < 0 || Convert.ToDecimal(lblTotalVal.Text.Replace("$", "").Replace(",", "")) < 0)
            {
                //Muestra un msj de aviso 
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.lblAvisoTotales, Resources.resCorpusCFDIEs.varContribuyente);
                return;
            }            

            DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];
            DataTable dtIncapacidades = (DataTable)ViewState["dtIncapacidades"];
            DataTable dtHorasExtra = (DataTable)ViewState["dtHorasExtra"];

            nId_Estructura_Expedido = (cbAgrExpEn.Checked ? Convert.ToInt32(ddlSucursales.SelectedValue) : 0);
            sLugar_Expedicion = ddlPaisLugExp.SelectedItem.Text + "," + (ddlEdoLugExp.Items.Count <= 0 ? string.Empty : ddlEdoLugExp.SelectedItem.Text);
            sMetodo_Pago = ddlMetodoPago.SelectedValue;
            sNumero_Cuenta = txtNumeroCuenta.Text;
            sMoneda = ddlMoneda.SelectedValue;
            sRegimen_Fiscal = txtRegimenfiscal.Text;
            sForma_Pago = ddlFormaPago.SelectedValue;
            sTipo_Cambio = txtTipoCambio.Text;            

            nId_Estructuctura = Convert.ToInt32(ddlSucursalesFis.SelectedValue);
            nId_Empleado = Convert.ToInt32(ViewState["Id_Empleado"]);
            dFechaPago = Convert.ToDateTime("01-01-1900");//Convert.ToDateTime(txtFechaPago.Text); Se modifico 05/06/2014. Ya no van a ser necesarias, se van a capturar en la Generación antes de Timbrar
            dFechaInicialPago = Convert.ToDateTime("01-01-1900");//Convert.ToDateTime(txtFechaInicialPago.Text); Se modifico 05/06/2014. Ya no van a ser necesarias, se van a capturar en la Generación antes de Timbrar
            dFechaFinalPago = Convert.ToDateTime("01-01-1900");//Convert.ToDateTime(txtFechaFinalPago.Text); Se modifico 05/06/2014. Ya no van a ser necesarias, se van a capturar en la Generación antes de Timbrar
            nNumeroDiasPagados = 0; //Convert.ToDecimal(txtNumeroDiasPagos.Text); Se modifico 05/06/2014. Ya no van a ser necesarias, se van a capturar en la Generación antes de Timbrar
            nAntiguedad = (string.IsNullOrEmpty(txtAntiguedad.Text) ? 0 : Convert.ToInt32(txtAntiguedad.Text));
            nTotal = Convert.ToDecimal(lblTotalVal.Text.Replace(",", "").Replace("$", ""));

            clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();

            #region Modificaciones 07/06/2014 Ismael Hidalgo

            //sdrInfo = cOperacionConsulta.fnObtenerDatosFiscalesSuc(nId_Estructuctura);
            //if (sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read())
            //{
            //    //Obtener el Id de RFC del emisor
            //    nId_Rfc = Convert.ToInt32(sdrInfo["id_rfc"].ToString());
            //}

            //if (nId_Rfc.Equals(0))
            //    throw new Exception("No es posible obtener el RFC de la sucursal padre.");

            //dtCertificado = cOperacionRFC.fnObtieneCertificado(nId_Rfc);
            //if (dtCertificado.Rows.Count.Equals(0))
            //    throw new Exception("No es posible obtener el certificado de la sucursal padre.");

            //dtCertificadoEstructrura = cTimbradoNomina.fnObtenerCertificado(Convert.ToInt32(dtCertificado.Rows[0]["id_certificado"]));
            //if (dtCertificadoEstructrura.Rows.Count.Equals(0))
            //    throw new Exception("No es posible obtener la sucursal padre.");

            //DataTable dtEstructuraPeriodo = cTimbradoNomina.fnObtenerPeriodoPorEstructura(Convert.ToInt32(dtCertificadoEstructrura.Rows[0]["id_estructura"]));
            //nId_Periodo = Convert.ToInt32(dtEstructuraPeriodo.Rows[0]["IdTipoPeriodo"]);

            #endregion

            if (!Convert.ToInt32(Session["Id_Nomina"]).Equals(0))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.msjEditarNominaTimbrada);
                return;
            }

            // Si el pago de Nómina es 0, es un registro que se quiere editar, sino es un registro nuevo
            if (Convert.ToInt32(Session["IdPago"]).Equals(0))
            {
                // Verificamos si el empleado tiene un registro de pago activo sin pagar
                if (cTimbradoNomina.fnExistePagoNominaPorIdEstructuraIdEmpleado(nId_Estructuctura, nId_Empleado))
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorEmpleadoNominaExistente);
                    return;
                }

                bResultado = cTimbradoNomina.fnRegistrarNomina(nId_Estructuctura, nId_Periodo, nId_Empleado, dFechaPago,
                                dFechaInicialPago, dFechaFinalPago, nNumeroDiasPagados, nAntiguedad, nTotal, nId_Estructura_Expedido,
                                sLugar_Expedicion, sMetodo_Pago, sNumero_Cuenta, sMoneda, sRegimen_Fiscal, sForma_Pago, sTipo_Cambio,
                                dtDetalle, dtHorasExtra, dtIncapacidades);

            }
            else
            {
                bResultado = cTimbradoNomina.fnRegistrarNominaActual(Convert.ToInt32(Session["IdPago"]), nId_Empleado,
                                dFechaPago, dFechaInicialPago, dFechaFinalPago, nNumeroDiasPagados, nAntiguedad, nTotal,
                                nId_Estructura_Expedido, sLugar_Expedicion, sMetodo_Pago, sNumero_Cuenta, sMoneda, sRegimen_Fiscal,
                                sForma_Pago, sTipo_Cambio, dtDetalle, dtHorasExtra, dtIncapacidades);
            }

            if (bResultado)
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
            else
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorAlta);

            Session["IdPago"] = Session["IdPagoNomina"];
            fnLimpiarCamposCaptura();
            fnLimpiarDatosEmpleado();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varNominaRegistroError);
        }
    }
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        fnHabilitaDeshabilitaCamposCaptura(true);
        fnLimpiarCamposCaptura();
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        fnLimpiarCamposCaptura();
        fnHabilitaDeshabilitaCamposCaptura(false);
    }
    protected void btnGenerarNomina_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Timbrado/webTimbradoGeneracionNomina.aspx");
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

    /// <summary>
    /// Método que se encarga de actualizar los ID de la percepción o deducción en las horas extra
    /// </summary>
    /// <param name="pnId_PercepcionDeduccion">ID de la percepción o la deducción</param>
    private void fnActualizarHorasExtra(int pnId_PercepcionDeduccion)
    {
        DataTable dtHorasExtra = (DataTable)ViewState["dtHorasExtra"];
        foreach (DataRow renglon in dtHorasExtra.Rows)
        {
            if (renglon["Id_PercepDedu"].ToString().Equals("0"))
                renglon["Id_PercepDedu"] = pnId_PercepcionDeduccion;
        }

        ViewState["dtHorasExtra"] = dtHorasExtra;
    }

    /// <summary>
    /// Método que se encarga de actualizar los ID de la percepción o deducción en las incapacidades
    /// </summary>
    /// <param name="pnId_PercepcionDeduccion">ID de la percepción o la deducción</param>
    private void fnActualizarIncapacidades(int pnId_PercepcionDeduccion)
    {
        DataTable dtIncapacidades = (DataTable)ViewState["dtIncapacidades"];
        foreach (DataRow renglon in dtIncapacidades.Rows)
        {
            if (renglon["Id_PercepcionDeduccion"].ToString().Equals("0"))
                renglon["Id_PercepcionDeduccion"] = pnId_PercepcionDeduccion;
        }

        ViewState["dtIncapacidades"] = dtIncapacidades;
    }

    /// <summary>
    /// Función que se encarga de buscar a los empleados de una estructura en especifico
    /// </summary>
    private void fnBuscarEmpleados()
    {
        clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
        try
        {
            gdvEmpleados.DataSource = cEmpleados.fnBuscarEmpleados(txtNumeroEmpleadoBusqueda.Text, txtRfcEmpleadoBusqueda.Text, 
                                        txtNombreEmpleadoBusqueda.Text, Convert.ToInt32(ddlSucursalesFis.SelectedValue));
            gdvEmpleados.DataBind();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
    }

    /// <summary>
    /// Método que se encarga de calcular los totales de los importes gravados y exentos de las deducciones
    /// </summary>
    private void fnCalcularTotalDeduccion()
    {
        DataTable dtDeduccion = new DataTable();
        string sTotalGravado = string.Empty;
        string sTotalExento = string.Empty;

        dtDeduccion = (DataTable)ViewState["dtDetalle"];

        sTotalGravado = dtDeduccion.Compute("Sum(ImporteGravado)", "Id_Tipo = " + ((int)clsEnumeraciones.TiposDetalleNomina.Deduccion).ToString()).ToString();
        sTotalExento = dtDeduccion.Compute("Sum(ImporteExento)", "Id_Tipo = " + ((int)clsEnumeraciones.TiposDetalleNomina.Deduccion).ToString()).ToString();

        sTotalGravado = (string.IsNullOrEmpty(sTotalGravado) ? "0" : sTotalGravado);
        sTotalExento = (string.IsNullOrEmpty(sTotalExento) ? "0" : sTotalExento);

        lblTotalGravadoDeducciones.Text = String.Format("{0:c6}", sTotalGravado);
        lblTotalExentoDeducciones.Text = String.Format("{0:c6}", sTotalExento);

        lblTotalesDeducciones.Visible = true;
        lblTotalGravadoDeducciones.Visible = true;
        lblTotalExentoDeducciones.Visible = true;
        tblDeduccionesTotales.Visible = true;
    }

    /// <summary>
    /// Método que se encarga de calcular los totales de los importes gravados y exentos de las percepciones
    /// </summary>
    private void fnCalcularTotalPercepciones()
    {
        DataTable dtPercepciones = new DataTable();
        string sTotalGravado = string.Empty;
        string sTotalExento = string.Empty;

        dtPercepciones = (DataTable)ViewState["dtDetalle"];

        sTotalGravado = dtPercepciones.Compute("Sum(ImporteGravado)", "Id_Tipo =" + ((int)clsEnumeraciones.TiposDetalleNomina.Percepcion).ToString()).ToString();
        sTotalExento = dtPercepciones.Compute("Sum(ImporteExento)", "Id_Tipo = " + ((int)clsEnumeraciones.TiposDetalleNomina.Percepcion).ToString()).ToString();

        sTotalGravado = (string.IsNullOrEmpty(sTotalGravado) ? "0" : sTotalGravado);
        sTotalExento = (string.IsNullOrEmpty(sTotalExento) ? "0" : sTotalExento);

        lblTotalGravadoPercepciones.Text = String.Format("{0:c6}", sTotalGravado);
        lblTotalExentoPercepciones.Text = String.Format("{0:c6}", sTotalExento);      

        lblTotalesPercepciones.Visible = true;
        lblTotalGravadoPercepciones.Visible = true;
        lblTotalExentoPercepciones.Visible = true;
        tblPercepcionesTotales.Visible = true;
    }

    /// <summary>
    /// Método que se encarga de realizar el calculo del área de totales
    /// </summary>
    private void fnCalcularTotales()
    {
        DataTable dtDeducciones = new DataTable();
        string sTotalGravadoSinISR = string.Empty;
        string sTotalExentoSinISR = string.Empty;
        string sTotalGravadoConISR = string.Empty;
        string sTotalExentoConISR = string.Empty;
        string sFiltro = string.Empty;

        decimal nSubtotal = 0;
        decimal nDescuento = 0;
        decimal nISR = 0;
        decimal nImporteGravadoPercepciones = 0;
        decimal nImporteExentoPercepciones = 0;

        dtDeducciones = (DataTable)ViewState["dtDetalle"];

        sFiltro = "Id_Tipo = " + ((int)clsEnumeraciones.TiposDetalleNomina.Deduccion).ToString() +
                                    " And Id_TipoPercepDedu <> " + ((int)clsEnumeraciones.TiposDeduccionesPercepciones.ISR).ToString();

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

        nImporteGravadoPercepciones = (string.IsNullOrEmpty(lblTotalGravadoPercepciones.Text) ? 0 : Convert.ToDecimal(lblTotalGravadoPercepciones.Text));
        nImporteExentoPercepciones = (string.IsNullOrEmpty(lblTotalExentoPercepciones.Text) ? 0 : Convert.ToDecimal(lblTotalExentoPercepciones.Text));
        
        nSubtotal = nImporteGravadoPercepciones + nImporteExentoPercepciones;
        nDescuento = Convert.ToDecimal(sTotalGravadoSinISR) + Convert.ToDecimal(sTotalExentoSinISR);
        nISR = Convert.ToDecimal(sTotalGravadoConISR) + Convert.ToDecimal(sTotalExentoConISR);

        lblDetSubtotal.Text = String.Format("{0:c6}", nSubtotal);
        lblDetDescuentoVal.Text = String.Format("{0:c6}", nDescuento);
        lblISRVal.Text = String.Format("{0:c6}", nISR);
        lblTotalVal.Text = String.Format("{0:c6}", nSubtotal - nDescuento - nISR);

        lblSubtotal.Visible = true;
        lblDetDescuento.Visible = true;
        lblISR.Visible = true;
        lblTotal.Visible = true;

        lblDetSubtotal.Visible = true;
        lblDetDescuentoVal.Visible = true;
        lblISRVal.Visible = true;
        lblTotalVal.Visible = true;
    }

    /// <summary>
    /// Método que se encarga de cambiar el tipo de moneda a utilizar
    /// </summary>
    private void fnCambiarTipoMoneda()
    {
        try
        {
            decimal dTotal = Convert.ToDecimal(lblTotalVal.Text.ToString().Replace("$", "").Replace(",", ""));

            NumaletPago numLetras = new NumaletPago();
            numLetras.LetraCapital = true;

            switch (ddlMoneda.SelectedValue)
            {
                case "MXN":
                    numLetras.TipoMoneda = NumaletPago.Moneda.Peso;
                    break;

                case "USD":
                    numLetras.TipoMoneda = NumaletPago.Moneda.Dolar;
                    break;

                case "XEU":
                    numLetras.TipoMoneda = NumaletPago.Moneda.Euro;
                    break;

            }

            lblNumerosLetras.Text = numLetras.ToCustomString(dTotal);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
    }

    /// <summary>
    /// Método que se encarga de cargar las deducciones
    /// </summary>
    private void fnCargarConceptosDeducciones()
    {
        DataView dvDeducciones = new DataView((DataTable)ViewState["dtDetalle"]);
        dvDeducciones.RowFilter = "Id_Tipo= " + ((int)clsEnumeraciones.TiposDetalleNomina.Deduccion).ToString();

        gdvConceptosDeducciones.DataSource = dvDeducciones.ToTable();
        gdvConceptosDeducciones.DataBind();
        gdvConceptosDeducciones.SelectedIndex = -1;
    }

    /// <summary>
    /// Método que se encarga de cargar las percepciones
    /// </summary>
    private void fnCargarConceptosPercepciones()
    {
        DataView dvPercepciones = new DataView((DataTable)ViewState["dtDetalle"]);
        dvPercepciones.RowFilter = "Id_Tipo= " + ((int)clsEnumeraciones.TiposDetalleNomina.Percepcion).ToString();

        gdvConceptosPercepciones.DataSource = dvPercepciones.ToTable();
        gdvConceptosPercepciones.DataBind();
        gdvConceptosPercepciones.SelectedIndex = -1;
    }

    /// <summary>
    /// Método qe se encarga de cargar los datos del Comprobante ligado a la Nómina
    /// </summary>
    /// <param name="dtComprobantePagoNomina">DataTable de los Datos del Comprobante</param>
    private void fnCargarDatosComprobanteInicio(DataTable dtComprobantePagoNomina)
    {
        object sender = new object();
        EventArgs e = new EventArgs();

        if (dtComprobantePagoNomina.Rows.Count <= 0)
        {
            return;
        }

        if (!string.IsNullOrEmpty(dtComprobantePagoNomina.Rows[0]["id_estructura_expedido"].ToString()))
        {
            cbAgrExpEn.Checked = true;
            cbAgrExpEn_CheckedChanged(sender, e);
            ddlSucursales.SelectedValue = dtComprobantePagoNomina.Rows[0]["id_estructura_expedido"].ToString();
        }

        string[] aLugar_Expedicion = dtComprobantePagoNomina.Rows[0]["lugar_expedicion"].ToString().Split(',');

        if (aLugar_Expedicion.Length > 0)
        {
            ddlEdoLugExp.SelectedValue = ddlEdoLugExp.Items.FindByText(aLugar_Expedicion[1].ToString()).Value;
        }

        ddlMetodoPago.SelectedValue = dtComprobantePagoNomina.Rows[0]["metodo_pago"].ToString();
        ddlMetodoPago_SelectedIndexChanged(sender, e);
        txtNumeroCuenta.Text = dtComprobantePagoNomina.Rows[0]["numero_cuenta"].ToString();
        ddlMoneda.SelectedValue = dtComprobantePagoNomina.Rows[0]["moneda"].ToString();
        txtRegimenfiscal.Text = dtComprobantePagoNomina.Rows[0]["regimen_fiscal"].ToString();
        txtTipoCambio.Text = dtComprobantePagoNomina.Rows[0]["tipo_cambio"].ToString();

    }

    /// <summary>
    /// Método que se encarga de cargar los datos del empleado
    /// </summary>
    /// <param name="pnId_Empleado">ID del Empleado</param>
    public void fnCargarDatosDatosEmpleadoInicio(int pnId_Empleado)
    {
        DataTable dtEmpleado = new DataTable();
        DateTime dtFechaInicioRelacionLaboral;

        clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
        dtEmpleado = cEmpleados.fnExisteEmpleado(Convert.ToInt32(pnId_Empleado));

        if (dtEmpleado.Rows.Count <= 0)
            return;

        ddlSucursalesFis.SelectedValue = dtEmpleado.Rows[0]["id_estructura"].ToString();
        ddlSucursalesFis_SelectedIndexChanged(new object(), new EventArgs());

        ViewState["Id_Empleado"] = dtEmpleado.Rows[0]["Id_Empleado"].ToString();

        lblRfcEmpleadoDescripion.Text = dtEmpleado.Rows[0]["RFC"].ToString();
        txtNombreEmpleado.Text = dtEmpleado.Rows[0]["Nombre"].ToString();
        lblRegistroPatronalDescripion.Text = dtEmpleado.Rows[0]["RegistroPatronal"].ToString();
        lblNumeroEmpleadoDescripion.Text = dtEmpleado.Rows[0]["NumEmpleado"].ToString();
        lblCURPDescripion.Text = dtEmpleado.Rows[0]["CURP"].ToString();
        lblNumeroSeguridadSocialDescripion.Text = dtEmpleado.Rows[0]["NumSeguridadSocial"].ToString();
        lblDepartamentoDescripion.Text = dtEmpleado.Rows[0]["Departamento"].ToString();
        lblCLABEDescripion.Text = dtEmpleado.Rows[0]["CLABE"].ToString();
        lblFechaInicioRelacionLaboralDescripion.Text = (dtEmpleado.Rows[0]["FechaInicioRelLaboral"].ToString().Equals(string.Empty) ? string.Empty : Convert.ToDateTime(dtEmpleado.Rows[0]["FechaInicioRelLaboral"].ToString()).ToShortDateString());
        lblPuestoDescripion.Text = dtEmpleado.Rows[0]["Puesto"].ToString();
        lblTipoContratoDescripion.Text = dtEmpleado.Rows[0]["TipoContrato"].ToString();
        lblTipoJornadaDescripion.Text = dtEmpleado.Rows[0]["TipoJornada"].ToString();
        lblSalarioBaseDescripion.Text = dtEmpleado.Rows[0]["SalarioBaseCotApor"].ToString();
        lblSalarioDiarioIntegradoDescripion.Text = dtEmpleado.Rows[0]["SalarioDiarioIntegrado"].ToString();

        lblTipoRegimenDescripion.Text = dtEmpleado.Rows[0]["DescripcionTipoRegimen"].ToString();
        lblBancoDescripion.Text = dtEmpleado.Rows[0]["DescripcionBanco"].ToString();
        lblRiesgoPuestoDescripion.Text = dtEmpleado.Rows[0]["DescripcionTipoRiesgoPuesto"].ToString();

        if (!string.IsNullOrEmpty(lblFechaInicioRelacionLaboralDescripion.Text))
        {
            dtFechaInicioRelacionLaboral = Convert.ToDateTime(lblFechaInicioRelacionLaboralDescripion.Text);
            TimeSpan tsFechaDiferencia = DateTime.Now - dtFechaInicioRelacionLaboral;
            int nSemanas = tsFechaDiferencia.Days / 7;
            txtAntiguedad.Text = nSemanas.ToString();
        }   
    }

    /// <summary>
    /// Método que se encarga de cargar los datos de una pago en especifico para la Generación de una Nómina
    /// </summary>
    /// <param name="pnId_Pago">ID del Pago de Nómina de un empleado</param>
    private void fnCargarDatosNominaGeneracion(int pnId_Pago)
    {
        DataTable dtComprobantePagoNomina = new DataTable();
        DataTable dtHorasExtra = new DataTable();
        DataTable dtIncapacidades = new DataTable();
        //DataTable dtNomina = new DataTable();
        DataTable dtPagoNomina = new DataTable();
        DataTable dtPercepcionesDeducciones = new DataTable();
        EventArgs e = new EventArgs();
        int nId_Empleado = 0;
        object sender = new object();
        try
        {
            clsTimbradoNomina cTimbradoNomina = new clsTimbradoNomina();
            dtPagoNomina = cTimbradoNomina.fnObtenerPagoNomina(pnId_Pago);

            // Modificado el 09/06/2014 Ismael Hidalgo
            // Ya no hay relación entre el Pago y la Nomina hasta despues del timbrado
            //dtNomina = cTimbradoNomina.fnObtenerNomina(pnId_Pago);

            if (dtPagoNomina.Rows.Count <= 0)
                return;

            //if (dtNomina.Rows.Count <= 0)
            //    return;

            dtComprobantePagoNomina = cTimbradoNomina.fnObtenerComprobantePagoNomina(pnId_Pago);
            fnCargarDatosComprobanteInicio(dtComprobantePagoNomina);

            //Session["Id_Nomina"] = "0";
            //ddlSucursalesFis.SelectedValue = dtEmpleado.Rows[0]["id_estructura"].ToString();
            //ddlSucursalesFis_SelectedIndexChanged(sender, e);

            nId_Empleado = Convert.ToInt32(dtPagoNomina.Rows[0]["Id_Empleado"].ToString());
            fnCargarDatosDatosEmpleadoInicio(nId_Empleado);

            /*Se modifico 05/06/2014. Ya no van a ser necesarias, se van a capturar en la Generación antes de Timbrar
            txtNumeroDiasPagos.Text = dtPagoNomina.Rows[0]["NumDiasPagados"].ToString();
            txtFechaInicialPago.Text = Convert.ToDateTime(dtPagoNomina.Rows[0]["FechaInicialPago"].ToString()).ToShortDateString();
            txtFechaFinalPago.Text = Convert.ToDateTime(dtPagoNomina.Rows[0]["FechaFinalPago"].ToString()).ToShortDateString();
            */
            Session["IdPago"] = pnId_Pago;

            dtPercepcionesDeducciones = cTimbradoNomina.fnObtenerPercepcionesDeducciones(pnId_Pago);

            if (dtPercepcionesDeducciones.Rows.Count <= 0)
                return;

            dtHorasExtra = cTimbradoNomina.fnObtenerHorasExtra(pnId_Pago);
            dtIncapacidades = cTimbradoNomina.fnObtenerIncapacidades(pnId_Pago);

            fnCargarPercepcionesDeduccionesInicio(dtPercepcionesDeducciones, dtIncapacidades, dtHorasExtra);
            fnCargarConceptosDeducciones();
            fnCargarConceptosPercepciones();
            fnCalcularTotalPercepciones();
            fnCalcularTotalDeduccion();
            fnCalcularTotales();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorCargaDatosRegistro);
        }
    }

    /// <summary>
    /// Método encargado de llenar el drop de los estados
    /// </summary>
    /// <param name="psIdPais">ID del País</param>
    private void fnCargarEstadosLugarExpedicion(string psId_Pais)
    {
        ddlEdoLugExp.DataSource = clsComun.fnLlenarDropEstados(psId_Pais);
        ddlEdoLugExp.DataBind();
    }

    /// <summary>
    /// Método que se encarga de cargar las horas extra de una percepción especifica
    /// </summary>
    private void fnCargarHorasExtra()
    {
        DataView dvHorasExtra = new DataView((DataTable)ViewState["dtHorasExtra"]);
        dvHorasExtra.RowFilter = "Id_PercepDedu = " + hfId_RegistroHoraExtra.Value;

        gdvHorasExtra.DataSource = dvHorasExtra.ToTable();
        gdvHorasExtra.DataBind();
        gdvHorasExtra.SelectedIndex = -1;
    }

    /// <summary>
    /// Método que se encarga de cargar las incapacidades de una deducción en especifico
    /// </summary>
    private void fnCargarIncapacidades()
    {
        DataView dvIncapacidades = new DataView((DataTable)ViewState["dtIncapacidades"]);
        dvIncapacidades.RowFilter = "Id_PercepcionDeduccion = " + hfId_RegistroIncapacidad.Value;

        gdvIncapacidades.DataSource = dvIncapacidades.ToTable();
        gdvIncapacidades.DataBind();
        gdvIncapacidades.SelectedIndex = -1;
    }

    /// <summary>
    /// Método encargado de llenar el drop de los países
    /// </summary>
    private void fnCargarPaisesLugarExpedicion()
    {
        ddlPaisLugExp.DataSource = clsComun.fnLlenarDropPaises();
        ddlPaisLugExp.DataBind();
    }

    /// <summary>
    /// Método que se encarga de cargar los detalles de la Nómina de un Empleado
    /// </summary>
    /// <param name="dPercepcionesDeducciones">DataTable de Percepciones y Deducciones</param>
    /// <param name="dtDetalleIncapacidades">DataTable de Incapacidades</param>
    /// <param name="dtDetalleHorasExta">DataTable de Horas Exta</param>
    private void fnCargarPercepcionesDeduccionesInicio(DataTable dPercepcionesDeducciones, DataTable dtDetalleIncapacidades, DataTable dtDetalleHorasExta)
    {
        DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];
        DataTable dtHorasExtra = (DataTable)ViewState["dtHorasExtra"];
        DataTable dtIncapacidades = (DataTable)ViewState["dtIncapacidades"];

        foreach (DataRow renglonDetalle in dPercepcionesDeducciones.Rows)
        {
            int nId_Registro = 0;

            DataRow RenglonDetalle = dtDetalle.NewRow();
            RenglonDetalle["Id_Tipo"] = Convert.ToInt32(renglonDetalle["Id_Tipo"].ToString());
            RenglonDetalle["Tipo"] = renglonDetalle["DescripcionTipo"].ToString();
            RenglonDetalle["Id_TipoPercepDedu"] = Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"].ToString());
            RenglonDetalle["Clave"] = renglonDetalle["Clave"].ToString();
            RenglonDetalle["Concepto"] = renglonDetalle["Concepto"].ToString();
            RenglonDetalle["ImporteGravado"] = Convert.ToDouble(renglonDetalle["ImporteGravado"].ToString());
            RenglonDetalle["ImporteExento"] = Convert.ToDouble(renglonDetalle["ImporteExento"].ToString());

            dtDetalle.Rows.Add(RenglonDetalle);

            nId_Registro = Convert.ToInt32(dtDetalle.Rows[dtDetalle.Rows.Count - 1][0].ToString());

            if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
            {
                DataView dvHorasExtra = new DataView(dtDetalleHorasExta);
                dvHorasExtra.RowFilter = "Id_PercepDedu = " + renglonDetalle["Id_PercepDedu"];

                foreach (DataRow renglonHoraExtra in dvHorasExtra.ToTable().Rows)
                {
                    DataRow RenglonHoraExtra = dtHorasExtra.NewRow();
                    RenglonHoraExtra["Id_PercepDedu"] = Convert.ToInt32(nId_Registro);
                    RenglonHoraExtra["Dias"] = Convert.ToInt32(renglonHoraExtra["Dias"]);
                    RenglonHoraExtra["TipoHoras"] = renglonHoraExtra["TipoHoras"];
                    RenglonHoraExtra["HorasExtra"] = Convert.ToInt32(renglonHoraExtra["HorasExtra"]);
                    RenglonHoraExtra["ImportePagado"] = Convert.ToDouble(renglonHoraExtra["ImportePagado"]);

                    dtHorasExtra.Rows.Add(RenglonHoraExtra);
                }

            }

            if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
            {
                DataView dvIncapacidades = new DataView(dtDetalleIncapacidades);
                dvIncapacidades.RowFilter = "Id_PercepcionDeduccion = " + renglonDetalle["Id_PercepDedu"];

                foreach (DataRow renglonIncapacidad in dvIncapacidades.ToTable().Rows)
                {
                    DataRow Renglon = dtIncapacidades.NewRow();
                    Renglon["Id_PercepcionDeduccion"] = nId_Registro;
                    Renglon["Tipo"] = Convert.ToInt32(renglonIncapacidad["Clave"].ToString());
                    Renglon["DiasIncapacidad"] = Convert.ToDouble(renglonIncapacidad["DiasIncapacidad"].ToString());
                    Renglon["TipoIncapacidad"] = renglonIncapacidad["DecripcionTipoIncapacidad"].ToString();
                    Renglon["Descuento"] = Convert.ToDouble(renglonIncapacidad["Descuento"].ToString());

                    dtIncapacidades.Rows.Add(Renglon);
                }
            }
        }

        ViewState["dtDetalle"] = dtDetalle;
        ViewState["dtHorasExtra"] = dtHorasExtra;
        ViewState["dtIncapacidades"] = dtIncapacidades;
    }

    /// <summary>
    /// Trae la lista de sucursales activas asignadas al usuario y las carga en el drop
    /// </summary>
    private void fnCargarSucursales()
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            ddlSucursales.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true);
            ddlSucursales.DataBind();

            //ddlSucursales.DataSource = ddlSucursales.DataSource;
            //ddlSucursales.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSucursales.DataSource = null;
            ddlSucursales.DataBind();
        }
    }

    /// <summary>
    /// Trae la lista filtrada de las sucursales de los emisores.
    /// </summary>
    private void fnCargarSucursalesExpedidoEn()
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();

            ddlSucursales.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true);
            ddlSucursales.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSucursales.DataSource = null;
            ddlSucursales.DataBind();
        }
    }

    /// <summary>
    /// Trae la lista filtrada de las sucursales fiscales.
    /// </summary>
    private void fnCargarSucursalesFiscal()
    {
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();
            ddlSucursalesFis.DataSource = clsTimbradoFuncionalidad.LlenarDropSucursales(datosUsuario.id_usuario); //clsComun.LlenarDropSucursales(true); //
            ddlSucursalesFis.DataBind();
        }

        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlSucursalesFis.DataSource = null;
            ddlSucursalesFis.DataBind();
        }
        catch
        {
            //referencia nula
        }
    }

    /// <summary>
    /// Método que se encarga de cargar las tipos de conceptos configurados
    /// </summary>
    private void fnCargarTipos()
    {
        clsOperacionTiposDeduccionPercepcion cOperacionTiposDeduccionPercepcion = new clsOperacionTiposDeduccionPercepcion();
        try
        {
            ddlTipo.DataSource = cOperacionTiposDeduccionPercepcion.fnListarTipos();
            ddlTipo.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlTipo.DataSource = null;
            ddlTipo.DataBind();
        }
    }

    /// <summary>
    /// Método que se encarga de cargar las percepciones configuradas
    /// </summary>
    private void fnCargarTipoDeduccionPercepcion(int pnId_Tipo)
    {
        clsOperacionTiposDeduccionPercepcion cOperacionTiposDeduccionPercepcion = new clsOperacionTiposDeduccionPercepcion();
        try
        {
            ddlTipoDeduccionPercepcion.DataSource = cOperacionTiposDeduccionPercepcion.fnListarTiposDeduccionPercepcion(pnId_Tipo);
            ddlTipoDeduccionPercepcion.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlTipoDeduccionPercepcion.DataSource = null;
            ddlTipoDeduccionPercepcion.DataBind();
        }
    }

    /// <summary>
    /// Método que se encarga de cargar los tipos de incapacidades
    /// </summary>
    private void fnCargarTiposIncapacidades()
    {
        clsOperacionTiposIncapacidades cOperacionTiposIncapacidades = new clsOperacionTiposIncapacidades();
        try
        {
            ddlTipoIncapacidad.DataSource = cOperacionTiposIncapacidades.fnListarTiposIncapacidades();
            ddlTipoIncapacidad.DataBind();
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            ddlTipoIncapacidad.DataSource = null;
            ddlTipoIncapacidad.DataBind();
        }
    }

    /// <summary>
    /// Método que se encarga de expandir el panel de horas extra
    /// </summary>
    private void fnExpandirPanelHorasExtra()
    {
        cpeConceptosHorasExtra.Collapsed = false;
        cpeConceptosHorasExtra.ClientState = "false";
    }

    /// <summary>
    /// Método que se encarga de expandir el panel de incapacidades
    /// </summary>
    private void fnExpadirPanelIncapacidades()
    {
        cpeConceptosIncapacidades.Collapsed = false;
        cpeConceptosIncapacidades.ClientState = "false";
    }

    /// <summary>
    /// Método que se encarga de inicializar los valores de los data tables de los conceptos de nómina
    /// </summary>
    private void fnInicializaConceptosNomina()
    {
        // DataTable de Detalle
        DataTable dtDetalle = new DataTable();
        DataColumn[] keysDetalle = new DataColumn[1];

        // Create column 1.
        DataColumn dcIdDetalle = new DataColumn();
        dcIdDetalle.DataType = typeof(int);
        dcIdDetalle.ColumnName = "Id_PercepDedu";
        dcIdDetalle.AutoIncrement = true;
        dcIdDetalle.AutoIncrementSeed = 1;
        dcIdDetalle.AutoIncrementStep = 1;
        dtDetalle.Columns.Add(dcIdDetalle);
        keysDetalle[0] = dcIdDetalle;

        dtDetalle.PrimaryKey = keysDetalle;
        dtDetalle.Columns.Add("Id_Tipo", typeof(int));
        dtDetalle.Columns.Add("Tipo", typeof(string));
        dtDetalle.Columns.Add("Id_TipoPercepDedu", typeof(int));
        dtDetalle.Columns.Add("Clave", typeof(string));
        dtDetalle.Columns.Add("Concepto", typeof(string));
        dtDetalle.Columns.Add("ImporteGravado", typeof(decimal));
        dtDetalle.Columns.Add("ImporteExento", typeof(decimal));

        // Fin DataTable de Detalle
        //////////////////////////////////

        // DataTable de Horas Extra
        DataTable dtHorasExtra = new DataTable();
        DataColumn[] keysHoraExtra = new DataColumn[1];

        DataColumn dcIdHoraExtra = new DataColumn();
        dcIdHoraExtra.DataType = typeof(int);
        dcIdHoraExtra.ColumnName = "Id_HoraExtra";
        dcIdHoraExtra.AutoIncrement = true;
        dcIdHoraExtra.AutoIncrementSeed = 1;
        dcIdHoraExtra.AutoIncrementStep = 1;
        dtHorasExtra.Columns.Add(dcIdHoraExtra);
        keysHoraExtra[0] = dcIdHoraExtra;

        dtHorasExtra.PrimaryKey = keysHoraExtra;
        dtHorasExtra.Columns.Add("Id_PercepDedu", typeof(int));
        dtHorasExtra.Columns.Add("Dias", typeof(int));
        dtHorasExtra.Columns.Add("TipoHoras", typeof(string));
        dtHorasExtra.Columns.Add("HorasExtra", typeof(int));
        dtHorasExtra.Columns.Add("ImportePagado", typeof(decimal));

        // Fin DataTable de Horas Extra
        //////////////////////////////////

        // DataTable de Incapacidades
        DataTable dtIncapacidades = new DataTable();
        DataColumn[] keysIncapacidad = new DataColumn[1];

        DataColumn dcIdIncapacidad = new DataColumn();
        dcIdIncapacidad.DataType = typeof(int);
        dcIdIncapacidad.ColumnName = "Id_Incapacidad";
        dcIdIncapacidad.AutoIncrement = true;
        dcIdIncapacidad.AutoIncrementSeed = 1;
        dcIdIncapacidad.AutoIncrementStep = 1;
        dtIncapacidades.Columns.Add(dcIdIncapacidad);
        keysIncapacidad[0] = dcIdIncapacidad;

        dtIncapacidades.PrimaryKey = keysIncapacidad;
        dtIncapacidades.Columns.Add("Id_PercepcionDeduccion", typeof(int));
        dtIncapacidades.Columns.Add("Tipo", typeof(int));
        dtIncapacidades.Columns.Add("DiasIncapacidad", typeof(decimal));
        dtIncapacidades.Columns.Add("TipoIncapacidad", typeof(string));
        dtIncapacidades.Columns.Add("Descuento", typeof(decimal));

        // Fin DataTable de Incapacidades
        //////////////////////////////////

        ViewState["dtDetalle"] = dtDetalle;
        ViewState["dtHorasExtra"] = dtHorasExtra;
        ViewState["dtIncapacidades"] = dtIncapacidades;

        hfId_Registro.Value = "0";
        hfId_Incapacidad.Value = "0";
        hfId_Hora_Extra.Value = "0";
        hfId_RegistroIncapacidad.Value = "0";
        hfId_RegistroHoraExtra.Value = "0";
    }

    /// <summary>
    /// Método que se encarga de hablitar o deshabilitar los campos de captura
    /// </summary>
    /// <param name="pbAccion">Acción a realizar</param>
    private void fnHabilitaDeshabilitaCamposCaptura(bool pbAccion)
    {
        txtAntiguedad.Enabled = pbAccion;
        /*Se modifico 05/06/2014. Ya no van a ser necesarias, se van a capturar en la Generación antes de Timbrar
        txtFechaPago.Enabled = pbAccion;
        txtFechaInicialPago.Enabled = pbAccion;
        txtFechaFinalPago.Enabled = pbAccion;
        txtNumeroDiasPagos.Enabled = pbAccion;
        */
        ddlTipo.Enabled = pbAccion;
        ddlTipoDeduccionPercepcion.Enabled = pbAccion;
        txtClaveConcepto.Enabled = pbAccion;
        txtConcepto.Enabled = pbAccion;
        txtImporteGravado.Enabled = pbAccion;
        txtImporteExento.Enabled = pbAccion;
        txtDiasIncapacidad.Enabled = pbAccion;
        ddlTipoIncapacidad.Enabled = pbAccion;
        txtDescuentoIncapacidad.Enabled = pbAccion;
        txtDiasHorasExtra.Enabled = pbAccion;
        ddlTipoHorasExtra.Enabled = pbAccion;
        txtHorasExtra.Enabled = pbAccion;
        txtImportePagadoHorasExtra.Enabled = pbAccion;
    }

    /// <summary>
    /// Método que se encarga de limpiar los capos de captura
    /// </summary>
    private void fnLimpiarCamposCaptura()
    {
        object sender = new object();
        EventArgs e = new EventArgs();

        DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];
        dtDetalle.Rows.Clear();
        DataTable dtHorasExtra = (DataTable)ViewState["dtHorasExtra"];
        dtHorasExtra.Rows.Clear();
        DataTable dtIncapacidades = (DataTable)ViewState["dtIncapacidades"];
        dtIncapacidades.Rows.Clear();

        ViewState["dtDetalle"] = dtDetalle;
        ViewState["dtHorasExtra"] = dtHorasExtra;
        ViewState["dtIncapacidades"] = dtIncapacidades;

        hfId_Registro.Value = "0";
        hfId_Incapacidad.Value = "0";
        hfId_Hora_Extra.Value = "0";
        hfId_RegistroIncapacidad.Value = "0";
        hfId_RegistroHoraExtra.Value = "0";

        gdvConceptosDeducciones.DataSource = null;
        gdvConceptosDeducciones.DataBind();
        gdvConceptosPercepciones.DataSource = null;
        gdvConceptosPercepciones.DataBind();
        btnCancelarDetalle_Click(sender, e);

        gdvHorasExtra.DataSource = null;
        gdvHorasExtra.DataBind();
        btnCancelarHoraExtra_Click(sender, e);

        gdvIncapacidades.DataSource = null;
        gdvIncapacidades.DataBind();
        btnCancelarIncapacidad_Click(sender, e);

        lblTotalGravadoPercepciones.Text = String.Format("{0:c6}", 0);
        lblTotalExentoPercepciones.Text = String.Format("{0:c6}", 0);

        lblTotalGravadoDeducciones.Text = String.Format("{0:c6}", 0);
        lblTotalExentoDeducciones.Text = String.Format("{0:c6}", 0);

        lblTotalesPercepciones.Visible = false;
        lblTotalGravadoPercepciones.Visible = false;
        lblTotalExentoPercepciones.Visible = false;
        tblPercepcionesTotales.Visible = false;

        lblTotalesDeducciones.Visible = false;
        lblTotalGravadoDeducciones.Visible = false;
        lblTotalExentoDeducciones.Visible = false;
        tblDeduccionesTotales.Visible = false;

        lblDetSubtotal.Text = String.Format("{0:c6}", 0);
        lblDetDescuentoVal.Text = String.Format("{0:c6}", 0);
        lblISRVal.Text = String.Format("{0:c6}", 0);
        lblTotalVal.Text = String.Format("{0:c6}", 0);

        lblSubtotal.Visible = false;
        lblDetDescuento.Visible = false;
        lblISR.Visible = false;
        lblTotal.Visible = false;

        lblDetSubtotal.Visible = false;
        lblDetDescuentoVal.Visible = false;
        lblISRVal.Visible = false;
        lblTotalVal.Visible = false;
    }

    /// <summary>
    /// Método que se encarga de limpiar los datos del Empleado
    /// </summary>
    private void fnLimpiarDatosEmpleado()
    { 
        lblRfcEmpleadoDescripion.Text = string.Empty;
        txtNombreEmpleado.Text = string.Empty;
        lblRegistroPatronalDescripion.Text = string.Empty;
        lblNumeroEmpleadoDescripion.Text = string.Empty;
        lblCURPDescripion.Text = string.Empty;
        lblNumeroSeguridadSocialDescripion.Text = string.Empty;
        lblDepartamentoDescripion.Text = string.Empty;
        lblCLABEDescripion.Text = string.Empty;
        lblFechaInicioRelacionLaboralDescripion.Text = string.Empty;
        lblPuestoDescripion.Text = string.Empty;
        lblTipoContratoDescripion.Text = string.Empty;
        lblTipoJornadaDescripion.Text = string.Empty;
        lblSalarioBaseDescripion.Text = string.Empty;
        lblSalarioDiarioIntegradoDescripion.Text = string.Empty;
        lblTipoRegimenDescripion.Text = string.Empty;
        lblBancoDescripion.Text = string.Empty;
        lblRiesgoPuestoDescripion.Text = string.Empty;
        txtAntiguedad.Text = string.Empty;
    }

    /// <summary>
    /// Método que se encarga de ocultar el panel de incapacidades
    /// </summary>
    private void fnOcultarPanelConceptosIncapacidades()
    {
        cpeConceptosIncapacidades.Collapsed = true;
        cpeConceptosIncapacidades.ClientState = "true";
    }

    /// <summary>
    /// Método que se encarga de ocultar el panel de horas extra
    /// </summary>
    private void fnOcultarPanelConceptosHorasExtra()
    {
        cpeConceptosHorasExtra.Collapsed = true;
        cpeConceptosHorasExtra.ClientState = "true";
    }

    /// <summary>
    /// Método que se encarga de obtener los datos de la sucursal en la sección de Expedido En
    /// </summary>
    /// <param name="pnId_Sucursal">ID de la Sucursal</param>
    private void fnObtieneDomicilioSucursalExpedidoEn(int pnId_Sucursal)
    {
        datosUsuario = clsComun.fnUsuarioEnSesion();
        clsOperacionSucursales cOperacionSucursales = new clsOperacionSucursales();
        clsOperacionCuenta cOperacionCuenta = new clsOperacionCuenta();

        DataTable sdrDomicilio = cOperacionSucursales.fnObtenerDomicilioSuc(pnId_Sucursal);
        DataTable sdrDatosEmisor = cOperacionCuenta.fnObtenerDatosFiscalesSuc(pnId_Sucursal);
        try
        {
            lblDomicilioExpedidoEn.Text = string.Empty;
            lblUbicacionExpedidoEn.Text = string.Empty;

            if (sdrDomicilio != null && sdrDomicilio.Rows.Count > 0  && sdrDatosEmisor != null && sdrDatosEmisor.Rows.Count > 0 )
            {
                lblDomicilioExpedidoEn.Text = "Calle " + sdrDomicilio.Rows[0]["calle"].ToString() + " ";
                lblDomicilioExpedidoEn.Text += "No. Ext " + sdrDomicilio.Rows[0]["numero_exterior"].ToString() + " ";
                lblDomicilioExpedidoEn.Text += "No. Int " + sdrDomicilio.Rows[0]["numero_interior"].ToString() + " ";
                lblDomicilioExpedidoEn.Text += "Col. " + sdrDomicilio.Rows[0]["colonia"].ToString() + " ";


                lblUbicacionExpedidoEn.Text += sdrDomicilio.Rows[0]["localidad"].ToString() + ", ";
                lblUbicacionExpedidoEn.Text += sdrDomicilio.Rows[0]["municipio"].ToString() + ", ";
                lblUbicacionExpedidoEn.Text += sdrDomicilio.Rows[0]["estado"].ToString() + ", México ";
                lblUbicacionExpedidoEn.Text += "C.P. " + sdrDomicilio.Rows[0]["codigo_postal"].ToString();


                lblRFCExpedidoEn.Text = "RFC " + sdrDatosEmisor.Rows[0]["rfc"].ToString();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Método que se encarga de obtener los datos del domicilio de la sucursal
    /// </summary>
    /// <param name="pnId_Sucursal">ID de la Sucursal</param>
    private void fnObtenerDomicilioSucursalFiscal(int pnId_Sucursal)
    {
        clsOperacionSucursales gOpeSucursal = new clsOperacionSucursales();
        clsOperacionCuenta gDAL = new clsOperacionCuenta();
        string sDireccionFis = string.Empty;
        string sUbicacionFis = string.Empty;
        try
        {
            datosUsuario = clsComun.fnUsuarioEnSesion();

            DataTable asd = gOpeSucursal.fnObtenerDomicilioSuc(pnId_Sucursal);
            DataTable sdrInfo = gDAL.fnObtenerDatosFiscalesSuc(pnId_Sucursal);

            if (asd != null && asd.Rows.Count > 0 && sdrInfo != null && sdrInfo.Rows.Count > 0)
            {
                ddlEdoLugExp.SelectedValue = asd.Rows[0]["id_estado"].ToString();

                if (asd.Rows[0]["calle"].ToString() != string.Empty)
                    sDireccionFis += "Calle " + asd.Rows[0]["calle"].ToString() + " "; ;

                if (asd.Rows[0]["numero_interior"].ToString() != string.Empty)
                    sDireccionFis += "No. Int " + asd.Rows[0]["numero_interior"].ToString() + " ";

                if (asd.Rows[0]["numero_exterior"].ToString() != string.Empty)
                    sDireccionFis += "No. Ext " + asd.Rows[0]["numero_exterior"].ToString() + " ";

                if (asd.Rows[0]["colonia"].ToString() != string.Empty)
                    sDireccionFis += "Col. " + asd.Rows[0]["colonia"].ToString() + " ";

                lblDireccionFis.Text = sDireccionFis;

                if (asd.Rows[0]["localidad"].ToString() != string.Empty)
                    sUbicacionFis += asd.Rows[0]["localidad"].ToString() + ", ";

                if (asd.Rows[0]["municipio"].ToString() != string.Empty)
                    sUbicacionFis += asd.Rows[0]["municipio"].ToString() + ", ";

                if (asd.Rows[0]["estado"].ToString() != string.Empty)
                    sUbicacionFis += asd.Rows[0]["estado"].ToString() + ", ";

                sUbicacionFis += "México "; //la cadena "México" esta fijo por q no hay manera de obtenerlo en tablas ya que no hay registro solo estado, municipio y localidad

                if (asd.Rows[0]["codigo_postal"].ToString() != string.Empty)
                    sUbicacionFis += "C.P " + asd.Rows[0]["codigo_postal"].ToString() + " ";

                lblUbicacionFis.Text = sUbicacionFis;

                lblRFCFis.Text = "RFC " + sdrInfo.Rows[0]["rfc"].ToString();

                //if (asd["regimen_fiscal"].ToString() != string.Empty)
                //    txtRegimenfiscal.Text = asd["regimen_fiscal"].ToString();
            }
            else
            {
                lblDireccionFis.Text = string.Empty;
                lblUbicacionFis.Text = string.Empty;
                lblRFCFis.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Función que se encarga de validar que se capturen horas extra capturadas cuando exista una percepción de ese tipo
    /// </summary>
    /// <returns></returns>
    private bool fnValidarCapturaHorasExtra()
    {
        bool bResultado = false;

        DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];
        DataTable dtHorasExtra = (DataTable)ViewState["dtHorasExtra"];


        foreach (DataRow renglon in dtDetalle.Rows)
        {
            if (Convert.ToInt32(renglon["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                    Convert.ToInt32(renglon["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
                {
                    DataView dvHorasExtra = new DataView(dtHorasExtra);
                    dvHorasExtra.RowFilter = "Id_PercepDedu= " + renglon["Id_PercepDedu"];

                    if (dvHorasExtra.ToTable().Rows.Count <= 0)
                        return bResultado;
                }
        }

        bResultado = true;

        return bResultado;
    }

    /// <summary>
    /// Función que se encarga de validar que se capturen incapacidades cuando exista una deducción de ese tipo
    /// </summary>
    /// <returns></returns>
    private bool fnValidarCapturaIncapacidades()
    {
        bool bResultado = false;

        DataTable dtDetalle = (DataTable)ViewState["dtDetalle"];
        DataTable dtIncapacidades = (DataTable)ViewState["dtIncapacidades"];

        foreach (DataRow renglon in dtDetalle.Rows)
        {
            if (Convert.ToInt32(renglon["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                    Convert.ToInt32(renglon["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
            {
                DataView dvIncapacidades = new DataView(dtIncapacidades);
                dvIncapacidades.RowFilter = "Id_PercepcionDeduccion= " + renglon["Id_PercepDedu"];

                if (dvIncapacidades.ToTable().Rows.Count <= 0)
                    return bResultado;
            }
        }

        bResultado = true;

        return bResultado;
    }
}

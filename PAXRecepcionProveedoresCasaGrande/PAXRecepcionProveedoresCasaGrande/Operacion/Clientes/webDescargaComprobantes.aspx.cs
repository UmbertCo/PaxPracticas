using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Globalization;

public partial class Operacion_Clientes_webDescargaComprobantes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                fnCargarSucursales();
                txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");


            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/webGlobalError.aspx");
        }

    }



    private void fnCargarSucursales()
    {
        DataTable dtSucursales = new clsOperacionSucursales().fnObtenerSucursalesEmpresas();
        ddlSucursal.DataSource = dtSucursales;
        ddlSucursal.DataValueField = "id_sucursal";
        ddlSucursal.DataTextField = "empresa_sucursal";
        ddlSucursal.DataBind();
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtFecha.Text)
            //|| string.IsNullOrEmpty(txtRfc.Text)
            //|| string.IsNullOrEmpty(txtTotalFactura.Text)
            )
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varValidacionError);
            return;
        }
        int nFolio = 0;
        DateTime dtFecha = new DateTime();
        string sRfc = string.Empty;
        double nTotalFactura = 0;
        string sIdSucursal = string.Empty;
        string sSerie = string.Empty;
        string sUuid = string.Empty;
        int nIdSucursal = 0;

        if (!string.IsNullOrEmpty(txtFolio.Text))
        {
            try
            {

                nFolio = Convert.ToInt32(txtFolio.Text);
                if (nFolio <= 0)
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varFolioRequerido);
                    return;
                }
            }
            catch (FormatException ex)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varFolioNumerico);
                return;
            }
        }
        
       

        try
        {
            dtFecha = DateTime.ParseExact(txtFecha.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        catch (FormatException)
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valFormatoFecha);
            return;

        }
        sRfc = txtRfc.Text.Trim();
        if (!clsComun.fnValidaExpresion(sRfc, "[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valRfcCadena);
            return;
        }
        if (!string.IsNullOrEmpty(txtTotalFactura.Text))
        {
            try
            {
                if (!clsComun.fnValidaExpresion(txtTotalFactura.Text.Trim(), "^[0-9]+(\\.[0-9]{2})?$"))
                {
                    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valPrecio);
                    return;
                }
                nTotalFactura = Convert.ToDouble(txtTotalFactura.Text);
            }
            catch (FormatException ex)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valTotalNumerico);
                return;
            }
        }
        sSerie = txtSerie.Text.Trim();
        nIdSucursal = Convert.ToInt32(ddlSucursal.SelectedValue);
        DataTable dtComprobantes = new clsOperacionComprobantes().fnObtenerComprobantesCliente(nIdSucursal, sRfc, sSerie, nFolio, dtFecha, txtTotalFactura.Text);
        if (dtComprobantes.Rows.Count > 0)
        {
            gvComprobantes.DataSource = dtComprobantes;
            gvComprobantes.DataBind();
            mpeConsultaCFDI.Show();
         
        }
        else
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valComprobanteNoEncontrado);
        }
        
        //string sIdComprobante = new clsOperacionComprobantes().fnObtenerComprobanteClienteSp(nIdSucursal, sRfc, sSerie, nFolio, dtFecha, txtTotalFactura.Text);
        //if (string.IsNullOrEmpty(sIdComprobante))
        //{
        //    clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valComprobanteNoEncontrado);
        //    return;
        //}
        //new clsOperacionComprobantes().fnComprobanteDescargadoCliente(Convert.ToInt32(sIdComprobante));
        //int nIdPlantilla = new clsOperacionSucursales().fnObtenerIdTipoPlantilla(nIdSucursal);
        //hpXML.NavigateUrl = "~/Consultas/webDescargarXML.aspx?idcomprobante=" + sIdComprobante + "&tipocomprobante=1";
        //hpPDF.NavigateUrl = "~/Consultas/webDescargaPDF.aspx?idcomprobante=" + sIdComprobante + "&tipocomprobante=1&tipoplantilla=" + nIdPlantilla;
        //mpeConsultaCFDI.Show();
    }

    protected void btnConsultaCDFI_Click(object sender, EventArgs e)
    {
        //hpXML.NavigateUrl = "_blank";
        //hpPDF.NavigateUrl = "_blank";
        gvComprobantes.DataSource = new DataTable();
        gvComprobantes.DataBind();
        mpeConsultaCFDI.Hide();
        txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtFolio.Text = "";
        txtRfc.Text = "";
        txtTotalFactura.Text = "";
        ddlSucursal.SelectedIndex = 0;
        txtSerie.Text = "";
        cbSeleccionar.Checked = false;
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
    protected void cbSeleccionar_CheckedChanged(object sender, EventArgs e)
    {

        if (cbSeleccionar.Checked)
        {
            foreach (GridViewRow renglon in gvComprobantes.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                CbCan.Checked = true;

            }
        }
        else
        {
            foreach (GridViewRow renglon in gvComprobantes.Rows)
            {
                CheckBox CbCan;

                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                CbCan.Checked = false;

            }
        }

    }
    protected void btnDescargar_Click(object sender, EventArgs e)
    {
        try
        {
            //Pasar parámetros de consulta para la descarga masiva de comprobantes
            //Receptor|Estatus|Sucursal|Documentos|Series|Folio inicio|Folio fin|Fecha inicio|Fecha fin|Usuario


            clsInicioSesionUsuario usuarioActivo = clsComun.fnUsuarioEnSesion();
            string sListaComprobantes = null;
            foreach (GridViewRow renglon in gvComprobantes.Rows)
            {
                CheckBox CbCan;
                CbCan = (CheckBox)(renglon.FindControl("cbSeleccion"));
                if (CbCan.Checked)
                {
                    Label sIdComprobante = ((Label)renglon.FindControl("lblidcomprobante"));
                    if (sListaComprobantes != null)
                    {
                        sListaComprobantes = sListaComprobantes + "," + sIdComprobante.Text;
                    }
                    else
                    {
                        sListaComprobantes = sIdComprobante.Text;
                    }
                }
            }


            if (!string.IsNullOrEmpty(sListaComprobantes))
            {

                lblMensaje.Text = string.Empty;
                //Se encriptan los parámetros
                string sParamEncriptados = Utilerias.Encriptacion.Base64.EncriptarBase64(sListaComprobantes);


                ScriptManager.RegisterStartupScript(this, this.GetType(), "descargarComprobantes",
                                                            String.Format("<script>window.open('{0}?p={1}','123','height=160, width= 355, status=yes, resizable= no, scrollbars= no, toolbar= no,menubar= no');</script>",
                                                                           "webDescargarComprobantesCliente.aspx", sParamEncriptados), false);
            }
            else
            {
                lblMensaje.Text= Resources.resCorpusCFDIEs.varDescargaVacio;

            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        finally
        {
            mpeConsultaCFDI.Show();
        }
    }
}
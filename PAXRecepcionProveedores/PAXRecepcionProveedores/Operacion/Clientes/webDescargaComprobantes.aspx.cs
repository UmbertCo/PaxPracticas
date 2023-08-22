using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Globalization;

namespace PAXRecepcionProveedores.Operacion.Clientes
{
    public partial class webDescargaComprobantes : System.Web.UI.Page
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
            if (string.IsNullOrEmpty(txtFolio.Text)
                || string.IsNullOrEmpty(txtFecha.Text)
                || string.IsNullOrEmpty(txtRfc.Text)
                || string.IsNullOrEmpty(txtTotalFactura.Text)
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
            int nIdSucursal = 0;
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
            try
            {
                dtFecha = DateTime.ParseExact(txtFecha.Text,"dd/MM/yyyy",CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valFormatoFecha);
                return;
                
            }
            sRfc = txtRfc.Text;
            if (!clsComun.fnValidaExpresion(sRfc, "[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valRfcCadena);
                return;
            }

            try
            {
                if (!clsComun.fnValidaExpresion(txtTotalFactura.Text, "^[0-9]+(\\.[0-9]{2})?$"))
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
            sSerie = txtSerie.Text.Trim();
            nIdSucursal = Convert.ToInt32(ddlSucursal.SelectedValue);
            string sIdComprobante = new clsOperacionComprobantes().fnObtenerComprobanteCliente(nIdSucursal, sRfc, sSerie, nFolio, dtFecha,txtTotalFactura.Text);
            if(string.IsNullOrEmpty(sIdComprobante))
            {
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.valComprobanteNoEncontrado);
                return;
            }
            int nIdPlantilla = new clsOperacionSucursales().fnObtenerIdTipoPlantilla(nIdSucursal);
            hpXML.NavigateUrl = "~/Consultas/webDescargarXML.aspx?idcomprobante="+sIdComprobante+"&tipocomprobante=1";
            hpPDF.NavigateUrl = "~/Consultas/webDescargaPDF.aspx?idcomprobante=" + sIdComprobante + "&tipocomprobante=1&tipoplantilla="+nIdPlantilla;
            mpeConsultaCFDI.Show();
        }

        protected void btnConsultaCDFI_Click(object sender, EventArgs e)
        {
            hpXML.NavigateUrl = "_blank";
            hpPDF.NavigateUrl = "_blank";
            mpeConsultaCFDI.Hide();
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtFolio.Text = "";
            txtRfc.Text = "";
            txtTotalFactura.Text = "";
            ddlSucursal.SelectedIndex = 0;
            txtSerie.Text = "";
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
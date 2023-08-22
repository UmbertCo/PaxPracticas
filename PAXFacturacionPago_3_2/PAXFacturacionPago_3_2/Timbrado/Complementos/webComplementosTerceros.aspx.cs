using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Timbrado_Complementos_webComplementosTerceros : System.Web.UI.Page
{
    public DataTable TablaComplementos;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cbIVARet.Attributes.Add("onclick", "javascript:enableCheckISR();");
            cbISRRet.Attributes.Add("onclick", "javascript:enableCheckIVARet();");
            cbIVATra.Attributes.Add("onclick", "javascript:enableCheckIEPSTra();");
            cbIEPSTra.Attributes.Add("onclick", "javascript:enableCheckIVATra();");
            //btnTerceros2.Attributes.Add("onclick", "javascript:enableValidador();");

            //LimpiaCampos();
            txtFechaIniT.Text = DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
            btnCerrar.Attributes.Add("onclick", "window.close();");
        }

    }
    protected void btnTerceros_Click(object sender, EventArgs e)
    {
        try
        {

            Complementos sComplemento = new Complementos();
            TablaComplementos = new DataTable();
            TablaComplementos = sComplemento.fnTerceros(tbVersionTerceros.Text, tbRFCTerceros.Text, cbIVARet, tbimporterIVARet.Text,
                cbISRRet, tbimporteISRRet.Text, cbIVATra, tbtasaIVATra.Text, tbimporteIVATra.Text, cbIEPSTra, tbtasaIEPSTra.Text, tbimporteIEPSTra.Text, 
                txtcalleT.Text, txtNumExtT.Text, txtNumIntT.Text, txtColoniaT.Text,
                txtLocalidadT.Text, txtReferenciaT.Text, txtMunicipioT.Text, txtEstadoT.Text, txtPaisT.Text, txtCodigoT.Text.PadLeft(5, '0'), txtnumeroT.Text,
                Convert.ToDateTime(txtFechaIniT.Text), txtaduanaT.Text, string.Empty /*txtPredialT.Text*/, txtnombreT.Text, txtCantidadT.Text, txtDescripcionT.Text);
            Session["TablaComplementos"] = TablaComplementos;
            //Session["CuentaPredialTerceros"] = txtPredialT.Text;
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
        }
        catch(Exception ex)
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
        }
    }

    public void LimpiaCampos()
    {
        
        tbRFCTerceros.Text= string.Empty;
        tbimporterIVARet.Text = "0";
        tbimporteISRRet.Text = "0";
        tbtasaIVATra.Text = string.Empty; 
        tbimporteIVATra.Text = string.Empty;
        txtcalleT.Text= string.Empty;
        txtNumExtT.Text= string.Empty;
        txtNumIntT.Text= string.Empty;
        txtColoniaT.Text= string.Empty;
        txtLocalidadT.Text= string.Empty;
        txtReferenciaT.Text= string.Empty;
        txtMunicipioT.Text= string.Empty;
        txtEstadoT.Text= string.Empty;
        txtPaisT.Text= string.Empty;
        txtCodigoT.Text= string.Empty;
        txtnumeroT.Text= string.Empty;
        txtFechaIniT.Text= string.Empty;
        txtaduanaT.Text= string.Empty;
        txtPredialT.Text= string.Empty;
        txtnombreT.Text = string.Empty;
        Session["TablaComplementos"] = null;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Timbrado_Complementos_webComplementoLeyendasFiscales : System.Web.UI.Page
{
    public DataTable TablaComplementos;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tbVersionLeyFis.Text = "1.0";
            LimpiaCampos();           
            btnCerrar.Attributes.Add("onclick", "window.close();");
        }
    }
    protected void btnLFicales_Click(object sender, EventArgs e)
    {
        try
        {
            Complementos sComplemento = new Complementos();
            TablaComplementos = new DataTable();
            TablaComplementos = sComplemento.fnLeyendasFiscales(tbVersionLeyFis.Text, tbDiposicionFiscal.Text, txtnormaLeyFis.Text, tbLeyendaFis.Text);
            Session["TablaComplementos"] = TablaComplementos;
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
        }
        catch
        {
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
        }
    }

    public void LimpiaCampos()
    {
        txtnormaLeyFis.Text = string.Empty;
        tbDiposicionFiscal.Text = string.Empty;
        tbLeyendaFis.Text = string.Empty;
        Session["TablaComplementos"] = null;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Timbrado_Complementos_webComplementosDonatarias : System.Web.UI.Page
{
    public DataTable TablaComplementos;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tbVersionDon.Text = "1.1";
            fnCargarTiposDonativos();
            LimpiaCampos();
            tbLeyendaDon.Text = "Este comprobante ampara un donativo, el cual será destinado por la donataria a los fines propios de su objeto social. En el caso de que los bienes donados hayan sido deducidos previamente para los efectos del impuesto sobre la renta, este donativo no es deducible. La reproducción no autorizada de este comprobante constituye un delito en los términos de las disposiciones fiscales.";            
            btnCerrar.Attributes.Add("onclick", "window.close();");
        }
    }
    protected void btnDonativas_Click(object sender, EventArgs e)
    {
         try
         {
             Complementos sComplemento = new Complementos();
             TablaComplementos = new DataTable();
             DateTime FechaDON = Convert.ToDateTime(txtFechaIni.Text);
             TablaComplementos = sComplemento.fnDonativas(tbVersionDon.Text, FechaDON, tbAutorizacionDon.Text, tbLeyendaDon.Text);
             Session["TablaComplementos"] = TablaComplementos;
             Session["TipoDonativo"] = ddlTipoDonativo.SelectedValue;
             clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
         }
         catch
         {
             clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varComplemento, Resources.resCorpusCFDIEs.lblContribuyente);
         }
    }
    public void LimpiaCampos()
    {
        txtFechaIni.Text = string.Empty;
         tbAutorizacionDon.Text= string.Empty;
         tbLeyendaDon.Text = string.Empty;
         Session["TablaComplementos"] = null;
         ddlTipoDonativo.SelectedValue = "0";
         Session["TipoDonativo"] = null;
    }

    public void fnCargarTiposDonativos()
    {
        ListItem[] items = new ListItem[2];

        items[0] = new ListItem();
        items[0].Text = "Monetario";
        items[0].Value = "0";
        items[1] = new ListItem();
        items[1].Text = "Especie";
        items[1].Value = "1";

        ddlTipoDonativo.Items.AddRange(items);
    }
}
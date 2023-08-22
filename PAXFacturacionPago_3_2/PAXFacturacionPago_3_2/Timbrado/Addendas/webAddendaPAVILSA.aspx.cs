using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using System.Threading;
using System.Globalization;

public partial class Timbrado_Addendas_webAddendaPAVILSA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!(Page.IsPostBack))
            {
                btnCerrar.Attributes.Add("onclick", "window.close();");
                txtSubTot.Text = string.Format("{0:n6}", Convert.ToDouble(Session["lblDetSubtotal"].ToString()));
                txtTotal.Text = string.Format("{0:n6}", Convert.ToDouble(Session["lblTotalVal"].ToString()));
                //Reiniciar valores en capos
                fnLimpiarCampos();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

    private void fnLimpiarCampos()
    {
        ddlMonedaArt.SelectedValue = "MXN";
        txtNoComp.Text = string.Empty;
        txtNoProv.Text = string.Empty;
        txtOrdCom.Text = string.Empty;
        Session.Remove("lblDetSubtotal");
        Session.Remove("lblTotalVal");
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            XmlDocument xmladenda = new XmlDocument();
            string sArmXML = fnArmarXML();
            if (!(string.IsNullOrEmpty(sArmXML)))
            {
                xmladenda.LoadXml(sArmXML);
                Session["AddendaGenerada"] += xmladenda.OuterXml;
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                btnGuardar.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    private string fnArmarXML()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<Documento>");
        sb.Append("<FacturaPAVILSA>");
        
        sb.Append("<Total>");
        sb.Append(txtTotal.Text.Replace(",",""));
        sb.Append("</Total>");
        sb.Append("<SubTotal>");
        sb.Append(txtSubTot.Text.Replace(",", ""));
        sb.Append("</SubTotal>");
        sb.Append("<Moneda>");
        sb.Append(ddlMonedaArt.Text);
        sb.Append("</Moneda>");
        sb.Append("<numCompania>");
        sb.Append(txtNoComp.Text);
        sb.Append("</numCompania>");
        sb.Append("<numProveedor>");
        sb.Append(txtNoProv.Text);
        sb.Append("</numProveedor>");
        sb.Append("<numOrdenCompra>");
        sb.Append(txtOrdCom.Text);
        sb.Append("</numOrdenCompra>");

        sb.Append("</FacturaPAVILSA>");
        sb.Append("</Documento>");
        
        return sb.ToString();
    }
}
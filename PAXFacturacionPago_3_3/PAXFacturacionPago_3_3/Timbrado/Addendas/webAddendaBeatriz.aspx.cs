using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Timbrado_Addendas_webAddendaBeatriz : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnCerrar.Attributes.Add("onclick", "window.close();");
            txtFechaGuia.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            txtFechaPago.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
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
                clsComun.fnMessage(this, Resources.resCorpusCFDIEs.varAlta, Resources.resCorpusCFDIEs.lblContribuyente, "small", "Error.png");
                btnGuardar.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "btnGuardar_Click", "webAddendaBeatriz");
        }
    }

    private string fnArmarXML()
    {
        StringBuilder sb = new StringBuilder();

        string sFeguia, sFepago, sNoGuia, sOrdenCompra, sOrigen, sDestino, sUnidad;
        sFeguia = sFepago = sNoGuia = sOrdenCompra = sOrigen = sDestino = sUnidad = string.Empty;

        sFeguia = txtFechaGuia.Text;
        sFepago = txtFechaPago.Text;
        sNoGuia = txtNoGuia.Text;
        sOrdenCompra = txtOrdenCompra.Text;
        sOrigen = txtOrigen.Text;
        sDestino = txtDestino.Text;
        sUnidad = txtUnidad.Text;

        sb.Append("<Documento>");
        sb.Append("<CargoExpress>");

        sb.Append("<FechaGuia>");
        sb.Append(sFeguia);
        sb.Append("</FechaGuia>");

        sb.Append("<Fechapago>");
        sb.Append(sFepago);
        sb.Append("</Fechapago>");

        sb.Append("<Noguia>");
        sb.Append(sNoGuia);
        sb.Append("</Noguia>");

        sb.Append("<Origen>");
        sb.Append(sOrigen);
        sb.Append("</Origen>");

        sb.Append("<OrigenCompra>");
        sb.Append(sOrdenCompra);
        sb.Append("</OrigenCompra>");

        sb.Append("<Destino>");
        sb.Append(sDestino);
        sb.Append("</Destino>");

        sb.Append("<Unidad>");
        sb.Append(sUnidad);
        sb.Append("</Unidad>");


        sb.Append("</CargoExpress>");
        sb.Append("</Documento>");

        return sb.ToString();
    }
}
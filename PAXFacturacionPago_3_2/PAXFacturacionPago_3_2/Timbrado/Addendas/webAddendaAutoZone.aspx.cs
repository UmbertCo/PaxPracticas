using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Timbrado_Addendas_webAddendaAutoZone : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            btnCerrar.Attributes.Add("onclick", "window.close();");
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
                clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                btnGuardar.Enabled = false;
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

    /// <summary>
    /// Función que se encarga de armar la Addenda de AutoZone
    /// </summary>
    /// <returns></returns>
    private string fnArmarXML()
    {
        XmlDocument addenda = new XmlDocument();

        XmlElement xeAutoZone = addenda.CreateElement("ADDENDA20");
        xeAutoZone.SetAttribute("VERSION", txtVersion.Text);
        xeAutoZone.SetAttribute("VENDOR_ID", txtVendorId.Text);
        xeAutoZone.SetAttribute("DEPTID", txtDeptId.Text);
        xeAutoZone.SetAttribute("BUYER", txtBuyer.Text);
        xeAutoZone.SetAttribute("EMAIL", txtEmail.Text);

        XmlAttribute atXsi = addenda.CreateAttribute("xsi:noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
        atXsi.Value = "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd";
        xeAutoZone.SetAttributeNode(atXsi);

        addenda.AppendChild(xeAutoZone);
        return addenda.OuterXml.ToString();
    }

}
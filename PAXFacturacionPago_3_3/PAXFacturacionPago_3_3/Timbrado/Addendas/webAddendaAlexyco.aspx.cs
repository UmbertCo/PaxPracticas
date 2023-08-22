using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Timbrado_Addendas_webAddendaAlexyco : System.Web.UI.Page
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
            Session["AddendaGenerada"] = null;
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "btnGuardar_Click", "webAddendaAlexyco");
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
        XmlNode xmlnode;
        XmlNode xmlnode2;
        XmlNode xmlnode3;
        XmlNode xmlnode4;

        xmlnode = addenda.CreateNode(XmlNodeType.Element, "kn", "KNRECEPCION", "http://www.w3.org/2001/XMLSchema");


        XmlAttribute idAttribute = addenda.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema");
        idAttribute.Value = "C:\\Users\\david.dominguez\\Desktop\\XSD_17_11_2016.xsd";

        xmlnode.Attributes.Append(idAttribute);
        addenda.AppendChild(xmlnode);

        xmlnode2 = addenda.CreateNode(XmlNodeType.Element, "kn", "Tipo", "http://www.w3.org/2001/XMLSchema");
        xmlnode.AppendChild(xmlnode2);

        xmlnode3 = addenda.CreateNode(XmlNodeType.Element, "kn", "FacturasKN", "http://www.w3.org/2001/XMLSchema");
        xmlnode2.AppendChild(xmlnode3);

        //NODOS PRINCIPALES

        xmlnode4 = addenda.CreateNode(XmlNodeType.Element, "kn", "Purchase_Order", "http://www.w3.org/2001/XMLSchema");
        xmlnode4.InnerText = txtOrdenDeCompraAlexyco.Text;
        xmlnode3.AppendChild(xmlnode4);


        xmlnode4 = addenda.CreateNode(XmlNodeType.Element, "kn", "FileNumber_GL", "http://www.w3.org/2001/XMLSchema");
        xmlnode4.InnerText = txtNumeroDeArchivoAlexyco.Text;
        xmlnode3.AppendChild(xmlnode4);


        xmlnode4 = addenda.CreateNode(XmlNodeType.Element, "kn", "Branch_Centre", "http://www.w3.org/2001/XMLSchema");
        xmlnode4.InnerText = txtRamaCentroAlexyco.Text;
        xmlnode3.AppendChild(xmlnode4);


        xmlnode4 = addenda.CreateNode(XmlNodeType.Element, "kn", "TransportRef", "http://www.w3.org/2001/XMLSchema");
        xmlnode4.InnerText = txtReferenciaDeTransporteAlexyco.Text;
        xmlnode3.AppendChild(xmlnode4);

        return addenda.OuterXml.ToString();
    }
    protected void btnCerrar_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
    }
}
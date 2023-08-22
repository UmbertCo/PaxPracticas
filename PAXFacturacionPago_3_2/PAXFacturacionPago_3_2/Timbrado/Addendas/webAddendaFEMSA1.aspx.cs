using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class Timbrado_Addendas_webAddendaFEMSA1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LimpiaCampos();
            tbVersion.Text = "1";
            Session["AddendaGenerada"] = null;
            Session["AddendaNamespace"] = null;
            Button1.Enabled = true;
            Button1.ValidationGroup = "grupoConsulta";
            btnCerrar.Attributes.Add("onclick", "window.close();");
        }
    }
    public void LimpiaCampos()
    {
        tbNumeroRemision.Text = string.Empty;
        tbNumeroEmpleado.Text = string.Empty;
        tbCentro.Text = string.Empty;
        tbProveedor.Text = string.Empty;
        txtFechaIni.Text = string.Empty;
        tbNumDocto.Text = string.Empty;
        txtFechaFin.Text = string.Empty;
        tbCorreo.Text = string.Empty;
        tbEntradaSAP.Text = string.Empty;
        tbRetencion1.Text = string.Empty;
        tbRetencion2.Text = string.Empty;
        tbRetencion3.Text = string.Empty;
        ddltipodocto.SelectedValue = "1";
        ddlmoneda.SelectedValue = "MXN";
        ddlretencion1.SelectedValue = "1";
        ddlretencion2.SelectedValue = "1";
        ddlretencion3.SelectedValue = "1";
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        XmlDocument xmladenda = new XmlDocument();
        //fnCrearAddenda();
        xmladenda.LoadXml(ArmarXML());
        Session["AddendaGenerada"] += xmladenda.OuterXml;
        clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
        Button1.Enabled = false;

    }

    public string ArmarXML()
    {
        XmlDocument peticion = new XmlDocument();
        XmlNode xmlnode;
        //XmlDeclaration xmldeclaracion;

        // Creo la línea de declaración de dos formas diferentes.
        // Esta por métodos de Xdocument
        //xmlnode = peticion.CreateNode(XmlNodeType.XmlDeclaration, string.Empty, string.Empty);
        //peticion.AppendChild(xmlnode);

        //// Le indico el encoding
        //xmldeclaracion = (XmlDeclaration)peticion.FirstChild;
        //xmldeclaracion.Encoding = "UTF-8";

        // Añado el raiz peticion
        xmlnode = peticion.CreateNode(XmlNodeType.Element, "Documento", "");
        peticion.AppendChild(xmlnode);


        // Le añado el nodo cuerpo
        XmlNode nodoPeticion = peticion.DocumentElement;
        XmlNode nodoCuerpo = peticion.CreateNode(XmlNodeType.Element, "FacturaFemsa", null);
        nodoPeticion.AppendChild(nodoCuerpo);

        // Le añado los elementos id y secreto y le doy valores.
        XmlElement elemento0 = peticion.CreateElement("noVersAdd");
        XmlElement elemento1 = peticion.CreateElement("claseDoc");
        XmlElement elemento2 = peticion.CreateElement("noSociedad");
        XmlElement elemento3 = peticion.CreateElement("noProveedor");
        XmlElement elemento4 = peticion.CreateElement("noPedido");
        XmlElement elemento5 = peticion.CreateElement("moneda");
        XmlElement elemento6 = peticion.CreateElement("noEntrada");
        XmlElement elemento7 = peticion.CreateElement("noRemision");
        XmlElement elemento8 = peticion.CreateElement("noSocio");
        XmlElement elemento9;
        if (ddltipodocto.SelectedValue != "2")
        {
            elemento9 = peticion.CreateElement("centroCostos");
        }
        else
        {
            elemento9 = peticion.CreateElement("centro");
        }
        XmlElement elemento10 = peticion.CreateElement("iniPerLiq");
        XmlElement elemento11 = peticion.CreateElement("finPerLiq");
        XmlElement elemento12 = peticion.CreateElement("retencion1");
        XmlElement elemento13 = peticion.CreateElement("email");

        nodoCuerpo.AppendChild(elemento0);
        nodoCuerpo.AppendChild(elemento1);
        nodoCuerpo.AppendChild(elemento2);
        nodoCuerpo.AppendChild(elemento3);
        nodoCuerpo.AppendChild(elemento4);
        nodoCuerpo.AppendChild(elemento5);
        nodoCuerpo.AppendChild(elemento6);
        nodoCuerpo.AppendChild(elemento7);
        nodoCuerpo.AppendChild(elemento8);
        nodoCuerpo.AppendChild(elemento9);
        nodoCuerpo.AppendChild(elemento10);
        nodoCuerpo.AppendChild(elemento11);
        nodoCuerpo.AppendChild(elemento12);

        if (ddltipodocto.SelectedValue != "2")
        {

            if (tbRetencion2.Text != string.Empty)
            {
                XmlElement elemento15 = peticion.CreateElement("retencion2");
                nodoCuerpo.AppendChild(elemento15);
            }

            if (tbRetencion3.Text != string.Empty)
            {
                if (tbRetencion2.Text == string.Empty)
                {
                    XmlElement elemento16 = peticion.CreateElement("retencion2");
                    nodoCuerpo.AppendChild(elemento16);
                }
                else
                {
                    XmlElement elemento16 = peticion.CreateElement("retencion3");
                    nodoCuerpo.AppendChild(elemento16);
                }
            }
        }
        nodoCuerpo.AppendChild(elemento13);



        peticion.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText = tbVersion.Text;
        peticion.ChildNodes[0].ChildNodes[0].ChildNodes[1].InnerText = ddltipodocto.SelectedValue;
        peticion.ChildNodes[0].ChildNodes[0].ChildNodes[2].InnerText = tbSociedad.SelectedValue;
        peticion.ChildNodes[0].ChildNodes[0].ChildNodes[3].InnerText = tbProveedor.Text;
        peticion.ChildNodes[0].ChildNodes[0].ChildNodes[5].InnerText = ddlmoneda.SelectedValue;

        if (ddltipodocto.SelectedValue == "1")
        {
            peticion.ChildNodes[0].ChildNodes[0].ChildNodes[4].InnerText = tbNumDocto.Text;
            peticion.ChildNodes[0].ChildNodes[0].ChildNodes[6].InnerText = tbEntradaSAP.Text;

            if (tbRetencion1.Text != string.Empty)
            {
                peticion.ChildNodes[0].ChildNodes[0].ChildNodes[12].InnerText = ddlretencion1.SelectedValue + "-" + tbRetencion1.Text;
            }

            if (tbRetencion2.Text != string.Empty)
            {
                peticion.ChildNodes[0].ChildNodes[0].ChildNodes[13].InnerText = ddlretencion2.SelectedValue + "-" + tbRetencion2.Text;
            }

            if (tbRetencion3.Text != string.Empty)
            {
                if (tbRetencion2.Text == string.Empty)
                {
                    peticion.ChildNodes[0].ChildNodes[0].ChildNodes[13].InnerText = ddlretencion3.SelectedValue + "-" + tbRetencion3.Text;
                }
                else
                {
                    peticion.ChildNodes[0].ChildNodes[0].ChildNodes[14].InnerText = ddlretencion3.SelectedValue + "-" + tbRetencion3.Text;
                }
            }
        }
        peticion.ChildNodes[0].ChildNodes[0].ChildNodes[7].InnerText = tbNumeroRemision.Text;

        //peticion.ChildNodes[0].ChildNodes[0].ChildNodes[8].InnerText = tbNumeroEmpleado.Text;
        if (ddltipodocto.SelectedValue == "2")
        {
            peticion.ChildNodes[0].ChildNodes[0].ChildNodes[9].InnerText = tbCentro.Text;
            DateTime fechaInicio = Convert.ToDateTime(txtFechaIni.Text);
            DateTime fechaFin = Convert.ToDateTime(txtFechaFin.Text);

            peticion.ChildNodes[0].ChildNodes[0].ChildNodes[10].InnerText = fechaInicio.Day + "." + fechaInicio.Month + "." + fechaInicio.Year;
            peticion.ChildNodes[0].ChildNodes[0].ChildNodes[11].InnerText = fechaFin.Day + "." + fechaFin.Month + "." + fechaFin.Year;
        }

        int nodos = peticion.ChildNodes[0].ChildNodes[0].ChildNodes.Count;
        peticion.ChildNodes[0].ChildNodes[0].LastChild.InnerText = tbCorreo.Text; ;


        return peticion.OuterXml;
    }

    protected void ddltipodocto_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddltipodocto.SelectedValue == "2")
        {
            Button1.ValidationGroup = "grupoCosignacion";
        }
        else if (ddltipodocto.SelectedValue == "1")
        {
            Button1.ValidationGroup = "grupoConsulta";
        }
        else
        {
            Button1.ValidationGroup = "grupoGeneral";
        }
    }
    protected void btnCerrar_Click(object sender, EventArgs e)
    {

    }
}
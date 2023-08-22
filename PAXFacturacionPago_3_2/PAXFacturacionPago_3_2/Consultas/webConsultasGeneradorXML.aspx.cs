using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.Xml.Serialization;

public partial class Consultas_webConsultasGeneradorXML : System.Web.UI.Page
{
    private clsOperacionConsulta gDAL;
    private clsConfiguracionAddenda gADD;
    private clsEnvioCorreoDocs gEvM;
    clsInicioSesionUsuario datosUsuario;
    private clsOperacionTimbradoSellado gTimbrado;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                int nIdContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;
                string sIdCfd = Request.QueryString["nic"];

                if (!string.IsNullOrEmpty(sIdCfd))
                    fnGenerarXML(nIdContribuyente, sIdCfd);

            }
            catch (Exception)
            {
                //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
            }
        }
    }

    private void fnGenerarXML(int pnIdContribuyente, string psIdCfd)
    {
        gDAL = new clsOperacionConsulta();
        gADD = new clsConfiguracionAddenda();
        datosUsuario = clsComun.fnUsuarioEnSesion();
        XmlDocument comprobante = new XmlDocument();
        DataTable addenda = new DataTable();
        string AddendaNamespace = string.Empty;
        clsNodoTimbre CreadorAddenda = new clsNodoTimbre();
        int idEstructura = 0;
        int idUsuario = 0;
        string snombreDoc=string.Empty;
        Guid Gid;
        Gid = Guid.NewGuid();

        try
        {

            comprobante = gDAL.fnObtenerComprobanteXML(pnIdContribuyente, psIdCfd);

            // Create an XML declaration. 
            XmlDeclaration xmldecl;
            xmldecl = comprobante.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";
            xmldecl.Standalone = null;

            // Add the new node to the document.
            XmlElement root = comprobante.DocumentElement;
            comprobante.InsertBefore(xmldecl, root);



            idEstructura = gADD.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
            idUsuario = gADD.fnObtieneIdUsuarioporContribuyente(pnIdContribuyente);
            addenda = gADD.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructura);
            if (addenda.Rows.Count > 0)
            {
                XmlDocument xAddenda = new XmlDocument();
                int idModulo = Convert.ToInt32(addenda.Rows[0]["id_modulo"]);
                xAddenda.LoadXml(Convert.ToString(addenda.Rows[0]["addenda"]));
                AddendaNamespace = gADD.fnObtieneNameSpace(idModulo);
               
                if (AddendaNamespace != "")
                {
                    string[] nombre = AddendaNamespace.Split('=');
                    XmlAttribute xAttribute = comprobante.CreateAttribute(nombre[0]);
                    xAttribute.InnerText = AddendaNamespace;
                    comprobante.ChildNodes[1].Attributes.Append(xAttribute);
                }

                XmlNode childElement = comprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", comprobante.DocumentElement.NamespaceURI);
                comprobante.ChildNodes[1].AppendChild(childElement);

                // El Mudulo 6 es de la addenda de AHMSA por lo que al agregar el nodo, agregaremos los atributos y 
                // el primer nodo de la addenda 
                if (idModulo.Equals(6))
                {
                    XmlAttribute atXmlns = comprobante.CreateAttribute("xmlns:ahmsa", "http://www.w3.org/2000/xmlns/");
                    atXmlns.Value = "http://www.ahmsa.com/xsd/AddendaAHM1";
                    childElement.Attributes.SetNamedItem(atXmlns);

                    XmlAttribute atXsi = comprobante.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    atXsi.Value = "http://www.ahmsa.com/xsd/AddendaAHM1 http://www.ahmsa.com/xsd/AddendaAHM1/AddendaAHM.xsd";
                    childElement.Attributes.SetNamedItem(atXsi);

                    childElement.InnerXml = xAddenda.FirstChild.InnerXml;
                }
                else if (idModulo.Equals(7))
                {
                    XmlElement xeAutoZone = comprobante.CreateElement("ADDENDA20");
                    XPathNavigator navAutoZone = xAddenda.CreateNavigator();

                    xeAutoZone.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    xeAutoZone.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd");
                    xeAutoZone.SetAttribute("VERSION", navAutoZone.SelectSingleNode("/ADDENDA20/@VERSION").Value);
                    xeAutoZone.SetAttribute("VENDOR_ID", navAutoZone.SelectSingleNode("/ADDENDA20/@VENDOR_ID").Value);
                    xeAutoZone.SetAttribute("DEPTID", navAutoZone.SelectSingleNode("/ADDENDA20/@DEPTID").Value);
                    xeAutoZone.SetAttribute("BUYER", navAutoZone.SelectSingleNode("/ADDENDA20/@BUYER").Value);
                    xeAutoZone.SetAttribute("EMAIL", navAutoZone.SelectSingleNode("/ADDENDA20/@EMAIL").Value);

                    childElement.AppendChild(xeAutoZone);
                }
                else 
                {
                    childElement.InnerXml = xAddenda.OuterXml; 
                }
            }

            clsComun.fnNuevaPistaAuditoria(
                "webConsultasGeneradorXML",
                "fnGenerarXML",
                "Se generó el XML para el comprobante con ID " + psIdCfd
                );

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(comprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            XPathNavigator navEncabezado = comprobante.CreateNavigator();
    
            try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
            catch { snombreDoc = Gid.ToString(); }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/xml";
            Response.AddHeader("Content-Disposition", "attachment; filename="+ snombreDoc +".xml");
            Response.Write(comprobante.InnerXml);
            //Response.Write(comprobante.OuterXml);
            Response.End();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
        catch (Exception)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
            //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varErrorDescarga);
        }
    }
}
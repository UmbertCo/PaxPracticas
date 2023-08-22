using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web
{
    public partial class GenerarAddenda : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnCapturarAddenda.Attributes.Add("onclick", "javascript:url();");
            }
        }
        protected void btnPegarAddenda_Click(object sender, EventArgs e)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDocument AddendaX = new XmlDocument();
            XPathNavigator navEncabezado;
            XmlElement elemento;
            string sComprobante = string.Empty;
            string sAdendda = string.Empty;
            try
            {
                sComprobante = Convert.ToString(Session["Comprobante"]);
                XmlDoc.LoadXml(sComprobante);

                sAdendda = Convert.ToString(Session["AddendaGenerada"]);
                AddendaX.LoadXml(sAdendda);

                XmlNode childElement = XmlDoc.CreateNode(XmlNodeType.Element, "cfdi:Addenda", XmlDoc.DocumentElement.NamespaceURI);

                //Opción para crear la addenda con xmlElement
                XmlElement xeAutoZone = XmlDoc.CreateElement("ADDENDA20");
                XPathNavigator navAutoZone = AddendaX.CreateNavigator();
                xeAutoZone.SetAttribute("VERSION", navAutoZone.SelectSingleNode("/ADDENDA20/@VERSION").Value);
                xeAutoZone.SetAttribute("VENDOR_ID", navAutoZone.SelectSingleNode("/ADDENDA20/@VENDOR_ID").Value);
                xeAutoZone.SetAttribute("DEPTID", navAutoZone.SelectSingleNode("/ADDENDA20/@DEPTID").Value);
                xeAutoZone.SetAttribute("BUYER", navAutoZone.SelectSingleNode("/ADDENDA20/@BUYER").Value);
                xeAutoZone.SetAttribute("EMAIL", navAutoZone.SelectSingleNode("/ADDENDA20/@EMAIL").Value);
                xeAutoZone.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeAutoZone.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd");
                childElement.AppendChild(xeAutoZone);

                //Opción para agregar a la addenda el schemaInstance cuando no la trae
                XmlNode childElementAddenda20 = XmlDoc.CreateNode(XmlNodeType.Element, "ADDENDA20", "");
                XmlAttribute atXsi = XmlDoc.CreateAttribute("xsi:noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                atXsi.Value = "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd";
                childElementAddenda20.Attributes.Append(atXsi);
                childElement.AppendChild(childElementAddenda20);

                //Opción para agregar la addenda directamente al nodo
                //childElement.InnerXml = AddendaX.InnerXml;
                childElement.AppendChild(AddendaX.FirstChild);

                //childElement.InnerXml = xeAutoZone.OuterXml;
                XmlDoc.ChildNodes[1].AppendChild(childElement);
                //childElement.InnerXml = AddendaX.InnerXml;





                //XmlAttribute atXsi = XmlDoc.CreateAttribute("xsi:noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                //atXsi.Value = "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd";
                //nodoAddenda[0].Attributes.Append(atXsi);

                //XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                //new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName), new Attribute (xsi + "noNamespaceSchemaLocation", "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd");

                //using (XmlWriter xw = XmlWriter.Create(XmlDoc.GetElementsByTagName("ADDENDA20").Item(0).OuterXml))
                //{
                //    xw.WriteAttributeString(XNamespace.Xmlns.ToString(), "xsi", xsi.NamespaceName, "http://www.w3.org/2001/XMLSchema-instance");
                //}

                //XmlNode nodoAddenda = XmlDoc.GetElementsByTagName("ADDENDA20").Item(0);
                //XmlAttribute atXsi = XmlDoc.CreateAttribute("xsi:noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                //atXsi.Value = "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd";
                ////nodoAddenda.Attributes.Append(atXsi);
                //XmlDoc.GetElementsByTagName("ADDENDA20").Item(0).Attributes.Append(atXsi);

                //XmlNode childElement = XmlDoc.CreateNode(XmlNodeType.Element, "cfdi:Addenda", XmlDoc.DocumentElement.NamespaceURI);
                //childElement.AppendChild(nodoAddenda);
                //XmlDoc.ChildNodes[1].AppendChild(childElement);

                //XmlAttribute atXsi = addenda.CreateAttribute("xsi:noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                //navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/ADDENDA20", nsmComprobante).CreateAttribute("xmlns", "xsi", "http://www.w3.org/2001/XMLSchema-instance", "http://www.w3.org/2001/XMLSchema-instance");
                //xeAutoZone.SetAttributeNode(atXsi);

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "text/xml";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "AutoZone" + ".xml");
                Response.Write(XmlDoc.InnerXml);
                //Response.Write(comprobante.OuterXml);
                Response.End();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
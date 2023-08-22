using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace SolucionPruebas.Presentacion.Web
{
    public partial class Addendas : System.Web.UI.Page
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
                if (Session["Comprobante"] == null)
                {
                    return;
                }

                XmlDocument xmladenda = new XmlDocument();
                string sArmXML = fnArmarXML();
                if (!(string.IsNullOrEmpty(sArmXML)))
                {
                    xmladenda.LoadXml(sArmXML);
                    //Session["AddendaGenerada"] += xmladenda.OuterXml;
                    Session["AddendaGenerada"] += sArmXML;
                    //clsComun.fnMostrarMensaje(this, Resources.resCorpusCFDIEs.varAlta);
                    btnGuardar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Función que se encarga de armar la Addenda de AutoZone
        /// </summary>
        /// <returns></returns>
        private string fnArmarXML()
        {
            XmlDocument addenda = new XmlDocument();
            XmlDocument xComprobante = new XmlDocument();
            xComprobante.LoadXml(Convert.ToString(Session["Comprobante"]));
            //XmlElement xNodoPadre = addenda.CreateElement("AutoZone");
            XmlElement xeAutoZone = xComprobante.CreateElement("ADDENDA20");
            xeAutoZone.SetAttribute("VERSION", txtVersion.Text);
            xeAutoZone.SetAttribute("VENDOR_ID", txtVendorId.Text);
            xeAutoZone.SetAttribute("DEPTID", txtDeptId.Text);
            xeAutoZone.SetAttribute("BUYER", txtBuyer.Text);
            xeAutoZone.SetAttribute("EMAIL", txtEmail.Text);

            //xeAutoZone.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //xeAutoZone.SetAttribute("xsi:noNamespaceSchemaLocation", "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd");

            XmlAttribute atXsi = addenda.CreateAttribute("xsi:noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            atXsi.Value = "http://azfest.autozone.com/fssit91/XSD/Addenda_Non_Merch_32.xsd";
            xeAutoZone.SetAttributeNode(atXsi);

            //addenda.AppendChild(xeAutoZone);

            //return addenda.OuterXml.ToString();
            return xeAutoZone.OuterXml;
        }

        public string fnValidate()
        {
            XmlDocument document = new XmlDocument();
            string retorno = string.Empty;
            try
            {
                document.LoadXml(Settings.Default.addenda);

                XPathNavigator navigator = document.CreateNavigator();

                DataTable tblComplementos = new DataTable();

                //tblComplementos = clsComun.fnObtenerXSDComplementos();

                XmlTextReader tr;

                tr = new XmlTextReader(new System.IO.StringReader(Settings.Default.esquema));
                document.Schemas.Add("http://www.ahmsa.com/xsd/AddendaAHM1", tr);


                ValidationEventHandler validation = new ValidationEventHandler(SchemaValidationHandler);

                document.Validate(validation);

            }
            catch (Exception)
            {
                return "Revise el esquema del XML y reintente de nuevo.";
            }


            return retorno;
        }

        public void SchemaValidationHandler(object sender, ValidationEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Mensaje" + DateTime.Now.Millisecond.ToString(), "jAlert('" + e.Message + "', 'Error');", true);
        }
    }
}
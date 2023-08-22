using SolucionPruebas.Presentacion.Servicios;
using SolucionPruebas.Presentacion.Servicios.CancelacionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Cancelacion
{
    public partial class webCancelacion : System.Web.UI.Page
    {
        private Servicios.CancelacionService.wcfCancelaASMXSoapClient SDCancelacionASMX;
        private Servicios.wsCancelaPruebas.wcfCancelaASMXSoapClient SDCancelacionPruebasASMX;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnVerAddenda_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            try
            {
                HttpFileCollection hfc = Request.Files;
                HttpPostedFile hpf = hfc[0];

                if (hpf.ContentLength < 0)
                    return;

                document.Load(hpf.InputStream);

                nsm = new XmlNamespaceManager(document.NameTable);
                nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                //string sUUID = document.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value;
                string sUUID = "96b0efb4-83e9-48a9-bdd9-0d675fb7a0b8";

                //Servicios.wsCancelaPruebas.ArrayOfString aUUID = new Servicios.wsCancelaPruebas.ArrayOfString();
                Servicios.CancelacionService.ArrayOfString aUUID = new Servicios.CancelacionService.ArrayOfString();
                aUUID.Add(sUUID.ToUpper());

                SDCancelacionASMX = ProxyLocator.ObtenerServicioCancelacionWCF();
                txtResultado.Text = SDCancelacionASMX.fnCancelarXML(aUUID, "AAA010101AAA", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==");

                //SDCancelacionPruebasASMX = ProxyLocator.ObtenerServicioCancelacionPruebas();
                //SDCancelacionPruebasASMX.fnCancelarXML(aUUID, "AAA010101AAA", 0, "ismael.hidalgo", "");                
            }
            catch (Exception ex)
            { 
            
            }
        }
    }
}
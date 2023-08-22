using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Timbrado
{
    public partial class TimbradoHash : System.Web.UI.Page
    {
        private Servicios.ServicioLocal.ServiceClient SDServicioLocal;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnObtenerHash_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            string sCadenaOriginal = string.Empty;
            string sSello = string.Empty;
            string sSelloRuta = string.Empty;
            string sSelloPatin = string.Empty;
            string sResultado = string.Empty;
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            try
            {
                HttpFileCollection hfc = Request.Files;
                HttpPostedFile hpf = hfc[0];

                if (hpf.ContentLength < 0)
                    return;

                document.Load(hpf.InputStream);

                //document.
                SDServicioLocal = Servicios.ProxyLocator.ObtenerServicioLocal();
                sCadenaOriginal = SDServicioLocal.fnAplicarHojaTransformacion(document.InnerXml);
                string  sHash = GetHASH(sCadenaOriginal);
                
            }
            catch (Exception ex)
            {

            }    
        }

        public string GetHASH(string text)
        {
            byte[] hashValue;
            byte[] message = Encoding.UTF8.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }
}
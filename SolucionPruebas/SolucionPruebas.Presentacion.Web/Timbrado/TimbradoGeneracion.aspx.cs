using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Timbrado
{
    public partial class TimbradoGeneracion : System.Web.UI.Page
    {
        private Servicios.wsGeneracionTimbrado.wcfRecepcionASMXSoapClient SDGeneracionTimbrado;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            string sResultado = string.Empty;
            string sCadenaOriginal = string.Empty;
            string sSello = string.Empty;
            string sLayout = string.Empty;
            try
            {
                HttpFileCollection hfc = Request.Files;
                HttpPostedFile hpf = hfc[0];

                if (hpf.ContentLength < 0)
                    return;
               
                StreamReader sr = new StreamReader(hpf.InputStream, System.Text.Encoding.UTF8, true);
                sLayout = sr.ReadToEnd();

                SDGeneracionTimbrado = Servicios.ProxyLocator.ObtenerServicioGeneracionTimbrado();
                sResultado = SDGeneracionTimbrado.fnEnviarTXT(sLayout, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2", "GT");
            }
            catch (Exception ex)
            {

            } 
        }
    }
}
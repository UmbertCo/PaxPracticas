using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using PAXServicioRecuperacionSOAP.ServiceReference1;

namespace PAXServicioRecuperacionSOAP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            string dir = PAXServicioRecuperacionSOAP.Properties.Settings.Default.rutaLogs.ToString();
            

            if (File.Exists(dir + "trace.log"))
                File.Delete(dir + "trace.log");

            if (File.Exists(dir + "archivo.log"))
                File.Delete(dir + "archivo.log");

            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

            try
            {
                //wcfRecepcionASMX.wcfRecepcionASMXSoapClient facturacion = new wcfRecepcionASMX.wcfRecepcionASMXSoapClient();
                Service1SoapClient ws = new Service1SoapClient();

                var requestInterceptor = new InspectorBehavior();
                ws.Endpoint.Behaviors.Add(requestInterceptor);

                ws.validarRFC("AAAA000000AAA");

                //XmlDocument xDocumento = new XmlDocument();

                //StreamReader sr = new StreamReader(PAXServicioRecuperacionSOAP.Properties.Settings.Default.rutaXML);

                //xDocumento.LoadXml(sr.ReadToEnd());

                //string mensajeServicio = facturacion.fnEnviarXML(xDocumento.OuterXml, "factura", 0, "WSDL_PAX", "wrTCvMS1w6/Dn8Sqw6HDscS1xIIzwoHEinnCssOtYMKjwrTDh+++pO+/gR3vvrLvvrTvv5bvvK/vvqI=", "3.2");

                //char[] cCad = { '-' };
                //string[] sCad = mensajeServicio.Split(cCad);

                //if (sCad.Length <= 3)
                //{
                //    string smensajefinal = string.Empty;
                //    for (int n = 0; n < sCad.Length; n++)
                //    {
                //        string sMensaje = sCad[n];
                //        smensajefinal += sMensaje + " ";
                //    }

                //    lblPrueba.Text = "Error al timbrar el comprobante: " + smensajefinal;
                //    lblPrueba.Visible = true;
                //}
                //else
                //{
                //    lblPrueba.Text = "Comprobante timbrado con éxito";
                //    lblPrueba.Visible = true;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            btnRequest.Enabled = true;
            btnResponse.Enabled = true;
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void btnResponse_Click(object sender, EventArgs e)
        {
            Response vResonse = new Response();
            vResonse.Show();
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            Request vRequest = new Request();
            vRequest.Show();
        }
    }
}

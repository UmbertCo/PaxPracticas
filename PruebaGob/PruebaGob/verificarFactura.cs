using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PruebaGob.ServiceReference1;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Microsoft.Win32;
using System.Management;
using System.ServiceProcess;
using System.Net;
using System.Xml;

namespace PruebaGob
{
    public partial class verificarFactura : Form
    {
        ServiceSoapClient ws = new ServiceSoapClient();
        public verificarFactura()
        {
            InitializeComponent();
        }

        private void btnverificarFactura_Click(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
            try
            {
                txtResultado.Text = ws.verificarFactura("", int.Parse(txtPeriodo.Text), int.Parse(txtNoOperacion.Text), double.Parse(txtImporteRecibo.Text));
                txtResultado.Enabled = true;
            }
            catch (InvalidCastException er)
            {
                txtResultado.Text="Error: "+ er;
            }
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void verificarFactura_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Visible = false;
            Principal ventana_principal = new Principal();
            ventana_principal.Visible = true;
        }

        

    }
}

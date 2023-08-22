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
    public partial class responseFactura : Form
    {
        ServiceSoapClient ws = new ServiceSoapClient();
        public responseFactura()
        {
            InitializeComponent();
            cbEstatus.SelectedIndex = 0;
        }

        private void btnresponseFactura_Click(object sender, EventArgs e)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(txtXML.Text);
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

            //obtenemos el estatus seleccionado por el usuario
            String estatus = "P";
            if (cbEstatus.Text == "Procesado")
            {
                estatus = "P";
            }
            else
            {
                estatus = "C";
            }

            try
            {
                txtResultado.Text = ws.responseFactura("", int.Parse(txtPeriodo.Text), int.Parse(txtNoOperacion.Text), estatus, "", xml.OuterXml);
                txtResultado.Enabled = true;
                //txtResultado.Text = xml.OuterXml;
            }
            catch (InvalidCastException er)
            {
                txtResultado.Text = "IOException source: " + er;
            }
        }

        private void btnSeleccionarXML_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "XML |*.xml";
            openFileDialog1.Title = "Selecciona un archivo XML";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtXML.Text = openFileDialog1.FileName;
                btnresponseFactura.Enabled = true;
            }
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void responseFactura_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Visible = false;
            Principal ventana_principal = new Principal();
            ventana_principal.Visible = true;
        }
    }
}

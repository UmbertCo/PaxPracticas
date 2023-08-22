using SolucionPruebas.Presentacion.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmManifiesto : Form
    {
        public frmManifiesto()
        {
            InitializeComponent();
        }

        private void btnEnviarCartaManifiesto_Click(object sender, EventArgs e)
        {
            try
            {
                //Servicios.wsRecepcionCartaManifiesto.wcfRecepcionManifiestoSoapClient wsCartaManifiesto;
                //Servicios.wsRecepcionCartaManifiesto.ArrayOfString asDatos = new Servicios.wsRecepcionCartaManifiesto.ArrayOfString();

                //Servicios.wcfRecepcionManifiesto.wcfRecepcionManifiesto wcfRecepcionManifiesto = new Servicios.wcfRecepcionManifiesto.wcfRecepcionManifiesto();
                

                //Servicios.wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoapClient wsCartaManifiesto;
                //Servicios.wsRecepcionCartaManifiestoTest.ArrayOfString asDatos = new Servicios.wsRecepcionCartaManifiestoTest.ArrayOfString();

                Servicios.wsRecepcionCartaManifiestoProductivo.wcfRecepcionManifiestoSoapClient wsCartaManifiesto;
                Servicios.wsRecepcionCartaManifiestoProductivo.ArrayOfString asDatos = new Servicios.wsRecepcionCartaManifiestoProductivo.ArrayOfString();

                byte[] abPfx = File.ReadAllBytes(@"C:\Users\Ismael.Hidalgo\Desktop\PAX\pfx_1.pfx");
                string sPfx64 = System.Convert.ToBase64String(abPfx);
                abPfx = System.Text.Encoding.UTF8.GetBytes(sPfx64);

                string abPassword = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("visa1987"));

                asDatos.Add("CFA110411FW5");
                asDatos.Add("Prueba del Prueba");
                asDatos.Add("Domicilio Fiscal de Prueba");
                asDatos.Add("Representante Legal de Prueba");
                asDatos.Add("Autorizado de Prueba");

                //wsCartaManifiesto = ProxyLocator.ObtenerServicioRegistroCartaManifiestoLocal();
                //txtResultado.Text = wsCartaManifiesto.fnEnviarManifiestoDatos(asDatos, abPfx, abPassword);

                wsCartaManifiesto = ProxyLocator.ObtenerServicioRegistroCartaManifiestoProductivo();
                txtResultado.Text = wsCartaManifiesto.fnEnviarManifiestoDatos(asDatos, abPfx, abPassword);
            }
            catch (Exception ex)
            { 
            
            }
        }

        private void btnEnviarXML_Click(object sender, EventArgs e)
        {
            //Servicios.wsRecepcionCartaManifiesto.wcfRecepcionASMXSoapClient wsCartaManifiesto;

            //Servicios.wsRecepcionCartaManifiestoTest.wcfRecepcionManifiestoSoapClient wsCartaManifiesto;

            Servicios.wsRecepcionCartaManifiestoProductivo.wcfRecepcionManifiestoSoapClient wsCartaManifiesto;
            Servicios.wsRecepcionCartaManifiestoProductivo.ArrayOfString asDatos = new Servicios.wsRecepcionCartaManifiestoProductivo.ArrayOfString();

            byte[] abPfx = File.ReadAllBytes(@"C:\Users\Ismael.Hidalgo\Desktop\PAX\pfx_1.pfx");
            string sCartaManifiesto = File.ReadAllText(@"C:\Users\Ismael.Hidalgo\Desktop\PAX\241a5fee-faf2-4867-b845-7bc0f64c9d32.xml");
            string sPfx64 = System.Convert.ToBase64String(abPfx);
            abPfx = System.Text.Encoding.UTF8.GetBytes(sPfx64);

            string abPassword = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("visa1987"));

            //wsCartaManifiesto = ProxyLocator.ObtenerServicioRegistroCartaManifiestoLocal();
            //txtResultado.Text = wsCartaManifiesto.fnEnviarManifiestoXML(sCartaManifiesto, abPfx, abPassword);

            wsCartaManifiesto = ProxyLocator.ObtenerServicioRegistroCartaManifiestoProductivo();
            txtResultado.Text = wsCartaManifiesto.fnEnviarManifiestoXML(sCartaManifiesto, abPfx, abPassword);
        }
    }
}

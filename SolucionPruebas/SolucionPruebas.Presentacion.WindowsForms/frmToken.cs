using SolucionPruebas.Presentacion.Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmToken : Form
    {
        Servicios.wsServiceRecepcionLocal.ServiceRecepcionSoapClient wsServiceLocal;

        public frmToken()
        {
            InitializeComponent();
        }

        private void frmToken_Load(object sender, EventArgs e)
        {
            
        }

        private void btnConsumirServicio_Click(object sender, EventArgs e)
        {
            try
            {
                wsServiceLocal = new Servicios.wsServiceRecepcionLocal.ServiceRecepcionSoapClient();
                txtResultado.Text = wsServiceLocal.fnServicioPrueba("May");
            }
            catch (Exception ex)
            { 
            
            }
        }

        private void btnConsumirServicioToken_Click(object sender, EventArgs e)
        {
            try
            {
                wsServiceLocal = new Servicios.wsServiceRecepcionLocal.ServiceRecepcionSoapClient();
                
                txtResultado.Text = wsServiceLocal.fnServicioPrueba("May");
            }
            catch (Exception ex)
            {

            }
        }
    }
}

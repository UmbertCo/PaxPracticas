using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PruebaGob
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

        private void btnverificarFactura_Click(object sender, EventArgs e)
        {
            verificarFactura ventana_verificar = new verificarFactura();
            ventana_verificar.Show();
            this.Visible = false;
        }

        private void btnresponseFactura_Click(object sender, EventArgs e)
        {
            responseFactura ventana_response = new responseFactura();
            ventana_response.Show();
            this.Visible = false;
        }

        private void Principal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Principal ventana_principal = new Principal();
            ventana_principal.Close();
        }
    }
}

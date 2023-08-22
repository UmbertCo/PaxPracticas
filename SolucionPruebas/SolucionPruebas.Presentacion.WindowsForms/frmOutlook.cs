//using PAXFacturacion.Office;
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
    public partial class frmOutlook : Form
    {
        public frmOutlook()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PAXFacturacion.Office.clsIntegracionOutlook outLook = new PAXFacturacion.Office.clsIntegracionOutlook();
            //dataGridView1.DataSource = outLook.fnObtenerCitas("ismael.hidalgo@paxfacturacion.com", DateTime.Today.AddDays(-60), DateTime.Now);           
        }
    }
}

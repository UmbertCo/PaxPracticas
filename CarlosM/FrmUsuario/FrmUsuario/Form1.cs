using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FrmUsuario
{
    public partial class Form1 : Form
    {
        public Form1()
        { 
            InitializeComponent();
        }

        private void btnInserta_Click(object sender, EventArgs e)
        {
            localhost.Service1 local = new localhost.Service1();
            
            if (Convert.ToInt32(cbSexo.SelectedValue) > 0)
                local.Inserta(txtNombre.Text, txtApPat.Text, txtApMat.Text, txtDir.Text, txtTel.Text);    
            else
                MessageBox.Show("Selecciona Sexo", "Elerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                      //5257257257 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarSexo();
        }

        private void btnElimina_Click(object sender, EventArgs e)
        {
        
                       
        }

        private void llenarGrid(DataSet ds)
        {
            localhost.Service1 local = new localhost.Service1();
            
        }
        public void CargarSexo()         // crea un combo y lo llena don un datatable, que se le asignaron x numero de registros.
        {
            DataTable tabla = new DataTable("ComboSexo");

            tabla.Columns.Add("id",typeof(Int32));
            tabla.Columns.Add("sexo", typeof(String));

            tabla.Rows.Add(0, "Ninguno");
            tabla.Rows.Add(1, "Masculino");
            tabla.Rows.Add(2, "Femenino");
            tabla.Rows.Add(3, "GAY");

            cbSexo.DataSource = tabla;
        }

        private void bntElimina_Click(object sender, EventArgs e)
        {
            try
            {
                localhost.Service1 local = new localhost.Service1();// nueva instancia del servicio web, se le pasan como parametro una string que
                string elimina = local.elimina(Convert.ToInt32(txtborrarID.Text));// hace referencia al id del registro, se convierte a int y en la variable string se retorna el mensaje de 
                MessageBox.Show(elimina);                                   //registro eliminado haciendo llamada a la funcion eliminar del servicio local.
            }
            catch (Exception)
            {

                throw;
            }       
        }
    }
}

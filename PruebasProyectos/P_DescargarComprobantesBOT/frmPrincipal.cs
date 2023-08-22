using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace P_DescargarComprobantesBOT
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnLlave_Click(object sender, EventArgs e)
        {
            fnAbrirDialogo();
        }

        private void btnCer_Click(object sender, EventArgs e)
        {
            fnAbrirDialogo();
        }

        public void fnAbrirDialogo()
        {

            OpenFileDialog ofdDialgo = new OpenFileDialog();

            ofdDialgo.Filter = "Archivos SAT (*.txt,*.key, *.cer)|*.txt;*.key;*.cer";

            ofdDialgo.Multiselect = true;

            if (ofdDialgo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (String sArchivo in ofdDialgo.FileNames) 
                {

                    switch (Path.GetExtension(sArchivo.ToUpper())) 
                    {
                        case ".KEY": txtLlave.Text = sArchivo; break;
                        case ".CER": txtCer.Text = sArchivo; break;
                        case ".TXT": txtPass.Text = File.ReadAllText(sArchivo,Encoding.UTF8); break;
                    
                    }

                }

            }
            else return;
        
        
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {

        }

      
    }
}

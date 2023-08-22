using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Canonicalizacion_Tests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cbTipo.SelectedIndex = 0;
            cbXPATH.SelectedIndex = 0;
        }

        private void btnCargarPFX_Click(object sender, EventArgs e)
        {
            oFD_PFX.ShowDialog();
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            //Carga de PFX
            byte[] PFX = File.ReadAllBytes(oFD_PFX.FileName);
            //Carga de archivo XML
            XmlDocument xdXML = new XmlDocument();
            xdXML.Load(oFD_XML.FileName);
            //String de archivo XML
            String XML = xdXML.OuterXml;
            //String de contraseña de PFX
            String password = txtPassword.Text;
            //STRING DE XPATH A UTILIZAR
            String XPATH = cbXPATH.SelectedItem.ToString();
            //TEXTO PARA ID (TIPOS DE ID: SelloPrestadorAutorizado y SelloReceptor
            String tipo = cbTipo.SelectedItem.ToString();

            Procesado P_C14N11 = new Procesado(PFX, XML, tipo, password, XPATH);

            XmlDocument XmlCanonico = P_C14N11.Procesar();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Xml (*.xml)|*.xml";
            saveFileDialog.ShowDialog();
            XmlCanonico.Save(saveFileDialog.FileName);
        }

        private void btnCargarXML_Click(object sender, EventArgs e)
        {
            oFD_XML.ShowDialog();
        }

    }
}

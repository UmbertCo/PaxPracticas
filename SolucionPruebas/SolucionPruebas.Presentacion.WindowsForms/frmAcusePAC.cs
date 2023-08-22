using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmAcusePAC : Form
    {
        public frmAcusePAC()
        {
            InitializeComponent();
        }
        private void btnCadenaBase64_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdArchivo = new OpenFileDialog();
            string sCadenaOriginal = string.Empty;
            string xml = string.Empty;
            XmlDocument xmlDocGenera = new XmlDocument();
            try
            {
                ofdArchivo.ShowDialog();
                if (ofdArchivo.FileName.Equals(string.Empty))
                    return;

                Stream archivo = File.Open(ofdArchivo.FileName, FileMode.Open);
                StreamReader sr = new StreamReader(archivo);
                xml = sr.ReadToEnd();
                archivo.Close();

                xmlDocGenera.LoadXml(xml);

                sCadenaOriginal = fnConstruirCadenaTimbrado(xmlDocGenera.CreateNavigator());

                byte[] retXML = System.Text.Encoding.UTF8.GetBytes(sCadenaOriginal);

                //byte[] retXML = System.Text.Encoding.UTF8.GetBytes(psComprobante);
                string HASH = Convert.ToBase64String(retXML);

                txtResultado.Text = HASH;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnBase64Cadena_Click(object sender, EventArgs e)
        {
            string sCadenaOriginal = string.Empty;
            string xml = string.Empty;
            try
            {
                byte[] data = Convert.FromBase64String(txtCadena.Text);
                string decodedString = Encoding.UTF8.GetString(data);

                txtResultado.Text = decodedString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnCadenaHash_Click(object sender, EventArgs e)
        {
            string sResultado = string.Empty;
            try
            {
                string HASH = fnGetHASH(txtCadena.Text);
                sResultado = HASH;

                txtResultado.Text = sResultado;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el hash del emisor: " + ex.Message);
            }
        }

        /// <summary>
        /// Función que contruye la cadena original
        /// </summary>
        /// <param name="xml">Documento</param>
        /// <param name="psNombreArchivoXSLT">Nombre del archivo de tranformación</param>
        /// <returns></returns>
        private string fnConstruirCadenaTimbrado(IXPathNavigable xml)
        {
            string sCadenaOriginal = string.Empty;
            MemoryStream ms;
            StreamReader srDll;
            XsltArgumentList args;
            XslCompiledTransform xslt;
            try
            {
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOri.V32));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                sCadenaOriginal = srDll.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception(DateTime.Now + " " + "Error al generar la cadena original." + " " + ex.Message);

            }
            return sCadenaOriginal;
        }

        public static string fnGetHASH(string psCadenaOriginal)
        {
            byte[] hashValue;
            byte[] message = Encoding.UTF8.GetBytes(psCadenaOriginal);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }
}

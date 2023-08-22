using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmComprobantesActualizados : Form
    {
        public frmComprobantesActualizados()
        {
            InitializeComponent();
        }

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            try 
            {
                txtArchivo.Text = string.Empty;
                ofdArchivo.ShowDialog();
                

                if (ofdArchivo.FileName.Length.Equals(0))
                    return;

                txtArchivo.Text = ofdArchivo.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnRevisar_Click(object sender, EventArgs e)
        {
            DataSet dsDocumento = new DataSet();
            try
            {
                dsDocumento.ReadXml(txtArchivo.Text);
                dgvEntradas.DataSource = dsDocumento.Tables[0];
                dgvDocumento.DataSource = dsDocumento.Tables[2];
                //dgvDocumento.da
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvDocumento_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            XmlDocument xDocumento = new XmlDocument();
            XmlDocument xDocumentoSalida = new XmlDocument();
            try
            {
                xDocumento.Load(txtArchivo.Text);
                XPathNavigator xpnNavegadorActual = xDocumento.CreateNavigator();

                xDocumentoSalida.LoadXml("<LogOut></LogOut>");

                XPathNodeIterator xpnIterador = xpnNavegadorActual.Select("/LogOut/Entrada");
                DataSet dsResultado = new DataSet();
                DataTable dtResultado = new DataTable();
                dtResultado.Columns.Add("xml");

                foreach (XPathNavigator xpnEntrada in xpnIterador)
                {
                    string _sFechaCreacion = xpnEntrada.SelectSingleNode("@fechaCreacion").Value.ToString();

                    if (Convert.ToDateTime(_sFechaCreacion) >= Convert.ToDateTime(txtFechaCreacionInicio.Text) && (Convert.ToDateTime(_sFechaCreacion) <= Convert.ToDateTime(txtFechaCreacionFinal.Text)))
                    {
                        xDocumentoSalida.CreateNavigator().SelectSingleNode("/LogOut").AppendChild(xpnEntrada.OuterXml);
                    }
                }

                XmlNodeReader xrDocumento = new XmlNodeReader(xDocumentoSalida);
                dsResultado.ReadXml(xrDocumento);

                dgvEntradas.DataSource = dsResultado.Tables[0];
                dgvDocumento.DataSource = dsResultado.Tables[2];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvEntradas_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
    }
}

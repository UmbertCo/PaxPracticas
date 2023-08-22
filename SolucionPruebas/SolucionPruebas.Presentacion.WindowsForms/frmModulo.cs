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
    public partial class frmModulo : Form
    {
        public frmModulo()
        {
            InitializeComponent();
        }

        private void tsmiEncriptacionEncriptaciónTexto_Click(object sender, EventArgs e)
        {
            frmEncriptacion fEncriptacion = new frmEncriptacion();
            fEncriptacion.MdiParent = this;
            fEncriptacion.Show();
        }
        private void tsmiOpenSSLSelloOpenSSL_Click(object sender, EventArgs e)
        {
            frmOpenSSL fOpenSSL = new frmOpenSSL();
            fOpenSSL.MdiParent = this;
            fOpenSSL.Show();
        }
        private void tsmiValidacionEsquema32_Click(object sender, EventArgs e)
        {
            frmValidarXML fValidarXML = new frmValidarXML();
            fValidarXML.MdiParent = this;
            fValidarXML.Show();
        }
        private void tsmiZipGeneraciónZip_Click(object sender, EventArgs e)
        {
            frmZip fZip = new frmZip();
            fZip.MdiParent = this;
            fZip.Show();
        }
        private void tsmiServiciosJson_Click(object sender, EventArgs e)
        {
            frmConsumirServicios fConsumirServicios = new frmConsumirServicios();
            fConsumirServicios.MdiParent = this;
            fConsumirServicios.Show();
        }
        private void tsmiVentasOutlook_Click(object sender, EventArgs e)
        {
            frmOutlook fOutlook = new frmOutlook();
            fOutlook.MdiParent = this;
            fOutlook.Show();
        }
        private void tsmiGeneracionSellos_Click(object sender, EventArgs e)
        {
            frmGenerarSello fGenerarSello = new frmGenerarSello();
            fGenerarSello.MdiParent = this;
            fGenerarSello.Show();

        }
        private void tsmiGeneracionComprobante22A32_Click(object sender, EventArgs e)
        {
            frmConvertirComprobante22 fConvertirComprobante22 = new frmConvertirComprobante22();
            fConvertirComprobante22.MdiParent = this;
            fConvertirComprobante22.Show();
        }
        private void tsmiOpenXmlExcel_Click(object sender, EventArgs e)
        {
            frmExcel fExcel = new frmExcel();
            fExcel.MdiParent = this;
            fExcel.Show();
        }
        private void tsmiGeneracionLayout_Click(object sender, EventArgs e)
        {
            frmGenerarLayouts fGenerarLayouts = new frmGenerarLayouts();
            fGenerarLayouts.MdiParent = this;
            fGenerarLayouts.Show();
        }
        private void tsmiPermutacionesParametros_Click(object sender, EventArgs e)
        {
            frmPermutaciones fPermutaciones = new frmPermutaciones();
            fPermutaciones.MdiParent = this;
            fPermutaciones.Show();
        }
        private void tsmiTimbradoEnviar_Click(object sender, EventArgs e)
        {
            frmTimbrado fTimbrado = new frmTimbrado();
            fTimbrado.MdiParent = this;
            fTimbrado.Show();
        }
        private void esquema32LayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmValidarLayoutsNomina fValidarLayoutsNomina = new frmValidarLayoutsNomina();
            fValidarLayoutsNomina.MdiParent = this;
            fValidarLayoutsNomina.Show();
        }
        private void tmiValidacionValidarLayout_Click(object sender, EventArgs e)
        {
            frmValidarLayout fValidarLayouts = new frmValidarLayout();
            fValidarLayouts.MdiParent = this;
            fValidarLayouts.Show();
        }
        private void tsmiActualizacionComprobantes_Click(object sender, EventArgs e)
        {
            frmComprobantesActualizados fComprobantesActualizados = new frmComprobantesActualizados();
            fComprobantesActualizados.MdiParent = this;
            fComprobantesActualizados.Show();
        }
        private void tsmiTCPNET_Click(object sender, EventArgs e)
        {
            frmLCO fLCO = new frmLCO();
            fLCO.MdiParent = this;
            fLCO.Show();
        }
        private void tsmiAcusePAC_Click(object sender, EventArgs e)
        {
            frmAcusePAC fAcusePAC = new frmAcusePAC();
            fAcusePAC.MdiParent = this;
            fAcusePAC.Show();
        }
        private void tsmiCargarLCO_Click(object sender, EventArgs e)
        {
            frmCargarLCO fCargarLCO = new frmCargarLCO();
            fCargarLCO.MdiParent = this;
            fCargarLCO.Show();
        }
        private void tsmiRegistro_Click(object sender, EventArgs e)
        {
            frmRegistroUsuario fRegistroUsuarios = new frmRegistroUsuario();
            fRegistroUsuarios.MdiParent = this;
            fRegistroUsuarios.Show();
        }
    }
}

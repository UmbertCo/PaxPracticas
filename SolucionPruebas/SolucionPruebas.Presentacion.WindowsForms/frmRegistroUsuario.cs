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
    public partial class frmRegistroUsuario : Form
    {
        Servicios.wsRegistroUsuariosLocal.wcfRegistroSoapClient wsRegistroUsuariosLocal;
        Servicios.wsRegistroUsuariosSVCLocal.IwcfRegistroSVCClient wsRegistroUsuariosSVCLocal;
        Servicios.wsRegistroUsuariosTest.wcfRegistroSoapClient wsRegistroUsuariosTest;
        Servicios.wsRegistroUsuariosTestN.wcfRegistroSoapClient wsRegistroUsuariosTestNuevo;
        Servicios.wsRegistroUsuariosProduccion.IwcfRegistroSVCClient wsRegistroUsuariosProduccionSVC;
        Servicios.wsRegistroUsuariosProduccionasmx.wcfRegistroSoapClient wsRegistroUsuariosProduccion;
        Servicios.wsRegistroUsuariosLocal.ArrayOfString asDatos;
        Servicios.wsRegistroUsuariosTest.ArrayOfString asDatosTest;
        Servicios.wsRegistroUsuariosTestN.ArrayOfString asDatosTestNuevo;
        Servicios.wsRegistroUsuariosProduccionasmx.ArrayOfString asDatosProduccion;

        public frmRegistroUsuario()
        {
            InitializeComponent();
        }
        private void frmRegistroUsuario_Load(object sender, EventArgs e)
        {
            cbUsuarioDistribuidor.Checked = true;
        }
        private void cbUsuarioHijo_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUsuarioHijo.Checked)
            {
                txtRFC.Enabled = false;
                txtRazonSocial.Enabled = false;
                txtSucursalMatriz.Enabled = false;
                txtReferencia.Enabled = false;
                txtRegimenFiscal.Enabled = false;
                txtClaveDistribuidor.Enabled = false;
                txtCalle.Enabled = false;
                txtLocalidad.Enabled = false;
                txtMunicipio.Enabled = false;
                txtEstado.Enabled = false;
                txtColonia.Enabled = false;
                txtNumeroInterior.Enabled = false;
                txtNumeroExterior.Enabled = false;
                txtCodigoPostal.Enabled = false;
                txtEmailUsuarioPrincipal.Enabled = false;

                btnCargarCertificado.Enabled = false;
                btnCargarLlavePrivada.Enabled = false;
                txtPasswordLlavePrivada.Enabled = false;

                txtUsuarioHijo.Enabled = true;
                txtContraseniaUsuarioHijo.Enabled = true;
                txtCorreoUsuarioHijo.Enabled = true;
            }
        }
        private void cbUsuarioDistribuidor_CheckedChanged(object sender, EventArgs e)
        {
            if (cbUsuarioDistribuidor.Checked)
            {
                txtUsuarioHijo.Enabled = false;
                txtContraseniaUsuarioHijo.Enabled = false;
                txtCorreoUsuarioHijo.Enabled = false;

                txtRFC.Enabled = true;
                txtRazonSocial.Enabled = true;
                txtSucursalMatriz.Enabled = true;
                txtReferencia.Enabled = true;
                txtRegimenFiscal.Enabled = true;
                txtClaveDistribuidor.Enabled = true;
                txtCalle.Enabled = true;
                txtLocalidad.Enabled = true;
                txtMunicipio.Enabled = true;
                txtEstado.Enabled = true;
                txtColonia.Enabled = true;
                txtNumeroInterior.Enabled = true;
                txtNumeroExterior.Enabled = true;
                txtCodigoPostal.Enabled = true;
                txtEmailUsuarioPrincipal.Enabled = true;

                btnCargarCertificado.Enabled = true;
                btnCargarLlavePrivada.Enabled = true;
                txtPasswordLlavePrivada.Enabled = true;
            }
        }  
        private void rbLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLocal.Checked)
            {
                rbDesarrollo.Checked = false;
                rbTest.Checked = false;
                rbProductivo.Checked = false;
            }
        }
        private void rbDesarrollo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDesarrollo.Checked)
            {
                rbLocal.Checked = false;
                rbTest.Checked = false;
                rbProductivo.Checked = false;
            }
        }
        private void rbTest_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTest.Checked)
            {
                rbLocal.Checked = false;
                rbDesarrollo.Checked = false;
                rbProductivo.Checked = false;
            }
        }
        private void rbProductivo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbProductivo.Checked)
            {
                rbLocal.Checked = false;
                rbDesarrollo.Checked = false;
                rbTest.Checked = false;
            }
        }        
        private void btnCargarCertificado_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdCertificado = new OpenFileDialog();
            fdCertificado.ShowDialog();

            txtCertificado.Text = string.Empty;

            if (!string.IsNullOrEmpty(fdCertificado.FileName))
                txtCertificado.Text = fdCertificado.FileName;

        }
        private void btnCargarLlavePrivada_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdLlavePrivada = new OpenFileDialog();
            fdLlavePrivada.ShowDialog();

            txtLlavePrivada.Text = string.Empty;

            if (!string.IsNullOrEmpty(fdLlavePrivada.FileName))
                txtLlavePrivada.Text = fdLlavePrivada.FileName;
        }
        private void btnRegistrarUsuario_Click(object sender, EventArgs e)
        {
            byte[] abCertificado = null;
            byte[] abLlavePrivada = null;
            List<string> lsDatos = new List<string>();
            string sUsuarioNuevo = string.Empty;
            string sContraseniaUsuarioNuevo = string.Empty;
            string sNombreCertificado = string.Empty;
            string sContraseniaLlavePrivada = string.Empty;
            string sNombreLlavePrivada = string.Empty;
            string sPasswordLlavePrivada = string.Empty;
            string sEmailUsuario = string.Empty;
            string sUsuarioHijo = string.Empty;
            string sContraseniaUsuarioHijo = string.Empty;
            string sEmailUsuarioHijo = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(txtUsuarioNuevo.Text))
                    throw new Exception("Campo Usuario nuevo requerido");

                if (string.IsNullOrEmpty(txtContraseniaUsuarioNuevo.Text))
                    throw new Exception("Campo Contraseña del nuevo usuario requerido");
                
                if (!string.IsNullOrEmpty(txtUsuarioHijo.Text))
                    sUsuarioHijo = txtUsuarioHijo.Text;

                if (!string.IsNullOrEmpty(txtContraseniaUsuarioHijo.Text))
                    sContraseniaUsuarioHijo = txtContraseniaUsuarioHijo.Text;

                if (!string.IsNullOrEmpty(txtEmailUsuarioPrincipal.Text))
                    sEmailUsuario = txtEmailUsuarioPrincipal.Text;
                

                sUsuarioNuevo = txtUsuarioNuevo.Text;
                sContraseniaUsuarioNuevo = txtContraseniaUsuarioNuevo.Text;

                if (!string.IsNullOrEmpty(txtCertificado.Text))
                {
                    sNombreCertificado = System.IO.Path.GetFileNameWithoutExtension(txtCertificado.Text);
                    abCertificado = System.IO.File.ReadAllBytes(txtCertificado.Text);
                }

                if (!string.IsNullOrEmpty(txtLlavePrivada.Text))
                {
                    sNombreLlavePrivada = System.IO.Path.GetFileNameWithoutExtension(txtLlavePrivada.Text);
                    abLlavePrivada = System.IO.File.ReadAllBytes(txtLlavePrivada.Text);
                }

                if (!string.IsNullOrEmpty(txtPasswordLlavePrivada.Text))
                    sPasswordLlavePrivada = txtPasswordLlavePrivada.Text;

                if (!string.IsNullOrEmpty(txtCorreoUsuarioHijo.Text))
                    sEmailUsuarioHijo = txtCorreoUsuarioHijo.Text;

                //if (!string.IsNullOrEmpty(txtRFC.Text))
                    lsDatos.Add(txtRFC.Text);
                //if (!string.IsNullOrEmpty(txtRazonSocial.Text))
                    lsDatos.Add(txtRazonSocial.Text);
                //if (!string.IsNullOrEmpty(txtSucursalMatriz.Text))
                    lsDatos.Add(txtSucursalMatriz.Text);
                //if (!string.IsNullOrEmpty(txtEstado.Text))
                    lsDatos.Add(txtEstado.Text);
                //if (!string.IsNullOrEmpty(txtMunicipio.Text))
                    lsDatos.Add(txtMunicipio.Text);
                //if (!string.IsNullOrEmpty(txtLocalidad.Text))
                    lsDatos.Add(txtLocalidad.Text);
                //if (!string.IsNullOrEmpty(txtCalle.Text))
                    lsDatos.Add(txtCalle.Text);
                //if (!string.IsNullOrEmpty(txtClaveDistribuidor.Text))
                    lsDatos.Add(txtClaveDistribuidor.Text);
                //if (!string.IsNullOrEmpty(txtRegimenFiscal.Text))
                    lsDatos.Add(txtRegimenFiscal.Text);
                //if (!string.IsNullOrEmpty(txtNumeroExterior.Text))
                    lsDatos.Add(txtNumeroExterior.Text);
                //if (!string.IsNullOrEmpty(txtNumeroInterior.Text))
                    lsDatos.Add(txtNumeroInterior.Text);
                //if (!string.IsNullOrEmpty(txtColonia.Text))
                    lsDatos.Add(txtColonia.Text);
                //if (!string.IsNullOrEmpty(txtReferencia.Text))
                    lsDatos.Add(txtReferencia.Text);
                //if (!string.IsNullOrEmpty(txtCodigoPostal.Text))
                    lsDatos.Add(txtCodigoPostal.Text);
                //if (!string.IsNullOrEmpty(txtEmailUsuarioPrincipal.Text))
                    lsDatos.Add(txtEmailUsuarioPrincipal.Text);

                if (rbLocal.Checked)
                {
                    asDatos = new Servicios.wsRegistroUsuariosLocal.ArrayOfString();
                    asDatos.AddRange(lsDatos);

                    fnRegistrarUsuarioLocal(sUsuarioNuevo, sContraseniaUsuarioNuevo, asDatos, sNombreCertificado, abCertificado, sPasswordLlavePrivada,
                        sNombreLlavePrivada, abLlavePrivada, sUsuarioHijo, sContraseniaUsuarioHijo, sEmailUsuarioHijo);

                    //fnRegistrarUsuarioSVCLocal(sUsuarioNuevo, sContraseniaUsuarioNuevo, lsDatos, sNombreCertificado, abCertificado, sPasswordLlavePrivada,
                    //    sNombreLlavePrivada, abLlavePrivada, sUsuarioHijo, sContraseniaUsuarioHijo, sEmailUsuarioHijo);
                }

                if (rbDesarrollo.Checked)
                {
                    //asDatos = new Servicios.wsRegistroUsuariosLocal.ArrayOfString();
                    //asDatos.AddRange(lsDatos);

                    fnRegistrarUsuarioLocal(sUsuarioNuevo, sContraseniaUsuarioNuevo, asDatos, sNombreCertificado, abCertificado, sPasswordLlavePrivada,
                        sNombreLlavePrivada, abLlavePrivada, sUsuarioHijo, sContraseniaUsuarioHijo, sEmailUsuarioHijo);
                }

                if (rbTest.Checked)
                {
                    asDatosTest = new Servicios.wsRegistroUsuariosTest.ArrayOfString();
                    asDatosTest.AddRange(lsDatos);

                    fnRegistrarUsuarioTest(sUsuarioNuevo, sContraseniaUsuarioNuevo, asDatosTest, sNombreCertificado, abCertificado, sPasswordLlavePrivada,
                        sNombreLlavePrivada, abLlavePrivada, sUsuarioHijo, sContraseniaUsuarioHijo, sEmailUsuarioHijo);

                    //asDatosTestNuevo = new Servicios.wsRegistroUsuariosTestN.ArrayOfString();
                    //asDatosTestNuevo.AddRange(lsDatos);

                    //fnRegistrarUsuarioTestNuevo(sUsuarioNuevo, sContraseniaUsuarioNuevo, asDatosTestNuevo, sNombreCertificado, abCertificado, sPasswordLlavePrivada,
                    //    sNombreLlavePrivada, abLlavePrivada, sUsuarioHijo, sContraseniaUsuarioHijo, sEmailUsuarioHijo);
                }

                if (rbProductivo.Checked)
                {
                    asDatosProduccion = new Servicios.wsRegistroUsuariosProduccionasmx.ArrayOfString();
                    asDatosProduccion.AddRange(lsDatos);

                    fnRegistrarUsuarioProductivo(sUsuarioNuevo, sContraseniaUsuarioNuevo, asDatosProduccion, sNombreCertificado, abCertificado, sPasswordLlavePrivada,
                        sNombreLlavePrivada, abLlavePrivada, sUsuarioHijo, sContraseniaUsuarioHijo, sEmailUsuarioHijo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consumir registrar el usuario: " + ex.Message);
            }
        }

        private void fnRegistrarUsuarioLocal(string psUsuario, string psContraseniaUsuario, Servicios.wsRegistroUsuariosLocal.ArrayOfString pasDatos,
            string psNombreCertificado, byte[] pabCertificado, string psPasswordLlavePrivada, string psNombreLlavePrivada, byte[] pabLlavePrivada,
            string psUsuarioHijo, string psContraseniaUsuarioHijo, string psEmailUsuarioHijo)
        {
            try
            {
                wsRegistroUsuariosLocal = new Servicios.wsRegistroUsuariosLocal.wcfRegistroSoapClient();
                txtResultado.Text = wsRegistroUsuariosLocal.fnRegistraUsuario(psUsuario, psContraseniaUsuario, pasDatos,
                    psNombreCertificado, pabCertificado, psPasswordLlavePrivada, psNombreLlavePrivada, pabLlavePrivada, psUsuarioHijo,
                    psContraseniaUsuarioHijo, psEmailUsuarioHijo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar un usuario en ambiente local: " + ex.Message);
            }
        }

        private void fnRegistrarUsuarioSVCLocal(string psUsuario, string psContraseniaUsuario, List<string> pasDatos,
           string psNombreCertificado, byte[] pabCertificado, string psPasswordLlavePrivada, string psNombreLlavePrivada, byte[] pabLlavePrivada,
           string psUsuarioHijo, string psContraseniaUsuarioHijo, string psEmailUsuarioHijo)
        {
            try
            {
                wsRegistroUsuariosSVCLocal = ProxyLocator.ObtenerServicioRegistroUsuariosSVCLocal();
                txtResultado.Text = wsRegistroUsuariosSVCLocal.fnRegistraUsuario(psUsuario, psContraseniaUsuario, pasDatos,
                    psNombreCertificado, pabCertificado, psPasswordLlavePrivada, psNombreLlavePrivada, pabLlavePrivada, psUsuarioHijo,
                    psContraseniaUsuarioHijo, psEmailUsuarioHijo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar un usuario en ambiente local: " + ex.Message);
            }
        }

        private void fnRegistrarUsuarioDesarrollo(string psUsuario, string psContraseniaUsuario, Servicios.wsRegistroUsuariosLocal.ArrayOfString pasDatos,
            string psNombreCertificado, byte[] pabCertificado, string psPasswordLlavePrivada, string psNombreLlavePrivada, byte[] pabLlavePrivada,
            string psUsuarioHijo, string psContraseniaUsuarioHijo, string psEmailUsuarioHijo)
        {
            try
            {
                wsRegistroUsuariosLocal = new Servicios.wsRegistroUsuariosLocal.wcfRegistroSoapClient();
                txtResultado.Text = wsRegistroUsuariosLocal.fnRegistraUsuario(psUsuario, psContraseniaUsuario, pasDatos,
                    psNombreCertificado, pabCertificado, psPasswordLlavePrivada, psNombreLlavePrivada, pabLlavePrivada, psUsuarioHijo,
                    psContraseniaUsuarioHijo, psEmailUsuarioHijo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar un usuario en ambiente local: " + ex.Message);
            }
        }

        private void fnRegistrarUsuarioTest(string psUsuario, string psContraseniaUsuario, Servicios.wsRegistroUsuariosTest.ArrayOfString pasDatos,
            string psNombreCertificado, byte[] pabCertificado, string psPasswordLlavePrivada, string psNombreLlavePrivada, byte[] pabLlavePrivada,
            string psUsuarioHijo, string psContraseniaUsuarioHijo, string psEmailUsuarioHijo)
        {
            try
            {
                wsRegistroUsuariosTest = new Servicios.wsRegistroUsuariosTest.wcfRegistroSoapClient();
                txtResultado.Text = wsRegistroUsuariosTest.fnRegistraUsuario(psUsuario, psContraseniaUsuario, pasDatos,
                    psNombreCertificado, pabCertificado, psPasswordLlavePrivada, psNombreLlavePrivada, pabLlavePrivada, psUsuarioHijo,
                    psContraseniaUsuarioHijo, psEmailUsuarioHijo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar un usuario en ambiente local: " + ex.Message);
            }
        }

        private void fnRegistrarUsuarioTestNuevo(string psUsuario, string psContraseniaUsuario, Servicios.wsRegistroUsuariosTestN.ArrayOfString pasDatos,
            string psNombreCertificado, byte[] pabCertificado, string psPasswordLlavePrivada, string psNombreLlavePrivada, byte[] pabLlavePrivada,
            string psUsuarioHijo, string psContraseniaUsuarioHijo, string psEmailUsuarioHijo)
        {
            try
            {
                wsRegistroUsuariosTestNuevo = new Servicios.wsRegistroUsuariosTestN.wcfRegistroSoapClient();
                txtResultado.Text = wsRegistroUsuariosTestNuevo.fnRegistraUsuario(psUsuario, psContraseniaUsuario, pasDatos,
                    psNombreCertificado, pabCertificado, psPasswordLlavePrivada, psNombreLlavePrivada, pabLlavePrivada, psUsuarioHijo,
                    psContraseniaUsuarioHijo, psEmailUsuarioHijo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar un usuario en ambiente local: " + ex.Message);
            }
        }

        private void fnRegistrarUsuarioProductivo(string psUsuario, string psContraseniaUsuario, Servicios.wsRegistroUsuariosProduccionasmx.ArrayOfString pasDatos,
            string psNombreCertificado, byte[] pabCertificado, string psPasswordLlavePrivada, string psNombreLlavePrivada, byte[] pabLlavePrivada,
            string psUsuarioHijo, string psContraseniaUsuarioHijo, string psEmailUsuarioHijo)
        {
            try
            {
                wsRegistroUsuariosProduccion = ProxyLocator.ObtenerServicioRegistroUsuariosProduccionAsmx();
                txtResultado.Text = wsRegistroUsuariosProduccion.fnRegistraUsuario(psUsuario, psContraseniaUsuario, pasDatos,
                    psNombreCertificado, pabCertificado, psPasswordLlavePrivada, psNombreLlavePrivada, pabLlavePrivada, psUsuarioHijo,
                    psContraseniaUsuarioHijo, psEmailUsuarioHijo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar un usuario en ambiente local: " + ex.Message);
            }
        }

        private void fnRegistrarUsuarioProductivoSVC(string psUsuario, string psContraseniaUsuario, List<string> pasDatos,
            string psNombreCertificado, byte[] pabCertificado, string psPasswordLlavePrivada, string psNombreLlavePrivada, byte[] pabLlavePrivada,
            string psUsuarioHijo, string psContraseniaUsuarioHijo, string psEmailUsuarioHijo)
        {
            try
            {
                wsRegistroUsuariosProduccionSVC = ProxyLocator.ObtenerServicioRegistroUsuariosProduccionSVC();
                txtResultado.Text = wsRegistroUsuariosProduccionSVC.fnRegistraUsuario(psUsuario, psContraseniaUsuario, pasDatos,
                    psNombreCertificado, pabCertificado, psPasswordLlavePrivada, psNombreLlavePrivada, pabLlavePrivada, psUsuarioHijo,
                    psContraseniaUsuarioHijo, psEmailUsuarioHijo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar un usuario en ambiente local: " + ex.Message);
            }
        }
    }
}

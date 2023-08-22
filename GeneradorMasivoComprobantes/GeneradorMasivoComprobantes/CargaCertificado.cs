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
using System.Xml.XPath;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

namespace GeneradorMasivoComprobantes
{
    public partial class CargaCertificado : Form
    {
        String srfcEmisor = String.Empty, sEmisor = String.Empty, sFecha = String.Empty, sserie = String.Empty;
        String scalleEmisor = String.Empty, snumero_exEmisor = String.Empty, snum_intEmisor = String.Empty, scoloniaEmisor = String.Empty;
        String slocalidadEmisor = String.Empty, smunicipioEmisor = String.Empty, scpEmisor = String.Empty, sregfEmisor = String.Empty;
        String spaisEmisor = String.Empty, sestadoEmisor = String.Empty;
        string resValidacion = string.Empty;
        string sRutaCer = string.Empty, sRutaKey = string.Empty, sPassCer = string.Empty;
            

        int id_usuario = 0, id_estructura = 0, tipo_comprobante = 0;
        char estatus;
        bool bGenerarArchivos, bTimbrarComprobantes, bGuardarZip, bImpuestosRetenidos, bNuevoEmisor;

        public CargaCertificado()
        {
            InitializeComponent();
        }

        private X509Certificate2 certificado;
        /// <summary>
        /// Retorna el certificado como un objeto de .NET
        /// </summary>
        public X509Certificate2 Certificado
        {
            get
            {
                return certificado;
            }
        }

        int nnumConceptos = 0, nnumComprobantes = 0;
        public CargaCertificado(int idUsuario, int idEstructura, int tipoComprobante, char cEstatus, String rfcEmisor, String Emisor, String calleEmisor, String numero_exEmisor, String num_intEmisor,
            String coloniaEmisor, String localidadEmisor, String municipioEmisor, String cpEmisor, String regfEmisor, String paisEmisor,
            String estadoEmisor, String Fecha, String serie, int numConceptos, int numComprobantes, bool generarArchivos, bool timbrarComprobantes, bool impuestosRetenidos, bool guardarZip, bool nuevoEmisor)
        {
            InitializeComponent();
            id_usuario = idUsuario;
            id_estructura = idEstructura;
            srfcEmisor = rfcEmisor.Trim();
            sEmisor = Emisor.Trim();
            sFecha = Fecha.Trim();
            sserie = serie.Trim();
            nnumConceptos = numConceptos;
            nnumComprobantes = numComprobantes;
            scalleEmisor = calleEmisor.Trim();
            snumero_exEmisor = numero_exEmisor.Trim();
            snum_intEmisor = num_intEmisor.Trim();
            scoloniaEmisor = coloniaEmisor.Trim();
            slocalidadEmisor = localidadEmisor.Trim();
            smunicipioEmisor = municipioEmisor.Trim();
            scpEmisor = cpEmisor.Trim();
            sregfEmisor = regfEmisor.Trim();
            spaisEmisor = paisEmisor.Trim();
            sestadoEmisor = estadoEmisor.Trim();
            tipo_comprobante = tipoComprobante;
            estatus = cEstatus;
            bGenerarArchivos = generarArchivos;
            bTimbrarComprobantes = timbrarComprobantes;
            bGuardarZip = guardarZip;
            bImpuestosRetenidos = impuestosRetenidos;
            bNuevoEmisor = nuevoEmisor;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            sRutaCer = txtCer.Text;
            sRutaKey = txtKey.Text;
            sPassCer = txtPass.Text;

            if (lblTipoValue.Text.Equals("FIEL"))
            {
                MessageBox.Show("Las Personas Morales NO pueden usar la FIEL");
            }
            else
            {
                clsValCertificado vValidadorCertificado;
                try
                {
                    byte[] Certificado = System.IO.File.ReadAllBytes(txtCer.Text);
                    byte[] CertificadoKey = System.IO.File.ReadAllBytes(txtKey.Text);
                    //realizamos las validaciones de SAT sobre el archivo
                    vValidadorCertificado = new clsValCertificado(Certificado);
                    vValidadorCertificado.LlavePrivada = new clsOperacionTimbradoSellado(CertificadoKey, txtPass.Text);
                    resValidacion = vValidadorCertificado.ValidarCertificado(srfcEmisor.TrimStart(' '), sEmisor);

                    srfcEmisor = vValidadorCertificado.sRFCCertificado;
                    sEmisor = vValidadorCertificado.sRazonSocialCertificado;
                    //Verificamos que el certificado del comprobante se de tipo CSD
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }


                //si los datos del certificado son diferentes a los seleccionados en el emisor buscamos en el XML de emisores
                if (bNuevoEmisor)
                {
                    XmlDocument xmlEmisores = new XmlDocument();

                    try
                    {
                        //cargamos el xml de receptores
                        xmlEmisores.Load(@"emisores.xml");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error al cargar catálogo de emisores.xml - " + ex);
                    }

                    XPathNavigator navEmisores = null;
                    XPathNodeIterator NodoIterEmisor;

                    //creamos el navegador para el xml de Emisores
                    navEmisores = xmlEmisores.CreateNavigator();

                    //guardamos todos los receptores
                    NodoIterEmisor = navEmisores.Select("/Emisores/emisor");

                    XPathNavigator navegadorEmisor = null;

                    bool bExiste = false;

                    string nombreEm = string.Empty;
                    string rfcEm = string.Empty;

                    //mientras encontremos emisores
                    //recorremos hasta encontrar si actualmente existe el RFC y la razón social en el XML de emisores
                    while (NodoIterEmisor.MoveNext())
                    {
                        navegadorEmisor = NodoIterEmisor.Current;
                        nombreEm = navegadorEmisor.SelectSingleNode("@nombre").ToString().Trim();
                        rfcEm = navegadorEmisor.SelectSingleNode("@rfc").ToString().Trim();

                        if (nombreEm == vValidadorCertificado.sRazonSocialCertificado && rfcEm == vValidadorCertificado.sRFCCertificado)
                        {
                            bExiste = true;
                            break;
                        }
                    }

                    //si no existe lo agregamos al XML emisores
                    if (!bExiste)
                    {
                        XmlAttribute xAttr;
                        XmlElement elemento;
                        XmlNode nodo = null;

                        nodo = xmlEmisores.SelectSingleNode("/Emisores");

                        elemento = xmlEmisores.CreateElement("emisor");

                        xAttr = xmlEmisores.CreateAttribute("rfc");
                        xAttr.Value = vValidadorCertificado.sRFCCertificado;
                        elemento.Attributes.Append(xAttr);

                        xAttr = xmlEmisores.CreateAttribute("nombre");
                        xAttr.Value = vValidadorCertificado.sRazonSocialCertificado;
                        elemento.Attributes.Append(xAttr);

                        nodo.AppendChild(elemento);
                        xmlEmisores.Save(@"emisores.xml");
                    }
                }


                backgroundWorker1.RunWorkerAsync();
                btnAceptar.Enabled = false;
            }
        }

        private void btnSeleccionarCer_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "cer |*.cer";
            openFileDialog1.Title = "Selecciona un archivo de Certificado de seguridad";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtCer.Text = openFileDialog1.FileName;
            }
        }

        private void btnSeleccionarKey_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a Cursor.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "key |*.key";
            openFileDialog1.Title = "Selecciona un archivo key";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtKey.Text = openFileDialog1.FileName;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 Principal = new Form1(id_usuario, id_estructura);
            Principal.Refresh();
            Principal.Visible = true;
        }

        private void txtCer_TextChanged(object sender, EventArgs e)
        {
            comprobarCampos();
        }

        private void comprobarCampos()
        {
            string srutaCer = txtCer.Text, srutaKey = txtKey.Text, spassCer = txtPass.Text;

            if (srutaCer != String.Empty)
            {
                lblProceso.Text = "Esperando archivo .key";
                btnAceptar.Enabled = false;
            }

            if (txtKey.Text != String.Empty)
            {
                lblProceso.Text = "Esperando archivo .cer";
                btnAceptar.Enabled = false;
            }

            if (srutaCer != String.Empty && srutaKey != String.Empty)
            {
                lblProceso.Text = "Esperando contraseña.";
                btnAceptar.Enabled = false;
            }

            if (srutaCer != String.Empty && srutaKey != String.Empty && spassCer != String.Empty)
            {
                lblProceso.Text = "Listo para generar comprobantes.";

                obtenerInfoComprobante();

                //aqui
                btnAceptar.Enabled = true;
            }
        }

        private void obtenerInfoComprobante()
        {
            //cargamos el certificado
            byte[] bCertificado = System.IO.File.ReadAllBytes(txtCer.Text);

            try
            {
                certificado = new X509Certificate2(bCertificado);
            }
            catch
            {
                try
                {
                    certificado = new X509Certificate2(fnDesencriptarCertificado(bCertificado));
                    bCertificado = fnDesencriptarCertificado(bCertificado);
                }
                catch (Exception ex)
                {
                    //throw new CryptographicException("El certificado esta bloqueado");
                }
            }
            
            //codificacion
            string codificacionCert = string.Empty;
            int tamanoCert = 0;
            
            tamanoCert = certificado.PublicKey.Key.KeySize;
            codificacionCert = certificado.SignatureAlgorithm.FriendlyName;

            string tipoCert = string.Empty;

            try
            {
                if (certificado.Extensions[1].Format(false).Contains("(c0)"))
                {
                    tipoCert = "CSD";
                }
                else
                {
                    if (certificado.Extensions[1].Format(false).Contains("(d8)"))
                    {
                        tipoCert = "FIEL";
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error al obtener tipo de certificado.");
            }
            
            //ponemos visible los labels
            lblInformacion.Visible = true;
            lblTipo.Visible = true;
            lblCodificacion.Visible = true;
            lblTamano.Visible = true;

            lblTipoValue.Text = tipoCert;
            lblCodificacionValue.Text = codificacionCert;
            lblTamanoValue.Text = Convert.ToString(tamanoCert);

            lblTipoValue.Visible = true;
            lblCodificacionValue.Visible = true;
            lblTamanoValue.Visible = true;

        }

        /// <summary>
        /// Devuelve el arreglo de bytes del certificado original
        /// </summary>
        /// <returns></returns>
        private byte[] fnDesencriptarCertificado(byte[] pbCertificadoEncriptado)
        {
            return Utilerias.Encriptacion.DES3.Desencriptar(pbCertificadoEncriptado);
        }

        private void txtKey_TextChanged(object sender, EventArgs e)
        {
            comprobarCampos();
        }

        public void ActualizarLabel(int nActual, int nTotal)
        {
            lblProceso.Text = "Generando comprobante " + nActual + " de " + nTotal + ".";
            this.Refresh();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int nFolio = 0;
            //Comprimidor
            Comprimidor comprimidor = new Comprimidor();

            if (bGuardarZip)
            {
                if (!Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaZips"]))
                    Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaZips"]);
            }

            try
            {
                //empezamos a crear el número de comprobantes que el usuario seleccionó
                for (int i = 0; i < nnumComprobantes; i++)
                {
                    if (tipo_comprobante == 10)
                    {
                        nFolio = (i + 1);
                        try
                        {
                            nomina.generaNomina(id_usuario, id_estructura, tipo_comprobante, estatus, srfcEmisor, sEmisor, scalleEmisor, snumero_exEmisor, snum_intEmisor, scoloniaEmisor, slocalidadEmisor, smunicipioEmisor, scpEmisor, sregfEmisor, spaisEmisor, sestadoEmisor, sFecha, sserie, nFolio, nnumConceptos, nnumComprobantes, sRutaCer, sRutaKey, sPassCer, bGenerarArchivos, bTimbrarComprobantes, bGuardarZip);
                            backgroundWorker1.ReportProgress(i);
                        }
                        catch (Exception ex)
                        {
                            throw new System.ArgumentException(ex.Message);
                        }
                    }
                    if (tipo_comprobante == 1 || tipo_comprobante == 6)
                    {
                        nFolio = (i + 1);
                        try
                        {
                            factura.generaFactura(id_usuario, id_estructura, tipo_comprobante, estatus, srfcEmisor, sEmisor, scalleEmisor, snumero_exEmisor, snum_intEmisor, scoloniaEmisor, slocalidadEmisor, smunicipioEmisor, scpEmisor, sregfEmisor, spaisEmisor, sestadoEmisor, sFecha, sserie, nFolio, nnumConceptos, nnumComprobantes, sRutaCer, sRutaKey, sPassCer, bGenerarArchivos, bTimbrarComprobantes, bImpuestosRetenidos, bGuardarZip);
                            backgroundWorker1.ReportProgress(i);
                        }
                        catch (Exception ex)
                        {
                            throw new System.ArgumentException(ex.Message);
                        }
                    }
                    if (tipo_comprobante == 11)
                    {
                        nFolio = (i + 1);
                        try
                        {
                            LayoutFactura.GenerarLayoutFactura(srfcEmisor, sEmisor, scalleEmisor, snumero_exEmisor, snum_intEmisor, scoloniaEmisor, slocalidadEmisor, smunicipioEmisor, scpEmisor, sregfEmisor, spaisEmisor, sestadoEmisor, sFecha, sserie, nFolio, nnumConceptos, nnumComprobantes, sRutaCer, sRutaKey, sPassCer, bImpuestosRetenidos, bGuardarZip);
                            backgroundWorker1.ReportProgress(i);
                        }
                        catch (Exception ex)
                        {
                            throw new System.ArgumentException(ex.Message);
                        }
                    }
                    if (tipo_comprobante == 12)
                    {
                        nFolio = (i + 1);
                        try
                        {
                            LayoutNomina.GenerarLayoutNomina(srfcEmisor, sEmisor, scalleEmisor, snumero_exEmisor, snum_intEmisor, scoloniaEmisor, slocalidadEmisor, smunicipioEmisor, scpEmisor, sregfEmisor, spaisEmisor, sestadoEmisor, sFecha, sserie, nFolio, nnumComprobantes, sRutaCer, sRutaKey, sPassCer, bGuardarZip);
                            backgroundWorker1.ReportProgress(i);
                        }
                        catch (Exception ex)
                        {
                            throw new System.ArgumentException(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (bGuardarZip && (tipo_comprobante != 11 || tipo_comprobante != 12))
                comprimidor.Comprimir(System.Configuration.ConfigurationManager.AppSettings["rutaZips"], "*.xml", "ZIPs", true);

            if (bGuardarZip && (tipo_comprobante == 11 || tipo_comprobante == 12))
                comprimidor.Comprimir(System.Configuration.ConfigurationManager.AppSettings["rutaZips"], "*.txt", "ZIPs", true);

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblProceso.Text = "Generando comprobante " + (e.ProgressPercentage + 1) + " de " + nnumComprobantes + ".";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblProceso.Text = nnumComprobantes + " de " + nnumComprobantes + " comprobantes generados.";
            btnAceptar.Enabled = true;
            btnCancelar.Text = "Salir.";
            btnAceptar.Text = "Repetir generación";
            lnkCarpeta.Visible = true;
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            comprobarCampos();
        }

        private void lnkCarpeta_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (tipo_comprobante == 10)
                System.Diagnostics.Process.Start(nomina.rutaCarpeta);
            if (tipo_comprobante == 1 || tipo_comprobante == 6)
                System.Diagnostics.Process.Start(factura.rutaCarpeta);
            if (tipo_comprobante == 11)
                System.Diagnostics.Process.Start(LayoutFactura.rutaCarpeta);
            if (tipo_comprobante == 12)
                System.Diagnostics.Process.Start(LayoutNomina.rutaCarpeta);
        }
    }
}
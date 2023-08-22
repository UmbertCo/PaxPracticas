using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Text.RegularExpressions;

namespace GeneradorMasivoComprobantes
{
    public partial class Form1 : Form
    {
        string sXMLgenerado = string.Empty, sRutaAnterior = string.Empty;
        List<string> liRfcReceptores = new List<string>();

        XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
        XmlDocument xDoc = new XmlDocument();

        //xpath
        XPathNavigator navEmisores = null;
        XPathNodeIterator NodoIterEmisor;
        XPathNavigator[] EmisorInfo;
        int id_usuario = 0, id_estructura = 0;
        bool bGuardarZip = false;
        bool bGenerarArchivos = false;
        bool bTimbrarComprobantes = true;
        bool bImpuestosRetenidos = false;
        bool bNuevoEmisor = false;

        public Form1(int idUsuario, int idEstructura)
        {
            InitializeComponent();
            id_usuario = idUsuario;
            id_estructura = idEstructura;
            cbTipoDoc.SelectedIndex = 0;
            dtFecha.CustomFormat = "yyyy-MM-ddTHH:mm:ss";
            nConceptos.Value = 2;
            lblIVA.Enabled = false;
            //nuevo
            XmlDocument xmlEmisores = new XmlDocument();

            #region Emisores
            try
            {
                //cargamos el xml de receptores
                xmlEmisores.Load(@"emisores.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar catálogo de emisores.xml - "+ex);
            }

            //creamos el navegador para el xml de Emisores
            navEmisores = xmlEmisores.CreateNavigator();

            //guardamos todos los receptores
            NodoIterEmisor = navEmisores.Select("/Emisores/emisor");

            XPathNavigator navegadorEmisor = null;

            int i = 0;

            //mientras encontremos emisores
            //recorremos para ver cuantos rfc estan correctos
            while(NodoIterEmisor.MoveNext())
            {
                navegadorEmisor = NodoIterEmisor.Current;
                string nombreEm = navegadorEmisor.SelectSingleNode("@nombre").ToString().Trim();
                string rfcEm = navegadorEmisor.SelectSingleNode("@rfc").ToString();

                if (rfcEm.Length >= 12 && rfcEm.Length <= 13)
                {
                    //cumple con el formato del RFC
                    if (Regex.IsMatch(rfcEm, "^[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A]?$")) //|| Regex.IsMatch(rfcEm, "[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                    {
                        //if (navegadorEmisor.OuterXml.Contains("regimen_fiscal")) temporal
                        //{
                            i++;
                        //}
                        //else
                        //{
                        //    MessageBox.Show("El emisor " + nombreEm + " es incorrecto. No cuenta con regimen fiscal. No se agregó a la lista de emisores.");
                        //}
                    }
                    else
                    {
                        MessageBox.Show("El RFC del emisor " + nombreEm + " es incorrecto (" + rfcEm + "). No se agregó a la lista de emisores.");
                    }
                }
            }

            //sabiendo cuantos rfc estan correctos podemos crear el arreglo de navegadores con el tamaño correcto
            EmisorInfo = new XPathNavigator[i];
            NodoIterEmisor = navEmisores.Select("/Emisores/emisor");

            i = 0;
            while (NodoIterEmisor.MoveNext())
            {
                navegadorEmisor = NodoIterEmisor.Current;
                string nombreEm = navegadorEmisor.SelectSingleNode("@nombre").ToString();
                string rfcEm = navegadorEmisor.SelectSingleNode("@rfc").ToString();
                if (rfcEm.Length >= 12 && rfcEm.Length <= 13)
                {
                    //cumple con el formato del RFC
                    if (Regex.IsMatch(rfcEm, "^[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A-Z]?$")) //|| Regex.IsMatch(rfcEm, "[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                    {
                        //if (navegadorEmisor.OuterXml.Contains("regimen_fiscal")) temporal
                        //{
                            cbEmisores.Items.Add(nombreEm + " - " + rfcEm);
                            EmisorInfo[i] = NodoIterEmisor.Current.Clone();
                            i++;
                        //}
                    }
                }
            }

            #endregion

            cbEmisores.SelectedIndex = 0;

            //llenamos el cb serie
            char letra = 'A';

            while (letra <= 'Z')
            {
                if(letra>'N'&&letra<'P')
                    cbSerie.Items.Add('Ñ');
                cbSerie.Items.Add(letra);
                letra++;
            }
            cbSerie.SelectedIndex = 0;
            cbEstatus.SelectedIndex = 0;

            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaFactura"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaFactura"]);
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaFacturaSinTimbre"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaFacturaSinTimbre"]);
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaHonorarios"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaHonorarios"]);
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaHonorariosSinTimbre"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaHonorariosSinTimbre"]);
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaNomina"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaNomina"]);
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaNominaSinTimbre"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaNominaSinTimbre"]);
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaZips"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaZips"]);
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaLayoutFactura"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaLayoutFactura"]);
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaLayoutNomina"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaLayoutNomina"]);
        }

        private void cbTipoDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cbTipoDoc.SelectedIndex == 0 || cbTipoDoc.SelectedIndex == 1 || cbTipoDoc.SelectedIndex == 3))
            {
                cboIVA.Enabled = false;
                cboISR.Enabled = false;
                chImpuestoRetenido.Checked = false;
                chImpuestoRetenido.Enabled = false;
            }
            else
            {
                chImpuestoRetenido.Enabled = true;
            }
            if (cbTipoDoc.SelectedIndex == 0 &&bGuardarZip==false)
            {
                nConceptos.Enabled = true;
                lblConceptos.Enabled = true;
                lblRutaXML.Text = System.Configuration.ConfigurationManager.AppSettings["rutaFactura"];
            }
            if (cbTipoDoc.SelectedIndex == 1 && bGuardarZip == false)
            {
                nConceptos.Enabled = true;
                lblConceptos.Enabled = true;
                lblRutaXML.Text = System.Configuration.ConfigurationManager.AppSettings["rutaHonorarios"];
            }
            if (cbTipoDoc.SelectedIndex == 2 && bGuardarZip == false)
            {
                nConceptos.Enabled = false;
                lblConceptos.Enabled = false;
                lblRutaXML.Text = System.Configuration.ConfigurationManager.AppSettings["rutaNomina"];
            }
            if (cbTipoDoc.SelectedIndex == 3 || cbTipoDoc.SelectedIndex == 4)
            {
                lbEstatus.Enabled = false;
                cbEstatus.Enabled = false;
                chTimbrar.Enabled = false;
                chTimbrar.Checked = false;
                chArchivos.Checked = false;
                chArchivos.Enabled = false;
                lbComprobantes.Text = "Layouts a generar:";
            }
            else
            {
                lbComprobantes.Text = "Comprobantes a generar:";
                lbEstatus.Enabled = true;
                cbEstatus.Enabled = true;
                chTimbrar.Enabled = true;
                chTimbrar.Checked = true;
                chArchivos.Checked = true;
                chArchivos.Enabled = true;
            }

            if (cbTipoDoc.SelectedIndex == 3 && bGuardarZip == false)
            {
                lblRutaXML.Text = System.Configuration.ConfigurationManager.AppSettings["rutaLayoutFactura"];
            }

            if (cbTipoDoc.SelectedIndex == 4 && bGuardarZip == false)
            {
                nConceptos.Enabled = false;
                lblConceptos.Enabled = false;
                lblRutaXML.Text = System.Configuration.ConfigurationManager.AppSettings["rutaLayoutNomina"];
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            string regimen_fiscalEmisor = string.Empty, localidadEmisor = string.Empty, numero_interiorEmisor = string.Empty, municipioEmisor = string.Empty, numero_exteriorEmisor = string.Empty, coloniaEmisor=string.Empty;
            string referenciaEmisor = string.Empty;

            string rfcEmisor = string.Empty;
            string nombreEmisor = string.Empty;

            try
            {
                //si es nuevo emisor, tomamos los datos de los text box
                if (chEditarEmisor.Checked == true)
                {
                    rfcEmisor = txtRFCEmisor.Text;
                    nombreEmisor = txtNombre.Text.Trim();
                    if (!string.IsNullOrEmpty(rfcEmisor))
                    {
                        if (rfcEmisor.Length >= 12 && rfcEmisor.Length <= 13)
                        {
                            //cumple con el formato del RFC
                            if (!Regex.IsMatch(rfcEmisor, "^[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A]?$"))
                            {
                                throw new System.ArgumentException("El RFC del emisor es incorrecto (" + rfcEmisor + ").");
                            }
                        }
                        else
                        {
                            throw new System.ArgumentException("El RFC del emisor es incorrecto (" + rfcEmisor + ").");
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException("El RFC del emisor no puede estar vacío.");
                    }

                    if (string.IsNullOrEmpty(nombreEmisor))
                    {
                        throw new System.ArgumentException("El nombre del emisor no puede estar vacío.");
                    }
                }
                else
                {
                    rfcEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("@rfc").ToString().Trim();
                    nombreEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("@nombre").ToString().Trim();
                }

                //cargamos los datos del emisor seleccionado
                if (EmisorInfo[cbEmisores.SelectedIndex].OuterXml.Contains("regimen_fiscal"))
                {
                    regimen_fiscalEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/@regimen_fiscal").ToString().Trim();
                }
                else
                {
                    regimen_fiscalEmisor = nombreEmisor;
                }

                //string calleEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/@calle").ToString();
                //if (EmisorInfo[cbEmisores.SelectedIndex].OuterXml.Contains("numero_exterior"))
                //    numero_exteriorEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/@numero_exterior").ToString();
                //if (EmisorInfo[cbEmisores.SelectedIndex].OuterXml.Contains("numero_interior"))
                //    numero_interiorEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/@numero_interior").ToString();
                //if (EmisorInfo[cbEmisores.SelectedIndex].OuterXml.Contains("colonia"))
                //    coloniaEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/@colonia").ToString();
                //if (EmisorInfo[cbEmisores.SelectedIndex].OuterXml.Contains("referencia"))
                //    referenciaEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/@referencia").ToString();
                //if (EmisorInfo[cbEmisores.SelectedIndex].OuterXml.Contains("localidad"))
                //    localidadEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/@localidad").ToString();
                //if (EmisorInfo[cbEmisores.SelectedIndex].OuterXml.Contains("municipio"))
                //    municipioEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/@municipio").ToString();
                //string codigo_postalEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/@codigo_postal").ToString();
                //string paisEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/pais/@pais").ToString();
                //string estadoEmisor = EmisorInfo[cbEmisores.SelectedIndex].SelectSingleNode("DomicilioFiscal/pais/estado/@estado").ToString();

                //DATOS FIJOS EMISOR
                string calleEmisor = "Heroico Colegio Militar";
                numero_exteriorEmisor = "4709";
                numero_interiorEmisor = "2";
                coloniaEmisor = "Nombre de Dios";
                referenciaEmisor = "Edificio PIT 2";
                localidadEmisor = "Chihuahua";
                municipioEmisor = "Chihuahua";
                string codigo_postalEmisor = "31210";
                string paisEmisor = "México";
                string estadoEmisor = "Chihuahua";

                int tipo_documento = 0;

                //Factura
                if (cbTipoDoc.SelectedIndex == 0)
                    tipo_documento = 1;

                //Honorarios
                if (cbTipoDoc.SelectedIndex == 1)
                    tipo_documento = 6;

                //Nomina       
                if (cbTipoDoc.SelectedIndex == 2)
                    tipo_documento = 10;

                //Layout factura
                if (cbTipoDoc.SelectedIndex == 3)
                    tipo_documento = 11;

                //Layout factura
                if (cbTipoDoc.SelectedIndex == 4)
                    tipo_documento = 12;

                CargaCertificado cargaCer = new CargaCertificado(id_usuario, id_estructura, tipo_documento, Convert.ToChar(cbEstatus.Text), rfcEmisor, nombreEmisor, calleEmisor, numero_exteriorEmisor, numero_interiorEmisor, coloniaEmisor, localidadEmisor, municipioEmisor, codigo_postalEmisor, regimen_fiscalEmisor, paisEmisor, estadoEmisor, dtFecha.Text, cbSerie.Text, Convert.ToInt16(nConceptos.Value), Convert.ToInt16(nComprobantesGenerar.Value), bGenerarArchivos, bTimbrarComprobantes, bImpuestosRetenidos, bGuardarZip, bNuevoEmisor);
                cargaCer.Visible = true;
                this.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            usuario formUsuario = new usuario();
            formUsuario.Visible = true;
            this.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void cbArchivos_CheckedChanged(object sender, EventArgs e)
        {
            if (chArchivos.Checked == true)
            {
                bGenerarArchivos = true;
            }
            else
            {
                bGenerarArchivos = false;
            }
        }

        private void chTimbrar_CheckedChanged(object sender, EventArgs e)
        {
            if (chTimbrar.Checked == true)
            {
                bTimbrarComprobantes = true;
                if(chGuardarZip.Checked==false)
                chArchivos.Enabled = true;
            }
            else
            {
                bTimbrarComprobantes = false;
                chArchivos.Checked = true;
                bGenerarArchivos = true;
                chArchivos.Enabled = false;
                if (!(cbTipoDoc.SelectedIndex == 3 || cbTipoDoc.SelectedIndex == 4))
                    MessageBox.Show("Si deshabilita el timbrado de comprobantes los comprobantes no se insertaran en la BD.");
            }
        }

        private void chGuardarZip_CheckedChanged(object sender, EventArgs e)
        {
            if (chGuardarZip.Checked == true)
            {
                chArchivos.Enabled = false;
                if(!(cbTipoDoc.SelectedIndex==3||cbTipoDoc.SelectedIndex==4))
                    chArchivos.Checked = true;
                bGuardarZip = true;
                sRutaAnterior = lblRutaXML.Text;
                lblRutaXML.Text = System.Configuration.ConfigurationManager.AppSettings["rutaZips"];
            }
            else
            {
                if (chTimbrar.Checked == true)
                    chArchivos.Enabled = true;
                bGuardarZip = false;
                lblRutaXML.Text = sRutaAnterior;
            }

        }

        private void lnkCarpetas_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\"+AppDomain.CurrentDomain.FriendlyName;
            dir = dir.Substring(0, dir.Length-11)+".exe.config";
            
            System.Diagnostics.Process.Start(dir);
        }

        private void chImpuestoRetenido_CheckedChanged(object sender, EventArgs e)
        {
            if (chImpuestoRetenido.Checked)
            {
                cboISR.Enabled = true;
                cboISR.SelectedIndex = 1;
            }
            else
            {
                cboISR.Enabled = false;
                cboIVA.Enabled = false;
                bImpuestosRetenidos = false;
            }
        }

        private void cboISR_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboISR.SelectedIndex == 1)
                bImpuestosRetenidos = true;
            else
                bImpuestosRetenidos = false;
        }

        private void chEditarEmisor_CheckedChanged(object sender, EventArgs e)
        {
            if (chEditarEmisor.Checked)
            {
                txtNombre.Enabled = true;
                txtRFCEmisor.Enabled = true;
                bNuevoEmisor = true;
                cbEmisores.Enabled = false;
            }
            else
            {
                txtNombre.Enabled = false;
                txtRFCEmisor.Enabled = false;
                bNuevoEmisor = false;
                cbEmisores.Enabled = true;
            }
        }

        private void cbEmisores_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] infoEmisor = cbEmisores.Text.Split('-');
            txtNombre.Text = infoEmisor[0].Trim();
            txtRFCEmisor.Text = infoEmisor[1].Trim();
        }
    }
}

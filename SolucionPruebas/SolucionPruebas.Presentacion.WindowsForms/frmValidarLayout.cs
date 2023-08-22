using OpenSSL_Lib;
using SolucionPruebas.Presentacion.WindowsForms.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmValidarLayout : Form
    {
        public frmValidarLayout()
        {
            InitializeComponent();
        }

        private string gsErrores = string.Empty;
        private string gsRutaCertificado = string.Empty;
        private string gsRutaGeneracion = string.Empty;
        OpenSSL_Lib.cSello cSello;
        byte[] gbLlavePrivada;

        private void btnSeleccionarArchivo_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileNames.Length.Equals(0))
                return;

            txtArchivo.Text = string.Empty;

            AgregarArchivos cAgregarAchivos = new AgregarArchivos(openFileDialog1.FileNames);
            Thread hilo = new Thread(new ThreadStart(cAgregarAchivos.fnSeleccionarArchivo));
            hilo.Start();
            hilo.Join();
            txtArchivo.Text = cAgregarAchivos.fnObtenerArchivos();
        }
        private void btnCargarLlavePrivada_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();

            if (openFileDialog2.FileName.Length.Equals(0))
                return;

            Stream streamkey = File.Open(openFileDialog2.FileName, FileMode.Open);
            StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
            using (BinaryReader br = new BinaryReader(streamkey))
            {
                gbLlavePrivada = br.ReadBytes(Convert.ToInt32(streamkey.Length));
            }

        }
        private void btnCargarCertificado_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();

            if (openFileDialog3.FileName.Length.Equals(0))
                return;

            gsRutaCertificado= openFileDialog3.FileName;
        }
        private void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                string[] asArchivos = txtArchivo.Text.Split(',');
                txtResultados.Text = string.Empty;

                gsRutaGeneracion = Settings1.Default.RutaPfx;
                //gsRutaCertificado = Settings1.Default.RutaCertificado;

                //gbLlavePrivada = File.ReadAllBytes(Settings1.Default.RutaLlave);

                for (int i = 0; i < asArchivos.Count(); i++)
                {
                    string sText = string.Empty;
                    string sXMLGenerado = string.Empty;
                    int nIndiceRfc = 0;

                    using (Stream stream = File.Open(asArchivos[i], FileMode.Open))
                    {
                        StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                        sText = sr.ReadToEnd();
                    }

                    nIndiceRfc = sText.IndexOf("re?rfc@");

                    if (nIndiceRfc.Equals(0))
                        continue;

                    if (!sText.Substring(nIndiceRfc + 7, 12).Equals("AAA010101AAA"))
                    {
                        sText = sText.Replace(sText.Substring(nIndiceRfc + 7, 12), "AAA010101AAA");
                    }


                    sXMLGenerado = fnGenerarComprobante(sText, asArchivos[i]);


                    XmlDocument document = new XmlDocument();
                    gsErrores = string.Empty;

                    // Declare local objects
                    XmlTextReader tr = null;
                    XmlSchemaCollection xsc = null;
                    XmlValidatingReader vr = null;

                    // Text reader object
                    tr = new XmlTextReader(new System.IO.StringReader(Resources.esquema));
                    xsc = new XmlSchemaCollection();
                    xsc.Add(null, tr);

                    // XML validator object
                    document.LoadXml(sXMLGenerado);

                    vr = new XmlValidatingReader(document.InnerXml,
                                    XmlNodeType.Document, null);

                    vr.Schemas.Add(xsc);

                    // Add validation event handler

                    vr.ValidationType = ValidationType.Schema;
                    vr.ValidationEventHandler +=
                                new ValidationEventHandler(ValidationHandler);

                    // Validate XML data
                    while (vr.Read())
                    {
                        if (vr.Name.Contains("Complemento") || vr.Name.Contains("Addenda"))
                        {
                            vr.Skip();
                        }
                    }

                    vr.Close();

                    txtResultados.Text += System.IO.Path.GetFileName(asArchivos[i]);
                    txtResultados.Text += System.Environment.NewLine;
                    txtResultados.Text += gsErrores;

                    if (i != asArchivos.Count() - 1)
                    {
                        txtResultados.Text += System.Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene los errores de validacion del esquema.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ValidationHandler(object sender, ValidationEventArgs args)
        {
            gsErrores += args.Message;
        }
        
        /// <summary>
        /// Función que genera el comprobante
        /// </summary>
        /// <param name="sLayout">Layout</param>
        /// <returns></returns>
        private string fnGenerarComprobante(string sLayout, string sNombreLayout)
        {
            int nBandera = 0;
            DateTime Fecha = DateTime.Today;
            string sCadenaOriginalEmisor = String.Empty;
            string linea = string.Empty;
            string lineaVersion = string.Empty;
            string noCertificado = string.Empty;
            string numeroCertificado = string.Empty;
            string sSello = string.Empty;

            string[] atributosVersionSeccion1 = null;
            string[] seccionVersion = null;

            System.IO.StringReader lectorVersion;
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            
            XmlDocument xDocumento = new XmlDocument();

            try
            {
                lectorVersion = new System.IO.StringReader(sLayout);
                while (true)
                {
                    lineaVersion = lectorVersion.ReadLine();
                    if (string.IsNullOrEmpty(lineaVersion))
                        break;

                    seccionVersion = lineaVersion.Split('?');

                    try
                    {
                        clsLlenarClases cLlenarClase = new clsLlenarClases(gsRutaCertificado, "12345678a", gbLlavePrivada, gsRutaGeneracion);
                        atributosVersionSeccion1 = seccionVersion[1].Split('|');

                        switch (seccionVersion[0])
                        {
                            case "co":
                                cLlenarClase.fnLlenarClaseComprobante(atributosVersionSeccion1);
                                break;

                            case "re":
                                cLlenarClase.fnLlenarClaseEmisor(atributosVersionSeccion1);
                                break;

                            case "de":
                                cLlenarClase.fnLlenarClaseEmisorDomicilio(atributosVersionSeccion1);
                                break;

                            case "ee":
                                cLlenarClase.fnLlenarClaseEmisorDomicilioExpedidoEn(atributosVersionSeccion1);
                                break;

                            case "rf":
                                cLlenarClase.fnLlenarClaseEmisorRegimen(atributosVersionSeccion1);
                                break;

                            case "rr":
                                cLlenarClase.fnLlenarClaseReceptor(atributosVersionSeccion1);
                                break;

                            case "dr":
                                cLlenarClase.fnLlenarClaseReceptorDomicilio(atributosVersionSeccion1);
                                break;

                            case "cc":
                                cLlenarClase.fnLlenarClaseConceptos(atributosVersionSeccion1);
                                break;

                            case "im":
                                cLlenarClase.fnLlenarClaseImpuestos(atributosVersionSeccion1);
                                break;

                            case "ir":
                                cLlenarClase.fnLlenarClaseImpuestosRetenidos(atributosVersionSeccion1);
                                break;

                            case "it":
                                cLlenarClase.fnLlenarClaseImpuestosTrasladados(atributosVersionSeccion1);
                                break;

                            case "nom":
                                cLlenarClase.fnLlenarClaseComplemento("nom", seccionVersion[0], atributosVersionSeccion1);
                                break;

                            case "percs":
                                cLlenarClase.fnLlenarClaseComplemento("nom", seccionVersion[0], atributosVersionSeccion1);
                                break;

                            case "per":
                                cLlenarClase.fnLlenarClaseComplemento("nom", seccionVersion[0], atributosVersionSeccion1);
                                break;

                            case "deducs":
                                cLlenarClase.fnLlenarClaseComplemento("nom", seccionVersion[0], atributosVersionSeccion1);
                                break;

                            case "dedu":
                                cLlenarClase.fnLlenarClaseComplemento("nom", seccionVersion[0], atributosVersionSeccion1);
                                break;

                            case "inca":
                                cLlenarClase.fnLlenarClaseComplemento("nom", seccionVersion[0], atributosVersionSeccion1);
                                break;

                            case "hora":
                                cLlenarClase.fnLlenarClaseComplemento("nom", seccionVersion[0], atributosVersionSeccion1);
                                break;

                        }
                    }
                    catch (Exception ex)
                    {
                        nBandera = 1;
                        throw new Exception(ex.Message);
                    }
                }

                lectorVersion.Close();

                if (!nBandera.Equals(0))
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return xDocumento.OuterXml;
        }

        /// <summary>
        /// Anexo 20 Eliminar en la reglas de estructura.
        /// </summary>
        /// <param name="varRep"></param>
        /// <returns></returns>
        private string fnReplaceCaracters(string varRep)
        {
            string sReplace = string.Empty;

            if (varRep.Contains('&'))
            {
                varRep.Replace("&", "&amp;");
            }

            if (varRep.Contains('<'))
            {
                varRep.Replace("<", "&lt;");
            }

            if (varRep.Contains('>'))
            {
                varRep.Replace(">", "&gt;");
            }

            if (varRep.Contains("'"))
            {
                varRep.Replace("'", "&apos;");
            }

            if (varRep.Contains("\""))
            {
                varRep.Replace("\"", "&quot;");
            }

            sReplace = varRep;
            return sReplace;
        }        
    }

    public class AgregarArchivos
    {
        string[] gListaArchivos;
        string gResultado;

        public AgregarArchivos(string[] paArchivos)
        {
            gListaArchivos = paArchivos;
            gResultado = string.Empty;
        }

        public void fnSeleccionarArchivo()
        {
            for (int i = 0; i < gListaArchivos.Count(); i++)
            {
                gResultado += gListaArchivos[i];

                if (i != gListaArchivos.Count() - 1)
                {
                    gResultado += "," + System.Environment.NewLine;
                }
            }
        }

        public string fnObtenerArchivos()
        { 
            return gResultado;
        }
    }
}

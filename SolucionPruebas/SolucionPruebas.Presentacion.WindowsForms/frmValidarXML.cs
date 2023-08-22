using SolucionPruebas.Presentacion.Servicios;
using SolucionPruebas.Presentacion.WindowsForms.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Windows.Forms;
using System.ServiceModel;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmValidarXML : Form
    {
        private string gsErrores = string.Empty;
        Servicios.wsValidacionTest.wcfValidaASMXSoapClient SDValidacionPrueba;
        Servicios.wsValidacionDesarrollo.wcfValidaASMXSoapClient SDValidacionDesarrollo;
        Servicios.wsValidaRetencionTest.wcfValidaASMXSoapClient SDValidacionRetencionTest;
        Servicios.ServicioLocal.ServiceClient wsServicioLocal;

        public frmValidarXML()
        {
            InitializeComponent();
        }
        private void btnSeleccionarArchivo_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileNames.Length.Equals(0))
                return;

            for (int i = 0; i < openFileDialog1.FileNames.Count(); i++)
			{
			    txtArchivo.Text += openFileDialog1.FileNames[i];

                if(i != openFileDialog1.FileNames.Count() - 1)
                {
                    txtArchivo.Text += ",";
                }
			}
        }
        private void rbArchivo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbArchivo.Checked)
            {
                rbLocal.Checked = false;
                rbDesarrollo.Checked = false;
                rbTest.Checked = false;
                rbProductivo.Checked = false;
            }
        } 
        private void rbLocal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLocal.Checked)
            {
                rbArchivo.Checked = false;
                rbDesarrollo.Checked = false;
                rbTest.Checked = false;
                rbProductivo.Checked = false;
            }
        }
        private void rbDesarrollo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDesarrollo.Checked)
            {
                rbArchivo.Checked = false;
                rbLocal.Checked = false;
                rbTest.Checked = false;
                rbProductivo.Checked = false;
            }
        }
        private void rbTest_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTest.Checked)
            {
                rbArchivo.Checked = false;
                rbLocal.Checked = false;
                rbDesarrollo.Checked = false;
                rbProductivo.Checked = false;
            }
        }
        private void rbProductivo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbProductivo.Checked)
            {
                rbArchivo.Checked = false;
                rbLocal.Checked = false;
                rbDesarrollo.Checked = false;
                rbTest.Checked = false;
            }
        }        
        private void btnValidar_Click(object sender, EventArgs e)
        {
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            try
            {
                string[] asArchivos = txtArchivo.Text.Split(',');
                txtResultados.Text = string.Empty;

                if (rbArchivo.Checked)
                    txtResultados.Text = fnValidarComprobanteArchivo(asArchivos);

                if (rbLocal.Checked)
                    txtResultados.Text = fnValidarComprobanteLocal(asArchivos);

                if (rbDesarrollo.Checked)
                    txtResultados.Text = fnValidarComprobanteDesarrollo(asArchivos);

                if (rbTest.Checked)
                    txtResultados.Text = fnValidarComprobanteTest(asArchivos);

                if (rbProductivo.Checked)
                    txtResultados.Text = fnValidarComprobanteProduccion(asArchivos);


                txtResultados.Text = fnValidarRetencionTest(asArchivos);                
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

        private string fnValidarComprobanteArchivo(string[] pasArchivos)
        {
            string sResultado = string.Empty;
            try
            {
                for (int i = 0; i < pasArchivos.Count(); i++)
                {
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
                    document.Load(pasArchivos[i]);

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
                        if (vr.Name.Contains("Addenda"))
                        {
                            vr.Skip();
                        }
                    }

                    vr.Close();

                    sResultado += System.IO.Path.GetFileName(pasArchivos[i]);
                    sResultado += System.Environment.NewLine;
                    sResultado += gsErrores;

                    if (i != pasArchivos.Count() - 1)
                    {
                        sResultado += System.Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el comprobante por archivo: " + ex.Message);
            }
            return sResultado;
        }

        private string fnValidarComprobanteLocal(string[] pasArchivos)
        {
            string sResultado = string.Empty;
            try
            {
                for (int i = 0; i < pasArchivos.Count(); i++)
                {
                    XmlDocument document = new XmlDocument();
                    XmlDocument documentoTimbrado = new XmlDocument();
                    string sDocumento = string.Empty;
                    document.Load(pasArchivos[i]);

                    //SDValidacionPrueba = Servicios.ProxyLocator.ObtenerServicioValidacionPrueba();
                    txtResultados.Text += pasArchivos[i];
                    //txtResultados.Text += SDValidacionPrueba.fnValidaXML(document.OuterXml, "WSDL_PAX", "wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=", "3.2");
               
                    if (i != pasArchivos.Count() - 1)
                    {
                        txtResultados.Text += System.Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el comprobante por archivo: " + ex.Message);
            }
            return sResultado;
        }

        private string fnValidarComprobanteDesarrollo(string[] pasArchivos)
        {
            string sResultado = string.Empty;
            try
            {
                for (int i = 0; i < pasArchivos.Count(); i++)
                {
                    XmlDocument document = new XmlDocument();
                    XmlDocument documentoTimbrado = new XmlDocument();
                    string sDocumento = string.Empty;
                    document.Load(pasArchivos[i]);

                    SDValidacionDesarrollo = Servicios.ProxyLocator.ObtenerServicioValidacionDesarrollo();
                    sResultado += pasArchivos[i];
                    sResultado += SDValidacionDesarrollo.fnValidaXML(document.OuterXml, "WSDL_PAX", "wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=", "3.2");

                    if (i != pasArchivos.Count() - 1)
                    {
                        sResultado += System.Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar en ambiente de Test: " + ex.Message);
            }
            return sResultado;
        }

        private string fnValidarComprobanteTest(string[] pasArchivos)
        {
            string sResultado = string.Empty;
            try
            {
                for (int i = 0; i < pasArchivos.Count(); i++)
                {
                    XmlDocument document = new XmlDocument();
                    XmlDocument documentoTimbrado = new XmlDocument();
                    string sDocumento = string.Empty;
                    document.Load(pasArchivos[i]);

                    SDValidacionPrueba = Servicios.ProxyLocator.ObtenerServicioValidacionPrueba();
                    txtResultados.Text += pasArchivos[i];
                    txtResultados.Text = SDValidacionPrueba.fnValidaXML(document.OuterXml, "WSDL_PAX", "wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=", "3.2");
                   
                    if (i != pasArchivos.Count() - 1)
                    {
                        txtResultados.Text += System.Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar en ambiente Desarrollo: " + ex.Message);
            }
            return sResultado;
        }

        private string fnValidarComprobanteProduccion(string[] pasArchivos)
        {
            string sResultado = string.Empty;
            try
            {
                for (int i = 0; i < pasArchivos.Count(); i++)
                {
                    XmlDocument document = new XmlDocument();
                    XmlDocument documentoTimbrado = new XmlDocument();
                    string sDocumento = string.Empty;
                    document.Load(pasArchivos[i]);

                    SDValidacionPrueba = Servicios.ProxyLocator.ObtenerServicioValidacionPrueba();
                    //SDValidacionDesarrollo = Servicios.ProxyLocator.ObtenerServicioValidacionDesarrollo();
                    txtResultados.Text = SDValidacionPrueba.fnValidaXML(document.OuterXml, "WSDL_PAX", "wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=", "3.2");
                    txtResultados.Text += pasArchivos[i];
                    //txtResultados.Text += " Resultado: " + SDValidacionDesarrollo.fnValidaXML(document.OuterXml, "ismael.hidalgo", "Z2/CpcODw4BtworChMOrw4AU77++N8ObQjpiPEXvv7vvvqrvvozvv7sV77yd776a77+I77+Q", "3.2");

                    if (i != pasArchivos.Count() - 1)
                    {
                        txtResultados.Text += System.Environment.NewLine;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el comprobante en ambiente productivo: " + ex.Message);
            }
            return sResultado;
        }

        private string fnValidarRetencionTest(string[] pasArchivos)
        {
            string sResultado = string.Empty;
            try
            {
                for (int i = 0; i < pasArchivos.Count(); i++)
                {
                    XmlDocument document = new XmlDocument();
                    XmlDocument documentoTimbrado = new XmlDocument();
                    string sDocumento = string.Empty;
                    document.Load(pasArchivos[i]);

                    SDValidacionRetencionTest = Servicios.ProxyLocator.ObtenerServicioValidacionRetencionTest();
                    //SDValidacionDesarrollo = Servicios.ProxyLocator.ObtenerServicioValidacionDesarrollo();
                    sResultado += SDValidacionRetencionTest.fnValidaXML(document.OuterXml, "ws_retenciones", "wrfCv8SVxITEscO8w43DhsSCxLBZcsKbwr/Em8OtwoJNwqTDo++9uO+/oO+/pREB77+Z776J772l", "1.0");
                    sResultado += pasArchivos[i];
                    //txtResultados.Text += " Resultado: " + SDValidacionDesarrollo.fnValidaXML(document.OuterXml, "ismael.hidalgo", "Z2/CpcODw4BtworChMOrw4AU77++N8ObQjpiPEXvv7vvvqrvvozvv7sV77yd776a77+I77+Q", "3.2");

                    if (i != pasArchivos.Count() - 1)
                    {
                        sResultado += System.Environment.NewLine;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar el comprobante en ambiente productivo: " + ex.Message);
            }
            return sResultado;
        }
    }
}

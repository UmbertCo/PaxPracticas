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
    public partial class frmValidarLayoutsNomina : Form
    {
        private string gsErrores = string.Empty;
        X509Certificate2 certEmisor = new X509Certificate2();
        OpenSSL_Lib.cSello cSello;
        string gsRutaPassword;
        string gsRutaLlavePrivada;

        public frmValidarLayoutsNomina()
        {
            InitializeComponent();
        }
        private void btnSeleccionarArchivo_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileNames.Length.Equals(0))
                return;

            txtArchivo.Text = string.Empty;

            AgregarArchivosNomina cAgregarAchivosNomina = new AgregarArchivosNomina(openFileDialog1.FileNames);
            Thread hilo = new Thread(new ThreadStart(cAgregarAchivosNomina.fnSeleccionarArchivo));
            hilo.Start();
            hilo.Join();
            txtArchivo.Text = cAgregarAchivosNomina.fnObtenerArchivos();
        }
        private void btnCargarLlavePrivada_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();

            if (openFileDialog2.FileName.Length.Equals(0))
                return;

            gsRutaLlavePrivada = openFileDialog2.FileName;
        }
        private void btnCargarCertificado_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();

            if (openFileDialog3.FileName.Length.Equals(0))
                return;

            certEmisor.Import(openFileDialog3.FileName);
        }
        private void btnCargarPassword_Click(object sender, EventArgs e)
        {
            openFileDialog4.ShowDialog();

            if (openFileDialog4.FileName.Length.Equals(0))
                return;

            gsRutaPassword = openFileDialog4.FileName;
        }
        private void btnValidar_Click(object sender, EventArgs e)
        {
            try
            {
                fnGenerarLlave();

                string[] asArchivos = txtArchivo.Text.Split(',');
                txtResultados.Text = string.Empty;

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

                    if (!sText.Substring(nIndiceRfc + 7 , 12).Equals("AAA010101AAA"))
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

        /// <summary>
        /// Función que genera el comprobante
        /// </summary>
        /// <param name="sLayout">Layout</param>
        /// <returns></returns>
        private string fnGenerarComprobante(string sLayout, string sNombreLayout)
        {

            int x = 0;

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
            XPathNavigator navNodoTimbre;
            XmlDocument xDocumento = new XmlDocument();


            Comprobante Cfd = new Comprobante();

            ComprobanteEmisor CEmisor = new ComprobanteEmisor();
            CEmisor.DomicilioFiscal = new t_UbicacionFiscal();
            CEmisor.ExpedidoEn = new t_Ubicacion();

            ComprobanteEmisorRegimenFiscal CERegimen = new ComprobanteEmisorRegimenFiscal();
            List<ComprobanteEmisorRegimenFiscal> ListRegimen = new List<ComprobanteEmisorRegimenFiscal>();

            ComprobanteReceptor CReceptor = new ComprobanteReceptor();
            CReceptor.Domicilio = new t_Ubicacion();

            List<ComprobanteConcepto> ListConcepto = new List<ComprobanteConcepto>();
            ComprobanteConcepto CConcepto = new ComprobanteConcepto();

            ComprobanteImpuestos CImpuestos = new ComprobanteImpuestos();
            ComprobanteImpuestosRetencion impuestosRetencion = new ComprobanteImpuestosRetencion();
            List<ComprobanteImpuestosRetencion> listaImpRetencion = new List<ComprobanteImpuestosRetencion>();


            Nomina CompNomina = new Nomina();

            NominaPercepciones nomPercepciones = new NominaPercepciones();
            List<NominaPercepcionesPercepcion> listaPercepciones = new List<NominaPercepcionesPercepcion>();

            NominaDeducciones nomDeducciones = new NominaDeducciones();
            List<NominaDeduccionesDeduccion> listaDeducciones = new List<NominaDeduccionesDeduccion>();

            List<NominaIncapacidad> listaIncapcidad = new List<NominaIncapacidad>();
            List<NominaHorasExtra> listaHorasExtra = new List<NominaHorasExtra>();

            string sCertificado = string.Empty;
            try
            {

                sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());
                byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
                numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();
            }
            catch { }


            try
            {
                lectorVersion = new System.IO.StringReader(sLayout);
                string sErrorAtributo = string.Empty;
                while (true)
                {
                    lineaVersion = lectorVersion.ReadLine();
                    if (string.IsNullOrEmpty(lineaVersion))
                        break;

                    seccionVersion = lineaVersion.Split('?');

                    try
                    {

                        atributosVersionSeccion1 = seccionVersion[1].Split('|');

                        switch (seccionVersion[0])
                        {
                            case "co":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("version"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.version = valores[1];
                                    }
                                    if (arreglo.Contains("serie"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.serie = valores[1];
                                    }
                                    if (arreglo.Contains("folio"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.folio = valores[1];
                                    }
                                    if (arreglo.Split('@')[0].Equals("fecha"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.fecha = Convert.ToDateTime(valores[1]);
                                    }


                                    if (arreglo.Contains("formaDePago"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.formaDePago = valores[1];
                                    }
                                    if (arreglo.Contains("noCertificado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        Cfd.noCertificado = numeroCertificado;
                                    }
                                    if (arreglo.Contains("certificado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        Cfd.certificado = sCertificado;
                                    }

                                    if (arreglo.Contains("condicionesDePago"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.condicionesDePago = valores[1];
                                    }

                                    if (arreglo.Contains("subTotal"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.subTotal = Convert.ToDecimal(valores[1]);
                                    }
                                    if (arreglo.Contains("descuento"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.descuento = Convert.ToDecimal(valores[1]);
                                        Cfd.descuentoSpecified = true;
                                    }
                                    if (arreglo.Contains("motivoDescuento"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.motivoDescuento = valores[1];
                                    }

                                    if (arreglo.Contains("TipoCambio"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.TipoCambio = valores[1];
                                    }

                                    if (arreglo.Contains("Moneda"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.Moneda = valores[1];
                                    }
                                    if (arreglo.Contains("total"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.total = Convert.ToDecimal(valores[1]);
                                    }
                                    if (arreglo.Contains("tipoDeComprobante"))
                                    {
                                        sErrorAtributo = arreglo;
                                        Cfd.tipoDeComprobante = ComprobanteTipoDeComprobante.egreso;
                                    }
                                    if (arreglo.Contains("metodoDePago"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.metodoDePago = valores[1];
                                    }

                                    if (arreglo.Contains("LugarExpedicion"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.LugarExpedicion = valores[1];
                                    }

                                    if (arreglo.Contains("NumCtaPago"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Cfd.NumCtaPago = valores[1];
                                    }


                                }

                                break;

                            case "re":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("rfc"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.rfc = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("nombre"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.nombre = fnReplaceCaracters(valores[1]);
                                    }

                                }

                                break;

                            case "de":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("calle"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.calle = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("noExterior"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.noExterior = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("noInterior"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.noInterior = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("colonia"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.colonia = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("localidad"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.localidad = fnReplaceCaracters(valores[1]);
                                    }

                                    if (arreglo.Contains("referencia"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.referencia = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("municipio"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.municipio = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("estado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.estado = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("pais"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.pais = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("codigoPostal"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.DomicilioFiscal.codigoPostal = fnReplaceCaracters(valores[1]);
                                    }
                                }

                                break;

                            case "ee":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("calle"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.calle = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("noExterior"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.noExterior = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("noInterior"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.noInterior = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("colonia"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.colonia = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("localidad"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.localidad = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("referencia"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.referencia = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("municipio"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.municipio = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("estado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.estado = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("pais"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.pais = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("codigoPostal"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CEmisor.ExpedidoEn.codigoPostal = fnReplaceCaracters(valores[1]);
                                    }
                                }

                                break;


                            case "rf":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("Regimen"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CERegimen.Regimen = fnReplaceCaracters(valores[1]);
                                        ListRegimen.Add(CERegimen);
                                    }
                                }

                                break;


                            case "rr":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("rfc"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.rfc = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("nombre"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.nombre = fnReplaceCaracters(valores[1]);
                                    }

                                }

                                break;

                            case "dr":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("calle"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.calle = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("noExterior"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.noExterior = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("noInterior"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.noInterior = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("colonia"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.colonia = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("localidad"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.localidad = fnReplaceCaracters(valores[1]);
                                    }

                                    if (arreglo.Contains("referencia"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.referencia = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("municipio"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.municipio = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("estado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.estado = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("pais"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.pais = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("codigoPostal"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CReceptor.Domicilio.codigoPostal = fnReplaceCaracters(valores[1]);
                                    }

                                }

                                break;
                            case "cc":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("cantidad"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CConcepto.cantidad = Convert.ToDecimal(valores[1]);
                                    }
                                    if (arreglo.Contains("unidad"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CConcepto.unidad = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("noIdentificacion"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CConcepto.noIdentificacion = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("descripcion"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CConcepto.descripcion = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("valorUnitario"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CConcepto.valorUnitario = Convert.ToDecimal(valores[1]);
                                    }
                                    if (arreglo.Contains("importe"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CConcepto.importe = Convert.ToDecimal(valores[1]);
                                    }
                                }
                                ListConcepto.Add(CConcepto);
                                break;

                            case "im":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("totalImpuestosRetenidos"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CImpuestos.totalImpuestosRetenidos = Convert.ToDecimal(valores[1]);
                                    }
                                }

                                break;

                            case "ir":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("impuesto"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        impuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto.ISR;
                                    }
                                    if (arreglo.Contains("importe"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        impuestosRetencion.importe = Convert.ToDecimal(valores[1]);
                                    }
                                }

                                listaImpRetencion.Add(impuestosRetencion);

                                break;

                            case "nom":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("Version"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.Version = valores[1];
                                    }
                                    if (arreglo.Contains("RegistroPatronal"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.RegistroPatronal = valores[1];
                                    }
                                    if (arreglo.Contains("NumEmpleado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.NumEmpleado = valores[1];
                                    }
                                    if (arreglo.Contains("CURP"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.CURP = valores[1];
                                    }
                                    if (arreglo.Contains("TipoRegimen"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.TipoRegimen = Convert.ToInt32(valores[1]);
                                    }
                                    if (arreglo.Contains("NumSeguridadSocial"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.NumSeguridadSocial = valores[1];
                                    }
                                    if (arreglo.Contains("FechaPago"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.FechaPago = Convert.ToDateTime(valores[1]);
                                    }
                                    if (arreglo.Contains("FechaInicialPago"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.FechaInicialPago = Convert.ToDateTime(valores[1]);
                                    }
                                    if (arreglo.Contains("FechaFinalPago"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.FechaFinalPago = Convert.ToDateTime(valores[1]);
                                    }
                                    if (arreglo.Contains("NumDiasPagados"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.NumDiasPagados = Convert.ToDecimal(valores[1]);
                                    }
                                    if (arreglo.Contains("Departamento"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.Departamento = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("CLABE"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.CLABE = valores[1];
                                    }
                                    if (arreglo.Contains("Banco"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.Banco = fnReplaceCaracters(valores[1]);
                                        CompNomina.BancoSpecified = true;
                                    }
                                    if (arreglo.Contains("FechaInicioRelLaboral"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.FechaInicioRelLaboral = Convert.ToDateTime(valores[1]);
                                        CompNomina.FechaInicioRelLaboralSpecified = true;
                                    }
                                    if (arreglo.Contains("Antiguedad"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.Antiguedad = Convert.ToInt32(valores[1]);
                                        CompNomina.AntiguedadSpecified = true;
                                    }
                                    if (arreglo.Split('@')[0].Equals("Puesto"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.Puesto = fnReplaceCaracters(valores[1]);

                                    }
                                    if (arreglo.Contains("TipoContrato"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.TipoContrato = valores[1];
                                    }
                                    if (arreglo.Contains("TipoJornada"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.TipoJornada = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("PeriodicidadPago"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.PeriodicidadPago = fnReplaceCaracters(valores[1]);
                                    }
                                    if (arreglo.Contains("SalarioBaseCotApor"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.SalarioBaseCotApor = Convert.ToDecimal(valores[1]);
                                        CompNomina.SalarioBaseCotAporSpecified = true;
                                    }
                                    if (arreglo.Contains("RiesgoPuesto"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.RiesgoPuesto = Convert.ToInt32(valores[1]);
                                        CompNomina.RiesgoPuestoSpecified = true;
                                    }
                                    if (arreglo.Contains("SalarioDiarioIntegrado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        CompNomina.SalarioDiarioIntegrado = Convert.ToDecimal(valores[1]);
                                        CompNomina.SalarioDiarioIntegradoSpecified = true;
                                    }
                                }

                                break;

                            case "percs":

                                foreach (string arreglo in atributosVersionSeccion1)
                                {

                                    if (arreglo.Contains("TotalGravado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        nomPercepciones.TotalGravado = Convert.ToDecimal(valores[1]);
                                    }
                                    if (arreglo.Contains("TotalExento"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        nomPercepciones.TotalExento = Convert.ToDecimal(valores[1]);
                                    }
                                }

                                break;

                            case "per":

                                NominaPercepcionesPercepcion Percepcion = new NominaPercepcionesPercepcion();

                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("TipoPercepcion"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Percepcion.TipoPercepcion = valores[1];
                                    }
                                    if (arreglo.Contains("Clave"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Percepcion.Clave = valores[1];
                                    }
                                    if (arreglo.Contains("Concepto"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Percepcion.Concepto = valores[1];
                                    }
                                    if (arreglo.Contains("ImporteGravado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Percepcion.ImporteGravado = Convert.ToDecimal(valores[1]);
                                    }
                                    if (arreglo.Contains("ImporteExento"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Percepcion.ImporteExento = Convert.ToDecimal(valores[1]);
                                    }
                                }

                                listaPercepciones.Add(Percepcion);

                                break;

                            case "deducs":

                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("TotalGravado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        nomDeducciones.TotalGravado = Convert.ToDecimal(valores[1]);
                                    }
                                    if (arreglo.Contains("TotalExento"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        nomDeducciones.TotalExento = Convert.ToDecimal(valores[1]);
                                    }
                                }

                                break;

                            case "dedu":

                                NominaDeduccionesDeduccion Deduccion = new NominaDeduccionesDeduccion();

                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("TipoDeduccion"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Deduccion.TipoDeduccion = valores[1];
                                    }
                                    if (arreglo.Contains("Clave"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Deduccion.Clave = valores[1];
                                    }
                                    if (arreglo.Contains("Concepto"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Deduccion.Concepto = valores[1];
                                    }
                                    if (arreglo.Contains("ImporteGravado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Deduccion.ImporteGravado = Convert.ToDecimal(valores[1]);
                                    }

                                    if (arreglo.Contains("ImporteExento"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Deduccion.ImporteExento = Convert.ToDecimal(valores[1]);
                                    }
                                }

                                listaDeducciones.Add(Deduccion);

                                break;


                            case "inca":

                                NominaIncapacidad Incapacidad = new NominaIncapacidad();

                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("DiasIncapacidad"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Incapacidad.DiasIncapacidad = Convert.ToDecimal(valores[1]);
                                    }
                                    if (arreglo.Contains("TipoIncapacidad"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Incapacidad.TipoIncapacidad = Convert.ToInt32(valores[1]);
                                    }
                                    if (arreglo.Contains("Descuento"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        Incapacidad.Descuento = Convert.ToDecimal(valores[1]);
                                    }
                                }

                                listaIncapcidad.Add(Incapacidad);

                                break;

                            case "hora":

                                NominaHorasExtra HoraExtra = new NominaHorasExtra();

                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("Dias"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        HoraExtra.Dias = Convert.ToInt32(valores[1]);
                                    }
                                    if (arreglo.Contains("TipoHoras"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');

                                        switch (valores[1].ToString())
                                        {
                                            case "Dobles":
                                                HoraExtra.TipoHoras = NominaHorasExtraTipoHoras.Dobles;
                                                break;
                                            case "Triples":
                                                HoraExtra.TipoHoras = NominaHorasExtraTipoHoras.Triples;
                                                break;
                                            default:
                                                break;
                                        }

                                    }
                                    if (arreglo.Contains("HorasExtra"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        HoraExtra.HorasExtra = Convert.ToInt32(valores[1]);
                                    }

                                    if (arreglo.Contains("ImportePagado"))
                                    {
                                        sErrorAtributo = arreglo;
                                        string[] valores = arreglo.Split('@');
                                        HoraExtra.ImportePagado = Convert.ToDecimal(valores[1]);
                                    }
                                }


                                listaHorasExtra.Add(HoraExtra);

                                break;

                        }
                    }
                    catch (Exception ex)
                    {
                        nBandera = 1;
                        throw new Exception("Error en la generación del comprobante. " + sErrorAtributo + ":" + ex.Message);
                        return string.Empty;
                    }
                }

                lectorVersion.Close();

                if (!nBandera.Equals(0))
                {
                    return string.Empty;
                }

                if (listaImpRetencion.Count > 0)
                {
                    CImpuestos.Retenciones = listaImpRetencion.ToArray();
                    CImpuestos.totalImpuestosRetenidosSpecified = true;
                    CImpuestos.totalImpuestosRetenidos = CImpuestos.totalImpuestosRetenidos;
                }

                if (listaPercepciones.Count > 0)
                {
                    nomPercepciones.Percepcion = listaPercepciones.ToArray();
                    CompNomina.Percepciones = nomPercepciones;
                }

                if (listaDeducciones.Count > 0)
                {
                    nomDeducciones.Deduccion = listaDeducciones.ToArray();
                    CompNomina.Deducciones = nomDeducciones;
                }

                if (listaIncapcidad.Count > 0)
                {
                    CompNomina.Incapacidades = listaIncapcidad.ToArray();
                }

                if (listaHorasExtra.Count > 0)
                {
                    CompNomina.HorasExtras = listaHorasExtra.ToArray();
                }

                if (ListConcepto.Count > 0)
                {
                    Cfd.Conceptos = ListConcepto.ToArray();
                }

                if (ListRegimen.Count > 0)
                {
                    CEmisor.RegimenFiscal = ListRegimen.ToArray();
                }

                MemoryStream ms = new MemoryStream();
                StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);

                XmlDocument xmlComplNomina = new XmlDocument();
                XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
                sns.Add("nomina", "http://www.sat.gob.mx/nomina");

                XmlSerializer serializador = new XmlSerializer(typeof(Nomina));
                serializador.Serialize(sw, CompNomina, sns);


                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                xmlComplNomina.LoadXml(sr.ReadToEnd());

                XmlElement xeComplNomina = xmlComplNomina.DocumentElement;

                ComprobanteComplemento complNomina = new ComprobanteComplemento();
                XmlElement[] axeComplNomina = new XmlElement[] { xeComplNomina };
                complNomina.Any = axeComplNomina;


                Cfd.Complemento = complNomina;
                Cfd.Emisor = CEmisor;
                Cfd.Receptor = CReceptor;
                Cfd.Impuestos = CImpuestos;

                //-----------------------------------------------------------------------------------------------------------------------

                string tNameSpace = "nomina" + "|" + "http://www.sat.gob.mx/nomina" + "|" + "http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina11.xsd";
                xDocumento = fnGenerarXML32(Cfd, tNameSpace);

                navNodoTimbre = xDocumento.CreateNavigator();
                sCadenaOriginalEmisor = fnConstruirCadenaTimbrado(navNodoTimbre);
                cSello.sCadenaOriginal = sCadenaOriginalEmisor;
                Cfd.sello = cSello.sSello;


                //Valida sello
                if (!fnVerificarSello(sCadenaOriginalEmisor, Cfd.sello))
                {
                    throw new Exception("Sello invalido");
                    return string.Empty;
                }


                xDocumento = fnGenerarXML32(Cfd, tNameSpace);


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
        public static string fnReplaceCaracters(string varRep)
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

        /// <summary>
        /// Genera el XML en base a la estructura que contiene los datos version 3.2
        /// </summary>
        /// <param name="datos">Estructura que contiene los datos</param>
        /// <returns>XmlDocument con los datos del objeto Comprobante</returns>
        public XmlDocument fnGenerarXML32(Comprobante datos, string tNamespace)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
            XmlDocument xXml = new XmlDocument();
            XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
            sns.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
            string[] pspace = { "" };
            if (!(tNamespace == null))
            {
                pspace = tNamespace.Split('|');
                if (pspace.Length > 1)
                {
                    sns.Add(pspace[0], pspace[1]);
                }
            }
            XmlSerializer serializador = new XmlSerializer(typeof(Comprobante));
            try
            {
                serializador.Serialize(sw, datos, sns);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);

                xXml.LoadXml(sr.ReadToEnd());
                if (!(tNamespace == null))
                {
                    XmlAttribute att = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    att.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd" + " " + pspace[1] + " " + pspace[2];
                    xXml.DocumentElement.SetAttributeNode(att);
                }
                else
                {
                    XmlAttribute att = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                    att.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
                    xXml.DocumentElement.SetAttributeNode(att);

                }


                return xXml;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Comprueba que el sello del comprobante refleje los datos de la cadena original
        /// </summary>
        /// <param name="psCadenaOriginal">Cadena original del comprobante</param>
        /// <returns>Booleano indicando si la cadena original corresponde al sello</returns>
        public bool fnVerificarSello(string psCadenaOriginal, string psSello)
        {
            RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certEmisor.PublicKey.Key);
            try
            {
                //Verificamos que el certificado obtenga el mismo resultado que el del sello
                byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(psCadenaOriginal));
                bool exito = publica.VerifyHash(
                        hash,
                        "sha1",
                        Convert.FromBase64String(psSello));

                return exito;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Función que genera las llaves para la generación del sello
        /// </summary>
        private void fnGenerarLlave()
        {
            //Llave del Emisor
            //*****************************
            //Se cambiaria el metodo de sellado por OpenSSL
            cSello = new cSello(gsRutaLlavePrivada, gsRutaPassword, @"C:\HIDALGO\ProyectosTF\SolucionPruebas\SolucionPruebas.Presentacion.WindowsForms\bin\Debug\PEM\");
            //
            //*****************************

        }        
    }

    public class AgregarArchivosNomina
    {
        string[] gListaArchivos;
        string gResultado;

        public AgregarArchivosNomina(string[] paArchivos)
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

using SolucionPruebas.Presentacion.Servicios;
using SolucionPruebas.Presentacion.Servicios.wsRecepcionDesarrollo;
//using SolucionPruebas.Presentacion.Servicios.TwsRecepcionTASMX;
using SolucionPruebas.Presentacion.Servicios.ServicioLocal;
//using SolucionPruebas.Presentacion.Servicios.RecepcionLocal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmTimbrado : Form
    {
        public frmTimbrado()
        {
            InitializeComponent();
        }

        Servicios.wsRecepcionDesarrollo.wcfRecepcionASMXSoapClient wsRecepcionDesarrollo;
        Servicios.ServicioLocal.ServiceClient wsServicioLocal;
        Servicios.wsRecepcionDP.wcfRecepcionASMXSoapClient wsRecepcionDP;
        Servicios.wsRecepcionDPAspel.wcfRecepcionASPELSoapClient wsRecepcionDPAspel;
        Servicios.wsRecepcionDPSVC.IwcfRecepcionClient wsRecepcionDPSVC;
        Servicios.wsRecepcionTestASMX.wcfRecepcionASMXSoapClient wsRecepcionTestDP;
        Servicios.wsRecepcionProduccion.wcfRecepcionASMXSoapClient wsRecepcionProduccion;
        Servicios.wsRecepcionLocalRetencion.wcfRecepcionASMXSoapClient wsRecepcionRetencionLocal;
        Servicios.wsRecepcionRetencionProduccion.wcfRecepcionASMXSoapClient wsRecepcionRetencionProduccion;

        public class PruebaJson
        {
            public string psId { get; set; }
        }

        private void rbProduccion_CheckedChanged(object sender, EventArgs e)
        {
            rbLocal.Checked = false;
            rbPruebas.Checked = false;
            rbTest.Checked = false;
            rbDP.Checked = false;
        }
        private void rbTest_CheckedChanged(object sender, EventArgs e)
        {
            rbLocal.Checked = false;
            rbPruebas.Checked = false;
            rbProduccion.Checked = false;
            rbDP.Checked = false;
        }
        private void rbPruebas_CheckedChanged(object sender, EventArgs e)
        {
            rbLocal.Checked = false;
            rbProduccion.Checked = false;
            rbTest.Checked = false;
            rbDP.Checked = false;
        }
        private void rbLocal_CheckedChanged(object sender, EventArgs e)
        {
            rbProduccion.Checked = false;
            rbPruebas.Checked = false;
            rbTest.Checked = false;
            rbDP.Checked = false;
        }
        private void rbDP_CheckedChanged(object sender, EventArgs e)
        {
            rbLocal.Checked = false;
            rbPruebas.Checked = false;
            rbTest.Checked = false;
            rbProduccion.Checked = false;
        }
        private void btnEnviar_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDocGenera = new XmlDocument();
            String xml = string.Empty;
            String sCadenaOriginal = String.Empty;
            String sSello = String.Empty;
            String sCertificado = String.Empty;
            String nNumeroCertificado = String.Empty;
            byte[] bLlave = null;
            OpenFileDialog ofdArchivo = new OpenFileDialog();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

                txtResultado.Text = string.Empty;

                ofdArchivo.ShowDialog();
                if (ofdArchivo.FileName.Equals(string.Empty))
                    return;

                Stream archivo = File.Open(ofdArchivo.FileName, FileMode.Open);
                StreamReader sr = new StreamReader(archivo);
                xml = sr.ReadToEnd();
                archivo.Close();

                xmlDocGenera.LoadXml(xml);

                if (cbSellarPrueba.Checked)
                {
                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlDocGenera.NameTable);
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                    nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    XPathNavigator navDocGenera = xmlDocGenera.CreateNavigator();
                    
                    sCadenaOriginal = fnConstruirCadenaTimbrado(xmlDocGenera.CreateNavigator());
                    bLlave = File.ReadAllBytes(Settings1.Default.RutaLlave);

                    wsServicioLocal = ProxyLocator.ObtenerServicioLocal();
                    sSello = wsServicioLocal.fnGenerarSello(Settings1.Default.RutaPfx, sCadenaOriginal, bLlave, "12345678a");
                    sSello = sSello.Replace("\n", "");

                    //xmlDocGenera.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sSello);
                    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sSello);
                    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).SetValue(nNumeroCertificado);
                    navDocGenera.SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);
                }

                if (rbProduccion.Checked)
                    txtResultado.Text = fnTimbrarProduccion(xmlDocGenera.InnerXml, cbTipoComprobante.Text);

                if (rbPruebas.Checked)
                    txtResultado.Text = fnTimbrarRetencionLocal(xmlDocGenera.InnerXml, cbTipoComprobante.Text);

                if (rbTest.Checked)
                    txtResultado.Text = fnTimbrarTest(xmlDocGenera.InnerXml, cbTipoComprobante.Text);

                if (rbDP.Checked)
                    txtResultado.Text = fnTimbrarDP(xmlDocGenera.InnerXml, cbTipoComprobante.Text);

                //byte[] retXML = System.Text.Encoding.UTF8.GetBytes(sCadenaOriginal);
                //string sXml = Convert.ToBase64String(retXML);

                //string HASH = fnGetHASH(sCadenaOriginal);
                //sResultado = HASH;
                                
                //sResultado = fnTimbraServicioPruebaJSON(xmlDocGenera.InnerXml);

                //wsGeneracionTimbrado = Servicios.ProxyLocator.ObtenerServicioGeneracionTimbrado();
                //Servicios.wsGeneracionTimbrado.ArrayOfAnyType oResultado = new Servicios.wsGeneracionTimbrado.ArrayOfAnyType();
                //oResultado = wsGeneracionTimbrado.fnEnviarTXTPAX001(xml, "Factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2", "");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnHashCadena_Click(object sender, EventArgs e)
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
        private void btnCadenaBase64_Click(object sender, EventArgs e)
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

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Función que obtiene el Hash de una cadena original
        /// </summary>
        /// <param name="psCadenaOriginal">Cadena original</param>
        /// <returns></returns>
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

        public string fnTimbrar(string psComprobante)
        {
            string sResultado = string.Empty;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
                string uri = "https://test.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Headers.Add("SOAPAction", "https://test.paxfacturacion.com.mx:453/fnEnviarXML");
                request.Method = "POST";
                request.Timeout = 2147483647;
                //request.Headers.Clear();
                request.KeepAlive = false;
                //request.Accept = "text/xml";
                //request.ContentType = "text/xml;charset=\"utf-8\"";
                request.ContentType = "application/soap+xml; charset=utf-8";

                string sParametros = string.Empty;
                sParametros = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                sParametros += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
                sParametros += "<soap:Body>";
                sParametros += "  <fnEnviarXML xmlns=\"https://test.paxfacturacion.com.mx:453\">";
                sParametros += "    <psComprobante>@Comprobante</psComprobante>";
                sParametros += "    <psTipoDocumento>Factura</psTipoDocumento>";
                sParametros += "    <pnId_Estructura>0</pnId_Estructura>";
                sParametros += "    <sNombre>WSDL</sNombre>";
                sParametros += "    <sContraseña>O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw=</sContraseña>";
                sParametros += "    <sVersion>3.2</sVersion>";
                sParametros += "  </fnEnviarXML>";
                sParametros += "</soap:Body>";
                sParametros += "</soap:Envelope>";

                sParametros = sParametros.Replace("@Comprobante", HttpUtility.HtmlEncode(fnEscapear(psComprobante)));
                //byte[] abDatos = Encoding.UTF8.GetBytes(sParametros);
                request.ContentLength = sParametros.Length;
                //XmlDocument xdEnvelope = new XmlDocument();
                //xdEnvelope.LoadXml(sParametros);

                Stream stream = request.GetRequestStream();
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(sParametros);
                    sw.Close();
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        sResultado = rd.ReadToEnd();
                    }
                }
                //using (Stream stream = request.GetRequestStream())
                //{
                //    xdEnvelope.Save(stream);
                //}

                //using (WebResponse response = request.GetResponse())
                //{
                //    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                //    {
                //        sResultado = rd.ReadToEnd();
                //    }
                //}
            }
            catch (UriFormatException)
            {
                return "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sResultado;
        }

        public string fnTimbrarHello(string psComprobante)
        {
            string sResultado = string.Empty;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
                string uri = "http://localhost:5249/ServiceRecepcion.asmx/HelloWorld";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                //request.Headers.Add("SOAPAction", "http://localhost:5249/ServiceRecepcion.asmx/HelloWorld");
                request.Headers.Clear();
                request.Method = "POST";
                //request.Accept = "text/xml";
                request.ContentType = "text/xml;charset=\"utf-8\"";

                string sParametros = string.Empty;
                sParametros = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                sParametros += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
                sParametros += "<soap:Body>";
                sParametros += "  <HelloWorld xmlns=\"http://tempuri.org\">";
                sParametros += "  </HelloWorld>";
                sParametros += "</soap:Body>";
                sParametros += "</soap:Envelope>";

                //Encoding encode = Encoding.GetEncoding("utf-8");
                //HttpWebResponse webres = null;
                //webres = (HttpWebResponse)request.GetResponse();
                //Stream reader = null;
                //reader = webres.GetResponseStream();
                //StreamReader sreader = new StreamReader(reader, encode, true);
                //string result = sreader.ReadToEnd();

                Stream stream = request.GetRequestStream();
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(sParametros);
                    sw.Close();
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        sResultado = rd.ReadToEnd();
                    }
                }
            }
            catch (UriFormatException)
            {
                return "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sResultado;
        }

        public string fnTimbrarServicioPrueba(string psComprobante)
        {
            string sResultado = string.Empty;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

                string sParametros = string.Empty;
                sParametros = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
                sParametros += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">";
                sParametros += "<soap:Body>";
                sParametros += "  <fnServicioPrueba xmlns=\"http://tempuri.org\">";
                sParametros += "    <psPrueba>Prueba</psPrueba>";
                sParametros += "  </fnServicioPrueba>";
                sParametros += "</soap:Body>";
                sParametros += "</soap:Envelope>";

                //string uri = "http://localhost:5249/ServiceRecepcion.asmx/fnServicioPrueba";
                string uri = "http://localhost:5249/ServiceRecepcion.svc/fnServicioPrueba";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                //request.Headers.Add("SOAPAction", "http://localhost:5249/IService/fnServicioPrueba");
                //request.Headers["SOAPAction"] = "http://localhost:5249/IService/fnServicioPrueba";
                //request.Headers.Add("SOAPAction", "http://tempuri.org/IService/fnServicioPrueba");
                //request.Headers["SOAPAction"] = "http://tempuri.org/IService/fnServicioPrueba";
                //request.ProtocolVersion = HttpVersion.Version10;
                //request.Headers.Clear();
                request.Method = "POST";
                request.KeepAlive = false;
                //request.Accept = "text/xml";
                //request.ContentType = "text/xml;charset=\"utf-8\"";
                //request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = Encoding.UTF8.GetBytes(sParametros).Length;
                

                //Encoding encode = Encoding.GetEncoding("utf-8");
                //HttpWebResponse webres = null;
                //webres = (HttpWebResponse)request.GetResponse();
                //Stream reader = null;
                //reader = webres.GetResponseStream();
                //StreamReader sreader = new StreamReader(reader, encode, true);
                //string result = sreader.ReadToEnd();

                
                Stream stream = request.GetRequestStream();
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(sParametros);
                    sw.Close();
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        sResultado = rd.ReadToEnd();
                    }
                }
            }
            catch (UriFormatException)
            {
                return "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sResultado;
        }

        public string fnTimbraServicioPruebaJSON(string psComprobante)
        { 
            string sResultado = string.Empty;
            string parameters = string.Empty;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

                PruebaJson objeto = new PruebaJson();
                objeto.psId = "Test";

                JavaScriptSerializer js = new JavaScriptSerializer();
                //string parameters = js.Serialize(objeto);

                HttpWebRequest request = WebRequest.Create("http://localhost:5249/JSONService.svc/chkPrueba") as HttpWebRequest;
                request.Method = "POST";
                request.ContentLength = parameters.Length;
                request.ContentType = "application/json";

                using (StreamWriter requestWriter2 = new StreamWriter(request.GetRequestStream()))
                {
                    requestWriter2.Write(parameters);
                }

                using (StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    // dumps the HTML from the response into a string variable
                    sResultado = responseReader.ReadToEnd();
                }
            }
            catch (UriFormatException)
            {
                return "0";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sResultado;
        }

        public string fnEscapear(string psComprobante)
        {
            string sResultado = string.Empty;
            try
            {
                sResultado = psComprobante.Replace("<", "&lt;").Replace(">", "&gt;");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return sResultado;
        }

        private string StreamToString(Stream str)
        {
            using (BinaryReader rdr = new BinaryReader(str))
            {
                using (MemoryStream mem = new MemoryStream())
                {
                    byte[] buf = new byte[256];
                    int bt;
                    int bts = 0;
                    while ((bt = rdr.Read(buf, 0, 256)) > 0)
                    {
                        mem.Write(buf, 0, bt);
                        bts += bt;
                    }
                    mem.Position = 0;
                    byte[] bytes = new byte[bts];
                    mem.Read(bytes, 0, bytes.Length);
                    return Encoding.ASCII.GetString(bytes);
                }
            }
        }

        private string fnTimbrarDP(string psComprobante, string psTipoDocumento)
        {
            string sResultado = string.Empty;

            wsRecepcionDP = Servicios.ProxyLocator.ObtenerServicioTimbradoDP();
            sResultado = wsRecepcionDP.fnEnviarXML(psComprobante, psTipoDocumento, 0, "Paxgeneracion", "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB", "3.2");

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sResultado);

            return sResultado;
        }

        private string fnTimbrarDPSVC(string psComprobante, string psTipoDocumento)
        {
            string sResultado = string.Empty;

            wsRecepcionDPSVC = Servicios.ProxyLocator.ObtenerServicioTimbradoDPSVC();
            sResultado = wsRecepcionDPSVC.fnEnviarXML(psComprobante, psTipoDocumento, 0, "Paxgeneracion", "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB", "3.2");

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sResultado);

            return sResultado;
        }

        private string fnTimbrarDPAspel(string psComprobante, string psTipoDocumento)
        {
            string sResultado = string.Empty;

            wsRecepcionDPAspel = Servicios.ProxyLocator.ObtenerServicioTimbradoDPAspel();
            sResultado = wsRecepcionDPAspel.fnEnviarXML(psComprobante, psTipoDocumento, 0, "Paxgeneracion", "UHJ1ZWJhMTIr", "3.2");

            XmlDocument xd = new XmlDocument();
            xd.LoadXml(sResultado);

            return sResultado;
        } 

        private string fnTimbrarProduccion(string psComprobante, string psTipoDocumento)
        {
            string sResultado = string.Empty;

            wsRecepcionProduccion = Servicios.ProxyLocator.ObtenerServicioRecepcionProduccion();
            sResultado = wsRecepcionProduccion.fnEnviarXML(psComprobante, psTipoDocumento, 0, "Paxgeneracion", "NT1RwqPCpV7CtGB0fSLvv5hQK++/oXw5VDfvv6LvvZ4z77+U77+R77+p7767BO++qw==", "3.2");    

            return sResultado;
        }

        private string fnTimbrarPruebas(string psComprobante, string psTipoDocumento)
        {
            string sResultado = string.Empty;

            wsRecepcionDesarrollo = Servicios.ProxyLocator.ObtenerServicioTimbradoDesarrollo();
            sResultado = wsRecepcionDesarrollo.fnEnviarXML(psComprobante, psTipoDocumento, 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");

            return sResultado;
        }

        private string fnTimbrarTest(string psComprobante, string psTipoDocumento)
        {
            string sResultado = string.Empty;

            wsRecepcionTestDP = Servicios.ProxyLocator.ObtenerServicioTimbradoTest();
            sResultado = wsRecepcionTestDP.fnEnviarXML(psComprobante, psTipoDocumento, 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");

            return sResultado;
        }

        private string fnTimbrarRetencionLocal(string psComprobante, string psTipoDocumento)
        {
            string sResultado = string.Empty;

            //wsRecepcionRetencionLocal = Servicios.ProxyLocator.ObtenerServicioRecepcionRetencionLocal();
            //sResultado = wsRecepcionRetencionLocal.fnEnviarXML(psComprobante, 1, 0, "ismael.hidalgo", "wrLCusSiw7jDkMOkw6bEocS4w47vv7krw4jCpEg4ZcK5LsKK776t77+KHe++tO+9pO+/n++/iu++hO++kA==", "1.0");

            wsRecepcionRetencionProduccion = Servicios.ProxyLocator.ObtenerServicioRecepcionRetencionProduccion();
            sResultado = wsRecepcionRetencionProduccion.fnEnviarXML(psComprobante, 1, 0, "ismael.hidalgo", "wrLCusSiw7jDkMOkw6bEocS4w47vv7krw4jCpEg4ZcK5LsKK776t77+KHe++tO+9pO+/n++/iu++hO++kA==", "1.0");

            return sResultado;
        }
    }
}

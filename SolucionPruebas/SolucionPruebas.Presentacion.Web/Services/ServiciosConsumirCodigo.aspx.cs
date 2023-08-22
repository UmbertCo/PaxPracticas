using SolucionPruebas.Presentacion.Servicios;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Services
{
    public partial class ServiciosConsumirCodigo : System.Web.UI.Page
    {
        private Servicios.wsRecepcionProduccion.wcfRecepcionASMXSoapClient SDRecepcionProduccion;
        private Servicios.ServicioLocal.ServiceClient SDServicioLocal;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            string sResultado = string.Empty;
            string sCadenaOriginal = string.Empty;
            string sSello = string.Empty;
            string sMensaje = string.Empty;
            XmlDocument soapEnvelopeXml = new XmlDocument();
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

                HttpFileCollection hfc = Request.Files;
                HttpPostedFile hpf = hfc[0];

                if (hpf.ContentLength < 0)
                    return;

                document.Load(hpf.InputStream);

                nsm = new XmlNamespaceManager(document.NameTable);
                nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                SDServicioLocal = Servicios.ProxyLocator.ObtenerServicioLocal();
                sCadenaOriginal = SDServicioLocal.fnAplicarHojaTransformacion(document.InnerXml);

                sSello = SDServicioLocal.fnGenerarSelloRutas(Settings.Default.RutaPfx, sCadenaOriginal, Settings.Default.RutaLlave, Settings.Default.RutaPassword);
                sSello = sSello.Replace("\n", "");
                document.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsm).SetValue(sSello);

                string local = SDServicioLocal.fnServicioPrueba("Entrada");

                string sSoap = string.Empty;
                string localCodigo = string.Empty;

                sSoap = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soapenv:Envelope xmlns:tem=""http://tempuri.org/""
                       xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <soapenv:Header/>
                          <soap:Body>
                            <tem:fnServicioPrueba>
                                <tem:psPrueba>Entrada</tem:psPrueba>
                            </tem:fnServicioPrueba>
                          </soap:Body>
                    </soapenv:Envelope>";

                HttpWebRequest request = CreateWebRequest();
                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        Console.WriteLine(soapResult);
                    }
                }


                sSoap = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soapenv:Envelope xmlns:test=""https://test.paxfacturacion.com.mx:453""
                       xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
                        <soapenv:Header/>
                          <soap:Body>
                            <test:fnEnviarXML>
                                @Datos
                            </test:fnEnviarXML>
                          </soap:Body>
                    </soapenv:Envelope>";

                string sDocumento = document.InnerXml.Replace("<", "&lt;").Replace(">","&gt;").Replace(@"""", "&quot;");

                StringBuilder sbParameters = new StringBuilder();
                sbParameters.Append("<test:psComprobante>" + document.InnerXml + "</test:psComprobante>");
                sbParameters.Append("<test:psTipoDocumento>Factura</test:psTipoDocumento>");
                sbParameters.Append("<test:psId_Estructura>0</test:psId_Estructura>");
                sbParameters.Append("<test:psNombre>WSDL_PAX</test:psNombre>");
                sbParameters.Append("<test:psContraseña>O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==" + "</test:psContraseña>");
                sbParameters.Append("<test:psVersion>3.2</test:psVersion>");

                sSoap = sSoap.Replace("@Datos", sbParameters.ToString());
             
                //Dictionary<string, string> dParametros = new Dictionary<string,string>();
                //dParametros.Add("psComprobante", document.InnerXml);
                //dParametros.Add("psTipoDocumento", "factura");
                //dParametros.Add("psId_Estructura", "0");
                //dParametros.Add("psNombre", "WSDL_PAX");
                //dParametros.Add("psContraseña", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==");
                //dParametros.Add("psVersion", "3.2");


                localCodigo = fnConsumirServicio("https://test.paxfacturacion.com.mx:453/webservices/wcfRecepcionasmx.asmx", "fnEnviarXML", sSoap);

                //SDRecepcionLocalASMX = Servicios.ProxyLocator.ObtenerServicioRecepcionLocal();
                //sResultado = SDRecepcionLocalASMX.fnEnviarXML(document.InnerXml, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");

                //SDRecepcionDesarrollo = Servicios.ProxyLocator.ObtenerServicioTimbradoDesarrollo();
                //sResultado = SDRecepcionDesarrollo.fnEnviarXML(document.InnerXml, "factura", 0, "abuelaprueba2", "U1tzbsKgXm/CqcKuwrMeK25wZT4ADcKwau++lxoW776577+077+S776+EO++qx0D77617766", "3.2");

                //SDRecepcionASMX = Servicios.ProxyLocator.ObtenerServicioRecepcion();
                //sResultado = SDRecepcionASMX.fnEnviarXML(document.InnerXml, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");

                SDRecepcionProduccion = Servicios.ProxyLocator.ObtenerServicioRecepcionProduccion();
                sResultado = SDRecepcionProduccion.fnEnviarXML(document.InnerXml, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");
            }
            catch (Exception ex)
            {

            } 
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public string fnConsumirServicio(string webServiceURL, string webMethod, string sMensaje)
        {
            string sResultado = string.Empty;
            UTF8Encoding encoding = new UTF8Encoding();
            try
            {
                WebRequest request = WebRequest.Create(webServiceURL);
                // Seteamos la propiedad Method del request a POST.
                request.Method = "POST";
                request.Proxy = null;
                // Creamos lo que se va a enviar por el metodo POST y lo convertimos a byte array.
                string postData = sMensaje;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Seteamos el ContentType del WebRequest a xml.
                request.ContentType = "text/xml";
                // Seteamos el ContentLength del WebRequest.
                request.ContentLength = byteArray.Length;
                // Obtenemos el request stream.
                Stream dataStream = request.GetRequestStream();
                // escribimos la data en el request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Cerramos el Stream object.
                dataStream.Close();
                //Obtiene la respuesta
                WebResponse response = null;
                string responseFromServer;
                StreamReader reader = null;

                try
                {
                    XmlDocument xDocument = new XmlDocument();

                    response = request.GetResponse();
                    reader = new StreamReader(response.GetResponseStream());
                    responseFromServer = string.Empty;
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                    response.Close();

                    xDocument.LoadXml(responseFromServer);

                }
                catch (WebException ex)
                {
                    response = ex.Response;
                    reader = new StreamReader(response.GetResponseStream());
                    responseFromServer = string.Empty;
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
            return sResultado;
        }

        private byte[] CreateHttpRequestData(Dictionary<string, string> dic)
        {
            StringBuilder _sbParameters = new StringBuilder();
            string sSoap = string.Empty;

            sSoap = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
                       xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                       xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                      <soap:Body>
                        <test:fnEnviarXML>
                            @Datos
                        </test:fnEnviarXML>
                      </soap:Body>
                    </soap:Envelope>";

            foreach (string param in dic.Keys)
            {
                _sbParameters.Append(param);//key => parameter name 
                _sbParameters.Append('=');
                _sbParameters.Append(dic[param]);//key value 
                _sbParameters.Append('&');
            }
            _sbParameters.Remove(_sbParameters.Length - 1, 1);

            UTF8Encoding encoding = new UTF8Encoding();

            return encoding.GetBytes(_sbParameters.ToString());

        }

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        /// <returns></returns>
        public HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://localhost:5249/ServiceRecepcion.svc");
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
    }
}
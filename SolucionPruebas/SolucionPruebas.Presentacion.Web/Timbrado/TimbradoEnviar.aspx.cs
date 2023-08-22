using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Timbrado
{
    public partial class TimbradoEnviar : System.Web.UI.Page
    {
        private Servicios.wsRecepcionTestASMX.wcfRecepcionASMXSoapClient SDRecepcionASMX;
        private Servicios.wsRecepcionDesarrollo.wcfRecepcionASMXSoapClient SDRecepcionDesarrollo;
        //private Servicios.RecepcionLocal.wcfRecepcionASMXSoapClient SDRecepcionLocalASMX;
        private Servicios.ServicioLocal.ServiceClient SDServicioLocal;
        private Servicios.wsRecepcionProduccion.wcfRecepcionASMXSoapClient SDRecepcionProduccion;
        private Servicios.wsRecepcionDP.wcfRecepcionASMXSoapClient SDRecepcionDP;

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

        private byte[] gbCertificado;
        /// <summary>
        /// Retorna o establece el certificado como arreglo de bytes
        /// </summary>
        public byte[] CertificadoBytes
        {
            get { return gbCertificado; }
            set { gbCertificado = value; }
        }

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

                string sCertificadoBase64 = document.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsm).Value;
                if (string.IsNullOrEmpty(sCertificadoBase64))
                {
                    throw new Exception("No se pudó recuperar el certificado del comprobante");
                }

                clsValCertificado(Convert.FromBase64String(sCertificadoBase64));

                SDServicioLocal = Servicios.ProxyLocator.ObtenerServicioLocal();
                sCadenaOriginal = SDServicioLocal.fnAplicarHojaTransformacion(document.InnerXml);

                sSello = SDServicioLocal.fnGenerarSelloRutas(Settings.Default.RutaPfx, sCadenaOriginal, Settings.Default.RutaLlave, Settings.Default.RutaPassword);
                sSello = sSello.Replace("\n", "");
                document.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsm).SetValue(sSello);

                //SDRecepcionLocalASMX = Servicios.ProxyLocator.ObtenerServicioRecepcionLocal();
                //sResultado = SDRecepcionLocalASMX.fnEnviarXML(document.InnerXml, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");

                //SDRecepcionDesarrollo = Servicios.ProxyLocator.ObtenerServicioTimbradoDesarrollo();
                //sResultado = SDRecepcionDesarrollo.fnEnviarXML(document.InnerXml, "factura", 0, "abuelaprueba2", "U1tzbsKgXm/CqcKuwrMeK25wZT4ADcKwau++lxoW776577+077+S776+EO++qx0D77617766", "3.2");

                SDRecepcionASMX = Servicios.ProxyLocator.ObtenerServicioTimbradoTest();
                sResultado = SDRecepcionASMX.fnEnviarXML(document.InnerXml, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");

                //SDRecepcionProduccion = Servicios.ProxyLocator.ObtenerServicioRecepcionProduccion();
                //sResultado = SDRecepcionProduccion.fnEnviarXML(document.InnerXml, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");

                //SDRecepcionDP = new Servicios.wcfRecepcionDP.wcfRecepcionASMXSoapClient();
                //sResultado = SDRecepcionDP.fnEnviarXML(document.InnerXml, "factura", 0, "ismael.hidalgo", "wrDCuMWFwrnCusOsw7fDtsOgxJXvv6Z6wqDCpD5Rw4nCqi9/CO++gO+/oO+/le++h++8ve+9mRs=", "3.2");

                txtResultado.Text = sResultado;
            }
            catch (Exception ex)
            {

            } 
        }

        /// <summary>
        /// Crea una nueva instancia de .NET tomando los datos del arreglo de bytes enviado
        /// </summary>
        /// <param name="pbCertificado">Arreglo de bytes que representan el archivo del certificado</param>
        public void clsValCertificado(byte[] pbCertificado)
        {
            try
            {
                certificado = new X509Certificate2(pbCertificado);
                gbCertificado = pbCertificado;
            }
            catch
            {
                try
                {
                    certificado = new X509Certificate2(fnDesencriptarCertificado(pbCertificado));
                    gbCertificado = fnDesencriptarCertificado(pbCertificado);
                }
                catch (Exception ex)
                {
                    throw new CryptographicException("El certificado esta bloqueado");
                }
            }

            if (certificado.Verify())
                throw new CryptographicException("El certificado no pasó la verificación");
        }

        /// <summary>
        /// Devuelve el arreglo de bytes del certificado encriptados
        /// </summary>
        /// <returns></returns>
        public byte[] fnEncriptarCertificado()
        {
            return Utilerias.Encriptacion.DES3.Encriptar(gbCertificado);
        }

        /// <summary>
        /// Devuelve el arreglo de bytes del certificado original
        /// </summary>
        /// <returns></returns>
        private byte[] fnDesencriptarCertificado(byte[] pbCertificadoEncriptado)
        {
            return Utilerias.Encriptacion.DES3.Desencriptar(pbCertificadoEncriptado);
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
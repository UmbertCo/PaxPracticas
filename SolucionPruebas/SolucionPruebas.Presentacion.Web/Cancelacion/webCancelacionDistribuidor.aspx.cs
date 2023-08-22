using SolucionPruebas.Presentacion.Servicios;
using SolucionPruebas.Presentacion.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace SolucionPruebas.Presentacion.Web.Cancelacion
{
    public partial class webCancelacionDistribuidor : System.Web.UI.Page
    {
        Servicios.ServicioLocal.ServiceClient wsServicioLocal = new Servicios.ServicioLocal.ServiceClient();

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCrearPfx_Click(object sender, EventArgs e)
        {
            string sCertificadoPEM = string.Empty;
            string sLlavePrivadaPEM = string.Empty;
            try
            {
                HttpFileCollection hfc = Request.Files;
                HttpPostedFile hpCertificadoPem = hfc[0];

                if (hpCertificadoPem.ContentLength < 0)
                    return;

                using (Stream stream = hpCertificadoPem.InputStream)
                {
                    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                    sCertificadoPEM = sr.ReadToEnd();
                }

                HttpPostedFile hpLlavePem = hfc[1];
                if (hpLlavePem.ContentLength < 0)
                    return;

                using (Stream stream = hpLlavePem.InputStream)
                {
                    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                    sLlavePrivadaPEM = sr.ReadToEnd();
                }


                bool bResultado = wsServicioLocal.fnGenerarPfxPem(Settings.Default.RutaPfx,
                                    sCertificadoPEM, sLlavePrivadaPEM, Settings.Default.Password, false);
            }
            catch (Exception ex)
            {
                txtResultado.Text = ex.Message;
            }
        }
        protected void btnCrearPfxRutas_Click(object sender, EventArgs e)
        {
            bool bResultado;
            try
            {
                bResultado = wsServicioLocal.fnGenerarPfxRuta(Settings.Default.RutaPfx, Settings.Default.RutaCertificado,
                    Settings.Default.RutaLlave, Settings.Default.Password, false);
            }
            catch (Exception ex)
            {
                txtResultado.Text = ex.Message;
            }
        }
        protected void btnCrearPfxBytes_Click(object sender, EventArgs e)
        {
            byte[] abCertificado;
            try
            {
                abCertificado = File.ReadAllBytes(Settings.Default.RutaCertificado);
                bool bResultado = wsServicioLocal.fnGenerarPfxBytes(Settings.Default.RutaPfx, abCertificado,
                    fnObtenerLlave(), Settings.Default.Password, false);
            }
            catch (Exception ex)
            {
                txtResultado.Text = ex.Message;
            }
        } 
        protected void btnCancelarComprobante_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            Servicios.wsCancelacionDistribuidor.ArrayOfString UUIDs = new Servicios.wsCancelacionDistribuidor.ArrayOfString();
            string sResultado = string.Empty;
            try
            {
                UUIDs.Add(("b145e18c-1501-42eb-9c61-ef79474f7f06").ToUpper());

                document = fnGenerarFirma("AAA010101AAA", "b145e18c-1501-42eb-9c61-ef79474f7f06");

                Servicios.wsCancelacionDistribuidor.wcfCancelaASMXSoapClient wsCancelacionDistribuidor = ProxyLocator.ObtenerServicioCancelacionDistribuidor();
                sResultado = wsCancelacionDistribuidor.fnCancelarXML(UUIDs, "AAA010101AAA", document.OuterXml, "WSDL_PAX", "wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=");

                txtResultado.Text = sResultado;
            }
            catch (Exception ex)
            {
                txtResultado.Text = ex.Message;
            }
        }
        protected void btnFirmar_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            Servicios.wsCancelacionDistribuidor.ArrayOfString UUIDs = new Servicios.wsCancelacionDistribuidor.ArrayOfString();
            string sResultado = string.Empty;
            try
            {
                UUIDs.Add(("b145e18c-1501-42eb-9c61-ef79474f7f06").ToUpper());

                //document = fnGenerarFirmaChilkat("AAA010101AAA", "b145e18c-1501-42eb-9c61-ef79474f7f06");
                document = fnGenerarFirmaChilkat("AAA010101AAA", "b145e18c-1501-42eb-9c61-ef79474f7f06");

                Servicios.wsCancelacionDistribuidor.wcfCancelaASMXSoapClient wsCancelacionDistribuidor = ProxyLocator.ObtenerServicioCancelacionDistribuidor();
                sResultado = wsCancelacionDistribuidor.fnCancelarXML(UUIDs, "AAA010101AAA", document.OuterXml, "WSDL_PAX", "wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=");

                txtResultado.Text = sResultado;
            }
            catch (Exception ex)
            {
                txtResultado.Text = ex.Message;
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            byte[] abPfx;
            string sResultado = string.Empty;
            XmlDocument document = new XmlDocument();
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            Servicios.wsCancelacionDistribuidor.ArrayOfString UUIDs = new Servicios.wsCancelacionDistribuidor.ArrayOfString();
            try
            {
                UUIDs.Add(("b145e18c-1501-42eb-9c61-ef79474f7f06").ToUpper());

                abPfx = wsServicioLocal.fnGenerarPfxRutasByte(Settings.Default.RutaPfx, Settings.Default.RutaCertificado,
                    Settings.Default.RutaLlave, Settings.Default.Password, false);

                document = fnGenerarFirmaChilkat("AAA010101AAA", "b145e18c-1501-42eb-9c61-ef79474f7f06", abPfx);

                Servicios.wsCancelacionDistribuidor.wcfCancelaASMXSoapClient wsCancelacionDistribuidor = ProxyLocator.ObtenerServicioCancelacionDistribuidor();
                sResultado = wsCancelacionDistribuidor.fnCancelarXML(UUIDs, "AAA010101AAA", document.OuterXml, "WSDL_PAX", "wqfCr8O3xLfEhMOHw4nEjMSrxJnvv7bvvr4cVcKuKkBEM++/ke+/gCPvv4nvvrfvvaDvvb/vvqTvvoA=");

                txtResultado.Text = sResultado;
            }
            catch (Exception ex)
            {
                txtResultado.Text = ex.Message;
            }
        }

        private XmlDocument fnGenerarFirma(string psRFC, string psUUID)
        {
            byte[] pfxBlob;
            XmlDocument xmlResultado = new XmlDocument();
            XmlDocument xmlPrefirma = new XmlDocument();
            X509Certificate2 certEmisor = new X509Certificate2();
            try
            {
                xmlResultado.PreserveWhitespace = false;
                xmlPrefirma.PreserveWhitespace = false;

                // Se genera la pre-firma 
                XmlElement xeCancelacion = xmlPrefirma.CreateElement("Cancelacion");
                xeCancelacion.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeCancelacion.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                xeCancelacion.SetAttribute("xmlns", "http://cancelacfd.sat.gob.mx");

                XmlAttribute atxmlrfc = xmlPrefirma.CreateAttribute("RfcEmisor");
                atxmlrfc.Value = psRFC;
                xeCancelacion.SetAttributeNode(atxmlrfc);

                XmlAttribute atxmlfecha = xmlPrefirma.CreateAttribute("Fecha");
                atxmlfecha.Value = Convert.ToDateTime("2014-04-04T11:19:29").ToString("s");
                //DateTime.Now.ToString("s");
                xeCancelacion.SetAttributeNode(atxmlfecha);

                XmlElement xeFolios = xmlPrefirma.CreateElement("Folios");
                XmlElement xeUUID = xmlPrefirma.CreateElement("UUID");
                xeUUID.InnerText = psUUID.ToUpper();

                xeFolios.AppendChild(xeUUID);
                xeCancelacion.AppendChild(xeFolios);
                xmlPrefirma.AppendChild(xeCancelacion);

                // Se lee la pfx generada
                pfxBlob = File.ReadAllBytes(Settings.Default.RutaArchivoPfx);

                //Se genera el xml firmado
                fnFirmarXml(xmlPrefirma, pfxBlob, Settings.Default.Password);

                // Se va formando el mensaje completo
                XmlElement xeEnvelope = xmlResultado.CreateElement("s:Envelope", "http://schemas.xmlsoap.org/soap/envelope/");

                XmlElement xeBody = xmlResultado.CreateElement("s:Body", "http://schemas.xmlsoap.org/soap/envelope/");
                xeBody.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeBody.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");

                XmlElement xeCancela = xmlResultado.CreateElement("CancelaCFD");
                xeCancela.SetAttribute("xmlns", "http://cancelacfd.sat.gob.mx");

                //xeCancela.InnerXml = xmlPrefirma.OuterXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "").Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace("xmlns=\"http://cancelacfd.sat.gob.mx\"", "").Replace("<?xml version=\"1.0\"?>", "");
                xeCancela.InnerXml = xmlPrefirma.OuterXml;

                xeBody.AppendChild(xeCancela);
                xeEnvelope.AppendChild(xeBody);
                xmlResultado.AppendChild(xeEnvelope);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return xmlResultado;
        }

        private XmlDocument fnGenerarFirmaChilkat(string psRFC, string psUUID)
        {
            Chilkat.Cert oCert = new Chilkat.Cert();
            string sRFC = string.Empty;
            string sUUID = string.Empty;
            XmlDocument xmlResultado = new XmlDocument();
            XmlDocument xmlPrefirma = new XmlDocument();
            try
            {
                sRFC = "AAA010101AAA";
                sUUID = "b145e18c-1501-42eb-9c61-ef79474f7f06";

                xmlResultado.PreserveWhitespace = false;
                xmlPrefirma.PreserveWhitespace = false;

                // Se genera la pre-firma 
                XmlElement xeCancelacion = xmlPrefirma.CreateElement("Cancelacion");
                xeCancelacion.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeCancelacion.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                xeCancelacion.SetAttribute("xmlns", "http://cancelacfd.sat.gob.mx");

                XmlAttribute atxmlrfc = xmlPrefirma.CreateAttribute("RfcEmisor");
                atxmlrfc.Value = sRFC;
                xeCancelacion.SetAttributeNode(atxmlrfc);

                XmlAttribute atxmlfecha = xmlPrefirma.CreateAttribute("Fecha");
                atxmlfecha.Value = Convert.ToDateTime("2014-04-04T11:19:29").ToString("s");
                //DateTime.Now.ToString("s");
                xeCancelacion.SetAttributeNode(atxmlfecha);

                XmlElement xeFolios = xmlPrefirma.CreateElement("Folios");
                XmlElement xeUUID = xmlPrefirma.CreateElement("UUID");
                xeUUID.InnerText = sUUID.ToUpper();

                xeFolios.AppendChild(xeUUID);
                xeCancelacion.AppendChild(xeFolios);
                xmlPrefirma.AppendChild(xeCancelacion);

                // Se lee la pfx generada
                oCert.LoadPfxFile(@"C:\ConectorPAXMYERS\XML\Certificados\RutaPfx\pem.pfx", Settings.Default.Password);
                //oCert.LoadPfxFile(Settings.Default.RutaArchivoPfx, Settings.Default.Password);

                //Se genera el xml firmado
                fnFirmarXml(xmlPrefirma, Settings.Default.Password, oCert);

                // Se va formando el mensaje completo
                XmlElement xeEnvelope = xmlResultado.CreateElement("s:Envelope", "http://schemas.xmlsoap.org/soap/envelope/");

                XmlElement xeBody = xmlResultado.CreateElement("s:Body", "http://schemas.xmlsoap.org/soap/envelope/");
                xeBody.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeBody.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");

                XmlElement xeCancela = xmlResultado.CreateElement("CancelaCFD");
                xeCancela.SetAttribute("xmlns", "http://cancelacfd.sat.gob.mx");

                //xeCancela.InnerXml = xmlPrefirma.OuterXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "").Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace("xmlns=\"http://cancelacfd.sat.gob.mx\"", "").Replace("<?xml version=\"1.0\"?>", "");
                xeCancela.InnerXml = xmlPrefirma.OuterXml;

                xeBody.AppendChild(xeCancela);
                xeEnvelope.AppendChild(xeBody);
                xmlResultado.AppendChild(xeEnvelope);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return xmlResultado;
        }

        private XmlDocument fnGenerarFirmaChilkat2(string psRFC, string psUUID)
        {
            XmlDocument xmlResultado = new XmlDocument();
            XmlDocument xmlPrefirma = new XmlDocument();
            X509Certificate2 certEmisor;
            try
            {
                xmlResultado.PreserveWhitespace = false;
                xmlPrefirma.PreserveWhitespace = false;

                // Se genera la pre-firma 
                XmlElement xeCancelacion = xmlPrefirma.CreateElement("Cancelacion");
                xeCancelacion.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeCancelacion.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                xeCancelacion.SetAttribute("xmlns", "http://cancelacfd.sat.gob.mx");

                XmlAttribute atxmlrfc = xmlPrefirma.CreateAttribute("RfcEmisor");
                atxmlrfc.Value = psRFC;
                xeCancelacion.SetAttributeNode(atxmlrfc);

                XmlAttribute atxmlfecha = xmlPrefirma.CreateAttribute("Fecha");
                atxmlfecha.Value = Convert.ToDateTime("2014-04-04T11:19:29").ToString("s");
                //DateTime.Now.ToString("s");
                xeCancelacion.SetAttributeNode(atxmlfecha);

                XmlElement xeFolios = xmlPrefirma.CreateElement("Folios");
                XmlElement xeUUID = xmlPrefirma.CreateElement("UUID");
                xeUUID.InnerText = psUUID.ToUpper();

                xeFolios.AppendChild(xeUUID);
                xeCancelacion.AppendChild(xeFolios);
                xmlPrefirma.AppendChild(xeCancelacion);

                // Se lee la pfx generada
                //pfxBlob = File.ReadAllBytes(Settings.Default.RutaArchivoPfx);
                certEmisor = new X509Certificate2(@"C:\ConectorPAXMYERS\XML\Certificados\RutaPfx\pem.pfx", Settings.Default.Password);

                //Se genera el xml firmado
                fnFirmarXml(xmlPrefirma, Settings.Default.Password, certEmisor);

                // Se va formando el mensaje completo
                XmlElement xeEnvelope = xmlResultado.CreateElement("s:Envelope", "http://schemas.xmlsoap.org/soap/envelope/");

                XmlElement xeBody = xmlResultado.CreateElement("s:Body", "http://schemas.xmlsoap.org/soap/envelope/");
                xeBody.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeBody.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");

                XmlElement xeCancela = xmlResultado.CreateElement("CancelaCFD");
                xeCancela.SetAttribute("xmlns", "http://cancelacfd.sat.gob.mx");

                //xeCancela.InnerXml = xmlPrefirma.OuterXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "").Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace("xmlns=\"http://cancelacfd.sat.gob.mx\"", "").Replace("<?xml version=\"1.0\"?>", "");
                xeCancela.InnerXml = xmlPrefirma.OuterXml;

                xeBody.AppendChild(xeCancela);
                xeEnvelope.AppendChild(xeBody);
                xmlResultado.AppendChild(xeEnvelope);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return xmlResultado;
        }

        private XmlDocument fnGenerarFirmaChilkat(string psRFC, string psUUID, byte[] paPfx)
        {
            XmlDocument xmlResultado = new XmlDocument();
            XmlDocument xmlPrefirma = new XmlDocument();
            X509Certificate2 certEmisor;
            try
            {
                xmlResultado.PreserveWhitespace = false;
                xmlPrefirma.PreserveWhitespace = false;

                // Se genera la pre-firma 
                XmlElement xeCancelacion = xmlPrefirma.CreateElement("Cancelacion");
                xeCancelacion.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeCancelacion.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                xeCancelacion.SetAttribute("xmlns", "http://cancelacfd.sat.gob.mx");

                XmlAttribute atxmlrfc = xmlPrefirma.CreateAttribute("RfcEmisor");
                atxmlrfc.Value = psRFC;
                xeCancelacion.SetAttributeNode(atxmlrfc);

                XmlAttribute atxmlfecha = xmlPrefirma.CreateAttribute("Fecha");
                atxmlfecha.Value = Convert.ToDateTime("2014-04-04T11:19:29").ToString("s");
                //DateTime.Now.ToString("s");
                xeCancelacion.SetAttributeNode(atxmlfecha);

                XmlElement xeFolios = xmlPrefirma.CreateElement("Folios");
                XmlElement xeUUID = xmlPrefirma.CreateElement("UUID");
                xeUUID.InnerText = psUUID.ToUpper();

                xeFolios.AppendChild(xeUUID);
                xeCancelacion.AppendChild(xeFolios);
                xmlPrefirma.AppendChild(xeCancelacion);

                // Se lee la pfx generada
                certEmisor = new X509Certificate2(paPfx, Settings.Default.Password);

                //Se genera el xml firmado
                fnFirmarXml(xmlPrefirma, Settings.Default.Password, certEmisor);

                // Se va formando el mensaje completo
                XmlElement xeEnvelope = xmlResultado.CreateElement("s:Envelope", "http://schemas.xmlsoap.org/soap/envelope/");

                XmlElement xeBody = xmlResultado.CreateElement("s:Body", "http://schemas.xmlsoap.org/soap/envelope/");
                xeBody.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xeBody.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");

                XmlElement xeCancela = xmlResultado.CreateElement("CancelaCFD");
                xeCancela.SetAttribute("xmlns", "http://cancelacfd.sat.gob.mx");

                //xeCancela.InnerXml = xmlPrefirma.OuterXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "").Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace("xmlns=\"http://cancelacfd.sat.gob.mx\"", "").Replace("<?xml version=\"1.0\"?>", "");
                xeCancela.InnerXml = xmlPrefirma.OuterXml;

                xeBody.AppendChild(xeCancela);
                xeEnvelope.AppendChild(xeBody);
                xmlResultado.AppendChild(xeEnvelope);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return xmlResultado;
        }

        private void fnFirmarXml(XmlDocument xmlDocumento, byte[] psLlave, string psPassword)
        {
            try
            {
                X509Certificate2 certificado = new X509Certificate2(psLlave, psPassword);

                RSACryptoServiceProvider rsKey = new RSACryptoServiceProvider();
                rsKey = (RSACryptoServiceProvider)certificado.PrivateKey;

                SignedXml xmlFirmado = new SignedXml(xmlDocumento);
                xmlFirmado.SigningKey = rsKey;

                Reference referencia = new Reference();
                referencia.Uri = string.Empty;

                KeyInfoX509Data DatosEmisor = new KeyInfoX509Data(certificado);
                DatosEmisor.AddIssuerSerial(certificado.Issuer, certificado.SerialNumber);

                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(DatosEmisor);

                xmlFirmado.KeyInfo = keyInfo;

                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                referencia.AddTransform(env);

                xmlFirmado.AddReference(referencia);
                xmlFirmado.ComputeSignature();
 
                xmlDocumento.DocumentElement.AppendChild(xmlDocumento.ImportNode(xmlFirmado.GetXml(), true));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void fnFirmarXml(XmlDocument xmlDocumento, string psPassword, Chilkat.Cert oCert)
        {           
            string sKeyXml = string.Empty;
            try
            {
                X509Certificate2 certificado = new X509Certificate2(System.Text.Encoding.UTF8.GetBytes(oCert.ExportCertPem()), psPassword);

                sKeyXml = oCert.ExportPrivateKey().GetXml();
                
                RSACryptoServiceProvider rsKey = new RSACryptoServiceProvider();
                rsKey.FromXmlString(sKeyXml);

                SignedXml xmlFirmado = new SignedXml(xmlDocumento);
                xmlFirmado.SigningKey = rsKey;

                Reference referencia = new Reference();
                referencia.Uri = string.Empty;

                KeyInfoX509Data DatosEmisor = new KeyInfoX509Data(certificado);
                
                //DatosEmisor.AddIssuerSerial(certificado.Issuer, certificado.SerialNumber);
                DatosEmisor.AddIssuerSerial(certificado.Issuer, certificado.SerialNumber);

                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(DatosEmisor);

                xmlFirmado.KeyInfo = keyInfo;

                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                referencia.AddTransform(env);

                xmlFirmado.AddReference(referencia);
                xmlFirmado.ComputeSignature();

                xmlDocumento.DocumentElement.AppendChild(xmlDocumento.ImportNode(xmlFirmado.GetXml(), true));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void fnFirmarXml(XmlDocument xmlDocumento, string psPassword, X509Certificate2 certificado)
        {
            try
            {
                if (xmlDocumento == null)
                    throw new ArgumentException("El argumento Doc no puede ser null.");
                if (certificado == null)
                    throw new ArgumentNullException("Key");

                SignedXml xmlFirmado = new SignedXml(xmlDocumento);

                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)certificado.PrivateKey;
                xmlFirmado.SigningKey = rsa;

                Reference referencia = new Reference();
                referencia.Uri = "";
                RSAKeyValue rsakey = new RSAKeyValue((RSA)rsa);
                KeyInfo keyInfo = new KeyInfo();
                KeyInfoX509Data kdata = new KeyInfoX509Data(certificado);
                X509IssuerSerial xserial;
                xserial.IssuerName = certificado.Issuer.ToString();
                xserial.SerialNumber = certificado.SerialNumber;
                kdata.AddIssuerSerial(xserial.IssuerName, xserial.SerialNumber);
                keyInfo.AddClause(kdata);
                xmlFirmado.KeyInfo = keyInfo;
                XmlDsigEnvelopedSignatureTransform transformacionENV = new XmlDsigEnvelopedSignatureTransform();
                referencia.AddTransform(transformacionENV);
                xmlFirmado.AddReference(referencia);
                xmlFirmado.ComputeSignature();
                XmlDocument xmlD = new XmlDocument();
                XmlElement firmaDigitalXML = xmlFirmado.GetXml();
                xmlDocumento.DocumentElement.AppendChild(xmlDocumento.ImportNode(firmaDigitalXML, true));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private RSACryptoServiceProvider fnObtenerLlavePublicaDex509Certificate(X509Certificate x509)
        {
            RSACryptoServiceProvider rsaCSDP = null;
            uint hProv = 0;
            IntPtr pLlavePubicaBlob = IntPtr.Zero;
            IntPtr pContextoCertificado = IntPtr.Zero;
            try
            { 
                //pContextoCertificado = (IntPtr)Crypto32
            }
            catch (Exception ex)
            { 
            
            }
            return rsaCSDP;
        }

        private byte[] fnObtenerLlave()
        {
            byte[] bLlavePrivada = null;

            Stream streamkey = File.Open(Settings.Default.RutaLlave, FileMode.Open);
            StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
            using (BinaryReader br = new BinaryReader(streamkey))
            {
                bLlavePrivada = br.ReadBytes(Convert.ToInt32(streamkey.Length));
            }

            return bLlavePrivada;
        }

        private void fnCrearPfx(string psRutaPfx, string psNombreCertificado, string psNombreLlave, string psPassword) 
        {
            string sInstruccion = string.Empty;
            Process proceso;
            ProcessStartInfo infoSello;
            try
            {
                //CryptosSys
                sInstruccion = "pkcs12 -export -out " + psRutaPfx + psNombreCertificado + ".pfx -inkey "
                    + psRutaPfx + psNombreLlave + ".pem -in "
                    + psRutaPfx + psNombreCertificado + ".pem -passout pass:" + psPassword;


                //sInstruccion = "pkcs12 -export -in " + psRutaPfx + psNombreCertificado + ".pem -inkey "
                //    + psRutaPfx + psNombreLlave + ".pem -passin pass:" + psPassword 
                //    + " -out " + psRutaPfx + psNombreCertificado + ".pfx";

                //ImaginaNet
                //sInstruccion = "pkcs12 -inkey " + psRutaPfx + psNombreLlave + ".pem -in " +
                //    psRutaPfx + psNombreCertificado + ".pem -export -out " +
                //    psRutaPfx + psNombreCertificado + ".pfx";

                //Samuel Ventura
                ////sInstruccion = "pkcs12 -in " + psRutaPfx + psNombreCertificado + ".pem -inkey " +
                ////    psRutaPfx + psNombreLlave + ".pem -export -out " +
                ////    psRutaPfx + psNombreCertificado;

                infoSello = new ProcessStartInfo(Settings.Default.RutaOpenSSL, sInstruccion);
                infoSello.CreateNoWindow = true;
                infoSello.UseShellExecute = false;
                proceso = Process.Start(infoSello);
                proceso.WaitForExit();

                if (proceso.ExitCode != 0) throw new Exception("No se pudo crear el archivo pfx: " + psRutaPfx + psNombreCertificado + ".pfx");
                proceso.Dispose();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }  
    }
}

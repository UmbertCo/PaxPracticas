using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmSellarFactura : Form
    {
        public frmSellarFactura()
        {
            InitializeComponent();
        }

        private void frmSellarFactura_Load(object sender, EventArgs e)
        {

        }

        private void btnSellarFactura_Click(object sender, EventArgs e)
        {
            try
            {
                string sComprobante = File.ReadAllText(@"D:\PAXRegeneracionBateria\Archivos Generados\Timbre_Previo.xml");

                txtResultado.Text = fnFirmar(sComprobante);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string fnFirmar(string psComprobante)
        {
            string sResultado = string.Empty;
            XmlDocument xdComprobante = new XmlDocument();
            XmlDocument xdFirma = new XmlDocument();
            try
            {
                byte[] abLlavePem = File.ReadAllBytes(@"D:\Mis Documentos\pfxpac.pfx");

                X509Certificate2 certificate = new X509Certificate2(abLlavePem, "12345678a");
                RSACryptoServiceProvider Key = (RSACryptoServiceProvider)certificate.PrivateKey;

                xdComprobante.LoadXml(psComprobante);

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdComprobante.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                XPathNavigator navDocGenera = xdComprobante.CreateNavigator();

                string UUID = navDocGenera.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;

                string sPrefirma = fnPrefirma(UUID.ToLower(), "20001000000100005761", "201 - UUID Entregado al SAT");
                xdFirma.LoadXml(sPrefirma);
                //fnSellarFactura(xdFirma, Key);

                HMACSHA512 hmac = new HMACSHA512(abLlavePem);

                fnSellarFactura(xdFirma, hmac);

                sResultado = xdFirma.InnerXml;
                sResultado = sResultado.Replace("<KeyName xmlns=\"\">", "<KeyValue>");

                bool res = VerifyXmlFile(sResultado, hmac);

                //bool bVerificarSello = VerifyXmlFile(sResultado);

                if (res)
                {
                    MessageBox.Show("El sello XML es valido");
                }
                else
                {
                    MessageBox.Show("El sello XML no es valido");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al genera la firma: " + ex.Message);
            }
            return sResultado;
        }

        private string fnPrefirma(string psUUID, string psNoCertificado, string psDescripcionEstatus)
        {
            string sResultado = string.Empty;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Acuse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Fecha=\"" + DateTime.Now.ToString("s") + "\" NoCertificadoSAT=\"" + psNoCertificado  + "\" UUID=\"" + psUUID + "\" CodEstatus=\"" + psDescripcionEstatus + "\">");
                sb.Append("</Acuse>");

                sResultado = sb.ToString();
            }
            catch (Exception ex)
            { 
                throw new Exception("Error al generar la prefirma: " + ex.Message);
            }
            return sResultado;
        }

        private void fnSellarFactura(XmlDocument psPrefirma, RSA pLlave)
        {
            try
            { 
                SignedXml sxComprobanteFirmado = new SignedXml(psPrefirma);
                sxComprobanteFirmado.SigningKey = pLlave;

                Signature sSello = sxComprobanteFirmado.Signature;
                sSello.Id = "SelloSAT";
                Reference rReferencia = new Reference("");              
                
                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                rReferencia.AddTransform(env);

                sSello.SignedInfo.AddReference(rReferencia);

                KeyInfo kiInformacionLlave = new KeyInfo();
                kiInformacionLlave.AddClause(new RSAKeyValue((RSA)pLlave));
                                
                sSello.KeyInfo = kiInformacionLlave;
                
                sxComprobanteFirmado.ComputeSignature();

                XmlElement xFirmado = sxComprobanteFirmado.GetXml();

                psPrefirma.DocumentElement.AppendChild(psPrefirma.ImportNode(xFirmado, true));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al sellar la factura: " + ex.Message);
            }
        }

        private void fnSellarFactura(XmlDocument pdPrefirma, KeyedHashAlgorithm paLlave)
        {
            try
            {
                SignedXml sxComprobanteFirmado = new SignedXml(pdPrefirma);
                
                Reference rReferencia = new Reference("");
                rReferencia.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha512";

                Signature sSello = sxComprobanteFirmado.Signature;
                sSello.Id = "SelloSAT"; 
                
                rReferencia.AddTransform(CreateXPathTransform("not(ancestor-or-self::*[local-name()='Signature'])"));

                sSello.SignedInfo.AddReference(rReferencia);               
                
                sxComprobanteFirmado.KeyInfo.LoadXml(fnCargarKeyInfo());

                sxComprobanteFirmado.ComputeSignature(paLlave);

                XmlElement xFirmado = sxComprobanteFirmado.GetXml();

                pdPrefirma.DocumentElement.AppendChild(pdPrefirma.ImportNode(xFirmado, true));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al sellar la factura: " + ex.Message);
            }
        }

        private void fnSellarFactura()
        {
            try
            { 
            
            }
            catch (Exception ex)
            { 
            
            }
        }

        public static Boolean VerifyXmlFile(String Name)
        {
            // Check the arguments.   
            if (Name == null)
                throw new ArgumentNullException("Name");

            // Create a new XML document.
            XmlDocument xmlDocument = new XmlDocument();

            // Format using white spaces.
            xmlDocument.PreserveWhitespace = true;

            // Load the passed XML file into the document. 
            xmlDocument.LoadXml(Name);

            // Create a new SignedXml object and pass it 
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDocument);

            // Find the "Signature" node and create a new 
            // XmlNodeList object.
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

            // Load the signature node.
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result. 
            return signedXml.CheckSignature();
        }

        public static Boolean VerifyXmlFile(String Name, KeyedHashAlgorithm Key)
        {
            // Check the arguments.   
            if (Name == null)
                throw new ArgumentNullException("Name");

            // Create a new XML document.
            XmlDocument xmlDocument = new XmlDocument();

            // Format using white spaces.
            xmlDocument.PreserveWhitespace = true;

            // Load the passed XML file into the document. 
            xmlDocument.LoadXml(Name);

            // Create a new SignedXml object and pass it 
            // the XML document class.
            SignedXml signedXml = new SignedXml(xmlDocument);

            // Find the "Signature" node and create a new 
            // XmlNodeList object.
            XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

            // Load the signature node.
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result. 
            return signedXml.CheckSignature(Key);
        }

        private XmlNodeList LoadTransformByXml()
        {
            XmlDocument xmlDoc = new XmlDocument();

            string transformXml = "<Transforms>";
            transformXml += "<Transform Algorithm=\"http://www.w3.org/TR/1999/REC-xpath-19991116\">";
            transformXml += "<XPath>not(ancestor-or-self::*[local-name()='Signature'])";
            transformXml += "</XPath></Transform>";
            transformXml += "</Transforms>";

            xmlDoc.LoadXml(transformXml);

            return xmlDoc.ChildNodes;
            //return xmlDoc;
        }

        private XmlElement fnContexto()
        {
            XmlDocument xmlDoc = new XmlDocument();

            string transformXml = "<XPath>";
            transformXml += "not(ancestor-or-self::*[local-name()='Signature'])";
            transformXml += "</XPath>";

            xmlDoc.LoadXml(transformXml);

            return xmlDoc.DocumentElement;
        }

        private XmlElement fnCargarKeyInfo()
        {
            XmlDocument xdLlave = new XmlDocument();
            XmlElement xeResultado; 
            try 
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<KeyInfo>");
                sb.Append("<KeyName>00001088888810000001</KeyName>");
                sb.Append("<KeyValue>");
                sb.Append("<RSAKeyValue>");
                sb.Append("<Modulus>vAr6QLmcvW6auTg7a+Ogm0veNvqJ30rD3j0iSAHxGzGVrg1d0xl0Fj5l+JX9EivD+qhkSY7pfLnJoObLpQ3GGZZOOihJVS2tbJDmnn9TW8fKUOVg+jGhcnpCHaUPq/Poj8I2OVb3g7hiaREORm6tLtzOIjkOv9INXxIpRMx54cw46D5F1+0M7ECEVO8Jg+3yoI6OvDNBH+jABsj7SutmSnL1Tov/omIlSWausdbXqykcl10BLu2XiQAc6KLnl0+Ntzxoxk+dPUSdRyR7f3Vls6yUlK/+C/4FacbR+fszT0XIaJNWkHaTOoqz76Ax9XgTv9UuT67j7rdTVzTvAN363w==</Modulus>");
                sb.Append("<Exponent>AQAB</Exponent>");
                sb.Append("</RSAKeyValue>");
                sb.Append("</KeyValue>");
                sb.Append("</KeyInfo>");

                xdLlave.LoadXml(sb.ToString());
                

                xeResultado = xdLlave.DocumentElement;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar KeyInfo: " + ex.Message);
            }
            return xeResultado;
        }

        private static XmlDsigXPathTransform CreateXPathTransform(string xpath)
        {
            // create the XML that represents the transform
            XmlDocument doc = new XmlDocument();
            XmlElement xpathElem = doc.CreateElement("XPath");
            xpathElem.InnerText = xpath;

            XmlDsigXPathTransform xform = new XmlDsigXPathTransform();
            xform.LoadInnerXml(xpathElem.SelectNodes("."));

            return xform;
        } 
    }
}

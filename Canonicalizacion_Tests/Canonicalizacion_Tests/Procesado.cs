using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml.Xsl;
using System.Globalization;
using System.Threading;

namespace Canonicalizacion_Tests
{
    class Procesado
    {
        public String _XML = string.Empty;
        public String _alias = string.Empty;
        public byte[] _PFX = null;
        public String _tipo = string.Empty;
        public String _password = string.Empty;
        public String _TRANSFORM = string.Empty;

        public Procesado(byte[] PFX, String XML, String tipo, String password, String XPATH)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            _PFX = PFX;
            _XML = XML;
            _tipo = tipo;
            _password = password;
            _TRANSFORM = XPATH;
        }

        public XmlDocument Procesar()
        {
            try
            {
                String xmlString = _XML;
                byte[] bInput = System.Text.Encoding.UTF8.GetBytes(xmlString);

                Canonicalizacion c14n11 = new Canonicalizacion();
                byte[] bInputCanonicalized = c14n11.canonicalize(bInput);

                XmlDocument xdDocFirmar = new XmlDocument();
                xdDocFirmar.PreserveWhitespace = true;
                string PRUEBA = Encoding.UTF8.GetString(bInputCanonicalized);
                xdDocFirmar.LoadXml(Encoding.UTF8.GetString(bInputCanonicalized));

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdDocFirmar.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("xmlns:ds", "http://www.w3.org/2000/09/xmldsig#");
                nsmComprobante.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
                nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                //XsltContext xcNombres = new XsltContext();

                //CARGAR PFX PARA FIRMA
                X509Certificate2 x509 = new X509Certificate2(_PFX, _password);


                //OBJETO DE FIRMA XML
                SignedXml signedXml = new SignedXml(xdDocFirmar);
                signedXml.Signature.Id = _tipo;
                signedXml.SigningKey = x509.PrivateKey;
                

                //CREAR REFERENCIA
                Reference reference = new Reference();
                reference.Uri = "";

                // CREAR TRANSFORMACION
                //Security.Cryptography.Xml.XmlDsigXPathWithNamespacesTransform
                XmlDsigXPathTransform XPathTransform = CreateXPathTransform(_TRANSFORM);
                //EVALUAR XPATH

                //XmlElement xeXPATHResult = (XmlElement)xdDocFirmar.SelectSingleNode(_TRANSFORM);

                XPathTransform.Algorithm = "http://www.w3.org/TR/1999/REC-xpath-19991116";
                XPathTransform.Context = xdDocFirmar.DocumentElement;
                reference.AddTransform(XPathTransform);
                reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";

                //INTENTAR AGREGAR EL ALGORITMO DE CANONICALIZACION 1.1
                CryptoConfig.AddAlgorithm(typeof(XmlDsigC14N11Transform), "http://www.w3.org/2006/12/xml-c14n11");

                //COLOCAR EL ALGORITMO DE CANONICALIZACION
                signedXml.SignedInfo.CanonicalizationMethod = Canonicalizacion.ALGO_ID_C14N11_OMIT_COMMENTS;
                signedXml.SignedInfo.SignatureMethod = Constants.ALGO_ID_SIGNATURE_RSA_SHA1;


                //KEYINFO
                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(x509));

                KeyInfoName kinNumero = new KeyInfoName();
                kinNumero.Value = x509.SerialNumber;
                

                signedXml.KeyInfo = keyInfo;

                // Add the reference to the SignedXml object.
                signedXml.AddReference(reference);

                //Sign
                signedXml.ComputeSignature();

                XmlElement xmlDigitalSignature = signedXml.GetXml();
                xdDocFirmar.DocumentElement.AppendChild(xmlDigitalSignature);

                return xdDocFirmar;
               
            }
            catch (Exception ex)
            {

                throw new Exception("Hubo un error: " + ex);
            }
        }

        private static XmlDsigXPathTransform CreateXPathTransform(string XPathString)
        {
            // Create a new XMLDocument object.
            XmlDocument doc = new XmlDocument();

            // Create a new XmlElement.
            XmlElement xPathElem = doc.CreateElement("XPath");

            // Set the element text to the value
            // of the XPath string.
            xPathElem.InnerText = XPathString;

            // Create a new XmlDsigXPathTransform object.
            XmlDsigXPathTransform xForm = new XmlDsigXPathTransform();

            // Load the XPath XML from the element. 
            xForm.LoadInnerXml(xPathElem.SelectNodes("."));

            // Return the XML that represents the transform.
            return xForm;
        }

    }

    public class XmlDsigC14N11Transform : XmlDsigC14NTransform
    {
        public override void LoadInput(object obj)
        { 
            base.LoadInput(obj); 
        }
        public override object GetOutput()
        {
            return base.GetOutput();
        }
    }
}

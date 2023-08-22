using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using EntregaPendientes.Properties;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Security.Cryptography;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace EntregaPendientes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dFechaComprobante;
            string sCadenaOriginalEmisor = string.Empty;
            string sCadenaOriginal = string.Empty;
            string HASHEmisor = string.Empty;
            string HASHTimbreFiscal = string.Empty;
            System.Diagnostics.EventLog eventLogExport = new System.Diagnostics.EventLog();

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

            string tipoDocumento = Settings.Default.tipoDocumento;
            int estructura = Convert.ToInt16(Settings.Default.estructura);
            wfcRecepcionFacturas.ServiceNadroSoapClient enviar = new wfcRecepcionFacturas.ServiceNadroSoapClient();

            var listaArchivos = RecuperaListaArchivos(Settings.Default.directorioCfdi);
            foreach (string nombreArchivo in listaArchivos)
            {
                var contenidoArchivo = RecuperaArchivo(nombreArchivo);
                byte[] contenidoArchivoBytes = RecuperaArchivoByte(contenidoArchivo, nombreArchivo);

                contenidoArchivo.Close();

                XmlDocument sXmlDocument = new XmlDocument();
                sXmlDocument.Load(nombreArchivo);

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                dFechaComprobante = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;

                XPathNavigator navNodoTimbre = sXmlDocument.CreateNavigator();

                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                // Hash Emisor
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOri.V32));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(navNodoTimbre, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sCadenaOriginalEmisor = srDll.ReadToEnd();
                HASHEmisor = GetHASH(sCadenaOriginalEmisor).ToUpper();

                XmlReader reader = XmlReader.Create(new StringReader(sXmlDocument.InnerXml));
                XElement root = XElement.Load(reader);
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(sXmlDocument.NameTable);
                namespaceManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                XElement childTimbre = root.XPathSelectElement("./cfdi:Complemento", namespaceManager);

                // Hash Timbre
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(Timbrado.V3.TFDXSLT));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(childTimbre.CreateNavigator(), args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sCadenaOriginal = srDll.ReadToEnd();
                HASHTimbreFiscal = GetHASH(sCadenaOriginal).ToUpper();

                if (enviar.fnRecibeFacturas(EncodeTo64(txtUsuario.Text), txtPassword.Text, HASHEmisor, HASHTimbreFiscal, EncodeTo64(sXmlDocument.OuterXml), tipoDocumento, estructura, dFechaComprobante.ToString("s")))
                {
                    File.Delete(nombreArchivo);
                }
                else
                {
                    eventLogExport.WriteEntry("Error del Archivo ENTREGA EXPRESS.");
                }
            }
        }

        public static string GetHASH(string text)
        {
            byte[] hashValue;
            byte[] message = Encoding.UTF8.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            } return hex;
        }

        public static IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos; 
        }

        public static Stream RecuperaArchivo(string rutaAbsoluta)
        {
            return File.OpenRead(rutaAbsoluta);
        }

        public static byte[] RecuperaArchivoByte(Stream rutaAbsoluta, string ruta)
        {
            StreamReader sr = new StreamReader(rutaAbsoluta);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
       
            return encoding.GetBytes(sr.ReadToEnd());

            
        }

        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}

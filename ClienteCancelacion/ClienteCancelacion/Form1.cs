using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using System.Security.Cryptography;
using System.IO;
using ClienteCancelacion.Properties;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace ClienteCancelacion
{
    public partial class Form1 : Form
    {
        ArrayList list = new ArrayList();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerarUUID_Click(object sender, EventArgs e)
        {
            txtUUID.Text = Guid.NewGuid().ToString();
        }

        private void btnLlave_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void btnCertificado_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            lstUUID.Items.Clear();
            list.Clear();
        }

        private void btnAgregarUUID_Click(object sender, EventArgs e)
        {
            lstUUID.Items.Add(txtUUID.Text.ToUpper());
            list.Add(txtUUID.Text.ToUpper());
            lstUUID.Refresh();
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void btnCancelarUUID_Click(object sender, EventArgs e)
        {
            XmlDocument refXMLDocument = new XmlDocument();
            RSACryptoServiceProvider KeyVal = new RSACryptoServiceProvider();
                SVCCANCELACIONPRUEBAS.wcfCancelaASMXSoapClient client = new SVCCANCELACIONPRUEBAS.wcfCancelaASMXSoapClient();
                SVCCANCELACIONPRUEBAS.ArrayOfString sListaUUID = new SVCCANCELACIONPRUEBAS.ArrayOfString();

                foreach (String s in list)
                {
                    sListaUUID.Add(s);
                }

            Firmar(ref refXMLDocument, ref KeyVal, list, cmbRFC.Text, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")));
            
            string strSoapMessage =
                "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                "<CancelaCFD xmlns=\"http://cancelacfd.sat.gob.mx\">" +
                "" + refXMLDocument.OuterXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace("xmlns=\"http://cancelacfd.sat.gob.mx\"", "").Replace("<?xml version=\"1.0\"?>", "") + "" +
                "</CancelaCFD>" +
                "</s:Body></s:Envelope>";

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
            string response = client.fnCancelarXML(sListaUUID, cmbRFC.Text, strSoapMessage, txtUsuario.Text, txtPassword.Text);
                richTextBox1.Clear();
            XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(response));

            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: 
                            this.richTextBox1.SelectionColor = Color.Blue;
                            this.richTextBox1.AppendText("<");
                            this.richTextBox1.SelectionColor = Color.Brown;
                            this.richTextBox1.AppendText(reader.Name);
                            this.richTextBox1.SelectionColor = Color.Blue;
                            this.richTextBox1.AppendText(">");
                            break;
                        case XmlNodeType.Text: 
                            this.richTextBox1.SelectionColor = Color.Black;
                            this.richTextBox1.AppendText(reader.Value);
                            break;
                        case XmlNodeType.EndElement: 
                            this.richTextBox1.SelectionColor = Color.Blue;
                            this.richTextBox1.AppendText("</");
                            this.richTextBox1.SelectionColor = Color.Brown;
                            this.richTextBox1.AppendText(reader.Name);
                            this.richTextBox1.SelectionColor = Color.Blue;
                            this.richTextBox1.AppendText(">");
                            this.richTextBox1.AppendText("\n");
                            break;
                    }
                } DarFormato(richTextBox1.Text);
            }
            catch
            {
                richTextBox1.Text = response;
            }
        }

        public void DarFormato(string texto)
        {
            richTextBox1.Clear();
            XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(texto));
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        this.richTextBox1.SelectionColor = Color.Blue;
                        this.richTextBox1.AppendText("<");
                        this.richTextBox1.SelectionColor = Color.Brown;
                        this.richTextBox1.AppendText(reader.Name);
                        this.richTextBox1.SelectionColor = Color.Blue;
                        this.richTextBox1.AppendText(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        this.richTextBox1.SelectionColor = Color.Black;
                        this.richTextBox1.AppendText(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        this.richTextBox1.SelectionColor = Color.Blue;
                        this.richTextBox1.AppendText("</");
                        this.richTextBox1.SelectionColor = Color.Brown;
                        this.richTextBox1.AppendText(reader.Name);
                        this.richTextBox1.SelectionColor = Color.Blue;
                        this.richTextBox1.AppendText(">");
                        this.richTextBox1.AppendText("\n");
                        break;
                }
            }
        }

        public void Firmar(ref XmlDocument retXmlDocument, ref RSACryptoServiceProvider KeyVal, IList uuid, string sRfcEmisor, DateTime sfechaTimbrado)
        {
            try
            {
                string filename = openFileDialog1.FileName;
                byte[] arrayCer = File.ReadAllBytes(filename);
                
                filename = openFileDialog2.FileName;
                byte[] arrayKey = File.ReadAllBytes(filename);

                string cerFileName = openFileDialog1.SafeFileName;
                string keyFileName = openFileDialog2.SafeFileName;
                          
                string ruta = Settings.Default.RutaCertificados;
                string rutapfx = Settings.Default.Rutapfx;
                File.WriteAllBytes(ruta + cerFileName, arrayCer);
                File.WriteAllBytes(ruta + keyFileName, arrayKey);
                string rutaCer = ruta + cerFileName;
                string rutaKey = ruta + keyFileName;
                byte[] certPFx = crearPfx(rutaCer, rutaKey, txtPasswordCer.Text, cerFileName, keyFileName, rutapfx);

                X509Certificate2 certificate = new X509Certificate2(certPFx, txtPasswordCer.Text);
                XmlDocument documentoXML = new XmlDocument();
                documentoXML.PreserveWhitespace = false;
                DateTime time = DateTime.Now;
                string prefirma = fnCrearPreFirma(uuid, sRfcEmisor, time);
                documentoXML.LoadXml(prefirma);
                FirmarXML(documentoXML, certificate);
                retXmlDocument = documentoXML;
            }
            catch
            {
            
            }
        }

        public static void FirmarXML(XmlDocument xmlDoc, X509Certificate2 cert)
        {
            try
            {
                if (xmlDoc == null)
                    throw new ArgumentException("El argumento Doc no puede ser null.");
                if (cert == null)
                    throw new ArgumentNullException("Key");

                SignedXml xmlFirmado = new SignedXml(xmlDoc);

                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
                xmlFirmado.SigningKey = rsa;
        
                Reference referencia = new Reference();
                referencia.Uri = "";
                RSAKeyValue rsakey = new RSAKeyValue((RSA)rsa);
                KeyInfo keyInfo = new KeyInfo();
                KeyInfoX509Data kdata = new KeyInfoX509Data(cert);
                X509IssuerSerial xserial;
                xserial.IssuerName = cert.Issuer.ToString();
                xserial.SerialNumber = cert.SerialNumber;
                kdata.AddIssuerSerial(xserial.IssuerName, xserial.SerialNumber);
                keyInfo.AddClause(kdata);
                xmlFirmado.KeyInfo = keyInfo;
                XmlDsigEnvelopedSignatureTransform transformacionENV = new XmlDsigEnvelopedSignatureTransform();
                referencia.AddTransform(transformacionENV);
                xmlFirmado.AddReference(referencia);
                xmlFirmado.ComputeSignature();
                XmlDocument xmlD = new XmlDocument();
                XmlElement firmaDigitalXML = xmlFirmado.GetXml();
                xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(firmaDigitalXML, true));
            }
            catch (Exception)
            {

            }
        }

        public static string fnCrearPreFirma(IList uuid, string sRfcEmisor, DateTime sfechaTimbrado)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\"?>");
            sb.Append("<Cancelacion xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" RfcEmisor=\"" +
                sRfcEmisor + "\" Fecha=\"" + sfechaTimbrado.ToString("s") + "\" xmlns=\"http://cancelacfd.sat.gob.mx\">");
            for (int count = 0; count < uuid.Count; count++)
            {
                sb.Append("<Folios>");
                sb.Append("<UUID>" + uuid[count] + "</UUID>");
                sb.Append("</Folios>");
            }
            sb.Append("</Cancelacion>");

            return sb.ToString();
        }

        public byte[] crearPfx(string pathCert, string pathKey, string psPassword, string NombreCert, string NombreKey, string pathPfx)
        {
            crearCertificadoPEM(pathCert, NombreCert, pathPfx);
            crearLlavePEM(pathKey, psPassword, NombreKey, pathPfx);
            ProcessStartInfo info;
            Process proceso;
                info = new ProcessStartInfo(Settings.Default.RutaOpenSSL,
                @"pkcs12 -export -out " + pathPfx + NombreCert + ".pfx -inkey " + pathPfx + NombreKey + ".pem -in " + pathPfx + NombreCert + ".pem -passout pass:" + psPassword);
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                proceso = Process.Start(info);
                proceso.WaitForExit();
                proceso.Dispose();
            byte[] key_pem = File.ReadAllBytes(pathPfx + NombreCert + ".pfx");
                File.Delete(pathCert);
                File.Delete(pathKey);
                File.Delete(pathPfx + NombreCert + ".pem");
                File.Delete(pathPfx + NombreKey + ".pem");
                File.Delete(pathPfx + NombreCert + ".pfx");
            return key_pem;
        }

        public void crearCertificadoPEM(string pathCert, string NombreCert, string pathPfx)
        {
            ProcessStartInfo info;
            Process proceso;
            info = new ProcessStartInfo(Settings.Default.RutaOpenSSL,
                  @"x509 -inform DER -in " + pathCert + " -out " + pathPfx + NombreCert + ".pem");

            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            proceso = Process.Start(info);
            proceso.WaitForExit();
            proceso.Dispose();
        }

        public void crearLlavePEM(string pathKey, string psPassword, string NombreKey, string pathPfx)
        {
            ProcessStartInfo info;
            Process proceso;

            info = new ProcessStartInfo(Settings.Default.RutaOpenSSL,
                @"pkcs8 -inform DER -in " + pathKey + " -passin pass:" + psPassword + " -out " + pathPfx + NombreKey + ".pem");
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            proceso = Process.Start(info);
            proceso.WaitForExit();
            proceso.Dispose();
        }
    }
}

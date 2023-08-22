using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace SectorPrimarioClaseXSD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox4.Text = (DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            cbTipo.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument refXMLDocument = new XmlDocument();
            System.Security.Cryptography.RSACryptoServiceProvider KeyVal = new System.Security.Cryptography.RSACryptoServiceProvider();
            Firmar(ref refXMLDocument, ref KeyVal);
            richTextBox1.Text = refXMLDocument.OuterXml;
        }

        public void Firmar(ref XmlDocument retXmlDocument, ref System.Security.Cryptography.RSACryptoServiceProvider KeyVal)
        {
            try
            {
                string rfcEmisor = cbEmisor.Text;
                string rfcAdquiriente = cbAdquiriente.Text;
                string rfcPAC = textBox3.Text;
                string fecha = textBox4.Text;

                DataTable certificado = new DataTable();
                string filename = openFileDialog1.FileName;
                byte[] arrayCer = File.ReadAllBytes(filename);
                filename = openFileDialog2.FileName;
                string cerFileName = openFileDialog1.SafeFileName;
                string keyFileName = openFileDialog2.SafeFileName;
                byte[] arrayKey = File.ReadAllBytes(filename);
                string ruta = @"C:\Archivos\Canonicalizacion\Certificados\PFXExtra\";
                string rutapfx = @"C:\Archivos\Canonicalizacion\Certificados\PFXExtra\";
                File.WriteAllBytes(ruta + cerFileName, arrayCer);
                File.WriteAllBytes(ruta + keyFileName, arrayKey);
                string rutaCer = ruta + cerFileName;
                string rutaKey = ruta + keyFileName;
                byte[] certPFx = crearPfx(rutaCer, rutaKey, "12345678a", cerFileName, keyFileName, rutapfx);



                string filename2 = openFileDialog3.FileName;
                byte[] arrayCer2 = File.ReadAllBytes(filename2);
                filename2 = openFileDialog4.FileName;
                string cerFileName2 = openFileDialog3.SafeFileName;
                string keyFileName2 = openFileDialog4.SafeFileName;
                byte[] arrayKey2 = File.ReadAllBytes(filename2);
                string ruta2 = @"C:\Archivos\Canonicalizacion\Certificados\PFXExtra\";
                string rutapfx2 = @"C:\Archivos\Canonicalizacion\Certificados\PFXExtra\";
                File.WriteAllBytes(ruta2 + cerFileName2, arrayCer2);
                File.WriteAllBytes(ruta2 + keyFileName2, arrayKey2);
                string rutaCer2 = ruta2 + cerFileName2;
                string rutaKey2 = ruta2 + keyFileName2;
                byte[] certPFx2 = crearPfx(rutaCer2, rutaKey2, "12345678a", cerFileName2, keyFileName2, rutapfx2);


                FirmaSectorPrimario.WSFirmaSectorPrimarioClient WSFirmaSectorPrimario = new FirmaSectorPrimario.WSFirmaSectorPrimarioClient();
                FirmaSectorPrimarioLocal.WSFirmaSectorPrimarioClient WSFirmaSectorPrimarioLocal = new FirmaSectorPrimarioLocal.WSFirmaSectorPrimarioClient();

                // fupCer.FileName, fupKey.FileName, txtPass.Text.Trim()
                // certPFx = Utilerias.Encriptacion.DES3.Desencriptar(certPFx);
                // clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Firma-Cargar PFX");
                //X509Certificate2 certificate = new X509Certificate2(certPFx, "12345678a");
                // RSACryptoServiceProvider Key = (RSACryptoServiceProvider)certificate.PrivateKey;
                // KeyVal = Key;
                // clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Firma-Recupera Key");
                // Cree un objeto XmlDocument; para ello, cargue un archivo XML de disco.
                // El objeto XmlDocument contiene el elemento XML que se debe cifrar.
                XmlDocument documentoXML = new XmlDocument();
                // Format the document to ignore white spaces.
                documentoXML.PreserveWhitespace = false;
                // se manda prefirmar el documento
                // string date = "2014-02-10 11:37:17";
                // DateTime time = DateTime.Parse(date);
                DateTime time = DateTime.Now;
                //string prefirma = fnCrearPreFirma(uuid, sRfcEmisor, time);
                string prefirma = fnCrearPreFirmaSectorPrimario(rfcEmisor, rfcAdquiriente, rfcPAC, fecha);

                int tipo = 1;
                if (cbTipo.SelectedIndex == 0)
                {
                    tipo = 1;
                }
                else if (cbTipo.SelectedIndex == 1)
                {
                    tipo = 2;
                }
                else if (cbTipo.SelectedIndex == 2)
                {
                    tipo = 3;
                }

                string resultado = WSFirmaSectorPrimario.fnFirmarSectorPirmario(prefirma, "12345678a", "12345678a", certPFx, certPFx2, tipo, textBox1.Text);
                //string resultado = WSFirmaSectorPrimarioLocal.fnFirmarSectorPirmario(prefirma, "12345678a", "12345678a", certPFx, certPFx2, tipo, textBox1.Text);

                // clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Firma-Crear PFX");
                documentoXML.LoadXml(resultado);
                // Firme el documento XML.
                //FirmarXML(documentoXML, certificate, "SelloReceptor");
                retXmlDocument = documentoXML;

            }
            catch (Exception ex) { }
        }


        public static string fnCrearPreFirmaSectorPrimario(string rfcEmisor, string rfcAdquiriente, string rfcPAC, string fecha)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\"?>");
            sb.Append("<ConsultaSectorPrimario xmlns=\"http://www.sat.gob.mx/idc/consulta/SectorPrimario\" rfcEmisor=\"" + rfcEmisor + "\" rfcPrestadorAutorizado=\"" + rfcPAC + "\" rfcReceptor=\"" + rfcAdquiriente + "\" tsSolicitud=\"" + fecha + "\">");
            sb.Append("</ConsultaSectorPrimario>");
            return sb.ToString();
        }

        public static void FirmarXML(XmlDocument xmlDoc, X509Certificate2 cert, string id_firma)
        {
            try
            {
                // Comprobamos los argumentos.
                if (xmlDoc == null)
                    throw new ArgumentException("El argumento Doc no puede ser null.");
                if (cert == null)
                    throw new ArgumentNullException("Key");
                // Cree un nuevo objeto SignedXml y pásele el objeto XmlDocument.

                //XmlAttribute att = xmlDoc.CreateAttribute("xmlns:ds");
                //att.Value = "http://www.w3.org/2000/09/xmldsig#";


                SignedXml xmlFirmado = new SignedXml(xmlDoc);
                //***************************************************************************;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = (System.Security.Cryptography.RSACryptoServiceProvider)cert.PrivateKey;
                xmlFirmado.SigningKey = rsa;

                //***************************************************************************
                // Agregue la clave RSA de firma al objeto SignedXml.
                // xmlFirmado.SigningKey = cert.PrivateKey;
                // Cree un objeto Reference que describa qué se debe firmar.
                // Para firmar el documento completo, establezca la propiedad Uri como "".
                Reference referencia = new Reference();
                referencia.Uri = "";

                RSAKeyValue rsakey = new RSAKeyValue((System.Security.Cryptography.RSA)rsa);
                KeyInfo keyInfo = new KeyInfo();
                KeyInfoX509Data kdata = new KeyInfoX509Data(cert);
                KeyInfoName kin = new KeyInfoName();
                keyInfo.AddClause(rsakey);
                byte[] bCertificadoInvertido = cert.GetSerialNumber().Reverse().ToArray();
                //kin.Value = cert.SerialNumber;
                kin.Value = Encoding.Default.GetString(bCertificadoInvertido);
                X509IssuerSerial xserial;
                xserial.IssuerName = cert.Issuer.ToString();
                xserial.SerialNumber = cert.SerialNumber;
                kdata.AddIssuerSerial(xserial.IssuerName, xserial.SerialNumber);
                keyInfo.AddClause(kin);
                //keyInfo.AddClause(kdata);
                xmlFirmado.KeyInfo = keyInfo;
                Signature XMLSignature = xmlFirmado.Signature;

                XMLSignature.Id = id_firma;

                // Agregue un objeto XmlDsigEnvelopedSignatureTransform al objeto Reference.
                // Una transformación permite al comprobador representar los datos XML
                // de idéntico modo que el firmante. Los datos XML se pueden representar de distintas maneras,
                // por lo que este paso es vital para la comprobación.
                //XmlDsigEnvelopedSignatureTransform transformacionENV = new XmlDsigEnvelopedSignatureTransform();
                //XmlDsigXPathTransform c14n = new XmlDsigXPathTransform();
                referencia.AddTransform(CreateXPathTransform("ancestor-or-self::Signature"));
                //referencia.AddTransform(transformacionENV);
                //referencia.AddTransform(c14n);
                // Agregue el objeto Reference al objetoSignedXml.
                xmlFirmado.AddReference(referencia);
                //xmlFirmado.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
                // Llame al método ComputeSignature para calcular la firma.
                xmlFirmado.ComputeSignature();
                //XmlDocument xmlD = new XmlDocument();
                // a = xmlFirmado.GetXml();
                // xmlD.LoadXml(xmlFirmado.ToString());
                // Recupere la representación XML de la firma (un elemento <Signature>)
                // y guárdela en un nuevo objeto XmlElement.
                XmlElement firmaDigitalXML = xmlFirmado.GetXml();

                // Anexe el elemento al objeto XmlDocument.
                xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(firmaDigitalXML, true));
            }
            catch (Exception) { }
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

        public byte[] crearPfx(string pathCert, string pathKey, string psPassword, string NombreCert, string NombreKey, string pathPfx)
        {
            crearCertificadoPEM(pathCert, NombreCert, pathPfx);
            crearLlavePEM(pathKey, psPassword, NombreKey, pathPfx);
            ProcessStartInfo info;
            Process proceso;
            info = new ProcessStartInfo(@"C:\OpenSSL-Win32\bin\openssl.exe",
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
            info = new ProcessStartInfo(@"C:\OpenSSL-Win32\bin\openssl.exe",
                  @"x509 -inform DER -in " + pathCert + " -out " + pathPfx + NombreCert + ".pem");
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            proceso = Process.Start(info);
            proceso.WaitForExit();
            proceso.Dispose();
        }
        //-------------------------------------------------------------------------------------
        public void crearLlavePEM(string pathKey, string psPassword, string NombreKey, string pathPfx)
        {
            ProcessStartInfo info;
            Process proceso;
            info = new ProcessStartInfo(@"C:\OpenSSL-Win32\bin\openssl.exe",
                @"pkcs8 -inform DER -in " + pathKey + " -passin pass:" + psPassword + " -out " + pathPfx + NombreKey + ".pem");
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            proceso = Process.Start(info);
            proceso.WaitForExit();
            proceso.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog3.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog4.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox4.Text = (DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
        }


        private void btnGenerarPeticion_Click(object sender, EventArgs e)
        {
            string firmaReq = richTextBox1.Text;
            string firmaPAC = richTextBox1.Text;
            StringBuilder peticion = new StringBuilder();
            peticion.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            peticion.Append("<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            peticion.Append("<ConsultaSectorPrimario xmlns=\"http://www.sat.gob.mx/idc/consulta/SectorPrimario\" rfcPrestadorAutorizado=\""+textBox3.Text+"\" tsSolicitud=\""+textBox4.Text+"\" rfcEmisor=\""+cbEmisor.Text+"\" rfcReceptor=\""+cbAdquiriente.Text+"\">");
            peticion.Append(firmaReq);
            peticion.Append(firmaPAC);
            peticion.Append("</ConsultaSectorPrimario>");
            peticion.Append("</s:Body>");
            peticion.Append("</s:Envelope>");
            String a = peticion.ToString();

            txtPeticion.Text = a;
        }

        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
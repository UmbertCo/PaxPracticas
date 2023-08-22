using OpenSSL;
using OpenSSL.Core;
using OpenSSL.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmOpenSSL : Form
    {
        public frmOpenSSL()
        {
            InitializeComponent();
        }        
        private void btnArchivoLlavePrivada_Click(object sender, EventArgs e)
        {
            OpenFileDialog pfdLlavePrivada = new OpenFileDialog();
            pfdLlavePrivada.ShowDialog();

            if (string.IsNullOrEmpty(pfdLlavePrivada.FileName))
                return;

            txtLlavePrivada.Text = pfdLlavePrivada.FileName;
        }
        private void btnSeleccionarPfx_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdPfx = new OpenFileDialog();
            ofdPfx.ShowDialog();

            txtPfx.Text = string.Empty;

            if (string.IsNullOrEmpty(ofdPfx.FileName))
                return;

            txtPfx.Text = ofdPfx.FileName;
        }
        private void btnSeleccionarArchivo_Click(object sender, EventArgs e)
        {
            OpenFileDialog pfdXml = new OpenFileDialog();
            //pfdCertificado.Filter = ".cer";
            pfdXml.ShowDialog();

            txtArchivoXml.Text = string.Empty;

            if (string.IsNullOrEmpty(pfdXml.FileName))
                return;

            txtArchivoXml.Text = pfdXml.FileName;
        }
        private void btnSeleccionarLlavePublica_Click(object sender, EventArgs e)
        {
            OpenFileDialog pfdCertificado = new OpenFileDialog();
            pfdCertificado.ShowDialog();

            txtLlavePublica.Text = string.Empty;

            if (string.IsNullOrEmpty(pfdCertificado.FileName))
                return;

            txtLlavePublica.Text = pfdCertificado.FileName;
        }
        private void btnGenerarSelloOpenSSLCL_Click(object sender, EventArgs e)
        {
            string sInstruccion = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(txtLlavePrivada.Text))
                    return;

                if (string.IsNullOrEmpty(txtArchivoXml.Text))
                    return;

                sInstruccion = "pkcs8 -inform DER -in " + txtLlavePrivada.Text + " -passin pass:" + "12345678a" + " -out " + "C:\\ConectorPAXMYERS\\XML\\Certificados\\PruebaPem.pem";
                string path = "C:\\ConectorPAXMYERS\\XML\\Certificados\\";

                XmlDocument xdComprobante = new XmlDocument();
                xdComprobante.Load(txtArchivoXml.Text);

                string sCadenaOriginal = fnConstruirCadenaTimbrado(xdComprobante.CreateNavigator());

                // Escribir archivo UTF8 de la cadena
                var tempCadena = path + "cadena";
                System.IO.File.WriteAllText(tempCadena, sCadenaOriginal);

                // Digestion SHA1
                var tempSha = path + "sha";
                var opensslPath = "C:\\OpenSSL-Win32\\bin\\openssl.exe";
                // Crear .pem del .key
                var tempPem = path + "\\openssl\\pem";
                Process process2 = new Process();
                process2.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                process2.StartInfo.FileName = opensslPath;
                process2.StartInfo.Arguments = sInstruccion;
                process2.StartInfo.UseShellExecute = false;
                process2.StartInfo.ErrorDialog = false;
                process2.StartInfo.RedirectStandardOutput = true;
                process2.Start();
                process2.WaitForExit();

                // Generar sello
                Process process3 = new Process();
                process3.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                process3.StartInfo.FileName = opensslPath;
                process3.StartInfo.Arguments = "dgst -sha1 -sign " + "C:\\ConectorPAXMYERS\\XML\\Certificados\\PruebaPem.pem" + " " + path + "cadena";
                process3.StartInfo.UseShellExecute = false;
                process3.StartInfo.ErrorDialog = false;
                process3.StartInfo.RedirectStandardOutput = true;
                process3.Start();

                // Codificar en Base64
                String selloTxt = process3.StandardOutput.ReadToEnd();
                String b64 = Convert.ToBase64String(Encoding.Default.GetBytes(selloTxt));
                process3.WaitForExit();

                txtResultado.Text = b64;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnGenerarSelloOpenSSLNetPfx_Click(object sender, EventArgs e)
        {
            byte[] aLlavePrivada = null;
            byte[] aLlavePublica = null;
            byte[] abCadenaOriginal = null;
            byte[] abCResultado = null;
            string sLlavePrivada = string.Empty;
            string sLlavePublica = string.Empty;
            string sArchivo = string.Empty;
            string sCadenaOriginal = string.Empty;
            string sSello = string.Empty;
            XmlDocument xdArchivo = new XmlDocument();
            try
            {
                if (string.IsNullOrEmpty(txtLlavePrivada.Text))
                    return;

                //if (string.IsNullOrEmpty(txtLlavePublica.Text))
                //    return;

                if (string.IsNullOrEmpty(txtArchivoXml.Text))
                    return;

                //OpenSSL.Core.PasswordCallback += new PasswordHandler(PasswordHandler);


                xdArchivo.Load(txtArchivoXml.Text);
                sCadenaOriginal = fnConstruirCadenaTimbrado(xdArchivo.CreateNavigator());

                aLlavePrivada = File.ReadAllBytes(txtLlavePrivada.Text);
                aLlavePublica = File.ReadAllBytes(txtLlavePublica.Text);
                sLlavePrivada = File.ReadAllText(txtLlavePrivada.Text);
                sLlavePublica = File.ReadAllText(txtLlavePublica.Text);
                //abCadenaOriginal = GetBytes(sCadenaOriginal);
                abCadenaOriginal = System.Text.Encoding.UTF8.GetBytes(sCadenaOriginal);

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdArchivo.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                XPathNavigator navDocGenera = xdArchivo.CreateNavigator();

                string sPemCertificado = string.Empty;
                //sPemCertificado = "-----BEGIN CERTIFICATE-----";
                sPemCertificado += Convert.ToBase64String(aLlavePublica);
                //sPemCertificado += "-----END CERTIFICATE-----";

                //X509Certificate2 certificado = new X509Certificate2(System.Text.Encoding.UTF8.GetBytes(psLlavePublica));
                //aLlavePublica = PEM("CERTIFICATE", aLlavePublica);

                //fnGenerarSello(txtLlavePrivada.Text, sCadenaOriginal);
                fnGenerarSello(aLlavePrivada, sCadenaOriginal);
                fnGenerarSelloPEM(aLlavePrivada, sCadenaOriginal);
                fnGenerarSelloPEM(txtLlavePrivada.Text, sCadenaOriginal);

                fnVerificarSelloPEM(aLlavePublica, sCadenaOriginal, navDocGenera.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value);
                fnVerificarSello(txtLlavePublica.Text, sCadenaOriginal, navDocGenera.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value);

                OpenSSL.Core.BIO key_bio = new OpenSSL.Core.BIO(aLlavePrivada);
                OpenSSL.Core.BIO cert_bio = new OpenSSL.Core.BIO(aLlavePublica);
                //OpenSSL.Core.BIO pfx_bio = new OpenSSL.Core.BIO(txtPfx.Text);


                //OpenSSL.Crypto.CryptoKey ckLlavePrivada2 = OpenSSL.Crypto.CryptoKey.FromPrivateKey(sLlavePrivada, "");
                //OpenSSL.Crypto.CryptoKey ckLlavePrivada = OpenSSL.Crypto.CryptoKey.FromPrivateKey(key_bio, "");

                //OpenSSL.Crypto.MessageDigestContext mdMensaje = new MessageDigestContext(MessageDigest.SHA1);

                ////byte[] abDigest = mdMensaje.Digest(abCadenaOriginal);
                //byte[] abCResultado = mdMensaje.Sign(abCadenaOriginal, ckLlavePrivada);
                //txtResultado.Text = Convert.ToBase64String(abCResultado);


                //OpenSSL.X509.PKCS12 Pfx = new OpenSSL.X509.PKCS12(pfx_bio, "12345678a");

                //OpenSSL.Crypto.RSA rsa = OpenSSL.Crypto.RSA.FromPublicKey(key_bio, null, null);
                //RSACryptoServiceProvider rsaprovider = (RSACryptoServiceProvider)Pfx.Certificate.PrivateKey;
                //byte[] message = new byte[1024];
                //message = rsa.PublicEncrypt(abCadenaOriginal, OpenSSL.Crypto.RSA.Padding.PKCS1);
                //txtResultado.Text = Convert.ToBase64String(message);
                //rsa = (OpenSSL.Crypto.RSA)Pfx.PrivateKey;
                //(OpenSSL.Crypto.RSA)Pfx.PrivateKey;

                //OpenSSL.Crypto.CryptoKey ckLlavePrivada2 = OpenSSL.Crypto.CryptoKey.FromPrivateKey(sLlavePrivada, "");
                //OpenSSL.Crypto.CryptoKey ckLlavePrivada = OpenSSL.Crypto.CryptoKey.FromPrivateKey(key_bio, null);


                //OpenSSL.Core.BIO key_bio = new OpenSSL.Core.BIO(aLlavePrivada);
                //OpenSSL.Core.BIO cert_bio = new OpenSSL.Core.BIO(aLlavePublica);

                OpenSSL.Crypto.CryptoKey key = OpenSSL.Crypto.CryptoKey.FromPrivateKey(key_bio, "");
                OpenSSL.X509.X509Certificate cert = new OpenSSL.X509.X509Certificate(cert_bio);

                OpenSSL.Core.Stack<OpenSSL.X509.X509Certificate> certificado = new OpenSSL.Core.Stack<OpenSSL.X509.X509Certificate>();
                //OpenSSL.X509.PKCS12 Pfx = new OpenSSL.X509.PKCS12("12345678a", key, cert, certificado);

                var arreglo = OpenSSL.Core.BIO.MemoryBuffer();
                //Pfx.Write(arreglo);
                byte[] aArreglo = arreglo.ReadBytes((int)arreglo.NumberWritten).Array;
                X509Certificate2 pfx_cer = new X509Certificate2(aArreglo, "12345678a", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                arreglo.Dispose();

                OpenSSL.Crypto.MessageDigestContext mdMensaje = new MessageDigestContext(MessageDigest.SHA1);
                byte[] abDigest = mdMensaje.Digest(abCadenaOriginal);
                //abCResultado = mdMensaje.Sign(abDigest, (OpenSSL.Crypto.CryptoKey)(pfx_cer.PrivateKey));

                //OpenSSL.Crypto.MessageDigestContext mdMensaje = new MessageDigestContext(MessageDigest.SHA1);
                //byte[] abDigest = mdMensaje.Digest(abCadenaOriginal);
                //byte[] abCResultado = mdMensaje.Sign(abDigest, key);
                //txtResultado.Text = Convert.ToBase64String(abCResultado);


                //using (var rsa = key.GetRSA())
                //{
                //    abCResultado = rsa.PrivateEncrypt(abCadenaOriginal, OpenSSL.Crypto.RSA.Padding.PKCS1);
                //}


                // Sella la cadena original en base al certificado creado con OpenSSL.Net y con RSACryptoSerfviceProvider
                /////////////////////////////////
                //using (SHA1 sha1 =SHA1.Create())
                //{
                //    RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)pfx_cer.PrivateKey;
                //    abCResultado = rsa.SignData(Encoding.UTF8.GetBytes(sCadenaOriginal), sha1);
                //}
                /////////////////////////////////

                // Encriptación con CipherContext
                /////////////////////////////////
                //OpenSSL.Crypto.CipherContext ctEncriptacion = new CipherContext(OpenSSL.Crypto.Cipher.AES_256_CBC);
                //abCResultado = ctEncriptacion.Encrypt(abCadenaOriginal, aLlavePrivada, System.Text.Encoding.UTF8.GetBytes("12345678a"));

                //byte[] aEncriptado = ctEncriptacion.Decrypt(abCResultado, aLlavePrivada, System.Text.Encoding.UTF8.GetBytes("12345678a"));
                //string sEncriptado = Convert.ToBase64String(abCResultado);
                /////////////////////////////////

                txtResultado.Text = Convert.ToBase64String(abCResultado);

                //XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdArchivo.NameTable);
                //nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                //nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                //nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                //XPathNavigator navDocGenera = xdArchivo.CreateNavigator();
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(txtResultado.Text);
                xdArchivo.Save(@"C:\ConectorPAXMYERS\XML\Certificados\prueba.xml");
            }
            catch (OpenSslException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnCrearPfxClase_Click(object sender, EventArgs e)
        {
            byte[] aLlavePrivada = null;
            byte[] aLlavePublica = null;
            try
            {
                if (string.IsNullOrEmpty(txtLlavePublica.Text))
                    return;

                if (string.IsNullOrEmpty(txtLlavePrivada.Text))
                    return;

                XmlDocument xdComprobante = new XmlDocument();
                xdComprobante.Load(txtArchivoXml.Text);

                string sCadenaOriginal = fnConstruirCadenaTimbrado(xdComprobante.CreateNavigator());

                aLlavePublica = File.ReadAllBytes(txtLlavePublica.Text);
                aLlavePrivada = File.ReadAllBytes(txtLlavePrivada.Text);

                X509Certificate2 cLlavePrivada = new X509Certificate2(txtLlavePrivada.Text);
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cLlavePrivada.PrivateKey;

                byte[] bDatosFirmados = rsa.SignData(Encoding.UTF8.GetBytes(sCadenaOriginal), "SHA1");
                txtResultado.Text = Convert.ToBase64String(bDatosFirmados);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnGenerarSelloRSA_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPfx.Text))
                    return;

                if (string.IsNullOrEmpty(txtArchivoXml.Text))
                    return;

                XmlDocument xdComprobante = new XmlDocument();
                xdComprobante.Load(txtArchivoXml.Text);

                string sCadenaOriginal = fnConstruirCadenaTimbrado(xdComprobante.CreateNavigator());

                X509Certificate2 pfx = new X509Certificate2(txtPfx.Text, "12345678a");
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)pfx.PrivateKey;
                byte[] bDatosFirmados = rsa.SignData(Encoding.UTF8.GetBytes(sCadenaOriginal), "SHA1");
                txtResultado.Text = Convert.ToBase64String(bDatosFirmados);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnCrearPfxOpenSSLNet_Click(object sender, EventArgs e)
        {
            byte[] aLlavePrivada = null;
            byte[] aLlavePublica = null;
            try
            {
                if (string.IsNullOrEmpty(txtLlavePrivada.Text))
                    return;

                if (string.IsNullOrEmpty(txtLlavePublica.Text))
                    return;
   
                aLlavePrivada = File.ReadAllBytes(txtLlavePrivada.Text);
                aLlavePublica = File.ReadAllBytes(txtLlavePublica.Text);

                OpenSSL.Core.BIO key_bio = new OpenSSL.Core.BIO(aLlavePrivada);
                OpenSSL.Core.BIO cert_bio = new OpenSSL.Core.BIO(aLlavePublica);

                OpenSSL.Crypto.CryptoKey key = OpenSSL.Crypto.CryptoKey.FromPrivateKey(key_bio, "");
                OpenSSL.X509.X509Certificate cert = new OpenSSL.X509.X509Certificate(cert_bio);

                OpenSSL.Core.Stack<OpenSSL.X509.X509Certificate> certificado = new OpenSSL.Core.Stack<OpenSSL.X509.X509Certificate>();
                //OpenSSL.X509.PKCS12 Pfx = new OpenSSL.X509.PKCS12("12345678a", key, cert, certificado);

                var arreglo = OpenSSL.Core.BIO.MemoryBuffer();
                //Pfx.Write(arreglo);
                byte[] aArreglo = arreglo.ReadBytes((int)arreglo.NumberWritten).Array;
                X509Certificate2 pfx = new X509Certificate2(aArreglo, "12345678a", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                arreglo.Dispose();

                File.WriteAllBytes(@"C:\ConectorPAXMYERS\XML\Certificados\pfx.pfx" , pfx.Export(X509ContentType.Pfx, ""));
                //OpenSSL.Crypto.RSA rsa = new OpenSSL.Crypto.RSA();
                //(OpenSSL.Crypto.RSA)Pfx.PrivateKey;

                //OpenSSL.Crypto.CryptoKey ckLlavePrivada2 = OpenSSL.Crypto.CryptoKey.FromPrivateKey(sLlavePrivada, "");
                //OpenSSL.Crypto.CryptoKey ckLlavePrivada = OpenSSL.Crypto.CryptoKey.FromPrivateKey(key_bio, null);

                //OpenSSL.Crypto.MessageDigestContext mdMensaje = new MessageDigestContext(MessageDigest.SHA1);
                //byte[] abDigest = mdMensaje.Digest(abCadenaOriginal);
                //byte[] abCResultado = mdMensaje.Sign(abDigest, ckLlavePrivada);
                //txtResultado.Text = Convert.ToBase64String(abCResultado);


            }
            catch (OpenSslException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnGenerarSelloOpenSSLNet_Click(object sender, EventArgs e)
        {
            byte[] aLlavePrivada = null;
            byte[] abCadenaOriginal = null;
            byte[] abCResultado = null;
            string sArchivo = string.Empty;
            string sCadenaOriginal = string.Empty;
            string sSello = string.Empty;
            XmlDocument xdArchivo = new XmlDocument();
            try
            {
                if (string.IsNullOrEmpty(txtLlavePrivada.Text))
                    return;

                if (string.IsNullOrEmpty(txtArchivoXml.Text))
                    return;

                xdArchivo.Load(txtArchivoXml.Text);
                sCadenaOriginal = fnConstruirCadenaTimbrado(xdArchivo.CreateNavigator());

                sArchivo = File.ReadAllText(txtLlavePrivada.Text);
                abCadenaOriginal = System.Text.Encoding.UTF8.GetBytes(sCadenaOriginal);

                OpenSSL.Core.BIO key_bio = new OpenSSL.Core.BIO(sArchivo);

                OpenSSL.Crypto.CryptoKey ckLlavePrivada2 = OpenSSL.Crypto.CryptoKey.FromPrivateKey(key_bio, "12345678a");
                OpenSSL.Crypto.RSA dsaLlavePrivada = OpenSSL.Crypto.RSA.FromPrivateKey(key_bio);
                //dsaLlavePrivada.

                //OpenSSL.Crypto.CryptoKey ckLlavePrivada = OpenSSL.Crypto.CryptoKey.KeyType.;               
                     
                //OpenSSL.Crypto.CryptoKey ckLlavePrivada2 = OpenSSL.Crypto.CryptoKey.FromPrivateKey(sLlavePrivada, "");
                //OpenSSL.Crypto.CryptoKey ckLlavePrivada = OpenSSL.Crypto.CryptoKey.FromPrivateKey(key_bio, "");

                //OpenSSL.Crypto.MessageDigestContext mdMensaje = new MessageDigestContext(MessageDigest.SHA1);

                ////byte[] abDigest = mdMensaje.Digest(abCadenaOriginal);
                //byte[] abCResultado = mdMensaje.Sign(abCadenaOriginal, ckLlavePrivada);
                //txtResultado.Text = Convert.ToBase64String(abCResultado);


                //OpenSSL.X509.PKCS12 Pfx = new OpenSSL.X509.PKCS12(pfx_bio, "12345678a");

                //OpenSSL.Crypto.RSA rsa = OpenSSL.Crypto.RSA.FromPublicKey(key_bio, null, null);
                //RSACryptoServiceProvider rsaprovider = (RSACryptoServiceProvider)Pfx.Certificate.PrivateKey;
                //byte[] message = new byte[1024];
                //message = rsa.PublicEncrypt(abCadenaOriginal, OpenSSL.Crypto.RSA.Padding.PKCS1);
                //txtResultado.Text = Convert.ToBase64String(message);
                //rsa = (OpenSSL.Crypto.RSA)Pfx.PrivateKey;
                //(OpenSSL.Crypto.RSA)Pfx.PrivateKey;

                //OpenSSL.Crypto.CryptoKey ckLlavePrivada2 = OpenSSL.Crypto.CryptoKey.FromPrivateKey(sLlavePrivada, "");
                //OpenSSL.Crypto.CryptoKey ckLlavePrivada = OpenSSL.Crypto.CryptoKey.FromPrivateKey(key_bio, null);


                //OpenSSL.Core.BIO key_bio = new OpenSSL.Core.BIO(aLlavePrivada);
                //OpenSSL.Core.BIO cert_bio = new OpenSSL.Core.BIO(aLlavePublica);

                //OpenSSL.Crypto.CryptoKey key = OpenSSL.Crypto.CryptoKey.FromPrivateKey(key_bio, "");
                //OpenSSL.X509.X509Certificate cert = new OpenSSL.X509.X509Certificate(cert_bio);

                //OpenSSL.Core.Stack<OpenSSL.X509.X509Certificate> certificado = new OpenSSL.Core.Stack<OpenSSL.X509.X509Certificate>();
                //OpenSSL.X509.PKCS12 Pfx = new OpenSSL.X509.PKCS12("12345678a", key, cert, certificado);

                //var arreglo = OpenSSL.Core.BIO.MemoryBuffer();
                //Pfx.Write(arreglo);
                //byte[] aArreglo = arreglo.ReadBytes((int)arreglo.NumberWritten).Array;
                //X509Certificate2 pfx_cer = new X509Certificate2(aArreglo, "12345678a", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                //arreglo.Dispose();

                //OpenSSL.Crypto.MessageDigestContext mdMensaje = new MessageDigestContext(MessageDigest.SHA1);
                //byte[] abDigest = mdMensaje.Digest(abCadenaOriginal);
                //abCResultado = mdMensaje.Sign(abDigest, (OpenSSL.Crypto.CryptoKey)(pfx_cer.PrivateKey));

                //OpenSSL.Crypto.MessageDigestContext mdMensaje = new MessageDigestContext(MessageDigest.SHA1);
                //byte[] abDigest = mdMensaje.Digest(abCadenaOriginal);
                //byte[] abCResultado = mdMensaje.Sign(abDigest, key);
                //txtResultado.Text = Convert.ToBase64String(abCResultado);


                //using (var rsa = key.GetRSA())
                //{
                //    abCResultado = rsa.PrivateEncrypt(abCadenaOriginal, OpenSSL.Crypto.RSA.Padding.PKCS1);
                //}


                // Sella la cadena original en base al certificado creado con OpenSSL.Net y con RSACryptoSerfviceProvider
                /////////////////////////////////
                //using (SHA1 sha1 =SHA1.Create())|
                //{
                //    RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)pfx_cer.PrivateKey;
                //    abCResultado = rsa.SignData(Encoding.UTF8.GetBytes(sCadenaOriginal), sha1);
                //}
                /////////////////////////////////

                // Encriptación con CipherContext
                /////////////////////////////////
                //OpenSSL.Crypto.CipherContext ctEncriptacion = new CipherContext(OpenSSL.Crypto.Cipher.AES_256_CBC);
                //abCResultado = ctEncriptacion.Encrypt(abCadenaOriginal, aLlavePrivada, System.Text.Encoding.UTF8.GetBytes("12345678a"));

                //byte[] aEncriptado = ctEncriptacion.Decrypt(abCResultado, aLlavePrivada, System.Text.Encoding.UTF8.GetBytes("12345678a"));
                //string sEncriptado = Convert.ToBase64String(abCResultado);
                /////////////////////////////////

                txtResultado.Text = Convert.ToBase64String(abCResultado);

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdArchivo.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                XPathNavigator navDocGenera = xdArchivo.CreateNavigator();
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(txtResultado.Text);
                xdArchivo.Save(@"C:\ConectorPAXMYERS\XML\Certificados\prueba.xml");
            }
            catch (OpenSslException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public String fnConstruirCadenaTimbrado(IXPathNavigable xml)
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

        public string fnCrearSello(string psCadenaOriginal)
        {
            String sello = String.Empty;
            try
            {
                OpenSSL.Crypto.MessageDigestContext digestContext = new MessageDigestContext(OpenSSL.Crypto.MessageDigest.SHA1);

            }
            catch (Exception)
            {

            }
            return sello;
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Función que se encarga de verificar el sello con el certificado
        /// </summary>
        /// <param name="paLlavePublica">Llave Publica</param>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <param name="psSello">Sello</param>
        private bool fnVerificarSello(byte[] paLlavePublica, string psCadenaOriginal, string psSello)
        {
            byte[] abSello = null;
            bool bVerificado = false;
            try
            {
                using (OpenSSL.Core.BIO public_bio = new OpenSSL.Core.BIO(paLlavePublica))
                {
                    abSello = Convert.FromBase64String(psSello);
                    using (OpenSSL.X509.X509Certificate cer = OpenSSL.X509.X509Certificate.FromDER(public_bio))
                    {
                        // Si existe una excepción, es porque el sello es invalido
                        try { bVerificado = MessageDigestContext.Verify(MessageDigest.SHA1, new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal)), abSello, cer.PublicKey); }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el sello: " + ex.Message);
            }
            return bVerificado;
        }

        /// <summary>
        /// Función que se encarga de verificar el sello con el certificado
        /// </summary>
        /// <param name="psLlavePublica">Ruta Llave Publica</param>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <param name="psSello">Sello</param>
        private bool fnVerificarSello(string psLlavePublica, string psCadenaOriginal, string psSello)
        {
            byte[] abSello = null;
            bool bVerificado = false;
            try
            {
                using (OpenSSL.Core.BIO public_bio = OpenSSL.Core.BIO.File(psLlavePublica, "r"))
                {
                    abSello = Convert.FromBase64String(psSello);
                    using (OpenSSL.X509.X509Certificate cer = OpenSSL.X509.X509Certificate.FromDER(public_bio))
                    {
                        // Si existe una excepción, es porque el sello es invalido
                        try { bVerificado = MessageDigestContext.Verify(MessageDigest.SHA1, new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal)), abSello, cer.PublicKey); }
                        catch { }
                    }
                }                 
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el sello: " + ex.Message);
            }
            return bVerificado;
        }

        /// <summary>
        /// Función que se encarga de verificar el sello con la PEM del certificado
        /// </summary>
        /// <param name="psLlavePublica">Ruta Llave Publica PEM</param>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <param name="psSello">Sello</param>
        private bool fnVerificarSelloPEM(byte[] paLlavePublica, string psCadenaOriginal, string psSello)
        {
            byte[] abSello = null;
            bool bVerificado = false;
            try
            {
                using (OpenSSL.Core.BIO public_bio = new OpenSSL.Core.BIO(paLlavePublica))
                {
                    abSello = Convert.FromBase64String(psSello);
                    using (OpenSSL.X509.X509Certificate cer = new OpenSSL.X509.X509Certificate(public_bio))
                    {
                        // Si existe una excepción, es porque el sello es invalido
                        try { bVerificado = MessageDigestContext.Verify(MessageDigest.SHA1, new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal)), abSello, cer.PublicKey); }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el sello: " + ex.Message);
            }
            return bVerificado;
        }

        /// <summary>
        /// Función que se encarga de verificar el sello con la PEM del certificado
        /// </summary>
        /// <param name="psLlavePublica">Ruta Llave Publica PEM</param>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <param name="psSello">Sello</param>
        private bool fnVerificarSelloPEM(string psLlavePublica, string psCadenaOriginal, string psSello)
        {
            byte[] abSello = null;
            bool bVerificado = false;
            try
            {
                using (OpenSSL.Core.BIO public_bio = OpenSSL.Core.BIO.File(psLlavePublica, "r"))
                {
                    abSello = Convert.FromBase64String(psSello);
                    using (OpenSSL.X509.X509Certificate cer = new OpenSSL.X509.X509Certificate(public_bio))
                    {
                        // Si existe una excepción, es porque el sello es invalido
                        try { bVerificado = MessageDigestContext.Verify(MessageDigest.SHA1, new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal)), abSello, cer.PublicKey); }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el sello: " + ex.Message);
            }
            return bVerificado;
        }




        /// <summary>
        /// Función que se encarga de generar el sello con la PEM de la Llave Privada
        /// </summary>
        /// <param name="paLLavePrivada">Llave Privada</param>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <returns></returns>
        private string fnGenerarSelloPEM(byte[] paLLavePrivada, string psCadenaOriginal)
        {
            string sResultado = string.Empty;
            try
            {
                using (OpenSSL.Core.BIO private_bio = new OpenSSL.Core.BIO(paLLavePrivada))
                {
                    OpenSSL.Crypto.RSA rsa = OpenSSL.Crypto.RSA.FromPrivateKey(private_bio, null, null);

                    MessageDigest md = MessageDigest.SHA1;

                    OpenSSL.Core.BIO pbCadenaOriginal = new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal));
                    CryptoKey ck = new CryptoKey(rsa);

                    byte[] aDigestion = MessageDigestContext.Sign(md, pbCadenaOriginal, ck);

                    sResultado = Convert.ToBase64String(aDigestion);
                }
            }
            catch (Exception ex)
            { 
                throw new Exception("Error al generar el sello: " + ex.Message);
            }
            return sResultado;
        }

        /// <summary>
        /// Función que se encarga de generar el sello con la PEM de la Llave Privada
        /// </summary>
        /// <param name="psRutaLLavePrivada">Ruta Llave Privada PEM</param>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <returns></returns>
        private string fnGenerarSelloPEM(string psRutaLLavePrivada, string psCadenaOriginal)
        {
            string sResultado = string.Empty;
            try
            {
                using (OpenSSL.Core.BIO private_bio = OpenSSL.Core.BIO.File(psRutaLLavePrivada, "r"))
                {
                    OpenSSL.Crypto.RSA rsa = OpenSSL.Crypto.RSA.FromPrivateKey(private_bio, null, null);

                    MessageDigest md = MessageDigest.SHA1;

                    OpenSSL.Core.BIO pbCadenaOriginal = new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal));
                    CryptoKey ck = new CryptoKey(rsa);

                    byte[] aDigestion = MessageDigestContext.Sign(md, pbCadenaOriginal, ck);

                    sResultado = Convert.ToBase64String(aDigestion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el sello: " + ex.Message);
            }
            return sResultado;
        }




        /// <summary>
        /// Función que se encarga de generar el sello con la Llave Privada
        /// </summary>
        /// <param name="paLLavePrivada">Llave Privada</param>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <returns></returns>
        private string fnGenerarSello(byte[] paLLavePrivada, string psCadenaOriginal)
        {
            string sResultado = string.Empty;
            try
            {
                using (OpenSSL.Core.BIO private_bio = new OpenSSL.Core.BIO(paLLavePrivada))
                {
                    using (OpenSSL.X509.PKCS7 private_pk = OpenSSL.X509.PKCS7.FromDER(private_bio))
                    { 
                    
                    }


                    using (OpenSSL.Crypto.RSA rsa = OpenSSL.Crypto.RSA.FromPrivateKey(private_bio, OnPassword, null))
                    {
                        MessageDigest md = MessageDigest.SHA1;

                        OpenSSL.Core.BIO pbCadenaOriginal = new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal));
                        CryptoKey ck = new CryptoKey(rsa);
                        //CryptoKey ck = OpenSSL.Crypto.CryptoKey.FromPrivateKey(private_bio, OnPassword, null);

                        byte[] aDigestion = MessageDigestContext.Sign(md, pbCadenaOriginal, ck);

                        sResultado = Convert.ToBase64String(aDigestion);
                    }
                }

                using (OpenSSL.Crypto.DSA private_dsa = new OpenSSL.Crypto.DSA(1024, paLLavePrivada, 0, null, null))
                {
                    MessageDigest md = MessageDigest.SHA1;

                    OpenSSL.Core.BIO pbCadenaOriginal = new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal));
                    
                    using (CryptoKey ck = new CryptoKey(private_dsa))
                    {
                        byte[] aDigestion = MessageDigestContext.Sign(md, pbCadenaOriginal, ck);

                        sResultado = Convert.ToBase64String(aDigestion);
                    }                    
                }
                

        

                using (OpenSSL.Core.BIO private_bio = new OpenSSL.Core.BIO(paLLavePrivada))
                {
                    foreach (string item in OpenSSL.Crypto.Cipher.AllNames)
                    {
                        using (OpenSSL.Crypto.CipherContext cc = new OpenSSL.Crypto.CipherContext(OpenSSL.Crypto.Cipher.CreateByName(item)))
                        {
                            byte[] iv;
                            byte[] password = Encoding.UTF8.GetBytes("12345678a");
                            byte[] key = cc.BytesToKey(MessageDigest.SHA1, paLLavePrivada, password, 0, out iv);
                            byte[] output = cc.Encrypt(Encoding.UTF8.GetBytes(psCadenaOriginal), key, iv);
                            sResultado = Convert.ToBase64String(output);

                            if ("LNt79deQB2ChnpLL25WJFxIqGkAFB14MGliYwzgnxiQ8fKsD9gZkf3Mr01QVOQR3UvL9QoRqXb+EHykm+ZjUzQGaxzjVhTOnubgbfxM004NasyJiv2ILW3B6GgQ0X1azk7dL75e8rLVwhh1xFs7zIMrF7CInK2pVy7eT6d/94qk=".Equals(sResultado))
                                MessageBox.Show("Encontrado");
                        }
                    }                    

                    using(OpenSSL.Crypto.CipherContext cc = new OpenSSL.Crypto.CipherContext(OpenSSL.Crypto.Cipher.AES_256_CBC))
                    {
                        byte[] iv;
                        byte[] password = Encoding.UTF8.GetBytes("12345678a");
                        byte[] key = cc.BytesToKey(MessageDigest.SHA1, paLLavePrivada, password, 0, out iv);
                        byte[] output = cc.Encrypt(Encoding.UTF8.GetBytes(psCadenaOriginal), key, iv);
                        sResultado = Convert.ToBase64String(output);
                    }            

                    using (OpenSSL.Crypto.RSA rsa = OpenSSL.Crypto.RSA.FromPrivateKey(private_bio, OnPassword, null))
                    {
                        MessageDigest md = MessageDigest.SHA1;

                        OpenSSL.Core.BIO pbCadenaOriginal = new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal));
                        CryptoKey ck = new CryptoKey(rsa);
                        //CryptoKey ck = OpenSSL.Crypto.CryptoKey.FromPrivateKey(private_bio, OnPassword, null);

                        byte[] aDigestion = MessageDigestContext.Sign(md, pbCadenaOriginal, ck);

                        sResultado = Convert.ToBase64String(aDigestion);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el sello: " + ex.Message);
            }
            return sResultado;
        }

        /// <summary>
        /// Función que se encarga de generar el sello con la Llave Privada
        /// </summary>
        /// <param name="psRutaLLavePrivada">Ruta Llave Privada PEM</param>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <returns></returns>
        private string fnGenerarSello(string psRutaLLavePrivada, string psCadenaOriginal)
        {
            string sResultado = string.Empty;
            try
            {
                using (OpenSSL.Core.BIO private_bio = OpenSSL.Core.BIO.File(psRutaLLavePrivada, "r"))
                {
                    OpenSSL.Crypto.RSA rsa = OpenSSL.Crypto.RSA.FromPrivateKey(private_bio, OnPassword, null);

                    MessageDigest md = MessageDigest.SHA1;

                    OpenSSL.Core.BIO pbCadenaOriginal = new OpenSSL.Core.BIO(Encoding.UTF8.GetBytes(psCadenaOriginal));
                    CryptoKey ck = new CryptoKey(rsa);

                    byte[] aDigestion = MessageDigestContext.Sign(md, pbCadenaOriginal, ck);

                    sResultado = Convert.ToBase64String(aDigestion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el sello: " + ex.Message);
            }
            return sResultado;
        }

        private string OnPassword(bool verify, object userdata)
        {
            return "12345678a";
        }
    }
}

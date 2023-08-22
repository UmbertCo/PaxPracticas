using SolucionPruebas.Presentacion.WindowsForms.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmGenerarSello : Form
    {
        private byte[] gLlave;
        private byte[] gLlavePAC;
        private string gsPassword;

        X509Certificate2 certEmisor = new X509Certificate2();

        public frmGenerarSello()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDocGenera = new XmlDocument();
            String xml = string.Empty;
            String sCadenaOriginal = String.Empty;
            String sSello = String.Empty;
            String sCertificado = String.Empty;
            String nNumeroCertificado = String.Empty;
            byte[] bCertificado = null;
            try
            {

                fnObtenerCertificado();

                ofdSello.ShowDialog();
                if (ofdSello.FileName.Equals(string.Empty))
                    return;

                ofdXml.ShowDialog();
                if (ofdXml.FileName.Equals(string.Empty))
                return;

                Stream archivo = File.Open(ofdXml.FileName, FileMode.Open);
                StreamReader sr  = new StreamReader(archivo);
                xml = sr.ReadToEnd();
                archivo.Close();

                xmlDocGenera.LoadXml(xml);

                //Crear el navegador para leer el xml
                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlDocGenera.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cgd/3");
                XPathNavigator navDocGenera = xmlDocGenera.CreateNavigator();

                sCadenaOriginal = fnConstruirCadenaTimbrado(navDocGenera);

                sSello = fnCrearSello(ofdSello.FileName);
                sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());
                bCertificado = certEmisor.GetSerialNumber().Reverse().ToArray();
                nNumeroCertificado = Encoding.Default.GetString(bCertificado).ToString();

                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sSello.Replace("\n", ""));
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).SetValue(nNumeroCertificado);
                navDocGenera.SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);

                AccesoDisco.GuardarArchivoTexto(@"C:\HIDALGO\Proyectos\SolucionPruebas\SolucionPruebas.Presentacion.WindowsForms\ArchivosGenerados\", xmlDocGenera.OuterXml);
            }
            catch
            {
            
            }
        }

        public string fnCrearSello(string psRutaBin) 
        {
            String sello = String.Empty;

            //ProcessStartInfo info;
            //Process proceso;

            //info = new ProcessStartInfo("C:\OpenSSL-Win32\bin\openssl.exe", "dgst -sha1 -sign PEM\pem_key.pem -out PEM\bin.txt PEM\cadenaOriginal.txt");
            //info.CreateNoWindow = true;
            //info.UseShellExecute = false;
            //proceso = Process.Start(info);
            //proceso.WaitForExit();
            //proceso.Dispose();

            ProcessStartInfo infoSello;
            Process procesoSello;

            infoSello = new ProcessStartInfo(@"C:\OpenSSL-Win32\bin\openssl.exe", "enc -base64 -in " + psRutaBin + " -out PEM\\sello.txt");            
            infoSello.CreateNoWindow = true;
            infoSello.UseShellExecute = false;
            procesoSello = Process.Start(infoSello);
            procesoSello.WaitForExit();
            procesoSello.Dispose();

            Stream archivo = File.Open(@"PEM\sello.txt", FileMode.Open);
            StreamReader sr = new StreamReader(archivo);
            sello = sr.ReadToEnd();
            archivo.Close();

            return sello;
        }

        public String fnConstruirCadenaTimbrado(IXPathNavigable xml)
        {
            String sCadenaOriginal  = String.Empty;
            String HojaTrans  = String.Empty;

            Stream archivo = File.Open("cadenaoriginal_3_2.xslt", FileMode.Open);
            StreamReader srCadena = new StreamReader(archivo);
            HojaTrans = srCadena.ReadToEnd();
            archivo.Close();
            try
            {
                MemoryStream ms = new MemoryStream();
                XsltArgumentList args = new XsltArgumentList();
                XslCompiledTransform trans = new XslCompiledTransform();
                StreamReader sr;

                trans.Load(XmlReader.Create(new StringReader(HojaTrans)));
                trans.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                sr = new StreamReader(ms);
                sCadenaOriginal = sr.ReadToEnd();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sCadenaOriginal;
        }

        public void fnObtenerCertificado()
        {
            try
            {
                //Obtener la Llave Privada del Emisor
                string[] FileKey = null;
                string RutaKey = (String)Settings.Default.rutaCertificados + "\\";
                string filtroKey = "*.key";
                FileKey = Directory.GetFiles(RutaKey, filtroKey);
                foreach (string filekey in FileKey)
                {
                    Stream streamkey = File.Open(filekey.ToString(), FileMode.Open);
                    StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
                    using (BinaryReader br = new BinaryReader(streamkey))
                    {
                        gLlave = br.ReadBytes(Convert.ToInt32(streamkey.Length));
                    }
                }
                //Obtener la Llave Privada del Emisor

                //Obtener el Password del Certificado Emisor
                string[] FilePwd = null;
                string RutaPwd = (String)Settings.Default.rutaCertificados + "\\";
                string filtroPwd = "*.txt";
                FilePwd = Directory.GetFiles(RutaPwd, filtroPwd);

                foreach (string filePwd in FilePwd)
                {
                    using (Stream streamPwd = File.Open(filePwd.ToString(), FileMode.Open))
                    {
                        StreamReader srPwd = new StreamReader(streamPwd, System.Text.Encoding.UTF8, true);
                        gsPassword = srPwd.ReadToEnd();
                    }
                }
                //Obtener el Password del Certificado Emisor

                //Obtener el Certificado del Emisor
                string[] FilesCer = null;
                string RutaCert = (String)Settings.Default.rutaCertificados + "\\";
                string filtroCert = "*.cer";
                FilesCer = Directory.GetFiles(RutaCert, filtroCert);

                foreach (string filecer in FilesCer)
                {
                    certEmisor.Import(filecer);
                }
                //Obtener el Certificado del Emisor
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

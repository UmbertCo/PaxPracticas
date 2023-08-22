using System;                                       
using System.IO;                                    
using System.Web;                                   
using System.Xml;                                   
using System.Net;                                   
using System.Data;                                  
using System.Linq;                                  
using System.Text;                                  
using System.Drawing;                               
using System.Xml.Xsl;                               
using System.Net.Http;                              
using System.Xml.Linq;                              
using System.Xml.XPath;                             
using System.Diagnostics;                           
using System.Collections;                           
using System.ServiceModel;                          
using System.Net.Security;                          
using System.Globalization;                         
using System.Configuration;                         
using System.Windows.Forms;                         
using System.ComponentModel;                        
using System.Data.SqlClient;                        
using System.Threading.Tasks;                       
using Microsoft.WindowsAzure;                       
using System.Net.Http.Headers;                      
using System.Xml.Serialization;                     
using System.Collections.Generic;                   
using System.Security.Cryptography;                 
using System.ServiceModel.Channels;                 
using System.Collections.Specialized;               
using System.ServiceModel.Description;              
using System.Security.Cryptography.Xml;             
using Microsoft.WindowsAzure.StorageClient;         
using System.Security.Cryptography.X509Certificates;
using AutenticacionRecepcion   = ClienteWebServiceRetenciones.ServiceReference3;
using AutenticacionCancelacion = ClienteWebServiceRetenciones.ServiceReference2;
using Microsoft.Win32.SafeHandles;

namespace ClienteWebServiceRetenciones
{
    public partial class Form1 : Form
    {
        private AutenticacionCancelacion.AutenticacionClient clienteAutenticacionCancelacion;
        private AutenticacionRecepcion.AutenticacionClient clienteAutenticacion;

        public static string conCuenta       = "w4fDj8SmxJ7FlMSrxL/Fg8SAxZDvv6UK77+n77+9776o77+XNu+/rBMM77+Y77+Z7765776977+5776l77+S77+f776r77+K7767776y77+4776n77+X77+k7768Cu+/se+/uDDvv5gN77+J776277+977+877+lM++/pgjvv6bvvrbvv6Lvv4zvv40C77+HBhvvv6YF77+777+477+n77+KBgzvv6gO77+x77+4QO++l++/qhfvv5kL77+F77+YOe+/rAbvv6Tvv4gP77+t77+277+n77+A77+l77+m7762C++/tu+/qjDvv54WG++/lO+/v++/se+/szXvvrLvv7EK77+mD++/v++/sznvv5vvv57vv6/vvqfvv7/vv7zvv7k577+YGe+/mu++owrvv6fvv4c17769";
        public static string conControl      = "wozClMOTwrnElMKbwrHEjcOWw4Hvv70077+sUu+/u++/kiVAKzbvv50uDO++uO+/qO+/ue+/qgnvvrAfDu++re+/p++/u++/rw7vv4FfRO+/sx8sJe+/s+++u1JP77+gIjogEO++uzcf77+I77+xGx5F77+rWk7vv7Pvv5YeHjbvv61jRO+/sy/vv6sCQe+/nmAY77+TKEAeDu+/jWRA77+x77+WFO+/vRDvvrtgSe+/syg6JQ7vv4hSTu+/si06KzfvvrU3D++/oipAKzTvv7AiC++/rRUO77+pI++/qmAm";
        public static string conTimbrado     = "wpXCncSnw5fDocOJw6zCt8SQwqHvvrIfKC3vv4kx77+0ae+/oCEZCe+/mhfvvrci776f77+077+s77+677+cDO++tiTvvqTvv7nvv706ElLvv65V77+a77+e77+3LR0/77+xY++/le+/u++/txLvv60n77+ARO+/kzAnNRxS776lR++/kyEpPhJS77++FO++tywaO++/pjLvv7dp77+T77+5CT8OUO++pT3vvrLvv7sINRZA77+3Ve+/ki3vv68cClHvv7hr77+dMBgJ77+tEu+/uWPvv6ESHTkLUO+/pljvv50E77+oLx1T77+3Ve+/pu+/r++/pDo=";
        public static string conInicioSesion = "woHCicOkwrvDosO2xJPDkcOqxI7vv6EnE++/rO++jgMG77+oDykE77+I776f77+p77+J776h77+O77+877+X7765776h77+e77+I776j77+TAe+/qO+/ue+/lyQA77+UCe+/pu+/ou+/rO+/ohED77+iBAPvv6Lvv5HvvrLvv7nvv5Lvv4MCOBLvv7Tvv6Ek776377+GAikU77+977+XJBDvvpPvv6Y0Be+/uu++qwQJ77+oAgHvv7Tvv77vv5Mi7763776877+hA++/qO+/ue+/lxMA77+i77+wKxLvv7Tvv50e77+S77+D77++ORIC77+dIu+/u+++sO+/oe+/uhPvv7rvv6Hvv7kF77+cAC8O77+e77+TIwDvv6ILDO+/k++/ru+/oiUJ77+UFe+/t++/j++/uQ==";

        ArrayList list = new ArrayList();
        public Form1() { InitializeComponent(); }

        private HttpRequestMessageProperty AutenticaServicio()
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                clienteAutenticacion = new AutenticacionRecepcion.AutenticacionClient();
                string token = clienteAutenticacion.Autentica();

                var headerValue = string.Format("WRAP access_token=\"{0}\"", (token));
                var property = new HttpRequestMessageProperty();
                property.Method = "POST";
                property.Headers.Add(HttpRequestHeader.Authorization, headerValue);
                return property;
            } catch(Exception) { return null; }
        }

        private HttpRequestMessageProperty AutenticaServicioCancelacion()
        {
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                clienteAutenticacionCancelacion = new AutenticacionCancelacion.AutenticacionClient();
                var token = clienteAutenticacionCancelacion.Autentica();
                var headerValue = string.Format("WRAP access_token=\"{0}\"", (token));
                var property = new HttpRequestMessageProperty();
                property.Method = "POST";
                property.Headers.Add(HttpRequestHeader.Authorization, headerValue);
                return property;
            } catch (Exception) { return null; }
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void btnGenerarUUID_Click(object sender, EventArgs e)
        {
            txtUUID.Text = Guid.NewGuid().ToString();
        }

        private void btnAgregarUUID_Click(object sender, EventArgs e)
        {
            lstUUID.Items.Add(txtUUID.Text.ToUpper());
            list.Add(txtUUID.Text.ToUpper());
            lstUUID.Refresh();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            lstUUID.Items.Clear();
            list.Clear();
        }

        private void btnCertificado_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DarFormato(richTextBox1.Text);
        }

        private void btnCancelarUUID_Click(object sender, EventArgs e)
        {
            // Metodo que Firma la Cancelacion.
            RSACryptoServiceProvider KeyVal = new RSACryptoServiceProvider();
            XmlDocument refXMLDocument = new XmlDocument();
            Firmar(ref refXMLDocument, ref KeyVal, list, cmbRFC.Text, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")));

            // Envia una Peticion de Cancelacion y regresa un Folio de Seguimiento.
            string action = "api/cancelauno";
            string baseUrl = "https://cancelaretencion.cloudapp.net/";

            // Metodo de Authenticacion del WEB Service.
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object send, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            clienteAutenticacionCancelacion = new AutenticacionCancelacion.AutenticacionClient();
            string token = clienteAutenticacionCancelacion.Autentica();

            var message = new HttpRequestMessage();
            message.Headers.Add("Authorization", string.Format("WRAP access_token=\"{0}\"", token));
            message.Method = HttpMethod.Post;
            message.RequestUri = new Uri(string.Format("{0}{1}", baseUrl, action));
            message.Content = new StringContent(refXMLDocument.OuterXml, Encoding.UTF8, "application/xml");
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

            var client = new HttpClient(); client.Timeout = TimeSpan.Parse("01:30:00");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            //Llamada Asincrona del Servico
            string acuse = string.Empty;
            try
            {
                client.SendAsync(message).ContinueWith(task =>
                {
                    if (task.Result.IsSuccessStatusCode)
                    {
                        Task<String> receiveString = task.Result.Content.ReadAsStringAsync();
                        acuse = receiveString.Result;
                    }
                }).Wait();
            }
            catch { }

            // Recupera el Acuse de Cancelacion Masiva pasandole como parametro el Folio de Seguimiento.
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(acuse); XmlNode root = doc.DocumentElement;

            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            //nsmgr.AddNamespace("ns", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            //XmlNode node = root.SelectSingleNode("descendant::ns:Folio", nsmgr);

            //string folioSeguimiento = node.InnerText;
            //action = "api/consultaacuse/";
            //message = new HttpRequestMessage();
            //message.Headers.Add("Authorization", string.Format("WRAP access_token=\"{0}\"", token));
            //message.Method = HttpMethod.Get;
            //message.RequestUri = new Uri(string.Format("{0}{1}{2}", baseUrl, action, folioSeguimiento));

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            //string response = string.Empty;
            //try
            //{
            //    client.SendAsync(message).ContinueWith(task =>
            //    {
            //        if (task.Result.IsSuccessStatusCode)
            //        {
            //            Task<String> receiveString = task.Result.Content.ReadAsStringAsync();
            //            acuse = receiveString.Result;
            //        }
            //    });
            //}
            //catch { }


           //Llamada Sincrona de Servicio
           //try
           //{
           //     var response = client.SendAsync(message).Result;
           //     var result = response.Content.ReadAsStringAsync().Result;
           // }
           // catch { }

            


            //XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(response));

            //richTextBox1.Clear();

            //try
            //{
            //    while (reader.Read())
            //    {
            //        switch (reader.NodeType)
            //        {
            //            case XmlNodeType.Element:
            //                this.richTextBox1.SelectionColor = Color.Blue;
            //                this.richTextBox1.AppendText("<");
            //                this.richTextBox1.SelectionColor = Color.Brown;
            //                this.richTextBox1.AppendText(reader.Name);
            //                this.richTextBox1.SelectionColor = Color.Blue;
            //                this.richTextBox1.AppendText(">");
            //                break;

            //            case XmlNodeType.Text:
            //                this.richTextBox1.SelectionColor = Color.Black;
            //                this.richTextBox1.AppendText(reader.Value);
            //                break;

            //            case XmlNodeType.EndElement:
            //                this.richTextBox1.SelectionColor = Color.Blue;
            //                this.richTextBox1.AppendText("</");
            //                this.richTextBox1.SelectionColor = Color.Brown;
            //                this.richTextBox1.AppendText(reader.Name);
            //                this.richTextBox1.SelectionColor = Color.Blue;
            //                this.richTextBox1.AppendText(">");
            //                this.richTextBox1.AppendText("\n");
            //                break;
            //        }
            //    }
            //}
            //catch { richTextBox1.Text = response; }

            #region
            ////---------------------------------------------------------------------------------
            //XmlDocument refXMLDocument = new XmlDocument();
            //RSACryptoServiceProvider KeyVal = new RSACryptoServiceProvider();
            //SVCCANCLEACIONPRUEBAS.wcfCancelaASMXSoapClient client = new SVCCANCLEACIONPRUEBAS.wcfCancelaASMXSoapClient();
            //SVCCANCLEACIONPRUEBAS.ArrayOfString sListaUUID = new SVCCANCLEACIONPRUEBAS.ArrayOfString();
            //DataTable tabla = new DataTable();
            //tabla = buscarUsuario(txtUsuario.Text);
            //string sVarContraseña = txtPassword.Text.Trim();
            //int pid_contribuyente = Convert.ToInt32(tabla.Rows[0]["id_contribuyente"]);
            ////---------------------------------------------------------------------------------
            //if (tabla.Rows.Count > 0)
            //{
            //    try
            //    {
            //        sVarContraseña = Utilerias.Encriptacion.Base64.DesencriptarBase64(sVarContraseña);
            //    }
            //    catch (Exception) { }
            //}
            ////---------------------------------------------------------------------------------
            //foreach (String s in list)
            //{
            //    sListaUUID.Add(s);
            //}
            ////---------------------------------------------------------------------------------
            //Firmar(ref refXMLDocument, ref KeyVal, list, cmbRFC.Text, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")), pid_contribuyente);
            ////-- string strSoapMessage = refXMLDocument.OuterXml; -----------------------------
            //string strSoapMessage =
            //"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
            //    "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " + "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
            //        "<CancelaCFD xmlns=\"http://cancelacfd.sat.gob.mx\">" +
            //            "" + refXMLDocument.OuterXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").
            //                 Replace("xmlns=\"http://cancelacfd.sat.gob.mx\"", "").Replace("<?xml version=\"1.0\"?>", "") + "" +
            //        "</CancelaCFD>" +
            //    "</s:Body>" +
            //"</s:Envelope>";
            ////---------------------------------------------------------------------------------
            //ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
            //string response = client.fnCancelarXML(sListaUUID, cmbRFC.Text, strSoapMessage, txtUsuario.Text, txtPassword.Text);
            //XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(response));
            //richTextBox1.Clear();
            ////---------------------------------------------------------------------------------
            //try
            //{
            //    while (reader.Read())
            //    {
            //        switch (reader.NodeType)
            //        {
            //            //-- The node is an element -------------------------------------------
            //            case XmlNodeType.Element:
            //                this.richTextBox1.SelectionColor = Color.Blue;
            //                this.richTextBox1.AppendText("<");
            //                this.richTextBox1.SelectionColor = Color.Brown;
            //                this.richTextBox1.AppendText(reader.Name);
            //                this.richTextBox1.SelectionColor = Color.Blue;
            //                this.richTextBox1.AppendText(">");
            //                break;
            //            //-- Display the text in each element ---------------------------------
            //            case XmlNodeType.Text:
            //                this.richTextBox1.SelectionColor = Color.Black;
            //                this.richTextBox1.AppendText(reader.Value);
            //                break;
            //            //-- Display the end of the element -----------------------------------
            //            case XmlNodeType.EndElement:
            //                this.richTextBox1.SelectionColor = Color.Blue;
            //                this.richTextBox1.AppendText("</");
            //                this.richTextBox1.SelectionColor = Color.Brown;
            //                this.richTextBox1.AppendText(reader.Name);
            //                this.richTextBox1.SelectionColor = Color.Blue;
            //                this.richTextBox1.AppendText(">");
            //                this.richTextBox1.AppendText("\n");
            //                break;
            //            //---------------------------------------------------------------------
            //        }
            //    }
            //}
            //catch
            //{
            //    richTextBox1.Text = response;
            //}
            #endregion

        }

        private void button2_Click(object sender, EventArgs e)
        {
            EnviarBloqueCfdi(ClienteWebServiceRetenciones.Properties.Settings.Default.Salida,
                             ClienteWebServiceRetenciones.Properties.Settings.Default.Logs,
                             ClienteWebServiceRetenciones.Properties.Settings.Default.Acuses,
                             ClienteWebServiceRetenciones.Properties.Settings.Default.CFDI);
        }

        public string AlmacenarCfdiFramework4(byte[] cfdi, string uuid, Stream cfdiStream)
        {
            //---------------------------------------------------------------------------------
            var sharedAccessSignature = new StorageCredentialsSharedAccessSignature(ConfigurationManager.AppSettings["SharedAccesSignature"].Replace('|', '&'));
            var blobClient = new CloudBlobClient(ConfigurationManager.AppSettings["BlobStorageEndpoint"], sharedAccessSignature);
            blobClient.RetryPolicy = RetryPolicies.RetryExponential(15, TimeSpan.FromSeconds(25));
            blobClient.Timeout = TimeSpan.FromMinutes(30);
            //---------------------------------------------------------------------------------
            var blob = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]).GetBlobReference(uuid);
            BlobStream blobstream = blob.OpenWrite();
            // Primer paso, subir el contenido al blob
            // blob.Properties.CacheControl = "public, max-age=259200";
            try { blob.UploadByteArray(cfdi); }
            catch (Exception) { }
            // Definir la metadata
            blob.Metadata["versionCFDI"] = "3.2";
            // Aquí se colocará la versión del CFDI a enviar.
            // Ultimo, siempre colocar este método para enviar la información en la metadata.
            blob.SetMetadata();
            // var blobContainer = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]);
            // var blob = blobContainer.GetBlobReference(uuid);
            // if (cfdiStream.Length <= MaximumBlobSizeBeforeTransmittingAsBlocks)
            // {
            //     blob.UploadFromStream(cfdiStream);
            //     blob.Metadata["versionCFDI"] = "3.2";
            //     blob.SetMetadata();
            // }
            // else
            // {
            //     var blockBlob = blobContainer.GetBlockBlobReference(blob.Uri.AbsoluteUri);
            //     blockBlob.UploadFromStream(cfdiStream);
            //     blob.Metadata["versionCFDI"] = "3.2";
            //     blob.SetMetadata();
            // }
            return blob.Uri.AbsoluteUri;
        }

        public static DateTime GetExpiryTime(string token)
        {
            var swt = token.Substring("wrap_access_token=\"".Length, token.Length - ("wrap_access_token=\"".Length + 1));
            var tokenValue = Uri.UnescapeDataString(swt);
            var properties = (from prop in tokenValue.Split('&')
                let pair = prop.Split(new[] { '=' }, 2)
                select new { Name = pair[0], Value = pair[1] }).ToDictionary(p => p.Name, p => p.Value);
            var expiresOnUnixTicks = int.Parse(properties["ExpiresOn"]);
            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            return epochStart.AddSeconds(expiresOnUnixTicks);
        }

        public static void GuardarArchivoTexto(string rutaAbsoluta, string contenidoArchivo)
        {
            File.WriteAllText(rutaAbsoluta, contenidoArchivo);
        }

        public static void MoverArchivo(string rutaAbsoluta, string rutaDestino)
        {
            var nombreArchivo = Path.GetFileName(rutaAbsoluta);
            File.Move(rutaAbsoluta, string.Format("{0}\\{1}", rutaDestino, nombreArchivo));
        }

        public static byte[] RecuperaArchivoByte(Stream rutaAbsoluta, string ruta)
        {
            StreamReader sr = new StreamReader(rutaAbsoluta);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(sr.ReadToEnd());
        }

        public static Stream RecuperaArchivo(string rutaAbsoluta)
        {
            return File.OpenRead(rutaAbsoluta);
        }

        public static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
        {
            string sCadenaOriginal = string.Empty;

            try
            {
                MemoryStream ms = new MemoryStream();
                XslCompiledTransform trans = new XslCompiledTransform();
                    trans.Load(XmlReader.Create(new StringReader(ObtenerParamentro(psNombreArchivoXSLT))));
                XsltArgumentList args = new XsltArgumentList();
                    trans.Transform(xml, args, ms);
                    ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                    sCadenaOriginal = sr.ReadToEnd();
            } catch (Exception) { }

            return sCadenaOriginal;
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos;
        }

        private static string GetHASH(string text)
        {
            byte[] hashValue;
            byte[] message = Encoding.UTF8.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public static string fnCrearPreFirma(IList uuid, string sRfcEmisor, DateTime sfechaTimbrado)
        {
            StringBuilder sb = new StringBuilder();
                sb.Append("<?xml version=\"1.0\"?>");
                sb.Append("<Cancelacion xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" RfcEmisor=\"" +                
                sRfcEmisor + "\" Fecha=\"" + sfechaTimbrado.ToString("s") + "\" xmlns=\"http://www.sat.gob.mx/esquemas/retencionpago/1\">");
            for (int count = 0; count < uuid.Count; count++)
            {
                sb.Append("<Folios>");
                sb.Append("<UUID>" + uuid[count] + "</UUID>");
                sb.Append("</Folios>");
            }
                sb.Append("</Cancelacion>");

                return sb.ToString();
        }

        public static void FirmarXML(XmlDocument xmlDoc, X509Certificate2 cert)
        {
            try
            {
                if (xmlDoc == null) throw new ArgumentException("El argumento Doc no puede ser null.");
                if (cert == null) throw new ArgumentNullException("Key");
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
            } catch (Exception) { }
        }

        public void Firmar(ref XmlDocument retXmlDocument, ref RSACryptoServiceProvider KeyVal, IList uuid, string sRfcEmisor, DateTime sfechaTimbrado)
        {
            try
            {
                string filename = openFileDialog1.FileName;
                byte[] arrayCer = File.ReadAllBytes(filename);
                filename = openFileDialog2.FileName;
                string cerFileName = openFileDialog1.SafeFileName;
                string keyFileName = openFileDialog2.SafeFileName;
                byte[] arrayKey = File.ReadAllBytes(filename);
                string ruta = @"D:\pfx\certificados";
                string rutapfx = @"D:\pfx";
                File.WriteAllBytes(ruta + cerFileName, arrayCer);
                File.WriteAllBytes(ruta + keyFileName, arrayKey);
                string rutaCer = ruta + cerFileName;
                string rutaKey = ruta + keyFileName;
                byte[] certPFx = crearPfx(rutaCer, rutaKey, "12345678a", cerFileName, keyFileName, rutapfx);
                X509Certificate2 certificate = new X509Certificate2(certPFx, "Visa1987");
                XmlDocument documentoXML = new XmlDocument();
                documentoXML.PreserveWhitespace = false;
                DateTime time = DateTime.Now;
                string prefirma = fnCrearPreFirma(uuid, sRfcEmisor, sfechaTimbrado);
                documentoXML.LoadXml(prefirma);
                FirmarXML(documentoXML, certificate);
                retXmlDocument = documentoXML;

            } catch (Exception) { }
        }

        public byte[] crearPfx(string pathCert, string pathKey, string psPassword, string NombreCert, string NombreKey, string pathPfx)
        {
            crearCertificadoPEM(pathCert, NombreCert, pathPfx);
            crearLlavePEM(pathKey, psPassword, NombreKey, pathPfx);
            ProcessStartInfo info;
            Process proceso;
            info = new ProcessStartInfo(ObtenerParamentro("RutaOpenSSL"),
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

        public void crearLlavePEM(string pathKey, string psPassword, string NombreKey, string pathPfx)
        {
            ProcessStartInfo info;
            Process proceso;
            info = new ProcessStartInfo(ObtenerParamentro("RutaOpenSSL"),
                @"pkcs8 -inform DER -in " + pathKey + " -passin pass:" + psPassword + " -out " + pathPfx + NombreKey + ".pem");
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            proceso = Process.Start(info);
            proceso.WaitForExit();
            proceso.Dispose();
        }

        public void crearCertificadoPEM(string pathCert, string NombreCert, string pathPfx)
        {
            ProcessStartInfo info;
            Process proceso;
            info = new ProcessStartInfo(ObtenerParamentro("RutaOpenSSL"),
                  @"x509 -inform DER -in " + pathCert + " -out " + pathPfx + NombreCert + ".pem");
            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            proceso = Process.Start(info);
            proceso.WaitForExit();
            proceso.Dispose();
        }

        public static string ObtenerParamentro(string sParametro)
        {
            string nRetorno = string.Empty;
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(conControl)))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand command = new SqlCommand("usp_Ctp_BuscarParametro_Sel", conexion))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sParametro", sParametro);
                        nRetorno = Convert.ToString(command.ExecuteScalar());
                    }
                } catch (Exception) { }
                
                finally
                {
                    conexion.Close();
                }
                    return nRetorno;
            }
        }

        public DataTable buscarUsuario(string sUsuario)
        {
            DataTable gdtAuxiliar = new DataTable();
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(conInicioSesion)))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand command = new SqlCommand("usp_InicioSesion_RecuperaUsu_Sel", conexion))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                        command.ExecuteNonQuery();
                        adapter.Fill(gdtAuxiliar);
                        adapter.Dispose();
                    }
                } catch (Exception) { }
                
                finally
                {
                    conexion.Close();
                }
                    return gdtAuxiliar;
            }
        }

        public static DataTable ObtenerCertificado(int nid_rfc)
        {
            DataTable gdtAuxiliar = new DataTable();
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(conTimbrado)))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand command = new SqlCommand("usp_Timbrado_RfcCertificado_Sel", conexion))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("nid_rfc", nid_rfc);
                        command.ExecuteNonQuery();
                        adapter.Fill(gdtAuxiliar);
                        adapter.Dispose();
                    }
                } catch (Exception) { }

                finally
                {
                    conexion.Close();
                }
                    return gdtAuxiliar;
            }

        }

        public SqlDataReader fnObtenerDatosFiscales(int pid_contribuyente)
        {
            SqlDataReader reader = null;
            using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(conCuenta)))
            {
                try
                {
                    conexion.Open();
                    using (SqlCommand command = new SqlCommand("usp_Con_Cuenta_Sel", conexion))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("nId_Contribuyente", pid_contribuyente);
                        reader = command.ExecuteReader();
                    }
                } catch (Exception) { }

                finally
                {                    
                    conexion.Close();
                }
                    return reader;
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
            }
        }

        private static string CreateFormDataBoundary()
        {
            return Guid.NewGuid().ToString();
        }
        
        public void EnviarBloqueCfdi(string directorioCfdi, string directorioLog, string directorioAcuse, string directorioEnviados)
        {
            var listaArchivos = RecuperaListaArchivos(directorioCfdi);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                clienteAutenticacion = new AutenticacionRecepcion.AutenticacionClient();
                string token = clienteAutenticacion.Autentica();

                foreach (string nombreArchivo in listaArchivos)
                {
                    try
                    {
                        DateTime expirationTime = GetExpiryTime(token);

                        var baseURL = "https://servicioretencion.cloudapp.net/";
                        string action = "api/Recibe?versionEsquema=1.0";
                        var message = new HttpRequestMessage();
                        var content = new MultipartFormDataContent();

                        var filestream = new FileStream(nombreArchivo, FileMode.Open);             
                        var fileName = System.IO.Path.GetFileName(nombreArchivo);                        
                        
                        content.Add(new StreamContent(filestream), "file", fileName);

                        message.Headers.Add("Authorization", string.Format("WRAP access_token=\"{0}\"", token));
                        message.Method = HttpMethod.Post;
                        message.Content = content;
                        message.RequestUri = new Uri(baseURL + action);

                        var client = new HttpClient();
                        client.Timeout = TimeSpan.Parse("01:30:00");

                        //try
                        //{
                        //    client.SendAsync(message).ContinueWith(task =>
                        //    {
                        //        if (task.Result.IsSuccessStatusCode)
                        //        {
                        //            var result = task.Result;
                        //            string str = result.ToString();
                        //        }
                        //    });
                        //} catch (Exception) {  }

                        try
                        {
                            var result = client.SendAsync(message).Result;
                            if (result.IsSuccessStatusCode)
                            {
                                string str = result.Content.ReadAsStringAsync().Result;
                            }
                        }
                        catch { }

                        #region
                        //XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
                        //nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                        //nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                        //XPathNavigator navEncabezado = sXmlDocument.CreateNavigator();
                        //string sVersion = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
                        //string sRfcEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
                        //string sRfcReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;
                        //string sNumeroCertificado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value;
                        //string sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                        //string sFechaTFD = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;
                        //XslCompiledTransform xslt = new XslCompiledTransform();
                        //xslt.Load(typeof(CaOri.V3211));
                        //MemoryStream ms = new MemoryStream();
                        //XsltArgumentList args = new XsltArgumentList();
                        //xslt.Transform(navEncabezado, args, ms);
                        //ms.Seek(0, SeekOrigin.Begin);
                        //StreamReader srDll = new StreamReader(ms);
                        //string sCadenaOriginal = srDll.ReadToEnd();
                        //string HASH = GetHASH(sCadenaOriginal);
                        //var encabezadoCfdi = new Recepcion.EncabezadoCFDI
                        //{
                        //    RfcEmisor = sRfcEmisor,
                        //    Hash = HASH.ToUpper(),
                        //    NumeroCertificado = sNumeroCertificado,
                        //    UUID = sUUID,
                        //    Fecha = Convert.ToDateTime(sFechaTFD)
                        //};
                        //var rutaBlob = AlmacenarCfdiFramework4(StrToByteArray(sXmlDocument.InnerXml), sUUID + ".xml", contenidoArchivo);
                        //var acuseRecepcion = ClienteRecepcion.Recibe(encabezadoCfdi, rutaBlob);
                        //var acuseStream = new MemoryStream();
                        //var xmlSerializer = new XmlSerializer(typeof(Recepcion.Acuse));
                        //xmlSerializer.Serialize(acuseStream, acuseRecepcion);
                        //acuseStream.Seek(0, SeekOrigin.Begin);
                        //var acuseReader = new StreamReader(acuseStream);
                        //var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", directorioAcuse,
                        //                                      Guid.NewGuid().ToString("N"));
                        //contenidoArchivo.Close();
                        //MoverArchivo(nombreArchivo, directorioEnviados);
                        //GuardarArchivoTexto(rutaAbsolutaAcuse, acuseReader.ReadToEnd());
                        #endregion

                    } catch (Exception) {  }
                }
            } catch (Exception) {  }
        }
    }
}
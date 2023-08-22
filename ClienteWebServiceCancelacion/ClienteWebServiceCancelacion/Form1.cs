//---------------------------------------------------------------------------------------------
using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Net;
using System.Data;
using System.Linq;
using System.Text;
using Utilerias.SQL;
using System.Xml.Xsl;
using System.Drawing;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using System.ServiceModel;
using System.Net.Security;
using System.Configuration;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SqlClient;
using Microsoft.WindowsAzure;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using Microsoft.WindowsAzure.StorageClient;
using System.Security.Cryptography.X509Certificates;
using Recepcion = ClienteWebServiceCancelacion.ServicioRecepcionCFDI;
using AutenticacionRecepcion = ClienteWebServiceCancelacion.ServicioRecepcionAutenticacionCFDI;
//---------------------------------------------------------------------------------------------
namespace ClienteWebServiceCancelacion
{
    //-----------------------------------------------------------------------------------------
    public partial class Form1 : Form
    {
        //-------------------------------------------------------------------------------------
        private AutenticacionRecepcion.AutenticacionClient clienteAutenticacion;
        private Recepcion.RecibeCFDIServiceClient clienteRecepcion;
        public static InterfazSQL giSql;
        public static string conCuenta = "conConfiguracion";
        ArrayList list = new ArrayList();
        //-------------------------------------------------------------------------------------
        public Form1()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------------
        public Recepcion.RecibeCFDIServiceClient ClienteRecepcion
        {
            get
            {
                if (clienteRecepcion == null)
                {
                    GenerarClienteRecepcion();
                }
                    return clienteRecepcion;
            }
        }
        //-------------------------------------------------------------------------------------
        private void GenerarClienteRecepcion()
        {
            clienteRecepcion = new Recepcion.RecibeCFDIServiceClient();
        }
        //-------------------------------------------------------------------------------------
        private void btnGenerarUUID_Click(object sender, EventArgs e)
        {
            txtUUID.Text = Guid.NewGuid().ToString();
        }
        //-------------------------------------------------------------------------------------
        private void btnAgregarUUID_Click(object sender, EventArgs e)
        {
            lstUUID.Items.Add(txtUUID.Text.ToUpper());
            list.Add(txtUUID.Text.ToUpper());
            lstUUID.Refresh();
        }
        //-------------------------------------------------------------------------------------
        private void btnBorrar_Click(object sender, EventArgs e)
        {
            lstUUID.Items.Clear();
            list.Clear();
        }
        //-------------------------------------------------------------------------------------
        private HttpRequestMessageProperty AutenticaServicio()
        {
            //-- Aceptar certificados caducados -----------------------------------------------
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
                clienteAutenticacion = new AutenticacionRecepcion.AutenticacionClient();
            var token = clienteAutenticacion.Autentica();
            var headerValue = string.Format("WRAP access_token=\"{0}\"", HttpUtility.UrlDecode(token));
            var property = new HttpRequestMessageProperty();
                property.Method = "POST";
                property.Headers.Add(HttpRequestHeader.Authorization, headerValue);
            return property;
        }
        //-------------------------------------------------------------------------------------
        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        //-------------------------------------------------------------------------------------
        private void btnCancelarUUID_Click(object sender, EventArgs e)
        {
            //---------------------------------------------------------------------------------
            XmlDocument refXMLDocument = new XmlDocument();
            RSACryptoServiceProvider KeyVal = new RSACryptoServiceProvider();
            //SVCCANCELACIONPRUEBAS2.wcfCancelaASMXSoapClient client = new SVCCANCELACIONPRUEBAS2.wcfCancelaASMXSoapClient();
            //SVCCANCELACIONPRUEBAS2.ArrayOfString sListaUUID = new SVCCANCELACIONPRUEBAS2.ArrayOfString();
            //SVCCANCELACIONPRUEBAS_LOCAL.wcfCancelaASMXSoapClient client = new SVCCANCELACIONPRUEBAS_LOCAL.wcfCancelaASMXSoapClient();
            //SVCCANCELACIONPRUEBAS_LOCAL.ArrayOfString sListaUUID = new SVCCANCELACIONPRUEBAS_LOCAL.ArrayOfString();
            //SVCCANCLEACIONPRUEBAS.wcfCancelaASMXSoapClient client = new SVCCANCLEACIONPRUEBAS.wcfCancelaASMXSoapClient();
            //SVCCANCLEACIONPRUEBAS.ArrayOfString sListaUUID = new SVCCANCLEACIONPRUEBAS.ArrayOfString();
            DataTable tabla = new DataTable();
            try
            {
                tabla = buscarUsuario(txtUsuario.Text);
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible obtenes los datos del usuario" + ex.Message);
            }
            string sVarContraseña = txtPassword.Text.Trim();
            int pid_contribuyente = Convert.ToInt32(tabla.Rows[0]["id_contribuyente"]);
            //---------------------------------------------------------------------------------
            if (tabla.Rows.Count > 0)
            {
                try
                {
                    sVarContraseña = Utilerias.Encriptacion.Base64.DesencriptarBase64(sVarContraseña);
                }
                    catch (Exception) { }
            }
            //---------------------------------------------------------------------------------
            foreach(String s in list)
            {
                //sListaUUID.Add(s);
            }
            //---------------------------------------------------------------------------------
            Firmar(ref refXMLDocument, ref KeyVal, list, cmbRFC.Text, Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")), pid_contribuyente);
            //-- string strSoapMessage = refXMLDocument.OuterXml; -----------------------------
            string strSoapMessage =
            "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " + "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                    "<CancelaCFD xmlns=\"http://cancelacfd.sat.gob.mx\">" +
                        "" + refXMLDocument.OuterXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").
                             Replace("xmlns=\"http://cancelacfd.sat.gob.mx\"", "").Replace("<?xml version=\"1.0\"?>", "") + "" +
                    "</CancelaCFD>" +
                "</s:Body>"+ 
            "</s:Envelope>";
            //---------------------------------------------------------------------------------
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
            string response = "";//client.fnCancelarXML(sListaUUID, cmbRFC.Text, strSoapMessage, txtUsuario.Text, txtPassword.Text);
            XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(response));
            richTextBox1.Clear();
            //---------------------------------------------------------------------------------
            try
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        //-- The node is an element -------------------------------------------
                        case XmlNodeType.Element: 
                            this.richTextBox1.SelectionColor = Color.Blue;
                            this.richTextBox1.AppendText("<");
                            this.richTextBox1.SelectionColor = Color.Brown;
                            this.richTextBox1.AppendText(reader.Name);
                            this.richTextBox1.SelectionColor = Color.Blue;
                            this.richTextBox1.AppendText(">");
                            break;
                        //-- Display the text in each element ---------------------------------
                        case XmlNodeType.Text: 
                            this.richTextBox1.SelectionColor = Color.Black;
                            this.richTextBox1.AppendText(reader.Value);
                            break;
                        //-- Display the end of the element -----------------------------------
                        case XmlNodeType.EndElement: 
                            this.richTextBox1.SelectionColor = Color.Blue;
                            this.richTextBox1.AppendText("</");
                            this.richTextBox1.SelectionColor = Color.Brown;
                            this.richTextBox1.AppendText(reader.Name);
                            this.richTextBox1.SelectionColor = Color.Blue;
                            this.richTextBox1.AppendText(">");
                            this.richTextBox1.AppendText("\n");
                            break;
                        //---------------------------------------------------------------------
                    }
                }
            }
            catch
            {
                richTextBox1.Text = response;
            }
        }
        //-------------------------------------------------------------------------------------
        public void DarFormato(string texto)
        {
            richTextBox1.Clear();
            XmlTextReader reader = new XmlTextReader(new System.IO.StringReader(texto));
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    //-- The node is an element -----------------------------------------------
                    case XmlNodeType.Element:
                        this.richTextBox1.SelectionColor = Color.Blue;
                        this.richTextBox1.AppendText("<");
                        this.richTextBox1.SelectionColor = Color.Brown;
                        this.richTextBox1.AppendText(reader.Name);
                        this.richTextBox1.SelectionColor = Color.Blue;
                        this.richTextBox1.AppendText(">");
                        break;
                    //-- Display the text in each element -------------------------------------
                    case XmlNodeType.Text:
                        this.richTextBox1.SelectionColor = Color.Black;
                        this.richTextBox1.AppendText(reader.Value);
                        break;
                    //-- Display the end of the element ---------------------------------------
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
        //-------------------------------------------------------------------------------------
        public SqlDataReader fnObtenerDatosFiscales(int pid_contribuyente)
        {
            giSql = fnCrearConexion(conCuenta);
            giSql.AgregarParametro("nId_Contribuyente", pid_contribuyente);
            return giSql.Query("usp_Con_Cuenta_Sel", true);
        }
        //-------------------------------------------------------------------------------------
        public static InterfazSQL fnCrearConexion(string psNombreConexion)
        {
            try
            {
                return new InterfazSQL(psNombreConexion);
            } catch (Exception ex) 
            { 
                throw new Exception("No fue posible abrir una nueva conexión: " + ex.Message); 
            }
        }
        //-------------------------------------------------------------------------------------
        public static DataTable ObtenerCertificado(int nid_rfc)
        {
            Utilerias.SQL.InterfazSQL giSql = fnCrearConexion("conTimbrado");
            DataTable gdtAuxiliar = new DataTable();
            try
            {
                giSql.AgregarParametro("nid_rfc", nid_rfc);

                giSql.Query("usp_Timbrado_RfcCertificado_Sel", true, ref gdtAuxiliar);
            }
            catch (Exception){ }

            return gdtAuxiliar;
        }
        //-------------------------------------------------------------------------------------
        public DataTable buscarUsuario(string sUsuario)
        {
            DataTable tabla = new DataTable();
            try
            {
                Utilerias.SQL.InterfazSQL conexion = fnCrearConexion("conInicioSesion");

                conexion.Comando.CommandTimeout = 200;

                conexion.AgregarParametro("sClaveUsuario", sUsuario);

                conexion.Query("usp_InicioSesion_RecuperaUsu_Sel", true, ref tabla);
            }
            catch (Exception ex) 
            {
                throw new Exception("Error al obtener el usuario: " + ex.Message);
            }

            return tabla;
        }
        //-------------------------------------------------------------------------------------
        public static string ObtenerParamentro(string sParametro)
        {
            string nRetorno = string.Empty;
            try
            {
                Utilerias.SQL.InterfazSQL conexion = fnCrearConexion("conControl");

                conexion.AgregarParametro("sParametro", sParametro);

                nRetorno = (string)conexion.TraerEscalar("usp_Ctp_BuscarParametro_Sel", true);
            } catch (Exception) { }

            return nRetorno;
        }
        //-------------------------------------------------------------------------------------
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
        //-------------------------------------------------------------------------------------
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
        //-------------------------------------------------------------------------------------
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
        //-------------------------------------------------------------------------------------
        public void Firmar(ref XmlDocument retXmlDocument, ref RSACryptoServiceProvider KeyVal, IList uuid, string sRfcEmisor, DateTime sfechaTimbrado, int id_contribuyente)
        {
            try
            {  
                SqlDataReader sdrInfo = null;
                DataTable certificado = new DataTable();    
                // RSAKeyValue rsaY = new RSAKeyValue();
                giSql = fnCrearConexion(conCuenta);
                giSql.AgregarParametro("nId_Contribuyente", id_contribuyente);
                sdrInfo = giSql.Query("usp_Con_Cuenta_Sel", true);

                //if (sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read())
                //{
                    //certificado = ObtenerCertificado(Convert.ToInt32(sdrInfo["id_rfc"].ToString()));
                    //MessageBox.Show("Obtener certificado");
                    //byte[] pfx = (byte[])certificado.Rows[0]["pfx"];
                    //MessageBox.Show("Obtener pfx");
                    //pfx = Utilerias.Encriptacion.DES3.Desencriptar(pfx);
                    //string password = Encoding.UTF8.GetString(Utilerias.Encriptacion.DES3.Desencriptar(Convert.FromBase64String(certificado.Rows[0]["password"].ToString())));
                    //MessageBox.Show("desencriptar pfx");
                    string filename = openFileDialog1.FileName;
                    byte[] arrayCer = File.ReadAllBytes(filename);
                    filename = openFileDialog2.FileName;
                    string cerFileName = openFileDialog1.SafeFileName;
                    string keyFileName = openFileDialog2.SafeFileName;
                    byte[] arrayKey = File.ReadAllBytes(filename);
                    string ruta = ObtenerParamentro("RutaCertificados");
                    string rutapfx = ObtenerParamentro("Rutapfx");
                    File.WriteAllBytes(ruta + cerFileName, arrayCer);
                    File.WriteAllBytes(ruta + keyFileName, arrayKey);
                    string rutaCer = ruta + cerFileName;
                    string rutaKey = ruta + keyFileName;
                    byte[] certPFx = crearPfx(rutaCer, rutaKey, "12345678a", cerFileName, keyFileName, rutapfx); 
                    // fupCer.FileName, fupKey.FileName, txtPass.Text.Trim()
                    // certPFx = Utilerias.Encriptacion.DES3.Desencriptar(certPFx);
                    // clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Firma-Cargar PFX");
                    X509Certificate2 certificate = new X509Certificate2(certPFx, "12345678a");
                    MessageBox.Show("instanciar pfxc");
                    RSACryptoServiceProvider Key = (RSACryptoServiceProvider)certificate.PrivateKey;
                    MessageBox.Show("Obtener llave de la pfx");
                    KeyVal = Key;
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
                    MessageBox.Show("generar prefirma");
                    string prefirma = fnCrearPreFirma(uuid, sRfcEmisor, time);
                    // clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Firma-Crear PFX");
                    documentoXML.LoadXml(prefirma);
                    // Firme el documento XML.
                    MessageBox.Show("generar firma");
                    FirmarXML(documentoXML, certificate);
                    retXmlDocument = documentoXML;
                //}
            } catch (Exception){ }
        }
        //-------------------------------------------------------------------------------------
        public static void FirmarXML(XmlDocument xmlDoc, X509Certificate2 cert)
        {
            try
            {
                // Comprobamos los argumentos.
                if (xmlDoc == null)
                    throw new ArgumentException("El argumento Doc no puede ser null.");
                if (cert == null)
                    throw new ArgumentNullException("Key");
                // Cree un nuevo objeto SignedXml y pásele el objeto XmlDocument.
                SignedXml xmlFirmado = new SignedXml(xmlDoc);
                //***************************************************************************;
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
                xmlFirmado.SigningKey = rsa;
                //***************************************************************************
                // Agregue la clave RSA de firma al objeto SignedXml.
                // xmlFirmado.SigningKey = cert.PrivateKey;
                // Cree un objeto Reference que describa qué se debe firmar.
                // Para firmar el documento completo, establezca la propiedad Uri como "".
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
                // Agregue un objeto XmlDsigEnvelopedSignatureTransform al objeto Reference.
                // Una transformación permite al comprobador representar los datos XML
                // de idéntico modo que el firmante. Los datos XML se pueden representar de distintas maneras,
                // por lo que este paso es vital para la comprobación.
                XmlDsigEnvelopedSignatureTransform transformacionENV = new XmlDsigEnvelopedSignatureTransform();
                referencia.AddTransform(transformacionENV);
                // Agregue el objeto Reference al objetoSignedXml.
                xmlFirmado.AddReference(referencia);
                // Llame al método ComputeSignature para calcular la firma.
                xmlFirmado.ComputeSignature();
                XmlDocument xmlD = new XmlDocument();
                // a = xmlFirmado.GetXml();
                // xmlD.LoadXml(xmlFirmado.ToString());
                // Recupere la representación XML de la firma (un elemento <Signature>)
                // y guárdela en un nuevo objeto XmlElement.
                XmlElement firmaDigitalXML = xmlFirmado.GetXml();
                // Anexe el elemento al objeto XmlDocument.
                xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(firmaDigitalXML, true));
            } catch (Exception){ }
        }
        //-------------------------------------------------------------------------------------
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
        //-------------------------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //-------------------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        //-------------------------------------------------------------------------------------
        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }
        //-------------------------------------------------------------------------------------
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
        //-------------------------------------------------------------------------------------
        public static IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos;
        }
        //-------------------------------------------------------------------------------------
        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }
        //-------------------------------------------------------------------------------------
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
            } 
                catch (Exception){ }

                return sCadenaOriginal;
        }
        //-------------------------------------------------------------------------------------
        public static Stream RecuperaArchivo(string rutaAbsoluta)
        {
            return File.OpenRead(rutaAbsoluta);
        }
        //-------------------------------------------------------------------------------------
        public static byte[] RecuperaArchivoByte(Stream rutaAbsoluta, string ruta)
        {
            StreamReader sr = new StreamReader(rutaAbsoluta);
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(sr.ReadToEnd());
        }
        //-------------------------------------------------------------------------------------
        public static void MoverArchivo(string rutaAbsoluta, string rutaDestino)
        {
            var nombreArchivo = Path.GetFileName(rutaAbsoluta);
            File.Move(rutaAbsoluta, string.Format("{0}\\{1}", rutaDestino, nombreArchivo));
        }
        //-------------------------------------------------------------------------------------
        public static void GuardarArchivoTexto(string rutaAbsoluta, string contenidoArchivo)
        {
            File.WriteAllText(rutaAbsoluta, contenidoArchivo);
        }
        //-------------------------------------------------------------------------------------
        public static DateTime GetExpiryTime(string token)
        {
            var swt = token.Substring("wrap_access_token=\"".Length, token.Length - ("wrap_access_token=\"".Length + 1));
            var tokenValue = Uri.UnescapeDataString(swt);
            var properties = (from prop in tokenValue.Split('&')
                              let pair = prop.Split(new[] { '=' }, 2)
                              select new { Name = pair[0], Value = pair[1] })
                             .ToDictionary(p => p.Name, p => p.Value);
            var expiresOnUnixTicks = int.Parse(properties["ExpiresOn"]);
            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            return epochStart.AddSeconds(expiresOnUnixTicks);
        }
        //-------------------------------------------------------------------------------------
        public void EnviarBloqueCfdi(string directorioCfdi, string directorioLog, string directorioAcuse, string directorioEnviados)
        {
            // string HASH = GetHASH("||3.2|2013-09-26T15:27:20|ingreso|Pago en una sola exhibición.|2250.000000|MXN|2610.000000|No Aplica|México, Chihuahua|No Aplica|SOGJ700308AT8|IVAN HERNANDEZ|FELIPE II|8243|MARMOL III|CHIHUAHUA|Chihuahua|México|31063|No Aplica|HEPR860402CV3|RICARDO IVAN HERNANDEZ|FELIPE II|8243|MARMOL III|CHIHUAHUA|CHIHUAHUA|MEXICO|31063|1|svr|001|servicio|2250.000000|2250.000000|IVA|16.00|360.000000|360.000000||");
            // ServicioRecepcionCFDI.RecibeCFDIServiceClient recepcionn = new ServicioRecepcionCFDI.RecibeCFDIServiceClient();            
            var procesoExitoso = true;

            var listaArchivos = RecuperaListaArchivos(directorioCfdi);
            // AdministradorLogs.RegistraEntrada(string.Format("Se inicia envío de {0} CFDIs encontrados.", listaArchivos.Count));
            try
            {
                var tokenAutenticacion = AutenticaServicio();
                string token = tokenAutenticacion.Headers.ToString();
                
                using (var scope = new OperationContextScope(ClienteRecepcion.InnerChannel))
                {
                    foreach (string nombreArchivo in listaArchivos)
                    {
                        try
                        {
                            DateTime expirationTime = GetExpiryTime(token);
                            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = tokenAutenticacion;
                            var contenidoArchivo = RecuperaArchivo(nombreArchivo);
                            byte[] contenidoArchivoBytes = RecuperaArchivoByte(contenidoArchivo, nombreArchivo);
                            //AdministradorLogs.RegistraEntrada(string.Format("Se inicia envío de archivo, Tamaño: {0} bytes", contenidoArchivo.Length));
                            XmlDocument sXmlDocument = new XmlDocument();
                            sXmlDocument.Load(nombreArchivo);

                            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
                            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                            XPathNavigator navEncabezado = sXmlDocument.CreateNavigator();

                            string sVersion = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
                            string sRfcEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
                            string sRfcReceptor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;
                            string sNumeroCertificado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value;
                            string sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                            string sFechaTFD = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;
                                                         
                            XslCompiledTransform xslt = new XslCompiledTransform();
                            xslt.Load(typeof(CaOri.V3211));

                            MemoryStream ms = new MemoryStream();
                            XsltArgumentList args = new XsltArgumentList();

                            xslt.Transform(navEncabezado, args, ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            StreamReader srDll = new StreamReader(ms);

                            string sCadenaOriginal = srDll.ReadToEnd();
                            string HASH = GetHASH(sCadenaOriginal);
                            
                            //string sCadenaOriginal = fnConstruirCadenaTimbrado(navEncabezado, "cadenaoriginal_3_2");
                            //string HASH = GetHASH(sCadenaOriginal);

                            var encabezadoCfdi = new Recepcion.EncabezadoCFDI
                            {
                                RfcEmisor = sRfcEmisor,
                                Hash = HASH.ToUpper(),
                                NumeroCertificado = sNumeroCertificado,
                                UUID = sUUID,
                                Fecha = Convert.ToDateTime(sFechaTFD)
                            };
                            var rutaBlob = AlmacenarCfdiFramework4(StrToByteArray(sXmlDocument.InnerXml), sUUID + ".xml", contenidoArchivo);
                            var acuseRecepcion = ClienteRecepcion.Recibe(encabezadoCfdi, rutaBlob);
                            var acuseStream = new MemoryStream();
                            var xmlSerializer = new XmlSerializer(typeof(Recepcion.Acuse));
                            xmlSerializer.Serialize(acuseStream, acuseRecepcion);
                            acuseStream.Seek(0, SeekOrigin.Begin);
                            var acuseReader = new StreamReader(acuseStream);
                            var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", directorioAcuse,
                                                                  Guid.NewGuid().ToString("N"));
                            contenidoArchivo.Close();
                            MoverArchivo(nombreArchivo, directorioEnviados);
                            GuardarArchivoTexto(rutaAbsolutaAcuse, acuseReader.ReadToEnd());
                            //AdministradorLogs.RegistraEntrada("Archivo cargado con exito.");
                        }
                        catch (Exception ex)
                        {
                            procesoExitoso = false;
                            //AdministradorLogs.RegistraEntrada(
                            //    string.Format("Se genero un error proceso de recepción: {0}\n\n Stack Trace: {1}",
                            //                  exception.Message,
                            //                  exception.StackTrace));
                        }
                    }
                }
            }
            //---------------------------------------------------------------------------------
            catch (Exception ex)
            {
                procesoExitoso = false;
                MessageBox.Show(string.Format("Se genero un error proceso de recepción: {0}\n\n Stack Trace: {1}",
                                              ex.Message,
                                              ex.StackTrace));
                // AdministradorLogs.RegistraEntrada(string.Format("Se genero un error proceso de recepción: {0}\n\n Stack Trace: {1}", exception.Message, exception.StackTrace));
            }
            // AdministradorLogs.RegistraEntrada(procesoExitoso ? "Envío terminado sin errores." : "Envío terminado con errores.");
            // AdministradorLogs.GenerarLog(string.Format("{0}\\{1}.log", directorioLog, Guid.NewGuid().ToString("N")));
        }
        //-------------------------------------------------------------------------------------
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
            try { blob.UploadByteArray(cfdi); } catch (Exception){ }
            // Definir la metadata
            blob.Metadata["versionCFDI"] = "3.2";
            // Aquí se colocará la versión del CFDI a enviar.
            // Ultimo, siempre colocar este método para enviar la información en la metadata.
            blob.SetMetadata();
            //---------------------------------------------------------------------------------
            // var blobContainer = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]);
            // var blob = blobContainer.GetBlobReference(uuid);
            // if (cfdiStream.Length <= MaximumBlobSizeBeforeTransmittingAsBlocks)
            // {
            //     blob.UploadFromStream(cfdiStream);
            //     blob.Metadata["versionCFDI"] = "3.2";
            //     blob.SetMetadata();
            // }
            //---------------------------------------------------------------------------------
            // else
            // {
            //     var blockBlob = blobContainer.GetBlockBlobReference(blob.Uri.AbsoluteUri);
            //     blockBlob.UploadFromStream(cfdiStream);
            //     blob.Metadata["versionCFDI"] = "3.2";
            //     blob.SetMetadata();
            // }
            //---------------------------------------------------------------------------------
            return blob.Uri.AbsoluteUri;
            //---------------------------------------------------------------------------------
        }
        //-------------------------------------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            EnviarBloqueCfdi(@"C:\Prueba Cancelacion\Origen CFDI",
                             @"C:\Prueba Cancelacion\Logs",
                             @"C:\Prueba Cancelacion\Guardar Acuse",
                             @"C:\Prueba Cancelacion\Mover CFDI");
        }
        //-------------------------------------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)
        {
            DarFormato(richTextBox1.Text);
        }
        //-------------------------------------------------------------------------------------
    }
    //-----------------------------------------------------------------------------------------
}
//---------------------------------------------------------------------------------------------
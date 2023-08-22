using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Timbrado
{
    public partial class TimbradoChilkat : System.Web.UI.Page
    {
        private byte[] _abLlave;
        public byte[] gabLlave
        {
            get
            {
                return _abLlave;
            }
            set
            {
                _abLlave = value;
            }
        }

        private string _sPassword;
        public string gsPassword
        {
            get
            {
                return _sPassword;
            }
            set
            {
                _sPassword = value;
            }
        }

        private string _sAlgoritmo;
        public string gsAlgoritmo
        {
            get
            {
                return _sAlgoritmo;
            }
            set
            {
                _sAlgoritmo = value;
            }
        }

        private static Chilkat.Rsa rsa;
        private static Chilkat.PrivateKey key;
        private static Chilkat.PrivateKey pem;

        private Servicios.ServicioLocal.ServiceClient SDServicioLocal;
        private Servicios.wsRecepcionTestASMX.wcfRecepcionASMXSoapClient SDRecepcionASMX;
        Servicios.wsRecepcionDP.wcfRecepcionASMXSoapClient wsRecepcionDP;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnChilkat_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            string sCadenaOriginal = string.Empty;
            string sSello = string.Empty;
            string sSelloRuta = string.Empty;
            string sSelloPatin = string.Empty;
            string sResultado = string.Empty;
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            try
            {
                HttpFileCollection hfc = Request.Files;
                HttpPostedFile hpf = hfc[0];

                if (hpf.ContentLength < 0)
                    return;

                document.Load(hpf.InputStream);

                //document.
                SDServicioLocal = Servicios.ProxyLocator.ObtenerServicioLocal();
                nsm = new XmlNamespaceManager(document.NameTable);
                nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                fnGenerarLlave();

                sCadenaOriginal = fnConstruirCadenaTimbrado(document.CreateNavigator());


                sSello = fnGenerarSello(sCadenaOriginal);

                document.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsm).SetValue(sSello.Replace("\n", ""));

                foreach (XmlNode node in document)
                {
                    if (node.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        document.RemoveChild(node);
                    }
                }

                wsRecepcionDP = Servicios.ProxyLocator.ObtenerServicioTimbradoDP();
                sResultado = wsRecepcionDP.fnEnviarXML(document.InnerXml, "Factura", 0, "ismael.hidalgo", "Z2/CpcODw4BtworChMOrw4AU77++N8ObQjpiPEXvv7vvvqrvvozvv7sV77yd776a77+I77+Q", "3.2"); 

                //SDRecepcionASMX = Servicios.ProxyLocator.ObtenerServicioRecepcion();
                //sResultado = SDRecepcionASMX.fnEnviarXML(document.OuterXml, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");
            }
            catch (Exception ex)
            {

            }    
        }

        /// <summary>
        /// Función que genera las llaves para la generación del sello
        /// </summary>
        private void fnGenerarLlave()
        {
            //Obtener la Llave Privada del Emisor
            string[] FileKey = null;
            string RutaKey = Settings.Default.RutaArchivos;
            string filtroKey = "*.key";
            FileKey = Directory.GetFiles(RutaKey, filtroKey);
            foreach (string filekey in FileKey)
            {
                Stream streamkey = File.Open(filekey.ToString(), FileMode.Open);
                StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
                using (BinaryReader br = new BinaryReader(streamkey))
                {
                    gabLlave = br.ReadBytes(Convert.ToInt32(streamkey.Length));
                }
            }
            //Obtener la Llave Privada del Emisor

            //Obtener el Password del Certificado Emisor
            string[] FilePwd = null;
            string RutaPwd = Settings.Default.RutaArchivos;
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

            //Llave del Emisor
            //*****************************
            key = new Chilkat.PrivateKey();
            key.LoadPkcs8Encrypted(gabLlave, gsPassword);
            pem = new Chilkat.PrivateKey();
            pem.LoadPem(key.GetPkcs8Pem());
            string pkeyXml = pem.GetXml();
            //*****************************
            rsa = new Chilkat.Rsa();
            //*****************************
            bool bSuccess;
            bSuccess = rsa.UnlockComponent("INTERMRSA_78UJEvED0IwK");
            bSuccess = rsa.GenerateKey(1024);
            //*****************************
            rsa.LittleEndian = false;
            rsa.EncodingMode = "base64";
            rsa.Charset = "utf-8";
            rsa.ImportPrivateKey(pkeyXml);
            gsAlgoritmo = "sha-1";
            //*****************************
            //Llave del Emisor
        }

        /// <summary>
        /// Procedimiento que Genera el Sello del Emisor
        /// </summary>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <returns></returns>
        public string fnGenerarSello(string psCadenaOriginal)
        {
            string sello = string.Empty;
            try
            {
                sello = rsa.SignStringENC(psCadenaOriginal, gsAlgoritmo);
            }
            catch (Exception)
            {
                return null;
            }
            return sello;
        }

        /// <summary>
        /// Función que contruye la cadena original
        /// </summary>
        /// <param name="xml">Documento</param>
        /// <param name="psNombreArchivoXSLT">Nombre del archivo de tranformación</param>
        /// <returns></returns>
        private string fnConstruirCadenaTimbrado(IXPathNavigable xml)
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
    }
}
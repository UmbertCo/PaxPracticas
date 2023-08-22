using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web.Timbrado
{
    public partial class TimbradoOpenSSL : System.Web.UI.Page
    {
        private Servicios.ServicioLocal.ServiceClient SDServicioLocal;
        private Servicios.wsRecepcionTestASMX.wcfRecepcionASMXSoapClient SDRecepcionASMX;

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

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnOpenSSL_Click(object sender, EventArgs e)
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

                sCadenaOriginal = SDServicioLocal.fnAplicarHojaTransformacion(document.InnerXml);
                byte[] abLlave = fnObtenerLlave();

                sSello = SDServicioLocal.fnGenerarSelloRutas(Settings.Default.RutaPfx, sCadenaOriginal, Settings.Default.RutaLlave, Settings.Default.RutaPassword);
                sSello = sSello.Replace("\n", "");

                sSelloRuta = SDServicioLocal.fnGenerarSello(Settings.Default.RutaPfx, sCadenaOriginal, fnObtenerLlave(), "12345678a");
                sSelloRuta = sSelloRuta.Replace("\n", "");

                sSelloPatin = SDServicioLocal.fnGenerarSelloOpenSSL(@"C:\ConectorPAXMYERS\XML\Certificados\aaa010101aaa__csd_01.cer", 
                        @"C:\ConectorPAXMYERS\XML\Certificados\aaa010101aaa__csd_01.key", "12345678a", "certificado",
                        "llave", sCadenaOriginal, @"C:\ConectorPAXMYERS\XML\Certificados\RutaPfx\");
                sSelloPatin = sSelloPatin.Replace("\n", "");
                
                document.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsm).SetValue(sSello.Replace("\n", ""));

                foreach (XmlNode node in document)
                {
                    if (node.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        document.RemoveChild(node);
                    }
                }

                SDRecepcionASMX = Servicios.ProxyLocator.ObtenerServicioTimbradoTest();
                sResultado = SDRecepcionASMX.fnEnviarXML(document.OuterXml, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");
            }
            catch (Exception ex)
            {

            }          
        }
        protected void btnOpenSSLLocal_Click(object sender, EventArgs e)
        {
            fnCrearPfx(Settings.Default.RutaLlave, "12345678a", "Llave", "",Settings.Default.RutaPfx);
        }

        public string fnCrearPfx(string psRutaLlave, string psPassword, string psNombreLlave, string psCadenaOriginal, string psRutaPfx)
        {
            string sResultado = string.Empty;
            ProcessStartInfo info;
            Process proceso;
            ProcessStartInfo infoSello;
            Process procesoSello;
            string sInstruccion = string.Empty;
            string sNombreCadenaOriginal = string.Empty;
            string sArchivo = string.Empty;
            try
            {
                fnCrearLlavePrivadaPEM(psRutaLlave, psPassword, psNombreLlave, psRutaPfx);

                //sArchivo = Guid.NewGuid().ToString();

                //sNombreCadenaOriginal = psRutaPfx + "CadenaOriginal_" + sArchivo + ".txt";
                //File.WriteAllText(sNombreCadenaOriginal, psCadenaOriginal);

                ////sInstruccion = "pkcs12 -export -out " + psRutaPfx + psNombreCertificado + ".pfx -inkey " + psRutaPfx + psNombreLlave + ".pem -in " + psRutaPfx + psNombreCertificado + ".pem -passout pass:" + psPassword;
                //sInstruccion = "dgst -sha1 -sign " + psRutaPfx + psNombreLlave + ".pem -out " + psRutaPfx + "BIN_" + sArchivo + ".txt " + sNombreCadenaOriginal;

                //info = new ProcessStartInfo(VariablesGlobales.RutaOpenSSL, sInstruccion);
                //info.CreateNoWindow = true;
                //info.UseShellExecute = false;
                //proceso = Process.Start(info);
                //proceso.WaitForExit();
                //proceso.Dispose();

                //sInstruccion = "enc -base64 -in " + psRutaPfx + "BIN_" + sArchivo + ".txt -out " + psRutaPfx + "SELLO_" + sArchivo + ".txt";

                //infoSello = new ProcessStartInfo(VariablesGlobales.RutaOpenSSL, sInstruccion);
                //infoSello.CreateNoWindow = true;
                //infoSello.UseShellExecute = false;
                //procesoSello = Process.Start(infoSello);
                //procesoSello.WaitForExit();
                //procesoSello.Dispose();


                using (Stream stream = File.Open(psRutaPfx + "SELLO_" + sArchivo + ".txt", FileMode.Open))
                {
                    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                    sResultado = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            finally
            {
                if (File.Exists(psRutaPfx + psNombreLlave + ".pem"))
                    File.Delete(psRutaPfx + psNombreLlave + ".pem");

                if (File.Exists(sNombreCadenaOriginal))
                    File.Delete(sNombreCadenaOriginal);

                if (File.Exists(psRutaPfx + "BIN_" + sArchivo + ".txt"))
                    File.Delete(psRutaPfx + "BIN_" + sArchivo + ".txt");

                if (File.Exists(psRutaPfx + "SELLO_" + sArchivo + ".txt"))
                    File.Delete(psRutaPfx + "SELLO_" + sArchivo + ".txt");
            }
            return sResultado;
        }

        private string fnCrearLlavePrivadaPEM(string psRutaLlave, string psPassword, string psNombreLlave, string psRutaPfx)
        {
            Process proceso;
            ProcessStartInfo info;
            string sInstruccion = string.Empty;
            string sResultado = string.Empty;
            try
            {
                sInstruccion = "pkcs8 -inform DER -in " + psRutaLlave + " -passin pass:" + psPassword + " -out " + psRutaPfx + psNombreLlave + ".pem";
                info = new ProcessStartInfo(Settings.Default.RutaOpenSSL, sInstruccion);
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                
                proceso = Process.Start(info);
                proceso.WaitForExit();
                proceso.Dispose();

                if (File.Exists(psRutaPfx + psNombreLlave + ".pem"))
                    gabLlave = File.ReadAllBytes(psRutaPfx + psNombreLlave + ".pem");
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            return sResultado;
        }

        private byte[] fnObtenerLlave()
        {
            byte[] bLlavePrivada = null;
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
                    bLlavePrivada = br.ReadBytes(Convert.ToInt32(streamkey.Length));
                }
            }

            return bLlavePrivada;
        }        
    }
}
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using ServicioRecepcionAutenticacionCFDI;
using ServicioRecepcionCFDI;
using ServicioRecepcionCFDI32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webEnvioSAT : System.Web.UI.Page
{
    ServicioRecepcionAutenticacionCFDI.AutenticacionClient wsAutentica = new AutenticacionClient();
    ServicioRecepcionCFDI.RecibeCFDIServiceClient wsRecepcion = new ServicioRecepcionCFDI.RecibeCFDIServiceClient();
    ServicioRecepcionCFDI32.RecibeCFDIServiceClient wsRecepcion32 = new ServicioRecepcionCFDI32.RecibeCFDIServiceClient();

    private const int KB = 1024;
    private const int MB = 1024 * KB;
    private const long MaximumBlobSizeBeforeTransmittingAsBlocks = 62 * MB;        

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Unnamed1_Click(object sender, EventArgs e)
    {
        XmlDocument xdComprobante= new XmlDocument();
        try
        {
            //xdComprobante.Load(@"C:\Prueba Cancelacion\525db0b3-4fd9-4ff8-be87-6ff2fbcaea7b.xml");
            xdComprobante.Load(@"C:\Prueba Cancelacion\XML_0DC11AA5-7071-46AF-B6AB-260FA0056329.xml");

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = xdComprobante.CreateNavigator();

            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(typeof(CaOri.V3211));

            MemoryStream ms = new MemoryStream();
            XsltArgumentList args = new XsltArgumentList();

            xslt.Transform(navEncabezado, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader srDll = new StreamReader(ms);

            string sCadenaOriginal = srDll.ReadToEnd();
            string HASH = GetHASH(sCadenaOriginal);




            fnEnviarBloqueCfdi(xdComprobante);
            //fnEnviarBloqueCfdi32(xdComprobante, HASH);
        }
        catch(Exception ex)
        {
        
        }
    }

    /// <summary>
    /// Funcion encargada de enviar las facturas al SAT
    /// </summary>
    /// <param name="sCadenaOriginal"></param>
    /// <param name="sRfcEmisor"></param>
    /// <param name="sNumeroCertificado"></param>
    /// <param name="sUUID"></param>
    /// <param name="sFechaTFD"></param>
    /// <param name="idUsuario"></param>
    public string fnEnviarBloqueCfdi(XmlDocument sXmlDocument)
    {
        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        XPathNavigator navEncabezado = sXmlDocument.CreateNavigator();
        string retorno = string.Empty;
      
        string sRfcEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@Rfc", nsmComprobante).Value;
        string sNumeroCertificado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@NoCertificadoSAT", nsmComprobante).Value;
        string sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
        string sFechaTFD = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;

        var encabezadoCfdi = new ServicioRecepcionCFDI.EncabezadoCFDI
        {
            RfcEmisor = sRfcEmisor,
            VersionComprobante = "3.3",
            NumeroCertificado = sNumeroCertificado,
            UUID = sUUID,
            Fecha = Convert.ToDateTime(sFechaTFD)
        };

                try
        {
            var tokenAutenticacion = AutenticaServicio();
            using (var scope = new OperationContextScope(wsRecepcion.InnerChannel))
            {
                try
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = tokenAutenticacion;
                    var contenidoArchivo = new MemoryStream(StrToByteArray(sXmlDocument.OuterXml));
                    var rutaBlob = AlmacenarCfdiFramework433(StrToByteArray(sXmlDocument.OuterXml), sUUID + ".xml", "3.2");

                    //Respuesta Request *************************************************************

                    string strSoapMessage =
                    "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope\">" +
                    "<s:Header>" +
                    "<h:EncabezadoCFDI xmlns:h=\"http://recibecfdi.sat.gob.mx\" xmlns=\"http://recibecfdi.sat.gob.mx\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                    "<RfcEmisor>" + encabezadoCfdi.RfcEmisor + "</RfcEmisor>" +
                    "<Hash>" + encabezadoCfdi.VersionComprobante + "</Hash>" +
                    "<UUID>" + encabezadoCfdi.UUID + "</UUID>" +
                    "<Fecha>" + encabezadoCfdi.Fecha.ToString("s") + "</Fecha>" +
                    "<NumeroCertificado>" + encabezadoCfdi.NumeroCertificado + "</NumeroCertificado>" +
                    "</h:EncabezadoCFDI>" +
                    "</s:Header>" +
                    "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                    "<CFDI xmlns=\"http://recibecfdi.sat.gob.mx\">" +
                    "<RutaCFDI>" + rutaBlob + "</RutaCFDI>" +
                    "</CFDI>" +
                    "</s:Body>" +
                    "</s:Envelope>";

                    var acuseRecepcion = wsRecepcion.Recibe(encabezadoCfdi, rutaBlob);
                    var acuseStream = new MemoryStream();
                    var xmlSerializer = new XmlSerializer(typeof(ServicioRecepcionCFDI.Acuse));
                    xmlSerializer.Serialize(acuseStream, acuseRecepcion);
                    acuseStream.Seek(0, SeekOrigin.Begin);
                    var acuseReader = new StreamReader(acuseStream);
                    contenidoArchivo.Close();

                    XmlDocument docAcuseSat = new XmlDocument();
                    docAcuseSat.LoadXml(acuseReader.ReadToEnd().Replace("<?xml version=\"1.0\"?>", ""));

                    XmlNamespaceManager docComprobante = new XmlNamespaceManager(docAcuseSat.NameTable);

      
                    retorno = docAcuseSat.DocumentElement.Attributes["CodEstatus"].Value;
           
                }
                catch (Exception ex)
                {
   
                }

            }
        }
        catch (Exception ex)
        {

        }
        return retorno;
    }

    /// <summary>
    /// Funcion encargada de enviar las facturas al SAT
    /// </summary>
    /// <param name="sCadenaOriginal"></param>
    /// <param name="sRfcEmisor"></param>
    /// <param name="sNumeroCertificado"></param>
    /// <param name="sUUID"></param>
    /// <param name="sFechaTFD"></param>
    /// <param name="idUsuario"></param>
    public string fnEnviarBloqueCfdi32(XmlDocument sXmlDocument, string psHash)
    {
        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        XPathNavigator navEncabezado = sXmlDocument.CreateNavigator();
        string retorno = string.Empty;


        //string HASH = GetHASH(sCadenaOriginal);
        //string sRfcEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@Rfc", nsmComprobante).Value;
        string sRfcEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
        string sNumeroCertificado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value;
        string sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
        string sFechaTFD = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;

        var encabezadoCfdi = new ServicioRecepcionCFDI32.EncabezadoCFDI
        {
            RfcEmisor = sRfcEmisor,
            Hash = psHash,
            NumeroCertificado = sNumeroCertificado,
            UUID = sUUID,
            Fecha = Convert.ToDateTime(sFechaTFD)
        };


         try
        {
            var tokenAutenticacion = AutenticaServicio();
            using (var scope = new OperationContextScope(wsRecepcion32.InnerChannel))
            {
                try
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = tokenAutenticacion;
                    var contenidoArchivo = new MemoryStream(StrToByteArray(sXmlDocument.OuterXml));
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(idUsuario, DateTime.Now, sTitulo + "|" + "2.-fnEnviarBloqueCfdi" + "|" + string.Format("Se inicia envío de archivo, Tamaño: {0} bytes", contenidoArchivo.Length));
                    var rutaBlob = AlmacenarCfdiFramework4(StrToByteArray(sXmlDocument.OuterXml), sUUID + ".xml", "3.2");

                    //Respuesta Request *************************************************************

                    string strSoapMessage =
                    "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope\">" +
                    "<s:Header>" +
                    "<h:EncabezadoCFDI xmlns:h=\"http://recibecfdi.sat.gob.mx\" xmlns=\"http://recibecfdi.sat.gob.mx\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
                    "<RfcEmisor>" + encabezadoCfdi.RfcEmisor + "</RfcEmisor>" +
                    "<Hash>" + encabezadoCfdi.Hash + "</Hash>" +
                    "<UUID>" + encabezadoCfdi.UUID + "</UUID>" +
                    "<Fecha>" + encabezadoCfdi.Fecha.ToString("s") + "</Fecha>" +
                    "<NumeroCertificado>" + encabezadoCfdi.NumeroCertificado + "</NumeroCertificado>" +
                    "</h:EncabezadoCFDI>" +
                    "</s:Header>" +
                    "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                    "<CFDI xmlns=\"http://recibecfdi.sat.gob.mx\">" +
                    "<RutaCFDI>" + rutaBlob + "</RutaCFDI>" +
                    "</CFDI>" +
                    "</s:Body>" +
                    "</s:Envelope>";




                    var acuseRecepcion = wsRecepcion32.Recibe(encabezadoCfdi, rutaBlob);
                    var acuseStream = new MemoryStream();
                    var xmlSerializer = new XmlSerializer(typeof(ServicioRecepcionCFDI32.Acuse));
                    xmlSerializer.Serialize(acuseStream, acuseRecepcion);
                    acuseStream.Seek(0, SeekOrigin.Begin);
                    var acuseReader = new StreamReader(acuseStream);
                    contenidoArchivo.Close();

                    XmlDocument docAcuseSat = new XmlDocument();
                    docAcuseSat.LoadXml(acuseReader.ReadToEnd().Replace("<?xml version=\"1.0\"?>", ""));

                    XmlNamespaceManager docComprobante = new XmlNamespaceManager(docAcuseSat.NameTable);

                }
                catch (Exception ex)
                {
                 
                }
            }
        }
        catch (Exception ex)
        {
        
        }
        return retorno;
    }

    private HttpRequestMessageProperty AutenticaServicio()
    {
        //Aceptar certificados caducados
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

        var token = wsAutentica.Autentica();
        var headerValue = string.Format("WRAP access_token=\"{0}\"", HttpUtility.UrlDecode(token));
        var property = new HttpRequestMessageProperty();
        property.Method = "POST";
        property.Headers.Add(HttpRequestHeader.Authorization, headerValue);
        return property;
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
        }
        return hex;
    }

    public static byte[] StrToByteArray(string str)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        return encoding.GetBytes(str);
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    public string AlmacenarCfdiFramework4(byte[] cfdi, string uuid, string sversion)
    {
        var sharedAccessSignature = new StorageCredentialsSharedAccessSignature(ConfigurationManager.AppSettings["SharedAccesSignature"].Replace('|', '&'));
        var blobClient = new CloudBlobClient(ConfigurationManager.AppSettings["BlobStorageEndpoint"],
                                             sharedAccessSignature);
        blobClient.RetryPolicy = RetryPolicies.RetryExponential(15, TimeSpan.FromSeconds(25));
        blobClient.Timeout = TimeSpan.FromMinutes(30);
        var blobContainer = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]);
        //var blob = blobContainer.GetBlobReference(uuid);

        var blob = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]).GetBlobReference(uuid);

        //Primer paso, subir el contenido al blob
        blob.UploadByteArray(cfdi);

        //Definir la metadata
        blob.Metadata["versionCFDI"] = "3.2"; //"3.2";  //Aquí se colocará la versión del CFDI a enviar.

        //Ultimo, siempre colocar este método para enviar la información en la metadata.
        blob.SetMetadata();

        //if (cfdi.Length <= MaximumBlobSizeBeforeTransmittingAsBlocks)
        //{                
        //    blob.UploadFromStream(cfdi);
        //}
        //else
        //{
        //    var blockBlob = blobContainer.GetBlockBlobReference(blob.Uri.AbsoluteUri);
        //    blockBlob.UploadFromStream(cfdi);
        //}

        return blob.Uri.AbsoluteUri;
    }

    public string AlmacenarCfdiFramework433(byte[] cfdi, string uuid, string sversion)
    {
        var sharedAccessSignature = new StorageCredentialsSharedAccessSignature(ConfigurationManager.AppSettings["SharedAccesSignature"].Replace('|', '&'));
        var blobClient = new CloudBlobClient(ConfigurationManager.AppSettings["BlobStorageEndpoint"],
                                             sharedAccessSignature);
        blobClient.RetryPolicy = RetryPolicies.RetryExponential(15, TimeSpan.FromSeconds(25));
        blobClient.Timeout = TimeSpan.FromMinutes(30);
        var blobContainer = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]);
        //var blob = blobContainer.GetBlobReference(uuid);

        var blob = blobClient.GetContainerReference(ConfigurationManager.AppSettings["ContainerName"]).GetBlobReference(uuid);

        //Primer paso, subir el contenido al blob
        blob.UploadByteArray(cfdi);

        //Definir la metadata
        blob.Metadata["versionCFDI"] = "3.3"; //"3.2";  //Aquí se colocará la versión del CFDI a enviar.

        //Ultimo, siempre colocar este método para enviar la información en la metadata.
        blob.SetMetadata();

        //if (cfdi.Length <= MaximumBlobSizeBeforeTransmittingAsBlocks)
        //{                
        //    blob.UploadFromStream(cfdi);
        //}
        //else
        //{
        //    var blockBlob = blobContainer.GetBlockBlobReference(blob.Uri.AbsoluteUri);
        //    blockBlob.UploadFromStream(cfdi);
        //}

        return blob.Uri.AbsoluteUri;
    }               
}
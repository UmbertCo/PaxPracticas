﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.XPath;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;


//Web Service SAT******************************************************
using AutenticacionCancelacion = ServicioCancelacionAutenticacionCFDI; //ServicioCancelacionAutenticacionCFDI;
using Cancelacion = ServicioCancelacionCFDI;//ServicioCancelacionCFDI;
using SAT.CFDI.Cliente.Procesamiento;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Web.Services;
using System.ServiceModel.Activation;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Addressing;
using Microsoft.Web.Services3.Messaging;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Configuration;


/// <summary>
/// Summary description for wcfCancelaCFDI
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class clsCancelacionSAT
{
    private clsOperacionTimbradoSellado gTimbrado;
    private clsValCertificado gCertificado;
    protected DataTable tablePas;
    public string conCuenta = "conConfiguracion";
    private string uuid = "00000000-0000-0000-0000-000000000000";
    string RespuestaFinal = string.Empty;
    //************************************************************************************************************
    //Campos servicios SAT
    #region Campos
    
    private Cancelacion.CancelaCFDBindingClient clienteCancelacion;
    private AutenticacionCancelacion.AutenticacionClient clienteAutenticacionCancelacion;
    private AccesoAlmacenBlob clienteAlmacenBlob;
    #endregion

    #region Propiedades
    
    public Cancelacion.CancelaCFDBindingClient ClienteCancelacion
    {
        get
        {
            if (clienteCancelacion == null)
            {
                GenerarClienteCancelacion();
            }

            return clienteCancelacion;
        }
    }
   

    public AutenticacionCancelacion.AutenticacionClient ClienteAutenticacionCancelacion
    {
        get
        {
            if (clienteAutenticacionCancelacion == null)
            {
                GenerarClienteAutenticacionCancelacion();
            }

            return clienteAutenticacionCancelacion;
        }
    }

    public AccesoAlmacenBlob ClienteAlmacenBlob
    {
        get
        {
            if (clienteAlmacenBlob == null)
            {
                GenerarClienteAlmacenBlob();
            }

            return clienteAlmacenBlob;
        }
    }
    #endregion

    #region Métodos Públicos
    
    /// <summary>
    /// Create a soap webrequest to [Url]
    /// </summary>
    /// <returns></returns>
    public HttpWebRequest CreateWebRequest()
    {
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsCancelacionSAT.AcceptAllCertificatePolicy);
        var token = ClienteAutenticacionCancelacion.Autentica();
        var headerValue = string.Format("WRAP access_token=\"{0}\"", HttpUtility.UrlDecode(token));
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://pruebacfdicancelacion.cloudapp.net/Cancelacion/CancelaCFDService.svc");
        return webRequest;
    }

    public byte[] stringToBase64ByteArray(String input)
    {
        byte[] ret = System.Text.Encoding.Unicode.GetBytes(input);
        string s = Convert.ToBase64String(ret);
        ret = System.Text.Encoding.Unicode.GetBytes(s);
        return ret;
    }

    public string CancelarBloqueCfdi(string RfcEmi, ArrayList listaUuid, clsInicioSesionUsuario datosUsuario, ref DataTable TableUUID, byte[] pfx, string pfxPassword)
    {
        //------------------------------------------------------------------------------------
        //XmlDocument XmlSignature = xmlsignature;
        //------------------------------------------------------------------------------------
        ArrayList listaUuidRes = new ArrayList();
        //------------------------------------------------------------------------------------
        string Respuesta = string.Empty;
        //------------------------------------------------------------------------------------
        for (int coun = 0; coun < listaUuid.Count; coun++)
        {
            string UUID = listaUuid[coun].ToString();
            listaUuidRes.Add(UUID);
        }
        //------------------------------------------------------------------------------------
        try
        {
            if (listaUuidRes.Count == 0)
            {
                //----------------------------------------------------------------------------
                {
                    throw new System.ArgumentNullException();
                }
                //----------------------------------------------------------------------------
            }
        }
        //------------------------------------------------------------------------------------
        catch (System.ArgumentNullException)
        {
            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, uuid, Respuesta, Convert.ToDateTime(DateTime.Now), "999", "Response", "C", string.Empty);
            Respuesta = fnListaUUID(TableUUID, RfcEmi, ":signature");
            return Respuesta;
        }
        //------------------------------------------------------------------------------------
        var mensajeCancelacion = new Cancelacion.Cancelacion
        {
            RfcEmisor = RfcEmi,
            Fecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"))
        };
        //------------------------------------------------------------------------------------
        XmlDocument docXmlDocument = new XmlDocument();
        RSACryptoServiceProvider key = new RSACryptoServiceProvider();
        //------------------------------------------------------------------------------------
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Antes de firma");
        Firmar(ref docXmlDocument, ref  key, listaUuidRes, mensajeCancelacion.RfcEmisor, mensajeCancelacion.Fecha, datosUsuario, pfx, pfxPassword);
        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Despues de firma");

        //------------------------------------------------------------------------------------
        string strSoapMessage =
            "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
            "<s:Body xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
            "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
            "<CancelaCFD xmlns=\"http://cancelacfd.sat.gob.mx\">" +
            "" + docXmlDocument.OuterXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "").Replace("xmlns=\"http://cancelacfd.sat.gob.mx\"", "").Replace("<?xml version=\"1.0\"?>", "") + "" +
            "</CancelaCFD>" +
            "</s:Body></s:Envelope>";
        //------------------------------------------------------------------------------------
        clsComun.fnInsertaAcuseSAT(datosUsuario.id_contribuyente.ToString(), listaUuidRes[0].ToString(), strSoapMessage, DateTime.Now, "0", "Request", "C");
        //------------------------------------------------------------------------------------
        XmlNodeList nDigestValue;
        XmlNodeList nSignatureValue;
        XmlNodeList nX509IssuerName;
        XmlNodeList nX509SerialNumber;
        XmlNodeList nX509Certificate;
        XmlNamespaceManager nsmComprobante = null;
        //------------------------------------------------------------------------------------
        string sDigestValue = string.Empty;
        string sSignatureValue = string.Empty;
        string sX509IssuerName = string.Empty;
        string sX509SerialNumber = string.Empty;
        string sX509Certificate = string.Empty;
        //------------------------------------------------------------------------------------
        XmlNodeList personas = docXmlDocument.GetElementsByTagName("Cancelacion");
        XmlNodeList lista = ((XmlElement)personas[0]).GetElementsByTagName("Signature");
        foreach (XmlElement nodo in lista)
        {
            //--------------------------------------------------------------------------------
            nDigestValue = nodo.GetElementsByTagName("DigestValue");
            nSignatureValue = nodo.GetElementsByTagName("SignatureValue");
            nX509IssuerName = nodo.GetElementsByTagName("X509IssuerName");
            nX509SerialNumber = nodo.GetElementsByTagName("X509SerialNumber");
            nX509Certificate = nodo.GetElementsByTagName("X509Certificate");
            //--------------------------------------------------------------------------------
            sDigestValue = nDigestValue[0].InnerText;
            sSignatureValue = nSignatureValue[0].InnerText;
            sX509IssuerName = nX509IssuerName[0].InnerText;
            sX509SerialNumber = nX509SerialNumber[0].InnerText;
            sX509Certificate = nX509Certificate[0].InnerText;
            //--------------------------------------------------------------------------------
        }
        //------------------------------------------------------------------------------------
        string[] varRSA = key.ToXmlString(false).Split('<');
        //------------------------------------Armar SOAP--------------------------------------
        byte[] utfBytes = System.Convert.FromBase64String(sSignatureValue);
        byte[] bytesDigest = System.Convert.FromBase64String(sDigestValue);
        byte[] bytesCertificate = System.Convert.FromBase64String(sX509Certificate);
        //------------------------------------------------------------------------------------
        Cancelacion.TransformType[] Trans1 = new Cancelacion.TransformType[1];
        Cancelacion.TransformType Trans = new Cancelacion.TransformType();
        Cancelacion.CanonicalizationMethodType CMT = new Cancelacion.CanonicalizationMethodType();
        Cancelacion.DigestMethodType DMT = new Cancelacion.DigestMethodType();
        Cancelacion.KeyInfoType KIT = new Cancelacion.KeyInfoType();
        Cancelacion.KeyValueType KVT = new Cancelacion.KeyValueType();
        Cancelacion.ObjectType[] objAr = new Cancelacion.ObjectType[1];
        Cancelacion.ObjectType obj = new Cancelacion.ObjectType();
        Cancelacion.ReferenceType RT = new Cancelacion.ReferenceType();
        Cancelacion.RSAKeyValueType RSAKVT = new Cancelacion.RSAKeyValueType();
        Cancelacion.SignatureMethodType SMT = new Cancelacion.SignatureMethodType();
        Cancelacion.SignatureType signature = new Cancelacion.SignatureType();
        Cancelacion.SignedInfoType SIT = new Cancelacion.SignedInfoType();
        Cancelacion.X509DataType X509DT = new Cancelacion.X509DataType();
        Cancelacion.X509IssuerSerialType X509I = new Cancelacion.X509IssuerSerialType();
        //------------------------------------------------------------------------------------
        SMT.Algorithm = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
        CMT.Algorithm = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
        DMT.Algorithm = "http://www.w3.org/2000/09/xmldsig#sha1";
        Trans.Algorithm = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";
        Trans1[0] = Trans;
        //------------------------------------------------------------------------------------
        RT.DigestMethod = DMT;
        RT.DigestValue = bytesDigest;
        RT.URI = "";
        RT.Transforms = Trans1;
        //------------------------------------------------------------------------------------
        X509I.X509IssuerName = sX509IssuerName;
        X509I.X509SerialNumber = sX509SerialNumber;
        //------------------------------------------------------------------------------------
        X509DT.X509IssuerSerial = X509I;
        X509DT.X509Certificate = bytesCertificate;
        KIT.X509Data = X509DT;
        SIT.CanonicalizationMethod = CMT;
        SIT.SignatureMethod = SMT;
        SIT.Reference = RT;
        signature.KeyInfo = KIT;
        signature.SignedInfo = SIT;
        signature.SignatureValue = utfBytes;
        mensajeCancelacion.Signature = signature;

        //------------------------------------------------------------------------------------
        var listaFolios = (from string uuid in listaUuidRes select new Cancelacion.CancelacionFolios { UUID = uuid }).ToList();
        mensajeCancelacion.Folios = listaFolios.ToArray();
        //------------------------------------------------------------------------------------
        try
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Antes de envia SAT");
            //--------------------------------------------------------------------------------
            var tokenAutenticacion = AutenticaServicioCancelacion();
            using (var scope = new OperationContextScope(ClienteCancelacion.InnerChannel))
            {
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Antes de envia SAT");
                //----------------------------------------------------------------------------
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = tokenAutenticacion;
                var acuseCancelacion = ClienteCancelacion.CancelaCFD(mensajeCancelacion);
                var acuseStream = new MemoryStream();
                var xmlSerializer = new XmlSerializer(typeof(Cancelacion.Acuse));
                xmlSerializer.Serialize(acuseStream, acuseCancelacion);
                acuseStream.Seek(0, SeekOrigin.Begin);
                var acuseReader = new StreamReader(acuseStream);
                XmlDocument xmlAcuse = new XmlDocument();
                xmlAcuse.Load(acuseReader);
                //----------------------------------------------------------------------------
                nsmComprobante = new XmlNamespaceManager(xmlAcuse.NameTable);
                nsmComprobante.AddNamespace("sig", "http://www.w3.org/2000/09/xmldsig#");
                string signatureValue = xmlAcuse.CreateNavigator().SelectSingleNode("/Acuse/sig:Signature/sig:SignatureValue", nsmComprobante).InnerXml;
                //----------------------------------------------------------------------------
                string codStatus = null;
                string fechaAcuse = xmlAcuse.DocumentElement.Attributes["Fecha"].Value;
                XmlNodeList list = null;
                codStatus = "";
                string status = null;
                string uuid = null;
                string errorsat = null;
                //----------------------------------------------------------------------------
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Despues de envia SAT");
                try
                {
                    //------------------------------------------------------------------------
                    codStatus = xmlAcuse.DocumentElement.Attributes["CodEstatus"].Value;
                    int nAcuse = clsComun.fnInsertaAcuseSAT(datosUsuario.id_contribuyente.ToString(),
                                          listaFolios[0].UUID, xmlAcuse.OuterXml,
                                          Convert.ToDateTime(fechaAcuse), codStatus, "Response", "C");
                    //------------------------------------------------------------------------
                    return fnRespuesta(listaUuidRes[0].ToString(), RfcEmi, codStatus, clsComun.fnRecuperaErrorSAT(codStatus, "Firma"), ref TableUUID);
                    //------------------------------------------------------------------------
                }
                //----------------------------------------------------------------------------
                catch (Exception)
                {
                    //------------------------------------------------------------------------
                    list = xmlAcuse.GetElementsByTagName("Folios");
                    foreach (XmlNode xn in list)
                    {
                        //--------------------------------------------------------------------
                        status = xn["EstatusUUID"].InnerText;
                        uuid = xn["UUID"].InnerText;
                        errorsat = clsComun.fnRecuperaErrorSAT(status, "Cancelación");
                        //--------------------------------------------------------------------
                        ActualizarRenglonUUID(TableUUID, uuid, status, errorsat, ref datosUsuario);
                        //--------------------------------------------------------------------
                    }
                    //------------------------------------------------------------------------
                    int nAcuse = clsComun.fnInsertaAcuseSAT(datosUsuario.id_contribuyente.ToString(),
                                          list[0]["UUID"].InnerText, xmlAcuse.OuterXml,
                                          Convert.ToDateTime(fechaAcuse), list[0]["EstatusUUID"].InnerText, "Response", "C");
                    //------------------------------------------------------------------------
                }
                //----------------------------------------------------------------------------
                //Guardar el acuse en un xmldocument para insertarlo en la base de datos
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Cancelacion CFDI" + "|" + "Revisando acuses." + " CFDIs");
                Respuesta = fnListaUUID(TableUUID, RfcEmi, signatureValue);
                //----------------------------------------------------------------------------
            }
            //--------------------------------------------------------------------------------
        }
        //------------------------------------------------------------------------------------
        catch (Exception ex)
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Cancelacion CFDI" + "|" + "Se genero un error en el proceso de cancelación:" + ex.Message);
            ActualizarRenglonUUID(TableUUID, listaUuidRes[0].ToString(), "999", "- No hay respuesta del SAT, favor de intentar de nuevo.", ref datosUsuario);
            Respuesta = fnListaUUID(TableUUID, RfcEmi, "signature:");
        }
        //------------------------------------------------------------------------------------
        return Respuesta;
        //------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------
    public void ActualizarRenglonUUID(DataTable tabla, String UUID, String Estatus, String Descripcion, ref clsInicioSesionUsuario datosUsuario)
    {
        //------------------------------------------------------------------------------------
        DataRow foundRow = tabla.Rows.Find(UUID);
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        foundRow.BeginEdit();
        foundRow.SetField("estatus_sat", Estatus);
        foundRow.SetField("fecha", DateTime.Now.ToString("s"));
        foundRow.SetField("descripcion", Descripcion);
        foundRow.EndEdit();
        foundRow.AcceptChanges();
        //------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------
    public string fnRespuesta(string UUID, string psRFC, string Estatus, string descripcion, ref DataTable TableUUID)
    {
        //------------------------------------------------------------------------------------
        try
        {
            //--------------------------------------------------------------------------------
            TableUUID.Clear();
            DataRow nuevo = TableUUID.NewRow();
            nuevo["UUID"] = UUID;
            nuevo["estatus"] = Estatus;
            nuevo["descripcion"] = descripcion;
            nuevo["fecha"] = DateTime.Now.ToString("s");
            TableUUID.Rows.Add(nuevo);
            RespuestaFinal = fnListaUUID(TableUUID, psRFC, ":no-signature");
            //--------------------------------------------------------------------------------
        }
        //------------------------------------------------------------------------------------
        catch
        {
            //--------------------------------------------------------------------------------
            StringBuilder sb = new StringBuilder();
            sb.Append("<Cancelacion xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" RfcEmisor=\"" +
                      psRFC + "\" Fecha=\"" + DateTime.Now.ToString("s") + "\" xmlns=\"http://cancelacfd.sat.gob.mx\">");
            sb.Append("<Folios>");
            sb.Append("<UUID>" + UUID + " " + "</UUID>");
            sb.Append("<UUIDEstatus>" + Estatus + "</UUIDEstatus>");
            sb.Append("<UUIDdescripcion>" + descripcion + "</UUIDdescripcion>");
            sb.Append("<UUIDfecha>" + DateTime.Now + "</UUIDfecha>");
            sb.Append("</Folios>");
            sb.Append("<Signature>" + ":no-signature" + "</Signature>");
            sb.Append("</Cancelacion>");
            RespuestaFinal = sb.ToString();
            //--------------------------------------------------------------------------------
        }
        //------------------------------------------------------------------------------------
        return RespuestaFinal;
        //------------------------------------------------------------------------------------
    }
    #endregion

    #region Métodos Privados
    private HttpRequestMessageProperty AutenticaServicioCancelacion()
    {
        //Aceptar certificados caducados
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsCancelacionSAT.AcceptAllCertificatePolicy);

        var token = ClienteAutenticacionCancelacion.Autentica();
        var headerValue = string.Format("WRAP access_token=\"{0}\"", HttpUtility.UrlDecode(token));
        var property = new HttpRequestMessageProperty();
        property.Method = "POST";
        property.Headers.Add(HttpRequestHeader.Authorization, headerValue);
        return property;
    }

    private void GenerarClienteAlmacenBlob()
    {
        clienteAlmacenBlob = new AccesoAlmacenBlob();
    }

    private void GenerarClienteCancelacion()
    {
        clienteCancelacion = new Cancelacion.CancelaCFDBindingClient();
    }

    private void GenerarClienteAutenticacionCancelacion()
    {
        clienteAutenticacionCancelacion = new AutenticacionCancelacion.AutenticacionClient();
    }

    #endregion

    public RSAKeyValue rsaG = new RSAKeyValue();

    private static bool ValidarCertificadoRemoto(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
    {
        return true;
    }

    public static string fnRecuperaMatriz(int pnId_Usuario)
    {
        try
        {
            string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Obtener_Matriz_Sel", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 200;
                    cmd.Parameters.AddWithValue("nId_Usuario", pnId_Usuario);
                    con.Open();
                    string retVal = cmd.ExecuteScalar().ToString();
                    con.Close();

                    if (retVal == "0")
                        return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
                    else
                        return retVal;
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante", pnId_Usuario);
            return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
        }
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
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

    public static byte[] StrToByteArray(string str)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        return encoding.GetBytes(str);
    }

    private static string MakeRequest(string uri, string method, WebProxy proxy)
    {
        var webRequest = (HttpWebRequest)WebRequest.Create(uri);
        webRequest.AllowAutoRedirect = true;
        webRequest.Method = method;
        ServicePointManager.ServerCertificateValidationCallback += ValidarCertificadoRemoto;
        if (proxy != null)
        {
            webRequest.Proxy = proxy;
        }

        HttpWebResponse response = null;
        try
        {
            response = (HttpWebResponse)webRequest.GetResponse();

            using (var s = response.GetResponseStream())
            {
                using (var sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        finally
        {
            if (response != null)
                response.Close();
        }
    }

    public static void Firmar(ref XmlDocument retXmlDocument, ref RSACryptoServiceProvider KeyVal, IList uuid, string sRfcEmisor, DateTime sfechaTimbrado, clsInicioSesionUsuario datosUsuario, byte[] pfx, String pfxPassword)
    {
        try
        {
            DataTable dtInfo = new DataTable();
            RSAKeyValue rsaY = new RSAKeyValue();

            clsCancelacionSAT cancela = new clsCancelacionSAT();
            dtInfo = cancela.fnObtenerDatosFiscales(datosUsuario.id_contribuyente);

            if (dtInfo.Rows.Count > 0)
            {
                string pfx2 = System.Text.Encoding.Default.GetString(pfx);
                byte[] pfx3 = System.Convert.FromBase64String(pfx2);

                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Firma-Cargar PFX");

                X509Certificate2 certificate = new X509Certificate2(pfx3, pfxPassword);
                RSACryptoServiceProvider Key = (RSACryptoServiceProvider)certificate.PrivateKey;
                KeyVal = Key;

                ////////////////////////////CAMBIAR POR PFX ENVIADO//////////////////////////////////////
                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Firma-Recupera Key");

                // Cree un objeto XmlDocument; para ello, cargue un archivo XML de disco.
                // El objeto XmlDocument contiene el elemento XML que se debe cifrar.
                XmlDocument documentoXML = new XmlDocument();

                // Format the document to ignore white spaces.
                documentoXML.PreserveWhitespace = false;
                //se manda prefirmar el documento
                string prefirma = fnCrearPreFirma(uuid, sRfcEmisor, sfechaTimbrado);

                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Firma-Crear PFX");

                documentoXML.LoadXml(prefirma);

                // Firme el documento XML.
                FirmarXML(documentoXML, certificate);

                retXmlDocument = documentoXML;
            }
        }
        catch (Exception ex)
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "Firma: " + ex.Message);
        }
    }
    //generacion de la prefirma del acuse de envio de cancelacion del SAT
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
           
            // Recupere la representación XML de la firma (un elemento <Signature>)
            // y guárdela en un nuevo objeto XmlElement.
            XmlElement firmaDigitalXML = xmlFirmado.GetXml();
            // Anexe el elemento al objeto XmlDocument.
            xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(firmaDigitalXML, true));
        }
        catch (Exception ex)
        {
            clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, "GeneralFirmarXML: " + ex.Message);
        }
    }

    public byte[] fbCrearPfx(Byte[] sPathCER, Byte[] sPathKEY, string sClave)
    {
        bool success;
        byte[] resultado = { };
        Chilkat.Cert cert = new Chilkat.Cert();
        Chilkat.PrivateKey key = new Chilkat.PrivateKey();
        success = cert.LoadFromBinary(sPathCER);
        if (success)
        {
            success = key.LoadPkcs8Encrypted(sPathKEY, sClave);
            if (success)
            {
                cert.SetPrivateKey(key);
            }
        }
        if (success)
        {

            resultado = Utilerias.Encriptacion.DES3.Encriptar(cert.ExportCertDer());
        }
        return resultado.ToArray();

    }

    public void crearCertificadoPEM(string pathCert, string NombreCert, string pathPfx)
    {
        ProcessStartInfo info;
        Process proceso;
        info = new ProcessStartInfo(clsComun.ObtenerParamentro("RutaOpenSSL"),
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

        info = new ProcessStartInfo(clsComun.ObtenerParamentro("RutaOpenSSL"),
            @"pkcs8 -inform DER -in " + pathKey + " -passin pass:" + psPassword + " -out " + pathPfx + NombreKey + ".pem");
        info.CreateNoWindow = true;
        info.UseShellExecute = false;
        proceso = Process.Start(info);
        proceso.WaitForExit();
        proceso.Dispose();
    }

    public byte[] crearPfx(string pathCert, string pathKey, string psPassword, string NombreCert, string NombreKey, string pathPfx)
    {
        crearCertificadoPEM(pathCert, NombreCert, pathPfx);
        crearLlavePEM(pathKey, psPassword, NombreKey, pathPfx);
        ProcessStartInfo info;
        Process proceso;

        info = new ProcessStartInfo(clsComun.ObtenerParamentro("RutaOpenSSL"),
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

    /// <summary>
    /// Retorna los datos fiscales de la sucursal matriz
    /// </summary>
    /// <returns>Retorna un SqlDataReader con los datos fiscales de la matriz</returns>
    public DataTable fnObtenerDatosFiscales(int pid_contribuyente)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Cuenta_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Contribuyente", pid_contribuyente));
                    
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }

    //metodo para recuperar el identificador del UUID en la base de datos
    public int fnRecuperaIdCFD(string psUUID, int nId_usuario)
    {
        Int32 nResultado = 0;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_RecuperaComprobanteUUID_USU_sel_Indicium", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nUUID", psUUID));
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nId_usuario));

                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return nResultado;
    }

    //metodo para recuperar el estatus del UUID en la base de datos
    public string fnRecuperaEstatusCFD(string psUUID, int nId_usuario)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_RecuperaEstatusComprobanteUUID_USU_sel_Indicium", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nUUID", psUUID));
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nId_usuario));
                    con.Open();
                    sResultado = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return sResultado;
    }

    // genera el response que se le enviara al cliente
    public string fnListaUUID(DataTable uuid, string sRfcEmisor, string signatureValue)
    {
        //------------------------------------------------------------------------------------
        StringBuilder sb = new StringBuilder();
        sb.Append("<Cancelacion xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" RfcEmisor=\"" +
            sRfcEmisor + "\" Fecha=\"" + DateTime.Now.ToString("s") + "\" xmlns=\"http://cancelacfd.sat.gob.mx\">");
        //------------------------------------------------------------------------------------
        foreach (DataRow renglon in uuid.Rows)
        {
            //--------------------------------------------------------------------------------
            sb.Append("<Folios>");
            sb.Append("<UUID>" + renglon["UUID"] + " " + "</UUID>");
            sb.Append("<UUIDEstatus>" + renglon["estatus_sat"] + " " + "</UUIDEstatus>");
            sb.Append("<UUIDdescripcion>" + renglon["descripcion"] + " " + "</UUIDdescripcion>");
            sb.Append("<UUIDfecha>" + renglon["fecha"] + " " + "</UUIDfecha>");
            sb.Append("</Folios>");
            //--------------------------------------------------------------------------------
        }
        //------------------------------------------------------------------------------------
        sb.Append("<Signature>" + signatureValue + "</Signature>");
        sb.Append("</Cancelacion>");
        //------------------------------------------------------------------------------------
        return sb.ToString();
        //------------------------------------------------------------------------------------
    }

    //Genera el request que se le envia al cliente
    public string fnListaRequest(ArrayList listaUuid, string sRfcEmisor, string pn_Usuario)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope\">");
        sb.Append("<s:Header>");
        sb.Append("<Action s:mustUnderstand=\"1\" xmlns=\"http://schemas.microsoft.com/ws/2005/05/addressing/none\">https://paxfacturacion.com.mx/IwcfCancelaCFDI/fnCancelarXML</Action>");
        sb.Append("</s:Header>");
        sb.Append("<s:Body>");
        sb.Append("<fnCancelarXML xmlns=\"https://paxfacturacion.com.mx\">");
        sb.Append("<sListaUUID xmlns:d4p1=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">");
        for (int coun = 0; coun < listaUuid.Count; coun++)
        {
            sb.Append("<d4p1:string>" + listaUuid[coun].ToString() + "</d4p1:string>");
        }
        sb.Append("</sListaUUID>");
        sb.Append("<psRFC>");
        sb.Append(sRfcEmisor);
        sb.Append("</psRFC>");
        sb.Append("<sNombre>");
        sb.Append(pn_Usuario);
        sb.Append("</sNombre>");
        sb.Append("</fnCancelarXML>");
        sb.Append("</s:Body>");
        sb.Append("</s:Envelope>");

        return sb.ToString();
    }
}

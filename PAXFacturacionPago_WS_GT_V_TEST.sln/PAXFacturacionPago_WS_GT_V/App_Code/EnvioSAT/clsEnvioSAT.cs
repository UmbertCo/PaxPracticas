using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Web Service SAT******************************************************
using AutenticacionRecepcion = ServicioRecepcionAutenticacionCFDI;
using AutenticacionCancelacion = ServicioCancelacionAutenticacionCFDI;
using Cancelacion = ServicioCancelacionCFDI;
using Recepcion = ServicioRecepcionCFDI;
using SAT.CFDI.Cliente.Procesamiento;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
//Web Service SAT******************************************************  

/// <summary>
/// Clase encargada de enviar los comprobantes al SAT
/// </summary>
public class clsEnvioSAT
{
    //************************************************************************************************************
    //Campos servicios SAT
    #region Campos

    private Recepcion.RecibeCFDIServiceClient clienteRecepcion;
    private Cancelacion.CancelaCFDBindingClient clienteCancelacion;
    private AutenticacionRecepcion.AutenticacionClient clienteAutenticacion;
    private AutenticacionCancelacion.AutenticacionClient clienteAutenticacionCancelacion;

    private AccesoAlmacenBlob clienteAlmacenBlob;
    #endregion

    #region Propiedades
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
    public AutenticacionRecepcion.AutenticacionClient ClienteAutenticacion
    {
        get
        {
            if (clienteAutenticacion == null)
            {
                GenerarClienteAutenticacion();
            }

            return clienteAutenticacion;
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

    private void GenerarClienteRecepcion()
    {
        clienteRecepcion = new Recepcion.RecibeCFDIServiceClient();
    }

    private void GenerarClienteAlmacenBlob()
    {
        clienteAlmacenBlob = new AccesoAlmacenBlob();
    }

    private void GenerarClienteCancelacion()
    {
        clienteCancelacion = new Cancelacion.CancelaCFDBindingClient();
    }

    private void GenerarClienteAutenticacion()
    {
        clienteAutenticacion = new AutenticacionRecepcion.AutenticacionClient();
    }

    private void GenerarClienteAutenticacionCancelacion()
    {
        clienteAutenticacionCancelacion = new AutenticacionCancelacion.AutenticacionClient();
    }
    //Campos servicios SAT
    //************************************************************************************************************
    clsInicioSesionUsuario datosUsuario;
    string sTitulo = "clsEnvioSAT";

	public clsEnvioSAT()
	{
		//
		// TODO: Add constructor logic here
		//
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
    public string fnEnviarBloqueCfdi(string HASH,
                                   int idUsuario, XmlDocument sXmlDocument, int retornoInsert, clsInicioSesionUsuario datosUsuario)
    {
        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(sXmlDocument.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        XPathNavigator navEncabezado = sXmlDocument.CreateNavigator();
        string retorno = string.Empty;
        clsGeneraEMAIL mail = new clsGeneraEMAIL();

        //string HASH = GetHASH(sCadenaOriginal);
        string sRfcEmisor = sXmlDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
        string sNumeroCertificado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value;
        string sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
        string sFechaTFD = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;

        var encabezadoCfdi = new Recepcion.EncabezadoCFDI
        {
            RfcEmisor = sRfcEmisor,
            Hash = HASH.ToUpper(),
            NumeroCertificado = sNumeroCertificado,
            UUID = sUUID,
            Fecha = Convert.ToDateTime(sFechaTFD)
        };


        clsPistasAuditoria.fnGenerarPistasAuditoria(idUsuario, DateTime.Now, sTitulo + "|" + "1.-fnEnviarBloqueCfdi" + "|" + "Se inicia envía del CFDI al SAT");

        try
        {
            var tokenAutenticacion = AutenticaServicio();
            using (var scope = new OperationContextScope(ClienteRecepcion.InnerChannel))
            {
                try
                {
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = tokenAutenticacion;
                    var contenidoArchivo = new MemoryStream(StrToByteArray(sXmlDocument.OuterXml));
                    clsPistasAuditoria.fnGenerarPistasAuditoria(idUsuario, DateTime.Now, sTitulo + "|" + "2.-fnEnviarBloqueCfdi" + "|" + string.Format("Se inicia envío de archivo, Tamaño: {0} bytes", contenidoArchivo.Length));
                    var rutaBlob = ClienteAlmacenBlob.AlmacenarCfdiFramework4(StrToByteArray(sXmlDocument.OuterXml), sUUID + ".xml", datosUsuario.version);

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

                    int nAcuseRequest = clsComun.fnInsertaAcuseSAT(datosUsuario.id_contribuyente.ToString(),encabezadoCfdi.UUID.ToUpper(),
                                                strSoapMessage,
                                                DateTime.Now,
                                                "0",
                                                "Request","E");

                    //Respuesta Request *************************************************************


                    var acuseRecepcion = ClienteRecepcion.Recibe(encabezadoCfdi, rutaBlob);
                    var acuseStream = new MemoryStream();
                    var xmlSerializer = new XmlSerializer(typeof(Recepcion.Acuse));
                    xmlSerializer.Serialize(acuseStream, acuseRecepcion);
                    acuseStream.Seek(0, SeekOrigin.Begin);
                    var acuseReader = new StreamReader(acuseStream);
                    contenidoArchivo.Close();

                    XmlDocument docAcuseSat = new XmlDocument();
                    docAcuseSat.LoadXml(acuseReader.ReadToEnd().Replace("<?xml version=\"1.0\"?>", ""));

                    XmlNamespaceManager docComprobante = new XmlNamespaceManager(docAcuseSat.NameTable);

                    clsPistasAuditoria.fnGenerarPistasAuditoria(idUsuario, DateTime.Now, sTitulo + "|" + "2.-fnEnviarBloqueCfdi" + "|" + "Guardando acuse del SAT.");

                    //Respuesta Response*************************************************************
                    int nAcuse = clsComun.fnInsertaAcuseSAT(datosUsuario.id_contribuyente.ToString(),docAcuseSat.DocumentElement.Attributes["UUID"].Value,
                                                docAcuseSat.OuterXml,
                                                Convert.ToDateTime(docAcuseSat.DocumentElement.Attributes["Fecha"].Value),
                                                docAcuseSat.DocumentElement.Attributes["CodEstatus"].Value,
                                                "Response","E");
                    retorno = docAcuseSat.DocumentElement.Attributes["CodEstatus"].Value;
                    //Respuesta Response*************************************************************

                    if (retorno.Contains("S"))
                    {
                        clsTimbradoFuncionalidad.fnActualizaComprobante(retornoInsert, "A");
                    }
                    else
                    {

                        //Generar mensaje a enviar.
                        string strMensaje = string.Empty;

                        strMensaje = "<table>";
                        strMensaje = strMensaje + "<tr><td><b>Soporte:</b></td><td>Se le ha enviado un correo para informarle que hay facturas que no pasaron ante el SAT, se actualizo su estado a N.</td></tr>";
                        strMensaje = strMensaje + "<tr><td></td><td></td></tr>";
                        strMensaje = strMensaje + "<tr><td><b>Motivo:</b></td><td>Factura.</td></tr>";
                        strMensaje = strMensaje + "<tr><td></td><td></td></tr>";
                        strMensaje = strMensaje + "</table>";
                        strMensaje = strMensaje + "<table>";
                        strMensaje = strMensaje + "<tr><td><b>ID_Contribuyente</b></td><td><b>UUID</b></td><td></td><td><b>Acuse</b></td><td></td><td><b>Fecha</b></td></tr>";
                        strMensaje = strMensaje + "<tr><td>" + datosUsuario.id_contribuyente.ToString() + "</td><td>" + docAcuseSat.DocumentElement.Attributes["UUID"].Value + 
                                                    "</td><td></td><td>" + docAcuseSat.DocumentElement.Attributes["CodEstatus"].Value + 
                                                    "</td><td></td><td>" + Convert.ToDateTime(docAcuseSat.DocumentElement.Attributes["Fecha"].Value) + "</td></tr>";
                        strMensaje = strMensaje + "</table>";


                        clsTimbradoFuncionalidad.fnActualizaComprobante(retornoInsert, "N");
                        mail.EnviarCorreo(System.Configuration.ConfigurationSettings.AppSettings["emailAll"], "Facturas enviadas al SAT", strMensaje);
                    }


                    clsPistasAuditoria.fnGenerarPistasAuditoria(idUsuario, DateTime.Now, sTitulo + "|" + "3.-fnEnviarBloqueCfdi" + "|" + "Archivo enviado correctamente.");

                }
                catch (Exception ex)
                {
                    clsTimbradoFuncionalidad.fnActualizaComprobante(retornoInsert, "E");
                    mail.EnviarCorreo(System.Configuration.ConfigurationSettings.AppSettings["emailAll"], "No se puede conectar con el SAT al Enviar", "Error del SAT: "+ ex.Message+ "UUID: "+sUUID);
                    clsPistasAuditoria.fnGenerarPistasAuditoria(idUsuario, DateTime.Now, sTitulo + "|" + "No se puede conectar con el SAT al Enviar");
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                }

            }
        }
        catch (Exception ex)
        {
            clsTimbradoFuncionalidad.fnActualizaComprobante(retornoInsert, "E");
            mail.EnviarCorreo(System.Configuration.ConfigurationSettings.AppSettings["emailAll"], "No se puede conectar con el SAT al Autentificar", "Error del SAT: " + ex.Message + "UUID: " + sUUID);
            clsPistasAuditoria.fnGenerarPistasAuditoria(idUsuario, DateTime.Now, sTitulo + "|" + "No se puede conectar con el SAT al Autentificar");
        }

        return retorno;


    }

    private HttpRequestMessageProperty AutenticaServicio()
    {
        //Aceptar certificados caducados
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsEnvioSAT.AcceptAllCertificatePolicy);

        var token = ClienteAutenticacion.Autentica();
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

}
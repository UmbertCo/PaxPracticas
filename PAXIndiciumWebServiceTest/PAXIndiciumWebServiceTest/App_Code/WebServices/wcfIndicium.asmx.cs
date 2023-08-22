using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Hosting;
using System.Net;
using System.Net.Security;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Xml.Xsl;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace PAXIndiciumWebServiceTest
{
    [WebService(Namespace = "https://test.paxfacturacion.com.mx:473")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    public class wcfIndicium : System.Web.Services.WebService
    {
        private clsInicioSesionUsuario datosUsuario;
        protected DataTable dtCreditos;
        private string xsd_error_code;
        private int errorCode;
        private clsOperacionTimbradoSellado gTimbrado;
        private clsValCertificado gCertificado;
        private TimbreFiscalDigital gNodoTimbre;

        [WebMethod(Description = "Permite realizar el timbrado del comprobante firmado por el cliente y devuelve el timbre del comprobante timbrado.")]
        public byte[] getTimbreCfdi(String user, String password, byte[] file)
        {
            byte[] var = { };

            int pnId_Usuario = 0;
            int pnId_Estructura = 0;
            string scadena = string.Empty;
            string sRequest = string.Empty;
            string sesquema = string.Empty;
            string sResponse = string.Empty;
            string sRetornoSAT = string.Empty;
            string sXmlDocument = string.Empty;
            string sCadenaOriginal = string.Empty;
            string sRetAutentication = string.Empty;
            string sCadenaOriginalEmisor = string.Empty;
            string sVersion = string.Empty;
            string TimbreEnviado = "";

            TimbreFiscalDigital gNodoTimbre;
            clsOperacionTimbradoSellado gTimbrado;
            clsConfiguracionCreditos css;
            clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();

            string sComprobante = Encoding.UTF8.GetString(file);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sComprobante);
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");

            try { sVersion = xmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value; }
            catch { }

            //Revisar version
            if (sVersion != "3.2")
            {
                return Encoding.UTF8.GetBytes("999 - Por disposición oficial estos comprobantes ya no se timbran.");
            }
            else
            {
                sesquema = "esquema_v3_2";
            }

            if (Convert.ToDouble(sComprobante.Length / 1024.0) > 100.0)
            {
                clsPistasAuditoria.fnGenerarPistasAuditoria(0, DateTime.Now, "WSD paso0 - Tamaño de factura excedida: " + user + " Tamaño: " + Convert.ToDouble(sComprobante.Length / 1024.0));
                try
                {
                    bool lista = false;
                    string value = File.ReadAllText(HostingEnvironment.MapPath("VipSizeFiles.xml"));
                    XmlDocument vipList = new XmlDocument();
                    vipList.LoadXml(value);

                    XmlNodeList nodeList = vipList.SelectNodes("/VipList/user");

                    foreach (XmlNode no in nodeList)
                    {
                        string clave_usuario = no.Attributes["clave_usuario"].Value;

                        if (clave_usuario == user)
                        {
                            lista = true;
                            clsPistasAuditoria.fnGenerarPistasAuditoria(0, DateTime.Now, "WSD paso0 - Se encuentra en la lista para poder timbrar " + user + " " + Convert.ToDouble(sComprobante.Length / 1024.0));

                            if (Convert.ToDouble(sComprobante.Length / 1024.0) > 200.0)
                            {
                                return Encoding.UTF8.GetBytes("999 - El tamaño del XML excede el limite permitido VipList.");
                            }
                            else
                            {
                                clsPistasAuditoria.fnGenerarPistasAuditoria(0, DateTime.Now, "WSD paso0 - Continua proceso.");
                                break;
                            }
                        }
                    }
                    if (lista == false)
                    {
                        return Encoding.UTF8.GetBytes("999 - El tamaño del XML excede el limite permitido. N");
                    }
                }
                catch (Exception)
                {
                    return Encoding.UTF8.GetBytes("999 - El tamaño del XML excede el limite permitido. T");
                }
            }

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

            sRetAutentication = fnAutentication(user, password, ref pnId_Usuario, ref datosUsuario);

            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso1 - " + "Verifica el usuario WSDL SVC Timbrado_DP." + "0.0.0.0" + " USER: " + user + " PASSWORD: " + password.Substring(0, 3) + "*************");

            //Revisa encabezado del XML
            try
            {
                if (sComprobante.Contains("<?xml version=\"1.0\" encoding=\"UTF-8\"?>"))
                {
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación")); //El archivo de texto esta mal formado
                }
            }
            catch (Exception)
            {
                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("301", "Timbrado y Cancelación")); //El archivo de texto esta mal formado
            }

            //Inserta acuse PAC
            try
            {
                XmlDocument acuseXML = new XmlDocument();
                acuseXML.LoadXml(sComprobante);
                //*******************************************************Insertar Request en tabla de acuses
                sRequest = clsComun.fnRequestRecepcion(acuseXML.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), "Factura", pnId_Estructura.ToString(), user, password);
                if (clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sRequest, DateTime.Now, "0", "Request", "E", string.Empty) == 0)
                {
                    return Encoding.UTF8.GetBytes("301 - Revise la codificación del documento que sea UTF-8.");
                }
                //*******************************************************Insertar Request en tabla de acuses
            }
            catch
            {
                return Convert.FromBase64String("301 - Revise la codificación del documento que sea UTF-8.");
            }

            string[] est = sRetAutentication.Split('-');
            if (est.Length > 2)
            {
                sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
            }
            else
            {
                sRetAutentication = est[0].Trim();
            }
            if (sRetAutentication == "0")
            {
                if (pnId_Estructura == 0)
                    pnId_Estructura = Convert.ToInt32(fnRecuperaMatriz(pnId_Usuario));

                int pnId_tipo_documento = 1;

                //Revisar los creditos disponibles.
                dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');

                if (dtCreditos.Rows.Count > 0)
                {
                    css = new clsConfiguracionCreditos();
                    double dCostCred = css.fnRecuperaPrecioServicio(1);
                    double creditos = 0;
                    creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                    if (creditos == 0)
                    {
                        clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                        dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                        double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                        if (creditos2 == 0)
                        {
                            return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("97", "Seguridad"));
                        }

                        if (creditos2 < dCostCred)
                        {
                            return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("97", "Seguridad"));
                        }
                    }
                    else
                    {
                        if (creditos < dCostCred)
                        {
                            return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("97", "Seguridad"));
                        }
                    }
                }
                else
                {
                    clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                    css = new clsConfiguracionCreditos();
                    double dCostCred = css.fnRecuperaPrecioServicio(1);

                    if (creditos == 0)
                    {
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("97", "Seguridad"));
                    }
                    if (creditos < dCostCred)
                    {
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("97", "Seguridad"));
                    }
                }
                try
                {
                    pnId_tipo_documento = clsTimbradoFuncionalidad.fnBuscarTipoDocumento("Factura");
                }
                catch (Exception ex)
                {
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El nombre de documento no corresponde a ningúno del sistema", pnId_Usuario);
                    //return "406"; //El nombre de documento no corresponde a ningúno del sistema
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("406", "Consulta"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "406", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("406", "Consulta"));
                }

                //Recupera datos del emisor.
                clsValCertificado vValidadorCertificado = null;
                XmlDocument xDocTimbrado = new XmlDocument();
                XmlDocument docNodoTimbre;
                gNodoTimbre = new TimbreFiscalDigital();
                string sRFCEmisor = string.Empty;
                string sSello = string.Empty;
                string sNoCertificado = string.Empty;
                string sRFCReceptor = string.Empty;
                string xsd_validacion = string.Empty;
                string sPassword = string.Empty;
                int retVal;
                DataTable tblFechas = new DataTable();
                string noCertificadoPAC = string.Empty;
                string selloPAC = string.Empty;
                string sErrorHSM = String.Empty;
                string sRetornoXML = string.Empty;
                DateTime dFechaComprobante;
                errorCode = 0;


                //****************************************************************************************************************************************Llamar al Data Power
                //****************************************************************************************************************************************Llamar al Data Power

                clsDataPowerTimbrado dataPower = new clsDataPowerTimbrado();

                string sDocumentoDP = dataPower.clsTimbrarDataPower(sComprobante, System.Configuration.ConfigurationManager.AppSettings["urlDataPower"]);



                if (sDocumentoDP != string.Empty)
                {
                    if (!sDocumentoDP.Contains("<cfdi:Comprobante"))
                    {
                        string sCodigo = "000";
                        try
                        {
                            var codigo = sDocumentoDP.Split('-');
                            sCodigo = codigo[0].ToString();
                        }
                        catch (Exception)
                        { }
                        sDocumentoDP = sDocumentoDP.Replace("<", "&lt;").Replace(">", "&gt;");
                        sResponse = clsComun.fnResponseRecepcion(sDocumentoDP);
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sCodigo, "Response", "E", string.Empty);
                        clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD Resultado - " + "Timbrado Incorrecto, ver acuse.");

                        return Encoding.UTF8.GetBytes(sDocumentoDP);
                    }

                    xDocTimbrado.LoadXml(sComprobante);

                    //Generar la cadena del emisor con los conceptos del XML
                    try
                    {
                        XslCompiledTransform xslt;
                        XsltArgumentList args;
                        MemoryStream ms;
                        StreamReader srDll;
                        switch (sVersion)
                        {
                            case "3.2":
                                // Load the type of the class.
                                xslt = new XslCompiledTransform();
                                xslt.Load(typeof(CaOri.V32));
                                ms = new MemoryStream();
                                args = new XsltArgumentList();
                                xslt.Transform(xDocTimbrado.CreateNavigator(), args, ms);
                                ms.Seek(0, SeekOrigin.Begin);
                                srDll = new StreamReader(ms);

                                sCadenaOriginalEmisor = srDll.ReadToEnd();
                                break;
                        }

                    }
                    catch (Exception)
                    {
                        return Convert.FromBase64String(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                    }
                    XmlNamespaceManager nsmComprobanteDP = new XmlNamespaceManager(xDocTimbrado.NameTable);
                    try
                    {
                        xDocTimbrado.LoadXml(sDocumentoDP);
                        nsmComprobanteDP.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                        nsmComprobanteDP.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                        gNodoTimbre.FechaTimbrado = Convert.ToDateTime(xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobanteDP).Value);
                        gNodoTimbre.selloCFD = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloCFD", nsmComprobanteDP).Value;
                        gNodoTimbre.selloSAT = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobanteDP).Value;
                        gNodoTimbre.UUID = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteDP).Value;
                        gNodoTimbre.version = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@version", nsmComprobanteDP).Value;
                        gNodoTimbre.noCertificadoSAT = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobanteDP).Value;

                        //Generamos el primer XML necesario para generar la cadena original
                        gTimbrado = new clsOperacionTimbradoSellado();
                        docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                        //Generamos la cadena original
                        XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();
                        XslCompiledTransform xslt;
                        XsltArgumentList args;
                        MemoryStream ms;
                        StreamReader srDll;

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(Timbrado.V3.TFDXSLT));
                        ms = new MemoryStream();
                        args = new XsltArgumentList();
                        xslt.Transform(navNodoTimbre, args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCadenaOriginal = srDll.ReadToEnd();
                    }
                    catch (Exception)
                    {
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                    }

                    string cadenaConDP = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaConDP)))
                    {
                        con.Open();

                        using (SqlTransaction tran = con.BeginTransaction())
                        {
                            try
                            {
                                //Buscar el HASH del comprobante de la cadena original del emisor.
                                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso24 - " + "Busca el HAS del emisor.");
                                string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginalEmisor).ToUpper();
                                string Comprobante = clsTimbradoFuncionalidad.fnBuscarHashCompXML(datosUsuario.id_usuario, HASHEmisor, "Emisor");
                                if (Comprobante != "0")
                                {
                                    XmlDocument hasComprobante = new XmlDocument();
                                    hasComprobante.LoadXml(Comprobante);


                                    XmlNamespaceManager nsmComprobante2 = new XmlNamespaceManager(hasComprobante.NameTable);
                                    nsmComprobante2.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                    nsmComprobante2.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                                    nsmComprobante2.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
                                    XmlNode TimbreGeneral = hasComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante2);
                                    TimbreEnviado = TimbreGeneral.OuterXml;

                                    //return "98";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    //return clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                                    return Encoding.UTF8.GetBytes(TimbreEnviado);
                                }

                                //Buscar el HASH del comprobante de la cadena original del timbre fiscal.
                                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso25 - " + "Busca el HAS del TFD.");
                                string HASHTimbreFiscal = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();
                                if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbreFiscal, "Timbre"))
                                {
                                    //return "98";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                                }
                                //--------------------------------------------------------------------
                                string sUUID = string.Empty;
                                string sSerie = string.Empty;
                                string sfolio = string.Empty;
                                string sTotal = string.Empty;
                                string sMoneda = string.Empty;
                                string sRfcEmisor = string.Empty;
                                string sRfcReceptor = string.Empty;
                                string sEmisorNombre = string.Empty;
                                string sFecha_emision = string.Empty;
                                string sFechaTimbrado = string.Empty;
                                string sReceptorNombre = string.Empty;
                                //--------------------------------------------------------------------
                                try { sUUID = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteDP).Value; }
                                catch { }
                                try { sFechaTimbrado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobanteDP).Value; }
                                catch { }
                                //--------------------------------------------------------------------
                                try { sSerie = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobanteDP).Value; }
                                catch { }
                                try { sfolio = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobanteDP).Value; }
                                catch { }
                                try { sTotal = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobanteDP).Value; }
                                catch { }
                                try { sMoneda = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobanteDP).Value; }
                                catch { }
                                try { sRfcEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobanteDP).Value; }
                                catch { }
                                try { sRfcReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobanteDP).Value; }
                                catch { }
                                try { sEmisorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobanteDP).Value; }
                                catch { }
                                try { sFecha_emision = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobanteDP).Value; }
                                catch { }
                                try { sReceptorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobanteDP).Value; }
                                catch { }
                                //--------------------------------------------------------------------

                                using (SqlCommand command = new SqlCommand("usp_Timbrado_InsertaComprobanteAll_Completo_Ins_Indicium", con))
                                {
                                    command.Transaction = tran;
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    command.CommandTimeout = 200;
                                    command.Parameters.AddWithValue("sXML", xDocTimbrado.DocumentElement.OuterXml);
                                    command.Parameters.AddWithValue("nId_tipo_documento", pnId_tipo_documento);
                                    command.Parameters.AddWithValue("cEstatus", "P");
                                    command.Parameters.AddWithValue("dFecha_Documento", DateTime.Now);
                                    command.Parameters.AddWithValue("nId_estructura", pnId_Estructura);
                                    command.Parameters.AddWithValue("nId_usuario_timbrado", pnId_Usuario);
                                    command.Parameters.AddWithValue("nSerie", "N/A");
                                    command.Parameters.AddWithValue("sOrigen", "R");
                                    command.Parameters.AddWithValue("sHash", HASHTimbreFiscal);
                                    command.Parameters.AddWithValue("sDatos", HASHEmisor);
                                    command.Parameters.AddWithValue("@sUuid", sUUID);
                                    command.Parameters.AddWithValue("@dFecha_Timbrado", sFechaTimbrado);
                                    command.Parameters.AddWithValue("@sRFC_Emisor", sRfcEmisor);
                                    command.Parameters.AddWithValue("@sNombre_Emisor", sEmisorNombre);
                                    command.Parameters.AddWithValue("@sRFC_Receptor", sRfcReceptor);
                                    command.Parameters.AddWithValue("@sNombre_Receptor", sReceptorNombre);
                                    command.Parameters.AddWithValue("@dFecha_Emision", sFecha_emision);
                                    command.Parameters.AddWithValue("@sSerie", sSerie);
                                    command.Parameters.AddWithValue("@sFolio", sfolio);
                                    command.Parameters.AddWithValue("@nTotal", sTotal);
                                    command.Parameters.AddWithValue("@sMoneda", sMoneda);

                                    retVal = Convert.ToInt32(command.ExecuteScalar());
                                }
                                fnActualizaCreditos(dtCreditos);

                                tran.Commit();

                                if (retVal == 0)
                                {
                                    //return "999";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                                }
                                else
                                {
                                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso27 - " + "Recupera XML para regresarlo al cliente.");
                                    sXmlDocument = xDocTimbrado.OuterXml;


                                    //Enviar comprobante al SAT --Comentado para mejorar el rendimento de entrega de comprobante.
                                    //clsEnvioSAT enviarSAT = new clsEnvioSAT();
                                    //sRetornoSAT = enviarSAT.fnEnviarBloqueCfdi(HASH, pnId_Usuario, xDocTimbrado, retVal, datosUsuario);

                                    //*******************************************************Insertar Response en tabla de acuses
                                    //string uuid = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                                    string uuid = gNodoTimbre.UUID;
                                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, uuid.ToUpper(), sDocumentoDP, DateTime.Now, "0", "Response", "E", string.Empty);
                                    sXmlDocument = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobanteDP).OuterXml;
                                    //*******************************************************Insertar Response en tabla de acuses
                                }
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante", pnId_Usuario);
                                //return "999"; //Error durante el registro del comprobante
                                //*******************************************************Insertar Response en tabla de acuses
                                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                                //*******************************************************Insertar Response en tabla de acuses
                                return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                    return Encoding.UTF8.GetBytes(sXmlDocument);
                }
                else
                {
                    //return Encoding.UTF8.GetBytes("999 - Servicio No disponible, comuníquese con PAX Facturación.");
                    //****************************************************************************************************************************************Llamar al Data Power
                    //****************************************************************************************************************************************Llamar al Data Power

                    //Verificamos que el XML cumpla con el esquema de SAT
                    try
                    {
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso5 - " + "Verifica el esquema.");
                        xsd_validacion = fnValidate(sComprobante, sesquema);//"esquema_v3");


                        if (xsd_validacion != string.Empty && xsd_validacion != null)
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("333", "Consulta"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "333", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes("333 - " + xsd_validacion);
                        }

                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "El XML no cumple con el esquema de hacienda", pnId_Usuario);
                        //return "333";
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("333", "Consulta"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "333", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("333", "Consulta"));
                    }

                    try
                    {
                        if (sComprobante.ToLowerInvariant().IndexOf('|') != -1)
                        {

                            return Encoding.UTF8.GetBytes("301 - XML mal formado. |");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Encoding.UTF8.GetBytes("301 - XML mal formado. |");
                    }




                    //Recuperamos el certificado a partir del XML del comprobante
                    try
                    {
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso6 - " + "Recupera Certificado.");
                        vValidadorCertificado = fnRecuperarCertificado(sComprobante);

                        if (vValidadorCertificado == null)
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("570", "Timbrado"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "570", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("570", "Timbrado"));
                        }
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudó recuperar el certificado del comprobante", pnId_Usuario);
                        // return "570";
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("570", "Timbrado"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "570", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("570", "Timbrado"));
                    }



                    //Verificamos que el certificado del comprobante se de tipo CSD
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso7 - " + "Verifica certificado sea CSD.");
                    if (!vValidadorCertificado.fnEsCSD())
                    {
                        clsErrorLog.fnNuevaEntrada(new Exception("El certificado no es de tipo CSD"), clsErrorLog.TipoErroresLog.Datos, "El certificado no es de tipo CSD", pnId_Usuario);
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("306", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "306", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("306", "Timbrado y Cancelación")); //El certificado no es de tipo CSD
                    }



                    //Certificado no expedido por el SAT
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso8 - " + "Verifica certificado no apocrifo");
                    if (!vValidadorCertificado.fnCSD308())
                    {
                        clsErrorLog.fnNuevaEntrada(new Exception("Certificado no expedido por el SAT"), clsErrorLog.TipoErroresLog.Datos, "Certificado no expedido por el SAT", pnId_Usuario);
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("308", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "308", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("308", "Timbrado y Cancelación")); //El certificado no es de tipo CSD
                    }



                    try
                    {
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso9 - " + "Recupera datos de XML");

                        xDocTimbrado.LoadXml(sComprobante);

                        sRFCEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
                        dFechaComprobante = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;
                        sSello = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value;
                        sNoCertificado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value;
                        sRFCReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;


                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "Faltan datos del comprobante", pnId_Usuario);
                        //return "799";//Faltan datos del comprobante
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("N-504", "Recepción"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "N - 504", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("N-504", "Recepción"));

                    }


                    //Verificar que no contenga adenda el comprobante.
                    try
                    {
                        if (xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante) != null)
                        {
                            if (xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante).LocalName != string.Empty)
                            {
                                return Encoding.UTF8.GetBytes("510 - No esta permitido enviar adendas en el comprobante.");
                            }
                        }

                    }
                    catch
                    {
                        return Encoding.UTF8.GetBytes("510 - No esta permitido enviar adendas en el comprobante.");
                    }


                    try
                    {
                        //Validar el RFC del receptor 18/06/2012 cambios en la recepcion del sat.
                        if (!clsComun.fnValidaExpresion(sRFCReceptor.Trim(), @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                        {
                            return Encoding.UTF8.GetBytes("509 - Verifique el RFC del receptor.");
                        }

                    }
                    catch (Exception)
                    {
                        return Encoding.UTF8.GetBytes("509 - Verifique el RFC del receptor.");
                    }



                    //verificamos si el comprobante ya contiene un nodo timbre
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso10 - " + "Revisar Timbre Previo.");
                    XmlNode aux = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante);

                    if (aux != null)
                    {
                        // return "472"; //El comprobante ya está timbrado
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("307", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "307", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("307", "Timbrado y Cancelación"));
                    }




                    //Generar la cadena del emisor con los conceptos del XML
                    try
                    {

                        XslCompiledTransform xslt;
                        XsltArgumentList args;
                        MemoryStream ms;
                        StreamReader srDll;

                        switch (sVersion)
                        {
                            case "3.2":

                                // Load the type of the class.
                                xslt = new XslCompiledTransform();
                                xslt.Load(typeof(CaOri.V32));

                                ms = new MemoryStream();
                                args = new XsltArgumentList();

                                xslt.Transform(xDocTimbrado.CreateNavigator(), args, ms);
                                ms.Seek(0, SeekOrigin.Begin);
                                srDll = new StreamReader(ms);

                                sCadenaOriginalEmisor = srDll.ReadToEnd();

                                break;
                        }

                    }
                    catch (Exception)
                    {
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                    }



                    try
                    {
                        //303------Preparamos los objetos de manejo tanto de la llave como del certificado---303
                        //Validamos el sello de emisor
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso12 - " + "Revisar que corresponda la candena del emisor.");
                        if (string.IsNullOrEmpty(sSello) || !(sCadenaOriginalEmisor.Contains(sRFCEmisor)))
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "303", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                        }
                    }
                    catch (Exception)
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "303", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));

                    }



                    //Certificado revocado o caduco R o C
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso13 - " + "Revisa caducidad del certificado.");
                    if (!fnVerificarCaducidadCertificado(sNoCertificado, "R", "C", ref vValidadorCertificado))
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("304", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "304", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("304", "Timbrado y Cancelación"));
                    }



                    try
                    {
                        //La fecha de emisión no esta dentro de la vigencia del CSD del Emisor
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso14 - " + "Revisar vigencia del certificado.");
                        if (!vValidadorCertificado.ComprobarFechas() || !fnRecuperaFechaLCO(sNoCertificado, "A", ref vValidadorCertificado))
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "305", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación"));
                        }
                    }
                    catch (Exception)
                    {
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación"));
                    }



                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso15 - " + "Revisar Fecha del comprobante no sea menor a 2011.");
                    if (!clsComun.fnRevisarFechaNoPosterior(dFechaComprobante))
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("403", "Timbrado"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "403", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("403", "Timbrado"));
                    }


                    //RFC del emisor no se encuentra en el régimen de contribuyentes 402
                    try
                    {
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso16 - " + "Revisar en el LCO.");
                        string rfcComprobante = vValidadorCertificado.VerificarExistenciaCertificado();
                        if (sRFCEmisor != rfcComprobante)
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("402", "Timbrado"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "402", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("402", "Timbrado"));
                        }
                    }
                    catch
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("402", "Timbrado"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "402", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("402", "Timbrado"));
                    }


                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso17 - " + "Revisar fecha dentro de 72 horas.");
                    if (!vValidadorCertificado.fnFechaContraPeriodoValidez(dFechaComprobante))
                    {
                        // return "504"; //La fecha del comprobante esta fuera de periodo
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("401", "Timbrado"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "401", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("401", "Timbrado"));
                    }



                    //Revisamos que esl sello del emisor sea valido.
                    try
                    {
                        //Preparamos los objetos de manejo tanto de la llave como del certificado
                        gTimbrado = new clsOperacionTimbradoSellado();
                        // *************Comentado por limitar rendimiento al HSM*************************************************
                        //Validamos el sello de emisor
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso18 - " + "Revisar sello del emisor.");
                        if (!vValidadorCertificado.fnVerificarSello(sCadenaOriginalEmisor, sSello))
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "302", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                        }
                        // *************Comentado por limitar rendimiento al HSM*************************************************
                    }
                    catch (Exception)
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "302", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                    }



                    try
                    {

                        //Llenamos los datos del nodo timbre
                        gNodoTimbre.UUID = Guid.NewGuid().ToString();
                        gNodoTimbre.FechaTimbrado = Convert.ToDateTime(DateTime.Now.ToString("s"));
                        gNodoTimbre.selloCFD = sSello;

                        //****************************************************BLOQUE HSM********************************************************
                        //****************************************************BLOQUE HSM********************************************************

                        wslServicioPAC gServicio = new wslServicioPAC();
                        ////Generamos el primer XML necesario para generar la cadena original
                        docNodoTimbre = null;

                        try
                        {

                            //HSM3
                            if (!fnFirmaHSMTerciario(gServicio, ref docNodoTimbre, vValidadorCertificado, ref sCadenaOriginal, "SeccionA", ref sErrorHSM))
                            {
                                try
                                {
                                    throw new Exception("No se pudo timbrar la factura, con el HSM3");
                                }
                                catch (Exception ex)
                                {
                                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudo timbrar la factura, con el HSM3", pnId_Usuario);
                                }

                                return Encoding.UTF8.GetBytes("999 - El sistema esta ocupado, por favor intente de nuevo.");
                            }
                        }
                        catch (Exception exHsm)
                        {
                            sErrorHSM = "Ocurrio un error al buscar los HSM Principal y Secundario. " + exHsm.Message;
                            //HSM3
                            if (!fnFirmaHSMTerciario(gServicio, ref docNodoTimbre, vValidadorCertificado, ref sCadenaOriginal, "SeccionC", ref sErrorHSM))
                            {
                                try
                                {
                                    throw new Exception("No se pudo timbrar la factura, con el HSM3");
                                }
                                catch (Exception ex)
                                {
                                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudo timbrar la factura, con el HSM3", pnId_Usuario);
                                }

                                return Encoding.UTF8.GetBytes("999 - El sistema esta ocupado, por favor intente de nuevo.");
                            }
                        }



                        //****************************************************BLOQUE HSM********************************************************
                        //****************************************************BLOQUE HSM********************************************************

                        //xDocTimbrado = gTimbrado.fnGenerarXML(gNodoTimbre);// Reescribe el documento. No Borrar.

                        //***********Genera la firma con el HSM*************************************

                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso23 - " + "Cargando timbre al documento.");

                        docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                        XmlNode Complemento = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsmComprobante);
                        if (Complemento == null)
                            Complemento = xDocTimbrado.CreateElement("cfdi", "Complemento", nsmComprobante.LookupNamespace("cfdi"));

                        Complemento.InnerXml = docNodoTimbre.DocumentElement.OuterXml + Complemento.InnerXml;

                        xDocTimbrado.DocumentElement.AppendChild(Complemento);


                        //Eliminamos la llave y el certificado
                        gTimbrado.fnDestruirLlave();
                    }
                    catch (Exception ex)
                    {
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "No se pudo generar el sello del PAC", pnId_Usuario);
                        //return "817"; //No se pudo generar el sello del PAC
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("817", "Timbrado"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "817", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes("999 - El sistema esta ocupado, por favor intente de nuevo.");
                    }

                    string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
                    {
                        con.Open();

                        using (SqlTransaction tran = con.BeginTransaction())
                        {

                            try
                            {

                                //Buscar el HASH del comprobante de la cadena original del emisor.
                                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso24 - " + "Busca el HAS del emisor.");

                                string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginalEmisor).ToUpper();
                                string Comprobante = clsTimbradoFuncionalidad.fnBuscarHashCompXML(datosUsuario.id_usuario, HASHEmisor, "Emisor");
                                if (Comprobante != "0")
                                {
                                    XmlDocument hasComprobante = new XmlDocument();
                                    hasComprobante.LoadXml(Comprobante);

                                    XmlNamespaceManager nsmComprobante2 = new XmlNamespaceManager(hasComprobante.NameTable);
                                    nsmComprobante2.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                    nsmComprobante2.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                                    nsmComprobante2.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
                                    XmlNode TimbreGeneral = hasComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante2);
                                    TimbreEnviado = TimbreGeneral.OuterXml;

                                    //return "98";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    //return clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                                    return Encoding.UTF8.GetBytes(TimbreEnviado);
                                }

                                //Buscar el HASH del comprobante de la cadena original del timbre fiscal.
                                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso25 - " + "Busca el HAS del TFD.");
                                string HASHTimbreFiscal = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();
                                if (clsTimbradoFuncionalidad.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbreFiscal, "Timbre"))
                                {
                                    //return "98";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("98", "Timbrado"));
                                }
                                //--------------------------------------------------------------------
                                string sUUID = string.Empty;
                                string sSerie = string.Empty;
                                string sfolio = string.Empty;
                                string sTotal = string.Empty;
                                string sMoneda = string.Empty;
                                string sRfcEmisor = string.Empty;
                                string sRfcReceptor = string.Empty;
                                string sEmisorNombre = string.Empty;
                                string sFecha_emision = string.Empty;
                                string sFechaTimbrado = string.Empty;
                                string sReceptorNombre = string.Empty;
                                //--------------------------------------------------------------------
                                try { sUUID = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                                catch { }
                                try { sFechaTimbrado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value; }
                                catch { }
                                //--------------------------------------------------------------------
                                try { sSerie = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value; }
                                catch { }
                                try { sfolio = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
                                catch { }
                                try { sTotal = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value; }
                                catch { }
                                try { sMoneda = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante).Value; }
                                catch { }
                                try { sRfcEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value; }
                                catch { }
                                try { sRfcReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value; }
                                catch { }
                                try { sEmisorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value; }
                                catch { }
                                try { sFecha_emision = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value; }
                                catch { }
                                try { sReceptorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value; }
                                catch { }
                                //--------------------------------------------------------------------
                                using (SqlCommand command = new SqlCommand("usp_Timbrado_InsertaComprobanteAll_Completo_Ins_Indicium", con))
                                {

                                    command.Transaction = tran;
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    command.CommandTimeout = 200;

                                    command.Parameters.AddWithValue("sXML", xDocTimbrado.DocumentElement.OuterXml);
                                    command.Parameters.AddWithValue("nId_tipo_documento", pnId_tipo_documento);
                                    command.Parameters.AddWithValue("cEstatus", "P");
                                    command.Parameters.AddWithValue("dFecha_Documento", DateTime.Now);
                                    command.Parameters.AddWithValue("nId_estructura", pnId_Estructura);
                                    command.Parameters.AddWithValue("nId_usuario_timbrado", pnId_Usuario);
                                    command.Parameters.AddWithValue("nSerie", "N/A");
                                    command.Parameters.AddWithValue("sOrigen", "R");
                                    command.Parameters.AddWithValue("sHash", HASHTimbreFiscal);
                                    command.Parameters.AddWithValue("sDatos", HASHEmisor);
                                    command.Parameters.AddWithValue("@sUuid", sUUID);
                                    command.Parameters.AddWithValue("@dFecha_Timbrado", sFechaTimbrado);
                                    command.Parameters.AddWithValue("@sRFC_Emisor", sRfcEmisor);
                                    command.Parameters.AddWithValue("@sNombre_Emisor", sEmisorNombre);
                                    command.Parameters.AddWithValue("@sRFC_Receptor", sRfcReceptor);
                                    command.Parameters.AddWithValue("@sNombre_Receptor", sReceptorNombre);
                                    command.Parameters.AddWithValue("@dFecha_Emision", sFecha_emision);
                                    command.Parameters.AddWithValue("@sSerie", sSerie);
                                    command.Parameters.AddWithValue("@sFolio", sfolio);
                                    command.Parameters.AddWithValue("@nTotal", sTotal);
                                    command.Parameters.AddWithValue("@sMoneda", sMoneda);
                                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso26 - " + "Inserta Comprobante.");
                                    retVal = Convert.ToInt32(command.ExecuteScalar());
                                }

                                //fnActualizaCreditos(dtCreditos);

                                tran.Commit();


                                if (retVal == 0)
                                {
                                    //return "999";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                                }
                                else
                                {

                                    clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso27 - " + "Recupera XML para regresarlo al cliente.");
                                    sXmlDocument = xDocTimbrado.OuterXml;
                                    fnActualizaCreditos(dtCreditos);

                                    //Enviar comprobante al SAT --Comentado para mejorar el rendimento de entrega de comprobante.
                                    //clsEnvioSAT enviarSAT = new clsEnvioSAT();
                                    //sRetornoSAT = enviarSAT.fnEnviarBloqueCfdi(HASH, pnId_Usuario, xDocTimbrado, retVal, datosUsuario);

                                    //*******************************************************Insertar Response en tabla de acuses
                                    string uuid = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, uuid.ToUpper(), sXmlDocument, DateTime.Now, "0", "Response", "E", string.Empty);
                                    sXmlDocument = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).OuterXml;
                                    //*******************************************************Insertar Response en tabla de acuses
                                }


                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante HSM ASMX", pnId_Usuario);
                                //return "999"; //Error durante el registro del comprobante
                                //*******************************************************Insertar Response en tabla de acuses
                                sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                                clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                                //*******************************************************Insertar Response en tabla de acuses
                                return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                            }
                            finally
                            {
                                //tran.Commit();
                                con.Close();
                            }

                        }
                    }

                }
            }
            else
            {
                try
                {
                    sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sRetAutentication, "Response", "E", string.Empty);
                    sXmlDocument = clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
                }
                catch
                {
                    sXmlDocument = clsComun.fnRecuperaErrorSAT("95", "Seguridad"); ;
                }
            }

            return Encoding.UTF8.GetBytes(sXmlDocument);

        }

        [WebMethod(Description = "Permite cancelar comprobantes timbrados.")]
        public Acuse cancelaCFDi(String user, String password, String rfc, String[] ListaUUID, byte[] pfx, String pfxPassword)
        {
            XmlDocument xmldoc = new XmlDocument();
            int pnId_Usuario = 0;
            int pnId_Estructura = 0;
            string RespuestaFinal = string.Empty;
            string sRetAutentication = string.Empty;
            string Respuesta = string.Empty;
            ArrayList psListaUUID = new ArrayList();
            clsConfiguracionCreditos css;
            Acuse CancelaResponse = new Acuse();

            DataTable TableUUID = new DataTable();

            DataColumn columna1 = new DataColumn();
            columna1.DataType = System.Type.GetType("System.String");
            columna1.AllowDBNull = true;
            columna1.Caption = "UUID";
            columna1.ColumnName = "UUID";
            columna1.DefaultValue = null;
            TableUUID.Columns.Add(columna1);

            DataColumn columna2 = new DataColumn();
            columna2.DataType = System.Type.GetType("System.String");
            columna2.AllowDBNull = true;
            columna2.Caption = "estatus";
            columna2.ColumnName = "estatus";
            columna2.DefaultValue = null;
            TableUUID.Columns.Add(columna2);

            DataColumn columna3 = new DataColumn();
            columna3.DataType = System.Type.GetType("System.String");
            columna3.AllowDBNull = true;
            columna3.Caption = "estatus_sat";
            columna3.ColumnName = "estatus_sat";
            columna3.DefaultValue = null;
            TableUUID.Columns.Add(columna3);

            DataColumn columna4 = new DataColumn();
            columna4.DataType = System.Type.GetType("System.String");
            columna4.AllowDBNull = true;
            columna4.Caption = "fecha";
            columna4.ColumnName = "fecha";
            columna4.DefaultValue = null;
            TableUUID.Columns.Add(columna4);

            DataColumn columna5 = new DataColumn();
            columna5.DataType = System.Type.GetType("System.String");
            columna5.AllowDBNull = true;
            columna5.Caption = "descripcion";
            columna5.ColumnName = "descripcion";
            columna5.DefaultValue = null;
            TableUUID.Columns.Add(columna5);

            DataColumn[] columns = new DataColumn[1];
            columns[0] = TableUUID.Columns["uuid"];
            TableUUID.PrimaryKey = columns;



            for (int coun = 0; coun < ListaUUID.Length; coun++)
            {
                psListaUUID.Add(ListaUUID[coun].ToUpper());
            }
            foreach (string s in ListaUUID.Distinct())
            {
                DataRow row = TableUUID.NewRow();
                row["UUID"] = s;
                TableUUID.Rows.Add(row);
            }

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

                sRetAutentication = fnAutentication(user, password, ref pnId_Usuario, ref datosUsuario);
                string[] est = sRetAutentication.Split('-');
                if (est.Length > 2)
                {
                    sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
                }
                else
                {
                    sRetAutentication = est[0].Trim();
                }

                if (sRetAutentication == "0")
                {
                    if (pnId_Estructura == 0)
                        pnId_Estructura = Convert.ToInt32(clsCancelacionSAT.fnRecuperaMatriz(pnId_Usuario));

                    //Revisa si contiene créditos
                    dtCreditos = clsTimbradoFuncionalidad.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Cancelacion", 'A', 'N');

                    if (dtCreditos.Rows.Count > 0)
                    {
                        css = new clsConfiguracionCreditos();
                        double dCostCred = css.fnRecuperaPrecioServicio(1);
                        double creditos = 0;
                        creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                        if (creditos == 0)
                        {
                            clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                            dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                            double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                            if (creditos2 == 0)
                            {
                                CancelaResponse.Estatus = 97;
                                CancelaResponse.FechaCancelacion = DateTime.Now;
                                CancelaResponse.Resultado = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                                //return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                                return CancelaResponse;

                            }

                            if (creditos2 < dCostCred)
                            {
                                CancelaResponse.Estatus = 97;
                                CancelaResponse.FechaCancelacion = DateTime.Now;
                                CancelaResponse.Resultado = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                                //return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                                return CancelaResponse;
                            }
                        }
                        else
                        {
                            if (creditos < dCostCred)
                            {
                                CancelaResponse.Estatus = 97;
                                CancelaResponse.FechaCancelacion = DateTime.Now;
                                CancelaResponse.Resultado = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                                //return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                                return CancelaResponse;
                            }
                        }
                    }
                    else
                    {
                        clsOperacionDistribuidores gDi = new clsOperacionDistribuidores();
                        dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                        double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                        css = new clsConfiguracionCreditos();
                        double dCostCred = css.fnRecuperaPrecioServicio(2);

                        if (creditos == 0)
                        {
                            CancelaResponse.Estatus = 97;
                            CancelaResponse.FechaCancelacion = DateTime.Now;
                            CancelaResponse.Resultado = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                            //return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                            return CancelaResponse;
                        }

                        if (creditos < dCostCred)
                        {
                            CancelaResponse.Estatus = 97;
                            CancelaResponse.FechaCancelacion = DateTime.Now;
                            CancelaResponse.Resultado = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                            //return clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                            return CancelaResponse;
                        }
                    }

                    if (dtCreditos.Rows.Count > 0)
                    {
                        bool CancelacionPrueba = Convert.ToBoolean(clsComun.ObtenerParamentro("CancelacionPrueba"));
                        if (CancelacionPrueba == false)
                        {
                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WebService Inicio de cancelacion RFC:" + rfc);

                            clsCancelacionSAT cancela = new clsCancelacionSAT();

                            //generacion del request del PAC
                            string request = cancela.fnListaRequest(psListaUUID, rfc, user);

                            //Metodo del web service del SAT para cancelar
                            Respuesta = cancela.CancelarBloqueCfdi(rfc, psListaUUID, datosUsuario, ref TableUUID, pfx, pfxPassword); //PASAR PFX

                            xmldoc.PreserveWhitespace = false;
                            xmldoc.LoadXml(Respuesta);
                            RespuestaFinal = xmldoc.OuterXml;

                            CancelaResponse.Archivo = Encoding.Default.GetBytes(RespuestaFinal);

                            //la respuesta del ws del SAT se carga en un XML y se recorren los nodos para que, según su estatus se
                            // actualicen en la base de datos
                            //-------
                            //int nodos = xmldoc.DocumentElement.ChildNodes.Count;
                            XmlNodeList elemList = xmldoc.GetElementsByTagName("Folios");
                            for (int i = 0; i < elemList.Count; i++)
                            {
                                System.Xml.XmlNode nodo = elemList[i];
                                Respuesta = clsComun.fnRecuperaErrorSAT(nodo.ChildNodes[1].InnerText, "Cancelación");

                                string[] est2 = Respuesta.Split('-');

                                if (est2.Length > 2)
                                {
                                    Respuesta = est2[0].Trim() + " - " + est2[1].Trim();
                                }
                                else
                                {
                                    Respuesta = est2[0].Trim();
                                }
                                //si el estatus de respuesta del SAT del UUID fue 201 cancelamos en la BD
                                if (Respuesta == "201" || Respuesta == "202")
                                {
                                    clsCancelacionSAT cancelacion = new clsCancelacionSAT();

                                    Int32 idcfdi = cancelacion.fnRecuperaIdCFD(nodo.ChildNodes[0].InnerText, datosUsuario.id_usuario);
                                    XmlNode nodeFecha = xmldoc.SelectSingleNode("(/*/@Fecha)");
                                    string fecha = nodeFecha.InnerText;
                                    string UUID = nodo.ChildNodes[0].InnerText;

                                    int retVal = clsTimbradoFuncionalidad.fnCancelarComprobante(idcfdi, fecha, UUID, rfc, "G", datosUsuario.id_usuario);
                                    //id_cancelacion, id_cfd, fecha_cancelacion, UUID, RFC_Emisor, Origen, id_usuario

                                    if (retVal != 0)
                                    {
                                        StringBuilder accion3 = new StringBuilder();
                                        accion3.Append("webConsultasCFDI");
                                        accion3.Append(" | ");
                                        accion3.Append("fnCancelarComprobante");
                                        accion3.Append(" | ");
                                        accion3.Append("Se canceló el comprobante con ID " + idcfdi);
                                    }
                                    if (Respuesta == "201")
                                    {
                                        fnActualizaCreditos(dtCreditos);
                                        //byte[] archivo = Encoding.Default.GetBytes(RespuestaFinal);
                                        CancelaResponse.Estatus = 201;
                                        CancelaResponse.FechaCancelacion = DateTime.Now;
                                        CancelaResponse.Resultado += clsComun.fnRecuperaErrorSAT(Respuesta, "Seguridad");
                                    }
                                    else
                                    {
                                        CancelaResponse.Estatus = 202;
                                        CancelaResponse.FechaCancelacion = DateTime.Now;
                                        CancelaResponse.Resultado += clsComun.fnRecuperaErrorSAT(Respuesta, "Seguridad");
                                    }

                                }
                                else
                                {
                                    CancelaResponse.Estatus = Int32.Parse(Respuesta);
                                    CancelaResponse.FechaCancelacion = DateTime.Now;
                                    CancelaResponse.Resultado += clsComun.fnRecuperaErrorSAT(Respuesta, "Seguridad");
                                }
                            }

                            StringBuilder accion = new StringBuilder();
                            accion.Append("webConsultasCFDI");
                            accion.Append(" | ");
                            accion.Append("CancelarBloqueCfdi");
                            accion.Append(" | ");
                            accion.Append("Envia cancelacion al SAT.");

                            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WebService Fin de cancelacion");
                        }
                        else
                        {
                            fnActualizaCreditos(dtCreditos);
                            RespuestaFinal = "Cancelación Exitosa, Modo de Pruebas";
                        }
                    }
                    else
                    {
                        CancelaResponse.Estatus = 97;
                        CancelaResponse.FechaCancelacion = DateTime.Now;
                        CancelaResponse.Resultado = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                        RespuestaFinal = clsComun.fnRecuperaErrorSAT("97", "Seguridad");
                    }
                }
                else
                {
                    try
                    {
                        string response = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
                        clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, psListaUUID[0].ToString(), response, DateTime.Now, string.Empty, "Response", "C", String.Empty);
                        RespuestaFinal = clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");

                        CancelaResponse.Estatus = Int32.Parse(sRetAutentication);
                        CancelaResponse.FechaCancelacion = DateTime.Now;
                        CancelaResponse.Resultado += clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
                    }
                    catch
                    {
                        CancelaResponse.Estatus = Int32.Parse(sRetAutentication);
                        RespuestaFinal = sRetAutentication;
                    }
                }
            }
            catch (Exception ex)
            {
                clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "GeneralfnCancelarXML: " + ex.Message);

                clsCancelacionSAT cancelacion = new clsCancelacionSAT();
                // No hay respuesta del SAT
                DataRow nuevo = TableUUID.NewRow();

                nuevo["UUID"] = "00000000-0000-0000-0000-000000000000";
                nuevo["estatus"] = "999";
                nuevo["descripcion"] = "999 - No hay respuesta del SAT, favor de intentar de nuevo. ";
                nuevo["fecha"] = DateTime.Now.ToString("s");

                TableUUID.Rows.Add(nuevo);

                CancelaResponse.Estatus = 999;
                CancelaResponse.FechaCancelacion = DateTime.Now;
                CancelaResponse.Resultado += "999 - No hay respuesta del SAT, favor de intentar de nuevo. ";

                RespuestaFinal = cancelacion.fnListaUUID(TableUUID, datosUsuario.rfc, ":signature");
                CancelaResponse.Archivo = Encoding.Default.GetBytes(RespuestaFinal);


            }

            //return RespuestaFinal;
            return CancelaResponse;
        }

        [WebMethod(Description = "Permite realizar el timbrado del comprobante firmado por el cliente en modo de prueba y devuelve el timbre del comprobante timbrado.")]
        public byte[] getTimbreCfdiTest(String user, String password, byte[] file)
        {
            byte[] var = { };

            int pnId_Usuario = 0;
            int pnId_Estructura = 0;
            string scadena = string.Empty;
            string sRequest = string.Empty;
            string sesquema = string.Empty;
            string sResponse = string.Empty;
            string sRetornoSAT = string.Empty;
            string sXmlDocument = string.Empty;
            string sCadenaOriginal = string.Empty;
            string sRetAutentication = string.Empty;
            string sCadenaOriginalEmisor = string.Empty;
            string sVersion = string.Empty;
            string TimbreEnviado = "";

            TimbreFiscalDigital gNodoTimbre;
            clsOperacionTimbradoSelladoTest gTimbrado;
            clsConfiguracionCreditosTest css;
            clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();

            string sComprobante = Encoding.UTF8.GetString(file);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(sComprobante);
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");

            try { sVersion = xmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value; }
            catch { }

            //Revisar version
            if (sVersion != "3.2")
            {
                return Encoding.UTF8.GetBytes("999 - Por disposición oficial estos comprobantes ya no se timbran.");
            }
            else
            {
                sesquema = "esquema_v3_2";
            }

            if (Convert.ToDouble(sComprobante.Length / 1024.0) > 100.0)
            {
                clsPistasAuditoriaTest.fnGenerarPistasAuditoria(0, DateTime.Now, "WSD paso0 - Tamaño de factura excedida: " + user + " Tamaño: " + Convert.ToDouble(sComprobante.Length / 1024.0));
                try
                {
                    bool lista = false;
                    string value = File.ReadAllText(HostingEnvironment.MapPath("VipSizeFiles.xml"));
                    XmlDocument vipList = new XmlDocument();
                    vipList.LoadXml(value);

                    XmlNodeList nodeList = vipList.SelectNodes("/VipList/user");

                    foreach (XmlNode no in nodeList)
                    {
                        string clave_usuario = no.Attributes["clave_usuario"].Value;

                        if (clave_usuario == user)
                        {
                            lista = true;
                            clsPistasAuditoriaTest.fnGenerarPistasAuditoria(0, DateTime.Now, "WSD paso0 - Se encuentra en la lista para poder timbrar " + user + " " + Convert.ToDouble(sComprobante.Length / 1024.0));

                            if (Convert.ToDouble(sComprobante.Length / 1024.0) > 200.0)
                            {
                                return Encoding.UTF8.GetBytes("999 - El tamaño del XML excede el limite permitido VipList.");
                            }
                            else
                            {
                                clsPistasAuditoriaTest.fnGenerarPistasAuditoria(0, DateTime.Now, "WSD paso0 - Continua proceso.");
                                break;
                            }
                        }
                    }
                    if (lista == false)
                    {
                        return Encoding.UTF8.GetBytes("999 - El tamaño del XML excede el limite permitido. N");
                    }
                }
                catch (Exception)
                {
                    return Encoding.UTF8.GetBytes("999 - El tamaño del XML excede el limite permitido. T");
                }
            }

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);

            sRetAutentication = fnAutentication(user, password, ref pnId_Usuario, ref datosUsuario);

            clsPistasAuditoriaTest.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso1 - " + "Verifica el usuario WSDL SVC Timbrado_DP." + "0.0.0.0" + " USER: " + user + " PASSWORD: " + password.Substring(0, 3) + "*************");

            //Revisa encabezado del XML
            try
            {
                if (sComprobante.Contains("<?xml version=\"1.0\" encoding=\"UTF-8\"?>"))
                {
                    sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("301", "Timbrado y Cancelación")); //El archivo de texto esta mal formado
                }
            }
            catch (Exception)
            {
                sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("301", "Timbrado y Cancelación"));
                clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "301", "Response", "E", string.Empty);
                //*******************************************************Insertar Response en tabla de acuses
                return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("301", "Timbrado y Cancelación")); //El archivo de texto esta mal formado
            }

            //Inserta acuse PAC
            try
            {
                XmlDocument acuseXML = new XmlDocument();
                acuseXML.LoadXml(sComprobante);
                //*******************************************************Insertar Request en tabla de acuses
                sRequest = clsComunTest.fnRequestRecepcion(acuseXML.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", ""), "Factura", pnId_Estructura.ToString(), user, password);
                if (clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sRequest, DateTime.Now, "0", "Request", "E", string.Empty) == 0)
                {
                    return Encoding.UTF8.GetBytes("301 - Revise la codificación del documento que sea UTF-8.");
                }
                //*******************************************************Insertar Request en tabla de acuses
            }
            catch
            {
                return Convert.FromBase64String("301 - Revise la codificación del documento que sea UTF-8.");
            }

            string[] est = sRetAutentication.Split('-');
            if (est.Length > 2)
            {
                sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
            }
            else
            {
                sRetAutentication = est[0].Trim();
            }
            if (sRetAutentication == "0")
            {
                if (pnId_Estructura == 0)
                    pnId_Estructura = Convert.ToInt32(fnRecuperaMatriz(pnId_Usuario));

                int pnId_tipo_documento = 1;

                //Revisar los creditos disponibles.
                dtCreditos = clsTimbradoFuncionalidadTest.fnObtenerCreditos(datosUsuario.id_usuario.ToString(), "Timbrado", 'A', 'N');

                if (dtCreditos.Rows.Count > 0)
                {
                    css = new clsConfiguracionCreditosTest();
                    double dCostCred = css.fnRecuperaPrecioServicio(1);
                    double creditos = 0;
                    creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"].ToString());
                    if (creditos == 0)
                    {
                        clsOperacionDistribuidoresTest gDi = new clsOperacionDistribuidoresTest();
                        dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                        double creditos2 = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);

                        if (creditos2 == 0)
                        {
                            return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("97", "Seguridad"));
                        }

                        if (creditos2 < dCostCred)
                        {
                            return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("97", "Seguridad"));
                        }
                    }
                    else
                    {
                        if (creditos < dCostCred)
                        {
                            return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("97", "Seguridad"));
                        }
                    }
                }
                else
                {
                    clsOperacionDistribuidoresTest gDi = new clsOperacionDistribuidoresTest();
                    dtCreditos = gDi.fnObtieneCreditosDistribuidorporUsuario(datosUsuario.id_usuario);
                    double creditos = Convert.ToDouble(dtCreditos.Rows[0]["creditos"]);
                    css = new clsConfiguracionCreditosTest();
                    double dCostCred = css.fnRecuperaPrecioServicio(1);

                    if (creditos == 0)
                    {
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("97", "Seguridad"));
                    }
                    if (creditos < dCostCred)
                    {
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("97", "Seguridad"));
                    }
                }
                try
                {
                    pnId_tipo_documento = clsTimbradoFuncionalidadTest.fnBuscarTipoDocumento("Factura");
                }
                catch (Exception ex)
                {
                    clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Datos, "El nombre de documento no corresponde a ningúno del sistema", pnId_Usuario);
                    //return "406"; //El nombre de documento no corresponde a ningúno del sistema
                    //*******************************************************Insertar Response en tabla de acuses
                    sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("406", "Consulta"));
                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "406", "Response", "E", string.Empty);
                    //*******************************************************Insertar Response en tabla de acuses
                    return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("406", "Consulta"));
                }

                //Recupera datos del emisor.
                clsValCertificadoTest vValidadorCertificado = null;
                XmlDocument xDocTimbrado = new XmlDocument();
                XmlDocument docNodoTimbre;
                gNodoTimbre = new TimbreFiscalDigital();
                string sRFCEmisor = string.Empty;
                string sSello = string.Empty;
                string sNoCertificado = string.Empty;
                string sRFCReceptor = string.Empty;
                string xsd_validacion = string.Empty;
                int retVal;
                string sPassword = string.Empty;
                DataTable tblFechas = new DataTable();
                string noCertificadoPAC = string.Empty;
                string selloPAC = string.Empty;
                string sErrorHSM = String.Empty;
                DateTime dFechaComprobante;
                errorCode = 0;

                //****************************************************************************************************************************************Llamar al Data Power
                clsDataPowerTimbrado dataPower = new clsDataPowerTimbrado();

                string sDocumentoDP = dataPower.clsTimbrarDataPower(sComprobante, System.Configuration.ConfigurationManager.AppSettings["urlDataPowerTest"]);

                if (sDocumentoDP != string.Empty)
                {
                    if (!sDocumentoDP.Contains("<cfdi:Comprobante"))
                    {
                        string sCodigo = "000";
                        try
                        {
                            var codigo = sDocumentoDP.Split('-');
                            sCodigo = codigo[0].ToString();
                        }
                        catch (Exception)
                        { }
                        sDocumentoDP = sDocumentoDP.Replace("<", "&lt;").Replace(">", "&gt;");
                        sResponse = clsComunTest.fnResponseRecepcion(sDocumentoDP);
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sCodigo, "Response", "E", string.Empty);
                        clsPistasAuditoriaTest.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD Resultado - " + "Timbrado Incorrecto, ver acuse.");

                        return Encoding.UTF8.GetBytes(sDocumentoDP);
                    }

                    xDocTimbrado.LoadXml(sComprobante);

                    //Generar la cadena del emisor con los conceptos del XML
                    try
                    {
                        XslCompiledTransform xslt;
                        XsltArgumentList args;
                        MemoryStream ms;
                        StreamReader srDll;
                        switch (sVersion)
                        {
                            case "3.2":
                                // Load the type of the class.
                                xslt = new XslCompiledTransform();
                                xslt.Load(typeof(CaOri.V32));
                                ms = new MemoryStream();
                                args = new XsltArgumentList();
                                xslt.Transform(xDocTimbrado.CreateNavigator(), args, ms);
                                ms.Seek(0, SeekOrigin.Begin);
                                srDll = new StreamReader(ms);
                                sCadenaOriginalEmisor = srDll.ReadToEnd();
                                break;
                        }

                    }
                    catch (Exception)
                    {
                        return Convert.FromBase64String(clsComunTest.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                    }
                    XmlNamespaceManager nsmComprobanteDP = new XmlNamespaceManager(xDocTimbrado.NameTable);
                    try
                    {
                        xDocTimbrado.LoadXml(sDocumentoDP);
                        nsmComprobanteDP.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                        nsmComprobanteDP.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                        gNodoTimbre.FechaTimbrado = Convert.ToDateTime(xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobanteDP).Value);
                        gNodoTimbre.selloCFD = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloCFD", nsmComprobanteDP).Value;
                        gNodoTimbre.selloSAT = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobanteDP).Value;
                        gNodoTimbre.UUID = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteDP).Value;
                        gNodoTimbre.version = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@version", nsmComprobanteDP).Value;
                        gNodoTimbre.noCertificadoSAT = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobanteDP).Value;

                        //Generamos el primer XML necesario para generar la cadena original
                        gTimbrado = new clsOperacionTimbradoSelladoTest();
                        docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                        //Generamos la cadena original
                        XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();
                        XslCompiledTransform xslt;
                        XsltArgumentList args;
                        MemoryStream ms;
                        StreamReader srDll;

                        // Load the type of the class.
                        xslt = new XslCompiledTransform();
                        xslt.Load(typeof(Timbrado.V3.TFDXSLT));
                        ms = new MemoryStream();
                        args = new XsltArgumentList();
                        xslt.Transform(navNodoTimbre, args, ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        srDll = new StreamReader(ms);

                        sCadenaOriginal = srDll.ReadToEnd();
                    }
                    catch (Exception)
                    {
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                    }

                    string cadenaConDP = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbradoTest"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaConDP)))
                    {
                        con.Open();

                        using (SqlTransaction tran = con.BeginTransaction())
                        {
                            try
                            {
                                //Buscar el HASH del comprobante de la cadena original del emisor.
                                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso24 - " + "Busca el HAS del emisor.");
                                string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginalEmisor).ToUpper();
                                string Comprobante = clsTimbradoFuncionalidadTest.fnBuscarHashCompXML(datosUsuario.id_usuario, HASHEmisor, "Emisor");
                                if (Comprobante != "0")
                                {
                                    XmlDocument hasComprobante = new XmlDocument();
                                    hasComprobante.LoadXml(Comprobante);

                                    XmlNamespaceManager nsmComprobante2 = new XmlNamespaceManager(hasComprobante.NameTable);
                                    nsmComprobante2.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                    nsmComprobante2.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                                    nsmComprobante2.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
                                    XmlNode TimbreGeneral = hasComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante2);
                                    TimbreEnviado = TimbreGeneral.OuterXml;

                                    //return "98";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("98", "Timbrado"));
                                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    //return clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                                    return Encoding.UTF8.GetBytes(TimbreEnviado);
                                }

                                //Buscar el HASH del comprobante de la cadena original del timbre fiscal.
                                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso25 - " + "Busca el HAS del TFD.");
                                string HASHTimbreFiscal = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();
                                if (clsTimbradoFuncionalidadTest.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbreFiscal, "Timbre"))
                                {
                                    //return "98";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("98", "Timbrado"));
                                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("98", "Timbrado"));
                                }
                                //--------------------------------------------------------------------
                                string sUUID = string.Empty;
                                string sSerie = string.Empty;
                                string sfolio = string.Empty;
                                string sTotal = string.Empty;
                                string sMoneda = string.Empty;
                                string sRfcEmisor = string.Empty;
                                string sRfcReceptor = string.Empty;
                                string sEmisorNombre = string.Empty;
                                string sFecha_emision = string.Empty;
                                string sFechaTimbrado = string.Empty;
                                string sReceptorNombre = string.Empty;
                                //--------------------------------------------------------------------
                                try { sUUID = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteDP).Value; }
                                catch { }
                                try { sFechaTimbrado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobanteDP).Value; }
                                catch { }
                                //--------------------------------------------------------------------
                                try { sSerie = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobanteDP).Value; }
                                catch { }
                                try { sfolio = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobanteDP).Value; }
                                catch { }
                                try { sTotal = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobanteDP).Value; }
                                catch { }
                                try { sMoneda = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobanteDP).Value; }
                                catch { }
                                try { sRfcEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobanteDP).Value; }
                                catch { }
                                try { sRfcReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobanteDP).Value; }
                                catch { }
                                try { sEmisorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobanteDP).Value; }
                                catch { }
                                try { sFecha_emision = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobanteDP).Value; }
                                catch { }
                                try { sReceptorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobanteDP).Value; }
                                catch { }
                                //--------------------------------------------------------------------

                                using (SqlCommand command = new SqlCommand("usp_Timbrado_InsertaComprobanteAll_Completo_Ins_Indicium", con))
                                {
                                    command.Transaction = tran;
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    command.CommandTimeout = 200;
                                    command.Parameters.AddWithValue("sXML", xDocTimbrado.DocumentElement.OuterXml);
                                    command.Parameters.AddWithValue("nId_tipo_documento", pnId_tipo_documento);
                                    command.Parameters.AddWithValue("cEstatus", "P");
                                    command.Parameters.AddWithValue("dFecha_Documento", DateTime.Now);
                                    command.Parameters.AddWithValue("nId_estructura", pnId_Estructura);
                                    command.Parameters.AddWithValue("nId_usuario_timbrado", pnId_Usuario);
                                    command.Parameters.AddWithValue("nSerie", "N/A");
                                    command.Parameters.AddWithValue("sOrigen", "R");
                                    command.Parameters.AddWithValue("sHash", HASHTimbreFiscal);
                                    command.Parameters.AddWithValue("sDatos", HASHEmisor);
                                    command.Parameters.AddWithValue("@sUuid", sUUID);
                                    command.Parameters.AddWithValue("@dFecha_Timbrado", sFechaTimbrado);
                                    command.Parameters.AddWithValue("@sRFC_Emisor", sRfcEmisor);
                                    command.Parameters.AddWithValue("@sNombre_Emisor", sEmisorNombre);
                                    command.Parameters.AddWithValue("@sRFC_Receptor", sRfcReceptor);
                                    command.Parameters.AddWithValue("@sNombre_Receptor", sReceptorNombre);
                                    command.Parameters.AddWithValue("@dFecha_Emision", sFecha_emision);
                                    command.Parameters.AddWithValue("@sSerie", sSerie);
                                    command.Parameters.AddWithValue("@sFolio", sfolio);
                                    command.Parameters.AddWithValue("@nTotal", sTotal);
                                    command.Parameters.AddWithValue("@sMoneda", sMoneda);

                                    retVal = Convert.ToInt32(command.ExecuteScalar());
                                }
                                fnActualizaCreditos(dtCreditos);

                                tran.Commit();

                                if (retVal == 0)
                                {
                                    //return "999";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("999", "Timbrado"));
                                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("999", "Timbrado"));
                                }
                                else
                                {
                                    clsPistasAuditoriaTest.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso27 - " + "Recupera XML para regresarlo al cliente.");
                                    sXmlDocument = xDocTimbrado.OuterXml;

                                    //Enviar comprobante al SAT --Comentado para mejorar el rendimento de entrega de comprobante.
                                    //clsEnvioSAT enviarSAT = new clsEnvioSAT();
                                    //sRetornoSAT = enviarSAT.fnEnviarBloqueCfdi(HASH, pnId_Usuario, xDocTimbrado, retVal, datosUsuario);

                                    //*******************************************************Insertar Response en tabla de acuses
                                    //string uuid = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                                    string uuid = gNodoTimbre.UUID;
                                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, uuid.ToUpper(), sDocumentoDP, DateTime.Now, "0", "Response", "E", string.Empty);
                                    sXmlDocument = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobanteDP).OuterXml;
                                    //*******************************************************Insertar Response en tabla de acuses
                                }
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante", pnId_Usuario);
                                //return "999"; //Error durante el registro del comprobante
                                //*******************************************************Insertar Response en tabla de acuses
                                sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("999", "Timbrado"));
                                clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                                //*******************************************************Insertar Response en tabla de acuses
                                return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("999", "Timbrado"));
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                    return Encoding.UTF8.GetBytes(sXmlDocument);
                }
                else
                {
                    //return Encoding.UTF8.GetBytes("999 - Servicio No disponible, comuníquese con PAX Facturación.");
                    //****************************************************************************************************************************************Llamar al Data Power
                    //****************************************************************************************************************************************Llamar al Data Power

                    //Verificamos que el XML cumpla con el esquema de SAT
                    try
                    {
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso5 - " + "Verifica el esquema.");
                        xsd_validacion = fnValidate(sComprobante, sesquema);//"esquema_v3");


                        if (xsd_validacion != string.Empty && xsd_validacion != null)
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("333", "Consulta"));
                            clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "333", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes("333 - " + xsd_validacion);
                        }

                    }
                    catch (Exception ex)
                    {
                        clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Datos, "El XML no cumple con el esquema de hacienda", pnId_Usuario);
                        //return "333";
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComun.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("333", "Consulta"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "333", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("333", "Consulta"));
                    }

                    try
                    {
                        if (sComprobante.ToLowerInvariant().IndexOf('|') != -1)
                        {

                            return Encoding.UTF8.GetBytes("301 - XML mal formado. |");
                        }
                    }
                    catch (Exception ex)
                    {
                        return Encoding.UTF8.GetBytes("301 - XML mal formado. |");
                    }




                    //Recuperamos el certificado a partir del XML del comprobante
                    try
                    {
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso6 - " + "Recupera Certificado.");
                        vValidadorCertificado = fnRecuperarCertificadoTest(sComprobante);

                        if (vValidadorCertificado == null)
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("570", "Timbrado"));
                            clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "570", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("570", "Timbrado"));
                        }
                    }
                    catch (Exception ex)
                    {
                        clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Datos, "No se pudó recuperar el certificado del comprobante", pnId_Usuario);
                        // return "570";
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("570", "Timbrado"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "570", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("570", "Timbrado"));
                    }



                    //Verificamos que el certificado del comprobante se de tipo CSD
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso7 - " + "Verifica certificado sea CSD.");
                    if (!vValidadorCertificado.fnEsCSD())
                    {
                        clsErrorLogTest.fnNuevaEntrada(new Exception("El certificado no es de tipo CSD"), clsErrorLogTest.TipoErroresLog.Datos, "El certificado no es de tipo CSD", pnId_Usuario);
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("306", "Timbrado y Cancelación"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "306", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("306", "Timbrado y Cancelación")); //El certificado no es de tipo CSD
                    }



                    //Certificado no expedido por el SAT
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso8 - " + "Verifica certificado no apocrifo");
                    if (!vValidadorCertificado.fnCSD308())
                    {
                        clsErrorLogTest.fnNuevaEntrada(new Exception("Certificado no expedido por el SAT"), clsErrorLogTest.TipoErroresLog.Datos, "Certificado no expedido por el SAT", pnId_Usuario);
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("308", "Timbrado y Cancelación"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "308", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("308", "Timbrado y Cancelación")); //El certificado no es de tipo CSD
                    }



                    try
                    {
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso9 - " + "Recupera datos de XML");
                        xDocTimbrado.LoadXml(sComprobante);
                        sRFCEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
                        dFechaComprobante = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;
                        sSello = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value;
                        sNoCertificado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value;
                        sRFCReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;


                    }
                    catch (Exception ex)
                    {
                        clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Datos, "Faltan datos del comprobante", pnId_Usuario);
                        //return "799";//Faltan datos del comprobante
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("N-504", "Recepción"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "N - 504", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("N-504", "Recepción"));

                    }


                    //Verificar que no contenga adenda el comprobante.
                    try
                    {
                        if (xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante) != null)
                        {
                            if (xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda", nsmComprobante).LocalName != string.Empty)
                            {
                                return Encoding.UTF8.GetBytes("510 - No esta permitido enviar adendas en el comprobante.");
                            }
                        }

                    }
                    catch
                    {
                        return Encoding.UTF8.GetBytes("510 - No esta permitido enviar adendas en el comprobante.");
                    }


                    try
                    {
                        //Validar el RFC del receptor 18/06/2012 cambios en la recepcion del sat.
                        if (!clsComun.fnValidaExpresion(sRFCReceptor.Trim(), @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                        {
                            return Encoding.UTF8.GetBytes("509 - Verifique el RFC del receptor.");
                        }

                    }
                    catch (Exception)
                    {
                        return Encoding.UTF8.GetBytes("509 - Verifique el RFC del receptor.");
                    }



                    //verificamos si el comprobante ya contiene un nodo timbre
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso10 - " + "Revisar Timbre Previo.");
                    XmlNode aux = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante);

                    if (aux != null)
                    {
                        // return "472"; //El comprobante ya está timbrado
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("307", "Timbrado y Cancelación"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "307", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("307", "Timbrado y Cancelación"));
                    }




                    //Generar la cadena del emisor con los conceptos del XML
                    try
                    {

                        XslCompiledTransform xslt;
                        XsltArgumentList args;
                        MemoryStream ms;
                        StreamReader srDll;

                        switch (sVersion)
                        {
                            case "3.2":

                                // Load the type of the class.
                                xslt = new XslCompiledTransform();
                                xslt.Load(typeof(CaOri.V32));

                                ms = new MemoryStream();
                                args = new XsltArgumentList();

                                xslt.Transform(xDocTimbrado.CreateNavigator(), args, ms);
                                ms.Seek(0, SeekOrigin.Begin);
                                srDll = new StreamReader(ms);

                                sCadenaOriginalEmisor = srDll.ReadToEnd();

                                break;
                        }

                    }
                    catch (Exception)
                    {
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                    }



                    try
                    {
                        //303------Preparamos los objetos de manejo tanto de la llave como del certificado---303
                        //Validamos el sello de emisor
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso12 - " + "Revisar que corresponda la candena del emisor.");
                        if (string.IsNullOrEmpty(sSello) || !(sCadenaOriginalEmisor.Contains(sRFCEmisor)))
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                            clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "303", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                        }
                    }
                    catch (Exception)
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "303", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("303", "Timbrado y Cancelación"));

                    }



                    //Certificado revocado o caduco R o C
                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso13 - " + "Revisa caducidad del certificado.");
                    if (!fnVerificarCaducidadCertificadoTest(sNoCertificado, "R", "C", ref vValidadorCertificado))
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("304", "Timbrado y Cancelación"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "304", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("304", "Timbrado y Cancelación"));
                    }



                    try
                    {
                        //La fecha de emisión no esta dentro de la vigencia del CSD del Emisor
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso14 - " + "Revisar vigencia del certificado.");
                        if (!vValidadorCertificado.ComprobarFechas() || !fnRecuperaFechaLCOTest(sNoCertificado, "A", ref vValidadorCertificado))
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("305", "Timbrado y Cancelación"));
                            clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "305", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("305", "Timbrado y Cancelación"));
                        }
                    }
                    catch (Exception)
                    {
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("305", "Timbrado y Cancelación"));
                    }



                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso15 - " + "Revisar Fecha del comprobante no sea menor a 2011.");
                    if (!clsComun.fnRevisarFechaNoPosterior(dFechaComprobante))
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("403", "Timbrado"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "403", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("403", "Timbrado"));
                    }


                    //RFC del emisor no se encuentra en el régimen de contribuyentes 402
                    try
                    {
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso16 - " + "Revisar en el LCO.");
                        string rfcComprobante = vValidadorCertificado.VerificarExistenciaCertificado();
                        if (sRFCEmisor != rfcComprobante)
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("402", "Timbrado"));
                            clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "402", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("402", "Timbrado"));
                        }
                    }
                    catch
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("402", "Timbrado"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "402", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("402", "Timbrado"));
                    }


                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso17 - " + "Revisar fecha dentro de 72 horas.");
                    if (!vValidadorCertificado.fnFechaContraPeriodoValidez(dFechaComprobante))
                    {
                        // return "504"; //La fecha del comprobante esta fuera de periodo
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("401", "Timbrado"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "401", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("401", "Timbrado"));
                    }



                    //Revisamos que esl sello del emisor sea valido.
                    try
                    {
                        //Preparamos los objetos de manejo tanto de la llave como del certificado
                        gTimbrado = new clsOperacionTimbradoSelladoTest();
                        // *************Comentado por limitar rendimiento al HSM*************************************************
                        //Validamos el sello de emisor
                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso18 - " + "Revisar sello del emisor.");
                        if (!vValidadorCertificado.fnVerificarSello(sCadenaOriginalEmisor, sSello))
                        {
                            //*******************************************************Insertar Response en tabla de acuses
                            sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                            clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "302", "Response", "E", string.Empty);
                            //*******************************************************Insertar Response en tabla de acuses
                            return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                        }
                        // *************Comentado por limitar rendimiento al HSM*************************************************
                    }
                    catch (Exception)
                    {
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "302", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("302", "Timbrado y Cancelación"));
                    }



                    try
                    {

                        //Llenamos los datos del nodo timbre
                        gNodoTimbre.UUID = Guid.NewGuid().ToString();
                        gNodoTimbre.FechaTimbrado = Convert.ToDateTime(DateTime.Now.ToString("s"));
                        gNodoTimbre.selloCFD = sSello;

                        //****************************************************BLOQUE HSM********************************************************
                        //****************************************************BLOQUE HSM********************************************************

                        wslServicioPAC gServicio = new wslServicioPAC();
                        ////Generamos el primer XML necesario para generar la cadena original
                        docNodoTimbre = null;

                        try
                        {

                            //HSM3
                            if (!fnFirmaHSMTerciarioTest(gServicio, ref docNodoTimbre, vValidadorCertificado, ref sCadenaOriginal, "SeccionA", ref sErrorHSM))
                            {
                                try
                                {
                                    throw new Exception("No se pudo timbrar la factura, con el HSM3");
                                }
                                catch (Exception ex)
                                {
                                    clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Datos, "No se pudo timbrar la factura, con el HSM3", pnId_Usuario);
                                }

                                return Encoding.UTF8.GetBytes("999 - El sistema esta ocupado, por favor intente de nuevo.");
                            }

                        }
                        catch (Exception exHsm)
                        {
                            sErrorHSM = "Ocurrio un error al buscar los HSM Principal y Secundario. " + exHsm.Message;
                            //HSM3
                            if (!fnFirmaHSMTerciarioTest(gServicio, ref docNodoTimbre, vValidadorCertificado, ref sCadenaOriginal, "SeccionC", ref sErrorHSM))
                            {
                                try
                                {
                                    throw new Exception("No se pudo timbrar la factura, con el HSM3");
                                }
                                catch (Exception ex)
                                {
                                    clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Datos, "No se pudo timbrar la factura, con el HSM3", pnId_Usuario);
                                }

                                return Encoding.UTF8.GetBytes("999 - El sistema esta ocupado, por favor intente de nuevo.");
                            }
                        }



                        //****************************************************BLOQUE HSM********************************************************
                        //****************************************************BLOQUE HSM********************************************************

                        //xDocTimbrado = gTimbrado.fnGenerarXML(gNodoTimbre);// Reescribe el documento. No Borrar.

                        //***********Genera la firma con el HSM*************************************

                        //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso23 - " + "Cargando timbre al documento.");

                        docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                        XmlNode Complemento = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsmComprobante);
                        if (Complemento == null)
                            Complemento = xDocTimbrado.CreateElement("cfdi", "Complemento", nsmComprobante.LookupNamespace("cfdi"));

                        Complemento.InnerXml = docNodoTimbre.DocumentElement.OuterXml + Complemento.InnerXml;

                        xDocTimbrado.DocumentElement.AppendChild(Complemento);


                        //Eliminamos la llave y el certificado
                        gTimbrado.fnDestruirLlave();
                    }
                    catch (Exception ex)
                    {
                        clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Datos, "No se pudo generar el sello del PAC", pnId_Usuario);
                        //return "817"; //No se pudo generar el sello del PAC
                        //*******************************************************Insertar Response en tabla de acuses
                        sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("817", "Timbrado"));
                        clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "817", "Response", "E", string.Empty);
                        //*******************************************************Insertar Response en tabla de acuses
                        return Encoding.UTF8.GetBytes("999 - El sistema esta ocupado, por favor intente de nuevo.");
                    }

                    string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
                    {
                        con.Open();

                        using (SqlTransaction tran = con.BeginTransaction())
                        {

                            try
                            {

                                //Buscar el HASH del comprobante de la cadena original del emisor.
                                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso24 - " + "Busca el HAS del emisor.");

                                string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginalEmisor).ToUpper();
                                string Comprobante = clsTimbradoFuncionalidadTest.fnBuscarHashCompXML(datosUsuario.id_usuario, HASHEmisor, "Emisor");
                                if (Comprobante != "0")
                                {
                                    XmlDocument hasComprobante = new XmlDocument();
                                    hasComprobante.LoadXml(Comprobante);

                                    XmlNamespaceManager nsmComprobante2 = new XmlNamespaceManager(hasComprobante.NameTable);
                                    nsmComprobante2.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                    nsmComprobante2.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                                    nsmComprobante2.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
                                    XmlNode TimbreGeneral = hasComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante2);
                                    TimbreEnviado = TimbreGeneral.OuterXml;

                                    //return "98";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("98", "Timbrado"));
                                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    //return clsComun.fnRecuperaErrorSAT("98", "Timbrado");
                                    return Encoding.UTF8.GetBytes(TimbreEnviado);
                                }

                                //Buscar el HASH del comprobante de la cadena original del timbre fiscal.
                                //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso25 - " + "Busca el HAS del TFD.");
                                string HASHTimbreFiscal = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();
                                if (clsTimbradoFuncionalidadTest.fnBuscarHashComprobantes(datosUsuario.id_usuario, HASHTimbreFiscal, "Timbre"))
                                {
                                    //return "98";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("98", "Timbrado"));
                                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "98", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("98", "Timbrado"));
                                }
                                //--------------------------------------------------------------------
                                string sUUID = string.Empty;
                                string sSerie = string.Empty;
                                string sfolio = string.Empty;
                                string sTotal = string.Empty;
                                string sMoneda = string.Empty;
                                string sRfcEmisor = string.Empty;
                                string sRfcReceptor = string.Empty;
                                string sEmisorNombre = string.Empty;
                                string sFecha_emision = string.Empty;
                                string sFechaTimbrado = string.Empty;
                                string sReceptorNombre = string.Empty;
                                //--------------------------------------------------------------------
                                try { sUUID = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                                catch { }
                                try { sFechaTimbrado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value; }
                                catch { }
                                //--------------------------------------------------------------------
                                try { sSerie = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value; }
                                catch { }
                                try { sfolio = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
                                catch { }
                                try { sTotal = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value; }
                                catch { }
                                try { sMoneda = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante).Value; }
                                catch { }
                                try { sRfcEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value; }
                                catch { }
                                try { sRfcReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value; }
                                catch { }
                                try { sEmisorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value; }
                                catch { }
                                try { sFecha_emision = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value; }
                                catch { }
                                try { sReceptorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value; }
                                catch { }
                                //--------------------------------------------------------------------
                                using (SqlCommand command = new SqlCommand("usp_Timbrado_InsertaComprobanteAll_Completo_Ins_Indicium", con))
                                {

                                    command.Transaction = tran;
                                    command.CommandType = System.Data.CommandType.StoredProcedure;
                                    command.CommandTimeout = 200;

                                    command.Parameters.AddWithValue("sXML", xDocTimbrado.DocumentElement.OuterXml);
                                    command.Parameters.AddWithValue("nId_tipo_documento", pnId_tipo_documento);
                                    command.Parameters.AddWithValue("cEstatus", "P");
                                    command.Parameters.AddWithValue("dFecha_Documento", DateTime.Now);
                                    command.Parameters.AddWithValue("nId_estructura", pnId_Estructura);
                                    command.Parameters.AddWithValue("nId_usuario_timbrado", pnId_Usuario);
                                    command.Parameters.AddWithValue("nSerie", "N/A");
                                    command.Parameters.AddWithValue("sOrigen", "R");
                                    command.Parameters.AddWithValue("sHash", HASHTimbreFiscal);
                                    command.Parameters.AddWithValue("sDatos", HASHEmisor);
                                    command.Parameters.AddWithValue("@sUuid", sUUID);
                                    command.Parameters.AddWithValue("@dFecha_Timbrado", sFechaTimbrado);
                                    command.Parameters.AddWithValue("@sRFC_Emisor", sRfcEmisor);
                                    command.Parameters.AddWithValue("@sNombre_Emisor", sEmisorNombre);
                                    command.Parameters.AddWithValue("@sRFC_Receptor", sRfcReceptor);
                                    command.Parameters.AddWithValue("@sNombre_Receptor", sReceptorNombre);
                                    command.Parameters.AddWithValue("@dFecha_Emision", sFecha_emision);
                                    command.Parameters.AddWithValue("@sSerie", sSerie);
                                    command.Parameters.AddWithValue("@sFolio", sfolio);
                                    command.Parameters.AddWithValue("@nTotal", sTotal);
                                    command.Parameters.AddWithValue("@sMoneda", sMoneda);
                                    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso26 - " + "Inserta Comprobante.");
                                    retVal = Convert.ToInt32(command.ExecuteScalar());
                                }

                                //fnActualizaCreditos(dtCreditos);

                                tran.Commit();


                                if (retVal == 0)
                                {
                                    //return "999";
                                    //*******************************************************Insertar Response en tabla de acuses
                                    sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("999", "Timbrado"));
                                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                                    //*******************************************************Insertar Response en tabla de acuses
                                    return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("999", "Timbrado"));
                                }
                                else
                                {

                                    clsPistasAuditoriaTest.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso27 - " + "Recupera XML para regresarlo al cliente.");
                                    sXmlDocument = xDocTimbrado.OuterXml;
                                    fnActualizaCreditos(dtCreditos);

                                    //Enviar comprobante al SAT --Comentado para mejorar el rendimento de entrega de comprobante.
                                    //clsEnvioSAT enviarSAT = new clsEnvioSAT();
                                    //sRetornoSAT = enviarSAT.fnEnviarBloqueCfdi(HASH, pnId_Usuario, xDocTimbrado, retVal, datosUsuario);

                                    //*******************************************************Insertar Response en tabla de acuses
                                    string uuid = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, uuid.ToUpper(), sXmlDocument, DateTime.Now, "0", "Response", "E", string.Empty);
                                    sXmlDocument = xDocTimbrado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).OuterXml;
                                    //*******************************************************Insertar Response en tabla de acuses
                                }


                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante HSM ASMX", pnId_Usuario);
                                //return "999"; //Error durante el registro del comprobante
                                //*******************************************************Insertar Response en tabla de acuses
                                sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT("999", "Timbrado"));
                                clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "999", "Response", "E", string.Empty);
                                //*******************************************************Insertar Response en tabla de acuses
                                return Encoding.UTF8.GetBytes(clsComunTest.fnRecuperaErrorSAT("999", "Timbrado"));
                            }
                            finally
                            {
                                //tran.Commit();
                                con.Close();
                            }

                        }
                    }

                }
            }
            else
            {
                try
                {
                    sResponse = clsComunTest.fnResponseRecepcion(clsComunTest.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
                    clsComunTest.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sRetAutentication, "Response", "E", string.Empty);
                    sXmlDocument = clsComunTest.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
                }
                catch
                {
                    sXmlDocument = clsComunTest.fnRecuperaErrorSAT("95", "Seguridad"); ;
                }
            }
            return Encoding.UTF8.GetBytes(sXmlDocument);
        }

        [WebMethod(Description = "obtener el acuse del timbrado del SAT.")]
        public byte[] getCfdiAck(String user, String password, String[] uuid)
        {
            byte[] var = { };

            int pnId_Usuario = 0;
            int pnId_Estructura = 0;
            string sRetAutentication = string.Empty;
            ArrayList psListaUUID = new ArrayList();

            DataTable TableUUID = new DataTable();

            DataColumn columna1 = new DataColumn();
            columna1.DataType = System.Type.GetType("System.String");
            columna1.AllowDBNull = true;
            columna1.Caption = "UUID";
            columna1.ColumnName = "UUID";
            columna1.DefaultValue = null;
            TableUUID.Columns.Add(columna1);

            DataColumn columna2 = new DataColumn();
            columna2.DataType = System.Type.GetType("System.String");
            columna2.AllowDBNull = true;
            columna2.Caption = "estatus";
            columna2.ColumnName = "estatus";
            columna2.DefaultValue = null;
            TableUUID.Columns.Add(columna2);

            DataColumn columna3 = new DataColumn();
            columna3.DataType = System.Type.GetType("System.String");
            columna3.AllowDBNull = true;
            columna3.Caption = "estatus_sat";
            columna3.ColumnName = "estatus_sat";
            columna3.DefaultValue = null;
            TableUUID.Columns.Add(columna3);

            DataColumn columna4 = new DataColumn();
            columna4.DataType = System.Type.GetType("System.String");
            columna4.AllowDBNull = true;
            columna4.Caption = "fecha";
            columna4.ColumnName = "fecha";
            columna4.DefaultValue = null;
            TableUUID.Columns.Add(columna4);

            DataColumn columna5 = new DataColumn();
            columna5.DataType = System.Type.GetType("System.String");
            columna5.AllowDBNull = true;
            columna5.Caption = "descripcion";
            columna5.ColumnName = "descripcion";
            columna5.DefaultValue = null;
            TableUUID.Columns.Add(columna5);

            DataColumn[] columns = new DataColumn[1];
            columns[0] = TableUUID.Columns["uuid"];
            TableUUID.PrimaryKey = columns;

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
            sRetAutentication = fnAutentication(user, password, ref pnId_Usuario, ref datosUsuario);
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso1 - " + "Verifica el usuario WSDL SVC Timbrado_DP." + "0.0.0.0" + " USER: " + user + " PASSWORD: " + password.Substring(0, 3) + "*************");

            string[] est = sRetAutentication.Split('-');
            if (est.Length > 2)
            {
                sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
            }
            else
            {
                sRetAutentication = est[0].Trim();
            }
            if (sRetAutentication == "0")
            {
                if (pnId_Estructura == 0)
                    pnId_Estructura = Convert.ToInt32(fnRecuperaMatriz(pnId_Usuario));
            }
            else
            {
                try
                {
                    string sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sRetAutentication, "Response", "E", string.Empty);
                    var = Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
                    return var;
                }
                catch
                {
                    var = Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("95", "Seguridad"));
                    return var;
                }
            }

            for (int coun = 0; coun < uuid.Length; coun++)
            {
                psListaUUID.Add(uuid[coun].ToUpper());
            }

            string sAcuse = string.Empty;

            sAcuse = "";

            clsCancelacionSAT cancelacion = new clsCancelacionSAT();
            ///SE REMPLAZA POR ACUSE PERSONALIZADO
            for (int coun = 0; coun < psListaUUID.Count; coun++)
            {
                //metodo para recuperar el identificador del UUID en la base de datos
                int valor = cancelacion.fnRecuperaIdCFD(psListaUUID[coun].ToString(), datosUsuario.id_usuario);
                if (valor != 0)
                {
                    //metodo para recuperar el estatus del UUID en la base de datos


                    string estatus = cancelacion.fnRecuperaEstatusCFD(psListaUUID[coun].ToString(), datosUsuario.id_usuario);
                    if (estatus == "C")
                    {
                        DataRow nuevo = TableUUID.NewRow();
                        nuevo["UUID"] = psListaUUID[coun].ToString();
                        nuevo["estatus"] = "202";
                        nuevo["descripcion"] = "202 - UUID previamente cancelado";
                        nuevo["fecha"] = "" + DateTime.Now.ToString("s");
                        TableUUID.Rows.Add(nuevo);
                    }
                    else if (estatus == "P")
                    {
                        DataRow nuevo = TableUUID.NewRow();
                        nuevo["UUID"] = psListaUUID[coun].ToString();
                        nuevo["estatus"] = "204";
                        nuevo["descripcion"] = " 204 - UUID no aplicable para cancelación";
                        nuevo["fecha"] = "" + DateTime.Now.ToString("s");
                        TableUUID.Rows.Add(nuevo);

                    }
                    else if (estatus == "A")
                    {
                        DataRow nuevo = TableUUID.NewRow();
                        nuevo["UUID"] = psListaUUID[coun].ToString();
                        nuevo["estatus"] = "201";
                        nuevo["descripcion"] = " 201 - UUID Entregado al SAT";
                        nuevo["fecha"] = "" + DateTime.Now.ToString("s");
                        TableUUID.Rows.Add(nuevo);
                    }
                    else
                    {
                        DataRow nuevo = TableUUID.NewRow();
                        nuevo["UUID"] = psListaUUID[coun].ToString();
                        nuevo["estatus"] = "204";
                        nuevo["descripcion"] = " 204 - UUID no aplicable para cancelación";
                        nuevo["fecha"] = "" + DateTime.Now.ToString("s");
                        TableUUID.Rows.Add(nuevo);
                    }
                }
                else
                {
                    DataRow nuevo = TableUUID.NewRow();
                    nuevo["UUID"] = psListaUUID[coun].ToString();
                    nuevo["estatus"] = "205";
                    nuevo["descripcion"] = "205 - UUID no existe";
                    nuevo["fecha"] = "" + DateTime.Now.ToString("s");
                    TableUUID.Rows.Add(nuevo);
                }
            }
            //ACUSE PERSONALIZADO
            MemoryStream output = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(output);
            zipStream.SetLevel(3);

            int acum = 0;


            //------------------------------------------------------------------------------------
            foreach (DataRow renglon in TableUUID.Rows)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<Acuse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Fecha=\"" + DateTime.Now.ToString("s") + "\" NoCertificadoSAT=\"20001000000100005761\" UUID=\"" + renglon["UUID"] + "\" CodEstatus=\"" + renglon["descripcion"] + "\">");
                sb.Append("<Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\" Id=\"SelloSAT\">");
                sb.Append("<SignedInfo>");
                sb.Append("<CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\" />");
                sb.Append("<SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#hmac-sha512\" />");
                sb.Append("<Reference URI=\"\">");
                sb.Append("<Transforms>");
                sb.Append("<Transform Algorithm=\"http://www.w3.org/TR/1999/REC-xpath-19991116\">");
                sb.Append("<XPath>not(ancestor-or-self::*[local-name()='Signature'])</XPath>");
                sb.Append("</Transform>");
                sb.Append("</Transforms>");
                sb.Append("<DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha512\" />");
                sb.Append("<DigestValue>1ehjz+jt8JuVC5VLf1TGCrmPEdUy+LdQDmYG87uHvIomQ67iBsg8b6MRLt969YQ/AsosPLgf3SMxpBptrTdEgg==</DigestValue>");
                sb.Append("</Reference>");
                sb.Append("</SignedInfo>");
                sb.Append("<SignatureValue>rzBcTvUFuIcpyO9NAZvovm0BVIG0XP3jUI5MUnrt7w1j4SxlHp4r8fMxqn1moaEbUfVwoKd+uYZK9V73iyfsTw==</SignatureValue>");
                sb.Append("<KeyInfo>");
                sb.Append("<KeyName>00001088888810000001</KeyName>");
                sb.Append("<KeyValue>");
                sb.Append("<RSAKeyValue>");
                sb.Append("<Modulus>vAr6QLmcvW6auTg7a+Ogm0veNvqJ30rD3j0iSAHxGzGVrg1d0xl0Fj5l+JX9EivD+qhkSY7pfLnJoObLpQ3GGZZOOihJVS2tbJDmnn9TW8fKUOVg+jGhcnpCHaUPq/Poj8I2OVb3g7hiaREORm6tLtzOIjkOv9INXxIpRMx54cw46D5F1+0M7ECEVO8Jg+3yoI6OvDNBH+jABsj7SutmSnL1Tov/omIlSWausdbXqykcl10BLu2XiQAc6KLnl0+Ntzxoxk+dPUSdRyR7f3Vls6yUlK/+C/4FacbR+fszT0XIaJNWkHaTOoqz76Ax9XgTv9UuT67j7rdTVzTvAN363w==</Modulus>");
                sb.Append("<Exponent>AQAB</Exponent>");
                sb.Append("</RSAKeyValue>");
                sb.Append("</KeyValue>");
                sb.Append("</KeyInfo>");
                sb.Append("</Signature>");
                sb.Append("</Acuse>");

                sAcuse = sb.ToString();
                //Crea XML
                XmlDocument docXmlDocument = new XmlDocument();
                docXmlDocument.LoadXml(sAcuse);
                MemoryStream stream = new MemoryStream();
                docXmlDocument.Save(stream);

                stream.Flush();
                stream.Position = 0;




                ZipEntry xmlEntry = new ZipEntry(renglon["UUID"] + ".xml");
                xmlEntry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(xmlEntry);
                StreamUtils.Copy(stream, zipStream, new byte[4096]);
                zipStream.CloseEntry();

                //stream.Close();



                acum++;
                if (acum == 500)
                {
                    break;
                }

            }
            //------------------------------------------------------------------------------------ 
            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();
            output.Position = 0;

            if (!string.IsNullOrEmpty(sAcuse))
            {
                byte[] byteComprobante = output.GetBuffer();
                var = byteComprobante;
            }

            return var;
        }

        [WebMethod(Description = "Permite obtener un duplicado de los CFDi generados  anteriormente.")]
        public byte[] getCfdiFromUUID(String user, String password, String rfc, String[] uuid)
        {
            int pnId_Usuario = 0;
            int pnId_Estructura = 0;
            string sRetAutentication = string.Empty;
            byte[] var = { };

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
            sRetAutentication = fnAutentication(user, password, ref pnId_Usuario, ref datosUsuario);
            clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso1 - " + "Verifica el usuario WSDL SVC Timbrado_DP." + "0.0.0.0" + " USER: " + user + " PASSWORD: " + password.Substring(0, 3) + "*************");

            string[] est = sRetAutentication.Split('-');
            if (est.Length > 2)
            {
                sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
            }
            else
            {
                sRetAutentication = est[0].Trim();
            }
            if (sRetAutentication == "0")
            {
                if (pnId_Estructura == 0)
                    pnId_Estructura = Convert.ToInt32(fnRecuperaMatriz(pnId_Usuario));
            }
            else
            {
                try
                {
                    string sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sRetAutentication, "Response", "E", string.Empty);
                    var = Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
                    return var;
                }
                catch
                {
                    var = Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("95", "Seguridad"));
                    return var;
                }
            }

            string sUUID = uuid[0];
            string sComprobante = string.Empty;

            if (!string.IsNullOrEmpty(sUUID))
            {
                sComprobante = clsTimbradoFuncionalidad.fnBuscarComprobantePorUUID(sUUID);
            }

            if (!string.IsNullOrEmpty(sComprobante))
            {
                byte[] byteComprobante = Encoding.UTF8.GetBytes(sComprobante);
                var = byteComprobante;
            }

            return var;
        }

        [WebMethod(Description = "Permite comprobar si un comprobante ha sido timbrado en nuestro  sistema.")]
        public string getUUID(String user, String password, byte[] file)
        {
            string sCadenaOriginal = string.Empty;
            string sUUID = string.Empty;
            string sRetAutentication = string.Empty;
            string sComprobanteRecibido = Encoding.UTF8.GetString(file);
            int pnId_Usuario = 0;
            int pnId_Estructura = 0;

            XmlDocument xmlDocRecibido = new XmlDocument();
            xmlDocRecibido.LoadXml(sComprobanteRecibido);

            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertificatePolicy);
            sRetAutentication = fnAutentication(user, password, ref pnId_Usuario, ref datosUsuario);
            clsPistasAuditoriaTest.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso1 - " + "Verifica el usuario WSDL SVC Timbrado_DP." + "0.0.0.0" + " USER: " + user + " PASSWORD: " + password.Substring(0, 3) + "*************");

            string[] est = sRetAutentication.Split('-');
            if (est.Length > 2)
            {
                sRetAutentication = est[0].Trim() + " - " + est[1].Trim();
            }
            else
            {
                sRetAutentication = est[0].Trim();
            }
            if (sRetAutentication == "0")
            {
                if (pnId_Estructura == 0)
                    pnId_Estructura = Convert.ToInt32(fnRecuperaMatriz(pnId_Usuario));
            }
            else
            {
                try
                {
                    string sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad"));
                    clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, sRetAutentication, "Response", "E", string.Empty);
                    sUUID = clsComun.fnRecuperaErrorSAT(sRetAutentication, "Seguridad");
                }
                catch
                {
                    sUUID = clsComun.fnRecuperaErrorSAT("95", "Seguridad"); ;
                }
            }

            //Generar la cadena del emisor con los conceptos del XML
            try
            {
                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                // Load the type of the class.
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOri.V32));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(xmlDocRecibido.CreateNavigator(), args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                sCadenaOriginal = srDll.ReadToEnd();
            }
            catch (Exception)
            {
                return clsComun.fnRecuperaErrorSAT("303", "Timbrado y Cancelación");
            }

            string HASHEmisor = clsEnvioSAT.GetHASH(sCadenaOriginal).ToUpper();

            string sComprobanteTimbrado = clsTimbradoFuncionalidad.fnBuscarHashCompXML(datosUsuario.id_usuario, HASHEmisor, "Emisor");

            if (sComprobanteTimbrado != "0")
            {
                XmlDocument xDocTimbrado = new XmlDocument();
                xDocTimbrado.LoadXml(sComprobanteTimbrado);
                XmlNamespaceManager nsmComprobanteDP = new XmlNamespaceManager(xDocTimbrado.NameTable);
                nsmComprobanteDP.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobanteDP.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                try { sUUID = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteDP).Value; }
                catch { }

                return sUUID;
            }

            return sUUID;
        }

        ///// <summary>
        ///// Validar datos del usario.
        ///// </summary>
        ///// <param name="sNombre"></param>
        ///// <param name="sContraseña"></param>
        ///// <returns></returns>
        private string fnAutentication(string sNombre, string sContraseña, ref int nId_usuario, ref clsInicioSesionUsuario datosUsuario)
        {
            string sVarNombre = sNombre.Trim();
            string sVarContraseña = sContraseña.Trim();
            clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
            datosUsuario = new clsInicioSesionUsuario();
            DataTable tabla = new DataTable();
            int nId_contribuyente;
            //int nId_usuario;
            char cEstatus;
            string sEmail;
            string sRetorno = string.Empty;

            if (string.IsNullOrEmpty(sVarNombre) || string.IsNullOrEmpty(sVarContraseña))
            {
                return clsComun.fnRecuperaErrorSAT("90", "Seguridad");
            }

            //LLena datos de usuario.
            tabla = busquedaUsuario.buscarUsuario(sVarNombre);

            if (tabla.Rows.Count > 0)
            {

                //Revisa contraseña
                if (sVarContraseña == Utilerias.Encriptacion.Classica.Desencriptar(tabla.Rows[0]["password"].ToString()))
                {
                    //Guardar datos en clase Usuario
                    nId_contribuyente = Convert.ToInt32(tabla.Rows[0]["id_contribuyente"]);
                    nId_usuario = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
                    cEstatus = Convert.ToChar(tabla.Rows[0]["estatus"]);
                    sEmail = Convert.ToString(tabla.Rows[0]["email"]);
                    datosUsuario.id_contribuyente = nId_contribuyente;
                    datosUsuario.id_usuario = nId_usuario;
                    datosUsuario.estatus = cEstatus;
                    datosUsuario.email = sEmail;

                    //Revisar el estatus del usuario.
                    switch (cEstatus)
                    {
                        case 'P':
                            sRetorno = clsComun.fnRecuperaErrorSAT("91", "Seguridad");
                            break;
                        case 'A':
                            sRetorno = clsComun.fnRecuperaErrorSAT("0", "Todos"); ;
                            break;
                        case 'B':
                            sRetorno = clsComun.fnRecuperaErrorSAT("92", "Seguridad");
                            break;
                        case 'E':
                            sRetorno = clsComun.fnRecuperaErrorSAT("93", "Seguridad");
                            break;
                        case 'C':
                            sRetorno = clsComun.fnRecuperaErrorSAT("94", "Seguridad");
                            break;
                    }
                }
                else
                {
                    sRetorno = clsComun.fnRecuperaErrorSAT("96", "Seguridad");
                }
            }
            else
            {
                sRetorno = clsComun.fnRecuperaErrorSAT("95", "Seguridad");
            }

            return sRetorno;
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private int fnActualizaCreditos(DataTable dtCreditos)
        {
            DataTable tlbCreditos = new DataTable();
            int idCredito = 0;
            int idEstructura = 0;
            double Creditos = 0;
            int nRetorno = 0;

            tlbCreditos = dtCreditos;

            if (tlbCreditos.Rows.Count > 0)
            {
                idCredito = Convert.ToInt32(tlbCreditos.Rows[0]["id_creditos"]);
                idEstructura = Convert.ToInt32(tlbCreditos.Rows[0]["id_estructura"]);
                Creditos = Convert.ToDouble(tlbCreditos.Rows[0]["creditos"]);
                nRetorno = clsTimbradoFuncionalidad.fnActualizarCreditos(idCredito, idEstructura, Creditos, "C");
                clsTimbradoFuncionalidad.fnActualizarCreditosHistorico(idCredito, idEstructura, Creditos);
            }

            return nRetorno;
        }

        /// <summary>
        /// Recupera el Id de la matriz del usuario.
        /// </summary>
        /// <param name="pnId_Usuario"></param>
        /// <returns></returns>
        private string fnRecuperaMatriz(int pnId_Usuario)
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
                            //return "999";
                            return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
                        else
                            return retVal;
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante", pnId_Usuario);
                //return "999"; //Error durante el registro del comprobante
                return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
            }
        }

        public string fnValidate(string psXmlDocument, string psNombreEsquema)
        {
            XmlDocument document = new XmlDocument();

            string retorno = string.Empty;

            try
            {
                document.LoadXml(psXmlDocument);

                XPathNavigator navigator = document.CreateNavigator();
                DataTable tblComplementos = new DataTable();

                //tblComplementos = clsComun.fnObtenerXSDComplementos();
                tblComplementos = (DataTable)Application["complementos"];
                XmlTextReader tr;
                int count = 1;
                foreach (DataRow row in tblComplementos.Rows)
                {
                    tr = new XmlTextReader(new System.IO.StringReader(row["Valor"].ToString()));
                    document.Schemas.Add(row["esquema"].ToString(), tr);
                    count++;
                }

                ValidationEventHandler validation = new ValidationEventHandler(SchemaValidationHandler);

                document.Validate(validation);

                if (xsd_error_code != string.Empty)
                {
                    retorno = xsd_error_code;
                }

            }
            catch (Exception)
            {
                return "Revise el esquema del XML y reintente de nuevo.";
            }


            return retorno;
        }

        public void SchemaValidationHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:

                    if (!e.Message.Contains("TimbreFiscalDigital"))
                    {
                        xsd_error_code = xsd_error_code + e.Message;
                    }

                    break;
            }
        }

        private clsValCertificado fnRecuperarCertificado(string psComprobante)
        {
            //recuperamos el certificado del comprobante
            XmlDocument xDocTimbrado = new XmlDocument();
            xDocTimbrado.LoadXml(psComprobante);

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            string sCertificadoBase64 = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).Value;
            if (string.IsNullOrEmpty(sCertificadoBase64))
            {
                errorCode = 570; //No se pudó recuperar el certificado del comprobante
                return null;
            }

            return new clsValCertificado(Convert.FromBase64String(sCertificadoBase64));
        }

        private clsValCertificadoTest fnRecuperarCertificadoTest(string psComprobante)
        {
            //recuperamos el certificado del comprobante
            XmlDocument xDocTimbrado = new XmlDocument();
            xDocTimbrado.LoadXml(psComprobante);

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            string sCertificadoBase64 = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).Value;
            if (string.IsNullOrEmpty(sCertificadoBase64))
            {
                errorCode = 570; //No se pudó recuperar el certificado del comprobante
                return null;
            }

            return new clsValCertificadoTest(Convert.FromBase64String(sCertificadoBase64));
        }

        private bool fnVerificarCaducidadCertificado(string sNoCertificado, string estatusR, string estatusC, ref clsValCertificado vValidadorCertificado)
        {
            string rfcCaduco = vValidadorCertificado.RevisaCaducidadCertificado(sNoCertificado, estatusR, estatusC);

            if (!string.IsNullOrEmpty(rfcCaduco))
                return false;

            return true;
        }

        private bool fnRecuperaFechaLCO(string sNoCertificado, string estatus, ref clsValCertificado vValidadorCertificado)
        {
            DataTable fechas = vValidadorCertificado.RevisaExistenciaCertificadoFechas(sNoCertificado, estatus);

            if (Convert.ToDateTime(fechas.Rows[0]["fecha_inicio"].ToString()).CompareTo(DateTime.Today) > 0
               || Convert.ToDateTime(fechas.Rows[0]["fecha_final"].ToString()).CompareTo(DateTime.Today) < 0)
                return false;

            return true;
        }

        public bool fnFirmaHSMTerciario(wslServicioPAC gServicio, ref XmlDocument docNodoTimbre,
                                                       clsValCertificado vValidadorCertificado, ref string sCadenaOriginal, string seccion, ref string sError)
        {

            bool bvalidacion = false;
            byte[] bLlave;
            byte[] bCertificado;
            string sPassword;
            string selloPAC;
            //string sCadenaOriginal;
            string noCertificadoPAC;

            //HSM3
            try
            {

                bLlave = gServicio.ObtenerLlavePAC();
                bCertificado = gServicio.ObtenerCertificado();
                sPassword = gServicio.ObtenerPassword();

                gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                gCertificado = new clsValCertificado(bCertificado);
                noCertificadoPAC = gCertificado.ObtenerNoCertificado();
                gNodoTimbre.noCertificadoSAT = noCertificadoPAC;

                //Generamos el primer XML necesario para generar la cadena original
                docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                //Generamos la cadena original
                XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();
                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                // Load the type of the class.
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(Timbrado.V3.TFDXSLT));

                ms = new MemoryStream();
                args = new XsltArgumentList();

                xslt.Transform(navNodoTimbre, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sCadenaOriginal = srDll.ReadToEnd();


                selloPAC = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                gNodoTimbre.selloSAT = selloPAC;

                if (vValidadorCertificado.fnVerificarSelloPAC(sCadenaOriginal, selloPAC, noCertificadoPAC, bCertificado))
                {
                    bvalidacion = true;
                }
                else
                {
                    sError = "CadenaOriginal: " + sCadenaOriginal + "SelloPAC: " + selloPAC + "Certificado: " + noCertificadoPAC;
                }
            }
            catch (Exception ex)
            {
                noCertificadoPAC = string.Empty;
                bvalidacion = false;
                sError = ex.Message;
            }

            return bvalidacion;
            //return false;
        }

        ////METODOS PRUEBA
        private bool fnRecuperaFechaLCOTest(string sNoCertificado, string estatus, ref clsValCertificadoTest vValidadorCertificado)
        {
            DataTable fechas = vValidadorCertificado.RevisaExistenciaCertificadoFechas(sNoCertificado, estatus);

            if (Convert.ToDateTime(fechas.Rows[0]["fecha_inicio"].ToString()).CompareTo(DateTime.Today) > 0
               || Convert.ToDateTime(fechas.Rows[0]["fecha_final"].ToString()).CompareTo(DateTime.Today) < 0)
                return false;

            return true;
        }

        private bool fnVerificarCaducidadCertificadoTest(string sNoCertificado, string estatusR, string estatusC, ref clsValCertificadoTest vValidadorCertificado)
        {
            string rfcCaduco = vValidadorCertificado.RevisaCaducidadCertificado(sNoCertificado, estatusR, estatusC);

            if (!string.IsNullOrEmpty(rfcCaduco))
                return false;

            return true;
        }

        public bool fnFirmaHSMTerciarioTest(wslServicioPAC gServicio, ref XmlDocument docNodoTimbre,
                                                       clsValCertificadoTest vValidadorCertificado, ref string sCadenaOriginal, string seccion, ref string sError)
        {

            bool bvalidacion = false;
            byte[] bLlave;
            byte[] bCertificado;
            string sPassword;
            string selloPAC;
            //string sCadenaOriginal;
            string noCertificadoPAC;

            //HSM3
            try
            {

                bLlave = gServicio.ObtenerLlavePAC();
                bCertificado = gServicio.ObtenerCertificado();
                sPassword = gServicio.ObtenerPassword();

                gTimbrado = new clsOperacionTimbradoSellado(bLlave, sPassword);
                gCertificado = new clsValCertificado(bCertificado);
                noCertificadoPAC = gCertificado.ObtenerNoCertificado();
                gNodoTimbre.noCertificadoSAT = noCertificadoPAC;

                //Generamos el primer XML necesario para generar la cadena original
                docNodoTimbre = gTimbrado.fnGenerarXML(gNodoTimbre);

                //Generamos la cadena original
                XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();
                XslCompiledTransform xslt;
                XsltArgumentList args;
                MemoryStream ms;
                StreamReader srDll;

                // Load the type of the class.
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(Timbrado.V3.TFDXSLT));

                ms = new MemoryStream();
                args = new XsltArgumentList();

                xslt.Transform(navNodoTimbre, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sCadenaOriginal = srDll.ReadToEnd();


                selloPAC = gTimbrado.fnGenerarSello(sCadenaOriginal, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);
                gNodoTimbre.selloSAT = selloPAC;

                if (vValidadorCertificado.fnVerificarSelloPAC(sCadenaOriginal, selloPAC, noCertificadoPAC))
                {
                    bvalidacion = true;
                }
                else
                {
                    sError = "CadenaOriginal: " + sCadenaOriginal + "SelloPAC: " + selloPAC + "Certificado: " + noCertificadoPAC;
                }
            }
            catch (Exception ex)
            {
                noCertificadoPAC = string.Empty;
                bvalidacion = false;
                sError = ex.Message;
            }

            return bvalidacion;
            //return false;
        }
    }
}

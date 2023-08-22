using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HSM;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Security;

/// <summary>
/// Clase encargada de comunicar el HSM con la aplicacion.
/// </summary>
public class clsHSMComunicacion
{
    private xmCryptoService     hsm;
    private authTokenType       autoToken;
    private authTokenType       newAutoToken;
    private clsGeneraEMAIL      mail;

    public clsHSMComunicacion()
	{

        ////Utilizar para HSM principal
        if (fnBuscarServidor(clsComun.ObtenerParamentro("HSMPrincipal"))
                &&
                (fnRevisarActividadServidor("https://" + clsComun.ObtenerParamentro("HSMPrincipal") + ":8443/xmc/services/xmCryptoService",
                //clsComun.ObtenerParamentro("HSMUserID"), Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("HSMPassword")),
                clsComun.ObtenerParamentro("HSMUserID"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("HSMPassword")),
                clsComun.ObtenerParamentro("HSMKeyAlias"))
                )
            )
        {
            hsm = new xmCryptoService();
            hsm.Url = "https://" + clsComun.ObtenerParamentro("HSMPrincipal") + ":8443/xmc/services/xmCryptoService";
        }
        else
        {

            mail = new clsGeneraEMAIL();
            mail.EnviarCorreo(System.Configuration.ConfigurationSettings.AppSettings["emailAll"], "Hay problemas con el HSMPrincipal", "Error al Generar el Sello del SAT");

            //Utilizar para HSM secundario
            if (fnBuscarServidor(clsComun.ObtenerParamentro("HSMSecundario"))
                &&
                fnRevisarActividadServidor("https://" + clsComun.ObtenerParamentro("HSMSecundario") + ":8443/xmc/services/xmCryptoService",
                    //clsComun.ObtenerParamentro("HSMUserID"), Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("HSMPassword")),
                    clsComun.ObtenerParamentro("HSMUserID"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("HSMPassword")),
                    clsComun.ObtenerParamentro("HSMKeyAlias")))
            {

                hsm = new xmCryptoService();
                hsm.Url = "https://" + clsComun.ObtenerParamentro("HSMSecundario") + ":8443/xmc/services/xmCryptoService";

            }
            else
            {
                mail = new clsGeneraEMAIL();
                mail.EnviarCorreo(System.Configuration.ConfigurationSettings.AppSettings["emailAll"], "Hay problemas con el HSMSecundario", "Error al Generar el Sello del SAT");
            }
        }

		autoToken       = new authTokenType();
        newAutoToken    = new authTokenType();
	}


    /// <summary>
    /// Verifica que el usuario sea valido y retorna el token
    /// </summary>
    /// <returns></returns>
    public authTokenType fnHSMLogin()
    {
        login loginHsm = new login();
        loginResponse loginResponse;

        try
        {
            //Usuario que va a firmar
            loginHsm.authModel = authModelType.PROP;
            loginHsm.UserID = clsComun.ObtenerParamentro("HSMUserID");
            //loginHsm.password = Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("HSMPassword"));
            loginHsm.password = PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("HSMPassword"));

            //Resultado del login en cadena
            loginResponse = hsm.login(loginHsm);

            if (loginResponse.Result.ResultMessage.Value == clsComun.ObtenerParamentro("HSM_Respuesta_Login"))
            {
                autoToken = loginResponse.AuthToken;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }


        return autoToken;
    }

    /// <summary>
    /// Termina la sesion del usuario del HSM
    /// </summary>
    /// <param name="autoToken"></param>
    public void fnHSMLogOut()
    {

        logout logoutHsm = new logout();
        logoutResponse logoutResponse;

        try
        {
            logoutHsm.AuthToken = autoToken;
            logoutResponse = hsm.logout(logoutHsm);
            newAutoToken = null;
            autoToken = null;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Firma la cadena original que se le manda y retorna el sello.
    /// </summary>
    /// <param name="Token"></param>
    /// <param name="dataToSign"></param>
    /// <returns></returns>
    public string fnFirmar(string dataToSign, authTokenType Token)
    {
        sign sign = new sign();
        signResponse signResponse;
        string signature = string.Empty;
        byte[] bytesEncode;

        try
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            bytesEncode = encoding.GetBytes(dataToSign);
            dataToSign = System.Convert.ToBase64String(bytesEncode); 

            //Firma el valor que le pases
            sign.SigningKeyAlias = clsComun.ObtenerParamentro("HSMKeyAlias");
            sign.SignatureModeSpecified = true;
            sign.SignatureMode = SignatureModeType.SHA1withRSA;
            sign.DataToSign = dataToSign;
            sign.AuthToken = autoToken;

            signResponse = hsm.sign(sign);

            if (signResponse.Result.ResultMajor == "urn:oasis:names:tc:dss:resultmajor:Success")
            {
                signature = signResponse.Signature;
                newAutoToken = signResponse.AuthToken;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return signature;
    }


    /// <summary>
    /// Obtiene el numero del certificado del PAC
    /// </summary>
    /// <param name="Token"></param>
    /// <returns></returns>
    public string fnObtenerNumeroCertificado(authTokenType Token)
    {

        getObjectInfo objInfo = new getObjectInfo();
        getObjectInfoResponse objInfoResponse;
        X509DataType x509Data = new X509DataType();
        object[] items;
        byte[] x509Items;
        byte[] bCertificadoInvertido;
        X509Certificate2 certificado;
        string sNumeroCertificado = string.Empty;

        try
        {
            //Obtiene la informacion del objeto.
            objInfo.AuthToken = Token;
            objInfo.KeySpace = clsComun.ObtenerParamentro("HSMKeySpace");
            objInfo.ObjectAlias = clsComun.ObtenerParamentro("HSMObjectAlias");
            objInfoResponse = hsm.getObjectInfo(objInfo);
            newAutoToken = objInfoResponse.AuthToken;

            if (objInfoResponse.Result.ResultMajor == "urn:oasis:names:tc:dss:resultmajor:Success")
            {
                items = objInfoResponse.KeyInfo.Items;
                x509Data = (X509DataType)items[0];
                x509Items = (byte[])x509Data.Item;
                certificado = new X509Certificate2(x509Items);

                bCertificadoInvertido = certificado.GetSerialNumber().Reverse().ToArray();
                sNumeroCertificado = Encoding.Default.GetString(bCertificadoInvertido);
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return sNumeroCertificado;
    }

    /// <summary>
    /// Busca la direccion del servidor para esperar respuesta.
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    private bool fnBuscarServidor(string ip)
    {

        Ping pingServerPrincipal = new Ping();
        PingReply pingServerPrincipalReplay;
        bool retorno = false;
        try
        {
            pingServerPrincipalReplay = pingServerPrincipal.Send(ip);
            if (pingServerPrincipalReplay.Status.ToString() != "DestinationHostUnreachable")
            {
                //clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, "WSD HSM USE - " + ip);
                retorno = true;
            }
        }
        catch (Exception)
        {
            retorno = false;
        }

        return retorno;
    }

    /// <summary>
    /// Verifica el funcionamiento del HSM
    /// </summary>
    /// <param name="urlHSM"></param>
    /// <returns></returns>
    private bool fnRevisarActividadServidor(string urlHSM, string UserId, string Password, string SigningKeyAlias)
    {
        ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsHSMComunicacion.AcceptAllCertificatePolicy);

        //Generacion de Objetos
        HSM.xmCryptoService hsm = new HSM.xmCryptoService();
        HSM.authTokenType AutoToken = new HSM.authTokenType();
        HSM.authTokenType newAutoToken = new HSM.authTokenType();

        HSM.login loginHsm = new HSM.login();
        HSM.loginResponse loginResponse;

        HSM.logout logoutHsm = new HSM.logout();
        HSM.logoutResponse logoutResponse;

        HSM.sign sign = new HSM.sign();
        HSM.signResponse signResponse;

        bool retorno = false;

        try
        {
            hsm.Url = urlHSM;

            //Usuario que va a firmar
            loginHsm.authModel = HSM.authModelType.PROP;
            loginHsm.UserID = UserId;
            loginHsm.password = Password;


            //Resultado del login en cadena
            loginResponse = hsm.login(loginHsm);

            if (loginResponse.Result.ResultMessage.Value == clsComun.ObtenerParamentro("HSM_Respuesta_Login"))
            {

                AutoToken = loginResponse.AuthToken;


                //Firma el valor que le pases
                sign.SigningKeyAlias = SigningKeyAlias;
                sign.SignatureModeSpecified = true;
                sign.SignatureMode = HSM.SignatureModeType.SHA1withRSA;
                sign.DataToSign = "Firma";
                sign.AuthToken = AutoToken;



                signResponse = hsm.sign(sign);
                if (signResponse.Result.ResultMajor == "urn:oasis:names:tc:dss:resultmajor:Success")
                {
                    retorno = true;
                    string signature = signResponse.Signature;
                    newAutoToken = signResponse.AuthToken;

                    logoutHsm.AuthToken = newAutoToken;
                    logoutResponse = hsm.logout(logoutHsm);
                    AutoToken = null;
                }
            }
        }
        catch (Exception)
        {
            retorno = false;
        }


        return retorno;
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}
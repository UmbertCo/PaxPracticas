using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PAXRegeneracionBateria.HSM;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Security;
using PAXRegeneracionBateria.HSM;

/// <summary>
/// Clase encargada de comunicar el HSM con la aplicacion.
/// </summary>
public class clsHSMComunicacion
{
    private xmCryptoService hsm;
    private authTokenType autoToken;
    private authTokenType newAutoToken;

    public clsHSMComunicacion()
    {

    }

    /// <summary>
    /// Verifica que el usuario sea valido y retorna el token
    /// </summary>
    /// <returns></returns>
    public authTokenType fnHSMLogin(xmCryptoService hsm)
    {
        login loginHsm = new login();
        loginResponse loginResponse;

        try
        {
            //Usuario que va a firmar
            loginHsm.authModel = authModelType.PROP;
            loginHsm.UserID = "PAXAdmin";
            loginHsm.password = "P4ssw0rd";

            //Resultado del login en cadena
            loginResponse = hsm.login(loginHsm);

            if (loginResponse.Result.ResultMessage.Value == "Luna XML xmCryptoService version: 1.1.3")
            {
                autoToken = loginResponse.AuthToken;
            }
        }
        catch (Exception)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }


        return autoToken;
    }

    /// <summary>
    /// Termina la sesion del usuario del HSM
    /// </summary>
    /// <param name="autoToken"></param>
    public void fnHSMLogOut(xmCryptoService hsm)
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
        catch (Exception)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Obtiene el numero del certificado del PAC
    /// </summary>
    /// <param name="Token"></param>
    /// <returns></returns>
    public string fnObtenerNumeroCertificado(authTokenType Token, xmCryptoService hsm)
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
            objInfo.KeySpace = "PAXAdmin";
            objInfo.ObjectAlias = "certificado";
            objInfo.ObjectAlias = "pacprivada";
            

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
        catch (Exception)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return sNumeroCertificado;
    }

    /// <summary>
    /// Genera la firma del HSM
    /// </summary>
    /// <param name="dataToSign"></param>
    /// <param name="hsm"></param>
    /// <returns></returns>
    public string fnRecuperaFirma(string dataToSign, xmCryptoService hsm)
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
            sign.SigningKeyAlias = "pacprivada";
            sign.SignatureModeSpecified = true;
            sign.SignatureMode = SignatureModeType.SHA1withRSA;
            sign.DataToSign = dataToSign;
            sign.AuthToken = newAutoToken;

            signResponse = hsm.sign(sign);

            if (signResponse.Result.ResultMajor == "urn:oasis:names:tc:dss:resultmajor:Success")
            {
                signature = signResponse.Signature;
                //newAutoToken = signResponse.AuthToken;
            }
        }
        catch (Exception)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return signature;
    }

    /// <summary>
    /// Busca la direccion del servidor para esperar respuesta.
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public bool fnBuscarServidor(string ip)
    {

        Ping pingServerPrincipal = new Ping();
        PingReply pingServerPrincipalReplay;

        bool retorno = false;

        int enviados = 0;
        int retornoPing1 = 0;
        int retornoPing2 = 0;
        int retornoPing3 = 0;

        try
        {
            pingServerPrincipalReplay = pingServerPrincipal.Send(ip);
            if (pingServerPrincipalReplay.Address != null)
            {
                if (pingServerPrincipalReplay.Address.ToString() != string.Empty)
                {
                    retornoPing1 = 1;
                }
            }

            pingServerPrincipalReplay = pingServerPrincipal.Send(ip);
            if (pingServerPrincipalReplay.Address != null)
            {
                if (pingServerPrincipalReplay.Address.ToString() != string.Empty)
                {
                    retornoPing2 = 1;
                }
            }

            pingServerPrincipalReplay = pingServerPrincipal.Send(ip);
            if (pingServerPrincipalReplay.Address != null)
            {
                if (pingServerPrincipalReplay.Address.ToString() != string.Empty)
                {
                    retornoPing3 = 1;
                }
            }

            enviados = retornoPing1 + retornoPing2 + retornoPing3;

            if (enviados >= 2)
            {
                retorno = true;
            }

        }
        catch (Exception)
        {
            retorno = false;
        }

        //Ping pingServerPrincipal = new Ping();
        //PingReply pingServerPrincipalReplay;
        //bool retorno = false;
        //try
        //{
        //    pingServerPrincipalReplay = pingServerPrincipal.Send(ip);
        //    if (pingServerPrincipalReplay.Status.ToString() != "DestinationHostUnreachable")
        //    {
        //        //clsPistasAuditoria.fnGenerarPistasAuditoria(null, DateTime.Now, "WSD HSM USE - " + ip);
        //        retorno = true;
        //    }
        //}
        //catch (Exception)
        //{
        //    retorno = false;
        //}

        return retorno;
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}

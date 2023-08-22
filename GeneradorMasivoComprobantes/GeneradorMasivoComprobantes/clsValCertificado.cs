using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Xml;
using System.Text;
using Utilerias.Encriptacion;
using Chilkat;
using System.Data;
using System.Text.RegularExpressions;

/// <summary>
/// Clase encargada de realizar las varias validaciones que SAT estipula sobre la FIEL y el certificado público
/// </summary>
public class clsValCertificado
{
    public string sRFCCertificado = string.Empty;

    public string sRazonSocialCertificado = string.Empty;

    private byte[] gbCertificado;
    /// <summary>
    /// Retorna o establece el certificado como arreglo de bytes
    /// </summary>
    public byte[] CertificadoBytes
    {
        get { return gbCertificado; }
        set { gbCertificado = value; }
    }

    private clsOperacionTimbradoSellado gLlave;
    /// <summary>
    /// Retorna o establece un objeto clsOperacionTimbradoSellado para el manejo de la llave privada
    /// </summary>
    public clsOperacionTimbradoSellado LlavePrivada
    {
        get { return gLlave; }
        set { gLlave = value; }
    }

    private X509Certificate2 certificado;
    /// <summary>
    /// Retorna el certificado como un objeto de .NET
    /// </summary>
    public X509Certificate2 Certificado
    {
        get
        {
            return certificado;
        }
    }

    /// <summary>
    /// Crea una nueva instancia de .NET tomando los datos del arreglo de bytes enviado
    /// </summary>
    /// <param name="pbCertificado">Arreglo de bytes que representan el archivo del certificado</param>
    public clsValCertificado(byte[] pbCertificado)
    {
        try
        {
            certificado = new X509Certificate2(pbCertificado);
            gbCertificado = pbCertificado;
        }
        catch
        {
            try
            {
                certificado = new X509Certificate2(fnDesencriptarCertificado(pbCertificado));
                gbCertificado = fnDesencriptarCertificado(pbCertificado);
            }
            catch (Exception ex)
            {
                throw new CryptographicException("El certificado esta bloqueado");
            }
        }

        if (certificado.Verify())
            throw new CryptographicException("El certificado no pasó la verificación");
    }

    /// <summary>
    /// Agrega una llave privada al certificado a partir del arreglo de bytes del archivo.
    /// </summary>
    /// <param name="pbLlave">Arreglo de bytes del archivo key</param>
    public void fnAgregarLlavePrivada(byte[] pbLlave)
    {
        gLlave = new clsOperacionTimbradoSellado(pbLlave);
    }

    /// <summary>
    /// Realiza las diversas validaciones que SAT especifica para los certificados
    /// </summary>
    /// <param name="psRFC">RFC al que se asociará el certificado</param>
    /// <returns>Retorna cadena vacia en éxito, de o contrario regresa el mensaje de error</returns>
    public string ValidarCertificado(string psRFC, string psRazonSocial)
    {

        string sResultado = string.Empty;

        if ((psRFC != ObtenerRfcEmisorDeCertificado()))
            throw new System.ArgumentException("El RFC no corresponde al Certificado.");
            //sResultado = "El RFC no corresponde al Certificado.";


        if ((psRazonSocial != ObtenerRazonSocialDeCertificado()))
            sResultado = "La razón socal no corresponde al Certificado.";

        string paridad = VerificarParidad();
        if (!string.IsNullOrEmpty(paridad))
            throw new System.ArgumentException(paridad);

        if (!string.IsNullOrEmpty(sResultado))
            return sResultado;

        return string.Empty;
    }

    /// <summary>
    /// Encargado de validar si la expreson es verdadera
    /// </summary>
    /// <param name="sValor">valor a evaluar</param>
    /// <param name="expresion">expresion regular</param>
    /// <returns>retorna si es verdadero</returns>
    public static bool fnValidaExpresion(string sValor, string expresion)
    {
        bool bRetorno = false;

        if (Regex.IsMatch(sValor, expresion))
        {
            bRetorno = true;
        }

        return bRetorno;
    }


    /// <summary>
    /// Obtiene el RFC contenido dentro del certificado
    /// </summary>
    /// <returns>Retorna una cadena ocn el RFC</returns>
    private string ObtenerRfcEmisorDeCertificado()
    {
        try
        {
            //return new System.Text.RegularExpressions.Regex(".*OID.2.5.4.45=(?'rfc'.*)/").Match(certificado.SubjectName.Decode(X500DistinguishedNameFlags.UseNewLines)).Groups["rfc"].ToString().Trim();
            string[] cert = certificado.SubjectName.Decode(X500DistinguishedNameFlags.UseNewLines).Split('\n');

            if (fnEsCSD())
            {

            string[] rfc = cert[3].Split('=');

            string sRfc = string.Empty;

            if (rfc[1].Contains("/"))
            {
                string[] rfcFinal = rfc[1].Split('/');


                if (!fnValidaExpresion(rfcFinal[0].Trim(), @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                {
                    sRfc = rfcFinal[0].Substring(1, 14).Trim();
                }
                else
                {
                    sRfc = rfcFinal[0].Trim();
                }

                sRFCCertificado = sRfc;

                return sRfc;
            }
            else
            {
                if (!fnValidaExpresion(rfc[1].Trim(), @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                {
                    sRfc = rfc[1].Substring(1, 14).Trim();
                }
                else
                {
                    sRfc = rfc[1].Trim();
                }

                sRFCCertificado = sRfc;
                return rfc[1].Trim();
            }
            }
        }
        catch (CryptographicException ex)
        {
            //MessageBox.Show(ex.ToString());
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.ToString());
        }

        return string.Empty;
    }

    public bool fnEsFiel()
    {
        try
        {
            return certificado.Extensions[1].Format(false).Contains("(d8)");
        }
        catch (Exception ex)
        {
            //
        }
        return false;
    }

    /// <summary>
    /// Obtiene el RFC contenido dentro del certificado
    /// </summary>
    /// <returns>Retorna una cadena ocn el RFC</returns>
    private string ObtenerRazonSocialDeCertificado()
    {
        try
        {
            //return new System.Text.RegularExpressions.Regex(".*OID.2.5.4.45=(?'rfc'.*)/").Match(certificado.SubjectName.Decode(X500DistinguishedNameFlags.UseNewLines)).Groups["rfc"].ToString().Trim();
            string[] cert = certificado.SubjectName.Decode(X500DistinguishedNameFlags.UseNewLines).Split('\n');

            string[] RazonSocial = cert[0].Split('=');

            string sRazonSocial = string.Empty;

            sRazonSocial = RazonSocial[1];
            sRazonSocial = sRazonSocial.Substring(0, sRazonSocial.Length - 1);

            sRazonSocialCertificado = sRazonSocial;
            return sRazonSocial;
        }
        catch (CryptographicException ex)
        {
            //MessageBox.Show(ex.ToString());
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.ToString());
        }

        return string.Empty;
    }

    public bool fnEsCSD()
    {
        try
        {
            return certificado.Extensions[1].Format(false).Contains("(c0)");
        }
        catch
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        return false;
    }


    /// <summary>
    /// Comprueba que el certificado corresponda a la llave privada proporcionada
    /// </summary>
    /// <returns>Un booleano indicando si el certificado corresponde o no a la llave privada</returns>
    private string VerificarParidad()
    {
        RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certificado.PublicKey.Key);
        string sCadenaPrueba = "Esta es una cadena de prueba";

        //Creamos un sello ficticio con la cadena de prueba
        string sSelloPrueba = LlavePrivada.fnGenerarSello(sCadenaPrueba, clsOperacionTimbradoSellado.AlgoritmoSellado.SHA1);

        //si falló la generación del sello retornamos el error correspondiente
        if (string.IsNullOrEmpty(sSelloPrueba))
            return "La contraseña proporcionada no es la correcta";

        try
        {
            //Verificamos que el certificado obtenga el mismo resultado que el del sello
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(sCadenaPrueba));
            bool exito = publica.VerifyHash(
                    hash,
                    "sha1",
                    Convert.FromBase64String(sSelloPrueba));

            if (exito)
                return string.Empty;
            else
                return "La contraseña proporcionada no es la correcta.";
        }
        catch
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return "El certificado no corresponde a la llave privada";
        }
    }


    /// <summary>
    /// Devuelve el arreglo de bytes del certificado original
    /// </summary>
    /// <returns></returns>
    private byte[] fnDesencriptarCertificado(byte[] pbCertificadoEncriptado)
    {
        return Utilerias.Encriptacion.DES3.Desencriptar(pbCertificadoEncriptado);
    }


}
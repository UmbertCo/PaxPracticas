﻿using System;
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

/// <summary>
/// Clase encargada de realizar las varias validaciones que SAT estipula sobre la FIEL y el certificado público
/// </summary>
public class clsValCertificado
{
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
	public  X509Certificate2 Certificado
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
	public string ValidarCertificado(string psRFC)
	{
        if (!fnEsCSD())
        {
            return Resources.resCorpusCFDIEs.valCSDCer;
        }
        if (!fnCSD308())
        {
            return Resources.resCorpusCFDIEs.varCSD308;
        }
        string sRFC = VerificarExistenciaCertificado();
		if (sRFC==string.Empty)
			return Resources.resCorpusCFDIEs.valEmisionCer;

        if ((psRFC != ObtenerRfcEmisorDeCertificado()) || (sRFC!=psRFC))
			return Resources.resCorpusCFDIEs.valRfcCer;

        string paridad = VerificarParidad();
        if (!string.IsNullOrEmpty(paridad))
            return paridad;

        if (!ComprobarFechas())
			return Resources.resCorpusCFDIEs.valFechaCer;


		return string.Empty;
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
            string[] rfc = cert[3].Split('=');
            if (rfc[1].Contains("/"))
            {
                string[] rfcFinal = rfc[1].Split('/');
                return rfcFinal[0].Trim();
            }
            else
            {
                return rfc[1].Trim();
            }
		}
		catch (CryptographicException ex)
		{
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
		}
		catch (Exception ex)
		{
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
		}

		return string.Empty;
	}

    public bool fnEsCSD()
    {
        try
        {
            return certificado.Extensions[1].Format(false).Contains("(c0)");
        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
        //}

        return false;
    }

    //Revisa que no sea apocrifo
    public bool fnCSD308()
    {
        try
        {

            bool ret = false;
            if (certificado.IssuerName.Name.Contains("A.C. del Servicio de Administración Tributaria"))
                ret = true;
            else
                ret = false;
            return ret;
        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        return false;
        //return true;
    }

    /// <summary>
    /// Obtiene el número de certificado asociado al archivo a partir del número de serie contenido en el certificado
    /// </summary>
    /// <returns></returns>
	public string ObtenerNoCertificado()
	{
		byte[] bCertificadoInvertido = certificado.GetSerialNumber().Reverse().ToArray();
		return Encoding.Default.GetString(bCertificadoInvertido);
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
            return Resources.resCorpusCFDIEs.varKeyPass;

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
                return Resources.resCorpusCFDIEs.valKeyCer;
        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return Resources.resCorpusCFDIEs.valKeyCer;
        }
    }


    /// <summary>
    /// Comprueba que el sello del comprobante refleje los datos de la caden original
    /// </summary>
    /// <param name="psCadenaOriginal">Cadena original del comprobante</param>
    /// <returns>Booleano indicando si la cadena original corresponde al sello</returns>
    public bool fnVerificarSello(string psCadenaOriginal, string psSello)
    {
        RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certificado.PublicKey.Key);

        try
        {
            //Verificamos que el certificado obtenga el mismo resultado que el del sello
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(psCadenaOriginal));
            bool exito = publica.VerifyHash(
                    hash,
                    "sha1",
                    Convert.FromBase64String(psSello));

            return exito;
        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return false;
        }
    }

    /// <summary>
    /// Comprueba que el sello del timbre del pac refleje los datos de la caden original
    /// </summary>
    /// <param name="psCadenaOriginal"></param>
    /// <param name="psSello"></param>
    /// <param name="noCertificado"></param>
    /// <returns></returns>
    public static bool fnVerificarSelloPAC(string psCadenaOriginal, string psSello, string noCertificado)
    {

        try
        {

            // The path to the certificate.
            string Certificate = "D:\\CertificadosPAC\\" + noCertificado + "\\" + noCertificado + ".cer";

            // Load the certificate into an X509Certificate object.
            X509Certificate2 cert = new X509Certificate2(Certificate);
            X509Certificate2 certificado;


            byte[] certData = cert.Export(X509ContentType.Cert);

            certificado = new X509Certificate2(certData);

            RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certificado.PublicKey.Key);


            //Verificamos que el certificado obtenga el mismo resultado que el del sello
            byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(psCadenaOriginal));
            bool exito = publica.VerifyHash(
                    hash,
                    "sha1",
                    Convert.FromBase64String(psSello));

            return exito;
        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return false;
        }
    }

    /// <summary>
    /// Revisa en BD la existencia del certificado.
    /// </summary>
    /// <returns></returns>
    public string VerificarExistenciaCertificado()
    {
        string sRetorno = string.Empty;
        string sNoCertificado = ObtenerNoCertificado();

        if (sNoCertificado.Length != 20)
        {
            //clsErrorLog.fnNuevaEntrada(new Exception("El número de certificado no cumple con la longitud adecuada"), clsErrorLog.TipoErroresLog.Datos);
        }

        try
        {
            sRetorno=RevisaExistenciaCertificado(sNoCertificado, 'A');
        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return sRetorno;
    }


    /// <summary>
    /// Revisa Existencia Certificado  Revisa existencia del certificado
    /// </summary>
    /// <param name="no_serie"></param>
    /// <param name="estado_cer"></param>
    /// <returns></returns>
    public string RevisaExistenciaCertificado(string no_serie, char estado_cer)
    {

        string sRetorno = string.Empty;

        try
        {

            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conControl");

            conexion.AgregarParametro("no_serie", no_serie);
            conexion.AgregarParametro("sEstado", estado_cer);

            sRetorno = Convert.ToString(conexion.TraerEscalar("usp_Ctp_RevisarExistenciaCSD", true));

        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }


        return sRetorno;
    }

    /// <summary>
    /// Revisa Existencia Certificado y sus fechas para la vigencia
    /// </summary>
    /// <param name="no_serie"></param>
    /// <param name="estado_cer"></param>
    /// <returns></returns>
    public DataTable RevisaExistenciaCertificadoFechas(string no_serie, string estado_cer)
    {

        string sRetorno = string.Empty;
        DataTable gdtAuxiliar = new DataTable();
        try
        {

            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conControl");

            conexion.AgregarParametro("no_serie", no_serie);
            conexion.AgregarParametro("sEstado", estado_cer);

            conexion.Query("usp_Ctp_RevisarExistenciaFechasCSD", true, ref gdtAuxiliar);

        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }


        return gdtAuxiliar;
    }

    /// <summary>
    /// Revisa caducidad del certificado en la LCO
    /// </summary>
    /// <param name="no_serie"></param>
    /// <param name="estado_cerR"></param>
    /// <param name="estado_cerC"></param>
    /// <returns></returns>
    public string RevisaCaducidadCertificado(string no_serie, string estado_cerR,string estado_cerC)
    {

        string sRetorno = string.Empty;
        try
        {

            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conControl");

            conexion.AgregarParametro("no_serie", no_serie);
            conexion.AgregarParametro("sEstadoR", estado_cerR);
            conexion.AgregarParametro("sEstadoC", estado_cerC);

            sRetorno = Convert.ToString(conexion.TraerEscalar("usp_Ctp_RevisarCaducidadCSD", true));

        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }


        return sRetorno;
    }



    /// <summary>
    /// Verifica que el certificado aún sea vigente
    /// </summary>
    /// <returns></returns>
	public bool ComprobarFechas()
	{

        if (certificado.NotBefore.CompareTo(DateTime.Today) > 0
            || certificado.NotAfter.CompareTo(DateTime.Today) < 0)
            return false;

		return true;
	}


    public bool fnFechaContraPeriodoValidez(DateTime pdFechaComprobante)
    {
        if (DateTime.Now.CompareTo(pdFechaComprobante) < 0 ||
            DateTime.Now.AddDays(-3).CompareTo(pdFechaComprobante) > 0)
            return false;

        //verificamos que la fecha del comprobante este dentro del periodo de validez del certificado
        if (certificado.NotBefore.CompareTo(pdFechaComprobante) > 0
            || certificado.NotAfter.CompareTo(pdFechaComprobante) < 0)
            return false;

        return true;
    }

    /// <summary>
    /// Devuelve el arreglo de bytes del certificado encriptados
    /// </summary>
    /// <returns></returns>
	public byte[] fnEncriptarCertificado()
	{
		return Utilerias.Encriptacion.DES3.Encriptar(gbCertificado);
	}

    /// <summary>
    /// Devuelve el arreglo de bytes del certificado original
    /// </summary>
    /// <returns></returns>
    private byte[] fnDesencriptarCertificado(byte[] pbCertificadoEncriptado)
    {
        return Utilerias.Encriptacion.DES3.Desencriptar(pbCertificadoEncriptado);
    }

    /// <summary>
    /// Devuelve el arreglo de bytes enviado como parametro de manera encriptada
    /// </summary>
    /// <param name="pbCertificado"></param>
    /// <returns></returns>
	public byte[] fnEncriptarLlave()
	{
        return gLlave.fnEncriptarLlave();
	}

    /// <summary>
    /// Devuelve el password proporcionado de manera encriptada
    /// </summary>
    /// <returns>Cadena encriptada</returns>
    public string fnEncriptarPassword()
    {
        return gLlave.fnEncriptarPassword();
    }

    /// <summary>
    /// Revisa en BD la existencia del certificado.
    /// </summary>
    /// <returns></returns>
    public string VerificarExistenciaCertificadoV2()
    {
        string sRetorno = string.Empty;
        string sNoCertificado = ObtenerNoCertificado();

        if (sNoCertificado.Length != 20)
        {
            //clsErrorLog.fnNuevaEntrada(new Exception("El número de certificado no cumple con la longitud adecuada"), clsErrorLog.TipoErroresLog.Datos);
        }

        try
        {
            sRetorno = RevisaExistenciaCertificadoV2(sNoCertificado, 'A');
        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return sRetorno;
    }
    /// <summary>
    /// Revisa Existencia Certificado  Revisa existencia del certificado
    /// </summary>
    /// <param name="no_serie"></param>
    /// <param name="estado_cer"></param>
    /// <returns></returns>
    public string RevisaExistenciaCertificadoV2(string no_serie, char estado_cer)
    {

        string sRetorno = string.Empty;

        try
        {

            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conControl");

            conexion.AgregarParametro("no_serie", no_serie);
            conexion.AgregarParametro("sEstado", estado_cer);

            sRetorno = Convert.ToString(conexion.TraerEscalar("usp_Ctp_RevisarExistenciaCSDV2", true));

        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }


        return sRetorno;
    }
}

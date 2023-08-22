using Chilkat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Web;
//using Utilerias.Encriptacion;

/// <summary>
/// Clase encargada de realizar las varias validaciones que SAT estipula sobre la FIEL y el certificado público
/// </summary>
public class clsValCertificado
{
    /// <summary>
    /// Objeto certificado 
    /// </summary>
    private byte[] gbCertificado;

    /// <summary>
    /// Retorna o establece el certificado como arreglo de bytes
    /// </summary>
    public byte[] CertificadoBytes
    {
        get { return gbCertificado; }
        set { gbCertificado = value; }
    }

    /// <summary>
    /// Objeto llave
    /// </summary>
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
    /// Verifica que el certificado aún sea vigente
    /// </summary>
    /// <returns></returns>
    public bool fnComprobarFechas()
    {

        if (certificado.NotBefore.CompareTo(DateTime.Today) > 0
            || certificado.NotAfter.CompareTo(DateTime.Today) < 0)
            return false;

        return true;
    }

    /// <summary>
    /// Revisa que no sea apocrifo
    /// </summary>
    /// <returns></returns>
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        return false;
        //return true;
    }

    /// <summary>
    /// Devuelve el arreglo de bytes del certificado original
    /// </summary>
    /// <returns></returns>
    private byte[] fnDesencriptarCertificado(byte[] pbCertificadoEncriptado)
    {
        //return Utilerias.Encriptacion.DES3.Desencriptar(pbCertificadoEncriptado);
        return (pbCertificadoEncriptado);
    }

    /// <summary>
    /// Devuelve el arreglo de bytes del certificado encriptados
    /// </summary>
    /// <returns></returns>
    public byte[] fnEncriptarCertificado()
    {
        //return Utilerias.Encriptacion.DES3.Encriptar(gbCertificado);
        return (gbCertificado);
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
    //public string fnEncriptarPassword()
    //{
    //    return gLlave.fnEncriptarPassword();
    //}

    public byte[] fnEncriptarPassword()
    {
        return gLlave.fnEncriptarPassword();
    }

    /// <summary>
    /// Función que revisa si es el certificado es un CSD
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Función que revisa si el certificado es una FIEL
    /// </summary>
    /// <returns></returns>
    public bool fnEsFiel()
    {
        try
        {
            return certificado.Extensions[1].Format(false).Contains("(d8)");
        }
        catch (Exception ex)
        {

        }

        return false;



    }

    /// <summary>
    /// Función que revisa si las fechas del certificado estan dentro del periodo de validez
    /// </summary>
    /// <param name="pdFechaComprobante">Fecha de comprobante</param>
    /// <returns></returns>
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
    /// Obtiene el número de certificado asociado al archivo a partir del número de serie contenido en el certificado
    /// </summary>
    /// <returns></returns>
	public string ObtenerNoCertificado()
	{
		byte[] bCertificadoInvertido = certificado.GetSerialNumber().Reverse().ToArray();
		return Encoding.Default.GetString(bCertificadoInvertido);
	}

    /// <summary>
    /// Obtiene el RFC contenido dentro del certificado
    /// </summary>
    /// <returns>Retorna una cadena ocn el RFC</returns>
    private string fnObtenerRfcEmisorDeCertificado()
    {
        try
        {
            //return new System.Text.RegularExpressions.Regex(".*OID.2.5.4.45=(?'rfc'.*)/").Match(certificado.SubjectName.Decode(X500DistinguishedNameFlags.UseNewLines)).Groups["rfc"].ToString().Trim();
            string[] cert = certificado.SubjectName.Decode(X500DistinguishedNameFlags.UseNewLines).Split('\n');

            //if (fnEsCSD())
            //{

            string[] rfc = cert[3].Split('=');

            string sRfc = string.Empty;

            if (rfc[1].Contains("/"))
            {
                string[] rfcFinal = rfc[1].Split('/');


                if (!clsComun.fnValidaExpresion(rfcFinal[0].Trim(), @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
                {
                    sRfc = rfcFinal[0].Substring(1, 14).Trim();
                }
                else
                {
                    sRfc = rfcFinal[0].Trim();
                }


                return sRfc;
            }
            else
            {
                return rfc[1].Trim();
            }
            //}
            //else
            //{
            //    if (fnEsFiel())
            //    {

            //        foreach (String sEntrada in cert)
            //        {

            //            string[] sElementos = sEntrada.Split('=')[1].Split('/');

            //            foreach (String sElemento in sElementos)
            //            {

            //                if (clsComun.fnValidaExpresion(sElemento.Trim(), @"[A-Z,Ñ,&amp;]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9][A-Z,0-9][0-9,A]"))
            //                {

            //                    return sElemento.Trim();

            //                }


            //            }


            //        }


            //    }


            //}
        }
        catch (CryptographicException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
        }

        return string.Empty;
    }

    /// <summary>
    /// Revisa caducidad del certificado en la LCO
    /// </summary>
    /// <param name="no_serie"></param>
    /// <param name="estado_cerR"></param>
    /// <param name="estado_cerC"></param>
    /// <returns></returns>
    public string fnRevisaCaducidadCertificado(string no_serie, string estado_cerR, string estado_cerC)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RevisarCaducidadCSD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("no_serie", no_serie));
                    cmd.Parameters.Add(new SqlParameter("sEstadoR", estado_cerR));
                    cmd.Parameters.Add(new SqlParameter("sEstadoC", estado_cerC));

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

    /// <summary>
    /// Revisa Existencia Certificado  Revisa existencia del certificado
    /// </summary>
    /// <param name="no_serie"></param>
    /// <param name="estado_cer"></param>
    /// <returns></returns>
    public string fnRevisaExistenciaCertificado(string no_serie, char estado_cer)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RevisarExistenciaCSD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("no_serie", no_serie));
                    cmd.Parameters.Add(new SqlParameter("sEstado", estado_cer));

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

    /// <summary>
    /// Revisa Existencia Certificado y sus fechas para la vigencia
    /// </summary>
    /// <param name="no_serie"></param>
    /// <param name="estado_cer"></param>
    /// <returns></returns>
    public DataTable fnRevisaExistenciaCertificadoFechas(string no_serie, string estado_cer)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RevisarExistenciaFechasCSD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("no_serie", no_serie));
                    cmd.Parameters.Add(new SqlParameter("sEstado", estado_cer));

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

    /// <summary>
    /// Revisa Existencia Certificado  Revisa existencia del certificado
    /// </summary>
    /// <param name="no_serie"></param>
    /// <param name="estado_cer"></param>
    /// <returns></returns>
    public string fnRevisaExistenciaCertificadoV2(string no_serie, char estado_cer)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RevisarExistenciaCSDV2", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("no_serie", no_serie));
                    cmd.Parameters.Add(new SqlParameter("sEstado", estado_cer));

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
        string sRFC = fnVerificarExistenciaCertificado();
        if (sRFC == string.Empty)
            return Resources.resCorpusCFDIEs.valEmisionCer;

        if ((psRFC != fnObtenerRfcEmisorDeCertificado()) || (sRFC != psRFC))
            return Resources.resCorpusCFDIEs.valRfcCer;

        string paridad = fnVerificarParidad();
        if (!string.IsNullOrEmpty(paridad))
            return paridad;

        if (!fnComprobarFechas())
            return Resources.resCorpusCFDIEs.valFechaCer;


        return string.Empty;
    }   

    /// <summary>
    /// Revisa en BD la existencia del certificado.
    /// </summary>
    /// <returns></returns>
    public string fnVerificarExistenciaCertificado()
    {
        string sRetorno = string.Empty;
        string sNoCertificado = ObtenerNoCertificado();

        if (sNoCertificado.Length != 20)
        {
            clsErrorLog.fnNuevaEntrada(new Exception("El número de certificado no cumple con la longitud adecuada"), clsErrorLog.TipoErroresLog.Datos);
        }

        try
        {
            sRetorno = fnRevisaExistenciaCertificado(sNoCertificado, 'A');
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return sRetorno;
    }

    /// <summary>
    /// Revisa en BD la existencia del certificado.
    /// </summary>
    /// <returns></returns>
    public string fnVerificarExistenciaCertificadoV2()
    {
        string sRetorno = string.Empty;
        string sNoCertificado = ObtenerNoCertificado();

        if (sNoCertificado.Length != 20)
        {
            clsErrorLog.fnNuevaEntrada(new Exception("El número de certificado no cumple con la longitud adecuada"), clsErrorLog.TipoErroresLog.Datos);
        }

        try
        {
            sRetorno = fnRevisaExistenciaCertificadoV2(sNoCertificado, 'A');
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return sRetorno;
    }

    /// <summary>
    /// Comprueba que el certificado corresponda a la llave privada proporcionada
    /// </summary>
    /// <returns>Un booleano indicando si el certificado corresponde o no a la llave privada</returns>
    private string fnVerificarParidad()
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
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
            string Certificate = "H:\\CertificadosPAC\\" + noCertificado + "\\" + noCertificado + ".cer";

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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return false;
        }
    }
}

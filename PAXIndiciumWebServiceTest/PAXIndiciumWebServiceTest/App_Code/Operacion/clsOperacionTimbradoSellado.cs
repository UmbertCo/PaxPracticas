using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;

/// <summary>
/// Clase encargada de manejar la llave privada y el firmado
/// </summary>
public class clsOperacionTimbradoSellado : IDisposable
{
    public clsOperacionTimbradoSellado()
    {

    }

    /// <summary>
    /// Crea una nueva instancia del manejador de llave privada
    /// </summary>
    /// <param name="pbLlave">Arreglo de bytes del archivo de llave privada</param>
    public clsOperacionTimbradoSellado(byte[] pbLlave)
    {
        gbLlave = pbLlave;
    }

    /// <summary>
    /// Crea una nueva instancia del manejador de llave privada
    /// </summary>
    /// <param name="pbLlave">Arreglo de bytes del archivo de llave privada</param>
    /// <param name="psPassword">Cadena con el password de la llave privada</param>
    public clsOperacionTimbradoSellado(byte[] pbLlave, string psPassword)
    {
        gbLlave = pbLlave;
        gsPassword = psPassword;
    }

    /// <summary>
    /// Tipo de algoritmo a usar
    /// </summary>
    public enum AlgoritmoSellado
    {
        MD5,
        SHA1
    }

    private byte[] gbLlave;

    /// <summary>
    /// Retorna o establece el arreglo de bytes del archivo key
    /// </summary>
    private byte[] LlavePrivada
    {
        get { return gbLlave; }
        set { gbLlave = value; }
    }

    private string gsPassword;

    /// <summary>
    /// Retorna o establece el password de la llave privada
    /// </summary>
    private string Password
    {
        get { return gsPassword; }
        set { gsPassword = value; }
    }

    #region Métodos Públicos

    /// <summary>
    /// Borra de memoria los bytes de la llave privada
    /// </summary>
    public void fnDestruirLlave()
    {
        try
        {
            Array.Clear(gbLlave, 0, gbLlave.Length);
            gbLlave = null;
        }
        catch
        {
            //Sin acción pues no existe llave alguna
        }
    }

    /// <summary>
    /// Devuelve el arreglo de bytes enviado como parametro de manera encriptada
    /// </summary>
    /// <param name="pbCertificado"></param>
    /// <returns></returns>
    public byte[] fnEncriptarLlave()
    {
        return Utilerias.Encriptacion.DES3.Encriptar(gbLlave);
    }

    /// <summary>
    /// Devuelve el password encriptado
    /// </summary>
    /// <returns></returns>
    public string fnEncriptarPassword()
    {
        return Convert.ToBase64String((Utilerias.Encriptacion.DES3.Encriptar(Encoding.UTF8.GetBytes(gsPassword))));
    }

    /// <summary>
    /// Firma la cadena pasada como parámetro con la llave privada
    /// </summary>
    /// <param name="psCadenaOriginal">Cadena a firmar</param>
    /// <param name="pAlgoritmo">Enumeración del algoritmo a usar</param>
    /// <returns>Retorna la cadena con el sello en base 64</returns>
    public string fnGenerarSello(string psCadenaOriginal, AlgoritmoSellado pAlgoritmo)
    {
        return fnGenerarSello(psCadenaOriginal, pAlgoritmo, false);
    }

    /// <summary>
    /// Firma la cadena pasada como parámetro con la llave privada
    /// </summary>
    /// <param name="psCadenaOriginal">Cadena a firmar</param>
    /// <param name="pAlgoritmo">Enumeración del algoritmo a usar</param>
    /// /// <param name="pbDesencriptar">Indica si la llave y el password deben ser desencriptados antes de ser usados</param>
    /// <returns>Retorna la cadena con el sello en base 64</returns>
    public string fnGenerarSello(string psCadenaOriginal, AlgoritmoSellado pAlgoritmo, bool pbDesencriptar)
    {
        try
        {
            //Llave privada original
            Chilkat.PrivateKey key = new Chilkat.PrivateKey();

            if (pbDesencriptar)
                key.LoadPkcs8Encrypted(fnDesencriptarLlave(gbLlave), fnDesencriptarPassword(gsPassword));
            else
                key.LoadPkcs8Encrypted(gbLlave, gsPassword);

            //Llave privada PEM
            Chilkat.PrivateKey pem = new Chilkat.PrivateKey();
            pem.LoadPem(key.GetPkcs8Pem());

            string pkeyXml = pem.GetXml();

            Chilkat.Rsa rsa = new Chilkat.Rsa();

            bool bSuccess;
            bSuccess = rsa.UnlockComponent("INTERMRSA_78UJEvED0IwK");
            bSuccess = rsa.GenerateKey(1024);

            rsa.LittleEndian = false;
            rsa.EncodingMode = "base64";
            rsa.Charset = "utf-8";
            rsa.ImportPrivateKey(pkeyXml);

            //Definimos el algoritmo
            string sAlgoritmo = string.Empty;
            if (pAlgoritmo == AlgoritmoSellado.SHA1)
                sAlgoritmo = "sha-1";
            else
                sAlgoritmo = "md5";

            string sello = rsa.SignStringENC(psCadenaOriginal, sAlgoritmo);

            //destruimos los objetos por seguridad
            try
            {
                key = new Chilkat.PrivateKey();
                key.Dispose();
                pem = new Chilkat.PrivateKey();
                pem.Dispose();
                rsa = new Chilkat.Rsa();
                rsa.Dispose();
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
            }

            return sello;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return null;
        }
    }

    /// <summary>
    /// Genera el XML en base a la estructura que contiene los datos
    /// </summary>
    /// <param name="datos">Estructura que contiene los datos</param>
    /// <returns>XmlDocument ocn los datos del objeto TimbreFiscalDigital</returns>
    public XmlDocument fnGenerarXML(TimbreFiscalDigital datos)
    {
        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms, Encoding.UTF8);
        XmlDocument xXml = new XmlDocument();
        XmlSerializerNamespaces sns = new XmlSerializerNamespaces();
        sns.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

        XmlSerializer serializador = new XmlSerializer(typeof(TimbreFiscalDigital));
        try
        {
            serializador.Serialize(sw, datos, sns);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);

            xXml.LoadXml(sr.ReadToEnd());
            XmlAttribute att = xXml.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            att.Value = "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/sitio_internet/timbrefiscaldigital/TimbreFiscalDigital.xsd";
            xXml.DocumentElement.SetAttributeNode(att);

            return xXml;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return xXml;
        }
    }

    #endregion

    #region Métodos Privados

    /// <summary>
    /// Devuelve el arreglo de bytes del archivo key
    /// </summary>
    /// <returns></returns>
    private byte[] fnDesencriptarLlave(byte[] pbLlave)
    {
        return Utilerias.Encriptacion.DES3.Desencriptar(pbLlave);
    }

    /// <summary>
    /// Desencripta el password
    /// </summary>
    /// <param name="psPassword">Cadena encriptada ocn el password</param>
    /// <returns></returns>
    private string fnDesencriptarPassword(string psPassword)
    {
        return Encoding.UTF8.GetString(Utilerias.Encriptacion.DES3.Desencriptar(Convert.FromBase64String(psPassword)));
    }

    #endregion   

    #region IDisposable Members

    void IDisposable.Dispose()
    {
        fnDestruirLlave();
    }

    #endregion
}

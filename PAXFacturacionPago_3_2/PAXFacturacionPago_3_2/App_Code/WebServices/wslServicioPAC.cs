using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;

/// <summary>
/// Summary description for wslServicioPAC
/// </summary>
[WebService(Namespace = "https://www.paxfacturacion.com.mx:452")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class wslServicioPAC : System.Web.Services.WebService {

    public wslServicioPAC () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public byte[] ObtenerLlavePAC()
    { 
        //Stream archivo = File.Open("C:\\pac.key", FileMode.Open);
        //BinaryReader br = new BinaryReader(archivo);

        byte[] key = Convert.FromBase64String(PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("keyPAC")));

        return key;
    }

    [WebMethod]
    public byte[] ObtenerCertificado()
    {
        //Stream archivo = File.Open("C:\\pac.cer", FileMode.Open);
        //BinaryReader br = new BinaryReader(archivo);

        byte[] cer = Convert.FromBase64String(PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("cerPAC")));

        return cer;
    }

    [WebMethod]
    public string ObtenerPassword()
    {
        return "12345678a";
    }

    /// <summary>
    /// *************************************************************************************
    /// </summary>


    [WebMethod]
    public byte[] HSM3_KEY()
    {
        //byte[] key = Convert.FromBase64String(Utilerias.Encriptacion.Base64.DesencriptarBase64(clsComun.ObtenerParamentro("HSMK")));
        byte[] key = Convert.FromBase64String(PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("HSMK")));
        return key;
    }

    [WebMethod]
    public byte[] HSM3_CER()
    {
        //byte[] cer = Convert.FromBase64String(Utilerias.Encriptacion.Base64.DesencriptarBase64(clsComun.ObtenerParamentro("HSMC")));
        byte[] cer = Convert.FromBase64String(PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("HSMC")));
        return cer;
    }

    [WebMethod]
    public string HSM3_PAS()
    {
        //return Utilerias.Encriptacion.Base64.DesencriptarBase64(clsComun.ObtenerParamentro("HSMP"));
        return PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("HSMP"));
    }

    /// <summary>
    /// *************************************************************************************
    /// </summary>
    
}

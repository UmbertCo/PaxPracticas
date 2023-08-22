using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for wslServicioPACTest
/// </summary>
public class wslServicioPACTest
{
	public wslServicioPACTest()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    [WebMethod]
    public byte[] ObtenerLlavePAC()
    {
        //Stream archivo = File.Open("C:\\pac.key", FileMode.Open);
        //BinaryReader br = new BinaryReader(archivo);

        byte[] key = Convert.FromBase64String(clsComunTest.fnObtenerParametro("keyPAC"));

        return key;
    }

    [WebMethod]
    public byte[] ObtenerCertificado()
    {
        //Stream archivo = File.Open("C:\\pac.cer", FileMode.Open);
        //BinaryReader br = new BinaryReader(archivo);

        byte[] cer = Convert.FromBase64String(clsComunTest.fnObtenerParametro("cerPAC"));

        return cer;
    }

    [WebMethod]
    public string ObtenerPassword()
    {
        return "a0123456789";
    }

    /// <summary>
    /// *************************************************************************************
    /// </summary>


    [WebMethod]
    public byte[] HSM3_KEY()
    {

        byte[] key = Convert.FromBase64String(Utilerias.Encriptacion.Base64.DesencriptarBase64(clsComunTest.fnObtenerParametro("HSMK")));

        return key;
    }

    [WebMethod]
    public byte[] HSM3_CER()
    {

        byte[] cer = Convert.FromBase64String(Utilerias.Encriptacion.Base64.DesencriptarBase64(clsComunTest.fnObtenerParametro("HSMC")));

        return cer;
    }

    [WebMethod]
    public string HSM3_PAS()
    {
        return Utilerias.Encriptacion.Base64.DesencriptarBase64(clsComunTest.fnObtenerParametro("HSMP"));
    }

    /// <summary>
    /// *************************************************************************************
    /// </summary>
}
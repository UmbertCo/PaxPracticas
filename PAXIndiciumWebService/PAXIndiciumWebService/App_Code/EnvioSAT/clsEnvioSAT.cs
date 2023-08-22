using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Web Service SAT******************************************************
using AutenticacionCancelacion = ServicioCancelacionAutenticacionCFDI;
using Cancelacion = ServicioCancelacionCFDI;
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
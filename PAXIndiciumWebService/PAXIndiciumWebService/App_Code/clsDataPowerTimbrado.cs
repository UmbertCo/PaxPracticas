using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;

/// <summary>
/// Descripción breve de clsDataPowerTimbrado
/// </summary>
public class clsDataPowerTimbrado
{
    public clsDataPowerTimbrado()
    { }

    public string clsTimbrarDataPower(string sDocumento, string urlDireccion)
    {
        string responseFromServer = string.Empty;

        try
        {
            WebRequest request = WebRequest.Create(urlDireccion);
            // Seteamos la propiedad Method del request a POST.
            request.Method = "POST";
            request.Proxy = null;
            // Creamos lo que se va a enviar por el metodo POST y lo convertimos a byte array.
            string postData = sDocumento;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Seteamos el ContentType del WebRequest a xml.
            request.ContentType = "text/xml";
            // Seteamos el ContentLength del WebRequest.
            request.ContentLength = byteArray.Length;
            // Obtenemos el request stream.
            Stream dataStream = request.GetRequestStream();
            // escribimos la data en el request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Cerramos el Stream object.
            dataStream.Close();
            //Obtiene la respuesta
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                //Peticion correcta del XML
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                responseFromServer = string.Empty;
                responseFromServer = reader.ReadToEnd();
                responseFromServer = responseFromServer.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n", "");
                reader.Close();
                response.Close();

            }
            catch (WebException ex)
            {
                //Control de errores del DP
                response = ex.Response;
                reader = new StreamReader(response.GetResponseStream());
                responseFromServer = string.Empty;
                responseFromServer = reader.ReadToEnd();
                responseFromServer = fnRecuepraCodigoError(responseFromServer);
                reader.Close();
                response.Close();
            }
        }
        catch (Exception)
        {
            responseFromServer = string.Empty;
        }

        return responseFromServer;
    }

    /// <summary>
    /// Manejador de control de errores de DP
    /// </summary>
    /// <param name="sResultado"></param>
    /// <returns></returns>
    public string fnRecuepraCodigoError(string sResultado)
    {
        try
        {
            XmlDocument xmlResultado = new XmlDocument();
            xmlResultado.LoadXml(sResultado);
            XmlNodeList response = xmlResultado.GetElementsByTagName("Response");
            XmlNodeList lista = ((XmlElement)response[0]).GetElementsByTagName("Error");

            foreach (XmlElement nodo in lista)
            {
                int i = 0;
                XmlNodeList nNombre = nodo.GetElementsByTagName("Code");
                XmlNodeList nApellido1 = nodo.GetElementsByTagName("Description");
                sResultado = nNombre[i].InnerText + " - " + nApellido1[i].InnerText;
            }
        }
        catch (Exception)
        {
            sResultado = string.Empty;
        }

        return sResultado;
    }
}
using Microsoft.Web.Services3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace SolucionPruebas.Servicios.Servicios
{
    /// <summary>
    /// Summary description for ServiceRecepcion
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Microsoft.Web.Services3.Policy("ServerPolicy")]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceRecepcion : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string fnServicioPrueba(string psPrueba)
        {
            //
            return psPrueba + "-" + "Regresado" + RequestSoapContext.Current.IdentityToken.Identity.Name;
        }

        [System.Web.Services.WebMethod]
        public void fnRecepcion()
        {
            XmlDocument xdDocumento = new XmlDocument();
            try
            {
                HttpRequest req = HttpContext.Current.Request;
                StreamReader reader = null;

                reader = new StreamReader(req.InputStream);

                string Peticion = reader.ReadToEnd();




                //Context.Response.Clear();
                //Context.Response.ContentType = "text/xml";
                //Context.Response.Write(Peticion);
                ////Context.Response.StatusCode = 500;
                //Context.Response.End();

                Context.Response.Clear();
                Context.Response.ContentType = "text/xml";
                Context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Response><Error><Code>300</Code><Description/></Error></Response>");
                Context.Response.StatusCode = 500;
                Context.Response.End();                
            }
            catch (Exception ex)
            {

            }
            //return xdDocumento;
        }
    }
}

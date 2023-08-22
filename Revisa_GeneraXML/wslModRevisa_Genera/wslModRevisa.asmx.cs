using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Revisa_GeneraXML;
using System.Xml;

namespace wslModRevisa_Genera
{
    /// <summary>
    /// Summary description for wslModRevisa
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class wslModRevisa : System.Web.Services.WebService
    {

        [WebMethod]
        public string fnRevisaTexto(string psXmlTexto)
        {
            Validador validar = new Validador(psXmlTexto);

            validar.fnRevisar();

            if (validar.bErrorAdvertencia)
            {
                return validar.sErrores;
            }
            
           

            return "Correcto";
        }

        [WebMethod]
        public string fnRevisaDocumento(XmlDocument pxdDocumento) 
        {


            return fnRevisaTexto(pxdDocumento.OuterXml);
        }

    }
}

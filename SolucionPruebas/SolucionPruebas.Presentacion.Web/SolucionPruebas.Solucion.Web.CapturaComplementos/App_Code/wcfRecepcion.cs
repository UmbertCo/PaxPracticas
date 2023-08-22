using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

/// <summary>
/// Summary description for wcfRecepcion
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class wcfRecepcion : System.Web.Services.WebService {

    [System.Web.Services.WebMethod]
    public static XmlDocument fnRecepcion()
    {
        XmlDocument xdDocumento = new XmlDocument();
        try
        {


            //sResultado = "<%@ Register Assembly=\"AjaxControlToolkit\" Namespace=\"AjaxControlToolkit\" TagPrefix=\"cc1\" %>" + sResultado;
        }
        catch (Exception ex)
        {

        }
        return xdDocumento;
    }
    
}

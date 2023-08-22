using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using MyFirstWebService;

namespace MyFirstWebService
{
    /// <summary>
    /// Summary description for wslServiceXML
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class wslServiceXML : System.Web.Services.WebService
    {


        [WebMethod]
        public string ProcessXMLFile(XmlDocument doc)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(512);

            string rfc = "";
            string fecha = "";
   
            XmlReader reader = new XmlNodeReader(doc);

            while (reader.Read())
            {
                if (reader.IsStartElement()) 
                {
                    switch (reader.Name)
                    {
                        case "cfdi:Comprobante":
                            fecha = reader["fecha"];
                            break;

                        case "cfdi:Emisor":
                            rfc = reader["rfc"];
                            break;
                    }
                }
            }

            if (!ValidaRFC(rfc))
            {
                sb.AppendLine("RFC no valido,");
            }
            else
            {
                sb.AppendLine("RFC valido,");
            }


            if (!ValidarFecha(fecha, "yyyy-MM-ddTHH:mm:ss"))
            {
                sb.AppendLine("Fecha No Valida");
                sb.Append(Environment.NewLine);
            }
            else
            {
                sb.AppendLine("Fecha Valida");
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
    
        }

        public bool ValidarFecha(string date, string date_format)
        {
            try
            {
                System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.DateTimeFormatInfo();
                dtfi.LongDatePattern = date_format;
                DateTime dt = DateTime.ParseExact(date, date_format, dtfi);
            }
            catch (Exception)
            {
                return false;
               
            }
            return true;
        }


        public bool ValidarFecha2(string date, string date_format)
        {
            try
            {
                System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.DateTimeFormatInfo();
                dtfi.LongDatePattern = date_format;
                DateTime dt = DateTime.ParseExact(date, date_format, dtfi);
            }
            catch (Exception)
            {
                return false;

            }
            return true;
        }

        private bool ValidaRFC(string sRFCNuevo)          
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(sRFCNuevo, @"^([a-zA-Z]{4})\d{6}([A-Z\w]{3})$"))
            {
                return true;
                //dehjhehjrjjhre
            }
            else
            {
                return false;
            }
          
        }

    }
}

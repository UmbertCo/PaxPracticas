using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using wcfTimbrado.Properties;
using System.Net;
using System.Net.Security;
using System.Xml;
using System.Text;
using System.ServiceModel.Web;
using System.IO;
using System.Xml.XPath;

namespace wcfTimbrado
{
    /// <summary>
    /// Descripción breve de ServiciodeTimbrado
    /// </summary>
    [System.Web.Script.Services.ScriptService()]
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiciodeTimbrado : System.Web.Services.WebService
    {
        public ServiciodeTimbrado()
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(AcceptAllCertifications);
        }

        [WebMethod]
        public void RecepcionTimbrado()
        {
            try
            {
                StreamReader reader = null;
                string sResultado, sComprobante, linea, sVersion, sTipoComprobante = string.Empty;
                StringBuilder sResponse = new StringBuilder();
                string[] seccion, error, codigo = null;
                System.IO.StringReader lector;
                XmlDocument xDocument = new XmlDocument();
                XmlNode nodo;
                HttpRequest peticion = HttpContext.Current.Request;
                wsRecepcionTimbrado.wcfRecepcionASMXSoapClient EnviarXml = new wsRecepcionTimbrado.wcfRecepcionASMXSoapClient();

                reader = new StreamReader(peticion.InputStream);
                sComprobante = reader.ReadToEnd();

                xDocument.LoadXml(sComprobante);

                XmlNamespaceManager nsmDocumento = new XmlNamespaceManager(xDocument.NameTable);
                nsmDocumento.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmDocumento.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                try
                {
                    sVersion = xDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Version", nsmDocumento).Value;
                }
                catch
                {
                    sVersion = xDocument.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@version", nsmDocumento).Value;
                }

                try
                {
                    XPathNavigator navComprobante = xDocument.CreateNavigator();
                    XPathNodeIterator navDetalles = navComprobante.Select("/xmlns:nomina12", nsmDocumento);
                    if (navDetalles != null)
                    {
                        sTipoComprobante = fnComparaTipoDocumento("recibo de nómina");
                    }
                    else
                    {
                        if(sVersion.Equals("3.3"))
                            sTipoComprobante = fnComparaTipoDocumento("factura");
                        else
                            sTipoComprobante = "factura";
                    }
                }
                catch
                {
                    sTipoComprobante = "factura";
                }

                try
                {
                    sResultado = EnviarXml.fnEnviarXML(sComprobante, sTipoComprobante, 0, Settings.Default.Usuario, Settings.Default.Contraseña, sVersion);
                }
                catch (Exception ex)
                {
                    throw new WebException(ex.ToString());
                }

                if (sResultado.Contains("<") || sResultado.Contains(">"))
                {
                    xDocument.LoadXml(sResultado);
                }
                else
                {
                    lector = new System.IO.StringReader(sResultado);
                    linea = lector.ReadLine().Trim();
                    seccion = linea.Split(';');

                    sResponse.Append("<Response>");
                    sResponse.Append("<Error>");

                    for (int i = 0; i < seccion.Length; i++)
                    {
                        error = seccion[i].Split('-');

                        if (error[0] != "")
                        {
                            if (error.Length > 1)
                            {
                                sResponse.Append("<Code>" + error[0].Trim() + "</Code>");
                                sResponse.Append("<Description>" + error[1].Trim() + ";</Description>");
                            }
                            else
                            {
                                codigo = error[0].Split('|');
                                sResponse.Append("<Code>" + codigo[0] + "</Code>");
                                sResponse.Append("<Description>" + codigo[0] + "|" + error[0] + ";</Description>");
                            }
                        }

                    }

                    sResponse.Append("</Error>");
                    sResponse.Append("</Response>");

                    xDocument.LoadXml(sResponse.ToString());
                }

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocument.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                XmlElement root = xDocument.DocumentElement;
                nodo = root.SelectSingleNode("/cfdi:Comprobante", nsmComprobante);

                Context.Response.ContentType = "text/xml";

                clsLog.Escribir(Settings.Default.LogError, xDocument.InnerXml);

                if (nodo == null)
                {
                    Context.Response.StatusCode = 500;
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(xDocument.InnerXml);

                //Context.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
                Context.Response.TrySkipIisCustomErrors = true;
                Context.Response.Clear();
                Context.Response.Buffer = true;
                Context.Response.Charset = "UTF-8";
                Context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                Context.Response.Write(xDocument.InnerXml);
                Context.Response.Flush();
                //Context.Response.End();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                clsLog.Escribir(Settings.Default.LogError, "Error: ThreadAbortException " + ex.Message);
            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError, "Error: " + ex.Message);
            }
        }

        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


        /// <summary>
        /// Compara metodos de pago con su clave y obtiene su descripcion
        /// </summary>
        /// <returns>string</returns>
        private string fnComparaTipoDocumento(string psTipoDocumento)
        {
            string sDescripcionOut = string.Empty;

            try
            {
                string[] asTipoDocumento;
                if (psTipoDocumento.Contains(","))
                {
                    asTipoDocumento = psTipoDocumento.Split(',');
                }
                else
                {
                    asTipoDocumento = new string[] { psTipoDocumento };
                }

                foreach (string sTipoDocumento in asTipoDocumento)
                {
                    XmlDocument xmlMetodos = new XmlDocument();

                    xmlMetodos.Load(AppDomain.CurrentDomain.BaseDirectory + "TiposDocumento.xml");

                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlMetodos.NameTable);
                    nsmComprobante.AddNamespace("dc", "dc:documents");

                    XPathNavigator navComprobante = xmlMetodos.CreateNavigator();
                    XPathNodeIterator navDetalles = navComprobante.Select("/TiposDoc/Document");

                    while (navDetalles.MoveNext())
                    {
                        XPathNavigator nodenavigator = navDetalles.Current;

                        string Nombre = nodenavigator.SelectSingleNode("@dc:nombre", nsmComprobante).Value;
                        string Id = nodenavigator.SelectSingleNode("@dc:id", nsmComprobante).Value;
                        string Tipo = nodenavigator.SelectSingleNode("@dc:tipo", nsmComprobante).Value;

                        if (sTipoDocumento.Equals(Nombre))
                        {
                            sDescripcionOut = Id;
                        }
                    }
                }

                if (sDescripcionOut.Equals(string.Empty))
                {
                    return psTipoDocumento;
                }
            }
            catch (System.Exception)
            {
                sDescripcionOut = string.Empty;
            }
            return sDescripcionOut;
        }
    }
}

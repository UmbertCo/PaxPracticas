using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;
using System.Globalization;
using OpenSSL_Lib;
using System.Xml.Serialization;

namespace ServicioXML
{
    /// <summary>
    /// Descripción breve de TimbrarFactura
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]

    public class TimbrarFactura : System.Web.Services.WebService
    {
        public static X509Certificate2 certEmisor;

        [WebMethod]
        // fnGuardaFactura: Proceso para guardar facturas
        // psRutaArchivo -> Ruta del archivo a convertir
        //public static X509Certificate2 certEmisor = new X509Certificate2();
        //public static OpenSSL_Lib.cSello cSello;
        public string fnGuardaFactura(string psComprobante, string psTipoDocumento, int pnId_Estructura, string sNombre, string sContraseña, string sVersion)
        {
            // Validar usuario y contraseña
            if(sNombre != "WSDL_PAX" || sContraseña != "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==")
                return "998 - Contraseña o usuario incorrecto.";

            // Crear XML
            XmlDocument xDoc = new XmlDocument();
            xDoc.InnerXml = psComprobante;

            // Validar que factura no se haya timbrado con anterioridad
            string sXmlconverted = fnGetSHA1(xDoc.OuterXml);
            string fldFactTimb = System.Web.Configuration.WebConfigurationManager.AppSettings["fld_facturas_timbradas"];
            bool exists = System.IO.Directory.Exists(fldFactTimb);
            if (!exists)
                System.IO.Directory.CreateDirectory(fldFactTimb);
            foreach (string sFile in Directory.GetFiles(fldFactTimb))
            {
                XmlDocument xDoc2 = new XmlDocument();
                xDoc2.Load(sFile);
                string sXmlconverted2 = fnGetSHA1(xDoc2.InnerXml); // Convertir a SHA1
                if (sXmlconverted == sXmlconverted2) // Comparar contenido de factura recibida con factura guardada
                    return "999 - El servicio encontró que esta factura ya habia sido timbrada.";
            }

            // Validar que no pasen 3 días de haber generado factura
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDoc.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            string sfecha = string.Empty;
            try { sfecha = xDoc.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value; }
            catch { }

            DateTime dFechaComprobante = DateTime.MinValue;
            if (!string.IsNullOrEmpty(sfecha))
                dFechaComprobante = Convert.ToDateTime(sfecha);
            else
                return "997 - El servicio encontró que esta factura no contiene una fecha.";

            DateTime dateHoy = DateTime.Now;
            TimeSpan ts = dateHoy - dFechaComprobante;

            int nDiffHoras = (ts.Days * 24) + ts.Hours;

            if (nDiffHoras > 72)
                return "401 - Que el rango de la fecha de generación no sea mayor a 72 horas para la emisión del timbre.";

            // Timbra factura
            XmlDocument xDocSinTimbre = new XmlDocument();
            xDocSinTimbre.InnerXml = xDoc.InnerXml;
            if (agregarTimbreFactura(xDoc))
            {
                xDocSinTimbre.Save(fldFactTimb + DateTime.Now.ToString("yymmddhhmmss") + ".xml"); // Guardar factura
            }
            else
            {
                return "404 - No se pudo timbrar la factura.";
            }

            // Factura timbrada con exito
            return xDoc.OuterXml;
            //return "";
        }

        

        // sfnGetSHA1: Función para convertir texto en hash SHA1
        // psTexto -> Texto que se convertira
        public static string fnGetSHA1(String psTexto)
        {
            SHA1 sha1 = SHA1CryptoServiceProvider.Create();
            Byte[] textOriginal = Encoding.UTF8.GetBytes(psTexto);
            Byte[] hash = sha1.ComputeHash(textOriginal);
            StringBuilder sCadena = new StringBuilder();
            foreach (byte i in hash)
            {
                sCadena.AppendFormat("{0:x2}", i);
            }
            return sCadena.ToString();
        }

        // sfnWriteErrorLog: Función para escribir mensaje en el log
        // psMensaje -> Texto que se convertira
        public static void sfnWriteErrorLog(string psMensaje)
        {

            StreamWriter sw = null;
            try
            {
                string sFldLogError = System.Web.Configuration.WebConfigurationManager.AppSettings["arc_LogError"];
                bool exists = System.IO.Directory.Exists(sFldLogError);
                if (!exists)
                    System.IO.Directory.CreateDirectory(sFldLogError);
                string sArcLogError = sFldLogError + "/LogFile.txt";
                sw = new StreamWriter(sArcLogError, true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + psMensaje);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Crear elementos Raiz del Documento en Version 3.0
        /// </summary>
        /// <param name="pxDocumento">Documento</param>
        /// <param name="psElemento">Elemento</param>
        /// <param name="psAtributos">Atributos</param>
        /// <returns></returns>
        public static XmlElement fnCrearElementoTimbre(XmlDocument pxDocumento, string psElemento, string[] psAtributos)
        {
            XmlAttribute xAttr;

            XmlElement elemento = pxDocumento.CreateElement("tfd", psElemento, "http://www.sat.gob.mx/TimbreFiscalDigital");

            foreach (string a in psAtributos)
            {
                string[] valores = a.Split('@');
                xAttr = pxDocumento.CreateAttribute(valores[0]);
                xAttr.Value = valores[1];
                elemento.Attributes.Append(xAttr);
            }

            //Agregar schemalocation//////////
            xAttr = pxDocumento.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xAttr.Value = "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/TimbreFiscalDigital/TimbreFiscalDigital.xsd";
            elemento.Attributes.Append(xAttr);
            ///////////////////////////////////

            return elemento;
        }

        /// <summary>
        /// Función que contruye la cadena original
        /// </summary>
        /// <param name="psXml">Documento</param>
        /// <param name="psNombreArchivoXSLT">Nombre del archivo de tranformación</param>
        /// <returns></returns>

        public static string fnConstruirCadenaTimbrado(IXPathNavigable psXml, string psNombreArchivoXSLT)
        {
            string sCadenaOriginal = string.Empty;
            MemoryStream ms;
            StreamReader srDll;
            XsltArgumentList args;
            XslCompiledTransform xslt;
            try
            {
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOri.V32));
                //xslt.Load(psNombreArchivoXSLT);
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(psXml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                sCadenaOriginal = srDll.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception(DateTime.Now + " " + "Error al generar la cadena original." + " " + ex.Message);
                //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + ex.Message);
            }
            return sCadenaOriginal;
        }

        public static Boolean agregarTimbreFactura(XmlDocument psXmlDoc)
        {
            try
            {
                string sRFCEmisor = string.Empty;
                string sSello = string.Empty;
                string sNoCertificado = string.Empty;
                string sRFCReceptor = string.Empty;
                string xsd_validacion = string.Empty;
                string noCertificadoPAC = string.Empty;
                DateTime dFechaComprobante;

                // Definir Nombre de espacio
                XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());

                nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsm.AddNamespace("fomadd", "http://www.ford.com.mx/cfdi/addenda");
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                // Obtener datos de XML
                sSello = psXmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsm).Value;
                sRFCEmisor = psXmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsm).Value;
                sRFCReceptor = psXmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsm).Value;
                sNoCertificado = psXmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsm).Value;
                dFechaComprobante = psXmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsm).ValueAsDateTime;

                // Obtener Certificado
                ObtenerCertificado();
                byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();

                // Generar nodo de Timbre Fiscal
                TimbreFiscalDigital gNodoTimbre = new TimbreFiscalDigital();
                gNodoTimbre.UUID = Guid.NewGuid().ToString();
                gNodoTimbre.FechaTimbrado = Convert.ToDateTime(DateTime.Now.ToString("s"));
                gNodoTimbre.selloCFD = sSello;
                gNodoTimbre.noCertificadoSAT = Encoding.Default.GetString(bCertificadoInvertido).ToString();

                XmlDocument docNodoTimbre;
                docNodoTimbre = fnGenerarXML(gNodoTimbre);
                gNodoTimbre.selloSAT = generarSelloSAT(docNodoTimbre);
                docNodoTimbre = fnGenerarXML(gNodoTimbre);

                XmlNode nodoComplemento = psXmlDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsm);

                if (nodoComplemento == null)
                    nodoComplemento = psXmlDoc.DocumentElement.AppendChild(psXmlDoc.CreateElement("cfdi", "Complemento", "http://www.sat.gob.mx/cfd/3"));

                nodoComplemento.InnerXml = docNodoTimbre.DocumentElement.OuterXml + nodoComplemento.InnerXml;

                psXmlDoc.DocumentElement.AppendChild(nodoComplemento);

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }

            /*//string sUUID = Guid.NewGuid().ToString();
           // string sFechaTimbrado = DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss");

            //string sSelloCFD = string.Empty;
            //try { sSelloCFD = psXmlDoc.SelectSingleNode("/cfdi:Comprobante/@sello", nsm).Value; }
            //catch { }

            string sNoCertCFD = string.Empty;
            try { sNoCertCFD = psXmlDoc.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsm).Value; }
            catch { }

            string sError = "";
            if (!string.IsNullOrEmpty(sSelloCFD))
                sError = "990 - No se puede obtener el sello del Emisor.";

            //Cerificado para agregar al XML
            string sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());
            //Numero del certificado
            //byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
            //string sNoCertSAT = Encoding.Default.GetString(bCertificadoInvertido).ToString();
            //string noCertSAT = "20001000000100005761";
            //string selloSAT = sfnGetSHA1(noCertSAT);
            string sSelloSAT = "||1.0|" + sUUID + "|" + sFechaTimbrado + "|" + sNoCertCFD + "|" + sNoCertSAT + "||";
            sSelloSAT = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sSelloSAT));

            string sCadenaTimbrado = "version@1.0|UUID@" + sUUID + "|FechaTimbrado@" + sFechaTimbrado + "|selloCFD@" + sSelloCFD + "|noCertificadoSAT@" + sNoCertSAT
                                        + "|selloSAT@" + sSelloSAT;
            string[] atributos = sCadenaTimbrado.Split('|');
            nodoComplemento.AppendChild(fnCrearElementoTimbre(psXmlDoc, "TimbreFiscalDigital", atributos));*/
        }

        public static void ObtenerCertificado()
        {
            try
            {
                // Obtener certifica y sello del sat
                string[] FilesCer = null;
                string sRutaCert = System.Web.Configuration.WebConfigurationManager.AppSettings["fld_certificadoSAT"];
                string sFiltroCert = "*.cer";
                FilesCer = Directory.GetFiles(sRutaCert, sFiltroCert);
                certEmisor = new X509Certificate2();

                foreach (string filecer in FilesCer)
                {
                    certEmisor.Import(filecer);
                }
            }
            
            catch(Exception e)
            {
                // Hubo un error
                sfnWriteErrorLog("Hubo el siguiente error: " + e.Message);
            }
        }

        public static XmlDocument fnGenerarXML(TimbreFiscalDigital datos)
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
                sfnWriteErrorLog("Hubo el siguiente error: " + ex.Message);
                return xXml;
            }
        }

        public static string generarSelloSAT(XmlDocument docTimbre)
        {
            //-- Generamos la cadena original ------------------------------------------------
            XPathNavigator navNodoTimbre = docTimbre.CreateNavigator();
            XslCompiledTransform xslt;
            XsltArgumentList args;
            MemoryStream ms;
            StreamReader srDll;
            //-- Load the type of the class --------------------------------------------------
            xslt = new XslCompiledTransform();
            xslt.Load(typeof(Timbrado.V3.TFDXSLT));
            //--------------------------------------------------------------------------------
            ms = new MemoryStream();
            args = new XsltArgumentList();
            //--------------------------------------------------------------------------------
            xslt.Transform(navNodoTimbre, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            srDll = new StreamReader(ms);
            //--------------------------------------------------------------------------------
            string sCadenaOriginal = srDll.ReadToEnd();
            //--------------------------------------------------------------------------------
            string selloSAT = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sCadenaOriginal));
            return selloSAT;
        }
    }
}
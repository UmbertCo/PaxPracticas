using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenSSL_Lib;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Security.Cryptography;
using System.Xml.Schema;
using System.Data;
using wsGeneraXml.wcfRecepcionasmx;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Security;
using wsGeneraXml.Properties;
using System.Reflection;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Net.Mail;
using wsGeneraXml;

namespace wsGeneraXml
{
    public class clsGenerar
    {
        #region Variables Privadas

        private byte[] gLlave;
        private byte[] gLlavePAC;
        private string gsPassword;
        private string _sSerie;
        private string _sFolio;

        #endregion

        #region Propiedades Públicas

        /// <summary>
        /// Retorna o establece el arreglo de bytes del archivo key
        /// </summary>
        public byte[] LlavePrivada
        {
            get { return gLlave; }
            set { gLlave = value; }
        }

        /// <summary>
        /// Retorna o establece el arreglo de bytes del archivo key
        /// </summary>
        public byte[] LlavePrivadaPAC
        {
            get { return gLlavePAC; }
            set { gLlavePAC = value; }
        }

        /// <summary>
        /// Retorna o establece el password de la llave privada
        /// </summary>
        public string Password
        {
            get { return gsPassword; }
            set { gsPassword = value; }
        }

        public string gsSerie
        {
            get
            {
                return _sSerie;
            }
            set
            {
                _sSerie = value;
            }
        }

        public string gsFolio
        {
            get
            {
                return _sFolio;
            }
            set
            {
                _sFolio = value;
            }
        }

        #endregion

        private bool bValida { get; set; }
        DataTable tblComplementos;
        XmlNamespaceManager nsmComprobante = null;
        Comprobante comprobante = new Comprobante();
        object clase;

        //Servicios
        private wcfRecepcionASMXSoapClient wsRecepcionT = new wcfRecepcionASMXSoapClient();

        //Seguridad    
        private static string xsd_validacion;
        private static string xsd_error_code;
        X509Certificate2 certEmisor = new X509Certificate2();
        OpenSSL_Lib.cSello cSello;

        //


        public clsGenerar()
        {
            DateTime Fecha = DateTime.Today;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsGenerar.AcceptAllCertificatePolicy);

                fnGenerarLlave();


            }
            catch (Exception ex)
            {
                clsLog.Escribir(Settings.Default.LogError+ "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "Error al generar las llaves para el sello. Asegurese de que las llaves esten en la carpeta de certificados y que la contraseña sea la correcta. " + ex.Message);
            }
        }

        public void fnComenzarGeneracion(string psServicio)
        {
            switch (psServicio)
            {
                case "GT": //Timbrado
                    fnTimbradoGeneracion();
                    break;
            }
        }

        public void fnTimbradoGeneracion()
        {
            string[] Files = null;
            string RutaXMLDocs = Settings.Default.rutaDocs;
            string filtro = "*.txt";
            Files = Directory.GetFiles(RutaXMLDocs, filtro);
            DateTime Fecha = DateTime.Today;
            XmlDocument xXML = new XmlDocument();

            foreach (string archivo in Files)
            {
            fnRecuperarEsquemas();
            char[] cCad = { '-' };
            string[] sCad;
            int nBandera = 0;
            System.IO.StringReader lectorVersion;
            string noCertificado = string.Empty;
            string sNombreTXT = Path.GetFileNameWithoutExtension(archivo);
            string sText = string.Empty;
            string sXMLGenerado = string.Empty;
            string sXML = string.Empty;
            XmlDocument xDocumento = new XmlDocument();
            XmlNode nodo = null;
            while ((fnWaitForFile(archivo) == false))
            {
                //Se hace pato un rato.(Se espera a que se desbloquee el Archivo)
            }

            using (Stream stream = File.Open(archivo.ToString(), FileMode.Open))
            {
                StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                sText = sr.ReadToEnd();
                lectorVersion = new System.IO.StringReader(sText);
            }

            try
            {
                sXMLGenerado = fnGenerarComprobante(sText, sNombreTXT);
                if (sXMLGenerado.Equals(string.Empty))
                {
                    //Si el txt es invalido
                    //Copia el archivo txt invalido a otra carpeta
                    File.Copy(archivo, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(archivo);
                    continue;
                    //return;
                }

                //xDocumento.LoadXml(sXMLGenerado);

                ///////RETIRAR ADDENDA///////
                XmlDocument xDocSinAddenda = new XmlDocument(); //Sera enviado al servicio
                xDocSinAddenda.LoadXml(sXMLGenerado);

                //Quitamos el nodo de la addenda del XMLDocument
                XmlNamespaceManager nsmCom = new XmlNamespaceManager(xDocSinAddenda.NameTable);
                nsmCom.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmCom.AddNamespace("fomadd", "http://www.ford.com.mx/cfdi/addenda");
                nsmCom.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                bool bAddenda = true;
                bool Validacion = true;

                String xAddenda = "";
                try
                {
                    xAddenda = xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmCom).OuterXml;

                    XmlDocument AddendaFOB = new XmlDocument();
                    AddendaFOB.LoadXml(xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmCom).InnerXml);


                    XmlNamespaceManager nsmComAd = new XmlNamespaceManager(AddendaFOB.NameTable);
                    nsmComAd.AddNamespace("fomadd", "http://www.ford.com.mx/cfdi/addenda");

                    string Ad = AddendaFOB.OuterXml;



                    xsd_validacion = string.Empty;
                    xsd_error_code = string.Empty;
                    xsd_validacion = fnValidate(AddendaFOB, 1);//"esquema_v3");
                    if (xsd_validacion != string.Empty && xsd_validacion != null)
                    {
                        Validacion = false;
                        throw new System.ArgumentException("333 - " + xsd_validacion, "valida esquema.");
                    }

                    xAddenda = xAddenda.Replace(" xmlns:cfdi=\"http://www.sat.gob.mx/cfd/3\"", string.Empty);
                    xAddenda = xAddenda.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", string.Empty);

                    xDocSinAddenda.CreateNavigator().SelectSingleNode("cfdi:Comprobante/cfdi:Addenda", nsmCom).DeleteSelf();
                }
                catch
                {
                    bAddenda = false;

                }
                string postData = xDocSinAddenda.OuterXml;
                ////////////////////////////


                System.Threading.Thread.Sleep(500);

                try
                {
                    //Se manda el xml a timbrar
                    if (Validacion == true)
                    {
                        sXML = wsRecepcionT.fnEnviarXML(xDocSinAddenda.OuterXml, "Factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2");
                    }
                    else
                    {
                        sXML = "Error en addenda";
                    }
                }
                catch (Exception ex)
                {
                    sXML = "Error al momento de timbrar el comprobante." + " " + ex.Message;

                }


                //Se valida el tipo de respuesta
                sCad = sXML.Split(cCad);
                if (sCad.Length <= 2)
                {
                    nBandera = 1; //Se indica que el comprobante no fue timbrado
                    //En caso de marcar error se graba un log

                    if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                        clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + sXML + ", Nombre txt: " + sNombreTXT);
                    else
                        clsLog.Escribir(Settings.Default.LogError + "LogErrorSinTimbrar" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + sXML + ", Nombre txt: " + sNombreTXT);
                }

                //Si el documento fue timbrado se guarda el XML en ruta definida
                if (nBandera == 0)
                {
                    //Se obtiene el xml

                    try
                    {
                        xXML.LoadXml(sXML);
                    }
                    catch (Exception ex)
                    {
                        //Si al cargar el comprobante marcar error
                        clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + sXML + ", Nombre txt: " + sNombreTXT + ". Respuesta: " + xXML + " " + ex.Message);
                        //Copia el archivo txt invalido a otra carpeta
                        File.Copy(archivo, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                        //Elimina el archivo txt
                        File.Delete(archivo);
                        //continue;
                    }
                    //Se vuelve a agregar la addenda si existe
                    if (bAddenda)
                        xXML.CreateNavigator().SelectSingleNode("cfdi:Comprobante", nsmCom).AppendChild(xAddenda);
                    ////////////////////////////////////////////
                    XPathNavigator navEncabezado = xXML.CreateNavigator();

                    //Se obtiene el UUID
                    //try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                    //catch { snombreDoc = Guid.NewGuid().ToString(); }

                    //Se guarda XML en ruta especificada 
                    //xXML.Save(Settings.Default.RutaDocZips + sNombreTXT + ".xml");

                    //var rutaAbsolutaAcuse = string.Format("{0}\\{1}.xml", "C:/Proyectos/", sNombreTXT);
                    //AccesoDisco.GuardarArchivoTexto(rutaAbsolutaAcuse, xXML.OuterXml);
                    //File.WriteAllText(rutaAbsolutaAcuse, xXML.OuterXml);
                    fnAgregarAddenda(ref xXML, sText, ref nsmCom);

                    nodo = xXML.SelectSingleNode("/cfdi:Comprobante/Addenda/Correos/Correo", nsmCom);
                    if (nodo != null)
                    {

                        string scorreo, sasunto, smensaje;
                        scorreo = sasunto = smensaje = string.Empty;

                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("gabriel.reyes@paxfacturacion.com", "3A4Xhlah");



                        scorreo = xXML.SelectSingleNode("/cfdi:Comprobante/Addenda/Correos/Correo/@CorreoElectronicoReceptor", nsmCom).Value;
                        sasunto = xXML.SelectSingleNode("/cfdi:Comprobante/Addenda/Correos/Correo/@Asunto", nsmCom).Value;
                        smensaje = xXML.SelectSingleNode("/cfdi:Comprobante/Addenda/Correos/Correo/@CuerpoMensaje", nsmCom).Value;

                        MailMessage email = new MailMessage();
                        email.To.Add(new MailAddress(scorreo));
                        email.From = new MailAddress("example2@example.com");
                        email.Subject = sasunto;
                        email.Body = smensaje;
                        email.IsBodyHtml = true;
                        email.Priority = MailPriority.Normal;

                        smtp.Send(email);
                        email.Dispose();
                    }
                    try
                    {
                        //Generar PDF
                        //string sRutaPDF = Settings.Default.RutaDocZips;
                        fnCrearPlantillaEnvio(xXML, Settings.Default.tipodocto, Settings.Default.rutaTXTGen + sNombreTXT + ".pdf");
                    }
                    catch (Exception ex)
                    {
                        clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "No se pudo generar la representación impresa del CFDI. " + ex.Message);
                    }

                    //Se guarda log de comprobantes timbrados
                    clsLog.Escribir(Settings.Default.LogTimbrados + "LogTimbrados", DateTime.Now + ", Nombre txt: " + sNombreTXT);

                    //Copia el archivo txt timbrado a otra carpeta
                    File.Copy(archivo, Settings.Default.rutaTXTGen + sNombreTXT + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(archivo);
                }
                else
                {
                    if (sXML.Contains("Error al momento de timbrar el comprobante"))
                    {
                        //Si el txt es invalido y fue por una problema en la llamada al servicio
                        //Copia el archivo txt invalido a otra carpeta
                        File.Copy(archivo, Settings.Default.rutaDesconexion + sNombreTXT + ".txt", true);
                        //Elimina el archivo txt
                        File.Delete(archivo);
                    }
                    else
                    {
                        //Si el txt es invalido
                        //Copia el archivo txt invalido a otra carpeta
                        File.Copy(archivo, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                        //Elimina el archivo txt
                        File.Delete(archivo);
                    }
                }
                xXML.Save(Settings.Default.rutaTXTGen + sNombreTXT + ".xml");
            }
            catch (Exception ex)
            {
                if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                    clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "Ha ocurrido un error al generar el comprobante. " + ex.Message);
                else
                    clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "Ha ocurrido un error al generar el comprobante. " + ex.Message);

                //Si el txt es invalido
                //Copia el archivo txt invalido a otra carpeta
                File.Copy(archivo, Settings.Default.rutaDocInv + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                //Elimina el archivo txt
                File.Delete(archivo);
            }
            }

            
            //return xXML.OuterXml;
        }

        /// <summary>
        /// Crea archivo pdf segun plantilla configurada para su posterior envio de correo
        /// </summary>
        /// <param name="pxComprobante"></param>
        /// <param name="psTipoDocumento"></param>
        /// <param name="sRuta"></param>
        public void fnCrearPlantillaEnvio(XmlDocument pxComprobante, string psTipoDocumento, string sRuta)
        {
            if (!(sRuta == string.Empty))
            {
                clsPlantilla pdf = new clsPlantilla(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdf.TipoDocumento = psTipoDocumento.ToUpper();
                pdf.fnGenerarPDFSave(sRuta, "Black");
            }
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Revisa que la fecha del certificado sea valida
        /// </summary>
        /// <returns></returns>
        private bool fnComprobarFechas()
        {
            bool bResultado = true;

            if (certEmisor.NotBefore.CompareTo(DateTime.Today) > 0 || certEmisor.NotAfter.CompareTo(DateTime.Today) < 0)
                return false;

            return bResultado;
        }

        /// <summary>
        /// Función que contruye la cadena original
        /// </summary>
        /// <param name="xml">Documento</param>
        /// <param name="psNombreArchivoXSLT">Nombre del archivo de tranformación</param>
        /// <returns></returns>
        private string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
        {
            string sCadenaOriginal = string.Empty;
            MemoryStream ms;
            StreamReader srDll;
            XsltArgumentList args;
            XslCompiledTransform xslt;
            try
            {
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CadOr));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(xml, args, ms);
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

        /// <summary>
        /// Función que revisa que el certificado no sea apocrifo
        /// </summary>
        /// <returns></returns>
        private bool fnCSD308()
        {
            bool bRetorno = false;
            try
            {
                if (certEmisor.IssuerName.Name.Contains("A.C. del Servicio de Administración Tributaria"))
                    bRetorno = true;
                else
                    bRetorno = false;
            }
            catch (Exception)
            {

            }
            return bRetorno;
        }

        /// <summary>
        /// Crear elementos Raiz del Documento en Version 3.0
        /// </summary>
        /// <param name="pxDocumento">Documento</param>
        /// <param name="psElemento">Elemento</param>
        /// <param name="pasAtributos">Atributos</param>
        /// <returns></returns>
        private XmlElement fnCrearElemento(XmlDocument pxDocumento, string psElemento, string[] pasAtributos)
        {
            XmlAttribute xAttr;
            XmlElement elemento = pxDocumento.CreateElement(psElemento);
            if (pasAtributos != null)
            {
                foreach (string a in pasAtributos)
                {
                    string[] valores = a.Split('@');
                    if (!valores[0].Equals("CorreoElectronicoReceptor"))
                    {
                        xAttr = pxDocumento.CreateAttribute(valores[0]);
                        xAttr.Value = valores[1];
                    }
                    else
                    {
                        xAttr = pxDocumento.CreateAttribute(valores[0]);
                        xAttr.Value = a.Remove(0, 26);
                    }
                    elemento.Attributes.Append(xAttr);
                }
            }
            return elemento;
        }

        //<summary>
        //Crear elementos Raiz del Documento en Version 3.2
        //</summary>
        //<param name="pxDocumento">Documento</param>
        //<param name="pasAtributos">Atributos</param>
        private void fnCrearElementoRoot32(XmlDocument pxDocumento)
        {
            XmlAttribute xAttr;
        //    foreach (string a in pasAtributos)
        //    {
        //        string[] valores = a.Split('@');
        //        xAttr = pxDocumento.CreateAttribute(valores[0]);
        //        xAttr.Value = valores[1];
        //        pxDocumento.DocumentElement.Attributes.Append(xAttr);
        //    }
            xAttr = pxDocumento.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
            pxDocumento.DocumentElement.Attributes.Append(xAttr);
        }

        /// <summary>
        /// Crea archivo pdf segun plantilla configurada para su posterior envio de correo
        /// </summary>
        /// <param name="pxComprobante"></param>
        /// <param name="psTipoDocumento"></param>
        /// <param name="sRuta"></param>
        //public void fnCrearPlantillaEnvio(XmlDocument pxComprobante, string psTipoDocumento, string sRuta)
        //{
        //    if (!(sRuta == string.Empty))
        //    {
        //        clsPlantillaMyers pdf = new clsPlantillaMyers(pxComprobante);

        //        if (!string.IsNullOrEmpty(psTipoDocumento))
        //            pdf.TipoDocumento = psTipoDocumento.ToUpper();
        //        pdf.fnGenerarPDFSave(sRuta, "Black");
        //    }
        //}

        /// <summary>
        /// Función que genera las llaves para la generación del sello
        /// </summary>
        private void fnGenerarLlave()
        {
            //Obtener la Llave Privada del Emisor
            string[] FileKey = null;
            string RutaKey = (String)Settings.Default.rutaCertificados + "\\";
            string filtroKey = "*.key";
            FileKey = Directory.GetFiles(RutaKey, filtroKey);
            foreach (string filekey in FileKey)
            {
                Stream streamkey = File.Open(filekey.ToString(), FileMode.Open);
                StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);
                using (BinaryReader br = new BinaryReader(streamkey))
                {
                    gLlave = br.ReadBytes(Convert.ToInt32(streamkey.Length));
                }
            }
            //Obtener la Llave Privada del Emisor

            //Obtener el Password del Certificado Emisor
            string[] FilePwd = null;
            string RutaPwd = (String)Settings.Default.rutaCertificados + "\\";
            string filtroPwd = "*.txt";
            FilePwd = Directory.GetFiles(RutaPwd, filtroPwd);

            foreach (string filePwd in FilePwd)
            {
                using (Stream streamPwd = File.Open(filePwd.ToString(), FileMode.Open))
                {
                    StreamReader srPwd = new StreamReader(streamPwd, System.Text.Encoding.UTF8, true);
                    gsPassword = srPwd.ReadToEnd();
                }
            }
            //Obtener el Password del Certificado Emisor

            //Obtener el Certificado del Emisor
            string[] FilesCer = null;
            string RutaCert = (String)Settings.Default.rutaCertificados + "\\";
            string filtroCert = "*.cer";
            FilesCer = Directory.GetFiles(RutaCert, filtroCert);

            foreach (string filecer in FilesCer)
            {
                certEmisor.Import(filecer);
            }
            //Obtener el Certificado del Emisor
            //Llave del Emisor
            //*****************************
            //Se cambiaria el metodo de sellado por OpenSSL
            //cSello = new cSello(FileKey[0], FilePwd[0], Settings.Default.rutaCertificados + @"\");
            //
            //*****************************
            //Llave del Emisor
        }

        /// <summary>
        /// Función que llena las clases del esquema para el xml
        /// </summary>
        /// <param name="sLayout">Layout</param>
        /// <returns></returns>
        // private object fnLlenarClase(string sclase, string[] satributos)
        //private XmlElement fnLlenarClase(XmlDocument pxDocumento, string psElemento, string sclase, string[] satributos)
        //{
        private object fnLlenarClase(string sclase, string[] satributos)
        {
            //XmlAttribute xAttr;
            //XmlElement elemento = null;
            string sfecha = string.Empty;
            DateTime dfecha = new DateTime();
            Type tipo = Type.GetType(sclase, true);
            clase = Activator.CreateInstance(tipo);
            Type t = clase.GetType();
            PropertyInfo[] propiedades = t.GetProperties();

            //if (!psElemento.Equals(""))
               //elemento = pxDocumento.CreateElement("cfdi", psElemento, "http://www.sat.gob.mx/cfd/3");

            try
            {
                foreach (string a in satributos)
                {
                    string[] valores = a.Split('@');
                    string atributo = valores[0];
                    for (int i = 0; i < propiedades.Length; i++)
                    {
                        if (propiedades[i].Name.ToString().Equals(atributo))
                        {
                            if (!propiedades[i].PropertyType.IsEnum)
                            {
                                propiedades[i].SetValue(clase, Convert.ChangeType(valores[1], propiedades[i].PropertyType), null);

                                if (propiedades[i + 1].PropertyType.Name == "Boolean")
                                    propiedades[i + 1].SetValue(clase, Convert.ChangeType(true, propiedades[i + 1].PropertyType), null);

                            }
                            else
                            {
                                switch (propiedades[i].PropertyType.Name.ToString())
                                {
                                    case "ComprobanteTipoDeComprobante":
                                        switch (valores[1])
                                        {
                                            case "ingreso":
                                                propiedades[i].SetValue(clase, ComprobanteTipoDeComprobante.ingreso, null);
                                                break;
                                            case "egreso":
                                                propiedades[i].SetValue(clase, ComprobanteTipoDeComprobante.egreso, null);
                                                break;
                                            case "traslado":
                                                propiedades[i].SetValue(clase, ComprobanteTipoDeComprobante.traslado, null);
                                                break;
                                        }
                                        break;
                                    case "ComprobanteImpuestosTrasladoImpuesto":
                                        switch (valores[1])
                                        {
                                            case "IVA":
                                                propiedades[i].SetValue(clase, ComprobanteImpuestosTrasladoImpuesto.IVA, null);
                                                break;
                                            case "IEPS":
                                                propiedades[i].SetValue(clase, ComprobanteImpuestosTrasladoImpuesto.IEPS, null);
                                                break;
                                        }
                                        break;
                                    case "ComprobanteImpuestosRetencionImpuesto":
                                        switch (valores[1])
                                        {
                                            case "ISR":
                                                propiedades[i].SetValue(clase, ComprobanteImpuestosRetencionImpuesto.ISR, null);
                                                break;
                                            case "IVA":
                                                propiedades[i].SetValue(clase, ComprobanteImpuestosRetencionImpuesto.IVA, null);
                                                break;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                    clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "Archivo: " + ex.Message);
                else
                    clsLog.Escribir(Settings.Default.LogError + "LogError" + dfecha.Day + "-" + dfecha.Month + "-" + dfecha.Year, DateTime.Now + " " + "Archivo: " + ex.Message);

                //return string.Empty;
            }
            //return elemento;
            return clase;
        }

        //private void fnCrearElementoRoot32(XmlDocument pxDocumento, object oClase)
        //{
        //    XmlAttribute xAttr;
        //    Type t = oClase.GetType();
        //    PropertyInfo[] propiedades = t.GetProperties();
        //    string sfecha = string.Empty;
        //    DateTime dfecha = new DateTime();
        //    try
        //    {
        //        foreach (PropertyInfo p in propiedades)
        //        {
        //            if (!Convert.ToString(p.GetValue(oClase, null)).Equals("") || p.Name.ToString() == "certificado" || p.Name.ToString() == "sello" && !p.PropertyType.Name.ToString().Equals("Boolean"))
        //            {
        //                xAttr = pxDocumento.CreateAttribute(p.Name);
        //                if (!p.Name.ToString().Equals("fecha"))
        //                {
        //                    xAttr.Value = Convert.ToString(p.GetValue(oClase, null));
        //                }
        //                else
        //                {
        //                    sfecha = Convert.ToString(p.GetValue(oClase, null));
        //                    dfecha = Convert.ToDateTime(sfecha);
        //                    xAttr.Value = dfecha.ToString("s");
        //                }
        //                pxDocumento.DocumentElement.Attributes.Append(xAttr);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Write(ex.ToString());
        //    }
        //    xAttr = pxDocumento.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
        //    xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
        //    pxDocumento.DocumentElement.Attributes.Append(xAttr);
        //}

        //private XmlElement fnCrearElemento(XmlDocument pxDocumento, string psElemento, object oClase)
        //{
        //    XmlAttribute xAttr;
        //    XmlElement elemento = pxDocumento.CreateElement("cfdi", psElemento, "http://www.sat.gob.mx/cfd/3");
        //    Type t = oClase.GetType();
        //    PropertyInfo[] propiedades = t.GetProperties();
        //    try
        //    {
        //        foreach (PropertyInfo p in propiedades)
        //        {
        //            if (!Convert.ToString(p.GetValue(oClase, null)).Equals(string.Empty) || Convert.ToString(p.GetValue(oClase, null)) != null)
        //            {
        //                xAttr = pxDocumento.CreateAttribute(p.Name);
        //                xAttr.Value = Convert.ToString(p.GetValue(oClase, null));
        //                elemento.Attributes.Append(xAttr);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Write(ex.ToString());
        //    }
        //    return elemento;
        //}

        /// <summary>
        /// Función que genera el comprobante
        /// </summary>
        /// <param name="sLayout">Layout</param>
        /// <returns></returns>
        /// 
        private string fnGenerarComprobante(string sLayout, string sNombreLayout)
        {
            int nBandera = 0;
            DateTime Fecha = DateTime.Today;
            string sCadenaOriginalEmisor = String.Empty;
            string linea = string.Empty;
            string lineaVersion = string.Empty;
            string noCertificado = string.Empty;
            string numeroCertificado = string.Empty;
            string sSello = string.Empty;
            string letra = "T";
            string[] atributos = null;
            string[] atributosAduana = null;
            string[] seccion = null;
            string[] atributosVersionSeccion1 = null;
            string[] seccionVersion = null;
            StringReader lector;
            System.IO.StringReader lectorVersion;
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            XPathNavigator navNodoTimbre;
            XmlDocument xDocumento = new XmlDocument();
            //Este es un comentario añadido para ver el funcionamiento del team foundation
            //uno mas
            List<string[]> listaAddenda = new List<string[]>();
            XmlSerializerNamespaces nsComprobante = new XmlSerializerNamespaces();
            int r = 0;
            int c = 0;
            int t = 0;
            int re = 0;

            string comp = string.Empty;
            //t_InformacionAduanera atributosAduana = new t_InformacionAduanera();

            try
            {
                lectorVersion = new System.IO.StringReader(sLayout);
                while (true)
                {
                    lineaVersion = lectorVersion.ReadLine();
                    if (string.IsNullOrEmpty(lineaVersion))
                        break;

                    seccionVersion = lineaVersion.Split('?');

                    try
                    {

                        atributosVersionSeccion1 = seccionVersion[1].Split('|');

                        switch (seccionVersion[0])
                        {
                            case "co":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("noCertificado"))
                                    {
                                        string[] snoCert = arreglo.Split('@');
                                        noCertificado = snoCert[1];
                                    }
                                    if (arreglo.Contains("serie"))
                                    {
                                        string[] sSerie = arreglo.Split('@');
                                        gsSerie = sSerie[1];
                                    }
                                    if (arreglo.Contains("folio"))
                                    {
                                        string[] sFolio = arreglo.Split('@');
                                        gsFolio = sFolio[1];
                                    }
                                }

                                break;
                            case "rf":
                                r++;
                                break;
                            case "cc":
                                c++;
                                break;
                            case "ir":
                                re++;
                                break;
                            case "it":
                                t++;
                                break;
                        }
                    }
                    catch
                    {
                        nBandera = 1;
                        return string.Empty;
                    }
                }
                lectorVersion.Close();

                lector = new System.IO.StringReader(sLayout);

                nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsm.AddNamespace("fomadd", "http://www.ford.com.mx/cfdi/addenda");
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                xDocumento = new XmlDocument(nsm.NameTable);
                xDocumento.CreateXmlDeclaration("1.0", "UTF-8", "no");
                xDocumento.AppendChild(xDocumento.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));

                ComprobanteEmisorRegimenFiscal[] cComprobanteRegimen = new ComprobanteEmisorRegimenFiscal[r];
                r = 0;
                ComprobanteConcepto[] cComprobanteConcepto = new ComprobanteConcepto[c];
                c = 0;
                ComprobanteImpuestosRetencion[] cComprobanteRetencion = new ComprobanteImpuestosRetencion[re];
                re = 0;
                ComprobanteImpuestosTraslado[] cComprobanteTraslado = new ComprobanteImpuestosTraslado[t];
                t = 0;

                while (true)
                {
                    linea = lector.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                        break;

                    seccion = linea.Split('?');

                    try
                    {
                        atributos = seccion[1].Split('|');

                        //Revisa datos aduanales
                        if (atributos[0].Contains("cantidad"))
                        {
                            atributosAduana = null;
                            if (seccion.Length > 2)
                            {
                                atributosAduana = seccion[2].Split('|');
                                //atributosAduana.aduana = seccion[2].Split('|');
                            }
                        }

                        //if (seccion[0] == "im" || seccion[0] == "it" || seccion[0] == "ir")
                        //{
                        //    if (impuestos == null)
                        //    {
                        //        if (seccion[0] == "im")
                        //            impuestos = xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Impuestos", atributos));
                        //        else
                        //            impuestos = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3"));

                        //    }
                        //}

                        switch (seccion[0])
                        {
                            case "co":
                                //fnCrearElementoRoot32(xDocumento, atributos);
                                //fnLlenarClase(xDocumento, "", "Comprobante", atributos);

                                comprobante = (Comprobante)fnLlenarClase("Comprobante", atributos);
                                if (!comprobante.descuento.Equals(0))
                                {
                                    comprobante.descuentoSpecified = true;
                                }
                                //fnCrearElementoRoot32(xDocumento, fnLlenarClase("Comprobante", atributos));
                                break;
                            case "re":
                                //xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Emisor", atributos));

                                //xDocumento.DocumentElement.AppendChild(fnLlenarClase(xDocumento, "Emisor", "ComprobanteEmisor", atributos));
                                //xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Emisor", fnLlenarClase("ComprobanteEmisor", atributos)));
                                comprobante.Emisor = (ComprobanteEmisor)fnLlenarClase("ComprobanteEmisor", atributos);
                                break;
                            case "de":
                                //padre.AppendChild(fnCrearElemento(xDocumento, "DomicilioFiscal", atributos));

                                //padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                //padre.AppendChild(fnLlenarClase(xDocumento, "DomicilioFiscal", "t_UbicacionFiscal", atributos));
                                //padre.AppendChild(fnCrearElemento(xDocumento, "DomicilioFiscal", fnLlenarClase("t_UbicacionFiscal", atributos)));
                                comprobante.Emisor.DomicilioFiscal = (t_UbicacionFiscal)fnLlenarClase("t_UbicacionFiscal", atributos);
                                break;
                            case "ee":
                                //padre.AppendChild(fnCrearElemento(xDocumento, "ExpedidoEn", atributos));

                                //padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                //padre.AppendChild(fnLlenarClase(xDocumento, "ExpedidoEn", "t_Ubicacion", atributos));
                                //padre.AppendChild(fnCrearElemento(xDocumento, "ExpedidoEn", fnLlenarClase("t_Ubicacion", atributos)));
                                comprobante.Emisor.ExpedidoEn = (t_Ubicacion)fnLlenarClase("t_Ubicacion", atributos);
                                break;
                            case "rf":
                                //padre.AppendChild(fnCrearElemento(xDocumento, "RegimenFiscal", atributos));

                                //padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                //padre.AppendChild(fnLlenarClase(xDocumento, "RegimenFiscal", "ComprobanteEmisorRegimenFiscal", atributos));
                                //padre.AppendChild(fnCrearElemento(xDocumento, "RegimenFiscal", fnLlenarClase("ComprobanteEmisorRegimenFiscal", atributos)));
                                cComprobanteRegimen[r] =(ComprobanteEmisorRegimenFiscal)fnLlenarClase("ComprobanteEmisorRegimenFiscal", atributos);
                                comprobante.Emisor.RegimenFiscal = cComprobanteRegimen;
                                r++;
                                break;
                            case "rr":
                                //xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Receptor", atributos));

                                //xDocumento.DocumentElement.AppendChild(fnLlenarClase(xDocumento, "Receptor", "ComprobanteReceptor", atributos));
                                //xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Receptor", fnLlenarClase("ComprobanteReceptor", atributos)));
                                comprobante.Receptor = (ComprobanteReceptor)fnLlenarClase("ComprobanteReceptor", atributos);
                                break;
                            case "dr":
                                //padre.AppendChild(fnCrearElemento(xDocumento, "Domicilio", atributos));

                                //padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor", nsm);
                                //padre.AppendChild(fnLlenarClase(xDocumento, "Domicilio", "t_Ubicacion", atributos));
                                //padre.AppendChild(fnCrearElemento(xDocumento, "Domicilio", fnLlenarClase("t_Ubicacion", atributos)));
                                comprobante.Receptor.Domicilio = (t_Ubicacion)fnLlenarClase("t_Ubicacion", atributos);
                                break;
                            case "cc":
                                cComprobanteConcepto[c] =(ComprobanteConcepto)fnLlenarClase("ComprobanteConcepto", atributos);
                                comprobante.Conceptos = cComprobanteConcepto;
                                //padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);
                                //if (padre == null)
                                //{
                                //    padre = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3"));

                                //    if (atributosAduana != null)
                                //    {
                                //        //padreConcepto = xDocumento.DocumentElement.LastChild.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));

                                //        //padreConcepto = xDocumento.DocumentElement.LastChild.AppendChild(fnLlenarClase(xDocumento, "Concepto", "ComprobanteConcepto", atributos));
                                //        padreConcepto = xDocumento.DocumentElement.LastChild.AppendChild(fnCrearElemento(xDocumento, "Concepto", fnLlenarClase("ComprobanteConcepto", atributos)));
                                //        padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsm);
                                //        padreConcepto.AppendChild(fnCrearElemento(xDocumento, "InformacionAduanera", atributosAduana));
                                //    }
                                //    else
                                //    {
                                //        //padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));

                                //        //padre.AppendChild(fnLlenarClase(xDocumento, "Concepto", "ComprobanteConcepto", atributos));
                                //        padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", fnLlenarClase("ComprobanteConcepto", atributos)));
                                //    }
                                //}
                                //else
                                //{
                                //    if (atributosAduana != null)
                                //    {
                                //        //padreConcepto = xDocumento.DocumentElement.LastChild.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));

                                //        //padreConcepto = xDocumento.DocumentElement.LastChild.AppendChild(fnLlenarClase(xDocumento, "Concepto", "ComprobanteConcepto", atributos));
                                //        padreConcepto = xDocumento.DocumentElement.LastChild.AppendChild(fnCrearElemento(xDocumento, "Concepto", fnLlenarClase("ComprobanteConcepto", atributos)));
                                //        padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsm);
                                //        padreConcepto.AppendChild(fnCrearElemento(xDocumento, "InformacionAduanera", atributosAduana));
                                //    }
                                //    else
                                //    {
                                //        //padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));

                                //        //padre.AppendChild(fnLlenarClase(xDocumento, "Concepto", "ComprobanteConcepto", atributos));
                                //        padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", fnLlenarClase("ComprobanteConcepto", atributos)));
                                //    }
                                //}
                                c++;
                                break;
                            case "im":
                                //xDocumento.DocumentElement.AppendChild(fnLlenarClase(xDocumento, "Impuestos", "ComprobanteImpuestos", atributos));
                                //xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Impuestos", fnLlenarClase("ComprobanteImpuestos", atributos)));
                                comprobante.Impuestos = (ComprobanteImpuestos)fnLlenarClase("ComprobanteImpuestos", atributos);
                                if (!comprobante.Impuestos.totalImpuestosTrasladados.Equals(0))
                                    comprobante.Impuestos.totalImpuestosTrasladadosSpecified = true;

                                if (!comprobante.Impuestos.totalImpuestosRetenidos.Equals(0))
                                    comprobante.Impuestos.totalImpuestosRetenidosSpecified = true;
                                break;
                            case "ir":
                                cComprobanteRetencion[re] =(ComprobanteImpuestosRetencion)fnLlenarClase("ComprobanteImpuestosRetencion", atributos) ;
                                comprobante.Impuestos.Retenciones = cComprobanteRetencion;
                                //padreimpuestos = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos", nsm);
                                //if (padreimpuestos == null)
                                //padre = padreimpuestos.AppendChild(xDocumento.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3"));
                                //padre.AppendChild(fnLlenarClase(xDocumento, "Retencion", "ComprobanteImpuestos", atributos));
                                //padre.AppendChild(fnCrearElemento(xDocumento, "Retencion", fnLlenarClase("ComprobanteImpuestos", atributos)));
                                /////////////////////////////////////////////////////////
                                //padre = impuestos.SelectSingleNode("cfdi:Retenciones", nsm);
                                //if (padre == null)
                                //    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3"));
                                ////padre.AppendChild(fnCrearElemento(xDocumento, "Retencion", atributos));

                                //padre.AppendChild(fnLlenarClase(xDocumento, "Retencion",  "ComprobanteImpuestos", atributos));
                                ////padre.AppendChild(fnCrearElemento(xDocumento, "Retencion", fnLlenarClase("ComprobanteImpuestos", atributos)));
                                //fnIncrementarTotalImpuestosRetenidos(xDocumento, padreimpuestos, atributos);
                                /////////////////////////////////////////////////////////
                                if (comprobante.Impuestos.totalImpuestosRetenidos.Equals(0))
                                    comprobante.Impuestos.totalImpuestosRetenidos = cComprobanteRetencion[re].importe;
                                else
                                    comprobante.Impuestos.totalImpuestosRetenidos = comprobante.Impuestos.totalImpuestosRetenidos + cComprobanteRetencion[re].importe;
                                re++;
                                break;
                            case "it":
                                 cComprobanteTraslado[t] =  (ComprobanteImpuestosTraslado)fnLlenarClase("ComprobanteImpuestosTraslado", atributos);
                                comprobante.Impuestos.Traslados = cComprobanteTraslado;
                                //padreimpuestos = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos", nsm);
                                //if (padreimpuestos == null)
                                //padre = padreimpuestos.AppendChild(xDocumento.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3"));
                                //padre.AppendChild(fnLlenarClase(xDocumento, "Traslado", "ComprobanteImpuestosTraslado", atributos));
                                //padre.AppendChild(fnCrearElemento(xDocumento, "Traslado", fnLlenarClase("ComprobanteImpuestosTraslado", atributos)));
                                /////////////////////////////////////////////////////////
                                //padre = impuestos.SelectSingleNode("cfdi:Traslados", nsm);
                                //if (padre == null)
                                //    padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3"));
                                ////padre.AppendChild(fnCrearElemento(xDocumento, "Traslado", atributos));

                                //padre.AppendChild(fnLlenarClase(xDocumento, "Traslado", "ComprobanteImpuestosTraslado", atributos));    
                                ////padre.AppendChild(fnCrearElemento(xDocumento, "Traslado", fnLlenarClase("ComprobanteImpuestosTraslado", atributos)));    
                                //fnIncrementarTotalImpuestosTrasladados(xDocumento, padreimpuestos, atributos);
                                /////////////////////////////////////////////////////////
                                if (comprobante.Impuestos.totalImpuestosTrasladados.Equals(0))
                                    comprobante.Impuestos.totalImpuestosRetenidos = cComprobanteRetencion[re].importe;
                                else
                                    comprobante.Impuestos.totalImpuestosTrasladados = comprobante.Impuestos.totalImpuestosTrasladados + cComprobanteTraslado[t].importe;
                                t++;
                                break;
                            //case "ad":
                            //    listaAddenda.Add(atributos);
                            //    break;
                        }
                        //Debug.Write(xDocumento.OuterXml);
                    }
                    catch (Exception ex)
                    {
                        nBandera = 1;
                        //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + sNombreLayout + " " + "El archivo de texto esta mal formado" + ex.Message);
                        if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                            clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + sNombreLayout + " " + "El archivo de texto esta mal formado." + " " + ex.Message);
                        else
                            clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + sNombreLayout + " " + "El archivo de texto esta mal formado." + " " + ex.Message);

                    }
                }

                lector.Close();
                //Termina el ciclo para generar el XML

                if (!nBandera.Equals(0))
                {
                    return string.Empty;
                }

                //Revisa el modo de ejecución
                if (letra == "P")
                {
                    //Valida certificado sea vigente
                    if (!fnComprobarFechas())
                    {
                        //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "El certificado está fuera de fecha");
                        if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                            clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "El certificado está fuera de fecha");
                        else
                            clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "El certificado está fuera de fecha");

                        return string.Empty;
                    }

                    //Valida certificado
                    if (!fnCSD308())
                    {
                        //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "308 - Certificado no expedido por el SAT");
                        if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                            clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "308 - Certificado no expedido por el SAT");
                        else
                            clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "308 - Certificado no expedido por el SAT");

                        return string.Empty;
                    }
                }

                //Cerificado para agregar al XML

                string sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());
                //Numero del certificado
                byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
                numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();

                // if (!noCertificado.Equals(numeroCertificado))
                if (!noCertificado.Equals(numeroCertificado))
                {
                    //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "El documento " + sNombreLayout + " no contiene o es incorrecto el número de certificado");
                    if (!gsSerie.Equals(string.Empty) && !gsSerie.Equals(string.Empty))
                        clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "El documento " + sNombreLayout + " no contiene o es incorrecto el número de certificado");
                    else
                        clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "El documento " + sNombreLayout + " no contiene o es incorrecto el número de certificado");

                    return string.Empty;
                }
                object cComprobante = (object)comprobante;
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(cComprobante.GetType());
                    xmlSerializer.Serialize(xmlStream, comprobante);
                    xmlStream.Position = 0;
                    xDocumento.Load(xmlStream);

                }

                string scadena = "cadenaoriginal_3_2";
                navNodoTimbre = xDocumento.CreateNavigator();
                sCadenaOriginalEmisor = fnConstruirCadenaTimbrado(navNodoTimbre, scadena);
                //cSello.sCadenaOriginal = sCadenaOriginalEmisor;
                //sSello = cSello.sSello;
                sSello = fnCrearSello(sCadenaOriginalEmisor);
                //sSello = fnGenerarSello(sCadenaOriginalEmisor);

                //Valida sello
                if (!fnVerificarSello(sCadenaOriginalEmisor, sSello))
                {
                    //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + "Sello incorrecto");
                    if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                        clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "Sello incorrecto");
                    else
                        clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "Sello incorrecto");

                    return string.Empty;
                }

                if (nBandera == 0)
                {
                    //Asignar los valores de certificado,numero de certificado y sello.
                    //nsmComprobante = new XmlNamespaceManager(xDocumento.NameTable);
                    //nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    nsComprobante.Add("cfdi", "http://www.sat.gob.mx/cfd/3");
                    //xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).SetValue(numeroCertificado);
                    //xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);
                    //xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sSello);
                    comprobante.certificado = sCertificado;
                    comprobante.sello = sSello;
                }

                object cNuevoComprobante = (object)comprobante;
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(cNuevoComprobante.GetType());
                    xmlSerializer.Serialize(xmlStream, comprobante, nsComprobante);
                    xmlStream.Position = 0;
                    xDocumento.Load(xmlStream);

                }

                fnCrearElementoRoot32(xDocumento);

                xsd_validacion = string.Empty;
                xsd_error_code = string.Empty;
                xsd_validacion = fnValidate(xDocumento, 0);//"esquema_v3");
                if (xsd_validacion != string.Empty && xsd_validacion != null)
                {
                    throw new System.ArgumentException("333 - " + xsd_validacion, "valida esquema.");
                }

                //if (!fnValidarEsquema(xDocumento))
                //{
                //    clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " " + sNombreLayout + " " + "Archivo no Cumple Con Esquema " + xsd_validacion);
                //    return string.Empty;<
                //}

            }
            catch (Exception ex)
            {
                if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
                    clsLog.Escribir(Settings.Default.LogError + gsSerie + "-" + gsFolio, DateTime.Now + " " + "Archivo: " + sNombreLayout + " " + ex.Message);
                else
                    clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, DateTime.Now + " " + "Archivo: " + sNombreLayout + " " + ex.Message);

                return string.Empty;
            }



            //fnAgregarAddenda(ref xDocumento, sLayout, ref nsm);

            return xDocumento.OuterXml;
        }

        /// <summary>
        /// Procedimiento que Genera el Sello del Emisor
        /// </summary>
        /// <param name="sCadena">Cadena Original</param>
        /// <returns></returns>
        public string fnCrearSello(string sCadena)
        {
            string[] keyFile;
            string filtroKey = "*.key";

            keyFile = Directory.GetFiles(Settings.Default.rutaCertificados, filtroKey);

            var rutaCadena = Settings.Default.rutaCertificados + DateTime.Now.ToString("yyMMddhhmmss") + ".txt";
            System.IO.File.WriteAllText(rutaCadena, sCadena);

            var rutaOpenssl = Settings.Default.rutaOpenssl + "openssl.exe";

            var rutaPem = Settings.Default.rutaCertificados + DateTime.Now.ToString("yyMMddhhmmss") + ".pem";
            Process archivoPem = new Process();
            archivoPem.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            archivoPem.StartInfo.FileName = rutaOpenssl;
            archivoPem.StartInfo.Arguments = "pkcs8 -inform DER -in " + keyFile[0] + " -passin pass:" + gsPassword + " -out " + rutaPem;
            archivoPem.StartInfo.UseShellExecute = false;
            archivoPem.StartInfo.ErrorDialog = false;
            archivoPem.StartInfo.RedirectStandardOutput = true;
            archivoPem.Start();
            archivoPem.WaitForExit();

            Process generaSello = new Process();
            generaSello.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            generaSello.StartInfo.FileName = rutaOpenssl;
            generaSello.StartInfo.Arguments = "dgst -sha1 -sign " + rutaPem + " " + rutaCadena;
            generaSello.StartInfo.UseShellExecute = false;
            generaSello.StartInfo.ErrorDialog = false;
            generaSello.StartInfo.RedirectStandardOutput = true;
            generaSello.Start();

            String selloTxt = generaSello.StandardOutput.ReadToEnd();
            String sSello = Convert.ToBase64String(Encoding.Default.GetBytes(selloTxt));
            generaSello.WaitForExit();

            File.Delete(rutaCadena);
            File.Delete(rutaPem);

            return sSello;
        }  

        /// <summary>
        /// Procedimiento que Genera el Sello del Emisor
        /// </summary>
        /// <param name="psCadenaOriginal">Cadena Original</param>
        /// <returns></returns>
        public static string fnGenerarSello(string psCadenaOriginal)
        {
            string sello = string.Empty;
            try
            {
                //sello = rsa.SignStringENC(psCadenaOriginal, sAlgoritmo);


                //clsLog.Escribir(Settings.Default.LogError + "conteo", "0");
                //StreamReader sr = new StreamReader(Settings.Default.LogError + @"\conteo.txt");
                //int line = Convert.ToInt16(sr.ReadToEnd()) + 1;
                //sr.Close();
                //clsLog.Escribir(Settings.Default.LogError + "conteo", line.ToString());
            }
            catch (Exception)
            {
                return null;
            }
            return sello;
        }

        /// <summary>
        /// Método que se encarga de incrementar o sumar el importe de los impuestos de traslado en el atributo de totalImpuestosTrasladados
        /// </summary>
        /// <param name="pxDocumento">Comprobante</param>
        /// <param name="nodoImpuestos">Nodo de impuestos</param>
        /// <param name="paAtributos">Atributos de impuesto de traslado</param>
        private void fnIncrementarTotalImpuestosTrasladados(XmlDocument pxDocumento, XmlNode nodoImpuestos, string[] paAtributos)
        {
            XmlNamespaceManager nsmComprobanteImpuestos;
            nsmComprobanteImpuestos = new XmlNamespaceManager(pxDocumento.NameTable);
            nsmComprobanteImpuestos.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            string sTotalImpuestosTrasladados = string.Empty;
            try { sTotalImpuestosTrasladados = nodoImpuestos.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosTrasladados", nsmComprobanteImpuestos).Value; }
            catch { }

            XmlAttribute xAttr;
            foreach (string a in paAtributos)
            {
                string[] valores = a.Split('@');
                if (valores[0].Equals("importe"))
                {
                    if (sTotalImpuestosTrasladados.Equals(string.Empty))
                    {
                        xAttr = pxDocumento.CreateAttribute("totalImpuestosTrasladados");
                        xAttr.Value = valores[1];
                        nodoImpuestos.Attributes.Append(xAttr);
                    }
                    else
                    {
                        CultureInfo languaje = new CultureInfo("es-Mx");
                        double dTotalImpuestosTransladados = Convert.ToDouble(sTotalImpuestosTrasladados, languaje) + Convert.ToDouble(valores[1], languaje);
                        nodoImpuestos.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosTrasladados", nsmComprobanteImpuestos).SetValue(dTotalImpuestosTransladados.ToString("F6", languaje));
                    }
                }
            }
        }

        /// <summary>
        /// Método que se encarga de incrementar o sumar el importe de los impuestos retenidos en el atributo de totalImpuestosRetenidos
        /// </summary>
        /// <param name="pxDocumento">Comprobante</param>
        /// <param name="nodoImpuestos">Nodo de impuestos</param>
        /// <param name="paAtributos">Atributos de impuesto de retencion</param>
        private void fnIncrementarTotalImpuestosRetenidos(XmlDocument pxDocumento, XmlNode nodoImpuestos, string[] paAtributos)
        {
            XmlNamespaceManager nsmComprobanteImpuestos;
            nsmComprobanteImpuestos = new XmlNamespaceManager(pxDocumento.NameTable);
            nsmComprobanteImpuestos.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            string sTotalImpuestosTrasladados = string.Empty;
            try { sTotalImpuestosTrasladados = pxDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosRetenidos", nsmComprobanteImpuestos).Value; }
            catch { }

            XmlAttribute xAttr;
            foreach (string a in paAtributos)
            {
                string[] valores = a.Split('@');
                if (valores[0].Equals("importe"))
                {
                    if (sTotalImpuestosTrasladados.Equals(string.Empty))
                    {
                        xAttr = pxDocumento.CreateAttribute("totalImpuestosRetenidos");
                        xAttr.Value = valores[1];
                        nodoImpuestos.Attributes.Append(xAttr);
                    }
                    else
                    {
                        CultureInfo languaje = new CultureInfo("es-Mx");
                        double dTotalImpuestosTransladados = Convert.ToDouble(sTotalImpuestosTrasladados, languaje) + Convert.ToDouble(valores[1], languaje);
                        nodoImpuestos.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosRetenidos", nsmComprobanteImpuestos).SetValue(dTotalImpuestosTransladados.ToString("F6", languaje));
                    }
                }
            }
        }


        /// <summary>
        /// Delegado para la validación del xml con el esquema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SchemaValidationHandler(object sender, ValidationEventArgs e)
        {

            switch (e.Severity)
            {
                case XmlSeverityType.Error:

                    if (!e.Message.Contains("TimbreFiscalDigital"))
                    {
                        xsd_error_code = xsd_error_code + e.Message;
                    }

                    break;
            }
        }

        /// <summary>
        /// Función que valida el comprobante contra el esquema
        /// </summary>
        /// <param name="psXmlDocument">Comprobante</param>
        /// <returns></returns>
        public string fnValidate(XmlDocument psXmlDocument, int sel)
        {
            string retorno = string.Empty;
            try
            {
                XmlTextReader tr;
                foreach (DataRow row in tblComplementos.Rows)
                {

                    if (sel == 0)
                    {
                        if (row["esquema"].ToString() == "http://www.sat.gob.mx/cfd/3")
                        {
                            tr = new XmlTextReader(new System.IO.StringReader(row["Valor"].ToString()));
                            psXmlDocument.Schemas.Add(row["esquema"].ToString(), tr);
                        }

                    }
                    else
                    {
                        if (row["esquema"].ToString() == "http://www.ford.com.mx/cfdi/addenda")
                        {
                            tr = new XmlTextReader(new System.IO.StringReader(row["Valor"].ToString()));
                            psXmlDocument.Schemas.Add(row["esquema"].ToString(), tr);
                        }
                    }
                }

                ValidationEventHandler validation = new ValidationEventHandler(SchemaValidationHandler);
                psXmlDocument.Validate(validation);

                if (xsd_error_code != string.Empty)
                {
                    retorno = xsd_error_code;
                }
            }
            catch (Exception)
            {
                return "Revise el esquema del XML y reintente de nuevo.";
            }
            return retorno;
        }

        /// <summary>
        /// Comprueba que el sello del comprobante refleje los datos de la cadena original
        /// </summary>
        /// <param name="psCadenaOriginal">Cadena original del comprobante</param>
        /// <returns>Booleano indicando si la cadena original corresponde al sello</returns>
        public bool fnVerificarSello(string psCadenaOriginal, string psSello)
        {
            RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certEmisor.PublicKey.Key);
            try
            {
                //Verificamos que el certificado obtenga el mismo resultado que el del sello
                byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(psCadenaOriginal));
                bool exito = publica.VerifyHash(
                        hash,
                        "sha1",
                        Convert.FromBase64String(psSello));

                return exito;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Función que espera a que el archivo pueda ser leido para ser procesado.
        /// </summary>
        /// <param name="fullPath">Dirección completa</param>
        /// <returns></returns>
        bool fnWaitForFile(string fullPath)
        {
            int numTries = 0;
            while (true)
            {
                ++numTries;
                try
                {
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                    {
                        fs.ReadByte();
                        break;
                    }
                }
                catch (Exception)
                {
                    if (numTries > 10)
                    {
                        throw new Exception("El archivo no se puede procesar porque otro proceso lo esta utilizando: " + fullPath);
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            }
            return true;
        }

        // Funcion que agrega la addenda al documento xml <param name="xXML"></param>
        //public void fnAgregarAddenda(ref XmlDocument xXML, List<string[]> listaAddenda, ref XmlNamespaceManager nsm)
        //{
        public void fnAgregarAddenda(ref XmlDocument xXML, string layout, ref XmlNamespaceManager nsm)
        {
            DateTime Fecha = DateTime.Today;
            string[] atributos = null;
            string[] seccion = null;
            string linea = string.Empty;
            XmlNode nodoRaiz = null;
            StringReader lector;
            lector = new System.IO.StringReader(layout);
            // Agregamos la addenda al XML
            xXML.DocumentElement.AppendChild(fnCrearElemento(xXML, "Addenda", null));
            while (true)
            {
                linea = lector.ReadLine();
                if (string.IsNullOrEmpty(linea))
                    break;

                seccion = linea.Split('?');

                try
                {
                    atributos = seccion[1].Split('|');

                    switch (seccion[0])
                    {
                        case "addCorreo":
                            nodoRaiz = xXML.SelectSingleNode("/cfdi:Comprobante/Addenda/Correos", nsm);
                            if (nodoRaiz == null)
                            {
                                nodoRaiz = xXML.SelectSingleNode("/cfdi:Comprobante/Addenda", nsm);
                                nodoRaiz.AppendChild(fnCrearElemento(xXML, "Correos", null));
                            }
                            nodoRaiz = xXML.SelectSingleNode("/cfdi:Comprobante/Addenda/Correos", nsm);
                            nodoRaiz.AppendChild(fnCrearElemento(xXML, "Correo", atributos));
                            break;
                        case "addInformacion":
                            nodoRaiz = xXML.SelectSingleNode("/cfdi:Comprobante/Addenda/Correos", nsm);
                            nodoRaiz.AppendChild(fnCrearElemento(xXML, "Informacion", atributos));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, ex.Message + " Error al agregar addenda al comprobante.  " + " " + DateTime.Now);
                }
            }
            //    if (listaAddenda.Count != 0)
            //{
            //    try
            //    {
            //        XmlNode nodoRaiz = xXML.SelectSingleNode("/cfdi:Comprobante/", nsm);
            //        //XmlNode nodoRaiz = xXML.DocumentElement;

            //        XmlNode nodoAddenda = xXML.CreateElement("Addenda", "http://www.sat.gob.mx/cfd/3");
            //        nodoRaiz.AppendChild(nodoAddenda);

            //        nodoRaiz = xXML.SelectSingleNode("/cfdi:Comprobante/Addenda", nsm);
            //        nodoRaiz.AppendChild(fnCrearElemento(xXML, "Correo", listaAddenda));
            //        XmlNode nodoFOMADD = xXML.CreateElement("fomadd", "addenda", "http://www.ford.com.mx/cfdi/addenda");

            //        //Agregar schemalocation//////////
            //        XmlAttribute idAttribute = xXML.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            //        idAttribute.Value = "http://www.ford.com.mx/cfdi/addenda http://www.ford.com.mx/cfdi/addenda/cfdiAddendaFord_1_0.xsd";
            //        nodoFOMADD.Attributes.Append(idAttribute);
            //        ///////////////////////////////////

            //        nodoAddenda.AppendChild(nodoFOMADD);

            //        XmlNode nodo = xXML.CreateElement("fomadd", "FOMASN", "http://www.ford.com.mx/cfdi/addenda");
            //        foreach (string[] arregloAddenda in listaAddenda)
            //        {
            //            for (int i = 0; i < arregloAddenda.Length; i++)
            //            {
            //                string[] attrValor = arregloAddenda[i].Split('@');
            //                if (attrValor[0] == "version")
            //                {
            //                    XmlAttribute verAttribute = xXML.CreateAttribute("version"); ;
            //                    verAttribute.Value = attrValor[1];
            //                    nodo.Attributes.Append(verAttribute);

            //                    nodoFOMADD.AppendChild(nodo);
            //                }
            //                else if (attrValor[0] == "GSDB")
            //                {
            //                    XmlElement GSDB = xXML.CreateElement("fomadd", "GSDB", "http://www.ford.com.mx/cfdi/addenda");

            //                    Regex regex = new Regex("^[A-Za-z0-9]{4,5}$");
            //                    Match match = regex.Match(attrValor[1]);
            //                    if (match.Success)
            //                    {
            //                        GSDB.InnerText = attrValor[1];
            //                    }
            //                    else
            //                    {
            //                        if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
            //                            clsLog.Escribir("C:/Proyectos/" + gsSerie + "-" + gsFolio, "Error: El valor ingresado no es alfanumerico o no cumple con las longitudes determinadas (GSDB) :  (" + attrValor[1] + ")");
            //                        else
            //                            clsLog.Escribir("C:/Proyectos/" + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, "Error: El valor ingresado no es alfanumerico o no cumple con las longitudes determinadas (GSDB) :  (" + attrValor[1] + ")");
            //                    }


            //                    nodo.AppendChild(GSDB);
            //                }
            //                else if (attrValor[0] == "ASN")
            //                {
            //                    XmlElement ASN = xXML.CreateElement("fomadd", "ASN", "http://www.ford.com.mx/cfdi/addenda");

            //                    Regex regex = new Regex("^[0-9]{1,11}$");
            //                    Match match = regex.Match(attrValor[1]);
            //                    if (match.Success)
            //                    {
            //                        ASN.InnerText = attrValor[1];
            //                    }
            //                    else
            //                    {
            //                        if (!gsSerie.Equals(string.Empty) && !gsFolio.Equals(string.Empty))
            //                            clsLog.Escribir("C:/Proyectos/" + gsSerie + "-" + gsFolio, "Error: El valor ingresado no es numerico o no cumple con las longitudes determinadas (ASN) :  (" + attrValor[1] + ")");
            //                        else
            //                            clsLog.Escribir("C:/Proyectos/" + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, "Error: El valor ingresado no es numerico o no cumple con las longitudes determinadas (ASN) :  (" + attrValor[1] + ")");
            //                    }

            //                    nodo.AppendChild(ASN);
            //                }
            //            }

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        clsLog.Escribir("C:/Proyectos/" + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, ex.Message + " Error al agregar addenda al comprobante.  " + " " + DateTime.Now);
            //    }
            //}
        }

        //Recupera esquemas
        public void fnRecuperarEsquemas()
        {
            DateTime Fecha = DateTime.Today;
            tblComplementos = new DataTable();
            tblComplementos.Columns.Add("Valor");
            tblComplementos.Columns.Add("esquema");

            DataRow row = tblComplementos.NewRow();
            row["Valor"] = Resources.esquema32;
            row["esquema"] = Resources.esquema_v3_2;// fnRecuperaNamespace(esquema);
            tblComplementos.Rows.Add(row);

            //DataRow row2 = tblComplementos.NewRow();

            //Buscar esquema ford//
            //string sText = "";
            //string[] Files = null;
            //string RutaEsquema = "C:/wsGenerarComprobante/XML/EsquemaFord/";
            //string filtro = "*.xsd";
            //Files = Directory.GetFiles(RutaEsquema, filtro);
            //foreach (string archivo in Files)
            //{
            //    using (Stream stream = File.Open(archivo.ToString(), FileMode.Open))
            //    {
            //        StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
            //        sText = sr.ReadToEnd();
            //        StringReader lectorVersion = new System.IO.StringReader(sText);
            //    }
            //}
            ///////////////////////
            //if (sText == "" || sText == null)
            //{
            //    clsLog.Escribir(Settings.Default.LogError + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year, "No se encontro ningun esquema en la carpeta de esquemas");
            //}
            //row2["Valor"] = sText;
            //row2["esquema"] = Resources.esquemaford;// fnRecuperaNamespace(esquema);
            //tblComplementos.Rows.Add(row2);
        }
    }
}

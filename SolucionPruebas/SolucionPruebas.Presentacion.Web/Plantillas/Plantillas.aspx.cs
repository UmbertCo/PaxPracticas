using Root.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;

namespace SolucionPruebas.Presentacion.Web
{
    public partial class Plantillas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnVerAddenda_Click(object sender, EventArgs e)
        {
            string sColor = string.Empty;
            XmlDocument document = new XmlDocument();
            try
            {
                HttpFileCollection hfc = Request.Files;
                HttpPostedFile hpf = hfc[0];

                if (hpf.ContentLength < 0)
                    return;

                document.Load(hpf.InputStream);

                //clsPlantillaFoxconn pdf = new clsPlantillaFoxconn(document);

                //pdf.TipoDocumento = "Factura";
                //pdf.fnGenerarPDF("Green");

                //pdf.fnMostrarPDF(this);

                clsPlantillaLogo pdfLogo = new clsPlantillaLogo(document);
                pdfLogo.TipoDocumento = "Factura";
                pdfLogo.fnGenerarPDF(842, 1, "Green");

                fnEnviarCorreoDocSinZIP("ismael.hidalgo@paxfacturacion.com", "Prueba", "Mensaje de prueba", document.InnerXml, pdfLogo, "ejemplo.zip", string.Empty);

                //pdfLogo.fnMostrarPDF(this);

                //XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(document.NameTable);
                //nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                //nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                //nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");

                //XPathNavigator nav = document.CreateNavigator();

                //string sTipoRiesgo = string.Empty;
                //string sBanco = string.Empty;
                //string sRegimen = string.Empty;

                //string sDescripcionTipoRiesgo = string.Empty;
                //string sDescripcionBanco = string.Empty;
                //string sDescripcionRegimen = string.Empty;

                //try { sTipoRiesgo = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@RiesgoPuesto", nsmComprobante).Value; }
                //catch { }

                //try { sBanco = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@Banco", nsmComprobante).Value; }
                //catch { }

                //try { sRegimen = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@TipoRegimen", nsmComprobante).Value; }
                //catch { }

                //if (!string.IsNullOrEmpty(sTipoRiesgo))
                //{
                //    clsOperacionRiesgoPuesto cRiesgoPuesto = new clsOperacionRiesgoPuesto();
                //    DataTable dtRiesgoPuesto = cRiesgoPuesto.fnExiste(Convert.ToInt32(sTipoRiesgo));
                //    sDescripcionTipoRiesgo = dtRiesgoPuesto.Rows[0]["Descripcion"].ToString();
                //}

                //if (!string.IsNullOrEmpty(sBanco))
                //{
                //    clsOperacionBanco cBanco = new clsOperacionBanco();
                //    DataTable dtBanco = cBanco.fnExisteBancos(Convert.ToInt32(sBanco));
                //    sDescripcionBanco = dtBanco.Rows[0]["NombreCorto"].ToString();
                //}

                //if (!string.IsNullOrEmpty(sRegimen))
                //{
                //    clsOperacionTipoRegimen cTipoRegimen = new clsOperacionTipoRegimen();
                //    DataTable dtTipoRegimen = cTipoRegimen.fnExiste(Convert.ToInt32(sRegimen));
                //    sDescripcionRegimen = dtTipoRegimen.Rows[0]["Descripcion"].ToString();
                //}


                //clsPlantillaNomina pdfGenerica = new clsPlantillaNomina(document, sTipoRiesgo, sBanco, sDescripcionRegimen);
                //pdfGenerica.TipoDocumento = "Factura";
                //pdfGenerica.fnGenerarPdf(841,556,"Green");

                //clsPlantillaLogo pdfLogo = new clsPlantillaLogo(document);
                //pdfLogo.TipoDocumento = "Factura";
                //pdfLogo.fnGenerarPDFSave(842, 556, "Green", @"C:\Prueba.pdf");

                //pdfGenerica.fnMostrarPDF(this);
            }
            catch (Exception ex)
            { 
            
            }            
        }

        public bool fnEnviarCorreoDocSinZIP(string strMailTo, string strSubject, string strMensaje, string psComprobante, clsPlantillaLogo rRutaPDF, string sNombreZip, string sCC)
        {
            bool bretorno = false;
            try
            {
                //Crear objetos para enviar correo.
                System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

                //Asignar variables de los objetos.
                //sMensaje.From = new System.Net.Mail.MailAddress(clsComun.ObtenerParamentro("emailAppFrom"));
                sMensaje.From = new System.Net.Mail.MailAddress("ismael.hidalgo@paxfacturacion.com");
                sMensaje.To.Add(strMailTo);
                sMensaje.Subject = strSubject;
                sMensaje.Body = strMensaje;
                if (sCC != string.Empty)
                    sMensaje.CC.Add(sCC);
                sMensaje.IsBodyHtml = true;

                if (!(string.IsNullOrEmpty(psComprobante)))
                {
                    XmlDocument xdComprobante = new XmlDocument();
                    xdComprobante.LoadXml(psComprobante);

                    Attachment data1 = new Attachment(ConvertToStream(xdComprobante), MediaTypeNames.Application.Pdf);
                    Attachment data2 = new Attachment(SerializeToStream(rRutaPDF), MediaTypeNames.Application.Pdf);
                    data1.Name = sNombreZip + ".xml";
                    data2.Name = sNombreZip + ".pdf";
                    sMensaje.Attachments.Add(data1);
                    sMensaje.Attachments.Add(data2);

                    //Cuenta generica para el envio de correos.
                    //smtp.Host = clsComun.ObtenerParamentro("emailHost");
                    smtp.Host = "smtp.gmail.com";
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.Credentials = new System.Net.NetworkCredential(clsComun.ObtenerParamentro("emailUser"), Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("emailPassword")));
                    smtp.Credentials = new System.Net.NetworkCredential("ismael.hidalgo@paxfacturacion.com", "aia175671");
                    //smtp.Port = Convert.ToInt32(clsComun.ObtenerParamentro("emailPort"));
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(sMensaje);
                    sMensaje.Dispose();
                    smtp.Dispose();
                    data1.Dispose();
                    data2.Dispose();
                }
                else
                {
                    //Cuenta generica para el envio de correos.
                    //smtp.Host = clsComun.ObtenerParamentro("emailHost");
                    smtp.Host = "smtp.gmail.com";
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.Credentials = new System.Net.NetworkCredential(clsComun.ObtenerParamentro("emailUser"), Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("emailPassword")));
                    smtp.Credentials = new System.Net.NetworkCredential("ismael.hidalgo@paxfacturacion.com", "aia175671");
                    //smtp.Port = Convert.ToInt32(clsComun.ObtenerParamentro("emailPort"));
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(sMensaje);
                    sMensaje.Dispose();
                    smtp.Dispose();
                }


                bretorno = true;
            }
            catch (Exception ex)
            {
                bretorno = false;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email);
            }
            return bretorno;
        }

        public static MemoryStream SerializeToStream(object objectType)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objectType);
            return stream;
        }

        private System.IO.Stream ConvertToStream(XmlDocument schemaDocument)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            schemaDocument.Save(memoryStream);

            memoryStream.Flush();

            // Rewind the Stream
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);

            return (System.IO.Stream)memoryStream;

        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Xml;
using Root.Reports;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text.RegularExpressions;

/// <summary>
/// Clase encargada del envio de documentos por correo (XML, PDF...)
/// </summary>
public class clsEnvioCorreoDocs
{
    clsPlantillaLista cpl;
    clsOperacionConsulta gDAL;
    /// <summary>
    /// Envia correo XML, PDF
    /// </summary>
    /// <param name="strMailTo">Lista de destinatarios separados por coma</param>
    /// <param name="strSubject">Asunto del correo</param>
    /// <param name="strMensaje">Mensaje del correo</param>
    /// <param name="strRuta">Ruta del archivo a adjuntar</param>
    /// <returns>Retorna un booleano indicando si el envío fue exitoso</returns>
    public bool fnEnviarCorreoDoc(string strMailTo, string strSubject, string strMensaje, string strRutaZip, string sNombreZip, string sCC)
        {
            bool bretorno = false;
            try
            {
                //Crear objetos para enviar correo.
                System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                
                //Asignar variables de los objetos.
                sMensaje.From = new System.Net.Mail.MailAddress(clsComun.ObtenerParamentro("emailAppFrom"));
                sMensaje.To.Add(strMailTo);
                sMensaje.Subject = strSubject;
                sMensaje.Body = strMensaje;
                if (sCC != string.Empty)
                sMensaje.CC.Add(sCC);
                sMensaje.IsBodyHtml = true;

                if (!(string.IsNullOrEmpty(strRutaZip)))
                {
                    Attachment data1 = new Attachment(strRutaZip, MediaTypeNames.Application.Zip);
                    data1.Name = sNombreZip + ".Zip";
                    sMensaje.Attachments.Add(data1);

                    //Cuenta generica para el envio de correos.
                    smtp.Host = clsComun.ObtenerParamentro("emailHost");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new System.Net.NetworkCredential(clsComun.ObtenerParamentro("emailAppFrom"), Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("emailPassword")));
                    smtp.Port = Convert.ToInt32(clsComun.ObtenerParamentro("emailPort"));
                    smtp.EnableSsl = true;
                    smtp.Send(sMensaje);
                    sMensaje.Dispose();
                    smtp.Dispose();
                    data1.Dispose();
                }
                else
                {
                    //Cuenta generica para el envio de correos.
                    smtp.Host = clsComun.ObtenerParamentro("emailHost");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new System.Net.NetworkCredential(clsComun.ObtenerParamentro("emailAppFrom"), Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("emailPassword")));
                    smtp.Port = Convert.ToInt32(clsComun.ObtenerParamentro("emailPort"));
                    smtp.EnableSsl = true;
                    smtp.Send(sMensaje);
                    sMensaje.Dispose();
                    smtp.Dispose();
                }

                //if (comprobante.InnerXml.Length > 0)
                //{
                //    System.IO.Stream xmlinfo = ConvertToStream(comprobante);
                //    Attachment data = new Attachment(xmlinfo, MediaTypeNames.Application.Octet);
                //    data.Name = "Comprobante.xml";
                //    sMensaje.Attachments.Add(data);
                //    xmlinfo.Dispose();
                //}


                bretorno = true;
            }
            catch (Exception ex)
            {
                bretorno = false;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email);
            }
            return bretorno;
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

        /// <summary>
        /// Crear archivo pdf, xml y enviarlos
        /// </summary>
        /// <param name="pxComprobante"></param>
        /// <param name="sPlantilla"></param>
        /// <param name="psIdCfd"></param>
        /// <param name="psTipoDocumento"></param>
        /// <param name="pagina"></param>
        /// <param name="sRutaPDF"></param>
        /// <param name="id_contribuyente"></param>
        /// <param name="id_rfc"></param>
        /// <param name="scolor"></param>
        /// <returns></returns>
        public bool fnPdfEnvioCorreo(string psPlantilla, string psIdCfd, string psTipoDocumento, 
        string sRutaPDF, int pnid_contribuyente, int pnid_rfc, string pscolor, string pstrMailTo, string pstrSubject, string pstrMensaje,
            string psNombreRutaZip, string psNombreRutaXML, string sNombreDoc, string sCC)
        {
            bool bEnvio = false;
            byte[] buffer = { };
            byte[] bufferPDF = { };
            DateTime Fecha = DateTime.Today;
            cpl = new clsPlantillaLista();
            
            try
            {
                //Guid GidXml;
                //GidXml = Guid.NewGuid();
                //Guid GidZip;
                //GidZip = Guid.NewGuid();
                string sRutaZip;
                //Se crean directorios para grabar archivo temporal
                Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocZips") + psNombreRutaZip);
                Directory.CreateDirectory(clsComun.ObtenerParamentro("RutaDocXmlZips") + psNombreRutaXML);

                //Se obtiene el xml del comprobante
                gDAL = new clsOperacionConsulta();
                XmlDocument pxComprobante = new XmlDocument();
                pxComprobante = gDAL.fnObtenerComprobanteXML(pnid_contribuyente, psIdCfd);

                //Crear pdf segun plantilla usuario
                cpl.fnCrearPLantillaEnvio(pxComprobante, psPlantilla, psIdCfd, psTipoDocumento, sRutaPDF, pnid_contribuyente, pnid_rfc, pscolor);
                //Se almacena el xml
                buffer = Encoding.UTF8.GetBytes(pxComprobante.InnerXml);
                //Se obtiene la ruta de creacion del xml
                string path = clsComun.ObtenerParamentro("RutaDocXmlZips") + psNombreRutaXML + "\\" + sNombreDoc + ".xml";//GidXml + ".xml";

                // Create the text file if it doesn't already exist.
                if (!File.Exists(path))
                {
                    File.WriteAllBytes(path, buffer);
                }

                sRutaZip = clsComun.ObtenerParamentro("RutaDocZips") + psNombreRutaZip + "\\" + sNombreDoc + ".zip";//GidZip + ".zip";

                ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(sRutaZip));
                zip.SetLevel(6);

                string folder = clsComun.ObtenerParamentro("RutaDocXmlZips") + psNombreRutaXML;
                //Se comprime el pdf y xml
                ComprimirCarpeta(folder, folder, zip);

                zip.Finish();
                zip.Close(); 

                //Enviar Zip
                bEnvio = fnEnviarCorreoDoc(pstrMailTo, pstrSubject, pstrMensaje, sRutaZip, sNombreDoc, sCC);

                zip.Dispose();
                zip.Finish();
                zip.Close();
   
                //Eliminar archivos creados
                foreach (string file in Directory.GetFiles(clsComun.ObtenerParamentro("RutaDocXmlZips") + psNombreRutaXML))
                {
                    File.Delete(file);
                }
                Directory.Delete(clsComun.ObtenerParamentro("RutaDocXmlZips") + psNombreRutaXML, true);

                //Eliminar carpetas temporales creadas
                foreach (string file in Directory.GetFiles(clsComun.ObtenerParamentro("RutaDocZips") + psNombreRutaZip))
                {
                    File.Delete(file);
                }
                Directory.Delete(clsComun.ObtenerParamentro("RutaDocZips") + psNombreRutaZip, true);
            }
            catch (Exception ex)
            {
                bEnvio = false;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email);
            }
            return bEnvio;
        }


        public static void ComprimirCarpeta(string RootFolder, string CurrentFolder, ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream)
        {
            string[] SubFolders = Directory.GetDirectories(CurrentFolder);

            //Llama de nuevo al metodo recursivamente para cada carpeta
            foreach (string Folder in SubFolders)
            {
                ComprimirCarpeta(RootFolder, Folder, zStream);
            }

            string relativePath = CurrentFolder.Substring(RootFolder.Length) + "/";

            //the path "/" is not added or a folder will be created
            //at the root of the file
            if (relativePath.Length > 1)
            {
                ICSharpCode.SharpZipLib.Zip.ZipEntry dirEntry;
                dirEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(relativePath);

            }

            //Añade todos los ficheros de la carpeta al zip
            foreach (string file in Directory.GetFiles(CurrentFolder))
            {
                AñadirFicheroaZip(zStream, relativePath, file);
            }
        }

        private static void AñadirFicheroaZip(ICSharpCode.SharpZipLib.Zip.ZipOutputStream zStream, string relativePath, string file)
        {
            byte[] buffer = new byte[4096];

            //the relative path is added to the file in order to place the file within
            //this directory in the zip
            string fileRelativePath = (relativePath.Length > 1 ? relativePath : string.Empty)
                                      + Path.GetFileName(file);

            ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(fileRelativePath);

            zStream.PutNextEntry(entry);

            using (FileStream fs = File.OpenRead(file))
            {
                int sourceBytes;
                do
                {
                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    zStream.Write(buffer, 0, sourceBytes);
                } while (sourceBytes > 0);
            }
        }

    /// <summary>
    /// Valida formato de email
    /// </summary>
    /// <param name="sCorreos">email a validar</param>
    /// <returns>regresa la cadena de email que contiene el formato incorrecto</returns>
        public string fValidaEmail(string sCorreos)
        {
            char[] cad = { ',' };
            string[] s1;
            int i = 0;
            string expresion, sCorreoVal;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"; //Formato para validar correo
            sCorreoVal = string.Empty;

            s1 = sCorreos.Split(cad);

            for (i = 0; i < s1.Length; i++) //Recorreo el texto con los correos
            {
                if (Regex.IsMatch(s1[i].Trim(), expresion))
                {
                    if (Regex.Replace(s1[i].Trim(), expresion, string.Empty).Length == 0) //Si la cadena es correcta regresa vacio
                        sCorreoVal = string.Empty;
                    else
                    {
                        sCorreoVal = s1[i]; //Si la cadena esta escrita de forma incorrecta regresa la cadena incorrecta
                        break;
                    }
                }
                else
                {
                    sCorreoVal = s1[i] + "."; //Si la cadena esta escrita de forma incorrecta regresa la cadena incorrecta
                    break;
                }
            }

            return sCorreoVal;
        }

}
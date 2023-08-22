using System;
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
using System.Text.RegularExpressions;
using System.Data;

/// <summary>
/// Clase encargada del envio de documentos por correo (XML, PDF...)
/// </summary>
public class clsEnvioCorreoDocs
{
    clsPlantillaLista cpl;
    //clsOperacionConsulta gDAL;
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
                sMensaje.From = new System.Net.Mail.MailAddress(ConectorPAXGenericoPDF.Properties.Settings.Default["emailAppFrom"].ToString());
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
                    smtp.Host = ConectorPAXGenericoPDF.Properties.Settings.Default["emailHost"].ToString();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new System.Net.NetworkCredential(ConectorPAXGenericoPDF.Properties.Settings.Default["emailAppFrom"].ToString(), Utilerias.Encriptacion.Base64.DesencriptarBase64(ConectorPAXGenericoPDF.Properties.Settings.Default["emailPassword"].ToString()));
                    smtp.Port = Convert.ToInt32(ConectorPAXGenericoPDF.Properties.Settings.Default["emailPort"].ToString());
                    smtp.EnableSsl = true;
                    smtp.Send(sMensaje);
                    sMensaje.Dispose();
                    smtp.Dispose();
                    data1.Dispose();
                }
                else
                {
                    //Cuenta generica para el envio de correos.
                    smtp.Host = ConectorPAXGenericoPDF.Properties.Settings.Default["emailHost"].ToString();
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new System.Net.NetworkCredential(ConectorPAXGenericoPDF.Properties.Settings.Default["emailAppFrom"].ToString(), Utilerias.Encriptacion.Base64.DesencriptarBase64(ConectorPAXGenericoPDF.Properties.Settings.Default["emailPassword"].ToString()));
                    smtp.Port = Convert.ToInt32(ConectorPAXGenericoPDF.Properties.Settings.Default["emailPort"].ToString());
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
                try
                {
                    DateTime Fechaex = DateTime.Today;
                    string pathex = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";

                    if (!File.Exists(pathex))
                    {
                        StreamWriter sr4 = new StreamWriter(pathex);
                        sr4.WriteLine(DateTime.Now + " " + ex.Message);
                        sr4.Close();
                    }
                    else
                    {
                        System.IO.StreamWriter sw4 = new System.IO.StreamWriter(pathex, true);
                        sw4.WriteLine(DateTime.Now + " " + ex.Message);
                        sw4.Close();
                    }
                }
                catch
                {
                }
            }
            return bretorno;
        }

    /// <summary>
    /// Envia los archivos sin comprimir en ZIP
    /// </summary>
    /// <param name="strMailTo"></param>
    /// <param name="strSubject"></param>
    /// <param name="strMensaje"></param>
    /// <param name="strRutaXML"></param>
    /// <param name="strRutaPDF"></param>
    /// <param name="sNombreZip"></param>
    /// <param name="sCC"></param>
    /// <returns></returns>
    public bool fnEnviarCorreoDocSinZIP(string strMailTo, string strSubject, string strMensaje, string strRutaXML,string strRutaPDF, string sNombreZip, string sCC)
    {
        bool bretorno = false;
        try
        {
            //Crear objetos para enviar correo.
            System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                
            //Asignar variables de los objetos.
            sMensaje.From = new System.Net.Mail.MailAddress(ConectorPAXGenericoPDF.Properties.Settings.Default["emailAppFrom"].ToString());
            sMensaje.To.Add(strMailTo);
            sMensaje.Subject = strSubject;
            sMensaje.Body = strMensaje;
            if (sCC != string.Empty)
            sMensaje.CC.Add(sCC);
            sMensaje.IsBodyHtml = true;

            if (!(string.IsNullOrEmpty(strRutaXML)))
            {
                Attachment data1 = new Attachment(strRutaXML, MediaTypeNames.Application.Pdf);
                Attachment data2 = new Attachment(strRutaPDF);
                data1.Name = sNombreZip + ".xml";
                data2.Name = sNombreZip + ".pdf";
                sMensaje.Attachments.Add(data1);
                sMensaje.Attachments.Add(data2);

                //Cuenta generica para el envio de correos.
                smtp.Host = ConectorPAXGenericoPDF.Properties.Settings.Default["emailHost"].ToString();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(ConectorPAXGenericoPDF.Properties.Settings.Default["emailAppFrom"].ToString(), ConectorPAXGenericoPDF.Properties.Settings.Default["emailPassword"].ToString());
                smtp.Port = Convert.ToInt32(ConectorPAXGenericoPDF.Properties.Settings.Default["emailPort"].ToString());
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
                smtp.Host = ConectorPAXGenericoPDF.Properties.Settings.Default["emailHost"].ToString();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(ConectorPAXGenericoPDF.Properties.Settings.Default["emailAppFrom"].ToString(), Utilerias.Encriptacion.Base64.DesencriptarBase64(ConectorPAXGenericoPDF.Properties.Settings.Default["emailPassword"].ToString()));
                smtp.Port = Convert.ToInt32(ConectorPAXGenericoPDF.Properties.Settings.Default["emailPort"].ToString());
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
            try
            {
                DateTime Fechaex = DateTime.Today;
                string pathex = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";

                if (!File.Exists(pathex))
                {
                    StreamWriter sr4 = new StreamWriter(pathex);
                    sr4.WriteLine(DateTime.Now + " " + ex.Message);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(pathex, true);
                    sw4.WriteLine(DateTime.Now + " " + ex.Message);
                    sw4.Close();
                }
            }
            catch
            {
            }
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
    //public bool fnPdfEnvioCorreo(string psPlantilla, string psIdCfd, string psTipoDocumento, 
    //string sRutaPDF, int pnid_contribuyente, int pnid_rfc, string pscolor, string pstrMailTo, string pstrSubject, string pstrMensaje,
    //    string psNombreRutaZip, string psNombreRutaXML, string sNombreDoc, string sCC)
    //{
    //    bool bEnvio = false;
    //    byte[] buffer = { };
    //    byte[] bufferPDF = { };
    //    DateTime Fecha = DateTime.Today;
    //    cpl = new clsPlantillaLista();
            
    //    try
    //    {

    //        string sRutaZip;
    //        //Se crean directorios para grabar archivo temporal
    //        Directory.CreateDirectory(ConectorPAXGenerico.Properties.Settings.Default["RutaDocZips"].ToString() + psNombreRutaZip);
    //        Directory.CreateDirectory(ConectorPAXGenerico.Properties.Settings.Default["RutaDocXmlZips"].ToString() + psNombreRutaXML);

    //        //Se obtiene el xml del comprobante
    //        gDAL = new clsOperacionConsulta();


    //        XmlDocument pxComprobante = new XmlDocument();
    //        XmlDocument pxComprobantePDF = new XmlDocument();
    //        pxComprobante = gDAL.fnObtenerComprobanteXML(pnid_contribuyente, psIdCfd);

    //        pxComprobantePDF.LoadXml(pxComprobante.InnerXml);

    //        //// Create an XML declaration. 
    //        //XmlDeclaration xmldecl;
    //        //xmldecl = pxComprobante.CreateXmlDeclaration("1.0", null, null);
    //        //xmldecl.Encoding = "UTF-8";
    //        //xmldecl.Standalone = null;

    //        //// Add the new node to the document.
    //        //XmlElement root = pxComprobante.DocumentElement;
    //        //pxComprobante.InsertBefore(xmldecl, root);

    //        //*************Se agrega Adenda a XML en caso de existir***************
    //        clsConfiguracionAddenda gADD = new clsConfiguracionAddenda();
    //        DataTable addenda = new DataTable();
    //        int idEstructura = 0;
    //        string AddendaNamespace = string.Empty;

    //        XmlDeclaration xDec = pxComprobantePDF.CreateXmlDeclaration("1.0", "UTF-8", "yes");
    //        pxComprobantePDF.InsertBefore(xDec, pxComprobantePDF.DocumentElement);

    //        idEstructura = gADD.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
    //        addenda = gADD.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructura);
    //        if (addenda.Rows.Count > 0)
    //        {
    //            XmlDocument xAddenda = new XmlDocument();
    //            int idModulo = Convert.ToInt32(addenda.Rows[0]["id_modulo"]);
    //            xAddenda.LoadXml(Convert.ToString(addenda.Rows[0]["addenda"]));
    //            AddendaNamespace = gADD.fnObtieneNameSpace(idModulo);

    //            if (AddendaNamespace != "")
    //            {
    //                string[] nombre = AddendaNamespace.Split('=');
    //                XmlAttribute xAttribute = pxComprobantePDF.CreateAttribute(nombre[0]);
    //                xAttribute.InnerText = AddendaNamespace;
    //                pxComprobantePDF.ChildNodes[1].Attributes.Append(xAttribute);
    //            }


    //            XmlNode childElement = pxComprobantePDF.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobantePDF.DocumentElement.NamespaceURI);
    //            pxComprobantePDF.ChildNodes[1].AppendChild(childElement);

    //            childElement.InnerXml = xAddenda.OuterXml;
    //        }
    //        //******************Fin agregado de adenda********************

    //        //Crear pdf segun plantilla usuario
    //        cpl.fnCrearPLantillaEnvio(pxComprobante, psTipoDocumento, sRutaPDF);
    //        //Se almacena el xml
    //        buffer = Encoding.UTF8.GetBytes(pxComprobantePDF.InnerXml);
    //        //Se obtiene la ruta de creacion del xml
    //        string path = ConectorPAXGenerico.Properties.Settings.Default["RutaDocXmlZips"].ToString() + psNombreRutaXML + "\\" + sNombreDoc + ".xml";//GidXml + ".xml";

    //        // Create the text file if it doesn't already exist.
    //        if (!File.Exists(path))
    //        {
    //            File.WriteAllBytes(path, buffer);
    //        }

    //        sRutaZip = ConectorPAXGenerico.Properties.Settings.Default["RutaDocZips"].ToString() + psNombreRutaZip + "\\" + sNombreDoc + ".zip";//GidZip + ".zip";

    //        ICSharpCode.SharpZipLib.Zip.ZipOutputStream zip = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(sRutaZip));
    //        zip.SetLevel(6);

    //        string folder = ConectorPAXGenerico.Properties.Settings.Default["RutaDocXmlZips"].ToString() + psNombreRutaXML;
    //        //Se comprime el pdf y xml
    //        ComprimirCarpeta(folder, folder, zip);

    //        zip.Finish();
    //        zip.Close(); 

    //        //Enviar Zip
    //        bEnvio = fnEnviarCorreoDoc(pstrMailTo, pstrSubject, pstrMensaje, sRutaZip, sNombreDoc, sCC);

    //        zip.Dispose();
    //        zip.Finish();
    //        zip.Close();
   
    //        //Eliminar archivos creados
    //        foreach (string file in Directory.GetFiles(ConectorPAXGenerico.Properties.Settings.Default["RutaDocXmlZips"].ToString() + psNombreRutaXML))
    //        {
    //            File.Delete(file);
    //        }
    //        Directory.Delete(ConectorPAXGenerico.Properties.Settings.Default["RutaDocXmlZips"].ToString() + psNombreRutaXML, true);

    //        //Eliminar carpetas temporales creadas
    //        foreach (string file in Directory.GetFiles(ConectorPAXGenerico.Properties.Settings.Default["RutaDocZips"].ToString() + psNombreRutaZip))
    //        {
    //            File.Delete(file);
    //        }
    //        Directory.Delete(ConectorPAXGenerico.Properties.Settings.Default["RutaDocZips"].ToString() + psNombreRutaZip, true);
    //    }
    //    catch (Exception ex)
    //    {
    //        bEnvio = false;
    //        try
    //        {
    //            DateTime Fechaex = DateTime.Today;
    //            string pathex = (String)ConectorPAXGenerico.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";

    //            if (!File.Exists(pathex))
    //            {
    //                StreamWriter sr4 = new StreamWriter(pathex);
    //                sr4.WriteLine(DateTime.Now + " " + ex.Message);
    //                sr4.Close();
    //            }
    //            else
    //            {
    //                System.IO.StreamWriter sw4 = new System.IO.StreamWriter(pathex, true);
    //                sw4.WriteLine(DateTime.Now + " " + ex.Message);
    //                sw4.Close();
    //            }
    //        }
    //        catch
    //        {
    //        }
    //    }
    //    return bEnvio;
    //}


    /// <summary>
    /// Crear archivo pdf, xml y enviarlos sin ZIP
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
    public bool fnPdfEnvioCorreoSinZIP(XmlDocument pxDocumento, string psRutaPDF, string pstrMailTo, string pstrSubject, 
        string pstrMensaje, string psRutaXML, string psNombreDoc, string psCC)
    {
        bool bEnvio = false;
        byte[] buffer = { };
        byte[] bufferPDF = { };
        DateTime Fecha = DateTime.Today;
        cpl = new clsPlantillaLista();

        try
        {

            //Se crean directorios para grabar archivo temporal
            //Directory.CreateDirectory(ConectorPAXGenerico.Properties.Settings.Default["RutaDocXmlZips"] + psNombreRutaXML);

            //Se obtiene el xml del comprobante
            //gDAL = new clsOperacionConsulta();

            XmlDocument pxComprobante = new XmlDocument();
            XmlDocument pxComprobantePDF = new XmlDocument();
            pxComprobante = pxDocumento;//gDAL.fnObtenerComprobanteXML(pnid_contribuyente, psIdCfd);

            pxComprobantePDF.LoadXml(pxComprobante.InnerXml);

            bEnvio = fnEnviarCorreoDocSinZIP(pstrMailTo, pstrSubject, pstrMensaje, psRutaXML, psRutaPDF, psNombreDoc, psCC);

        }
        catch (Exception ex)
        {
            bEnvio = false;
            try
            {
                DateTime Fechaex = DateTime.Today;
                string pathex = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";

                if (!File.Exists(pathex))
                {
                    StreamWriter sr4 = new StreamWriter(pathex);
                    sr4.WriteLine(DateTime.Now + " " + ex.Message);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(pathex, true);
                    sw4.WriteLine(DateTime.Now + " " + ex.Message);
                    sw4.Close();
                }
            }
            catch
            {
            }
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
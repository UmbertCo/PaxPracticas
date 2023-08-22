using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Text;
using System.Collections;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Net.Mime;


namespace XmlWebService
{
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    public class Service1 : System.Web.Services.WebService
    {
        public XmlDocument recibido;
        [WebMethod]
        public void ValidarXml(XmlDocument XmlRecibido) 
        {
            ///asfasdfasfd
            ///asdfasdfasdfsadf
            /////asdfasdfasdfasdf
            recibido = XmlRecibido; // ASIGNAR EL XMLDOCUMENT A UN NUEVO DOCUMENT LLAMADO RECIBIDO  
            XmlNodeList usuarios = recibido.GetElementsByTagName("Usuarios");// CREAR LISTA DE USUARIOS EN BASE A EL TAGNAME USUARIOS
            XmlNodeList listaUsuario = ((XmlElement)usuarios[0]).GetElementsByTagName("Nombre");// CREAR LISTA DE NOMBRES
            XmlNodeList listaRFC = ((XmlElement)usuarios[0]).GetElementsByTagName("RFC");// CREAR LISTA DE RFC´S


            foreach (XmlElement nodo in listaUsuario)// PARA CADA ELEMENTO EN LA LISTA USUARIO, VERIFICA SI NO ES UNA CADENA VACIA
            {
                if (nodo.InnerText != string.Empty)
                {                                     
                    nodo.InnerText = nodo.InnerText + " --> Verificado";
                    
                }

            }

            foreach (XmlElement nodoRFC in listaRFC)
            {
                if (Regex.IsMatch(nodoRFC.InnerText, @"([A-Z]{4})\d{6}([A-Z]{3})"))
                {
                 nodoRFC.InnerText = nodoRFC.InnerText + "--> RFC Correcto";
                }
                else
	            {
                 nodoRFC.InnerText = nodoRFC.InnerText + "--> RFC invalido";
	            }
            }

            enviarEmail();
            recibido.Save(@"C:\WS3\Info3copia.xml");
            //XmlAttribute newatt = recibido.CreateAttribute("mensaje");
            //newatt.Value = "Verificado 2";
            //XmlAttributeCollection atCol = recibido.DocumentElement.Attributes;
            //atCol.InsertAfter(newatt, atCol[0]);

            //
            
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void enviarEmail()
        {


            MemoryStream mStream = new MemoryStream();
            recibido.Save(mStream);
            mStream.Flush();
            mStream.Position = 0;

            MailMessage mail = new MailMessage("cartuleitor@hotmail.com","carlos_mt@live.com")
            {
                Subject = "Prueba de mail",
                IsBodyHtml = true,
                Body = "Envio de archivo .xml"
            };

            mail.Attachments.Add(new Attachment(mStream, "recibido.xml"));
            SmtpClient client = new SmtpClient("smtp.live.com", 587)
            {
                Host = "smtp.live.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("cartuleitor@hotmail.com", "elcartu2276875")
            };

            client.Send(mail);
        }

    }
}

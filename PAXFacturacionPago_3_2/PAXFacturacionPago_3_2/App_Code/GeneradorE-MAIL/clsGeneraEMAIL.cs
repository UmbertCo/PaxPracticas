using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;

/// <summary>
/// Clase encargada de la generacion de correos.
/// </summary>
public class clsGeneraEMAIL
{

    /// <summary>
    /// EnviarCorreo
    /// </summary>
    /// <param name="strMailTo">Lista de destinatarios separados por coma</param>
    /// <param name="strSubject">Asunto del correo</param>
    /// <param name="strMensaje">Mensaje del correo</param>
    /// <returns>Retorna un booleano indicando si el envío fue exitoso</returns>
    public bool EnviarCorreo(string strMailTo, string strSubject,string strMensaje)
    {
        bool bretorno=false;

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
            sMensaje.IsBodyHtml = true;
            //Cuenta generica para el envio de correos.
            smtp.Host = clsComun.ObtenerParamentro("emailHost");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.Credentials = new System.Net.NetworkCredential(clsComun.ObtenerParamentro("emailUser"), Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("emailPassword")));            
            smtp.Credentials = new System.Net.NetworkCredential(clsComun.ObtenerParamentro("emailUser"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("emailPassword")));
            smtp.Port = Convert.ToInt32(clsComun.ObtenerParamentro("emailPort"));
            smtp.EnableSsl = true;
            smtp.Send(sMensaje);
             

            bretorno=true;

        }
        catch(Exception ex)
        {
            bretorno=false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email); 
        }


        return bretorno;
    }


    /// <summary>
    /// EnviarCorreo
    /// </summary>
    /// <param name="strMailTo">Lista de destinatarios separados por coma</param>
    /// <param name="strSubject">Asunto del correo</param>
    /// <param name="strMensaje">Mensaje del correo</param>
    /// <returns>Retorna un booleano indicando si el envío fue exitoso</returns>
    public bool EnviarCorreoticket(string strMailTo, string strSubject, string strMensaje, string strRuta)
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
            sMensaje.IsBodyHtml = true;

            if (!(string.IsNullOrEmpty(strRuta)))
            {
                Attachment data = new Attachment(strRuta, MediaTypeNames.Application.Octet);
                sMensaje.Attachments.Add(data);
            }

            //Cuenta generica para el envio de correos.
            smtp.Host = clsComun.ObtenerParamentro("emailHost");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.Credentials = new System.Net.NetworkCredential(clsComun.ObtenerParamentro("emailUser"), Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("emailPassword")));            
            smtp.Credentials = new System.Net.NetworkCredential(clsComun.ObtenerParamentro("emailUser"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.ObtenerParamentro("emailPassword")));
            smtp.Port = Convert.ToInt32(clsComun.ObtenerParamentro("emailPort"));
            smtp.EnableSsl = true;
            smtp.Send(sMensaje);


            bretorno = true;

        }
        catch (Exception ex)
        {
            bretorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email);
        }


        return bretorno;
    }
}
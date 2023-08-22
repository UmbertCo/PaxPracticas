using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Linq;
using System.Web;

/// <summary>
/// Clase encargada de la generacion de correos.
/// </summary>
public class clsGeneraEMAIL
{
    /// <summary>
    /// EnviarCorreo
    /// </summary>
    /// <param name="psRemitente">Lista de destinatarios separados por coma</param>
    /// <param name="psAsunto">Asunto del correo</param>
    /// <param name="psMensaje">Mensaje del correo</param>
    /// <param name="psRuta">Ruta del adjunto</param>
    /// <returns>Retorna un booleano indicando si el envío fue exitoso</returns>
    public bool fnEnviarCorreo(string psRemitente, string psAsunto,string psMensaje, string psRuta)
    {
        bool bretorno=false;
        try
        {
            //Crear objetos para enviar correo.
            System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

            //Asignar variables de los objetos.
            sMensaje.From = new System.Net.Mail.MailAddress(clsComun.fnObtenerParamentro("emailAppFrom"));
            sMensaje.To.Add(psRemitente);
            sMensaje.Subject = psAsunto;
            sMensaje.Body = psMensaje;
            sMensaje.IsBodyHtml = true;
            if (!(psRuta == "") && psRuta != null)
            {
                Attachment data = new Attachment(psRuta, MediaTypeNames.Application.Octet);
                sMensaje.Attachments.Add(data);
            }
            //Cuenta generica para el envio de correos.
            smtp.Host = clsComun.fnObtenerParamentro("emailHost");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.Credentials = new System.Net.NetworkCredential(clsComun.fnObtenerParamentro("emailAppFrom"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.fnObtenerParamentro("emailPassword")));
            smtp.Credentials = new System.Net.NetworkCredential(clsComun.fnObtenerParamentro("emailUser"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.fnObtenerParamentro("emailPassword")));
            smtp.Port = Convert.ToInt32(clsComun.fnObtenerParamentro("emailPort"));
            smtp.EnableSsl = true;
            smtp.Send(sMensaje);
             
            bretorno=true;
        }
        catch(Exception ex)
        {
            bretorno=false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email, "fnEnviarCorreo", "clsGeneraEMAIL"); 
        }
        return bretorno;
    }

    /// <summary>
    /// EnviarCorreo
    /// </summary>
    /// <param name="psRemitente">Lista de destinatarios separados por coma</param>
    /// <param name="psAsunto">Asunto del correo</param>
    /// <param name="psMensaje">Mensaje del correo</param>
    /// <param name="psReplyTo">Copiar a</param>
    /// <returns>Retorna un booleano indicando si el envío fue exitoso</returns>
    public bool fnEnviarCorreoAtencionIncidencia(string psRemitente, string psAsunto, string psMensaje, string psReplyTo)
    {
        bool bretorno = false;
        try
        {
            //Crear objetos para enviar correo
            System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

            //Asignar variables de los objetos.
            sMensaje.From = new System.Net.Mail.MailAddress(clsComun.fnObtenerParamentro("emailAppFrom"));
            sMensaje.To.Add(psRemitente);
            sMensaje.ReplyToList.Add(psReplyTo);
            sMensaje.Subject = psAsunto;
            sMensaje.Body = psMensaje;
            sMensaje.IsBodyHtml = true;
            //Cuenta generica para el envio de correos.
            smtp.Host = clsComun.fnObtenerParamentro("emailHost");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.Credentials = new System.Net.NetworkCredential(clsComun.fnObtenerParamentro("emailAppFrom"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.fnObtenerParamentro("emailPassword")));
            smtp.Credentials = new System.Net.NetworkCredential(clsComun.fnObtenerParamentro("emailUser"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.fnObtenerParamentro("emailPassword")));
            smtp.Port = Convert.ToInt32(clsComun.fnObtenerParamentro("emailPort"));
            smtp.EnableSsl = true;
            smtp.Send(sMensaje);

            bretorno = true;
        }
        catch (Exception ex)
        {
            bretorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email, "fnEnviarCorreoAtencionIncidencia", "clsGeneraEMAIL");
        }
        return bretorno;
    }

    /// <summary>
    /// EnviarCorreo
    /// </summary>
    /// <param name="psRemitente">Lista de destinatarios separados por coma</param>
    /// <param name="psAsunto">Asunto del correo</param>
    /// <param name="psMensaje">Mensaje del correo</param>
    /// <param name="psReplyTo">Copiar a</param>
    /// <returns>Retorna un booleano indicando si el envío fue exitoso</returns>
    public bool fnEnviarCorreoAtencionPruebas(string psRemitente, string psAsunto, string psMensaje, string strReplyTo, string psArchivo)
    {
        bool bretorno = false;
        try
        {
            //Crear objetos para enviar correo
            System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

            //Asignar variables de los objetos.
            sMensaje.From = new System.Net.Mail.MailAddress(clsComun.fnObtenerParamentro("emailAppFrom"));
            sMensaje.To.Add(psRemitente);
            sMensaje.ReplyToList.Add(strReplyTo);
            sMensaje.Subject = psAsunto;
            sMensaje.Body = psMensaje;
            sMensaje.IsBodyHtml = true;
            if (!(psArchivo == "") && !(psArchivo == null))
            {
                Attachment data = new Attachment(psArchivo, MediaTypeNames.Application.Octet);
                sMensaje.Attachments.Add(data);
            }
            //Cuenta generica para el envio de correos.
            smtp.Host = clsComun.fnObtenerParamentro("emailHost");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.Credentials = new System.Net.NetworkCredential(clsComun.fnObtenerParamentro("emailAppFrom"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.fnObtenerParamentro("emailPassword")));
            smtp.Credentials = new System.Net.NetworkCredential(clsComun.fnObtenerParamentro("emailUser"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.fnObtenerParamentro("emailPassword")));
            smtp.Port = Convert.ToInt32(clsComun.fnObtenerParamentro("emailPort"));
            smtp.EnableSsl = true;
            smtp.Send(sMensaje);

            bretorno = true;
        }
        catch (Exception ex)
        {
            bretorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email, "fnEnviarCorreoAtencionPruebas", "clsGeneraEMAIL");
        }

        return bretorno;
    }

    /// <summary>
    /// EnviarCorreo
    /// </summary>
    /// <param name="psRemitente">Lista de destinatarios separados por coma</param>
    /// <param name="psAsunto">Asunto del correo</param>
    /// <param name="psMensaje">Mensaje del correo</param>
    /// <param name="psReplyTo">Copiar a</param>
    /// <returns>Retorna un booleano indicando si el envío fue exitoso</returns>
    public bool fnEnviarCorreoAtencionSAT(string psRemitente, string psAsunto, string psMensaje, string psReplyTo, string psArchivo)
    {
        bool bretorno = false;
        try
        {
            //Crear objetos para enviar correo
            System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

            //Asignar variables de los objetos.
            sMensaje.From = new System.Net.Mail.MailAddress(clsComun.fnObtenerParamentro("emailAppFrom"));
            sMensaje.To.Add(psRemitente);
            sMensaje.ReplyToList.Add(psReplyTo);
            sMensaje.Subject = psAsunto;
            sMensaje.Body = psMensaje;
            sMensaje.IsBodyHtml = true;
            if (!(psArchivo == "") && !(psArchivo == null))
            {
                Attachment data = new Attachment(psArchivo, MediaTypeNames.Application.Octet);
                sMensaje.Attachments.Add(data);
            }
            //Cuenta generica para el envio de correos.
            smtp.Host = clsComun.fnObtenerParamentro("emailHost");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.Credentials = new System.Net.NetworkCredential(clsComun.fnObtenerParamentro("emailAppFrom"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.fnObtenerParamentro("emailPassword")));
            smtp.Credentials = new System.Net.NetworkCredential(clsComun.fnObtenerParamentro("emailUser"), PAXCrypto.CryptoAES.DesencriptaAES64(clsComun.fnObtenerParamentro("emailPassword")));
            smtp.Port = Convert.ToInt32(clsComun.fnObtenerParamentro("emailPort"));
            smtp.EnableSsl = true;
            smtp.Send(sMensaje);

            bretorno = true;
        }
        catch (Exception ex)
        {
            bretorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email, "fnEnviarCorreoAtencionSAT", "clsGeneraEMAIL");
        }
        return bretorno;
    }
}

using System;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Data;
using System.IO;
using System.Data.SqlClient;

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
    public bool EnviarCorreo(string strMailTo, string strSubject, string strMensaje)
    {
        bool bretorno = false;

        //try
        //{
        //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");
        //    DataTable dtConfiguracion = new DataTable("ConfiguracionCorreo");
        //    string strUsuario = string.Empty;
        //    string strPassword = string.Empty;
        //    string strHost = string.Empty;
        //    bool bEnableSsl = false;
        //    int nPort = 0;
        //    string strRemitente = string.Empty; 
        //    iSql.Query("usp_Ctp_Cuenta_Envio_Sel", true, ref dtConfiguracion);
        //    if (dtConfiguracion.Rows.Count > 0)
        //    {
        //        strUsuario = dtConfiguracion.Rows[0]["usuario"].ToString();
        //        strPassword = dtConfiguracion.Rows[0]["password"].ToString();
        //        strHost = dtConfiguracion.Rows[0]["host"].ToString();
        //        strRemitente = dtConfiguracion.Rows[0]["remitente"].ToString();
        //        bEnableSsl = Convert.ToBoolean(dtConfiguracion.Rows[0]["enable_ssl"].ToString());
        //        nPort = Convert.ToInt32(dtConfiguracion.Rows[0]["port"].ToString());
        //    }

            
        //    //Crear objetos para enviar correo.
        //    System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
        //    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

        //    //Asignar variables de los objetos.
        //    sMensaje.From = new System.Net.Mail.MailAddress(strRemitente);
        //    sMensaje.To.Add(strMailTo);

        //    sMensaje.Subject = strSubject;
        //    sMensaje.Body = strMensaje;
        //    sMensaje.IsBodyHtml = true;
        //    //Cuenta generica para el envio de correos.
        //    smtp.Host = strHost;
        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    smtp.Credentials = new System.Net.NetworkCredential(strUsuario, Utilerias.Encriptacion.Base64.DesencriptarBase64(strPassword));
        //    smtp.Port = nPort;
        //    smtp.EnableSsl = bEnableSsl;
        //    smtp.Send(sMensaje);


        //    bretorno = true;

        //}
        //catch (Exception ex)
        //{
        //    bretorno = false;
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email);
        //}


        //return bretorno;

        DataTable dtConfiguracion = new DataTable("ConfiguracionCorreo");
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    string strUsuario = string.Empty;
                    string strPassword = string.Empty;
                    string strHost = string.Empty;
                    bool bEnableSsl = false;
                    int nPort = 0;
                    string strRemitente = string.Empty;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Cuenta_Envio_Sel";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtConfiguracion);
                    }
                    if (dtConfiguracion.Rows.Count > 0)
                    {
                        strUsuario = dtConfiguracion.Rows[0]["usuario"].ToString();
                        strPassword = dtConfiguracion.Rows[0]["password"].ToString();
                        strHost = dtConfiguracion.Rows[0]["host"].ToString();
                        strRemitente = dtConfiguracion.Rows[0]["remitente"].ToString();
                        bEnableSsl = Convert.ToBoolean(dtConfiguracion.Rows[0]["enable_ssl"].ToString());
                        nPort = Convert.ToInt32(dtConfiguracion.Rows[0]["port"].ToString());
                    }

                    //Crear objetos para enviar correo.
                    System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

                    //Asignar variables de los objetos.
                    sMensaje.From = new System.Net.Mail.MailAddress(strRemitente);
                    sMensaje.To.Add(strMailTo);

                    sMensaje.Subject = strSubject;
                    sMensaje.Body = strMensaje;
                    sMensaje.IsBodyHtml = true;
                    //Cuenta generica para el envio de correos.
                    smtp.Host = strHost;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new System.Net.NetworkCredential(strUsuario, Utilerias.Encriptacion.Base64.DesencriptarBase64(strPassword));
                    smtp.Port = nPort;
                    smtp.EnableSsl = bEnableSsl;
                    smtp.Send(sMensaje);

                    bretorno = true;
                }
            }
        }
        catch (Exception ex)
        {
            bretorno = false;
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
    public bool EnviarCorreoPlantilla(string strMailTo, string strSubject, string strMensaje)
    {
        //bool bretorno = false;
        //clsPlantillaCorreo plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AltaProveedor);
        //try
        //{
        //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");
        //    DataTable dtConfiguracion = new DataTable("ConfiguracionCorreo");
        //    string strUsuario = string.Empty;
        //    string strPassword = string.Empty;
        //    string strHost = string.Empty;
        //    string strRemitente = string.Empty;
        //    bool bEnableSsl = false;
        //    int nPort = 0;
        //    iSql.Query("usp_Ctp_Cuenta_Envio_Sel", true, ref dtConfiguracion);
        //    if (dtConfiguracion.Rows.Count > 0)
        //    {
        //        strUsuario = dtConfiguracion.Rows[0]["usuario"].ToString();
        //        strPassword = dtConfiguracion.Rows[0]["password"].ToString();
        //        strHost = dtConfiguracion.Rows[0]["host"].ToString();
        //        bEnableSsl = Convert.ToBoolean(dtConfiguracion.Rows[0]["enable_ssl"].ToString());
        //        nPort = Convert.ToInt32(dtConfiguracion.Rows[0]["port"].ToString());
        //        strRemitente = dtConfiguracion.Rows[0]["remitente"].ToString();
        //    }

        //    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(strMensaje, null, "text/html");
        //    if (plantilla.logoImagen != null && plantilla.logoImagen.Length > 0)
        //    {
        //        MemoryStream msLogo = new MemoryStream(plantilla.logoImagen);
        //        LinkedResource lrLogo = new LinkedResource(msLogo);
        //        lrLogo.ContentId = "imgLogo";
        //        alternateView.LinkedResources.Add(lrLogo);
        //    }
        //    if (plantilla.firmaImagen != null && plantilla.firmaImagen.Length > 0)
        //    {
        //        MemoryStream msFirma = new MemoryStream(plantilla.firmaImagen);
        //        LinkedResource lrFirma = new LinkedResource(msFirma);
        //        lrFirma.ContentId = "imgFirma";

        //        alternateView.LinkedResources.Add(lrFirma);
        //    }
        //    //Crear objetos para enviar correo.
        //    System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
        //    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

        //    //Asignar variables de los objetos.
        //    sMensaje.From = new System.Net.Mail.MailAddress(strRemitente);
        //    sMensaje.To.Add(strMailTo);
        //    sMensaje.AlternateViews.Add(alternateView);
        //    sMensaje.Subject = strSubject;
        //    //sMensaje.Body = strMensaje;
        //    sMensaje.IsBodyHtml = true;
        //    //Cuenta generica para el envio de correos.
        //    smtp.Host = strHost;
        //    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    smtp.Credentials = new System.Net.NetworkCredential(strUsuario, Utilerias.Encriptacion.Base64.DesencriptarBase64(strPassword));
        //    smtp.Port = nPort;
        //    smtp.EnableSsl = bEnableSsl;
        //    smtp.Send(sMensaje);


        //    bretorno = true;

        //}
        //catch (Exception ex)
        //{
        //    bretorno = false;
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email);
        //}


        //return bretorno;

        bool bretorno = false;
        clsPlantillaCorreo plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AltaProveedor);
        try
        {
            DataTable dtConfiguracion = new DataTable("ConfiguracionCorreo");
            string strUsuario = string.Empty;
            string strPassword = string.Empty;
            string strHost = string.Empty;
            string strRemitente = string.Empty;
            bool bEnableSsl = false;
            int nPort = 0;

            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Cuenta_Envio_Sel";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtConfiguracion);
                    }
                    con.Close();
                    con.Dispose();

                    if (dtConfiguracion.Rows.Count > 0)
                    {
                        strUsuario = dtConfiguracion.Rows[0]["usuario"].ToString();
                        strPassword = dtConfiguracion.Rows[0]["password"].ToString();
                        strHost = dtConfiguracion.Rows[0]["host"].ToString();
                        bEnableSsl = Convert.ToBoolean(dtConfiguracion.Rows[0]["enable_ssl"].ToString());
                        nPort = Convert.ToInt32(dtConfiguracion.Rows[0]["port"].ToString());
                        strRemitente = dtConfiguracion.Rows[0]["remitente"].ToString();
                    }

                    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(strMensaje, null, "text/html");
                    if (plantilla.logoImagen != null && plantilla.logoImagen.Length > 0)
                    {
                        MemoryStream msLogo = new MemoryStream(plantilla.logoImagen);
                        LinkedResource lrLogo = new LinkedResource(msLogo);
                        lrLogo.ContentId = "imgLogo";
                        alternateView.LinkedResources.Add(lrLogo);
                    }
                    if (plantilla.firmaImagen != null && plantilla.firmaImagen.Length > 0)
                    {
                        MemoryStream msFirma = new MemoryStream(plantilla.firmaImagen);
                        LinkedResource lrFirma = new LinkedResource(msFirma);
                        lrFirma.ContentId = "imgFirma";

                        alternateView.LinkedResources.Add(lrFirma);
                    }
                    //Crear objetos para enviar correo.
                    System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

                    //Asignar variables de los objetos.
                    sMensaje.From = new System.Net.Mail.MailAddress(strRemitente);
                    sMensaje.To.Add(strMailTo);
                    sMensaje.AlternateViews.Add(alternateView);
                    sMensaje.Subject = strSubject;
                    //sMensaje.Body = strMensaje;
                    sMensaje.IsBodyHtml = true;
                    //Cuenta generica para el envio de correos.
                    smtp.Host = strHost;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new System.Net.NetworkCredential(strUsuario, Utilerias.Encriptacion.Base64.DesencriptarBase64(strPassword));
                    smtp.Port = nPort;
                    smtp.EnableSsl = bEnableSsl;
                    smtp.Send(sMensaje);


                    bretorno = true;
                }
            }
        }
        catch (Exception ex)
        {
            bretorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
            smtp.Credentials = new System.Net.NetworkCredential(clsComun.ObtenerParamentro("emailAppFrom"), Utilerias.Encriptacion.Classica.Desencriptar(clsComun.ObtenerParamentro("emailPassword")));
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
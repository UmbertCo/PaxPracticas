using System;
using PaxApplicationPoolService.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace PaxApplicationPoolService
{
    class EnvioCorreo
    {
        public static bool fnEnviarCorreo(string psMailTo, string psSubject, string psMensaje, string psCC, string psCO)
        {
            bool bretorno = false;
            try
            {
                //Crear objetos para enviar correo.
                System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

                //Asignar variables de los objetos.
                sMensaje.From = new System.Net.Mail.MailAddress(PaxApplicationPoolService.Properties.Settings.Default.emailAppFrom);
                sMensaje.To.Add(psMailTo);
                sMensaje.Subject = psSubject;
                sMensaje.Body = psMensaje;
                if (psCC != string.Empty)
                    sMensaje.CC.Add(psCC);
                if (psCO != string.Empty)
                    sMensaje.Bcc.Add(psCO);
                sMensaje.IsBodyHtml = true;

                //Cuenta generica para el envio de correos.
                smtp.Host = PaxApplicationPoolService.Properties.Settings.Default.emailHost;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(PaxApplicationPoolService.Properties.Settings.Default.emailAppFrom, PaxApplicationPoolService.Properties.Settings.Default.emailPassword);
                smtp.Port = Convert.ToInt32(PaxApplicationPoolService.Properties.Settings.Default.emailPort);
                smtp.EnableSsl = true;
                smtp.Send(sMensaje);
                sMensaje.Dispose();
                smtp.Dispose();

                bretorno = true;
            }
            catch (Exception ex)
            {
                bretorno = false;
                //clsLog.Escribir(Settings.Default.LogError + "LogError", DateTime.Now + " Error de Correo: " + ex.Message);
            }
            return bretorno;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;

namespace SolucionPruebas.Presentacion.ConsoleApplication.EnvioCorreo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enviar correo");
            Console.ReadLine();
            EnviarCorreo("Prueba", "Mensaje de prueba");
            Console.WriteLine("Finaliza correo");
            Console.ReadLine();
        }

        /// <summary>
        /// EnviarCorreo
        /// </summary>
        /// <param name="strMailTo">Lista de destinatarios separados por coma</param>
        /// <param name="strSubject">Asunto del correo</param>
        /// <param name="strMensaje">Mensaje del correo</param>
        /// <returns>Retorna un booleano indicando si el envío fue exitoso</returns>
        public static bool EnviarCorreo(string strSubject, string strMensaje)
        {
            bool bretorno = false;

            try
            {
                //Crear objetos para enviar correo.
                System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

                //Asignar variables de los objetos.
                sMensaje.From = new System.Net.Mail.MailAddress("ismael.hidalgo@paxfacturacion.com");
                sMensaje.To.Add("ismael.hidalgo@paxfacturacion.com");

                sMensaje.Subject = strSubject;
                sMensaje.Body = strMensaje;
                sMensaje.IsBodyHtml = true;
                //Cuenta generica para el envio de correos.
                smtp.Host = "smtp.gmail.com";
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential("ismael.hidalgo@paxfacturacion.com", "aia175671");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(sMensaje);


                bretorno = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                Console.ReadLine();
            }
            return bretorno;
        }
    }
}

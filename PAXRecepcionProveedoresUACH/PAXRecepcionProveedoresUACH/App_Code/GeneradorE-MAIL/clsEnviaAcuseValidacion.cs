using System;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;

    public class clsEnviaAcuseValidacion
    {
        /// <summary>
        /// Usuario para el login del servidor de correo electronico
        /// </summary>
        private string strUsuario { get; set; }
        /// <summary>
        /// Contraseña de la cuenta con la que se va a enviar el correo
        /// </summary>
        private string strPassword { get; set; }
        /// <summary>
        /// Nombre del host de correo
        /// </summary>
        private string strHost { get; set; }
        /// <summary>
        /// Indica si el servidor utiliza SSL
        /// </summary>
        private bool bEnableSsl { get; set; }
        /// <summary>
        /// Indica si se encontro una cuenta para el envio de correo
        /// </summary>
        public bool bCuentaActiva { get; set; }
        /// <summary>
        /// Puerto del servidor SMTP
        /// </summary>
        private int nPort { get; set; }
        /// <summary>
        /// Alias del correo
        /// </summary>
        private string strRemitente { get; set; }

        public clsEnviaAcuseValidacion()
        {
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");
            //    DataTable dtConfiguracion = new DataTable("ConfiguracionCorreo");

            //    iSql.Query("usp_Ctp_Cuenta_Envio_Sel", true, ref dtConfiguracion);

            //    if (dtConfiguracion.Rows.Count > 0)
            //    {
            //        strUsuario = dtConfiguracion.Rows[0]["usuario"].ToString();
            //        strPassword = dtConfiguracion.Rows[0]["password"].ToString();
            //        strHost = dtConfiguracion.Rows[0]["host"].ToString();
            //        bEnableSsl = Convert.ToBoolean(dtConfiguracion.Rows[0]["enable_ssl"].ToString());
            //        nPort = Convert.ToInt32(dtConfiguracion.Rows[0]["port"].ToString());
            //        strRemitente = dtConfiguracion.Rows[0]["remitente"].ToString();
            //        bCuentaActiva = true;
            //    }
            //    else
            //    {
            //        bCuentaActiva = false;
            //    }
            //}
            //catch
            //{
            //    bCuentaActiva = false;
            //}

            DataTable dtConfiguracion = new DataTable("ConfiguracionCorreo");
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
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
                            bEnableSsl = Convert.ToBoolean(dtConfiguracion.Rows[0]["enable_ssl"].ToString());
                            nPort = Convert.ToInt32(dtConfiguracion.Rows[0]["port"].ToString());
                            strRemitente = dtConfiguracion.Rows[0]["remitente"].ToString();
                            bCuentaActiva = true;
                        }
                        else
                        {
                            bCuentaActiva = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                bCuentaActiva = false;
            }

        }

        public void fnEnviarAcuse(DataTable dtComprobantes, int nIdSucursal)
        {
            //TODO: Obtener los correos a los que se enviara el acuse
            clsInicioSesionSolicitudReg busquedaUsuario = new clsInicioSesionSolicitudReg();
       
            clsInicioSesionUsuario usu = clsComun.fnUsuarioEnSesion();

            DataTable tabla = busquedaUsuario.buscarUsuario(usu.userName);
            string email = Convert.ToString(tabla.Rows[0]["email"]);

            clsPlantillaCorreo plantilla = new clsPlantillaCorreo();
            plantilla = new clsPlantillaCorreo(clsPlantillaCorreo.Tipo.AcuseValidacion);
            try
            {
                //Crear objetos para enviar correo.
                System.Net.Mail.MailMessage sMensaje = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();

                //Asignar variables de los objetos.
                //TODO: Obtener el correo del usuario que se encuentra logueado
                sMensaje.From = new MailAddress(this.strRemitente);
                //se Agrega el email del usuario en sesion
                sMensaje.To.Add(email);

                //TODO: Obtener los correos a donde se debe enviar el acuse para la empresa correspondiente
                // y adjuntarlos a sMensaje.To
                //ej.
                DataTable dtCorreos = new clsOperacionSucursales().fnObtenerCorreosSucursal(nIdSucursal);
                foreach (DataRow row in dtCorreos.Rows)
                {
                    sMensaje.To.Add(row["correo"].ToString());
                }
                string sBody = plantilla.fnObtenerMensajeAcuseValidacion(dtComprobantes,
                    (plantilla.logoImagen != null && plantilla.logoImagen.Length > 0),
                    (plantilla.firmaImagen != null && plantilla.firmaImagen.Length > 0));
                AlternateView alternateView = AlternateView.CreateAlternateViewFromString(sBody, null, "text/html");
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
                sMensaje.AlternateViews.Add(alternateView);
                sMensaje.Subject = plantilla.asunto;
                //sMensaje.Body = sBody;
                sMensaje.IsBodyHtml = true;
                //Cuenta generica para el envio de correos.
                smtp.Host = this.strHost;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(this.strUsuario, Utilerias.Encriptacion.Base64.DesencriptarBase64(this.strPassword));
                smtp.Port = this.nPort;
                smtp.EnableSsl = this.bEnableSsl;
                smtp.Send(sMensaje);
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Email);
            }
        }
        /// <summary>
        /// Genera el cuerpo del Acuse com formato HTML
        /// </summary>
        /// <param name="dtComprobantes">tabla con el resultado de la validacion</param>
        /// <returns></returns>
        private string fnGenerarCuerpoMensaje(DataTable dtComprobantes)
        {
            StringBuilder strCuerpoMensaje = new StringBuilder();
            int nRowCount = 0;
            string strRowColor = string.Empty;
            strCuerpoMensaje.Append("<div style=\"font-family: 'Century Gothic'\">");
            strCuerpoMensaje.Append("A continuación se detalla el resultado de la validación de comprobantes:");
            strCuerpoMensaje.Append("<br />");
            strCuerpoMensaje.AppendLine("<Table style=\"table-layout:fixed; overflow:hidden; white-space: nowrap;\">");
            strCuerpoMensaje.AppendLine("<tr>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:#336666\">");
            strCuerpoMensaje.AppendLine("Nombre");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:#336666\">");
            strCuerpoMensaje.AppendLine("Resultado");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:#336666\">");
            strCuerpoMensaje.AppendLine("Mensaje");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("</tr>");
            foreach (DataRow drComprobante in dtComprobantes.Rows)
            {
                if (nRowCount % 2 == 0)
                    strRowColor = " bgcolor=\"#D6E0E0\"";
                else
                    strRowColor = " bgcolor=\"#ADC2C2\"";

                if (!Convert.ToBoolean(drComprobante["valido"].ToString()))
                    strRowColor = " bgcolor=\"#FF9696\"";
                strCuerpoMensaje.AppendLine("<tr" + strRowColor + ">");
                strCuerpoMensaje.AppendLine("<td width=\"200px\">");
                strCuerpoMensaje.AppendLine(drComprobante["nombre_xml"].ToString());
                strCuerpoMensaje.AppendLine("</td>");
                strCuerpoMensaje.AppendLine("<td align=\"center\" width=\"100px\">");
                strCuerpoMensaje.AppendLine(Convert.ToBoolean(drComprobante["valido"].ToString()) ? "Valido" : "No Valido");
                strCuerpoMensaje.AppendLine("</td>");
                strCuerpoMensaje.AppendLine("<td width=\"400px\">");
                strCuerpoMensaje.AppendLine(drComprobante["error"].ToString());
                strCuerpoMensaje.AppendLine("</td>");
                strCuerpoMensaje.AppendLine("</tr>");
                nRowCount++;
            }
            strCuerpoMensaje.AppendLine("</Table>");
            strCuerpoMensaje.AppendLine("</div>");


            return strCuerpoMensaje.ToString();
        }

    }

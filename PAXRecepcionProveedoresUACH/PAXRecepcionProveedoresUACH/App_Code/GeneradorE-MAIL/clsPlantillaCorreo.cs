using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;


    public class clsPlantillaCorreo
    {
        public Tipo tipo { get; set; }
        public string asunto { get; set; }
        public string mensaje { get; set; }
        public string firma { get; set; }
        public string colorTablaEncabezado { get; set; }
        public string colorTablaLinea { get; set; }
        public string colorTablaLineaError { get; set; }
        public byte[] logoImagen { get; set; }
        public byte[] firmaImagen { get; set; }
        private string conSucursales = "conRecepcionProveedores";

        public clsPlantillaCorreo()
        {

        }

        public clsPlantillaCorreo(Tipo tipo, string asunto, string mensaje, string firma, string colorTablaEncabezado,
            string colorTablaLinea, string colorTablaLineaError, byte[] logoImagen, byte[] firmaImagen)
        {
            this.tipo = tipo;
            this.asunto = asunto;
            this.mensaje = mensaje;
            this.firma = firma;
            this.colorTablaEncabezado = colorTablaEncabezado;
            this.colorTablaLinea = colorTablaLinea;
            this.colorTablaLineaError = colorTablaLineaError;
            this.logoImagen = logoImagen;
            this.firmaImagen = firmaImagen;
        }
        
        public enum Tipo
        {
            AcusePago=1,
            AcuseValidacion,
            AltaProveedor

        }

        public string fnObtenerTipo(Tipo tipo)
        {
            switch (tipo)
            {
                case Tipo.AcusePago:
                    return "acuse_pago";
                case Tipo.AcuseValidacion:
                    return "acuse_validacion";
                case Tipo.AltaProveedor:
                    return "alta_proveedor";
                default: return "";
            }
        }
        public clsPlantillaCorreo(Tipo tipo)
        {
            //clsPlantillaCorreo plantilla = new clsPlantillaCorreo();
            //DataTable res = new DataTable();
            //try
            //{
            //    giSql = clsComun.fnCrearConexion(conSucursales);
            //    giSql.AgregarParametro("@sTipo", fnObtenerTipo(tipo));
            //    giSql.Query("usp_rfp_Plantilla_Correo_Sel", true, ref res);
            //    if (res.Rows.Count > 0)
            //    {
            //        this.tipo = tipo;
            //        asunto = res.Rows[0]["asunto"].ToString();
            //        mensaje = res.Rows[0]["mensaje"].ToString();
            //        firma = res.Rows[0]["firma"].ToString();
            //        colorTablaEncabezado = res.Rows[0]["color_tabla_encabezado"].ToString();
            //        colorTablaLinea = res.Rows[0]["color_tabla_linea"].ToString();
            //        colorTablaLineaError = res.Rows[0]["color_tabla_linea_error"].ToString();
            //        if(!string.IsNullOrEmpty(res.Rows[0]["logo_imagen"].ToString()))
            //            logoImagen = (byte[])res.Rows[0]["logo_imagen"];
            //        if (!string.IsNullOrEmpty(res.Rows[0]["firma_imagen"].ToString()))
            //            firmaImagen = (byte[])res.Rows[0]["firma_imagen"];
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
            clsPlantillaCorreo plantilla = new clsPlantillaCorreo();
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conSucursales].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_rfp_Plantilla_Correo_Sel";
                        cmd.Parameters.Add(new SqlParameter("sTipo", fnObtenerTipo(tipo)));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(res);
                        }

                        if (res.Rows.Count > 0)
                        {

                            this.tipo = tipo;
                            asunto = res.Rows[0]["asunto"].ToString();
                            mensaje = res.Rows[0]["mensaje"].ToString();
                            firma = res.Rows[0]["firma"].ToString();
                            colorTablaEncabezado = res.Rows[0]["color_tabla_encabezado"].ToString();
                            colorTablaLinea = res.Rows[0]["color_tabla_linea"].ToString();
                            colorTablaLineaError = res.Rows[0]["color_tabla_linea_error"].ToString();
                            if (!string.IsNullOrEmpty(res.Rows[0]["logo_imagen"].ToString()))
                                logoImagen = (byte[])res.Rows[0]["logo_imagen"];
                            if (!string.IsNullOrEmpty(res.Rows[0]["firma_imagen"].ToString()))
                                firmaImagen = (byte[])res.Rows[0]["firma_imagen"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }

        public void fnGuardarPlantilla(Tipo tipo, string asunto, string mensaje, string firma, string colorTablaEncabezado,
            string colorTablaLinea, string colorTablaLineaError, byte[] logoImagen, byte[] firmaImagen)
        {
            //try
            //{
            //    giSql = clsComun.fnCrearConexion(conSucursales);
            //    giSql.AgregarParametro("@sTipo",fnObtenerTipo(tipo));
            //    giSql.AgregarParametro("@sAsunto", asunto);
            //    giSql.AgregarParametro("@sMensaje", mensaje);
            //    giSql.AgregarParametro("@sFirma", firma);
            //    giSql.AgregarParametro("@sColorTablaEncabezado", colorTablaEncabezado);
            //    giSql.AgregarParametro("@sColorTablaLinea", colorTablaLinea);
            //    giSql.AgregarParametro("@sColorTablaLineaError", colorTablaLineaError);
            //    if(logoImagen!=null)
            //        giSql.AgregarParametro("@sLogoImagen", logoImagen);
            //    if(firmaImagen!=null)
            //        giSql.AgregarParametro("@sFirmaImagen", firmaImagen);
            //    giSql.NoQuery("usp_rfp_Plantilla_Correo_ins", true);

            //}
            //catch (Exception ex)
            //{

            //}

            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conSucursales].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_rfp_Plantilla_Correo_ins";
                        cmd.Parameters.Add(new SqlParameter("@sTipo", fnObtenerTipo(tipo)));
                        cmd.Parameters.Add(new SqlParameter("@sAsunto", asunto));
                        cmd.Parameters.Add(new SqlParameter("@sMensaje", mensaje));
                        cmd.Parameters.Add(new SqlParameter("@sFirma", firma));
                        cmd.Parameters.Add(new SqlParameter("@sColorTablaEncabezado", colorTablaEncabezado));
                        cmd.Parameters.Add(new SqlParameter("@sColorTablaLinea", colorTablaLinea));
                        cmd.Parameters.Add(new SqlParameter("@sColorTablaLineaError", colorTablaLineaError));
                        if (logoImagen != null)
                            cmd.Parameters.Add(new SqlParameter("@sLogoImagen", logoImagen));
                        if (firmaImagen != null)
                            cmd.Parameters.Add(new SqlParameter("@sFirmaImagen", firmaImagen));
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }

        public string fnObtenerMensajeAcuseValidacion(DataTable dtComprobantes, bool bImgLogo, bool bImgFirma)
        {
            StringBuilder strCuerpoMensaje = new StringBuilder();
            int nRowCount = 0;
            string strRowColor = string.Empty;
            if(bImgLogo)
            strCuerpoMensaje.Append("<img src=cid:imgLogo>");
            strCuerpoMensaje.Append("<br />");
            strCuerpoMensaje.Append("<br />");
            strCuerpoMensaje.Append("<div style=\"font-family: 'Century Gothic'\">");
            strCuerpoMensaje.Append(mensaje);
            strCuerpoMensaje.AppendLine("<Table style=\"table-layout:fixed; overflow:hidden; white-space: nowrap;\">");
            strCuerpoMensaje.AppendLine("<tr>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:"+colorTablaEncabezado+"\">");
            strCuerpoMensaje.AppendLine("Nombre");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("Resultado");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
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
                strCuerpoMensaje.AppendLine(Convert.ToBoolean(drComprobante["valido"].ToString()) ? "Válido" : "No Válido");
                strCuerpoMensaje.AppendLine("</td>");
                strCuerpoMensaje.AppendLine("<td width=\"400px\">");
                strCuerpoMensaje.AppendLine(drComprobante["error"].ToString());
                strCuerpoMensaje.AppendLine("</td>");
                strCuerpoMensaje.AppendLine("</tr>");
                nRowCount++;
            }
            strCuerpoMensaje.AppendLine("</Table>");
            strCuerpoMensaje.AppendLine("</div>");
            strCuerpoMensaje.AppendLine("<div style=\"font-family: 'Century Gothic';\">" + firma+"</div>");
            if(bImgFirma)
            strCuerpoMensaje.Append("<img src=cid:imgFirma>");

            return strCuerpoMensaje.ToString();
        }

        public string fnObtenerMensajeAcusePago(DataTable dtComprobantes, bool bImgLogo, bool bImgFirma)
        {
            StringBuilder strCuerpoMensaje = new StringBuilder();
            int nRowCount = 0;
            string strRowColor = string.Empty;
            if(bImgLogo)
                strCuerpoMensaje.Append("<img src=cid:imgLogo>");
            strCuerpoMensaje.AppendLine("<br />");
            strCuerpoMensaje.AppendLine("<br />");
            strCuerpoMensaje.AppendLine("<div style=\"font-family: 'Century Gothic'\">");
            strCuerpoMensaje.AppendLine(mensaje);
            strCuerpoMensaje.AppendLine("<br />");
            strCuerpoMensaje.AppendLine("<Table style=\"table-layout:fixed; overflow:hidden; white-space: nowrap;\">");
            strCuerpoMensaje.AppendLine("<tr>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("Emisor");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("Receptor");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("Serie");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("Folio");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("UUID");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("Total");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("Estatus");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("Fecha Documento");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("<th style=\"font-family: 'Century Gothic'; color: white; background-color:" + colorTablaEncabezado + "\">");
            strCuerpoMensaje.AppendLine("Fecha Pago");
            strCuerpoMensaje.AppendLine("</th>");
            strCuerpoMensaje.AppendLine("</tr>");
            DataRow drComprobante = dtComprobantes.Rows[0];
            if (nRowCount % 2 == 0)
                strRowColor = " bgcolor=\"#D6E0E0\"";
            else
                strRowColor = " bgcolor=\"#ADC2C2\"";


            strCuerpoMensaje.AppendLine("<tr" + strRowColor + ">");
            strCuerpoMensaje.AppendLine("<td width=\"200px\">");
            strCuerpoMensaje.AppendLine(drComprobante["nombre_emisor"].ToString());
            strCuerpoMensaje.AppendLine("</td>");
            strCuerpoMensaje.AppendLine("<td width=\"200px\">");
            strCuerpoMensaje.AppendLine(drComprobante["nombre_receptor"].ToString());
            strCuerpoMensaje.AppendLine("</td>");
            strCuerpoMensaje.AppendLine("<td width=\"200px\">");
            strCuerpoMensaje.AppendLine(drComprobante["serie"].ToString());
            strCuerpoMensaje.AppendLine("</td>");
            strCuerpoMensaje.AppendLine("<td width=\"200px\">");
            strCuerpoMensaje.AppendLine(drComprobante["folio"].ToString());
            strCuerpoMensaje.AppendLine("</td>");
            strCuerpoMensaje.AppendLine("<td width=\"200px\">");
            strCuerpoMensaje.AppendLine(drComprobante["uuid"].ToString());
            strCuerpoMensaje.AppendLine("</td>");
            strCuerpoMensaje.AppendLine("<td width=\"200px\">");
            strCuerpoMensaje.AppendLine(drComprobante["total"].ToString());
            strCuerpoMensaje.AppendLine("</td>");
            strCuerpoMensaje.AppendLine("<td width=\"200px\">");
            strCuerpoMensaje.AppendLine(drComprobante["status"].ToString());
            strCuerpoMensaje.AppendLine("</td>");
            strCuerpoMensaje.AppendLine("<td width=\"200px\">");
            strCuerpoMensaje.AppendLine(Convert.ToDateTime(drComprobante["fecha_documento"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"));
            strCuerpoMensaje.AppendLine("</td>");
            strCuerpoMensaje.AppendLine("<td width=\"200px\">");
            string sFechaPago = string.Empty;
            if(!string.IsNullOrEmpty(drComprobante["fecha_pago"].ToString()))
                sFechaPago =  Convert.ToDateTime(drComprobante["fecha_pago"].ToString()).ToString("dd/MM/yyyy");
            strCuerpoMensaje.AppendLine(sFechaPago);
            strCuerpoMensaje.AppendLine("</td>");

            strCuerpoMensaje.AppendLine("</tr>");
            nRowCount++;

            strCuerpoMensaje.AppendLine("</Table>");
            strCuerpoMensaje.AppendLine("</div>");

            strCuerpoMensaje.AppendLine("<div style=\"font-family: 'Century Gothic';\">" + firma + "</div>");
            if(bImgFirma)
            strCuerpoMensaje.Append("<img src=cid:imgFirma>");

            return strCuerpoMensaje.ToString();
        }


        public string fnObtenerMensajeAltaProveedor(string sNombre, string sUsuario, string sPassword, bool bImgLogo, bool bImgFirma)
        {
            StringBuilder strMensaje = new StringBuilder();
            if(bImgLogo)
                strMensaje.Append("<img src=cid:imgLogo>");
            strMensaje.Append("<br />");
            strMensaje.Append("<br />");
            strMensaje.Append("<table>");
            strMensaje.Append("<tr><td colspan='2'><b>A nuestro proveedor: " + sNombre + "</b></td></tr>");
            strMensaje.Append("<tr><td colspan='2'>Se ha registrado en nuestro portal.</td></tr>");
            strMensaje.Append("<tr><td colspan='2'>Para completar el registro, inicie sesión con los siguientes datos.</td></tr>");
            strMensaje.Append("<tr><td><b>Usuario:</b></td><td>" + sUsuario + "</td></tr>");
            strMensaje.Append("<tr><td><b>Contraseña temporal:</b></td><td>" + sPassword + "</td></tr>");
            strMensaje.Append("<tr><td><a href='" + clsComun.ObtenerParamentro("urlPortal") + "'>" + clsComun.ObtenerParamentro("urlPortal") + "</a></td></tr>");
            strMensaje.Append("</table>");
            strMensaje.AppendLine("<div style=\"font-family: 'Century Gothic';\">" + firma + "</div>");
            if(bImgFirma)
            strMensaje.Append("<img src=cid:imgFirma>");
            return strMensaje.ToString();
        }

        public DataTable fnObtenerTablaEjemplo(Tipo tipo)
        {
            DataTable dtEjemplo = new DataTable();
            if (tipo == Tipo.AcusePago)
            {
                dtEjemplo.Columns.Add("nombre_emisor");
                dtEjemplo.Columns.Add("nombre_receptor");
                dtEjemplo.Columns.Add("serie");
                dtEjemplo.Columns.Add("folio");
                dtEjemplo.Columns.Add("uuid");
                dtEjemplo.Columns.Add("total");
                dtEjemplo.Columns.Add("status");
                dtEjemplo.Columns.Add("fecha_documento");
                dtEjemplo.Columns.Add("fecha_pago");

                DataRow drEjemplo = dtEjemplo.NewRow();
                drEjemplo["nombre_emisor"] = "Emisor Ejemplo";
                drEjemplo["nombre_receptor"] = "Receptor Ejemplo";
                drEjemplo["serie"] = "A";
                drEjemplo["folio"] = "1";
                drEjemplo["uuid"] = "UUID_Ejemplo";
                drEjemplo["total"] = "0.00";
                drEjemplo["status"] = "Programada";
                drEjemplo["fecha_documento"] = "01/01/2000";
                drEjemplo["fecha_pago"] = "01/01/2000";
                dtEjemplo.Rows.Add(drEjemplo);
            }
            else if (tipo == Tipo.AcuseValidacion)
            {
                dtEjemplo.Columns.Add("nombre_xml");
                dtEjemplo.Columns.Add("valido");
                dtEjemplo.Columns.Add("error");
                

                DataRow drEjemplo = dtEjemplo.NewRow();
                drEjemplo["nombre_xml"] = "archivo.xml";
                drEjemplo["valido"] = true;
                drEjemplo["error"] = "Sin Errores";
               
                dtEjemplo.Rows.Add(drEjemplo);

                drEjemplo = dtEjemplo.NewRow();
                drEjemplo["nombre_xml"] = "archivo2.xml";
                drEjemplo["valido"] = true;
                drEjemplo["error"] = "Sin Errores";

                dtEjemplo.Rows.Add(drEjemplo);

                drEjemplo = dtEjemplo.NewRow();
                drEjemplo["nombre_xml"] = "archivo3.xml";
                drEjemplo["valido"] = false;
                drEjemplo["error"] = "Error Ejemplo";

                dtEjemplo.Rows.Add(drEjemplo);
            }

            return dtEjemplo;
        }
    }

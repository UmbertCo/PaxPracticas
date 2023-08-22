using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Net;
using wsPracticaTeamFoundation.Properties;
using wsPracticaTeamFundation;

namespace wsPracticaTeamFoundation
{
    class clsValidacion
    {
        public void fnValidacion(List<string> lsLista)
        {
            string sErrores = Settings.Default.rutaErrores;
            string sLog = Settings.Default.rutaLog;
            string sRespuesta = string.Empty;
            string version = string.Empty;
            string sUUID = string.Empty;
            string sSalida = Settings.Default.rutaSalida;
            XmlDocument xComprobante = new XmlDocument();
            wsValidacion.wcfValidaASMXSoapClient wsValida = new wsValidacion.wcfValidaASMXSoapClient ();
            DataTable TablaUUID = new DataTable();
            DataRow nuevo = null;
            DataColumn columna1 = new DataColumn();
            DataColumn columna2 = new DataColumn();
            StreamWriter sr4 = null;
            DateTime fecha = DateTime.Today;

            columna1.DataType = System.Type.GetType("System.String");
            columna1.AllowDBNull = true;
            columna1.Caption = "UUID";
            columna1.ColumnName = "UUID";
            columna1.DefaultValue = null;
            TablaUUID.Columns.Add(columna1);

            columna2.DataType = System.Type.GetType("System.String");
            columna2.AllowDBNull = true;
            columna2.Caption = "Mensaje";
            columna2.ColumnName = "Mensaje";
            columna2.DefaultValue = null;
            TablaUUID.Columns.Add(columna2);

            foreach (string xml in lsLista)
            {
                xComprobante.LoadXml(xml);
                version = fnObtenerVersion(xComprobante);

            if (version.StartsWith("2"))
            {
                sRespuesta = wsValida.fnValidaCFD(xml, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", version);
            }
            if (version.StartsWith("3"))
            {
                sRespuesta = wsValida.fnValidaXML(xml, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", version);
            }

            sUUID = fnObtenerUUID(xComprobante);

            clsLog.Escribir(fecha.ToString() + " " + sRespuesta);

            nuevo = TablaUUID.NewRow();
            nuevo["UUID"] = sUUID;
            nuevo["Mensaje"] = sRespuesta;
            TablaUUID.Rows.Add(nuevo);

            try
            {
                if (sRespuesta.ToUpper().Contains("SIN ERRORES"))
                {
                    xComprobante.Save(sSalida + sUUID + ".xml");
                    sr4 = new StreamWriter(sSalida + sUUID + ".txt");
                }
                else
                {
                    xComprobante.Save(sErrores + sUUID + ".xml");
                    sr4 = new StreamWriter(sErrores + sUUID + ".txt");
                }
                    sr4.WriteLine(sRespuesta);
                    sr4.Close();
            }
            catch (Exception ex)
             {
                    clsLog.Escribir(ex.ToString());
             }

            fnEnviarCorreo(TablaUUID);

            }
        }

        private string fnObtenerVersion(XmlDocument doc)
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(doc.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");

            string version = string.Empty;
            try
            {
                version = doc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
            }
            catch { }

            return version;
        }
        private string fnObtenerUUID(XmlDocument xXml)
        {
            string sUUID = string.Empty;
            XPathNavigator navEncabezado = null;
            XmlNamespaceManager nsmComprobante = null;

            navEncabezado = xXml.CreateNavigator();
            nsmComprobante = new XmlNamespaceManager(xXml.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            try { nsmComprobante.AddNamespace("donat", "http://www.sat.gob.mx/donat"); }
            catch { }

            try { sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
            catch { }
            return sUUID;
        }
        private void fnEnviarCorreo(DataTable dtTabla)
        {
            string stabla = string.Empty;

            stabla = "<table><tr><td>UUID</td><td>Mensaje</td></tr>";
            foreach (DataRow row in dtTabla.Rows)
            {
                stabla += "<tr><td>" + row["UUID"].ToString() + "</td><td>" + row["Mensaje"].ToString() + "</td></tr>";
            }
            stabla += "</table>";

            try
            {

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("gabriel.reyes@paxfacturacion.com", "3A4Xhlah");

                MailMessage email = new MailMessage();
                email.To.Add(new MailAddress(Settings.Default.Correo));
                email.From = new MailAddress("example2@example.com");
                email.Subject = "Validación de comprobantes";
                email.Body = stabla;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.Normal;

                smtp.Send(email);
                email.Dispose();
            }
            catch(Exception ex){
                clsLog.Escribir(ex.ToString());
            }
        }
    }
}

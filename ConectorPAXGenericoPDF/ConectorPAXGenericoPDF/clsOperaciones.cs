using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Data.SqlTypes;

namespace ConectorPAXGenericoPDF
{
    class clsOperaciones
    {
        public static bool fnActualizarImpresion(string sUUID, bool sStatus)
        {
            int sStatus2 = 0;
            if (sStatus == true)
            {
                sStatus2 = 1;
            }
            bool sRetorno = false;
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64((String)ConectorPAXGenericoPDF.Properties.Settings.Default.conNaya)))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("usp_cfd_Comprobante_Upd_Impresion", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("nUUID", sUUID);
                        cmd.Parameters.AddWithValue("nStatus", sStatus2);
                        con.Open();
                        cmd.ExecuteScalar();
                        sRetorno = true;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar el comprobante para actualizar." + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return sRetorno;
        }

        public static bool fnActualizarImpresionPrim(int nID_Cfd, bool sStatus)
        {
            int sStatus2 = 0;
            if (sStatus == true)
            {
                sStatus2 = 1;
            }
            bool sRetorno = false;
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64((String)ConectorPAXGenericoPDF.Properties.Settings.Default.conNaya)))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("usp_cfd_Comprobante_Upd_Impresion_Prim", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("nId_Cfd", nID_Cfd);
                        cmd.Parameters.AddWithValue("nStatus", sStatus2);
                        con.Open();
                        cmd.ExecuteScalar();
                        sRetorno = true;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar el comprobante para actualizar (P)." + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return sRetorno;
        }

        public static string fnInsertaAddenda(int nID, XmlDocument xmlAddenda)
        {
            string sRetorno = "";
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64((String)ConectorPAXGenericoPDF.Properties.Settings.Default.conNaya)))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Cfd_InsertaAddenda_ins", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("nIdCfd", nID);
                        cmd.Parameters.AddWithValue("sAddenda", xmlAddenda.OuterXml);
                        con.Open();
                        sRetorno = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar el comprobante para addenda." + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return sRetorno;
        }

        public static int InsertarComprobante(XmlDocument xDocTimbrado, bool bImpreso, string HASH)
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64((String)ConectorPAXGenericoPDF.Properties.Settings.Default.conNaya)))
            {
                con.Open();

                int retVal = 0;
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        XmlNamespaceManager nsmComprobanteDP = new XmlNamespaceManager(xDocTimbrado.NameTable);
                        nsmComprobanteDP.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                        nsmComprobanteDP.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                        
                        //--------------------------------------------------------------------
                        string sUUID = string.Empty;
                        string sSerie = string.Empty;
                        string sfolio = string.Empty;
                        string sTotal = string.Empty;
                        string sMoneda = string.Empty;
                        string sRfcEmisor = string.Empty;
                        string sRfcReceptor = string.Empty;
                        string sEmisorNombre = string.Empty;
                        string sFecha_emision = string.Empty;
                        string sFechaTimbrado = string.Empty;
                        string sReceptorNombre = string.Empty;
                        //--------------------------------------------------------------------
                        try { sUUID = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteDP).Value; }
                        catch { }
                        try { sFechaTimbrado = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobanteDP).Value; }
                        catch { }
                        //--------------------------------------------------------------------
                        try { sSerie = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobanteDP).Value; }
                        catch { }
                        try { sfolio = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobanteDP).Value; }
                        catch { }
                        try { sTotal = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobanteDP).Value; }
                        catch { }
                        try { sMoneda = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobanteDP).Value; }
                        catch { }
                        try { sRfcEmisor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobanteDP).Value; }
                        catch { }
                        try { sRfcReceptor = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobanteDP).Value; }
                        catch { }
                        try { sEmisorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobanteDP).Value; }
                        catch { }
                        try { sFecha_emision = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobanteDP).Value; }
                        catch { }
                        try { sReceptorNombre = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobanteDP).Value; }
                        catch { }
                        //--------------------------------------------------------------------

                        
                            using (SqlCommand command = new SqlCommand("usp_Timbrado_InsertaComprobante_Ins", con))
                            {
                                command.Transaction = tran;
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                command.CommandTimeout = 200;
                                command.Parameters.AddWithValue("sXML", xDocTimbrado.DocumentElement.OuterXml);
                                command.Parameters.AddWithValue("@nId_tipo_documento", "1");
                                command.Parameters.AddWithValue("@cEstatus", "A");
                                command.Parameters.AddWithValue("@dFecha_Documento", DateTime.Now);
                                command.Parameters.AddWithValue("@nId_estructura", "1");
                                command.Parameters.AddWithValue("@nId_usuario_timbrado", "1");
                                command.Parameters.AddWithValue("@nSerie", "N/A");
                                command.Parameters.AddWithValue("@sOrigen", "R");
                                command.Parameters.AddWithValue("@sHash", ""); 
                                command.Parameters.AddWithValue("@sDatos", HASH); // BUENO EMISOR
                                command.Parameters.AddWithValue("@sUUID", sUUID);
                                command.Parameters.AddWithValue("@dFecha_Timbrado", sFechaTimbrado);
                                command.Parameters.AddWithValue("@sRFC_Emisor", sRfcEmisor);
                                command.Parameters.AddWithValue("@sNombre_Emisor", sEmisorNombre);
                                command.Parameters.AddWithValue("@sRFC_Receptor", sRfcReceptor);
                                command.Parameters.AddWithValue("@sNombre_Receptor", sReceptorNombre);
                                command.Parameters.AddWithValue("@dFecha_Emision", sFecha_emision);
                                command.Parameters.AddWithValue("@sSerie", sSerie);
                                command.Parameters.AddWithValue("@sFolio", sfolio);
                                command.Parameters.AddWithValue("@nTotal", sTotal);
                                command.Parameters.AddWithValue("@sMoneda", sMoneda);
                                command.Parameters.AddWithValue("@sbImpreso", bImpreso);

                                retVal = Convert.ToInt32(command.ExecuteScalar());
                            }
                        

                        tran.Commit();

                        if (retVal == 0)
                        {
                            DateTime Fechaex = DateTime.Today;

                            string pathex = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";

                            clsLog.Escribir(pathex, "Error durante el registro del comprobante");
                        }
                        else
                        {
                           /* clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso27 - " + "Recupera XML para regresarlo al cliente.");

                            string uuid = gNodoTimbre.UUID;
                            clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, uuid.ToUpper(), sDocumentoDP, DateTime.Now, "0", "Response", "E", string.Empty);*/
                            //*******************************************************Insertar Response en tabla de acuses
                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        DateTime Fechaex = DateTime.Today;

                        string pathex = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";

                        clsLog.Escribir(pathex, "Error durante el registro del comprobante: " + ex.Message);

                        //return Encoding.UTF8.GetBytes(clsComun.fnRecuperaErrorSAT("999", "Timbrado"));
                    }
                    finally
                    {
                        con.Close();
                    }
                    return retVal;
                }
            }
        }

        public static string fnBuscarHashCompXML(string HASH)
        {
            string sRetorno = "";
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64((String)ConectorPAXGenericoPDF.Properties.Settings.Default.conNaya)))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Timbrado_BuscaHASH_XML_Sel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("sHash", HASH);
                        con.Open();
                        sRetorno = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al buscar hash del comprobante." + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            return sRetorno;
        }

        public static string GetHASH(string text)
        {
            byte[] hashValue;
            byte[] message = Encoding.UTF8.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }






    }
}

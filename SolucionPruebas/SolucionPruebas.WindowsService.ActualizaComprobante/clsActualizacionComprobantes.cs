using SolucionPruebas.WindowsService.ActualizaComprobante.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.XPath;

namespace SolucionPruebas.WindowsService.ActualizaComprobante
{
    public static class clsActualizacionComprobantes
    {
        //static int gnLimiteInferior;
        //static int gnLimiteSuperior;

        public static void fnActualizarComprobantes()
        {
            bool bBandera = true;
            //gnLimiteInferior = (gnLimiteInferior.Equals(0) ? Convert.ToInt32(Settings.Default.LimiteInferior) : gnLimiteInferior);
            //gnLimiteSuperior = gnLimiteInferior + Convert.ToInt32(Settings.Default.RegistrosPagina);

            while (bBandera)
            {
                DataTable dtComprobantes = new DataTable();
                try
                {
                    dtComprobantes = fnObtenerComprobantes();
                    if (dtComprobantes.Rows.Count.Equals(0))
                    {
                        clsLog.fnEscribir(Settings.Default.LogError + "LogError" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                        DateTime.Now + string.Format("No se encontraron comprobantes {0} resultados", dtComprobantes.Rows.Count));
                        bBandera = false;
                        return;
                    }

                    clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                        string.Format("Comprobantes {0} resultados", dtComprobantes.Rows.Count));

                    foreach (DataRow drRenglon in dtComprobantes.Rows)
                    {
                        XmlDocument xdComprobante = new XmlDocument();
                        XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());

                        xdComprobante.LoadXml(drRenglon["xml"].ToString());

                        nsm = new XmlNamespaceManager(xdComprobante.NameTable);
                        nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                        nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                        nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                        int nId_Cfd = 0;
                        string sUUID = string.Empty;
                        string sFechaTimbrado = string.Empty;
                        string sRfcReceptor = string.Empty;
                        string sNombreReceptor = string.Empty;
                        string sRfcEmisor = string.Empty;
                        string sNombreEmisor = string.Empty;
                        string sFechaEmision = string.Empty;
                        string sSerie = string.Empty;
                        string sFolio = string.Empty;
                        string sTotal = string.Empty;
                        string sMoneda = string.Empty;

                        nId_Cfd = Convert.ToInt32(drRenglon["id_cfd"].ToString());
                        try { sUUID = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value; }
                        catch { }
                        try { sFechaTimbrado = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsm).Value; }
                        catch { }
                        try { sRfcReceptor = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsm).Value; }
                        catch { }
                        try { sNombreReceptor = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsm).Value; }
                        catch { }
                        try { sRfcEmisor = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsm).Value; }
                        catch { }
                        try { sNombreEmisor = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsm).Value; }
                        catch { }
                        try { sFechaEmision = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsm).Value; }
                        catch { }
                        try { sSerie = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", nsm).Value; }
                        catch { }
                        try { sFolio = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsm).Value; }
                        catch { }
                        try { sTotal = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsm).Value; }
                        catch { }
                        try { sMoneda = xdComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@Moneda", nsm).Value; }
                        catch { }

                        fnActualizarComprobantes(nId_Cfd, sUUID, sFechaTimbrado, sRfcReceptor, sNombreReceptor, sRfcEmisor, sNombreEmisor,
                            sFechaEmision, sSerie, sFolio, sTotal, sMoneda);
                    }

                    clsLog.fnEscribir(Settings.Default.Log + "Log" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                        "Terminado " + DateTime.Now);

                }
                catch (Exception ex)
                {
                    clsLog.fnEscribir(Settings.Default.LogError + "LogError" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                        "Error al actualizar los comprobantes: " + ex.Message);
                }

                System.Threading.Thread.Sleep(60000);
            }
        }

        private static DataTable fnObtenerComprobantes(int pnLimiteInferior, int pnLimiteSuperior)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                string cadena = ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString;
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena)))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_cfd_Comprobantes_ObtenerRango_sel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("nLimiteInferior", pnLimiteInferior);
                        //cmd.Parameters.AddWithValue("nLimiteSuperior", pnLimiteSuperior);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtResultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.fnEscribir(Settings.Default.LogError + "LogError" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, 
                    DateTime.Now + "Error al obtener los comprobante. Limite inferior: " + pnLimiteInferior + ", Limite superior: " + pnLimiteSuperior + " ." + ex.Message);
            }
            return dtResultado;
        }

        private static DataTable fnObtenerComprobantes()
        {
            DataTable dtResultado = new DataTable();
            try
            {
                string cadena = ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString;
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena)))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_cfd_Comprobantes_ObtenerRango_sel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtResultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsLog.fnEscribir(Settings.Default.LogError + "LogError" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year,
                    DateTime.Now + "Error al obtener los comprobante. " + ex.Message);
            }
            return dtResultado;
        }

        private static void fnActualizarComprobantes(int pnId_Cfd, string psUUID, string psFechaTimbrado, string psRfcReceptor,
                                                    string psNombreReceptor, string psRfcEmisor, string psNombreEmisor, string psFechaEmision,
                                                    string psSerie, string psFolio, string psTotal, string psMoneda)
        {
            int nFilasAfectadas = 0;
            using (SqlConnection scConexion = new SqlConnection())
            {
                try
                {
                    scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString);
                    scConexion.Open();

                    using (SqlCommand scoComando = new SqlCommand("usp_cfd_Comprobantes_Valores_upd", scConexion))
                    {
                        scoComando.Connection = scConexion;
                        scoComando.CommandType = System.Data.CommandType.StoredProcedure;

                        scoComando.Parameters.AddWithValue("nid_cfd", pnId_Cfd);
                        scoComando.Parameters.AddWithValue("sUUID", psUUID);
                        scoComando.Parameters.AddWithValue("sFechaTimbrado", psFechaTimbrado);
                        scoComando.Parameters.AddWithValue("sRfcReceptor", psRfcReceptor);
                        scoComando.Parameters.AddWithValue("sRfcEmisor", psRfcEmisor);
                        scoComando.Parameters.AddWithValue("sFechaEmision", psFechaEmision);
                        scoComando.Parameters.AddWithValue("sTotal", psTotal);
                        scoComando.Parameters.AddWithValue("sMoneda", psMoneda);

                        if (!string.IsNullOrEmpty(psNombreReceptor))
                            scoComando.Parameters.AddWithValue("sNombreReceptor", psNombreReceptor);

                        if (!string.IsNullOrEmpty(psNombreEmisor))
                            scoComando.Parameters.AddWithValue("sNombreEmisor", psNombreEmisor);

                        if (!string.IsNullOrEmpty(psSerie))
                            scoComando.Parameters.AddWithValue("sSerie", psSerie);

                        if (!string.IsNullOrEmpty(psFolio))
                            scoComando.Parameters.AddWithValue("sFolio", psFolio);

                        nFilasAfectadas = scoComando.ExecuteNonQuery();

                        if (nFilasAfectadas.Equals(0))
                        {
                            throw new Exception("No se actualizaron los datos.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsLog.fnEscribir(Settings.Default.LogError + "LogError" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, 
                        DateTime.Now + "Error al actualizar el comprobante " + pnId_Cfd + " " + ex.Message);
                }
            }
        }
    }
}

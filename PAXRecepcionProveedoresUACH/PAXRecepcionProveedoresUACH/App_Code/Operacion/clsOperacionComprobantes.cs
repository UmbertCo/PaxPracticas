using System;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.Data.SqlClient;
using System.Globalization;
using System.Configuration;

    public class clsOperacionComprobantes
    {
        /// <summary>
        /// Obtiene el catálogo de estatus del comprobante
        /// </summary>
        /// <returns></returns>
        public DataTable fnObtenerStatus()
        {
            //DataTable dtStatus = new DataTable();
            //try
            //{
            //    giSql = clsComun.fnCrearConexion(conexion);
            //    giSql.Query("usp_cfd_Obtener_Status_sel",true,ref dtStatus);


            //}
            //catch (Exception ex)
            //{
                
            //}
            //return dtStatus;

            DataTable dtStatus = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Obtener_Status_sel";
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtStatus);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }

            return dtStatus;
        }
        /// <summary>
        /// Cambia el estatus del comprobante
        /// </summary>
        /// <param name="nIdComprobante"></param>
        /// <param name="nIdStatus"></param>
        public void fnCambiarStatus(int nIdComprobante, int nIdStatus, string sFechaPago)
        {
            //try
            //{
            //    giSql = clsComun.fnCrearConexion(conexion);
            //    giSql.AgregarParametro("nIdStatus", nIdStatus);
            //    giSql.AgregarParametro("nIdComprobante", nIdComprobante);
            //    if (!string.IsNullOrEmpty(sFechaPago))
            //        // giSql.AgregarParametro("dFechaPago", Convert.ToDateTime(sFechaPago));
            //        giSql.AgregarParametro("dFechaPago", DateTime.ParseExact(sFechaPago, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            //    giSql.Query("usp_cfd_Cambiar_Status_Comprobante_upd", true);
            //}
            //catch (Exception ex)
            //{

            //}

            //error raro procedimiento almacenado intenta insertar en tabla pero no tiene id auto incrementable
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Cambiar_Status_Comprobante_upd";
                        cmd.Parameters.Add(new SqlParameter("nIdStatus", nIdStatus));
                        cmd.Parameters.Add(new SqlParameter("nIdComprobante", nIdComprobante));
                        if (!string.IsNullOrEmpty(sFechaPago))
                            cmd.Parameters.Add(new SqlParameter("dFechaPago", DateTime.ParseExact(sFechaPago, "dd/MM/yyyy", CultureInfo.InvariantCulture)));

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
        /// <summary>
        /// Verifica si existe el comprobante
        /// Si el UUID viene nulo, lo busca con el folio, serie y RFC del emisor
        /// </summary>
        /// <param name="sUUID"></param>
        /// <param name="sRfc"></param>
        /// <param name="sSerie"></param>
        /// <param name="sFolio"></param>
        /// <returns></returns>
        public bool fnComprobanteExiste(XmlDocument xXml)
        {
            string sUUID = string.Empty;
            string sRfc = string.Empty;
            string sSerie = string.Empty;
            int nFolio = 0;
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xXml.NameTable);
            nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navComprobante = xXml.CreateNavigator();
            string sVersionDoc = string.Empty;
            //Verfica la versión del comprobante
            try
            {
                sVersionDoc = navComprobante.SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value;
            }
            catch
            {
                try
                {
                    sVersionDoc = navComprobante.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //Si es versión 3 , obtiene el UUID
            if (sVersionDoc.StartsWith("3"))
            {
                try
                {
                    sUUID = navComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;
                }
                catch
                {
                    sUUID = string.Empty;
                }
            }
                // Si es versión 2, obtiene el folio, serie y rfc del emisor
            else if(sVersionDoc.StartsWith("2"))
            {
                try
                {
                    sRfc = navComprobante.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@rfc", nsmComprobante).Value;
                }
                catch
                {
                    sRfc = string.Empty;
                }
                try
                {
                    sSerie = navComprobante.SelectSingleNode("/cfd:Comprobante/@serie", nsmComprobante).Value;
                }
                catch
                {
                    sSerie = string.Empty;
                }
                try
                {
                    nFolio = Convert.ToInt32(navComprobante.SelectSingleNode("/cfd:Comprobante/@folio", nsmComprobante).Value);
                }
                catch
                {
                    nFolio = 0;
                }
            }
            bool res = false;
            //try
            //{
            //    giSql = clsComun.fnCrearConexion(conexion);
            //    if(!string.IsNullOrEmpty(sUUID))
            //        giSql.AgregarParametro("sUUID", sUUID);
            //    if(!string.IsNullOrEmpty(sRfc))
            //        giSql.AgregarParametro("sRfcEmisor", sRfc);
            //    if (!string.IsNullOrEmpty(sSerie))
            //        giSql.AgregarParametro("sSerie", sSerie);
            //    if (nFolio > 0)
            //        giSql.AgregarParametro("nFolio", nFolio);
            //    giSql.AgregarParametro("sVersion", sVersionDoc);
            //    res = Convert.ToInt32(giSql.TraerEscalar("usp_cfd_Obtener_Comprobante_Sel", true)) > 0;
            //}
            //catch (Exception ex)
            //{
            //    return true;
            //}
            //return res;
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Obtener_Comprobante_Sel";
                        if (!string.IsNullOrEmpty(sUUID))
                            cmd.Parameters.Add(new SqlParameter("sUUID", sUUID));
                        if (!string.IsNullOrEmpty(sRfc))
                            cmd.Parameters.Add(new SqlParameter("sRfcEmisor", sRfc));
                        if (!string.IsNullOrEmpty(sSerie))
                            cmd.Parameters.Add(new SqlParameter("sSerie", sSerie));
                        if (nFolio > 0)
                            cmd.Parameters.Add(new SqlParameter("nFolio", nFolio));
                        cmd.Parameters.Add(new SqlParameter("sVersion", sVersionDoc));
                        res = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                        con.Close();
                        con.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            return res;
        }

        /// <summary>
        /// Busca comprobantes con los filtros agregados que sean del proveedor con paginado
        /// </summary>
        /// <param name="datFechaInicio"></param>
        /// <param name="datFechaFin"></param>
        /// <param name="sRfcEmisor"></param>
        /// <param name="sRfcReceptor"></param>
        /// <param name="sVersion"></param>
        /// <param name="nIdUsuario"></param>
        /// <returns></returns>
        public DataTable fnBuscarComprobantesProveedorPag(DateTime datFechaInicio, DateTime datFechaFin, string sRfcEmisor,
            string sRfcReceptor, string sVersion, int nIdUsuario, int nTipo, string sLista, int nPagina, int nNumPPag)
        {
            //DataTable dtResultado = new DataTable("dtResultadoComprobantes");
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


            //    iSql.AgregarParametro("dDesde", datFechaInicio);
            //    iSql.AgregarParametro("dHasta", datFechaFin);

            //    if (!string.IsNullOrEmpty(sRfcEmisor))
            //        iSql.AgregarParametro("sRFCEmisor", sRfcEmisor);
            //    if (!string.IsNullOrEmpty(sRfcReceptor))
            //        iSql.AgregarParametro("sRFCReceptor", sRfcReceptor);
            //    if (!string.IsNullOrEmpty(sVersion))
            //        iSql.AgregarParametro("sVersion", sVersion);
            //    iSql.AgregarParametro("nUsuario", nIdUsuario);
            //    iSql.AgregarParametro("nTipo", nTipo);
            //    iSql.AgregarParametro("nPagina", nPagina);
            //    iSql.AgregarParametro("nNumPPagina", nNumPPag);
            //    iSql.AgregarParametro("sLista", sLista);
            //    iSql.Query("usp_cfd_Comprobantes_Proveedor_Pag_sel", true, ref dtResultado);
            //}
            //catch (Exception ex)
            //{

            //}

            //return dtResultado;

            DataTable dtResultado = new DataTable("dtResultadoComprobantes");
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Comprobantes_Proveedor_Pag_sel";

                        cmd.Parameters.Add(new SqlParameter("dDesde", datFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("dHasta", datFechaFin));

                        if (!string.IsNullOrEmpty(sRfcEmisor))
                            cmd.Parameters.Add(new SqlParameter("sRFCEmisor", sRfcEmisor));
                        if (!string.IsNullOrEmpty(sRfcReceptor))
                            cmd.Parameters.Add(new SqlParameter("sRFCReceptor", sRfcReceptor));
                        if (!string.IsNullOrEmpty(sVersion))
                            cmd.Parameters.Add(new SqlParameter("sVersion", sVersion));

                        cmd.Parameters.Add(new SqlParameter("nUsuario", nIdUsuario));
                        cmd.Parameters.Add(new SqlParameter("nTipo", nTipo));
                        cmd.Parameters.Add(new SqlParameter("nPagina", nPagina));
                        cmd.Parameters.Add(new SqlParameter("nNumPPagina", nNumPPag));
                        cmd.Parameters.Add(new SqlParameter("sLista", sLista));

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtResultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }

            return dtResultado;
        }

        /// <summary>
        /// Busca comprobantes con los filtros agregados con paginado
        /// </summary>
        /// <param name="datFechaInicio"></param>
        /// <param name="datFechaFin"></param>
        /// <param name="sRfcEmisor"></param>
        /// <param name="sRfcReceptor"></param>
        /// <param name="sVersion"></param>
        /// <param name="nIdUsuario"></param>
        /// <returns></returns>
        public DataTable fnBuscarComprobantesPag(DateTime datFechaInicio, DateTime datFechaFin, string sRfcEmisor,
            string sRfcReceptor, string sVersion, int nIdUsuario, int nTipo, string sLista, int nPagina, int nNumPPag)
        {
            //DataTable dtResultado = new DataTable("dtResultadoComprobantes");
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


            //    iSql.AgregarParametro("dDesde", datFechaInicio);
            //    iSql.AgregarParametro("dHasta", datFechaFin);
            //    if (!string.IsNullOrEmpty(sRfcEmisor))
            //        iSql.AgregarParametro("sRFCEmisor", sRfcEmisor);
            //    if (!string.IsNullOrEmpty(sRfcReceptor))
            //        iSql.AgregarParametro("sRFCReceptor", sRfcReceptor);
            //    if (!string.IsNullOrEmpty(sVersion))
            //        iSql.AgregarParametro("sVersion", sVersion);
            //    iSql.AgregarParametro("nUsuario", nIdUsuario);
            //    iSql.AgregarParametro("nTipo", nTipo);
            //    iSql.AgregarParametro("sLista", sLista);
            //    iSql.AgregarParametro("nPagina", nPagina);
            //    iSql.AgregarParametro("nNumPPagina", nNumPPag);
            //    iSql.Query("usp_cfd_Comprobantes_Pag_sel", true, ref dtResultado);
            //}
            //catch (Exception)
            //{

            //}

            //return dtResultado;

            DataTable dtResultado = new DataTable("dtResultadoComprobantes");
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Comprobantes_Pag_sel";

                        cmd.Parameters.Add(new SqlParameter("dDesde", datFechaInicio));
                        cmd.Parameters.Add(new SqlParameter("dHasta", datFechaFin));

                        if (!string.IsNullOrEmpty(sRfcEmisor))
                            cmd.Parameters.Add(new SqlParameter("sRFCEmisor", sRfcEmisor));
                        if (!string.IsNullOrEmpty(sRfcReceptor))
                            cmd.Parameters.Add(new SqlParameter("sRFCReceptor", sRfcReceptor));
                        if (!string.IsNullOrEmpty(sVersion))
                            cmd.Parameters.Add(new SqlParameter("sVersion", sVersion));

                        cmd.Parameters.Add(new SqlParameter("nUsuario", nIdUsuario));
                        cmd.Parameters.Add(new SqlParameter("nTipo", nTipo));
                        cmd.Parameters.Add(new SqlParameter("sLista", sLista));
                        cmd.Parameters.Add(new SqlParameter("nPagina", nPagina));
                        cmd.Parameters.Add(new SqlParameter("nNumPPagina", nNumPPag));

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtResultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }

            return dtResultado;
        }

        //public DataSet fnObtenerDataSetFiltros()
        //{
        //    //TODO: Filtrar info por usuario a menos de que sea el administrador
        //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");
        //    DataSet dsFiltros = new DataSet("dsFiltros");
        //    iSql.Query("usp_rfp_ObtieneFiltrosConsulta_Ins", true, ref dsFiltros);
        //    return dsFiltros;
        //}

        public void fnGuardarComprobante(XmlDocument xComprobante, clsResultadoValidacion res,  
            int idUsuario, byte[] bPdf, int nIdSucursal, DateTime dFechaValidacion)
        {
            DateTime dFechaDocumento = DateTime.MinValue;
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xComprobante.NameTable);
            nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navComprobante = xComprobante.CreateNavigator();
            string sVersionDoc = string.Empty;
            //Verfica la versión del comprobante
            try
            {
                sVersionDoc = navComprobante.SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value;
            }
            catch
            {
                try
                {
                    sVersionDoc = navComprobante.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if(sVersionDoc.StartsWith("3"))
            {
                try
                {
                    dFechaDocumento = Convert.ToDateTime(navComprobante.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (sVersionDoc.StartsWith("2"))
            {
                try
                {
                    dFechaDocumento = Convert.ToDateTime(navComprobante.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).Value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");
                
            //    iSql.AgregarParametro("dFecha_Validacion", dFechaValidacion);
            //    iSql.AgregarParametro("bValido", res.valido);
            //    if (!res.valido)
            //        iSql.AgregarParametro("sMensaje", res.mensaje);
            //    iSql.AgregarParametro("nId_Usuario", idUsuario);
            //    iSql.AgregarParametro("xXml", xComprobante.InnerXml.ToString().Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>",""));
            //    if (bPdf.Length > 0)
            //        iSql.AgregarParametro("bPdf", bPdf);
            //    iSql.AgregarParametro("nIdSucursal", nIdSucursal);
            //    iSql.AgregarParametro("@dFechaDocumento", dFechaDocumento);
            //    iSql.AgregarParametro("sVersion", sVersionDoc);
            //    iSql.NoQuery("usp_rfp_GuardaComprobante_Ins", true);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_rfp_GuardaComprobante_Ins";
                        cmd.Parameters.Add(new SqlParameter("dFecha_Validacion", dFechaValidacion));
                        cmd.Parameters.Add(new SqlParameter("bValido", res.valido));
                        if (!res.valido)
                            cmd.Parameters.Add(new SqlParameter("sMensaje", res.mensaje));
                        cmd.Parameters.Add(new SqlParameter("nId_Usuario", idUsuario));
                        cmd.Parameters.Add(new SqlParameter("xXml", xComprobante.InnerXml.ToString().Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "")));
                        if (bPdf.Length > 0)
                            cmd.Parameters.Add(new SqlParameter("bPdf", bPdf));
                        cmd.Parameters.Add(new SqlParameter("nIdSucursal", nIdSucursal));
                        cmd.Parameters.Add(new SqlParameter("@dFechaDocumento", dFechaDocumento));
                        cmd.Parameters.Add(new SqlParameter("sVersion", sVersionDoc));

                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable fnObtenerComprobanteDatos(int nIdComprobante)
        {
            //DataTable res = new DataTable();
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");

                
            //    iSql.AgregarParametro("@nIdComprobante", nIdComprobante);
            //    iSql.Query("usp_cfd_Obtener_Comprobante_Datos_sel", true, ref res);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return res;

            DataTable res = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Obtener_Comprobante_Datos_sel";
                        cmd.Parameters.Add(new SqlParameter("@nIdComprobante", nIdComprobante));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(res);
                        }
                        con.Close();
                        con.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public clsResultadoValidacion fnValidarProduccion(XmlDocument doc)
        {
            //throw new NotImplementedException();
            string usuarioPruebas = clsComun.fnObtenerSetting("usuarioWS");
            string pwdUsuarioPruebas = clsComun.fnObtenerSetting("pwdUsuarioWS");
            string resultado = string.Empty;
            string version = string.Empty;
            clsResultadoValidacion res;
            
            //wsTestValidacion.wcfValidaASMX servicio = new wsTestValidacion.wcfValidaASMX();
            wsProduccionValidacion.wcfValidaASMX servicio = new wsProduccionValidacion.wcfValidaASMX();
            
            version = fnObtenerVersion(doc);
            if (version.StartsWith("2"))
            {
                resultado = servicio.fnValidaCFD(doc.DocumentElement.OuterXml, usuarioPruebas, pwdUsuarioPruebas, version);
            }
            if (version.StartsWith("3"))
            {
                resultado = servicio.fnValidaXML(doc.DocumentElement.OuterXml, usuarioPruebas, pwdUsuarioPruebas, version);
            }

            if (resultado.ToUpper().Contains("SIN ERRORES"))
                res = new clsResultadoValidacion(true, resultado, resultado.Split('-')[0]);
            else
                res = new clsResultadoValidacion(false, resultado, resultado.Split('-')[0]);

            return res;
        }

        /// <summary>
        /// Valida un documento en el webservice de pruebas de PAX
        /// </summary>
        /// <param name="doc">Docuemento a validar</param>
        /// <returns></returns>
        public clsResultadoValidacion fnValidarTest(XmlDocument doc)
        {
            string usuarioPruebas = clsComun.fnObtenerSetting("usuarioWS");
            string pwdUsuarioPruebas = clsComun.fnObtenerSetting("pwdUsuarioWS");
            string resultado = string.Empty;
            string version = string.Empty;
            clsResultadoValidacion res;
            wsTestValidacion.wcfValidaASMX servicio = new wsTestValidacion.wcfValidaASMX();
            version = fnObtenerVersion(doc);
            if (version.StartsWith("2"))
            {
                resultado = servicio.fnValidaCFD(doc.DocumentElement.OuterXml, usuarioPruebas, pwdUsuarioPruebas, version);
            }
            if (version.StartsWith("3"))
            {
                resultado = servicio.fnValidaXML(doc.DocumentElement.OuterXml, usuarioPruebas, pwdUsuarioPruebas, version);
            }

            if (resultado.ToUpper().Contains("SIN ERRORES"))
                res = new clsResultadoValidacion(true, resultado, resultado.Split('-')[0]);
            else
                res = new clsResultadoValidacion(false, resultado, resultado.Split('-')[0]);

            return res;
        }

        /// <summary>
        /// Obtiene la version del documento
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
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
            try
            {
                version = doc.CreateNavigator().SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value;
            }
            catch { }

            return version;
        }

        /// <summary>
        /// Obtiene el archivo XML del comprobante
        /// </summary>
        /// <param name="nIdComprobante"></param>
        /// <returns></returns>
        public DataTable fnObtenerArchivoXml(int nIdComprobante)
        {
            //DataTable res = new DataTable();
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


            //    iSql.AgregarParametro("@nIdComprobante", nIdComprobante);
            //    iSql.Query("usp_cfd_Obtener_Xml_sel", true, ref res);
            //}
            //catch (Exception ex)
            //{

            //}
            //return res;
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Obtener_Xml_sel";
                        cmd.Parameters.Add(new SqlParameter("nIdComprobante", nIdComprobante));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(res);
                        }
                        con.Close();
                        con.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            return res;
        }

        /// <summary>
        /// Obtiene el archivo PDF del comprobante
        /// </summary>
        /// <param name="nIdComprobante"></param>
        /// <returns></returns>
        public DataTable fnObtenerArchivoPdf(int nIdComprobante)
        {
            //DataTable res = new DataTable();
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


            //    iSql.AgregarParametro("@nIdComprobante", nIdComprobante);
            //    iSql.Query("usp_cfd_Obtener_Pdf_sel", true, ref res);
            //}
            //catch (Exception ex)
            //{

            //}
            //return res;
            DataTable res = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Obtener_Pdf_sel";
                        cmd.Parameters.Add(new SqlParameter("nIdComprobante", nIdComprobante));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(res);
                        }
                        con.Close();
                        con.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            return res;
        }

        /// <summary>
        /// Obtiene los comprobantes asignados a un estatus
        /// /// <param name="nidUsuario">identificador del estatus</param>
        /// </summary>   
        /// <returns>Regresa la lista de los comprobantes</returns>
        public int fnObtenerEstatusComprobantes(int nidEstatus)
        {
            //try
            //{
            //    giSql = clsComun.fnCrearConexion("conRecepcionProveedores");
            //    dtAuxiliar = new DataTable();
            //    giSql.AgregarParametro("@nId_Estatus", nidEstatus);
            //    int retorno = Convert.ToInt32(giSql.TraerEscalar("usp_Cfd_EstatusComprobantes_sel", true));
            //    return retorno;
            //}
            //catch (Exception ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            //    return 0;
            //}
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_Cfd_EstatusComprobantes_sel";
                        cmd.Parameters.Add(new SqlParameter("nId_Estatus", nidEstatus));
                        int retorno = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                        con.Dispose();
                        return retorno;
                    }
                }


            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                return 0;
            }
        }

        /// <summary>
        /// Elimina un estatus de comprobante
        /// </summary>
        /// <param name="@nIdEstatus">Id de Estatus</param>
        public void fnEliminEstatus(int idEstatus)
        {
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");
            //    iSql.AgregarParametro("@nIdEstatus", idEstatus);
            //    iSql.NoQuery("usp_cfd_Eliminar_Estatus_up", true);
            //}
            //catch (Exception ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            //}

            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Eliminar_Estatus_up";
                        cmd.Parameters.Add(new SqlParameter("nIdEstatus", idEstatus));
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

        /// <summary>
        /// Elimina un estatus de comprobante
        /// </summary>
        /// <param name="@nIdEstatus">Id de Estatus</param>
        public int fnBuscaEstatusProtegido(int idEstatus)
        {
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");
            //    iSql.AgregarParametro("@nIdStatus", idEstatus);
            //    int retorno = Convert.ToInt32( iSql.TraerEscalar("usp_Obtener_Status_Protegido_sel", true));
            //    return retorno;
            //}
            //catch (Exception ex)
            //{
            //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            //    return 0;
            //}
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_Obtener_Status_Protegido_sel";
                        cmd.Parameters.Add(new SqlParameter("nIdStatus", idEstatus));
                        int retorno = Convert.ToInt32(cmd.ExecuteScalar());
                        con.Close();
                        con.Dispose();
                        return retorno;
                    }
                }

            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                return 0;
            }
        }

        /// <summary>
        /// Guarda un estatus de comprobante
        /// </summary>
        /// <param name="@sNombre">Nombre de Estatus</param>
        public void fnGuardarStatus(string sNombre)
        {
            
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


            //    iSql.AgregarParametro("@sNombre", sNombre);
            //    iSql.NoQuery("usp_cfd_Guardar_Estatus_Ins", true);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Guardar_Estatus_Ins";
                        cmd.Parameters.Add(new SqlParameter("sNombre", sNombre));
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        public DataTable fnObtenerComprobantesTemp(int nIdUsuario)
        {
            //DataTable res = new DataTable();
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


            //    iSql.AgregarParametro("@nIdUsuario", nIdUsuario);
            //    iSql.Query("usp_cfd_Obtener_Comprobantes_temp", true, ref res);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return res;

            DataTable res = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Obtener_Comprobantes_temp";
                        cmd.Parameters.Add(new SqlParameter("@nIdUsuario", nIdUsuario));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(res);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }

            return res;
        }

        public DataTable fnObtenerComprobanteArchivosTemp(int nIdComprobante)
        {
            DataTable res = new DataTable();

            string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ToString();
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena)))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_cfd_Obtener_Comprobante_archivo_temp", con))
                        {

                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@nIdComprobante", nIdComprobante);

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(res);

                            tran.Commit();

                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                    finally
                    {
                        con.Close();
                    }

                }
            }

            return res;
        }

        public void fnGuardarComprobanteTemp(XmlDocument xComprobante,
            int idUsuario, byte[] bPdf, string sNombreXml, string sNombrePdf, bool bValidar, string sError,
            bool bValido)
        {
            string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ToString();
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena)))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_cfd_Comprobante_temp_ins", con))
                        {
                            string reemplazar = string.Empty;
                            string xml = xComprobante.InnerXml.ToString();
                            if (xComprobante.InnerXml.ToString().StartsWith("<?xml"))
                            {
                                reemplazar = xComprobante.InnerXml.ToString().Split('>')[0] + ">";
                                xml = xComprobante.InnerXml.ToString().Replace(reemplazar, "");
                            }


                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@sNombreXml", sNombreXml);
                            cmd.Parameters.AddWithValue("@sNombrePdf", sNombrePdf);
                            cmd.Parameters.AddWithValue("@bValido", bValido);
                            cmd.Parameters.AddWithValue("@sError", sError);
                            cmd.Parameters.AddWithValue("@nIdUsuario", idUsuario);
                            cmd.Parameters.AddWithValue("@Xml", xml);
                            cmd.Parameters.AddWithValue("@Pdf", bPdf);
                            cmd.Parameters.AddWithValue("@bValidar", bValidar);

                            cmd.ExecuteScalar();

                            tran.Commit();

                        }
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                    finally
                    {
                        con.Close();
                    }

                }
            }
        }

        public void fnEliminarComprobantesTemp(int nIdUsuario)
        {
            //try
            //{
            //    InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


            //    iSql.AgregarParametro("@nIdUsuario", nIdUsuario);
            //    iSql.NoQuery("usp_cfd_Eliminar_Comprobantes_temp", true);

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_cfd_Eliminar_Comprobantes_temp";
                        cmd.Parameters.Add(new SqlParameter("nIdUsuario", nIdUsuario));
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }

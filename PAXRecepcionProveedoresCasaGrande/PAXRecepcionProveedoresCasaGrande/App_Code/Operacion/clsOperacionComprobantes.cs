using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;



    public class clsOperacionComprobantes
    {

        private InterfazSQL giSql;
        private DataTable dtAuxiliar;
        private string conexion = "conRecepcionProveedores";
        /// <summary>
        /// Obtiene el catálogo de estatus del comprobante
        /// </summary>
        /// <returns></returns>
        public DataTable fnObtenerStatus()
        {
            DataTable dtStatus = new DataTable();
            try
            {
                giSql = clsComun.fnCrearConexion(conexion);
                giSql.Query("usp_cfd_Obtener_Status_sel",true,ref dtStatus);


            }
            catch (Exception ex)
            {
                
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
            try
            {
                giSql = clsComun.fnCrearConexion(conexion);
                giSql.AgregarParametro("nIdStatus",nIdStatus);
                giSql.AgregarParametro("nIdComprobante", nIdComprobante);
                if(!string.IsNullOrEmpty(sFechaPago))
                    giSql.AgregarParametro("dFechaPago", DateTime.ParseExact(sFechaPago,"dd/MM/yyyy",CultureInfo.InvariantCulture));
                giSql.Query("usp_cfd_Cambiar_Status_Comprobante_upd", true);
            }
            catch (Exception ex)
            {

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
            try
            {
                giSql = clsComun.fnCrearConexion(conexion);
                if(!string.IsNullOrEmpty(sUUID))
                    giSql.AgregarParametro("sUUID", sUUID);
                if(!string.IsNullOrEmpty(sRfc))
                    giSql.AgregarParametro("sRfcEmisor", sRfc);
                if (!string.IsNullOrEmpty(sSerie))
                    giSql.AgregarParametro("sSerie", sSerie);
                if (nFolio > 0)
                    giSql.AgregarParametro("nFolio", nFolio);
                giSql.AgregarParametro("sVersion", sVersionDoc);
                res = Convert.ToInt32(giSql.TraerEscalar("usp_cfd_Obtener_Comprobante_Sel", true)) > 0;
            }
            catch (Exception ex)
            {
                return true;
            }
            return res;
        }
        ///// <summary>
        ///// Busca comprobantes con los filtros agregados
        ///// </summary>
        ///// <param name="datFechaInicio"></param>
        ///// <param name="datFechaFin"></param>
        ///// <param name="sRfcEmisor"></param>
        ///// <param name="sRfcReceptor"></param>
        ///// <param name="sVersion"></param>
        ///// <param name="nIdUsuario"></param>
        ///// <returns></returns>
        //public DataTable fnBuscarComprobantes(DateTime datFechaInicio, DateTime datFechaFin, string sRfcEmisor,
        //    string sRfcReceptor, string sVersion, int nIdUsuario)
        //{
        //    DataTable dtResultado = new DataTable("dtResultadoComprobantes");
        //    try
        //    {
        //        InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


        //        iSql.AgregarParametro("dDesde", datFechaInicio);
        //        iSql.AgregarParametro("dHasta", datFechaFin);
        //        if (!string.IsNullOrEmpty(sRfcEmisor))
        //            iSql.AgregarParametro("sRFCEmisor", sRfcEmisor);
        //        if (!string.IsNullOrEmpty(sRfcReceptor))
        //            iSql.AgregarParametro("sRFCReceptor", sRfcReceptor);
        //        if (!string.IsNullOrEmpty(sVersion))
        //            iSql.AgregarParametro("sVersion", sVersion);
        //        iSql.AgregarParametro("nUsuario", nIdUsuario);

        //        iSql.Query("usp_cfd_Comprobantes_sel", true, ref dtResultado);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //        return dtResultado;
        //}

        ///// <summary>
        ///// Busca comprobantes con los filtros agregados que sean del proveedor
        ///// </summary>
        ///// <param name="datFechaInicio"></param>
        ///// <param name="datFechaFin"></param>
        ///// <param name="sRfcEmisor"></param>
        ///// <param name="sRfcReceptor"></param>
        ///// <param name="sVersion"></param>
        ///// <param name="nIdUsuario"></param>
        ///// <returns></returns>
        //public DataTable fnBuscarComprobantesProveedor(DateTime datFechaInicio, DateTime datFechaFin, string sRfcEmisor,
        //    string sRfcReceptor, string sVersion, int nIdUsuario)
        //{
        //    DataTable dtResultado = new DataTable("dtResultadoComprobantes");
        //    try
        //    {
        //        InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


        //        iSql.AgregarParametro("dDesde", datFechaInicio);
        //        iSql.AgregarParametro("dHasta", datFechaFin);
        //        if (!string.IsNullOrEmpty(sRfcEmisor))
        //            iSql.AgregarParametro("sRFCEmisor", sRfcEmisor);
        //        if (!string.IsNullOrEmpty(sRfcReceptor))
        //            iSql.AgregarParametro("sRFCReceptor", sRfcReceptor);
        //        if (!string.IsNullOrEmpty(sVersion))
        //            iSql.AgregarParametro("sVersion", sVersion);
        //        iSql.AgregarParametro("nUsuario", nIdUsuario);

        //        iSql.Query("usp_cfd_Comprobantes_Proveedor_sel", true, ref dtResultado);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return dtResultado;
        //}
        /// <summary>
        /// Busca comprobantes con los filtros agregados
        /// </summary>
        /// <param name="datFechaInicio"></param>
        /// <param name="datFechaFin"></param>
        /// <param name="sRfcEmisor"></param>
        /// <param name="sRfcReceptor"></param>
        /// <param name="sVersion"></param>
        /// <param name="nIdUsuario"></param>
        /// <returns></returns>
        public DataTable fnBuscarComprobantes(DateTime datFechaInicio, DateTime datFechaFin, string sRfcEmisor,
            string sRfcReceptor, string sVersion, int nIdUsuario, int nTipo, string sLista)
        {
            DataTable dtResultado = new DataTable("dtResultadoComprobantes");
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("dDesde", datFechaInicio);
                iSql.AgregarParametro("dHasta", datFechaFin);
                if (!string.IsNullOrEmpty(sRfcEmisor))
                    iSql.AgregarParametro("sRFCEmisor", sRfcEmisor);
                if (!string.IsNullOrEmpty(sRfcReceptor))
                    iSql.AgregarParametro("sRFCReceptor", sRfcReceptor);
                if (!string.IsNullOrEmpty(sVersion))
                    iSql.AgregarParametro("sVersion", sVersion);
                iSql.AgregarParametro("nUsuario", nIdUsuario);
                iSql.AgregarParametro("nTipo", nTipo);
                iSql.AgregarParametro("sLista", sLista);

                iSql.Query("usp_cfd_Comprobantes_sel", true, ref dtResultado);
            }
            catch (Exception)
            {

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
            DataTable dtResultado = new DataTable("dtResultadoComprobantes");
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("dDesde", datFechaInicio);
                iSql.AgregarParametro("dHasta", datFechaFin);
                if (!string.IsNullOrEmpty(sRfcEmisor))
                    iSql.AgregarParametro("sRFCEmisor", sRfcEmisor);
                if (!string.IsNullOrEmpty(sRfcReceptor))
                    iSql.AgregarParametro("sRFCReceptor", sRfcReceptor);
                if (!string.IsNullOrEmpty(sVersion))
                    iSql.AgregarParametro("sVersion", sVersion);
                iSql.AgregarParametro("nUsuario", nIdUsuario);
                iSql.AgregarParametro("nTipo", nTipo);
                iSql.AgregarParametro("sLista", sLista);
                iSql.AgregarParametro("nPagina", nPagina);
                iSql.AgregarParametro("nNumPPagina", nNumPPag);
                iSql.Query("usp_cfd_Comprobantes_Pag_sel", true, ref dtResultado);
            }
            catch (Exception)
            {

            }

            return dtResultado;
        }

        /// <summary>
        /// Busca comprobantes con los filtros agregados que sean del proveedor
        /// </summary>
        /// <param name="datFechaInicio"></param>
        /// <param name="datFechaFin"></param>
        /// <param name="sRfcEmisor"></param>
        /// <param name="sRfcReceptor"></param>
        /// <param name="sVersion"></param>
        /// <param name="nIdUsuario"></param>
        /// <returns></returns>
        public DataTable fnBuscarComprobantesProveedor(DateTime datFechaInicio, DateTime datFechaFin, string sRfcEmisor,
            string sRfcReceptor, string sVersion, int nIdUsuario, int nTipo, string sLista)
        {
            DataTable dtResultado = new DataTable();
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("dDesde", datFechaInicio);
                iSql.AgregarParametro("dHasta", datFechaFin);

                if (!string.IsNullOrEmpty(sRfcEmisor))
                    iSql.AgregarParametro("sRFCEmisor", sRfcEmisor);
                if (!string.IsNullOrEmpty(sRfcReceptor))
                    iSql.AgregarParametro("sRFCReceptor", sRfcReceptor);
                if (!string.IsNullOrEmpty(sVersion))
                    iSql.AgregarParametro("sVersion", sVersion);
                iSql.AgregarParametro("nUsuario", nIdUsuario);
                iSql.AgregarParametro("nTipo", nTipo);
                iSql.AgregarParametro("sLista", sLista);

                iSql.Query("usp_cfd_Comprobantes_Proveedor_sel", true, ref dtResultado);
            }
            catch (Exception ex)
            {

            }

            return dtResultado;
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
            DataTable dtResultado = new DataTable("dtResultadoComprobantes");
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("dDesde", datFechaInicio);
                iSql.AgregarParametro("dHasta", datFechaFin);

                if (!string.IsNullOrEmpty(sRfcEmisor))
                    iSql.AgregarParametro("sRFCEmisor", sRfcEmisor);
                if (!string.IsNullOrEmpty(sRfcReceptor))
                    iSql.AgregarParametro("sRFCReceptor", sRfcReceptor);
                if (!string.IsNullOrEmpty(sVersion))
                    iSql.AgregarParametro("sVersion", sVersion);
                iSql.AgregarParametro("nUsuario", nIdUsuario);
                iSql.AgregarParametro("nTipo", nTipo);
                iSql.AgregarParametro("sLista", sLista);
                iSql.AgregarParametro("nPagina", nPagina);
                iSql.AgregarParametro("nNumPPagina", nNumPPag);
                iSql.Query("usp_cfd_Comprobantes_Proveedor_Pag_sel", true, ref dtResultado);
            }
            catch (Exception ex)
            {

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
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");
                
                iSql.AgregarParametro("dFecha_Validacion", dFechaValidacion);
                iSql.AgregarParametro("bValido", res.valido);
                if (!res.valido)
                    iSql.AgregarParametro("sMensaje", res.mensaje);
                iSql.AgregarParametro("nId_Usuario", idUsuario);
                iSql.AgregarParametro("xXml", xComprobante.InnerXml.ToString().Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>",""));
                if (bPdf.Length > 0)
                    iSql.AgregarParametro("bPdf", bPdf);
                iSql.AgregarParametro("nIdSucursal", nIdSucursal);
                iSql.AgregarParametro("@dFechaDocumento", dFechaDocumento);
                iSql.AgregarParametro("sVersion", sVersionDoc);
                iSql.NoQuery("usp_rfp_GuardaComprobante_Ins", true);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable fnObtenerComprobanteDatos(int nIdComprobante)
        {
            DataTable res = new DataTable();
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");

                
                iSql.AgregarParametro("@nIdComprobante", nIdComprobante);
                iSql.Query("usp_cfd_Obtener_Comprobante_Datos_sel", true, ref res);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return res;
        }

        public clsResultadoValidacion fnValidarProduccion(XmlDocument doc)
        {
            string usuarioPruebas = clsComun.fnObtenerSetting("usuarioWS");
            string pwdUsuarioPruebas = clsComun.fnObtenerSetting("pwdUsuarioWS");
            string resultado = string.Empty;
            string version = string.Empty;
            clsResultadoValidacion res;
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
            DataTable res = new DataTable();
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("@nIdComprobante", nIdComprobante);
                iSql.Query("usp_cfd_Obtener_Xml_sel", true, ref res);
            }
            catch (Exception ex)
            {

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
            DataTable res = new DataTable();
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("@nIdComprobante", nIdComprobante);
                iSql.Query("usp_cfd_Obtener_Pdf_sel", true, ref res);
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        /// <summary>
        /// Obtiene el archivo XML del comprobante
        /// </summary>
        /// <param name="nIdComprobante"></param>
        /// <returns></returns>
        public DataTable fnObtenerArchivoXmlCliente(int nIdComprobante)
        {
            DataTable res = new DataTable();
            string ver = clsComun.ObtenerParamentro("versionCompCliente");
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("@nIdComprobante", nIdComprobante);
                iSql.AgregarParametro("@sVersion", ver);
                iSql.Query("usp_cfd_Obtener_Xml_Cliente_sel", true, ref res);
            }
            catch (Exception ex)
            {

            }
            return res;
        }

        /// <summary>
        /// Obtiene el archivo PDF del comprobante
        /// </summary>
        /// <param name="nIdComprobante"></param>
        /// <returns></returns>
        public DataTable fnObtenerArchivoPdfCliente(int nIdComprobante)
        {
            DataTable res = new DataTable();
            string ver = clsComun.ObtenerParamentro("versionCompCliente");
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("@nIdComprobante", nIdComprobante);
                iSql.AgregarParametro("@sVersion", ver);
                iSql.Query("usp_cfd_Obtener_Pdf_Cliente_sel", true, ref res);
            }
            catch (Exception ex)
            {

            }
            return res;
        }


        public string fnObtenerComprobanteCliente(int nIdSucursal, string sRfc, string sSerie, int nFolio, DateTime dtFecha, string nTotal, string sUuid)
        {
             DataTable res = new DataTable();
            SqlDataReader dr = null;
            string retorno = string.Empty;
            try
            {
                string ver = clsComun.ObtenerParamentro("versionCompCliente");
                string cfd = "cfd";
                if (ver == "3")
                {
                    cfd = "cfdi";
                }
                //Este query va a cambiar por los datos que nos pase el cliente
                StringBuilder sbQuery = new StringBuilder();

                sbQuery.Append(";WITH XMLNAMESPACES ('http://www.sat.gob.mx/cfd/3' as cfdi, 'http://www.sat.gob.mx/TimbreFiscalDigital' as tfd, 'http://www.paxfacturacion.com.mx' as doc) ");
                sbQuery.Append(" SELECT c.id_comprobante FROM tbl_cfd_Comprobante_Cliente_cat c ");
                sbQuery.Append("WHERE c.id_sucursal="+nIdSucursal);
                sbQuery.Append(" AND c.[xml].value('(/"+cfd+":Comprobante/cfdi:Receptor/@rfc)[1]', 'NVARCHAR(13)') = '" + sRfc + "'  ");
                sbQuery.Append("AND c.[xml].value('/" + cfd + ":Comprobante[1]/@total', 'FLOAT') = " + nTotal + " ");
                sbQuery.Append("AND c.[xml].value('/" + cfd + ":Comprobante[1]/@fecha', 'NVARCHAR(25)') LIKE '" + dtFecha.ToString("yyyy-MM-dd") + "%'  "); //Settings.Default["Query"].ToString();
                if(!string.IsNullOrEmpty(sSerie))
                {
                    sbQuery.Append("AND c.[xml].value('/" + cfd + ":Comprobante[1]/@serie', 'NVARCHAR(25)') = '" + sSerie + "' ");
                }
                if(nFolio>0)
                {
                    sbQuery.Append("AND c.[xml].value('/" + cfd + ":Comprobante[1]/@folio', 'BIGINT') = " + nFolio + " ");
                }
                if (!string.IsNullOrEmpty(sUuid))
                {
                    sbQuery.Append("[xml].value('(/" + cfd + ":Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID)[1]', 'NVARCHAR(255)') = '" + sUuid + "' ");
                }
                string query = sbQuery.ToString();
                //Hay que ver si el query lo guardamos encriptado o lo dejamos así
                //query = Utilerias.Encriptacion.Base64.DesencriptarBase64(query);
                //Hay que cambiar la cadena de conexión a la base de datos del cliente
                string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString;
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena)))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        try
                        {
                            cmd.Transaction = tran;
                            dr = cmd.ExecuteReader();
                            res.Load(dr);
                            tran.Commit();
                            retorno = res.Rows[0]["id_comprobante"].ToString();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return retorno;
        }

        public string fnObtenerComprobanteClienteSp(int nIdSucursal, string sRfc, string sSerie, int nFolio, DateTime dtFecha, string nTotal)
        {
            string sIdComprobante = string.Empty;
            string ver = clsComun.ObtenerParamentro("versionCompCliente");
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("@dFecha", dtFecha);
                iSql.AgregarParametro("@sRFCReceptor", sRfc);
                iSql.AgregarParametro("@sVersion", ver);
                iSql.AgregarParametro("@nIdSucursal", nIdSucursal);
                iSql.AgregarParametro("@nTotal", nTotal);
                iSql.AgregarParametro("@sSerie", sSerie);
                iSql.AgregarParametro("@nFolio", nFolio);
                sIdComprobante = iSql.TraerEscalar("usp_cfd_Buscar_Comprobantes_Cliente_sel", true).ToString();
            }
            catch (Exception ex)
            {

            }
            return sIdComprobante;
        }

        public DataTable fnObtenerComprobantesCliente(int nIdSucursal, string sRfc, string sSerie, int nFolio, DateTime dtFecha, string nTotal)
        {
            DataTable dtComprobantes = new DataTable();
            string ver = clsComun.ObtenerParamentro("versionCompCliente");
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("@dFecha", dtFecha);
                iSql.AgregarParametro("@sRFCReceptor", sRfc);
                iSql.AgregarParametro("@sVersion", ver);
                iSql.AgregarParametro("@nIdSucursal", nIdSucursal);
                if(!string.IsNullOrEmpty(nTotal))
                    iSql.AgregarParametro("@nTotal", nTotal);
                if (!string.IsNullOrEmpty(sSerie))
                    iSql.AgregarParametro("@sSerie", sSerie);
                if(nFolio>0)
                    iSql.AgregarParametro("@nFolio", nFolio);
                iSql.Query("usp_cfd_Buscar_Comprobantes_Cliente_Varios_sel", true, ref dtComprobantes);
            }
            catch (Exception ex)
            {

            }
            return dtComprobantes;
        }
        public DataTable fnObtenerComprobanteCliente(int nIdComprobante)
        {
            DataTable res = new DataTable();
            SqlDataReader dr = null;
            try
            {
                string ver = clsComun.ObtenerParamentro("versionCompCliente");
                string cfd = "cfd";
                if (ver == "3")
                {
                    cfd = "cfdi";
                }
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(";WITH XMLNAMESPACES ('http://www.sat.gob.mx/cfd/3' as cfdi, 'http://www.sat.gob.mx/TimbreFiscalDigital' as tfd, 'http://www.paxfacturacion.com.mx' as doc) ");
                sbQuery.Append(" SELECT ");
                sbQuery.Append("xml,");
                if (cfd == "cfdi")
                {
                    sbQuery.Append("[xml].value('/" + cfd + ":Comprobante[1]/@uuid', 'NVARCHAR(50)') as uuid");
                }
                else
                {
                    sbQuery.Append("[xml].value('(/" + cfd + ":Comprobante/*:Emisor/@rfc)[1]', 'NVARCHAR(13)') as rfc_emisor, ");
                    sbQuery.Append("[xml].value('(/" + cfd + ":Comprobante[1]/@serie)', 'NVARCHAR(13)') AS serie,");
                    sbQuery.Append("[xml].value('(/" + cfd + ":Comprobante[1]/@folio)', 'INT') AS folio");
                }
                sbQuery.Append(" FROM tbl_cfd_Comprobante_Cliente_cat ");
                sbQuery.Append("WHERE id_comprobante_cliente=");
                sbQuery.Append(nIdComprobante);
                string query = sbQuery.ToString();
                //query = Utilerias.Encriptacion.Base64.DesencriptarBase64(query);
                string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["conRecepconProveedores"].ConnectionString;
                cadena = Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena);
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena)))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        try
                        {
                            cmd.Transaction = tran;
                            dr = cmd.ExecuteReader();
                            res.Load(dr);
                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return res;
        }

        public DataTable fnObtenerComprobantePdfCliente(int nIdComprobante)
        {
            DataTable res = new DataTable();
            SqlDataReader dr = null;
            try
            {

                string ver = clsComun.ObtenerParamentro("versionCompCliente");
                string cfd = "cfd";
                if (ver == "3")
                {
                    cfd = "cfdi";
                }
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append(";WITH XMLNAMESPACES ('http://www.sat.gob.mx/cfd/3' as cfdi, 'http://www.sat.gob.mx/TimbreFiscalDigital' as tfd, 'http://www.paxfacturacion.com.mx' as doc) ");
                sbQuery.Append(" SELECT ");
                sbQuery.Append("pdf,");
                if (cfd == "cfdi")
                {
                    sbQuery.Append("[xml].value('/" + cfd + ":Comprobante[1]/@uuid', 'NVARCHAR(50)') as uuid");
                }
                else
                {
                    sbQuery.Append("[xml].value('(/" + cfd + ":Comprobante/*:Emisor/@rfc)[1]', 'NVARCHAR(13)') as rfc_emisor, ");
                    sbQuery.Append("[xml].value('(/" + cfd + ":Comprobante[1]/@serie)', 'NVARCHAR(13)') AS serie,");
                    sbQuery.Append("[xml].value('(/" + cfd + ":Comprobante[1]/@folio)', 'INT') AS folio");
                }
                sbQuery.Append(" FROM tbl_cfd_Comprobante_Cliente_cat ");
                sbQuery.Append("WHERE id_comprobante_cliente=");
                sbQuery.Append(nIdComprobante);
                string query = sbQuery.ToString();
                //query = Utilerias.Encriptacion.Base64.DesencriptarBase64(query);
                string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["conRecepconProveedores"].ConnectionString;
                cadena = Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena);
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena)))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        try
                        {
                            cmd.Transaction = tran;
                            dr = cmd.ExecuteReader();
                            res.Load(dr);
                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return res;
        }

        public DataTable fnObtenerComprobantesTemp(int nIdUsuario)
        {
            DataTable res = new DataTable();
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("@nIdUsuario", nIdUsuario);
                iSql.Query("usp_cfd_Obtener_Comprobantes_temp", true, ref res);

            }
            catch (Exception ex)
            {
                throw ex;
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
            int idUsuario, byte[] bPdf, string sNombreXml,string sNombrePdf, bool bValidar,string sError,
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
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("@nIdUsuario", nIdUsuario);
                iSql.NoQuery("usp_cfd_Eliminar_Comprobantes_temp", true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void fnComprobanteDescargadoCliente(int nIdComprobante)
        {
            try
            {
                InterfazSQL iSql = clsComun.fnCrearConexion("conRecepcionProveedores");


                iSql.AgregarParametro("@nIdComprobante", nIdComprobante);
                iSql.NoQuery("usp_cfd_Comprobante_Descargado_up", true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }

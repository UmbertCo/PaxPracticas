using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using System.Xml;

/// <summary>
/// Clase encargada de la funcionalidad generica del timbrado.
/// </summary>
public class clsTimbradoFuncionalidad
{

    /// <summary>
    /// Recupera los datos del receptor seleccionado.
    /// </summary>
    /// <param name="nId_receptor">id del receptor</param>
    /// <returns>Regresa toda la lista de sucursales del receptor</returns>
    public static DataTable RecuperaSucReceptor(int nId_receptor)
    {

        DataTable tabla = new DataTable();

        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conTimbrado");

            conexion.AgregarParametro("nId_receptor", nId_receptor);

            conexion.Query("usp_Timbrado_Recupera_Sel", true, ref tabla);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return tabla;
    }

    /// <summary>
    /// Recupera la lista de las sucursales del emisor.
    /// </summary>
    /// <param name="nId_receptor">id del receptor</param>
    /// <param name="nId_estructura">id de la esctructura</param>
    /// <remarks>Ivan Lopez - 13 Mayo 2011 - Se quitó el parámetros del nId_receptor</remarks>
    /// <returns>Regresa la lista de las sucursales.</returns>
    public static DataTable fnRecuperaDetallesReceptorSuc(int nId_estructura)
    {

        DataTable tabla = new DataTable();

        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conTimbrado");

            //conexion.AgregarParametro("nId_receptor", nId_receptor);
            conexion.AgregarParametro("nId_estructura", nId_estructura);

            conexion.Query("usp_Timbrado_RecuperaDetSuc_Sel", true, ref tabla);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return tabla;
    }

    /// <summary>
    /// Trae la lista de receptores activos del usuario, y que tengan sucursal.
    /// La relación es usuario-estructura-receptor
    /// </summary>
    public DataTable fnLlenarDropReceptores(int nId_Estructura)
    {
        DataTable dtAuxiliar = new DataTable();

        try
        {
            Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conTimbrado");

            giSql.AgregarParametro("nId_Estructura", nId_Estructura);
            giSql.Query("usp_Cli_ReceptoresSuc_Sel", true, ref dtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return dtAuxiliar;
    }

    /// <summary>
    /// Trae la lista de receptores activos del usuario, y que tengan sucursal.
    /// La relación es usuario-estructura-receptor
    /// </summary>
    public DataTable fnLlenarGridReceptores(string nId_Estructura, string psRfc, string psRazonSocial)
    {
        DataTable dtAuxiliar = new DataTable();

        try
        {
            Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conTimbrado");

            giSql.AgregarParametro("nId_Estructura", nId_Estructura);

            if (!string.IsNullOrEmpty(psRfc))
                giSql.AgregarParametro("sRfc", psRfc);

            if (!string.IsNullOrEmpty(psRazonSocial))
                giSql.AgregarParametro("sRazon_Social", psRazonSocial);

            giSql.Query("usp_Cli_ReceptoresSuc_Sel", true, ref dtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return dtAuxiliar;
    }

    /// <summary>
    /// Trae la lista de los tipos de documentos disponibles.
    /// </summary>
    /// <returns>Regresa toda la lista de los documentos disponibles</returns>
    public static DataTable fnLlenarTiposDocumentos(int nId_Contribuyente)
    {
        DataTable dtAuxiliar = new DataTable();

        try
        {

            clsOperacionSeriesFolios giSq = new clsOperacionSeriesFolios();
            dtAuxiliar=giSq.fnObtenerTiposDocumentos(); 

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return dtAuxiliar;
    }

    /// <summary>
    /// Recupera la lista de los tipos de impuestos por usuario y documento.
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario</param>
    /// <param name="id_tipo_documento">id del tipo de documento</param>
    /// <returns>Regresa la lista de los impuestos disponibles</returns>
    public static DataSet fnRecuperaTipoImpuesto(int pnIdUsuario, int nid_tipo_documento, Decimal sSubtotal)
    {
        DataSet dtAuxiliar = new DataSet();

        try
        {
            Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conTimbrado");

            giSql.AgregarParametro("nid_Usuario", pnIdUsuario);
            giSql.AgregarParametro("nid_tipo_documento", nid_tipo_documento);
            giSql.AgregarParametro("sSubtotal", sSubtotal);

            giSql.Query("usp_Cfd_DocumentosTiposImp_Sel", true, ref dtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return dtAuxiliar;
    }

    /// <summary>
    /// Recupera la lista de las sucurslaes.
    /// </summary>
    /// <param name="nId_usuario">id del usuario</param>
    /// <returns>recupera la lista de las sucursales.</returns>
    public static DataTable LlenarDropSucursales(int nId_usuario)
    {
        Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conTimbrado");
        DataTable gdtAuxiliar = new DataTable("Sucursales");
        try
        {
            giSql.AgregarParametro("nId_Usuario", nId_usuario);
            giSql.Query("usp_Timbrado_Sucursal_Sel", true, ref gdtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return gdtAuxiliar;
    }

    /// <summary>
    /// Recupera las series configuradas
    /// </summary>
    /// <param name="nId_Estructura">id Estrucutra</param>
    /// <param name="nId_Tipo_Documento">id del tipo de documento</param>
    /// <returns>Regresa la lista de las series</returns>
    public static DataTable LlenarDropSeries(int nId_Estructura, int nId_Tipo_Documento)
    {

        clsOperacionSeriesFolios giSq = new clsOperacionSeriesFolios();
        DataTable gdtAuxiliar = new DataTable();
        
        try
        {
            clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
            gdtAuxiliar = giSq.fnObtenerSeries(nId_Estructura.ToString(), nId_Tipo_Documento.ToString(), datosUsuario.id_usuario.ToString()); 
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return gdtAuxiliar;
    }

    /// <summary>
    /// Obtiene el certificado por el id del rfc del contribuyente.
    /// </summary>
    /// <param name="nid_rfc">id del rfc</param>
    /// <returns>regresa el certificado</returns>
    public static DataTable ObtenerCertificado(int nid_rfc)
    {
        Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conTimbrado");
        DataTable gdtAuxiliar = new DataTable();

        try
        {
            giSql.AgregarParametro("nid_rfc", nid_rfc);

            giSql.Query("usp_Timbrado_RfcCertificado_Sel", true, ref gdtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return gdtAuxiliar;
    }

    /// <summary>
    /// Genera el objeto del comprobante
    /// </summary>
    /// <param name="sVersion"></param>
    /// <param name="sFolio"></param>
    /// <param name="sTipoComprobante"></param>
    /// <param name="sSubtotal"></param>
    /// <param name="sTotal"></param>
    /// <param name="sSerie"></param>
    /// <param name="moneda"></param>
    /// <param name="rfc"></param>
    /// <param name="snombre"></param>
    /// <param name="pais"></param>
    /// <param name="estado"></param>
    /// <param name="municipio"></param>
    /// <param name="localidad"></param>
    /// <param name="calle"></param>
    /// <param name="noExterior"></param>
    /// <param name="noInterior"></param>
    /// <param name="colonia"></param>
    /// <param name="codigoPostal"></param>
    /// <param name="grvGrid"></param>
    /// <param name="dtlDataList"></param>
    /// <returns>Retorna el objeto comprobante con los paramentros capturados.</returns>
    //public Comprobante fnObtenerXML(string sVersion,
    //                                    string sFolio,
    //                                    string sTipoComprobante,
    //                                    decimal sSubtotal, decimal sTotal, string sSerie, string moneda,
    //                                    string rfc,
    //                                    string snombre,
    //                                    string pais,
    //                                    string estado,
    //                                    string municipio,
    //                                    string localidad,
    //                                    string calle,
    //                                    string noExterior,
    //                                    string noInterior,
    //                                    string colonia,
    //                                    string codigoPostal,
    //                                    GridView grvGrid,
    //                                    DataList dtlDataList,
    //                                    byte[] bCertificado,
    //                                    int id_usuario,
    //                                    string sTitle,
    //                                    string metodopago,
    //                                    string lugarexp,
    //                                    string noCuenta,
    //                                    string paisemisor,
    //                                    string estadoemisor,
    //                                    string municipioemisor,
    //                                    string calleemisor,
    //                                    string codigopostal,
    //                                    string tipocambio,
    //                                    string regimenfiscal,
    //                                    string formapago)
    //{
        
    //    Comprobante cfd = new Comprobante();

    //    switch (sVersion)
    //    {
    //        case "3.0":
    //           return fnObtenerXML3_0(sVersion,
    //                                     sFolio,
    //                                     sTipoComprobante,
    //                                     sSubtotal, sTotal, sSerie, moneda,
    //                                     rfc,
    //                                     snombre,
    //                                     pais,
    //                                     estado,
    //                                     municipio,
    //                                     localidad,
    //                                     calle,
    //                                     noExterior,
    //                                     noInterior,
    //                                     colonia,
    //                                     codigoPostal,
    //                                     grvGrid,
    //                                     dtlDataList,
    //                                     bCertificado,
    //                                     id_usuario,
    //                                     sTitle);
    //        case "3.2":
    //           return fnObtenerXML3_2(sVersion,
    //                                     sFolio,
    //                                     sTipoComprobante,
    //                                     sSubtotal,  sTotal,  sSerie,  moneda,
    //                                     rfc,
    //                                     snombre,
    //                                     pais,
    //                                     estado,
    //                                     municipio,
    //                                     localidad,
    //                                     calle,
    //                                     noExterior,
    //                                     noInterior,
    //                                     colonia,
    //                                     codigoPostal,
    //                                     grvGrid,
    //                                     dtlDataList,
    //                                     bCertificado,
    //                                     id_usuario,
    //                                     sTitle,
    //                                     metodopago,
    //                                     lugarexp,
    //                                     noCuenta,
    //                                     paisemisor,
    //                                     estadoemisor,
    //                                     municipioemisor,
    //                                     calleemisor,
    //                                     codigopostal,
    //                                     tipocambio,
    //                                     regimenfiscal,
    //                                     formapago);
    //        default:
    //           return cfd;
    //    }
    //}


    /// <summary>
    /// Genera el objeto del comprobante version 3.2
    /// </summary>
    /// <param name="sVersion"></param>
    /// <param name="sFolio"></param>
    /// <param name="sTipoComprobante"></param>
    /// <param name="sSubtotal"></param>
    /// <param name="sTotal"></param>
    /// <param name="sSerie"></param>
    /// <param name="moneda"></param>
    /// <param name="rfc"></param>
    /// <param name="snombre"></param>
    /// <param name="pais"></param>
    /// <param name="estado"></param>
    /// <param name="municipio"></param>
    /// <param name="localidad"></param>
    /// <param name="calle"></param>
    /// <param name="noExterior"></param>
    /// <param name="noInterior"></param>
    /// <param name="colonia"></param>
    /// <param name="codigoPostal"></param>
    /// <param name="grvGrid"></param>
    /// <param name="dtlDataList"></param>
    /// <returns>Retorna el objeto comprobante con los paramentros capturados.</returns>
    public  Comprobante fnObtenerXML3_2(string sVersion,
                                        string sFolio,
                                        string sTipoComprobante,
                                        decimal sSubtotal, decimal sTotal, string sSerie, string moneda,
                                        string rfc,
                                        string snombre,
                                        string pais,
                                        string estado,
                                        string municipio,
                                        string localidad,
                                        string calle,
                                        string noExterior,
                                        string noInterior,
                                        string colonia,
                                        string codigoPostal,
                                        GridView grvGrid,
                                        DataList dtlDataList,
                                        byte[] bCertificado,
                                        int id_usuario,
                                        string sTitle,
                                        string metodopago,
                                        string lugarexp,
                                        string noCuenta,
                                        string paisExpEn,
                                        string estadoExpEn,
                                        string municipioExpEn,
                                        string calleExpEn,
                                        string codigopostalExpEn,
                                        string tipocambio,
                                        string regimenfiscal,
                                        string formapago,
                                        string snum_intExpEn,
                                        string snum_extExpEn,
                                        string scoloniaExpEn,
                                        string slocalidadExpEn,
                                        string sreferenciaExpEn,
                                        bool bGenPagPar,
                                        string sFolFisOri,
                                        string sSerFolFisOri,
                                        DateTime sFecFolFisOri,
                                        decimal sMonFolFisOri,
                                        bool bAgrExpEn,
                                        string sdescFormaPago,
                                        int nidSucursalFis)
    {

        ComprobanteEmisor emisor = new ComprobanteEmisor();

        List<ComprobanteEmisorRegimenFiscal> lsitaRegimen = new List<ComprobanteEmisorRegimenFiscal>();
        ComprobanteEmisorRegimenFiscal regimen = new ComprobanteEmisorRegimenFiscal();

        emisor.DomicilioFiscal = new t_UbicacionFiscal();
        Comprobante cfd = new Comprobante();
        clsValCertificado cer;
        ComprobanteReceptor receptor = new ComprobanteReceptor();
        receptor.Domicilio = new t_Ubicacion();
        clsOperacionCuenta gDAL = new clsOperacionCuenta();
        clsOperacionSucursales gOpeSuc = new clsOperacionSucursales();

        byte[] certificadoBinario;

        List<ComprobanteConcepto> listaConcepto = new List<ComprobanteConcepto>();
        ComprobanteConcepto concepto;

        ComprobanteImpuestos impuestos = new ComprobanteImpuestos();
        ComprobanteImpuestosTraslado impuestosTraslado = new ComprobanteImpuestosTraslado();
        ComprobanteImpuestosRetencion impuestosRetencion = new ComprobanteImpuestosRetencion();

        Label lblTasaVal = new Label();
        Label lblTipoImpDoc = new Label();
        Label lblTipoImpuesto = new Label();
        Label lblCalculo = new Label();

        try
        {
            //Recupera datos del emisor.
            SqlDataReader sdrInfo = gDAL.fnObtenerDatosFiscales(); //Obtiene pais
            SqlDataReader sdrInfoFis = gDAL.fnObtenerDatosFiscalesSuc(nidSucursalFis); //Obtiene rfc, razon social
            SqlDataReader sdrInfoSuc = gOpeSuc.fnObtenerDomicilioSuc(nidSucursalFis); //Obtiene direccion fiscal de la sucursal

            if (sdrInfoFis != null && sdrInfoFis.HasRows && sdrInfoFis.Read() && sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read() && sdrInfoSuc != null && sdrInfoSuc.HasRows && sdrInfoSuc.Read())
            {

                if (fnReplaceCaracters(sdrInfoFis["rfc"].ToString()) != string.Empty)
                    emisor.rfc = fnReplaceCaracters(sdrInfoFis["rfc"].ToString());
                if (fnReplaceCaracters(sdrInfoFis["razon_social"].ToString()) != string.Empty)
                    emisor.nombre = fnReplaceCaracters(sdrInfoFis["razon_social"].ToString());
                if (fnReplaceCaracters(sdrInfo["pais"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.pais = fnReplaceCaracters(sdrInfo["pais"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["estado"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.estado = fnReplaceCaracters(sdrInfoSuc["estado"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["municipio"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.municipio = fnReplaceCaracters(sdrInfoSuc["municipio"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["localidad"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.localidad = fnReplaceCaracters(sdrInfoSuc["localidad"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["calle"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.calle = fnReplaceCaracters(sdrInfoSuc["calle"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["numero_exterior"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.noExterior = fnReplaceCaracters(sdrInfoSuc["numero_exterior"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["numero_interior"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.noInterior = fnReplaceCaracters(sdrInfoSuc["numero_interior"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["colonia"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.colonia = fnReplaceCaracters(sdrInfoSuc["colonia"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["codigo_postal"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.codigoPostal = fnReplaceCaracters(sdrInfoSuc["codigo_postal"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["referencia"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.referencia = fnReplaceCaracters(sdrInfoSuc["referencia"].ToString());

                if (regimenfiscal != string.Empty)
                    regimen.Regimen = regimenfiscal;

                lsitaRegimen.Add(regimen);


                emisor.RegimenFiscal = lsitaRegimen.ToArray();
            }

            //Agregar Expedido En
            if (bAgrExpEn == true)
            {
                //SqlDataReader sdrInfoExpEn = gDAL.fnObtenerDatosFiscalesSuc(nidSucursalExpEn);

                t_Ubicacion expedidoEnField = new t_Ubicacion();

                if (codigopostalExpEn != string.Empty)
                    expedidoEnField.codigoPostal = codigopostalExpEn;

                if (snum_extExpEn != string.Empty)
                    expedidoEnField.noExterior = snum_extExpEn;

                if (snum_intExpEn != string.Empty)
                    expedidoEnField.noInterior = snum_intExpEn;

                if (scoloniaExpEn != string.Empty)
                    expedidoEnField.colonia = scoloniaExpEn;

                if (estadoExpEn != string.Empty)
                    expedidoEnField.estado = estadoExpEn;

                if (calleExpEn != string.Empty)
                    expedidoEnField.calle = calleExpEn;

                if (paisExpEn != string.Empty)
                    expedidoEnField.pais = paisExpEn;

                if (municipioExpEn != string.Empty)
                    expedidoEnField.municipio = municipioExpEn;

                if (slocalidadExpEn != string.Empty)
                    expedidoEnField.localidad = slocalidadExpEn;

                if (sreferenciaExpEn != string.Empty)
                    expedidoEnField.referencia = sreferenciaExpEn;

                emisor.ExpedidoEn = expedidoEnField;
            }

            //Obtener el certificado
            certificadoBinario = Utilerias.Encriptacion.DES3.Desencriptar(bCertificado);

            cer = new clsValCertificado(certificadoBinario);

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, DateTime.Now, sTitle + "|" + "5.-cfd" + "|" + "Se crea el ancabezado del documento XML");

            //Parte inicial del CFDI
            cfd.version = sVersion;//"3.2";
            if (sSerie != string.Empty)
                cfd.serie = sSerie;
            if (sFolio != string.Empty)
                cfd.folio = sFolio;

            //Eliminar el incremento de año.
            //cfd.fecha = Convert.ToDateTime(DateTime.Now.AddYears(1).AddDays(-22).AddMonths(-11).ToString("s"));
            cfd.fecha = Convert.ToDateTime(DateTime.Now.ToString("s"));
            cfd.noCertificado = cer.ObtenerNoCertificado();
            cfd.certificado = Convert.ToBase64String(certificadoBinario);
            cfd.subTotal = sSubtotal;
            cfd.Moneda = moneda;
            cfd.total = sTotal;
            cfd.metodoDePago = metodopago;
            cfd.LugarExpedicion = lugarexp;

            if (sdescFormaPago == "Pago en Parcialidades") //Se agrega los campos adicionales en caso de Pago en Parcialidades
            {
                if (bGenPagPar == false) //Si no es generación por primera vez de las parcialidades
                {
                    cfd.FolioFiscalOrig = sFolFisOri;
                    if (sSerFolFisOri != string.Empty)
                        cfd.SerieFolioFiscalOrig = sSerFolFisOri;

                    cfd.FechaFolioFiscalOrig = sFecFolFisOri;
                    cfd.FechaFolioFiscalOrigSpecified = true;
                    cfd.MontoFolioFiscalOrig = sMonFolFisOri;
                    cfd.MontoFolioFiscalOrigSpecified = true;                    
                }
                else //Si es por primera vez la generación de parcialidades
                {
                    cfd.FechaFolioFiscalOrigSpecified = false;
                    cfd.MontoFolioFiscalOrigSpecified = false;
                }
            }
            else
            {
                cfd.FechaFolioFiscalOrigSpecified = false;
                cfd.MontoFolioFiscalOrigSpecified = false;
            }

            cfd.formaDePago = formapago;

            if (noCuenta != string.Empty)
                cfd.NumCtaPago = noCuenta;

            //Detalle del tipo de comprobante I,E,T
            switch (sTipoComprobante)
            {
                case "I":
                    cfd.tipoDeComprobante = ComprobanteTipoDeComprobante.ingreso;
                    break;
                case "E":
                    cfd.tipoDeComprobante = ComprobanteTipoDeComprobante.egreso;
                    break;
                case "T":
                    cfd.tipoDeComprobante = ComprobanteTipoDeComprobante.traslado;
                    break;
                default :
                    cfd.tipoDeComprobante = ComprobanteTipoDeComprobante.ingreso;
                    break;
            }

            //Datos del receptor

            if (rfc != string.Empty)
                receptor.rfc = rfc;
            if (snombre != string.Empty)
                receptor.nombre = snombre;
            if (calle != string.Empty)
                receptor.Domicilio.calle = calle;
            if (noInterior != string.Empty)
                receptor.Domicilio.noInterior = noInterior;
            if (noExterior != string.Empty)
                receptor.Domicilio.noExterior = noExterior;
            if (colonia != string.Empty)
                receptor.Domicilio.colonia = colonia;
            if (localidad != string.Empty)
                receptor.Domicilio.localidad = localidad;
            if (municipio != string.Empty)
                receptor.Domicilio.municipio = municipio;
            if (estado != string.Empty)
                receptor.Domicilio.estado = estado;
            if (pais != string.Empty)
                receptor.Domicilio.pais = pais;
            if (codigoPostal != string.Empty)
                receptor.Domicilio.codigoPostal = codigoPostal;

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, DateTime.Now, sTitle + "|" + "6.-receptor" + "|" + "Se crea los datos del receptor en archivo XML");


            //Creacion de conceptos dinamicamente.
            if (grvGrid.Rows.Count > 0)
            {
                foreach (GridViewRow item in grvGrid.Rows)
                {

                    concepto = new ComprobanteConcepto();

                    if (item.Cells[3].Text != string.Empty)
                        concepto.noIdentificacion = HttpUtility.HtmlDecode(item.Cells[3].Text);
                    if (item.Cells[4].Text != string.Empty)
                        concepto.unidad = HttpUtility.HtmlDecode(item.Cells[4].Text);
                    if (item.Cells[5].Text != string.Empty)
                        concepto.descripcion = HttpUtility.HtmlDecode(item.Cells[5].Text); 
                    if (item.Cells[6].Text != string.Empty)
                        concepto.valorUnitario = Convert.ToDecimal(HttpUtility.HtmlDecode(item.Cells[6].Text).ToString().Replace("$", "").Replace(",", ""));
                    if (item.Cells[7].Text != string.Empty)
                        concepto.cantidad = Convert.ToDecimal(HttpUtility.HtmlDecode(item.Cells[7].Text));
                    if (item.Cells[8].Text != string.Empty)
                        concepto.importe = Convert.ToDecimal(HttpUtility.HtmlDecode(item.Cells[8].Text).ToString().Replace("$", "").Replace(",", ""));

                    listaConcepto.Add(concepto);
                }
            }

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, DateTime.Now, sTitle + "|" + "7.-conceptos" + "|" + "Se crea los conceptos en archivo XML");

            List<ComprobanteImpuestosTraslado> listaImpTraslado = new List<ComprobanteImpuestosTraslado>();
            List<ComprobanteImpuestosRetencion> listaImpRetencion = new List<ComprobanteImpuestosRetencion>();

            //Creacion de Impuestos dinamicamente.
            if (dtlDataList.Items.Count >0  )
            {
                foreach (DataListItem item in dtlDataList.Items)
                {

                    lblTasaVal = (Label)dtlDataList.Items[item.ItemIndex].FindControl("lblTasaVal");
                    lblTipoImpDoc = (Label)dtlDataList.Items[item.ItemIndex].FindControl("lblTipoImpDoc");
                    lblTipoImpuesto = (Label)dtlDataList.Items[item.ItemIndex].FindControl("lblTipoImpuesto");
                    lblCalculo = (Label)dtlDataList.Items[item.ItemIndex].FindControl("lblCalculo");


                    if (lblTipoImpDoc.Text == "Traslado" && lblTipoImpDoc.Text != string.Empty)
                    {

                        impuestosTraslado = new ComprobanteImpuestosTraslado();
                      
                        if (lblTipoImpuesto.Text.ToString().Contains("IVA"))
                            lblTipoImpuesto.Text = "IVA";

                        if (lblTipoImpuesto.Text.ToString().Contains("IEPS"))
                            lblTipoImpuesto.Text = "IEPS";

                        if (lblTipoImpuesto.Text.ToString().Contains("ISH"))
                            lblTipoImpuesto.Text = "ISH";

                        if (lblTipoImpuesto.Text.ToString().Contains("Cargos no gravables"))
                            lblTipoImpuesto.Text = "Cargos no gravables";

                        switch (lblTipoImpuesto.Text)
                        {
                            case "IVA":
                                impuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.IVA;
                                break;
                            case "IEPS":
                                impuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.IEPS;
                                break;
                            case "ISH":
                                impuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.ISH;
                                break;
                            case "Cargos no gravables":
                                impuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto.CargosNoGravables;
                                break;
                        }

                        if (lblCalculo.Text != string.Empty)
                            impuestosTraslado.importe = Convert.ToDecimal(HttpUtility.HtmlDecode(lblCalculo.Text).ToString().Replace("$", "").Replace(",", ""));
                        if (lblTasaVal.Text != string.Empty)
                            impuestosTraslado.tasa = Convert.ToDecimal(HttpUtility.HtmlDecode(lblTasaVal.Text).ToString().Replace(" %", ""));

                        impuestos.totalImpuestosTrasladados += Convert.ToDecimal(impuestosTraslado.importe); 

                        listaImpTraslado.Add(impuestosTraslado);
                    }
                    else
                    {
                        impuestosRetencion = new ComprobanteImpuestosRetencion();

                        if (lblTipoImpuesto.Text.ToString().Contains("IVA"))
                            lblTipoImpuesto.Text = "IVA";

                        if (lblTipoImpuesto.Text.ToString().Contains("ISR"))
                            lblTipoImpuesto.Text = "ISR";


                        switch (lblTipoImpuesto.Text)
                        {
                            case "IVA":
                                impuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto.IVA; 
                                break;
                            case "ISR":
                                impuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto.ISR;
                                break;     
                        }

                        if (lblCalculo.Text != string.Empty)
                            impuestosRetencion.importe = Convert.ToDecimal(HttpUtility.HtmlDecode(lblCalculo.Text).ToString().Replace("$", "").Replace(",", ""));

                        impuestos.totalImpuestosRetenidos += Convert.ToDecimal(impuestosRetencion.importe);

                        listaImpRetencion.Add(impuestosRetencion);
                    }
                }


            }

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, DateTime.Now, sTitle + "|" + "8.-impuestos" + "|" + "Se crea los impuestos en archivo XML");

            if (listaImpRetencion.Count > 0)
            {
                impuestos.Retenciones = listaImpRetencion.ToArray();
                impuestos.totalImpuestosRetenidosSpecified = true;
                impuestos.totalImpuestosRetenidos = impuestos.totalImpuestosRetenidos;
            }

            if (listaImpTraslado.Count > 0)
            {
                impuestos.Traslados = listaImpTraslado.ToArray();
                impuestos.totalImpuestosTrasladadosSpecified = true;
                impuestos.totalImpuestosTrasladados = impuestos.totalImpuestosTrasladados;
            }

            cfd.Conceptos = listaConcepto.ToArray();
            cfd.Emisor = emisor;
            cfd.Receptor = receptor;
            cfd.Impuestos = impuestos;
            
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        return cfd;
    }


    /// <summary>
    /// Genera el objeto del comprobante Version 3.0
    /// </summary>
    /// <param name="sVersion"></param>
    /// <param name="sFolio"></param>
    /// <param name="sTipoComprobante"></param>
    /// <param name="sSubtotal"></param>
    /// <param name="sTotal"></param>
    /// <param name="sSerie"></param>
    /// <param name="moneda"></param>
    /// <param name="rfc"></param>
    /// <param name="snombre"></param>
    /// <param name="pais"></param>
    /// <param name="estado"></param>
    /// <param name="municipio"></param>
    /// <param name="localidad"></param>
    /// <param name="calle"></param>
    /// <param name="noExterior"></param>
    /// <param name="noInterior"></param>
    /// <param name="colonia"></param>
    /// <param name="codigoPostal"></param>
    /// <param name="grvGrid"></param>
    /// <param name="dtlDataList"></param>
    /// <returns>Retorna el objeto comprobante con los paramentros capturados.</returns>
    public Comprobante30 fnObtenerXML3_0(string sVersion,
                                        string sFolio,
                                        string sTipoComprobante,
                                        decimal sSubtotal, decimal sTotal, string sSerie, string moneda,
                                        string rfc,
                                        string snombre,
                                        string pais,
                                        string estado,
                                        string municipio,
                                        string localidad,
                                        string calle,
                                        string noExterior,
                                        string noInterior,
                                        string colonia,
                                        string codigoPostal,
                                        GridView grvGrid,
                                        DataList dtlDataList,
                                        byte[] bCertificado,
                                        int id_usuario,
                                        string sTitle,
                                        int nidSucursalFis)
    {

        ComprobanteEmisor30 emisor = new ComprobanteEmisor30();
        emisor.DomicilioFiscal = new t_UbicacionFiscal30();
        Comprobante30 cfd = new Comprobante30();
        clsValCertificado cer;
        ComprobanteReceptor30 receptor = new ComprobanteReceptor30();
        receptor.Domicilio = new t_Ubicacion30();
        clsOperacionCuenta gDAL = new clsOperacionCuenta();
        clsOperacionSucursales gOpeSuc = new clsOperacionSucursales();
        byte[] certificadoBinario;
        
        List<ComprobanteConcepto30> listaConcepto = new List<ComprobanteConcepto30>();
        ComprobanteConcepto30 concepto;

        ComprobanteImpuestos30 impuestos = new ComprobanteImpuestos30();
        ComprobanteImpuestosTraslado30 impuestosTraslado = new ComprobanteImpuestosTraslado30();
        ComprobanteImpuestosRetencion30 impuestosRetencion = new ComprobanteImpuestosRetencion30();

        Label lblTasaVal = new Label();
        Label lblTipoImpDoc = new Label();
        Label lblTipoImpuesto = new Label();
        Label lblCalculo = new Label();

        try
        {
            //Recupera datos del emisor.
            SqlDataReader sdrInfo = gDAL.fnObtenerDatosFiscales();
            SqlDataReader sdrInfoFis = gDAL.fnObtenerDatosFiscalesSuc(nidSucursalFis); //Obtiene rfc, razon social
            SqlDataReader sdrInfoSuc = gOpeSuc.fnObtenerDomicilioSuc(nidSucursalFis); //Obtiene direccion fiscal de la sucursal

            if (sdrInfoFis != null && sdrInfoFis.HasRows && sdrInfoFis.Read() && sdrInfo != null && sdrInfo.HasRows && sdrInfo.Read() && sdrInfoSuc != null && sdrInfoSuc.HasRows && sdrInfoSuc.Read())
            {
                if (fnReplaceCaracters(sdrInfoFis["rfc"].ToString()) != string.Empty)
                    emisor.rfc = fnReplaceCaracters(sdrInfoFis["rfc"].ToString());
                if (fnReplaceCaracters(sdrInfoFis["razon_social"].ToString()) != string.Empty)
                    emisor.nombre = fnReplaceCaracters(sdrInfoFis["razon_social"].ToString());
                if (fnReplaceCaracters(sdrInfo["pais"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.pais = fnReplaceCaracters(sdrInfo["pais"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["estado"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.estado = fnReplaceCaracters(sdrInfoSuc["estado"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["municipio"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.municipio = fnReplaceCaracters(sdrInfoSuc["municipio"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["localidad"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.localidad = fnReplaceCaracters(sdrInfoSuc["localidad"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["calle"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.calle = fnReplaceCaracters(sdrInfoSuc["calle"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["numero_exterior"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.noExterior = fnReplaceCaracters(sdrInfoSuc["numero_exterior"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["numero_interior"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.noInterior = fnReplaceCaracters(sdrInfoSuc["numero_interior"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["colonia"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.colonia = fnReplaceCaracters(sdrInfoSuc["colonia"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["codigo_postal"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.codigoPostal = fnReplaceCaracters(sdrInfoSuc["codigo_postal"].ToString());
                if (fnReplaceCaracters(sdrInfoSuc["referencia"].ToString()) != string.Empty)
                    emisor.DomicilioFiscal.referencia = fnReplaceCaracters(sdrInfoSuc["referencia"].ToString());

                //if (fnReplaceCaracters(sdrInfo["rfc"].ToString()) != string.Empty)
                //    emisor.rfc = fnReplaceCaracters(sdrInfo["rfc"].ToString());
                //if (fnReplaceCaracters(sdrInfo["razon_social"].ToString()) != string.Empty)
                //    emisor.nombre = fnReplaceCaracters(sdrInfo["razon_social"].ToString());
                //if (fnReplaceCaracters(sdrInfo["pais"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.pais = fnReplaceCaracters(sdrInfo["pais"].ToString());
                //if (fnReplaceCaracters(sdrInfo["estado"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.estado = fnReplaceCaracters(sdrInfo["estado"].ToString());
                //if (fnReplaceCaracters(sdrInfo["municipio"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.municipio = fnReplaceCaracters(sdrInfo["municipio"].ToString());
                //if (fnReplaceCaracters(sdrInfo["localidad"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.localidad = fnReplaceCaracters(sdrInfo["localidad"].ToString());
                //if (fnReplaceCaracters(sdrInfo["calle"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.calle = fnReplaceCaracters(sdrInfo["calle"].ToString());
                //if (fnReplaceCaracters(sdrInfo["numero_exterior"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.noExterior = fnReplaceCaracters(sdrInfo["numero_exterior"].ToString());
                //if (fnReplaceCaracters(sdrInfo["numero_interior"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.noInterior = fnReplaceCaracters(sdrInfo["numero_interior"].ToString());
                //if (fnReplaceCaracters(sdrInfo["colonia"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.colonia = fnReplaceCaracters(sdrInfo["colonia"].ToString());
                //if (fnReplaceCaracters(sdrInfo["codigo_postal"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.codigoPostal = fnReplaceCaracters(sdrInfo["codigo_postal"].ToString());
                //if (fnReplaceCaracters(sdrInfo["referencia"].ToString()) != string.Empty)
                //    emisor.DomicilioFiscal.referencia = fnReplaceCaracters(sdrInfo["referencia"].ToString());

            }

            //Obtener el certificado
            certificadoBinario = Utilerias.Encriptacion.DES3.Desencriptar(bCertificado);

            cer = new clsValCertificado(certificadoBinario);

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, DateTime.Now, sTitle + "|" + "5.-cfd" + "|" + "Se crea el ancabezado del documento XML");

            //Parte inicial del CFDI
            cfd.version = "3.0";
            if (sSerie != string.Empty)
                cfd.serie = sSerie;
            if (sFolio != string.Empty)
                cfd.folio = sFolio;
            cfd.fecha = Convert.ToDateTime(DateTime.Now.ToString("s"));
            cfd.formaDePago = "Pago en una sola exhibicion";
            cfd.noCertificado = cer.ObtenerNoCertificado();
            cfd.certificado = Convert.ToBase64String(certificadoBinario);
            cfd.subTotal = sSubtotal;
            cfd.Moneda = moneda;
            cfd.total = sTotal;

            //Detalle del tipo de comprobante I,E,T
            switch (sTipoComprobante)
            {
                case "I":
                    cfd.tipoDeComprobante  = ComprobanteTipoDeComprobante30.ingreso;
                    break;
                case "E":
                    cfd.tipoDeComprobante = ComprobanteTipoDeComprobante30.egreso;
                    break;
                case "T":
                    cfd.tipoDeComprobante = ComprobanteTipoDeComprobante30.traslado;
                    break;
                default:
                    cfd.tipoDeComprobante = ComprobanteTipoDeComprobante30.ingreso;
                    break;
            }

            //Datos del receptor

            if (rfc != string.Empty)
                receptor.rfc = rfc;
            if (snombre != string.Empty)
                receptor.nombre = snombre;
            if (calle != string.Empty)
                receptor.Domicilio.calle = calle;
            if (noInterior != string.Empty)
                receptor.Domicilio.noInterior = noInterior;
            if (noExterior != string.Empty)
                receptor.Domicilio.noExterior = noExterior;
            if (colonia != string.Empty)
                receptor.Domicilio.colonia = colonia;
            if (localidad != string.Empty)
                receptor.Domicilio.localidad = localidad;
            if (municipio != string.Empty)
                receptor.Domicilio.municipio = municipio;
            if (estado != string.Empty)
                receptor.Domicilio.estado = estado;
            if (pais != string.Empty)
                receptor.Domicilio.pais = pais;
            if (codigoPostal != string.Empty)
                receptor.Domicilio.codigoPostal = codigoPostal;

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, DateTime.Now, sTitle + "|" + "6.-receptor" + "|" + "Se crea los datos del receptor en archivo XML");


            //Creacion de conceptos dinamicamente.
            if (grvGrid.Rows.Count > 0)
            {
                foreach (GridViewRow item in grvGrid.Rows)
                {

                    concepto = new ComprobanteConcepto30();

                    if (item.Cells[3].Text != string.Empty)
                        concepto.noIdentificacion = HttpUtility.HtmlDecode(item.Cells[3].Text);
                    if (item.Cells[4].Text != string.Empty)
                        concepto.unidad = HttpUtility.HtmlDecode(item.Cells[4].Text);
                    if (item.Cells[5].Text != string.Empty)
                        concepto.descripcion = HttpUtility.HtmlDecode(item.Cells[5].Text);
                    if (item.Cells[6].Text != string.Empty)
                        concepto.valorUnitario = Convert.ToDecimal(HttpUtility.HtmlDecode(item.Cells[6].Text).ToString().Replace("$", "").Replace(",", ""));
                    if (item.Cells[7].Text != string.Empty)
                        concepto.cantidad = Convert.ToDecimal(HttpUtility.HtmlDecode(item.Cells[7].Text));
                    if (item.Cells[8].Text != string.Empty)
                        concepto.importe = Convert.ToDecimal(HttpUtility.HtmlDecode(item.Cells[8].Text).ToString().Replace("$", "").Replace(",", ""));

                    listaConcepto.Add(concepto);
                }
            }

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, DateTime.Now, sTitle + "|" + "7.-conceptos" + "|" + "Se crea los conceptos en archivo XML");

            List<ComprobanteImpuestosTraslado30> listaImpTraslado = new List<ComprobanteImpuestosTraslado30>();
            List<ComprobanteImpuestosRetencion30> listaImpRetencion = new List<ComprobanteImpuestosRetencion30>();

            //Creacion de Impuestos dinamicamente.
            if (dtlDataList.Items.Count > 0)
            {
                foreach (DataListItem item in dtlDataList.Items)
                {

                    lblTasaVal = (Label)dtlDataList.Items[item.ItemIndex].FindControl("lblTasaVal");
                    lblTipoImpDoc = (Label)dtlDataList.Items[item.ItemIndex].FindControl("lblTipoImpDoc");
                    lblTipoImpuesto = (Label)dtlDataList.Items[item.ItemIndex].FindControl("lblTipoImpuesto");
                    lblCalculo = (Label)dtlDataList.Items[item.ItemIndex].FindControl("lblCalculo");


                    if (lblTipoImpDoc.Text == "Traslado" && lblTipoImpDoc.Text != string.Empty)
                    {

                        impuestosTraslado = new ComprobanteImpuestosTraslado30();

                        if (lblTipoImpuesto.Text.ToString().Contains("IVA"))
                            lblTipoImpuesto.Text = "IVA";

                        if (lblTipoImpuesto.Text.ToString().Contains("IEPS"))
                            lblTipoImpuesto.Text = "IEPS";

                        switch (lblTipoImpuesto.Text)
                        {
                            case "IVA":
                                impuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto30.IVA;
                                break;
                            case "IEPS":
                                impuestosTraslado.impuesto = ComprobanteImpuestosTrasladoImpuesto30.IEPS;
                                break;
                        }

                        if (lblCalculo.Text != string.Empty)
                            impuestosTraslado.importe = Convert.ToDecimal(HttpUtility.HtmlDecode(lblCalculo.Text).ToString().Replace("$", "").Replace(",", ""));
                        if (lblTasaVal.Text != string.Empty)
                            impuestosTraslado.tasa = Convert.ToDecimal(HttpUtility.HtmlDecode(lblTasaVal.Text).ToString().Replace(" %", ""));

                        listaImpTraslado.Add(impuestosTraslado);
                    }
                    else
                    {
                        impuestosRetencion = new ComprobanteImpuestosRetencion30();

                        if (lblTipoImpuesto.Text.ToString().Contains("IVA"))
                            lblTipoImpuesto.Text = "IVA";

                        if (lblTipoImpuesto.Text.ToString().Contains("ISR"))
                            lblTipoImpuesto.Text = "ISR";


                        switch (lblTipoImpuesto.Text)
                        {
                            case "IVA":
                                impuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto30.IVA;
                                break;
                            case "ISR":
                                impuestosRetencion.impuesto = ComprobanteImpuestosRetencionImpuesto30.ISR;
                                break;
                        }

                        if (lblCalculo.Text != string.Empty)
                            impuestosRetencion.importe = Convert.ToDecimal(HttpUtility.HtmlDecode(lblCalculo.Text).ToString().Replace("$", "").Replace(",", ""));

                        listaImpRetencion.Add(impuestosRetencion);
                    }
                }


            }

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, DateTime.Now, sTitle + "|" + "8.-impuestos" + "|" + "Se crea los impuestos en archivo XML");

            if (listaImpRetencion.Count > 0)
                impuestos.Retenciones = listaImpRetencion.ToArray();
            if (listaImpTraslado.Count > 0)
                impuestos.Traslados = listaImpTraslado.ToArray();

            cfd.Conceptos = listaConcepto.ToArray();
            cfd.Emisor = emisor;
            cfd.Receptor = receptor;
            cfd.Impuestos = impuestos;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        return cfd;
    }

    /// <summary>
    /// Agrega el comprobante a la BD
    /// </summary>
    /// <param name="sXML">Comprobante</param>
    /// <param name="nId_tipo_documento">Tipo de documento</param>
    /// <param name="cEstatus">estatus de generacion</param>
    /// <param name="dFecha_Documento">fecha de generacion</param>
    /// <param name="nId_estructura">id de estructura</param>
    /// <param name="nId_usuario_timbrado">id de usuario que genera</param>
    /// <param name="nSerie">Serie a generar el folio</param>
    /// <returns></returns>
    public static int fnInsertarComprobante(   
                                                string sXML, 
                                                int nId_tipo_documento, 
                                                char cEstatus, 
                                                DateTime dFecha_Documento,
                                                int nId_estructura, 
                                                int nId_usuario_timbrado,
                                                string nSerie,
                                                string psOrigen,
                                                string HASHTimbre,
                                                string HASHEmisor         
                                            )
    {

        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
        int nRetorno = 0;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {

            

            con.Open();
            using (SqlTransaction tran = con.BeginTransaction())
            {

                try
                {

                    SqlCommand cmd = new SqlCommand("usp_Timbrado_InsertaComprobante_Ins", con);

                    cmd.Transaction = tran;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("sXML", sXML);
                    cmd.Parameters.AddWithValue("nId_tipo_documento", nId_tipo_documento);
                    cmd.Parameters.AddWithValue("cEstatus", cEstatus);
                    cmd.Parameters.AddWithValue("dFecha_Documento", dFecha_Documento);
                    cmd.Parameters.AddWithValue("nId_estructura", nId_estructura);
                    cmd.Parameters.AddWithValue("nId_usuario_timbrado", nId_usuario_timbrado);
                    cmd.Parameters.AddWithValue("nSerie", nSerie);
                    cmd.Parameters.AddWithValue("sOrigen", psOrigen);
                    cmd.Parameters.AddWithValue("sHash", HASHTimbre.ToUpper());
                    cmd.Parameters.AddWithValue("sDatos", HASHEmisor.ToUpper());


                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                }
            }
        }
        return nRetorno;
    }



    /// <summary>
    /// Anexo 20 Eliminar en la reglas de estructura.
    /// </summary>
    /// <param name="varRep"></param>
    /// <returns></returns>
    public static string fnReplaceCaracters(string varRep)
    {
        string sReplace = string.Empty;

        if (varRep.Contains('&'))
        {
            varRep.Replace("&", "&amp;");
        }

        if (varRep.Contains('<'))
        {
            varRep.Replace("<", "&lt;");
        }

        if (varRep.Contains('>'))
        {
            varRep.Replace(">", "&gt;");
        }

        if (varRep.Contains("'"))
        {
            varRep.Replace("'", "&apos;");
        }

        if (varRep.Contains("\""))
        {
            varRep.Replace("\"", "&quot;");
        }
                
        sReplace = varRep;
        return sReplace;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="psTipoDocumento"></param>
    /// <returns></returns>
    public static int fnBuscarTipoDocumento(string psTipoDocumento)
    {
        Utilerias.SQL.InterfazSQL iSql = clsComun.fnCrearConexion("conTimbrado");
        iSql.AgregarParametro("sTipoDocumento", psTipoDocumento);
        return Convert.ToInt32(iSql.TraerEscalar("usp_Cfd_Busqueda_Documento_Sel", true));
    }

    /// <summary>
    /// Funcione encargada de actualizar el estatus del comprobante
    /// </summary>
    /// <param name="psRetornoInsert"></param>
    /// <returns></returns>
    public static int fnActualizaComprobante(int psRetornoInsert,string estatus)
    {
        Utilerias.SQL.InterfazSQL iSql = clsComun.fnCrearConexion("conTimbrado");
        iSql.AgregarParametro("@nRetornoInsert", psRetornoInsert);
        iSql.AgregarParametro("@sEstatus", estatus);
        return iSql.NoQuery("usp_Timbrado_ActualizaComprobante_Upd", true);
    }

    /// <summary>
    /// Recupera el HASH que exista en los comprobantes.
    /// </summary>
    /// <param name="nId_usuario_timbrado"></param>
    /// <param name="HASH"></param>
    /// <returns></returns>
    public static bool fnBuscarHashComprobantes(int nId_usuario_timbrado,string HASH,string tipo)
    {
        Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conTimbrado");

        int nRetorno = 0;
        bool bRetorno=false;

        try
        {
            giSql.AgregarParametro("nId_usuario_timbrado", nId_usuario_timbrado);
            giSql.AgregarParametro("sHash", HASH);
            giSql.AgregarParametro("sTipo", tipo);
            nRetorno = (Convert.ToInt32(giSql.TraerEscalar("usp_Timbrado_BuscaHASH_Sel", true)));
            if (nRetorno >0)
                bRetorno=true;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return bRetorno;
    }

    /// <summary>
    /// Regresa el comprobante si encuntra el hash
    /// </summary>
    /// <param name="nId_usuario_timbrado"></param>
    /// <param name="HASH"></param>
    /// <param name="tipo"></param>
    /// <returns></returns>
    public static string fnBuscarHashCompXML(int nId_usuario_timbrado, string HASH, string tipo)
    {
        Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conTimbrado");

        string bRetorno = String.Empty;

        try
        {
            giSql.AgregarParametro("nId_usuario_timbrado", nId_usuario_timbrado);
            giSql.AgregarParametro("sHash", HASH);
            giSql.AgregarParametro("sTipo", tipo);
            bRetorno = giSql.TraerEscalar("usp_Timbrado_BuscaHASH_XML_Sel", true).ToString();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return bRetorno;
    }

    /// <summary>
    /// Recupera la cantidad de creditos para el usuario registrado
    /// </summary>
    /// <param name="id_usuario"></param>
    /// <param name="descripcion"></param>
    /// <param name="estatus"></param>
    /// <param name="master"></param>
    /// <returns></returns>
    public static DataTable fnObtenerCreditos(string id_usuario,string descripcion, char estatus,char master)
    {
        Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conTimbrado");
        DataTable gdtAuxiliar = new DataTable();

        try
        {
            giSql.AgregarParametro("psId_usuario", id_usuario);
            giSql.AgregarParametro("psDescripcion", descripcion);
            giSql.AgregarParametro("sEstatus", estatus);
            giSql.AgregarParametro("sMaster", master);

            giSql.Query("usp_Ctp_Servicios_Recupera_Sel", true, ref gdtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return gdtAuxiliar;
    }

    /// <summary>
    /// Actualiza los creditos disponibles
    /// </summary>
    /// <param name="id_credito"></param>
    /// <param name="id_estructura"></param>
    /// <param name="creditos"></param>
    /// <returns></returns>
    public static int fnActualizarCreditos(int id_credito, int id_estructura, double creditos, string servicio)
    {

        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
        int nRetorno = 0;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {

            

            con.Open();
            using (SqlTransaction tran = con.BeginTransaction())
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("usp_Ctp_Servicios_Actualiza_Creditos_Upd", con);

                    cmd.Transaction = tran;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("id_credito", id_credito);
                    cmd.Parameters.AddWithValue("id_estructura", id_estructura);
                    cmd.Parameters.AddWithValue("creditos", creditos);
                    cmd.Parameters.AddWithValue("sServicio", servicio);

                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                }
            }
        }
        return nRetorno;
    }


    /// <summary>
    /// Funcione encargada de actualizar el xml con addenda del comprobante
    /// </summary>
    /// <param name="psRetornoInsert"></param>
    /// <returns></returns>
    public static int fnActualizaComprobanteAddenda(int psRetornoInsert, string xmldocumento)
    {
        Utilerias.SQL.InterfazSQL iSql = clsComun.fnCrearConexion("conTimbrado");
        iSql.AgregarParametro("@nRetornoInsert", psRetornoInsert);
        iSql.AgregarParametro("@sXmlAddenda", xmldocumento);
        return iSql.NoQuery("usp_Timbrado_ActualizaComprobanteAddenda_Upd", true);
    }

    /// <summary>
    /// Actualiza los creditos disponibles
    /// </summary>
    /// <param name="id_credito"></param>
    /// <param name="id_estructura"></param>
    /// <param name="creditos"></param>
    /// <returns></returns>
    public static int fnActualizarCreditosHistorico(int id_credito, int id_estructura, double creditos)
    {

        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString;
        int nRetorno = 0;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {

            

            con.Open();
            using (SqlTransaction tran = con.BeginTransaction())
            {

                try
                {
                    SqlCommand cmd = new SqlCommand("usp_Ctp_Servicios_Actualiza_Creditos_Historico_Upd", con);

                    cmd.Transaction = tran;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("id_credito", id_credito);
                    cmd.Parameters.AddWithValue("id_estructura", id_estructura);
                    cmd.Parameters.AddWithValue("creditos", creditos);

                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                }
            }
        }
        return nRetorno;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;

/// <summary>
/// Clase de capa de negocios para la pantalla webOperacionConsulta
/// </summary>
public class clsOperacionConsulta
{
    private DataTable dtAuxiliar;
    private string conSucursal = "conConsultas";

    /// <summary>
    /// Actualiza el comprobante especificado poniendo su estatus a 'Cancelado'
    /// </summary>
    /// <param name="psIdCfd">Identificador del comprobante a cancelar</param>
    /// <returns></returns>
    public int fnCancelarComprobante(int psIdCfd, string comentario)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursal].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Cancela_Comprobante_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Cfd", psIdCfd));
                    cmd.Parameters.Add(new SqlParameter("nnIdUsuario", clsComun.fnUsuarioEnSesion().nIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("sComentarioCancelacion", comentario));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnCancelarComprobante", "clsOperacionConsulta");
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Realiza la búsqueda de comprobantes bajo los filtros especificados
    /// </summary>
    /// <param name="psRfc">Cadena con el RFC del receptor que se quiere buscar. Cero es todos</param>
    /// <param name="psEstatus">Clave del estatus a buscar. Cero es todos</param>
    /// <param name="psIdSucursal">Identificador de la sucursal a buscar. Cero es todos</param>
    /// <param name="psIdDocumento">Identificador del documento a buscar. Cero es todos</param>
    /// <param name="psSerie">Cadena de la serie que se quiere buscar. Cero es todos</param>
    /// <param name="psFolioIni">Número de folio desde el cuál se realizará la búsqueda. Si se omite no abrá límite inferior</param>
    /// <param name="psFolioFin">Número de folio hasta el cuál se realizará la búsqueda. Si se omite no abrá límite superior</param>
    /// <param name="pdFechaIni">Fecha desde la cuál se realizará la búsqueda.</param>
    /// <param name="pdFechaFin">Fecha hasta la cuál se realizará la búsqueda.</param>
    /// <param name="sUUID">UUID del Comprobante</param>
    /// <param name="pnPagina">Página a visualizar</param>
    /// <param name="pnNumeroRegistros">Número de registros a visualizar</param>
    /// <returns>Retorna un DataTable con los resultados de la búsqueda</returns>
    public DataTable fnObtenerComprobantes(string psRfc, string psEstatus, string psIdSucursal, string psIdDocumento, string psSerie, string psFolioIni, string psFolioFin, DateTime pdFechaIni, DateTime pdFechaFin, string sUUID,
                                            int pnPagina, int pnNumeroRegistros)
    {
        DataTable dtTabla = new DataTable();
        bool berror = false;
        string sMensajeError = string.Empty;
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursal].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_Cfd_Comprobantes_Sel";

                    scoComando.Parameters.AddWithValue("@nnIdUsuario", clsComun.fnUsuarioEnSesion().nIdUsuario);
                    scoComando.Parameters.AddWithValue("@dFecha_Inicio", pdFechaIni);
                    scoComando.Parameters.AddWithValue("@dFecha_Fin", pdFechaFin);

                    // Verificamos los demás parametros para decidir
                    // si se usarán como filtros
                    if (psIdSucursal != "0")
                        scoComando.Parameters.AddWithValue("nId_Estructura", psIdSucursal);

                    if (psIdDocumento != "0")
                        scoComando.Parameters.AddWithValue("nId_Tipo_Documento", psIdDocumento);

                    if (psEstatus != "0")
                        scoComando.Parameters.AddWithValue("sEstatus", psEstatus);

                    if (psRfc != "0")
                        scoComando.Parameters.AddWithValue("sRfc_Receptor", psRfc);

                    if (psSerie != Resources.resCorpusCFDIEs.VarDropTodos)
                        scoComando.Parameters.AddWithValue("sSerie", psSerie);

                    if (!string.IsNullOrEmpty(psFolioIni))
                        scoComando.Parameters.AddWithValue("nFolio_Inicio", psFolioIni);

                    if (!string.IsNullOrEmpty(psFolioFin))
                        scoComando.Parameters.AddWithValue("nFolio_Fin", psFolioFin);

                    if (!string.IsNullOrEmpty(sUUID))
                        scoComando.Parameters.AddWithValue("sUUID", sUUID);

                    scoComando.Parameters.AddWithValue("@nPagina", pnPagina);
                    scoComando.Parameters.AddWithValue("@nNumeroRegistros", pnNumeroRegistros);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtTabla);
                    }
                }
            }
            catch (Exception ex)
            {
                berror = true;
                sMensajeError = ex.Message;
            }
            finally
            {
                scConexion.Close();
                if (berror)
                {
                    throw new Exception(sMensajeError);
                }
            }
            return dtTabla;
        }

    }

    /// <summary>
    /// Retorna el objeto XmlDocumento con la información del comprobante XML especificado
    /// </summary>
    /// <param name="psIdCfd">Identificador del comprobante  abuscar</param>
    /// <returns>XmlDocumento con la información del comprobante</returns>
    public XmlDocument fnObtenerComprobanteXML(int pnIdContribuyente, string psIdCfd)
    {
        XmlDocument xComprobante = new XmlDocument();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursal].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Comprobante_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Cfd", psIdCfd));
                    cmd.Parameters.Add(new SqlParameter("nId_Contribuyente", pnIdContribuyente));

                    con.Open();
                    xComprobante.LoadXml(Convert.ToString(cmd.ExecuteScalar()));
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerComprobanteXML", "clsOperacionConsulta");
            }
        }
        return xComprobante;
    }

    /// <summary>
    /// Retorna la lista de documentos existentes en el sistema, más la opción 'todos'
    /// </summary>
    /// <returns>DataTable con la lista de documentos</returns>
    public DataTable fnObtenerDocumentos()
    {
        clsOperacionDocImpuestos documentos = new clsOperacionDocImpuestos();

        dtAuxiliar = documentos.fnCargarTiposDocumento();
        DataRow drFila = dtAuxiliar.NewRow();
        drFila["id_tipo_documento"] = 0;
        drFila["nombre"] = Resources.resCorpusCFDIEs.VarDropTodos;

        dtAuxiliar.Rows.InsertAt(drFila, 0);

        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de estatus posibles para consultar, más la opción 'todos'
    /// </summary>
    /// <returns>DataTable con la lista de estatus</returns>
    public DataTable fnObtenerEstatus()
    {
        DataTable dtAuxiliar = new DataTable();
        dtAuxiliar.Columns.Add("estatus", typeof(string));
        dtAuxiliar.Columns.Add("clave", typeof(string));

        dtAuxiliar.Rows.Add(Resources.resCorpusCFDIEs.VarDropTodos, "0");
        dtAuxiliar.Rows.Add(Resources.resCorpusCFDIEs.varEstatusActivo, "A");
        dtAuxiliar.Rows.Add(Resources.resCorpusCFDIEs.varEstatusCancelado, "C");

        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de receptores disponibles para el usuario, más la opción de 'todos'
    /// </summary>
    /// <returns>DataTable con la lista de receptores</returns>
    public DataTable fnObtenerReceptores()
    {
        clsOperacionClientes receptores = new clsOperacionClientes();
        dtAuxiliar = receptores.fnLlenarReceptores();

        DataRow drFila = dtAuxiliar.NewRow();
        drFila["rfc_receptor"] = "0";
        drFila["nombre_receptor"] = Resources.resCorpusCFDIEs.VarDropTodos;

        dtAuxiliar.Rows.InsertAt(drFila, 0);
        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de series disponibles para el contribuyente filtradas según se especifique, más la opción 'todos'
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la estructura de la cuál se quieren obtener sus series</param>
    /// <param name="psIdDocumento">Identificador del documento del cuál se quieren obtener sus series</param>
    /// <returns>DataTable con la lista de series</returns>
    public DataTable fnObtenerSeries(string psIdEstructura, string psIdDocumento)
    {
        clsOperacionSeriesFolios documentos = new clsOperacionSeriesFolios();

        if (psIdEstructura == "0")
            psIdEstructura = string.Empty;

        if (psIdDocumento == "0")
            psIdDocumento = string.Empty;
        clsInicioSesionUsuario datosUsuario = clsComun.fnUsuarioEnSesion();
        dtAuxiliar = documentos.fnObtenerSeries(psIdEstructura, psIdDocumento, datosUsuario.nIdUsuario.ToString());
        DataRow drFila = dtAuxiliar.NewRow();
        drFila["id_serie"] = 0;
        drFila["serie"] = Resources.resCorpusCFDIEs.VarDropTodos;

        dtAuxiliar.Rows.InsertAt(drFila, 0);

        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de sucursales disponibles para el contribuyente, más la opción 'todos'
    /// </summary>
    /// <returns>DataTable con la lista de sucursales</returns>
    public DataTable fnObtenerSucursales()
    {
        dtAuxiliar = clsComun.fnLlenarDropSucursales(true);
        DataRow drFila = dtAuxiliar.NewRow();
        drFila["id_estructura"] = 0;
        drFila["nombre"] = Resources.resCorpusCFDIEs.VarDropTodos;

        dtAuxiliar.Rows.InsertAt(drFila, 0);

        return dtAuxiliar;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="linea"></param>
    /// <returns></returns>
    public string fnReemplaza(string linea)
    {
        linea = linea.Replace("Ã¡", "á");
        linea = linea.Replace("Ã©", "é");
        linea = linea.Replace("Ã­", "í");
        linea = linea.Replace("Ã³", "ó");
        linea = linea.Replace("Ã¨", "è");
        linea = linea.Replace("Ã", "Á");
        linea = linea.Replace("Ã", "É");
        linea = linea.Replace("Ã", "Í");
        linea = linea.Replace("Ã", "Ó");
        linea = linea.Replace("Ã?", "Ú");
        linea = linea.Replace("Ã", "Ñ");
        linea = linea.Replace("Ã±", "ñ");
        linea = linea.Replace("Âº", "º");
        linea = linea.Replace("Ã?ÃÂ¨", "É");
        linea = linea.Replace("Ã??", "É");
        linea = linea.Replace("ÃÂ©", "é");
        linea = linea.Replace("ÃÂ?ÃÂÃÂ¨", "é");
        linea = linea.Replace("ÃÂ", "É");
        linea = linea.Replace("ÃÂ", "Ó");
        linea = linea.Replace("&amp;", "&");
        linea = linea.Replace("&lt;", "<");
        linea = linea.Replace("&gt;", ">");
        linea = linea.Replace("&#39;", "'");
        linea = linea.Replace("&#32;", "");
        linea = linea.Replace("&#33;;", "!");
        // linea.Replace("&#34;", "");
        linea = linea.Replace("&#35;", "#");
        linea = linea.Replace("&#36;", "$");
        linea = linea.Replace("&#37;", "%");
        linea = linea.Replace("&#38;", "&");
        linea = linea.Replace("&#40;", "(");
        linea = linea.Replace("&#41;", ")");
        linea = linea.Replace("&#42;", "*");
        linea = linea.Replace("&#43;", "+");
        linea = linea.Replace("&#44;", ",");
        linea = linea.Replace("&#45;", "-");
        linea = linea.Replace("&#46;", ".");
        linea = linea.Replace("&#47;", "/");
        linea = linea.Replace("&#64;", "@");
        linea = linea.Replace("&#96;", "`");
        linea = linea.Replace("&#123;", "{");
        linea = linea.Replace("&#124;", "|");
        linea = linea.Replace("&#32;", "}");
        linea = linea.Replace("&#126;", "~");
        linea = linea.Replace("&#160;", "");
        linea = linea.Replace("&#161;", "¡");
        linea = linea.Replace("&#162;", "¢");
        linea = linea.Replace("&#163;", "£");
        linea = linea.Replace("&#164;", "¤");
        linea = linea.Replace("&#165;", "¥");
        linea = linea.Replace("&#166;", "¦");
        linea = linea.Replace("&#167;", "§");
        linea = linea.Replace("&#168;", "¨");
        linea = linea.Replace("&#169;", "©");
        linea = linea.Replace("&#170;", "ª");
        linea = linea.Replace("&#171;", "«");
        linea = linea.Replace("&#172;", "¬");
        linea = linea.Replace("&#173;", " ");
        linea = linea.Replace("&#174;", "®");
        linea = linea.Replace("&#175;", "¯");
        linea = linea.Replace("&nbsp;", "");
        linea = linea.Replace("&iexcl;", "¡");
        linea = linea.Replace("&cent;", "¢");
        linea = linea.Replace("&pound;", "£");
        linea = linea.Replace("&curren;", "¤");
        linea = linea.Replace("&yen;", "¥");
        linea = linea.Replace("&brvbar;", "¦");
        linea = linea.Replace("&sect;", "§");
        linea = linea.Replace("&uml;", "¨");
        linea = linea.Replace("&copy;", "©");
        linea = linea.Replace("&ordf;", "ª");
        linea = linea.Replace("&laquo;", "«");
        linea = linea.Replace("&not;", "¬");
        linea = linea.Replace("&shy;", " ");
        linea = linea.Replace("&reg;", "®");
        linea = linea.Replace("&macr;", "¯");
        linea = linea.Replace("&#176;", "°");
        linea = linea.Replace("&#177;", "±");
        linea = linea.Replace("&#178;", "²");
        linea = linea.Replace("&#179;", "³");
        linea = linea.Replace("&#180;", "´");
        linea = linea.Replace("&#181;", "µ");
        linea = linea.Replace("&#182;", "¶");
        linea = linea.Replace("&#183;", "·");
        linea = linea.Replace("&#184;", "¸");
        linea = linea.Replace("&#185;", "¹");
        linea = linea.Replace("&#186;", "º");
        linea = linea.Replace("&#187;", "»");
        linea = linea.Replace("&#188;", "¼");
        linea = linea.Replace("&#189;", "½");
        linea = linea.Replace("&#190;", "¾");
        linea = linea.Replace("&#191;", "¿");
        linea = linea.Replace("&deg;", "°");
        linea = linea.Replace("&plusmn;", "±");
        linea = linea.Replace("&sup2;", "²");
        linea = linea.Replace("&sup3;", "³");
        linea = linea.Replace("&acute;", "´");
        linea = linea.Replace("&micro;", "µ");
        linea = linea.Replace("&para;", "¶");
        linea = linea.Replace("&middot;", "·");
        linea = linea.Replace("&cedil;", "¸");
        linea = linea.Replace("&sup1;", "¹");
        linea = linea.Replace("&ordm;", "º");
        linea = linea.Replace("&raquo;", "»");
        linea = linea.Replace("&frac14;", "¼");
        linea = linea.Replace("&frac12;", "½");
        linea = linea.Replace("&frac34;", "¾");
        linea = linea.Replace("&iquest;", "¿");
        linea = linea.Replace("&#192;", "À");
        linea = linea.Replace("&#193;", "Á");
        linea = linea.Replace("&#194;", "Â");
        linea = linea.Replace("&#195;", "Ã");
        linea = linea.Replace("&#196;", "Ä");
        linea = linea.Replace("&#197;", "Å");
        linea = linea.Replace("&#198;", "Æ");
        linea = linea.Replace("&#199;", "Ç");
        linea = linea.Replace("&#200;", "È");
        linea = linea.Replace("&#201;", "É");
        linea = linea.Replace("&#202;", "Ê");
        linea = linea.Replace("&#203;", "Ë");
        linea = linea.Replace("&#204;", "Ì");
        linea = linea.Replace("&#205;", "Í");
        linea = linea.Replace("&#206;", "Î");
        linea = linea.Replace("&#207;", "Ï");
        linea = linea.Replace("&Agrave;", "À");
        linea = linea.Replace("&Aacute;", "Á");
        linea = linea.Replace("&Acirc;", "Â");
        linea = linea.Replace("&Atilde;", "Ã");
        linea = linea.Replace("&Auml;", "Ä");
        linea = linea.Replace("&Aring;", "Å");
        linea = linea.Replace("&AElig;", "Æ");
        linea = linea.Replace("&Ccedil;", "Ç");
        linea = linea.Replace("&Egrave;", "È");
        linea = linea.Replace("&Eacute;", "É");
        linea = linea.Replace("&Ecirc;", "Ê");
        linea = linea.Replace("&Euml;", "Ë");
        linea = linea.Replace("&Igrave;", "Ì");
        linea = linea.Replace("&Iacute;", "Í");
        linea = linea.Replace("&Icirc;", "Î");
        linea = linea.Replace("&Iuml;", "Ï");
        linea = linea.Replace("&#208;", "Ð");
        linea = linea.Replace("&#209;", "Ñ");
        linea = linea.Replace("&#210;", "Ò");
        linea = linea.Replace("&#211;", "Ó");
        linea = linea.Replace("&#212;", "Ô");
        linea = linea.Replace("&#213;", "Õ");
        linea = linea.Replace("&#214;", "Ö");
        linea = linea.Replace("&#215;", "×");
        linea = linea.Replace("&#216;", "Ø");
        linea = linea.Replace("&#217;", "Ù");
        linea = linea.Replace("&#218;", "Ú");
        linea = linea.Replace("&#219;", "Û");
        linea = linea.Replace("&#220;", "Ü");
        linea = linea.Replace("&#221;", "Ý");
        linea = linea.Replace("&#222;", "Þ");
        linea = linea.Replace("&#223;", "ß");
        linea = linea.Replace("&ETH;", "Ð");
        linea = linea.Replace("&Ntilde;", "Ñ");
        linea = linea.Replace("&Ograve;", "Ò");
        linea = linea.Replace("&Oacute;", "Ó");
        linea = linea.Replace("&Ocirc;", "Ô");
        linea = linea.Replace("&Otilde;", "Õ");
        linea = linea.Replace("&Ouml;", "Ö");
        linea = linea.Replace("&times;", "×");
        linea = linea.Replace("&Oslash;", "Ø");
        linea = linea.Replace("&Ugrave;", "Ù");
        linea = linea.Replace("&Uacute;", "Ú");
        linea = linea.Replace("&Ucirc;", "Û");
        linea = linea.Replace("&Uuml;", "Ü");
        linea = linea.Replace("&Yacute;", "Ý");
        linea = linea.Replace("&THORN;", "Þ");
        linea = linea.Replace("&szlig;", "ß");
        linea = linea.Replace("&#224;", "à");
        linea = linea.Replace("&#225;", "á");
        linea = linea.Replace("&#226;", "â");
        linea = linea.Replace("&#227;", "ã");
        linea = linea.Replace("&#228;", "ä");
        linea = linea.Replace("&#229;", "å");
        linea = linea.Replace("&#230;", "æ");
        linea = linea.Replace("&#231;", "ç");
        linea = linea.Replace("&#232;", "è");
        linea = linea.Replace("&#233;", "é");
        linea = linea.Replace("&#234;", "ê");
        linea = linea.Replace("&#235;", "ë");
        linea = linea.Replace("&#236;", "ì");
        linea = linea.Replace("&#237;", "í");
        linea = linea.Replace("&#238;", "î");
        linea = linea.Replace("&#239;", "ï");
        linea = linea.Replace("&agrave;", "à");
        linea = linea.Replace("&aacute;", "á");
        linea = linea.Replace("&acirc;", "â");
        linea = linea.Replace("&atilde;", "ã");
        linea = linea.Replace("&auml;", "ä");
        linea = linea.Replace("&aring;", "å");
        linea = linea.Replace("&aelig;", "æ");
        linea = linea.Replace("&ccedil;", "ç");
        linea = linea.Replace("&egrave;", "è");
        linea = linea.Replace("&eacute;", "é");
        linea = linea.Replace("&ecirc;", "ê");
        linea = linea.Replace("&euml;", "ë");
        linea = linea.Replace("&igrave;", "ì");
        linea = linea.Replace("&iacute;", "í");
        linea = linea.Replace("&icirc;", "î");
        linea = linea.Replace("&iuml;", "ï");
        linea = linea.Replace("&#240;", "ð");
        linea = linea.Replace("&#241;", "ñ");
        linea = linea.Replace("&#242;", "ò");
        linea = linea.Replace("&#243;", "ó");
        linea = linea.Replace("&#244;", "ô");
        linea = linea.Replace("&#245;", "õ");
        linea = linea.Replace("&#246;", "ö");
        linea = linea.Replace("&#247;", "÷");
        linea = linea.Replace("&#248;", "ø");
        linea = linea.Replace("&#249;", "ù");
        linea = linea.Replace("&#250;", "ú");
        linea = linea.Replace("&#251;", "û");
        linea = linea.Replace("&#252;", "ü");
        linea = linea.Replace("&#253;", "ý");
        linea = linea.Replace("&#254;", "þ");
        linea = linea.Replace("&#255;", "ÿ");
        linea = linea.Replace("&eth;", "ð");
        linea = linea.Replace("&ntilde;", "ñ");
        linea = linea.Replace("&ograve;", "ò");
        linea = linea.Replace("&oacute;", "ó");
        linea = linea.Replace("&ocirc;", "ô");
        linea = linea.Replace("&otilde;", "õ");
        linea = linea.Replace("&ouml;", "ö");
        linea = linea.Replace("&divide;", "÷");
        linea = linea.Replace("&oslash;", "ø");
        linea = linea.Replace("&ugrave;", "ù");
        linea = linea.Replace("&uacute;", "ú");
        linea = linea.Replace("&ucirc;", "û");
        linea = linea.Replace("&uuml;", "ü");
        linea = linea.Replace("&yacute;", "ý");
        linea = linea.Replace("&thorn;", "þ");
        linea = linea.Replace("&yuml;", "ÿ");

        return linea;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="XmlDoc"></param>
    /// <returns></returns>
    public XmlDocument fnReemplazaXML(XmlDocument XmlDoc)
    {

        byte[] byteArray = Encoding.ASCII.GetBytes(XmlDoc.OuterXml);
        MemoryStream stream = new MemoryStream(byteArray);
        stream.Position = 0;

        StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
        StringBuilder sw = new StringBuilder();
        while (!(sr.EndOfStream))
        {
            string linea = sr.ReadLine();
            char[] Arreglo = { };

            if (linea.Contains("<?xml"))
            {
                int pos = linea.IndexOf("<?xml");
                if (pos > -1)
                {
                    linea = linea.Substring(pos, linea.Length - pos);
                }
            }
            linea = fnReemplaza(linea);
            sw.Append(linea);
        }
        XmlDocument xmlfinal = new XmlDocument();
        xmlfinal.XmlResolver = null;
        sr.Close();
        try
        {
            xmlfinal.Load(sw.ToString());
        }
        catch (Exception)
        {
        }
        return xmlfinal;
    }   
}

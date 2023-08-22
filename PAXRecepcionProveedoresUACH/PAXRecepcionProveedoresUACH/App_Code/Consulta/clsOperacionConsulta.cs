using System;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Clase de capa de negocios para la pantalla webOperacionConsulta
/// </summary>
public class clsOperacionConsulta
{
    /// <summary>
    /// Retorna el objeto XmlDocumento con la información del comprobante XML especificado
    /// </summary>
    /// <param name="psIdCfd">Identificador del comprobante  abuscar</param>
    /// <returns>XmlDocumento con la información del comprobante</returns>
    public XmlDocument fnObtenerComprobanteXML(int pnIdContribuyente, string psIdCfd)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursal);
        //    XmlDocument xComprobante = new XmlDocument();
        //    giSql.AgregarParametro("nId_Comprobante", psIdCfd);
        //    //giSql.AgregarParametro("nId_Contribuyente", pnIdContribuyente);

        //    //xComprobante.LoadXml(HttpUtility.HtmlDecode(giSql.TraerEscalar("usp_Cfd_Comprobante_Sel", true).ToString()));
        //    xComprobante.LoadXml(giSql.TraerEscalar("usp_Cfd_Comprobante_Sel", true).ToString());


        //    return xComprobante;
        //}
        //catch
        //{
        //    return null;
        //}


        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Cfd_Comprobante_Sel";
                    XmlDocument xComprobante = new XmlDocument();
                    cmd.Parameters.Add(new SqlParameter("nId_Comprobante", psIdCfd));
                    xComprobante.LoadXml(Convert.ToString(cmd.ExecuteScalar()));
                    con.Close();
                    con.Dispose();
                    return xComprobante;
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    /// <summary>
    /// Retorna el objeto PDFDocumento con la información del comprobante XML especificado
    /// </summary>
    /// <param name="psIdCfd">Identificador del comprobante  abuscar</param>
    /// <returns>XmlDocumento con la información del comprobante</returns>
    public byte[] fnObtenerComprobantePDF(int pnIdContribuyente, string psIdCfd)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursal);
        //    XmlDocument xComprobante = new XmlDocument();
        //    giSql.AgregarParametro("nId_Comprobante", psIdCfd);
        //    //giSql.AgregarParametro("nId_Contribuyente", pnIdContribuyente);

        //    //xComprobante.LoadXml(HttpUtility.HtmlDecode(giSql.TraerEscalar("usp_Cfd_Comprobante_Sel", true).ToString()));

        //    return (byte[])giSql.TraerEscalar("usp_Cfd_Comprobante_PDF_Sel", true);
        //}
        //catch
        //{
        //    return null;
        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Cfd_Comprobante_PDF_Sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Comprobante", psIdCfd));
                    byte[] resultado = (byte[])cmd.ExecuteScalar();
                    con.Close();
                    con.Dispose();
                    return resultado;
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    public string reemplaza(string linea)
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
    /// Retorna el email con la información del comprobante XML especificado
    /// </summary>
    /// <param name="psIdCfd">Identificador del comprobante  abuscar</param>
    /// <returns>XmlDocumento con la información del comprobante</returns>
    public DataTable fnObtenerComprobanteEmail(string psIdCfd)
    {
        //try
        //{
        //    DataTable dtAuxiliar = new DataTable();
        //    giSql = clsComun.fnCrearConexion(conSucursal);
        //    giSql.AgregarParametro("nId_Comprobante", psIdCfd);
        //    giSql.Query("usp_Cfd_Comprobante_Sel",true,ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception)
        //{
        //    return null;
        //}

        DataTable dtAuxiliar = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Cfd_Comprobante_Sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Comprobante", psIdCfd));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
                return dtAuxiliar;
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }
}

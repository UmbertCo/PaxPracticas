using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsOperacionSeriesFolios
/// </summary>
public class clsOperacionSeriesFolios
{
    private string conSeries = "conConfiguracion";

    /// <summary>
    /// Inserta o actualiza una serie para la combinación estructura-documento
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la estructura</param>
    /// <param name="psIdTipoDocumento">Identificador del documento</param>
    /// <param name="psSerie">Nombre de la serie</param>
    /// <param name="psFolio">Número de folio</param>
    /// <returns>Un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnAgregarSerie(string psIdEstructura, string psIdTipoDocumento, string psSerie, string psFolio)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSeries].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Serie_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Estructura", psIdEstructura));
                    cmd.Parameters.Add(new SqlParameter("nId_Tipo_Documento", psIdTipoDocumento));
                    cmd.Parameters.Add(new SqlParameter("sSerie", psSerie.ToUpper().Trim()));
                    cmd.Parameters.Add(new SqlParameter("nFolio", psFolio));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnAgregarSerie", "clsOperacionSeriesFolios");
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Elimina de manera lógica una serie
    /// </summary>
    /// <param name="poIdSerie">Identificador de la serie</param>
    /// <param name="poSerie">Nombre de la serie</param>
    /// <param name="psIdTipoDocumento">Identificador del documento al que está asociada esta serie</param>
    /// <returns>Un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnEliminarSerie(object poIdSerie, object poSerie, string psIdTipoDocumento, int pnId_estructura)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSeries].ConnectionString)))
        {
            try
            {
                if (clsComun.fnUsuarioEnSesion() == null)
                    throw new Exception("La sesión del usuario es nula.") { Source = "clsOperacionSeriesFolios|fnEliminarSerie" };

                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Serie_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Serie", poIdSerie));
                    cmd.Parameters.Add(new SqlParameter("sSerie", poSerie));
                    cmd.Parameters.Add(new SqlParameter("nId_Tipo_Documento", psIdTipoDocumento));
                    cmd.Parameters.Add(new SqlParameter("nId_Contribuyente", clsComun.fnUsuarioEnSesion().nIdContribuyente));
                    cmd.Parameters.Add(new SqlParameter("nId_estructura", pnId_estructura));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnEliminarSerie", "clsOperacionSeriesFolios");
            }
        }
        return nResultado;
    }    

    /// <summary>
    /// Retorna la lista de series activas para la combinación estructura-documento
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la estructura</param>
    /// <param name="psIdTipoDocumento">Identificador del tipo de documento</param>
    /// <returns>DataTable con la lista de series</returns>
    public DataTable fnObtenerSeries(string psIdEstructura, string psIdTipoDocumento, string nIdUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSeries].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Serie_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(psIdEstructura))
                        cmd.Parameters.Add(new SqlParameter("nId_Estructura", psIdEstructura));
                    if (!string.IsNullOrEmpty(psIdTipoDocumento))
                        cmd.Parameters.Add(new SqlParameter("nId_Tipo_Documento", psIdTipoDocumento));
                    if (!string.IsNullOrEmpty(nIdUsuario))
                        cmd.Parameters.Add(new SqlParameter("nId_Usuario", nIdUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerSeries", "clsOperacionSeriesFolios");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Devuelve el listado de documentos previamente configurados por el contribuyente
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerTiposDocumentos()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSeries].ConnectionString)))
        {
            try
            {
                if (clsComun.fnUsuarioEnSesion() == null)
                    throw new Exception("La sesión del usuario es nula.") { Source = "clsOperacionSeriesFolios|fnObtenerTiposDocumentos" };

                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Documentos_Asignados_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", clsComun.fnUsuarioEnSesion().nIdUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerTiposDocumentos", "clsOperacionSeriesFolios");
            }
        }
        return dtResultado;
    }
}
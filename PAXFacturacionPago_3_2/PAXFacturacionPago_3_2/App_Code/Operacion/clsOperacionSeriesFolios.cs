using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for clsOperacionSeriesFolios
/// </summary>
public class clsOperacionSeriesFolios
{
    private string conSeries = "conConfiguracion";

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
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Documentos_Asignados_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener los tipos de documento." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conSeries);
        //DataTable dtAuxiliar = new DataTable();

        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.Query("usp_Cfd_Documentos_Asignados_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de series activas para la combinación estructura-documento
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la estructura</param>
    /// <param name="psIdTipoDocumento">Identificador del tipo de documento</param>
    /// <returns>DataTable con la lista de series</returns>
    public DataTable fnObtenerSeries(string psIdEstructura, string psIdTipoDocumento,string nIdUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSeries].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Serie_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(!string.IsNullOrEmpty(psIdEstructura))
                        cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);

                    if(!string.IsNullOrEmpty(psIdTipoDocumento))
                        cmd.Parameters.AddWithValue("nId_Tipo_Documento", psIdTipoDocumento);

                    if (!string.IsNullOrEmpty(nIdUsuario))
                        cmd.Parameters.AddWithValue("nId_Usuario", nIdUsuario);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener la lista de series." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conSeries);
        //DataTable dtAuxiliar  = new DataTable();

        //if(!string.IsNullOrEmpty(psIdEstructura))
        //    giSql.AgregarParametro("nId_Estructura", psIdEstructura);

        //if(!string.IsNullOrEmpty(psIdTipoDocumento))
        //    giSql.AgregarParametro("nId_Tipo_Documento", psIdTipoDocumento);

        //if (!string.IsNullOrEmpty(nIdUsuario))
        //    giSql.AgregarParametro("nId_Usuario", nIdUsuario);

        //giSql.Query("usp_Cfd_Serie_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

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
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    cmd.Parameters.AddWithValue("nId_Tipo_Documento", psIdTipoDocumento);
                    cmd.Parameters.AddWithValue("sSerie", psSerie.ToUpper().Trim());
                    cmd.Parameters.AddWithValue("nFolio", psFolio);
                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar serie." + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return nResultado;
        }

        //giSql = clsComun.fnCrearConexion(conSeries);

        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        //giSql.AgregarParametro("nId_Tipo_Documento", psIdTipoDocumento);
        //giSql.AgregarParametro("sSerie", psSerie.ToUpper().Trim());
        //giSql.AgregarParametro("nFolio", psFolio);

        //return giSql.NoQuery("usp_Cfd_Serie_Ins", true);
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
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Serie_Cobro_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Serie", poIdSerie);
                    cmd.Parameters.AddWithValue("sSerie", poSerie);
                    cmd.Parameters.AddWithValue("nId_Tipo_Documento", psIdTipoDocumento);
                    cmd.Parameters.AddWithValue("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);
                    cmd.Parameters.AddWithValue("nId_estructura", pnId_estructura);
                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la serie." + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return nResultado;
        }

        //giSql = clsComun.fnCrearConexion(conSeries);

        //giSql.AgregarParametro("nId_Serie", poIdSerie);
        //giSql.AgregarParametro("sSerie", poSerie);
        //giSql.AgregarParametro("nId_Tipo_Documento", psIdTipoDocumento);
        //giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);
        //giSql.AgregarParametro("nId_estructura", pnId_estructura);

        //return giSql.NoQuery("usp_Cfd_Serie_Cobro_Del", true);
        ////return giSql.NoQuery("usp_Cfd_Serie_Del", true);
    }

    /// <summary>
    /// Actualiza número de serie
    /// </summary>
    /// <param name="poIdSerie"></param>
    /// <param name="poSerie"></param>
    /// <param name="psIdTipoDocumento"></param>
    public void fnActualizarSerie(int pnIdEstructura, int pnIdTipoDocumento, string psSerie)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSeries].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Serie_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nId_estructura", pnIdEstructura);
                    cmd.Parameters.AddWithValue("@nId_tipo_documento", pnIdTipoDocumento);
                    cmd.Parameters.AddWithValue("@sSerie", psSerie);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la serie." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        //giSql = clsComun.fnCrearConexion(conSeries);

        //////giSql.AgregarParametro("nId_Serie", poIdSerie);
        //giSql.AgregarParametro("@nId_estructura", pnIdEstructura);
        //giSql.AgregarParametro("@nId_tipo_documento", pnIdTipoDocumento);
        //giSql.AgregarParametro("@sSerie", psSerie);

        //giSql.NoQuery("usp_Cfd_Serie_Upd", true);
    }
}
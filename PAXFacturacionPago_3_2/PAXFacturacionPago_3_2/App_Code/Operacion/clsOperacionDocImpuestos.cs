using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Clase de capad e negocio para la pantalla webOperacionDocImpuestos
/// </summary>
public class clsOperacionDocImpuestos
{
    private string conDocumentos = "conConfiguracion";

    /// <summary>
    /// Retorna la lista de efectos disponibles
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de efectos disponibles</returns>
    public DataTable fnCargarEfectos()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conDocumentos].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Efectos_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                throw new Exception("Error al obtener la lista de efectos." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conDocumentos);
        //DataTable dtAuxiliar = new DataTable();

        //giSql.Query("usp_Cfd_Efectos_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de documentos disponibles
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de documentos disponibles</returns>
    public DataTable fnCargarTiposDocumento()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conDocumentos].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Documentos_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                throw new Exception("Error al obtener la lista de documentos." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conDocumentos);
        //dtAuxiliar = new DataTable();

        //giSql.Query("usp_Cfd_Documentos_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de documentos en general
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de documentos en general</returns>
    public DataTable fnCargarTiposDocumentoGen()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conDocumentos].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Documentos_General_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                throw new Exception("Error al obtener la lista de documentos general." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conDocumentos);
        //dtAuxiliar = new DataTable();

        //giSql.Query("usp_Cfd_Documentos_General_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de documentos disponibles
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de documentos disponibles</returns>
    public DataTable fnCargarTiposDocumentoPago(int p_idusuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conDocumentos].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Documentos_Sel_Pago", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nId_Usuario", p_idusuario); 
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
                throw new Exception("Error al obtener la lista de documentos disponibles." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conDocumentos);
        //dtAuxiliar = new DataTable();
        //giSql.AgregarParametro("@nId_Usuario", p_idusuario);
        //giSql.Query("usp_Cfd_Documentos_Sel_Pago", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Retorna una lista con los impuestos disponibles bajo el efecto especificado
    /// </summary>
    /// <param name="psEfecto"></param>
    /// <returns></returns>
    public DataTable fnCargarImpuestos(string psEfecto)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conDocumentos].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Impuestos_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cEfecto", psEfecto);
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
                throw new Exception("Error al obtener la lista de impuestos." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conDocumentos);
        //dtAuxiliar = new DataTable();

        //giSql.AgregarParametro("cEfecto", psEfecto);
        //giSql.Query("usp_Cfd_Impuestos_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de Documentos a los que ya se les ha asignado algún impuesto
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de Documentos a los que ya se les ha asignado algún impuesto</returns>
    public DataTable fnObtenerDocumentosAsignados()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conDocumentos].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Impuestos_Asignados_Sel", con))
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
                throw new Exception("Error al obtener la lista de documentos asignados." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conDocumentos);
        //dtAuxiliar = new DataTable();

        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.Query("usp_Cfd_Impuestos_Asignados_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Inserta o Actualiza el registro de la relación documento-impuesto
    /// </summary>
    /// <param name="psIdTipoDocumento">Identificador del documento</param>
    /// <param name="psIdImpuesto">Identificador del impuesto</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnAgregarImpuesto(string psIdTipoDocumento, string psIdImpuesto)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conDocumentos].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Impuestos_Asignados_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
                    cmd.Parameters.AddWithValue("nId_Tipo_Documento", psIdTipoDocumento);
                    cmd.Parameters.AddWithValue("nId_Impuesto", psIdImpuesto);
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la relacion documento-impuesto." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return nResultado;

        //giSql = clsComun.fnCrearConexion(conDocumentos);

        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.AgregarParametro("nId_Tipo_Documento", psIdTipoDocumento);
        //giSql.AgregarParametro("nId_Impuesto", psIdImpuesto);

        //return giSql.NoQuery("usp_Cfd_Impuestos_Asignados_Ins", true);
    }

    /// <summary>
    /// Elimina de manera lógica el registro de la relación documento-impuesto
    /// </summary>
    /// <param name="psIdTipoDocumento">Identificador del documento</param>
    /// <param name="psIdImpuesto">Identificador del impuesto</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnEliminarImpuesto(object poIdTipoDocumento, object poIdImpuesto)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conDocumentos].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Impuestos_Asignados_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
                    cmd.Parameters.AddWithValue("nId_Tipo_Documento", poIdTipoDocumento);
                    cmd.Parameters.AddWithValue("nId_Impuesto", poIdImpuesto);
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la relacion documento-impuesto." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return nResultado;

        //giSql = clsComun.fnCrearConexion(conDocumentos);

        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.AgregarParametro("nId_Tipo_Documento", poIdTipoDocumento);
        //giSql.AgregarParametro("nId_Impuesto", poIdImpuesto);

        //return giSql.NoQuery("usp_Cfd_Impuestos_Asignados_Del", true);
    }
}
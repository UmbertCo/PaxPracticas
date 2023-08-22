using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Clase de capad e negocio para la pantalla webOperacionDocImpuestos
/// </summary>
public class clsOperacionDocImpuestos
{
    private string conDocumentos = "conConfiguracion";

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
                if (clsComun.fnUsuarioEnSesion() == null)
                    throw new Exception("La sesión del usuario es nula.") { Source = "clsOperacionDocImpuestos|fnAgregarImpuesto" };

                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Impuestos_Asignados_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", clsComun.fnUsuarioEnSesion().nIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("nId_Tipo_Documento", psIdTipoDocumento));
                    cmd.Parameters.Add(new SqlParameter("nId_Impuesto", psIdImpuesto));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnAgregarImpuesto", "clsOperacionDocImpuestos");
            }
        }
        return nResultado;
    }

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

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnCargarEfectos", "clsOperacionDocImpuestos");
            }
        }
        return dtResultado;
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
                    cmd.Parameters.Add(new SqlParameter("cEfecto", psEfecto));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnCargarImpuestos", "clsOperacionDocImpuestos");
            }
        }
        return dtResultado;
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

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnCargarTiposDocumento", "clsOperacionDocImpuestos");
            }
        }
        return dtResultado;
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
                if (clsComun.fnUsuarioEnSesion() == null)
                    throw new Exception("La sesión del usuario es nula.") { Source = "clsOperacionDocImpuestos|fnEliminarImpuesto" };

                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Impuestos_Asignados_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", clsComun.fnUsuarioEnSesion().nIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("nId_Tipo_Documento", poIdTipoDocumento));
                    cmd.Parameters.Add(new SqlParameter("nId_Impuesto", poIdImpuesto));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnEliminarImpuesto", "clsOperacionDocImpuestos");
            }
        }
        return nResultado;
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
                if (clsComun.fnUsuarioEnSesion() == null)
                    throw new Exception("La sesión del usuario es nula.") { Source = "clsOperacionDocImpuestos|fnObtenerDocumentosAsignados" };

                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Impuestos_Asignados_Sel", con))
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerDocumentosAsignados", "clsOperacionDocImpuestos");
            }
        }
        return dtResultado;
    } 
}
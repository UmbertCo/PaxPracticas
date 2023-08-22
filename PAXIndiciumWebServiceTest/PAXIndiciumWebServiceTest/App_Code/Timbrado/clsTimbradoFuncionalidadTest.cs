using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections;
using System.Xml;
using System.Configuration;

/// <summary>
/// Clase encargada de la funcionalidad generica del timbrado.
/// </summary>
public class clsTimbradoFuncionalidadTest
{
    /// <summary>
    /// Obtiene el certificado por el id del rfc del contribuyente.
    /// </summary>
    /// <param name="nid_rfc">id del rfc</param>
    /// <returns>regresa el certificado</returns>
    public static DataTable ObtenerCertificado(int nid_rfc)
    {
        DataTable gdtAuxiliar = new DataTable();
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbradoTest"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Timbrado_RfcCertificado_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nid_rfc", nid_rfc);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(gdtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                gdtAuxiliar = null;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            }
            finally
            {
                con.Close();
            }
        }
        return gdtAuxiliar;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="psTipoDocumento"></param>
    /// <returns></returns>
    public static int fnBuscarTipoDocumento(string psTipoDocumento)
    {
        int nResultado = 0;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbradoTest"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Busqueda_Documento_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sTipoDocumento", psTipoDocumento);
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            }
            finally
            {
                con.Close();
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Recupera el HASH que exista en los comprobantes.
    /// </summary>
    /// <param name="nId_usuario_timbrado"></param>
    /// <param name="HASH"></param>
    /// <returns></returns>
    public static bool fnBuscarHashComprobantes(int nId_usuario_timbrado, string HASH, string tipo)
    {
        int nRetorno = 0;
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbradoTest"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Timbrado_BuscaHASH_Sel_Indicium", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_usuario_timbrado", nId_usuario_timbrado);
                    cmd.Parameters.AddWithValue("sHash", HASH);
                    cmd.Parameters.AddWithValue("sTipo", tipo);
                    con.Open();
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                    if (nRetorno > 0)
                        bRetorno = false;
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            }
            finally
            {
                con.Close();
            }
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
        string sRetorno = "";
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbradoTest"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Timbrado_BuscaHASH_XML_Sel_Indicium", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_usuario_timbrado", nId_usuario_timbrado);
                    cmd.Parameters.AddWithValue("sHash", HASH);
                    cmd.Parameters.AddWithValue("sTipo", tipo);
                    con.Open();
                    sRetorno = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            }
            finally
            {
                con.Close();
            }
        }
        return sRetorno;
    }

    /// <summary>
    /// Recupera la cantidad de creditos para el usuario registrado
    /// </summary>
    /// <param name="id_usuario"></param>
    /// <param name="descripcion"></param>
    /// <param name="estatus"></param>
    /// <param name="master"></param>
    /// <returns></returns>
    public static DataTable fnObtenerCreditos(string id_usuario, string descripcion, char estatus, char master)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbradoTest"].ConnectionString);
                scConexion.Open();
                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_Ctp_Servicios_Recupera_Sel";
                    scoComando.Parameters.AddWithValue("psId_usuario", id_usuario);
                    scoComando.Parameters.AddWithValue("psDescripcion", descripcion);
                    scoComando.Parameters.AddWithValue("sEstatus", estatus);
                    scoComando.Parameters.AddWithValue("sMaster", master);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Recupera el Id de la matriz del usuario.
    /// </summary>
    /// <param name="pnId_Usuario">ID del Usuario</param>
    /// <returns></returns>
    public static string fnRecuperaMatriz(int pnId_Usuario)
    {
        try
        {
            string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbradoTest"].ConnectionString;
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Obtener_Matriz_Sel", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 200;

                    cmd.Parameters.AddWithValue("nId_Usuario", pnId_Usuario);

                    con.Open();
                    string retVal = cmd.ExecuteScalar().ToString();
                    con.Close();

                    if (retVal == "0")
                        //return "999";
                        return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
                    else
                        return retVal;
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "Error durante el registro del comprobante", pnId_Usuario);
            //return "999"; //Error durante el registro del comprobante
            return clsComun.fnRecuperaErrorSAT("999", "Timbrado");
        }
    }
}

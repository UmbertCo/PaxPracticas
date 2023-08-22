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
public class clsTimbradoFuncionalidad
{
    /// <summary>
    /// Actualiza los creditos disponibles
    /// </summary>
    /// <param name="id_credito"></param>
    /// <param name="id_estructura"></param>
    /// <param name="creditos"></param>
    /// <returns></returns>
    public static int fnActualizarCreditos(int id_credito, int id_estructura, double creditos, string servicio)
    {
        int nRetorno = 0;
        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {
            con.Open();
            using (SqlTransaction tran = con.BeginTransaction())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Ctp_Servicios_Actualiza_Creditos_Upd", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandTimeout = 200;
                        cmd.Parameters.AddWithValue("id_credito", id_credito);
                        cmd.Parameters.AddWithValue("id_estructura", id_estructura);
                        cmd.Parameters.AddWithValue("creditos", creditos);
                        cmd.Parameters.AddWithValue("sServicio", servicio);
                        nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        return nRetorno;
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
        int nRetorno = 0;
        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {
            con.Open();
            using (SqlTransaction tran = con.BeginTransaction())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("usp_Ctp_Servicios_Actualiza_Creditos_Historico_Upd", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandTimeout = 200;
                        cmd.Parameters.AddWithValue("id_credito", id_credito);
                        cmd.Parameters.AddWithValue("id_estructura", id_estructura);
                        cmd.Parameters.AddWithValue("creditos", creditos);
                        nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        return nRetorno;
    }

    /// <summary>
    /// Función que se encarga de buscar el Acuse de Entrega de un comprobante por el UUID
    /// </summary>
    /// <param name="psUUID">UUID del Comprobante</param>
    /// <returns></returns>
    public static DataTable fnBuscarAcusePorUUID(string psUUID)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_cfd_Acuse_Indicium_SAT_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sUUID", psUUID);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Metodo que se encarga de obtener el comprobante por medio del UUID
    /// </summary>
    /// <param name="sUUID">UUID del Comprobante</param>
    /// <returns></returns>
    public static string fnBuscarComprobantePorUUID(string sUUID)
    {
        string sRetorno = "";
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_cfd_Comprobantes_PorUUID_Indicium", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nUUID", sUUID);
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
    /// Recupera el HASH que exista en los comprobantes.
    /// </summary>
    /// <param name="nId_usuario_timbrado">ID del Usuario</param>
    /// <param name="HASH">Hash del XML</param>
    /// <param name="tipo">Tipo</param>
    /// <returns></returns>
    public static bool fnBuscarHashComprobantes(int nId_usuario_timbrado, string HASH, string tipo)
    {
        int nRetorno = 0;
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString)))
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
    /// <param name="nId_usuario_timbrado">ID del Usuario</param>
    /// <param name="HASH">Hash del XML</param>
    /// <param name="tipo">Tipo</param>
    /// <returns></returns>
    public static string fnBuscarHashCompXML(int nId_usuario_timbrado, string HASH, string tipo)
    {
        string sRetorno = "";
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString)))
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
    /// Función que se encarga de buscar el tipo de documento
    /// </summary>
    /// <param name="psTipoDocumento">Tipo de Documento</param>
    /// <returns></returns>
    public static int fnBuscarTipoDocumento(string psTipoDocumento)
    {
        int nResultado = 0;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString)))
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
    /// Actualiza el comprobante especificado poniendo su estatus a 'Cancelado'
    /// </summary>
    /// <param name="psIdCfd">Identificador del Comprobante a cancelar</param>
    /// <param name="nFecha_Cancelacion">Fecha de cancelación del Comprobante</param>
    /// <param name="nUUID">UUID del Comprobante</param>
    /// <param name="nRFCEmisor">RFC del Emisor</param>
    /// <param name="nOrigen">Origen de la Cancelación</param>
    /// <param name="nidUsuario">ID del Usuario que cancela</param>
    /// <returns></returns>
    public static int fnCancelarComprobante(int psIdCfd, string nFecha_Cancelacion, string nUUID, string nRFCEmisor, string nOrigen, int nidUsuario)
    {
        //------------------------------------------------------------------------------------
        int retVal = 0;
        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString;
        using (SqlConnection conexion = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {
            //--------------------------------------------------------------------------------
            try
            {
                //----------------------------------------------------------------------------
                conexion.Open();
                using (SqlCommand command = new SqlCommand("usp_Cfd_Comprobante_Cancelacion_Indicium_Ins", conexion))
                {
                    //------------------------------------------------------------------------
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("nidCFD", psIdCfd);
                    command.Parameters.AddWithValue("nFecha_Cancelacion", nFecha_Cancelacion);
                    command.Parameters.AddWithValue("nUUID", nUUID);
                    command.Parameters.AddWithValue("nRFCEmisor", nRFCEmisor);
                    command.Parameters.AddWithValue("nOrigen", nOrigen);
                    command.Parameters.AddWithValue("nidUsuario", nidUsuario);
                    //------------------------------------------------------------------------
                    retVal = command.ExecuteNonQuery();
                    //------------------------------------------------------------------------
                }
                //----------------------------------------------------------------------------
            }
            //--------------------------------------------------------------------------------
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            //--------------------------------------------------------------------------------
            finally
            {
                conexion.Close();
            }
            //--------------------------------------------------------------------------------
            return retVal;
            //--------------------------------------------------------------------------------
        }
        //------------------------------------------------------------------------------------
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
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString);
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
            string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;
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

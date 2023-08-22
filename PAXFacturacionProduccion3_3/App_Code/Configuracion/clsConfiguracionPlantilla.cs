using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsConfiguracionPlantilla
/// </summary>
public class clsConfiguracionPlantilla
{
    private string conConfig = "conConfiguracion";

    /// <summary>
    /// Función que se encarga de actualizar la plantilla configurada del Usuario
    /// </summary>
    /// <param name="pnIdConfiguracion">ID Configuracion</param>
    /// <param name="pnIdPlantilla">ID Plantilla</param>
    /// <param name="pnIdEstructura">ID Estructura</param>
    /// <param name="psColor">Color</param>
    /// <param name="pnIdUsuario">ID Usuario</param>
    /// <returns></returns>
    public int fnActualizaPlantilla(int pnIdConfiguracion, int pnIdPlantilla, int pnIdEstructura, string psColor, int pnIdUsuario)
    {
        Int32 nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ConfiguracionPlantillas_upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (pnIdConfiguracion != 0)
                        cmd.Parameters.Add("nId_Configuracion", pnIdConfiguracion);

                    cmd.Parameters.Add(new SqlParameter("nId_Plantilla", pnIdPlantilla));
                    cmd.Parameters.Add(new SqlParameter("nId_Estructura", pnIdEstructura));
                    cmd.Parameters.Add(new SqlParameter("sColor", psColor));
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", pnIdUsuario));

                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaPlantilla", "clsConfiguracionPlantilla");
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Función que se encarga de obtener la plantilla configurada para esa estructura
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <returns></returns>
    public DataTable fnObtieneConfiguracionPlantilla(int pnId_Estructura)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Ctp_ConfiguracionPlantillas_sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nId_Estructura", pnId_Estructura);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneConfiguracionPlantilla", "clsConfiguracionPlantilla");
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que obtiene las plantillas base configuradas
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtienePlantillasBase()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RecuperaPlantillasBase_sel", con))
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtienePlantillasBase", "clsConfiguracionPlantilla");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que obtiene el ID de la Estructura de un usuario en especifico
    /// </summary>
    /// <param name="pnIdUsuario">ID Usuario</param>
    /// <returns></returns>
    public int fnRecuperaEstructura(int pnIdUsuario)
    {
        Int32 nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RecuperaEstructura_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", pnIdUsuario));

                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnRecuperaEstructura", "clsConfiguracionPlantilla");
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Función que se encarga de obtener la plantilla recursiva
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <returns></returns>
    public int fnRecuperaPlantillaRecursiva(int pnId_Estructura)
    {
        int nResultado = 0;
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_ctp_RecuperaPlantillaRecursiva_sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nIdEstructura", pnId_Estructura);

                    scConexion.Open();
                    nResultado = Convert.ToInt32(scoComando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnRecuperaPlantillaRecursiva", "clsConfiguracionPlantilla");
            }
            finally
            {
                scConexion.Close();
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Función que obtiene el nombre de la plantilla de acuerdo a su ID
    /// </summary>
    /// <param name="pnIdPlantilla">ID Plantilla</param>
    /// <returns></returns>
    //public string fnRecuperaPlantillaNombre(int pnIdPlantilla)
      public string fnRecuperaPlantillaNombre(int pnIdEstructura, int pnTipoComprobante)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_ctp_RecuperaPlantillaNombre_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdEstructura", pnIdEstructura));
                    cmd.Parameters.Add(new SqlParameter("nIdTipoComprobante", pnTipoComprobante));

                    con.Open();
                    sResultado = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnRecuperaPlantillaNombre", "clsConfiguracionPlantilla");
            }
        }
        return sResultado;
    }

    /// <summary>
    /// Función que obtiene el Documento 
    /// </summary>
    /// <returns></returns>
    public DataTable fnLlenarDropDocumento()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp33_RecuperaDocumento_sel", con))
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneDocumento", "clsConfiguracionPlantilla");
            }
        }
        return dtResultado;
    }

    
    /// <summary>
    /// Función que se encarga de obtener la plantilla configurada para esa estructura
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <returns></returns>
    public DataTable fnObtienePlantilla(int pnId_Estructura, int pnTipoDocumento)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Ctp33_ObtienePlantillas_sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nId_Estructura", pnId_Estructura);
                    scoComando.Parameters.AddWithValue("nId_Documento", pnTipoDocumento);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtienePlantilla", "clsConfiguracionPlantilla");
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que se encarga de obtener la plantilla configurada para esa estructura
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <returns></returns>
    public DataTable fnObtienePlantillaConfigurada(int pnIdEstructura, int pnIdTipoDocumento)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Ctp33_PlantillaConfigurada_sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nId_Estructura", pnIdEstructura);
                    scoComando.Parameters.AddWithValue("nId_Documento", pnIdTipoDocumento);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtienePlantillaConfigurada", "clsConfiguracionPlantilla");
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que se encarga de obtener la plantilla configurada para esa estructura
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <returns></returns>
    public DataTable fnObtienePlantillaDefault(int pnIdTipoDocumento)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Ctp33_ObtienePlantillasDefault_sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nId_Documento", pnIdTipoDocumento);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtienePlantillaDefault", "clsConfiguracionPlantilla");
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }

    public int fnRegistrarPlantillasDefault(DataTable dtPlanDefa, int pnIdEstructura)
    {

        int nRegistrosAfectados = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            con.Open();
            using (SqlTransaction tran = con.BeginTransaction())
            {
                try
                {
                    //// Se insertan las plantillas default
                    foreach (DataRow renglonDefault in dtPlanDefa.Rows)
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Ctp33_Plantilla_Config_ins", con))
                        {

                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("nIdEstructura", pnIdEstructura);
                            cmd.Parameters.AddWithValue("nIdPlantilla", Convert.ToDouble(renglonDefault["IdPlantilla"]));
                            cmd.Parameters.AddWithValue("nIdDocumento", renglonDefault["IdDocumentoPlantilla"]);

                            nRegistrosAfectados = cmd.ExecuteNonQuery();

                            if (nRegistrosAfectados.Equals(0))
                                throw new Exception("No se registraron las plantillas default.");
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnRegistrarPlantillasDefault", "clsConfiguracionPlantilla");
                }
                finally
                {
                    con.Close();
                }
            }
        }
        return nRegistrosAfectados;
    }

    /// <summary>
    /// Función que se encarga de actualizar la plantilla configurada del Usuario
    /// </summary>
    /// <param name="pnIdConfiguracion">ID Configuracion</param>
    /// <param name="pnIdPlantilla">ID Plantilla</param>
    /// <param name="pnIdEstructura">ID Estructura</param>
    /// <param name="psColor">Color</param>
    /// <param name="pnIdUsuario">ID Usuario</param>
    /// <returns></returns>
    public int fnInsertUpdatePlantilla(int pnIdConfiguracion, int pnIdPlantilla, int pnIdEstructura, int pnIdDocumento,string psColor)
    {
        Int32 nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp33_ConfiguracionPlantillas_upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (pnIdConfiguracion != 0)
                        cmd.Parameters.Add("nId_Configuracion", pnIdConfiguracion);

                    if (pnIdPlantilla != 0)
                    cmd.Parameters.Add(new SqlParameter("nId_Plantilla", pnIdPlantilla));

                    cmd.Parameters.Add(new SqlParameter("nId_Estructura", pnIdEstructura));
                    cmd.Parameters.Add(new SqlParameter("nId_Documento", pnIdDocumento));
                    cmd.Parameters.Add(new SqlParameter("sColor", psColor));
                    

                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnInsertUpdatePlantilla", "clsConfiguracionPlantilla");
            }
        }
        return nResultado;
    }
}
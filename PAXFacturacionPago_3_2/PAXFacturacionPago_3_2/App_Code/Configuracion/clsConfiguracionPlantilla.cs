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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
    public string fnRecuperaPlantillaNombre(int pnIdPlantilla)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_ctp_RecuperaPlantillaNombre_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdPlantilla", pnIdPlantilla));

                    con.Open();
                    sResultado = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return sResultado;
    }    
}
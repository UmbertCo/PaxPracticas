using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Encargada de registrar las pistas de auditoria.
/// </summary>
public class clsPistasAuditoria
{
    /// <summary>
    /// Encargada de registrar las Pistas de Auditoria
    /// </summary>
    /// <param name="pnId_Usuario">id del usuario</param>
    /// <param name="pdFecha_Accion">fecha de generacion</param>
    /// <param name="psAccion">accion del evento a capturar</param>
    /// <returns></returns>
    public static bool fnGenerarPistasAuditoria(int? pnId_Usuario, DateTime pdFecha_Accion, string psAccion)
    {
        bool bRetorno = false;
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_Ctp_Registrar_Pistas_Ins";

                    scoComando.Parameters.AddWithValue("nId_Usuario", pnId_Usuario);
                    scoComando.Parameters.AddWithValue("dFecha_Accion", pdFecha_Accion);
                    scoComando.Parameters.AddWithValue("sAccion", psAccion);

                    scoComando.ExecuteNonQuery();

                    bRetorno = true;
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
        return bRetorno;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

/// <summary>
/// Encargada de registrar las pistas de auditoria.
/// </summary>
public class clsPistasAuditoria
{
    /// <summary>
    /// Encargada de registrar las Pistas de Auditoria
    /// </summary>
    /// <param name="nId_Usuario">id del usuario</param>
    /// <param name="pdFecha_Accion">fecha de generacion</param>
    /// <param name="psAccion">accion del evento a capturar</param>
    /// <returns></returns>
    public static bool fnGenerarPistasAuditoria(int? nId_Usuario, DateTime pdFecha_Accion, string psAccion)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Registrar_Pistas_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("nId_Usuario", nId_Usuario);
                    cmd.Parameters.Add("dFecha_Accion", pdFecha_Accion);
                    cmd.Parameters.Add("sAccion", psAccion);

                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnGenerarPistasAuditoria", "clsPistasAuditoria");
            }
            finally
            {
                con.Close();
            }
        }
            return bRetorno;
        
    }
}
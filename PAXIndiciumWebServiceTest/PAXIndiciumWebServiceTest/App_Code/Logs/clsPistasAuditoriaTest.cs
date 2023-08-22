using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Encargada de registrar las pistas de auditoria.
/// </summary>
public class clsPistasAuditoriaTest
{
    /// <summary>
    /// Encargada de registrar las Pistas de Auditoria
    /// </summary>
    /// <param name="nId_Usuario">id del usuario</param>
    /// <param name="dFecha_Accion">fecha de generacion</param>
    /// <param name="sAccion">accion del evento a capturar</param>
    /// <returns></returns>
    public static bool fnGenerarPistasAuditoria(int? nId_Usuario, DateTime dFecha_Accion, string sAccion)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Registrar_Pistas_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 200;
                    cmd.Parameters.AddWithValue("nId_Usuario", nId_Usuario);
                    cmd.Parameters.AddWithValue("dFecha_Accion", dFecha_Accion);
                    cmd.Parameters.AddWithValue("sAccion", sAccion);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }
        return bRetorno;
    }
}
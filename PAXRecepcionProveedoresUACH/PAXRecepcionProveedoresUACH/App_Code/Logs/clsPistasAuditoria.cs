using System;
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
    /// <param name="dFecha_Accion">fecha de generacion</param>
    /// <param name="sAccion">accion del evento a capturar</param>
    /// <returns></returns>
    public static bool fnGenerarPistasAuditoria(int? nId_Usuario, DateTime dFecha_Accion, string sAccion)
    {
        //bool bRetorno = false;

        //try
        //{
        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("nId_Usuario", nId_Usuario);
        //    conexion.AgregarParametro("dFecha_Accion", dFecha_Accion);
        //    conexion.AgregarParametro("sAccion", sAccion);

        //    conexion.NoQuery("usp_Ctp_Registrar_Pistas_Ins", true);
        //    bRetorno = true;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}

        //return bRetorno;

        bool bRetorno = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Registrar_Pistas_Ins";
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nId_Usuario));
                    cmd.Parameters.Add(new SqlParameter("dFecha_Accion", dFecha_Accion));
                    cmd.Parameters.Add(new SqlParameter("sAccion", sAccion));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return bRetorno;
    }
}
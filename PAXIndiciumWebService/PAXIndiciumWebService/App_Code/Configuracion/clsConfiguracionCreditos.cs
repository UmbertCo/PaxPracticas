using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for clsConfiguracionCreditos
/// </summary>
public class clsConfiguracionCreditos
{
    private string conConfig = "conConfiguracion";

    public double fnRecuperaPrecioServicio(int pIdServicio)
    {
        double precio = 0;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RecuperaPrecioServicio_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdServicio", pIdServicio);
                    cmd.CommandTimeout = 200;
                    con.Open();
                    precio = Convert.ToDouble(cmd.ExecuteScalar());
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
        return precio;
    }
}
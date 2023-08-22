using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for clsOperacionDistribuidores
/// </summary>
public class clsOperacionDistribuidores
{
    private string conConfig = "conConfiguracion";

    public DataTable fnObtieneCreditosDistribuidorporUsuario(int pIdUsuario)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ObtieneCreditosdeDistribuidorporHijo_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
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
        return dtResultado;
    }

}
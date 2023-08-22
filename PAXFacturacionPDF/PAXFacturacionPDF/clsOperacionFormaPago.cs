using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for clsOperacionFormaPago
/// </summary>
public class clsOperacionFormaPago
{
    private string conFormaPago = "conConfiguracion";

    /// <summary>
    /// Retorna la lista de efectos disponibles
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de efectos disponibles</returns>
    public DataTable fnFormaPago()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conFormaPago].ConnectionString);
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd33_FormaPago_All", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                throw new Exception("Error al obtener la lista de Forma de Pago." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;
    }
}
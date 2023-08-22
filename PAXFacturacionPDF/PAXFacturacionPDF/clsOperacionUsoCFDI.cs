using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for clsOperacionUsoCFDI
/// </summary>
public class clsOperacionUsoCFDI
{
    private string conUsoCFDI = "conConfiguracion";

    /// <summary>
    /// Retorna la lista de efectos disponibles
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de efectos disponibles</returns>
    public DataTable fnCargarUsoCFDI(string pcTipo)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection())
        {
            con.ConnectionString = PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conUsoCFDI].ConnectionString);
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd33_UsoCFDI_All", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@psTipo", pcTipo);
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
                throw new Exception("Error al obtener la lista de Uso CFDI." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;
    }
}
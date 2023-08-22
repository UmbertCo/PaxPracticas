using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsOperacionTiposIncapacidades
/// </summary>
public class clsOperacionTiposIncapacidades
{
    private string conCuenta = "conControl";

	public clsOperacionTiposIncapacidades()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Función que se encarga de listar los tipos de deduciones o percepciones configurados
    /// </summary>
    /// <returns></returns>
    public DataTable fnListarTiposIncapacidades()
    {
        DataTable dtBancos = new DataTable();
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_Tipos_Incapacidades_sel";

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtBancos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los tipos de incapacidades. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtBancos;
    }
}
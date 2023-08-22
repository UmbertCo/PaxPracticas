using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsOperacionTipoRegimen
/// </summary>
public class clsOperacionTipoRegimen
{
    private string conCuenta = "conControl";

    public clsOperacionTipoRegimen()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Función que se encarga de obtener un regimen en especifico
    /// </summary>
    /// <returns></returns>
    public DataTable fnExiste(int pnClave)
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
                    scoComando.CommandText = "usp_nom_Tipos_Regimen_Sel_Existe";
                    scoComando.Parameters.AddWithValue("nClave", pnClave);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtBancos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar un regimen en espcifico los bancos. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtBancos;
    }

    /// <summary>
    /// Función que se encarga de listar los tipos regimenes configurados
    /// </summary>
    /// <returns></returns>
    public DataTable fnListarTiposRegimenes()
    {
        DataTable dtTiposRegimenes = new DataTable();
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
                    scoComando.CommandText = "usp_nom_TiposRegimen_sel";

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtTiposRegimenes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los tipos de regimenes. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtTiposRegimenes;
    }
}
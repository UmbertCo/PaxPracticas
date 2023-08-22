using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsOperacionTiposDeduccionPercepcion
/// </summary>
public class clsOperacionTiposDeduccionPercepcion
{
    private string conCuenta = "conControl";

	public clsOperacionTiposDeduccionPercepcion()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Función que se encarga de listar los tipos de deduciones o percepciones configurados
    /// </summary>
    /// <returns></returns>
    public DataTable fnListarTiposDeduccionPercepcion(int pnId_Tipo)
    {
        DataTable dtTiposDeduccionPercepcion = new DataTable();
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
                    scoComando.CommandText = "usp_nom_Tipos_Deduccion_Percepcion_sel";

                    scoComando.Parameters.AddWithValue("nId_Tipo", pnId_Tipo);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtTiposDeduccionPercepcion);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los tipos de deducciones y percepciones. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtTiposDeduccionPercepcion;
    }

    /// <summary>
    /// Función que se encarga de listar los tipos de conceptos de nómina
    /// </summary>
    /// <returns></returns>
    public DataTable fnListarTipos()
    {
        DataTable dtTipos = new DataTable();
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
                    scoComando.CommandText = "usp_nom_Tipos_sel";

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtTipos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los tipos de deducciones y percepciones. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtTipos;
    }
}
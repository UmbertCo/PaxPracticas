using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsBanco
/// </summary>
public class clsOperacionBanco
{
    private string conCuenta = "conControl";

    public clsOperacionBanco()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Función que se encarga de obtener un banco en especifico
    /// </summary>
    /// <returns></returns>
    public DataTable fnExisteBancos(int pnClave)
    {
        DataTable dtBancos = new DataTable();
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_Bancos_Sel_Existe";
                    scoComando.Parameters.AddWithValue("nClave", pnClave);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtBancos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar un banco en especifico. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtBancos;
    }

    /// <summary>
    /// Función que se encarga de listar los bancos configurados
    /// </summary>
    /// <returns></returns>
    public DataTable fnListarBancos()
    {
        DataTable dtBancos = new DataTable();
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_Bancos_Sel";

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtBancos);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los bancos. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtBancos;
    }  
}
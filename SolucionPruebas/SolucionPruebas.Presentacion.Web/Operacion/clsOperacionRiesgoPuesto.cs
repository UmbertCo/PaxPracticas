using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsOperacionRiesgoPuesto
/// </summary>
public class clsOperacionRiesgoPuesto
{
    private string conCuenta = "conControl";

    public clsOperacionRiesgoPuesto()
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
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_Riesgo_Puesto_Sel_Existe";
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
    /// Función que se encarga de listar los tipos riesgos de puestos configurados
    /// </summary>
    /// <returns></returns>
    public DataTable fnListarTiposRiesgosPuesto()
    {
        DataTable dtTiposRiesgosPuesto = new DataTable();
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
                    scoComando.CommandText = "usp_nom_Tipos_Riesgo_Puesto_Sel";

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtTiposRiesgosPuesto);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los tipos de riesgos de puesto. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtTiposRiesgosPuesto;
    }
}
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
	///	 Obtiene el catalogo de Regimenes Fiscales
	/// </summary>
	/// <!--Carlos Sanchez-->
	/// <!--14/11/2016-->
	/// <returns></returns>
	public DataTable fnObtenerTiposRegimenFiscal()
	{
		DataTable dtResultado = new DataTable();
		try
		{
			using (SqlConnection con = new SqlConnection())
			{
                con.ConnectionString = PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);

				using (SqlCommand cmd = con.CreateCommand())
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = "usp_nom_Tipos_RegimenFiscal_sel_n12";

					using (SqlDataAdapter da = new SqlDataAdapter(cmd))
					{
						da.Fill(dtResultado);
					}
				}
			}
		}
		catch (Exception ex)
		{
		}
		return dtResultado;
	}
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Clase con la capa de negocios para la pantalla webOperacionClientes
/// </summary>
public class clsOperacionClientes
{
    private string conClientes = "conConfiguracion";

	#region RFC

    /// <summary>
    /// Inserta o actualiza a un receptor en la base de datos.
    /// Si el parámetro psIdRfc es cadena vacía entonces es inserción de lo contrario es actualización
    /// </summary>
    /// <param name="psIdRfc">Identificador del receptor</param>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <param name="psRfc">RFC del receptor</param>
    /// <param name="psRazonSocial">razón social del receptor</param>
    /// <returns></returns>
    public int fnGuardarReceptor(string psIdRfc, string psIdEstructura, string psRfc, string psRazonSocial)
	{
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(psIdRfc))
                        cmd.Parameters.Add(new SqlParameter("nId_Receptor", psIdRfc));
                    cmd.Parameters.Add(new SqlParameter("nId_Estructura", psIdEstructura));
                    cmd.Parameters.Add(new SqlParameter("sRfc", psRfc));
                    cmd.Parameters.Add(new SqlParameter("sRazon_Social", psRazonSocial));

                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnGuardarReceptor", "clsOperacionClientes");
            }
        }
        return nResultado;
	}

    /// <summary>
    /// Busca y retorna el conjunto de datos de un receptor en especifico
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns>Retorna un DataTable con los datos del receptor</returns>
    public DataTable fnEditarReceptor(string psIdReceptor)
	{
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Receptor", psIdReceptor));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnEditarReceptor", "clsOperacionClientes");
            }
        }
        return dtResultado;
	}

    /// <summary>
    /// Elimina de manera lógica a un receptor
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns></returns>
	public int fnEliminarReceptor(string psIdReceptor)
	{
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Receptor", psIdReceptor));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnEliminarReceptor", "clsOperacionClientes");
            }
        }
        return nResultado;
	}

	/// <summary>
	/// Trae la lista de receptores activos del usuario
    /// La relación es usuario-estructura-receptor
	/// </summary>
    /// /// <returns>DataTable con la lista de todos los receptores</returns>
	public DataTable fnLlenarReceptores()
	{
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                if (clsComun.fnUsuarioEnSesion() == null)
                    throw new Exception("La sesión del usuario es nula.") { Source = "clsOperacionClientes|fnLlenarReceptores" };

                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptores_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nnIdUsuario", clsComun.fnUsuarioEnSesion().nIdUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarReceptores", "clsOperacionClientes");
            }
        }
        return dtResultado;
	}

    /// <summary>
    /// Trae la lista de receptores activos del usuario
    /// La relación es usuario-estructura-receptor
    /// </summary>
    /// <param name="psIdEstructura">Estructura para la cual se quieren obtener los receptores</param>
    /// <returns>DataTable con la lista de receptores adecuados</returns>
    public DataTable fnLlenarReceptores(string psIdEstructura)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptores_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Estructura", psIdEstructura));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarReceptores", "clsOperacionClientes");
            }
        }
        return dtResultado;
    }

	#endregion

	#region Sucursal

    /// <summary>
    /// Inserta o actualiza un sucursal de receptor en la base de datos.
    /// Si el parámetro psIdEstructura es cadena vacía entonces es inserción de lo
    /// contrario es actualización.
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <param name="psSucursal">Nombre de la sucursal</param>
    /// <param name="pbCalle">Nombre de la calle</param>
    /// <param name="pbNoExterior">El número exterior del lugar</param>
    /// <param name="pbNoInterior">El número interior del lugar</param>
    /// <param name="pbColonia">El nombre de la colonia</param>
    /// <param name="pbCodigoPostal">El numero de código postal del área</param>
    /// <param name="pbLocalidad">El nombre de la localidad</param>
    /// <param name="pbMunicipio">El nombre del municipio</param>
    /// <param name="pbEstado">El nombre del estado</param>
    /// <param name="pbPais">El nombre del país</param>
    /// <returns>Retorna un entero indicando las filas afectadas en la consulta</returns>
    public int fnGuardarSucursal(string psIdEstructura, string psIdReceptor, string psSucursal, byte[] pbCalle, byte[] pbNoExterior, byte[] pbNoInterior,
                                    byte[] pbColonia, byte[] pbCodigoPostal, byte[] pbLocalidad, byte[] pbMunicipio, byte[] pbEstado, byte[] pbPais)
	{
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_Sucursal", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(psIdEstructura))
                        cmd.Parameters.Add(new SqlParameter("nId_Estructura", psIdEstructura));
                    cmd.Parameters.Add(new SqlParameter("nId_Receptor", psIdReceptor));
                    cmd.Parameters.Add(new SqlParameter("sSucursal", psSucursal));
                    cmd.Parameters.Add(new SqlParameter("sCalle", pbCalle));
                    if (pbNoExterior != null)
                        cmd.Parameters.Add(new SqlParameter("sNumero_Exterior", pbNoExterior));
                    if (pbNoInterior != null)
                        cmd.Parameters.Add(new SqlParameter("sNumero_Interior", pbNoInterior));
                    if (pbColonia!= null)
                        cmd.Parameters.Add(new SqlParameter("sColonia", pbColonia));
                    if (pbCodigoPostal != null)
                        cmd.Parameters.Add(new SqlParameter("sCodigo_Postal",pbCodigoPostal));
                    if (pbLocalidad != null)
                        cmd.Parameters.Add(new SqlParameter("sLocalidad", pbLocalidad));
                    cmd.Parameters.Add(new SqlParameter("sMunicipio", pbMunicipio));
                    cmd.Parameters.Add(new SqlParameter("sEstado", pbEstado));
                    cmd.Parameters.Add(new SqlParameter("sPais", pbPais));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnGuardarSucursal", "clsOperacionClientes");
            }
        }
        return nResultado;
	}

    /// <summary>
    /// Elimina de manera lógica una sucursal de receptor
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <returns>Retorna un entero indicando las filas afectadas por la consulta</returns>
    public int fnEliminarSucursalReceptor(string psIdEstructura)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_Sucursal_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Estructura", psIdEstructura));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnEliminarSucursalReceptor", "clsOperacionClientes");
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Retorna una lista de sucursales pertenecientes al receptor especificado
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns>Retorna un DataTable con las sucursales encontradas</returns>
    public DataTable fnLlenarGridSucursalesReceptores(string psIdReceptor)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_Sucursales_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Receptor", psIdReceptor));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarGridSucursalesReceptores", "clsOperacionClientes");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Verifica que el receptor pertenesca al usuario en sesión
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns>Booleano indicando si el receptor pertenece al usuario</returns>
    public bool fnVerificarPropiedad(string psIdReceptor)
    {
        bool bResultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                if (clsComun.fnUsuarioEnSesion() == null)
                    throw new Exception("La sesión del usuario es nula.") { Source = "clsOperacionClientes|fnVerificarPropiedad" };

                using (SqlCommand cmd = new SqlCommand("usp_Cli_Verificar_Receptor_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("nnIdUsuario", clsComun.fnUsuarioEnSesion().nIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("nId_Receptor", psIdReceptor));

                    con.Open();
                    bResultado = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnVerificarPropiedad", "clsOperacionClientes");
            }
        }
        return bResultado;
    }

	#endregion
}
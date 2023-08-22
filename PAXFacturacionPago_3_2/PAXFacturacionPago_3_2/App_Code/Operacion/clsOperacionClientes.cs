using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

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
        int resultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor", con))
                {
                    if (!string.IsNullOrEmpty(psIdRfc))
                        cmd.Parameters.AddWithValue("nId_Receptor", psIdRfc);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    cmd.Parameters.AddWithValue("sRfc", psRfc.Trim());
                    cmd.Parameters.AddWithValue("sRazon_Social", psRazonSocial);
                    con.Open();
                    resultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el receptor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //giSql = clsComun.fnCrearConexion(conClientes);

        //if (!string.IsNullOrEmpty(psIdRfc))
        //    giSql.AgregarParametro("nId_Receptor", psIdRfc);
        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        //giSql.AgregarParametro("sRfc", psRfc.Trim());
        //giSql.AgregarParametro("sRazon_Social", psRazonSocial);

        //return giSql.NoQuery("usp_Cli_Receptor", true);
	}

    /// <summary>
    /// Busca y retorna el conjunto de datos de un receptor en especifico
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns>Retorna un SqlDataReader con los datos del receptor</returns>
	public DataTable fnEditarReceptor(string psIdReceptor)
	{
        DataTable dtResultado = new DataTable();
        
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_Sel_Cobro", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Receptor", psIdReceptor);
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
                throw new Exception("Error al editar información del receptor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conClientes);

        //giSql.AgregarParametro("nId_Receptor", psIdReceptor);
        //return giSql.Query("usp_Cli_Receptor_Sel_Cobro", true);
	}

    /// <summary>
    /// Elimina de manera lógica a un receptor
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns></returns>
	public int fnEliminarReceptor(string psIdReceptor)
	{
        int resultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Receptor", psIdReceptor);
                    con.Open();
                    resultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar receptor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //giSql = clsComun.fnCrearConexion(conClientes);

        //giSql.AgregarParametro("nId_Receptor", psIdReceptor);
        //return giSql.NoQuery("usp_Cli_Receptor_Del", true);
	}

	/// <summary>
	/// Trae la lista de receptores activos del usuario
    /// La relación es usuario-estructura-receptor
	/// </summary>
    /// /// <returns>DataTable con la lista de todos los receptores</returns>
	public DataTable fnLlenarReceptores(int psIdEstructura)
	{
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptores_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
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
                throw new Exception("Error al recuperar la lista de receptores del usuario." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conClientes);
        //dtAuxiliar = new DataTable();

        ////giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        //giSql.Query("usp_Cli_Receptores_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
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
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
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
                throw new Exception("Error al recuperar la lista de receptores activos del usuario." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conClientes);
        //dtAuxiliar = new DataTable();

        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        //giSql.Query("usp_Cli_Receptores_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }


    /// <summary>
    /// Inserta o actualiza a un receptor en la base de datos.
    /// Si el parámetro psIdRfc es cadena vacía entonces es inserción de lo contrario es actualización
    /// </summary>
    /// <param name="psIdRfc">Identificador del receptor</param>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <param name="psRfc">RFC del receptor</param>
    /// <param name="psRazonSocial">razón social del receptor</param>
    /// <returns></returns>
    public int fnGuardarReceptorCobro(string psIdRfc, string psIdEstructura, string psRfc, string psRazonSocial,byte[] sCorreo)
    {
        int resultado;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_cobro", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(psIdRfc))
                        cmd.Parameters.AddWithValue("nId_Receptor", psIdRfc);
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    cmd.Parameters.AddWithValue("sRfc", psRfc.Trim());
                    cmd.Parameters.AddWithValue("sRazon_Social", psRazonSocial);
                    cmd.Parameters.AddWithValue("sCorreo", sCorreo);
                    con.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar receptor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;


        //giSql = clsComun.fnCrearConexion(conClientes);

        //if (!string.IsNullOrEmpty(psIdRfc))
        //giSql.AgregarParametro("nId_Receptor", psIdRfc);
        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        //giSql.AgregarParametro("sRfc", psRfc.Trim());
        //giSql.AgregarParametro("sRazon_Social", psRazonSocial);
        //giSql.AgregarParametro("sCorreo", sCorreo);

        //return Convert.ToInt32(giSql.TraerEscalar("usp_Cli_Receptor_cobro", true));
    }

	#endregion

	#region Sucursal

    /// <summary>
    /// Verifica que el receptor pertenesca al usuario en sesión
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns>Booleano indicando si el receptor pertenece al usuario</returns>
    public bool fnVerificarPropiedad(string psIdReceptor)
    {
        bool resultado = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Verificar_Receptor_Cobro_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
                    cmd.Parameters.AddWithValue("nId_Receptor", psIdReceptor);
                   
                    con.Open();
                    resultado = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar que el receptor pertenece al usuario." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //giSql = clsComun.fnCrearConexion("conConfiguracion");

        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.AgregarParametro("nId_Receptor", psIdReceptor);
        //return Convert.ToBoolean(giSql.TraerEscalar("usp_Cli_Verificar_Receptor_Cobro_Sel", true));
    }

    /// <summary>
    /// Inserta o actualiza un sucursal de receptor en la base de datos.
    /// Si el parámetro psIdEstructura es cadena vacía entonces es inserción de lo
    /// contrario es actualización.
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <param name="psSucursal">Nombre de la sucursal</param>
    /// <param name="psCalle">Nombre de la calle</param>
    /// <param name="psNoExterior">El número exterior del lugar</param>
    /// <param name="psNoInterior">El número interior del lugar</param>
    /// <param name="psColonia">El nombre de la colonia</param>
    /// <param name="psCodigoPostal">El numero de código postal del área</param>
    /// <param name="psLocalidad">El nombre de la localidad</param>
    /// <param name="psMunicipio">El nombre del municipio</param>
    /// <param name="psEstado">El nombre del estado</param>
    /// <param name="psPais">El nombre dle país</param>
    /// <returns>Retorna un entero indicando las filas afectadas en la consulta</returns>
	public int fnGuardarSucursal(string psIdEstructura, string psIdReceptor, string psSucursal,
                                    byte[] psCalle, byte[] psNoExterior, byte[] psNoInterior,
                                     byte[] psColonia, byte[] psCodigoPostal, byte[] psLocalidad,
                                     byte[] psMunicipio, byte[] psEstado, byte[] psPais)
	{

        int resultado;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_Sucursal", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(psIdEstructura))
                        cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    cmd.Parameters.AddWithValue("@nId_Receptor", psIdReceptor);
                    cmd.Parameters.AddWithValue("@sSucursal", psSucursal);
                    cmd.Parameters.AddWithValue("@sCalle", psCalle);
                    if (psNoExterior != null && psNoExterior.Length > 0)
                   // if (!string.IsNullOrEmpty(psNoExterior))
                        cmd.Parameters.AddWithValue("@sNumero_Exterior", psNoExterior);
                   // if (!string.IsNullOrEmpty(psNoInterior))
                    if (psNoInterior != null && psNoInterior.Length > 0)
                        cmd.Parameters.AddWithValue("@sNumero_Interior", psNoInterior);
                   // if (!string.IsNullOrEmpty(psColonia))
                    if (psColonia != null && psColonia.Length > 0)
                        cmd.Parameters.AddWithValue("@sColonia", psColonia);
                   // if(!string.IsNullOrEmpty(psCodigoPostal))
                    if (psCodigoPostal != null && psCodigoPostal.Length > 0)
                        cmd.Parameters.AddWithValue("@sCodigo_Postal", psCodigoPostal);                    
                    // if (!string.IsNullOrEmpty(psLocalidad))
                    if (psLocalidad != null && psLocalidad.Length > 0)
                        cmd.Parameters.AddWithValue("@sLocalidad", psLocalidad);
                    cmd.Parameters.AddWithValue("@sMunicipio", psMunicipio);
                    cmd.Parameters.AddWithValue("@sEstado", psEstado);
                    cmd.Parameters.AddWithValue("@sPais", psPais);
                    con.Open();
                    resultado = cmd.ExecuteNonQuery();
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar sucursal." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;


        //giSql = clsComun.fnCrearConexion(conClientes);

        //if (!string.IsNullOrEmpty(psIdEstructura))
        //    giSql.AgregarParametro("nId_Estructura", psIdEstructura);

        //giSql.AgregarParametro("@nId_Receptor", psIdReceptor);
        //giSql.AgregarParametro("@sSucursal", psSucursal);
        //giSql.AgregarParametro("@sCalle", psCalle);
        //if (!string.IsNullOrEmpty(psNoExterior))
        //    giSql.AgregarParametro("@sNumero_Exterior", psNoExterior);
        //if (!string.IsNullOrEmpty(psNoInterior))
        //    giSql.AgregarParametro("@sNumero_Interior", psNoInterior);
        //if (!string.IsNullOrEmpty(psColonia))
        //    giSql.AgregarParametro("@sColonia", psColonia);
        //if(!string.IsNullOrEmpty(psCodigoPostal))
        //    giSql.AgregarParametro("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
        //if (!string.IsNullOrEmpty(psLocalidad))
        //    giSql.AgregarParametro("@sLocalidad", psLocalidad);
        //giSql.AgregarParametro("@sMunicipio", psMunicipio);
        //giSql.AgregarParametro("@sEstado", psEstado);
        //giSql.AgregarParametro("@sPais", psPais);

        //return giSql.NoQuery("usp_Cli_Receptor_Sucursal", true);
	}

    /// <summary>
    /// Elimina de manera lógica una sucursal de receptor
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <returns>Retorna un entero indicando las filas afectadas por la consulta</returns>
    public int fnEliminarSucursalReceptor(string psIdEstructura)
    {
        int resultado;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cli_Receptor_Sucursal_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    con.Open();
                    resultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al borrar sucursal." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //giSql = clsComun.fnCrearConexion(conClientes);
        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);

        //return giSql.NoQuery("usp_Cli_Receptor_Sucursal_Del", true);
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
                    cmd.Parameters.AddWithValue("@nId_Receptor", psIdReceptor);
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
                throw new Exception("Error al recuperar la lista de sucursales del receptor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;


        //giSql = clsComun.fnCrearConexion(conClientes);
        //dtAuxiliar = new DataTable();

        //giSql.AgregarParametro("@nId_Receptor", psIdReceptor);

        //giSql.Query("usp_Cli_Receptor_Sucursales_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Obtiene lista de correos del cliente
    /// </summary>
    /// <param name="nidEstructura">identificador de estructura</param>
    /// <param name="sRfc">identificador de RFC</param>
    /// <param name="sRazonSocial">identificador de Raxon Social</param>
    /// <returns></returns>
    public DataTable fnObtenerCorreoCliente(int? nidRfc_Receptor, int? nidEstructura, string sRfc, string sRazon_Social)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_ObtieneEmailReceptor_Sel_Cobro", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdRfc_Receptor", nidRfc_Receptor);
                    cmd.Parameters.AddWithValue("@nidEstructura", nidEstructura);
                    cmd.Parameters.AddWithValue("@sRfc", sRfc);
                    cmd.Parameters.AddWithValue("@sRazon_Social", sRazon_Social);
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
                throw new Exception("Error al recuperar la lista de correos del cliente." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conClientes);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nIdRfc_Receptor", nidRfc_Receptor);
        //    giSql.AgregarParametro("@nidEstructura", nidEstructura);
        //    giSql.AgregarParametro("@sRfc", sRfc);
        //    giSql.AgregarParametro("@sRazon_Social", sRazon_Social);
        //    giSql.Query("usp_Cfd_ObtieneEmailReceptor_Sel_Cobro", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return null;
        //}
    }

	#endregion

}
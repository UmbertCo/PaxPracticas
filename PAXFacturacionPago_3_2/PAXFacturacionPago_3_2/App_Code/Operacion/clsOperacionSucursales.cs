using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Configuration;

/// <summary>
/// Clase de capa de negocio para la pantalla webOperacionSucursales
/// </summary>
public class clsOperacionSucursales
{
    private string conSucursales = "conConfiguracion";

    /// <summary>
    /// Inserta o actualiza un sucursal de receptor en la base de datos.
    /// Si el parámetro psIdEstructura es cadena vacía entonces es inserción de lo
    /// contrario es actualización.
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <param name="psSucursal">Nombre de la sucursal</param>
    /// <param name="psCalle">Nombre de la calle</param>
    /// <param name="psNoExterior">El número exterior del lugar</param>
    /// <param name="psNoInterior">El número interior del lugar</param>
    /// <param name="psColonia">El nombre de la colonia</param>
    /// <param name="psCodigoPostal">El numero de código postal del área</param>
    /// <param name="psLocalidad">El nombre de la localidad</param>
    /// <param name="psMunicipio">El nombre del municipio</param>
    /// <param name="psIdEstado">El Identificador del estado</param>
    /// <returns>Retorna un entero indicando las filas afectadas en la consulta</returns>
    public int fnGuardarSucursal(string psIdEstructura, string psSucursal,
                                    byte[] psCalle, byte[] psNoExterior, byte[] psNoInterior,
                                    byte[] psColonia, string psReferencia, byte[] psCodigoPostal, byte[] psLocalidad,
                                    byte[] psMunicipio, string psIdEstado, string nId_padre)
    {

        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursales].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Estructrua", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(psIdEstructura))
                        cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    cmd.Parameters.AddWithValue("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
                    cmd.Parameters.AddWithValue("@sSucursal", psSucursal);
                    cmd.Parameters.AddWithValue("@sCalle", psCalle);
                    
                    if(psNoExterior != null && psNoExterior.Length > 0)
                    //if (!string.IsNullOrEmpty(psNoExterior))
                        cmd.Parameters.AddWithValue("@sNumero_Exterior", psNoExterior);
                    //if (!string.IsNullOrEmpty(psNoInterior))
                    if (psNoInterior != null && psNoInterior.Length > 0)
                        cmd.Parameters.AddWithValue("@sNumero_Interior", psNoInterior);
                    //if (!string.IsNullOrEmpty(psColonia))
                    if (psColonia != null && psColonia.Length > 0)
                        cmd.Parameters.AddWithValue("@sColonia", psColonia);
                    if (!string.IsNullOrEmpty(psReferencia))
                        cmd.Parameters.AddWithValue("@sReferencia", psReferencia);
                    if (psCodigoPostal != null && psCodigoPostal.Length > 0)
                        cmd.Parameters.AddWithValue("@sCodigo_Postal", psCodigoPostal);
                    //cmd.Parameters.AddWithValue("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
                    //if (!string.IsNullOrEmpty(psLocalidad))
                    if (psLocalidad != null && psLocalidad.Length > 0)
                        cmd.Parameters.AddWithValue("@sLocalidad", psLocalidad);
                    cmd.Parameters.AddWithValue("@sMunicipio", psMunicipio);
                    cmd.Parameters.AddWithValue("@nId_Estado", psIdEstado);
                    cmd.Parameters.AddWithValue("@nId_padre", nId_padre);
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
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
        return nResultado;

        //giSql = clsComun.fnCrearConexion(conSucursales);

        //if (!string.IsNullOrEmpty(psIdEstructura))
        //    giSql.AgregarParametro("nId_Estructura", psIdEstructura);

        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.AgregarParametro("@sSucursal", psSucursal);
        //giSql.AgregarParametro("@sCalle", psCalle);
        //if (!string.IsNullOrEmpty(psNoExterior))
        //    giSql.AgregarParametro("@sNumero_Exterior", psNoExterior);
        //if (!string.IsNullOrEmpty(psNoInterior))
        //    giSql.AgregarParametro("@sNumero_Interior", psNoInterior);
        //if (!string.IsNullOrEmpty(psColonia))
        //    giSql.AgregarParametro("@sColonia", psColonia);
        //if (!string.IsNullOrEmpty(psReferencia))
        //    giSql.AgregarParametro("@sReferencia", psReferencia);
        //giSql.AgregarParametro("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
        //if (!string.IsNullOrEmpty(psLocalidad))
        //    giSql.AgregarParametro("@sLocalidad", psLocalidad);
        //giSql.AgregarParametro("@sMunicipio", psMunicipio);
        //giSql.AgregarParametro("@nId_Estado", psIdEstado);
        //giSql.AgregarParametro("@nId_padre", nId_padre);

        //return (int)giSql.TraerEscalar("usp_Con_Estructrua", true);//giSql.NoQuery("usp_Con_Estructrua", true);

        
    }

    /// <summary>
    /// Elimina de manera lógica una sucursal
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <returns>Retorna un entero indicando las filas afectadas en la consulta</returns>
    public int fnEliminarSucursal(string psIdEstructura)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursales].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Sucursal_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar sucursal." + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return nResultado;
        }
        

        //giSql = clsComun.fnCrearConexion(conSucursales);
        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);

        //return giSql.NoQuery("usp_Con_Sucursal_Del", true);
    }

    /// <summary>
    /// Retorna la lista de sucursales previamente borradas por el usuario
    /// </summary>
    /// <returns>DataTable con la lista de sucursales borradas</returns>
    public DataTable fnObtenerSucursalesBorradas()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursales].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Sucursal_Borrados_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
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
                throw new Exception("Error al obtener la lista de sucursales borradas por el usuario." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conSucursales);
        //dtAuxiliar = new DataTable();

        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.Query("usp_Con_Sucursal_Borrados_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Cambia el estatus de una sucursal previamente borrada a Activo
    /// </summary>
    /// <param name="psIdEstructura"></param>
    /// <returns></returns>
    public int fnActualizarSucursalBorrada(string psIdEstructura)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursales].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Sucursal_Borrados_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar el estatus de la sucursal." + ex.Message);
            }
            finally
            {
                con.Close();
            }

            return nResultado;
        }

        //giSql = clsComun.fnCrearConexion(conSucursales);

        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        //return giSql.NoQuery("usp_Con_Sucursal_Borrados_Upd", true);
    }

    /// <summary>
    /// Obtienen la lista que compone la estructura del sistema.
    /// </summary>
    /// <param name="psIdPadre"></param>
    /// <returns></returns>
    public DataTable fnObtenerEstructura(string psIdPadre)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursales].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Recupera_Estructura_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Padre", psIdPadre);
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
                throw new Exception("Error al obtener la lista de la estructura." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conSucursales);
        //dtAuxiliar = new DataTable();

        //giSql.AgregarParametro("nId_Padre", psIdPadre);
        //giSql.Query("usp_Con_Recupera_Estructura_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
    }

    /// <summary>
    /// Regresa la cantidad de facturas generadas con esa estructura.
    /// </summary>
    /// <param name="psIdEstructura"></param>
    /// <returns></returns>
    public int fnBuscarGenerados(string psIdEstructura)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursales].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Busca_Comprobantes_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
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
        return nResultado;

        //giSql = clsComun.fnCrearConexion(conSucursales);

        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        //return (int)giSql.TraerEscalar("usp_Con_Busca_Comprobantes_Sel", true);

    }

    /// <summary>
    /// Obtiene domicilio de sucursal
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerDomicilioSuc(int pnSucursal)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conSucursales].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Domicilio_Suc_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NIDSUCURSAL", pnSucursal);
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
                throw new Exception("Error al obtener el domicilio de la sucursal." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conSucursales);

        //giSql.AgregarParametro("@NIDSUCURSAL", pnSucursal);
        //return giSql.Query("usp_Con_Domicilio_Suc_Sel", true);
    }
}
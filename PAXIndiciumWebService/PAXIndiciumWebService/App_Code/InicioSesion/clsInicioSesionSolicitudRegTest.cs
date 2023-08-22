using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Clase encargada de preparar el contribuyente para el registro.
/// </summary>
public class clsInicioSesionSolicitudRegTest
{
    private string conInicioSesion = "conInicioSesionTest";

    /// <summary>
    /// Encargado de generar el registro de la solicitud del contribuyente.
    /// </summary>
    /// <param name="sNombre">Nombre del contribuyente</param>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="correo">Correo del contribuyente</param>
    /// <param name="sPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool solicitudRegistroContribuyente(string sNombre, string sUsuario, string correo, string sPassword, char sOrigen)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_Registro_In", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sNombre", sNombre);
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    cmd.Parameters.AddWithValue("sEmail", correo);
                    cmd.Parameters.AddWithValue("sPassword", sPassword);
                    cmd.Parameters.AddWithValue("sOrigen", sOrigen);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return bRetorno;

    }

    /// <summary>
    /// Encargado de traer la contraseña del contribuyente.
    /// </summary>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="email">Correo del contribuyente</param>
    /// <returns>Regresa un numero cuando encuentra la clave existente</returns>
    public int buscarClaveExistente(string sUsuario, string email)
    {
        int nRetorno = 0;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_BuscarUsu_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    cmd.Parameters.AddWithValue("sEmail", email);
                    con.Open();
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return nRetorno;
    }

    /// <summary>
    /// Encargado de ir a buscar la existencia del usuario.
    /// </summary>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="sPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa los datos del usuario</returns>
    public DataTable buscarUsuario(string sUsuario)
    {
        DataTable tabla = new DataTable();

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RecuperaUsu_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(tabla);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return tabla;
    }

    /// <summary>
    /// Encargado de ir a buscar la existencia del usuario.
    /// </summary>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="sPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa los datos del usuario</returns>
    public DataTable buscarUsuarioRFC(string sUsuario)
    {

        DataTable tabla = new DataTable();

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RecuperaUsuRFC_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(tabla);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return tabla;
    }

    /// <summary>
    /// Encargado de recuperar la lista de modulos del usuario.
    /// </summary>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <returns>Regresa los datos del usuario</returns>
    public DataTable fnRecuperaModulosUsuario(string sUsuario)
    {

        DataTable tabla = new DataTable();

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RecuperaModulos_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(tabla);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return tabla;
    }

    /// <summary>
    /// Encargado de actualizar la contraseña a petición del contribuyente.
    /// </summary>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="correo">Correo del contribuyente</param>
    /// <param name="sPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaContraseña(string sUsuario, string correo, string sPassword)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RecuperaUsu_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    cmd.Parameters.AddWithValue("sEmail", correo);
                    cmd.Parameters.AddWithValue("sEmail", correo);
                    cmd.Parameters.AddWithValue("sPassword", sPassword);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return bRetorno;
    }

    /// <summary>
    /// Actualiza el estado actual del usuario.
    /// </summary>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="correo">Correo del contribuyente</param>
    /// <param name="estadoActual">Estado actual del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaEstadoActual(string sUsuario, string correo, char estadoActual)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizaEstado_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    cmd.Parameters.AddWithValue("sEmail", correo);
                    cmd.Parameters.AddWithValue("sEstadoActual", estadoActual);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return bRetorno;
    }

    /// <summary>
    /// Encargado de actualizar la fecha de ingreso al sistema.
    /// </summary>
    /// <param name="id_usuario">id del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaFechaIngreso(int id_usuario, string sUsuario, string correo)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizaFechaIngreso_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idUsu", id_usuario);
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    cmd.Parameters.AddWithValue("sEmail", correo);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return bRetorno;
    }

    /// <summary>
    /// Actualiza la fecha de cambio para la expiracion de contraseña.
    /// </summary>
    /// <param name="id_usuario">id del usuario del contribuyente</param>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="correo">Correo del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaFechaCambio(int id_usuario, string sUsuario, string correo)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizaFechaCambio_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idUsu", id_usuario);
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    cmd.Parameters.AddWithValue("sEmail", correo);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return bRetorno;
    }

    /// <summary>
    /// Encargado de Actualizar las contraseñas para la caducidad.
    /// </summary>
    /// <param name="idUsuario">id del usuario del contribuyente</param>
    /// <param name="tipo">tipo del usuario del contribuyente</param>
    /// <param name="sPassword">contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaContraseñaCaducidad(int idUsuario, char tipo, string sPassword)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizaFechaCambio_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nIdUsu", idUsuario);
                    cmd.Parameters.AddWithValue("cTipo", tipo);
                    cmd.Parameters.AddWithValue("sPassword", sPassword);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return bRetorno;
    }


    /// <summary>
    /// Revisa la inactividad del usuario durante 30 dias.
    /// </summary>
    /// <param name="idUsuario">id del usuario del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool RevisaInactividad(int idUsuario)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RevisarInactividad_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nIdUsu", idUsuario);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return bRetorno;
    }

    /// <summary>
    /// Revisa Expiracion de la contraseña del contribuyente.
    /// </summary>
    /// <param name="idUsuario">id del usuario del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool RevisaExpiracion(int idUsuario)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RevisarExpiracionPass_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nIdUsu", idUsuario);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return bRetorno;
    }

    /// <summary>
    /// Revisa Pass Repetido Revisa 
    /// </summary>
    /// <param name="idUsuario">id del usuario del contribuyente</param>
    /// <param name="password">contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool RevisaPassRepetido(int idUsuario, string password)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizPassRepetido_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("id_usuario", idUsuario);
                    cmd.Parameters.AddWithValue("sPassword", password);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return bRetorno;
    }


}
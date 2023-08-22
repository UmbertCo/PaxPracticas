using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Clase encargada de preparar el contribuyente para el registro.
/// </summary>
public class clsInicioSesionSolicitudReg
{
    /// <summary>
    /// Encargado de generar el registro de la solicitud del contribuyente.
    /// </summary>
    /// <param name="psNombre">Nombre del contribuyente</param>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <param name="psCorreo">Correo del contribuyente</param>
    /// <param name="psPassword">Contraseña del contribuyente</param>
    /// <param name="psOrigen">Origen</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool solicitudRegistroContribuyente(string psNombre, string psUsuario, byte[] psCorreo, byte[] psPassword,char psOrigen)
    {
        bool bRetorno=false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_Registro_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sNombre", psNombre));
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", psCorreo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", psPassword));
                    cmd.Parameters.Add(new SqlParameter("sOrigen", psOrigen));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                bRetorno = true;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Encargado de traer la contraseña del contribuyente.
    /// </summary>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <param name="psEmail">Correo del contribuyente</param>
    /// <returns>Regresa un numero cuando encuentra la clave existente</returns>
    public int buscarClaveExistente(string psUsuario, string psEmail)
    {
        int nRetorno = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_BuscarUsu_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", psEmail));

                    con.Open();
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return nRetorno;
    }

    /// <summary>
    /// Encargado de ir a buscar la existencia del usuario.
    /// </summary>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <returns>Regresa los datos del usuario</returns>
    public DataTable buscarUsuario(string psUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RecuperaUsu_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Encargado de ir a buscar la existencia del usuario.
    /// </summary>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <returns>Regresa los datos del usuario</returns>
    public DataTable buscarUsuarioRFC(string psUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RecuperaUsuRFC_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Encargado de recuperar la lista de modulos del usuario.
    /// </summary>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <returns>Regresa los datos del usuario</returns>
    public DataTable fnRecuperaModulosUsuario(string psUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RecuperaModulos_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Encargado de actualizar la contraseña a petición del contribuyente.
    /// </summary>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <param name="psCorreo">Correo del contribuyente</param>
    /// <param name="psPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaContraseña(string psUsuario, string psCorreo, byte[] psPassword)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RecuperaUsu_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", psCorreo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", psPassword));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                bRetorno = true;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Actualiza el estado actual del usuario.
    /// </summary>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <param name="psCorreo">Correo del contribuyente</param>
    /// <param name="psEstadoActual">Estado actual del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaEstadoActual(string psUsuario, string psCorreo, char psEstadoActual)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizaEstado_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", psCorreo));
                    cmd.Parameters.Add(new SqlParameter("sEstadoActual", psEstadoActual));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                bRetorno = true;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Encargado de actualizar la fecha de ingreso al sistema.
    /// </summary>
    /// <param name="pnId_Usuario">ID Usuario</param>
    /// <param name="psUsuario">Usuario</param>
    /// <param name="psCorreo">Correo</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaFechaIngreso(int pnId_Usuario, string psUsuario, string psCorreo)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizaFechaIngreso_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("idUsu", pnId_Usuario));
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", psCorreo));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                bRetorno = true;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Actualiza la fecha de cambio para la expiracion de contraseña.
    /// </summary>
    /// <param name="pnId_usuario">id del usuario del contribuyente</param>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <param name="psCorreo">Correo del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaFechaCambio(int pnId_usuario, string psUsuario, string psCorreo)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizaFechaCambio_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("idUsu", pnId_usuario));
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", psCorreo));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                bRetorno = true;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Encargado de Actualizar las contraseñas para la caducidad.
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario del contribuyente</param>
    /// <param name="psTipo">tipo del usuario del contribuyente</param>
    /// <param name="psPassword">contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool actualizaContraseñaCaducidad(int pnIdUsuario, char psTipo, byte[] psPassword)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizaPassword_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nidUsu", pnIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("cTipo", psTipo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", psPassword));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                bRetorno = true;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    
    }

    /// <summary>
    /// Revisa la inactividad del usuario durante 30 dias.
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool RevisaInactividad(int pnIdUsuario)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RevisarInactividad_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdUsu", pnIdUsuario));

                    con.Open();
                    bRetorno = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Revisa Expiracion de la contraseña del contribuyente.
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool RevisaExpiracion(int pnIdUsuario)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RevisarExpiracionPass_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdUsu", pnIdUsuario));

                    con.Open();
                    bRetorno = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Revisa Pass Repetido Revisa 
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario del contribuyente</param>
    /// <param name="psPassword">contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool RevisaPassRepetido(int pnIdUsuario, byte[] psPassword)
    {
        bool bRetorno = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_ActualizPassRepetido_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("id_usuario", pnIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("sPassword", psPassword));

                    con.Open();
                    bRetorno = Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    }
}
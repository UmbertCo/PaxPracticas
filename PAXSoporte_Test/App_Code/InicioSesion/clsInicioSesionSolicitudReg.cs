using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Clase encargada de preparar el contribuyente para el registro.
/// </summary>
public class clsInicioSesionSolicitudReg
{
    /// <summary>
    /// Encargado de actualizar la contraseña a petición del contribuyente.
    /// </summary>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <param name="pbyCorreo">Correo del contribuyente</param>
    /// <param name="pbyPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool fnActualizaContraseña(string psUsuario, byte[] pbyCorreo, byte[] pbyPassword)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RecuperaUsuSoporte_Upd";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", pbyCorreo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", pbyPassword));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                } 
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaContraseña", "clsInicioSesionSolicitudReg");
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Encargado de Actualizar las contraseñas para la caducidad.
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario del contribuyente</param>
    /// <param name="pcTipo">tipo del usuario del contribuyente</param>
    /// <param name="psPassword">contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool fnActualizaContraseñaCaducidad(int pnIdUsuario, char pcTipo, string psPassword)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizaPassword_Upd";
                    cmd.Parameters.Add(new SqlParameter("nIdUsu", pnIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("cTipo", pcTipo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", psPassword));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaContraseñaCaducidad", "clsInicioSesionSolicitudReg");
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Actualiza el estado actual del usuario.
    /// </summary>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <param name="pbyCorreo">Correo del contribuyente</param>
    /// <param name="pcEstadoActual">Estado actual del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool fnActualizaEstadoActual(string psUsuario, byte[] pbyCorreo, char pcEstadoActual)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizaEstadoSoporte_Upd";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", pbyCorreo));
                    cmd.Parameters.Add(new SqlParameter("sEstadoActual", pcEstadoActual));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaEstadoActual", "clsInicioSesionSolicitudReg");
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Actualiza la fecha de cambio para la expiracion de contraseña.
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario del contribuyente</param>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <param name="psCorreo">Correo del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool fnActualizaFechaCambio(int pnIdUsuario, string psUsuario, string psCorreo)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizaFechaCambioSoporte_Upd";
                    cmd.Parameters.Add(new SqlParameter("idUsu", pnIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", psCorreo));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaFechaCambio", "clsInicioSesionSolicitudReg");
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Encargado de actualizar la fecha de ingreso al sistema.
    /// </summary>
    /// <param name="pnIdUsuario">id del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool fnActualizaFechaIngreso(int pnIdUsuario, string psUsuario, string psCorreo)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizaFechaIngresoSoporte_Upd";
                    cmd.Parameters.Add(new SqlParameter("idUsu", pnIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", psCorreo));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaFechaIngreso", "clsInicioSesionSolicitudReg");
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
    public int fnBuscarClaveExistente(string psUsuario, string psEmail)
    {
        int nRetorno = 0;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_BuscarUsu_Sel";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", psEmail));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnBuscarClaveExistente", "clsInicioSesionSolicitudReg");
            }
        }
        return nRetorno;
    }

    /// <summary>
    /// Encargado de ir a buscar la existencia del usuario.
    /// </summary>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <returns>Regresa los datos del usuario</returns>
    public DataTable fnBuscarUsuario(string psUsuario)
    {
        DataTable tabla = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RecuperaUsuarioSoporte_Sel";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(tabla);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnBuscarUsuario", "clsInicioSesionSolicitudReg");
            }
        }
        return tabla;
    }

    /// <summary>
    /// Revisa Expiracion de la contraseña del contribuyente.
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool fnRevisaExpiracion(int pnIdUsuario)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RevisarExpiracionPass_Sel";
                    cmd.Parameters.Add(new SqlParameter("nIdUsu", pnIdUsuario));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnRevisaExpiracion", "clsInicioSesionSolicitudReg");
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Revisa la inactividad del usuario durante 30 dias.
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool fnRevisaInactividad(int pnIdUsuario)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RevisarInactividad_Sel";
                    cmd.Parameters.Add(new SqlParameter("nIdUsu", pnIdUsuario));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnRevisaInactividad", "clsInicioSesionSolicitudReg");
            }
        }
            return bRetorno;
    }

    /// <summary>
    /// Revisa Pass Repetido Revisa 
    /// </summary>
    /// <param name="pnIdUsuario">id del usuario del contribuyente</param>
    /// <param name="password">contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool fnRevisaPassRepetido(int pnIdUsuario,string psPassword)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizPassRepetido_Upd";
                    cmd.Parameters.Add(new SqlParameter("id_usuario", pnIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("sPassword", psPassword));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnRevisaPassRepetido", "clsInicioSesionSolicitudReg");
            }
        }
            return bRetorno;
    }

    /// <summary>
    /// Encargado de generar el registro de la solicitud del contribuyente.
    /// </summary>
    /// <param name="psNombre">Nombre del contribuyente</param>
    /// <param name="psUsuario">Usuario del contribuyente</param>
    /// <param name="pbyCorreo">Correo del contribuyente</param>
    /// <param name="pbyPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool fnSolicitudRegistroContribuyente(string psNombre, string psUsuario, byte[] pbyCorreo, byte[] pbyPassword)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_Registro_Ins";
                    cmd.Parameters.Add(new SqlParameter("sNombre", psNombre));
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", pbyCorreo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", pbyPassword));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnSolicitudRegistroContribuyente", "clsInicioSesionSolicitudReg");
            }
        }
        return bRetorno;
    }
}
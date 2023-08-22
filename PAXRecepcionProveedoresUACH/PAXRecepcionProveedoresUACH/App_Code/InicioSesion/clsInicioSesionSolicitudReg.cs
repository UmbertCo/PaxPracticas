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
public class clsInicioSesionSolicitudReg
{
    /// <summary>
    /// Encargado de generar el registro de la solicitud del contribuyente.
    /// </summary>
    /// <param name="sNombre">Nombre del contribuyente</param>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="correo">Correo del contribuyente</param>
    /// <param name="sPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool solicitudRegistroContribuyente(string sNombre,string sUsuario, string correo,string sPassword,char sOrigen)
    {

        //bool bRetorno=false;

        //try
        //{

        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("sNombre", sNombre);
        //    conexion.AgregarParametro("sClaveUsuario", sUsuario);
        //    conexion.AgregarParametro("sEmail", correo);
        //    conexion.AgregarParametro("sPassword", sPassword);
        //    conexion.AgregarParametro("sOrigen", sOrigen);

        //    conexion.Query("usp_InicioSesion_Registro_Ins", true);

        //    bRetorno=true;
        //}
        //catch (Exception ex)
        //{
        //    bRetorno = false;
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}
        

        //return bRetorno;
        bool bRetorno = false;

        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_Registro_Ins";
                    cmd.Parameters.Add(new SqlParameter("sNombre", sNombre));
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", correo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", sPassword));
                    cmd.Parameters.Add(new SqlParameter("sOrigen", sOrigen));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            bRetorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }


        return bRetorno;
    }

    /// <summary>
    /// Encargado de traer la contraseña del contribuyente.
    /// </summary>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="email">Correo del contribuyente</param>
    /// <returns>Regresa un numero cuando encuentra la clave existente</returns>
    public int buscarClaveExistente(string sUsuario)
    {

        //int nRetorno = 0;

        //try
        //{
        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("sClaveUsuario", sUsuario);

        //    nRetorno=(int)conexion.TraerEscalar("usp_InicioSesion_BuscarUsu_Clave_Sel", true);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}

        //return nRetorno;

        int nRetorno = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_BuscarUsu_Clave_Sel";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return nRetorno;
    }

    /// <summary>
    /// Encargado de traer la contraseña del contribuyente.
    /// </summary>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="email">Correo del contribuyente</param>
    /// <returns>Regresa un numero cuando encuentra la clave existente</returns>
    public int buscarClaveExistente(string sUsuario, string sEmail)
    {

        //int nRetorno = 0;

        //try
        //{
        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("sClaveUsuario", sUsuario);
        //    conexion.AgregarParametro("sEmail", sEmail);
        //    nRetorno = (int)conexion.TraerEscalar("usp_InicioSesion_BuscarUsu_Sel", true);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}

        //return nRetorno;

        int nRetorno = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_BuscarUsu_Sel";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", sEmail));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //DataTable tabla = new DataTable();

        //try
        //{
        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("sClaveUsuario", sUsuario);

        //    conexion.Query("usp_InicioSesion_RecuperaUsu_Sel", true,ref tabla);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}

        //return tabla;

        DataTable tabla = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RecuperaUsu_Sel";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(tabla);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //DataTable tabla = new DataTable();

        //try
        //{
        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("sClaveUsuario", sUsuario);

        //    conexion.Query("usp_InicioSesion_RecuperaModulos_Sel", true, ref tabla);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}

        //return tabla;

        DataTable tabla = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RecuperaModulos_Sel";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(tabla);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //bool bRetorno = false;

        //try
        //{

        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("sClaveUsuario", sUsuario);
        //    conexion.AgregarParametro("sEmail", correo);
        //    conexion.AgregarParametro("sPassword", sPassword);

        //    conexion.Query("usp_InicioSesion_RecuperaUsu_Upd", true);

        //    bRetorno = true;
        //}
        //catch (Exception ex)
        //{
        //    bRetorno = false;
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}


        //return bRetorno;

        bool bRetorno = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RecuperaUsu_Upd";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", correo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", sPassword));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            bRetorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //bool bRetorno = false;

        //try
        //{

        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("sClaveUsuario", sUsuario);
        //    conexion.AgregarParametro("sEmail", correo);
        //    conexion.AgregarParametro("sEstadoActual", estadoActual);

        //    conexion.Query("usp_InicioSesion_ActualizaEstado_Upd", true);

        //    bRetorno = true;
        //}
        //catch (Exception ex)
        //{
        //    bRetorno = false;
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}


        //return bRetorno;

        bool bRetorno = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizaEstado_Upd";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", correo));
                    cmd.Parameters.Add(new SqlParameter("sEstadoActual", estadoActual));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            bRetorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //bool bRetorno = false;

        //try
        //{

        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("idUsu", id_usuario);
        //    conexion.AgregarParametro("sClaveUsuario", sUsuario);
        //    conexion.AgregarParametro("sEmail", correo);

        //    conexion.Query("usp_InicioSesion_ActualizaFechaIngreso_Upd", true);

        //    bRetorno = true;
        //}
        //catch (Exception ex)
        //{
        //    bRetorno = false;
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}


        //return bRetorno;

        bool bRetorno = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizaFechaIngreso_Upd";
                    cmd.Parameters.Add(new SqlParameter("idUsu", id_usuario));
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", correo));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            bRetorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //bool bRetorno = false;

        //try
        //{

        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("idUsu", id_usuario);
        //    conexion.AgregarParametro("sClaveUsuario", sUsuario);
        //    conexion.AgregarParametro("sEmail", correo);

        //    conexion.Query("usp_InicioSesion_ActualizaFechaCambio_Upd", true);

        //    bRetorno = true;
        //}
        //catch (Exception ex)
        //{
        //    bRetorno = false;
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}


        //return bRetorno;

        bool bRetorno = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizaFechaCambio_Upd";
                    cmd.Parameters.Add(new SqlParameter("idUsu", id_usuario));
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", correo));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            bRetorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //bool bRetorno = false;

        //try
        //{

        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("nIdUsu", idUsuario);
        //    conexion.AgregarParametro("cTipo", tipo);
        //    conexion.AgregarParametro("sPassword", sPassword);

        //    conexion.Query("usp_InicioSesion_ActualizaPassword_Upd", true);

        //    bRetorno = true;
        //}
        //catch (Exception ex)
        //{
        //    bRetorno = false;
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}


        //return bRetorno;

        bool bRetorno = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizaPassword_Upd";
                    cmd.Parameters.Add(new SqlParameter("nIdUsu", idUsuario));
                    cmd.Parameters.Add(new SqlParameter("cTipo", tipo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", sPassword));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            bRetorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //bool bRetorno = false;

        //try
        //{

        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("nIdUsu", idUsuario);

        //    bRetorno = Convert.ToBoolean(conexion.TraerEscalar("usp_InicioSesion_RevisarInactividad_Sel", true));

        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}


        //return bRetorno;

        bool bRetorno = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RevisarInactividad_Sel";
                    cmd.Parameters.Add(new SqlParameter("nIdUsu", idUsuario));
                    bRetorno = Convert.ToBoolean(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            bRetorno = false;
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //bool bRetorno = false;

        //try
        //{

        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("nIdUsu", idUsuario);

        //    bRetorno = Convert.ToBoolean(conexion.TraerEscalar("usp_InicioSesion_RevisarExpiracionPass_Sel", true));

        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}


        //return bRetorno;
        bool bRetorno = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RevisarExpiracionPass_Sel";
                    cmd.Parameters.Add(new SqlParameter("nIdUsu", idUsuario));

                    bRetorno = Convert.ToBoolean(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return bRetorno;
    }

    /// <summary>
    /// Revisa Pass Repetido Revisa 
    /// </summary>
    /// <param name="idUsuario">id del usuario del contribuyente</param>
    /// <param name="password">contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public bool RevisaPassRepetido(int idUsuario,string password)
    {

        //bool bRetorno = false;

        //try
        //{

        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("id_usuario", idUsuario);
        //    conexion.AgregarParametro("sPassword", password);

        //    bRetorno = Convert.ToBoolean(conexion.TraerEscalar("usp_InicioSesion_ActualizPassRepetido_Upd", true));

        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}


        //return bRetorno;


        bool bRetorno = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_ActualizPassRepetido_Upd";
                    cmd.Parameters.Add(new SqlParameter("id_usuario", idUsuario));
                    cmd.Parameters.Add(new SqlParameter("sPassword", password));
                    bRetorno = Convert.ToBoolean(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return bRetorno;
    }


}
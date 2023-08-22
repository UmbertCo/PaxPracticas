using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Configuration;

/// <summary>
/// Clase de capa de negocios para la pantalla webOperacionUsuarios
/// </summary>
public class clsOperacionUsuarios
{
    DataTable dtAuxiliar;


    /// <summary>
    /// Obtienen la lista que compone la estructura del sistema.(Version Paginada)
    /// </summary>
    /// <param name="psIdPadre"></param>
    /// <returns></returns>
    public DataTable fnObtenerEstructuraProveedorPag(string psIdPadre, bool bProveedor, int nPagina, int nNumPPagina, string sClave)
    {
        //giSql = clsComun.fnCrearConexion(conCuenta);
        //dtAuxiliar = new DataTable();

        //giSql.AgregarParametro("nId_Padre", psIdPadre);
        //giSql.AgregarParametro("bProveedor", bProveedor);
        //giSql.AgregarParametro("@sClave", sClave);
        //giSql.AgregarParametro("nPagina", nPagina);
        //giSql.AgregarParametro("nNumPPagina", nNumPPagina);
        //giSql.Query("usp_Con_Recupera_Hijos_Proveedor_Pag_Sel", true, ref dtAuxiliar);

        //return dtAuxiliar;
        DataTable dtAuxiliar = new DataTable();

        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Con_Recupera_Hijos_Proveedor_Pag_Sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Padre", psIdPadre));
                    cmd.Parameters.Add(new SqlParameter("bProveedor", bProveedor));
                    cmd.Parameters.Add(new SqlParameter("sClave", sClave));
                    cmd.Parameters.Add(new SqlParameter("nPagina", nPagina));
                    cmd.Parameters.Add(new SqlParameter("nNumPPagina", nNumPPagina));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }


        return dtAuxiliar;
    }

    /// <summary>
    /// Encargado de generar el registro de la solicitud del contribuyente.
    /// </summary>
    /// <param name="sNombre">Nombre del contribuyente</param>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="correo">Correo del contribuyente</param>
    /// <param name="sPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public int fnRegistroContribuyente( string sUsuario, string correo, string sPassword, string sUsuarioAlta, 
        string sSistemaOrigen, int nId_perfil, int nId_sucursal, string sEstatus)
    {
        //int Resultado = 0;
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    giSql.AgregarParametro("@sClaveUsuario", sUsuario);
        //    giSql.AgregarParametro("@sEmail", correo);
        //    giSql.AgregarParametro("@sPassword", sPassword);
        //    giSql.AgregarParametro("@sUsuarioAlta", sUsuarioAlta);
        //    giSql.AgregarParametro("@sSistemaOrigen", sSistemaOrigen);
        //    giSql.AgregarParametro("@nId_perfil", nId_perfil);
        //    giSql.AgregarParametro("@nId_sucursal", nId_sucursal);
        //    giSql.AgregarParametro("@sEstatus", sEstatus);

        //    Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Con_RegistroUsuario_Ins", true));
        //    return Resultado;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return Resultado;
        //}
        int Resultado = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Con_RegistroUsuario_Ins";
                    cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", correo));
                    cmd.Parameters.Add(new SqlParameter("sPassword", sPassword));
                    cmd.Parameters.Add(new SqlParameter("sUsuarioAlta", sUsuarioAlta));
                    cmd.Parameters.Add(new SqlParameter("sSistemaOrigen", sSistemaOrigen));
                    cmd.Parameters.Add(new SqlParameter("nId_perfil", nId_perfil));
                    cmd.Parameters.Add(new SqlParameter("nId_sucursal", nId_sucursal));
                    cmd.Parameters.Add(new SqlParameter("sEstatus", sEstatus));
                    Resultado = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }

            return Resultado;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return Resultado;
        }
    }


    /// <summary>
    /// Encargado de generar el registro de la relacion usuario-estructura.
    /// </summary>
    /// <param name="nidUsuario">Nombre del usuario</param>
    /// <param name="nidEstructura">identificador de estructura</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public void fnInsertaRelacionUsuarioSucursal(int nidUsuario, int nIdSucursal)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    giSql.AgregarParametro("@nIdUsuario", nidUsuario);
        //    giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
        //    giSql.TraerEscalar("usp_rfp_Usuario_Sucursal_Rel_ins", true);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);

        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Usuario_Sucursal_Rel_ins";
                    cmd.Parameters.Add(new SqlParameter("nIdUsuario", nidUsuario));
                    cmd.Parameters.Add(new SqlParameter("nIdSucursal", nIdSucursal));
                    cmd.ExecuteScalar();
                    con.Close();
                    con.Dispose();
                }
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Encargado de generar el registro de la relacion usuario-estatus.
    /// </summary>
    /// <param name="nidUsuario">Nombre del usuario</param>
    /// <param name="nidEstructura">identificador de estatus</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public void fnInsertaRelacionUsuarioEstatus(int nidUsuario, int nIdEstatus)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    giSql.AgregarParametro("@nIdUsuario", nidUsuario);
        //    giSql.AgregarParametro("@nIdEstatus", nIdEstatus);
        //    giSql.TraerEscalar("usp_rfp_Usuario_Estatus_rel_ins", true);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);

        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Usuario_Estatus_rel_ins";
                    cmd.Parameters.Add(new SqlParameter("nIdUsuario", nidUsuario));
                    cmd.Parameters.Add(new SqlParameter("nIdEstatus", nIdEstatus));
                    cmd.ExecuteScalar();
                    con.Close();
                    con.Dispose();
                }
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);

        }
    }

    public DataTable fnObtenerEstatusUsuario(int nIdEstatus)
    {
        //DataTable dtRes = new DataTable();
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    giSql.AgregarParametro("nIdUsuario", nIdEstatus);
        //    giSql.Query("usp_rfp_Obtener_Estatus_Usuario_sel", true, ref dtRes);
        //}
        //catch (Exception ex)
        //{

        //}
        //return dtRes;
        DataTable dtRes = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Obtener_Estatus_Usuario_sel";
                    cmd.Parameters.Add(new SqlParameter("nIdUsuario", nIdEstatus));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtRes);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return dtRes;
    }

    /// <summary>
    /// Obtiene los modulos hijo segun el idpadre
    /// </summary>
    /// <param name="nidPadre">identificador padre</param>
    /// <param name="incluyeRuta">indica si se debe recuperar la ruta del modulo</param>
    /// <returns>Regresa la lista de modulos hijo</returns>
    public DataTable fnSeleccionaModulosHijo(int nidPerfil, bool incluyeRuta = false)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("nId_Perfil", nidPerfil);
        //    if (incluyeRuta)
        //        giSql.AgregarParametro("bObtieneRuta", incluyeRuta);
        //    giSql.Query("usp_Ctp_ModulosHijos_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return null;

        DataTable dtAuxiliar = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_ModulosHijos_sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Perfil", nidPerfil));
                    if (incluyeRuta)
                        cmd.Parameters.Add(new SqlParameter("bObtieneRuta", incluyeRuta));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }

            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    /// <summary>
    /// Obtiene los perfiles de la aplicacion de cobro
    /// </summary>   
    /// <returns>Regresa la lista de los perfiles de cobro</returns>
    public DataTable fnCargaPerfiles()
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.Query("usp_rfp_PerfilesSistema_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return null;
        //}
        DataTable dtAuxiliar = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_PerfilesSistema_sel";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }

            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }


    /// <summary>
    /// Obtiene el detalle de un usuario en especifico
    /// /// <param name="nidUsuario">identificador del usuario</param>
    /// </summary>   
    /// <returns>Regresa la lista de los usuarios</returns>
    public DataTable fnObtenerInfoUsuario(int nidUsuario)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nId_Usuario", nidUsuario);
        //    giSql.Query("usp_Ctp_UsuarioDetalle_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return null;
        //}
        try
        {
            DataTable dtAuxiliar = new DataTable();

            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_UsuarioDetalle_sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nidUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }

            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }


    /// <summary>
    /// Obtiene los comprobantes de un usuario 
    /// /// <param name="nidUsuario">identificador del usuario</param>
    /// </summary>   
    /// <returns>Regresa la lista de los usuarios</returns>
    public int fnObtenerComprobantesUsuario(int nidUsuario)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nId_Usuario", nidUsuario);
        //    int retorno = Convert.ToInt32(giSql.TraerEscalar("usp_Ctp_UsuarioComprobantesGenerados_sel", true));
        //    return retorno;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return 0;
        //}
        int retorno = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_UsuarioComprobantesGenerados_sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nidUsuario));
                    retorno = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
        return retorno;
    }

    /// <summary>
    /// Obtiene los usuarios asignados a un perfil
    /// /// <param name="nidUsuario">identificador del perfil</param>
    /// </summary>   
    /// <returns>Regresa la lista de los usuarios</returns>
    public int fnObtenerUsuariosPerfil(int nidPerfil)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nId_Perfil", nidPerfil);
        //    int retorno = Convert.ToInt32(giSql.TraerEscalar("usp_Ctp_PerfilUsuarios_sel", true));
        //    return retorno;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return 0;
        //}
        try
        {
            DataTable dtAuxiliar = new DataTable();

            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_PerfilUsuarios_sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Perfil", nidPerfil));
                    int retorno = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                    return retorno;
                }
            }


        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
    }

    /// <summary>
    /// Elimina los modulos del usuario seleccionado
    /// /// <param name="nidUsuario">identificador del usuario</param>
    /// </summary>   
    /// <returns>Regresa la lista de los usuarios</returns>
    public void fnEliminarModulosUsuario(int nidUsuario)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nId_Usuario", nidUsuario);
        //    giSql.NoQuery("usp_Ctp_Usuario_Modulos_del", true);

        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Usuario_Modulos_del";
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nidUsuario));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                }
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Elimina los modulos del usuario seleccionado
    /// /// <param name="nidUsuario">identificador del usuario</param>
    /// </summary>   
    /// <returns>Regresa la lista de los usuarios</returns>
    public void fnEliminarEstatusUsuario(int nidUsuario)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nId_Usuario", nidUsuario);
        //    giSql.NoQuery("usp_Ctp_Usuario_Estatus_del", true);

        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Usuario_Estatus_del";
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nidUsuario));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                }
            }


        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }


    /// <summary>
    /// Elimina la relacion de usuario con estructura
    /// /// <param name="nidUsuario">identificador del usuario</param>
    /// </summary>   
    /// <returns>Regresa la lista de los usuarios</returns>
    public void fnEliminarSucursalUsuario(int nidUsuario)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nIdUsuario", nidUsuario);
        //    giSql.NoQuery("usp_rfp_Eliminar_Usuario_Sucursales_Rel_del", true);

        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Eliminar_Usuario_Sucursales_Rel_del";
                    cmd.Parameters.Add(new SqlParameter("nIdUsuario", nidUsuario));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                }
            }


        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Elimina la relacion de perfiles con modulos
    /// /// <param name="nidUsuario">identificador del perfil</param>
    /// </summary>   
    public void fnEliminarPerfilModulo(int nidPerfil)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nIdPerfil", nidPerfil);
        //    giSql.NoQuery("usp_rfp_Eliminar_Perfil_Modulo_Rel_del", true);

        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Eliminar_Perfil_Modulo_Rel_del";
                    cmd.Parameters.Add(new SqlParameter("nIdPerfil", nidPerfil));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                }
            }


        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Actualiza la informacion del usuario en especifico
    ///<param name="nidUsuario">identificador del usuario</param>
    /// ///<param name="sNombre">razon social del usuario</param>
    ///  ///<param name="nIdContribuyente">identificador del contribuyente</param>
    ///   ///<param name="sClaveUsuario">identificador del usuario</param>
    ///    ///<param name="sEmail">correo electronico del usuario</param>
    /// </summary>   
    /// <returns></returns>
    public void fnActualizaUsuarioInfo(int nidUsuario,   string sClaveUsuario, string sEmail, string sPassword, int nIdPerfil)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nId_Usuario", nidUsuario);
        //    giSql.AgregarParametro("@sClave_Usuario", sClaveUsuario);
        //    giSql.AgregarParametro("@sEmail", sEmail);
        //    if(!string.IsNullOrEmpty(sPassword))
        //        giSql.AgregarParametro("@sPassword", sPassword);
        //    giSql.AgregarParametro("@nIdPerfil", nIdPerfil);
        //    giSql.NoQuery("usp_Ctp_UsuarioDetalle_upd", true);

        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_UsuarioDetalle_upd";
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nidUsuario));
                    cmd.Parameters.Add(new SqlParameter("sClave_Usuario", sClaveUsuario));
                    cmd.Parameters.Add(new SqlParameter("sEmail", sEmail));
                    if (!string.IsNullOrEmpty(sPassword))
                        cmd.Parameters.Add(new SqlParameter("Password", sPassword));
                    cmd.Parameters.Add(new SqlParameter("nIdPerfil", nIdPerfil));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }


    /// <summary>
    /// Obtiene los comprobantes de un usuario 
    /// /// <param name="nidUsuario">identificador del usuario</param>
    /// </summary>   
    /// <returns>Regresa la lista de los usuarios</returns>
    public DataTable fnObtenerInfoBasicaUsuario(int nidUsuario)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nId_Usuario", nidUsuario);
        //    giSql.Query("usp_Ctp_UsuarioBasico_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return null;
        //}
        DataTable dtAuxiliar = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_UsuarioBasico_sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nidUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }

            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    /// <summary>
    /// Obtiene los modulos para asignar a un nuevo perfil
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerModulos()
    {
        //try
        //{
        //    DataTable modulos = new DataTable("Modulos");
        //    InterfazSQL iSql = clsComun.fnCrearConexion(conCuenta);
        //    iSql.Query("usp_InicioSesion_Modulos_Sel", true, ref modulos);
        //    return modulos;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return null;
        //}
        try
        {
            DataTable modulos = new DataTable("Modulos");
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_Modulos_Sel";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(modulos);
                    }
                }
            }

            return modulos;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    /// <summary>
    /// Guarda un nuevo perfil con los modulos asignados por el usuario
    /// </summary>
    /// <param name="nombrePerfil">Nombre del nuevo perfil</param>
    /// <param name="nIdModulosSel">Lista de id de los modulos correspondientes al perfil</param>
    public void fnGuardaNuevoPerfil(string nombrePerfil, List<int> nIdModulosSel)
    {
        //try
        //{
        //    InterfazSQL iSql = clsComun.fnCrearConexion(conCuenta);
        //    string listaModulosXml = string.Empty;
        //    string modulosEnXml = string.Empty;
        //    foreach (int id in nIdModulosSel)
        //    {
        //        listaModulosXml += "<Modulo><id>" + id + "</id></Modulo>";
        //    }
        //    string cadenaEnXml = string.Empty;
        //    modulosEnXml += "<Modulos>" + listaModulosXml + "</Modulos>";
        //    iSql.AgregarParametro("cNombrePerfil", nombrePerfil);
        //    iSql.AgregarParametro("cModulos", modulosEnXml);
        //    iSql.NoQuery("usp_InicioSesion_Perfiles_ins", true);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    string listaModulosXml = string.Empty;
                    string modulosEnXml = string.Empty;
                    foreach (int id in nIdModulosSel)
                    {
                        listaModulosXml += "<Modulo><id>" + id + "</id></Modulo>";
                    }
                    string cadenaEnXml = string.Empty;
                    modulosEnXml += "<Modulos>" + listaModulosXml + "</Modulos>";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_Perfiles_ins";
                    cmd.Parameters.Add(new SqlParameter("cNombrePerfil", nombrePerfil));
                    cmd.Parameters.Add(new SqlParameter("cModulos", modulosEnXml));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                }
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Elimina un perfil de Usuario
    /// </summary>
    /// <param name="@nIdPerfil">Id de Perfil</param>
    public void fnEliminaPerfil(int idPerfil)
    {
        //try
        //{
        //    InterfazSQL iSql = clsComun.fnCrearConexion(conCuenta);
        //    iSql.AgregarParametro("@nIdPerfil", idPerfil);
        //    iSql.NoQuery("usp_InicioSesion_Eliminar_Perfil_up", true);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_Eliminar_Perfil_up";
                    cmd.Parameters.Add(new SqlParameter("nIdPerfil", idPerfil));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                }
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Verifica si el usuario tiene el perfil seleccionado
    /// </summary>
    /// <param name="nIdUsuario"></param>
    /// <param name="sPerfil"></param>
    /// <returns></returns>
    public bool fnVerificarUsuarioPerfil(int nIdUsuario, string sPerfil)
    {
        //bool res = false;
        //try
        //{
        //    InterfazSQL iSql = clsComun.fnCrearConexion(conCuenta);
            
        //    iSql.AgregarParametro("nIdUsuario", nIdUsuario);
        //    iSql.AgregarParametro("sPerfil", sPerfil);
        //    res = Convert.ToBoolean(iSql.TraerEscalar("usp_ses_Usuario_Perfil_sel", true));
        //}
        //catch (Exception ex)
        //{
        //    return false;
        //}
        //return res;
        bool res = false;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_ses_Usuario_Perfil_sel";
                    cmd.Parameters.Add(new SqlParameter("nIdUsuario", nIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("sPerfil", sPerfil));
                    res = Convert.ToBoolean(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return false;
        }
        return res;
    }
}
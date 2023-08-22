using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Data;
using System.Text;

/// <summary>
/// Clase de capa de negocios para la pantalla webOperacionUsuarios
/// </summary>
public class clsOperacionUsuarios
{
    private InterfazSQL giSql;
    private string conCuenta = "conConfiguracion";
    DataTable dtAuxiliar;

    /// <summary>
    /// Retorna los datos del usuario
    /// </summary>
    /// <returns>Retorna un SqlDataReader con los datos fiscales de la matriz</returns>
    public SqlDataReader fnObtenerDatosUsuario()
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);
        return giSql.Query("usp_Con_Usuario_Sel", true);
    }

    /// <summary>
    /// Obtienen la lista que compone la estructura del sistema.
    /// </summary>
    /// <param name="psIdPadre"></param>
    /// <returns></returns>
    public DataTable fnObtenerEstructura(string psIdPadre)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Padre", psIdPadre);
        giSql.Query("usp_Con_Recupera_Usuarios_Sel", true, ref dtAuxiliar);

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
    public int fnRegistroContribuyente(string sNombre, string sUsuario, string correo, string sPassword,string sUsuarioAlta, string sSistemaOrigen)
    {
        int Resultado = 0;
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            giSql.AgregarParametro("@sNombre", sNombre);
            giSql.AgregarParametro("@sClaveUsuario", sUsuario);
            giSql.AgregarParametro("@sEmail", correo);
            giSql.AgregarParametro("@sPassword", sPassword);
            giSql.AgregarParametro("@sUsuarioAlta", sUsuarioAlta);
            giSql.AgregarParametro("@sSistemaOrigen", sSistemaOrigen);

            Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Con_RegistroUsuario_Ins", true));
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
    public void fnInsertaRelacionUsuarioEstructura(int nidUsuario, int nidEstructura)
    {        
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            giSql.AgregarParametro("@nId_Usuario", nidUsuario);
            giSql.AgregarParametro("@nId_Estructura", nidEstructura);
            giSql.TraerEscalar("usp_Ctp_Usuario_RegistraDatos_ins", true);                
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
         
        }          
    }



    /// <summary>
    /// Encargado de consultar los modulos padre del sistema.
    /// </summary>
  
    /// <returns>Regresa todos los registros padre en la tabla de modulos</returns>
    public int fnSeleccionaModulosPadre(int pIdModuloHijo)
    {
        try
        {
            int IdpModuloPadre = 0;
            giSql = clsComun.fnCrearConexion(conCuenta);
            giSql.AgregarParametro("@nIdModuloHijo", pIdModuloHijo);
            IdpModuloPadre = Convert.ToInt32(giSql.TraerEscalar("usp_Ctp_ModulosPadre_sel", true));
            return IdpModuloPadre;
            
        }
        catch (Exception ex)
        {           
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            int IdpModuloPadre = 0;
            return IdpModuloPadre;
        }
    }


    /// <summary>
    /// Obtiene los modulos hijo segun el idpadre
    /// </summary>
    /// <param name="nidPadre">identificador padre</param>
    /// <returns>Regresa la lista de modulos hijo</returns>
    public DataTable fnSeleccionaModulosHijo(int nidPerfil)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Perfil", nidPerfil);
            giSql.Query("usp_Ctp_ModulosHijos_sel", true, ref dtAuxiliar);
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
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.Query("usp_Ctp_PerfilesSistema_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }


    /// <summary>
    /// Inserta un nuevo registro en las tablas de usuario-modulo y perfil-modulo
    /// </summary>
    /// <param name="nidUsuario">identificador del usuario</param>
    /// <param name="nidPerfil">identificador del perfil</param>
    /// <param name="nIdModulo">identificador del modulo</param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public void fnInsertaRelacionPerfilUsuarioModulo(int nidUsuario, int nidPerfil, int nIdModulo)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            giSql.AgregarParametro("@nIdUsuario", nidUsuario);
            giSql.AgregarParametro("@nIdPerfil", nidPerfil);
            giSql.AgregarParametro("@nIdModulo", nIdModulo);
            giSql.TraerEscalar("usp_Ctp_ModulosRelacion_Ins", true);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);

        }
    }

    /// <summary>
    /// Obtiene el detalle de un usuario en especifico
    /// /// <param name="nidUsuario">identificador del usuario</param>
    /// </summary>   
    /// <returns>Regresa la lista de los usuarios</returns>
    public DataTable fnObtenerInfoUsuario(int nidUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Usuario", nidUsuario);
            giSql.Query("usp_Ctp_UsuarioDetalle_sel", true, ref dtAuxiliar);
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
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Usuario", nidUsuario);
            int retorno =Convert.ToInt32(giSql.TraerEscalar("usp_Ctp_UsuarioComprobantesGenerados_sel", true));
            return retorno;
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
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Usuario", nidUsuario);
            giSql.NoQuery("usp_Ctp_Usuario_Modulos_del", true);

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
    public void fnEliminarEstructuraUsuario(int nidUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Usuario", nidUsuario);
            giSql.NoQuery("usp_Ctp_UsuarioEstructura_del", true);

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
    public void fnActualizaUsuarioInfo(int nidUsuario, string sNombre, int nIdContribuyente, string sClaveUsuario, string sEmail)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Usuario", nidUsuario);
            giSql.AgregarParametro("@sNombre", sNombre);
            giSql.AgregarParametro("@nId_Contribuyente", nIdContribuyente);
            giSql.AgregarParametro("@sClave_Usuario", sClaveUsuario);
            giSql.AgregarParametro("@sEmail", sEmail);
            giSql.NoQuery("usp_Ctp_UsuarioDetalle_upd", true);

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
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Usuario", nidUsuario);
            giSql.Query("usp_Ctp_UsuarioBasico_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    /// <summary>
    /// Obtiene el correo del receptor
    /// </summary>
    /// <param name="pnid_cfd"></param>
    /// <returns></returns>
    public string fnObtenerCorreoReceptor(int pnid_cfd)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdCfd", pnid_cfd);
            string retorno = Convert.ToString(giSql.TraerEscalar("usp_Cfd_ObtieneEmailReceptor_Sel", true));
            return retorno;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return "";
        }
    }
}
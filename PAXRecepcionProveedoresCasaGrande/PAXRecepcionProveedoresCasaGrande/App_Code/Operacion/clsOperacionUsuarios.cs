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
    private string conCuenta = "conRecepcionProveedores";
    DataTable dtAuxiliar;

    /// <summary>
    /// Retorna los datos del usuario
    /// </summary>
    /// <returns>Retorna un SqlDataReader con los datos fiscales de la matriz</returns>
    public SqlDataReader fnObtenerDatosUsuario()
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_usuario);
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
        giSql.Query("usp_Con_Recupera_Hijos_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Obtienen la lista que compone la estructura del sistema.(Version Paginada)
    /// </summary>
    /// <param name="psIdPadre"></param>
    /// <returns></returns>
    public DataTable fnObtenerEstructuraPag(string psIdPadre, int nPagina, int numPag, string sClave)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Padre", psIdPadre);
        giSql.AgregarParametro("sClave", sClave);
        giSql.AgregarParametro("nPagina", nPagina);
        giSql.AgregarParametro("nNumPPagina", numPag);
        giSql.Query("usp_Con_Recupera_Hijos_Pag_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }


    /// <summary>
    /// Obtienen la lista que compone la estructura del sistema.
    /// </summary>
    /// <param name="psIdPadre"></param>
    /// <returns></returns>
    public DataTable fnObtenerEstructuraProveedor(string psIdPadre, bool bProveedor)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Padre", psIdPadre);
        giSql.AgregarParametro("bProveedor", bProveedor);
        giSql.Query("usp_Con_Recupera_Hijos_Proveedor_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Obtienen la lista que compone la estructura del sistema.(Version Paginada)
    /// </summary>
    /// <param name="psIdPadre"></param>
    /// <returns></returns>
    public DataTable fnObtenerEstructuraProveedorPag(string psIdPadre, bool bProveedor, int nPagina, int nNumPPagina, string sClave)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Padre", psIdPadre);
        giSql.AgregarParametro("bProveedor", bProveedor);
        giSql.AgregarParametro("@sClave", sClave);
        giSql.AgregarParametro("nPagina", nPagina);
        giSql.AgregarParametro("nNumPPagina", nNumPPagina);
        giSql.Query("usp_Con_Recupera_Hijos_Proveedor_Pag_Sel", true, ref dtAuxiliar);

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
        int Resultado = 0;
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            giSql.AgregarParametro("@sClaveUsuario", sUsuario);
            giSql.AgregarParametro("@sEmail", correo);
            giSql.AgregarParametro("@sPassword", sPassword);
            giSql.AgregarParametro("@sUsuarioAlta", sUsuarioAlta);
            giSql.AgregarParametro("@sSistemaOrigen", sSistemaOrigen);
            giSql.AgregarParametro("@nId_perfil", nId_perfil);
            giSql.AgregarParametro("@nId_sucursal", nId_sucursal);
            giSql.AgregarParametro("@sEstatus", sEstatus);

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
    public void fnInsertaRelacionUsuarioSucursal(int nidUsuario, int nIdSucursal)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            giSql.AgregarParametro("@nIdUsuario", nidUsuario);
            giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
            giSql.TraerEscalar("usp_rfp_Usuario_Sucursal_Rel_ins", true);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);

        }
    }

    public void fnInsertaSucursal(string nombre, int nIdPadre = 0)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            giSql.AgregarParametro("cNombreSuc", nombre);
            if (nIdPadre > 0)
                giSql.AgregarParametro("nIdPadre", nIdPadre);
            giSql.NoQuery("usp_rfp_Sucursal_ins", true);

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
    /// <param name="incluyeRuta">indica si se debe recuperar la ruta del modulo</param>
    /// <returns>Regresa la lista de modulos hijo</returns>
    public DataTable fnSeleccionaModulosHijo(int nidPerfil, bool incluyeRuta = false)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("nId_Perfil", nidPerfil);
            if (incluyeRuta)
                giSql.AgregarParametro("bObtieneRuta", incluyeRuta);
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
            giSql.Query("usp_rfp_PerfilesSistema_sel", true, ref dtAuxiliar);
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
            int retorno = Convert.ToInt32(giSql.TraerEscalar("usp_Ctp_UsuarioComprobantesGenerados_sel", true));
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
    public void fnEliminarSucursalUsuario(int nidUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdUsuario", nidUsuario);
            giSql.NoQuery("usp_rfp_Eliminar_Usuario_Sucursales_Rel_del", true);

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
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Usuario", nidUsuario);
            giSql.AgregarParametro("@sClave_Usuario", sClaveUsuario);
            giSql.AgregarParametro("@sEmail", sEmail);
            //if(!string.IsNullOrEmpty(sPassword))
            //    giSql.AgregarParametro("@sPassword", sPassword);
            giSql.AgregarParametro("@nIdPerfil", nIdPerfil);
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
    /// <summary>
    /// Obtiene los modulos para asignar a un nuevo perfil
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerModulos()
    {
        try
        {
            DataTable modulos = new DataTable("Modulos");
            InterfazSQL iSql = clsComun.fnCrearConexion(conCuenta);
            iSql.Query("usp_InicioSesion_Modulos_Sel", true, ref modulos);
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
    public int fnGuardaNuevoPerfil(string nombrePerfil, List<int> nIdModulosSel)
    {
        int res = 0;
        try
        {
            InterfazSQL iSql = clsComun.fnCrearConexion(conCuenta);
            string listaModulosXml = string.Empty;
            string modulosEnXml = string.Empty;
            foreach (int id in nIdModulosSel)
            {
                listaModulosXml += "<Modulo><id>" + id + "</id></Modulo>";
            }
            string cadenaEnXml = string.Empty;
            modulosEnXml += "<Modulos>" + listaModulosXml + "</Modulos>";
            iSql.AgregarParametro("cNombrePerfil", nombrePerfil);
            iSql.AgregarParametro("cModulos", modulosEnXml);
            res = Convert.ToInt32(iSql.TraerEscalar("usp_InicioSesion_Perfiles_ins", true));
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return res;
    }

    /// <summary>
    /// Guarda un nuevo perfil con los modulos asignados por el usuario
    /// </summary>
    /// <param name="nIdPefil">Nombre del nuevo perfil</param>
    /// <param name="nIdModulosSel">Lista de id de los modulos correspondientes al perfil</param>
    public int fnEditarPerfil(int nIdPefil, List<int> nIdModulosSel)
    {
        int res = 0;
        try
        {
            InterfazSQL iSql = clsComun.fnCrearConexion(conCuenta);
            string listaModulosXml = string.Empty;
            string modulosEnXml = string.Empty;
            foreach (int id in nIdModulosSel)
            {
                listaModulosXml += "<Modulo><id>" + id + "</id></Modulo>";
            }
            string cadenaEnXml = string.Empty;
            modulosEnXml += "<Modulos>" + listaModulosXml + "</Modulos>";
            iSql.AgregarParametro("nIdPerfil", nIdPefil);
            iSql.AgregarParametro("cModulos", modulosEnXml);
            res = Convert.ToInt32(iSql.TraerEscalar("usp_InicioSesion_Perfiles_Editar_ins", true));
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return res;
    }
    /// <summary>
    /// Verifica si el usuario tiene el perfil seleccionado por el tipo de perfil
    /// 
    /// </summary>
    /// <param name="nIdUsuario">Id Usuario</param>
    /// <param name="sPerfil">Tipo Perfil: A=Adminsitrador, P=Proveedor, C=Resto</param>
    /// <returns></returns>
    public bool fnVerificarUsuarioPerfil(int nIdUsuario, string sPerfil)
    {
        bool res = false;
        try
        {
            InterfazSQL iSql = clsComun.fnCrearConexion(conCuenta);
            
            iSql.AgregarParametro("nIdUsuario", nIdUsuario);
            iSql.AgregarParametro("sPerfil", sPerfil);
            res = Convert.ToBoolean(iSql.TraerEscalar("usp_ses_Usuario_Perfil_sel", true));
        }
        catch (Exception ex)
        {
            return false;
        }
        return res;
    }

    /// <summary>
    /// Obtiene los usuarios asignados a un perfil
    /// /// <param name="nidUsuario">identificador del perfil</param>
    /// </summary>   
    /// <returns>Regresa la lista de los usuarios</returns>
    public int fnObtenerUsuariosPerfil(int nidPerfil)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Perfil", nidPerfil);
            int retorno = Convert.ToInt32(giSql.TraerEscalar("usp_Ctp_PerfilUsuarios_sel", true));
            return retorno;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
    }

    /// <summary>
    /// Elimina la relacion de perfiles con modulos
    /// /// <param name="nidUsuario">identificador del perfil</param>
    /// </summary>   
    public void fnEliminarPerfilModulo(int nidPerfil)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conCuenta);
            dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdPerfil", nidPerfil);
            giSql.NoQuery("usp_rfp_Eliminar_Perfil_Modulo_Rel_del", true);

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
        try
        {
            InterfazSQL iSql = clsComun.fnCrearConexion(conCuenta);
            iSql.AgregarParametro("@nIdPerfil", idPerfil);
            iSql.NoQuery("usp_InicioSesion_Eliminar_Perfil_up", true);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Data;
using System.Text;

/// <summary>
/// Clase de capa de negocio para la pantalla webOperacionSucursales
/// </summary>
public class clsOperacionSucursales
{
    private InterfazSQL giSql;
    private DataTable dtAuxiliar;
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
    public int fnGuardarSucursal(string sNumTienda,string sIdSucursal,string psIdEstructura, string psSucursal,
                                    string psCalle, string psNoExterior, string psNoInterior,
                                    string psColonia, string psReferencia, string psCodigoPostal, string psLocalidad,
                                    string psMunicipio, string psIdEstado, string nId_padre)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        if (!string.IsNullOrEmpty(psIdEstructura))
            giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        giSql.AgregarParametro("@sNumTienda", sNumTienda);
        giSql.AgregarParametro("@nIdSucursal", sIdSucursal);
        giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        giSql.AgregarParametro("@sSucursal", psSucursal);
        giSql.AgregarParametro("@sCalle", psCalle);
        if (!string.IsNullOrEmpty(psNoExterior))
            giSql.AgregarParametro("@sNumero_Exterior", psNoExterior);
        if (!string.IsNullOrEmpty(psNoInterior))
            giSql.AgregarParametro("@sNumero_Interior", psNoInterior);
        if (!string.IsNullOrEmpty(psColonia))
            giSql.AgregarParametro("@sColonia", psColonia);
        if (!string.IsNullOrEmpty(psReferencia))
            giSql.AgregarParametro("@sReferencia", psReferencia);
        giSql.AgregarParametro("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
        if (!string.IsNullOrEmpty(psLocalidad))
            giSql.AgregarParametro("@sLocalidad", psLocalidad);
        giSql.AgregarParametro("@sMunicipio", psMunicipio);
        giSql.AgregarParametro("@nId_Estado", psIdEstado);
        giSql.AgregarParametro("@nId_padre", nId_padre);

        return (int)giSql.TraerEscalar("usp_Con_Estructrua", true);//giSql.NoQuery("usp_Con_Estructrua", true);


    }
    /// <summary>
    /// Actualiza el nombre del nodo principal
    /// </summary>
    /// <param name="snomTienda"></param>
    /// <param name="nidSucursal"></param>
    public void fnActualizaTienda(string snomTienda, int nidSucursal, byte[] byLogo, byte[] byTicket
        ,string calle
        ,string numero_exterior
        ,string numero_interior
        ,string colonia
        ,string referencia
        ,string localidad
        ,string municipio
        ,int    id_estado
        ,string codigo_postal
        ,string regimen_fiscal )
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        try
        {

            if (!string.IsNullOrEmpty(snomTienda))
                giSql.AgregarParametro("@nomTienda", snomTienda);
            if (nidSucursal > 0)
                giSql.AgregarParametro("@Estructura", nidSucursal);
            if (byLogo.Length > 0)
                giSql.AgregarParametro("@byLogo", byLogo);
            if (byTicket.Length > 0)
                giSql.AgregarParametro("@byTicket", byTicket);
            //giSql.AgregarParametro("@id_domicilio", id_domicilio);
            giSql.AgregarParametro("@calle", calle);
            if (!string.IsNullOrEmpty(numero_exterior))
                giSql.AgregarParametro("@numero_exterior", numero_exterior);
            if (!string.IsNullOrEmpty(numero_interior))
                giSql.AgregarParametro("@numero_interior", numero_interior);
            if (!string.IsNullOrEmpty(colonia))
                giSql.AgregarParametro("@colonia", colonia);
            if (!string.IsNullOrEmpty(referencia))
                giSql.AgregarParametro("@referencia", referencia);
            if (!string.IsNullOrEmpty(localidad))
                giSql.AgregarParametro("@localidad", localidad);
            giSql.AgregarParametro("@municipio", municipio);
            giSql.AgregarParametro("@id_estado", id_estado);
            giSql.AgregarParametro("@codigo_postal", codigo_postal);
            if (!string.IsNullOrEmpty(regimen_fiscal))
                giSql.AgregarParametro("@regimen_fiscal", regimen_fiscal);
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

    /*@id_domicilio		NVARCHAR(50),        
	  @calle			NVARCHAR(50),        
	  @numero_exterior  NVARCHAR(50)= NULL,
	  @numero_interior	NVARCHAR(50)= NULL,
	  @colonia			NVARCHAR(50)= NULL,
	  @referencia		NVARCHAR(50)= NULL,
	  @localidad		NVARCHAR(50)= NULL,
	  @municipio		NVARCHAR(50),        
	  @id_estado		NVARCHAR(50),        
      @codigo_postal	NVARCHAR(50),        
      @regimen_fiscal	NVARCHAR(50)= NULL*/

        giSql.NoQuery("usp_Con_Estructura_Upd", true);
    }

    /// <summary>
    /// Guarda la Tienda como matriz, inserta la relacion con la pagina de Cobro
    /// </summary>
    /// <param name="snomTienda"></param>
    /// <param name="nidSucursal"></param>
    public int fnGuardarTienda(string sFranquicia, int nEstructuraCobro
        , string calle
        , string numero_exterior
        , string numero_interior
        , string colonia
        , string referencia
        , string localidad
        , string municipio
        , int id_estado
        , string codigo_postal
        , string regimen_fiscal)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        giSql.AgregarParametro("@nomTienda", sFranquicia);
        giSql.AgregarParametro("@EstructuraCobro", nEstructuraCobro);

        giSql.AgregarParametro("@calle", calle);
        if (!string.IsNullOrEmpty(numero_exterior))
            giSql.AgregarParametro("@numero_exterior", numero_exterior);
        if (!string.IsNullOrEmpty(numero_interior))
            giSql.AgregarParametro("@numero_interior", numero_interior);
        if (!string.IsNullOrEmpty(colonia))
            giSql.AgregarParametro("@colonia", colonia);
        if (!string.IsNullOrEmpty(referencia))
            giSql.AgregarParametro("@referencia", referencia);
        if (!string.IsNullOrEmpty(localidad))
            giSql.AgregarParametro("@localidad", localidad);
        giSql.AgregarParametro("@municipio", municipio);
        giSql.AgregarParametro("@id_estado", id_estado);
        giSql.AgregarParametro("@codigo_postal", codigo_postal);
        if (!string.IsNullOrEmpty(regimen_fiscal))
            giSql.AgregarParametro("@regimen_fiscal", regimen_fiscal);

        return (int)giSql.TraerEscalar("usp_Con_Estructura_Ins", true);

    }

    /// <summary>
    /// Guarda la Relacion de la regla de negocio
    /// </summary>
    /// <param name="snomTienda"></param>
    /// <param name="nidSucursal"></param>
    public void fnGuardarRelacionRegla(string sEstructura, string sRegla, string retVal)
    {
        
        giSql = clsComun.fnCrearConexion(conSucursales);

        try
        {

            if (!string.IsNullOrEmpty(sEstructura))
                giSql.AgregarParametro("@sEstructura", sEstructura);
            giSql.AgregarParametro("@Regla", sRegla);
            if (!string.IsNullOrEmpty(retVal))
                giSql.AgregarParametro("@retVal", retVal);
            giSql.NoQuery("usp_Con_Regla_Rel_Ins", true);

        }catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    /// <summary>
    /// Guarda la Imagen el la sucursal
    /// </summary>
    /// <param name="snomTienda"></param>
    /// <param name="nidSucursal"></param>
    public void fnAgregarImagen(int retVal, byte[] bytLogo)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        giSql.AgregarParametro("@retVal", retVal);
        if (bytLogo.Length > 0)
            giSql.AgregarParametro("@urlLogo", bytLogo);

        giSql.NoQuery("usp_con_Imagen_estructura_Ins", true);
    }

    public void fnGuardarRelUsuarioEstructura(int id_usuario, int id_estructura)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        giSql.AgregarParametro("@id_usuario", id_usuario);
        giSql.AgregarParametro("@id_estructura", id_estructura);

        giSql.NoQuery("usp_Con_Usuario_Estructura_rel_Ins", true);
    }

    /// <summary>
    /// Obtienen la lista de sucursales asignadas al usuario.
    /// </summary>
    /// <param name="psIdPadre"></param>
    /// <returns></returns>
    public DataTable fnObtenerEstructuraTodo()
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            dtAuxiliar = new DataTable();

            giSql.Query("usp_Con_Recupera_Estructura_Todo_Sel", true, ref dtAuxiliar);

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// Obtienen la lista de sucursales asignadas al usuario.
    /// </summary>
    /// <param name="psIdPadre"></param>
    /// <returns></returns>
    public DataTable fnObtenerEstructuraUsuario(int nIdUsuario)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Usuario", nIdUsuario);
        giSql.Query("usp_Con_Recupera_Estructura_Usr_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Elimina de manera lógica una sucursal
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <returns>Retorna un entero indicando las filas afectadas en la consulta</returns>
    public int fnEliminarSucursal(string psIdEstructura)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);
        giSql.AgregarParametro("nId_Estructura", psIdEstructura);

        return giSql.NoQuery("usp_Con_Sucursal_Del", true);
    }

    /// <summary>
    /// Retorna la lista de sucursales previamente borradas por el usuario
    /// </summary>
    /// <returns>DataTable con la lista de sucursales borradas</returns>
    public DataTable fnObtenerSucursalesBorradas()
    {
        giSql = clsComun.fnCrearConexion(conSucursales);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        giSql.Query("usp_Con_Sucursal_Borrados_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Cambia el estatus de una sucursal previamente borrada a Activo
    /// </summary>
    /// <param name="psIdEstructura"></param>
    /// <returns></returns>
    public int fnActualizarSucursalBorrada(string psIdEstructura)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        return giSql.NoQuery("usp_Con_Sucursal_Borrados_Upd", true);
    }

    /// <summary>
    /// Obtiene la estructura deseada
    /// </summary>
    /// <param name="sIdEstructura">ID de la estructura</param>
    /// <returns></returns>
    public DataTable fnObtenerSucursal(string sIdEstructura)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Estructura", sIdEstructura);
        giSql.Query("usp_Con_Recupera_Sucursal_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Obtienen la lista que compone la estructura del sistema.
    /// </summary>
    /// <param name="psIdPadre"></param>
    /// <returns></returns>
    public DataTable fnObtenerEstructura(string psIdPadre)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Padre", psIdPadre);
        giSql.Query("usp_Con_Recupera_Estructura_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Regresa la cantidad de facturas generadas con esa estructura.
    /// </summary>
    /// <param name="psIdEstructura"></param>
    /// <returns></returns>
    public int fnBuscarGenerados(string psIdEstructura)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        return (int)giSql.TraerEscalar("usp_Con_Busca_Comprobantes_Sel", true);

    }

    /// <summary>
    /// Obtiene domicilio de sucursal
    /// </summary>
    /// <returns></returns>
    public SqlDataReader fnObtenerDomicilioSuc(int pnSucursal, string sTienda)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        giSql.AgregarParametro("@NIDSUCURSAL", pnSucursal);
        giSql.AgregarParametro("@sTienda", sTienda);
        
        return giSql.Query("usp_Con_Domicilio_Suc_Sel", true);
    }

    /// <summary>
    /// Obtiene domicilio de sucursal en un DataTable
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerTablaDomicilioSuc(int pnSucursal)
    {
        DataTable dtDomicilio = new DataTable();
        giSql = clsComun.fnCrearConexion(conSucursales);

        giSql.AgregarParametro("@NIDSUCURSAL", pnSucursal);
        giSql.Query("usp_Con_Domicilio_Suc_Sel", true,ref dtDomicilio);
        return dtDomicilio;
    }

    /// <summary>
    /// Obtiene el id de estructura del número de tienda
    /// </summary>
    /// <param name="sNumTienda"></param>
    /// <returns></returns>
    public int fnObtenerIdEstructura(string sNumTienda)
    {
        try
        {
            string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString;
            InterfazSQL conexion = new InterfazSQL(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena));
            conexion.AgregarParametro("@sNumTienda", sNumTienda);
            int res = Convert.ToInt32(conexion.TraerEscalar("usp_con_Obtener_IdEstructura_NumTienda_sel", true));
            return res;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// Obtiene el id de estructura del ID estructura de cobro
    /// </summary>
    /// <param name="sNumTienda"></param>
    /// <returns></returns>
    public int fnObtenerIdEstructura(int nIdEstructuraCobro)
    {
        try
        {
            string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString;
            InterfazSQL conexion = new InterfazSQL(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena));
            conexion.AgregarParametro("@nIdEstructuraCobro", nIdEstructuraCobro);
            int res = Convert.ToInt32(conexion.TraerEscalar("usp_con_Obtener_IdEstructura_IdCobro_sel", true));
            return res;
        }
        catch (Exception)
        {
            return 0;
        }
    }
}

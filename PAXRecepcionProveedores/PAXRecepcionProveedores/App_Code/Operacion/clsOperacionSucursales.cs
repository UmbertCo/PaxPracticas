using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

/// <summary>
/// Clase de capa de negocio para la pantalla webOperacionSucursales
/// </summary>
public class clsOperacionSucursales
{
    private InterfazSQL giSql;
    private DataTable dtAuxiliar;
    private string conSucursales = "conRecepcionProveedores";

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
                                    string psCalle, string psNoExterior, string psNoInterior,
                                    string psColonia, string psReferencia, string psCodigoPostal, string psLocalidad,
                                    string psMunicipio, string psIdEstado, string nId_padre)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        if (!string.IsNullOrEmpty(psIdEstructura))
            giSql.AgregarParametro("nId_Estructura", psIdEstructura);

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
    /// Elimina de manera lógica una sucursal
    /// </summary>
    /// <param name="nIdSucursal">Identificador de la sucursal</param>
    /// <returns>Retorna un entero indicando las filas afectadas en la consulta</returns>
    public int fnEliminarSucursal(int nIdSucursal)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("nIdSucursal", nIdSucursal);

            return Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Sucursal_Borrar_up", true));
        }
        catch (Exception ex)
        {
            return 0;
        }
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
    public SqlDataReader fnObtenerDomicilioSuc(int pnSucursal)
    {
        giSql = clsComun.fnCrearConexion(conSucursales);

        giSql.AgregarParametro("@NIDSUCURSAL", pnSucursal);
        return giSql.Query("usp_Con_Domicilio_Suc_Sel", true);
    }
    /// <summary>
    /// Obtiene las empresas relacionadas al usuario
    /// </summary>
    /// <param name="nIdUsuario"></param>
    /// <returns></returns>
    public DataTable fnObtenerEmpresasUsuario(int nIdUsuario)
    {
        DataTable dtEmpresas = new DataTable();
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("@IdUsuario", nIdUsuario);
            giSql.Query("usp_rfp_Obtener_Empresas_Sel", true, ref dtEmpresas);
        }
        catch (Exception ex)
        {
            return null;
        }
        return dtEmpresas;
    }
    /// <summary>
    /// Guarda o edita una empresa
    /// </summary>
    /// <param name="nIdEmpresa"></param>
    /// <param name="nRazonSocial"></param>
    /// <param name="sRfc"></param>
    /// <param name="bLogo"></param>
    /// <returns></returns>
    public int fnGuardarEmpresa(int nIdUsuario, int nIdEmpresa, string nRazonSocial, string sRfc, byte[] bLogo)
    {
        int res = 0;
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            if (nIdEmpresa > 0)
            {
                giSql.AgregarParametro("@nIdEmpresa", nIdEmpresa);
            }
            giSql.AgregarParametro("@nIdUsuario", nIdUsuario);
            giSql.AgregarParametro("@sRazonSocial", nRazonSocial);
            giSql.AgregarParametro("@sRfc", sRfc);
            if (bLogo.Length > 0)
            {
                giSql.AgregarParametro("@iLogo", bLogo);
            }
            res = Convert.ToInt32(giSql.TraerEscalar("usp_cfd_Registro_Empresa_ins", true));
        }
        catch (Exception ex)
        {
            return 0;
        }
        return res;
    }
    /// <summary>
    /// Verifica si la empresa está relacionada con alguna sucursal
    /// </summary>
    /// <param name="nIdEmpresa"></param>
    /// <returns></returns>
    public bool fnVerificaEmpresaSucursal(int nIdEmpresa)
    {

        int res = 0;
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("@nIdEmpresa", nIdEmpresa);
            res = Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Relacion_Empresa_Sucursal_sel", true));
        }
        catch (Exception ex)
        {
            return true;
        }
        return res>0;
    }

    /// <summary>
    /// Elimina la empresa selecionada
    /// </summary>
    /// <param name="nIdEmpresa"></param>
    /// <returns></returns>
    public bool fnEliminarEmpresa(int nIdEmpresa)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("@nIdEmpresa", nIdEmpresa);
            giSql.TraerEscalar("usp_rfp_Empresa_Baja_up", true);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    /// <summary>
    /// Obtiene las sucursales de la empresa y el usuario seleccionado
    /// </summary>
    /// <param name="nIdEmpresa"></param>
    /// <param name="nIdUsuario"></param>
    /// <returns></returns>
    public DataTable fnObtenerSucursales(int nIdEmpresa, int nIdUsuario)
    {
        DataTable res = new DataTable();
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("@IdEmpresa", nIdEmpresa);
            giSql.AgregarParametro("@IdUsuario", nIdUsuario);
            giSql.Query("usp_rfp_Obtener_Sucursales_Sel", true, ref res);

        }
        catch (Exception ex)
        {

        }
        return res;


    }

    public DataTable fnObtenerSucursal(int nIdSucursal)
    {
        DataTable res = new DataTable();
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
            giSql.Query("usp_rfp_Obtener_Sucursal_sel", true, ref res);

        }
        catch (Exception ex)
        {

        }
        return res;
    }
    /// <summary>
    /// Registra o edita una sucursal
    /// </summary>
    /// <param name="nIdSucursal"></param>
    /// <param name="nIdEmpresa"></param>
    /// <param name="nIdUsuario"></param>
    /// <param name="sNombre"></param>
    /// <param name="nIdMunicipio"></param>
    /// <param name="sLocalidad"></param>
    /// <param name="sColonia"></param>
    /// <param name="sCalle"></param>
    /// <param name="sNoExterior"></param>
    /// <param name="sNoInterior"></param>
    /// <param name="nCodigoPostal"></param>
    /// <returns></returns>
    public int fnGuardarSucursal(int nIdSucursal, int nIdEmpresa, int nIdUsuario, string sNombre,
        int nIdMunicipio, string sLocalidad, string sColonia, string sCalle, string sNoExterior,
        string sNoInterior, string nCodigoPostal, bool bFacturaUnica, int nTipoPlantilla)
    {
        int res = 0;
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("@nIdEmpresa", nIdEmpresa);
            giSql.AgregarParametro("@nIdUsuario", nIdUsuario);
            if(nIdSucursal > 0)
                giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
            giSql.AgregarParametro("@sNombre", sNombre);
            giSql.AgregarParametro("@nIdMunicipio", nIdMunicipio);
            if(!string.IsNullOrEmpty(sLocalidad))
                giSql.AgregarParametro("@sLocalidad", sLocalidad);
            if (!string.IsNullOrEmpty(sColonia))
                giSql.AgregarParametro("@sColonia", sColonia);
            if (!string.IsNullOrEmpty(sCalle))
                giSql.AgregarParametro("@sCalle", sCalle);
            if (!string.IsNullOrEmpty(sNoExterior))
                giSql.AgregarParametro("@sNoExterior", sNoExterior);
            if (!string.IsNullOrEmpty(sNoInterior))
                giSql.AgregarParametro("@sNoInterior", sNoInterior);
            if (!string.IsNullOrEmpty(nCodigoPostal))
                giSql.AgregarParametro("@nCodigoPostal", nCodigoPostal);
            giSql.AgregarParametro("@bFacturaUnica", bFacturaUnica);
            giSql.AgregarParametro("@nTipoPlantilla", nTipoPlantilla);
            res=Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Guardar_Sucursal_ins", true));
        }
        catch (Exception ex)
        {
            return 0;
        }
        return res;
    }

    /// <summary>
    /// Guarda una lista de correos de la sucursal
    /// </summary>
    /// <param name="licCorreos"></param>
    /// <param name="nIdSucursal"></param>
    /// <returns></returns>
    public bool fnGuardarCorreos(ListItemCollection licCorreos, int nIdSucursal)
    {
        try
        {
            foreach (ListItem liCorreo in licCorreos)
            {
                giSql = clsComun.fnCrearConexion(conSucursales);
                giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
                giSql.AgregarParametro("@sCorreo", liCorreo.Text);
                giSql.Query("usp_rfp_Guardar_Correo_ins", true);
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    /// <summary>
    /// Borra todos los correos de una sucursal
    /// </summary>
    /// <param name="nIdSucursal"></param>
    public void fnBorrarCorreos(int nIdSucursal)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
            giSql.Query("usp_rfp_Borrar_Correos_up", true);

        }
        catch (Exception ex)
        {

        }
    }
    /// <summary>
    /// Obtiene los correos de una sucursal
    /// </summary>
    /// <param name="nIdSucursal"></param>
    /// <returns></returns>
    public DataTable fnObtenerCorreosSucursal(int nIdSucursal)
    {
        DataTable res = new DataTable();
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
            giSql.Query("usp_rfp_Obtener_Correos_sel", true, ref res);

        }
        catch (Exception ex)
        {

        }
        return res;
    }
    /// <summary>
    /// Obtiene todas las sucursales con sus empresas
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerSucursalesEmpresas()
    {
        DataTable dtRes = new DataTable();
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.Query("usp_rfp_Obtener_Sucursales_Empresa_sel", true, ref dtRes);
        }
        catch (Exception ex)
        {

        }
        return dtRes;
    }

    public DataTable fnObtenerSucursalesUsuario(int nIdUsuario)
    {
        DataTable dtRes = new DataTable();
        try
        {
            giSql = clsComun.fnCrearConexion(conSucursales);
            giSql.AgregarParametro("nIdUsuario",nIdUsuario);
            giSql.Query("usp_rfp_Obtener_Sucursales_Usuario_sel", true, ref dtRes);
        }
        catch (Exception ex)
        {

        }
        return dtRes;
    }

    /// <summary>
    /// Recupera la lista de las sucurslaes.
    /// </summary>
    /// <param name="nId_usuario">id del usuario</param>
    /// <returns>recupera la lista de las sucursales.</returns>
    public static DataTable LlenarDropSucursales(int nId_usuario)
    {
        Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conRecepcionProveedores");
        DataTable gdtAuxiliar = new DataTable("Sucursales");
        try
        {
            giSql.AgregarParametro("nId_Usuario", nId_usuario);
            giSql.Query("usp_Usuario_Sucursal_Sel", true, ref gdtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return gdtAuxiliar;
    }
    /// <summary>
    /// Recupera la lista de empresas o sucursales.
    /// </summary>
    /// <param name="nIdEmpresa">id de la empresa padre</param>
    /// <returns>Lista con las empresas o sucursales</returns>
    public static DataTable fnObtenerSucursales(int nIdEmpresa = 0)
    {
        Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conRecepcionProveedores");
        DataTable gdtAuxiliar = new DataTable("Sucursales");
        try
        {
            if (nIdEmpresa > 0)
                giSql.AgregarParametro("nId_Empresa", nIdEmpresa);
            giSql.Query("usp_Usuario_Sucursal_Sel", true, ref gdtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return gdtAuxiliar;
    }
    /// <summary>
    /// Obtiene el catálogo de tipos de plantillas
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerTiposPlantilla()
    {
        Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conRecepcionProveedores");
        DataTable gdtAuxiliar = new DataTable();
        try
        {

            giSql.Query("usp_rfp_Obtener_TipoPlantilla_sel", true, ref gdtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return gdtAuxiliar;
    }
    /// <summary>
    /// Obtiene el ID del tipo de plantilla
    /// </summary>
    /// <param name="nIdSucursal"></param>
    /// <returns></returns>
    public int fnObtenerIdTipoPlantilla(int nIdSucursal)
    {
        int res = 0;
        Utilerias.SQL.InterfazSQL giSql = clsComun.fnCrearConexion("conRecepcionProveedores");
        try
        {
            giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
            res = Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Obtener_TipoPlantilla_Sucursal_sel", true));
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return res;
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Clase con la capa de negocios para la pantalla webOperacionClientes
/// </summary>
public class clsOperacionClientes
{
	private InterfazSQL giSql;
	private DataTable dtAuxiliar;
    private string conClientes = "conConfiguracion";

	#region RFC

    /// <summary>
    /// Inserta o actualiza a un receptor en la base de datos.
    /// Si el parámetro psIdRfc es cadena vacía entonces es inserción de lo contrario es actualización
    /// </summary>
    /// <param name="psIdRfc">Identificador del receptor</param>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <param name="psRfc">RFC del receptor</param>
    /// <param name="psRazonSocial">razón social del receptor</param>
    /// <returns></returns>
	public int fnGuardarReceptor(string psIdRfc, string psIdEstructura, string psRfc, string psRazonSocial)
	{
		giSql = clsComun.fnCrearConexion(conClientes);

		if (!string.IsNullOrEmpty(psIdRfc))
			giSql.AgregarParametro("nId_Receptor", psIdRfc);
		giSql.AgregarParametro("nId_Estructura", psIdEstructura);
		giSql.AgregarParametro("sRfc", psRfc.Trim());
		giSql.AgregarParametro("sRazon_Social", psRazonSocial);

		return giSql.NoQuery("usp_Cli_Receptor", true);
	}

    /// <summary>
    /// Busca y retorna el conjunto de datos de un receptor en especifico
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns>Retorna un SqlDataReader con los datos del receptor</returns>
	public SqlDataReader fnEditarReceptor(string psIdReceptor)
	{
        giSql = clsComun.fnCrearConexion(conClientes);

		giSql.AgregarParametro("nId_Receptor", psIdReceptor);
		return giSql.Query("usp_Cli_Receptor_Sel", true);
	}

    /// <summary>
    /// Elimina de manera lógica a un receptor
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns></returns>
	public int fnEliminarReceptor(string psIdReceptor)
	{
        giSql = clsComun.fnCrearConexion(conClientes);

		giSql.AgregarParametro("nId_Receptor", psIdReceptor);
		return giSql.NoQuery("usp_Cli_Receptor_Del", true);
	}

	/// <summary>
	/// Trae la lista de receptores activos del usuario
    /// La relación es usuario-estructura-receptor
	/// </summary>
    /// /// <returns>DataTable con la lista de todos los receptores</returns>
	public DataTable fnLlenarReceptores(int psIdEstructura)
	{
		giSql = clsComun.fnCrearConexion(conClientes);
		dtAuxiliar = new DataTable();

		//giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        giSql.Query("usp_Cli_Receptores_Sel", true, ref dtAuxiliar);

		return dtAuxiliar;
	}

    /// <summary>
    /// Trae la lista de receptores activos del usuario
    /// La relación es usuario-estructura-receptor
    /// </summary>
    /// <param name="psIdEstructura">Estructura para la cual se quieren obtener los receptores</param>
    /// <returns>DataTable con la lista de receptores adecuados</returns>
    public DataTable fnLlenarReceptores(string psIdEstructura)
    {
        giSql = clsComun.fnCrearConexion(conClientes);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        giSql.Query("usp_Cli_Receptores_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }


    /// <summary>
    /// Inserta o actualiza a un receptor en la base de datos.
    /// Si el parámetro psIdRfc es cadena vacía entonces es inserción de lo contrario es actualización
    /// </summary>
    /// <param name="psIdRfc">Identificador del receptor</param>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <param name="psRfc">RFC del receptor</param>
    /// <param name="psRazonSocial">razón social del receptor</param>
    /// <returns></returns>
    public int fnGuardarReceptorCobro(string psIdRfc, string psIdEstructura, string psRfc, string psRazonSocial)
    {
        giSql = clsComun.fnCrearConexion(conClientes);

        if (!string.IsNullOrEmpty(psIdRfc))
        giSql.AgregarParametro("nId_Receptor", psIdRfc);
        giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        giSql.AgregarParametro("sRfc", psRfc.Trim());
        giSql.AgregarParametro("sRazon_Social", psRazonSocial);

        return Convert.ToInt32(giSql.TraerEscalar("usp_Cli_Receptor_cobro", true));
    }

	#endregion

	#region Sucursal

    /// <summary>
    /// Verifica que el receptor pertenesca al usuario en sesión
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns>Booleano indicando si el receptor pertenece al usuario</returns>
    public bool fnVerificarPropiedad(string psIdReceptor)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");

        giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        giSql.AgregarParametro("nId_Receptor", psIdReceptor);
        return Convert.ToBoolean(giSql.TraerEscalar("usp_Cli_Verificar_Receptor_Cobro_Sel", true));
    }

    /// <summary>
    /// Inserta o actualiza un sucursal de receptor en la base de datos.
    /// Si el parámetro psIdEstructura es cadena vacía entonces es inserción de lo
    /// contrario es actualización.
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <param name="psSucursal">Nombre de la sucursal</param>
    /// <param name="psCalle">Nombre de la calle</param>
    /// <param name="psNoExterior">El número exterior del lugar</param>
    /// <param name="psNoInterior">El número interior del lugar</param>
    /// <param name="psColonia">El nombre de la colonia</param>
    /// <param name="psCodigoPostal">El numero de código postal del área</param>
    /// <param name="psLocalidad">El nombre de la localidad</param>
    /// <param name="psMunicipio">El nombre del municipio</param>
    /// <param name="psEstado">El nombre del estado</param>
    /// <param name="psPais">El nombre dle país</param>
    /// <returns>Retorna un entero indicando las filas afectadas en la consulta</returns>
	public int fnGuardarSucursal(string psIdEstructura, string psIdReceptor, string psSucursal,
                                    string psCalle, string psNoExterior, string psNoInterior,
                                    string psColonia, string psCodigoPostal, string psLocalidad,
									string psMunicipio, string psEstado, string psPais)
	{
		giSql = clsComun.fnCrearConexion(conClientes);

		if (!string.IsNullOrEmpty(psIdEstructura))
			giSql.AgregarParametro("nId_Estructura", psIdEstructura);

		giSql.AgregarParametro("@nId_Receptor", psIdReceptor);
		giSql.AgregarParametro("@sSucursal", psSucursal);
		giSql.AgregarParametro("@sCalle", psCalle);
		if (!string.IsNullOrEmpty(psNoExterior))
			giSql.AgregarParametro("@sNumero_Exterior", psNoExterior);
		if (!string.IsNullOrEmpty(psNoInterior))
			giSql.AgregarParametro("@sNumero_Interior", psNoInterior);
		if (!string.IsNullOrEmpty(psColonia))
			giSql.AgregarParametro("@sColonia", psColonia);
        if(!string.IsNullOrEmpty(psCodigoPostal))
            giSql.AgregarParametro("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
		if (!string.IsNullOrEmpty(psLocalidad))
			giSql.AgregarParametro("@sLocalidad", psLocalidad);
		giSql.AgregarParametro("@sMunicipio", psMunicipio);
		giSql.AgregarParametro("@sEstado", psEstado);
		giSql.AgregarParametro("@sPais", psPais);

        return giSql.NoQuery("usp_Cli_Receptor_Sucursal", true);
	}

    /// <summary>
    /// Elimina de manera lógica una sucursal de receptor
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal</param>
    /// <returns>Retorna un entero indicando las filas afectadas por la consulta</returns>
    public int fnEliminarSucursalReceptor(string psIdEstructura)
    {
        giSql = clsComun.fnCrearConexion(conClientes);
        giSql.AgregarParametro("nId_Estructura", psIdEstructura);

        return giSql.NoQuery("usp_Cli_Receptor_Sucursal_Del", true);
    }

    /// <summary>
    /// Retorna una lista de sucursales pertenecientes al receptor especificado
    /// </summary>
    /// <param name="psIdReceptor">Identificador del receptor</param>
    /// <returns>Retorna un DataTable con las sucursales encontradas</returns>
    public DataTable fnLlenarGridSucursalesReceptores(string psIdReceptor)
    {
        giSql = clsComun.fnCrearConexion(conClientes);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("@nId_Receptor", psIdReceptor);

        giSql.Query("usp_Cli_Receptor_Sucursales_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

	#endregion

}
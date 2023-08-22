using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Clase que contiene los metódos para el manejo de los empleados
/// </summary>
public class clsOperacionEmpleados
{
    private string conCuenta = "conControl";

	public clsOperacionEmpleados()
	{
		
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pnId_Empleado"></param>
    /// <param name="psNombre"></param>
    /// <param name="psCorreo"></param>
    /// <param name="psRegistroPatronal"></param>
    /// <param name="pnRiesgoPuesto"></param>
    /// <param name="psNumeroEmpleado"></param>
    /// <param name="psCURP"></param>
    /// <param name="psRFC"></param>
    /// <param name="pnBanco"></param>
    /// <param name="psCLABE"></param>
    /// <param name="pnTipoRegimen"></param>
    /// <param name="psNumeroSeguridadSocial"></param>
    /// <param name="psDepartamento"></param>
    /// <param name="psPuesto"></param>
    /// <param name="psTipoContrato"></param>
    /// <param name="psTipoJornada"></param>
    /// <param name="psPeriodicidadPago"></param>
    /// <param name="pdFechaInicialRelacionLaboral"></param>
    /// <param name="pdSalarioBase"></param>
    /// <param name="pdSalarioDiarioIntegrado"></param>
    /// <param name="pnId_Estructura"></param>
    /// <param name="psEstatus"></param>
    /// <returns></returns>
    public bool fnActualizarEmpleado(int pnId_Empleado, string psNombre, string psCorreo, string psRegistroPatronal, int pnRiesgoPuesto, 
                                    string psNumeroEmpleado, string psCURP, string psRFC, int pnBanco, string psCLABE, int pnTipoRegimen, 
                                    string psNumeroSeguridadSocial, string psDepartamento, string psPuesto, string psTipoContrato, 
                                    string psTipoJornada, DateTime pdFechaInicialRelacionLaboral, 
                                    double pdSalarioBase, double pdSalarioDiarioIntegrado, int pnId_Estructura, char psEstatus)
    {
        bool bResultado = false;
        int nFilasAfectadas = 0;
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);

                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_empleado_upd";

                    scoComando.Parameters.AddWithValue("nidEmpleado", pnId_Empleado);
                    scoComando.Parameters.AddWithValue("sNombre", psNombre);
                    scoComando.Parameters.AddWithValue("sCorreo", psCorreo);

                    if (!psRegistroPatronal.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sRegistroPatronal", psRegistroPatronal);

                    if (!pnRiesgoPuesto.Equals(0))
                        scoComando.Parameters.AddWithValue("nRiesgoPuesto", pnRiesgoPuesto);

                    if (!psNumeroEmpleado.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sNumEmpleado", psNumeroEmpleado);

                    scoComando.Parameters.AddWithValue("sCURP", psCURP);
                    scoComando.Parameters.AddWithValue("sRFC", psRFC);

                    if (!pnBanco.Equals(0))
                        scoComando.Parameters.AddWithValue("nBanco", pnBanco);

                    if (!psCLABE.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sCLABE", psCLABE);

                    scoComando.Parameters.AddWithValue("nTipoRegimen", pnTipoRegimen);

                    if (!psNumeroSeguridadSocial.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sNumSeguridadSocial", psNumeroSeguridadSocial);

                    if (!psDepartamento.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sDepartamento", psDepartamento);

                    if (!psPuesto.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sPuesto", psPuesto);

                    if (!psTipoContrato.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sTipoContrato", psTipoContrato);

                    if (!psTipoJornada.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sTipoJornada", psTipoJornada);

                    if (!pdFechaInicialRelacionLaboral.Equals(Convert.ToDateTime("01-01-1900")))
                        scoComando.Parameters.AddWithValue("dFechaInicioRelLaboral", pdFechaInicialRelacionLaboral);

                    if (!pdSalarioBase.Equals(0))
                        scoComando.Parameters.AddWithValue("nSalarioBaseCotApor", pdSalarioBase);

                    if (!pdSalarioDiarioIntegrado.Equals(0))
                        scoComando.Parameters.AddWithValue("nSalarioDiarioIntegrado", pdSalarioDiarioIntegrado);

                    scoComando.Parameters.AddWithValue("nid_estructura", pnId_Estructura);
                    scoComando.Parameters.AddWithValue("sEstatus", psEstatus);

                    nFilasAfectadas = scoComando.ExecuteNonQuery();

                    if(nFilasAfectadas.Equals(0))
                    {
                        throw new Exception("No se actualizaron los datos.");
                    }
                }

                bResultado = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar al empleado. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return bResultado;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="psNombre"></param>
    /// <param name="psCorreo"></param>
    /// <param name="psRegistroPatronal"></param>
    /// <param name="pnRiesgoPuesto"></param>
    /// <param name="psNumeroEmpleado"></param>
    /// <param name="psCURP"></param>
    /// <param name="psRFC"></param>
    /// <param name="pnBanco"></param>
    /// <param name="psCLABE"></param>
    /// <param name="pnTipoRegimen"></param>
    /// <param name="psNumeroSeguridadSocial"></param>
    /// <param name="psDepartamento"></param>
    /// <param name="psPuesto"></param>
    /// <param name="psTipoContrato"></param>
    /// <param name="psTipoJornada"></param>
    /// <param name="psPeriodicidadPago"></param>
    /// <param name="pdFechaInicialRelacionLaboral"></param>
    /// <param name="pdSalarioBase"></param>
    /// <param name="pdSalarioDiarioIntegrado"></param>
    /// <param name="pnId_Estructura"></param>
    /// <param name="psEstatus"></param>
    /// <returns></returns>
    public bool fnIngresarEmpleado(string psNombre, string psCorreo, string psRegistroPatronal, int pnRiesgoPuesto,
                                    string psNumeroEmpleado, string psCURP, string psRFC, int pnBanco, string psCLABE, int pnTipoRegimen,
                                    string psNumeroSeguridadSocial, string psDepartamento, string psPuesto, string psTipoContrato,
                                    string psTipoJornada, DateTime pdFechaInicialRelacionLaboral,
                                    double pdSalarioBase, double pdSalarioDiarioIntegrado, int pnId_Estructura, char psEstatus)
    {
        bool bResultado = false;
        int nFilasAfectadas = 0;
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);

                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_empleado_ins";

                    scoComando.Parameters.AddWithValue("sNombre", psNombre);
                    scoComando.Parameters.AddWithValue("sCorreo", psCorreo);

                    if (!psRegistroPatronal.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sRegistroPatronal", psRegistroPatronal);

                    if (!pnRiesgoPuesto.Equals(0))
                        scoComando.Parameters.AddWithValue("nRiesgoPuesto", pnRiesgoPuesto);

                    if (!psNumeroEmpleado.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sNumEmpleado", psNumeroEmpleado);

                    scoComando.Parameters.AddWithValue("sCURP", psCURP);
                    scoComando.Parameters.AddWithValue("sRFC", psRFC);

                    if (!pnBanco.Equals(0))
                        scoComando.Parameters.AddWithValue("nBanco", pnBanco);

                    if (!psCLABE.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sCLABE", psCLABE);

                    scoComando.Parameters.AddWithValue("nTipoRegimen", pnTipoRegimen);

                    if (!psNumeroSeguridadSocial.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sNumSeguridadSocial", psNumeroSeguridadSocial);

                    if (!psDepartamento.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sDepartamento", psDepartamento);

                    if (!psPuesto.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sPuesto", psPuesto);

                    if (!psTipoContrato.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sTipoContrato", psTipoContrato);

                    if (!psTipoJornada.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sTipoJornada", psTipoJornada);

                    if (!pdFechaInicialRelacionLaboral.Equals(Convert.ToDateTime("01-01-1900")))
                        scoComando.Parameters.AddWithValue("dFechaInicioRelLaboral", pdFechaInicialRelacionLaboral);

                    if (!pdSalarioBase.Equals(0))
                        scoComando.Parameters.AddWithValue("nSalarioBaseCotApor", pdSalarioBase);

                    if (!pdSalarioDiarioIntegrado.Equals(0))
                        scoComando.Parameters.AddWithValue("nSalarioDiarioIntegrado", pdSalarioDiarioIntegrado);

                    scoComando.Parameters.AddWithValue("nid_estructura", pnId_Estructura);
                    scoComando.Parameters.AddWithValue("sEstatus", psEstatus);

                    nFilasAfectadas = Convert.ToInt32(scoComando.ExecuteScalar());

                    if (nFilasAfectadas.Equals(0))
                    {
                        throw new Exception("No se insertaron los datos.");
                    }
                }

                bResultado = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ingresar al empleado. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return bResultado;
    }

    /// <summary>
    /// Función que se encarga de buscar a los empleados mediante filtros de numero de empleado, RFC y Nombre.
    /// </summary>
    /// <param name="psNumeroEmpleado">Numero de empleado</param>
    /// <param name="psRFC">RFC</param>
    /// <param name="psNombre">Nombre del empleado</param>
    /// <param name="pnId_Estructura">ID de la estructura</param>
    /// <param name="pnPagina">Página a mostrar</param>
    /// <param name="pnNumeroRegistros">Número de registros a mostrar</param>
    /// <returns></returns>
    public DataTable fnBuscarEmpleados(string psNumeroEmpleado, string psRFC, string psNombre, int pnId_Estructura, int pnPagina, int pnNumeroRegistros)
    {
        DataTable dtResultado = new DataTable();

        bool berror = false;
        string sMensajeError = string.Empty;

        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_empleado_sel";

                    if (!psNumeroEmpleado.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("snum_empleado", psNumeroEmpleado);

                    if (!psNombre .Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sNombre", psNombre);

                    if (!psRFC.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sRFC", psRFC);

                    scoComando.Parameters.AddWithValue("nid_estructura", pnId_Estructura);
                    scoComando.Parameters.AddWithValue("nMAXRegistros", pnNumeroRegistros);
                    scoComando.Parameters.AddWithValue("nNumPagina", pnPagina);
                                      
                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                berror = true;
                sMensajeError = ex.Message;
            }
            finally
            {
                scConexion.Close();

                if (berror)
                {
                    throw new Exception(sMensajeError);
                }
            }
        }
        return dtResultado;
    }
    
    /// <summary>
    /// Función que se encarga de buscar a los empleados mediante filtros de numero de empleado, RFC y Nombre.
    /// </summary>
    /// <param name="psNumeroEmpleado">Numero de empleado</param>
    /// <param name="psRFC">RFC</param>
    /// <param name="psNombre">Nombre del empleado</param>
    /// <param name="pnId_Estructura">ID de la estructura</param>
    /// <returns></returns>
    public DataTable fnBuscarEmpleados(string psNumeroEmpleado, string psRFC, string psNombre, int pnId_Estructura)
    {
        DataTable dtResultado = new DataTable();

        bool berror = false;
        string sMensajeError = string.Empty;

        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_empleado_sel_cats";

                    if (!psNumeroEmpleado.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("snum_empleado", psNumeroEmpleado);

                    if (!psNombre.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sNombre", psNombre);

                    if (!psRFC.Equals(string.Empty))
                        scoComando.Parameters.AddWithValue("sRFC", psRFC);

                    scoComando.Parameters.AddWithValue("nid_estructura", pnId_Estructura);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                berror = true;
                sMensajeError = ex.Message;
            }
            finally
            {
                scConexion.Close();

                if (berror)
                {
                    throw new Exception(sMensajeError);
                }
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que se encarga de obtener los datos de un empleado buscando por campo indentity
    /// </summary>
    /// <param name="pnId_Empleado">ID del empleado</param>
    /// <returns></returns>
    public DataTable fnExisteEmpleado(int pnId_Empleado)
    {
        DataTable dtResultado = new DataTable();

        bool berror = false;
        string sMensajeError = string.Empty;

        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = "Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n";
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_Empleado_Existe_sel";

                    scoComando.Parameters.AddWithValue("nId_Empleado", pnId_Empleado);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                berror = true;
                sMensajeError = ex.Message;
            }
            finally
            {
                scConexion.Close();

                if (berror)
                {
                    throw new Exception(sMensajeError);
                }
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que se encarga de guardar los datos del periodo de la sucursal.
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <param name="pnId_Periodo">ID del Periodo</param>
    /// <returns></returns>
    public bool fnIngresarSucursalPeriodo(int pnId_Estructura, int pnId_Periodo)
    {
        bool bResultado = false;
        int nFilasAfectadas = 0;
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);

                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_Tipos_Periodos_Estructura_Ins";

                    scoComando.Parameters.AddWithValue("nIdTipoPeriodo", pnId_Periodo);
                    scoComando.Parameters.AddWithValue("nId_Estructura", pnId_Estructura);

                    nFilasAfectadas = Convert.ToInt32(scoComando.ExecuteScalar());

                    if (nFilasAfectadas.Equals(0))
                    {
                        throw new Exception("No se insertaron los datos.");
                    }
                }

                bResultado = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ingresar el periodo de una estructura. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return bResultado;
    }

    /// <summary>
    /// Función que se encarga de ingresar el periodo de una sucursal
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <param name="pnId_Periodo">ID del Periodo</param>
    /// <returns></returns>
    public bool fnActualizarSucursalPeriodo(int pnId_Estructura, int pnId_Periodo)
    {
        bool bResultado = false;
        int nFilasAfectadas = 0;
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString);

                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_nom_Tipos_Periodos_Estructura_Upd_Por_Id_Estructura";

                    scoComando.Parameters.AddWithValue("nIdTipoPeriodo", pnId_Periodo);
                    scoComando.Parameters.AddWithValue("nId_Estructura", pnId_Estructura);

                    nFilasAfectadas = scoComando.ExecuteNonQuery();

                    if (nFilasAfectadas.Equals(0))
                    {
                        throw new Exception("No se insertaron los datos.");
                    }
                }

                bResultado = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ingresar el periodo de una estructura. " + ex.Message);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return bResultado;
    }
}
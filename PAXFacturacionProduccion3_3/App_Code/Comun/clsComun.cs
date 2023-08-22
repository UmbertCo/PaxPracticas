using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

/// <summary>
/// Clase encargada de proporcionar funciones de ayuda a todas las demás clases.
/// </summary>
public class clsComun
{

    /// <summary>
    /// Metodo que se encarga de separar una consulta por la moneda 
    /// </summary>
    /// <param name="pdtTabla">Tabla de todos los registros</param>
    /// <param name="psNombreCampoMoneda">Nombre de la columna donde esta la moneda</param>
    /// <returns>Tabla con tablas de registros separados por moneda</returns>
    public static DataTable fnSepararMonedas(DataTable pdtTabla, String psNombreCampoMoneda) 
    {

        DataRow[] drAux;
        DataTable tblMXN=null;
        DataTable tblUSD=null;
        DataTable tblEUR=null;
        
        drAux = pdtTabla.Select(psNombreCampoMoneda + "= 'MXN'");
        if(drAux.Length >0)
        tblMXN=    drAux.CopyToDataTable();

        drAux = pdtTabla.Select(psNombreCampoMoneda + "= 'USD'");
        if(drAux.Length >0)
         tblUSD= drAux.CopyToDataTable();

        drAux = pdtTabla.Select(psNombreCampoMoneda + "= 'XEU' or " + psNombreCampoMoneda + " ='EUR'");
        if(drAux.Length >0)
         tblEUR= drAux.CopyToDataTable();


        DataTable tblRes = new DataTable();

        tblRes.Columns.Add("tabla",typeof(DataTable));
        tblRes.Columns.Add("mostrar", typeof(bool));
        tblRes.Columns.Add("moneda", typeof(String));

        if(tblMXN !=null)
        tblRes.Rows.Add(tblMXN, false, "MXN");
        if (tblUSD != null)
            tblRes.Rows.Add(tblUSD, false, "USD");
        if (tblEUR != null)
            tblRes.Rows.Add(tblEUR, false, "EUR");

        tblRes.Rows[0]["mostrar"] = true;

        return tblRes;
    
    }

    /// <summary>
    /// Inserta o actualiza un parametro.
    /// Si el parámetro psIdParametro es cadena vacía entonces es inserción de lo
    /// contrario es actualización.
    /// </summary>
    public int fnGuardarParametro(string psIdParametros, string psParametro, string psValor, string psEstatus)
    {
        int nResultado = 0;
        
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Parametros", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!string.IsNullOrEmpty(psIdParametros))
                        cmd.Parameters.Add(new SqlParameter("nId_Parametro", psIdParametros));
                    if (!string.IsNullOrEmpty(psParametro))
                        cmd.Parameters.Add(new SqlParameter("sParametro", psParametro));
                    if (!string.IsNullOrEmpty(psValor))
                        cmd.Parameters.Add(new SqlParameter("sValor", psValor));
                    if (!string.IsNullOrEmpty(psEstatus))
                        cmd.Parameters.Add(new SqlParameter("sEstatus", psEstatus));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnGuardarParametro", "clsComun");
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Inserta un nuevo registro de acuse del PAC
    /// </summary>
    /// /// <param name="p_idcte">identificador del cliente</param>
    /// /// <param name="p_UUID">UUID del comprobante</param>
    ///  <param name="sFecha">fecha de envio</param>
    ///  <param name="pCorreoElectronico">correo electronico</param>
    ///  <param name="p_tipoAcuse">tipo de acuse</param>
    /// <returns></returns>
    public static Int32 fnInsertaAcusePAC(int pIdcte, string pUUID, string pAcuse, DateTime sFecha, string pIdCodigo, string pSoap, string origen, string pCorreoElectronico)
    {
        Int32 nResultado = 0;

        using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(System.Configuration.ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                conexion.Open();
                using (SqlCommand command = new SqlCommand("usp_Cfd_Acuse_PAC_Ins", conexion))
                {
                    command.CommandTimeout = 20;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nid_Contribuyente", pIdcte);
                    command.Parameters.AddWithValue("@sUUID", pUUID);
                    command.Parameters.AddWithValue("@sAcuse", pAcuse);
                    command.Parameters.AddWithValue("@sFecha", sFecha);
                    command.Parameters.AddWithValue("@nid_codigo", pIdCodigo);
                    command.Parameters.AddWithValue("@sSoap", pSoap);
                    command.Parameters.AddWithValue("@sOrigen", origen);
                    command.Parameters.AddWithValue("@sMail", pCorreoElectronico);
                    nResultado = Convert.ToInt32(command.ExecuteNonQuery());


                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnInsertaAcusePAC", "clsComun");
            }
            finally
            {
                conexion.Close();
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Inserta un nuevo registro de acuse del emitido por el SAT
    /// </summary>
    /// /// <param name="p_UUID">UUID del comprobante</param>
    ///  <param name="sFecha">fecha de envio</param>
    ///  <param name="p_Acuse">xml con el acuse que emitio el SAT</param>
    ///  <param name="p_idcodigo">tipo de codigo que se genero</param>
    /// <returns></returns>
    public static Int32 fnInsertaAcuseSAT(string sIdContribuyente, string pUUID, string pAcuse, DateTime sFecha, string pidcodigo, string pSoap, string origen)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Acuse_SAT_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("nid_Contribuyente", sIdContribuyente));
                    cmd.Parameters.Add(new SqlParameter("sUUID", pUUID));
                    cmd.Parameters.Add(new SqlParameter("sAcuse", pAcuse));
                    cmd.Parameters.Add(new SqlParameter("sFecha", sFecha));
                    cmd.Parameters.Add(new SqlParameter("nid_codigo", pidcodigo));
                    cmd.Parameters.Add(new SqlParameter("sSoap", pSoap));
                    cmd.Parameters.Add(new SqlParameter("sOrigen", origen));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnInsertaAcuseSAT", "clsComun");
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Revisa que el valor sea doble.
    /// </summary>
    /// <param name="Expression">valor a evaluar</param>
    /// <returns>retorna si es verdadero.</returns>
    public static bool fnIsDouble(object Expression)
    {
        bool isNum;
        double retNum;

        isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
    }

    /// <summary>
    /// Revisa que el valor sea entero.
    /// </summary>
    /// <param name="Expression">valor a evaluar</param>
    /// <returns>retorna si es verdadero.</returns>
    public static bool fnIsInt(object Expression)
    {
        bool isNum;
        int retNum;

        isNum = int.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
    }

    /// <summary>
    /// Devuelve el catalogo de paises
    /// </summary>
    /// <returns>DataTable con los paises disponibles</returns>
    public static DataTable fnLlenarDropPaises()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Obtener_cce_Pais_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarDropPaises", "clsComun");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Devuelve el catalogo de sucursales para este usuario
    /// </summary>
    /// <param name="pbDrop">Indica si también se quiere devolver la matriz</param>
    /// <returns></returns>
    public static DataTable fnLlenarDropSucursales(bool pbMatriz)
    {
        DataTable dtResultado = new DataTable("Sucursales");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con33_Sucursal_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", fnUsuarioEnSesion().nIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("bDrop", pbMatriz));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarDropSucursales", "clsComun");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Devuelve una parte del catalogo de estados, filtrando por el país indicado
    /// </summary>
    /// <param name="pnIdPais">ID del país para el cual se quiere recuperar sus estados</param>
    /// <returns>DataTable con los estados filtrados por país</returns>
    public static DataTable fnLlenarDropEstados(int pnIdPais)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Con_Obtener_cce_Estado_Sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nPais", pnIdPais);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarDropEstados", "clsComun");
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Utileria para mostrar mensaje al estilo JQuery
    /// </summary>
    /// <param name="pPagina">La página que llama al mensaje</param>
    /// <param name="psMensaje">El mensaje a mostrar</param>
    public static void fnMostrarMensaje(Page pPagina, string psMensaje)
    {
        fnMostrarMensaje(pPagina, psMensaje, Resources.resCorpusCFDIEs.varContribuyente);
    }

    /// <summary>
    /// Utileria para mostrar mensaje al estilo JQuery
    /// </summary>
    /// <param name="pPagina">La página que llama al mensaje</param>
    /// <param name="psMensaje">El mensaje a mostrar</param>
    /// <param name="psTitulo">El titulo que tendrá el modal</param>
    public static void fnMostrarMensaje(Page pPagina, string psMensaje, string psTitulo)
    {
        ScriptManager.RegisterStartupScript(pPagina, typeof(UserControl), "Mensaje" + DateTime.Now.Millisecond.ToString(),
            "jAlert('" + psMensaje + "', '" + psTitulo + "');", true);
    }

    /// <summary>
    /// Utileria para mostrar mensaje al estilo JQuery
    /// </summary>
    /// <param name="pPagina">Control de usaurio que llama al mensaje</param>
    /// <param name="psMensaje">El mensaje a mostrar</param>
    /// <param name="psTitulo">El titulo que tendrá el modal</param>
    public static void fnMostrarMensaje(UserControl pPagina, string psMensaje, string psTitulo)
    {
        ScriptManager.RegisterStartupScript(pPagina, typeof(UserControl), "Mensaje" + DateTime.Now.Millisecond.ToString(),
            "jAlert('" + psMensaje + "', '" + psTitulo + "');", true);
    }

    /// <summary>
    /// Crea una nueva entrada para las pistas de auditoría para el usuario en sesión
    /// en el momento exacto de la llamada del método y con la descipción de acción especificada
    /// </summary>
    /// <param name="args">Argumentos para la descripción de la acción</param>
    public static void fnNuevaPistaAuditoria(params object[] args)
    {
        try
        {
            int id_usuario = fnUsuarioEnSesion().nIdUsuario;
            DateTime fecha = DateTime.Now;
            StringBuilder accion = new StringBuilder();

            foreach (object arg in args)
            {
                accion.Append(arg);
                accion.Append(" | ");
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnNuevaPistaAuditoria", args[0].ToString());
        }
    }
   
    /// <summary>
    /// Obtener Paramentro Correo Obtiene el valor del parametro para el correo.
    /// </summary>
    /// <param name="psParametro">Nombre del parametros a buscar</param>
    /// <returns>Cadena con le valor del parámetro</returns>
    public static string fnObtenerParametro(string psParametro)
    {
        string sRetorno = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_BuscarParametro_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sParametro", psParametro));
                    con.Open();
                    sRetorno = (string)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "ObtenerParametro", "clsComun");
            }
        }
        return sRetorno;
    }

    /// <summary>
    /// Devuelve el catalogo de parametros
    /// </summary>
    /// <param name="pbDrop">Indica si también se quiere devolver la matriz</param>
    /// <returns></returns>
    public static DataTable fnLlenarParametros()
    {
        DataTable dtResultado = new DataTable("Parametros");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Parametros_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "LlenarParametros", "clsComun");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Devuelve el catalogo de usuarios activos
    /// </summary>
    /// <returns></returns>
    public static DataTable fnObtenerUsuarios()
    {
        DataTable dtResultado = new DataTable("Parametros");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RecuperaUsu_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerUsuarios", "clsComun");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Devuelve la consulta de pistas
    /// </summary>
    /// <param name="psUsuario">Usuario del sistema</param>
    /// <param name="psAccion">Accion a buscar</param>
    /// <param name="psAccion2">Accion a buscar 2</param>
    /// <param name="psAccion3">Accion a buscar 3</param>
    /// <param name="psFechaReg">Fecha registros</param>
    /// <param name="psFechaReg2">Fecha registros</param>
    /// <param name="psPagina"></param>
    public static DataTable fnObtenerPistas(string psUsuario, string psAccion,
                                            string psAccion2, string psAccion3, 
                                            string psFechaReg, string psFechaReg2, string psPagina)
    {
        DataTable dtResultado = new DataTable("Pistas");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_PistasFiltros_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(psUsuario))
                        cmd.Parameters.Add(new SqlParameter("psUsuario", psUsuario));
                    if (!string.IsNullOrEmpty(psAccion))
                        cmd.Parameters.Add(new SqlParameter("psAccion", psAccion));
                    if (!string.IsNullOrEmpty(psAccion2))
                        cmd.Parameters.Add(new SqlParameter("psAccion2", psAccion2));
                    if (!string.IsNullOrEmpty(psAccion3))
                        cmd.Parameters.Add(new SqlParameter("psAccion3", psAccion3));
                    if (!string.IsNullOrEmpty(psFechaReg))
                        cmd.Parameters.Add(new SqlParameter("sFechaReg", psFechaReg));
                    if (!string.IsNullOrEmpty(psFechaReg2))
                        cmd.Parameters.Add(new SqlParameter("sFechaReg2", psFechaReg2));
                    if (psPagina != "0")
                        cmd.Parameters.Add(new SqlParameter("nPagina", psPagina));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }

                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerPistas", "clsComun");
            }
        }
        return dtResultado;

    }

    /// <summary>
    /// Devuelve la consulta de pistas BD
    /// </summary>
    /// <param name="psUsuario">Usuario del sistema</param>
    /// <param name="psAccion">Accion a buscar</param>
    /// <param name="sFechaReg">Fecha registros</param>
    /// <param name="sFechaReg2">Fecha registros</param>
    public static DataSet fnObtenerPistasBD(string psUsuario, string psAccion,
                                            string psAccion2, string psAccion3,
                                            string sFechaReg, string sFechaReg2)
    {
        DataSet dsResultado = new DataSet("PistasBd");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conAuditoriasBD"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_AuditoriasBD_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(psUsuario))
                        cmd.Parameters.Add(new SqlParameter("psUsuario", psUsuario));
                    if (!string.IsNullOrEmpty(psAccion))
                        cmd.Parameters.Add(new SqlParameter("psAccion", psAccion));
                    if (!string.IsNullOrEmpty(psAccion2))
                        cmd.Parameters.Add(new SqlParameter("psAccion2", psAccion2));
                    if (!string.IsNullOrEmpty(psAccion3))
                        cmd.Parameters.Add(new SqlParameter("psAccion3", psAccion3));
                    if (!string.IsNullOrEmpty(sFechaReg))
                        cmd.Parameters.Add(new SqlParameter("sFechaReg", sFechaReg));
                    if (!string.IsNullOrEmpty(sFechaReg2))
                        cmd.Parameters.Add(new SqlParameter("sFechaReg2", sFechaReg2));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerPistasBD", "clsComun");
            }
        }
        return dsResultado;
    }

    /// <summary>
    /// Devuelve la consulta de pistas SO
    /// </summary>
    /// <param name="psUsuario"></param>
    /// <param name="psAccion"></param>
    /// <param name="psAccion2"></param>
    /// <param name="psAccion3"></param>
    /// <param name="sFechaReg"></param>
    /// <param name="sFechaReg2"></param>
    /// <param name="sEntryType"></param>
    /// <param name="sSource"></param>
    /// <param name="sHost"></param>
    /// <returns></returns>
    public static DataSet fnObtenerPistasSO(string psUsuario, string psAccion,
                                        string psAccion2, string psAccion3,
                                        string sFechaReg, string sFechaReg2,
                                        string sEntryType, string sSource,
                                        string sHost)
    {
        DataSet dsResultado = new DataSet("PistasSO");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_AuditoriasSO_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(psUsuario))
                        cmd.Parameters.Add(new SqlParameter("psUsuario", psUsuario));
                    if (!string.IsNullOrEmpty(psAccion))
                        cmd.Parameters.Add(new SqlParameter("psAccion", psAccion));
                    if (!string.IsNullOrEmpty(psAccion2))
                        cmd.Parameters.Add(new SqlParameter("psAccion2", psAccion2));
                    if (!string.IsNullOrEmpty(psAccion3))
                        cmd.Parameters.Add(new SqlParameter("psAccion3", psAccion3));
                    if (!string.IsNullOrEmpty(sFechaReg))
                        cmd.Parameters.Add(new SqlParameter("sFechaReg", sFechaReg));
                    if (!string.IsNullOrEmpty(sFechaReg2))
                        cmd.Parameters.Add(new SqlParameter("sFechaReg2", sFechaReg2));
                    if (!string.IsNullOrEmpty(sEntryType))
                        cmd.Parameters.Add(new SqlParameter("entryType", sEntryType));
                    if (!string.IsNullOrEmpty(sSource))
                        cmd.Parameters.Add(new SqlParameter("source", sSource));
                    if (!string.IsNullOrEmpty(sHost))
                        cmd.Parameters.Add(new SqlParameter("host", sHost));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerPistasSO", "clsComun");
            }
        }
        return dsResultado;
    }

    /// <summary>
    /// Devuelve el resultado de PSECFDI
    /// </summary>
    /// <param name="sFechaIni"></param>
    /// <param name="sFechaFin"></param>
    /// <returns></returns>
    public static DataSet fnObtenerConsultaComprobantesPSECFDI(string fechaIni, string fechaFin)
    {
        DataSet dsResultado = new DataSet("ConsultaPSECFDI");

        //Cambiar conexion
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_PSECFDI_Fechas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sFechaIni", fechaIni));
                    cmd.Parameters.Add(new SqlParameter("sFechaFin", fechaFin));
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerConsultaComprobantesPSECFDI", "clsComun");
            }
        }
        return dsResultado;
    }

    /// <summary>
    /// Devuelve el resultado de PSECFDI de Totales
    /// </summary>
    /// <param name="sFechaIni"></param>
    /// <param name="sFechaFin"></param>
    /// <returns></returns>
    public static int fnObtenerConsultaComprobantesPSECFDITotales(string fechaIni, string fechaFin, int seleccion)
    {
        DataSet table = new DataSet("ConsultaPSECFDI");

        int dsResultado = 0;
        //Cambiar conexion
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_PSECFDI_Fechas_Totales", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sFechaIni", fechaIni));
                    cmd.Parameters.Add(new SqlParameter("sFechaFin", fechaFin));
                    cmd.Parameters.Add(new SqlParameter("nTipo", seleccion));
                    con.Open();
                    if (seleccion == 2 || seleccion == 3)
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                           da.Fill(table);
                           dsResultado = (int)table.Tables[0].Rows.Count;
                        }
                    }
                    else
                    {
                        dsResultado = (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerConsultaComprobantesPSECFDITotales", "clsComun");
            }
        }
        return dsResultado;
    }

    /// <summary>
    /// Devuelve el Total de comprobantes cat
    /// </summary>
    /// <returns></returns>
    public static int fnObtenerComprobantesTotales(int nTipo)
    {
        int dsResultado = 0;
        //Cambiar conexion
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_ComprobantesTotales", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nTipo", nTipo));
                    con.Open();
                    dsResultado = (int)cmd.ExecuteScalar();

                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerComprobantesTotales", "clsComun");
            }
        }
        return dsResultado;
    }

    /// <summary>
    /// Recupera la consulta de los acuses del SOAP
    /// </summary>
    /// <param name="psUUID"></param>
    /// <param name="psSOAP"></param>
    /// <param name="sFechaIni"></param>
    /// <param name="sFechaFin"></param>
    /// <returns></returns>
    public static DataTable fnObtenerSOAP(string sIdContribuyente,string psUUID, string psSOAP,
                                        string sFechaIni, string sFechaFin,string sOrigen,string sEfecto,string sCodigo)
    {
        DataTable dtResultado = new DataTable("Pistas");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_SOAP_Consulta_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(sIdContribuyente))
                        cmd.Parameters.Add(new SqlParameter("nid_Contribuyente", sIdContribuyente));
                    if (!string.IsNullOrEmpty(psUUID))
                        cmd.Parameters.Add(new SqlParameter("psUUID", psUUID));
                    if (!string.IsNullOrEmpty(psUUID))
                        cmd.Parameters.Add(new SqlParameter("psSOAP", psSOAP));
                    if (!string.IsNullOrEmpty(sFechaIni))
                        cmd.Parameters.Add(new SqlParameter("sFechaIni", sFechaIni));
                    if (!string.IsNullOrEmpty(sFechaFin))
                        cmd.Parameters.Add(new SqlParameter("sFechaFin", sFechaFin));
                    if (!string.IsNullOrEmpty(sOrigen))
                        cmd.Parameters.Add(new SqlParameter("sOrigen", sOrigen));
                    if (!string.IsNullOrEmpty(sEfecto))
                        cmd.Parameters.Add(new SqlParameter("sEfecto", sEfecto));
                    if (!string.IsNullOrEmpty(sCodigo))
                        cmd.Parameters.Add(new SqlParameter("sCodigo", sCodigo));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerSOAP", "clsComun");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Recupera la lista de UUD del SOAP
    /// </summary>
    /// <returns></returns>
    public static DataTable fnObtenerUUID(string sSoap, string sEfecto)
    {
        DataTable dtResultado = new DataTable("SOAP");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_SOAP_UUID_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(sSoap))
                        cmd.Parameters.Add(new SqlParameter("sSOAP", sSoap));
                    if (!string.IsNullOrEmpty(sEfecto))
                        cmd.Parameters.Add(new SqlParameter("sEfecto", sEfecto));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerUUID", "clsComun");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static string fnObtieneValoresTablaDatosPistas(DataTable dtDatos)
    {
        string sRetorno = string.Empty;
        try
        {
            foreach (DataRow drRenglon in dtDatos.Rows)
            {
                for (int i = 0; i < dtDatos.Columns.Count; i++)
                {
                    sRetorno += "|" + drRenglon[i].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnObtieneValoresTablaDatos", "clsComun");
        }
        return sRetorno;
    }

    /// <summary>
    /// Devuelve el catalogo de paises de ccePaises
    /// </summary>
    /// <returns>DataTable con los paises disponibles</returns>
    public static DataTable fnLlenarDropPaises_n12()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Obtener_cce_Pais_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarDropPaises_n12", "clsComun");
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Devuelve una parte del catalogo de estados cceEstados, filtrando por el país indicado
    /// </summary>
    /// <param name="psId_Pais">ID del país para el cual se quiere recuperar sus estados</param>
    /// <returns>DataTable con los estados filtrados por país</returns>
    public static DataTable fnLlenarDropEstados_n12(int pnC_Pais)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Con_Obtener_cce_Estado_Sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nPais", pnC_Pais);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarDropEstado_n12", "clsComun");
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }
    /// <summary>
    /// Método encargado de establecer el titulo de la página actual en una Label más vistosa
    /// localizada en la MasterPage correspondiente
    /// </summary>
    /// <param name="pPagina">La página actual</param>
    public static void fnPonerTitulo(Page pPagina)
    {
        //try
        //{
        //    Label lblNombreModulo = (Label)pPagina.Master.FindControl("lblNombreModulo");
        //    lblNombreModulo.Text = pPagina.Title;
        //}
        //catch
        //{
        //    //La Label no existe en la MasterPage, no se realiza acción alguna
        //}
    }

    /// <summary>
    /// Devuelve la descripcion del error del SAT
    /// </summary>
    /// /// <param name="pIdCodigo">numero de error</param>
    /// /// <param name="psTipo">tipo de error</param>
    /// <returns></returns>
    public static string fnRecuperaErrorSAT(string pIdCodigo, string psTipo)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RecuperaCodigo_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Codigo", pIdCodigo));
                    cmd.Parameters.Add(new SqlParameter("nsTipo", psTipo));

                    con.Open();
                    sResultado = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnRecuperaErrorSAT", "clsComun");
            }
        }
        return sResultado;

    }

    /// <summary>
    /// Regresa el request armado para la recepcion.
    /// </summary>
    /// <param name="psComprobante"></param>
    /// <param name="psTipoDocumento"></param>
    /// <param name="pnIdEstructura"></param>
    /// <param name="sNombre"></param>
    /// <param name="sContraseña"></param>
    /// <returns></returns>
    public static string fnRequestRecepcion(string psComprobante, string psTipoDocumento,
        string pnIdEstructura, string sNombre, string sContraseña)
    {
        byte[] retXML = System.Text.Encoding.UTF8.GetBytes(psComprobante);
        string sXml = Convert.ToBase64String(retXML);

        byte[] retUsuario = System.Text.Encoding.UTF8.GetBytes(sNombre);
        string sUsuario = Convert.ToBase64String(retUsuario);

        byte[] retDoc = System.Text.Encoding.UTF8.GetBytes(psTipoDocumento);
        string sDoc = Convert.ToBase64String(retDoc);

        byte[] retEs = System.Text.Encoding.UTF8.GetBytes(pnIdEstructura);
        string sEs = Convert.ToBase64String(retEs);

        //Respuesta Request *************************************************************
        string strSoapMessage =
        "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope\">" +
            "<s:Header>" +
            "<Action s:mustUnderstand=\"1\" xmlns=\"http://schemas.microsoft.com/ws/2005/05/addressing/none\">https://www.paxfacturacion.com.mx:452/</Action>" +
            "</s:Header>" +
            "<s:Body>" +
            "<fnEnviarXML xmlns=\"https://www.paxfacturacion.com.mx\">" +
                "<psComprobante>" + sXml + "</psComprobante>" +
                "<psTipoDocumento>" + sDoc + "</psTipoDocumento>" +
                "<pnId_Estructura>" + sEs + "</pnId_Estructura>" +
                "<sNombre>" + sUsuario + "</sNombre>" +
            //"<sContraseña>" + sContraseña + "</sContraseña>" +
            "</fnEnviarXML>" +
            "</s:Body>" +
        "</s:Envelope>";
        return strSoapMessage;
        //Respuesta Request *************************************************************
    }

    /// <summary>
    /// Regresa el response armado para la recepcion
    /// </summary>
    /// <returns></returns>
    public static string fnResponseRecepcion(string fnEnviarXMLResult)
    {
        //Respuesta Response*************************************************************
        string strSoapMessage =
        "<s:Acuse xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope\">" +
          "<s:Header />" +
          "<s:Body>" +
            "<fnEnviarXMLResponse xmlns=\"https://paxfacturacion.com.mx\">" +
              "<fnEnviarXMLFecha>" + DateTime.Now.ToString("s") + "</fnEnviarXMLFecha>" +
              "<fnEnviarXMLResult>" + fnEnviarXMLResult + "</fnEnviarXMLResult>" +
            "</fnEnviarXMLResponse>" +
          "</s:Body>" +
        "</s:Acuse>";

        return strSoapMessage;
        //Respuesta Response*************************************************************
    }

    /// <summary>
    /// Verifica la fecha de emisión no es posterior al 01 de enero 2011
    /// </summary>
    /// <returns></returns>
    public static bool fnRevisarFechaNoPosterior(DateTime pdFecha)
    {
        bool retorno = false;

        try
        {

            if (pdFecha >= Convert.ToDateTime(clsComun.fnObtenerParametro("FechaPosterior_v3")))
                retorno = true;
            else
                retorno = false;
        }
        catch
        {
            if (pdFecha <= Convert.ToDateTime("2011-01-01"))
                retorno = true;
            else
                retorno = false;
        }

        return retorno;
    }

    /// <summary>
    /// Retorna un objeto con la información del usuario en sesión
    /// </summary>
    /// <returns>Objeto clsInicioSesionUsuario</returns>
    public static clsInicioSesionUsuario fnUsuarioEnSesion()
    {
        try
        {
            return (clsInicioSesionUsuario)System.Web.HttpContext.Current.Session["objUsuario"];
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Encargado de validar si la expreson es verdadera
    /// </summary>
    /// <param name="sValor">valor a evaluar</param>
    /// <param name="expresion">expresion regular</param>
    /// <returns>retorna si es verdadero</returns>
    public static bool fnValidaExpresion(string sValor, string expresion)
    {
        bool bRetorno = false;

        if (Regex.IsMatch(sValor, expresion))
        {
            bRetorno = true;
        }

        return bRetorno;
    }



    public static DataTable fnLlenarDropMunicipios(int pnIdEstado)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Con_Obtener_cce_Municipios_Sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nEstado", pnIdEstado);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarDropMunicipios", "clsComun");
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }

    public static DataTable fnLlenarDropCodigoPostal(int pnIdMunicipio)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Con_Obtener_cce_CodigosPostales_Sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nMunicipio", pnIdMunicipio);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnLlenarDropCodigoPostal", "clsComun");
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }

    /// <summary>
    ///	Author		: César Negrete Villa
    /// Date  		: 15/10/2016  10:45
    /// Description : Modal de Avisos y Errores Bootstrap
    /// </summary>
    /// <param name="pPagina">Pagina Origen</param>
    /// <param name="psMensaje">Text Body Modal</param>
    /// <param name="psTitle">Text header Modal</param>
    /// <param name="psSize">Tamaño Modal(small,medium,large) </param>
    /// <param name="psImageName">Imagen Header</param>
    public static void fnMessage(Page pPagina, string psMensaje, string psTitle, string psSize, string psImageName)
    {
        ScriptManager.RegisterStartupScript(pPagina, pPagina.GetType(), "Alert", "ShowAlertError('" + psTitle + "','" + psMensaje + "','" + Resources.resCorpusCFDIEs.lblAceptar + "','" + psSize + "','" + psImageName + "');", true);
    }
}

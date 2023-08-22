using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
    ///  <param name="p_CorreoElectronico">correo electronico</param>
    ///  <param name="p_tipoAcuse">tipo de acuse</param>
    /// <returns></returns>
    public static Int32 fnInsertaAcusePAC(int p_idcte, string p_UUID, string p_Acuse, DateTime sFecha, string p_idcodigo, string p_Soap, string origen, string p_CorreoElectronico)
    {
        Int32 Resultado = 0;

        using (SqlConnection conexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(System.Configuration.ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                conexion.Open();
                using (SqlCommand command = new SqlCommand("usp_Cfd_Acuse_PAC_Ins", conexion))
                {
                    command.CommandTimeout = 20;
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@nid_Contribuyente", p_idcte);
                    command.Parameters.AddWithValue("@sUUID", p_UUID);
                    command.Parameters.AddWithValue("@sAcuse", p_Acuse);
                    command.Parameters.AddWithValue("@sFecha", sFecha);
                    command.Parameters.AddWithValue("@nid_codigo", p_idcodigo);
                    command.Parameters.AddWithValue("@sSoap", p_Soap);
                    command.Parameters.AddWithValue("@sOrigen", origen);
                    command.Parameters.AddWithValue("@sMail", p_CorreoElectronico);
                    Resultado = Convert.ToInt32(command.ExecuteNonQuery());


                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            finally
            {
                conexion.Close();
            }
        }

        //giSql = clsComun.fnCrearConexion("conControl");
        //giSql.AgregarParametro("@nid_Contribuyente", p_idcte);
        //giSql.AgregarParametro("@sUUID", p_UUID);
        //giSql.AgregarParametro("@sAcuse", p_Acuse);
        //giSql.AgregarParametro("@sFecha", sFecha);
        //giSql.AgregarParametro("@nid_codigo", p_idcodigo);
        //giSql.AgregarParametro("@sSoap", p_Soap);
        //giSql.AgregarParametro("@sOrigen", origen);
        //giSql.AgregarParametro("@sMail", p_CorreoElectronico);

        //Int32 Resultado = Convert.ToInt32(giSql.NoQuery("usp_Cfd_Acuse_PAC_Ins", true));
        return Resultado;
    }

    /// <summary>
    /// Inserta un nuevo registro de acuse del emitido por el SAT
    /// </summary>
    /// /// <param name="p_UUID">UUID del comprobante</param>
    ///  <param name="sFecha">fecha de envio</param>
    ///  <param name="p_Acuse">xml con el acuse que emitio el SAT</param>
    ///  <param name="p_idcodigo">tipo de codigo que se genero</param>
    /// <returns></returns>
    public static Int32 fnInsertaAcuseSAT(string sId_Contribuyente, string p_UUID, string p_Acuse, DateTime sFecha, string p_idcodigo, string p_Soap, string origen)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Acuse_SAT_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("nid_Contribuyente", sId_Contribuyente));
                    cmd.Parameters.Add(new SqlParameter("sUUID", p_UUID));
                    cmd.Parameters.Add(new SqlParameter("sAcuse", p_Acuse));
                    cmd.Parameters.Add(new SqlParameter("sFecha", sFecha));
                    cmd.Parameters.Add(new SqlParameter("nid_codigo", p_idcodigo));
                    cmd.Parameters.Add(new SqlParameter("sSoap", p_Soap));
                    cmd.Parameters.Add(new SqlParameter("sOrigen", origen));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                using (SqlCommand cmd = new SqlCommand("usp_Con_Obtener_Paises_Sel", con))
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                using (SqlCommand cmd = new SqlCommand("usp_Con_Sucursal_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", fnUsuarioEnSesion().id_usuario));
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Devuelve una parte del catalogo de estados, filtrando por el país indicado
    /// </summary>
    /// <param name="psId_Pais">ID del país para el cual se quiere recuperar sus estados</param>
    /// <returns>DataTable con los estados filtrados por país</returns>
    public static DataTable fnLlenarDropEstados(string psId_Pais)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Con_Obtener_Estados_Sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nId_Pais", psId_Pais);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
        ScriptManager.RegisterStartupScript(pPagina, typeof(Page), "Mensaje" + DateTime.Now.Millisecond.ToString(),
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
    /// Muestra un mensaje de error en la parte superior de la página
    /// </summary>
    /// <param name="pPagina">Página que llama al método</param>
    /// <param name="psMensajeError">Mensaje de error a mostrar</param>
    public static void fnMostrarError(System.Web.UI.Page pPagina, string psMensajeError)
    {
        Label lbl = (Label)pPagina.Master.FindControl("lblErrorGenerico");
        lbl.Text = psMensajeError;
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
            int id_usuario = fnUsuarioEnSesion().id_usuario;
            DateTime fecha = DateTime.Now;
            StringBuilder accion = new StringBuilder();

            foreach (object arg in args)
            {
                accion.Append(arg);
                accion.Append(" | ");
            }

            clsPistasAuditoria.fnGenerarPistasAuditoria(id_usuario, fecha, accion.ToString().Trim('|'));
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }
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
    /// Obtener Paramentro Correo Obtiene el valor del parametro para el correo.
    /// </summary>
    /// <param name="psParametro">Nombre del parametros a buscar</param>
    /// <returns>Cadena con le valor del parámetro</returns>
    public static string ObtenerParamentro(string psParametro)
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return sRetorno;
    }

    /// <summary>
    /// Devuelve el catalogo de parametros
    /// </summary>
    /// <param name="pbDrop">Indica si también se quiere devolver la matriz</param>
    /// <returns></returns>
    public static DataTable LlenarParametros()
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dsResultado;
    }

    /// <summary>
    /// Devuelve el Total de comprobantes cat
    /// </summary>
    /// <returns></returns>
    public static int fnObtenerComprobantesTotales(int Tipo)
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
                    cmd.Parameters.Add(new SqlParameter("nTipo", Tipo));
                    con.Open();
                    dsResultado = (int)cmd.ExecuteScalar();

                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dsResultado;
    }

    /// <summary>
    /// Devuelve el ultimo registro de la tabla historica
    /// </summary>
    /// <returns></returns>
    /*public static int fnObtenerComprobantesHistorico(int Tipo)
    {
        int dsResultado = 0;
        //Cambiar conexion
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Comprobantes_Historico_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nTipo", Tipo));
                    con.Open();
                    dsResultado = (int)cmd.ExecuteScalar();

                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dsResultado;
    }*/

    /// <summary>
    /// Recupera la consulta de los acuses del SOAP
    /// </summary>
    /// <param name="psUUID"></param>
    /// <param name="psSOAP"></param>
    /// <param name="sFechaIni"></param>
    /// <param name="sFechaFin"></param>
    /// <returns></returns>
    public static DataTable fnObtenerSOAP(string sId_Contribuyente,string psUUID, string psSOAP,
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

                    if (!string.IsNullOrEmpty(sId_Contribuyente))
                        cmd.Parameters.Add(new SqlParameter("nid_Contribuyente", sId_Contribuyente));
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Recupera la lista de UUD del SOAP
    /// </summary>
    /// <returns></returns>
    public static DataTable fnObtenerUUID(string soap, string efecto)
    {
        DataTable dtResultado = new DataTable("SOAP");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConsultas"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_SOAP_UUID_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(soap))
                        cmd.Parameters.Add(new SqlParameter("sSOAP", soap));
                    if (!string.IsNullOrEmpty(efecto))
                        cmd.Parameters.Add(new SqlParameter("sEfecto", efecto));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }


    /// <summary>
    /// Devuelve la descripcion del error del SAT
    /// </summary>
    /// /// <param name="p_idcodigo">numero de error</param>
    /// /// <param name="p_sTipo">tipo de error</param>
    /// <returns></returns>
    public static string fnRecuperaErrorSAT(string p_idcodigo, string p_sTipo)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RecuperaCodigo_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Codigo", p_idcodigo));
                    cmd.Parameters.Add(new SqlParameter("nsTipo", p_sTipo));

                    con.Open();
                    sResultado = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return sResultado;

    }

    /// <summary>
    /// Regresa el request armado para la recepcion.
    /// </summary>
    /// <param name="psComprobante"></param>
    /// <param name="psTipoDocumento"></param>
    /// <param name="pnId_Estructura"></param>
    /// <param name="sNombre"></param>
    /// <param name="sContraseña"></param>
    /// <returns></returns>
    public static string fnRequestRecepcion(string psComprobante, string psTipoDocumento,
        string pnId_Estructura, string sNombre, string sContraseña)
    {
        byte[] retXML = System.Text.Encoding.UTF8.GetBytes(psComprobante);
        string sXml = Convert.ToBase64String(retXML);

        byte[] retUsuario = System.Text.Encoding.UTF8.GetBytes(sNombre);
        string sUsuario = Convert.ToBase64String(retUsuario);

        byte[] retDoc = System.Text.Encoding.UTF8.GetBytes(psTipoDocumento);
        string sDoc = Convert.ToBase64String(retDoc);

        byte[] retEs = System.Text.Encoding.UTF8.GetBytes(pnId_Estructura);
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
            
            if (pdFecha>= Convert.ToDateTime(clsComun.ObtenerParamentro("FechaPosterior_v3")))
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
}

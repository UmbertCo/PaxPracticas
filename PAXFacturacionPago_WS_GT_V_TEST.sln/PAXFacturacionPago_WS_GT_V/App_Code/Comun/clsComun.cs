using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

/// <summary>
/// Clase encargada de proporcionar funciones de ayuda a todas las demás clases.
/// </summary>
public class clsComun
{
    private static InterfazSQL giSql;
    private static DataTable gdtAuxiliar;

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
    /// Genera un nuevo objeto de conexión a la base de datos
    /// </summary>
    /// /// <param name="psNombreConexion">Nombre de la cadena de conexión guardada en el web config</param>
    /// <returns>objeto InterfazSQL</returns>
    public static InterfazSQL fnCrearConexion(string psNombreConexion)
    {
        try
        {
            string cadena = System.Configuration.ConfigurationManager.ConnectionStrings[psNombreConexion].ConnectionString;
            return new InterfazSQL(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena));
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    /// <summary>
    /// Devuelve el catalogo de paises
    /// </summary>
    /// <returns>DataTable con los paises disponibles</returns>
    public static DataTable fnLlenarDropPaises()
    {
        giSql = fnCrearConexion("conConfiguracion");
        gdtAuxiliar = new DataTable();

        try
        {
            giSql.Query("usp_Con_Obtener_Paises_Sel", true, ref gdtAuxiliar);
        }
        catch(SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }

        return gdtAuxiliar;
    }

    /// <summary>
    /// Devuelve el catalogo de sucursales para este usuario
    /// </summary>
    /// <param name="pbDrop">Indica si también se quiere devolver la matriz</param>
    /// <returns></returns>
    public static DataTable LlenarDropSucursales(bool pbMatriz)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");
        DataTable gdtAuxiliar = new DataTable("Sucursales");

        giSql.AgregarParametro("nId_Usuario", fnUsuarioEnSesion().id_usuario);
        giSql.AgregarParametro("bDrop", pbMatriz);
        giSql.Query("usp_Con_Sucursal_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
    }

    /// <summary>
    /// Devuelve una parte del catalogo de estados, filtrando por el país indicado
    /// </summary>
    /// <param name="idPais">ID del país para el cual se quiere recuperar sus estados</param>
    /// <returns>DataTable con los estados filtrados por país</returns>
    public static DataTable fnLlenarDropEstados(string psIdPais)
    {
        giSql = fnCrearConexion("conConfiguracion");
        gdtAuxiliar = new DataTable();

        try
        {
            giSql.AgregarParametro("nId_Pais", psIdPais);
            giSql.Query("usp_Con_Obtener_Estados_Sel", true, ref gdtAuxiliar);
        }
        catch (SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }

        return gdtAuxiliar;
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
    /// Obtener Paramentro Correo Obtiene el valor del parametro para el correo.
    /// </summary>
    /// <param name="sParametro">Nombre del parametros a buscar</param>
    /// <returns>Cadena con le valor del parámetro</returns>
    public static string ObtenerParamentro(string sParametro)
    {

        string nRetorno = string.Empty;

        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conControl");

            conexion.AgregarParametro("sParametro", sParametro);

            nRetorno = (string)conexion.TraerEscalar("usp_Ctp_BuscarParametro_Sel", true);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return nRetorno;
    }

    /// <summary>
    /// Recupera los esquemas de los complementos de la version 3.2 
    /// </summary>
    /// <returns></returns>
    public static DataTable fnObtenerXSDComplementos()
    {
        giSql = clsComun.fnCrearConexion("conConsultas");
        DataTable gdtAuxiliar = new DataTable("complementos");

        giSql.Query("usp_Con_RecuperaXSDComplementos", true, ref gdtAuxiliar);

        return gdtAuxiliar;
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
    /// Encargado de validar si la expreson es verdadera
    /// </summary>
    /// <param name="sValor">valor a evaluar</param>
    /// <param name="expresion">expresion regular</param>
    /// <returns>retorna si es verdadero</returns>
    public static bool fnValidaExpresion(string sValor,string expresion)
    {
        bool bRetorno=false;

        if (Regex.IsMatch(sValor, expresion))
        {
            bRetorno =true;
        }

        return bRetorno;
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
    /// Devuelve el catalogo de parametros
    /// </summary>
    /// <param name="pbDrop">Indica si también se quiere devolver la matriz</param>
    /// <returns></returns>
    public static DataTable LlenarParametros()
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");
        DataTable gdtAuxiliar = new DataTable("Parametros");

        giSql.Query("usp_Con_Parametros_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
    }

    /// <summary>
    /// Inserta o actualiza un parametro.
    /// Si el parámetro psIdParametro es cadena vacía entonces es inserción de lo
    /// contrario es actualización.
    /// </summary>
    public int fnGuardarParametro(string psIdParametros, string psParametro,
                                    string psValor, string psEstatus)
    {
        giSql = clsComun.fnCrearConexion("conConfiguracion");

        if (!string.IsNullOrEmpty(psIdParametros))
            giSql.AgregarParametro("nId_Parametro", psIdParametros);

        if (!string.IsNullOrEmpty(psParametro))
            giSql.AgregarParametro("@sParametro", psParametro);
        if (!string.IsNullOrEmpty(psValor))
            giSql.AgregarParametro("@sValor", psValor);
        if (!string.IsNullOrEmpty(psEstatus))
            giSql.AgregarParametro("@sEstatus", psEstatus);

        return giSql.NoQuery("usp_Con_Parametros", true);
    }

    /// <summary>
    /// Devuelve el catalogo de usuarios activos
    /// </summary>
    /// <returns></returns>
    public static DataTable fnObtenerUsuarios()
    {
        giSql = clsComun.fnCrearConexion("conConsultas");
        DataTable gdtAuxiliar = new DataTable("Parametros");

        giSql.Query("usp_Con_RecuperaUsu_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
    }

    /// <summary>
    /// Devuelve la consulta de pistas
    /// </summary>
    /// <param name="psUsuario">Usuario del sistema</param>
    /// <param name="psAccion">Accion a buscar</param>
    /// <param name="sFechaReg">Fecha registros</param>
    /// <param name="sFechaReg2">Fecha registros</param>
    public static DataTable fnObtenerPistas(string psUsuario, string psAccion,
                                            string psAccion2, string psAccion3, 
                                            string sFechaReg, string sFechaReg2)
    {
        giSql = clsComun.fnCrearConexion("conConsultas");
        DataTable gdtAuxiliar = new DataTable("Pistas");

        if (!string.IsNullOrEmpty(psUsuario))
            giSql.AgregarParametro("@psUsuario", psUsuario);
        if (!string.IsNullOrEmpty(psAccion))
            giSql.AgregarParametro("@psAccion", psAccion);
        if (!string.IsNullOrEmpty(psAccion2))
            giSql.AgregarParametro("@psAccion2", psAccion2);
        if (!string.IsNullOrEmpty(psAccion3))
            giSql.AgregarParametro("@psAccion3", psAccion3);
        if (!string.IsNullOrEmpty(sFechaReg))
            giSql.AgregarParametro("@sFechaReg", sFechaReg);
        if (!string.IsNullOrEmpty(sFechaReg2))
            giSql.AgregarParametro("@sFechaReg2", sFechaReg2);

        giSql.Query("usp_Con_PistasFiltros_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
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
        giSql = clsComun.fnCrearConexion("conAuditoriasBD");
        DataSet gdtAuxiliar = new DataSet("PistasBd");

        if (!string.IsNullOrEmpty(psUsuario))
            giSql.AgregarParametro("@psUsuario", psUsuario);
        if (!string.IsNullOrEmpty(psAccion))
            giSql.AgregarParametro("@psAccion", psAccion);
        if (!string.IsNullOrEmpty(psAccion2))
            giSql.AgregarParametro("@psAccion2", psAccion2);
        if (!string.IsNullOrEmpty(psAccion3))
            giSql.AgregarParametro("@psAccion3", psAccion3);
        if (!string.IsNullOrEmpty(sFechaReg))
            giSql.AgregarParametro("@sFechaReg", sFechaReg);
        if (!string.IsNullOrEmpty(sFechaReg2))
            giSql.AgregarParametro("@sFechaReg2", sFechaReg2);

        giSql.Query("usp_Ctp_AuditoriasBD_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
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
        giSql = clsComun.fnCrearConexion("conConsultas");
        DataSet gdtAuxiliar = new DataSet("PistasSO");

        if (!string.IsNullOrEmpty(psUsuario))
            giSql.AgregarParametro("@psUsuario", psUsuario);
        if (!string.IsNullOrEmpty(psAccion))
            giSql.AgregarParametro("@psAccion", psAccion);
        if (!string.IsNullOrEmpty(psAccion2))
            giSql.AgregarParametro("@psAccion2", psAccion2);
        if (!string.IsNullOrEmpty(psAccion3))
            giSql.AgregarParametro("@psAccion3", psAccion3);
        if (!string.IsNullOrEmpty(sFechaReg))
            giSql.AgregarParametro("@sFechaReg", sFechaReg);
        if (!string.IsNullOrEmpty(sFechaReg2))
            giSql.AgregarParametro("@sFechaReg2", sFechaReg2);
        if (!string.IsNullOrEmpty(sEntryType))
            giSql.AgregarParametro("@entryType", sEntryType);
        if (!string.IsNullOrEmpty(sSource))
            giSql.AgregarParametro("@source", sSource);
        if (!string.IsNullOrEmpty(sHost))
            giSql.AgregarParametro("@host", sHost);

        giSql.Query("usp_Ctp_AuditoriasSO_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
    }

    /// <summary>
    /// Devuelve la descripcion del error del SAT
    /// </summary>
    /// /// <param name="p_idcodigo">numero de error</param>
    /// /// <param name="p_sTipo">tipo de error</param>
    /// <returns></returns>
    public static string fnRecuperaErrorSAT(string p_idcodigo, string p_sTipo)
    {
        string Resultado = string.Empty;

        //giSql = clsComun.fnCrearConexion("conConfiguracion");
        //giSql.AgregarParametro("@nId_Codigo", p_idcodigo);
        //giSql.AgregarParametro("@nsTipo", p_sTipo);
        //string Resultado = Convert.ToString(giSql.TraerEscalar("usp_Ctp_RecuperaCodigo_Sel", true));
        //return Resultado;

        switch (p_idcodigo)
        {
            case "301":
                Resultado = "301 - XML mal formado.";
                break;
            case "97":
                Resultado = "97 - No hay créditos disponibles.";
                break;
            case "406":
                Resultado = "406 - El nombre de documento no corresponde a ningúno del sistema.";
                break;
            case "333":
                Resultado = "333 - El xml no cumple con el esquema de Hacienda.";
                break;
            case "570":
                Resultado = "570 - No se pudó recuperar el certificado del comprobante.";
                break;
            case "306":
                Resultado = "306 - El certificado no es de tipo CSD.";
                break;
            case "308":
                Resultado = "308 - Certificado no expedido por el SAT.";
                break;
            case "N-504":
                Resultado = "N-504 - La estructura del comprobante recibido no es válida.";
                break;
            case "307":
                Resultado = "307 - El CFDI contiene un timbre previo.";
                break;
            case "303":
                Resultado = "303 - Sello no corresponde a emisor o caduco.";
                break;
            case "304":
                Resultado = "304 - Certificado revocado o caduco.";
                break;
            case "305":
                Resultado = "305 - La fecha de emisión no está dentro de la vigencia del CSD del Emisor.";
                break;
            case "403":
                Resultado = "403 - Que la fecha de emisión sea posterior al 01 de Enero 2011.";
                break;
            case "402":
                Resultado = "402 - Que exista el RFC del emisor conforme al régimen autorizado (Lista de validación de régimen) LCO.";
                break;
            case "401":
                Resultado = "401 - Que el rango de la fecha de generación no sea mayor a 72 horas para la emisión del timbre.";
                break;
            case "302":
                Resultado = "302 - Sello mal formado o inválido.";
                break;
            case "622":
                Resultado = "622 - El servicio no esta disponible.";
                break;
            case "817":
                Resultado = "817 - No se pudo generar el sello del PAC.";
                break;
            case "98":
                Resultado = "98 - El comprobante ya se encuentra en la base de datos.";
                break;
            case "999":
                Resultado = "999 - No se pudo registrar el comprobante.";
                break;
            case "95":
                Resultado = "95 - Usuario inexistente.";
                break;
            case "90":
                Resultado = "90 - Los datos del usuario son requeridos.";
                break;
            case "96":
                Resultado = "96 - Usuario o contraseña incorrecta.";
                break;
            case "91":
                Resultado = "91 - El usuario esta en estado pendiente.";
                break;
            case "0":
                Resultado = "0 - Sin Errores.";
                break;
            case "92":
                Resultado = "92 - El usuario esta en estado bloqueado.";
                break;
            case "93":
                Resultado = "93 - El usuario esta en estado expirado.";
                break;
            case "94":
                Resultado = "94 - El usuario esta en estado por cambiar contraseña.";
                break;
            case "201":
                Resultado = "201 - UUID Cancelado.";
                break;
            case "202":
                Resultado = "202 - UUID Previamente Cancelado.";
                break;
            case "203":
                Resultado = "203 - UUID No corresponde al emisor.";
                break;
            case "204":
                Resultado = "204 - UUID No aplicable para Cancelación, intente en 10 minutos.";
                break;
            case "205":
                Resultado = "205 - UUID No existente, intente en 10 minutos.";
                break;
            case "504":
                Resultado = "504 - La estructura del comprobante recibido no es válida.";
                break;
            case "309":
                Resultado = "309 - No se puede validar por falta de sello.";
                break;
            case "310":
                Resultado = "310 - No se puede validar por problemas al generar la cadena.";
                break;
            case "311":
                Resultado = "311 - Certificado reportado no corresponde a certificado usado.";
                break;
            case "312":
                Resultado = "312 - El comprobante no contiene un Timbre Fiscal, esto es requerido en CFDI.";
                break;
            case "179":
                Resultado = "179 - El RFC del Certificado no corresponde al del comprobante.";
                break;

            default:
                Resultado = "999 - Intente de nuevo.";
                break;

        }

        return Resultado;
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
    public static Int32 fnInsertaAcusePAC(int p_idcte, string p_UUID, string p_Acuse,DateTime sFecha, string p_idcodigo, string p_Soap, string origen,string p_CorreoElectronico)
    {
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@nid_Contribuyente", p_idcte);
        giSql.AgregarParametro("@sUUID", p_UUID);
        giSql.AgregarParametro("@sAcuse", p_Acuse);
        giSql.AgregarParametro("@sFecha", sFecha);
        giSql.AgregarParametro("@nid_codigo", p_idcodigo);
        giSql.AgregarParametro("@sSoap", p_Soap);
        giSql.AgregarParametro("@sOrigen", origen);
        giSql.AgregarParametro("@sMail", p_CorreoElectronico);
        
        Int32 Resultado = Convert.ToInt32(giSql.NoQuery("usp_Cfd_Acuse_PAC_Ins", true));
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
    public static Int32 fnInsertaAcuseSAT(string sId_Contribuyente,string p_UUID, string p_Acuse, DateTime sFecha, string p_idcodigo, string p_Soap,string origen)
    {
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@nid_Contribuyente", sId_Contribuyente);
        giSql.AgregarParametro("@sUUID", p_UUID);
        giSql.AgregarParametro("@sAcuse", p_Acuse);
        giSql.AgregarParametro("@sFecha", sFecha);
        giSql.AgregarParametro("@nid_codigo", p_idcodigo);
        giSql.AgregarParametro("@sSoap", p_Soap);
        giSql.AgregarParametro("@sOrigen", origen);
        Int32 Resultado = Convert.ToInt32(giSql.NoQuery("usp_Cfd_Acuse_SAT_Ins", true));
        return Resultado;
    }

    /// <summary>
    /// Recupera la lista de UUD del SOAP
    /// </summary>
    /// <returns></returns>
    public static DataTable fnObtenerUUID(string soap,string efecto)
    {
        giSql = clsComun.fnCrearConexion("conConsultas");
        DataTable gdtAuxiliar = new DataTable("SOAP");

        if (!string.IsNullOrEmpty(soap))
            giSql.AgregarParametro("@sSOAP", soap);
        if (!string.IsNullOrEmpty(soap))
            giSql.AgregarParametro("@sEfecto", efecto);

        giSql.Query("usp_Ctp_SOAP_UUID_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
    }

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
        giSql = clsComun.fnCrearConexion("conConsultas");
        DataTable gdtAuxiliar = new DataTable("Pistas");

        if (!string.IsNullOrEmpty(sId_Contribuyente))
            giSql.AgregarParametro("@nid_Contribuyente", sId_Contribuyente);
        if (!string.IsNullOrEmpty(psUUID))
            giSql.AgregarParametro("@psUUID", psUUID);
        if (!string.IsNullOrEmpty(psSOAP))
            giSql.AgregarParametro("@psSOAP", psSOAP);
        if (!string.IsNullOrEmpty(sFechaIni))
            giSql.AgregarParametro("@sFechaIni", sFechaIni);
        if (!string.IsNullOrEmpty(sFechaFin))
            giSql.AgregarParametro("@sFechaFin", sFechaFin);
        if (!string.IsNullOrEmpty(sOrigen))
            giSql.AgregarParametro("@sOrigen", sOrigen);
        if (!string.IsNullOrEmpty(sEfecto))
            giSql.AgregarParametro("@sEfecto", sEfecto);
        if (!string.IsNullOrEmpty(sCodigo))
            giSql.AgregarParametro("@sCodigo", sCodigo);

        giSql.Query("usp_Ctp_SOAP_Consulta_Sel", true, ref gdtAuxiliar);

        return gdtAuxiliar;
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
        //Respuesta Request *************************************************************
        string strSoapMessage =
        "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope\">" +
            "<s:Header>" +
            "<Action s:mustUnderstand=\"1\" xmlns=\"http://schemas.microsoft.com/ws/2005/05/addressing/none\">https://paxfacturacion.com.mx/IwcfRecepcion/fnEnviarXML</Action>" +
            "</s:Header>" +
            "<s:Body>" +
            "<fnEnviarXML xmlns=\"https://paxfacturacion.com.mx\">" +
                "<psComprobante>" + psComprobante + "</psComprobante>" +
                "<psTipoDocumento>" + psTipoDocumento + "</psTipoDocumento>" +
                "<pnId_Estructura>" + pnId_Estructura + "</pnId_Estructura>" +
                "<sNombre>" + sNombre + "</sNombre>" +
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

}

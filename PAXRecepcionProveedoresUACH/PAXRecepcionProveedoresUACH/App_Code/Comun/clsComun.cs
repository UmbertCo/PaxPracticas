using System;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;

/// <summary>
/// Clase encargada de proporcionar funciones de ayuda a todas las demás clases.
/// </summary>
public class clsComun
{
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
    /// Devuelve el catalogo de paises
    /// </summary>
    /// <returns>DataTable con los paises disponibles</returns>
    public static DataTable fnLlenarDropPaises()
    {
        DataTable gdtAuxiliar = new DataTable();

        try
        {
            using (SqlConnection con = new SqlConnection

            (Utilerias.Encriptacion.Base64.DesencriptarBase64

            (ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Con_Obtener_Paises_Sel";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(gdtAuxiliar);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return gdtAuxiliar;
    }

    /// <summary>
    /// Devuelve una parte del catalogo de municipios, filtrando por el estado indicado
    /// </summary>
    /// <param name="idPais">ID del estado para el cual se quiere recuperar sus municipios</param>
    /// <returns>DataTable con los estados filtrados por estado</returns>
    public static DataTable fnLlenarDropMunicipios(string sIdEstado)
    {
        //giSql = fnCrearConexion("conRecepcionProveedores");
        //gdtAuxiliar = new DataTable();

        //try
        //{
        //    giSql.AgregarParametro("nId_Estado", sIdEstado);
        //    giSql.Query("usp_Con_Obtener_Municipios_Sel", true, ref gdtAuxiliar);
        //}
        //catch (SqlException ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //}

        //return gdtAuxiliar;

        DataTable gdtAuxiliar = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection
            (Utilerias.Encriptacion.Base64.DesencriptarBase64

            (ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Con_Obtener_Municipios_Sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Estado", sIdEstado));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(gdtAuxiliar);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return gdtAuxiliar;
    }

    /// <summary>
    /// Devuelve una parte del catalogo de estados, filtrando por el país indicado
    /// </summary>
    /// <param name="idPais">ID del país para el cual se quiere recuperar sus estados</param>
    /// <returns>DataTable con los estados filtrados por país</returns>
    public static DataTable fnLlenarDropEstados(string psIdPais)
    {
        //giSql = fnCrearConexion("conRecepcionProveedores");
        //gdtAuxiliar = new DataTable();

        //try
        //{
        //    giSql.AgregarParametro("nId_Pais", psIdPais);
        //    giSql.Query("usp_Con_Obtener_Estados_Sel", true, ref gdtAuxiliar);
        //}
        //catch (SqlException ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //}

        //return gdtAuxiliar;

        DataTable gdtAuxiliar = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection

            (Utilerias.Encriptacion.Base64.DesencriptarBase64

            (ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Con_Obtener_Estados_Sel";
                    cmd.Parameters.Add(new SqlParameter("nId_Pais", psIdPais));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(gdtAuxiliar);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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

        //string nRetorno = string.Empty;

        //try
        //{
        //    Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

        //    conexion.AgregarParametro("sParametro", sParametro);

        //    nRetorno = (string)conexion.TraerEscalar("usp_Ctp_BuscarParametro_Sel", true);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}

        //return nRetorno;


        string nRetorno = string.Empty;

        try
        {
            using (SqlConnection con = new SqlConnection
            (Utilerias.Encriptacion.Base64.DesencriptarBase64
            (ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_BuscarParametro_Sel";
                    cmd.Parameters.Add(new SqlParameter("sParametro", sParametro));
                    nRetorno = Convert.ToString(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return nRetorno;

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

    /// <summary>
    /// Encargado de validar valor existe en la base de Datos
    /// </summary>
    /// <param name="sValor">valor a evaluar</param>
    /// <param name="expresion">expresion regular</param>
    /// <returns>retorna si es verdadero</returns>
    public static bool fnValidaExiste(string sCampo, string sTabla, string sValor)
    {
        //giSql = clsComun.fnCrearConexion("conRecepcionProveedores");
        //DataTable gdtAuxiliar = new DataTable("Parametros");
        //giSql.AgregarParametro("@sCampo", sCampo);
        //giSql.AgregarParametro("@tablename", sTabla);
        //giSql.AgregarParametro("@sValor", sValor);
        //giSql.Query("usp_ses_Existe_Valor_Sel", true, ref gdtAuxiliar);
        //if (gdtAuxiliar.Rows.Count == 0)
        //{
        //    return false;
        //}
        //else
        //{
        //    return true;
        //}

        bool res = false;
        DataTable gdtAuxiliar = new DataTable("Parametros");
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_ses_Existe_Valor_Sel";

                    cmd.Parameters.Add(new SqlParameter("sCampo", sCampo));
                    cmd.Parameters.Add(new SqlParameter("tablename", sTabla));
                    cmd.Parameters.Add(new SqlParameter("sValor", sValor));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(gdtAuxiliar);
                    }

                    con.Close();
                    con.Dispose();

                    if (gdtAuxiliar.Rows.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    } 
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return res;

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
    /// Rcupera un parametro Booleano del web.config
    /// </summary>
    /// <param name="nombreParam">Nombre del parametro a recuperar</param>
    /// <returns></returns>
    public static bool fnObtenerParametroBool(string nombreParam, int usuario = 0)
    {
        string parametro = System.Configuration.ConfigurationManager.AppSettings[nombreParam].ToString();
        return Convert.ToBoolean(parametro);

    }

    /// <summary>
    /// Obtiene el valor correspondiente de un AppSetting almacenado en el web.config
    /// </summary>
    /// <param name="key">Nombre del valor a recuperar</param>
    /// <returns></returns>
    public static string fnObtenerSetting(string key)
    {
        string strRetVal = string.Empty;
        try
        {
            strRetVal = ConfigurationManager.AppSettings[key];
        }
        catch { }
        return strRetVal;
    }

    public static int fnObtenerIdPerfil(string sDesc)
    {
        //int res = 0;
        //try
        //{

        //    giSql = clsComun.fnCrearConexion("conRecepcionProveedores");
            

        //    giSql.AgregarParametro("@sDesc", sDesc);

        //    res =Convert.ToInt32(giSql.TraerEscalar("usp_Obtener_IdPerfil_sel", true));
        //}
        //catch (Exception)
        //{
        //    return 0;
        //}
        //return res;

        int res = 0;
        try
        {
            using (SqlConnection con = new SqlConnection

            (Utilerias.Encriptacion.Base64.DesencriptarBase64

            (ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Obtener_IdPerfil_sel";
                    cmd.Parameters.Add(new SqlParameter("@sDesc", sDesc));
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return res;
    }

}

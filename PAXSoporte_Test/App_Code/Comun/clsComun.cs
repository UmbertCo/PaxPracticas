using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Clase encargada de proporcionar funciones de ayuda a todas las demás clases.
/// </summary>
public class clsComun
{
    private static DataTable gdtAuxiliar;

    /// <summary>
    /// Utileria para mostrar mensaje al estilo JQuery
    /// </summary>
    /// <param name="pPagina">La página que llama al mensaje</param>
    /// <param name="psMensajeError">El mensaje a mostrar</param>
    public static void fnMostrarError(System.Web.UI.Page pPagina, string psMensajeError)
    {
        Label lbl = (Label)pPagina.Master.FindControl("lblErrorGenerico");
        // lbl.Text = psMensajeError;
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnNuevaPistaAuditoria", "clsComun");
        }
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
    /// <param name="psParametro">Nombre del parametros a buscar</param>
    /// <returns>Cadena con le valor del parámetro</returns>
    public static string fnObtenerParamentro(string psParametro)
    {
        string nRetorno = string.Empty;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_BuscarParametro_Sel";
                    cmd.Parameters.Add(new SqlParameter("sParametro", psParametro));
                    nRetorno = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerParamentro", "clsComun");
            }
        }
        return nRetorno;
    }

    /// <summary>
    /// Encargado de validar si la expreson es verdadera
    /// </summary>
    /// <param name="psValor">valor a evaluar</param>
    /// <param name="psExpresion">expresion regular</param>
    /// <returns>retorna si es verdadero</returns>
    public static bool fnValidaExpresion(string psValor, string psExpresion)
    {
        bool bRetorno = false;

        if (Regex.IsMatch(psValor, psExpresion))
        {
            bRetorno = true;
        }

        return bRetorno;
    }

}
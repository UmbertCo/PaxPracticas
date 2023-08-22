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


/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{

    private static InterfazSQL giSql;
    private static DataTable gdtAuxiliar;
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
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }
    /// <summary>
    /// Utileria para mostrar mensaje al estilo JQuery
    /// </summary>
    /// <param name="pPagina">La página que llama al mensaje</param>
    /// <param name="psMensaje">El mensaje a mostrar</param>
    public static void fnMostrarMensaje(Page pPagina, string psMensaje)
    {
        //fnMostrarMensaje(pPagina, psMensaje, Resources.resCorpusCFDIEs.varAceptar);
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
            "alert('" + psMensaje + "', '" + psTitulo + "');", true);
    }

}
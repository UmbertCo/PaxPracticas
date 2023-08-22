using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for clsErrorLog
/// </summary>
public class clsErrorLog
{
	public clsErrorLog()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private static clsInicioSesionUsuario gUsuario;

    /// <summary>
    /// Definición general por grupos de errores para facilitar la comprensión del error
    /// </summary>
    public enum TipoErroresLog
    {
        Datos,          //Para los errores provocados por datos erroneos
        Referencia,     //Para los errores provocados por referencias nulas o fuera de indice
        Conexion,       //Para errores por falla o perdida de cualquier tipo de conexión
        BaseDatos,      //Errores devueltos por SQL
        Email,          //Error de conexion servitor smtp.
        PDF             //Error durante la generación del PDF
    }

    /// <summary>
    /// Registra una nueva entrada de error en la base de datos con información del usuairo y el módulo.
    /// </summary>
    /// <param name="pExcepcion">La excepción generada por .NET</param>
    /// <param name="pTipoError">El tipo de error que se generó</param>
    /// <param name="psMetodo">Metodo donde se genero el error</param>
    /// <param name="psModulo">Modulo donde se genero el error</param>
    public static void fnNuevaEntrada(Exception pExcepcion, TipoErroresLog pTipoError, string psMetodo, string psModulo)
    {
        fnNuevaEntrada(pExcepcion, pTipoError, psMetodo, psModulo, 0);
    }

    /// <summary>
    /// Registra una nueva entrada de error en la base de datos con información del usuairo y el módulo.
    /// </summary>
    /// <param name="pExcepcion">La excepción generada por .NET</param>
    /// <param name="pTipoError">El tipo de error que se generó</param>
    /// <param name="psMetodo">Metodo donde se genero el error</param>
    /// <param name="psModulo">Modulo donde se genero el error</param>
    /// <param name="psObservaciones">Mensaje personal en código definido por el desarrollador para aclarar dudas sobre el error</param>
    /// <param name="pnIdUsuario">ID de usuario</param>
    public static void fnNuevaEntrada(Exception pExcepcion, TipoErroresLog pTipoError, string psMetodo, string psModulo, int pnIdUsuario)
    {
        string sObservaciones = string.Empty;
        string sUbicacion = string.Empty;
        try
        {
            if (pnIdUsuario.Equals(0))
            {
                gUsuario = (clsInicioSesionUsuario)System.Web.HttpContext.Current.Session["objUsuario"];
                pnIdUsuario = gUsuario.nIdUsuario;
            }
        }
        catch (Exception)
        {

        }

        sUbicacion = "Ubicado en Pagina Gratuita";
        sObservaciones = string.Format("{0} | Mensaje Error:{1}", sUbicacion, pExcepcion.Message);

        psModulo = (psModulo.Length > 49 ? psModulo.Substring(0, 49) : psModulo);
        psMetodo = (psMetodo.Length > 19 ? psMetodo.Substring(0, 19) : psMetodo);
        sObservaciones = (sObservaciones.Length > 3999 ? sObservaciones.Substring(0, 3999) : sObservaciones);
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Registrar_Error_Ins";
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", pnIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("sMensaje", pExcepcion.Message));
                    cmd.Parameters.Add(new SqlParameter("sModulo", psModulo));
                    cmd.Parameters.Add(new SqlParameter("sMetodo", psMetodo));
                    cmd.Parameters.Add(new SqlParameter("sTipo_Error", pTipoError.ToString()));
                    cmd.Parameters.Add(new SqlParameter("sObservaciones", sObservaciones));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            fnEntradaExtra(ex.Message, pExcepcion.Message);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="psMensajeError"></param>
    /// <param name="psErrorMensajeOriginal"></param>
    private static void fnEntradaExtra(string psMensajeError, string psErrorMensajeOriginal)
    {
        if (!EventLog.SourceExists("Portal33Cobro"))
        {
            EventLog.CreateEventSource("Portal33Cobro", "Portal33CobroLog");
        }

        EventLog PaxEventLog = new EventLog();
        PaxEventLog.Source = "Portal33Cobro";
        PaxEventLog.WriteEntry(string.Format("Mensaje de Error: {0}, Mensaje Original: {1}", psMensajeError, psErrorMensajeOriginal));
    }
}
using System;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;

/// <summary>
/// Clase encargada de manejar las entradas al log de errores
/// </summary>
public class clsErrorLog
{
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
    /// Metodo que se encarga de registrar un error en caso de no poder registrarlo en BD
    /// </summary>
    /// <param name="psMensajeError">Mensaje al registrar el error</param>
    /// <param name="psErrorMensajeOriginal">Mensaje original</param>
    private static void fnEntradaExtra(string psMensajeError, string psErrorMensajeOriginal)
    {
        if (!EventLog.SourceExists("PortalSoporte_Test"))
        {
            EventLog.CreateEventSource("PortalSoporte_Test", "PortalSoporte_Test");
        }

        EventLog PaxEventLog = new EventLog();
        PaxEventLog.Source = "PortalSoporte_Test";
        PaxEventLog.WriteEntry(string.Format("Mensaje de Error: {0}, Mensaje Original: {1}", psMensajeError, psErrorMensajeOriginal));
    }

    /// <summary>
    /// Registra una nueva entrada de error en la base de datos con información del usuairo y el módulo.
    /// </summary>
    /// <param name="pExcepcion">La excepción generada por .NET</param>
    /// <param name="pTipoError">El tipo de error que se generó</param>
    /// <param name="psMetodo">Metodo donde se genero el error</param>
    /// <param name="psModulo">Modulo donde se genero el error</param>
    /// <param name="psObservaciones">Mensaje personal en código definido por el desarrollador para aclarar dudas sobre el error</param>
    /// /// <param name="pnIdUsuario">ID de usuario, si es cero no se toma en cuenta</param>
    public static void fnNuevaEntrada(Exception pExcepcion, TipoErroresLog pTipoError, string psMetodo, string psModulo)
    {
        int nId_Usuario = 0;
        string sObservaciones = string.Empty;
        string sUbicacion = string.Empty;
        try
        {
            gUsuario = (clsInicioSesionUsuario)System.Web.HttpContext.Current.Session["objUsuario"];
            nId_Usuario = gUsuario.id_usuario;
        }
        catch (Exception)
        {

        }

        sUbicacion = "Ubicado en PortalAlSuperClientes";
        sObservaciones = string.Format("{0} | Mensaje Error:{1}", sUbicacion, pExcepcion.Message);
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Registrar_Error_Ins";
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nId_Usuario));
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

   
}
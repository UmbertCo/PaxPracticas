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
/// Clase encargada de manejar las entradas al log de errores
/// </summary>
public class clsErrorLog
{
    private static clsInicioSesionUsuario gUsuario;
    
    public clsErrorLog()
    {
    }

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
    /// Registra una nueva entrada de error en la base de datos con información del usuairo y el módulo
    /// </summary>
    /// <param name="pExcepcion">La excepción generada por .NET</param>
    /// <param name="pTipoError">El tipo de error que se generó</param>
    public static void fnNuevaEntrada(Exception pExcepcion, TipoErroresLog pTipoError)
    {
        fnNuevaEntrada(pExcepcion, pTipoError, string.Empty, 0);
    }

    /// <summary>
    /// Registra una nueva entrada de error en la base de datos con información del usuairo y el módulo.
    /// </summary>
    /// <param name="pExcepcion">La excepción generada por .NET</param>
    /// <param name="pTipoError">El tipo de error que se generó</param>
    /// <param name="psObservaciones">Mensaje personal en código definido por el desarrollador para aclarar dudas sobre el error</param>
    /// /// <param name="pnId_Usuario">ID de usuario, si es cero no se toma en cuenta</param>
    public static void fnNuevaEntrada(Exception pExcepcion, TipoErroresLog pTipoError, string psObservaciones, int pnId_Usuario)
    {
        string sUbicacion = string.Empty;
        string sMetodo = string.Empty;
        string sModulo = string.Empty;
        int nId_Usuario = pnId_Usuario;
        string[] asInfoLocalizacion = null;
        string sMetodos = string.Empty;
        string sInformacionAdicional = string.Empty;
        string sMensaje = string.Empty;
        if (pnId_Usuario == 0)
        {
            try
            {
                gUsuario = (clsInicioSesionUsuario)System.Web.HttpContext.Current.Session["objUsuario"];
                nId_Usuario = gUsuario.id_usuario;
            }
            catch (Exception)
            {
            }
        }

        sUbicacion = "Ubicado en Indicium Web Service";

        try
        {
            //sInformacionAdicional += string.Format("BE{0}", (pExcepcion.GetBaseException() == null ? string.Empty : pExcepcion.GetBaseException().ToString()));
            sInformacionAdicional += string.Format("ST:{0};", (pExcepcion.StackTrace == null ? "NA" : pExcepcion.StackTrace));
            sInformacionAdicional += string.Format("Mensaje:{0};", pExcepcion.Message);
            psObservaciones += sUbicacion + " - " + sInformacionAdicional;

            if (pExcepcion.GetBaseException().TargetSite != null)
            {
                //sUbicacion += " - " + (pExcepcion.GetBaseException() == null ? pExcepcion.StackTrace : pExcepcion.GetBaseException().ToString());

                if (pExcepcion.ToString().LastIndexOf('\\') > 0 && pExcepcion.ToString().LastIndexOf('\\') < pExcepcion.ToString().Length)
                    sModulo = pExcepcion.ToString().Substring(pExcepcion.ToString().LastIndexOf('\\'), pExcepcion.ToString().Length - pExcepcion.ToString().LastIndexOf('\\'));
                else if (!string.IsNullOrEmpty(pExcepcion.Source))
                    sModulo = pExcepcion.Source;
                else
                    sModulo = "Indefinido";

                if (pExcepcion.GetBaseException().TargetSite != null)
                    sMetodo = pExcepcion.GetBaseException().TargetSite.Name;
                else if (!string.IsNullOrEmpty(pExcepcion.Source))
                    sModulo = pExcepcion.Source;
                else
                    sMetodo = "Indefinido";

            }
            else if (pExcepcion.StackTrace != null)
            {
                string sStack = pExcepcion.StackTrace;
                string sLocalizacion = sStack.Substring(0, sStack.IndexOf('('));
                asInfoLocalizacion = sLocalizacion.Replace("at", string.Empty).Trim().Split('.');

                sModulo = asInfoLocalizacion[0];
                sMetodo = asInfoLocalizacion[1];
            }
            else
            {
                if (!string.IsNullOrEmpty(pExcepcion.Source) && pExcepcion.Source.Contains("|"))
                    sMetodo = pExcepcion.Source.Split('|')[1];
                else
                    sMetodo = "Indefinido";

                if (!string.IsNullOrEmpty(pExcepcion.Source) && pExcepcion.Source.Contains("|"))
                    sModulo = pExcepcion.Source.Split('|')[0];
                else
                    sModulo = "Indefinido";
            }
        }
        catch (Exception ex)
        {
            sModulo = "clsErrorLog";
            sMetodo = "fnNuevaEntrada";
            psObservaciones = sUbicacion + " - " + pExcepcion.Message + " - " + ex.Message;
        }

        sModulo = (sModulo.Length > 49 ? sModulo.Substring(0, 49) : sModulo);
        sMetodo = (sMetodo.Length > 19 ? sMetodo.Substring(0, 19) : sMetodo);
        sMensaje = (pExcepcion.Message.Length > 299 ? pExcepcion.Message.Substring(0, 299) : pExcepcion.Message);
        psObservaciones = (psObservaciones.Length > 3999 ? psObservaciones.Substring(0, 3999) : psObservaciones);

        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_Ctp_Registrar_Error_Ins";

                    scoComando.Parameters.AddWithValue("nId_Usuario", nId_Usuario);
                    scoComando.Parameters.AddWithValue("sMensaje", sMensaje);

                    scoComando.Parameters.AddWithValue("sModulo", sModulo);
                    scoComando.Parameters.AddWithValue("sMetodo", sMetodo);

                    scoComando.Parameters.AddWithValue("sTipo_Error", pTipoError.ToString());
                    if (!string.IsNullOrEmpty(psObservaciones))
                        scoComando.Parameters.AddWithValue("sObservaciones", psObservaciones);

                    scoComando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //Si falla no se realiza acción alguna
                fnEntradaExtra(ex.Message, pExcepcion.Message);
            }
        }
    }

    /// <summary>
    /// Función que se encarga de guardar un error en el EventViewer en caso de que la BD no se pueda insertar el error
    /// </summary>
    /// <param name="psMensajeError">Error generado</param>
    /// <param name="psErrorMensajeOriginal">Error original</param>
    private static void fnEntradaExtra(string psMensajeError, string psErrorMensajeOriginal)
    {
        if (!EventLog.SourceExists("PAXIndiciumWebService"))
        {
            EventLog.CreateEventSource("PAXIndiciumWebService", "PAXIndiciumWebServiceLog");
        }

        EventLog PaxEventLog = new EventLog();
        PaxEventLog.Source = "PAXIndiciumWebService";
        PaxEventLog.WriteEntry(string.Format("Mensaje de Error: {0}, Mensaje Original: {1}", psMensajeError, psErrorMensajeOriginal));
    }
}
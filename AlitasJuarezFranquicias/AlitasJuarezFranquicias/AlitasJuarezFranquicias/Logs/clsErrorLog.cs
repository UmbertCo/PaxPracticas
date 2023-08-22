using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Web.UI;

/// <summary>
/// Clase encargada de manejar las entradas al log de errores
/// </summary>
public class clsErrorLog
{
	private static clsInicioSesionUsuario gUsuario;
	private static InterfazSQL giSql;

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
        int nId_Usuario = pnId_Usuario;

        if(pnId_Usuario == 0)
		    gUsuario = (clsInicioSesionUsuario)System.Web.HttpContext.Current.Session["objUsuario"];

		giSql = clsComun.fnCrearConexion("conControl");
		string[] infoLoc = null;

		try
		{
			//Obtenemos la clase y método de origen a través del StackTrace
			string st = pExcepcion.StackTrace;
			string sub = st.Substring(0, st.IndexOf('('));
			infoLoc = sub.Replace("at", string.Empty).Trim().Split('.');
		}
		catch
		{
			//Si algo falla los definimos como indefinidos
			infoLoc = new string[] {"Indefinido", "Indefinido"};
		}

		try
		{
			giSql.AgregarParametro("@nId_Usuario", nId_Usuario);
			giSql.AgregarParametro("@sMensaje", pExcepcion.Message);
			giSql.AgregarParametro("@sModulo", infoLoc[0]);
			giSql.AgregarParametro("@sMetodo", infoLoc[1]);
			giSql.AgregarParametro("@sTipo_Error", pTipoError.ToString());
			if(!string.IsNullOrEmpty(psObservaciones))
				giSql.AgregarParametro("@sObservaciones", psObservaciones);

			giSql.NoQuery("usp_Ctp_Registrar_Error_Ins", true);
		}
		catch
		{
			//Si falla no se realiza acción alguna
		}
	}
}
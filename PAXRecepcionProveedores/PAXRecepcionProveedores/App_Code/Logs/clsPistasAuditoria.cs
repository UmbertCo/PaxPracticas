using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Encargada de registrar las pistas de auditoria.
/// </summary>
public class clsPistasAuditoria
{
    /// <summary>
    /// Encargada de registrar las Pistas de Auditoria
    /// </summary>
    /// <param name="nId_Usuario">id del usuario</param>
    /// <param name="dFecha_Accion">fecha de generacion</param>
    /// <param name="sAccion">accion del evento a capturar</param>
    /// <returns></returns>
    public static bool fnGenerarPistasAuditoria(int? nId_Usuario, DateTime dFecha_Accion, string sAccion)
    {
        bool bRetorno = false;

        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conRecepcionProveedores");

            conexion.AgregarParametro("nId_Usuario", nId_Usuario);
            conexion.AgregarParametro("dFecha_Accion", dFecha_Accion);
            conexion.AgregarParametro("sAccion", sAccion);

            conexion.NoQuery("usp_Ctp_Registrar_Pistas_Ins", true);
            bRetorno = true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return bRetorno;
    }
}
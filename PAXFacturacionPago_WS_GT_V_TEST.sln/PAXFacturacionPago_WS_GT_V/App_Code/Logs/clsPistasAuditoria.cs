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
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conControl");

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

    /// <summary>
    /// Inserta acuse de la validacion de los comprobantes.
    /// </summary>
    /// <param name="id_usuario"></param>
    /// <param name="fecha_validacion"></param>
    /// <param name="uuid"></param>
    /// <param name="serie"></param>
    /// <param name="folio"></param>
    /// <param name="rfc_emisor"></param>
    /// <param name="rfc_receptor"></param>
    /// <param name="version"></param>
    /// <param name="estatus_validacion"></param>
    /// <returns></returns>
    public static bool fnInsertaValidacion(int id_usuario, DateTime fecha_validacion, string uuid, string serie, string folio,
                                            string rfc_emisor, string rfc_receptor, string version, string estatus_validacion)
    {
        bool bRetorno = false;

        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conControl");

            conexion.AgregarParametro("id_usuario", id_usuario);
            conexion.AgregarParametro("fecha_validacion", fecha_validacion);
            conexion.AgregarParametro("uuid", uuid);
            conexion.AgregarParametro("serie", serie);
            conexion.AgregarParametro("folio", folio);
            conexion.AgregarParametro("rfc_emisor", rfc_emisor);
            conexion.AgregarParametro("rfc_receptor", rfc_receptor);
            conexion.AgregarParametro("version", version);
            conexion.AgregarParametro("estatus_validacion", estatus_validacion);


            conexion.NoQuery("usp_Cfd_InsertaValidacion_Ins", true);
            bRetorno = true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return bRetorno;
    }
}
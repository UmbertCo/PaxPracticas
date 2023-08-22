using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;

/// <summary>
/// Clase encargada de manejar las entradas a la lista de monitoreos
/// </summary>
public class clsMonitoreo
{

    private InterfazSQL giSql;
    private string conCuenta = "conConfiguracion";

	public clsMonitoreo()
	{
	}

    /// <summary>
    /// verifica la extension del archivo a enviar por correo
    /// 
    /// </summary>
    /// <param name="psArchivo">Nombre del archivo</param>
    public bool fnverificaarchivo(string psArchivo)
    {
        bool valor = false;

        try
        {

            string Extensiones = clsComun.ObtenerParamentro("Extensiones");
            string[] Extension = Extensiones.Split(',');

            string[] psExtension = null;
            psExtension = psArchivo.Split('.');
            string Ext = null;
            Ext = psExtension[1];
            foreach (string ExVal in Extension)
            {
                if (ExVal.Trim() == Ext)
                {
                    valor = true;
                    return valor;
                }
            }

        }
        catch (Exception)
        {
            valor = false;
        }
        return valor;
    }

    /// <summary>
    /// verifica el tamaño del archivo
    /// 
    /// </summary>
    /// <param name="psArchivo">tamaño del archivo en KB</param>
    public bool fnVerificaTamanioMax(int psTamanio)
    {
        bool valor = false;
        try
        {

            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFile"));

            if (psTamanio <= psMaximo)
            {
                valor = true;
                return valor;
            }
            else
            {
                valor = false;
                return valor;
            }
        }
        catch (Exception)
        {
            valor = false;
        }
        return valor;
    }


    /// <summary>
    /// funcion encargada de registrar el monitoreo.
    /// </summary>
    /// <param name="sddlUbicacion"></param>
    /// <param name="sddlDispositivo"></param>
    /// <param name="sddlOrigen"></param>
    /// <param name="stxtPromedio"></param>
    /// <param name="stxtMaximo"></param>
    /// <param name="stxtCapacidad"></param>
    /// <param name="adjunto"></param>
    /// <param name="stxtFechaIni"></param>
    /// <param name="stxtFechaFin"></param>
    /// <param name="nId_Usuario"></param>
    /// <param name="fechaCaptura"></param>
    /// <param name="estatus"></param>
    /// <returns></returns>
    public int fnGuardarMonitoreo(string sddlUbicacion, string sddlDispositivo, string sddlOrigen,
                                      string stxtPromedio, string stxtMaximo, string stxtCapacidad,byte [] adjunto,
                                      DateTime stxtFechaIni, DateTime stxtFechaFin, int nId_Usuario,
                                      DateTime fechaCaptura,char estatus)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);

        giSql.AgregarParametro("@sddlUbicacion", sddlUbicacion);
        giSql.AgregarParametro("@sddlDispositivo", sddlDispositivo);
        giSql.AgregarParametro("@sddlOrigen", sddlOrigen);
        giSql.AgregarParametro("@stxtFechaIni", stxtFechaIni);
        giSql.AgregarParametro("@stxtFechaFin", stxtFechaFin);
        giSql.AgregarParametro("@stxtPromedio", stxtPromedio);
        giSql.AgregarParametro("@stxtMaximo", stxtMaximo);
        giSql.AgregarParametro("@stxtCapacidad", stxtCapacidad);
        giSql.AgregarParametro("@iadjunto", adjunto);
        giSql.AgregarParametro("@nId_Usuario", nId_Usuario);
        giSql.AgregarParametro("@fecha_captura", fechaCaptura);
        giSql.AgregarParametro("@estatus", estatus);

        return giSql.NoQuery("usp_Ctp_Monitoreo_Registro_Ins", true);
    }

    /// <summary>
    /// Funcion encargada de recuperar la listas de usuarios
    /// </summary>
    /// <returns></returns>
    public DataTable fnRecuperUsuariosMonitoreo()
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        DataTable dtAuxiliar = new DataTable();

        giSql.Query("usp_Ctp_Monitoreo_Usuarios_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// recupera la informacion filtrada de la consulta.
    /// </summary>
    /// <returns></returns>
    public DataTable fnConsultaMonitoreo(string sUbicacion,string sDispositivo, string sFechaIni,
                                         string sFechaFIn,string sUsuario)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        DataTable dtAuxiliar = new DataTable();

        giSql.AgregarParametro("@psUbicacion", sUbicacion);
        giSql.AgregarParametro("@psDispositivo", sDispositivo);
        giSql.AgregarParametro("@sFechaIni", sFechaIni);
        giSql.AgregarParametro("@sFechaFin", sFechaFIn);
        giSql.AgregarParametro("@sUsuario", sUsuario);
        //giSql.AgregarParametro("@sFechaUsuIni", sFechaUsuIni);
        //giSql.AgregarParametro("@sFechaUsuFin", sFehcaUsuFin);

        giSql.Query("usp_Ctp_Monitoreo_Consulta_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Recupera la imagen que tiene el registro de monitore.
    /// </summary>
    /// <param name="sIdCfd"></param>
    /// <returns></returns>
    public DataTable fnRecuperaImagen(string sIdCfd)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        DataTable dtAuxiliar = new DataTable();

        giSql.AgregarParametro("@nId_Monitoreo", sIdCfd);
        giSql.Query("usp_Ctp_Monitoreo_Imagen_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Recuepera la existencia de registros por tipo en el dia.
    /// </summary>
    /// <param name="sIdCfd"></param>
    /// <returns></returns>
    public DataSet fnRecuperaExistentes(string sUbicacion,string sDispositivo)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        DataSet dtAuxiliar = new DataSet();

        giSql.AgregarParametro("@sUbicacion", sUbicacion);
        giSql.AgregarParametro("@sDispositivo", sDispositivo);
        giSql.Query("usp_Ctp_Monitoreo_Existentes_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Recupera alarmas.
    /// </summary>
    /// <returns></returns>
    public DataSet fnRecuperaAlarmas()
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        DataSet dtAuxiliar = new DataSet();


        giSql.Query("usp_Ctp_Monitoreo_Alarmas_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Clase encargada de manejar las entradas a la lista de monitoreos
/// </summary>
public class clsMonitoreo
{
    private string conCuenta = "conConfiguracion";

	public clsMonitoreo()
	{
	}

    /// <summary>
    /// recupera la informacion filtrada de la consulta.
    /// </summary>
    /// <returns></returns>
    public DataTable fnConsultaMonitoreo(string psUbicacion, string psDispositivo, string psFechaIni,
                                         string psFechaFIn, string psUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Monitoreo_Consulta_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("psUbicacion", psUbicacion));
                    cmd.Parameters.Add(new SqlParameter("psDispositivo", psDispositivo));
                    cmd.Parameters.Add(new SqlParameter("sFechaIni", psFechaIni));
                    cmd.Parameters.Add(new SqlParameter("sFechaFin", psFechaFIn));
                    cmd.Parameters.Add(new SqlParameter("sUsuario", psUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
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
                                      string stxtPromedio, string stxtMaximo, string stxtCapacidad, byte[] adjunto,
                                      DateTime stxtFechaIni, DateTime stxtFechaFin, int nId_Usuario,
                                      DateTime fechaCaptura, char estatus)
    {
        Int32 nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Monitoreo_Registro_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sddlUbicacion", sddlUbicacion));
                    cmd.Parameters.Add(new SqlParameter("sddlDispositivo", sddlDispositivo));
                    cmd.Parameters.Add(new SqlParameter("sddlOrigen", sddlOrigen));
                    cmd.Parameters.Add(new SqlParameter("stxtFechaIni", stxtFechaIni));
                    cmd.Parameters.Add(new SqlParameter("stxtFechaFin", stxtFechaFin));
                    cmd.Parameters.Add(new SqlParameter("stxtPromedio", stxtPromedio));
                    cmd.Parameters.Add(new SqlParameter("stxtMaximo", stxtMaximo));
                    cmd.Parameters.Add(new SqlParameter("stxtCapacidad", stxtCapacidad));
                    cmd.Parameters.Add(new SqlParameter("iadjunto", adjunto));
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nId_Usuario));
                    cmd.Parameters.Add(new SqlParameter("fecha_captura", fechaCaptura));
                    cmd.Parameters.Add(new SqlParameter("estatus", estatus));

                    con.Open();
                    nResultado = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return nResultado;
    }

    /// <summary>
    /// Recupera alarmas.
    /// </summary>
    /// <returns></returns>
    public DataSet fnRecuperaAlarmas()
    {
        DataSet dsResultado = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Monitoreo_Alarmas_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dsResultado;

    }

    /// <summary>
    /// Recuepera la existencia de registros por tipo en el dia.
    /// </summary>
    /// <param name="psUbicacion">Ubicacion</param>
    /// <param name="psDispositivo">Dispositivo</param>
    /// <returns></returns>
    public DataSet fnRecuperaExistentes(string psUbicacion, string psDispositivo)
    {
        DataSet dsResultado = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Monitoreo_Existentes_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sUbicacion", psUbicacion));
                    cmd.Parameters.Add(new SqlParameter("sDispositivo", psDispositivo));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dsResultado;
    }

    /// <summary>
    /// Recupera la imagen que tiene el registro de monitore.
    /// </summary>
    /// <param name="psIdCfd">ID CFD</param>
    /// <returns></returns>
    public DataTable fnRecuperaImagen(string psIdCfd)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Monitoreo_Imagen_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Monitoreo", psIdCfd));
                    

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Funcion encargada de recuperar la listas de usuarios
    /// </summary>
    /// <returns></returns>
    public DataTable fnRecuperUsuariosMonitoreo()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Monitoreo_Usuarios_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
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
}
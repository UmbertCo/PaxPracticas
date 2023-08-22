using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsConfiguracionCreditos
/// </summary>
public class clsConfiguracionCreditos
{
    private string conConfig = "conConfiguracion";

    /// <summary>
    /// Metódo que se encarga de actualizar los créditos.
    /// </summary>
    /// <param name="pnIdEstructura">ID Estructura</param>
    /// <param name="pnCreditos">Créditos</param>
    /// <param name="psEstatus">Estatus</param>
    /// <param name="pdFechaCompra">Fecha de compra</param>
    /// <param name="pnPrecio">Precio</param>
    /// <param name="psServicio">Servicio</param>
    public void fnActualizaCreditos(int pnIdEstructura, double pnCreditos, char psEstatus, DateTime pdFechaCompra, double pnPrecio, string psServicio)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                DateTime dFechaVigencia = pdFechaCompra;
                dFechaVigencia = dFechaVigencia.AddYears(2);

                using (SqlCommand cmd = new SqlCommand("usp_Con_ActualizaCreditos_upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Estructura", pnIdEstructura));
                    cmd.Parameters.Add(new SqlParameter("nCreditos", pnCreditos));
                    cmd.Parameters.Add(new SqlParameter("sEstatus", psEstatus));
                    cmd.Parameters.Add(new SqlParameter("sFechaCompra", pdFechaCompra));
                    cmd.Parameters.Add(new SqlParameter("sFechaVigencia", dFechaVigencia));
                    cmd.Parameters.Add(new SqlParameter("nPrecioUnit", pnPrecio));
                    cmd.Parameters.Add(new SqlParameter("sServicio", psServicio));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
    }

    /// <summary>
    /// Función que se encarga de actualizar los servicios de un Estructura determinada
    /// </summary>
    /// <param name="pnIdEstructura">ID Estructura</param>
    /// <param name="psServicio">Servicio</param>
    public void fnActualizaServicios(int pnIdEstructura, string psServicio)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ActualizaServicios_upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Estructura", pnIdEstructura));
                    cmd.Parameters.Add(new SqlParameter("sServicio", psServicio));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
    }

    /// <summary>
    /// Función que se encarga de obtener la Clave del Usuario de un usuario determinado
    /// </summary>
    /// <param name="pnIdUsuario">ID Usuario</param>
    /// <returns></returns>
    public string fnObtenerClaveUsuario(int pnIdUsuario)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ObtenerClaveUsuario_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdUsuario", pnIdUsuario));

                    con.Open();
                    sResultado = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return sResultado;
    }

    /// <summary>
    /// Función que obtiene un reporte de acumulado por Estructuras del Usuario
    /// </summary>
    /// <param name="pnIdUsuario"></param>
    /// <returns></returns>
    public DataTable fnObtenerSucursalesCreditosAcumulados(int pnIdUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ObtenerSucursalesReporteAcumulado_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdUsuario", pnIdUsuario));

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
    /// Función que obtiene un reporte del acumulado de créditos.
    /// </summary>
    /// <param name="pnIdUsuario">ID Usuario</param>
    /// <param name="pnIdEstructura">ID Estructura</param>
    /// <param name="psFechaCompra">Fecha de Compra</param>
    /// <param name="psFechaVigencia">Fecha Vigencia</param>
    /// <param name="pnPrecioUnitario">Precio Unitario</param>
    /// <returns></returns>
    public DataTable fnObtieneReporteAcumulado(int pnIdUsuario, int pnIdEstructura, string psFechaCompra, string psFechaVigencia, double pnPrecioUnitario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ReporteCreditosAcumulado_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdUsuario", pnIdUsuario));

                    if (pnIdEstructura != 0)
                        cmd.Parameters.Add(new SqlParameter("nIdEstructura", pnIdEstructura));

                    if (psFechaCompra != "")
                    {
                        DateTime dFechaCompra = Convert.ToDateTime(psFechaCompra);
                        cmd.Parameters.Add(new SqlParameter("sFechaCompra", dFechaCompra));
                    }

                    if (psFechaVigencia != "")
                    {
                        DateTime dFechaVigencia = Convert.ToDateTime(psFechaVigencia);
                        cmd.Parameters.Add(new SqlParameter("sFechaVigencia", dFechaVigencia));
                    }

                    if (pnPrecioUnitario != 0)
                        cmd.Parameters.Add(new SqlParameter("nPrecioUnitario", pnPrecioUnitario));

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
    /// Función que obtiene los servicios configurados 
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtieneServicios()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RecuperaServiciosCreditos_sel", con))
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
    /// Función que obtiene un reporte del estado de cuenta
    /// </summary>
    /// <param name="pnIdEstructura">ID Estructura</param>
    /// <param name="psFechaInicio">Fecha Inicio</param>
    /// <param name="psFechaFin">Fecha Fin</param>
    /// <returns></returns>
    public DataTable fnObtieneReporteHistorico(int pnIdEstructura, string psFechaInicio, string psFechaFin)
    {
         DataTable dtResultado = new DataTable();

         using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
         {
             try
             {
                 using (SqlCommand cmd = new SqlCommand("usp_Con_ReporteEstadodeCuentaHistorico_sel", con))
                 {
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.Add(new SqlParameter("nIdEstructura", pnIdEstructura));

                     if (psFechaInicio != "")
                     {
                         DateTime dFechaIn = Convert.ToDateTime(psFechaInicio);
                         cmd.Parameters.Add(new SqlParameter("sFechaInicio", dFechaIn));
                     }

                     if (psFechaFin != "")
                     {
                         DateTime dFechafin = Convert.ToDateTime(psFechaFin);
                         cmd.Parameters.Add(new SqlParameter("sFechaFin", dFechafin));
                     }
                     
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
    /// Función que obtiene los créditos de un usuario especifico
    /// </summary>
    /// <param name="clave_usuario">Clave de Usuario</param>
    /// <returns></returns>
    public DataSet fnRecuperaCreditos(string clave_usuario)
    {
        DataSet dsResultado = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RecuperaCreditos_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sClave_Usuario", clave_usuario));

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
    /// Función que se encarga de obtener el precio de un servicio en especifico
    /// </summary>
    /// <param name="pnId_Servicio">ID del servicio</param>
    /// <returns></returns>
    public double fnRecuperaPrecioServicio(int pnId_Servicio)
    {
        double nResultado = 0;
        using (SqlConnection scConexion = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand scoComando = new SqlCommand("usp_Con_RecuperaPrecioServicio_sel", scConexion))
                {
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.Parameters.AddWithValue("nIdServicio", pnId_Servicio);

                    scConexion.Open();
                    nResultado = Convert.ToDouble(scoComando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                scConexion.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return nResultado;
    }
}
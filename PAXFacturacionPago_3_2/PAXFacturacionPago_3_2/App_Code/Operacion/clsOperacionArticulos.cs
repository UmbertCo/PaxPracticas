using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsOperacionArticulos
/// </summary>
public class clsOperacionArticulos
{
    private string conCuenta = "conControl";

    /// <summary>
    /// Función que se encarga de guardar un articulo
    /// </summary>
    /// <param name="pnId_Articulo">ID Articulo</param>
    /// <param name="psDescripcion">Descripcion</param>
    /// <param name="psMedida">Medida</param>
    /// <param name="pnPrecio">Precio</param>
    /// <param name="psIva">IVA</param>
    /// <param name="pnIeps">IEPS</param>
    /// <param name="pnIsr">ISR</param>
    /// <param name="pnIvaRetenido">IVA Retenido</param>
    /// <param name="psMoneda">Moneda</param>
    /// <param name="psEstatus">Estatus</param>
    /// <param name="pnIdEstructura">ID Estructura</param>
    /// <param name="psClave">Clave</param>
    /// <returns></returns>
    public int fnUpdateArticulo(int pnId_Articulo, string psDescripcion, string psMedida, double pnPrecio, string psIva, double pnIeps, 
        double pnIsr, double pnIvaRetenido, string psMoneda, string psEstatus, int pnIdEstructura, string psClave)
    {
        Int32 nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Articulo_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("idArticulo", pnId_Articulo));
                    cmd.Parameters.Add(new SqlParameter("sDescripcion", psDescripcion));
                    cmd.Parameters.Add(new SqlParameter("sMedida", psMedida));
                    cmd.Parameters.Add(new SqlParameter("nPrecio", pnPrecio));
                    if (!(psIva == null))
                        cmd.Parameters.Add(new SqlParameter("nIva", psIva));
                    cmd.Parameters.Add(new SqlParameter("nIeps", pnIeps));
                    cmd.Parameters.Add(new SqlParameter("nIsr", pnIsr));
                    cmd.Parameters.Add(new SqlParameter("nIvaRetenido", pnIvaRetenido));
                    cmd.Parameters.Add(new SqlParameter("sMoneda", psMoneda));
                    cmd.Parameters.Add(new SqlParameter("sEstatus", psEstatus));
                    cmd.Parameters.Add(new SqlParameter("nIdEstructura", pnIdEstructura));
                    cmd.Parameters.Add(new SqlParameter("nClave", psClave));

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
    /// Función que se encarga de dar de baja un articulo en especifico
    /// </summary>
    /// <param name="pId_Articulo">ID Articulo</param>
    /// <returns></returns>
    public bool fnBajaArticulo(int pId_Articulo)
    {
        bool bResultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Articulo_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("idArticulo", pId_Articulo));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                bResultado = true;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bResultado;
    }

    /// <summary>
    /// Función que obtiene los datos de un articulo en especifico
    /// </summary>
    /// <param name="pnIdArticulo">ID Articulo</param>
    /// <returns></returns>
    public DataTable fnObtieneArticulo(int pnIdArticulo)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Articulo_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("idArticulo", pnIdArticulo));

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
    /// Función que se encarga de obtener todos los articulos
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtieneArticulos()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Articulo_Sel_All", con))
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
    /// Función que obtiene los articulos de una Estructura especifica
    /// </summary>
    /// <param name="pnIdEstructura">ID Estructura</param>
    /// <returns></returns>
    public DataTable fnObtieneArticulosEstructura(int pnIdEstructura)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ArticuloEst_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdEstructura", pnIdEstructura));

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
    /// Trae la lista de articulos según la sucursal.
    /// <param name="pnId_Estructura">ID Estructura</param>
    /// <param name="psClave">Clave</param>
    /// <param name="psDescripcion">Descripcion</param>
    /// </summary>
    public DataTable fnLlenarGridArticulos(string pnId_Estructura, string psClave, string psDescripcion)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ArticulosSuc_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdEstructura", pnId_Estructura));
                    if (!string.IsNullOrEmpty(psClave))
                        cmd.Parameters.Add(new SqlParameter("sClave", psClave));
                    if (!string.IsNullOrEmpty(psDescripcion))
                        cmd.Parameters.Add(new SqlParameter("sDescripcion", psDescripcion));

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
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for clsConfiguracionAddenda
/// </summary>
public class clsConfiguracionAddenda
{   
    /// <summary>
    /// Metodo que se encarga de registrar la addenda relacionada a un comprobante, la estructura y el tipo de addenda configurada
    /// </summary>
    /// <param name="pnidCfd">ID CFD</param>
    /// <param name="pnidEstructura">ID Estructura</param>
    /// <param name="pAddenda">Addenda</param>
    /// <param name="pnidAddenda">ID Modulo</param>
    public void fnInsertaAddenda(int pnidCfd, int pnidEstructura, XmlDocument pAddenda, int pnidAddenda)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_InsertaAddenda_ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdCfd", pnidCfd));
                    cmd.Parameters.Add(new SqlParameter("nIdEstructura", pnidEstructura));
                    cmd.Parameters.Add(new SqlParameter("sAddenda", pAddenda.OuterXml));
                    cmd.Parameters.Add(new SqlParameter("nIdModulo", pnidAddenda));

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
    /// Función que obtiene la addenda de acuerdo a la estructura y al usuario
    /// </summary>
    /// <param name="pnidEstructura">ID de Estructura</param>
    /// <param name="pnidUsuario">ID de Usuario</param>
    /// <returns></returns>
    public DataTable fnObtieneAddendaConfiguracion(int pnidEstructura, int pnidUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_AddendaConfiguracion_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_estructura", pnidEstructura));
                    cmd.Parameters.Add(new SqlParameter("nId_usuario", pnidUsuario));

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
    /// Función que obtiene la Addenda de acuerdo al ID del CFD y el ID de la Estructura
    /// </summary>
    /// <param name="pnidCfd">ID CFD</param>
    /// <param name="pnidEstructura">ID Estructura</param>
    /// <returns></returns>
    public DataTable fnObtieneAddendaporIdCfd(int pnidCfd, int pnidEstructura)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_ObtieneAddenda_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdCfd", pnidCfd));
                    cmd.Parameters.Add(new SqlParameter("nIdEstructura", pnidEstructura));

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
    /// Función que obtiene el ID de la Estructura de acuerdo al ID del CFD timbrado
    /// </summary>
    /// <param name="pnidCfd">ID del CFD</param>
    /// <returns></returns>
    public int fnObtieneEstructuraCFD(int pnidCfd)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_Estructura_Sel_Cobro", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Cfd", pnidCfd));

                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
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
    /// Función que obtiene el la Addenda configurada de acuerdo al nombre
    /// </summary>
    /// <param name="psNombre">Nombre de la Addenda</param>
    /// <returns></returns>
    public int fnObtieneidAddenda(string psNombre)
    {
        Int32 nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_ObtieneIdAddendaConfiguracion_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nNombre", psNombre));

                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
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
    /// Función que obtiene el ID del Usuario de acuerdo al ID del Contribuyente
    /// </summary>
    /// <param name="pnIdContribuyente">ID Contribuyente</param>
    /// <returns></returns>
    public int fnObtieneIdUsuarioporContribuyente(int pnIdContribuyente)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_RecuperaUsuarioContribuyente_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdContribuyente", pnIdContribuyente));

                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
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
    /// Función que obtiene el Namespace de una Addenda determinada
    /// </summary>
    /// <param name="pnIdModulo">ID del Modulo</param>
    /// <returns></returns>
    public string fnObtieneNameSpace(int pnIdModulo)
    {
        string sResultado = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_ObtieneAddendaConfigurada_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIdModulo", pnIdModulo));

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
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for clsConfiguracionAddenda
/// </summary>
public class clsConfiguracionAddenda
{
    public int fnObtieneEstructuraCFD(int nidCfd)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    DataTable dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nId_Cfd", nidCfd);
        //    int idEstructura;
        //    idEstructura = Convert.ToInt32(giSql.TraerEscalar("usp_Cfd_Estructura_Sel_Cobro", true));
        //    return idEstructura;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return 0;
        //}


        int idEstructura = 0;
        try
        {
            using (SqlConnection con = new SqlConnection

            (Utilerias.Encriptacion.Base64.DesencriptarBase64

            (ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Cfd_Estructura_Sel_Cobro";
                    cmd.Parameters.Add(new SqlParameter("nId_Cfd", nidCfd));
                    idEstructura = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
        return idEstructura;

    }

    public DataTable fnObtieneAddendaporIdCfd(int nidCfd, int nidEstructura)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    DataTable dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nIdCfd", nidCfd);
        //    giSql.AgregarParametro("@nIdEstructura", nidEstructura);
        //    XmlDocument Addenda = new XmlDocument();
        //    giSql.Query("usp_Cfd_ObtieneAddenda_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    XmlDocument Addenda = new XmlDocument();
        //    return null;
        //}

        DataTable gdtAuxiliar = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection

            (Utilerias.Encriptacion.Base64.DesencriptarBase64

            (ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Cfd_ObtieneAddenda_sel";
                    cmd.Parameters.Add(new SqlParameter("nIdCfd", nidCfd));
                    cmd.Parameters.Add(new SqlParameter("nIdEstructura", nidEstructura));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(gdtAuxiliar);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
        return gdtAuxiliar;
    }

    public string fnObtieneNameSpace(int nIdModulo)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    string Namespacex = string.Empty;
        //    giSql.AgregarParametro("@nIdModulo", nIdModulo);
        //    Namespacex = Convert.ToString(giSql.TraerEscalar("usp_Cfd_ObtieneAddendaConfigurada_sel", true));
        //    return Namespacex;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return string.Empty;
        //}


        try
        {
            using (SqlConnection con = new SqlConnection
            (Utilerias.Encriptacion.Base64.DesencriptarBase64
            (ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    string Namespacex = string.Empty;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Cfd_ObtieneAddendaConfigurada_sel";
                    cmd.Parameters.Add(new SqlParameter("nIdModulo", nIdModulo));
                    Namespacex = Convert.ToString(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                    return Namespacex;
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return string.Empty;
        }
    }
}
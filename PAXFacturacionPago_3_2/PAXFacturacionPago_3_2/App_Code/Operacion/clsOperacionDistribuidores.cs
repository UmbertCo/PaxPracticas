using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for clsOperacionDistribuidores
/// </summary>
public class clsOperacionDistribuidores
{
    private string conConfig = "conConfiguracion";

    public int fnInsertaDistribuidor(int pIdUsuario, string sNumeroDist, bool Certificado, DateTime sFechaInicio, DateTime sFechaFinal, string sEstatus)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RegistroDistribuidor_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    cmd.Parameters.AddWithValue("@nNumeroDist", sNumeroDist);
                    cmd.Parameters.AddWithValue("@nCertificado", Certificado);
                    cmd.Parameters.AddWithValue("@sFechaInicio", sFechaInicio);
                    cmd.Parameters.AddWithValue("@sFechaFin", sFechaFinal);
                    cmd.Parameters.AddWithValue("@sEstatus", sEstatus);
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return nResultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
        //    giSql.AgregarParametro("@nNumeroDist", sNumeroDist);
        //    giSql.AgregarParametro("@nCertificado", Certificado);
        //    giSql.AgregarParametro("@sFechaInicio", sFechaInicio);
        //    giSql.AgregarParametro("@sFechaFin", sFechaFinal);
        //    giSql.AgregarParametro("@sEstatus", sEstatus);
        //    Int32 Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Con_RegistroDistribuidor_Ins", true));
        //    return Resultado;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    int Resultado = 0;
        //    return Resultado;            
        //}
    }

    public int fnObtieneidUsuarioDistribuidor(string sClaveUsuario, string sEmail)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ObtieneIdUsuario_por_Clave", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sClaveUsuario", sClaveUsuario);
                    cmd.Parameters.AddWithValue("@sEmail", sEmail);
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el id del usuario distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return nResultado;

        //try
        //{
        //    int Resultado = 0;
        //giSql = clsComun.fnCrearConexion(conConfig);
        //giSql.AgregarParametro("@sClaveUsuario", sClaveUsuario);
        //giSql.AgregarParametro("@sEmail", sEmail);
        //Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Con_ObtieneIdUsuario_por_Clave", true));
        //return Resultado;
        // }
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    int Resultado = 0;
        //    return Resultado;
        //}
    }

    public bool fnEliminaDistribuidor(int pIdDistribuidor)
    {
        bool resultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_BajaDistribuidor_del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdDistribuidor", pIdDistribuidor);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar usuario distribuidor" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //try
        //{            
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
        //    giSql.NoQuery("usp_Con_BajaDistribuidor_del", true);
        //    return true;            
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    return false;        
        //}
    }

    public bool fnActualizaDistribuidor(int pIdDistribuidor, bool Certificado, DateTime sFechaFinal)
    {
        bool resultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ActualizaDistribuidor_upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdDistribuidor", pIdDistribuidor);
                    cmd.Parameters.AddWithValue("@nCertificado", Certificado);
                    cmd.Parameters.AddWithValue("@sFechaFin", sFechaFinal);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
        //    giSql.AgregarParametro("@nCertificado", Certificado);
        //    giSql.AgregarParametro("@sFechaFin", sFechaFinal);
        //    giSql.NoQuery("usp_Con_ActualizaDistribuidor_upd", true);
        //    bool resultado = true;
        //    return resultado;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    bool resultado = false;
        //    return resultado;
        //}
    }

    public DataTable fnObtieneDistribuidoresAll()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ObtieneDistribuidor_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener distribuidores." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    DataTable dtAuxiliar = new DataTable();
        //    giSql.Query("usp_Con_ObtieneDistribuidor_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    DataTable dtAuxiliar = new DataTable();
        //    dtAuxiliar = null;
        //    return dtAuxiliar;
        //}

    }


    public DataTable fnObtieneDistribuidoresporNumero(string sNumeroDist)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_con_ObtenerDistribuidorpornumero_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sNumDist", sNumeroDist);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener distribuidor por numero." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    DataTable dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@sNumDist", sNumeroDist);
        //    giSql.Query("usp_con_ObtenerDistribuidorpornumero_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    DataTable dtAuxiliar = new DataTable();
        //    dtAuxiliar = null;
        //    return dtAuxiliar;
        //}

    }

    public DataTable fnObtieneDistribuidoresporidUsuario(int nIdUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_con_ObtenerDistribuidorporusuario_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nidUsuario", nIdUsuario);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener distribuidor por id de usuario." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    DataTable dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nidUsuario", nIdUsuario);
        //    giSql.Query("usp_con_ObtenerDistribuidorporusuario_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    DataTable dtAuxiliar = new DataTable();
        //    dtAuxiliar = null;
        //    return dtAuxiliar;
        //}

    }



    public bool fnInsertaDistribuidorRelacion(int pIdDist,int pIdUsuario, string sAcceso,string sEstatus, DateTime sFechaCaptura)
    {
        bool resultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Distribuidor_Usuario_rel_ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdDist", pIdDist);
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    cmd.Parameters.AddWithValue("@sAcceso", sAcceso);
                    cmd.Parameters.AddWithValue("@sEstatus", sEstatus);
                    cmd.Parameters.AddWithValue("@sFechaCap", sFechaCaptura);
                    con.Open();
                    cmd.ExecuteScalar();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la relacion usuario distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);

        //    giSql.AgregarParametro("@nIdDist", pIdDist);
        //    giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
        //    giSql.AgregarParametro("@sAcceso", sAcceso);
        //    giSql.AgregarParametro("@sEstatus", sEstatus);
        //    giSql.AgregarParametro("@sFechaCap", sFechaCaptura);

        //    giSql.TraerEscalar("usp_Con_Distribuidor_Usuario_rel_ins", true);
        //    return true;
           
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

        //    return false;
        //}
    }


    public bool fnEliminaDistribuidorRelacion(int pIdDistribuidor, int pIdUsuario)
    {
        bool resultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Distribuidor_Usuario_rel_del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdDist", pIdDistribuidor);
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    con.Open();
                    cmd.ExecuteScalar();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error al eliminar la relacion de usuario distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    giSql.AgregarParametro("@nIdDist", pIdDistribuidor);
        //    giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
        //    giSql.NoQuery("usp_Con_Distribuidor_Usuario_rel_del", true);
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    return false;
        //}
    }


    public DataTable fnObtieneClientesporDistribuidor(int pidDistribuidor, string sClave_usuario, string sEmail, string sAcceso )
    {

        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ClientesDistribuidor_sel_prueba", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdDistribuidor", pidDistribuidor);
                    cmd.Parameters.AddWithValue("@sClave_Usuario", sClave_usuario);
                    cmd.Parameters.AddWithValue("@sEmail", sEmail);
                    cmd.Parameters.AddWithValue("@sAcceso", sAcceso);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener cliente por distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    DataTable dtAuxiliar = new DataTable();

        //    giSql.AgregarParametro("@nIdDistribuidor",  pidDistribuidor);
        //    giSql.AgregarParametro("@sClave_Usuario",   sClave_usuario);
        //    giSql.AgregarParametro("@sEmail",           sEmail);
        //    giSql.AgregarParametro("@sAcceso",          sAcceso);

        //    giSql.Query("usp_Con_ClientesDistribuidor_sel_prueba", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    DataTable dtAuxiliar = new DataTable();
        //    dtAuxiliar = null;
        //    return dtAuxiliar;
        //}
    }

    /// <summary>
    /// Retorna la lista de usuarios por distribuidor
    /// </summary>
    /// <param name="nIdDistribuidor"></param>
    /// <returns></returns>
    public DataTable fnObtieneUsuariosporDistribuidor(int nIdDistribuidor)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_UsuariosDistribuidor_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdDistribuidor", nIdDistribuidor);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener la lista de usuarios por distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    DataTable dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nIdDistribuidor", nIdDistribuidor);
        //    giSql.Query("usp_Con_UsuariosDistribuidor_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    DataTable dtAuxiliar = new DataTable();
        //    dtAuxiliar = null;
        //    return dtAuxiliar;
        //}
    }

    public DataTable fnObtieneCreditosUsuarioDistribuidor(int pIdUsuario, int pIdestructura)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ClientesDistCreditos_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    cmd.Parameters.AddWithValue("@nIdEstructura", pIdestructura);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener creditos de usuario distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    DataTable dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
        //    giSql.AgregarParametro("@nIdEstructura", pIdestructura);
        //    giSql.Query("usp_Con_ClientesDistCreditos_sel", true, ref dtAuxiliar);
        //    return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    return null;
        //}
    }

    public bool fnBajaUsuariodeDistribuidor(int pIdDistribuidor, int pIdUsuario, double pCreditos, int pIdUsuarioDist)
    {
        bool resultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RegresaCreditosADistribuidor_upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdDistribuidor", pIdDistribuidor);
                    cmd.Parameters.AddWithValue("@nIdUsuarioDistribuidor", pIdUsuarioDist);
                    cmd.Parameters.AddWithValue("@nCreditos", pCreditos);
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    con.Open();
                    cmd.ExecuteScalar();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error al dar de baja al usuario distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
        //    giSql.AgregarParametro("@nIdUsuarioDistribuidor", pIdUsuarioDist);
        //    giSql.AgregarParametro("@nCreditos", pCreditos);
        //    giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
        //    giSql.NoQuery("usp_Con_RegresaCreditosADistribuidor_upd", true);
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    return false;
        //}
    }


    public bool fnActualizaCreditosUsuariodeDistribuidor(int pIdDistribuidor, int pIdUsuario, double pCreditos, int pEstructuraUsuario,
        double pCreditosAnt, string sAcceso, int pIdDistribuidorUsuario, int pIdEstructuraDistribuidor, double nPrecioUnitario, string sServicio)
    {
        bool resultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ActualizaCreditosUsuarioDistribuidor", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdDistribuidor", pIdDistribuidor);
                    cmd.Parameters.AddWithValue("@nCreditosPos", pCreditos);
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    cmd.Parameters.AddWithValue("@nIdEstructuraUsuario", pEstructuraUsuario);
                    cmd.Parameters.AddWithValue("@nCreditosAnt", pCreditosAnt);
                    cmd.Parameters.AddWithValue("@sStatus", sAcceso);
                    cmd.Parameters.AddWithValue("@nIdUsuarioDistribuidor", pIdDistribuidorUsuario);
                    cmd.Parameters.AddWithValue("@nIdEstructuraDistribuidor", pIdEstructuraDistribuidor);
                    cmd.Parameters.AddWithValue("@sFechaCompra", DateTime.Now);
                    cmd.Parameters.AddWithValue("@nPrecioUnitario", nPrecioUnitario);
                    cmd.Parameters.AddWithValue("@sServicio", sServicio);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error al actualizar creditos del usuario distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
        //    giSql.AgregarParametro("@nCreditosPos", pCreditos);
        //    giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
        //    giSql.AgregarParametro("@nIdEstructuraUsuario", pEstructuraUsuario);
        //    giSql.AgregarParametro("@nCreditosAnt", pCreditosAnt);
        //    giSql.AgregarParametro("@sStatus", sAcceso);
        //    giSql.AgregarParametro("@nIdUsuarioDistribuidor", pIdDistribuidorUsuario);
        //    giSql.AgregarParametro("@nIdEstructuraDistribuidor", pIdEstructuraDistribuidor);
        //    giSql.AgregarParametro("@sFechaCompra", DateTime.Now);
        //    giSql.AgregarParametro("@nPrecioUnitario", nPrecioUnitario);
        //    giSql.AgregarParametro("@sServicio", sServicio);
        //    giSql.NoQuery("usp_Con_ActualizaCreditosUsuarioDistribuidor", true);
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    return false;
        //}
    }


    public bool fnActualizaUsuariodeDistribuidor(int pIdDistribuidor, int pIdUsuario, double pCreditos, int pIdUsuarioDist, string sAcceso, int pIdEstructuraUsuario, int pIdEstructuraDistribuidor)
    {
        bool resultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ActualizaCreditosADistribuidor_upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdDistribuidor", pIdDistribuidor);
                    cmd.Parameters.AddWithValue("@nIdUsuarioDistribuidor", pIdUsuarioDist);
                    cmd.Parameters.AddWithValue("@nCreditos", pCreditos);
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    cmd.Parameters.AddWithValue("@sStatus", sAcceso);
                    cmd.Parameters.AddWithValue("@nIdEstructuraUsuario", pIdEstructuraUsuario);
                    cmd.Parameters.AddWithValue("@nIdEstructuraDistribuidor", pIdEstructuraDistribuidor);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error al actualizar usuario distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
        //    giSql.AgregarParametro("@nIdUsuarioDistribuidor", pIdUsuarioDist);
        //    giSql.AgregarParametro("@nCreditos", pCreditos);
        //    giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
        //    giSql.AgregarParametro("@sStatus", sAcceso);
        //    giSql.AgregarParametro("@nIdEstructuraUsuario", pIdEstructuraUsuario);
        //    giSql.AgregarParametro("@nIdEstructuraDistribuidor", pIdEstructuraDistribuidor);
        //    giSql.NoQuery("usp_Con_ActualizaCreditosADistribuidor_upd", true);
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    return false;
        //}
    }

    /// <summary>
    /// Inserta los modulos de distribuidor al usuario nuevo
    /// </summary>
    /// <param name="pIdUsuario"></param>
    /// <returns></returns>
    public void fnInsertarModulosDistribuidor(int pIdUsuario)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_ctp_RegistraModulosADistribuidor_ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nId_Usuario", pIdUsuario);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar los modulos de distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    giSql.AgregarParametro("@nId_Usuario", pIdUsuario);
        //    giSql.NoQuery("usp_ctp_RegistraModulosADistribuidor_ins", true);
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //}
    }


    public double fnObtieneCreditosDistribuidor(int pIdUsuario, int pIdestructura)
    {
        double resultado = 0;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_CreditosdeDistribuidor_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    cmd.Parameters.AddWithValue("@nIdEstructura", pIdestructura);
                    con.Open();
                    resultado =  Convert.ToDouble(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los creditos del distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
        //    giSql.AgregarParametro("@nIdEstructura", pIdestructura);
        //    Double Resultado = Convert.ToDouble(giSql.TraerEscalar("usp_Con_CreditosdeDistribuidor_sel", true));
        //    return Resultado;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    double Resultado = 0;
        //    return Resultado;
        //}
    }

    /// <summary>
    /// Función que se encarga de obtener los créditos del usuario si pertenece a algun distribuidor
    /// </summary>
    /// <param name="pnId_Usuario">ID del Usuario</param>
    /// <returns></returns>
    public DataTable fnObtieneCreditosDistribuidorporUsuario(int pnId_Usuario)
    {
        DataTable dtResultado = new DataTable();
        using (SqlConnection scConexion = new SqlConnection())
        {
            try
            {
                scConexion.ConnectionString = PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString);
                scConexion.Open();

                using (SqlCommand scoComando = new SqlCommand())
                {
                    scoComando.Connection = scConexion;
                    scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                    scoComando.CommandText = "usp_Con_ObtieneCreditosdeDistribuidorporHijo_sel";
                    scoComando.Parameters.AddWithValue("nIdUsuario", pnId_Usuario);

                    using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                    {
                        sdaAdaptador.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
            finally
            {
                scConexion.Close();
            }
        }
        return dtResultado;
    }

    public DataTable fnObtieneClientedeDistribuidor(int pIdUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_ctpClientedeDistribuidorbyIdusuario", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdUsuario", pIdUsuario);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener cliente de distribuidor." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;


        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conConfig);
        //    DataTable dtAuxiliar = new DataTable();
        //    giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
        //    giSql.Query("usp_ctpClientedeDistribuidorbyIdusuario", true, ref dtAuxiliar);
        //    return dtAuxiliar;
   
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    return null;
        //}
    }

    public DataTable fnObtenerDatosUsuario()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfig].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Usuario_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener datos de usuario." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //try
        //{
        //giSql = clsComun.fnCrearConexion(conConfig);
        //DataTable dtAuxiliar = new DataTable();
        //giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);
        //giSql.Query("usp_Con_Usuario_Sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    return null;
        //}
    }

    /// <summary>
    /// Retorna el reporte de timbres y cancelaciones por usuarios de distribuidor
    /// </summary>
    /// <param name="nIdDistribuidor"></param>
    /// <returns></returns>
    public DataSet fnObtieneReporteDistribuidor(int nIdUsuario, DateTime nFechaIni, DateTime nFechaFin)
    {
        DataSet dtResultado = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracionDistribuidor"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_cfd_Comprobantes_corte_mes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nId_Usuario", nIdUsuario);
                    cmd.Parameters.AddWithValue("@sFechaIni", nFechaIni);
                    cmd.Parameters.AddWithValue("@sFechaFin", nFechaFin);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener reporte de timbres y cancelaciones." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;
    }


    /// <summary>
    /// Retorna el reporte del detalle del corte del mes
    /// </summary>
    /// <param name="nIdDistribuidor"></param>
    /// <returns></returns>
    public DataSet fnObtieneCorteMesDetalle(int nIdUsuario, DateTime nFechaIni, DateTime nFechaFin)
    {
        DataSet dtResultado = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conConfiguracionDistribuidor"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_cfd_Comprobantes_corte_mes_detalle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nId_Usuario", nIdUsuario);
                    cmd.Parameters.AddWithValue("@sFechaIni", nFechaIni);
                    cmd.Parameters.AddWithValue("@sFechaFin", nFechaFin);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                dtResultado = null;
                throw new Exception("Error al obtener reporte de timbres y cancelaciones." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;
    }
}
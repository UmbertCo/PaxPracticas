using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;


/// <summary>
/// Summary description for clsOperacionRFC
/// </summary>
public class clsOperacionRFC
{
    private string conClientes = "conConfiguracion";

    /// <summary>
    /// Retorna todos los rfc configurados para ese usuario
    /// </summary>
    /// <param name="p_idContribuyente">Identificador del usuario  a buscar</param>
    /// <returns>rfcs segun su usuario</returns>
    public DataTable fnObtieneRFCsContribuyente(int p_idusuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RCFs_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nId_Usuario", p_idusuario);
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
                throw new Exception("Error al obtener los RFCs del usuario." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;
        //giSql = clsComun.fnCrearConexion(conClientes);
        //giSql.AgregarParametro("@nId_Usuario", p_idusuario);
        //DataTable dtAuxiliar = new DataTable();
        //giSql.Query("usp_Con_RCFs_Sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
    }


    /// <summary>
    /// Retorna el idrfc para ese rfc
    /// </summary>
    /// <param name="p_RFC">rfc del usuario en sesion</param>
    /// <returns>idrfc segun su rfc</returns>
    public Int32 fnObtieneidRFC(string p_RFC)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_IDRfc_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sRFC", p_RFC);  
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el idRFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return nResultado;

        //giSql = clsComun.fnCrearConexion(conClientes);
        //giSql.AgregarParametro("@sRFC", p_RFC);             
        //Int32 Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Con_IDRfc_Sel",true));
        //return Resultado;
    }

    /// <summary>
    /// Retorna el idrfc para ese rfc
    /// </summary>
    /// <param name="p_RFC">rfc del usuario en sesion</param>
    /// <returns>idrfc segun su rfc</returns>
    public DataTable fnObtieneCertificado(int p_idRFC)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Timbrado_RfcCertificado_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nId_Rfc", p_idRFC);
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
                throw new Exception("Error al obtener el idRFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion("conTimbrado");
        //DataTable dtAuxiliar = new DataTable();
        //giSql.AgregarParametro("@nId_Rfc", p_idRFC);
        //giSql.Query("usp_Timbrado_RfcCertificado_Sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
    }


    /// <summary>
    /// Retorna el idrfc para ese rfc
    /// </summary>
    /// <param name="p_RFC">rfc del usuario en sesion</param>
    /// <returns>idrfc segun su rfc</returns>
    public int fnInsertaRFC(string p_RFC, string p_estatus, string p_razon_social, int idContribuyente, int pIdVersion, byte[] nLogo)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RCF_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sRFC", p_RFC);
                    cmd.Parameters.AddWithValue("@sEstatus", p_estatus);
                    cmd.Parameters.AddWithValue("@sRazon_social", p_razon_social);
                    cmd.Parameters.AddWithValue("@nIdContribuyente", idContribuyente);
                    cmd.Parameters.AddWithValue("@nIdVersion", pIdVersion);
                    if (nLogo.Length != 0)
                    {
                        cmd.Parameters.AddWithValue("@nLogo", nLogo);
                    }
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el RFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return nResultado;

        //int Resultado = 0;
        //giSql = clsComun.fnCrearConexion(conClientes);
        //giSql.AgregarParametro("@sRFC", p_RFC);
        //giSql.AgregarParametro("@sEstatus", p_estatus);
        //giSql.AgregarParametro("@sRazon_social", p_razon_social);
        //giSql.AgregarParametro("@nIdContribuyente", idContribuyente);
        //giSql.AgregarParametro("@nIdVersion", pIdVersion);
        //if (nLogo.Length != 0)
        //{
        //    giSql.AgregarParametro("@nLogo", nLogo);
        //}
        //Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Con_RCF_Ins",true));
        //return Resultado;
    }


    /// <summary>
    /// Retorna el idrfc para ese rfc
    /// </summary>
    /// <param name="p_RFC">identificador del rfc a buscar</param>
    /// <returns>idrfc segun su rfc</returns>
    public DataTable fnObtenerRFCsParaNoEliminar(int p_idRFC)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RelacionRFCComprobante_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nidRFC", p_idRFC);
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
                throw new Exception("Error al obtener el idRFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;
        //giSql = clsComun.fnCrearConexion(conClientes);
        //DataTable dtAuxiliar = new DataTable();
        //giSql.AgregarParametro("@nidRFC", p_idRFC);
        //giSql.Query("usp_Ctp_RelacionRFCComprobante_sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
    }


    /// <summary>
    /// Retorna el rfc para ese idrfc
    /// </summary>
    /// <param name="p_idRFC">identificador unico de rfc</param>
    /// <returns>rfc segun su idrfc</returns>
    public DataTable fnObtenerRFCbyId(int p_idRFC)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_IDRfcObtener_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@n_IdRFC", p_idRFC);
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
                throw new Exception("Error al obtener el RFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conClientes);
        //DataTable dtAuxiliar = new DataTable();
        //giSql.AgregarParametro("@n_IdRFC", p_idRFC);
        //giSql.Query("usp_Con_IDRfcObtener_Sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
    }


    /// <summary>
    /// dar de baja un RFC
    /// </summary>
    /// <param name="p_idRFC">identificador unico de rfc</param>
    /// <returns>rfc segun su idrfc</returns>
    public void fnEliminaRFCbyId(int p_idRFC)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_IdRFC_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@n_IdRFC", p_idRFC);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al dar de baja el RFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        //giSql = clsComun.fnCrearConexion(conClientes);             
        //giSql.AgregarParametro("@n_IdRFC", p_idRFC);
        //giSql.NoQuery("usp_Con_IdRFC_Del", true);           
    }

    /// <summary>
    /// Retorna todos el rfc configurado para esa estructura
    /// </summary>
    /// <param name="pIdEstructura">Identificador de la Estructura</param>
    /// <returns>rfcs segun su Estructura</returns>
    public DataTable fnObtieneRFCdeEstructura(int pIdEstructura)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RCFs_Por_Estructura", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nId_Estructura", pIdEstructura);
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
                throw new Exception("Error al obtener el RFC de la estructura." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;
        //giSql = clsComun.fnCrearConexion(conClientes);
        //giSql.AgregarParametro("@nId_Estructura", pIdEstructura);
        //DataTable dtAuxiliar = new DataTable();
        //giSql.Query("usp_Con_RCFs_Por_Estructura", true, ref dtAuxiliar);
        //return dtAuxiliar;
     }

    /// <summary>
    /// Retorna todos el rfc configurado para esa estructura
    /// </summary>
    /// <param name="pnId_Estructura">Identificador de la estructura  a buscar</param>
    /// <returns>rfc segun su estructura</returns>
    public DataTable fnObtieneRFCsEstructura(int pnId_Estructura)
    {
        DataTable gdtAuxiliar = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Con_RCFsEstructura_Sel";
                    cmd.Parameters.Add(new SqlParameter("@nId_Estructura", pnId_Estructura));

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
        }
              return gdtAuxiliar;
    }

    /// <summary>
    /// Retorna todos el rfc configurado para esa estructura
    /// </summary>
    /// <param name="p_idEstructura">Identificador de la estructura  a buscar</param>
    /// <returns>rfc segun su estructura</returns>
    public DataTable fnObtieneRFCsRFCClave(string sRFC, int p_idEstructura, int p_idContribuyente)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RCFs_Por_claveRFC", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sRFC", sRFC);
                    cmd.Parameters.AddWithValue("@nId_Estructura", p_idEstructura);
                    cmd.Parameters.AddWithValue("@nId_Contribuyente", p_idContribuyente);
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
                throw new Exception("Error al obtener el RFC de la estructura." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conClientes);
        //giSql.AgregarParametro("@sRFC", sRFC);
        //giSql.AgregarParametro("@nId_Estructura", p_idEstructura);
        //giSql.AgregarParametro("@nId_Contribuyente", p_idContribuyente);
        //DataTable dtAuxiliar = new DataTable();
        //giSql.Query("usp_Con_RCFs_Por_claveRFC", true, ref dtAuxiliar);
        //return dtAuxiliar;
     }



    /// <summary>
    /// Retorna el objeto imagen con la información identificador del RFC
    /// </summary>
    /// <param name="pidRFC">Identificador del rfc</param>
    /// <returns>logo del rfc</returns>
    public byte[] fnObtenerImagenRFC(int pidRFC)
    {
        Byte[] xImagen = new Byte[] { };

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ImagenRFC_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdRFC", pidRFC);
                    con.Open();
                    xImagen = (byte[]) cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error al obtener la imagen del RFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return xImagen;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conClientes);
        //    Byte[] xImagen = new Byte[] { };
        //    giSql.AgregarParametro("@nIdRFC", pidRFC);
        //    xImagen = (byte[])giSql.TraerEscalar("usp_Con_ImagenRFC_sel", true);
        //    return xImagen;
        //}
        //catch (Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    byte[] Resultado = { };
        //    return Resultado;            
        //}
    }

    /// <summary>
    /// Retorna el idrfc para ese rfc y contribuyente
    /// </summary>
    /// <param name="p_RFC">rfc del usuario en sesion</param>
    /// <returns>idrfc segun su rfc</returns>
    public Int32 fnObtieneidRFCCon(string p_RFC,int id_Contribuyente)
    {
        int nResultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_IDRfcCont_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sRFC", p_RFC);
                    cmd.Parameters.AddWithValue("@nId_Contribuyente", id_Contribuyente);
                    con.Open();
                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el idRFC del RFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return nResultado;

        //giSql = clsComun.fnCrearConexion(conClientes);
        //giSql.AgregarParametro("@sRFC", p_RFC);
        //giSql.AgregarParametro("@nId_Contribuyente", id_Contribuyente);
        //Int32 Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Con_IDRfcCont_Sel", true));
        //return Resultado;
    }

    /// <summary>
    /// Retorna los usuarios que dependen de ese padre
    /// </summary>
    /// <param name="pIdUsuarioPadre">identificador de usuario padre</param>
    /// <returns>usuarios segun identificador padre</returns>
    public DataTable fnObtieneUsuariosporPadre(int pIdUsuarioPadre, int pIdEstructura)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_con_RecuperaUsuariosPadre_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdUsuarioAlta", pIdUsuarioPadre);
                    cmd.Parameters.AddWithValue("@nIdEstructura", pIdEstructura);
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
                throw new Exception("Error al obtener usuarios dependientes del padre." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conClientes);
        //DataTable dtAuxiliar = new DataTable();
        //giSql.AgregarParametro("@nIdUsuarioAlta", pIdUsuarioPadre);
        //giSql.AgregarParametro("@nIdEstructura", pIdEstructura);
        //giSql.Query("usp_con_RecuperaUsuariosPadre_Sel", true, ref dtAuxiliar);
        //return dtAuxiliar;           
    }

    /// <summary>
    /// Modificar Logo
    /// </summary>
    /// <param name="p_idRFC">identificador unico de rfc</param>
    /// <param name="pLogo">logo</param>
    public bool fnActualizaLogo(int p_idRFC, byte[] pLogo)
    {

        bool logo = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conClientes].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_ActualizaLogo_upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdRFC", p_idRFC);
                    cmd.Parameters.AddWithValue("@nLogo", pLogo);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    logo = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el logo." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return logo;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conClientes);
        //    giSql.AgregarParametro("@nIdRFC", p_idRFC);
        //    giSql.AgregarParametro("@nLogo", pLogo);
        //    giSql.NoQuery("usp_Con_ActualizaLogo_upd", true);
        //    return true;
        //}
        //catch(Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        //    return false;
        //}
    }
}

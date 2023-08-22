using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for clsOperacionCorreos
/// </summary>
public class clsOperacionCorreos
{
    private string conControl = "conControl";
    private string conConfiguracion = "conConfiguracion";
  
    /// <summary>
    ///actualiza el password del usuario
    /// </summary>
    /// <param name="psPassword">password del usuario</param>
    ///  /// <param name="psIdUsuario">identificador del usuario</param>
   
    public bool fnActualizaPasswordUsuario(int psidCorreo, string psPassword, string psEstatus)
    {
        bool resultado = false;
        //string psPass = Utilerias.Encriptacion.Classica.Encriptar(psPassword);        
        string psPass = PAXCrypto.CryptoAES.EncriptarAES64(psPassword);

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conControl].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Correo_Actualiza_Password", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sidCorreo", psidCorreo);
                    cmd.Parameters.AddWithValue("@sPassword", psPass);
                    cmd.Parameters.AddWithValue("@sEstatus", psEstatus);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualisar el password del usuario." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //string psPass = Utilerias.Encriptacion.Classica.Encriptar(psPassword);
        //giSql = clsComun.fnCrearConexion("conControl");
        //giSql.AgregarParametro("@sidCorreo", psidCorreo);
        //giSql.AgregarParametro("@sPassword", psPass);
        //giSql.AgregarParametro("@sEstatus", psEstatus);
        //giSql.NoQuery("usp_Correo_Actualiza_Password", true);
        //return true;
    }

    /// <summary>
    ///Recupera todos los correos configurados del sistema
    /// </summary>
    /// <param name="psPassword">password del usuario</param>
    ///  /// <param name="psIdUsuario">identificador del usuario</param>

    public DataTable fnBuscaCorreoConf()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conControl].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_CorreoProveedores_sel", con))
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
                throw new Exception("Error al obtener los correos configurados." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //DataTable dtAuxiliar = new DataTable();
        //giSql = clsComun.fnCrearConexion("conControl");
        //giSql.Query("usp_Ctp_CorreoProveedores_sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
    }

    /// <summary>
    ///inserta o actualiza un registro de correo configurado del sistema
    /// </summary>
    /// <param name="psidCorreo">identificador del correo</param>
    /// <param name="psPassword">identificador del password</param>
    /// /// <param name="psPassword">identificador del Contribuyente</param>
    public bool fnCorreoInsUpd(string psidCorreo, string psCorreoE, string psPassword, string psEstatus, int psidContribuyente)
    {
        bool resultado = false;
        if(psPassword != null)
        {
            //string psPass = Utilerias.Encriptacion.Classica.Encriptar(psPassword);
            string psPass = PAXCrypto.CryptoAES.EncriptarAES64(psPassword);
        }

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conControl].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_CorreoProveedores_ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdCorreo", psidCorreo);
                    cmd.Parameters.AddWithValue("@sCorreoElectronico", psCorreoE);
                    cmd.Parameters.AddWithValue("@sPassword", psPassword);
                    cmd.Parameters.AddWithValue("@sEstatus", psEstatus);
                    cmd.Parameters.AddWithValue("@nIdContribuyente", psidContribuyente);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualisar registro de correo." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //if (psPassword != null)
        //{
        //    string psPass = Utilerias.Encriptacion.Classica.Encriptar(psPassword);
        //}
        //giSql = clsComun.fnCrearConexion("conControl");
        //giSql.AgregarParametro("@nIdCorreo", psidCorreo);
        //giSql.AgregarParametro("@sCorreoElectronico", psCorreoE);
        //giSql.AgregarParametro("@sPassword", psPassword);
        //giSql.AgregarParametro("@sEstatus", psEstatus);
        //giSql.AgregarParametro("@nIdContribuyente", psidContribuyente);
        //giSql.NoQuery("usp_Ctp_CorreoProveedores_ins", true);
        //return true;
    }
    
    /// <summary>
    ///recupera la informacion del rfc en especifico
    /// </summary>
    /// <param name="sRFC">identificador del RFC</param>
    public DataTable ObtenerInfoContribuyenteRFC(string sRFC)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conControl].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_RFC_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sRFC", sRFC);
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
                throw new Exception("Error al recuperar información del RFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //DataTable dtAuxiliar = new DataTable();
        //giSql = clsComun.fnCrearConexion("conControl");
        //giSql.AgregarParametro("@sRFC", sRFC);
        //giSql.Query("usp_Ctp_RFC_sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
    }

    /// <summary>
    ///recupera la informacion del correo en especifico
    /// </summary>
    /// <param name="sCorreo">identificador del Correo</param>
    public DataTable ObtenerInfoCorreo(int sCorreo)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conControl].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Correos_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@sidCorreo", sCorreo);
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
                throw new Exception("Error al recuperar información del correo." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //DataTable dtAuxiliar = new DataTable();
        //giSql = clsComun.fnCrearConexion("conControl");
        //giSql.AgregarParametro("@sidCorreo", sCorreo);
        //giSql.Query("usp_Correos_sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
    }

    /// <summary>
    ///recupera la informacion del rfc en especifico
    /// </summary>
    /// <param name="sCorreo">identificador del contribuyente</param>
    public DataTable ObtenerInfoCorreoContribuyente(int psIdUsuario)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conConfiguracion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RCFs_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nId_Usuario", psIdUsuario);
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
                throw new Exception("Error al recuperar información del RFC." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //DataTable dtAuxiliar = new DataTable();
        //giSql = clsComun.fnCrearConexion("conConfiguracion");
        //giSql.AgregarParametro("@nId_Usuario", psIdUsuario);
        //giSql.Query("usp_Con_RCFs_Sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
    }


    /// <summary>
    ///cambia el estatus a 'baja' de un correo 
    /// </summary>
    /// <param name="psidCorreo">identificador del correo</param>
    public bool fnBajaCorreo(int psidCorreo)
    {
        bool resultado = false;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conControl].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_CorreoProveedores_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idCorreo", psidCorreo);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al borrar registro de correo." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //giSql = clsComun.fnCrearConexion("conControl");
        //giSql.AgregarParametro("@idCorreo", psidCorreo);
        //giSql.NoQuery("usp_Ctp_CorreoProveedores_Del", true);
        //return true;
    }
}
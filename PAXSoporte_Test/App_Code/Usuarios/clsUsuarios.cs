using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

/// <summary>
/// Summary description for clsUsuarios
/// </summary>
public class clsUsuarios
{
    private string conCuenta = System.Configuration.ConfigurationManager.ConnectionStrings["conControl"].ConnectionString;


    /// <summary>
    /// Actualiza el password del usuario
    /// </summary>
    /// <param name="psPassword">Password del usuario</param>
    ///  /// <param name="psIdUsuario">Identificador del usuario</param>
    public bool fnActualizaPasswordUsuario(string psUsuario, string psPassword, string psEstatus)
    {
        bool bRetorno = false;
        byte[] psPass = PAXCrypto.CryptoAES.EncriptaAES(psPassword);

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64
              (ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_Actualiza_Password", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("@sPassword", psPass));
                    cmd.Parameters.Add(new SqlParameter("@sEstatus", psEstatus));
                    cmd.ExecuteNonQuery(); 
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnActualizaPasswordUsuario", "clsUsuarios");
            }
            finally
            {
                con.Close();
            }

        }
        return bRetorno;

    }


    /// <summary>
    /// Inserta o modifica un perfil
    /// </summary>
    /// <param name="psIdPerfil">Identificador del perfil</param>
    /// <param name="psDesc">Descripcion del perfil</param>
    public int fnActualizaPerfil(int psIdPerfil, string psDesc)
    {
        Int32 nRetorno = 0;
        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Perfiles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (psIdPerfil != 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@nId_Perfil", psIdPerfil));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@nId_Perfil", null));
                    }
                    cmd.Parameters.Add(new SqlParameter("@sDescripcion", psDesc));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnActualizaPerfil", "clsUsuarios");
            }
            finally
            {
                con.Close();
            }
        }
        return nRetorno;
    }


    /// <summary>
    /// Actualiza una relacion entre el usuario y el perfil
    /// </summary>
    /// <param name="psIdPerfil">Identificador del perfil</param>
    ///  /// <param name="psIdUsuario">Identificador del usuario</param>
    public bool fnActualizaRelacionUsuarioPerfil(int psIdPerfil, int psIdUsuario)
    {
        bool bRetorno = false;
        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_PerfilesUsuarios_rel_upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Perfil", psIdPerfil));
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario", psIdUsuario));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnActualizaRelacionUsuarioPerfil", "clsUsuarios");
            }
            finally
            {
                con.Close();
            }
        }
        return bRetorno;
    }


    /// <summary>
    /// Metodo para dar de baja al usuario de soporte
    /// </summary>
    /// <param name="psIdUsuario">Id del usuario de soporte</param>
    /// <returns></returns>
    public bool fnBajaUsuarioSoporte(int psIdUsuario)
    {
        bool bRetorno = false;
        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuario_Soporte_Del", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sIdUsuario", psIdUsuario));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }

            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnBajaUsuarioSoporte", "clsUsuarios");
            }
            finally
            {
                con.Close();
            }
        }
        return bRetorno;
    }


    /// <summary>
    ///Obtiene todo el catalogo de tipos de incidente
    /// </summary>
    public DataTable fnCargarCatalogoTipoIncidencias()
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Tipos_Incidentes_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnCargarCatalogoTipoIncidencias", "clsUsuarios");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    /// Elimina una  relacion entre el tipo de incidente y el usuario
    /// </summary>
    /// <param name="psidUsuario">Identificador del usuario</param>
    public bool fnEliminaRelacionUsuarioTipoIncidente(int psidUsuario)
    {
        bool bRetorno = false;
        using (SqlConnection con = 
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuario_Soporte_del_rel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sIdUsuario", psidUsuario));
                    cmd.ExecuteNonQuery(); con.Close();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEliminaRelacionUsuarioTipoIncidente", "clsUsuarios");
            }
            finally
            {
                con.Close();
            }
        }
        return bRetorno;
    }


    /// <summary>
    /// Genera o modifica la informacion del usuario de soporte
    /// </summary>
    /// <param name="psIdUsuario">Contiene el identificador del usuario</param>
    /// <param name="psNombre">Nombre del usuario</param>
    /// <param name="psEmail">Correo electronico del usuario</param>
    /// <param name="psPassword">password encriptado del usuario</param>
    /// <param name="psStatus">estatus del usuario</param>
    /// <param name="psTipoIncidencia">Identificador de la categoría a la que pertenece le incidente</param>
    /// <param name="psUsuario">Identificador del usuario</param>
    public int fnGuardaInformacionUsuarioSoporte(int psIdUsuario, string psNombre, byte[] psEmail, byte[] psPassword, string psStatus, int psTipoIncidencia, string psUsuario)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuario_Soporte", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (!(psIdUsuario == null))
                    {
                        if (!(psIdUsuario == 0))
                        {
                            cmd.Parameters.Add(new SqlParameter("@sIdUsuario", psIdUsuario));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@sIdUsuario", null));
                        }
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@sIdUsuario", null));
                    }


                    cmd.Parameters.Add(new SqlParameter("@sNombre", psNombre));

                    cmd.Parameters.Add(new SqlParameter("@sEmail", psEmail));

                    cmd.Parameters.Add(new SqlParameter("@sPassword", psPassword));

                    cmd.Parameters.Add(new SqlParameter("@sEstatus", psStatus));

                    cmd.Parameters.Add(new SqlParameter("@sId_tipo_incidencia", psTipoIncidencia));

                    cmd.Parameters.Add(new SqlParameter("@sUsuario", psUsuario));

                    cmd.Parameters.Add(new SqlParameter("@nid_Sistema", 2));


                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos, "fnGuardaInformacionUsuarioSoporte", "clsUsuarios");
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
    }


    /// <summary>
    /// Inserta una nueva relacion entre el tipo de incidente y el perfil
    /// </summary>
    /// <param name="psIdPerfil">Identificador del perfil</param>
    /// <param name="psIdTipoIncidente">Identificador del tipo de incidente</param>
    public bool fnInsertaRelacionPerfilTipoIncidente(int psIdPerfil, int psIdTipoIncidente)
    {
        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_PerfilesTipoIncidente_rel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Perfil", psIdPerfil));
                    cmd.Parameters.Add(new SqlParameter("@nId_TipoIncidente", psIdTipoIncidente));
                    cmd.ExecuteNonQuery(); con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnInsertaRelacionPerfilTipoIncidente", "clsUsuarios");
                return false;
            }
            finally
            {
                con.Close();
            }
        }
    }


    /// <summary>
    ///I nserta una nueva relacion entre el usuario y el perfil
    /// </summary>
    /// <param name="psIdPerfil">Identificador del perfil</param>
    /// <param name="psIdUsuario">identificador del usuario</param>
    public bool fnInsertaRelacionUsuarioPerfil(int psIdPerfil, int psIdUsuario)
    {
        bool bRetorno = false;
        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_PerfilesUsuarios_rel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Perfil", psIdPerfil));
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario", psIdUsuario));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnInsertaRelacionUsuarioPerfil", "clsUsuarios");
            }
            finally
            {
                con.Close();
            }
        }
        return bRetorno;
    }


    /// <summary>
    /// Inserta una relacion entre el usuario y el tipo de incidente
    /// </summary>
    /// <param name="psidTipoInc">Identificador del tipo incidende</param>
    /// <param name="psIdUsuario">identificador del usuario</param>
    public bool fnInsertaRelacionUsuarioTipoIncidente(int psidUsuario, int psidTipoInc)
    {
        bool bRetorno = false;
        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuario_Soporte_rel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sIdUsuario", psidUsuario));
                    cmd.Parameters.Add(new SqlParameter("@sId_tipo_incidencia", psidTipoInc));
                    cmd.ExecuteNonQuery(); con.Close();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnInsertaRelacionUsuarioTipoIncidente", "clsUsuarios");
            }
            finally
            {
                con.Close();
            }

        }
        return bRetorno;

    }


    /// <summary>
    /// Obtiene los usuarios de soporte
    /// </summary>
    /// <returns></returns>
    public DataTable fnLlenarGridUsuariosSoporte()
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuarios_Soporte_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnLlenarGridUsuariosSoporte", "clsUsuarios");
            }

        }
        return dtAuxiliar;
    }


    /// <summary>
    /// Obtiene todos los registros de la tabla de perfiles
    /// </summary>
    public DataSet fnLlenaPerfiles()
    {
        DataSet dtAuxiliar = new DataSet();

        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Perfiles_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnLlenaPerfiles", "clsUsuarios");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    /// Obtiene la contraseña del usuario
    /// </summary>
    /// <param name="psUsuario">Identificador del usuario</param>
    /// <param name="psEmail">Email del usuario</param>
    public string fnObtenerContraseniaUsuario(string psUsuario, string psEmail)
    {
        byte[] psAuxiliar = null;
        string psPass = null;

        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_BuscarUsuSop_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sClaveUsuario", psUsuario));
                    cmd.Parameters.Add(new SqlParameter("@sEmail", psEmail));

                    psAuxiliar = (byte[])cmd.ExecuteScalar();

                    // if (psAuxiliar != "")
                    if (psAuxiliar != null && psAuxiliar.Length > 0)
                    {
                        psPass = Convert.ToString(PAXCrypto.CryptoAES.DesencriptaAES(psAuxiliar));
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtenerContraseniaUsuario", "clsUsuarios");
            }
            finally
            {
                con.Close();
            }
        }
        return psPass;
    }


    /// <summary>
    /// Obtiene los tipos de incidente por usuario
    /// </summary>
    /// <param name="psIdUsuario">Identificador del usuario</param>
    public DataTable fnObtenerTipoIncidentesporUsuario(int psidUsuario)
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuarios_Soporte_tipo_inc_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nIdUsuario", psidUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtenerTipoIncidentesporUsuario", "clsUsuarios");
            }
        }
        return dtAuxiliar;
 
    }


    /// <summary>
    /// Obtiene la informacion de un usuario en especifico segun el nombre de usuario
    /// </summary>
    /// <param name="psIdUsuario">Identificador del usuario</param>
    public DataTable fnObtenerUsuariobyUsuario(string psUsuario)
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con =
            new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuarios_Soporte_User_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sUsuario", psUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtenerUsuariobyUsuario", "clsUsuarios");
            }
        }
        return dtAuxiliar;
      
    }
}
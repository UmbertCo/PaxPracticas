using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Configuration;

/// <summary>
/// Clase de capa de negocios para la pantalla webGlobalCuenta 
/// </summary>
public class clsOperacionCuenta
{
    private string conCuenta = "conConfiguracion";

    /// <summary>
    /// Retorna los datos fiscales de la sucursal matriz
    /// </summary>
    /// <returns>Retorna un SqlDataReader con los datos fiscales de la matriz</returns>
    public DataTable fnObtenerDatosFiscales()
    {

        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Cuenta_Sel", con))
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
                throw new Exception("Error al obtener los datos fiscales de la matriz." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;
        ////int idContribuyente = 0;
        //giSql = clsComun.fnCrearConexion(conCuenta);
        //////Se obtiene contribuyente padre, si es que el usuario es hijo del nodo
        ////giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        ////SqlDataReader sdrUsuario = giSql.Query("usp_Ctp_DatosPadre_sel", true);
        ////if (sdrUsuario != null && sdrUsuario.HasRows && sdrUsuario.Read())
        ////{
        ////    idContribuyente = Convert.ToInt32(sdrUsuario["id_contribuyente"].ToString());
        ////}
        ////else
        ////{
        ////    idContribuyente = clsComun.fnUsuarioEnSesion().id_contribuyente;
        ////}
        ////sdrUsuario.Close();
        ////giSql.LimpiarParametros();
        //giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);

        //return giSql.Query("usp_Con_Cuenta_Sel", true);
    }


    /// <summary>
    /// Actualiza la información fiscal de la sucursal matriz
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la sucursal matriz</param>
    /// <param name="psSucursal">Nombre de la sucursal matriz</param>
    /// <param name="psCalle">Nombre de la calle</param>
    /// <param name="psNoExterior">El número exterior del lugar</param>
    /// <param name="psNoInterior">El número interior del lugar</param>
    /// <param name="psColonia">El nombre de la colonia</param>
    /// <param name="psReferencia">Descripción de referencia</param>
    /// <param name="psCodigoPostal">El numero de código postal del área</param>
    /// <param name="psLocalidad">El nombre de la localidad</param>
    /// <param name="psMunicipio">El nombre del municipio</param>
    /// <param name="psIdEstado">Identificador del estado</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnGuardarDatosFiscales(string psIdEstructura, string psSucursal, byte[] psCalle,
                                    byte[] psNoExterior, byte[] psNoInterior, byte[] psColonia,
                                    string psReferencia, byte[] psLocalidad, byte[] psMunicipio,
                                    string psIdEstado, byte[] psCodigoPostal, string psRegimenFiscal)
    {
        int resultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Cuenta_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Estructura", psIdEstructura);
                    cmd.Parameters.AddWithValue("@sSucursal", psSucursal);
                    cmd.Parameters.AddWithValue("@sCalle", psCalle);
                    if (psNoExterior != null && psNoExterior.Length > 0)
                    //if (!string.IsNullOrEmpty(psNoExterior))
                        cmd.Parameters.AddWithValue("@sNumero_Exterior", psNoExterior);
                    if (psNoInterior != null && psNoInterior.Length > 0)
                    //if (!string.IsNullOrEmpty(psNoInterior))
                        cmd.Parameters.AddWithValue("@sNumero_Interior", psNoInterior);
                    if (psColonia != null && psColonia.Length > 0)
                    //if (!string.IsNullOrEmpty(psColonia))
                        cmd.Parameters.AddWithValue("@sColonia", psColonia);
                    if (!string.IsNullOrEmpty(psReferencia))
                        cmd.Parameters.AddWithValue("@sReferencia", psReferencia);
                    //cmd.Parameters.AddWithValue("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
                    if (psColonia != null && psCodigoPostal.Length > 0)
                    cmd.Parameters.AddWithValue("@sCodigo_Postal", psCodigoPostal);
                    if (psLocalidad != null && psLocalidad.Length > 0)
                    //if (!string.IsNullOrEmpty(psLocalidad))
                        cmd.Parameters.AddWithValue("@sLocalidad", psLocalidad);
                    cmd.Parameters.AddWithValue("@sMunicipio", psMunicipio);
                    cmd.Parameters.AddWithValue("@nId_Estado", psIdEstado);
                    if (!string.IsNullOrEmpty(psRegimenFiscal))
                        cmd.Parameters.AddWithValue("@sRegimen_Fiscal", psRegimenFiscal);
                    con.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la información fiscal de la sucursal matriz." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;
        
        //giSql = clsComun.fnCrearConexion(conCuenta);

        //giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        //giSql.AgregarParametro("@sSucursal", psSucursal);
        //giSql.AgregarParametro("@sCalle", psCalle);
        //if (!string.IsNullOrEmpty(psNoExterior))
        //    giSql.AgregarParametro("@sNumero_Exterior", psNoExterior);
        //if (!string.IsNullOrEmpty(psNoInterior))
        //    giSql.AgregarParametro("@sNumero_Interior", psNoInterior);
        //if (!string.IsNullOrEmpty(psColonia))
        //    giSql.AgregarParametro("@sColonia", psColonia);
        //if (!string.IsNullOrEmpty(psReferencia))
        //    giSql.AgregarParametro("@sReferencia", psReferencia);
        //giSql.AgregarParametro("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
        //if (!string.IsNullOrEmpty(psLocalidad))
        //    giSql.AgregarParametro("@sLocalidad", psLocalidad);
        //giSql.AgregarParametro("@sMunicipio", psMunicipio);
        //giSql.AgregarParametro("@nId_Estado", psIdEstado);
        //if (!string.IsNullOrEmpty(psRegimenFiscal))
        //    giSql.AgregarParametro("@sRegimen_Fiscal", psRegimenFiscal);

        //return giSql.NoQuery("usp_Con_Cuenta_Upd", true);
    }

    /// <summary>
    /// Actualiza los archivos CSD en la base de datos para la sucursal matriz
    /// </summary>
    /// <param name="psIdRfc">Identificador del RFC de la matriz</param>
    /// <param name="vValidadorCertificado">Objeto de validación y manipulación del certificado</param>
    /// <param name="pbKey">Arreglo de bytes del archivo key</param>
    /// <param name="psPassword">Contraseña de cifrado del archivo key</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnGuardarCertificados(string psIdRfc, clsValCertificado vValidadorCertificado, byte[] nPfx)
    {
        int resultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Certificado", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Rfc", psIdRfc);
                    cmd.Parameters.AddWithValue("mCertificado", vValidadorCertificado.fnEncriptarCertificado());
                    cmd.Parameters.AddWithValue("mKey", vValidadorCertificado.fnEncriptarLlave());
                    cmd.Parameters.AddWithValue("sPassword", vValidadorCertificado.fnEncriptarPassword());
                    cmd.Parameters.AddWithValue("dFecha_Inicio", vValidadorCertificado.Certificado.NotBefore);
                    cmd.Parameters.AddWithValue("dFecha_Termino", vValidadorCertificado.Certificado.NotAfter);
                    cmd.Parameters.AddWithValue("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
                    cmd.Parameters.AddWithValue("@mPfx", nPfx);
                    con.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el certificado para la sucursal matriz." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //giSql = clsComun.fnCrearConexion(conCuenta);

        //giSql.AgregarParametro("nId_Rfc", psIdRfc);
        //giSql.AgregarParametro("mCertificado", vValidadorCertificado.fnEncriptarCertificado());
        //giSql.AgregarParametro("mKey", vValidadorCertificado.fnEncriptarLlave());
        //giSql.AgregarParametro("sPassword", vValidadorCertificado.fnEncriptarPassword());
        //giSql.AgregarParametro("dFecha_Inicio", vValidadorCertificado.Certificado.NotBefore);
        //giSql.AgregarParametro("dFecha_Termino", vValidadorCertificado.Certificado.NotAfter);
        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.AgregarParametro("@mPfx", nPfx);
        
        //return giSql.NoQuery("usp_Con_Certificado", true);
    }

    /// <summary>
    /// Autentica al usuario y actualiza el estatus del usuario en sesión para ponerlo en estado de baja.
    /// </summary>
    /// <param name="psPass">Password encriptado del usuario</param>
    /// <param name="psUserName">Clave del usuario</param>
    /// <param name="psEmail">Email del usuario</param>
    /// <returns>booleano indicando si la baja tuvo éxito</returns>
    public bool fnEliminarCuenta(string psPass, string psUserName, string psEmail)
    {
        clsInicioSesionSolicitudReg reg = new clsInicioSesionSolicitudReg();

        try
        {
            DataTable dtAuxiliar = reg.buscarUsuario(psUserName);
            //string sRawPass = Utilerias.Encriptacion.Classica.Desencriptar(psPass);
            string sRawPass = PAXCrypto.CryptoAES.DesencriptaAES64(psPass);
            //if (sRawPass == Utilerias.Encriptacion.Classica.Desencriptar(dtAuxiliar.Rows[0]["password"].ToString()))
            if (sRawPass == PAXCrypto.CryptoAES.DesencriptaAES((byte[])dtAuxiliar.Rows[0]["password"]))
                return reg.actualizaEstadoActual(psUserName, psEmail, 'B');
            else
                return false;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
            return false;
        }
    }

    public bool fnConstruirCuerpoCorreo(string psPass)
    {
        clsInicioSesionSolicitudReg reg = new clsInicioSesionSolicitudReg();
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        DataTable dtAuxiliar = reg.buscarUsuario(clsComun.fnUsuarioEnSesion().userName);

        //Verificamos la contraseña
        //if (psPass != Utilerias.Encriptacion.Classica.Desencriptar(dtAuxiliar.Rows[0]["password"].ToString()))
        if (psPass != PAXCrypto.CryptoAES.DesencriptaAES((Byte[])dtAuxiliar.Rows[0]["password"]))
            throw new Exception("No se pudo dar de baja la cuenta del usuario");

        string sName = clsComun.fnUsuarioEnSesion().userName;
        string sEmail = clsComun.fnUsuarioEnSesion().email;
        //string sPass = Utilerias.Encriptacion.Classica.Encriptar(psPass);
        string sPass = PAXCrypto.CryptoAES.EncriptarAES64(psPass);
        string servidor = clsComun.ObtenerParamentro("urlHostCosto");

        string url = sPass + ":" + sName + ":" + sEmail;
        
        //Byte[] bBytes = Utilerias.Encriptacion.DES3.Encriptar(System.Text.Encoding.UTF8.GetBytes(url));
        Byte[] bBytes = PAXCrypto.CryptoAES.EncriptaAESB(System.Text.Encoding.UTF8.GetBytes(url));


        StringBuilder url64 = new StringBuilder();
        foreach (byte b in bBytes)
        {
            url64.Append(b);
            url64.Append(",");
        }

        //enviamos correo de autenticación
        return  email.EnviarCorreo(
            sEmail,
            Resources.resCorpusCFDIEs.varSubjectBajaCuenta,
            string.Format(Resources.resCorpusCFDIEs.varBajaMailBody, sName, sEmail, servidor, url64.ToString().Trim(',')));

        
    }

    /// <summary>
    /// Recupera los datos fiscales del emisor ex
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerDatosFiscalesEx()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Cuenta_Ex_Sel", con))
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
                throw new Exception("Error al recuperar los datos fiscales del emisor ex." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //giSql = clsComun.fnCrearConexion(conCuenta);
        //giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);
        //return giSql.Query("usp_Con_Cuenta_Ex_Sel", true);
    }


    /// <summary>
    /// Actualiza los datos fiscales ex
    /// </summary>
    /// <param name="nId_Contribuyente"></param>
    /// <param name="psCalle"></param>
    /// <param name="psMunicipio"></param>
    /// <param name="psIdEstado"></param>
    /// <param name="psIdPais"></param>
    /// <param name="psCodigoPostal"></param>
    /// <returns></returns>
    public int fnGuardarDatosFiscalesEx(int nId_Contribuyente, byte[] psCalle, byte[] psMunicipio,
                                string psIdEstado, string psIdPais, byte[] psCodigoPostal)
    {
        int resultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Cuenta_Ex_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Contribuyente", nId_Contribuyente);
                    cmd.Parameters.AddWithValue("@sCalle", psCalle);
                    ////cmd.Parameters.AddWithValue("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
                    cmd.Parameters.AddWithValue("sCodigo_Postal", psCodigoPostal);
                    cmd.Parameters.AddWithValue("@sMunicipio", psMunicipio);
                    cmd.Parameters.AddWithValue("@nId_Estado", psIdEstado);
                    cmd.Parameters.AddWithValue("@nId_Pais", psIdPais);
                    con.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar los datos fiscales ex." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //giSql = clsComun.fnCrearConexion(conCuenta);

        //giSql.AgregarParametro("nId_Contribuyente", nId_Contribuyente);
        //giSql.AgregarParametro("@sCalle", psCalle);
        ////giSql.AgregarParametro("@sCodigo_Postal", string.Format("{0:00000}", Convert.ToInt32(psCodigoPostal)));
        //giSql.AgregarParametro("sCodigo_Postal", psCodigoPostal);
        //giSql.AgregarParametro("@sMunicipio", psMunicipio);
        //giSql.AgregarParametro("@nId_Estado", psIdEstado);
        //giSql.AgregarParametro("@nId_Pais", psIdPais);

        //return giSql.NoQuery("usp_Con_Cuenta_Ex_Upd", true);
    }




    /// <summary>
    /// Actualiza los archivos CSD en la base de datos para la sucursal matriz
    /// </summary>
    /// <param name="psIdRfc">Identificador del RFC de la matriz</param>
    /// <param name="vValidadorCertificado">Objeto de validación y manipulación del certificado</param>
    /// <param name="pbKey">Arreglo de bytes del archivo key</param>
    /// <param name="psPassword">Contraseña de cifrado del archivo key</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnGuardarCertificadosCobro(string psIdRfc, clsValCertificado vValidadorCertificado, int pId_Estructura)
    {
        int resultado = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_Certificado_Cobro", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("nId_Rfc", psIdRfc);
                    cmd.Parameters.AddWithValue("mCertificado", vValidadorCertificado.fnEncriptarCertificado());
                    cmd.Parameters.AddWithValue("mKey", vValidadorCertificado.fnEncriptarLlave());
                    cmd.Parameters.AddWithValue("sPassword", vValidadorCertificado.fnEncriptarPassword());
                    cmd.Parameters.AddWithValue("dFecha_Inicio", vValidadorCertificado.Certificado.NotBefore);
                    cmd.Parameters.AddWithValue("dFecha_Termino", vValidadorCertificado.Certificado.NotAfter);
                    cmd.Parameters.AddWithValue("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
                    cmd.Parameters.AddWithValue("nId_Estructura", pId_Estructura);
                    con.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteNonQuery());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar los archivos CSD para la sucursal matriz." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;


        //giSql = clsComun.fnCrearConexion(conCuenta);

        //giSql.AgregarParametro("nId_Rfc", psIdRfc);
        //giSql.AgregarParametro("mCertificado", vValidadorCertificado.fnEncriptarCertificado());
        //giSql.AgregarParametro("mKey", vValidadorCertificado.fnEncriptarLlave());
        //giSql.AgregarParametro("sPassword", vValidadorCertificado.fnEncriptarPassword());
        //giSql.AgregarParametro("dFecha_Inicio", vValidadorCertificado.Certificado.NotBefore);
        //giSql.AgregarParametro("dFecha_Termino", vValidadorCertificado.Certificado.NotAfter);
        //giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        //giSql.AgregarParametro("nId_Estructura", pId_Estructura);
        //return giSql.NoQuery("usp_Con_Certificado_Cobro", true);
    }

    /// <summary>
    /// Retorna los datos fiscales de la sucursal seleccionada
    /// </summary>
    /// <returns>Retorna un SqlDataReader con los datos fiscales de la sucursal</returns>
    public DataTable fnObtenerDatosFiscalesSuc(int pnIdsucursal)
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Con_RfcSuc_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdSucursal", pnIdsucursal);
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
                throw new Exception("Error al recuperar los datos fiscales de la sucursal." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

            ////int idContribuyente = 0;
            //giSql = clsComun.fnCrearConexion(conCuenta);

            //giSql.AgregarParametro("@nIdSucursal", pnIdsucursal);

            //return giSql.Query("usp_Con_RfcSuc_Sel", true);
    }

    public DataTable fnCargarComplementos()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_ctp_Complementos_Sel", con))
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
                throw new Exception("Error al cargar complementos." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return dtResultado;

        //DataTable dtAuxiliar = new DataTable();
        //giSql = clsComun.fnCrearConexion(conCuenta);

        //giSql.Query("usp_ctp_Complementos_Sel", true, ref dtAuxiliar);
        //return dtAuxiliar;
    }


    public string fnRetornaRutaComplemento(int nIdComplemento)
    {
        string resultado = "";
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[conCuenta].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Cfd_ObtieneRutaComplemento_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nIdComplemento", nIdComplemento);
                    con.Open();
                    resultado = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener ruta del complemento." + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        return resultado;

        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conCuenta);
        //    giSql.AgregarParametro("@nIdComplemento", nIdComplemento);
        //    string Resultado = Convert.ToString(giSql.TraerEscalar("usp_Cfd_ObtieneRutaComplemento_sel", true));
        //    return Resultado;
        //}
        //catch(Exception ex)
        //{
        //    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        //    return null;
        //}
    }
}

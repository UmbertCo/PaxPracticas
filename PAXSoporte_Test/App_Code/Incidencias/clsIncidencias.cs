using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;


/// <summary>
/// Summary description for clsIncidencias
/// </summary>
public class clsIncidencias
{
    private string conCuenta = ConfigurationManager.ConnectionStrings["conControl"].ConnectionString;

    /// <summary>
    ///actualiza el estatus del sat si la incidencia requiere notificacion
    /// </summary>
    /// <param name="pnidIncidente">Identificador del incidente</param>
    /// <param name="psNotificador">persona que autoriza</param>
    /// <param name="pnEstatusSat">estatus del sat</param>
    /// <param name="psIdNotificacion">identificador de la notificacion del sat</param>
    public bool fnActualizaEstatusSAT(int pnIdIncidente, string psNotificador, int pnEstatusSat, string psIdNotificacion)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_IncidenteEstatusSAT_Upd";
                    cmd.Parameters.Add(new SqlParameter("@nId_Incidente", pnIdIncidente));
                    cmd.Parameters.Add(new SqlParameter("@sNotificador", psNotificador));
                    cmd.Parameters.Add(new SqlParameter("@sEstatusSAT", pnEstatusSat));
                    cmd.Parameters.Add(new SqlParameter("@nId_notificacion", psIdNotificacion));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaEstatusSAT", "clsIncidencias");
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Actualiza la fecha de atencion de un incidente y el estatus del incidente
    /// </summary>
    /// <param name="pnIncidente">Identificador del incidente</param>
    public bool fnActualizaFechaRegistroIncidencia(int pnIncidente)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_IncidenteFechaAtencion_Upd";
                    cmd.Parameters.Add(new SqlParameter("nId_Incidente", pnIncidente));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaFechaRegistroIncidencia", "clsIncidencias");
            }
        }
        return bRetorno;
    }

    /// <summary>
    ///actualiza la solucion de usuario y la de soporte segun el identificador de incidencia
    /// </summary>
    /// <param name="pnIdIncidente">Identificador del incidente</param>
    /// <param name="psSolucionUsuario">Solucion que se le dio al usuario</param>
    /// <param name="psSolucionSoporte">Splucion que se le dio al </param>
    public bool fnActualizaIncidencia(int pnIdIncidente, string psSolucionUsuario, string psSolucionSoporte)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                 {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_Ctp_IncidenteAtendido_Upd" ;
                        cmd.Parameters.Add(new SqlParameter("@nId_Incidente", pnIdIncidente));
                        if (psSolucionUsuario == string.Empty)
                        {
                            cmd.Parameters.Add(new SqlParameter("@sSolucionUsuario", null));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@sSolucionUsuario", psSolucionUsuario));
                        }
                        if (psSolucionSoporte == string.Empty)
                        {
                            cmd.Parameters.Add(new SqlParameter("@sSolucionSoporte", null));
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@sSolucionSoporte", psSolucionSoporte));
                        }
                        cmd.ExecuteScalar();
                        bRetorno = true;
                 }
             }
             catch (Exception ex)
             {
              con.Close();
              clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnActualizaIncidencia", "clsIncidencias");
             }
        }
        return bRetorno;
    }

    /// <summary>
    ///actualiza el estatus de prueba y la solucion de prueba cuando estas ya fueron realizadas
    /// </summary>
    /// <param name="pnidIncidente">Identificador del incidente</param>
    /// <param name="psSolucionPrueba">descripcion de la solucion de pruebas</param>
    /// <param name="pnEstatusPrueba">estatus del incidente</param>
    public bool fnActualizaIncidenciaPrueba(int pnIdIncidente, string psSolucionPrueba, int pnEstatusPrueba)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_IncidenteEstatusPrueba_Upd";
                    cmd.Parameters.Add(new SqlParameter("@nId_Incidente", pnIdIncidente));
                    cmd.Parameters.Add(new SqlParameter("@sSolucionPrueba", psSolucionPrueba));
                    cmd.Parameters.Add(new SqlParameter("@sEstatusPrueba", pnEstatusPrueba));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaIncidenciaPrueba", "clsIncidencias");
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// actualiza la fecha de notificacion al SAT según el identificador del incidente
    /// </summary>
    /// <param name="pnIdIncidente">Identificador del problema</param>
    public bool fnActualizaNotificacionSAT(int pnIdIncidente)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_IncidenteFechaNotificacionSAT_Upd";
                    cmd.Parameters.Add(new SqlParameter("@nId_Incidente", pnIdIncidente));
                    cmd.ExecuteNonQuery();
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaNotificacionSAT", "clsIncidencias");
            }
        }
        return bRetorno;
    }

    /// <summary>
    ///inserta el identificador del ticket de problema en el registro de incidencia cuando este se convierte en problema
    /// </summary>
    /// <param name="pnIdIncidente">Identificador del incidente</param>
    /// <param name="psTicketProblema">identificador del ticket del problema</param>
    public bool fnActualizaTicketProblemaenIncidencias(int pnIdIncidente, string psTicketProblema)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_IncidenteTicketProblema_Upd";
                    cmd.Parameters.Add(new SqlParameter("@nId_Incidente", pnIdIncidente));
                    cmd.Parameters.Add(new SqlParameter("@sTicket_Problema", psTicketProblema));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnActualizaTicketProblemaenIncidencias", "clsIncidencias");
            }
        }
        return bRetorno;
    }

    /// <summary>
    /// Envía un correo electrónico tanto al usuario que levanto el ticket como al área de soporte
    /// El correo contiene la información relacionada al incidente
    /// </summary>
    /// <param name="psTicket">Número de ticket generado</param>
    /// <param name="psIdCategoria">Identificador de la categoría a la que pertenece le incidente</param>
    private void fnEnviarNotificacion(string psTicket, string psIdCategoria, string psRuta, int pnIdRelacion)
    {
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Usuario_Reporta_Incidente";
                    cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    cmd.Parameters.Add(new SqlParameter("@nIdRelacion", pnIdRelacion));
                    SqlDataReader lector = cmd.ExecuteReader();

                    lector.Read();

                    email.fnEnviarCorreo(clsComun.fnUsuarioEnSesion().email + ", " + lector["email"].ToString(),
                                    string.Format(Resources.resCorpusCFDIEs.varTicketSubject, psTicket, clsComun.fnUsuarioEnSesion().userName),
                                    string.Format(Resources.resCorpusCFDIEs.varTicketMailBody1, psTicket, lector["nombre"].ToString()), psRuta);
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarNotificacion", "clsIncidencias");
            }
            finally
            {
                con.Close();
            }
        }
    }

    /// <summary>
    /// Envía un correo electrónico tanto al usuario que levanto el ticket como al área de soporte
    /// El correo contiene la información relacionada al incidente
    /// </summary>
    /// <param name="psTicket">Número de ticket generado</param>
    /// <param name="psIdCategoria">Identificador de la categoría a la que pertenece le incidente</param>
    public bool fnEnviarNotificacionAtencionIncidencia(string psTicket, string psIdCategoria, string psMensaje, int pnIdusuario, int pnIdRelacion)
    {
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        { 
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Usuario_Reporta_Incidente";
                    cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    cmd.Parameters.Add(new SqlParameter("@nIdRelacion", pnIdRelacion));
                    SqlDataReader lector = cmd.ExecuteReader();

                    lector.Read();

                    DataSet dtAuxiliar = null;
                    int idUsuario = (pnIdusuario);
                    dtAuxiliar = fnObtenerUsuarioSoporteInfo(idUsuario);
                    foreach (DataRow renglon in dtAuxiliar.Tables[0].Rows)
                    {
                        string psEmail = Convert.ToString(renglon["email"]);
                        email.fnEnviarCorreoAtencionIncidencia(psEmail + ", " + lector["email"].ToString(),
                                                                Resources.resCorpusCFDIEs.varTicketAtencion, psMensaje, lector["email"].ToString());
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarNotificacionAtencionIncidencia", "clsIncidencias");
                return false;
            }
            finally
            {
                con.Close();
            }
        }
    }

    /// <summary>
    /// Envía un correo electrónico tanto al usuario que levanto el ticket como al área de soporte
    /// El correo contiene la información relacionada al incidente
    /// </summary>
    /// <param name="psTicket">Número de ticket generado</param>
    /// <param name="psIdCategoria">Identificador de la categoría a la que pertenece le incidente</param>
    public void fnEnviarNotificacionAtencionIncidenciaPrueba(string psTicket, string psIdCategoria, string psMensaje, int pnIdusuario, string psArchivo, int pnIdRelacion)
    {
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {   
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Usuario_Reporta_Incidente";
                    cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    cmd.Parameters.Add(new SqlParameter("@nIdRelacion", pnIdRelacion));
                    SqlDataReader lector = cmd.ExecuteReader();

                    lector.Read();

                    DataSet dtAuxiliar = null;
                    int idUsuario = (pnIdusuario);
                    dtAuxiliar = fnObtenerUsuarioSoporteInfo(idUsuario);
                    foreach (DataRow renglon in dtAuxiliar.Tables[0].Rows)
                    {
                        string psEmail = Convert.ToString(renglon["email"]);
                        email.fnEnviarCorreoAtencionPruebas(psEmail + ", " + lector["email"].ToString(),
                                                                Resources.resCorpusCFDIEs.VarTicketPrueba, psMensaje, lector["email"].ToString(), psArchivo);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarNotificacionAtencionIncidenciaPrueba", "clsIncidencias");
            }
            finally
            {
                con.Close();
            }
        }
    }

    /// <summary>
    /// Envía un correo electrónico tanto al usuario que levanto el ticket como al área de soporte
    /// El correo contiene la información relacionada al incidente
    /// </summary>
    /// <param name="psTicket">Número de ticket generado</param>
    /// <param name="psIdCategoria">Identificador de la categoría a la que pertenece le incidente</param>
    public void fnEnviarNotificacionSAT(string psTicket, string psIdCategoria, string psMensaje, string psArchivo, int pnIdRelacion)
    {
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Usuario_Reporta_Incidente";
                    cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    cmd.Parameters.Add(new SqlParameter("@nIdRelacion", pnIdRelacion));
                    SqlDataReader lector = cmd.ExecuteReader();

                    lector.Read();

                    string psEmail = Convert.ToString(lector["email"]);
                    email.fnEnviarCorreoAtencionSAT(lector["email"].ToString() + ", " + clsComun.fnObtenerParamentro("emailSAT"),
                                                             Resources.resCorpusCFDIEs.varTicketSATSubject, psMensaje, lector["email"].ToString(), psArchivo);
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarNotificacionSAT", "clsIncidencias");
            }
            finally
            {
                con.Close();
            }
        }
    }

    /// <summary>
    /// Guarda en la base de datos el registro del incidente
    /// </summary>
    /// <param name="psIdCategoria">Identificador de la categoría general a la que pertenece el incidente</param>
    /// <param name="psDescripción">Descripción del incidente sucedido</param>
    /// <param name="psIdUsuarioSop">identificador del usuario que atendera el incidente</param>
    /// <returns>Devuelve una cadena con el número de ticket a ocho caracteres</returns>
    public string fnEnviarTicket(string psIdCategoria, string psDescripción, int pnIdUsuarioSop, string psRuta, int pnIdRelacion)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                int nIdUsuario = clsComun.fnUsuarioEnSesion().id_usuario;
                string sTicket = clsGeneraLlaves.GenerarTicket();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Incidente_Ins";
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario", nIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("@nTipo_Incidente", psIdCategoria));
                    cmd.Parameters.Add(new SqlParameter("@sDescripcion", psDescripción));
                    cmd.Parameters.Add(new SqlParameter("@sTicket", sTicket));
                    cmd.Parameters.Add(new SqlParameter("@nid_Usuario_Sop", pnIdUsuarioSop));
                    cmd.Parameters.Add(new SqlParameter("@nId_Relacion", pnIdRelacion));

                    int retVal = cmd.ExecuteNonQuery();

                    if (retVal != 0)
                    {
                        fnEnviarNotificacion(sTicket, psIdCategoria, psRuta, pnIdRelacion);
                        return sTicket;
                    }
                    else
                        throw new Exception("No se inserto ningún registro");
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnEnviarTicket", "clsIncidencias");
                return string.Empty;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarTicket", "clsIncidencias");
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }
    }

    /// <summary>
    /// Obtiene el estatus del los Tickets
    /// </summary>
    public DataTable fnEstatusTicket()
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Usuarios_Soporte_sel";

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }             
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnEstatusTicket", "clsIncidencias");
            }
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// Obtiene informacion de las incidencias segun los filtros que se le manden

    /// </summary>
    /// <param name="psEstatus">estatus de la incidencia</param>
    /// <param name="psTipoInc">Identificador del tipo de incidente</param>
    /// <param name="psFechaReg">Fecha de registro del incidente</param>
    /// <param name="psUsuarioSop">Identificador del usuario de soporte</param>
    /// <param name="psTicketPro">Identificador del ticket de problema</param>
    public DataTable fnGetIncidenciasbyFiltros(string psEstatus, string psTipoInc, string psFechaReg, string psUsuarioSop, string psTicketPro, string psFechareg2, string psTicket)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    DataTable dtAuxiliar = null;
                    dtAuxiliar = new DataTable();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_IncidenciasFiltros_temp";

                    if (!(psEstatus == string.Empty)){
                        cmd.Parameters.Add(new SqlParameter("@psEstatus", psEstatus));
                    }
                    else{
                        cmd.Parameters.Add(new SqlParameter("@psEstatus", null));
                    }

                    if (!(psTipoInc == string.Empty)){
                        cmd.Parameters.Add(new SqlParameter("@nTipoIncidente", psTipoInc));
                    }
                    else{
                        cmd.Parameters.Add(new SqlParameter("@nTipoIncidente", null));
                    }

                    if (!(psFechaReg == null)){
                        cmd.Parameters.Add(new SqlParameter("@sFechaReg", psFechaReg));
                    }
                    else{
                        cmd.Parameters.Add(new SqlParameter("@sFechaReg", null));
                    }

                    if (!(psFechareg2 == null)){
                        cmd.Parameters.Add(new SqlParameter("@sFechaReg2", psFechareg2));
                    }
                    else{
                        cmd.Parameters.Add(new SqlParameter("@sFechaReg2", null));
                    }

                    if (!(psUsuarioSop == string.Empty)){
                        cmd.Parameters.Add(new SqlParameter("@nUsuarioSop", psUsuarioSop));
                    }
                    else{
                        cmd.Parameters.Add(new SqlParameter("@nUsuarioSop", null));
                    }

                    if (!(psTicketPro == string.Empty)){
                        cmd.Parameters.Add(new SqlParameter("@sTicketProblema", psTicketPro));
                    }
                    else{
                        cmd.Parameters.Add(new SqlParameter("@sTicketProblema", null));
                    }

                    if (!(psTicket == string.Empty)){
                        cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    }
                    else{
                        cmd.Parameters.Add(new SqlParameter("@sTicket", null));
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)){
                        da.Fill(dtAuxiliar);
                    }

                    return dtAuxiliar;
                }
            }
            catch(Exception ex)
            {
                return null;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnGetIncidenciasbyFiltros", "clsIncidencias");
            }
        }
    }

    /// <summary>
    ///obtiene el idproblema segun el ticket
    /// </summary>
    /// <param name="psTicketProblema">Identificador del ticket del problema</param>
    public int fnObtieneIdProblemabyTicket(string psTicketProblema)
    {
        int nRetorno = 0;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_ProblemaId_sel";
                    cmd.Parameters.Add(new SqlParameter("@sProblema", psTicketProblema));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneIdProblemabyTicket", "clsIncidencias");
            }
         return nRetorno;
        }
    }

    /// <summary>
    /// Obtiene todas las incidencias asignadas al usuario activo
    /// </summary>
    /// <param name="pnIdUsuario">Identificador del usuario activo</param>
    public DataTable fnObtieneIncidenciasporUsuario(int pnIdUsuario)
    {
        DataTable dtAuxiliar = new DataTable();;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Incidencias_Usuario_sel";
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario", pnIdUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneIncidenciasporUsuario", "clsIncidencias");
            }
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// Obtiene el la informacion según el identificador de la incidencia
    /// </summary>
    /// <param name="pnTipoIncidente">Identificador del incidente</param>
    public DataSet fnObtenerInformacionIncidencia(int pnIdIncidente)
    {
        DataSet dtAuxiliar = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Incidencias_sel";
                    cmd.Parameters.Add(new SqlParameter("@nId_Incidencia", pnIdIncidente));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerInformacionIncidencia", "clsIncidencias");
            }
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// Obtiene el identificador del usuario de soporte que atendera el tipo de incidencia
    /// </summary>
    /// <param name="pnTipoIncidente">Identificador del tipo de incidente</param>
    public DataSet fnObtieneUsuarioSoporte(int pnTipoIncidente)
    {
        DataSet dtAuxiliar = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Incidente_Encargado_Sel";
                    cmd.Parameters.Add(new SqlParameter("@nIncidencia", pnTipoIncidente));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneUsuarioSoporte", "clsIncidencias");
            }          
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// Obtiene el la informacion del usuario según el identificador del tipo de  incidencia
    /// </summary>
    /// <param name="pnIdIncidente">Identificador del tipo de incidente</param>
    public int fnObtieneUsuarioSoporteporTipoIncidente(int pnIdIncidente)
    {
        int nRetorno = 0;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_IncidenciasReasigna_sel";
                    cmd.Parameters.Add(new SqlParameter("@nId_Incidencia", pnIdIncidente));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneUsuarioSoporteporTipoIncidente", "clsIncidencias");
            }
        }
        return nRetorno;
    }

    /// <summary>
    /// obtiene todos los usuarios del catalogo de usuarios de soporte
    /// </summary>
    public DataTable fnObtieneUsuariosSoporte()
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Usuarios_Sop_sel";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneUsuariosSoporte", "clsIncidencias");
            } 
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// obtiene la informacion del usuario de soporte segun el identificador
    /// </summary>
    /// <param name="pnIdUsuario">Identificador del usuario</param>
    public DataSet fnObtenerUsuarioSoporteInfo(int pnIdUsuario)
    {
        DataSet dtAuxiliar = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Usuarios_Soporte_Id_sel";
                    cmd.Parameters.Add(new SqlParameter("@nId_usuario", pnIdUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtenerUsuarioSoporteInfo", "clsIncidencias");
            }
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// obtiene todos los tickets de incidencia
    /// 
    public DataTable fnObtieneTicketIncidencia()
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Ticket_Incidencia_sel";

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneTicketIncidencia", "clsIncidencias");
            } 
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// obtiene todos los tickets de incidencia por usuario
    /// 
    /// </summary>
    /// <param name="pnIdUsuarioSop">identificador de usuario de soporte</param>
    public DataTable fnObtieneTicketsIncidenciaporUsuario(int pnIdUsuarioSop)
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Incidencias_Por_Usuario_sel";
                    cmd.Parameters.Add(new SqlParameter("@nIdUsuarioSop", pnIdUsuarioSop));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneTicketsIncidenciaporUsuario", "clsIncidencias");
            }
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// obtiene todos los tickets de porblema de la tabla de incidencias
    /// 
    public DataTable fnObtieneTicketProblemadeIncidencia()
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(conCuenta)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_Ticket_Inc_Problema_sel";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnObtieneTicketProblemadeIncidencia", "clsIncidencias");
            }
        }
        return dtAuxiliar;
    }

    /// <summary>
    /// Reasigna la incidencia a otro usuario de soporte
    /// </summary>
    /// <param name="pnIdIncidente">Identificador del tipo de incidente</param>
    /// <param name="pnIdUsuarioAnt">Identificador del usuario anterior</param>
    /// <param name="pnIdUsuarioPos">Identificador del usuario posterior</param>
    /// <param name="pnTipoIncidente">Identificador del tipo de incidente</param>
    public int fnReasignaIncidencia(int pnIdIncidente, int pnIdUsuarioAnt, int pnIdUsuarioPos, int pnTipoIncidente)
    {
        int nRetorno = 0;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_IncidenteReasignacion_Upd";
                    cmd.Parameters.Add(new SqlParameter("@nId_Incidente", pnIdIncidente));
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario_Pos", pnIdUsuarioPos));
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario_Ant", pnIdUsuarioAnt));
                    cmd.Parameters.Add(new SqlParameter("@nId_Tipo_Incidente", pnTipoIncidente));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnReasignaIncidencia", "clsIncidencias");
            }
        }
        return nRetorno;
    }

    /// <summary>
    /// cambia el estatus de la incidencia a terminado
    /// </summary>
    /// <param name="psIdIncidente">Identificador del tipo de incidente</param>
    public bool fnTerminaIncidencia(int pnIdIncidente)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {                
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_Ctp_IncidenteFinalizado_Upd";
                    cmd.Parameters.Add(new SqlParameter("@nId_Incidente", pnIdIncidente));
                    cmd.ExecuteScalar();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnTerminaIncidencia", "clsIncidencias");
            }
        }
        return bRetorno;
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
            string Extensiones = clsComun.fnObtenerParamentro("Extensiones");
            string[] Extension = Extensiones.Split(',');

            string[] psExtension = null;
            psExtension = psArchivo.Split('.');
            string Ext = System.IO.Path.GetExtension(psArchivo);
            //Ext = psExtension[1];
            foreach (string ExVal in Extension)
            {
                if ("."+ExVal.Trim() == Ext)
                {
                    valor = true;
                    return valor;
                }
            }
           
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnverificaarchivo", "clsIncidencias");
        }
        return valor;
    }

    /// <summary>
    /// verifica el tamaño del archivo
    /// 
    /// </summary>
    /// <param name="psTamanio">tamaño del archivo en KB</param>
    public bool fnVerificaTamanioMax(int pnTamanio)
    {
        bool valor = false;
        try
        {

            int psMaximo = Convert.ToInt32(clsComun.fnObtenerParamentro("MaxFile"));

            if (pnTamanio <= psMaximo)
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
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnVerificaTamanioMax", "clsIncidencias");
        }
        return valor;
    }
}
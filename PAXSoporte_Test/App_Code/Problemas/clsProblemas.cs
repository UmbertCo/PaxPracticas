using System;
using System.Web;
using System.Linq;
using System.Data;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// Summary description for clsBusquedaIncidentes
/// </summary>
public class clsBusquedaIncidentes
{
    private DataTable dtAux;

    /// <summary>
    /// Actualiza el estatus del sat si el problema requiere notificacion
    /// </summary>
    /// <param name="psidIncidente">Identificador del problema</param>
    /// <param name="psNotificador">persona que autoriza</param>
    /// <param name="psEstatusSat">estatus del sat</param>
    /// <param name="psIdNotificacion">identificador de la notificacion del sat</param>
    public bool fnActualizaEstatusSAT(int psidProblema, string psNotificador, int psEstatusSat, string psIdNotificacion)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ProblemaEstatusSAT_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psidProblema));
                    cmd.Parameters.Add(new SqlParameter("@sNotificador", psNotificador));
                    cmd.Parameters.Add(new SqlParameter("@sEstatusSAT", psEstatusSat));
                    cmd.Parameters.Add(new SqlParameter("@nId_notificacion", psIdNotificacion));
                    cmd.ExecuteNonQuery(); 
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnActualizaEstatusSAT", "clsBusquedaIncidentes");
            }
            finally
            {
                con.Close();
            }
        }
        return bRetorno;
    }


    /// <summary>
    /// Actualiza la fecha de atencion de un problema y el estatus del problema
    /// </summary>
    /// <param name="psProblema">Identificador del problema</param>
    public bool fnActualizaFechaRegistroProblema(int psProblema)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ProblemaFechaAtencion_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psProblema));
                    cmd.ExecuteNonQuery();
                }
                bRetorno = true;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnActualizaFechaRegistroProblema", "clsBusquedaIncidentes");
            }
            finally
            {
                con.Close();
            }
        }
        return bRetorno;
    }


    /// <summary>
    /// Actualiza la fecha de notificacion al SAT según el identificador del problema
    /// </summary>
    /// <param name="psIdProblema">Identificador del problema</param>
    public bool fnActualizaNotificacionSAT(int psIdProblema)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
                    ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ProblemaFechaNotificacionSAT_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psIdProblema));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnActualizaNotificacionSAT", "clsBusquedaIncidentes");
            }
            finally
            {
                con.Close();
            }
        }
        return bRetorno;
    }


    /// <summary>
    /// Actualiza la solucion de usuario y la de soporte segun el identificador de problema
    /// </summary>
    /// <param name="psidProblema">Identificador del problema</param>
    /// <param name="psSolucionUsuario">Solución que se le dio al usuario</param>
    /// <param name="psSolucionSoporte">Solución que se le dio al problema</param>
    public bool fnActualizaProblema(int psidProblema, string psSolucionUsuario, string psSolucionSoporte)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
              ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ProblemaAtendido_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psidProblema));
                    if (psSolucionUsuario == "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@sSolucionUsuario", null));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@sSolucionUsuario", psSolucionUsuario));
                    }
                    if (psSolucionSoporte == "")
                    {
                        cmd.Parameters.Add(new SqlParameter("@sSolucionSoporte", null));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@sSolucionSoporte", psSolucionSoporte));
                    }
                    cmd.ExecuteNonQuery(); 
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnActualizaProblema", "clsBusquedaIncidentes");
            }
            finally
            {
                con.Close();
            }

        }
        return bRetorno;
    }


    /// <summary>
    /// Actualiza el estatus de prueba y la solucion de prueba cuando estas ya fueron realizadas
    /// </summary>
    /// <param name="psidIncidente">Identificador del incidente</param>
    /// <param name="psSolucionPrueba">Descripción de la solución de pruebas</param>
    /// <param name="psEstatusPrueba">Estatus del incidente</param>
    public bool fnActualizaProblemaPrueba(int psidProblema, string psSolucionPrueba, int psEstatusPrueba)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ProblemaEstatusPrueba_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psidProblema));
                    cmd.Parameters.Add(new SqlParameter("@sSolucionPrueba", psSolucionPrueba));
                    cmd.Parameters.Add(new SqlParameter("@sEstatusPrueba", psEstatusPrueba));
                    cmd.ExecuteNonQuery(); 
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnActualizaProblemaPrueba", "clsBusquedaIncidentes");
            }
            finally
            {
                con.Close();
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
    private void fnEnviarNotificacion(string psTicket, string psIdCategoria, string psRuta)
    {
        clsGeneraEMAIL email = new clsGeneraEMAIL();

        //giSql = clsComun.fnCrearConexion("conControl");
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
            ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuario_Reporta_Problema", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    SqlDataReader lector = cmd.ExecuteReader();

                    lector.Read();

                    email.fnEnviarCorreo(clsComun.fnUsuarioEnSesion().email + ", " + lector["email"].ToString(),
                          string.Format(Resources.resCorpusCFDIEs.varTicketSubject, psTicket, clsComun.fnUsuarioEnSesion().userName),
                          string.Format(Resources.resCorpusCFDIEs.varTicketMailBody1, psTicket, lector["nombre"].ToString()), psRuta);
                }
                catch (Exception ex)
                {
                    con.Close();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarNotificacion", "clsBusquedaIncidentes");
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }


    /// <summary>
    /// Envía un correo electrónico tanto al usuario que levanto el ticket como al área de soporte
    /// El correo contiene la información relacionada al problema
    /// </summary>
    /// <param name="psTicket">Número de ticket generado</param>
    /// <param name="psIdCategoria">Identificador de la categoría a la que pertenece el problema</param>
    /// <param name="psIdUsuario">Id del usuario al que se enviara la notificación</param>
    public bool fnEnviarNotificacionAtencionProblema(string psTicket, string psIdCategoria, string psMensaje, int psIdUsuario)
    {
        bool bRetorno = false;
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        //giSql = clsComun.fnCrearConexion("conControl");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
            ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuario_Reporta_Problema", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    SqlDataReader lector = cmd.ExecuteReader();

                    lector.Read();

                    DataSet dtAuxiliar = null;
                    int idUsuario = (Convert.ToInt32(psIdUsuario));
                    dtAuxiliar = fnObtenerUsuarioSoporteInfo(idUsuario);
                    foreach (DataRow renglon in dtAuxiliar.Tables[0].Rows)
                    {
                        string psEmail = Convert.ToString(renglon["email"]);

                        email.fnEnviarCorreoAtencionIncidencia(psEmail + ", " + lector["email"].ToString(),
                                                             Resources.resCorpusCFDIEs.varTicketAtencion, psMensaje, lector["email"].ToString());
                    }
                    bRetorno = true;
                }
                catch (Exception ex)
                {
                    con.Close();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarNotificacionAtencionProblema", "clsBusquedaIncidentes");
                }
                finally
                {
                    con.Close();
                }
            }
        }
        return bRetorno;
    }


    /// <summary>
    /// Envía un correo electrónico tanto al usuario que levanto el ticket como al área de soporte
    /// El correo contiene la información relacionada al problema
    /// </summary>
    /// <param name="psTicket">Número de ticket generado</param>
    /// <param name="psIdCategoria">Identificador de la categoría a la que pertenece el problema</param>
    /// <param name="psMensaje">Mensaje del correo</param>
    /// <param name="psArchivo">Archivo que se adjuntará al correo</param>
    public void fnEnviarNotificacionPruebas(string psTicket, string psIdCategoria, string psMensaje, string psArchivo)
    {
        clsGeneraEMAIL email = new clsGeneraEMAIL();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuario_Reporta_Problema", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    SqlDataReader lector = cmd.ExecuteReader();

                    lector.Read();

                    //email.EnviarCorreoAtencionPruebas(lector["email"].ToString() + ", " + clsComun.fnObtenerParamentro("emailSAT"),
                    //Resources.resCorpusCFDIEs.varTicketSATSubject, psMensaje, lector["email"].ToString(), psArchivo);

                    email.fnEnviarCorreoAtencionPruebas(lector["email"].ToString(),
                                                        Resources.resCorpusCFDIEs.varTicketSubject, psMensaje, lector["email"].ToString(), psArchivo);
                }
                catch (Exception ex)
                {
                    con.Close();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarNotificacionAtencionProblema", "clsBusquedaIncidentes");
                }
                finally
                {
                    con.Close();
                }
            }
        }

    }


    /// <summary>
    /// Envía un correo electrónico tanto al usuario que levanto el ticket como al área de soporte
    /// El correo contiene la información relacionada al problema
    /// </summary>
    /// <param name="psTicket">Número de ticket generado</param>
    /// <param name="psIdCategoria">Identificador de la categoría a la que pertenece el problema</param>
    /// <param name="psMensaje">Mensaje de la notificación</param>
    /// <param name="psArchivo">Archivo para adjuntar al correo</param>
    public void fnEnviarNotificacionSAT(string psTicket, string psIdCategoria, string psMensaje, string psArchivo)
    {
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuario_Reporta_Problema", con))
            {
                try
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    SqlDataReader lector = cmd.ExecuteReader();

                    lector.Read();

                    email.fnEnviarCorreoAtencionSAT(lector["email"].ToString() + ", " + clsComun.fnObtenerParamentro("emailSAT"),
                                                         Resources.resCorpusCFDIEs.varTicketSATSubject, psMensaje, lector["email"].ToString(), psArchivo);
                }
                catch (Exception ex)
                {
                    con.Close();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarNotificacionSAT", "clsBusquedaIncidentes");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        //}
    }


    /// <summary>
    /// Inserta y notifica acerca del ticket de problema
    /// </summary>
    /// <param name="psIdCategoria">Categoria del problema</param>
    /// <param name="psDescripción">Descripción del problema</param>
    /// <param name="psIdUsuarioSop">Id del usuario de soporte</param>
    /// <param name="psRuta">Ruta de la imagen guardara para el problema</param>
    /// <returns></returns>
    public string fnEnviarTicket(string psIdCategoria, string psDescripción, int psIdUsuarioSop, string psRuta)
    {

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
            ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                int nIdUsuario = clsComun.fnUsuarioEnSesion().id_usuario;
                string sTicket = clsGeneraLlaves.GenerarTicket();

                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Problemas_Cat_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", nIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("nTipo_Incidente", psIdCategoria));
                    cmd.Parameters.Add(new SqlParameter("sDescripcion", psDescripción));
                    cmd.Parameters.Add(new SqlParameter("sTicket", sTicket));
                    cmd.Parameters.Add(new SqlParameter("nid_Usuario_Sop", psIdUsuarioSop));
                    int retVal = cmd.ExecuteNonQuery();

                    if (retVal != 0)
                    {
                        fnEnviarNotificacion(sTicket, psIdCategoria, psRuta);
                        return sTicket;
                    }
                    else
                        throw new Exception("No se inserto ningún registro");
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos, "fnEnviarTicket", "clsBusquedaIncidentes");
                return string.Empty;
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnEnviarTicket", "clsBusquedaIncidentes");
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }
    }


    /// <summary>
    /// Termina la incidencia relacionada al problema cuando este fue resuelto
    /// </summary>
    /// <param name="psidIncidente">Identificador del incidente</param>
    public bool fnFInalizaIncidenciabyProblema(int psidIncidente)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
                  ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_IncidenteFinalizadoByTicketProblema_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Incidente", psidIncidente));
                    cmd.ExecuteNonQuery();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnFInalizaIncidenciabyProblema", "clsBusquedaIncidentes");
            }
            finally
            {
                con.Close();
            }
        }
        return bRetorno;
    }


    /// <summary>
    /// Obtiene informacion de los problemas segun los filtros que se le manden
    /// </summary>
    /// <param name="psEstatus">estatus del problema</param>
    /// <param name="psTipoInc">Identificador del tipo de incidente</param>
    /// <param name="psFechaReg">Fecha de registro del problema</param>
    /// <param name="psUsuarioSop">Identificador del usuario de soporte</param>
    /// /// <param name="psFechaReg2">Fecha final de registro del problema</param>
    /// /// <param name="psTicket">Numero de ticket del problema</param>
    public DataTable fnGetProblemasbyFiltros(string psEstatus, string psTipoInc, string psFechaReg, string psUsuarioSop, string psFechareg2, string psTicket)
    {
        try
        {

            DataTable dtAuxiliar = null;
            dtAuxiliar = new DataTable();

            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
                ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ProblemasFiltros_temp", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!(psEstatus == ""))
                    {
                        cmd.Parameters.Add(new SqlParameter("@psEstatus", psEstatus));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@psEstatus", null));
                    }

                    if (!(psTipoInc == ""))
                    {
                        cmd.Parameters.Add(new SqlParameter("@nTipoIncidente", psTipoInc));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@nTipoIncidente", null));
                    }

                    if (!(psFechaReg == null))
                    {
                        cmd.Parameters.Add(new SqlParameter("@sFechaReg", psFechaReg));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@sFechaReg", null));
                    }

                    if (!(psFechareg2 == null))
                    {
                        cmd.Parameters.Add(new SqlParameter("@sFechaReg2", psFechareg2));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@sFechaReg2", null));
                    }

                    if (!(psUsuarioSop == ""))
                    {
                        cmd.Parameters.Add(new SqlParameter("@nUsuarioSop", psUsuarioSop));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@nUsuarioSop", null));
                    }

                    if (!(psTicket == ""))
                    {
                        cmd.Parameters.Add(new SqlParameter("@sTicket", psTicket));
                    }
                    else
                    {
                        cmd.Parameters.Add(new SqlParameter("@sTicket", null));
                    }


                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }

                    return dtAuxiliar;
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnGetProblemasbyFiltros", "clsBusquedaIncidentes");
            return null;
        }

    }


    /// <summary>
    /// Obtiene el la informacion según el identificador del problema
    /// </summary>
    /// <param name="psidproblema">Identificador del incidente</param>
    public DataSet fnObtenerInformacionProblema(int psidproblema)
    {
        DataSet dtAuxiliar = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {

                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Problemas_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psidproblema));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtenerInformacionProblema", "clsBusquedaIncidentes");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    ///  Realiza una busqueda de los incidentes en la base de datos.
    /// </summary>
    /// <param name="psEstatus">Estatus del incidente</param>
    /// <param name="pdFecIni">Fecha de inicio del incidente</param>
    /// <param name="pdFecFin">Fecha de termino del incidente</param>
    /// <param name="psUrgencia">Urgencia del incidente</param>
    /// <param name="psImpacto">Impacto del incidente</param>
    /// <param name="psPalabrasClave">Palabras clave para la busqueda del incidente</param>
    /// <returns></returns>
    public DataTable fnObtenerIncidentes(string psEstatus, DateTime pdFecIni, DateTime pdFecFin,
                                         string psUrgencia, string psImpacto, string psPalabrasClave)
    {
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64
              (ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            dtAux = new DataTable();
            using (SqlCommand cmd = new SqlCommand("usp_Ctp_Incidentes_Sel", con))
            {

                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //NOTA: Cambiar por id de usuario logeado
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", 1));   //Pendiente?
                    cmd.Parameters.Add(new SqlParameter("dFecha_Inicial", pdFecIni));
                    cmd.Parameters.Add(new SqlParameter("dFecha_Final", pdFecFin));

                    if (psEstatus != "-1")
                        cmd.Parameters.Add(new SqlParameter("cEstatus", psEstatus));

                    if (psUrgencia != "-1")
                        cmd.Parameters.Add(new SqlParameter("nUrgencia", psUrgencia));

                    if (psImpacto != "-1")
                        //iSql.AgregarParametro("nImpacto", psImpacto);
                        cmd.Parameters.Add(new SqlParameter("nImpacto", psImpacto));

                    if (!string.IsNullOrEmpty(psPalabrasClave))
                    {

                        cmd.Parameters.Add(new SqlParameter("sPalabrasClave", psPalabrasClave));
                    }


                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAux);
                    }
                }
                catch (Exception ex)
                {
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtenerIncidentes", "clsBusquedaIncidentes");
                }
            }
            return dtAux;
        }
    }


    /// <summary>
    /// Obtiene la relacion de todas las incidencias con un problema
    /// </summary>
    /// <param name="psIdIncidente">Identificador del tipo de problema</param>
    public DataSet fnObtieneIncidenciasbyProblema(int psIdProblema)
    {
        DataSet dtAuxiliar = new DataSet();
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ActualizaIncidenciabyProblema_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psIdProblema));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtieneIncidenciasbyProblema", "clsBusquedaIncidentes");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    /// Obtiene todas los problemas asignados al usuario activo
    /// </summary>
    /// <param name="nId_Usuario">Identificador del usuario activo</param>
    public DataTable fnObtieneProblemasporUsuario(int psIdUsuario)
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
            ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Problemas_Usuario_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario", psIdUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtieneProblemasporUsuario", "clsBusquedaIncidentes");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    /// Obtiene todos los tickets de problemas
    /// </summary>
    public DataTable fnObtieneTicketProblema()
    {
        DataTable dtAuxiliar = new DataTable();
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Ticket_Problema_sel", con))
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtieneTicketProblema", "clsBusquedaIncidentes");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    /// Obtiene todos los tickets de problema por usuario
    /// </summary>
    /// <param name="psIdUsuarioSop">Identificador de usuario de soporte</param>
    public DataTable fnObtieneTicketsProblemaporUsuario(int psIdUsuarioSop)
    {
        DataTable dtAuxiliar = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Problemas_Por_Usuario_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@nIdUsuarioSop", psIdUsuarioSop);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch(Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtieneTicketsProblemaporUsuario", "clsBusquedaIncidentes");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    /// Obtiene todos los tickets de problemas terminados
    /// </summary>
    public DataTable fnObtieneTicketProblemaTerminados()
    {
        DataTable dtAuxiliar = new DataTable();
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Ticket_Problema__Terminado_sel", con))
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
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtieneTicketProblemaTerminados", "clsBusquedaIncidentes");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    /// Obtiene el identificador del usuario de soporte que atendera el tipo de incidencia
    /// </summary>
    /// <param name="psTipoIncidente">Identificador del tipo de incidente</param>
    public DataSet fnObtieneUsuarioSoporte(int psTipoIncidente)
    {
        DataSet dtAuxiliar = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
            ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Incidente_Encargado_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nIncidencia", psTipoIncidente));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtieneUsuarioSoporte", "clsBusquedaIncidentes");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    /// obtiene la informacion del usuario de soporte segun el identificador
    /// </summary>
    /// <param name="psIdUsuario">Identificador del usuario</param>
    public DataSet fnObtenerUsuarioSoporteInfo(int psIdUsuario)
    {
        DataSet dtAuxiliar = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuarios_Soporte_Id_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario", psIdUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtAuxiliar);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtenerUsuarioSoporteInfo", "clsBusquedaIncidentes");
            }
        }
        return dtAuxiliar;
    }


    /// <summary>
    /// Obtiene el la informacion del usuario según el identificador del tipo de  problema
    /// </summary>
    /// <param name="psIdIncidente">Identificador del tipo de incidente</param>
    public int fnObtieneUsuarioSoporteporTipoProblema(int psIdProblema)
    {
        Int32 nRetorno = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
            ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ProblemasReasigna_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psIdProblema));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnObtieneUsuarioSoporteporTipoProblema", "clsBusquedaIncidentes");
            }
            finally
            {
                con.Close();
            }
        }
        return nRetorno;
    }


    /// <summary>
    ///Insercion de un nuevo registro de problema
    /// </summary>
    /// <param name="psTicketproblema">Numero de ticket del problema</param>
    /// <param name="psIdUsuario">Id del usuario que reporta</param>
    /// <param name="psIdTipoProblema">Tipo del problema</param>
    /// <param name="psIdUsuarioSop">Is usuario soporte</param>
    /// <param name="psDesc">Descripcion del problema</param>
    /// <returns></returns>
    public int fnProblemasIns(string psTicketproblema, int psIdUsuario, int psIdTipoProblema, int psIdUsuarioSop, string psDesc)
    {
        Int32 nRetorno = 0;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Problemas_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sticket_problema", psTicketproblema));
                    cmd.Parameters.Add(new SqlParameter("@sidUsuario", psIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("@sIdTIpoProblema", psIdTipoProblema));
                    cmd.Parameters.Add(new SqlParameter("@sIdUsuarioSoporte", psIdUsuarioSop));
                    cmd.Parameters.Add(new SqlParameter("@sDecripcion", psDesc));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar()); con.Close();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnProblemasIns", "clsBusquedaIncidentes");
            }
        }
        return nRetorno;
    }


    /// <summary>
    ///Insercion de un nuevo registro de problema
    /// </summary>
    /// <param name="psIdIncidencia">Id del incidente</param>
    /// <param name="psIdProblema">Id del problema a relacionar</param>
    /// <returns></returns>
    public int fnProblemasInsRel(int psIdIncidencia, int psIdProblema)
    {
        Int32 nRetorno = 0;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
                 ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Problemas_Ins_rel_ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@sidIncidencia", psIdIncidencia));
                    cmd.Parameters.Add(new SqlParameter("@nIdProblema", psIdProblema));
                    nRetorno = cmd.ExecuteNonQuery(); con.Close();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnProblemasInsRel", "clsBusquedaIncidentes");
            }
        }
        return nRetorno;
    }


    /// <summary>
    /// Reasigna la incidencia a otro usuario de soporte
    /// </summary>
    /// <param name="psIdIncidente">Identificador del tipo de problema</param>
    /// /// <param name="psIdUsuarioAnt">Identificador del usuario anterior</param>
    /// /// <param name="psIdUsuarioPos">Identificador del usuario posterior</param>
    public int fnReasignaProblema(int psIdProblema, int psIdUsuarioAnt, int psIdUsuarioPos, int psTipoIncidente)
    {
        Int32 nRetorno = 0;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
            ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ProblemaReasignacion_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psIdProblema));
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario_Pos", psIdUsuarioPos));
                    cmd.Parameters.Add(new SqlParameter("@nId_Usuario_Ant", psIdUsuarioAnt));
                    cmd.Parameters.Add(new SqlParameter("@nId_Tipo_Incidente", psTipoIncidente));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar()); con.Close();
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnReasignaProblema", "clsBusquedaIncidentes");
            }
        }
        return nRetorno;

    }


    /// <summary>
    /// Cambia el estatus del problema a terminado
    /// </summary>
    /// <param name="psIdProblema">Identificador del tipo de problema</param>
    public bool fnTerminaProblema(int psIdProblema)
    {
        bool bRetorno = false;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(
               ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_ProblemaFinalizado_Upd", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nId_Problema", psIdProblema));
                    cmd.ExecuteNonQuery(); con.Close();
                    bRetorno = true;
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia, "fnTerminaProblema", "clsBusquedaIncidentes");
            }
        }
        return bRetorno;
    }


    /// <summary>
    /// Verifica la extension del archivo a enviar por correo
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
                if ("." + ExVal.Trim() == Ext)
                {
                    valor = true;
                    return valor;
                }
            }
           
        }
        catch (Exception ex)
        {
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
        }
        return valor;
    }


    /// <summary>
    /// Verifica el tamaño del archivo
    /// </summary>
    /// <param name="psTamanio">Tamaño del archivo en KB</param>
    public bool fnVerificaTamanioMax(int psTamanio)
    {
        bool valor = false;
        try
        {

            int psMaximo = Convert.ToInt32(clsComun.fnObtenerParamentro("MaxFile"));

            if (psTamanio <= psMaximo)
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
            //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
        }
        return valor;
    }
    

}
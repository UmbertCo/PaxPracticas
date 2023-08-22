using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsTimbradoNomina
/// </summary>
public class clsTimbradoNomina
{
    
    /// <summary>
    /// Función que revisa si existe un registro de pago de un empleado en una Nómina
    /// </summary>
    /// <param name="pnId_Nomina">ID de la Nómina</param>
    /// <param name="pnId_Empleado">ID del Empleado</param>
    /// <returns></returns>
    public bool fnExistePagoNominaPorIdEmpleadoIdNomina(int pnId_Nomina, int pnId_Empleado)
    {
        bool bResultado = false;
        DataTable dtPagosNomina = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Pago_sel_ExistePorIdEmpleadoIdNomina";

                    cmd.Parameters.Add(new SqlParameter("nId_Nomina", pnId_Nomina));
                    cmd.Parameters.Add(new SqlParameter("nId_Empleado", pnId_Empleado));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtPagosNomina);
                    }
                }
            }

            if (dtPagosNomina.Rows.Count > 0)
                bResultado = true;

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return bResultado;
    }

    /// <summary>
    /// Función que se encarga de generar una Nómina en base a una Nómina registrada
    /// </summary>
    /// <param name="pnId_Pago_Nomina">ID del Pago de la Nómina por Empleado</param>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <param name="pdFechaPago">Fecha de Pago</param>
    /// <param name="pdFechaInicialPago">Fecha Inicial de Pago</param>
    /// <param name="pdFechaFinalPago">Fecha Final de Pago</param>
    /// <param name="pnNumDiasPagados">Numero de días pagados</param>
    /// <returns></returns>
    public bool fnGenerarNomina(int pnId_Pago_Nomina, int pnId_Estructura, DateTime pdFechaPago,
                                DateTime pdFechaInicialPago, DateTime pdFechaFinalPago, decimal pnNumDiasPagados)
    {
        bool bRetorno = false;
        Calendar oCalendario;
        clsOperacionEmpleados cEmpleados = new clsOperacionEmpleados();
        DataTable dtComprobantePagoNomina = new DataTable();
        DataTable dtEmpleados = new DataTable();
        DataTable dtHorasExtra = new DataTable();
        DataTable dtIncapacidades = new DataTable();
        DataTable dtPagoNomina = new DataTable();
        DataTable dtPercepcionesDeducciones = new DataTable();
        DataTable dtPeriodosEstructura = new DataTable();
        DateTime dFechaIngresoRelacionLaboral;
        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
        int nAntiguedad = 0;
        int nId_Periodo = 0;
        int nId_Nomina = 0;
        int nId_Pago_Nomina = 0;
        int nId_Percepcion_Deduccion = 0;
        string sFechaIngresoRelacionLaboral;
        string sClaveNomina = string.Empty;
        try
        {
            dtPagoNomina = fnObtenerPagoNomina(pnId_Pago_Nomina);
            if(dtPagoNomina.Rows.Count <= 0)
                return bRetorno;

            dtEmpleados = cEmpleados.fnExisteEmpleado(Convert.ToInt32(dtPagoNomina.Rows[0]["Id_Empleado"]));
            if(dtEmpleados.Rows.Count <= 0)
                return bRetorno;

            dtComprobantePagoNomina = fnObtenerComprobantePagoNomina(pnId_Pago_Nomina);
            dtPercepcionesDeducciones = fnObtenerPercepcionesDeducciones(pnId_Pago_Nomina);

            if (dtPercepcionesDeducciones.Rows.Count <= 0)
                return bRetorno;

            dtHorasExtra = fnObtenerHorasExtra(pnId_Pago_Nomina);
            dtIncapacidades = fnObtenerIncapacidades(pnId_Pago_Nomina);

            dtPeriodosEstructura = LlenarDropPeriodos(pnId_Estructura);
            nId_Periodo = Convert.ToInt32(dtPeriodosEstructura.Rows[0]["IdTipoPeriodo"].ToString());

            sFechaIngresoRelacionLaboral = dtEmpleados.Rows[0]["FechaInicioRelLaboral"].ToString();
            if (!string.IsNullOrEmpty(sFechaIngresoRelacionLaboral))
            {
                dFechaIngresoRelacionLaboral = Convert.ToDateTime(sFechaIngresoRelacionLaboral);
                TimeSpan tsFechaDiferencia = DateTime.Now - dFechaIngresoRelacionLaboral;
                nAntiguedad = tsFechaDiferencia.Days / 7;
            }  

            oCalendario = dfi.Calendar;

            sClaveNomina = DateTime.Now.ToShortDateString() + oCalendario.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek).ToString() + pnId_Estructura.ToString();

            string cadenaCon = "Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n";

            using (SqlConnection con = new SqlConnection(cadenaCon))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        if (Convert.ToInt32(HttpContext.Current.Session["Id_Nomina"]).Equals(0))
                        {
                            // Se inserta el registro de la Nómina
                            using (SqlCommand cmd = new SqlCommand("usp_nom_Nomina_Ins", con))
                            {
                                cmd.Transaction = tran;
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("dFechaHoraCreacion", DateTime.Now);
                                cmd.Parameters.AddWithValue("sClaveNomina", sClaveNomina);
                                cmd.Parameters.AddWithValue("nid_estructura", pnId_Estructura);
                                cmd.Parameters.AddWithValue("nid_Periodo", nId_Periodo);

                                nId_Nomina = Convert.ToInt32(cmd.ExecuteScalar());

                                if (nId_Nomina.Equals(0))
                                    throw new Exception("No se registro el ID de la Nómina.");
                            }
                        }
                        else
                        {
                            nId_Nomina = Convert.ToInt32(HttpContext.Current.Session["Id_Nomina"]);
                        }
                        

                        // Se inserta el registro del pago de la Nómina del Empleado
                        using (SqlCommand cmd = new SqlCommand("usp_nom_Pagos_Ins", con))
                        {
                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("nId_Nomina", nId_Nomina);
                            cmd.Parameters.AddWithValue("nId_Empleado", Convert.ToInt32(dtPagoNomina.Rows[0]["Id_Empleado"]));
                            cmd.Parameters.AddWithValue("dFechaPago", pdFechaPago);
                            cmd.Parameters.AddWithValue("dFechaInicialPago", pdFechaInicialPago);
                            cmd.Parameters.AddWithValue("dFechaFinalPago", pdFechaFinalPago);
                            cmd.Parameters.AddWithValue("nNumDiasPagados", pnNumDiasPagados);
                            if(!nAntiguedad.Equals(0))
                                cmd.Parameters.AddWithValue("nAntiguedad", nAntiguedad);
                            cmd.Parameters.AddWithValue("nTotal", Convert.ToDecimal(dtPagoNomina.Rows[0]["Total"]));

                            nId_Pago_Nomina = Convert.ToInt32(cmd.ExecuteScalar());

                            if (nId_Pago_Nomina.Equals(0))
                                throw new Exception("No se registro el ID de la Nómina por Empleado.");
                        }

                        // Se registra los datos del Comprobante relacionado a la Nómina
                        using (SqlCommand cmd = new SqlCommand("usp_nom_Comprobante_Pagos_Nomina_Ins", con))
                        {
                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("nId_Pago_Nomina", nId_Pago_Nomina);
                            if (!string.IsNullOrEmpty(dtComprobantePagoNomina.Rows[0]["id_estructura_expedido"].ToString()))
                                cmd.Parameters.AddWithValue("nId_Estructura_Expedido", Convert.ToInt32(dtComprobantePagoNomina.Rows[0]["id_estructura_expedido"]));
                            cmd.Parameters.AddWithValue("sLugar_Expedicion", dtComprobantePagoNomina.Rows[0]["lugar_expedicion"]);
                            cmd.Parameters.AddWithValue("sMetodo_Pago", dtComprobantePagoNomina.Rows[0]["metodo_pago"]);
                            cmd.Parameters.AddWithValue("sNumero_Cuenta", dtComprobantePagoNomina.Rows[0]["numero_cuenta"]);
                            cmd.Parameters.AddWithValue("sMoneda", dtComprobantePagoNomina.Rows[0]["moneda"]);
                            cmd.Parameters.AddWithValue("sRegimen_Fiscal", dtComprobantePagoNomina.Rows[0]["regimen_fiscal"]);
                            cmd.Parameters.AddWithValue("sForma_Pago", dtComprobantePagoNomina.Rows[0]["forma_pago"]);
                            cmd.Parameters.AddWithValue("sTipo_Cambio", dtComprobantePagoNomina.Rows[0]["tipo_cambio"]);

                            int nRegistrosAfectados = cmd.ExecuteNonQuery();

                            if (nRegistrosAfectados <= 0)
                                throw new Exception("No se registro los datos del comprobante ligado a la Nómina.");
                        }

                        // Se registran los detalles de la Nómina (Percepciones y Deducciones)
                        foreach (DataRow renglonDetalle in dtPercepcionesDeducciones.Rows)
                        {
                            // Se registran las percepciones y deducciones
                            using (SqlCommand cmd = new SqlCommand("usp_nom_PercepDedu_Ins", con))
                            {
                                cmd.Transaction = tran;
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("nId_PagoNomina", nId_Pago_Nomina);
                                cmd.Parameters.AddWithValue("nId_TipoPercepDedu", Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]));
                                cmd.Parameters.AddWithValue("sClave", renglonDetalle["Clave"]);
                                cmd.Parameters.AddWithValue("sConcepto", renglonDetalle["Concepto"]);
                                cmd.Parameters.AddWithValue("nImporteGravado", Convert.ToInt32(renglonDetalle["ImporteGravado"]));
                                cmd.Parameters.AddWithValue("nImporteExento", Convert.ToInt32(renglonDetalle["ImporteExento"]));
 
                                nId_Percepcion_Deduccion = Convert.ToInt32(cmd.ExecuteScalar());

                                if (nId_Percepcion_Deduccion.Equals(0))
                                    throw new Exception("No se registro el ID del detalle de la Nómina.");
                            }


                            // Se insertan las horas extras relacionadas a la percepción
                            if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                                Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
                            {
                                DataView dvHorasExtra = new DataView(dtHorasExtra);
                                dvHorasExtra.RowFilter = "Id_PercepDedu= " + renglonDetalle["Id_PercepDedu"];

                                foreach (DataRow renglonHoraExtra in dvHorasExtra.ToTable().Rows)
                                {
                                    using (SqlCommand cmd = new SqlCommand("usp_nom_HorasExtras_Ins", con))
                                    {

                                        cmd.Transaction = tran;
                                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                        cmd.Parameters.AddWithValue("nId_PercepDedu", nId_Percepcion_Deduccion);
                                        cmd.Parameters.AddWithValue("nDias", renglonHoraExtra["Dias"]);
                                        cmd.Parameters.AddWithValue("sTipoHoras", renglonHoraExtra["TipoHoras"]);
                                        cmd.Parameters.AddWithValue("nHorasExtra", Convert.ToInt32(renglonHoraExtra["HorasExtra"]));
                                        cmd.Parameters.AddWithValue("nImportePagado", Convert.ToDouble(renglonHoraExtra["ImportePagado"]));
                  
                                        int nRegistrosAfectados = cmd.ExecuteNonQuery();

                                        if (nRegistrosAfectados.Equals(0))
                                            throw new Exception("No se registro las horas extra.");
                                    }

                                }                            
                           }

                            // Se insertan las incapacidades relacionadas a la deducción
                            if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                                Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
                            {
                                DataView dvIncapacidades = new DataView(dtIncapacidades);
                                dvIncapacidades.RowFilter = "Id_PercepcionDeduccion= " + renglonDetalle["Id_PercepDedu"];

                                foreach (DataRow renglonIncapacidad in dvIncapacidades.ToTable().Rows)
                                {
                                    using (SqlCommand cmd = new SqlCommand("usp_nom_Incapacidades_Ins", con))
                                    {

                                        cmd.Transaction = tran;
                                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                        cmd.Parameters.AddWithValue("nId_PercepcionDeduccion", nId_Percepcion_Deduccion);
                                        cmd.Parameters.AddWithValue("nTipo", renglonIncapacidad["Tipo"]);
                                        cmd.Parameters.AddWithValue("nDiasIncapacidad", renglonIncapacidad["DiasIncapacidad"]);
                                        cmd.Parameters.AddWithValue("nDescuento", Convert.ToInt32(renglonIncapacidad["Descuento"]));

                                        int nRegistrosAfectados = cmd.ExecuteNonQuery();

                                        if (nRegistrosAfectados.Equals(0))
                                            throw new Exception("No se registro de las incapacidades.");
                                    }
                                }
                            }
                        }

                        tran.Commit();

                        HttpContext.Current.Session["Id_Nomina"] = nId_Nomina;

                        bRetorno = true;
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
        }
        return bRetorno;
    }

    /// <summary>
    /// Recupera la lista de los Periodos por Usuario.
    /// </summary>
    /// <param name="nId_usuario">id del usuario</param>
    /// <returns>recupera la lista de los Periodos.</returns>
    public DataTable LlenarDropPeriodos(int nId_Estructura)
    {
        DataTable gdtAuxiliar = new DataTable("Periodos");

        try
        {
            using (SqlConnection con = new SqlConnection("Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n"))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Timbrado_Periodos_Sel";
                    cmd.Parameters.Add(new SqlParameter("@IdEstructura", nId_Estructura));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(gdtAuxiliar);
                    }
                }
                con.Close();
                con.Dispose();
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return gdtAuxiliar;
    }

    /// <summary>
    /// Recupera la lista de los Periodos disponibles.
    /// </summary>
    /// <returns>recupera la lista de los Periodos disponibles.</returns>
    public DataTable LlenarDropPeriodos()
    {
        DataTable gdtAuxiliar = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Periodos_sel";

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(gdtAuxiliar);
                    }
                }
                con.Close();
                con.Dispose();
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        
        return gdtAuxiliar;
    }

    /// <summary>
    /// Registra la nomina
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <param name="psClaveNomina">Clave de la Nómina</param>
    /// <param name="pnId_Empleado">ID del Empleado</param>
    /// <param name="pdFechaPago">Fecha de Pago</param>
    /// <param name="pdFechaInicialPago">Fecha Inicial de Pago</param>
    /// <param name="pdFechaFinalPago">Fecha Final de Pago</param>
    /// <param name="pnNumDiasPagados">Número de días de pagados</param>
    /// <param name="pnAntiguedad">Antigüedad</param>
    /// <param name="dtDetalle">DataTable de Detalle</param>
    /// <param name="dtHorasExtra">DataTable de Horas Extra</param>
    /// <param name="dtIncapacidades">DataTable de Incapacidades</param>
    /// <returns></returns>
    public bool fnRegistrarNomina(int pnId_Estructura, string psClaveNomina, int pnId_Periodo, int pnId_Empleado, DateTime pdFechaPago,
                                DateTime pdFechaInicialPago, DateTime pdFechaFinalPago, decimal pnNumDiasPagados, int pnAntiguedad, decimal nTotal,
                                int pnId_Estructura_Expedido, int pnId_Estado_Expedido, string psMetodo_Pago, string psNumero_Cuenta,
                                string psMoneda, string psRegimenFiscal, string psForma_Pago, string psTipo_Cambio,
                                DataTable dtDetalle, DataTable dtHorasExtra, DataTable dtIncapacidades)
    {
        bool bRetorno = false;
        int nId_Detalle = 0;
        int nId_Nomina = 0;
        int nId_PagoNomina = 0;
        int nRegistrosAfectados = 0;

        HttpContext.Current.Session["Id_Nomina"] = 0;
        HttpContext.Current.Session["IdPagoNomina"] = 0;

        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {
            con.Open();
            using (SqlTransaction tran = con.BeginTransaction())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("usp_nom_Nomina_Ins", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("dFechaHoraCreacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("sClaveNomina", psClaveNomina);
                        cmd.Parameters.AddWithValue("nid_estructura", pnId_Estructura);
                        cmd.Parameters.AddWithValue("nid_Periodo", pnId_Periodo);

                        nId_Nomina = Convert.ToInt32(cmd.ExecuteScalar());

                        if (nId_Nomina.Equals(0))
                            throw new Exception("No se registro el ID de la Nómina.");                       
                    }

                    using (SqlCommand cmd = new SqlCommand("usp_nom_Pagos_Ins", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("nId_Nomina", nId_Nomina);
                        cmd.Parameters.AddWithValue("nId_Empleado", pnId_Empleado);
                        cmd.Parameters.AddWithValue("dFechaPago", pdFechaPago);
                        cmd.Parameters.AddWithValue("dFechaInicialPago", pdFechaInicialPago);
                        cmd.Parameters.AddWithValue("dFechaFinalPago", pdFechaFinalPago);
                        cmd.Parameters.AddWithValue("nNumDiasPagados", pnNumDiasPagados);
                        cmd.Parameters.AddWithValue("nAntiguedad", pnAntiguedad);
                        cmd.Parameters.AddWithValue("nTotal", nTotal);

                        nId_PagoNomina = Convert.ToInt32(cmd.ExecuteScalar());

                        if (nId_PagoNomina.Equals(0))
                            throw new Exception("No se registro el ID de la Nómina por Empleado.");
                    }

                    using (SqlCommand cmd = new SqlCommand("usp_nom_Comprobante_Pagos_Nomina_Ins", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("nId_Pago_Nomina", nId_PagoNomina);
                        if (!pnId_Estructura_Expedido.Equals(0))
                            cmd.Parameters.AddWithValue("nId_Estructura_Expedido", pnId_Estructura_Expedido);
                        cmd.Parameters.AddWithValue("nId_Estado_Expedido", pnId_Estado_Expedido);
                        cmd.Parameters.AddWithValue("sMetodo_Pago", psMetodo_Pago);
                        cmd.Parameters.AddWithValue("sNumero_Cuenta", psNumero_Cuenta);
                        cmd.Parameters.AddWithValue("sMoneda", psMoneda);
                        cmd.Parameters.AddWithValue("sRegimen_Fiscal", psRegimenFiscal);
                        cmd.Parameters.AddWithValue("sForma_Pago", psForma_Pago);
                        cmd.Parameters.AddWithValue("sTipo_Cambio", psTipo_Cambio);

                        nRegistrosAfectados = cmd.ExecuteNonQuery();

                        if (nRegistrosAfectados <= 0)
                            throw new Exception("No se registro los datos del comprobante ligado a la Nómina.");
                    }

                    foreach (DataRow renglonDetalle in dtDetalle.Rows)
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_nom_PercepDedu_Ins", con))
                        {
                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("nId_PagoNomina", nId_PagoNomina);
                            cmd.Parameters.AddWithValue("nId_TipoPercepDedu", Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]));
                            cmd.Parameters.AddWithValue("sClave", renglonDetalle["Clave"]);
                            cmd.Parameters.AddWithValue("sConcepto", renglonDetalle["Concepto"]);
                            cmd.Parameters.AddWithValue("nImporteGravado", Convert.ToInt32(renglonDetalle["ImporteGravado"]));
                            cmd.Parameters.AddWithValue("nImporteExento", Convert.ToInt32(renglonDetalle["ImporteExento"]));
 
                            nId_Detalle = Convert.ToInt32(cmd.ExecuteScalar());

                            if (nId_Detalle.Equals(0))
                                throw new Exception("No se registro el ID del detalle de la Nómina.");
                        }


                        // Se insertan las horas extras relacionadas a la percepción
                        if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                            Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
                        {
                            DataView dvHorasExtra = new DataView(dtHorasExtra);
                            dvHorasExtra.RowFilter = "Id_PercepDedu= " + renglonDetalle["Id_PercepDedu"];

                            foreach (DataRow renglonHoraExtra in dvHorasExtra.ToTable().Rows)
                            {
                                using (SqlCommand cmd = new SqlCommand("usp_nom_HorasExtras_Ins", con))
                                {

                                    cmd.Transaction = tran;
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("nId_PercepDedu", nId_Detalle);
                                    cmd.Parameters.AddWithValue("nDias", renglonHoraExtra["Dias"]);
                                    cmd.Parameters.AddWithValue("sTipoHoras", renglonHoraExtra["TipoHoras"]);
                                    cmd.Parameters.AddWithValue("nHorasExtra", Convert.ToInt32(renglonHoraExtra["HorasExtra"]));
                                    cmd.Parameters.AddWithValue("nImportePagado", Convert.ToDouble(renglonHoraExtra["ImportePagado"]));
                  
                                    nRegistrosAfectados = cmd.ExecuteNonQuery();

                                    if (nRegistrosAfectados.Equals(0))
                                        throw new Exception("No se registro las horas extra.");
                                }

                            }                            
                       }

                        // Se insertan las incapacidades relacionadas a la deducción
                        if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                            Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
                        {
                            DataView dvIncapacidades = new DataView(dtIncapacidades);
                            dvIncapacidades.RowFilter = "Id_PercepcionDeduccion= " + renglonDetalle["Id_PercepDedu"];

                            foreach (DataRow renglonIncapacidad in dvIncapacidades.ToTable().Rows)
                            {
                                using (SqlCommand cmd = new SqlCommand("usp_nom_Incapacidades_Ins", con))
                                {

                                    cmd.Transaction = tran;
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("nId_PercepcionDeduccion", nId_Detalle);
                                    cmd.Parameters.AddWithValue("nTipo", renglonIncapacidad["Tipo"]);
                                    cmd.Parameters.AddWithValue("nDiasIncapacidad", renglonIncapacidad["DiasIncapacidad"]);
                                    cmd.Parameters.AddWithValue("nDescuento", Convert.ToInt32(renglonIncapacidad["Descuento"]));

                                    nRegistrosAfectados = cmd.ExecuteNonQuery();

                                    if (nRegistrosAfectados.Equals(0))
                                        throw new Exception("No se registro de las incapacidades.");
                                }
                            }
                        }
                    }                  
                    tran.Commit();

                    HttpContext.Current.Session["Id_Nomina"] = nId_Nomina;
                    HttpContext.Current.Session["IdPagoNomina"] = nId_PagoNomina;

                    bRetorno = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
    /// Registra la nomina
    /// </summary>
    /// <param name="pnId_Nomina">ID de la Nomina</param>
    /// <param name="pnId_Empleado">ID del Empleado</param>
    /// <param name="pdFechaPago">Fecha de Pago</param>
    /// <param name="pdFechaInicialPago">Fecha Inicial de Pago</param>
    /// <param name="pdFechaFinalPago">Fecha Final de Pago</param>
    /// <param name="pnNumDiasPagados">Número de días de pagados</param>
    /// <param name="pnAntiguedad">Antigüedad</param>
    /// <param name="dtDetalle">DataTable de Detalle</param>
    /// <param name="dtHorasExtra">DataTable de Horas Extra</param>
    /// <param name="dtIncapacidades">DataTable de Incapacidades</param>
    /// <returns></returns>
    public bool fnRegistrarNominaActual(int pnId_Nomina, int pnId_Empleado, DateTime pdFechaPago,
                                DateTime pdFechaInicialPago, DateTime pdFechaFinalPago, decimal pnNumDiasPagados, int pnAntiguedad, decimal nTotal,
                                int pnId_Estructura_Expedido, int pnId_Estado_Expedido, string psMetodo_Pago, string psNumero_Cuenta,
                                string psMoneda, string psRegimenFiscal, string psForma_Pago, string psTipo_Cambio,
                                DataTable dtDetalle, DataTable dtHorasExtra, DataTable dtIncapacidades)
    {
        bool bRetorno = false;
        int nId_Detalle = 0;
        int nId_PagoNomina = 0;
        int nRegistrosAfectados = 0;

        HttpContext.Current.Session["IdPagoNomina"] = 0;

        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {
            con.Open();
            using (SqlTransaction tran = con.BeginTransaction())
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("usp_nom_Pagos_Ins", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("nId_Nomina", pnId_Nomina);
                        cmd.Parameters.AddWithValue("nId_Empleado", pnId_Empleado);
                        cmd.Parameters.AddWithValue("dFechaPago", pdFechaPago);
                        cmd.Parameters.AddWithValue("dFechaInicialPago", pdFechaInicialPago);
                        cmd.Parameters.AddWithValue("dFechaFinalPago", pdFechaFinalPago);
                        cmd.Parameters.AddWithValue("nNumDiasPagados", pnNumDiasPagados);
                        cmd.Parameters.AddWithValue("nAntiguedad", pnAntiguedad);
                        cmd.Parameters.AddWithValue("nTotal", nTotal);

                        nId_PagoNomina = Convert.ToInt32(cmd.ExecuteScalar());

                        if (nId_PagoNomina.Equals(0))
                            throw new Exception("No se registro el ID de la Nómina por Empleado.");
                    }

                    using (SqlCommand cmd = new SqlCommand("usp_nom_Comprobante_Pagos_Nomina_Ins", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("nId_Pago_Nomina", nId_PagoNomina);
                        if (!pnId_Estructura_Expedido.Equals(0))
                            cmd.Parameters.AddWithValue("nId_Estructura_Expedido", pnId_Estructura_Expedido);
                        cmd.Parameters.AddWithValue("nId_Estado_Expedido", pnId_Estado_Expedido);
                        cmd.Parameters.AddWithValue("sMetodo_Pago", psMetodo_Pago);
                        cmd.Parameters.AddWithValue("sNumero_Cuenta", psNumero_Cuenta);
                        cmd.Parameters.AddWithValue("sMoneda", psMoneda);
                        cmd.Parameters.AddWithValue("sRegimen_Fiscal", psRegimenFiscal);
                        cmd.Parameters.AddWithValue("sForma_Pago", psForma_Pago);
                        cmd.Parameters.AddWithValue("sTipo_Cambio", psTipo_Cambio);

                        nRegistrosAfectados = cmd.ExecuteNonQuery();

                        if (nRegistrosAfectados <= 0)
                            throw new Exception("No se registro los datos del comprobante ligado a la Nómina.");
                    }

                    foreach (DataRow renglonDetalle in dtDetalle.Rows)
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_nom_PercepDedu_Ins", con))
                        {
                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("nId_PagoNomina", nId_PagoNomina);
                            cmd.Parameters.AddWithValue("nId_TipoPercepDedu", Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]));
                            cmd.Parameters.AddWithValue("sClave", renglonDetalle["Clave"]);
                            cmd.Parameters.AddWithValue("sConcepto", renglonDetalle["Concepto"]);
                            cmd.Parameters.AddWithValue("nImporteGravado", Convert.ToInt32(renglonDetalle["ImporteGravado"]));
                            cmd.Parameters.AddWithValue("nImporteExento", Convert.ToInt32(renglonDetalle["ImporteExento"]));

                            nId_Detalle = Convert.ToInt32(cmd.ExecuteScalar());

                            if (nId_Detalle.Equals(0))
                                throw new Exception("No se registro el ID del detalle de la Nómina.");
                        }


                        // Se insertan las horas extras relacionadas a la percepción
                        if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                            Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
                        {
                            DataView dvHorasExtra = new DataView(dtHorasExtra);
                            dvHorasExtra.RowFilter = "Id_PercepDedu= " + renglonDetalle["Id_PercepDedu"];

                            foreach (DataRow renglonHoraExtra in dvHorasExtra.ToTable().Rows)
                            {
                                using (SqlCommand cmd = new SqlCommand("usp_nom_HorasExtras_Ins", con))
                                {

                                    cmd.Transaction = tran;
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("nId_PercepDedu", nId_Detalle);
                                    cmd.Parameters.AddWithValue("nDias", renglonHoraExtra["Dias"]);
                                    cmd.Parameters.AddWithValue("sTipoHoras", renglonHoraExtra["TipoHoras"]);
                                    cmd.Parameters.AddWithValue("nHorasExtra", Convert.ToInt32(renglonHoraExtra["HorasExtra"]));
                                    cmd.Parameters.AddWithValue("nImportePagado", Convert.ToDouble(renglonHoraExtra["ImportePagado"]));

                                    nRegistrosAfectados = cmd.ExecuteNonQuery();

                                    if (nRegistrosAfectados.Equals(0))
                                        throw new Exception("No se registro las horas extra.");
                                }

                            }
                        }

                        // Se insertan las incapacidades relacionadas a la deducción
                        if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                            Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
                        {
                            DataView dvIncapacidades = new DataView(dtIncapacidades);
                            dvIncapacidades.RowFilter = "Id_PercepcionDeduccion= " + renglonDetalle["Id_PercepDedu"];

                            foreach (DataRow renglonIncapacidad in dvIncapacidades.ToTable().Rows)
                            {
                                using (SqlCommand cmd = new SqlCommand("usp_nom_Incapacidades_Ins", con))
                                {

                                    cmd.Transaction = tran;
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("nId_PercepcionDeduccion", nId_Detalle);
                                    cmd.Parameters.AddWithValue("nTipo", renglonIncapacidad["Tipo"]);
                                    cmd.Parameters.AddWithValue("nDiasIncapacidad", renglonIncapacidad["DiasIncapacidad"]);
                                    cmd.Parameters.AddWithValue("nDescuento", Convert.ToInt32(renglonIncapacidad["Descuento"]));

                                    nRegistrosAfectados = cmd.ExecuteNonQuery();

                                    if (nRegistrosAfectados.Equals(0))
                                        throw new Exception("No se registro de las incapacidades.");
                                }
                            }
                        }
                    }
                    tran.Commit();

                    bRetorno = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
    /// Registra la nomina
    /// </summary>
    /// <param name="pnId_Pago_Nomina">ID del Pago de Nómina</param>
    /// <param name="pnId_Empleado">ID del Empleado</param>
    /// <param name="pdFechaPago">Fecha de Pago</param>
    /// <param name="pdFechaInicialPago">Fecha Inicial de Pago</param>
    /// <param name="pdFechaFinalPago">Fecha Final de Pago</param>
    /// <param name="pnNumDiasPagados">Número de días de pagados</param>
    /// <param name="pnAntiguedad">Antigüedad</param>
    /// <param name="dtDetalle">DataTable de Detalle</param>
    /// <param name="dtHorasExtra">DataTable de Horas Extra</param>
    /// <param name="dtIncapacidades">DataTable de Incapacidades</param>
    /// <returns></returns>
    public bool fnRegistrarNominaGeneracion(int pnId_Estructura, int pnId_Nomina, int pnId_Pago_Nomina, int pnId_Empleado, DateTime pdFechaPago,
                                DateTime pdFechaInicialPago, DateTime pdFechaFinalPago, decimal pnNumDiasPagados, int pnAntiguedad, decimal nTotal,
                                int pnId_Estructura_Expedido, int pnId_Estado_Expedido, string psMetodo_Pago, string psNumero_Cuenta,
                                string psMoneda, string psRegimenFiscal, string psForma_Pago, string psTipo_Cambio,
                                DataTable dtDetalle, DataTable dtHorasExtra, DataTable dtIncapacidades)
    {
        bool bRetorno = false;
        int nId_Detalle = 0;
        int nRegistrosAfectados = 0;

        HttpContext.Current.Session["IdPagoNomina"] = 0;

        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString;

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {
            con.Open();
            using (SqlTransaction tran = con.BeginTransaction())
            {
                try
                {
                    // Se actualizan los registro de la tabla de Pagos de ese empleado
                    using (SqlCommand cmd = new SqlCommand("usp_nom_Pagos_Upd", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("nIdPagoNomina", pnId_Pago_Nomina);
                        cmd.Parameters.AddWithValue("nId_Empleado", pnId_Empleado);
                        cmd.Parameters.AddWithValue("dFechaPago", pdFechaPago);
                        cmd.Parameters.AddWithValue("dFechaInicialPago", pdFechaInicialPago);
                        cmd.Parameters.AddWithValue("dFechaFinalPago", pdFechaFinalPago);
                        cmd.Parameters.AddWithValue("nNumDiasPagados", pnNumDiasPagados);
                        cmd.Parameters.AddWithValue("nAntiguedad", pnAntiguedad);
                        cmd.Parameters.AddWithValue("nTotal", nTotal);

                        nRegistrosAfectados = cmd.ExecuteNonQuery();

                        if (nRegistrosAfectados.Equals(0))
                            throw new Exception("No se actualizo el pago de la Nómina por empleado.");
                    }

                    // Se actualiza los datos del Comprobante ligado a la Nómina
                    using (SqlCommand cmd = new SqlCommand("usp_nom_Comprobante_Pagos_Nomina_Upd", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("nId_Pago_Nomina", pnId_Pago_Nomina);
                        if (!pnId_Estructura_Expedido.Equals(0))
                            cmd.Parameters.AddWithValue("nId_Estructura_Expedido", pnId_Estructura_Expedido);
                        cmd.Parameters.AddWithValue("nId_Estado_Expedido", pnId_Estado_Expedido);
                        cmd.Parameters.AddWithValue("sMetodo_Pago", psMetodo_Pago);
                        cmd.Parameters.AddWithValue("sNumero_Cuenta", psNumero_Cuenta);
                        cmd.Parameters.AddWithValue("sMoneda", psMoneda);
                        cmd.Parameters.AddWithValue("sRegimen_Fiscal", psRegimenFiscal);
                        cmd.Parameters.AddWithValue("sForma_Pago", psForma_Pago);
                        cmd.Parameters.AddWithValue("sTipo_Cambio", psTipo_Cambio);

                        nRegistrosAfectados = cmd.ExecuteNonQuery();

                        if (nRegistrosAfectados <= 0)
                            throw new Exception("No se registro los datos del comprobante ligado a la Nómina.");
                    }

                    // Se borran los datos de las horas extra relacionadas al pago de esa Nómina
                    using (SqlCommand cmd = new SqlCommand("usp_nom_HorasExtra_del_PorIdEstructuraIdNominaIdPagoNomina", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("nId_Estructura", pnId_Estructura);
                        cmd.Parameters.AddWithValue("nId_Nomina", pnId_Nomina);
                        cmd.Parameters.AddWithValue("nId_Pago_Nomina", pnId_Pago_Nomina);

                        cmd.ExecuteNonQuery();
                    }

                    // Se borran los datos de las incapacidades relacionadas al pago de esa Nómina
                    using (SqlCommand cmd = new SqlCommand("usp_nom_Incapacidades_del_PorIdEstructuraIdNominaIdPagoNomina", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("nId_Estructura", pnId_Estructura);
                        cmd.Parameters.AddWithValue("nId_Nomina", pnId_Nomina);
                        cmd.Parameters.AddWithValue("nId_Pago_Nomina", pnId_Pago_Nomina);

                        cmd.ExecuteNonQuery();
                    }

                    // Se borran los datos de las percepciones y deducciones del pago de esa Nómina
                    using (SqlCommand cmd = new SqlCommand("usp_nom_PercepcionDeduccion_del_PorIdEstructuraIdNominaIdPagoNomina", con))
                    {
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("nId_Estructura", pnId_Estructura);
                        cmd.Parameters.AddWithValue("nId_Nomina", pnId_Nomina);
                        cmd.Parameters.AddWithValue("nId_Pago_Nomina", pnId_Pago_Nomina);

                        cmd.ExecuteNonQuery();
                    }
                   
                    foreach (DataRow renglonDetalle in dtDetalle.Rows)
                    {
                        // Se insertan las percepciones y deducciones
                        using (SqlCommand cmd = new SqlCommand("usp_nom_PercepDedu_Ins", con))
                        {
                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("nId_PagoNomina", pnId_Pago_Nomina);
                            cmd.Parameters.AddWithValue("nId_TipoPercepDedu", Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]));
                            cmd.Parameters.AddWithValue("sClave", renglonDetalle["Clave"]);
                            cmd.Parameters.AddWithValue("sConcepto", renglonDetalle["Concepto"]);
                            cmd.Parameters.AddWithValue("nImporteGravado", Convert.ToInt32(renglonDetalle["ImporteGravado"]));
                            cmd.Parameters.AddWithValue("nImporteExento", Convert.ToInt32(renglonDetalle["ImporteExento"]));

                            nId_Detalle = Convert.ToInt32(cmd.ExecuteScalar());

                            if (nId_Detalle.Equals(0))
                                throw new Exception("No se registro el ID del detalle de la Nómina.");
                        }                     

                        // Se insertan las horas extras relacionadas a la percepción
                        if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Percepcion) &&
                            Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.HorasExtra))
                        {
                            DataView dvHorasExtra = new DataView(dtHorasExtra);
                            dvHorasExtra.RowFilter = "Id_PercepDedu= " + renglonDetalle["Id_PercepDedu"];

                            foreach (DataRow renglonHoraExtra in dvHorasExtra.ToTable().Rows)
                            {
                                using (SqlCommand cmd = new SqlCommand("usp_nom_HorasExtras_Ins", con))
                                {

                                    cmd.Transaction = tran;
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("nId_PercepDedu", nId_Detalle);
                                    cmd.Parameters.AddWithValue("nDias", renglonHoraExtra["Dias"]);
                                    cmd.Parameters.AddWithValue("sTipoHoras", renglonHoraExtra["TipoHoras"]);
                                    cmd.Parameters.AddWithValue("nHorasExtra", Convert.ToInt32(renglonHoraExtra["HorasExtra"]));
                                    cmd.Parameters.AddWithValue("nImportePagado", Convert.ToDouble(renglonHoraExtra["ImportePagado"]));

                                    nRegistrosAfectados = cmd.ExecuteNonQuery();

                                    if (nRegistrosAfectados.Equals(0))
                                        throw new Exception("No se registro las horas extra.");
                                }

                            }
                        }

                        // Se insertan las incapacidades relacionadas a la deducción
                        if (Convert.ToInt32(renglonDetalle["Id_Tipo"]).Equals((int)clsEnumeraciones.TiposDetalleNomina.Deduccion) &&
                            Convert.ToInt32(renglonDetalle["Id_TipoPercepDedu"]).Equals((int)clsEnumeraciones.TiposDeduccionesPercepciones.DescuentoIncapacidad))
                        {
                            DataView dvIncapacidades = new DataView(dtIncapacidades);
                            dvIncapacidades.RowFilter = "Id_PercepcionDeduccion= " + renglonDetalle["Id_PercepDedu"];

                            foreach (DataRow renglonIncapacidad in dvIncapacidades.ToTable().Rows)
                            {
                                using (SqlCommand cmd = new SqlCommand("usp_nom_Incapacidades_Ins", con))
                                {

                                    cmd.Transaction = tran;
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("nId_PercepcionDeduccion", nId_Detalle);
                                    cmd.Parameters.AddWithValue("nTipo", renglonIncapacidad["Tipo"]);
                                    cmd.Parameters.AddWithValue("nDiasIncapacidad", renglonIncapacidad["DiasIncapacidad"]);
                                    cmd.Parameters.AddWithValue("nDescuento", Convert.ToInt32(renglonIncapacidad["Descuento"]));

                                    nRegistrosAfectados = cmd.ExecuteNonQuery();

                                    if (nRegistrosAfectados.Equals(0))
                                        throw new Exception("No se registro de las incapacidades.");
                                }
                            }
                        }
                    }
                    tran.Commit();

                    bRetorno = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
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
    /// Función que obtiene los datos de los pagos realizados a una Nómina es especifico
    /// </summary>
    /// <param name="pnId_PagoNomina">ID del Pago de la Nómina</param>
    /// <returns></returns>
    public DataTable fnObtenerComprobantePagoNomina(int pnId_PagoNomina)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection("Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n"))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Comprobante_Pagos_Nomina_sel_PorIdPagoNomina";
                    cmd.Parameters.Add(new SqlParameter("nId_Pago_Nomina", pnId_PagoNomina));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que obtiene los datos de la Nómina en especifico de un pago en especifico
    /// </summary>
    /// <param name="pnId_PagoNomina">ID del Pago de la Nómina</param>
    /// <returns></returns>
    public DataTable fnObtenerNomina(int pnId_PagoNomina)
    {
        DataTable gdtAuxiliar = new DataTable();

        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Nomina_sel_PorIdPagoNomina";
                    cmd.Parameters.Add(new SqlParameter("nId_PagoNomina", pnId_PagoNomina));

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
    /// Recupera la lista de los Periodos.
    /// </summary>
    /// <param name="nId_usuario">id del usuario</param>
    /// <returns>recupera la lista de los Periodos.</returns>
    public DataTable fnBusquedaLastNomina(int nId_Estructura, string sEstatus, int nPeriodo)
    {
        DataTable gdtAuxiliar = new DataTable("PagosNomina");

        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conTimbrado"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Timbrado_LastNomina_Sel";
                    cmd.Parameters.Add(new SqlParameter("@IdEstructura", nId_Estructura));
                    cmd.Parameters.Add(new SqlParameter("@Estatus", sEstatus));
                    cmd.Parameters.Add(new SqlParameter("@IdTipoPeriodo", nPeriodo));

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
    /// Función que obtiene los datos de los pagos realizados a una Nómina es especifico
    /// </summary>
    /// <param name="pnId_PagoNomina">ID del Pago de la Nómina</param>
    /// <returns></returns>
    public DataTable fnObtenerPagoNomina(int pnId_PagoNomina)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection("Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n"))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Pago_sel_Existe";
                    cmd.Parameters.Add(new SqlParameter("nId_PagoNomina", pnId_PagoNomina));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que obtiene el detalle de las percepciones y deducciones dentro de una Nómina
    /// </summary>
    /// <param name="pnId_PagoNomina">ID del Pago de la Nómina</param>
    /// <returns></returns>
    public DataTable fnObtenerPercepcionesDeducciones(int pnId_PagoNomina)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection("Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n"))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_PercepcionDeduccion_sel_PorIdPagoNomina";
                    cmd.Parameters.Add(new SqlParameter("nId_PagoNomina", pnId_PagoNomina));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que obtiene las horas extras en un pago en especifico Nómina 
    /// </summary>
    /// <param name="pnId_PagoNomina">ID del Pago de la Nómina</param>
    /// <returns></returns>
    public DataTable fnObtenerHorasExtra(int pnId_PagoNomina)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection("Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n"))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_HorasExtra_sel_PorIdPagoNomina";
                    cmd.Parameters.Add(new SqlParameter("nId_PagoNomina", pnId_PagoNomina));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que obtiene las incapacidades en un pago en especifico Nómina 
    /// </summary>
    /// <param name="pnId_PagoNomina">ID del Pago de la Nómina</param>
    /// <returns></returns>
    public DataTable fnObtenerIncapacidades(int pnId_PagoNomina)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection("Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n"))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Incapacidades_sel_PorIdPagoNomina";
                    cmd.Parameters.Add(new SqlParameter("nId_PagoNomina", pnId_PagoNomina));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return dtResultado;
    }

    /// <summary>
    /// Función que se encarga de obtener la ultima Nómina abierta de una estructura en especifico
    /// </summary>
    /// <param name="pnId_Estructura">ID de la Estructura</param>
    /// <returns></returns>
    public int fnObtenerNominaCerrada(int pnId_Estructura)
    {
        int nResultado = 0;
        try
        {
            using (SqlConnection con = new SqlConnection("Data Source=192.168.3.106;Initial Catalog=CFDI;Persist Security Info=True;User ID=sa;Password=F4cturax10n"))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_NominaAbierta_sel_PorIdEstructura";
                    cmd.Parameters.Add(new SqlParameter("nId_Estructura", pnId_Estructura));

                    nResultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return nResultado;
    }
}

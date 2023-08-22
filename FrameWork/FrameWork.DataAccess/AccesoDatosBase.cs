using FrameWork.ExceptionHandling.CustomException;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameWork.DataAccess
{
    public class AccesoDatosBase
    {
        public static string _cadenaConexion = string.Empty;
        public int _filasAfectadas = 0;
        private static System.Transactions.TransactionScope TransaccionAmbiente;
        private static System.Data.SqlClient.SqlTransaction Transaccion;
        private static System.Transactions.CommittableTransaction TransaccionComitable;
        private static SqlConnection conEstatica;

        public AccesoDatosBase()
        {

        }

        public string sCadenaConexion
        {
            set
            {
                _cadenaConexion = value;
            }
        }

        public int nFilasAfectadas
        {
            get
            {
                return _filasAfectadas;
            }
        }

        #region Métodos públicos

        public void fnActualizar<T>(ref T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            //try
            //{
                SqlCommand Comando = fnCargarComando(Entidad, psProcedimientoAlmacenado);
                fnEjecutar(ref Comando, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
                fnCargarEntidad(ref Entidad, Comando);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHandling.DataAccessExceptionHandler.HadleException(ex);
            //}
        }

        public void fnInsertar<T>(ref T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            //try
            //{
                SqlCommand Comando = fnCargarComando(Entidad, psProcedimientoAlmacenado);
                fnEjecutar(ref Comando, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
                fnCargarEntidad(ref Entidad, Comando);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHandling.DataAccessExceptionHandler.HadleException(ex);
            //}
        }

        public void fnActualizar<T>(ref T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Data.IsolationLevel psIsolacion = System.Data.IsolationLevel.ReadCommitted)
        {
            //try
            //{
                SqlCommand Comando = fnCargarComando(Entidad, psProcedimientoAlmacenado);
                fnEjecutarComitable(ref Comando, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
                fnCargarEntidad(ref Entidad, Comando);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHandling.DataAccessExceptionHandler.HadleException(ex);
            //}
        }

        public void fnInsertar<T>(ref T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Data.IsolationLevel psIsolacion = System.Data.IsolationLevel.ReadCommitted)
        {
            //try
            //{
                SqlCommand Comando = fnCargarComando(Entidad, psProcedimientoAlmacenado);
                fnEjecutar(ref Comando, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
                fnCargarEntidad(ref Entidad, Comando);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHandling.DataAccessExceptionHandler.HadleException(ex);
            //}
        }

        public DataTable fnConsultar<T>(T Entidad, string psProcedimientoAlmacenado)
        {
            //DataTable dtResultado = new DataTable();
            //try
            //{
            SqlCommand Comando = new SqlCommand();
            fnCargarComando(Entidad, psProcedimientoAlmacenado);
            return fnEjecutarConsulta(Comando);
                //dtResultado = fnEjecutarConsulta(Comando);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHandling.DataAccessExceptionHandler.HadleException(ex);
            //}
            //return dtResultado;
        }

        public DataTable fnConsultar<T>(T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            //DataTable dtResultado = new DataTable();
            //try
            //{
            SqlCommand Comando = new SqlCommand();
            fnCargarComando(Entidad, psProcedimientoAlmacenado);
            return fnEjecutarConsulta(Comando);
                //dtResultado = fnEjecutarConsulta(Comando);
            //}
            //catch (Exception ex)
            //{
            //    ExceptionHandling.DataAccessExceptionHandler.HadleException(ex);
            //}
            //return dtResultado;
        }

        #endregion

        #region Métodos privados

        private SqlCommand fnCargarComando<T>(T Entidad, string psProcedimientoAlmacenado)
        {
            SqlCommand Comando = new SqlCommand();
            object oValor = null;
            using (SqlConnection con = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    Comando.CommandText = psProcedimientoAlmacenado;
                    Comando.Connection = con;
                    Comando.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlCommandBuilder.DeriveParameters(Comando);

                    foreach (SqlParameter parametro in Comando.Parameters)
                    {
                        if (!parametro.Direction.Equals(System.Data.ParameterDirection.ReturnValue))
                        {
                            string sNombreParametro = parametro.ParameterName.Split('@')[1];
                            try
                            {
                                oValor = Entidad.GetType().GetProperty(sNombreParametro).GetValue(Entidad, null);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Error al obtener el valor {0} de la Entidad {1}. " + ex.Message, sNombreParametro, Entidad.GetType()));
                            }
                            Comando.Parameters[parametro.ParameterName].Value = oValor;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (Transaccion != null)
                    {
                        try { Transaccion.Rollback(); }
                        catch { }

                        Transaccion.Dispose();
                    }

                    if (TransaccionAmbiente != null) TransaccionAmbiente.Dispose();

                    if (TransaccionComitable != null) TransaccionComitable.Rollback();

                    if (conEstatica != null)
                        conEstatica.Close();

                    if (con != null)
                        con.Close();

                    throw new ExceptionHandling.CustomException.DataAccesLayerException("Se ha generado un error a la hora de cargar los parametros: " + ex.Message);
                }
                finally
                {
                    if (con != null)
                        con.Close();
                }
            }
            return Comando;
        }

        private SqlParameter[] fnCargarParametro<T>(T Entidad, string psProcedimientoAlmacenado)
        {
            SqlCommand Comando = new SqlCommand();
            List<SqlParameter> ListaParameter = new List<SqlParameter>();
            object oValor = null;

            using (SqlConnection con = new SqlConnection(_cadenaConexion))
            {
                try
                {

                    Comando.CommandText = psProcedimientoAlmacenado;
                    Comando.Connection = con;
                    Comando.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlCommandBuilder.DeriveParameters(Comando);

                    foreach (SqlParameter parametro in Comando.Parameters)
                    {
                        if (!parametro.Direction.Equals(System.Data.ParameterDirection.ReturnValue))
                        {
                            string sNombreParametro = parametro.ParameterName.Split('@')[1];
                            try
                            {
                                oValor = Entidad.GetType().GetProperty(sNombreParametro).GetValue(Entidad, null);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(string.Format("Error al obtener el valor {0} de la Entidad {1}. " + ex.Message, sNombreParametro, Entidad.GetType()));
                            }
                            ListaParameter.Add(new SqlParameter(parametro.ParameterName, oValor));
                        }
                    }

                }
                catch (Exception ex)
                {
                    if (Transaccion != null)
                    {
                        try { Transaccion.Rollback(); }
                        catch { }

                        Transaccion.Dispose();
                    }

                    if (TransaccionAmbiente != null) TransaccionAmbiente.Dispose();

                    if (TransaccionComitable != null) TransaccionComitable.Rollback();

                    if (conEstatica != null)
                        conEstatica.Close();

                    if (con != null)
                        con.Close();

                    throw new ExceptionHandling.CustomException.DataAccesLayerException("Se ha generado un error a la hora de cargar los parametros: " + ex.Message);
                }
            }
            return ListaParameter.ToArray();
        }

        private void fnCargarEntidad<T>(ref T Entidad, SqlCommand Comando)
        {
            try
            {
                foreach (SqlParameter parametro in Comando.Parameters)
                {
                    if (parametro.Direction.Equals(System.Data.ParameterDirection.InputOutput))
                    {
                        string sNombreParametro = parametro.ParameterName.Split('@')[1];
                        try
                        {
                            switch (parametro.SqlDbType)
                            {
                                case SqlDbType.Int:
                                    Entidad.GetType().GetProperty(sNombreParametro).SetValue(Entidad, Convert.ToInt32(parametro.Value), null);
                                    break;
                                case SqlDbType.NVarChar:
                                    Entidad.GetType().GetProperty(sNombreParametro).SetValue(Entidad, Convert.ToString(parametro.Value), null);
                                    break;
                                case SqlDbType.Xml:
                                    Entidad.GetType().GetProperty(sNombreParametro).SetValue(Entidad, Convert.ToString(parametro.Value), null);
                                    break;
                                case SqlDbType.DateTime:
                                    Entidad.GetType().GetProperty(sNombreParametro).SetValue(Entidad, Convert.ToDateTime(parametro.Value), null);
                                    break;
                                case SqlDbType.BigInt:
                                    Entidad.GetType().GetProperty(sNombreParametro).SetValue(Entidad, Convert.ToInt64(parametro.Value), null);
                                    break;
                                case SqlDbType.Binary:
                                    Entidad.GetType().GetProperty(sNombreParametro).SetValue(Entidad, Convert.ToByte(parametro.Value), null);
                                    break;
                                case SqlDbType.Char:
                                    Entidad.GetType().GetProperty(sNombreParametro).SetValue(Entidad, Convert.ToChar(parametro.Value), null);
                                    break;
                                case SqlDbType.Decimal:
                                    Entidad.GetType().GetProperty(sNombreParametro).SetValue(Entidad, Convert.ToDecimal(parametro.Value), null);
                                    break;
                                case SqlDbType.VarChar:
                                    Entidad.GetType().GetProperty(sNombreParametro).SetValue(Entidad, Convert.ToString(parametro.Value), null);
                                    break;
                                default:
                                    throw new Exception(string.Format("No se encuentra un valor para {0} a convertir de tipo {1}", sNombreParametro, parametro.SqlDbType.ToString()));
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Error al obtener el valor {0} de la Entidad {1}. " + ex.Message, sNombreParametro, Entidad.GetType()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Transaccion != null)
                {
                    try { Transaccion.Rollback(); }
                    catch { }

                    Transaccion.Dispose();
                }

                if (TransaccionAmbiente != null) TransaccionAmbiente.Dispose();

                if (TransaccionComitable != null) TransaccionComitable.Rollback();

                if (conEstatica != null)
                    conEstatica.Close();

                throw new ExceptionHandling.CustomException.DataAccesLayerException("Se ha generado un error a la hora de cargar la Entidad: " + ex.Message);
            }
        }

        private void fnEjecutar(ref SqlCommand Comando, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Data.IsolationLevel psIsolacion = System.Data.IsolationLevel.ReadCommitted)
        {
            try
            {
                if (conEstatica == null)
                    conEstatica = new SqlConnection(_cadenaConexion);

                if (conEstatica.State != ConnectionState.Open)
                    conEstatica.Open();

                if (pbEnTransaccion)
                {
                    fnIniciarTransaccion(conEstatica, pbEnTransaccion, psIsolacion);
                    Comando.Transaction = Transaccion;
                }

                Comando.Connection = conEstatica;

                _filasAfectadas = Comando.ExecuteNonQuery();

                if (pbTerminarTransaccion)
                    fnCompletarTransaccion();
            }
            catch (Exception ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                    Transaccion.Dispose();
                }

                if (conEstatica != null)
                    conEstatica.Close();

                throw new ExceptionHandling.CustomException.DataAccesLayerException("Error al ejecutar el procedimiento:  " + ex.Message);
            }
            
        }

        private void fnEjecutar(ref SqlCommand Comando, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            using (SqlConnection con = new SqlConnection(_cadenaConexion))
            {
                try
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    if (pbEnTransaccion)
                    {
                        fnIniciarTransaccion(con, pbRequiereTransaccion, pbEnTransaccion, psIsolacion);
                    }

                    Comando.Connection = con;

                    _filasAfectadas = Comando.ExecuteNonQuery();

                    if (pbTerminarTransaccion)
                        fnCompletarTransaccionAmbiente();
                }
                catch (Exception ex)
                {
                    if (TransaccionAmbiente != null)
                        TransaccionAmbiente.Dispose();

                    if (con != null)
                        con.Close();

                    throw new ExceptionHandling.CustomException.DataAccesLayerException("Error al ejecutar el procedimiento:  " + ex.Message);
                }
            }
        }

        private void fnEjecutarComitable(ref SqlCommand Comando, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Data.IsolationLevel psIsolacion = System.Data.IsolationLevel.ReadCommitted)
        {
            try
            {
                if(conEstatica == null)
                    conEstatica = new SqlConnection(_cadenaConexion);

                if (conEstatica.State != ConnectionState.Open)
                    conEstatica.Open();

                if (pbEnTransaccion)
                    fnIniciarTransaccionComitable(conEstatica, pbRequiereTransaccion, pbEnTransaccion, psIsolacion);

                Comando.Connection = conEstatica;

                _filasAfectadas = Comando.ExecuteNonQuery();

                if (pbTerminarTransaccion)
                    fnCompletarTransaccionComitable();
            }
            catch (System.Transactions.TransactionException ex)
            {
                if (!TransaccionComitable.TransactionInformation.Status.Equals(TransactionStatus.Aborted))
                    TransaccionComitable.Rollback();

                if (conEstatica != null)
                {
                    conEstatica.Close();
                    conEstatica = null;
                }

                throw new ExceptionHandling.CustomException.DataAccesLayerException("No fue posible completar la transacción: " + ex.Message);
            }
            catch (Exception ex)
            {
                if (conEstatica != null)
                {
                    conEstatica.Close();
                    conEstatica = null;
                }

                if (TransaccionComitable != null) TransaccionComitable.Rollback();

                throw new ExceptionHandling.CustomException.DataAccesLayerException("Error al ejecutar el procedimiento:  " + ex.Message);
            }
        }

        private DataTable fnEjecutarConsulta(SqlCommand Comando)
        {
            DataTable gdtAuxiliar = new DataTable("dtResultado");
            try
            {
                using (SqlConnection con = new SqlConnection(_cadenaConexion))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(Comando))
                    {
                        da.Fill(gdtAuxiliar);
                    }
                }

                _filasAfectadas = gdtAuxiliar.Rows.Count;
            }
            catch (Exception ex)
            {
                throw new ExceptionHandling.CustomException.DataAccesLayerException("Error al generar la consulta: " + ex.Message);
            }
            return gdtAuxiliar;
        }

        private void fnIniciarTransaccion(SqlConnection con, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            try
            {
                if (pbRequiereTransaccion)
                {
                    TransactionOptions tsoOpcion = new TransactionOptions();
                    tsoOpcion.IsolationLevel = psIsolacion;

                    TransaccionAmbiente = new TransactionScope(TransactionScopeOption.Required, tsoOpcion);
                }

                con.EnlistTransaction(System.Transactions.Transaction.Current);
            }
            catch (Exception ex)
            {
                if (TransaccionAmbiente != null)
                    TransaccionAmbiente.Dispose();

                throw new Exception("Error al meter la conexión a la transacción: " + ex.Message);
            }
        }

        private void fnIniciarTransaccion(SqlConnection con, Boolean pbEnTransaccion, System.Data.IsolationLevel psIsolacion = System.Data.IsolationLevel.ReadCommitted)
        {
            try
            {
                if (pbEnTransaccion && Transaccion == null)
                {
                    Transaccion = con.BeginTransaction(psIsolacion);
                }

                try
                {
                    //con.EnlistTransaction(System.Transactions.Transaction.Current);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al meter la conexión a la transacción: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                if (Transaccion != null)
                {
                    Transaccion.Rollback();
                    Transaccion.Dispose();
                }

                throw new Exception("Error al iniciar la transacción: " + ex.Message);
            }
        }

        private void fnIniciarTransaccionComitable(SqlConnection con, bool pbRequiereTransaccion = true, Boolean pbEnTransaccion = false, System.Data.IsolationLevel psIsolacion = System.Data.IsolationLevel.ReadCommitted)
        {
            try
            {
                if (pbEnTransaccion && pbRequiereTransaccion)
                {
                    TransaccionComitable = new CommittableTransaction();
                }

                try
                {
                    con.EnlistTransaction(TransaccionComitable);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al meter la conexión a la transacción: " + ex.Message);
                }
            }
            catch (System.Transactions.TransactionException ex)
            {
                if (!TransaccionComitable.TransactionInformation.Status.Equals(TransactionStatus.Aborted))
                    TransaccionComitable.Rollback();

                throw new Exception("No fue posible completar la transacción: " + ex.Message);
            }
            catch (Exception ex)
            {
                if (TransaccionComitable != null) TransaccionComitable.Rollback();

                throw new Exception("Error al iniciar la transacción: " + ex.Message);
            }
        }

        private void fnCompletarTransaccion()
        {
            try
            {
                Transaccion.Commit();

                if (Transaccion != null) Transaccion.Dispose();
            }
            catch (Exception ex)
            {
                if (Transaccion != null)
                {
                    try { Transaccion.Rollback(); }
                    catch { }

                    Transaccion.Dispose();
                }

                throw new Exception("No fue posible completar la transacción: " + ex.Message);
            }
        }

        private void fnCompletarTransaccionAmbiente()
        {
            try
            {
                TransaccionAmbiente.Complete();

                if (TransaccionAmbiente != null) TransaccionAmbiente.Dispose();
            }
            catch (Exception ex)
            {
                if (TransaccionAmbiente != null) TransaccionAmbiente.Dispose();

                throw new Exception("No fue posible completar la transacción: " + ex.Message);
            }
        }

        private void fnCompletarTransaccionComitable()
        {
            try
            {
                TransaccionComitable.Commit();

                if (TransaccionComitable != null) TransaccionComitable.Dispose();
            }
            catch (System.Transactions.TransactionException ex)
            {
                if (!TransaccionComitable.TransactionInformation.Status.Equals(TransactionStatus.Aborted))
                    TransaccionComitable.Rollback();

                throw new Exception("No fue posible completar la transacción: " + ex.Message);
            }
            catch (Exception ex)
            {
                if (TransaccionComitable != null) TransaccionComitable.Rollback();

                throw new Exception("No fue posible completar la transacción: " + ex.Message);
            }
        }

        #endregion
    }
}

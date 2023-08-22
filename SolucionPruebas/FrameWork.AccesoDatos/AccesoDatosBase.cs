using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FrameWork.AccesoDatos
{
    public class AccesoDatosBase
    {
        public static string _cadenaConexion = string.Empty;
        public int _filasAfectadas = 0;
        private System.Transactions.TransactionScope TransaccionAmbiente;
        private System.Data.SqlClient.SqlTransaction Transaccion;

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
            SqlCommand Comando = fnCargarComando(Entidad, psProcedimientoAlmacenado);
            fnEjecutar(ref Comando, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
            fnCargarEntidad(ref Entidad, Comando);
        }

        public void fnInsertar<T>(ref T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            SqlCommand Comando = fnCargarComando(Entidad, psProcedimientoAlmacenado);
            fnEjecutar(ref Comando, pbRequiereTransaccion, pbEnTransaccion, pbTerminarTransaccion, psIsolacion);
            fnCargarEntidad(ref Entidad, Comando);
        }

        public DataTable fnConsultar<T>(T Entidad, string psProcedimientoAlmacenado)
        {
            SqlCommand Comando = new SqlCommand();
            fnCargarComando(Entidad, psProcedimientoAlmacenado);
            return fnEjecutarConsulta(Comando);
        }

        public DataTable fnConsultar<T>(T Entidad, string psProcedimientoAlmacenado, bool pbRequiereTransaccion = true, bool pbEnTransaccion = false, bool pbTerminarTransaccion = false, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            SqlCommand Comando = new SqlCommand();
            fnCargarComando(Entidad, psProcedimientoAlmacenado);
            return fnEjecutarConsulta(Comando);
        }

        #endregion

        #region Métodos privados

        private SqlCommand fnCargarComando<T>(T Entidad, string psProcedimientoAlmacenado)
        {
            SqlCommand Comando = new SqlCommand();
            //List<SqlParameter> ListaParameter = new List<SqlParameter>();
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
                            //ListaParameter.Add(new SqlParameter(parametro.ParameterName, oValor));
                            Comando.Parameters[parametro.ParameterName].Value = oValor;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (con != null)
                        con.Close();

                    throw new Exception("Se ha generado un error a la hora de cargar los parametros: " + ex.Message);
                }
                finally
                {
                    if (con != null)
                        con.Close();
                }
            }
            return Comando;
            //return ListaParameter.ToArray();
        }

        private SqlParameter[] fnCargarParametro<T>(T Entidad, string psProcedimientoAlmacenado)
        {
            SqlCommand Comando = new SqlCommand();
            List<SqlParameter> ListaParameter = new List<SqlParameter>();
            object oValor = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_cadenaConexion))
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
                            //Comando.Parameters[parametro.ParameterName].Value = oValor;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Se ha generado un error a la hora de cargar los parametros: " + ex.Message);
            }
            //return Comando;
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
                throw new Exception("Se ha generado un error a la hora de cargar la Entidad: " + ex.Message);
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

                    if (pbRequiereTransaccion)
                        fnIniciarTransaccion(con, pbEnTransaccion, psIsolacion);

                    Comando.Connection = con;

                    _filasAfectadas = Comando.ExecuteNonQuery();

                    if (pbTerminarTransaccion)
                        fnCompletarTransaccion();
                }
                catch (Exception ex)
                {
                    if (con != null)
                        con.Close();

                    if (TransaccionAmbiente != null)
                        TransaccionAmbiente.Dispose();

                    throw new Exception("Error al ejecutar el procedimiento:  " + ex.Message);
                }
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
                throw new Exception("Error al generar la consulta: " + ex.Message);
            }
            return gdtAuxiliar;
        }

        private void fnIniciarTransaccion(SqlConnection con, Boolean pbEnTransaccion, System.Transactions.IsolationLevel psIsolacion = System.Transactions.IsolationLevel.ReadCommitted)
        {
            //TransactionOptions tsoOpcion = new TransactionOptions();
            try
            {
                //tsoOpcion.IsolationLevel = psIsolacion;

                if (pbEnTransaccion && TransaccionAmbiente != null)
                {
                    con.EnlistTransaction(System.Transactions.Transaction.Current);
                }
                else
                {
                    //TransaccionAmbiente = new TransactionScope(TransactionScopeOption.Required, tsoOpcion);
                    Transaccion = con.BeginTransaction(psIsolacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al meter la conexión a la transacción: " + ex.Message);
            }
        }

        private void fnCompletarTransaccion()
        {
            try
            {
                TransaccionAmbiente.Complete();
                //Transaccion.Commit();

                if (TransaccionAmbiente != null) TransaccionAmbiente.Dispose();
            }
            catch (Exception ex)
            {
                if (Transaccion != null) Transaccion.Dispose();
            }
        }

        #endregion
    }
}

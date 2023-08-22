using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SolucionPruebas.Presentacion.Web
{
    public partial class Transacciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIniciarTransaccion_Click(object sender, EventArgs e)
        {
            string cadena_conexion = string.Empty;
            int id_persona = 0;
            int id_contacto = 0;
            try
            {
                cadena_conexion = string.Format("Data source={0}; Initial catalog={1};User ID={2}; Password={3};", "192.168.2.13", "PAXSistemaVentas", "sa", "F4cturax10n");
                //cadena_conexion = string.Format("Data source={0}; Initial catalog={1};Trusted_Connection=True;", "localhost", "Prueba");
                using (SqlConnection con = new SqlConnection(cadena_conexion))
                {
                    con.Open();
                    using (SqlTransaction tran = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("usp_Personas_ins", con))
                            {
                                cmd.Transaction = tran;
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("sNombre", "Luis");

                                id_persona = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            using (SqlCommand cmd = new SqlCommand("usp_Contacto_ins", con))
                            {
                                cmd.Transaction = tran;
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("nId_Persona", id_persona);
                                cmd.Parameters.AddWithValue("sNombre", "Tony");

                                id_contacto = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            //Servicios.Pista.InsertarPista();

                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
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
                throw new System.Exception(ex.Message);
            }
            
        }

        protected void btnIniciarTransaccionAmbiente_Click(object sender, EventArgs e)
        {
            string cadena_conexion = string.Empty;
            int id_persona;
            int id_contacto;
            using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, System.TimeSpan.MaxValue))
            {
                cadena_conexion = string.Format("Data source={0}; Initial catalog={1};User ID={2}; Password={3};", "192.168.2.13", "PAXSistemaVentas", "sa", "F4cturax10n");
                //cadena_conexion = string.Format("Data source={0}; Initial catalog={1};Trusted_Connection=True;", "localhost", "Prueba");
                try
                {
                    //using (SqlConnection conexion = new SqlConnection(cadena_conexion))
                    //{
                    //    conexion.Open();
                    //    using (SqlCommand comando = new SqlCommand("usp_Personas_ins", conexion))
                    //    {
                    //        comando.CommandType = CommandType.StoredProcedure;
                    //        comando.Parameters.AddWithValue("sNombre", "Nena");

                    //        id_persona = Convert.ToInt32(comando.ExecuteScalar());
                    //        if (id_persona <= 0)
                    //        {
                    //            return;
                    //        }
                    //    }
                    //}
                    //using (SqlConnection conexion = new SqlConnection(cadena_conexion))
                    //{
                    //    conexion.Open();
                    //    using (SqlCommand comando = new SqlCommand("usp_Contacto_ins", conexion))
                    //    {
                    //        comando.CommandType = CommandType.StoredProcedure;
                    //        comando.Parameters.AddWithValue("nId_Persona", id_persona);
                    //        comando.Parameters.AddWithValue("sNombre", "Alonso");

                    //        id_contacto = Convert.ToInt32(comando.ExecuteScalar());
                    //        if (id_contacto <= 0)
                    //        {
                    //            return;
                    //        }
                    //    }
                    //}

                    using (Utilerias.SQL.InterfazSQL conexion = new Utilerias.SQL.InterfazSQL(cadena_conexion))
                    { 
                        conexion.AgregarParametro("sNombre", "Ismael");
                        id_persona = Convert.ToInt32(conexion.TraerEscalar("usp_Personas_ins", true));
                    }

                    using (Utilerias.SQL.InterfazSQL conexion = new Utilerias.SQL.InterfazSQL(cadena_conexion))
                    {
                        conexion.AgregarParametro("nId_Persona", id_persona);
                        conexion.AgregarParametro("sNombre", "Ismael");
                        id_contacto = Convert.ToInt32(conexion.TraerEscalar("usp_Contacto_ins", true));
                    }                    

                    //Servicios.Pista.InsertarPista();

                    tran.Complete();
                }
                catch (Exception ex)
                {
                    tran.Dispose();
                    throw new System.Exception(ex.Message);
                }
            }
        }
    }
}
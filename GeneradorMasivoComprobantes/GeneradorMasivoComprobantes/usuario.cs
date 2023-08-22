using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using GeneradorMasivoComprobantes.Properties;
using System.Configuration;

namespace GeneradorMasivoComprobantes
{
    public partial class usuario : Form
    {
        public usuario()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            String sUsuario = txtUsuario.Text;
            DataTable tabla = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_InicioSesion_RecuperaUsu_Sel";
                        cmd.Parameters.Add(new SqlParameter("sClaveUsuario", sUsuario));
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(tabla);
                        }

                        int nIdUsuario = -1;

                        try
                        {
                            nIdUsuario = Convert.ToInt32(tabla.Rows[0]["id_usuario"]);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al recuperar datos de usuario : " + sUsuario + ": "+ex);
                        }
                        con.Close();
                        con.Dispose();

                        if (nIdUsuario != -1)
                        {
                            int nestructura = obtenerEstructura(nIdUsuario);
                            if (nestructura != 0)
                            {
                                MessageBox.Show("Bienvenido " + sUsuario + ".");
                                Form1 formu = new Form1(nIdUsuario, nestructura);
                                formu.Visible = true;
                                this.Visible = false;
                            }
                            else
                            {
                                MessageBox.Show("El usuario no tiene estructura.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("El usuario no existe.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar un catálogo: " + ex);
            }
        }

        private static int obtenerEstructura(int idUsuario)
        {
            int estructura = 0;
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Ctp_RecuperaEstructura_sel", con))
                        {

                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("nId_Usuario", idUsuario);
                            try
                            {
                                estructura = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error al obtener estructura de usuario: " + ex);
                                return 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener estructura de usuario: " + ex);
                        return 0;
                    }
                    finally
                    {
                        con.Close();
                    }
                    return estructura;
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void usuario_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

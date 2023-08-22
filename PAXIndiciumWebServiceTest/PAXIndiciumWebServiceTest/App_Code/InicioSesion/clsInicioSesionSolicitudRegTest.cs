using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Clase encargada de preparar el contribuyente para el registro.
/// </summary>
public class clsInicioSesionSolicitudRegTest
{
    private string conInicioSesion = "conInicioSesionTest";

    /// <summary>
    /// Encargado de ir a buscar la existencia del usuario.
    /// </summary>
    /// <param name="sUsuario">Usuario del contribuyente</param>
    /// <param name="sPassword">Contraseña del contribuyente</param>
    /// <returns>Regresa los datos del usuario</returns>
    public DataTable fnBuscarUsuario(string sUsuario)
    {
        DataTable tabla = new DataTable();

        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings[conInicioSesion].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_InicioSesion_RecuperaUsu_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sClaveUsuario", sUsuario);
                    con.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(tabla);
                    }
                }
            }
            catch (Exception ex)
            {
                clsErrorLogTest.fnNuevaEntrada(ex, clsErrorLogTest.TipoErroresLog.Conexion);
            }
            finally
            {
                con.Close();
            }
        }

        return tabla;
    }
}
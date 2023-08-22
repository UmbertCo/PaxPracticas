using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Clase de capa de negocio para la pantalla webGlobalSoporte
/// </summary>
public class clsGlobalSoporte
{
    /// <summary>
    /// Obtiene la lista de tipos de incidentes disponibles
    /// </summary>
    /// <returns>DataTable con la lista de tipos de incidentes</returns>
    public DataTable fnCargarAsuntos()
    {
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Categorias_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;
    }

    /// <summary>
    /// Guarda en la base de datos el registro del incidente
    /// </summary>
    /// <param name="psIdCategoria">Identificador de la categoría general a la que pertenece el incidente</param>
    /// <param name="psDescripción">Descripción del incidente sucedido</param>
    ///  /// <param name="psIdUsuarioSop">identificador del usuario que atendera el incidente</param>
    /// <returns>Devuelve una cadena con el número de ticket a ocho caracteres</returns>
    public string fnEnviarTicket(string psIdCategoria, string psDescripción, int psIdUsuarioSop, string psRuta, int psIdUsuario, int pIdRelacion)
    {
        string sTicket = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                string nEmail = clsComun.fnUsuarioEnSesion().email;
                sTicket = clsGeneraLlaves.GenerarTicket();

                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Incidente_Ins", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", psIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("nTipo_Incidente", psIdCategoria));
                    cmd.Parameters.Add(new SqlParameter("sDescripcion", psDescripción));
                    cmd.Parameters.Add(new SqlParameter("sTicket", sTicket));
                    cmd.Parameters.Add(new SqlParameter("nid_Usuario_Sop", psIdUsuarioSop));
                    cmd.Parameters.Add(new SqlParameter("nId_Relacion", pIdRelacion));
                    
                    con.Open();
                    int retVal = cmd.ExecuteNonQuery();

                    if (retVal != 0)
                    {
                        fnEnviarNotificacion(sTicket, psIdCategoria, psRuta, pIdRelacion);
                    }
                    else
                        throw new Exception("No se inserto ningún registro");

                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
                sTicket = string.Empty;
            }
            catch (Exception ex)
            {
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
                sTicket = string.Empty;
            }
        }
        return sTicket;
    }

    /// <summary>
    /// Envía un correo electrónico tanto al usuario que levanto el ticket como al área de soporte
    /// El correo contiene la información relacionada al incidente
    /// </summary>
    /// <param name="psTicket">Número de ticket generado</param>
    /// <param name="psIdCategoria">Identificador de la categoría a la que pertenece le incidente</param>
    private void fnEnviarNotificacion(string psTicket, string psIdCategoria, string psRuta, int psIdRelacion)
    {
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        DataTable dtResultado = new DataTable();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Usuario_Reporta_Incidente", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sTicket", psTicket));
                    cmd.Parameters.Add(new SqlParameter("nIdRelacion", psIdRelacion));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }

                email.EnviarCorreoticket(clsComun.fnUsuarioEnSesion().email + ", " + dtResultado.Rows[0]["email"].ToString(),
                            string.Format(Resources.resCorpusCFDIEs.varTicketSubject, psTicket, clsComun.fnUsuarioEnSesion().userName),
                            string.Format(Resources.resCorpusCFDIEs.varTicketMailBody1, psTicket, dtResultado.Rows[0]["nombre"].ToString()), psRuta);
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
    }

    /// <summary>
    /// Obtiene el identificador del usuario de soporte que atendera el tipo de incidencia
    /// </summary>
    /// <param name="psTipoIncidente">Identificador del tipo de incidente</param>
    public DataSet fnObtieneUsuarioSoporte(int psTipoIncidente)
    {
        DataSet dsResultado = new DataSet();

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Incidente_Encargado_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nIncidencia", psTipoIncidente));
                    
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dsResultado;
    }

    /// <summary>
    /// Obtiene todas las incidencias asignadas al usuario activo
    /// </summary>
    /// <param name="nId_Usuario">Identificador del usuario activo</param>
    public DataTable fnObtieneIncidenciasporUsuario(int psIdUsuario)
    {
        DataTable dtResultado = new DataTable("Sucursales");

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Incidencias_Usuario_sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", psIdUsuario));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return dtResultado;

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
            string[] psExtension = null;
            string Extensiones = clsComun.ObtenerParamentro("ExtInc");
            string[] Extension = Extensiones.Split(',');
            psExtension = psArchivo.Split('.');
            string Ext = null;
            Ext = psExtension[1];
            foreach (string ExVal in Extension)
            {
                if (ExVal.Trim() == Ext)
                {
                    valor = true;
                    return valor;                    
                }             
            }
       
        }
        catch (Exception ex)
        {
        }
        return valor;
    }

    /// <summary>
    /// verifica el tamaño del archivo
    /// 
    /// </summary>
    /// <param name="psArchivo">tamaño del archivo en KB</param>
    public bool fnVerificaTamanioMax(int psTamanio)
    {
        bool valor = false;
        try
        {

            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFile"));

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

        }
        return valor;
    }
}
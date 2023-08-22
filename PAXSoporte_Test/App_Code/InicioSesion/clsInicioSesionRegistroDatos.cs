using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Clase encargada de guardar los datos para el contribuyente.
/// </summary>
public class clsInicioSesionRegistroDatos
{
    /// <summary>
    /// Registrar los datos del contribuyente
    /// </summary>
    /// <param name="sRfc"></param>
    /// <param name="sRazonSocial"></param>
    /// <param name="sSucursal"></param>
    /// <param name="nId_Contribuyente"></param>
    /// <param name="nId_Estado"></param>
    /// <param name="sMunicipio"></param>
    /// <param name="sLocalidad"></param>
    /// <param name="mKey"></param>
    /// <param name="mCertificado"></param>
    /// <param name="mKeyPem"></param>
    /// <param name="dFecha_Inicio"></param>
    /// <param name="dFecha_Termino"></param>
    /// <param name="sPassword"></param>
    /// <param name="sCalle"></param>
    /// <param name="sNumero_Exterior"></param>
    /// <param name="sNumero_Interior"></param>
    /// <param name="sColonia"></param>
    /// <param name="sReferencia"></param>
    /// <param name="sCodigo_Postal"></param>
    /// <param name="nId_Usuario"></param>
    /// <returns>Regresa una valor booleano para verificar el estatus</returns>
    public static bool solicitudRegistroContribuyente( string psRfc, string psRazonSocial, string psSucursal, int pnIdContribuyente, int pnIdEstado, byte[] psMunicipio, byte[] psLocalidad, byte[] pmKey,
                                                       byte[] pmCertificado, byte[] pmKeyPem, DateTime pdFechaInicio, DateTime pdFechaTermino, byte[] psPassword, byte[] psCalle, byte[] psNumeroExterior,
                                                       byte[] psNumeroInterior, byte[] psColonia, string psReferencia, byte[] psCodigoPostal, int pnIdUsuario)
    {
        bool bRetorno = false;
        int nRetorno  = 0;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_InicioSesion_RegistraDatos";
                    cmd.Parameters.Add(new SqlParameter("sRfc", psRfc));
                    cmd.Parameters.Add(new SqlParameter("sRazonSocial", psRazonSocial));
                    cmd.Parameters.Add(new SqlParameter("sSucursal", psSucursal));
                    cmd.Parameters.Add(new SqlParameter("nId_Contribuyente", pnIdContribuyente));
                    cmd.Parameters.Add(new SqlParameter("nId_Estado", pnIdEstado));
                    cmd.Parameters.Add(new SqlParameter("sMunicipio", psMunicipio));
                    cmd.Parameters.Add(new SqlParameter("sLocalidad", psLocalidad));
                    cmd.Parameters.Add(new SqlParameter("mKey", pmKey));
                    cmd.Parameters.Add(new SqlParameter("mCertificado", pmCertificado));
                    cmd.Parameters.Add(new SqlParameter("mKeyPem", pmKeyPem));
                    cmd.Parameters.Add(new SqlParameter("dFecha_Inicio", pdFechaInicio));
                    cmd.Parameters.Add(new SqlParameter("dFecha_Termino", pdFechaTermino));
                    cmd.Parameters.Add(new SqlParameter("sPassword", psPassword));
                    cmd.Parameters.Add(new SqlParameter("sCalle", psCalle));
                    cmd.Parameters.Add(new SqlParameter("sNumero_Exterior", psNumeroExterior));
                    cmd.Parameters.Add(new SqlParameter("sNumero_Interior", psNumeroInterior));
                    cmd.Parameters.Add(new SqlParameter("sColonia", psColonia));
                    cmd.Parameters.Add(new SqlParameter("sReferencia", psReferencia));
                    cmd.Parameters.Add(new SqlParameter("sCodigo_Postal", psCodigoPostal));
                    cmd.Parameters.Add(new SqlParameter("nId_Usuario", pnIdUsuario));

                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                    bRetorno = Convert.ToBoolean(nRetorno);
                }
            }
            catch (Exception ex)
            {
                con.Close();
                bRetorno = false;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "solicitudRegistroContribuyente", "clsInicioSesionSolicitudReg");
            }
            finally
            {
                con.Close();
            }
        }
            return bRetorno;
    }
}
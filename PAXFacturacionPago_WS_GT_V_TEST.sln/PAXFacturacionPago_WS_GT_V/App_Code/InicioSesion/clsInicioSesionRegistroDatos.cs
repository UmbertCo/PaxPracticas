using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

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
    public static bool solicitudRegistroContribuyente( string   sRfc,
                                                string   sRazonSocial,
                                                string   sSucursal,
                                                int      nId_Contribuyente,
                                                int      nId_Estado,
                                                string   sMunicipio,
                                                string   sLocalidad,
                                                byte[]   mKey,
                                                byte[]   mCertificado,
                                                byte[]   mKeyPem,
                                                DateTime dFecha_Inicio,
                                                DateTime dFecha_Termino,
                                                string   sPassword,
                                                string   sCalle,
                                                string   sNumero_Exterior,
                                                string   sNumero_Interior,
                                                string   sColonia,
                                                string   sReferencia,
                                                string   sCodigo_Postal,
                                                int      nId_Usuario,
                                                byte[]   nPfx,
                                                int      id_version)
    {

        bool bRetorno = false;
        int nRetorno  = 0;


        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {

            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            try
            {

                SqlCommand cmd = new SqlCommand("usp_InicioSesion_RegistraDatos", con);

                cmd.Transaction = tran;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("sRfc", sRfc);
                cmd.Parameters.AddWithValue("sRazonSocial", sRazonSocial);
                cmd.Parameters.AddWithValue("sSucursal", sSucursal);
                cmd.Parameters.AddWithValue("nId_Contribuyente", nId_Contribuyente);
                cmd.Parameters.AddWithValue("nId_Estado", nId_Estado);
                cmd.Parameters.AddWithValue("sMunicipio", sMunicipio);
                cmd.Parameters.AddWithValue("sLocalidad", sLocalidad);
                cmd.Parameters.AddWithValue("mKey", mKey);
                cmd.Parameters.AddWithValue("mCertificado", mCertificado);
                cmd.Parameters.AddWithValue("mKeyPem", mKeyPem);
                cmd.Parameters.AddWithValue("dFecha_Inicio", dFecha_Inicio);
                cmd.Parameters.AddWithValue("dFecha_Termino", dFecha_Termino);
                cmd.Parameters.AddWithValue("sPassword", sPassword);
                cmd.Parameters.AddWithValue("sCalle", sCalle);
                cmd.Parameters.AddWithValue("sNumero_Exterior", sNumero_Exterior);
                cmd.Parameters.AddWithValue("sNumero_Interior", sNumero_Interior);
                cmd.Parameters.AddWithValue("sColonia", sColonia);
                cmd.Parameters.AddWithValue("sReferencia", sReferencia);
                cmd.Parameters.AddWithValue("sCodigo_Postal", sCodigo_Postal);
                cmd.Parameters.AddWithValue("nId_Usuario", nId_Usuario);
                cmd.Parameters.AddWithValue("@mPfx", nPfx);
                cmd.Parameters.AddWithValue("nId_Version", id_version);


                nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                bRetorno = Convert.ToBoolean(nRetorno);

                tran.Commit();


            }
            catch (Exception ex)
            {
                tran.Rollback();
                bRetorno = false;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }


        return bRetorno;
    }


    /// <summary>
    /// Recupera la lista de versiones vigentes, o una en especifico
    /// </summary>
    /// <param name="sVersion"></param>
    /// <returns></returns>
    public static DataTable fnRecuperaVersionesVigentes(string sVersion)
    {

        DataTable tabla = new DataTable();

        try
        {
            Utilerias.SQL.InterfazSQL conexion = clsComun.fnCrearConexion("conConfiguracion");
            if(sVersion != "")
            conexion.AgregarParametro("sVersion", sVersion);

            conexion.Query("usp_Con_Versiones_Vigentes_Sel", true, ref tabla);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }

        return tabla;
    }


    /// <summary>
    /// Inserta el domicilio de expedicion.
    /// </summary>
    /// <param name="nId_Contribuyente"></param>
    /// <param name="nId_Estado"></param>
    /// <param name="nId_Pais"></param>
    /// <param name="sMunicipio"></param>
    /// <param name="sCalle"></param>
    /// <param name="sCodigo_Postal"></param>
    /// <returns></returns>
    public static bool fnRegistraDomicilioEx(int nId_Contribuyente,
                                            int nId_Estado,
                                            int nId_Pais,
                                            string sMunicipio,
                                            string sCalle,
                                            string sCodigo_Postal)
    {

        bool bRetorno = false;
        int nRetorno = 0;


        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {

            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            try
            {

                SqlCommand cmd = new SqlCommand("usp_Con_Domicilios_Ex_Ins", con);

                cmd.Transaction = tran;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("sMunicipio", sMunicipio);
                cmd.Parameters.AddWithValue("sCalle", sCalle);
                cmd.Parameters.AddWithValue("sCodigo_Postal", sCodigo_Postal);
                cmd.Parameters.AddWithValue("nId_Estado", nId_Estado);
                cmd.Parameters.AddWithValue("nId_Pais", nId_Pais);
                cmd.Parameters.AddWithValue("nId_Contribuyente", nId_Contribuyente);


                nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                bRetorno = Convert.ToBoolean(nRetorno);

                tran.Commit();


            }
            catch (Exception ex)
            {
                tran.Rollback();
                bRetorno = false;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }


        return bRetorno;
    }



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
    public static bool solicitudRegistroContribuyenteDistribuidor(string sRfc,
                                                string sRazonSocial,
                                                string sSucursal,
                                                int nId_Contribuyente,
                                                int nId_Estado,
                                                string sMunicipio,
                                                string sLocalidad,                                             
                                                string sCalle,
                                                string sNumero_Exterior,
                                                string sNumero_Interior,
                                                string sColonia,
                                                string sReferencia,
                                                string sCodigo_Postal,
                                                int nId_Usuario,
                                                int id_version, 
                                                byte[] Logo,
                                                string sRegimenFiscal)
    {
        bool bRetorno = false;
        int nRetorno = 0;

        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {

            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            try
            {
                SqlCommand cmd = new SqlCommand("usp_InicioSesion_RegistraDatosDistribuidor_ins", con);

                cmd.Transaction = tran;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("sRfc", sRfc);
                cmd.Parameters.AddWithValue("sRazonSocial", sRazonSocial);
                cmd.Parameters.AddWithValue("sSucursal", sSucursal);
                cmd.Parameters.AddWithValue("nId_Contribuyente", nId_Contribuyente);
                cmd.Parameters.AddWithValue("nId_Estado", nId_Estado);
                cmd.Parameters.AddWithValue("sMunicipio", sMunicipio);
                cmd.Parameters.AddWithValue("sLocalidad", sLocalidad);
                cmd.Parameters.AddWithValue("sCalle", sCalle);
                cmd.Parameters.AddWithValue("sNumero_Exterior", sNumero_Exterior);
                cmd.Parameters.AddWithValue("sNumero_Interior", sNumero_Interior);
                cmd.Parameters.AddWithValue("sColonia", sColonia);
                cmd.Parameters.AddWithValue("sReferencia", sReferencia);
                cmd.Parameters.AddWithValue("sCodigo_Postal", sCodigo_Postal);
                cmd.Parameters.AddWithValue("nId_Usuario", nId_Usuario);
                cmd.Parameters.AddWithValue("nId_Version", id_version);
                cmd.Parameters.AddWithValue("nLogo", Logo);
                if (sRegimenFiscal != string.Empty)
                    cmd.Parameters.AddWithValue("sRegimen_Fiscal", sRegimenFiscal);

                nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                bRetorno = Convert.ToBoolean(nRetorno);

                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                bRetorno = false;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }
        return bRetorno;
    }

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
    public static bool solicitudRegistroContribuyenteCobro(string sRfc,
                                                string sRazonSocial,
                                                string sSucursal,
                                                int nId_Contribuyente,
                                                int nId_Estado,
                                                string sMunicipio,
                                                string sLocalidad,
                                                byte[] mKey,
                                                byte[] mCertificado,
                                                byte[] mKeyPem,
                                                DateTime dFecha_Inicio,
                                                DateTime dFecha_Termino,
                                                string sPassword,
                                                string sCalle,
                                                string sNumero_Exterior,
                                                string sNumero_Interior,
                                                string sColonia,
                                                string sReferencia,
                                                string sCodigo_Postal,
                                                int nId_Usuario,
                                                byte[] nPfx,
                                                int id_version,
                                                byte[] logo,
                                                string sRegimenFiscal
                                                )
    {

        bool bRetorno = false;
        int nRetorno = 0;


        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conInicioSesion"].ConnectionString;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {

            con.Open();
            SqlTransaction tran = con.BeginTransaction();

            try
            {

                SqlCommand cmd = new SqlCommand("usp_InicioSesion_RegistraDatos_Cobro", con);

                cmd.Transaction = tran;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("sRfc", sRfc);
                cmd.Parameters.AddWithValue("sRazonSocial", sRazonSocial);
                cmd.Parameters.AddWithValue("sSucursal", sSucursal);
                cmd.Parameters.AddWithValue("nId_Contribuyente", nId_Contribuyente);
                cmd.Parameters.AddWithValue("nId_Estado", nId_Estado);
                cmd.Parameters.AddWithValue("sMunicipio", sMunicipio);
                cmd.Parameters.AddWithValue("sLocalidad", sLocalidad);
                cmd.Parameters.AddWithValue("mKey", mKey);
                cmd.Parameters.AddWithValue("mCertificado", mCertificado);
                cmd.Parameters.AddWithValue("mKeyPem", mKeyPem);
                cmd.Parameters.AddWithValue("dFecha_Inicio", dFecha_Inicio);
                cmd.Parameters.AddWithValue("dFecha_Termino", dFecha_Termino);
                cmd.Parameters.AddWithValue("sPassword", sPassword);
                cmd.Parameters.AddWithValue("sCalle", sCalle);
                cmd.Parameters.AddWithValue("sNumero_Exterior", sNumero_Exterior);
                cmd.Parameters.AddWithValue("sNumero_Interior", sNumero_Interior);
                cmd.Parameters.AddWithValue("sColonia", sColonia);
                cmd.Parameters.AddWithValue("sReferencia", sReferencia);
                cmd.Parameters.AddWithValue("sCodigo_Postal", sCodigo_Postal);
                cmd.Parameters.AddWithValue("nId_Usuario", nId_Usuario);
                cmd.Parameters.AddWithValue("@mPfx", nPfx);
                cmd.Parameters.AddWithValue("@nId_Version", id_version);
                cmd.Parameters.AddWithValue("@nLogo", logo);
                if (sRegimenFiscal != string.Empty)
                    cmd.Parameters.AddWithValue("@sRegimen_Fiscal", sRegimenFiscal);

                nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                bRetorno = Convert.ToBoolean(nRetorno);

                tran.Commit();


            }
            catch (Exception ex)
            {
                tran.Rollback();
                bRetorno = false;
                clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            }
        }


        return bRetorno;
    }

}
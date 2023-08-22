using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;

/// <summary>
/// Clase de capa de negocio para la pantalla webOperacionSucursales
/// </summary>
public class clsOperacionSucursales
{

    /// <summary>
    /// Elimina de manera lógica una sucursal
    /// </summary>
    /// <param name="nIdSucursal">Identificador de la sucursal</param>
    /// <returns>Retorna un entero indicando las filas afectadas en la consulta</returns>
    public int fnEliminarSucursal(int nIdSucursal)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("nIdSucursal", nIdSucursal);

        //    return Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Sucursal_Borrar_up", true));
        //}
        //catch (Exception ex)
        //{
        //    return 0;
        //}

        int nRetorno = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Sucursal_Borrar_up";
                    cmd.Parameters.Add(new SqlParameter("nIdSucursal", nIdSucursal));
                    nRetorno = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
        return nRetorno;
    }

    /// <summary>
    /// Obtiene las empresas relacionadas al usuario
    /// </summary>
    /// <param name="nIdUsuario"></param>
    /// <returns></returns>
    public DataTable fnObtenerEmpresasUsuario(int nIdUsuario)
    {
        //DataTable dtEmpresas = new DataTable();
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("@IdUsuario", nIdUsuario);
        //    giSql.Query("usp_rfp_Obtener_Empresas_Sel", true, ref dtEmpresas);
        //}
        //catch (Exception ex)
        //{
        //    return null;
        //}
        //return dtEmpresas;
        DataTable dtEmpresas = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Obtener_Empresas_Sel";
                    cmd.Parameters.Add(new SqlParameter("IdUsuario", nIdUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtEmpresas);
                    }
                }
            }
        }

        catch (Exception)
        {
            return null;
        }
        return dtEmpresas;
    }
    /// <summary>
    /// Guarda o edita una empresa
    /// </summary>
    /// <param name="nIdEmpresa"></param>
    /// <param name="nRazonSocial"></param>
    /// <param name="sRfc"></param>
    /// <param name="bLogo"></param>
    /// <returns></returns>
    public int fnGuardarEmpresa(int nIdUsuario, int nIdEmpresa, string nRazonSocial, string sRfc, byte[] bLogo)
    {
        //int res = 0;
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    if (nIdEmpresa > 0)
        //    {
        //        giSql.AgregarParametro("@nIdEmpresa", nIdEmpresa);
        //    }
        //    giSql.AgregarParametro("@nIdUsuario", nIdUsuario);
        //    giSql.AgregarParametro("@sRazonSocial", nRazonSocial);
        //    giSql.AgregarParametro("@sRfc", sRfc);
        //    if (bLogo.Length > 0)
        //    {
        //        giSql.AgregarParametro("@iLogo", bLogo);
        //    }
        //    res = Convert.ToInt32(giSql.TraerEscalar("usp_cfd_Registro_Empresa_ins", true));
        //}
        //catch (Exception ex)
        //{
        //    return 0;
        //}
        //return res;
        int res = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_cfd_Registro_Empresa_ins";
                    if (nIdEmpresa > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@nIdEmpresa", nIdEmpresa));
                    }
                    cmd.Parameters.Add(new SqlParameter("@nIdUsuario", nIdUsuario));
                    cmd.Parameters.Add(new SqlParameter("@sRazonSocial", nRazonSocial));
                    cmd.Parameters.Add(new SqlParameter("@sRfc", sRfc));
                    if (bLogo.Length > 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@iLogo", bLogo));
                    }
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return res;
    }
    /// <summary>
    /// Verifica si la empresa está relacionada con alguna sucursal
    /// </summary>
    /// <param name="nIdEmpresa"></param>
    /// <returns></returns>
    public bool fnVerificaEmpresaSucursal(int nIdEmpresa)
    {

        //int res = 0;
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("@nIdEmpresa", nIdEmpresa);
        //    res = Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Relacion_Empresa_Sucursal_sel", true));
        //}
        //catch (Exception ex)
        //{
        //    return true;
        //}
        //return res>0;
        int res = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Relacion_Empresa_Sucursal_sel";
                    cmd.Parameters.Add(new SqlParameter("@nIdEmpresa", nIdEmpresa));
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);

        }
        return res > 0;
    }

    /// <summary>
    /// Elimina la empresa selecionada
    /// </summary>
    /// <param name="nIdEmpresa"></param>
    /// <returns></returns>
    public bool fnEliminarEmpresa(int nIdEmpresa)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("@nIdEmpresa", nIdEmpresa);
        //    giSql.TraerEscalar("usp_rfp_Empresa_Baja_up", true);
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    return false;
        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Empresa_Baja_up";
                    cmd.Parameters.Add(new SqlParameter("@nIdEmpresa", nIdEmpresa));
                    cmd.ExecuteScalar();
                    con.Close();
                    con.Dispose();
                    return true;
                }
            }
        }

        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return false;
        }
    }
    /// <summary>
    /// Obtiene las sucursales de la empresa y el usuario seleccionado
    /// </summary>
    /// <param name="nIdEmpresa"></param>
    /// <param name="nIdUsuario"></param>
    /// <returns></returns>
    public DataTable fnObtenerSucursales(int nIdEmpresa, int nIdUsuario)
    {
        //DataTable res = new DataTable();
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("@IdEmpresa", nIdEmpresa);
        //    giSql.AgregarParametro("@IdUsuario", nIdUsuario);
        //    giSql.Query("usp_rfp_Obtener_Sucursales_Sel", true, ref res);

        //}
        //catch (Exception ex)
        //{

        //}
        //return res;

        DataTable res = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Obtener_Sucursales_Sel";
                    cmd.Parameters.Add(new SqlParameter("IdEmpresa", nIdEmpresa));
                    cmd.Parameters.Add(new SqlParameter("IdUsuario", nIdUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(res);
                    }
                }
            }
        }

        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return res;
    }

    public DataTable fnObtenerSucursal(int nIdSucursal)
    {
        //DataTable res = new DataTable();
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
        //    giSql.Query("usp_rfp_Obtener_Sucursal_sel", true, ref res);

        //}
        //catch (Exception ex)
        //{

        //}
        //return res;
        DataTable res = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Obtener_Sucursal_sel";
                    cmd.Parameters.Add(new SqlParameter("@nIdSucursal", nIdSucursal));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(res);
                    }
                }
            }
        }

        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return res;
    }
    /// <summary>
    /// Registra o edita una sucursal
    /// </summary>
    /// <param name="nIdSucursal"></param>
    /// <param name="nIdEmpresa"></param>
    /// <param name="nIdUsuario"></param>
    /// <param name="sNombre"></param>
    /// <param name="nIdMunicipio"></param>
    /// <param name="sLocalidad"></param>
    /// <param name="sColonia"></param>
    /// <param name="sCalle"></param>
    /// <param name="sNoExterior"></param>
    /// <param name="sNoInterior"></param>
    /// <param name="nCodigoPostal"></param>
    /// <returns></returns>
    public int fnGuardarSucursal(int nIdSucursal, int nIdEmpresa, int nIdUsuario, string sNombre,
        int nIdMunicipio, string sLocalidad, string sColonia, string sCalle, string sNoExterior,
        string sNoInterior, string nCodigoPostal, bool bFacturaUnica)
    {
        //int res = 0;
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("@nIdEmpresa", nIdEmpresa);
        //    giSql.AgregarParametro("@nIdUsuario", nIdUsuario);
        //    if(nIdSucursal > 0)
        //        giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
        //    giSql.AgregarParametro("@sNombre", sNombre);
        //    giSql.AgregarParametro("@nIdMunicipio", nIdMunicipio);
        //    if(!string.IsNullOrEmpty(sLocalidad))
        //        giSql.AgregarParametro("@sLocalidad", sLocalidad);
        //    if (!string.IsNullOrEmpty(sColonia))
        //        giSql.AgregarParametro("@sColonia", sColonia);
        //    if (!string.IsNullOrEmpty(sCalle))
        //        giSql.AgregarParametro("@sCalle", sCalle);
        //    if (!string.IsNullOrEmpty(sNoExterior))
        //        giSql.AgregarParametro("@sNoExterior", sNoExterior);
        //    if (!string.IsNullOrEmpty(sNoInterior))
        //        giSql.AgregarParametro("@sNoInterior", sNoInterior);
        //    if (!string.IsNullOrEmpty(nCodigoPostal))
        //        giSql.AgregarParametro("@nCodigoPostal", nCodigoPostal);
        //    giSql.AgregarParametro("@bFacturaUnica", bFacturaUnica);
        //    res=Convert.ToInt32(giSql.TraerEscalar("usp_rfp_Guardar_Sucursal_ins", true));
        //}
        //catch (Exception ex)
        //{
        //    return 0;
        //}
        //return res;
        int res = 0;
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Guardar_Sucursal_ins";

                    cmd.Parameters.Add(new SqlParameter("@nIdEmpresa", nIdEmpresa));
                    cmd.Parameters.Add(new SqlParameter("@nIdUsuario", nIdUsuario));
                    if (nIdSucursal > 0)
                        cmd.Parameters.Add(new SqlParameter("@nIdSucursal", nIdSucursal));
                    cmd.Parameters.Add(new SqlParameter("@sNombre", sNombre));
                    cmd.Parameters.Add(new SqlParameter("@nIdMunicipio", nIdMunicipio));
                    if (!string.IsNullOrEmpty(sLocalidad))
                        cmd.Parameters.Add(new SqlParameter("@sLocalidad", sLocalidad));
                    if (!string.IsNullOrEmpty(sColonia))
                        cmd.Parameters.Add(new SqlParameter("@sColonia", sColonia));
                    if (!string.IsNullOrEmpty(sCalle))
                        cmd.Parameters.Add(new SqlParameter("@sCalle", sCalle));
                    if (!string.IsNullOrEmpty(sNoExterior))
                        cmd.Parameters.Add(new SqlParameter("@sNoExterior", sNoExterior));
                    if (!string.IsNullOrEmpty(sNoInterior))
                        cmd.Parameters.Add(new SqlParameter("@sNoInterior", sNoInterior));
                    if (!string.IsNullOrEmpty(nCodigoPostal))
                        cmd.Parameters.Add(new SqlParameter("@nCodigoPostal", nCodigoPostal));
                    cmd.Parameters.Add(new SqlParameter("@bFacturaUnica", bFacturaUnica));
                    res = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    con.Dispose();
                }
            }
        }

        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
        return res;
    }

    /// <summary>
    /// Guarda una lista de correos de la sucursal
    /// </summary>
    /// <param name="licCorreos"></param>
    /// <param name="nIdSucursal"></param>
    /// <returns></returns>
    public bool fnGuardarCorreos(ListItemCollection licCorreos, int nIdSucursal)
    {
        //try
        //{
        //    foreach (ListItem liCorreo in licCorreos)
        //    {
        //        giSql = clsComun.fnCrearConexion(conSucursales);
        //        giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
        //        giSql.AgregarParametro("@sCorreo", liCorreo.Text);
        //        giSql.Query("usp_rfp_Guardar_Correo_ins", true);
        //    }
        //    return true;
        //}
        //catch (Exception ex)
        //{
        //    return false;
        //}
        try
        {
            foreach (ListItem liCorreo in licCorreos)
            {
                using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
                {
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "usp_rfp_Guardar_Correo_ins";
                        cmd.Parameters.Add(new SqlParameter("@nIdSucursal", nIdSucursal));
                        cmd.Parameters.Add(new SqlParameter("@sCorreo", liCorreo.Text));
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                    }
                }

            }
            return true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return false;
        }
    }
    /// <summary>
    /// Borra todos los correos de una sucursal
    /// </summary>
    /// <param name="nIdSucursal"></param>
    public void fnBorrarCorreos(int nIdSucursal)
    {
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
        //    giSql.Query("usp_rfp_Borrar_Correos_up", true);

        //}
        //catch (Exception ex)
        //{

        //}
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Borrar_Correos_up";
                    cmd.Parameters.Add(new SqlParameter("@nIdSucursal", nIdSucursal));
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }
    /// <summary>
    /// Obtiene los correos de una sucursal
    /// </summary>
    /// <param name="nIdSucursal"></param>
    /// <returns></returns>
    public DataTable fnObtenerCorreosSucursal(int nIdSucursal)
    {
        //DataTable res = new DataTable();
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("@nIdSucursal", nIdSucursal);
        //    giSql.Query("usp_rfp_Obtener_Correos_sel", true, ref res);

        //}
        //catch (Exception ex)
        //{

        //}
        //return res;
        DataTable res = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Obtener_Correos_sel";
                    cmd.Parameters.Add(new SqlParameter("@nIdSucursal", nIdSucursal));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(res);
                    }
                    con.Close();
                    con.Dispose();
                }
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return res;
    }
    /// <summary>
    /// Obtiene todas las sucursales con sus empresas
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerSucursalesEmpresas()
    {
        //DataTable dtRes = new DataTable();
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.Query("usp_rfp_Obtener_Sucursales_Empresa_sel", true, ref dtRes);
        //}
        //catch (Exception ex)
        //{

        //}
        //return dtRes;
        DataTable dtRes = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Obtener_Sucursales_Empresa_sel";
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtRes);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return dtRes;
    }

    public DataTable fnObtenerSucursalesUsuario(int nIdUsuario)
    {
        //DataTable dtRes = new DataTable();
        //try
        //{
        //    giSql = clsComun.fnCrearConexion(conSucursales);
        //    giSql.AgregarParametro("nIdUsuario",nIdUsuario);
        //    giSql.Query("usp_rfp_Obtener_Sucursales_Usuario_sel", true, ref dtRes);
        //}
        //catch (Exception ex)
        //{

        //}
        //return dtRes;
        DataTable dtRes = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_rfp_Obtener_Sucursales_Usuario_sel";
                    cmd.Parameters.Add(new SqlParameter("nIdUsuario", nIdUsuario));
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtRes);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
        return dtRes;
    }
}
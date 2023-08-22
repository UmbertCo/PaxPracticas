using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;

/// <summary>
/// Summary description for clsOperacionDistribuidores
/// </summary>
public class clsOperacionDistribuidores
{
    private InterfazSQL giSql;
    private string conConfig = "conConfiguracion";

    public int fnInsertaDistribuidor(int pIdUsuario, string sNumeroDist, bool Certificado, DateTime sFechaInicio, DateTime sFechaFinal, string sEstatus)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.AgregarParametro("@nNumeroDist", sNumeroDist);
            giSql.AgregarParametro("@nCertificado", Certificado);
            giSql.AgregarParametro("@sFechaInicio", sFechaInicio);
            giSql.AgregarParametro("@sFechaFin", sFechaFinal);
            giSql.AgregarParametro("@sEstatus", sEstatus);
            Int32 Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Con_RegistroDistribuidor_Ins", true));
            return Resultado;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            int Resultado = 0;
            return Resultado;            
        }
    }

    public int fnObtieneidUsuarioDistribuidor(string sClaveUsuario, string sEmail)
    {
        try
        {
            int Resultado = 0;
        giSql = clsComun.fnCrearConexion(conConfig);
        giSql.AgregarParametro("@sClaveUsuario", sClaveUsuario);
        giSql.AgregarParametro("@sEmail", sEmail);
        Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Con_ObtieneIdUsuario_por_Clave", true));
        return Resultado;
         }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            int Resultado = 0;
            return Resultado;
        }
    }

    public bool fnEliminaDistribuidor(int pIdDistribuidor)
    {
        try
        {            
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
            giSql.NoQuery("usp_Con_BajaDistribuidor_del", true);
            return true;            
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return false;        
        }
    }

    public bool fnActualizaDistribuidor(int pIdDistribuidor, bool Certificado, DateTime sFechaFinal)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
            giSql.AgregarParametro("@nCertificado", Certificado);
            giSql.AgregarParametro("@sFechaFin", sFechaFinal);
            giSql.NoQuery("usp_Con_ActualizaDistribuidor_upd", true);
            bool resultado = true;
            return resultado;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            bool resultado = false;
            return resultado;
        }
    }

    public DataTable fnObtieneDistribuidoresAll()
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.Query("usp_Con_ObtieneDistribuidor_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            DataTable dtAuxiliar = new DataTable();
            dtAuxiliar = null;
            return dtAuxiliar;
        }

    }


    public DataTable fnObtieneDistribuidoresporNumero(string sNumeroDist)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@sNumDist", sNumeroDist);
            giSql.Query("usp_con_ObtenerDistribuidorpornumero_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            DataTable dtAuxiliar = new DataTable();
            dtAuxiliar = null;
            return dtAuxiliar;
        }

    }

    public DataTable fnObtieneDistribuidoresporidUsuario(int nIdUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nidUsuario", nIdUsuario);
            giSql.Query("usp_con_ObtenerDistribuidorporusuario_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            DataTable dtAuxiliar = new DataTable();
            dtAuxiliar = null;
            return dtAuxiliar;
        }

    }



    public bool fnInsertaDistribuidorRelacion(int pIdDist,int pIdUsuario, string sAcceso,string sEstatus, DateTime sFechaCaptura)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);

            giSql.AgregarParametro("@nIdDist", pIdDist);
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.AgregarParametro("@sAcceso", sAcceso);
            giSql.AgregarParametro("@sEstatus", sEstatus);
            giSql.AgregarParametro("@sFechaCap", sFechaCaptura);

            giSql.TraerEscalar("usp_Con_Distribuidor_Usuario_rel_ins", true);
            return true;
           
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);

            return false;
        }
    }


    public bool fnEliminaDistribuidorRelacion(int pIdDistribuidor, int pIdUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdDist", pIdDistribuidor);
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.NoQuery("usp_Con_Distribuidor_Usuario_rel_del", true);
            return true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return false;
        }
    }


    public DataTable fnObtieneClientesporDistribuidor(int nIdDistribuidor)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdDistribuidor", nIdDistribuidor);
            giSql.Query("usp_Con_ClientesDistribuidor_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            DataTable dtAuxiliar = new DataTable();
            dtAuxiliar = null;
            return dtAuxiliar;
        }
    }

    /// <summary>
    /// Retorna la lista de usuarios por distribuidor
    /// </summary>
    /// <param name="nIdDistribuidor"></param>
    /// <returns></returns>
    public DataTable fnObtieneUsuariosporDistribuidor(int nIdDistribuidor)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdDistribuidor", nIdDistribuidor);
            giSql.Query("usp_Con_UsuariosDistribuidor_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            DataTable dtAuxiliar = new DataTable();
            dtAuxiliar = null;
            return dtAuxiliar;
        }
    }

    public DataTable fnObtieneCreditosUsuarioDistribuidor(int pIdUsuario, int pIdestructura)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.AgregarParametro("@nIdEstructura", pIdestructura);
            giSql.Query("usp_Con_ClientesDistCreditos_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return null;
        }
    }

    public bool fnBajaUsuariodeDistribuidor(int pIdDistribuidor, int pIdUsuario, double pCreditos, int pIdUsuarioDist)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
            giSql.AgregarParametro("@nIdUsuarioDistribuidor", pIdUsuarioDist);
            giSql.AgregarParametro("@nCreditos", pCreditos);
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.NoQuery("usp_Con_RegresaCreditosADistribuidor_upd", true);
            return true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return false;
        }
    }


    public bool fnActualizaCreditosUsuariodeDistribuidor(int pIdDistribuidor, int pIdUsuario, double pCreditos, int pEstructuraUsuario,
        double pCreditosAnt, string sAcceso, int pIdDistribuidorUsuario, int pIdEstructuraDistribuidor, double nPrecioUnitario, string sServicio)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
            giSql.AgregarParametro("@nCreditosPos", pCreditos);
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.AgregarParametro("@nIdEstructuraUsuario", pEstructuraUsuario);
            giSql.AgregarParametro("@nCreditosAnt", pCreditosAnt);
            giSql.AgregarParametro("@sStatus", sAcceso);
            giSql.AgregarParametro("@nIdUsuarioDistribuidor", pIdDistribuidorUsuario);
            giSql.AgregarParametro("@nIdEstructuraDistribuidor", pIdEstructuraDistribuidor);
            giSql.AgregarParametro("@sFechaCompra", DateTime.Now);
            giSql.AgregarParametro("@nPrecioUnitario", nPrecioUnitario);
            giSql.AgregarParametro("@sServicio", sServicio);
            giSql.NoQuery("usp_Con_ActualizaCreditosUsuarioDistribuidor", true);
            return true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return false;
        }
    }


    public bool fnActualizaUsuariodeDistribuidor(int pIdDistribuidor, int pIdUsuario, double pCreditos, int pIdUsuarioDist, string sAcceso, int pIdEstructuraUsuario, int pIdEstructuraDistribuidor)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdDistribuidor", pIdDistribuidor);
            giSql.AgregarParametro("@nIdUsuarioDistribuidor", pIdUsuarioDist);
            giSql.AgregarParametro("@nCreditos", pCreditos);
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.AgregarParametro("@sStatus", sAcceso);
            giSql.AgregarParametro("@nIdEstructuraUsuario", pIdEstructuraUsuario);
            giSql.AgregarParametro("@nIdEstructuraDistribuidor", pIdEstructuraDistribuidor);
            giSql.NoQuery("usp_Con_ActualizaCreditosADistribuidor_upd", true);
            return true;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return false;
        }
    }

    /// <summary>
    /// Inserta los modulos de distribuidor al usuario nuevo
    /// </summary>
    /// <param name="pIdUsuario"></param>
    /// <returns></returns>
    public void fnInsertarModulosDistribuidor(int pIdUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nId_Usuario", pIdUsuario);
            giSql.NoQuery("usp_ctp_RegistraModulosADistribuidor_ins", true);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }


    public double fnObtieneCreditosDistribuidor(int pIdUsuario, int pIdestructura)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.AgregarParametro("@nIdEstructura", pIdestructura);
            Double Resultado = Convert.ToDouble(giSql.TraerEscalar("usp_Con_CreditosdeDistribuidor_sel", true));
            return Resultado;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            double Resultado = 0;
            return Resultado;
        }
    }



    public DataTable fnObtieneCreditosDistribuidorporUsuario(int pIdUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.Query("usp_Con_ObtieneCreditosdeDistribuidorporHijo_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);           
            return null;
        }
    }


    public DataTable fnObtieneClientedeDistribuidor(int pIdUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdUsuario", pIdUsuario);
            giSql.Query("usp_ctpClientedeDistribuidorbyIdusuario", true, ref dtAuxiliar);
            return dtAuxiliar;
   
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return null;
        }
    }

    public DataTable fnObtenerDatosUsuario()
    {
        try
        {
        giSql = clsComun.fnCrearConexion(conConfig);
        DataTable dtAuxiliar = new DataTable();
        giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);
        giSql.Query("usp_Con_Usuario_Sel", true, ref dtAuxiliar);
        return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return null;
        }
    }

    /// <summary>
    /// Retorna el reporte de timbres y cancelaciones por usuarios de distribuidor
    /// </summary>
    /// <param name="nIdDistribuidor"></param>
    /// <returns></returns>
    public DataSet fnObtieneReporteDistribuidor(int nIdUsuario, DateTime nFechaIni, DateTime nFechaFin)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataSet dtAuxiliar = new DataSet();
            giSql.AgregarParametro("@nId_Usuario", nIdUsuario);
            giSql.AgregarParametro("@sFechaIni", nFechaIni);
            giSql.AgregarParametro("@sFechaFin", nFechaFin);
            giSql.Query("usp_Con_ReporteTimbradoyCancelado_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            DataSet dtAuxiliar = new DataSet();
            dtAuxiliar = null;
            return dtAuxiliar;
        }
    }
}
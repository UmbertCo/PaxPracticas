using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;
/// <summary>
/// Summary description for clsConfiguracionCreditos
/// </summary>
public class clsConfiguracionCreditos
{
    private InterfazSQL giSql;
    private string conConfig = "conConfiguracion";
    public DataTable fnObtieneServicios()
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.Query("usp_Con_RecuperaServiciosCreditos_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    public DataSet fnRecuperaCreditos(string clave_usuario)
    {

        DataSet creditos = new DataSet();

        try
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");

            if (!string.IsNullOrEmpty(clave_usuario))
                giSql.AgregarParametro("sClave_Usuario", clave_usuario);

            giSql.Query("usp_Con_RecuperaCreditos_Sel", true, ref creditos);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }

        return creditos;
    }

    public void fnActualizaCreditos(int nIdEstructura, double nCreditos, char estatus, DateTime sFechaCompra, double nPrecio, string sServicio)
    {
        try
        {
             DateTime sFechaVigencia = sFechaCompra;
             sFechaVigencia = sFechaVigencia.AddYears(2);
             giSql = clsComun.fnCrearConexion("conConfiguracion");
             giSql.AgregarParametro("@nId_Estructura", nIdEstructura);
             giSql.AgregarParametro("@nCreditos", nCreditos);
             giSql.AgregarParametro("@sEstatus", estatus);
             giSql.AgregarParametro("@sFechaCompra", sFechaCompra);
             giSql.AgregarParametro("@sFechaVigencia", sFechaVigencia);
             giSql.AgregarParametro("@nPrecioUnit", nPrecio);
             giSql.AgregarParametro("@sServicio", sServicio);
             giSql.TraerEscalar("usp_Con_ActualizaCreditos_upd", true);            
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    public DataTable fnObtieneReporteAcumulado(int nIdUsuario, int nIdEstructura, string sFechaCompra, string sFechaVigencia, double nPrecioUnitario)
    {
        try
        {
            DataTable dtAuxiliar = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            giSql.AgregarParametro("@nIdUsuario", nIdUsuario);
            if(nIdEstructura != 0)
            giSql.AgregarParametro("@nIdEstructura", nIdEstructura);
            if (sFechaCompra != "")
            {
                DateTime FechaCompra = Convert.ToDateTime(sFechaCompra);
                giSql.AgregarParametro("@sFechaCompra", FechaCompra);
            }
            if (sFechaVigencia != "")
            {
                DateTime FechaVigencia = Convert.ToDateTime(sFechaVigencia);
                giSql.AgregarParametro("@sFechaVigencia", FechaVigencia);
            }
       
            if(nPrecioUnitario != 0)
            giSql.AgregarParametro("@nPrecioUnitario", nPrecioUnitario);
            giSql.Query("usp_Con_ReporteCreditosAcumulado_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    public DataTable fnObtenerSucursalesCreditosAcumulados(int nIdUsuario)
    {
        try
        {
            DataTable dtAuxiliar = new DataTable();
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            giSql.AgregarParametro("@nIdUsuario", nIdUsuario);
            giSql.Query("usp_Con_ObtenerSucursalesReporteAcumulado_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    public string fnObtenerClaveUsuario(int nIdUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion("conConfiguracion");
            giSql.AgregarParametro("@nIdUsuario", nIdUsuario);
            string claveusuario = Convert.ToString(giSql.TraerEscalar("usp_Con_ObtenerClaveUsuario_sel",true));
            return claveusuario;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

     public void fnActualizaServicios(int nIdEstructura, string sServicio)
    {
        try
        {        
             giSql = clsComun.fnCrearConexion("conConfiguracion");
             giSql.AgregarParametro("@nId_Estructura", nIdEstructura);
             giSql.AgregarParametro("@sServicio", sServicio);
             giSql.TraerEscalar("usp_Con_ActualizaServicios_upd", true);            
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

     public double fnRecuperaPrecioServicio(int pIdServicio)
     {
         try
         {
             giSql = clsComun.fnCrearConexion("conConfiguracion");
             giSql.AgregarParametro("@nIdServicio", pIdServicio);
             double Precio = 0;
             Precio = Convert.ToDouble(giSql.TraerEscalar("usp_Con_RecuperaPrecioServicio_sel", true));
             return Precio;
         }
         catch (Exception ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
             return 0;
         }
     }



     public DataTable fnObtieneReporteHistorico(int nIdEstructura, string sFechaInicio, string sFechaFin)
     {
         try
         {
             DataTable dtAuxiliar = new DataTable();
             giSql = clsComun.fnCrearConexion("conConfiguracion");


             giSql.AgregarParametro("@nIdEstructura", nIdEstructura);
                 if (sFechaInicio != "")
             {
                 DateTime FechaIn = Convert.ToDateTime(sFechaInicio);
                 giSql.AgregarParametro("@sFechaInicio", FechaIn);
             }
                 if (sFechaFin != "")
             {
                 DateTime Fechafin = Convert.ToDateTime(sFechaFin);
                 giSql.AgregarParametro("@sFechaFin", Fechafin);
             }

             giSql.Query("usp_Con_ReporteEstadodeCuentaHistorico_sel", true, ref dtAuxiliar);
             return dtAuxiliar;
         }
         catch (Exception ex)
         {
             clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
             return null;
         }
     }
}
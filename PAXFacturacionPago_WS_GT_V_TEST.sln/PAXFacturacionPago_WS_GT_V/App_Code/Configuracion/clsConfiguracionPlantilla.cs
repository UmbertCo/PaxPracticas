using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;

/// <summary>
/// Summary description for clsConfiguracionPlantilla
/// </summary>
public class clsConfiguracionPlantilla
{
    private InterfazSQL giSql;
    private string conConfig = "conConfiguracion";

    public int fnActualizaPlantilla(int nIdConfiguracion, int nIdPlantilla, int nIdEstructura, string sColor, int nIdUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            if (nIdConfiguracion != 0)
            {
                giSql.AgregarParametro("@nId_Configuracion", nIdConfiguracion);
            }
            giSql.AgregarParametro("@nId_Plantilla", nIdPlantilla);
            giSql.AgregarParametro("@nId_Estructura", nIdEstructura);
            giSql.AgregarParametro("@sColor", sColor);
            giSql.AgregarParametro("@nId_Usuario", nIdUsuario); 

            Int32 Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Ctp_ConfiguracionPlantillas_upd", true));
            return Resultado;
            
        }
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
    }

    public DataTable fnObtieneConfiguracionPlantilla(int nIdEstructura)
    {
        try
        {
            DataTable dtAuxiliar = new DataTable();
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nId_Estructura", nIdEstructura);
            giSql.Query("usp_Ctp_ConfiguracionPlantillas_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    public DataTable fnObtienePlantillasBase()
    {
        try
        {
            DataTable dtAuxiliar = new DataTable();
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.Query("usp_Ctp_RecuperaPlantillasBase_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }


    public int fnRecuperaEstructura(int nIdUsuario)
    {
        try
        {
            DataTable dtAuxiliar = new DataTable();
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nId_Usuario", nIdUsuario);
            Int32 Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Ctp_RecuperaEstructura_sel", true));
            return Resultado;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
    }

    public int fnRecuperaPlantillaRecursiva(int nIdEstructura)
    {
        try
        {
            DataTable dtAuxiliar = new DataTable();
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdEstructura", nIdEstructura);
            Int32 Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_ctp_RecuperaPlantillaRecursiva_sel", true));
            return Resultado;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
    }


        public string fnRecuperaPlantillaNombre(int nIdPlantilla)
    {
        try
        {
            DataTable dtAuxiliar = new DataTable();
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdPlantilla", nIdPlantilla);
            string Resultado = Convert.ToString(giSql.TraerEscalar("usp_ctp_RecuperaPlantillaNombre_sel", true));
            return Resultado;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    
    
}
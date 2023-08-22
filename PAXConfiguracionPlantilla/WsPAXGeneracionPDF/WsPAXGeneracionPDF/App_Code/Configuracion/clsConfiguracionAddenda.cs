using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;
using System.Xml;

/// <summary>
/// Summary description for clsConfiguracionAddenda
/// </summary>
public class clsConfiguracionAddenda
{
    private InterfazSQL giSql;
    private string conConfig = "conConfiguracion";
   
    public DataTable fnObtieneAddendaConfiguracion(int nidEstructura, int nidUsuario)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_estructura", nidEstructura);
            giSql.AgregarParametro("@nId_usuario", nidUsuario);
            giSql.Query("usp_Cfd_AddendaConfiguracion_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return null;
        }
    }

    public int fnObtieneEstructuraCFD(int nidCfd)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nId_Cfd", nidCfd);
            int idEstructura;
            idEstructura = Convert.ToInt32(giSql.TraerEscalar("usp_Cfd_Estructura_Sel_Cobro", true));
            return idEstructura;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
    }

    public int fnObtieneIdUsuarioporContribuyente(int nIdContribuyente)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdContribuyente", nIdContribuyente);
            int idUsuario;
            idUsuario = Convert.ToInt32(giSql.TraerEscalar("usp_Cfd_RecuperaUsuarioContribuyente_sel", true));
            return idUsuario;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
    }

    public DataTable fnObtieneAddendaporIdCfd(int nidCfd, int nidEstructura)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            DataTable dtAuxiliar = new DataTable();
            giSql.AgregarParametro("@nIdCfd", nidCfd);
            giSql.AgregarParametro("@nIdEstructura", nidEstructura);
            XmlDocument Addenda = new XmlDocument();
            giSql.Query("usp_Cfd_ObtieneAddenda_sel", true, ref dtAuxiliar);
            return dtAuxiliar;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            XmlDocument Addenda = new XmlDocument();
            return null;
        }
    }

    public void fnInsertaAddenda(int nidCfd, int nidEstructura, XmlDocument nAddenda, int nidAddenda)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            giSql.AgregarParametro("@nIdCfd", nidCfd);
            giSql.AgregarParametro("@nIdEstructura", nidEstructura);
            giSql.AgregarParametro("@sAddenda", nAddenda.OuterXml);
            giSql.AgregarParametro("@nIdModulo", nidAddenda);
            giSql.TraerEscalar("usp_Cfd_InsertaAddenda_ins", true);

        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
    }

    public string fnObtieneNameSpace(int nIdModulo)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            string Namespacex = string.Empty;
            giSql.AgregarParametro("@nIdModulo", nIdModulo);
            Namespacex = Convert.ToString(giSql.TraerEscalar("usp_Cfd_ObtieneAddendaConfigurada_sel", true));
            return Namespacex;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return string.Empty;
        }
    }


    public int fnObtieneidAddenda(string sNombre)
    {
        try
        {
            giSql = clsComun.fnCrearConexion(conConfig);
            int idmodulo;
            giSql.AgregarParametro("@nNombre", sNombre);
            idmodulo = Convert.ToInt32(giSql.TraerEscalar("usp_Cfd_ObtieneIdAddendaConfiguracion_sel", true));
            return idmodulo;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
            return 0;
        }
    }
}
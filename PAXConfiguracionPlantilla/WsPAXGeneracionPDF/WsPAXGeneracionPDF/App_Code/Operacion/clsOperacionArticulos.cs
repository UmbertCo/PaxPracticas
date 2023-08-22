using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;

/// <summary>
/// Summary description for clsOperacionArticulos
/// </summary>
public class clsOperacionArticulos
{
    private InterfazSQL giSql;
    private string conCuenta = "conControl";

    public int fnUpdateArticulo(int pId_Articulo, string psDescripcion, string psMedida, double pPrecio, string pIva, double pIeps
        , double pIsr, double pIvaRetenido, string psMoneda, string psEstatus, int pIdEstructura, string pnClave)
    {
                   
        giSql = clsComun.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@idArticulo", pId_Articulo);
        giSql.AgregarParametro("@sDescripcion", psDescripcion);
        giSql.AgregarParametro("@sMedida", psMedida);
        giSql.AgregarParametro("@nPrecio", pPrecio);
        if (!(pIva == null))
        giSql.AgregarParametro("@nIva", pIva);
        giSql.AgregarParametro("@nIeps", pIeps);
        giSql.AgregarParametro("@nIsr", pIsr);
        giSql.AgregarParametro("@nIvaRetenido", pIvaRetenido);
        giSql.AgregarParametro("@sMoneda", psMoneda);
        giSql.AgregarParametro("@sEstatus", psEstatus);
        giSql.AgregarParametro("@nIdEstructura", pIdEstructura);
        giSql.AgregarParametro("@nClave", pnClave);

        Int32 Resultado = Convert.ToInt32(giSql.TraerEscalar("usp_Ctp_Articulo_Ins", true));
        return Resultado;
    }

    public bool fnBajaArticulo(int pId_Articulo)
    {
        giSql = clsComun.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@idArticulo", pId_Articulo);
        giSql.NoQuery("usp_Ctp_Articulo_Del", true);
        return true;
    }

    public DataTable fnObtieneArticulo(int pIdArticulo)
    {
        DataTable dtAuxiliar = new DataTable();
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@idArticulo", pIdArticulo);
        giSql.Query("usp_Ctp_Articulo_Sel", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    public DataTable fnObtieneArticulos()
    {
        DataTable dtAuxiliar = new DataTable();
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.Query("usp_Ctp_Articulo_Sel_All", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    public DataTable fnObtieneArticulosEstructura(int pidEstructura)
    {
        DataTable dtAuxiliar = new DataTable();
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@nIdEstructura", pidEstructura);
        giSql.Query("usp_Ctp_ArticuloEst_Sel", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    /// <summary>
    /// Trae la lista de articulos según la sucursal.
    /// </summary>
    public DataTable fnLlenarGridArticulos(string nId_Estructura, string psClave, string psDescripcion)
    {
        DataTable dtAuxiliar = new DataTable();

        try
        {
            giSql = clsComun.fnCrearConexion("conControl");

            giSql.AgregarParametro("@nIdEstructura", nId_Estructura);

            if (!string.IsNullOrEmpty(psClave))
                giSql.AgregarParametro("@sClave", psClave);

            if (!string.IsNullOrEmpty(psDescripcion))
                giSql.AgregarParametro("@sDescripcion", psDescripcion);

            giSql.Query("usp_Ctp_ArticulosSuc_Sel", true, ref dtAuxiliar);
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion);
        }
      
        return dtAuxiliar;
    }


}
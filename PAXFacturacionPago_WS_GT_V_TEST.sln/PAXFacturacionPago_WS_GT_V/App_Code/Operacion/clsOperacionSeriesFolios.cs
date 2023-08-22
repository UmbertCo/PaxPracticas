using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;

/// <summary>
/// Summary description for clsOperacionSeriesFolios
/// </summary>
public class clsOperacionSeriesFolios
{
    private InterfazSQL giSql;
    private DataTable dtAuxiliar;
    private string conSeries = "conConfiguracion";

    /// <summary>
    /// Devuelve el listado de documentos previamente configurados por el contribuyente
    /// </summary>
    /// <returns></returns>
    public DataTable fnObtenerTiposDocumentos()
    {
        giSql = clsComun.fnCrearConexion(conSeries);
        DataTable dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        giSql.Query("usp_Cfd_Documentos_Asignados_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de series activas para la combinación estructura-documento
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la estructura</param>
    /// <param name="psIdTipoDocumento">Identificador del tipo de documento</param>
    /// <returns>DataTable con la lista de series</returns>
    public DataTable fnObtenerSeries(string psIdEstructura, string psIdTipoDocumento,string nIdUsuario)
    {
        giSql = clsComun.fnCrearConexion(conSeries);
        DataTable dtAuxiliar  = new DataTable();

        if(!string.IsNullOrEmpty(psIdEstructura))
            giSql.AgregarParametro("nId_Estructura", psIdEstructura);

        if(!string.IsNullOrEmpty(psIdTipoDocumento))
            giSql.AgregarParametro("nId_Tipo_Documento", psIdTipoDocumento);

        if (!string.IsNullOrEmpty(nIdUsuario))
            giSql.AgregarParametro("nId_Usuario", nIdUsuario);

        giSql.Query("usp_Cfd_Serie_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Inserta o actualiza una serie para la combinación estructura-documento
    /// </summary>
    /// <param name="psIdEstructura">Identificador de la estructura</param>
    /// <param name="psIdTipoDocumento">Identificador del documento</param>
    /// <param name="psSerie">Nombre de la serie</param>
    /// <param name="psFolio">Número de folio</param>
    /// <returns>Un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnAgregarSerie(string psIdEstructura, string psIdTipoDocumento, string psSerie, string psFolio)
    {
        giSql = clsComun.fnCrearConexion(conSeries);

        giSql.AgregarParametro("nId_Estructura", psIdEstructura);
        giSql.AgregarParametro("nId_Tipo_Documento", psIdTipoDocumento);
        giSql.AgregarParametro("sSerie", psSerie.ToUpper().Trim());
        giSql.AgregarParametro("nFolio", psFolio);

        return giSql.NoQuery("usp_Cfd_Serie_Ins", true);
    }

    /// <summary>
    /// Elimina de manera lógica una serie
    /// </summary>
    /// <param name="poIdSerie">Identificador de la serie</param>
    /// <param name="poSerie">Nombre de la serie</param>
    /// <param name="psIdTipoDocumento">Identificador del documento al que está asociada esta serie</param>
    /// <returns>Un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnEliminarSerie(object poIdSerie, object poSerie, string psIdTipoDocumento)
    {
        giSql = clsComun.fnCrearConexion(conSeries);

        giSql.AgregarParametro("nId_Serie", poIdSerie);
        giSql.AgregarParametro("sSerie", poSerie);
        giSql.AgregarParametro("nId_Tipo_Documento", psIdTipoDocumento);
        giSql.AgregarParametro("nId_Contribuyente", clsComun.fnUsuarioEnSesion().id_contribuyente);

        return giSql.NoQuery("usp_Cfd_Serie_Del", true);
    }
}
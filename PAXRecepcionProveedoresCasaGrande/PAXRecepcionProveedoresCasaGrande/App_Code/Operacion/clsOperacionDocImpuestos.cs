using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;

/// <summary>
/// Clase de capad e negocio para la pantalla webOperacionDocImpuestos
/// </summary>
public class clsOperacionDocImpuestos
{
    private InterfazSQL giSql;
    private DataTable dtAuxiliar;
    private string conDocumentos = "conConfiguracion";

    /// <summary>
    /// Retorna la lista de efectos disponibles
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de efectos disponibles</returns>
    public DataTable fnCargarEfectos()
    {
        giSql = clsComun.fnCrearConexion(conDocumentos);
        DataTable dtAuxiliar = new DataTable();

        giSql.Query("usp_Cfd_Efectos_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de documentos disponibles
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de documentos disponibles</returns>
    public DataTable fnCargarTiposDocumento()
    {
        giSql = clsComun.fnCrearConexion(conDocumentos);
        dtAuxiliar = new DataTable();

        giSql.Query("usp_Cfd_Documentos_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de documentos en general
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de documentos en general</returns>
    public DataTable fnCargarTiposDocumentoGen()
    {
        giSql = clsComun.fnCrearConexion(conDocumentos);
        dtAuxiliar = new DataTable();

        giSql.Query("usp_Cfd_Documentos_General_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de documentos disponibles
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de documentos disponibles</returns>
    public DataTable fnCargarTiposDocumentoPago(int p_idusuario)
    {
        giSql = clsComun.fnCrearConexion(conDocumentos);
        dtAuxiliar = new DataTable();
        giSql.AgregarParametro("@nId_Usuario", p_idusuario);
        giSql.Query("usp_Cfd_Documentos_Sel_Pago", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna una lista con los impuestos disponibles bajo el efecto especificado
    /// </summary>
    /// <param name="psEfecto"></param>
    /// <returns></returns>
    public DataTable fnCargarImpuestos(string psEfecto)
    {
        giSql = clsComun.fnCrearConexion(conDocumentos);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("cEfecto", psEfecto);
        giSql.Query("usp_Cfd_Impuestos_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Retorna la lista de Documentos a los que ya se les ha asignado algún impuesto
    /// </summary>
    /// <returns>Retorna un DataTable con la lista de Documentos a los que ya se les ha asignado algún impuesto</returns>
    public DataTable fnObtenerDocumentosAsignados()
    {
        giSql = clsComun.fnCrearConexion(conDocumentos);
        dtAuxiliar = new DataTable();

        giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        giSql.Query("usp_Cfd_Impuestos_Asignados_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Inserta o Actualiza el registro de la relación documento-impuesto
    /// </summary>
    /// <param name="psIdTipoDocumento">Identificador del documento</param>
    /// <param name="psIdImpuesto">Identificador del impuesto</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnAgregarImpuesto(string psIdTipoDocumento, string psIdImpuesto)
    {
        giSql = clsComun.fnCrearConexion(conDocumentos);

        giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        giSql.AgregarParametro("nId_Tipo_Documento", psIdTipoDocumento);
        giSql.AgregarParametro("nId_Impuesto", psIdImpuesto);

        return giSql.NoQuery("usp_Cfd_Impuestos_Asignados_Ins", true);
    }

    /// <summary>
    /// Elimina de manera lógica el registro de la relación documento-impuesto
    /// </summary>
    /// <param name="psIdTipoDocumento">Identificador del documento</param>
    /// <param name="psIdImpuesto">Identificador del impuesto</param>
    /// <returns>Retorna un entero indicando el número de filas afectadas por la consulta</returns>
    public int fnEliminarImpuesto(object poIdTipoDocumento, object poIdImpuesto)
    {
        giSql = clsComun.fnCrearConexion(conDocumentos);

        giSql.AgregarParametro("nId_Usuario", clsComun.fnUsuarioEnSesion().id_usuario);
        giSql.AgregarParametro("nId_Tipo_Documento", poIdTipoDocumento);
        giSql.AgregarParametro("nId_Impuesto", poIdImpuesto);

        return giSql.NoQuery("usp_Cfd_Impuestos_Asignados_Del", true);
    }
}
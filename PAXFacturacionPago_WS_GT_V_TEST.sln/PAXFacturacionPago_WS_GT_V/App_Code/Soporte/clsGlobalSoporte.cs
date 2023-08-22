using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data;
using Utilerias.SQL;

/// <summary>
/// Clase de capa de negocio para la pantalla webGlobalSoporte
/// </summary>
public class clsGlobalSoporte
{
    private InterfazSQL giSql;

    /// <summary>
    /// Obtiene la lista de tipos de incidentes disponibles
    /// </summary>
    /// <returns>DataTable con la lista de tipos de incidentes</returns>
    public DataTable fnCargarAsuntos()
    {
        DataTable dtAsuntos = new System.Data.DataTable();
        giSql = clsComun.fnCrearConexion("conControl");

        giSql.Query("usp_Ctp_Categorias_Sel", true, ref dtAsuntos);

        return dtAsuntos;
    }

    /// <summary>
    /// Guarda en la base de datos el registro del incidente
    /// </summary>
    /// <param name="psIdCategoria">Identificador de la categoría general a la que pertenece el incidente</param>
    /// <param name="psDescripción">Descripción del incidente sucedido</param>
    ///  /// <param name="psIdUsuarioSop">identificador del usuario que atendera el incidente</param>
    /// <returns>Devuelve una cadena con el número de ticket a ocho caracteres</returns>
    public string fnEnviarTicket(string psIdCategoria, string psDescripción, int psIdUsuarioSop, string psRuta, int psIdUsuario, int pIdRelacion)
    {
        giSql = clsComun.fnCrearConexion("conControl");

        try
        {
            string nEmail = clsComun.fnUsuarioEnSesion().email;
            string sTicket = clsGeneraLlaves.GenerarTicket();

            giSql.AgregarParametro("nId_Usuario", psIdUsuario);
            giSql.AgregarParametro("nTipo_Incidente", psIdCategoria);
            giSql.AgregarParametro("sDescripcion", psDescripción);
            giSql.AgregarParametro("sTicket", sTicket);
            giSql.AgregarParametro("nid_Usuario_Sop", psIdUsuarioSop);
            giSql.AgregarParametro("@nId_Relacion", pIdRelacion);

            int retVal = giSql.NoQuery("usp_Ctp_Incidente_Ins", true);

            if (retVal != 0)
            {
                fnEnviarNotificacion(sTicket, psIdCategoria, psRuta, pIdRelacion);
                return sTicket;
            }
            else
                throw new Exception("No se inserto ningún registro");
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.BaseDatos);
            return string.Empty;
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
            return string.Empty;
        }
    }

    /// <summary>
    /// Envía un correo electrónico tanto al usuario que levanto el ticket como al área de soporte
    /// El correo contiene la información relacionada al incidente
    /// </summary>
    /// <param name="psTicket">Número de ticket generado</param>
    /// <param name="psIdCategoria">Identificador de la categoría a la que pertenece le incidente</param>
    private void fnEnviarNotificacion(string psTicket, string psIdCategoria, string psRuta, int psIdRelacion)
    {
        clsGeneraEMAIL email = new clsGeneraEMAIL();
        giSql = clsComun.fnCrearConexion("conControl");
        DataTable dtAuxiliar;
        dtAuxiliar = new DataTable();
        giSql.AgregarParametro("@sTicket", psTicket);
        giSql.AgregarParametro("@nIdRelacion", psIdRelacion);
         
        giSql.Query("usp_Ctp_Usuario_Reporta_Incidente", true, ref dtAuxiliar);

        email.EnviarCorreoticket(clsComun.fnUsuarioEnSesion().email + ", " + dtAuxiliar.Rows[0]["email"].ToString(),
                            string.Format(Resources.resCorpusCFDIEs.varTicketSubject, psTicket, clsComun.fnUsuarioEnSesion().userName),
                            string.Format(Resources.resCorpusCFDIEs.varTicketMailBody1, psTicket, dtAuxiliar.Rows[0]["nombre"].ToString()), psRuta);
    }

    /// <summary>
    /// Obtiene el identificador del usuario de soporte que atendera el tipo de incidencia
    /// </summary>
    /// <param name="psTipoIncidente">Identificador del tipo de incidente</param>
    public DataSet fnObtieneUsuarioSoporte(int psTipoIncidente)
    {
        DataSet dtAuxiliar;
        dtAuxiliar = new DataSet();
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@nIncidencia", psTipoIncidente);
        giSql.Query("usp_Ctp_Incidente_Encargado_Sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// Obtiene todas las incidencias asignadas al usuario activo
    /// </summary>
    /// <param name="nId_Usuario">Identificador del usuario activo</param>
    public DataTable fnObtieneIncidenciasporUsuario(int psIdUsuario)
    {
        DataTable dtAuxiliar;
        giSql = clsComun.fnCrearConexion("conControl");
        dtAuxiliar = new DataTable();
        giSql.AgregarParametro("@nId_Usuario", psIdUsuario);
        giSql.Query("usp_Ctp_Incidencias_Usuario_sel", true, ref dtAuxiliar);

        return dtAuxiliar;
    }

    /// <summary>
    /// verifica la extension del archivo a enviar por correo
    /// 
    /// </summary>
    /// <param name="psArchivo">Nombre del archivo</param>
    public bool fnverificaarchivo(string psArchivo)
    {
        bool valor = false;
        try
        {
            string[] psExtension = null;
            string Extensiones = clsComun.ObtenerParamentro("ExtInc");
            string[] Extension = Extensiones.Split(',');
            psExtension = psArchivo.Split('.');
            string Ext = null;
            Ext = psExtension[1];
            foreach (string ExVal in Extension)
            {
                if (ExVal.Trim() == Ext)
                {
                    valor = true;
                    return valor;                    
                }             
            }
       
        }
        catch (Exception ex)
        {
        }
        return valor;
    }

    /// <summary>
    /// verifica el tamaño del archivo
    /// 
    /// </summary>
    /// <param name="psArchivo">tamaño del archivo en KB</param>
    public bool fnVerificaTamanioMax(int psTamanio)
    {
        bool valor = false;
        try
        {

            int psMaximo = Convert.ToInt32(clsComun.ObtenerParamentro("MaxFile"));

            if (psTamanio <= psMaximo)
            {
                valor = true;
                return valor;
            }
            else
            {
                valor = false;
                return valor;
            }
        }
        catch (Exception ex)
        {

        }
        return valor;
    }
}
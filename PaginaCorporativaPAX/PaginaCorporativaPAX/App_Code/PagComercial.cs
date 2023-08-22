using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;
/// <summary>
/// Summary description for PagComercial
/// </summary>
public class PagComercial
{

    private InterfazSQL giSql;
    private string conCuenta = "conControl";


    public DataTable fnPreguntasFrecuentes()
    {
        DataTable dtAuxiliar;
        giSql = Common.fnCrearConexion(conCuenta);
        dtAuxiliar = new DataTable();
        giSql.Query("usp_Ctp_Preguntas_Frecuentes_sel", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    public DataTable fnPreguntasFrecuentesIdPregunta(int idPregunta)
    {
        DataTable dtAuxiliar;
        giSql = Common.fnCrearConexion(conCuenta);
        dtAuxiliar = new DataTable();
        giSql.AgregarParametro("@nId_Pregunta", idPregunta);
        giSql.Query("usp_Ctp_Preguntas_Frecuentes", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    public void fnInsertaNoticia(string sNoticia)
    {

        giSql = Common.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@sNoticia", sNoticia);
        giSql.NoQuery("usp_Ctp_Noticias_Ins", true);

    }

    public DataTable fnSeleccionaNoticia()
    {
        DataTable dtAuxiliar = new DataTable();
        giSql = Common.fnCrearConexion("conControl");
        giSql.Query("usp_Ctp_Noticias_Sel", true, ref dtAuxiliar);
        return dtAuxiliar;
    }


    public DataTable fnObtieneUsuarioSoporte(string sUsuario)
    {
        DataTable dtAuxiliar;
        dtAuxiliar = new DataTable();
        giSql = Common.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@sClaveUsuario", sUsuario);
        giSql.Query("usp_InicioSesion_UsuarioPaginaComercial", true, ref dtAuxiliar);
        return dtAuxiliar;
    }



    public DataTable fnObtieneUsuarioSoporteLogin(int nIdUsuario, string componente)
    {
        DataTable dtAuxiliar;
        dtAuxiliar = new DataTable();
        giSql = Common.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@nIdUsuario", nIdUsuario);
        giSql.AgregarParametro("@sComponente", componente);
        giSql.Query("usp_InicioSesion_UsuarioPaginaComercial_Sel", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    public void fnInsertaPreguntaFrecuenta(string sPregunta, string sRespuesta)
    {

        giSql = Common.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@sPregunta", sPregunta);
        giSql.AgregarParametro("@sRespuesta", sRespuesta);
        giSql.NoQuery("usp_Ctp_PreguntasFreq_Ins", true);

    }

    public void fnDelPreguntaFrecuente(int nIdPregunta)
    {
        giSql = Common.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@nId_Pregunta", nIdPregunta);
        giSql.NoQuery("usp_Ctp_Preguntas_Frecuentes_del", true);
    }

    public void fnUpdPreguntaFrecuente(int nIdPregunta, string sPregunta, string sRespuesta)
    {
        giSql = Common.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@nIdPregunta", nIdPregunta);
        giSql.AgregarParametro("@sPregunta", sPregunta);
        giSql.AgregarParametro("@sRespuesta", sRespuesta);
        giSql.NoQuery("usp_Ctp_PreguntasFreq_upd", true);
    }


    public void fnUpdNoticia(int nIdNoticia, string sDescripcion)
    {
        giSql = Common.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@idNoticia", nIdNoticia);
        giSql.AgregarParametro("@sNoticia", sDescripcion);
        giSql.NoQuery("usp_Ctp_Noticias_upd", true);
    }


    public void fnDelNoticia(int nIdNoticia)
    {
        giSql = Common.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@idNoticia", nIdNoticia);
        giSql.NoQuery("usp_Ctp_Noticias_del", true);
    }


    public DataTable fnSelNoticia()
    {
        DataTable dtAuxiliar;
        dtAuxiliar = new DataTable();
        giSql = Common.fnCrearConexion(conCuenta);
        giSql.Query("usp_Ctp_Noticias", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    public DataTable fnSeleccionaNoticia(int nIdNoticia)
    {
        DataTable dtAuxiliar;
        dtAuxiliar = new DataTable();
        giSql = Common.fnCrearConexion(conCuenta);
        giSql.AgregarParametro("@nIdNoticia", nIdNoticia);
        giSql.Query("usp_Ctp_Noticias_Gr", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

}
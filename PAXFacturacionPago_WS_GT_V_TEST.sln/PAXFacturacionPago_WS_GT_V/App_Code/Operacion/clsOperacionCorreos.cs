using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utilerias.SQL;
using System.Data;

/// <summary>
/// Summary description for clsOperacionCorreos
/// </summary>
public class clsOperacionCorreos
{
    private InterfazSQL giSql;
    private string conCuenta = "conControl";
  
    /// <summary>
    ///actualiza el password del usuario
    /// </summary>
    /// <param name="psPassword">password del usuario</param>
    ///  /// <param name="psIdUsuario">identificador del usuario</param>
   
    public bool fnActualizaPasswordUsuario(int psidCorreo, string psPassword, string psEstatus)
    {
        string psPass = Utilerias.Encriptacion.Classica.Encriptar(psPassword);
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@sidCorreo", psidCorreo);
        giSql.AgregarParametro("@sPassword", psPass);
        giSql.AgregarParametro("@sEstatus", psEstatus);
        giSql.NoQuery("usp_Correo_Actualiza_Password", true);
        return true;
    }

    /// <summary>
    ///Recupera todos los correos configurados del sistema
    /// </summary>
    /// <param name="psPassword">password del usuario</param>
    ///  /// <param name="psIdUsuario">identificador del usuario</param>

    public DataTable fnBuscaCorreoConf()
    {
        DataTable dtAuxiliar = new DataTable();
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.Query("usp_Ctp_CorreoProveedores_sel", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    /// <summary>
    ///inserta o actualiza un registro de correo configurado del sistema
    /// </summary>
    /// <param name="psidCorreo">identificador del correo</param>
    /// <param name="psPassword">identificador del password</param>
    /// /// <param name="psPassword">identificador del Contribuyente</param>
    public bool fnCorreoInsUpd(string psidCorreo, string psCorreoE, string psPassword, string psEstatus, int psidContribuyente)
    {
        if (psPassword != null)
        {
            string psPass = Utilerias.Encriptacion.Classica.Encriptar(psPassword);
        }
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@nIdCorreo", psidCorreo);
        giSql.AgregarParametro("@sCorreoElectronico", psCorreoE);
        giSql.AgregarParametro("@sPassword", psPassword);
        giSql.AgregarParametro("@sEstatus", psEstatus);
        giSql.AgregarParametro("@nIdContribuyente", psidContribuyente);
        giSql.NoQuery("usp_Ctp_CorreoProveedores_ins", true);
        return true;
    }
    
    /// <summary>
    ///recupera la informacion del rfc en especifico
    /// </summary>
    /// <param name="sRFC">identificador del RFC</param>
    public DataTable ObtenerInfoContribuyenteRFC(string sRFC)
    {
        DataTable dtAuxiliar = new DataTable();
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@sRFC", sRFC);
        giSql.Query("usp_Ctp_RFC_sel", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    /// <summary>
    ///recupera la informacion del correo en especifico
    /// </summary>
    /// <param name="sCorreo">identificador del Correo</param>
    public DataTable ObtenerInfoCorreo(int sCorreo)
    {
        DataTable dtAuxiliar = new DataTable();
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@sidCorreo", sCorreo);
        giSql.Query("usp_Correos_sel", true, ref dtAuxiliar);
        return dtAuxiliar;
    }

    /// <summary>
    ///recupera la informacion del rfc en especifico
    /// </summary>
    /// <param name="sCorreo">identificador del contribuyente</param>
    public DataTable ObtenerInfoCorreoContribuyente(int psIdUsuario)
    {
        DataTable dtAuxiliar = new DataTable();
        giSql = clsComun.fnCrearConexion("conConfiguracion");
        giSql.AgregarParametro("@nId_Usuario", psIdUsuario);
        giSql.Query("usp_Con_RCFs_Sel", true, ref dtAuxiliar);
        return dtAuxiliar;
    }


    /// <summary>
    ///cambia el estatus a 'baja' de un correo 
    /// </summary>
    /// <param name="psidCorreo">identificador del correo</param>
    public bool fnBajaCorreo(int psidCorreo)
    {
        giSql = clsComun.fnCrearConexion("conControl");
        giSql.AgregarParametro("@idCorreo", psidCorreo);
        giSql.NoQuery("usp_Ctp_CorreoProveedores_Del", true);
        return true;
    }
}
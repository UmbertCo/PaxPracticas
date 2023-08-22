using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsInicioSesionUsuario
/// </summary>
public class clsInicioSesionUsuario
{
    public int nIdUsuario { get; set; }
    public char sEstatus { get; set; }
    public int nIdContribuyente { get; set; }
    public string sUserName { get; set; }
    public string sEmail { get; set; }
    public string sRol { get; set; }
    public string sLenguaje { get; set; }
    public string sRfc { get; set; }
    public char sSistemaOrigen { get; set; }
    public string sVersion { get; set; }
    public int nPlantilla { get; set; }
    public string sColor { get; set; }
    public int nIdRfc { get; set; }

    /// <summary>
    /// Encargado de refrescar los valores agregados o modificados a las sesion.
    /// </summary>
    public void Actualizar()
    {
        System.Web.HttpContext.Current.Session.Add("objUsuario", this);
    }
}
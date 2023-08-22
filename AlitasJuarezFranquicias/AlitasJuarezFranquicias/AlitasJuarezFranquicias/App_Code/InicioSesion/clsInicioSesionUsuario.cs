using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Clase encargada de mantener y manipular la información del usuario en sesión
/// </summary>
public class clsInicioSesionUsuario
{
    public int      id_usuario          { get; set; }
    public char     estatus             { get; set; }
    public int      id_contribuyente    { get; set; }
    public string   userName            { get; set; }
    public string   email               { get; set; }
    public string   rol                 { get; set; }
    public string   lenguaje            { get; set; }
    public string   rfc                 { get; set; }
    public char     sistema_origen      { get; set; }
    public string   version             { get; set; }
    public int      plantilla           { get; set; }
    public string   color               { get; set; }
    public int      id_rfc              { get; set; }
    
    /// <summary>
    /// Encargado de refrescar los valores agregados o modificados a las sesion.
    /// </summary>
    public void Actualizar()
    {
        System.Web.HttpContext.Current.Session.Add("objUsuario",this);
    }
}
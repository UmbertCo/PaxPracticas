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

    /// <summary>
    /// Encargado de refrescar los valores agregados o modificados a las sesion.
    /// </summary>
    public void Actualizar()
    {
        System.Web.HttpContext.Current.Session.Add("objUsuario",this);
    }
}
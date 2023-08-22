using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class clsResultadoValidacion
{
    public bool valido { get; set; }
    public string mensaje { get; set; }
    public string codigo { get; set; }
    /// <summary>
    /// Crea una nueva instancia
    /// </summary>
    /// <param name="bValido">Indica si el comprobante es valido</param>
    /// <param name="sMensaje">Mensaje que regreso el webService</param>
    /// <param name="nCodigo">Codigo del error regreado por el webService</param>
    public clsResultadoValidacion(bool bValido, string sMensaje, string nCodigo)
    {
        valido = bValido;
        mensaje = sMensaje;
        codigo = nCodigo;
    }
}

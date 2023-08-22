using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class Divisas
{
    // Crear Complemento Divisas (VERSION 1.0)
    public static void fnCrearComplementoDivisas10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosDivisas)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "Divisas", atributosDivisas, "divisas", "http://www.sat.gob.mx/divisas"));
    }
}

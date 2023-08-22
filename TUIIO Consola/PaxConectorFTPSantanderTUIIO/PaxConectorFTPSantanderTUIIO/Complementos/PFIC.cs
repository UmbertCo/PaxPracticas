using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class PFIC
{
    // Crear Complemento Persona Fisica Integrante Coordinado (VERSION 1.0)
    public static void fnCrearComplementoPFIC10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosPFIC)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "PFintegranteCoordinado", atributosPFIC, "pfic", "http://www.sat.gob.mx/pfic"));
    }
}

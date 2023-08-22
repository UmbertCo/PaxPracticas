using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class ObrasArte
{
    // Crear Elemento Obras Arte y Antiguedades Version 1.0
    public static void fnCrearComplementoObrasArte10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosObrasArte)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "obrasarteantiguedades", atributosObrasArte, "obrasarte", "http://www.sat.gob.mx/arteantiguedades"));
    }
}

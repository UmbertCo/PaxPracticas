using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class Donatarias
{
    // Crear Nodo Complemento Donatarias VERSION 1.1
    public static void fnCrearComplementoDonatarias11(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosDonatarias)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "Donatarias", atributosDonatarias, "donat", "http://www.sat.gob.mx/donat"));
    }
}
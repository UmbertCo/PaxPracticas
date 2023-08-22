using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class PagoenEspecie
{
    // Crear Elemento Pago en Especie (VERSION 1.0)
    public static void fnCrearComplementoPagoenEspecie10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosPagoenEspecie)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "PagoEnEspecie", atributosPagoenEspecie, "pagoenespecie", "http://www.sat.gob.mx/pagoenespecie"));
    }
}
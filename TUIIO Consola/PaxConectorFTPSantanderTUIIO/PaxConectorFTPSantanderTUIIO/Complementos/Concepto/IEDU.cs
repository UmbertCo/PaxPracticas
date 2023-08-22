using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

class IEDU
{
    // Crear Elemento IEDU Version 1.0
    public static void fnCrearComplementoIEDU10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, List<clsAtributos33> atributosIEDU)
    {
        foreach (var item in atributosIEDU)
        {
            XmlNode Conceptos = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);
            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];

            if (ComplementoConcepto.ChildNodes.Count != 0)
            {
                Conceptos.ChildNodes[item.concepto].ChildNodes[0].AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "instEducativas", item.atributos, "iedu", "http://www.sat.gob.mx/iedu"));
            }
            else
            {
                ComplementoConcepto = Conceptos.ChildNodes[item.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "ComplementoConcepto", "cfdi", "http://www.sat.gob.mx/cfd/3"));
                ComplementoConcepto.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "instEducativas", item.atributos, "iedu", "http://www.sat.gob.mx/iedu"));
            }
        }
    }
}

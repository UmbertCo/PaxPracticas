using System;
using System.Web;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

class Pagos10
{
    public static void fnCrearComplementoPagos10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] AtributosPagos, List<String[]> AtributosPago, List<clsAtributos33> AtributosDocTo, List<clsAtributos33> AtributosPagosImpuestos, List<clsAtributosPTE33> AtributosPagosRetenciones, List<clsAtributosPTE33> AtributosPagosTraslados)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "Pagos", AtributosPagos, "pago10", "http://www.sat.gob.mx/Pagos"));        

        foreach (var pago in AtributosPago)
        {
            complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Pago", pago, "pago10", "http://www.sat.gob.mx/Pagos"));
        }

        XmlNode Conceptos = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/pago10:Pagos", nsm);

        foreach (var DocTo in AtributosDocTo)
        {
            Conceptos.ChildNodes[DocTo.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DoctoRelacionado", DocTo.atributos, "pago10", "http://www.sat.gob.mx/Pagos"));
        }

        foreach (var Impuesto in AtributosPagosImpuestos)
        {          
            XmlNode NodeImpuestos = Conceptos.ChildNodes[Impuesto.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Impuestos", Impuesto.atributos, "pago10", "http://www.sat.gob.mx/Pagos"));
        }

        foreach (var item in AtributosPagosRetenciones)
        {
            XmlNodeList instructions = Conceptos.ChildNodes[item.concepto].SelectNodes("pago10:Impuestos", nsm);

            XmlNode NodeRetenciones = instructions[item.PTE].SelectSingleNode("pago10:Retenciones",nsm);

            if (NodeRetenciones == null)
            {
                NodeRetenciones = instructions[item.PTE].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Retenciones", "pago10", "http://www.sat.gob.mx/Pagos"));
            }

            NodeRetenciones.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Retencion", item.atributos, "pago10", "http://www.sat.gob.mx/Pagos"));
        }

        foreach (var item in AtributosPagosTraslados)
        {
            XmlNodeList instructions = Conceptos.ChildNodes[item.concepto].SelectNodes("pago10:Impuestos", nsm);
            XmlNode NodeTraslados = instructions[item.PTE].SelectSingleNode("pago10:Traslados", nsm);

            if (NodeTraslados == null)
            {
               NodeTraslados = instructions[item.PTE].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Traslados", "pago10", "http://www.sat.gob.mx/Pagos"));
            }
                NodeTraslados.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Traslado", item.atributos, "pago10", "http://www.sat.gob.mx/Pagos"));
        }
    }
}
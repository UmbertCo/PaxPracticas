using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class Terceros
{
    // Crear Elemento Terceros  Version 1.1
    public static void fnCrearComplementoTerceros11(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, List<clsAtributos33> atributosTerceros, List<clsAtributos33> atributosTercerosAdu, List<clsAtributos33> atributosTercerosInfo, List<clsAtributos33> atributosTercerosParte, List<clsAtributos33> atributosTercerosPredial, List<clsAtributos33> atributosTercerosTraslados, List<clsAtributos33> atributosTercerosRetenciones, List<clsAtributosPTE33> atributosTercerosPTEInfoAdu)
    {
        XmlNode Conceptos = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);

        foreach (var item in atributosTerceros)
        {

            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            if (ComplementoConcepto.ChildNodes.Count != 0)
            {
                XmlNode Impuestos = Conceptos.ChildNodes[item.concepto].ChildNodes[0].AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "PorCuentadeTerceros", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
            }
            else
            {
                ComplementoConcepto = Conceptos.ChildNodes[item.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "ComplementoConcepto", "cfdi", "http://www.sat.gob.mx/cfd/3"));
                XmlNode Impuestos = ComplementoConcepto.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "PorCuentadeTerceros", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
            }
        }

        nsm.AddNamespace("terceros", "http://www.sat.gob.mx/terceros");

        foreach (var item in atributosTercerosInfo)
        {

            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNode Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros", nsm);
            Node.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "InformacionFiscalTercero", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
        }

        foreach (var item in atributosTercerosAdu)
        {

            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNode Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros", nsm);
            Node.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "InformacionAduanera", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
        }

        foreach (var item in atributosTercerosParte)
        {

            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNode Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros", nsm);
            Node.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Parte", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
        }

        foreach (var item in atributosTercerosPredial)
        {

            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNode Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros", nsm);
            Node.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "CuentaPredial", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
        }

        foreach (var item in atributosTercerosPTEInfoAdu)
        {
            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNodeList instructions = Conceptos.SelectNodes("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto/cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros/terceros:Parte", nsm);
            instructions[item.PTE].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "InformacionAduanera", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
        }

        foreach (var item in atributosTercerosRetenciones)
        {
            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNode Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros/terceros:Impuestos/terceros:Retenciones", nsm);

            if (Node != null)
            {
                Node.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Retencion", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
            }
            else
            {
                Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros", nsm)
                    .AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Impuestos", "terceros", "http://www.sat.gob.mx/terceros"))
                    .AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Retenciones", "terceros", "http://www.sat.gob.mx/terceros"))
                    .AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Retencion", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
            }
        }

        foreach (var item in atributosTercerosTraslados)
        {
            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNode Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros/terceros:Impuestos/terceros:Traslados", nsm);

            if (Node != null)
            {
                Node.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Traslado", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
            }
            else
            {
                Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros/terceros:Impuestos", nsm)
                    .AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Traslados", "terceros", "http://www.sat.gob.mx/terceros"))
                    .AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Traslado", item.atributos, "terceros", "http://www.sat.gob.mx/terceros"));
            }
        }
    }
}
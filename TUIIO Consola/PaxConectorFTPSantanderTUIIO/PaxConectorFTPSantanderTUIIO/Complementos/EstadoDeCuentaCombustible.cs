using System;
using System.Web;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

class EstadoDeCuentaCombustible
{
    // Crear Nodo Complemento Estado de Cuenta Combustibles VERSION 1.1
    public static void fnCrearComplementoEdoCuentaCombustible11(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosECC, List<String[]> atributosECCConceptos, List<clsAtributos33> atributosECCConceptosTraslado)
    {
        nsm.AddNamespace("ecc11", "http://www.sat.gob.mx/EstadoDeCuentaCombustible");

        XmlNode complemento =
            padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "EstadoDeCuentaCombustible", atributosECC, "ecc11", "http://www.sat.gob.mx/EstadoDeCuentaCombustible"));

        XmlNode Conceptos =
            complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Conceptos", "ecc11", "http://www.sat.gob.mx/EstadoDeCuentaCombustible"));

        foreach (var concepto in atributosECCConceptos)
        {
            Conceptos.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "ConceptoEstadoDeCuentaCombustible", concepto, "ecc11", "http://www.sat.gob.mx/EstadoDeCuentaCombustible"))
                .AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Traslados", "ecc11", "http://www.sat.gob.mx/EstadoDeCuentaCombustible"));
        }

        XmlNode ConceptosECC = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/ecc11:EstadoDeCuentaCombustible/ecc11:Conceptos", nsm);

        foreach (var traslados in atributosECCConceptosTraslado)
        {
            ConceptosECC.ChildNodes[traslados.concepto].SelectSingleNode("./ecc11:Traslados", nsm)
                .AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Traslado", traslados.atributos, "ecc11", "http://www.sat.gob.mx/EstadoDeCuentaCombustible"));
        }
    }
}
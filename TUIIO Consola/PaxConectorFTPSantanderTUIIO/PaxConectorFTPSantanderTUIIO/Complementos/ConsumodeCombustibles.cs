using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class ConsumodeCombustibles
{
    // Crear Complemento Consumo de Combustibles Version 1.0
    public static void fnCrearComplementoConsumodeCombustibles10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosConsumoCombustibles, List<String[]> atributosConsumoCombustiblesConceptos, List<clsAtributos33> atributosConsumoCombustiblesDeterminados)
    {
        nsm.AddNamespace("consumodecombustibles", "http://www.sat.gob.mx/consumodecombustibles");

        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "ConsumoDeCombustibles", atributosConsumoCombustibles, "consumodecombustibles", "http://www.sat.gob.mx/consumodecombustibles"));

        XmlNode Conceptos = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Conceptos", "consumodecombustibles", "http://www.sat.gob.mx/consumodecombustibles"));

        foreach (var concepto in atributosConsumoCombustiblesConceptos)
        {
            Conceptos.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "ConceptoConsumoDeCombustibles", concepto, "consumodecombustibles", "http://www.sat.gob.mx/consumodecombustibles"))
                     .AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Determinados", "consumodecombustibles", "http://www.sat.gob.mx/consumodecombustibles"));
        }

        Conceptos = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/consumodecombustibles:ConsumoDeCombustibles/consumodecombustibles:Conceptos", nsm);

        foreach (var traslados in atributosConsumoCombustiblesDeterminados)
        {
            Conceptos.ChildNodes[traslados.concepto].SelectSingleNode("./consumodecombustibles:Determinados", nsm)
                .AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Determinado", traslados.atributos, "consumodecombustibles", "http://www.sat.gob.mx/consumodecombustibles"));
        }
    }
}

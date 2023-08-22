using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

class ValesdeDespensa
{
    // Crear Elemento Vales de Despensa Version 1.0
    public static void fnCrearComplementoValesdeDespensa10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosValesDespensa, List<String[]> atributosValesDespensaConceptos)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "ValesDeDespensa", atributosValesDespensa, "valesdedespensa", "http://www.sat.gob.mx/valesdedespensa"));

        complemento = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Conceptos", "valesdedespensa", "http://www.sat.gob.mx/valesdedespensa"));

        foreach (var Concepto in atributosValesDespensaConceptos)
        {
            complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Concepto", Concepto, "valesdedespensa", "http://www.sat.gob.mx/valesdedespensa"));
        }
    }
}
using System;
using System.Web;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

class INE
{
    // Crear Nodo Complemento Estado de Cuenta Combustibles VERSION 1.1
    public static void fnCrearComplementoINE11(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosINE, List<String[]> atributosINEConceptos, List<clsAtributos33> atributosINEContabilidad)
    {
        nsm.AddNamespace("ine", "http://www.sat.gob.mx/ine");

        XmlNode complemento =
            padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "INE", atributosINE, "ine", "http://www.sat.gob.mx/ine"));        

        foreach (var concepto in atributosINEConceptos)
        {
            complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Entidad", concepto, "ine", "http://www.sat.gob.mx/ine"));                
        }

        XmlNode ConceptosINE = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/ine:INE", nsm);

        foreach (var traslados in atributosINEContabilidad)
        {
            ConceptosINE.ChildNodes[traslados.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Contabilidad", traslados.atributos, "ine", "http://www.sat.gob.mx/ine"));
        }
    }
}
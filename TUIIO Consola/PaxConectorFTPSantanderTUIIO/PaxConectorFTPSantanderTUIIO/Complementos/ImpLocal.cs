using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class ImpLocal
{
    // Crear Complemento Impuestos Locales Version 1.0
    public static void fnCrearComplementoImpLocal10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosImpLocal, List<String[]> atributosImpLocalRetenciones, List<String[]> atributosImpLocalTraslados)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "ImpuestosLocales", atributosImpLocal, "implocal", "http://www.sat.gob.mx/implocal"));

        foreach (var Retencion in atributosImpLocalRetenciones)
        {
            complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "RetencionesLocales", Retencion, "implocal", "http://www.sat.gob.mx/implocal"));
        }

        foreach (var Traslado in atributosImpLocalTraslados)
        {
            complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "TrasladosLocales", Traslado, "implocal", "http://www.sat.gob.mx/implocal"));
        }
    }
}

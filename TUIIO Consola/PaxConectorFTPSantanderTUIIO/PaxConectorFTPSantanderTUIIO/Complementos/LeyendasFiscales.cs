using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class LeyendasFiscales
{
    // Crear Elemento Leyendas Fiscales Version 1.0
    public static void fnCrearComplementoLeyendasFiscales10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosLeyFisc, List<String[]> atributosLeyFiscLeyenda)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "LeyendasFiscales", atributosLeyFisc, "leyendasFisc", "http://www.sat.gob.mx/leyendasFiscales"));

        foreach (var Leyenda in atributosLeyFiscLeyenda)
        {
            complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Leyenda", Leyenda, "leyendasFisc", "http://www.sat.gob.mx/leyendasFiscales"));
        }
    }
}
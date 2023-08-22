using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class VentaVehicular
{
    // Crear Elemento VentaVehicular  Version 1.1
    public static XmlNode fnCrearComplementoVentaVehicular11(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, List<clsAtributos33> atributosVentaVeh, List<clsAtributos33> atributosVentaVehParte, List<clsAtributos33> atributosVentaVehInfoAdu, List<clsAtributosPTE33> atributosVentaVehPTEInfoAdu)
    {

        XmlNode Conceptos = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);

        foreach (var item in atributosVentaVeh)
        {

            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            if (ComplementoConcepto.ChildNodes.Count != 0)
            {
                Conceptos.ChildNodes[item.concepto].ChildNodes[0].AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "VentaVehiculos", item.atributos, "ventavehiculos", "http://www.sat.gob.mx/ventavehiculos"));
            }
            else
            {
                ComplementoConcepto = Conceptos.ChildNodes[item.concepto].AppendChild(xDocumento.CreateElement("cfdi", "ComplementoConcepto", "http://www.sat.gob.mx/cfd/3"));
                ComplementoConcepto.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "VentaVehiculos", item.atributos, "ventavehiculos", "http://www.sat.gob.mx/ventavehiculos"));
            }
        }

        nsm.AddNamespace("ventavehiculos", "http://www.sat.gob.mx/ventavehiculos");

        foreach (var item in atributosVentaVehInfoAdu)
        {

            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNode Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/ventavehiculos:VentaVehiculos", nsm);
            Node.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "InformacionAduanera", item.atributos, "ventavehiculos", "http://www.sat.gob.mx/ventavehiculos"));
        }

        foreach (var item in atributosVentaVehParte)
        {

            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNode Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/ventavehiculos:VentaVehiculos", nsm);
            Node.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Parte", item.atributos, "ventavehiculos", "http://www.sat.gob.mx/ventavehiculos"));
        }

        foreach (var item in atributosVentaVehPTEInfoAdu)
        {

            XmlNode ComplementoConcepto = Conceptos.ChildNodes[item.concepto];
            XmlNode Node = ComplementoConcepto.SelectSingleNode("./cfdi:ComplementoConcepto/ventavehiculos:VentaVehiculos", nsm);
            Node.ChildNodes[item.PTE].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "InformacionAduanera", item.atributos, "ventavehiculos", "http://www.sat.gob.mx/ventavehiculos"));
        }

        return xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsm);
    }
}

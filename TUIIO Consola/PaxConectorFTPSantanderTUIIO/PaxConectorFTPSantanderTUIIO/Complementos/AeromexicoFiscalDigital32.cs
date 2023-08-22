using System;
using System.Web;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

public class AeromexicoFiscalDigital32
{

    public static void fnCrearElementoRootComplemento32(XmlDocument pxDoc, List<String> pasAtributos, String tNamespace)
    {
        XmlAttribute xAttr;

        foreach (String a in pasAtributos)
        {
            String[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = valores[1];
            pxDoc.DocumentElement.Attributes.Append(xAttr);
        }

        String[] pspace = { "" };


        if (!(String.IsNullOrEmpty(tNamespace)))
        {
            pspace = tNamespace.Split('@');


            xAttr = pxDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";

            foreach (String item in pspace)
            {
                String[] items = item.Split('|');
                xAttr.Value = xAttr.Value + " " + items[1] + " " + items[2];
                pxDoc.DocumentElement.SetAttribute("xmlns:" + items[0], items[1]);
            }
            pxDoc.DocumentElement.Attributes.Append(xAttr);
        }
        else
        {
            xAttr = pxDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
            pxDoc.DocumentElement.Attributes.Append(xAttr);
            pxDoc.DocumentElement.Attributes.RemoveNamedItem("schemaLocation");
        }
    }

    public static XmlElement fnCrearElemento(XmlDocument pxDoc, String psElemento, List<String> pasAtributos)
    {
        XmlAttribute xAttr;
        XmlElement elemento = pxDoc.CreateElement("cfdi", psElemento, "http://www.sat.gob.mx/cfd/3");

        foreach (String a in pasAtributos)
        {
            String[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = valores[1];
            elemento.Attributes.Append(xAttr);
        } return elemento;
    }

    public static void fnCrearComplementoAerolineas10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, List<String> atributosAerolineas, List<String> atributosAerolineasOtrosCargos, List<List<String>> atributosAerolineasCargo)
    {
        XmlNode complemento = padre.AppendChild(AeromexicoFiscalDigital32.fnCrearElementoComplemento(xDocumento, "Aerolineas", atributosAerolineas, "aerolineas", "http://www.sat.gob.mx/aerolineas"));

        complemento = complemento.AppendChild(AeromexicoFiscalDigital32.fnCrearElemento(xDocumento, "OtrosCargos", atributosAerolineasOtrosCargos, "aerolineas", "http://www.sat.gob.mx/aerolineas"));

        foreach (var cargo in atributosAerolineasCargo)
        {
            complemento.AppendChild(AeromexicoFiscalDigital32.fnCrearElemento(xDocumento, "Cargo", cargo, "aerolineas", "http://www.sat.gob.mx/aerolineas"));
        }
    }

    public static void fnCrearComplementoINE11(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, List<String> atributosComplementoINE, List<List<String>> ListaEntidadesINE, List<clsAtributosAeromexico> atributosINEEntidadesIDContabilidad)
    {
        nsm.AddNamespace("ine", "http://www.sat.gob.mx/ine");
        XmlNode complemento = padre.AppendChild(AeromexicoFiscalDigital32.fnCrearElementoComplemento(xDocumento, "INE", atributosComplementoINE, "ine", "http://www.sat.gob.mx/ine"));

        foreach (var item in ListaEntidadesINE)
        {
            complemento.AppendChild(AeromexicoFiscalDigital32.fnCrearElemento(xDocumento, "Entidad", item, "ine", "http://www.sat.gob.mx/ine"));            
        }

        XmlNodeList Entidades = xDocumento.SelectNodes("/cfdi:Comprobante/cfdi:Complemento/ine:INE/ine:Entidad", nsm);

        foreach (var item in atributosINEEntidadesIDContabilidad)
        {
            foreach(var item2 in item.atributos)
            {
                List<String> idContabilidad = new List<String>(); idContabilidad.Add(item2);
                Entidades[item.concepto].AppendChild(AeromexicoFiscalDigital32.fnCrearElemento(xDocumento, "Contabilidad", idContabilidad, "ine", "http://www.sat.gob.mx/ine"));
            }
        }
    }

    private static XmlElement fnCrearElementoComplemento(XmlDocument pxDoc, String psElemento, List<String> pasAtributos, String preFijo, String NamespaceURI)
    {
        XmlAttribute xAttr;
        XmlElement elemento = pxDoc.CreateElement(preFijo, psElemento, NamespaceURI);

        foreach (String a in pasAtributos)
        {
            String[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = valores[1];
            elemento.Attributes.Append(xAttr);
        } return elemento;
    }

    private static XmlElement fnCrearElemento(XmlDocument pxDoc, String psElemento, List<String> pasAtributos, String prefix, String NSpace)
    {
        XmlAttribute xAttr;
        XmlElement elemento = pxDoc.CreateElement(prefix, psElemento, NSpace);

        foreach (String a in pasAtributos)
        {
            String[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = valores[1];
            elemento.Attributes.Append(xAttr);
        } return elemento;
    }

}
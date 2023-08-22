using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


class Nomina12
{
    public static void fnCrearComplementoNonima33(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosNomina, String[] atributosNominaEmisor, String[] atributosEntidadSNCF, String[] atributosNominaReceptor, List<String[]> atributosNominaSubcontratacion,
        String[] atributosNominaPercepciones, List<String[]> atributosNominaPerc, List<clsAtributos33> atributosNominaPercHorasExtra, List<clsAtributos33> atributosNominaPercAccionesOTitulos, String[] atributosJubilacionPensionRetiro, String[] atributosSeparacionIndemnizacion, String[] atributosNominaDeducciones,
        List<String[]> atributosNominaDeduc, List<String[]> atributosNominaOtroPago, List<clsAtributos33> atributosNominaOtroPagoSubsidio, List<clsAtributos33> atributosNominaOtroPagoCompensacion, List<String[]> atributosNominaIncapacidad)
    {
        XmlNode NodePercs = null;
        XmlNode NodeOtrosPagos = null;
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "Nomina", atributosNomina, "nomina12", "http://www.sat.gob.mx/nomina12"));

        try
        {
            XmlNode NodeEmisor = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Emisor", atributosNominaEmisor, "nomina12", "http://www.sat.gob.mx/nomina12"));
            NodeEmisor.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "EntidadSNCF", atributosEntidadSNCF, "nomina12", "http://www.sat.gob.mx/nomina12"));
        }
        catch
        {

        }

        XmlNode NodeReceptor = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Receptor", atributosNominaReceptor, "nomina12", "http://www.sat.gob.mx/nomina12"));
        foreach (var subcontratacion in atributosNominaSubcontratacion)
        {
            NodeReceptor.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "SubContratacion", subcontratacion, "nomina12", "http://www.sat.gob.mx/nomina12"));
        }

        try
        {
            NodePercs = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Percepciones", atributosNominaPercepciones, "nomina12", "http://www.sat.gob.mx/nomina12"));
            foreach (var Percepcion in atributosNominaPerc)
            {
                NodePercs.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Percepcion", Percepcion, "nomina12", "http://www.sat.gob.mx/nomina12"));
            }
        }
        catch
        {

        }

        try
        {
            XmlNode NodeDeducs = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Deducciones", atributosNominaDeducciones, "nomina12", "http://www.sat.gob.mx/nomina12"));
            foreach (var Deduccion in atributosNominaDeduc)
            {
                NodeDeducs.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Deduccion", Deduccion, "nomina12", "http://www.sat.gob.mx/nomina12"));
            }
        }
        catch
        {

        }

        try
        {
            if (atributosNominaOtroPago.Count != 0)
                NodeOtrosPagos = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "OtrosPagos", "nomina12", "http://www.sat.gob.mx/nomina12"));
            foreach (var OtroPago in atributosNominaOtroPago)
            {
                NodeOtrosPagos.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "OtroPago", OtroPago, "nomina12", "http://www.sat.gob.mx/nomina12"));
            }
        }
        catch
        {

        }

        try
        {
            XmlNode NodeIncaps = null;
            if (atributosNominaIncapacidad.Count != 0)
                NodeIncaps = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Incapacidades", "nomina12", "http://www.sat.gob.mx/nomina12"));
            foreach (var Incapacidad in atributosNominaIncapacidad)
            {
                NodeIncaps.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Incapacidad", Incapacidad, "nomina12", "http://www.sat.gob.mx/nomina12"));
            }
        }
        catch
        {

        }

        try
        {
            NodePercs = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Percepciones", nsm);

            foreach (var horaExtra in atributosNominaPercHorasExtra)
            {
                NodePercs.ChildNodes[horaExtra.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "HorasExtra", horaExtra.atributos, "nomina12", "http://www.sat.gob.mx/nomina12"));
            }

            foreach (var accionOTitulo in atributosNominaPercAccionesOTitulos)
            {
                NodePercs.ChildNodes[accionOTitulo.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "AccionesOTitulos", accionOTitulo.atributos, "nomina12", "http://www.sat.gob.mx/nomina12"));
            }

            try
            {
                NodePercs.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "JubilacionPensionRetiro", atributosJubilacionPensionRetiro, "nomina12", "http://www.sat.gob.mx/nomina12"));
            }
            catch { }
            NodePercs.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "SeparacionIndemnizacion", atributosSeparacionIndemnizacion, "nomina12", "http://www.sat.gob.mx/nomina12"));
        }
        catch
        {

        }

        try
        {
            NodeOtrosPagos = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:OtrosPagos", nsm);
            foreach (var subsidio in atributosNominaOtroPagoSubsidio)
            {
                NodeOtrosPagos.ChildNodes[subsidio.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "SubsidioAlEmpleo", subsidio.atributos, "nomina12", "http://www.sat.gob.mx/nomina12"));
            }

            foreach (var compensacion in atributosNominaOtroPagoCompensacion)
            {
                NodeOtrosPagos.ChildNodes[compensacion.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "CompensacionSaldosAFavor", compensacion.atributos, "nomina12", "http://www.sat.gob.mx/nomina12"));
            }
        }
        catch
        {

        }
    }

    public static XmlElement fnCrearElemento(XmlDocument pxDoc, String psElemento, String preFijo, String NamespaceURI)
    {
        XmlElement elemento = pxDoc.CreateElement(preFijo, psElemento, NamespaceURI);
        return elemento;
    }

    //Funcion para crear el Nodo Raiz dentro del Complemento Nomina Version 1.2
    private static XmlElement fnCrearElementoComplemento(XmlDocument pxDoc, String psElemento, String[] pasAtributos, String preFijo, String NamespaceURI)
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

    //Funcion para crear elementos dentro del nodo Complemento Nomina Version 1.2
    private static XmlElement fnCrearElemento(XmlDocument pxDoc, String psElemento, String[] pasAtributos, String prefix, String NSpace)
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
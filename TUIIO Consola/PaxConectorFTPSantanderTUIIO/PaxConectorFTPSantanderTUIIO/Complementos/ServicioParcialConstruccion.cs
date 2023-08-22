using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class ParcialConstruccion
{
    // Crear Elemento Servicio Parcial Construccion(VERSION 1.0)
    public static void fnCrearComplementoParcialConstruccion10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosParcialConstruccion, String[] atributosInmueble)
    {
        XmlNode Complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "parcialesconstruccion", atributosParcialConstruccion, "servicioparcial", "http://www.sat.gob.mx/servicioparcialconstruccion"));
        Complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Inmueble", atributosInmueble, "servicioparcial", "http://www.sat.gob.mx/servicioparcialconstruccion"));
    }
}
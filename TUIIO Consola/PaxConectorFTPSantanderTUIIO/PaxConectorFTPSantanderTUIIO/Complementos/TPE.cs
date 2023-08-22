using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

class TPE
{
    // Crear Complemento Turista Pasajero Extranjero (VERSION 1.0)
    public static void fnCrearComplementoTPE10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosTPE, String[] atributosTPEDatos)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "TuristaPasajeroExtranjero", atributosTPE, "tpe", "http://www.sat.gob.mx/TuristaPasajeroExtranjero"));
        complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "datosTransito", atributosTPEDatos, "tpe", "http://www.sat.gob.mx/TuristaPasajeroExtranjero"));
    }
}
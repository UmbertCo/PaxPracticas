using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


class Notarios
{
    // Crear Elemento Notarios Publicos Version 1.0
    public static void fnCrearComplementoNotarios10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosNotarios, List<String[]> atributosDescInmueble, String[] atributosDatosOperacion, String[] atributosDatosNotario, String[] atributosDatosEnajenante, List<String[]> atributosEnajenantes, String[] atributosUnEnajenante, String[] atributosDatosAdquiriente, List<String[]> atributosAdquirientes, String[] atributosUnAdquiriente)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "NotariosPublicos", atributosNotarios, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));

        XmlNode DescInmuebles = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DescInmuebles", "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));

        foreach (var DescInmueble in atributosDescInmueble)
        {
            DescInmuebles.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DescInmueble", DescInmueble, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
        }

        complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosOperacion", atributosDatosOperacion, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
        complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosNotario", atributosDatosNotario, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));

        XmlNode DatosEnajenante = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosEnajenante", atributosDatosEnajenante, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
        if (DatosEnajenante.Attributes["CoproSocConyugalE"].Value == "Si")
        {
            XmlNode DatosEnajenantes = DatosEnajenante.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosEnajenantesCopSC", "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
            foreach (var Enajenantes in atributosEnajenantes)
            {
                DatosEnajenantes.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosEnajenanteCopSC", Enajenantes, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
            }
        }
        else if (DatosEnajenante.Attributes["CoproSocConyugalE"].Value == "No")
        {
            DatosEnajenante.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosUnEnajenante", atributosUnEnajenante, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
        }

        XmlNode DatosAdquiriente = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosAdquiriente", atributosDatosAdquiriente, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
        if (DatosAdquiriente.Attributes["CoproSocConyugalE"].Value == "Si")
        {
            XmlNode DatosAdquirientes = DatosAdquiriente.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosAdquirientesCopSC", "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
            foreach (var Adquirientes in atributosAdquirientes)
            {
                DatosAdquirientes.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosAdquirienteCopSC", Adquirientes, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
            }
        }
        else if (DatosAdquiriente.Attributes["CoproSocConyugalE"].Value == "No")
        {
            DatosAdquiriente.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DatosUnAdquiriente", atributosUnAdquiriente, "notariospublicos", "http://www.sat.gob.mx/notariospublicos"));
        }
    }
}

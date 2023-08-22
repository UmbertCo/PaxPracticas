using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class RenovSustVehicular
{
    // Crear Elemento Notarios Publicos Version 1.0
    public static void fnCrearComplementoSustRenov10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosSust, String[] atributosRenov, String[] atributosDecreto, String[] atributosVehNvoEnajFabAlPerm, String[] atributosVehUsadoEnajPermAlFab, List<String[]> atributosVehUsadosEnajPermAlFab)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "renovacionysustitucionvehiculos", atributosDecreto, "decreto", "http://www.sat.gob.mx/renovacionysustitucionvehiculos"));

        if (atributosRenov != null)
        {
            XmlNode Renov = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DecretoRenovVehicular", atributosRenov, "decreto", "http://www.sat.gob.mx/renovacionysustitucionvehiculos"));

            foreach (var VehUsado in atributosVehUsadosEnajPermAlFab)
            {
                Renov.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "VehiculosUsadosEnajenadoPermAlFab", VehUsado, "decreto", "http://www.sat.gob.mx/renovacionysustitucionvehiculos"));
            }

            Renov.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "VehiculoNuvoSemEnajenadoFabAlPerm", atributosVehNvoEnajFabAlPerm, "decreto", "http://www.sat.gob.mx/renovacionysustitucionvehiculos"));
        }

        if (atributosSust != null)
        {
            XmlNode Renov = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DecretoSustitVehicular", atributosSust, "decreto", "http://www.sat.gob.mx/renovacionysustitucionvehiculos"));

            foreach (var VehUsado in atributosVehUsadosEnajPermAlFab)
            {
                Renov.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "VehiculoUsadoEnajenadoPermAlFab", VehUsado, "decreto", "http://www.sat.gob.mx/renovacionysustitucionvehiculos"));
            }

            Renov.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "VehiculoNuvoSemEnajenadoFabAlPerm", atributosVehNvoEnajFabAlPerm, "decreto", "http://www.sat.gob.mx/renovacionysustitucionvehiculos"));
        }
    }

}

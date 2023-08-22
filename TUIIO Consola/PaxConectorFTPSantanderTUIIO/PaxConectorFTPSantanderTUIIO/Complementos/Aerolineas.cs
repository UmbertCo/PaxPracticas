using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class Aerolineas
{
    public static void fnCrearComplementoAerolineas10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosAerolineas, String[] atributosAerolineasOtrosCargos, List<String[]> atributosAerolineasCargo)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "Aerolineas", atributosAerolineas, "aerolineas", "http://www.sat.gob.mx/aerolineas"));

        if (atributosAerolineasOtrosCargos.Count() > 0)
        {
            complemento = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "OtrosCargos", atributosAerolineasOtrosCargos, "aerolineas", "http://www.sat.gob.mx/aerolineas"));

            foreach (var cargo in atributosAerolineasCargo)
            {
                complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Cargo", cargo, "aerolineas", "http://www.sat.gob.mx/aerolineas"));
            }
        }
    }
}
using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class VehiculosUsados
{
    //-- Complemento Vehiculos Usados Version 1.0
    public static void fnCrearComplementoVehiculosUsados10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosVehiculoUsado, List<String[]> atributosInfoAduanera)
    {
        XmlNode complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "VehiculoUsado", atributosVehiculoUsado, "vehiculousado", "http://www.sat.gob.mx/vehiculousado"));

        foreach (var Info in atributosInfoAduanera)
        {
            complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "InformacionAduanera", Info, "vehiculousado", "http://www.sat.gob.mx/vehiculousado"));
        }
    }
}
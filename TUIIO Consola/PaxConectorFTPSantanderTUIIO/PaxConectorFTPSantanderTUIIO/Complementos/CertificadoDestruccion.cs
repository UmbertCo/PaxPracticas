using System;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

class Destruccion
{
    public static void fnCrearComplementoDestruccion10(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosCertDestruccion, String[] atributosVehDestruido, String[] atributosVehDestruidoAduana)
    {
        XmlNode Complemento = padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "certificadodedestruccion", atributosCertDestruccion, "destruccion", "http://www.sat.gob.mx/certificadodestruccion"));
        Complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "VehiculoDestruido", atributosVehDestruido, "destruccion", "http://www.sat.gob.mx/certificadodestruccion"));

        if (atributosVehDestruidoAduana.Count() > 0)
        {
            Complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "InformacionAduanera", atributosVehDestruidoAduana, "destruccion", "http://www.sat.gob.mx/certificadodestruccion"));
        }
    }
}

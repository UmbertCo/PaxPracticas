using System;
using System.Xml;
using System.Web;
using System.Linq;
using System.Collections.Generic;

class ComercioExterior
{
    // Crear Nodo Complemento Estado de Cuenta Combustibles VERSION 1.1
    public static void fnCrearComplementoCCE11(XmlDocument xDocumento, XmlNamespaceManager nsm, XmlNode padre, String[] atributosCCE11, String[] atributosCCE11Emisor, String[] atributosCCE11Domicilio, List<String[]> atributosPropietarios, String[] atributosCCE11Receptor, String[] atributosCCE11ReceptorDomicilio, List<String[]> atributosDestinatarios, List<clsAtributos33> atributosDestinatarioDomicilio, List<String[]> atributosMercancias, List<clsAtributos33> atributosDescripciones)
    {
        nsm.AddNamespace("cce11", "http://www.sat.gob.mx/ComercioExterior11");

        XmlNode complemento =
            padre.AppendChild(ComprobanteFiscalDigital33.fnCrearElementoComplemento(xDocumento, "ComercioExterior", atributosCCE11, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));

        if (atributosCCE11Emisor != null)
        {
            if (atributosCCE11Emisor[0] == "Att@No aplica")
            {
                XmlNode Emisor = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Emisor", "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
                Emisor.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Domicilio", atributosCCE11Domicilio, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
            }
            else
            {
                XmlNode Emisor = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Emisor", atributosCCE11Emisor, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
                if (atributosCCE11Domicilio != null)
                {
                    Emisor.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Domicilio", atributosCCE11Domicilio, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
                }
            }
        }

        foreach (var concepto in atributosPropietarios)
        {
            complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Propietario", concepto, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
        }

        if (atributosCCE11Receptor != null)
        {
            if (atributosCCE11Receptor[0] == "Att@No aplica")
            {
                XmlNode Receptor = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Receptor", "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
                Receptor.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Domicilio", atributosCCE11ReceptorDomicilio, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
            }
            else
            {
                XmlNode Receptor = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Receptor", atributosCCE11Receptor, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));

                if (atributosCCE11ReceptorDomicilio != null)
                {
                    Receptor.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Domicilio", atributosCCE11ReceptorDomicilio, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
                }
            }
        }


        foreach (var concepto in atributosDestinatarios)
        {
            if (concepto[0] == "Att@No aplica")
            {
                complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Destinatario", "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
            }
            else
            {
                complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Destinatario", concepto, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
            }
        }

        XmlNodeList Destinatarios = xDocumento.SelectNodes("/cfdi:Comprobante/cfdi:Complemento/cce11:ComercioExterior/cce11:Destinatario", nsm);

        foreach (var destinatario in atributosDestinatarioDomicilio)
        {
            Destinatarios[destinatario.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Domicilio", destinatario.atributos, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
        }

        if (atributosMercancias != null)
        {
            XmlNode Mercancias = complemento.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Mercancias", "cce11", "http://www.sat.gob.mx/ComercioExterior11"));

            foreach (var concepto in atributosMercancias)
            {
                Mercancias.AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "Mercancia", concepto, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
            }

            XmlNodeList ListMercancias = xDocumento.SelectNodes("/cfdi:Comprobante/cfdi:Complemento/cce11:ComercioExterior/cce11:Mercancias/cce11:Mercancia", nsm);

            foreach (var destinatario in atributosDescripciones)
            {
                ListMercancias[destinatario.concepto].AppendChild(ComprobanteFiscalDigital33.fnCrearElemento(xDocumento, "DescripcionesEspecificas", destinatario.atributos, "cce11", "http://www.sat.gob.mx/ComercioExterior11"));
            }
        }
    }
}
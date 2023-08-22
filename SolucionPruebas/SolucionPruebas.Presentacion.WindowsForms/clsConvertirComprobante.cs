using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public class clsConvertirComprobante
    {
        public XElement fnRemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                {
                    xElement.Add(new XAttribute(attribute.Name.LocalName, attribute.Value));
                }

                return xElement;
            }

            else
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => fnRemoveAllNamespaces(el)));

                foreach (XAttribute attribute in xmlDocument.Attributes())
                {
                    if (!(attribute.Name.LocalName.Equals("xsi") || attribute.Name.LocalName.Equals("xmlns") || attribute.Name.LocalName.Equals("schemaLocation")))
                    {
                        xElement.Add(new XAttribute(attribute.Name.LocalName, attribute.Value));
                    }
                }

                return xElement;
            }
        }
    }
}

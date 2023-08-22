using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace PAXPruebaPlantillas
{
    class Program
    {
        static void Main(string[] args)
        {
            string psColor = string.Empty;
            XmlDocument pxComprobante = new XmlDocument();

            try
            {
                pxComprobante.Load(@"D:\Proyectos\Practicas\PAXPruebaPlantillas\PAXPruebaPlantillas\XML\TercerosInfoAduanera Prueba  30_01_2018.xml");
                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(pxComprobante.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                if (pxComprobante.OuterXml.Contains("http://www.sat.gob.mx/terceros"))
                {
                    //Se toma el comprobante actual
                    nsmComprobante.AddNamespace("terceros", "http://www.sat.gob.mx/terceros");

                    XPathNavigator navComprobante = pxComprobante.CreateNavigator();
                    XPathNodeIterator navDetalles = navComprobante.Select("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsmComprobante);

                    while (navDetalles.MoveNext())
                    {
                        XPathNavigator nodenavigator = navDetalles.Current;

                        if (nodenavigator.HasChildren)//Si contiene nodo hijo
                        {
                            XPathNavigator navComplTerceros = nodenavigator.SelectSingleNode("cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros/terceros:Impuestos", nsmComprobante);

                            if (navComplTerceros != null)
                            {
                                XPathNavigator navComplTercerosRetenciones = navComplTerceros.SelectSingleNode("terceros:Retenciones", nsmComprobante);
                                if (navComplTercerosRetenciones != null)
                                {
                                    RenameNode(((IHasXmlNode)navComplTercerosRetenciones).GetNode(), "terceros:RetencionesTerceros", "terceros");
                                }

                                XPathNavigator navComplTercerosTraslados = navComplTerceros.SelectSingleNode("terceros:Traslados", nsmComprobante);
                                if (navComplTercerosTraslados != null)
                                {
                                    RenameNode(((IHasXmlNode)navComplTercerosTraslados).GetNode(), "terceros:TrasladosTerceros", "terceros");
                                }

                                RenameNode(((IHasXmlNode)navComplTerceros).GetNode(), "terceros:ImpuestosTerceros", "terceros");
                            }

                            XPathNodeIterator navDetallesParte = nodenavigator.Select("cfdi:ComplementoConcepto/terceros:PorCuentadeTerceros/terceros:Parte", nsmComprobante);
                            while (navDetallesParte.MoveNext())
                            {
                                string sCadenaNueva = string.Empty;
                                XPathNavigator nodeNavigatorParte = navDetallesParte.Current;

                                XPathNavigator first = nodeNavigatorParte.SelectSingleNode("terceros:InformacionAduanera[1]", nsmComprobante);

                                if (nodeNavigatorParte.HasChildren && first != null)//Si contiene nodo hijo
                                {
                                    XPathNodeIterator navDetallesParteInfoAduanera = nodeNavigatorParte.SelectDescendants(XPathNodeType.Element, false);

                                    int nNumeroNodos = navDetallesParteInfoAduanera.Count;
                                    
                                    foreach (XPathNavigator nodenavigatorInfo in navDetallesParteInfoAduanera)
                                    {
                                        string sCadena = nodenavigatorInfo.OuterXml;
                                        sCadenaNueva += sCadena.Replace("terceros:InformacionAduanera", "terceros:InformacionAduaneraTerceros");
                                    }

                                    XPathNavigator last = nodeNavigatorParte.SelectSingleNode("terceros:InformacionAduanera[" + nNumeroNodos + "]", nsmComprobante);

                                    nodeNavigatorParte.MoveTo(first);
                                    nodeNavigatorParte.DeleteRange(last);

                                    nodeNavigatorParte.AppendChild(sCadenaNueva);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
            
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public static XmlNode RenameNode(XmlNode e, string newName, string psAlias)
        {
            XmlDocument doc = e.OwnerDocument;
            XmlNode newNode = doc.CreateNode(e.NodeType, newName, "http://www.sat.gob.mx/cfd/3");
            newNode.Prefix = psAlias;
            while (e.HasChildNodes)
            {
                newNode.AppendChild(e.FirstChild);
            }
            XmlAttributeCollection ac = e.Attributes;
            while (ac.Count > 0)
            {
                newNode.Attributes.Append(ac[0]);
            }
            XmlNode parent = e.ParentNode;
            parent.ReplaceChild(newNode, e);
            return newNode;
        }
    }
}

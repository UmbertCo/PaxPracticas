using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmConvertirComprobante22 : Form
    {
        public frmConvertirComprobante22()
        {
            InitializeComponent();
        }

        private void Convertir_Click(object sender, EventArgs e)
        {
            XmlDocument xComprobante = new XmlDocument();
            XmlDocument xDocumento = new XmlDocument();
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            string sComprobante = string.Empty;
            string sRuta = string.Empty;
            try
            {
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName.Equals(string.Empty))
                {
                    return;
                }
                sRuta = openFileDialog1.FileName;
                xComprobante.Load(sRuta);

                clsConvertirComprobante clConvertir = new clsConvertirComprobante();
                XElement xElemento = XElement.Parse(xComprobante.OuterXml);
                XElement xElementoSalida;
                xElementoSalida = clConvertir.fnRemoveAllNamespaces(xElemento);

                xDocumento.CreateXmlDeclaration("1.0", "UTF-8", "no");
                xDocumento.LoadXml(xElementoSalida.ToString());

                xDocumento.ChildNodes[0].Attributes.RemoveNamedItem("noAprobacion");
                xDocumento.ChildNodes[0].Attributes.RemoveNamedItem("anoAprobacion");
                xDocumento.ChildNodes[0].Attributes.RemoveNamedItem("TipoCambio");
                xDocumento.ChildNodes[0].Attributes.RemoveNamedItem("FolioFiscalOrig");
                xDocumento.ChildNodes[0].Attributes.RemoveNamedItem("SerieFolioFiscalOrig");
                xDocumento.ChildNodes[0].Attributes.RemoveNamedItem("FechaFolioFiscalOrig");
                xDocumento.ChildNodes[0].Attributes.RemoveNamedItem("FolioFiscalOrig");
                xDocumento.ChildNodes[0].Attributes.RemoveNamedItem("MontoFolioFiscalOrig");

                XPathNavigator navAutoZone = xDocumento.CreateNavigator();
                navAutoZone.SelectSingleNode("/Comprobante/@version").SetValue("3.2");
                navAutoZone.SelectSingleNode("/Comprobante/@sello").SetValue("3.2");
                navAutoZone.SelectSingleNode("/Comprobante/@certificado").SetValue("3.2");

                XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(xDocumento.NameTable);
                xmlnsManager.AddNamespace("xmlns:cdfi", "http://www.sat.gob.mx/cfd/3");

                xDocumento.DocumentElement.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xDocumento.DocumentElement.SetAttribute("xmlns:cdfi", "http://www.sat.gob.mx/cfd/3");
                xDocumento.DocumentElement.SetAttribute("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGenerar22_Click(object sender, EventArgs e)
        {
            XmlDocument xComprobante = new XmlDocument();
            XmlDocument xDocumento = new XmlDocument();
            XmlDocument xSalida = new XmlDocument();
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            XmlNodeList xNodoComprobante;
            string sRuta = string.Empty;
            try
            {
                openFileDialog1.ShowDialog();
                if (openFileDialog1.FileName.Equals(string.Empty))
                {
                    return;
                }
                sRuta = openFileDialog1.FileName;
                xDocumento.Load(sRuta);

                clsConvertirComprobante clConvertir = new clsConvertirComprobante();
                XElement xElemento = XElement.Parse(xDocumento.OuterXml);
                XElement xElementoSalida;
                xElementoSalida = clConvertir.fnRemoveAllNamespaces(xElemento);

                xSalida.LoadXml(xElementoSalida.ToString());

                //Quitamos los nodos que no necesitamos para el 2.2
                xSalida.ChildNodes[0].Attributes.RemoveNamedItem("noAprobacion");
                xSalida.ChildNodes[0].Attributes.RemoveNamedItem("anoAprobacion");
                xSalida.ChildNodes[0].Attributes.RemoveNamedItem("TipoCambio");
                xSalida.ChildNodes[0].Attributes.RemoveNamedItem("FolioFiscalOrig");
                xSalida.ChildNodes[0].Attributes.RemoveNamedItem("SerieFolioFiscalOrig");
                xSalida.ChildNodes[0].Attributes.RemoveNamedItem("FechaFolioFiscalOrig");
                xSalida.ChildNodes[0].Attributes.RemoveNamedItem("FolioFiscalOrig");
                xSalida.ChildNodes[0].Attributes.RemoveNamedItem("MontoFolioFiscalOrig");

                //Quitamos los valores de los atributos de la versión, el sello y el certificado
                XPathNavigator navDocumento = xSalida.CreateNavigator();
                navDocumento.SelectSingleNode("/Comprobante/@version").SetValue("3.2");
                navDocumento.SelectSingleNode("/Comprobante/@sello").SetValue(string.Empty);
                navDocumento.SelectSingleNode("/Comprobante/@certificado").SetValue(string.Empty);

                nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xComprobante = new XmlDocument(nsm.NameTable);
                xComprobante.CreateXmlDeclaration("1.0", "UTF-8", "no");
                xComprobante.AppendChild(xComprobante.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));

                fnCrearElementoRoot32(xComprobante, xSalida.DocumentElement.Attributes);

                foreach (XmlNode Nodo in xDocumento.DocumentElement.ChildNodes)
	            {
                    fnCrearElemento(xComprobante, Nodo.Name, Nodo, nsm);
                    //switch (Nodo.LocalName)
                    //{
                    //    case "Emisor":
                    //        //xComprobante.DocumentElement.AppendChild(fnCrearElemento(xComprobante, "Emisor", Nodo, nsm));
                    //        fnCrearElemento(xComprobante, "Emisor", Nodo, nsm);
                    //        break;
                    //    case "Receptor":
                    //        fnCrearElemento(xComprobante, "Receptor", Nodo, nsm);
                    //        break;
                    //    case "Conceptos":
                    //        fnCrearElemento(xComprobante, "Conceptos", Nodo, nsm);
                    //        break;
                    //    case "Impuestos":
                    //        fnCrearElemento(xComprobante, "Impuestos", Nodo, nsm);
                    //        break;

                    //}
	            }


            }
            catch (Exception ex)
            { 
            
            }
        }

        /// <summary>
        /// Crear elementos Raiz del Documento en Version 3.0
        /// </summary>
        /// <param name="pxDocumento">Documento</param>
        /// <param name="psElemento">Elemento</param>
        /// <param name="pasAtributos">Atributos</param>
        /// <returns></returns>
        private XmlElement fnCrearElemento(XmlDocument pxDocumento, string psElemento, XmlNode pasNodo, XmlNamespaceManager nsm)
        {
            XmlAttribute xAttr;

            if (("iedu").Contains(psElemento))
            {

            }
            else
            { 
            
            }

            XmlElement elemento = pxDocumento.CreateElement("cfdi", psElemento, "http://www.sat.gob.mx/cfd/3");


            XmlNode padre = null;

            foreach (XmlAttribute atributo in pasNodo.Attributes)
            {
                xAttr = pxDocumento.CreateAttribute(atributo.Name);
                xAttr.Value = atributo.Value;
                elemento.Attributes.Append(xAttr);
            }

            pxDocumento.DocumentElement.AppendChild(elemento);

            if (pasNodo.HasChildNodes)
            {
                foreach (XmlNode NodoHijo in pasNodo.ChildNodes)
                {
                    //padre = pxDocumento.SelectSingleNode(string.Format("/cfdi:Comprobante/cfdi:{0}", pasNodo.Name), nsm);
                    //padre = pxDocumento.SelectSingleNode(fnRevisarNodosPadre(pasNodo), nsm);
                    //padre.AppendChild(fnCrearElemento(pxDocumento, NodoHijo.Name, NodoHijo, nsm));
                    padre = pxDocumento.DocumentElement.LastChild.AppendChild(fnCrearElemento(pxDocumento, NodoHijo.Name, NodoHijo, nsm));
                }
            }
            return elemento;
        }

        /// <summary>
        /// Crear elementos Raiz del Documento en Version 3.2
        /// </summary>
        /// <param name="pxDocumento">Documento</param>
        /// <param name="pasAtributos">Atributos</param>
        private void fnCrearElementoRoot32(XmlDocument pxDocumento, XmlAttributeCollection pasAtributos)
        {
            XmlAttribute xAttr;
            foreach (XmlAttribute atributo in pasAtributos)
            {
                if (!(atributo.Name.Equals("xsi") || atributo.Name.Equals("xmlns") || atributo.Name.Equals("schemaLocation")))
                {
                    xAttr = pxDocumento.CreateAttribute(atributo.Name);
                    xAttr.Value = atributo.Value;
                    pxDocumento.DocumentElement.Attributes.Append(xAttr);
                }
            }
            xAttr = pxDocumento.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
            pxDocumento.DocumentElement.Attributes.Append(xAttr);
        }

        private string fnRevisarNodosPadre(XmlNode psNodo)
        {
            string sResultado = string.Empty;

            if (psNodo.Name == "Comprobante")
            {
                sResultado = string.Format("/cfdi:{0}", psNodo.Name) + sResultado;
            }
            else
            {
                sResultado = string.Format("/cfdi:{0}", psNodo.Name);
                sResultado = fnRevisarNodosPadre(psNodo.ParentNode) + sResultado;
            }

            return sResultado;
        }

        //private string fnRevisarNodosPadre(XmlNode psNodo)
        //{
        //    string sResultado = string.Empty;
        //    string[] Padres = null;

        //    if (!(psNodo.ParentNode == null))
        //    {
        //        sResultado += fnRevisarNodosPadre(psNodo.ParentNode);
        //    }




        //    return sResultado;
        //}
    }
}

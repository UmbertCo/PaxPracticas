using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace PaxConectorRetenciones
{
    public class clsDatosAdenda
    {
        public string sNombreCompleto { set; get; }

        public int nId { set; get; }

        XmlDocument xdDoc;

        bool bexisteAdenda;
        public bool bExisteAdenda { get { return bexisteAdenda; } }


        public clsDatosAdenda(XmlDocument pxdComprobante)
        {

            xdDoc = pxdComprobante;

            fnLlenarParametros();

        }

        public void fnLlenarParametros()
        {
            XmlNamespaceManager xnmNamespaces = new XmlNamespaceManager(xdDoc.NameTable);

            xnmNamespaces.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            try
            {
                XPathNodeIterator xpniIterador = xdDoc.CreateNavigator().Select("/cfdi:Comprobante/cfdi:Addenda/Vendedor", xnmNamespaces);


                xpniIterador.MoveNext();

                XPathNavigator xpnNavegador = xpniIterador.Current;

                sNombreCompleto = xpnNavegador.SelectSingleNode("nombre").Value + " "
                    + xpnNavegador.SelectSingleNode("apellido_paterno").Value + " "
                    + xpnNavegador.SelectSingleNode("apellido_materno").Value;


                nId = int.Parse(xpnNavegador.SelectSingleNode("id").Value);

                bexisteAdenda = true;
            }
            catch { bexisteAdenda = false; }
        }


    }
}

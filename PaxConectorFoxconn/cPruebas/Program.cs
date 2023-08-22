using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtileriasXML.Adendas;
using UtileriasXML;
using System.Xml;
namespace cPruebas
{
    class Program
    {
        static void Main(string[] args)
        {

             string sLayout = Properties.Resources.sLayout;



            AdendaFoxconn aAdenda = new AdendaFoxconn();

           

            XmlDocument comprobante = new XmlDocument();

            comprobante.LoadXml(Properties.Resources.sComprobante);

            aAdenda.xdComprobante = comprobante;

            aAdenda.psLayoutEntrante = Properties.Resources.sLayout;

            XmlDocument comad = aAdenda.xdComprobanteAdenda;

        }
    }
}

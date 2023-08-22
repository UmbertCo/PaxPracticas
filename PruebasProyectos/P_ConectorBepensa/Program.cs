using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;

namespace P_ConectorBepensa
{
    class Program
    {
        static void Main(string[] args)
        {

           XmlDocument xdDocument = new XmlDocument();

           xdDocument.Load(@"C:\Users\Ismael Hidalgo\Documents\GeneracionFacturas\asd1.xml");
            
            //clsTbl_Facturas tblEntrada = new clsTbl_Facturas(xdDocument);
            //clsCorrerSP_Scripts cspScripts = new clsCorrerSP_Scripts(@"Data Source=CORPUS\SQL2012EXPRESS;Initial Catalog=Factoraje;Persist Security Info=True;User ID=hexehell;Password=P4ssw0rd","/SEL.sql", EnuTipoScript.SEL);

            //cspScripts.tfParametrosEntrada = tblEntrada;

            clsValidacionBD ValidacionBD = new clsValidacionBD(xdDocument, @"Data Source=CORPUS\SQL2012EXPRESS;Initial Catalog=Factoraje;Persist Security Info=True;User ID=hexehell;Password=P4ssw0rd", "/INS.sql", "/SEL.sql");

            bool bExisteXML = ValidacionBD.fnExisteComprobante();

            bool bInsertoXML = ValidacionBD.fnInsertarComprobante();

            bExisteXML = ValidacionBD.fnExisteComprobante();

            //cspScripts.fnProc();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.Threading;
using System.Xml.Linq;

namespace P_PruebasconXML
{
    class Program
    {
        public static DataTable dtTablaErrores;

        static void Main(string[] args)
        {
            //XmlDocument xdDoc = new XmlDocument();

            //xdDoc.Load(@"C:\Users\Ismael Hidalgo\Documents\LogErr\Log.xml");

            //XPathNodeIterator xpni = xdDoc.CreateNavigator().Select("/LogOut/Entrada");

            //XPathNavigator[] xpNav = new XPathNavigator[xpni.Count];

            //int i = 0;


            XDocument xdDoc = XDocument.Parse("<a bnm='foo'><b asd='bar'></b><b asd='bir'></b><b asd='ber'></b></a>");

            XElement xeRoot = XElement.Load(@"C:\Users\Ismael Hidalgo\Documents\LogErr\Log.xml");//xdDoc.Root;

            XElement xRoot = XElement.Load(@"C:\Users\Ismael Hidalgo\Documents\LogErr\LogErrores24082016_0401 (1).xml"); //XElement.Load(new XmlNodeReader(xdDocErrores));

            XmlDocument xd = new XmlDocument();

            xd.Load(@"C:\Users\Ismael Hidalgo\Documents\LogErr\LogErrores24082016_0401 (1).xml");

            xd.CreateNavigator().SelectSingleNode("/Log").AppendChild(xd.DocumentElement.InnerXml);

            IEnumerable<XElement> xConsulta = xeRoot.Descendants("Entrada")
                                                   .Descendants()
                                                   .Where ( x => x.Attribute("hora") !=null && x.Attribute("nombre") !=null)
                                                   .Where(x => x.Attribute("hora").fnFechaDentroRango("17-11-2015 11:19:50", "17-11-2015 11:25:50")
                                                   //             || x.Attribute("nombre").Value.Contains("DepositoTP510517.088Deposito2")
                                                   //      )
                                                   ////.Where(x => x.Attribute("fechaCreacion") !=null)
                                                   //.Where(x => x.Attribute("fechaCreacion").fnFechaDentroRango( "22-08-2016 12:00:00", "23-08-2016 12:00:00")

                                                       // || x.Element("FolioSAT").this XElement xeFechaValue == "PM000011479669.xml"
                                                   //|| x.Element("Fecha").fnFechaDentroRango(x.Element("Hora"), "22-08-2016 12:00:00", "23-08-2016 12:00:00")
                                                   )
                                                   ;

            //IEnumerable<XElement> iConsulta = xeRoot.Descendants()
            //                                        .AncestorsAndSelf("Entrada")
            //                                        .Where(z => z.Attribute("fechaCreacion")!=null)
            //                                        .Where(r => r.Attribute("fechaCreacion").Value == "17-05-2016 22:56")
                                                    ////.DescendantsAndSelf()
                                                    //.Where(x => x.Attribute("nombre") != null)
                                                    //.Where(y => y.Attribute("nombre").Value =="DepositoTP510517.088Deposito225536.zip")
                                                    ;
                                                     
                //from entradas in xeRoot.Descendants("Entrada")
                //where DateTime.Parse((string)entradas.Attribute("fechaCreacion")).CompareTo(DateTime.Parse("17-05-2016 22:56")) == 0
                ////&&
                //select entradas;

            foreach (XElement xeEntrada in xConsulta)
            {
                Console.WriteLine(xeEntrada.Name.ToString());
            
            }
           

            

        }

        public static void fnHilo(Object oNavegadores) 
        {
            XPathNavigator[] xpNav = ( XPathNavigator[])oNavegadores;


            fnAgregarObjetoErrores(xpNav);

            Console.WriteLine(Thread.CurrentThread.Name + " ha terminado");
        
        }

        public static void fnAgregarObjetoErrores(XPathNavigator [] xpi )
        {
      
            try
            {

                for (int i = 0; i < xpi.Length; i++)
                {
                    string sNodo = string.Empty;
                    String sNombreArchivo = string.Empty;
                    string sMensaje = string.Empty;
                    string sFecha = string.Empty;
                    string sHora = string.Empty;

                    try
                    {
                        sNodo = xpi[i].Name;

                    }
                    catch { }

                    try
                    {
                        sMensaje = xpi[i].SelectSingleNode("text()").Value;

                    }
                    catch { }

                    DateTime dtTiempo = DateTime.MinValue;

                    try
                    {
                        sFecha = xpi[i].SelectSingleNode("@fechaCreacion").Value;

                        dtTiempo = DateTime.Parse(sFecha);

                        sFecha = dtTiempo.ToString("DD/MM/yyyy");
                    }
                    catch { }

                    try
                    {
                        sHora = dtTiempo.ToString("HH:mm:ss");
                    }
                    catch { }

                    XPathNodeIterator xpniHijos = xpi[i].Select("descendant::*");

                    while (xpniHijos.MoveNext())
                    {
                        sNodo = string.Empty;
                        sNombreArchivo = string.Empty;
                        sMensaje = string.Empty;
                        sFecha = string.Empty;
                        sHora = string.Empty;

                        try
                        {
                            sNodo = xpniHijos.Current.Name;

                        }
                        catch { }

                        try
                        {
                            sNombreArchivo = xpniHijos.Current.SelectSingleNode("@nombre").Value;

                        }
                        catch { }


                        try
                        {
                            sMensaje = xpniHijos.Current.SelectSingleNode("text()").Value;

                        }
                        catch { }

                        DateTime dtTiempoHijo = DateTime.MinValue;

                        try
                        {
                            sFecha = xpniHijos.Current.SelectSingleNode("@hora").Value;

                            dtTiempo = DateTime.Parse(sFecha);

                            sFecha = dtTiempo.ToString("DD/MM/yyyy");
                        }
                        catch { }

                        try
                        {
                            sHora = dtTiempo.ToString("HH:mm:ss");
                        }
                        catch { }

                        lock (dtTablaErrores)
                        dtTablaErrores.Rows.Add(sNodo, sNombreArchivo, sMensaje, sFecha, sHora);

                    }

                    lock(dtTablaErrores)
                    dtTablaErrores.Rows.Add(sNodo, sNombreArchivo, sMensaje, sFecha, sHora);

                }
            }
            catch (Exception ex)
            {
                //if (gvLog != null)
                //    gvLog.Rows.Add("Error", ex.Message, ex.StackTrace);
            }

        }
    }
}

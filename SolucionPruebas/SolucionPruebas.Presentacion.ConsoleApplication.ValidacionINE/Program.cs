using SolucionPruebas.Presentacion.ConsoleApplication.ValidacionINE.wsTimbrado;
using SolucionPruebas.Presentacion.ConsoleApplication.ValidacionINE.wsTimbradoTest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace SolucionPruebas.Presentacion.ConsoleApplication.ValidacionINE
{
    public class cError87
    {
        public string sClaveEntidad;
        public string sAmbito;
    }

    class Program
    {
        static void Main(string[] args)
        {
            //wsTimbrado.wcfRecepcionASMXSoapClient wsTimbrado = new wsTimbrado.wcfRecepcionASMXSoapClient();

            wsTimbradoTest.wcfRecepcionASMXSoapClient wsTimbradoTest = new wsTimbradoTest.wcfRecepcionASMXSoapClient();

            var listaArchivos = RecuperaListaArchivos(@"C:\Users\Ismael.Hidalgo\Desktop\INE\Sellados\");

            foreach (string nombreArchivo in listaArchivos)
            {

                if (Path.GetExtension(nombreArchivo) != ".xml")
                    continue;

                XmlDocument xdDocumento = new XmlDocument();
                xdDocumento.Load(nombreArchivo);
                //xdDocumento.Load(@"C:\Users\Ismael.Hidalgo\Desktop\INE\Sellados\INE(180).xml");
                //xdDocumento.Load(@"D:\PAXRegeneracionBateria\Archivos Generados\Ruta Sellados\Normal.xml");

                //string sResultado = wsTimbrado.fnEnviarXML(xdDocumento.InnerXml, "Factura", 0, "ismael.hidalgo", "L2KjRupim/8Wg2jv+YBKGcHusen9cytG33nqtNzSmf1g2KqmtbYXFmWcib6iF/Gn", "3.2");
                string sResultado = wsTimbradoTest.fnEnviarXML(xdDocumento.InnerXml, "Factura", 0, "WSDL_PAX", "C1EdgPPG1C3Zx52SSRIyKpkkK193GTsNq6nJRuHoRVj+gmEHSI/iLYaWmGQH9OMg", "3.2");




            }

            //string sResultado = string.Empty;
           
            //sResultado = fnValidacionINE(xdDocumento);

            //Console.WriteLine(string.Format("Comprobante validado: {0}, resultado: {1}", "INE(181).xml", sResultado));
            //Console.ReadLine();
        }

        public static string fnValidacionINE(XmlDocument xdComprobante)
        {
            string sValidacion = string.Empty;
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdComprobante.NameTable);
            try
            {
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("ine", "http://www.sat.gob.mx/ine");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                XPathNavigator xpnNavegador = xdComprobante.CreateNavigator();

                XPathNodeIterator xpniINE = xpnNavegador.Select("/cfdi:Comprobante/cfdi:Complemento/ine:INE", nsmComprobante);

                foreach (XPathNavigator xpnINE in xpniINE)
                {
                    string sTipoProceso = xpnINE.SelectSingleNode("@TipoProceso", nsmComprobante).Value.ToString();

                    //Error 180
                    if (sTipoProceso.Equals("Ordinario"))
                    {
                        string sTipoComite  = string.Empty;
                        try
                        {
                            sTipoComite = xpnINE.SelectSingleNode("@TipoComite", nsmComprobante).Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            return sValidacion = "Atributo TipoProceso: con valor {Ordinario}, debe existir en atributo ine:TipoComite";
                        }

                        //Error 184
                        try
                        {
                            if (sTipoComite.Equals("Ejecutivo Nacional"))
                            {                                
                                try
                                {
                                    XPathNodeIterator xpniEntidad = xpnINE.Select("/cfdi:Comprobante/cfdi:Complemento/ine:INE/ine:Entidad", nsmComprobante);

                                    foreach (XPathNavigator xpnEntidad in xpniEntidad)
                                    {
                                        return sValidacion = "Atributo TipoComite, con valor {Ejecutivo Nacional}, no debe existir ningún elemento ine:Entidad";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //return sValidacion = "Atributo TipoProceso: con valor {Precampaña} o valor {Campaña}, debe existir al menos un elemento Entidad:Ambito";
                                }

                            }
                        }
                        catch (Exception ex)
                        { 
                        
                        }

                        //Error 185
                        try
                        {
                            

                            if (sTipoComite.Equals("Ejecutivo Estatal"))
                            {
                                string sIdContabilidad = string.Empty;
                                try
                                {
                                    //try
                                    //{
                                        sIdContabilidad = xpnINE.SelectSingleNode("@IdContabilidad", nsmComprobante).Value.ToString();
                                    //}
                                    //catch { }

                                    //if (!string.IsNullOrEmpty(sIdContabilidad))
                                    //    return sValidacion = "Atributo TipoComite, con valor {Ejecutivo Estatal}, no debe existir ine:IdContabilidad";

                                    return sValidacion = "Atributo TipoComite, con valor {Ejecutivo Estatal}, no debe existir ine:IdContabilidad";
                                }
                                catch (Exception ex)
                                {
                                    //return sValidacion = "Atributo TipoProceso: con valor {Precampaña} o valor {Campaña}, debe existir al menos un elemento Entidad:Ambito";
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        //Error 186
                        if (!string.IsNullOrEmpty(sTipoComite))
                        {
                            try
                            {
                                XPathNodeIterator xpniEntidad = xpnINE.Select("/cfdi:Comprobante/cfdi:Complemento/ine:INE/ine:Entidad", nsmComprobante);

                                if(xpniEntidad.Count.Equals(0))
                                    return "Atributo TipoComite, debe existir al menos un elemente ine:Entidad y en ningún caso debe existir ine:Entidad:Ambito";

                                foreach (XPathNavigator xpnEntidad in xpniEntidad)
                                {
                                    string sAmbito = string.Empty;
                                    //try
                                    //{
                                        sAmbito = xpnINE.SelectSingleNode("@Ambito", nsmComprobante).Value.ToString();
                                    //}
                                    //catch { }

                                    //if (!string.IsNullOrEmpty(sTipoComite))
                                    //    return "Atributo TipoComite, debe existir al menos un elemente ine:Entidad y en ningún caso debe existir ine:Entidad:Ambito";

                                    return "Atributo TipoComite, debe existir al menos un elemente ine:Entidad y en ningún caso debe existir ine:Entidad:Ambito";
                                }
                            }
                            catch (Exception ex)
                            {
                                //return "Atributo TipoComite, debe existir al menos un elemente ine:Entidad y en ningún caso debe existir ine:Entidad:Ambito";
                            }
                        }
                    }


                    if (sTipoProceso.Equals("Precampaña") || sTipoProceso.Equals("Campaña"))
                    {
                        //Error 181
                        try
                        {
                            XPathNodeIterator xpniEntidad = xpnINE.Select("/cfdi:Comprobante/cfdi:Complemento/ine:INE/ine:Entidad", nsmComprobante);

                            foreach (XPathNavigator xpnEntidad in xpniEntidad)
                            {
                                string sAmbito = xpnEntidad.SelectSingleNode("@Ambito", nsmComprobante).Value.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            return sValidacion = "Atributo TipoProceso: con valor {Precampaña} o valor {Campaña}, debe existir al menos un elemento Entidad:Ambito";
                        }

                        //Error 182
                        try
                        {
                            string sTipoComite = xpnINE.SelectSingleNode("@TipoComite", nsmComprobante).Value.ToString();

                            //if(!string.IsNullOrEmpty(sTipoComite))
                            //    return sValidacion = "Atributo TipoProceso: con valor {Precampaña} o valor {Campaña}, no debe existir en atributo ine:TipoComite";

                            return sValidacion = "Atributo TipoProceso: con valor {Precampaña} o valor {Campaña}, no debe existir en atributo ine:TipoComite";
                        }
                        catch (Exception ex)
                        {
                            
                        }

                        //Error 183
                        try
                        {
                            string sIdContabilidad = xpnINE.SelectSingleNode("@IdContabilidad", nsmComprobante).Value.ToString();

                            //if (!string.IsNullOrEmpty(sIdContabilidad))
                            //    return sValidacion = "Atributo TipoProceso: con valor {Precampaña} o valor {Campaña}, no debe existir ine:IdContabilidad";

                            return sValidacion = "Atributo TipoProceso: con valor {Precampaña} o valor {Campaña}, no debe existir ine:IdContabilidad";
                        }
                        catch (Exception ex)
                        {
                            
                        }

                        //Error 187
                        try
                        {
                            
                            string sAmbito = string.Empty;
                            string sClaveEntidad = string.Empty;
                            XPathNodeIterator xpniEntidad = xpnINE.Select("/cfdi:Comprobante/cfdi:Complemento/ine:INE/ine:Entidad", nsmComprobante);

                            List<cError87> lError = new List<cError87>();

                            foreach (XPathNavigator xpnEntidad in xpniEntidad)
                            {
                                int nContador = 0;
                                cError87 cError = new cError87();

                                sAmbito = xpnEntidad.SelectSingleNode("@Ambito", nsmComprobante).Value.ToString();
                                sClaveEntidad = xpnEntidad.SelectSingleNode("@ClaveEntidad", nsmComprobante).Value.ToString();

                                foreach (XPathNavigator xpnEntidad1 in xpniEntidad)
                                {
                                    string sAmbitoBusqueda = string.Empty;
                                    string sClaveEntidadBusqueda = string.Empty;

                                    sAmbitoBusqueda = xpnEntidad1.SelectSingleNode("@Ambito", nsmComprobante).Value.ToString();
                                    sClaveEntidadBusqueda = xpnEntidad1.SelectSingleNode("@ClaveEntidad", nsmComprobante).Value.ToString();

                                    if (sAmbito.Equals(sAmbitoBusqueda) && sClaveEntidad.Equals(sClaveEntidadBusqueda))
                                    {
                                        nContador++;
                                    }

                                    if(nContador > 1)
                                        return sValidacion = "Elemento entidad, no se debe repetir la combinación de ine:Entidad:ClaveEntidad con ine:Entidad:Ambito";
                                }
                                //lError.Add(cError);
                            }
                            //List<cError87> lResultado = lError.GroupBy(item => item).Select(group => new {})
                        }
                        catch (Exception ex)
                        {
                            return sValidacion = "Elemento entidad, no se debe repetir la combinación de ine:Entidad:ClaveEntidad con ine:Entidad:Ambito";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
                sValidacion = "999";
            }
            return sValidacion;
        }

        public static IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos;
        }
    }
}

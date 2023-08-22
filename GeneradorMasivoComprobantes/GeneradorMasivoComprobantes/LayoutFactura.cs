using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Xml.Xsl;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Globalization;

namespace GeneradorMasivoComprobantes
{
    class LayoutFactura
    {
        static XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
        static XmlDocument xDoc = new XmlDocument();
        static XmlDocument xmlReceptores = new XmlDocument();
        static XmlDocument xmlConceptos = new XmlDocument();
        //objeto para seleccionar un nodo
        static XmlNode nodo = null;
        //objeto para crear atributos
        static XmlAttribute xAttr;
        //objeto para crear elementos
        static XmlElement elemento;

        //cxpath
        static XPathNavigator nav = null;
        static XPathNavigator navReceptores = null;
        static XPathNavigator navConceptos = null;
        static XPathNodeIterator NodoIterReceptor;
        static XPathNodeIterator NodoIterConcepto;
        static XPathNavigator[] ReceptorInfo;

        public static String rutaCarpeta;

        public static void GenerarLayoutFactura(String rfcEmisor, String Emisor, String calleEmisor, String numero_exEmisor, String num_intEmisor,
            String coloniaEmisor, String localidadEmisor, String municipioEmisor, String cpEmisor, String regfEmisor, String paisEmisor,
            String estadoEmisor, String Fecha, String serie, int folio, int numConceptos, int numComprobantes, String rutaCer, String rutaKey, String contrasena, bool impuestoRetenidos, bool guardarZip)
        {
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaLayoutFactura"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaLayoutFactura"]);

            rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaLayoutFactura"];

            //tomamos el directorio donde está siendo ejecutado el servicio
            String dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            DateTime dtFecha = DateTime.Now;

            string nombreArchivo = Guid.NewGuid() + "- " + dtFecha.ToString("yyyy-MM-dd") +".txt";

            string ruta = rutaCarpeta + nombreArchivo;

            StreamWriter sw = new StreamWriter(ruta, false);

            #region carga_receptores
            try
            {
                //cargamos el xml de receptores
                xmlReceptores.Load(@"receptores.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar catálogo de receptores.xml: " + ex);
            }

            //creamos el navegador para el xml de receptores
            navReceptores = xmlReceptores.CreateNavigator();
            #endregion

            #region carga_conceptos
            //path = Path.Combine(dir, "conceptos.xml");
            try
            {
                //cargamos el xml de conceptos
                xmlConceptos.Load(@"conceptos.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar catálogo de conceptos.xml: " + ex);
            }

            //creamos el navegador para el xml de receptores
            navConceptos = xmlConceptos.CreateNavigator();
            #endregion

            //subtotal
            double nsubTotal = 0;

            //guardamos todos los receptores
            NodoIterReceptor = navReceptores.Select("/receptores/receptor");

            //escogemos un receptor al azar
            Random r = new Random((int)DateTime.Now.Ticks);

            //navegador que será usado para recorrer el receptor seleccionado aleatoriamente
            XPathNavigator navegadorReceptor = null;

            int i = 0;

            //mientras encontremos receptores
            //recorremos para ver cuantos rfc estan correctos
            while (NodoIterReceptor.MoveNext())
            {
                navegadorReceptor = NodoIterReceptor.Current;
                string rfcRe = navegadorReceptor.SelectSingleNode("@rfc_receptor").ToString();
                if (rfcRe.Length >= 12 && rfcRe.Length <= 13)
                {
                    //cumple con el formato del RFC
                    if (Regex.IsMatch(rfcRe, "^[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A]?$"))
                    {
                        i++;
                    }
                }
            }

            //sabiendo cuantos rfc estan correctos podemos crear el arreglo de navegadores con el tamaño correcto
            ReceptorInfo = new XPathNavigator[i];

            NodoIterReceptor = navReceptores.Select("/receptores/receptor");

            i = 0;
            //volvemos a reccorer los receptores está vez para agregarlos al arreglo de navegadores
            while (NodoIterReceptor.MoveNext())
            {
                navegadorReceptor = NodoIterReceptor.Current;
                string rfcRe = navegadorReceptor.SelectSingleNode("@rfc_receptor").ToString();
                if (rfcRe.Length >= 12 && rfcRe.Length <= 13)
                {
                    //cumple con el formato del RFC
                    if (Regex.IsMatch(rfcRe, "^[A-Z,Ñ,&]{3,4}[0-9]{2}[0-1][0-9][0-3][0-9][A-Z,0-9]?[A-Z,0-9]?[0-9,A]?$"))
                    {
                        ReceptorInfo[i] = NodoIterReceptor.Current.Clone();
                        i++;
                    }
                }
            }

            int nReceptorSeleccionado = r.Next(1, ReceptorInfo.Length);

            navegadorReceptor = ReceptorInfo[nReceptorSeleccionado];

            //establecemos los namespace para su navegacion
            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            //********************************************[COMPROBANTE]***************************************************
            string sCo = string.Empty;

            //atributos
            sCo += crearAtributo("version", "3.2");
            sCo += crearAtributo("serie", serie);
            sCo += crearAtributo("folio", folio.ToString());
            sCo += crearAtributo("fecha", Fecha);
            sCo += crearAtributo("sello", "NA");
            sCo += crearAtributo("formaDePago", "Pago en una sola exhibicion");
            sCo += crearAtributo("noCertificado", "");//pendiente cambiar
            sCo += crearAtributo("certificado", "NA"); //pendiente cambiar
            sCo += crearAtributo("subTotal", ""); //pendiente calcular
            sCo += crearAtributo("Moneda", "MXN"); //pendiente calcular
            sCo += crearAtributo("TipoCambio", "1.0");
            sCo += crearAtributo("descuento", "0"); //pendiente calcular
            sCo += crearAtributo("total", "");//pendiente calcular
            sCo += crearAtributo("tipoDeComprobante", "ingreso");

            sCo += crearAtributo("LugarExpedicion", "Chihuahua, Chih.");
            sCo += crearAtributo("metodoDePago", "Efectivo");
            sCo = sCo.Substring(0, sCo.Length - 1);
            //********************************************[EMISOR]***************************************************
            string sRe = string.Empty;
            //atributos
            sRe += crearAtributo("rfc", rfcEmisor);
            sRe += crearAtributo("nombre", Emisor);
            sRe = sRe.Substring(0, sRe.Length - 1);

            //DomicilioFiscal
            string sDe = string.Empty;
            if (!calleEmisor.Equals(""))
                sDe += crearAtributo("calle", calleEmisor);
            if (!numero_exEmisor.Equals(""))
                sDe += crearAtributo("noExterior", numero_exEmisor);
            if (!coloniaEmisor.Equals(""))
                sDe += crearAtributo("colonia", coloniaEmisor);
            if (!localidadEmisor.Equals(""))
                sDe += crearAtributo("localidad", localidadEmisor);
            if (!municipioEmisor.Equals(""))
                sDe += crearAtributo("municipio", municipioEmisor);
            if (!estadoEmisor.Equals(""))
                sDe += crearAtributo("estado", estadoEmisor);
            if (!paisEmisor.Equals(""))
                sDe += crearAtributo("pais", paisEmisor);
            if (!cpEmisor.Equals(""))
                sDe += crearAtributo("codigoPostal", cpEmisor);
            
            sDe = sDe.Substring(0, sDe.Length - 1);

            //RegimenFiscal
            string sRf = string.Empty;
            if (!string.IsNullOrEmpty(regfEmisor))
            {
                if (!regfEmisor.Equals(""))
                {
                    sRf = crearAtributo("Regimen", regfEmisor);
                    sRf = sRf.Substring(0, sRf.Length - 1);
                }
            }
            //********************************************[RECEPTOR]***************************************************
            string sRr = string.Empty;
            //atributos
            sRr += crearAtributo("rfc", navegadorReceptor.SelectSingleNode("@rfc_receptor").ToString());
            sRr += crearAtributo("nombre", navegadorReceptor.SelectSingleNode("@nombre_receptor").ToString());

            sRr = sRr.Substring(0, sRr.Length - 1);

            //domicilio receptor
            string sDr = string.Empty;
            //atributos
            if (navegadorReceptor.OuterXml.Contains("calle"))
                sDr += crearAtributo("calle", navegadorReceptor.SelectSingleNode("domicilio/@calle").ToString());
            if (navegadorReceptor.OuterXml.Contains("noExterior"))
                sDr += crearAtributo("noExterior", navegadorReceptor.SelectSingleNode("domicilio/@noExterior").ToString());
            if (navegadorReceptor.OuterXml.Contains("colonia"))
                sDr += crearAtributo("colonia", navegadorReceptor.SelectSingleNode("domicilio/@colonia").ToString());
            if (navegadorReceptor.OuterXml.Contains("localidad"))
                sDr += crearAtributo("localidad", navegadorReceptor.SelectSingleNode("domicilio/@localidad").ToString());
            if (navegadorReceptor.OuterXml.Contains("municipio"))
                sDr += crearAtributo("municipio", navegadorReceptor.SelectSingleNode("domicilio/@municipio").ToString());
            if (navegadorReceptor.OuterXml.Contains("estado"))
                sDr += crearAtributo("estado", navegadorReceptor.SelectSingleNode("domicilio/@estado").ToString());
            if (navegadorReceptor.OuterXml.Contains("pais"))
                sDr += crearAtributo("pais", navegadorReceptor.SelectSingleNode("domicilio/@pais").ToString());
            if (navegadorReceptor.OuterXml.Contains("codigoPostal"))
                sDr += crearAtributo("codigoPostal", navegadorReceptor.SelectSingleNode("domicilio/@codigoPostal").ToString());

            sDr = sDr.Substring(0, sDr.Length - 1);
            //********************************************[CONCEPTOS]***************************************************
            string sCc = string.Empty;
            List<string> conceptos = new List<string>();
            //concepto
            //navegador que será usado para recorrer el concepto seleccionado aleatoriamente
            XPathNavigator navegadorConcepto = null;
            //empezamos a crear el número de conceptos que el usuario seleccionó
            for (int n = 0; n < numConceptos; n++)
            {
                NodoIterConcepto = navConceptos.Select("/conceptos/concepto");
                try
                {
                    //escogemos un receptor al azar
                    int nConceptoSeleccionado = r.Next(1, NodoIterConcepto.Count);

                    //mientras encontremos conceptos
                    for (int x = 0; NodoIterConcepto.MoveNext(); x++)
                    {
                        if (nConceptoSeleccionado == x)
                        {
                            navegadorConcepto = NodoIterConcepto.Current;
                            break;
                        }
                    };

                    //atributos
                    sCc = crearAtributo("cantidad", "1");
                    sCc += crearAtributo("unidad", navegadorConcepto.SelectSingleNode("@unidad").ToString());
                    sCc += crearAtributo("noIdentificacion", navegadorConcepto.SelectSingleNode("@noIdentificacion").ToString());
                    sCc += crearAtributo("descripcion", navegadorConcepto.SelectSingleNode("@descripcion").ToString());
                    sCc += crearAtributo("valorUnitario", factura.convertirNum(navegadorConcepto.SelectSingleNode("@valorUnitario").Value));
                    sCc += crearAtributo("importe", factura.convertirNum(navegadorConcepto.SelectSingleNode("@valorUnitario").Value));
                    sCc = sCc.Substring(0, sCc.Length - 1);
                    conceptos.Add(sCc);
                    nsubTotal += double.Parse(factura.convertirNum(navegadorConcepto.SelectSingleNode("@valorUnitario").Value), CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al crear nodo de concepto: " + ex);
                }
            }
            //asignamos el valor de subtotal una vez caculado todos los conceptos
            sCo = sCo.Replace("subTotal@", "subTotal@" + factura.convertirNum(nsubTotal.ToString()));
            //********************************************[IMPUESTOS]***************************************************
            string sIt = string.Empty;
            string sIr = string.Empty;

            sIt+= crearAtributo("impuesto", "IVA");
            sIt += crearAtributo("tasa", "16");
            double nIt = (nsubTotal * .16);
            sIt += crearAtributo("importe", factura.convertirNum((nsubTotal * .16).ToString()));
            sIt = sIt.Substring(0, sIt.Length - 1);

            if (impuestoRetenidos)
            {
                sIr += crearAtributo("impuesto", "ISR");
                double nIr = 200.00;
                sIr += crearAtributo("importe", (nIr).ToString("f2"));
                sIr = sIr.Substring(0, sIr.Length - 1);
            }
            
            string sIm = string.Empty;
            sIm += crearAtributo("totalImpuestosTrasladados", factura.convertirNum((nsubTotal * .16).ToString()));

            if (impuestoRetenidos)
            {
                sIm += crearAtributo("totalImpuestosRetenidos", "200.00");
            }

            //certificado emisor
            X509Certificate2 cCertificadoEmisor = new X509Certificate2();
            cCertificadoEmisor.Import(rutaCer);

            //certificado
            byte[] bCertificadoInvertido = cCertificadoEmisor.GetSerialNumber().Reverse().ToArray();
            string numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();

            sCo = sCo.Replace("noCertificado@", "noCertificado@" + numeroCertificado.ToString());

            if (impuestoRetenidos)
            {
                sCo = sCo.Replace("total@", "total@" + factura.convertirNum(((nsubTotal + (nsubTotal * .16)) - 200).ToString()));
            }
            else
                sCo = sCo.Replace("total@", "total@" + factura.convertirNum((nsubTotal + (nsubTotal * .16)).ToString()));

            sw.WriteLine("co?" + sCo);
            sw.WriteLine("re?" + sRe);
            sw.WriteLine("de?" + sDe);
            sw.WriteLine("rf?" + sRf);
            sw.WriteLine("rr?" + sRr);
            sw.WriteLine("dr?" + sDr);
            foreach(string concepto in conceptos)
                sw.WriteLine("cc?" + concepto);
            sw.WriteLine("im?" + sIm);
            sw.WriteLine("it?" + sIt);
            if (impuestoRetenidos)
                sw.WriteLine("ir?" + sIr);    
            
            //cerramos el archivo
            sw.Close();

            if(guardarZip)
            {
                File.Move(ruta, System.Configuration.ConfigurationManager.AppSettings["rutaZips"]+nombreArchivo);
                rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaZips"];
            }
        }

        public static string crearAtributo(string AtrNombre, string AtrValor)
        {
            string sRegresado = string.Empty;
            sRegresado = AtrNombre + "@" + AtrValor + "|";
            return sRegresado;
        }
    }
}

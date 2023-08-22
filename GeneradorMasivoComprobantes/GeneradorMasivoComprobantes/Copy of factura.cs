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
    class factura_c
    {
         XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
         XmlDocument xDoc = new XmlDocument();
        static XmlDocument xmlReceptores;
        static XmlDocument xmlConceptos ;
        //objeto para seleccionar un nodo
         XmlNode nodo = null;
        //objeto para crear atributos
         XmlAttribute xAttr;
        //objeto para crear elementos
        XmlElement elemento;

        //cxpath
         XPathNavigator nav = null;
         XPathNavigator navReceptores = null;
         XPathNavigator navConceptos = null;
         XPathNodeIterator NodoIterReceptor;
         XPathNodeIterator NodoIterConcepto;
         XPathNavigator[] ReceptorInfo;

        public static String rutaCarpeta;

        public static void fnInit() 
        {
            xmlReceptores = new XmlDocument();
            xmlConceptos = new XmlDocument();
        
        }

        public  void generaFactura(int idUsuario, int idEstructura, int tipoComprobante, char cEstatus, String rfcEmisor, String Emisor, String calleEmisor, String numero_exEmisor, String num_intEmisor,
            String coloniaEmisor, String localidadEmisor, String municipioEmisor, String cpEmisor, String regfEmisor, String paisEmisor,
            String estadoEmisor, String Fecha, String serie, int folio, int numConceptos, int numComprobantes, String rutaCer, String rutaKey, String contrasena, bool generarArchivos, bool timbrarComprobantes, bool impuestoRetenidos, bool guardarZip)
        {
            //tomamos el directorio donde está siendo ejecutado el servicio
            String dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            #region carga_receptores
            try
            {
                //cargamos el xml de receptores

                lock (xmlReceptores)
                {
                    if(xmlReceptores.OuterXml.Equals(""))
                    xmlReceptores.Load(@"receptores.xml");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar catálogo de receptores.xml: " + ex);
            }

            //creamos el navegador para el xml de receptores
            navReceptores = xmlReceptores.Clone().CreateNavigator();
            #endregion

            #region carga_conceptos
            //path = Path.Combine(dir, "conceptos.xml");
            try
            {
                //cargamos el xml de conceptos
                lock (xmlConceptos)
                {
                    if (xmlConceptos.OuterXml.Equals(""))
                        xmlConceptos.Load(@"conceptos.xml");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar catálogo de conceptos.xml: "+ex);
            }

            //creamos el navegador para el xml de receptores
            navConceptos = xmlConceptos.Clone().CreateNavigator();
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
            //comenzamos a escribir el xml
            xDoc = new XmlDocument();
            xDoc.CreateXmlDeclaration("1.0", "UTF-8", "no");
            //establecemos el nodo raiz
            xDoc.AppendChild(xDoc.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));

            //si estamos en co, inserta en el nodo raíz el atributo schemaLocation con el valor bla bla
            xAttr = xDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
            xDoc.DocumentElement.Attributes.Append(xAttr);

            //atributos
            crearAtributoRaiz("version", "3.2");
            crearAtributoRaiz("serie", serie);
            crearAtributoRaiz("folio", folio.ToString());
            crearAtributoRaiz("fecha", Fecha);
            crearAtributoRaiz("sello", "NA");
            crearAtributoRaiz("formaDePago", "Pago en una sola exhibicion");
            crearAtributoRaiz("noCertificado", "20001000000100005867");//pendiente cambiar
            crearAtributoRaiz("certificado", "NA"); //pendiente cambiar
            crearAtributoRaiz("subTotal", "0"); //pendiente calcular
            crearAtributoRaiz("Moneda", "MXN"); //pendiente calcular
            crearAtributoRaiz("TipoCambio", "1.0");
            crearAtributoRaiz("descuento", "0"); //pendiente calcular
            crearAtributoRaiz("total", "0");//pendiente calcular
            crearAtributoRaiz("tipoDeComprobante", "ingreso");
                
            crearAtributoRaiz("LugarExpedicion", "Chihuahua, Chih.");
            crearAtributoRaiz("metodoDePago", "Efectivo");
            //********************************************[EMISOR]***************************************************
            elemento = xDoc.CreateElement("cfdi", "Emisor", "http://www.sat.gob.mx/cfd/3");
            //atributos
            crearAtributo(elemento, "rfc", rfcEmisor);
            crearAtributo(elemento, "nombre", Emisor);
            //escribimos en el xml el nodo nuevo con sus atributos
            xDoc.DocumentElement.AppendChild(elemento);

            //DomicilioFiscal
            nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
            elemento = xDoc.CreateElement("cfdi", "DomicilioFiscal", "http://www.sat.gob.mx/cfd/3");
            if (!calleEmisor.Equals(""))
                crearAtributo(elemento, "calle", calleEmisor);
            if(!numero_exEmisor.Equals(""))
                crearAtributo(elemento, "noExterior", numero_exEmisor);
            if (!coloniaEmisor.Equals(""))
                crearAtributo(elemento, "colonia", coloniaEmisor);
            if (!localidadEmisor.Equals(""))
                crearAtributo(elemento, "localidad", localidadEmisor);
            if (!municipioEmisor.Equals(""))
                crearAtributo(elemento, "municipio", municipioEmisor);
            if (!estadoEmisor.Equals(""))
                crearAtributo(elemento, "estado", estadoEmisor);
            if (!paisEmisor.Equals(""))
                crearAtributo(elemento, "pais", paisEmisor);
            if (!cpEmisor.Equals(""))
                crearAtributo(elemento, "codigoPostal", cpEmisor);

            nodo.AppendChild(elemento);

            if (!string.IsNullOrEmpty(regfEmisor))
            {
                //RegimenFiscal
                elemento = xDoc.CreateElement("cfdi", "RegimenFiscal", "http://www.sat.gob.mx/cfd/3");
                if (!regfEmisor.Equals(""))
                    crearAtributo(elemento, "Regimen", regfEmisor);
                nodo.AppendChild(elemento);
            }

            //********************************************[RECEPTOR]***************************************************
            elemento = xDoc.CreateElement("cfdi", "Receptor", "http://www.sat.gob.mx/cfd/3");
            //atributos
            crearAtributo(elemento, "rfc", navegadorReceptor.SelectSingleNode("@rfc_receptor").ToString());
            crearAtributo(elemento, "nombre", navegadorReceptor.SelectSingleNode("@nombre_receptor").ToString());
            //escribimos en el xml el nodo nuevo con sus atributos
            xDoc.DocumentElement.AppendChild(elemento);

            //domicilio receptor
            nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor", nsm);
            elemento = xDoc.CreateElement("cfdi", "Domicilio", "http://www.sat.gob.mx/cfd/3");
            //atributos
            if (navegadorReceptor.OuterXml.Contains("calle"))
            crearAtributo(elemento, "calle", navegadorReceptor.SelectSingleNode("domicilio/@calle").ToString());
            if (navegadorReceptor.OuterXml.Contains("noExterior"))
            crearAtributo(elemento, "noExterior", navegadorReceptor.SelectSingleNode("domicilio/@noExterior").ToString());
            if (navegadorReceptor.OuterXml.Contains("colonia"))
            crearAtributo(elemento, "colonia", navegadorReceptor.SelectSingleNode("domicilio/@colonia").ToString());
            if (navegadorReceptor.OuterXml.Contains("localidad"))
            crearAtributo(elemento, "localidad", navegadorReceptor.SelectSingleNode("domicilio/@localidad").ToString());
            if (navegadorReceptor.OuterXml.Contains("municipio"))
            crearAtributo(elemento, "municipio", navegadorReceptor.SelectSingleNode("domicilio/@municipio").ToString());
            if (navegadorReceptor.OuterXml.Contains("estado"))
            crearAtributo(elemento, "estado", navegadorReceptor.SelectSingleNode("domicilio/@estado").ToString());
            crearAtributo(elemento, "pais", navegadorReceptor.SelectSingleNode("domicilio/@pais").ToString());
            if (navegadorReceptor.OuterXml.Contains("codigoPostal"))
            crearAtributo(elemento, "codigoPostal", navegadorReceptor.SelectSingleNode("domicilio/@codigoPostal").ToString());
            nodo.AppendChild(elemento);
            //********************************************[CONCEPTOS]***************************************************
            elemento = xDoc.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3");
            xDoc.DocumentElement.AppendChild(elemento);
            //concepto
            //navegador que será usado para recorrer el concepto seleccionado aleatoriamente
            XPathNavigator navegadorConcepto = null;
            //empezamos a crear el número de conceptos que el usuario seleccionó
            for (int n = 0; n < numConceptos; n++)
            {
                NodoIterConcepto = navConceptos.Select("/conceptos/concepto");
                try
                {
                    nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);
                    elemento = xDoc.CreateElement("cfdi", "Concepto", "http://www.sat.gob.mx/cfd/3");


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
                    crearAtributo(elemento, "cantidad", "1");
                    crearAtributo(elemento, "unidad", navegadorConcepto.SelectSingleNode("@unidad").ToString());
                    crearAtributo(elemento, "noIdentificacion", navegadorConcepto.SelectSingleNode("@noIdentificacion").ToString());
                    crearAtributo(elemento, "descripcion", navegadorConcepto.SelectSingleNode("@descripcion").ToString());
                    crearAtributo(elemento, "valorUnitario", convertirNum(navegadorConcepto.SelectSingleNode("@valorUnitario").Value));
                    crearAtributo(elemento, "importe", convertirNum(navegadorConcepto.SelectSingleNode("@valorUnitario").Value));
                    nsubTotal += double.Parse(convertirNum(navegadorConcepto.SelectSingleNode("@valorUnitario").Value), CultureInfo.InvariantCulture);
                    nodo.AppendChild(elemento);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error al crear nodo de concepto: "+ex);
                }
            }
            //asignamos el valor de subtotal una vez caculado todos los conceptos
            xDoc.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsm).Value = convertirNum(nsubTotal.ToString());

            //********************************************[IMPUESTOS]***************************************************
            //crealo
            elemento = xDoc.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3");
            //if (!(tipoComprobante == 1 || tipoComprobante == 3 || tipoComprobante == 4 || tipoComprobante == 5))
            //{
            //    xAttr = xDoc.CreateAttribute("totalImpuestosRetenidos");
            //    xAttr.Value = "0.00";
            //    elemento.Attributes.Append(xAttr);
            //}

            if (tipoComprobante == 1 || tipoComprobante == 6)
            {
                xAttr = xDoc.CreateAttribute("totalImpuestosTrasladados");
                xAttr.Value = "";
                elemento.Attributes.Append(xAttr);

                if (impuestoRetenidos)
                {
                    xAttr = xDoc.CreateAttribute("totalImpuestosRetenidos");
                    xAttr.Value = "";
                    elemento.Attributes.Append(xAttr);
                }
            }
            xDoc.DocumentElement.AppendChild(elemento);

            if (tipoComprobante == 1 || tipoComprobante == 6)
            {
                //nodo Traslados
                nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos", nsm);
                elemento = xDoc.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3");
                nodo.AppendChild(elemento);

                //nodo Traslado
                nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados", nsm);
                elemento = xDoc.CreateElement("cfdi", "Traslado", "http://www.sat.gob.mx/cfd/3");

                crearAtributo(elemento, "impuesto", "IVA");
                crearAtributo(elemento, "tasa", "16");
                crearAtributo(elemento, "importe", convertirNum((nsubTotal * .16).ToString()));
                nodo.AppendChild(elemento);
                //asignamos el valor de totalImpuestosTrasladados una vez caculado todos los conceptos
                xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosTrasladados", nsm).Value = convertirNum((nsubTotal * .16).ToString());

                if (impuestoRetenidos)
                {
                    //nodo Retenciones
                    nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos", nsm);
                    elemento = xDoc.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3");
                    nodo.AppendChild(elemento);

                    //nodo Retencion
                    nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Retenciones", nsm);
                    elemento = xDoc.CreateElement("cfdi", "Retencion", "http://www.sat.gob.mx/cfd/3");

                    //aqui me quede
                    crearAtributo(elemento, "impuesto", "ISR");
                    crearAtributo(elemento, "importe", "200.00");
                    nodo.AppendChild(elemento);
                    //asignamos el valor de totalImpuestosTrasladados una vez caculado todos los conceptos
                    xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosRetenidos", nsm).Value = "200.00";
                }
            }

            //certificado emisor
            X509Certificate2 cCertificadoEmisor = new X509Certificate2();
            cCertificadoEmisor.Import(rutaCer);

            //certificado
            string sCertificado = Convert.ToBase64String(cCertificadoEmisor.GetRawCertData());

            byte[] bCertificadoInvertido = cCertificadoEmisor.GetSerialNumber().Reverse().ToArray();
            string numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();

            xDoc.SelectSingleNode("/cfdi:Comprobante/@certificado", nsm).Value = sCertificado;
            xDoc.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsm).Value = numeroCertificado;

            if (tipoComprobante == 1 || tipoComprobante == 6)
            {
                if (impuestoRetenidos)
                {
                    xDoc.SelectSingleNode("/cfdi:Comprobante/@total", nsm).Value = convertirNum((nsubTotal + double.Parse(xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosTrasladados", nsm).Value, CultureInfo.InvariantCulture) - double.Parse(xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosRetenidos", nsm).Value, CultureInfo.InvariantCulture)).ToString());
                }
                else
                    xDoc.SelectSingleNode("/cfdi:Comprobante/@total", nsm).Value = convertirNum((nsubTotal + double.Parse(xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosTrasladados", nsm).Value, CultureInfo.InvariantCulture)).ToString());
            }

            //creamos el navegador del xml del comprobante
            nav = xDoc.CreateNavigator();

            string sCadenaOriginal = fnConstruirCadenaTimbrado(nav);

            //sello
            string error = string.Empty;

            string sello = fnGenerarSello(sCadenaOriginal, File.ReadAllBytes(rutaCer), File.ReadAllBytes(rutaKey), contrasena, ref error);
            xDoc.SelectSingleNode("/cfdi:Comprobante/@sello", nsm).Value = sello;

            if (timbrarComprobantes == true)
            {

                #region xml_timbre
                //nodo timbre
                //comenzamos a escribir el xml
                XmlDocument xDocTimbre = new XmlDocument();
                //establecemos el nodo raiz
                xDocTimbre.AppendChild(xDocTimbre.CreateElement("tfd", "TimbreFiscalDigital", "http://www.sat.gob.mx/TimbreFiscalDigital"));

                //navegador del xml del timbre
                XPathNavigator navTimbre = null;
                navTimbre = xDocTimbre.CreateNavigator();

                xAttr = xDocTimbre.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                xAttr.Value = "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/TimbreFiscalDigital/TimbreFiscalDigital.xsd";

                xDocTimbre.DocumentElement.Attributes.Append(xAttr);

                DateTime fechaTimbrado = DateTime.Now;

                xAttr = xDocTimbre.CreateAttribute("FechaTimbrado");
                xAttr.Value = fechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss");
                xDocTimbre.DocumentElement.Attributes.Append(xAttr);

                xAttr = xDocTimbre.CreateAttribute("version");
                xAttr.Value = fechaTimbrado.ToString("1.0");
                xDocTimbre.DocumentElement.Attributes.Append(xAttr);

                xAttr = xDocTimbre.CreateAttribute("UUID");
                xAttr.Value = Guid.NewGuid().ToString();
                xDocTimbre.DocumentElement.Attributes.Append(xAttr);

                X509Certificate2 cCertificadoSAT = new X509Certificate2();
                cCertificadoSAT.Import(Properties.Resources.pac);

                byte[] bCertificadoSATInvertido = cCertificadoSAT.GetSerialNumber().Reverse().ToArray();
                numeroCertificado = Encoding.Default.GetString(bCertificadoSATInvertido).ToString();

                xAttr = xDocTimbre.CreateAttribute("noCertificadoSAT");
                xAttr.Value = numeroCertificado;
                xDocTimbre.DocumentElement.Attributes.Append(xAttr);

                xAttr = xDocTimbre.CreateAttribute("selloCFD");
                xAttr.Value = xDoc.SelectSingleNode("/cfdi:Comprobante/@sello", nsm).Value;
                xDocTimbre.DocumentElement.Attributes.Append(xAttr);

                string scadenaOriginalSAT = fnConstruirCadenaTimbradoSAT(navTimbre);

                string selloSAT = fnGenerarSello(scadenaOriginalSAT, GeneradorMasivoComprobantes.Properties.Resources.pac, GeneradorMasivoComprobantes.Properties.Resources.pack, System.Configuration.ConfigurationManager.AppSettings["contrasenaTimbrado"], ref error);

                xAttr = xDocTimbre.CreateAttribute("selloSAT");
                xAttr.Value = selloSAT;
                xDocTimbre.DocumentElement.Attributes.Append(xAttr);
                #endregion

                //objeto para seleccionar un nodo
                XmlNode nodoTimbre = null;

                elemento = xDoc.CreateElement("cfdi", "Complemento", "http://www.sat.gob.mx/cfd/3");
                xDoc.DocumentElement.AppendChild(elemento);
                nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsm);

                nodoTimbre = xDoc.ImportNode(xDocTimbre.DocumentElement, true);

                nodo.AppendChild(nodoTimbre);

                //Generar el Hash para revisar si no hay uno existente en la BD
                string HASHTimbre = GetHASH(scadenaOriginalSAT).ToUpper();
                string HASHEmisor = GetHASH(sCadenaOriginal).ToUpper();

                string res = fnIndentar(xDoc);
                Console.WriteLine(res);
                insertarComprobante(xDoc, tipoComprobante, cEstatus, Fecha, idEstructura, idUsuario, Convert.ToChar(serie), 'C', HASHTimbre, HASHEmisor);
            }

            if (generarArchivos == true)
            {
                String res = fnIndentar(xDoc);
                xDoc.LoadXml(res);
                string UUID = string.Empty;
                String nombreArchivo = string.Empty;

                if (timbrarComprobantes == true)
                {
                    UUID = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value;
                    try
                    {        
                        if (tipoComprobante == 1)
                        {
                            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaFactura"])))
                                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaFactura"]);
                            nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaFactura"] + rfcEmisor + "-" + UUID + ".xml";
                            rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaFactura"];

                            if (guardarZip == true)
                            {
                                nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaZips"] + rfcEmisor + "-" + UUID + ".xml";
                                rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaZips"];
                            }

                            xDoc.Save(nombreArchivo);
                        }
                        if (tipoComprobante == 6)
                        {
                            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaHonorarios"])))
                                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaHonorarios"]);
                            nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaHonorarios"] + rfcEmisor + "-" + UUID + ".xml";
                            rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaHonorarios"];

                            if (guardarZip == true)
                            {
                                nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaZips"] + rfcEmisor + "-" + UUID + ".xml";
                                rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaZips"];
                            }

                            xDoc.Save(nombreArchivo);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al guardar archivo: " + ex);
                    }
                }
                else {
                    UUID = Guid.NewGuid().ToString();
                    try
                    {
                        if (tipoComprobante == 1)
                        {
                            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaFacturaSinTimbre"])))
                                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaFacturaSinTimbre"]);
                            nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaFacturaSinTimbre"] + rfcEmisor + "-" + UUID + ".xml";
                            rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaFacturaSinTimbre"];

                            if (guardarZip == true)
                            {
                                nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaZips"] + rfcEmisor + "-" + UUID + ".xml";
                                rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaZips"];
                            }

                            xDoc.Save(nombreArchivo);
                        }
                        if (tipoComprobante == 6)
                        {
                            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaHonorariosSinTimbre"])))
                                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaHonorariosSinTimbre"]);
                            nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaHonorariosSinTimbre"] + rfcEmisor + "-" + UUID + ".xml";
                            rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaHonorariosSinTimbre"];

                            if (guardarZip == true)
                            {
                                nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaZips"] + rfcEmisor + "-" + UUID + ".xml";
                                rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaZips"];
                            }

                            xDoc.Save(nombreArchivo);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al guardar archivo: " + ex);
                    }
                }
            }
        }

        public  void crearAtributoRaiz(string AtrNombre, String AtrValor)
        {
            xAttr = xDoc.CreateAttribute(AtrNombre);
            xAttr.Value = AtrValor;
            xDoc.DocumentElement.Attributes.Append(xAttr);
        }

        public  void crearAtributo(XmlElement element, string AtrNombre, String AtrValor)
        {
            xAttr = xDoc.CreateAttribute(AtrNombre);
            xAttr.Value = AtrValor;
            element.Attributes.Append(xAttr);
        }

        public  string fnIndentar(XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false);
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.OmitXmlDeclaration = true;
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }

            return sb.ToString();
        }

        public static string fnConstruirCadenaTimbrado(IXPathNavigable xml)
        {
            string sCadenaOriginal = string.Empty;
            MemoryStream ms;
            StreamReader srDll;
            XsltArgumentList args;
            XslCompiledTransform xslt;
            try
            {
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOri.V3211));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                sCadenaOriginal = srDll.ReadToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar CadOri_3_2_11.dll - "+ex);
            }
            return sCadenaOriginal;
        }

        public static string fnGenerarSello(String sCadenaOriginal, byte[] bCertificado, byte[] bLlave, string sContraseña, ref string sError)
        {
            #region Declaracion
            String sSello = String.Empty;
            X509Certificate2 cCertificadoEmisor = new X509Certificate2();
            Chilkat.Rsa rsa = new Chilkat.Rsa();
            Chilkat.PrivateKey key = new Chilkat.PrivateKey();
            Chilkat.PrivateKey pem = new Chilkat.PrivateKey();
            #endregion

            cCertificadoEmisor.Import(bCertificado);

            key.LoadPkcs8Encrypted(bLlave, sContraseña);

            pem.LoadPem(key.GetPkcs8Pem());

            bool bExito;
            bExito = rsa.UnlockComponent("INTERMRSA_78UJEvED0IwK");
            bExito = rsa.GenerateKey(1024);

            rsa.LittleEndian = false;

            rsa.EncodingMode = "base64";

            rsa.Charset = "utf-8";

            rsa.ImportPrivateKey(pem.GetXml());

            try
            {
                sSello = rsa.SignStringENC(sCadenaOriginal, "sha-1");

            }
            catch (Exception ex)
            {
                sError = ex.Message;
                MessageBox.Show(sError);
            }

            return sSello;
        }

        public static string fnConstruirCadenaTimbradoSAT(IXPathNavigable xml)
        {
            string sCadenaOriginalTimbre = string.Empty;
            MemoryStream ms;
            StreamReader srDll;
            XsltArgumentList args;
            XslCompiledTransform xslt;
            try
            {
                xslt = new XslCompiledTransform();
                //xslt.Load(@"tfd.xslt");
                xslt.Load(typeof(Timbrado.V3.TFDXSLT));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                sCadenaOriginalTimbre = srDll.ReadToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar tfd.xslt - "+ex);
            }
            return sCadenaOriginalTimbre;
        }

        public static void insertarComprobante(XmlDocument xml, int tipo_documento, char Estatus, String fechaDocumento, int idEstructura, int idUsuario, char serie, char origen, string HASHTimbre, string HASHEmisor)
        {

            String sUUID = "";
          String dFecha_Timbrado="";
          String sRfc_Emisor	="";
          String sNombre_Emisor	="";
          String sRfc_Receptor	="";
          String sNombre_Receptor="";
          String dFecha_Emision	="";
          String sSerie			="";
          String sFolio			="";
          String sTotal			="";
          String sMoneda        ="";
         

          XmlNamespaceManager xnms = new XmlNamespaceManager(xml.NameTable);
          xnms.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
          xnms.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

          try { sUUID = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", xnms).Value; }
          catch { }

          try { dFecha_Timbrado = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", xnms).Value; }
          catch { }

          try { sRfc_Emisor = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", xnms).Value; }
          catch { }


          try { sNombre_Emisor = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", xnms).Value; }
          catch { }


          try { sRfc_Receptor = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Recepto/@rfc", xnms).Value; }
          catch { }


          try { sNombre_Receptor = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Recepto/@nombre", xnms).Value; }
          catch { }


          try { dFecha_Emision = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", xnms).Value; }
          catch { }


          try { sSerie = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", xnms).Value; }
          catch { }


          try { sFolio = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", xnms).Value; }
          catch { }


          try { sTotal = xml.CreateNavigator().SelectSingleNode("cfdi:Comprobante/@total", xnms).Value;
        
          }
          catch { }


          try { sMoneda = xml.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", xnms).Value; }
          catch { }





            using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
            {
                con.Open();
                using (SqlTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("usp_Timbrado_InsertaComprobante_Ins", con))
                        {

                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("sXML",PAXCrypto.CryptoAES.EncriptaAES( xml.InnerXml));
                            cmd.Parameters.AddWithValue("nId_tipo_documento", tipo_documento);
                            cmd.Parameters.AddWithValue("cEstatus", Estatus);
                            cmd.Parameters.AddWithValue("dFecha_Documento", fechaDocumento);
                            cmd.Parameters.AddWithValue("nId_estructura", idEstructura);
                            cmd.Parameters.AddWithValue("nId_usuario_timbrado", idUsuario);
                            cmd.Parameters.AddWithValue("nSerie", serie);
                            cmd.Parameters.AddWithValue("sOrigen", origen);
                            cmd.Parameters.AddWithValue("sHash", HASHTimbre.ToUpper());
                            cmd.Parameters.AddWithValue("sDatos", HASHEmisor.ToUpper());

                            //Nuevos parametros 15-08-2016
                            cmd.Parameters.AddWithValue("@sUUID", sUUID);                     //@sUUID				
                            cmd.Parameters.AddWithValue("@dFecha_Timbrado", dFecha_Timbrado);      //@dFecha_Timbrado	
                            cmd.Parameters.AddWithValue("@sRfc_Emisor", sRfc_Emisor);          //@sRfc_Emisor		
                            cmd.Parameters.AddWithValue("@sNombre_Emisor", sNombre_Emisor);       //@sNombre_Emisor	
                            cmd.Parameters.AddWithValue("@sRfc_Receptor", sRfc_Receptor);        //@sRfc_Receptor		
                            cmd.Parameters.AddWithValue("@sNombre_Receptor", sNombre_Receptor);     //@sNombre_Receptor	
                            cmd.Parameters.AddWithValue("@dFecha_Emision", dFecha_Emision);       //@dFecha_Emision	
                            cmd.Parameters.AddWithValue("@sSerie", sSerie);               //@sSerie			
                            cmd.Parameters.AddWithValue("@sFolio", sFolio);               //@sFolio			
                            cmd.Parameters.AddWithValue("@nTotal",  PAXCrypto.CryptoAES.EncriptaAES(Convert.ToString(sTotal)));               //@nTotal			
                            cmd.Parameters.AddWithValue("@sMoneda", sMoneda);              //@sMoneda			
                         

                            cmd.ExecuteNonQuery();
                            tran.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al insertar comprobante en BD: "+ex);
                    }
                    finally
                    {
                        //tran.Commit();
                        con.Close();
                    }
                }
            }
        }

        public static  string GetHASH(string text)
        {
            byte[] hashValue;
            byte[] message = Encoding.UTF8.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public static string convertirNum(string valor)
        {
            //valor = valor.Replace('.', ',');
            valor = double.Parse(valor).ToString("f2");
            valor = valor.Replace(',', '.');
            return valor;
        }

  
    }
}

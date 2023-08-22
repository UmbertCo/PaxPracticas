using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Globalization;

namespace GeneradorMasivoComprobantes
{
    class nomina
    {
        static XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
        static XmlDocument xDoc = new XmlDocument();
        static XmlDocument xmlEmpleados = new XmlDocument();
        //objeto para seleccionar un nodo
        static XmlNode nodo = null;
        //objeto para crear atributos
        static XmlAttribute xAttr;
        //objeto para crear elementos
        static XmlElement elemento;

        //cxpath
        static XPathNavigator nav = null;
        static XPathNavigator navEmpleados = null;
        static XPathNodeIterator NodoIterEmpleado;

        public static String rutaCarpeta;

        public static void generaNomina(int idUsuario, int idEstructura, int tipoComprobante, char cEstatus, String rfcEmisor, String Emisor, String calleEmisor, String numero_exEmisor, String num_intEmisor,
            String coloniaEmisor, String localidadEmisor, String municipioEmisor, String cpEmisor, String regfEmisor, String paisEmisor,
            String estadoEmisor, String Fecha, String serie, int folio, int numConceptos, int numComprobantes, String rutaCer, String rutaKey, String contrasena, bool generarArchivos, bool timbrarComprobantes, bool guardarZip)
        {
            //tomamos el directorio donde está siendo ejecutado el servicio
            String dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, "empleados.xml");

            #region carga_empleados
            try
            {
                //cargamos el xml de empleados
                xmlEmpleados.Load(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al cargar catálogo empleados.xml -" + ex);
            }

            //creamos el navegador para el xml de receptores
            navEmpleados = xmlEmpleados.CreateNavigator();
            #endregion

            double perTotalGravado = 0;
            double deduTotalGravado = 0;

            //guardamos todos los receptores
            NodoIterEmpleado = navEmpleados.Select("/Empleados/Empleado");


            //escogemos un receptor al azar
            Random r = new Random((int)DateTime.Now.Ticks);

            int nReceptorSeleccionado = r.Next(1, NodoIterEmpleado.Count);

            //navegador que será usado para recorrer el receptor seleccionado aleatoriamente
            XPathNavigator navegadorEmpleado = null;

            //mientras encontremos receptores
            for (int n = 0; NodoIterEmpleado.MoveNext(); n++)
            {
                if (nReceptorSeleccionado == n)
                {
                    navegadorEmpleado = NodoIterEmpleado.Current;
                    break;
                }
            };

            //establecemos los namespace para su navegacion
            nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsm.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");


            //********************************************[COMPROBANTE]***************************************************
            //comenzamos a escribir el xml
            xDoc = new XmlDocument();
            xDoc.CreateXmlDeclaration("1.0", "UTF-8", "no");
            //establecemos el nodo raiz
            xDoc.AppendChild(xDoc.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));

            //si estamos en co, inserta en el nodo raíz el atributo schemaLocation con el valor bla bla
            xAttr = xDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd http://www.sat.gob.mx/nomina http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina11.xsd";
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
            crearAtributoRaiz("tipoDeComprobante", "egreso");
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
            if (!numero_exEmisor.Equals(""))
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
            crearAtributo(elemento, "rfc", navegadorEmpleado.SelectSingleNode("@RFC").ToString());
            crearAtributo(elemento, "nombre", navegadorEmpleado.SelectSingleNode("@Nombre").ToString());
            //escribimos en el xml el nodo nuevo con sus atributos
            xDoc.DocumentElement.AppendChild(elemento);
            //********************************************[CONCEPTOS]***************************************************
            elemento = xDoc.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3");
            xDoc.DocumentElement.AppendChild(elemento);
            //concepto
            try
            {
                nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);
                elemento = xDoc.CreateElement("cfdi", "Concepto", "http://www.sat.gob.mx/cfd/3");

                //atributos
                crearAtributo(elemento, "cantidad", "1");
                crearAtributo(elemento, "unidad", "Servicio");
                //crearAtributo(elemento, "noIdentificacion", navegadorConcepto.SelectSingleNode("@noIdentificacion").ToString());
                crearAtributo(elemento, "descripcion", "Pago de nómina.");
                crearAtributo(elemento, "valorUnitario", "0");
                crearAtributo(elemento, "importe", "0");
                nodo.AppendChild(elemento);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrió un error al crear nodo de concepto. - "+ex);
            }               

            //********************************************[IMPUESTOS]***************************************************
            //crealo
            elemento = xDoc.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3");

            xAttr = xDoc.CreateAttribute("totalImpuestosRetenidos");
            xAttr.Value = "0.00";
            elemento.Attributes.Append(xAttr);              
                
            xDoc.DocumentElement.AppendChild(elemento);

            nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos", nsm);
            elemento = xDoc.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3");
            nodo.AppendChild(elemento);

            nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Retenciones", nsm);
            elemento = xDoc.CreateElement("cfdi", "Retencion", "http://www.sat.gob.mx/cfd/3");
            crearAtributo(elemento, "impuesto", "ISR");
            crearAtributo(elemento, "importe", "0.00");
            nodo.AppendChild(elemento);

            //********************************************[NOMINA]***************************************************
            elemento = xDoc.CreateElement("cfdi", "Complemento", "http://www.sat.gob.mx/cfd/3");
            xDoc.DocumentElement.AppendChild(elemento);
            nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsm);
            elemento = xDoc.CreateElement("nomina", "Nomina", "http://www.sat.gob.mx/nomina");
            crearAtributo(elemento, "Version", "1.1");

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("RegistroPatronal"))
                    crearAtributo(elemento, "RegistroPatronal", navegadorEmpleado.SelectSingleNode("@RegistroPatronal").Value.ToString());
            }
            catch { }

            try
            {
                crearAtributo(elemento, "NumEmpleado", navegadorEmpleado.SelectSingleNode("@NumEmpleado").Value.ToString());
            }
            catch { }

            try
            {
                crearAtributo(elemento, "CURP", navegadorEmpleado.SelectSingleNode("@CURP").Value.ToString());
            }
            catch { }

            try{
                crearAtributo(elemento, "TipoRegimen", navegadorEmpleado.SelectSingleNode("@TipoRegimen").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("NumSeguridadSocial"))
                    crearAtributo(elemento, "NumSeguridadSocial", navegadorEmpleado.SelectSingleNode("@NumSeguridadSocial").Value.ToString());
                }
            catch { }

            try
            {
                crearAtributo(elemento, "FechaPago", Fecha.Remove(10, 9));
            }
            catch { }

            try{
                crearAtributo(elemento, "FechaInicialPago", Fecha.Remove(10, 9));
            }
            catch { }

            try
            {
                crearAtributo(elemento, "FechaFinalPago", Fecha.Remove(10, 9));
            }
            catch { }

            try{
                crearAtributo(elemento, "NumDiasPagados", "15");
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("Departamento"))
                    crearAtributo(elemento, "Departamento", navegadorEmpleado.SelectSingleNode("@Departamento").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("CLABE"))
                {
                    if (navegadorEmpleado.SelectSingleNode("@CLABE").Value.Length == 17)
                    {
                        crearAtributo(elemento, "CLABE", navegadorEmpleado.SelectSingleNode("@CLABE").Value.ToString());
                    }
                }
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("banco"))
                    crearAtributo(elemento, "Banco", navegadorEmpleado.SelectSingleNode("@banco").Value.ToString());
            }
            catch { }

            try
            {
            if (navegadorEmpleado.OuterXml.Contains("FechaInicioRelLaboral"))
                crearAtributo(elemento, "FechaInicioRelLaboral", navegadorEmpleado.SelectSingleNode("@FechaInicioRelLaboral").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("Puesto"))
                    crearAtributo(elemento, "Puesto", navegadorEmpleado.SelectSingleNode("@Puesto").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("TipoContrato"))
                    crearAtributo(elemento, "TipoContrato", navegadorEmpleado.SelectSingleNode("@TipoContrato").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("TipoJornada"))
                    crearAtributo(elemento, "TipoJornada", navegadorEmpleado.SelectSingleNode("@TipoJornada").Value.ToString());
            }
            catch { }

            crearAtributo(elemento, "PeriodicidadPago", "Quincenal");

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("SalarioBaseCotApor"))
                    crearAtributo(elemento, "SalarioBaseCotApor", factura.convertirNum(navegadorEmpleado.SelectSingleNode("@SalarioBaseCotApor").Value.ToString()));
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("RiesgoPuesto"))
                    crearAtributo(elemento, "RiesgoPuesto", navegadorEmpleado.SelectSingleNode("@RiesgoPuesto").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("SalarioDiarioIntegrado"))
                    crearAtributo(elemento, "SalarioDiarioIntegrado", factura.convertirNum(navegadorEmpleado.SelectSingleNode("@SalarioDiarioIntegrado").Value.ToString()));
            }
            catch { }

            nodo.AppendChild(elemento);

            //creamos el navegador del xml del comprobante
            nav = xDoc.CreateNavigator();

            nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina", nsm);

            //escogemos un receptor al azar
            Random salario = new Random();

            double nSalario = RandomDouble(1000, 50000);

            //percepciones
            elemento = xDoc.CreateElement("nomina", "Percepciones", "http://www.sat.gob.mx/nomina");
            crearAtributo(elemento, "TotalGravado", factura.convertirNum(nSalario.ToString()));
            crearAtributo(elemento, "TotalExento", "0.00");
            nodo.AppendChild(elemento);
                    
            nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Percepciones", nsm);
            //percepcion
            elemento = xDoc.CreateElement("nomina", "Percepcion", "http://www.sat.gob.mx/nomina");

            //Sueldos, Salarios Rayas y Jornales
            crearAtributo(elemento, "TipoPercepcion","001");
            crearAtributo(elemento, "Clave", "001");
            crearAtributo(elemento, "Concepto", "Sueldos, Salarios Rayas y Jornales");
            crearAtributo(elemento, "ImporteGravado", factura.convertirNum(nSalario.ToString()));
            crearAtributo(elemento, "ImporteExento", "0.0");
            perTotalGravado += nSalario;
            nodo.AppendChild(elemento);

            nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina", nsm);
            //deducciones
            elemento = xDoc.CreateElement("nomina", "Deducciones", "http://www.sat.gob.mx/nomina");
            crearAtributo(elemento, "TotalGravado", "0.0");
            crearAtributo(elemento, "TotalExento", "0.0");
            nodo.AppendChild(elemento);


            double nSeguro = RandomDouble(2000, 3000);
            nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Deducciones", nsm);
            //deduccion
            //Seguridad social
            elemento = xDoc.CreateElement("nomina", "Deduccion", "http://www.sat.gob.mx/nomina");
            crearAtributo(elemento, "TipoDeduccion", "001");
            crearAtributo(elemento, "Clave", "002");
            crearAtributo(elemento, "Concepto", "Seguridad social");
            crearAtributo(elemento, "ImporteGravado", factura.convertirNum(nSeguro.ToString()));
            crearAtributo(elemento, "ImporteExento", "0.0");
            nodo.AppendChild(elemento);

            double ISR = 200.00;

            //ISR
            elemento = xDoc.CreateElement("nomina", "Deduccion", "http://www.sat.gob.mx/nomina");
            crearAtributo(elemento, "TipoDeduccion", "002");
            crearAtributo(elemento, "Clave", "003");
            crearAtributo(elemento, "Concepto", "ISR");
            crearAtributo(elemento, "ImporteGravado", factura.convertirNum(ISR.ToString()));
            crearAtributo(elemento, "ImporteExento", "0.00");
            nodo.AppendChild(elemento);

            deduTotalGravado += nSeguro+ISR;

            xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Percepciones/@TotalGravado", nsm).Value = factura.convertirNum(nSalario.ToString());

            xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto/@valorUnitario", nsm).Value = factura.convertirNum((nSalario).ToString());
            xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto/@importe", nsm).Value = factura.convertirNum((nSalario).ToString());

            xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Deducciones/@TotalGravado", nsm).Value = factura.convertirNum((deduTotalGravado).ToString());

            xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion/@importe", nsm).Value = factura.convertirNum((ISR).ToString());
            xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Impuestos/@totalImpuestosRetenidos", nsm).Value = factura.convertirNum((ISR).ToString());

            xDoc.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsm).Value = factura.convertirNum(xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto/@importe", nsm).Value.ToString());
            xDoc.SelectSingleNode("/cfdi:Comprobante/@descuento", nsm).Value = factura.convertirNum(nSeguro.ToString());
            xDoc.SelectSingleNode("/cfdi:Comprobante/@total", nsm).Value = factura.convertirNum(((double.Parse(xDoc.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsm).Value.ToString(), CultureInfo.InvariantCulture) - nSeguro) - ISR).ToString());

            //certificado emisor
            X509Certificate2 cCertificadoEmisor = new X509Certificate2();
            cCertificadoEmisor.Import(rutaCer);

            //certificado
            string sCertificado = Convert.ToBase64String(cCertificadoEmisor.GetRawCertData());

            byte[] bCertificadoInvertido = cCertificadoEmisor.GetSerialNumber().Reverse().ToArray();
            string numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();

            xDoc.SelectSingleNode("/cfdi:Comprobante/@certificado", nsm).Value = sCertificado;
            xDoc.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsm).Value = numeroCertificado;

            string sCadenaOriginal = factura.fnConstruirCadenaTimbrado(nav);

            //sello
            string error = string.Empty;

            string sello = factura.fnGenerarSello(sCadenaOriginal, File.ReadAllBytes(rutaCer), File.ReadAllBytes(rutaKey), contrasena, ref error);
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

                string scadenaOriginalSAT = factura.fnConstruirCadenaTimbradoSAT(navTimbre);

                string selloSAT = factura.fnGenerarSello(scadenaOriginalSAT, GeneradorMasivoComprobantes.Properties.Resources.pac, GeneradorMasivoComprobantes.Properties.Resources.pack, System.Configuration.ConfigurationManager.AppSettings["contrasenaTimbrado"], ref error);

                xAttr = xDocTimbre.CreateAttribute("selloSAT");
                xAttr.Value = selloSAT;
                xDocTimbre.DocumentElement.Attributes.Append(xAttr);
                #endregion

                //objeto para seleccionar un nodo
                XmlNode nodoTimbre = null;

                nodo = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento", nsm);

                nodoTimbre = xDoc.ImportNode(xDocTimbre.DocumentElement, true);

                nodo.AppendChild(nodoTimbre);

                //Generar el Hash para revisar si no hay uno existente en la BD
                string HASHTimbre = factura.GetHASH(scadenaOriginalSAT).ToUpper();
                string HASHEmisor = factura.GetHASH(sCadenaOriginal).ToUpper();

                factura.insertarComprobante(xDoc, tipoComprobante, cEstatus, Fecha, idEstructura, idUsuario, Convert.ToChar(serie), 'C', HASHTimbre, HASHEmisor);
            }

            String nombreArchivo = string.Empty;

            if (generarArchivos == true)
            {
                String res = fnIndentar(xDoc);
                xDoc.LoadXml(res);

                string UUID = string.Empty;

                if (timbrarComprobantes == true)
                {
                    UUID = xDoc.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value;
                    try
                    {
                        if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaNomina"])))
                            Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaNomina"]);

                        nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaNomina"] + rfcEmisor + "-" + UUID + ".xml";
                        rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaNomina"];

                        if (guardarZip == true)
                        {
                            nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaZips"] + rfcEmisor + "-" + UUID + ".xml";
                            rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaZips"];
                        }

                        xDoc.Save(nombreArchivo);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al guardar archivo: " + ex);
                    }
                }
                else
                {
                    UUID = Guid.NewGuid().ToString();
                    try
                    {
                        if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaNominaSinTimbre"])))
                            Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaNominaSinTimbre"]);

                        nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaNominaSinTimbre"] + rfcEmisor + "-" + UUID + ".xml";
                        rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaNominaSinTimbre"];

                        if (guardarZip == true)
                        {
                            nombreArchivo = System.Configuration.ConfigurationManager.AppSettings["rutaZips"] + rfcEmisor + "-" + UUID + ".xml";
                            rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaZips"];
                        }

                        xDoc.Save(nombreArchivo);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error al guardar archivo: " + ex);
                    }
                }
            }
        }

        public static void crearAtributoRaiz(string AtrNombre, String AtrValor)
        {
            xAttr = xDoc.CreateAttribute(AtrNombre);
            xAttr.Value = AtrValor;
            xDoc.DocumentElement.Attributes.Append(xAttr);
        }

        public static void crearAtributo(XmlElement element, string AtrNombre, String AtrValor)
        {
            xAttr = xDoc.CreateAttribute(AtrNombre);
            xAttr.Value = AtrValor;
            element.Attributes.Append(xAttr);
        }

        public static string fnIndentar(XmlDocument doc)
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

        public static double RandomDouble(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}

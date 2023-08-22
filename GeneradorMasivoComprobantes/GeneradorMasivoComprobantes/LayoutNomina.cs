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

namespace GeneradorMasivoComprobantes
{
    class LayoutNomina
    {
        static XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
        static XmlDocument xDoc = new XmlDocument();
        static XmlDocument xmlReceptores = new XmlDocument();
        static XmlDocument xmlConceptos = new XmlDocument();
        static XmlDocument xmlEmpleados = new XmlDocument();
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
        static XPathNavigator navEmpleados = null;
        static XPathNodeIterator NodoIterEmpleado;

        public static String rutaCarpeta;

        public static void GenerarLayoutNomina(String rfcEmisor, String Emisor, String calleEmisor, String numero_exEmisor, String num_intEmisor,
            String coloniaEmisor, String localidadEmisor, String municipioEmisor, String cpEmisor, String regfEmisor, String paisEmisor,
            String estadoEmisor, String Fecha, String serie, int folio, int numComprobantes, String rutaCer, String rutaKey, String contrasena, bool guardarZip)
        {
            if (!(Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["rutaLayoutNomina"])))
                Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["rutaLayoutNomina"]);

            rutaCarpeta = System.Configuration.ConfigurationManager.AppSettings["rutaLayoutNomina"];
            //tomamos el directorio donde está siendo ejecutado el servicio
            String dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(dir, "empleados.xml");

            DateTime dtFecha = DateTime.Now;

            string nombreArchivo = Guid.NewGuid() + "- " + dtFecha.ToString("yyyy-MM-dd") + ".txt";
            string ruta = System.Configuration.ConfigurationManager.AppSettings["rutaLayoutNomina"] + nombreArchivo;

            StreamWriter sw = new StreamWriter(ruta, false);

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

            //subtotal
            double nsubTotal = 0;

            //guardamos todos los receptores
            NodoIterEmpleado = navEmpleados.Select("/Empleados/Empleado");

            int i = 0;

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
            sCo += crearAtributo("sello", "");
            sCo += crearAtributo("formaDePago", "Pago en una sola exhibicion");
            sCo += crearAtributo("noCertificado", "");//pendiente cambiar
            sCo += crearAtributo("certificado", ""); //pendiente cambiar
            sCo += crearAtributo("subTotal", ""); //pendiente calcular
            sCo += crearAtributo("Moneda", "MXN"); //pendiente calcular
            sCo += crearAtributo("TipoCambio", "1.0");
            sCo += crearAtributo("descuento", ""); //pendiente calcular
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
            sRr += crearAtributo("rfc", navegadorEmpleado.SelectSingleNode("@RFC").ToString());
            sRr += crearAtributo("nombre", navegadorEmpleado.SelectSingleNode("@Nombre").ToString());

            sRr = sRr.Substring(0, sRr.Length - 1);
            //********************************************[CONCEPTOS]***************************************************
            string sCc = string.Empty;
            //concepto

            //atributos
            sCc += crearAtributo("cantidad", "1");
            sCc += crearAtributo("unidad", "Servicio");
            //crearAtributo(elemento, "noIdentificacion", navegadorConcepto.SelectSingleNode("@noIdentificacion").ToString());
            sCc += crearAtributo("descripcion", "Pago de nómina.");
            sCc += crearAtributo("valorUnitario", "");
            sCc += crearAtributo("importe", "");
            sCc = sCc.Substring(0, sCc.Length - 1);
            
            //asignamos el valor de subtotal una vez caculado todos los conceptos
            //sCo = sCo.Replace("subTotal@", "subTotal@" + factura.convertirNum(nsubTotal.ToString()));

            //********************************************[IMPUESTOS]***************************************************
            string sIm = string.Empty;

            sIm += crearAtributo("totalImpuestosRetenidos", "");
            sIm = sIm.Substring(0, sIm.Length - 1);

            string sIr = string.Empty;

            sIr += crearAtributo("impuesto", "ISR");
            sIr += crearAtributo("importe","");
            sIr = sIr.Substring(0, sIr.Length - 1);


            //********************************************[NOMINA]***************************************************
            string sNom = string.Empty;
            sNom+=crearAtributo("Version", "1.1");


            try
            {
                if (navegadorEmpleado.OuterXml.Contains("RegistroPatronal"))
                    sNom += crearAtributo("RegistroPatronal", navegadorEmpleado.SelectSingleNode("@RegistroPatronal").Value.ToString());
            }
            catch { }

            try
            {
                sNom += crearAtributo("NumEmpleado", navegadorEmpleado.SelectSingleNode("@NumEmpleado").Value.ToString());
            }
            catch { }

            try
            {
                sNom += crearAtributo("CURP", navegadorEmpleado.SelectSingleNode("@CURP").Value.ToString());
            }
            catch { }

            try
            {
                sNom += crearAtributo("TipoRegimen", navegadorEmpleado.SelectSingleNode("@TipoRegimen").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("NumSeguridadSocial "))
                    sNom += crearAtributo("NumSeguridadSocial", navegadorEmpleado.SelectSingleNode("@NumSeguridadSocial").Value.ToString());
            }
            catch { }

            try
            {
                sNom += crearAtributo("FechaPago", Fecha.Remove(10, 9));
            }
            catch { }

            try
            {
                sNom += crearAtributo("FechaInicialPago", Fecha.Remove(10, 9));
            }
            catch { }

            try
            {
                sNom += crearAtributo("FechaFinalPago", Fecha.Remove(10, 9));
            }
            catch { }

            sNom += crearAtributo("NumDiasPagados", "15");


            try
            {
                if (navegadorEmpleado.OuterXml.Contains("Departamento"))
                    sNom += crearAtributo("Departamento", navegadorEmpleado.SelectSingleNode("@Departamento").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("CLABE"))
                {
                    if (navegadorEmpleado.SelectSingleNode("@CLABE").Value.Length==17)
                    {
                        sNom += crearAtributo("CLABE", navegadorEmpleado.SelectSingleNode("@CLABE").Value.ToString());
                    }
                }
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("banco"))
                    sNom += crearAtributo("Banco", navegadorEmpleado.SelectSingleNode("@banco").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("FechaInicioRelLaboral"))
                    sNom += crearAtributo("FechaInicioRelLaboral", navegadorEmpleado.SelectSingleNode("@FechaInicioRelLaboral").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("Puesto"))
                    sNom += crearAtributo("Puesto", navegadorEmpleado.SelectSingleNode("@Puesto").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("TipoContrato"))
                    sNom += crearAtributo("TipoContrato", navegadorEmpleado.SelectSingleNode("@TipoContrato").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("TipoJornada"))
                    sNom += crearAtributo("TipoJornada", navegadorEmpleado.SelectSingleNode("@TipoJornada").Value.ToString());
            }
            catch { }

            sNom += crearAtributo("PeriodicidadPago", "Quincenal");

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("SalarioBaseCotApor"))
                    sNom += crearAtributo("SalarioBaseCotApor", factura.convertirNum(navegadorEmpleado.SelectSingleNode("@SalarioBaseCotApor").Value.ToString()));
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("RiesgoPuesto"))//esto debe borrarse y pendiente calcular
                    sNom += crearAtributo("RiesgoPuesto", navegadorEmpleado.SelectSingleNode("@RiesgoPuesto").Value.ToString());
            }
            catch { }

            try
            {
                if (navegadorEmpleado.OuterXml.Contains("SalarioDiarioIntegrado"))
                    sNom += crearAtributo("SalarioDiarioIntegrado", factura.convertirNum(navegadorEmpleado.SelectSingleNode("@SalarioDiarioIntegrado").Value.ToString()));
            }
            catch { }

            sNom = sNom.Substring(0, sNom.Length - 1);

            //escogemos un receptor al azar
            Random salario = new Random();

            double nSalario = RandomDouble(1000, 50000);

            //percepciones
            string sPercs = string.Empty;
            sPercs += crearAtributo("TotalGravado", factura.convertirNum(nSalario.ToString()));
            sPercs += crearAtributo("TotalExento", "0.00");
            sPercs = sPercs.Substring(0, sPercs.Length - 1);

            //percepcion
            string sPer = string.Empty;

            //Sueldos, Salarios Rayas y Jornales
            sPer += crearAtributo("TipoPercepcion", "001");
            sPer += crearAtributo("Clave", "001");
            sPer += crearAtributo("Concepto", "Sueldos, Salarios Rayas y Jornales");
            sPer += crearAtributo("ImporteGravado", factura.convertirNum(nSalario.ToString()));
            sPer += crearAtributo("ImporteExento", "0.0");
            perTotalGravado += nSalario;
            sPer = sPer.Substring(0, sPer.Length - 1);

            //deducciones
            string sDeducs = string.Empty;
            sDeducs += crearAtributo("TotalGravado", "");
            sDeducs += crearAtributo("TotalExento", "0.00");
            sDeducs = sDeducs.Substring(0, sDeducs.Length - 1);

            double nSeguro = RandomDouble(1000, 50000);
            //deduccion
            //Seguridad social
            string sDedu = string.Empty;
            sDedu += crearAtributo("TipoDeduccion", "001");
            sDedu += crearAtributo("Clave", "002");
            sDedu += crearAtributo("Concepto", "Seguridad social");
            sDedu += crearAtributo("ImporteGravado", factura.convertirNum(nSeguro.ToString()));
            sDedu += crearAtributo("ImporteExento", "0.0");
            sDedu = sDedu.Substring(0, sDedu.Length - 1);

            double ISR = 200.00;

            string sDedu2 = string.Empty;
            //ISR
            sDedu2 += crearAtributo("TipoDeduccion", "002");
            sDedu2 += crearAtributo("Clave", "003");
            sDedu2 += crearAtributo("Concepto", "ISR");
            sDedu2 += crearAtributo("ImporteGravado", factura.convertirNum(ISR.ToString()));
            sDedu2 += crearAtributo("ImporteExento", "0.00");
            sDedu2 = sDedu2.Substring(0, sDedu2.Length - 1);

            deduTotalGravado += nSeguro + ISR;

            //conceptos
            sCc = sCc.Replace("valorUnitario@", "valorUnitario@" + factura.convertirNum(nSalario.ToString()));
            sCc = sCc.Replace("importe@", "importe@" + factura.convertirNum(nSalario.ToString()));

            sPercs = sPercs.Replace("valorUnitario@", "valorUnitario@" + factura.convertirNum((nSalario).ToString()));

            sPercs = sPercs.Replace("importe@", "importe@" + factura.convertirNum((nSalario).ToString()));

            sDeducs = sDeducs.Replace("TotalGravado@", "TotalGravado@" + factura.convertirNum((deduTotalGravado).ToString()));

            sIr = sIr.Replace("importe@", "importe@" + factura.convertirNum((ISR).ToString()));
            sIm = sIm.Replace("totalImpuestosRetenidos@", "totalImpuestosRetenidos@" + factura.convertirNum((ISR).ToString()));

            sCo=sCo.Replace("subTotal@", "subTotal@" + factura.convertirNum((nSalario).ToString()));
            sCo=sCo.Replace("descuento@", "descuento@" + factura.convertirNum(nSeguro.ToString()));
            sCo=sCo.Replace("total@", "total@" + factura.convertirNum(((nSalario- nSeguro)-ISR).ToString()));

            //certificado emisor
            X509Certificate2 cCertificadoEmisor = new X509Certificate2();
            cCertificadoEmisor.Import(rutaCer);

            //certificado
            byte[] bCertificadoInvertido = cCertificadoEmisor.GetSerialNumber().Reverse().ToArray();
            string numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();

            sCo = sCo.Replace("noCertificado@", "noCertificado@" + numeroCertificado.ToString());

            sw.WriteLine("co?" + sCo);
            sw.WriteLine("re?" + sRe);
            sw.WriteLine("de?" + sDe);
            sw.WriteLine("rf?" + sRf);
            sw.WriteLine("rr?" + sRr);
            sw.WriteLine("cc?" + sCc);
            sw.WriteLine("im?" + sIm);
            sw.WriteLine("ir?" + sIr);
            sw.WriteLine("nom?" + sNom);
            sw.WriteLine("percs?" + sPercs);
            sw.WriteLine("per?" + sPer);
            sw.WriteLine("deducs?" + sDeducs);
            sw.WriteLine("dedu?" + sDedu);
            sw.WriteLine("dedu?" + sDedu2);

            //cerramos el archivo
            sw.Close();

            if (guardarZip)
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

        public static double RandomDouble(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}


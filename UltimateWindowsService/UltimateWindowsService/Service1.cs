using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using Root.Reports;
using System.Reflection;


namespace UltimateWindowsService
{
    public partial class UltimateService : ServiceBase
    {
     
        string[] asArchivos;
       // static  Thread _thread = new Thread(DoYaThing);

        public UltimateService()
        {
     
            InitializeComponent();
            //DoYaThing();
        }
    

        protected override void OnStart(string[] args)
        {
           // if (args[0] == "DEBUG") { Debugger.Launch(); }
            Timer time;
            time = new Timer();
            time.Interval = 3000;
            try
            {
                 time.Elapsed += new ElapsedEventHandler(DoYaThing);
                 time.Start();
                
                //oh my dayum
               
               //comentario ismael
                //trololol
            }
            catch(Exception e)
            {
                escribirLog(e.Message);
            }
        }

        protected override void OnStop()
        {
        }


        public void DoYaThing(object source, ElapsedEventArgs e) 
        {
            asArchivos = leerArchivos();
            foreach (string s in asArchivos) 
            {
                YOLO(s);
            }
            try
            {
                generarPDF();
            }
            catch(Exception E) { escribirLog(E.Message); }
           
        }
       

       /* public  void DoYaThing()
        {
            asArchivos = leerArchivos();
            foreach (string s in asArchivos)
            {
                YOLO(s);
            }
            try
            {
                
                generarPDF();
            }
            catch (Exception E) { escribirLog(E.Message); }

        }*/

        //Obtener layouts y guardar su contenido como string
        public   string[] leerArchivos() 
        {
            string[] aLayouts = Directory.GetFiles(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\UltimateWindowsService\LAYOUTS\");
            string[] aStringLayout = new string[aLayouts.Length];
            for (int i = 0; i < aLayouts.Length; i++)
            {
                StreamReader file = new StreamReader(aLayouts[i]);
                aStringLayout[i] = file.ReadToEnd();
                file.Close();
            }
            escribirLog("Se obtuvieron: " + aStringLayout.Length + " de la carpeta");
            return aStringLayout;
            
        }

        public  void YOLO(string info)
        {
            bool chafa = verificarLayout(info);

            string yolo = info;

            if (chafa == true)
            {
                string sLinea = "El archivo " + " contiene errores y se movio al directorio: C:\\Users\\Marco.Santana\\Documents\\Visual Studio 2010\\Projects\\UltimateWindowsService\\LAYOUT MAL";
                escribirLog(sLinea);
                StreamWriter writer = new StreamWriter("C:\\Users\\Marco.Santana\\Documents\\Visual Studio 2010\\Projects\\UltimateWindowsService\\LAYOUT MAL\\error.txt");
                writer.Write(info);
                writer.Close();
               // yolo = "error";
            }
            else
            {
                StreamWriter writer = new StreamWriter("C:\\Users\\Marco.Santana\\Documents\\Visual Studio 2010\\Projects\\UltimateWindowsService\\LAYOUT BIEN\\infoXML.txt");
                writer.Write(info);
                writer.Close();
                generarXML("C:\\Users\\Marco.Santana\\Documents\\Visual Studio 2010\\Projects\\UltimateWindowsService\\LAYOUT BIEN\\infoXML.txt");
                string sLinea = "El archivo " + "esta bien chido y se genero el XML en : C:\\Users\\Marco.Santana\\Documents\\Visual Studio 2010\\Projects\\UltimateWindowsService\\LAYOUT BIEN";
                escribirLog(sLinea);
            }

            //return yolo;
        }

        public  bool verificarLayout(string layout)
        {
            bool flag = false;

            string[] cadena = layout.Split('?');
            string separador = cadena[0];
            string atrval = cadena[1];
            string[] info = atrval.Split('|');
            foreach (string s in info)
            {

                string[] temp = s.Split('@');
                string atributo = temp[0];
                string valor = temp[1];

                if (valor == "")
                {
                    string sLinea = "El archivo: " + " contine un error en el atributo: " + atributo;
                    escribirLog(sLinea);
                    flag = true;

                }
            }

            return flag;
        }

        public  void generarAtributo(XmlElement parentNode, string[] info)
        {
            foreach (string s in info)
            {
                string[] temp = s.Split('@');

                string atributo = temp[0];
                string valor = temp[1];
                parentNode.SetAttribute(atributo, valor);
            }
        }

        public  void generarXML(string layout)
        {
            bool flagConceptos = false;
            bool flagRetenciones = false;
            StreamReader file = new StreamReader(layout);
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement nodoComprobante = xmlDoc.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3");
            XmlElement nodoEmisor = xmlDoc.CreateElement("cfdi", "Emisor", "http://www.sat.gob.mx/cfd/3");
            XmlElement nodoReceptor = xmlDoc.CreateElement("cfdi", "Receptor", "http://www.sat.gob.mx/cfd/3");
            XmlElement nodoConceptos = xmlDoc.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3");
            XmlElement nodoImpuestos = xmlDoc.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3");
            string linea;
            while ((linea = file.ReadLine()) != null)
            {
                string[] cadena = linea.Split('?');
                string separador = cadena[0];
                string atrval = cadena[1];
                string[] info = atrval.Split('|');

                switch (separador)
                {
                    case "co":
                        XmlAttribute xAttr = xmlDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                        xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
                        xmlDoc.AppendChild(nodoComprobante);
                        nodoComprobante.SetAttributeNode(xAttr);
                        generarAtributo(nodoComprobante, info);
                        break;
                    case "re":
                        generarAtributo(nodoEmisor, info);
                        nodoComprobante.AppendChild(nodoEmisor);
                        break;
                    case "de":
                        XmlElement nodoDirEmi = xmlDoc.CreateElement("cfdi", "DomicilioFiscal", "http://www.sat.gob.mx/cfd/3");
                        generarAtributo(nodoDirEmi, info);
                        nodoEmisor.AppendChild(nodoDirEmi);
                        break;
                    case "ee":
                        XmlElement nodoExpedido = xmlDoc.CreateElement("cfdi", "ExpedidoEn", "http://www.sat.gob.mx/cfd/3");
                        generarAtributo(nodoExpedido, info);
                        nodoEmisor.AppendChild(nodoExpedido);
                        break;
                    case "rf":
                        XmlElement nodoRegimen = xmlDoc.CreateElement("cfdi", "RegimenFiscal", "http://www.sat.gob.mx/cfd/3");
                        generarAtributo(nodoRegimen, info);
                        nodoEmisor.AppendChild(nodoRegimen);
                        break;
                    case "rr":
                        generarAtributo(nodoReceptor, info);
                        nodoComprobante.AppendChild(nodoReceptor);
                        break;
                    case "dr":
                        XmlElement nodoDireccion = xmlDoc.CreateElement("cfdi", "Domicilio", "http://www.sat.gob.mx/cfd/3");
                        generarAtributo(nodoDireccion, info);
                        nodoReceptor.AppendChild(nodoDireccion);
                        break;
                    case "cc":
                        if (flagConceptos == false)
                        {
                            nodoComprobante.AppendChild(nodoConceptos);
                            flagConceptos = true;
                        }
                        else
                        {
                            XmlElement nodoConcepto = xmlDoc.CreateElement("cfdi", "Concepto", "http://www.sat.gob.mx/cfd/3");
                            generarAtributo(nodoConcepto, info);
                            nodoConceptos.AppendChild(nodoConcepto);
                        }
                        break;
                    case "ir":
                        XmlElement nodoRetenciones = xmlDoc.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3");
                        if (flagRetenciones == false)
                        {
                            nodoComprobante.AppendChild(nodoImpuestos);
                            nodoImpuestos.AppendChild(nodoRetenciones);
                            flagRetenciones = true;

                            XmlElement nodoRetencion = xmlDoc.CreateElement("cfdi", "Retencion", "http://www.sat.gob.mx/cfd/3");
                            generarAtributo(nodoRetencion, info);
                            nodoRetenciones.AppendChild(nodoRetencion);
                        }
                        else
                        {
                            XmlElement nodoRetencion = xmlDoc.CreateElement("cfdi", "Retencion", "http://www.sat.gob.mx/cfd/3");
                            generarAtributo(nodoRetencion, info);
                            nodoRetenciones.AppendChild(nodoRetencion);
                        }
                        break;
                    case "it":
                        XmlElement nodoTraslados = xmlDoc.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3");
                        nodoImpuestos.AppendChild(nodoTraslados);
                        XmlElement nodoTraslado = xmlDoc.CreateElement("cfdi", "Traslado", "http://www.sat.gob.mx/cfd/3");
                        generarAtributo(nodoTraslado, info);
                        nodoTraslados.AppendChild(nodoTraslado);
                        break;
                }
            }

            xmlDoc.Save("C:\\Users\\Marco.Santana\\Documents\\Visual Studio 2010\\Projects\\UltimateWindowsService\\XML GENERADO\\xmlConHeader.xml");
            file.Close();

        }

        public  void escribirLog(string mensaje)
        {

            string path = @"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\UltimateWindowsService\LOG\log.txt";
            StreamWriter tw;

            if (!File.Exists(path))
            {
                tw = new StreamWriter(path);
                tw.WriteLine(DateTime.Now.ToString());
                tw.WriteLine(mensaje);
                tw.Close();

            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(mensaje);
                    sw.Close();
                }
            }
        }

        public  void generarPDF() 
        {
            PdfFormatter pdfFormat = new PdfFormatter();
            Report report = new Report(pdfFormat);
            report.sAuthor = "David Elizalde";
            report.sTitle = "Factura de prueba";

            Page page = new Page(report);
            BrushProp brushProp1 = new BrushProp(report, Color.LightGoldenrodYellow);
            page.Add(10, 10, new RepRect(brushProp1, 700, 700));

            StaticContainer container = new StaticContainer(200, 200);
            container.Add(10, 100, new RepRect(brushProp1, 700, 700));
            page.Add(10, 10, container);

            //LOGO
            Image imagen = Image.FromFile(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\UltimateWindowsService\DogeCoin.jpg");
            MemoryStream memoStream1 = new MemoryStream();
            imagen.Save(memoStream1, ImageFormat.Jpeg);
            page.AddCC_MM(20, 20, new RepImage(memoStream1, 100, 100));

            //Nombre Empresa
            FontDef fontCorp = new FontDef(report, "Helvetica");
            FontProp fontPropCorp = new FontProp(fontCorp, 18);
            fontPropCorp.bBold = true;
            page.AddCB(30, new RepString(fontPropCorp, "DOGE CORP S.A DE C.V"));

            // Moto de la empresa
            // FontDef fontMoto = new FontDef(report, "Helvetica");
            FontProp fontPropMoto = new FontProp(fontCorp, 10);
            fontPropMoto.bItalic = true;
            page.Add(175, 50, new RepString(fontPropMoto, "\"So much IVA. Such Factura. Wow\""));
            fontPropMoto.bItalic = false;

            //Cargar datos de XML
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\UltimateWindowsService\XML GENERADO\xmlConHeader.xml");

            XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);
            nsm.AddNamespace("cfdi","http://www.sat.gob.mx/cfd/3");
            //Obteniendo valores individuales
            XPathNavigator nav = doc.CreateNavigator();
            string metodoDePago = nav.SelectSingleNode("/cfdi:Comprobante/@metodoDePago",nsm).Value;
            string lugarExpedicion = nav.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", nsm).Value;
            string total = nav.SelectSingleNode("/cfdi:Comprobante/@total", nsm).Value;
            string nombreEmisor = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsm).Value;
            string rfcEmisor = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsm).Value;
            string nombreReceptor = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsm).Value;

            FontProp fontContent = new FontProp(fontCorp, 8);
            page.Add(10, 130, new RepString(fontContent, "Metodo de pago: " + metodoDePago));
            page.Add(10, 150, new RepString(fontContent, "Lugar de expedicion: " + lugarExpedicion));
            page.Add(10, 170, new RepString(fontContent, "Emisor: " + nombreEmisor));
            page.Add(10, 190, new RepString(fontContent, "RFC: " + rfcEmisor));
            page.Add(10, 210, new RepString(fontContent, "Receptor: " + nombreReceptor));


            //Obteniendo lista de conceptos

            XPathNodeIterator iterator = nav.Select("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsm);
            int renglon = 230;
            while (iterator.MoveNext())
            {
                string importe = iterator.Current.SelectSingleNode("@importe").Value;
                string valorUnitario = iterator.Current.SelectSingleNode("@valorUnitario").Value;
                string descripcion = iterator.Current.SelectSingleNode("@descripcion").Value;
                string noIdentificacion = iterator.Current.SelectSingleNode("@noIdentificacion").Value;
                string unidad = iterator.Current.SelectSingleNode("@unidad").Value;
                string cantidad = iterator.Current.SelectSingleNode("@cantidad").Value;

                page.Add(10, renglon, new RepString(fontContent, "Importe:"));
                page.Add(10, renglon + 10, new RepString(fontContent, importe));

                page.Add(70, renglon, new RepString(fontContent, "V.U:"));
                page.Add(70, renglon + 10, new RepString(fontContent, valorUnitario));

                page.Add(120, renglon, new RepString(fontContent, "Descripcion:"));
                page.Add(120, renglon + 10, new RepString(fontContent, descripcion));

                page.Add(190, renglon, new RepString(fontContent, "Identificacion:"));
                page.Add(190, renglon + 10, new RepString(fontContent, noIdentificacion));

                page.Add(270, renglon, new RepString(fontContent, "Unidad:"));
                page.Add(270, renglon + 10, new RepString(fontContent, unidad));

                page.Add(330, renglon, new RepString(fontContent, "Cantidad:"));
                page.Add(330, renglon + 10, new RepString(fontContent, cantidad));

                renglon += 20;
            }
            report.Save(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\UltimateWindowsService\PDF\DOGEfactura.pdf");
            //File.Create(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\MISPELOTAS.TXT");
           // RT.ViewPDF(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+@"\PDF\DogeFactura.pdf");


        }

    }
}

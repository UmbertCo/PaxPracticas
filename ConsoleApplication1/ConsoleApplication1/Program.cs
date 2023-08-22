using DevExpress.Pdf;
using DevExpress.Pdf.Drawing;
using DevExpress.Pdf.Native;





using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using ConsoleApplication1.Cancel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

namespace ConsoleApplication1
{
    public class Program 
    {
        #region Attributtes
        public static List<Class1> listaDatos = new List<Class1>();
        public static List<int> listaIds = new List<int>();
        public static List<int> listaIdCC = new List<int>();
        public static List<int> listaElementosNivelUno = new List<int>();
        public static int contadorElementosNivelActual;
        

        public static int contadorLayouts = 0;
        public static int contadorCancelaciones = 0;
        public static int inicio = 0;
        public static int fin = 1;
        public static int ccits = 0;
        public static int ccirs = 0;
        public static int its = 0;
        public static string noCertificado = String.Empty;
        public static string certificadoBase64 = String.Empty;
        public static int irs = 0;
        public static int tempId = 0;
        public static int controlador = 0;
        public static int tempAnteriorNivelUno = 0;
        public static int tempSiguienteNivelUno = 0;        


        public static string rutaXML = "C:\\PAXConector-Test\\XMLGenerado\\";
        public static string rutaTXT = "C:\\PAXConector-Test\\TXTGenerado\\";
        public static string rutaCER = "C:\\PAXConector-Test\\Certificados\\";
        public static string rutaESQ = "C:\\PAXConector-Test\\Esquemas\\";
        public static string rutaSSL = "C:\\PAXConector-Test\\OpenSSL\\";
        public static string rutaENT = "C:\\PAXConector-Test\\Entrada\\";
        public static string rutaCEF = "C:\\PAXConector-Test\\CancelacionesEfectivas\\";
        public static string rutaCNE = "C:\\PAXConector-Test\\CancelacionesNegativas\\";
        public static string rutaCEN = "C:\\PAXConector-Test\\CancelacionEntrada\\";


        public static string comando1 = rutaSSL + "openssl.exe x509 -in " + rutaCER + "CSD_Pruebas_CFDI_MAG041126GT8.cer -inform DER -out " + rutaCER + "test.cer.pem -outform PEM";
        public static string comando2 = rutaSSL + "openssl.exe pkcs8 -inform DER -in " + rutaCER + "CSD_Pruebas_CFDI_MAG041126GT8.key -passin pass:12345678a -out " + rutaCER + "test.key.pem";
        public static string comando3 = rutaSSL + "openssl.exe dgst -sha256 -sign " + rutaCER + "test.key.pem -out " + rutaTXT + "digest" + contadorLayouts + ".txt " + rutaTXT + "cadenaOriginal" + contadorLayouts + ".txt";
        public static string comando4 = rutaSSL + "openssl.exe enc -in " + rutaTXT + "digest" + contadorLayouts + ".txt -out " + rutaTXT + "sello" + contadorLayouts + ".txt -base64 -A -K " + rutaCER + "test.key.pem";
        
        private static System.Timers.Timer aTimer;
        #endregion

        #region Main
        static void Main(string[] args)
        {
            //ejecutar();
            //leerFolderCancelacion();            


            //using (FileSystemWatcher watcher = new FileSystemWatcher())
            //{
            //    watcher.Path = rutaCEN;
            //    watcher.NotifyFilter = NotifyFilters.LastAccess
            //                         | NotifyFilters.LastWrite
            //                         | NotifyFilters.FileName
            //                         | NotifyFilters.DirectoryName;

            //    // Only watch text files.
            //    watcher.Filter = "*.txt";

            //    // Add event handlers.
            //    watcher.Created += OnChanged;

            //    // Begin watching.
            //    watcher.EnableRaisingEvents = true;
            //    Console.WriteLine("Press 'q' to quit the sample.");
            //    while (Console.Read() != 'q') ;
            //}

            string reportPath = @"c:\\Temp\Test.pdf";

            using (XtraReport1 report = new XtraReport1())
            {
                // Specify PDF-specific export options.
                PdfExportOptions pdfOptions = report.ExportOptions.Pdf;
                pdfOptions.PdfACompatibility = PdfACompatibility.PdfA2b;
                bool hello = pdfOptions.PdfACompatible;
                
                if (hello==false)
                    Console.WriteLine("no se armo");
                else
                    report.ExportToPdf(reportPath, pdfOptions);
            }
            
        }
        #endregion


                

        #region Methods

        private static string[] readAllFiles(string path)
        {
            string[] fileEntries = Directory.GetFiles(path);
            foreach (var item in fileEntries)
            {
                Console.WriteLine(item);
            }
            return fileEntries;
        }

        private static void ejecutar()
        {
            string[] archivos = readAllFiles(rutaENT);
            if (archivos.Count() > 0)
            {
                foreach (var item in archivos)
                {
                    //Preparativos
                    ReadFromFile(item);
                    listaIdCC = generateList();
                    listaElementosNivelUno = generateRelations();                    
                    //Fin Preparativos

                    //Comandos parte uno (PEMS)
                    executeCommand(comando1);
                    executeCommand(comando2);
                    //Fin Comandos parte uno (PEMS)

                    //Armado
                    createXML();
                    insertarNoCertificado();
                    crearCadenaOriginal();
                    //Fin Armado

                    //Comandos parte dos (Digest y sello)
                    executeCommand(comando3);
                    executeCommand(comando4);
                    //Fin Comandos parte dos (Digest y sello)

                    //peticionSOAP y ultimos ensambles
                    insertarSello();
                    ExportCertToBase64();
                    insertarCertificado();
                    validarXML();
                    peticionSOAP();
                    //Fin peticionSOAP y ultimos ensambles

                    //Limpieza
                    //File.Delete(item);
                    listaDatos.Clear();
                    listaIds.Clear();
                    listaIdCC.Clear();
                    listaElementosNivelUno.Clear();
                    contadorLayouts++;

                    inicio = 0;
                    fin = 1;
                    ccits = 0;
                    ccirs = 0;
                    its = 0;
                    noCertificado = String.Empty;
                    certificadoBase64 = String.Empty;
                    irs = 0;
                    tempId = 0;
                    controlador = 0;
                    tempAnteriorNivelUno = 0;
                    tempSiguienteNivelUno = 0;
                    
                    comando1 = rutaSSL + "openssl.exe x509 -in " + rutaCER + "CSD_Pruebas_CFDI_MAG041126GT8.cer -inform DER -out " + rutaCER + "test.cer.pem -outform PEM";
                    comando2 = rutaSSL + "openssl.exe pkcs8 -inform DER -in " + rutaCER + "CSD_Pruebas_CFDI_MAG041126GT8.key -passin pass:12345678a -out " + rutaCER + "test.key.pem";
                    comando3 = rutaSSL + "openssl.exe dgst -sha256 -sign " + rutaCER + "test.key.pem -out " + rutaTXT + "digest" + contadorLayouts + ".txt " + rutaTXT + "cadenaOriginal" + contadorLayouts + ".txt";
                    comando4 = rutaSSL + "openssl.exe enc -in " + rutaTXT + "digest" + contadorLayouts + ".txt -out " + rutaTXT + "sello" + contadorLayouts + ".txt -base64 -A -K " + rutaCER + "test.key.pem";
                    //Fin Limpieza
                   
                }
            }
            else {
                Console.WriteLine("No hay archivos que leer");
                Console.ReadKey();
            }
        }      

        private static void ReadFromFile(string filePath)
        {
            int i = 0;
            string tempConceptoBase = "";
            //string path = @"C:\Users\diego.flores\Desktop\Layout_3_3.txt";
            string path = filePath;
            // Read a text file line by line.  
            string[] lines = File.ReadAllLines(path);
            //For each correspondiente a cada linea parseada del archivo de texto            
            foreach (string line in lines)
            {
                String[] spearator = { "?" };
                String[] strlistPipes;
                Int32 count = 2;
                String[] strlist = line.Split(spearator, count, StringSplitOptions.RemoveEmptyEntries);

                //For each correspondiente al identificador del extramo izquiero separado por "?"
                foreach (var itemMostLeft in strlist)
                {
                    if (!itemMostLeft.Contains("|") || !itemMostLeft.Contains("@"))
                    {
                        tempConceptoBase = itemMostLeft;
                        listaIds.Add(i);
                        i++;
                    }
                    String[] spearatorPipe = { "|" };
                    Int32 countPipe = 20;
                    strlistPipes = itemMostLeft.Split(spearatorPipe, countPipe, StringSplitOptions.RemoveEmptyEntries);
                    //For each correspondiente al siguiente identificador separado por "|"
                    foreach (var itemPipe in strlistPipes)
                    {
                        string tempValor = "";
                        string tempConcepto = "";
                        int identifier = 0;
                        String[] spearatorArroba = { "@" };
                        Int32 countArroba = 5;
                        String[] strlistArroba = itemPipe.Split(spearatorArroba, countArroba, StringSplitOptions.RemoveEmptyEntries);
                        //For each correspondiente al siguiente identificador separado por "@"
                        foreach (var itemArroba in strlistArroba)
                        {
                            if (identifier == 1)
                            {
                                tempValor = itemArroba;
                            }
                            if (identifier == 0)
                            {
                                tempConcepto = itemArroba;
                            }
                            identifier = identifier + 1;
                        }
                        //Final del For each correspondiente al siguiente identificador separado por "@"
                        if (tempConcepto != "" && tempValor != "")
                        {
                            listaDatos.Add(new Class1
                            {
                                strMostLeft = tempConceptoBase,
                                strConcepto = tempConcepto,
                                strValor = tempValor,
                                strId = i,
                            });
                        }
                    }
                    //Final del For each correspondiente al siguiente identificador separado por "|"
                }
                //Final del For each correspondiente al identificador del extramo izquiero separado por "?"                
            }
            //Final del For each correspondiente a cada linea parseada del archivo de texto
        }

        private static void createXML()
        {
            bool banderaParte = false;
            bool ccTraslados = false;
            bool ccNoTraslados = true;
            bool ccRetenciones = false;
            List<string> Lineas = new List<string>();
            Lineas.Add("<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "UTF-8" + '"' + "?>");
            //Seccion del comprobante
            Lineas.Add("<cfdi:Comprobante ");
            foreach (var item in listaDatos)
            {
                if (item.strMostLeft == "co")
                {
                    Lineas.Add(item.strConcepto + "=" + '"' + item.strValor + '"');
                }
            }
            Lineas.Add("xmlns:xsi=" + '"' + "http://www.w3.org/2001/XMLSchema-instance" + '"');
            Lineas.Add("xmlns:cfdi=" + '"' + "http://www.sat.gob.mx/cfd/3" + '"');
            Lineas.Add("xsi:schemaLocation=" + '"' + "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv33.xsd" + '"' + ">");
            //Fin de la Seccion del comprobante

            //Inicio de la seccion Emisor
            Lineas.Add("<cfdi:Emisor");
            foreach (var item in listaDatos)
            {
                if (item.strMostLeft == "re")
                {
                    Lineas.Add(item.strConcepto + "=" + '"' + item.strValor + '"');
                }
            }
            Lineas.Add("/>");
            //Fin de la seccion Emisor

            //Inicio de la seccion Receptor
            Lineas.Add("<cfdi:Receptor");
            foreach (var item in listaDatos)
            {
                if (item.strMostLeft == "rr")
                {
                    Lineas.Add(item.strConcepto + "=" + '"' + item.strValor + '"');
                }
            }
            Lineas.Add("/>");
            //Final de la seccion Receptor

            //Inicio de la seccion Conceptos
            Lineas.Add("<cfdi:Conceptos>");
            int hello = 1;
            
                contadorElementosNivelActual = obtenerElementosDeCadaNivel(hello);
                foreach (var itemDato in listaDatos)
                {
                    if (itemDato.strMostLeft == "cc" && hello<=listaIdCC.Count)
                    {
                        if (itemDato.strConcepto == "ClaveProdServ")
                        {
                            Lineas.Add("<cfdi:Concepto");
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                        }
                        if (itemDato.strConcepto != "ClaveProdServ" && itemDato.strConcepto != "Descuento")
                        {
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                        }
                        if (itemDato.strConcepto == "Descuento")
                        {
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                            Lineas.Add(">");
                        }
                        if (contadorElementosNivelActual == 0 )
                        {                            
                            hello++;
                            contadorElementosNivelActual = obtenerElementosDeCadaNivel(hello);
                            Lineas.Add("</cfdi:Impuestos>");
                            Lineas.Add("</cfdi:Concepto>");
                        }


                    }
                    //Lineas.Add("<cfdi:Traslados>");
                    if (itemDato.strMostLeft == "ccit")
                    {
                        ccNoTraslados = false;
                        if (ccTraslados == false) {
                            Lineas.Add("<cfdi:Impuestos>");
                            Lineas.Add("<cfdi:Traslados>");
                            ccTraslados = true;
                        }                        
                        if (itemDato.strConcepto == "Base")
                        {                            
                            Lineas.Add("<cfdi:Traslado");
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                        }
                        if (itemDato.strConcepto != "Base" && itemDato.strConcepto != "Importe")
                        {
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                        }
                        if (itemDato.strConcepto == "Importe")
                        {
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                            Lineas.Add("/>");
                        }
                        if (contadorElementosNivelActual == 0 )
                        {
                            hello++;
                            contadorElementosNivelActual = obtenerElementosDeCadaNivel(hello);
                            Lineas.Add("</cfdi:Retenciones>");
                            Lineas.Add("</cfdi:Impuestos>");
                            Lineas.Add("</cfdi:Concepto>");                            
                        }
                    }
                    if (itemDato.strMostLeft == "ccir")
                    {
                        if (ccTraslados == true)
                        {
                            Lineas.Add("</cfdi:Traslados>");
                            ccTraslados = false;
                        }
                        if (ccTraslados == false && ccNoTraslados==true)
                        {
                            Lineas.Add("<cfdi:Impuestos>");
                            ccNoTraslados = true;
                        }
                        if (ccRetenciones == false)
                        {
                            Lineas.Add("<cfdi:Retenciones>");
                            ccRetenciones = true;
                        }    
                        if (itemDato.strConcepto == "Base" )
                        {
                            Lineas.Add("<cfdi:Retencion");
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                        }
                        if (itemDato.strConcepto != "Base" && itemDato.strConcepto != "Importe")
                        {
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                        }
                        if (itemDato.strConcepto == "Importe")
                        {
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            Lineas.Add("/>");
                            contadorElementosNivelActual--;
                        }
                        if (contadorElementosNivelActual == 0 )
                        {
                            hello++;
                            contadorElementosNivelActual = obtenerElementosDeCadaNivel(hello);
                            Lineas.Add("</cfdi:Retenciones>");
                            Lineas.Add("</cfdi:Impuestos>");
                            Lineas.Add("</cfdi:Concepto>");
                        }
                    }                    
                    if (itemDato.strMostLeft == "pt")
                    {
                        banderaParte = true;
                        if (ccTraslados == true)
                        {
                            Lineas.Add("</cfdi:Traslados>");
                            Lineas.Add("</cfdi:Impuestos>");
                            ccTraslados = false;
                        }
                        if (ccRetenciones == true)
                        {
                            Lineas.Add("</cfdi:Retenciones>");
                            Lineas.Add("</cfdi:Impuestos>");
                            ccRetenciones = false;
                        }           
                        if (itemDato.strConcepto == "ClaveProdServ")
                        {
                            Lineas.Add("<cfdi:Parte");
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                        }
                        if (itemDato.strConcepto != "ClaveProdServ" && itemDato.strConcepto != "Importe")
                        {
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            contadorElementosNivelActual--;
                        }
                        if (itemDato.strConcepto == "Importe")
                        {
                            Lineas.Add(itemDato.strConcepto + "=" + '"' + itemDato.strValor + '"');
                            Lineas.Add("/>");
                            contadorElementosNivelActual--;
                        }
                        if (contadorElementosNivelActual == 0)
                        {
                            hello++;
                            contadorElementosNivelActual = obtenerElementosDeCadaNivel(hello);
                            Lineas.Add("</cfdi:Concepto>");
                        }
                    }
                }
            
                
            Lineas.Add("</cfdi:Conceptos>");
            //Final de la seccion Conceptos

            //inicio de la seccion Impuestos de nivel concepto
            bool banderaImpuiestos = false;
            foreach (var itemLista in listaDatos)
            {
                if (itemLista.strMostLeft == "im")
                {
                    if (banderaParte == false)
                    {
                        Lineas.Add("</cfdi:Retenciones>");
                        Lineas.Add("</cfdi:Impuestos>");
                        Lineas.Add("</cfdi:Concepto>");
                    }
                    banderaImpuiestos = true;
                    if (itemLista.strMostLeft == "im" && itemLista.strConcepto == "TotalImpuestosRetenidos")
                    {
                        Lineas.Add("<cfdi:Impuestos");
                    }
                    if (itemLista.strMostLeft == "im")
                    {
                        Lineas.Add(itemLista.strConcepto + "=" + '"' + itemLista.strValor + '"');
                    }

                }
            }
            if (banderaImpuiestos==true)
            {
                Lineas.Add(">"); 
            }
            bool banderaIR = false;
            foreach (var itemLista in listaDatos)
            {
                if (itemLista.strMostLeft == "ir")
                {
                    if (banderaIR == false)
                    {
                        Lineas.Add("<cfdi:Retenciones>");
                    }
                    banderaIR = true;

                    if (itemLista.strMostLeft == "ir" && itemLista.strConcepto == "Impuesto" )
                    {
                        Lineas.Add("<cfdi:Retencion");
                    }
                    if (itemLista.strMostLeft == "ir")
                    {
                        Lineas.Add(itemLista.strConcepto + "=" + '"' + itemLista.strValor + '"');
                    }
                    if (itemLista.strMostLeft == "ir" && itemLista.strConcepto == "Importe" )
                    {
                        Lineas.Add("/>");
                    }
                }
            }

            if (banderaIR == true) { Lineas.Add("</cfdi:Retenciones>"); }
            bool banderaIT = false;
            foreach (var itemLista in listaDatos)
            {
                if (itemLista.strMostLeft == "it")
                {
                    if (banderaIT == false)
                    {
                        Lineas.Add("<cfdi:Traslados>");
                    }
                    banderaIT = true;

                    if (itemLista.strMostLeft == "it" && itemLista.strConcepto == "Impuesto")
                    {
                        Lineas.Add("<cfdi:Traslado");
                    }
                    if (itemLista.strMostLeft == "it")
                    {
                        Lineas.Add(itemLista.strConcepto + "=" + '"' + itemLista.strValor + '"');
                    }
                    if (itemLista.strMostLeft == "it" && itemLista.strConcepto == "TipoFactor")
                    {
                        Lineas.Add("/>");
                    }
                }
            }
            if (banderaIT == true) { Lineas.Add("</cfdi:Traslados>"); }


            if (banderaImpuiestos == true) { Lineas.Add("</cfdi:Impuestos>"); }
            //Lineas.Add("</cfdi:Impuestos>");
            Lineas.Add("</cfdi:Comprobante>");
            //Final del final, de todos los tiempos
            System.IO.File.WriteAllLines(rutaXML + "WriteLines" + contadorLayouts + ".xml", Lineas);
            
        }

        private static List<int> generateList()
        {
            List<int> listaProvisional = new List<int>();
            int hola = 0;
            foreach (var item in listaDatos)
            {
                Console.WriteLine("     Concepto Base:      " + item.strMostLeft);
                Console.WriteLine("     Concepto:           " + item.strConcepto);
                Console.WriteLine("     Valor:              " + item.strValor);
                Console.WriteLine("     Id:                 " + item.strId);
                Console.WriteLine("-------------------------------------------------------------------------------------");

                if (item.strMostLeft == "cc")
                {
                    listaIdCC.Add(item.strId);
                }
            }
            //Console.ReadKey(); 
            foreach (var idCC in listaIdCC)
            {
                if (listaProvisional.Count == 0)
                {
                    listaProvisional.Add(idCC);
                    hola = listaProvisional.First();
                }
                else
                {
                    int index = listaProvisional.IndexOf(hola);
                    if (hola != idCC)
                    {
                        listaProvisional.Add(idCC);
                        index = index + 1;
                        hola = listaProvisional.ElementAt(index);
                    }
                }
            }

            return listaProvisional;
        }
        
        private static List<int> generateRelations()
        {
            List<int> listaProvisional = new List<int>();
            int hola = 0;
            foreach (var item in listaDatos)
            {
                if (item.strMostLeft == "cc")
                {
                    listaElementosNivelUno.Add(item.strId);
                }
                if (item.strMostLeft == "im")
                {
                    listaElementosNivelUno.Add(item.strId);
                }
            }

            foreach (var idCC in listaElementosNivelUno)
            {
                if (listaProvisional.Count == 0)
                {
                    listaProvisional.Add(idCC);
                    hola = listaProvisional.First();
                }
                else
                {
                    int index = listaProvisional.IndexOf(hola);
                    if (hola != idCC)
                    {
                        listaProvisional.Add(idCC);
                        index = index + 1;
                        hola = listaProvisional.ElementAt(index);
                    }
                }
            }

            return listaProvisional;
        }

        private static int obtenerElementosDeCadaNivel(int nivel)
        {
            
            int listaProvisional = 0;

            if (nivel < listaElementosNivelUno.Count && nivel>0)
            {
                foreach (var item2 in listaDatos)
                {                    
                    if (item2.strId < listaElementosNivelUno.ElementAt(nivel) && item2.strId >= listaElementosNivelUno.ElementAt(nivel-1))
                    {
                        listaProvisional = listaProvisional + 1;
                    }                   
                }
            }
            if (nivel == listaElementosNivelUno.Count && nivel > 0)
            {
                foreach (var item2 in listaDatos)
                {
                    if (item2.strId > listaElementosNivelUno.ElementAt(nivel-1))
                    {
                        listaProvisional = listaProvisional + 1;
                    }
                }
            }   
            return listaProvisional;
        }

        private static void executeCommand(String commandR)
        {
            try
            {
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c " + commandR,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false
                    }
                };
                proc.Start();
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                }
            }
            catch (Exception e)
            {
            }
        }

        private static void crearCadenaOriginal()
        {
            string LineasCO = String.Empty;
            XmlDocument doc = new XmlDocument();
            doc.Load(rutaXML + "WriteLines"+contadorLayouts+".xml");
            XslTransform trans = new XslTransform();
            trans.Load(rutaESQ + "cadenaoriginal_3_3.xslt");
            XmlReader rdr = trans.Transform(doc, null);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(rutaTXT + "cadenaOriginal" + contadorLayouts + ".txt"))
            {
                while (rdr.Read())
                {
                    file.Write(rdr.Value);
                }
            }
        }

        private static void insertarNoCertificado()
        {

            // The path to the certificate.
            string Certificate = rutaCER + "CSD_Pruebas_CFDI_MAG041126GT8.cer";

            // Load the certificate into an X509Certificate object.
            X509Certificate cert = X509Certificate.CreateFromCertFile(Certificate);

            // Get the value.
            string resultsTrue = cert.ToString(true);

            String hola1 = cert.GetSerialNumberString();

            char[] ch = hola1.ToCharArray();
            string salida = "";
            for (int i = 0; i < ch.Length; i++)
            {
                if (i % 2 != 0)
                {
                    //Console.Write(ch[i].ToString());
                    salida = salida + ch[i];
                }
            }
            noCertificado = salida;
            string path = rutaXML + "WriteLines" + contadorLayouts + ".xml";
            // Read a text file line by line.  
            var lines = File.ReadAllLines(path);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf("Fecha") != -1 && lines[i + 1].IndexOf("NoCertificado") == -1)
                {
                    //Console.WriteLine(i);
                    var lineToAdd = "NoCertificado=" + '"' + salida + "" + '"';
                    var txtLines = lines.ToList();
                    txtLines.Insert(i + 1, lineToAdd);  //Insert the line you want to add last under the tag 'item1'.
                    File.WriteAllLines(path, txtLines);
                }
            }
        }

        private static void insertarSello()
        {
            string pathXML = rutaXML + "WriteLines" + contadorLayouts + ".xml";
            string pathSello = rutaTXT + "sello" + contadorLayouts + ".txt";
            // Read a text file line by line.  
            var lines = File.ReadAllLines(pathXML);
            var linesSello = File.ReadAllLines(pathSello);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf("Fecha") != -1)
                {
                    var lineToAdd = "Sello=" + '"' + linesSello.GetValue(0) + '"';
                    var txtLines = lines.ToList();
                    txtLines.Insert(i + 1, lineToAdd);  //Insert the line you want to add last under the tag 'item1'.
                    File.WriteAllLines(pathXML, txtLines);
                }
            }
            //Console.ReadKey();
        }

        private static void ExportCertToBase64()
        {
            var certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(rutaCER + "CSD_Pruebas_CFDI_MAG041126GT8.cer"
                );
            StringBuilder builder = new StringBuilder();
            //.Export(X509ContentType.Cert))
            builder.AppendLine(Convert.ToBase64String(certificate.GetRawCertData()));
            string test2 = Convert.ToBase64String(certificate.Export(X509ContentType.SerializedCert));
            string test = Convert.ToBase64String(certificate.GetRawCertData());
            certificadoBase64 = test;

        }

        private static void insertarCertificado()
        {
            string pathXML = rutaXML + "WriteLines" + contadorLayouts + ".xml";
            var lines = File.ReadAllLines(pathXML);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf("NoCertificado") != -1)
                {
                    var lineToAdd = "Certificado=" + '"' + certificadoBase64 + '"';
                    var txtLines = lines.ToList();
                    txtLines.Insert(i + 1, lineToAdd);  //Insert the line you want to add last under the tag 'item1'.
                    File.WriteAllLines(pathXML, txtLines);
                }
            }
        }

        private static void validarXML()
        {
            string salida = "Todo salio bien hasta desmostrar lo contrario";
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add("http://www.sat.gob.mx/cfd/3", rutaESQ + "cfdv33.xsd");
                settings.ValidationType = ValidationType.Schema;
                XmlReader reader = XmlReader.Create(rutaXML + "WriteLines"+contadorLayouts+".xml", settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);
                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                document.Validate(eventHandler);
            }
            catch (Exception ex)
            {
                salida = ex.Message;
                Console.WriteLine("");
                Console.WriteLine("-------------------------------------------La validacion regresa lo siguente:-------------------------------------------");
                Console.WriteLine(ex.Message);
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
            }
            string pathResult = rutaXML + "Result" + contadorLayouts + ".xml";
            File.WriteAllText(pathResult, salida);
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
            }
        }

        private static void peticionSOAP()
        {
            string pathXML = rutaXML + "WriteLines" + contadorLayouts + ".xml";
            string pathResult = rutaXML + "Result" + contadorLayouts + ".xml";
            string txtEvaluar = File.ReadAllText(pathXML);
            Test.wcfRecepcionASMXSoapClient servicioXML = new Test.wcfRecepcionASMXSoapClient();
            string resultado = servicioXML.fnEnviarXML(txtEvaluar, "01", 0, "WSDL_PAX", "wqrCssOUw4HDgMSUxJTDq8OkwrQXMnBpSS4Ocm/Cve+/te+9tu++me+/tiEc776v776B", "3.3");
            servicioXML.Close();
            File.WriteAllText(pathResult, resultado);
        }

        private static void peticionCanceladoSOAP(string sListaUUID, string psRFC, string psRFCReceptor, string sListaTotales, string sNombre, string sContrasena)
        {    
            ArrayOfString UUID = new ArrayOfString();
            UUID.Add(sListaUUID);
            ArrayOfString RFCReceptor = new ArrayOfString();
            RFCReceptor.Add(sListaUUID);
            ArrayOfString ListaTotales = new ArrayOfString();
            ListaTotales.Add(sListaUUID);

            string pathResultadoPositivo = rutaCEF + "ResultadoCancelacionEfectiva" + contadorCancelaciones + ".xml";
            string pathResultadoNegativo = rutaCNE + "ResultadoCancelacionNegativa" + contadorCancelaciones + ".xml";

            Cancel.wcfCancelaASMXSoapClient servicioCancelacion = new Cancel.wcfCancelaASMXSoapClient();

            string resultado = servicioCancelacion.fnCancelarXML(UUID, psRFC, RFCReceptor, ListaTotales, sNombre, sContrasena);
            if (resultado.Contains("<UUIDEstatus>103</UUIDEstatus>") || resultado.Contains("<UUIDEstatus>205</UUIDEstatus>") || resultado.Contains("<UUIDEstatus>107</UUIDEstatus>") || resultado.Contains("<UUIDEstatus>201</UUIDEstatus>") || resultado.Contains("<UUIDEstatus>202</UUIDEstatus>"))
            {
                File.WriteAllText(pathResultadoPositivo, resultado);
            }
            else {
                File.WriteAllText(pathResultadoNegativo, resultado);
            }
            
            contadorCancelaciones++;
        }

        private static void leerFolderCancelacion()
        {
            string path = "C:\\PAXConector-Test\\CancelacionEntrada\\";
            string user = "wsdl_pax";
            string password = "wrnDgcOvxYXEr8OKw6jDm8WDxYXCgzV5xLTEgMKoXk/EjcK5776k77+V77+QMu++qe++s++9se+8kw==";
            string tempID =String.Empty;
            string tempRFCEmisor = String.Empty;
            string tempRFCReceptor = String.Empty;
            string tempTotal = String.Empty;
            int countRFC = 0;
            foreach (var item in readAllFiles(path))
            {
                string[] lines = File.ReadAllLines(item);   
                foreach (string line in lines)
                {
                    String[] spearator = { "|" };
                    Int32 count = 5;
                    String[] strlist = line.Split(spearator, count, StringSplitOptions.RemoveEmptyEntries);
                    Console.WriteLine("---------------------------------------------------");
                    foreach (var itemMostLeft in strlist)
                    {
                        if (itemMostLeft.Contains("-"))
                        {
                            tempID  = itemMostLeft;                            
                        }
                        if ((itemMostLeft.Length==12 || itemMostLeft.Length==13) && countRFC==0)
                        {
                            tempRFCEmisor = itemMostLeft;
                            countRFC++;
                        }
                        if ((itemMostLeft.Length == 12 || itemMostLeft.Length == 13) && countRFC == 1)
                        {
                            tempRFCReceptor = itemMostLeft;
                        }
                        else {
                            tempTotal =  itemMostLeft;
                        }
                    }
                    Console.WriteLine("---------------------------------------------------");
                    peticionCanceladoSOAP(tempID, tempRFCEmisor, tempRFCReceptor, tempTotal, user, password);
                    countRFC = 0;
                }
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e) {
        // Specify what is done when a file is changed, created, or deleted.
            leerFolderCancelacion();
        }
        
        private static void crearPDF(){
        
        }

        #endregion    
    
    
    }
}

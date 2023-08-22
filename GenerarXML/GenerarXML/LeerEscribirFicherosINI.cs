using GenerarXML.localhost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace GenerarXML
{
    public static class LeerEscribirFicherosINI
    {


        public static void LeerEsciribirFichero()
        {
            try
            {
                // Leer contenido del archivo TXT
                using (StreamReader sr = new StreamReader("C:/encriptar/archivo.txt"))
                {
                    string line;
                    // Crear factura por cada linea del archivo.
                    while ((line = sr.ReadLine()) != null)
                    {


                        CrearFactura(line);
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                WriteErrorLog("El archivo no se pudo leer:");
                WriteErrorLog(e.Message);
            }
        }

        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }

        public static void CrearFactura(string Line)
        {
            try
            {
                string[] arrDatos = Line.Split('|');
                string fecha = arrDatos[0];
                string rfcEmisor = arrDatos[1];
                string rfcReceptor = arrDatos[2];
                string uuid = arrDatos[3];

                if (!RFC.EsRFCValido(rfcEmisor))
                {
                    WriteErrorLog("RFC EMISOR NO VALIDO");
                    return;
                }

                if (!RFC.EsRFCValido(rfcReceptor))
                {
                    WriteErrorLog("RFC RECEPTOR NO VALIDO");
                    return;
                }

                DateTime temp;
                if (!DateTime.TryParse(fecha,out temp))
                {
                    WriteErrorLog("998- La fecha de emisión no corresponde al formato UTC");
                    return;
                }

                XmlDocument doc = new XmlDocument();

                //(1) the xml declaration is recommended, but not mandatory
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                //(2) string.Empty makes cleaner code
                XmlElement element1 = doc.CreateElement(string.Empty, "Factura", string.Empty);
                doc.AppendChild(element1);

                XmlElement element2 = doc.CreateElement(string.Empty, "datos", string.Empty);
                element1.AppendChild(element2);

                XmlElement element3 = doc.CreateElement(string.Empty, "fecha", string.Empty);
                XmlText text1 = doc.CreateTextNode(fecha);
                element3.AppendChild(text1);
                element2.AppendChild(element3);

                XmlElement element4 = doc.CreateElement(string.Empty, "rfcEmisor", string.Empty);
                XmlText text2 = doc.CreateTextNode(rfcEmisor);
                element4.AppendChild(text2);
                element2.AppendChild(element4);

                XmlElement element5 = doc.CreateElement(string.Empty, "rfcReceptor", string.Empty);
                XmlText text3 = doc.CreateTextNode(rfcReceptor);
                element5.AppendChild(text3);
                element2.AppendChild(element5);

                XmlElement element6 = doc.CreateElement(string.Empty, "uuid", string.Empty);
                XmlText text4 = doc.CreateTextNode(uuid);
                element6.AppendChild(text4);
                element2.AppendChild(element6);

                doc.Save("C:/encriptar/factura.xml");
                TimbrarFactura operacion = new TimbrarFactura();
                WriteErrorLog(operacion.fnGuardaFactura("C:/encriptar/factura.xml"));


            }
            catch (SystemException e)
            {
                WriteErrorLog("Hubo un error:" + e.Message);
            }
        }
    }

    
}

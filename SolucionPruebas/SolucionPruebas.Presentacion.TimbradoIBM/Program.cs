using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SolucionPruebas.Presentacion.TimbradoIBM
{
    class Program
    {
        static void Main(string[] args)
        {
            var listaArchivos = RecuperaListaArchivos(@"D:\PAXRegeneracionBateria\Archivos Generados\");

            foreach (string nombreArchivo in listaArchivos)
            {
                string sResultado = string.Empty;
                XmlDocument xDocTimbrado = new XmlDocument();
                xDocTimbrado.Load(nombreArchivo);

                Console.WriteLine(Path.GetFileNameWithoutExtension(nombreArchivo));

                wsTimbrado.wcfRecepcionASMXSoapClient wsServicio = new wsTimbrado.wcfRecepcionASMXSoapClient();
                sResultado = wsServicio.fnEnviarXML(xDocTimbrado.InnerXml, "factura", 0, "paxgeneracion", "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB", "3.2");

                //wsTimbradoASPEL.wcfRecepcionASPELSoapClient wsServicio = new wsTimbradoASPEL.wcfRecepcionASPELSoapClient();
                //sResultado = wsServicio.fnEnviarXML(xDocTimbrado.InnerXml, "factura", 0, "paxgeneracion", "UHJ1ZWJhMTIr", "3.2");

                //wsTimbradoSVC.wcfRecepcion wsServicio = new wsTimbradoSVC.wcfRecepcion();
                //sResultado = wsServicio.fnEnviarXML(xDocTimbrado.InnerXml, "factura", 0, false, "paxgeneracion", "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB", "3.2");

                Console.WriteLine(sResultado);
                //Console.ReadLine();

                xDocTimbrado.LoadXml(sResultado);

                File.WriteAllText(@"D:\PAXRegeneracionBateria\Salida\" + Path.GetFileNameWithoutExtension(nombreArchivo) + ".txt", sResultado);
            }
        }

        public static IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.EnumerateFiles(directorioRaiz, "*.xml").ToList();
            return listaArchivos;
        }
    }
}

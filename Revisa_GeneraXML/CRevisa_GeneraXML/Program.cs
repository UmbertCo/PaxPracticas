using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Xsl;
using System.Xml.XPath;
using Revisa_GeneraXML;
using System.Xml.Linq;
using ReportSamples;




namespace CRevisa_GeneraXML
{
    class Program
    {
       

        static void Main(string[] args)
        {
            Ejercicios.crear_PDF();


            
        }

        static void booksSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.Write("WARNING: \n- ");
                Console.WriteLine(e.Message);
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.Write("ERROR: \n- ");
                Console.WriteLine(e.Message);
            }
        }

    }
}

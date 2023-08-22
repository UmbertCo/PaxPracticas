using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Xsl;
using System.Xml;

namespace Revisa_GeneraXML
{
    class Transformador
    {
            //       XslCompiledTransform xctTransformar = new XslCompiledTransform();

            //xctTransformar.Load(@"C:\Users\Ismael Hidalgo\Desktop\Transformacion3_2\Transformacion3_2\cadenaoriginal_3_2.xslt");

            //StringBuilder resultString = new StringBuilder();

            //StringWriter txtw = new StringWriter(resultString);

            //xctTransformar.Transform(@"C:\Revisa_Genera\Validos\prueba2.xml", null, txtw);

        XslCompiledTransform xctTransformador;

        string sRutaXml;

        StringWriter swEscritor;

        StringBuilder sbCadenaConstruida;


        public Transformador(string psrutaXsl, string psrutaXml)
        {
            xctTransformador = new XslCompiledTransform();

            xctTransformador.Load(psrutaXsl);

            this.sRutaXml = psrutaXml;

            sbCadenaConstruida = new StringBuilder();

            swEscritor = new StringWriter(sbCadenaConstruida);

            fntransformar();
        }

        private void fntransformar() 
        {

            xctTransformador.Transform(sRutaXml, null, swEscritor);
        
        }

        public String sCadenaOriginal 
        {

            get 
            {

                return sbCadenaConstruida.ToString();       
            
            }
        
        }

    }
}

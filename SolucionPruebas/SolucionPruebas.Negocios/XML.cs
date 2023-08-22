using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SolucionPruebas.Negocios
{
    public class XML
    {
        public XML()
        { 
        
        }

        public string fnAplicarHojaTransformacion(string psDocumento)
        {
            string sResultado = string.Empty;
            MemoryStream ms;
            StreamReader srDll;
            XsltArgumentList args;
            XslCompiledTransform xslt;
            XmlDocument documento = new XmlDocument();
            try
            {
                documento.LoadXml(psDocumento);

                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOri.V32));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(documento.CreateNavigator(), args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);
                sResultado = srDll.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            return sResultado;
        }
    }
}

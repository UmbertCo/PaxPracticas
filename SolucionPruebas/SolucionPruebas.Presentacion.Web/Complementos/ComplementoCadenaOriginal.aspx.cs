using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace SolucionPruebas.Presentacion.Web.Complementos
{
    public partial class ComplementoCadenaOriginal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            XmlDocument document = new XmlDocument();
            XmlDocument xdComplemento = new XmlDocument();
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            string sResultado = string.Empty;
            string sCadenaOriginal = string.Empty;
            string sSello = string.Empty;
            try
            {
                HttpFileCollection hfc = Request.Files;
                HttpPostedFile hpf = hfc[0];
                HttpPostedFile hpfComplemento = hfc[1];

                if (hpf.ContentLength < 0)
                    return;

                if (hpfComplemento.ContentLength < 0)
                    return;

                document.Load(hpf.InputStream);
                xdComplemento.Load(hpfComplemento.InputStream);

                sCadenaOriginal = fnConstruirCadenaTimbrado(document.CreateNavigator(), xdComplemento.InnerXml);
            }
            catch (Exception ex)
            {

            } 
        }

        /// <summary>
        /// Contruye la cadena original a partir de un XML de CFDI
        /// </summary>
        /// <param name="xml">Objeto navegador del XML</param>
        /// <returns>Retorna la cadena original</returns>
        public string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psComplemento)
        {
            string sCadenaOriginal = string.Empty;

            try
            {
                MemoryStream ms = new MemoryStream();
                XslCompiledTransform trans = new XslCompiledTransform();
                trans.Load(XmlReader.Create(new StringReader(psComplemento)));
                XsltArgumentList args = new XsltArgumentList();
                trans.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                sCadenaOriginal = sr.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            return sCadenaOriginal;
        }
    }
}
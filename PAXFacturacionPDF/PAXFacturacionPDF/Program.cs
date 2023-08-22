using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Xml.Xsl;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System.Threading;
using System.Globalization;
using System.Drawing;
using PAXFacturacionPDF.Properties;
namespace PAXFacturacionPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            
            try
                    {
                string sCadenaOriginal = string.Empty;
                string filtro = "*.xml";
                string[] Files = Directory.GetFiles(Settings.Default.rutaDocs, filtro);
                XmlDocument xmlReport = new XmlDocument();
                string sText = string.Empty;
              

                foreach (string archivo in Files)
                {
                    try
                    {
                        rptGenericaGratuita33 report = new rptGenericaGratuita33();
                        string sNombre = Path.GetFileNameWithoutExtension(archivo);

                        xmlReport.Load(archivo);
                        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlReport.NameTable);
                        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                        
                        XmlNodeList listaConcepto = xmlReport.SelectNodes("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsmComprobante);

                        foreach (XmlNode node in listaConcepto)
                        {
                            XmlNode nodo = xmlReport.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto/cfdi:Impuestos", nsmComprobante);
                            RenameNode(nodo, "cfdi:ImpuestosConcepto");
                        }
                        
                        xmlReport.Save(Settings.Default.rutaTemporal + sNombre + ".xml");

                        report.XmlDataPath = Settings.Default.rutaTemporal + sNombre + ".xml";

                        PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

                        pdfOptions.Compressed = true;
                        pdfOptions.ConvertImagesToJpeg = false;

                        

                        try { sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(xmlReport.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_1") + "||"; }
                        catch { }

                        XRTableCell xrtblCellCadenaOriginal = (XRTableCell)report.FindControl("CellCadenaOriginal", true);
                        xrtblCellCadenaOriginal.Text = sCadenaOriginal;

                        pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
                        pdfOptions.DocumentOptions.Title = "CFDI";

                        report.Name = sNombre;

                        report.ExportToPdf(Settings.Default.rutaPDFs + sNombre + ".pdf");
                        File.Delete(Settings.Default.rutaTemporal + sNombre + ".xml");
                    }
                    catch (System.Xml.XmlException ex)
                    {

                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {

                    }
                    catch (System.Threading.ThreadAbortException)
                    {

                    }
                    catch (Exception ex)
                    {

                    }
                }
                    }
            catch (System.Exception ex)
            {

            }
            
        }
        public static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
        {
            string sCadenaOriginal = string.Empty;
            try
            {
                MemoryStream ms = new MemoryStream();
                XslCompiledTransform trans = new XslCompiledTransform();
                trans.Load(typeof(Timbrado.V3.TFD11XSLT));
                XsltArgumentList args = new XsltArgumentList();
                trans.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                sCadenaOriginal = sr.ReadToEnd();
            }
            catch (Exception)
            {
                //LOGO DE ERROR
            }

            return sCadenaOriginal;
        }
        public static XmlNode RenameNode(XmlNode e, string newName)
        {
            XmlDocument doc = e.OwnerDocument;
            XmlNode newNode = doc.CreateNode(e.NodeType, newName, "http://www.sat.gob.mx/cfd/3");
            newNode.Prefix = "cfdi";
            while (e.HasChildNodes)
            {
                newNode.AppendChild(e.FirstChild);
            }
            XmlAttributeCollection ac = e.Attributes;
            while (ac.Count > 0)
            {
                newNode.Attributes.Append(ac[0]);
            }
            XmlNode parent = e.ParentNode;
            parent.ReplaceChild(newNode, e);
            return newNode;
        }
    }

}

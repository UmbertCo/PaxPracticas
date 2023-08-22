using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Xml.XPath;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using PAXConectorFTPGTCFDI33.Properties;


namespace PAXConectorFTPGTCFDI33
{

    public class clsPlantilla
    {
        private XmlDocument gXmlComprobante;

        public clsPlantilla(XmlDocument pXmlComprobante)
        {
            gXmlComprobante = pXmlComprobante;
        }

        private static XmlNode RenameNode(XmlNode e, string newName)
        {
            XmlDocument doc = e.OwnerDocument;
            XmlNode newNode = doc.CreateNode(e.NodeType, newName, "http://www.sat.gob.mx/cfd/4");
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

        private static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
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

        public bool fnGenerarPdf(string sLogo, string nombrePDF, string nombreXML)
        {
            try
            {

                string sXmlReportPath = Settings.Default.FolderSalida + @"\" + System.IO.Path.GetFileNameWithoutExtension(nombreXML) + "2.xml";

                string sCadenaOriginal = string.Empty;

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gXmlComprobante.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                XmlNodeList listaConcepto = gXmlComprobante.SelectNodes("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsmComprobante);

                try
                {
                    foreach (XmlNode node in listaConcepto)
                    {
                        XmlNode nodo = gXmlComprobante.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto/cfdi:Impuestos", nsmComprobante);
                        RenameNode(nodo, "cfdi:ImpuestosConcepto");
                    }
                    sCadenaOriginal = "|" + fnConstruirCadenaTimbrado(gXmlComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; 
                }
                catch { }

                gXmlComprobante.Save(sXmlReportPath);

                rptGenericaGratuita33 report = new rptGenericaGratuita33();
                try
                {
                    report.XmlDataPath = sXmlReportPath;

                    //report.Parameters["pColor"].Value = Properties.Settings.Default.Color;
                    //report.Parameters["pTipoDocumento"].Value = "Factura";
                    report.Parameters["pLinkSAT"].Value = Properties.Settings.Default.LinkSAT;

                    PdfExportOptions pdfOptions = report.ExportOptions.Pdf;

                    pdfOptions.Compressed = true;
                    pdfOptions.ConvertImagesToJpeg = false;

                    XRTableCell xrtblCellCadenaOriginal = (XRTableCell)report.FindControl("CellCadenaOriginal", true);
                    xrtblCellCadenaOriginal.Text = sCadenaOriginal;

                    pdfOptions.DocumentOptions.Author = "CORPUS Facturacion";
                    pdfOptions.DocumentOptions.Title = "CFDI 3.3"; 
                    report.ExportToPdf(nombrePDF);

                }
                catch (Exception ex)
                {
                    clsLog.WriteLine("Error al Generar el PDF " + DateTime.Now + ex.Message);
                    clsLogRespaldo.WriteLine("Error al Generar el PDF  " + DateTime.Now + ex.Message);
                }
                finally
                {
                    report.Dispose();
                } 
                return true; 
            }

            catch (Exception ex)
            {
                
                clsLog.WriteLine("Error al Generar el PDF " + DateTime.Now + ex.Message);
                clsLogRespaldo.WriteLine("Error al Generar el PDF  " + DateTime.Now + ex.Message);
                return false;

            }
        }
    }
}



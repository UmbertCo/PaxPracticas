using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Net;
using System.Net.Security;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using System.Data.SqlClient;
using ConectorPAXGenericoPDF.Properties;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml.XPath;
using Utilerias;
using System.Xml.Xsl;
using System.Net.NetworkInformation;
using System.Management;
using Microsoft.Win32;

public class clsTimbradoTxt
{
    public ConectorPAXGenericoPDF.wsRecepcionASMX.wcfRecepcionASMX wsRecepcion = new ConectorPAXGenericoPDF.wsRecepcionASMX.wcfRecepcionASMX();

    public void fnTimbradoTXT()
    {
        try
        {

            string sTXT, sXML, snombreDoc;
            DateTime Fecha = DateTime.Today;
            sTXT = sXML = snombreDoc = string.Empty;
            string psTipoDocumento = ConectorPAXGenericoPDF.Properties.Settings.Default["tipodocto"].ToString();
            char[] cCad = { '-' };
            string[] sCad;
            //Se obtiene lista de txt almacenados en carpeta para timbrar
            string[] files = Directory.GetFiles(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocs"].ToString(), "*.txt");

            foreach (string sText in files)
            {
                int nBan = 0;
                //Se obtiene archivo txt de la ruta correspondiente
                Stream archivoTXT = File.Open(sText, FileMode.Open);
                //Nombre de archivo 
                string sNombreTXT = Path.GetFileNameWithoutExtension(sText);
                StreamReader srTXT = new StreamReader(archivoTXT);
                sTXT = srTXT.ReadToEnd();

                string sVersion = null;
                string lineaVersion = string.Empty;
                string[] atributosVersionSeccion1 = null;
                string[] seccionVersion = null;

                //Se obtiene version desde TXT
                StringReader lectorVersion = new StringReader(sTXT);
                while (true)
                {
                    lineaVersion = lectorVersion.ReadLine();
                    if (string.IsNullOrEmpty(lineaVersion))
                        break;

                    seccionVersion = lineaVersion.Split('?');

                    try
                    {

                        atributosVersionSeccion1 = seccionVersion[1].Split('|');

                        switch (seccionVersion[0])
                        {
                            case "co":
                                foreach (string arreglo in atributosVersionSeccion1)
                                {
                                    if (arreglo.Contains("version"))
                                    {
                                        string[] sversion = arreglo.Split('@');
                                        sVersion = sversion[1];
                                    }
                                }

                                break;
                        }
                    }
                    catch
                    {

                    }
                }

                //Se manda txt a generar y timbrar
                sXML = wsRecepcion.fnEnviarTXT(sTXT, psTipoDocumento, 0, ConectorPAXGenericoPDF.Properties.Settings.Default["usuario"].ToString(),
                                        ConectorPAXGenericoPDF.Properties.Settings.Default["password"].ToString(), sVersion, "");
                //Se valida el tipo de respuesta
                sCad = sXML.Split(cCad);
                if (sCad.Length <= 2)
                {
                    try
                    {
                        nBan = 1; //Se indica que el comprobante no fue timbrado
                        //En caso de marcar error se graba un log
                        string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogErrorSinTimbrar" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                        if (!File.Exists(path))
                        {
                            StreamWriter sr4 = new StreamWriter(path);
                            sr4.WriteLine(DateTime.Now + " " + sXML + ", Nombre txt: " + sNombreTXT);
                            sr4.Close();
                        }
                        else
                        {
                            System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                            sw4.WriteLine(DateTime.Now + " " + sXML + ", Nombre txt: " + sNombreTXT);
                            sw4.Close();
                        }

                    }
                    catch
                    {
                    }
                }

                srTXT.Close();
                archivoTXT.Close();
                //Si el documento fue timbrado se guarda el XML en ruta definida
                if (nBan == 0)
                {
                    //Se obtiene el xml
                    XmlDocument xXML = new XmlDocument();
                    //string withOutEncoding = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                    xXML.LoadXml(sXML);

                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xXML.NameTable);
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                    XPathNavigator navEncabezado = xXML.CreateNavigator();
                    try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                    catch { snombreDoc = Guid.NewGuid().ToString(); }

                    //Se guarda XML en ruta especificada 
                    xXML.Save(ConectorPAXGenericoPDF.Properties.Settings.Default["RutaDocZips"].ToString() + snombreDoc + ".xml");

                    try
                    {
                        //Generar PDF
                        string sRutaPDF = ConectorPAXGenericoPDF.Properties.Settings.Default["RutaDocZips"].ToString();

                        fnCrearPLantillaEnvio(xXML, psTipoDocumento, sRutaPDF + snombreDoc + ".pdf");
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            DateTime Fechaex = DateTime.Today;
                            string pathex = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";

                            if (!File.Exists(pathex))
                            {
                                StreamWriter sr4 = new StreamWriter(pathex);
                                sr4.WriteLine(DateTime.Now + " " + ex.Message);
                                sr4.Close();
                            }
                            else
                            {
                                System.IO.StreamWriter sw4 = new System.IO.StreamWriter(pathex, true);
                                sw4.WriteLine(DateTime.Now + " " + ex.Message);
                                sw4.Close();
                            }
                        }
                        catch
                        {
                        }
                    }

                    //Se guarda log de comprobantes timbrados
                    string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogTimbrados"] + "LogTimbrados" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                    if (!File.Exists(path))
                    {
                        StreamWriter sr4 = new StreamWriter(path);
                        sr4.WriteLine(DateTime.Now + ", Nombre txt: " + sNombreTXT);
                        sr4.Close();
                    }
                    else
                    {
                        System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                        sw4.WriteLine(DateTime.Now + ", Nombre txt: " + sNombreTXT);
                        sw4.Close();
                    }
                    //Copia el archivo txt timbrado a otra carpeta
                    File.Copy(sText, ConectorPAXGenericoPDF.Properties.Settings.Default["rutaTXTGen"].ToString() + sNombreTXT + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(sText);
                }
                else
                {
                    //Si el txt es invalido
                    //Copia el archivo txt invalido a otra carpeta
                    File.Copy(sText, ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(sText);
                }
            }
        }
        catch (Exception ex)
        {
            try
            {
                DateTime Fecha = DateTime.Today;
                string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                if (!File.Exists(path))
                {
                    StreamWriter sr4 = new StreamWriter(path);
                    sr4.WriteLine(DateTime.Now + " " + ex.Message);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                    sw4.WriteLine(DateTime.Now + " " + ex.Message);
                    sw4.Close();
                }
            }
            catch
            {
            }
        }
    }


    /// <summary>
    /// Crea archivo pdf segun plantilla configurada para su posterior envio de correo
    /// </summary>
    /// <param name="pxComprobante"></param>
    /// <param name="psTipoDocumento"></param>
    /// <param name="sRuta"></param>
    public void fnCrearPLantillaEnvio(XmlDocument pxComprobante, string psTipoDocumento, string sRuta)
    {

        if (!(sRuta == string.Empty))
        {
            clsOperacionConsultaPdf pdf = new clsOperacionConsultaPdf(pxComprobante);

            if (!string.IsNullOrEmpty(psTipoDocumento))
                pdf.TipoDocumento = psTipoDocumento.ToUpper();
            pdf.fnGenerarPDFSave(sRuta, "Black");

        }
    }
}


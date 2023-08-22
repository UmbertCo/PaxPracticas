using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Security.Cryptography;
using System.Drawing.Printing;
using System.Diagnostics;
using OpenSSL_Lib;
using ConectorPAXGenericoPDF;


public class clsTimbradoGeneracion
{   
    public ConectorPAXGenericoPDF.wsRecepcionASMX.wcfRecepcionASMX wsRecepcion = new ConectorPAXGenericoPDF.wsRecepcionASMX.wcfRecepcionASMX();
    X509Certificate2 certEmisor = new X509Certificate2();
    

    public void fnTimbradoGeneracion(string stipoServicio)
    {
        try
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(clsTimbradoGeneracion.AcceptAllCertificatePolicy);

            //Funcion para formar XML desde *.txt
            fnEnviarTXT(stipoServicio);
        }
        catch (Exception ex)
        {
            try
            {
                DateTime Fecha = DateTime.Today;
                string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                clsLog.Escribir(path, DateTime.Now + " " + ex.Message);
            }
            catch
            {
            }
        }
    }

    public void fnEnviarTXT(string stipoServicio)
    {
        string printerName = "";
        string correo = "";
        string[] Files = null;
        string RutaXMLDocs = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocs"];
        string filtro = "*.txt";
        Files = Directory.GetFiles(RutaXMLDocs, filtro);
        DateTime Fecha = DateTime.Today;

        foreach (string archivo in Files)
        {
            int nBan = 0;
            System.IO.StringReader lectorVersion;
            System.IO.StringReader lector;
            string sVersion = null;
            string sRFC = null;
            string noCertificado = string.Empty;
            string sNombreTXT = Path.GetFileNameWithoutExtension(archivo);
            string text = string.Empty;

            using (Stream stream = File.Open(archivo.ToString(), FileMode.Open))
            {
                StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8, true);
                text = sr.ReadToEnd();
                lector = new System.IO.StringReader(text);
                lectorVersion = new System.IO.StringReader(text);
            }

            string linea = string.Empty;
            string[] seccion = null;
            string[] atributos = null;
            string sXmlDocument = string.Empty;
            string sRetAutentication = string.Empty;
            string sRequest = string.Empty;
            string sResponse = string.Empty;

            string lineaVersion = string.Empty;
            string[] atributosVersionSeccion1 = null;
            string[] seccionVersion = null;

            //Recupera datos del emisor.
            string resValidacion = string.Empty;
            string sCoriginalEmisor = String.Empty;
            string sCoriginalTimbre = string.Empty;
            string sello = string.Empty;
            OpenSSL_Lib.cSello csello;
            string numeroCertificado = string.Empty;

            XmlNode impuestos = null;
            XmlNode padre = null;
            XmlNamespaceManager nsm = new XmlNamespaceManager(new NameTable());
            XmlDocument xDocumento = new XmlDocument();

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
                                if (arreglo.Contains("noCertificado"))
                                {
                                    string[] snoCert = arreglo.Split('@');
                                    noCertificado = snoCert[1];
                                }
                            }

                            break;
                        case "re":
                            foreach (string arreglo in atributosVersionSeccion1)
                            {
                                if (arreglo.Contains("rfc"))
                                {
                                    string[] rfc = arreglo.Split('@');
                                    sRFC = rfc[1];
                                }
                            }
                            break;
                    }
                }
                catch
                {
                    nBan = 1;
                    //Si el txt es invalido
                    //Copia el archivo txt invalido a otra carpeta
                    File.Copy(archivo, ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                    //Elimina el archivo txt
                    File.Delete(archivo);
                }
            }
            lectorVersion.Close();

            //Si no existe ningun error
            if (nBan == 0) //nBan 1
            {
                if (sVersion == "3.0")
                {
                    try
                    {
                        string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                        clsLog.Escribir(path, DateTime.Now + " " + "Por disposición oficial estos comprobantes ya no se admiten"); 
                    }
                    catch
                    {
                    }

                }

                if (sVersion == "3.2")
                {

                    nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    xDocumento = new XmlDocument(nsm.NameTable);
                    xDocumento.CreateXmlDeclaration("1.0", "UTF-8", "no");
                    xDocumento.AppendChild(xDocumento.CreateElement("cfdi", "Comprobante", "http://www.sat.gob.mx/cfd/3"));
                    while (true)
                    {
                        linea = lector.ReadLine();
                        if (string.IsNullOrEmpty(linea))
                            break;

                        seccion = linea.Split('?');

                        try
                        {
                            atributos = seccion[1].Split('|');

                            if (seccion[0] == "im" || seccion[0] == "it" || seccion[0] == "ir")
                            {
                                if (impuestos == null)
                                {
                                    if (seccion[0] == "im")
                                        impuestos = xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Impuestos", atributos));
                                    else
                                        impuestos = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Impuestos", "http://www.sat.gob.mx/cfd/3"));

                                }
                            }

                            switch (seccion[0])
                            {
                                case "co":
                                    fnCrearElementoRoot32(xDocumento, atributos);
                                    break;
                                case "re":
                                    xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Emisor", atributos));
                                    break;
                                case "de":
                                    padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "DomicilioFiscal", atributos));
                                    break;
                                case "ee":
                                    padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "ExpedidoEn", atributos));
                                    break;
                                case "rf":
                                    padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "RegimenFiscal", atributos));
                                    break;
                                case "rr":
                                    xDocumento.DocumentElement.AppendChild(fnCrearElemento(xDocumento, "Receptor", atributos));
                                    break;
                                case "dr":
                                    padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor", nsm);
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Domicilio", atributos));
                                    break;
                                case "cc":
                                    padre = xDocumento.SelectSingleNode("/cfdi:Comprobante/cfdi:Conceptos", nsm);
                                    if (padre == null)
                                        padre = xDocumento.DocumentElement.AppendChild(xDocumento.CreateElement("cfdi", "Conceptos", "http://www.sat.gob.mx/cfd/3"));
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Concepto", atributos));
                                    break;
                                case "ir":
                                    padre = impuestos.SelectSingleNode("cfdi:Retenciones", nsm);
                                    if (padre == null)
                                        padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Retenciones", "http://www.sat.gob.mx/cfd/3"));
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Retencion", atributos));
                                    break;
                                case "it":
                                    padre = impuestos.SelectSingleNode("cfdi:Traslados", nsm);
                                    if (padre == null)
                                        padre = impuestos.AppendChild(xDocumento.CreateElement("cfdi", "Traslados", "http://www.sat.gob.mx/cfd/3"));
                                    padre.AppendChild(fnCrearElemento(xDocumento, "Traslado", atributos));
                                    break;
                                case "ad":
                                    foreach (string a in atributos)
                                    {
                                        string[] printSet = a.Split('@');
                                        if (printSet[0] == "impresora")
                                        {
                                            printerName = printSet[1];
                                        }
                                        if (printSet[0] == "correo")
                                        {
                                            correo = printSet[1] + "@" + printSet[2];
                                        }
                                    }
                                    
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                                clsLog.Escribir(path, DateTime.Now + " " + ex.Message);
                            }
                            catch
                            {
                            }
                            nBan = 1;
                            //Si el txt es invalido
                            //Copia el archivo txt invalido a otra carpeta
                            File.Copy(archivo, ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                            //Elimina el archivo txt
                            File.Delete(archivo);
                        }
                    }

                }
                lector.Close();
                //Si no existe ningun error
                if (nBan == 0) //nBan 2
                {
                    try
                    {
                        //obtener la llave privada
                        string[] FileKey = null;
                        string RutaKey = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["rutaCertificados"];
                        string filtroKey = "*.key";
                        FileKey = Directory.GetFiles(RutaKey, filtroKey);

                        foreach (string filekey in FileKey)
                        {
                            Stream streamkey = File.Open(filekey.ToString(), FileMode.Open);
                            StreamReader srkey = new StreamReader(streamkey, System.Text.Encoding.UTF8, true);

                            using (BinaryReader br = new BinaryReader(streamkey))
                            {
                                gbLlave = br.ReadBytes(Convert.ToInt32(streamkey.Length));
                            }

                        }
                        //Obtener el password
                        string[] FilePwd = null;
                        string RutaPwd = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["rutaCertificados"];
                        string filtroPwd = "*.txt";
                        FilePwd = Directory.GetFiles(RutaPwd, filtroPwd);

                        foreach (string filePwd in FilePwd)
                        {
                            using (Stream streamPwd = File.Open(filePwd.ToString(), FileMode.Open))
                            {
                                StreamReader srPwd = new StreamReader(streamPwd, System.Text.Encoding.UTF8, true);
                                gsPassword = srPwd.ReadToEnd();
                            }
                        }

                        //Obtener el certificado 
                        string[] FilesCer = null;
                        string RutaCert = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["rutaCertificados"];
                        string filtroCert = "*.cer";
                        FilesCer = Directory.GetFiles(RutaCert, filtroCert);

                        foreach (string filecer in FilesCer)
                        {
                            certEmisor.Import(filecer);
                        }

                        //Si no es modo prueba
                        if (ConectorPAXGenericoPDF.Properties.Settings.Default["Modo"].ToString() == "P")
                        {
                            //Valida certificado sea vigente
                            if (!ComprobarFechas())
                            {
                                try
                                {
                                    string path1 = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                                    clsLog.Escribir(path1, DateTime.Now + " " + "El certificado está fuera de fecha ");
                                   
                                }
                                catch
                                {
                                }
                                break;
                            }

                            //Valida certificado
                            if (!fnCSD308())
                            {
                                try
                                {
                                    string path1 = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                                    clsLog.Escribir(path1, DateTime.Now + " " + "308 - Certificado no expedido por el SAT");
                                    
                                }
                                catch
                                {
                                }
                                break;
                            }
                        }

                        if (gsPassword == null)
                        {
                            gsPassword = "";
                        }

                        string sCertificado = string.Empty;
                        string sTipoFirmado = string.Empty;
                        byte[] bCertificadoInvertido = null;
                        if (FilesCer != null && FileKey != null && FilesCer.Length > 0 && FileKey.Length > 0)
                        {
                            //Cerificado para agregar al XML
                            sCertificado = Convert.ToBase64String(certEmisor.GetRawCertData());
                            
                            //Numero del certificado
                            bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
                            numeroCertificado = Encoding.Default.GetString(bCertificadoInvertido).ToString();
                        }

                        if (noCertificado == numeroCertificado && noCertificado != "" && noCertificado != null)
                        {
                            //Cadena Orginal del Emisor.
                            string scadena = "";
                            switch (sVersion)
                            {
                                case "3.2":
                                    scadena = "cadenaoriginal_3_2";
                                    break;
                            }
                            //Generamos la cadena original
                            XPathNavigator navNodoTimbre = xDocumento.CreateNavigator();
                            sCoriginalEmisor = fnConstruirCadenaTimbrado(navNodoTimbre, scadena); //"cadenaoriginal_3_2"); 
                            

                            //Genera sello de la cadena original

                            switch (sVersion)
                            {
                                case "3.2":
                                    try
                                    {
                                        //sello = fnGenerarSello(sCoriginalEmisor, AlgoritmoSellado.SHA1, false);
                                        csello = new cSello(FileKey[0], FilePwd[0], (String)ConectorPAXGenericoPDF.Properties.Settings.Default["rutaCertificados"] + @"\");

                                        //Obtener tipo de certificado
                                        sTipoFirmado = fnObtenerMetodoFirmadoCertificado(certEmisor);

                                        if (sTipoFirmado.Equals("sha256RSA"))
                                        {
                                            csello.Digestion = enuMetodoDigestion.SHA256;
                                        }
                                        csello.sCadenaOriginal = sCoriginalEmisor;
                                        sello = csello.sSello;
                                    }
                                    catch (Exception ex)
                                    {

                                        string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                                        clsLog.Escribir(path, DateTime.Now + " " + "Fallo la generacion del sello: " + ex.Message);
                                    }
                                    
                                    break;
                            }

                            //Valida sello
                            if (!fnVerificarSello(sCoriginalEmisor, sello, sTipoFirmado))
                            {
                                try
                                {
                                    string path1 = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                                    clsLog.Escribir(path1, DateTime.Now + " " + "Sello incorrecto, verifique que los sellos sean compatibles y que la contraseña este correcta - " + archivo);
                                }
                                catch
                                {
                                }
                                nBan = 1;
                                //Si el txt es invalido
                                //Copia el archivo txt invalido a otra carpeta
                                File.Copy(archivo, ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                                //Elimina el archivo txt
                                File.Delete(archivo);
                            }

                            //Si no existe ningun error
                            if (nBan == 0)
                            {
                                //Asignar los valores de certificado,numero de certificado y sello.
                                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocumento.NameTable);
                                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

                                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).SetValue(numeroCertificado);
                                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(sCertificado);
                                xDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).SetValue(sello);

                                //byte[] buffer = { };
                                //Se almacena el xml
                                //buffer = Encoding.UTF8.GetBytes(xDocumento.InnerXml);
                                //Se obtiene la ruta de creacion del xml
                                //string[] RutasArchivo = archivo.Split('.');
                                //string[] NombreArchivo = RutasArchivo[0].Split('\\');
                                //string path = (String)ConectorPAXGenerico.Properties.Settings.Default["rutaDocs"] + "\\" + sNombreTXT + ".xml";
                                string sbAdenda = "";

                                //Si es envio por correo
                                if (stipoServicio == "GE" || stipoServicio == "GT")
                                {
                                    //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°ADDENDA°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
                                    //Se agrega adenda en caso de existir
                                    //Verifica y forma la adenda
                                    StringReader lectorAd = new StringReader(text);
                                    string lineaAd = string.Empty;
                                    string[] seccionAd = null;
                                    string[] atributosAd = null;

                                    XmlDocument xDocumentoAd;
                                    nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                                    nsm.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                                    xDocumentoAd = new XmlDocument(nsm.NameTable);
                                    //string sCorreo = string.Empty;
                                    int x = 0;
                                    while (true)
                                    {
                                        lineaAd = lectorAd.ReadLine();
                                        if (string.IsNullOrEmpty(lineaAd))
                                            break;

                                        seccionAd = lineaAd.Split('?');

                                        try
                                        {
                                            atributosAd = seccionAd[1].Split('|');
                                            //Nodo adenda en txt
                                            if (seccionAd[0] == "ad")
                                            {
                                                //XmlAttribute xAttr;
                                                XmlNode elemento = xDocumentoAd.CreateElement("cfdi", "Addenda", "http://www.sat.gob.mx/cfd/3");
                                                //XmlNode padreAd = xDocumento.SelectSingleNode("/cfdi:Addenda", nsm);
                                                xDocumentoAd.AppendChild(elemento);
                                                foreach (string a in atributosAd)
                                                {
                                                    string[] valores = a.Split('@');
                                                    if (valores[0] == "correo")
                                                    {
                                                        /*xAttr = xDocumentoAd.CreateAttribute(valores[0]);
                                                        xAttr.Value = valores[1] + "@" + valores[2];*/

                                                        XmlElement currEl = xDocumentoAd.CreateElement(valores[0]);
                                                        currEl.InnerText = valores[1] + "@" + valores[2];
                                                        xDocumentoAd.DocumentElement.AppendChild(currEl);
                                                        //padre.AppendChild(fnCrearElemento(xDocumento, "Traslado", atributos));
                                                    }
                                                    else
                                                    {
                                                        /*xAttr = xDocumentoAd.CreateAttribute(valores[0]);
                                                        xAttr.Value = valores[1];*/

                                                        XmlElement currEl = xDocumentoAd.CreateElement(valores[0]);
                                                        currEl.InnerText = valores[1];
                                                        xDocumentoAd.DocumentElement.AppendChild(currEl);
                                                    }
                                                    //elemento.Attributes.Append(xAttr);
                                                }
                                                //xDocumentoAd.AppendChild(elemento);
                                            }
                                            

                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception(ex.Message);
                                        }
                                    }
                                    //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
                                    //°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°°
                                    if (xDocumentoAd.OuterXml != "")
	                                {
                                        sbAdenda = xDocumentoAd.OuterXml;
	                                }
                                    
                                }

                                bool Existe = false;

                                string HASH = clsOperaciones.GetHASH(sCoriginalEmisor).ToUpper();
                                string Comprobante = clsOperaciones.fnBuscarHashCompXML(HASH);
                                if (Comprobante != "0")
                                {
                                    Existe = true;
                                }

                                if (Existe == false)
                                {
                                    //Envia xml a timbrar
                                    fnEnviaXML(xDocumento, sNombreTXT, sbAdenda, stipoServicio, printerName, correo, HASH);
                                }
                                
                                
                            }
                        }
                        else
                        {
                            try
                            {


                                if ((noCertificado != numeroCertificado || numeroCertificado == null || numeroCertificado == "") && (FilesCer != null && FileKey != null && FilesCer.Length > 0 && FileKey.Length > 0))
                                {
                                    string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                                    clsLog.Escribir(path, DateTime.Now + " " + "El documento " + archivo + " no contiene o es incorrecto el número de certificado");
                                }

                                if (FilesCer == null || FileKey == null || FilesCer.Length == 0 || FileKey.Length == 0)
                                {
                                    string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                                    clsLog.Escribir(path, "Certificados faltantes!");

                                    try
                                    {
                                        if (!File.Exists(path))
                                        {
                                            StreamWriter sr4 = new StreamWriter(path);
                                            sr4.WriteLine("No se encuentran los certificados, antes de enviar mas documentos verifique que los certificados estan en la carpeta de certificados");
                                            sr4.Close();
                                        }
                                        else
                                        {
                                            System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                                            sw4.WriteLine("No se encuentran los certificados, antes de enviar mas documentos verifique que los certificados estan en la carpeta de certificados");
                                            sw4.Close();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        clsLog.Escribir(path,ex.Message);
                                    }
                                }
                            }
                            catch
                            {
                            }
                            nBan = 1;
                            //Si el txt es invalido
                            //Copia el archivo txt invalido a otra carpeta
                            File.Copy(archivo, ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreTXT + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                            //Elimina el archivo txt
                            File.Delete(archivo);
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                            clsLog.Escribir(path, DateTime.Now + " " + ex.Message);

                            File.Copy(archivo, ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreTXT + "_" + "Error" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                            //Elimina el archivo txt
                            File.Delete(archivo);
                            
                        }
                        catch
                        {
                        }
                        nBan = 1;
                    }
                    //Si no existe ningun error
                    if (nBan == 0) //nBan 3
                    {
                        DateTime Fecha1 = DateTime.Today;
                        //Copia el archivo txt con formato correcto a otra carpeta
                        File.Copy(archivo, ConectorPAXGenericoPDF.Properties.Settings.Default["rutaTXTGen"].ToString() + sNombreTXT + "_" + Fecha1.Day + "-" + Fecha1.Month + "-" + Fecha1.Year + ".txt", true);
                        File.Delete(archivo);
                    } //Fin nBan 3
                }//Fin nBan 2
            } //Fin nBan 1
        }
    }

    # region MetodosInternos

    /// <summary>
    /// Comprueba que el sello del comprobante refleje los datos de la caden original
    /// </summary>
    /// <param name="psCadenaOriginal">Cadena original del comprobante</param>
    /// <returns>Booleano indicando si la cadena original corresponde al sello</returns>
    public bool fnVerificarSello(string psCadenaOriginal, string psSello, string sTipoFirmado)//////////////////////////////////////////////////////////////////////////////////////////////////////////////
    {
        RSACryptoServiceProvider publica = ((RSACryptoServiceProvider)certEmisor.PublicKey.Key);
        try
        {
            if (sTipoFirmado.Equals("sha256RSA"))
            {
                try
                {
                    //Verificamos que el certificado obtenga el mismo resultado que el del sello
                    byte[] hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(psCadenaOriginal));
                    bool exito = publica.VerifyHash(
                            hash,
                            "SHA256",
                            Convert.FromBase64String(psSello));

                    return exito;
                }
                catch (Exception)
                {
                    return false;
                }

            }
            else if (sTipoFirmado.Equals("sha1RSA"))
            {
                try
                {
                    //Verificamos que el certificado obtenga el mismo resultado que el del sello
                    byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(psCadenaOriginal));
                    bool exito = publica.VerifyHash(
                            hash,
                            "sha1",
                            Convert.FromBase64String(psSello));

                    return exito;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    //Revisa que no sea apocrifo
    public bool fnCSD308()
    {
        try
        {

            bool ret = false;
            if (certEmisor.IssuerName.Name.Contains("A.C. del Servicio de Administración Tributaria"))
                ret = true;
            else
                ret = false;
            return ret;
        }
        catch (Exception ex)
        {
            
        }
        return false;
        //return true;
    }

    /// <summary>
    /// Verifica que el certificado aún sea vigente
    /// </summary>
    /// <returns></returns>
    public bool ComprobarFechas()
    {

        if (certEmisor.NotBefore.CompareTo(DateTime.Today) > 0
            || certEmisor.NotAfter.CompareTo(DateTime.Today) < 0)
            return false;

        return true;
    }

    /// <summary>
    /// Devuelve el arreglo de bytes del certificado original
    /// </summary>
    /// <returns></returns>
    private byte[] fnDesencriptarCertificado(byte[] pbCertificadoEncriptado)
    {
        return Utilerias.Encriptacion.DES3.Desencriptar(pbCertificadoEncriptado);
    }

    private void fnCrearElementoRoot(XmlDocument pxDoc, string[] pasAtributos)
    {
        XmlAttribute xAttr;
        foreach (string a in pasAtributos)
        {
            string[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = valores[1];
            pxDoc.DocumentElement.Attributes.Append(xAttr);
        }
        xAttr = pxDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
        xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv3.xsd";
        pxDoc.DocumentElement.Attributes.Append(xAttr);
    }

    private void fnCrearElementoRoot32(XmlDocument pxDoc, string[] pasAtributos)
    {
        XmlAttribute xAttr;
        foreach (string a in pasAtributos)
        {
            string[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = valores[1];
            pxDoc.DocumentElement.Attributes.Append(xAttr);
        }
        xAttr = pxDoc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
        xAttr.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd";
        pxDoc.DocumentElement.Attributes.Append(xAttr);
    }

    private XmlElement fnCrearElemento(XmlDocument pxDoc, string psElemento, string[] pasAtributos)
    {
        XmlAttribute xAttr;
        XmlElement elemento = pxDoc.CreateElement("cfdi", psElemento, "http://www.sat.gob.mx/cfd/3");
        foreach (string a in pasAtributos)
        {
            string[] valores = a.Split('@');
            xAttr = pxDoc.CreateAttribute(valores[0]);
            xAttr.Value = valores[1];
            elemento.Attributes.Append(xAttr);
        }
        return elemento;
    }


    /// <summary>
    /// Obtiene el número de certificado asociado al archivo a partir del número de serie contenido en el certificado
    /// </summary>
    /// <returns></returns>
    public string ObtenerNoCertificado()
    {
        byte[] bCertificadoInvertido = certEmisor.GetSerialNumber().Reverse().ToArray();
        return Encoding.Default.GetString(bCertificadoInvertido);
    }

    public static bool AcceptAllCertificatePolicy(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
    {
        string sCadenaOriginal = string.Empty;

        MemoryStream ms;
        StreamReader srDll;
        XsltArgumentList args;
        XslCompiledTransform xslt;
        try
        {
            xslt = new XslCompiledTransform();
            xslt.Load(typeof(CaOri.V32));
            ms = new MemoryStream();
            args = new XsltArgumentList();
            xslt.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            srDll = new StreamReader(ms);
            sCadenaOriginal = srDll.ReadToEnd();
        }
        catch (Exception ex)
        {
            try
            {
                DateTime Fecha = DateTime.Today;
                string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                clsLog.Escribir(path, DateTime.Now + " " + ex.Message);
                
            }
            catch
            {
            }
        }
        return sCadenaOriginal;
    }
    /// <summary>
    /// Tipo de algoritmo a usar
    /// </summary>
    public enum AlgoritmoSellado
    {
        MD5,
        SHA1
    }
    /// <summary>
    /// Genera documento XML
    /// </summary>
    /// <param name="sNombreArchivo"></param>
    /// <param name="sAddenda"></param>
    private void fnEnviaXML(XmlDocument xXmlDoc, string sNombreArchivo, string sAddenda, string stipoServicio, string printerName, string correo, string HASH)
    {
        try
        {
            char[] cCad = { '-' };
            string[] sCad;
            string sXML = string.Empty;
            DateTime Fecha = DateTime.Today;
            //string[] Files = null;
            //string RutaXMLDocs = ConectorPAXGenerico.Properties.Settings.Default["rutaDocs"].ToString() + sNombreArchivo + ".xml";
            //string filtro = "*.xml";
            //Files = Directory.GetFiles(RutaXMLDocs, filtro);

            //foreach (string archivo in Files)
            //{
                int nBan = 0;
                string snombreDoc = string.Empty;
                //Stream stream = File.Open(RutaXMLDocs, FileMode.Open);
                string sNombreXML = sNombreArchivo; //Path.GetFileNameWithoutExtension(RutaXMLDocs);

                //Si no existe ningun error
                if (nBan == 0) //nBan 1
                {
                    string RFCReceptor = string.Empty;
                    string sVersion = string.Empty;
                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xXmlDoc.NameTable); //xmlfinal.NameTable);
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                    string FechaDocto = string.Empty;
                    try
                    {
                        RFCReceptor = xXmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;
                        FechaDocto = xXmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value;
                        sVersion = xXmlDoc.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;

                        string usuario = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["usuario"];
                        string password = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["password"];
                        string tipodocumento = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["tipodocto"];
                        string sServicioTimbrado = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["ServicioTimbrado"];
                        //Se consume servicio de timbrado
                        sXML = wsRecepcion.fnEnviarTXT(xXmlDoc.OuterXml, tipodocumento, 0, usuario, password, sVersion, sServicioTimbrado);
                        
                        //Se valida el tipo de respuesta
                        sCad = sXML.Split(cCad);
                        if (sCad.Length <= 2)
                        {
                            try
                            {
                                nBan = 1; //Se indica que el comprobante no fue timbrado
                                //En caso de marcar error se graba un log
                                string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogErrorSinTimbrar" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                                clsLog.Escribir(path, DateTime.Now + " " + sXML + ", Nombre XML: " + sNombreXML);
  
                                //Guarda XML invalido
                                xXmlDoc.Save(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreXML + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".xml");
                                //Elimina el archivo xml
                                //File.Delete(RutaXMLDocs);
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            string status = string.Empty;
                            bool impreso = false;
                            //Si el xml fue timbrado
                            XmlDocument XMLTimbrado = new XmlDocument();
                            XMLTimbrado.LoadXml(sXML);

                            int id = clsOperaciones.InsertarComprobante(XMLTimbrado, impreso, HASH);

                            //Se agrega la adenda si existe
                            if (!string.IsNullOrEmpty(sAddenda))
                            {
                                XmlDeclaration xDec = XMLTimbrado.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                                XMLTimbrado.InsertBefore(xDec, XMLTimbrado.DocumentElement);

                                XmlDocument xAddenda = new XmlDocument();
                                xAddenda.LoadXml(sAddenda);

                                XmlNode node = xAddenda.SelectSingleNode("(/*)");

                                XmlNode childElement = XMLTimbrado.CreateNode(XmlNodeType.Element, "cfdi:Addenda", XMLTimbrado.DocumentElement.NamespaceURI);
                                XMLTimbrado.ChildNodes[1].AppendChild(childElement);

                                childElement.InnerXml = node.InnerXml;

                                clsOperaciones.fnInsertaAddenda(id, xAddenda);
                            }

                            XmlNamespaceManager nsmComprobanteT = new XmlNamespaceManager(XMLTimbrado.NameTable);
                            nsmComprobanteT.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                            nsmComprobanteT.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                            XPathNavigator navEncabezado = XMLTimbrado.CreateNavigator();
                            try { snombreDoc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobanteT).Value; }
                            catch { snombreDoc = Guid.NewGuid().ToString(); }

                            //Ruta XML
                            string sRutaXML = ConectorPAXGenericoPDF.Properties.Settings.Default["RutaDocZips"].ToString() + snombreDoc + ".xml";
                            //Se guarda XML en ruta especificada 
                            XMLTimbrado.Save(sRutaXML);
                            
                            //Ruta PDF
                            string sRutaPDF = ConectorPAXGenericoPDF.Properties.Settings.Default["rutaPDF"].ToString() + snombreDoc + ".pdf";
                            try
                            {
                                //Generar PDF
                                clsPlantillaLista PlantillaLista = new clsPlantillaLista();

                                PlantillaLista.fnCrearPLantillaEnvio(XMLTimbrado, tipodocumento, sRutaPDF);

                                PrinterSettings printer = new PrinterSettings();
                                printer.PrinterName = printerName;

                                if (printer.IsValid)
                                {
                                    ExecuteCommandBAT(sRutaPDF);
                                    string Filename = sRutaPDF + ".ps";
                                    if (File.Exists(Filename))
                                    {
                                            // Print the file to the printer.
                                            RawPrinterHelper.SendFileToPrinter(printer.PrinterName, Filename);
                                            string archivo = Path.GetFileName(Filename);

                                            impreso = RawPrinterHelper.GetJobs(printerName, archivo);
                                            clsOperaciones.fnActualizarImpresionPrim(id, impreso);
                                            File.Delete(Filename);
                                    }
                                }
                                else
                                {
                                    if (printerName != "")
                                    {
                                        string pathI = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogErrorSinImprimir" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                                        clsLog.Escribir(pathI, DateTime.Now + " " + ", Nombre PDF: " + snombreDoc + " asegurese de que la impresora " + printerName + " este conectada");
                                    }
                                    
                                }

                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    DateTime Fechaex = DateTime.Today;
                                    string pathex = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fechaex.Day + "-" + Fechaex.Month + "-" + Fechaex.Year + ".txt";
                                    clsLog.Escribir(pathex, DateTime.Now + " " + ex.Message);
                                }
                                catch
                                {
                                }
                            }

                            
                            //Envia documento por correo electronico
                            if ((stipoServicio == "GE" || stipoServicio == "GT") && correo != "")
                            {
                                clsEnvioCorreoDocs EnvioCorreoDocs = new clsEnvioCorreoDocs();
                                string sMensaje = string.Empty;
                                sMensaje = "<table>";
                                sMensaje = sMensaje + "<tr><td>Estimado Cliente. <br /><br />Se adjunta por medio de este correo electrónico su Comprobante Fiscal Digital por Internet.<br />"+ status +"</td></tr>";
                                sMensaje = sMensaje + "</table>";
                                string sMailTo = correo;
                                bool sEnvio = false;
                                //////////////////////////////////////////////////////////////////////////////
                                //Envia documento
                                sEnvio = EnvioCorreoDocs.fnPdfEnvioCorreoSinZIP(XMLTimbrado, sRutaPDF, sMailTo, "Comprobantes", sMensaje, sRutaXML, snombreDoc, "");
                                //Se verifica si el correo fue enviado
                                if (sEnvio == false)
                                {
                                    try
                                    {                                        
                                        if (!File.Exists((String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt"))
                                        {
                                            StreamWriter sr4 = new StreamWriter((String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt");
                                            sr4.WriteLine(DateTime.Now + " No se puede enviar el documento - " + snombreDoc + " - al correo - " + sMailTo);
                                            sr4.Close();
                                        }
                                        else
                                        {
                                            System.IO.StreamWriter sw4 = new System.IO.StreamWriter((String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt", true);
                                            sw4.WriteLine(DateTime.Now + " No se puede enviar el documento - " + snombreDoc + " - al correo - " + sMailTo);
                                            sw4.Close();
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }

                            //Se guarda log de comprobantes timbrados
                            string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogTimbrados"] + "LogTimbrados" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                            if (!File.Exists(path))
                            {
                                StreamWriter sr4 = new StreamWriter(path);
                                sr4.WriteLine(DateTime.Now + ", Nombre XML: " + sNombreXML);
                                sr4.Close();
                            }
                            else
                            {
                                System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                                sw4.WriteLine(DateTime.Now + ", Nombre XML: " + sNombreXML);
                                sw4.Close();
                            }
                            //Elimina el archivo xml
                            //File.Delete(RutaXMLDocs);
                        }

                    }
                    catch (Exception ex)
                    {
                        nBan = 1;
                        try
                        {
                            string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                            clsLog.Escribir(path, DateTime.Now + " " + ex.Message);
                            //Guarda XML invalido
                            xXmlDoc.Save(ConectorPAXGenericoPDF.Properties.Settings.Default["rutaDocInv"].ToString() + sNombreXML + "_" + "Invalido" + "_" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".xml");
                            //Elimina el archivo xml
                            //File.Delete(RutaXMLDocs);
                        }
                        catch
                        {
                        }
                    }
                }//Fin nBan 1                
            //}
        }
        catch (Exception ex)
        {
            try
            {
                DateTime Fecha = DateTime.Today;
                string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";
                clsLog.Escribir(path, DateTime.Now + " " + ex.Message);
            }
            catch
            {
            }
        }
    }

    //Envia los documentos generados por correo electronico
    private void fnEnviarDocsCorreo()
    {

    }

    private void ExecuteCommandBAT(string file)
    {
        string outputO = "";
        string errorO = "";
        string ExitCode = "";

        try
        {
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo(AppDomain.CurrentDomain.BaseDirectory + "lib\\pdf2ps.bat");
            processInfo.Verb = "runas";
            processInfo.Arguments = file + " " + file + ".ps";
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            outputO = "output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output);
            errorO = "error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error);
            ExitCode = "ExitCode: " + exitCode.ToString();

            process.Close();


        }
        catch(Exception ex)
        { 
                DateTime Fecha = DateTime.Today;
                string path = (String)ConectorPAXGenericoPDF.Properties.Settings.Default["LogError"] + "LogError" + Fecha.Day + "-" + Fecha.Month + "-" + Fecha.Year + ".txt";

                if (!File.Exists(path))
                {
                    StreamWriter sr4 = new StreamWriter(path);
                    sr4.WriteLine(DateTime.Now + " " + ex.Message);
                    sr4.WriteLine(DateTime.Now + " " + outputO);
                    sr4.WriteLine(DateTime.Now + " " + errorO);
                    sr4.WriteLine(DateTime.Now + " " + ExitCode);
                    sr4.Close();
                }
                else
                {
                    System.IO.StreamWriter sw4 = new System.IO.StreamWriter(path, true);
                    sw4.WriteLine(DateTime.Now + " " + ex.Message);
                    sw4.WriteLine(DateTime.Now + " " + outputO);
                    sw4.WriteLine(DateTime.Now + " " + errorO);
                    sw4.WriteLine(DateTime.Now + " " + ExitCode);
                    sw4.Close();
                }
        }
    }

    /// <summary>
    /// Devuelve el arreglo de bytes del archivo key
    /// </summary>
    /// <returns></returns>
    private byte[] fnDesencriptarLlave(byte[] pbLlave)
    {
        return Utilerias.Encriptacion.DES3.Desencriptar(pbLlave);
    }


    /// <summary>
    /// Devuelve el password encriptado
    /// </summary>
    /// <returns></returns>
    public string fnEncriptarPassword()
    {
        return Convert.ToBase64String((Utilerias.Encriptacion.DES3.Encriptar(Encoding.UTF8.GetBytes(gsPassword))));
    }

    /// <summary>
    /// Desencripta el password
    /// </summary>
    /// <param name="psPassword">Cadena encriptada ocn el password</param>
    /// <returns></returns>
    private string fnDesencriptarPassword(string psPassword)
    {
        return Encoding.UTF8.GetString(Utilerias.Encriptacion.DES3.Desencriptar(Convert.FromBase64String(psPassword)));
    }


    private byte[] gbLlave;
    /// <summary>
    /// Retorna o establece el arreglo de bytes del archivo key
    /// </summary>
    public byte[] LlavePrivada
    {
        get { return gbLlave; }
        set { gbLlave = value; }
    }
    private string gsPassword;
    /// <summary>
    /// Retorna o establece el password de la llave privada
    /// </summary>
    public string Password
    {
        get { return gsPassword; }
        set { gsPassword = value; }
    }
    public String fnObtenerMetodoFirmadoCertificado(X509Certificate2 px509Cer)
    {

        return px509Cer.SignatureAlgorithm.FriendlyName;
    }
    public String fnObtenerMetodoFirmadoCertificado(String sCer)
    {
        byte[] bCer = Convert.FromBase64String(sCer);

        X509Certificate2 x509cer = new X509Certificate2(bCer);

        return fnObtenerMetodoFirmadoCertificado(x509cer);

    }


    # endregion
}


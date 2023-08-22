using ConectorPDFSharp.Properties;
using ConectorPDFSharp.wcfCancelaASMX;
using Microsoft.Win32;
using OpenSSL_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Serialization;
using ConectorPDFSharp;


class clsTimbradoCancelacion
{
    wcfCancelaASMX wsCancelacion = new wcfCancelaASMX();


    public string gsRfc;
    public string[] UUI;

    public void fnTimbradoCancelacion()
    {
        string[] Files = null;
        string RutaXMLDocs = Settings.Default.RutaDocCan;
        string filtro = "*.txt";
        Files = Directory.GetFiles(RutaXMLDocs, filtro);
        DateTime Fecha = DateTime.Today;


        foreach (string archivo in Files)
        {
            while ((clsLog.fnWaitForFile(archivo) == false))
            {
                //Se hace pato un rato.(Se espera a que se desbloquee el Archivo)
            }

            string sLineasValidas = "";
            string sLineasErroneas = "";
            try
            {
                StringReader lector = new StringReader(File.ReadAllText(archivo));
                string sCancelados = "Cancelados en " + System.IO.Path.GetFileName(archivo) + ":";
                bool bCancelados = false;
                bool bRechazados = false;
                string sRechazados = "Rechazados en " + System.IO.Path.GetFileName(archivo) + ":";
                while (true)
                {
                    string linea = lector.ReadLine();
                    if (string.IsNullOrEmpty(linea))
                        break;

                    string[] datos = linea.Split('|');
                    if (datos.Length >= 2)
                    {

                        string[] uuid = { datos[0] };
                        //uuid[0] = datos[0];
                        string rfc = datos[1];
                        // string usuario = Settings.Default["usuario"].ToString();
                        string usuario = Settings.Default.Usuario;
                        string password = Settings.Default.Password;

                        try
                        {
                            string uuidEstatus = string.Empty;
                            string uuidDescripcion = string.Empty;
                            string uuidFecha = string.Empty;
                            string resultado = wsCancelacion.fnCancelarXML(uuid, rfc, 0, usuario, password); ;

                            XmlDocument xResultado = new XmlDocument();
                            try
                            {
                                xResultado.LoadXml(resultado);
                                XmlElement xFolios = (XmlElement)xResultado.GetElementsByTagName("Folios")[0];
                                uuidEstatus = xFolios.GetElementsByTagName("UUIDEstatus")[0].InnerText.Trim();
                                uuidDescripcion = xFolios.GetElementsByTagName("UUIDdescripcion")[0].InnerText.Trim();
                                uuidFecha = xFolios.GetElementsByTagName("UUIDfecha")[0].InnerText.Replace(" - ", "").Trim();
                            }
                            catch (Exception ex)
                            {
                                uuidDescripcion = resultado;
                                sLineasErroneas += linea + System.Environment.NewLine;
                            }
                            //verificar el estatus que devuelve el web service

                            if (Settings.Default.Modo == "T")
                            {

                                if (uuidEstatus == "999") //Factura Cancelada
                                {

                                    sCancelados += System.Environment.NewLine + uuid[0];
                                    bCancelados = true;
                                    sLineasValidas += linea + System.Environment.NewLine;


                                }
                                else
                                {

                                    sRechazados += System.Environment.NewLine + uuid[0] + " " + uuidDescripcion;
                                    bRechazados = true;
                                    sLineasErroneas += linea + System.Environment.NewLine;

                                }
                            }

                            if (Settings.Default.Modo == "P")
                            {

                                if (uuidEstatus == "201" || uuidEstatus == "202") //Factura Cancelada
                                {
                                    if (uuidEstatus == "201")
                                    {

                                        sCancelados += "\n" + uuid[0];
                                        bCancelados = true;
                                        sLineasValidas += linea + System.Environment.NewLine;
                                    }
                                    else
                                    {

                                        sCancelados += System.Environment.NewLine + " [Cancelado Anteriormente] " + uuid[0];
                                        bCancelados = true;
                                        sLineasValidas += linea + System.Environment.NewLine;

                                    }
                                }
                                else
                                {
                                    sRechazados += System.Environment.NewLine + uuid + " " + uuidDescripcion;
                                    bRechazados = true;
                                    sLineasErroneas += linea + System.Environment.NewLine;
                                }
                            }

                        }

                        catch (Exception ex)
                        {

                            throw new Exception(ex.Message);
                        }
                    }
                }

                if (bCancelados)
                {
                    clsLog.EscribirLogCancelacion(sCancelados);
                }
                if (bRechazados)
                {
                    clsLog.EscribirLogCancelacion(sRechazados);

                }

                try
                {
                    if (bCancelados)
                        clsLog.EscribirLogCancelacion("Documentos cancelados con las siguientes lineas: " + sLineasValidas);
                    if (bRechazados)
                        clsLog.EscribirLogCancelacion("Documentos rechazados con las siguientes lineas: " + sLineasErroneas);

                    File.Delete(archivo);
                }
                catch (Exception ex)
                {

                    clsLog.EscribirLog(ex.Message);
                }
            }
            catch (Exception ex)
            {

                clsLog.EscribirLog(ex.Message);

            }

        }
    }
}


using PAXCrypto;
using SolucionPruebas.Presentacion.Servicios;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Addressing;
using Microsoft.Web.Services3.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace SolucionPruebas.Presentacion.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xdComplemento = new XmlDocument();
            XmlNode xnNodoPrincipal;
            XmlNode xmlNodoComplemento;
            try
            {
                //PrinterSettings printer = new PrinterSettings();
                ////printer.PrinterName = "NPI834C87";

                ////printer.PrinterName = "NPI834C87 (HP LaserJet CM1415fn)";
                //printer.PrinterName = "NPI140512 (HP LaserJet 200 colorMFP M276nw)";
                ////printer.PrinterName = "HP LaserJet 200 color MFP M276 PCL 6";

                XmlDocument xmComplemento = new XmlDocument();
                xmComplemento.Load(@"C:\Users\Ismael.Hidalgo\Desktop\a2a6fa37-e492-447b-8b58-2e13ced0c5c0.xml");

                XmlNamespaceManager nsmComplemento = new XmlNamespaceManager(xmComplemento.NameTable);
                nsmComplemento.AddNamespace("ventavehiculos", "http://www.sat.gob.mx/ventavehiculos");
                nsmComplemento.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComplemento.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                XPathNavigator navComprobante = xmComplemento.CreateNavigator();
                XPathNodeIterator navDetalles = navComprobante.Select("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsmComplemento);

                if (xmComplemento.OuterXml.Contains("http://www.sat.gob.mx/ventavehiculos"))
                {
                    while (navDetalles.MoveNext())
                    {
                        XPathNavigator nodenavigator = navDetalles.Current;

                        if (nodenavigator.HasChildren)//Si contiene nodo hijo
                        {
                            XPathNodeIterator navPartes = nodenavigator.Select("cfdi:ComplementoConcepto/ventavehiculos:VentaVehiculos/ventavehiculos:Parte", nsmComplemento);
                            XPathNodeIterator navInfoAduanera = nodenavigator.Select("cfdi:ComplementoConcepto/ventavehiculos:VentaVehiculos/ventavehiculos:InformacionAduanera", nsmComplemento);

                            while (navInfoAduanera.MoveNext())
                            {
                                XPathNavigator nnInfoAduanera = navInfoAduanera.Current;

                                if (nnInfoAduanera != null)//Si contiene nodo hijo
                                {
                                    RenameNode(((IHasXmlNode)nnInfoAduanera).GetNode(), "VehiculoInformacionAduanera", "ventavehiculos", "http://www.sat.gob.mx/ventavehiculos");
                                }
                            }
                        }
                    }

                    xmComplemento.LoadXml(xmComplemento.InnerXml.Replace("ventavehiculos:InformacionAduanera", "ventavehiculos:ParteInformacionAduanera"));
                }


                //XmlNamespaceManager nsmComplemento = new XmlNamespaceManager(xmComplemento.NameTable);
                //XPathNavigator xnComplemento;

                ////XmlNode root = xmComplemento.DocumentElement;
                //xnComplemento = xmComplemento.CreateNavigator();

                //string sNombreComplemento = xnComplemento.SelectSingleNode("/Datos/comp/@tipo").Value;

                //if (string.IsNullOrEmpty(sNombreComplemento))
                //{
                //    throw new Exception("");
                //}

                //string sPrefijo = string.Empty;
                //string sNameSpace = string.Empty;
                //string sNameSpaceURI = string.Empty;

                //fnObtenerValoresComplementos(sNombreComplemento, ref sPrefijo, ref sNameSpace, ref sNameSpaceURI);

                //xnNodoPrincipal = xdComplemento.CreateNode(XmlNodeType.Element, sPrefijo, sNameSpace, sNameSpaceURI);

                //XmlNodeList xnlNodos = xmComplemento.DocumentElement.ChildNodes;

                //if (xnlNodos.Count <= 0)
                //{
                //    throw new Exception("");
                //}

                //foreach (XmlNode xnNodo in xnlNodos)
                //{
                //    string sNombreNodo = string.Empty;
                //    try { sNombreNodo = xnNodo.SelectSingleNode("@tipo", nsmComplemento).Value; }
                //    catch { }

                //    if (!string.IsNullOrEmpty(sNombreNodo))
                //        continue;


                //    XmlAttributeCollection aacAtributos = xnNodo.Attributes;

                //    XmlAttribute xAttr;
                //    foreach (XmlAttribute item in aacAtributos)
                //    {
                //        xAttr = xdComplemento.CreateAttribute(item.Name);
                //        xAttr.Value = item.Value;
                //        xnNodoPrincipal.Attributes.Append(xAttr);
                //    }

                //}


                //string ss = string.Empty;

                //ss = "10.21";

                //Console.WriteLine(Convert.ToDecimal(ss).ToString("F2"));
                
                //ss = "10.2";

                //Console.WriteLine(Convert.ToDecimal(ss).ToString("F2"));

                //ss = "10.221";

                //Console.WriteLine(Convert.ToDecimal(ss).ToString("F2"));

                //ss = "10";

                //Console.WriteLine(Convert.ToDecimal(ss).ToString("F2"));

                //Console.WriteLine("P{0}Y{1}M{2}D", nAnios, nMeses, nDias);
                //Console.Read();


                //string sFechaFinalPago = "01/06/2017";
                //string sFechaInicioRelLaboral = "01/03/2017";

                //DateTime dFechaFinalPago = DateTime.ParseExact(sFechaFinalPago, "dd/MM/yyyy", null);

                //DateTime dFechaInicioRelLaboral = DateTime.ParseExact(sFechaInicioRelLaboral, "dd/MM/yyyy", null);

                //TimeSpan difference = dFechaFinalPago.Subtract(dFechaInicioRelLaboral.Date);

                //DateTime totalDate1 = DateTime.MinValue + difference;

                //int DiasTotales = difference.Days;


                //string[] sFechaSeparadaPago = sFechaFinalPago.Split('/');

                //int DiaPago = Convert.ToInt32(sFechaSeparadaPago[0]);
                //int MesPago = Convert.ToInt32(sFechaSeparadaPago[1]);
                //int AnioPago = Convert.ToInt32(sFechaSeparadaPago[2]);



                //string[] sFechaSeparada = sFechaInicioRelLaboral.Split('/');

                //int DiaMenor = Convert.ToInt32(sFechaSeparada[0]);
                //int MesMenor = Convert.ToInt32(sFechaSeparada[1]);
                //int AnioMenor = Convert.ToInt32(sFechaSeparada[2]);


                //int contadorAnios = 0;
                //int contadorMeses = 0;
                //int contadorDias = 0;

                //int[] Enero = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
                //int[] Febrero = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };
                //int[] Marzo = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
                //int[] Abril = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
                //int[] Mayo = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
                //int[] Junio = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
                //int[] Julio = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
                //int[] Agosto = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
                //int[] Septiembre = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
                //int[] Octubre = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
                //int[] Noviembre = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 };
                //int[] Diciembre = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };

                //List<int[]> arrayList = new List<int[]>();

                //arrayList.Add(Enero);

                //arrayList.Add(Febrero);

                //arrayList.Add(Marzo);

                //arrayList.Add(Abril);

                //arrayList.Add(Mayo);

                //arrayList.Add(Junio);

                //arrayList.Add(Julio);

                //arrayList.Add(Agosto);

                //arrayList.Add(Septiembre);

                //arrayList.Add(Octubre);

                //arrayList.Add(Noviembre);

                //arrayList.Add(Diciembre);


                //DiaMenor = Convert.ToInt32(sFechaSeparada[0]);
                //MesMenor = Convert.ToInt32(sFechaSeparada[1]); 


                //int MesOperacion = MesMenor - 1; 
                //int DiaOperacion = DiaMenor - 1;

                //int MesesEnAnio = 12;

                //int loops = 0;

                //int DiaPuntero = 0;

                //while (DiasTotales != 0)
                //{

                //    if (loops == 0 && MesOperacion > 0)
                //    {
                //        foreach (int[] Mes in arrayList.Skip(MesOperacion - 1))
                //        {
                //            if (DiaOperacion > 0)
                //            {
                //                foreach (int Dia in Mes.Skip(DiaOperacion))
                //                {

                //                }
                //                DiaOperacion = 0;
                //            }
                //            else
                //            {
                //                int UltimoDelMes = Mes.Length;
                //                foreach (int Dia in Mes)
                //                {

                //                    if (DiaMenor > 1 && Dia == DiaMenor)
                //                    {
                //                        contadorMeses++;
                //                    }
                //                    else if (Dia == UltimoDelMes)
                //                    {
                //                        contadorMeses++;

                //                    }
                //                    if (DiasTotales > 0)
                //                    {
                //                        contadorDias++;
                //                        DiasTotales--;
                //                    }

                //                }
                //            }
                //        }
                //        MesOperacion = 0;
                //    }
                //    else
                //    {
                //        foreach (int[] Mes in arrayList)
                //        {
                //            if (DiaOperacion > 0)
                //            {

                //                foreach (int Dia in Mes.Skip(DiaOperacion))
                //                {

                //                }
                //                DiaOperacion = 0;
                //            }
                //            else
                //            {
                //                int UltimoDelMes = Mes.Length;
                //                foreach (int Dia in Mes)
                //                {
                //                    if (DiaMenor > 1 && Dia == DiaMenor)
                //                    {
                //                        contadorMeses++;
                //                    }
                //                    else if (Dia == UltimoDelMes)
                //                    {
                //                        contadorMeses++;

                //                    }
                //                    if (DiasTotales > 0)
                //                    {
                //                        contadorDias++;
                //                        DiasTotales--;
                //                    }
                //                }
                //            }
                //        }
                //    }


                //    loops++;
                //}


                //DateTime dFIRL = new DateTime();
                //DateTime dFFP = new DateTime();

                //dFIRL = Convert.ToDateTime("2016-02-01");
                //dFFP = Convert.ToDateTime("2017-03-01");

                //int daysInBaseMonth = DateTime.DaysInMonth(dFIRL.Year, dFIRL.Month);


                //int nAnios = dFFP.Year - dFIRL.Year;
                //int nMeses = dFFP.Month - dFIRL.Month;
                //int nDias = 0;

                //if (nAnios == 0) // Periodos en un solo año
                //{
                //    if (nMeses == 0)
                //    {
                //        nDias = (dFFP - dFIRL).Days + 1;
                //    }
                //    else if (nMeses == 1)
                //    {
                //        if (dFIRL.Day <= dFFP.Day)
                //        {
                //            nDias = (dFFP - dFIRL).Days + 1;
                //            nMeses = 0;
                //        }
                //        else
                //        {
                //            nDias = dFFP.Day - dFIRL.Day;
                //        }
                //    }
                //    else // Diferencía mayor a 1 mes
                //    {
                //        nMeses = 0;

                //        for (int i = dFIRL.Month; i <= dFFP.Month; i++)
                //        {
                //            if (i == dFIRL.Month)
                //            {
                //                nDias += (DateTime.DaysInMonth(dFIRL.Year, dFIRL.Month) - dFIRL.Day) + 1;
                //            }
                //            else if (i == dFFP.Month)
                //            {
                //                nDias += dFFP.Day;
                //            }
                //            else
                //            {
                //                nMeses++;
                //            }
                //        }
                //    }
                //} 
                //else if (nAnios == 0 && nMeses < 0) // Un año de diferencia pero sin llegar al año
                //{
                //    nMeses = 0;
                //    nAnios = 0;
                //    nDias = 0;

                //    for (int i = dFIRL.Year; i <= dFFP.Year; i++)
                //    {
                //        for (int j = 1; j <= 12; j++)
                //        {
                //            DateTime dFecha = new DateTime(i, j, 1);

                //            if (j == dFIRL.Month && i == dFIRL.Year)
                //            {
                //                nDias += (DateTime.DaysInMonth(dFIRL.Year, dFIRL.Month) - dFIRL.Day) + 1;
                //            }
                //            else if (j == dFFP.Month && i == dFFP.Year)
                //            {
                //                nDias += dFFP.Day;
                //            }
                //            else if (dFecha > new DateTime(dFIRL.Year, dFIRL.Month, 1) && dFecha < new DateTime(dFFP.Year, dFFP.Month, 1))
                //            {
                //                nMeses++;
                //            }
                //            else
                //            {
                            
                //            }
                //        }
                //    }
                //}
                //else if (nAnios > 0 && nMeses == 0)//Un año y días
                //{
                //    nDias = dFFP.Day - dFIRL.Day;
                //}
                //else// Un año y meses y días
                //{
                //    if (nMeses < 0)
                //    {
                //        nMeses += 12;
                //    }

                //    nDias = dFFP.Day - dFIRL.Day;

                //    if (nDias < 0)
                //    {
                //        nDias = 0;
                //        nDias += dFFP.Day;
                //        nDias += (DateTime.DaysInMonth(dFIRL.Year, dFIRL.Month) - dFIRL.Day) + 1;
                //    }
                //}

                //Console.WriteLine("P{0}Y{1}M{2}D", nAnios, nMeses, nDias);
                //Console.Read();





                //clsValCertificado vValidadorCertificado = null;
                //XmlDocument xdComprobante = new XmlDocument();

                //string sEncriptado = PAXCrypto.CryptoAES.DesencriptaAES64("OOxrcQOKLIv4KMZhBTZMvWF5ldvdVEqm4jBtZcLSj5zzsV+YI+7Kpwqsioe6eQBCIQopZJVLe5CAeJhWwGX27ACG23p2P178JD7l1cxK0aDaMWX1jW4OHMlTZd6VwyNwDoKV17b8WS3PvDoIiQXKRBW+e/53Uxqycjncd+g2iJ5Lk3K+5jZTWbBnDDoMwzH62Coo/03c2hHbyv5sBdRta1hzWEIkBwiz2tQQDuepJbmJXZdWMEbN9go2p/eXNyyA4P8synDXiShV/XseUyInSr/KGMdK7QX60wh6Xo2I0WspXMGg/gJm1zW8tLvn5cDzB5A17BPmleFGjoIWUSx+7Q==");
                //string psEncriptado = sEncriptado;
                //try
                //{
                //    xdComprobante.Load(@"C:\generaldeseguros.xml");

                //    vValidadorCertificado = fnRecuperarCertificado(xdComprobante.InnerXml);

                //    //La fecha de emisión no esta dentro de la vigencia del CSD del Emisor
                //    //clsPistasAuditoria.fnGenerarPistasAuditoria(datosUsuario.id_usuario, DateTime.Now, "WSD paso14 - " + "Revisar vigencia del certificado.");
                //    if (!vValidadorCertificado.ComprobarFechas() || !fnRecuperaFechaLCO("00001000000403772766", "A", ref vValidadorCertificado))
                //    {
                //        //*******************************************************Insertar Response en tabla de acuses
                //        //sResponse = clsComun.fnResponseRecepcion(clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación"));
                //        //clsComun.fnInsertaAcusePAC(datosUsuario.id_contribuyente, string.Empty, sResponse, DateTime.Now, "305", "Response", "E", string.Empty);
                //        //*******************************************************Insertar Response en tabla de acuses
                //        //return clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
                //    }
                //}
                //catch (Exception)
                //{
                //    //return clsComun.fnRecuperaErrorSAT("305", "Timbrado y Cancelación");
                //}

                //XmlDocument xd = new XmlDocument();
                //xd.Load(@"C:\4dae36ba-07bc-4790-be4a-e8d512baeaa4.xml");


                //string Addenda = string.Empty;

                //for (int countP = 0; countP < xd.ChildNodes.Count; countP++)
                //{
                //    for (int count = 0; count < xd.ChildNodes[countP].ChildNodes.Count; count++)
                //    {
                //        Addenda = xd.ChildNodes[countP].ChildNodes[count].Name;
                //        if (Addenda.Contains("Addenda"))
                //        {
                //            xd.ChildNodes[countP].RemoveChild(xd.ChildNodes[countP].ChildNodes[count]);
                //        }
                //    }
                //}

                //for (int countP = 0; countP < xd.ChildNodes.Count; countP++)
                //{
                //    for (int count = 0; count < xd.ChildNodes[countP].Attributes.Count; count++)
                //    {
                //        Addenda = xd.ChildNodes[countP].Attributes[count].Value;
                //        if (Addenda.Contains("Addenda"))
                //        {
                //            //documento.ChildNodes[countP].Attributes[count].Value.Remove(81);
                //            xd.ChildNodes[countP].Attributes[count].Value = xd.ChildNodes[countP].Attributes[count].Value.Remove(81);
                //        }
                //    }
                //}

                //var listaArchivos = RecuperaListaArchivos(@"C:\Users\Ismael.Hidalgo\Desktop\Certificado\");

                //foreach (string nombreArchivo in listaArchivos)
                //{
                //    if (Path.GetExtension(nombreArchivo) != ".xml")
                //        continue;

                //    XmlDocument xdDocumento = new XmlDocument();
                //    xdDocumento.Load(nombreArchivo);

                //    byte[] bCertificado = null;
                //    bCertificado = File.ReadAllBytes(@"C:\Users\Ismael.Hidalgo\Desktop\Certificado\00001000000401528798.cer");

                //    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdDocumento.NameTable);
                //    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                //    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                //    nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                //    xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).SetValue(Convert.ToBase64String(bCertificado));

                //    File.WriteAllText(@"C:\Users\Ismael.Hidalgo\Desktop\Certificado\Nuevos\" + Path.GetFileNameWithoutExtension(nombreArchivo) + ".xml", xdDocumento.InnerXml);
                //}


                //var listaArchivos = RecuperaListaArchivos(@"C:\Users\Administrator\Desktop\PendientesEntrega\");

                //foreach (string nombreArchivo in listaArchivos)
                //{
                //    string sUUID = string.Empty;
                //    string sSerie = string.Empty;
                //    string sfolio = string.Empty;
                //    string sTotal = string.Empty;
                //    string sRfcEmisor = string.Empty;
                //    string sRfcReceptor = string.Empty;
                //    string sEmisorNombre = string.Empty;
                //    DateTime sFechaTimbrado;
                //    string sReceptorNombre = string.Empty;

                //    XmlDocument xdDocumento = new XmlDocument();
                //    xdDocumento.Load(nombreArchivo);

                //    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdDocumento.NameTable);
                //    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                //    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                //    nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

                //    try { sSerie = xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value; }
                //    catch { }
                //    try { sfolio = xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
                //    catch { }
                //    try { sTotal = xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value; }
                //    catch { }
                //    try { sRfcEmisor = xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value; }
                //    catch { }
                //    try { sRfcReceptor = xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value; }
                //    catch { }
                //    try { sEmisorNombre = xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value; }
                //    catch { }
                //    try { sReceptorNombre = xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value; }
                //    catch { }

                //    sUUID = xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;

                //    sFechaTimbrado = Convert.ToDateTime(xdDocumento.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value);


                //    string cadenaConDP = "Persist Security Info=False;User ID=sa;Initial Catalog=PAXUnionProgreso;Data Source=UnionP;Password=P4ssw0rd";
                //    int retVal = 0;
                //    using (SqlConnection con = new SqlConnection(cadenaConDP))
                //    {
                //        con.Open();

                //        using (SqlTransaction tran = con.BeginTransaction())
                //        {
                //            try
                //            {
                //                using (SqlCommand command = new SqlCommand("usp_Cfd_Comprobante_temp_Ins", con))
                //                {

                //                    command.Transaction = tran;
                //                    command.CommandType = System.Data.CommandType.StoredProcedure;
                //                    command.CommandTimeout = 200;

                //                    command.Parameters.AddWithValue("sXML", xdDocumento.DocumentElement.OuterXml);
                //                    command.Parameters.AddWithValue("sNombreArchivo", Path.GetFileNameWithoutExtension(nombreArchivo));
                //                    command.Parameters.AddWithValue("sUuid", sUUID);
                //                    command.Parameters.AddWithValue("dFechaTimbrado", sFechaTimbrado);
                //                    command.Parameters.AddWithValue("sRFCEmisor", sRfcEmisor);
                //                    command.Parameters.AddWithValue("sNombreEmisor", sEmisorNombre);
                //                    command.Parameters.AddWithValue("sRFCReceptor", sRfcReceptor);
                //                    command.Parameters.AddWithValue("sNombreReceptor", sReceptorNombre);
                //                    command.Parameters.AddWithValue("sSerie", sSerie);
                //                    command.Parameters.AddWithValue("sFolio", sfolio);
                //                    command.Parameters.AddWithValue("nTotal", sTotal);
                //                    retVal = Convert.ToInt32(command.ExecuteScalar());
                //                }

                //                tran.Commit();

                //                Console.WriteLine("Insertado " + retVal);
                //                Console.ReadLine();
                //            }
                //            catch (Exception ex)
                //            {
                //                Console.WriteLine("Error " + ex.Message);
                //                Console.ReadLine();
                //            }
                //        }
                //    }

                //}



                //var listaArchivos = RecuperaListaArchivos(@"C:\Users\ismael.hidalgo\Desktop\PendientesEntrega\");

                //foreach (string nombreArchivo in listaArchivos)
                //{
                //    string sUUID = string.Empty;
                //    string sSerie = string.Empty;
                //    string sfolio = string.Empty;
                //    string sTotal = string.Empty;
                //    string sRfcEmisor = string.Empty;
                //    string sRfcReceptor = string.Empty;
                //    string sEmisorNombre = string.Empty;
                //    string sFechaTimbrado = string.Empty;
                //    DateTime dFechaComprobante;
                //    string sReceptorNombre = string.Empty;
                //    string sCadenaOriginal = string.Empty;
                //    string sCadenaOriginalEmisor = string.Empty;
                //    string sHASHEmisor = string.Empty;
                //    string sHASHTimbreFiscal = string.Empty;
                //    string sMoneda = string.Empty;

                //    XmlDocument xdDocumento = new XmlDocument();
                //    xdDocumento.Load(nombreArchivo);

                //    XPathNavigator navNodoTimbre = xdDocumento.CreateNavigator();

                //    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xdDocumento.NameTable);
                //    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                //    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                //    nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    
                //    sCadenaOriginalEmisor = fnObtenerCadenaOriginalEmisor(navNodoTimbre);

                //    sHASHEmisor = fnGetHASH(sCadenaOriginalEmisor).ToUpper();

                //    XmlReader reader = XmlReader.Create(new StringReader(xdDocumento.InnerXml));
                //    XElement root = XElement.Load(reader);

                //    XElement childTimbre = root.XPathSelectElement("./cfdi:Complemento", nsmComprobante);

                //    sCadenaOriginal = fnObtenerCadenaOriginalTimbreFiscal(childTimbre.CreateNavigator());
                //    sHASHTimbreFiscal = fnGetHASH(sCadenaOriginal).ToUpper();

                //    dFechaComprobante = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;

                //    try { sSerie = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value; }
                //    catch { }
                //    try { sfolio = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
                //    catch { }
                //    try { sTotal = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value; }
                //    catch { }
                //    try { sRfcEmisor = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value; }
                //    catch { }
                //    try { sRfcReceptor = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value; }
                //    catch { }
                //    try { sEmisorNombre = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value; }
                //    catch { }
                //    try { sReceptorNombre = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value; }
                //    catch { }
                //    try { sMoneda = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante).Value; }
                //    catch (Exception) { }

                //    sUUID = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value;

                //    sFechaTimbrado = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;

                //    dFechaComprobante = navNodoTimbre.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).ValueAsDateTime;

                //    string cadenaConDP = "Data Source=10.54.57.195;Initial Catalog=CFDI_Grandes_Crypto;Persist Security Info=True;User ID=corpusgood;Password=F4cturax10n";
                //    int retVal = 0;
                //    using (SqlConnection con = new SqlConnection(cadenaConDP))
                //    {
                //        con.Open();

                //        using (SqlTransaction tran = con.BeginTransaction())
                //        {
                //            try
                //            {
                //                retVal = 0;
                //                using (SqlCommand command = new SqlCommand("usp_dp_Timbrado_InsertaComprobanteAll_Ins_ftp", con))
                //                {
                //                    command.Transaction = tran;
                //                    command.CommandType = CommandType.StoredProcedure;
                //                    command.CommandTimeout = 200;
                //                    command.Parameters.AddWithValue("@sXML", PAXCrypto.CryptoAES.EncriptaAES(xdDocumento.InnerXml));
                //                    command.Parameters.AddWithValue("@nId_tipo_documento", 1);
                //                    command.Parameters.AddWithValue("@cEstatus", "P");
                //                    command.Parameters.AddWithValue("@dFecha_Documento", dFechaComprobante.ToString());
                //                    command.Parameters.AddWithValue("@nId_estructura", 1005);
                //                    command.Parameters.AddWithValue("@nId_usuario_timbrado", 1005);
                //                    command.Parameters.AddWithValue("@nSerie", "N/A");
                //                    command.Parameters.AddWithValue("@sOrigen", "I");
                //                    command.Parameters.AddWithValue("@sHash", sHASHTimbreFiscal);
                //                    command.Parameters.AddWithValue("@sDatos", sHASHEmisor);
                //                    command.Parameters.AddWithValue("@sUuid", sUUID);
                //                    command.Parameters.AddWithValue("@dFecha_Timbrado", sFechaTimbrado);
                //                    command.Parameters.AddWithValue("@sRFC_Emisor", sRfcEmisor);
                //                    command.Parameters.AddWithValue("@sNombre_Emisor", sEmisorNombre);
                //                    command.Parameters.AddWithValue("@sRFC_Receptor", sRfcReceptor);
                //                    command.Parameters.AddWithValue("@sNombre_Receptor", sReceptorNombre);
                //                    command.Parameters.AddWithValue("@dFecha_Emision", dFechaComprobante.ToString());
                //                    command.Parameters.AddWithValue("@sSerie", sSerie);
                //                    command.Parameters.AddWithValue("@sFolio", sfolio);
                //                    command.Parameters.AddWithValue("@nTotal", PAXCrypto.CryptoAES.EncriptaAES(sTotal));
                //                    command.Parameters.AddWithValue("@sMoneda", sMoneda);
                //                    retVal = Convert.ToInt32(command.ExecuteScalar());
                //                }

                //                if (retVal > 0)
                //                {
                //                    tran.Commit();
                //                }
                //                else
                //                {
                //                    tran.Rollback();
                //                }

                //                Console.WriteLine("Insertado " + retVal);
                //                Console.ReadLine();
                //            }
                //            catch (Exception ex)
                //            {
                //                Console.WriteLine("Error " + ex.Message);
                //                Console.ReadLine();
                //            }
                //        }
                //    }

                //}


                //string sEncriptado = string.Empty;
                //string sEncriptado_2 = string.Empty;

                //sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=DB-DESAROLLO;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Configuracion;Password=F4cturax10n_CnF");
                //sEncriptado_2 = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=DB-DESAROLLO;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Control;Password=F4cturax10n_C0ProK");
                //sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=DB-DESAROLLO;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=InicioSesion;Password=D4tosInicioSesionF4cturax10n");
                //sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=DB-DESAROLLO;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Consultas;Password=F4cturax10n_c0N; Asynchronous Processing = true");
                //sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=DB-DESAROLLO;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Timbrado;Password=D4tosTimbradoF4cturax10n");

                //sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=implementacion;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Configuracion;Password=F4cturax10n_CnF");
                //sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=implementacion;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Control;Password=F4cturax10n_C0ProK");
                //sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=implementacion;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=InicioSesion;Password=D4tosInicioSesionF4cturax10n");
                //sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=implementacion;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Consultas;Password=F4cturax10n_c0N; Asynchronous Processing = true");
                //sEncriptado = PAXCrypto.CryptoAES.EncriptarAES64("Data Source=implementacion;Initial Catalog=CFDI_Crypto_Test;Persist Security Info=True;User ID=Timbrado;Password=D4tosTimbradoF4cturax10n");



                //byte[] abResultado = null;

                //System.Data.DataTable dtTimbrado = new System.Data.DataTable();
                //using (SqlConnection scConexion = new SqlConnection())
                //{
                //    scConexion.ConnectionString = "Data Source=DB-DESAROLLO;Initial Catalog=CFDI_Crypto;Persist Security Info=True;User ID=Desarrollo;Password=F4cturax10n";
                //    scConexion.Open();
                //    try
                //    {
                //        using (SqlCommand scoComando = new SqlCommand())
                //        {
                //            scoComando.Connection = scConexion;
                //            scoComando.CommandType = System.Data.CommandType.Text;
                //            scoComando.CommandText = "select [dbo].[PAXCryptoDesencriptarClaveAESB]([dbo].[PAXCryptoEncriptarClaveAESB](convert(varbinary(8), 123456789.999999)))";

                //            abResultado = (byte[])scoComando.ExecuteScalar();
                            
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        //clsLog.EscribirLog("Error al obtener los datos para el reporte de timbrado -" + e.Message);
                //    }
                //}

                //float b = System.BitConverter.ToSingle(abResultado, 0);
                //decimal dec = Convert.ToDecimal(b);

                //string original = "Here is some data to encrypt!";

                // Create a new instance of the AesCryptoServiceProvider 
                // class.  This generates a new key and initialization  
                // vector (IV). 
                //using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                //{

                //    // Encrypt the string to an array of bytes. 
                //    byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

                //    // Decrypt the bytes to a string. 
                //    string roundtrip = DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

                //    //Display the original data and the decrypted data.
                //    Console.WriteLine("Original:   {0}", original);
                //    Console.WriteLine("Round Trip: {0}", roundtrip);
                //}





                //Console.WriteLine(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz"));
                //Console.ReadLine();


                /*Timbrado*/

                //var listaArchivos = RecuperaListaArchivos(@"D:\PAXRegeneracionBateriaGT\Archivos Generados\");

                /*
                foreach (string nombreArchivo in listaArchivos)
                {
                    XmlDocument xDocTimbrado = new XmlDocument();
                    xDocTimbrado.Load(nombreArchivo);
                    
                    Console.WriteLine(Path.GetFileNameWithoutExtension(nombreArchivo));

                    wsTimbradoRetencionesTest.wcfRecepcionASMXSoapClient wsServicio = new wsTimbradoRetencionesTest.wcfRecepcionASMXSoapClient();
                    string sResultado = wsServicio.fnEnviarXML(xDocTimbrado.InnerXml, 25, 0, "ws_retenciones", "wrfCv8SVxITEscO8w43DhsSCxLBZcsKbwr/Em8OtwoJNwqTDo++9uO+/oO+/pREB77+Z776J772l", "1.0");

                    //wsTimbradoRetencionLocal.wcfRecepcionASMXSoapClient wsServicio = new wsTimbradoRetencionLocal.wcfRecepcionASMXSoapClient();
                    //string sResultado = wsServicio.fnEnviarXML(xDocTimbrado.InnerXml, 25, 0, "ws_retenciones", "wrfCv8SVxITEscO8w43DhsSCxLBZcsKbwr/Em8OtwoJNwqTDo++9uO+/oO+/pREB77+Z776J772l", "1.0");

                    Console.WriteLine(sResultado);
                    Console.ReadLine();

                    File.WriteAllText(@"D:\PAXRegeranacionBateriaRetenciones\Salida\" + Path.GetFileNameWithoutExtension(nombreArchivo) + ".txt", sResultado);
                }*/

                /*Validación*/
                //XmlDocument xDocTimbrado = new XmlDocument();
                //xDocTimbrado.Load(@"C:\Users\Ismael.Hidalgo\Desktop\Timbrados\Timbrados\FielPFisica.xml");

                //wsValidacionRetencionesTest.wcfValidaASMXSoapClient wsServicio = new wsValidacionRetencionesTest.wcfValidaASMXSoapClient();
                //string sResultado = wsServicio.fnValidaXML(xDocTimbrado.InnerXml, "ws_retenciones", "wrfCv8SVxITEscO8w43DhsSCxLBZcsKbwr/Em8OtwoJNwqTDo++9uO+/oO+/pREB77+Z776J772l", "1.0");

                //Console.WriteLine(sResultado);
                //Console.ReadLine();


                /* Generacion y Timbrado */
                //string sComprobante = File.ReadAllText(@"C:\Users\Ismael.Hidalgo\Desktop\Timbrados\Timbrados\normal.txt", Encoding.UTF8);

                //wsGeneracionTimbradoTest.wcfRecepcionASMXSoapClient wsServicio = new wsGeneracionTimbradoTest.wcfRecepcionASMXSoapClient();
                //string sResultado = wsServicio.fnEnviarTXT(sComprobante, 1, 0, "ws_retenciones", "wrfCv8SVxITEscO8w43DhsSCxLBZcsKbwr/Em8OtwoJNwqTDo++9uO+/oO+/pREB77+Z776J772l", "1.0");
                //Console.WriteLine(sResultado);
                //Console.ReadLine();



                /* Generacion y Timbrado */
                
                //foreach (string nombreArchivo in listaArchivos)
                //{
                //    wsGeneracionTimbradoTest.ArrayOfAnyType sResultado = new wsGeneracionTimbradoTest.ArrayOfAnyType();
                //    string sComprobante = File.ReadAllText(nombreArchivo, Encoding.UTF8);
                //    wsGeneracionTimbradoTest.wcfRecepcionASMXSoapClient wsServicio = new wsGeneracionTimbradoTest.wcfRecepcionASMXSoapClient();
                //    sResultado = wsServicio.fnEnviarTXTPAX001(sComprobante, "factura", 0, "WSDL_PAX", "O0PCvcOOwqFNU1LCkcKs77+o77+fIWB9wr9KMe+/vUjvv41YMu+/ou+/lu++uO++mu+8pw==", "3.2", "GT");
                //}

                //foreach (string nombreArchivo in listaArchivos)
                //{
                //    string sResultado = string.Empty;
                //    string sComprobante = File.ReadAllText(nombreArchivo, Encoding.UTF8);
                //    wsGeneracionTimbradoWS.wcfRecepcionASMXSoapClient wsServicio = new wsGeneracionTimbradoWS.wcfRecepcionASMXSoapClient();
                //    sResultado = wsServicio.fnEnviarTXT(sComprobante, "factura", 0, "Paxgeneracion", "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB", "3.2", "GT");
                //}
                
                
                //foreach (string nombreArchivo in listaArchivos)
                //{
                //    string sComprobante = File.ReadAllText(nombreArchivo, Encoding.UTF8);
                //    wsGeneracionTimbradoWS.wcfRecepcionASMXSoapClient wsServicio = new wsGeneracionTimbradoWS.wcfRecepcionASMXSoapClient();
                //    wsGeneracionTimbradoWS.ArrayOfAnyType sResultado = wsServicio.fnEnviarTXTPAX001(sComprobante, "factura", 0, "Paxgeneracion", "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB", "3.2", "GT");
                //}
                
                //foreach (string nombreArchivo in listaArchivos)
                //{
                //    string sComprobante = File.ReadAllText(nombreArchivo, Encoding.UTF8);
                //    wsGeneracionTimbradoSVCWS.IwcfRecepcionClient wsServicio = new wsGeneracionTimbradoSVCWS.IwcfRecepcionClient();
                //    string sResultado = wsServicio.fnEnviarTXT(sComprobante, "factura", 0, "Paxgeneracion", "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB", "3.2", "GT");
                //}

                //string sComprobante = File.ReadAllText(@"D:\Mis Documentos\IBM\Ruta GT\Layout_3_2.txt", Encoding.UTF8);

                ////wsGeneracionTimbradoWS.wcfRecepcionASMXSoapClient wsServicio = new wsGeneracionTimbradoWS.wcfRecepcionASMXSoapClient();
                ////string sResultado = wsServicio.fnEnviarTXT(sComprobante, "factura", 0, "Paxgeneracion", "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB", "3.2", "GT");

                //wsGeneracionTimbradoSVCWS.IwcfRecepcionClient wsServicio = new wsGeneracionTimbradoSVCWS.IwcfRecepcionClient();
                //string sResultado = wsServicio.fnEnviarTXT(sComprobante, "factura", 0, "Paxgeneracion", "wqrCssSoxKvDgsOww6rEr8SPxIPvv6jvv61gXsKM77+2d2tVZCbvv67vv4/vv4vvvofvvqrvvYbvvLIB", "3.2", "GT");

                //XmlDocument xd = new XmlDocument();
                //xd.LoadXml(sResultado);

                //Console.WriteLine(sResultado);
                //Console.ReadLine();




                //string sComprobante = "﻿ret?Version@1.0|FolioInt@01234567890123456789|Sello@|NumCert@20001000000300003693|Cert@|FechaExp@2015-05-31T12:08:14-06:00|CveRetenc@25|DescRetenc@descripcion retencion" + Environment.NewLine +
                //        "re?RFCEmisor@DCO020624P10|NomDenRazSocE@PAX.CORPUS|CURPE@AAQM010101HCSMNZ00" + Environment.NewLine +
                //        "rr?Nacionalidad@Nacional" + Environment.NewLine +
                //        "rn?RFCRecep@AAA010101AAA|NomDenRazSocR@PAX.NACIONAL|CURPR@AAQM010101HCSMNZ00" + Environment.NewLine +
                //        "rp?MesIni@10|MesFin@10|Ejerc@2014" + Environment.NewLine +
                //        "rt?montoTotOperacion@1600.000000|montoTotGrav@1200.000000|montoTotExent@400.000000|montoTotRet@200.000000" + Environment.NewLine +
                //        "ri?BaseRet@150.000000|Impuesto@01|montoRet@15.000000|TipoPagoRet@Pago definitivo";

                //wsGeneracionTimbradoTest.wcfRecepcionASMXSoapClient wsServicio = new wsGeneracionTimbradoTest.wcfRecepcionASMXSoapClient();
                //string sResultado = wsServicio.fnEnviarTXT(sComprobante, 20, 0, "ws_retenciones", "wrfCv8SVxITEscO8w43DhsSCxLBZcsKbwr/Em8OtwoJNwqTDo++9uO+/oO+/pREB77+Z776J772l", "1.0");
                //Console.WriteLine(sResultado);
                //Console.ReadLine();



                //XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
                //nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
                //nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                //DateTime dFechaComprobante = xDocTimbrado.CreateNavigator().SelectSingleNode("/retenciones:Retenciones/@FechaExp", nsmComprobante).ValueAsDateTime;
                

                //Console.WriteLine("Iniciar");
                //Console.WriteLine(DateTime.Now);
                //Console.ReadLine();

                //for (int i = 0; i < 10000; i++)
                //{
                //    string sResultado = fnGenerarComprobante("﻿co?version@3.2|serie@A|folio@123|fecha@2015-03-24T15:50:34|formaDePago@Pago en una sola exhibicion|noCertificado@20001000000100005867|condicionesDePago@8 dias fecha factura|subTotal@4938.53|descuento@448.69|motivoDescuento@Deducciones nómina|TipoCambio@0.0|Moneda@MXN|total@4079.22|tipoDeComprobante@egreso|metodoDePago@Deposito|LugarExpedicion@Mexico,Juarez|NumCtaPago@1238" +
                //        "re?rfc@AAA010101AAA|nombre@KEYTRONIC JUAREZ S.A. DE C.V." +
                //        "de?calle@THOMAS BECKET|noExterior@2220|noInterior@1-A|colonia@PARQUE IND. GEMA|localidad@JUAREZ|referencia@Despues de las Vias|municipio@JUAREZ|estado@CHIHUAHUA|pais@México|codigoPostal@32380" +
                //        "ee?calle@CONSTITUCION|noExterior@404 SUR|noInterior@2-A|colonia@DURANGO CENTRO|localidad@VICTORIA DE DURANGO|referencia@Despues del Puente|municipio@DURANGO|estado@DURANGO|pais@México|codigoPostal@34000" +
                //        "rf?Regimen@Regimen General de Ley" +
                //        "rr?rfc@MAMR850719LT8|nombre@MARTINEZ MUNOZ RICARDO" +
                //        "dr?calle@PRESA DE LA ANGOSTURA|noExterior@7597|noInterior@478|colonia@INDEPENDENCIA 1|localidad@JUAREZ|referencia@Entre calle Encino y Sicomoro|municipio@JUAREZ|estado@CHIHUAHUA|pais@México|codigoPostal@32640" +
                //        "cc?cantidad@1.00|unidad@Servicio|noIdentificacion@2014NO7|descripcion@PAGO DE NÓMINA|valorUnitario@4938.53|importe@4938.53" +
                //        "im?totalImpuestosRetenidos@410.62" +
                //        "ir?impuesto@ISR|importe@410.62"
                //    );
                //}

                //Console.WriteLine("Se termino fnGenerarComprobante - " + DateTime.Now.ToString("dd-mm-yyyy hh:mm:ss.fff"));

                //for (int i = 0; i < 10000; i++)
                //{
                //    string sResultado_1 = fnGenerarComprobante_1("﻿co?version@3.2|serie@A|folio@123|fecha@2015-03-24T15:50:34|formaDePago@Pago en una sola exhibicion|noCertificado@20001000000100005867|condicionesDePago@8 dias fecha factura|subTotal@4938.53|descuento@448.69|motivoDescuento@Deducciones nómina|TipoCambio@0.0|Moneda@MXN|total@4079.22|tipoDeComprobante@egreso|metodoDePago@Deposito|LugarExpedicion@Mexico,Juarez|NumCtaPago@1238" +
                //        "re?rfc@AAA010101AAA|nombre@KEYTRONIC JUAREZ S.A. DE C.V." +
                //        "de?calle@THOMAS BECKET|noExterior@2220|noInterior@1-A|colonia@PARQUE IND. GEMA|localidad@JUAREZ|referencia@Despues de las Vias|municipio@JUAREZ|estado@CHIHUAHUA|pais@México|codigoPostal@32380" +
                //        "ee?calle@CONSTITUCION|noExterior@404 SUR|noInterior@2-A|colonia@DURANGO CENTRO|localidad@VICTORIA DE DURANGO|referencia@Despues del Puente|municipio@DURANGO|estado@DURANGO|pais@México|codigoPostal@34000" +
                //        "rf?Regimen@Regimen General de Ley" +
                //        "rr?rfc@MAMR850719LT8|nombre@MARTINEZ MUNOZ RICARDO" +
                //        "dr?calle@PRESA DE LA ANGOSTURA|noExterior@7597|noInterior@478|colonia@INDEPENDENCIA 1|localidad@JUAREZ|referencia@Entre calle Encino y Sicomoro|municipio@JUAREZ|estado@CHIHUAHUA|pais@México|codigoPostal@32640" +
                //        "cc?cantidad@1.00|unidad@Servicio|noIdentificacion@2014NO7|descripcion@PAGO DE NÓMINA|valorUnitario@4938.53|importe@4938.53" +
                //        "im?totalImpuestosRetenidos@410.62" +
                //        "ir?impuesto@ISR|importe@410.62"
                //    );
                //}

                //Console.WriteLine("Se termino fnGenerarComprobante_1 - " + DateTime.Now.ToString("dd-mm-yyyy hh:mm:ss.fff"));
                //Console.ReadLine();

                //char[] acAlfabeto = null;
                //string[] asaAlfabeto = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,AA,AB".Split(','); 

                //acAlfabeto = new char[asaAlfabeto.Count()];
                //for (int i = 0; i < asaAlfabeto.Count(); i++)
                //{
                //    acAlfabeto[i] = Convert.ToChar(Convert.ToByte(asaAlfabeto[i]));
                //}


                //Console.WriteLine("Iniciar");
                //Console.WriteLine(DateTime.Now);
                //Console.ReadLine();



                
                ////char[] scAlfabeto = sAlfabeto.Select(c => ;
                ////sAlfabeto = (string[])asAlfabeto;
                //string sDimension = "A:AB";
                //string[] asDimesiones = sDimension.Split(':');
                //string sColumnaInferior = string.Empty;
                //string sColumnaSuperior = string.Empty;

                //string[] asCeldaValorInferior = asDimesiones[0].Split(acAlfabeto);
                //string[] asCeldaValorSuperior = asDimesiones[1].Split(acAlfabeto);

                //String[] elementos = "0,1".Split(',');
                //int n = 2;                  //Tipos para escoger
                //int r = elementos.Length;   //Elementos elegidos
                //fnPermutar(elementos, "", n, r);

                //MailMessage mail = new MailMessage();

                //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                //mail.From = new MailAddress("ismael.hidalgo@paxfacturacion.com");
                //mail.To.Add("ismael.hidalgo@paxfacturacion.com");
                //mail.Subject = "Test Mail";
                //mail.Body = "This is for testing SMTP mail from GMAIL";


                //SmtpServer.Port = 587;
                //SmtpServer.Credentials = new System.Net.NetworkCredential("ismael.hidalgo@paxfacturacion.com", "aia175671");
                //SmtpServer.EnableSsl = true;
                //SmtpServer.Send(mail);

                //Console.WriteLine("mail Send");
                //Console.ReadLine();
            
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
                Console.ReadLine();
            }
        }

        public static void fnObtenerValoresComplementos(string psNombreComplemento, ref string psPrefijo, ref string psNameSpace, ref string psNameSpeceURI)
        {
            try
            {
                switch (psNombreComplemento)
                {
                    case "Aerolineas":
                        psPrefijo = "aerolineas";
                        psNameSpace = "Aerolineas";
                        psNameSpeceURI = "http://www.sat.gob.mx/aerolineas";
                        break;
                    case "certificadodedestruccion":
                        psPrefijo = "destruccion";
                        psNameSpace = "certificadodedestruccion";
                        psNameSpeceURI = "http://www.sat.gob.mx/certificadodestruccion";
                        break;
                    case "ConsumoDeCombustibles":
                        psPrefijo = "consumodecombustibles";
                        psNameSpace = "ConsumoDeCombustibles";
                        psNameSpeceURI = "http://www.sat.gob.mx/consumodecombustibles";
                        break;
                    case "Divisas":
                        psPrefijo = "divisas";
                        psNameSpace = "Divisas";
                        psNameSpeceURI = "http://www.sat.gob.mx/divisas";
                        break;
                    case "Donatarias":
                        psPrefijo = "donat";
                        psNameSpace = "Donatarias";
                        psNameSpeceURI = "http://www.sat.gob.mx/donat";
                        break;
                    case "LeyendasFisc":
                        psPrefijo = "LeyendasFiscales";
                        psNameSpace = "LeyendasFisc";
                        psNameSpeceURI = "http://www.sat.gob.mx/leyendasFiscales";
                        break;
                    case "INE":
                        psPrefijo = "ine";
                        psNameSpace = "INE";
                        psNameSpeceURI = "http://www.sat.gob.mx/ine";
                        break;
                    case "PagoEnEspecie":
                        psPrefijo = "pagoenespecie";
                        psNameSpace = "PagoEnEspecie";
                        psNameSpeceURI = "http://www.sat.gob.mx/pagoenespecie";
                        break;

                }
            }
            catch (Exception ex)
            { 
            
            }
        }

        public static List<int> GetDurationInEnglish(DateTime from, DateTime to)
        {
            try
            {
                if (from > to)
                    return null;

                var fY = from.Year;
                var fM = from.Month;
                var fD = DateTime.DaysInMonth(fY, fM);

                var tY = to.Year;
                var tM = to.Month;
                var tD = DateTime.DaysInMonth(tY, tM);

                int dY = 0;
                int dM = 0;
                int dD = 0;

                if (fD > tD)
                {
                    tM--;

                    if (tM <= 0)
                    {
                        tY--;
                        tM = 12;
                        tD += DateTime.DaysInMonth(tY, tM);
                    }
                    else
                    {
                        tD += DateTime.DaysInMonth(tY, tM);
                    }
                }
                dD = tD - fD;

                if (fM > tM)
                {
                    tY--;

                    tM += 12;
                }
                dM = tM - fM;

                dY = tY - fY;

                return new List<int>() { dY, dM, dD };
            }
            catch (Exception exception)
            {
                //todo: log exception with parameters in db

                return null;
            }
        }

        public static IList RecuperaListaArchivos(string directorioRaiz)
        {
            IList listaArchivos = Directory.GetFiles(directorioRaiz).ToList();
            return listaArchivos;
        }

        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an AesCryptoServiceProvider object 
            // with the specified key and IV. 
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        /// <summary>
        /// Función que se encarga de generar el Hash de una cadena
        /// </summary>
        /// <param name="psCadena">Cadena</param>
        /// <returns></returns>
        private static string fnGetHASH(string psCadena)
        {
            byte[] hashValue;
            byte[] message = Encoding.UTF8.GetBytes(psCadena);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            } return hex;
        }

        /// <summary>
        /// Funció que se encarga de generar la cadena original del emisor
        /// </summary>
        /// <param name="navNodo"></param>
        /// <returns></returns>
        private static string fnObtenerCadenaOriginalEmisor(XPathNavigator navNodo)
        {
            MemoryStream ms;
            StreamReader srDll;
            string sResultado = string.Empty;
            XslCompiledTransform xslt;
            XsltArgumentList args;
            try
            {
                // Hash Emisor
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(CaOri.V32));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(navNodo, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sResultado = srDll.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la cadena original del emisor: " + ex.Message);
            }
            return sResultado;
        }

        /// <summary>
        /// Función que se encarga de obtener la cadena original del timbre fiscal
        /// </summary>
        /// <param name="navNodo"></param>
        /// <returns></returns>
        private static string fnObtenerCadenaOriginalTimbreFiscal(XPathNavigator navNodo)
        {
            MemoryStream ms;
            StreamReader srDll;
            string sResultado = string.Empty;
            XslCompiledTransform xslt;
            XsltArgumentList args;
            try
            {
                xslt = new XslCompiledTransform();
                xslt.Load(typeof(Timbrado.V3.TFDXSLT));
                ms = new MemoryStream();
                args = new XsltArgumentList();
                xslt.Transform(navNodo, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                srDll = new StreamReader(ms);

                sResultado = srDll.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la cadena original del timbre fiscal: " + ex.Message);
            }
            return sResultado;
        }


        private static clsValCertificado fnRecuperarCertificado(string psComprobante)
        {
            //recuperamos el certificado del comprobante
            XmlDocument xDocTimbrado = new XmlDocument();
            xDocTimbrado.LoadXml(psComprobante);

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xDocTimbrado.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            string sCertificadoBase64 = xDocTimbrado.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).Value;
            if (string.IsNullOrEmpty(sCertificadoBase64))
            {
                //errorCode = 570; //No se pudó recuperar el certificado del comprobante
                return null;
            } return new clsValCertificado(Convert.FromBase64String(sCertificadoBase64));
        }

        private static bool fnRecuperaFechaLCO(string sNoCertificado, string estatus, ref clsValCertificado vValidadorCertificado)
        {
            DataTable fechas = vValidadorCertificado.RevisaExistenciaCertificadoFechas(sNoCertificado, estatus);
            if (Convert.ToDateTime(fechas.Rows[0]["fecha_inicio"].ToString()).CompareTo(DateTime.Today) > 0
            || Convert.ToDateTime(fechas.Rows[0]["fecha_final"].ToString()).CompareTo(DateTime.Today) < 0)
                return false;

            return true;
        }

        public static XmlNode RenameNode(XmlNode e, string psNombreNuevo, string psPrefijo, string psUrl)
        {
            XmlDocument doc = e.OwnerDocument;
            XmlNode newNode = doc.CreateNode(e.NodeType, psNombreNuevo, psUrl);
            newNode.Prefix = psPrefijo;
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


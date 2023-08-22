using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Data.SqlClient;

/// <summary>
/// Clase encargada de manjera la lista de plantillas desponibles por usuario.
/// </summary>
public class clsPlantillaLista
{
    public clsPlantillaLista()
	{

	}

    public void fnObtenerPLantilla(XmlDocument pxComprobante, string sPlantilla, string psIdCfd, string psTipoDocumento, System.Web.UI.Page pagina, string sRuta, int id_contribuyente, int id_rfc, string scolor)
    {
        
        switch (sPlantilla)
        {
                
            case "Logo":

                clsPlantillaLogo pdfLogo = new clsPlantillaLogo(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdfLogo.TipoDocumento = psTipoDocumento.ToUpper();
                pdfLogo.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                pdfLogo.fnMostrarPDF(pagina);
                break;


            case "V2":

                clsOperacionConsultaPdfV2 pdf2 = new clsOperacionConsultaPdfV2(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdf2.TipoDocumento = psTipoDocumento.ToUpper();
                pdf2.fnGenerarPDF();

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                pdf2.fnMostrarPDF(pagina);
                break;

            case "Ayudar es Amar":
                clsPlantillaAyudarEsAmarAC pdfAyudar = new clsPlantillaAyudarEsAmarAC(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdfAyudar.TipoDocumento = psTipoDocumento.ToUpper();
                pdfAyudar.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                pdfAyudar.fnMostrarPDF(pagina);
                break;

            case "Alberto Gutiérrez":
                clsPlantillaAlbertoGutierrez PdfAlbertoGtz = new clsPlantillaAlbertoGutierrez(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfAlbertoGtz.TipoDocumento = psTipoDocumento.ToUpper();
                PdfAlbertoGtz.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfAlbertoGtz.fnMostrarPDF(pagina);
                break;

            case "Constructora GUET":
                clsPlantillaConstGUET PdfConstGUET = new clsPlantillaConstGUET(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfConstGUET.TipoDocumento = psTipoDocumento.ToUpper();
                PdfConstGUET.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfConstGUET.fnMostrarPDF(pagina);
                break;

            case "Coinsa":
                clsPlantillaCoinsa PdfCoinsa = new clsPlantillaCoinsa(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfCoinsa.TipoDocumento = psTipoDocumento.ToUpper();
                PdfCoinsa.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfCoinsa.fnMostrarPDF(pagina);
                break;

            case "Termo Climas":
                clsPlantillaTermoClimas PdfTerCli = new clsPlantillaTermoClimas(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfTerCli.TipoDocumento = psTipoDocumento.ToUpper();
                PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfTerCli.fnMostrarPDF(pagina);
                break;

            case "Le Parfum":
                clsPlantillaLeParfum PdfLeParfum = new clsPlantillaLeParfum(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfLeParfum.TipoDocumento = psTipoDocumento.ToUpper();
                PdfLeParfum.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfLeParfum.fnMostrarPDF(pagina);
                break;

            case "Marybel Le Parfum":
                clsPlantillaMarybelLeParfum PdfMarybel = new clsPlantillaMarybelLeParfum(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfMarybel.TipoDocumento = psTipoDocumento.ToUpper();
                PdfMarybel.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfMarybel.fnMostrarPDF(pagina);
                break;
            case "Blancos Jessy":
                DataTable sdrInfo = null;
                try 
                {
                    clsOperacionCuenta gDAL = new clsOperacionCuenta(); 
                    clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();
                    datosUsuario = clsComun.fnUsuarioEnSesion();
                    sdrInfo = gDAL.fnObtenerDatosFiscales();
                }
                catch { }
                
                clsPlantillaBlancosJessy PdfBlancos = new clsPlantillaBlancosJessy(pxComprobante, sdrInfo);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfBlancos.TipoDocumento = psTipoDocumento.ToUpper();
                PdfBlancos.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);
                PdfBlancos.fnMostrarPDF(pagina);
                break;
            case "CastroyReyesRetanaAbogados":
                clsPlantillaCastro Pdfcastro = new clsPlantillaCastro(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    Pdfcastro.TipoDocumento = psTipoDocumento.ToUpper();
                Pdfcastro.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                Pdfcastro.fnMostrarPDF(pagina);
                break;
            case "Fideapech":
                //*************Se agrega Adenda a XML***************
                clsConfiguracionAddenda gADD = new clsConfiguracionAddenda();
                DataTable addenda = new DataTable();
                int idEstructura = 0;
                string AddendaNamespace = string.Empty;
                 


                idEstructura = gADD.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
                addenda = gADD.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructura);
                if (addenda.Rows.Count > 0)
                {
                    XmlDeclaration xDec = pxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                    pxComprobante.InsertBefore(xDec, pxComprobante.DocumentElement);

                    XmlDocument xAddenda = new XmlDocument();
                    int idModulo = Convert.ToInt32(addenda.Rows[0]["id_modulo"]);
                    xAddenda.LoadXml(Convert.ToString(addenda.Rows[0]["addenda"]));
                    AddendaNamespace = gADD.fnObtieneNameSpace(idModulo);

                    if (AddendaNamespace != "")
                    {
                        string[] nombre = AddendaNamespace.Split('=');
                        XmlAttribute xAttribute = pxComprobante.CreateAttribute(nombre[0]);
                        xAttribute.InnerText = AddendaNamespace;
                        pxComprobante.ChildNodes[1].Attributes.Append(xAttribute);
                    }


                    XmlNode childElement = pxComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobante.DocumentElement.NamespaceURI);
                    pxComprobante.ChildNodes[1].AppendChild(childElement);

                    childElement.InnerXml = xAddenda.OuterXml;

                    clsPlantillaFideapech PdfFideapech = new clsPlantillaFideapech(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfFideapech.TipoDocumento = psTipoDocumento.ToUpper();
                    PdfFideapech.fnGenerarPDF(scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfFideapech.fnMostrarPDF(pagina);
                }
                else
                {

                    clsOperacionConsultaPdf pdf = new clsOperacionConsultaPdf(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdf.TipoDocumento = psTipoDocumento.ToUpper();
                    if (scolor == string.Empty || scolor == null)
                        scolor = "Black";

                    pdf.fnGenerarPDF(scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    pdf.fnMostrarPDF(pagina);
                }
                
                break;
            case "BalatasyEmbragues":
                clsPlantillaBalatasyEmbragues PdfBalatas = new clsPlantillaBalatasyEmbragues(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfBalatas.TipoDocumento = psTipoDocumento.ToUpper();
                PdfBalatas.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfBalatas.fnMostrarPDF(pagina);
                break;

            case "Promotora":
                clsPlantillaPromotora PdfPromotora = new clsPlantillaPromotora(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfPromotora.TipoDocumento = psTipoDocumento.ToUpper();
                PdfPromotora.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfPromotora.fnMostrarPDF(pagina);
                break;

            case "PAVILSA":
             
                   //*************Se agrega Adenda a XML***************
                clsConfiguracionAddenda gADDPav = new clsConfiguracionAddenda();
                DataTable addendaPav = new DataTable();
                int idEstructuraPav = 0;
                string AddendaNamespacePav = string.Empty;

                idEstructuraPav = gADDPav.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
                addendaPav = gADDPav.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructuraPav);
                if (addendaPav.Rows.Count > 0)
                {
                    XmlDeclaration xDec = pxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                    pxComprobante.InsertBefore(xDec, pxComprobante.DocumentElement);

                    XmlDocument xAddenda = new XmlDocument();
                    int idModulo = Convert.ToInt32(addendaPav.Rows[0]["id_modulo"]);
                    xAddenda.LoadXml(Convert.ToString(addendaPav.Rows[0]["addenda"]));
                    AddendaNamespacePav = gADDPav.fnObtieneNameSpace(idModulo);

                    if (AddendaNamespacePav != "")
                    {
                        string[] nombre = AddendaNamespacePav.Split('=');
                        XmlAttribute xAttribute = pxComprobante.CreateAttribute(nombre[0]);
                        xAttribute.InnerText = AddendaNamespacePav;
                        pxComprobante.ChildNodes[1].Attributes.Append(xAttribute);
                    }


                    XmlNode childElement = pxComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobante.DocumentElement.NamespaceURI);
                    pxComprobante.ChildNodes[1].AppendChild(childElement);

                    childElement.InnerXml = xAddenda.OuterXml;
                }
                clsPlantillaPAVILSA PdfPAVILSA = new clsPlantillaPAVILSA(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfPAVILSA.TipoDocumento = psTipoDocumento.ToUpper();
                PdfPAVILSA.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfPAVILSA.fnMostrarPDF(pagina);
                break;

            //case "Palliser":
            //    clsPlantillaPalliser PdfPalliser = new clsPlantillaPalliser(pxComprobante);

            //    if (!string.IsNullOrEmpty(psTipoDocumento))
            //        PdfPalliser.TipoDocumento = psTipoDocumento.ToUpper();
            //    PdfPalliser.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

            //    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

            //    PdfPalliser.fnMostrarPDF(pagina);
            //    break;

            //case "GeneraldeSeguros":

            //    clsPlantillaGeneraldeSeguros pdfGeneraldeSeguros = new clsPlantillaGeneraldeSeguros(pxComprobante);

            //    if (!string.IsNullOrEmpty(psTipoDocumento))
            //        pdfGeneraldeSeguros.TipoDocumento = psTipoDocumento.ToUpper();
            //    pdfGeneraldeSeguros.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

            //    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

            //    pdfGeneraldeSeguros.fnMostrarPDF(pagina);
            //    break;

            case "AcerosyEquiposMineros":

                //*************Se agrega Adenda a XML***************
                clsConfiguracionAddenda gADDAEM = new clsConfiguracionAddenda();
                DataTable addendaAEM = new DataTable();
                int idEstructuraAEM = 0;
                string AddendaNamespaceAEM = string.Empty;

                idEstructuraAEM = gADDAEM.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
                addendaAEM = gADDAEM.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructuraAEM);
                if (addendaAEM.Rows.Count > 0)
                {
                    XmlDeclaration xDec = pxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                    pxComprobante.InsertBefore(xDec, pxComprobante.DocumentElement);

                    XmlDocument xAddenda = new XmlDocument();
                    int idModulo = Convert.ToInt32(addendaAEM.Rows[0]["id_modulo"]);
                    xAddenda.LoadXml(Convert.ToString(addendaAEM.Rows[0]["addenda"]));
                    AddendaNamespaceAEM = gADDAEM.fnObtieneNameSpace(idModulo);

                    if (AddendaNamespaceAEM != "")
                    {
                        string[] nombre = AddendaNamespaceAEM.Split('=');
                        XmlAttribute xAttribute = pxComprobante.CreateAttribute(nombre[0]);
                        xAttribute.InnerText = AddendaNamespaceAEM;
                        pxComprobante.ChildNodes[1].Attributes.Append(xAttribute);
                    }


                    XmlNode childElement = pxComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobante.DocumentElement.NamespaceURI);
                    pxComprobante.ChildNodes[1].AppendChild(childElement);

                    childElement.InnerXml = xAddenda.OuterXml;
                }

                clsAcerosyEquiposMineros PdfAcerosyEquipsoMineros = new clsAcerosyEquiposMineros(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfAcerosyEquipsoMineros.TipoDocumento = psTipoDocumento.ToUpper();
                PdfAcerosyEquipsoMineros.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfAcerosyEquipsoMineros.fnMostrarPDF(pagina);
                break;

            case "GCC":
                clsPlantillaGCC PdfGCC = new clsPlantillaGCC(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    PdfGCC.TipoDocumento = psTipoDocumento.ToUpper();
                PdfGCC.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                PdfGCC.fnMostrarPDF(pagina);
                break;

            case "Beatriz":
                //*************Se agrega Adenda a XML***************
                clsConfiguracionAddenda gADDB = new clsConfiguracionAddenda();
                DataTable addendaB = new DataTable();
                int idEstructuraB = 0;
                string AddendaNamespaceB = string.Empty;

                idEstructuraB = gADDB.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
                addenda = gADDB.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructuraB);
                if (addenda.Rows.Count > 0)
                {
                    XmlDeclaration xDec = pxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                    pxComprobante.InsertBefore(xDec, pxComprobante.DocumentElement);

                    XmlDocument xAddenda = new XmlDocument();
                    int idModulo = Convert.ToInt32(addenda.Rows[0]["id_modulo"]);
                    xAddenda.LoadXml(Convert.ToString(addenda.Rows[0]["addenda"]));
                    AddendaNamespaceB = gADDB.fnObtieneNameSpace(idModulo);

                    if (AddendaNamespaceB != "")
                    {
                        string[] nombre = AddendaNamespaceB.Split('=');
                        XmlAttribute xAttribute = pxComprobante.CreateAttribute(nombre[0]);
                        xAttribute.InnerText = AddendaNamespaceB;
                        pxComprobante.ChildNodes[1].Attributes.Append(xAttribute);
                    }


                    XmlNode childElement = pxComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobante.DocumentElement.NamespaceURI);
                    pxComprobante.ChildNodes[1].AppendChild(childElement);

                    childElement.InnerXml = xAddenda.OuterXml;
                }

                clsPlantillaBeatriz pdfBeatriz = new clsPlantillaBeatriz(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdfBeatriz.TipoDocumento = psTipoDocumento.ToUpper();
                pdfBeatriz.fnGenerarPDF(id_contribuyente, id_rfc,scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                pdfBeatriz.fnMostrarPDF(pagina);
                break;

            case "QuimicaCelta":

                clsPlantillaQuimicaCelta pdfQuimicaCelta = new clsPlantillaQuimicaCelta(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdfQuimicaCelta.TipoDocumento = psTipoDocumento.ToUpper();
                pdfQuimicaCelta.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                pdfQuimicaCelta.fnMostrarPDF(pagina);
                break;

            case "AHM":

                clsPlantillaAHM pdfAHM = new clsPlantillaAHM(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdfAHM.TipoDocumento = psTipoDocumento.ToUpper();
                pdfAHM.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                pdfAHM.fnMostrarPDF(pagina);
                break;

            case "JCConstruccion":
                clsPlantillaJCConstruccion pdfJC = new clsPlantillaJCConstruccion(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdfJC.TipoDocumento = psTipoDocumento.ToUpper();
                pdfJC.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                pdfJC.fnMostrarPDF(pagina);
                break;
            case "Masa":
                clsPlantillaMasa pdfMasa = new clsPlantillaMasa(pxComprobante);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdfMasa.TipoDocumento = psTipoDocumento.ToUpper();
                pdfMasa.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                pdfMasa.fnMostrarPDF(pagina);
                break;

            case "Nomina":

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(pxComprobante.NameTable);
                nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");

                XPathNavigator nav = pxComprobante.CreateNavigator();

                string sTipoRiesgo = string.Empty;
                string sBanco = string.Empty;
                string sRegimen = string.Empty;

                string sDescripcionTipoRiesgo = string.Empty;
                string sDescripcionBanco = string.Empty;
                string sDescripcionRegimen = string.Empty;

                try { sTipoRiesgo = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@RiesgoPuesto", nsmComprobante).Value; }
                catch { }

                try { sBanco = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@Banco", nsmComprobante).Value; }
                catch { }

                try { sRegimen = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@TipoRegimen", nsmComprobante).Value; }
                catch { }

                if (!string.IsNullOrEmpty(sTipoRiesgo))
                {
                    clsOperacionRiesgoPuesto cRiesgoPuesto = new clsOperacionRiesgoPuesto();
                    DataTable dtRiesgoPuesto = cRiesgoPuesto.fnExiste(Convert.ToInt32(sTipoRiesgo));
                    sDescripcionTipoRiesgo = dtRiesgoPuesto.Rows[0]["Descripcion"].ToString();
                }

                if (!string.IsNullOrEmpty(sBanco))
                {
                    clsOperacionBanco cBanco = new clsOperacionBanco();
                    DataTable dtBanco = cBanco.fnExisteBancos(Convert.ToInt32(sBanco));
                    sDescripcionBanco = dtBanco.Rows[0]["NombreCorto"].ToString();
                }

                if (!string.IsNullOrEmpty(sRegimen))
                {
                    clsOperacionTipoRegimen cTipoRegimen = new clsOperacionTipoRegimen();
                    DataTable dtTipoRegimen = cTipoRegimen.fnExiste(Convert.ToInt32(sRegimen));
                    sDescripcionRegimen = dtTipoRegimen.Rows[0]["Descripcion"].ToString();
                }

                clsPlantillaNomina pdfNomina = new clsPlantillaNomina(pxComprobante, sDescripcionTipoRiesgo, sDescripcionBanco, sDescripcionRegimen);

                if (!string.IsNullOrEmpty(psTipoDocumento))
                    pdfNomina.TipoDocumento = psTipoDocumento.ToUpper();
                pdfNomina.fnGenerarPdf(id_contribuyente, id_rfc, scolor);

                clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                pdfNomina.fnMostrarPDF(pagina);
                break;

            default:

                if (sRuta == string.Empty)
                {
                    clsOperacionConsultaPdf pdf = new clsOperacionConsultaPdf(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdf.TipoDocumento = psTipoDocumento.ToUpper();
                    if (scolor == string.Empty || scolor == null)
                        scolor = "Black";

                    pdf.fnGenerarPDF(scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    pdf.fnMostrarPDF(pagina);
                }
                else
                {
                    clsOperacionConsultaPdf pdfsave = new clsOperacionConsultaPdf(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfsave.TipoDocumento = psTipoDocumento.ToUpper();
                    pdfsave.fnGenerarPDFSave(sRuta, scolor);

                }
                break;

        }
    }

    /// <summary>
    /// Crea archivo pdf segun plantilla configurada para su posterior envio de correo
    /// </summary>
    /// <param name="pxComprobante"></param>
    /// <param name="sPlantilla"></param>
    /// <param name="psIdCfd"></param>
    /// <param name="psTipoDocumento"></param>
    /// <param name="pagina"></param>
    /// <param name="sRuta"></param>
    /// <param name="id_contribuyente"></param>
    /// <param name="id_rfc"></param>
    /// <param name="scolor"></param>
    public void fnCrearPLantillaEnvio(XmlDocument pxComprobante, string sPlantilla, string psIdCfd, string psTipoDocumento, string sRuta, int id_contribuyente, int id_rfc, string scolor)
    {

        if (!(sRuta == string.Empty))
        {
            switch (sPlantilla)
            {

                case "Logo":

                    clsPlantillaLogo pdfLogo = new clsPlantillaLogo(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfLogo.TipoDocumento = psTipoDocumento.ToUpper();
                    pdfLogo.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);

                    //clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDFSave", "Se envio el PDF para el comprobante con ID " + psIdCfd);

                    break;


                case "V2":

                    clsOperacionConsultaPdfV2 pdf2 = new clsOperacionConsultaPdfV2(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdf2.TipoDocumento = psTipoDocumento.ToUpper();
                    pdf2.fnGenerarPDFSave(sRuta);

                    //clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDFSave", "Se envio el PDF para el comprobante con ID " + psIdCfd);

                    break;

                case "Ayudar es Amar":
                    clsPlantillaAyudarEsAmarAC pdfAyudar = new clsPlantillaAyudarEsAmarAC(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfAyudar.TipoDocumento = psTipoDocumento.ToUpper();
                    pdfAyudar.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    pdfAyudar.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "Alberto Gutiérrez":
                    clsPlantillaAlbertoGutierrez PdfAlbertoGtz = new clsPlantillaAlbertoGutierrez(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfAlbertoGtz.TipoDocumento = psTipoDocumento.ToUpper();
                    PdfAlbertoGtz.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfAlbertoGtz.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "Constructora GUET":
                    clsPlantillaConstGUET PdfConstGUET = new clsPlantillaConstGUET(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfConstGUET.TipoDocumento = psTipoDocumento.ToUpper();
                    PdfConstGUET.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfConstGUET.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "Coinsa":
                    clsPlantillaCoinsa PdfCoinsa = new clsPlantillaCoinsa(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfCoinsa.TipoDocumento = psTipoDocumento.ToUpper();
                    PdfCoinsa.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfCoinsa.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "Termo Climas":
                    clsPlantillaTermoClimas PdfTerCli = new clsPlantillaTermoClimas(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfTerCli.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfTerCli.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "Le Parfum":
                    clsPlantillaLeParfum PdfLePerfum = new clsPlantillaLeParfum(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfLePerfum.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfLePerfum.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "Marybel Le Parfum":
                    clsPlantillaMarybelLeParfum PdfMarybel = new clsPlantillaMarybelLeParfum(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfMarybel.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfMarybel.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;
                case "Blancos Jessy":
                    DataTable sdrInfo = null;
                    try 
                    {
                        clsOperacionCuenta gDAL = new clsOperacionCuenta(); 
                        clsInicioSesionUsuario datosUsuario = new clsInicioSesionUsuario();
                        datosUsuario = clsComun.fnUsuarioEnSesion();
                        sdrInfo = gDAL.fnObtenerDatosFiscales();
                    }
                    catch { }
                
                    clsPlantillaBlancosJessy PdfBlancos = new clsPlantillaBlancosJessy(pxComprobante, sdrInfo);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfBlancos.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfBlancos.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;
                case "CastroyReyesRetanaAbogados":
                    clsPlantillaCastro Pdfcastro = new clsPlantillaCastro(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        Pdfcastro.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    Pdfcastro.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;
                case "Fideapech":
                    //*************Se agrega Adenda a XML***************
                    clsConfiguracionAddenda gADD = new clsConfiguracionAddenda();
                    DataTable addenda = new DataTable();
                    int idEstructura = 0;
                    string AddendaNamespace = string.Empty;


                    idEstructura = gADD.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
                    addenda = gADD.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructura);
                    if (addenda.Rows.Count > 0)
                    {
                        XmlDeclaration xDec = pxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                        pxComprobante.InsertBefore(xDec, pxComprobante.DocumentElement);

                        XmlDocument xAddenda = new XmlDocument();
                        int idModulo = Convert.ToInt32(addenda.Rows[0]["id_modulo"]);
                        xAddenda.LoadXml(Convert.ToString(addenda.Rows[0]["addenda"]));
                        AddendaNamespace = gADD.fnObtieneNameSpace(idModulo);

                        if (AddendaNamespace != "")
                        {
                            string[] nombre = AddendaNamespace.Split('=');
                            XmlAttribute xAttribute = pxComprobante.CreateAttribute(nombre[0]);
                            xAttribute.InnerText = AddendaNamespace;
                            pxComprobante.ChildNodes[1].Attributes.Append(xAttribute);
                        }


                        XmlNode childElement = pxComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobante.DocumentElement.NamespaceURI);
                        pxComprobante.ChildNodes[1].AppendChild(childElement);

                        childElement.InnerXml = xAddenda.OuterXml;

                        clsPlantillaFideapech PdfFideapech = new clsPlantillaFideapech(pxComprobante);

                        if (!string.IsNullOrEmpty(psTipoDocumento))
                            PdfFideapech.TipoDocumento = psTipoDocumento.ToUpper();
                        //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                        clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                        PdfFideapech.fnGenerarPDFSave(sRuta, scolor);
                    }
                    else
                    {
                        clsOperacionConsultaPdf pdf_alt = new clsOperacionConsultaPdf(pxComprobante);
                        if (scolor == string.Empty || scolor == null)
                            scolor = "Black";

                        if (!string.IsNullOrEmpty(psTipoDocumento))
                            pdf_alt.TipoDocumento = psTipoDocumento.ToUpper();
                        pdf_alt.fnGenerarPDFSave(sRuta, scolor);
                    }
                    //******************Fin agregado de adenda********************

                    break;
                case "BalatasyEmbragues":

                    clsPlantillaBalatasyEmbragues PdfBalatas = new clsPlantillaBalatasyEmbragues(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfBalatas.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfBalatas.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "Promotora":

                    clsPlantillaPromotora PdfPromotora = new clsPlantillaPromotora(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfPromotora.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfPromotora.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "PAVILSA":
                    //*************Se agrega Adenda a XML***************
                    clsConfiguracionAddenda gADDPav = new clsConfiguracionAddenda();
                    DataTable addendaPav = new DataTable();
                    int idEstructuraPav = 0;
                    string AddendaNamespacePav = string.Empty;

                    idEstructuraPav = gADDPav.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
                    addendaPav = gADDPav.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructuraPav);
                    if (addendaPav.Rows.Count > 0)
                    {
                        XmlDeclaration xDec = pxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                        pxComprobante.InsertBefore(xDec, pxComprobante.DocumentElement);

                        XmlDocument xAddenda = new XmlDocument();
                        int idModulo = Convert.ToInt32(addendaPav.Rows[0]["id_modulo"]);
                        xAddenda.LoadXml(Convert.ToString(addendaPav.Rows[0]["addenda"]));
                        AddendaNamespacePav = gADDPav.fnObtieneNameSpace(idModulo);

                        if (AddendaNamespacePav != "")
                        {
                            string[] nombre = AddendaNamespacePav.Split('=');
                            XmlAttribute xAttribute = pxComprobante.CreateAttribute(nombre[0]);
                            xAttribute.InnerText = AddendaNamespacePav;
                            pxComprobante.ChildNodes[1].Attributes.Append(xAttribute);
                        }


                        XmlNode childElement = pxComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobante.DocumentElement.NamespaceURI);
                        pxComprobante.ChildNodes[1].AppendChild(childElement);

                        childElement.InnerXml = xAddenda.OuterXml;
                    }

                    clsPlantillaPAVILSA PdfPAVILSA = new clsPlantillaPAVILSA(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfPAVILSA.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfPAVILSA.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                //case "Palliser":

                //    clsPlantillaPalliser PdfPalliser = new clsPlantillaPalliser(pxComprobante);

                //    if (!string.IsNullOrEmpty(psTipoDocumento))
                //        PdfPalliser.TipoDocumento = psTipoDocumento.ToUpper();
                //    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                //    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                //    PdfPalliser.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                //    break;

                //case "GeneraldeSeguros":

                //    clsPlantillaGeneraldeSeguros pdfGeneraldeSeguros = new clsPlantillaGeneraldeSeguros(pxComprobante);

                //    if (!string.IsNullOrEmpty(psTipoDocumento))
                //        pdfGeneraldeSeguros.TipoDocumento = psTipoDocumento.ToUpper();
                //    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                //    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                //    pdfGeneraldeSeguros.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                //    break;

                case "AcerosyEquiposMineros":

                    //*************Se agrega Adenda a XML***************
                    clsConfiguracionAddenda gADDAEM = new clsConfiguracionAddenda();
                    DataTable addendaAEM = new DataTable();
                    int idEstructuraAEM = 0;
                    string AddendaNamespaceAEM = string.Empty;

                    idEstructuraAEM = gADDAEM.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
                    addendaAEM = gADDAEM.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructuraAEM);
                    if (addendaAEM.Rows.Count > 0)
                    {
                        XmlDeclaration xDec = pxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                        pxComprobante.InsertBefore(xDec, pxComprobante.DocumentElement);

                        XmlDocument xAddenda = new XmlDocument();
                        int idModulo = Convert.ToInt32(addendaAEM.Rows[0]["id_modulo"]);
                        xAddenda.LoadXml(Convert.ToString(addendaAEM.Rows[0]["addenda"]));
                        AddendaNamespaceAEM = gADDAEM.fnObtieneNameSpace(idModulo);

                        if (AddendaNamespaceAEM != "")
                        {
                            string[] nombre = AddendaNamespaceAEM.Split('=');
                            XmlAttribute xAttribute = pxComprobante.CreateAttribute(nombre[0]);
                            xAttribute.InnerText = AddendaNamespaceAEM;
                            pxComprobante.ChildNodes[1].Attributes.Append(xAttribute);
                        }


                        XmlNode childElement = pxComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobante.DocumentElement.NamespaceURI);
                        pxComprobante.ChildNodes[1].AppendChild(childElement);

                        childElement.InnerXml = xAddenda.OuterXml;
                    }

                    clsAcerosyEquiposMineros PdfAcerosyEquiposMineros = new clsAcerosyEquiposMineros(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfAcerosyEquiposMineros.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfAcerosyEquiposMineros.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "GCC":

                    clsPlantillaGCC PdfGCC = new clsPlantillaGCC(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        PdfGCC.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    PdfGCC.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "Beatriz":
                    //*************Se agrega Adenda a XML***************
                    clsConfiguracionAddenda gADDB = new clsConfiguracionAddenda();
                    DataTable addendaB = new DataTable();
                    int idEstructuraB = 0;
                    string AddendaNamespaceB = string.Empty;

                    idEstructuraB = gADDB.fnObtieneEstructuraCFD(Convert.ToInt32(psIdCfd));
                    addenda = gADDB.fnObtieneAddendaporIdCfd(Convert.ToInt32(psIdCfd), idEstructuraB);
                    if (addenda.Rows.Count > 0)
                    {
                        XmlDeclaration xDec = pxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                        pxComprobante.InsertBefore(xDec, pxComprobante.DocumentElement);

                        XmlDocument xAddenda = new XmlDocument();
                        int idModulo = Convert.ToInt32(addenda.Rows[0]["id_modulo"]);
                        xAddenda.LoadXml(Convert.ToString(addenda.Rows[0]["addenda"]));
                        AddendaNamespaceB = gADDB.fnObtieneNameSpace(idModulo);

                        if (AddendaNamespaceB != "")
                        {
                            string[] nombre = AddendaNamespaceB.Split('=');
                            XmlAttribute xAttribute = pxComprobante.CreateAttribute(nombre[0]);
                            xAttribute.InnerText = AddendaNamespaceB;
                            pxComprobante.ChildNodes[1].Attributes.Append(xAttribute);
                        }


                        XmlNode childElement = pxComprobante.CreateNode(XmlNodeType.Element, "cfdi:Addenda", pxComprobante.DocumentElement.NamespaceURI);
                        pxComprobante.ChildNodes[1].AppendChild(childElement);

                        childElement.InnerXml = xAddenda.OuterXml;
                    }

                    clsPlantillaBeatriz pdfBeatriz = new clsPlantillaBeatriz(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfBeatriz.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);
                    
                    pdfBeatriz.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;


                case "QuimicaCelta":

                    clsPlantillaQuimicaCelta pdfQuimicaCelta = new clsPlantillaQuimicaCelta(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfQuimicaCelta.TipoDocumento = psTipoDocumento.ToUpper();
                    pdfQuimicaCelta.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);

                    //clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDFSave", "Se envio el PDF para el comprobante con ID " + psIdCfd);

                    break;

                case "AHM":

                    clsPlantillaAHM pdfAHM = new clsPlantillaAHM(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfAHM.TipoDocumento = psTipoDocumento.ToUpper();
                    pdfAHM.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                                     

                    
                    break;

                case "JCConstruccion":

                    clsPlantillaJCConstruccion pdfJC = new clsPlantillaJCConstruccion(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfJC.TipoDocumento = psTipoDocumento.ToUpper();
                    pdfJC.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);

                    //clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDFSave", "Se envio el PDF para el comprobante con ID " + psIdCfd);

                    break;

                case "Masa":
                    clsPlantillaMasa pdfMasa = new clsPlantillaMasa(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfMasa.TipoDocumento = psTipoDocumento.ToUpper();
                    pdfMasa.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);
                    break;

                case "Nomina":

                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(pxComprobante.NameTable);
                    nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
                    nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
                    nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");

                    XPathNavigator nav = pxComprobante.CreateNavigator();

                    string sTipoRiesgo = string.Empty;
                    string sBanco = string.Empty;
                    string sRegimen = string.Empty;

                    string sDescripcionTipoRiesgo = string.Empty;
                    string sDescripcionBanco = string.Empty;
                    string sDescripcionRegimen = string.Empty;

                    try { sTipoRiesgo = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@RiesgoPuesto", nsmComprobante).Value; }
                    catch { }

                    try { sBanco = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@Banco", nsmComprobante).Value; }
                    catch { }

                    try { sRegimen = nav.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@TipoRegimen", nsmComprobante).Value; }
                    catch { }

                    if (!string.IsNullOrEmpty(sTipoRiesgo))
                    {
                        clsOperacionRiesgoPuesto cRiesgoPuesto = new clsOperacionRiesgoPuesto();
                        DataTable dtRiesgoPuesto = cRiesgoPuesto.fnExiste(Convert.ToInt32(sTipoRiesgo));
                        sDescripcionTipoRiesgo = dtRiesgoPuesto.Rows[0]["Descripcion"].ToString();
                    }

                    if (!string.IsNullOrEmpty(sBanco))
                    {
                        clsOperacionBanco cBanco = new clsOperacionBanco();
                        DataTable dtBanco = cBanco.fnExisteBancos(Convert.ToInt32(sBanco));
                        sDescripcionBanco = dtBanco.Rows[0]["NombreCorto"].ToString();
                    }

                    if (!string.IsNullOrEmpty(sRegimen))
                    {
                        clsOperacionTipoRegimen cTipoRegimen = new clsOperacionTipoRegimen();
                        DataTable dtTipoRegimen = cTipoRegimen.fnExiste(Convert.ToInt32(sRegimen));
                        sDescripcionRegimen = dtTipoRegimen.Rows[0]["Descripcion"].ToString();
                    }

                    clsPlantillaNomina pdfNomina = new clsPlantillaNomina(pxComprobante, sDescripcionTipoRiesgo, sDescripcionBanco, sDescripcionRegimen);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfNomina.TipoDocumento = psTipoDocumento.ToUpper();
                    pdfNomina.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta);

                    break;

                default:
                  
                    clsOperacionConsultaPdf pdf = new clsOperacionConsultaPdf(pxComprobante);
                    if (scolor == string.Empty || scolor == null)
                        scolor = "Black";

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdf.TipoDocumento = psTipoDocumento.ToUpper();
                    pdf.fnGenerarPDFSave(sRuta, scolor);

                    //clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDFSave", "Se envio el PDF para el comprobante con ID " + psIdCfd);
                    break;

            }
        }
    }
}
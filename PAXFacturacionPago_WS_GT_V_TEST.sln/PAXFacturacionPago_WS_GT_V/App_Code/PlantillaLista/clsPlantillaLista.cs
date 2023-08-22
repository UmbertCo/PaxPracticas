using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;

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
                pdfLogo.fnGenerarPDF(id_contribuyente,id_rfc, scolor);

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
                clsPlantillaAlbertoGutierrez  PdfAlbertoGtz =  new clsPlantillaAlbertoGutierrez(pxComprobante);

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

                case "GeneraldeSeguros":

                    //clsPlantillaGeneraldeSeguros pdfGeneraldeSeguros = new clsPlantillaGeneraldeSeguros(pxComprobante);

                    //if (!string.IsNullOrEmpty(psTipoDocumento))
                    //    pdfGeneraldeSeguros.TipoDocumento = psTipoDocumento.ToUpper();
                    ////PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

                    ////clsComun.fnNuevaPistaAuditoria("webConsultasGeneradorPDF", "fnGenerarPDF", "Se generó el PDF para el comprobante con ID " + psIdCfd);

                    //pdfGeneraldeSeguros.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta, ref imgCodigo);
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
    public void fnCrearPLantillaEnvioGS(XmlDocument pxComprobante, string sPlantilla, string psIdCfd, string psTipoDocumento, string sRuta, int id_contribuyente, int id_rfc, string scolor)
    {

        if (!(sRuta == string.Empty))
        {
            switch (sPlantilla)
            {

                case "GeneraldeSeguros":

                    clsPlantillaGeneraldeSeguros pdfGeneraldeSeguros = new clsPlantillaGeneraldeSeguros(pxComprobante);

                    if (!string.IsNullOrEmpty(psTipoDocumento))
                        pdfGeneraldeSeguros.TipoDocumento = psTipoDocumento.ToUpper();
                    //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);
                    byte[] imgCodigo = { };
                    pdfGeneraldeSeguros.fnGenerarPDFSave(id_contribuyente, id_rfc, scolor, sRuta, ref imgCodigo);
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

    /// <summary>
    /// Genera codigo bidimensional
    /// </summary>
    /// <param name="imgCodigo"></param>
    public void fnGenerarCodigoBidimensional(XmlDocument pxComprobante, ref byte[] imgCodigo)
    {
        clsPlantillaGeneraldeSeguros pdfGeneraldeSeguros = new clsPlantillaGeneraldeSeguros(pxComprobante);

        //if (!string.IsNullOrEmpty(psTipoDocumento))
        //    pdfGeneraldeSeguros.TipoDocumento = psTipoDocumento.ToUpper();
        //PdfTerCli.fnGenerarPDF(id_contribuyente, id_rfc, scolor);

        pdfGeneraldeSeguros.fnGenerarCodigoBidimensional(ref imgCodigo);
    }
}
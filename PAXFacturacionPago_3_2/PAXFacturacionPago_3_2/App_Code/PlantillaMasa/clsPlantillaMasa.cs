using Root.Reports;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Text;
using System.Web;
using System.Xml.Xsl;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;

/// <summary>
/// Summary description for clsPantillaMasa
/// </summary>
public class clsPlantillaMasa
{
    #region atributos

    private XmlDocument gxComprobante;
    private Report PDF;
    public string TipoDocumento { get; set; }

    private FontDef fuenteTitulo;
    private FontProp fPropTitulo;
    private const double tamFuenteTitulo = 8;

    private FontDef fuenteNormal;
    private FontProp fPropNormal;
    private const double tamFuenteNormal = 6;

    private FontProp fPropExpedidoEn;
    private const double tamFuenteExpedidoEn = 6.8;

    private FontProp fPropChica;
    private const double tamFuenteChica = 5;

    private FontProp fPropBlanca;
    private FontProp fPropRoja;
    private FontProp fPropNegrita;

    //tamaños en mm
    private const double anchoPagina = 215.9;
    private const double altoPagina = 279.4;

    //tamaños en puntos
    private const double altoEncabezado = 60;
    private double altoPie = 70;
    private const double factorSeparador = 2;
    private const double grosorPen = 1;
    private const double radioCurva = 4;

    //Tamaños en puntos
    private const double margenPagina = 20;
    private const double anchoSeccion = anchoPagina - margenPagina * 2;
    private const double tamCodigo = 90;
    private const string leyendaPDF = "Este documento es una representación impresa de un CFDI";

    #endregion

    /// <summary>
    /// Crea una nueva instancia de la clase con un documento XML de CFDI
    /// </summary>
    /// <param name="pxComprobante">Comprobante</param>
    public clsPlantillaMasa(XmlDocument pxComprobante)
	{
        gxComprobante = pxComprobante;
        XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
        gxComprobante.InsertBefore(xDec, gxComprobante.DocumentElement);
        //TipoDocumento = "FACTURA";
	}

    /// <summary>
    /// Genera un nuevo PDF mediante un objeto Report cargado con los datos del XML
    /// </summary>
    public void fnGenerarPDF(int id_contribuyente, int sid_rfc, string scolor)
    {
        Formatter formato = new PdfFormatter();
        PDF = new Report(formato);

        //Datos del documento
        PDF.sTitle = "CFDI";
        PDF.sAuthor = "CORPUS Facturación";

        //Letra titulo
        fuenteTitulo = new FontDef(PDF, FontDef.StandardFont.TimesRoman);
        fPropTitulo = new FontProp(fuenteTitulo, tamFuenteTitulo, Color.Navy);
        fPropTitulo.bBold = true;

        //letra normal
        fuenteNormal = new FontDef(PDF, FontDef.StandardFont.Helvetica);
        fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
        fPropChica = new FontProp(fuenteNormal, tamFuenteChica);

        fPropExpedidoEn = new FontProp(fuenteNormal, tamFuenteExpedidoEn);

        fPropBlanca = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
        fPropBlanca.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);

        fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;

        //Obtenemos los detalles para contar
        List<DetalleMasa> detallesNum = fnObtenerDetalles();

        //Contar paginas
        int nTotPag = 0;
        altoPie = 70;
        while (detallesNum.Count > 0)
        {
            StaticContainer Pie = fnContarCrearPie();

            fnContarPaginas(detallesNum);

            nTotPag += 1;
        }

        //Despues de haber calculado total de páginas se reinicia valor altoPie.
        altoPie = 70;

        //Obtenemos los detalles
        List<DetalleMasa> detalles = fnObtenerDetalles();

        bool bSeguir = true;

        while (bSeguir)
        {
            //Tamaño carta
            Page pagina = new Page(PDF);
            pagina.rWidthMM = anchoPagina;
            pagina.rHeightMM = altoPagina;

            System.Drawing.ColorConverter colConvert = new ColorConverter();
            System.Drawing.Color ColorT = new System.Drawing.Color();

            ColorT = (System.Drawing.Color)colConvert.ConvertFromString(scolor);
            //ColorT = (System.Drawing.Color)colConvert.ConvertFromString("#f4b33d");
           
            StaticContainer Encabezado = fnCrearEncabezado(ColorT);
            altoPie = 92.4;
            StaticContainer Pie = fnCrearPie(pagina.iPageNo, nTotPag, scolor);

            //Sección de observaciones
            //pagina.Add(tamCodigo + margenPagina, margenPagina + 400, fnCrearObservaciones(Pie, ColorT, pagina.iPageNo, nTotPag));

            //Agregamos el encabezado y pie a la nueva página
            pagina.Add(margenPagina, margenPagina, Encabezado);

            //Marco codigo de barras Bidimensional
            fnCrearPanelRedondeado(Pie, 0, 60, tamCodigo, tamCodigo, grosorPen, radioCurva, false, ColorT);

            if (pagina.iPageNo == nTotPag)//Si es ultima pagina
                pagina.Add(margenPagina + 5, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo + 55, GenerarCodigoBidimensional());
            
            
            pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
            pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " de " + nTotPag));

            MemoryStream ms = new MemoryStream();
            ms = fnImagenCliente("logo-masa.jpg");

            //Creamos el área de detalle
            if (ms.Length > 0)
            {
                RepImage image = new RepImage(ms, 120, 45);
                pagina.Add(margenPagina + 225, margenPagina + 43.5, image);
            }


            MemoryStream msLogos = new MemoryStream();
            msLogos = fnImagenCliente("Masa_img_logos.jpg");
            //Creamos el área de detalleismae
            if (msLogos.Length > 0)
            {
                RepImage image = new RepImage(msLogos, 450, 41);
                pagina.Add(margenPagina + 50, margenPagina + 540, image);
            }

            pagina.Add(margenPagina, margenPagina + 755, fnImagenPAX());
            pagina.Add(margenPagina, margenPagina + Encabezado.rHeight+15, fnCrearDetalle(detalles, ColorT));

            //verificamos si aún quedan detalles
            if (detalles.Count <= 0)
                bSeguir = false;
        }
    }

    /// <summary>
    /// Cuenta la pagina
    /// </summary>
    /// <param name="detalles"></param>
    /// <returns></returns>
    private StaticContainer fnContarPaginas(List<DetalleMasa> detalles)
    {
        //StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, (RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2) - 70);
        StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, (RT.rPointFromMM(altoPagina - altoEncabezado - 72) - margenPagina * 2) - 70);

        DetalleMasa[] copiaDetalles = detalles.ToArray();
        DetalleMasa d;
        double posRenglon;
        double altoRenglon = fPropNormal.rSize * factorSeparador;
        int renglonActual = 1;
        //int     contRenActual = 1;
        int rengTotal = 0;
        //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
        int maxRenglones = (int)(areaDetalle.rHeight / (fPropNormal.rSize + 2) - altoRenglon);
        //Definimos el número de conceptos que se agregarán en el for
        int maxConceptos = (maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones);

        if (maxConceptos > 13)
            maxConceptos = 13;

        //definimos las posiciones
        double posColumna1 = areaDetalle.rWidth * 0.01;
        double posColumna2 = areaDetalle.rWidth * 0.114;
        double posColumna3 = areaDetalle.rWidth * 0.28; //0.2;
        int renglones = 0;
        double a = 0;

        //Mediante el for controlamos el numero de renglones para el detalle
        for (int i = 0; renglonActual <= maxRenglones; i++)
        {
            if (maxConceptos <= 0)
                break;

            d = copiaDetalles[i];
            double nAlturaRenglon = fPropNormal.rSize * 1.2;
            //primero verificamos si la descripción cabrá en el espacio restante
            //renglones = d.descripcion.Length / 25;

            if (renglonActual == 1) //Si es nueva hoja se posiciona debajo de encabezado
            {
                posRenglon = (altoRenglon * renglonActual);
            }
            else
            {
                //Si es en la misma hoja se posiciona debajo del anterior concepto
                posRenglon = altoRenglon + (nAlturaRenglon * (renglonActual + a)); //(altoRenglon * renglonActual);    
                a += 1;
            }

            if (d.aduana == null && String.IsNullOrEmpty(d.RfcT))//primero verificamos si la descripción cabrá en el espacio restante
                renglones = fnContarMultilinea(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon, 42, true);
            //else
            //    renglones = fnContarMultilinea(areaDetalle, d.aduana, fPropNormal, posColumna3, posRenglon, 42, true);
            else
            {
                if (!String.IsNullOrEmpty(d.aduana))
                    renglones = fnContarMultilinea(areaDetalle, d.aduana, fPropNormal, posColumna3, posRenglon, 42, true);

                //Complemento concepto terceros
                if (!String.IsNullOrEmpty(d.RfcT))
                    renglones += fnContarMultilinea(areaDetalle, d.RfcT, fPropNormal, posColumna3, posRenglon, 42, true);
            }

            if (!(renglonActual == 1)) //Si es en la misma hoja se verifica si cabe el sig. concepto
            {
                if (renglones > maxRenglones - renglonActual)
                    break;
            }
            else
            {
                if (renglones > maxRenglones - renglonActual) //Si es hoja nueva e verifica si el concepto cabe en su totalidad 
                {
                    rengTotal = Convert.ToInt32((renglones - maxRenglones - renglonActual));
                    rengTotal = renglones - rengTotal;
                }
            }

            int Col1, Col2, Col3;
            Col1 = Col2 = Col3 = 0;

            if (d.aduana != null || !String.IsNullOrEmpty(d.RfcT))
            {
                //Escribimos Iformacion Aduanal del concepto 
                //Col3 = fnContarMultilinea(areaDetalle, "Nombre Aduana: " + d.aduana + "; " + "N° Documento: " + d.NumAduana + "; " + "Fecha: " + Convert.ToDateTime(d.Fecha).ToShortDateString(), fPropNormal, posColumna3, posRenglon, 42, true);

                if (!String.IsNullOrEmpty(d.aduana))
                    Col3 = fnAgregarMultilineaDetalle(areaDetalle, "Nombre Aduana: " + d.aduana + "; " + "N° Documento: " + d.NumAduana + "; " + "Fecha: " + Convert.ToDateTime(d.Fecha).ToShortDateString(), fPropNormal, posColumna3, posRenglon, 42, true);

                //Complemento concepto terceros
                if (!String.IsNullOrEmpty(d.RfcT))
                {
                    areaDetalle.AddAligned(posColumna3, RepObj.rAlignLeft, posRenglon, RepObj.rAlignCenter, new RepString(fPropNegrita, "Información por cuenta de terceros: "));
                    StringBuilder sDetalleTerceros = new StringBuilder();
                    //sDetalleTerceros.Append("Información por cuenta de terceros: \n");
                    sDetalleTerceros.Append("\n\nRFC: " + d.RfcT);                                              //RFC Tercero
                    if (!String.IsNullOrEmpty(d.NombreT)) sDetalleTerceros.Append("\nNombre: " + d.NombreT);  //Nommbre Tercero

                    if (d.UbicacionFiscalT != null && !String.IsNullOrEmpty(d.UbicacionFiscalT.calle))    //Información fiscal tercero
                    {
                        string direccion = string.Empty;
                        direccion += d.UbicacionFiscalT.calle;
                        if (!string.IsNullOrEmpty(d.UbicacionFiscalT.noExterior))
                            direccion += " No. " + d.UbicacionFiscalT.noExterior;
                        if (!string.IsNullOrEmpty(d.UbicacionFiscalT.noInterior))
                            direccion += " Int. " + d.UbicacionFiscalT.noInterior;
                        if (!string.IsNullOrEmpty(d.UbicacionFiscalT.colonia))
                            direccion += " Colonia " + d.UbicacionFiscalT.colonia;

                        string ubicacion = string.Empty;
                        ubicacion += d.UbicacionFiscalT.localidad;
                        if (!string.IsNullOrEmpty(ubicacion))
                        {
                            ubicacion += ", " + d.UbicacionFiscalT.municipio;
                        }
                        else
                        {
                            ubicacion += d.UbicacionFiscalT.municipio;
                        }

                        ubicacion += ", " + d.UbicacionFiscalT.estado;
                        ubicacion += ". " + d.UbicacionFiscalT.pais;
                        ubicacion += " C.P. " + d.UbicacionFiscalT.codigoPostal;

                        //Dirección
                        direccion += "\n" + ubicacion;

                        sDetalleTerceros.Append("\n" + direccion);
                    }

                    if (d.InformacionAduaneraT != null && !String.IsNullOrEmpty(d.InformacionAduaneraT.numero)) //Información aduanera tercero
                    {
                        sDetalleTerceros.Append("\n\nInformación Aduanera Tercero: ");
                        sDetalleTerceros.Append("\nN° Documento: " + d.InformacionAduaneraT.numero);
                        if (!String.IsNullOrEmpty(d.InformacionAduaneraT.aduana)) sDetalleTerceros.Append(", Aduana: " + d.InformacionAduaneraT.aduana);
                        sDetalleTerceros.Append(", Fecha: " + d.InformacionAduaneraT.fecha.ToShortDateString());
                    }

                    if (!String.IsNullOrEmpty(d.NumeroPredialT))
                    {
                        sDetalleTerceros.Append("\n\nCuenta Predial Tercero: ");
                        sDetalleTerceros.Append(d.NumeroPredialT);
                    }

                    if ((d.RetencionesT != null && d.RetencionesT.Count > 0) || (d.TrasladosT != null && d.TrasladosT.Count > 0)) //Impuestos tercero
                    {
                        sDetalleTerceros.Append("\n\nImpuestos por cuenta de terceros: \n");

                        if (d.RetencionesT != null && d.RetencionesT.Count > 0)
                        {
                            foreach (ComprobanteImpuestosRetencionTMasa retencion in d.RetencionesT)
                            {
                                switch (retencion.impuesto)
                                {
                                    case ComprobanteImpuestosRetencionImpuestoTMasa.IVA: sDetalleTerceros.Append("IVA Ret: "); break;
                                    case ComprobanteImpuestosRetencionImpuestoTMasa.ISR: sDetalleTerceros.Append("ISR: "); break;
                                }
                                sDetalleTerceros.Append(TransformacionMasa.fnFormatoCurrency(retencion.importe.ToString()) + " ");
                            }
                        }

                        if (d.TrasladosT != null && d.TrasladosT.Count > 0)
                        {
                            foreach (ComprobanteImpuestosTrasladoTMasa traslado in d.TrasladosT)
                            {
                                switch (traslado.impuesto)
                                {
                                    case ComprobanteImpuestosTrasladoImpuestoTMasa.IVA: sDetalleTerceros.Append("IVA Tras "); break;
                                    case ComprobanteImpuestosTrasladoImpuestoTMasa.IEPS: sDetalleTerceros.Append("IEPS "); break;
                                }
                                sDetalleTerceros.Append(TransformacionMasa.fnFormatoPorcentaje(traslado.tasa.ToString()) + ": ");
                                sDetalleTerceros.Append(TransformacionMasa.fnFormatoCurrency(traslado.importe.ToString()) + " ");
                            }
                        }
                    }

                    Col3 = fnAgregarMultilineaDetalle(areaDetalle, sDetalleTerceros.ToString(), fPropNormal, posColumna3, posRenglon, 42, true);
                }
            }
            else
            {
                Col1 = fnAgregarMultilinea(areaDetalle, d.noIdentificacion, fPropNormal, posColumna1, posRenglon, 9, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.unidad, fPropNormal, posColumna2, posRenglon, 9, true);
                Col3 = fnContarMultilinea(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon, 42, true);
            }

            renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);

            maxConceptos -= 1;
            detalles.Remove(d);
        }

        return areaDetalle;
    }

    private int fnContarMultilinea(StaticContainer pContenedor, string psTexto, FontProp pFuente, double pdPosX, double pdPosY, int pnTamRenglon, bool bBuscarEspacio)
    {
        double nAlturaRenglon = pFuente.rSize * 1.2;
        int nNoRenglon = 0;

        foreach (RepString r in fnDividirRenglonesDetalle(psTexto, pnTamRenglon, pFuente, bBuscarEspacio))
        {
            nNoRenglon++;
        }

        return nNoRenglon;
    }

    public void fnGenerarPDFSave(int id_contribuyente, int sid_rfc, string scolor, string ruta)
    {
        Formatter formato = new PdfFormatter();
        PDF = new Report(formato);

        //Datos del documento
        PDF.sTitle = "CFDI";
        PDF.sAuthor = "CORPUS Facturación";

        //Letra titulo
        fuenteTitulo = new FontDef(PDF, FontDef.StandardFont.TimesRoman);
        fPropTitulo = new FontProp(fuenteTitulo, tamFuenteTitulo, Color.Navy);
        fPropTitulo.bBold = true;

        //letra normal
        fuenteNormal = new FontDef(PDF, FontDef.StandardFont.Helvetica);
        fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
        fPropChica = new FontProp(fuenteNormal, tamFuenteChica);

        fPropExpedidoEn = new FontProp(fuenteNormal, tamFuenteExpedidoEn);

        fPropBlanca = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
        fPropBlanca.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);

        fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;

        //Obtenemos los detalles para contar
        List<DetalleMasa> detallesNum = fnObtenerDetalles();

        //Contar paginas
        int nTotPag = 0;
        altoPie = 70;
        while (detallesNum.Count > 0)
        {
            StaticContainer Pie = fnContarCrearPie();

            fnContarPaginas(detallesNum);

            nTotPag += 1;
        }

        //Despues de haber calculado total de páginas se reinicia valor altoPie.
        altoPie = 70;

        //Obtenemos los detalles
        List<DetalleMasa> detalles = fnObtenerDetalles();

        bool bSeguir = true;

        while (bSeguir)
        {
            //Tamaño carta
            Page pagina = new Page(PDF);
            pagina.rWidthMM = anchoPagina;
            pagina.rHeightMM = altoPagina;

            System.Drawing.ColorConverter colConvert = new ColorConverter();
            System.Drawing.Color ColorT = new System.Drawing.Color();

            ColorT = (System.Drawing.Color)colConvert.ConvertFromString(scolor);
            //ColorT = (System.Drawing.Color)colConvert.ConvertFromString("#f4b33d");

            StaticContainer Encabezado = fnCrearEncabezado(ColorT);
            altoPie = 92.4;
            StaticContainer Pie = fnCrearPie(pagina.iPageNo, nTotPag, scolor);

            //Sección de observaciones
            //pagina.Add(tamCodigo + margenPagina, margenPagina + 400, fnCrearObservaciones(Pie, ColorT, pagina.iPageNo, nTotPag));

            //Agregamos el encabezado y pie a la nueva página
            pagina.Add(margenPagina, margenPagina, Encabezado);

            //Marco codigo de barras Bidimensional
            fnCrearPanelRedondeado(Pie, 0, 60, tamCodigo, tamCodigo, grosorPen, radioCurva, false, ColorT);

            if (pagina.iPageNo == nTotPag)//Si es ultima pagina
                pagina.Add(margenPagina + 5, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo + 55, GenerarCodigoBidimensional());


            pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
            pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " de " + nTotPag));

            MemoryStream ms = new MemoryStream();
            ms = fnImagenCliente("logo-masa.jpg");

            //Creamos el área de detalle
            if (ms.Length > 0)
            {
                RepImage image = new RepImage(ms, 120, 45);
                pagina.Add(margenPagina + 225, margenPagina + 43.5, image);
            }


            MemoryStream msLogos = new MemoryStream();
            msLogos = fnImagenCliente("Masa_img_logos.jpg");
            //Creamos el área de detalleismae
            if (msLogos.Length > 0)
            {
                RepImage image = new RepImage(msLogos, 450, 41);
                pagina.Add(margenPagina + 50, margenPagina + 540, image);
            }

            pagina.Add(margenPagina, margenPagina + 755, fnImagenPAX());
            pagina.Add(margenPagina, margenPagina + Encabezado.rHeight + 15, fnCrearDetalle(detalles, ColorT));

            //verificamos si aún quedan detalles
            if (detalles.Count <= 0)
                bSeguir = false;
        }
        PDF.Save(ruta);
        //return pagina;
    }

    /// <summary>
    /// Despliega en pantalla el PDF
    /// </summary>
    /// <param name="pPagina">Página desde la cual se mostrará el PDF</param>
    public void fnMostrarPDF(System.Web.UI.Page pPagina)
    {
        RT.ResponsePDF(PDF, pPagina);
    }


    //============== DETALLE ========================================
    #region detalle

    /// <summary>
    /// Crea el área de detalles del documento acomodando cierto número de detalles
    /// de la lista proporcionada según sea el alto total del área.
    /// Dichos detalles son consumidos
    /// </summary>
    /// <param name="detalles">Lsta de detalles del comprobante</param>
    /// <returns></returns>
    private StaticContainer fnCrearDetalle(List<DetalleMasa> detalles, System.Drawing.Color sColor)
    {
        //StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, (RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2) - 70);
        double x = 80;
        StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, (RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2)+17.6);

        fnAgregarBordeRedondeado(areaDetalle, grosorPen, radioCurva, sColor);

        DetalleMasa[] copiaDetalles = detalles.ToArray();
        DetalleMasa d;
        double posRenglon;
        double altoRenglon = fPropNormal.rSize * factorSeparador;
        int renglonActual = 1;
        //int     contRenActual = 1;
        int rengTotal = 0;

        //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
        int maxRenglones = (int)((areaDetalle.rHeight / (fPropNormal.rSize + 2)) - altoRenglon);
        //Definimos el número de conceptos que se agregarán en el for
        int maxConceptos = maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones;

        if (maxConceptos > 13)
            maxConceptos = 13;

        //definimos las posiciones
        //double posColumna1 = areaDetalle.rWidth * 0.01;
        //double posColumna2 = areaDetalle.rWidth * 0.114;
        //double posColumna3 = areaDetalle.rWidth * 0.28; //0.2;
        //double posColumna4 = areaDetalle.rWidth * 0.75;
        //double posColumna5 = areaDetalle.rWidth * 0.88;
        //double posColumna6 = areaDetalle.rWidth * 0.99;
        double posColumna1 = areaDetalle.rWidth * 0.080;    //CANTIDAD
        double posColumna2 = areaDetalle.rWidth / 9.5;      //CODIGO 
        double posColumna3 = areaDetalle.rWidth / 4.8;      //U. MEDIDA 
        double posColumna4 = areaDetalle.rWidth * 0.32;     //DESCRIPCION
        double posColumna5 = areaDetalle.rWidth * 0.82;     //P. UNITARIO
        double posColumna6 = areaDetalle.rWidth * 0.97;     //IMPORTE
        int renglones = 0;
        double a = 0;
        //maxRenglones = 50;
        //Mediante el for controlamos el numero de renglones para el detalle
        //for (int i = 0; renglonActual <= maxConceptos; i++)
        for (int i = 0; renglonActual <= maxRenglones; i++)
        {
            if (maxConceptos <= 0)
                break;

            d = copiaDetalles[i];
            double nAlturaRenglon = fPropNormal.rSize * 1.2;
            //primero verificamos si la descripción cabrá en el espacio restante
            //renglones = d.descripcion.Length / 25;

            if (renglonActual == 1) //Si es nueva hoja se posiciona debajo de encabezado
            {
                posRenglon = (altoRenglon * renglonActual);
            }
            else
            {
                //Si es en la misma hoja se posiciona debajo del anterior concepto
                posRenglon = altoRenglon + (nAlturaRenglon * (renglonActual + a)); //(altoRenglon * renglonActual);    
                a += 1;
            }

            if (d.aduana == null && String.IsNullOrEmpty(d.RfcT))//primero verificamos si la descripción cabrá en el espacio restante
                renglones = fnContarMultilinea(areaDetalle, d.descripcion, fPropNormal, posColumna4, posRenglon, 42, true);
            //else
            //    renglones = fnContarMultilinea(areaDetalle, d.aduana, fPropNormal, posColumna3, posRenglon, 42, true);
            else
            {
                if (!String.IsNullOrEmpty(d.aduana))
                    renglones = fnContarMultilinea(areaDetalle, d.aduana, fPropNormal, posColumna4, posRenglon, 42, true);

                //Complemento concepto terceros
                if (!String.IsNullOrEmpty(d.RfcT))
                    renglones += fnContarMultilinea(areaDetalle, d.RfcT, fPropNormal, posColumna4, posRenglon, 42, true);
            }

            if (!(renglonActual == 1)) //Si es en la misma hoja se verifica si cabe el sig. concepto
            {
                if (renglones > maxRenglones - renglonActual)
                    break;
            }
            else
            {
                if (renglones > maxRenglones - renglonActual) //Si es hoja nueva e verifica si el concepto cabe en su totalidad 
                {
                    rengTotal = Convert.ToInt32((renglones - maxRenglones - renglonActual));
                    rengTotal = renglones - rengTotal;
                }
            }
            //d.descripcion += char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10);

            //Primero los datos fijos del primer renglon del detalle
            //areaDetalle.Add(posColumna1, posRenglon, new RepString(fPropNormal, d.noIdentificacion));
            //areaDetalle.Add(posColumna2, posRenglon, new RepString(fPropNormal, d.unidad));

            int Col1, Col2, Col3;
            Col1 = Col2 = Col3 = 0;

            if (d.aduana != null || !String.IsNullOrEmpty(d.RfcT))
            {
                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, "Nombre Aduana: " + d.aduana + "; " + "N° Documento: " + d.NumAduana + "; " + "Fecha: " + Convert.ToDateTime(d.Fecha).ToShortDateString(), fPropNormal, posColumna3, posRenglon, 42, true);
                //areaDetalle.AddAligned(posColumna4, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal,"N° " +d.NumAduana));
                //areaDetalle.AddAligned(posColumna5, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal,"Fecha " +Convert.ToDateTime(d.Fecha).ToShortDateString()));
                if (!String.IsNullOrEmpty(d.aduana))
                    Col3 = fnAgregarMultilineaDetalle(areaDetalle, "Nombre Aduana: " + d.aduana + "; " + "N° Documento: " + d.NumAduana + "; " + "Fecha: " + Convert.ToDateTime(d.Fecha).ToShortDateString(), fPropNormal, posColumna4, posRenglon, 42, true);

                //Complemento concepto terceros
                if (!String.IsNullOrEmpty(d.RfcT))
                {
                    areaDetalle.AddAligned(posColumna4, RepObj.rAlignLeft, posRenglon, RepObj.rAlignCenter, new RepString(fPropNegrita, "Información por cuenta de terceros: "));
                    StringBuilder sDetalleTerceros = new StringBuilder();
                    //sDetalleTerceros.Append("Información por cuenta de terceros: \n");
                    sDetalleTerceros.Append("\n\nRFC: " + d.RfcT);                                              //RFC Tercero
                    if (!String.IsNullOrEmpty(d.NombreT)) sDetalleTerceros.Append("\nNombre: " + d.NombreT);  //Nommbre Tercero

                    if (d.UbicacionFiscalT != null && !String.IsNullOrEmpty(d.UbicacionFiscalT.calle))    //Información fiscal tercero
                    {
                        string direccion = string.Empty;
                        direccion += d.UbicacionFiscalT.calle;
                        if (!string.IsNullOrEmpty(d.UbicacionFiscalT.noExterior))
                            direccion += " No. " + d.UbicacionFiscalT.noExterior;
                        if (!string.IsNullOrEmpty(d.UbicacionFiscalT.noInterior))
                            direccion += " Int. " + d.UbicacionFiscalT.noInterior;
                        if (!string.IsNullOrEmpty(d.UbicacionFiscalT.colonia))
                            direccion += " Colonia " + d.UbicacionFiscalT.colonia;

                        string ubicacion = string.Empty;
                        ubicacion += d.UbicacionFiscalT.localidad;
                        if (!string.IsNullOrEmpty(ubicacion))
                        {
                            ubicacion += ", " + d.UbicacionFiscalT.municipio;
                        }
                        else
                        {
                            ubicacion += d.UbicacionFiscalT.municipio;
                        }

                        ubicacion += ", " + d.UbicacionFiscalT.estado;
                        ubicacion += ". " + d.UbicacionFiscalT.pais;
                        ubicacion += " C.P. " + d.UbicacionFiscalT.codigoPostal;

                        //Dirección
                        direccion += "\n" + ubicacion;

                        sDetalleTerceros.Append("\n" + direccion);
                    }

                    if (d.InformacionAduaneraT != null && !String.IsNullOrEmpty(d.InformacionAduaneraT.numero)) //Información aduanera tercero
                    {
                        sDetalleTerceros.Append("\n\nInformación Aduanera Tercero: ");
                        sDetalleTerceros.Append("\nN° Documento: " + d.InformacionAduaneraT.numero);
                        if (!String.IsNullOrEmpty(d.InformacionAduaneraT.aduana)) sDetalleTerceros.Append(", Aduana: " + d.InformacionAduaneraT.aduana);
                        sDetalleTerceros.Append(", Fecha: " + d.InformacionAduaneraT.fecha.ToShortDateString());
                    }

                    if (!String.IsNullOrEmpty(d.NumeroPredialT))
                    {
                        sDetalleTerceros.Append("\n\nCuenta Predial Tercero: ");
                        sDetalleTerceros.Append(d.NumeroPredialT);
                    }

                    if ((d.RetencionesT != null && d.RetencionesT.Count > 0) || (d.TrasladosT != null && d.TrasladosT.Count > 0)) //Impuestos tercero
                    {
                        sDetalleTerceros.Append("\n\nImpuestos por cuenta de terceros: \n");

                        if (d.RetencionesT != null && d.RetencionesT.Count > 0)
                        {
                            foreach (ComprobanteImpuestosRetencionTMasa retencion in d.RetencionesT)
                            {
                                switch (retencion.impuesto)
                                {
                                    case ComprobanteImpuestosRetencionImpuestoTMasa.IVA: sDetalleTerceros.Append("IVA Ret: "); break;
                                    case ComprobanteImpuestosRetencionImpuestoTMasa.ISR: sDetalleTerceros.Append("ISR: "); break;
                                }
                                sDetalleTerceros.Append(TransformacionMasa.fnFormatoCurrency(retencion.importe.ToString()) + " ");
                            }
                        }

                        if (d.TrasladosT != null && d.TrasladosT.Count > 0)
                        {
                            foreach (ComprobanteImpuestosTrasladoTMasa traslado in d.TrasladosT)
                            {
                                switch (traslado.impuesto)
                                {
                                    case ComprobanteImpuestosTrasladoImpuestoTMasa.IVA: sDetalleTerceros.Append("IVA Tras "); break;
                                    case ComprobanteImpuestosTrasladoImpuestoTMasa.IEPS: sDetalleTerceros.Append("IEPS "); break;
                                }
                                sDetalleTerceros.Append(TransformacionMasa.fnFormatoPorcentaje(traslado.tasa.ToString()) + ": ");
                                sDetalleTerceros.Append(TransformacionMasa.fnFormatoCurrency(traslado.importe.ToString()) + " ");
                            }
                        }
                    }

                    Col3 = fnAgregarMultilineaDetalle(areaDetalle, sDetalleTerceros.ToString(), fPropNormal, posColumna4, posRenglon, 42, true);
                }

            }
            else
            {
                //Col1 = fnAgregarMultilinea(areaDetalle, d.noIdentificacion, fPropNormal, posColumna1, posRenglon, 9, true);
                //Col2 = fnAgregarMultilinea(areaDetalle, d.unidad, fPropNormal, posColumna2, posRenglon, 9, true);
                //areaDetalle.AddAligned(posColumna4, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.cantidad));
                //areaDetalle.AddAligned(posColumna5, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.valorUnitario));
                //areaDetalle.AddAligned(posColumna6, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.importe));


                Col1 = fnAgregarMultilinea(areaDetalle, d.noIdentificacion, fPropNormal, posColumna2, posRenglon, 9, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.unidad, fPropNormal, posColumna3, posRenglon, 9, true);
                areaDetalle.AddAligned(posColumna1, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.cantidad));
                areaDetalle.AddAligned(posColumna5, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.valorUnitario));
                areaDetalle.AddAligned(posColumna6, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.importe));

                //Acomodamos la descripción en tantos renglones como necesite           
                Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropNormal, posColumna4, posRenglon, 42, true);
                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);

            }

            renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
            //renglonActual = renglonActual+1;
            maxConceptos -= 1;
            detalles.Remove(d);
        }

        return areaDetalle;
    }

    /// <summary>
    /// Obtiene el número de renglon mayor
    /// </summary>
    /// <param name="Col1"></param>
    /// <param name="Col2"></param>
    /// <param name="Col3"></param>
    /// <returns></returns>
    private int fnObtenerRenglonMayor(int Col1, int Col2, int Col3)
    {
        int nRenMayor = 0;
        if (Col1 >= Col2 && Col1 >= Col3)
        {
            nRenMayor = Col1;
        }
        else
        {
            if (Col2 >= Col1 && Col2 >= Col3)
            {
                nRenMayor = Col2;
            }
            else
            {
                if (Col3 >= Col1 && Col3 >= Col2)
                {
                    nRenMayor = Col3;
                }
            }
        }

        return nRenMayor;
    }

    /// <summary>
    /// Crea la lista de detalles a partir de los conceptos contenidos en el comprobante XML
    /// </summary>
    /// <returns>Lista de objetos Detalle</returns>
    private List<DetalleMasa> fnObtenerDetalles()
    {
        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

        List<DetalleMasa> detalles = new List<DetalleMasa>();

        XPathNavigator navComprobante = gxComprobante.CreateNavigator();
        XPathNodeIterator navDetalles = navComprobante.Select("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsmComprobante);

        while (navDetalles.MoveNext())
        {
            detalles.Add(new DetalleMasa(navDetalles.Current, nsmComprobante));
            XPathNavigator nodenavigator = navDetalles.Current;
            if (nodenavigator.HasChildren)//Si contiene nodo hijo
            {
                ////Desplaza al primero nodo hijo
                //nodenavigator.MoveToFirstChild();
                ////Obtiene la informacion del nodo
                //XPathNodeIterator navDetallesAduana = nodenavigator.Select("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto/cfdi:InformacionAduanera", nsmComprobante);
                //if (navDetallesAduana.Count > 0)//Si contiene Informacion Aduanal se agrega al concepto
                //    detalles.Add(new DetalleMasa(navDetallesAduana.Current, nsmComprobante));
                //Obtiene la informacion del nodo aduana si existe
                XPathNavigator navDetallesAduana = nodenavigator.SelectSingleNode("cfdi:InformacionAduanera", nsmComprobante);
                if (navDetallesAduana != null)
                    detalles.Add(new DetalleMasa(navDetallesAduana, nsmComprobante));

                //Obtiene información del nodo complemento concepto si existe
                XPathNavigator navComplTerceros = nodenavigator.SelectSingleNode("cfdi:ComplementoConcepto", nsmComprobante);
                if (navComplTerceros != null)
                {
                    XmlNamespaceManager nsmComprobanteComplTerceros = new XmlNamespaceManager(gxComprobante.NameTable);
                    nsmComprobanteComplTerceros.AddNamespace("terceros", "http://www.sat.gob.mx/terceros");

                    //Obtiene información del nodo cuenta de terceros si existe
                    XPathNavigator navTerceros = navComplTerceros.SelectSingleNode("terceros:PorCuentadeTerceros", nsmComprobanteComplTerceros);

                    if (navTerceros != null)
                        detalles.Add(new DetalleMasa(navTerceros, nsmComprobanteComplTerceros));
                }
            }
        }

        return detalles;
    }

    #endregion
    //===============================================================

    //========= PIE =================================================
    #region pie

    /// <summary>
    /// Se calcula el total de páginas
    /// </summary>
    /// <returns>StaticContainer con la información del pie de página</returns>
    private StaticContainer fnContarCrearPie()
    {
        StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));

        fnContarTotales(Pie);

        return Pie;
    }

    /// <summary>
    /// Calcula total de páginas
    /// </summary>
    /// <param name="nsmComprobante">Manejador de nombre de espacios</param>
    /// <param name="navPie">Navegador con los datos del pie de página</param>
    /// <param name="Pie">Contenedor en el cual se desplegarán los datos del pie de página</param>
    private void fnContarTotales(StaticContainer Pie)
    {
        int renglon = 1;

        //Pie.rHeight += renglon * (fPropChica.rSize * 1.2);
        //altoPie = Pie.rHeightMM;

    }

    /// <summary>
    /// Crea el pie de página del documento
    /// </summary>
    /// <returns>StaticContainer con la información del pie de página</returns>
    private StaticContainer fnCrearPie(int nNumPag, int nTotPag, string sColor)
    {
        //StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));
        StaticContainer Pie;
        if (nTotPag == nNumPag)
            Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));
        else
            Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie + 2));
        

        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");

        XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

        fnTotales(nsmComprobante, navEncabezado, Pie, nNumPag, nTotPag, sColor);

        return Pie;
    }

    /// <summary>
    /// Crea un nuevo panel con la información del pie de página del comprobante
    /// </summary>
    /// <param name="nsmComprobante">Manejador de nombre de espacios</param>
    /// <param name="navPie">Navegador con los datos del pie de página</param>
    /// <param name="Pie">Contenedor en el cual se desplegarán los datos del pie de página</param>
    private void fnTotales(XmlNamespaceManager nsmComprobante, XPathNavigator navPie, StaticContainer Pie, int nNumPag, int nTotPag, string sColor)
    {
        System.Drawing.ColorConverter colConvert = new ColorConverter();
        System.Drawing.Color ColorT = new System.Drawing.Color();

        ColorT = (System.Drawing.Color)colConvert.ConvertFromString(sColor);

        string subtotal, total, moneda, sello, timbre, formaDePago, metodoPago, Regimenfiscal, version, sNumCtaPago, descuento;
        subtotal = total = moneda = sello = timbre = formaDePago = metodoPago = Regimenfiscal = version = sNumCtaPago = descuento = string.Empty;

        List<ImpuestoMasa> impuestos = new List<ImpuestoMasa>();
        List<ImpuestoCompMasa> impuestosComp = new List<ImpuestoCompMasa>();

        subtotal = navPie.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsmComprobante).Value;
        total = navPie.SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value;
        timbre = navPie.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value;
        formaDePago = navPie.SelectSingleNode("/cfdi:Comprobante/@formaDePago", nsmComprobante).Value;
        sello = navPie.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value;
        version = navPie.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
        try { moneda = navPie.SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante).Value; }
        catch { }

        try { metodoPago = navPie.SelectSingleNode("/cfdi:Comprobante/@metodoDePago", nsmComprobante).Value; }
        catch { }

        try { descuento = navPie.SelectSingleNode("/cfdi:Comprobante/@descuento", nsmComprobante).Value; }
        catch { }

        try { sNumCtaPago = navPie.SelectSingleNode("/cfdi:Comprobante/@NumCtaPago", nsmComprobante).Value; }
        catch { }

        //try { Regimenfiscal = navPie.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:RegimenFiscal/@Regimen", nsmComprobante).Value; }
        //catch { }

        XPathNodeIterator navImpuestos = navPie.Select("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado", nsmComprobante);
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            while (navImpuestos.MoveNext())
            {
                impuestos.Add(new ImpuestoMasa(navImpuestos.Current, nsmComprobante));
            }

            navImpuestos = navPie.Select("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion", nsmComprobante);
            while (navImpuestos.MoveNext())
            {
                impuestos.Add(new ImpuestoMasa(navImpuestos.Current, nsmComprobante));
            }
        }

        //Complementos impuestos locales
        try
        {
            XPathNodeIterator navComplemento = navPie.Select("/cfdi:Comprobante/cfdi:Complemento/implocal:ImpuestosLocales/implocal:TrasladosLocales", nsmComprobante);

            while (navComplemento.MoveNext())
            {
                impuestosComp.Add(new ImpuestoCompMasa(navComplemento.Current, nsmComprobante));
            }

            navComplemento = navPie.Select("/cfdi:Comprobante/cfdi:Complemento/implocal:ImpuestosLocales/implocal:RetencionesLocales", nsmComprobante);

            while (navComplemento.MoveNext())
            {
                impuestosComp.Add(new ImpuestoCompMasa(navComplemento.Current, nsmComprobante));
            }
        }
        catch { }

        double verPadding = Pie.rHeight * 0.02;
        double horPadding = Pie.rWidth * 0.01;
        double posPanelTotales = Pie.rWidth - 160;
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            Pie.AddAligned(horPadding + posPanelTotales+40, RepObj.rAlignLeft, verPadding+80, RepObj.rAlignTop, new RepString(fPropNormal, "SUBTOTAL"));
            Pie.AddAligned(Pie.rWidth - horPadding-10, RepObj.rAlignRight, verPadding+80, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionMasa.fnFormatoCurrency(subtotal)));
        }
        double altoRenglon = fPropNormal.rSize + verPadding;
        int renglon = 1;
        string textoRenglon = string.Empty;
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            if (descuento != string.Empty)
            {
                Pie.AddAligned(horPadding + posPanelTotales + 40, RepObj.rAlignLeft, verPadding + altoRenglon * renglon+82, RepObj.rAlignTop, new RepString(fPropNormal, "DESCUENTO"));
                Pie.AddAligned(Pie.rWidth - horPadding-10, RepObj.rAlignRight, verPadding + altoRenglon * renglon+82, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionMasa.fnFormatoCurrency(descuento)));
                renglon++;
            }
            foreach (ImpuestoMasa i in impuestos)
            {
                //Solamente IVA como impuesto, sin ISR, impuestos locales ni IVA de retención
                if (i.Nombre.Equals("IVA") && !i.Tasa.Equals("Retención"))
                {
                    Pie.AddAligned(horPadding + posPanelTotales+40, RepObj.rAlignLeft, verPadding + altoRenglon * renglon+84, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + TransformacionMasa.fnFormatoPorcentaje(i.Tasa)));
                    Pie.AddAligned(Pie.rWidth - horPadding-10, RepObj.rAlignRight, verPadding + altoRenglon * renglon+84, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionMasa.fnFormatoCurrency(i.Importe)));
                    renglon++;
                }
            }
            //No se van a utilizar impuestos locales para esta plantilla
            //try
            //{
            //    //Agrega complemento impuestos locales 
            //    foreach (ImpuestoCompMasa i in impuestosComp)
            //    {
            //        Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + TransformacionMasa.fnFormatoPorcentaje(i.Tasa)));
            //        Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionMasa.fnFormatoCurrency(i.Importe)));
            //        renglon++;
            //    }
            //}
            //catch { }


            //Agregamos el separador y el total
            Pie.Add(posPanelTotales + horPadding+20, tamCodigo - altoRenglon - verPadding+50, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - (horPadding * 2)-20, 0));

            Pie.AddAligned(horPadding + posPanelTotales+40, RepObj.rAlignLeft, tamCodigo - altoRenglon+50, RepObj.rAlignTop, new RepString(fPropNormal, "TOTAL"));
            Pie.AddAligned(Pie.rWidth - horPadding-10, RepObj.rAlignRight, tamCodigo - altoRenglon+50, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionMasa.fnFormatoCurrency(total)));
        }
        //Finalmente creamos el panel de los totales

        //cuadro a la derecha con subtotal iva y total a la altura del QR
        fnCrearPanelRedondeado(Pie, posPanelTotales, 60, Pie.rWidth - posPanelTotales, tamCodigo, grosorPen, radioCurva, false, ColorT);
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            //Mostramos forma de pago y texto importe en su panel correspondiente 
            Pie.Add(horPadding + tamCodigo, altoRenglon+62, new RepString(fPropNegrita, "Total con letra:"));
            fnAgregarMultilinea(Pie, fnTextoImporte(TransformacionMasa.fnFormatoRedondeo(total), moneda), fPropChica, horPadding + tamCodigo, altoRenglon * 1.7+62, 115, true);

            fnAgregarMultilinea(Pie, "Forma de Pago:", fPropNegrita, horPadding + tamCodigo, altoRenglon * 3+62, 38, true);
            fnAgregarMultilinea(Pie, formaDePago, fPropChica, horPadding + tamCodigo, altoRenglon * 4+62, 38, true);

            if (version == "3.2") //Si es version 3.2 muestra el siguiente contenido
            {
                //Metodo de pago 
                if (!string.IsNullOrEmpty(metodoPago))
                {
                    fnAgregarMultilinea(Pie, "Método de Pago:", fPropNegrita, horPadding + tamCodigo + 140, altoRenglon * 3+62, 38, true);
                    fnAgregarMultilinea(Pie, metodoPago, fPropChica, horPadding + tamCodigo + 140, altoRenglon * 4+62, 80, true);

                    //Número de cuenta
                    if (!string.IsNullOrEmpty(sNumCtaPago))
                    {
                        fnAgregarMultilinea(Pie, "Número de Cuenta:", fPropNegrita, horPadding + tamCodigo, altoRenglon * 5+64, 38, true);
                        fnAgregarMultilinea(Pie, sNumCtaPago, fPropChica, horPadding + tamCodigo , altoRenglon * 6+64, 80, true);
                    }
                }
                else
                {
                    //Número de cuenta
                    if (!string.IsNullOrEmpty(sNumCtaPago))
                    {
                        fnAgregarMultilinea(Pie, "Número de Cuenta:", fPropNegrita, horPadding + tamCodigo, altoRenglon * 5 + 64, 38, true);
                        fnAgregarMultilinea(Pie, sNumCtaPago, fPropChica, horPadding + tamCodigo, altoRenglon * 6 + 64, 80, true);
                    }
                }
            }
        }
        //
        //fnCrearPanelRedondeado(Pie, 0, 0, posPanelTotales, 60, grosorPen, radioCurva, false, ColorT);


        //fnCrearPanelRedondeado(Pie, tamCodigo, tamCodigo, Pie.rWidth - tamCodigo, tamCodigo, grosorPen, radioCurva, false, sColor);
        //fnCrearPanelRedondeado(areaObservaciones, 0, 78, areaObservaciones.rWidth, tamCodigo, grosorPen, radioCurva, false, sColor);
        //Panel de Observaciones
        //Pie.Add(tamCodigo + 12, 80, new RepString(fPropNegrita, "Observaciones:"));
        //totales en medio
        fnCrearPanelRedondeado(Pie, tamCodigo, 60, Pie.rWidth - tamCodigo-160, tamCodigo, grosorPen, radioCurva, false, ColorT);

        //Estos datos estan debajo del CBB
        renglon = 1;
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            Pie.Add(horPadding, tamCodigo + 60 + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del Emisor:"));
            renglon += fnAgregarMultilinea(Pie, sello, fPropChica, horPadding, tamCodigo + 60 + altoRenglon * renglon, 130, false);

            Pie.Add(horPadding, tamCodigo + 60 + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del SAT:"));
            renglon += fnAgregarMultilinea(Pie, timbre, fPropChica, horPadding, tamCodigo + 60 + altoRenglon * renglon, 130, false);


            //Agregamos la cadena original y alargamos la sección del pie según sea necesario
            Pie.Add(horPadding, tamCodigo + 60 + altoRenglon * renglon++, new RepString(fPropNormal, "Cadena original del complemento de certificación digital del SAT:"));
            renglon = fnAgregarMultilinea(Pie, TransformacionMasa.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0")
                , fPropChica, horPadding, tamCodigo + 60 + altoRenglon * renglon, 130, false);

            renglon = 1;
            Pie.rHeight += renglon * (fPropChica.rSize * 1.2);
            //altoPie = Pie.rHeightMM;
        }
        //dibujamos el borde del pie
        fnCrearPanelRedondeado(Pie, 0, tamCodigo + 60, Pie.rWidth, Pie.rHeight - (tamCodigo * 2) - altoRenglon + 10 + 16, grosorPen, radioCurva, false, ColorT);

        //Agrega www.paxfacturacion.com
        fPropNormal = new FontProp(fuenteNormal, 4);
        Pie.AddAligned(Pie.rWidth / 7, RepObj.rAlignCenter, Pie.rHeight - verPadding, RepObj.rAlignTop, new RepString(fPropNormal, clsComunMasa.ObtenerParamentro("urlHostComercial")));

        //Agrega leyenda de comprobante
        fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
        Pie.AddAligned(Pie.rWidth / 2, RepObj.rAlignCenter, Pie.rHeight - verPadding, RepObj.rAlignTop, new RepString(fPropNormal, leyendaPDF));
    }

    #endregion
    //===============================================================

    //====== ENCABEZADO =============================================
    #region encabezado

    /// <summary>
    /// Crea el área de encabezado del documento
    /// </summary>
    /// <returns>StaticContainer con la información del encabezado del comprobante</returns>
    private StaticContainer fnCrearEncabezado(System.Drawing.Color sColor)
    {
        StaticContainer Encabezado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoEncabezado)+20);
        //fnCrearPanelRedondeado(Encabezado, 0, 0, Encabezado.rWidth, Encabezado.rHeight - fPropBlanca.rSize * 2, grosorPen, radioCurva, false, sColor);

        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        try { nsmComprobante.AddNamespace("donat", "http://www.sat.gob.mx/donat"); }
        catch { }

        XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
        double Renglon = 0;
        fnDatosEmisor(nsmComprobante, navEncabezado, Encabezado, sColor, ref Renglon);
        fnDatosReceptor(nsmComprobante, navEncabezado, Encabezado, Renglon, sColor);
        fnDibujarTitulosDetalle(Encabezado, sColor);

        return Encabezado;
    }

    /// <summary>
    /// Dibuja el recuadro de titulos para el área de detalle
    /// </summary>
    /// <param name="Encabezado">Contenedor al que se agregarán los titulos</param>
    private void fnDibujarTitulosDetalle(StaticContainer Encabezado, System.Drawing.Color sColor)
    {
        System.Drawing.ColorConverter colConvert = new ColorConverter();
        System.Drawing.Color ColorT = new System.Drawing.Color();

        ColorT = (System.Drawing.Color)colConvert.ConvertFromString("#f4b33d");

        //Dibujamos el área de los titulos
        double altoBarra = fPropBlanca.rSize * 2;
        fnCrearPanelRedondeado(Encabezado, 0, Encabezado.rHeight - altoBarra+15.3, Encabezado.rWidth, altoBarra - 2, grosorPen, 1, true, ColorT);

        //Dibujamos los titulos del detalle
        //El ancho total del área es de 572 puntos
        double puntoMedio = Encabezado.rHeight - fPropBlanca.rSize+15;

        //Definimos la posicion de los titulos
        //Definimos la posicion de los titulos
        double posColumna1 = Encabezado.rWidth * 0.02;     //CANTIDAD
        double posColumna2 = Encabezado.rWidth / 9.5;      //CODIGO 
        double posColumna3 = Encabezado.rWidth / 4.8;      //U. MEDIDA 
        double posColumna4 = Encabezado.rWidth / 2.4;      //DESCRIPCION
        double posColumna5 = Encabezado.rWidth * 0.75;     //P. UNITARIO  
        double posColumna6 = Encabezado.rWidth * 0.90;     //IMPORTE

        Encabezado.AddAligned(posColumna1, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "CANTIDAD"));
        Encabezado.AddAligned(posColumna2, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "CODIGO"));
        Encabezado.AddAligned(posColumna3, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "U. MEDIDA"));
        Encabezado.AddAligned(posColumna4, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "DESCRIPCION"));
        Encabezado.AddAligned(posColumna5, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "P. UNITARIO"));
        Encabezado.AddAligned(posColumna6, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "IMPORTE"));
    }

    /// <summary>
    /// Agrega los datos del emisor al área de encabezado
    /// </summary>
    /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
    /// <param name="navEncabezado">Navegador con los datos del comprobante XML</param>
    /// <param name="Encabezado">Contenedor donde se mostrsrán los datos</param>
    private void fnDatosEmisor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, System.Drawing.Color sColor, ref double Renglon)
    {
        string razonSocial, rfc, calle, noExterior, noInterior, colonia, municipio, estado, pais, codigoPostal, serie, folio, fecha, noCertificadoEmisor,
            referencia, Localidad, version, Regimenfiscal, fechaTimb, estadolug, noAutorizacion, fechaAutorizacion, leyenda, exCalle, exNoInterior, exNoExterior,
            exColonia, exMunicipio, exEstado, exPais, exCodigoPostal, exLocalidad;
        razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = fecha = serie = folio = noCertificadoEmisor =
            referencia = Localidad = version = Regimenfiscal = fechaTimb = estadolug = noAutorizacion = fechaAutorizacion = leyenda =
            exCalle = exNoInterior = exNoExterior = exColonia = exMunicipio = exEstado = exPais = exCodigoPostal = exLocalidad = string.Empty;

        try { serie = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value; }
        catch { }
        try { folio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
        catch { }
        try { fecha = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value; }
        catch { }
        try { fechaTimb = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value; }
        catch { }

        DateTime fechaComprobante = DateTime.MinValue;
        DateTime fechaTimbrado = DateTime.MinValue;
        if (!string.IsNullOrEmpty(fecha))
            fechaComprobante = Convert.ToDateTime(fecha);

        if (!string.IsNullOrEmpty(fechaTimb))
            fechaTimbrado = Convert.ToDateTime(fechaTimb);

        razonSocial = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value;
        rfc = "RFC: " + navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
        calle = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@calle", nsmComprobante).Value;
        try { noExterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@noExterior", nsmComprobante).Value; }
        catch { }
        try { noInterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@noInterior", nsmComprobante).Value; }
        catch { }
        try { colonia = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@colonia", nsmComprobante).Value; }
        catch { }

        try { referencia = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@referencia", nsmComprobante).Value; }
        catch { }

        try { Localidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@localidad", nsmComprobante).Value; }
        catch { }

        version = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;

        try { Regimenfiscal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:RegimenFiscal/@Regimen", nsmComprobante).Value; }
        catch { }

        municipio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@municipio", nsmComprobante).Value;

        pais = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@pais", nsmComprobante).Value;
        estado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@estado", nsmComprobante).Value;
        if (version == "3.0")
            estadolug = pais + ", " + estado;
        else
        {
            try { estadolug = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", nsmComprobante).Value; }
            catch { }
        }
        codigoPostal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@codigoPostal", nsmComprobante).Value;
        noCertificadoEmisor = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value;

        //Si existe complemento donativas se agrega a PDf
        try { noAutorizacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/donat:Donatarias/@noAutorizacion", nsmComprobante).Value; }
        catch { }

        try { fechaAutorizacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/donat:Donatarias/@fechaAutorizacion", nsmComprobante).Value; }
        catch { }

        try { leyenda = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/donat:Donatarias/@leyenda", nsmComprobante).Value; }
        catch { }

        //------Expedido En--------
        try { exCalle = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@calle", nsmComprobante).Value; }
        catch { }

        if (exCalle != string.Empty)
        {

            try { exNoExterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@noExterior", nsmComprobante).Value; }
            catch { }

            try { exNoInterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@noInterior", nsmComprobante).Value; }
            catch { }

            try { exColonia = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@colonia", nsmComprobante).Value; }
            catch { }

            try { exMunicipio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@municipio", nsmComprobante).Value; }
            catch { }

            try { exEstado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@estado", nsmComprobante).Value; }
            catch { }

            try { exPais = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@pais", nsmComprobante).Value; }
            catch { }

            try { exCodigoPostal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@codigoPostal", nsmComprobante).Value; }
            catch { }

            try { exLocalidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@localidad", nsmComprobante).Value; }
            catch { }
        }


        string direccion = string.Empty;
        string coloniaemisor = string.Empty;

        direccion += calle;
        if (!string.IsNullOrEmpty(noExterior))
            direccion += " No. " + noExterior;
        if (!string.IsNullOrEmpty(noInterior))
            direccion += " Int. " + noInterior;
        if (!string.IsNullOrEmpty(colonia))
            direccion += " Colonia " + colonia; //coloniaemisor += "Colonia " + colonia;

        string ubicacion = string.Empty;
        ubicacion += Localidad;
        if (!string.IsNullOrEmpty(ubicacion))
        {
            ubicacion += ", " + municipio;
        }
        else
        {
            ubicacion += municipio;
        }


        //ubicacion += ", "; //+ estado;
        //ubicacion += ". " + pais;
        //ubicacion += " C.P. " + codigoPostal;
        string ubicacion2 = string.Empty;
        ubicacion2 += estado;
        ubicacion2 += ". " + pais;
        ubicacion2 += " C.P. " + codigoPostal;

        //Direccion Expedido En 
        string exDireccion = string.Empty;
        exDireccion += exCalle;
        if (!string.IsNullOrEmpty(exNoExterior))
            exDireccion += " No. " + exNoExterior;
        if (!string.IsNullOrEmpty(exNoInterior))
            exDireccion += " Int. " + exNoInterior;
        if (!string.IsNullOrEmpty(exColonia))
            exDireccion += " Colonia " + exColonia;

        string exUbicacion = string.Empty;
        exUbicacion += exLocalidad;
        if (!string.IsNullOrEmpty(exUbicacion))
        {
            exUbicacion += ", " + exMunicipio;
        }
        else
        {
            exUbicacion += exMunicipio;
        }

        //exUbicacion += ", ";//+ exEstado;
        //exUbicacion += ". " + exPais;
        //exUbicacion += " C.P. " + exCodigoPostal;

        string exUbicacion2 = string.Empty;
        exUbicacion2 += exEstado;
        exUbicacion2 += ". " + exPais;
        exUbicacion2 += " C.P. " + exCodigoPostal;

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 5; //5
        double posRazon = fPropTitulo.rSize + sep;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;

        fPropTitulo.rSize = 6;
        fPropNormal.rSize = 5;
        //fPropNormal = new FontProp(fuenteNormal, 4);
        fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);

        double i = 0;
        string sDateElaboracion = string.Empty;

        //Agregamos los paneles visuales para el tipo de documento, serie y folio
        double fAltoPanel = Encabezado.rHeight / 6;
        double fAnchoPanel = Encabezado.rWidth / 5;
        double posX = Encabezado.rWidth - fAnchoPanel;

        System.Drawing.ColorConverter colConvert = new ColorConverter();
        System.Drawing.Color ColorT = new System.Drawing.Color();
        ColorT = (System.Drawing.Color)colConvert.ConvertFromString("#f4b33d");

        //Panel de factura
        fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - 190, 0, fAnchoPanel + 75, fAltoPanel, grosorPen, radioCurva, false, sColor);
        //Panel de folio fiscal
        //fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - 190, fAltoPanel, fAnchoPanel + 75, fAltoPanel, grosorPen, radioCurva, false, sColor);
        //Panel de fecha de luga y fecha de emisión
        fnCrearPanelRedondeado(Encabezado, leftPadding - 12, 0, fAnchoPanel + 75, fAltoPanel, grosorPen, radioCurva, false, sColor);
        //Panel donde esta el emisor y datos fiscales
        fnCrearPanelRedondeado(Encabezado, leftPadding - 12, fAltoPanel + 2, Encabezado.rWidth, Encabezado.rHeight - (fAltoPanel * 3) + 23, grosorPen, radioCurva, true, ColorT);


        sDateElaboracion = fechaComprobante.ToString("yyyy/MM/dd") + " T " + fechaComprobante.ToString("HH") + ":" + fechaComprobante.ToString("mm") + ":" + fechaComprobante.ToString("ss");//fechaComprobante.ToString("dd") + " de " + fechaComprobante.ToString("MMMM") + " de " + fechaComprobante.ToString("yyyy") + " T " + fechaComprobante.ToString("HH") +":"+ fechaComprobante.ToString("mm") +":"+ fechaComprobante.ToString("ss");
        //Lugar y Fecha
        if (fechaComprobante != DateTime.MinValue)
        {
            //i = fnAgregarMultilinea(Encabezado, "Lugar y fecha de elaboración:", fPropNormal, leftPadding, posRenglon, 40, true);
            //Renglon += i;
            //if (i > 1)
            //    Renglon -= 0.5;
            //i = fnAgregarMultilinea(Encabezado, pais + "," + estado + " a " + sDateElaboracion, fPropNormal, leftPadding, posRenglon + tamRenglon * Renglon, 40, true);
            //Renglon += i;
            //if (i > 1)
            //    Renglon -= 0.5;

            Encabezado.AddAligned(leftPadding, RepObj.rAlignLeft,
                posRenglon - 12, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Lugar y fecha de elaboración:"));
            Encabezado.AddAligned(leftPadding, RepObj.rAlignLeft,
                posRenglon - 2, RepObj.rAlignTop,
                new RepString(fPropNormal, pais + "," + estado + " a " + sDateElaboracion));

        }

        Renglon = 3;
        fPropNegrita = new FontProp(fuenteNormal, 10, Color.Black);
        //i = fnAgregarMultilinea(Encabezado, razonSocial, fPropTitulo, leftPadding, posRenglon + tamRenglon * Renglon, 40, true);
        i = fnAgregarMultilinea(Encabezado, "MANTENIMIENTO Y", fPropNegrita, leftPadding + 30, posRenglon + tamRenglon * Renglon, 40, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 0.5;
        Renglon += i;
        if (i > 1)
            Renglon -= 0.5;
        i = fnAgregarMultilinea(Encabezado, "SUMINISTRO AUTOMOTRIZ,", fPropNegrita, leftPadding, posRenglon + tamRenglon * Renglon, 40, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 0.5;
        Renglon += i;
        if (i > 1)
            Renglon -= 0.5;
        i = fnAgregarMultilinea(Encabezado, "S.A DE C.V.", fPropNegrita, leftPadding + 50, posRenglon + tamRenglon * Renglon, 40, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 0.5;
        i = fnAgregarMultilinea(Encabezado, "R.F.C.: MSA130220NP2", fPropNormal, leftPadding + 50, posRenglon + tamRenglon * Renglon, 46, true);

        Renglon = 3;
        fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);

        if (exCalle != string.Empty)
        {
            i = fnAgregarMultilinea(Encabezado, "EXPEDIDO EN", fPropExpedidoEn, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
            i = fnAgregarMultilinea(Encabezado, exDireccion, fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
            i = fnAgregarMultilinea(Encabezado, exUbicacion, fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
            i = fnAgregarMultilinea(Encabezado, exUbicacion2, fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

        }
        else
        {
            i = fnAgregarMultilinea(Encabezado, "E. Pallares y Portilo No. 8", fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
            i = fnAgregarMultilinea(Encabezado, "Col. Parque San Andrés,", fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
            i = fnAgregarMultilinea(Encabezado, "Deleg. Coyoacán,", fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
            i = fnAgregarMultilinea(Encabezado, "C.P. 04040, México, D.F.", fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
            i = fnAgregarMultilinea(Encabezado, "Tels.: 5575-3652 / 5575-2698", fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
        }

        string sRegFis = string.Empty;
        if (!string.IsNullOrEmpty(referencia))
        {
            string Refer = "Referencia: " + referencia;
            //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 4.5, new RepString(fPropNormal, Refer));
            i = fnAgregarMultilinea(Encabezado, Refer, fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
            if (version == "3.2")
            {
                sRegFis = "Régimen Fiscal: ";
                //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 5.3, new RepString(fPropNormal, sRegFis));
                //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 6.1, new RepString(fPropNormal, Regimenfiscal));
                //i = fnAgregarMultilinea(Encabezado, sRegFis, fPropNormal, leftPadding + 250, posRenglon + tamRenglon * Renglon, 46, true);
                //Renglon += i;
                //if (i > 1)
                //    Renglon -= 0.5;
                i = fnAgregarMultilinea(Encabezado, sRegFis + "Régimen General de Ley de Personas Morales", fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
                Renglon += i;
                if (i > 1)
                    Renglon -= 0.5;
            }
        }
        else
        {
            if (version == "3.2")
            {
                sRegFis = "Régimen Fiscal: ";
                //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 4.5, new RepString(fPropNormal, sRegFis));
                //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 5.3, new RepString(fPropNormal, Regimenfiscal));
                //i = fnAgregarMultilinea(Encabezado, sRegFis, fPropNormal, leftPadding + 250, posRenglon + tamRenglon * Renglon, 46, true);
                //Renglon += i;
                //if (i > 1)
                //    Renglon -= 0.5;
                i = fnAgregarMultilinea(Encabezado, sRegFis + "Régimen General de Ley de Personas Morales", fPropNormal, leftPadding + 230, posRenglon + tamRenglon * Renglon, 46, true);
                Renglon += i;
                if (i > 1)
                    Renglon -= 0.5;
            }
        }

        //fnAgregarMultilineaCentrada(Encabezado, TipoDocumento, fPropBlanca, posX + fAnchoPanel / 2, fAltoPanel / 2, 15);
        fnAgregarMultilinea(Encabezado, TipoDocumento, fPropNegrita, posX, fAltoPanel / 3, 15, true);
        if (!string.IsNullOrEmpty(serie) && !string.IsNullOrEmpty(folio))
        {
            fnAgregarMultilinea(Encabezado, "No." + serie + folio, fPropNormal, posX, (fAltoPanel / 3) + (fAltoPanel / 3), 15, true);
        }
        

        //Si existe complemento donativas en XML se agrega un panel y se escrcibe datos del mismo
        if (!(string.IsNullOrEmpty(noAutorizacion)))
        {
            //double dYPnlDon = 484;
            //double dWPnlDon = 277;
            //double dHPnlDon = 35;
            //fnCrearPanelRedondeado(Encabezado, 0, dYPnlDon, fAnchoPanel + dWPnlDon, fAltoPanel + dHPnlDon, grosorPen, radioCurva, false, sColor);
            int r = 497;
            int ni = 0;
            double dXDon = 7;
            int dWTxt = 120;
            ni = fnAgregarMultilinea(Encabezado, leyenda, fPropNormal, dXDon, r, dWTxt, true);
            if (ni == 1)
                r += ni + 10;
            else
                r += ni + 30;

            ni = fnAgregarMultilinea(Encabezado, "No. Aprobación: " + noAutorizacion, fPropNormal, dXDon, r, dWTxt, true);
            if (ni == 1)
                r += ni + 10;
            else
                r += ni + 30;

            fnAgregarMultilinea(Encabezado, "Fecha de Aprobación: " + fechaAutorizacion, fPropNormal, dXDon, r, dWTxt, true);
        }

        string sUUID = string.Empty;
        string noCertificadoSAT = string.Empty;

        try { sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
        catch { }
        try { noCertificadoSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
        catch { }

        posX = Encabezado.rWidth - leftPadding;


        //Folio Fiscal
        if (!string.IsNullOrEmpty(sUUID))
        {
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel + fAltoPanel / 4.5, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Folio Fiscal:"));
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel + fAltoPanel / 1.5, RepObj.rAlignTop,
                new RepString(fPropNormal, sUUID.ToUpper()));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 4, RepObj.rAlignTop,
            //    new RepString(fPropNegrita, "Folio Fiscal:"));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 16, RepObj.rAlignTop,
            //    new RepString(fPropNormal, sUUID));
        }

        fPropNormal = new FontProp(fuenteNormal, 4);
        fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);

        //Numero de Serie del Emisor
        if (!string.IsNullOrEmpty(noCertificadoEmisor))
        {
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + 4, RepObj.rAlignTop,
                new RepString(fPropNegrita, "No. de Serie del Certificado del Emisor:"));
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + 12, RepObj.rAlignTop,
                new RepString(fPropNormal, noCertificadoEmisor));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 28, RepObj.rAlignTop,
            //    new RepString(fPropNegrita, "No de Serie del Certificado del SAT:"));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 40, RepObj.rAlignTop,
            //    new RepString(fPropNormal, noCertificadoSAT));
        }

        //Numero de Serie del SAT
        if (!string.IsNullOrEmpty(noCertificadoSAT))
        {
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + 22, RepObj.rAlignTop,
                new RepString(fPropNegrita, "No. de Serie del Certificado del SAT:"));
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + 30, RepObj.rAlignTop,
                new RepString(fPropNormal, noCertificadoSAT));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 28, RepObj.rAlignTop,
            //    new RepString(fPropNegrita, "No de Serie del Certificado del SAT:"));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 40, RepObj.rAlignTop,
            //    new RepString(fPropNormal, noCertificadoSAT));
        }


        //Fecha de expedicion
        if (fechaTimbrado != DateTime.MinValue)//(fechaComprobante != DateTime.MinValue)
        {
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + 40, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Fecha y hora de certificación:"));
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + 48, RepObj.rAlignTop,
                new RepString(fPropNormal, fechaTimbrado.ToString("s")));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 52, RepObj.rAlignTop,
            //    new RepString(fPropNegrita, "Fecha y hora de certificación:"));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 64, RepObj.rAlignTop,
            //    new RepString(fPropNormal, fechaComprobante.ToString("s")));
        }

        //sDateElaboracion = fechaComprobante.ToString("yyyy/MM/dd") + " T " + fechaComprobante.ToString("HH") + ":" + fechaComprobante.ToString("mm") + ":" + fechaComprobante.ToString("ss");//fechaComprobante.ToString("dd") + " de " + fechaComprobante.ToString("MMMM") + " de " + fechaComprobante.ToString("yyyy") + " T " + fechaComprobante.ToString("HH") +":"+ fechaComprobante.ToString("mm") +":"+ fechaComprobante.ToString("ss");
        ////Lugar y Fecha
        //if (fechaComprobante != DateTime.MinValue)
        //{
        //    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //    //    fAltoPanel * 2 + 58, RepObj.rAlignTop,
        //    //    new RepString(fPropNegrita, "Lugar, fecha y hora de emisión:"));
        //    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //    //    fAltoPanel * 2 + 66, RepObj.rAlignTop,
        //    //    new RepString(fPropNormal, estadolug + " " + sDateElaboracion));

        //    //fnAgregarMultilinea(Encabezado, razonSocial, fPropTitulo, leftPadding, posRenglon, 40, true);

        //    Encabezado.AddAligned(leftPadding, RepObj.rAlignLeft,
        //        5, RepObj.rAlignTop,
        //        new RepString(fPropNegrita, "Lugar y fecha de elaboración:"));
        //    Encabezado.AddAligned(leftPadding, RepObj.rAlignLeft,
        //        15, RepObj.rAlignTop,
        //        new RepString(fPropNormal, pais + "," + estado + " a " + sDateElaboracion));
        //}

        //Serie y Folio
        //if (!string.IsNullOrEmpty(serie) && !string.IsNullOrEmpty(folio))
        //{
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //            fAltoPanel * 2 + 75, RepObj.rAlignTop,
        //        new RepString(fPropNegrita, "Serie y Folio:"));
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //            fAltoPanel * 2 + 83, RepObj.rAlignTop,
        //        new RepString(fPropRoja, serie + folio));
        //}


        fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
        fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);

    }

    /// <summary>
    /// Agrega los datos del receptor al área de encabezado
    /// </summary>
    /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
    /// <param name="navEncabezado">Navegador con los datos del comprobante XML</param>
    /// <param name="Encabezado">Área del encabezado donde se mostrarán los datos</param>
    private void fnDatosReceptor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, double Renglon, System.Drawing.Color sColor)
    {
        string razonSocial, rfc, calle, noExterior, noInterior, colonia, municipio, estado, pais, codigoPostal, serie, folio, Localidad;
        razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = serie = folio = Localidad = string.Empty;

        try { serie = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value; }
        catch { }
        try { folio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
        catch { }

        try { razonSocial = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value; }
        catch { }
        rfc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;
        calle = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@calle", nsmComprobante).Value;
        try { noExterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@noExterior", nsmComprobante).Value; }
        catch { }
        try { noInterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@noInterior", nsmComprobante).Value; }
        catch { }
        try { colonia = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@colonia", nsmComprobante).Value; }
        catch { }
        try { municipio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@municipio", nsmComprobante).Value; }
        catch { }
        try { estado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@estado", nsmComprobante).Value; }
        catch { }
        pais = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@pais", nsmComprobante).Value;
        try { codigoPostal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@codigoPostal", nsmComprobante).Value; }
        catch { }

        try { Localidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@localidad", nsmComprobante).Value; }
        catch { }

        string direccion = string.Empty;
        direccion += calle;
        if (!string.IsNullOrEmpty(noExterior))
            direccion += " No. " + noExterior;
        if (!string.IsNullOrEmpty(noInterior))
            direccion += " Int. " + noInterior;
        if (!string.IsNullOrEmpty(colonia))
            direccion += " Colonia " + colonia;


        string ubicacion = string.Empty;

        if (!string.IsNullOrEmpty(Localidad))
            ubicacion += Localidad;
        if (!string.IsNullOrEmpty(Localidad))
        {
            ubicacion += ", " + municipio;
        }
        else
        {
            ubicacion += municipio;
        }

        if (!string.IsNullOrEmpty(estado))
            ubicacion += ", " + estado;
        if (!string.IsNullOrEmpty(pais))
            ubicacion += ". " + pais;
        if (!string.IsNullOrEmpty(codigoPostal))
            ubicacion += " C.P. " + codigoPostal;

        //fPropNormal = new FontProp(fuenteNormal, 4);
        fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 5;//10
        double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;
        fPropNormal.rSize = 5;

        double fAltoPanel = Encabezado.rHeight / 4;
        double fAnchoPanel = Encabezado.rWidth / 5;
        double posX = Encabezado.rWidth - fAnchoPanel;
        //panel dividido en 2 cliente, direccion
        fnCrearPanelRedondeado(Encabezado, leftPadding - 12, Encabezado.rHeight - 36, Encabezado.rWidth, fAltoPanel-10, grosorPen, radioCurva, false, sColor);
        Encabezado.Add(leftPadding - 12, Encabezado.rHeight - 18, new RepLine(new PenProp(PDF, grosorPen, sColor), Encabezado.rWidth, 0));
        //fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - 190, fAltoPanel, fAnchoPanel + 75, fAltoPanel, grosorPen, radioCurva, false, sColor);

        Encabezado.Add(leftPadding, Encabezado.rHeight - 25, new RepString(fPropNegrita, "CLIENTE: " + razonSocial)); //posRazon + 30
        Encabezado.Add(leftPadding, Encabezado.rHeight - 8, new RepString(fPropNegrita, "DIRECCIÓN: " + direccion + " " + ubicacion)); //* 2

        Encabezado.Add((Encabezado.rWidth / 2) + 70, Encabezado.rHeight - 25, new RepString(fPropNegrita, "R.F.C: " + rfc)); //*3
        Encabezado.Add((Encabezado.rWidth / 2) + 170, Encabezado.rHeight - 25, new RepString(fPropNegrita, "TEL.:")); //*2.5


        //double Renglon = 0;
        //Renglon += 0.5;
        //double i = 0;
        //i = fnAgregarMultilinea(Encabezado, razonSocial, fPropTitulo, leftPadding, posRenglon + tamRenglon * Renglon++, 75, true);
        //Renglon += i;
        //if (i > 1)
        //    Renglon -= 1.5;
        //else
        //    Renglon -= 1;

        //i = fnAgregarMultilinea(Encabezado, direccion, fPropNormal, leftPadding, posRenglon + tamRenglon * Renglon, 90, true);
        //Renglon += i;
        //if (i > 1)
        //    Renglon -= 0.5;

        //if (Renglon - 0.8 < Renglon)
        //{
        //    i = fnAgregarMultilinea(Encabezado, ubicacion, fPropNormal, leftPadding, posRenglon + tamRenglon * (Renglon - 0.3), 90, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
        //}
        //else
        //{
        //    i = fnAgregarMultilinea(Encabezado, ubicacion, fPropNormal, leftPadding, posRenglon + tamRenglon * (Renglon - 0.8), 90, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
        //}
        //if (Renglon - 1.2 < Renglon)
        //{
        //    i = fnAgregarMultilinea(Encabezado, rfc, fPropNormal, leftPadding, posRenglon + tamRenglon * (Renglon - 0.5), 90, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
        //}
        //else
        //{
        //    i = fnAgregarMultilinea(Encabezado, rfc, fPropNormal, leftPadding, posRenglon + tamRenglon * (Renglon - 1.2), 90, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
        //}


        fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
        fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);
    }

    #endregion
    //===============================================================

    //====== FUNCIONES AYUDA ========================================
    #region ayuda

    /// <summary>
    /// Calcula la posición intermedia de un renglon ficticio
    /// </summary>
    /// <param name="poObjetoContenedor">Objeto contenedor que define el área máxima</param>
    /// <param name="pnNoRenglones">Número de renglones ficticio en el que estaría dividido el contenedor</param>
    /// <param name="renglon">Renglon ficticio del cual se desea obtener la posición intermedia</param>
    /// <returns></returns>
    private double fnCentrarEnRenglon(RepObj poObjetoContenedor, int pnNoRenglones, int renglon)
    {
        double altoRenglon = poObjetoContenedor.rHeight / pnNoRenglones;
        return altoRenglon * renglon - altoRenglon / 2;
    }
    /// <summary>
    /// Inserta en el documento una cadena multilinea con el formato especificado
    /// </summary>
    /// <param name="pContenedor">Objeto StaticContainer al cual se le añadirá la cadena de texto</param>
    /// <param name="psTexto">Cadena de texto</param>
    /// <param name="pFuente">Fuente a utilizar</param>
    /// <param name="pdPosX">Posición en X relativa al margen del contenedor</param>
    /// <param name="pdPosY">Posición en Y relativa al margen del contenedor</param>
    /// <param name="pnTamRenglon">Ancho en número de caracteres que tendrá cada renglon</param>
    /// /// <param name="bBuscarEspacio">Especifica si la cadena de texto debe ser cortada en la posición del ultimo espacio en blanco</param>
    /// <returns>Retorna un entero especificando el número de renglones final</returns>
    private int fnAgregarMultilinea(StaticContainer pContenedor, string psTexto, FontProp pFuente, double pdPosX, double pdPosY, int pnTamRenglon, bool bBuscarEspacio)
    {
        double nAlturaRenglon = pFuente.rSize * 1.2;
        int nNoRenglon = 0;

        foreach (RepString r in fnDividirRenglones(psTexto, pnTamRenglon, pFuente, bBuscarEspacio))
        {
            pContenedor.Add(pdPosX, pdPosY + nAlturaRenglon * nNoRenglon, r);
            nNoRenglon++;
        }

        return nNoRenglon;
    }

    /// <summary>
    /// Inserta en el documento una cadena multilinea con el formato especificado
    /// </summary>
    /// <param name="pContenedor">Objeto StaticContainer al cual se le añadirá la cadena de texto</param>
    /// <param name="psTexto">Cadena de texto</param>
    /// <param name="pFuente">Fuente a utilizar</param>
    /// <param name="pdPosX">Posición en X relativa al margen del contenedor</param>
    /// <param name="pdPosY">Posición en Y relativa al margen del contenedor</param>
    /// <param name="pnTamRenglon">Ancho en número de caracteres que tendrá cada renglon</param>
    /// <returns>Retorna un entero especificando el número de renglones final</returns>
    private int fnAgregarMultilineaCentrada(StaticContainer pContenedor, string psTexto, FontProp pFuente, double pdPosX, double pdPosY, int pnTamRenglon)
    {
        double nAlturaRenglon = pFuente.rSize * 1.2;
        int nNoRenglon = 0;

        foreach (RepString r in fnDividirRenglones(psTexto, pnTamRenglon, pFuente, true))
        {
            pContenedor.AddAligned(pdPosX, RepObj.rAlignCenter, pdPosY + nAlturaRenglon * nNoRenglon, RepObj.rAlignCenter, r);
            nNoRenglon++;
        }

        return nNoRenglon;
    }

    /// <summary>
    /// Inserta en el documento una cadena multilinea con el formato especificado
    /// </summary>
    /// <param name="pContenedor">Objeto StaticContainer al cual se le añadirá la cadena de texto</param>
    /// <param name="psTexto">Cadena de texto</param>
    /// <param name="pFuente">Fuente a utilizar</param>
    /// <param name="pdPosX">Posición en X relativa al margen del contenedor</param>
    /// <param name="pdPosY">Posición en Y relativa al margen del contenedor</param>
    /// <param name="pnTamRenglon">Ancho en número de caracteres que tendrá cada renglon</param>
    /// /// <param name="bBuscarEspacio">Especifica si la cadena de texto debe ser cortada en la posición del ultimo espacio en blanco</param>
    /// <returns>Retorna un entero especificando el número de renglones final</returns>
    private int fnAgregarMultilineaDetalle(StaticContainer pContenedor, string psTexto, FontProp pFuente, double pdPosX, double pdPosY, int pnTamRenglon, bool bBuscarEspacio)
    {
        double nAlturaRenglon = pFuente.rSize * 1.2;
        int nNoRenglon = 0;
        double dY = pdPosY + nAlturaRenglon * nNoRenglon;
        foreach (RepString r in fnDividirRenglonesDetalle(psTexto, pnTamRenglon, pFuente, bBuscarEspacio))
        {
            pContenedor.Add(pdPosX, pdPosY + nAlturaRenglon * nNoRenglon, r);
            nNoRenglon++;

        }
        return nNoRenglon;
    }

    /// <summary>
    /// Toma una cadena de texto y la divide en renglones del ancho especificado,
    /// donde el ancho es el número de caracteres por linea
    /// </summary>
    /// <param name="psTexto">Cadena de texto a dividir</param>
    /// <param name="pnTamRenglon">Número de caracteres por renglón</param>
    /// <param name="pFuente">Objeto FontProp con las características de la fuente a usar</param>
    /// /// <param name="bBuscarEspacio">Especifica si la cadena de texto debe ser cortada en la posición del ultimo espacio en blanco</param>
    /// <returns>Retorna una lista de objetos RepString repersentando los renglones</returns>
    private List<RepString> fnDividirRenglonesDetalle(string psTexto, int pnTamRenglon, FontProp pFuente, bool bBuscarEspacio)
    {
        List<RepString> renglones = new List<RepString>();
        int nlongitudTexto = psTexto.Length;
        int nUltimoEspacio = pnTamRenglon;
        int nCursorCadena = 0;
        int nSeguro = 0;
        //En el resto de la cadena se busca salto de lineas en el resto de la cadena
        string sText = string.Empty; ;
        string[] sCad;
        char[] cCad = { '\n' };
        sCad = psTexto.Split(cCad);
        //Si la cadena sobre pasa de la longitud configurada se parte la cadena
        if (nlongitudTexto > pnTamRenglon || sCad.Length > 1)
        {
            //Si hay que buscar espacios
            if (bBuscarEspacio)
            {
                //Mientras allá texto que escribir
                while (nlongitudTexto > 0)
                {
                    //Se busca un salto de linea en la cadena
                    sCad = psTexto.Split(cCad);
                    //Si existe un salto de linea en la cadena
                    if (sCad.Length > 1)
                    {
                        foreach (string s in sCad)
                        {
                            //Se verifica si la cadena no revasa al limite permitido
                            if (s.Length > pnTamRenglon)
                            {
                                //Si revasa el limite se corta en un espacio y se escribe
                                nUltimoEspacio = s.Substring(0, pnTamRenglon).LastIndexOf(" ");
                                if (nUltimoEspacio == -1 || nUltimoEspacio == 0)
                                    nUltimoEspacio = pnTamRenglon;
                                renglones.Add(new RepString(pFuente, s.Substring(0, nUltimoEspacio).TrimStart()));
                            }
                            else
                            {
                                //Si no revasa lo permitido se escribe la linea
                                nUltimoEspacio = s.Length + 1;
                                renglones.Add(new RepString(pFuente, s.ToString().TrimStart()));
                            }
                            //Se asigna la cadena restante a escribir
                            psTexto = psTexto.Substring(nUltimoEspacio).ToString();

                            break;
                        }
                    }
                    else //Si no existe saltos de linea en la cadena
                    {
                        //Se verifica si la cadena no revasa al limite permitido
                        if (psTexto.Length > pnTamRenglon)
                        {
                            //Si revasa el limite se corta en un espacio y se escribe
                            nUltimoEspacio = psTexto.Substring(0, pnTamRenglon).LastIndexOf(" ");
                            if (nUltimoEspacio == -1 || nUltimoEspacio == 0)
                                nUltimoEspacio = pnTamRenglon;
                            renglones.Add(new RepString(pFuente, psTexto.Substring(0, nUltimoEspacio).TrimStart()));
                        }
                        else
                        {
                            //Si no revasa lo permitido se escribe la linea
                            nUltimoEspacio = psTexto.Length;
                            renglones.Add(new RepString(pFuente, psTexto.ToString().TrimStart()));
                        }
                        //Se asigna la cadena restante a escribir
                        psTexto = psTexto.Substring(nUltimoEspacio).ToString();
                    }

                    nCursorCadena += nUltimoEspacio;
                    nlongitudTexto = psTexto.Length; //nUltimoEspacio;

                    //Seguro para evitar ciclos infinitos
                    nSeguro++;
                    if (nSeguro > 100)
                    {
                        renglones.Clear();
                        break;
                    }
                }

            }
            else
                nUltimoEspacio = pnTamRenglon;
        }
        else
            renglones.Add(new RepString(pFuente, psTexto.Substring(nCursorCadena).TrimStart()));

        return renglones;
    }

    /// <summary>
    /// Toma una cadena de texto y la divide en renglones del ancho especificado,
    /// donde el ancho es el número de caracteres por linea
    /// </summary>
    /// <param name="psTexto">Cadena de texto a dividir</param>
    /// <param name="pnTamRenglon">Número de caracteres por renglón</param>
    /// <param name="pFuente">Objeto FontProp con las características de la fuente a usar</param>
    /// /// <param name="bBuscarEspacio">Especifica si la cadena de texto debe ser cortada en la posición del ultimo espacio en blanco</param>
    /// <returns>Retorna una lista de objetos RepString repersentando los renglones</returns>
    private List<RepString> fnDividirRenglones(string psTexto, int pnTamRenglon, FontProp pFuente, bool bBuscarEspacio)
    {
        List<RepString> renglones = new List<RepString>();
        int nlongitudTexto = psTexto.Length;
        int nUltimoEspacio = pnTamRenglon;
        int nCursorCadena = 0;
        int nSeguro = 0;

        while (nlongitudTexto > pnTamRenglon)
        {
            if (bBuscarEspacio)
            {
                nUltimoEspacio = psTexto.Substring(nCursorCadena, pnTamRenglon).LastIndexOf(" ");

                if (nUltimoEspacio == -1 || nUltimoEspacio == 0)
                    nUltimoEspacio = pnTamRenglon;
            }
            else
                nUltimoEspacio = pnTamRenglon;

            renglones.Add(new RepString(pFuente, psTexto.Substring(nCursorCadena, nUltimoEspacio).TrimStart()));
            nCursorCadena += nUltimoEspacio;
            nlongitudTexto -= nUltimoEspacio;

            //Seguro para evitar ciclos infinitos
            nSeguro++;
            if (nSeguro > 100)
            {
                renglones.Clear();
                break;
            }
        }
        renglones.Add(new RepString(pFuente, psTexto.Substring(nCursorCadena).TrimStart()));

        return renglones;
    }

    /// <summary>
    /// Crea un nuevo panel visual
    /// </summary>
    /// <param name="ancho">ancho del panel en mm</param>
    /// <param name="alto">alto del panel en mm</param>
    /// <returns>Retorna un objeto RepRect con las caraterísticas visuales especificadas</returns>
    private RepRect fnCrearPanel(double ancho, double alto, System.Drawing.Color sColor)
    {
        PenProp pen = new PenProp(PDF, 1, sColor);

        return new RepRect(pen, RT.rPointFromMM(ancho), RT.rPointFromMM(alto));
    }

    /// <summary>
    /// Crea un nuevo panel visualcon esquinas redondeadas
    /// </summary>
    /// <param name="poObjeto">Objeto al que se añadira el panel</param>
    /// <param name="pfPosX">Posición en X relativa al objeto contenedor</param>
    /// <param name="pfPosY">Posición en Y relativa al objeto contenedor</param>
    /// <param name="pfAncho">Ancho del panel</param>
    /// <param name="pfAlto">Alto del panel</param>
    /// <param name="pfGrosor">Grosor en puntos de la linea del borde</param>
    /// <param name="pfRadioCurva">Radio de la cirunferencia tomada para crear las esquinas redondeadas</param>
    /// <param name="pbRellenar">Indica si el panel debe ser rellenado</param>
    public void fnCrearPanelRedondeado(StaticContainer poObjeto, double pfPosX, double pfPosY, double pfAncho, double pfAlto, double pfGrosor, double pfRadioCurva, bool pbRellenar, System.Drawing.Color sColor)
    {

        PenProp pen = new PenProp(PDF, pfGrosor, sColor);
        BrushProp brush = new BrushProp(PDF, sColor);

        //Longitud del recuadro que contiene al arco
        //Recordar que la posicion que se le de a dicho recuadro es relativa 
        //a su esquina suprior izquierda
        double lArc = pfRadioCurva * 2;

        if (pbRellenar)
        {
            //creamos el relleno
            //primero los recuadros base
            poObjeto.Add(pfPosX, pfPosY + pfRadioCurva, new RepRect(pen, brush, pfAncho, lArc - pfAlto));
            poObjeto.Add(pfPosX + pfRadioCurva, pfPosY, new RepRect(pen, brush, pfAncho - lArc, -pfAlto));

            //luego las esquinas redondeadas
            poObjeto.Add(pfPosX, pfPosY + lArc, new RepCircle(pen, brush, pfRadioCurva));
            poObjeto.Add(pfPosX + pfAncho - lArc, pfPosY + lArc, new RepCircle(pen, brush, pfRadioCurva));
            poObjeto.Add(pfPosX, pfPosY + pfAlto, new RepCircle(pen, brush, pfRadioCurva));
            poObjeto.Add(pfPosX + pfAncho - lArc, pfPosY + pfAlto, new RepCircle(pen, brush, pfRadioCurva));
        }
        else
        {
            //esquina superior izquierda
            poObjeto.Add(pfPosX, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 180, 90));

            //esquina superior derecha
            poObjeto.Add(pfPosX + pfAncho - lArc, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 270, 90));

            //esquina inferior izquierda
            poObjeto.Add(pfPosX, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 90, 90));

            //esquina inferior derecha
            poObjeto.Add(pfPosX + pfAncho - lArc, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 0, 90));


            //añadimos los bordes rectos
            //borde superior
            poObjeto.Add(pfPosX + pfRadioCurva, pfPosY, new RepLine(pen, pfAncho - lArc, 0));

            //borde inferior
            poObjeto.Add(pfPosX + pfRadioCurva, pfPosY + pfAlto, new RepLine(pen, pfAncho - lArc, 0));

            ////borde izquierdo
            poObjeto.Add(pfPosX, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));

            ////borde derecho
            poObjeto.Add(pfPosX + pfAncho, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));
        }
    }

    /// <summary>
    /// Añade un borde al objecto especificado
    /// </summary>
    /// <param name="poObjeto">Objeto al que se le añadirá el borde</param>
    /// <param name="pfGrosor">Medida en puntos para el grosor del borde</param>
    private void fnAgregarBorde(StaticContainer poObjeto, double pfGrosor, System.Drawing.Color sColor)
    {
        PenProp pen = new PenProp(PDF, pfGrosor, sColor);
        poObjeto.Add(0, poObjeto.rHeight, new RepRect(pen, poObjeto.rWidth, poObjeto.rHeight));
    }

    /// <summary>
    /// Añade un borde con esquinas redondeadas al objeto especificado
    /// </summary>
    /// <param name="poObjeto">Objeto al que se le añadirá el borde</param>
    /// <param name="pfGrosor">Medida en puntos para el grosor del borde</param>
    /// <param name="pfRadioCurva">Radio para calcular los bordes redondos</param>
    private void fnAgregarBordeRedondeado(StaticContainer poObjeto, double pfGrosor, double pfRadioCurva, System.Drawing.Color sColor)
    {
        PenProp pen = new PenProp(PDF, pfGrosor, sColor);

        //Longitud del recuadro que contiene al arco
        //Recordar que la posicion que se le de a dicho recuadro es relativa 
        //a su esquina suprior izquierda
        double lArc = pfRadioCurva * 2;

        //esquina superior izquierda
        poObjeto.Add(0, lArc, new RepArc(pen, pfRadioCurva, 180, 90));

        //esquina superior derecha
        poObjeto.Add(poObjeto.rWidth - lArc, lArc, new RepArc(pen, pfRadioCurva, 270, 90));

        //esquina inferior izquierda
        poObjeto.Add(0, poObjeto.rHeight, new RepArc(pen, pfRadioCurva, 90, 90));

        //esquina inferior derecha
        poObjeto.Add(poObjeto.rWidth - lArc, poObjeto.rHeight, new RepArc(pen, pfRadioCurva, 0, 90));


        //añadimos los bordes rectos
        //borde superior
        poObjeto.Add(pfRadioCurva, 0, new RepLine(pen, poObjeto.rWidth - lArc, 0));

        //borde inferior
        poObjeto.Add(pfRadioCurva, poObjeto.rHeight, new RepLine(pen, poObjeto.rWidth - lArc, 0));

        ////borde izquierdo
        poObjeto.Add(0, pfRadioCurva, new RepLine(pen, 0, pfRadioCurva * 2 - poObjeto.rHeight));

        ////borde derecho
        poObjeto.Add(poObjeto.rWidth, pfRadioCurva, new RepLine(pen, 0, pfRadioCurva * 2 - poObjeto.rHeight));

        //Borde izquierdo antes del codigo
        double dWBD1 = 515;
        double dYBD1 = -16;
        poObjeto.Add(poObjeto.rWidth - dWBD1, pfRadioCurva + dYBD1, new RepLine(pen, 0, pfRadioCurva * 2 - poObjeto.rHeight - 20));

        //Borde izquierdo antes de la unidad de medida
        double dWBD2 = 465;
        double dYBD2 = -16;
        poObjeto.Add(poObjeto.rWidth - dWBD2, pfRadioCurva + dYBD2, new RepLine(pen, 0, pfRadioCurva * 2 - poObjeto.rHeight - 20));

        //Borde derecho despues de descripción
        double dWBD3 = 400;
        double dYBD3 = -16;
        poObjeto.Add(poObjeto.rWidth - dWBD3, pfRadioCurva + dYBD3, new RepLine(pen, 0, pfRadioCurva * 2 - poObjeto.rHeight - 20));

        //Borde derecho despues del precio unitario
        double dWBD4 = 160;
        double dYBD4 = -16;
        poObjeto.Add(poObjeto.rWidth - dWBD4, pfRadioCurva + dYBD4, new RepLine(pen, 0, pfRadioCurva * 2 - poObjeto.rHeight - 20));

        //Borde derecho despues del importe
        double dWBD5 = 80;
        double dYBD5 = -16;
        poObjeto.Add(poObjeto.rWidth - dWBD5, pfRadioCurva + dYBD5, new RepLine(pen, 0, pfRadioCurva * 2 - poObjeto.rHeight - 20));
    }

    /// <summary>
    /// Devuelve la cadena de texto correspondiente al valor especificado
    /// </summary>
    /// <param name="total">Valor del cual se quiere el texto</param>
    /// <returns>Cadena con el texto del valor</returns>
    private string fnTextoImporte(string psValor, string psMoneda)
    {
        CultureInfo languaje;
        NumaletMasa parser = new NumaletMasa();
        parser.LetraCapital = true;

        switch (psMoneda)
        {
            case "MXN":
                parser.TipoMoneda = NumaletMasa.Moneda.Peso;
                break;
            case "USD":
                parser.TipoMoneda = NumaletMasa.Moneda.Dolar;
                break;
            case "XEU":
                parser.TipoMoneda = NumaletMasa.Moneda.Euro;
                break;
        }

        languaje = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString());

        //return parser.ToCustomString(Convert.ToDouble(psValor));
        return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
    }


    /// <summary>
    /// Genera una nueva imagen en formato jpeg con el código de barras bidimensional
    /// </summary>
    /// <param name="psCadena">La cadena a convertir</param>
    /// <returns></returns>
    private RepImage GenerarCodigoBidimensional()
    {
        //Creamos la cadena que generará el código
        XmlNamespaceManager nsm = new XmlNamespaceManager(gxComprobante.NameTable);
        nsm.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        XPathNavigator navCodigo = gxComprobante.CreateNavigator();

        string sCadenaCodigo = "?re=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsm).Value
                            + "&rr=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsm).Value
                            + "&tt=" + string.Format("{0:0000000000.000000}", navCodigo.SelectSingleNode("/cfdi:Comprobante/@total", nsm).ValueAsDouble)
                            + "&id=" + navCodigo.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value.ToUpper();

        //La cadena debe tener un longitud entre 93 y 95 caracteres
        if (sCadenaCodigo.Length < 93 || sCadenaCodigo.Length > 95)
            throw new Exception("Los datos para la cadena del código CBB no cumplen con la especificación de hacienda");

        QRCodeEncoder ce = new QRCodeEncoder();
        ce.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        ce.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
        ce.QRCodeScale = 4;
        ce.QRCodeVersion = 0;

        MemoryStream ms = new MemoryStream();
        ce.Encode(sCadenaCodigo, System.Text.Encoding.UTF8).
        Save(ms, ImageFormat.Jpeg);

        return new RepImage(ms, tamCodigo - 10, tamCodigo - 10);
    }

    /// <summary>
    /// Genera Imagen con marca de agua.
    /// </summary>
    /// <returns></returns>
    private RepImage fnMarcaAguaCorpus()
    {
        MemoryStream ms = new MemoryStream();
        try
        {
            string fichero = HttpContext.Current.Server.MapPath("~/Imagenes/marca_agua_corpus.png");
            Image image = Image.FromFile(fichero);
            image.Save(ms, ImageFormat.Jpeg);
        }
        catch (Exception ex)
        {
            clsErrorLogMasa.fnNuevaEntrada(ex, clsErrorLogMasa.TipoErroresLog.Referencia);
        }
        return new RepImage(ms, 283, 144);
    }

    /// <summary>
    /// Genera Imagen PAX
    /// </summary>
    /// <returns></returns>
    private RepImage fnImagenPAX()
    {
        MemoryStream ms = new MemoryStream();
        try
        {
            string fichero = HttpContext.Current.Server.MapPath("~/Imagenes/logo_pax.png");
            Image image = Image.FromFile(fichero);
            image.Save(ms, ImageFormat.Jpeg);
        }
        catch (Exception ex)
        {
            clsErrorLogMasa.fnNuevaEntrada(ex, clsErrorLogMasa.TipoErroresLog.Referencia);
        }
        return new RepImage(ms, 40, 10);
    }

    /// <summary>
    /// Genera Imagen Cliente
    /// </summary>
    /// <returns></returns>
    private MemoryStream fnImagenCliente(int id_contribuyente, int s_Id_Rfc)
    {
        MemoryStream ms = new MemoryStream();


        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conControl"].ConnectionString;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadenaCon)))
        {
            con.Open();

            try
            {
                byte[] byteImage = { };
                using (SqlCommand cmd = new SqlCommand("usp_Con_Recupera_Logo_Sel", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (!string.IsNullOrEmpty(s_Id_Rfc.ToString()))
                        cmd.Parameters.AddWithValue("@nId_rfc", s_Id_Rfc);

                    if (!string.IsNullOrEmpty(id_contribuyente.ToString()))
                        cmd.Parameters.AddWithValue("@nId_Contribuyente", id_contribuyente);

                    if (cmd.ExecuteScalar().ToString() != string.Empty)
                        byteImage = (byte[])cmd.ExecuteScalar();
                    con.Close();

                    ms = new MemoryStream(byteImage);
                }


                //string sql = "Select logo From tbl_con_Rfcs_cat Where id_rfc =63 ";
                //SqlConnection SqlConn = new SqlConnection("server=192.168.2.13;uid=sa;pwd=F4cturax10n;database=CFDI");
                //SqlCommand SqlCom = new SqlCommand(sql, SqlConn);

                //SqlConn.Open();
                //byte[] byteImage = (byte[])SqlCom.ExecuteScalar();
                //SqlConn.Close();

                //ms = new MemoryStream(byteImage);

            }
            catch (Exception ex)
            {
                clsErrorLogMasa.fnNuevaEntrada(ex, clsErrorLogMasa.TipoErroresLog.Referencia);
            }
        }
        return ms;
    }

    /// <summary>
    /// Obtiene la imagen desde la carpeta LogosPlantillas
    /// </summary>
    /// <param name="sNombreLogo">Nombre de la imagen</param>
    /// <returns></returns>
    private MemoryStream fnImagenCliente(string sNombreLogo)
    {
        MemoryStream ms = new MemoryStream();

        try
        {
            string sImagen = HttpContext.Current.Server.MapPath("~/Imagenes/" + sNombreLogo);
            System.Drawing.Image img = new Bitmap(sImagen);
            byte[] byteImage = imageToByteArray(img);
            ms = new MemoryStream(byteImage);
        }
        catch (Exception ex)
        {

            clsErrorLogMasa.fnNuevaEntrada(ex, clsErrorLogMasa.TipoErroresLog.Referencia);
        }
        return ms;
    }

    /// <summary>
    /// Función para generar un byte array de una imagen
    /// </summary>
    /// <param name="imageIn">Imagen</param>
    /// <returns></returns>
    private byte[] imageToByteArray(System.Drawing.Image imageIn)
    {
        MemoryStream ms = new MemoryStream();
        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        return ms.ToArray();
    }

    ///// <summary>
    ///// Genera Imagen Cliente
    ///// </summary>
    ///// <returns></returns>
    //private RepImage fnImagenCliente(int id_contribuyente,int s_Id_Rfc)
    //{
    //    MemoryStream ms= new MemoryStream();


    //    string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conControl"].ConnectionString;
    //    SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadenaCon));
    //    con.Open();

    //    try
    //    {
    //        byte[] byteImage={};
    //        SqlCommand cmd = new SqlCommand("usp_Con_Recupera_Logo_Sel", con);
    //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

    //        if (!string.IsNullOrEmpty(s_Id_Rfc.ToString()))
    //            cmd.Parameters.AddWithValue("@nId_rfc", s_Id_Rfc);

    //        if (!string.IsNullOrEmpty(id_contribuyente.ToString()))
    //            cmd.Parameters.AddWithValue("@nId_Contribuyente", id_contribuyente);

    //        if (cmd.ExecuteScalar().ToString()!=string.Empty)
    //            byteImage = (byte[])cmd.ExecuteScalar();
    //        con.Close();

    //        ms = new MemoryStream(byteImage);


    //        //string sql = "Select logo From tbl_con_Rfcs_cat Where id_rfc =63 ";
    //        //SqlConnection SqlConn = new SqlConnection("server=192.168.2.13;uid=sa;pwd=F4cturax10n;database=CFDI");
    //        //SqlCommand SqlCom = new SqlCommand(sql, SqlConn);

    //        //SqlConn.Open();
    //        //byte[] byteImage = (byte[])SqlCom.ExecuteScalar();
    //        //SqlConn.Close();

    //        //ms = new MemoryStream(byteImage);

    //    }
    //    catch (Exception ex)
    //    {
    //        clsErrorLogLogo.fnNuevaEntrada(ex, clsErrorLogLogo.TipoErroresLog.Referencia);
    //    }
    //    return new RepImage(ms, 150, 41);
    //}

    //private StaticContainer fnCrearObservaciones(StaticContainer Pie, System.Drawing.Color sColor, int iPageNo, int nTotPag)
    //{
    //    //StaticContainer areaObservaciones = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, (RT.rPointFromMM(altoPie) - 135));
    //    StaticContainer areaObservaciones = new StaticContainer(RT.rPointFromMM(anchoPagina) - (margenPagina * 2) - tamCodigo, (RT.rPointFromMM(72) - 135));

    //    double leftPadding = areaObservaciones.rWidth * 0.02;
    //    double sep = 5;
    //    double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
    //    double tamRenglon = fPropNormal.rSize + sep;
    //    double posRenglon = posRazon + sep;
    //    fPropNormal.rSize = 5;

    //    //if (iPageNo == nTotPag)
    //    //    fnCrearPanelRedondeado(areaObservaciones, 0, 78, 572, 69, grosorPen, radioCurva, false, sColor);
    //    //else
    //    //    fnCrearPanelRedondeado(areaObservaciones, 0, 78, 572, 75, grosorPen, radioCurva, false, sColor);

    //    //if (iPageNo == nTotPag)
    //    //    fnCrearPanelRedondeado(areaObservaciones, 0, 78, areaObservaciones.rWidth, 69, grosorPen, radioCurva, false, sColor);
    //    //else
    //    //    fnCrearPanelRedondeado(areaObservaciones, 0, 78, areaObservaciones.rWidth, 75, grosorPen, radioCurva, false, sColor);

    //    if (iPageNo == nTotPag)
    //        fnCrearPanelRedondeado(areaObservaciones, 0, 78, areaObservaciones.rWidth, tamCodigo, grosorPen, radioCurva, false, sColor);
    //    else
    //        fnCrearPanelRedondeado(areaObservaciones, 0, 78, areaObservaciones.rWidth, tamCodigo, grosorPen, radioCurva, false, sColor);


    //    //areaObservaciones.AddAligned(posColumna4, RepObj.rAlignLeft, posRenglon, RepObj.rAlignCenter, new RepString(fPropNegrita, "Información por cuenta de terceros: "));
    //    //pagina.Add(margenPagina + 5, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo - 5 - 75, new RepString(fPropNormal, "Observaciones"));
    //    areaObservaciones.Add(leftPadding, posRazon + tamRenglon + 70, new RepString(fPropNegrita, "Observaciones:")); //posRazon + 30

    //    return areaObservaciones;
    //}

    #endregion
    //===============================================================

}

/// <summary>
/// Clase encargada de mantener y manipular la información de los impuestos del comprobante, 
/// tanto para traslados como para retenciones
/// </summary>
public class ImpuestoMasa
{
    public string Nombre { get; set; }
    public string Tasa { get; set; }
    public string Importe { get; set; }

    //Esta propiedad retorna el texto del renglon a mostrar en el PDF
    public string TextoImpuesto
    {
        get
        {
            return Nombre + " " + TransformacionMasa.fnFormatoPorcentaje(Tasa) + " " + TransformacionMasa.fnFormatoCurrency(Importe);
        }
    }

    /// <summary>
    /// Crea una nueva instancia de Impuesto
    /// </summary>
    /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
    /// <param name="nsmComprobante"></param>
    public ImpuestoMasa(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
    {
        Nombre = navImpuesto.SelectSingleNode("@impuesto", nsmComprobante).Value;
        if (Nombre != "IEPS")
        {
            try { Tasa = navImpuesto.SelectSingleNode("@tasa", nsmComprobante).Value; }
            catch { Tasa = "Retención"; }
            Importe = navImpuesto.SelectSingleNode("@importe", nsmComprobante).Value;
        }
    }
}

/// <summary>
/// Clase encargada de mantener y manipular la información de los impuestos del comprobante, 
/// tanto para traslados como para retenciones
/// </summary>
public class ImpuestoCompMasa
{
    public string Nombre { get; set; }
    public string Tasa { get; set; }
    public string Importe { get; set; }

    //Esta propiedad retorna el texto del renglon a mostrar en el PDF
    public string TextoImpuesto
    {
        get
        {
            return Nombre + " " + TransformacionMasa.fnFormatoPorcentaje(Tasa) + " " + TransformacionMasa.fnFormatoCurrency(Importe);
        }
    }

    /// <summary>
    /// Crea una nueva instancia de Impuesto
    /// </summary>
    /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
    /// <param name="nsmComprobante"></param>
    public ImpuestoCompMasa(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
    {
        try
        {
            Nombre = navImpuesto.SelectSingleNode("@ImpLocTrasladado", nsmComprobante).Value;

            try { Tasa = navImpuesto.SelectSingleNode("@TasadeTraslado", nsmComprobante).Value; }
            catch { }
        }
        catch
        {
            Nombre = navImpuesto.SelectSingleNode("@ImpLocRetenido", nsmComprobante).Value + " Retención";
            Tasa = navImpuesto.SelectSingleNode("@TasadeRetencion", nsmComprobante).Value;
        }
        Importe = navImpuesto.SelectSingleNode("@Importe", nsmComprobante).Value;
    }

}

/// <summary>
/// Clase encargada de mantener y manipular los datos de los conceptos del comprobante
/// </summary>
public class DetalleMasa
{
    private string sCantidad;
    CultureInfo languaje;

    public string cantidad
    {
        get { return sCantidad; }
        set
        {
            languaje = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString());

            //sCantidad = string.Format("{0:F2}", Convert.ToDouble(value));
            sCantidad = Convert.ToDouble(value, languaje).ToString("F2", languaje);
        }
    }
    public string unidad { get; set; }
    public string noIdentificacion { get; set; }

    public string descripcion { get; set; }

    private string sValorUnitario;
    public string valorUnitario
    {
        get { return sValorUnitario; }
        set
        {
            languaje = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString());

            //sValorUnitario = string.Format("{0:c}", Convert.ToDouble(value));
            sValorUnitario = Convert.ToDouble(value, languaje).ToString("c", languaje);
        }
    }

    private string sImporte;
    public string importe
    {
        get { return sImporte; }
        set
        {
            languaje = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString());

            //sImporte = string.Format("{0:c}", Convert.ToDouble(value));
            sImporte = Convert.ToDouble(value, languaje).ToString("c", languaje);
        }
    }

    //Campos de Aduana
    private string sNumAduana;
    public string NumAduana
    {
        get { return sNumAduana; }
        set { sNumAduana = value; }
    }
    private string sAduana;
    public string aduana
    {
        get { return sAduana; }
        set { sAduana = value; }

    }
    private DateTime dFecha;
    public DateTime Fecha
    {
        get { return dFecha; }
        set { dFecha = value; }
    }

    #region campos complemento concepto terceros

    private string _sVersionT = string.Empty;
    public string VersionT
    {
        get { return _sVersionT; }
        set { _sVersionT = value; }
    }

    private string _sRfcT = string.Empty;
    public string RfcT
    {
        get { return _sRfcT; }
        set { _sRfcT = value; }
    }

    private string _sNombreT = string.Empty;
    public string NombreT
    {
        get { return _sNombreT; }
        set { _sNombreT = value; }
    }

    private List<ComprobanteImpuestosRetencionTMasa> _retencionesT;
    public List<ComprobanteImpuestosRetencionTMasa> RetencionesT
    {
        get { return _retencionesT; }
        set { _retencionesT = value; }
    }

    private List<ComprobanteImpuestosTrasladoTMasa> _trasladosT;
    public List<ComprobanteImpuestosTrasladoTMasa> TrasladosT
    {
        get { return _trasladosT; }
        set { _trasladosT = value; }
    }

    private t_UbicacionFiscalTMasa _ubicacionFiscalT;
    public t_UbicacionFiscalTMasa UbicacionFiscalT
    {
        get { return _ubicacionFiscalT; }
        set { _ubicacionFiscalT = value; }
    }

    private t_InformacionAduaneraTMasa _informacionAduaneraT;
    public t_InformacionAduaneraTMasa InformacionAduaneraT
    {
        get { return _informacionAduaneraT; }
        set { _informacionAduaneraT = value; }
    }

    private string _sNumeroPredialT = string.Empty;
    public string NumeroPredialT
    {
        get { return _sNumeroPredialT; }
        set { _sNumeroPredialT = value; }
    }

    #endregion

    /// <summary>
    /// Crea una nueva instancia de Detalle
    /// </summary>
    /// <param name="navDetalle">Navegador con los datos del concepto</param>
    /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
    public DetalleMasa(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
    {
        try
        {
            cantidad = navDetalle.SelectSingleNode("@cantidad", nsmComprobante).Value;
            try { unidad = navDetalle.SelectSingleNode("@unidad", nsmComprobante).Value; }
            catch { unidad = string.Empty; }
            try { noIdentificacion = navDetalle.SelectSingleNode("@noIdentificacion", nsmComprobante).Value; }
            catch { noIdentificacion = string.Empty; }
            descripcion = navDetalle.SelectSingleNode("@descripcion", nsmComprobante).Value;
            valorUnitario = navDetalle.SelectSingleNode("@valorUnitario", nsmComprobante).Value;
            importe = navDetalle.SelectSingleNode("@importe", nsmComprobante).Value;
        }
        catch { }
        try
        {
            NumAduana = navDetalle.SelectSingleNode("@numero", nsmComprobante).Value;
            Fecha = Convert.ToDateTime(navDetalle.SelectSingleNode("@fecha", nsmComprobante).Value);
            aduana = navDetalle.SelectSingleNode("@aduana", nsmComprobante).Value;
        }
        catch { }

        //Complemento concepto terceros
        #region complemento concepto terceros
        try
        {
            List<ComprobanteImpuestosRetencionTMasa> impuestosRetencion = new List<ComprobanteImpuestosRetencionTMasa>();
            List<ComprobanteImpuestosTrasladoTMasa> impuestosTraslado = new List<ComprobanteImpuestosTrasladoTMasa>();

            //Navegar al nodo correspondiente
            VersionT = navDetalle.SelectSingleNode("@version", nsmComprobante).Value;
            RfcT = navDetalle.SelectSingleNode("@rfc", nsmComprobante).Value;
            try { NombreT = navDetalle.SelectSingleNode("@nombre", nsmComprobante).Value; }
            catch { NombreT = string.Empty; }

            //Impuestos retenidos
            XPathNodeIterator navImpuestosRet = navDetalle.Select("terceros:Impuestos/terceros:Retenciones/terceros:Retencion", nsmComprobante);

            while (navImpuestosRet.MoveNext())
            {
                ComprobanteImpuestosRetencionTMasa impRet = new ComprobanteImpuestosRetencionTMasa();

                try
                {
                    string sImpuestoRet = navImpuestosRet.Current.SelectSingleNode("@impuesto", nsmComprobante).Value;
                    if (!String.IsNullOrEmpty(sImpuestoRet))
                    {
                        switch (sImpuestoRet.ToUpper())
                        {
                            case "IVA": impRet.impuesto = ComprobanteImpuestosRetencionImpuestoTMasa.IVA; break;
                            case "ISR": impRet.impuesto = ComprobanteImpuestosRetencionImpuestoTMasa.ISR; break;
                        }
                    }

                    impRet.importe = Convert.ToDecimal(navImpuestosRet.Current.SelectSingleNode("@importe", nsmComprobante).Value);

                    impuestosRetencion.Add(impRet);
                }
                catch { }
            }

            if (impuestosRetencion != null && impuestosRetencion.Count > 0)
            {
                RetencionesT = impuestosRetencion;
            }

            //Impuestos trasladados
            XPathNodeIterator navImpuestosTras = navDetalle.Select("terceros:Impuestos/terceros:Traslados/terceros:Traslado", nsmComprobante);
            while (navImpuestosTras.MoveNext())
            {
                ComprobanteImpuestosTrasladoTMasa impTras = new ComprobanteImpuestosTrasladoTMasa();

                try
                {
                    string sImpuestoTras = navImpuestosTras.Current.SelectSingleNode("@impuesto", nsmComprobante).Value;
                    if (!String.IsNullOrEmpty(sImpuestoTras))
                    {
                        switch (sImpuestoTras.ToUpper())
                        {
                            case "IVA": impTras.impuesto = ComprobanteImpuestosTrasladoImpuestoTMasa.IVA; break;
                            case "IEPS": impTras.impuesto = ComprobanteImpuestosTrasladoImpuestoTMasa.IEPS; break;
                        }
                    }

                    impTras.importe = Convert.ToDecimal(navImpuestosTras.Current.SelectSingleNode("@importe", nsmComprobante).Value);

                    impTras.tasa = Convert.ToDecimal(navImpuestosTras.Current.SelectSingleNode("@tasa", nsmComprobante).Value);

                    impuestosTraslado.Add(impTras);
                }
                catch { }


            }

            if (impuestosTraslado != null && impuestosTraslado.Count > 0)
            {
                TrasladosT = impuestosTraslado;
            }

            //Información fiscal tercero
            try
            {
                t_UbicacionFiscalTMasa ubicacionTerceros = new t_UbicacionFiscalTMasa();

                ubicacionTerceros.calle = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@calle", nsmComprobante).Value;
                try { ubicacionTerceros.noExterior = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@noExterior", nsmComprobante).Value; }
                catch { }
                try { ubicacionTerceros.noInterior = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@noInterior", nsmComprobante).Value; }
                catch { }
                try { ubicacionTerceros.colonia = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@colonia", nsmComprobante).Value; }
                catch { }
                try { ubicacionTerceros.localidad = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@localidad", nsmComprobante).Value; }
                catch { }
                try { ubicacionTerceros.referencia = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@referencia", nsmComprobante).Value; }
                catch { }
                ubicacionTerceros.municipio = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@municipio", nsmComprobante).Value;
                ubicacionTerceros.estado = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@estado", nsmComprobante).Value;
                ubicacionTerceros.pais = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@pais", nsmComprobante).Value;
                ubicacionTerceros.codigoPostal = navDetalle.SelectSingleNode("terceros:InformacionFiscalTercero/@codigoPostal", nsmComprobante).Value;

                UbicacionFiscalT = ubicacionTerceros;
            }
            catch { }

            //Información aduanera terceros
            try
            {
                t_InformacionAduaneraTMasa aduaneraTerceros = new t_InformacionAduaneraTMasa();

                aduaneraTerceros.numero = navDetalle.SelectSingleNode("terceros:InformacionAduanera/@numero", nsmComprobante).Value;
                aduaneraTerceros.fecha = Convert.ToDateTime(navDetalle.SelectSingleNode("terceros:InformacionAduanera/@fecha", nsmComprobante).Value);
                try { aduaneraTerceros.aduana = navDetalle.SelectSingleNode("terceros:InformacionAduanera/@aduana", nsmComprobante).Value; }
                catch { }

                InformacionAduaneraT = aduaneraTerceros;
            }
            catch { }

            //Cuenta predial terceros
            try { NumeroPredialT = navDetalle.SelectSingleNode("terceros:CuentaPredial/@numero", nsmComprobante).Value; }
            catch { }

        }
        catch { }

        #endregion
    }
}

#region clases auxiliares para el complemento de terceros en la clase Detalle
public partial class ComprobanteImpuestosRetencionTMasa
{
    private ComprobanteImpuestosRetencionImpuestoTMasa impuestoField;
    private decimal importeField;

    public ComprobanteImpuestosRetencionImpuestoTMasa impuesto
    {
        get { return this.impuestoField; }
        set { this.impuestoField = value; }
    }

    public decimal importe
    {
        get { return this.importeField; }
        set { this.importeField = value; }
    }
}

public enum ComprobanteImpuestosRetencionImpuestoTMasa
{
    ISR, IVA,
}

public partial class ComprobanteImpuestosTrasladoTMasa
{
    private ComprobanteImpuestosTrasladoImpuestoTMasa impuestoField;
    private decimal tasaField;
    private decimal importeField;

    public ComprobanteImpuestosTrasladoImpuestoTMasa impuesto
    {
        get { return this.impuestoField; }
        set { this.impuestoField = value; }
    }


    public decimal tasa
    {
        get { return this.tasaField; }
        set { this.tasaField = value; }
    }

    public decimal importe
    {
        get { return this.importeField; }
        set { this.importeField = value; }
    }
}

public enum ComprobanteImpuestosTrasladoImpuestoTMasa
{
    IVA, IEPS,
}

public partial class t_UbicacionFiscalTMasa
{
    private string calleField;
    private string noExteriorField;
    private string noInteriorField;
    private string coloniaField;
    private string localidadField;
    private string referenciaField;
    private string municipioField;
    private string estadoField;
    private string paisField;
    private string codigoPostalField;

    public string calle
    {
        get { return this.calleField; }
        set { this.calleField = value; }
    }

    public string noExterior
    {
        get { return this.noExteriorField; }
        set { this.noExteriorField = value; }
    }

    public string noInterior
    {
        get { return this.noInteriorField; }
        set { this.noInteriorField = value; }
    }

    public string colonia
    {
        get { return this.coloniaField; }
        set { this.coloniaField = value; }
    }

    public string localidad
    {
        get { return this.localidadField; }
        set { this.localidadField = value; }
    }

    public string referencia
    {
        get { return this.referenciaField; }
        set { this.referenciaField = value; }
    }

    public string municipio
    {
        get { return this.municipioField; }
        set { this.municipioField = value; }
    }

    public string estado
    {
        get { return this.estadoField; }
        set { this.estadoField = value; }
    }

    public string pais
    {
        get { return this.paisField; }
        set { this.paisField = value; }
    }

    public string codigoPostal
    {
        get { return this.codigoPostalField; }
        set { this.codigoPostalField = value; }
    }
}

public partial class t_InformacionAduaneraTMasa
{

    private string numeroField;
    private System.DateTime fechaField;
    private string aduanaField;

    public string numero
    {
        get { return this.numeroField; }
        set { this.numeroField = value; }
    }

    public System.DateTime fecha
    {
        get { return this.fechaField; }
        set { this.fechaField = value; }
    }

    public string aduana
    {
        get { return this.aduanaField; }
        set { this.aduanaField = value; }
    }
}
#endregion


/// <summary>
/// Funciones de ayuda comunes a todas las demás clases
/// </summary>
public class TransformacionMasa
{

    /// <summary>
    /// Transforma un valor a su representación porcentual
    /// </summary>
    /// <param name="valor">cadena con el valor a ser transformado</param>
    /// <returns>Cadena con el nuevo valor porcentual</returns>
    public static string fnFormatoPorcentaje(string valor)
    {
        CultureInfo languaje;
        languaje = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString());

        try
        {
            //return string.Format("{0:F2}%", Convert.ToDouble(valor));
            return Convert.ToDouble(valor, languaje).ToString("F2", languaje) + "%";
        }
        catch
        {
            return valor;
        }
    }

    /// <summary>
    /// Transforma un valor a su representación de formato monetario
    /// </summary>
    /// <param name="valor">cadena con el valor a ser transformado</param>
    /// <returns>Cadena con el nuevo valor en formato monetario</returns>
    public static string fnFormatoCurrency(string valor)
    {
        CultureInfo languaje;
        languaje = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString());

        //return string.Format("{0:c2}", Convert.ToDouble(valor));
        return Convert.ToDouble(valor, languaje).ToString("c2", languaje);
    }

    /// <summary>
    /// Formatea el valor a dos decimales
    /// </summary>
    /// <param name="valor">cadena con el valor a ser transformado</param>
    /// <returns>Cadena con el nuevo valor en formato monetario</returns>
    public static string fnFormatoRedondeo(string valor)
    {
        CultureInfo languaje;
        languaje = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString());

        //return string.Format("{0:n2}", Convert.ToDouble(valor));
        return Convert.ToDouble(valor, languaje).ToString("n2", languaje);
    }

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFDI
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string psNombreArchivoXSLT)
    {
        string sCadenaOriginal = string.Empty;

        try
        {
            MemoryStream ms = new MemoryStream();
            XslCompiledTransform trans = new XslCompiledTransform();
            trans.Load(XmlReader.Create(new StringReader(clsComunMasa.ObtenerParamentro(psNombreArchivoXSLT))));
            XsltArgumentList args = new XsltArgumentList();
            trans.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            sCadenaOriginal = sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            clsErrorLogMasa.fnNuevaEntrada(ex, clsErrorLogMasa.TipoErroresLog.Datos);
        }

        return sCadenaOriginal;
    }
}

#region ClasesPlantillas
public class clsErrorLogMasa
{
    //private static clsInicioSesionUsuario gUsuario;

    public clsErrorLogMasa()
    {
    }

    /// <summary>
    /// Definición general por grupos de errores para facilitar la comprensión del error
    /// </summary>
    public enum TipoErroresLog
    {
        Datos,          //Para los errores provocados por datos erroneos
        Referencia,     //Para los errores provocados por referencias nulas o fuera de indice
        Conexion,       //Para errores por falla o perdida de cualquier tipo de conexión
        BaseDatos,      //Errores devueltos por SQL
        Email,          //Error de conexion servitor smtp.
        PDF             //Error durante la generación del PDF
    }

    /// <summary>
    /// Registra una nueva entrada de error en la base de datos con información del usuairo y el módulo
    /// </summary>
    /// <param name="pExcepcion">La excepción generada por .NET</param>
    /// <param name="pTipoError">El tipo de error que se generó</param>
    public static void fnNuevaEntrada(Exception pExcepcion, TipoErroresLog pTipoError)
    {
        fnNuevaEntrada(pExcepcion, pTipoError, string.Empty, 0);
    }

    /// <summary>
    /// Registra una nueva entrada de error en la base de datos con información del usuairo y el módulo.
    /// </summary>
    /// <param name="pExcepcion">La excepción generada por .NET</param>
    /// <param name="pTipoError">El tipo de error que se generó</param>
    /// <param name="psObservaciones">Mensaje personal en código definido por el desarrollador para aclarar dudas sobre el error</param>
    /// /// <param name="pnId_Usuario">ID de usuario, si es cero no se toma en cuenta</param>
    public static void fnNuevaEntrada(Exception pExcepcion, TipoErroresLog pTipoError, string psObservaciones, int pnId_Usuario)
    {
        int nId_Usuario = pnId_Usuario;

        //if (pnId_Usuario == 0)
        //    gUsuario = (clsInicioSesionUsuario)System.Web.HttpContext.Current.Session["objUsuario"];

        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conControl"].ConnectionString;
        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadenaCon)))
        {
            con.Open();

            //giSql = clsComun.fnCrearConexion("conControl");
            string[] infoLoc = null;

            try
            {
                //Obtenemos la clase y método de origen a través del StackTrace
                string st = pExcepcion.StackTrace;
                string sub = st.Substring(0, st.IndexOf('('));
                infoLoc = sub.Replace("at", string.Empty).Trim().Split('.');
            }
            catch
            {
                //Si algo falla los definimos como indefinidos
                infoLoc = new string[] { "Indefinido", "Indefinido" };
            }

            try
            {


                using (SqlCommand cmd = new SqlCommand("usp_Ctp_Registrar_Error_Ins", con))
                {


                    cmd.CommandType = System.Data.CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@nId_Usuario", nId_Usuario);
                    cmd.Parameters.AddWithValue("@sMensaje", pExcepcion.Message);
                    cmd.Parameters.AddWithValue("@sModulo", infoLoc[0]);
                    cmd.Parameters.AddWithValue("@sMetodo", infoLoc[1]);
                    cmd.Parameters.AddWithValue("@sTipo_Error", pTipoError.ToString());
                    if (!string.IsNullOrEmpty(psObservaciones))
                        cmd.Parameters.AddWithValue("@sObservaciones", psObservaciones);


                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                //giSql.AgregarParametro("@nId_Usuario", nId_Usuario);
                //giSql.AgregarParametro("@sMensaje", pExcepcion.Message);
                //giSql.AgregarParametro("@sModulo", infoLoc[0]);
                //giSql.AgregarParametro("@sMetodo", infoLoc[1]);
                //giSql.AgregarParametro("@sTipo_Error", pTipoError.ToString());
                //if (!string.IsNullOrEmpty(psObservaciones))
                //    giSql.AgregarParametro("@sObservaciones", psObservaciones);

                //giSql.NoQuery("usp_Ctp_Registrar_Error_Ins", true);
            }
            catch
            {
                //Si falla no se realiza acción alguna
            }
        }
    }
}

public class clsComunMasa
{
    public static string ObtenerParamentro(string sParametro)
    {
        string sRetorno = string.Empty;

        using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings["conControl"].ConnectionString)))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("usp_Ctp_BuscarParametro_Sel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("sParametro", sParametro));

                    con.Open();
                    sRetorno = Convert.ToString(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                con.Close();
                clsErrorLogMasa.fnNuevaEntrada(ex, clsErrorLogMasa.TipoErroresLog.Conexion);
            }
        }
        return sRetorno;
    }
}

public sealed class NumaletMasa
{
    private const int UNI = 0, DIECI = 1, DECENA = 2, CENTENA = 3;
    private static string[,] _matriz = new string[CENTENA + 1, 10] 
            { 
                {null," uno", " dos", " tres", " cuatro", " cinco", " seis", " siete", " ocho", " nueve"}, 
                {" diez"," once"," doce"," trece"," catorce"," quince"," dieciseis"," diecisiete"," dieciocho"," diecinueve"}, 
                {null,null,null," treinta"," cuarenta"," cincuenta"," sesenta"," setenta"," ochenta"," noventa"}, 
                {null,null,null,null,null," quinientos",null," setecientos",null," novecientos"}
            };

    //*********************************************
    //Código agregado por Ivan Lopez - 21 de Abril 2011
    //Esta propiedad permite definir la moneda que será usada
    //Asignando automáticamente el separadorDecimal, cultura y la abreviación de moneda
    //En la parte final de la cadena
    //NOTA: se deja la cultura a la de méxico pues es aquí donde se forma la cadena
    private string _abvMoneda;
    private Moneda _tipoMoneda;
    public Moneda TipoMoneda
    {
        get
        {
            return _tipoMoneda;
        }
        set
        {
            switch (value)
            {
                case Moneda.Peso:
                    _tipoMoneda = value;
                    _abvMoneda = "MXN";
                    _separadorDecimalSalida = "pesos";
                    _cultureInfo = new CultureInfo("es-MX");
                    break;
                case Moneda.Dolar:
                    _tipoMoneda = value;
                    _abvMoneda = "USD";
                    _separadorDecimalSalida = "dólares";
                    _cultureInfo = new CultureInfo("es-MX");
                    break;
                case Moneda.Euro:
                    _tipoMoneda = value;
                    _abvMoneda = "XEU";
                    _separadorDecimalSalida = "euros";
                    _cultureInfo = new CultureInfo("es-MX");
                    break;
            }
        }
    }

    public enum Moneda
    {
        Peso,
        Dolar,
        Euro
    }
    //*****************************************



    #region Miembros estáticos

    private const Char sub = (Char)26;
    //Cambiar acá si se quiere otro comportamiento en los métodos de clase
    public const String SeparadorDecimalSalidaDefault = "con";
    public const String MascaraSalidaDecimalDefault = "00'/100.-'";
    public const Int32 DecimalesDefault = 2;
    public const Boolean LetraCapitalDefault = false;
    public const Boolean ConvertirDecimalesDefault = false;
    public const Boolean ApocoparUnoParteEnteraDefault = false;
    public const Boolean ApocoparUnoParteDecimalDefault = false;

    #endregion

    #region Propiedades de instancia

    private Int32 _decimales = DecimalesDefault;
    private CultureInfo _cultureInfo = CultureInfo.CurrentCulture;
    private String _separadorDecimalSalida = SeparadorDecimalSalidaDefault;
    private Int32 _posiciones = DecimalesDefault;
    private String _mascaraSalidaDecimal, _mascaraSalidaDecimalInterna = MascaraSalidaDecimalDefault;
    private Boolean _esMascaraNumerica = true;
    private Boolean _letraCapital = LetraCapitalDefault;
    private Boolean _convertirDecimales = ConvertirDecimalesDefault;
    private Boolean _apocoparUnoParteEntera = false;
    private Boolean _apocoparUnoParteDecimal;

    /// <summary>
    /// Indica la cantidad de decimales que se pasarán a entero para la conversión
    /// </summary>
    /// <remarks>Esta propiedad cambia al cambiar MascaraDecimal por un valor que empieze con '0'</remarks>
    public Int32 Decimales
    {
        get { return _decimales; }
        set
        {
            if (value > 10) throw new ArgumentException(value.ToString() + " excede el número máximo de decimales admitidos, solo se admiten hasta 10.");
            _decimales = value;
        }
    }

    /// <summary>
    /// Objeto CultureInfo utilizado para convertir las cadenas de entrada en números
    /// </summary>
    public CultureInfo CultureInfo
    {
        get { return _cultureInfo; }
        set { _cultureInfo = value; }
    }

    /// <summary>
    /// Indica la cadena a intercalar entre la parte entera y la decimal del número
    /// </summary>
    public String SeparadorDecimalSalida
    {
        get { return _separadorDecimalSalida; }
        set
        {
            _separadorDecimalSalida = value;
            //Si el separador decimal es compuesto, infiero que estoy cuantificando algo,
            //por lo que apocopo el "uno" convirtiéndolo en "un"
            if (value.Trim().IndexOf(" ") > 0)
                _apocoparUnoParteEntera = true;
            else _apocoparUnoParteEntera = false;
        }
    }

    /// <summary>
    /// Indica el formato que se le dara a la parte decimal del número
    /// </summary>
    public String MascaraSalidaDecimal
    {
        get
        {
            if (!String.IsNullOrEmpty(_mascaraSalidaDecimal))
                return _mascaraSalidaDecimal;
            else return "";
        }
        set
        {
            //determino la cantidad de cifras a redondear a partir de la cantidad de '0' o '#' 
            //que haya al principio de la cadena, y también si es una máscara numérica
            int i = 0;
            while (i < value.Length
                && (value[i] == '0')
                    | value[i] == '#')
                i++;
            _posiciones = i;
            if (i > 0)
            {
                _decimales = i;
                _esMascaraNumerica = true;
            }
            else _esMascaraNumerica = false;
            _mascaraSalidaDecimal = value;
            if (_esMascaraNumerica)
                _mascaraSalidaDecimalInterna = value.Substring(0, _posiciones) + "'"
                    + value.Substring(_posiciones)
                    .Replace("''", sub.ToString())
                    .Replace("'", String.Empty)
                    .Replace(sub.ToString(), "'") + "'";
            else
                _mascaraSalidaDecimalInterna = value
                    .Replace("''", sub.ToString())
                    .Replace("'", String.Empty)
                    .Replace(sub.ToString(), "'");
        }
    }

    /// <summary>
    /// Indica si la primera letra del resultado debe estár en mayúscula
    /// </summary>
    public Boolean LetraCapital
    {
        get { return _letraCapital; }
        set { _letraCapital = value; }
    }

    /// <summary>
    /// Indica si se deben convertir los decimales a su expresión nominal
    /// </summary>
    public Boolean ConvertirDecimales
    {
        get { return _convertirDecimales; }
        set
        {
            _convertirDecimales = value;
            _apocoparUnoParteDecimal = value;
            if (value)
            {// Si la máscara es la default, la borro
                if (_mascaraSalidaDecimal == MascaraSalidaDecimalDefault)
                    MascaraSalidaDecimal = "";
            }
            else if (String.IsNullOrEmpty(_mascaraSalidaDecimal))
                //Si no hay máscara dejo la default
                MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
        }
    }

    /// <summary>
    /// Indica si de debe cambiar "uno" por "un" en las unidades.
    /// </summary>
    public Boolean ApocoparUnoParteEntera
    {
        get { return _apocoparUnoParteEntera; }
        set { _apocoparUnoParteEntera = value; }
    }

    /// <summary>
    /// Determina si se debe apococopar el "uno" en la parte decimal
    /// </summary>
    /// <remarks>El valor de esta propiedad cambia al setear ConvertirDecimales</remarks>
    public Boolean ApocoparUnoParteDecimal
    {
        get { return _apocoparUnoParteDecimal; }
        set { _apocoparUnoParteDecimal = value; }
    }

    #endregion

    #region Constructores

    public NumaletMasa()
    {
        MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
        SeparadorDecimalSalida = SeparadorDecimalSalidaDefault;
        LetraCapital = LetraCapitalDefault;
        ConvertirDecimales = _convertirDecimales;
    }

    public NumaletMasa(Boolean ConvertirDecimales, String MascaraSalidaDecimal, String SeparadorDecimalSalida, Boolean LetraCapital)
    {
        if (!String.IsNullOrEmpty(MascaraSalidaDecimal))
            this.MascaraSalidaDecimal = MascaraSalidaDecimal;
        if (!String.IsNullOrEmpty(SeparadorDecimalSalida))
            _separadorDecimalSalida = SeparadorDecimalSalida;
        _letraCapital = LetraCapital;
        _convertirDecimales = ConvertirDecimales;
    }
    #endregion

    #region Conversores de instancia

    public String ToCustomString(Double Numero)
    {
        return Convertir((Decimal)Numero,
            _decimales, _separadorDecimalSalida,
            _mascaraSalidaDecimalInterna,
            _esMascaraNumerica,
            _letraCapital,
            _convertirDecimales,
            _apocoparUnoParteEntera,
            _apocoparUnoParteDecimal,
            _abvMoneda);
    }

    public String ToCustomString(String Numero)
    {
        Double dNumero;
        if (Double.TryParse(Numero, NumberStyles.Float, _cultureInfo, out dNumero))
            return ToCustomString(dNumero);
        else throw new ArgumentException("'" + Numero + "' no es un número válido.");
    }

    public String ToCustomString(Decimal Numero)
    { return ToString(Convert.ToDouble(Numero)); }

    public String ToCustomString(Int32 Numero)
    { return Convertir((Decimal)Numero, 0, _separadorDecimalSalida, _mascaraSalidaDecimalInterna, _esMascaraNumerica, _letraCapital, _convertirDecimales, _apocoparUnoParteEntera, false); }


    #endregion

    #region Conversores estáticos

    public static String ToString(Int32 Numero)
    {
        return Convertir((Decimal)Numero, 0, null, null, true, LetraCapitalDefault, ConvertirDecimalesDefault, ApocoparUnoParteEnteraDefault, ApocoparUnoParteDecimalDefault);
    }

    public static String ToString(Double Numero)
    { return Convertir((Decimal)Numero, DecimalesDefault, SeparadorDecimalSalidaDefault, MascaraSalidaDecimalDefault, true, LetraCapitalDefault, ConvertirDecimalesDefault, ApocoparUnoParteEnteraDefault, ApocoparUnoParteDecimalDefault); }

    public static String ToString(String Numero, CultureInfo ReferenciaCultural)
    {
        Double dNumero;
        if (Double.TryParse(Numero, NumberStyles.Float, ReferenciaCultural, out dNumero))
            return ToString(dNumero);
        else throw new ArgumentException("'" + Numero + "' no es un número válido.");
    }

    public static String ToString(String Numero)
    {
        return NumaletMasa.ToString(Numero, CultureInfo.CurrentCulture);
    }

    public static String ToString(Decimal Numero)
    { return ToString(Convert.ToDouble(Numero)); }

    #endregion

    //Sobrecargar hecha por Ivan Lopez para no tener que modificar todas los métodos
    private static String Convertir(Decimal Numero, Int32 Decimales, String SeparadorDecimalSalida, String MascaraSalidaDecimal, Boolean EsMascaraNumerica, Boolean LetraCapital, Boolean ConvertirDecimales, Boolean ApocoparUnoParteEntera, Boolean ApocoparUnoParteDecimal)
    {
        return Convertir(Numero, Decimales, SeparadorDecimalSalida, MascaraSalidaDecimal, EsMascaraNumerica, LetraCapital, ConvertirDecimales, ApocoparUnoParteEntera, ApocoparUnoParteDecimal, string.Empty);
    }

    private static String Convertir(Decimal Numero, Int32 Decimales, String SeparadorDecimalSalida, String MascaraSalidaDecimal, Boolean EsMascaraNumerica, Boolean LetraCapital, Boolean ConvertirDecimales, Boolean ApocoparUnoParteEntera, Boolean ApocoparUnoParteDecimal, string _Moneda)
    {
        Int64 Num;
        Int32 terna, pos, centenaTerna, decenaTerna, unidadTerna, iTerna;
        String numcad, cadTerna;
        StringBuilder Resultado = new StringBuilder();

        Num = (Int64)Math.Abs(Numero);

        if (Num >= 1000000000000 || Num < 0) throw new ArgumentException("El número '" + Numero.ToString() + "' excedió los límites del conversor: [0;1.000.000.000.000)");
        if (Num == 0)
            Resultado.Append(" cero");
        else
        {
            numcad = Num.ToString();
            iTerna = 0;
            pos = numcad.Length;

            do //Se itera por las ternas de atrás para adelante
            {
                iTerna++;
                cadTerna = String.Empty;
                if (pos >= 3)
                    terna = Int32.Parse(numcad.Substring(pos - 3, 3));
                else
                    terna = Int32.Parse(numcad.Substring(0, pos));

                centenaTerna = (Int32)(terna / 100);
                decenaTerna = terna - centenaTerna * 100;
                unidadTerna = (decenaTerna - (Int32)(decenaTerna / 10) * 10);

                if ((decenaTerna > 0) && (decenaTerna < 10))
                    cadTerna = _matriz[UNI, unidadTerna] + cadTerna;
                else if ((decenaTerna >= 10) && (decenaTerna < 20))
                    cadTerna = cadTerna + _matriz[DIECI, decenaTerna - (Int32)(decenaTerna / 10) * 10];
                else if (decenaTerna == 20)
                    cadTerna = cadTerna + " veinte";
                else if ((decenaTerna > 20) && (decenaTerna < 30))
                    cadTerna = " veinti" + _matriz[UNI, unidadTerna].Substring(1, _matriz[UNI, unidadTerna].Length - 1);
                else if ((decenaTerna >= 30) && (decenaTerna < 100))
                    if (unidadTerna != 0)
                        cadTerna = _matriz[DECENA, (Int32)(decenaTerna / 10)] + " y" + _matriz[UNI, unidadTerna] + cadTerna;
                    else
                        cadTerna += _matriz[DECENA, (Int32)(decenaTerna / 10)];

                switch (centenaTerna)
                {
                    case 1:
                        if (decenaTerna > 0) cadTerna = " ciento" + cadTerna;
                        else cadTerna = " cien" + cadTerna;
                        break;
                    case 5:
                    case 7:
                    case 9:
                        cadTerna = _matriz[CENTENA, (Int32)(terna / 100)] + cadTerna;
                        break;
                    default:
                        if ((Int32)(terna / 100) > 1) cadTerna = _matriz[UNI, (Int32)(terna / 100)] + "cientos" + cadTerna;
                        break;
                }
                //Reemplazo el 'uno' por 'un' si no es en las únidades o si se solicító apocopar
                if ((iTerna > 1 | ApocoparUnoParteEntera) && decenaTerna == 21)
                    cadTerna = cadTerna.Replace("veintiuno", "veintiún");
                else if ((iTerna > 1 | ApocoparUnoParteEntera) && unidadTerna == 1 && decenaTerna != 11)
                    cadTerna = cadTerna.Substring(0, cadTerna.Length - 1);
                //Acentúo 'dieciseís', 'veintidós', 'veintitrés' y 'veintiséis'
                else if (decenaTerna == 16) cadTerna = cadTerna.Replace("dieciseis", "dieciséis");
                else if (decenaTerna == 22) cadTerna = cadTerna.Replace("veintidos", "veintidós");
                else if (decenaTerna == 23) cadTerna = cadTerna.Replace("veintitres", "veintitrés");
                else if (decenaTerna == 26) cadTerna = cadTerna.Replace("veintiseis", "veintiséis");
                //Reemplazo 'uno' por 'un' si no es en las únidades o si se solicító apocopar (si _apocoparUnoParteEntera es verdadero) 

                switch (iTerna)
                {
                    case 3:
                        if (Num < 2000000) cadTerna += " millón";
                        else cadTerna += " millones";
                        break;
                    case 2:
                    case 4:
                        if (terna > 0) cadTerna += " mil";
                        break;
                }
                Resultado.Insert(0, cadTerna);
                pos = pos - 3;
            } while (pos > 0);
        }
        //Se agregan los decimales si corresponde
        if (Decimales > 0)
        {
            Resultado.Append(" " + SeparadorDecimalSalida + " ");
            Int32 EnteroDecimal = (Int32)Math.Round((Double)(Numero - (Int64)Numero) * Math.Pow(10, Decimales), 0);
            //string NumeroS = Numero.ToString();
            //string[] num = NumeroS.Split('.');
            //if (num.Length > 1)
            //{
            //    if (num[1].Length == 1)
            //        num[1] += "0";
            //    Int32 EnteroDec = Convert.ToInt32(num[1].Substring(0, 2));
            //    EnteroDecimal = EnteroDec;
            //}
            //else
            //    EnteroDecimal = 0;
            if (ConvertirDecimales)
            {
                Boolean esMascaraDecimalDefault = MascaraSalidaDecimal == MascaraSalidaDecimalDefault;
                Resultado.Append(Convertir((Decimal)EnteroDecimal, 0, null, null, EsMascaraNumerica, false, false, (ApocoparUnoParteDecimal && !EsMascaraNumerica/*&& !esMascaraDecimalDefault*/), false) + " "
                    + (EsMascaraNumerica ? "" : MascaraSalidaDecimal));
            }
            else
                if (EsMascaraNumerica) Resultado.Append(EnteroDecimal.ToString(MascaraSalidaDecimal));
                else Resultado.Append(EnteroDecimal.ToString() + " " + MascaraSalidaDecimal);
        }

        //Código añadido por Ivan Lopez - 21 de Abril de 2011 - Agregamos la abreviación de moneda al final de la candena
        Resultado.Append(_Moneda);

        //Se pone la primer letra en mayúscula si corresponde y se retorna el resultado
        if (LetraCapital)
            return Resultado[1].ToString().ToUpper() + Resultado.ToString(2, Resultado.Length - 2);
        else
            return Resultado.ToString().Substring(1);
    }
}
#endregion

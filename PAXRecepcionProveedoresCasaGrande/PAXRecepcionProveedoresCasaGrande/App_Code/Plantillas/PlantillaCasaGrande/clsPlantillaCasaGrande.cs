﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Root.Reports;
using ThoughtWorks.QRCode;
using System.Xml;
using System.Xml.XPath;
using ThoughtWorks.QRCode.Codec;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;
using System.Xml.Xsl;
using System.Globalization;
using Utilerias.SQL;
using System.Data.SqlClient;
using System.Security.Cryptography;

/// <summary>
/// Clase encargada de recibir y manipular los datos de un XML (CFD) para formar luego un archivo PDF
/// </summary>
public class clsPlantillaCasaGrande
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

	private FontProp fPropChica;
	private const double tamFuenteChica = 4;

    private FontProp fPropBlanca;
    private FontProp fPropRoja;
    private FontProp fPropNegrita;

	//tamaños en mm
	private const double anchoPagina = 215.9;
	private const double altoPagina = 279.4;

    //tamaños en puntos
	private const double altoEncabezado = 80;
	private double altoPie = 70;
	private const double factorSeparador = 2;
    private const double grosorPen = 0.5;
    private const double radioCurva = 1;

	//Tamaños en puntos
	private const double margenPagina = 20;
	private const double anchoSeccion = anchoPagina - margenPagina * 2;
	private const double tamCodigo = 90;
	private const string leyendaPDF = "Este documento es una representación impresa de un CFD";

    #endregion

    /// <summary>
    /// Crea una nueva instancia de la clase con un documento XML de CFD
    /// </summary>
    /// <param name="pxComprobante"></param>
    public clsPlantillaCasaGrande(XmlDocument pxComprobante)
	{
		gxComprobante = pxComprobante;
        XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
        gxComprobante.InsertBefore(xDec, gxComprobante.DocumentElement);
        //TipoDocumento = "FACTURA";
	}

    /// <summary>
    /// Genera un nuevo PDF mediante un objeto Report cargado con los datos del XML
    /// </summary>
	public void fnGenerarPDF(int nIdComprobante)
	{
        TipoDocumento = "Factura";
		Formatter formato = new PdfFormatter();
		PDF = new Report(formato);

        //Datos del documento
        PDF.sTitle = "CFD";
        PDF.sAuthor = "CORPUS Facturación";

		//Letra titulo
        fuenteTitulo = new FontDef(PDF, FontDef.StandardFont.Helvetica);
		fPropTitulo = new FontProp(fuenteTitulo, tamFuenteTitulo);
		fPropTitulo.bBold = true;

		//letra normal
        //fuenteNormal = new FontDef(PDF, FontDef.StandardFont.TimesRoman);
		fPropNormal = new FontProp(fuenteTitulo, tamFuenteNormal);
        fPropChica = new FontProp(fuenteTitulo, tamFuenteChica);

        fPropBlanca = new FontProp(fuenteTitulo, tamFuenteNormal, Color.White);
        fPropBlanca.bBold = true;
        fPropRoja = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Red);

        fPropNegrita = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;

        //Obtenemos los detalles para contar
        List<DetalleCasaGrande> detallesNum = fnObtenerDetalles();

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
		List<DetalleCasaGrande> detalles = fnObtenerDetalles();

		bool bSeguir = true;

		while (bSeguir)
		{
			//Tamaño carta
			Page pagina = new Page(PDF);
			pagina.rWidthMM = anchoPagina;
			pagina.rHeightMM = altoPagina;

            System.Drawing.ColorConverter colConvert = new ColorConverter();
            System.Drawing.Color ColorT = new System.Drawing.Color();

            ColorT = Color.Silver;

            StaticContainer Encabezado = fnCrearEncabezado(ColorT);
            StaticContainer Pie = fnCrearPie(pagina.iPageNo, nTotPag);

            //Agregamos el encabezado y pie a la nueva página
            pagina.Add(margenPagina, margenPagina, Encabezado);

            //Marco codigo de barras Bidimensional

            //fnCrearPanelRedondeado(Pie, 0, 0, tamCodigo, tamCodigo, grosorPen, radioCurva, false, ColorT);
            
            //if (pagina.iPageNo == nTotPag)//Si es ultima pagina
            //pagina.Add(margenPagina + 5, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo - 5, GenerarCodigoBidimensional());

			pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
			pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " de " + nTotPag));

            MemoryStream ms = new MemoryStream();
            ms = fnImagenCliente(nIdComprobante);

			//Creamos el área de detalle
            if (ms.Length > 0)
            {
                RepImage image = new RepImage(ms, 240, 65);
                pagina.Add(margenPagina, margenPagina + 75, image);
            }

            pagina.Add(margenPagina, margenPagina + 755, fnImagenPAX());
            pagina.Add(margenPagina, margenPagina + Encabezado.rHeight+6, fnCrearDetalle(detalles, ColorT));

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
    private StaticContainer fnContarPaginas(List<DetalleCasaGrande> detalles)
    {
        StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);

        DetalleCasaGrande[] copiaDetalles = detalles.ToArray();
        DetalleCasaGrande d;
        double posRenglon;
        double altoRenglon = fPropNormal.rSize * factorSeparador;
        int renglonActual = 1;
        //int     contRenActual = 1;
        int rengTotal = 0;
        //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
        int maxRenglones = (int)(areaDetalle.rHeight / (fPropNormal.rSize + 2) - altoRenglon);
        //Definimos el número de conceptos que se agregarán en el for
        int maxConceptos = (maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones);

        if (maxConceptos > 22)
            maxConceptos = 22;

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
                            foreach (ComprobanteImpuestosRetencionTCasaGrande retencion in d.RetencionesT)
                            {
                                switch (retencion.impuesto)
                                {
                                    case ComprobanteImpuestosRetencionImpuestoTCasaGrande.IVA: sDetalleTerceros.Append("IVA Ret: "); break;
                                    case ComprobanteImpuestosRetencionImpuestoTCasaGrande.ISR: sDetalleTerceros.Append("ISR: "); break;
                                }
                                sDetalleTerceros.Append(Transformacion.fnFormatoCurrency(retencion.importe.ToString()) + " ");
                            }
                        }

                        if (d.TrasladosT != null && d.TrasladosT.Count > 0)
                        {
                            foreach (ComprobanteImpuestosTrasladoTCasaGrande traslado in d.TrasladosT)
                            {
                                switch (traslado.impuesto)
                                {
                                    case ComprobanteImpuestosTrasladoImpuestoTCasaGrande.IVA: sDetalleTerceros.Append("IVA Tras "); break;
                                    case ComprobanteImpuestosTrasladoImpuestoTCasaGrande.IEPS: sDetalleTerceros.Append("IEPS "); break;
                                }
                                sDetalleTerceros.Append(Transformacion.fnFormatoPorcentaje(traslado.tasa.ToString()) + ": ");
                                sDetalleTerceros.Append(Transformacion.fnFormatoCurrency(traslado.importe.ToString()) + " ");
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

    public void fnGenerarPDFSave(int nIdComprobante,string ruta)
    {
        TipoDocumento = "Factura";
        Formatter formato = new PdfFormatter();
        PDF = new Report(formato);

        //Datos del documento
        PDF.sTitle = "CFD";
        PDF.sAuthor = "CORPUS Facturación";

        //Letra titulo
        fuenteTitulo = new FontDef(PDF, FontDef.StandardFont.Helvetica);
        fPropTitulo = new FontProp(fuenteTitulo, tamFuenteTitulo);
        fPropTitulo.bBold = true;

        //letra normal
        //fuenteNormal = new FontDef(PDF, FontDef.StandardFont.Helvetica);
        fPropNormal = new FontProp(fuenteTitulo, tamFuenteNormal);
        fPropChica = new FontProp(fuenteTitulo, tamFuenteChica);

        fPropBlanca = new FontProp(fuenteTitulo, tamFuenteNormal, Color.White);
        fPropBlanca.bBold = true;
        fPropRoja = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Red);

        fPropNegrita = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;

        //Obtenemos los detalles para contar
        List<DetalleCasaGrande> detallesNum = fnObtenerDetalles();

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
        List<DetalleCasaGrande> detalles = fnObtenerDetalles();

        bool bSeguir = true;

        while (bSeguir)
        {
            //Tamaño carta
            Page pagina = new Page(PDF);
            pagina.rWidthMM = anchoPagina;
            pagina.rHeightMM = altoPagina;
            
            System.Drawing.ColorConverter colConvert = new ColorConverter();
            System.Drawing.Color ColorT = new System.Drawing.Color();

            ColorT = Color.Silver ;


            StaticContainer Encabezado = fnCrearEncabezado(ColorT);
            StaticContainer Pie = fnCrearPie(pagina.iPageNo, nTotPag);

            //Agregamos el encabezado y pie a la nueva página
            pagina.Add(margenPagina, margenPagina, Encabezado);

            //Marco codigo de barras Bidimensional

            //fnCrearPanelRedondeado(Pie, 0, 0, tamCodigo, tamCodigo, grosorPen, radioCurva, false, ColorT);
            //if (pagina.iPageNo == nTotPag)//Si es ultima pagina
            //pagina.Add(margenPagina + 5, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo - 5, GenerarCodigoBidimensional());

            pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
            pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " de " + nTotPag));

            MemoryStream ms = new MemoryStream();
            ms = fnImagenCliente(nIdComprobante);

            //Creamos el área de detalle
            if (ms.Length > 0)
            {
                RepImage image = new RepImage(ms, 240, 65);
                pagina.Add(margenPagina + 12, margenPagina + 60, image);
            }

            pagina.Add(margenPagina, margenPagina + 755, fnImagenPAX());
            pagina.Add(margenPagina, margenPagina + Encabezado.rHeight+6, fnCrearDetalle(detalles, ColorT));

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
	private StaticContainer fnCrearDetalle(List<DetalleCasaGrande> detalles, System.Drawing.Color sColor)
	{
		StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);

        //fnAgregarBordeRedondeado(areaDetalle, grosorPen, radioCurva, sColor);

		DetalleCasaGrande[] copiaDetalles = detalles.ToArray();
		DetalleCasaGrande d;
		double  posRenglon;
		double  altoRenglon = fPropNormal.rSize * factorSeparador;
		int  renglonActual = 1;
        //int     contRenActual = 1;
        int rengTotal = 0;

		//calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
		int     maxRenglones = (int)((areaDetalle.rHeight / (fPropNormal.rSize + 2)) - altoRenglon);
		//Definimos el número de conceptos que se agregarán en el for
		int     maxConceptos = maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones;

        if (maxConceptos > 22)
            maxConceptos = 22;

		//definimos las posiciones
		double posColumna1 = areaDetalle.rWidth * 0.01;
		double posColumna2 = areaDetalle.rWidth * 0.1;
        double posColumna3 = areaDetalle.rWidth * 0.2; //0.2;
		double posColumna4 = areaDetalle.rWidth * 0.77;
		double posColumna5 = areaDetalle.rWidth * 0.96;
        
		//double posColumna6 = areaDetalle.rWidth * 0.99;
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
                            foreach (ComprobanteImpuestosRetencionTCasaGrande retencion in d.RetencionesT)
                            {
                                switch (retencion.impuesto)
                                {
                                    case ComprobanteImpuestosRetencionImpuestoTCasaGrande.IVA: sDetalleTerceros.Append("IVA Ret: "); break;
                                    case ComprobanteImpuestosRetencionImpuestoTCasaGrande.ISR: sDetalleTerceros.Append("ISR: "); break;
                                }
                                sDetalleTerceros.Append(Transformacion.fnFormatoCurrency(retencion.importe.ToString()) + " ");
                            }
                        }

                        if (d.TrasladosT != null && d.TrasladosT.Count > 0)
                        {
                            foreach (ComprobanteImpuestosTrasladoTCasaGrande traslado in d.TrasladosT)
                            {
                                switch (traslado.impuesto)
                                {
                                    case ComprobanteImpuestosTrasladoImpuestoTCasaGrande.IVA: sDetalleTerceros.Append("IVA Tras "); break;
                                    case ComprobanteImpuestosTrasladoImpuestoTCasaGrande.IEPS: sDetalleTerceros.Append("IEPS "); break;
                                }
                                sDetalleTerceros.Append(Transformacion.fnFormatoPorcentaje(traslado.tasa.ToString()) + ": ");
                                sDetalleTerceros.Append(Transformacion.fnFormatoCurrency(traslado.importe.ToString()) + " ");
                            }
                        }
                    }

                    Col3 = fnAgregarMultilineaDetalle(areaDetalle, sDetalleTerceros.ToString(), fPropNormal, posColumna3, posRenglon, 42, true);
                }  

            }
            else
            {
                Col1 = fnAgregarMultilinea(areaDetalle, (i+1).ToString("0000"), fPropNormal, posColumna1, posRenglon, 9, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.cantidad, fPropNormal, posColumna2, posRenglon, 9, true);
                areaDetalle.AddAligned(posColumna4, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.valorUnitario));
                areaDetalle.AddAligned(posColumna5, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.importe));
                //areaDetalle.AddAligned(posColumna6, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.importe));
                //Acomodamos la descripción en tantos renglones como necesite           
                Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon, 42, true);
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
	private List<DetalleCasaGrande> fnObtenerDetalles()
	{
		XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
		nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        string cfd = string.Empty;
        string version = string.Empty;
        
		List<DetalleCasaGrande> detalles = new List<DetalleCasaGrande>();

		XPathNavigator navComprobante = gxComprobante.CreateNavigator();
        try { version = navComprobante.SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value; }
        catch {
            version = navComprobante.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
        }
        cfd = version.StartsWith("2") ? "cfd" : "cfdi";
		XPathNodeIterator navDetalles = navComprobante.Select("/"+cfd+":Comprobante/"+cfd+":Conceptos/"+cfd+":Concepto", nsmComprobante);
        
		while (navDetalles.MoveNext())
		{
            detalles.Add(new DetalleCasaGrande(navDetalles.Current, nsmComprobante));
            XPathNavigator nodenavigator = navDetalles.Current;
            if (nodenavigator.HasChildren)//Si contiene nodo hijo
            {
                ////Desplaza al primero nodo hijo
                //nodenavigator.MoveToFirstChild();
                ////Obtiene la informacion del nodo
                //XPathNodeIterator navDetallesAduana = nodenavigator.Select("/cfd:Comprobante/cfd:Conceptos/cfd:Concepto/cfd:InformacionAduanera", nsmComprobante);
                //if (navDetallesAduana.Count > 0)//Si contiene Informacion Aduanal se agrega al concepto
                //    detalles.Add(new DetalleLogo(navDetallesAduana.Current, nsmComprobante));
                //Obtiene la informacion del nodo aduana si existe
                XPathNavigator navDetallesAduana = nodenavigator.SelectSingleNode("InformacionAduanera", nsmComprobante);
                if (navDetallesAduana != null)
                    detalles.Add(new DetalleCasaGrande(navDetallesAduana, nsmComprobante));

                //Obtiene información del nodo complemento concepto si existe
                XPathNavigator navComplTerceros = nodenavigator.SelectSingleNode("ComplementoConcepto", nsmComprobante);
                if (navComplTerceros != null)
                {
                    XmlNamespaceManager nsmComprobanteComplTerceros = new XmlNamespaceManager(gxComprobante.NameTable);
                    nsmComprobanteComplTerceros.AddNamespace("terceros", "http://www.sat.gob.mx/terceros");

                    //Obtiene información del nodo cuenta de terceros si existe
                    XPathNavigator navTerceros = navComplTerceros.SelectSingleNode("terceros:PorCuentadeTerceros", nsmComprobanteComplTerceros);

                    if (navTerceros != null)
                        detalles.Add(new DetalleCasaGrande(navTerceros, nsmComprobanteComplTerceros));
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
	private StaticContainer fnCrearPie(int nNumPag, int nTotPag)
	{
		StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));

		XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
		nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
		nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");

		XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

        fnTotales(nsmComprobante, navEncabezado, Pie, nNumPag, nTotPag);

		return Pie;
	}

	/// <summary>
	/// Crea un nuevo panel con la información del pie de página del comprobante
	/// </summary>
	/// <param name="nsmComprobante">Manejador de nombre de espacios</param>
	/// <param name="navPie">Navegador con los datos del pie de página</param>
	/// <param name="Pie">Contenedor en el cual se desplegarán los datos del pie de página</param>
	private void fnTotales(XmlNamespaceManager nsmComprobante, XPathNavigator navPie, StaticContainer Pie, int nNumPag, int nTotPag)
	{
          System.Drawing.ColorConverter colConvert = new ColorConverter();
        System.Drawing.Color ColorT = new System.Drawing.Color();

        ColorT = Color.Silver;

        string subtotal, total, moneda, sello,  formaDePago, Regimenfiscal, version, cadenaOriginal, cfd, iva, ish;
        subtotal = total = moneda = sello =  formaDePago  = Regimenfiscal = version = cadenaOriginal = cfd = iva = ish = string.Empty;
        try { version = navPie.SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value; }
        catch
        {
            version = navPie.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
        }
        cfd = version.StartsWith("2") ? "cfd" : "cfdi";
		List<ImpuestoCasaGrande> impuestos = new List<ImpuestoCasaGrande>();
        //List<ImpuestoComp> impuestosComp = new List<ImpuestoComp>();

		subtotal = navPie.SelectSingleNode("/"+cfd+":Comprobante/@subTotal", nsmComprobante).Value;
		total = navPie.SelectSingleNode("/"+cfd+":Comprobante/@total", nsmComprobante).Value;
		formaDePago = navPie.SelectSingleNode("/"+cfd+":Comprobante/@formaDePago", nsmComprobante).Value;
		sello = navPie.SelectSingleNode("/"+cfd+":Comprobante/@sello", nsmComprobante).Value;
        version = navPie.SelectSingleNode("/"+cfd+":Comprobante/@version", nsmComprobante).Value;
		try { moneda = navPie.SelectSingleNode("/"+cfd+":Comprobante/@Moneda", nsmComprobante).Value; }
		catch { }

        

        try { Regimenfiscal = navPie.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":RegimenFiscal/@Regimen", nsmComprobante).Value; }
        catch { }
        try { iva = navPie.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Impuestos/"+cfd+":Traslados/"+cfd+":Traslado/@importe", nsmComprobante).Value; }
        catch { iva = "0"; }
        try { ish = navPie.SelectSingleNode("/" + cfd + ":Comprobante/" + cfd + ":Impuestos/" + cfd + ":Retenciones/" + cfd + ":Retencion/@importe", nsmComprobante).Value; }
        catch { ish = "0"; }
        //XPathNodeIterator navImpuestos = navPie.Select("/"+cfd+":Comprobante/"+cfd+":Impuestos/"+cfd+":Traslados/"+cfd+":Traslado", nsmComprobante);
        //if (nNumPag == nTotPag)//Si es ultima pagina
        //{
        //    while (navImpuestos.MoveNext())
        //    {
        //        impuestos.Add(new ImpuestoCasaGrande(navImpuestos.Current, nsmComprobante));
        //    }

        //    navImpuestos = navPie.Select("/"+cfd+":Comprobante/"+cfd+":Impuestos/"+cfd+":Retenciones/"+cfd+":Retencion", nsmComprobante);
        //    while (navImpuestos.MoveNext())
        //    {
        //        impuestos.Add(new ImpuestoCasaGrande(navImpuestos.Current, nsmComprobante));
        //    }
        //}

        //Complementos impuestos locales
        //try
        //{
        //    XPathNodeIterator navComplemento = navPie.Select("/cfd:Comprobante/cfd:Complemento/implocal:ImpuestosLocales/implocal:TrasladosLocales", nsmComprobante);

        //    while (navComplemento.MoveNext())
        //    {
        //        impuestosComp.Add(new ImpuestoComp(navComplemento.Current, nsmComprobante));
        //    }

        //    navComplemento = navPie.Select("/cfd:Comprobante/cfd:Complemento/implocal:ImpuestosLocales/implocal:RetencionesLocales", nsmComprobante);

        //    while (navComplemento.MoveNext())
        //    {
        //        impuestosComp.Add(new ImpuestoComp(navComplemento.Current, nsmComprobante));
        //    }
        //}
        //catch { }

        double verPadding = Pie.rHeight * 0.02;
        double horPadding = Pie.rWidth * 0.01;
        double posPanelTotales = Pie.rWidth - 180;
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding, RepObj.rAlignTop, new RepString(fPropNormal, "SUBTOTAL: "));
            Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionCasaGrande.fnFormatoCurrency(subtotal)));
        }
        double altoRenglon = fPropNormal.rSize + verPadding;
        double altoSello = altoRenglon;
        int renglon = 1;
        string textoRenglon = string.Empty;
        fnAgregarMultilinea(Pie, "**"+formaDePago.ToUpper()+"**", fPropNegrita, 200, altoRenglon-10 , 80, true);
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            //foreach (ImpuestoCasaGrande i in impuestos)
            //{
            //    if (i.Nombre != "IEPS")
            //    {
            //        Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + ": " + TransformacionCasaGrande.fnFormatoPorcentaje(i.Tasa)));
            //        Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionCasaGrande.fnFormatoCurrency(i.Importe)));
            //        renglon++;
            //    }
            //}

            Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, "ISH: " ));
            Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionCasaGrande.fnFormatoCurrency(ish)));
            renglon++; 
            Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal,  "IVA: " ));
            Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionCasaGrande.fnFormatoCurrency(iva)));
            renglon++;
            //try
            //{
            //    //Agrega complemento impuestos locales 
            //    foreach (ImpuestoComp i in impuestosComp)
            //    {
            //        Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + Transformacion.fnFormatoPorcentaje(i.Tasa)));
            //        Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, Transformacion.fnFormatoCurrency(i.Importe)));
            //        renglon++;
            //    }
            //}
            //catch { }


            //Agregamos el separador y el total
      

        Pie.Add(posPanelTotales + horPadding, tamCodigo - altoRenglon - verPadding, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));

            Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, tamCodigo - altoRenglon, RepObj.rAlignTop, new RepString(fPropNormal, "TOTAL FACTURA: "));
            Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, tamCodigo - altoRenglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionCasaGrande.fnFormatoCurrency(total)));
        }
        //Finalmente creamos el panel de los totales

        fnCrearPanel( Pie.rWidth - posPanelTotales, tamCodigo, ColorT);
        //if (nNumPag == nTotPag)//Si es ultima pagina
        //{
        //    //Mostramos forma de pago y texto importe en su panel correspondiente 
        //    //Pie.Add(tamCodigo + horPadding, altoRenglon, new RepString(fPropNegrita, "Total con letra:"));
        //    //fnAgregarMultilinea(Pie, fnTextoImporte(TransformacionCasaGrande.fnFormatoRedondeo(total), moneda), fPropChica, tamCodigo + horPadding, altoRenglon * 1.7, 80, true);

        //   // fnAgregarMultilinea(Pie, "Forma de Pago:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 3.2, 80, true);
        //    //fnAgregarMultilinea(Pie, formaDePago, fPropChica, tamCodigo + horPadding, altoRenglon * 4, 80, true);

        //    if (version == "3.2") //Si es version 3.2 muestra el siguiente contenido
        //    {
        //        //Regimen Fiscal
        //        //if (!string.IsNullOrEmpty(Regimenfiscal))
        //        //{
        //        //    fnAgregarMultilinea(Pie, "Régimen Fiscal:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 5, 80, true);
        //        //    fnAgregarMultilinea(Pie, Regimenfiscal, fPropChica, tamCodigo + horPadding, altoRenglon * 6, 80, true);
        //        //}

        //        //Metodo de pago 
        //        //if (!string.IsNullOrEmpty(metodoPago))
        //        //{
        //        //    fnAgregarMultilinea(Pie, "Método de Pago:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 5, 80, true);
        //        //    fnAgregarMultilinea(Pie, metodoPago, fPropChica, tamCodigo + horPadding, altoRenglon * 6, 80, true);

        //        //    //Número de cuenta
        //        //    if (!string.IsNullOrEmpty(sNumCtaPago))
        //        //    {
        //        //        fnAgregarMultilinea(Pie, "Número de Cuenta:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 7, 80, true);
        //        //        fnAgregarMultilinea(Pie, sNumCtaPago, fPropChica, tamCodigo + horPadding, altoRenglon * 8, 80, true);
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    //Número de cuenta
        //        //    if (!string.IsNullOrEmpty(sNumCtaPago))
        //        //    {
        //        //        fnAgregarMultilinea(Pie, "Número de Cuenta:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 5, 80, true);
        //        //        fnAgregarMultilinea(Pie, sNumCtaPago, fPropChica, tamCodigo + horPadding, altoRenglon * 6, 80, true);
        //        //    }
        //        //}
        //    }
        //}
        fnCrearPanel( posPanelTotales - tamCodigo, tamCodigo,  ColorT);
        string xsltCadena = string.Empty;
        switch(version)
        {
            case "2.0":
                xsltCadena = "Cadena_original_v2";
                break;
            case "2.2":
                xsltCadena = "Cadena_original_v22";
                break;
            case "3.0":
                xsltCadena = "cadenaoriginal_3_0";
                break;
            case "3.2":
                xsltCadena = "cadenaoriginal_3_2";
                break;
        }
        //Estos datos estan debajo del CBB
        renglon = 1;
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            IXPathNavigable navDocumento = gxComprobante.CreateNavigator();
            Pie.Add(horPadding, altoSello * renglon++, new RepString(fPropNegrita, "Sello digital:"));
            renglon += fnAgregarMultilinea(Pie, sello, fPropChica, horPadding,  altoSello * renglon, 110, false);
            fnCrearPanelRedondeado(Pie, 0,altoSello+3, 390, (altoSello * (renglon+5))-(altoSello * renglon), grosorPen, radioCurva, false, ColorT);
            renglon += 3;
            renglon += fnAgregarMultilinea(Pie, "*** " + fnTextoImporte(TransformacionCasaGrande.fnFormatoRedondeo(total), moneda).ToUpper() + " ***", 
                fPropChica, horPadding, altoSello * renglon, 110, true);
            renglon++;
            //Pie.Add(horPadding, tamCodigo + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del SAT:"));
            //renglon += fnAgregarMultilinea(Pie, timbre, fPropChica, horPadding, tamCodigo + altoRenglon * renglon, 130, false);

            //Agregamos la cadena original y alargamos la sección del pie según sea necesario
            Pie.Add(horPadding, altoSello * renglon++, new RepString(fPropNegrita, "Cadena original:"));
            renglon = fnAgregarMultilinea(Pie, TransformacionCasaGrande.fnConstruirCadenaTimbrado(gxComprobante, xsltCadena),
                 fPropChica, horPadding, altoSello * renglon, 172, false);
            fnCrearPanelRedondeado(Pie, 0, tamCodigo, Pie.rWidth, Pie.rHeight - tamCodigo - altoSello -10, grosorPen, radioCurva, false, ColorT);
            renglon = 1;
            Pie.rHeight += renglon * (fPropChica.rSize * 1.2);
            altoPie = Pie.rHeightMM;
        }
        //dibujamos el borde del pie

        //Agrega www.paxfacturacion.com
        fPropNormal = new FontProp(fuenteTitulo, 4);
        Pie.AddAligned(Pie.rWidth /7 , RepObj.rAlignCenter, Pie.rHeight - verPadding, RepObj.rAlignTop, new RepString(fPropNormal, clsComunPDF.ObtenerParamentro("urlHostComercial")));

        //Agrega leyenda de comprobante
        fPropNormal = new FontProp(fuenteTitulo, tamFuenteNormal);
        Pie.AddAligned(Pie.rWidth / 2, RepObj.rAlignCenter, Pie.rHeight - verPadding, RepObj.rAlignTop, new RepString(fPropNormal, leyendaPDF));
        //Agrega Régimen fiscal
        fPropNormal = new FontProp(fuenteTitulo, tamFuenteNormal);
        Pie.AddAligned(Pie.rWidth / 6, RepObj.rAlignCenter, Pie.rHeight - verPadding - 15, RepObj.rAlignTop, new RepString(fPropNormal, "Régimen Fiscal: " + Regimenfiscal));
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
		StaticContainer Encabezado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoEncabezado));
        //fnCrearPanelRedondeado(Encabezado, 0, 0, Encabezado.rWidth, Encabezado.rHeight - fPropBlanca.rSize * 2, grosorPen, radioCurva, false, sColor);

        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
        nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        try { nsmComprobante.AddNamespace("donat", "http://www.sat.gob.mx/donat"); }
        catch { }
        double leftPadding = Encabezado.rWidth * 0.02;
		XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
        double Renglon = 0;
		
        fnDatosEmisor(nsmComprobante, navEncabezado, Encabezado, sColor, ref Renglon);
       
        fnDatosReceptor(nsmComprobante, navEncabezado, Encabezado, sColor, ref Renglon);
        
        fnDatosFactura(nsmComprobante, navEncabezado, Encabezado, sColor,ref Renglon);
        
       
        
        
        fnDibujarTitulosDetalle(Encabezado, sColor);

		return Encabezado;
	}

	/// <summary>
	/// Dibuja el recuadro de titulos para el área de detalle
	/// </summary>
	/// <param name="Encabezado">Contenedor al que se agregarán los titulos</param>
	private void fnDibujarTitulosDetalle(StaticContainer Encabezado, System.Drawing.Color sColor)
	{
		//Dibujamos el área de los titulos
            double altoBarra =  fPropTitulo.rSize * 2;
            fnCrearPanelRedondeado(Encabezado, 0, Encabezado.rHeight - altoBarra, Encabezado.rWidth, altoBarra, grosorPen, radioCurva, true,sColor);
            fnAgregarMultilinea(Encabezado, "PRODUCTOS Y SERVICIOS", fPropTitulo, 10, Encabezado.rHeight-3 , 40, true);
            //Dibujamos los titulos del detalle
            //El ancho total del área es de 572 puntos
            double puntoMedio = Encabezado.rHeight+2;

            //Definimos la posicion de los titulos
            double posColumna1 = Encabezado.rWidth * 0.01;     //clave
            double posColumna2 = Encabezado.rWidth * 0.1;      //unidad
            double posColumna3 = Encabezado.rWidth * 0.2;     //descripcion
            double posColumna4 = Encabezado.rWidth * 0.65;     //precio unitario
            double posColumna5 = Encabezado.rWidth * 0.90;     //importe
            //double posColumna6 = Encabezado.rWidth * 0.92;     //importe
            
            Encabezado.AddAligned(posColumna1, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignLeft, new RepString(fPropTitulo, "PARTIDA"));
            Encabezado.AddAligned(posColumna2, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignLeft, new RepString(fPropTitulo, "CANTIDAD"));
            Encabezado.AddAligned(posColumna3, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignLeft, new RepString(fPropTitulo, "DESCRIPCIÓN"));
            Encabezado.AddAligned(posColumna4, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignLeft, new RepString(fPropTitulo, "PRECIO UNITARIO"));
            Encabezado.AddAligned(posColumna5, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignLeft, new RepString(fPropTitulo, "IMPORTE"));
            //Encabezado.AddAligned(posColumna6, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "TOTAL"));
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
            referencia, Localidad, version, Regimenfiscal, noAprobacion, anioAprobacion, cfd; 
            //exCalle, exNoInterior, exNoExterior,
            //exColonia, exMunicipio, exEstado, exPais, exCodigoPostal, exLocalidad;
        razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = fecha = serie = folio = noCertificadoEmisor = 
            referencia = Localidad = version = Regimenfiscal =  noAprobacion = anioAprobacion = cfd = string.Empty;
        try { version =  navEncabezado.SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value; }
        catch
        {
            version = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
        }
        cfd = version.StartsWith("2") ? "cfd" : "cfdi";
		try { serie = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@serie", nsmComprobante).Value; }
		catch { }
		try { folio = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@folio", nsmComprobante).Value; }
		catch { }
        try { fecha = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@fecha", nsmComprobante).Value; }
        catch { }
        //try { fechaTimb = navEncabezado.SelectSingleNode("/Comprobante/Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value; }
        //catch { }

        DateTime fechaComprobante = DateTime.MinValue;
        //DateTime fechaTimbrado = DateTime.MinValue;
        if (!string.IsNullOrEmpty(fecha))
            fechaComprobante = Convert.ToDateTime(fecha);

        //if (!string.IsNullOrEmpty(fechaTimb))
        //    fechaTimbrado = Convert.ToDateTime(fechaTimb);

		razonSocial = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/@nombre", nsmComprobante).Value;
		rfc = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/@rfc", nsmComprobante).Value;
		calle = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@calle", nsmComprobante).Value;
		try { noExterior = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@noExterior", nsmComprobante).Value; }
		catch { }
		try { noInterior = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@noInterior", nsmComprobante).Value; }
		catch { }
		try { colonia = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@colonia", nsmComprobante).Value; }
		catch { }
      
        try { referencia = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@referencia", nsmComprobante).Value; }
		catch { }

        try { Localidad = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@localidad", nsmComprobante).Value; }
        catch { }

        version = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@version", nsmComprobante).Value;

        try { Regimenfiscal = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":RegimenFiscal/@Regimen", nsmComprobante).Value; }
        catch { }

        municipio = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@municipio", nsmComprobante).Value;

		pais = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@pais", nsmComprobante).Value;
        estado = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@estado", nsmComprobante).Value;
        //if (version == "3.0")
        //    estadolug = pais + ", " + estado;
        //else
        //{
        //    try { estadolug = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@LugarExpedicion", nsmComprobante).Value; }
        //    catch { }
        //}
		codigoPostal = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":DomicilioFiscal/@codigoPostal", nsmComprobante).Value;
        noCertificadoEmisor = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@noCertificado", nsmComprobante).Value;

        //Si existe complemento donativas se agrega a PDf
        try { noAprobacion = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@noAprobacion", nsmComprobante).Value; }
        catch { }

        try { anioAprobacion = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@anoAprobacion", nsmComprobante).Value; }
        catch { }

        //try { leyenda = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Complemento/donat:Donatarias/@leyenda", nsmComprobante).Value; }
        //catch { }

        //------Expedido En--------
        //try { exCalle = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":ExpedidoEn/@calle", nsmComprobante).Value; }
        //catch { }

        //if (exCalle != string.Empty)
        //{

        //    try { exNoExterior = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Emisor/"+cfd+":ExpedidoEn/@noExterior", nsmComprobante).Value; }
        //    catch { }

        //    try { exNoInterior = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:ExpedidoEn/@noInterior", nsmComprobante).Value; }
        //    catch { }

        //    try { exColonia = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:ExpedidoEn/@colonia", nsmComprobante).Value; }
        //    catch { }

        //    try { exMunicipio = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:ExpedidoEn/@municipio", nsmComprobante).Value; }
        //    catch { }

        //    try { exEstado = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:ExpedidoEn/@estado", nsmComprobante).Value; }
        //    catch { }

        //    try { exPais = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:ExpedidoEn/@pais", nsmComprobante).Value; }
        //    catch { }

        //    try { exCodigoPostal = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:ExpedidoEn/@codigoPostal", nsmComprobante).Value; }
        //    catch { }

        //    try { exLocalidad = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:ExpedidoEn/@localidad", nsmComprobante).Value; }
        //    catch { }
        //}


        //string direccion = string.Empty;
        //string coloniaemisor = string.Empty;
        //string expedidoEn = string.Empty;

        //direccion += calle;
        //if (!string.IsNullOrEmpty(noExterior))
        //    direccion += " No. " + noExterior;
        //if (!string.IsNullOrEmpty(noInterior))
        //    direccion += " Int. " + noInterior;
        //if (!string.IsNullOrEmpty(colonia))
        //    direccion += " Colonia " + colonia; //coloniaemisor += "Colonia " + colonia;

        //string ubicacion = string.Empty;
        //ubicacion += Localidad;
        //if (!string.IsNullOrEmpty(ubicacion))
        //{
        //    ubicacion += ", " + municipio;
        //}
        //else
        //{
        //    ubicacion += municipio;
        //}


        //ubicacion += ", "; //+ estado;
        //ubicacion += ". " + pais;
        //ubicacion += " C.P. " + codigoPostal;
        //string ubicacion2 = string.Empty;
        //ubicacion2 += estado;
        //ubicacion2 += ". " + pais;
        //ubicacion2 += " C.P. " + codigoPostal;

        //Direccion Expedido En 
        //string exDireccion = string.Empty;
        //exDireccion += exCalle;
        //if (!string.IsNullOrEmpty(exNoExterior))
        //    exDireccion += " No. " + exNoExterior;
        //if (!string.IsNullOrEmpty(exNoInterior))
        //    exDireccion += " Int. " + exNoInterior;
        //if (!string.IsNullOrEmpty(exColonia))
        //    exDireccion += " Colonia " + exColonia;

        //string exUbicacion = string.Empty;
        //exUbicacion += exLocalidad;
        //if (!string.IsNullOrEmpty(exUbicacion))
        //{
        //    exUbicacion += ", " + exMunicipio;
        //}
        //else
        //{
        //    exUbicacion += exMunicipio;
        //}

        ////exUbicacion += ", ";//+ exEstado;
        ////exUbicacion += ". " + exPais;
        ////exUbicacion += " C.P. " + exCodigoPostal;

        //string exUbicacion2 = string.Empty;
        //exUbicacion2 += exEstado;
        //exUbicacion2 += ". " +exPais;
        //exUbicacion2 += " C.P. " + exCodigoPostal;

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 5; //10
		double posRazon = fPropTitulo.rSize + sep;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;

        fPropTitulo.rSize = 6;
        fPropNormal.rSize = 5;
        //fPropNormal = new FontProp(fuenteNormal, 4);
        fPropNegrita = new FontProp(fuenteTitulo, 5, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteTitulo, 5, Color.Red);
       
        //Encabezado.Add(leftPadding + 155, posRazon, new RepString(fPropTitulo, razonSocial));
        //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 0.5, new RepString(fPropNormal, direccion));
        //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 1.3, new RepString(fPropNormal, coloniaemisor));
        //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 2.1, new RepString(fPropNormal, ubicacion));
        //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 2.9, new RepString(fPropNormal, ubicacion2));
        //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 3.7, new RepString(fPropNormal, rfc));
        
        //double Renglon = 0;
        double i = 0;
        //i = fnAgregarMultilinea(Encabezado, razonSocial, fPropTitulo, leftPadding + 155, posRenglon, 40, true);
        //Renglon += i;
        //if (i > 1)
        //    Renglon -= 0.5;
        //if (exCalle != string.Empty)
        //{
        //    i = fnAgregarMultilinea(Encabezado, rfc, fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
        //    i = fnAgregarMultilinea(Encabezado,"EXPEDIDO EN", fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
        //    i = fnAgregarMultilinea(Encabezado, exDireccion, fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
        //    i = fnAgregarMultilinea(Encabezado, exUbicacion, fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
        //    i = fnAgregarMultilinea(Encabezado, exUbicacion2, fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
            
        //}
        //else
        //{
            i = fnAgregarMultilinea(Encabezado, "Expedido en :", fPropNegrita, leftPadding + 255, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, razonSocial, fPropNormal, leftPadding + 305, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
            i = fnAgregarMultilinea(Encabezado, "Calle:", fPropNegrita, leftPadding + 255, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, calle, fPropNormal, leftPadding + 280, posRenglon + tamRenglon * Renglon, 46, true);

            i = fnAgregarMultilinea(Encabezado, "Colonia:", fPropNegrita, leftPadding + 400, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, colonia, fPropNormal, leftPadding + 430, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            i = fnAgregarMultilinea(Encabezado, "Localidad:", fPropNegrita, leftPadding + 255, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, municipio, fPropNormal, leftPadding + 295, posRenglon + tamRenglon * Renglon, 46, true);

            i = fnAgregarMultilinea(Encabezado, "Estado:", fPropNegrita, leftPadding + 400, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, estado, fPropNormal, leftPadding + 430, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, "C.P.:", fPropNegrita, leftPadding + 500, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, codigoPostal, fPropNormal, leftPadding + 520, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            i = fnAgregarMultilinea(Encabezado, "RFC:", fPropNegrita, leftPadding + 300, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, rfc, fPropNormal, leftPadding + 320, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, "N.E.:", fPropNegrita, leftPadding + 400, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, noExterior, fPropNormal, leftPadding + 420, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, "N.I.:", fPropNegrita, leftPadding + 460, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, noInterior, fPropNormal, leftPadding + 480, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            i = fnAgregarMultilinea(Encabezado, "Fecha:", fPropNegrita, leftPadding + 300, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, fechaComprobante.ToString("yyyy/MM/dd"), fPropNormal, leftPadding + 325, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, "Hora:", fPropNegrita, leftPadding + 380, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, fechaComprobante.ToString("HH:mm:ss"), fPropNormal, leftPadding + 405, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            i = fnAgregarMultilinea(Encabezado, "Factura:    "+folio+" --- "+serie, fPropNegrita, leftPadding + 300, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            i = fnAgregarMultilinea(Encabezado, "No. Certificado:", fPropNegrita, leftPadding + 300, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, noCertificadoEmisor, fPropNormal, leftPadding + 355, posRenglon + tamRenglon * Renglon, 46, true);
            i=fnAgregarMultilinea(Encabezado, "Hotel Casa Grande CD Juarez", fPropNormal, 0, posRenglon + tamRenglon * Renglon, 46, true);
            
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            i = fnAgregarMultilinea(Encabezado, "Año de Aprobación:", fPropNegrita, leftPadding + 300, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, anioAprobacion, fPropNormal, leftPadding + 370, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, "Reservaciones: 01-800-711-42-22", fPropNormal, 0, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            i = fnAgregarMultilinea(Encabezado, "No. de Aprobación:", fPropNegrita, leftPadding + 300, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, noAprobacion, fPropNormal, leftPadding + 370, posRenglon + tamRenglon * Renglon, 46, true);
            i = fnAgregarMultilinea(Encabezado, "Teléfono (656) 629 40 00", fPropNormal, 0, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
           
            //Datos fijos
           

        //}
        //string sRegFis = string.Empty;
        //if (!string.IsNullOrEmpty(referencia))
        //{
        //    string Refer = "Referencia: " + referencia;
        //    //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 4.5, new RepString(fPropNormal, Refer));
        //     i = fnAgregarMultilinea(Encabezado, Refer, fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //    Renglon += i;
        //    if (i > 1)
        //        Renglon -= 0.5;
        //    if (version == "3.2")
        //    {
        //        sRegFis = "Regimen Fiscal:";
        //        //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 5.3, new RepString(fPropNormal, sRegFis));
        //        //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 6.1, new RepString(fPropNormal, Regimenfiscal));
        //        i = fnAgregarMultilinea(Encabezado, sRegFis, fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //        Renglon += i;
        //        if (i > 1)
        //            Renglon -= 0.5;
        //        i = fnAgregarMultilinea(Encabezado, Regimenfiscal, fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //        Renglon += i;
        //        if (i > 1)
        //            Renglon -= 0.5;
        //    }
        //}
        //else
        //{
        //    if (version == "3.2")
        //    {
        //        sRegFis = "Regimen Fiscal:";
        //        //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 4.5, new RepString(fPropNormal, sRegFis));
        //        //Encabezado.Add(leftPadding + 155, posRenglon + tamRenglon * 5.3, new RepString(fPropNormal, Regimenfiscal));
        //        i = fnAgregarMultilinea(Encabezado, sRegFis, fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //        Renglon += i;
        //        if (i > 1)
        //            Renglon -= 0.5;
        //        i = fnAgregarMultilinea(Encabezado, Regimenfiscal, fPropNormal, leftPadding + 155, posRenglon + tamRenglon * Renglon, 46, true);
        //        Renglon += i;
        //        if (i > 1)
        //            Renglon -= 0.5;
        //    }
        //}

        //Agregamos los paneles visuales para el tipo de documento, serie y folio
        //double fAltoPanel = Encabezado.rHeight / 6;
        //double fAnchoPanel = Encabezado.rWidth / 5;
        //double posX = Encabezado.rWidth - fAnchoPanel;
        ////fnCrearPanelRedondeado(Encabezado, posX, 0, fAnchoPanel, fAltoPanel, grosorPen, radioCurva, true);
        ////fnCrearPanelRedondeado(Encabezado, posX, fAltoPanel, fAnchoPanel, fAltoPanel, grosorPen, radioCurva, false);
        ////fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - 190, 0, fAnchoPanel + 75, fAltoPanel, grosorPen, radioCurva, false, sColor);
        ////fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - 190, fAltoPanel, fAnchoPanel + 75, fAltoPanel, grosorPen, radioCurva, false, sColor);

        ////fnAgregarMultilineaCentrada(Encabezado, TipoDocumento, fPropBlanca, posX + fAnchoPanel / 2, fAltoPanel / 2, 15);
        ////fnAgregarMultilineaCentrada(Encabezado, TipoDocumento, fPropNegrita, posX + fAnchoPanel / 6, fAltoPanel / 2, 15);

        ////Si existe complemento donativas en XML se agrega un panel y se escrcibe datos del mismo
        //if (!(string.IsNullOrEmpty(noAprobacion)))
        //{
        //    //double dYPnlDon = 484;
        //    //double dWPnlDon = 277;
        //    //double dHPnlDon = 35;
        //    //fnCrearPanelRedondeado(Encabezado, 0, dYPnlDon, fAnchoPanel + dWPnlDon, fAltoPanel + dHPnlDon, grosorPen, radioCurva, false, sColor);
        //    int r = 497;
        //    int ni = 0;
        //    double dXDon = 7;
        //    int dWTxt = 120;
        //    ni = fnAgregarMultilinea(Encabezado, leyenda, fPropNormal, dXDon, r, dWTxt, true);
        //    if (ni == 1)
        //        r += ni + 10;
        //    else
        //    r += ni + 30;

        //    ni = fnAgregarMultilinea(Encabezado, "No. Aprobación: " + noAprobacion, fPropNormal, dXDon, r, dWTxt, true);
        //    if (ni == 1)
        //        r += ni + 10;
        //    else
        //        r += ni + 30;

        //    fnAgregarMultilinea(Encabezado, "Fecha de Aprobación: " + anioAprobacion, fPropNormal, dXDon, r, dWTxt, true);
        //}
      
        ////string sUUID = string.Empty;
        ////string noCertificadoSAT = string.Empty;
        //string sDateElaboracion = string.Empty;

        ////try { sUUID = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
        ////catch { }
        ////try { noCertificadoSAT = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
        ////catch { }

        //posX = Encabezado.rWidth - leftPadding;


        ////Folio Fiscal
        ////if (!string.IsNullOrEmpty(sUUID))
        ////{
        ////    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        ////        fAltoPanel + fAltoPanel / 4.5, RepObj.rAlignTop,
        ////        new RepString(fPropNegrita, "Folio Fiscal:"));
        ////    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        ////        fAltoPanel + fAltoPanel / 1.5, RepObj.rAlignTop,
        ////        new RepString(fPropNormal, sUUID.ToUpper()));
        ////    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        ////    //    fAltoPanel * 2 + 4, RepObj.rAlignTop,
        ////    //    new RepString(fPropNegrita, "Folio Fiscal:"));
        ////    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        ////    //    fAltoPanel * 2 + 16, RepObj.rAlignTop,
        ////    //    new RepString(fPropNormal, sUUID));
        ////}

        //fPropNormal = new FontProp(fuenteNormal, 4);
        //fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
        //fPropNegrita.bBold = true;
        //fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);

        ////Numero de Serie del Emisor
        //if (!string.IsNullOrEmpty(noCertificadoEmisor))
        //{
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //        fAltoPanel * 2 + 4, RepObj.rAlignTop,
        //        new RepString(fPropNegrita, "No. de Serie del Certificado del Emisor:"));
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //        fAltoPanel * 2 + 12, RepObj.rAlignTop,
        //        new RepString(fPropNormal, noCertificadoEmisor));
        //    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //    //    fAltoPanel * 2 + 28, RepObj.rAlignTop,
        //    //    new RepString(fPropNegrita, "No de Serie del Certificado del SAT:"));
        //    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //    //    fAltoPanel * 2 + 40, RepObj.rAlignTop,
        //    //    new RepString(fPropNormal, noCertificadoSAT));
        //}

        ////Numero de Serie del SAT
        //if (!string.IsNullOrEmpty(noCertificadoSAT))
        //{
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //        fAltoPanel * 2 + 22, RepObj.rAlignTop,
        //        new RepString(fPropNegrita, "No. de Serie del Certificado del SAT:"));
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //        fAltoPanel * 2 + 30, RepObj.rAlignTop,
        //        new RepString(fPropNormal, noCertificadoSAT));
        //    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //    //    fAltoPanel * 2 + 28, RepObj.rAlignTop,
        //    //    new RepString(fPropNegrita, "No de Serie del Certificado del SAT:"));
        //    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //    //    fAltoPanel * 2 + 40, RepObj.rAlignTop,
        //    //    new RepString(fPropNormal, noCertificadoSAT));
        //}


        ////Fecha de expedicion
        ////if (fechaTimbrado != DateTime.MinValue)//(fechaComprobante != DateTime.MinValue)
        ////{
        ////    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        ////        fAltoPanel * 2 + 40, RepObj.rAlignTop,
        ////        new RepString(fPropNegrita, "Fecha y hora de certificación:"));
        ////    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        ////        fAltoPanel * 2 + 48, RepObj.rAlignTop,
        ////        new RepString(fPropNormal, fechaTimbrado.ToString("s")));
        ////    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        ////    //    fAltoPanel * 2 + 52, RepObj.rAlignTop,
        ////    //    new RepString(fPropNegrita, "Fecha y hora de certificación:"));
        ////    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        ////    //    fAltoPanel * 2 + 64, RepObj.rAlignTop,
        ////    //    new RepString(fPropNormal, fechaComprobante.ToString("s")));
        ////}

        //sDateElaboracion = fechaComprobante.ToString("yyyy/MM/dd") + " T " + fechaComprobante.ToString("HH") + ":" + fechaComprobante.ToString("mm") + ":" + fechaComprobante.ToString("ss");//fechaComprobante.ToString("dd") + " de " + fechaComprobante.ToString("MMMM") + " de " + fechaComprobante.ToString("yyyy") + " T " + fechaComprobante.ToString("HH") +":"+ fechaComprobante.ToString("mm") +":"+ fechaComprobante.ToString("ss");
        ////Lugar y Fecha
        //if (fechaComprobante != DateTime.MinValue)
        //{
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //        fAltoPanel * 2 + 58, RepObj.rAlignTop,
        //        new RepString(fPropNegrita, "Lugar, fecha y hora de emisión:"));
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //        fAltoPanel * 2 + 66, RepObj.rAlignTop,
        //        new RepString(fPropNormal, estadolug + " " + sDateElaboracion));
        //    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //    //    fAltoPanel * 2 + 76, RepObj.rAlignTop,
        //    //    new RepString(fPropNegrita, "Lugar y fecha de elaboración:"));
        //    //Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //    //    fAltoPanel * 2 + 88, RepObj.rAlignTop,
        //    //    new RepString(fPropNormal, pais + "," + estado + " a " + sDateElaboracion));
        //}

        ////Serie y Folio
        //if (!string.IsNullOrEmpty(serie) && !string.IsNullOrEmpty(folio))
        //{
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //            fAltoPanel * 2 + 75, RepObj.rAlignTop,
        //        new RepString(fPropNegrita, "Serie y Folio:"));
        //    Encabezado.AddAligned(posX, RepObj.rAlignRight,
        //            fAltoPanel * 2 + 83, RepObj.rAlignTop,
        //        new RepString(fPropRoja, serie + folio));
        //}

        fPropNormal = new FontProp(fuenteTitulo, tamFuenteNormal);
        fPropNegrita = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Red);

	}

	/// <summary>
	/// Agrega los datos del receptor al área de encabezado
	/// </summary>
	/// <param name="nsmComprobante">Manejador de nombres de espacio</param>
	/// <param name="navEncabezado">Navegador con los datos del comprobante XML</param>
	/// <param name="Encabezado">Área del encabezado donde se mostrarán los datos</param>
    private void fnDatosReceptor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, System.Drawing.Color sColor, ref double Renglon)
	{

        //fnCrearPanel(
        string razonSocial, rfc, calle, noExterior, noInterior, colonia, municipio, estado, pais, codigoPostal, serie, folio, Localidad, version,cfd;
        razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = serie = folio = Localidad = version = cfd = string.Empty;
        try { version = navEncabezado.SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value; }
        catch
        {
            version = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
        }
        cfd = version.StartsWith("2") ? "cfd" : "cfdi";
		try { serie = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@serie", nsmComprobante).Value; }
		catch { }
		try { folio = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@folio", nsmComprobante).Value; }
		catch { }

		try { razonSocial = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/@nombre", nsmComprobante).Value; }
		catch { }
		rfc = "RFC: " + navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/@rfc", nsmComprobante).Value;
		calle = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/"+cfd+":Domicilio/@calle", nsmComprobante).Value;
		try { noExterior = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/"+cfd+":Domicilio/@noExterior", nsmComprobante).Value; }
		catch { }
		try { noInterior = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/"+cfd+":Domicilio/@noInterior", nsmComprobante).Value; }
		catch { }
		try { colonia = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/"+cfd+":Domicilio/@colonia", nsmComprobante).Value; }
		catch { }
		try { municipio = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/"+cfd+":Domicilio/@municipio", nsmComprobante).Value; }
		catch { }
		try { estado = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/"+cfd+":Domicilio/@estado", nsmComprobante).Value; }
		catch { }
		pais = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/"+cfd+":Domicilio/@pais", nsmComprobante).Value;
		try { codigoPostal = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/"+cfd+":Domicilio/@codigoPostal", nsmComprobante).Value; }
		catch { }

        try { Localidad = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Receptor/"+cfd+":Domicilio/@localidad", nsmComprobante).Value; }
        catch { }

		string direccion = string.Empty;
		direccion += calle;
		if (!string.IsNullOrEmpty(noExterior))
			direccion += " No. " + noExterior;
		if (!string.IsNullOrEmpty(noInterior))
			direccion += " Int. " + noInterior;
        if (!string.IsNullOrEmpty(colonia))
            direccion += " Col. " + colonia;


		string ubicacion = string.Empty;
        ubicacion += "C.P.: " + codigoPostal;
        
            ubicacion += "     Ciudad: " + municipio;
        
            ubicacion += "    Estado: " + estado;
            ubicacion += "   Pais:" + pais;
        
        
      
		

        //fPropNormal = new FontProp(fuenteNormal, 4);
        fPropNegrita = new FontProp(fuenteTitulo, 5, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteTitulo, 5, Color.Red);

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 5;//10
        double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;
        fPropNormal.rSize = 5;
        double altoBarra = fPropBlanca.rSize * 2;
        //Encabezado.Add(leftPadding, posRazon + tamRenglon, new RepString(fPropTitulo, razonSocial)); //posRazon + 30
        //Encabezado.Add(leftPadding, posRenglon + tamRenglon * 1.5, new RepString(fPropNormal, direccion)); //* 2
        //Encabezado.Add(leftPadding, posRenglon + tamRenglon * 2.3, new RepString(fPropNormal, ubicacion)); //*2.5
        //Encabezado.Add(leftPadding, posRenglon + tamRenglon * 3.1, new RepString(fPropNormal, rfc)); //*3



        //double Renglon = 0;
        double i = 0;
        fnCrearPanelRedondeado(Encabezado, 0, posRenglon + tamRenglon * Renglon++, Encabezado.rWidth, altoBarra, grosorPen, radioCurva, true, sColor);
        i = fnAgregarMultilinea(Encabezado, "DATOS DEL CLIENTE", fPropTitulo, leftPadding, posRenglon + tamRenglon * Renglon++ -2, 75, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 1.5;
        else
            Renglon -= 1;
        i = fnAgregarMultilinea(Encabezado, rfc, fPropNegrita, leftPadding, posRenglon + tamRenglon * Renglon++, 75, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 1.5;
        else
            Renglon -= 1;
        i = fnAgregarMultilinea(Encabezado, "Razón Social: "+razonSocial, fPropNormal, leftPadding, posRenglon + tamRenglon * Renglon++, 75, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 1.5;
        else
            Renglon -= 1;

        i = fnAgregarMultilinea(Encabezado, "Dirección: "+direccion, fPropNormal, leftPadding, posRenglon + tamRenglon * Renglon, 90, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 0.5;

        
        i = fnAgregarMultilinea(Encabezado, ubicacion, fPropNormal, leftPadding, posRenglon + tamRenglon * (Renglon - 0.3), 90, true);
        
        

        fPropNormal = new FontProp(fuenteTitulo, tamFuenteNormal);
        fPropNegrita = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Red);
	}

    /// <summary>
    /// Agrega los datos de la factura
    /// </summary>
    /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
    /// <param name="navEncabezado">Navegador con los datos del comprobante XML</param>
    /// <param name="Encabezado">Área del encabezado donde se mostrarán los datos</param>
    private void fnDatosFactura(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, System.Drawing.Color sColor, ref double Renglon)
    {

        //fnCrearPanel(
        string formaDePago, numCuenta, unidadMedida, version, cfd;
        formaDePago = numCuenta = unidadMedida = string.Empty;
        try { version = navEncabezado.SelectSingleNode("/cfd:Comprobante/@version", nsmComprobante).Value; }
        catch
        {
            version = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
        }
        cfd = version.StartsWith("2") ? "cfd" : "cfdi";
        try { formaDePago = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@metodoDePago", nsmComprobante).Value; }
        catch { }
        try { numCuenta= navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/@NumCtaPago", nsmComprobante).Value; }
        catch { }

        try { unidadMedida = navEncabezado.SelectSingleNode("/"+cfd+":Comprobante/"+cfd+":Conceptos/"+cfd+":Concepto[1]/@unidad", nsmComprobante).Value; }
        catch { }
       




        //fPropNormal = new FontProp(fuenteNormal, 4);
        fPropNegrita = new FontProp(fuenteTitulo, 5, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteTitulo, 5, Color.Red);

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 5;//10
        double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;
        fPropNormal.rSize = 5;
        double altoBarra = fPropBlanca.rSize * 2;
        //Encabezado.Add(leftPadding, posRazon + tamRenglon, new RepString(fPropTitulo, razonSocial)); //posRazon + 30
        //Encabezado.Add(leftPadding, posRenglon + tamRenglon * 1.5, new RepString(fPropNormal, direccion)); //* 2
        //Encabezado.Add(leftPadding, posRenglon + tamRenglon * 2.3, new RepString(fPropNormal, ubicacion)); //*2.5
        //Encabezado.Add(leftPadding, posRenglon + tamRenglon * 3.1, new RepString(fPropNormal, rfc)); //*3



        //double Renglon = 0;
        double i = 0;
        fnCrearPanelRedondeado(Encabezado, 0, posRenglon + tamRenglon * Renglon++, Encabezado.rWidth, altoBarra, grosorPen, radioCurva, true, sColor);
        Renglon += 1;
        i = fnAgregarMultilinea(Encabezado, "Forma de Pago: ", fPropNegrita, leftPadding, posRenglon + tamRenglon * Renglon, 75, true);
        i = fnAgregarMultilinea(Encabezado, formaDePago, fPropNormal, leftPadding + 55, posRenglon + tamRenglon * Renglon++, 75, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 1.5;
        else
            Renglon -= 1;
        i = fnAgregarMultilinea(Encabezado, "Num. Cuenta: ", fPropNegrita, leftPadding, posRenglon + tamRenglon * Renglon, 75, true);
        i = fnAgregarMultilinea(Encabezado, numCuenta, fPropNormal, leftPadding + 50, posRenglon + tamRenglon * Renglon, 75, true);
        i = fnAgregarMultilinea(Encabezado, "U. Medida: ", fPropNegrita, leftPadding+300, posRenglon + tamRenglon * Renglon, 75, true);
        i = fnAgregarMultilinea(Encabezado, unidadMedida, fPropNormal, leftPadding + 340, posRenglon + tamRenglon * Renglon++, 75, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 1.5;
        else
            Renglon -= 1;
        


        fPropNormal = new FontProp(fuenteTitulo, tamFuenteNormal);
        fPropNegrita = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteTitulo, tamFuenteNormal, Color.Red);
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
    }

	/// <summary>
	/// Devuelve la cadena de texto correspondiente al valor especificado
	/// </summary>
	/// <param name="total">Valor del cual se quiere el texto</param>
	/// <returns>Cadena con el texto del valor</returns>
	private string fnTextoImporte(string psValor, string psMoneda)
	{
        CultureInfo languaje;
		NumaletCasaGrande parser = new NumaletCasaGrande();
        parser.LetraCapital = true;

		switch (psMoneda)
		{
			case "MXN":
				parser.TipoMoneda = NumaletCasaGrande.Moneda.Peso;
				break;
			case "USD":
				parser.TipoMoneda = NumaletCasaGrande.Moneda.Dolar;
				break;
			case "XEU":
				parser.TipoMoneda = NumaletCasaGrande.Moneda.Euro;
				break;
		}

        languaje = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString());

        //return parser.ToCustomString(Convert.ToDouble(psValor));
        return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
	}


    ///// <summary>
    ///// Genera una nueva imagen en formato jpeg con el código de barras bidimensional
    ///// </summary>
    ///// <param name="psCadena">La cadena a convertir</param>
    ///// <returns></returns>
    //private RepImage GenerarCodigoBidimensional()
    //{
    //    //Creamos la cadena que generará el código
    //    XmlNamespaceManager nsm = new XmlNamespaceManager(gxComprobante.NameTable);
    //    nsm.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/3");
    //    nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
    //    XPathNavigator navCodigo = gxComprobante.CreateNavigator();

    //    string sCadenaCodigo = "?re=" + navCodigo.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@rfc", nsm).Value
    //                        + "&rr=" + navCodigo.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/@rfc", nsm).Value
    //                        + "&tt=" + string.Format("{0:0000000000.000000}", navCodigo.SelectSingleNode("/cfd:Comprobante/@total", nsm).ValueAsDouble)
    //                        + "&id=" + navCodigo.SelectSingleNode("/cfd:Comprobante/cfd:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value.ToUpper();

    //    //La cadena debe tener un longitud entre 93 y 95 caracteres
    //    if (sCadenaCodigo.Length < 93 || sCadenaCodigo.Length > 95)
    //        throw new Exception("Los datos para la cadena del código CBB no cumplen con la especificación de hacienda");

    //    QRCodeEncoder ce = new QRCodeEncoder();
    //    ce.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
    //    ce.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
    //    ce.QRCodeScale = 4;
    //    ce.QRCodeVersion = 0;

    //    MemoryStream ms = new MemoryStream();
    //    ce.Encode(sCadenaCodigo, System.Text.Encoding.UTF8).
    //    Save(ms, ImageFormat.Jpeg);

    //    return new RepImage(ms, tamCodigo - 10 , tamCodigo - 10);
    //}

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
        catch(Exception ex)
        {
            clsErrorLogCasaGrande.fnNuevaEntrada(ex, clsErrorLogCasaGrande.TipoErroresLog.Referencia);
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
            clsErrorLogCasaGrande.fnNuevaEntrada(ex, clsErrorLogCasaGrande.TipoErroresLog.Referencia);
        }
        return new RepImage(ms, 40, 10);
    }


    /// <summary>
    /// Genera Imagen Cliente
    /// </summary>
    /// <returns></returns>
    private MemoryStream fnImagenCliente(int nIdComprobante)
    {
        MemoryStream ms = new MemoryStream();


        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {
            con.Open();

            try
            {
                byte[] byteImage = { };
                using (SqlCommand cmd = new SqlCommand("usp_Con_Recupera_Logo_Sel", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@nIdComprobante", nIdComprobante);
                    if (cmd.ExecuteScalar().ToString() != string.Empty)
                        byteImage = (byte[])cmd.ExecuteScalar();
                    con.Close();
                    if(byteImage !=null)
                    ms = new MemoryStream(byteImage);
                }


                //string sql = "Select logo From tbl_con_Rfcs_cat Where id_rfc =63 ";
                //SqlConnection SqlConn = new SqlConnection("server=192.168.2.13;uid=sa;pwd=F4cturax10n;database=CFD");
                //SqlCommand SqlCom = new SqlCommand(sql, SqlConn);

                //SqlConn.Open();
                //byte[] byteImage = (byte[])SqlCom.ExecuteScalar();
                //SqlConn.Close();

                //ms = new MemoryStream(byteImage);

            }
            catch (Exception ex)
            {
                clsErrorLogCasaGrande.fnNuevaEntrada(ex, clsErrorLogCasaGrande.TipoErroresLog.Referencia);
            }
        }
        return ms;
    }

    ///// <summary>
    ///// Genera Imagen Cliente
    ///// </summary>
    ///// <returns></returns>
    //private RepImage fnImagenCliente(int id_contribuyente,int s_Id_Rfc)
    //{
    //    MemoryStream ms= new MemoryStream();


    //    string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString;
    //    SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon));
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
    //        //SqlConnection SqlConn = new SqlConnection("server=192.168.2.13;uid=sa;pwd=F4cturax10n;database=CFD");
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
	#endregion
	//===============================================================

}

/// <summary>
/// Clase encargada de mantener y manipular la información de los impuestos del comprobante, 
/// tanto para traslados como para retenciones
/// </summary>
public class ImpuestoCasaGrande
{
	public string Nombre { get; set; }
	public string Tasa { get; set; }
	public string Importe { get; set; }

	//Esta propiedad retorna el texto del renglon a mostrar en el PDF
	public string TextoImpuesto
	{
		get
		{
			return Nombre + " " + TransformacionCasaGrande.fnFormatoPorcentaje(Tasa) + " " + TransformacionCasaGrande.fnFormatoCurrency(Importe);
		}
	}

	/// <summary>
	/// Crea una nueva instancia de Impuesto
	/// </summary>
	/// <param name="navPie">Navegador XML con los valores de los impuestos</param>
	/// <param name="nsmComprobante"></param>
	public ImpuestoCasaGrande(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
/// Clase encargada de mantener y manipular los datos de los conceptos del comprobante
/// </summary>
public class DetalleCasaGrande
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

    private List<ComprobanteImpuestosRetencionTCasaGrande> _retencionesT;
    public List<ComprobanteImpuestosRetencionTCasaGrande> RetencionesT
    {
        get { return _retencionesT; }
        set { _retencionesT = value; }
    }

    private List<ComprobanteImpuestosTrasladoTCasaGrande> _trasladosT;
    public List<ComprobanteImpuestosTrasladoTCasaGrande> TrasladosT
    {
        get { return _trasladosT; }
        set { _trasladosT = value; }
    }

    private t_UbicacionFiscalTCasaGrande _ubicacionFiscalT;
    public t_UbicacionFiscalTCasaGrande UbicacionFiscalT
    {
        get { return _ubicacionFiscalT; }
        set { _ubicacionFiscalT = value; }
    }

    private t_InformacionAduaneraTCasaGrande _informacionAduaneraT;
    public t_InformacionAduaneraTCasaGrande InformacionAduaneraT
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
	public DetalleCasaGrande(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
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
            List<ComprobanteImpuestosRetencionTCasaGrande> impuestosRetencion = new List<ComprobanteImpuestosRetencionTCasaGrande>();
            List<ComprobanteImpuestosTrasladoTCasaGrande> impuestosTraslado = new List<ComprobanteImpuestosTrasladoTCasaGrande>();

            //Navegar al nodo correspondiente
            VersionT = navDetalle.SelectSingleNode("@version", nsmComprobante).Value;
            RfcT = navDetalle.SelectSingleNode("@rfc", nsmComprobante).Value;
            try { NombreT = navDetalle.SelectSingleNode("@nombre", nsmComprobante).Value; }
            catch { NombreT = string.Empty; }

            //Impuestos retenidos
            XPathNodeIterator navImpuestosRet = navDetalle.Select("terceros:Impuestos/terceros:Retenciones/terceros:Retencion", nsmComprobante);

            while (navImpuestosRet.MoveNext())
            {
                ComprobanteImpuestosRetencionTCasaGrande impRet = new ComprobanteImpuestosRetencionTCasaGrande();

                try
                {
                    string sImpuestoRet = navImpuestosRet.Current.SelectSingleNode("@impuesto", nsmComprobante).Value;
                    if (!String.IsNullOrEmpty(sImpuestoRet))
                    {
                        switch (sImpuestoRet.ToUpper())
                        {
                            case "IVA": impRet.impuesto = ComprobanteImpuestosRetencionImpuestoTCasaGrande.IVA; break;
                            case "ISR": impRet.impuesto = ComprobanteImpuestosRetencionImpuestoTCasaGrande.ISR; break;
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
                ComprobanteImpuestosTrasladoTCasaGrande impTras = new ComprobanteImpuestosTrasladoTCasaGrande();

                try
                {
                    string sImpuestoTras = navImpuestosTras.Current.SelectSingleNode("@impuesto", nsmComprobante).Value;
                    if (!String.IsNullOrEmpty(sImpuestoTras))
                    {
                        switch (sImpuestoTras.ToUpper())
                        {
                            case "IVA": impTras.impuesto = ComprobanteImpuestosTrasladoImpuestoTCasaGrande.IVA; break;
                            case "IEPS": impTras.impuesto = ComprobanteImpuestosTrasladoImpuestoTCasaGrande.IEPS; break;
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
                t_UbicacionFiscalTCasaGrande ubicacionTerceros = new t_UbicacionFiscalTCasaGrande();

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
                t_InformacionAduaneraTCasaGrande aduaneraTerceros = new t_InformacionAduaneraTCasaGrande();

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
public partial class ComprobanteImpuestosRetencionTCasaGrande
{
    private ComprobanteImpuestosRetencionImpuestoTCasaGrande impuestoField;
    private decimal importeField;

    public ComprobanteImpuestosRetencionImpuestoTCasaGrande impuesto
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

public enum ComprobanteImpuestosRetencionImpuestoTCasaGrande
{
    ISR, IVA,
}

public partial class ComprobanteImpuestosTrasladoTCasaGrande
{
    private ComprobanteImpuestosTrasladoImpuestoTCasaGrande impuestoField;
    private decimal tasaField;
    private decimal importeField;

    public ComprobanteImpuestosTrasladoImpuestoTCasaGrande impuesto
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

public enum ComprobanteImpuestosTrasladoImpuestoTCasaGrande
{
    IVA, IEPS,
}

public partial class t_UbicacionFiscalTCasaGrande
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

public partial class t_InformacionAduaneraTCasaGrande
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
public class TransformacionCasaGrande
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
            return Convert.ToDouble(valor, languaje).ToString("F2", languaje) +"%";
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
    /// Contruye la cadena original a partir de un XML de CFD
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
            trans.Load(XmlReader.Create(new StringReader(clsComunCasaGrande.ObtenerParamentro(psNombreArchivoXSLT))));
            XsltArgumentList args = new XsltArgumentList();
            trans.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            sCadenaOriginal = sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            clsErrorLogCasaGrande.fnNuevaEntrada(ex, clsErrorLogCasaGrande.TipoErroresLog.Datos);
        }

        return sCadenaOriginal;
    }

    /// <summary>
    /// Contruye la cadena original a partir de un XML de CFD
    /// </summary>
    /// <param name="xml">Objeto navegador del XML</param>
    /// <returns>Retorna la cadena original</returns>
    public static string fnConstruirCadenaTimbrado(XmlDocument docNodoTimbre, string psNombreArchivoXSLT )
    {
        string sCadenaOriginal = string.Empty;

        try
        {
            //Generamos la cadena original
            XPathNavigator navNodoTimbre = docNodoTimbre.CreateNavigator();
            XslCompiledTransform xslt;
            XsltArgumentList args;
            MemoryStream ms;
            StreamReader srDll;

            // Load the type of the class.
            xslt = new XslCompiledTransform();

            switch (psNombreArchivoXSLT)
            {
                case "Cadena_original_v2":
                    xslt.Load(typeof(cadenaoriginal_2_0));
                    break;
                case "Cadena_original_v22":
                    xslt.Load(typeof(cadenaoriginal_2_2));
                    break;
                case "cadenaoriginal_3_0":
                    xslt.Load(typeof(cadenaoriginal_3_0));
                    break;
                case "cadenaoriginal_3_2":
                    xslt.Load(typeof(cadenaoriginal_3_2));
                    break;
            }
            

            ms = new MemoryStream();
            args = new XsltArgumentList();

            xslt.Transform(navNodoTimbre, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            srDll = new StreamReader(ms);

            sCadenaOriginal = srDll.ReadToEnd();

        }
        catch (Exception ex)
        {
            clsErrorLogCasaGrande.fnNuevaEntrada(ex, clsErrorLogCasaGrande.TipoErroresLog.Datos);
        }

        return sCadenaOriginal;
    }

    

   
}

#region ClasesPlantillas
public class clsErrorLogCasaGrande
{
    //private static clsInicioSesionUsuario gUsuario;
    private static InterfazSQL giSql;

    public clsErrorLogCasaGrande()
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

        string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conRecepcionProveedores"].ConnectionString;
        using (SqlConnection con = new SqlConnection(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadenaCon)))
        {
            con.Open();

            //giSql = clsComun.fnCrearConexion("conRecepcionProveedores");
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

public class clsComunCasaGrande
{

    /// <summary>
    /// Genera un nuevo objeto de conexión a la base de datos
    /// </summary>
    /// /// <param name="psNombreConexion">Nombre de la cadena de conexión guardada en el web config</param>
    /// <returns>objeto InterfazSQL</returns>
    public static InterfazSQL fnCrearConexion(string psNombreConexion)
    {
        try
        {
            string cadena = System.Configuration.ConfigurationManager.ConnectionStrings[psNombreConexion].ConnectionString;
            return new InterfazSQL(Utilerias.Encriptacion.Base64.DesencriptarBase64(cadena));
        }
        catch (Exception ex)
        {
            clsErrorLogCasaGrande.fnNuevaEntrada(ex, clsErrorLogCasaGrande.TipoErroresLog.Conexion);
            return null;
        }
    }

    public static string ObtenerParamentro(string sParametro)
    {

        string nRetorno = string.Empty;

        try
        {
            Utilerias.SQL.InterfazSQL conexion = fnCrearConexion("conRecepcionProveedores");

            conexion.AgregarParametro("sParametro", sParametro);

            nRetorno = (string)conexion.TraerEscalar("usp_Ctp_BuscarParametro_Sel", true);
        }
        catch (Exception ex)
        {
            clsErrorLogCasaGrande.fnNuevaEntrada(ex, clsErrorLogCasaGrande.TipoErroresLog.Conexion);
        }

        return nRetorno;
    }
}

public sealed class NumaletCasaGrande
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

    public NumaletCasaGrande()
    {
        MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
        SeparadorDecimalSalida = SeparadorDecimalSalidaDefault;
        LetraCapital = LetraCapitalDefault;
        ConvertirDecimales = _convertirDecimales;
    }

    public NumaletCasaGrande(Boolean ConvertirDecimales, String MascaraSalidaDecimal, String SeparadorDecimalSalida, Boolean LetraCapital)
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
        return NumaletCasaGrande.ToString(Numero, CultureInfo.CurrentCulture);
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

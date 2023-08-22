using System;
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

/// <summary>
/// Clase encargada de recibir y manipular los datos de un XML (cfd) para formar luego un archivo PDF
/// </summary>
public class clsOperacionConsultaPdfV2
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
	private const string leyendaPDF = "Este documento es una representación impresa de un CFD";


    #endregion

    /// <summary>
    /// Crea una nueva instancia de la clase con un documento XML de cfd
    /// </summary>
    /// <param name="pxComprobante"></param>
    public clsOperacionConsultaPdfV2(XmlDocument pxComprobante)
	{
		gxComprobante = pxComprobante;
        XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
        gxComprobante.InsertBefore(xDec, gxComprobante.DocumentElement);
        TipoDocumento = "FACTURA";
	}

    /// <summary>
    /// Genera un nuevo PDF mediante un objeto Report cargado con los datos del XML
    /// </summary>
    public void fnGenerarPDF()
    {
        Formatter formato = new PdfFormatter();
        PDF = new Report(formato);

        //Datos del documento
        PDF.sTitle = "cfd";
        PDF.sAuthor = "PAX Facturación";

        //Letra titulo
        fuenteTitulo = new FontDef(PDF, FontDef.StandardFont.TimesRoman);
        fPropTitulo = new FontProp(fuenteTitulo, tamFuenteTitulo, Color.Navy);
        fPropTitulo.bBold = true;

        //letra normal
        fuenteNormal = new FontDef(PDF, FontDef.StandardFont.Helvetica);
        fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
        fPropChica = new FontProp(fuenteNormal, tamFuenteChica);

        fPropBlanca = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
        fPropBlanca.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);

        fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;
        
        //Obtenemos los detalles
        List<DetalleV2> detalles = fnObtenerDetalles();

        bool bSeguir = true;

        while (bSeguir)
        {
            //Tamaño carta
            Page pagina = new Page(PDF);
            pagina.rWidthMM = anchoPagina;
            pagina.rHeightMM = altoPagina;

            StaticContainer Encabezado = fnCrearEncabezado();
            StaticContainer Pie = fnCrearPie();

            //Agregamos el encabezado y pie a la nueva página
            pagina.Add(margenPagina, margenPagina, Encabezado);

            //Marco codigo de barras Bidimensional
            fnCrearPanelRedondeado(Pie, 0, 0, tamCodigo, tamCodigo, grosorPen, radioCurva, false);
            try
            {
                pagina.Add(margenPagina + 5, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo - 5, GenerarCodigoBidimensional());
            }
            catch { }

            pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
            pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString()));

            //Creamos el área de detalle
            pagina.Add(margenPagina + 150, margenPagina + 450, fnMarcaAguaCorpus());
            pagina.Add(margenPagina, margenPagina + Encabezado.rHeight, fnCrearDetalle(detalles));

            //verificamos si aún quedan detalles
            if (detalles.Count <= 0)
                bSeguir = false;
        }
    }

    public void fnGenerarPDFSave(string ruta)
    {
        Formatter formato = new PdfFormatter();
        PDF = new Report(formato);

        //Datos del documento
        PDF.sTitle = "cfd";
        PDF.sAuthor = "PAX Facturación";

        //Letra titulo
        fuenteTitulo = new FontDef(PDF, FontDef.StandardFont.TimesRoman);
        fPropTitulo = new FontProp(fuenteTitulo, tamFuenteTitulo, Color.Navy);
        fPropTitulo.bBold = true;

        //letra normal
        fuenteNormal = new FontDef(PDF, FontDef.StandardFont.Helvetica);
        fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
        fPropChica = new FontProp(fuenteNormal, tamFuenteChica);

        fPropBlanca = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
        fPropBlanca.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);

        fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;

        //Obtenemos los detalles
        List<DetalleV2> detalles = fnObtenerDetalles();

        bool bSeguir = true;

        while (bSeguir)
        {
            //Tamaño carta
            Page pagina = new Page(PDF);
            pagina.rWidthMM = anchoPagina;
            pagina.rHeightMM = altoPagina;

            StaticContainer Encabezado = fnCrearEncabezado();
            StaticContainer Pie = fnCrearPie();

            //Agregamos el encabezado y pie a la nueva página
            pagina.Add(margenPagina, margenPagina, Encabezado);

            //Marco codigo de barras Bidimensional
            fnCrearPanelRedondeado(Pie, 0, 0, tamCodigo, tamCodigo, grosorPen, radioCurva, false);
            try
            {
                pagina.Add(margenPagina + 5, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo - 5, GenerarCodigoBidimensional());
            }
            catch { }

            pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
            pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString()));

            //Creamos el área de detalle
            pagina.Add(margenPagina + 150, margenPagina + 450, fnMarcaAguaCorpus());
            pagina.Add(margenPagina, margenPagina + Encabezado.rHeight, fnCrearDetalle(detalles));

            //verificamos si aún quedan detalles
            if (detalles.Count <= 0)
                bSeguir = false;
        }

        PDF.Save(ruta);
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
	private StaticContainer fnCrearDetalle(List<DetalleV2> detalles)
	{
		StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);

        fnAgregarBordeRedondeado(areaDetalle, grosorPen, radioCurva);

		DetalleV2[] copiaDetalles = detalles.ToArray();
		DetalleV2 d;
		double  posRenglon;
		double  altoRenglon = fPropNormal.rSize * factorSeparador;
		int     renglonActual = 1;

		//calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
		int     maxRenglones = (int)(areaDetalle.rHeight / altoRenglon);
		//Definimos el número de conceptos que se agregarán en el for
		int     maxConceptos = maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones;

		//definimos las posiciones
		double posColumna1 = areaDetalle.rWidth * 0.01;
		double posColumna2 = areaDetalle.rWidth * 0.11;
		double posColumna3 = areaDetalle.rWidth * 0.2;
		double posColumna4 = areaDetalle.rWidth * 0.75;
		double posColumna5 = areaDetalle.rWidth * 0.88;
		double posColumna6 = areaDetalle.rWidth * 0.99;

		//Mediante el for controlamos el numero de renglones para el detalle
		for (int i = 0; renglonActual <= maxConceptos; i++)
		{
			d = copiaDetalles[i];
			posRenglon = altoRenglon * renglonActual;

			//primero verificamos si la descripción cabrá en el espacio restante
			int renglones = d.descripcion.Length / 40;
			if (renglones > maxRenglones - renglonActual)
				break;

			//Primero los datos fijos del primer renglon del detalle
			areaDetalle.Add(posColumna1, posRenglon, new RepString(fPropNormal, d.noIdentificacion));
			areaDetalle.Add(posColumna2, posRenglon, new RepString(fPropNormal, d.unidad));
			areaDetalle.AddAligned(posColumna4, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.cantidad));
			areaDetalle.AddAligned(posColumna5, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.valorUnitario));
			areaDetalle.AddAligned(posColumna6, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.importe));

			//Acomodamos la descripción en tantos renglones como necesite
			renglonActual += fnAgregarMultilinea(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon, 70, true);
			detalles.Remove(d);
		}

		return areaDetalle;
	}

    /// <summary>
    /// Crea la lista de detalles a partir de los conceptos contenidos en el comprobante XML
    /// </summary>
    /// <returns>Lista de objetos Detalle</returns>
    private List<DetalleV2> fnObtenerDetalles()
    {
        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
        nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");

        List<DetalleV2> detalles = new List<DetalleV2>();

        XPathNavigator navComprobante = gxComprobante.CreateNavigator();
        XPathNodeIterator navDetalles = navComprobante.Select("/cfd:Comprobante/cfd:Conceptos/cfd:Concepto", nsmComprobante);
        while (navDetalles.MoveNext())
        {
            detalles.Add(new DetalleV2(navDetalles.Current, nsmComprobante));
        }

        return detalles;
    }

	#endregion
	//===============================================================

	//========= PIE =================================================
	#region pie

    /// <summary>
    /// Crea el pie de página del documento
    /// </summary>
    /// <returns>StaticContainer con la información del pie de página</returns>
    private StaticContainer fnCrearPie()
    {
    	StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));

		XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
		nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");

		XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

		fnTotales(nsmComprobante, navEncabezado, Pie);

		return Pie;
    }

    /// <summary>
    /// Crea un nuevo panel con la información del pie de página del comprobante
    /// </summary>
    /// <param name="nsmComprobante">Manejador de nombre de espacios</param>
    /// <param name="navPie">Navegador con los datos del pie de página</param>
    /// <param name="Pie">Contenedor en el cual se desplegarán los datos del pie de página</param>
    private void fnTotales(XmlNamespaceManager nsmComprobante, XPathNavigator navPie, StaticContainer Pie)
    {
        string subtotal, total, moneda, sello, timbre, formaDePago;
        subtotal = total = moneda = sello = timbre = formaDePago = string.Empty;

        List<ImpuestoV2> impuestos = new List<ImpuestoV2>();

        try { subtotal = navPie.SelectSingleNode("/cfd:Comprobante/@subTotal", nsmComprobante).Value; }catch{ }
        try { total = navPie.SelectSingleNode("/cfd:Comprobante/@total", nsmComprobante).Value; }catch{}
        try { timbre = navPie.SelectSingleNode("/cfd:Comprobante/cfd:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value; }catch{}
        try{formaDePago = navPie.SelectSingleNode("/cfd:Comprobante/@formaDePago", nsmComprobante).Value;}catch{}

        try { sello = navPie.SelectSingleNode("/cfd:Comprobante/@sello", nsmComprobante).Value; }catch {}
        try { moneda = navPie.SelectSingleNode("/cfd:Comprobante/@Moneda", nsmComprobante).Value; }catch {}
        XPathNodeIterator navImpuestos = null;
        try { navImpuestos = navPie.Select("/cfd:Comprobante/cfd:Impuestos/cfd:Traslados/cfd:Traslado", nsmComprobante); }catch{}

        while (navImpuestos.MoveNext())
        {
            impuestos.Add(new ImpuestoV2(navImpuestos.Current, nsmComprobante));
        }

        try { navImpuestos = navPie.Select("/cfd:Comprobante/cfd:Impuestos/cfd:Retenciones/cfd:Retencion", nsmComprobante); }catch {}
        while (navImpuestos.MoveNext())
        {
            impuestos.Add(new ImpuestoV2(navImpuestos.Current, nsmComprobante));
        }

        double verPadding = Pie.rHeight * 0.02;
        double horPadding = Pie.rWidth * 0.01;
        double posPanelTotales = Pie.rWidth - 180;

        Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding, RepObj.rAlignTop, new RepString(fPropNormal, "SUBTOTAL"));
        try
        {
            Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionV2.fnFormatoCurrency(subtotal)));
        }
        catch { }
        double altoRenglon = fPropNormal.rSize + verPadding;
        int renglon = 1;
        string textoRenglon = string.Empty;

        foreach (ImpuestoV2 i in impuestos)
        {
            Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + TransformacionV2.fnFormatoPorcentaje(i.Tasa)));
            Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionV2.fnFormatoCurrency(i.Importe)));
            renglon++;
        }

        //Agregamos el separador y el total
        Pie.Add(posPanelTotales + horPadding, tamCodigo - altoRenglon - verPadding, new RepLine(new PenProp(PDF, grosorPen, Color.Navy), Pie.rWidth - posPanelTotales - horPadding * 2, 0));

        Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, tamCodigo - altoRenglon, RepObj.rAlignTop, new RepString(fPropNormal, "TOTAL"));
        Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, tamCodigo - altoRenglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionV2.fnFormatoCurrency(total)));

        //Finalmente creamos el panel de los totales
        fnCrearPanelRedondeado(Pie, posPanelTotales, 0, Pie.rWidth - posPanelTotales, tamCodigo, grosorPen, radioCurva, false);

        //Mostramos fomra de pago y texto importe en su panel correspondiente
        Pie.Add(tamCodigo + horPadding, altoRenglon, new RepString(fPropNormal, formaDePago));
        fnAgregarMultilinea(Pie, fnTextoImporte(total, moneda), fPropChica, tamCodigo + horPadding, altoRenglon * 2, 80, true);
        fnCrearPanelRedondeado(Pie, tamCodigo, 0, posPanelTotales - tamCodigo, tamCodigo, grosorPen, radioCurva, false);

        //Estos datos estan debajo del CBB
        renglon = 1;
       
            Pie.Add(horPadding, tamCodigo + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del CFD:"));
            renglon += fnAgregarMultilinea(Pie, sello, fPropChica, horPadding, tamCodigo + altoRenglon * renglon, 130, false);
        
      
        //dibujamos el borde del pie
        fnCrearPanelRedondeado(Pie, 0, tamCodigo, Pie.rWidth, Pie.rHeight - tamCodigo - altoRenglon, grosorPen, radioCurva, false);
        try
        {
            navImpuestos = navPie.Select("/cfd:Comprobante/cfd:Impuestos/cfd:Retenciones/cfd:Retencion", nsmComprobante);
            Pie.AddAligned(Pie.rWidth / 2, RepObj.rAlignCenter, Pie.rHeight - verPadding, RepObj.rAlignTop, new RepString(fPropNormal, leyendaPDF));
        }
        catch
        {
          
        }
    }

	#endregion
	//===============================================================

	//====== ENCABEZADO =============================================
	#region encabezado

    /// <summary>
    /// Crea el área de encabezado del documento
    /// </summary>
    /// <returns>StaticContainer con la información del encabezado del comprobante</returns>
    private StaticContainer fnCrearEncabezado()
    {
        StaticContainer Encabezado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoEncabezado));
        fnCrearPanelRedondeado(Encabezado, 0, 0, Encabezado.rWidth, Encabezado.rHeight - fPropBlanca.rSize * 2, grosorPen, radioCurva, false);
        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);

        nsmComprobante.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

        XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

        try
        {
            fnDatosEmisor(nsmComprobante, navEncabezado, Encabezado);
            fnDatosReceptor(nsmComprobante, navEncabezado, Encabezado);
            fnDibujarTitulosDetalle(Encabezado);
        }
        catch
        {
       
        }
        return Encabezado;
    }

	/// <summary>
	/// Dibuja el recuadro de titulos para el área de detalle
	/// </summary>
	/// <param name="Encabezado">Contenedor al que se agregarán los titulos</param>
	private void fnDibujarTitulosDetalle(StaticContainer Encabezado)
	{
		//Dibujamos el área de los titulos
            double altoBarra = fPropBlanca.rSize * 2;
            fnCrearPanelRedondeado(Encabezado, 0, Encabezado.rHeight - altoBarra, Encabezado.rWidth, altoBarra, grosorPen, radioCurva, true);

            //Dibujamos los titulos del detalle
            //El ancho total del área es de 572 puntos
            double puntoMedio = Encabezado.rHeight - fPropBlanca.rSize;

            //Definimos la posicion de los titulos
            double posColumna1 = Encabezado.rWidth * 0.02;     //clave
            double posColumna2 = Encabezado.rWidth * 0.1;      //unidad
            double posColumna3 = Encabezado.rWidth * 0.35;     //descripcion
            double posColumna4 = Encabezado.rWidth * 0.68;     //cantidad
            double posColumna5 = Encabezado.rWidth * 0.81;     //precio
            double posColumna6 = Encabezado.rWidth * 0.92;     //importe

            Encabezado.AddAligned(posColumna1, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "CLAVE"));
            Encabezado.AddAligned(posColumna2, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "UNIDAD"));
            Encabezado.AddAligned(posColumna3, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "DESCRIPCIÓN"));
            Encabezado.AddAligned(posColumna4, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "CANTIDAD"));
            Encabezado.AddAligned(posColumna5, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "PRECIO"));
            Encabezado.AddAligned(posColumna6, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "TOTAL"));
	}

    /// <summary>
    /// Agrega los datos del emisor al área de encabezado
    /// </summary>
    /// <param name="nsmComprobante">Manejador de nombres de espacio</param>
    /// <param name="navEncabezado">Navegador con los datos del comprobante XML</param>
    /// <param name="Encabezado">Contenedor donde se mostrsrán los datos</param>
    private void fnDatosEmisor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado)
    {
        string razonSocial, rfc, calle, noExterior, noInterior, colonia, municipio, estado, pais, codigoPostal, serie, folio, fecha, noCertificadoEmisor, fechaTimb;
        razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = fecha = serie = folio = noCertificadoEmisor = fechaTimb =string.Empty;

        try { serie = navEncabezado.SelectSingleNode("/cfd:Comprobante/@serie", nsmComprobante).Value; }
        catch { }

        try { folio = navEncabezado.SelectSingleNode("/cfd:Comprobante/@folio", nsmComprobante).Value; }
        catch {  }

        try { fecha = navEncabezado.SelectSingleNode("/cfd:Comprobante/@fecha", nsmComprobante).Value; }
        catch {  }
        try { fechaTimb = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value; }
        catch { }

        DateTime fechaComprobante = DateTime.MinValue;
        DateTime fechaTimbrado = DateTime.MinValue;
        if (!string.IsNullOrEmpty(fecha))
            fechaComprobante = Convert.ToDateTime(fecha);

        if (!string.IsNullOrEmpty(fechaTimb))
            fechaTimbrado = Convert.ToDateTime(fechaTimb);

        try { razonSocial = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@nombre", nsmComprobante).Value; }
        catch { }

        try { rfc = "RFC: " + navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@rfc", nsmComprobante).Value; }
        catch { }

        try { calle = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:DomicilioFiscal/@calle", nsmComprobante).Value; }
        catch {  }

        try { noExterior = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:DomicilioFiscal/@noExterior", nsmComprobante).Value; }
        catch
        {}

        try { noInterior = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:DomicilioFiscal/@noInterior", nsmComprobante).Value; }
        catch{}

        try { colonia = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:DomicilioFiscal/@colonia", nsmComprobante).Value; }
        catch { }

        try { municipio = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:DomicilioFiscal/@municipio", nsmComprobante).Value; }
        catch { }

        try { estado = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:DomicilioFiscal/@estado", nsmComprobante).Value; }
        catch { }

        try { pais = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:DomicilioFiscal/@pais", nsmComprobante).Value; }
        catch { }

        try { codigoPostal = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/cfd:DomicilioFiscal/@codigoPostal", nsmComprobante).Value; }
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
        ubicacion += municipio;
        ubicacion += ", " + estado;
        ubicacion += ". " + pais;
        ubicacion += " C.P. " + codigoPostal;

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 5;
        double posRazon = fPropTitulo.rSize + sep;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;

        Encabezado.Add(leftPadding, posRazon, new RepString(fPropTitulo, razonSocial));
        Encabezado.Add(leftPadding, posRenglon + tamRenglon, new RepString(fPropNormal, direccion));
        Encabezado.Add(leftPadding, posRenglon + tamRenglon * 2, new RepString(fPropNormal, ubicacion));
        Encabezado.Add(leftPadding, posRenglon + tamRenglon * 3, new RepString(fPropNormal, rfc));

        //Agregamos los paneles visuales para el tipo de documento, serie y folio
        double fAltoPanel = Encabezado.rHeight / 6;
        double fAnchoPanel = Encabezado.rWidth / 5;
        double posX = Encabezado.rWidth - fAnchoPanel;
        fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - 190, 0, fAnchoPanel + 75, fAltoPanel, grosorPen, radioCurva, false);
        fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - 190, fAltoPanel, fAnchoPanel + 75, fAltoPanel, grosorPen, radioCurva, false);

        fnAgregarMultilineaCentrada(Encabezado, TipoDocumento, fPropNegrita, posX + fAnchoPanel / 6, fAltoPanel / 2, 15);


        //Encabezado.AddAligned(posX + fAnchoPanel / 2, RepObj.rAlignCenter,
        //    fAltoPanel + fAltoPanel / 2, RepObj.rAlignCenter,
        //    new RepString(fPropRoja, serie + folio));

        string sUUID = string.Empty;
        string noCertificadoSAT = string.Empty;
        string sDateElaboracion = string.Empty;

        try { sUUID = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
        catch { }
        try { noCertificadoSAT = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
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
        }

        sDateElaboracion = fechaComprobante.ToString("dd") + " de " + fechaComprobante.ToString("MMMM") + " de " + fechaComprobante.ToString("yyyy") + " T " + fechaComprobante.ToString("HH") + ":" + fechaComprobante.ToString("mm") + ":" + fechaComprobante.ToString("ss");
        //Lugar y Fecha
        if (fechaComprobante != DateTime.MinValue)
        {
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + 58, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Lugar, fecha y hora de emisión:"));
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + 66, RepObj.rAlignTop,
                new RepString(fPropNormal, pais + "," + estado + " a " + sDateElaboracion));
        }

        //Serie y Folio
        if (!string.IsNullOrEmpty(serie) && !string.IsNullOrEmpty(folio))
        {
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    fAltoPanel * 2 + 75, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Serie y Folio:"));
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    fAltoPanel * 2 + 83, RepObj.rAlignTop,
                new RepString(fPropRoja, serie + folio));
        }


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
    private void fnDatosReceptor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado)
    {
        string razonSocial, rfc, calle, noExterior, noInterior, colonia, municipio, estado, pais, codigoPostal, serie, folio;
        razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = serie = folio = string.Empty;

        try { serie = navEncabezado.SelectSingleNode("/cfd:Comprobante/@serie", nsmComprobante).Value; }
        catch {  }
        try { folio = navEncabezado.SelectSingleNode("/cfd:Comprobante/@folio", nsmComprobante).Value; }
        catch {  }

        try { razonSocial = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/@nombre", nsmComprobante).Value; }
        catch { }

        try { rfc = "RFC:" + navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/@rfc", nsmComprobante).Value; }
        catch {  }

        try { calle = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/cfd:Domicilio/@calle", nsmComprobante).Value; }
        catch {  }

        try { noExterior = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/cfd:Domicilio/@noExterior", nsmComprobante).Value; }
        catch{}

        try { noInterior = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/cfd:Domicilio/@noInterior", nsmComprobante).Value; }
        catch{}

        try { colonia = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/cfd:Domicilio/@colonia", nsmComprobante).Value; }
        catch{}

        try { municipio = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/cfd:Domicilio/@municipio", nsmComprobante).Value; }
        catch{}

        try { estado = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/cfd:Domicilio/@estado", nsmComprobante).Value; }
        catch{}
        try { pais = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/cfd:Domicilio/@pais", nsmComprobante).Value; }
        catch{}

        try { codigoPostal = navEncabezado.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/cfd:Domicilio/@codigoPostal", nsmComprobante).Value; }
        catch{}

        string direccion = string.Empty;
        direccion += calle;
        if (!string.IsNullOrEmpty(noExterior))
            direccion += " No. " + noExterior;
        if (!string.IsNullOrEmpty(noInterior))
            direccion += " Int. " + noInterior;
        if (!string.IsNullOrEmpty(noInterior))
            direccion += " Colonia " + colonia;

        string ubicacion = string.Empty;
        ubicacion += municipio;
        if (!string.IsNullOrEmpty(estado))
            ubicacion += ", " + estado;
        if (!string.IsNullOrEmpty(pais))
            ubicacion += ". " + pais;
        if (!string.IsNullOrEmpty(codigoPostal))
            ubicacion += " C.P. " + codigoPostal;

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 5;
        double posRazon = Encabezado.rHeight / 2 + fPropTitulo.rSize;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;

        Encabezado.Add(leftPadding, posRazon, new RepString(fPropTitulo, razonSocial));
        Encabezado.Add(leftPadding, posRenglon + tamRenglon, new RepString(fPropNormal, direccion));
        Encabezado.Add(leftPadding, posRenglon + tamRenglon * 2, new RepString(fPropNormal, ubicacion));
        Encabezado.Add(leftPadding, posRenglon + tamRenglon * 3, new RepString(fPropNormal, rfc));
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
	private RepRect fnCrearPanel(double ancho, double alto)
	{
		PenProp pen = new PenProp(PDF, 1, Color.Navy);

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
    public void fnCrearPanelRedondeado(StaticContainer poObjeto, double pfPosX, double pfPosY, double pfAncho, double pfAlto, double pfGrosor, double pfRadioCurva, bool pbRellenar)
    {
        PenProp pen = new PenProp(PDF, pfGrosor, Color.Navy);
        BrushProp brush = new BrushProp(PDF, Color.Navy);

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
	private void fnAgregarBorde(StaticContainer poObjeto, double pfGrosor)
	{
		PenProp pen = new PenProp(PDF, pfGrosor, Color.Navy);
		poObjeto.Add(0, poObjeto.rHeight, new RepRect(pen, poObjeto.rWidth, poObjeto.rHeight));
	}

    /// <summary>
    /// Añade un borde con esquinas redondeadas al objeto especificado
    /// </summary>
    /// <param name="poObjeto">Objeto al que se le añadirá el borde</param>
    /// <param name="pfGrosor">Medida en puntos para el grosor del borde</param>
    /// <param name="pfRadioCurva">Radio para calcular los bordes redondos</param>
    private void fnAgregarBordeRedondeado(StaticContainer poObjeto, double pfGrosor, double pfRadioCurva)
    {
        PenProp pen = new PenProp(PDF, pfGrosor, Color.Navy);

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
		Numalet parser = new Numalet();
        parser.LetraCapital = true;

		switch (psMoneda)
		{
			case "MXN":
				parser.TipoMoneda = Numalet.Moneda.Peso;
				break;
			case "USD":
				parser.TipoMoneda = Numalet.Moneda.Dolar;
				break;
			case "XEU":
				parser.TipoMoneda = Numalet.Moneda.Euro;
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
		nsm.AddNamespace("cfd", "http://www.sat.gob.mx/cfd/2");
		nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
		XPathNavigator navCodigo = gxComprobante.CreateNavigator();

		string sCadenaCodigo = "?re=" + navCodigo.SelectSingleNode("/cfd:Comprobante/cfd:Emisor/@rfc", nsm).Value
							+ "&rr=" + navCodigo.SelectSingleNode("/cfd:Comprobante/cfd:Receptor/@rfc", nsm).Value
							+ "&tt=" + string.Format("{0:0000000000.000000}", navCodigo.SelectSingleNode("/cfd:Comprobante/@total", nsm).ValueAsDouble)
							+ "&id=" + navCodigo.SelectSingleNode("/cfd:Comprobante/cfd:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value.ToUpper();

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

		return new RepImage(ms, tamCodigo - 10 , tamCodigo - 10);
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
        catch(Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Referencia);
        }
        return new RepImage(ms, 283, 144);
    }

	#endregion
	//===============================================================

}

/// <summary>
/// Clase encargada de mantener y manipular la información de los impuestos del comprobante, 
/// tanto para traslados como para retenciones
/// </summary>
public class ImpuestoV2
{
	public string Nombre { get; set; }
	public string Tasa { get; set; }
	public string Importe { get; set; }

	//Esta propiedad retorna el texto del renglon a mostrar en el PDF
	public string TextoImpuesto
	{
		get
		{
			return Nombre + " " + TransformacionV2.fnFormatoPorcentaje(Tasa) + " " + TransformacionV2.fnFormatoCurrency(Importe);
		}
	}

	/// <summary>
	/// Crea una nueva instancia de Impuesto
	/// </summary>
	/// <param name="navPie">Navegador XML con los valores de los impuestos</param>
	/// <param name="nsmComprobante"></param>
	public ImpuestoV2(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
	{
		Nombre = navImpuesto.SelectSingleNode("@impuesto", nsmComprobante).Value;
		try { Tasa = navImpuesto.SelectSingleNode("@tasa", nsmComprobante).Value; }
		catch { Tasa = "Retención"; }
		Importe = navImpuesto.SelectSingleNode("@importe", nsmComprobante).Value;
	}
}

/// <summary>
/// Clase encargada de mantener y manipular los datos de los conceptos del comprobante
/// </summary>
public class DetalleV2
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

	/// <summary>
	/// Crea una nueva instancia de Detalle
	/// </summary>
	/// <param name="navDetalle">Navegador con los datos del concepto</param>
	/// <param name="nsmComprobante">Manejador de nombres de espacio</param>
	public DetalleV2(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
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
}

/// <summary>
/// Funciones de ayuda comunes a todas las demás clases
/// </summary>
public class TransformacionV2
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
    /// Contruye la cadena original a partir de un XML de cfd
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
            trans.Load(XmlReader.Create(new StringReader(clsComun.ObtenerParamentro(psNombreArchivoXSLT))));
            XsltArgumentList args = new XsltArgumentList();
            trans.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            sCadenaOriginal = sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Datos);
        }

        return sCadenaOriginal;
    }
}
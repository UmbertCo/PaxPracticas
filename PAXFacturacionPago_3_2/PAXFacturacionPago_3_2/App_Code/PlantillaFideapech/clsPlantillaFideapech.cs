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
using System.Threading;
using System.Web;
using System.Xml.Xsl;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;

/// <summary>
/// Clase encargada de recibir y manipular los datos de un XML (CFDI) para formar luego un archivo PDF
/// </summary>
public class clsPlantillaFideapech
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
	private const double altoEncabezado = 90;
	private double altoPie = 85;
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
    /// <param name="pxComprobante"></param>
    public clsPlantillaFideapech(XmlDocument pxComprobante)
	{           
		gxComprobante = pxComprobante;
        //XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
        //gxComprobante.InsertBefore(xDec, gxComprobante.DocumentElement);
        //TipoDocumento = "FACTURA";
	}

    /// <summary>
    /// Genera un nuevo PDF mediante un objeto Report cargado con los datos del XML
    /// </summary>
	public void fnGenerarPDF(string scolor)
	{
        System.Drawing.ColorConverter colConvert = new ColorConverter();
        System.Drawing.Color ColorT = new System.Drawing.Color();
        scolor = "#00CC00";
        ColorT = (System.Drawing.Color)colConvert.ConvertFromString(scolor);

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

        fPropBlanca = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
        fPropBlanca.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);

        fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;

		//Obtenemos los detalles para contar
		List<Detalle> detallesNum = fnObtenerDetalles();

        //Contar paginas
        int nTotPag = 0;
        while (detallesNum.Count > 0)
        {
            StaticContainer Pie = fnContarCrearPie();

            fnContarPaginas(detallesNum);

            nTotPag += 1;
        }

        //Obtenemos los detalles
        List<Detalle> detalles = fnObtenerDetalles();

		bool bSeguir = true;

		while (bSeguir)
		{
            DataSet ds = new DataSet();
			//Tamaño carta
			Page pagina = new Page(PDF);
			pagina.rWidthMM = anchoPagina;
			pagina.rHeightMM = altoPagina;

            StaticContainer Encabezado = fnCrearEncabezado(ColorT, ref ds);
            StaticContainer Pie = fnCrearPie(pagina.iPageNo, nTotPag, scolor, ds);

			//Agregamos el encabezado y pie a la nueva página
			pagina.Add(margenPagina, margenPagina, Encabezado);

            
            //posicion codigo Bidimensional
            double dpsX= 490;
            double dpsY = -150;
            pagina.Add(margenPagina + dpsX, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo - dpsY, GenerarCodigoBidimensional());

			pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
            pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " de " + nTotPag));

			//Creamos el área de detalle
            //pagina.Add(margenPagina + 150, margenPagina + 450, fnMarcaAguaCorpus());
            pagina.Add(margenPagina, margenPagina + 755, fnImagenPAX());
			pagina.Add(margenPagina, margenPagina + Encabezado.rHeight, fnCrearDetalle(detalles,ColorT, ds));

			//verificamos si aún quedan detalles
			//if (detalles.Count <= 0)
				bSeguir = false;
		}
	}

    /// <summary>
    /// Cuenta la pagina
    /// </summary>
    /// <param name="detalles"></param>
    /// <returns></returns>
    private StaticContainer fnContarPaginas(List<Detalle> detalles)
    {
        StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);

        Detalle[] copiaDetalles = detalles.ToArray();
        Detalle d;
        double posRenglon;
        double altoRenglon = fPropNormal.rSize * factorSeparador;
        int renglonActual = 1;
        //int     contRenActual = 1;
        int rengTotal = 0;
        //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
        int maxRenglones = (int)((areaDetalle.rHeight / (fPropNormal.rSize + 2)) - altoRenglon);
        //Definimos el número de conceptos que se agregarán en el for
        int maxConceptos = (maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones);

        if (maxConceptos > 22)
            maxConceptos = 22;

        //definimos las posiciones
        double posColumna1 = areaDetalle.rWidth * 0.01;
        double posColumna2 = areaDetalle.rWidth * 0.114;
        double posColumna3 = areaDetalle.rWidth * 0.28; //0.2;
        //double posColumna4 = areaDetalle.rWidth * 0.75;
        //double posColumna5 = areaDetalle.rWidth * 0.88;
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

            //primero verificamos si la descripción cabrá en el espacio restante
            renglones = fnContarMultilinea(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon, 42, true);

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

            int Col1, Col2, Col3 = 0;
            Col1 = fnAgregarMultilinea(areaDetalle, d.noIdentificacion, fPropNormal, posColumna1, posRenglon, 9, true);
            Col2 = fnAgregarMultilinea(areaDetalle, d.unidad, fPropNormal, posColumna2, posRenglon, 9, true);
            Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon, 42, true);
            //Acomodamos la descripción en tantos renglones como necesite
            renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);

            //renglonActual = renglonActual+1;
            maxConceptos -= 1;
            detalles.Remove(d);
        }

        return areaDetalle;
    }

    private int fnContarMultilinea(StaticContainer pContenedor, string psTexto, FontProp pFuente, double pdPosX, double pdPosY, int pnTamRenglon, bool bBuscarEspacio)
    {
        double nAlturaRenglon = pFuente.rSize * 1.2;
        int nNoRenglon = 0;

        foreach (RepString r in fnContarDividirRenglones(psTexto, pnTamRenglon, pFuente, bBuscarEspacio))
        {
            nNoRenglon++;
        }

        return nNoRenglon;
    }

    /// <summary>
    /// Simula la escritura de texto para calcular el total de paginas.
    /// </summary>
    /// <param name="psTexto">Cadena de texto a dividir</param>
    /// <param name="pnTamRenglon">Número de caracteres por renglón</param>
    /// <param name="pFuente">Objeto FontProp con las características de la fuente a usar</param>
    /// /// <param name="bBuscarEspacio">Especifica si la cadena de texto debe ser cortada en la posición del ultimo espacio en blanco</param>
    /// <returns>Retorna una lista de objetos RepString repersentando los renglones</returns>
    private List<RepString> fnContarDividirRenglones(string psTexto, int pnTamRenglon, FontProp pFuente, bool bBuscarEspacio)
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
                            renglones.Add(new RepString(pFuente, psTexto.Substring(0, nUltimoEspacio).TrimStart()));
                        }
                        else
                        {
                            //Si no revasa lo permitido se escribe la linea
                            nUltimoEspacio = psTexto.Length;
                            //renglones.Add(new RepString(pFuente, psTexto.ToString().TrimStart()));
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

    public void fnGenerarPDFSave(string ruta, string scolor)
    {
        System.Drawing.ColorConverter colConvert = new ColorConverter();
        System.Drawing.Color ColorT = new System.Drawing.Color();
        scolor = "#00CC00";
        ColorT = (System.Drawing.Color)colConvert.ConvertFromString(scolor);

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

        fPropBlanca = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
        fPropBlanca.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);

        fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
        fPropNegrita.bBold = true;

        //Obtenemos los detalles para contar
        List<Detalle> detallesNum = fnObtenerDetalles();

        //Contar paginas
        int nTotPag = 0;
        while (detallesNum.Count > 0)
        {
            StaticContainer Pie = fnContarCrearPie();

            fnContarPaginas(detallesNum);

            nTotPag += 1;
        }

        //Obtenemos los detalles
        List<Detalle> detalles = fnObtenerDetalles();

        bool bSeguir = true;

        while (bSeguir)
        {
            DataSet ds = new DataSet();
            //Tamaño carta
            Page pagina = new Page(PDF);
            pagina.rWidthMM = anchoPagina;
            pagina.rHeightMM = altoPagina;

            StaticContainer Encabezado = fnCrearEncabezado(ColorT, ref ds);
            StaticContainer Pie = fnCrearPie(pagina.iPageNo, nTotPag, scolor, ds);

            //Agregamos el encabezado y pie a la nueva página
            pagina.Add(margenPagina, margenPagina, Encabezado);


            //posicion codigo Bidimensional
            double dpsX = 490;
            double dpsY = -150;
            pagina.Add(margenPagina + dpsX, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo - dpsY, GenerarCodigoBidimensional());

            pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
            pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " de " + nTotPag));

            //Creamos el área de detalle
            //pagina.Add(margenPagina + 150, margenPagina + 450, fnMarcaAguaCorpus());
            pagina.Add(margenPagina, margenPagina + 755, fnImagenPAX());
            pagina.Add(margenPagina, margenPagina + Encabezado.rHeight, fnCrearDetalle(detalles, ColorT, ds));

            //verificamos si aún quedan detalles
            //if (detalles.Count <= 0)
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
    private StaticContainer fnCrearDetalle(List<Detalle> detalles, System.Drawing.Color sColor, DataSet ds)
	{
		StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);

        fnAgregarBordeRedondeado(areaDetalle, grosorPen, radioCurva, sColor);

		Detalle[] copiaDetalles = detalles.ToArray();
		Detalle d;
		double  posRenglon = 0;
		double  altoRenglon = fPropNormal.rSize * factorSeparador;
		int  renglonActual = 1;
        //int     contRenActual = 1;
        int rengTotal = 0;

		//calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
		int     maxRenglones = (int)((areaDetalle.rHeight / (fPropNormal.rSize + 2)) - altoRenglon);
		//Definimos el número de conceptos que se agregarán en el for
		int     maxConceptos = (maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones);

        if (maxConceptos > 22)
            maxConceptos = 22;
     
        //definimos las posiciones
        double posColumnatitulo = areaDetalle.rWidth * 0.02;
        double posColumna1 = areaDetalle.rWidth * 0.095;        //Fechas
        double posColumna2 = 51.48 * 2.7  ;                       //Días
        double posColumna3 = 102 + posColumna1 + 20;            //Capital
        double posColumna4 = posColumna3 + 51.48 + 20;          //Int. Norm
        double posColumna5 = posColumna4 + 51.48 + 20;          //Tasa
        double posColumna6 = posColumna5 + 51.48 + 20;          //Int. Morat
        double posColumna7 = posColumna6 + 51.48 + 20;          //Tasa
        double posColumna8 = 520; //posColumna7 + 51.48 + 15;          //SubTotal

        //int renglones = 0;
        double a = 0;
        //maxRenglones = 50;

        foreach (DataRow dr in ds.Tables["Concepto"].Rows)
        {
            double nAlturaRenglon = fPropNormal.rSize * 1.2;

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

            fnAgregarMultilinea(areaDetalle, dr["noContrato"].ToString(), fPropNormal, posColumnatitulo, posRenglon, 9, true); //No Contrato
            fnAgregarMultilinea(areaDetalle, dr["Fecha"].ToString(), fPropNormal, posColumna1, posRenglon, 16, true); //Fecha
            fnAgregarMultilinea(areaDetalle, dr["Dias"].ToString(), fPropNormal, posColumna2, posRenglon, 9, true); //
            fnAgregarMultilinea(areaDetalle,Transformacion.fnFormatoCurrency(dr["Capital"].ToString()), fPropNormal, posColumna3, posRenglon, 13, true); //
            fnAgregarMultilinea(areaDetalle,Transformacion.fnFormatoCurrency(dr["IntNorm"].ToString()), fPropNormal, posColumna4, posRenglon, 13, true); //
            fnAgregarMultilinea(areaDetalle,Transformacion.fnFormatoPorcentaje(dr["TasaUno"].ToString()), fPropNormal, posColumna5, posRenglon, 9, true); //
            fnAgregarMultilinea(areaDetalle,Transformacion.fnFormatoCurrency(dr["IntMorat"].ToString()), fPropNormal, posColumna6, posRenglon, 13, true); //
            fnAgregarMultilinea(areaDetalle,Transformacion.fnFormatoPorcentaje(dr["TasaDos"].ToString()), fPropNormal, posColumna7, posRenglon, 9, true); //
            fnAgregarMultilinea(areaDetalle,Transformacion.fnFormatoCurrency(dr["SubTotal"].ToString()), fPropNormal, posColumna8, posRenglon, 13, true); //

            renglonActual += 1;
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
	private List<Detalle> fnObtenerDetalles()
	{
		XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
		nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

		List<Detalle> detalles = new List<Detalle>();

		XPathNavigator navComprobante = gxComprobante.CreateNavigator();
		XPathNodeIterator navDetalles = navComprobante.Select("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsmComprobante);

		while (navDetalles.MoveNext())
		{
			detalles.Add(new Detalle(navDetalles.Current, nsmComprobante));
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

        Pie.rHeight += renglon * (fPropChica.rSize * 1.2);
        altoPie = Pie.rHeightMM;

    }

	/// <summary>
	/// Crea el pie de página del documento
	/// </summary>
	/// <returns>StaticContainer con la información del pie de página</returns>
    private StaticContainer fnCrearPie(int nNumPag, int nTotPag, string sColor, DataSet ds)
	{
		StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));

		XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
		nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
		nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");

		XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

		fnTotales(nsmComprobante, navEncabezado, Pie, nNumPag, nTotPag, sColor, ds);

		return Pie;
	}

	/// <summary>
	/// Crea un nuevo panel con la información del pie de página del comprobante
	/// </summary>
	/// <param name="nsmComprobante">Manejador de nombre de espacios</param>
	/// <param name="navPie">Navegador con los datos del pie de página</param>
	/// <param name="Pie">Contenedor en el cual se desplegarán los datos del pie de página</param>
    private void fnTotales(XmlNamespaceManager nsmComprobante, XPathNavigator navPie, StaticContainer Pie, int nNumPag, int nTotPag, string sColor, DataSet ds)
	{
        System.Drawing.ColorConverter colConvert = new ColorConverter();
        System.Drawing.Color ColorT = new System.Drawing.Color();

        ColorT = (System.Drawing.Color)colConvert.ConvertFromString(sColor);

        string subtotal, total, moneda, sello, timbre, formaDePago, metodoPago, Regimenfiscal, version, sNumCtaPago;
        subtotal = total = moneda = sello = timbre = formaDePago = Regimenfiscal = metodoPago = version = sNumCtaPago = string.Empty;

		List<Impuesto> impuestos = new List<Impuesto>();
        List<ImpuestoComp> impuestosComp = new List<ImpuestoComp>();

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

            try { sNumCtaPago = navPie.SelectSingleNode("/cfdi:Comprobante/@NumCtaPago", nsmComprobante).Value; }
            catch { }

        //try { Regimenfiscal = navPie.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:RegimenFiscal/@Regimen", nsmComprobante).Value; }
        //catch { }

		XPathNodeIterator navImpuestos = navPie.Select("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Traslados/cfdi:Traslado", nsmComprobante);
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            while (navImpuestos.MoveNext())
            {
                impuestos.Add(new Impuesto(navImpuestos.Current, nsmComprobante));
            }

            navImpuestos = navPie.Select("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion", nsmComprobante);
            while (navImpuestos.MoveNext())
            {
                impuestos.Add(new Impuesto(navImpuestos.Current, nsmComprobante));
            }
        }

        //Complementos impuestos locales
        try
        {
            XPathNodeIterator navComplemento = navPie.Select("/cfdi:Comprobante/cfdi:Complemento/implocal:ImpuestosLocales/implocal:TrasladosLocales", nsmComprobante);

            while (navComplemento.MoveNext())
            {
                impuestosComp.Add(new ImpuestoComp(navComplemento.Current, nsmComprobante));
            }

            navComplemento = navPie.Select("/cfdi:Comprobante/cfdi:Complemento/implocal:ImpuestosLocales/implocal:RetencionesLocales", nsmComprobante);

            while (navComplemento.MoveNext())
            {
                impuestosComp.Add(new ImpuestoComp(navComplemento.Current, nsmComprobante));
            }
        }
        catch { }

        double verPadding = Pie.rHeight * 0.02;
        double horPadding = Pie.rWidth * 0.01;
        double posPanelTotales = Pie.rWidth - 202;//370
        double altoRenglon = fPropNormal.rSize + verPadding;
        Color ColorV = new Color();
        ColorV = (Color)colConvert.ConvertFromString("#00CC00");
        Color ColorN = new Color();
        ColorN = (Color)colConvert.ConvertFromString("#000000");

        int renglon = 1;

        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            double dRenglonY = 10;

            double dvalor = 0;
            //Mostramos forma de pago y texto importe en su panel correspondiente
            fPropNegrita.color = ColorV;
            Pie.Add(tamCodigo + horPadding, altoRenglon, new RepString(fPropNegrita, "Totales"));
            //if (ds.Tables["Totales"].Rows.Count > 0)
            //{
                //foreach (DataRow dr in ds.Tables["Totales"].Rows)
                //{
                //    dvalor = 492;
                //    horPadding = Pie.rWidth - dvalor;//80
                //    fPropNegrita.color = ColorN;
                //    Pie.Add(tamCodigo + horPadding, altoRenglon - 2, new RepString(fPropNegrita, Transformacion.fnFormatoCurrency(dr["TotalCapital"].ToString())));//suma de capital
                //    //Pie.Add(tamCodigo + horPadding, altoRenglon, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));
                //    dvalor = 497;
                //    horPadding = Pie.rWidth - dvalor;//75
                //    //Recorre posicion en X
                //    horPadding += horPadding;
                //    //double nXPLin = 100;
                //    //double nXLin = 100;
                //    Pie.Add(tamCodigo + horPadding, altoRenglon - 2, new RepString(fPropNegrita, Transformacion.fnFormatoCurrency(dr["TotalNorm"].ToString())));//suma Int. Norm
                //    //Pie.Add(nXPLin, altoRenglon, new RepLine(new PenProp(PDF, grosorPen, ColorT), nXLin, 0));
                //    dvalor = 424.5;
                //    horPadding = Pie.rWidth - dvalor;//50
                //    //Recorre posicion en X
                //    horPadding += horPadding;//295
                //    Pie.Add(tamCodigo + horPadding, altoRenglon - 2, new RepString(fPropNegrita, Transformacion.fnFormatoCurrency(dr["TotalMorat"].ToString())));//suma Int.Morat
                //    //Pie.Add(tamCodigo + horPadding, altoRenglon, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));
                //    //dvalor = 537;
                //    //horPadding = Pie.rWidth - dvalor;//45
                //    //Recorre posicion en X
                //    horPadding = 435;//+= horPadding;
                //    Pie.Add(tamCodigo + horPadding, altoRenglon - 2, new RepString(fPropNegrita, Transformacion.fnFormatoCurrency(dr["TotalSubTot"].ToString())));//suma Subtotal
                //    //Pie.Add(tamCodigo + horPadding, altoRenglon, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));
                //}
            //}
            //else
            //{
                //dvalor = 492;
                //horPadding = Pie.rWidth - dvalor;//80
                //fPropNegrita.color = ColorN;
                //Pie.Add(tamCodigo + horPadding, altoRenglon - 2, new RepString(fPropNegrita, Transformacion.fnFormatoCurrency("0")));//suma de capital
                ////Pie.Add(tamCodigo + horPadding, altoRenglon, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));
                //dvalor = 497;
                //horPadding = Pie.rWidth - dvalor;//75
                ////Recorre posicion en X
                //horPadding += horPadding;
                ////double nXPLin = 100;
                ////double nXLin = 100;
                //Pie.Add(tamCodigo + horPadding, altoRenglon - 2, new RepString(fPropNegrita, Transformacion.fnFormatoCurrency("0")));//suma Int. Norm
                ////Pie.Add(nXPLin, altoRenglon, new RepLine(new PenProp(PDF, grosorPen, ColorT), nXLin, 0));
                //dvalor = 424.5;
                //horPadding = Pie.rWidth - dvalor;//50
                ////Recorre posicion en X
                //horPadding += horPadding;//295
                //Pie.Add(tamCodigo + horPadding, altoRenglon - 2, new RepString(fPropNegrita, Transformacion.fnFormatoCurrency("0")));//suma Int.Morat
                ////Pie.Add(tamCodigo + horPadding, altoRenglon, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));
                ////dvalor = 537;
                ////horPadding = Pie.rWidth - dvalor;//45
                ////Recorre posicion en X
                //horPadding = 435;//+= horPadding;
                //Pie.Add(tamCodigo + horPadding, altoRenglon - 2, new RepString(fPropNegrita, Transformacion.fnFormatoCurrency("0")));//suma Subtotal
                ////Pie.Add(tamCodigo + horPadding, altoRenglon, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));
            //}

            //reinicia valor en X
            horPadding = Pie.rWidth * 0.01;
            //saldo de renglon 
            altoRenglon += dRenglonY;
            fPropNegrita.color = ColorV;
            Pie.Add(tamCodigo + horPadding, altoRenglon, new RepString(fPropNegrita, "Saldo de capital :"));
            dvalor = 290;
            //movimiento X
            horPadding = Pie.rWidth - dvalor;
            Pie.Add(tamCodigo + horPadding, altoRenglon, new RepString(fPropNegrita, "Programa :"));
            double nXPro = 50;
            fPropNegrita.color = ColorN;
            if (ds.Tables["Datos"].Rows.Count > 0)
            {
                if (!(string.IsNullOrEmpty(ds.Tables["Datos"].Rows[0]["Programa"].ToString())))
                {
                    Pie.Add(tamCodigo + horPadding + nXPro, altoRenglon, new RepString(fPropNegrita, ds.Tables["Datos"].Rows[0]["Programa"].ToString()));
                }
            }
            //movimiento X
            dvalor = 470;
            horPadding = Pie.rWidth - dvalor;

            //Escribe totales
            dRenglonY = 8;
            double nXPie = 50;
            foreach (DataRow dr in ds.Tables["Pie"].Rows)
            {
                Pie.Add(tamCodigo + horPadding, altoRenglon, new RepString(fPropNegrita, dr["noContrato"].ToString()));
                Pie.Add(tamCodigo + horPadding + nXPie, altoRenglon, new RepString(fPropNegrita,Transformacion.fnFormatoCurrency(dr["SaldoCapital"].ToString())));
                //salto de renglon
                altoRenglon += dRenglonY;
            }
            dRenglonY = 0;
            //reinicia valor en X
            horPadding = Pie.rWidth * 0.01;
            //saldo de renglon
            altoRenglon += dRenglonY;
            double nXObs = 70;
            fPropNegrita.color = ColorV;
            Pie.Add(tamCodigo + horPadding, altoRenglon, new RepString(fPropNegrita, "Observaciones"));
            fPropNegrita.color = ColorN;
            if (ds.Tables["Datos"].Rows.Count > 0)
            {
                if (!(string.IsNullOrEmpty(ds.Tables["Datos"].Rows[0]["Observaciones"].ToString())))
                {
                    fnAgregarMultilinea(Pie, ds.Tables["Datos"].Rows[0]["Observaciones"].ToString(), fPropNegrita, tamCodigo + horPadding + nXObs,altoRenglon, 80, true);
                    //Pie.Add(tamCodigo + horPadding + nXObs, altoRenglon, new RepString(fPropNegrita, ds.Tables["Datos"].Rows[0]["Observaciones"].ToString()));
                }
            }
            //Reinicia valor
            altoRenglon = fPropNormal.rSize + verPadding;
        }
        //Color de letra Negro
        fPropNormal.color = ColorN;
        //Estos datos estan debajo del CBB
        renglon = -2;
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            Pie.Add(horPadding, tamCodigo + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del Emisor:"));
            renglon += fnAgregarMultilinea(Pie, sello, fPropChica, horPadding, tamCodigo + altoRenglon * renglon, 130, false);

            Pie.Add(horPadding, tamCodigo + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del SAT:"));
            renglon += fnAgregarMultilinea(Pie, timbre, fPropChica, horPadding, tamCodigo + altoRenglon * renglon, 130, false);

            //Agregamos la cadena original y alargamos la sección del pie según sea necesario
            Pie.Add(horPadding, tamCodigo + altoRenglon * renglon++, new RepString(fPropNormal, "Cadena original del complemento de certificación digital del SAT:"));
            renglon = fnAgregarMultilinea(Pie, Transformacion.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0")
                , fPropChica, horPadding, tamCodigo + altoRenglon * renglon, 130, false);
           
            fnAgregarMultilinea(Pie, "Forma de Pago:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 19, 80, true);
            double dPosY = 487;
            horPadding = Pie.rWidth - dPosY;//85
            fnAgregarMultilinea(Pie, formaDePago, fPropChica, tamCodigo + horPadding, altoRenglon * 19, 80, true);
            horPadding = Pie.rWidth * 0.01;
            
            if (version == "3.2") //Si es version 3.2 muestra el siguiente contenido
            {
                //Metodo de pago 
                if (!string.IsNullOrEmpty(metodoPago))
                {
                    dPosY = 20;
                    fnAgregarMultilinea(Pie, "Método de Pago:", fPropNegrita, tamCodigo + horPadding, altoRenglon * dPosY, 80, true);
                    horPadding = 85;
                    fnAgregarMultilinea(Pie, metodoPago, fPropChica, tamCodigo + horPadding ,altoRenglon * dPosY, 80, true);
                }
            }
            
        }
        renglon = 1;
        Pie.rHeight += renglon * (fPropChica.rSize * 1.2);
        altoPie = Pie.rHeightMM;

        //dibujamos el borde del pie
        //fnCrearPanelRedondeado(Pie, 0, tamCodigo, Pie.rWidth, Pie.rHeight - tamCodigo - altoRenglon, grosorPen, radioCurva, false, ColorT);

        //Agrega www.paxfacturacion.com
        fPropNormal = new FontProp(fuenteNormal, 4);
        Pie.AddAligned(Pie.rWidth / 7, RepObj.rAlignCenter, Pie.rHeight - verPadding, RepObj.rAlignTop, new RepString(fPropNormal, clsComunPDF.ObtenerParamentro("urlHostComercial")));

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
	private StaticContainer fnCrearEncabezado(System.Drawing.Color sColor, ref DataSet ds)
	{
		StaticContainer Encabezado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoEncabezado));
        //crea el panel del encabezado
        //fnCrearPanelRedondeado(Encabezado, 0, 0, Encabezado.rWidth, Encabezado.rHeight - fPropBlanca.rSize * 2, grosorPen, radioCurva, false, sColor);

        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
        try { nsmComprobante.AddNamespace("donat", "http://www.sat.gob.mx/donat"); }
        catch { }
        
		XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
        double Renglon = 8;
		fnDatosEmisor(nsmComprobante, navEncabezado, Encabezado, sColor, ref ds);
		fnDatosReceptor(nsmComprobante, navEncabezado, Encabezado,Renglon);
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
            //double altoBarra = fPropBlanca.rSize * 2.5;
            double altoBarra = 12;
            fnCrearPanelRedondeado(Encabezado, 0, Encabezado.rHeight - altoBarra, Encabezado.rWidth, altoBarra, grosorPen, radioCurva, true, sColor);
           
            //Dibujamos los titulos del detalle
            //El ancho total del área es de 572 puntos
            double puntoMedio = Encabezado.rHeight - fPropBlanca.rSize; //26;

            //Definimos la posicion de los titulos
            double posColumnatitulo = Encabezado.rWidth * 0.02;          //Que se aplicará con sigue :
            double posColumna1 = Encabezado.rWidth * 0.095;              //Fechas
            double posColumna2 = 51.48 * 2.7;                      //Días
            double posColumna3 = 102 + posColumna1 + 20;                 //Capital
            double posColumna4 = posColumna3 + 51.48 + 20;         //Int. Norm
            double posColumna5 = posColumna4 + 51.48 + 20;         //Tasa
            double posColumna6 = posColumna5 + 51.48 + 20;         //Int. Morat
            double posColumna7 = posColumna6 + 51.48 + 20;         //Tasa
            double posColumna8 = 520; //posColumna7 + 51.48 + 20;         //SubTotal
            double nY = 20;
            Color ColorV = new Color();
            ColorConverter colConvert = new ColorConverter();
            ColorV = (Color)colConvert.ConvertFromString("#00CC00");

            fPropNormal.color = ColorV;
            Encabezado.AddAligned(posColumnatitulo, RepObj.rAlignLeft, puntoMedio - nY, RepObj.rAlignCenter, new RepString(fPropNormal, "Que se aplicará como sigue :"));
            //salto de renglon
            //puntoMedio += altoBarra;
            
            //Titulos de Columnas
            Encabezado.AddAligned(posColumna1, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Fechas"));
            Encabezado.AddAligned(posColumna2, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Días"));
            Encabezado.AddAligned(posColumna3, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Capital"));
            Encabezado.AddAligned(posColumna4, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Int. Norm"));
            Encabezado.AddAligned(posColumna5, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Tasa"));
            Encabezado.AddAligned(posColumna6, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Int. Morat"));
            Encabezado.AddAligned(posColumna7, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Tasa"));
            Encabezado.AddAligned(posColumna8, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "SubTotal"));
	}

	/// <summary>
	/// Agrega los datos del emisor al área de encabezado
	/// </summary>
	/// <param name="nsmComprobante">Manejador de nombres de espacio</param>
	/// <param name="navEncabezado">Navegador con los datos del comprobante XML</param>
	/// <param name="Encabezado">Contenedor donde se mostrsrán los datos</param>
    private void fnDatosEmisor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, System.Drawing.Color sColor, ref DataSet ds)
	{
        string razonSocial, rfc, calle, noExterior, noInterior, colonia, municipio, estado, pais, codigoPostal, serie, folio, fecha, fechaTimb,
            noCertificadoEmisor, referencia, Localidad, Regimenfiscal, version, estadolug, noAutorizacion, fechaAutorizacion, leyenda,moneda;
        razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = fecha = serie = folio =
            noCertificadoEmisor = referencia = Localidad = Regimenfiscal = version = fechaTimb = estadolug = noAutorizacion = fechaAutorizacion = leyenda = moneda= string.Empty;

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

        //Si existe complemento donativas se agrega a PDf***********************************************************************************
        try { noAutorizacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/donat:Donatarias/@noAutorizacion", nsmComprobante).Value; }
        catch { }

        try { fechaAutorizacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/donat:Donatarias/@fechaAutorizacion", nsmComprobante).Value; }
        catch { }

        try { leyenda = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/donat:Donatarias/@leyenda", nsmComprobante).Value; }
        catch { }

        try { moneda =  navEncabezado.SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante).Value; }
        catch { }
        //**********************************************************************************************************************************
        #region Adenda
        //**************************************Adenda***************************************************************
        //Crea la tabla que se genera en linea.
        DataTable table = new DataTable("Concepto");
        DataTable dtTot = new DataTable("Totales");
        DataTable dtPie = new DataTable("Pie");
        DataTable dtDat = new DataTable("Datos");

        //Estructura Tabla Concepto
        DataColumn[] keys = new DataColumn[1];

        // Create column 1.
        DataColumn column = new DataColumn();
        column.DataType = System.Type.GetType("System.Int32");
        column.ColumnName = "id_Concepto";
        column.AutoIncrement = true;
        column.AutoIncrementSeed = 1;
        column.AutoIncrementStep = 1;

        table.Columns.Add(column);
        keys[0] = column;

        table.PrimaryKey = keys;

        table.Columns.Add("noContrato");
        table.Columns.Add("Fecha");
        table.Columns.Add("Dias");
        table.Columns.Add("Capital", typeof(double));
        table.Columns.Add("IntNorm", typeof(double));
        table.Columns.Add("TasaUno", typeof(double));
        table.Columns.Add("IntMorat", typeof(double));
        table.Columns.Add("TasaDos", typeof(double));
        table.Columns.Add("SubTotal", typeof(double));

        //Estructura tabla totales
        DataColumn[] keysTot = new DataColumn[1];

        // Create column 1.
        DataColumn columnTot = new DataColumn();
        columnTot.DataType = System.Type.GetType("System.Int32");
        columnTot.ColumnName = "id_Total";
        columnTot.AutoIncrement = true;
        columnTot.AutoIncrementSeed = 1;
        columnTot.AutoIncrementStep = 1;

        dtTot.Columns.Add(columnTot);
        keysTot[0] = columnTot;

        dtTot.PrimaryKey = keysTot;

        dtTot.Columns.Add("TotalCapital",typeof(double));
        dtTot.Columns.Add("TotalNorm", typeof(double));
        dtTot.Columns.Add("TotalMorat", typeof(double));
        dtTot.Columns.Add("TotalSubTot", typeof(double));
       

        //Estructura tabla pie
        DataColumn[] keysPie = new DataColumn[1];

        // Create column 1.
        DataColumn columnPie = new DataColumn();
        columnPie.DataType = System.Type.GetType("System.Int32");
        columnPie.ColumnName = "id_Pie";
        columnPie.AutoIncrement = true;
        columnPie.AutoIncrementSeed = 1;
        columnPie.AutoIncrementStep = 1;

        dtPie.Columns.Add(columnPie);
        keysPie[0] = columnPie;

        dtPie.PrimaryKey = keysPie;

        dtPie.Columns.Add("noContrato");
        dtPie.Columns.Add("SaldoCapital", typeof(double));

        //Estructura tabla datos
        DataColumn[] keysDat = new DataColumn[1];

        // Create column 1.
        DataColumn columnDat = new DataColumn();
        columnDat.DataType = System.Type.GetType("System.Int32");
        columnDat.ColumnName = "id_Dato";
        columnDat.AutoIncrement = true;
        columnDat.AutoIncrementSeed = 1;
        columnDat.AutoIncrementStep = 1;

        dtDat.Columns.Add(columnDat);
        keysDat[0] = columnDat;

        dtDat.PrimaryKey = keysDat;

        dtDat.Columns.Add("Programa");
        dtDat.Columns.Add("Observaciones");

        string sIncidencia = string.Empty;
        string sContrato1, sImporte1, sSaldoCptalCredito1, sContrato2, sImporte2, sSaldoCptalCredito2,
               sContrato3, sImporte3, sSaldoCptalCredito3, sContrato4, sImporte4, sSaldoCptalCredito4,
            sCantidadPago, sTotalCapital, sTotalNorm, sTotalMorat, sTotalSubTot;
        sContrato1 = sImporte1 = sSaldoCptalCredito1 = sContrato2 = sImporte2 = sSaldoCptalCredito2 =
        sContrato3 = sImporte3 = sSaldoCptalCredito3 = sContrato4 = sImporte4 = sSaldoCptalCredito4 =
            sCantidadPago = sTotalCapital = sTotalNorm = sTotalMorat = sTotalSubTot = string.Empty;

        //Se lee nodo Adenda
        XmlNodeList Adenda = gxComprobante.GetElementsByTagName("cfdi:Addenda");
        //****************DATOS CONTRATO UNO**************************************
        //Se lee nodos que contiene Contrato Uno
        XmlNodeList listaCto1 =
        ((XmlElement)Adenda[0]).GetElementsByTagName("noContratoUno");
        //Se recorre nodos incidencia
        foreach (XmlElement nodo in listaCto1)
        {
            
            sContrato1 = nodo.GetAttribute("noContrato");            
            sSaldoCptalCredito1 = nodo.GetAttribute("SaldoCptalCredito");                        
            sImporte1 = nodo.GetAttribute("Importe");

            DataRow dr = dtPie.NewRow();
            dr["noContrato"] = sContrato1;
            dr["SaldoCapital"] = nodo.GetAttribute("SaldoCptal");
            dtPie.Rows.Add(dr);
        }

        XmlNodeList CtoUno = gxComprobante.GetElementsByTagName("noContratoUno");
        XmlNodeList listaCpto1 =
        ((XmlElement)CtoUno[0]).GetElementsByTagName("Concepto");
        foreach (XmlElement nodo in listaCpto1)
        {
            DataRow dr = table.NewRow();

            dr["noContrato"] = sContrato1;
            dr["Fecha"] = nodo.GetAttribute("Fecha");
            dr["Dias"] = nodo.GetAttribute("Dias");
            dr["Capital"] = nodo.GetAttribute("Capital");
            dr["IntNorm"] = nodo.GetAttribute("IntNorm");
            dr["TasaUno"] = nodo.GetAttribute("TasaUno");
            dr["IntMorat"] = nodo.GetAttribute("IntMorat");
            dr["TasaDos"] = nodo.GetAttribute("TasaDos");
            dr["SubTotal"] = nodo.GetAttribute("SubTotal");
            table.Rows.Add(dr);
        }

        //************************DATOS CONTRATO DOS********************************************
        //Se lee nodos que contiene Contrato Dos
        XmlNodeList listaCto2 =
        ((XmlElement)Adenda[0]).GetElementsByTagName("noContratoDos");
        //Se recorre nodos incidencia
        foreach (XmlElement nodo in listaCto2)
        {
            sContrato2 = nodo.GetAttribute("noContrato");
            sSaldoCptalCredito2 = nodo.GetAttribute("SaldoCptalCredito");
            sImporte2 = nodo.GetAttribute("Importe");

            DataRow dr = dtPie.NewRow();
            dr["noContrato"] = sContrato2;
            dr["SaldoCapital"] = nodo.GetAttribute("SaldoCptal");
            dtPie.Rows.Add(dr);
        }

        XmlNodeList CtoDos = gxComprobante.GetElementsByTagName("noContratoDos");
        if (CtoDos.Count > 0)
        {
            XmlNodeList listaCpto2 =
            ((XmlElement)CtoDos[0]).GetElementsByTagName("Concepto");
            foreach (XmlElement nodo in listaCpto2)
            {
                DataRow dr = table.NewRow();

                dr["noContrato"] = sContrato2;
                dr["Fecha"] = nodo.GetAttribute("Fecha");
                dr["Dias"] = nodo.GetAttribute("Dias");
                dr["Capital"] = nodo.GetAttribute("Capital");
                dr["IntNorm"] = nodo.GetAttribute("IntNorm");
                dr["TasaUno"] = nodo.GetAttribute("TasaUno");
                dr["IntMorat"] = nodo.GetAttribute("IntMorat");
                dr["TasaDos"] = nodo.GetAttribute("TasaDos");
                dr["SubTotal"] = nodo.GetAttribute("SubTotal");
                table.Rows.Add(dr);
            }
        }

        //************************DATOS CONTRATO TRES********************************************
        //Se lee nodos que contiene Contrato Tres
        XmlNodeList listaCto3 =
        ((XmlElement)Adenda[0]).GetElementsByTagName("noContratoTres");
        //Se recorre nodos incidencia
        foreach (XmlElement nodo in listaCto3)
        {
            sContrato3 = nodo.GetAttribute("noContrato");
            sSaldoCptalCredito3 = nodo.GetAttribute("SaldoCptalCredito");
            sImporte3 = nodo.GetAttribute("Importe");

            DataRow dr = dtPie.NewRow();
            dr["noContrato"] = sContrato3;
            dr["SaldoCapital"] = nodo.GetAttribute("SaldoCptal");
            dtPie.Rows.Add(dr);
        }

        XmlNodeList CtoTres = gxComprobante.GetElementsByTagName("noContratoTres");
        if (CtoTres.Count > 0)
        {
            XmlNodeList listaCpto3 =
            ((XmlElement)CtoTres[0]).GetElementsByTagName("Concepto");
            foreach (XmlElement nodo in listaCpto3)
            {
                DataRow dr = table.NewRow();

                dr["noContrato"] = sContrato3;
                dr["Fecha"] = nodo.GetAttribute("Fecha");
                dr["Dias"] = nodo.GetAttribute("Dias");
                dr["Capital"] = nodo.GetAttribute("Capital");
                dr["IntNorm"] = nodo.GetAttribute("IntNorm");
                dr["TasaUno"] = nodo.GetAttribute("TasaUno");
                dr["IntMorat"] = nodo.GetAttribute("IntMorat");
                dr["TasaDos"] = nodo.GetAttribute("TasaDos");
                dr["SubTotal"] = nodo.GetAttribute("SubTotal");
                table.Rows.Add(dr);
            }
        }
        //************************DATOS CONTRATO CUATRO********************************************
        //Se lee nodos que contiene Contrato Cuatro
        XmlNodeList listaCto4 =
        ((XmlElement)Adenda[0]).GetElementsByTagName("noContratoCuatro");
        //Se recorre nodos incidencia
        foreach (XmlElement nodo in listaCto4)
        {
            sContrato4 = nodo.GetAttribute("noContrato");
            sSaldoCptalCredito4 = nodo.GetAttribute("SaldoCptalCredito");
            sImporte4 = nodo.GetAttribute("Importe");

            DataRow dr = dtPie.NewRow();
            dr["noContrato"] = sContrato4;
            dr["SaldoCapital"] = nodo.GetAttribute("SaldoCptal");
            dtPie.Rows.Add(dr);
        }

        XmlNodeList CtoCuatro = gxComprobante.GetElementsByTagName("noContratoCuatro");
        if (CtoCuatro.Count > 0)
        {
            XmlNodeList listaCpto4 =
            ((XmlElement)CtoCuatro[0]).GetElementsByTagName("Concepto");
            foreach (XmlElement nodo in listaCpto4)
            {
                DataRow dr = table.NewRow();

                dr["noContrato"] = sContrato4;
                dr["Fecha"] = nodo.GetAttribute("Fecha");
                dr["Dias"] = nodo.GetAttribute("Dias");
                dr["Capital"] = nodo.GetAttribute("Capital");
                dr["IntNorm"] = nodo.GetAttribute("IntNorm");
                dr["TasaUno"] = nodo.GetAttribute("TasaUno");
                dr["IntMorat"] = nodo.GetAttribute("IntMorat");
                dr["TasaDos"] = nodo.GetAttribute("TasaDos");
                dr["SubTotal"] = nodo.GetAttribute("SubTotal");
                table.Rows.Add(dr);
            }
        }
        //************************Detalle Adenda***********************************************
        try
        {
            XmlNodeList listaTot =
                ((XmlElement)Adenda[0]).GetElementsByTagName("Total");
            foreach (XmlElement nodo in listaTot)
            {
                sCantidadPago = nodo.GetAttribute("CantidadPago");
                sTotalCapital = nodo.GetAttribute("TotalCapital");
                sTotalNorm = nodo.GetAttribute("TotalNorm");
                sTotalMorat = nodo.GetAttribute("TotalMorat");
                sTotalSubTot = nodo.GetAttribute("TotalSubTot");
                
                DataRow dr = dtTot.NewRow();
                dr["TotalCapital"] = nodo.GetAttribute("TotalCapital");
                dr["TotalNorm"] = nodo.GetAttribute("TotalNorm");
                dr["TotalMorat"] = nodo.GetAttribute("TotalMorat");
                dr["TotalSubTot"] = nodo.GetAttribute("TotalSubTot");
                dtTot.Rows.Add(dr);
            }
        }
        catch {  }
        try
        {
            XmlNodeList listaDat =
        ((XmlElement)Adenda[0]).GetElementsByTagName("Datos");
            foreach (XmlElement nodo in listaDat)
            {
                DataRow dr = dtDat.NewRow();
                dr["Programa"] = nodo.GetAttribute("Programa");
                dr["Observaciones"] = nodo.GetAttribute("Observaciones");
                dtDat.Rows.Add(dr);
            }
        }
        catch { }

        ds.Tables.Add(table);
        ds.Tables.Add(dtTot);
        ds.Tables.Add(dtPie);
        ds.Tables.Add(dtDat);
        //**********************************************************************************************************************************
        #endregion

        string direccion = string.Empty;
		direccion += calle;
		if (!string.IsNullOrEmpty(noExterior))
			direccion += " No. " + noExterior;
        if (!string.IsNullOrEmpty(colonia))
            direccion += " Colonia " + colonia;
        if (!string.IsNullOrEmpty(noInterior))
			direccion += " Int. " + noInterior;

		string ubicacion = string.Empty;
        ubicacion += Localidad;
        ubicacion += " C.P. " + codigoPostal;
        if (!string.IsNullOrEmpty(Localidad))
        {
            ubicacion += " " + municipio;
        }
        else
        {
            ubicacion += municipio;
        }
		ubicacion += ", " + estado;
		ubicacion += ". " + pais;

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 1;
		double posRazon = fPropTitulo.rSize + sep;
        double tamRenglon = fPropNormal.rSize + sep;
        //asignacion de color 
        Color ColorN = new Color();
        ColorConverter colConvert = new ColorConverter();
        //Color Negro
        ColorN = (System.Drawing.Color)colConvert.ConvertFromString("#000000");
        Color ColorV = new Color();

        //Color Verde
        ColorV = (System.Drawing.Color)colConvert.ConvertFromString("#00CC00");
        double posRenglon = posRazon;     
        double dPX = 400;
        double dposRen = 0.8;
        double dcalRen = 1.2;

        //double Renglon = 0;
        if (!(string.IsNullOrEmpty(sContrato1)))
        {
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, sContrato1));
            dPX = 500;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,Transformacion.fnFormatoCurrency(sImporte1)));
           
        }
        dposRen += dcalRen;
        if (!(string.IsNullOrEmpty(sContrato2)))
        {
            dPX = 400;            
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, sContrato2));
            dPX = 500;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,Transformacion.fnFormatoCurrency(sImporte2)));
          
        }
        dposRen += dcalRen;
        if (!(string.IsNullOrEmpty(sContrato3)))
        {
            dPX = 400;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, sContrato3));
            dPX = 500;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,Transformacion.fnFormatoCurrency(sImporte3)));
        }
        dposRen += dcalRen;
        if (!(string.IsNullOrEmpty(sContrato4)))
        {

            dPX = 400;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, sContrato4));
            dPX = 500;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,Transformacion.fnFormatoCurrency(sImporte4)));
        }
        dposRen += dcalRen;
        //salto de renglon 
        dposRen += dcalRen;
        dPX = 300;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, "Regimen Fiscal: " + Regimenfiscal));
        dcalRen = 1;
        dposRen += dcalRen;
        dposRen += dcalRen;
        dPX = 80;//50
        fPropNormal.color = ColorV;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,"FIDEAPECH"));
        dPX = 500;
        fPropNormal.color = ColorN;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, TipoDocumento));
        dcalRen = 1.5;
        dposRen += dcalRen;
        fPropNormal.color = ColorV;
        Encabezado.AddAligned(leftPadding, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,rfc));
        dPX = 95;//95
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,direccion));
        dposRen += dcalRen;
        Encabezado.AddAligned(leftPadding, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,ubicacion));
        dposRen += dcalRen;
        dposRen += dcalRen;
        Encabezado.AddAligned(leftPadding, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,"Chihuahua,Chih,"));
        fPropNormal.color = ColorN;
        Encabezado.AddAligned(leftPadding + dPX , RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, fecha));
        dcalRen = 3.9;
        dposRen += dcalRen;
        fPropNormal.color = ColorV;
        Encabezado.AddAligned(leftPadding, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,"La cantidad de $ "));
        fPropNormal.color = ColorN;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,Transformacion.fnFormatoCurrency(sCantidadPago)));
        dcalRen = 1.5;
        dposRen += dcalRen;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,fnTextoImporte(sCantidadPago,moneda)));

        //************Totales
        double dTX1 = 165;//160
        double dRenglon = 70.5;
        Encabezado.AddAligned(leftPadding + dTX1, RepObj.rAlignLeft, posRenglon + tamRenglon * dRenglon, RepObj.rAlignTop, new RepString(fPropNormal, Transformacion.fnFormatoCurrency(sTotalCapital)));
        double dTX2 = 235;//230
        Encabezado.AddAligned(leftPadding + dTX2, RepObj.rAlignLeft, posRenglon + tamRenglon * dRenglon, RepObj.rAlignTop, new RepString(fPropNormal, Transformacion.fnFormatoCurrency(sTotalNorm)));
        double dTX3 = 380;//375
        Encabezado.AddAligned(leftPadding + dTX3, RepObj.rAlignLeft, posRenglon + tamRenglon * dRenglon, RepObj.rAlignTop, new RepString(fPropNormal, Transformacion.fnFormatoCurrency(sTotalMorat)));
        double dTX4 = 510;//515
        Encabezado.AddAligned(leftPadding + dTX4, RepObj.rAlignLeft, posRenglon + tamRenglon * dRenglon, RepObj.rAlignTop, new RepString(fPropNormal, Transformacion.fnFormatoCurrency(sTotalSubTot)));

        //********************************************

        //Agregamos los paneles visuales para el tipo de documento, serie y folio
        double fAltoPanel = Encabezado.rHeight / 6;
        double fAnchoPanel = Encabezado.rWidth / 5;
        double posX = Encabezado.rWidth - fAnchoPanel;
        
        //Serie y Folio
        if (!string.IsNullOrEmpty(serie) && !string.IsNullOrEmpty(folio))
        {
            Encabezado.AddAligned(posX - 17, RepObj.rAlignRight,
                    fAltoPanel * 2, RepObj.rAlignTop,
                new RepString(fPropNegrita, "No. "));
            Encabezado.AddAligned(posX, RepObj.rAlignLeft,
                    fAltoPanel * 2, RepObj.rAlignTop,
                new RepString(fPropRoja, serie + folio));
        }

        //double dPosY = 105;
        //double dVariable = 185;
        sColor = Color.Green;

        string sUUID = string.Empty;
        string noCertificadoSAT = string.Empty;
        string sDateElaboracion = string.Empty;

        try { sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
        catch { }
        try { noCertificadoSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
        catch { }

        double dSizeY = 85;
        double dPosX = 397;

        //Folio Fiscal
        if (!string.IsNullOrEmpty(sUUID))
        {
            Encabezado.AddAligned(posX - dPosX, RepObj.rAlignRight,
                fAltoPanel * 2 + dSizeY , RepObj.rAlignTop,
                new RepString(fPropNegrita, "Folio Fiscal:"));
            dPosX = 215;
            Encabezado.AddAligned(posX - dPosX, RepObj.rAlignRight,
               fAltoPanel * 2 + dSizeY, RepObj.rAlignTop,
                new RepString(fPropNormal, sUUID.ToUpper()));
        }

        //Fecha de expedicion
        if (fechaTimbrado != DateTime.MinValue)//fechaComprobante != DateTime.MinValue)
        {
            dSizeY = 85;
            dPosX = 20;
            Encabezado.AddAligned(posX - dPosX , RepObj.rAlignRight,
                fAltoPanel * 2 + dSizeY, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Fecha y hora de certificación:"));
            posX = Encabezado.rWidth - fAnchoPanel;
            dPosX = 65;
            Encabezado.AddAligned(posX + dPosX, RepObj.rAlignRight,
                fAltoPanel * 2 + dSizeY, RepObj.rAlignTop,
                new RepString(fPropNormal, fechaTimbrado.ToString("s"))); //fechaComprobante.ToString("s")));
        }

        dposRen = 26;
        dPX = 250;
        fPropNormal.color = ColorV;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, "Saldo de capital antes del pago crédito"));
        fPropNormal.color = ColorN;
        if (!(string.IsNullOrEmpty(sContrato1)))
        {
            dPX = 400;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, sContrato1));
            dPX = 500;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,Transformacion.fnFormatoCurrency(sSaldoCptalCredito1)));
        }
        dposRen += dcalRen;
        if (!(string.IsNullOrEmpty(sContrato2)))
        {
            dPX = 400;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, sContrato2));
            dPX = 500;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,Transformacion.fnFormatoCurrency(sSaldoCptalCredito2)));
        }
        dposRen += dcalRen;
        if (!(string.IsNullOrEmpty(sContrato3)))
        {
            dPX = 400;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, sContrato3));
            dPX = 500;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,Transformacion.fnFormatoCurrency(sSaldoCptalCredito3)));
        }
        dposRen += dcalRen;
        if (!(string.IsNullOrEmpty(sContrato4)))
        {
            dPX = 400;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, sContrato4));
            dPX = 500;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignLeft, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal,Transformacion.fnFormatoCurrency(sSaldoCptalCredito4)));
        }
        ////Folio Fiscal
        ////if (!string.IsNullOrEmpty(sUUID))
        ////{
        ////    Encabezado.AddAligned(posX - dPosX, RepObj.rAlignRight,
        ////        fAltoPanel * 2 + dSizeY, RepObj.rAlignTop,
        ////        new RepString(fPropNegrita, "Folio Fiscal:"));
        ////    dPosX = 215;
        ////    Encabezado.AddAligned(posX - dPosX, RepObj.rAlignRight,
        ////       fAltoPanel * 2 + dSizeY, RepObj.rAlignTop,
        ////        new RepString(fPropNormal, sUUID.ToUpper()));
        ////}
        dSizeY = 580;
        dPosX = 295;
        //Numero de Serie del Emisor
        if (!string.IsNullOrEmpty(noCertificadoEmisor))
        {
            Encabezado.AddAligned(posX - dPosX, RepObj.rAlignRight,
                fAltoPanel * 2 + dSizeY, RepObj.rAlignTop,
                new RepString(fPropNegrita, "No. de Serie del Certificado del Emisor:"));
            dPosX = 190;
            Encabezado.AddAligned(posX - dPosX, RepObj.rAlignRight,
                fAltoPanel * 2 + dSizeY, RepObj.rAlignTop,
                new RepString(fPropNormal, noCertificadoEmisor));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 28, RepObj.rAlignTop,
            //    new RepString(fPropNegrita, "No de Serie del Certificado del SAT:"));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 40, RepObj.rAlignTop,
            //    new RepString(fPropNormal, noCertificadoSAT));
        }
        dSizeY = 600;
        dPosX = 307;
        //Numero de Serie del SAT
        if (!string.IsNullOrEmpty(noCertificadoSAT))
        {
            Encabezado.AddAligned(posX - dPosX, RepObj.rAlignRight,
                fAltoPanel * 2 + dSizeY, RepObj.rAlignTop,
                new RepString(fPropNegrita, "No. de Serie del Certificado del SAT:"));
            dPosX = 190;
            Encabezado.AddAligned(posX - dPosX, RepObj.rAlignRight,
                fAltoPanel * 2 + dSizeY, RepObj.rAlignTop,
                new RepString(fPropNormal, noCertificadoSAT));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 28, RepObj.rAlignTop,
            //    new RepString(fPropNegrita, "No de Serie del Certificado del SAT:"));
            //Encabezado.AddAligned(posX, RepObj.rAlignRight,
            //    fAltoPanel * 2 + 40, RepObj.rAlignTop,
            //    new RepString(fPropNormal, noCertificadoSAT));
        }
	}

	/// <summary>
	/// Agrega los datos del receptor al área de encabezado
	/// </summary>
	/// <param name="nsmComprobante">Manejador de nombres de espacio</param>
	/// <param name="navEncabezado">Navegador con los datos del comprobante XML</param>
	/// <param name="Encabezado">Área del encabezado donde se mostrarán los datos</param>
	private void fnDatosReceptor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, double Renglon)
	{
        string razonSocial, rfc, calle, noExterior, noInterior, colonia, municipio, estado, pais, codigoPostal, serie, folio, Localidad;
		razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = serie = folio = Localidad = string.Empty;

		try { serie = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value; }
		catch { }
		try { folio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
		catch { }

		try { razonSocial = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value; }
		catch { }
		rfc = "RFC:" + navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value;
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

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 7;
        double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight) / 2 + fPropTitulo.rSize;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;
        //double Renglon = 0;
        Renglon += 0.5; 
        double dVariable = 95;
        //asignacion de color 
        ColorConverter colConvert = new ColorConverter();
        Color ColorV = new Color();
        Color ColorN = new Color();
        //Color Verde
        ColorV = (System.Drawing.Color)colConvert.ConvertFromString("#00CC00");
        ColorN = (Color)colConvert.ConvertFromString("#000000");
        fPropNormal.color = ColorV;
        Renglon += fnAgregarMultilinea(Encabezado,"Recibidos de :",fPropNormal, leftPadding, posRenglon + tamRenglon * Renglon++, 55, true);
        posRenglon -= posRazon -2;
        fPropNormal.color = ColorN;
        Renglon += fnAgregarMultilinea(Encabezado, razonSocial + " "+rfc, fPropNormal, leftPadding + dVariable, posRenglon + tamRenglon * Renglon, 75, true);
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

        ////esquina superior izquierda
        //poObjeto.Add(0, lArc, new RepArc(pen, pfRadioCurva, 180, 90));

        ////esquina superior derecha
        //poObjeto.Add(poObjeto.rWidth - lArc, lArc, new RepArc(pen, pfRadioCurva, 270, 90));

        ////esquina inferior izquierda
        //poObjeto.Add(0, poObjeto.rHeight, new RepArc(pen, pfRadioCurva, 90, 90));

        ////esquina inferior derecha
        //poObjeto.Add(poObjeto.rWidth - lArc, poObjeto.rHeight, new RepArc(pen, pfRadioCurva, 0, 90));


        ////añadimos los bordes rectos
        ////borde superior
        //poObjeto.Add(pfRadioCurva, 0, new RepLine(pen, poObjeto.rWidth - lArc, 0));

        ////borde inferior
        //poObjeto.Add(pfRadioCurva, poObjeto.rHeight, new RepLine(pen, poObjeto.rWidth - lArc, 0));

        //////borde izquierdo
        //poObjeto.Add(0, pfRadioCurva, new RepLine(pen, 0, pfRadioCurva * 2 - poObjeto.rHeight));

        //////borde derecho
        //poObjeto.Add(poObjeto.rWidth, pfRadioCurva, new RepLine(pen, 0, pfRadioCurva * 2 - poObjeto.rHeight));

        //Linea totales
        double nYH = -150;
        double nYHL = 570;
        poObjeto.Add(pfRadioCurva, nYH, new RepLine(pen, nYHL, 0));

        //Linea sumatoria de Capital
        double nXT1 = 173;//168
        double nYT1 = 256;
        double nYTL1 = 45;
        poObjeto.Add(nXT1, nYT1, new RepLine(pen, nYTL1, 0));

        //Linea sumatoria de Int. Norm
        double nXT2 = 245;
        double nYT2 = 256;
        double nYTL2 = 45;
        poObjeto.Add(nXT2, nYT2, new RepLine(pen, nYTL2, 0));

        //Linea sumatoria de Int. Morat 
        double nXT3 = 390;
        double nYT3 = 256;
        double nYTL3 = 45;
        poObjeto.Add(nXT3, nYT3, new RepLine(pen, nYTL3, 0));

        //Linea sumatoria de SubTotal
        double nXT4 = 520;
        double nYT4 = 256;
        double nYTL4 = 55;
        poObjeto.Add(nXT4, nYT4, new RepLine(pen, nYTL4, 0));
    }

	/// <summary>
	/// Devuelve la cadena de texto correspondiente al valor especificado
	/// </summary>
	/// <param name="total">Valor del cual se quiere el texto</param>
	/// <returns>Cadena con el texto del valor</returns>
	private string fnTextoImporte(string psValor, string psMoneda)
	{
        CultureInfo languaje;
		NumaletPDF parser = new NumaletPDF();
        parser.LetraCapital = true;

		switch (psMoneda)
		{
			case "MXN":
				parser.TipoMoneda = NumaletPDF.Moneda.Peso;
				break;
			case "USD":
				parser.TipoMoneda = NumaletPDF.Moneda.Dolar;
				break;
			case "XEU":
				parser.TipoMoneda = NumaletPDF.Moneda.Euro;
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
            clsErrorLogPDF.fnNuevaEntrada(ex, clsErrorLogPDF.TipoErroresLog.Referencia);
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
            clsErrorLogPDF.fnNuevaEntrada(ex, clsErrorLogPDF.TipoErroresLog.Referencia);
        }
        return new RepImage(ms, 40, 10);
    }
	#endregion
	//===============================================================

}

/// <summary>
/// Clase encargada de mantener y manipular la información de los impuestos del comprobante, 
/// tanto para traslados como para retenciones
/// </summary>
public class Impuesto
{
	public string Nombre { get; set; }
	public string Tasa { get; set; }
	public string Importe { get; set; }

	//Esta propiedad retorna el texto del renglon a mostrar en el PDF
	public string TextoImpuesto
	{
		get
		{
			return Nombre + " " + Transformacion.fnFormatoPorcentaje(Tasa) + " " + Transformacion.fnFormatoCurrency(Importe);
		}
	}

	/// <summary>
	/// Crea una nueva instancia de Impuesto
	/// </summary>
	/// <param name="navPie">Navegador XML con los valores de los impuestos</param>
	/// <param name="nsmComprobante"></param>
	public Impuesto(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
public class ImpuestoComp
{
    public string Nombre { get; set; }
    public string Tasa { get; set; }
    public string Importe { get; set; }

    //Esta propiedad retorna el texto del renglon a mostrar en el PDF
    public string TextoImpuesto
    {
        get
        {
            return Nombre + " " + Transformacion.fnFormatoPorcentaje(Tasa) + " " + Transformacion.fnFormatoCurrency(Importe);
        }
    }

    /// <summary>
    /// Crea una nueva instancia de Impuesto
    /// </summary>
    /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
    /// <param name="nsmComprobante"></param>
    public ImpuestoComp(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
public class Detalle
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
	public Detalle(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
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
public class Transformacion
{

	/// <summary>
	/// Transforma un valor a su representación porcentual
	/// </summary>
	/// <param name="valor">cadena con el valor a ser transformado</param>
	/// <returns>Cadena con el nuevo valor porcentual</returns>
	public static string fnFormatoPorcentaje(string valor)
	{
        string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

        CultureInfo languaje;
        languaje = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString());;
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
        string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

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
        string language = System.Configuration.ConfigurationManager.AppSettings["DefaultLanguage"].ToString();
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

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
            trans.Load(XmlReader.Create(new StringReader(clsComunPDF.ObtenerParamentro(psNombreArchivoXSLT))));
            XsltArgumentList args = new XsltArgumentList();
            trans.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            sCadenaOriginal = sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            clsErrorLogPDF.fnNuevaEntrada(ex, clsErrorLogPDF.TipoErroresLog.Datos);
        }

        return sCadenaOriginal;
    }
}

#region ClasesPlantillas
public class clsErrorLogFideapech
{
    //private static clsInicioSesionUsuario gUsuario;

    public clsErrorLogFideapech()
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


                SqlCommand cmd = new SqlCommand("usp_Ctp_Registrar_Error_Ins", con);


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


public class clsComunPDF
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
                clsErrorLogFideapech.fnNuevaEntrada(ex, clsErrorLogFideapech.TipoErroresLog.Conexion);
            }
        }
        return sRetorno;
    }
}

public sealed class NumaletPDF
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

    public NumaletPDF()
    {
        MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
        SeparadorDecimalSalida = SeparadorDecimalSalidaDefault;
        LetraCapital = LetraCapitalDefault;
        ConvertirDecimales = _convertirDecimales;
    }

    public NumaletPDF(Boolean ConvertirDecimales, String MascaraSalidaDecimal, String SeparadorDecimalSalida, Boolean LetraCapital)
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
        return NumaletPDF.ToString(Numero, CultureInfo.CurrentCulture);
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

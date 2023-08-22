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
/// Clase encargada de recibir y manipular los datos de un XML (CFDI) para formar luego un archivo PDF
/// </summary>
public class clsPlantillaMarybelLeParfum
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
    private const double altoEncabezado = 76;
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
    /// <param name="pxComprobante"></param>
    public clsPlantillaMarybelLeParfum(XmlDocument pxComprobante)
	{
        gxComprobante = pxComprobante;
        XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
        gxComprobante.InsertBefore(xDec, gxComprobante.DocumentElement);
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
        fPropTitulo = new FontProp(fuenteTitulo, tamFuenteTitulo, Color.Black);
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
        List<DetalleLogo> detallesNum = fnObtenerDetalles();

        //Contar paginas
        int nTotPag = 0;
        while (detallesNum.Count > 0)
        {
            StaticContainer Pie = fnContarCrearPie();

            fnContarPaginas(detallesNum);

            nTotPag += 1;
        }

        //Despues de haber calculado total de páginas se reinicia valor altoPie.
        altoPie = 70;

        //Obtenemos los detalles
        List<DetalleLogo> detalles = fnObtenerDetalles();

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
            //Se agrega logos
            MemoryStream ms = new MemoryStream();
            ms = fnImagenCliente("MaribelLeParfum.jpg");

            //Creamos el área de detalle
            if (ms.Length > 0)
            {
                double dAlt = 70;
                double dAnc = 56;
                double dXimg = 13;
                double dYimg = 75;
                RepImage image = new RepImage(ms, dAnc, dAlt);
                pagina.Add(margenPagina + dXimg, margenPagina + dYimg, image);
            }

            ms = fnImagenCliente("MaribelCyP.jpg");

            //Creamos el área de detalle
            if (ms.Length > 0)
            {
                double dAlt = 81;
                double dAnc = 84;
                double dXimg = 460;
                double dYimg = 83;
                RepImage image = new RepImage(ms, dAnc, dAlt);
                pagina.Add(margenPagina + dXimg, margenPagina + dYimg, image);
            }
            //*************************************************************************************

            StaticContainer Encabezado = fnCrearEncabezado(ColorT);
            StaticContainer Pie = fnCrearPie(pagina.iPageNo, nTotPag, scolor);

            //Agregamos el encabezado y pie a la nueva página
            pagina.Add(margenPagina, margenPagina, Encabezado);

            //Marco codigo de barras Bidimensional
            fnCrearPanelRedondeado(Pie, 0, 0, tamCodigo, tamCodigo, grosorPen, radioCurva, false, ColorT);

            if (pagina.iPageNo == nTotPag)//Si es ultima pagina
                pagina.Add(margenPagina + 5, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo - 5, GenerarCodigoBidimensional());

            pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
            pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " de " + nTotPag));

            pagina.Add(margenPagina, margenPagina + 755, fnImagenPAX());
            pagina.Add(margenPagina, margenPagina + Encabezado.rHeight, fnCrearDetalle(detalles, ColorT));

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
    private StaticContainer fnContarPaginas(List<DetalleLogo> detalles)
    {
        StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);

        DetalleLogo[] copiaDetalles = detalles.ToArray();
        DetalleLogo d;
        double posRenglon;
        double altoRenglon = fPropNormal.rSize * factorSeparador;
        int renglonActual = 1;
        //int     contRenActual = 1;
        int rengTotal = 0;
        //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
        int maxRenglones = (int)((areaDetalle.rHeight / (fPropNormal.rSize + 2)) - 11);//altoRenglon);
        //Definimos el número de conceptos que se agregarán en el for
        int maxConceptos = (maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones);

        if (maxConceptos > 23)
            maxConceptos = 23;

        //definimos las posiciones
        double posColumna1 = areaDetalle.rWidth * 0.01;
        double posColumna2 = areaDetalle.rWidth * 0.11;
        double posColumna3 = areaDetalle.rWidth * 0.2;
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
            //renglones = d.descripcion.Length / 30;

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
            renglones = fnContarMultilinea(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon, 40, true);

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
            Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon, 40, true);
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

        foreach (RepString r in fnDividirRenglones(psTexto, pnTamRenglon, pFuente, bBuscarEspacio))
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
        fPropTitulo = new FontProp(fuenteTitulo, tamFuenteTitulo, Color.Black);
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
        List<DetalleLogo> detallesNum = fnObtenerDetalles();

        //Contar paginas
        int nTotPag = 0;
        while (detallesNum.Count > 0)
        {
            StaticContainer Pie = fnContarCrearPie();

            fnContarPaginas(detallesNum);

            nTotPag += 1;
        }

        //Despues de haber calculado total de páginas se reinicia valor altoPie.
        altoPie = 70;

        //Obtenemos los detalles
        List<DetalleLogo> detalles = fnObtenerDetalles();

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
            
            MemoryStream ms = new MemoryStream();
            ms = fnImagenCliente("MaribelLeParfum.jpg");

            //Creamos el área de detalle
            if (ms.Length > 0)
            {
                double dAlt = 70;
                double dAnc = 56;
                double dXimg = 13;
                double dYimg = 75;
                RepImage image = new RepImage(ms, dAnc, dAlt);
                pagina.Add(margenPagina + dXimg, margenPagina + dYimg, image);
            }

            ms = fnImagenCliente("MaribelCyP.jpg");

            //Creamos el área de detalle
            if (ms.Length > 0)
            {
                double dAlt = 81;
                double dAnc = 84;
                double dXimg = 460;
                double dYimg = 83;
                RepImage image = new RepImage(ms, dAnc, dAlt);
                pagina.Add(margenPagina + dXimg, margenPagina + dYimg, image);
            }

            StaticContainer Encabezado = fnCrearEncabezado(ColorT);
            StaticContainer Pie = fnCrearPie(pagina.iPageNo, nTotPag, scolor);

            //Agregamos el encabezado y pie a la nueva página
            pagina.Add(margenPagina, margenPagina, Encabezado);

            //Marco codigo de barras Bidimensional

            fnCrearPanelRedondeado(Pie, 0, 0, tamCodigo, tamCodigo, grosorPen, radioCurva, false, ColorT);
            if (pagina.iPageNo == nTotPag)//Si es ultima pagina
                pagina.Add(margenPagina + 5, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + tamCodigo - 5, GenerarCodigoBidimensional());

            pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);
            pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " de " + nTotPag));

            pagina.Add(margenPagina, margenPagina + 755, fnImagenPAX());
            pagina.Add(margenPagina, margenPagina + Encabezado.rHeight, fnCrearDetalle(detalles, ColorT));

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
    private StaticContainer fnCrearDetalle(List<DetalleLogo> detalles, System.Drawing.Color sColor)
    {
        StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);

        fnAgregarBordeRedondeado(areaDetalle, grosorPen, radioCurva, sColor);
        
        //Paneles en detalle
        double dXPMed, dXPDes, dXPPre, dXPCan, dXPTot, dAnPCod, dAnPMed, dAnPDes, dAnPPre, dAnPCan, dAnPTot, dAltoDet;
        dXPMed = dXPDes = dXPPre = dXPCan = dXPTot = dAnPCod = dAnPMed = dAnPDes = dAnPPre = dAnPCan = dAnPTot = 0;
        dXPMed = 50;
        dXPDes = 150;
        dXPCan = 380;
        dXPPre = 450;
        dXPTot = 510;
        dAnPMed = 100;
        dAnPCod = 50;
        dAnPDes = 230;
        dAnPCan = 70;
        dAnPPre = 60;
        dAnPTot = 62;
        dAltoDet = RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2;
        //Panel Código
        fnCrearPanelRedondeado(areaDetalle, 0, 0, dAnPCod, dAltoDet, grosorPen, radioCurva, false, sColor);
        //Unidad Medida
        fnCrearPanelRedondeado(areaDetalle, dXPMed, 0, dAnPMed, dAltoDet, grosorPen, radioCurva, false, sColor);
        //Descripcion
        fnCrearPanelRedondeado(areaDetalle, dXPDes, 0, dAnPDes, dAltoDet, grosorPen, radioCurva, false, sColor);
        //Cantidad
        fnCrearPanelRedondeado(areaDetalle, dXPCan, 0, dAnPCan, dAltoDet, grosorPen, radioCurva, false, sColor);
        //Precio
        fnCrearPanelRedondeado(areaDetalle, dXPPre, 0, dAnPPre, dAltoDet, grosorPen, radioCurva, false, sColor);
        //Total
        fnCrearPanelRedondeado(areaDetalle, dXPTot, 0, dAnPTot, dAltoDet, grosorPen, radioCurva, false, sColor);

        DetalleLogo[] copiaDetalles = detalles.ToArray();
        DetalleLogo d;
        double posRenglon;
        double altoRenglon = fPropNormal.rSize * factorSeparador;
        int renglonActual = 1;
        //int     contRenActual = 1;
        int rengTotal = 0;

        //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
        int maxRenglones = (int)((areaDetalle.rHeight / (fPropNormal.rSize + 2)) - 11); // - altoRenglon);
        //Definimos el número de conceptos que se agregarán en el for
        int maxConceptos = maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones;

        if (maxConceptos > 23)
            maxConceptos = 23;

        //definimos las posiciones
        double posColumna1 = areaDetalle.rWidth * 0.01;
        double posColumna2 = areaDetalle.rWidth * 0.114;
        double posColumna3 = areaDetalle.rWidth * 0.28; //0.2;
        double posColumna4 = areaDetalle.rWidth * 0.75;
        double posColumna5 = areaDetalle.rWidth * 0.88;
        double posColumna6 = areaDetalle.rWidth * 0.99;
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
            renglones = fnContarMultilinea(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon, 40, true);

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

            int Col1, Col2, Col3 = 0;

            Col1 = fnAgregarMultilinea(areaDetalle, d.noIdentificacion, fPropNormal, posColumna1, posRenglon, 9, true);
            Col2 = fnAgregarMultilinea(areaDetalle, d.unidad, fPropNormal, posColumna2, posRenglon, 9, true);
            areaDetalle.AddAligned(posColumna4, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.cantidad));
            areaDetalle.AddAligned(posColumna5, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.valorUnitario));
            areaDetalle.AddAligned(posColumna6, RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropNormal, d.importe));

            //Acomodamos la descripción en tantos renglones como necesite           
            Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropNormal, posColumna3, posRenglon,40, true);
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
    private List<DetalleLogo> fnObtenerDetalles()
    {
        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

        List<DetalleLogo> detalles = new List<DetalleLogo>();

        XPathNavigator navComprobante = gxComprobante.CreateNavigator();
        XPathNodeIterator navDetalles = navComprobante.Select("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsmComprobante);

        while (navDetalles.MoveNext())
        {
            detalles.Add(new DetalleLogo(navDetalles.Current, nsmComprobante));
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
        StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));

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

        string subtotal, total, moneda, sello, timbre, formaDePago, metodoPago, Regimenfiscal, version, sNumCtaPago;
        subtotal = total = moneda = sello = timbre = formaDePago = metodoPago = Regimenfiscal = version = sNumCtaPago = string.Empty;

        List<ImpuestoLogo> impuestos = new List<ImpuestoLogo>();
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
                impuestos.Add(new ImpuestoLogo(navImpuestos.Current, nsmComprobante));
            }

            navImpuestos = navPie.Select("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion", nsmComprobante);
            while (navImpuestos.MoveNext())
            {
                impuestos.Add(new ImpuestoLogo(navImpuestos.Current, nsmComprobante));
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
        double posPanelTotales = Pie.rWidth - 180;
        double renglon = 1.9;
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding * renglon, RepObj.rAlignTop, new RepString(fPropNormal, "SUBTOTAL"));
            Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionLogo.fnFormatoCurrency(subtotal)));
        }
        double altoRenglon = fPropNormal.rSize + verPadding;
        renglon += 0.4;
        string textoRenglon = string.Empty;
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            foreach (ImpuestoLogo i in impuestos)
            {
                if (i.Nombre != "IEPS")
                {
                    Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + TransformacionLogo.fnFormatoPorcentaje(i.Tasa)));
                    Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionLogo.fnFormatoCurrency(i.Importe)));
                    renglon++;
                }
            }

            try
            {
                //Agrega complemento impuestos locales 
                foreach (ImpuestoComp i in impuestosComp)
                {
                    Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + Transformacion.fnFormatoPorcentaje(i.Tasa)));
                    Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, Transformacion.fnFormatoCurrency(i.Importe)));
                    renglon++;
                }
            }
            catch { }


            //Agregamos el separador y el total


            //Pie.Add(posPanelTotales + horPadding, tamCodigo - altoRenglon - verPadding, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));
            renglon += 1;
            Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, "TOTAL"));
            Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionLogo.fnFormatoCurrency(total)));
        }
        //Finalmente creamos el panel de los totales
        //Panel Subtotal
        double dXPTot = 12;
        double dAnPTot = 12;
        double dAlPTot = 70;
        fnCrearPanelRedondeado(Pie, posPanelTotales - dXPTot, 0, Pie.rWidth - posPanelTotales + dAnPTot, tamCodigo - dAlPTot, grosorPen, radioCurva, false, ColorT);
        //Panel IVA
        double dYPIva = 20;
        fnCrearPanelRedondeado(Pie, posPanelTotales - dXPTot, dYPIva, Pie.rWidth - posPanelTotales + dAnPTot, tamCodigo - dAlPTot, grosorPen, radioCurva, false, ColorT);
        //Panel Total
        double dYPTot = 40;
        fnCrearPanelRedondeado(Pie, posPanelTotales - dXPTot, dYPTot, Pie.rWidth - posPanelTotales + dAnPTot, tamCodigo - dAlPTot, grosorPen, radioCurva, false, ColorT);
        double dYPVac = 61;
        double dYAltPVac = 62;
        Color c = new Color();
        c = System.Drawing.ColorTranslator.FromHtml("#333333");
        fnCrearPanelRedondeado(Pie, posPanelTotales - dXPTot, dYPVac, Pie.rWidth - posPanelTotales + dAnPTot, tamCodigo - dYAltPVac, grosorPen, radioCurva, true, c);

        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            //Mostramos forma de pago y texto importe en su panel correspondiente 
            Pie.Add(tamCodigo + horPadding, altoRenglon, new RepString(fPropNegrita, "Total con letra:"));
            fnAgregarMultilinea(Pie, fnTextoImporte(TransformacionLogo.fnFormatoRedondeo(total), moneda), fPropChica, tamCodigo + horPadding, altoRenglon * 1.7, 80, true);

            fnAgregarMultilinea(Pie, "Forma de Pago:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 3.2, 80, true);
            fnAgregarMultilinea(Pie, formaDePago, fPropChica, tamCodigo + horPadding, altoRenglon * 4, 80, true);

            if (version == "3.2") //Si es version 3.2 muestra el siguiente contenido
            {
                //Regimen Fiscal
                //if (!string.IsNullOrEmpty(Regimenfiscal))
                //{
                //    fnAgregarMultilinea(Pie, "Régimen Fiscal:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 5, 80, true);
                //    fnAgregarMultilinea(Pie, Regimenfiscal, fPropChica, tamCodigo + horPadding, altoRenglon * 6, 80, true);
                //}

                //Metodo de pago 
                if (!string.IsNullOrEmpty(metodoPago))
                {
                    fnAgregarMultilinea(Pie, "Método de Pago:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 5, 80, true);
                    fnAgregarMultilinea(Pie, metodoPago, fPropChica, tamCodigo + horPadding, altoRenglon * 6, 80, true);

                    //Número de cuenta
                    if (!string.IsNullOrEmpty(sNumCtaPago))
                    {
                        fnAgregarMultilinea(Pie, "Número de Cuenta:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 7, 80, true);
                        fnAgregarMultilinea(Pie, sNumCtaPago, fPropChica, tamCodigo + horPadding, altoRenglon * 8, 80, true);
                    }
                }
                else
                {
                    //Número de cuenta
                    if (!string.IsNullOrEmpty(sNumCtaPago))
                    {
                        fnAgregarMultilinea(Pie, "Número de Cuenta:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 5, 80, true);
                        fnAgregarMultilinea(Pie, sNumCtaPago, fPropChica, tamCodigo + horPadding, altoRenglon * 6, 80, true);
                    }
                }
            }
        }
        double dXPTotLet = 12;
        fnCrearPanelRedondeado(Pie, tamCodigo, 0, posPanelTotales - tamCodigo - dXPTotLet, tamCodigo, grosorPen, radioCurva, false, ColorT);

        //Estos datos estan debajo del CBB
        renglon = 1;
        if (nNumPag == nTotPag)//Si es ultima pagina
        {
            Pie.Add(horPadding, tamCodigo + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del Emisor:"));
            renglon += fnAgregarMultilinea(Pie, sello, fPropChica, horPadding, tamCodigo + altoRenglon * renglon, 130, false);

            Pie.Add(horPadding, tamCodigo + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del SAT:"));
            renglon += fnAgregarMultilinea(Pie, timbre, fPropChica, horPadding, tamCodigo + altoRenglon * renglon, 130, false);


            //Agregamos la cadena original y alargamos la sección del pie según sea necesario
            Pie.Add(horPadding, tamCodigo + altoRenglon * renglon++, new RepString(fPropNormal, "Cadena original del complemento de certificación digital del SAT:"));
            renglon = fnAgregarMultilinea(Pie, TransformacionLogo.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0")
                , fPropChica, horPadding, tamCodigo + altoRenglon * renglon, 130, false);

            renglon = 1;
            Pie.rHeight += renglon * (fPropChica.rSize * 1.2);
            altoPie = Pie.rHeightMM;
        }
        //dibujamos el borde del pie
        fnCrearPanelRedondeado(Pie, 0, tamCodigo, Pie.rWidth, Pie.rHeight - tamCodigo - altoRenglon, grosorPen, radioCurva, false, ColorT);

        //Agrega www.paxfacturacion.com
        fPropNormal = new FontProp(fuenteNormal, 4);
        Pie.AddAligned(Pie.rWidth / 7, RepObj.rAlignCenter, Pie.rHeight - verPadding, RepObj.rAlignTop, new RepString(fPropNormal, clsComunPDF.ObtenerParamentro("urlHostComercial")));

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
        StaticContainer Encabezado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoEncabezado));
        fnCrearPanelRedondeado(Encabezado, 0, 0, Encabezado.rWidth, Encabezado.rHeight - fPropBlanca.rSize * 2, grosorPen, radioCurva, false, sColor);

        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");


        XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
        double Renglon = 7.5;
        fnDatosEmisor(nsmComprobante, navEncabezado, Encabezado, sColor);
        fnDatosReceptor(nsmComprobante, navEncabezado, Encabezado, Renglon);
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
        double altoBarra = fPropBlanca.rSize * 2;
        fnCrearPanelRedondeado(Encabezado, 0, Encabezado.rHeight - altoBarra, Encabezado.rWidth, altoBarra, grosorPen, radioCurva, true, sColor);

        //Dibujamos los titulos del detalle
        //El ancho total del área es de 572 puntos
        double puntoMedio = Encabezado.rHeight - fPropBlanca.rSize;

        //Definimos la posicion de los titulos
        double posColumna1 = Encabezado.rWidth * 0.02;     //clave
        double posColumna2 = Encabezado.rWidth * 0.1;      //unidad
        double posColumna3 = Encabezado.rWidth * 0.39;     //descripcion
        double posColumna4 = Encabezado.rWidth * 0.68;     //cantidad
        double posColumna5 = Encabezado.rWidth * 0.81;     //precio
        double posColumna6 = Encabezado.rWidth * 0.92;     //importe

        Encabezado.AddAligned(posColumna1, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "CLAVE"));
        Encabezado.AddAligned(posColumna2, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "UNIDAD DE MEDIDA"));
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
    private void fnDatosEmisor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, System.Drawing.Color sColor)
    {
        string razonSocial, rfc, calle, noExterior, noInterior, colonia, municipio, estado, pais, codigoPostal, serie, folio, fecha, noCertificadoEmisor,
            referencia, Localidad, version, Regimenfiscal, fechaTimb, estadolug;
        razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = fecha = serie = folio = noCertificadoEmisor =
            referencia = Localidad = version = Regimenfiscal = fechaTimb = estadolug = string.Empty;

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
        //Se busca si contiene ExpedidoEn
        XPathNodeIterator xPathIt = navEncabezado.Select("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn",nsmComprobante);

        razonSocial = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value;
        rfc = "RFC: " + navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
      
        version = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;

        try { Regimenfiscal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:RegimenFiscal/@Regimen", nsmComprobante).Value; }
        catch { }
        //Si no contiene expedidoEn aplica datos emisor
        if (xPathIt.Count == 0)
        {
            //******************************Emisor****************************************************************************************************************
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

            municipio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@municipio", nsmComprobante).Value;

            pais = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@pais", nsmComprobante).Value;
            estado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@estado", nsmComprobante).Value;

            codigoPostal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@codigoPostal", nsmComprobante).Value;
            //*****************************************************************************************************************************************************
        }
        else
        {
            //Si contiene ExpedidoEn aplica datos de este mismo
            //***********************ExpedidoEn*********************************************************************************************************************
            calle = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@calle", nsmComprobante).Value;

            try { noExterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@noExterior", nsmComprobante).Value; }
            catch { }
            try { noInterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@noInterior", nsmComprobante).Value; }
            catch { }
            try { colonia = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@colonia", nsmComprobante).Value; }
            catch { }

            try { referencia = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@referencia", nsmComprobante).Value; }
            catch { }

            try { Localidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@localidad", nsmComprobante).Value; }
            catch { }

            municipio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@municipio", nsmComprobante).Value;

            pais = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@pais", nsmComprobante).Value;
            estado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@estado", nsmComprobante).Value;

            codigoPostal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@codigoPostal", nsmComprobante).Value;
            //******************************************************************************************************************************************************
        }
        if (version == "3.0")
            estadolug = pais + ", " + estado;
        else
        {
            try { estadolug = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", nsmComprobante).Value; }
            catch { }
        }
        
        noCertificadoEmisor = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value;

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
        if (!string.IsNullOrEmpty(Localidad))
        {
            ubicacion += ", " + municipio;
        }
        else
        {
            ubicacion += municipio;
        }


        ubicacion += ", ";
        string ubicacion2 = string.Empty;
        ubicacion2 += estado;
        ubicacion2 += ". " + pais;
        ubicacion2 += " C.P. " + codigoPostal;

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 5; //10
        double posRazon = fPropTitulo.rSize + sep;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;

        fPropTitulo.rSize = 5.7;
        fPropNormal.rSize = 5;
        fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);

        double dPX = 250;
        double dposRen = 0.5;
        double dcalRen = 0.8;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRazon - 3, RepObj.rAlignTop, new RepString(fPropTitulo, razonSocial));
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, direccion));
        dposRen += dcalRen;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, ubicacion));
        dposRen += dcalRen;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, ubicacion2));
        dposRen += dcalRen;
        Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, rfc));
        dposRen += dcalRen;

        string sRegFis = string.Empty;
        if (!string.IsNullOrEmpty(referencia))
        {
            string Refer = "Referencia: " + referencia;
            Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, Refer));
  
            if (version == "3.2")
            {
                dposRen += dcalRen;
                Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, "Regimen Fiscal: " + Regimenfiscal));
            }
        }
        else
        {
            if (version == "3.2")
            {
                Encabezado.AddAligned(leftPadding + dPX, RepObj.rAlignCenter, posRenglon + tamRenglon * dposRen, RepObj.rAlignTop, new RepString(fPropNormal, "Regimen Fiscal: " + Regimenfiscal));
            }
        }

        fPropNormal.rSize = 4;
        //Agregamos los paneles visuales para el tipo de documento, serie y folio
        double fAltoPanel = Encabezado.rHeight / 6;
        double fAnchoPanel = Encabezado.rWidth / 5;
        double posX = Encabezado.rWidth - fAnchoPanel;
        double dposXpnl = 134.8;
        double dAncPnl = 20;
                               
        fPropNormal = new FontProp(fuenteNormal, 4);
        fPropNegrita = new FontProp(fuenteNormal, 4, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, 4, Color.Red);
        double dYPos = 2.48;
        double dYPosP = 2.36;
        double dAltP = 8;
        double dAltPFol = 50;
        double dRen = 8;
        double dOpe = 2.4;
        fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - dposXpnl, fAltoPanel * dYPosP, fAnchoPanel + dAncPnl, dAltP, grosorPen, radioCurva, false, sColor);

        fnAgregarMultilineaCentrada(Encabezado, TipoDocumento, fPropNegrita, posX + fAnchoPanel / dOpe, fAltoPanel * dYPos, 10);

        dYPosP = 2.58;
        dAltPFol = 110;
        fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - dposXpnl, fAltoPanel * dYPosP, fAnchoPanel + dAncPnl, dAltPFol, grosorPen, radioCurva, false, sColor);

        string sUUID = string.Empty;
        string noCertificadoSAT = string.Empty;
        string sDateElaboracion = string.Empty;

        try { sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
        catch { }
        try { noCertificadoSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
        catch { }

        posX = Encabezado.rWidth - 2;

        dYPos = 25;
        //Folio Fiscal
        if (!string.IsNullOrEmpty(sUUID))
        {
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Folio Fiscal:"));
            dYPos += dRen;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNormal, sUUID.ToUpper()));
        }

        
        //Numero de Serie del Emisor
        if (!string.IsNullOrEmpty(noCertificadoEmisor))
        {
            dYPos += 10;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNegrita, "No. de Serie del Certificado del Emisor:"));
            dYPos += dRen;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNormal, noCertificadoEmisor));
        }

        //Numero de Serie del SAT
        if (!string.IsNullOrEmpty(noCertificadoSAT))
        {
            dYPos += 10;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNegrita, "No. de Serie del Certificado del SAT:"));
            dYPos += dRen;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNormal, noCertificadoSAT));
        }


        //Fecha de expedicion
        if (fechaTimbrado != DateTime.MinValue)//(fechaComprobante != DateTime.MinValue)
        {
            dYPos += 10;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Fecha y hora de certificación:"));
            dYPos += dRen;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNormal, fechaTimbrado.ToString("s")));
        }

        sDateElaboracion = fechaComprobante.ToString("yyyy/MM/dd") + " T " + fechaComprobante.ToString("HH") + ":" + fechaComprobante.ToString("mm") + ":" + fechaComprobante.ToString("ss");//fechaComprobante.ToString("dd") + " de " + fechaComprobante.ToString("MMMM") + " de " + fechaComprobante.ToString("yyyy") + " T " + fechaComprobante.ToString("HH") +":"+ fechaComprobante.ToString("mm") +":"+ fechaComprobante.ToString("ss");
        //Lugar y Fecha
        if (fechaComprobante != DateTime.MinValue)
        {
            dYPos += 10;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Lugar, fecha y hora de emisión:"));
            dYPos += dRen;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNormal, estadolug + " " + sDateElaboracion));
        }

        //Serie y Folio
        if (!string.IsNullOrEmpty(serie) && !string.IsNullOrEmpty(folio))
        {
            dYPos += 10;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
                new RepString(fPropNegrita, "Serie y Folio:"));
            dYPos += dRen;
            Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    fAltoPanel * 2 + dYPos, RepObj.rAlignTop,
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

        //fPropNormal = new FontProp(fuenteNormal, 4);
        fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
        fPropNegrita.bBold = true;
        fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);

        double leftPadding = Encabezado.rWidth * 0.02;
        double sep = 5;//10
        double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
        double tamRenglon = fPropNormal.rSize + sep;
        double posRenglon = posRazon + sep;
        fPropNormal.rSize = 5.5;

        //Encabezado.Add(leftPadding, posRazon + tamRenglon, new RepString(fPropTitulo, razonSocial)); //posRazon + 30
        //Encabezado.Add(leftPadding, posRenglon + tamRenglon * 1.5, new RepString(fPropNormal, direccion)); //* 2
        //Encabezado.Add(leftPadding, posRenglon + tamRenglon * 2.3, new RepString(fPropNormal, ubicacion)); //*2.5
        //Encabezado.Add(leftPadding, posRenglon + tamRenglon * 3.1, new RepString(fPropNormal, rfc)); //*3

        fPropNormal.rSize = 5;

        //double Renglon = 0;
        Renglon += 0.5;
        double i = 0;
        i = fnAgregarMultilinea(Encabezado, razonSocial, fPropTitulo, leftPadding, posRenglon + tamRenglon * Renglon++, 75, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 1.5;
        else
            Renglon -= 1;

        i = fnAgregarMultilinea(Encabezado, direccion, fPropNormal, leftPadding, posRenglon + tamRenglon * Renglon, 90, true);
        Renglon += i;
        if (i > 1)
            Renglon -= 0.5;

        if (Renglon - 0.8 < Renglon)
        {
            i = fnAgregarMultilinea(Encabezado, ubicacion, fPropNormal, leftPadding, posRenglon + tamRenglon * (Renglon - 0.3), 90, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
        }
        else
        {
            i = fnAgregarMultilinea(Encabezado, ubicacion, fPropNormal, leftPadding, posRenglon + tamRenglon * (Renglon - 0.8), 90, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
        }
        if (Renglon - 1.2 < Renglon)
        {
            i = fnAgregarMultilinea(Encabezado, rfc, fPropNormal, leftPadding, posRenglon + tamRenglon * (Renglon - 0.5), 90, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
        }
        else
        {
            i = fnAgregarMultilinea(Encabezado, rfc, fPropNormal, leftPadding, posRenglon + tamRenglon * (Renglon - 1.2), 90, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;
        }
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
                            if (nUltimoEspacio <= 0)
                                nUltimoEspacio = psTexto.Substring(0, pnTamRenglon).Length;

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
        NumaletLogo parser = new NumaletLogo();
        parser.LetraCapital = true;

        switch (psMoneda)
        {
            case "MXN":
                parser.TipoMoneda = NumaletLogo.Moneda.Peso;
                break;
            case "USD":
                parser.TipoMoneda = NumaletLogo.Moneda.Dolar;
                break;
            case "XEU":
                parser.TipoMoneda = NumaletLogo.Moneda.Euro;
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
            clsErrorLogLogo.fnNuevaEntrada(ex, clsErrorLogLogo.TipoErroresLog.Referencia);
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
            clsErrorLogLogo.fnNuevaEntrada(ex, clsErrorLogLogo.TipoErroresLog.Referencia);
        }
        return new RepImage(ms, 40, 10);
    }


    /// <summary>
    /// Genera Imagen Cliente
    /// </summary>
    /// <returns></returns>
    private MemoryStream fnImagenCliente(string sNombreLogo)
    {
        MemoryStream ms = new MemoryStream();


        //string cadenaCon = System.Configuration.ConfigurationManager.ConnectionStrings["conControl"].ConnectionString;
        //SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(cadenaCon));
        //con.Open();

        try
        {

            string sImagen = HttpContext.Current.Server.MapPath("~/LogosPlantillas/" + sNombreLogo);
            System.Drawing.Image img = new Bitmap(sImagen);
            byte[] byteImage = imageToByteArray(img);
                                 
            ms = new MemoryStream(byteImage);

        }
        catch (Exception ex)
        {
            clsErrorLogLogo.fnNuevaEntrada(ex, clsErrorLogLogo.TipoErroresLog.Referencia);
        }
        return ms;
    }

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
    #endregion
    //===============================================================
}

/// <summary>
/// Clase encargada de mantener y manipular la información de los impuestos del comprobante, 
/// tanto para traslados como para retenciones
/// </summary>
public class ImpuestoLogo
{
    public string Nombre { get; set; }
    public string Tasa { get; set; }
    public string Importe { get; set; }

    //Esta propiedad retorna el texto del renglon a mostrar en el PDF
    public string TextoImpuesto
    {
        get
        {
            return Nombre + " " + TransformacionLogo.fnFormatoPorcentaje(Tasa) + " " + TransformacionLogo.fnFormatoCurrency(Importe);
        }
    }

    /// <summary>
    /// Crea una nueva instancia de Impuesto
    /// </summary>
    /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
    /// <param name="nsmComprobante"></param>
    public ImpuestoLogo(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
public class DetalleLogo
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
    public DetalleLogo(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
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
public class TransformacionLogo
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
            trans.Load(XmlReader.Create(new StringReader(clsComunLogo.ObtenerParamentro(psNombreArchivoXSLT))));
            XsltArgumentList args = new XsltArgumentList();
            trans.Transform(xml, args, ms);
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            sCadenaOriginal = sr.ReadToEnd();
        }
        catch (Exception ex)
        {
            clsErrorLogLogo.fnNuevaEntrada(ex, clsErrorLogLogo.TipoErroresLog.Datos);
        }

        return sCadenaOriginal;
    }
}

#region ClasesPlantillas
public class clsErrorLogMarybellLeParfum
{
    //private static clsInicioSesionUsuario gUsuario;

    public clsErrorLogMarybellLeParfum()
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

public class clsComunLogo
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
                clsErrorLogMarybellLeParfum.fnNuevaEntrada(ex, clsErrorLogMarybellLeParfum.TipoErroresLog.Conexion);
            }
        }
        return sRetorno;
    }
}

public sealed class NumaletLogo
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

    public NumaletLogo()
    {
        MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
        SeparadorDecimalSalida = SeparadorDecimalSalidaDefault;
        LetraCapital = LetraCapitalDefault;
        ConvertirDecimales = _convertirDecimales;
    }

    public NumaletLogo(Boolean ConvertirDecimales, String MascaraSalidaDecimal, String SeparadorDecimalSalida, Boolean LetraCapital)
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
        return NumaletLogo.ToString(Numero, CultureInfo.CurrentCulture);
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
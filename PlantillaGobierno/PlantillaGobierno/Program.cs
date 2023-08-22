using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.IO;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using Root.Reports;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

//Clase que genera la plantilla de Gobierno del Estado

namespace PlantillaGobierno
{
    class clsPlantillaGobierno
    {
        private XmlDocument gxComprobante;
        public Report PDF;
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
        private double altoPie = 105;
        private const double factorSeparador = 2;
        private const double grosorPen = 1;
        private const double radioCurva = 4;

        //Tamaños en puntos
        private const double margenPagina = 20;
        private const double anchoSeccion = anchoPagina - margenPagina * 2;
        private const double tamCodigo = 90;
        private const string leyendaPDF = "ESTE DOCUMENTO ES UNA REPRESENTACIÓN GRÁFICA DE UN CFDI";

        static void Main(string[] args)
        {
            XmlDocument xmlDOC = new XmlDocument();
            xmlDOC.Load(@"C:\PaxConectorNomina\prueba2.xml");
            clsPlantillaGobierno plantillaGob = new clsPlantillaGobierno(xmlDOC);
            plantillaGob.fnGenerarPdf("Black");
        }

        public clsPlantillaGobierno(XmlDocument pxComprobante)
        {
            gxComprobante = pxComprobante;
            XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            // gxComprobante.InsertBefore(xDec, gxComprobante.DocumentElement);
        }

        public void fnGenerarPdf(string scolor)
        {
            PdfFormatter formato = new PdfFormatter();
            PDF = new Report(formato);
            PDF.sAuthor = "CORPUS Facturacion";
            PDF.sTitle = "CFDI";

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

            List<DetalleGobierno> detallesNum = fnObtenerDetalles();

            int nTotPag = 0;
            int altoPie = 105;

            while (detallesNum.Count > 0)
            {
                StaticContainer Pie = fnContarCrearPie();
                fnContarPaginas(detallesNum);
                nTotPag += 1;
            }

            //Despues de haber calculado total de páginas se reinicia valor altoPie.
            altoPie = 105;

            //Obtenemos los detalles
            List<DetalleGobierno> detalles = fnObtenerDetalles();

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

                StaticContainer Encabezado = fnCrearEncabezado();
                StaticContainer panelIzquierdo = fnCrearPanelIzquierdo();
                StaticContainer panelDerecho = fnCrearPanelDerecho();
                StaticContainer panelContribuyente = fnCrearPanelContribuyente();

                altoPie = 105;

                StaticContainer Pie = fnCrearPanelPie();
                pagina.Add(margenPagina, margenPagina, Encabezado);
                pagina.Add(margenPagina + 10, margenPagina + 70, panelIzquierdo);
                pagina.Add(anchoPagina + 170, margenPagina + 70, panelDerecho);
                pagina.Add(margenPagina + 10, margenPagina + 200, panelContribuyente);
                pagina.Add(margenPagina + 170, RT.rPointFromMM(altoPagina) - margenPagina + 8, new RepString(fPropChica, leyendaPDF));
                pagina.Add(margenPagina + 50, RT.rPointFromMM(altoPagina) - margenPagina + 8, new RepString(fPropChica, "http://www.paxfacturacion.com/"));

                //Si es ultima pagina
                if (pagina.iPageNo == nTotPag)
                {
                    StaticContainer piePanelDetalle = fnCrearPieDeDetalle();
                    pagina.Add(margenPagina + 10, 500, piePanelDetalle);
                    pagina.Add(margenPagina + 10, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + (tamCodigo * 2) - 150, GenerarCodigoBidimensional());
                    pagina.Add(margenPagina + 10, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight + 45, Pie);
                }

                //Se agrega el numero de paginas en la pagina
                pagina.AddAligned(RT.rPointFromMM(anchoPagina) - margenPagina, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenPagina, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " / " + nTotPag));

                //Agregamos las imagenes al documento
                MemoryStream ms1 = new MemoryStream();
                ms1 = fnImagenCliente("logo2.jpg");

                MemoryStream ms2 = new MemoryStream();
                ms2 = fnImagenCliente("chvive.jpg");

                MemoryStream ms3 = new MemoryStream();
                ms3 = fnImagenCliente("logo_pax.jpg");

                //Creamos el área de detalle
                if (ms1.Length > 0)
                {
                    RepImage image1 = new RepImage(ms1, 50, 60);
                    pagina.Add(margenPagina + 10, margenPagina + 50, image1);

                    RepImage image2 = new RepImage(ms2, 60, 60);
                    pagina.Add(pagina.rWidth - 72, margenPagina + 50, image2);

                    RepImage image3 = new RepImage(ms3, 30, 10);
                    pagina.Add(margenPagina + 10, RT.rPointFromMM(altoPagina) - margenPagina + 10, image3);
                }

                pagina.Add(margenPagina + 10, margenPagina + Encabezado.rHeight + 90, fnCrearDetalle(detalles, ColorT));
                //verificamos si aún quedan detalles
                if (detalles.Count <= 0)
                    bSeguir = false;

            }

            RT.ViewPDF(PDF);
        }

        public StaticContainer fnCrearPanelIzquierdo()
        {
            StaticContainer panelIzquierdo = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            PenProp pen = new PenProp(PDF, 1, Color.Black);
            BrushProp brush = new BrushProp(PDF, Color.Black);

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelIzquierdo(nsmComprobante, navEncabezado, panelIzquierdo, Renglon);

            double pfRadioCurva = 10;
            double lArc = pfRadioCurva * 2;
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 200;
            double pfAlto = 115;

            //esquina superior izquierda
            panelIzquierdo.Add(pfPosX, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 180, 90));
            //esquina superior derecha
            panelIzquierdo.Add(pfPosX + pfAncho - lArc, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 270, 90));
            //esquina inferior izquierda
            panelIzquierdo.Add(pfPosX, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 90, 90));
            //esquina inferior derecha
            panelIzquierdo.Add(pfPosX + pfAncho - lArc, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 0, 90));
            //añadimos los bordes rectos
            //borde superior
            panelIzquierdo.Add(pfPosX + pfRadioCurva, pfPosY, new RepLine(pen, pfAncho - lArc, 0));
            //borde inferior
            panelIzquierdo.Add(pfPosX + pfRadioCurva, pfPosY + pfAlto, new RepLine(pen, pfAncho - lArc, 0));
            ////borde izquierdo
            panelIzquierdo.Add(pfPosX, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));
            ////borde derecho
            panelIzquierdo.Add(pfPosX + pfAncho, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));
            //Dibujando lineas separadoras
            panelIzquierdo.Add(pfPosX, pfPosY + 28, new RepLine(pen, 200, 0));
            panelIzquierdo.Add(pfPosX, pfPosY + 56, new RepLine(pen, 200, 0));
            panelIzquierdo.Add(pfPosX, pfPosY + 84, new RepLine(pen, 200, 0));
            //Escribiendo el texto de los titulos
            fPropNormal.bBold = true;
            panelIzquierdo.Add(5, 9, new RepString(fPropNormal, "Recaudación de Rentas:"));
            panelIzquierdo.Add(5, 36, new RepString(fPropNormal, "Caja:"));
            panelIzquierdo.Add(5, 64, new RepString(fPropNormal, "No. de Operación:"));
            panelIzquierdo.Add(5, 93, new RepString(fPropNormal, "Lugar, Fecha y Hora de Emisión:"));
            fPropNormal.bBold = false;

            return panelIzquierdo;
        }

        public StaticContainer fnCrearPanelDerecho()
        {
            StaticContainer panelDerecho = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            PenProp pen = new PenProp(PDF, 1, Color.Black);
            BrushProp brush = new BrushProp(PDF, Color.Black);

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelDerecho(nsmComprobante, navEncabezado, panelDerecho, Renglon);

            double pfRadioCurva = 10;
            double lArc = pfRadioCurva * 2;
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 200;
            double pfAlto = 115;

            //esquina superior izquierda
            panelDerecho.Add(pfPosX, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 180, 90));
            //esquina superior derecha
            panelDerecho.Add(pfPosX + pfAncho - lArc, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 270, 90));
            //esquina inferior izquierda
            panelDerecho.Add(pfPosX, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 90, 90));
            //esquina inferior derecha
            panelDerecho.Add(pfPosX + pfAncho - lArc, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 0, 90));
            //añadimos los bordes rectos
            //borde superior
            panelDerecho.Add(pfPosX + pfRadioCurva, pfPosY, new RepLine(pen, pfAncho - lArc, 0));
            //borde inferior
            panelDerecho.Add(pfPosX + pfRadioCurva, pfPosY + pfAlto, new RepLine(pen, pfAncho - lArc, 0));
            ////borde izquierdo
            panelDerecho.Add(pfPosX, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));
            ////borde derecho
            panelDerecho.Add(pfPosX + pfAncho, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));

            //Dibujando lineas separadoras
            panelDerecho.Add(pfPosX, pfPosY + 28, new RepLine(pen, 200, 0));
            panelDerecho.Add(pfPosX, pfPosY + 56, new RepLine(pen, 200, 0));
            panelDerecho.Add(pfPosX, pfPosY + 84, new RepLine(pen, 200, 0));

            //Escribiendo el texto de los titulos
            fPropNormal.bBold = true;
            panelDerecho.AddLT_MM(47, 1, new RepString(fPropNormal, "Folio Fiscal UUID:"));
            panelDerecho.AddLT_MM(43, 11, new RepString(fPropNormal, "No. de Serie del CSD:"));
            panelDerecho.AddLT_MM(33, 21, new RepString(fPropNormal, "No. de Serie del CSD del SAT:"));
            panelDerecho.AddLT_MM(33, 31, new RepString(fPropNormal, "Fecha y Hora de Certificación:"));
            fPropNormal.bBold = false;

            return panelDerecho;
        }

        public StaticContainer fnCrearPanelContribuyente()
        {
            StaticContainer panelContribuyente = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            PenProp pen = new PenProp(PDF, 1, Color.Black);
            BrushProp brush = new BrushProp(PDF, Color.Black);

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelContribuyente(nsmComprobante, navEncabezado, panelContribuyente, Renglon);

            double pfRadioCurva = 10;
            double lArc = pfRadioCurva * 2;
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 555;
            double pfAlto = 55;

            //esquina superior izquierda
            panelContribuyente.Add(pfPosX, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 180, 90));
            //esquina superior derecha
            panelContribuyente.Add(pfPosX + pfAncho - lArc, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 270, 90));
            //esquina inferior izquierda
            panelContribuyente.Add(pfPosX, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 90, 90));
            //esquina inferior derecha
            panelContribuyente.Add(pfPosX + pfAncho - lArc, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 0, 90));
            //añadimos los bordes rectos
            //borde superior
            panelContribuyente.Add(pfPosX + pfRadioCurva, pfPosY, new RepLine(pen, pfAncho - lArc, 0));
            //borde inferior
            panelContribuyente.Add(pfPosX + pfRadioCurva, pfPosY + pfAlto, new RepLine(pen, pfAncho - lArc, 0));
            ////borde izquierdo
            panelContribuyente.Add(pfPosX, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));
            ////borde derecho
            panelContribuyente.Add(pfPosX + pfAncho, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));

            //Dibujando linea separadoras
            panelContribuyente.Add(pfPosX, pfPosY + 12, new RepLine(pen, pfAncho, 0));

            //Texto
            fPropNormal.bBold = true;
            panelContribuyente.Add((pfAncho / 2) - 50, 11, new RepString(fPropNormal, "CONTRIBUYENTE"));
            panelContribuyente.Add(5, pfAlto / 2 - 3, new RepString(fPropNormal, "Nombre o Razón Social:"));
            panelContribuyente.Add(5, (pfAlto / 2) + 10, new RepString(fPropNormal, "RFC:"));
            panelContribuyente.Add(5, (pfAlto / 2) + 23, new RepString(fPropNormal, "Pago:"));
            fPropNormal.bBold = false;

            return panelContribuyente;
        }

        public StaticContainer fnCrearPanelPie()
        {
            StaticContainer panelPie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            PenProp pen = new PenProp(PDF, 1, Color.Black);
            BrushProp brush = new BrushProp(PDF, Color.Black);

            double verPadding = panelPie.rHeight * 0.02;
            double pfRadioCurva = 10;
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 555;
            double pfAlto = 110;
            double lArc = pfRadioCurva * 2;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelPie(nsmComprobante, navEncabezado, panelPie, Renglon);

            //esquina superior izquierda
            panelPie.Add(pfPosX, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 180, 90));
            //esquina superior derecha
            panelPie.Add(pfPosX + pfAncho - lArc, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 270, 90));
            //esquina inferior izquierda
            panelPie.Add(pfPosX, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 90, 90));
            //esquina inferior derecha
            panelPie.Add(pfPosX + pfAncho - lArc, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 0, 90));
            //añadimos los bordes rectos
            //borde superior
            panelPie.Add(pfPosX + pfRadioCurva, pfPosY, new RepLine(pen, pfAncho - lArc, 0));
            //borde inferior
            panelPie.Add(pfPosX + pfRadioCurva, pfPosY + pfAlto, new RepLine(pen, pfAncho - lArc, 0));
            ////borde izquierdo
            panelPie.Add(pfPosX, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));
            ////borde derecho
            panelPie.Add(pfPosX + pfAncho, pfPosY + pfRadioCurva, new RepLine(pen, 0, lArc - pfAlto));

            //Dibujando lineas separadoras horizontales
            panelPie.Add(0, 40, new RepLine(pen, pfAncho, 0));
            panelPie.Add(0, 73, new RepLine(pen, pfAncho, 0));
            //Texto
            fPropNormal.bBold = true;
            panelPie.Add(5, 10, new RepString(fPropNormal, "Sello Digital del CFDI:"));
            panelPie.Add(5, 50, new RepString(fPropNormal, "Sello Digital del SAT:"));
            panelPie.Add(5, 80, new RepString(fPropNormal, "Cadena original del complemento de certificacion digital del SAT:"));
            panelPie.Add(90, -90, new RepString(fPropNormal, "Forma de Pago:"));
            panelPie.Add(90, -80, new RepString(fPropNormal, "Metodo de Pago:"));
            fPropNormal.bBold = false;

            return panelPie;
        }

        public StaticContainer fnCrearPanelDetalle()
        {
            StaticContainer panelDetalle = new StaticContainer(555, 400);
            PenProp pen = new PenProp(PDF, 1, Color.Black);
            BrushProp brush = new BrushProp(PDF, Color.Black);

            double posColumna1 = panelDetalle.rWidth * 0.01;
            double posColumna2 = panelDetalle.rWidth * 0.10;
            double posColumna3 = panelDetalle.rWidth * 0.20;
            double posColumna4 = panelDetalle.rWidth * 0.70;
            double posColumna5 = panelDetalle.rWidth * 0.80;

            double pfRadioCurva = 10;
            double lArc = pfRadioCurva * 2;
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 555;
            double pfAlto = 220;

            //esquina superior izquierda
            panelDetalle.Add(pfPosX, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 180, 90));
            //esquina superior derecha
            panelDetalle.Add(pfPosX + pfAncho - lArc, pfPosY + lArc, new RepArc(pen, pfRadioCurva, 270, 90));
            //añadimos los bordes rectos
            //borde superior
            panelDetalle.Add(pfPosX + pfRadioCurva, pfPosY, new RepLine(pen, pfAncho - lArc, 0));
            ////borde izquierdo
            panelDetalle.Add(pfPosX, pfPosY + pfRadioCurva, new RepLine(pen, 0, -pfAlto + 10));
            ////borde derecho
            panelDetalle.Add(pfPosX + pfAncho, pfPosY + pfRadioCurva, new RepLine(pen, 0, -pfAlto + 10));
            //Linea inferior
            panelDetalle.Add(pfPosX, pfAlto, new RepLine(pen, pfAncho, 0));

            //Dibujando linea separadoras horizontales
            panelDetalle.Add(pfPosX, pfPosY + 12, new RepLine(pen, pfAncho, 0));
            panelDetalle.Add(pfPosX, pfPosY + 27, new RepLine(pen, pfAncho, 0));

            //Dibujando lineas separadoras verticales
            panelDetalle.Add(posColumna2 - 5, pfPosY + 12, new RepLine(pen, 0, 12 - pfAlto));
            panelDetalle.Add(posColumna3 - 5, pfPosY + 12, new RepLine(pen, 0, 12 - pfAlto));
            panelDetalle.Add(posColumna4 - 5, pfPosY + 12, new RepLine(pen, 0, 12 - pfAlto));
            panelDetalle.Add(posColumna5 - 5, pfPosY + 12, new RepLine(pen, 0, 12 - pfAlto));

            //Texto
            fPropNormal.bBold = true;
            panelDetalle.Add((pfAncho / 2) - 70, 11, new RepString(fPropNormal, "DESCRIPCIÓN DEL PAGO"));
            panelDetalle.AddAligned(posColumna1, RepObj.rAlignLeft, 25, RepObj.rAlignBottom, new RepString(fPropNormal, "CANT."));
            panelDetalle.AddAligned(posColumna2, RepObj.rAlignLeft, 25, RepObj.rAlignBottom, new RepString(fPropNormal, "U.M"));
            panelDetalle.AddAligned(posColumna3, RepObj.rAlignLeft, 25, RepObj.rAlignBottom, new RepString(fPropNormal, "CONCEPTO"));
            panelDetalle.AddAligned(posColumna4, RepObj.rAlignLeft, 25, RepObj.rAlignBottom, new RepString(fPropNormal, "P.U."));
            panelDetalle.AddAligned(posColumna5, RepObj.rAlignLeft, 25, RepObj.rAlignBottom, new RepString(fPropNormal, "IMPORTE"));
            fPropNormal.bBold = false;

            return panelDetalle;
        }

        public StaticContainer fnCrearPieDeDetalle()
        {
            StaticContainer panelPieDeDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);
            PenProp pen = new PenProp(PDF, 1, Color.Black);
            BrushProp brush = new BrushProp(PDF, Color.Black);

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelPieDeDetalle(nsmComprobante, navEncabezado, panelPieDeDetalle, Renglon);

            double pfRadioCurva = 10;
            double lArc = pfRadioCurva * 2;
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 555;
            double pfAlto = 40;

            panelPieDeDetalle.Add(pfPosX, pfPosY, new RepLine(pen, pfAncho, 0));

            //esquina inferior izquierda
            panelPieDeDetalle.Add(pfPosX, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 90, 90));
            //esquina inferior derecha
            panelPieDeDetalle.Add(pfPosX + pfAncho - lArc, pfPosY + pfAlto, new RepArc(pen, pfRadioCurva, 0, 90));
            //añadimos los bordes rectos
            //borde superior
            panelPieDeDetalle.Add(pfPosX + pfRadioCurva, pfPosY, new RepLine(pen, pfAncho - lArc, 0));
            //borde inferior
            panelPieDeDetalle.Add(pfPosX + pfRadioCurva, pfPosY + pfAlto, new RepLine(pen, pfAncho - lArc, 0));
            ////borde izquierdo
            panelPieDeDetalle.Add(pfPosX, pfPosY, new RepLine(pen, 0, pfRadioCurva - pfAlto));
            ////borde derecho
            panelPieDeDetalle.Add(pfPosX + pfAncho, pfPosY, new RepLine(pen, 0, pfRadioCurva - pfAlto));

            //Dibujando linea separadoras horizontales
            panelPieDeDetalle.Add(0, pfPosY + 12, new RepLine(pen, pfAncho, 0));
            panelPieDeDetalle.Add(pfAncho - 172, pfAlto - 15, new RepLine(pen, 172, 0));

            //Dibujando lineas separadoras verticales
            panelPieDeDetalle.Add(pfPosX + 383, pfPosY, new RepLine(pen, 0, -pfAlto));
            panelPieDeDetalle.Add(pfAncho - 116, pfPosY, new RepLine(pen, 0, -pfAlto));
            //Texto
            fPropNormal.bBold = true;
            panelPieDeDetalle.Add(5, pfAlto - 30, new RepString(fPropNormal, "IMPORTE CON LETRA"));
            panelPieDeDetalle.Add(pfAncho - 170, pfAlto - 30, new RepString(fPropNormal, "SUBTOTAL:"));
            panelPieDeDetalle.Add(pfAncho - 170, pfAlto - 18, new RepString(fPropNormal, "IVA:"));
            panelPieDeDetalle.Add(pfAncho - 170, pfAlto - 6, new RepString(fPropNormal, "TOTAL:"));
            fPropNormal.bBold = false;

            return panelPieDeDetalle;
        }

        private StaticContainer fnContarCrearPie()
        {
            StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));
            return Pie;
        }

        private StaticContainer fnCrearDetalle(List<DetalleGobierno> detalles, System.Drawing.Color sColor)
        {
            StaticContainer areaDetalle = fnCrearPanelDetalle();
            DetalleGobierno[] copiaDetalles = detalles.ToArray();
            DetalleGobierno d;
            double posRenglon;
            double altoRenglon = fPropNormal.rSize * factorSeparador;
            int renglonActual = 1;
            int rengTotal = 0;

            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)((areaDetalle.rHeight / (fPropNormal.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones;

            if (maxConceptos > 16)
                maxConceptos = 16;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.01;
            double posColumna2 = areaDetalle.rWidth * 0.10;
            double posColumna3 = areaDetalle.rWidth * 0.20;
            double posColumna4 = areaDetalle.rWidth * 0.70;
            double posColumna5 = areaDetalle.rWidth * 0.80;

            int renglones = 0;
            double a = 0;
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
                    renglones = fnContarMultilinea(areaDetalle, d.descripcion, fPropNormal, posColumna2, posRenglon, 42, true);
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
                                foreach (ComprobanteImpuestosRetencionTGobierno retencion in d.RetencionesT)
                                {
                                    switch (retencion.impuesto)
                                    {
                                        case ComprobanteImpuestosRetencionImpuestoTGobierno.IVA: sDetalleTerceros.Append("IVA Ret: "); break;
                                        case ComprobanteImpuestosRetencionImpuestoTGobierno.ISR: sDetalleTerceros.Append("ISR: "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionGobierno.fnFormatoCurrency(retencion.importe.ToString()) + " ");
                                }
                            }

                            if (d.TrasladosT != null && d.TrasladosT.Count > 0)
                            {
                                foreach (ComprobanteImpuestosTrasladoTGobierno traslado in d.TrasladosT)
                                {
                                    switch (traslado.impuesto)
                                    {
                                        case ComprobanteImpuestosTrasladoImpuestoTGobierno.IVA: sDetalleTerceros.Append("IVA Tras "); break;
                                        case ComprobanteImpuestosTrasladoImpuestoTGobierno.IEPS: sDetalleTerceros.Append("IEPS "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionGobierno.fnFormatoPorcentaje(traslado.tasa.ToString()) + ": ");
                                    sDetalleTerceros.Append(TransformacionGobierno.fnFormatoCurrency(traslado.importe.ToString()) + " ");
                                }
                            }
                        }

                        Col3 = fnAgregarMultilineaDetalle(areaDetalle, sDetalleTerceros.ToString(), fPropNormal, posColumna3, posRenglon, 42, true);
                    }

                }
                else
                {

                    Col1 = fnAgregarMultilinea(areaDetalle, d.noIdentificacion, fPropNormal, posColumna1, posRenglon + 27, 9, true);
                    areaDetalle.AddAligned(posColumna2, RepObj.rAlignLeft, posRenglon + 27, RepObj.rAlignBottom, new RepString(fPropNormal, d.unidad));
                    areaDetalle.AddAligned(posColumna3, RepObj.rAlignLeft, posRenglon + 27, RepObj.rAlignBottom, new RepString(fPropNormal, d.descripcion));
                    areaDetalle.AddAligned(posColumna4, RepObj.rAlignLeft, posRenglon + 27, RepObj.rAlignBottom, new RepString(fPropNormal, d.cantidad));
                    areaDetalle.AddAligned(posColumna5, RepObj.rAlignLeft, posRenglon + 27, RepObj.rAlignBottom, new RepString(fPropNormal, d.importe));
                }

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                maxConceptos -= 1;
                detalles.Remove(d);
            }

            return areaDetalle;
        }

        private StaticContainer fnContarPaginas(List<DetalleGobierno> detalles)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);
            DetalleGobierno[] copiaDetalles = detalles.ToArray();
            DetalleGobierno d;
            double posRenglon;
            double altoRenglon = fPropNormal.rSize * factorSeparador;
            int renglonActual = 1;
            int rengTotal = 0;

            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(areaDetalle.rHeight / (fPropNormal.rSize + 2) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones);

            if (maxConceptos > 16)
                maxConceptos = 16;

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

                    if (!String.IsNullOrEmpty(d.aduana))
                        Col3 = fnAgregarMultilineaDetalle(areaDetalle, "Nombre Aduana: " + d.aduana + "; " + "N° Documento: " + d.NumAduana + "; " + "Fecha: " + Convert.ToDateTime(d.Fecha).ToShortDateString(), fPropNormal, posColumna3, posRenglon, 42, true);

                    //Complemento concepto terceros
                    if (!String.IsNullOrEmpty(d.RfcT))
                    {
                        areaDetalle.AddAligned(posColumna3, RepObj.rAlignLeft, posRenglon, RepObj.rAlignCenter, new RepString(fPropNegrita, "Información por cuenta de terceros: "));
                        StringBuilder sDetalleTerceros = new StringBuilder();
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
                                foreach (ComprobanteImpuestosRetencionTGobierno retencion in d.RetencionesT)
                                {
                                    switch (retencion.impuesto)
                                    {
                                        case ComprobanteImpuestosRetencionImpuestoTGobierno.IVA: sDetalleTerceros.Append("IVA Ret: "); break;
                                        case ComprobanteImpuestosRetencionImpuestoTGobierno.ISR: sDetalleTerceros.Append("ISR: "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionGobierno.fnFormatoCurrency(retencion.importe.ToString()) + " ");
                                }
                            }

                            if (d.TrasladosT != null && d.TrasladosT.Count > 0)
                            {
                                foreach (ComprobanteImpuestosTrasladoTGobierno traslado in d.TrasladosT)
                                {
                                    switch (traslado.impuesto)
                                    {
                                        case ComprobanteImpuestosTrasladoImpuestoTGobierno.IVA: sDetalleTerceros.Append("IVA Tras "); break;
                                        case ComprobanteImpuestosTrasladoImpuestoTGobierno.IEPS: sDetalleTerceros.Append("IEPS "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionGobierno.fnFormatoPorcentaje(traslado.tasa.ToString()) + ": ");
                                    sDetalleTerceros.Append(TransformacionGobierno.fnFormatoCurrency(traslado.importe.ToString()) + " ");
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

        private List<DetalleGobierno> fnObtenerDetalles()
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            List<DetalleGobierno> detalles = new List<DetalleGobierno>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();
            XPathNodeIterator navDetalles = navComprobante.Select("/cfdi:Comprobante/cfdi:Conceptos/cfdi:Concepto", nsmComprobante);

            while (navDetalles.MoveNext())
            {

                XPathNavigator nodenavigator = navDetalles.Current;
                if (nodenavigator.HasChildren)//Si contiene nodo hijo
                {

                    XPathNavigator navDetallesAduana = nodenavigator.SelectSingleNode("cfdi:InformacionAduanera", nsmComprobante);
                    XPathNavigator navComplTerceros = nodenavigator.SelectSingleNode("cfdi:ComplementoConcepto", nsmComprobante);
                    if (navComplTerceros != null)
                    {
                        XmlNamespaceManager nsmComprobanteComplTerceros = new XmlNamespaceManager(gxComprobante.NameTable);
                        nsmComprobanteComplTerceros.AddNamespace("terceros", "http://www.sat.gob.mx/terceros");

                        //Obtiene información del nodo cuenta de terceros si existe
                        XPathNavigator navTerceros = navComplTerceros.SelectSingleNode("terceros:PorCuentadeTerceros", nsmComprobanteComplTerceros);
                    }
                }
                detalles.Add(new DetalleGobierno(navDetalles.Current, nsmComprobante));
            }

            return detalles;
        }

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

        private MemoryStream fnImagenCliente(string sNombreLogo)
        {
            MemoryStream ms = new MemoryStream();

            try
            {
                string sImagen = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.LastIndexOf("\\bin"))  +  @"\Imagenes\" + sNombreLogo;
                System.Drawing.Image img = new Bitmap(sImagen);
                byte[] byteImage = imageToByteArray(img);
                ms = new MemoryStream(byteImage);
            }
            catch (Exception ex)
            {
                //CODIGO DE ESCRIBIR EN LOG VA AQUI
            }
            return ms;
        }



        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
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

        private int fnAgregarMultilineaCentrado(StaticContainer pContenedor, string psTexto, FontProp pFuente, double pdPosX, double pdPosY, int pnTamRenglon, bool bBuscarEspacio)
        {
            double nAlturaRenglon = pFuente.rSize * 1.2;
            int nNoRenglon = 0;

            foreach (RepString r in fnDividirRenglones(psTexto, pnTamRenglon, pFuente, bBuscarEspacio))
            {
                pContenedor.AddCB(pdPosY, r);
                nNoRenglon++;
            }

            return nNoRenglon;
        }

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

        private StaticContainer fnCrearEncabezado()
        {
            StaticContainer Encabezado = new StaticContainer(560, RT.rPointFromMM(altoEncabezado));
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            try { nsmComprobante.AddNamespace("donat", "http://www.sat.gob.mx/donat"); }
            catch { }

            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosEmisor(nsmComprobante, navEncabezado, Encabezado, ref Renglon);

            return Encabezado;
        }

        private void fnDatosPanelIzquierdo(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panelIzquierdo, double Renglon)
        {
            string sRecaudacion, sCaja, sOperacion, sFechaEmision, sLugarExpedicion, sHoraExpedicion;
            sRecaudacion = sCaja = sOperacion = sFechaEmision = sLugarExpedicion = sHoraExpedicion = "PRUEBA";

            fPropNegrita = new FontProp(fuenteNormal, 6, Color.Black);
            fPropNegrita.bBold = true;
            double sep = 9.5;
            double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
            double tamRenglon = 28;
            double posRenglon = posRazon + sep;
            fPropNormal.rSize = 5;
            fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
            fPropNegrita.bBold = false;

            try { sRecaudacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
            catch { }

            try { sLugarExpedicion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", nsmComprobante).Value; }
            catch { }

            try
            {
                sFechaEmision = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value;
                string[] s = sFechaEmision.Split('T');
                sFechaEmision = s[0];
                sHoraExpedicion = s[1];
            }
            catch { }


            double i = 0;
            i = fnAgregarMultilinea(panelIzquierdo, sRecaudacion, fPropNormal, 5, posRenglon, 40, true);
            Renglon += i;
            //if (i > 1)
            //     Renglon -= 0.5;
            i = fnAgregarMultilinea(panelIzquierdo, sCaja, fPropNormal, 5, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            //  if (i > 1)
            //      Renglon -= 0.5;
            i = fnAgregarMultilinea(panelIzquierdo, sOperacion, fPropNormal, 5, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            //  if (i > 1)
            //      Renglon -= 0.5;
            i = fnAgregarMultilinea(panelIzquierdo, sLugarExpedicion + ", " + sFechaEmision + ", " + sHoraExpedicion, fPropNormal, 5, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;

        }

        private void fnDatosPanelDerecho(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panelDerecho, double Renglon)
        {
            string sUUID, sCSD, sCSDSAT, sFechaCertificacion, sHoraCertificacion;
            sUUID = sCSD = sCSDSAT = sFechaCertificacion = sHoraCertificacion = string.Empty;

            fPropNegrita = new FontProp(fuenteNormal, 6, Color.Black);
            fPropNegrita.bBold = true;
            double sep = 9.5;
            double posRazon = fPropTitulo.rSize + sep;
            double tamRenglon = 28;
            double posRenglon = posRazon + sep;
            fPropNormal.rSize = 5;
            fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
            fPropNegrita.bBold = false;

            try { sUUID = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
            catch { }

            try { sCSD = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
            catch { }

            try { sCSDSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
            catch { }

            try
            {
                sFechaCertificacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;
                string[] temp = sFechaCertificacion.Split('T');
                sFechaCertificacion = temp[0];
                sHoraCertificacion = temp[1];
            }
            catch { }

            double i = 0;
            i = fnAgregarMultilinea(panelDerecho, sUUID, fPropNormal, 60, posRenglon, 40, true);
            Renglon += i;
            //if (i > 1)
            //     Renglon -= 0.5;
            i = fnAgregarMultilinea(panelDerecho, sCSD, fPropNormal, 180, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            //  if (i > 1)
            //      Renglon -= 0.5;
            i = fnAgregarMultilinea(panelDerecho, sCSDSAT, fPropNormal, 115, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
            //  if (i > 1)
            //      Renglon -= 0.5;
            i = fnAgregarMultilinea(panelDerecho, sFechaCertificacion + ", " + sHoraCertificacion, fPropNormal, 125, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;
        }

        private void fnDatosPanelContribuyente(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panelContribuyente, double Renglon)
        {
            string sNombre, sRFC;
            sNombre = sRFC = string.Empty;

            fPropNegrita = new FontProp(fuenteNormal, 6, Color.Black);
            fPropNegrita.bBold = true;
            double leftPadding = 0.02;
            double sep = 9.5;
            double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
            double tamRenglon = 13;
            double posRenglon = posRazon + sep;
            fPropNormal.rSize = 5;
            fPropNegrita = new FontProp(fuenteNormal, 8, Color.Black);
            fPropNegrita.bBold = false;

            try { sNombre = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value; }
            catch { }

            try { sRFC = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value; }
            catch { }

            double i = 0;
            i = fnAgregarMultilinea(panelContribuyente, sNombre, fPropNormal, leftPadding + 90, posRenglon, 40, true);
            Renglon += i;
            //if (i > 1)
            //     Renglon -= 0.5;
            i = fnAgregarMultilinea(panelContribuyente, sRFC, fPropNormal, leftPadding + 25, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;

        }

        private void fnDatosPanelPieDeDetalle(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panelContribuyente, double Renglon)
        {
            string sTotal, sSubtotal, sIVA, sImporteConLetra;
            sTotal = sSubtotal = sIVA = sImporteConLetra = string.Empty;

            try
            {
                sTotal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value;
                sTotal = TransformacionGobierno.fnFormatoRedondeo(sTotal);
            }
            catch { }

            try
            {
                sSubtotal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsmComprobante).Value;
                sSubtotal = TransformacionGobierno.fnFormatoRedondeo(sSubtotal);
            }
            catch { }

            double IVA = Convert.ToDouble(sSubtotal) * .16;

            panelContribuyente.Add(5, 34, new RepString(fPropNormal, fnTextoImporte(sTotal, "MXN")));
            panelContribuyente.Add(555 - 105, 10, new RepString(fPropNormal, "$" + sSubtotal));
            panelContribuyente.Add(555 - 105, 22, new RepString(fPropNormal, "$" + IVA));
            panelContribuyente.Add(555 - 105, 34, new RepString(fPropNormal, "$" + sTotal));

        }

        private void fnDatosPanelPie(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panelPie, double Renglon)
        {
            string sSelloEmisor, sSelloDigitalSAT, sCadenaOriginal, sMetodoPago, sFormaPago;
            sSelloEmisor = sSelloDigitalSAT = sCadenaOriginal = sMetodoPago = sFormaPago = string.Empty;

            sSelloEmisor = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value;
            sFormaPago = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@formaDePago", nsmComprobante).Value;
            sMetodoPago = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@metodoDePago", nsmComprobante).Value;
            sSelloDigitalSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value;

            fPropNegrita = new FontProp(fuenteNormal, 6, Color.Black);
            fPropNegrita.bBold = true;
            double sep = 9.5;
            double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
            double tamRenglon = 16;
            double posRenglon = posRazon + sep;
            double verPadding = panelPie.rHeight * 0.01;
            double horPadding = panelPie.rWidth * 0.01;
            fPropNormal.rSize = 5;
            fPropNegrita = new FontProp(fuenteNormal, 8, Color.Black);
            fPropNegrita.bBold = false;
            double altoRenglon = fPropNormal.rSize + verPadding;
            double i = 0;

            panelPie.Add(160, -90, new RepString(fPropNormal, sFormaPago));
            panelPie.Add(160, -80, new RepString(fPropNormal, sMetodoPago));

            i = fnAgregarMultilinea(panelPie, sSelloEmisor, fPropNormal, 5, posRenglon + tamRenglon * Renglon, 120, true);
            Renglon += i;

            i = fnAgregarMultilinea(panelPie, sSelloDigitalSAT, fPropNormal, 5, posRenglon + tamRenglon * Renglon, 120, true);
            Renglon += i;

            fnAgregarMultilinea(panelPie, TransformacionGobierno.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_3_2")
                 , fPropNormal, 5, posRenglon + tamRenglon * Renglon, 130, true);



        }

        //NO ESTOY UTILIZANDO ESTE METODO POR EL MOMENTO
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

            bool bBanderaAddenda = false;
            string sNombreEnvio = "", sdireccionEnvio = "", sLocalidad = "", sPais = "", sCP = "", staxid = "", sEstado = "";
            XPathNavigator navFoxconn = null;
            try
            {
                XmlDocument xdFoxconn = new XmlDocument();
                //xdFoxconn.LoadXml(gaAddenda[0].InnerXml);
                navFoxconn = xdFoxconn.CreateNavigator();
                bBanderaAddenda = true;
            }
            catch
            {

            }

            if (bBanderaAddenda)
            {
                /* No me sirve por el momento en lo que se define el XML
                 * try
                 { sNombreEnvio = navFoxconn.SelectSingleNode("/Foxconn/EnvioA/@nombre").Value; }
                 catch { }

                 try
                 { sdireccionEnvio = navFoxconn.SelectSingleNode("/Foxconn/DireccionEnvio/@POBox").Value; }
                 catch { }

                 try
                 { sdireccionEnvio = navFoxconn.SelectSingleNode("/Foxconn/DireccionEnvio/@calle").Value; }
                 catch { }

                 try
                 { sLocalidad = navFoxconn.SelectSingleNode("/Foxconn/DireccionEnvio/@localidad").Value; }
                 catch { }

                 try
                 { sPais = navFoxconn.SelectSingleNode("/Foxconn/DireccionEnvio/@pais").Value; }
                 catch { }

                 try
                 { sCP = navFoxconn.SelectSingleNode("/Foxconn/DireccionEnvio/@codigoPostal").Value; }
                 catch { }

                 try
                 { staxid = navFoxconn.SelectSingleNode("/Foxconn/TaxID/@taxid").Value; }
                 catch { }

                 try
                 { sEstado = navFoxconn.SelectSingleNode("/Foxconn/DireccionEnvio/@estado").Value; }
                 catch { }
                 **/
            }
            else
            {


            }

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

            fPropNegrita = new FontProp(fuenteNormal, 6, Color.Black);
            fPropNegrita.bBold = true;
            fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);

            double leftPadding = Encabezado.rWidth * 0.02;
            double sep = 5;//10
            double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
            double tamRenglon = fPropNormal.rSize + sep;
            double posRenglon = posRazon + sep;
            fPropNormal.rSize = 5;

            double nRenglon = Encabezado.rHeight * 0.47;

            nRenglon += 1 + fPropNegrita.rSize;
            fnAgregarMultilinea(Encabezado, razonSocial, fPropNormal, Encabezado.rWidth * 0.05, nRenglon, 46, true);

            nRenglon += 7 + fPropNegrita.rSize;
            fnAgregarMultilinea(Encabezado, direccion, fPropNormal, Encabezado.rWidth * 0.05, nRenglon, 96, true);

            nRenglon += 1 + fPropNegrita.rSize;
            fnAgregarMultilinea(Encabezado, estado + " " + pais + " C.P. " + codigoPostal, fPropNormal, Encabezado.rWidth * 0.05, nRenglon, 96, true);

            if (rfc.Equals("XEXX010101000"))
            {
                nRenglon += 1 + fPropNegrita.rSize;
                fnAgregarMultilinea(Encabezado, staxid, fPropNormal, Encabezado.rWidth * 0.05, nRenglon, 96, true);
            }
            nRenglon += 1 + fPropNegrita.rSize;
            fnAgregarMultilinea(Encabezado, "RFC: " + rfc, fPropNormal, Encabezado.rWidth * 0.05, nRenglon, 96, true);

            double nAnchoRenglon = Encabezado.rHeight * 0.47;
            double nRnglon = 1;

            nAnchoRenglon += fPropNegrita.rSize * nRnglon + 0.5;
            nRnglon = fnAgregarMultilinea(Encabezado, sNombreEnvio, fPropChica, Encabezado.rWidth * 0.35, nAnchoRenglon, 40, true);

            nAnchoRenglon += fPropNegrita.rSize * nRnglon + 7;
            nRnglon = fnAgregarMultilinea(Encabezado, sdireccionEnvio, fPropChica, Encabezado.rWidth * 0.35, nAnchoRenglon, 46, true);

            nAnchoRenglon += fPropNegrita.rSize * nRnglon + 1;
            nRnglon = fnAgregarMultilinea(Encabezado, sLocalidad + ", " + sEstado + ", " + sPais + " " + sCP, fPropChica, Encabezado.rWidth * 0.35, nAnchoRenglon, 100, true);

            nAnchoRenglon += fPropNegrita.rSize * nRnglon + 1;
            nRnglon = fnAgregarMultilinea(Encabezado, sEstado + ", " + sPais + " " + sCP, fPropChica, Encabezado.rWidth * 0.35, nAnchoRenglon, 100, true);


            if (rfc.Equals("XEXX010101000"))
            {
                nAnchoRenglon += fPropNegrita.rSize * nRnglon + 1;
                nRnglon = fnAgregarMultilinea(Encabezado, staxid, fPropChica, Encabezado.rWidth * 0.35, nAnchoRenglon, 96, true);
            }

            fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
            fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
            fPropNegrita.bBold = true;
            fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);
        }

        private void fnDatosEmisor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, ref double Renglon)
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
            rfc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value;
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

            string ubicacion2 = string.Empty;
            ubicacion2 += " C.P. " + codigoPostal;
            ubicacion2 += " " + estado + " " + pais;

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

            string exUbicacion2 = string.Empty;
            exUbicacion2 += exEstado;
            exUbicacion2 += ". " + exPais;
            exUbicacion2 += " C.P. " + exCodigoPostal;

            double leftPadding = Encabezado.rWidth * 0.01;
            double sep = 3;
            double posRazon = fPropTitulo.rSize + sep;
            double tamRenglon = fPropNormal.rSize + sep;
            double posRenglon = posRazon + sep;

            fPropTitulo.rSize = 9;
            fPropNormal.rSize = 5;

            fPropNegrita = new FontProp(fuenteNormal, 6, Color.Black);
            fPropNegrita.bBold = true;
            fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);

            double i = 0;

            double nPosXEncabezado = (Encabezado.rWidth * 0.5);

            double nPosYEncabezado = 0;

            i = fnAgregarMultilineaCentrado(Encabezado, razonSocial, fPropNegrita, leftPadding + nPosXEncabezado, nPosYEncabezado, 100, true);
            fPropTitulo.rSize = 6;
            Renglon += 0.5;
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            string sSecrHacienda = "SECRETARIA DE HACIENDA";

            i = fnAgregarMultilineaCentrado(Encabezado, sSecrHacienda, fPropNormal, -leftPadding + nPosXEncabezado, nPosYEncabezado + tamRenglon * Renglon, 96, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;


            string sRFC = rfc;

            i = fnAgregarMultilineaCentrado(Encabezado, "RFC: " + sRFC, fPropNormal, -leftPadding + nPosXEncabezado, nPosYEncabezado + tamRenglon * Renglon, 96, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;


            string sDireccion = direccion + " C.P:" + exCodigoPostal;

            i = fnAgregarMultilineaCentrado(Encabezado, sDireccion, fPropNormal, -leftPadding + nPosXEncabezado, nPosYEncabezado + tamRenglon * Renglon, 96, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            if (i > 1)
                Renglon -= 0.5;

            string sRegFis = string.Empty;
            string sLugarExp = estado + ", " + pais;
            sRegFis = "Regimen Fiscal: " + Regimenfiscal;

            i = fnAgregarMultilineaCentrado(Encabezado, sLugarExp, fPropNormal, leftPadding + nPosXEncabezado, nPosYEncabezado + tamRenglon * Renglon, 96, true);
            Renglon += i;

            i = fnAgregarMultilineaCentrado(Encabezado, sRegFis, fPropNormal, leftPadding + nPosXEncabezado, nPosYEncabezado + tamRenglon * Renglon, 96, true);
            Renglon += i;
            if (i > 1)
                Renglon -= 0.5;

            double fAltoPanel = Encabezado.rHeight / 6;
            double fAnchoPanel = Encabezado.rWidth / 5;
            double posX = Encabezado.rWidth - fAnchoPanel;

            if (!(string.IsNullOrEmpty(noAutorizacion)))
            {
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
            string sDateElaboracion = string.Empty;

            posX = Encabezado.rWidth - leftPadding;

            fPropNormal = new FontProp(fuenteNormal, 4);
            fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
            fPropNegrita.bBold = true;
            fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);

            fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
            fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
            fPropNegrita.bBold = true;
            fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);

        }

        private void fnTotales(XmlNamespaceManager nsmComprobante, XPathNavigator navPie, StaticContainer Pie, int nNumPag, int nTotPag, string sColor)
        {
            System.Drawing.ColorConverter colConvert = new ColorConverter();
            System.Drawing.Color ColorT = new System.Drawing.Color();

            ColorT = (System.Drawing.Color)colConvert.ConvertFromString(sColor);

            string subtotal, total, moneda, sello, timbre, formaDePago, metodoPago, Regimenfiscal, version, sNumCtaPago, descuento, sTotalAddenda, sSalesOrder, sDelivery, sMetodoPagoAddenda;
            subtotal = total = moneda = sello = timbre = formaDePago = metodoPago = Regimenfiscal = version = sNumCtaPago = descuento = sTotalAddenda = sSalesOrder = sDelivery = sMetodoPagoAddenda = string.Empty;

            string sRetIva = "";

            XmlDocument xdGobierno = new XmlDocument();

            //xdGobierno.LoadXml(gaAddenda[0].InnerXml);

            XPathNavigator navGobierno = xdGobierno.CreateNavigator();

            try { sRetIva = navGobierno.SelectSingleNode("/Gobierno/Impuesto/@importe").Value; }
            catch { }

            List<ImpuestoGobierno> impuestos = new List<ImpuestoGobierno>();
            List<ImpuestoCompGobierno> impuestosComp = new List<ImpuestoCompGobierno>();

            try
            {
                subtotal = navPie.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsmComprobante).Value;
                total = navPie.SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value;
                timbre = navPie.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value;
                formaDePago = navPie.SelectSingleNode("/cfdi:Comprobante/@formaDePago", nsmComprobante).Value;
                sello = navPie.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value;
                version = navPie.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
            }
            catch { }
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
                    impuestos.Add(new ImpuestoGobierno(navImpuestos.Current, nsmComprobante));
                }

                navImpuestos = navPie.Select("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion", nsmComprobante);
                while (navImpuestos.MoveNext())
                {
                    impuestos.Add(new ImpuestoGobierno(navImpuestos.Current, nsmComprobante));
                }
            }

            //Complementos impuestos locales
            try
            {
                XPathNodeIterator navComplemento = navPie.Select("/cfdi:Comprobante/cfdi:Complemento/implocal:ImpuestosLocales/implocal:TrasladosLocales", nsmComprobante);

                while (navComplemento.MoveNext())
                {
                    impuestosComp.Add(new ImpuestoCompGobierno(navComplemento.Current, nsmComprobante));
                }

                navComplemento = navPie.Select("/cfdi:Comprobante/cfdi:Complemento/implocal:ImpuestosLocales/implocal:RetencionesLocales", nsmComprobante);

                while (navComplemento.MoveNext())
                {
                    impuestosComp.Add(new ImpuestoCompGobierno(navComplemento.Current, nsmComprobante));
                }
            }
            catch { }

            try { sTotalAddenda = navGobierno.SelectSingleNode("Gobierno/TotalTexto/@qty").Value; }
            catch { }
            try { sMetodoPagoAddenda = navGobierno.SelectSingleNode("Gobierno/MetodoPago/@metodoDePago").Value; }
            catch { }
            try { sSalesOrder = navGobierno.SelectSingleNode("Gobierno/OrdenVenta/@so").Value; }
            catch { }
            try { sDelivery = navGobierno.SelectSingleNode("Gobierno/Envio/@del").Value; }
            catch { }

            double verPadding = Pie.rHeight * 0.02;
            double horPadding = Pie.rWidth * 0.01;
            double posPanelTotales = Pie.rWidth - 180;
            if (nNumPag == nTotPag)
            {
                Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding, RepObj.rAlignTop, new RepString(fPropNormal, "SUBTOTAL"));
                Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionGobierno.fnFormatoCurrency(subtotal)));
            }
            double altoRenglon = fPropNormal.rSize + verPadding;
            int renglon = 1;
            string textoRenglon = string.Empty;
            if (nNumPag == nTotPag)
            {
                if (descuento != string.Empty)
                {
                    Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, "DESCUENTO"));
                    Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionGobierno.fnFormatoCurrency(descuento)));
                    renglon++;
                }
                foreach (ImpuestoGobierno i in impuestos)
                {
                    if (i.Nombre != "IEPS")
                    {
                        Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + TransformacionGobierno.fnFormatoPorcentaje(i.Tasa)));
                        Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionGobierno.fnFormatoCurrency(i.Importe)));
                        renglon++;
                    }
                }

                try
                {
                    //Agrega complemento impuestos locales 
                    foreach (ImpuestoCompGobierno i in impuestosComp)
                    {
                        Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + TransformacionGobierno.fnFormatoPorcentaje(i.Tasa)));
                        Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionGobierno.fnFormatoCurrency(i.Importe)));
                        renglon++;
                    }
                }
                catch { }

                //Agregamos el separador y el total
                Pie.Add(posPanelTotales + horPadding, tamCodigo - altoRenglon - verPadding, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));
                Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, tamCodigo - altoRenglon, RepObj.rAlignTop, new RepString(fPropNormal, "TOTAL" + " (" + moneda + ")"));
                Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, tamCodigo - altoRenglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionGobierno.fnFormatoCurrency(total)));
            }
            //Finalmente creamos el panel de los totales
            if (nNumPag == nTotPag)//Si es ultima pagina
            {
                double i = 0;
                double Renglon = 1;
                //Mostramos forma de pago y texto importe en su panel correspondiente 
                i = fnAgregarMultilinea(Pie, sTotalAddenda, fPropNormal, horPadding, altoRenglon * Renglon, 70, true);
                //Renglon += 0.5;
                Renglon += i;
                if (i > 1)
                    Renglon -= 0.5;

                Renglon += i;
                if (i > 1)
                    Renglon -= 0.5;

                if (version == "3.2")
                {
                    //Metodo de pago 
                    if (!string.IsNullOrEmpty(metodoPago))
                    {
                        //fnAgregarMultilinea(Pie, "Método de Pago:", fPropNegrita, horPadding + 140, altoRenglon * 3, 80, true);
                        i = fnAgregarMultilinea(Pie, sMetodoPagoAddenda, fPropChica, horPadding, altoRenglon * Renglon, 70, true);
                        Renglon += i;
                        if (i > 1)
                            Renglon -= 0.5;

                        //Número de cuenta
                        if (!string.IsNullOrEmpty(sNumCtaPago))
                        {
                            fnAgregarMultilinea(Pie, "Número de Cuenta:", fPropChica, horPadding, altoRenglon * Renglon, 17, true);
                            i = fnAgregarMultilinea(Pie, sNumCtaPago, fPropChica, horPadding + 65, altoRenglon * Renglon, 53, true);
                            Renglon += i;
                            if (i > 1)
                                Renglon -= 0.5;
                        }
                    }
                    else
                    {
                        //Número de cuenta
                        if (!string.IsNullOrEmpty(sNumCtaPago))
                        {
                            fnAgregarMultilinea(Pie, "Número de Cuenta:", fPropChica, horPadding, altoRenglon * Renglon, 17, true);
                            i = fnAgregarMultilinea(Pie, sNumCtaPago, fPropChica, horPadding + 65, altoRenglon * Renglon, 53, true);
                            Renglon += i;
                            if (i > 1)
                                Renglon -= 0.5;
                        }
                    }
                }

                i = fnAgregarMultilinea(Pie, sSalesOrder, fPropChica, horPadding + 40, altoRenglon * Renglon, 58, true);
                Renglon += i;
                if (i > 1)
                    Renglon -= 0.5;

                i = fnAgregarMultilinea(Pie, sDelivery, fPropChica, horPadding + 30, altoRenglon * Renglon, 61, true);
                Renglon += i;
                if (i > 1)
                    Renglon -= 0.5;
            }

            if (nNumPag == nTotPag)
            {
                //Sección de datos bancarios de la Addenda
                // fnDatosBancarios(Pie, navGobierno, horPadding, altoRenglon);
            }
            //Estos datos estan debajo del CBB
            renglon = 1;
            if (nNumPag == nTotPag)//Si es ultima pagina
            {
                Pie.Add(horPadding, (tamCodigo * 2) + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del Emisor:"));
                renglon += fnAgregarMultilinea(Pie, sello, fPropChica, horPadding, (tamCodigo * 2) + altoRenglon * renglon, 130, false);

                Pie.Add(horPadding, (tamCodigo * 2) + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del SAT:"));
                renglon += fnAgregarMultilinea(Pie, timbre, fPropChica, horPadding, (tamCodigo * 2) + altoRenglon * renglon, 130, false);

                //Agregamos la cadena original y alargamos la sección del pie según sea necesario
                Pie.Add(horPadding, (tamCodigo * 2) + altoRenglon * renglon, new RepString(fPropNormal, "Cadena original del complemento de certificación digital del SAT:"));
                try
                {
                    renglon = fnAgregarMultilinea(Pie, TransformacionGobierno.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_3_2")
                , fPropChica, horPadding, tamCodigo + altoRenglon * renglon, 130, false);
                }
                catch { }

                renglon = 1;
                Pie.rHeight += renglon * (fPropChica.rSize * 1.2);
                altoPie = Pie.rHeightMM;
            }
            //dibujamos el borde del pie
        }

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

        private string fnTextoImporte(string psValor, string psMoneda)
        {
            CultureInfo languaje;
            NumaletGobierno parser = new NumaletGobierno();
            parser.LetraCapital = true;

            switch (psMoneda)
            {
                case "MXN":
                    parser.TipoMoneda = NumaletGobierno.Moneda.Peso;
                    break;
                case "USD":
                    parser.TipoMoneda = NumaletGobierno.Moneda.Dolar;
                    break;
                case "XEU":
                    parser.TipoMoneda = NumaletGobierno.Moneda.Euro;
                    break;
            }

            languaje = new CultureInfo("es-Mx");

            return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
        }


    }

    public class TransformacionGobierno
    {
        /// <summary>
        /// Transforma un valor a su representación porcentual
        /// </summary>
        /// <param name="valor">cadena con el valor a ser transformado</param>
        /// <returns>Cadena con el nuevo valor porcentual</returns>
        public static string fnFormatoPorcentaje(string valor)
        {
            CultureInfo languaje;
            languaje = new CultureInfo("es-Mx");

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
            languaje = new CultureInfo("es-Mx");

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
            languaje = new CultureInfo("es-Mx");

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
                trans.Load(XmlReader.Create(new StringReader(Settings.Default[psNombreArchivoXSLT].ToString())));
                XsltArgumentList args = new XsltArgumentList();
                trans.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                sCadenaOriginal = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                //LOGO DE ERROR
            }

            return sCadenaOriginal;
        }
    }
    public class ImpuestoGobierno
    {
        public string Nombre { get; set; }
        public string Tasa { get; set; }
        public string Importe { get; set; }

        //Esta propiedad retorna el texto del renglon a mostrar en el PDF
        public string TextoImpuesto
        {
            get
            {
                return Nombre + " " + TransformacionGobierno.fnFormatoPorcentaje(Tasa) + " " + TransformacionGobierno.fnFormatoCurrency(Importe);
            }
        }

        /// <summary>
        /// Crea una nueva instancia de Impuesto
        /// </summary>
        /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
        /// <param name="nsmComprobante"></param>
        public ImpuestoGobierno(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
    public class ImpuestoCompGobierno
    {
        public string Nombre { get; set; }
        public string Tasa { get; set; }
        public string Importe { get; set; }

        //Esta propiedad retorna el texto del renglon a mostrar en el PDF
        public string TextoImpuesto
        {
            get
            {
                return Nombre + " " + TransformacionGobierno.fnFormatoPorcentaje(Tasa) + " " + TransformacionGobierno.fnFormatoCurrency(Importe);
            }
        }
        /// <summary>
        /// Crea una nueva instancia de Impuesto
        /// </summary>
        /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
        /// <param name="nsmComprobante"></param>
        public ImpuestoCompGobierno(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
    public class DetalleGobierno
    {
        private string sCantidad;
        CultureInfo languaje;

        public string cantidad
        {
            get { return sCantidad; }
            set
            {
                languaje = new CultureInfo("es-Mx");

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
                languaje = new CultureInfo("es-Mx"); ;
                sValorUnitario = Convert.ToDouble(value, languaje).ToString("c", languaje);
            }
        }

        private string sImporte;
        public string importe
        {
            get { return sImporte; }
            set
            {
                languaje = new CultureInfo("es-Mx");
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

        private List<ComprobanteImpuestosRetencionTGobierno> _retencionesT;
        public List<ComprobanteImpuestosRetencionTGobierno> RetencionesT
        {
            get { return _retencionesT; }
            set { _retencionesT = value; }
        }

        private List<ComprobanteImpuestosTrasladoTGobierno> _trasladosT;
        public List<ComprobanteImpuestosTrasladoTGobierno> TrasladosT
        {
            get { return _trasladosT; }
            set { _trasladosT = value; }
        }

        private t_UbicacionFiscalTGobierno _ubicacionFiscalT;
        public t_UbicacionFiscalTGobierno UbicacionFiscalT
        {
            get { return _ubicacionFiscalT; }
            set { _ubicacionFiscalT = value; }
        }

        private t_InformacionAduaneraTGobierno _informacionAduaneraT;
        public t_InformacionAduaneraTGobierno InformacionAduaneraT
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
        public DetalleGobierno(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
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

    #region clases auxiliares para el complemento de terceros en la clase Detalle

    public partial class ComprobanteImpuestosRetencionTGobierno
    {
        private ComprobanteImpuestosRetencionImpuestoTGobierno impuestoField;
        private decimal importeField;

        public ComprobanteImpuestosRetencionImpuestoTGobierno impuesto
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

    public enum ComprobanteImpuestosRetencionImpuestoTGobierno
    {
        ISR, IVA,
    }

    public partial class ComprobanteImpuestosTrasladoTGobierno
    {
        private ComprobanteImpuestosTrasladoImpuestoTGobierno impuestoField;
        private decimal tasaField;
        private decimal importeField;

        public ComprobanteImpuestosTrasladoImpuestoTGobierno impuesto
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

    public enum ComprobanteImpuestosTrasladoImpuestoTGobierno
    {
        IVA, IEPS,
    }

    public partial class t_UbicacionFiscalTGobierno
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

    public partial class t_InformacionAduaneraTGobierno
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

    public sealed class NumaletGobierno
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
        public NumaletGobierno()
        {
            MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
            SeparadorDecimalSalida = SeparadorDecimalSalidaDefault;
            LetraCapital = LetraCapitalDefault;
            ConvertirDecimales = _convertirDecimales;
        }

        public NumaletGobierno(Boolean ConvertirDecimales, String MascaraSalidaDecimal, String SeparadorDecimalSalida, Boolean LetraCapital)
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
            return NumaletGobierno.ToString(Numero, CultureInfo.CurrentCulture);
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

}

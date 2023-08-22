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
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using Root.Reports;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace cslPlantillaUnionProgresoCC
{
    class cslPlantillaUnionProgresoCC
    {
        private XmlDocument gxComprobante;
        public Report PDF;
        public string TipoDocumento { get; set; }
        public string sLogo;
        public double anchoLogo;
        public double altoLogo;
        public List<XmlDocument> datosCargosObjetados;
        public List<XmlDocument> datosMovimientosRealizados;
        public Queue<Page> paginasReporte;

        private FontDef fuenteTitulo;
        private FontProp fPropTitulo;
        private const double tamFuenteTitulo = 8;

        private FontDef fuenteNormal;
        private FontProp fPropNormal;
        private const double tamFuenteNormal = 6;

        private FontProp fPropChica;
        private const double tamFuenteChica = 4.5;

        private FontProp fPropBlanca;
        private FontProp fPropRoja;
        private FontProp fPropNegrita;

        private PenProp penGruesa;
        private const double tamPlumaGruesa = 3;

        private PenProp penMediana;
        private const double tamPlumaMediana = 2;

        private PenProp penDelgada;
        private const double tamPlumaDelgada = 1;

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

        double puntoY = margenPagina + 540; //Coordenada donde se dibujara el primer panel ;
        double limiteY = 700;  //Coordenada donde se marca el limite para los paneles dinamicos.

        static void Main(string[] args)
        {
            XmlDocument xmlDOC = new XmlDocument();
            xmlDOC.Load(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\cslPlantillaUnionProgresoCC\XML\xmlCC.xml");
            cslPlantillaUnionProgresoCC plantillaUnion = new cslPlantillaUnionProgresoCC(xmlDOC);
            plantillaUnion.fnGenerarPdf(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\clsPlantillaUnionProgresoPR\Imagenes\doge.jpg", @"C:\Users\Marco.Santana\Pictures\logo.jpg", "Akala");

        }

        public cslPlantillaUnionProgresoCC(XmlDocument pxComprobante)
        {
            gxComprobante = pxComprobante;
            XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
           
        }

        public void fnGenerarPdf(string sBanner, string sFooter, string sLogo)
        {

            if (sLogo.ToLower() == "akala")
            {
                this.sLogo = @"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\clsPlantillaUnionProgresoPR\Imagenes\AkalaLogo.png";
                this.anchoLogo = 150;
                this.altoLogo = 80;
            }
            else
            {
                this.sLogo = @"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\clsPlantillaUnionProgresoPR\Imagenes\Union.jpg";
                this.anchoLogo = 230;
                this.altoLogo = 80;
            }


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

            penGruesa = new PenProp(PDF, tamPlumaGruesa, Color.DarkRed);
            penMediana = new PenProp(PDF, tamPlumaMediana, Color.DarkRed);
            penDelgada = new PenProp(PDF, tamPlumaDelgada, Color.DarkRed);

            datosCargosObjetados = new List<XmlDocument>();
            datosMovimientosRealizados = new List<XmlDocument>();
            paginasReporte = new Queue<Page>();
            fnObtenerDatosCargosObjetados();
            fnObtenerDatosMovimientosRealizados();


            //Tamaño carta
            Page pagina = new Page(PDF);
            pagina.rWidthMM = anchoPagina;
            pagina.rHeightMM = altoPagina;
            paginasReporte.Enqueue(pagina);

            //Cargamos publicidad
            MemoryStream memPub = new MemoryStream();
            memPub = fnImagenCliente(sBanner);
            RepImage publicidad = new RepImage(memPub, 563, 100);
            pagina.Add(margenPagina, margenPagina + 250, publicidad);

            fnCrearPie(ref pagina, ref PDF);
            fnDibujaPanelCargosObjetados(ref puntoY, ref pagina, PDF);
            fnDibujarPanelMovimientos(ref puntoY, ref pagina, PDF);

            fnDibujarGrafica(ref puntoY, ref pagina, PDF);

            fnCrearUltimaPagina(PDF, sFooter, this.sLogo, this.anchoLogo, this.altoLogo);


            foreach (Page pag in paginasReporte)
            {

                if (pag.iPageNo != 1)
                {
                    Page currentPage = pag;
                    fnCrearEncabezado2(ref currentPage, ref PDF, this.sLogo, this.anchoLogo, this.altoLogo);
                }
                else 
                {
                    Page primeraPagina = paginasReporte.First();
                    StaticContainer panelContenido = fnCrearPanelContenido();
                    primeraPagina.Add(margenPagina, margenPagina + 80, panelContenido);
                    fnCrearEncabezado(ref primeraPagina, ref PDF, this.sLogo, this.anchoLogo, this.altoLogo);
                }
             

            }

                RT.ViewPDF(PDF);
        }

        private StaticContainer fnCrearPanelContenido()
        {
            StaticContainer panelContenido = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            fnCrearPanelCliente(panelContenido);
            fnCrearPanelGAT(panelContenido);
            fnCrearLeyendas(panelContenido);
            fnCrearPanelCuenta(panelContenido);
            fnCrearPanelDesgloseIntereses(panelContenido);
            fnCrearPanelComportamiento(panelContenido);
            fnCrearPanelDesgloseSaldo(panelContenido);
            fnCrearPanelComisiones(panelContenido);

            return panelContenido;

        }

        private StaticContainer fnCrearPanelPie()
        {
            StaticContainer panelPie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelPie(nsmComprobante, navEncabezado, panelPie, Renglon);

            return panelPie;
        }

        private void fnDatosPanelPie(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sInformacionEmpresa = string.Empty;

            try { sInformacionEmpresa = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@InformacionEmpresa", nsmComprobante).Value; }
            catch { }

            double leftPadding = 0.02;
            double sep = 2.5;
            double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
            double posRenglon = 10;
            double i = 0;
            i = fnAgregarMultilinea(panel, sInformacionEmpresa, fPropChica, 70, 0, 120, true);
            

        }

        private void fnCrearEncabezado(ref Page pagina, ref Report PDF, string sLogo, double anchoLogo, double altoLogo)
        {
            MemoryStream ms1 = new MemoryStream();
            ms1 = fnImagenCliente(sLogo);
            RepImage logo = new RepImage(ms1, anchoLogo, altoLogo);
            pagina.Add(margenPagina, margenPagina + 70, logo);

            StaticContainer panelEstado = fnCrearPanelEstado(pagina.iPageNo);
            pagina.Add(330, margenPagina + 25, panelEstado);

        }

        private void fnCrearPie(ref Page pagina, ref Report PDF)
        {
            StaticContainer pie = fnCrearPanelPie();
            pagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - 8, pie);
        }

        private void fnCrearEncabezado2(ref Page pagina, ref Report PDF, string sLogo, double anchoLogo, double altoLogo)
        {
            MemoryStream ms1 = new MemoryStream();
            ms1 = fnImagenCliente(sLogo);
            RepImage logo = new RepImage(ms1, anchoLogo, altoLogo);
            pagina.Add(margenPagina, margenPagina + 70, logo);

            StaticContainer panelEstado = fnCrearPanelEstado2(pagina.iPageNo);
            pagina.Add(330, margenPagina + 25, panelEstado);
        }

        private StaticContainer fnCrearPanelEstado2(int nNumeroPagina)
        {
            StaticContainer panelEstado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            FontProp fPropRojoObscuro = new FontProp(fuenteNormal, 10);
            fPropRojoObscuro.color = Color.DarkRed;
            fPropRojoObscuro.bBold = true;

            string sCliente, sCuenta;
            sCliente = sCuenta = string.Empty;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            try { sCliente = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@CodigoCliente", nsmComprobante).Value; }
            catch { }

            try { sCuenta = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@NumeroCuenta", nsmComprobante).Value; }
            catch { }

            fPropChica.bBold = true;
            panelEstado.AddAligned(245, RepObj.rAlignRight, -3, RepObj.rAlignBottom, new RepString(fPropRojoObscuro, "Estado de Cuenta"));
            panelEstado.AddAligned(245, RepObj.rAlignRight, 10, RepObj.rAlignBottom, new RepString(fPropChica, "No. de Cuenta: " + sCuenta));
            panelEstado.AddAligned(245, RepObj.rAlignRight, 22, RepObj.rAlignBottom, new RepString(fPropChica, "No. de Cliente: " + sCliente));
            panelEstado.AddAligned(245, RepObj.rAlignRight, 34, RepObj.rAlignBottom, new RepString(fPropChica, "Página " + nNumeroPagina + " de " + paginasReporte.Count()));
            fPropChica.bBold = false;
            return panelEstado;

        }

        private StaticContainer fnCrearPanelEstado(int nNumeroPagina)
        {
            StaticContainer panelEstado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            FontProp fPropRojoObscuro = new FontProp(fuenteNormal, 10);
            fPropRojoObscuro.color = Color.DarkRed;
            fPropRojoObscuro.bBold = true;

            string sPeriodoInicio, sPeriodoFin, sFechaExp, sPagina;
            sPeriodoInicio = sPeriodoFin = sFechaExp = sPagina = string.Empty;
            sPagina = Convert.ToString(nNumeroPagina);
            
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            try
            {
                string temp1 = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@PeriodoInicio", nsmComprobante).Value;
                string temp2 = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@PeriodoFin", nsmComprobante).Value;
                string[] aTemp = temp1.Split('T');
                string[] aTemp2 = temp2.Split('T');
                sPeriodoInicio = aTemp[0];
                sPeriodoFin = aTemp2[0];
            }
            catch { }

            try
            {
                sFechaExp = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@FechaExpedicion", nsmComprobante).Value;
                string[] temp = sFechaExp.Split('T');
                sFechaExp = temp[0];

            }
            catch { }

            sPeriodoInicio = sPeriodoInicio.Replace('-', '/');
            sPeriodoFin = sPeriodoFin.Replace('-', '/');
            sFechaExp = sFechaExp.Replace('-','/');


            fPropChica.bBold = true;
            panelEstado.AddAligned(245, RepObj.rAlignRight, -3, RepObj.rAlignBottom, new RepString(fPropRojoObscuro,"Estado de Cuenta"));
            panelEstado.AddAligned(245, RepObj.rAlignRight, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Período Desde " + sPeriodoInicio +" Hasta " + sPeriodoFin));
            panelEstado.AddAligned(245, RepObj.rAlignRight, 22, RepObj.rAlignBottom, new RepString(fPropChica, "Fecha Expedición " + sFechaExp));
            panelEstado.AddAligned(245, RepObj.rAlignRight, 34, RepObj.rAlignBottom, new RepString(fPropChica, "Página " + nNumeroPagina +" de " + paginasReporte.Count()));
            fPropChica.bBold = false;

            return panelEstado;
        }

        private void fnCrearPanelGAT(StaticContainer panelContenido)
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelGAT(nsmComprobante, navEncabezado, panelContenido, Renglon);

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 200;
            double pfAlto = 50;
            //borde superior
            panelContenido.Add(360, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelContenido.Add(360, pfAlto, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelContenido.Add(360, pfPosY, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelContenido.Add(360 + pfAncho, pfPosY, new RepLine(penMediana, 0, -pfAlto));

            fPropChica.bBold = true;
            panelContenido.Add(365, pfPosY + 10, new RepString(fPropChica, "GAT"));
            panelContenido.Add(365, pfPosY + 22, new RepString(fPropChica, "Comisiones Cobradas"));
            panelContenido.Add(365, pfPosY + 34, new RepString(fPropChica, "Tasa de Intereses Bruta"));
            panelContenido.Add(365, pfPosY + 46, new RepString(fPropChica, "Intereses Pagados"));
            fPropChica.bBold = false;
        }

        private void fnDatosPanelGAT(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sGAT, sComisiones, sTasaInteres, sInteresPagado;
            sGAT = sTasaInteres = sInteresPagado = sComisiones = string.Empty;

            double leftPadding = 0.02;
            double sep = 2.5;
            double posRazon = fPropTitulo.rSize + sep; 
            double tamRenglon = 12;
            double posRenglon = 10;

            try { sGAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@GAT", nsmComprobante).Value; }
            catch { }
            try { sTasaInteres = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@TasaInteres", nsmComprobante).Value; }
            catch { }
            try { sInteresPagado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@InteresPagado", nsmComprobante).Value; }
            catch { }
            try { sComisiones = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@ComisionesCobradas", nsmComprobante).Value; }
            catch { }

            panel.AddAligned(555, RepObj.rAlignRight, 10, RepObj.rAlignBottom, new RepString(fPropChica, sGAT + "%"));
            panel.AddAligned(555, RepObj.rAlignRight, 22, RepObj.rAlignBottom, new RepString(fPropChica, sComisiones + "%"));
            panel.AddAligned(555, RepObj.rAlignRight, 34, RepObj.rAlignBottom, new RepString(fPropChica, sTasaInteres + "%"));
            panel.AddAligned(555, RepObj.rAlignRight, 46, RepObj.rAlignBottom, new RepString(fPropChica, sInteresPagado + "%"));

           
        }

        private void fnCrearPanelCliente(StaticContainer panelContenido)
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelCliente(nsmComprobante, navEncabezado, panelContenido, Renglon);

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 190;
            double pfAlto = 80;

            fPropChica.bBold = true;
            panelContenido.Add(5, 10, new RepString(fPropChica, "No. de Cliente"));
            panelContenido.Add(5, 22, new RepString(fPropChica, "Nombre"));
            panelContenido.Add(5, 34, new RepString(fPropChica, "RFC"));
            panelContenido.Add(5, 46, new RepString(fPropChica, "Domicilio"));
            fPropChica.bBold = false;
        }

        private void fnDatosPanelCliente(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {

            string razonSocial, rfc, calle, noExterior, noInterior, colonia, municipio, estado, pais, codigoPostal, serie, folio, Localidad, sCodigoCliente;
            razonSocial = rfc = calle = noExterior = noInterior = colonia = municipio = estado = pais = codigoPostal = serie = folio = Localidad = sCodigoCliente = string.Empty;

            double leftPadding = 0.02;
            double sep = 2.5;
            double posRazon = fPropTitulo.rSize + sep; 
            double posRenglon = 10;
            double tamRenglon = 12;

            try { sCodigoCliente = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@CodigoCliente", nsmComprobante).Value; }
            catch { }
            try { serie = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@serie", nsmComprobante).Value; }
            catch { }
            try { folio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@folio", nsmComprobante).Value; }
            catch { }
            try { razonSocial = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value; }
            catch { }
            try { rfc = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value; }
            catch { }
            try { calle = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@calle", nsmComprobante).Value; }
            catch { }
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
            try { pais = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@pais", nsmComprobante).Value; }
            catch { }
            try { codigoPostal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@codigoPostal", nsmComprobante).Value; }
            catch { }
            try { Localidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/cfdi:Domicilio/@localidad", nsmComprobante).Value; }
            catch { }

            string sDomicilio = calle + " " + noExterior + " " + colonia + ". " + municipio + ", " + estado + ". " + codigoPostal;

            double i = 0;
            i = fnAgregarMultilinea(panel, sCodigoCliente, fPropChica, leftPadding + 60, posRenglon, 30, true);
            Renglon += i;

            i = fnAgregarMultilinea(panel, razonSocial.ToUpper(), fPropChica, leftPadding + 60, posRenglon + tamRenglon * Renglon, 30, true);
            Renglon += i;

            i = fnAgregarMultilinea(panel, rfc, fPropChica, leftPadding + 60, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;

            i = fnAgregarMultilinea(panel, sDomicilio, fPropChica, leftPadding + 60, posRenglon + tamRenglon * Renglon, 46, true);
            Renglon += i;

        }

        private void fnCrearLeyendas(StaticContainer panelContenido)
        {

            string sLeyendaInformacionGeneral = string.Empty;
            string sLeyendaInformacion = string.Empty;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            try { sLeyendaInformacionGeneral = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@LeyendaInformacionGeneral", nsmComprobante).Value; }
            catch { }
            try { sLeyendaInformacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@LeyendaInformacion", nsmComprobante).Value; }
            catch { }

            panelContenido.Add(5, 185, new RepString(fPropChica, sLeyendaInformacionGeneral));
            panelContenido.Add(5, 200, new RepString(fPropChica, sLeyendaInformacion));

        }

        private void fnCrearPanelCuenta(StaticContainer panelContenido)
        {

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelCuenta(nsmComprobante, navEncabezado, panelContenido, Renglon);

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 60;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelContenido, 0, pfPosY - 10 + 220, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelContenido.rWidth * 0.040;
            panelContenido.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio + 220, RepObj.rAlignCenter, new RepString(fPropBlanca, "Cuenta"));

            fPropChica.bBold = true;
            panelContenido.Add(5, 10 + 220, new RepString(fPropChica, "No. Cuenta"));
            panelContenido.Add(5, 22 + 220, new RepString(fPropChica, "Producto"));
            panelContenido.Add(5, 34 + 220, new RepString(fPropChica, "Moneda"));
            panelContenido.Add(5, 46 + 220, new RepString(fPropChica, "No. de CLABE"));
            panelContenido.Add(5, 58 + 220, new RepString(fPropChica, "Sucursal"));
            fPropChica.bBold = false;

            //borde superior
            panelContenido.Add(pfPosX, pfPosY + 220, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelContenido.Add(pfPosX, pfAlto + 220, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelContenido.Add(pfPosX, pfPosY + 220, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelContenido.Add(pfPosX + pfAncho, pfPosY + 220, new RepLine(penMediana, 0, -pfAlto));
        }

        private void fnDatosPanelCuenta(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sCuenta, sProducto, sMoneda, sCLABE, sSucursal ;
            sCuenta = sProducto = sMoneda = sCLABE = sSucursal =  string.Empty;

           
            double sep = 2.5;
            double posRazon = fPropTitulo.rSize + sep; 
            
            try { sCuenta = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@NumeroCuenta", nsmComprobante).Value; }
            catch { }
            try { sProducto = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@Producto", nsmComprobante).Value; }
            catch { }
            try { sMoneda = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@Moneda", nsmComprobante).Value; }
            catch { }
            try { sCLABE = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@Clabe", nsmComprobante).Value; }
            catch { }
            try { sSucursal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@Sucursal", nsmComprobante).Value; }
            catch { }

            panel.AddAligned(90, RepObj.rAlignLeft, 10 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sCuenta));
            panel.AddAligned(90, RepObj.rAlignLeft, 22 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sProducto));
            panel.AddAligned(90, RepObj.rAlignLeft, 34 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sMoneda));
            panel.AddAligned(90, RepObj.rAlignLeft, 46 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sCLABE));
            panel.AddAligned(90, RepObj.rAlignLeft, 58 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sSucursal));
            
        }

        private void fnCrearPanelDesgloseIntereses(StaticContainer panelContenido)
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelDesgloseIntereses(nsmComprobante, navEncabezado, panelContenido, Renglon);

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 64;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelContenido, 0, pfPosY - 10 + 300, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelContenido.rWidth * .065;
            panelContenido.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio + 300, RepObj.rAlignCenter, new RepString(fPropBlanca, "Rendimiento"));
            fPropChica.bBold = true;
            panelContenido.Add(5, 10 + 300, new RepString(fPropChica, "Saldo Promedio"));
            panelContenido.Add(5, 22 + 300, new RepString(fPropChica, "Días del Periodo"));
            panelContenido.Add(5, 34 + 300, new RepString(fPropChica, "Tasa Bruta Anual"));
            panelContenido.Add(5, 46 + 300, new RepString(fPropChica, "Interés a favor(+)"));
            panelContenido.Add(5, 58 + 300, new RepString(fPropChica, "I.S.R. Retenido(-)"));
            fPropChica.bBold = false;

            //borde superior
            panelContenido.Add(pfPosX, pfPosY + 300, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelContenido.Add(pfPosX, pfAlto + 300, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelContenido.Add(pfPosX, pfPosY + 300, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelContenido.Add(pfPosX + pfAncho, pfPosY + 300, new RepLine(penMediana, 0, -pfAlto));
            //Barra separadora horizontal 
            panelContenido.Add(pfPosX, pfAlto - 27 + 300, new RepLine(penDelgada, pfAncho, 0));
        }

        private void fnDatosPanelDesgloseIntereses(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sSaldoPromedio, sDiasPeriodo, sTasaPromedio, sInteresBrutos, sISRRetenido;
            sSaldoPromedio = sDiasPeriodo = sTasaPromedio = sInteresBrutos = sISRRetenido = string.Empty;

            try {
                sSaldoPromedio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoPromedio", nsmComprobante).Value;
                sSaldoPromedio = TransformacionUnionProg.fnFormatoRedondeo(sSaldoPromedio);
            }
            catch { }
            try { sDiasPeriodo = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@DiasPeriodo", nsmComprobante).Value; }
            catch { }
            try {
                sTasaPromedio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@TasaInteres", nsmComprobante).Value;
                sTasaPromedio = TransformacionUnionProg.fnFormatoRedondeo(sTasaPromedio);
            }
            catch { }
            try { 
                sInteresBrutos = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@InteresPagado", nsmComprobante).Value;
                sInteresBrutos = TransformacionUnionProg.fnFormatoRedondeo(sInteresBrutos);
            }
            catch { }
            try 
            { 
                sISRRetenido = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@ISRRetenido", nsmComprobante).Value;
                sISRRetenido = TransformacionUnionProg.fnFormatoRedondeo(sISRRetenido);
            }
            catch { }


            double leftPadding = 0.02;
            double sep = 2.5;
            double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
            double tamRenglon = 12;
            double posRenglon = 10;

            double i = 0;

            panel.AddAligned(245, RepObj.rAlignRight, 10 + 300, RepObj.rAlignBottom, new RepString(fPropChica, sSaldoPromedio));
            panel.AddAligned(245, RepObj.rAlignRight, 22 + 300, RepObj.rAlignBottom, new RepString(fPropChica, sDiasPeriodo));
            panel.AddAligned(245, RepObj.rAlignRight, 34 + 300, RepObj.rAlignBottom, new RepString(fPropChica, sTasaPromedio));
            panel.AddAligned(245, RepObj.rAlignRight, 46 + 300, RepObj.rAlignBottom, new RepString(fPropChica, sInteresBrutos));
            panel.AddAligned(245, RepObj.rAlignRight, 58 + 300, RepObj.rAlignBottom, new RepString(fPropChica, sISRRetenido));
            

          
        }

        private void fnCrearPanelComportamiento(StaticContainer panelContenido)
        {

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelComportamiento(nsmComprobante, navEncabezado, panelContenido, Renglon);

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 60;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelContenido, 315, pfPosY - 10 + 220, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelContenido.rWidth * 0.085;
            panelContenido.AddAligned(posColumna1 + 315, RepObj.rAlignCenter, puntoMedio + 220, RepObj.rAlignCenter, new RepString(fPropBlanca, "Comportamiento"));

            fPropChica.bBold = true;
            panelContenido.Add(5 + 315, 10 + 220, new RepString(fPropChica, "Saldo Anterior (+)"));
            panelContenido.Add(5 + 315, 22 + 220, new RepString(fPropChica, "Depósitos/Abonos (+)"));
            panelContenido.Add(5 + 315, 34 + 220, new RepString(fPropChica, "Retiros/Cargos (-)"));
            panelContenido.Add(5 + 315, 46 + 220, new RepString(fPropChica, "Saldo Final"));
            panelContenido.Add(5 + 315, 58 + 220, new RepString(fPropChica, "Saldo Mínimo"));
            fPropChica.bBold = false;

            //borde superior
            panelContenido.Add(pfPosX + 315, pfPosY + 220, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelContenido.Add(pfPosX + 315, pfAlto + 220, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelContenido.Add(pfPosX + 315, pfPosY + 220, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelContenido.Add(pfPosX + 315 + pfAncho, pfPosY + 220, new RepLine(penMediana, 0, -pfAlto));
            //Barra separadora horizontal 
            panelContenido.Add(pfPosX + 315, pfAlto - 23 + 220, new RepLine(penDelgada, pfAncho, 0));
        }

        private void fnDatosPanelComportamiento(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sSaldoAnterior, sDepositosAbonos, sRetirosCargos, sSaldoFinal, sSaldoMinimo;
            sSaldoAnterior =sDepositosAbonos = sRetirosCargos = sSaldoFinal = sSaldoMinimo =  string.Empty;

            try 
            { 
                sSaldoAnterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoAnterior", nsmComprobante).Value;
                sSaldoAnterior = TransformacionUnionProg.fnFormatoRedondeo(sSaldoAnterior);
            }
            catch { }
            try { 
                sDepositosAbonos = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@TotalAbonos", nsmComprobante).Value;
                sDepositosAbonos = TransformacionUnionProg.fnFormatoRedondeo(sDepositosAbonos);
            }
            catch { }
            try 
            {
                sRetirosCargos = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@TotalCargos", nsmComprobante).Value;
                sRetirosCargos = TransformacionUnionProg.fnFormatoRedondeo(sRetirosCargos);
            }
            catch { }
            try 
            { 
                sSaldoFinal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoCierre", nsmComprobante).Value;
                sSaldoFinal = TransformacionUnionProg.fnFormatoRedondeo(sSaldoFinal);
            }
            catch { }
            try 
            { 
                sSaldoMinimo = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoMinimo", nsmComprobante).Value;
                sSaldoMinimo = TransformacionUnionProg.fnFormatoRedondeo(sSaldoMinimo);
            }
            catch { }

            panel.AddAligned(560, RepObj.rAlignRight, 10 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sSaldoAnterior));
            panel.AddAligned(560, RepObj.rAlignRight, 22 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sDepositosAbonos));
            panel.AddAligned(560, RepObj.rAlignRight, 34 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sRetirosCargos));
            panel.AddAligned(560, RepObj.rAlignRight, 46 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sSaldoFinal));
            panel.AddAligned(560, RepObj.rAlignRight, 58 + 220, RepObj.rAlignBottom, new RepString(fPropChica, sSaldoMinimo));
        }

        //panel Desglose saldo final
        private void fnCrearPanelDesgloseSaldo(StaticContainer panelContenido)
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelDesgloseSaldo(nsmComprobante, navEncabezado, panelContenido, Renglon);

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 64;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelContenido, 315, pfPosY - 10 + 300, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelContenido.rWidth * .115;
            panelContenido.AddAligned(posColumna1 + 315, RepObj.rAlignCenter, puntoMedio + 300, RepObj.rAlignCenter, new RepString(fPropBlanca, "Desglose de Saldo Final"));

            fPropChica.bBold = true;
            //La separacion es de 12 Puntos
            panelContenido.Add(5 + 315, 10 + 300, new RepString(fPropChica, "Saldo Disponible (+)"));
            panelContenido.Add(5 + 315, 22 + 300, new RepString(fPropChica, "Saldo Congelado (+)"));
            panelContenido.Add(5 + 315, 34 + 300, new RepString(fPropChica, "Fondos por Confirmar (+)"));
            
            fPropNormal.bBold = false;

            //borde superior
            panelContenido.Add(pfPosX + 315, pfPosY + 300, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelContenido.Add(pfPosX + 315, pfAlto + 300, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelContenido.Add(pfPosX + 315, pfPosY + 300, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelContenido.Add(pfPosX + 315 + pfAncho, pfPosY + 300, new RepLine(penMediana, 0, -pfAlto));
        }

        private void fnDatosPanelDesgloseSaldo(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sSaldoDisponible, sSaldoCongelado, sFondosConfirmar;
            sSaldoDisponible = sSaldoCongelado = sFondosConfirmar =  string.Empty;

            try {
                sSaldoDisponible = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoDisponible", nsmComprobante).Value;
                sSaldoDisponible = TransformacionUnionProg.fnFormatoRedondeo(sSaldoDisponible);
            }
            catch { }
            try { 
                sSaldoCongelado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoCongelado", nsmComprobante).Value;
                sSaldoCongelado = TransformacionUnionProg.fnFormatoRedondeo(sSaldoCongelado);
            }
            catch { }
            try { 
                sFondosConfirmar = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoReserva", nsmComprobante).Value;
                sFondosConfirmar = TransformacionUnionProg.fnFormatoRedondeo(sFondosConfirmar);
            } 
            catch { }


            panel.AddAligned(560, RepObj.rAlignRight, 10 + 300, RepObj.rAlignBottom, new RepString(fPropChica, sSaldoDisponible));
            panel.AddAligned(560, RepObj.rAlignRight, 22 + 300, RepObj.rAlignBottom, new RepString(fPropChica, sSaldoCongelado));
            panel.AddAligned(560, RepObj.rAlignRight, 34 + 300, RepObj.rAlignBottom, new RepString(fPropChica, sFondosConfirmar));
           

        }

        private void fnCrearPanelComisiones(StaticContainer panelContenido) 
        {

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelComisiones(nsmComprobante, navEncabezado, panelContenido, Renglon);

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 50;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelContenido, 0, pfPosY - 10 + 385, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelContenido.rWidth * .065;
            panelContenido.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio + 385, RepObj.rAlignCenter, new RepString(fPropBlanca, "Comisiones"));

            fPropChica.bBold = true;
            panelContenido.Add(5, 10 + 385, new RepString(fPropChica, "Cheques pagados"));
            panelContenido.Add(5, 22 + 385, new RepString(fPropChica, "Manejo de cuenta"));
            panelContenido.Add(5, 34 + 385, new RepString(fPropChica, "Anualidad"));
            panelContenido.Add(5, 46 + 385, new RepString(fPropChica, "Operaciones"));
            fPropChica.bBold = false;

            //borde superior
            panelContenido.Add(pfPosX, pfPosY + 385, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelContenido.Add(pfPosX, pfAlto + 385, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelContenido.Add(pfPosX, pfPosY + 385, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelContenido.Add(pfPosX + pfAncho, pfPosY + 385, new RepLine(penMediana, 0, -pfAlto));
        
        }

        private void fnDatosPanelComisiones(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon) 
        {
            string sChequesPagados, sManejoCuenta, sAnualidad, sOperaciones;
            sChequesPagados = sManejoCuenta = sAnualidad = sOperaciones ="100";

            try 
            { 
                sChequesPagados = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@ChequesPagados", nsmComprobante).Value;
                sChequesPagados = TransformacionUnionProg.fnFormatoRedondeo(sChequesPagados);
            }
            catch { }
            try 
            {
                sManejoCuenta = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@ManejoCuenta", nsmComprobante).Value;
                sManejoCuenta = TransformacionUnionProg.fnFormatoRedondeo(sManejoCuenta);
            
            }
            catch { }
            try 
            { 
                sAnualidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@Anualidad", nsmComprobante).Value;
                sAnualidad = TransformacionUnionProg.fnFormatoRedondeo(sAnualidad);
            }
            catch { }
            try 
            { 
                sOperaciones = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@Operaciones", nsmComprobante).Value;
                sOperaciones = TransformacionUnionProg.fnFormatoRedondeo(sOperaciones); 
            }
            catch { }
            fPropChica.bBold = false;
            panel.AddAligned(245, RepObj.rAlignRight, 10 + 385, RepObj.rAlignBottom, new RepString(fPropChica, sChequesPagados));
            panel.AddAligned(245, RepObj.rAlignRight, 22 + 385, RepObj.rAlignBottom, new RepString(fPropChica, sManejoCuenta));
            panel.AddAligned(245, RepObj.rAlignRight, 34 + 385, RepObj.rAlignBottom, new RepString(fPropChica, sAnualidad));
            panel.AddAligned(245, RepObj.rAlignRight, 46 + 385, RepObj.rAlignBottom, new RepString(fPropChica, sOperaciones));
           
        }

        private void fnObtenerDatosCargosObjetados()
        {

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");

            XPathNodeIterator xIterador = gxComprobante.CreateNavigator().Select("cfdi:Comprobante/cfdi:Addenda[1]/UnionProgreso[1]/EC[1]/OC", nsmComprobante);

            while (xIterador.MoveNext())
            {
                XmlDocument aux = new XmlDocument();

                aux.LoadXml(xIterador.Current.OuterXml);

                datosCargosObjetados.Add(aux);
            }
        }

        private void fnObtenerDatosMovimientosRealizados()
        {

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");

            XPathNodeIterator xIterador2 = gxComprobante.CreateNavigator().Select("cfdi:Comprobante/cfdi:Addenda[1]/UnionProgreso[1]/EC[1]/MC", nsmComprobante);

            while (xIterador2.MoveNext())
            {
                XmlDocument aux2 = new XmlDocument();

                aux2.LoadXml(xIterador2.Current.OuterXml);

                datosMovimientosRealizados.Add(aux2);
            }
        }

        private void fnDibujaPanelCargosObjetados(ref double posY, ref Page pagina, Report reporte)
        {
            //Dibujamos primero el encabezado de el panel
            posY = posY + 10;
            pagina.Add(margenPagina, posY, fnDibujaEncabezadoPanelCargosObjetados());
            posY = posY + 10;

            foreach (XmlDocument xDoc in datosCargosObjetados)
            {
                string sMontoObjetado = string.Empty;
                string sDescripcionObjetado = string.Empty;
                string sReferenciaObjetado = string.Empty;
                string sFechaObjetado = string.Empty;

                sMontoObjetado = xDoc.CreateNavigator().SelectSingleNode("/OC/@MontoObjetado").ToString();
                sDescripcionObjetado =xDoc.CreateNavigator().SelectSingleNode("/OC/@DescripcionObjetado").ToString();
                sReferenciaObjetado = xDoc.CreateNavigator().SelectSingleNode("/OC/@ReferenciaObjetado").ToString();
                sFechaObjetado = xDoc.CreateNavigator().SelectSingleNode("/OC/@FechaObjetado").ToString();

                sMontoObjetado = TransformacionUnionProg.fnFormatoRedondeo(sMontoObjetado);

                 if ((limiteY - posY) > 12)
                {
                    fnDibujaDatosPanelCargosObjetados(ref posY, ref pagina, sMontoObjetado, sDescripcionObjetado, sReferenciaObjetado, sFechaObjetado);
                    //posY = posY + 10;
                }
                else
                {
                    pagina = new Page(PDF);
                    pagina.rWidthMM = anchoPagina;
                    pagina.rHeightMM = altoPagina;
                    paginasReporte.Enqueue(pagina);
                    fnCrearPie(ref pagina, ref PDF);
                    posY = 130;
                    pagina.Add(margenPagina, posY, fnDibujaEncabezadoPanelCargosObjetados());
                    posY = posY + 10;
                    fnDibujaDatosPanelCargosObjetados(ref posY, ref pagina, sMontoObjetado, sDescripcionObjetado, sReferenciaObjetado, sFechaObjetado);
                   // posY = posY + 10;
                }

            }
            posY = posY + 10;
            pagina.Add(margenPagina, posY, new RepLine(penMediana, 565, 0));
            posY = posY + 10;
        }

        private StaticContainer fnDibujaEncabezadoPanelCargosObjetados()
        {
            StaticContainer encabezadoCargos = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 565;
            double pfAlto = 30;
            double posColumna1 = encabezadoCargos.rWidth * 0.01;
            double posColumna2 = encabezadoCargos.rWidth * 0.20;
            double posColumna3 = encabezadoCargos.rWidth * .50;
            double posColumna4 = encabezadoCargos.rWidth * 1.24;

            
            encabezadoCargos.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            
            fPropNormal.bBold = true;
            encabezadoCargos.Add(5, -3, new RepString(fPropNormal, "Cargos Objetados"));
            encabezadoCargos.AddAligned(posColumna1, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Fecha"));
            encabezadoCargos.AddAligned(posColumna2, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Referencia"));
            encabezadoCargos.AddAligned(posColumna3, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Descripción"));
            encabezadoCargos.AddAligned(posColumna4, RepObj.rAlignRight, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Monto"));
            fPropNormal.bBold = false;

            return encabezadoCargos;

        }

        private void fnDibujaDatosPanelCargosObjetados(ref double posY, ref Page pagina, string sMontoObjetado, string sDescripcionObjetado, string sReferenciaObjetado, string sFechaObjetado)
        {
            string[] temp = sFechaObjetado.Split('T');
            double posColumna1 = pagina.rWidth * 0.065;
            double posColumna2 = pagina.rWidth * 0.25;
            double posColumna3 = pagina.rWidth * .4;
            double posColumna4 = pagina.rWidth * .95;

            double i = 0;
            double j = 0;
            pagina.AddAligned(posColumna1, RepObj.rAlignCenter, posY + 10, RepObj.rAlignCenter, new RepString(fPropChica, temp[0]));
         //   pagina.AddAligned(posColumna2, RepObj.rAlignCenter, posY + 10, RepObj.rAlignCenter, new RepString(fPropChica, sReferenciaObjetado));
         //   pagina.AddAligned(posColumna3, RepObj.rAlignLeft, posY + 10, RepObj.rAlignCenter, new RepString(fPropChica, sDescripcionObjetado));
            pagina.AddAligned(posColumna4, RepObj.rAlignRight, posY + 10, RepObj.rAlignCenter, new RepString(fPropChica, sMontoObjetado));

            j = fnAgregarMultilinea(pagina, sReferenciaObjetado, fPropChica, 100, posY + 10, 20, true);
            i = fnAgregarMultilinea(pagina, sDescripcionObjetado, fPropChica, posColumna3, posY + 10, 60, true);


            if (i >= 2 || j >=2)
            {

                posY += i * (10) + 8;
            }
            else if(i ==1 && j ==1)
            {
                posY += 10;
            }


        }

        private void fnDibujarPanelMovimientos(ref double posY, ref Page pagina, Report reporte)
        {
            //Dibujamos primero el encabezado de el panel
            posY = posY + 10;
            pagina.Add(margenPagina, posY, fnDibujarEncabezadoPanelMovimientos());
            posY = posY + 10;

            double totalCargo = 0;
            double totalAbono = 0;

            foreach (XmlDocument xDoc in datosMovimientosRealizados)
            {
                string sSobregiro = string.Empty;
                string sSaldo = string.Empty;
                string sAbono = string.Empty;
                string sCargo = string.Empty;
                string sFormaPago = string.Empty;
                string sDescripcion = string.Empty;
                string sReferencia = string.Empty;
                string sFechaAplicacion = string.Empty;
                string sTransaccion = string.Empty;
                string sFechaMovimiento = string.Empty;

                sSobregiro = xDoc.CreateNavigator().SelectSingleNode("/MC/@Sobregiro").ToString();
                sSaldo = xDoc.CreateNavigator().SelectSingleNode("/MC/@Saldo").ToString();
                sAbono = xDoc.CreateNavigator().SelectSingleNode("/MC/@Abono").ToString();
                sCargo = xDoc.CreateNavigator().SelectSingleNode("/MC/@Cargo").ToString();
                sFormaPago = xDoc.CreateNavigator().SelectSingleNode("/MC/@FormaPago").ToString();
                sDescripcion = xDoc.CreateNavigator().SelectSingleNode("/MC/@Descripcion").ToString();
                sReferencia = xDoc.CreateNavigator().SelectSingleNode("/MC/@Referencia").ToString();
                sFechaAplicacion = xDoc.CreateNavigator().SelectSingleNode("/MC/@FechaAplicacion").ToString();
                sTransaccion = xDoc.CreateNavigator().SelectSingleNode("/MC/@Transaccion").ToString();
                sFechaMovimiento = xDoc.CreateNavigator().SelectSingleNode("/MC/@FechaMovimiento").ToString();

                totalCargo += Convert.ToDouble(sCargo);
                totalAbono += Convert.ToDouble(sAbono);

                sSobregiro = TransformacionUnionProg.fnFormatoRedondeo(sSobregiro);
                sSaldo = TransformacionUnionProg.fnFormatoRedondeo(sSaldo);
                sAbono = TransformacionUnionProg.fnFormatoRedondeo(sAbono);
                sCargo = TransformacionUnionProg.fnFormatoRedondeo(sCargo);
              
                if ((limiteY - posY) > 12)
                {
                    fnDibujaDatosPanelMovimientosRealizados(ref posY, ref pagina, sSobregiro, sSaldo, sAbono, sCargo, sFormaPago, sDescripcion, sReferencia, sFechaAplicacion, sTransaccion, sFechaMovimiento);
                   // posY = posY + 30;
                }
                else
                {
                    pagina = new Page(PDF);
                    pagina.rWidthMM = anchoPagina;
                    pagina.rHeightMM = altoPagina;
                    paginasReporte.Enqueue(pagina);
                    fnCrearPie(ref pagina, ref PDF);
                    posY = 130;
                    pagina.Add(margenPagina, posY, fnDibujarEncabezadoPanelMovimientos());
                    posY = posY + 10;
                    fnDibujaDatosPanelMovimientosRealizados(ref posY, ref pagina, sSobregiro, sSaldo, sAbono, sCargo, sFormaPago, sDescripcion, sReferencia, sFechaAplicacion, sTransaccion, sFechaMovimiento);
                 //   posY = posY + 30;
                }

            }
            posY = posY + 10;
            pagina.Add(margenPagina, posY, new RepLine(penMediana, 565, 0));
            posY = posY + 10;
            pagina.Add(30, posY, new RepString(fPropNormal, "TOTALES"));
            pagina.AddAligned(430, RepObj.rAlignLeft, posY, RepObj.rAlignBottom, new RepString(fPropNormal, TransformacionUnionProg.fnFormatoRedondeo(totalCargo.ToString())));
            pagina.AddAligned(493, RepObj.rAlignLeft, posY, RepObj.rAlignBottom, new RepString(fPropNormal, TransformacionUnionProg.fnFormatoRedondeo(totalAbono.ToString())));
            posY = posY + 10;
        }

        private StaticContainer fnDibujarEncabezadoPanelMovimientos()
        {
            StaticContainer panelMovimientos = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            double pfAncho = 565;
            double pfAlto = 50;
            double posColumna1 = panelMovimientos.rWidth * 0.035;
            double posColumna2 = panelMovimientos.rWidth * .15;
            double posColumna3 = panelMovimientos.rWidth * .30;
            double posColumna4 = panelMovimientos.rWidth * .74;
            double posColumna5 = panelMovimientos.rWidth * .93;
            double posColumna6 = panelMovimientos.rWidth * 1.07;
            double posColumna7 = panelMovimientos.rWidth * 1.22;
           

            //borde superior
            panelMovimientos.Add(0, -3, new RepLine(penMediana, pfAncho, 0));

            fPropChica.bBold = true;
            panelMovimientos.Add(5, 0 - 5, new RepString(fPropNormal, "Movimientos Realizados"));
            panelMovimientos.AddAligned(posColumna1 + 20 , RepObj.rAlignCenter, 5, RepObj.rAlignBottom, new RepString(fPropChica, "Fecha"));
            panelMovimientos.AddAligned(posColumna1, RepObj.rAlignCenter, 13, RepObj.rAlignBottom, new RepString(fPropChica, "Mov."));
            panelMovimientos.AddAligned(posColumna2, RepObj.rAlignCenter, 13, RepObj.rAlignBottom, new RepString(fPropChica, "Apl."));
            panelMovimientos.AddAligned(posColumna3, RepObj.rAlignCenter, 13, RepObj.rAlignBottom, new RepString(fPropChica, "Descripción"));
            panelMovimientos.AddAligned(posColumna4, RepObj.rAlignCenter, 13, RepObj.rAlignBottom, new RepString(fPropChica, "Forma Pago"));
            panelMovimientos.AddAligned(posColumna5, RepObj.rAlignCenter, 13, RepObj.rAlignBottom, new RepString(fPropChica, "Cargo"));
            panelMovimientos.AddAligned(posColumna6, RepObj.rAlignCenter, 13, RepObj.rAlignBottom, new RepString(fPropChica, "Abono"));
            panelMovimientos.AddAligned(posColumna7, RepObj.rAlignCenter, 13, RepObj.rAlignBottom, new RepString(fPropChica, "Saldo"));
            fPropChica.bBold = false;

            return panelMovimientos;
        }

        private void fnDibujaDatosPanelMovimientosRealizados(ref double posY, ref Page pagina, string sSobregiro, string sSaldo, string sAbono, string sCargo, string sFormaPago, string sDescripcion, string sReferencia, string sFechaAplicacion, string sTransaccion, string sFechaMovimiento)
        {
            string[] aFechaAplicacion = sFechaMovimiento.Split('T');
            string[] aFechaMovimiento = sFechaMovimiento.Split('T');
            
            double leftPadding = 0.02;
            double sep = 2.5;
            double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
            double posRenglon = 10;
            double i = 0;

            sFormaPago = "PAGO EN OXXO ASDASDASDASDASDASDASDASDASDASDASD";

            fPropChica.bBold = false;
            pagina.AddAligned(27, RepObj.rAlignLeft, posY + 13, RepObj.rAlignBottom, new RepString(fPropChica, aFechaMovimiento[0]));
            pagina.AddAligned(82, RepObj.rAlignLeft, posY + 13, RepObj.rAlignBottom, new RepString(fPropChica, "07/ENE"));
            i = fnAgregarMultilinea(pagina, sDescripcion, fPropChica, leftPadding + 135, posY + 13, 35, true);
            i = fnAgregarMultilinea(pagina, sFormaPago, fPropChica, 335, posY + 13, 20, true);
            pagina.AddAligned(430, RepObj.rAlignLeft, posY + 13, RepObj.rAlignBottom, new RepString(fPropChica, sCargo));
            pagina.AddAligned(493, RepObj.rAlignLeft, posY + 13, RepObj.rAlignBottom, new RepString(fPropChica, sAbono));
            pagina.AddAligned(580, RepObj.rAlignRight, posY + 13, RepObj.rAlignBottom, new RepString(fPropChica, sSaldo));

            if (i > 2)
            {

                posY += i * (8);
            }
            else
            {
                posY += 13;
            }
        }

        private void fnDibujarGrafica(ref double posY, ref Page pagina, Report reporte) 
        {
            MemoryStream ms2 = new MemoryStream();
            ms2 = fnGenerarGrafica();
            RepImage grafica = new RepImage(ms2, 700, 230);
            
            double columna1 = 120;
            double columna2 = 190;
            double columna3 = 250;
            double columna4 = 320;
            double columna5 = 380;
            double columna6 = 450;
            double columna7 = 520;
            //sISRRetenido + sIVAComisiones

            string sSaldoAnterior, sAbonos, sCargos, sComisionesCobradas, sInteresesPagados, sISRRetenido, sIVAComisiones, sSaldoCierre;

            sSaldoAnterior = sAbonos = sCargos = sComisionesCobradas = sInteresesPagados = sISRRetenido = sIVAComisiones = sSaldoCierre = "1100";

            sSaldoAnterior = TransformacionUnionProg.fnFormatoRedondeo(sSaldoAnterior);
            sAbonos = TransformacionUnionProg.fnFormatoRedondeo(sAbonos);
            sCargos = TransformacionUnionProg.fnFormatoRedondeo(sCargos);
            sComisionesCobradas = TransformacionUnionProg.fnFormatoRedondeo(sComisionesCobradas);
            string sSuma = Convert.ToString(Convert.ToDouble(sISRRetenido) + Convert.ToDouble(sIVAComisiones));
            sSuma = TransformacionUnionProg.fnFormatoRedondeo(sSuma);
            sSaldoCierre = TransformacionUnionProg.fnFormatoRedondeo(sSaldoCierre);

            if (limiteY - posY <= 230)
            {
                pagina = new Page(PDF);
                pagina.rWidthMM = anchoPagina;
                pagina.rHeightMM = altoPagina;
                paginasReporte.Enqueue(pagina);
                fnCrearPie(ref pagina, ref PDF);
                posY = 330;
                pagina.Add(-100, posY, grafica);
                
            }
            else 
            {
                pagina.Add(-100, posY + 230, grafica);
               
                posY += 230;
            }

            pagina.AddAligned(columna1, RepObj.rAlignCenter, posY + 30, RepObj.rAlignBottom, new RepString(fPropChica, "$" + sSaldoAnterior));
            pagina.AddAligned(columna2, RepObj.rAlignCenter, posY + 30, RepObj.rAlignBottom, new RepString(fPropChica, "$" + sAbonos));
            pagina.AddAligned(columna3, RepObj.rAlignCenter, posY + 30, RepObj.rAlignBottom, new RepString(fPropChica, "$" + sCargos));
            pagina.AddAligned(columna4, RepObj.rAlignCenter, posY + 30, RepObj.rAlignBottom, new RepString(fPropChica, "$" + sComisionesCobradas));
            pagina.AddAligned(columna5, RepObj.rAlignCenter, posY + 30, RepObj.rAlignBottom, new RepString(fPropChica, "$" + sInteresesPagados));
            pagina.AddAligned(columna6, RepObj.rAlignCenter, posY + 30, RepObj.rAlignBottom, new RepString(fPropChica, "$" + sSuma));
            pagina.AddAligned(columna7, RepObj.rAlignCenter, posY + 30, RepObj.rAlignBottom, new RepString(fPropChica, "$" + sSaldoCierre));


        }

        private void fnCrearUltimaPagina(Report PDF, string sFooter, string sLogo, double anchoLogo, double altoLogo)
        {
            Page ultimaPagina = new Page(PDF);
            ultimaPagina.rWidthMM = anchoPagina;
            ultimaPagina.rHeightMM = altoPagina;
            paginasReporte.Enqueue(ultimaPagina);

            string sTimbrado = string.Empty;
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            string sSaldoInicial = string.Empty;
            string sSaldoFinal = string.Empty;

            try { 
                sSaldoInicial = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoAnterior", nsmComprobante).Value;
                sSaldoInicial = TransformacionUnionProg.fnFormatoRedondeo(sSaldoInicial);
            }
            catch { }
            try 
            {   sSaldoFinal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoCierre", nsmComprobante).Value;
                sSaldoFinal = TransformacionUnionProg.fnFormatoRedondeo(sSaldoFinal);
            }
            catch { }

            try { sTimbrado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@Timbrado", nsmComprobante).Value; }
            catch { }

            //SE CARGA LA PUBLICIDAD
            MemoryStream memPub = new MemoryStream();
            memPub = fnImagenCliente(sFooter);
            RepImage publicidad = new RepImage(memPub, 563, 420);
            ultimaPagina.Add(margenPagina, 520, publicidad);

            
            //Si es plantilla fiscal
            if (sTimbrado == "S")
            {
                string sFolio, sCertificadoCSD, sFechaExpedicion, sLugarExpedicion, sHoraExpedicion, sRFC, sRegimenFiscal, sSelloCFDI, sSelloSat,
                    sNoSerieCertificadoSAT, sFechaCertificacion, sHoraCertificacion, sCertificadoEmisor, sMetodoPago, sNumeroCuenta;

                sFolio = sCertificadoCSD = sFechaExpedicion = sLugarExpedicion = sHoraExpedicion = sRFC = sRegimenFiscal = sSelloCFDI = sSelloSat =
                    sNoSerieCertificadoSAT = sFechaCertificacion = sHoraCertificacion = sCertificadoEmisor = sMetodoPago = sNumeroCuenta = string.Empty;

                try { sFolio = navEncabezado.SelectSingleNode("//cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
                catch { }
                try { sCertificadoCSD = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@certificado", nsmComprobante).Value; }
                catch { }
                try
                {
                    sFechaExpedicion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value;
                    string[] temp = sFechaExpedicion.Split('T');
                    sFechaExpedicion = temp[0];
                    sHoraExpedicion = temp[1];
                }
                catch { }
                try { sLugarExpedicion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", nsmComprobante).Value; }
                catch { }
                try { sRFC = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value; }
                catch { }
                try { sRegimenFiscal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:RegimenFiscal/@Regimen", nsmComprobante).Value; }
                catch { }
                try { sSelloCFDI = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloCFD", nsmComprobante).Value; }
                catch { }
                try { sSelloSat = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value; }
                catch { }
                try { sNoSerieCertificadoSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
                catch { }
                try
                {
                    sFechaCertificacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;
                    string[] temp = sFechaCertificacion.Split('T');
                    sFechaCertificacion = temp[0];
                    sHoraCertificacion = temp[1];
                }
                catch { }
                try { sCertificadoEmisor = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value; }
                catch { }
                try { sMetodoPago = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@metodoDePago", nsmComprobante).Value; }
                catch { }
                try { sNumeroCuenta = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@NumCtaPago", nsmComprobante).Value; }
                catch { }

                //Agregar el codido bidimensional
                ultimaPagina.Add(margenPagina + 10, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 450, GenerarCodigoBidimensional());
                //Agregamos los titulos a la tabla
                fPropChica.bBold = true;
                ultimaPagina.Add(margenPagina + 200, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 385, new RepString(fPropChica, "Este documento es una representación impresa de un CFDI"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 395, new RepString(fPropChica, "FOLIO FICAL:"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 405, new RepString(fPropChica, "SERIE DEL CERTIFICADO DEL SAT:"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 415, new RepString(fPropChica, "SERIE DEL CERTIFICADO DEL EMISOR:"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 425, new RepString(fPropChica, "FECHA Y HORA DE CERTIFICACIÓN:"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 435, new RepString(fPropChica, "REGIMEN FISCAL:"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 445, new RepString(fPropChica, "LUGAR DE EXPEDICIÓN:"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 455, new RepString(fPropChica, "METODO DE PAGO"));
                ultimaPagina.Add(margenPagina + 270, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 455, new RepString(fPropChica, "NUMERO DE CUENTA"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 485, new RepString(fPropChica, "CADENA ORIGINAL DEL COMPLEMENTO DE CERTIFICACION DIGITAL DEL SAT:"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 515 + 10, new RepString(fPropChica, "SELLO DEL SAT:"));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 545 + 10, new RepString(fPropChica, "SELLO DIGITAL DEL CFDI:"));
                fPropChica.bBold = false;

                //Lineas horizontales
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 387, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 397, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 407, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 417, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 427, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 437, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 447, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 457, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 477, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 487, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 507 + 10, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 517 + 10, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 537 + 10, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 547 + 10, new RepLine(penDelgada, 400, 0));
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 577 + 10, new RepLine(penDelgada, 400, 0));
                //Lineas verticales
                ultimaPagina.Add(margenPagina + 95, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 387, new RepLine(penDelgada, 0, -200));
                ultimaPagina.Add(margenPagina + 495, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 387, new RepLine(penDelgada, 0, -200));
                ultimaPagina.Add(margenPagina + 265, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 447, new RepLine(penDelgada, 0, -30));

                //Agregamos los datos a el panel
                ultimaPagina.Add(margenPagina + 150, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 395, new RepString(fPropChica, sFolio));
                ultimaPagina.Add(margenPagina + 225, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 405, new RepString(fPropChica, sNoSerieCertificadoSAT));
                ultimaPagina.Add(margenPagina + 237, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 415, new RepString(fPropChica, sCertificadoEmisor));
                ultimaPagina.Add(margenPagina + 230, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 425, new RepString(fPropChica, sFechaCertificacion + " - " + sHoraCertificacion));
                ultimaPagina.Add(margenPagina + 165, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 435, new RepString(fPropChica, sRegimenFiscal));
                ultimaPagina.Add(margenPagina + 187, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 445, new RepString(fPropChica, sLugarExpedicion));
                ultimaPagina.Add(margenPagina + 100, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 465, new RepString(fPropChica, sMetodoPago));
                ultimaPagina.Add(margenPagina + 270, RT.rPointFromMM(altoPagina) - margenPagina - ultimaPagina.rHeight + (tamCodigo * 2) + 465, new RepString(fPropChica, sNumeroCuenta));

                double i = 0;
                double Renglon = 0;
                double sep = 9.5;
                double posRazon = fPropTitulo.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
                double tamRenglon = 16;
                double posRenglon = posRazon + sep;

                i = fnAgregarMultilinea(ultimaPagina, TransformacionUnionProg.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_3_2")
                , fPropChica, margenPagina + 97, 655, 90, true);
                Renglon += i;

                i = fnAgregarMultilinea(ultimaPagina, sSelloSat, fPropChica, margenPagina + 97, 685 + 10, 94, true);
                Renglon += i;

                i = fnAgregarMultilinea(ultimaPagina, sSelloCFDI, fPropChica, margenPagina + 97, 715 + 10, 94, true);
                Renglon += i;
            }
            else
            {
                string sPeriodoInicio = string.Empty;
                string sPeriodoFin = string.Empty;
                try
                {
                    sPeriodoInicio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@PeriodoInicio", nsmComprobante).Value;
                    string[] temp = sPeriodoInicio.Split('T');
                    sPeriodoInicio = temp[0];
                }
                catch { }
                try
                {
                    sPeriodoFin = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@PeriodoFin", nsmComprobante).Value;
                    string[] temp = sPeriodoFin.Split('T');
                    sPeriodoFin = temp[0];
                }
                catch { }

                string sMensaje = "POR EL PERIODO DEL ESTADO DE CUENTA COMPRENDIDO DEL " + sPeriodoInicio + " al " + sPeriodoFin +
                    " NO SE GENERARON MOVIMIENTOS DE DEDUCCION FISCAL, TRASLADO O RETENCION DE IMPUESTOS A REPORTAR, POR LO CUAL, NO SE INCLUYE LA CADENA ORIGINAL NI SELLO DIGITAL.";
                double leftPadding = 0.02;
                double sep = 2.5;
                double posRazon = fPropTitulo.rSize + sep; 
                double posRenglon = 10;
                double i = 0;

                fPropChica.bBold = true;
                i = fnAgregarMultilinea(ultimaPagina, sMensaje, fPropChica, margenPagina + 5, 500, 130, true);
                fPropChica.bBold = false;
            }

            StaticContainer pie = fnCrearPanelPie();
            ultimaPagina.Add(margenPagina, RT.rPointFromMM(altoPagina) - margenPagina - 8, pie);
        }

        //Panel Cargos Objetados
        private StaticContainer fnCrearPanelCargos()
        {
            StaticContainer panelCargos = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 580;
            double pfAlto = 50;
            double posColumna1 = panelCargos.rWidth * 0.01;
            double posColumna2 = panelCargos.rWidth * 0.15;
            double posColumna3 = panelCargos.rWidth * 0.35;
            double posColumna4 = panelCargos.rWidth * 1.15;

            fPropChica.bBold = true;
            fPropNormal.bBold = true;
            panelCargos.Add(5, -3, new RepString(fPropNormal, "Cargos Objetados"));
            panelCargos.AddAligned(posColumna1, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Fecha"));
            panelCargos.AddAligned(posColumna2, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Referencia"));
            panelCargos.AddAligned(posColumna3, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Descripción"));
            panelCargos.AddAligned(posColumna4, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Monto"));
            fPropNormal.bBold = false;
            fPropChica.bBold = false;

            //borde superior
            panelCargos.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelCargos.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));

            return panelCargos;
        }

        //Movimientos realizados
        private StaticContainer fnCrearPanelMovimientos()
        {
            StaticContainer panelMov = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 580;
            double pfAlto = 50;
            double posColumna1 = panelMov.rWidth * 0.01;
            double posColumna2 = panelMov.rWidth * 0.20;
            double posColumna3 = panelMov.rWidth * 0.35;
            double posColumna4 = panelMov.rWidth * 0.60;
            double posColumna5 = panelMov.rWidth * 0.75;
            double posColumna6 = panelMov.rWidth * 0.87;
            double posColumna7 = panelMov.rWidth * 1.0;
            double posColumna8 = panelMov.rWidth * 1.15;

            //borde superior
            panelMov.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelMov.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));

            fPropChica.bBold = true;
            fPropNormal.bBold = true;
            panelMov.Add(5, -3, new RepString(fPropNormal, "Movimientos Realizados"));
            panelMov.AddAligned(posColumna1 - 5, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Fecha Movimiento"));
            panelMov.AddAligned(posColumna3 - 5, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropChica, "Transacción"));
            panelMov.AddAligned(posColumna1, RepObj.rAlignLeft, 20, RepObj.rAlignBottom, new RepString(fPropChica, "Fecha Aplicación"));
            panelMov.AddAligned(posColumna2, RepObj.rAlignLeft, 20, RepObj.rAlignBottom, new RepString(fPropChica, "Referencia"));
            panelMov.AddAligned(posColumna3, RepObj.rAlignLeft, 20, RepObj.rAlignBottom, new RepString(fPropChica, "Descripción"));
            panelMov.AddAligned(posColumna4, RepObj.rAlignLeft, 20, RepObj.rAlignBottom, new RepString(fPropChica, "Forma Pago"));
            panelMov.AddAligned(posColumna5, RepObj.rAlignLeft, 20, RepObj.rAlignBottom, new RepString(fPropChica, "Cargo"));
            panelMov.AddAligned(posColumna6, RepObj.rAlignLeft, 20, RepObj.rAlignBottom, new RepString(fPropChica, "Abono"));
            panelMov.AddAligned(posColumna7, RepObj.rAlignLeft, 20, RepObj.rAlignBottom, new RepString(fPropChica, "Saldo"));
            panelMov.AddAligned(posColumna8, RepObj.rAlignLeft, 20, RepObj.rAlignBottom, new RepString(fPropChica, "Sobregiro"));
            panelMov.Add(5, pfAlto + 9, new RepString(fPropNormal, "TOTALES"));
            fPropNormal.bBold = false;
            fPropChica.bBold = false;

            return panelMov;

        }

       
        private MemoryStream fnGenerarGrafica()
        {
            double[] yValues = { 0, 0, 0, 0, 0, 0, 0 };
            string[] xValues = { "Saldo Inicial", "Abonos (+)", "Retiros (-)", "Comisiones (-)", "Intereses a favor (+)", "Otros Cargos(-)","Saldo Final" };

            //Otros Cargos  = ISRRetenido + IVAComisiones + IDE

            double ISRRetenido = 0;
            double IVAComisiones = 0;
            double IDE = 0;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            try { ISRRetenido = Convert.ToDouble(navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@ISRRetenido", nsmComprobante).Value); }
            catch { }
            try { IVAComisiones = Convert.ToDouble(navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@IVAComisiones", nsmComprobante).Value); }
            catch { }
            try { IDE = Convert.ToDouble(navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@IDE", nsmComprobante).Value); }
            catch { }
            try { yValues[0] = Convert.ToDouble(navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@ComisionesCobradas", nsmComprobante).Value); }
            catch { }
            try { yValues[1] = Convert.ToDouble(navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@Abonos", nsmComprobante).Value); }
            catch { }
            try { yValues[2] = Convert.ToDouble(navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@Cargos", nsmComprobante).Value); }
            catch { }
            try { yValues[3] = Convert.ToDouble(navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@SaldoDisponible", nsmComprobante).Value); }
            catch { }
            try { yValues[4] = Convert.ToDouble(navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Addenda/UnionProgreso/EC/@InteresPagado", nsmComprobante).Value); }
            catch { }
            yValues[5] = -2000;

          
            Chart chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            // chart1
            // 
            chartArea1.AlignmentOrientation = AreaAlignmentOrientations.Horizontal;
            
            chartArea1.BackColor = System.Drawing.Color.White;
            chartArea1.BackImageAlignment = ChartImageAlignmentStyle.Center;
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.BorderColor = System.Drawing.Color.Black;
            legend1.Docking = Docking.Left;
            legend1.IsDockedInsideChartArea = false;
            legend1.Alignment = StringAlignment.Center;
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new System.Drawing.Point(500, 500);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart1.Series.Add(series1);
            chart1.Size = new System.Drawing.Size(1000, 500);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);
            chart1.Series["Series1"].ChartType = SeriesChartType.Column;
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("Helvetica", 10f);
            chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = new Font("Helvetica", 10f);
            
           

            MemoryStream ms1 = new MemoryStream();
            byte[] byteImage = imageToByteArray(chart1);
            ms1 = new MemoryStream(byteImage);

            return ms1;

        }

        private StaticContainer fnContarCrearPie()
        {
            StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));
            return Pie;
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
                string sImagen = sNombreLogo; ;
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

        private byte[] imageToByteArray(Chart chart)
        {
            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
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
            NumaletUnionProg parser = new NumaletUnionProg();
            parser.LetraCapital = true;

            switch (psMoneda)
            {
                case "MXN":
                    parser.TipoMoneda = NumaletUnionProg.Moneda.Peso;
                    break;
                case "USD":
                    parser.TipoMoneda = NumaletUnionProg.Moneda.Dolar;
                    break;
                case "XEU":
                    parser.TipoMoneda = NumaletUnionProg.Moneda.Euro;
                    break;
            }

            languaje = new CultureInfo("es-Mx");

            return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
        }


    }

    public class TransformacionUnionProg
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
    public class ImpuestoUnionProg
    {
        public string Nombre { get; set; }
        public string Tasa { get; set; }
        public string Importe { get; set; }

        //Esta propiedad retorna el texto del renglon a mostrar en el PDF
        public string TextoImpuesto
        {
            get
            {
                return Nombre + " " + TransformacionUnionProg.fnFormatoPorcentaje(Tasa) + " " + TransformacionUnionProg.fnFormatoCurrency(Importe);
            }
        }

        /// <summary>
        /// Crea una nueva instancia de Impuesto
        /// </summary>
        /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
        /// <param name="nsmComprobante"></param>
        public ImpuestoUnionProg(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
    public class ImpuestoCompUnionProg
    {
        public string Nombre { get; set; }
        public string Tasa { get; set; }
        public string Importe { get; set; }

        //Esta propiedad retorna el texto del renglon a mostrar en el PDF
        public string TextoImpuesto
        {
            get
            {
                return Nombre + " " + TransformacionUnionProg.fnFormatoPorcentaje(Tasa) + " " + TransformacionUnionProg.fnFormatoCurrency(Importe);
            }
        }
        /// <summary>
        /// Crea una nueva instancia de Impuesto
        /// </summary>
        /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
        /// <param name="nsmComprobante"></param>
        public ImpuestoCompUnionProg(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
    public class DetalleUnionProg
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

        private List<ComprobanteImpuestosRetencionTUnionProg> _retencionesT;
        public List<ComprobanteImpuestosRetencionTUnionProg> RetencionesT
        {
            get { return _retencionesT; }
            set { _retencionesT = value; }
        }

        private List<ComprobanteImpuestosTrasladoTUnionProg> _trasladosT;
        public List<ComprobanteImpuestosTrasladoTUnionProg> TrasladosT
        {
            get { return _trasladosT; }
            set { _trasladosT = value; }
        }

        private t_UbicacionFiscalTUnionProg _ubicacionFiscalT;
        public t_UbicacionFiscalTUnionProg UbicacionFiscalT
        {
            get { return _ubicacionFiscalT; }
            set { _ubicacionFiscalT = value; }
        }

        private t_InformacionAduaneraTUnionProg _informacionAduaneraT;
        public t_InformacionAduaneraTUnionProg InformacionAduaneraT
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
        public DetalleUnionProg(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
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

    public partial class ComprobanteImpuestosRetencionTUnionProg
    {
        private ComprobanteImpuestosRetencionImpuestoTUnionProg impuestoField;
        private decimal importeField;

        public ComprobanteImpuestosRetencionImpuestoTUnionProg impuesto
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

    public enum ComprobanteImpuestosRetencionImpuestoTUnionProg
    {
        ISR, IVA,
    }

    public partial class ComprobanteImpuestosTrasladoTUnionProg
    {
        private ComprobanteImpuestosTrasladoImpuestoTUnionProg impuestoField;
        private decimal tasaField;
        private decimal importeField;

        public ComprobanteImpuestosTrasladoImpuestoTUnionProg impuesto
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

    public enum ComprobanteImpuestosTrasladoImpuestoTUnionProg
    {
        IVA, IEPS,
    }

    public partial class t_UbicacionFiscalTUnionProg
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

    public partial class t_InformacionAduaneraTUnionProg
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

    public sealed class NumaletUnionProg
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
        public NumaletUnionProg()
        {
            MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
            SeparadorDecimalSalida = SeparadorDecimalSalidaDefault;
            LetraCapital = LetraCapitalDefault;
            ConvertirDecimales = _convertirDecimales;
        }

        public NumaletUnionProg(Boolean ConvertirDecimales, String MascaraSalidaDecimal, String SeparadorDecimalSalida, Boolean LetraCapital)
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
            return NumaletUnionProg.ToString(Numero, CultureInfo.CurrentCulture);
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
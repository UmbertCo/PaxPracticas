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
using System.Collections;
using System.Text.RegularExpressions;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Fonts;
using PdfSharp.Pdf.Security;

namespace PDFSharpPlanitillaNomina2
{
    /// <summary>
    /// Los paneles se dibujan dinamicamente, hize la letra mas pequeña
    /// </summary>
    class PDFSharpNomina
    {

        public PdfDocument nomina;
        public XmlDocument gxComprobante;
        public XmlNamespaceManager nsmComprobante;
        XPathNavigator navEncabezado;

        public Queue<XGraphics> paginasReporte;  //Lista donde se almacenaran las paginas del documento

        string filename = "ferretNomina.pdf"; //Nombre de ruta\archivo
        private const string leyendaPDF = "ESTE DOCUMENTO ES UNA REPRESENTACIÓN GRÁFICA DE UN CFDI";

        private double puntoY;
        private double limiteY = 590; // Limite donde se dibujaran los paneles 

        XFont fuente14Negrita = new XFont("Harlow Solid Italic", 14, XFontStyle.Bold);
        XFont fuente12Normal = new XFont("Harlow Solid Italic", 12, XFontStyle.Regular);
        XFont fuente12Negrita = new XFont("Harlow Solid Italic", 12, XFontStyle.Bold);
        XFont fuente8Negrita = new XFont("Harlow Solid Italic", 8, XFontStyle.Bold);
        XFont fuente8Normal = new XFont("Harlow Solid Italic", 8, XFontStyle.Regular);
        XFont fuente6Normal = new XFont("Harlow Solid Italic", 6, XFontStyle.Regular);

        static double separacionVertical;

        XTextFormatter textFormatter;
        XImage logo;
        XStringFormat formatoRight;
        XStringFormat formatoLeft;

        XPen pen = new XPen(XColors.Navy, 1);

        XPen penFashion = new XPen(XColors.HotPink, 1); //Pen de pruebas  para saber donde se esta dibujando lo que sea.

        public Hashtable tipoIncapacidad;
        public Hashtable riesgoPuesto;
        public Hashtable regimenContratacion;
        public Hashtable catalogoBancos;
        public string sISR;

        static void Main(string[] args)
        {
            XmlDocument xmlDOC = new XmlDocument();
            xmlDOC.Load(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\PDFSharpPlanitillaNomina2\AAA010101AAA-1c6fd159-0261-4d51-9bd9-335f6af9ca69.xml");

            PDFSharpNomina nomina = new PDFSharpNomina(xmlDOC);
            nomina.fnGenerarPDF();
        }

        public PDFSharpNomina(XmlDocument pxComprobante)
        {
            try { logo = XImage.FromGdiPlusImage(Resource.ferret); }
            catch { }

            gxComprobante = pxComprobante;
            XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");

            tipoIncapacidad = new Hashtable();
            tipoIncapacidad[0] = "";
            tipoIncapacidad[1] = "Riesgo de trabajo";
            tipoIncapacidad[2] = "Enfermedad en general";
            tipoIncapacidad[3] = "Maternidad";

            riesgoPuesto = new Hashtable();
            riesgoPuesto[0] = "";
            riesgoPuesto[1] = "Clase I";
            riesgoPuesto[2] = "Clase II";
            riesgoPuesto[3] = "Clase III";
            riesgoPuesto[4] = "Clase IV";
            riesgoPuesto[5] = "Clase V";

            regimenContratacion = new Hashtable();
            regimenContratacion[0] = "";
            regimenContratacion[2] = "Sueldos y salarios";
            regimenContratacion[3] = "Jubilados";
            regimenContratacion[4] = "Pensionados";
            regimenContratacion[5] = "Asimilados a salarios, Miembros de las Sociedades Cooperativas de Producción";
            regimenContratacion[6] = "Asimilados a salarios, Integrantes de Sociedades y Asociaciones Civiles";
            regimenContratacion[7] = "Asimilados a salarios, Miembros de consejos directivos, de vigilancia, consultivos, honorarios a administradores, comisarios y gerentes generales";
            regimenContratacion[8] = "Asimilados a salarios, Actividad empresarial (comisionistas)";
            regimenContratacion[9] = "Asimilados a salarios, Honorarios asimilados a salarios";
            regimenContratacion[10] = "Asimilados a salarios, Ingresos acciones o títulos valor";

            catalogoBancos = new Hashtable();
            catalogoBancos[0] = "";
            catalogoBancos[2] = "BANAMEX";
            catalogoBancos[6] = "BANCOMEXT";
            catalogoBancos[9] = "BANOBRAS";
            catalogoBancos[12] = "BBVA BANCOMER";
            catalogoBancos[14] = "SANTANDER";
            catalogoBancos[19] = "BANJERCITO";
            catalogoBancos[21] = "HSBC";
            catalogoBancos[30] = "BAJIO";
            catalogoBancos[32] = "IXE";
            catalogoBancos[36] = "INBURSA";
            catalogoBancos[37] = "INTERACCIONES";
            catalogoBancos[42] = "MIFEL";
            catalogoBancos[44] = "SCOTIABANK";
            catalogoBancos[58] = "BANREGIO";
            catalogoBancos[59] = "INVEX";
            catalogoBancos[60] = "BANSI";
            catalogoBancos[62] = "AFIRME";
            catalogoBancos[72] = "BANORTE";
            catalogoBancos[102] = "THE ROYAL BANK";
            catalogoBancos[103] = "AMERICAN EXPRESS";
            catalogoBancos[106] = "BAMSA";
            catalogoBancos[108] = "TOKYO";
            catalogoBancos[110] = "JP MORGAN";
            catalogoBancos[112] = "BMONEX";
            catalogoBancos[113] = "VE POR MAS";
            catalogoBancos[116] = "ING";
            catalogoBancos[124] = "DEUTSCHE";
            catalogoBancos[126] = "CREDIT SUISSE";
            catalogoBancos[127] = "AZTECA";
            catalogoBancos[128] = "AUTOFIN";
            catalogoBancos[129] = "BARCLAYS";
            catalogoBancos[130] = "COMPARTAMOS";
            catalogoBancos[131] = "BANCO FAMSA";
            catalogoBancos[132] = "BMULTIVA";
            catalogoBancos[133] = "ACTINVER";
            catalogoBancos[134] = "WAL-MART";
            catalogoBancos[135] = "NAFIN";
            catalogoBancos[136] = "INTERBANCO";
            catalogoBancos[137] = "BANCOPPEL";
            catalogoBancos[138] = "ABC CAPITAL";
            catalogoBancos[139] = "UBS BANK";
            catalogoBancos[140] = "CONSUBANCO";
            catalogoBancos[141] = "VOLKSWAGEN";
            catalogoBancos[143] = "CIBANCO";
            catalogoBancos[145] = "BBASE";
            catalogoBancos[166] = "BANSEFI";
            catalogoBancos[168] = "HIPOTECARIA FEDERAL";
            catalogoBancos[600] = "MONEXCB";
            catalogoBancos[601] = "GBM";
            catalogoBancos[602] = "MASARI";
            catalogoBancos[605] = "VALUE";
            catalogoBancos[606] = "ESTRUCTURADORES";
            catalogoBancos[607] = "TIBER";
            catalogoBancos[608] = "VECTOR";
            catalogoBancos[610] = "BB&BB";
            catalogoBancos[614] = "ACCIVAL";
            catalogoBancos[615] = "MERRILL LYNCH";
            catalogoBancos[616] = "FINAMEX";
            catalogoBancos[617] = "VALMEX";
            catalogoBancos[618] = "UNICA";
            catalogoBancos[619] = "MAPFRE";
            catalogoBancos[620] = "PROFUTURO";
            catalogoBancos[621] = "CB ACTINVER";
            catalogoBancos[622] = "OACTIN";
            catalogoBancos[623] = "SKANDIA";
            catalogoBancos[626] = "CBDEUTSCHE";
            catalogoBancos[627] = "ZURICH";
            catalogoBancos[628] = "ZURICHVI";
            catalogoBancos[629] = "SU CASITA";
            catalogoBancos[630] = "CB INTERCAM";
            catalogoBancos[631] = "CI BOLSA";
            catalogoBancos[632] = "BULLTICK CB";
            catalogoBancos[633] = "STERLING";
            catalogoBancos[634] = "FINCOMUN";
            catalogoBancos[636] = "HDI SEGUROS";
            catalogoBancos[637] = "ORDER";
            catalogoBancos[638] = "AKALA";
            catalogoBancos[640] = "CB JPMORGAN";
            catalogoBancos[642] = "REFORMA";
            catalogoBancos[646] = "STP";
            catalogoBancos[647] = "TELECOMM";
            catalogoBancos[648] = "EVERCORE";
            catalogoBancos[649] = "SKANDIA";
            catalogoBancos[651] = "SEGMTY";
            catalogoBancos[652] = "ASEA";
            catalogoBancos[653] = "KUSPIT";
            catalogoBancos[655] = "SOFIEXPRESS";
            catalogoBancos[656] = "UNAGRA";
            catalogoBancos[659] = "OPCIONES EMPRESARIALES DEL NOROESTE";
            catalogoBancos[901] = "CLS";
            catalogoBancos[902] = "INDEVAL";
            catalogoBancos[670] = "LIBERTAD";

            nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");
            navEncabezado = gxComprobante.CreateNavigator();
        }

        public void setISR(XmlDocument pxComprobante)
        {
            this.sISR = "0";

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");

            XPathNavigator nav = gxComprobante.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("cfdi:Comprobante/cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion", nsmComprobante);

            while (iterator.MoveNext())
            {
                string sTipo = "";
                string sImporte = "";

                try
                {
                    sTipo = iterator.Current.SelectSingleNode("@impuesto", nsmComprobante).ToString();
                    sImporte = iterator.Current.SelectSingleNode("@importe", nsmComprobante).ToString();
                }
                catch { }

                if (sTipo == "ISR") { this.sISR = sImporte; }
            }


        }

        public void fnGenerarPDF()
        {
            setISR(gxComprobante);

            nomina = new PdfDocument();
            nomina.Info.Title = "CFDI";
            nomina.Info.Author = "CORPUS Facturación";

            PdfPage pagina = nomina.AddPage();
            pagina.Size = PageSize.A4;
            XGraphics gfx = XGraphics.FromPdfPage(pagina);

            separacionVertical = gfx.MeasureString("A", fuente8Normal).Height;

            paginasReporte = new Queue<XGraphics>();
            paginasReporte.Enqueue(gfx);

            textFormatter = new XTextFormatter(gfx);
            formatoRight = new XStringFormat();
            formatoLeft = new XStringFormat();

            formatoRight.Alignment = XStringAlignment.Far;
            formatoLeft.Alignment = XStringAlignment.Near;

            fnDibujarEncabezadoPiePagina(ref gfx);
            this.puntoY = fnDibujarContenido(ref gfx);

            fnDibujarPanelPercepciones(ref gfx, ref this.puntoY, ref pagina);
            fnDibujaPanelHorasExtra(ref gfx, ref this.puntoY, ref pagina);
            fnDibujaPanelSumaPercepciones(ref gfx, ref this.puntoY, ref pagina);

            fnDibujaPanelDeducciones(ref gfx, ref this.puntoY, ref pagina);
            fnDibujaPanelIncapacidad(ref gfx, ref this.puntoY, ref pagina);
            fnDibujaPanelSumaDeducciones(ref gfx, ref this.puntoY, ref pagina);

            fnDibujarPanelTotales(ref gfx);
            fnDibujarPanelSellos(ref gfx);


            //Dibujamos el total de numero de paginas en todas las paginas del reporte
            foreach (XGraphics grafix in paginasReporte)
            {
                grafix.DrawString(Convert.ToString(nomina.PageCount), fuente8Normal, XBrushes.Black, 515, 805, formatoLeft);
            }



            nomina.Save(filename);

        }

        public void fnDibujarEncabezadoPiePagina(ref XGraphics gfx)
        {
            fnDibujarEncabezado(ref gfx);
            fnDibujarPiePagina(ref gfx);
        }

        public void fnDibujarEncabezado(ref XGraphics gfx)
        {
            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, 12, 568, 770, 10, 10);

            fnDibujarLogo(ref gfx);
            fnCrearPanelEmisor(ref gfx);
            fnCrearPanelRecibo(ref gfx);
            fnCrearPanelExpedidoEn(ref gfx);

        }

        public void fnDibujarPiePagina(ref XGraphics gfx)
        {
            fnDibujarLogoPax(ref gfx);
            gfx.DrawString("http://www.paxfacturacion.com/", fuente8Normal, XBrushes.Black, 50, 805, formatoLeft);
            gfx.DrawString(leyendaPDF, fuente8Normal, XBrushes.Black, 170, 805, formatoLeft);
            gfx.DrawString(Convert.ToString(nomina.PageCount) + " de", fuente8Normal, XBrushes.Black, 500, 805, formatoLeft);
        }

        public double fnDibujarContenido(ref XGraphics gfx)
        {
            double posicionY = fnCrearPanelDatosEmpleado(ref gfx);
            posicionY = fnCrearPanelInformacionLaboral(ref gfx, posicionY);
            posicionY = fnCrearPanelInformacionGeneral(ref gfx,posicionY);

            return posicionY;
        }

        public void fnDibujarLogo(ref XGraphics gfx)
        {
            gfx.DrawImage(logo, 20, 20, 80, 80);
        }

        public void fnDibujarLogoPax(ref XGraphics gfx)
        {
            gfx.DrawImage(logo, 20, 800, 20, 20);
        }

        public void fnCrearPanelEmisor(ref XGraphics gfx)
        {
            fnDatosPanelEmisor(nsmComprobante, navEncabezado, ref gfx);
        }

        public void fnDatosPanelEmisor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, ref XGraphics gfx)
        {
            string sEmisor, sRFC, sCodigoPostal, sPais, sEstado, sMunicipio, sLocalidad, sColonia, sNoInterior, sNoExterior, sCalle, sRegimenFiscal, sRegistroPatronal;

            sEmisor = sRFC = sCodigoPostal = sPais = sEstado = sMunicipio = sLocalidad = sColonia = sNoInterior = sNoExterior = sCalle = sRegimenFiscal = sRegistroPatronal = "";

            try { sEmisor = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value; }
            catch { }
            try { sRFC = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@rfc", nsmComprobante).Value; }
            catch { }
            try { sCodigoPostal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@codigoPostal", nsmComprobante).Value; }
            catch { }
            try { sPais = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@pais", nsmComprobante).Value; }
            catch { }
            try { sEstado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@estado", nsmComprobante).Value; }
            catch { }
            try { sMunicipio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@municipio", nsmComprobante).Value; }
            catch { }
            try { sLocalidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@localidad", nsmComprobante).Value; }
            catch { }
            try { sColonia = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@colonia", nsmComprobante).Value; }
            catch { }
            try { sNoInterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@noInterior", nsmComprobante).Value; }
            catch { }
            try { sNoExterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@noExterior", nsmComprobante).Value; }
            catch { }
            try { sCalle = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@calle", nsmComprobante).Value; }
            catch { }
            try { sRegimenFiscal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:RegimenFiscal/@Regimen", nsmComprobante).Value; }
            catch { }
            try { sRegistroPatronal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@RegistroPatronal", nsmComprobante).Value; }
            catch { }

            string sDireccion1 = sCalle + " No. Interior: " + " " + sNoInterior + "No.Exterior: " + sNoExterior + " Colonia " + sColonia;
            string sDireccion2 = sLocalidad + ", " + sMunicipio + ", " + sEstado + ". " + sPais + " C.P. " + sCodigoPostal;

            gfx.DrawString(sEmisor, fuente8Negrita, XBrushes.Black, new Point(100, 30), formatoLeft);
            gfx.DrawString(sDireccion1, fuente8Normal, XBrushes.Black, new Point(100, 50), formatoLeft);
            gfx.DrawString(sDireccion2, fuente8Normal, XBrushes.Black, new Point(100, 60), formatoLeft);
            gfx.DrawString("RFC: " + sRFC, fuente8Normal, XBrushes.Black, new Point(100, 70), formatoLeft);
            gfx.DrawString("Registro Patronal: " + sRegistroPatronal, fuente8Normal, XBrushes.Black, new Point(100, 80), formatoLeft);
            gfx.DrawString("Registro Fiscal: " + sRegimenFiscal, fuente8Normal, XBrushes.Black, new Point(100, 90), formatoLeft);

        }

        public void fnCrearPanelRecibo(ref XGraphics gfx)
        {
            Rectangle rect = new Rectangle(470, 20, 100, 100);
            
            gfx.DrawRoundedRectangle(pen, XBrushes.White, 370, 10, 210, 52, 20, 20);
            gfx.DrawLine(pen, 370, 30, 580, 30);

            gfx.DrawString("RECIBO DE NÓMINA", fuente12Negrita, XBrushes.Black, new Point(500, 20), XStringFormats.Center);
            gfx.DrawString("Folio Fiscal:", fuente8Negrita, XBrushes.Black, new Point(570, 40), formatoRight);
            gfx.DrawString("No. de Serie del Certificado del Emisor:", fuente8Negrita, XBrushes.Black, new Point(570, 60), formatoRight);
            gfx.DrawString("No. de Serie del certificado del SAT:", fuente8Negrita, XBrushes.Black, new Point(570, 80), formatoRight);
            gfx.DrawString("Fecha y hora de certificación:", fuente8Negrita, XBrushes.Black, new Point(570, 100), formatoRight);
            gfx.DrawString("Lugar, fecha y hora de emisión:", fuente8Negrita, XBrushes.Black, new Point(570, 120), formatoRight);

            fnDatosPanelRecibo(nsmComprobante, navEncabezado, ref gfx);
        }

        public void fnDatosPanelRecibo(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, ref XGraphics gfx)
        {
            string sFolioFiscal, sCertificadoEmisor, sCertificadoSAT, sFechaCertificacion, sFechaEmision, sLugarEmision;
            sFolioFiscal = sCertificadoEmisor = sCertificadoSAT = sFechaCertificacion = sFechaEmision = sLugarEmision = "";

            try { sFolioFiscal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
            catch { }
            try { sCertificadoEmisor = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@noCertificado", nsmComprobante).Value; }
            catch { }
            try { sCertificadoSAT = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
            catch { }
            try { sFechaCertificacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value; }
            catch { }
            try { sFechaEmision = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@fecha", nsmComprobante).Value; }
            catch { }
            try { sLugarEmision = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", nsmComprobante).Value; }
            catch { }

            gfx.DrawString(sFolioFiscal, fuente8Normal, XBrushes.Black, new Point(570, 50), formatoRight);
            gfx.DrawString(sCertificadoEmisor, fuente8Normal, XBrushes.Black, new Point(570, 70), formatoRight);
            gfx.DrawString(sCertificadoSAT, fuente8Normal, XBrushes.Black, new Point(570, 90), formatoRight);
            gfx.DrawString(sFechaCertificacion, fuente8Normal, XBrushes.Black, new Point(570, 110), formatoRight);
            gfx.DrawString(sLugarEmision + " " + sFechaEmision, fuente8Normal, XBrushes.Black, new Point(570, 130), formatoRight);

        }

        public void fnCrearPanelExpedidoEn(ref XGraphics gfx)
        {

            fnDatosPanelExpedidoEn(nsmComprobante, navEncabezado, ref gfx);

        }

        public void fnDatosPanelExpedidoEn(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, ref XGraphics gfx)
        {
            string sCalle, sNoCalle, sNoInterior, sNoExterior, sColonia, sLocalidad, sEstado, sPais, sCodigoPostal, sMunicipio;

            sCalle = sNoCalle = sNoInterior = sNoExterior = sColonia = sLocalidad = sEstado = sPais = sCodigoPostal = sMunicipio = "";

            try { sCodigoPostal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@codigoPostal", nsmComprobante).Value; }
            catch { }
            try { sPais = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@pais", nsmComprobante).Value; }
            catch { }
            try { sEstado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@estado", nsmComprobante).Value; }
            catch { }
            try { sMunicipio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@municipio", nsmComprobante).Value; }
            catch { }
            try { sLocalidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@localidad", nsmComprobante).Value; }
            catch { }
            try { sColonia = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@colonia", nsmComprobante).Value; }
            catch { }
            try { sNoInterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@noInterior", nsmComprobante).Value; }
            catch { }
            try { sNoExterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@noExterior", nsmComprobante).Value; }
            catch { }
            try { sCalle = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:ExpedidoEn/@calle", nsmComprobante).Value; }
            catch { }

            if (sCalle != string.Empty)
            {

                string sDireccion1 = sCalle + " No. Interior: " + " " + sNoInterior + "No.Exterior: " + sNoExterior + " Colonia " + sColonia;
                string sDireccion2 = sMunicipio + ", " + sLocalidad + ", " + sEstado + ". " + sPais + " C.P. " + sCodigoPostal;

                gfx.DrawString("Expedido En:", fuente8Negrita, XBrushes.Black, new Point(20, 110), formatoLeft);
                gfx.DrawString(sDireccion1, fuente8Normal, XBrushes.Black, new Point(20, 120), formatoLeft);
                gfx.DrawString(sDireccion2, fuente8Normal, XBrushes.Black, new Point(20, 130), formatoLeft);
            }

        }

        public double fnCrearPanelDatosEmpleado(ref XGraphics gfx)
        {
            double posYInicial = 150;
            double posYInicial2 = 150 + 12;

            string sNombre, sRFC, sRegimen, sRiesgoPuesto, sNoEmpleado, sCURP, sPuesto, sNNS, sDepartamento;
            sNombre = sRFC = sRegimen = sRiesgoPuesto = sNoEmpleado = sCURP = sPuesto = sNNS = sDepartamento = "";

            try { sNombre = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value; }
            catch { }
            try { sRFC = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value; }
            catch { }
            try { sRegimen = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@TipoRegimen", nsmComprobante).Value; } //Donde viene en el xml!?
            catch { sRegimen = "0"; }
            try { sRiesgoPuesto = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@RiesgoPuesto", nsmComprobante).Value; }
            catch { sRiesgoPuesto = "0"; }
            try { sNoEmpleado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@NumEmpleado", nsmComprobante).Value; }
            catch { }
            try { sCURP = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@CURP", nsmComprobante).Value; }
            catch { }
            try { sPuesto = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@Puesto", nsmComprobante).Value; }
            catch { }
            try { sNNS = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@NumSeguridadSocial", nsmComprobante).Value; }
            catch { }
            try { sDepartamento = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@Departamento", nsmComprobante).Value; }
            catch { }

            try
            {
                int riesgoPuestos = Convert.ToInt16(sRiesgoPuesto);
                int nRegimen = Convert.ToInt16(sRegimen);
                sRiesgoPuesto = (string)riesgoPuesto[riesgoPuestos];
                sRegimen = (string)regimenContratacion[nRegimen];

                if (sRiesgoPuesto == null) { sRiesgoPuesto = ""; }
                if (sRegimen == null) { sRegimen = ""; }
            }
            catch { }



            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, posYInicial, 570, 12, 10, 10);
            gfx.DrawString("Datos del Empleado", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);

            posYInicial += 12;

            gfx.DrawString("Nombre:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sNombre, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("Puesto:", fuente8Negrita, XBrushes.Black, 20,posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sPuesto, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("Régimen:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sRegimen, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("Departamento:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sDepartamento, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("Riesgo Puesto:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sRiesgoPuesto, 40, 110, posYInicial, fuente8Normal, ref gfx);

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            gfx.DrawString("No. Empleado:", fuente8Negrita, XBrushes.Black, 330, posYInicial2, formatoLeft);
            posYInicial2 = fnEscribirMultilinea(sNoEmpleado, 40, 400, posYInicial2, fuente8Normal, ref gfx);

            posYInicial2 += separacionVertical;

            gfx.DrawString("N.N.S:", fuente8Negrita, XBrushes.Black, 330, posYInicial2, formatoLeft);
            posYInicial2 = fnEscribirMultilinea(sNNS, 40, 400, posYInicial2, fuente8Normal, ref gfx);

            posYInicial2 += separacionVertical;

            gfx.DrawString("CURP:", fuente8Negrita, XBrushes.Black, 330, posYInicial2, formatoLeft);
            posYInicial2 = fnEscribirMultilinea(sCURP, 40, 400, posYInicial2, fuente8Normal, ref gfx);

            posYInicial2 += separacionVertical;

            gfx.DrawString("RFC:", fuente8Negrita, XBrushes.Black, 330, posYInicial2, formatoLeft);
            posYInicial2 = fnEscribirMultilinea(sRFC, 40, 400, posYInicial2, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            return posYInicial;
        }

        public double fnCrearPanelInformacionLaboral(ref XGraphics gfx, double posicionY)
        {
            double posYInicial = posicionY;
            double posYInicial2 = posicionY + 12;

            string sContrato, sDiasPagados, sSalarioBase, sFechaPago, sJornada, sPeriodo, sAntiguedad, sSalarioDiario, sDelegacion, sFormaPago, sBanco, sMetodoPago, sCLABE;
            sContrato = sDiasPagados = sSalarioBase = sFechaPago = sJornada = sPeriodo = sAntiguedad = sSalarioDiario = sDelegacion = sFormaPago = sMetodoPago = sCLABE = sBanco = "";

            try { sContrato = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@TipoContrato", nsmComprobante).Value; }
            catch { }
            try { sDiasPagados = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@NumDiasPagados", nsmComprobante).Value; }
            catch { }
            try { sSalarioBase = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@SalarioBaseCotApor", nsmComprobante).Value; }
            catch { }
            try { sFechaPago = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@FechaPago", nsmComprobante).Value; }
            catch { }
            try { sJornada = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@TipoJornada", nsmComprobante).Value; }
            catch { }
            try { sPeriodo = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@PeriodicidadPago", nsmComprobante).Value; }
            catch { }
            try { sAntiguedad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@Antiguedad", nsmComprobante).Value; }
            catch { }
            try { sSalarioDiario = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@SalarioDiarioIntegrado", nsmComprobante).Value; }
            catch { }


            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, posYInicial, 570, 12, 10, 10);
            gfx.DrawString("Información Laboral", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial += 12;

            gfx.DrawString("Contrato:", fuente8Negrita, XBrushes.Black, 20,posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sContrato, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("Periodo:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sPeriodo, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("Antigüedad:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sAntiguedad, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("Salario Base:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sSalarioBase, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("Jornada:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sJornada, 40, 110, posYInicial, fuente8Normal, ref gfx);

            /////////////////////////////////////////////////////////////////////////////////////////////////////

            gfx.DrawString("Fecha de Pago:", fuente8Negrita, XBrushes.Black, 330,posYInicial2, formatoLeft);
            posYInicial2 = fnEscribirMultilinea(sFechaPago, 40, 400, posYInicial2, fuente8Normal, ref gfx);

            posYInicial2 += separacionVertical;

            gfx.DrawString("Dias Pagados:", fuente8Negrita, XBrushes.Black, 330, posYInicial2, formatoLeft);
            posYInicial2 = fnEscribirMultilinea(sDiasPagados, 40, 400, posYInicial2, fuente8Normal, ref gfx);

            posYInicial2 += separacionVertical;

            gfx.DrawString("Salario Diario:", fuente8Negrita, XBrushes.Black, 330, posYInicial2, formatoLeft);
            posYInicial2 = fnEscribirMultilinea(sSalarioDiario, 40, 400, posYInicial2, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            return posYInicial;
           

        }

        public double fnCrearPanelInformacionGeneral(ref XGraphics gfx, double posicionY)
        {
            double posYInicial = posicionY;
            double posYInicial2 = posicionY + 12;

            string sDelegacion, sBanco, sClabe, sFormaPago, sMetodoPago;
            sDelegacion = sBanco = sClabe = sFormaPago = sMetodoPago = "";

            sBanco = "0";
            try { sDelegacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@municipio", nsmComprobante).Value; }
            catch { }
            try { sClabe = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@CLABE", nsmComprobante).Value; }
            catch { }
            try { sFormaPago = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@formaDePago", nsmComprobante).Value; }
            catch { }
            try { sMetodoPago = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@metodoDePago", nsmComprobante).Value; }
            catch { }
            try { sBanco = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@Banco", nsmComprobante).Value; }
            catch { }
            try
            {
                int nBanco = Convert.ToInt16(sBanco);
                sBanco = (string)catalogoBancos[nBanco];
                if (sBanco == null) { sBanco = ""; }
            }
            catch
            {
                // clsLog.EscribirLog("Error al obtener dato de Banco");
            }



            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, posYInicial, 570, 12, 10, 10);
            gfx.DrawString("Información General", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);

            posYInicial += 12;

            gfx.DrawString("Delegación/Municipio:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sDelegacion, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("Banco:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sBanco, 40, 110, posYInicial, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            gfx.DrawString("CLABE:", fuente8Negrita, XBrushes.Black, 20, posYInicial, formatoLeft);
            posYInicial = fnEscribirMultilinea(sClabe, 40, 110, posYInicial, fuente8Normal, ref gfx);

            ////////////////////////////////////////////////////////////////////////////////////////////////////////

            gfx.DrawString("Forma de Pago:", fuente8Negrita, XBrushes.Black, 330, posYInicial2, formatoLeft);
            posYInicial2 = fnEscribirMultilinea(sFormaPago, 40, 400, posYInicial2, fuente8Normal, ref gfx);

            posYInicial2 += separacionVertical;

            gfx.DrawString("Método de Pago:", fuente8Negrita, XBrushes.Black, 330, posYInicial2, formatoLeft);
            posYInicial2 = fnEscribirMultilinea(sMetodoPago, 40, 400, posYInicial2, fuente8Normal, ref gfx);

            posYInicial += separacionVertical;

            return posYInicial;
        }

        public void fnDibujarPanelPercepciones(ref XGraphics gfx, ref double posY, ref PdfPage pagina)
        {
            fnCrearEncabezadoPercepciones(ref gfx, ref  posY);

            XPathNavigator nav = gxComprobante.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Percepciones/nomina:Percepcion", nsmComprobante);

            while (iterator.MoveNext())
            {
                string sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento;
                sTipo = sClave = sConcepto = sImporteGravado = sImporteExcento = "";

                try { sTipo = iterator.Current.SelectSingleNode("@TipoPercepcion", nsmComprobante).ToString(); }
                catch { }
                try { sClave = iterator.Current.SelectSingleNode("@Clave", nsmComprobante).ToString(); }
                catch { }
                try { sConcepto = iterator.Current.SelectSingleNode("@Concepto", nsmComprobante).ToString(); }
                catch { }
                try { sImporteGravado = iterator.Current.SelectSingleNode("@ImporteGravado", nsmComprobante).ToString(); }
                catch { }
                try { sImporteExcento = iterator.Current.SelectSingleNode("@ImporteExento", nsmComprobante).ToString(); }
                catch { }

                if ((limiteY - posY) > 12)
                {
                    fnDibujaDatosPanelPercepciones(ref gfx, ref posY, sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento);
                }
                else
                {
                    gfx.DrawLine(pen, 12, posY, 580, posY);
                    pagina = nomina.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    paginasReporte.Enqueue(gfx);
                    fnDibujarEncabezadoPiePagina(ref gfx);
                    posY = 150;
                    fnCrearEncabezadoPercepciones(ref gfx, ref  posY);
                    fnDibujaDatosPanelPercepciones(ref gfx, ref posY, sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento);

                }
            }
        }

        public void fnCrearEncabezadoPercepciones(ref XGraphics gfx, ref double posY)
        {
            gfx.DrawRoundedRectangle(pen, XBrushes.Navy, 12, posY, 570, 12, 10, 10);
            gfx.DrawString("PERCEPCIONES", fuente8Negrita, XBrushes.White, 20, posY, formatoLeft);
            posY += 12;
            gfx.DrawString("Tipo", fuente8Negrita, XBrushes.Black, 20, posY, formatoLeft);
            gfx.DrawString("Clave", fuente8Negrita, XBrushes.Black, 70, posY, formatoLeft);
            gfx.DrawString("Concepto", fuente8Negrita, XBrushes.Black, 130, posY, formatoLeft);
            gfx.DrawString("Importe Gravado", fuente8Negrita, XBrushes.Black, 400, posY, formatoLeft);
            gfx.DrawString("Importe Exento", fuente8Negrita, XBrushes.Black, 500, posY, formatoLeft);
            posY += 10;
        }

        public void fnCrearPanelRedondeadoPercepciones(ref XGraphics gfx, ref double posY)
        {
            gfx.DrawRoundedRectangle(pen, XBrushes.Navy, 12, posY, 570, 12, 10, 10);
            gfx.DrawString("PERCEPCIONES", fuente8Negrita, XBrushes.White, 20, posY, formatoLeft);
            posY += 12;
        }

        public void fnDibujaDatosPanelPercepciones(ref XGraphics gfx, ref double posY, string sTipo, string sClave, string sConcepto, string sImporteGravado, string sImporteEcento)
        {
            sImporteGravado = TransformacionNomina.fnFormatoRedondeo(sImporteGravado);
            sImporteEcento = TransformacionNomina.fnFormatoRedondeo(sImporteEcento);

            double pos = posY;
            gfx.DrawString(sTipo, fuente8Normal, XBrushes.Black, 20, pos, formatoLeft);
            gfx.DrawString(sClave, fuente8Normal, XBrushes.Black, 70, pos, formatoLeft);
            posY = fnEscribirMultilinea(sConcepto, 50, 130, posY, fuente8Normal, ref gfx);
            gfx.DrawString(sImporteGravado, fuente8Normal, XBrushes.Black, 400, pos, formatoLeft);
            gfx.DrawString(sImporteEcento, fuente8Normal, XBrushes.Black, 500, pos, formatoLeft);

            posY += 10;

        }

        public void fnDibujaPanelHorasExtra(ref XGraphics gfx, ref double posY, ref PdfPage pagina)
        {
            if ((limiteY - posY) >= 20)
            {
                fnCrearEncabezadoHorasExtra(ref gfx, ref posY);
            }
            else
            {
                pagina = nomina.AddPage();
                gfx = XGraphics.FromPdfPage(pagina);
                paginasReporte.Enqueue(gfx);
                fnDibujarEncabezadoPiePagina(ref gfx);
                posY = 150;
                fnCrearPanelRedondeadoPercepciones(ref gfx, ref  posY);
                fnCrearEncabezadoHorasExtra(ref gfx, ref  posY);
            }

            XPathNavigator nav = gxComprobante.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:HorasExtras/nomina:HorasExtra", nsmComprobante);

            while (iterator.MoveNext())
            {
                string sDias, sHorasExtra, sTipoHora, sImportePagado;
                sDias = sHorasExtra = sTipoHora = sImportePagado = "";

                try { sDias = iterator.Current.SelectSingleNode("@Dias", nsmComprobante).ToString(); }
                catch { }
                try { sHorasExtra = iterator.Current.SelectSingleNode("@HorasExtra", nsmComprobante).ToString(); }
                catch { }
                try { sTipoHora = iterator.Current.SelectSingleNode("@TipoHoras", nsmComprobante).ToString(); }
                catch { }
                try { sImportePagado = iterator.Current.SelectSingleNode("@ImportePagado", nsmComprobante).ToString(); }
                catch { }

                if ((limiteY - posY) > 12)
                {

                    fnDibujaDatosPanelHorasExtra(ref gfx, ref posY, sDias, sHorasExtra, sTipoHora, sImportePagado);

                }
                else
                {
                    gfx.DrawLine(pen, 12, posY, 580, posY);
                    pagina = nomina.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    paginasReporte.Enqueue(gfx);
                    fnDibujarEncabezadoPiePagina(ref gfx);
                    posY = 150;
                    fnCrearPanelRedondeadoPercepciones(ref gfx, ref  posY);
                    fnCrearEncabezadoHorasExtra(ref gfx, ref  posY);
                    fnDibujaDatosPanelHorasExtra(ref gfx, ref posY, sDias, sHorasExtra, sTipoHora, sImportePagado);
                }

            }
        }

        public void fnCrearEncabezadoHorasExtra(ref XGraphics gfx, ref double posY)
        {
            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, posY, 570, 12, 10, 10);
            gfx.DrawString("Días", fuente8Negrita, XBrushes.Black, 20, posY, formatoLeft);
            gfx.DrawString("Horas Extra", fuente8Negrita, XBrushes.Black, 70, posY, formatoLeft);
            gfx.DrawString("Tipo Hora", fuente8Negrita, XBrushes.Black, 270, posY, formatoLeft);
            gfx.DrawString("Importe Pagado", fuente8Negrita, XBrushes.Black, 500, posY, formatoLeft);

            posY += 12;

        }

        public void fnDibujaDatosPanelHorasExtra(ref XGraphics gfx, ref double posY, string sDias, string sHorasExtra, string sTipoHora, string sImportePagado)
        {

            gfx.DrawString(sDias, fuente8Normal, XBrushes.Black, 20, posY, formatoLeft);
            gfx.DrawString(sHorasExtra, fuente8Normal, XBrushes.Black, 70, posY, formatoLeft);
            gfx.DrawString(sTipoHora, fuente8Normal, XBrushes.Black, 275, posY, formatoLeft);
            gfx.DrawString(sImportePagado, fuente8Normal, XBrushes.Black, 500, posY, formatoLeft);

            posY += 10;

        }

        public void fnDibujaPanelSumaPercepciones(ref XGraphics gfx, ref double posY, ref PdfPage pagina)
        {
            if ((limiteY - posY) >= 20)
            {
                fnCrearPanelSumaPercepciones(ref gfx, ref posY);
            }
            else
            {
                gfx.DrawLine(pen, 12, posY, 580, posY);
                pagina = nomina.AddPage();
                gfx = XGraphics.FromPdfPage(pagina);
                paginasReporte.Enqueue(gfx);
                fnDibujarEncabezadoPiePagina(ref gfx);
                posY = 150;
                fnCrearPanelRedondeadoPercepciones(ref gfx, ref  posY);
                fnCrearPanelSumaPercepciones(ref gfx, ref  posY);
            }

        }

        public void fnCrearPanelSumaPercepciones(ref XGraphics gfx, ref double posY)
        {
            XPathNavigator nav = gxComprobante.CreateNavigator();
            string sSumaPercepciones = "";
            try { sSumaPercepciones = nav.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsmComprobante).Value; }
            catch { }

            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, posY, 570, 12, 10, 10);
            gfx.DrawString("Suma Percepciones", fuente8Negrita, XBrushes.Black, 400, posY, formatoLeft);
            gfx.DrawString(sSumaPercepciones, fuente8Negrita, XBrushes.Black, 500, posY, formatoLeft);
            posY += 12;
        }

        public void fnDibujaPanelDeducciones(ref XGraphics gfx, ref double posY, ref PdfPage pagina)
        {
            if ((limiteY - posY) >= 20)
            {
                fnCrearEncabezadoDeducciones(ref gfx, ref posY);
            }
            else
            {
                gfx.DrawLine(pen, 12, posY, 580, posY);
                pagina = nomina.AddPage();
                gfx = XGraphics.FromPdfPage(pagina);
                paginasReporte.Enqueue(gfx);
                fnDibujarEncabezadoPiePagina(ref gfx);
                posY = 150;
                fnCrearEncabezadoDeducciones(ref gfx, ref  posY);
            }

            XPathNavigator nav = gxComprobante.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Deducciones/nomina:Deduccion", nsmComprobante);

            while (iterator.MoveNext())
            {
                string sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento;
                sTipo = sClave = sConcepto = sImporteGravado = sImporteExcento = "";

                try { sTipo = iterator.Current.SelectSingleNode("@TipoDeduccion", nsmComprobante).ToString(); }
                catch { }
                try { sClave = iterator.Current.SelectSingleNode("@Clave", nsmComprobante).ToString(); }
                catch { }
                try { sConcepto = iterator.Current.SelectSingleNode("@Concepto", nsmComprobante).ToString(); }
                catch { }
                try { sImporteGravado = iterator.Current.SelectSingleNode("@ImporteGravado", nsmComprobante).ToString(); }
                catch { }
                try { sImporteExcento = iterator.Current.SelectSingleNode("@ImporteExento", nsmComprobante).ToString(); }
                catch { }

                if ((limiteY - posY) > 12)
                {
                    fnDibujaDatosPanelDeducciones(ref gfx, ref  posY, sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento);
                }
                else
                {
                    gfx.DrawLine(pen, 12, posY, 580, posY);
                    pagina = nomina.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    paginasReporte.Enqueue(gfx);
                    fnDibujarEncabezadoPiePagina(ref gfx);
                    posY = 150;
                    fnCrearEncabezadoDeducciones(ref gfx, ref  posY);
                    fnDibujaDatosPanelDeducciones(ref gfx, ref  posY, sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento);
                }

            }
        }

        public void fnCrearEncabezadoDeducciones(ref XGraphics gfx, ref double posY)
        {
            gfx.DrawRoundedRectangle(pen, XBrushes.Navy, 12, posY, 570, 12, 10, 10);
            gfx.DrawString("DEDUCCIONES", fuente8Negrita, XBrushes.White, 20, posY, formatoLeft);
            posY += 12;
            gfx.DrawString("Tipo", fuente8Negrita, XBrushes.Black, 20, posY, formatoLeft);
            gfx.DrawString("Clave", fuente8Negrita, XBrushes.Black, 70, posY, formatoLeft);
            gfx.DrawString("Concepto", fuente8Negrita, XBrushes.Black, 130, posY, formatoLeft);
            gfx.DrawString("Importe Gravado", fuente8Negrita, XBrushes.Black, 400, posY, formatoLeft);
            gfx.DrawString("Importe Exento", fuente8Negrita, XBrushes.Black, 500, posY, formatoLeft);
            posY += 10;
        }

        public void fnCrearPanelRedondeadoDeducciones(ref XGraphics gfx, ref double posY)
        {
            gfx.DrawRoundedRectangle(pen, XBrushes.Navy, 12, posY, 570, 12, 10, 10);
            gfx.DrawString("DEDUCCIONES", fuente8Negrita, XBrushes.White, 20, posY, formatoLeft);
            posY += 12;
        }

        public void fnDibujaDatosPanelDeducciones(ref XGraphics gfx, ref double posY, string sTipo, string sClave, string sConcepto, string sImporteGravado, string sImporteExcento)
        {
            sImporteExcento = TransformacionNomina.fnFormatoRedondeo(sImporteExcento);
            sImporteGravado = TransformacionNomina.fnFormatoRedondeo(sImporteGravado);

            double pos = posY;
            gfx.DrawString(sTipo, fuente8Normal, XBrushes.Black, 20, pos, formatoLeft);
            gfx.DrawString(sClave, fuente8Normal, XBrushes.Black, 70, pos, formatoLeft);
            posY = fnEscribirMultilinea(sConcepto, 50, 130, posY, fuente8Normal, ref gfx);
            gfx.DrawString(sImporteGravado, fuente8Normal, XBrushes.Black, 400, pos, formatoLeft);
            gfx.DrawString(sImporteExcento, fuente8Normal, XBrushes.Black, 500, pos, formatoLeft);

            posY += 10;

        }

        public void fnDibujaPanelIncapacidad(ref XGraphics gfx, ref double posY, ref PdfPage pagina)
        {
            if ((limiteY - posY) >= 20)
            {
                fnEncabezadoPanelIncapacidad(ref gfx, ref posY);
            }
            else
            {
                gfx.DrawLine(pen, 12, posY, 580, posY);
                pagina = nomina.AddPage();
                gfx = XGraphics.FromPdfPage(pagina);
                paginasReporte.Enqueue(gfx);
                fnDibujarEncabezadoPiePagina(ref gfx);
                posY = 150;
                fnCrearPanelRedondeadoDeducciones(ref gfx, ref  posY);
                fnEncabezadoPanelIncapacidad(ref gfx, ref  posY);
            }


            XPathNavigator nav = gxComprobante.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Incapacidades/nomina:Incapacidad", nsmComprobante);

            while (iterator.MoveNext())
            {
                string sDiasIncapacidad, sTipoIncapacidad, sDescuento;
                sDiasIncapacidad = sTipoIncapacidad = sDescuento = "";

                try { sDiasIncapacidad = iterator.Current.SelectSingleNode("@DiasIncapacidad", nsmComprobante).ToString(); }
                catch { }
                try { sTipoIncapacidad = iterator.Current.SelectSingleNode("@TipoIncapacidad", nsmComprobante).ToString(); }
                catch { }
                try { sDescuento = iterator.Current.SelectSingleNode("@Descuento", nsmComprobante).ToString(); }
                catch { }

                if ((limiteY - posY) > 12)
                {

                    fnDibujaDatosPanelIncapacidad(ref gfx, ref posY, sDiasIncapacidad, sTipoIncapacidad, sDescuento);

                }
                else
                {
                    gfx.DrawLine(pen, 12, posY, 580, posY);
                    pagina = nomina.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    paginasReporte.Enqueue(gfx);
                    fnDibujarEncabezadoPiePagina(ref gfx);
                    posY = 150;
                    fnCrearPanelRedondeadoDeducciones(ref gfx, ref  posY);
                    fnEncabezadoPanelIncapacidad(ref gfx, ref posY);
                    fnDibujaDatosPanelIncapacidad(ref gfx, ref posY, sDiasIncapacidad, sTipoIncapacidad, sDescuento);
                }

            }



        }

        public void fnEncabezadoPanelIncapacidad(ref XGraphics gfx, ref double posY)
        {
            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, posY, 570, 12, 10, 10);
            gfx.DrawString("Días de Incapacidad", fuente8Negrita, XBrushes.Black, 20, posY, formatoLeft);
            gfx.DrawString("Tipo de Incapacidad", fuente8Negrita, XBrushes.Black, 250, posY, formatoLeft);
            gfx.DrawString("Descuento", fuente8Negrita, XBrushes.Black, 500, posY, formatoLeft);
            posY += 12;
        }

        public void fnDibujaDatosPanelIncapacidad(ref XGraphics gfx, ref double posY, string sDiasIncapacidad, string sTipoIncapacidad, string sDescuento)
        {
            sDescuento = TransformacionNomina.fnFormatoRedondeo(sDescuento);

            int nTipoIncapacidad = Convert.ToInt32(sTipoIncapacidad);

            try { sTipoIncapacidad = (string)tipoIncapacidad[nTipoIncapacidad]; }
            catch { }

            gfx.DrawString(sDiasIncapacidad, fuente8Normal, XBrushes.Black, 20, posY, formatoLeft);
            gfx.DrawString(sTipoIncapacidad, fuente8Normal, XBrushes.Black, 270, posY + 4, XStringFormats.Center);
            gfx.DrawString(sDescuento, fuente8Normal, XBrushes.Black, 500, posY, formatoLeft);
            posY += 10;
        }

        public void fnDibujaPanelSumaDeducciones(ref XGraphics gfx, ref double posY, ref PdfPage pagina)
        {
            if ((limiteY - posY) >= 20)
            {
                fnCrearPanelSumaDeducciones(ref gfx, ref posY);
            }
            else
            {
                gfx.DrawLine(pen, 12, posY, 580, posY);
                pagina = nomina.AddPage();
                gfx = XGraphics.FromPdfPage(pagina);
                paginasReporte.Enqueue(gfx);
                fnDibujarEncabezadoPiePagina(ref gfx);
                posY = 150;
                fnCrearPanelRedondeadoDeducciones(ref gfx, ref  posY);
                fnCrearPanelSumaDeducciones(ref gfx, ref  posY);
            }

        }

        public void fnCrearPanelSumaDeducciones(ref XGraphics gfx, ref double posY)
        {
            string sSumaDeducciones = "0";
            string sDescuento = "0";

            XPathNavigator nav = gxComprobante.CreateNavigator();

            try { sDescuento = nav.SelectSingleNode("/cfdi:Comprobante/@descuento", nsmComprobante).Value; }
            catch { }
            sSumaDeducciones = Convert.ToString(Convert.ToDouble(sDescuento) + Convert.ToDouble(this.sISR));

            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, posY, 570, 12, 10, 10);
            gfx.DrawString("Suma Deducciones", fuente8Negrita, XBrushes.Black, 400, posY, formatoLeft);
            gfx.DrawString(sSumaDeducciones, fuente8Negrita, XBrushes.Black, 500, posY, formatoLeft);
            posY += 12;
        }

        public void fnDibujarPanelTotales(ref XGraphics gfx)
        {
            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, 590, 570, 100, 10, 10); //Panel redondeado enorme
            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, 590, 100, 100, 10, 10); //Panel para el codigo bidimensional
            gfx.DrawRoundedRectangle(pen, XBrushes.White, 110, 590, 300, 100, 10, 10); //Panel para total con letra 

            gfx.DrawString("Total con letra:", fuente12Negrita, XBrushes.Black, new Point(115, 590), formatoLeft);
            gfx.DrawString("SubTotal", fuente8Negrita, XBrushes.Black, new Point(450, 590), formatoLeft);
            gfx.DrawString("Descuentos", fuente8Negrita, XBrushes.Black, new Point(450, 610), formatoLeft);
            gfx.DrawString("ISR", fuente8Negrita, XBrushes.Black, new Point(450, 620), formatoLeft);

            gfx.DrawLine(pen, 415, 675, 570, 675);
            gfx.DrawString("TOTAL", fuente12Negrita, XBrushes.Black, new Point(415, 675), formatoLeft);


            XImage codigo = XImage.FromGdiPlusImage(GenerarCodigoBidimensional());
            gfx.DrawImage(codigo, 15, 595, 90, 90);

            fnDatosPanelTotales(nsmComprobante, navEncabezado, ref gfx);

        }

        public void fnDatosPanelTotales(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, ref XGraphics gfx)
        {
            string sTotalLetra, sTotal, sSubTotal, sDescuentos, sISR;
            sTotalLetra = sTotal = sSubTotal = sDescuentos = sISR = "";

            try
            {
                sTotal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value;
                sTotal = TransformacionNomina.fnFormatoRedondeo(sTotal);
            }
            catch { }

            try
            {
                sSubTotal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsmComprobante).Value;
                sSubTotal = TransformacionNomina.fnFormatoRedondeo(sSubTotal);
            }
            catch { }
            try
            {
                sDescuentos = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@descuento", nsmComprobante).Value;
                sDescuentos = TransformacionNomina.fnFormatoRedondeo(sDescuentos);
            }
            catch { }
            try
            {

                sISR = TransformacionNomina.fnFormatoRedondeo(this.sISR);
            }
            catch { }

            gfx.DrawString(fnTextoImporte(sTotal, "MXN"), fuente8Normal, XBrushes.Black, new Point(115, 610), formatoLeft);
            gfx.DrawString(sSubTotal, fuente8Normal, XBrushes.Black, new Point(570, 590), formatoRight);
            gfx.DrawString(sDescuentos, fuente8Normal, XBrushes.Black, new Point(570, 610), formatoRight);
            gfx.DrawString(sISR, fuente8Normal, XBrushes.Black, new Point(570, 620), formatoRight);
            gfx.DrawString(sTotal, fuente8Normal, XBrushes.Black, new Point(570, 675), formatoRight);


        }

        public void fnDibujarPanelSellos(ref XGraphics gfx)
        {
            gfx.DrawRoundedRectangle(pen, XBrushes.White, 12, 690, 570, 100, 10, 10); //Panel redondeado enorme
            gfx.DrawString("Sello digital del Emisor:", fuente8Normal, XBrushes.Black, new Point(15, 695), formatoLeft);
            gfx.DrawString("Sello digital del SAT:", fuente8Normal, XBrushes.Black, new Point(15, 720), formatoLeft);
            gfx.DrawString("Cadena Original del complemento de certificación digital del SAT:", fuente8Normal, XBrushes.Black, new Point(15, 750), formatoLeft);

            fnDatosPanelSellos(nsmComprobante, navEncabezado, ref gfx);
        }

        public void fnDatosPanelSellos(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, ref XGraphics gfx)
        {
            string sSelloEmisor, sSelloSat, sCadenaOriginal;
            sSelloEmisor = sSelloSat = sCadenaOriginal =    "";

            try { sSelloEmisor = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value; }
            catch { }
            try { sSelloSat = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value; }
            catch { }
            try { sCadenaOriginal = "|" + TransformacionNomina.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_TFD_1_0") + "||"; }
            catch { }

            puntoY = 702;
            puntoY = fnEscribirMultilinea(sSelloEmisor, 130, 15, puntoY, fuente6Normal, ref gfx);
            puntoY = 730;
            puntoY = fnEscribirMultilinea(sSelloSat, 130, 15, puntoY, fuente6Normal, ref gfx);
            puntoY = 765;
            puntoY = fnEscribirMultilinea(sCadenaOriginal, 120, 15, puntoY, fuente6Normal, ref gfx);
        }

     

        public string fnBreakString(string sLinea, int nCaracteres)
        {
            sLinea = sLinea.Replace("\n", string.Empty);
            sLinea = sLinea.Replace("\r", string.Empty);
            sLinea = Regex.Replace(sLinea, ".{" + nCaracteres + "}", "$0\n");
            return sLinea;
        }

        /// <summary>
        /// Escribie multilinea, el metodo hace la separacion entre caracteres y calcula la nueva posicion del renglon automaticamente
        /// </summary>
        /// <param name="sLinea">Cadena a escribir</param>
        /// <param name="nCaracteres">Numero de caracteres permitidos por renglon</param>
        /// <param name="posX">Posicion inicial  en X donde se comenzara a dibujar la cadena</param>
        /// <param name="posY">Posicion inicial en Y donde se comenzara a dibujar la cadena</param>
        /// <param name="xFuente">Tipo de fuente</param>
        /// <param name="gfx">Objeto XGraphics</param>
        /// <returns></returns>
        public double fnEscribirMultilinea(string sLinea, int nCaracteres, double posX, double posY, XFont xFuente, ref XGraphics gfx)
        {
            sLinea = sLinea.Replace("\n", "");
            sLinea = sLinea.Replace("\r", "");

            int nCaracteresCadena = sLinea.Length;
            double posXTemp = posX;

            double xColumna = posX;
            double yRenglon = posY;

            XSize xSeparacion = new XSize();
            double separacionLetra = 0;
            double separacionRenglon = 0;
            separacionRenglon = gfx.MeasureString(sLinea, xFuente).Height;

            if (nCaracteresCadena > nCaracteres)
            {

                for (int i = 0; i < nCaracteresCadena; i++)
                {
                    char caracter = sLinea[i];

                   if (caracter.ToString() != " ")
                    {
                        xSeparacion = gfx.MeasureString(caracter.ToString(), xFuente);
                      separacionLetra = xSeparacion.Width;
                    }


                    if (((i == nCaracteres) || i % nCaracteres == 0) & i != 0)
                    {
                        yRenglon += separacionRenglon;
                        gfx.DrawString(caracter.ToString(), xFuente, XBrushes.Black, posX, yRenglon, XStringFormats.TopLeft);
                        xColumna = posX + separacionLetra;

                    }
                    else if (i == 0)
                    {
                        gfx.DrawString(caracter.ToString(), xFuente, XBrushes.Black, xColumna, yRenglon, XStringFormats.TopLeft);
                        xColumna += separacionLetra;
                    }
                    else if (i < nCaracteres)
                    {
                        gfx.DrawString(caracter.ToString(), xFuente, XBrushes.Black, xColumna, yRenglon, XStringFormats.TopLeft);
                        xColumna += separacionLetra;
                    }
                    else
                    {
                        gfx.DrawString(caracter.ToString(), xFuente, XBrushes.Black, xColumna, yRenglon, XStringFormats.TopLeft);
                        xColumna += separacionLetra;
                    }
                }


            }
            else { gfx.DrawString(sLinea, xFuente, XBrushes.Black, xColumna, yRenglon, XStringFormats.TopLeft); }


            return yRenglon;

        }

        private Image GenerarCodigoBidimensional()
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
            ce.Encode(sCadenaCodigo, System.Text.Encoding.UTF8).Save(ms, ImageFormat.Png);

            Image codigoBidimensional = Image.FromStream(ms);

            return codigoBidimensional;


        }

        private string fnTextoImporte(string psValor, string psMoneda)
        {
            CultureInfo languaje;
            NumaletNomina parser = new NumaletNomina();
            parser.LetraCapital = true;

            switch (psMoneda)
            {
                case "MXN":
                    parser.TipoMoneda = NumaletNomina.Moneda.Peso;
                    break;
                case "USD":
                    parser.TipoMoneda = NumaletNomina.Moneda.Dolar;
                    break;
                case "XEU":
                    parser.TipoMoneda = NumaletNomina.Moneda.Euro;
                    break;
            }

            languaje = new CultureInfo("es-Mx");

            return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
        }
    }

    public class TransformacionNomina
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
            string sCadenaOriginal = "";
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
    public class ImpuestoNomina
    {
        public string Nombre { get; set; }
        public string Tasa { get; set; }
        public string Importe { get; set; }

        //Esta propiedad retorna el texto del renglon a mostrar en el PDF
        public string TextoImpuesto
        {
            get
            {
                return Nombre + " " + TransformacionNomina.fnFormatoPorcentaje(Tasa) + " " + TransformacionNomina.fnFormatoCurrency(Importe);
            }
        }

        /// <summary>
        /// Crea una nueva instancia de Impuesto
        /// </summary>
        /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
        /// <param name="nsmComprobante"></param>
        public ImpuestoNomina(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
    public class ImpuestoCompNomina
    {
        public string Nombre { get; set; }
        public string Tasa { get; set; }
        public string Importe { get; set; }

        //Esta propiedad retorna el texto del renglon a mostrar en el PDF
        public string TextoImpuesto
        {
            get
            {
                return Nombre + " " + TransformacionNomina.fnFormatoPorcentaje(Tasa) + " " + TransformacionNomina.fnFormatoCurrency(Importe);
            }
        }
        /// <summary>
        /// Crea una nueva instancia de Impuesto
        /// </summary>
        /// <param name="navPie">Navegador XML con los valores de los impuestos</param>
        /// <param name="nsmComprobante"></param>
        public ImpuestoCompNomina(XPathNavigator navImpuesto, XmlNamespaceManager nsmComprobante)
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
    public class DetalleNomina
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

        private List<ComprobanteImpuestosRetencionTNomina> _retencionesT;
        public List<ComprobanteImpuestosRetencionTNomina> RetencionesT
        {
            get { return _retencionesT; }
            set { _retencionesT = value; }
        }

        private List<ComprobanteImpuestosTrasladoTNomina> _trasladosT;
        public List<ComprobanteImpuestosTrasladoTNomina> TrasladosT
        {
            get { return _trasladosT; }
            set { _trasladosT = value; }
        }

        private t_UbicacionFiscalTNomina _ubicacionFiscalT;
        public t_UbicacionFiscalTNomina UbicacionFiscalT
        {
            get { return _ubicacionFiscalT; }
            set { _ubicacionFiscalT = value; }
        }

        private t_InformacionAduaneraTNomina _informacionAduaneraT;
        public t_InformacionAduaneraTNomina InformacionAduaneraT
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
        public DetalleNomina(XPathNavigator navDetalle, XmlNamespaceManager nsmComprobante)
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

    public partial class ComprobanteImpuestosRetencionTNomina
    {
        private ComprobanteImpuestosRetencionImpuestoTNomina impuestoField;
        private decimal importeField;

        public ComprobanteImpuestosRetencionImpuestoTNomina impuesto
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

    public enum ComprobanteImpuestosRetencionImpuestoTNomina
    {
        ISR, IVA,
    }

    public partial class ComprobanteImpuestosTrasladoTNomina
    {
        private ComprobanteImpuestosTrasladoImpuestoTNomina impuestoField;
        private decimal tasaField;
        private decimal importeField;

        public ComprobanteImpuestosTrasladoImpuestoTNomina impuesto
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

    public enum ComprobanteImpuestosTrasladoImpuestoTNomina
    {
        IVA, IEPS,
    }

    public partial class t_UbicacionFiscalTNomina
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

    public partial class t_InformacionAduaneraTNomina
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

    public sealed class NumaletNomina
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
        public NumaletNomina()
        {
            MascaraSalidaDecimal = MascaraSalidaDecimalDefault;
            SeparadorDecimalSalida = SeparadorDecimalSalidaDefault;
            LetraCapital = LetraCapitalDefault;
            ConvertirDecimales = _convertirDecimales;
        }

        public NumaletNomina(Boolean ConvertirDecimales, String MascaraSalidaDecimal, String SeparadorDecimalSalida, Boolean LetraCapital)
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
            return NumaletNomina.ToString(Numero, CultureInfo.CurrentCulture);
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

﻿using System;
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

namespace clsPlantillaNomina
{
    class clsPlantillaNomina
    {
        private XmlDocument gxComprobante;
        public Report PDF;

        private double puntoY = 288; // Punto donde se comenzara a dibujar el panel de percepciones /Deducciones
        private double horasExtraY;  // Punto donde se comenzara a dibujar el panel de horas extra;
        private double incapacidadY; // Punto donde se comenzara a dibujar el panel de deducciones;
        private double limiteY = 650; // Limite donde se dibujaran los paneles 

        public List<Page> paginasReporte;  //Lista donde se almacenaran las paginas del documento

        public string TipoDocumento { get; set; }
        public string sColor;
        private FontDef fuenteTitulo;
        private FontProp fPropTitulo;
        private const double tamFuenteTitulo = 6;

        private FontDef fuenteNormal;
        private FontProp fPropNormal;
        private const double tamFuenteNormal = 5;

        private FontProp fPropChica;
        private const double tamFuenteChica = 4;

        private FontProp fPropBlanca;
        private FontProp fPropRoja;
        private FontProp fPropNegrita;

        private PenProp penGruesa;
        private const double tamPlumaGruesa = 3;

        private PenProp penMediana;
        private const double tamPlumaMediana = 2;

        private PenProp penDelgada;
        private const double tamPlumaDelgada = 0.5;

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
            xmlDOC.Load(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\clsPlantillaNomina\muchaPercepcion.xml");
            clsPlantillaNomina plantillaUnion = new clsPlantillaNomina(xmlDOC);
            plantillaUnion.fnGenerarPdf("Blue");
        }

        public clsPlantillaNomina(XmlDocument pxComprobante)
        {
            gxComprobante = pxComprobante;
            XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            //gxComprobante.InsertBefore(xDec, gxComprobante.DocumentElement);
        }

        public void fnGenerarPdf(string sColor)
        {
            this.sColor = sColor;
            PdfFormatter formato = new PdfFormatter();
            PDF = new Report(formato);
            PDF.sAuthor = "CORPUS Facturacion";
            PDF.sTitle = "CFDI";

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

            penGruesa = new PenProp(PDF, tamPlumaGruesa, Color.FromName(this.sColor));
            penMediana = new PenProp(PDF, tamPlumaMediana, Color.FromName(this.sColor));
            penDelgada = new PenProp(PDF, tamPlumaDelgada, Color.FromName(this.sColor));

            paginasReporte = new List<Page>();

            //Tamaño carta
            Page pagina = new Page(PDF);
            pagina.rWidthMM = anchoPagina;
            pagina.rHeightMM = altoPagina;

            paginasReporte.Add(pagina);

            //Se crea el encabezado en la primera pagina
            fnCrearEncabezado(ref pagina, ref PDF);

            //Se crea el panel de contenido unicamente en la primera pagina
            StaticContainer panelContenido = fnCrearPanelContenido();
            pagina.Add(margenPagina, margenPagina, panelContenido);

            //Se crea el panel de la leyenda del pie en la primera pagina
            fnCrearLeyendaPie(ref pagina, ref PDF);

            //Dibujando los paneles de el lado izquiero de la hoja
            fnDibujarPanelPercepciones(ref this.puntoY, ref  pagina, PDF);
            this.horasExtraY = this.puntoY + 10;
            fnDibujaPanelHorasExtra(ref this.horasExtraY, ref pagina, PDF);
            fnDibujaPanelSumaPercepciones(ref this.horasExtraY, ref pagina, PDF);

            //Reinciamos el valor de la posicion para dibujar el panel de Deducciones
            this.puntoY = 288;

            //Dibujamos los paneles de el lado derecho de la hoja
            pagina = fnDibujaPanelDeducciones(ref this.puntoY, ref paginasReporte, PDF);
            this.incapacidadY = this.puntoY + 10;
            pagina = fnDibujaPanelDiasIncapacidad(ref this.incapacidadY, ref pagina, PDF);
            pagina = fnDibujaPanelSumaDeducciones(ref this.incapacidadY, ref pagina, PDF);

            if (paginasReporte.Count > pagina.iPageNo)
            {
                pagina = paginasReporte.Last();
            }
          
            //Dibujando el panel de sellos en la ultima pagina 
            fnCrearPanelSellos(ref pagina);

            RT.ViewPDF(PDF);

        }

        private void fnDibujarLogo(ref Page pagina)
        {
            MemoryStream ms1 = new MemoryStream();
            ms1 = fnImagenCliente(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\clsPlantillaNomina\logo_pax.jpg");
            RepImage logo = new RepImage(ms1, 140, 75);
            pagina.Add(margenPagina, margenPagina + 70, logo);
        }

        private void fnCrearEncabezado(ref Page pagina, ref Report PDF)
        {
            StaticContainer panelEncabezado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            fnCrearPanelEmisor(panelEncabezado);
            fnCrearPanelReciboNomina(panelEncabezado);
            fnCrearPanelExpedidoEn(panelEncabezado);
            fnDibujarLogo(ref pagina);
            pagina.Add(margenPagina, margenPagina, panelEncabezado);
        }

        private void fnCrearLeyendaPie(ref Page pagina, ref Report PDF)
        {
            MemoryStream ms3 = new MemoryStream();
            ms3 = fnImagenCliente(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\clsPlantillaNomina\logo_pax.jpg");

            RepImage image3 = new RepImage(ms3, 30, 10);
            pagina.Add(margenPagina + 10, RT.rPointFromMM(altoPagina) - margenPagina + 10, image3);

            pagina.Add(margenPagina + 170, RT.rPointFromMM(altoPagina) - margenPagina + 8, new RepString(fPropChica, leyendaPDF));
            pagina.Add(margenPagina + 50, RT.rPointFromMM(altoPagina) - margenPagina + 8, new RepString(fPropChica, "http://www.paxfacturacion.com/"));
        }

        private StaticContainer fnCrearPanelContenido()
        {
            StaticContainer panelContenido = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            fnCrearPanelEmisor(panelContenido);
            fnCrearPanelReciboNomina(panelContenido);
            fnCrearPanelExpedidoEn(panelContenido);
            panelContenido.Add(0, 130, new RepLine(penMediana, 580, 0)); //Linea azul separadora
            fnCrearPanelEmpleado(panelContenido);
            fnCrearPanelInformacionLaboral(panelContenido);

            panelContenido.Add(0, 130, new RepLine(penDelgada, 0, -147)); //Linea vertical izquierda
            panelContenido.Add(580, 130, new RepLine(penDelgada, 0, -147)); //Linea vertical derecha

            return panelContenido;
        }

        private void fnCrearPanelEmisor(StaticContainer panel)
        {

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            double Renglon = 0;
            fnDatosPanelEmisor(nsmComprobante, navEncabezado, panel, Renglon);

        }

        private void fnDatosPanelEmisor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sEmisor, sRFC, sCodigoPostal, sPais, sEstado, sMunicipio, sLocalidad, sColonia, sNoInterior, sNoExterior, sCalle, sRegimenFiscal, sRegistroPatronal;

            sEmisor = sRFC = sCodigoPostal = sPais = sEstado = sMunicipio = sLocalidad = sColonia = sNoInterior = sNoExterior = sCalle = sRegimenFiscal = sRegistroPatronal = string.Empty;

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

            string sDireccion1 = sCalle + " No." + " " + sNoInterior + " " + " Colonia " + sColonia;
            string sDireccion2 = sMunicipio + ", " + sEstado + ". " + sPais + " C.P. " + sCodigoPostal;

            fPropChica.bBold = true;
            panel.AddAligned(150, RepObj.rAlignLeft, 0, RepObj.rAlignBottom, new RepString(fPropChica, sEmisor));
            fPropChica.bBold = false;
            panel.AddAligned(150, RepObj.rAlignLeft, 12, RepObj.rAlignBottom, new RepString(fPropChica, sDireccion1));
            panel.AddAligned(150, RepObj.rAlignLeft, 24, RepObj.rAlignBottom, new RepString(fPropChica, sDireccion2));
            panel.AddAligned(150, RepObj.rAlignLeft, 36, RepObj.rAlignBottom, new RepString(fPropChica, "RFC: " + sRFC));
            panel.AddAligned(150, RepObj.rAlignLeft, 48, RepObj.rAlignBottom, new RepString(fPropChica, "Registro Patronal: " + sRegistroPatronal));
            panel.AddAligned(150, RepObj.rAlignLeft, 60, RepObj.rAlignBottom, new RepString(fPropChica, "Regimen Fiscal: " + sRegimenFiscal));

        }

        private void fnCrearPanelReciboNomina(StaticContainer panel)
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            double Renglon = 0;
            fnDatosPanelReciboNomina(nsmComprobante, navEncabezado, panel, Renglon);

            //fPropTitulo.bBold = true;
            panel.AddAligned(470, RepObj.rAlignCenter, 0, RepObj.rAlignBottom, new RepString(fPropTitulo, "RECIBO DE NÓMINA"));
            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 12, RepObj.rAlignBottom, new RepString(fPropTitulo, "Folio Fiscal:"));
            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 36, RepObj.rAlignBottom, new RepString(fPropTitulo, "No. de Serie del Certificado del Emisor:"));
            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 60, RepObj.rAlignBottom, new RepString(fPropTitulo, "No. de Serie del Certificado del SAT:"));
            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 84, RepObj.rAlignBottom, new RepString(fPropTitulo, "Fecha y hora de certificación:"));
            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 106, RepObj.rAlignBottom, new RepString(fPropTitulo, "Lugar, fecha y hora de emisión:"));

        }

        private void fnDatosPanelReciboNomina(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sFolioFiscal, sCertificadoEmisor, sCertificadoSAT, sFechaCertificacion, sFechaEmision, sLugarEmision;
            sFolioFiscal = sCertificadoEmisor = sCertificadoSAT = sFechaCertificacion = sFechaEmision = sLugarEmision = string.Empty;

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

            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 24, RepObj.rAlignBottom, new RepString(fPropChica, sFolioFiscal));
            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 48, RepObj.rAlignBottom, new RepString(fPropChica, sCertificadoEmisor));
            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 72, RepObj.rAlignBottom, new RepString(fPropChica, sCertificadoSAT));
            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 96, RepObj.rAlignBottom, new RepString(fPropChica, sFechaCertificacion));
            panel.AddAligned(590 - margenPagina, RepObj.rAlignRight, 118, RepObj.rAlignBottom, new RepString(fPropChica, sLugarEmision + " " + sFechaEmision));
        }

        private void fnCrearPanelExpedidoEn(StaticContainer panel)
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            double Renglon = 0;
            fnDatosPanelExpedidoEn(nsmComprobante, navEncabezado, panel, Renglon);

            panel.AddAligned(margenPagina, RepObj.rAlignLeft, 90, RepObj.rAlignBottom, new RepString(fPropTitulo, "Expedido en:"));

        }

        private void fnDatosPanelExpedidoEn(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sCalle, sNoCalle, sNoInterior, sColonia, sLocalidad, sEstado, sPais, sCodigoPostal;

            sCalle = sNoCalle = sNoInterior = sColonia = sLocalidad = sEstado = sPais = sCodigoPostal = string.Empty;

            try { sCodigoPostal = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@codigoPostal", nsmComprobante).Value; }
            catch { }
            try { sPais = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@pais", nsmComprobante).Value; }
            catch { }
            try { sEstado = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@estado", nsmComprobante).Value; }
            catch { }
            try { sLocalidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@municipio", nsmComprobante).Value; }
            catch { }
            try { sLocalidad = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@localidad", nsmComprobante).Value; }
            catch { }
            try { sColonia = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@colonia", nsmComprobante).Value; }
            catch { }
            try { sNoInterior = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@noInterior", nsmComprobante).Value; }
            catch { }
            try { sCalle = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/cfdi:DomicilioFiscal/@calle", nsmComprobante).Value; }
            catch { }

            string sDireccion1 = sCalle + " No." + " " + sNoInterior + " " + " Colonia " + sColonia;
            string sDireccion2 = sLocalidad + ", " + sEstado + ". " + sPais + " C.P. " + sCodigoPostal;

            panel.AddAligned(margenPagina, RepObj.rAlignLeft, 102, RepObj.rAlignBottom, new RepString(fPropChica, sDireccion1));
            panel.AddAligned(margenPagina, RepObj.rAlignLeft, 114, RepObj.rAlignBottom, new RepString(fPropChica, sDireccion2));
        }

        private void fnCrearPanelEmpleado(StaticContainer panel)
        {
            double columna2 = 230;
            double columna3 = 420;

            fPropNormal.bBold = true;
            panel.Add(margenPagina, 142, new RepString(fPropTitulo, "Datos del Empleado"));
            panel.Add(margenPagina, 152, new RepString(fPropNormal, "Nombre:"));
            panel.Add(margenPagina, 162, new RepString(fPropNormal, "RFC:"));
            panel.Add(margenPagina, 172, new RepString(fPropNormal, "Regimen:"));
            panel.Add(margenPagina, 182, new RepString(fPropNormal, "Riesgo de Puesto:"));
            panel.Add(columna2, 152, new RepString(fPropNormal, "No.Empleado:"));
            panel.Add(columna2, 162, new RepString(fPropNormal, "CURP:"));
            panel.Add(columna2, 172, new RepString(fPropNormal, "Puesto:"));
            panel.Add(columna3, 162, new RepString(fPropNormal, "NNS:"));
            panel.Add(columna3, 172, new RepString(fPropNormal, "Departamento:"));
            fPropNormal.bBold = false;
            //Lineas Divisorias
            panel.Add(0, 145, new RepLine(penDelgada, 580, 0));
            panel.Add(0, 185, new RepLine(penDelgada, 580, 0));

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            double Renglon = 0;
            fnDatosPanelEmpleado(nsmComprobante, navEncabezado, panel, Renglon);

        }

        private void fnDatosPanelEmpleado(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            double columna2 = 230;
            double columna3 = 450;
            double separacion = 80;
            double separacion2 = 40;

            string sNombre, sRFC, sRegimen, sRiesgoPuesto, sNoEmpleado, sCURP, sPuesto, sNNS, sDepartamento;
            sNombre = sRFC = sRegimen = sRiesgoPuesto = sNoEmpleado = sCURP = sPuesto = sNNS = sDepartamento = string.Empty;

            try { sNombre = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@nombre", nsmComprobante).Value; }
            catch { }
            try { sRFC = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Receptor/@rfc", nsmComprobante).Value; }
            catch { }
            try { sRegimen = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Emisor/@nombre", nsmComprobante).Value; } //Donde viene en el xml!?
            catch { }
            try { sRiesgoPuesto = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@RiesgoPuesto", nsmComprobante).Value; }
            catch { }
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

            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 152, RepObj.rAlignBottom, new RepString(fPropNormal, sNombre));
            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 162, RepObj.rAlignBottom, new RepString(fPropNormal, sRFC));
            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 172, RepObj.rAlignBottom, new RepString(fPropNormal, sRegimen));
            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 182, RepObj.rAlignBottom, new RepString(fPropNormal, sRiesgoPuesto));
            panel.AddAligned(columna2 + separacion, RepObj.rAlignLeft, 152, RepObj.rAlignBottom, new RepString(fPropNormal, sNoEmpleado));
            panel.AddAligned(columna2 + separacion, RepObj.rAlignLeft, 162, RepObj.rAlignBottom, new RepString(fPropNormal, sCURP));
            panel.AddAligned(columna2 + separacion, RepObj.rAlignLeft, 172, RepObj.rAlignBottom, new RepString(fPropNormal, sPuesto));
            panel.AddAligned(columna3 + separacion2, RepObj.rAlignLeft, 162, RepObj.rAlignBottom, new RepString(fPropNormal, sNNS));
            panel.AddAligned(columna3 + separacion2, RepObj.rAlignLeft, 172, RepObj.rAlignBottom, new RepString(fPropNormal, sDepartamento));
        }

        private void fnCrearPanelInformacionLaboral(StaticContainer panel)
        {
            double columna2 = 230;
            double columna3 = 420;

            fPropNormal.bBold = true;
            panel.Add(margenPagina, 195, new RepString(fPropTitulo, "Información Laboral"));
            panel.Add(margenPagina, 205, new RepString(fPropNormal, "Contrato:"));
            panel.Add(margenPagina, 215, new RepString(fPropNormal, "Dias Pagados:"));
            panel.Add(margenPagina, 225, new RepString(fPropNormal, "Salario Base:"));
            panel.Add(columna2, 205, new RepString(fPropNormal, "Folio:"));
            panel.Add(columna2, 215, new RepString(fPropNormal, "Fecha de Pago:"));
            panel.Add(columna2, 225, new RepString(fPropNormal, "Jornada:"));
            panel.Add(columna3, 205, new RepString(fPropNormal, "Periodo:"));
            panel.Add(columna3, 215, new RepString(fPropNormal, "Antigüedad:"));
            panel.Add(columna3, 225, new RepString(fPropNormal, "Salario Diario:"));
            panel.Add(margenPagina, 235, new RepString(fPropNormal, "Delegacion/municipio:"));
            panel.Add(margenPagina, 245, new RepString(fPropNormal, "Forma de Pago:"));
            panel.Add(margenPagina, 255, new RepString(fPropNormal, "Banco:"));
            panel.Add(columna2, 245, new RepString(fPropNormal, "Metodo de Pago:"));
            panel.Add(columna2, 255, new RepString(fPropNormal, "Clabe:"));
            fPropNormal.bBold = false;
            //Lineas Divisorias
            panel.Add(0, 197, new RepLine(penDelgada, 580, 0));
            panel.Add(0, 228, new RepLine(penDelgada, 580, 0));
            panel.Add(0, 258, new RepLine(penDelgada, 580, 0));

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            double Renglon = 0;
            fnDatosPanelInformacionLaboral(nsmComprobante, navEncabezado, panel, Renglon);
        }

        private void fnDatosPanelInformacionLaboral(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            double columna2 = 230;
            double columna3 = 420;
            double separacion = 80;
            double separacion2 = 40;


            string sContrato, sDiasPagados, sSalarioBase, sFolio, sFechaPago, sJornada, sPeriodo, sAntiguedad, sSalarioDiario, sDelegacion, sFormaPago, sBanco, sMetodoPago, sCLABE;

            sContrato = sDiasPagados = sSalarioBase = sFolio = sFechaPago = sJornada = sPeriodo = sAntiguedad = sSalarioDiario = sDelegacion = sFormaPago = sMetodoPago = sCLABE = sBanco = string.Empty;

            try { sContrato = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@TipoContrato", nsmComprobante).Value; }
            catch { }
            try { sDiasPagados = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@NumDiasPagados", nsmComprobante).Value; }
            catch { }
            try { sSalarioBase = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@SalarioBaseCotApor", nsmComprobante).Value; }
            catch { }
            try { sFolio = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@Departamento", nsmComprobante).Value; }
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
            try { sDelegacion = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@SalarioDiarioIntegrado", nsmComprobante).Value; }
            catch { }
            try { sFormaPago = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@formaDePago", nsmComprobante).Value; }
            catch { }
            try { sMetodoPago = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@metodoDePago", nsmComprobante).Value; }
            catch { }
            try { sCLABE = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/@SalarioDiarioIntegrado", nsmComprobante).Value; }
            catch { }

            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 205, RepObj.rAlignBottom, new RepString(fPropNormal, sContrato));
            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 215, RepObj.rAlignBottom, new RepString(fPropNormal, sDiasPagados));
            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 225, RepObj.rAlignBottom, new RepString(fPropNormal, sSalarioBase));
            panel.AddAligned(columna2 + separacion, RepObj.rAlignLeft, 205, RepObj.rAlignBottom, new RepString(fPropNormal, sFolio));
            panel.AddAligned(columna2 + separacion, RepObj.rAlignLeft, 215, RepObj.rAlignBottom, new RepString(fPropNormal, sFechaPago));
            panel.AddAligned(columna2 + separacion, RepObj.rAlignLeft, 225, RepObj.rAlignBottom, new RepString(fPropNormal, sJornada));
            panel.AddAligned(columna3 + separacion, RepObj.rAlignLeft, 205, RepObj.rAlignBottom, new RepString(fPropNormal, sPeriodo));
            panel.AddAligned(columna3 + separacion, RepObj.rAlignLeft, 215, RepObj.rAlignBottom, new RepString(fPropNormal, sAntiguedad));
            panel.AddAligned(columna3 + separacion, RepObj.rAlignLeft, 225, RepObj.rAlignBottom, new RepString(fPropNormal, sSalarioDiario));
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 235, RepObj.rAlignBottom, new RepString(fPropNormal, sDelegacion));
            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 245, RepObj.rAlignBottom, new RepString(fPropNormal, sFormaPago));
            panel.AddAligned(margenPagina + separacion, RepObj.rAlignLeft, 255, RepObj.rAlignBottom, new RepString(fPropNormal, sBanco));
            panel.AddAligned(columna2 + separacion, RepObj.rAlignLeft, 245, RepObj.rAlignBottom, new RepString(fPropNormal, sMetodoPago));
            panel.AddAligned(columna2 + separacion, RepObj.rAlignLeft, 255, RepObj.rAlignBottom, new RepString(fPropNormal, sCLABE));
        }

        #region panelesIzquierdos

        private void fnDibujarPanelPercepciones(ref double posY, ref Page pagina, Report reporte)
        {

            pagina.Add(margenPagina, posY, fnDibujarEncabezadoPanelPercepciones());
            posY += 17;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");

            XPathNavigator nav = gxComprobante.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Percepciones/nomina:Percepcion", nsmComprobante);
            while (iterator.MoveNext())
            {
                string sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento;
                sTipo = sClave = sConcepto = sImporteGravado = sImporteExcento = string.Empty;

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

                sImporteGravado = TransformacionNomina.fnFormatoRedondeo(sImporteGravado);
                sImporteExcento = TransformacionNomina.fnFormatoRedondeo(sImporteExcento);

                double renglon = 0;
                double temporal = 0;
                renglon = fnContarMultilinea(pagina, sConcepto, fPropChica, 2, posY, 30, true);

                if (renglon > 1)
                {
                    temporal = posY + (renglon * (renglon - 1));
                }
                else
                {
                    temporal = posY;
                }



                if ((limiteY - temporal) > 12)
                {
                    fnDibujaDatosPanelPercepciones(ref posY, ref pagina, sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento);
                   // posY += 10;
                }
                else
                {
                    pagina.Add(margenPagina, posY, new RepLine(penDelgada, 290, 0)); //Creamos ultima linea azul horizontal

                    pagina = new Page(PDF);
                    paginasReporte.Add(pagina); //Agregamos la pagina creada a la lista
                    fnCrearEncabezado(ref pagina, ref PDF);
                    fnCrearLeyendaPie(ref pagina, ref PDF);
                    pagina.rWidthMM = anchoPagina;
                    pagina.rHeightMM = altoPagina;
                    posY = 170;
                    pagina.Add(margenPagina, posY, fnDibujarEncabezadoPanelPercepciones());
                    posY = posY + 17;
                    fnDibujaDatosPanelPercepciones(ref posY, ref pagina, sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento);
                   // posY = posY + 10;
                }
            }

            pagina.Add(margenPagina, posY, new RepLine(penDelgada, 290, 0)); //Linea azul separadora

        }

        private StaticContainer fnDibujarEncabezadoPanelPercepciones()
        {
            StaticContainer encabezadoPercepciones = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            double columna2 = 60;
            double columna3 = 110;
            double columna4 = 190;
            double columna5 = 260;

            double pfPosY = 0;
            double pfAncho = 290;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(encabezadoPercepciones, 0, pfPosY - 10, pfAncho, altoBarra, grosorPen, 1, true, Color.FromName(sColor));
            double puntoMedio = 0 - fPropBlanca.rSize;
            double posColumna1 = encabezadoPercepciones.rWidth * .1;
            encabezadoPercepciones.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "PERCEPCIONES"));

            fPropNormal.bBold = true;
            encabezadoPercepciones.AddAligned(margenPagina, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Tipo"));
            encabezadoPercepciones.AddAligned(columna2, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Clave"));
            encabezadoPercepciones.AddAligned(columna3, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Concepto"));
            encabezadoPercepciones.AddAligned(columna4, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Importe Gravado"));
            encabezadoPercepciones.AddAligned(columna5, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Importe Excento"));
            fPropNormal.bBold = false;

            encabezadoPercepciones.Add(0, 11, new RepLine(penDelgada, 290, 0));

            encabezadoPercepciones.Add(0, 0, new RepLine(penDelgada, 0, -20));
            encabezadoPercepciones.Add(pfAncho, 0, new RepLine(penDelgada, 0, -20));

            return encabezadoPercepciones;
        }

        private void fnDibujaDatosPanelPercepciones(ref double posY, ref Page pagina, string sTipo, string sClave, string sConcepto, string sImporteGravado, string sImporteExcento)
        {

            double columna2 = 85;
            double columna3 = 110;
            double columna4 = 235.5;
            double columna5 = 305.5;

            double leftPadding = 0.02;
            double sep = 2.5;
            double posRazon = fPropChica.rSize + sep;
            double i = 0;

            pagina.AddAligned(45, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sTipo));
            pagina.AddAligned(columna2, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sClave));
            i = fnAgregarMultilinea(pagina, sConcepto, fPropChica, leftPadding + columna3, posY, 30, true);
            pagina.AddAligned(columna4, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sImporteGravado));
            pagina.AddAligned(columna5, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sImporteExcento));

            if (i >= 2)
            {
                pagina.Add(margenPagina, posY - 10, new RepLine(penDelgada, 0, -(i * 6)));
                pagina.Add(310, posY - 10, new RepLine(penDelgada, 0, -(i * 6)));

                posY += i * (6);
            }
            else
            {
                pagina.Add(margenPagina, posY - 10, new RepLine(penDelgada, 0, -20));
                pagina.Add(310, posY - 10, new RepLine(penDelgada, 0, -20));
                posY += 10;

            }

        }

        private void fnDibujaPanelHorasExtra(ref double horasExtraY, ref Page pagina, Report reporte)
        {

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");

            XPathNavigator nav = gxComprobante.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:HorasExtras/nomina:HorasExtra", nsmComprobante);

            if ((limiteY - horasExtraY) >= 20)
            {
                pagina.Add(margenPagina, horasExtraY, fnDibujarEncabezadoPanelHorasExtra());
                horasExtraY += 10;
            }
            else
            {
                pagina = new Page(PDF);
                pagina.rWidthMM = anchoPagina;
                pagina.rHeightMM = altoPagina;
                paginasReporte.Add(pagina);
                fnCrearEncabezado(ref pagina, ref PDF);
                fnCrearLeyendaPie(ref pagina, ref PDF);
                horasExtraY = 170;
                pagina.Add(margenPagina, horasExtraY, fnPanelRedondeadoPercepciones());
                horasExtraY += 10;
                pagina.Add(margenPagina, horasExtraY, fnDibujarEncabezadoPanelHorasExtra());
                horasExtraY += 10;
            }

            while (iterator.MoveNext())
            {
                string sDias, sHorasExtra, sTipoHora, sImportePagado;
                sDias = sHorasExtra = sTipoHora = sImportePagado = string.Empty;

                try { sDias = iterator.Current.SelectSingleNode("@Dias", nsmComprobante).ToString(); }
                catch { }
                try { sHorasExtra = iterator.Current.SelectSingleNode("@HorasExtra", nsmComprobante).ToString(); }
                catch { }
                try { sTipoHora = iterator.Current.SelectSingleNode("@TipoHoras", nsmComprobante).ToString(); }
                catch { }
                try { sImportePagado = iterator.Current.SelectSingleNode("@ImportePagado", nsmComprobante).ToString(); }
                catch { }

                sImportePagado = TransformacionNomina.fnFormatoRedondeo(sImportePagado);

                if ((limiteY - horasExtraY) > 12)
                {

                    fnDibujaDatosPanelHorasExtra(ref horasExtraY, ref pagina, sDias, sHorasExtra, sTipoHora, sImportePagado);
                    horasExtraY = horasExtraY + 10;
                }
                else
                {
                   // pagina.Add(margenPagina, horasExtraY - 8, new RepLine(penDelgada, 290, 0)); //Linea azul separadora
                    pagina = new Page(PDF);
                    pagina.rWidthMM = anchoPagina;
                    pagina.rHeightMM = altoPagina;
                    paginasReporte.Add(pagina);
                    fnCrearEncabezado(ref pagina, ref PDF);
                    fnCrearLeyendaPie(ref pagina, ref PDF);
                    horasExtraY = 170;
                    pagina.Add(margenPagina, horasExtraY, fnPanelRedondeadoPercepciones());
                    horasExtraY = horasExtraY + 10;
                    pagina.Add(margenPagina, horasExtraY, fnDibujarEncabezadoPanelHorasExtra());
                    horasExtraY = horasExtraY + 10;
                    fnDibujaDatosPanelHorasExtra(ref horasExtraY, ref pagina, sDias, sHorasExtra, sTipoHora, sImportePagado);
                    horasExtraY = horasExtraY + 10;
                }
            }
            pagina.Add(margenPagina, horasExtraY - 7, new RepLine(penDelgada, 290, 0)); //Linea azul separadora
            //horasExtraY = horasExtraY + 10;
        }

        private StaticContainer fnDibujarEncabezadoPanelHorasExtra()
        {
            StaticContainer encabezadoPanelHorasExtra = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            double columna2 = 70;
            double columna3 = 150;
            double columna4 = 260;
            double pfPosY = 0;
            double pfAncho = 290;
            double altoBarra = fPropBlanca.rSize * 2;

            fPropNormal.bBold = true;
            encabezadoPanelHorasExtra.AddAligned(margenPagina, RepObj.rAlignCenter, 0, RepObj.rAlignBottom, new RepString(fPropNormal, "Dias"));
            encabezadoPanelHorasExtra.AddAligned(columna2, RepObj.rAlignCenter, 0, RepObj.rAlignBottom, new RepString(fPropNormal, "Horas Extra"));
            encabezadoPanelHorasExtra.AddAligned(columna3, RepObj.rAlignCenter, 0, RepObj.rAlignBottom, new RepString(fPropNormal, "Tipo Hora"));
            encabezadoPanelHorasExtra.AddAligned(columna4, RepObj.rAlignCenter, 0, RepObj.rAlignBottom, new RepString(fPropNormal, "Importe Pagado"));
            fPropNormal.bBold = false;

            encabezadoPanelHorasExtra.Add(0, 3, new RepLine(penDelgada, 290, 0));
            encabezadoPanelHorasExtra.Add(0, -10, new RepLine(penDelgada, 290, 0));
            encabezadoPanelHorasExtra.Add(0, -10, new RepLine(penDelgada, 0, -13));
            encabezadoPanelHorasExtra.Add(pfAncho, -10, new RepLine(penDelgada, 0, -13));

            return encabezadoPanelHorasExtra;

        }

        private void fnDibujaDatosPanelHorasExtra(ref double posY, ref Page pagina, string sDias, string sHorasExtra, string sTipoHora, string sImportePagado)
        {
            double columna2 = 85;
            double columna3 = 180;
            double columna4 = 305;

            double leftPadding = 0.02;
            double sep = 2.5;
            double posRazon = fPropChica.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
            double i = 0;

            pagina.AddAligned(50, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sDias));
            pagina.AddAligned(columna2, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sHorasExtra));
            pagina.AddAligned(columna3, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sTipoHora));
            pagina.AddAligned(columna4, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sImportePagado));
            pagina.Add(margenPagina, posY - 12, new RepLine(penDelgada, 0, -14));
            pagina.Add(310, posY - 12, new RepLine(penDelgada, 0, -14));
        }

        private void fnDibujaPanelSumaPercepciones(ref double horasExtraY, ref Page pagina, Report reporte)
        {
            //horasExtraY -= 10;
            string sSumaPercepciones = string.Empty;
            try { sSumaPercepciones = "132323.00"; }
            catch { }

            sSumaPercepciones = TransformacionNomina.fnFormatoRedondeo(sSumaPercepciones);

            if ((limiteY - horasExtraY) > 10)
            {

                fPropChica.bBold = true;
                pagina.AddAligned(180, RepObj.rAlignLeft, horasExtraY, RepObj.rAlignBottom, new RepString(fPropChica, "Suma Percepciones"));
                fPropChica.bBold = false;

                pagina.AddAligned(305, RepObj.rAlignRight, horasExtraY, RepObj.rAlignBottom, new RepString(fPropChica, sSumaPercepciones));

                pagina.Add(margenPagina, horasExtraY + 2, new RepLine(penDelgada, 290, 0)); //Linea azul separadora
                pagina.Add(margenPagina, horasExtraY - 10, new RepLine(penDelgada, 0, -12)); //Linea azul separadora
                pagina.Add(310, horasExtraY - 10, new RepLine(penDelgada, 0, -12)); //Linea azul separadora
            }
            else
            {

                pagina = new Page(PDF);
                pagina.rWidthMM = anchoPagina;
                pagina.rHeightMM = altoPagina;
                paginasReporte.Add(pagina);
                fnCrearEncabezado(ref pagina, ref PDF);
                fnCrearLeyendaPie(ref pagina, ref PDF);
                horasExtraY = 170;
                fPropChica.bBold = true;
                pagina.Add(margenPagina, horasExtraY, fnPanelRedondeadoPercepciones());
                horasExtraY += 10;

                pagina.AddAligned(180, RepObj.rAlignLeft, horasExtraY, RepObj.rAlignBottom, new RepString(fPropChica, "Suma Percepciones"));
                fPropChica.bBold = false;
                pagina.AddAligned(305, RepObj.rAlignRight, horasExtraY, RepObj.rAlignBottom, new RepString(fPropChica, sSumaPercepciones));
                pagina.Add(margenPagina, horasExtraY - 10, new RepLine(penDelgada, 290, 0)); //Linea azul separadora
                pagina.Add(margenPagina, horasExtraY + 2, new RepLine(penDelgada, 290, 0)); //Linea azul separadora
                pagina.Add(margenPagina, horasExtraY - 10, new RepLine(penDelgada, 0, -12)); //Linea azul separadora
                pagina.Add(310, horasExtraY - 10, new RepLine(penDelgada, 0, -12)); //Linea azul separadora
            }



        }

        private StaticContainer fnPanelRedondeadoPercepciones()
        {
            StaticContainer encabezadoPanelPercepciones = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            double pfPosY = 0;
            double pfAncho = 290;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(encabezadoPanelPercepciones, 0, pfPosY - 10, pfAncho, altoBarra, grosorPen, 1, true, Color.FromName(sColor));
            double puntoMedio = 0 - fPropBlanca.rSize;
            double posColumna1 = encabezadoPanelPercepciones.rWidth * .1;
            encabezadoPanelPercepciones.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "PERCEPCIONES"));

            return encabezadoPanelPercepciones;
        }

        #endregion

        #region panelesDerechos

        private Page fnDibujaPanelDeducciones(ref double posY, ref List<Page> paginasReporte, Report reporte)
        {
            Page pagina = paginasReporte.First();

            pagina.Add(310, posY, fnDibujarEncabezadoPanelDeducciones());
            posY += 17;

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");

            XPathNavigator nav = gxComprobante.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Deducciones/nomina:Deduccion", nsmComprobante);

            while (iterator.MoveNext())
            {
                string sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento;
                sTipo = sClave = sConcepto = sImporteGravado = sImporteExcento = string.Empty;

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

                sImporteGravado = TransformacionNomina.fnFormatoRedondeo(sImporteGravado);
                sImporteExcento = TransformacionNomina.fnFormatoRedondeo(sImporteExcento);

                double renglon = 0;
                double temporal = 0;
                renglon = fnContarMultilinea(pagina, sConcepto, fPropChica, 2, posY, 30, true);

                if (renglon > 1)
                {
                    temporal = posY + (renglon * (renglon - 1));
                }
                else 
                {
                    temporal = posY;
                }


                if ((limiteY - temporal) > 12)
                {
                    fnDibujaDatosPanelDeducciones(ref posY, ref pagina, sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento);
                   // posY = posY + 10;
                }
                else
                {
                    //Si la lista tiene otra pagina dibuja sobre ella, si no crea una nueva pagina y agregala
                    if (paginasReporte.Count() > 1)
                    {
                        pagina.Add(310, posY, new RepLine(penDelgada, 290, 0)); //Creamos ultima linea azul horizontal
                        paginasReporte.Remove(pagina);
                        pagina = paginasReporte.First();
                        // paginasReporte.Add(pagina);
                        posY = 170;
                        pagina.Add(310, posY, fnDibujarEncabezadoPanelDeducciones());
                        posY = posY + 17;
                        fnDibujaDatosPanelDeducciones(ref posY, ref pagina, sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento);
                       // posY = posY + 10;

                    }
                    else
                    {
                        pagina.Add(310, posY , new RepLine(penDelgada, 290, 0)); //Creamos ultima linea azul horizontal
                        pagina = new Page(PDF);
                        pagina.rWidthMM = anchoPagina;
                        pagina.rHeightMM = altoPagina;
                        //paginasReporte.Add(pagina);
                        fnCrearEncabezado(ref pagina, ref PDF);
                        fnCrearLeyendaPie(ref pagina, ref PDF);
                        posY = 170;
                        pagina.Add(310, posY, fnDibujarEncabezadoPanelDeducciones());
                        posY = posY + 17;
                        fnDibujaDatosPanelDeducciones(ref posY, ref pagina, sTipo, sClave, sConcepto, sImporteGravado, sImporteExcento);
                       // posY = posY + 10;


                    }
                }
            }
            pagina.Add(310, posY , new RepLine(penDelgada, 290, 0)); //Linea azul separadora

            return pagina;
        }

        private StaticContainer fnDibujarEncabezadoPanelDeducciones()
        {
            StaticContainer encabezadoDeducciones = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            double columna2 = 60;
            double columna3 = 110;
            double columna4 = 190;
            double columna5 = 260;

            double pfPosY = 0;
            double pfAncho = 290;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(encabezadoDeducciones, 0, pfPosY - 10, pfAncho, altoBarra, grosorPen, 1, true, Color.FromName(sColor));
            double puntoMedio = 0 - fPropBlanca.rSize;
            double posColumna1 = encabezadoDeducciones.rWidth * .1;
            encabezadoDeducciones.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "DEDUCCIONES"));

            fPropNormal.bBold = true;
            encabezadoDeducciones.AddAligned(margenPagina, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Tipo"));
            encabezadoDeducciones.AddAligned(columna2, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Clave"));
            encabezadoDeducciones.AddAligned(columna3, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Concepto"));
            encabezadoDeducciones.AddAligned(columna4, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Importe Gravado"));
            encabezadoDeducciones.AddAligned(columna5, RepObj.rAlignCenter, 8, RepObj.rAlignBottom, new RepString(fPropNormal, "Importe Excento"));
            fPropNormal.bBold = false;

            encabezadoDeducciones.Add(0, 11, new RepLine(penDelgada, 290, 0));
            encabezadoDeducciones.Add(290, 0, new RepLine(penDelgada, 0, -11));
            encabezadoDeducciones.Add(0, 0, new RepLine(penDelgada, 0, -11));
            return encabezadoDeducciones;
        }

        private void fnDibujaDatosPanelDeducciones(ref double posY, ref Page pagina, string sTipo, string sClave, string sConcepto, string sImporteGravado, string sImporteExcento)
        {
            double columna1 = 335;
            double columna2 = 375.5;
            double columna3 = 400;
            double columna4 = 525.5;
            double columna5 = 590;

            double leftPadding = 0.02;
            double sep = 2.5;
            double posRazon = fPropChica.rSize + sep;
            double i = 0;

            pagina.AddAligned(columna1, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sTipo));
            pagina.AddAligned(columna2, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sClave));
            i = fnAgregarMultilinea(pagina, sConcepto, fPropChica, leftPadding + columna3, posY, 30, true);
            pagina.AddAligned(columna4, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sImporteGravado));
            pagina.AddAligned(columna5, RepObj.rAlignRight, posY, RepObj.rAlignBottom, new RepString(fPropChica, sImporteExcento));

           

            if (i >= 2)
            {
                 pagina.Add(600, posY -10, new RepLine(penDelgada, 0,  -(i * 6) ));
                 pagina.Add(310, posY - 10, new RepLine(penDelgada, 0,  -(i * 6) ));

                 posY += i * (6);
            }
            else 
            {
                pagina.Add(600, posY - 10, new RepLine(penDelgada, 0, -20));
                pagina.Add(310, posY - 10, new RepLine(penDelgada, 0, -20));
                posY += 10;
               
            }

        }

        private Page fnDibujaPanelDiasIncapacidad(ref double incapacidadY, ref Page pagina, Report reporte)
        {
            
            
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");

            XPathNavigator nav = gxComprobante.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("cfdi:Comprobante/cfdi:Complemento/nomina:Nomina/nomina:Incapacidades/nomina:Incapacidad", nsmComprobante);

            if ((limiteY - incapacidadY) >= 20)
            {
                pagina.Add(310, incapacidadY, fnDibujarEncabezadoPanelDiasIncapacidad());
                incapacidadY += 10;
            }
            else 
            {
                pagina = new Page(PDF);
                pagina.rWidthMM = anchoPagina;
                pagina.rHeightMM = altoPagina;
                fnCrearEncabezado(ref pagina, ref PDF);
                fnCrearLeyendaPie(ref pagina, ref PDF);
                incapacidadY = 170;
                pagina.Add(310, incapacidadY, fnPanelRedondeadoDeducciones());
                incapacidadY = incapacidadY + 10;
                pagina.Add(310, incapacidadY, fnDibujarEncabezadoPanelDiasIncapacidad());
                incapacidadY = incapacidadY + 10;
            }


            while (iterator.MoveNext())
            {
                string sDiasIncapacidad, sTipoIncapacidad, sDescuento;
                sDiasIncapacidad = sTipoIncapacidad = sDescuento = string.Empty;

                try { sDiasIncapacidad = iterator.Current.SelectSingleNode("@DiasIncapacidad", nsmComprobante).ToString(); }
                catch { }
                try { sTipoIncapacidad = iterator.Current.SelectSingleNode("@TipoIncapacidad", nsmComprobante).ToString(); }
                catch { }
                try { sDescuento = iterator.Current.SelectSingleNode("@Descuento", nsmComprobante).ToString(); }
                catch { }

                sDescuento = TransformacionNomina.fnFormatoRedondeo(sDescuento);

                if ((limiteY - incapacidadY) > 12)
                {
                    fnDibujaDatosPanelIncapacidad(ref incapacidadY, ref pagina, sDiasIncapacidad, sTipoIncapacidad, sDescuento);
                    incapacidadY = incapacidadY + 10;
                }
                else
                {

                    pagina.Add(310, incapacidadY - 8, new RepLine(penDelgada, 290, 0)); //Creamos ultima linea azul horizontal
                    pagina = new Page(PDF);
                    pagina.rWidthMM = anchoPagina;
                    pagina.rHeightMM = altoPagina;
                    fnCrearEncabezado(ref pagina, ref PDF);
                    fnCrearLeyendaPie(ref pagina, ref PDF);
                    incapacidadY = 170;
                    pagina.Add(310, incapacidadY, fnPanelRedondeadoDeducciones());
                    incapacidadY = incapacidadY + 10;
                    pagina.Add(310, incapacidadY, fnDibujarEncabezadoPanelDiasIncapacidad());
                    incapacidadY = incapacidadY + 10;
                    fnDibujaDatosPanelIncapacidad(ref incapacidadY, ref pagina, sDiasIncapacidad, sTipoIncapacidad, sDescuento);
                    incapacidadY = incapacidadY + 10;
                }
            }

            pagina.Add(310, incapacidadY - 8, new RepLine(penDelgada, 290, 0)); //Linea azul separadora
            this.incapacidadY = incapacidadY + 10;

            return pagina;
        }

        private StaticContainer fnDibujarEncabezadoPanelDiasIncapacidad()
        {
            StaticContainer encabezadoPanelDiasIncapacidad = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            double columna2 = 170;
            double columna3 = 267;

            double pfPosY = 0;
            double pfAncho = 290;
            double altoBarra = fPropBlanca.rSize * 2;

            fPropNormal.bBold = true;
            encabezadoPanelDiasIncapacidad.AddAligned(50, RepObj.rAlignCenter, 0, RepObj.rAlignBottom, new RepString(fPropNormal, "Dias de incapacidad"));
            encabezadoPanelDiasIncapacidad.AddAligned(columna2, RepObj.rAlignCenter, 0, RepObj.rAlignBottom, new RepString(fPropNormal, "Clave"));
            encabezadoPanelDiasIncapacidad.AddAligned(columna3, RepObj.rAlignCenter, 0, RepObj.rAlignBottom, new RepString(fPropNormal, "Concepto"));
            fPropNormal.bBold = false;

            encabezadoPanelDiasIncapacidad.Add(0, 3, new RepLine(penDelgada, 290, 0));
            encabezadoPanelDiasIncapacidad.Add(0, -10, new RepLine(penDelgada, 0, -13));
            encabezadoPanelDiasIncapacidad.Add(290, -10, new RepLine(penDelgada, 0, -13));
            return encabezadoPanelDiasIncapacidad;

        }

        private void fnDibujaDatosPanelIncapacidad(ref double horasExtraY, ref Page pagina, string sDiasIncapacidad, string sTipoIncapacidad, string sDescuento)
        {
            double columna2 = 480;
            double columna3 = 585;

            double leftPadding = 0.02;
            double sep = 2.5;
            double posRazon = fPropChica.rSize + sep; //(Encabezado.rHeight + 20) / 2 + fPropTitulo.rSize;
            double i = 0;

            pagina.AddAligned(380, RepObj.rAlignRight, horasExtraY, RepObj.rAlignBottom, new RepString(fPropChica, sDiasIncapacidad));
            pagina.AddAligned(columna2, RepObj.rAlignRight, horasExtraY, RepObj.rAlignBottom, new RepString(fPropChica, sTipoIncapacidad));
            pagina.AddAligned(columna3, RepObj.rAlignRight, horasExtraY, RepObj.rAlignBottom, new RepString(fPropChica, sDescuento));

            pagina.Add(310, horasExtraY - 10, new RepLine(penDelgada, 0, -12));
            pagina.Add(600, horasExtraY - 10, new RepLine(penDelgada, 0, -12));
        }

        private Page fnDibujaPanelSumaDeducciones(ref double incapacidadY, ref Page pagina, Report reporte)
        {

            incapacidadY -= 8;
            string sSumaDeducciones = string.Empty;
            try { sSumaDeducciones = "132323.00"; }
            catch { }
            sSumaDeducciones = TransformacionNomina.fnFormatoRedondeo(sSumaDeducciones);

            if ((limiteY - incapacidadY) > 10)
            {

                pagina.Add(310, incapacidadY + 2, new RepLine(penDelgada, 290, 0)); //Linea azul separadora
                pagina.Add(310, incapacidadY - 10, new RepLine(penDelgada, 290, 0));
                pagina.Add(310, incapacidadY - 10, new RepLine(penDelgada, 0, -12)); //Linea azul separadora
                pagina.Add(600, incapacidadY - 10, new RepLine(penDelgada, 0, -12)); //Linea azul separadora

                fPropChica.bBold = true;
                pagina.AddAligned(470, RepObj.rAlignLeft, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, "Suma Deducciones"));
                pagina.AddAligned(587, RepObj.rAlignRight, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, sSumaDeducciones));
                incapacidadY += 10;
                pagina.AddAligned(470, RepObj.rAlignLeft, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, "Subtotal"));
                incapacidadY += 10;
                pagina.AddAligned(470, RepObj.rAlignLeft, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, "Descuentos"));
                incapacidadY += 10;
                pagina.AddAligned(470, RepObj.rAlignLeft, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, "ISR"));
                fPropChica.bBold = false;
            }
            else
            {
                pagina = new Page(PDF);
                pagina.rWidthMM = anchoPagina;
                pagina.rHeightMM = altoPagina;
                paginasReporte.Add(pagina);
                fnCrearEncabezado(ref pagina, ref PDF);
                fnCrearLeyendaPie(ref pagina, ref PDF);
                incapacidadY = 170;
                fPropChica.bBold = true;
                pagina.Add(310, incapacidadY, fnPanelRedondeadoDeducciones());
                incapacidadY += 10;
                pagina.AddAligned(470, RepObj.rAlignLeft, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, "Suma Deducciones"));
                pagina.Add(310, incapacidadY + 2, new RepLine(penDelgada, 290, 0)); //Linea azul separadora
                pagina.Add(310, incapacidadY - 10, new RepLine(penDelgada, 0, -12)); //Linea azul separadora
                pagina.Add(600, incapacidadY - 10, new RepLine(penDelgada, 0, -12)); //Linea azul separadora
                pagina.AddAligned(587, RepObj.rAlignRight, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, sSumaDeducciones));
                incapacidadY += 10;
                pagina.AddAligned(470, RepObj.rAlignLeft, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, "Subtotal"));
                incapacidadY += 10;
                pagina.AddAligned(470, RepObj.rAlignLeft, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, "Descuentos"));
                incapacidadY += 10;
                pagina.AddAligned(470, RepObj.rAlignLeft, incapacidadY, RepObj.rAlignBottom, new RepString(fPropChica, "ISR"));
                fPropChica.bBold = false;



            }

            return pagina;
        }

        private StaticContainer fnPanelRedondeadoDeducciones()
        {
            StaticContainer encabezadoPanelDeducciones = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));

            double pfPosY = 0;
            double pfAncho = 290;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(encabezadoPanelDeducciones, 0, pfPosY - 10, pfAncho, altoBarra, grosorPen, 1, true, Color.FromName(sColor));
            double puntoMedio = 0 - fPropBlanca.rSize;
            double posColumna1 = encabezadoPanelDeducciones.rWidth * .1;
            encabezadoPanelDeducciones.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "DEDUCCIONES"));

            return encabezadoPanelDeducciones;
        }


        #endregion

        private void fnCrearPanelSellos(ref Page pagina)
        {
            pagina.Add(margenPagina + 10, RT.rPointFromMM(altoPagina) - margenPagina - pagina.rHeight + (tamCodigo * 2) + 580, GenerarCodigoBidimensional());
            pagina.AddAligned(120, RepObj.rAlignLeft, 660, RepObj.rAlignBottom, new RepString(fPropChica, "Sello digital del Emisor:"));
            pagina.AddAligned(120, RepObj.rAlignLeft, 690, RepObj.rAlignBottom, new RepString(fPropChica, "Sello digital del SAT:"));
            pagina.AddAligned(120, RepObj.rAlignLeft, 720, RepObj.rAlignBottom, new RepString(fPropChica, "Cadena Original del complemento de certificación digital del SAT:"));

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");
            nsmComprobante.AddNamespace("nomina", "http://www.sat.gob.mx/nomina");
            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;
            fnDatosPanelSello(nsmComprobante, navEncabezado, pagina, Renglon);
        }

        private void fnDatosPanelSello(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer panel, double Renglon)
        {
            string sSelloEmisor, sSelloSat, sCadenaOriginal;
            sSelloEmisor = sSelloSat = sCadenaOriginal = string.Empty;

            try { sSelloEmisor = navEncabezado.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value; }
            catch { }
            try { sSelloSat = navEncabezado.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value; }
            catch { }
            try { sCadenaOriginal = "|" + TransformacionNomina.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_3_2") + "||"; }
            catch { }

            double sep = 9;
            double tamRenglon = 16;
            double i = 0;
            double posRazon = fPropChica.rSize + sep;
            double posRenglon = posRazon + sep;
            i = fnAgregarMultilinea(panel, sSelloEmisor, fPropChica, 120, posRenglon + tamRenglon * Renglon + 645, 120, true);
            Renglon += i;

            i = fnAgregarMultilinea(panel, sSelloSat, fPropChica, 120, posRenglon + tamRenglon * Renglon + 645, 120, true);
            Renglon += i;

            i = fnAgregarMultilinea(panel, sCadenaOriginal, fPropChica, 120, posRenglon + tamRenglon * Renglon + 645, 120, true);
            Renglon += i;

        }

        private StaticContainer fnContarPaginas(List<DetalleNomina> detalles)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);
            DetalleNomina[] copiaDetalles = detalles.ToArray();
            DetalleNomina d;
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
                                foreach (ComprobanteImpuestosRetencionTNomina retencion in d.RetencionesT)
                                {
                                    switch (retencion.impuesto)
                                    {
                                        case ComprobanteImpuestosRetencionImpuestoTNomina.IVA: sDetalleTerceros.Append("IVA Ret: "); break;
                                        case ComprobanteImpuestosRetencionImpuestoTNomina.ISR: sDetalleTerceros.Append("ISR: "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionNomina.fnFormatoCurrency(retencion.importe.ToString()) + " ");
                                }
                            }

                            if (d.TrasladosT != null && d.TrasladosT.Count > 0)
                            {
                                foreach (ComprobanteImpuestosTrasladoTNomina traslado in d.TrasladosT)
                                {
                                    switch (traslado.impuesto)
                                    {
                                        case ComprobanteImpuestosTrasladoImpuestoTNomina.IVA: sDetalleTerceros.Append("IVA Tras "); break;
                                        case ComprobanteImpuestosTrasladoImpuestoTNomina.IEPS: sDetalleTerceros.Append("IEPS "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionNomina.fnFormatoPorcentaje(traslado.tasa.ToString()) + ": ");
                                    sDetalleTerceros.Append(TransformacionNomina.fnFormatoCurrency(traslado.importe.ToString()) + " ");
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

        private List<DetalleNomina> fnObtenerDetalles()
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            List<DetalleNomina> detalles = new List<DetalleNomina>();

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
                detalles.Add(new DetalleNomina(navDetalles.Current, nsmComprobante));
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
                string sImagen = sNombreLogo;
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


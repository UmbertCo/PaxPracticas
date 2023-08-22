using Root.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using ThoughtWorks.QRCode.Codec;
using System.Drawing.Imaging;
using System.Drawing;
using System.Globalization;
using CaOriRet;
using RetencionesProyecto.Properties;

namespace RetencionesProyecto
{
    class Funciones
    {
        public Report PDF;
        private FontDef fuenteTitulo;

        public FontDef FuenteTitulo
        {
            get { return fuenteTitulo; }
            set { fuenteTitulo = value; }
        }
        private FontProp fPropTitulo;

        public FontProp FPropTitulo
        {
            get { return fPropTitulo; }
            set { fPropTitulo = value; }
        }

        private const double tamFuenteTitulo = 8;

        public double TamFuenteTitulo
        {
            get { return tamFuenteTitulo; }
        }


        private FontDef fuenteNormal;

        public FontDef FuenteNormal
        {
            get { return fuenteNormal; }
            set { fuenteNormal = value; }
        }

        private FontProp fPropNormal;

        public FontProp FPropNormal
        {
            get { return fPropNormal; }
            set { fPropNormal = value; }
        }

        private const double tamFuenteNormal = 6;

        public double TamFuenteNormal
        {
            get { return tamFuenteNormal; }
        }


        private FontProp fPropChica;

        public FontProp FPropChica
        {
            get { return fPropChica; }
            set { fPropChica = value; }
        }

        private const double tamFuenteChica = 5;

        public double TamFuenteChica
        {
            get { return tamFuenteChica; }
        }


        private FontProp fPropBlanca;

        public FontProp FPropBlanca
        {
            get { return fPropBlanca; }
            set { fPropBlanca = value; }
        }

        private FontProp fPropRoja;

        public FontProp FPropRoja
        {
            get { return fPropRoja; }
            set { fPropRoja = value; }
        }

        private FontProp fPropNegrita;

        public FontProp FPropNegrita
        {
            get { return fPropNegrita; }
            set { fPropNegrita = value; }
        }

        //tamaños en mm
        private const double anchoPagina = 215.9;
        //private const double anchoPagina = 210;

        public double AnchoPagina
        {
            get { return anchoPagina; }
        } 


        private const double altoPagina = 279.4;
        //private const double altoPagina = 297;

        public double AltoPagina
        {
            get { return altoPagina; }
        } 


        //tamaños en puntos
        private const double altoEncabezado = 60;

        public double AltoEncabezado
        {
            get { return altoEncabezado; }
        } 


        private double altoPie = 70;

        public double AltoPie
        {
            get { return altoPie; }
            set { altoPie = value; }
        }

        private int nMaxConceptos = 17;

        public int NMaxConceptos
        {
            get { return nMaxConceptos; }
            set { nMaxConceptos = value; }
        }

        private const double factorSeparador = 2;

        public double FactorSeparador
        {
            get { return factorSeparador; }
        } 

        
        private const double grosorPen = 1;

        public double GrosorPen
        {
            get { return grosorPen; }
        } 


        private const double radioCurva = 4;

        public double RadioCurva
        {
            get { return radioCurva; }
        } 


        //Tamaños en puntos
        private const double margenPagina = 0;

        public double MargenPagina
        {
            get { return margenPagina; }
        } 


        private double margenIzquierdo = RT.rPointFromMM(5); //37

        public double MargenIzquierdo
        {
            get { return margenIzquierdo; }
            set { margenIzquierdo = value; }
        }
//PARA AMPLIAR CONCEPTOS
        private double margenDerecho = RT.rPointFromMM(5); //13

        public double MargenDerecho
        {
            get { return margenDerecho; }
            set { margenDerecho = value; }
        }

        private double margenSuperior = RT.rPointFromMM(23);

        public double MargenSuperior
        {
            get { return margenSuperior; }
            set { margenSuperior = value; }
        }

        private double margenInferior = RT.rPointFromMM(10);

        public double MargenInferior
        {
            get { return margenInferior; }
            set { margenInferior = value; }
        }

        private const double anchoSeccion = anchoPagina - margenPagina * 2;

        public double AnchoSeccion
        {
            get { return anchoSeccion; }
        } 


        private const double tamCodigo = 90;

        public double TamCodigo
        {
            get { return tamCodigo; }
        } 

        private double posCadenaSello = RT.rPointFromMM(5);

        public double PosCadenaSello
        {
            get { return posCadenaSello; }
            set { posCadenaSello = value; }
        }

        private const string leyendaPDF = "Este documento es una representación impresa de un CFDI";

        public string LeyendaPDF
        {
            get { return leyendaPDF; }
        } 


        public static string ruta = System.AppDomain.CurrentDomain.BaseDirectory;


        public Bitmap bLogo
        {

            set;
            get;

        }

        public Report fnGenerarPDF(Color pColor, XmlDocument gxComprobante)
        {

            // crear Documento
            Formatter formato = new PdfFormatter();
            PDF = new Report(formato);

            //Asignar Creador
            PDF.sTitle = "RETENCIONES";
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
            //List<Detalle> detallesNum = fnObtenerDetalles(gxComprobante);

            int nTotPag = 0;
            //altoPie = 90;

            //NOTA MAXCONCEPTOS Y ESTA VARIABLE FIJA TIENEN QUE SER LA MISMA
           

            //Despues de haber calculado total de páginas se reinicia valor altoPie.
            altoPie = 70;

            //Obtenemos los detalles
            List<Detalle> detalles = fnObtenerDetalles(gxComprobante);

            //LISTAS DE COMPLEMENTOS
            List<intereses> intereses = fnObtenerIntereses(gxComprobante);
            List<enajenacionDeAcciones> enajenacionDeAcciones = fnObtenerEnajenacion(gxComprobante);
            List<arrendamientoFideicomiso> arrendamientoFideicomiso = fnObtenerArrendamiento(gxComprobante);
            List<dividendos> dividendos = fnObtenerDividendos(gxComprobante);
            List<sectorFinanciero> sectorFinanciero = fnObtenerSector(gxComprobante);
            List<premios> premios = fnObtenerPremios(gxComprobante);
            List<pagosAExtranjeros> pagosAExtranjeros = fnObtenerPagos(gxComprobante);
            List<derivados> derivados = fnObtenerDerivados(gxComprobante);
            List<fideicomisoNoEmpresarial> fideicomiso = fnObtenerfideicomiso(gxComprobante);
            List<interesesHipotecarios> intereseshipotecarios = fnObtenerInteresesHipotecarios(gxComprobante);
            List<planesDeRetiro> planes = fnObtenerPlanesDeRetiro(gxComprobante);


            pagosAExCont(gxComprobante);
            FideiCont(gxComprobante);

            if (detalles.Count <= nMaxConceptos)
            {
                nTotPag = 1;
            }
            else
            {
                decimal detallesNumPage = (decimal)detalles.Count / nMaxConceptos;
                nTotPag = (int)Math.Ceiling(detallesNumPage);

            }

            bool bSeguir = true;
            int ncontadorComplementos = 0;
            int nTotComPag = nTotPag;

            while (bSeguir)
            {
                //Tamaño carta
                Page pagina = new Page(PDF);
                pagina.rWidthMM = anchoPagina;
                pagina.rHeightMM = altoPagina;

                StaticContainer Encabezado = fnCrearEncabezado(Color.DarkBlue, gxComprobante);

                //fnColocar_Logo(Encabezado.rWidth * 0.02+ margenPagina, 73.0+ margenPagina, bLogo.Width*0.05, bLogo.Height*0.05, pagina);

                StaticContainer Pie = fnCrearPie(pagina.iPageNo, nTotPag, "Black", gxComprobante);

                //Agregar Borde al PIE

                //fnAgregarBordeRedondeado(Pie, grosorPen, 0.1, Color.Black);
                fnCrearPanelRedondeado(Pie, 0, Pie.rHeight, Pie.rWidth, RT.rPointFromMM(15), grosorPen, radioCurva, false, Color.DarkBlue);

                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //Agregar logo de empresa
                try
                {
                    pagina.Add(RT.rPointFromMM(9), RT.rPointFromMM(32), fnImagenEmpresa());
                }
                catch {}
                

                //Agregar marca
                //pagina.Add(Encabezado.rWidth / 2 - RT.rPointFromMM(23), Encabezado.rHeight + 500 / 2, fnImagenBeyond());
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                
                //Agregamos el encabezado y pie a la nueva página
                pagina.Add(margenIzquierdo, margenSuperior, Encabezado);


                pagina.Add(margenIzquierdo, RT.rPointFromMM(altoPagina) - margenPagina - Pie.rHeight, Pie);

                pagina.AddAligned(RT.rPointFromMM(anchoPagina) - 13, RepObj.rAlignRight, RT.rPointFromMM(altoPagina) - margenInferior, RepObj.rAlignCenter, new RepString(fPropNormal, pagina.iPageNo.ToString() + " de " + nTotPag));


                StaticContainer Detalle = fnCrearDetalle(detalles, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);

                pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior - RT.rPointFromMM(10), Detalle);


                //Creamos el área de detalle
                //pagina.Add(margenPagina + 150, margenPagina + 450, fnMarcaAguaCorpus());
                pagina.Add(margenIzquierdo + RT.rPointFromMM(2), margenSuperior + RT.rPointFromMM(20) + Encabezado.rHeight + Detalle.rHeight + Pie.rHeight - margenInferior, fnImagenPAX());


                if (detalles.Count == 0)
                {
                    XPathNavigator navPie = gxComprobante.CreateNavigator();

                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
                    nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
                    nsmComprobante.AddNamespace("arrendamientoenfideicomiso", "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso");
                    nsmComprobante.AddNamespace("dividendos", "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos");
                    nsmComprobante.AddNamespace("enajenaciondeacciones", "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones");
                    nsmComprobante.AddNamespace("fideicomisonoempresarial", "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial");
                    nsmComprobante.AddNamespace("intereses", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses");
                    nsmComprobante.AddNamespace("intereseshipotecarios", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios");
                    nsmComprobante.AddNamespace("operacionesconderivados", "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados");
                    nsmComprobante.AddNamespace("pagosaextranjeros", "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros");
                    nsmComprobante.AddNamespace("planesderetiro", "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro");
                    nsmComprobante.AddNamespace("premios", "http://www.sat.gob.mx/esquemas/retencionpago/1/premios");
                    nsmComprobante.AddNamespace("sectorfinanciero", "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero");
                    nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");


                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/enajenaciondeacciones:EnajenaciondeAcciones", nsmComprobante) != null)
                    {
                        for (int i = 0; i < enajenacionDeAcciones.Count; i++)
                        {
                            StaticContainer Enajenacion = fnCrearEnajenacion(enajenacionDeAcciones, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight/4.5) - RT.rPointFromMM(10), Enajenacion);
                        }
                        ncontadorComplementos = ncontadorComplementos + enajenacionDeAcciones.Count;
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/dividendos:Dividendos", nsmComprobante) != null)
                    {
                        for (int i = 0; i < dividendos.Count; i++)
                        {
                            StaticContainer Dividendos = fnCrearDividendos(dividendos, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), Dividendos);
                        }
                        ncontadorComplementos = ncontadorComplementos + dividendos.Count;
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/intereses:Intereses", nsmComprobante) != null)
                    {
                        for (int i = 0; i < intereses.Count; i++)
                        {
                            StaticContainer Intereses = fnCrearIntereses(intereses, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), Intereses);
                        }
                        ncontadorComplementos = ncontadorComplementos + intereses.Count;
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/arrendamientoenfideicomiso:Arrendamientoenfideicomiso", nsmComprobante) != null)
                    {
                        for (int i = 0; i < arrendamientoFideicomiso.Count; i++)
                        {
                            StaticContainer ArrendamientoFideicomiso = fnCrearArrendamientoFideicomiso(arrendamientoFideicomiso, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), ArrendamientoFideicomiso);
                        }
                        ncontadorComplementos = ncontadorComplementos + arrendamientoFideicomiso.Count;
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/pagosaextranjeros:Pagosaextranjeros", nsmComprobante) != null)
                    {
                        for (int i = 0; i < pagosAExtranjeros.Count; i++)
                        {
                            StaticContainer PagosAExtranjeros = fnCrearPagosAExtranjeros(pagosAExtranjeros, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), PagosAExtranjeros);
                        }
                        ncontadorComplementos = ncontadorComplementos + pagosAExtranjeros.Count;
                
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/premios:Premios", nsmComprobante) != null)
                    {
                        for (int i = 0; i < premios.Count; i++)
                        {
                            StaticContainer Premios = fnCrearPremios(premios, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), Premios);
                        }
                        ncontadorComplementos = ncontadorComplementos + premios.Count;
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsmComprobante) != null)
                    {
                        for (int i = 0; i < fideicomiso.Count; i++)
                        {
                            StaticContainer FideicomisoNoEmpresarial = fnCrearFideicomisoNoEmpresarial(fideicomiso, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), FideicomisoNoEmpresarial);
                        }
                        ncontadorComplementos = ncontadorComplementos + fideicomiso.Count;
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/planesderetiro:Planesderetiro", nsmComprobante) != null)
                    {
                        for (int i = 0; i < planes.Count; i++)
                        {
                            StaticContainer PlanesDeRetiro = fnCrearPlanesDeRetiro(planes, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), PlanesDeRetiro);
                        }
                        ncontadorComplementos = ncontadorComplementos +  planes.Count;
              
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/intereseshipotecarios:Intereseshipotecarios", nsmComprobante) != null)
                    {
                        for (int i = 0; i < intereseshipotecarios.Count; i++)
                        {
                            StaticContainer InteresesHipotecarios = fnCrearInteresesHipotecarios(intereseshipotecarios, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), InteresesHipotecarios);
                        }
                        ncontadorComplementos = ncontadorComplementos + intereseshipotecarios.Count;
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/operacionesconderivados:Operacionesconderivados", nsmComprobante) != null)
                    {
                        for (int i = 0; i < derivados.Count; i++)
                        {
                            StaticContainer Derivados = fnCrearDerivados(derivados, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), Derivados);
                        }
                        ncontadorComplementos = ncontadorComplementos + derivados.Count;
                   
                    }
                    if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/sectorfinanciero:SectorFinanciero", nsmComprobante) != null)
                    {
                        for (int i = 0; i < sectorFinanciero.Count; i++)
                        {
                            StaticContainer SectorFinanciero = fnCrearSectorFinanciero(sectorFinanciero, Color.DarkBlue, pagina.iPageNo, nTotPag, PDF, gxComprobante);
                            pagina.Add(margenIzquierdo, margenPagina + Encabezado.rHeight + margenSuperior + Detalle.rHeight - (Detalle.rHeight / 4.5) - RT.rPointFromMM(10), SectorFinanciero);
                        }
                        ncontadorComplementos = ncontadorComplementos + sectorFinanciero.Count;
                 
                    }
                }

                //verificamos si aún quedan detalles
                if (detalles.Count <= 0 && ncontadorComplementos == 0)
                {
                    pagina.Add(margenIzquierdo + 13, margenSuperior + RT.rPointFromMM(19) + Detalle.rHeight + Encabezado.rHeight, GenerarCodigoBidimensional(gxComprobante));
                    bSeguir = false;
                }
            }
            return PDF;

        }

        public void pagosAExCont(XmlDocument gxComprobante)
        {
                    XPathNavigator navPie = gxComprobante.CreateNavigator();

                    XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
                    nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
                    nsmComprobante.AddNamespace("arrendamientoenfideicomiso", "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso");
                    nsmComprobante.AddNamespace("dividendos", "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos");
                    nsmComprobante.AddNamespace("enajenaciondeacciones", "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones");
                    nsmComprobante.AddNamespace("fideicomisonoempresarial", "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial");
                    nsmComprobante.AddNamespace("intereses", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses");
                    nsmComprobante.AddNamespace("intereseshipotecarios", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios");
                    nsmComprobante.AddNamespace("operacionesconderivados", "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados");
                    nsmComprobante.AddNamespace("pagosaextranjeros", "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros");
                    nsmComprobante.AddNamespace("planesderetiro", "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro");
                    nsmComprobante.AddNamespace("premios", "http://www.sat.gob.mx/esquemas/retencionpago/1/premios");
                    nsmComprobante.AddNamespace("sectorfinanciero", "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero");
                    nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/pagosaextranjeros:Pagosaextranjeros", nsmComprobante) != null)
            {
                nMaxConceptos = 10;
            }
        }

        public void FideiCont(XmlDocument gxComprobante)
        {
            XPathNavigator navPie = gxComprobante.CreateNavigator();

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("arrendamientoenfideicomiso", "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso");
            nsmComprobante.AddNamespace("dividendos", "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos");
            nsmComprobante.AddNamespace("enajenaciondeacciones", "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones");
            nsmComprobante.AddNamespace("fideicomisonoempresarial", "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial");
            nsmComprobante.AddNamespace("intereses", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses");
            nsmComprobante.AddNamespace("intereseshipotecarios", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios");
            nsmComprobante.AddNamespace("operacionesconderivados", "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados");
            nsmComprobante.AddNamespace("pagosaextranjeros", "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros");
            nsmComprobante.AddNamespace("planesderetiro", "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro");
            nsmComprobante.AddNamespace("premios", "http://www.sat.gob.mx/esquemas/retencionpago/1/premios");
            nsmComprobante.AddNamespace("sectorfinanciero", "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero");
            nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            if (navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsmComprobante) != null)
            {
                nMaxConceptos = 10;
            }
        }


        public static void Log(string e)
        {
            string path = Settings.Default.LogError + "LOG.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine("Error " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
            else if (File.Exists(path))
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine("MError " + e + " / " + System.DateTime.Now.ToString());
                tw.Close();
                tw.Dispose();
            }
        }

        public static string fnConstruirCadenaTimbrado(IXPathNavigable xml, string cadenaoriginalTFD)
        {
            string sCadenaOriginal = string.Empty;
            try
            {
                MemoryStream ms = new MemoryStream();
                XslCompiledTransform trans = new XslCompiledTransform();
                trans.Load(typeof(CaOriRet.V10));//Cambiar a cadena de RETENCIONES
                XsltArgumentList args = new XsltArgumentList();
                trans.Transform(xml, args, ms);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                sCadenaOriginal = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }

            return sCadenaOriginal;
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

        private int fnAgregarMultilineaPagare(StaticContainer pContenedor, string psTexto, FontProp pFuente, double pdPosX, double pdPosY, int pnTamRenglon, bool bBuscarEspacio)
        {
            double nAlturaRenglon = pFuente.rSize * 1.5;
            int nNoRenglon = 0;

            foreach (RepString r in fnDividirRenglones(psTexto, pnTamRenglon, pFuente, bBuscarEspacio))
            {
                pContenedor.Add(pdPosX, pdPosY + nAlturaRenglon * nNoRenglon, r);
                nNoRenglon++;
            }

            return nNoRenglon;
        }

        private RepImage fnImagenPAX()
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                string fichero = Settings.Default.imagenes + "logo_pax.png";
                Image image = Image.FromFile(fichero);
                image.Save(ms, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            return new RepImage(ms, 40, 10);
        }

        private RepImage fnImagenEmpresa()
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                string fichero = Settings.Default.imagenes + "logo_empresa.png";
                Image image = Image.FromFile(fichero);
                image.Save(ms, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            return new RepImage(ms, 85, 85);
        }

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

        private StaticContainer fnCrearPie(int nNumPag, int nTotPag, string sColor, XmlDocument gxComprobante)
        {
            StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPie));


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            //nsmComprobante.AddNamespace("implocal", "http://www.sat.gob.mx/implocal");

            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();

            fnTotales(nsmComprobante, navEncabezado, Pie, nNumPag, nTotPag, sColor, gxComprobante);

            return Pie;
        }


        private RepImage GenerarCodigoBidimensional(XmlDocument gxComprobante)
        {
            //REMPLAZAR CON RFC DE INTERNACIONALES CUANDO SEA EXTRANJERO
         

            //Creamos la cadena que generará el código
            XmlNamespaceManager nsm = new XmlNamespaceManager(gxComprobante.NameTable);
            nsm.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsm.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            XPathNavigator navCodigo = gxComprobante.CreateNavigator();

            string Nacional = "";
            string Extranjero = "";

            try{Nacional = navCodigo.SelectSingleNode("/retenciones:Retenciones/retenciones:Receptor/retenciones:Nacional/@RFCRecep", nsm).Value;}catch{}
            try {Extranjero = "XEXX010101000"; }catch {}

            string RR;
            if (Nacional == "" || Nacional == null)
            {
                RR = Extranjero;

            }
            else if (Nacional != "")
            {
                RR = Nacional;
            }
            else
            {
                RR = "";
            }

            string sCadenaCodigo = "?re=" + navCodigo.SelectSingleNode("/retenciones:Retenciones/retenciones:Emisor/@RFCEmisor", nsm).Value
                                + "&rr=" + RR
                                + "&tt=" + string.Format("{0:0000000000.000000}", navCodigo.SelectSingleNode("/retenciones:Retenciones/retenciones:Totales/@montoTotOperacion", nsm).ValueAsDouble)
                                + "&id=" + navCodigo.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/tfd:TimbreFiscalDigital/@UUID", nsm).Value.ToUpper();

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

        private List<Detalle> fnObtenerDetalles(XmlDocument gxComprobante)
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");

            List<Detalle> detalles = new List<Detalle>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();
            XPathNodeIterator navDetalles = navComprobante.Select("/retenciones:Retenciones/retenciones:Totales/retenciones:ImpRetenidos", nsmComprobante);
/////////////////////////////////////////////////////////////////////////////////////////CODIGO A CONSIDERAR////////////////////////////////////////////////////////////////////////////////////////////
            while (navDetalles.MoveNext())
            {
                detalles.Add(new Detalle(navDetalles.Current, nsmComprobante));  //Agrega detalle concepto
                XPathNavigator nodenavigator = navDetalles.Current;
            }
/////////////////////////////////////////////////////////////////////////////////////////CODIGO A CONSIDERAR////////////////////////////////////////////////////////////////////////////////////////////

            return detalles;
        }

        private StaticContainer fnCrearDetalle(List<Detalle> detalles, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            
            fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            Detalle[] copiaDetalles = detalles.ToArray();
            Detalle d;
            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;
            int resta = (int)(areaDetalle.rHeight / 3);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaDetalles.Count() ? copiaDetalles.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.1;

            double posColumna2 = areaDetalle.rWidth * 0.25;

            double posColumna3 = areaDetalle.rWidth * 0.56; //0.2;

            double posColumna4 = areaDetalle.rWidth * 0.8; //0.2;
  

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
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //primero verificamos si la descripción cabrá en el espacio restante
                renglones = fnContarMultilinea(areaDetalle, d.Impuesto, fPropChica, posColumna3, posRenglon, 42, true);
                
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;

                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                string impuesto = "";
                switch (d.Impuesto)
                {
                    case "01":
                        impuesto = "IVA";
                        break;
                    case "02":
                        impuesto = "ISR";
                        break;
                    case "03":
                        impuesto = "IEPS";
                        break;
                    default:
                        impuesto = d.Impuesto;
                        break;
                }
                 Col1 = fnAgregarMultilinea(areaDetalle, impuesto, fPropChica, posColumna1, posRenglon, 9, true);
                 Col2 = fnAgregarMultilinea(areaDetalle, d.TipoPagoRet, fPropChica, posColumna2, posRenglon, 15, true);
                 areaDetalle.AddAligned(posColumna3, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.BaseRet));
                 areaDetalle.AddAligned(posColumna4, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.MontoRet));
                 //areaDetalle.AddAligned(areaDetalle.rWidth - RT.rPointFromMM(2), RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.importe));


                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropChica, posColumna3, posRenglon, 30, true);

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                detalles.Remove(d);
            }


            string montoTotOperacion, montoTotGrav, montoTotExent, montoTotRet;

            XPathNavigator navPie = gxComprobante.CreateNavigator();

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("arrendamientoenfideicomiso", "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso");
            nsmComprobante.AddNamespace("dividendos", "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos");
            nsmComprobante.AddNamespace("enajenaciondeacciones", "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones");
            nsmComprobante.AddNamespace("fideicomisonoempresarial", "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial");
            nsmComprobante.AddNamespace("intereses", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses");
            nsmComprobante.AddNamespace("intereseshipotecarios", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios");
            nsmComprobante.AddNamespace("operacionesconderivados", "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados");
            nsmComprobante.AddNamespace("pagosaextranjeros", "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros");
            nsmComprobante.AddNamespace("planesderetiro", "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro");
            nsmComprobante.AddNamespace("premios", "http://www.sat.gob.mx/esquemas/retencionpago/1/premios");
            nsmComprobante.AddNamespace("sectorfinanciero", "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero");
            nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            List<Impuesto> impuestos = new List<Impuesto>();

            ////////////////////////CAMPOS DE TOTALES/////////////////////////////////////////////////
            montoTotOperacion = navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Totales/@montoTotOperacion", nsmComprobante).Value;
            montoTotGrav = navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Totales/@montoTotGrav", nsmComprobante).Value;
            montoTotExent = navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Totales/@montoTotExent", nsmComprobante).Value;
            montoTotRet = navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Totales/@montoTotRet", nsmComprobante).Value;

            if (detalles.Count == 0)
            {
                //StaticContainer areaDetalle2 = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (37 + 13), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

                double posicionXtotales = areaDetalle.rWidth - areaDetalle.rWidth * 0.01;

                //XHYV,REP
                areaDetalle.AddAligned(posColumna4 - RT.rPointFromMM(20), RepObj.rAlignLeft, fPropChica.rSize + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, RepObj.rAlignTop, new RepString(fPropChica, "Monto Total Exento:"));

                decimal montoTotExentB = Convert.ToDecimal(montoTotExent);
                string montoTotExentC = Math.Round(montoTotExentB, 2).ToString();
                areaDetalle.AddAligned(posicionXtotales, RepObj.rAlignRight, fPropChica.rSize + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, RepObj.rAlignTop, new RepString(fPropChica, "$ " + montoTotExentC));


                areaDetalle.AddAligned(posColumna4 - RT.rPointFromMM(20), RepObj.rAlignLeft, fPropChica.rSize * 2.5 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, RepObj.rAlignTop, new RepString(fPropChica, "Monto Total Gravado:"));

                decimal montoTotGravB = Convert.ToDecimal(montoTotGrav);
                string montoTotGravC = Math.Round(montoTotGravB, 2).ToString();
                areaDetalle.AddAligned(posicionXtotales, RepObj.rAlignRight, fPropChica.rSize * 2.5 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, RepObj.rAlignTop, new RepString(fPropChica, "$ " + montoTotGravC));


                areaDetalle.AddAligned(posColumna4 - RT.rPointFromMM(20), RepObj.rAlignLeft, fPropChica.rSize * 4 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, RepObj.rAlignTop, new RepString(fPropChica, "Monto Total Retenido:"));

                decimal montoTotRetB = Convert.ToDecimal(montoTotRet);
                string montoTotRetC = Math.Round(montoTotRetB, 2).ToString();
                areaDetalle.AddAligned(posicionXtotales, RepObj.rAlignRight, fPropChica.rSize * 4 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, RepObj.rAlignTop, new RepString(fPropChica, "$ " + montoTotRetC));


                areaDetalle.AddAligned(posColumna4 -RT.rPointFromMM(20), RepObj.rAlignLeft, fPropChica.rSize * 13 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, RepObj.rAlignTop, new RepString(fPropChica, "Monto Total de la Operacion:"));

                decimal montoTotOperacionB = Convert.ToDecimal(montoTotOperacion);
                string montoTotOperacionC = Math.Round(montoTotOperacionB, 2).ToString();
                areaDetalle.AddAligned(posicionXtotales, RepObj.rAlignRight, fPropChica.rSize * 13 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, RepObj.rAlignTop, new RepString(fPropChica, "$ " + montoTotOperacionC));

                //ADJUNTOS
                areaDetalle.Add(RT.rPointFromMM(45), fPropChica.rSize + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, new RepString(fPropNegrita, "Total con letra:"));
                fnAgregarMultilinea(areaDetalle, fnTextoImporte(Transformacion.fnFormatoRedondeo(montoTotOperacion), "MXN"), fPropChica, RT.rPointFromMM(45), fPropChica.rSize * 2.5 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02, 80, true);

                fnAgregarMultilinea(areaDetalle, "Forma de Pago:", fPropNegrita, RT.rPointFromMM(45),fPropChica.rSize + areaDetalle.rHeight + areaDetalle.rHeight * 0.02 + RT.rPointFromMM(6), 80, true);
                fnAgregarMultilinea(areaDetalle, "Pago en una sola exhibición", fPropChica, RT.rPointFromMM(45), fPropChica.rSize * 2.5 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02 + RT.rPointFromMM(6), 80, true);

                fnAgregarMultilinea(areaDetalle, "Método de Pago:", fPropNegrita, RT.rPointFromMM(45), fPropChica.rSize + areaDetalle.rHeight + areaDetalle.rHeight * 0.02 + RT.rPointFromMM(12), 80, true);
                fnAgregarMultilinea(areaDetalle, "No aplica", fPropChica, RT.rPointFromMM(45), fPropChica.rSize * 2.5 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02 + RT.rPointFromMM(12), 80, true);

                fnAgregarMultilinea(areaDetalle, "Número de Cuenta:", fPropNegrita, RT.rPointFromMM(45), fPropChica.rSize + areaDetalle.rHeight + areaDetalle.rHeight * 0.02 + RT.rPointFromMM(18), 80, true);
                fnAgregarMultilinea(areaDetalle, "No aplica", fPropChica, RT.rPointFromMM(45), fPropChica.rSize * 2.5 + areaDetalle.rHeight + areaDetalle.rHeight * 0.02 + RT.rPointFromMM(18), 80, true); 
                //ADJUNTOS

                //PANELES

                //PANEL DE COMPLEMENTOS
                fnCrearPanelRedondeado(areaDetalle, 0, areaDetalle.rHeight - RT.rPointFromMM(30), areaDetalle.rWidth, RT.rPointFromMM(30), grosorPen, radioCurva, false, sColor);


                //Panel de complementos Relleno
                fnCrearPanelRedondeado(areaDetalle, 0, areaDetalle.rHeight - RT.rPointFromMM(33), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, true, sColor);


                //Paneles de totales

                fnCrearPanelRedondeado(areaDetalle, 0, areaDetalle.rHeight, RT.rPointFromMM(40), RT.rPointFromMM(30), grosorPen, radioCurva, false, sColor);

                fnCrearPanelRedondeado(areaDetalle, RT.rPointFromMM(40), areaDetalle.rHeight, areaDetalle.rWidth - RT.rPointFromMM(104.9), RT.rPointFromMM(30), grosorPen, radioCurva, false, sColor);

                fnCrearPanelRedondeado(areaDetalle, areaDetalle.rWidth - RT.rPointFromMM(64.9), areaDetalle.rHeight, RT.rPointFromMM(64.9), RT.rPointFromMM(30), grosorPen, radioCurva, false, sColor);

                fnCrearPanelRedondeado(areaDetalle, 0, areaDetalle.rHeight - RT.rPointFromMM(30),areaDetalle.rWidth, RT.rPointFromMM(6), grosorPen, 0.1, false, Color.DarkBlue);
            }
                return areaDetalle;

        }



        public void fnColocar_Logo(double px, double py, double pW, double pH, Page pPagina_Actual)
        {
            MemoryStream ms = new MemoryStream();


            bLogo.Save(ms, ImageFormat.Jpeg);

            RepImage riLogo = new RepImage(ms, pW, pH);

            Page pPagina = pPagina_Actual;

            pPagina_Actual.Add(px, py, riLogo);

        }

        private StaticContainer fnCrearEncabezado(System.Drawing.Color sColor, XmlDocument gxComprobante)
        {
            StaticContainer Encabezado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoEncabezado));

            fnCrearPanelRedondeado(Encabezado, 0, -RT.rPointFromMM(22), Encabezado.rWidth, Encabezado.rHeight - RT.rPointFromMM(1.5), grosorPen, 0.1, false, Color.DarkBlue);

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("arrendamientoenfideicomiso", "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso");
            nsmComprobante.AddNamespace("dividendos", "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos");
            nsmComprobante.AddNamespace("enajenaciondeacciones", "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones");
            nsmComprobante.AddNamespace("fideicomisonoempresarial", "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial");
            nsmComprobante.AddNamespace("intereses", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses");
            nsmComprobante.AddNamespace("intereseshipotecarios", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios");
            nsmComprobante.AddNamespace("operacionesconderivados", "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados");
            nsmComprobante.AddNamespace("pagosaextranjeros", "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros");
            nsmComprobante.AddNamespace("planesderetiro", "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro");
            nsmComprobante.AddNamespace("premios", "http://www.sat.gob.mx/esquemas/retencionpago/1/premios");
            nsmComprobante.AddNamespace("sectorfinanciero", "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero");
            nsmComprobante.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            XPathNavigator navEncabezado = gxComprobante.CreateNavigator();
            double Renglon = 0;

            fnDatosEmisor(nsmComprobante, navEncabezado, Encabezado, sColor, ref Renglon);
            fnDatosReceptor(nsmComprobante, navEncabezado, Encabezado, ref Renglon);


            fnDibujarTitulosDetalle(Encabezado, sColor);

            return Encabezado;
        }

        private void fnDatosEmisor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, System.Drawing.Color sColor, ref double Renglon)
        {
            string razonSocial, rfc, curp, version, fecha, folio, fechaTimb, numcert, Sello, cert, CveRetenc, DescRetenc;
            razonSocial = rfc = curp = version = fecha = folio = fechaTimb = numcert = Sello = cert = CveRetenc = DescRetenc = string.Empty;

            version = navEncabezado.SelectSingleNode("/retenciones:Retenciones/@Version", nsmComprobante).Value;

            try { folio = navEncabezado.SelectSingleNode("/retenciones:Retenciones/@FolioInt", nsmComprobante).Value; }catch { }

            Sello = navEncabezado.SelectSingleNode("/retenciones:Retenciones/@Sello", nsmComprobante).Value;

            numcert = navEncabezado.SelectSingleNode("/retenciones:Retenciones/@NumCert", nsmComprobante).Value;

            cert = navEncabezado.SelectSingleNode("/retenciones:Retenciones/@Cert", nsmComprobante).Value;

            fecha = navEncabezado.SelectSingleNode("/retenciones:Retenciones/@FechaExp", nsmComprobante).Value;

            CveRetenc = navEncabezado.SelectSingleNode("/retenciones:Retenciones/@CveRetenc", nsmComprobante).Value;

            try {DescRetenc = navEncabezado.SelectSingleNode("/retenciones:Retenciones/@DescRetenc", nsmComprobante).Value;}catch{}

            try { fechaTimb = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value; }
            catch { }

            DateTime fechaComprobante = DateTime.MinValue;
            DateTime fechaTimbrado = DateTime.MinValue;
            if (!string.IsNullOrEmpty(fecha))
                fechaComprobante = Convert.ToDateTime(fecha);

            if (!string.IsNullOrEmpty(fechaTimb))
                fechaTimbrado = Convert.ToDateTime(fechaTimb);

            //OPCIONAL
            try {razonSocial = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Emisor/@NomDenRazSocE", nsmComprobante).Value;}catch { }
            //OBLIGATORIA
            rfc = "RFC: " + navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Emisor/@RFCEmisor", nsmComprobante).Value;
            //OPCIONAL
            try {curp = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Emisor/@CURPE", nsmComprobante).Value;}catch { }



//********************************************************************************************************************************************************************************


            double leftPadding = Encabezado.rWidth * 0.02 + 1;
            double sep = 1;
            double posRazon = fPropTitulo.rSize + sep;
            double tamRenglon = fPropNormal.rSize + sep;
            //double posRenglon = posRazon + sep;
            double posRenglon = -30.0;

            Renglon = fnAgregarMultilinea(Encabezado, razonSocial, fPropTitulo, Encabezado.rWidth / 2 - RT.rPointFromMM(55), posRenglon, 50, true);
            Renglon = fnAgregarMultilinea(Encabezado, rfc, fPropNormal, Encabezado.rWidth / 2 - RT.rPointFromMM(55), posRenglon + 12.0, 50, true);
            Renglon = fnAgregarMultilinea(Encabezado, curp, fPropNormal, Encabezado.rWidth / 2 - RT.rPointFromMM(55), posRenglon + 24.0, 50, true);


            //Agregamos los paneles visuales para el tipo de documento, serie y folio
            double fAltoPanel = Encabezado.rHeight / 6;
            double fAnchoPanel = Encabezado.rWidth / 5;
            double posX = Encabezado.rWidth - fAnchoPanel;

            //PANELES CURVOS OPCIONALES

            fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - 192, -RT.rPointFromMM(22), fAnchoPanel + 75, fAltoPanel - 2, grosorPen, radioCurva, false, sColor);
            fnCrearPanelRedondeado(Encabezado, Encabezado.rWidth - 192, -RT.rPointFromMM(12.5), fAnchoPanel + 75, fAltoPanel - 2, grosorPen, radioCurva, false, sColor);

            string TipoDocumento = "";
            switch (CveRetenc)
	        {
                case "1":
                    TipoDocumento = "Servicios Profesionales.";
                    break;
                case "2":
                    TipoDocumento = "Regalías por derechos de autor.";
                    break;
                case "3":
                    TipoDocumento = "Autotransporte terrestre de carga.";
                    break;
                case "4":
                    TipoDocumento = "Servicios prestados por comisionistas.";
                    break;
                case "5":
                    TipoDocumento = "Arrendamiento.";
                    break;
                case "6":
                    TipoDocumento = "Enajenación de acciones.";
                    break;
                case "7":
                    TipoDocumento = "Enajenación de bienes objeto de la LIEPS, a través de mediadores, agentes, representantes, corredores, consignatarios o distribuidores.";
                    break;
                case "8":
                    TipoDocumento = "Enajenación de bienes inmuebles consignada en escritura pública.";
                    break;
                case "9":
                    TipoDocumento = "Enajenación de otros bienes, no consignada en escritura pública.";
                    break;
                case "10":
                    TipoDocumento = "Adquisición de desperdicios industriales.";
                    break;
                case "11":
                    TipoDocumento = "Adquisición de bienes consignada en escritura pública.";
                    break;
                case "12":
                    TipoDocumento = "Adquisición de otros bienes, no consignada en escritura pública.";
                    break;
                case "13":
                    TipoDocumento = "Otros retiros de AFORE.";
                    break;
                case "14":
                    TipoDocumento = "Dividendos o utilidades distribuidas.";
                    break;
                case "15":
                    TipoDocumento = "Remanente distribuible.";
                    break;
                case "16":
                    TipoDocumento = "Intereses.";
                    break;
                case "17":
                    TipoDocumento = "Arrendamiento en fideicomiso.";
                    break;
                case "18":
                    TipoDocumento = "Pagos realizados a favor de residentes en el extranjero.";
                    break;
                case "19":
                    TipoDocumento = "Enajenación de acciones u operaciones en bolsa de valores.";
                    break;
                case "20":
                    TipoDocumento = "Obtención de premios.";
                    break;
                case "21":
                    TipoDocumento = "Fideicomisos que no realizan actividades empresariales.";
                    break;
                case "22":
                    TipoDocumento = "Planes personales de retiro.";
                    break;
                case "23":
                    TipoDocumento = "Intereses reales deducibles por créditos.";
                    break;
                case "24":
                    TipoDocumento = "Operaciones Financieras Derivadas de Capital.";
                    break;
                case "25":
                    TipoDocumento = "Otro tipo de Retenciones";
                    break;
		        default:
                    TipoDocumento = "Otro tipo de Retenciones";
                    break;
	        }

            fPropNormal.rSize = 5;
            

            string sUUID = string.Empty;
            string noCertificadoSAT = string.Empty;
            string sDateElaboracion = string.Empty;

            try { sUUID = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/tfd:TimbreFiscalDigital/@UUID", nsmComprobante).Value; }
            catch { }
            try { noCertificadoSAT = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value; }
            catch { }

            posX = Encabezado.rWidth - (Encabezado.rWidth * 0.02);
            double posDataEmi = -RT.rPointFromMM(10);

            fPropNegrita = new FontProp(fuenteNormal, 7, Color.Black);
            fPropNegrita.bBold = true;

            Encabezado.AddAligned(Encabezado.rWidth - (Encabezado.rWidth / 7), RepObj.rAlignCenter, posDataEmi - RT.rPointFromMM(7), RepObj.rAlignTop, new RepString(fPropNegrita, "RETENCIONES"));


            fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
            fPropNegrita.bBold = true;

            //Folio Fiscal
            if (!string.IsNullOrEmpty(sUUID))
            {
                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNegrita, "Folio Fiscal:"));

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNormal, sUUID.ToUpper()));

                posDataEmi = posDataEmi + RT.rPointFromMM(4);
            }

            fPropNormal = new FontProp(fuenteNormal, 4);
            fPropNegrita = new FontProp(fuenteNormal, 5, Color.Black);
            fPropNegrita.bBold = true;
            fPropRoja = new FontProp(fuenteNormal, 5, Color.Red);
            
// ARGUMENTOS OPCIONALES
            //Numero de Serie del Emisor
            if (!string.IsNullOrEmpty(numcert))
            {
                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNegrita, "No. de Serie del Certificado del Emisor:"));

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNormal, numcert));

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

            }

            //Numero de Serie del SAT
            if (!string.IsNullOrEmpty(noCertificadoSAT))
            {
                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNegrita, "No. de Serie del Certificado del SAT:"));

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNormal, noCertificadoSAT));

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

            }

            //Fecha de expedicion
            if (fechaTimbrado != DateTime.MinValue)//fechaComprobante != DateTime.MinValue)
            {
                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNegrita, "Fecha y hora de certificación:"));

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNormal, fechaTimbrado.ToString("s"))); //fechaComprobante.ToString("s")));*/

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

            }

            sDateElaboracion = fechaComprobante.ToString("yyyy/MM/dd") + " T " + fechaComprobante.ToString("HH") + ":" + fechaComprobante.ToString("mm") + ":" + fechaComprobante.ToString("ss"); //fechaComprobante.ToString("dd") + " de " + fechaComprobante.ToString("MMMM") + " de " + fechaComprobante.ToString("yyyy") + " T " + fechaComprobante.ToString("HH") +":"+ fechaComprobante.ToString("mm") +":"+ fechaComprobante.ToString("ss");
            //Lugar y Fecha
            if (fechaComprobante != DateTime.MinValue)
            {
                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNegrita, "Fecha y hora de emisión:"));

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNormal, sDateElaboracion));

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

            }

            //Serie y Folio
            if (!string.IsNullOrEmpty(folio))
            {
               /* posX = Encabezado.rWidth - fAnchoPanel;

                Encabezado.AddAligned(posX, RepObj.rAlignRight, fAltoPanel / 2, RepObj.rAlignTop, new RepString(fPropNegrita, "Folio:"));
                Encabezado.AddAligned(posX, RepObj.rAlignRight, fAltoPanel / 2 + 8, RepObj.rAlignTop, new RepString(fPropRoja, folio));

                Encabezado.AddAligned(RT.rPointFromMM(18.5), RepObj.rAlignRight, RT.rPointFromMM(3), RepObj.rAlignTop, new RepString(fPropNegrita, "Folio:"));
                Encabezado.AddAligned(RT.rPointFromMM(12), RepObj.rAlignRight, RT.rPointFromMM(6), RepObj.rAlignTop, new RepString(fPropRoja, folio));*/


            }


            if (!string.IsNullOrEmpty(TipoDocumento))
            {
                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNegrita, "Tipo de Documento"));

                posDataEmi = posDataEmi + RT.rPointFromMM(3.5);

                Encabezado.AddAligned(posX, RepObj.rAlignRight,
                    posDataEmi, RepObj.rAlignTop,
                    new RepString(fPropNormal, TipoDocumento));
            }

            fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
            fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
            fPropNegrita.bBold = true;
            fPropRoja = new FontProp(fuenteNormal, tamFuenteNormal, Color.Red);
            

        }

        private void fnTotales(XmlNamespaceManager nsmComprobante, XPathNavigator navPie,
           StaticContainer Pie, int nNumPag, int nTotPag, string sColor, XmlDocument gxComprobante)
        {
            System.Drawing.ColorConverter colConvert = new ColorConverter();
            System.Drawing.Color ColorT = new System.Drawing.Color();

            ColorT = (System.Drawing.Color)colConvert.ConvertFromString(sColor);

            //CHECAR

            string subtotal, total, moneda, sello, timbre, formaDePago, metodoPago, Regimenfiscal, version, sNumCtaPago, descuento, motivoDescuento, sFecha;
            subtotal = total = moneda = sello = timbre = formaDePago = Regimenfiscal = metodoPago = version = sNumCtaPago = descuento = motivoDescuento = sFecha = string.Empty;

            List<Impuesto> impuestos = new List<Impuesto>();
            List<ImpuestoComp> impuestosComp = new List<ImpuestoComp>();
            try
            {
                //subtotal = navPie.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsmComprobante).Value;
                total = navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Totales/@montoTotOperacion", nsmComprobante).Value;

                timbre = navPie.SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value;

                //formaDePago = navPie.SelectSingleNode("/cfdi:Comprobante/@formaDePago", nsmComprobante).Value;

                sello = navPie.SelectSingleNode("/retenciones:Retenciones/@Sello", nsmComprobante).Value;
                version = navPie.SelectSingleNode("/retenciones:Retenciones/@Version", nsmComprobante).Value;
            }
            catch { }
            try { formaDePago= "Pago en una sola exhibición"; }
            catch { }
            try { formaDePago = "No aplica"; }
            catch { }
            /*try { moneda = navPie.SelectSingleNode("/cfdi:Comprobante/@Moneda", nsmComprobante).Value; }
            catch { }

            try { descuento = navPie.SelectSingleNode("/cfdi:Comprobante/@descuento", nsmComprobante).Value; }
            catch { }

            try { sNumCtaPago = navPie.SelectSingleNode("/cfdi:Comprobante/@NumCtaPago", nsmComprobante).Value; }
            catch { }*/

            try { sFecha = navPie.SelectSingleNode("/retenciones:Retenciones/@FechaExp", nsmComprobante).Value; }
            catch { }

           //CHECAR



            double verPadding = Pie.rHeight * 0.02;
            double horPadding = Pie.rWidth * 0.01;
            double posPanelTotales = Pie.rWidth - 180;

            double altoRenglon = fPropNormal.rSize + verPadding;
            int renglon = 1;
            string textoRenglon = string.Empty;

            //Estos datos estan debajo del CBB
            renglon = 1;
            if (nNumPag == nTotPag)//Si es ultima pagina
            {
                Pie.Add(horPadding, posCadenaSello + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del Emisor:"));
                renglon += fnAgregarMultilinea(Pie, sello, fPropChica, horPadding, posCadenaSello + altoRenglon * renglon, 130, false);

                Pie.Add(horPadding, posCadenaSello + altoRenglon * renglon++, new RepString(fPropNormal, "Sello digital del SAT:"));
                renglon += fnAgregarMultilinea(Pie, timbre, fPropChica, horPadding, posCadenaSello + altoRenglon * renglon, 130, false);

                //Agregamos la cadena original y alargamos la sección del pie según sea necesario
                Pie.Add(horPadding, posCadenaSello + altoRenglon * renglon++, new RepString(fPropNormal, "Cadena original del complemento de certificación digital del SAT:"));

                string sCadenaComplemento = Transformacion.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator());
                renglon += fnAgregarMultilinea(Pie, sCadenaComplemento, fPropChica, horPadding, posCadenaSello + altoRenglon * renglon, 130, false);

                string sCertificadoSAT = gxComprobante.CreateNavigator().SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/tfd:TimbreFiscalDigital/@noCertificadoSAT", nsmComprobante).Value;

                Pie.Add(horPadding, posCadenaSello + altoRenglon * renglon++, new RepString(fPropNormal, "No de serie del Certificado del SAT: " + sCertificadoSAT));

                string sFechaCertificacion = gxComprobante.CreateNavigator().SelectSingleNode("/retenciones:Retenciones/retenciones:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", nsmComprobante).Value;

                Pie.Add(horPadding, posCadenaSello + altoRenglon * renglon++, new RepString(fPropNormal, "Fecha Certificación: " + sFechaCertificacion));

                Pie.Add(horPadding, posCadenaSello + altoRenglon * renglon++, new RepString(fPropNormal, "Fecha Expedición: " + sFecha));


                /*//SECCION MEDIA
                Pie.Add(tamCodigo + horPadding, altoRenglon, new RepString(fPropNegrita, "Total con letra:"));
                fnAgregarMultilinea(Pie, fnTextoImporte(Transformacion.fnFormatoRedondeo(total), moneda), fPropChica, tamCodigo + horPadding, altoRenglon * 1.7, 80, true);

                fnAgregarMultilinea(Pie, "Forma de Pago:", fPropNegrita, tamCodigo + horPadding, altoRenglon, 80, true);
                fnAgregarMultilinea(Pie, formaDePago, fPropChica, tamCodigo + horPadding, altoRenglon * 1.5, 80, true);

                fnAgregarMultilinea(Pie, "Método de Pago:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 2, 80, true);
                fnAgregarMultilinea(Pie, metodoPago, fPropChica, tamCodigo + horPadding, altoRenglon * 2.5, 80, true);

                fnAgregarMultilinea(Pie, "Número de Cuenta:", fPropNegrita, tamCodigo + horPadding, altoRenglon * 3, 80, true);
                fnAgregarMultilinea(Pie, sNumCtaPago, fPropChica, tamCodigo + horPadding, altoRenglon * 2, 80, true);     */    


                renglon = 1;
                Pie.rHeight += renglon * (fPropChica.rSize * 1.2);
                altoPie = Pie.rHeightMM;
            }

            //dibujamos el borde del pie

            //Agrega www.paxfacturacion.com
            fPropNormal = new FontProp(fuenteNormal, 4);
            Pie.AddAligned(fnImagenPAX().rWidth + RT.rPointFromMM(15), RepObj.rAlignCenter, Pie.rHeight - margenInferior, RepObj.rAlignTop, new RepString(fPropNormal, "www.paxfacturacion.com"));

            fPropNormal = new FontProp(fuenteNormal, tamFuenteNormal);
            Pie.AddAligned(Pie.rWidth / 2, RepObj.rAlignCenter, Pie.rHeight - margenInferior, RepObj.rAlignTop, new RepString(fPropNormal, leyendaPDF));

            
            //fnCrearPanelRedondeado(Pie, Pie.rWidth / 2 + RT.rPointFromMM(20), Pie.rHeight - margenInferior, RepObj.rAlignCenter, RepObj.rAlignTop, grosorPen, 0.1, false, Color.Black);
           // fnCrearPanelRedondeado(Pie, 0, Pie.rHeight, Pie.rWidth, RT.rPointFromMM(15), grosorPen, 0.1, false, Color.Black);
            //PANEL DE PIE DE PAGINA
            fnCrearPanelRedondeado(Pie, RT.rPointFromMM(0), RT.rPointFromMM(2.9), Pie.rWidth, Pie.rHeight - (Pie.rHeight / 4), grosorPen, radioCurva, false, Color.DarkBlue);
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
                    break;
                }
            }
            renglones.Add(new RepString(pFuente, psTexto.Substring(nCursorCadena).TrimStart()));

            return renglones;
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

        private void fnDatosReceptor(XmlNamespaceManager nsmComprobante, XPathNavigator navEncabezado, StaticContainer Encabezado, ref double Renglon)
        {
            string nacionalidad, RFCRecep, razonSocial, CURPR, MesIni, MesFin, Ejerc;
            nacionalidad = RFCRecep = razonSocial = CURPR = MesIni = MesFin = Ejerc = string.Empty;

            nacionalidad = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Receptor/@Nacionalidad", nsmComprobante).Value;
            RFCRecep = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Receptor/retenciones:Nacional/@RFCRecep", nsmComprobante).Value;
            try { razonSocial = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Receptor/retenciones:Nacional/@NomDenRazSocR", nsmComprobante).Value; }catch { }
            try { CURPR = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Receptor/retenciones:Nacional/@CURPR", nsmComprobante).Value; }catch { }

            //Datos de Periodo
            MesIni = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Periodo/@MesIni", nsmComprobante).Value;
            MesFin = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Periodo/@MesFin", nsmComprobante).Value;
            Ejerc = navEncabezado.SelectSingleNode("/retenciones:Retenciones/retenciones:Periodo/@Ejerc", nsmComprobante).Value;
            


            double leftPadding = Encabezado.rWidth * 0.02;
            double sep = 5;
            double posRazon = Encabezado.rHeight - Encabezado.rHeight * 0.38; //(Encabezado.rHeight) / 2 + fPropTitulo.rSize;
            double tamRenglon = fPropNormal.rSize + sep;
            double posRenglon = posRazon + sep;




            double auxRenglon = Renglon;
            Renglon += fnAgregarMultilinea(Encabezado, "Cliente", fPropNegrita, leftPadding, posRenglon + tamRenglon * Renglon - RT.rPointFromMM(30), 75, true);
            Renglon += fnAgregarMultilinea(Encabezado, razonSocial, fPropNormal, leftPadding, posRenglon - RT.rPointFromMM(30) + tamRenglon * Renglon++, 55, true);
            Renglon += fnAgregarMultilinea(Encabezado, nacionalidad, fPropNormal, leftPadding, posRenglon - RT.rPointFromMM(30) + tamRenglon * Renglon, 75, true);
            Renglon += fnAgregarMultilinea(Encabezado, "RFC", fPropNegrita, leftPadding, posRenglon - RT.rPointFromMM(30) + tamRenglon * Renglon++, 75, true);
            Renglon += fnAgregarMultilinea(Encabezado, RFCRecep, fPropNormal, leftPadding, posRenglon - RT.rPointFromMM(30) + tamRenglon * Renglon, 75, true);

            //Periodo
            //fnAgregarMultilinea(Encabezado, "Periodo", fPropNegrita, leftPadding, posRenglon - RT.rPointFromMM(26) + tamRenglon * Renglon, 75, true);
            string MesInicial = "";
            string MesFinal = "";

            switch (MesIni)
            {
                case "1":
                    MesInicial = "Enero";
                    break;
                case "2":
                    MesInicial = "Febrero";
                    break;
                case "3":
                    MesInicial = "Marzo";
                    break;
                case "4":
                    MesInicial = "Abril";
                    break;
                case "5":
                    MesInicial = "Mayo";
                    break;
                case "6":
                    MesInicial = "Junio";
                    break;
                case "7":
                    MesInicial = "Julio";
                    break;
                case "8":
                    MesInicial = "Agosto";
                    break;
                case "9":
                    MesInicial = "Septiembre";
                    break;
                case "10":
                    MesInicial = "Octubre";
                    break;
                case "11":
                    MesInicial = "Noviembre";
                    break;
                case "12":
                    MesInicial = "Diciembre";
                    break;
                default:
                    break;
            }
            switch (MesFin)
            {
                case "1":
                    MesFinal = "Enero";
                    break;
                case "2":
                    MesFinal = "Febrero";
                    break;
                case "3":
                    MesFinal = "Marzo";
                    break;
                case "4":
                    MesFinal = "Abril";
                    break;
                case "5":
                    MesFinal = "Mayo";
                    break;
                case "6":
                    MesFinal = "Junio";
                    break;
                case "7":
                    MesFinal = "Julio";
                    break;
                case "8":
                    MesFinal = "Agosto";
                    break;
                case "9":
                    MesFinal = "Septiembre";
                    break;
                case "10":
                    MesFinal = "Octubre";
                    break;
                case "11":
                    MesFinal = "Noviembre";
                    break;
                case "12":
                    MesFinal = "Diciembre";
                    break;
                default:
                    break;
            }
            fnAgregarMultilinea(Encabezado, "Mes Inicial:" + MesInicial, fPropNormal, leftPadding, posRenglon - RT.rPointFromMM(22) + tamRenglon * Renglon, 75, true);
            fnAgregarMultilinea(Encabezado, "Mes Final: " + MesFinal, fPropNormal, leftPadding + RT.rPointFromMM(90), posRenglon - RT.rPointFromMM(22) + tamRenglon * Renglon, 75, true);
            fnAgregarMultilinea(Encabezado, "Ejercicio: " + Ejerc, fPropNormal, leftPadding + RT.rPointFromMM(160), posRenglon - RT.rPointFromMM(22) + tamRenglon * Renglon, 75, true);



            fnCrearPanelRedondeado(Encabezado, 0, Encabezado.rHeight - RT.rPointFromMM(10) - Encabezado.rHeight * 0.3, Encabezado.rWidth, Encabezado.rHeight * 0.3, grosorPen, radioCurva, false, Color.DarkBlue);

            //Panel de relleno de periodo
            fnCrearPanelRedondeado(Encabezado, 0, Encabezado.rHeight - RT.rPointFromMM(10) - Encabezado.rHeight * 0.3, Encabezado.rWidth, (Encabezado.rHeight * 0.3)/4, grosorPen / 2, radioCurva, true, Color.DarkBlue);


            fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
            fPropNegrita.bBold = true;

            fnAgregarMultilinea(Encabezado, "PERIODO", fPropNegrita, leftPadding, posRenglon - RT.rPointFromMM(28) + tamRenglon * Renglon, 75, true);

            fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
            fPropNegrita.bBold = true;

            /*leftPadding = Encabezado.rWidth * 0.7;
            auxRenglon += fnAgregarMultilinea(Encabezado, "RFC", fPropNegrita, leftPadding, posRenglon - RT.rPointFromMM(14) + tamRenglon * auxRenglon, 75, true);
            auxRenglon += fnAgregarMultilinea(Encabezado, RFCRecep, fPropNormal, leftPadding, posRenglon - RT.rPointFromMM(14) + tamRenglon * auxRenglon, 75, true);*/

        }

        private void fnDibujarTitulosDetalle(StaticContainer Encabezado, System.Drawing.Color sColor)
        {
            //Dibujamos el área de los titulos
            double altoBarra = fPropBlanca.rSize * 2;
            fnCrearPanelRedondeado(Encabezado, 0, Encabezado.rHeight - RT.rPointFromMM(10) - altoBarra, Encabezado.rWidth, altoBarra, grosorPen, radioCurva, false, sColor);

            //Dibujamos los titulos del detalle
            //El ancho total del área es de 572 puntos
            double puntoMedio = Encabezado.rHeight - fPropBlanca.rSize - RT.rPointFromMM(10);

            //Definimos la posicion de los titulos
            /*double posColumna1 = Encabezado.rWidth * 0.015;     //Impuesto
            double posColumna2 = Encabezado.rWidth * 0.138;      //Tipo de pago retenido 
            double posColumna3 = Encabezado.rWidth * 0.415;     //Base del impuesto retenido
            double posColumna4 = Encabezado.rWidth * 0.8;     //Monto Retenido*/

            double posColumna1 = Encabezado.rWidth * 0.07;     //Impuesto
            double posColumna2 = Encabezado.rWidth * 0.2;      //Tipo de pago retenido 
            double posColumna3 = Encabezado.rWidth * 0.46;     //Base del impuesto retenido
            double posColumna4 = Encabezado.rWidth * 0.75;     //Monto Retenido


            Encabezado.AddAligned(posColumna1, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "IMPUESTO"));
            RepLine rlLineaClave = new RepLine(new PenProp(PDF, 1), 100, 0);
            Encabezado.AddAligned(posColumna2, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "TIPO DE PAGO RETENIDO"));
            Encabezado.AddAligned(posColumna3, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "BASE DEL IMPUESTO RETENIDO"));
            Encabezado.AddAligned(posColumna4, RepObj.rAlignLeft, puntoMedio, RepObj.rAlignCenter, new RepString(fPropNegrita, "MONTO RETENIDO"));

        }


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

            languaje = new CultureInfo("es-Mx");

            //return parser.ToCustomString(Convert.ToDouble(psValor));
            return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
        }

 ///////////////////////////////////////////////////////////////////////////////////////////


        private List<dividendos> fnObtenerDividendos(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("dividendos", "http://www.sat.gob.mx/esquemas/retencionpago/1/dividendos");

            List<dividendos> dividendos = new List<dividendos>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();
            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante); ;

            try
            {
                XPathNodeIterator div = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/dividendos:Dividendos", nsmComprobante);

                while (div.MoveNext())
                {
                    dividendos.Add(new dividendos(div.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = div.Current;
                }
            }
            catch { }

            return dividendos;
        }
        private List<pagosAExtranjeros> fnObtenerPagos(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("pagosaextranjeros", "http://www.sat.gob.mx/esquemas/retencionpago/1/pagosaextranjeros");


            List<pagosAExtranjeros> pagosAExtranjeros = new List<pagosAExtranjeros>();


            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator pag = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/pagosaextranjeros:Pagosaextranjeros", nsmComprobante);

                while (pag.MoveNext())
                {
                    pagosAExtranjeros.Add(new pagosAExtranjeros(pag.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = pag.Current;
                }
            }
            catch { }
            return pagosAExtranjeros;



        }
        private List<arrendamientoFideicomiso> fnObtenerArrendamiento(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("arrendamientoenfideicomiso", "http://www.sat.gob.mx/esquemas/retencionpago/1/arrendamientoenfideicomiso");

            List<arrendamientoFideicomiso> arrendamientoFideicomiso = new List<arrendamientoFideicomiso>();



            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator arr = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/arrendamientoenfideicomiso:Arrendamientoenfideicomiso", nsmComprobante);

                while (arr.MoveNext())
                {
                    arrendamientoFideicomiso.Add(new arrendamientoFideicomiso(arr.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = arr.Current;
                }
            }
            catch { }
            return arrendamientoFideicomiso;
        }

        private List<enajenacionDeAcciones> fnObtenerEnajenacion(XmlDocument gxComprobante)
        {

            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("enajenaciondeacciones", "http://www.sat.gob.mx/esquemas/retencionpago/1/enajenaciondeacciones");

            List<enajenacionDeAcciones> enajenacionDeAcciones = new List<enajenacionDeAcciones>();
            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator arr = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/enajenaciondeacciones:EnajenaciondeAcciones", nsmComprobante);

                while (arr.MoveNext())
                {
                    enajenacionDeAcciones.Add(new enajenacionDeAcciones(arr.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = arr.Current;
                }
            }
            catch { }

            return enajenacionDeAcciones;

        }
        private List<intereses> fnObtenerIntereses(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("intereses", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereses");

            List<intereses> intereres = new List<intereses>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator inter = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/intereses:Intereses", nsmComprobante);

                while (inter.MoveNext())
                {
                    intereres.Add(new intereses(inter.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = inter.Current;
                }
            }
            catch { }

            return intereres;

        }
        private List<sectorFinanciero> fnObtenerSector(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("sectorfinanciero", "http://www.sat.gob.mx/esquemas/retencionpago/1/sectorfinanciero");

            List<sectorFinanciero> sectorFinanciero = new List<sectorFinanciero>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator sec = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/sectorfinanciero:SectorFinanciero", nsmComprobante);

                while (sec.MoveNext())
                {
                    sectorFinanciero.Add(new sectorFinanciero(sec.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = sec.Current;
                }
            }
            catch { }

            return sectorFinanciero;

        }
        private List<premios> fnObtenerPremios(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("premios", "http://www.sat.gob.mx/esquemas/retencionpago/1/premios");

            List<premios> premios = new List<premios>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator pre = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/premios:Premios", nsmComprobante);

                while (pre.MoveNext())
                {
                    premios.Add(new premios(pre.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = pre.Current;
                }
            }
            catch { }

            return premios;

        }

        private List<derivados> fnObtenerDerivados(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("operacionesconderivados", "http://www.sat.gob.mx/esquemas/retencionpago/1/operacionesconderivados");

            List<derivados> derivados = new List<derivados>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator der = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/operacionesconderivados:Operacionesconderivados", nsmComprobante);

                while (der.MoveNext())
                {
                    derivados.Add(new derivados(der.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = der.Current;
                }
            }
            catch { }

            return derivados;

        }

        private List<fideicomisoNoEmpresarial> fnObtenerfideicomiso(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("fideicomisonoempresarial", "http://www.sat.gob.mx/esquemas/retencionpago/1/fideicomisonoempresarial");

            List<fideicomisoNoEmpresarial> fideicomiso = new List<fideicomisoNoEmpresarial>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator fid = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/fideicomisonoempresarial:Fideicomisonoempresarial", nsmComprobante);

                while (fid.MoveNext())
                {
                    fideicomiso.Add(new fideicomisoNoEmpresarial(fid.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = fid.Current;
                }
            }
            catch { }

            return fideicomiso;

        }
        private List<interesesHipotecarios> fnObtenerInteresesHipotecarios(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("intereseshipotecarios", "http://www.sat.gob.mx/esquemas/retencionpago/1/intereseshipotecarios");

            List<interesesHipotecarios> InteresesHipotecarios = new List<interesesHipotecarios>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator IntHip = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/intereseshipotecarios:Intereseshipotecarios", nsmComprobante);

                while (IntHip.MoveNext())
                {
                    InteresesHipotecarios.Add(new interesesHipotecarios(IntHip.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = IntHip.Current;
                }
            }
            catch { }

            return InteresesHipotecarios;

        }
        private List<planesDeRetiro> fnObtenerPlanesDeRetiro(XmlDocument gxComprobante)
        {


            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("retenciones", "http://www.sat.gob.mx/esquemas/retencionpago/1");
            nsmComprobante.AddNamespace("planesderetiro", "http://www.sat.gob.mx/esquemas/retencionpago/1/planesderetiro");

            List<planesDeRetiro> PlanesDeRetiro = new List<planesDeRetiro>();

            XPathNavigator navComprobante = gxComprobante.CreateNavigator();

            XPathNodeIterator navComplementos = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento", nsmComprobante);

            try
            {
                XPathNodeIterator Plane = navComprobante.Select("/retenciones:Retenciones/retenciones:Complemento/planesderetiro:Planesderetiro", nsmComprobante);

                while (Plane.MoveNext())
                {
                    PlanesDeRetiro.Add(new planesDeRetiro(Plane.Current, nsmComprobante));  //Agrega detalle concepto
                    XPathNavigator nodenavigator = Plane.Current;
                }
            }
            catch { }

            return PlanesDeRetiro;

        }

        private StaticContainer fnCrearEnajenacion(List<enajenacionDeAcciones> enajenacion, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            enajenacionDeAcciones[] copiaEnajenacion = enajenacion.ToArray();
            enajenacionDeAcciones d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;

            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaEnajenacion.Count() ? copiaEnajenacion.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.15;
            double posColumna2 = areaDetalle.rWidth * 0.45;
            double posColumna3 = areaDetalle.rWidth * 0.7; //0.2;

            

            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaEnajenacion[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;


                if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "ENAJENACION DE ACCIONES", fPropNegrita, posColumna1 - RT.rPointFromMM(27), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Contrato de Intermediación", fPropNegrita, posColumna1 - RT.rPointFromMM(5), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Ganancias", fPropNegrita, posColumna2 - RT.rPointFromMM(2), posRenglon - RT.rPointFromMM(5), 15, true);
                    areaDetalle.AddAligned(posColumna3 - RT.rPointFromMM(2), RepObj.rAlignLeft, posRenglon - RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropNegrita, "Perdidas"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.ContratoIntermediacion, fPropChica, posColumna1, posRenglon, 20, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.Ganancia, fPropChica, posColumna2, posRenglon, 15, true);
                areaDetalle.AddAligned(posColumna3, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.Perdida));
                //areaDetalle.AddAligned(areaDetalle.rWidth - RT.rPointFromMM(2), RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.importe));


                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropChica, posColumna3, posRenglon, 30, true);

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                enajenacion.Remove(d);
            }

            return areaDetalle;

        }

        private StaticContainer fnCrearDividendos(List<dividendos> dividendo, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            dividendos[] copiaDividendo = dividendo.ToArray();
            dividendos d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;
            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaDividendo.Count() ? copiaDividendo.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.1;
            double posColumna2 = areaDetalle.rWidth * 0.25;
            double posColumna3 = areaDetalle.rWidth * 0.55; //0.2;
            double posColumna4 = areaDetalle.rWidth * 0.82; //0.2;



            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaDividendo[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;


                if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "DIVIDENDOS", fPropNegrita, posColumna1 - RT.rPointFromMM(15), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Clave", fPropNegrita, posColumna1 - RT.rPointFromMM(2), posRenglon - RT.rPointFromMM(5), 15, true);
                    fnAgregarMultilinea(areaDetalle, "Tipo de Sociedades", fPropNegrita, posColumna2 - RT.rPointFromMM(6), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Retención o Utilidad Nacional", fPropNegrita, posColumna3 - RT.rPointFromMM(16), posRenglon - RT.rPointFromMM(5), 40, true);
                    areaDetalle.AddAligned(posColumna4 - RT.rPointFromMM(16), RepObj.rAlignLeft, posRenglon - RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropNegrita, "Retención o Utilidad Extranjera"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.CveTipDivOUtil, fPropChica, posColumna1, posRenglon, 20, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.TipoSocDistrDiv, fPropChica, posColumna2, posRenglon, 40, true);
                areaDetalle.AddAligned(posColumna3, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.MontISRAcredRetMexico));
                areaDetalle.AddAligned(posColumna4, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.MontISRAcredRetExtranjero));
                //areaDetalle.AddAligned(areaDetalle.rWidth - RT.rPointFromMM(2), RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.importe));


                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropChica, posColumna3, posRenglon, 30, true);

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                dividendo.Remove(d);
            }

            return areaDetalle;

        }

        private StaticContainer fnCrearIntereses(List<intereses> interes, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            intereses[] copiaIntereses = interes.ToArray();
            intereses d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;

            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaIntereses.Count() ? copiaIntereses.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.06;
            double posColumna2 = areaDetalle.rWidth * 0.22;
            double posColumna3 = areaDetalle.rWidth * 0.4; //0.2;
            double posColumna4 = areaDetalle.rWidth * 0.58; //0.2;
            double posColumna5 = areaDetalle.rWidth * 0.75; //0.2;
            double posColumna6 = areaDetalle.rWidth * 0.9; //0.2;



            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaIntereses[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;


                if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "INTERESES", fPropNegrita, posColumna1 - RT.rPointFromMM(7), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Sistema Financiero", fPropNegrita, posColumna1 - RT.rPointFromMM(10), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Intereses Retirados", fPropNegrita, posColumna2 - RT.rPointFromMM(11), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Operaciones Financieras", fPropNegrita, posColumna3 - RT.rPointFromMM(16), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Monto Interés Nominal", fPropNegrita, posColumna4 - RT.rPointFromMM(11), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Monto Interés Real", fPropNegrita, posColumna5 - RT.rPointFromMM(8), posRenglon - RT.rPointFromMM(5), 40, true);
                    areaDetalle.AddAligned(posColumna6 - RT.rPointFromMM(3), RepObj.rAlignLeft, posRenglon - RT.rPointFromMM(4), RepObj.rAlignBottom, new RepString(fPropNegrita, "Perdidas"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.SistFinanciero, fPropChica, posColumna1, posRenglon, 20, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.RetiroAORESRetInt, fPropChica, posColumna2, posRenglon, 40, true);
                Col3 = fnAgregarMultilinea(areaDetalle, d.OperFinancDerivad, fPropChica, posColumna3, posRenglon, 40, true);
                areaDetalle.AddAligned(posColumna4, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.MontIntNominal));
                areaDetalle.AddAligned(posColumna5, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.MontIntReal));
                areaDetalle.AddAligned(posColumna6, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.Perdida));
                //areaDetalle.AddAligned(areaDetalle.rWidth - RT.rPointFromMM(2), RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.importe));


                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropChica, posColumna3, posRenglon, 30, true);

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                interes.Remove(d);
            }

            return areaDetalle;

        }

        private StaticContainer fnCrearArrendamientoFideicomiso(List<arrendamientoFideicomiso> arrFid, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            arrendamientoFideicomiso[] copiaArrFid = arrFid.ToArray();
            arrendamientoFideicomiso d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;

            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaArrFid.Count() ? copiaArrFid.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.1;
            double posColumna2 = areaDetalle.rWidth * 0.45;
            double posColumna3 = areaDetalle.rWidth * 0.8; //0.2;




            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaArrFid[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;


                if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "ARRENDAMIENTO EN FIDEICOMISO", fPropNegrita, posColumna1 - RT.rPointFromMM(17), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Importe Fiduciario", fPropNegrita, posColumna1 - RT.rPointFromMM(7), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Importe de Rendimientos", fPropNegrita, posColumna2 - RT.rPointFromMM(12.5), posRenglon - RT.rPointFromMM(5), 40, true);
                    areaDetalle.AddAligned(posColumna3 - RT.rPointFromMM(12.5), RepObj.rAlignLeft, posRenglon - RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropNegrita, "Importe de Deducciones"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.PagProvEfecPorFiduc, fPropChica, posColumna1, posRenglon, 20, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.RendimFideicom, fPropChica, posColumna2, posRenglon, 40, true);
                areaDetalle.AddAligned(posColumna3, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.DeduccCorresp));

                //areaDetalle.AddAligned(areaDetalle.rWidth - RT.rPointFromMM(2), RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.importe));


                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropChica, posColumna3, posRenglon, 30, true);

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                arrFid.Remove(d);
            }

            return areaDetalle;

        }

        private StaticContainer fnCrearPagosAExtranjeros(List<pagosAExtranjeros> pagos, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            pagosAExtranjeros[] copiaPagos = pagos.ToArray();
            pagosAExtranjeros d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 

            int rengTotal = 0;
            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaPagos.Count() ? copiaPagos.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;




            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.01;
            double posColumna2 = areaDetalle.rWidth * 0.12;
            double posColumna3 = areaDetalle.rWidth * 0.28; 
            double posColumna4 = areaDetalle.rWidth * 0.49;
            double posColumna5 = areaDetalle.rWidth * 0.9;

            double posColumna1A = areaDetalle.rWidth * 0.07;
            double posColumna2A = areaDetalle.rWidth * 0.3;
            double posColumna3A = areaDetalle.rWidth * 0.8; 




            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaPagos[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;

                if (i == 0)
                {
                    fnCrearPanelRedondeado(areaDetalle, 0, -RT.rPointFromMM(40), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, true, sColor);
                    fnCrearPanelRedondeado(areaDetalle, 0, -RT.rPointFromMM(35), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, false, sColor);
                    fnCrearPanelRedondeado(areaDetalle, 0, -RT.rPointFromMM(30), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, true, sColor);
                    fnCrearPanelRedondeado(areaDetalle, 0, -RT.rPointFromMM(25), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, false, sColor);


                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "PAGOS A EXTRANJEROS", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(40), 40, true);


                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;


                    fnAgregarMultilinea(areaDetalle, "Beneficiario del Pago:", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(35), 40, true);
                    fnAgregarMultilinea(areaDetalle, d.EsBenefEfectDelCobro, fPropNegrita, posColumna1 + RT.rPointFromMM(35), posRenglon - RT.rPointFromMM(35), 40, true);


                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;


                    fnAgregarMultilinea(areaDetalle, "No Beneficiario", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(30), 40, true);

                    fnAgregarMultilinea(areaDetalle, "Beneficiario", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(9.5), 40, true);


                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;


                    fnAgregarMultilinea(areaDetalle, "Clave del País", fPropNegrita, posColumna1A - RT.rPointFromMM(8), posRenglon - RT.rPointFromMM(25), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Descripción", fPropNegrita, posColumna2A + RT.rPointFromMM(20), posRenglon - RT.rPointFromMM(25), 50, true);
                    fnAgregarMultilinea(areaDetalle, "Tipo de Contribuyente", fPropNegrita, posColumna3A - RT.rPointFromMM(14), posRenglon - RT.rPointFromMM(25), 40, true);


                    fnAgregarMultilinea(areaDetalle, "RFC", fPropNegrita, posColumna1 + RT.rPointFromMM(5), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "CURP", fPropNegrita, posColumna2 + RT.rPointFromMM(8), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Razón Social", fPropNegrita, posColumna3 + RT.rPointFromMM(8), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Descripción", fPropNegrita, posColumna4 + RT.rPointFromMM(17), posRenglon - RT.rPointFromMM(5), 40, true);
                    areaDetalle.AddAligned(posColumna5 - RT.rPointFromMM(15), RepObj.rAlignLeft, posRenglon - RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropNegrita, "Importe de Deducciones"));

                    
                    
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////

                fPropChica = new FontProp(fuenteNormal, 4);

                fnAgregarMultilinea(areaDetalle, d.PaisDeResidParaEfecFisc, fPropChica, posColumna1A, posRenglon - RT.rPointFromMM(21), 20, true);
                fnAgregarMultilinea(areaDetalle, d.DescripcionConcepto, fPropChica, posColumna2A, posRenglon - RT.rPointFromMM(21), 55, true);
                fnAgregarMultilinea(areaDetalle, d.ConceptoPago, fPropChica, posColumna3A, posRenglon - RT.rPointFromMM(21), 15, true);


                Col1 = fnAgregarMultilinea(areaDetalle, d.RFC, fPropChica, posColumna1, posRenglon, 20, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.CURP, fPropChica, posColumna2, posRenglon, 40, true);
                fnAgregarMultilinea(areaDetalle, d.NomDenRazSocB, fPropChica, posColumna3, posRenglon, 25, true);
                fnAgregarMultilinea(areaDetalle, d.DescripcionConceptoBen, fPropChica, posColumna4, posRenglon, 50, true);
                areaDetalle.AddAligned(posColumna5, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.ConceptoPagoBen));

                fPropChica = new FontProp(fuenteNormal, tamFuenteChica);

                //areaDetalle.AddAligned(areaDetalle.rWidth - RT.rPointFromMM(2), RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.importe));


                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropChica, posColumna3, posRenglon, 30, true);

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                pagos.Remove(d);
            }

            return areaDetalle;

        }

        private StaticContainer fnCrearPremios(List<premios> premios, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            premios[] copiaPremios = premios.ToArray();
            premios d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;

            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaPremios.Count() ? copiaPremios.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            /*double posColumna1 = areaDetalle.rWidth * 0.1;
            double posColumna2 = areaDetalle.rWidth * 0.27;
            double posColumna3 = areaDetalle.rWidth * 0.55; //0.2;
            double posColumna4 = areaDetalle.rWidth * 0.8; //0.2;*/

            double posColumna1 = areaDetalle.rWidth * 0.02;
            double posColumna2 = areaDetalle.rWidth * 0.44;
            double posColumna3 = areaDetalle.rWidth * 0.8; //0.2;
            double posColumna4 = areaDetalle.rWidth * 0.8; //0.2;




            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaPremios[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;


               /* if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "PREMIOS", fPropNegrita, posColumna1 - RT.rPointFromMM(17), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Entidad Federativa", fPropNegrita, posColumna1 - RT.rPointFromMM(11), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Importe de Pago", fPropNegrita, posColumna2 - RT.rPointFromMM(6), posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Importe de Pago Gravado", fPropNegrita, posColumna3 - RT.rPointFromMM(14), posRenglon - RT.rPointFromMM(5), 40, true);
                    areaDetalle.AddAligned(posColumna4 - RT.rPointFromMM(12.5), RepObj.rAlignLeft, posRenglon - RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropNegrita, "Importe de Pago Exento"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.EntidadFederativa, fPropChica, posColumna1, posRenglon, 20, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.MontTotPago, fPropChica, posColumna2, posRenglon, 40, true);
                Col3 = fnAgregarMultilinea(areaDetalle, d.MontTotPagoGrav, fPropChica, posColumna3, posRenglon, 40, true);
                areaDetalle.AddAligned(posColumna4, RepObj.rAlignLeft, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.MontTotPagoExent));*/

                if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "PREMIOS", fPropNegrita, posColumna1 + RT.rPointFromMM(10), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Entidad Federativa:", fPropNegrita, posColumna1, posRenglon, 40, true);
                    fnAgregarMultilinea(areaDetalle, "Total de Pago:", fPropNegrita, posColumna3, posRenglon + RT.rPointFromMM(6), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Total Gravado:", fPropNegrita, posColumna1, posRenglon + RT.rPointFromMM(6), 40, true);
                    areaDetalle.AddAligned(posColumna2, RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(6), RepObj.rAlignBottom, new RepString(fPropNegrita, "Total Exento:"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.EntidadFederativa, fPropChica, posColumna1 + RT.rPointFromMM(35), posRenglon, 20, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.MontTotPago, fPropChica, posColumna3 + RT.rPointFromMM(25), posRenglon + RT.rPointFromMM(6), 40, true);
                Col3 = fnAgregarMultilinea(areaDetalle, d.MontTotPagoGrav, fPropChica, posColumna1 + RT.rPointFromMM(25), posRenglon + RT.rPointFromMM(6), 40, true);
                areaDetalle.AddAligned(posColumna2 + RT.rPointFromMM(25), RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(6), RepObj.rAlignBottom, new RepString(fPropChica, d.MontTotPagoExent));

                //areaDetalle.AddAligned(areaDetalle.rWidth - RT.rPointFromMM(2), RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.importe));


                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropChica, posColumna3, posRenglon, 30, true);

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                premios.Remove(d);
            }

            return areaDetalle;

        }

        private StaticContainer fnCrearFideicomisoNoEmpresarial(List<fideicomisoNoEmpresarial> fideicomiso, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            fideicomisoNoEmpresarial[] copiaFideicomiso = fideicomiso.ToArray();
            fideicomisoNoEmpresarial d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;

            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaFideicomiso.Count() ? copiaFideicomiso.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.03;
            double posColumna2 = areaDetalle.rWidth * 0.42;
            double posColumna3 = areaDetalle.rWidth * 0.7; //0.2;


            double posColumna1A = areaDetalle.rWidth * 0.02;
            double posColumna2A = areaDetalle.rWidth * 0.7;






            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaFideicomiso[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;

                if (i == 0)
                {
                    fnCrearPanelRedondeado(areaDetalle, 0, -RT.rPointFromMM(40), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, true, sColor);
                    fnCrearPanelRedondeado(areaDetalle, 0, -RT.rPointFromMM(35), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, false, sColor);
                    fnCrearPanelRedondeado(areaDetalle, 0, -RT.rPointFromMM(30), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, true, sColor);
                    fnCrearPanelRedondeado(areaDetalle, 0, -RT.rPointFromMM(25), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, false, sColor);

                    fnCrearPanelRedondeado(areaDetalle, 0, +RT.rPointFromMM(5), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, true, sColor);
                    fnCrearPanelRedondeado(areaDetalle, 0, +RT.rPointFromMM(10), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, false, sColor);

                    /*fnCrearPanelRedondeado(areaDetalle, 0, areaDetalle.rHeight - RT.rPointFromMM(43), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, true, sColor);
                    fnCrearPanelRedondeado(areaDetalle, 0, areaDetalle.rHeight - RT.rPointFromMM(47), areaDetalle.rWidth, RT.rPointFromMM(5), grosorPen, radioCurva, true, sColor);*/


                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "FIDEICOMISO NO EMPRESARIAL", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(40), 40, true);


                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;


                   /* fnAgregarMultilinea(areaDetalle, "Beneficiario del Pago:", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(35), 40, true);
                    fnAgregarMultilinea(areaDetalle, d.EsBenefEfectDelCobro, fPropNegrita, posColumna1 + RT.rPointFromMM(35), posRenglon - RT.rPointFromMM(35), 40, true);*/


                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;


                    fnAgregarMultilinea(areaDetalle, "Ingresos O Entrada", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(30), 40, true);

                    fnAgregarMultilinea(areaDetalle, "Deducciones O Salidas", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(9.5), 40, true);


                    fnAgregarMultilinea(areaDetalle, "Retenciones Efectuadas al Fideicomiso", fPropNegrita, posColumna1, posRenglon + RT.rPointFromMM(5), 40, true);


                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;


                    fnAgregarMultilinea(areaDetalle, "Monto Total de Participacíon", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(25), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Acomulados del Periodo", fPropNegrita, posColumna2 , posRenglon - RT.rPointFromMM(25), 50, true);
                    fnAgregarMultilinea(areaDetalle, "Total de Ingresos de Periodo", fPropNegrita, posColumna3, posRenglon - RT.rPointFromMM(25), 40, true);



                    fnAgregarMultilinea(areaDetalle, "Monto Total de Participacíon", fPropNegrita, posColumna1, posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Deducción Autorizadas Periodo", fPropNegrita, posColumna2, posRenglon - RT.rPointFromMM(5), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Total de Egresos en el Periodo", fPropNegrita, posColumna3, posRenglon - RT.rPointFromMM(5), 40, true);


                    fnAgregarMultilinea(areaDetalle, "Monto Retenido del Fideicomiso", fPropNegrita, posColumna2A, posRenglon + RT.rPointFromMM(10), 40, true);
                    areaDetalle.AddAligned(posColumna1A, RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(10), RepObj.rAlignBottom, new RepString(fPropNegrita, "Descripción de Retenciones"));



                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////

                fPropChica = new FontProp(fuenteNormal, 4);

                fnAgregarMultilinea(areaDetalle, d.PropDelMontTot, fPropChica, posColumna1, posRenglon - RT.rPointFromMM(21), 20, true);
                fnAgregarMultilinea(areaDetalle, d.PartPropAcumDelFideicom, fPropChica, posColumna2, posRenglon - RT.rPointFromMM(21), 55, true);
                fnAgregarMultilinea(areaDetalle, d.MontTotEntradasPeriodo, fPropChica, posColumna3, posRenglon - RT.rPointFromMM(21), 15, true);
      


                Col1 = fnAgregarMultilinea(areaDetalle, d.PropDelMontTot2, fPropChica, posColumna1, posRenglon, 20, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.PartPropDelFideicom, fPropChica, posColumna2, posRenglon, 40, true);
                fnAgregarMultilinea(areaDetalle, d.MontTotEgresPeriodo, fPropChica, posColumna3, posRenglon, 40, true);


                fnAgregarMultilinea(areaDetalle, d.DescRetRelPagFideic, fPropChica, posColumna1A, posRenglon + RT.rPointFromMM(15), 50, true);
                areaDetalle.AddAligned(posColumna2A, RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(15), RepObj.rAlignBottom, new RepString(fPropChica, d.MontRetRelPagFideic));

                fPropChica = new FontProp(fuenteNormal, tamFuenteChica);


                //areaDetalle.AddAligned(areaDetalle.rWidth - RT.rPointFromMM(2), RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.importe));


                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropChica, posColumna3, posRenglon, 30, true);

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                fideicomiso.Remove(d);
            }

            return areaDetalle;

        }

        private StaticContainer fnCrearDerivados(List<derivados> derivados, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            derivados[] copiaDerivados = derivados.ToArray();
            derivados d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;

            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaDerivados.Count() ? copiaDerivados.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.04;
            double posColumna2 = areaDetalle.rWidth * 0.7;





            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaDerivados[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;


                if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "OPERACIONES CON DERIVADOS", fPropNegrita, posColumna1 - RT.rPointFromMM(3), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Ganancia Acomulable", fPropNegrita, posColumna1, posRenglon, 40, true);
                    areaDetalle.AddAligned(posColumna1, RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropNegrita, "Perdida Deducible"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.MontGanAcum, fPropChica, posColumna1 + RT.rPointFromMM(40), posRenglon, 20, true);
                areaDetalle.AddAligned(posColumna1 + RT.rPointFromMM(40), RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(5) , RepObj.rAlignBottom, new RepString(fPropChica, d.MontPerdDed));

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                derivados.Remove(d);
            }

            return areaDetalle;

        }

        private StaticContainer fnCrearSectorFinanciero(List<sectorFinanciero> sectorfinanciero, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            sectorFinanciero[] copiaSectorFinanciero = sectorfinanciero.ToArray();
            sectorFinanciero d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;

            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaSectorFinanciero.Count() ? copiaSectorFinanciero.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.04;
            double posColumna2 = areaDetalle.rWidth * 0.7;





            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaSectorFinanciero[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;


                if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "SECTOR FINANCIERO", fPropNegrita, posColumna1 - RT.rPointFromMM(3), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Numero de Fideicomiso", fPropNegrita, posColumna1, posRenglon, 40, true);
                    areaDetalle.AddAligned(posColumna1, RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropNegrita, "Descripción"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.IdFideicom, fPropChica, posColumna1 + RT.rPointFromMM(40), posRenglon, 20, true);
                areaDetalle.AddAligned(posColumna1 + RT.rPointFromMM(40), RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropChica, d.DescripFideicom));

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                sectorfinanciero.Remove(d);
            }

            return areaDetalle;

        }
        private StaticContainer fnCrearInteresesHipotecarios(List<interesesHipotecarios> InteresesHipotecarios, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            interesesHipotecarios[] copiaInteresesHipotecarios = InteresesHipotecarios.ToArray();
            interesesHipotecarios d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;

            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaInteresesHipotecarios.Count() ? copiaInteresesHipotecarios.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            //definimos las posiciones
            double posColumna1 = areaDetalle.rWidth * 0.04;
            double posColumna2 = areaDetalle.rWidth * 0.7;





            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaInteresesHipotecarios[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;


                if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "INTERESES HIPOTECARIOS", fPropNegrita, posColumna1 - RT.rPointFromMM(3), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Credito Financiero", fPropNegrita, posColumna1, posRenglon, 40, true);
                    areaDetalle.AddAligned(posColumna1, RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropNegrita, "Saldo Insoluto"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.CreditoDeInstFinanc, fPropChica, posColumna1 + RT.rPointFromMM(40), posRenglon, 20, true);
                areaDetalle.AddAligned(posColumna1 + RT.rPointFromMM(40), RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(5), RepObj.rAlignBottom, new RepString(fPropChica, d.SaldoInsoluto));

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                InteresesHipotecarios.Remove(d);
            }

            return areaDetalle;

        }

        private StaticContainer fnCrearPlanesDeRetiro(List<planesDeRetiro> planes, System.Drawing.Color sColor, int pnPaginas, int pnPaginaActual, Report PDF, XmlDocument gxComprobante)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2 - (margenIzquierdo + margenDerecho), RT.rPointFromMM(altoPagina - altoEncabezado - altoPie - 40) - margenPagina * 2);

            //fnAgregarBordeRedondeado(areaDetalle, grosorPen, 0.1, sColor);

            planesDeRetiro[] copiaPlanes = planes.ToArray();
            planesDeRetiro d;

            double posRenglon;
            double altoRenglon = fPropChica.rSize * factorSeparador;
            int renglonActual = 1;
            //int     contRenActual = 1;
            int rengTotal = 0;

            int resta = (int)(areaDetalle.rHeight / 2);
            //calculamos el número de renglones a partir del alto del area de detalle entre el alto de cada renglon
            int maxRenglones = (int)(((areaDetalle.rHeight - resta) / (fPropChica.rSize + 2)) - altoRenglon);
            //Definimos el número de conceptos que se agregarán en el for
            int maxConceptos = (maxRenglones > copiaPlanes.Count() ? copiaPlanes.Count() : maxRenglones);

            if (maxConceptos > nMaxConceptos)
                maxConceptos = nMaxConceptos;

            
             double posColumna1 = areaDetalle.rWidth * 0.02;

             double posColumna2 = areaDetalle.rWidth * 0.38;

             double posColumna3 = areaDetalle.rWidth * 0.72; //0.2;

  




            int renglones = 0;
            double a = 0;

            for (int i = 0; renglonActual <= maxRenglones; i++)
            {

                if (maxConceptos <= 0)
                    break;

                d = copiaPlanes[i];
                double nAlturaRenglon = fPropChica.rSize * 1.2;
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                int Col1, Col2, Col3;
                Col1 = Col2 = Col3 = 0;

                if (i == 0)
                {
                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.White);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "PLANES DE RETIRO", fPropNegrita, posColumna1 + RT.rPointFromMM(10), posRenglon - RT.rPointFromMM(10), 40, true);

                    fPropNegrita = new FontProp(fuenteNormal, tamFuenteNormal, Color.Black);
                    fPropNegrita.bBold = true;

                    fnAgregarMultilinea(areaDetalle, "Sistema Financiero:", fPropNegrita, posColumna1, posRenglon, 40, true);
                    fnAgregarMultilinea(areaDetalle, "Total de Aportaciones::", fPropNegrita, posColumna3, posRenglon + RT.rPointFromMM(6), 40, true);
                    fnAgregarMultilinea(areaDetalle, "Retiro de Recursos:", fPropNegrita, posColumna1, posRenglon + RT.rPointFromMM(6), 40, true);
                    areaDetalle.AddAligned(posColumna2, RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(6), RepObj.rAlignBottom, new RepString(fPropNegrita, "Retiros en el Ejercicio Anterior:"));
                }
                //////////////////////////////////////////////////////////////////////////////////////////////CAMBIAR//////////////////////////////////////////////////////////////////
                Col1 = fnAgregarMultilinea(areaDetalle, d.SistemaFinanc, fPropChica, posColumna1 + RT.rPointFromMM(35), posRenglon, 20, true);
                Col2 = fnAgregarMultilinea(areaDetalle, d.MontTotAportAnioInmAnterior, fPropChica, posColumna3 + RT.rPointFromMM(35), posRenglon + RT.rPointFromMM(6), 40, true);
                Col3 = fnAgregarMultilinea(areaDetalle, d.HuboRetirosAnioInmAntPer, fPropChica, posColumna1 + RT.rPointFromMM(35), posRenglon + RT.rPointFromMM(6), 40, true);
                areaDetalle.AddAligned(posColumna2 + RT.rPointFromMM(45), RepObj.rAlignLeft, posRenglon + RT.rPointFromMM(6), RepObj.rAlignBottom, new RepString(fPropChica, d.HuboRetirosAnioInmAnt));

                //areaDetalle.AddAligned(areaDetalle.rWidth - RT.rPointFromMM(2), RepObj.rAlignRight, posRenglon, RepObj.rAlignBottom, new RepString(fPropChica, d.importe));


                //Col3 = fnAgregarMultilineaDetalle(areaDetalle, d.descripcion, fPropChica, posColumna3, posRenglon, 30, true);

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                //renglonActual = renglonActual+1;
                maxConceptos -= 1;
                planes.Remove(d);
            }

            return areaDetalle;

        }
     
        
        

    
    }//Fin de clase
}//Fin de namespace

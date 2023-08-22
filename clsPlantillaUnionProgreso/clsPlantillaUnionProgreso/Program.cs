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

namespace clsPlantillaUnionProgreso
{
    class clsPlantillaUnionProgresoPR
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
        //private const string leyendaPDF = "ESTE DOCUMENTO ES UNA REPRESENTACIÓN GRÁFICA DE UN CFDI";
        private const string sNotasInformativas = "NOTAS INFORMATIVAS: INCUMPLIR CON TUS OBLIGACIONES TE PUEDE GENERAR COMISIONES E INTERESES MORATORIOS. CONTRATAR CREDITOS POR ARRIBA DE TU CAPACIDAD DE PAGO PUEDE AFECTAR TU HISTORIAL CREDITICIO";
        private const string sCatNotas = "CAT (COSTO ANUAL TOTAL, SIN IVA PARA FINES INFORMATIVOS Y DE COMPARACION EXCLUSIVAMENTE). IDE (IMPUESTO A LOS DEPOSITOS EN EFECTIVO)";
        private const string sInformacionImp = 
            "INFORMACION IMPORTANTE: " + "\n" +
            "CUENTAS CON UN PLAZO DE 90 DIAS CONTADOS A PARTIR DE LA FECHA DE CPRTE, PARA OBJETAR TU ESTADO DE CUENTA, POR LO QUE SI NO LO RECIBES OPORTUNAMENTE " +
            "DEBERA SOLICITARLO A LA INSTITUCION, PARA SU EN CASO PODER OBJETARLO EN TIEMPO. TRANSCURRIDO DICHO PLAZO SIN HABER OBJETADO EL ESTADO DE CUENTA, " + 
            "LOS ASIENTOS QUE FIGUREN EN NUESTRA CONTABILIDAD HARIAN PRUEBA A FAVOR DE AKALA, SA DE CV, SFP.";
        private const string sDudas = 
            "DUDAS O ACLARACIONES CORRESPONDIENTES A LA OPERACION O SERVICIO DEL PRESENTE ESTADO DE CUENTA COMUNICATE A AKALA. (639) 470-8229 "+ 
            "O BIEN A LA UNIDAD ESPECIALIZADA DE ATENCION A USUARIOS(UNE): TELEFONO (614) 415-4553 " +
            "CORREO ELECTRONICO: unidad@unionprogreso.com.mx. " + "\n" +"CONDUSEF TELEFONO: 01-800-999-8080 PAGINA DE INTERNET www.codusef.gob.mx";

        private const string akalaPie = "AKALA S.A DE C.V, S.F.P., PASEO TECNOLOGICO #1 COL. NUEVO TERRAZAS. CD. DELICIAS, CHIHUAHUA, (639)4708200";

        static void Main(string[] args)
        {

            XmlDocument xmlDOC = new XmlDocument();
            xmlDOC.Load(@"C:\Users\Ismael Hidalgo\Desktop\Hector Portillo\Escritorio 2014-01-22\b14ed56b-1418-47a7-8143-9ed1c755590a.xml");
            clsPlantillaUnionProgresoPR plantillaUnion = new clsPlantillaUnionProgresoPR(xmlDOC);
            plantillaUnion.fnGenerarPdf("Red");
        }


        public clsPlantillaUnionProgresoPR(XmlDocument pxComprobante) 
        {
            gxComprobante = pxComprobante;
            XmlDeclaration xDec = gxComprobante.CreateXmlDeclaration("1.0", "UTF-8", "yes");
           //gxComprobante.InsertBefore(xDec, gxComprobante.DocumentElement);
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

            penGruesa = new PenProp(PDF, tamPlumaGruesa, Color.DarkRed);
            penMediana = new PenProp(PDF, tamPlumaMediana, Color.DarkRed);
            penDelgada = new PenProp(PDF, tamPlumaDelgada, Color.DarkRed);

            List<DetalleUnionProg> detallesNum = fnObtenerDetalles();

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
            List<DetalleUnionProg> detalles = fnObtenerDetalles();

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
                StaticContainer panelEstado = fnCrearPanelEstado(pagina.iPageNo);
                StaticContainer panelCliente = fnCrearPanelCliente();
                StaticContainer panelCAT = fnCrearPanelCAT();
                StaticContainer panelRaro = fnCrearPanelRaro();
                StaticContainer panelContrato = fnCrearPanelContrato();
                StaticContainer panelSaldo = fnCrearPanelSaldo();
                StaticContainer panelPago = fnCrearPanelPago();
                StaticContainer panelDesglose = fnCrearPanelDesglose();
                StaticContainer panelPagosRec = fnCrearPanelPagosRecibidos();
                StaticContainer panelMov = fnCrearPanelMovimientos();
                StaticContainer panelCargos = fnCrearPanelCargos();
                StaticContainer tablaFecha = fnCrearTablaFecha();

                altoPie = 105;
                pagina.Add(330, margenPagina + 25, panelEstado);
                pagina.Add(margenPagina, margenPagina + 80, panelCliente);
                pagina.Add(380, margenPagina + 80, panelCAT);
                pagina.Add(margenPagina, margenPagina + 150, panelRaro);
                pagina.Add(margenPagina, margenPagina + 295, panelContrato);
                pagina.Add(margenPagina, margenPagina + 457, panelSaldo);
                pagina.Add(margenPagina + 300, margenPagina + 295, panelPago);
                pagina.Add(margenPagina + 300, margenPagina + 360, panelDesglose);
                pagina.Add(margenPagina + 300, margenPagina + 470, panelPagosRec);
                pagina.Add(anchoPagina /2 , margenPagina + 530, tablaFecha);
                pagina.Add(margenPagina, margenPagina + 600, panelCargos);
                pagina.Add(margenPagina, margenPagina + 670, panelMov);

                pagina.Add(margenPagina + 300, margenPagina + 500, new RepString(fPropChica,"* Si corresponde a un dia inhabil bancario, el pago podra"));
                pagina.Add(margenPagina + 300, margenPagina + 510, new RepString(fPropChica, "realizarse sin cargo adicional al siguiente dia inhabil bancario."));
                pagina.Add(margenPagina + 170, RT.rPointFromMM(altoPagina) - margenPagina + 8, new RepString(fPropChica, akalaPie));
                pagina.Add(margenPagina + 10 , RT.rPointFromMM(altoPagina) - margenPagina + 8, new RepString(fPropChica, "http://www.paxfacturacion.com/"));

                //Si es ultima pagina
                if (pagina.iPageNo == nTotPag)
                {
                    
                   
                }
                
                //Se agrega el numero de paginas en la pagina
                
                //Agregamos las imagenes al documento
                MemoryStream ms1 = new MemoryStream();
                ms1 = fnImagenCliente("AkalaLogo.png");

                //Creamos el área de detalle
                if (ms1.Length > 0)
                {
                   RepImage image1 = new RepImage(ms1, 150, 80);
                    pagina.Add(margenPagina , margenPagina + 70, image1);
                }

               //pagina.Add(margenPagina + 10, margenPagina + Encabezado.rHeight , fnCrearDetalle(detalles, ColorT));
                //verificamos si aún quedan detalles
               // if (detalles.Count <= 0)
                    bSeguir = false;

            }

            // Esto lo voy a usar para generar la grafica como pruebas
            Page pagina2 = new Page(PDF);
            pagina2.rWidthMM = anchoPagina;
            pagina2.rHeightMM = altoPagina;

            MemoryStream ms2 = new MemoryStream();
            ms2 = fnGenerarGrafica();

            RepImage image2 = new RepImage(ms2, 400, 400);
            pagina2.Add(margenPagina, margenPagina + 400, image2);

            RT.ViewPDF(PDF);
        }

        private StaticContainer fnCrearPanelEstado(int nNumeroPagina)
        {
            StaticContainer panelEstado = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            
            FontProp fPropRojoObscuro = new FontProp(fuenteNormal, 10);
            fPropRojoObscuro.color = Color.DarkRed;
            fPropRojoObscuro.bBold = true;
            
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 38;
            
            panelEstado.Add(pfPosX , pfPosY, new RepLine(penGruesa, pfAncho , 0));
            panelEstado.Add(pfPosX, pfAlto, new RepLine(penGruesa, pfAncho, 0));

            fPropNormal.bBold = true;
            panelEstado.Add(5, -3, new RepString(fPropRojoObscuro, "Estado de Cuenta"));
            panelEstado.Add(5, 10, new RepString(fPropNormal, "Periodo"));
            panelEstado.Add(5, 22, new RepString(fPropNormal, "Fecha Expedición"));
            panelEstado.Add(5, 34, new RepString(fPropNormal, "Página"));
            fPropChica.bBold = false;

            return panelEstado;
        }

        private StaticContainer fnCrearPanelCAT() 
        {
            StaticContainer panelGAT = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 200;
            double pfAlto = 50;

            //borde superior
            panelGAT.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelGAT.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelGAT.Add(pfPosX, pfPosY, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelGAT.Add(pfPosX + pfAncho, pfPosY, new RepLine(penMediana, 0, -pfAlto));

            fPropNormal.bBold = true;
            panelGAT.Add(5, 10, new RepString(fPropNormal, "CAT"));
            panelGAT.Add(5, 22, new RepString(fPropNormal, "Tasa Anual Ordinaria"));
            panelGAT.Add(5, 34, new RepString(fPropNormal, "Tasa Anual Moratoria"));
            panelGAT.Add(5, 46, new RepString(fPropNormal, "Comisiones Cobradas"));
            fPropNormal.bBold = false;

            return panelGAT;
        }

        private StaticContainer fnCrearPanelCliente() 
        {
            StaticContainer panelGAT = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
           
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 190;
            double pfAlto = 80;

            fPropNormal.bBold = true;
            panelGAT.Add(5, 10, new RepString(fPropNormal, "Cliente"));
            panelGAT.Add(5, 22, new RepString(fPropNormal, "Nombre"));
            panelGAT.Add(5, 34, new RepString(fPropNormal, "RFC"));
            panelGAT.Add(5, 46, new RepString(fPropNormal, "Domicilio"));
            fPropNormal.bBold = false;

            return panelGAT;
        
        }

        //No se de que otra forma llamarlo, asumo que es el panel de detalle?
        private StaticContainer fnCrearPanelRaro() 
        {
            StaticContainer panelRaro = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            PenProp pen = new PenProp(PDF, 2, Color.DarkBlue);
            
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 570;
            double pfAlto = 125;
            
            //borde superior
            panelRaro.Add(pfPosX, pfPosY, new RepLine(pen, pfAncho, 0));
            //borde inferior
            panelRaro.Add(pfPosX, pfAlto, new RepLine(pen, pfAncho, 0));
            ////borde izquierdo
            panelRaro.Add(pfPosX, pfPosY, new RepLine(pen, 0, -pfAlto));
            ////borde derecho
            panelRaro.Add(pfPosX + pfAncho, pfPosY, new RepLine(pen, 0, -pfAlto));

            return panelRaro;
        }

        //Falta el metodo para agregar la informacion al panel
        private StaticContainer fnCrearPanelContrato() 
        {
            StaticContainer panelContrato = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 150;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelContrato, 0, pfPosY-10, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelContrato.rWidth * 0.05;
            panelContrato.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Contrato"));

            fPropNormal.bBold = true;
            panelContrato.Add(5, 10, new RepString(fPropNormal, "Contrato"));
            panelContrato.Add(5, 22, new RepString(fPropNormal, "Producto"));
            panelContrato.Add(5, 34, new RepString(fPropNormal, "Moneda"));
            panelContrato.Add(5, 46, new RepString(fPropNormal, "Sucursal"));
            panelContrato.Add(5, 58, new RepString(fPropNormal, "Fecha Formalización"));
            panelContrato.Add(5, 70, new RepString(fPropNormal, "Fecha de Corte"));
            panelContrato.Add(5, 82, new RepString(fPropNormal, "Periodo de Pago"));
            panelContrato.Add(5, 94, new RepString(fPropNormal, "Tipos de Interes"));
            panelContrato.Add(5, 106, new RepString(fPropNormal, "Cuota"));
            panelContrato.Add(5, 118, new RepString(fPropNormal, "Plazo"));
            panelContrato.Add(5, 130, new RepString(fPropNormal, "Saldo Inicial"));
            panelContrato.Add(5, 142, new RepString(fPropNormal, "Limite de Credito"));
            fPropNormal.bBold = false;

            //borde superior
            panelContrato.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));

            //borde inferior
            panelContrato.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));

            ////borde izquierdo
            panelContrato.Add(pfPosX, pfPosY, new RepLine(penMediana, 0, -pfAlto));

            ////borde derecho
            panelContrato.Add(pfPosX + pfAncho, pfPosY, new RepLine(penMediana, 0, -pfAlto));
            
            return panelContrato;
        }

        //Panel saldo insoluto, falta el metodo para agregar los datos
        private StaticContainer fnCrearPanelSaldo() 
        {
            StaticContainer panelSaldo = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 40;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelSaldo, 0, pfPosY - 10, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelSaldo.rWidth * 0.075;
            panelSaldo.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Saldo Insoluto"));
            fPropNormal.bBold = true;
            panelSaldo.Add(5, 10, new RepString(fPropNormal, "Capital Vigente"));
            panelSaldo.Add(5, 20, new RepString(fPropNormal, "Capital Vencido"));
            fPropNormal.bBold = false;
            fPropNormal.color = Color.DarkRed;
            panelSaldo.Add(5, 35, new RepString(fPropNormal, "Saldo Insoluto"));
            fPropNormal.color = Color.Black;

            //borde superior
            panelSaldo.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelSaldo.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelSaldo.Add(pfPosX, pfPosY, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelSaldo.Add(pfPosX + pfAncho, pfPosY, new RepLine(penMediana, 0, -pfAlto));
            //Barra separadora horizontal 
            panelSaldo.Add(pfPosX, pfAlto - 15, new RepLine(penMediana, pfAncho, 0));

            return panelSaldo;
        }

        private StaticContainer fnCrearPanelPago() 
        {
            StaticContainer panelPago = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 50;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelPago, 0, pfPosY - 10, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelPago.rWidth * 0.04;
            panelPago.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Pago"));

            fPropNormal.color  = Color.DarkRed;
            panelPago.Add(5, 34, new RepString(fPropNormal, "Fecha Limite de Pago*"));
            panelPago.Add(5, 46, new RepString(fPropNormal, "Total a Pagar"));
            fPropNormal.color = Color.Black;

            //borde superior
            panelPago.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelPago.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelPago.Add(pfPosX, pfPosY, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelPago.Add(pfPosX + pfAncho, pfPosY, new RepLine(penMediana, 0, -pfAlto));

            return panelPago;
        }

        //panel Desglose total a pagar, faltan los metodos para agregar los datos
        private StaticContainer fnCrearPanelDesglose() 
        {
            StaticContainer panelDesglose = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 90;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelDesglose, 0, pfPosY - 10, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelDesglose.rWidth * 0.13;
            panelDesglose.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Desglose del Total a Pagar"));

            //borde superior
            panelDesglose.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelDesglose.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelDesglose.Add(pfPosX, pfPosY, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelDesglose.Add(pfPosX + pfAncho, pfPosY, new RepLine(penMediana, 0, -pfAlto));
           
            fPropNormal.bBold = true;
            //La separacion es de 12 Puntos
            panelDesglose.Add(5, 10, new RepString(fPropNormal, "Capital(+)"));
            panelDesglose.Add(5, 22, new RepString(fPropNormal, "Interes Ordinario(+)"));
            panelDesglose.Add(5, 34, new RepString(fPropNormal, "I.V.A. Sobre Intereses Ordinario(+)"));
            panelDesglose.Add(5, 46, new RepString(fPropNormal, "Interes Moratorio(+)"));
            panelDesglose.Add(5, 58, new RepString(fPropNormal, "I.V.A Sobre Intereses Moratorio(+)"));
            panelDesglose.Add(5, 70, new RepString(fPropNormal, "Comisiones Cobradas(+)"));
            panelDesglose.Add(5, 82, new RepString(fPropNormal, "I.V.A Sobre Comisiones(+)"));
            fPropNormal.bBold = false;

            return panelDesglose;
        
        }

        //Se crea el panel de pagos recibidos, falta el metodo para agregar los datos
        private StaticContainer fnCrearPanelPagosRecibidos() 
        {
            StaticContainer panelPagosRecibidos = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 250;
            double pfAlto = 20;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelPagosRecibidos, 0, pfPosY - 10, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelPagosRecibidos.rWidth * 0.085;
            panelPagosRecibidos.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Pagos Recibidos"));

            //borde superior
            panelPagosRecibidos.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelPagosRecibidos.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));
            ////borde izquierdo
            panelPagosRecibidos.Add(pfPosX, pfPosY, new RepLine(penMediana, 0, -pfAlto));
            ////borde derecho
            panelPagosRecibidos.Add(pfPosX + pfAncho, pfPosY, new RepLine(penMediana, 0, -pfAlto));
            //linea separadora vertical
            panelPagosRecibidos.Add((pfAncho / 2.2), pfPosY, new RepLine(penDelgada, 0, -pfAlto));

            fPropNormal.bBold = true;
            panelPagosRecibidos.Add(5, 10, new RepString(fPropNormal, "Cuotas:"));
            panelPagosRecibidos.Add((pfAncho / 2.2) + 5 , 10, new RepString(fPropNormal, "Monto Capital"));
            


            return panelPagosRecibidos;
        }

        //Crea la tabla que contiene 'Fecha Cuota' Falta definir el metodo para dibujar los datos
        private StaticContainer fnCrearTablaFecha() 
        {
            StaticContainer panelTabla = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 450;
            double pfAlto = 20;
            double altoBarra = fPropBlanca.rSize * 2;

            fnCrearPanelRedondeado(panelTabla, 0, pfPosY - 10, pfAncho, altoBarra, grosorPen, 1, true, Color.DarkRed);
            double puntoMedio = pfPosY - fPropBlanca.rSize;
            double posColumna1 = panelTabla.rWidth * 0.1;
            double posColumna2 = panelTabla.rWidth * 0.3;
            double posColumna3 = panelTabla.rWidth * 0.5;
            double posColumna4 = panelTabla.rWidth * 0.68;
            double posColumna5 = panelTabla.rWidth * 0.9;

            panelTabla.AddAligned(posColumna1, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Fecha Cuota"));
            panelTabla.AddAligned(posColumna2, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Cuota"));
            panelTabla.AddAligned(posColumna3, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Principal"));
            panelTabla.AddAligned(posColumna4, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "Intereses"));
            panelTabla.AddAligned(posColumna5, RepObj.rAlignCenter, puntoMedio, RepObj.rAlignCenter, new RepString(fPropBlanca, "IVA Intereses"));

            return panelTabla;

        }

        //Panel Cargos Objetados
        private StaticContainer fnCrearPanelCargos() 
        {
            StaticContainer panelCargos = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 8, RT.rPointFromMM(altoEncabezado));
            
            double pfPosX = 0;
            double pfPosY = 0;
            double pfAncho = 580;
            double pfAlto = 30;
            double posColumna1 = panelCargos.rWidth * 0.01;
            double posColumna2 = panelCargos.rWidth * 0.25;
            double posColumna3 = panelCargos.rWidth * 0.45;
            double posColumna4 = panelCargos.rWidth * 1.2;

            //borde superior
            panelCargos.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelCargos.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));

            fPropNormal.bBold = true;
            panelCargos.Add(5, -3, new RepString(fPropNormal, "Cargos Objetados"));
            panelCargos.AddAligned(posColumna1, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropNormal, "Fecha"));
            panelCargos.AddAligned(posColumna2, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropNormal, "Referencia"));
            panelCargos.AddAligned(posColumna3, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropNormal, "Descripción"));
            panelCargos.AddAligned(posColumna4, RepObj.rAlignLeft, 10, RepObj.rAlignBottom, new RepString(fPropNormal, "Monto"));
            fPropNormal.bBold = false;

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
            double posColumna2 = panelMov.rWidth * 0.25;
            double posColumna3 = panelMov.rWidth * 0.45;
            double posColumna4 = panelMov.rWidth * 0.60;
            double posColumna5 = panelMov.rWidth * 0.80;
            double posColumna6 = panelMov.rWidth * 1.0;
            double posColumna7 = panelMov.rWidth * 1.2;

            //borde superior
            panelMov.Add(pfPosX, pfPosY, new RepLine(penMediana, pfAncho, 0));
            //borde inferior
            panelMov.Add(pfPosX, pfAlto, new RepLine(penMediana, pfAncho, 0));

            fPropNormal.bBold = true;
            panelMov.Add(5, -3, new RepString(fPropNormal, "Movimientos Realizados"));
            panelMov.AddAligned(posColumna1, RepObj.rAlignLeft, 15, RepObj.rAlignBottom, new RepString(fPropNormal, "Fecha Movimiento"));
            panelMov.AddAligned(posColumna2, RepObj.rAlignLeft, 15, RepObj.rAlignBottom, new RepString(fPropNormal, "Transaccion"));
            panelMov.AddAligned(posColumna3, RepObj.rAlignLeft, 15, RepObj.rAlignBottom, new RepString(fPropNormal, "Monto"));
            panelMov.AddAligned(posColumna4, RepObj.rAlignLeft, 15, RepObj.rAlignBottom, new RepString(fPropNormal, "Principal"));
            panelMov.AddAligned(posColumna5 , RepObj.rAlignLeft,15, RepObj.rAlignBottom, new RepString(fPropNormal, "Intereses"));
            panelMov.AddAligned(posColumna6, RepObj.rAlignLeft, 15, RepObj.rAlignBottom, new RepString(fPropNormal, "Mora"));
            panelMov.AddAligned(posColumna7, RepObj.rAlignLeft, 15, RepObj.rAlignBottom, new RepString(fPropNormal, "IVA"));
            panelMov.Add(5, pfAlto + 9, new RepString(fPropNormal, "Saldo Final"));
            fPropNormal.bBold = true;

            return panelMov;

        }
        //Generamos grafica de prueba
        private MemoryStream fnGenerarGrafica()
        {
            double[] yValues = { 65.62, 75.54, 60.45, 34.73, 85.42 ,80,80};
            string[] xValues = { "Abono", "Cargo", "Comisión", "IVA", "Interés", "Mora","Saldo" };

            System.Windows.Forms.DataVisualization.Charting.Chart chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            System.Windows.Forms.DataVisualization.Charting.Chart chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart(); ;

            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            chart1.Legends.Add(legend2);
            chart1.Location = new System.Drawing.Point(400, 400);
            chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            chart1.Series.Add(series2);
            chart1.Size = new System.Drawing.Size(500, 500);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            // 
            // chart2
            // 
            chartArea1.Name = "ChartArea1";
            chart2.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart2.Legends.Add(legend1);
            chart2.Location = new System.Drawing.Point(12, 12);
            chart2.Name = "chart2";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart2.Series.Add(series1);
            chart2.Size = new System.Drawing.Size(300, 300);
            chart2.TabIndex = 1;
            chart2.Text = "chart2";

            chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);
            chart1.Series["Series1"].ChartType = SeriesChartType.Pie;
            
            chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new Font("Arial", 120f);
            chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = new Font("Arial", 120f);

            MemoryStream ms1 = new MemoryStream();
            byte[] byteImage = imageToByteArray(chart1);
            ms1 = new MemoryStream(byteImage);

            //chart1.SaveImage(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\clsPlantillaUnionProgreso\chart.png", System.Drawing.Imaging.ImageFormat.Png);
           // Image imagen = Image.FromFile(@"C:\Users\Marco.Santana\Documents\Visual Studio 2010\Projects\clsPlantillaUnionProgreso\chart.png");
            //imagen.Save(ms1, ImageFormat.Png);

            return ms1;

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
            panelDetalle.AddAligned(posColumna5 + 50, RepObj.rAlignCenter, 25, RepObj.rAlignBottom, new RepString(fPropNormal, "IMPORTE"));
            fPropNormal.bBold = false;
            return panelDetalle;
        }

        private StaticContainer fnContarCrearPie()
        {
            StaticContainer Pie = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPie));
            return Pie;
        }

        private StaticContainer fnCrearDetalle(List<DetalleUnionProg> detalles, System.Drawing.Color sColor)
        {
            StaticContainer areaDetalle = fnCrearPanelDetalle();
            DetalleUnionProg[] copiaDetalles = detalles.ToArray();
            DetalleUnionProg d;
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
                                foreach (ComprobanteImpuestosRetencionTUnionProg retencion in d.RetencionesT)
                                {
                                    switch (retencion.impuesto)
                                    {
                                        case ComprobanteImpuestosRetencionImpuestoTUnionProg.IVA: sDetalleTerceros.Append("IVA Ret: "); break;
                                        case ComprobanteImpuestosRetencionImpuestoTUnionProg.ISR: sDetalleTerceros.Append("ISR: "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionUnionProg.fnFormatoCurrency(retencion.importe.ToString()) + " ");
                                }
                            }

                            if (d.TrasladosT != null && d.TrasladosT.Count > 0)
                            {
                                foreach (ComprobanteImpuestosTrasladoTUnionProg traslado in d.TrasladosT)
                                {
                                    switch (traslado.impuesto)
                                    {
                                        case ComprobanteImpuestosTrasladoImpuestoTUnionProg.IVA: sDetalleTerceros.Append("IVA Tras "); break;
                                        case ComprobanteImpuestosTrasladoImpuestoTUnionProg.IEPS: sDetalleTerceros.Append("IEPS "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionUnionProg.fnFormatoPorcentaje(traslado.tasa.ToString()) + ": ");
                                    sDetalleTerceros.Append(TransformacionUnionProg.fnFormatoCurrency(traslado.importe.ToString()) + " ");
                                }
                            }
                        }

                        Col3 = fnAgregarMultilineaDetalle(areaDetalle, sDetalleTerceros.ToString(), fPropNormal, posColumna3, posRenglon, 42, true);
                    }

                }
                else
                {
                    
                    //Col1 = fnAgregarMultilinea(areaDetalle, d.noIdentificacion, fPropNormal, posColumna1, posRenglon + 27, 9, true);
               /*     areaDetalle.AddAligned(posColumna1 + 42, RepObj.rAlignRight, posRenglon + 27, RepObj.rAlignBottom, new RepString(fPropNormal, d.noIdentificacion));
                    areaDetalle.AddAligned(posColumna2  + 45, RepObj.rAlignRight, posRenglon + 27, RepObj.rAlignBottom, new RepString(fPropNormal, d.unidad));
                    areaDetalle.AddAligned(posColumna3 + 265, RepObj.rAlignRight, posRenglon + 27, RepObj.rAlignBottom, new RepString(fPropNormal, d.descripcion));
                    areaDetalle.AddAligned(posColumna4 + 45, RepObj.rAlignRight, posRenglon + 27, RepObj.rAlignBottom, new RepString(fPropNormal, d.cantidad));
                    areaDetalle.AddAligned(posColumna5 + 105, RepObj.rAlignRight, posRenglon + 27, RepObj.rAlignBottom, new RepString(fPropNormal, d.importe));
                */
                    renglonActual++;
                }

                renglonActual += fnObtenerRenglonMayor(Col1, Col2, Col3);
                maxConceptos -= 1;
                detalles.Remove(d);
            }

            return areaDetalle;
        }

        private StaticContainer fnContarPaginas(List<DetalleUnionProg> detalles)
        {
            StaticContainer areaDetalle = new StaticContainer(RT.rPointFromMM(anchoPagina) - margenPagina * 2, RT.rPointFromMM(altoPagina - altoEncabezado - altoPie) - margenPagina * 2);
            DetalleUnionProg[] copiaDetalles = detalles.ToArray();
            DetalleUnionProg d;
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
                                foreach (ComprobanteImpuestosRetencionTUnionProg retencion in d.RetencionesT)
                                {
                                    switch (retencion.impuesto)
                                    {
                                        case ComprobanteImpuestosRetencionImpuestoTUnionProg.IVA: sDetalleTerceros.Append("IVA Ret: "); break;
                                        case ComprobanteImpuestosRetencionImpuestoTUnionProg.ISR: sDetalleTerceros.Append("ISR: "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionUnionProg.fnFormatoCurrency(retencion.importe.ToString()) + " ");
                                }
                            }

                            if (d.TrasladosT != null && d.TrasladosT.Count > 0)
                            {
                                foreach (ComprobanteImpuestosTrasladoTUnionProg traslado in d.TrasladosT)
                                {
                                    switch (traslado.impuesto)
                                    {
                                        case ComprobanteImpuestosTrasladoImpuestoTUnionProg.IVA: sDetalleTerceros.Append("IVA Tras "); break;
                                        case ComprobanteImpuestosTrasladoImpuestoTUnionProg.IEPS: sDetalleTerceros.Append("IEPS "); break;
                                    }
                                    sDetalleTerceros.Append(TransformacionUnionProg.fnFormatoPorcentaje(traslado.tasa.ToString()) + ": ");
                                    sDetalleTerceros.Append(TransformacionUnionProg.fnFormatoCurrency(traslado.importe.ToString()) + " ");
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

        private List<DetalleUnionProg> fnObtenerDetalles()
        {
            XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(gxComprobante.NameTable);
            nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");

            List<DetalleUnionProg> detalles = new List<DetalleUnionProg>();

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
                detalles.Add(new DetalleUnionProg(navDetalles.Current, nsmComprobante));
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
            string sImagen = Settings1.Default["imagenes"].ToString() + "\\"+ sNombreLogo;
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
                pContenedor.AddCB(pdPosY,r);
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

            i = fnAgregarMultilineaCentrado(Encabezado, razonSocial, fPropNegrita, leftPadding + nPosXEncabezado , nPosYEncabezado, 100, true);
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

            i = fnAgregarMultilineaCentrado(Encabezado, "RFC: " + sRFC, fPropNormal, - leftPadding + nPosXEncabezado , nPosYEncabezado + tamRenglon * Renglon, 96, true);
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

             XmlDocument xdUnionProg = new XmlDocument();

            //xdUnionProg.LoadXml(gaAddenda[0].InnerXml);

            XPathNavigator navUnionProg = xdUnionProg.CreateNavigator();

            try { sRetIva = navUnionProg.SelectSingleNode("/UnionProg/Impuesto/@importe").Value; }
            catch { }

            List<ImpuestoUnionProg> impuestos = new List<ImpuestoUnionProg>();
            List<ImpuestoCompUnionProg> impuestosComp = new List<ImpuestoCompUnionProg>();

            try
            {
                subtotal = navPie.SelectSingleNode("/cfdi:Comprobante/@subTotal", nsmComprobante).Value;
                total = navPie.SelectSingleNode("/cfdi:Comprobante/@total", nsmComprobante).Value;
                timbre = navPie.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@selloSAT", nsmComprobante).Value;
                formaDePago = navPie.SelectSingleNode("/cfdi:Comprobante/@formaDePago", nsmComprobante).Value;
                sello = navPie.SelectSingleNode("/cfdi:Comprobante/@sello", nsmComprobante).Value;
                version = navPie.SelectSingleNode("/cfdi:Comprobante/@version", nsmComprobante).Value;
            }
            catch {} 
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
                    impuestos.Add(new ImpuestoUnionProg(navImpuestos.Current, nsmComprobante));
                }

                navImpuestos = navPie.Select("/cfdi:Comprobante/cfdi:Impuestos/cfdi:Retenciones/cfdi:Retencion", nsmComprobante);
                while (navImpuestos.MoveNext())
                {
                    impuestos.Add(new ImpuestoUnionProg(navImpuestos.Current, nsmComprobante));
                }
            }

            //Complementos impuestos locales
            try
            {
                XPathNodeIterator navComplemento = navPie.Select("/cfdi:Comprobante/cfdi:Complemento/implocal:ImpuestosLocales/implocal:TrasladosLocales", nsmComprobante);

                while (navComplemento.MoveNext())
                {
                    impuestosComp.Add(new ImpuestoCompUnionProg(navComplemento.Current, nsmComprobante));
                }

                navComplemento = navPie.Select("/cfdi:Comprobante/cfdi:Complemento/implocal:ImpuestosLocales/implocal:RetencionesLocales", nsmComprobante);

                while (navComplemento.MoveNext())
                {
                    impuestosComp.Add(new ImpuestoCompUnionProg(navComplemento.Current, nsmComprobante));
                }
            }
            catch { }

            try { sTotalAddenda = navUnionProg.SelectSingleNode("UnionProg/TotalTexto/@qty").Value; }
            catch { }
            try { sMetodoPagoAddenda = navUnionProg.SelectSingleNode("UnionProg/MetodoPago/@metodoDePago").Value; }
            catch { }
            try { sSalesOrder = navUnionProg.SelectSingleNode("UnionProg/OrdenVenta/@so").Value; }
            catch { }
            try { sDelivery = navUnionProg.SelectSingleNode("UnionProg/Envio/@del").Value; }
            catch { }

            double verPadding = Pie.rHeight * 0.02;
            double horPadding = Pie.rWidth * 0.01;
            double posPanelTotales = Pie.rWidth - 180;
            if (nNumPag == nTotPag)
            {
                Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding, RepObj.rAlignTop, new RepString(fPropNormal, "SUBTOTAL"));
                Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionUnionProg.fnFormatoCurrency(subtotal)));
            }
            double altoRenglon = fPropNormal.rSize + verPadding;
            int renglon = 1;
            string textoRenglon = string.Empty;
            if (nNumPag == nTotPag)
            {
                if (descuento != string.Empty)
                {
                    Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, "DESCUENTO"));
                    Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionUnionProg.fnFormatoCurrency(descuento)));
                    renglon++;
                }
                foreach (ImpuestoUnionProg i in impuestos)
                {
                    if (i.Nombre != "IEPS")
                    {
                        Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + TransformacionUnionProg.fnFormatoPorcentaje(i.Tasa)));
                        Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionUnionProg.fnFormatoCurrency(i.Importe)));
                        renglon++;
                    }
                }

                try
                {
                    //Agrega complemento impuestos locales 
                    foreach (ImpuestoCompUnionProg i in impuestosComp)
                    {
                        Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, i.Nombre + " " + TransformacionUnionProg.fnFormatoPorcentaje(i.Tasa)));
                        Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, verPadding + altoRenglon * renglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionUnionProg.fnFormatoCurrency(i.Importe)));
                        renglon++;
                    }
                }
                catch { }

                //Agregamos el separador y el total
                Pie.Add(posPanelTotales + horPadding, tamCodigo - altoRenglon - verPadding, new RepLine(new PenProp(PDF, grosorPen, ColorT), Pie.rWidth - posPanelTotales - horPadding * 2, 0));
                Pie.AddAligned(horPadding + posPanelTotales, RepObj.rAlignLeft, tamCodigo - altoRenglon, RepObj.rAlignTop, new RepString(fPropNormal, "TOTAL" + " (" + moneda + ")"));
                Pie.AddAligned(Pie.rWidth - horPadding, RepObj.rAlignRight, tamCodigo - altoRenglon, RepObj.rAlignTop, new RepString(fPropNormal, TransformacionUnionProg.fnFormatoCurrency(total)));
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
               // fnDatosBancarios(Pie, navUnionProg, horPadding, altoRenglon);
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
                    renglon = fnAgregarMultilinea(Pie, TransformacionUnionProg.fnConstruirCadenaTimbrado(gxComprobante.CreateNavigator().SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital", nsmComprobante).CreateNavigator(), "cadenaoriginal_3_2")
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
            trans.Load(XmlReader.Create(new StringReader(Settings1.Default[psNombreArchivoXSLT].ToString())));
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


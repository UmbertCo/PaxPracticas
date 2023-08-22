using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Threading;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml;
using System.Web;

/// <summary>
/// Summary description for rptNomina12
/// </summary>
public class rptNomina12 : DevExpress.XtraReports.UI.XtraReport
{
    private CalculatedField fxAntiguedadTit;
    private CalculatedField fxAutorizacion;
    private XRTableRow xrTableRow44;
    private XRTableCell xrTableCell100;
    private DetailReportBand DetailSubEmpleo;
    private DetailBand DetSubEmpleo;
    private CalculatedField fxPorcentaje;
    private CalculatedField fxSalarioBase;
    private DetailBand DetOtroPago;
    private XRTable xrTable8;
    private XRTableRow xrTableRow33;
    private XRTableCell xrTableCell7;
    private XRTableCell xrTableCell13;
    private XRTableCell xrTableCell14;
    private XRTableCell CellImporteOtroPago;
    private XRBarCode xrBarCodeQRCFDI;
    private DetailReportBand DetailSaldosAFavor;
    private DetailBand DetSaldosAFavor;
    private XRTable xrTable10;
    private XRTableRow xrTableRow36;
    private XRTableCell xrTableCell18;
    private XRTableCell CellSaldoAFavor;
    private XRTableCell xrTableCell21;
    private XRTableCell xrTableCell22;
    private XRTableCell xrTableCell33;
    private XRTableCell CellRemanenteSalFav;
    private CalculatedField fxSaldoAFavor;
    private XRTable tblTotalesLeft;
    private XRTableRow xrTableRow8;
    private XRTableCell xrTableCell15;
    private XRTableRow xrTableRow9;
    private XRTableCell CellCantidadLetra;
    private XRPageInfo xrPageInfo;
    private DetailBand Detail;
    private CalculatedField fxImporteOtroPago;
    private CalculatedField fxIMSS;
    private CalculatedField fxIngresoNoAcumulable;
    private CalculatedField fxReceptorDomicilio;
    private CalculatedField fxReferenciaExpedidoEn;
    private XRTableCell CellDescuentoTit;
    private CalculatedField fxNombreEmisor;
    private CalculatedField fxRemanenteSalFav;
    private XRTableRow xrTableRow4;
    private XRTableCell xrTableCell3;
    private CalculatedField fxAduanera;
    private CalculatedField fxTotalPercepcionesTit;
    private XRTableCell CellSubTotalValue;
    private CalculatedField fxImporteLocTra;
    private XRTableCell xrTableCell25;
    private XRPictureBox xrPictureBox1;
    private ReportHeaderBand ReportHeader;
    private SubBand SubBandHeader4;
    private XRTableRow xrTableRow15;
    private XRTableCell CellSelloEmisor;
    private CalculatedField fxEstadoTit;
    private CalculatedField fxJubIngresoAcumulable;
    private XRTableCell CellDescuentoValue;
    private CalculatedField fxReceptorNombreTit;
    private XRTableCell xrTableCell109;
    private XRTableCell xrTableCell97;
    private CalculatedField fxReceptorNombre;
    private BottomMarginBand BottomMargin;
    private CalculatedField fxSalarioDiario;
    private CalculatedField fxCuentaBancaria;
    private GroupHeaderBand GroupHeaderOtroPago;
    private CalculatedField fxSalarioBaseTit;
    private CalculatedField fxTotal;
    private CalculatedField fxFechaAutorizacion;
    private PageFooterBand PageFooter;
    private XRLabel xrLabel1;
    private XRLabel xrLabel8;
    private CalculatedField fxRiesgoTit;
    private CalculatedField fxMontoRecursoPropio;
    private CalculatedField fxNoIdentificacion;
    private XRTableCell xrTableCell96;
    private CalculatedField fxDepto;
    private CalculatedField fxImporteImpRetencionISR;
    private CalculatedField fxImporteImpRetencionIVA;
    private XRTableRow xrTableRow46;
    private XRTableCell CellTotalTit;
    private XRTableCell CellTotalValue;
    private TopMarginBand TopMargin;
    private CalculatedField fxPuestoTit;
    private XRTable xrTable15;
    private DetailReportBand DetailJubilacion;
    private DetailBand DetJubilacion;
    private GroupHeaderBand GroupHeaderJubilacion;
    private XRTableCell xrtblCellCadenaOriginal;
    private XRTable xrTable5;
    private CalculatedField fxIngresoAcumulable;
    private CalculatedField fxDescripLocTra;
    private CalculatedField fxMontoDiario;
    private CalculatedField fxEmisorDomicilio;
    private CalculatedField fxImpRetencionISR;
    private CalculatedField fxTotalPagado;
    private XRTableCell xrTableCell16;
    private CalculatedField fxHorasExtraImporte;
    private XRTableRow xrTableRow18;
    private CalculatedField fxCuentaBancariaTit;
    private CalculatedField fxTotalParcialidad;
    private XRTableRow xrTableRow61;
    private CalculatedField fxSalarioDiarioTit;
    private CalculatedField fxPuesto;
    private CalculatedField fxTotalUnaExhibicion;
    private CalculatedField fxUUID;
    private CalculatedField fxDomicilioExpedidoEn;
    private XRLine LineFooterConceptos;
    private CalculatedField fxJubIngresoNoAcumulable;
    private CalculatedField fxDeptoTit;
    private CalculatedField fxNumCtaPago;
    private XRTableCell xrTableCell110;
    private GroupFooterBand GroupFooterLine;
    private CalculatedField fxTipoJornada;
    private DetailReportBand DetailOtroPago;
    private XRTableRow xrTableRow60;
    private CalculatedField fxEstado;
    private XRTableCell CellSubTotalTit;
    private CalculatedField fxImporteConcepto;
    private CalculatedField fxTipoJornadaTit;
    private XRTable xrTable14;
    private XRTableRow xrTableRow43;
    private XRTableCell xrTableCell98;
    private XRTable tblTotalesRight;
    private XRTableRow RowSubtotal;
    private XRTableRow RowDescuento;
    private XRTableRow xrTableRow63;
    private XRTableRow xrTableRow62;
    private CalculatedField fxTotalPercepciones;
    private CalculatedField fxSubsidioCausado;
    private CalculatedField fxAntiguedad;
    private GroupHeaderBand GroupHeaderSepIndemnizacion;
    private CalculatedField fxRiesgo;
    private CalculatedField fxMontoRecursoPropioTit;
    private CalculatedField fxIMSSTit;
    private CalculatedField fxUltimoSueldo;
    private DetailReportBand DetailSepIndemnizacion;
    private DetailBand DetSepIndemnizacion;
    private CalculatedField fxSubTotal;
    private GroupFooterBand GroupFooterSubtotales;
    private CalculatedField fxCantidad;
    private CalculatedField fxImpRetencionIVA;
    private CalculatedField fxFecha;
    private CalculatedField fxValorUnitario;
    private CalculatedField fxFechaInicioRelLaboralTit;
    private CalculatedField fxFechaInicioRelLaboral;
    private CalculatedField fxPeriodoDePago;
    private CalculatedField fxFechaInicialPago;
    private CalculatedField fxFechaFinalPago;
    private CalculatedField fxFechaPago;
    private CalculatedField fxNumDiasPagados;
    private XRCrossBandLine CrossLineLeft;
    private XRCrossBandLine CrossLineRight;
    private SubBand SubBandHeader5;
    private CalculatedField fxMetodoPago;
    private XRTable xrTable3;
    private XRTableRow xrTableRow21;
    private XRTableCell xrTableCell1;
    private XRTableCell CellRegimen;
    private XRTableRow xrTableRow26;
    private XRTableCell xrTableCell45;
    private XRTableCell xrTableCell47;
    private XRTableRow xrTableRow30;
    private XRTableCell xrTableCell48;
    private XRTableCell xrTableCell49;
    private SubBand SubBandHeader6;
    private XRTable tblHeader;
    private XRTableRow xrTableRow57;
    private XRTableCell xrTableCell57;
    private XRTableCell CellTipoContrato;
    private XRTableRow xrTableRow64;
    private XRTableCell xrTableCell71;
    private XRTableCell xrTableCell72;
    private XRTableRow xrTableRow68;
    private XRTableCell xrTableCell85;
    private XRTableCell xrTableCell86;
    private XRTableRow xrTableRow72;
    private XRTableCell xrTableCell95;
    private XRTableCell CellPeriodicidad;
    private XRTable xrTable2;
    private XRTableRow xrTableRow2;
    private XRTableCell xrTableCell40;
    private XRTableCell xrTableCell46;
    private XRTableRow xrTableRow3;
    private XRTableCell xrTableCell51;
    private XRTableCell xrTableCell52;
    private XRTableRow xrTableRow11;
    private XRTableCell xrTableCell114;
    private XRTableCell xrTableCell120;
    private XRTableRow xrTableRow17;
    private XRTableCell xrTableCell127;
    private XRTableCell xrTableCell132;
    private XRTable xrTable22;
    private XRTableRow xrTableRow19;
    private XRTableCell xrTableCell139;
    private XRTableCell xrTableCell140;
    private XRTableRow xrTableRow23;
    private XRTableCell xrTableCell145;
    private XRTableCell xrTableCell146;
    private XRTableRow xrTableRow24;
    private XRTableCell xrTableCell151;
    private XRTableCell xrTableCell152;
    private XRTableRow xrTableRow25;
    private XRTableCell xrTableCell157;
    private XRTableCell xrTableCell158;
    private SubBand SubBandHeader7;
    private XRTable tblMetodoPago;
    private XRTableRow xrTableRow27;
    private XRTableCell CellMetodoDePago;
    private XRLine xrLine1;
    private CalculatedField fxRegistroPatronalTit;
    private CalculatedField fxRegistroPatronal;
    private XRTable xrTable4;
    private XRTableRow xrTableRow7;
    private XRTableCell xrTableCell28;
    private XRTableCell xrTableCell50;
    private XRTableRow xrTableRow14;
    private XRTableCell xrTableCell53;
    private XRTableCell CellEstado;
    private SubBand SubBandHeader3;
    private SubBand SubBandHeader1;
    private XRTable xrTable1;
    private XRTableRow xrTableRow10;
    private XRTableCell xrTableCell24;
    private SubBand SubBandHeader2;
    private XRTable xrTable25;
    private XRTableRow xrTableRow73;
    private XRTableCell xrTableCell76;
    private XRTableRow xrTableRow74;
    private XRTableCell xrTableCell77;
    private XRTableCell xrTableCell78;
    private XRTable xrTable23;
    private XRTableRow xrTableRow12;
    private XRTableCell xrTableCell63;
    private XRTableRow xrTableRow20;
    private XRTableCell xrTableCell64;
    private XRTableCell xrTableCell65;
    private XRTableRow xrTableRow31;
    private XRTableCell xrTableCell66;
    private XRTableCell CellRegimenFiscal;
    private XRLine xrLine2;
    private XRCrossBandLine xrCrossBandCenter;
    private XRTableCell CellReceptorRFC;
    private XRTableCell xrTableCell27;
    private XRTableCell CellEmisorRFC;
    private XRTableCell xrTableCell70;
    private CalculatedField fxEmisorCURPTit;
    private CalculatedField fxEmisorCURP;
    private XRTable xrTable26;
    private XRTableRow xrTableRow6;
    private XRTableCell xrTableCell87;
    private XRPictureBox xrpicLogo;
    private XRTableCell xrTableCell2;
    private XRTable xrTable27;
    private XRTableRow xrTableRow1;
    private XRTableCell xrTableCell74;
    private XRTableRow xrTableRow5;
    private XRTableCell CellUUID;
    private XRTableRow xrTableRow13;
    private XRTableCell xrTableCell12;
    private XRTableCell xrTableCell79;
    private XRTableRow xrTableRow47;
    private XRTableCell xrTableCell29;
    private XRTableCell xrTableCell84;
    private XRTableRow xrTableRow49;
    private XRTableCell xrTableCell73;
    private XRTableCell xrTableCell88;
    private XRTableRow xrTableRow48;
    private XRTableCell xrTableCell69;
    private XRTableCell xrTableCell89;
    private CalculatedField fxFechaTimbrado;
    private CalculatedField fxSerieFolio;
    private CalculatedField fxConfirmacion;
    private XRTableCell xrTableCell23;
    private XRTableRow xrTableRow28;
    private XRTableCell xrTableCell37;
    private XRTableCell xrTableCell34;
    private XRTableCell xrTableCell36;
    private XRTableCell xrTableCell35;
    private XRTableCell xrTableCell19;
    private XRTable xrTable13;
    private XRTableRow xrTableRow34;
    private XRTableCell xrTableCell5;
    private XRTableCell xrTableCell38;
    private XRTableCell xrTableCell42;
    private XRTableCell xrTableCell56;
    private XRTableCell xrTableCell58;
    private XRTable xrTable16;
    private XRTableRow xrTableRow37;
    private XRTableCell xrTableCell81;
    private XRTableCell xrTableCell82;
    private XRTableCell xrTableCell83;
    private XRTableCell xrTableCell94;
    private XRTableCell xrTableCell101;
    private XRTable xrTable6;
    private XRTableRow xrTableRow16;
    private XRTableCell xrTableCell4;
    private XRTableRow xrTableRow32;
    private XRTableCell xrTableCell6;
    private XRTableCell xrTableCell59;
    private XRTableCell xrTableCell60;
    private XRTableCell xrTableCell67;
    private XRTableCell xrTableCell80;
    private DetailReportBand DetailAcciones;
    private DetailBand DetAcciones;
    private GroupHeaderBand GroupHeaderAcciones;
    private XRTable xrTable11;
    private XRTableRow xrTableRow38;
    private XRTableCell xrTableCell20;
    private XRTableRow xrTableRow39;
    private XRTableCell xrTableCell31;
    private XRTableCell xrTableCell41;
    private XRTable xrTable12;
    private XRTableRow xrTableRow41;
    private XRTableCell xrTableCell39;
    private XRTableCell xrTableCell43;
    private CalculatedField fxPrecioAlOtorgarse;
    private CalculatedField fxValorMercado;
    private XRTable xrTable7;
    private XRTableRow xrTableRow22;
    private XRTableCell xrTableCell8;
    private XRTableRow xrTableRow29;
    private XRTableCell xrTableCell11;
    private XRTableCell xrTableCell9;
    private XRTableCell xrTableCell32;
    private XRTableCell xrTableCell10;
    private XRTable xrTable9;
    private XRTableRow xrTableRow35;
    private XRTableCell xrTableCell17;
    private XRTableCell xrTableCell44;
    private XRTableCell xrTableCell91;
    private XRTableCell xrTableCell55;
    private XRTableCell xrTableCell90;
    private XRTableCell xrTableCell93;
    private XRTableCell xrTableCell102;
    private XRTableCell xrTableCell92;
    private DevExpress.XtraReports.Parameters.Parameter pLinkSAT;
    private XRTableRow xrTableRow40;
    private XRTableCell xrTableCell26;
    private XRTableCell xrTableCell61;
    private CalculatedField fxTotalDeduccionesTit;
    private CalculatedField fxTotalDeducciones;
    private CalculatedField fxTotalOtrosPagosTit;
    private CalculatedField fxTotalOtrosPagos;
    private XRTableRow xrTableRow42;
    private XRTableCell xrTableCell105;
    private XRTableCell xrTableCell104;
    private XRTableCell xrTableCell75;
    private XRTableRow xrTableRow45;
    private XRTableCell xrTableCell107;
    private XRTableCell xrTableCell106;
    private XRTableCell xrTableCell108;
    private XRTableCell xrTableCell103;
    private CalculatedField fxImportePagado;
    private PageHeaderBand PageHeader;
    private XRLine xrLine3;
    private XRCrossBandLine CrossLineTipoPerce;
    private XRCrossBandLine CrossLineClavePerce;
    private XRCrossBandLine CrossLineConceptoPerce;
    private XRCrossBandLine CrossBandGravadoPerce;
    private XRCrossBandLine CrossBandExentoPerce;
    private XRCrossBandLine CrossLineTipoDedu;
    private XRCrossBandLine CrossBandClaveDedu;
    private XRCrossBandLine CrossLineConceptoDedu;
    private DetailReportBand DetailReportExtraInca;
    private DetailBand DetailExtraInca;
    private GroupHeaderBand GroupHeaderExtraInca;
    private XRTable tblHeaderHorasExtra;
    private XRTableRow xrTableRow56;
    private XRTableCell xrTableCell123;
    private XRTableRow xrTableRow59;
    private XRTableCell xrTableCell134;
    private XRTableCell xrTableCell135;
    private XRTableCell xrTableCell136;
    private XRTableCell xrTableCell137;
    private XRSubreport SubrHorasExtra;
    private XRCrossBandLine CrossLineHorasExtra;
    private XRCrossBandLine CrossLineTipoHora;
    private XRCrossBandLine CrossLineImporteHorasExtra;
    private XRTable tblHeaderIncapacidades;
    private XRTableRow xrTableRow58;
    private XRTableCell xrTableCell128;
    private XRTableRow xrTableRow65;
    private XRTableCell xrTableCell129;
    private XRTableCell xrTableCell130;
    private XRTableCell xrTableCell131;
    private XRSubreport SubrIncapacidades;
    private rptIncapacidades rptIncapacidades1;
    private XRCrossBandLine CrossLineDiasIncapacidades;
    private XRCrossBandLine CrossLineTipoInca;
    private XRCrossBandLine CrossLineDiasHoras;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private DevExpress.XtraReports.Parameters.Parameter pColor;
    private GroupFooterBand GroupFooterExtraInca;
    private XRLine LineExtraInca;
    private DetailReportBand DetailReportPerceDedu;
    private DetailBand DetailPerceDedu;
    private GroupFooterBand GroupFooterPerceDedu;
    private XRLine LinePerceDedu;
    private XRSubreport SubDeducciones;
    private XRSubreport SubPercepciones;
    private GroupHeaderBand GroupHeaderPerceDedu;
    private XRTable xrTable17;
    private XRTableRow xrTableRow50;
    private XRTableCell xrTableCell111;
    private XRTableRow xrTableRow51;
    private XRTableCell xrTableCell112;
    private XRTableCell xrTableCell115;
    private XRTableCell xrTableCell116;
    private XRTableCell xrTableCell117;
    private XRTable xrTable19;
    private XRTableRow xrTableRow54;
    private XRTableCell xrTableCell124;
    private XRTableRow xrTableRow55;
    private XRTableCell xrTableCell125;
    private XRTableCell xrTableCell126;
    private XRTable xrTable18;
    private XRTableRow xrTableRow52;
    private XRTableCell xrTableCell113;
    private XRTableRow xrTableRow53;
    private XRTableCell xrTableCell118;
    private XRTableCell xrTableCell119;
    private XRTableCell xrTableCell121;
    private XRTableCell xrTableCell122;

    private string sConexion = "conConsultas";

    public rptNomina12()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
        InitializeCulture();
    }

    public void InitializeCulture()
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX");
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            DevExpress.XtraPrinting.BarCode.QRCodeGenerator qrCodeGenerator1 = new DevExpress.XtraPrinting.BarCode.QRCodeGenerator();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rptNomina12));
            this.fxAntiguedadTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxAutorizacion = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableRow44 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell100 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetailSubEmpleo = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetSubEmpleo = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable9 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow35 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell91 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell55 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell90 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxPorcentaje = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxSalarioBase = new DevExpress.XtraReports.UI.CalculatedField();
            this.DetOtroPago = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow33 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellImporteOtroPago = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrBarCodeQRCFDI = new DevExpress.XtraReports.UI.XRBarCode();
            this.DetailSaldosAFavor = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetSaldosAFavor = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable10 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow36 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell93 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell102 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellSaldoAFavor = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellRemanenteSalFav = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell92 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxSaldoAFavor = new DevExpress.XtraReports.UI.CalculatedField();
            this.tblTotalesLeft = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellCantidadLetra = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.fxImporteOtroPago = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxIMSS = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxIngresoNoAcumulable = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxReceptorDomicilio = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxReferenciaExpedidoEn = new DevExpress.XtraReports.UI.CalculatedField();
            this.CellDescuentoTit = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxNombreEmisor = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxRemanenteSalFav = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxAduanera = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxTotalPercepcionesTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.CellSubTotalValue = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxImporteLocTra = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrTable26 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell87 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrpicLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable27 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell74 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellUUID = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell79 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow47 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell84 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow49 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell73 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell88 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow48 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell69 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell89 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBandHeader1 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBandHeader2 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable25 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow73 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell76 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow74 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell77 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellReceptorRFC = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell78 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable23 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell63 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell64 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellEmisorRFC = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell70 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell65 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow31 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell66 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellRegimenFiscal = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBandHeader3 = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.SubBandHeader4 = new DevExpress.XtraReports.UI.SubBand();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell50 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell53 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellEstado = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow21 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellRegimen = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow26 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell47 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow30 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell48 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell49 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBandHeader5 = new DevExpress.XtraReports.UI.SubBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.SubBandHeader6 = new DevExpress.XtraReports.UI.SubBand();
            this.tblHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow57 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell57 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellTipoContrato = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow64 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell71 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell72 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow68 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell85 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell86 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow72 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell95 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellPeriodicidad = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell46 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell114 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell120 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell127 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell132 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable22 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell139 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell140 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow23 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell145 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell146 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow24 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell151 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell152 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow25 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell157 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell158 = new DevExpress.XtraReports.UI.XRTableCell();
            this.SubBandHeader7 = new DevExpress.XtraReports.UI.SubBand();
            this.tblMetodoPago = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow27 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellMetodoDePago = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellSelloEmisor = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxEstadoTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxJubIngresoAcumulable = new DevExpress.XtraReports.UI.CalculatedField();
            this.CellDescuentoValue = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxReceptorNombreTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableCell109 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell97 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxReceptorNombre = new DevExpress.XtraReports.UI.CalculatedField();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.fxSalarioDiario = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxCuentaBancaria = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupHeaderOtroPago = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow29 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxSalarioBaseTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxTotal = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxFechaAutorizacion = new DevExpress.XtraReports.UI.CalculatedField();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.fxRiesgoTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxMontoRecursoPropio = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxNoIdentificacion = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableCell96 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxDepto = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImporteImpRetencionISR = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImporteImpRetencionIVA = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableRow46 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellTotalTit = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellTotalValue = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.fxPuestoTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTable15 = new DevExpress.XtraReports.UI.XRTable();
            this.DetailJubilacion = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetJubilacion = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable16 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow37 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell81 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell82 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell83 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell94 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell101 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeaderJubilacion = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow16 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow32 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell59 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell60 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell67 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell80 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrtblCellCadenaOriginal = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow28 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxIngresoAcumulable = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxDescripLocTra = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxMontoDiario = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxEmisorDomicilio = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImpRetencionISR = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxTotalPagado = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxHorasExtraImporte = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
            this.fxCuentaBancariaTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxTotalParcialidad = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableRow61 = new DevExpress.XtraReports.UI.XRTableRow();
            this.fxSalarioDiarioTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxPuesto = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxTotalUnaExhibicion = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxUUID = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxDomicilioExpedidoEn = new DevExpress.XtraReports.UI.CalculatedField();
            this.LineFooterConceptos = new DevExpress.XtraReports.UI.XRLine();
            this.fxJubIngresoNoAcumulable = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxDeptoTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxNumCtaPago = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableCell110 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupFooterLine = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.fxTipoJornada = new DevExpress.XtraReports.UI.CalculatedField();
            this.DetailOtroPago = new DevExpress.XtraReports.UI.DetailReportBand();
            this.xrTableRow60 = new DevExpress.XtraReports.UI.XRTableRow();
            this.fxEstado = new DevExpress.XtraReports.UI.CalculatedField();
            this.CellSubTotalTit = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxImporteConcepto = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxTipoJornadaTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTable14 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow43 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell98 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tblTotalesRight = new DevExpress.XtraReports.UI.XRTable();
            this.RowSubtotal = new DevExpress.XtraReports.UI.XRTableRow();
            this.RowDescuento = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableRow40 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell61 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow63 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableRow62 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableRow42 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell105 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell104 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell75 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow45 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell107 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell106 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell108 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell103 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxTotalPercepciones = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxSubsidioCausado = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxAntiguedad = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupHeaderSepIndemnizacion = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.fxRiesgo = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxMontoRecursoPropioTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxIMSSTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxUltimoSueldo = new DevExpress.XtraReports.UI.CalculatedField();
            this.DetailSepIndemnizacion = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetSepIndemnizacion = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable13 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow34 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell56 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell58 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxSubTotal = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupFooterSubtotales = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.fxCantidad = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImpRetencionIVA = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxFecha = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxValorUnitario = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxFechaInicioRelLaboralTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxFechaInicioRelLaboral = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxPeriodoDePago = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxFechaInicialPago = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxFechaFinalPago = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxFechaPago = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxNumDiasPagados = new DevExpress.XtraReports.UI.CalculatedField();
            this.CrossLineLeft = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossLineRight = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.fxMetodoPago = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxRegistroPatronalTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxRegistroPatronal = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrCrossBandCenter = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.fxEmisorCURPTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxEmisorCURP = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxFechaTimbrado = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxSerieFolio = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxConfirmacion = new DevExpress.XtraReports.UI.CalculatedField();
            this.DetailAcciones = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetAcciones = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable12 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow41 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeaderAcciones = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable11 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow38 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow39 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxPrecioAlOtorgarse = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxValorMercado = new DevExpress.XtraReports.UI.CalculatedField();
            this.pLinkSAT = new DevExpress.XtraReports.Parameters.Parameter();
            this.fxTotalDeduccionesTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxTotalDeducciones = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxTotalOtrosPagosTit = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxTotalOtrosPagos = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImportePagado = new DevExpress.XtraReports.UI.CalculatedField();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.CrossLineTipoPerce = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.GroupFooterPerceDedu = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.LinePerceDedu = new DevExpress.XtraReports.UI.XRLine();
            this.GroupHeaderPerceDedu = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable17 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow50 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell111 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow51 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell112 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell115 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell116 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell117 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable19 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow54 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell124 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow55 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell125 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell126 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable18 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow52 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell113 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow53 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell118 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell119 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell121 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell122 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CrossLineClavePerce = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossLineConceptoPerce = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossBandGravadoPerce = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossBandExentoPerce = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossLineTipoDedu = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossBandClaveDedu = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossLineConceptoDedu = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.DetailReportExtraInca = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetailExtraInca = new DevExpress.XtraReports.UI.DetailBand();
            this.SubrIncapacidades = new DevExpress.XtraReports.UI.XRSubreport();
            this.SubrHorasExtra = new DevExpress.XtraReports.UI.XRSubreport();
            this.GroupHeaderExtraInca = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.tblHeaderIncapacidades = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow58 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell128 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow65 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell129 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell130 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell131 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tblHeaderHorasExtra = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow56 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell123 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow59 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell134 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell135 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell136 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell137 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupFooterExtraInca = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.LineExtraInca = new DevExpress.XtraReports.UI.XRLine();
            this.CrossLineHorasExtra = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossLineTipoHora = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossLineImporteHorasExtra = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossLineDiasIncapacidades = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossLineTipoInca = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.CrossLineDiasHoras = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.pColor = new DevExpress.XtraReports.Parameters.Parameter();
            this.DetailReportPerceDedu = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetailPerceDedu = new DevExpress.XtraReports.UI.DetailBand();
            this.SubDeducciones = new DevExpress.XtraReports.UI.XRSubreport();
            this.SubPercepciones = new DevExpress.XtraReports.UI.XRSubreport();
            this.rptIncapacidades1 = new rptIncapacidades();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTotalesLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable26)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable27)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable25)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable23)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblMetodoPago)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTotalesRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable19)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable18)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblHeaderIncapacidades)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblHeaderHorasExtra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptIncapacidades1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // fxAntiguedadTit
            // 
            this.fxAntiguedadTit.DataMember = "Receptor";
            this.fxAntiguedadTit.Expression = "Iif(IsNullOrEmpty([Antigüedad]), \'\' ,\'Antigüedad:\' )";
            this.fxAntiguedadTit.Name = "fxAntiguedadTit";
            // 
            // fxAutorizacion
            // 
            this.fxAutorizacion.DataMember = "Complemento.Complemento_Donatarias";
            this.fxAutorizacion.Expression = "Concat(\'No. Aprobación: \',[noAutorizacion] )";
            this.fxAutorizacion.Name = "fxAutorizacion";
            // 
            // xrTableRow44
            // 
            this.xrTableRow44.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell100});
            this.xrTableRow44.Name = "xrTableRow44";
            this.xrTableRow44.Weight = 0.10317464411844296D;
            // 
            // xrTableCell100
            // 
            this.xrTableCell100.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell100.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell100.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell100.Name = "xrTableCell100";
            this.xrTableCell100.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell100.StylePriority.UseBorderColor = false;
            this.xrTableCell100.StylePriority.UseBorders = false;
            this.xrTableCell100.StylePriority.UseFont = false;
            this.xrTableCell100.StylePriority.UsePadding = false;
            this.xrTableCell100.Text = "Sello digital del Emisor:";
            this.xrTableCell100.Weight = 7.8599995044549695D;
            // 
            // DetailSubEmpleo
            // 
            this.DetailSubEmpleo.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetSubEmpleo});
            this.DetailSubEmpleo.DataMember = "OtroPago.OtroPago_SubsidioAlEmpleo";
            this.DetailSubEmpleo.Level = 0;
            this.DetailSubEmpleo.Name = "DetailSubEmpleo";
            this.DetailSubEmpleo.ReportPrintOptions.PrintOnEmptyDataSource = false;
            // 
            // DetSubEmpleo
            // 
            this.DetSubEmpleo.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable9});
            this.DetSubEmpleo.HeightF = 15F;
            this.DetSubEmpleo.Name = "DetSubEmpleo";
            // 
            // xrTable9
            // 
            this.xrTable9.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable9.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable9.Name = "xrTable9";
            this.xrTable9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTable9.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow35});
            this.xrTable9.SizeF = new System.Drawing.SizeF(630.4F, 15F);
            this.xrTable9.StylePriority.UseBorders = false;
            this.xrTable9.StylePriority.UseFont = false;
            this.xrTable9.StylePriority.UsePadding = false;
            this.xrTable9.Tag = "WithColor";
            // 
            // xrTableRow35
            // 
            this.xrTableRow35.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17,
            this.xrTableCell44,
            this.xrTableCell91,
            this.xrTableCell55,
            this.xrTableCell90});
            this.xrTableRow35.Name = "xrTableRow35";
            this.xrTableRow35.Weight = 11.499971850768331D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell17.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell17.StylePriority.UseBorders = false;
            this.xrTableCell17.StylePriority.UseFont = false;
            this.xrTableCell17.StylePriority.UsePadding = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell17.Weight = 0.28125380175809489D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell44.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell44.StylePriority.UseBorders = false;
            this.xrTableCell44.StylePriority.UseFont = false;
            this.xrTableCell44.StylePriority.UsePadding = false;
            this.xrTableCell44.StylePriority.UseTextAlignment = false;
            this.xrTableCell44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell44.Weight = 0.39967653023923055D;
            // 
            // xrTableCell91
            // 
            this.xrTableCell91.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell91.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell91.Name = "xrTableCell91";
            this.xrTableCell91.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 5, 1, 1, 100F);
            this.xrTableCell91.StylePriority.UseBorders = false;
            this.xrTableCell91.StylePriority.UseFont = false;
            this.xrTableCell91.StylePriority.UsePadding = false;
            this.xrTableCell91.StylePriority.UseTextAlignment = false;
            this.xrTableCell91.Text = "Subsidio Causado:";
            this.xrTableCell91.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell91.Weight = 1.0732045632416536D;
            // 
            // xrTableCell55
            // 
            this.xrTableCell55.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell55.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OtroPago.OtroPago_SubsidioAlEmpleo.fxSubsidioCausado", "{0:$ #,###0.00}")});
            this.xrTableCell55.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell55.Multiline = true;
            this.xrTableCell55.Name = "xrTableCell55";
            this.xrTableCell55.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell55.StylePriority.UseBorders = false;
            this.xrTableCell55.StylePriority.UseFont = false;
            this.xrTableCell55.StylePriority.UsePadding = false;
            this.xrTableCell55.StylePriority.UseTextAlignment = false;
            this.xrTableCell55.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell55.Weight = 1.2508389952028522D;
            // 
            // xrTableCell90
            // 
            this.xrTableCell90.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell90.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell90.Name = "xrTableCell90";
            this.xrTableCell90.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell90.StylePriority.UseBorders = false;
            this.xrTableCell90.StylePriority.UseFont = false;
            this.xrTableCell90.StylePriority.UsePadding = false;
            this.xrTableCell90.StylePriority.UseTextAlignment = false;
            this.xrTableCell90.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell90.Weight = 0.50151983659283683D;
            // 
            // fxPorcentaje
            // 
            this.fxPorcentaje.DataMember = "SubContratacion";
            this.fxPorcentaje.Expression = "ToDecimal([PorcentajeTiempo])/100";
            this.fxPorcentaje.FieldType = DevExpress.XtraReports.UI.FieldType.Decimal;
            this.fxPorcentaje.Name = "fxPorcentaje";
            // 
            // fxSalarioBase
            // 
            this.fxSalarioBase.DataMember = "Receptor";
            this.fxSalarioBase.Expression = "Iif(IsNullOrEmpty([SalarioBaseCotApor]), \'\' ,[SalarioBaseCotApor] )";
            this.fxSalarioBase.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxSalarioBase.Name = "fxSalarioBase";
            // 
            // DetOtroPago
            // 
            this.DetOtroPago.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable8});
            this.DetOtroPago.HeightF = 15.00003F;
            this.DetOtroPago.Name = "DetOtroPago";
            // 
            // xrTable8
            // 
            this.xrTable8.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable8.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable8.Name = "xrTable8";
            this.xrTable8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTable8.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow33});
            this.xrTable8.SizeF = new System.Drawing.SizeF(630.4F, 15F);
            this.xrTable8.StylePriority.UseBorders = false;
            this.xrTable8.StylePriority.UseFont = false;
            this.xrTable8.StylePriority.UsePadding = false;
            this.xrTable8.Tag = "WithColor";
            // 
            // xrTableRow33
            // 
            this.xrTableRow33.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell13,
            this.xrTableCell14,
            this.CellImporteOtroPago});
            this.xrTableRow33.Name = "xrTableRow33";
            this.xrTableRow33.Weight = 11.499971850768331D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OtroPago.TipoOtroPago")});
            this.xrTableCell7.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UsePadding = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell7.Weight = 0.28125380175809489D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OtroPago.Clave")});
            this.xrTableCell13.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell13.StylePriority.UseBorders = false;
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UsePadding = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell13.Weight = 0.39967653023923055D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OtroPago.Concepto")});
            this.xrTableCell14.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Multiline = true;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UsePadding = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell14.Weight = 2.324043332571291D;
            // 
            // CellImporteOtroPago
            // 
            this.CellImporteOtroPago.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.CellImporteOtroPago.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OtroPago.fxImporteOtroPago", "{0:$ #,###0.00}")});
            this.CellImporteOtroPago.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellImporteOtroPago.Name = "CellImporteOtroPago";
            this.CellImporteOtroPago.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellImporteOtroPago.StylePriority.UseBorders = false;
            this.CellImporteOtroPago.StylePriority.UseFont = false;
            this.CellImporteOtroPago.StylePriority.UsePadding = false;
            this.CellImporteOtroPago.StylePriority.UseTextAlignment = false;
            this.CellImporteOtroPago.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.CellImporteOtroPago.Weight = 0.5015200624660513D;
            // 
            // xrBarCodeQRCFDI
            // 
            this.xrBarCodeQRCFDI.Alignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrBarCodeQRCFDI.AutoModule = true;
            this.xrBarCodeQRCFDI.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCodeQRCFDI.BorderWidth = 1F;
            this.xrBarCodeQRCFDI.Font = new System.Drawing.Font("Helvetica-Normal", 6F);
            this.xrBarCodeQRCFDI.LocationFloat = new DevExpress.Utils.PointFloat(2F, 2F);
            this.xrBarCodeQRCFDI.Module = 3F;
            this.xrBarCodeQRCFDI.Name = "xrBarCodeQRCFDI";
            this.xrBarCodeQRCFDI.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 4, 4, 100F);
            this.xrBarCodeQRCFDI.ShowText = false;
            this.xrBarCodeQRCFDI.SizeF = new System.Drawing.SizeF(135F, 124.25F);
            this.xrBarCodeQRCFDI.StylePriority.UseBorders = false;
            this.xrBarCodeQRCFDI.StylePriority.UseBorderWidth = false;
            this.xrBarCodeQRCFDI.StylePriority.UseFont = false;
            this.xrBarCodeQRCFDI.StylePriority.UsePadding = false;
            this.xrBarCodeQRCFDI.StylePriority.UseTextAlignment = false;
            qrCodeGenerator1.CompactionMode = DevExpress.XtraPrinting.BarCode.QRCodeCompactionMode.Byte;
            qrCodeGenerator1.ErrorCorrectionLevel = DevExpress.XtraPrinting.BarCode.QRCodeErrorCorrectionLevel.Q;
            qrCodeGenerator1.Version = DevExpress.XtraPrinting.BarCode.QRCodeVersion.Version4;
            this.xrBarCodeQRCFDI.Symbology = qrCodeGenerator1;
            this.xrBarCodeQRCFDI.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrBarCodeQRCFDI_BeforePrint);
            // 
            // DetailSaldosAFavor
            // 
            this.DetailSaldosAFavor.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetSaldosAFavor});
            this.DetailSaldosAFavor.DataMember = "OtroPago.OtroPago_CompensacionSaldosAFavor";
            this.DetailSaldosAFavor.Level = 1;
            this.DetailSaldosAFavor.Name = "DetailSaldosAFavor";
            this.DetailSaldosAFavor.ReportPrintOptions.PrintOnEmptyDataSource = false;
            // 
            // DetSaldosAFavor
            // 
            this.DetSaldosAFavor.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable10});
            this.DetSaldosAFavor.HeightF = 15F;
            this.DetSaldosAFavor.Name = "DetSaldosAFavor";
            // 
            // xrTable10
            // 
            this.xrTable10.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable10.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable10.Name = "xrTable10";
            this.xrTable10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTable10.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow36});
            this.xrTable10.SizeF = new System.Drawing.SizeF(630.4F, 14.99999F);
            this.xrTable10.StylePriority.UseFont = false;
            this.xrTable10.StylePriority.UsePadding = false;
            this.xrTable10.Tag = "WithColor";
            // 
            // xrTableRow36
            // 
            this.xrTableRow36.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell93,
            this.xrTableCell102,
            this.xrTableCell18,
            this.CellSaldoAFavor,
            this.xrTableCell21,
            this.xrTableCell22,
            this.xrTableCell33,
            this.CellRemanenteSalFav,
            this.xrTableCell92});
            this.xrTableRow36.Name = "xrTableRow36";
            this.xrTableRow36.Weight = 0.6D;
            // 
            // xrTableCell93
            // 
            this.xrTableCell93.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell93.Font = new System.Drawing.Font("Helvetica-Normal", 7F);
            this.xrTableCell93.Name = "xrTableCell93";
            this.xrTableCell93.StylePriority.UseBorders = false;
            this.xrTableCell93.StylePriority.UseFont = false;
            this.xrTableCell93.StylePriority.UseTextAlignment = false;
            this.xrTableCell93.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell93.Weight = 0.43837181015753535D;
            // 
            // xrTableCell102
            // 
            this.xrTableCell102.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell102.Font = new System.Drawing.Font("Helvetica-Normal", 7F);
            this.xrTableCell102.Name = "xrTableCell102";
            this.xrTableCell102.StylePriority.UseBorders = false;
            this.xrTableCell102.StylePriority.UseFont = false;
            this.xrTableCell102.StylePriority.UseTextAlignment = false;
            this.xrTableCell102.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell102.Weight = 0.62294958017647772D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell18.Font = new System.Drawing.Font("Helvetica-Normal", 7F);
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Padding = new DevExpress.XtraPrinting.PaddingInfo(15, 5, 1, 1, 100F);
            this.xrTableCell18.StylePriority.UseBorders = false;
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.StylePriority.UsePadding = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "Saldo a Favor:";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell18.Weight = 0.7464409038232116D;
            // 
            // CellSaldoAFavor
            // 
            this.CellSaldoAFavor.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.CellSaldoAFavor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OtroPago.OtroPago_CompensacionSaldosAFavor.fxSaldoAFavor", "{0:$ #,###0.00}")});
            this.CellSaldoAFavor.Name = "CellSaldoAFavor";
            this.CellSaldoAFavor.StylePriority.UseBorders = false;
            this.CellSaldoAFavor.StylePriority.UseTextAlignment = false;
            this.CellSaldoAFavor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.CellSaldoAFavor.Weight = 0.68331723431875224D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell21.Font = new System.Drawing.Font("Helvetica-Normal", 7F);
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseBorders = false;
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "Año:";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell21.Weight = 0.276786883916769D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OtroPago.OtroPago_CompensacionSaldosAFavor.Año")});
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.StylePriority.UseBorders = false;
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell22.Weight = 0.35463286116115683D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell33.Font = new System.Drawing.Font("Helvetica-Normal", 7F);
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.StylePriority.UseBorders = false;
            this.xrTableCell33.StylePriority.UseFont = false;
            this.xrTableCell33.StylePriority.UseTextAlignment = false;
            this.xrTableCell33.Text = "Remanente:";
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell33.Weight = 0.56222229164352222D;
            // 
            // CellRemanenteSalFav
            // 
            this.CellRemanenteSalFav.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.CellRemanenteSalFav.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "OtroPago.OtroPago_CompensacionSaldosAFavor.fxRemanenteSalFav", "{0:$ #,###0.00}")});
            this.CellRemanenteSalFav.Name = "CellRemanenteSalFav";
            this.CellRemanenteSalFav.StylePriority.UseBorders = false;
            this.CellRemanenteSalFav.StylePriority.UseTextAlignment = false;
            this.CellRemanenteSalFav.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.CellRemanenteSalFav.Weight = 0.987397600499733D;
            // 
            // xrTableCell92
            // 
            this.xrTableCell92.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell92.Name = "xrTableCell92";
            this.xrTableCell92.StylePriority.UseBorders = false;
            this.xrTableCell92.StylePriority.UseTextAlignment = false;
            this.xrTableCell92.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell92.Weight = 0.78168610515235359D;
            // 
            // fxSaldoAFavor
            // 
            this.fxSaldoAFavor.DataMember = "OtroPago.OtroPago_CompensacionSaldosAFavor";
            this.fxSaldoAFavor.Expression = "[SaldoAFavor]";
            this.fxSaldoAFavor.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxSaldoAFavor.Name = "fxSaldoAFavor";
            // 
            // tblTotalesLeft
            // 
            this.tblTotalesLeft.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tblTotalesLeft.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblTotalesLeft.LocationFloat = new DevExpress.Utils.PointFloat(1.651407F, 1.999982F);
            this.tblTotalesLeft.Name = "tblTotalesLeft";
            this.tblTotalesLeft.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 15, 4, 0, 100F);
            this.tblTotalesLeft.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8,
            this.xrTableRow9});
            this.tblTotalesLeft.SizeF = new System.Drawing.SizeF(401.6514F, 31.25934F);
            this.tblTotalesLeft.StylePriority.UseBorderColor = false;
            this.tblTotalesLeft.StylePriority.UseBorders = false;
            this.tblTotalesLeft.StylePriority.UseFont = false;
            this.tblTotalesLeft.StylePriority.UsePadding = false;
            this.tblTotalesLeft.StylePriority.UseTextAlignment = false;
            this.tblTotalesLeft.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15});
            this.xrTableRow8.Font = new System.Drawing.Font("Helvetica-Normal", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 4, 0, 100F);
            this.xrTableRow8.StylePriority.UseFont = false;
            this.xrTableRow8.StylePriority.UsePadding = false;
            this.xrTableRow8.Weight = 0.89049377288036391D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.CanGrow = false;
            this.xrTableCell15.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UsePadding = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "Total con letra:";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell15.Weight = 3.6999999999999993D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellCantidadLetra});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableRow9.StylePriority.UsePadding = false;
            this.xrTableRow9.Weight = 0.89049377288036391D;
            // 
            // CellCantidadLetra
            // 
            this.CellCantidadLetra.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.total")});
            this.CellCantidadLetra.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellCantidadLetra.Name = "CellCantidadLetra";
            this.CellCantidadLetra.StylePriority.UseFont = false;
            this.CellCantidadLetra.StylePriority.UseTextAlignment = false;
            this.CellCantidadLetra.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellCantidadLetra.Weight = 3.6999999999999993D;
            this.CellCantidadLetra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellCantidadLetra_BeforePrint);
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPageInfo.Font = new System.Drawing.Font("Helvetica-Normal", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo.Format = "{0} de {1}";
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(706F, 4.999987F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(77.49988F, 17.91668F);
            this.xrPageInfo.StylePriority.UseBorders = false;
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UsePadding = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.Visible = false;
            // 
            // fxImporteOtroPago
            // 
            this.fxImporteOtroPago.DataMember = "OtroPago";
            this.fxImporteOtroPago.Expression = "[Importe]";
            this.fxImporteOtroPago.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteOtroPago.Name = "fxImporteOtroPago";
            // 
            // fxIMSS
            // 
            this.fxIMSS.DataMember = "Receptor";
            this.fxIMSS.Expression = "Iif(IsNullOrEmpty([NumSeguridadSocial]),\'\' ,[NumSeguridadSocial] )";
            this.fxIMSS.Name = "fxIMSS";
            // 
            // fxIngresoNoAcumulable
            // 
            this.fxIngresoNoAcumulable.DataMember = "SeparacionIndemnizacion";
            this.fxIngresoNoAcumulable.Expression = "[IngresoNoAcumulable]";
            this.fxIngresoNoAcumulable.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxIngresoNoAcumulable.Name = "fxIngresoNoAcumulable";
            // 
            // fxReceptorDomicilio
            // 
            this.fxReceptorDomicilio.DataMember = "Receptor.Receptor_Domicilio";
            this.fxReceptorDomicilio.Expression = resources.GetString("fxReceptorDomicilio.Expression");
            this.fxReceptorDomicilio.Name = "fxReceptorDomicilio";
            // 
            // fxReferenciaExpedidoEn
            // 
            this.fxReferenciaExpedidoEn.DataMember = "Emisor.Emisor_DomicilioFiscal";
            this.fxReferenciaExpedidoEn.Expression = "Iif(IsNullOrEmpty([referencia]),\'\',Concat(\'Referencia: \',[referencia]))";
            this.fxReferenciaExpedidoEn.Name = "fxReferenciaExpedidoEn";
            // 
            // CellDescuentoTit
            // 
            this.CellDescuentoTit.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellDescuentoTit.CanShrink = true;
            this.CellDescuentoTit.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Nomina.fxTotalOtrosPagosTit")});
            this.CellDescuentoTit.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellDescuentoTit.Name = "CellDescuentoTit";
            this.CellDescuentoTit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.CellDescuentoTit.StylePriority.UseBorders = false;
            this.CellDescuentoTit.StylePriority.UseFont = false;
            this.CellDescuentoTit.StylePriority.UsePadding = false;
            this.CellDescuentoTit.StylePriority.UseTextAlignment = false;
            this.CellDescuentoTit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellDescuentoTit.Weight = 1.3D;
            // 
            // fxNombreEmisor
            // 
            this.fxNombreEmisor.DataMember = "EmisorComprobante";
            this.fxNombreEmisor.Expression = "Iif(IsNullOrEmpty([nombre]), \'\' ,[nombre] )";
            this.fxNombreEmisor.Name = "fxNombreEmisor";
            // 
            // fxRemanenteSalFav
            // 
            this.fxRemanenteSalFav.DataMember = "OtroPago.OtroPago_CompensacionSaldosAFavor";
            this.fxRemanenteSalFav.Expression = "[RemanenteSalFav]";
            this.fxRemanenteSalFav.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxRemanenteSalFav.Name = "fxRemanenteSalFav";
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 11.499971850768331D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BackColor = System.Drawing.Color.Black;
            this.xrTableCell3.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.ForeColor = System.Drawing.Color.White;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell3.StylePriority.UseBackColor = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseForeColor = false;
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Separación - Indemnización";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 4.3686668141489893D;
            // 
            // fxAduanera
            // 
            this.fxAduanera.DataMember = "Conceptos.Conceptos_Concepto.Concepto_InformacionAduanera";
            this.fxAduanera.Expression = "Concat(Iif(IsNullOrEmpty([aduana]),\'\', Concat(\'Nombre Aduana: \',[aduana],\'; \' ))\r" +
    "\n,\'No.Documento: \',[numero],\'; \' ,\'Fecha: \',[fecha]\r\n )";
            this.fxAduanera.FieldType = DevExpress.XtraReports.UI.FieldType.String;
            this.fxAduanera.Name = "fxAduanera";
            // 
            // fxTotalPercepcionesTit
            // 
            this.fxTotalPercepcionesTit.DataMember = "Nomina";
            this.fxTotalPercepcionesTit.Expression = "Iif(IsNullOrEmpty([TotalPercepciones]),\'\' ,\'Total Percepciones\' )";
            this.fxTotalPercepcionesTit.Name = "fxTotalPercepcionesTit";
            // 
            // CellSubTotalValue
            // 
            this.CellSubTotalValue.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellSubTotalValue.CanShrink = true;
            this.CellSubTotalValue.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Nomina.fxTotalPercepciones", "{0:$ #,###0.00}")});
            this.CellSubTotalValue.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellSubTotalValue.Name = "CellSubTotalValue";
            this.CellSubTotalValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 4, 0, 100F);
            this.CellSubTotalValue.StylePriority.UseBorders = false;
            this.CellSubTotalValue.StylePriority.UseFont = false;
            this.CellSubTotalValue.StylePriority.UsePadding = false;
            this.CellSubTotalValue.StylePriority.UseTextAlignment = false;
            this.CellSubTotalValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.CellSubTotalValue.Weight = 1.12D;
            // 
            // fxImporteLocTra
            // 
            this.fxImporteLocTra.DataMember = "ImpuestosLocales.ImpuestosLocales_TrasladosLocales";
            this.fxImporteLocTra.Expression = "[Importe]";
            this.fxImporteLocTra.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteLocTra.Name = "fxImporteLocTra";
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell25.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell25.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell25.StylePriority.UseBorderColor = false;
            this.xrTableCell25.StylePriority.UseBorders = false;
            this.xrTableCell25.StylePriority.UseFont = false;
            this.xrTableCell25.StylePriority.UsePadding = false;
            this.xrTableCell25.Text = "Cadena Original del Complemento de Certificación Digital del SAT:";
            this.xrTableCell25.Weight = 7.8599995044549695D;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(4.750029F, 4.999987F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(31.25F, 15F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // ReportHeader
            // 
            this.ReportHeader.BorderColor = System.Drawing.Color.LightGray;
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable26});
            this.ReportHeader.Expanded = false;
            this.ReportHeader.HeightF = 92.00001F;
            this.ReportHeader.KeepTogether = true;
            this.ReportHeader.Name = "ReportHeader";
            this.ReportHeader.StylePriority.UseBorderColor = false;
            this.ReportHeader.SubBands.AddRange(new DevExpress.XtraReports.UI.SubBand[] {
            this.SubBandHeader1,
            this.SubBandHeader2,
            this.SubBandHeader3,
            this.SubBandHeader4,
            this.SubBandHeader5,
            this.SubBandHeader6,
            this.SubBandHeader7});
            // 
            // xrTable26
            // 
            this.xrTable26.BackColor = System.Drawing.Color.Transparent;
            this.xrTable26.BorderColor = System.Drawing.Color.Black;
            this.xrTable26.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable26.BorderWidth = 1F;
            this.xrTable26.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable26.ForeColor = System.Drawing.Color.Black;
            this.xrTable26.LocationFloat = new DevExpress.Utils.PointFloat(0.9999911F, 0F);
            this.xrTable26.Name = "xrTable26";
            this.xrTable26.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.xrTable26.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable26.SizeF = new System.Drawing.SizeF(788F, 92.00001F);
            this.xrTable26.StylePriority.UseBorderColor = false;
            this.xrTable26.StylePriority.UseTextAlignment = false;
            this.xrTable26.Tag = "WithColor";
            this.xrTable26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell87,
            this.xrTableCell2});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1.8707350721933651D;
            // 
            // xrTableCell87
            // 
            this.xrTableCell87.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell87.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrpicLogo});
            this.xrTableCell87.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell87.Name = "xrTableCell87";
            this.xrTableCell87.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell87.StylePriority.UseBorders = false;
            this.xrTableCell87.StylePriority.UseFont = false;
            this.xrTableCell87.StylePriority.UsePadding = false;
            this.xrTableCell87.StylePriority.UseTextAlignment = false;
            this.xrTableCell87.Text = "xrTableCell87";
            this.xrTableCell87.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell87.Weight = 10.185889153491994D;
            this.xrTableCell87.WordWrap = false;
            // 
            // xrpicLogo
            // 
            this.xrpicLogo.AnchorHorizontal = ((DevExpress.XtraReports.UI.HorizontalAnchorStyles)((DevExpress.XtraReports.UI.HorizontalAnchorStyles.Left | DevExpress.XtraReports.UI.HorizontalAnchorStyles.Right)));
            this.xrpicLogo.AnchorVertical = ((DevExpress.XtraReports.UI.VerticalAnchorStyles)((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top | DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom)));
            this.xrpicLogo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrpicLogo.Image = ((System.Drawing.Image)(resources.GetObject("xrpicLogo.Image")));
            this.xrpicLogo.LocationFloat = new DevExpress.Utils.PointFloat(0.9999911F, 1.999998F);
            this.xrpicLogo.Name = "xrpicLogo";
            this.xrpicLogo.SizeF = new System.Drawing.SizeF(263.0002F, 88.99998F);
            this.xrpicLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            this.xrpicLogo.StylePriority.UseBorders = false;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell2.CanShrink = true;
            this.xrTableCell2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable27});
            this.xrTableCell2.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell2.Weight = 19.988849220004639D;
            // 
            // xrTable27
            // 
            this.xrTable27.BackColor = System.Drawing.Color.Transparent;
            this.xrTable27.BorderColor = System.Drawing.Color.Black;
            this.xrTable27.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable27.BorderWidth = 1F;
            this.xrTable27.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable27.ForeColor = System.Drawing.Color.Black;
            this.xrTable27.LocationFloat = new DevExpress.Utils.PointFloat(0F, 2F);
            this.xrTable27.Name = "xrTable27";
            this.xrTable27.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.xrTable27.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow5,
            this.xrTableRow13,
            this.xrTableRow47,
            this.xrTableRow49,
            this.xrTableRow48});
            this.xrTable27.SizeF = new System.Drawing.SizeF(522F, 89.99999F);
            this.xrTable27.Tag = "WithColor";
            this.xrTable27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell74});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrTableRow1.StylePriority.UsePadding = false;
            this.xrTableRow1.StylePriority.UseTextAlignment = false;
            this.xrTableRow1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableRow1.Weight = 1.8707350721933651D;
            // 
            // xrTableCell74
            // 
            this.xrTableCell74.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell74.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell74.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell74.ForeColor = System.Drawing.Color.White;
            this.xrTableCell74.Name = "xrTableCell74";
            this.xrTableCell74.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell74.StylePriority.UseBackColor = false;
            this.xrTableCell74.StylePriority.UseBorders = false;
            this.xrTableCell74.StylePriority.UseFont = false;
            this.xrTableCell74.StylePriority.UseForeColor = false;
            this.xrTableCell74.StylePriority.UsePadding = false;
            this.xrTableCell74.StylePriority.UseTextAlignment = false;
            this.xrTableCell74.Text = "Folio Fiscal (UUID)";
            this.xrTableCell74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell74.Weight = 15.526807124226417D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellUUID});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1.8707350721933653D;
            // 
            // CellUUID
            // 
            this.CellUUID.BackColor = System.Drawing.Color.White;
            this.CellUUID.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.CellUUID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.fxUUID")});
            this.CellUUID.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellUUID.Name = "CellUUID";
            this.CellUUID.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellUUID.StylePriority.UseBackColor = false;
            this.CellUUID.StylePriority.UseBorders = false;
            this.CellUUID.StylePriority.UseFont = false;
            this.CellUUID.StylePriority.UsePadding = false;
            this.CellUUID.StylePriority.UseTextAlignment = false;
            this.CellUUID.Text = "Recibo de Nómina";
            this.CellUUID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellUUID.Weight = 15.526807124226421D;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12,
            this.xrTableCell79});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1.8707350721933653D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell12.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell12.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.ForeColor = System.Drawing.Color.White;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell12.StylePriority.UseBackColor = false;
            this.xrTableCell12.StylePriority.UseBorderColor = false;
            this.xrTableCell12.StylePriority.UseBorders = false;
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseForeColor = false;
            this.xrTableCell12.StylePriority.UsePadding = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "Recibo de Nómina";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell12.Weight = 7.7634035621132114D;
            // 
            // xrTableCell79
            // 
            this.xrTableCell79.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell79.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell79.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell79.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell79.ForeColor = System.Drawing.Color.White;
            this.xrTableCell79.Name = "xrTableCell79";
            this.xrTableCell79.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell79.StylePriority.UseBackColor = false;
            this.xrTableCell79.StylePriority.UseBorderColor = false;
            this.xrTableCell79.StylePriority.UseBorders = false;
            this.xrTableCell79.StylePriority.UseFont = false;
            this.xrTableCell79.StylePriority.UseForeColor = false;
            this.xrTableCell79.StylePriority.UsePadding = false;
            this.xrTableCell79.StylePriority.UseTextAlignment = false;
            this.xrTableCell79.Text = "Lugar de Expidición";
            this.xrTableCell79.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell79.Weight = 7.7634035621132123D;
            // 
            // xrTableRow47
            // 
            this.xrTableRow47.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell29,
            this.xrTableCell84});
            this.xrTableRow47.Name = "xrTableRow47";
            this.xrTableRow47.Weight = 1.8707350721933653D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.BackColor = System.Drawing.Color.White;
            this.xrTableCell29.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.fxSerieFolio")});
            this.xrTableCell29.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell29.StylePriority.UseBackColor = false;
            this.xrTableCell29.StylePriority.UseBorders = false;
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.StylePriority.UsePadding = false;
            this.xrTableCell29.StylePriority.UseTextAlignment = false;
            this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell29.Weight = 7.7634035621132114D;
            // 
            // xrTableCell84
            // 
            this.xrTableCell84.BackColor = System.Drawing.Color.White;
            this.xrTableCell84.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell84.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.LugarExpedicion")});
            this.xrTableCell84.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell84.Name = "xrTableCell84";
            this.xrTableCell84.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell84.StylePriority.UseBackColor = false;
            this.xrTableCell84.StylePriority.UseBorders = false;
            this.xrTableCell84.StylePriority.UseFont = false;
            this.xrTableCell84.StylePriority.UsePadding = false;
            this.xrTableCell84.StylePriority.UseTextAlignment = false;
            this.xrTableCell84.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell84.Weight = 7.7634035621132123D;
            // 
            // xrTableRow49
            // 
            this.xrTableRow49.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell73,
            this.xrTableCell88});
            this.xrTableRow49.Name = "xrTableRow49";
            this.xrTableRow49.Weight = 1.8707350721933653D;
            // 
            // xrTableCell73
            // 
            this.xrTableCell73.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell73.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell73.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell73.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell73.ForeColor = System.Drawing.Color.White;
            this.xrTableCell73.Name = "xrTableCell73";
            this.xrTableCell73.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell73.StylePriority.UseBackColor = false;
            this.xrTableCell73.StylePriority.UseBorderColor = false;
            this.xrTableCell73.StylePriority.UseBorders = false;
            this.xrTableCell73.StylePriority.UseFont = false;
            this.xrTableCell73.StylePriority.UseForeColor = false;
            this.xrTableCell73.StylePriority.UsePadding = false;
            this.xrTableCell73.StylePriority.UseTextAlignment = false;
            this.xrTableCell73.Text = "Confirmación";
            this.xrTableCell73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell73.Weight = 7.7634035621132114D;
            // 
            // xrTableCell88
            // 
            this.xrTableCell88.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell88.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell88.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell88.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell88.ForeColor = System.Drawing.Color.White;
            this.xrTableCell88.Name = "xrTableCell88";
            this.xrTableCell88.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell88.StylePriority.UseBackColor = false;
            this.xrTableCell88.StylePriority.UseBorderColor = false;
            this.xrTableCell88.StylePriority.UseBorders = false;
            this.xrTableCell88.StylePriority.UseFont = false;
            this.xrTableCell88.StylePriority.UseForeColor = false;
            this.xrTableCell88.StylePriority.UsePadding = false;
            this.xrTableCell88.StylePriority.UseTextAlignment = false;
            this.xrTableCell88.Text = "Fecha";
            this.xrTableCell88.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell88.Weight = 7.7634035621132123D;
            // 
            // xrTableRow48
            // 
            this.xrTableRow48.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell69,
            this.xrTableCell89});
            this.xrTableRow48.Name = "xrTableRow48";
            this.xrTableRow48.Weight = 1.8707350721933653D;
            // 
            // xrTableCell69
            // 
            this.xrTableCell69.BackColor = System.Drawing.Color.White;
            this.xrTableCell69.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell69.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.fxConfirmacion")});
            this.xrTableCell69.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell69.Name = "xrTableCell69";
            this.xrTableCell69.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell69.StylePriority.UseBackColor = false;
            this.xrTableCell69.StylePriority.UseBorders = false;
            this.xrTableCell69.StylePriority.UseFont = false;
            this.xrTableCell69.StylePriority.UsePadding = false;
            this.xrTableCell69.StylePriority.UseTextAlignment = false;
            this.xrTableCell69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell69.Weight = 7.7634035621132114D;
            // 
            // xrTableCell89
            // 
            this.xrTableCell89.BackColor = System.Drawing.Color.White;
            this.xrTableCell89.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell89.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.fxFechaTimbrado", "{0:dd-MM-yyyy HH:mm}")});
            this.xrTableCell89.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell89.Name = "xrTableCell89";
            this.xrTableCell89.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell89.StylePriority.UseBackColor = false;
            this.xrTableCell89.StylePriority.UseBorders = false;
            this.xrTableCell89.StylePriority.UseFont = false;
            this.xrTableCell89.StylePriority.UsePadding = false;
            this.xrTableCell89.StylePriority.UseTextAlignment = false;
            this.xrTableCell89.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell89.Weight = 7.7634035621132123D;
            // 
            // SubBandHeader1
            // 
            this.SubBandHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.SubBandHeader1.Expanded = false;
            this.SubBandHeader1.HeightF = 15F;
            this.SubBandHeader1.Name = "SubBandHeader1";
            this.SubBandHeader1.StylePriority.UseBackColor = false;
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.Transparent;
            this.xrTable1.BorderColor = System.Drawing.Color.Black;
            this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable1.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.ForeColor = System.Drawing.Color.White;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow10});
            this.xrTable1.SizeF = new System.Drawing.SizeF(788F, 15F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorderColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseForeColor = false;
            this.xrTable1.StylePriority.UsePadding = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.Tag = "WithColor";
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell24,
            this.xrTableCell23});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 2.3384188402417063D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell24.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell24.StylePriority.UseBackColor = false;
            this.xrTableCell24.StylePriority.UseBorders = false;
            this.xrTableCell24.StylePriority.UsePadding = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "Emisor";
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell24.Weight = 1.8620767663682174D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell23.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell23.CanShrink = true;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell23.StylePriority.UseBackColor = false;
            this.xrTableCell23.StylePriority.UseBorders = false;
            this.xrTableCell23.StylePriority.UsePadding = false;
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "Receptor";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell23.Weight = 1.8620767663682174D;
            // 
            // SubBandHeader2
            // 
            this.SubBandHeader2.BackColor = System.Drawing.Color.Transparent;
            this.SubBandHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable25,
            this.xrTable23});
            this.SubBandHeader2.Expanded = false;
            this.SubBandHeader2.HeightF = 45F;
            this.SubBandHeader2.Name = "SubBandHeader2";
            this.SubBandHeader2.StylePriority.UseBackColor = false;
            // 
            // xrTable25
            // 
            this.xrTable25.BackColor = System.Drawing.Color.Transparent;
            this.xrTable25.BorderColor = System.Drawing.Color.Black;
            this.xrTable25.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable25.BorderWidth = 1F;
            this.xrTable25.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable25.ForeColor = System.Drawing.Color.Black;
            this.xrTable25.LocationFloat = new DevExpress.Utils.PointFloat(396.0001F, 0F);
            this.xrTable25.Name = "xrTable25";
            this.xrTable25.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.xrTable25.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow73,
            this.xrTableRow74});
            this.xrTable25.SizeF = new System.Drawing.SizeF(392.9999F, 29.99998F);
            this.xrTable25.StylePriority.UseBorderColor = false;
            this.xrTable25.StylePriority.UseTextAlignment = false;
            this.xrTable25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow73
            // 
            this.xrTableRow73.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell76});
            this.xrTableRow73.Name = "xrTableRow73";
            this.xrTableRow73.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrTableRow73.StylePriority.UsePadding = false;
            this.xrTableRow73.StylePriority.UseTextAlignment = false;
            this.xrTableRow73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableRow73.Weight = 1.8707350721933651D;
            // 
            // xrTableCell76
            // 
            this.xrTableCell76.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell76.CanShrink = true;
            this.xrTableCell76.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ReceptorComprobante.fxReceptorNombre")});
            this.xrTableCell76.Font = new System.Drawing.Font("Helvetica-Normal", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell76.ForeColor = System.Drawing.Color.DarkBlue;
            this.xrTableCell76.Name = "xrTableCell76";
            this.xrTableCell76.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell76.StylePriority.UseBorders = false;
            this.xrTableCell76.StylePriority.UseFont = false;
            this.xrTableCell76.StylePriority.UseForeColor = false;
            this.xrTableCell76.StylePriority.UsePadding = false;
            this.xrTableCell76.StylePriority.UseTextAlignment = false;
            this.xrTableCell76.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell76.Weight = 10.525041563577616D;
            // 
            // xrTableRow74
            // 
            this.xrTableRow74.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell77,
            this.CellReceptorRFC,
            this.xrTableCell27,
            this.xrTableCell78});
            this.xrTableRow74.Name = "xrTableRow74";
            this.xrTableRow74.Weight = 1.8707350721933653D;
            // 
            // xrTableCell77
            // 
            this.xrTableCell77.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell77.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell77.Name = "xrTableCell77";
            this.xrTableCell77.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell77.StylePriority.UseBorders = false;
            this.xrTableCell77.StylePriority.UseFont = false;
            this.xrTableCell77.StylePriority.UsePadding = false;
            this.xrTableCell77.StylePriority.UseTextAlignment = false;
            this.xrTableCell77.Text = "RFC:";
            this.xrTableCell77.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell77.Weight = 1.0444691715856669D;
            // 
            // CellReceptorRFC
            // 
            this.CellReceptorRFC.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellReceptorRFC.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ReceptorComprobante.Rfc")});
            this.CellReceptorRFC.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellReceptorRFC.Name = "CellReceptorRFC";
            this.CellReceptorRFC.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellReceptorRFC.StylePriority.UseBorders = false;
            this.CellReceptorRFC.StylePriority.UseFont = false;
            this.CellReceptorRFC.StylePriority.UsePadding = false;
            this.CellReceptorRFC.StylePriority.UseTextAlignment = false;
            this.CellReceptorRFC.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellReceptorRFC.Weight = 3.6690345712466019D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell27.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell27.StylePriority.UseBorders = false;
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.StylePriority.UsePadding = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.Text = "CURP:";
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell27.Weight = 1.312282930749674D;
            // 
            // xrTableCell78
            // 
            this.xrTableCell78.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell78.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.Curp")});
            this.xrTableCell78.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell78.Name = "xrTableCell78";
            this.xrTableCell78.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell78.StylePriority.UseBorders = false;
            this.xrTableCell78.StylePriority.UseFont = false;
            this.xrTableCell78.StylePriority.UsePadding = false;
            this.xrTableCell78.StylePriority.UseTextAlignment = false;
            this.xrTableCell78.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell78.Weight = 4.4992548899956768D;
            // 
            // xrTable23
            // 
            this.xrTable23.BackColor = System.Drawing.Color.Transparent;
            this.xrTable23.BorderColor = System.Drawing.Color.Black;
            this.xrTable23.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable23.BorderWidth = 1F;
            this.xrTable23.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable23.ForeColor = System.Drawing.Color.Black;
            this.xrTable23.LocationFloat = new DevExpress.Utils.PointFloat(0.9999911F, 0F);
            this.xrTable23.Name = "xrTable23";
            this.xrTable23.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.xrTable23.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow12,
            this.xrTableRow20,
            this.xrTableRow31});
            this.xrTable23.SizeF = new System.Drawing.SizeF(392.9999F, 45F);
            this.xrTable23.StylePriority.UseBorderColor = false;
            this.xrTable23.StylePriority.UseTextAlignment = false;
            this.xrTable23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell63});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrTableRow12.StylePriority.UsePadding = false;
            this.xrTableRow12.StylePriority.UseTextAlignment = false;
            this.xrTableRow12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableRow12.Weight = 1.2289504059708025D;
            // 
            // xrTableCell63
            // 
            this.xrTableCell63.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell63.CanShrink = true;
            this.xrTableCell63.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmisorComprobante.fxNombreEmisor")});
            this.xrTableCell63.Font = new System.Drawing.Font("Helvetica-Normal", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell63.ForeColor = System.Drawing.Color.DarkBlue;
            this.xrTableCell63.Name = "xrTableCell63";
            this.xrTableCell63.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell63.StylePriority.UseBorders = false;
            this.xrTableCell63.StylePriority.UseFont = false;
            this.xrTableCell63.StylePriority.UseForeColor = false;
            this.xrTableCell63.StylePriority.UsePadding = false;
            this.xrTableCell63.StylePriority.UseTextAlignment = false;
            this.xrTableCell63.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell63.Weight = 10.525042179185514D;
            // 
            // xrTableRow20
            // 
            this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell64,
            this.CellEmisorRFC,
            this.xrTableCell70,
            this.xrTableCell65});
            this.xrTableRow20.Name = "xrTableRow20";
            this.xrTableRow20.Weight = 1.2289504059708025D;
            // 
            // xrTableCell64
            // 
            this.xrTableCell64.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell64.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell64.Name = "xrTableCell64";
            this.xrTableCell64.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell64.StylePriority.UseBorders = false;
            this.xrTableCell64.StylePriority.UseFont = false;
            this.xrTableCell64.StylePriority.UsePadding = false;
            this.xrTableCell64.StylePriority.UseTextAlignment = false;
            this.xrTableCell64.Text = "RFC:";
            this.xrTableCell64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell64.Weight = 1.044469245825066D;
            // 
            // CellEmisorRFC
            // 
            this.CellEmisorRFC.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellEmisorRFC.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmisorComprobante.Rfc")});
            this.CellEmisorRFC.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellEmisorRFC.Name = "CellEmisorRFC";
            this.CellEmisorRFC.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellEmisorRFC.StylePriority.UseBorders = false;
            this.CellEmisorRFC.StylePriority.UseFont = false;
            this.CellEmisorRFC.StylePriority.UsePadding = false;
            this.CellEmisorRFC.StylePriority.UseTextAlignment = false;
            this.CellEmisorRFC.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellEmisorRFC.Weight = 3.6690346676226948D;
            // 
            // xrTableCell70
            // 
            this.xrTableCell70.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell70.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Emisor.fxEmisorCURPTit")});
            this.xrTableCell70.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell70.Name = "xrTableCell70";
            this.xrTableCell70.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell70.StylePriority.UseBorders = false;
            this.xrTableCell70.StylePriority.UseFont = false;
            this.xrTableCell70.StylePriority.UsePadding = false;
            this.xrTableCell70.StylePriority.UseTextAlignment = false;
            this.xrTableCell70.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell70.Weight = 1.3122831219125422D;
            // 
            // xrTableCell65
            // 
            this.xrTableCell65.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell65.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Emisor.fxEmisorCURP")});
            this.xrTableCell65.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell65.Name = "xrTableCell65";
            this.xrTableCell65.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell65.StylePriority.UseBorders = false;
            this.xrTableCell65.StylePriority.UseFont = false;
            this.xrTableCell65.StylePriority.UsePadding = false;
            this.xrTableCell65.StylePriority.UseTextAlignment = false;
            this.xrTableCell65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell65.Weight = 4.4992551438252129D;
            // 
            // xrTableRow31
            // 
            this.xrTableRow31.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell66,
            this.CellRegimenFiscal});
            this.xrTableRow31.Name = "xrTableRow31";
            this.xrTableRow31.Weight = 1.2289504059708021D;
            // 
            // xrTableCell66
            // 
            this.xrTableCell66.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell66.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell66.Name = "xrTableCell66";
            this.xrTableCell66.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell66.StylePriority.UseBorders = false;
            this.xrTableCell66.StylePriority.UseFont = false;
            this.xrTableCell66.StylePriority.UsePadding = false;
            this.xrTableCell66.StylePriority.UseTextAlignment = false;
            this.xrTableCell66.Text = "Regimen Fiscal:";
            this.xrTableCell66.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell66.Weight = 2.6252114069722254D;
            // 
            // CellRegimenFiscal
            // 
            this.CellRegimenFiscal.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellRegimenFiscal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EmisorComprobante.RegimenFiscal")});
            this.CellRegimenFiscal.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellRegimenFiscal.Name = "CellRegimenFiscal";
            this.CellRegimenFiscal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellRegimenFiscal.StylePriority.UseBorders = false;
            this.CellRegimenFiscal.StylePriority.UseFont = false;
            this.CellRegimenFiscal.StylePriority.UsePadding = false;
            this.CellRegimenFiscal.StylePriority.UseTextAlignment = false;
            this.CellRegimenFiscal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellRegimenFiscal.Weight = 7.89983077221329D;
            this.CellRegimenFiscal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellRegimenFiscal_BeforePrint);
            // 
            // SubBandHeader3
            // 
            this.SubBandHeader3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine2});
            this.SubBandHeader3.Expanded = false;
            this.SubBandHeader3.HeightF = 2.000014F;
            this.SubBandHeader3.Name = "SubBandHeader3";
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(788F, 2F);
            this.xrLine2.Tag = "WithColor";
            // 
            // SubBandHeader4
            // 
            this.SubBandHeader4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrTable3});
            this.SubBandHeader4.Expanded = false;
            this.SubBandHeader4.HeightF = 44.99998F;
            this.SubBandHeader4.Name = "SubBandHeader4";
            // 
            // xrTable4
            // 
            this.xrTable4.BackColor = System.Drawing.Color.Transparent;
            this.xrTable4.BorderColor = System.Drawing.Color.Black;
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.BorderWidth = 1F;
            this.xrTable4.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.ForeColor = System.Drawing.Color.Black;
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(395F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7,
            this.xrTableRow14});
            this.xrTable4.SizeF = new System.Drawing.SizeF(394F, 29.99998F);
            this.xrTable4.StylePriority.UseBorderColor = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell28,
            this.xrTableCell50});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrTableRow7.StylePriority.UsePadding = false;
            this.xrTableRow7.StylePriority.UseTextAlignment = false;
            this.xrTableRow7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableRow7.Weight = 1.8707350721933651D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell28.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell28.StylePriority.UseBorders = false;
            this.xrTableCell28.StylePriority.UseFont = false;
            this.xrTableCell28.StylePriority.UsePadding = false;
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "No. Empleado:";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell28.Weight = 2.4983893676151712D;
            // 
            // xrTableCell50
            // 
            this.xrTableCell50.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell50.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.NumEmpleado")});
            this.xrTableCell50.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell50.Name = "xrTableCell50";
            this.xrTableCell50.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell50.StylePriority.UseBorders = false;
            this.xrTableCell50.StylePriority.UseFont = false;
            this.xrTableCell50.StylePriority.UsePadding = false;
            this.xrTableCell50.StylePriority.UseTextAlignment = false;
            this.xrTableCell50.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell50.Weight = 7.5436845933741559D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell53,
            this.CellEstado});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1.8707350721933653D;
            // 
            // xrTableCell53
            // 
            this.xrTableCell53.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell53.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxEstadoTit")});
            this.xrTableCell53.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell53.Name = "xrTableCell53";
            this.xrTableCell53.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell53.StylePriority.UseBorders = false;
            this.xrTableCell53.StylePriority.UseFont = false;
            this.xrTableCell53.StylePriority.UsePadding = false;
            this.xrTableCell53.StylePriority.UseTextAlignment = false;
            this.xrTableCell53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell53.Weight = 2.4983893676151721D;
            // 
            // CellEstado
            // 
            this.CellEstado.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellEstado.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxEstado")});
            this.CellEstado.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellEstado.Name = "CellEstado";
            this.CellEstado.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellEstado.StylePriority.UseBorders = false;
            this.CellEstado.StylePriority.UseFont = false;
            this.CellEstado.StylePriority.UsePadding = false;
            this.CellEstado.StylePriority.UseTextAlignment = false;
            this.CellEstado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellEstado.Weight = 7.5436845933741576D;
            this.CellEstado.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellEstado_BeforePrint);
            // 
            // xrTable3
            // 
            this.xrTable3.BackColor = System.Drawing.Color.Transparent;
            this.xrTable3.BorderColor = System.Drawing.Color.Black;
            this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable3.BorderWidth = 1F;
            this.xrTable3.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable3.ForeColor = System.Drawing.Color.Black;
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow21,
            this.xrTableRow26,
            this.xrTableRow30});
            this.xrTable3.SizeF = new System.Drawing.SizeF(394F, 44.99998F);
            this.xrTable3.StylePriority.UseBorderColor = false;
            this.xrTable3.StylePriority.UseTextAlignment = false;
            this.xrTable3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow21
            // 
            this.xrTableRow21.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.CellRegimen});
            this.xrTableRow21.Name = "xrTableRow21";
            this.xrTableRow21.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrTableRow21.StylePriority.UsePadding = false;
            this.xrTableRow21.StylePriority.UseTextAlignment = false;
            this.xrTableRow21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableRow21.Weight = 1.8707350721933651D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell1.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Régimen:";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell1.Weight = 2.4983893676151712D;
            // 
            // CellRegimen
            // 
            this.CellRegimen.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellRegimen.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.TipoRegimen")});
            this.CellRegimen.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellRegimen.Name = "CellRegimen";
            this.CellRegimen.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellRegimen.StylePriority.UseBorders = false;
            this.CellRegimen.StylePriority.UseFont = false;
            this.CellRegimen.StylePriority.UsePadding = false;
            this.CellRegimen.StylePriority.UseTextAlignment = false;
            this.CellRegimen.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.CellRegimen.Weight = 7.5436845933741559D;
            this.CellRegimen.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellRegimen_BeforePrint);
            // 
            // xrTableRow26
            // 
            this.xrTableRow26.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell45,
            this.xrTableCell47});
            this.xrTableRow26.Name = "xrTableRow26";
            this.xrTableRow26.Weight = 1.8707350721933653D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell45.CanShrink = true;
            this.xrTableCell45.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxIMSSTit")});
            this.xrTableCell45.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell45.StylePriority.UseBorders = false;
            this.xrTableCell45.StylePriority.UseFont = false;
            this.xrTableCell45.StylePriority.UsePadding = false;
            this.xrTableCell45.StylePriority.UseTextAlignment = false;
            this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell45.Weight = 2.4983893676151721D;
            // 
            // xrTableCell47
            // 
            this.xrTableCell47.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell47.CanShrink = true;
            this.xrTableCell47.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxIMSS")});
            this.xrTableCell47.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell47.Name = "xrTableCell47";
            this.xrTableCell47.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell47.StylePriority.UseBorders = false;
            this.xrTableCell47.StylePriority.UseFont = false;
            this.xrTableCell47.StylePriority.UsePadding = false;
            this.xrTableCell47.StylePriority.UseTextAlignment = false;
            this.xrTableCell47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell47.Weight = 7.5436845933741576D;
            // 
            // xrTableRow30
            // 
            this.xrTableRow30.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell48,
            this.xrTableCell49});
            this.xrTableRow30.Name = "xrTableRow30";
            this.xrTableRow30.Weight = 1.8707350721933651D;
            // 
            // xrTableCell48
            // 
            this.xrTableCell48.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell48.CanShrink = true;
            this.xrTableCell48.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Emisor.fxRegistroPatronalTit")});
            this.xrTableCell48.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell48.Name = "xrTableCell48";
            this.xrTableCell48.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell48.StylePriority.UseBorders = false;
            this.xrTableCell48.StylePriority.UseFont = false;
            this.xrTableCell48.StylePriority.UsePadding = false;
            this.xrTableCell48.StylePriority.UseTextAlignment = false;
            this.xrTableCell48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell48.Weight = 2.4983893676151721D;
            // 
            // xrTableCell49
            // 
            this.xrTableCell49.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell49.CanShrink = true;
            this.xrTableCell49.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Emisor.fxRegistroPatronal")});
            this.xrTableCell49.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell49.Name = "xrTableCell49";
            this.xrTableCell49.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell49.StylePriority.UseBorders = false;
            this.xrTableCell49.StylePriority.UseFont = false;
            this.xrTableCell49.StylePriority.UsePadding = false;
            this.xrTableCell49.StylePriority.UseTextAlignment = false;
            this.xrTableCell49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell49.Weight = 7.5436845933741576D;
            // 
            // SubBandHeader5
            // 
            this.SubBandHeader5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1});
            this.SubBandHeader5.Expanded = false;
            this.SubBandHeader5.HeightF = 2F;
            this.SubBandHeader5.Name = "SubBandHeader5";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(788F, 2F);
            this.xrLine1.Tag = "WithColor";
            // 
            // SubBandHeader6
            // 
            this.SubBandHeader6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tblHeader,
            this.xrTable2,
            this.xrTable22});
            this.SubBandHeader6.Expanded = false;
            this.SubBandHeader6.HeightF = 60F;
            this.SubBandHeader6.Name = "SubBandHeader6";
            // 
            // tblHeader
            // 
            this.tblHeader.BackColor = System.Drawing.Color.Transparent;
            this.tblHeader.BorderColor = System.Drawing.Color.Black;
            this.tblHeader.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tblHeader.BorderWidth = 1F;
            this.tblHeader.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblHeader.ForeColor = System.Drawing.Color.Black;
            this.tblHeader.LocationFloat = new DevExpress.Utils.PointFloat(1.000061F, 0F);
            this.tblHeader.Name = "tblHeader";
            this.tblHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.tblHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow57,
            this.xrTableRow64,
            this.xrTableRow68,
            this.xrTableRow72});
            this.tblHeader.SizeF = new System.Drawing.SizeF(308F, 60F);
            this.tblHeader.StylePriority.UseBorderColor = false;
            this.tblHeader.StylePriority.UseTextAlignment = false;
            this.tblHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow57
            // 
            this.xrTableRow57.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell57,
            this.CellTipoContrato});
            this.xrTableRow57.Name = "xrTableRow57";
            this.xrTableRow57.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrTableRow57.StylePriority.UsePadding = false;
            this.xrTableRow57.StylePriority.UseTextAlignment = false;
            this.xrTableRow57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableRow57.Weight = 1.8707350721933651D;
            // 
            // xrTableCell57
            // 
            this.xrTableCell57.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell57.CanShrink = true;
            this.xrTableCell57.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell57.Name = "xrTableCell57";
            this.xrTableCell57.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell57.StylePriority.UseBorders = false;
            this.xrTableCell57.Text = "Tipo Contrato:";
            this.xrTableCell57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell57.Weight = 4.1434067693409942D;
            // 
            // CellTipoContrato
            // 
            this.CellTipoContrato.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellTipoContrato.CanShrink = true;
            this.CellTipoContrato.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.TipoContrato")});
            this.CellTipoContrato.Multiline = true;
            this.CellTipoContrato.Name = "CellTipoContrato";
            this.CellTipoContrato.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellTipoContrato.StylePriority.UseBorders = false;
            this.CellTipoContrato.StylePriority.UsePadding = false;
            this.CellTipoContrato.StylePriority.UseTextAlignment = false;
            this.CellTipoContrato.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.CellTipoContrato.Weight = 8.2465900670518426D;
            this.CellTipoContrato.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellTipoContrato_BeforePrint);
            // 
            // xrTableRow64
            // 
            this.xrTableRow64.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell71,
            this.xrTableCell72});
            this.xrTableRow64.Name = "xrTableRow64";
            this.xrTableRow64.Weight = 1.8707350721933653D;
            // 
            // xrTableCell71
            // 
            this.xrTableCell71.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell71.CanShrink = true;
            this.xrTableCell71.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxDeptoTit")});
            this.xrTableCell71.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell71.Name = "xrTableCell71";
            this.xrTableCell71.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell71.StylePriority.UseBorders = false;
            this.xrTableCell71.StylePriority.UseFont = false;
            this.xrTableCell71.StylePriority.UsePadding = false;
            this.xrTableCell71.StylePriority.UseTextAlignment = false;
            this.xrTableCell71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell71.Weight = 4.1434067693409942D;
            // 
            // xrTableCell72
            // 
            this.xrTableCell72.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell72.CanShrink = true;
            this.xrTableCell72.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxDepto")});
            this.xrTableCell72.Name = "xrTableCell72";
            this.xrTableCell72.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell72.StylePriority.UseBorders = false;
            this.xrTableCell72.StylePriority.UsePadding = false;
            this.xrTableCell72.StylePriority.UseTextAlignment = false;
            this.xrTableCell72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell72.Weight = 8.2465900670518426D;
            // 
            // xrTableRow68
            // 
            this.xrTableRow68.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell85,
            this.xrTableCell86});
            this.xrTableRow68.Name = "xrTableRow68";
            this.xrTableRow68.Weight = 1.8707350721933651D;
            // 
            // xrTableCell85
            // 
            this.xrTableCell85.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell85.CanShrink = true;
            this.xrTableCell85.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxPuestoTit")});
            this.xrTableCell85.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell85.Name = "xrTableCell85";
            this.xrTableCell85.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell85.StylePriority.UseBorders = false;
            this.xrTableCell85.StylePriority.UseFont = false;
            this.xrTableCell85.StylePriority.UsePadding = false;
            this.xrTableCell85.StylePriority.UseTextAlignment = false;
            this.xrTableCell85.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell85.Weight = 4.1434067693409942D;
            // 
            // xrTableCell86
            // 
            this.xrTableCell86.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell86.CanShrink = true;
            this.xrTableCell86.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxPuesto")});
            this.xrTableCell86.Name = "xrTableCell86";
            this.xrTableCell86.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell86.StylePriority.UseBorders = false;
            this.xrTableCell86.StylePriority.UsePadding = false;
            this.xrTableCell86.StylePriority.UseTextAlignment = false;
            this.xrTableCell86.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell86.Weight = 8.2465900670518426D;
            // 
            // xrTableRow72
            // 
            this.xrTableRow72.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell95,
            this.CellPeriodicidad});
            this.xrTableRow72.Name = "xrTableRow72";
            this.xrTableRow72.Weight = 1.8707350721933649D;
            // 
            // xrTableCell95
            // 
            this.xrTableCell95.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell95.CanShrink = true;
            this.xrTableCell95.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell95.Name = "xrTableCell95";
            this.xrTableCell95.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell95.StylePriority.UseBorders = false;
            this.xrTableCell95.StylePriority.UseFont = false;
            this.xrTableCell95.StylePriority.UsePadding = false;
            this.xrTableCell95.StylePriority.UseTextAlignment = false;
            this.xrTableCell95.Text = "Periodicidad Pago:";
            this.xrTableCell95.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell95.Weight = 4.1434067693409942D;
            // 
            // CellPeriodicidad
            // 
            this.CellPeriodicidad.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellPeriodicidad.CanShrink = true;
            this.CellPeriodicidad.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.PeriodicidadPago")});
            this.CellPeriodicidad.Font = new System.Drawing.Font("Arial Unicode MS", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellPeriodicidad.Name = "CellPeriodicidad";
            this.CellPeriodicidad.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellPeriodicidad.StylePriority.UseBorders = false;
            this.CellPeriodicidad.StylePriority.UseFont = false;
            this.CellPeriodicidad.StylePriority.UsePadding = false;
            this.CellPeriodicidad.StylePriority.UseTextAlignment = false;
            this.CellPeriodicidad.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellPeriodicidad.Weight = 8.2465900670518426D;
            this.CellPeriodicidad.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellPeriodicidad_BeforePrint);
            // 
            // xrTable2
            // 
            this.xrTable2.BackColor = System.Drawing.Color.Transparent;
            this.xrTable2.BorderColor = System.Drawing.Color.Black;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.BorderWidth = 1F;
            this.xrTable2.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.ForeColor = System.Drawing.Color.Black;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(310.0001F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2,
            this.xrTableRow3,
            this.xrTableRow11,
            this.xrTableRow17});
            this.xrTable2.SizeF = new System.Drawing.SizeF(233.9999F, 60F);
            this.xrTable2.StylePriority.UseBorderColor = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell40,
            this.xrTableCell46});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrTableRow2.StylePriority.UsePadding = false;
            this.xrTableRow2.StylePriority.UseTextAlignment = false;
            this.xrTableRow2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableRow2.Weight = 1.8707350721933651D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell40.CanShrink = true;
            this.xrTableCell40.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxFechaInicioRelLaboralTit")});
            this.xrTableCell40.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell40.StylePriority.UseBorders = false;
            this.xrTableCell40.StylePriority.UseFont = false;
            this.xrTableCell40.StylePriority.UsePadding = false;
            this.xrTableCell40.StylePriority.UseTextAlignment = false;
            this.xrTableCell40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell40.Weight = 1.3076149370333543D;
            // 
            // xrTableCell46
            // 
            this.xrTableCell46.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell46.CanShrink = true;
            this.xrTableCell46.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxFechaInicioRelLaboral", "{0:dd-MM-yyyy}")});
            this.xrTableCell46.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell46.Name = "xrTableCell46";
            this.xrTableCell46.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell46.StylePriority.UseBorders = false;
            this.xrTableCell46.StylePriority.UseFont = false;
            this.xrTableCell46.StylePriority.UsePadding = false;
            this.xrTableCell46.StylePriority.UseTextAlignment = false;
            this.xrTableCell46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell46.Weight = 1.6630831657977212D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell51,
            this.xrTableCell52});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1.8707350721933653D;
            // 
            // xrTableCell51
            // 
            this.xrTableCell51.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell51.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell51.StylePriority.UseBorders = false;
            this.xrTableCell51.StylePriority.UseFont = false;
            this.xrTableCell51.StylePriority.UsePadding = false;
            this.xrTableCell51.StylePriority.UseTextAlignment = false;
            this.xrTableCell51.Text = "Periodo de Pago:";
            this.xrTableCell51.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell51.Weight = 1.3076149370333543D;
            // 
            // xrTableCell52
            // 
            this.xrTableCell52.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell52.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Nomina.fxPeriodoDePago")});
            this.xrTableCell52.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell52.StylePriority.UseBorders = false;
            this.xrTableCell52.StylePriority.UseFont = false;
            this.xrTableCell52.StylePriority.UsePadding = false;
            this.xrTableCell52.StylePriority.UseTextAlignment = false;
            this.xrTableCell52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell52.Weight = 1.6630831657977212D;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell114,
            this.xrTableCell120});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1.8707350721933651D;
            // 
            // xrTableCell114
            // 
            this.xrTableCell114.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell114.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell114.Name = "xrTableCell114";
            this.xrTableCell114.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell114.StylePriority.UseBorders = false;
            this.xrTableCell114.StylePriority.UseFont = false;
            this.xrTableCell114.StylePriority.UsePadding = false;
            this.xrTableCell114.StylePriority.UseTextAlignment = false;
            this.xrTableCell114.Text = "Fecha de Pago:";
            this.xrTableCell114.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell114.Weight = 1.3076149370333543D;
            // 
            // xrTableCell120
            // 
            this.xrTableCell120.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell120.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Nomina.fxFechaPago", "{0:dd-MM-yyyy}")});
            this.xrTableCell120.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell120.Name = "xrTableCell120";
            this.xrTableCell120.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell120.StylePriority.UseBorders = false;
            this.xrTableCell120.StylePriority.UseFont = false;
            this.xrTableCell120.StylePriority.UsePadding = false;
            this.xrTableCell120.StylePriority.UseTextAlignment = false;
            this.xrTableCell120.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell120.Weight = 1.6630831657977212D;
            // 
            // xrTableRow17
            // 
            this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell127,
            this.xrTableCell132});
            this.xrTableRow17.Name = "xrTableRow17";
            this.xrTableRow17.Weight = 1.8707350721933649D;
            // 
            // xrTableCell127
            // 
            this.xrTableCell127.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell127.CanShrink = true;
            this.xrTableCell127.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxAntiguedadTit")});
            this.xrTableCell127.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell127.Name = "xrTableCell127";
            this.xrTableCell127.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell127.StylePriority.UseBorders = false;
            this.xrTableCell127.StylePriority.UseFont = false;
            this.xrTableCell127.StylePriority.UsePadding = false;
            this.xrTableCell127.StylePriority.UseTextAlignment = false;
            this.xrTableCell127.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell127.Weight = 1.3076149370333543D;
            // 
            // xrTableCell132
            // 
            this.xrTableCell132.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell132.CanShrink = true;
            this.xrTableCell132.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxAntiguedad")});
            this.xrTableCell132.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell132.Name = "xrTableCell132";
            this.xrTableCell132.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell132.StylePriority.UseBorders = false;
            this.xrTableCell132.StylePriority.UseFont = false;
            this.xrTableCell132.StylePriority.UsePadding = false;
            this.xrTableCell132.StylePriority.UseTextAlignment = false;
            this.xrTableCell132.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell132.Weight = 1.6630831657977212D;
            // 
            // xrTable22
            // 
            this.xrTable22.BackColor = System.Drawing.Color.Transparent;
            this.xrTable22.BorderColor = System.Drawing.Color.Black;
            this.xrTable22.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable22.BorderWidth = 1F;
            this.xrTable22.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable22.ForeColor = System.Drawing.Color.Black;
            this.xrTable22.LocationFloat = new DevExpress.Utils.PointFloat(575.0001F, 0F);
            this.xrTable22.Name = "xrTable22";
            this.xrTable22.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.xrTable22.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow19,
            this.xrTableRow23,
            this.xrTableRow24,
            this.xrTableRow25});
            this.xrTable22.SizeF = new System.Drawing.SizeF(213.9999F, 59.99997F);
            this.xrTable22.StylePriority.UseBorderColor = false;
            this.xrTable22.StylePriority.UseTextAlignment = false;
            this.xrTable22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell139,
            this.xrTableCell140});
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrTableRow19.StylePriority.UsePadding = false;
            this.xrTableRow19.StylePriority.UseTextAlignment = false;
            this.xrTableRow19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableRow19.Weight = 1.8707350721933651D;
            // 
            // xrTableCell139
            // 
            this.xrTableCell139.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell139.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell139.Name = "xrTableCell139";
            this.xrTableCell139.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell139.StylePriority.UseBorders = false;
            this.xrTableCell139.StylePriority.UseFont = false;
            this.xrTableCell139.StylePriority.UsePadding = false;
            this.xrTableCell139.StylePriority.UseTextAlignment = false;
            this.xrTableCell139.Text = "Días Pagados:";
            this.xrTableCell139.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell139.Weight = 2.6252114069722245D;
            // 
            // xrTableCell140
            // 
            this.xrTableCell140.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell140.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Nomina.fxNumDiasPagados", "{0:#.00}")});
            this.xrTableCell140.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell140.Name = "xrTableCell140";
            this.xrTableCell140.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell140.StylePriority.UseBorders = false;
            this.xrTableCell140.StylePriority.UseFont = false;
            this.xrTableCell140.StylePriority.UsePadding = false;
            this.xrTableCell140.StylePriority.UseTextAlignment = false;
            this.xrTableCell140.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell140.Weight = 2.8291106952419534D;
            // 
            // xrTableRow23
            // 
            this.xrTableRow23.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell145,
            this.xrTableCell146});
            this.xrTableRow23.Name = "xrTableRow23";
            this.xrTableRow23.Weight = 1.8707350721933653D;
            // 
            // xrTableCell145
            // 
            this.xrTableCell145.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell145.CanShrink = true;
            this.xrTableCell145.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxSalarioBaseTit")});
            this.xrTableCell145.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell145.Name = "xrTableCell145";
            this.xrTableCell145.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell145.StylePriority.UseBorders = false;
            this.xrTableCell145.StylePriority.UseFont = false;
            this.xrTableCell145.StylePriority.UsePadding = false;
            this.xrTableCell145.StylePriority.UseTextAlignment = false;
            this.xrTableCell145.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell145.Weight = 2.6252114069722254D;
            // 
            // xrTableCell146
            // 
            this.xrTableCell146.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell146.CanShrink = true;
            this.xrTableCell146.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxSalarioBase", "{0:$ #,###0.00}")});
            this.xrTableCell146.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell146.Name = "xrTableCell146";
            this.xrTableCell146.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell146.StylePriority.UseBorders = false;
            this.xrTableCell146.StylePriority.UseFont = false;
            this.xrTableCell146.StylePriority.UsePadding = false;
            this.xrTableCell146.StylePriority.UseTextAlignment = false;
            this.xrTableCell146.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell146.Weight = 2.8291106952419534D;
            // 
            // xrTableRow24
            // 
            this.xrTableRow24.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell151,
            this.xrTableCell152});
            this.xrTableRow24.Name = "xrTableRow24";
            this.xrTableRow24.Weight = 1.8707350721933651D;
            // 
            // xrTableCell151
            // 
            this.xrTableCell151.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell151.CanShrink = true;
            this.xrTableCell151.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxSalarioDiarioTit")});
            this.xrTableCell151.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell151.Name = "xrTableCell151";
            this.xrTableCell151.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell151.StylePriority.UseBorders = false;
            this.xrTableCell151.StylePriority.UseFont = false;
            this.xrTableCell151.StylePriority.UsePadding = false;
            this.xrTableCell151.StylePriority.UseTextAlignment = false;
            this.xrTableCell151.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell151.Weight = 2.6252114069722254D;
            // 
            // xrTableCell152
            // 
            this.xrTableCell152.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell152.CanShrink = true;
            this.xrTableCell152.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxSalarioDiario", "{0:$ #,###0.00}")});
            this.xrTableCell152.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell152.Name = "xrTableCell152";
            this.xrTableCell152.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell152.StylePriority.UseBorders = false;
            this.xrTableCell152.StylePriority.UseFont = false;
            this.xrTableCell152.StylePriority.UsePadding = false;
            this.xrTableCell152.StylePriority.UseTextAlignment = false;
            this.xrTableCell152.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell152.Weight = 2.8291106952419534D;
            // 
            // xrTableRow25
            // 
            this.xrTableRow25.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell157,
            this.xrTableCell158});
            this.xrTableRow25.Name = "xrTableRow25";
            this.xrTableRow25.Weight = 1.8707350721933649D;
            // 
            // xrTableCell157
            // 
            this.xrTableCell157.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell157.CanShrink = true;
            this.xrTableCell157.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxCuentaBancariaTit")});
            this.xrTableCell157.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell157.Name = "xrTableCell157";
            this.xrTableCell157.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell157.StylePriority.UseBorders = false;
            this.xrTableCell157.StylePriority.UseFont = false;
            this.xrTableCell157.StylePriority.UsePadding = false;
            this.xrTableCell157.StylePriority.UseTextAlignment = false;
            this.xrTableCell157.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell157.Weight = 2.6252114069722254D;
            // 
            // xrTableCell158
            // 
            this.xrTableCell158.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell158.CanShrink = true;
            this.xrTableCell158.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.fxCuentaBancaria")});
            this.xrTableCell158.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell158.Name = "xrTableCell158";
            this.xrTableCell158.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell158.StylePriority.UseBorders = false;
            this.xrTableCell158.StylePriority.UseFont = false;
            this.xrTableCell158.StylePriority.UsePadding = false;
            this.xrTableCell158.StylePriority.UseTextAlignment = false;
            this.xrTableCell158.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell158.Weight = 2.8291106952419534D;
            // 
            // SubBandHeader7
            // 
            this.SubBandHeader7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tblMetodoPago});
            this.SubBandHeader7.Expanded = false;
            this.SubBandHeader7.HeightF = 15F;
            this.SubBandHeader7.Name = "SubBandHeader7";
            // 
            // tblMetodoPago
            // 
            this.tblMetodoPago.BackColor = System.Drawing.Color.Transparent;
            this.tblMetodoPago.BorderColor = System.Drawing.Color.Black;
            this.tblMetodoPago.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tblMetodoPago.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblMetodoPago.ForeColor = System.Drawing.Color.White;
            this.tblMetodoPago.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.tblMetodoPago.Name = "tblMetodoPago";
            this.tblMetodoPago.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 1, 1, 100F);
            this.tblMetodoPago.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow27});
            this.tblMetodoPago.SizeF = new System.Drawing.SizeF(788F, 15F);
            this.tblMetodoPago.StylePriority.UseBackColor = false;
            this.tblMetodoPago.StylePriority.UseBorderColor = false;
            this.tblMetodoPago.StylePriority.UseBorders = false;
            this.tblMetodoPago.StylePriority.UseFont = false;
            this.tblMetodoPago.StylePriority.UseForeColor = false;
            this.tblMetodoPago.StylePriority.UsePadding = false;
            this.tblMetodoPago.StylePriority.UseTextAlignment = false;
            this.tblMetodoPago.Tag = "WithColor";
            this.tblMetodoPago.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow27
            // 
            this.xrTableRow27.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellMetodoDePago});
            this.xrTableRow27.Name = "xrTableRow27";
            this.xrTableRow27.Weight = 2.3384188402417063D;
            // 
            // CellMetodoDePago
            // 
            this.CellMetodoDePago.BackColor = System.Drawing.Color.DarkGray;
            this.CellMetodoDePago.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.CellMetodoDePago.CanShrink = true;
            this.CellMetodoDePago.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.MetodoPago")});
            this.CellMetodoDePago.Name = "CellMetodoDePago";
            this.CellMetodoDePago.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellMetodoDePago.StylePriority.UseBackColor = false;
            this.CellMetodoDePago.StylePriority.UseBorders = false;
            this.CellMetodoDePago.StylePriority.UsePadding = false;
            this.CellMetodoDePago.StylePriority.UseTextAlignment = false;
            this.CellMetodoDePago.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellMetodoDePago.Weight = 3.7241535327364348D;
            this.CellMetodoDePago.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellMetodoDePago_BeforePrint);
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellSelloEmisor});
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.Weight = 0.11111109092753135D;
            // 
            // CellSelloEmisor
            // 
            this.CellSelloEmisor.BorderColor = System.Drawing.Color.Black;
            this.CellSelloEmisor.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellSelloEmisor.CanShrink = true;
            this.CellSelloEmisor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.sello")});
            this.CellSelloEmisor.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellSelloEmisor.Name = "CellSelloEmisor";
            this.CellSelloEmisor.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.CellSelloEmisor.StylePriority.UseBorderColor = false;
            this.CellSelloEmisor.StylePriority.UseBorders = false;
            this.CellSelloEmisor.StylePriority.UseFont = false;
            this.CellSelloEmisor.StylePriority.UsePadding = false;
            this.CellSelloEmisor.Weight = 7.8599995044549695D;
            // 
            // fxEstadoTit
            // 
            this.fxEstadoTit.DataMember = "Receptor";
            this.fxEstadoTit.Expression = "Iif(IsNullOrEmpty([ClaveEntFed]),\'\' ,\'Estado:\' )";
            this.fxEstadoTit.Name = "fxEstadoTit";
            // 
            // fxJubIngresoAcumulable
            // 
            this.fxJubIngresoAcumulable.DataMember = "JubilacionPensionRetiro";
            this.fxJubIngresoAcumulable.Expression = "[IngresoAcumulable]";
            this.fxJubIngresoAcumulable.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxJubIngresoAcumulable.Name = "fxJubIngresoAcumulable";
            // 
            // CellDescuentoValue
            // 
            this.CellDescuentoValue.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellDescuentoValue.CanShrink = true;
            this.CellDescuentoValue.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Nomina.fxTotalOtrosPagos", "{0:$ #,###0.00}")});
            this.CellDescuentoValue.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellDescuentoValue.Name = "CellDescuentoValue";
            this.CellDescuentoValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.CellDescuentoValue.StylePriority.UseBorders = false;
            this.CellDescuentoValue.StylePriority.UseFont = false;
            this.CellDescuentoValue.StylePriority.UsePadding = false;
            this.CellDescuentoValue.StylePriority.UseTextAlignment = false;
            this.CellDescuentoValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.CellDescuentoValue.Weight = 1.12D;
            // 
            // fxReceptorNombreTit
            // 
            this.fxReceptorNombreTit.DataMember = "ReceptorComprobante";
            this.fxReceptorNombreTit.Expression = "Iif(IsNullOrEmpty([nombre]),\'\' ,\'Nombre:\' )";
            this.fxReceptorNombreTit.Name = "fxReceptorNombreTit";
            // 
            // xrTableCell109
            // 
            this.xrTableCell109.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell109.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell109.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.selloSAT")});
            this.xrTableCell109.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell109.Name = "xrTableCell109";
            this.xrTableCell109.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell109.StylePriority.UseBorderColor = false;
            this.xrTableCell109.StylePriority.UseBorders = false;
            this.xrTableCell109.StylePriority.UseFont = false;
            this.xrTableCell109.StylePriority.UsePadding = false;
            this.xrTableCell109.Weight = 7.8599995044549695D;
            // 
            // xrTableCell97
            // 
            this.xrTableCell97.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell97.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell97.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tblTotalesLeft});
            this.xrTableCell97.Name = "xrTableCell97";
            this.xrTableCell97.StylePriority.UseBorderColor = false;
            this.xrTableCell97.StylePriority.UseBorders = false;
            this.xrTableCell97.Weight = 4.0530999202489246D;
            // 
            // fxReceptorNombre
            // 
            this.fxReceptorNombre.DataMember = "ReceptorComprobante";
            this.fxReceptorNombre.Expression = "Iif(IsNullOrEmpty([nombre]),\'\' ,[nombre] )";
            this.fxReceptorNombre.Name = "fxReceptorNombre";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 30F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // fxSalarioDiario
            // 
            this.fxSalarioDiario.DataMember = "Receptor";
            this.fxSalarioDiario.Expression = "Iif(IsNullOrEmpty([SalarioDiarioIntegrado]), \'\' ,[SalarioDiarioIntegrado] )";
            this.fxSalarioDiario.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxSalarioDiario.Name = "fxSalarioDiario";
            // 
            // fxCuentaBancaria
            // 
            this.fxCuentaBancaria.DataMember = "Receptor";
            this.fxCuentaBancaria.Expression = "Iif(IsNullOrEmpty([CuentaBancaria]), \'\' ,[CuentaBancaria])";
            this.fxCuentaBancaria.Name = "fxCuentaBancaria";
            // 
            // GroupHeaderOtroPago
            // 
            this.GroupHeaderOtroPago.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable7});
            this.GroupHeaderOtroPago.HeightF = 30F;
            this.GroupHeaderOtroPago.KeepTogether = true;
            this.GroupHeaderOtroPago.Name = "GroupHeaderOtroPago";
            this.GroupHeaderOtroPago.RepeatEveryPage = true;
            // 
            // xrTable7
            // 
            this.xrTable7.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable7.Name = "xrTable7";
            this.xrTable7.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow22,
            this.xrTableRow29});
            this.xrTable7.SizeF = new System.Drawing.SizeF(630.4F, 30F);
            this.xrTable7.StylePriority.UseFont = false;
            this.xrTable7.StylePriority.UsePadding = false;
            this.xrTable7.Tag = "WithColor";
            // 
            // xrTableRow22
            // 
            this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell8});
            this.xrTableRow22.Name = "xrTableRow22";
            this.xrTableRow22.Weight = 11.499971850768331D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.BackColor = System.Drawing.Color.Black;
            this.xrTableCell8.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell8.ForeColor = System.Drawing.Color.White;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTableCell8.StylePriority.UseBackColor = false;
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.StylePriority.UseForeColor = false;
            this.xrTableCell8.StylePriority.UsePadding = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "Otros Pagos";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell8.Weight = 4.3671987693860874D;
            // 
            // xrTableRow29
            // 
            this.xrTableRow29.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell9,
            this.xrTableCell32,
            this.xrTableCell10});
            this.xrTableRow29.Name = "xrTableRow29";
            this.xrTableRow29.Weight = 11.499971850768331D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell11.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell11.ForeColor = System.Drawing.Color.White;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell11.StylePriority.UseBackColor = false;
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.StylePriority.UseForeColor = false;
            this.xrTableCell11.StylePriority.UsePadding = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "Tipo";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 0.20988919263242795D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.ForeColor = System.Drawing.Color.White;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell9.StylePriority.UseBackColor = false;
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseForeColor = false;
            this.xrTableCell9.StylePriority.UsePadding = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Clave";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.2982636446134142D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell32.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell32.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell32.ForeColor = System.Drawing.Color.White;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell32.StylePriority.UseBackColor = false;
            this.xrTableCell32.StylePriority.UseBorders = false;
            this.xrTableCell32.StylePriority.UseFont = false;
            this.xrTableCell32.StylePriority.UseForeColor = false;
            this.xrTableCell32.StylePriority.UsePadding = false;
            this.xrTableCell32.StylePriority.UseTextAlignment = false;
            this.xrTableCell32.Text = "Concepto";
            this.xrTableCell32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell32.Weight = 1.7343466439401518D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell10.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell10.ForeColor = System.Drawing.Color.White;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell10.StylePriority.UseBackColor = false;
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseForeColor = false;
            this.xrTableCell10.StylePriority.UsePadding = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Importe";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.37426560036884327D;
            // 
            // fxSalarioBaseTit
            // 
            this.fxSalarioBaseTit.DataMember = "Receptor";
            this.fxSalarioBaseTit.Expression = "Iif(IsNullOrEmpty([SalarioBaseCotApor]), \'\' ,\'Salario Base:\' )";
            this.fxSalarioBaseTit.Name = "fxSalarioBaseTit";
            // 
            // fxTotal
            // 
            this.fxTotal.DataMember = "Comprobante";
            this.fxTotal.Expression = "[total]";
            this.fxTotal.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxTotal.Name = "fxTotal";
            // 
            // fxFechaAutorizacion
            // 
            this.fxFechaAutorizacion.DataMember = "Complemento.Complemento_Donatarias";
            this.fxFechaAutorizacion.Expression = "Concat(\'Fecha de Aprobación: \',[fechaAutorizacion] )";
            this.fxFechaAutorizacion.Name = "fxFechaAutorizacion";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrLabel1,
            this.xrLabel8,
            this.xrPageInfo});
            this.PageFooter.Expanded = false;
            this.PageFooter.HeightF = 26F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Helvetica-Normal", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(55.75003F, 4.999987F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.NavigateUrl = "http://www.paxfaxturacion.com/";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(126F, 17.91668F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "http://www.paxfaxturacion.com/";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.CanGrow = false;
            this.xrLabel8.Font = new System.Drawing.Font("Helvetica-Normal", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(251F, 4.999987F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(306.9999F, 17.91668F);
            this.xrLabel8.StylePriority.UseBorders = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UsePadding = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Este documento es una representación impresa de un CFDI";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // fxRiesgoTit
            // 
            this.fxRiesgoTit.DataMember = "Receptor";
            this.fxRiesgoTit.Expression = "Iif(IsNullOrEmpty([RiesgoPuesto]),\'\' ,\'Riesgo Puesto:\')";
            this.fxRiesgoTit.Name = "fxRiesgoTit";
            // 
            // fxMontoRecursoPropio
            // 
            this.fxMontoRecursoPropio.DataMember = "EntidadSNCF";
            this.fxMontoRecursoPropio.Expression = "Iif(IsNullOrEmpty([MontoRecursoPropio]),\'\' ,[MontoRecursoPropio] )";
            this.fxMontoRecursoPropio.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxMontoRecursoPropio.Name = "fxMontoRecursoPropio";
            // 
            // fxNoIdentificacion
            // 
            this.fxNoIdentificacion.DataMember = "Conceptos.Conceptos_Concepto";
            this.fxNoIdentificacion.Expression = "Iif(IsNullOrEmpty([noIdentificacion]),\'\',[noIdentificacion] )";
            this.fxNoIdentificacion.Name = "fxNoIdentificacion";
            // 
            // xrTableCell96
            // 
            this.xrTableCell96.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell96.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell96.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCodeQRCFDI});
            this.xrTableCell96.Name = "xrTableCell96";
            this.xrTableCell96.StylePriority.UseBorderColor = false;
            this.xrTableCell96.StylePriority.UseBorders = false;
            this.xrTableCell96.Text = "xrTableCell96";
            this.xrTableCell96.Weight = 1.3700000040689948D;
            // 
            // fxDepto
            // 
            this.fxDepto.DataMember = "Receptor";
            this.fxDepto.Expression = "Iif(IsNullOrEmpty([Departamento]),\'\' ,[Departamento])";
            this.fxDepto.Name = "fxDepto";
            // 
            // fxImporteImpRetencionISR
            // 
            this.fxImporteImpRetencionISR.DataMember = "Impuestos.Impuestos_Retenciones.Retenciones_Retencion";
            this.fxImporteImpRetencionISR.Expression = "[][[impuesto]==\'ISR\'].Sum(ToDouble([importe]))";
            this.fxImporteImpRetencionISR.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteImpRetencionISR.Name = "fxImporteImpRetencionISR";
            // 
            // fxImporteImpRetencionIVA
            // 
            this.fxImporteImpRetencionIVA.DataMember = "Impuestos.Impuestos_Retenciones.Retenciones_Retencion";
            this.fxImporteImpRetencionIVA.Expression = "[][[impuesto]==\'IVA\'].Sum(ToDouble([importe]))";
            this.fxImporteImpRetencionIVA.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteImpRetencionIVA.Name = "fxImporteImpRetencionIVA";
            // 
            // xrTableRow46
            // 
            this.xrTableRow46.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellTotalTit,
            this.CellTotalValue});
            this.xrTableRow46.Name = "xrTableRow46";
            this.xrTableRow46.Weight = 0.60000000000000009D;
            // 
            // CellTotalTit
            // 
            this.CellTotalTit.BorderColor = System.Drawing.Color.Black;
            this.CellTotalTit.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.CellTotalTit.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellTotalTit.Name = "CellTotalTit";
            this.CellTotalTit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.CellTotalTit.StylePriority.UseBorderColor = false;
            this.CellTotalTit.StylePriority.UseBorders = false;
            this.CellTotalTit.StylePriority.UseFont = false;
            this.CellTotalTit.StylePriority.UsePadding = false;
            this.CellTotalTit.StylePriority.UseTextAlignment = false;
            this.CellTotalTit.Text = "TOTAL";
            this.CellTotalTit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellTotalTit.Weight = 1.3D;
            // 
            // CellTotalValue
            // 
            this.CellTotalValue.BorderColor = System.Drawing.Color.Black;
            this.CellTotalValue.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.CellTotalValue.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.fxTotal", "{0:$ #,###0.00}")});
            this.CellTotalValue.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellTotalValue.Name = "CellTotalValue";
            this.CellTotalValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.CellTotalValue.StylePriority.UseBorderColor = false;
            this.CellTotalValue.StylePriority.UseBorders = false;
            this.CellTotalValue.StylePriority.UseFont = false;
            this.CellTotalValue.StylePriority.UsePadding = false;
            this.CellTotalValue.StylePriority.UseTextAlignment = false;
            this.CellTotalValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.CellTotalValue.Weight = 1.1300000000000001D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 30F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // fxPuestoTit
            // 
            this.fxPuestoTit.DataMember = "Receptor";
            this.fxPuestoTit.Expression = "Iif(IsNullOrEmpty([Puesto]),\'\' ,\'Puesto\' )";
            this.fxPuestoTit.Name = "fxPuestoTit";
            // 
            // xrTable15
            // 
            this.xrTable15.BorderColor = System.Drawing.Color.Green;
            this.xrTable15.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTable15.LocationFloat = new DevExpress.Utils.PointFloat(0F, 114F);
            this.xrTable15.Name = "xrTable15";
            this.xrTable15.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow46});
            this.xrTable15.SizeF = new System.Drawing.SizeF(244F, 15F);
            this.xrTable15.StylePriority.UseBorderColor = false;
            this.xrTable15.StylePriority.UseBorders = false;
            this.xrTable15.Tag = "WithColor";
            // 
            // DetailJubilacion
            // 
            this.DetailJubilacion.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetJubilacion,
            this.GroupHeaderJubilacion});
            this.DetailJubilacion.DataMember = "JubilacionPensionRetiro";
            this.DetailJubilacion.Expanded = false;
            this.DetailJubilacion.Level = 3;
            this.DetailJubilacion.Name = "DetailJubilacion";
            this.DetailJubilacion.ReportPrintOptions.PrintOnEmptyDataSource = false;
            // 
            // DetJubilacion
            // 
            this.DetJubilacion.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable16});
            this.DetJubilacion.HeightF = 15F;
            this.DetJubilacion.Name = "DetJubilacion";
            // 
            // xrTable16
            // 
            this.xrTable16.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable16.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable16.Name = "xrTable16";
            this.xrTable16.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable16.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow37});
            this.xrTable16.SizeF = new System.Drawing.SizeF(787.9999F, 15F);
            this.xrTable16.StylePriority.UseFont = false;
            this.xrTable16.StylePriority.UsePadding = false;
            this.xrTable16.Tag = "WithColor";
            // 
            // xrTableRow37
            // 
            this.xrTableRow37.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell81,
            this.xrTableCell82,
            this.xrTableCell83,
            this.xrTableCell94,
            this.xrTableCell101});
            this.xrTableRow37.Name = "xrTableRow37";
            this.xrTableRow37.Weight = 11.499971850768331D;
            // 
            // xrTableCell81
            // 
            this.xrTableCell81.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell81.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell81.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "JubilacionPensionRetiro.fxTotalUnaExhibicion", "{0:$ #,###0.00}")});
            this.xrTableCell81.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell81.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell81.Name = "xrTableCell81";
            this.xrTableCell81.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell81.StylePriority.UseBackColor = false;
            this.xrTableCell81.StylePriority.UseBorders = false;
            this.xrTableCell81.StylePriority.UseFont = false;
            this.xrTableCell81.StylePriority.UseForeColor = false;
            this.xrTableCell81.StylePriority.UsePadding = false;
            this.xrTableCell81.StylePriority.UseTextAlignment = false;
            this.xrTableCell81.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell81.Weight = 0.87444443346918466D;
            // 
            // xrTableCell82
            // 
            this.xrTableCell82.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell82.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell82.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "JubilacionPensionRetiro.fxTotalParcialidad", "{0:$ #,###0.00}")});
            this.xrTableCell82.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell82.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell82.Name = "xrTableCell82";
            this.xrTableCell82.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell82.StylePriority.UseBackColor = false;
            this.xrTableCell82.StylePriority.UseBorders = false;
            this.xrTableCell82.StylePriority.UseFont = false;
            this.xrTableCell82.StylePriority.UseForeColor = false;
            this.xrTableCell82.StylePriority.UsePadding = false;
            this.xrTableCell82.StylePriority.UseTextAlignment = false;
            this.xrTableCell82.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell82.Weight = 0.874444418452236D;
            // 
            // xrTableCell83
            // 
            this.xrTableCell83.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell83.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell83.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "JubilacionPensionRetiro.fxMontoDiario", "{0:$ #,###0.00}")});
            this.xrTableCell83.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell83.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell83.Name = "xrTableCell83";
            this.xrTableCell83.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell83.StylePriority.UseBackColor = false;
            this.xrTableCell83.StylePriority.UseBorders = false;
            this.xrTableCell83.StylePriority.UseFont = false;
            this.xrTableCell83.StylePriority.UseForeColor = false;
            this.xrTableCell83.StylePriority.UsePadding = false;
            this.xrTableCell83.StylePriority.UseTextAlignment = false;
            this.xrTableCell83.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell83.Weight = 0.87444443346918466D;
            // 
            // xrTableCell94
            // 
            this.xrTableCell94.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell94.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell94.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "JubilacionPensionRetiro.fxJubIngresoAcumulable", "{0:$ #,###0.00}")});
            this.xrTableCell94.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell94.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell94.Name = "xrTableCell94";
            this.xrTableCell94.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell94.StylePriority.UseBackColor = false;
            this.xrTableCell94.StylePriority.UseBorders = false;
            this.xrTableCell94.StylePriority.UseFont = false;
            this.xrTableCell94.StylePriority.UseForeColor = false;
            this.xrTableCell94.StylePriority.UsePadding = false;
            this.xrTableCell94.StylePriority.UseTextAlignment = false;
            this.xrTableCell94.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell94.Weight = 0.87444443837446839D;
            // 
            // xrTableCell101
            // 
            this.xrTableCell101.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell101.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell101.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "JubilacionPensionRetiro.fxJubIngresoNoAcumulable", "{0:$ #,###0.00}")});
            this.xrTableCell101.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell101.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell101.Name = "xrTableCell101";
            this.xrTableCell101.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell101.StylePriority.UseBackColor = false;
            this.xrTableCell101.StylePriority.UseBorders = false;
            this.xrTableCell101.StylePriority.UseFont = false;
            this.xrTableCell101.StylePriority.UseForeColor = false;
            this.xrTableCell101.StylePriority.UsePadding = false;
            this.xrTableCell101.StylePriority.UseTextAlignment = false;
            this.xrTableCell101.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell101.Weight = 0.874444348668017D;
            // 
            // GroupHeaderJubilacion
            // 
            this.GroupHeaderJubilacion.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable6});
            this.GroupHeaderJubilacion.HeightF = 30F;
            this.GroupHeaderJubilacion.KeepTogether = true;
            this.GroupHeaderJubilacion.Name = "GroupHeaderJubilacion";
            this.GroupHeaderJubilacion.RepeatEveryPage = true;
            // 
            // xrTable6
            // 
            this.xrTable6.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow16,
            this.xrTableRow32});
            this.xrTable6.SizeF = new System.Drawing.SizeF(787.9999F, 30F);
            this.xrTable6.StylePriority.UseFont = false;
            this.xrTable6.StylePriority.UsePadding = false;
            this.xrTable6.Tag = "WithColor";
            // 
            // xrTableRow16
            // 
            this.xrTableRow16.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4});
            this.xrTableRow16.Name = "xrTableRow16";
            this.xrTableRow16.Weight = 11.499971850768331D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BackColor = System.Drawing.Color.Black;
            this.xrTableCell4.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.ForeColor = System.Drawing.Color.White;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell4.StylePriority.UseBackColor = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseForeColor = false;
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Jubilación, Pensiones o Haberes para el Retiro";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 4.3775552559404387D;
            // 
            // xrTableRow32
            // 
            this.xrTableRow32.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6,
            this.xrTableCell59,
            this.xrTableCell60,
            this.xrTableCell67,
            this.xrTableCell80});
            this.xrTableRow32.Name = "xrTableRow32";
            this.xrTableRow32.Weight = 11.499971850768331D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell6.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell6.ForeColor = System.Drawing.Color.White;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell6.StylePriority.UseBackColor = false;
            this.xrTableCell6.StylePriority.UseBorders = false;
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseForeColor = false;
            this.xrTableCell6.StylePriority.UsePadding = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Total una Exhibición";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell6.Weight = 0.87551110110150332D;
            // 
            // xrTableCell59
            // 
            this.xrTableCell59.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell59.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell59.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell59.ForeColor = System.Drawing.Color.White;
            this.xrTableCell59.Name = "xrTableCell59";
            this.xrTableCell59.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell59.StylePriority.UseBackColor = false;
            this.xrTableCell59.StylePriority.UseBorders = false;
            this.xrTableCell59.StylePriority.UseFont = false;
            this.xrTableCell59.StylePriority.UseForeColor = false;
            this.xrTableCell59.StylePriority.UsePadding = false;
            this.xrTableCell59.StylePriority.UseTextAlignment = false;
            this.xrTableCell59.Text = "Total Parcialidad";
            this.xrTableCell59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell59.Weight = 0.87551110110150332D;
            // 
            // xrTableCell60
            // 
            this.xrTableCell60.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell60.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell60.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell60.ForeColor = System.Drawing.Color.White;
            this.xrTableCell60.Name = "xrTableCell60";
            this.xrTableCell60.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell60.StylePriority.UseBackColor = false;
            this.xrTableCell60.StylePriority.UseBorders = false;
            this.xrTableCell60.StylePriority.UseFont = false;
            this.xrTableCell60.StylePriority.UseForeColor = false;
            this.xrTableCell60.StylePriority.UsePadding = false;
            this.xrTableCell60.StylePriority.UseTextAlignment = false;
            this.xrTableCell60.Text = "Monto Diario";
            this.xrTableCell60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell60.Weight = 0.87551110110150332D;
            // 
            // xrTableCell67
            // 
            this.xrTableCell67.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell67.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell67.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell67.ForeColor = System.Drawing.Color.White;
            this.xrTableCell67.Name = "xrTableCell67";
            this.xrTableCell67.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell67.StylePriority.UseBackColor = false;
            this.xrTableCell67.StylePriority.UseBorders = false;
            this.xrTableCell67.StylePriority.UseFont = false;
            this.xrTableCell67.StylePriority.UseForeColor = false;
            this.xrTableCell67.StylePriority.UsePadding = false;
            this.xrTableCell67.StylePriority.UseTextAlignment = false;
            this.xrTableCell67.Text = "Ingreso Acumulable";
            this.xrTableCell67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell67.Weight = 0.87551110600678717D;
            // 
            // xrTableCell80
            // 
            this.xrTableCell80.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell80.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell80.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell80.ForeColor = System.Drawing.Color.White;
            this.xrTableCell80.Name = "xrTableCell80";
            this.xrTableCell80.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell80.StylePriority.UseBackColor = false;
            this.xrTableCell80.StylePriority.UseBorders = false;
            this.xrTableCell80.StylePriority.UseFont = false;
            this.xrTableCell80.StylePriority.UseForeColor = false;
            this.xrTableCell80.StylePriority.UsePadding = false;
            this.xrTableCell80.StylePriority.UseTextAlignment = false;
            this.xrTableCell80.Text = "Ingreso No Acumulable";
            this.xrTableCell80.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell80.Weight = 0.87551084662914225D;
            // 
            // xrtblCellCadenaOriginal
            // 
            this.xrtblCellCadenaOriginal.BorderColor = System.Drawing.Color.Black;
            this.xrtblCellCadenaOriginal.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrtblCellCadenaOriginal.CanShrink = true;
            this.xrtblCellCadenaOriginal.Name = "xrtblCellCadenaOriginal";
            this.xrtblCellCadenaOriginal.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrtblCellCadenaOriginal.StylePriority.UseBorderColor = false;
            this.xrtblCellCadenaOriginal.StylePriority.UseBorders = false;
            this.xrtblCellCadenaOriginal.StylePriority.UsePadding = false;
            this.xrtblCellCadenaOriginal.Weight = 7.8599995044549695D;
            // 
            // xrTable5
            // 
            this.xrTable5.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow4,
            this.xrTableRow28});
            this.xrTable5.SizeF = new System.Drawing.SizeF(787.9999F, 30F);
            this.xrTable5.StylePriority.UseFont = false;
            this.xrTable5.StylePriority.UsePadding = false;
            this.xrTable5.Tag = "WithColor";
            // 
            // xrTableRow28
            // 
            this.xrTableRow28.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell37,
            this.xrTableCell34,
            this.xrTableCell36,
            this.xrTableCell35,
            this.xrTableCell19});
            this.xrTableRow28.Name = "xrTableRow28";
            this.xrTableRow28.Weight = 11.499971850768331D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell37.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell37.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell37.ForeColor = System.Drawing.Color.White;
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell37.StylePriority.UseBackColor = false;
            this.xrTableCell37.StylePriority.UseBorders = false;
            this.xrTableCell37.StylePriority.UseFont = false;
            this.xrTableCell37.StylePriority.UseForeColor = false;
            this.xrTableCell37.StylePriority.UsePadding = false;
            this.xrTableCell37.StylePriority.UseTextAlignment = false;
            this.xrTableCell37.Text = "Total Pagado";
            this.xrTableCell37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell37.Weight = 0.8737333449298148D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell34.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell34.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell34.ForeColor = System.Drawing.Color.White;
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell34.StylePriority.UseBackColor = false;
            this.xrTableCell34.StylePriority.UseBorders = false;
            this.xrTableCell34.StylePriority.UseFont = false;
            this.xrTableCell34.StylePriority.UseForeColor = false;
            this.xrTableCell34.StylePriority.UsePadding = false;
            this.xrTableCell34.StylePriority.UseTextAlignment = false;
            this.xrTableCell34.Text = "Años de Servicio";
            this.xrTableCell34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell34.Weight = 0.8737333449298148D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell36.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell36.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell36.ForeColor = System.Drawing.Color.White;
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell36.StylePriority.UseBackColor = false;
            this.xrTableCell36.StylePriority.UseBorders = false;
            this.xrTableCell36.StylePriority.UseFont = false;
            this.xrTableCell36.StylePriority.UseForeColor = false;
            this.xrTableCell36.StylePriority.UsePadding = false;
            this.xrTableCell36.StylePriority.UseTextAlignment = false;
            this.xrTableCell36.Text = "Último Sueldo Mensual";
            this.xrTableCell36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell36.Weight = 0.8737333449298148D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell35.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell35.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell35.ForeColor = System.Drawing.Color.White;
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell35.StylePriority.UseBackColor = false;
            this.xrTableCell35.StylePriority.UseBorders = false;
            this.xrTableCell35.StylePriority.UseFont = false;
            this.xrTableCell35.StylePriority.UseForeColor = false;
            this.xrTableCell35.StylePriority.UsePadding = false;
            this.xrTableCell35.StylePriority.UseTextAlignment = false;
            this.xrTableCell35.Text = "Ingreso Acumulable";
            this.xrTableCell35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell35.Weight = 0.87373334983509865D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell19.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell19.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell19.ForeColor = System.Drawing.Color.White;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell19.StylePriority.UseBackColor = false;
            this.xrTableCell19.StylePriority.UseBorders = false;
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseForeColor = false;
            this.xrTableCell19.StylePriority.UsePadding = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.Text = "Ingreso No Acumulable";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell19.Weight = 0.87373342952444566D;
            // 
            // fxIngresoAcumulable
            // 
            this.fxIngresoAcumulable.DataMember = "SeparacionIndemnizacion";
            this.fxIngresoAcumulable.Expression = "[IngresoAcumulable]";
            this.fxIngresoAcumulable.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxIngresoAcumulable.Name = "fxIngresoAcumulable";
            // 
            // fxDescripLocTra
            // 
            this.fxDescripLocTra.DataMember = "ImpuestosLocales.ImpuestosLocales_TrasladosLocales";
            this.fxDescripLocTra.Name = "fxDescripLocTra";
            // 
            // fxMontoDiario
            // 
            this.fxMontoDiario.DataMember = "JubilacionPensionRetiro";
            this.fxMontoDiario.Expression = "Iif(IsNullOrEmpty([MontoDiario]),0,[MontoDiario])";
            this.fxMontoDiario.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxMontoDiario.Name = "fxMontoDiario";
            // 
            // fxEmisorDomicilio
            // 
            this.fxEmisorDomicilio.DataMember = "Emisor.Emisor_DomicilioFiscal";
            this.fxEmisorDomicilio.Expression = resources.GetString("fxEmisorDomicilio.Expression");
            this.fxEmisorDomicilio.Name = "fxEmisorDomicilio";
            // 
            // fxImpRetencionISR
            // 
            this.fxImpRetencionISR.DataMember = "Impuestos.Impuestos_Retenciones.Retenciones_Retencion";
            this.fxImpRetencionISR.Expression = "Iif([][[impuesto] == \'ISR\'],\'ISR\' ,\'\')";
            this.fxImpRetencionISR.Name = "fxImpRetencionISR";
            // 
            // fxTotalPagado
            // 
            this.fxTotalPagado.DataMember = "SeparacionIndemnizacion";
            this.fxTotalPagado.Expression = "[TotalPagado]";
            this.fxTotalPagado.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxTotalPagado.Name = "fxTotalPagado";
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell16.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell16.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell16.StylePriority.UseBorderColor = false;
            this.xrTableCell16.StylePriority.UseBorders = false;
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UsePadding = false;
            this.xrTableCell16.Text = "Sello digital del SAT:";
            this.xrTableCell16.Weight = 7.8599995044549695D;
            // 
            // fxHorasExtraImporte
            // 
            this.fxHorasExtraImporte.DataMember = "HorasExtras.HorasExtras_HorasExtra";
            this.fxHorasExtraImporte.Expression = "[ImportePagado]";
            this.fxHorasExtraImporte.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxHorasExtraImporte.Name = "fxHorasExtraImporte";
            // 
            // xrTableRow18
            // 
            this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25});
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Weight = 0.11111109092753135D;
            // 
            // fxCuentaBancariaTit
            // 
            this.fxCuentaBancariaTit.DataMember = "Receptor";
            this.fxCuentaBancariaTit.Expression = "Iif(IsNullOrEmpty([CuentaBancaria]), \'\' ,\'Cuenta Bancaria:\' )";
            this.fxCuentaBancariaTit.Name = "fxCuentaBancariaTit";
            // 
            // fxTotalParcialidad
            // 
            this.fxTotalParcialidad.DataMember = "JubilacionPensionRetiro";
            this.fxTotalParcialidad.Expression = "Iif(IsNullOrEmpty([TotalParcialidad]),0,[TotalParcialidad] )";
            this.fxTotalParcialidad.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxTotalParcialidad.Name = "fxTotalParcialidad";
            // 
            // xrTableRow61
            // 
            this.xrTableRow61.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell109});
            this.xrTableRow61.Name = "xrTableRow61";
            this.xrTableRow61.Weight = 0.11111109092753135D;
            // 
            // fxSalarioDiarioTit
            // 
            this.fxSalarioDiarioTit.DataMember = "Receptor";
            this.fxSalarioDiarioTit.Expression = "Iif(IsNullOrEmpty([SalarioDiarioIntegrado]), \'\' ,\'Salario Diario:\' )";
            this.fxSalarioDiarioTit.Name = "fxSalarioDiarioTit";
            // 
            // fxPuesto
            // 
            this.fxPuesto.DataMember = "Receptor";
            this.fxPuesto.Expression = "Iif(IsNullOrEmpty([Puesto]),\'\' ,[Puesto])";
            this.fxPuesto.Name = "fxPuesto";
            // 
            // fxTotalUnaExhibicion
            // 
            this.fxTotalUnaExhibicion.DataMember = "JubilacionPensionRetiro";
            this.fxTotalUnaExhibicion.Expression = "Iif(IsNullOrEmpty([TotalUnaExhibicion]),0,[TotalUnaExhibicion])";
            this.fxTotalUnaExhibicion.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxTotalUnaExhibicion.Name = "fxTotalUnaExhibicion";
            // 
            // fxUUID
            // 
            this.fxUUID.DataMember = "TimbreFiscalDigital";
            this.fxUUID.Expression = "Upper([UUID])";
            this.fxUUID.Name = "fxUUID";
            // 
            // fxDomicilioExpedidoEn
            // 
            this.fxDomicilioExpedidoEn.DataMember = "Emisor.Emisor_ExpedidoEn";
            this.fxDomicilioExpedidoEn.Expression = resources.GetString("fxDomicilioExpedidoEn.Expression");
            this.fxDomicilioExpedidoEn.Name = "fxDomicilioExpedidoEn";
            // 
            // LineFooterConceptos
            // 
            this.LineFooterConceptos.BorderColor = System.Drawing.Color.Green;
            this.LineFooterConceptos.ForeColor = System.Drawing.Color.Black;
            this.LineFooterConceptos.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.LineFooterConceptos.Name = "LineFooterConceptos";
            this.LineFooterConceptos.SizeF = new System.Drawing.SizeF(788F, 2F);
            this.LineFooterConceptos.StylePriority.UseBorderColor = false;
            this.LineFooterConceptos.StylePriority.UseForeColor = false;
            this.LineFooterConceptos.Tag = "WithColor";
            this.LineFooterConceptos.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.LineFooterConceptos_PrintOnPage);
            // 
            // fxJubIngresoNoAcumulable
            // 
            this.fxJubIngresoNoAcumulable.DataMember = "JubilacionPensionRetiro";
            this.fxJubIngresoNoAcumulable.Expression = "[IngresoNoAcumulable]";
            this.fxJubIngresoNoAcumulable.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxJubIngresoNoAcumulable.Name = "fxJubIngresoNoAcumulable";
            // 
            // fxDeptoTit
            // 
            this.fxDeptoTit.DataMember = "Receptor";
            this.fxDeptoTit.Expression = "Iif(IsNullOrEmpty([Departamento]),\'\' ,\'Departamento:\')";
            this.fxDeptoTit.Name = "fxDeptoTit";
            // 
            // fxNumCtaPago
            // 
            this.fxNumCtaPago.DataMember = "Comprobante";
            this.fxNumCtaPago.Expression = "Iif(IsNullOrEmpty([NumCtaPago]), \'\' ,[NumCtaPago] )";
            this.fxNumCtaPago.Name = "fxNumCtaPago";
            // 
            // xrTableCell110
            // 
            this.xrTableCell110.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell110.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell110.Name = "xrTableCell110";
            this.xrTableCell110.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell110.StylePriority.UseBorderColor = false;
            this.xrTableCell110.StylePriority.UseBorders = false;
            this.xrTableCell110.StylePriority.UsePadding = false;
            this.xrTableCell110.Weight = 7.8599995044549695D;
            // 
            // GroupFooterLine
            // 
            this.GroupFooterLine.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.LineFooterConceptos});
            this.GroupFooterLine.HeightF = 2F;
            this.GroupFooterLine.Name = "GroupFooterLine";
            this.GroupFooterLine.PrintAtBottom = true;
            this.GroupFooterLine.RepeatEveryPage = true;
            // 
            // fxTipoJornada
            // 
            this.fxTipoJornada.DataMember = "Receptor";
            this.fxTipoJornada.Expression = "Iif(IsNullOrEmpty([TipoJornada]),\'\' ,[TipoJornada] )";
            this.fxTipoJornada.Name = "fxTipoJornada";
            // 
            // DetailOtroPago
            // 
            this.DetailOtroPago.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetOtroPago,
            this.GroupHeaderOtroPago,
            this.DetailSubEmpleo,
            this.DetailSaldosAFavor});
            this.DetailOtroPago.DataMember = "OtroPago";
            this.DetailOtroPago.Expanded = false;
            this.DetailOtroPago.Level = 5;
            this.DetailOtroPago.Name = "DetailOtroPago";
            this.DetailOtroPago.ReportPrintOptions.PrintOnEmptyDataSource = false;
            // 
            // xrTableRow60
            // 
            this.xrTableRow60.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell16});
            this.xrTableRow60.Name = "xrTableRow60";
            this.xrTableRow60.Weight = 0.11111109092753135D;
            // 
            // fxEstado
            // 
            this.fxEstado.DataMember = "Receptor";
            this.fxEstado.Expression = "Iif(IsNullOrEmpty([ClaveEntFed]),\'\' ,[ClaveEntFed] )";
            this.fxEstado.Name = "fxEstado";
            // 
            // CellSubTotalTit
            // 
            this.CellSubTotalTit.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellSubTotalTit.CanShrink = true;
            this.CellSubTotalTit.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Nomina.fxTotalPercepcionesTit")});
            this.CellSubTotalTit.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellSubTotalTit.Name = "CellSubTotalTit";
            this.CellSubTotalTit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 4, 0, 100F);
            this.CellSubTotalTit.StylePriority.UseBorders = false;
            this.CellSubTotalTit.StylePriority.UseFont = false;
            this.CellSubTotalTit.StylePriority.UsePadding = false;
            this.CellSubTotalTit.StylePriority.UseTextAlignment = false;
            this.CellSubTotalTit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellSubTotalTit.Weight = 1.3D;
            // 
            // fxImporteConcepto
            // 
            this.fxImporteConcepto.DataMember = "Conceptos.Conceptos_Concepto";
            this.fxImporteConcepto.Expression = "[importe]";
            this.fxImporteConcepto.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteConcepto.Name = "fxImporteConcepto";
            // 
            // fxTipoJornadaTit
            // 
            this.fxTipoJornadaTit.DataMember = "Receptor";
            this.fxTipoJornadaTit.Expression = "Iif(IsNullOrEmpty([TipoJornada]),\'\' ,\'Tipo Jornada:\' )";
            this.fxTipoJornadaTit.Name = "fxTipoJornadaTit";
            // 
            // xrTable14
            // 
            this.xrTable14.BorderColor = System.Drawing.Color.Black;
            this.xrTable14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable14.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable14.Name = "xrTable14";
            this.xrTable14.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow43,
            this.xrTableRow63,
            this.xrTableRow44,
            this.xrTableRow15,
            this.xrTableRow60,
            this.xrTableRow61,
            this.xrTableRow18,
            this.xrTableRow62,
            this.xrTableRow42,
            this.xrTableRow45});
            this.xrTable14.SizeF = new System.Drawing.SizeF(788F, 251F);
            this.xrTable14.StylePriority.UseBorderColor = false;
            this.xrTable14.StylePriority.UseBorders = false;
            this.xrTable14.Tag = "WithColor";
            // 
            // xrTableRow43
            // 
            this.xrTableRow43.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell96,
            this.xrTableCell97,
            this.xrTableCell98});
            this.xrTableRow43.Name = "xrTableRow43";
            this.xrTableRow43.Weight = 1.0158729543861449D;
            // 
            // xrTableCell98
            // 
            this.xrTableCell98.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell98.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell98.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable15,
            this.tblTotalesRight});
            this.xrTableCell98.Name = "xrTableCell98";
            this.xrTableCell98.StylePriority.UseBorderColor = false;
            this.xrTableCell98.StylePriority.UseBorders = false;
            this.xrTableCell98.StylePriority.UseTextAlignment = false;
            this.xrTableCell98.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            this.xrTableCell98.Weight = 2.436899580137049D;
            // 
            // tblTotalesRight
            // 
            this.tblTotalesRight.BorderColor = System.Drawing.Color.Green;
            this.tblTotalesRight.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.tblTotalesRight.LocationFloat = new DevExpress.Utils.PointFloat(0.3100675F, 2F);
            this.tblTotalesRight.Name = "tblTotalesRight";
            this.tblTotalesRight.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.RowSubtotal,
            this.RowDescuento,
            this.xrTableRow40});
            this.tblTotalesRight.SizeF = new System.Drawing.SizeF(242F, 42F);
            this.tblTotalesRight.StylePriority.UseBorderColor = false;
            this.tblTotalesRight.StylePriority.UseBorders = false;
            // 
            // RowSubtotal
            // 
            this.RowSubtotal.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellSubTotalTit,
            this.CellSubTotalValue});
            this.RowSubtotal.Name = "RowSubtotal";
            this.RowSubtotal.Weight = 0.56D;
            // 
            // RowDescuento
            // 
            this.RowDescuento.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellDescuentoTit,
            this.CellDescuentoValue});
            this.RowDescuento.Name = "RowDescuento";
            this.RowDescuento.Weight = 0.56D;
            // 
            // xrTableRow40
            // 
            this.xrTableRow40.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell26,
            this.xrTableCell61});
            this.xrTableRow40.Name = "xrTableRow40";
            this.xrTableRow40.Weight = 0.56D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell26.CanShrink = true;
            this.xrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Nomina.fxTotalDeduccionesTit")});
            this.xrTableCell26.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTableCell26.StylePriority.UseBorders = false;
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.StylePriority.UsePadding = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell26.Weight = 1.3D;
            // 
            // xrTableCell61
            // 
            this.xrTableCell61.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell61.CanShrink = true;
            this.xrTableCell61.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Nomina.fxTotalDeducciones", "{0:$ #,###0.00}")});
            this.xrTableCell61.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell61.Name = "xrTableCell61";
            this.xrTableCell61.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTableCell61.StylePriority.UseBorders = false;
            this.xrTableCell61.StylePriority.UseFont = false;
            this.xrTableCell61.StylePriority.UsePadding = false;
            this.xrTableCell61.StylePriority.UseTextAlignment = false;
            this.xrTableCell61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell61.Weight = 1.12D;
            // 
            // xrTableRow63
            // 
            this.xrTableRow63.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell110});
            this.xrTableRow63.Name = "xrTableRow63";
            this.xrTableRow63.Weight = 0.095238136541386312D;
            // 
            // xrTableRow62
            // 
            this.xrTableRow62.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrtblCellCadenaOriginal});
            this.xrTableRow62.Name = "xrTableRow62";
            this.xrTableRow62.Weight = 0.11111109092753135D;
            // 
            // xrTableRow42
            // 
            this.xrTableRow42.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell105,
            this.xrTableCell104,
            this.xrTableCell75});
            this.xrTableRow42.Name = "xrTableRow42";
            this.xrTableRow42.Weight = 0.11111109092753135D;
            // 
            // xrTableCell105
            // 
            this.xrTableCell105.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell105.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell105.Name = "xrTableCell105";
            this.xrTableCell105.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell105.StylePriority.UseBorderColor = false;
            this.xrTableCell105.StylePriority.UseBorders = false;
            this.xrTableCell105.StylePriority.UsePadding = false;
            this.xrTableCell105.Text = "Certificado del Emisor: ";
            this.xrTableCell105.Weight = 1.2268784757400346D;
            // 
            // xrTableCell104
            // 
            this.xrTableCell104.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell104.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell104.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.NoCertificado")});
            this.xrTableCell104.Name = "xrTableCell104";
            this.xrTableCell104.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell104.StylePriority.UseBorderColor = false;
            this.xrTableCell104.StylePriority.UseBorders = false;
            this.xrTableCell104.StylePriority.UsePadding = false;
            this.xrTableCell104.Weight = 2.70312127648745D;
            // 
            // xrTableCell75
            // 
            this.xrTableCell75.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell75.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell75.Name = "xrTableCell75";
            this.xrTableCell75.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell75.StylePriority.UseBorderColor = false;
            this.xrTableCell75.StylePriority.UseBorders = false;
            this.xrTableCell75.StylePriority.UsePadding = false;
            this.xrTableCell75.Weight = 3.9299997522274848D;
            // 
            // xrTableRow45
            // 
            this.xrTableRow45.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell107,
            this.xrTableCell106,
            this.xrTableCell108,
            this.xrTableCell103});
            this.xrTableRow45.Name = "xrTableRow45";
            this.xrTableRow45.Weight = 0.11111109092753135D;
            // 
            // xrTableCell107
            // 
            this.xrTableCell107.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell107.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell107.Name = "xrTableCell107";
            this.xrTableCell107.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell107.StylePriority.UseBorderColor = false;
            this.xrTableCell107.StylePriority.UseBorders = false;
            this.xrTableCell107.StylePriority.UsePadding = false;
            this.xrTableCell107.Text = "Certificado SAT:";
            this.xrTableCell107.Weight = 1.2268784757400346D;
            // 
            // xrTableCell106
            // 
            this.xrTableCell106.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell106.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell106.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.NoCertificadoSAT")});
            this.xrTableCell106.Name = "xrTableCell106";
            this.xrTableCell106.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell106.StylePriority.UseBorderColor = false;
            this.xrTableCell106.StylePriority.UseBorders = false;
            this.xrTableCell106.StylePriority.UsePadding = false;
            this.xrTableCell106.Weight = 2.70312127648745D;
            // 
            // xrTableCell108
            // 
            this.xrTableCell108.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell108.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell108.Name = "xrTableCell108";
            this.xrTableCell108.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell108.StylePriority.UseBorderColor = false;
            this.xrTableCell108.StylePriority.UseBorders = false;
            this.xrTableCell108.StylePriority.UsePadding = false;
            this.xrTableCell108.Text = "Proveedor de Certificación:";
            this.xrTableCell108.Weight = 1.3864719938061429D;
            // 
            // xrTableCell103
            // 
            this.xrTableCell103.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell103.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell103.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.RfcProvCertif")});
            this.xrTableCell103.Name = "xrTableCell103";
            this.xrTableCell103.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrTableCell103.StylePriority.UseBorderColor = false;
            this.xrTableCell103.StylePriority.UseBorders = false;
            this.xrTableCell103.StylePriority.UsePadding = false;
            this.xrTableCell103.Weight = 2.5435277584213414D;
            // 
            // fxTotalPercepciones
            // 
            this.fxTotalPercepciones.DataMember = "Nomina";
            this.fxTotalPercepciones.Expression = "Iif(IsNullOrEmpty([TotalPercepciones]),\'\' ,[TotalPercepciones])";
            this.fxTotalPercepciones.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxTotalPercepciones.Name = "fxTotalPercepciones";
            // 
            // fxSubsidioCausado
            // 
            this.fxSubsidioCausado.DataMember = "OtroPago.OtroPago_SubsidioAlEmpleo";
            this.fxSubsidioCausado.Expression = "[SubsidioCausado]";
            this.fxSubsidioCausado.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxSubsidioCausado.Name = "fxSubsidioCausado";
            // 
            // fxAntiguedad
            // 
            this.fxAntiguedad.DataMember = "Receptor";
            this.fxAntiguedad.Expression = "Iif(IsNullOrEmpty([Antigüedad]), \'\' ,[Antigüedad])";
            this.fxAntiguedad.Name = "fxAntiguedad";
            // 
            // GroupHeaderSepIndemnizacion
            // 
            this.GroupHeaderSepIndemnizacion.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable5});
            this.GroupHeaderSepIndemnizacion.HeightF = 30F;
            this.GroupHeaderSepIndemnizacion.KeepTogether = true;
            this.GroupHeaderSepIndemnizacion.Name = "GroupHeaderSepIndemnizacion";
            this.GroupHeaderSepIndemnizacion.RepeatEveryPage = true;
            // 
            // fxRiesgo
            // 
            this.fxRiesgo.DataMember = "Receptor";
            this.fxRiesgo.Expression = "Iif(IsNullOrEmpty([RiesgoPuesto]),\'\' ,[RiesgoPuesto])";
            this.fxRiesgo.Name = "fxRiesgo";
            // 
            // fxMontoRecursoPropioTit
            // 
            this.fxMontoRecursoPropioTit.DataMember = "EntidadSNCF";
            this.fxMontoRecursoPropioTit.Expression = "Iif(IsNullOrEmpty([MontoRecursoPropio]),\'\' ,\'Monto del Recurso Propio:\' )";
            this.fxMontoRecursoPropioTit.Name = "fxMontoRecursoPropioTit";
            // 
            // fxIMSSTit
            // 
            this.fxIMSSTit.DataMember = "Receptor";
            this.fxIMSSTit.Expression = "Iif(IsNullOrEmpty([NumSeguridadSocial]),\'\' ,\'N.S.S. :\')";
            this.fxIMSSTit.Name = "fxIMSSTit";
            // 
            // fxUltimoSueldo
            // 
            this.fxUltimoSueldo.DataMember = "SeparacionIndemnizacion";
            this.fxUltimoSueldo.Expression = "[UltimoSueldoMensOrd]";
            this.fxUltimoSueldo.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxUltimoSueldo.Name = "fxUltimoSueldo";
            // 
            // DetailSepIndemnizacion
            // 
            this.DetailSepIndemnizacion.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetSepIndemnizacion,
            this.GroupHeaderSepIndemnizacion});
            this.DetailSepIndemnizacion.DataMember = "SeparacionIndemnizacion";
            this.DetailSepIndemnizacion.Expanded = false;
            this.DetailSepIndemnizacion.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DetailSepIndemnizacion.Level = 2;
            this.DetailSepIndemnizacion.Name = "DetailSepIndemnizacion";
            this.DetailSepIndemnizacion.ReportPrintOptions.PrintOnEmptyDataSource = false;
            // 
            // DetSepIndemnizacion
            // 
            this.DetSepIndemnizacion.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable13});
            this.DetSepIndemnizacion.HeightF = 15F;
            this.DetSepIndemnizacion.Name = "DetSepIndemnizacion";
            // 
            // xrTable13
            // 
            this.xrTable13.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable13.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable13.Name = "xrTable13";
            this.xrTable13.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable13.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow34});
            this.xrTable13.SizeF = new System.Drawing.SizeF(787.9999F, 15F);
            this.xrTable13.StylePriority.UseFont = false;
            this.xrTable13.StylePriority.UsePadding = false;
            this.xrTable13.Tag = "WithColor";
            // 
            // xrTableRow34
            // 
            this.xrTableRow34.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell38,
            this.xrTableCell42,
            this.xrTableCell56,
            this.xrTableCell58});
            this.xrTableRow34.Name = "xrTableRow34";
            this.xrTableRow34.Weight = 11.499971850768331D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SeparacionIndemnizacion.fxTotalPagado", "{0:$ #,###0.00}")});
            this.xrTableCell5.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell5.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell5.StylePriority.UseBackColor = false;
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseForeColor = false;
            this.xrTableCell5.StylePriority.UsePadding = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell5.Weight = 0.8737333449298148D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell38.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SeparacionIndemnizacion.NumAñosServicio", "{0:$ #,###0.00}")});
            this.xrTableCell38.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell38.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell38.StylePriority.UseBackColor = false;
            this.xrTableCell38.StylePriority.UseBorders = false;
            this.xrTableCell38.StylePriority.UseFont = false;
            this.xrTableCell38.StylePriority.UseForeColor = false;
            this.xrTableCell38.StylePriority.UsePadding = false;
            this.xrTableCell38.StylePriority.UseTextAlignment = false;
            this.xrTableCell38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell38.Weight = 0.8737333449298148D;
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell42.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell42.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SeparacionIndemnizacion.fxUltimoSueldo", "{0:$ #,###0.00}")});
            this.xrTableCell42.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell42.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell42.StylePriority.UseBackColor = false;
            this.xrTableCell42.StylePriority.UseBorders = false;
            this.xrTableCell42.StylePriority.UseFont = false;
            this.xrTableCell42.StylePriority.UseForeColor = false;
            this.xrTableCell42.StylePriority.UsePadding = false;
            this.xrTableCell42.StylePriority.UseTextAlignment = false;
            this.xrTableCell42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell42.Weight = 0.8737333449298148D;
            // 
            // xrTableCell56
            // 
            this.xrTableCell56.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell56.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell56.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SeparacionIndemnizacion.fxIngresoAcumulable", "{0:$ #,###0.00}")});
            this.xrTableCell56.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell56.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell56.Name = "xrTableCell56";
            this.xrTableCell56.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell56.StylePriority.UseBackColor = false;
            this.xrTableCell56.StylePriority.UseBorders = false;
            this.xrTableCell56.StylePriority.UseFont = false;
            this.xrTableCell56.StylePriority.UseForeColor = false;
            this.xrTableCell56.StylePriority.UsePadding = false;
            this.xrTableCell56.StylePriority.UseTextAlignment = false;
            this.xrTableCell56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell56.Weight = 0.87373334983509865D;
            // 
            // xrTableCell58
            // 
            this.xrTableCell58.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell58.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell58.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SeparacionIndemnizacion.fxIngresoNoAcumulable", "{0:$ #,###0.00}")});
            this.xrTableCell58.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell58.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell58.Name = "xrTableCell58";
            this.xrTableCell58.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell58.StylePriority.UseBackColor = false;
            this.xrTableCell58.StylePriority.UseBorders = false;
            this.xrTableCell58.StylePriority.UseFont = false;
            this.xrTableCell58.StylePriority.UseForeColor = false;
            this.xrTableCell58.StylePriority.UsePadding = false;
            this.xrTableCell58.StylePriority.UseTextAlignment = false;
            this.xrTableCell58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell58.Weight = 0.87373334492981458D;
            // 
            // fxSubTotal
            // 
            this.fxSubTotal.DataMember = "Comprobante";
            this.fxSubTotal.Expression = "[subTotal]";
            this.fxSubTotal.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxSubTotal.Name = "fxSubTotal";
            // 
            // GroupFooterSubtotales
            // 
            this.GroupFooterSubtotales.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable14});
            this.GroupFooterSubtotales.HeightF = 251F;
            this.GroupFooterSubtotales.KeepTogether = true;
            this.GroupFooterSubtotales.Level = 1;
            this.GroupFooterSubtotales.Name = "GroupFooterSubtotales";
            this.GroupFooterSubtotales.PrintAtBottom = true;
            // 
            // fxCantidad
            // 
            this.fxCantidad.DataMember = "Conceptos.Conceptos_Concepto";
            this.fxCantidad.Expression = "[cantidad]";
            this.fxCantidad.FieldType = DevExpress.XtraReports.UI.FieldType.Decimal;
            this.fxCantidad.Name = "fxCantidad";
            // 
            // fxImpRetencionIVA
            // 
            this.fxImpRetencionIVA.DataMember = "Impuestos.Impuestos_Retenciones.Retenciones_Retencion";
            this.fxImpRetencionIVA.Expression = "Iif([][[impuesto] == \'IVA\'],\'IVA\' ,\'\')";
            this.fxImpRetencionIVA.Name = "fxImpRetencionIVA";
            // 
            // fxFecha
            // 
            this.fxFecha.DataMember = "Comprobante";
            this.fxFecha.Expression = "[fecha]";
            this.fxFecha.FieldType = DevExpress.XtraReports.UI.FieldType.DateTime;
            this.fxFecha.Name = "fxFecha";
            // 
            // fxValorUnitario
            // 
            this.fxValorUnitario.DataMember = "Conceptos.Conceptos_Concepto";
            this.fxValorUnitario.Expression = "[valorUnitario]";
            this.fxValorUnitario.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxValorUnitario.Name = "fxValorUnitario";
            // 
            // fxFechaInicioRelLaboralTit
            // 
            this.fxFechaInicioRelLaboralTit.DataMember = "Receptor";
            this.fxFechaInicioRelLaboralTit.Expression = "Iif(IsNullOrEmpty([FechaInicioRelLaboral]), \'\' ,\'Fecha de Ingreso:\' )";
            this.fxFechaInicioRelLaboralTit.Name = "fxFechaInicioRelLaboralTit";
            // 
            // fxFechaInicioRelLaboral
            // 
            this.fxFechaInicioRelLaboral.DataMember = "Receptor";
            this.fxFechaInicioRelLaboral.Expression = "Iif(IsNullOrEmpty([FechaInicioRelLaboral]), \'\' ,[FechaInicioRelLaboral])";
            this.fxFechaInicioRelLaboral.FieldType = DevExpress.XtraReports.UI.FieldType.DateTime;
            this.fxFechaInicioRelLaboral.Name = "fxFechaInicioRelLaboral";
            // 
            // fxPeriodoDePago
            // 
            this.fxPeriodoDePago.DataMember = "Nomina";
            this.fxPeriodoDePago.Expression = resources.GetString("fxPeriodoDePago.Expression");
            this.fxPeriodoDePago.FieldType = DevExpress.XtraReports.UI.FieldType.String;
            this.fxPeriodoDePago.Name = "fxPeriodoDePago";
            // 
            // fxFechaInicialPago
            // 
            this.fxFechaInicialPago.DataMember = "Nomina";
            this.fxFechaInicialPago.Expression = "[FechaInicialPago]";
            this.fxFechaInicialPago.FieldType = DevExpress.XtraReports.UI.FieldType.DateTime;
            this.fxFechaInicialPago.Name = "fxFechaInicialPago";
            // 
            // fxFechaFinalPago
            // 
            this.fxFechaFinalPago.DataMember = "Nomina";
            this.fxFechaFinalPago.Expression = "[FechaFinalPago]";
            this.fxFechaFinalPago.FieldType = DevExpress.XtraReports.UI.FieldType.DateTime;
            this.fxFechaFinalPago.Name = "fxFechaFinalPago";
            // 
            // fxFechaPago
            // 
            this.fxFechaPago.DataMember = "Nomina";
            this.fxFechaPago.Expression = "[FechaPago]";
            this.fxFechaPago.FieldType = DevExpress.XtraReports.UI.FieldType.DateTime;
            this.fxFechaPago.Name = "fxFechaPago";
            // 
            // fxNumDiasPagados
            // 
            this.fxNumDiasPagados.DataMember = "Nomina";
            this.fxNumDiasPagados.Expression = "[NumDiasPagados]";
            this.fxNumDiasPagados.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxNumDiasPagados.Name = "fxNumDiasPagados";
            // 
            // CrossLineLeft
            // 
            this.CrossLineLeft.EndBand = this.PageFooter;
            this.CrossLineLeft.EndPointFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.CrossLineLeft.ForeColor = System.Drawing.Color.Black;
            this.CrossLineLeft.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.CrossLineLeft.Name = "CrossLineLeft";
            this.CrossLineLeft.StartBand = this.ReportHeader;
            this.CrossLineLeft.StartPointFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.CrossLineLeft.Tag = "WithColor";
            this.CrossLineLeft.WidthF = 1F;
            // 
            // CrossLineRight
            // 
            this.CrossLineRight.EndBand = this.PageFooter;
            this.CrossLineRight.EndPointFloat = new DevExpress.Utils.PointFloat(789F, 0F);
            this.CrossLineRight.ForeColor = System.Drawing.Color.Black;
            this.CrossLineRight.LocationFloat = new DevExpress.Utils.PointFloat(789F, 0F);
            this.CrossLineRight.Name = "CrossLineRight";
            this.CrossLineRight.StartBand = this.ReportHeader;
            this.CrossLineRight.StartPointFloat = new DevExpress.Utils.PointFloat(789F, 0F);
            this.CrossLineRight.Tag = "WithColor";
            this.CrossLineRight.WidthF = 1F;
            // 
            // fxMetodoPago
            // 
            this.fxMetodoPago.DataMember = "Comprobante";
            this.fxMetodoPago.Expression = "Iif(IsNullOrEmpty([MetodoPago]), \'\' ,[MetodoPago] )";
            this.fxMetodoPago.Name = "fxMetodoPago";
            // 
            // fxRegistroPatronalTit
            // 
            this.fxRegistroPatronalTit.DataMember = "Emisor";
            this.fxRegistroPatronalTit.Expression = "Iif(IsNullOrEmpty([RegistroPatronal]), \'\' ,\'Registro Patronal:\' )";
            this.fxRegistroPatronalTit.Name = "fxRegistroPatronalTit";
            // 
            // fxRegistroPatronal
            // 
            this.fxRegistroPatronal.DataMember = "Emisor";
            this.fxRegistroPatronal.Expression = "Iif(IsNullOrEmpty([RegistroPatronal]), \'\' ,[RegistroPatronal] )";
            this.fxRegistroPatronal.Name = "fxRegistroPatronal";
            // 
            // xrCrossBandCenter
            // 
            this.xrCrossBandCenter.EndBand = this.SubBandHeader3;
            this.xrCrossBandCenter.EndPointFloat = new DevExpress.Utils.PointFloat(395F, 2F);
            this.xrCrossBandCenter.ForeColor = System.Drawing.Color.Black;
            this.xrCrossBandCenter.LocationFloat = new DevExpress.Utils.PointFloat(395F, 0F);
            this.xrCrossBandCenter.Name = "xrCrossBandCenter";
            this.xrCrossBandCenter.StartBand = this.SubBandHeader1;
            this.xrCrossBandCenter.StartPointFloat = new DevExpress.Utils.PointFloat(395F, 0F);
            this.xrCrossBandCenter.Tag = "WithColor";
            this.xrCrossBandCenter.WidthF = 1F;
            // 
            // fxEmisorCURPTit
            // 
            this.fxEmisorCURPTit.DataMember = "Emisor";
            this.fxEmisorCURPTit.Expression = "Iif(IsNullOrEmpty([Curp]), \'\' ,\'CURP:\' )";
            this.fxEmisorCURPTit.Name = "fxEmisorCURPTit";
            // 
            // fxEmisorCURP
            // 
            this.fxEmisorCURP.DataMember = "Emisor";
            this.fxEmisorCURP.Expression = "Iif(IsNullOrEmpty([Curp]), \'\', [Curp] )";
            this.fxEmisorCURP.Name = "fxEmisorCURP";
            // 
            // fxFechaTimbrado
            // 
            this.fxFechaTimbrado.DataMember = "TimbreFiscalDigital";
            this.fxFechaTimbrado.Expression = "[FechaTimbrado]";
            this.fxFechaTimbrado.FieldType = DevExpress.XtraReports.UI.FieldType.DateTime;
            this.fxFechaTimbrado.Name = "fxFechaTimbrado";
            // 
            // fxSerieFolio
            // 
            this.fxSerieFolio.DataMember = "Comprobante";
            this.fxSerieFolio.Expression = "Concat(\r\nIif(IsNullOrEmpty([Serie]),\'\',Concat([Serie])) ,\' \',\r\nIif(IsNullOrEmpty(" +
    "[Folio]),\'\',Concat([Folio])))";
            this.fxSerieFolio.Name = "fxSerieFolio";
            // 
            // fxConfirmacion
            // 
            this.fxConfirmacion.DataMember = "Comprobante";
            this.fxConfirmacion.Expression = "Iif(IsNullOrEmpty([Confirmacion]), \'\' ,[Confirmacion] )";
            this.fxConfirmacion.Name = "fxConfirmacion";
            // 
            // DetailAcciones
            // 
            this.DetailAcciones.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetAcciones,
            this.GroupHeaderAcciones});
            this.DetailAcciones.DataMember = "AccionesOTitulos";
            this.DetailAcciones.Expanded = false;
            this.DetailAcciones.Level = 4;
            this.DetailAcciones.Name = "DetailAcciones";
            this.DetailAcciones.ReportPrintOptions.PrintOnEmptyDataSource = false;
            // 
            // DetAcciones
            // 
            this.DetAcciones.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable12});
            this.DetAcciones.HeightF = 15F;
            this.DetAcciones.Name = "DetAcciones";
            // 
            // xrTable12
            // 
            this.xrTable12.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable12.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable12.Name = "xrTable12";
            this.xrTable12.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable12.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow41});
            this.xrTable12.SizeF = new System.Drawing.SizeF(630.4F, 15F);
            this.xrTable12.StylePriority.UseFont = false;
            this.xrTable12.StylePriority.UsePadding = false;
            this.xrTable12.Tag = "WithColor";
            // 
            // xrTableRow41
            // 
            this.xrTableRow41.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell39,
            this.xrTableCell43});
            this.xrTableRow41.Name = "xrTableRow41";
            this.xrTableRow41.Weight = 11.499971850768331D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell39.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell39.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "AccionesOTitulos.fxValorMercado", "{0:$ #,###0.00}")});
            this.xrTableCell39.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell39.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell39.StylePriority.UseBackColor = false;
            this.xrTableCell39.StylePriority.UseBorders = false;
            this.xrTableCell39.StylePriority.UseFont = false;
            this.xrTableCell39.StylePriority.UseForeColor = false;
            this.xrTableCell39.StylePriority.UsePadding = false;
            this.xrTableCell39.StylePriority.UseTextAlignment = false;
            this.xrTableCell39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell39.Weight = 1.5448915170323938D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell43.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell43.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "AccionesOTitulos.fxPrecioAlOtorgarse", "{0:$ #,###0.00}")});
            this.xrTableCell43.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell43.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell43.StylePriority.UseBackColor = false;
            this.xrTableCell43.StylePriority.UseBorders = false;
            this.xrTableCell43.StylePriority.UseFont = false;
            this.xrTableCell43.StylePriority.UseForeColor = false;
            this.xrTableCell43.StylePriority.UsePadding = false;
            this.xrTableCell43.StylePriority.UseTextAlignment = false;
            this.xrTableCell43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell43.Weight = 1.5448915767800453D;
            // 
            // GroupHeaderAcciones
            // 
            this.GroupHeaderAcciones.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable11});
            this.GroupHeaderAcciones.HeightF = 30F;
            this.GroupHeaderAcciones.KeepTogether = true;
            this.GroupHeaderAcciones.Name = "GroupHeaderAcciones";
            this.GroupHeaderAcciones.RepeatEveryPage = true;
            // 
            // xrTable11
            // 
            this.xrTable11.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable11.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable11.Name = "xrTable11";
            this.xrTable11.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable11.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow38,
            this.xrTableRow39});
            this.xrTable11.SizeF = new System.Drawing.SizeF(630.4F, 29.99999F);
            this.xrTable11.StylePriority.UseFont = false;
            this.xrTable11.StylePriority.UsePadding = false;
            this.xrTable11.Tag = "WithColor";
            // 
            // xrTableRow38
            // 
            this.xrTableRow38.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell20});
            this.xrTableRow38.Name = "xrTableRow38";
            this.xrTableRow38.Weight = 11.499971850768331D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.BackColor = System.Drawing.Color.Black;
            this.xrTableCell20.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell20.ForeColor = System.Drawing.Color.White;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell20.StylePriority.UseBackColor = false;
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UseForeColor = false;
            this.xrTableCell20.StylePriority.UsePadding = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "Acciones o Titulos";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell20.Weight = 4.3671987693860874D;
            // 
            // xrTableRow39
            // 
            this.xrTableRow39.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell31,
            this.xrTableCell41});
            this.xrTableRow39.Name = "xrTableRow39";
            this.xrTableRow39.Weight = 11.499971850768331D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell31.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell31.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell31.ForeColor = System.Drawing.Color.White;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell31.StylePriority.UseBackColor = false;
            this.xrTableCell31.StylePriority.UseBorders = false;
            this.xrTableCell31.StylePriority.UseFont = false;
            this.xrTableCell31.StylePriority.UseForeColor = false;
            this.xrTableCell31.StylePriority.UsePadding = false;
            this.xrTableCell31.StylePriority.UseTextAlignment = false;
            this.xrTableCell31.Text = "Valor de Mercado";
            this.xrTableCell31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell31.Weight = 1.3083825109035929D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell41.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell41.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell41.ForeColor = System.Drawing.Color.White;
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell41.StylePriority.UseBackColor = false;
            this.xrTableCell41.StylePriority.UseBorders = false;
            this.xrTableCell41.StylePriority.UseFont = false;
            this.xrTableCell41.StylePriority.UseForeColor = false;
            this.xrTableCell41.StylePriority.UsePadding = false;
            this.xrTableCell41.StylePriority.UseTextAlignment = false;
            this.xrTableCell41.Text = "Precio al otorgarse";
            this.xrTableCell41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell41.Weight = 1.3083825706512446D;
            // 
            // fxPrecioAlOtorgarse
            // 
            this.fxPrecioAlOtorgarse.DataMember = "AccionesOTitulos";
            this.fxPrecioAlOtorgarse.Expression = "[PrecioAlOtorgarse]";
            this.fxPrecioAlOtorgarse.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxPrecioAlOtorgarse.Name = "fxPrecioAlOtorgarse";
            // 
            // fxValorMercado
            // 
            this.fxValorMercado.DataMember = "AccionesOTitulos";
            this.fxValorMercado.Expression = "[ValorMercado]";
            this.fxValorMercado.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxValorMercado.Name = "fxValorMercado";
            // 
            // pLinkSAT
            // 
            this.pLinkSAT.Description = "Link del SAT para el CBB";
            this.pLinkSAT.Name = "pLinkSAT";
            this.pLinkSAT.ValueInfo = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx";
            this.pLinkSAT.Visible = false;
            // 
            // fxTotalDeduccionesTit
            // 
            this.fxTotalDeduccionesTit.DataMember = "Nomina";
            this.fxTotalDeduccionesTit.Expression = "Iif(IsNullOrEmpty([TotalDeducciones]),\'\' ,\'Total Deducciones\' )";
            this.fxTotalDeduccionesTit.FieldType = DevExpress.XtraReports.UI.FieldType.String;
            this.fxTotalDeduccionesTit.Name = "fxTotalDeduccionesTit";
            // 
            // fxTotalDeducciones
            // 
            this.fxTotalDeducciones.DataMember = "Nomina";
            this.fxTotalDeducciones.Expression = "Iif(IsNullOrEmpty([TotalDeducciones]),\'\' ,[TotalDeducciones])";
            this.fxTotalDeducciones.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxTotalDeducciones.Name = "fxTotalDeducciones";
            // 
            // fxTotalOtrosPagosTit
            // 
            this.fxTotalOtrosPagosTit.DataMember = "Nomina";
            this.fxTotalOtrosPagosTit.Expression = " Iif(IsNullOrEmpty([TotalOtrosPagos]),\'\' ,\'Total Otros Pagos\' )";
            this.fxTotalOtrosPagosTit.Name = "fxTotalOtrosPagosTit";
            // 
            // fxTotalOtrosPagos
            // 
            this.fxTotalOtrosPagos.DataMember = "Nomina";
            this.fxTotalOtrosPagos.Expression = " Iif(IsNullOrEmpty([TotalOtrosPagos]),\'\' ,[TotalOtrosPagos] )";
            this.fxTotalOtrosPagos.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxTotalOtrosPagos.Name = "fxTotalOtrosPagos";
            // 
            // fxImportePagado
            // 
            this.fxImportePagado.DataMember = "HorasExtra";
            this.fxImportePagado.Expression = "[ImportePagado]";
            this.fxImportePagado.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImportePagado.Name = "fxImportePagado";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine3});
            this.PageHeader.Expanded = false;
            this.PageHeader.HeightF = 2.000046F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.PrintOn = DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader;
            this.PageHeader.Visible = false;
            // 
            // xrLine3
            // 
            this.xrLine3.BorderColor = System.Drawing.Color.Green;
            this.xrLine3.ForeColor = System.Drawing.Color.Black;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(788F, 2F);
            this.xrLine3.StylePriority.UseBorderColor = false;
            this.xrLine3.StylePriority.UseForeColor = false;
            this.xrLine3.Tag = "WithColor";
            // 
            // CrossLineTipoPerce
            // 
            this.CrossLineTipoPerce.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineTipoPerce.EndBand = this.GroupFooterPerceDedu;
            this.CrossLineTipoPerce.EndPointFloat = new DevExpress.Utils.PointFloat(32F, 1F);
            this.CrossLineTipoPerce.ForeColor = System.Drawing.Color.Black;
            this.CrossLineTipoPerce.LocationFloat = new DevExpress.Utils.PointFloat(32F, 15F);
            this.CrossLineTipoPerce.Name = "CrossLineTipoPerce";
            this.CrossLineTipoPerce.StartBand = this.GroupHeaderPerceDedu;
            this.CrossLineTipoPerce.StartPointFloat = new DevExpress.Utils.PointFloat(32F, 15F);
            this.CrossLineTipoPerce.Tag = "WithColor";
            this.CrossLineTipoPerce.WidthF = 1F;
            // 
            // GroupFooterPerceDedu
            // 
            this.GroupFooterPerceDedu.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.LinePerceDedu});
            this.GroupFooterPerceDedu.HeightF = 2F;
            this.GroupFooterPerceDedu.KeepTogether = true;
            this.GroupFooterPerceDedu.Name = "GroupFooterPerceDedu";
            // 
            // LinePerceDedu
            // 
            this.LinePerceDedu.BorderColor = System.Drawing.Color.Green;
            this.LinePerceDedu.ForeColor = System.Drawing.Color.Black;
            this.LinePerceDedu.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.LinePerceDedu.Name = "LinePerceDedu";
            this.LinePerceDedu.SizeF = new System.Drawing.SizeF(788F, 2F);
            this.LinePerceDedu.StylePriority.UseBorderColor = false;
            this.LinePerceDedu.StylePriority.UseForeColor = false;
            this.LinePerceDedu.Tag = "WithColor";
            // 
            // GroupHeaderPerceDedu
            // 
            this.GroupHeaderPerceDedu.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable17,
            this.xrTable18});
            this.GroupHeaderPerceDedu.HeightF = 45F;
            this.GroupHeaderPerceDedu.KeepTogether = true;
            this.GroupHeaderPerceDedu.Level = 1;
            this.GroupHeaderPerceDedu.Name = "GroupHeaderPerceDedu";
            this.GroupHeaderPerceDedu.RepeatEveryPage = true;
            // 
            // xrTable17
            // 
            this.xrTable17.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable17.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable17.Name = "xrTable17";
            this.xrTable17.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable17.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow50,
            this.xrTableRow51});
            this.xrTable17.SizeF = new System.Drawing.SizeF(394F, 45F);
            this.xrTable17.StylePriority.UseFont = false;
            this.xrTable17.StylePriority.UsePadding = false;
            this.xrTable17.Tag = "WithColor";
            // 
            // xrTableRow50
            // 
            this.xrTableRow50.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell111});
            this.xrTableRow50.Name = "xrTableRow50";
            this.xrTableRow50.Weight = 11.499971850768331D;
            // 
            // xrTableCell111
            // 
            this.xrTableCell111.BackColor = System.Drawing.Color.Black;
            this.xrTableCell111.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell111.ForeColor = System.Drawing.Color.White;
            this.xrTableCell111.Name = "xrTableCell111";
            this.xrTableCell111.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell111.StylePriority.UseBackColor = false;
            this.xrTableCell111.StylePriority.UseFont = false;
            this.xrTableCell111.StylePriority.UseForeColor = false;
            this.xrTableCell111.StylePriority.UsePadding = false;
            this.xrTableCell111.StylePriority.UseTextAlignment = false;
            this.xrTableCell111.Text = "Percepciones";
            this.xrTableCell111.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell111.Weight = 4.3584794990305218D;
            // 
            // xrTableRow51
            // 
            this.xrTableRow51.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell112,
            this.xrTableCell115,
            this.xrTableCell116,
            this.xrTableCell117});
            this.xrTableRow51.Name = "xrTableRow51";
            this.xrTableRow51.Weight = 22.999943701536662D;
            // 
            // xrTableCell112
            // 
            this.xrTableCell112.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell112.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell112.CanGrow = false;
            this.xrTableCell112.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell112.ForeColor = System.Drawing.Color.White;
            this.xrTableCell112.Name = "xrTableCell112";
            this.xrTableCell112.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell112.StylePriority.UseBackColor = false;
            this.xrTableCell112.StylePriority.UseBorders = false;
            this.xrTableCell112.StylePriority.UseFont = false;
            this.xrTableCell112.StylePriority.UseForeColor = false;
            this.xrTableCell112.StylePriority.UsePadding = false;
            this.xrTableCell112.StylePriority.UseTextAlignment = false;
            this.xrTableCell112.Text = "Tipo";
            this.xrTableCell112.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell112.Weight = 0.5663810810540848D;
            // 
            // xrTableCell115
            // 
            this.xrTableCell115.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell115.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell115.CanGrow = false;
            this.xrTableCell115.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell115.ForeColor = System.Drawing.Color.White;
            this.xrTableCell115.Name = "xrTableCell115";
            this.xrTableCell115.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell115.StylePriority.UseBackColor = false;
            this.xrTableCell115.StylePriority.UseBorders = false;
            this.xrTableCell115.StylePriority.UseFont = false;
            this.xrTableCell115.StylePriority.UseForeColor = false;
            this.xrTableCell115.StylePriority.UsePadding = false;
            this.xrTableCell115.StylePriority.UseTextAlignment = false;
            this.xrTableCell115.Text = "Clave";
            this.xrTableCell115.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell115.Weight = 0.97346749804563149D;
            // 
            // xrTableCell116
            // 
            this.xrTableCell116.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell116.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell116.CanGrow = false;
            this.xrTableCell116.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell116.ForeColor = System.Drawing.Color.White;
            this.xrTableCell116.Name = "xrTableCell116";
            this.xrTableCell116.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell116.StylePriority.UseBackColor = false;
            this.xrTableCell116.StylePriority.UseBorders = false;
            this.xrTableCell116.StylePriority.UseFont = false;
            this.xrTableCell116.StylePriority.UseForeColor = false;
            this.xrTableCell116.StylePriority.UsePadding = false;
            this.xrTableCell116.StylePriority.UseTextAlignment = false;
            this.xrTableCell116.Text = "Concepto";
            this.xrTableCell116.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell116.Weight = 2.6903102022083525D;
            // 
            // xrTableCell117
            // 
            this.xrTableCell117.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell117.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell117.CanGrow = false;
            this.xrTableCell117.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable19});
            this.xrTableCell117.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell117.ForeColor = System.Drawing.Color.White;
            this.xrTableCell117.Name = "xrTableCell117";
            this.xrTableCell117.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrTableCell117.StylePriority.UseBackColor = false;
            this.xrTableCell117.StylePriority.UseBorders = false;
            this.xrTableCell117.StylePriority.UseFont = false;
            this.xrTableCell117.StylePriority.UseForeColor = false;
            this.xrTableCell117.StylePriority.UsePadding = false;
            this.xrTableCell117.StylePriority.UseTextAlignment = false;
            this.xrTableCell117.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell117.Weight = 2.7434083684598844D;
            // 
            // xrTable19
            // 
            this.xrTable19.BorderColor = System.Drawing.Color.Transparent;
            this.xrTable19.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable19.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable19.Name = "xrTable19";
            this.xrTable19.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow54,
            this.xrTableRow55});
            this.xrTable19.SizeF = new System.Drawing.SizeF(155F, 29F);
            this.xrTable19.StylePriority.UseBorderColor = false;
            this.xrTable19.StylePriority.UseBorders = false;
            this.xrTable19.Tag = "WithColor";
            // 
            // xrTableRow54
            // 
            this.xrTableRow54.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell124});
            this.xrTableRow54.Name = "xrTableRow54";
            this.xrTableRow54.Weight = 0.8399993903009676D;
            // 
            // xrTableCell124
            // 
            this.xrTableCell124.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell124.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell124.CanGrow = false;
            this.xrTableCell124.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell124.Name = "xrTableCell124";
            this.xrTableCell124.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTableCell124.StylePriority.UseBorderColor = false;
            this.xrTableCell124.StylePriority.UseBorders = false;
            this.xrTableCell124.StylePriority.UseFont = false;
            this.xrTableCell124.StylePriority.UsePadding = false;
            this.xrTableCell124.StylePriority.UseTextAlignment = false;
            this.xrTableCell124.Text = "Importe";
            this.xrTableCell124.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell124.Weight = 2.98144D;
            // 
            // xrTableRow55
            // 
            this.xrTableRow55.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell125,
            this.xrTableCell126});
            this.xrTableRow55.Name = "xrTableRow55";
            this.xrTableRow55.Weight = 0.89999930236060077D;
            // 
            // xrTableCell125
            // 
            this.xrTableCell125.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell125.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell125.CanGrow = false;
            this.xrTableCell125.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell125.Name = "xrTableCell125";
            this.xrTableCell125.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTableCell125.StylePriority.UseBorderColor = false;
            this.xrTableCell125.StylePriority.UseBorders = false;
            this.xrTableCell125.StylePriority.UseFont = false;
            this.xrTableCell125.StylePriority.UsePadding = false;
            this.xrTableCell125.StylePriority.UseTextAlignment = false;
            this.xrTableCell125.Text = "Gravado";
            this.xrTableCell125.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell125.Weight = 1.4907200463867187D;
            // 
            // xrTableCell126
            // 
            this.xrTableCell126.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell126.CanGrow = false;
            this.xrTableCell126.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell126.Name = "xrTableCell126";
            this.xrTableCell126.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTableCell126.StylePriority.UseBorders = false;
            this.xrTableCell126.StylePriority.UseFont = false;
            this.xrTableCell126.StylePriority.UsePadding = false;
            this.xrTableCell126.StylePriority.UseTextAlignment = false;
            this.xrTableCell126.Text = "Exento";
            this.xrTableCell126.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell126.Weight = 1.4907199536132816D;
            // 
            // xrTable18
            // 
            this.xrTable18.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable18.LocationFloat = new DevExpress.Utils.PointFloat(395F, 0F);
            this.xrTable18.Name = "xrTable18";
            this.xrTable18.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable18.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow52,
            this.xrTableRow53});
            this.xrTable18.SizeF = new System.Drawing.SizeF(394F, 45F);
            this.xrTable18.StylePriority.UseFont = false;
            this.xrTable18.StylePriority.UsePadding = false;
            this.xrTable18.Tag = "WithColor";
            // 
            // xrTableRow52
            // 
            this.xrTableRow52.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell113});
            this.xrTableRow52.Name = "xrTableRow52";
            this.xrTableRow52.Weight = 11.499971850768331D;
            // 
            // xrTableCell113
            // 
            this.xrTableCell113.BackColor = System.Drawing.Color.Black;
            this.xrTableCell113.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell113.ForeColor = System.Drawing.Color.White;
            this.xrTableCell113.Name = "xrTableCell113";
            this.xrTableCell113.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell113.StylePriority.UseBackColor = false;
            this.xrTableCell113.StylePriority.UseFont = false;
            this.xrTableCell113.StylePriority.UseForeColor = false;
            this.xrTableCell113.StylePriority.UsePadding = false;
            this.xrTableCell113.StylePriority.UseTextAlignment = false;
            this.xrTableCell113.Text = "Deducciones";
            this.xrTableCell113.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell113.Weight = 4.3584794990305218D;
            // 
            // xrTableRow53
            // 
            this.xrTableRow53.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell118,
            this.xrTableCell119,
            this.xrTableCell121,
            this.xrTableCell122});
            this.xrTableRow53.Name = "xrTableRow53";
            this.xrTableRow53.Weight = 22.999943701536662D;
            // 
            // xrTableCell118
            // 
            this.xrTableCell118.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell118.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell118.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell118.ForeColor = System.Drawing.Color.White;
            this.xrTableCell118.Name = "xrTableCell118";
            this.xrTableCell118.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell118.StylePriority.UseBackColor = false;
            this.xrTableCell118.StylePriority.UseBorders = false;
            this.xrTableCell118.StylePriority.UseFont = false;
            this.xrTableCell118.StylePriority.UseForeColor = false;
            this.xrTableCell118.StylePriority.UsePadding = false;
            this.xrTableCell118.StylePriority.UseTextAlignment = false;
            this.xrTableCell118.Text = "Tipo";
            this.xrTableCell118.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell118.Weight = 0.5663810810540848D;
            // 
            // xrTableCell119
            // 
            this.xrTableCell119.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell119.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell119.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell119.ForeColor = System.Drawing.Color.White;
            this.xrTableCell119.Name = "xrTableCell119";
            this.xrTableCell119.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell119.StylePriority.UseBackColor = false;
            this.xrTableCell119.StylePriority.UseBorders = false;
            this.xrTableCell119.StylePriority.UseFont = false;
            this.xrTableCell119.StylePriority.UseForeColor = false;
            this.xrTableCell119.StylePriority.UsePadding = false;
            this.xrTableCell119.StylePriority.UseTextAlignment = false;
            this.xrTableCell119.Text = "Clave";
            this.xrTableCell119.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell119.Weight = 0.97346749804563149D;
            // 
            // xrTableCell121
            // 
            this.xrTableCell121.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell121.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell121.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell121.ForeColor = System.Drawing.Color.White;
            this.xrTableCell121.Name = "xrTableCell121";
            this.xrTableCell121.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell121.StylePriority.UseBackColor = false;
            this.xrTableCell121.StylePriority.UseBorders = false;
            this.xrTableCell121.StylePriority.UseFont = false;
            this.xrTableCell121.StylePriority.UseForeColor = false;
            this.xrTableCell121.StylePriority.UsePadding = false;
            this.xrTableCell121.StylePriority.UseTextAlignment = false;
            this.xrTableCell121.Text = "Concepto";
            this.xrTableCell121.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell121.Weight = 3.6814760264577044D;
            // 
            // xrTableCell122
            // 
            this.xrTableCell122.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell122.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell122.CanGrow = false;
            this.xrTableCell122.CanShrink = true;
            this.xrTableCell122.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell122.ForeColor = System.Drawing.Color.White;
            this.xrTableCell122.Name = "xrTableCell122";
            this.xrTableCell122.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrTableCell122.StylePriority.UseBackColor = false;
            this.xrTableCell122.StylePriority.UseBorders = false;
            this.xrTableCell122.StylePriority.UseFont = false;
            this.xrTableCell122.StylePriority.UseForeColor = false;
            this.xrTableCell122.StylePriority.UsePadding = false;
            this.xrTableCell122.StylePriority.UseTextAlignment = false;
            this.xrTableCell122.Text = "Importe";
            this.xrTableCell122.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell122.Weight = 1.7522425442105323D;
            // 
            // CrossLineClavePerce
            // 
            this.CrossLineClavePerce.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineClavePerce.EndBand = this.GroupFooterPerceDedu;
            this.CrossLineClavePerce.EndPointFloat = new DevExpress.Utils.PointFloat(87F, 1F);
            this.CrossLineClavePerce.ForeColor = System.Drawing.Color.Black;
            this.CrossLineClavePerce.LocationFloat = new DevExpress.Utils.PointFloat(87F, 15F);
            this.CrossLineClavePerce.Name = "CrossLineClavePerce";
            this.CrossLineClavePerce.StartBand = this.GroupHeaderPerceDedu;
            this.CrossLineClavePerce.StartPointFloat = new DevExpress.Utils.PointFloat(87F, 15F);
            this.CrossLineClavePerce.Tag = "WithColor";
            this.CrossLineClavePerce.WidthF = 1F;
            // 
            // CrossLineConceptoPerce
            // 
            this.CrossLineConceptoPerce.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineConceptoPerce.EndBand = this.GroupFooterPerceDedu;
            this.CrossLineConceptoPerce.EndPointFloat = new DevExpress.Utils.PointFloat(239F, 1F);
            this.CrossLineConceptoPerce.ForeColor = System.Drawing.Color.Black;
            this.CrossLineConceptoPerce.LocationFloat = new DevExpress.Utils.PointFloat(239F, 15F);
            this.CrossLineConceptoPerce.Name = "CrossLineConceptoPerce";
            this.CrossLineConceptoPerce.StartBand = this.GroupHeaderPerceDedu;
            this.CrossLineConceptoPerce.StartPointFloat = new DevExpress.Utils.PointFloat(239F, 15F);
            this.CrossLineConceptoPerce.Tag = "WithColor";
            this.CrossLineConceptoPerce.WidthF = 1F;
            // 
            // CrossBandGravadoPerce
            // 
            this.CrossBandGravadoPerce.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossBandGravadoPerce.EndBand = this.GroupFooterPerceDedu;
            this.CrossBandGravadoPerce.EndPointFloat = new DevExpress.Utils.PointFloat(316.5F, 1F);
            this.CrossBandGravadoPerce.ForeColor = System.Drawing.Color.Black;
            this.CrossBandGravadoPerce.LocationFloat = new DevExpress.Utils.PointFloat(316.5F, 30F);
            this.CrossBandGravadoPerce.Name = "CrossBandGravadoPerce";
            this.CrossBandGravadoPerce.StartBand = this.GroupHeaderPerceDedu;
            this.CrossBandGravadoPerce.StartPointFloat = new DevExpress.Utils.PointFloat(316.5F, 30F);
            this.CrossBandGravadoPerce.Tag = "WithColor";
            this.CrossBandGravadoPerce.WidthF = 1F;
            // 
            // CrossBandExentoPerce
            // 
            this.CrossBandExentoPerce.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossBandExentoPerce.EndBand = this.GroupFooterPerceDedu;
            this.CrossBandExentoPerce.EndPointFloat = new DevExpress.Utils.PointFloat(394F, 1F);
            this.CrossBandExentoPerce.ForeColor = System.Drawing.Color.Black;
            this.CrossBandExentoPerce.LocationFloat = new DevExpress.Utils.PointFloat(394F, 15F);
            this.CrossBandExentoPerce.Name = "CrossBandExentoPerce";
            this.CrossBandExentoPerce.StartBand = this.GroupHeaderPerceDedu;
            this.CrossBandExentoPerce.StartPointFloat = new DevExpress.Utils.PointFloat(394F, 15F);
            this.CrossBandExentoPerce.Tag = "WithColor";
            this.CrossBandExentoPerce.WidthF = 1F;
            // 
            // CrossLineTipoDedu
            // 
            this.CrossLineTipoDedu.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineTipoDedu.EndBand = this.GroupFooterPerceDedu;
            this.CrossLineTipoDedu.EndPointFloat = new DevExpress.Utils.PointFloat(426F, 1F);
            this.CrossLineTipoDedu.ForeColor = System.Drawing.Color.Black;
            this.CrossLineTipoDedu.LocationFloat = new DevExpress.Utils.PointFloat(426F, 15F);
            this.CrossLineTipoDedu.Name = "CrossLineTipoDedu";
            this.CrossLineTipoDedu.StartBand = this.GroupHeaderPerceDedu;
            this.CrossLineTipoDedu.StartPointFloat = new DevExpress.Utils.PointFloat(426F, 15F);
            this.CrossLineTipoDedu.Tag = "WithColor";
            this.CrossLineTipoDedu.WidthF = 1F;
            // 
            // CrossBandClaveDedu
            // 
            this.CrossBandClaveDedu.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossBandClaveDedu.EndBand = this.GroupFooterPerceDedu;
            this.CrossBandClaveDedu.EndPointFloat = new DevExpress.Utils.PointFloat(481F, 1F);
            this.CrossBandClaveDedu.ForeColor = System.Drawing.Color.Black;
            this.CrossBandClaveDedu.LocationFloat = new DevExpress.Utils.PointFloat(481F, 15F);
            this.CrossBandClaveDedu.Name = "CrossBandClaveDedu";
            this.CrossBandClaveDedu.StartBand = this.GroupHeaderPerceDedu;
            this.CrossBandClaveDedu.StartPointFloat = new DevExpress.Utils.PointFloat(481F, 15F);
            this.CrossBandClaveDedu.Tag = "WithColor";
            this.CrossBandClaveDedu.WidthF = 1F;
            // 
            // CrossLineConceptoDedu
            // 
            this.CrossLineConceptoDedu.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineConceptoDedu.EndBand = this.GroupFooterPerceDedu;
            this.CrossLineConceptoDedu.EndPointFloat = new DevExpress.Utils.PointFloat(689F, 1F);
            this.CrossLineConceptoDedu.ForeColor = System.Drawing.Color.Black;
            this.CrossLineConceptoDedu.LocationFloat = new DevExpress.Utils.PointFloat(689F, 15F);
            this.CrossLineConceptoDedu.Name = "CrossLineConceptoDedu";
            this.CrossLineConceptoDedu.StartBand = this.GroupHeaderPerceDedu;
            this.CrossLineConceptoDedu.StartPointFloat = new DevExpress.Utils.PointFloat(689F, 15F);
            this.CrossLineConceptoDedu.Tag = "WithColor";
            this.CrossLineConceptoDedu.WidthF = 1F;
            // 
            // DetailReportExtraInca
            // 
            this.DetailReportExtraInca.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetailExtraInca,
            this.GroupHeaderExtraInca,
            this.GroupFooterExtraInca});
            this.DetailReportExtraInca.Level = 1;
            this.DetailReportExtraInca.Name = "DetailReportExtraInca";
            this.DetailReportExtraInca.ReportPrintOptions.DetailCountOnEmptyDataSource = 0;
            this.DetailReportExtraInca.ReportPrintOptions.PrintOnEmptyDataSource = false;
            this.DetailReportExtraInca.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DetailReportExtraInca_BeforePrint);
            // 
            // DetailExtraInca
            // 
            this.DetailExtraInca.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.SubrIncapacidades,
            this.SubrHorasExtra});
            this.DetailExtraInca.HeightF = 15F;
            this.DetailExtraInca.Name = "DetailExtraInca";
            // 
            // SubrIncapacidades
            // 
            this.SubrIncapacidades.CanShrink = true;
            this.SubrIncapacidades.LocationFloat = new DevExpress.Utils.PointFloat(396F, 0F);
            this.SubrIncapacidades.Name = "SubrIncapacidades";
            this.SubrIncapacidades.ReportSource = new rptIncapacidades();
            this.SubrIncapacidades.SizeF = new System.Drawing.SizeF(394F, 15F);
            this.SubrIncapacidades.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.SubrIncapacidades_BeforePrint);
            // 
            // SubrHorasExtra
            // 
            this.SubrHorasExtra.CanShrink = true;
            this.SubrHorasExtra.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.SubrHorasExtra.Name = "SubrHorasExtra";
            this.SubrHorasExtra.ReportSource = new rptHorasExtra();
            this.SubrHorasExtra.SizeF = new System.Drawing.SizeF(394F, 15F);
            this.SubrHorasExtra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.SubrHorasExtra_BeforePrint);
            // 
            // GroupHeaderExtraInca
            // 
            this.GroupHeaderExtraInca.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tblHeaderIncapacidades,
            this.tblHeaderHorasExtra});
            this.GroupHeaderExtraInca.HeightF = 30F;
            this.GroupHeaderExtraInca.KeepTogether = true;
            this.GroupHeaderExtraInca.Name = "GroupHeaderExtraInca";
            this.GroupHeaderExtraInca.RepeatEveryPage = true;
            // 
            // tblHeaderIncapacidades
            // 
            this.tblHeaderIncapacidades.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblHeaderIncapacidades.LocationFloat = new DevExpress.Utils.PointFloat(394F, 0F);
            this.tblHeaderIncapacidades.Name = "tblHeaderIncapacidades";
            this.tblHeaderIncapacidades.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.tblHeaderIncapacidades.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow58,
            this.xrTableRow65});
            this.tblHeaderIncapacidades.SizeF = new System.Drawing.SizeF(394F, 30F);
            this.tblHeaderIncapacidades.StylePriority.UseFont = false;
            this.tblHeaderIncapacidades.StylePriority.UsePadding = false;
            this.tblHeaderIncapacidades.Tag = "WithColor";
            // 
            // xrTableRow58
            // 
            this.xrTableRow58.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell128});
            this.xrTableRow58.Name = "xrTableRow58";
            this.xrTableRow58.Weight = 11.499971850768331D;
            // 
            // xrTableCell128
            // 
            this.xrTableCell128.BackColor = System.Drawing.Color.Black;
            this.xrTableCell128.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTableCell128.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell128.ForeColor = System.Drawing.Color.White;
            this.xrTableCell128.Name = "xrTableCell128";
            this.xrTableCell128.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell128.StylePriority.UseBackColor = false;
            this.xrTableCell128.StylePriority.UseBorders = false;
            this.xrTableCell128.StylePriority.UseFont = false;
            this.xrTableCell128.StylePriority.UseForeColor = false;
            this.xrTableCell128.StylePriority.UsePadding = false;
            this.xrTableCell128.StylePriority.UseTextAlignment = false;
            this.xrTableCell128.Text = "Incapacidades";
            this.xrTableCell128.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell128.Weight = 0.97391672590359679D;
            // 
            // xrTableRow65
            // 
            this.xrTableRow65.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell129,
            this.xrTableCell130,
            this.xrTableCell131});
            this.xrTableRow65.Name = "xrTableRow65";
            this.xrTableRow65.Weight = 11.499971850768331D;
            // 
            // xrTableCell129
            // 
            this.xrTableCell129.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell129.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell129.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell129.ForeColor = System.Drawing.Color.White;
            this.xrTableCell129.Name = "xrTableCell129";
            this.xrTableCell129.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell129.StylePriority.UseBackColor = false;
            this.xrTableCell129.StylePriority.UseBorders = false;
            this.xrTableCell129.StylePriority.UseFont = false;
            this.xrTableCell129.StylePriority.UseForeColor = false;
            this.xrTableCell129.StylePriority.UsePadding = false;
            this.xrTableCell129.StylePriority.UseTextAlignment = false;
            this.xrTableCell129.Text = "Días";
            this.xrTableCell129.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell129.Weight = 0.5663810810540848D;
            // 
            // xrTableCell130
            // 
            this.xrTableCell130.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell130.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell130.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell130.ForeColor = System.Drawing.Color.White;
            this.xrTableCell130.Name = "xrTableCell130";
            this.xrTableCell130.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell130.StylePriority.UseBackColor = false;
            this.xrTableCell130.StylePriority.UseBorders = false;
            this.xrTableCell130.StylePriority.UseFont = false;
            this.xrTableCell130.StylePriority.UseForeColor = false;
            this.xrTableCell130.StylePriority.UsePadding = false;
            this.xrTableCell130.StylePriority.UseTextAlignment = false;
            this.xrTableCell130.Text = "Tipo de Incapacidad";
            this.xrTableCell130.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell130.Weight = 1.1327622118864271D;
            // 
            // xrTableCell131
            // 
            this.xrTableCell131.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell131.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell131.CanGrow = false;
            this.xrTableCell131.CanShrink = true;
            this.xrTableCell131.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell131.ForeColor = System.Drawing.Color.White;
            this.xrTableCell131.Name = "xrTableCell131";
            this.xrTableCell131.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTableCell131.StylePriority.UseBackColor = false;
            this.xrTableCell131.StylePriority.UseBorders = false;
            this.xrTableCell131.StylePriority.UseFont = false;
            this.xrTableCell131.StylePriority.UseForeColor = false;
            this.xrTableCell131.StylePriority.UsePadding = false;
            this.xrTableCell131.StylePriority.UseTextAlignment = false;
            this.xrTableCell131.Text = "Importe";
            this.xrTableCell131.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell131.Weight = 0.56638106028359481D;
            // 
            // tblHeaderHorasExtra
            // 
            this.tblHeaderHorasExtra.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblHeaderHorasExtra.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.tblHeaderHorasExtra.Name = "tblHeaderHorasExtra";
            this.tblHeaderHorasExtra.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.tblHeaderHorasExtra.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow56,
            this.xrTableRow59});
            this.tblHeaderHorasExtra.SizeF = new System.Drawing.SizeF(394F, 30F);
            this.tblHeaderHorasExtra.StylePriority.UseFont = false;
            this.tblHeaderHorasExtra.StylePriority.UsePadding = false;
            this.tblHeaderHorasExtra.Tag = "WithColor";
            // 
            // xrTableRow56
            // 
            this.xrTableRow56.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell123});
            this.xrTableRow56.Name = "xrTableRow56";
            this.xrTableRow56.Weight = 11.499971850768331D;
            // 
            // xrTableCell123
            // 
            this.xrTableCell123.BackColor = System.Drawing.Color.Black;
            this.xrTableCell123.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell123.ForeColor = System.Drawing.Color.White;
            this.xrTableCell123.Name = "xrTableCell123";
            this.xrTableCell123.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell123.StylePriority.UseBackColor = false;
            this.xrTableCell123.StylePriority.UseFont = false;
            this.xrTableCell123.StylePriority.UseForeColor = false;
            this.xrTableCell123.StylePriority.UsePadding = false;
            this.xrTableCell123.StylePriority.UseTextAlignment = false;
            this.xrTableCell123.Text = "Horas Extra";
            this.xrTableCell123.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell123.Weight = 0.97391672590359679D;
            // 
            // xrTableRow59
            // 
            this.xrTableRow59.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell134,
            this.xrTableCell135,
            this.xrTableCell136,
            this.xrTableCell137});
            this.xrTableRow59.Name = "xrTableRow59";
            this.xrTableRow59.Weight = 11.499971850768331D;
            // 
            // xrTableCell134
            // 
            this.xrTableCell134.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell134.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell134.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell134.ForeColor = System.Drawing.Color.White;
            this.xrTableCell134.Name = "xrTableCell134";
            this.xrTableCell134.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell134.StylePriority.UseBackColor = false;
            this.xrTableCell134.StylePriority.UseBorders = false;
            this.xrTableCell134.StylePriority.UseFont = false;
            this.xrTableCell134.StylePriority.UseForeColor = false;
            this.xrTableCell134.StylePriority.UsePadding = false;
            this.xrTableCell134.StylePriority.UseTextAlignment = false;
            this.xrTableCell134.Text = "Días";
            this.xrTableCell134.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell134.Weight = 0.5663810810540848D;
            // 
            // xrTableCell135
            // 
            this.xrTableCell135.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell135.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell135.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell135.ForeColor = System.Drawing.Color.White;
            this.xrTableCell135.Name = "xrTableCell135";
            this.xrTableCell135.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell135.StylePriority.UseBackColor = false;
            this.xrTableCell135.StylePriority.UseBorders = false;
            this.xrTableCell135.StylePriority.UseFont = false;
            this.xrTableCell135.StylePriority.UseForeColor = false;
            this.xrTableCell135.StylePriority.UsePadding = false;
            this.xrTableCell135.StylePriority.UseTextAlignment = false;
            this.xrTableCell135.Text = "Hrs. Extra";
            this.xrTableCell135.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell135.Weight = 0.56638109082567489D;
            // 
            // xrTableCell136
            // 
            this.xrTableCell136.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell136.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell136.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell136.ForeColor = System.Drawing.Color.White;
            this.xrTableCell136.Name = "xrTableCell136";
            this.xrTableCell136.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell136.StylePriority.UseBackColor = false;
            this.xrTableCell136.StylePriority.UseBorders = false;
            this.xrTableCell136.StylePriority.UseFont = false;
            this.xrTableCell136.StylePriority.UseForeColor = false;
            this.xrTableCell136.StylePriority.UsePadding = false;
            this.xrTableCell136.StylePriority.UseTextAlignment = false;
            this.xrTableCell136.Text = "Tipo de Hora";
            this.xrTableCell136.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell136.Weight = 0.56638112106075234D;
            // 
            // xrTableCell137
            // 
            this.xrTableCell137.BackColor = System.Drawing.Color.DarkGray;
            this.xrTableCell137.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell137.CanGrow = false;
            this.xrTableCell137.CanShrink = true;
            this.xrTableCell137.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell137.ForeColor = System.Drawing.Color.White;
            this.xrTableCell137.Name = "xrTableCell137";
            this.xrTableCell137.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTableCell137.StylePriority.UseBackColor = false;
            this.xrTableCell137.StylePriority.UseBorders = false;
            this.xrTableCell137.StylePriority.UseFont = false;
            this.xrTableCell137.StylePriority.UseForeColor = false;
            this.xrTableCell137.StylePriority.UsePadding = false;
            this.xrTableCell137.StylePriority.UseTextAlignment = false;
            this.xrTableCell137.Text = "Importe";
            this.xrTableCell137.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell137.Weight = 0.56638106028359481D;
            // 
            // GroupFooterExtraInca
            // 
            this.GroupFooterExtraInca.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.LineExtraInca});
            this.GroupFooterExtraInca.GroupUnion = DevExpress.XtraReports.UI.GroupFooterUnion.WithLastDetail;
            this.GroupFooterExtraInca.HeightF = 2F;
            this.GroupFooterExtraInca.KeepTogether = true;
            this.GroupFooterExtraInca.Name = "GroupFooterExtraInca";
            // 
            // LineExtraInca
            // 
            this.LineExtraInca.BorderColor = System.Drawing.Color.Green;
            this.LineExtraInca.ForeColor = System.Drawing.Color.Black;
            this.LineExtraInca.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.LineExtraInca.Name = "LineExtraInca";
            this.LineExtraInca.SizeF = new System.Drawing.SizeF(788F, 2F);
            this.LineExtraInca.StylePriority.UseBorderColor = false;
            this.LineExtraInca.StylePriority.UseForeColor = false;
            this.LineExtraInca.Tag = "WithColor";
            // 
            // CrossLineHorasExtra
            // 
            this.CrossLineHorasExtra.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineHorasExtra.EndBand = this.GroupFooterExtraInca;
            this.CrossLineHorasExtra.EndPointFloat = new DevExpress.Utils.PointFloat(197F, 1F);
            this.CrossLineHorasExtra.ForeColor = System.Drawing.Color.Black;
            this.CrossLineHorasExtra.LocationFloat = new DevExpress.Utils.PointFloat(197F, 11.5F);
            this.CrossLineHorasExtra.Name = "CrossLineHorasExtra";
            this.CrossLineHorasExtra.StartBand = this.GroupHeaderExtraInca;
            this.CrossLineHorasExtra.StartPointFloat = new DevExpress.Utils.PointFloat(197F, 11.5F);
            this.CrossLineHorasExtra.Tag = "WithColor";
            this.CrossLineHorasExtra.WidthF = 1F;
            // 
            // CrossLineTipoHora
            // 
            this.CrossLineTipoHora.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineTipoHora.EndBand = this.GroupFooterExtraInca;
            this.CrossLineTipoHora.EndPointFloat = new DevExpress.Utils.PointFloat(295.5F, 1F);
            this.CrossLineTipoHora.ForeColor = System.Drawing.Color.Black;
            this.CrossLineTipoHora.LocationFloat = new DevExpress.Utils.PointFloat(295.5F, 11.5F);
            this.CrossLineTipoHora.Name = "CrossLineTipoHora";
            this.CrossLineTipoHora.StartBand = this.GroupHeaderExtraInca;
            this.CrossLineTipoHora.StartPointFloat = new DevExpress.Utils.PointFloat(295.5F, 11.5F);
            this.CrossLineTipoHora.Tag = "WithColor";
            this.CrossLineTipoHora.WidthF = 1F;
            // 
            // CrossLineImporteHorasExtra
            // 
            this.CrossLineImporteHorasExtra.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineImporteHorasExtra.EndBand = this.GroupFooterExtraInca;
            this.CrossLineImporteHorasExtra.EndPointFloat = new DevExpress.Utils.PointFloat(394F, 1F);
            this.CrossLineImporteHorasExtra.ForeColor = System.Drawing.Color.Black;
            this.CrossLineImporteHorasExtra.LocationFloat = new DevExpress.Utils.PointFloat(394F, 11.5F);
            this.CrossLineImporteHorasExtra.Name = "CrossLineImporteHorasExtra";
            this.CrossLineImporteHorasExtra.StartBand = this.GroupHeaderExtraInca;
            this.CrossLineImporteHorasExtra.StartPointFloat = new DevExpress.Utils.PointFloat(394F, 11.5F);
            this.CrossLineImporteHorasExtra.Tag = "WithColor";
            this.CrossLineImporteHorasExtra.WidthF = 1F;
            // 
            // CrossLineDiasIncapacidades
            // 
            this.CrossLineDiasIncapacidades.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineDiasIncapacidades.EndBand = this.GroupFooterExtraInca;
            this.CrossLineDiasIncapacidades.EndPointFloat = new DevExpress.Utils.PointFloat(492.5F, 1F);
            this.CrossLineDiasIncapacidades.ForeColor = System.Drawing.Color.Black;
            this.CrossLineDiasIncapacidades.LocationFloat = new DevExpress.Utils.PointFloat(492.5F, 11.5F);
            this.CrossLineDiasIncapacidades.Name = "CrossLineDiasIncapacidades";
            this.CrossLineDiasIncapacidades.StartBand = this.GroupHeaderExtraInca;
            this.CrossLineDiasIncapacidades.StartPointFloat = new DevExpress.Utils.PointFloat(492.5F, 11.5F);
            this.CrossLineDiasIncapacidades.Tag = "WithColor";
            this.CrossLineDiasIncapacidades.WidthF = 1F;
            // 
            // CrossLineTipoInca
            // 
            this.CrossLineTipoInca.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineTipoInca.EndBand = this.GroupFooterExtraInca;
            this.CrossLineTipoInca.EndPointFloat = new DevExpress.Utils.PointFloat(689.5F, 1F);
            this.CrossLineTipoInca.ForeColor = System.Drawing.Color.Black;
            this.CrossLineTipoInca.LocationFloat = new DevExpress.Utils.PointFloat(689.5F, 11.5F);
            this.CrossLineTipoInca.Name = "CrossLineTipoInca";
            this.CrossLineTipoInca.StartBand = this.GroupHeaderExtraInca;
            this.CrossLineTipoInca.StartPointFloat = new DevExpress.Utils.PointFloat(689.5F, 11.5F);
            this.CrossLineTipoInca.Tag = "WithColor";
            this.CrossLineTipoInca.WidthF = 1F;
            // 
            // CrossLineDiasHoras
            // 
            this.CrossLineDiasHoras.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.CrossLineDiasHoras.EndBand = this.GroupFooterExtraInca;
            this.CrossLineDiasHoras.EndPointFloat = new DevExpress.Utils.PointFloat(98.5F, 1F);
            this.CrossLineDiasHoras.ForeColor = System.Drawing.Color.Black;
            this.CrossLineDiasHoras.LocationFloat = new DevExpress.Utils.PointFloat(98.5F, 11.5F);
            this.CrossLineDiasHoras.Name = "CrossLineDiasHoras";
            this.CrossLineDiasHoras.StartBand = this.GroupHeaderExtraInca;
            this.CrossLineDiasHoras.StartPointFloat = new DevExpress.Utils.PointFloat(98.5F, 11.5F);
            this.CrossLineDiasHoras.Tag = "WithColor";
            this.CrossLineDiasHoras.WidthF = 1F;
            // 
            // pColor
            // 
            this.pColor.Description = "Color Bordes";
            this.pColor.Name = "pColor";
            this.pColor.ValueInfo = "Black";
            this.pColor.Visible = false;
            // 
            // DetailReportPerceDedu
            // 
            this.DetailReportPerceDedu.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetailPerceDedu,
            this.GroupFooterPerceDedu,
            this.GroupHeaderPerceDedu});
            this.DetailReportPerceDedu.Level = 0;
            this.DetailReportPerceDedu.Name = "DetailReportPerceDedu";
            // 
            // DetailPerceDedu
            // 
            this.DetailPerceDedu.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.SubDeducciones,
            this.SubPercepciones});
            this.DetailPerceDedu.HeightF = 15F;
            this.DetailPerceDedu.Name = "DetailPerceDedu";
            // 
            // SubDeducciones
            // 
            this.SubDeducciones.CanShrink = true;
            this.SubDeducciones.LocationFloat = new DevExpress.Utils.PointFloat(396F, 0F);
            this.SubDeducciones.Name = "SubDeducciones";
            this.SubDeducciones.ReportSource = new rptDeducciones();
            this.SubDeducciones.SizeF = new System.Drawing.SizeF(394F, 15F);
            // 
            // SubPercepciones
            // 
            this.SubPercepciones.CanShrink = true;
            this.SubPercepciones.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.SubPercepciones.Name = "SubPercepciones";
            this.SubPercepciones.ReportSource = new rptPercepciones();
            this.SubPercepciones.SizeF = new System.Drawing.SizeF(394F, 15F);
            // 
            // rptIncapacidades1
            // 
            this.rptIncapacidades1.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rptIncapacidades1.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.rptIncapacidades1.Name = "rptIncapacidades1";
            this.rptIncapacidades1.PageHeight = 1100;
            this.rptIncapacidades1.PageWidth = 394;
            this.rptIncapacidades1.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.rptIncapacidades1.SnapGridSize = 1F;
            this.rptIncapacidades1.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.rptIncapacidades1.Version = "15.2";
            this.rptIncapacidades1.XmlDataPath = "D:\\ProjectsCrypto\\PAXPlantillaNomina12CFDI33\\PAXPlantillaNomina12CFDI33\\XML\\NOMIN" +
    "A12CDF33.xml";
            // 
            // rptNomina12
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupFooterLine,
            this.GroupFooterSubtotales,
            this.DetailSepIndemnizacion,
            this.DetailJubilacion,
            this.DetailOtroPago,
            this.DetailAcciones,
            this.PageHeader,
            this.DetailReportExtraInca,
            this.DetailReportPerceDedu});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.fxUUID,
            this.fxFecha,
            this.fxReceptorDomicilio,
            this.fxEmisorDomicilio,
            this.fxReferenciaExpedidoEn,
            this.fxDomicilioExpedidoEn,
            this.fxNumCtaPago,
            this.fxTotal,
            this.fxSubTotal,
            this.fxDescripLocTra,
            this.fxImporteLocTra,
            this.fxImpRetencionISR,
            this.fxImporteImpRetencionISR,
            this.fxImporteImpRetencionIVA,
            this.fxImpRetencionIVA,
            this.fxNoIdentificacion,
            this.fxCantidad,
            this.fxValorUnitario,
            this.fxImporteConcepto,
            this.fxAutorizacion,
            this.fxFechaAutorizacion,
            this.fxAduanera,
            this.fxAntiguedad,
            this.fxAntiguedadTit,
            this.fxSalarioBaseTit,
            this.fxSalarioBase,
            this.fxSalarioDiarioTit,
            this.fxSalarioDiario,
            this.fxCuentaBancariaTit,
            this.fxCuentaBancaria,
            this.fxHorasExtraImporte,
            this.fxNombreEmisor,
            this.fxReceptorNombre,
            this.fxReceptorNombreTit,
            this.fxIMSSTit,
            this.fxIMSS,
            this.fxEstadoTit,
            this.fxEstado,
            this.fxTipoJornadaTit,
            this.fxTipoJornada,
            this.fxDeptoTit,
            this.fxDepto,
            this.fxPuesto,
            this.fxPuestoTit,
            this.fxRiesgoTit,
            this.fxRiesgo,
            this.fxTotalPagado,
            this.fxIngresoAcumulable,
            this.fxIngresoNoAcumulable,
            this.fxUltimoSueldo,
            this.fxJubIngresoAcumulable,
            this.fxJubIngresoNoAcumulable,
            this.fxMontoDiario,
            this.fxTotalUnaExhibicion,
            this.fxTotalParcialidad,
            this.fxImporteOtroPago,
            this.fxSubsidioCausado,
            this.fxRemanenteSalFav,
            this.fxSaldoAFavor,
            this.fxTotalPercepciones,
            this.fxTotalPercepcionesTit,
            this.fxMontoRecursoPropio,
            this.fxMontoRecursoPropioTit,
            this.fxPorcentaje,
            this.fxFechaInicioRelLaboralTit,
            this.fxFechaInicioRelLaboral,
            this.fxPeriodoDePago,
            this.fxFechaInicialPago,
            this.fxFechaFinalPago,
            this.fxFechaPago,
            this.fxNumDiasPagados,
            this.fxMetodoPago,
            this.fxRegistroPatronalTit,
            this.fxRegistroPatronal,
            this.fxEmisorCURPTit,
            this.fxEmisorCURP,
            this.fxFechaTimbrado,
            this.fxSerieFolio,
            this.fxConfirmacion,
            this.fxPrecioAlOtorgarse,
            this.fxValorMercado,
            this.fxTotalDeduccionesTit,
            this.fxTotalDeducciones,
            this.fxTotalOtrosPagosTit,
            this.fxTotalOtrosPagos,
            this.fxImportePagado});
            this.CrossBandControls.AddRange(new DevExpress.XtraReports.UI.XRCrossBandControl[] {
            this.CrossLineDiasHoras,
            this.CrossLineTipoInca,
            this.CrossLineDiasIncapacidades,
            this.CrossLineImporteHorasExtra,
            this.CrossLineTipoHora,
            this.CrossLineHorasExtra,
            this.CrossLineConceptoDedu,
            this.CrossBandClaveDedu,
            this.CrossLineTipoDedu,
            this.CrossBandExentoPerce,
            this.CrossBandGravadoPerce,
            this.CrossLineConceptoPerce,
            this.CrossLineClavePerce,
            this.CrossLineTipoPerce,
            this.xrCrossBandCenter,
            this.CrossLineRight,
            this.CrossLineLeft});
            this.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(30, 30, 30, 30);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.pLinkSAT,
            this.pColor});
            this.SnapGridSize = 1F;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.StyleSheetPath = "";
            this.Version = "15.2";
            this.XmlDataPath = "D:\\ProjectsCrypto\\PAXPlantillaNomina12CFDI33\\PAXPlantillaNomina12CFDI33\\XML\\NOMIN" +
    "A12CDF33.xml";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.rptNomina12_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTotalesLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable26)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable27)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable25)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable23)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblMetodoPago)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblTotalesRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable19)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable18)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblHeaderIncapacidades)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblHeaderHorasExtra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptIncapacidades1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void xrBarCodeQRCFDI_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRBarCode BarCodeQRCFDI = (XRBarCode)sender;
        string sCadenaCodigo = string.Empty;              

        sCadenaCodigo = Parameters["pLinkSAT"].Value.ToString()
                        + "&id=" + ((XRTableCell)FindControl("CellUUID", true)).Text.Trim()
                        + "?re=" + ((XRTableCell)FindControl("CellEmisorRFC", true)).Text.Replace("RFC:", "").Trim()
                        + "&rr=" + ((XRTableCell)FindControl("CellReceptorRFC", true)).Text.Replace("RFC:", "").Trim()
                        + "&tt=" + string.Format("{0:0000000000.000000}", Convert.ToDouble(GetCurrentColumnValue("Total").ToString()))
                        + "&fe=" + System.Text.RegularExpressions.Regex.Match(((XRTableCell)FindControl("CellSelloEmisor", true)).Text.Trim(), @"(.{8})\s*$").ToString();
               
        (sender as XRBarCode).Text = sCadenaCodigo;
    }

    private void CellCantidadLetra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRTableCell CeldaTotal = (XRTableCell)sender;

        if (CeldaTotal.Text != "")
        {
            (sender as XRTableCell).Text = fnTextoImporte(CeldaTotal.Text, GetCurrentColumnValue("Moneda").ToString()).ToUpper();
        } 
    }

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
            case "EUR":
                parser.TipoMoneda = Numalet.Moneda.Euro;
                break;
        }

        languaje = new CultureInfo("es-Mx");

        return parser.ToCustomString(Convert.ToDouble(psValor, languaje));
    }

    private void SubPercepciones_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        SubPercepciones.ReportSource.Report.XmlDataPath = this.XmlDataPath;
    }

    private void SubDeducciones_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        SubDeducciones.ReportSource.Report.XmlDataPath = this.XmlDataPath;
    }

    private void SubrHorasExtra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        SubrHorasExtra.ReportSource.Report.XmlDataPath = this.XmlDataPath;
    }

    private void SubrIncapacidades_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        SubrIncapacidades.ReportSource.Report.XmlDataPath = this.XmlDataPath;
    }

    private void CellRegimenFiscal_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRTableCell CellRegimenFiscal = (XRTableCell)sender;
        if (!string.IsNullOrEmpty(CellRegimenFiscal.Text))
        {
            DataTable dtRegimen = fnComparaRegimenFiscal(CellRegimenFiscal.Text);
            (sender as XRTableCell).Text = (dtRegimen.Rows[0]["Descripcion"] != DBNull.Value ? dtRegimen.Rows[0]["Descripcion"].ToString() : "");
        }
    }

    private DataTable fnComparaRegimenFiscal(string psClave)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[sConexion].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Tipos_RegimenFiscal_Sel_Existe_n12";
                    cmd.Parameters.Add(new SqlParameter("@psClave", psClave));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnComparaRegimenFiscal", "rptNomina12");
        }
        return dtResultado;
    }

    private void CellRegimen_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRTableCell CellRegimenFiscal = (XRTableCell)sender;
        if (!string.IsNullOrEmpty(CellRegimenFiscal.Text))
        {
            DataTable dtRegimen = fnComparaRegimen(CellRegimenFiscal.Text);
            (sender as XRTableCell).Text = (dtRegimen.Rows[0]["Descripcion"] != DBNull.Value ? dtRegimen.Rows[0]["Descripcion"].ToString() : "");
         }
    }

    private DataTable fnComparaRegimen(string psClave)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[sConexion].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Tipos_Regimen_Sel_Existe_n12";
                    cmd.Parameters.Add(new SqlParameter("@sClave", psClave));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnComparaRegimen", "rptNomina12");
        }
        return dtResultado;
    }

    private void CellEstado_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRTableCell CellEstado = (XRTableCell)sender;
        if (!string.IsNullOrEmpty(CellEstado.Text))
        {
            DataTable dtEstado = fnComparaEstado(CellEstado.Text);
            (sender as XRTableCell).Text = (dtEstado.Rows[0]["Nombre"] != DBNull.Value ? dtEstado.Rows[0]["Nombre"].ToString() : "");
         }
    }

    private DataTable fnComparaEstado(string psClave)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[sConexion].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_cce_Estado_Sel_Existe_n12";
                    cmd.Parameters.Add(new SqlParameter("@psEstado", psClave));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnComparaEstado", "rptNomina12");
        }
        return dtResultado;
    }

    private void CellTipoContrato_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRTableCell CellTipoContrato = (XRTableCell)sender;
        if (!string.IsNullOrEmpty(CellTipoContrato.Text))
        {
            DataTable dtContrato = fnComparaTipoContrato(CellTipoContrato.Text);
            (sender as XRTableCell).Text = (dtContrato.Rows[0]["Descripcion"] != DBNull.Value ? dtContrato.Rows[0]["Descripcion"].ToString() : "");
        }
    }

    private DataTable fnComparaTipoContrato(string psClave)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[sConexion].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Tipo_Contratos_sel_PorClave_n12";
                    cmd.Parameters.Add(new SqlParameter("@psClave", psClave));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnComparaTipoContrato", "rptNomina12");
        }
        return dtResultado;
    }

    private void CellPeriodicidad_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRTableCell CellPeriodicidad = (XRTableCell)sender;
        if (!string.IsNullOrEmpty(CellPeriodicidad.Text))
        {
            DataTable dtPeriodicidad = fnComparaTipoContrato(CellPeriodicidad.Text);
            (sender as XRTableCell).Text = (dtPeriodicidad.Rows[0]["Descripcion"] != DBNull.Value ? dtPeriodicidad.Rows[0]["Descripcion"].ToString() : "");
        }
    }

    private DataTable fnComparaPeriodicidad(string psClave)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[sConexion].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_PeriodosPorClave_sel_n12";
                    cmd.Parameters.Add(new SqlParameter("@psClave", psClave));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtResultado);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnComparaTipoContrato", "rptNomina12");
        }
        return dtResultado;
    }

    private void LineFooterConceptos_PrintOnPage(object sender, PrintOnPageEventArgs e)
    {
        if (e.PageIndex == e.PageCount - 1)
            e.Cancel = true;
        else
            e.Cancel = false;
    }

    private void rptNomina12_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    { 
        fnChangeColorTables(sender);
        fnChangeColorLines(sender);
        fnChangeColorCrossBandLines(sender);
    }

    private void fnChangeColorTables(object sender)    
    {
        XtraReport report = sender as XtraReport;

        IEnumerable<XRControl> ETableControls = report.AllControls<XRTable>();

        foreach (XRTable Table in ETableControls)
        {
            if (Table.Tag.ToString() == "WithColor")
            {
                foreach (XRTableRow Row in Table.Rows)
                {
                    foreach (XRTableCell Cell in Row.Cells)
                    {
                        Cell.BorderColor = Color.FromName(Parameters["pColor"].Value.ToString());
                        if (Cell.BackColor == Color.DarkGray && Parameters["pColor"].Value.ToString()!="Black")
                        {
                            Cell.BackColor = Color.FromName(Parameters["pColor"].Value.ToString());
                        }
                    }
                }

            }
        }    
    }

    private void fnChangeColorLines(object sender)
    {
        XtraReport report = sender as XtraReport;

        IEnumerable<XRControl> EControls = report.AllControls<XRLine>();

        foreach (XRLine Line in EControls)
        {
            if (Line.Tag.ToString() == "WithColor")
            {
              Line.ForeColor = Color.FromName(Parameters["pColor"].Value.ToString());  
            }
        }
    }

    private void fnChangeColorCrossBandLines(object sender)
    {
        XtraReport report = sender as XtraReport;

        IEnumerable<XRControl> EControls = report.AllControls<XRCrossBandLine>();

        foreach (XRCrossBandLine CrossLine in EControls)
        {
            if (CrossLine.Tag.ToString() == "WithColor")
            {
                CrossLine.ForeColor = Color.FromName(Parameters["pColor"].Value.ToString());
            }
        }
    }

    private void CellMetodoDePago_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRTableCell CeldaMetodoPago = (XRTableCell)sender;
        if (!string.IsNullOrEmpty(CeldaMetodoPago.Text))
        {
            (sender as XRTableCell).Text = fnComparaMetodoPago(CeldaMetodoPago.Text);
        }
    }

    public string fnComparaMetodoPago(string psMetodo)
    {
        string sDescripcionOut = string.Empty;

        try
        {
            string[] sMetodos;
            if (psMetodo.Contains(","))
            {
                sMetodos = psMetodo.Split(',');
            }
            else
            {
                sMetodos = new string[] { psMetodo };
            }

            foreach (string sMetodo in sMetodos)
            {
                XmlDocument xmlMetodos = new XmlDocument();

                xmlMetodos.Load(HttpRuntime.AppDomainAppPath + "/XML/MetodoPago.xml");

                XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlMetodos.NameTable);
                nsmComprobante.AddNamespace("mp", "mp:ListaMetodosPago");

                XPathNavigator navComprobante = xmlMetodos.CreateNavigator();
                XPathNodeIterator navDetalles = navComprobante.Select("/mp:ListaMetodosPago/mp:MetodosPago", nsmComprobante);

                while (navDetalles.MoveNext())
                {
                    XPathNavigator nodenavigator = navDetalles.Current;

                    if (nodenavigator.HasChildren)
                    {
                        XPathNodeIterator navDetallesMetodos = nodenavigator.Select("mp:MetodoPago", nsmComprobante);

                        while (navDetallesMetodos.MoveNext())
                        {
                            XPathNavigator navDetallesPago = navDetallesMetodos.Current;

                            string Clave = navDetallesPago.SelectSingleNode("@Clave", nsmComprobante).Value;
                            string Descripcion = navDetallesPago.SelectSingleNode("@Descripcion", nsmComprobante).Value;
                            string Estatus = navDetallesPago.SelectSingleNode("@Estatus", nsmComprobante).Value;

                            if (Estatus.Equals("A"))
                            {
                                if (sMetodo.Equals(Clave))
                                {
                                    if (sDescripcionOut.Equals(string.Empty))
                                    {
                                        sDescripcionOut = Clave + " " + Descripcion;
                                    }
                                    else
                                    {
                                        sDescripcionOut = sDescripcionOut + ", " + Clave + " " + Descripcion;
                                    }
                                }
                            }
                        }

                    }
                }
            }

            if (sDescripcionOut.Equals(string.Empty))
            {
                return psMetodo;
            }
        }
        catch (System.Exception)
        {
            sDescripcionOut = string.Empty;
        }
        return sDescripcionOut;
    }

    private void DetailReportExtraInca_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(this.XmlDataPath);

        XmlNamespaceManager nsmComprobante = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmComprobante.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
        nsmComprobante.AddNamespace("nomina12", "http://www.sat.gob.mx/nomina12");

        XmlNodeList HorasExtra = xmlDoc.SelectNodes("//cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Percepciones/nomina12:Percepcion/nomina12:HorasExtra", nsmComprobante);
        XmlNodeList Incapacidades = xmlDoc.SelectNodes("//cfdi:Comprobante/cfdi:Complemento/nomina12:Nomina/nomina12:Incapacidades/nomina12:Incapacidad", nsmComprobante);

        if (HorasExtra.Count == 0 && Incapacidades.Count == 0)
        {
            e.Cancel = true;
        }
        
        if (HorasExtra.Count == 0)
        {
            tblHeaderHorasExtra.Visible = false;
            CrossLineDiasHoras.Visible = false;
            CrossLineHorasExtra.Visible = false;
            CrossLineHorasExtra.Visible = false;
            CrossLineTipoHora.Visible = false;
            SubrHorasExtra.Visible = false;
        }

        if (Incapacidades.Count == 0)
        {
            tblHeaderIncapacidades.Visible = false;
            CrossLineDiasIncapacidades.Visible = false;
            CrossLineTipoInca.Visible = false;
            SubrIncapacidades.Visible = false;
        }


    }


}

using System;
using System.Data;
using System.Globalization;
using System.Text;
using DevExpress.XtraReports.UI;
namespace PAXFacturacionPDF
{
    partial class rptGenericaGratuita33
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rptGenericaGratuita33));
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellFormaPago = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellFecha = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.CellIvaRetencion = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellProveedorCertificacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxTrasladoIVA = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTableCell42 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CampoCalculadoTotal = new DevExpress.XtraReports.UI.CalculatedField();
            this.Detail2 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable9 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow40 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell34 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell35 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell41 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell43 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell44 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow41 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell45 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCrossBandLine2 = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrTable7 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow33 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellQR = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrBarCodeQRGratuita = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrTable6 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow25 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow26 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellSelloDigitalEmisor = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow27 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow28 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow29 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow30 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellCadenaOriginal = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow31 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellCertificadoEmisor = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell39 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow32 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellCertificadoSAT = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow21 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellTotalLetra = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellSubtotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellImpSubtotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow22 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellTotalconLetra = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellIvaTraslado = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellImpIvaTraslado = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow35 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CelllblTipoCambio = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellTipoCambio = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellIepsTraslado = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellImpIepsTraslado = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow34 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellIsrRetencion = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellImpIsrRetencion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow23 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellVacio = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellImpIvaRetencion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow37 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow36 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow24 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellTotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellImpTotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellUUID = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellMetodoPago = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellSerieFolio = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellCveUnidad = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellTituloImporte = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxImpRetencionISR = new DevExpress.XtraReports.UI.CalculatedField();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellRazonReceptor = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow15 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellRFCReceptor = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow16 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellUsoCFDI = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellCondicionesPago = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellLugarExpedicion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellRazonEmisor = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellRFCEmisor = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellRegimenEmisor = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellUnidad = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow17 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellTituloClave = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellNoIdentificacion = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellCantidad = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellPUnitario = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCellSerieFolio = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellDescripcion = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow39 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell36 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell37 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell38 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell40 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable8 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow38 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrCellEmisor = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCellReceptor = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCellUUID = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageInfoPaginas = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrCrossBandLine3 = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.TableEncabezadoConceptos = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow19 = new DevExpress.XtraReports.UI.XRTableRow();
            this.DetailReport1 = new DevExpress.XtraReports.UI.DetailReportBand();
            this.ImporteCampoCalculado = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImpRetencionIVA = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImpTrasladoIVA = new DevExpress.XtraReports.UI.CalculatedField();
            this.PUnitarioCampoCalc = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.LabelLink = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCrossBandLine1 = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.fxTrasladoIEPS = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImpTrasladoIEPS = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrCrossBandLine4 = new DevExpress.XtraReports.UI.XRCrossBandLine();
            this.UUIDCampoCalculado = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxRetencionISR = new DevExpress.XtraReports.UI.CalculatedField();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.fxRetencionIVA = new DevExpress.XtraReports.UI.CalculatedField();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.SubTotalCampoCalculado = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow18 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TableEncabezadoConceptos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellFormaPago,
            this.CellFecha});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // CellFormaPago
            // 
            this.CellFormaPago.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.CellFormaPago.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.FormaPago")});
            this.CellFormaPago.Font = new System.Drawing.Font("Arial Narrow", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellFormaPago.Name = "CellFormaPago";
            this.CellFormaPago.StylePriority.UseBorders = false;
            this.CellFormaPago.StylePriority.UseFont = false;
            this.CellFormaPago.StylePriority.UseTextAlignment = false;
            this.CellFormaPago.Text = "CellFormaPago";
            this.CellFormaPago.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellFormaPago.Weight = 1.5D;
            this.CellFormaPago.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellFormaPago_BeforePrint);
            // 
            // CellFecha
            // 
            this.CellFecha.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.CellFecha.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.Fecha")});
            this.CellFecha.Font = new System.Drawing.Font("Arial Narrow", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellFecha.Name = "CellFecha";
            this.CellFecha.StylePriority.UseBorders = false;
            this.CellFecha.StylePriority.UseFont = false;
            this.CellFecha.StylePriority.UseTextAlignment = false;
            this.CellFecha.Text = "CellFecha";
            this.CellFecha.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellFecha.Weight = 1.5D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell24.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseBorders = false;
            this.xrTableCell24.StylePriority.UseFont = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell24.Weight = 1.836298978279951D;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 30F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // CellIvaRetencion
            // 
            this.CellIvaRetencion.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Impuestos.Impuestos_Retenciones.Retenciones_Retencion.fxRetencionIVA")});
            this.CellIvaRetencion.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellIvaRetencion.Name = "CellIvaRetencion";
            this.CellIvaRetencion.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellIvaRetencion.StylePriority.UseFont = false;
            this.CellIvaRetencion.StylePriority.UsePadding = false;
            this.CellIvaRetencion.StylePriority.UseTextAlignment = false;
            this.CellIvaRetencion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellIvaRetencion.Weight = 0.60854094884275356D;
            // 
            // CellProveedorCertificacion
            // 
            this.CellProveedorCertificacion.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.CellProveedorCertificacion.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.RfcProvCertif")});
            this.CellProveedorCertificacion.Name = "CellProveedorCertificacion";
            this.CellProveedorCertificacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellProveedorCertificacion.StylePriority.UseBorders = false;
            this.CellProveedorCertificacion.StylePriority.UsePadding = false;
            this.CellProveedorCertificacion.Text = "CellProveedorCertificacion";
            this.CellProveedorCertificacion.Weight = 1.03862674041879D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BackColor = System.Drawing.Color.Black;
            this.xrTableCell5.BorderColor = System.Drawing.Color.Black;
            this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell5.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell5.ForeColor = System.Drawing.Color.White;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseBackColor = false;
            this.xrTableCell5.StylePriority.UseBorderColor = false;
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseForeColor = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "FOLIO FISCAL (UUID)";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell5.Weight = 3D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BackColor = System.Drawing.Color.Black;
            this.xrTableCell4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell4.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.ForeColor = System.Drawing.Color.White;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBackColor = false;
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseForeColor = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "EMISOR";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 3D;
            // 
            // fxTrasladoIVA
            // 
            this.fxTrasladoIVA.DataMember = "Impuestos.Impuestos_Traslados.Traslados_Traslado";
            this.fxTrasladoIVA.Expression = "Iif([][[Impuesto]==\'002\'],Concat([][[Impuesto]==\'002\'].Max([Impuesto]), \' IVA \',[" +
    "TipoFactor], \' \',[][[Impuesto]==\'002\'].Max([TasaOCuota])), \'\')";
            this.fxTrasladoIVA.Name = "fxTrasladoIVA";
            // 
            // xrTableCell42
            // 
            this.xrTableCell42.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell42.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell42.Name = "xrTableCell42";
            this.xrTableCell42.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell42.StylePriority.UseBorders = false;
            this.xrTableCell42.StylePriority.UseFont = false;
            this.xrTableCell42.StylePriority.UsePadding = false;
            this.xrTableCell42.Text = "Certificado SAT:";
            this.xrTableCell42.Weight = 0.41201733110288696D;
            // 
            // CampoCalculadoTotal
            // 
            this.CampoCalculadoTotal.DataMember = "Comprobante";
            this.CampoCalculadoTotal.Expression = "[Total]";
            this.CampoCalculadoTotal.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.CampoCalculadoTotal.Name = "CampoCalculadoTotal";
            // 
            // Detail2
            // 
            this.Detail2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable9});
            this.Detail2.HeightF = 52F;
            this.Detail2.Name = "Detail2";
            // 
            // xrTable9
            // 
            this.xrTable9.BackColor = System.Drawing.Color.White;
            this.xrTable9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable9.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable9.ForeColor = System.Drawing.Color.Black;
            this.xrTable9.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable9.Name = "xrTable9";
            this.xrTable9.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow40,
            this.xrTableRow41});
            this.xrTable9.SizeF = new System.Drawing.SizeF(697F, 50F);
            this.xrTable9.StylePriority.UseBackColor = false;
            this.xrTable9.StylePriority.UseBorders = false;
            this.xrTable9.StylePriority.UseFont = false;
            this.xrTable9.StylePriority.UseForeColor = false;
            this.xrTable9.StylePriority.UseTextAlignment = false;
            this.xrTable9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow40
            // 
            this.xrTableRow40.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell31,
            this.xrTableCell32,
            this.xrTableCell34,
            this.xrTableCell35,
            this.xrTableCell41,
            this.xrTableCell43,
            this.xrTableCell44});
            this.xrTableRow40.Name = "xrTableRow40";
            this.xrTableRow40.Weight = 1D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTableCell31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Conceptos.Conceptos_Concepto.ClaveProdServ")});
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.StylePriority.UseBorders = false;
            this.xrTableCell31.Weight = 0.55396557863747709D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Conceptos.Conceptos_Concepto.NoIdentificacion")});
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.Weight = 1.0532182148568661D;
            // 
            // xrTableCell34
            // 
            this.xrTableCell34.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Conceptos.Conceptos_Concepto.ClaveUnidad")});
            this.xrTableCell34.Name = "xrTableCell34";
            this.xrTableCell34.Weight = 0.64971235953178175D;
            // 
            // xrTableCell35
            // 
            this.xrTableCell35.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Conceptos.Conceptos_Concepto.Unidad")});
            this.xrTableCell35.Name = "xrTableCell35";
            this.xrTableCell35.Weight = 0.5608043992554359D;
            // 
            // xrTableCell41
            // 
            this.xrTableCell41.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Conceptos.Conceptos_Concepto.Cantidad")});
            this.xrTableCell41.Name = "xrTableCell41";
            this.xrTableCell41.Weight = 0.5676441221287607D;
            // 
            // xrTableCell43
            // 
            this.xrTableCell43.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Conceptos.Conceptos_Concepto.PUnitarioCampoCalc", "{0:$0.00}")});
            this.xrTableCell43.Name = "xrTableCell43";
            this.xrTableCell43.Weight = 0.64287361951320543D;
            // 
            // xrTableCell44
            // 
            this.xrTableCell44.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell44.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Conceptos.Conceptos_Concepto.ImporteCampoCalculado", "{0:$0.00}")});
            this.xrTableCell44.Name = "xrTableCell44";
            this.xrTableCell44.StylePriority.UseBorders = false;
            this.xrTableCell44.Weight = 0.73178109572491046D;
            // 
            // xrTableRow41
            // 
            this.xrTableRow41.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell45});
            this.xrTableRow41.Name = "xrTableRow41";
            this.xrTableRow41.Weight = 1D;
            // 
            // xrTableCell45
            // 
            this.xrTableCell45.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell45.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Conceptos.Conceptos_Concepto.Descripcion")});
            this.xrTableCell45.Name = "xrTableCell45";
            this.xrTableCell45.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell45.StylePriority.UseBorders = false;
            this.xrTableCell45.StylePriority.UsePadding = false;
            this.xrTableCell45.StylePriority.UseTextAlignment = false;
            this.xrTableCell45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell45.Weight = 4.7599993896484376D;
            // 
            // xrCrossBandLine2
            // 
            this.xrCrossBandLine2.EndBand = this.ReportFooter;
            this.xrCrossBandLine2.EndPointFloat = new DevExpress.Utils.PointFloat(696.9999F, 1.907349E-05F);
            this.xrCrossBandLine2.LocationFloat = new DevExpress.Utils.PointFloat(696.9999F, 0F);
            this.xrCrossBandLine2.Name = "xrCrossBandLine2";
            this.xrCrossBandLine2.StartBand = this.ReportFooter;
            this.xrCrossBandLine2.StartPointFloat = new DevExpress.Utils.PointFloat(696.9999F, 0F);
            this.xrCrossBandLine2.WidthF = 1F;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable7,
            this.xrTable6,
            this.xrTable5});
            this.ReportFooter.HeightF = 305F;
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.PrintAtBottom = true;
            // 
            // xrTable7
            // 
            this.xrTable7.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)));
            this.xrTable7.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable7.Name = "xrTable7";
            this.xrTable7.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow33});
            this.xrTable7.SizeF = new System.Drawing.SizeF(135F, 130F);
            this.xrTable7.StylePriority.UseBorders = false;
            // 
            // xrTableRow33
            // 
            this.xrTableRow33.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellQR});
            this.xrTableRow33.Name = "xrTableRow33";
            this.xrTableRow33.Weight = 1D;
            // 
            // CellQR
            // 
            this.CellQR.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCodeQRGratuita});
            this.CellQR.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.CampoCalculadoTotal")});
            this.CellQR.Name = "CellQR";
            this.CellQR.Weight = 3D;
            // 
            // xrBarCodeQRGratuita
            // 
            this.xrBarCodeQRGratuita.Alignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrBarCodeQRGratuita.AutoModule = true;
            this.xrBarCodeQRGratuita.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrBarCodeQRGratuita.BorderWidth = 1F;
            this.xrBarCodeQRGratuita.Font = new System.Drawing.Font("Helvetica-Normal", 6F);
            this.xrBarCodeQRGratuita.LocationFloat = new DevExpress.Utils.PointFloat(3.000041F, 2F);
            this.xrBarCodeQRGratuita.Module = 3F;
            this.xrBarCodeQRGratuita.Name = "xrBarCodeQRGratuita";
            this.xrBarCodeQRGratuita.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 4, 4, 100F);
            this.xrBarCodeQRGratuita.ShowText = false;
            this.xrBarCodeQRGratuita.SizeF = new System.Drawing.SizeF(135F, 124.25F);
            this.xrBarCodeQRGratuita.StylePriority.UseBorders = false;
            this.xrBarCodeQRGratuita.StylePriority.UseBorderWidth = false;
            this.xrBarCodeQRGratuita.StylePriority.UseFont = false;
            this.xrBarCodeQRGratuita.StylePriority.UsePadding = false;
            this.xrBarCodeQRGratuita.StylePriority.UseTextAlignment = false;
            qrCodeGenerator1.CompactionMode = DevExpress.XtraPrinting.BarCode.QRCodeCompactionMode.Byte;
            qrCodeGenerator1.ErrorCorrectionLevel = DevExpress.XtraPrinting.BarCode.QRCodeErrorCorrectionLevel.Q;
            qrCodeGenerator1.Version = DevExpress.XtraPrinting.BarCode.QRCodeVersion.Version4;
            this.xrBarCodeQRGratuita.Symbology = qrCodeGenerator1;
            this.xrBarCodeQRGratuita.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrBarCodeQRGratuita_BeforePrint);
            // 
            // xrTable6
            // 
            this.xrTable6.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable6.LocationFloat = new DevExpress.Utils.PointFloat(0.9999593F, 128F);
            this.xrTable6.Name = "xrTable6";
            this.xrTable6.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow25,
            this.xrTableRow26,
            this.xrTableRow27,
            this.xrTableRow28,
            this.xrTableRow29,
            this.xrTableRow30,
            this.xrTableRow31,
            this.xrTableRow32});
            this.xrTable6.SizeF = new System.Drawing.SizeF(697F, 177F);
            this.xrTable6.StylePriority.UseFont = false;
            this.xrTable6.StylePriority.UseTextAlignment = false;
            this.xrTable6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow25
            // 
            this.xrTableRow25.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell21});
            this.xrTableRow25.Name = "xrTableRow25";
            this.xrTableRow25.Weight = 1D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell21.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell21.StylePriority.UseBorders = false;
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UsePadding = false;
            this.xrTableCell21.Text = "Sello digital delEmisor:";
            this.xrTableCell21.Weight = 3D;
            // 
            // xrTableRow26
            // 
            this.xrTableRow26.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellSelloDigitalEmisor});
            this.xrTableRow26.Name = "xrTableRow26";
            this.xrTableRow26.Weight = 1D;
            // 
            // CellSelloDigitalEmisor
            // 
            this.CellSelloDigitalEmisor.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellSelloDigitalEmisor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.Sello")});
            this.CellSelloDigitalEmisor.Name = "CellSelloDigitalEmisor";
            this.CellSelloDigitalEmisor.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellSelloDigitalEmisor.StylePriority.UseBorders = false;
            this.CellSelloDigitalEmisor.StylePriority.UsePadding = false;
            this.CellSelloDigitalEmisor.Weight = 3D;
            // 
            // xrTableRow27
            // 
            this.xrTableRow27.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell27});
            this.xrTableRow27.Name = "xrTableRow27";
            this.xrTableRow27.Weight = 1D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell27.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell27.StylePriority.UseBorders = false;
            this.xrTableCell27.StylePriority.UseFont = false;
            this.xrTableCell27.StylePriority.UsePadding = false;
            this.xrTableCell27.Text = "Sello digital del SAT:";
            this.xrTableCell27.Weight = 3D;
            // 
            // xrTableRow28
            // 
            this.xrTableRow28.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell30});
            this.xrTableRow28.Name = "xrTableRow28";
            this.xrTableRow28.Weight = 1D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.SelloSAT")});
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell30.StylePriority.UseBorders = false;
            this.xrTableCell30.StylePriority.UsePadding = false;
            this.xrTableCell30.Weight = 3D;
            // 
            // xrTableRow29
            // 
            this.xrTableRow29.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell33});
            this.xrTableRow29.Name = "xrTableRow29";
            this.xrTableRow29.Weight = 1D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell33.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell33.StylePriority.UseBorders = false;
            this.xrTableCell33.StylePriority.UseFont = false;
            this.xrTableCell33.StylePriority.UsePadding = false;
            this.xrTableCell33.Text = "Cadena Original del Complemento de Certificación del SAT:";
            this.xrTableCell33.Weight = 3D;
            // 
            // xrTableRow30
            // 
            this.xrTableRow30.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellCadenaOriginal});
            this.xrTableRow30.Name = "xrTableRow30";
            this.xrTableRow30.Weight = 1D;
            // 
            // CellCadenaOriginal
            // 
            this.CellCadenaOriginal.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellCadenaOriginal.Name = "CellCadenaOriginal";
            this.CellCadenaOriginal.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellCadenaOriginal.StylePriority.UseBorders = false;
            this.CellCadenaOriginal.StylePriority.UsePadding = false;
            this.CellCadenaOriginal.Text = "CellCadenaOriginal";
            this.CellCadenaOriginal.Weight = 3D;
            // 
            // xrTableRow31
            // 
            this.xrTableRow31.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell22,
            this.CellCertificadoEmisor,
            this.xrTableCell39});
            this.xrTableRow31.Name = "xrTableRow31";
            this.xrTableRow31.Weight = 1D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.xrTableCell22.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell22.StylePriority.UseBorders = false;
            this.xrTableCell22.StylePriority.UseFont = false;
            this.xrTableCell22.StylePriority.UsePadding = false;
            this.xrTableCell22.Text = "Certificado del Emisor:";
            this.xrTableCell22.Weight = 0.55794011062818527D;
            // 
            // CellCertificadoEmisor
            // 
            this.CellCertificadoEmisor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.NoCertificado")});
            this.CellCertificadoEmisor.Name = "CellCertificadoEmisor";
            this.CellCertificadoEmisor.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellCertificadoEmisor.StylePriority.UsePadding = false;
            this.CellCertificadoEmisor.Weight = 0.68240294231365672D;
            // 
            // xrTableCell39
            // 
            this.xrTableCell39.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell39.Name = "xrTableCell39";
            this.xrTableCell39.StylePriority.UseBorders = false;
            this.xrTableCell39.Weight = 1.7596569470581578D;
            // 
            // xrTableRow32
            // 
            this.xrTableRow32.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell42,
            this.CellCertificadoSAT,
            this.xrTableCell23,
            this.CellProveedorCertificacion});
            this.xrTableRow32.Name = "xrTableRow32";
            this.xrTableRow32.Weight = 1D;
            // 
            // CellCertificadoSAT
            // 
            this.CellCertificadoSAT.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.CellCertificadoSAT.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.NoCertificadoSAT")});
            this.CellCertificadoSAT.Name = "CellCertificadoSAT";
            this.CellCertificadoSAT.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellCertificadoSAT.StylePriority.UseBorders = false;
            this.CellCertificadoSAT.StylePriority.UsePadding = false;
            this.CellCertificadoSAT.Weight = 0.82832575458313784D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell23.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell23.StylePriority.UseBorders = false;
            this.xrTableCell23.StylePriority.UseFont = false;
            this.xrTableCell23.StylePriority.UsePadding = false;
            this.xrTableCell23.Text = "Proveedor de Certificación:";
            this.xrTableCell23.Weight = 0.721030173895185D;
            // 
            // xrTable5
            // 
            this.xrTable5.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(136.0001F, 0F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow21,
            this.xrTableRow22,
            this.xrTableRow35,
            this.xrTableRow34,
            this.xrTableRow23,
            this.xrTableRow37,
            this.xrTableRow36,
            this.xrTableRow24});
            this.xrTable5.SizeF = new System.Drawing.SizeF(562F, 128F);
            this.xrTable5.StylePriority.UseBorders = false;
            this.xrTable5.StylePriority.UseFont = false;
            // 
            // xrTableRow21
            // 
            this.xrTableRow21.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellTotalLetra,
            this.CellSubtotal,
            this.CellImpSubtotal});
            this.xrTableRow21.Name = "xrTableRow21";
            this.xrTableRow21.Weight = 1D;
            // 
            // CellTotalLetra
            // 
            this.CellTotalLetra.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellTotalLetra.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellTotalLetra.Name = "CellTotalLetra";
            this.CellTotalLetra.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellTotalLetra.StylePriority.UseBorders = false;
            this.CellTotalLetra.StylePriority.UseFont = false;
            this.CellTotalLetra.StylePriority.UsePadding = false;
            this.CellTotalLetra.StylePriority.UseTextAlignment = false;
            this.CellTotalLetra.Text = "Total con Letra:";
            this.CellTotalLetra.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellTotalLetra.Weight = 1.836298978279951D;
            // 
            // CellSubtotal
            // 
            this.CellSubtotal.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.CellSubtotal.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellSubtotal.Name = "CellSubtotal";
            this.CellSubtotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellSubtotal.StylePriority.UseBorders = false;
            this.CellSubtotal.StylePriority.UseFont = false;
            this.CellSubtotal.StylePriority.UsePadding = false;
            this.CellSubtotal.StylePriority.UseTextAlignment = false;
            this.CellSubtotal.Text = "Subtotal";
            this.CellSubtotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellSubtotal.Weight = 0.60854094884275356D;
            // 
            // CellImpSubtotal
            // 
            this.CellImpSubtotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellImpSubtotal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.SubTotalCampoCalculado", "{0:$0.00}")});
            this.CellImpSubtotal.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellImpSubtotal.Name = "CellImpSubtotal";
            this.CellImpSubtotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellImpSubtotal.StylePriority.UseBorders = false;
            this.CellImpSubtotal.StylePriority.UseFont = false;
            this.CellImpSubtotal.StylePriority.UsePadding = false;
            this.CellImpSubtotal.StylePriority.UseTextAlignment = false;
            this.CellImpSubtotal.Text = "CellImpSubtotal";
            this.CellImpSubtotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellImpSubtotal.Weight = 0.55516007287729563D;
            // 
            // xrTableRow22
            // 
            this.xrTableRow22.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellTotalconLetra,
            this.CellIvaTraslado,
            this.CellImpIvaTraslado});
            this.xrTableRow22.Name = "xrTableRow22";
            this.xrTableRow22.Weight = 1D;
            // 
            // CellTotalconLetra
            // 
            this.CellTotalconLetra.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellTotalconLetra.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.CampoCalculadoTotal")});
            this.CellTotalconLetra.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellTotalconLetra.Name = "CellTotalconLetra";
            this.CellTotalconLetra.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellTotalconLetra.StylePriority.UseBorders = false;
            this.CellTotalconLetra.StylePriority.UseFont = false;
            this.CellTotalconLetra.StylePriority.UsePadding = false;
            this.CellTotalconLetra.StylePriority.UseTextAlignment = false;
            this.CellTotalconLetra.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellTotalconLetra.Weight = 1.836298978279951D;
            this.CellTotalconLetra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellTotalconLetra_BeforePrint);
            // 
            // CellIvaTraslado
            // 
            this.CellIvaTraslado.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Impuestos.Impuestos_Traslados.Traslados_Traslado.fxTrasladoIVA")});
            this.CellIvaTraslado.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellIvaTraslado.Name = "CellIvaTraslado";
            this.CellIvaTraslado.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellIvaTraslado.StylePriority.UseFont = false;
            this.CellIvaTraslado.StylePriority.UsePadding = false;
            this.CellIvaTraslado.StylePriority.UseTextAlignment = false;
            this.CellIvaTraslado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellIvaTraslado.Weight = 0.60854094884275356D;
            // 
            // CellImpIvaTraslado
            // 
            this.CellImpIvaTraslado.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.CellImpIvaTraslado.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Impuestos.Impuestos_Traslados.Traslados_Traslado.fxImpTrasladoIVA", "{0:$0.00}")});
            this.CellImpIvaTraslado.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellImpIvaTraslado.Name = "CellImpIvaTraslado";
            this.CellImpIvaTraslado.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellImpIvaTraslado.StylePriority.UseBorders = false;
            this.CellImpIvaTraslado.StylePriority.UseFont = false;
            this.CellImpIvaTraslado.StylePriority.UsePadding = false;
            this.CellImpIvaTraslado.StylePriority.UseTextAlignment = false;
            this.CellImpIvaTraslado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellImpIvaTraslado.Weight = 0.55516007287729563D;
            // 
            // xrTableRow35
            // 
            this.xrTableRow35.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CelllblTipoCambio,
            this.CellTipoCambio,
            this.CellIepsTraslado,
            this.CellImpIepsTraslado});
            this.xrTableRow35.Name = "xrTableRow35";
            this.xrTableRow35.Weight = 1D;
            // 
            // CelllblTipoCambio
            // 
            this.CelllblTipoCambio.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.CelllblTipoCambio.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CelllblTipoCambio.Name = "CelllblTipoCambio";
            this.CelllblTipoCambio.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CelllblTipoCambio.StylePriority.UseBorders = false;
            this.CelllblTipoCambio.StylePriority.UseFont = false;
            this.CelllblTipoCambio.StylePriority.UsePadding = false;
            this.CelllblTipoCambio.StylePriority.UseTextAlignment = false;
            this.CelllblTipoCambio.Text = "Tipo de Cambio:";
            this.CelllblTipoCambio.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CelllblTipoCambio.Weight = 0.46441222344112509D;
            // 
            // CellTipoCambio
            // 
            this.CellTipoCambio.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.CellTipoCambio.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellTipoCambio.Name = "CellTipoCambio";
            this.CellTipoCambio.StylePriority.UseBorders = false;
            this.CellTipoCambio.StylePriority.UseFont = false;
            this.CellTipoCambio.StylePriority.UseTextAlignment = false;
            this.CellTipoCambio.Text = "[Comprobante.TipoCambio]";
            this.CellTipoCambio.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellTipoCambio.Weight = 1.3718867548388258D;
            // 
            // CellIepsTraslado
            // 
            this.CellIepsTraslado.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Impuestos.Impuestos_Traslados.Traslados_Traslado.fxTrasladoIEPS")});
            this.CellIepsTraslado.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellIepsTraslado.Name = "CellIepsTraslado";
            this.CellIepsTraslado.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellIepsTraslado.StylePriority.UseFont = false;
            this.CellIepsTraslado.StylePriority.UsePadding = false;
            this.CellIepsTraslado.StylePriority.UseTextAlignment = false;
            this.CellIepsTraslado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellIepsTraslado.Weight = 0.60854094884275356D;
            // 
            // CellImpIepsTraslado
            // 
            this.CellImpIepsTraslado.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.CellImpIepsTraslado.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Impuestos.Impuestos_Traslados.Traslados_Traslado.fxImpTrasladoIEPS", "{0:$0.00}")});
            this.CellImpIepsTraslado.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellImpIepsTraslado.Name = "CellImpIepsTraslado";
            this.CellImpIepsTraslado.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellImpIepsTraslado.StylePriority.UseBorders = false;
            this.CellImpIepsTraslado.StylePriority.UseFont = false;
            this.CellImpIepsTraslado.StylePriority.UsePadding = false;
            this.CellImpIepsTraslado.StylePriority.UseTextAlignment = false;
            this.CellImpIepsTraslado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellImpIepsTraslado.Weight = 0.55516007287729563D;
            // 
            // xrTableRow34
            // 
            this.xrTableRow34.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell24,
            this.CellIsrRetencion,
            this.CellImpIsrRetencion});
            this.xrTableRow34.Name = "xrTableRow34";
            this.xrTableRow34.Weight = 1D;
            // 
            // CellIsrRetencion
            // 
            this.CellIsrRetencion.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Impuestos.Impuestos_Retenciones.Retenciones_Retencion.fxRetencionISR")});
            this.CellIsrRetencion.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellIsrRetencion.Name = "CellIsrRetencion";
            this.CellIsrRetencion.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellIsrRetencion.StylePriority.UseFont = false;
            this.CellIsrRetencion.StylePriority.UsePadding = false;
            this.CellIsrRetencion.StylePriority.UseTextAlignment = false;
            this.CellIsrRetencion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellIsrRetencion.Weight = 0.60854094884275356D;
            // 
            // CellImpIsrRetencion
            // 
            this.CellImpIsrRetencion.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.CellImpIsrRetencion.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Impuestos.Impuestos_Retenciones.Retenciones_Retencion.fxImpRetencionISR", "{0:$0.00}")});
            this.CellImpIsrRetencion.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellImpIsrRetencion.Name = "CellImpIsrRetencion";
            this.CellImpIsrRetencion.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellImpIsrRetencion.StylePriority.UseBorders = false;
            this.CellImpIsrRetencion.StylePriority.UseFont = false;
            this.CellImpIsrRetencion.StylePriority.UsePadding = false;
            this.CellImpIsrRetencion.StylePriority.UseTextAlignment = false;
            this.CellImpIsrRetencion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellImpIsrRetencion.Weight = 0.55516007287729563D;
            // 
            // xrTableRow23
            // 
            this.xrTableRow23.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellVacio,
            this.CellIvaRetencion,
            this.CellImpIvaRetencion});
            this.xrTableRow23.Name = "xrTableRow23";
            this.xrTableRow23.Weight = 1D;
            // 
            // CellVacio
            // 
            this.CellVacio.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellVacio.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellVacio.Name = "CellVacio";
            this.CellVacio.StylePriority.UseBorders = false;
            this.CellVacio.StylePriority.UseFont = false;
            this.CellVacio.StylePriority.UseTextAlignment = false;
            this.CellVacio.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellVacio.Weight = 1.836298978279951D;
            // 
            // CellImpIvaRetencion
            // 
            this.CellImpIvaRetencion.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.CellImpIvaRetencion.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Impuestos.Impuestos_Retenciones.Retenciones_Retencion.fxImpRetencionIVA", "{0:$0.00}")});
            this.CellImpIvaRetencion.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellImpIvaRetencion.Name = "CellImpIvaRetencion";
            this.CellImpIvaRetencion.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellImpIvaRetencion.StylePriority.UseBorders = false;
            this.CellImpIvaRetencion.StylePriority.UseFont = false;
            this.CellImpIvaRetencion.StylePriority.UsePadding = false;
            this.CellImpIvaRetencion.StylePriority.UseTextAlignment = false;
            this.CellImpIvaRetencion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellImpIvaRetencion.Weight = 0.55516007287729563D;
            // 
            // xrTableRow37
            // 
            this.xrTableRow37.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell26,
            this.xrTableCell28,
            this.xrTableCell29});
            this.xrTableRow37.Name = "xrTableRow37";
            this.xrTableRow37.Weight = 1D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell26.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseBorders = false;
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell26.Weight = 1.836298978279951D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseFont = false;
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell28.Weight = 0.60854094884275356D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell29.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseBorders = false;
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.StylePriority.UseTextAlignment = false;
            this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell29.Weight = 0.55516007287729563D;
            // 
            // xrTableRow36
            // 
            this.xrTableRow36.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell25});
            this.xrTableRow36.Name = "xrTableRow36";
            this.xrTableRow36.Weight = 1D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell19.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseBorders = false;
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell19.Weight = 1.836298978279951D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell20.Weight = 0.60854094884275356D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell25.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseBorders = false;
            this.xrTableCell25.StylePriority.UseFont = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell25.Weight = 0.55516007287729563D;
            // 
            // xrTableRow24
            // 
            this.xrTableRow24.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18,
            this.CellTotal,
            this.CellImpTotal});
            this.xrTableRow24.Name = "xrTableRow24";
            this.xrTableRow24.Weight = 1D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.xrTableCell18.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseBorders = false;
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell18.Weight = 1.836298978279951D;
            // 
            // CellTotal
            // 
            this.CellTotal.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.CellTotal.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellTotal.Name = "CellTotal";
            this.CellTotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellTotal.StylePriority.UseBorders = false;
            this.CellTotal.StylePriority.UseFont = false;
            this.CellTotal.StylePriority.UsePadding = false;
            this.CellTotal.StylePriority.UseTextAlignment = false;
            this.CellTotal.Text = "TOTAL";
            this.CellTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellTotal.Weight = 0.60854094884275356D;
            // 
            // CellImpTotal
            // 
            this.CellImpTotal.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellImpTotal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.CampoCalculadoTotal", "{0:$0.00}")});
            this.CellImpTotal.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellImpTotal.Name = "CellImpTotal";
            this.CellImpTotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellImpTotal.StylePriority.UseBorders = false;
            this.CellImpTotal.StylePriority.UseFont = false;
            this.CellImpTotal.StylePriority.UsePadding = false;
            this.CellImpTotal.StylePriority.UseTextAlignment = false;
            this.CellImpTotal.Text = "CellImpTotal";
            this.CellImpTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellImpTotal.Weight = 0.55516007287729563D;
            // 
            // CellUUID
            // 
            this.CellUUID.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellUUID.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.UUIDCampoCalculado")});
            this.CellUUID.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellUUID.Name = "CellUUID";
            this.CellUUID.StylePriority.UseBorders = false;
            this.CellUUID.StylePriority.UseFont = false;
            this.CellUUID.StylePriority.UseTextAlignment = false;
            this.CellUUID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellUUID.Weight = 3D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellMetodoPago,
            this.CellSerieFolio});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // CellMetodoPago
            // 
            this.CellMetodoPago.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellMetodoPago.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.MetodoPago")});
            this.CellMetodoPago.Font = new System.Drawing.Font("Arial Narrow", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellMetodoPago.Name = "CellMetodoPago";
            this.CellMetodoPago.StylePriority.UseBorders = false;
            this.CellMetodoPago.StylePriority.UseFont = false;
            this.CellMetodoPago.StylePriority.UseTextAlignment = false;
            this.CellMetodoPago.Text = "CellMetodoPago";
            this.CellMetodoPago.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellMetodoPago.Weight = 1.5D;
            this.CellMetodoPago.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellMetodoPago_BeforePrint);
            // 
            // CellSerieFolio
            // 
            this.CellSerieFolio.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.CellSerieFolio.Font = new System.Drawing.Font("Arial Narrow", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellSerieFolio.ForeColor = System.Drawing.Color.Red;
            this.CellSerieFolio.Name = "CellSerieFolio";
            this.CellSerieFolio.StylePriority.UseBorders = false;
            this.CellSerieFolio.StylePriority.UseFont = false;
            this.CellSerieFolio.StylePriority.UseForeColor = false;
            this.CellSerieFolio.StylePriority.UseTextAlignment = false;
            this.CellSerieFolio.Text = "[Comprobante.Serie][COmprobante.Folio]";
            this.CellSerieFolio.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellSerieFolio.Weight = 1.5D;
            // 
            // CellCveUnidad
            // 
            this.CellCveUnidad.Name = "CellCveUnidad";
            this.CellCveUnidad.Text = "Cve. Unidad";
            this.CellCveUnidad.Weight = 0.64971235953178175D;
            // 
            // CellTituloImporte
            // 
            this.CellTituloImporte.Name = "CellTituloImporte";
            this.CellTituloImporte.Text = "Importe";
            this.CellTituloImporte.Weight = 0.73178109572491046D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.BackColor = System.Drawing.Color.Black;
            this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell11.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell11.ForeColor = System.Drawing.Color.White;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseBackColor = false;
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.StylePriority.UseForeColor = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "FECHA";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 1.5D;
            // 
            // fxImpRetencionISR
            // 
            this.fxImpRetencionISR.DataMember = "Impuestos.Impuestos_Retenciones.Retenciones_Retencion";
            this.fxImpRetencionISR.Expression = "[][[Impuesto]==\'001\'].Sum(ToDouble([Importe]))";
            this.fxImpRetencionISR.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImpRetencionISR.Name = "fxImpRetencionISR";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3,
            this.xrTable2,
            this.xrTable1});
            this.ReportHeader.HeightF = 203.4583F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrTable3
            // 
            this.xrTable3.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0.9999593F, 104F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow13,
            this.xrTableRow14,
            this.xrTableRow15,
            this.xrTableRow16});
            this.xrTable3.SizeF = new System.Drawing.SizeF(305F, 97.99998F);
            this.xrTable3.StylePriority.UseFont = false;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.BackColor = System.Drawing.Color.Black;
            this.xrTableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell13.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.ForeColor = System.Drawing.Color.White;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseBackColor = false;
            this.xrTableCell13.StylePriority.UseBorders = false;
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UseForeColor = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "RECEPTOR";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell13.Weight = 3D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellRazonReceptor});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // CellRazonReceptor
            // 
            this.CellRazonReceptor.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellRazonReceptor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.Nombre")});
            this.CellRazonReceptor.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellRazonReceptor.Name = "CellRazonReceptor";
            this.CellRazonReceptor.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellRazonReceptor.StylePriority.UseBorders = false;
            this.CellRazonReceptor.StylePriority.UseFont = false;
            this.CellRazonReceptor.StylePriority.UsePadding = false;
            this.CellRazonReceptor.StylePriority.UseTextAlignment = false;
            this.CellRazonReceptor.Text = "xrTableCell1";
            this.CellRazonReceptor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellRazonReceptor.Weight = 3D;
            // 
            // xrTableRow15
            // 
            this.xrTableRow15.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellRFCReceptor});
            this.xrTableRow15.Name = "xrTableRow15";
            this.xrTableRow15.Weight = 1D;
            // 
            // CellRFCReceptor
            // 
            this.CellRFCReceptor.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellRFCReceptor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.Rfc")});
            this.CellRFCReceptor.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellRFCReceptor.Name = "CellRFCReceptor";
            this.CellRFCReceptor.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellRFCReceptor.StylePriority.UseBorders = false;
            this.CellRFCReceptor.StylePriority.UseFont = false;
            this.CellRFCReceptor.StylePriority.UsePadding = false;
            this.CellRFCReceptor.StylePriority.UseTextAlignment = false;
            this.CellRFCReceptor.Text = "xrTableCell3";
            this.CellRFCReceptor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellRFCReceptor.Weight = 3D;
            // 
            // xrTableRow16
            // 
            this.xrTableRow16.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellUsoCFDI});
            this.xrTableRow16.Name = "xrTableRow16";
            this.xrTableRow16.Weight = 1D;
            // 
            // CellUsoCFDI
            // 
            this.CellUsoCFDI.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.CellUsoCFDI.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.UsoCFDI")});
            this.CellUsoCFDI.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellUsoCFDI.Name = "CellUsoCFDI";
            this.CellUsoCFDI.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellUsoCFDI.StylePriority.UseBorders = false;
            this.CellUsoCFDI.StylePriority.UseFont = false;
            this.CellUsoCFDI.StylePriority.UsePadding = false;
            this.CellUsoCFDI.StylePriority.UseTextAlignment = false;
            this.CellUsoCFDI.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellUsoCFDI.Weight = 3D;
            this.CellUsoCFDI.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellUsoCFDI_BeforePrint);
            // 
            // xrTable2
            // 
            this.xrTable2.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(307.9999F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3,
            this.xrTableRow4,
            this.xrTableRow7,
            this.xrTableRow8,
            this.xrTableRow9,
            this.xrTableRow10,
            this.xrTableRow11,
            this.xrTableRow12});
            this.xrTable2.SizeF = new System.Drawing.SizeF(390.0001F, 202F);
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellUUID});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell7});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BackColor = System.Drawing.Color.Black;
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.ForeColor = System.Drawing.Color.White;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseForeColor = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "METODO DE PAGO";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 1.5D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.BackColor = System.Drawing.Color.Black;
            this.xrTableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell7.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell7.ForeColor = System.Drawing.Color.White;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseBackColor = false;
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UseForeColor = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "FACTURA";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 1.5D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell9});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.Color.Black;
            this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell2.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.ForeColor = System.Drawing.Color.White;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseForeColor = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "CONDICIONES DE PAGO";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 1.5D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BackColor = System.Drawing.Color.Black;
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.ForeColor = System.Drawing.Color.White;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBackColor = false;
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseForeColor = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "LUGAR DE EXPEDICION";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 1.5D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellCondicionesPago,
            this.CellLugarExpedicion});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // CellCondicionesPago
            // 
            this.CellCondicionesPago.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellCondicionesPago.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.CondicionesDePago")});
            this.CellCondicionesPago.Font = new System.Drawing.Font("Arial Narrow", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellCondicionesPago.Name = "CellCondicionesPago";
            this.CellCondicionesPago.StylePriority.UseBorders = false;
            this.CellCondicionesPago.StylePriority.UseFont = false;
            this.CellCondicionesPago.StylePriority.UseTextAlignment = false;
            this.CellCondicionesPago.Text = "CellCondicionesPago";
            this.CellCondicionesPago.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellCondicionesPago.Weight = 1.5D;
            // 
            // CellLugarExpedicion
            // 
            this.CellLugarExpedicion.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.CellLugarExpedicion.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Comprobante.LugarExpedicion")});
            this.CellLugarExpedicion.Font = new System.Drawing.Font("Arial Narrow", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellLugarExpedicion.Name = "CellLugarExpedicion";
            this.CellLugarExpedicion.StylePriority.UseBorders = false;
            this.CellLugarExpedicion.StylePriority.UseFont = false;
            this.CellLugarExpedicion.StylePriority.UseTextAlignment = false;
            this.CellLugarExpedicion.Text = "CellLugarExpedicion";
            this.CellLugarExpedicion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellLugarExpedicion.Weight = 1.5D;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell11});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BackColor = System.Drawing.Color.Black;
            this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell3.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.ForeColor = System.Drawing.Color.White;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBackColor = false;
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseForeColor = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "FORMA DE PAGO";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 1.5D;
            // 
            // xrTable1
            // 
            this.xrTable1.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2,
            this.xrTableRow1,
            this.xrTableRow6,
            this.xrTableRow5});
            this.xrTable1.SizeF = new System.Drawing.SizeF(305F, 101F);
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellRazonEmisor});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // CellRazonEmisor
            // 
            this.CellRazonEmisor.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellRazonEmisor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Emisor.Nombre")});
            this.CellRazonEmisor.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellRazonEmisor.Name = "CellRazonEmisor";
            this.CellRazonEmisor.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellRazonEmisor.StylePriority.UseBorders = false;
            this.CellRazonEmisor.StylePriority.UseFont = false;
            this.CellRazonEmisor.StylePriority.UsePadding = false;
            this.CellRazonEmisor.StylePriority.UseTextAlignment = false;
            this.CellRazonEmisor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellRazonEmisor.Weight = 3D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellRFCEmisor});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // CellRFCEmisor
            // 
            this.CellRFCEmisor.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
            this.CellRFCEmisor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Emisor.Rfc")});
            this.CellRFCEmisor.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellRFCEmisor.Name = "CellRFCEmisor";
            this.CellRFCEmisor.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellRFCEmisor.StylePriority.UseBorders = false;
            this.CellRFCEmisor.StylePriority.UseFont = false;
            this.CellRFCEmisor.StylePriority.UsePadding = false;
            this.CellRFCEmisor.StylePriority.UseTextAlignment = false;
            this.CellRFCEmisor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellRFCEmisor.Weight = 3D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellRegimenEmisor});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // CellRegimenEmisor
            // 
            this.CellRegimenEmisor.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.CellRegimenEmisor.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Emisor.RegimenFiscal")});
            this.CellRegimenEmisor.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellRegimenEmisor.Name = "CellRegimenEmisor";
            this.CellRegimenEmisor.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellRegimenEmisor.StylePriority.UseBorders = false;
            this.CellRegimenEmisor.StylePriority.UseFont = false;
            this.CellRegimenEmisor.StylePriority.UsePadding = false;
            this.CellRegimenEmisor.StylePriority.UseTextAlignment = false;
            this.CellRegimenEmisor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellRegimenEmisor.Weight = 3D;
            this.CellRegimenEmisor.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellRegimenEmisor_BeforePrint);
            // 
            // CellUnidad
            // 
            this.CellUnidad.Name = "CellUnidad";
            this.CellUnidad.Text = "Unidad";
            this.CellUnidad.Weight = 0.5608043992554359D;
            // 
            // xrTableRow17
            // 
            this.xrTableRow17.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellTituloClave,
            this.CellNoIdentificacion,
            this.CellCveUnidad,
            this.CellUnidad,
            this.CellCantidad,
            this.CellPUnitario,
            this.CellTituloImporte});
            this.xrTableRow17.Name = "xrTableRow17";
            this.xrTableRow17.Weight = 1D;
            // 
            // CellTituloClave
            // 
            this.CellTituloClave.Name = "CellTituloClave";
            this.CellTituloClave.StylePriority.UseBorderColor = false;
            this.CellTituloClave.Text = "Clave P/S";
            this.CellTituloClave.Weight = 0.55396557863747709D;
            // 
            // CellNoIdentificacion
            // 
            this.CellNoIdentificacion.Name = "CellNoIdentificacion";
            this.CellNoIdentificacion.Text = "No. De Identificación";
            this.CellNoIdentificacion.Weight = 1.0532182148568661D;
            // 
            // CellCantidad
            // 
            this.CellCantidad.Name = "CellCantidad";
            this.CellCantidad.Text = "Cantidad";
            this.CellCantidad.Weight = 0.5676441221287607D;
            // 
            // CellPUnitario
            // 
            this.CellPUnitario.Name = "CellPUnitario";
            this.CellPUnitario.Text = "P. Unitario";
            this.CellPUnitario.Weight = 0.64287361951320543D;
            // 
            // xrCellSerieFolio
            // 
            this.xrCellSerieFolio.Name = "xrCellSerieFolio";
            this.xrCellSerieFolio.Text = "Factura (Comprobante de ingreso)";
            this.xrCellSerieFolio.Weight = 0.88793129756532863D;
            // 
            // CellDescripcion
            // 
            this.CellDescripcion.Name = "CellDescripcion";
            this.CellDescripcion.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.CellDescripcion.StylePriority.UsePadding = false;
            this.CellDescripcion.StylePriority.UseTextAlignment = false;
            this.CellDescripcion.Text = "Descripción";
            this.CellDescripcion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellDescripcion.Weight = 4.7599993896484376D;
            // 
            // xrTableRow39
            // 
            this.xrTableRow39.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell36,
            this.xrTableCell37,
            this.xrTableCell38,
            this.xrTableCell40});
            this.xrTableRow39.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableRow39.Name = "xrTableRow39";
            this.xrTableRow39.StylePriority.UseFont = false;
            this.xrTableRow39.StylePriority.UseTextAlignment = false;
            this.xrTableRow39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableRow39.Weight = 1D;
            // 
            // xrTableCell36
            // 
            this.xrTableCell36.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Emisor.Rfc")});
            this.xrTableCell36.Name = "xrTableCell36";
            this.xrTableCell36.Text = "xrTableCell36";
            this.xrTableCell36.Weight = 0.51724137931034486D;
            // 
            // xrTableCell37
            // 
            this.xrTableCell37.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Receptor.Rfc")});
            this.xrTableCell37.Name = "xrTableCell37";
            this.xrTableCell37.Text = "xrTableCell37";
            this.xrTableCell37.Weight = 0.51724137931034475D;
            // 
            // xrTableCell38
            // 
            this.xrTableCell38.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TimbreFiscalDigital.UUIDCampoCalculado")});
            this.xrTableCell38.Name = "xrTableCell38";
            this.xrTableCell38.Text = "xrTableCell38";
            this.xrTableCell38.Weight = 1.0775862068965516D;
            // 
            // xrTableCell40
            // 
            this.xrTableCell40.ForeColor = System.Drawing.Color.Red;
            this.xrTableCell40.Name = "xrTableCell40";
            this.xrTableCell40.StylePriority.UseForeColor = false;
            this.xrTableCell40.Text = "[Comprobante.Serie][Comprobante.Folio]";
            this.xrTableCell40.Weight = 0.88793103448275856D;
            // 
            // xrTable8
            // 
            this.xrTable8.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable8.LocationFloat = new DevExpress.Utils.PointFloat(0.9999592F, 0F);
            this.xrTable8.Name = "xrTable8";
            this.xrTable8.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow38,
            this.xrTableRow39});
            this.xrTable8.SizeF = new System.Drawing.SizeF(697F, 54.99998F);
            this.xrTable8.StylePriority.UseBorders = false;
            // 
            // xrTableRow38
            // 
            this.xrTableRow38.BackColor = System.Drawing.Color.Black;
            this.xrTableRow38.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrCellEmisor,
            this.xrCellReceptor,
            this.xrCellUUID,
            this.xrCellSerieFolio});
            this.xrTableRow38.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableRow38.ForeColor = System.Drawing.Color.White;
            this.xrTableRow38.Name = "xrTableRow38";
            this.xrTableRow38.StylePriority.UseBackColor = false;
            this.xrTableRow38.StylePriority.UseFont = false;
            this.xrTableRow38.StylePriority.UseForeColor = false;
            this.xrTableRow38.StylePriority.UseTextAlignment = false;
            this.xrTableRow38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableRow38.Weight = 1D;
            // 
            // xrCellEmisor
            // 
            this.xrCellEmisor.Name = "xrCellEmisor";
            this.xrCellEmisor.Text = "EMISOR";
            this.xrCellEmisor.Weight = 0.517241149113096D;
            // 
            // xrCellReceptor
            // 
            this.xrCellReceptor.Name = "xrCellReceptor";
            this.xrCellReceptor.Text = "RECEPTOR";
            this.xrCellReceptor.Weight = 0.51724134642502362D;
            // 
            // xrCellUUID
            // 
            this.xrCellUUID.Name = "xrCellUUID";
            this.xrCellUUID.Text = "FOLIO FISCAL (UUID)";
            this.xrCellUUID.Weight = 1.0775862068965518D;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(193F, 9.999974F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(328.9999F, 10.00004F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "ESTE DOCUMENTO ES UNA REPRESENTACIÓN GRÁFICA DE UN CFDI";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // PageInfoPaginas
            // 
            this.PageInfoPaginas.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PageInfoPaginas.Format = "{0} de {1}";
            this.PageInfoPaginas.LocationFloat = new DevExpress.Utils.PointFloat(587F, 10F);
            this.PageInfoPaginas.Name = "PageInfoPaginas";
            this.PageInfoPaginas.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.PageInfoPaginas.SizeF = new System.Drawing.SizeF(99.99994F, 10.00004F);
            this.PageInfoPaginas.StylePriority.UseFont = false;
            this.PageInfoPaginas.StylePriority.UseTextAlignment = false;
            this.PageInfoPaginas.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(5.000051F, 7.000001F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(40.99998F, 23.00005F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrCrossBandLine3
            // 
            this.xrCrossBandLine3.EndBand = this.ReportFooter;
            this.xrCrossBandLine3.EndPointFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.xrCrossBandLine3.LocationFloat = new DevExpress.Utils.PointFloat(1F, 50F);
            this.xrCrossBandLine3.Name = "xrCrossBandLine3";
            this.xrCrossBandLine3.StartBand = this.Detail2;
            this.xrCrossBandLine3.StartPointFloat = new DevExpress.Utils.PointFloat(1F, 50F);
            this.xrCrossBandLine3.WidthF = 1.000016F;
            // 
            // TableEncabezadoConceptos
            // 
            this.TableEncabezadoConceptos.BackColor = System.Drawing.Color.Black;
            this.TableEncabezadoConceptos.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.TableEncabezadoConceptos.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TableEncabezadoConceptos.ForeColor = System.Drawing.Color.White;
            this.TableEncabezadoConceptos.LocationFloat = new DevExpress.Utils.PointFloat(1F, 0F);
            this.TableEncabezadoConceptos.Name = "TableEncabezadoConceptos";
            this.TableEncabezadoConceptos.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow17,
            this.xrTableRow19});
            this.TableEncabezadoConceptos.SizeF = new System.Drawing.SizeF(697F, 49.99999F);
            this.TableEncabezadoConceptos.StylePriority.UseBackColor = false;
            this.TableEncabezadoConceptos.StylePriority.UseBorders = false;
            this.TableEncabezadoConceptos.StylePriority.UseFont = false;
            this.TableEncabezadoConceptos.StylePriority.UseForeColor = false;
            this.TableEncabezadoConceptos.StylePriority.UseTextAlignment = false;
            this.TableEncabezadoConceptos.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow19
            // 
            this.xrTableRow19.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellDescripcion});
            this.xrTableRow19.Name = "xrTableRow19";
            this.xrTableRow19.Weight = 1D;
            // 
            // DetailReport1
            // 
            this.DetailReport1.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail2});
            this.DetailReport1.DataMember = "Conceptos.Conceptos_Concepto";
            this.DetailReport1.Level = 0;
            this.DetailReport1.Name = "DetailReport1";
            // 
            // ImporteCampoCalculado
            // 
            this.ImporteCampoCalculado.DataMember = "Conceptos.Conceptos_Concepto";
            this.ImporteCampoCalculado.Expression = "[Importe]";
            this.ImporteCampoCalculado.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.ImporteCampoCalculado.Name = "ImporteCampoCalculado";
            // 
            // fxImpRetencionIVA
            // 
            this.fxImpRetencionIVA.DataMember = "Impuestos.Impuestos_Retenciones.Retenciones_Retencion";
            this.fxImpRetencionIVA.Expression = "[][[Impuesto]==\'002\'].Sum(ToDouble([Importe]))";
            this.fxImpRetencionIVA.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImpRetencionIVA.Name = "fxImpRetencionIVA";
            // 
            // fxImpTrasladoIVA
            // 
            this.fxImpTrasladoIVA.DataMember = "Impuestos.Impuestos_Traslados.Traslados_Traslado";
            this.fxImpTrasladoIVA.Expression = "[][[Impuesto]==\'002\'].Sum(ToDouble([Importe]))";
            this.fxImpTrasladoIVA.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImpTrasladoIVA.Name = "fxImpTrasladoIVA";
            // 
            // PUnitarioCampoCalc
            // 
            this.PUnitarioCampoCalc.DataMember = "Conceptos.Conceptos_Concepto";
            this.PUnitarioCampoCalc.Expression = "[ValorUnitario]";
            this.PUnitarioCampoCalc.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.PUnitarioCampoCalc.Name = "PUnitarioCampoCalc";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.TableEncabezadoConceptos});
            this.GroupHeader1.HeightF = 49.99999F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1,
            this.GroupHeader1,
            this.DetailReport1});
            this.DetailReport.DataMember = "Conceptos";
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.HeightF = 0F;
            this.Detail1.Name = "Detail1";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.PageInfoPaginas,
            this.LabelLink,
            this.xrLabel8});
            this.PageFooter.HeightF = 35F;
            this.PageFooter.Name = "PageFooter";
            // 
            // LabelLink
            // 
            this.LabelLink.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelLink.ForeColor = System.Drawing.Color.Blue;
            this.LabelLink.LocationFloat = new DevExpress.Utils.PointFloat(48F, 6F);
            this.LabelLink.Name = "LabelLink";
            this.LabelLink.NavigateUrl = "http://www.paxfacturacion.com.mx";
            this.LabelLink.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.LabelLink.SizeF = new System.Drawing.SizeF(127F, 22.99998F);
            this.LabelLink.StylePriority.UseFont = false;
            this.LabelLink.StylePriority.UseForeColor = false;
            this.LabelLink.StylePriority.UseTextAlignment = false;
            this.LabelLink.Text = "www.paxfacturacion.com.mx";
            this.LabelLink.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrCrossBandLine1
            // 
            this.xrCrossBandLine1.EndBand = this.ReportFooter;
            this.xrCrossBandLine1.EndPointFloat = new DevExpress.Utils.PointFloat(0.9999859F, 1.000004F);
            this.xrCrossBandLine1.LocationFloat = new DevExpress.Utils.PointFloat(0.9999859F, 0F);
            this.xrCrossBandLine1.Name = "xrCrossBandLine1";
            this.xrCrossBandLine1.StartBand = this.ReportFooter;
            this.xrCrossBandLine1.StartPointFloat = new DevExpress.Utils.PointFloat(0.9999859F, 0F);
            this.xrCrossBandLine1.WidthF = 1F;
            // 
            // fxTrasladoIEPS
            // 
            this.fxTrasladoIEPS.DataMember = "Impuestos.Impuestos_Traslados.Traslados_Traslado";
            this.fxTrasladoIEPS.Expression = "Iif([][[Impuesto]==\'003\'],Concat([][[Impuesto]==\'003\'].Max([Impuesto]), \' IEPS \'," +
    " [TipoFactor], \' \',[][[Impuesto]==\'003\'].Max([TasaOCuota])), \'\')";
            this.fxTrasladoIEPS.Name = "fxTrasladoIEPS";
            // 
            // fxImpTrasladoIEPS
            // 
            this.fxImpTrasladoIEPS.DataMember = "Impuestos.Impuestos_Traslados.Traslados_Traslado";
            this.fxImpTrasladoIEPS.Expression = "[][[Impuesto]==\'003\'].Sum(ToDouble([Importe]))";
            this.fxImpTrasladoIEPS.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImpTrasladoIEPS.Name = "fxImpTrasladoIEPS";
            // 
            // xrCrossBandLine4
            // 
            this.xrCrossBandLine4.EndBand = this.ReportFooter;
            this.xrCrossBandLine4.EndPointFloat = new DevExpress.Utils.PointFloat(697F, 0F);
            this.xrCrossBandLine4.LocationFloat = new DevExpress.Utils.PointFloat(697F, 50F);
            this.xrCrossBandLine4.Name = "xrCrossBandLine4";
            this.xrCrossBandLine4.StartBand = this.Detail2;
            this.xrCrossBandLine4.StartPointFloat = new DevExpress.Utils.PointFloat(697F, 50F);
            this.xrCrossBandLine4.WidthF = 1F;
            // 
            // UUIDCampoCalculado
            // 
            this.UUIDCampoCalculado.DataMember = "TimbreFiscalDigital";
            this.UUIDCampoCalculado.Expression = "Upper([UUID])";
            this.UUIDCampoCalculado.Name = "UUIDCampoCalculado";
            // 
            // fxRetencionISR
            // 
            this.fxRetencionISR.DataMember = "Impuestos.Impuestos_Retenciones.Retenciones_Retencion";
            this.fxRetencionISR.Expression = "Iif([][[Impuesto] == \'001\'],Concat([][[Impuesto]==\'001\'].Max([Impuesto]),\' ISR Re" +
    "tencion\'), \'\')";
            this.fxRetencionISR.Name = "fxRetencionISR";
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // fxRetencionIVA
            // 
            this.fxRetencionIVA.DataMember = "Impuestos.Impuestos_Retenciones.Retenciones_Retencion";
            this.fxRetencionIVA.Expression = "Iif([][[Impuesto]==\'002\'],Concat([Impuesto], \' \', \'IVA Retencion\' ), \'\')";
            this.fxRetencionIVA.Name = "fxRetencionIVA";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 32F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4,
            this.xrTable8});
            this.PageHeader.HeightF = 121F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.PrintOn = DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader;
            // 
            // SubTotalCampoCalculado
            // 
            this.SubTotalCampoCalculado.DataMember = "Comprobante";
            this.SubTotalCampoCalculado.Expression = "[SubTotal]";
            this.SubTotalCampoCalculado.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.SubTotalCampoCalculado.Name = "SubTotalCampoCalculado";
            // 
            // xrTable4
            // 
            this.xrTable4.BackColor = System.Drawing.Color.Black;
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.ForeColor = System.Drawing.Color.White;
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(1F, 70F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow18,
            this.xrTableRow20});
            this.xrTable4.SizeF = new System.Drawing.SizeF(697F, 49.99999F);
            this.xrTable4.StylePriority.UseBackColor = false;
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UseForeColor = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow18
            // 
            this.xrTableRow18.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6,
            this.xrTableCell8,
            this.xrTableCell10,
            this.xrTableCell12,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell16});
            this.xrTableRow18.Name = "xrTableRow18";
            this.xrTableRow18.Weight = 1D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseBorderColor = false;
            this.xrTableCell6.Text = "Clave P/S";
            this.xrTableCell6.Weight = 0.55396557863747709D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "No. De Identificación";
            this.xrTableCell8.Weight = 1.0532182148568661D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "Cve. Unidad";
            this.xrTableCell10.Weight = 0.64971235953178175D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "Unidad";
            this.xrTableCell12.Weight = 0.5608043992554359D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "Cantidad";
            this.xrTableCell14.Weight = 0.5676441221287607D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Text = "P. Unitario";
            this.xrTableCell15.Weight = 0.64287361951320543D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Text = "Importe";
            this.xrTableCell16.Weight = 0.73178109572491046D;
            // 
            // xrTableRow20
            // 
            this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17});
            this.xrTableRow20.Name = "xrTableRow20";
            this.xrTableRow20.Weight = 1D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 100F);
            this.xrTableCell17.StylePriority.UsePadding = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "Descripción";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell17.Weight = 4.7599993896484376D;
            // 
            // rptGenericaGratuita33
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.ReportFooter,
            this.PageHeader,
            this.DetailReport});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.CampoCalculadoTotal,
            this.fxImpTrasladoIVA,
            this.fxImpTrasladoIEPS,
            this.fxImpRetencionISR,
            this.fxImpRetencionIVA,
            this.fxTrasladoIVA,
            this.fxTrasladoIEPS,
            this.fxRetencionISR,
            this.fxRetencionIVA,
            this.SubTotalCampoCalculado,
            this.ImporteCampoCalculado,
            this.PUnitarioCampoCalc,
            this.UUIDCampoCalculado});
            this.CrossBandControls.AddRange(new DevExpress.XtraReports.UI.XRCrossBandControl[] {
            this.xrCrossBandLine4,
            this.xrCrossBandLine3,
            this.xrCrossBandLine2,
            this.xrCrossBandLine1});
            this.Margins = new System.Drawing.Printing.Margins(100, 51, 32, 30);
            this.SnapGridSize = 1F;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.StyleSheetPath = "";
            this.Version = "15.2";
            this.Watermark.Font = new System.Drawing.Font("Helvetica-Normal", 40F);
            this.Watermark.Image = ((System.Drawing.Image)(resources.GetObject("rptGenericaGratuita33.Watermark.Image")));
            this.Watermark.ImageTransparency = 150;
            this.Watermark.TextTransparency = 150;
            this.XmlDataPath = "C:\\Users\\gabriel.reyes\\Documents\\XML_0DC11AA5-7071-46AF-B6AB-260FA0056329.xml";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TableEncabezadoConceptos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private XRTableRow xrTableRow12;
        private XRTableCell CellFormaPago;
        private XRTableCell CellFecha;
        private XRTableCell xrTableCell24;
        private BottomMarginBand BottomMargin;
        private XRTableCell CellIvaRetencion;
        private XRTableCell CellProveedorCertificacion;
        private XRTableRow xrTableRow3;
        private XRTableCell xrTableCell5;
        private XRTableCell xrTableCell4;
        private CalculatedField fxTrasladoIVA;
        private XRTableCell xrTableCell42;
        private CalculatedField CampoCalculadoTotal;
        private DetailBand Detail2;
        private XRTable xrTable9;
        private XRTableRow xrTableRow40;
        private XRTableCell xrTableCell31;
        private XRTableCell xrTableCell32;
        private XRTableCell xrTableCell34;
        private XRTableCell xrTableCell35;
        private XRTableCell xrTableCell41;
        private XRTableCell xrTableCell43;
        private XRTableCell xrTableCell44;
        private XRTableRow xrTableRow41;
        private XRTableCell xrTableCell45;
        private XRCrossBandLine xrCrossBandLine2;
        private ReportFooterBand ReportFooter;
        private XRTable xrTable7;
        private XRTableRow xrTableRow33;
        private XRTableCell CellQR;
        private XRBarCode xrBarCodeQRGratuita;
        private XRTable xrTable6;
        private XRTableRow xrTableRow25;
        private XRTableCell xrTableCell21;
        private XRTableRow xrTableRow26;
        private XRTableCell CellSelloDigitalEmisor;
        private XRTableRow xrTableRow27;
        private XRTableCell xrTableCell27;
        private XRTableRow xrTableRow28;
        private XRTableCell xrTableCell30;
        private XRTableRow xrTableRow29;
        private XRTableCell xrTableCell33;
        private XRTableRow xrTableRow30;
        private XRTableCell CellCadenaOriginal;
        private XRTableRow xrTableRow31;
        private XRTableCell xrTableCell22;
        private XRTableCell CellCertificadoEmisor;
        private XRTableCell xrTableCell39;
        private XRTableRow xrTableRow32;
        private XRTableCell CellCertificadoSAT;
        private XRTableCell xrTableCell23;
        private XRTable xrTable5;
        private XRTableRow xrTableRow21;
        private XRTableCell CellTotalLetra;
        private XRTableCell CellSubtotal;
        private XRTableCell CellImpSubtotal;
        private XRTableRow xrTableRow22;
        private XRTableCell CellTotalconLetra;
        private XRTableCell CellIvaTraslado;
        private XRTableCell CellImpIvaTraslado;
        private XRTableRow xrTableRow35;
        private XRTableCell CelllblTipoCambio;
        private XRTableCell CellTipoCambio;
        private XRTableCell CellIepsTraslado;
        private XRTableCell CellImpIepsTraslado;
        private XRTableRow xrTableRow34;
        private XRTableCell CellIsrRetencion;
        private XRTableCell CellImpIsrRetencion;
        private XRTableRow xrTableRow23;
        private XRTableCell CellVacio;
        private XRTableCell CellImpIvaRetencion;
        private XRTableRow xrTableRow37;
        private XRTableCell xrTableCell26;
        private XRTableCell xrTableCell28;
        private XRTableCell xrTableCell29;
        private XRTableRow xrTableRow36;
        private XRTableCell xrTableCell19;
        private XRTableCell xrTableCell20;
        private XRTableCell xrTableCell25;
        private XRTableRow xrTableRow24;
        private XRTableCell xrTableCell18;
        private XRTableCell CellTotal;
        private XRTableCell CellImpTotal;
        private XRTableCell CellUUID;
        private XRTableRow xrTableRow8;
        private XRTableCell CellMetodoPago;
        private XRTableCell CellSerieFolio;
        private XRTableCell CellCveUnidad;
        private XRTableCell CellTituloImporte;
        private XRTableCell xrTableCell11;
        private CalculatedField fxImpRetencionISR;
        private ReportHeaderBand ReportHeader;
        private XRTable xrTable3;
        private XRTableRow xrTableRow13;
        private XRTableCell xrTableCell13;
        private XRTableRow xrTableRow14;
        private XRTableCell CellRazonReceptor;
        private XRTableRow xrTableRow15;
        private XRTableCell CellRFCReceptor;
        private XRTableRow xrTableRow16;
        private XRTableCell CellUsoCFDI;
        private XRTable xrTable2;
        private XRTableRow xrTableRow4;
        private XRTableRow xrTableRow7;
        private XRTableCell xrTableCell1;
        private XRTableCell xrTableCell7;
        private XRTableRow xrTableRow9;
        private XRTableCell xrTableCell2;
        private XRTableCell xrTableCell9;
        private XRTableRow xrTableRow10;
        private XRTableCell CellCondicionesPago;
        private XRTableCell CellLugarExpedicion;
        private XRTableRow xrTableRow11;
        private XRTableCell xrTableCell3;
        private XRTable xrTable1;
        private XRTableRow xrTableRow2;
        private XRTableRow xrTableRow1;
        private XRTableCell CellRazonEmisor;
        private XRTableRow xrTableRow6;
        private XRTableCell CellRFCEmisor;
        private XRTableRow xrTableRow5;
        private XRTableCell CellRegimenEmisor;
        private XRTableCell CellUnidad;
        private XRTableRow xrTableRow17;
        private XRTableCell CellTituloClave;
        private XRTableCell CellNoIdentificacion;
        private XRTableCell CellCantidad;
        private XRTableCell CellPUnitario;
        private XRTableCell xrCellSerieFolio;
        private XRTableCell CellDescripcion;
        private XRTableRow xrTableRow39;
        private XRTableCell xrTableCell36;
        private XRTableCell xrTableCell37;
        private XRTableCell xrTableCell38;
        private XRTableCell xrTableCell40;
        private XRTable xrTable8;
        private XRTableRow xrTableRow38;
        private XRTableCell xrCellEmisor;
        private XRTableCell xrCellReceptor;
        private XRTableCell xrCellUUID;
        private XRLabel xrLabel8;
        private XRPageInfo PageInfoPaginas;
        private XRPictureBox xrPictureBox1;
        private XRCrossBandLine xrCrossBandLine3;
        private XRTable TableEncabezadoConceptos;
        private XRTableRow xrTableRow19;
        private DetailReportBand DetailReport1;
        private CalculatedField ImporteCampoCalculado;
        private CalculatedField fxImpRetencionIVA;
        private CalculatedField fxImpTrasladoIVA;
        private CalculatedField PUnitarioCampoCalc;
        private GroupHeaderBand GroupHeader1;
        private DetailReportBand DetailReport;
        private DetailBand Detail1;
        private PageFooterBand PageFooter;
        private XRLabel LabelLink;
        private XRCrossBandLine xrCrossBandLine1;
        private CalculatedField fxTrasladoIEPS;
        private CalculatedField fxImpTrasladoIEPS;
        private XRCrossBandLine xrCrossBandLine4;
        private CalculatedField UUIDCampoCalculado;
        private CalculatedField fxRetencionISR;
        private DetailBand Detail;
        private CalculatedField fxRetencionIVA;
        private TopMarginBand TopMargin;
        private PageHeaderBand PageHeader;
        private CalculatedField SubTotalCampoCalculado;
        private XRTable xrTable4;
        private XRTableRow xrTableRow18;
        private XRTableCell xrTableCell6;
        private XRTableCell xrTableCell8;
        private XRTableCell xrTableCell10;
        private XRTableCell xrTableCell12;
        private XRTableCell xrTableCell14;
        private XRTableCell xrTableCell15;
        private XRTableCell xrTableCell16;
        private XRTableRow xrTableRow20;
        private XRTableCell xrTableCell17;

    }
}

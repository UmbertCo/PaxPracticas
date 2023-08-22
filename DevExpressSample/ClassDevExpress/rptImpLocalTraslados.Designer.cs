namespace ClassDevExpress
{
    partial class rptImpLocalTraslados
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.pColor = new DevExpress.XtraReports.Parameters.Parameter();
            this.fxImporteLocalesTras = new DevExpress.XtraReports.UI.CalculatedField();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetImpLocalesTras = new DevExpress.XtraReports.UI.DetailBand();
            this.tblImpLocalTra = new DevExpress.XtraReports.UI.XRTable();
            this.RowImpLocalTra = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellImpLocalTraTit = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellImpLocalTraValue = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxDescripLocalesTras = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.tblImpLocalTra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // pColor
            // 
            this.pColor.Description = "Color Bordes";
            this.pColor.Name = "pColor";
            this.pColor.ValueInfo = "Red";
            this.pColor.Visible = false;
            // 
            // fxImporteLocalesTras
            // 
            this.fxImporteLocalesTras.DataMember = "ImpuestosLocales.ImpuestosLocales_TrasladosLocales";
            this.fxImporteLocalesTras.Expression = "[Importe]";
            this.fxImporteLocalesTras.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteLocalesTras.Name = "fxImporteLocalesTras";
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetImpLocalesTras});
            this.DetailReport.DataMember = "ImpuestosLocales.ImpuestosLocales_TrasladosLocales";
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            this.DetailReport.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DetailReport_BeforePrint);
            // 
            // DetImpLocalesTras
            // 
            this.DetImpLocalesTras.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tblImpLocalTra});
            this.DetImpLocalesTras.HeightF = 14F;
            this.DetImpLocalesTras.KeepTogether = true;
            this.DetImpLocalesTras.KeepTogetherWithDetailReports = true;
            this.DetImpLocalesTras.Name = "DetImpLocalesTras";
            // 
            // tblImpLocalTra
            // 
            this.tblImpLocalTra.BorderColor = System.Drawing.Color.Green;
            this.tblImpLocalTra.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tblImpLocalTra.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.tblImpLocalTra.Name = "tblImpLocalTra";
            this.tblImpLocalTra.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.RowImpLocalTra});
            this.tblImpLocalTra.SizeF = new System.Drawing.SizeF(243F, 14F);
            this.tblImpLocalTra.StylePriority.UseBorderColor = false;
            this.tblImpLocalTra.StylePriority.UseBorders = false;
            // 
            // RowImpLocalTra
            // 
            this.RowImpLocalTra.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellImpLocalTraTit,
            this.CellImpLocalTraValue});
            this.RowImpLocalTra.Name = "RowImpLocalTra";
            this.RowImpLocalTra.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.RowImpLocalTra.StylePriority.UsePadding = false;
            this.RowImpLocalTra.Weight = 0.56D;
            // 
            // CellImpLocalTraTit
            // 
            this.CellImpLocalTraTit.BorderColor = System.Drawing.Color.Green;
            this.CellImpLocalTraTit.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.CellImpLocalTraTit.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ImpuestosLocales.ImpuestosLocales_TrasladosLocales.fxDescripLocalesTras")});
            this.CellImpLocalTraTit.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellImpLocalTraTit.Name = "CellImpLocalTraTit";
            this.CellImpLocalTraTit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.CellImpLocalTraTit.StylePriority.UseBorderColor = false;
            this.CellImpLocalTraTit.StylePriority.UseBorders = false;
            this.CellImpLocalTraTit.StylePriority.UseFont = false;
            this.CellImpLocalTraTit.StylePriority.UsePadding = false;
            this.CellImpLocalTraTit.StylePriority.UseTextAlignment = false;
            this.CellImpLocalTraTit.Tag = "Color";
            this.CellImpLocalTraTit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellImpLocalTraTit.Weight = 1.3D;
            // 
            // CellImpLocalTraValue
            // 
            this.CellImpLocalTraValue.BorderColor = System.Drawing.Color.Green;
            this.CellImpLocalTraValue.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellImpLocalTraValue.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ImpuestosLocales.ImpuestosLocales_TrasladosLocales.fxImporteLocalesTras")});
            this.CellImpLocalTraValue.Name = "CellImpLocalTraValue";
            this.CellImpLocalTraValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.CellImpLocalTraValue.StylePriority.UseBorderColor = false;
            this.CellImpLocalTraValue.StylePriority.UseBorders = false;
            this.CellImpLocalTraValue.StylePriority.UsePadding = false;
            this.CellImpLocalTraValue.StylePriority.UseTextAlignment = false;
            this.CellImpLocalTraValue.Tag = "Color";
            this.CellImpLocalTraValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.CellImpLocalTraValue.Weight = 1.12D;
            // 
            // fxDescripLocalesTras
            // 
            this.fxDescripLocalesTras.DataMember = "ImpuestosLocales.ImpuestosLocales_TrasladosLocales";
            this.fxDescripLocalesTras.Expression = "Iif(IsNullOrEmpty([ImpLocTrasladado]), \' \', Concat([ImpLocTrasladado],\' \',[Tasade" +
                "Traslado],\' \',\'%\' ))";
            this.fxDescripLocalesTras.Name = "fxDescripLocalesTras";
            // 
            // rptImpLocalTraslados
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReport});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.fxImporteLocalesTras,
            this.fxDescripLocalesTras});
            this.Font = new System.Drawing.Font("Arial Unicode MS", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageWidth = 243;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.pColor});
            this.SnapGridSize = 1F;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.StyleSheetPath = "";
            this.Version = "15.2";
            this.XmlDataPath = "C:\\DevExpressSample\\DevExpressWebSample\\XML\\Impuestos2.xml";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.rptImpLocalTraslados_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.tblImpLocalTra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.Parameters.Parameter pColor;
        private DevExpress.XtraReports.UI.CalculatedField fxImporteLocalesTras;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.DetailBand DetImpLocalesTras;
        private DevExpress.XtraReports.UI.XRTable tblImpLocalTra;
        private DevExpress.XtraReports.UI.XRTableRow RowImpLocalTra;
        private DevExpress.XtraReports.UI.XRTableCell CellImpLocalTraTit;
        private DevExpress.XtraReports.UI.XRTableCell CellImpLocalTraValue;
        private DevExpress.XtraReports.UI.CalculatedField fxDescripLocalesTras;

    }
}

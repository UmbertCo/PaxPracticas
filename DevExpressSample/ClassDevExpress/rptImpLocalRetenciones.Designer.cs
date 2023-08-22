namespace ClassDevExpress
{
    partial class rptImpLocalRetenciones
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.pColor = new DevExpress.XtraReports.Parameters.Parameter();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.CellImpLocalRetTit = new DevExpress.XtraReports.UI.XRTableCell();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetImpLocalesRet = new DevExpress.XtraReports.UI.DetailBand();
            this.tblImpLocalRet = new DevExpress.XtraReports.UI.XRTable();
            this.RowImpLocalRet = new DevExpress.XtraReports.UI.XRTableRow();
            this.CellImpLocalRetValue = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxDescripLocalesRet = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImporteLocalesRet = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.tblImpLocalRet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
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
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // CellImpLocalRetTit
            // 
            this.CellImpLocalRetTit.BorderColor = System.Drawing.Color.Green;
            this.CellImpLocalRetTit.Borders = DevExpress.XtraPrinting.BorderSide.Left;
            this.CellImpLocalRetTit.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ImpuestosLocales.ImpuestosLocales_RetencionesLocales.fxDescripLocalesRet")});
            this.CellImpLocalRetTit.Name = "CellImpLocalRetTit";
            this.CellImpLocalRetTit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.CellImpLocalRetTit.StylePriority.UseBorderColor = false;
            this.CellImpLocalRetTit.StylePriority.UseBorders = false;
            this.CellImpLocalRetTit.StylePriority.UsePadding = false;
            this.CellImpLocalRetTit.StylePriority.UseTextAlignment = false;
            this.CellImpLocalRetTit.Tag = "Color";
            this.CellImpLocalRetTit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.CellImpLocalRetTit.Weight = 1.3D;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetImpLocalesRet});
            this.DetailReport.DataMember = "ImpuestosLocales.ImpuestosLocales_RetencionesLocales";
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            this.DetailReport.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.DetailReport_BeforePrint);
            // 
            // DetImpLocalesRet
            // 
            this.DetImpLocalesRet.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tblImpLocalRet});
            this.DetImpLocalesRet.HeightF = 14F;
            this.DetImpLocalesRet.KeepTogether = true;
            this.DetImpLocalesRet.KeepTogetherWithDetailReports = true;
            this.DetImpLocalesRet.Name = "DetImpLocalesRet";
            // 
            // tblImpLocalRet
            // 
            this.tblImpLocalRet.BorderColor = System.Drawing.Color.Green;
            this.tblImpLocalRet.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tblImpLocalRet.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.tblImpLocalRet.Name = "tblImpLocalRet";
            this.tblImpLocalRet.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.RowImpLocalRet});
            this.tblImpLocalRet.SizeF = new System.Drawing.SizeF(243F, 14F);
            this.tblImpLocalRet.StylePriority.UseBorderColor = false;
            this.tblImpLocalRet.StylePriority.UseBorders = false;
            // 
            // RowImpLocalRet
            // 
            this.RowImpLocalRet.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CellImpLocalRetTit,
            this.CellImpLocalRetValue});
            this.RowImpLocalRet.Name = "RowImpLocalRet";
            this.RowImpLocalRet.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.RowImpLocalRet.StylePriority.UsePadding = false;
            this.RowImpLocalRet.Weight = 0.56D;
            // 
            // CellImpLocalRetValue
            // 
            this.CellImpLocalRetValue.BorderColor = System.Drawing.Color.Green;
            this.CellImpLocalRetValue.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellImpLocalRetValue.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ImpuestosLocales.ImpuestosLocales_RetencionesLocales.fxImporteLocalesRet")});
            this.CellImpLocalRetValue.Font = new System.Drawing.Font("Arial Unicode MS", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellImpLocalRetValue.Name = "CellImpLocalRetValue";
            this.CellImpLocalRetValue.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.CellImpLocalRetValue.StylePriority.UseBorderColor = false;
            this.CellImpLocalRetValue.StylePriority.UseBorders = false;
            this.CellImpLocalRetValue.StylePriority.UseFont = false;
            this.CellImpLocalRetValue.StylePriority.UsePadding = false;
            this.CellImpLocalRetValue.StylePriority.UseTextAlignment = false;
            this.CellImpLocalRetValue.Tag = "Color";
            this.CellImpLocalRetValue.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.CellImpLocalRetValue.Weight = 1.12D;
            // 
            // fxDescripLocalesRet
            // 
            this.fxDescripLocalesRet.DataMember = "ImpuestosLocales.ImpuestosLocales_RetencionesLocales";
            this.fxDescripLocalesRet.Expression = "Iif(IsNullOrEmpty([ImpLocRetenido]), \' \',Concat([ImpLocRetenido],\' \',[TasadeReten" +
                "cion],\' \',\'%\' ))";
            this.fxDescripLocalesRet.Name = "fxDescripLocalesRet";
            // 
            // fxImporteLocalesRet
            // 
            this.fxImporteLocalesRet.DataMember = "ImpuestosLocales.ImpuestosLocales_RetencionesLocales";
            this.fxImporteLocalesRet.Expression = "[Importe]";
            this.fxImporteLocalesRet.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteLocalesRet.Name = "fxImporteLocalesRet";
            // 
            // rptImpLocalRetenciones
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReport});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.fxDescripLocalesRet,
            this.fxImporteLocalesRet});
            this.Font = new System.Drawing.Font("Helvetica-Normal", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.rptImpLocalRetenciones_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.tblImpLocalRet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.Parameters.Parameter pColor;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTableCell CellImpLocalRetTit;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.DetailBand DetImpLocalesRet;
        private DevExpress.XtraReports.UI.XRTable tblImpLocalRet;
        private DevExpress.XtraReports.UI.XRTableRow RowImpLocalRet;
        private DevExpress.XtraReports.UI.XRTableCell CellImpLocalRetValue;
        private DevExpress.XtraReports.UI.CalculatedField fxDescripLocalesRet;
        private DevExpress.XtraReports.UI.CalculatedField fxImporteLocalesRet;

    }
}

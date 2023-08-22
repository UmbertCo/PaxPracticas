using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

/// <summary>
/// Summary description for rptPercepciones
/// </summary>
public class rptPercepciones : DevExpress.XtraReports.UI.XtraReport
{
    private DevExpress.XtraReports.UI.DetailBand Detail;
    private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
    private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    private DetailReportBand DetailPercepciones;
    private DetailBand DetPercepciones;
    private XRTable xrTable20;
    private XRTableRow xrTableRow58;
    private XRTableCell xrTableCell128;
    private XRTableCell xrTableCell129;
    private XRTableCell xrTableCell130;
    private XRTableCell xrTableCell133;
    private XRTableCell xrTableCell131;
    private CalculatedField fxImporteExentoPercep;
    private CalculatedField fxImporteGravadoPercep;
    private CalculatedField fxImportePagado;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public rptPercepciones()
    {
        InitializeComponent();
        //
        // TODO: Add constructor logic here
        //
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.DetailPercepciones = new DevExpress.XtraReports.UI.DetailReportBand();
            this.DetPercepciones = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable20 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow58 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell128 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell129 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell130 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell133 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell131 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fxImporteExentoPercep = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImporteGravadoPercep = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImportePagado = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // DetailPercepciones
            // 
            this.DetailPercepciones.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetPercepciones});
            this.DetailPercepciones.DataMember = "Percepciones.Percepciones_Percepcion";
            this.DetailPercepciones.Level = 0;
            this.DetailPercepciones.Name = "DetailPercepciones";
            // 
            // DetPercepciones
            // 
            this.DetPercepciones.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable20});
            this.DetPercepciones.HeightF = 15F;
            this.DetPercepciones.Name = "DetPercepciones";
            // 
            // xrTable20
            // 
            this.xrTable20.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable20.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable20.Name = "xrTable20";
            this.xrTable20.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable20.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow58});
            this.xrTable20.SizeF = new System.Drawing.SizeF(394F, 15F);
            this.xrTable20.StylePriority.UseFont = false;
            this.xrTable20.StylePriority.UsePadding = false;
            // 
            // xrTableRow58
            // 
            this.xrTableRow58.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell128,
            this.xrTableCell129,
            this.xrTableCell130,
            this.xrTableCell133,
            this.xrTableCell131});
            this.xrTableRow58.Name = "xrTableRow58";
            this.xrTableRow58.Weight = 11.499971850768331D;
            // 
            // xrTableCell128
            // 
            this.xrTableCell128.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell128.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell128.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Percepciones.Percepciones_Percepcion.TipoPercepcion")});
            this.xrTableCell128.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell128.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell128.Name = "xrTableCell128";
            this.xrTableCell128.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell128.StylePriority.UseBackColor = false;
            this.xrTableCell128.StylePriority.UseBorders = false;
            this.xrTableCell128.StylePriority.UseFont = false;
            this.xrTableCell128.StylePriority.UseForeColor = false;
            this.xrTableCell128.StylePriority.UsePadding = false;
            this.xrTableCell128.StylePriority.UseTextAlignment = false;
            this.xrTableCell128.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell128.Weight = 0.56770491033894133D;
            // 
            // xrTableCell129
            // 
            this.xrTableCell129.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell129.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell129.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Percepciones.Percepciones_Percepcion.Clave")});
            this.xrTableCell129.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell129.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell129.Name = "xrTableCell129";
            this.xrTableCell129.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell129.StylePriority.UseBackColor = false;
            this.xrTableCell129.StylePriority.UseBorders = false;
            this.xrTableCell129.StylePriority.UseFont = false;
            this.xrTableCell129.StylePriority.UseForeColor = false;
            this.xrTableCell129.StylePriority.UsePadding = false;
            this.xrTableCell129.StylePriority.UseTextAlignment = false;
            this.xrTableCell129.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell129.Weight = 0.97574321048495927D;
            // 
            // xrTableCell130
            // 
            this.xrTableCell130.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell130.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell130.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Percepciones.Percepciones_Percepcion.Concepto")});
            this.xrTableCell130.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell130.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell130.Name = "xrTableCell130";
            this.xrTableCell130.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell130.StylePriority.UseBackColor = false;
            this.xrTableCell130.StylePriority.UseBorders = false;
            this.xrTableCell130.StylePriority.UseFont = false;
            this.xrTableCell130.StylePriority.UseForeColor = false;
            this.xrTableCell130.StylePriority.UsePadding = false;
            this.xrTableCell130.StylePriority.UseTextAlignment = false;
            this.xrTableCell130.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell130.Weight = 2.6965984850177938D;
            // 
            // xrTableCell133
            // 
            this.xrTableCell133.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell133.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell133.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Percepciones.Percepciones_Percepcion.fxImporteGravadoPercep", "{0:$ #,###0.00}")});
            this.xrTableCell133.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell133.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell133.Name = "xrTableCell133";
            this.xrTableCell133.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell133.StylePriority.UseBackColor = false;
            this.xrTableCell133.StylePriority.UseBorders = false;
            this.xrTableCell133.StylePriority.UseFont = false;
            this.xrTableCell133.StylePriority.UseForeColor = false;
            this.xrTableCell133.StylePriority.UsePadding = false;
            this.xrTableCell133.StylePriority.UseTextAlignment = false;
            this.xrTableCell133.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell133.Weight = 1.3749103270281129D;
            // 
            // xrTableCell131
            // 
            this.xrTableCell131.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell131.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell131.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Percepciones.Percepciones_Percepcion.fxImporteExentoPercep", "{0:$ #,###0.00}")});
            this.xrTableCell131.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell131.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell131.Name = "xrTableCell131";
            this.xrTableCell131.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell131.StylePriority.UseBackColor = false;
            this.xrTableCell131.StylePriority.UseBorders = false;
            this.xrTableCell131.StylePriority.UseFont = false;
            this.xrTableCell131.StylePriority.UseForeColor = false;
            this.xrTableCell131.StylePriority.UsePadding = false;
            this.xrTableCell131.StylePriority.UseTextAlignment = false;
            this.xrTableCell131.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell131.Weight = 1.3749104623795132D;
            // 
            // fxImporteExentoPercep
            // 
            this.fxImporteExentoPercep.DataMember = "Percepciones.Percepciones_Percepcion";
            this.fxImporteExentoPercep.Expression = "[ImporteExento]";
            this.fxImporteExentoPercep.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteExentoPercep.Name = "fxImporteExentoPercep";
            // 
            // fxImporteGravadoPercep
            // 
            this.fxImporteGravadoPercep.DataMember = "Percepciones.Percepciones_Percepcion";
            this.fxImporteGravadoPercep.Expression = "[ImporteGravado]";
            this.fxImporteGravadoPercep.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteGravadoPercep.Name = "fxImporteGravadoPercep";
            // 
            // fxImportePagado
            // 
            this.fxImportePagado.DataMember = "HorasExtra";
            this.fxImportePagado.Expression = "[HorasExtra]";
            this.fxImportePagado.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImportePagado.Name = "fxImportePagado";
            // 
            // rptPercepciones
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailPercepciones});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.fxImporteExentoPercep,
            this.fxImporteGravadoPercep,
            this.fxImportePagado});
            this.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageWidth = 394;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.SnapGridSize = 1F;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.Version = "15.2";
            this.XmlDataPath = "D:\\ProjectsCrypto\\PAXPlantillaNomina12CFDI33\\PAXPlantillaNomina12CFDI33\\XML\\NOMIN" +
    "A12CDF33.xml";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion
}

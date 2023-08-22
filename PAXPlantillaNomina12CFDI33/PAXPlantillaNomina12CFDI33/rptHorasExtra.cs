using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for rptHorasExtra
/// </summary>
public class rptHorasExtra : DevExpress.XtraReports.UI.XtraReport
{
    private BottomMarginBand BottomMargin;
    private DetailBand DetHorasExtra;
    private XRTable xrTable24;
    private XRTableRow xrTableRow66;
    private XRTableCell xrTableCell141;
    private XRTableCell xrTableCell142;
    private XRTableCell CellTipoHorasExtra;
    private XRTableCell xrTableCell144;
    private DetailBand Detail;
    private DetailReportBand DetailHorasExtra;
    private CalculatedField fxImporteExentoPercep;
    private TopMarginBand TopMargin;
    private CalculatedField fxImporteGravadoPercep;
    private CalculatedField fxImportePagado;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private string sConexion = "conConsultas";

    public rptHorasExtra()
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
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.DetHorasExtra = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable24 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow66 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell141 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell142 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellTipoHorasExtra = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell144 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.DetailHorasExtra = new DevExpress.XtraReports.UI.DetailReportBand();
            this.fxImporteExentoPercep = new DevExpress.XtraReports.UI.CalculatedField();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.fxImporteGravadoPercep = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImportePagado = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // DetHorasExtra
            // 
            this.DetHorasExtra.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable24});
            this.DetHorasExtra.HeightF = 15F;
            this.DetHorasExtra.Name = "DetHorasExtra";
            // 
            // xrTable24
            // 
            this.xrTable24.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable24.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable24.Name = "xrTable24";
            this.xrTable24.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 1, 1, 1, 100F);
            this.xrTable24.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow66});
            this.xrTable24.SizeF = new System.Drawing.SizeF(394F, 15F);
            this.xrTable24.StylePriority.UseFont = false;
            this.xrTable24.StylePriority.UsePadding = false;
            // 
            // xrTableRow66
            // 
            this.xrTableRow66.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell141,
            this.xrTableCell142,
            this.CellTipoHorasExtra,
            this.xrTableCell144});
            this.xrTableRow66.Name = "xrTableRow66";
            this.xrTableRow66.Weight = 11.499971850768331D;
            // 
            // xrTableCell141
            // 
            this.xrTableCell141.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell141.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell141.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "HorasExtra.Dias")});
            this.xrTableCell141.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell141.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell141.Name = "xrTableCell141";
            this.xrTableCell141.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell141.StylePriority.UseBackColor = false;
            this.xrTableCell141.StylePriority.UseBorders = false;
            this.xrTableCell141.StylePriority.UseFont = false;
            this.xrTableCell141.StylePriority.UseForeColor = false;
            this.xrTableCell141.StylePriority.UsePadding = false;
            this.xrTableCell141.StylePriority.UseTextAlignment = false;
            this.xrTableCell141.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell141.Weight = 0.5663810810540848D;
            // 
            // xrTableCell142
            // 
            this.xrTableCell142.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell142.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell142.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "HorasExtra.HorasExtra")});
            this.xrTableCell142.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell142.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell142.Name = "xrTableCell142";
            this.xrTableCell142.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.xrTableCell142.StylePriority.UseBackColor = false;
            this.xrTableCell142.StylePriority.UseBorders = false;
            this.xrTableCell142.StylePriority.UseFont = false;
            this.xrTableCell142.StylePriority.UseForeColor = false;
            this.xrTableCell142.StylePriority.UsePadding = false;
            this.xrTableCell142.StylePriority.UseTextAlignment = false;
            this.xrTableCell142.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell142.Weight = 0.56638109082567489D;
            // 
            // CellTipoHorasExtra
            // 
            this.CellTipoHorasExtra.BackColor = System.Drawing.Color.Transparent;
            this.CellTipoHorasExtra.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellTipoHorasExtra.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "HorasExtra.TipoHoras")});
            this.CellTipoHorasExtra.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellTipoHorasExtra.ForeColor = System.Drawing.Color.Black;
            this.CellTipoHorasExtra.Name = "CellTipoHorasExtra";
            this.CellTipoHorasExtra.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellTipoHorasExtra.StylePriority.UseBackColor = false;
            this.CellTipoHorasExtra.StylePriority.UseBorders = false;
            this.CellTipoHorasExtra.StylePriority.UseFont = false;
            this.CellTipoHorasExtra.StylePriority.UseForeColor = false;
            this.CellTipoHorasExtra.StylePriority.UsePadding = false;
            this.CellTipoHorasExtra.StylePriority.UseTextAlignment = false;
            this.CellTipoHorasExtra.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.CellTipoHorasExtra.Weight = 0.56638112106075234D;
            this.CellTipoHorasExtra.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellTipoHorasExtra_BeforePrint);
            // 
            // xrTableCell144
            // 
            this.xrTableCell144.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell144.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell144.CanGrow = false;
            this.xrTableCell144.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "HorasExtra.fxImportePagado", "{0:$ #,###0.00}")});
            this.xrTableCell144.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell144.ForeColor = System.Drawing.Color.Black;
            this.xrTableCell144.Name = "xrTableCell144";
            this.xrTableCell144.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTableCell144.StylePriority.UseBackColor = false;
            this.xrTableCell144.StylePriority.UseBorders = false;
            this.xrTableCell144.StylePriority.UseFont = false;
            this.xrTableCell144.StylePriority.UseForeColor = false;
            this.xrTableCell144.StylePriority.UsePadding = false;
            this.xrTableCell144.StylePriority.UseTextAlignment = false;
            this.xrTableCell144.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell144.Weight = 0.56638106028359481D;
            // 
            // Detail
            // 
            this.Detail.Expanded = false;
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // DetailHorasExtra
            // 
            this.DetailHorasExtra.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetHorasExtra});
            this.DetailHorasExtra.DataMember = "HorasExtra";
            this.DetailHorasExtra.Level = 0;
            this.DetailHorasExtra.Name = "DetailHorasExtra";
            this.DetailHorasExtra.ReportPrintOptions.DetailCountOnEmptyDataSource = 0;
            this.DetailHorasExtra.ReportPrintOptions.PrintOnEmptyDataSource = false;
            // 
            // fxImporteExentoPercep
            // 
            this.fxImporteExentoPercep.DataMember = "Percepciones.Percepciones_Percepcion";
            this.fxImporteExentoPercep.Expression = "[ImporteExento]";
            this.fxImporteExentoPercep.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteExentoPercep.Name = "fxImporteExentoPercep";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            // rptHorasExtra
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailHorasExtra});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.fxImporteExentoPercep,
            this.fxImporteGravadoPercep,
            this.fxImportePagado});
            this.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageWidth = 394;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ReportPrintOptions.DetailCountOnEmptyDataSource = 0;
            this.ReportPrintOptions.PrintOnEmptyDataSource = false;
            this.SnapGridSize = 1F;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.StyleSheetPath = "";
            this.Version = "15.2";
            this.XmlDataPath = "D:\\ProjectsCrypto\\PAXPlantillaNomina12CFDI33\\PAXPlantillaNomina12CFDI33\\XML\\NOMIN" +
    "A12CDF33.xml";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

    }

    #endregion

    private void CellTipoHorasExtra_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRTableCell CellTipoHorasExtra = (XRTableCell)sender;
        if (!string.IsNullOrEmpty(CellTipoHorasExtra.Text))
        {
            DataTable dtTipoHorasExtra = fnComparaTipoHoraExtra(CellTipoHorasExtra.Text);
            (sender as XRTableCell).Text = (dtTipoHorasExtra.Rows[0]["Descripcion"] != DBNull.Value ? dtTipoHorasExtra.Rows[0]["Descripcion"].ToString() : "");
        }
    }

    private DataTable fnComparaTipoHoraExtra(string psClave)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[sConexion].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Tipo_HorasExtras_sel_n12";
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
}

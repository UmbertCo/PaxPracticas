using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for rptIncapacidades
/// </summary>
public class rptIncapacidades : DevExpress.XtraReports.UI.XtraReport
{
    private CalculatedField fxImporteMonetario;
    private DetailBand Detail;
    private XRTable xrTable24;
    private XRTableRow xrTableRow66;
    private XRTableCell xrTableCell141;
    private XRTableCell CellTipoIncapacidad;
    private XRTableCell xrTableCell144;
    private DetailBand DetIncapacidades;
    private DetailReportBand DetailIncapacidades;
    private CalculatedField fxImporteDeduccion;
    private CalculatedField fxImportePagado;
    private CalculatedField fxImporteExentoPercep;
    private BottomMarginBand BottomMargin;
    private TopMarginBand TopMargin;
    private CalculatedField fxImporteGravadoPercep;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private string sConexion = "conConsultas";

    public rptIncapacidades()
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
            this.fxImporteMonetario = new DevExpress.XtraReports.UI.CalculatedField();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable24 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow66 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell141 = new DevExpress.XtraReports.UI.XRTableCell();
            this.CellTipoIncapacidad = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell144 = new DevExpress.XtraReports.UI.XRTableCell();
            this.DetIncapacidades = new DevExpress.XtraReports.UI.DetailBand();
            this.DetailIncapacidades = new DevExpress.XtraReports.UI.DetailReportBand();
            this.fxImporteDeduccion = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImportePagado = new DevExpress.XtraReports.UI.CalculatedField();
            this.fxImporteExentoPercep = new DevExpress.XtraReports.UI.CalculatedField();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.fxImporteGravadoPercep = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // fxImporteMonetario
            // 
            this.fxImporteMonetario.DataMember = "Incapacidades.Incapacidades_Incapacidad";
            this.fxImporteMonetario.Expression = "[ImporteMonetario]";
            this.fxImporteMonetario.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteMonetario.Name = "fxImporteMonetario";
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            this.CellTipoIncapacidad,
            this.xrTableCell144});
            this.xrTableRow66.Name = "xrTableRow66";
            this.xrTableRow66.Weight = 11.499971850768331D;
            // 
            // xrTableCell141
            // 
            this.xrTableCell141.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell141.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell141.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Incapacidad.DiasIncapacidad")});
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
            this.xrTableCell141.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell141.Weight = 0.5663810810540848D;
            // 
            // CellTipoIncapacidad
            // 
            this.CellTipoIncapacidad.BackColor = System.Drawing.Color.Transparent;
            this.CellTipoIncapacidad.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.CellTipoIncapacidad.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Incapacidades.Incapacidades_Incapacidad.TipoIncapacidad")});
            this.CellTipoIncapacidad.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellTipoIncapacidad.ForeColor = System.Drawing.Color.Black;
            this.CellTipoIncapacidad.Name = "CellTipoIncapacidad";
            this.CellTipoIncapacidad.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 1, 1, 100F);
            this.CellTipoIncapacidad.StylePriority.UseBackColor = false;
            this.CellTipoIncapacidad.StylePriority.UseBorders = false;
            this.CellTipoIncapacidad.StylePriority.UseFont = false;
            this.CellTipoIncapacidad.StylePriority.UseForeColor = false;
            this.CellTipoIncapacidad.StylePriority.UsePadding = false;
            this.CellTipoIncapacidad.StylePriority.UseTextAlignment = false;
            this.CellTipoIncapacidad.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CellTipoIncapacidad.Weight = 1.1327622118864271D;
            this.CellTipoIncapacidad.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.CellTipoIncapacidad_BeforePrint);
            // 
            // xrTableCell144
            // 
            this.xrTableCell144.BackColor = System.Drawing.Color.Transparent;
            this.xrTableCell144.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell144.CanGrow = false;
            this.xrTableCell144.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Incapacidades.Incapacidades_Incapacidad.fxImporteMonetario", "{0:$ #,###0.00}")});
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
            this.xrTableCell144.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell144.Weight = 0.56638106028359481D;
            // 
            // DetIncapacidades
            // 
            this.DetIncapacidades.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable24});
            this.DetIncapacidades.HeightF = 15F;
            this.DetIncapacidades.Name = "DetIncapacidades";
            this.DetIncapacidades.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // DetailIncapacidades
            // 
            this.DetailIncapacidades.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.DetIncapacidades});
            this.DetailIncapacidades.DataMember = "Incapacidades.Incapacidades_Incapacidad";
            this.DetailIncapacidades.Level = 0;
            this.DetailIncapacidades.Name = "DetailIncapacidades";
            this.DetailIncapacidades.ReportPrintOptions.PrintOnEmptyDataSource = false;
            // 
            // fxImporteDeduccion
            // 
            this.fxImporteDeduccion.DataMember = "Deducciones.Deducciones_Deduccion";
            this.fxImporteDeduccion.Expression = "[Importe]";
            this.fxImporteDeduccion.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteDeduccion.Name = "fxImporteDeduccion";
            // 
            // fxImportePagado
            // 
            this.fxImportePagado.DataMember = "HorasExtra";
            this.fxImportePagado.Expression = "[HorasExtra]";
            this.fxImportePagado.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImportePagado.Name = "fxImportePagado";
            // 
            // fxImporteExentoPercep
            // 
            this.fxImporteExentoPercep.DataMember = "Percepciones.Percepciones_Percepcion";
            this.fxImporteExentoPercep.Expression = "[ImporteExento]";
            this.fxImporteExentoPercep.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteExentoPercep.Name = "fxImporteExentoPercep";
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
            // fxImporteGravadoPercep
            // 
            this.fxImporteGravadoPercep.DataMember = "Percepciones.Percepciones_Percepcion";
            this.fxImporteGravadoPercep.Expression = "[ImporteGravado]";
            this.fxImporteGravadoPercep.FieldType = DevExpress.XtraReports.UI.FieldType.Double;
            this.fxImporteGravadoPercep.Name = "fxImporteGravadoPercep";
            // 
            // rptIncapacidades
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailIncapacidades});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.fxImporteExentoPercep,
            this.fxImporteGravadoPercep,
            this.fxImportePagado,
            this.fxImporteDeduccion,
            this.fxImporteMonetario});
            this.Font = new System.Drawing.Font("Helvetica-Normal", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageWidth = 394;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
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

    private void CellTipoIncapacidad_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
    {
        XRTableCell CellTipoIncapacidad = (XRTableCell)sender;
        if (!string.IsNullOrEmpty(CellTipoIncapacidad.Text))
        {
            DataTable dtTipoIncapacidad = fnComparaTipoIncapacidad(CellTipoIncapacidad.Text);
            (sender as XRTableCell).Text = (dtTipoIncapacidad.Rows[0]["Descripcion"] != DBNull.Value ? dtTipoIncapacidad.Rows[0]["Descripcion"].ToString() : "");
        }
    }

    private DataTable fnComparaTipoIncapacidad(string psClave)
    {
        DataTable dtResultado = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(PAXCrypto.CryptoAES.DesencriptaAES64(ConfigurationManager.ConnectionStrings[sConexion].ConnectionString)))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "usp_nom_Tipos_Incapacidades_sel_Existe_n12";
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
            clsErrorLog.fnNuevaEntrada(ex, clsErrorLog.TipoErroresLog.Conexion, "fnComparaTipoContrato", "rptNomina12");
        }
        return dtResultado;
    }
}

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolucionPruebas.Presentacion.WindowsForms
{
    public partial class frmExcel : Form
    {
        DateTime dtAyer;
        DateTime dtHoy;
        //Fecha del dia anterior
        string sDia1;
        string sMes1;
        string sAnio1;

        //Fecha actual 
        string sDia2;
        string sMes2;
        string sAnio2;

        DataTable dtTimbrado;

        private string sCadenaConexion = "wrHCucS0xITEtcOKxIHDvcOfxLLvv4EO77+wSO+/kAdB77+077+vEO+/oSTvv7Tvv7bvv7/vv4Pvv4IA7769Oe+/vwAe77+O776477+277+qUCQdM++/q+++ne+/sO+/nVsRIEHvv6bvvrrvv73vv51f77++FTbvv7Hvv6wg77+dIgAZRO+/su+/piDvv7AHAxk177+077+vFu+/sGDvv5Dvv71A77+l77+s77+q77+QWSUZDe+/lO+/sBLvv64H77+577+4D++/g++/oiDvv51ZIiM+77+r77+s77+o77+MSCMnSe+/ru+/rxHvvrkt77+kF0bvv7Tvv68O77+0GO+/oCI=";

        public frmExcel()
        {
            InitializeComponent();
        }

        private void btnGenerarExcel_Click(object sender, EventArgs e)
        {
            dtTimbrado = new DataTable();

            using (SqlConnection scConexion = new SqlConnection())
            {
                scConexion.ConnectionString = PAXCrypto.Base64.DesencriptarBase64(sCadenaConexion);
                scConexion.Open();
                try
                {
                    using (SqlCommand scoComando = new SqlCommand())
                    {
                        scoComando.Connection = scConexion;
                        scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                        scoComando.CommandText = "usp_reporte_consumo";

                        scoComando.Parameters.AddWithValue("dia", sDia1);
                        scoComando.Parameters.AddWithValue("mes", sMes1);
                        scoComando.Parameters.AddWithValue("anio", sAnio1);
                        scoComando.Parameters.AddWithValue("dia2", sDia2);
                        scoComando.Parameters.AddWithValue("@mes2", sMes2);
                        scoComando.Parameters.AddWithValue("@anio2", sAnio2);

                        using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                        {
                            sdaAdaptador.Fill(dtTimbrado);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //clsLog.EscribirLog("Error al obtener los datos para el reporte de timbrado -" + e.Message);
                }
            }

            //clsLog.EscribirLog("Se obtuvieron " + dtTimbrado.Rows.Count + " registros para el reporte de consumo.");

            MemoryStream msReporteConsumo = new MemoryStream();
            try
            {
                using (SpreadsheetDocument excel = SpreadsheetDocument.Create(msReporteConsumo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, true))
                {
                    WorkbookPart vbpParte = excel.AddWorkbookPart();

                    // Se agrega la hoja del reporte
                    Workbook wbReporte = new Workbook();
                    Sheets sHojas = new Sheets();
                    Sheet sHoja = new Sheet { Name = "HojaConsumo", SheetId = 1, Id = "rId1" };
                    sHojas.Append(sHoja);
                    wbReporte.Append(sHojas);
                    vbpParte.Workbook = wbReporte;

                    // Se agregan los datos
                    WorksheetPart wspParte = vbpParte.AddNewPart<WorksheetPart>("rId1");
                    Worksheet wsReporte = new Worksheet();
                    SheetData sdDatos = new SheetData();

                    string sRenglonTitulo = "1";
                    Row rRenglonTitulos = new Row { RowIndex = UInt32.Parse(sRenglonTitulo) };

                    Cell cTituloCarpeta = new Cell { CellReference = "A" + sRenglonTitulo, CellValue = new CellValue("Carpeta"), DataType = CellValues.String };
                    rRenglonTitulos.Append(cTituloCarpeta);

                    Cell cTituloFecha = new Cell { CellReference = "B" + sRenglonTitulo, CellValue = new CellValue("Fecha"), DataType = CellValues.Date };
                    rRenglonTitulos.Append(cTituloFecha);

                    Cell cTituloTotal = new Cell { CellReference = "C" + sRenglonTitulo, CellValue = new CellValue("Total"), DataType = CellValues.Number };
                    rRenglonTitulos.Append(cTituloTotal);

                    sdDatos.Append(rRenglonTitulos);


                    for (int i = 0; i < dtTimbrado.Rows.Count; i++)
                    {
                        string sNumeroRenglon = (i + 2).ToString();
                        Row rRenglon = new Row { RowIndex = UInt32.Parse(sNumeroRenglon) };

                        Cell cCarpeta = new Cell { CellReference = "A" + sNumeroRenglon, CellValue = new CellValue(dtTimbrado.Rows[i]["origen"].ToString()), DataType = CellValues.String };
                        rRenglon.Append(cCarpeta);

                        Cell cFecha = new Cell { CellReference = "B" + sNumeroRenglon, CellValue = new CellValue(dtTimbrado.Rows[i]["fecha"].ToString()), DataType = CellValues.Date };
                        rRenglon.Append(cFecha);

                        Cell cTotal = new Cell { CellReference = "C" + sNumeroRenglon, CellValue = new CellValue(dtTimbrado.Rows[i]["total"].ToString()), DataType = CellValues.Number };
                        rRenglon.Append(cTotal);

                        sdDatos.Append(rRenglon);
                    }

                    wsReporte.Append(sdDatos);
                    wspParte.Worksheet = wsReporte;
                }

                File.WriteAllBytes(@"C:\" + "ReporteConsumo_" + sDia1 + sMes1 + sAnio1 + ".xlsx", msReporteConsumo.ToArray());
                //clsLog.EscribirLog("Reporte de Consumo generado correctamente: " + "ReporteConsumo_" + sDia1 + sMes1 + sAnio1 + ".xls");
            }
            catch (Exception ex)
            {
                //clsLog.EscribirLog("Error al momento de crear el Reporte de consumo -" + e.Message);
            }        
        }

        private void frmExcel_Load(object sender, EventArgs e)
        {
            dtAyer = DateTime.Today.AddDays(-1);
            sDia1 = dtAyer.Day.ToString();
            sMes1 = dtAyer.Month.ToString();
            sAnio1 = dtAyer.Year.ToString();

            if (sMes1.Length == 1)
            {
                sMes1 = "0" + sMes1;
            }

            dtHoy = DateTime.Today;
            sDia2 = dtHoy.Day.ToString();
            sMes2 = dtHoy.Month.ToString();
            sAnio2 = dtHoy.Year.ToString();

            if (sMes2.Length == 1)
            {
                sMes2 = "0" + sMes2;
            }
        }

        private void btnImportar_Click(object sender, EventArgs e)
        {
            DataSet dsTimbrado = new DataSet();

            using (SqlConnection scConexion = new SqlConnection())
            {
                scConexion.ConnectionString = PAXCrypto.Base64.DesencriptarBase64(sCadenaConexion);
                scConexion.Open();
                try
                {
                    using (SqlCommand scoComando = new SqlCommand())
                    {
                        scoComando.Connection = scConexion;
                        scoComando.CommandType = System.Data.CommandType.StoredProcedure;
                        scoComando.CommandText = "usp_reporte_consumo";

                        scoComando.Parameters.AddWithValue("dia", sDia1);
                        scoComando.Parameters.AddWithValue("mes", sMes1);
                        scoComando.Parameters.AddWithValue("anio", sAnio1);
                        scoComando.Parameters.AddWithValue("dia2", sDia2);
                        scoComando.Parameters.AddWithValue("@mes2", sMes2);
                        scoComando.Parameters.AddWithValue("@anio2", sAnio2);

                        using (SqlDataAdapter sdaAdaptador = new SqlDataAdapter(scoComando))
                        {
                            sdaAdaptador.Fill(dsTimbrado);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //clsLog.EscribirLog("Error al obtener los datos para el reporte de timbrado -" + e.Message);
                }
            }

            try
            {
                ExportDSToExcel(dsTimbrado, @"C:\ReporteConsumo_" + sDia1 + sMes1 + sAnio1 + ".xlsx");
            }
            catch (Exception ex)
            {
                //clsLog.EscribirLog("Error al momento de crear el Reporte de consumo -" + e.Message);
            }

        }

        private void ExportDSToExcel(DataSet dsTablas, string destination)
        {
            using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                uint sheetId = 1;

                foreach (DataTable table in dsTablas.Tables)
                {
                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(headerRow);

                    foreach (DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }
                }
            }
        }
    }
}

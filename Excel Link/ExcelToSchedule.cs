using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.FormulaParsing.Excel;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using AW = Autodesk.Windows;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]//EVERY COMMAND REQUIRES THIS!
    public class Excel
    {
        public static string getExcelFile()
        {
            OpenFileDialog excelFileDialog = new OpenFileDialog();
            excelFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm|All files (*.*)|*.*";
            excelFileDialog.Title = "Select Excel File";
            string excelFile = string.Empty;
            if (excelFileDialog.ShowDialog() == DialogResult.OK)
            {
                excelFile = excelFileDialog.FileName;
            }
            else
            {
                TaskDialog.Show("Error", "No file selected.");
                return null;
            }
            return excelFile;
        }

        public static void saveLinkInfo(string file) {
            Document doc = linkUI.doc;
            doc.GetCloudModelPath();

        }

        public static void updateLinkInfo(string file)
        {

        }

        public static bool xlsxToSchedule(string xlsx, string name) {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //create schedule
            ViewSchedule schedule = createBlankSchedule(name);

            //get table info
            TableSectionData tableData = schedule.GetTableData().GetSectionData(SectionType.Header);

            //get xlsx data 
            ExcelPackage excel = new ExcelPackage(new FileInfo(xlsx));
            ExcelWorksheet worksheet = excel.Workbook.Worksheets[0];
            
            //get range to link
            ExcelNamedRange range = null;
            foreach (ExcelNamedRange r in worksheet.Names) {
                if ((r.Name).Contains("Print_Area")) {
                    range = r; 
                    break;
                }
            }
            if (range == null) {
                TaskDialog.Show("Error", "No named range found in the worksheet.");
                return false;
            }
            int nCol = range.Columns;
            int nRow = range.Rows;

            //set size
            schedule.GetTableData().GetSectionData(SectionType.Body).SetColumnWidthInPixels(0, nCol * 100);

            //Make cells and populate data
            for (int i = 1; i < nCol; i++)
            {
                tableData.InsertColumn(i);
            }
            for (int i = 1; i < nRow; i++)
            {
                tableData.InsertRow(i);
            }

            //Merge cells
            foreach (String r in worksheet.MergedCells)
            {
                ExcelRange excelRange = worksheet.Cells[r];
                TableMergedCell tableMergedCell = new TableMergedCell();
                tableMergedCell.Left = excelRange.Start.Column - range.Start.Column;
                tableMergedCell.Top = excelRange.Start.Row - range.Start.Row;
                tableMergedCell.Right = excelRange.End.Column - range.Start.Column;
                tableMergedCell.Bottom = excelRange.End.Row - range.Start.Row;
                tableData.MergeCells(tableMergedCell);
            }

            //Go into each cell (format and add text)
            for (int i = 0; i < nCol; i++) {
                // Set column width
                int width = (int)worksheet.Column(i + range.Start.Column).Width;
                tableData.SetColumnWidthInPixels(i, width);
                for (int j = 0; j < nRow; j++) {
                    if (i == 0)
                    {
                        //Set row height
                        int height = (int)worksheet.Row(j + range.Start.Row).Height;
                        tableData.SetColumnWidthInPixels(i, height);
                    }

                    string text = range.GetCellValue<String>(j, i);
                    
                    //Style cell
                    ExcelStyle Style = worksheet.Cells[j + 1, i + 1].Style;
                    TableCellStyle newStyle = new TableCellStyle();
                    newStyle.TextSize = Style.Font.Size/ 0.013834867007874;
                    newStyle.FontName = Style.Font.Name;
                    tableData.SetCellStyle(newStyle);

                    if (!string.IsNullOrEmpty(text)) {
                        tableData.SetCellText(j, i, text);
                    }
                }
            }
            return true;
        }

        private static ViewSchedule createBlankSchedule(string name) {
            Document doc = linkUI.doc;
            ViewSchedule schedule = ViewSchedule.CreateKeySchedule(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            //schedule.Name = name;
            ScheduleDefinition def = schedule.Definition;

            List<string> field = new List<string>();
            def.ShowTitle = true;
            def.ShowHeaders = false;
            return schedule;
        }
    }
}

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
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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

        public static bool xlsxToSchedule(string xlsx, string name, Transaction t) {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //create schedule
            ViewSchedule schedule = createBlankSchedule(name);

            //get table info
            TableSectionData tableData = schedule.GetTableData().GetSectionData(SectionType.Header);

            Debug.Write(tableData.GetColumnWidthInPixels(0));
            Debug.Write(schedule.GetTableData().GetSectionData(SectionType.Body).GetColumnWidthInPixels(0));

            //get xlsx data 
            ExcelPackage excel = new ExcelPackage(new FileInfo(xlsx));
            ExcelWorksheet worksheet = excel.Workbook.Worksheets[0];

            //double scaleChange = 1.5;
            
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

            Double totalWidth = 0;
            Double totalHeight = 0;
            for (int i = range.Start.Column; i < nCol + range.Start.Column; i++)
            {
                totalWidth += worksheet.Column(i).Width;
            }
            for (int i = range.Start.Row; i < nRow + range.Start.Row; i++)
            {
                totalHeight += worksheet.Row(i).Height;
            }

            //set size
            schedule.GetTableData().GetSectionData(SectionType.Body).SetColumnWidthInPixels(0, (int)(totalWidth));
            Debug.WriteLine(schedule.GetTableData().GetSectionData(SectionType.Body).GetColumnWidthInPixels(0));
            return true;
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

            //add text
            for (int i = 0; i < nCol; i++) {
                for (int j = 0; j < nRow; j++) {
                    

                    string text = range.GetCellValue<String>(j, i);
                    if (!string.IsNullOrEmpty(text))
                    {
                        tableData.SetCellText(j, i, text);
                    }
                }
            }

            //Need to reload between text and formating
            t.Commit();
            t.Start();

            //format
            for (int i = 0; i < nCol; i++)
            {
                // Set column width
                int width = (int)worksheet.Column(i + range.Start.Column).Width;
                tableData.SetColumnWidthInPixels(i, width);
                for (int j = 0; j < nRow; j++)
                {
                    //Style cell
                    ExcelStyle Style = worksheet.Cells[j + 1, i + 1].Style;
                    TableCellStyle newStyle = new TableCellStyle();
                    TableCellStyleOverrideOptions options = new TableCellStyleOverrideOptions();
                    options.SetAllOverrides(true);
                    newStyle.ResetOverride();
                    newStyle.SetCellStyleOverrideOptions(options);

                    //font
                    newStyle.TextSize = Style.Font.Size;
                    newStyle.FontName = Style.Font.Name;
                    newStyle.IsFontBold = Style.Font.Bold;

                    //border
                    //copyBorder(newStyle, Style.Border);
                    
                    //newPat.;
                    //LinePatternElement.Create(doc,null);
                    tableData.SetCellStyle(j ,i ,newStyle);
                    if (i == 0)
                    {
                        //Set row height
                        int height = (int)worksheet.Row(j + range.Start.Row).Height + 1;
                        tableData.SetRowHeightInPixels(j, height);
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

        private static void copyBorder(TableCellStyle scdB, Border excB) {
            scdB.BorderTopLineStyle = getRevitBoarderIDFromExcel(excB.Top);
            scdB.BorderBottomLineStyle = getRevitBoarderIDFromExcel(excB.Bottom);
            scdB.BorderRightLineStyle = getRevitBoarderIDFromExcel(excB.Right);
            scdB.BorderLeftLineStyle = getRevitBoarderIDFromExcel(excB.Left);
        }

        private static ElementId getRevitBoarderIDFromExcel(ExcelBorderItem border) {
            Document doc = linkUI.doc;
            FilteredElementCollector fec = new FilteredElementCollector(doc)
                .OfClass(typeof(LinePatternElement));
            LinePattern newPat = new LinePattern();
            LinePatternSegment newseg = new LinePatternSegment();
            return null;
        }

        private static ElementId createLineType(ExcelBorderItem border) {
            string RGBstring = border.Color.Rgb;
            if (RGBstring == "") {
                RGBstring = "#000000";
            }
            System.Drawing.Color excColor = ColorTranslator.FromHtml(RGBstring);
            Byte r = Convert.ToByte(excColor.R);
            Byte g = Convert.ToByte(excColor.G);
            Byte b = Convert.ToByte(excColor.B);
            Autodesk.Revit.DB.Color color = new Autodesk.Revit.DB.Color(r, g, b);

            Document doc = linkUI.doc;
            Settings settings = doc.Settings;
            Categories cats = settings.Categories;
            Category lineCat = cats.get_Item(BuiltInCategory.OST_Lines);

            Category lineStyleCat = cats.NewSubcategory(lineCat, "MyLineStyle");
            lineStyleCat.LineColor = color;
            lineStyleCat.SetLineWeight(2, GraphicsStyleType.Projection);

            return lineStyleCat.Id;
        }
    }
}

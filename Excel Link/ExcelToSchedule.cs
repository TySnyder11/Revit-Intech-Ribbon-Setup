using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]//EVERY COMMAND REQUIRES THIS!
    public class Excel
    {

        public static void updateLinkInfo(string file)
        {

        }

        public static string newLink(string xlsx, string name, string worksheetName, string rangeName, string viewName)
        {
            //create schedule
            ViewSchedule schedule = createBlankSchedule(name);

            //get xlsx data 
            ExcelPackage excel = new ExcelPackage(new FileInfo(xlsx));
            ExcelWorksheets worksheets = excel.Workbook.Worksheets;
            ExcelWorksheet worksheet = null;
            foreach (ExcelWorksheet ws in worksheets)
            {
                if (ws.Name.Equals(worksheetName))
                {
                    worksheet = ws;
                    break;
                }
            }
            Document doc = linkUI.doc;
            //get all sheets from doc
            ViewSheet sheet = null;
            foreach (ViewSheet e in new FilteredElementCollector(doc).OfClass(typeof(ViewSheet))
                .WhereElementIsNotElementType().ToElements())
            {
                if ((e.SheetNumber + " - " + e.Name).Equals(viewName)) 
                {
                    sheet = e;
                }
            }

            xlsxToSchedule(worksheet, schedule, rangeName);
            XYZ origin = new XYZ(0, 0, 0);
            ScheduleSheetInstance instance = ScheduleSheetInstance.Create(linkUI.doc, sheet.Id, schedule.Id, origin);

            return schedule.Name;
        }

        public static void Update(string xlsx, string scheduleName, string worksheetName, string rangeName)
        {
            ExcelPackage excel = new ExcelPackage(new FileInfo(xlsx));
            ExcelWorksheets worksheets = excel.Workbook.Worksheets;
            ExcelWorksheet worksheet = null;
            foreach (ExcelWorksheet ws in worksheets)
            {
                if (ws.Name.Equals(worksheetName))
                {
                    worksheet = ws;
                    break;
                }
            }
            Document doc = linkUI.doc;
            ViewSchedule schedule = null;
            foreach (ViewSchedule e in new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule))
                .WhereElementIsNotElementType().ToElements())
            {
                if (e.Name.Equals(scheduleName)) 
                {
                    //get schedule
                    schedule = e;
                }
            }
            schedule.GetTableData().GetSectionData(SectionType.Header).Dispose();
            
            xlsxToSchedule(worksheet, schedule, rangeName);
        }
        public static bool xlsxToSchedule(ExcelWorksheet worksheet, ViewSchedule schedule, string rangeName) {

            //get table info
            TableSectionData tableData = schedule.GetTableData().GetSectionData(SectionType.Header);

            //get range to link
            ExcelNamedRange range = null;
            foreach (ExcelNamedRange r in worksheet.Names) {
                if ((r.Name).Contains(rangeName)) {
                    range = r; 
                    break;
                }
            }
            if (range == null) {
                TaskDialog.Show("Error", "No range named " + rangeName + " found in the worksheet.");
                return false;
            }
            int nCol = range.Columns;
            int nRow = range.Rows;
            List<int> hidden = new List<int>();

            Double totalWidth = 0;
            for (int i = range.Start.Column; i < nCol + range.Start.Column; i++)
            {
                if (worksheet.Column(i).Hidden) 
                {
                    hidden.Add(i);
                    continue;
                }
                totalWidth += worksheet.Column(i).Width * 7.5 + 1;
            }
            nCol -= hidden.Count;

            //set size
            schedule.GetTableData().GetSectionData(SectionType.Body).SetColumnWidthInPixels(0, (int)(totalWidth));

            while (tableData.NumberOfColumns > nCol)
            {
                tableData.RemoveColumn(1);
            }
            while (tableData.NumberOfRows > nRow)
            {
                tableData.RemoveRow(1);
            }

            //Make cells and populate data
            for (int i = 1; tableData.NumberOfColumns < nCol; i++)
            {
                tableData.InsertColumn(i);
            }
            for (int i = 1; tableData.NumberOfRows < nRow; i++)
            {
                tableData.InsertRow(i);
            }

            //Merge cells
            foreach (String r in worksheet.MergedCells)
            {
                //Need to take account of hidden cells during indexing
                ExcelRange excelRange = worksheet.Cells[r];
                int startCol = excelRange.Start.Column;
                int endCol = excelRange.End.Column;
                int hBefore = 0;
                int hInside = 0;
                bool cont = true;
                foreach (int i in hidden)
                {
                    if (i == startCol && i == endCol)
                    {
                        cont = false;
                    }
                    if (i <= startCol)
                    {
                        hBefore++;
                        continue;
                    }
                    if (startCol < i && i <= endCol)
                    {
                        hInside++;
                        continue;
                    }
                }
                if (!cont)
                {
                    continue;
                }

                //Merge
                TableMergedCell tableMergedCell = new TableMergedCell();
                tableMergedCell.Left = startCol - range.Start.Column - hBefore;
                tableMergedCell.Top = excelRange.Start.Row - range.Start.Row;
                tableMergedCell.Right = endCol - range.Start.Column - hBefore - hInside;
                tableMergedCell.Bottom = excelRange.End.Row - range.Start.Row;
                TableMergedCell check = tableData.GetMergedCell(tableMergedCell.Top, tableMergedCell.Left);
                if (0 <= tableMergedCell.Left && tableMergedCell.Left <= nCol
                    && 0 <= tableMergedCell.Top && tableMergedCell.Top <= nRow 
                    && !check.Equals(tableMergedCell))
                {
                    tableData.MergeCells(tableMergedCell);
                }
            }

            //add text
            int colIndex = 0;
            for (int i = 0; i < nCol; i++) {
                if (worksheet.Column(colIndex + range.Start.Column).Hidden) 
                {
                    i--;
                    colIndex++;
                    continue;
                }
                for (int j = 0; j < nRow; j++) {
                    string text = range.GetCellValue<String>(j, colIndex);
                    if (!string.IsNullOrEmpty(text))
                    {
                        tableData.SetCellText(j, i, text);
                    }

                    //Style cell
                    ExcelStyle style = worksheet.Cells[j + range.Start.Row, colIndex + range.Start.Column].Style;
                    TableCellStyle newStyle = new TableCellStyle();
                    TableCellStyleOverrideOptions options = new TableCellStyleOverrideOptions();
                    options.SetAllOverrides(true);
                    newStyle.ResetOverride();
                    newStyle.SetCellStyleOverrideOptions(options);

                    //font
                    string fontName = style.Font.Name;
                    newStyle.FontName = fontName;
                    newStyle.TextSize = revitFontWrongScaleFix(fontName, style.Font.Size);
                    newStyle.IsFontBold = style.Font.Bold;
                    newStyle.IsFontItalic = style.Font.Italic;
                    newStyle.IsFontUnderline = style.Font.UnderLine;
                    newStyle.TextColor = ExcelColorToRevit(style.Font.Color, "#FF000000");

                    //background
                    newStyle.BackgroundColor = ExcelColorToRevit(style.Fill.BackgroundColor, "#FFFFFFFF");

                    //Text alignment
                    HorizontalAlignmentStyle x = HorizontalAlignmentStyle.Left; //base case center
                    switch ((int)style.HorizontalAlignment) 
                    {
                        case 1: x = HorizontalAlignmentStyle.Left; break; // left
                        case 2: x = HorizontalAlignmentStyle.Center; break; // center
                        case 4: x = HorizontalAlignmentStyle.Right; break; // right
                        default: x = HorizontalAlignmentStyle.Left; break;
                    }
                    newStyle.FontHorizontalAlignment = x;

                    //works due to there similar numbering for vert alignment
                    newStyle.FontVerticalAlignment = (VerticalAlignmentStyle)((int)style.VerticalAlignment * 4); 

                    //border
                    copyBorder(newStyle, worksheet,j + range.Start.Row, colIndex + range.Start.Column);

                    //check for notes cell and fix
                    if (newStyle.FontHorizontalAlignment == HorizontalAlignmentStyle.Left && !string.IsNullOrEmpty(tableData.GetCellText(j, i)))
                    {
                        Font font = new Font(style.Font.Name, (int)(style.Font.Size));

                        using (Bitmap bitmap = new Bitmap(1, 1))
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            int textSize = System.Windows.Forms.TextRenderer.MeasureText(text, font).Width + 10;
                            int cellwidth = tableData.GetColumnWidthInPixels(i);
                            int p = i;
                            int copyColumnIndex = colIndex;
                            while (textSize > cellwidth && !worksheet.Cells[j + range.Start.Row, colIndex + range.Start.Column].Merge)
                            {
                                if (worksheet.Column(copyColumnIndex + range.Start.Column).Hidden)
                                {
                                    copyColumnIndex++;
                                    continue;
                                }

                                p++;
                                copyColumnIndex++;
                                if (p >= nCol)
                                {
                                    break;
                                }
                                cellwidth += (int)(worksheet.Column(copyColumnIndex + range.Start.Column).Width * 7.5);
                                
                            }
                            if (i != p)
                            {
                                TableMergedCell tableMergedCell = new TableMergedCell(j, i, j, p);
                                tableData.MergeCells(tableMergedCell);
                            }
                        }
                    }
 
                    tableData.SetCellStyle(j, i, newStyle);
                    if (i == 0)
                    {
                        //Set row height
                        int height = (int)(worksheet.Row(j + range.Start.Row).Height * 4 / 3) + 1;
                        tableData.SetRowHeightInPixels(j, height);
                    }
                }
                int width = (int)(worksheet.Column(colIndex + range.Start.Column).Width * 7.5 + 1);
                tableData.SetColumnWidthInPixels(i, width);
                colIndex++;
            }
                    return true;
        }

        private static ViewSchedule createBlankSchedule(string name) {
            Document doc = linkUI.doc;
            ViewSchedule schedule = ViewSchedule.CreateKeySchedule(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            //rename existing schedules to avoid name conflicts
            name = renameHelper(name, new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule)).WhereElementIsNotElementType().ToElements());
            schedule.Name = name;
            schedule.LookupParameter("View Template").Set("");
            ScheduleDefinition def = schedule.Definition;

            List<string> field = new List<string>();
            def.ShowTitle = true;
            def.ShowHeaders = false;


            List<Element> text = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).WhereElementIsElementType().ToElements() as List<Element>;
            ElementId textId = null;
            foreach (Element e in text)
            {
                if (e.Name.Equals("Schedule Text 1"))
                {
                    textId = e.Id;
                }
            }
            if (textId != null)
            {
                schedule.TitleTextTypeId = textId;
            }
            return schedule;
        }

        private static string renameHelper(string name, IList<Element> schedules)
        {
            foreach (Element view in schedules)
            {
                if (view.Name.Equals(name))
                {
                    name = view.Name + " - Copy";
                    name = renameHelper(name, schedules);
                }
            }
            return name;
        }

        private static void copyBorder(TableCellStyle scdB, ExcelWorksheet worksheet, int row, int column) {
            Border excB = worksheet.Cells[row, column].Style.Border;
            ElementId top = getRevitBoarderIDFromExcel(excB.Top);
            int tRow = row;
            if (tRow >= 2 && top == null)
            {
                top = getRevitBoarderIDFromExcel(worksheet.Cells[tRow - 1, column].Style.Border.Bottom);
            }
            if (top != null)
            {
                scdB.BorderTopLineStyle = top;
            }
            ElementId bottom = getRevitBoarderIDFromExcel(excB.Bottom);
            int bRow = row;
            if (bRow <= worksheet.Rows.EndRow && bottom == null)
            {
                bottom = getRevitBoarderIDFromExcel(worksheet.Cells[bRow + 1, column].Style.Border.Top);
            }
            if (bottom != null)
            {
                scdB.BorderBottomLineStyle = bottom;
            }
            ElementId right = getRevitBoarderIDFromExcel(excB.Right);
            int rColumn = column + 1;
            if (right == null)
            {
                while (
                    rColumn <= worksheet.Columns.EndColumn
                    && (worksheet.Column(rColumn).Hidden
                    ||  mergeCheck(worksheet, row, column, row, rColumn)))
                {
                    rColumn ++;
                }
                if (rColumn != column + 1)
                {
                    right = getRevitBoarderIDFromExcel(worksheet.Cells[row, rColumn - 1].Style.Border.Right);
                }
                if (right == null)
                {
                    right = getRevitBoarderIDFromExcel(worksheet.Cells[row, rColumn].Style.Border.Left);
                }
            }
            if (right != null)
            {
                scdB.BorderRightLineStyle = right;
            }
            ElementId left = getRevitBoarderIDFromExcel(excB.Left);
            int lColumn = column - 1;
            if (left == null)
            {
                while (
                    lColumn >= 2 
                    && (worksheet.Column(lColumn).Hidden 
                    || mergeCheck(worksheet, row, lColumn, row, column)))
                {
                    lColumn--;
                }
                if (lColumn >= 1)
                {
                    left = getRevitBoarderIDFromExcel(worksheet.Cells[row, lColumn].Style.Border.Right);
                }
            }
            if (left != null)
            {
                scdB.BorderLeftLineStyle = left;
            }
        }

        private static ElementId getRevitBoarderIDFromExcel(ExcelBorderItem border) {
            if (border == null || border.Color.Rgb == null) 
            {
                return null;
            }

            Document doc = linkUI.doc;
            Categories categories = doc.Settings.Categories;
            Category lineCat = categories.get_Item(BuiltInCategory.OST_Lines);
            CategoryNameMap graphicStyleCategories = lineCat.SubCategories;

            //get color
            Autodesk.Revit.DB.Color color = ExcelColorToRevit(border.Color, "#FF000000");
            int r = color.Red;
            int g = color.Green;
            int b = color.Blue;

            //get weight
            int weight = 0;
            switch ((int)border.Style)
            {
                case 1: weight = 1; break;  //super thin
                case 4: weight = 4; break;  //thin
                case 11: weight = 6; break; //medium 
                case 10: weight = 7; break; //thick
                
                default: weight = 4; break; //default (in case line type not recognized)
            }

            ElementId lineID = null;
            foreach (Category cal in graphicStyleCategories)
            {
                if (cal.Name == "RGB " + r + " ," + g + " ," + b + ", Weight " + weight) 
                {
                    lineID = cal.Id;
                }
            }

            //Create if not in project
            if (lineID == null)
            {
                lineID = createLineType(weight, color);
            }


            return lineID;
        }

        private static ElementId createLineType(int weight, Autodesk.Revit.DB.Color color) {
            int r = color.Red;
            int g = color.Green;
            int b = color.Blue;
            Document doc = linkUI.doc;
            Categories cats = doc.Settings.Categories;
            Category lineCat = cats.get_Item(BuiltInCategory.OST_Lines);
            CategoryNameMap graphicStyleCategory = lineCat.SubCategories;
            Category lineStyleCat = cats.NewSubcategory(lineCat, "RGB " + r + " ," + g + " ," + b + ", Weight " + weight);
            lineStyleCat.LineColor = color;
            lineStyleCat.SetLineWeight(weight, GraphicsStyleType.Projection);

            return lineStyleCat.Id;
        }

        private static Autodesk.Revit.DB.Color ExcelColorToRevit(ExcelColor ExcColor, string defaultColorHex)
        {
            string RGBhex = ExcColor.LookupColor();
            if (ExcColor.Indexed == 0)
            {
                RGBhex = defaultColorHex;
            }
            int argb = Int32.Parse(RGBhex.Replace("#", ""), NumberStyles.HexNumber);
            System.Drawing.Color excColor = System.Drawing.Color.FromArgb(argb);
            Byte r = Convert.ToByte(excColor.R);
            Byte g = Convert.ToByte(excColor.G);
            Byte b = Convert.ToByte(excColor.B);
            Autodesk.Revit.DB.Color color = new Autodesk.Revit.DB.Color(r, g, b);
            return color;
        }

        private static double revitFontWrongScaleFix(string fontName, double value)
        {
            double offBy = 0;
            switch (fontName)
            {
                case "Calibri": offBy = 1.25; break; //sheet side is larger by 3/64"
                
                default: offBy = 0; break; //not off by anything //fonts like ariel
            }

            double adjusted = value - offBy;
            return adjusted;
        }

        private static bool mergeCheck(
            ExcelWorksheet worksheet, 
            int startRow, 
            int startColumn, 
            int endRow, 
            int endColumn)
        {
            if (!worksheet.Cells[startRow, startColumn, endRow, endColumn].Merge)
            {
                return false;
            }
            if (startRow == endRow && startColumn == endColumn)
            {
                return true;
            }
            foreach (string r in worksheet.MergedCells)
            {
                ExcelRange range = worksheet.Cells[r];

                if (range.Start.Row <= startRow 
                    && endRow <= range.End.Row 
                    && range.Start.Column <= startColumn
                    && endColumn <= range.End.Column)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}

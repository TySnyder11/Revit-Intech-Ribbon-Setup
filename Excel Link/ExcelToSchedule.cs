using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml;
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
            
            for (int i = 0; i < nCol; i++) {
                if (i != 0) 
                { 
                    tableData.InsertColumn(i);
                }
                tableData.SetColumnWidthInPixels(i, 100); // Set column width
                for (int j = 0; j < nRow; j++) {
                    if (j != 0 && i == 0)
                    {
                        tableData.InsertRow(j);
                    }
                    string text = range.GetCellValue<String>(j, i);
                    if (text == null) {
                        text = "";
                    }
                    tableData.SetCellText(j, i, text);
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

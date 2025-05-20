using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AW = Autodesk.Windows;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public static void xlsxToSchedule(string xlsx, string name) {
            createBlankSchedule(name);
        }

        private static ViewSchedule createBlankSchedule(string name) {
            Document doc = linkUI.doc;
            ViewSchedule schedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            ParameterSet ps = schedule.Parameters;
            schedule.Name = name;
            //schedule.GetParameters("Show Headers").FirstOrDefault().Set(0);
            return schedule;
        }
    }
}

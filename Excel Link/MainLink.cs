using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Intech.Revit;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]//EVERY COMMAND REQUIRES THIS!
    public class linkUI : IExternalCommand
    {
        public static Document doc = null;
        public static UIDocument uidoc = null;
        public static string settingsFile = Path.Combine(App.BasePath, "LinkSettings.txt");

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            uidoc = uiApp.ActiveUIDocument;
            doc = uidoc.Document;
            Transaction t = new Transaction(doc, "Excel Link");
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            Excel_Link.ExcelLinkUI excelLinkUI = new Excel_Link.ExcelLinkUI(t);
            excelLinkUI.ShowDialog();

            return Result.Succeeded;
        }

        public static string getCurrentSheetName()
        {
            ViewSheet sheet = doc.ActiveView as ViewSheet;
            if (sheet != null)
            {
                return sheet.SheetNumber + " - " + sheet.Name;
            }
            return null;
        }

        public static ViewSheet getSheetFromName(string sheetName)
        {
            ViewSheet sheet = null;
            foreach (ViewSheet e in new FilteredElementCollector(doc).OfClass(typeof(ViewSheet))
                .WhereElementIsNotElementType().ToElements())
            {
                if ((e.SheetNumber + " - " + e.Name).Equals(sheetName))
                {
                    //get schedule
                    sheet = e;
                }
            }
            return sheet;
        }

        public static Element revitElementCollect(string name, Type type)
        {
            Element elem = null;
            foreach (Element e in new FilteredElementCollector(doc).OfClass(type)
                .WhereElementIsNotElementType().ToElements())
            {
                if (e.Name.Equals(name))
                {
                    //get schedule
                    elem = e;
                }
            }
            return elem;
        }

        public static ViewSchedule getScheduleFromName(string scheduleName)
        {
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
            return schedule;
        }

        public static void setActiveViewSheet(string sheetName)
        {
            ViewSheet sheet = getSheetFromName(sheetName);
            if (sheet != null)
            {
                uidoc.ActiveView = sheet;
            }
            else
            {
                MessageBox.Show("Sheet not found: " + sheetName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static String[][] readSave()
        {
            SaveFileManager saveFile = new SaveFileManager(Path.Combine(Path.Combine(App.BasePath, "SaveFileManager"), "temp.txt"), new TxtFormat());
            List<SaveFileSection> sections = saveFile.GetSectionsByProject(doc.Title);
            foreach (SaveFileSection section in sections)
            {
                if (section.SecondaryName == "ExcelLinkData")
                {
                    // If the section already exists, we can return its data
                    return section.Rows.ToArray();
                }
            }
            // If the section does not exist, return an empty array
            return new string[0][];
        }

        public static void appendSave(string[] data)
        {
            SaveFileManager saveFile = new SaveFileManager(Path.Combine(Path.Combine(App.BasePath, "SaveFileManager") , "temp.txt"), new TxtFormat());
            List<SaveFileSection> sections = saveFile.GetSectionsByProject(doc.Title);
            foreach (SaveFileSection section in sections)
            {
                if (section.SecondaryName == "ExcelLinkData")
                {
                    // If the section already exists, we can update it
                    section.Rows.Add(data);
                    saveFile.AddOrUpdateSection(section);
                    return;
                }
            }
            // If the section does not exist, create a new one
            SaveFileSection newSection = new SaveFileSection(doc.Title, "ExcelLinkData", "Excel Link Data Section");
            newSection.Rows.Add(data);
            saveFile.AddOrUpdateSection(newSection);
        }
        public static void removeLineFromSave(int lineVal)
        {
            SaveFileManager saveFile = new SaveFileManager(Path.Combine(Path.Combine(App.BasePath, "SaveFileManager"), "temp.txt"), new TxtFormat());
            List<SaveFileSection> sections = saveFile.GetSectionsByProject(doc.Title);
            foreach (SaveFileSection section in sections)
            {
                if (section.SecondaryName == "ExcelLinkData")
                {
                    // If the section already exists, we can update it
                    section.Rows.RemoveAt(lineVal);
                    saveFile.AddOrUpdateSection(section);
                    return;
                }
            }
            SaveFileSection newSection = new SaveFileSection(doc.Title, "ExcelLinkData", "Excel Link Data Section");
            newSection.Rows.RemoveAt(lineVal);
            saveFile.AddOrUpdateSection(newSection);
        }

        public static void newLink(string path, string workSheet, string area, string viewSheet, string schedule)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(workSheet) || string.IsNullOrEmpty(area))
            {
                MessageBox.Show("All fields must be filled out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //get system time to be able to check if file is out of date in the future
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string user = Environment.UserName; // Get the current user's name

            // Append the new link to the save file
            appendSave(new string[] {path, workSheet, area, viewSheet, timeStamp, user, schedule });
        }

        public static string getSaveFile()
        {
            return Path.Combine(Path.Combine(App.BasePath, "SaveFileManager"), "temp.txt");
        }

        public static int findLineIndexFromDataRow(string[] dataRow)
        {
            SaveFileManager saveFile = new SaveFileManager(Path.Combine(Path.Combine(App.BasePath, "SaveFileManager"), "temp.txt"), new TxtFormat());
            List<SaveFileSection> sections = saveFile.GetSectionsByProject(doc.Title);

            foreach (SaveFileSection section in sections)
            {
                if (section.SecondaryName == "ExcelLinkData")
                {
                    // If the section already exists, we can search for the data row
                    for (int i = 0; i < section.Rows.Count; i++)
                    {
                        if (dataRow.SequenceEqual(section.Rows[i]))
                        {
                            return i; // Return the index of the matching row
                        }
                    }
                }
            }
            return -1;
        }
    }
}
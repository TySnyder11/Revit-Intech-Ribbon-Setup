using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using AW = Autodesk.Windows;
using System.Collections.Generic;
using System.IO;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]//EVERY COMMAND REQUIRES THIS!
    public class linkUI : IExternalCommand
    {
        public static Document doc = null;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            doc = uiApp.ActiveUIDocument.Document;
            Transaction t = new Transaction(doc, "Excel Link");
            string name = "test";

            string file = Excel.getExcelFile();
            if (file == null)
            {
                return Result.Failed;
            }

            t.Start();
            if (Excel.xlsxToSchedule(file, name) == false) {
                t.RollBack();
                return Result.Failed;
            }
            t.Commit();

            return Result.Succeeded;
        }
    }
}
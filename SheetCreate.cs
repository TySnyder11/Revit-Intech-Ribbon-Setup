using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Forms;
using TitleBlockSetup;
namespace Intech
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    //Settings
    public class SheetCreate : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Result.Succeeded;
        }

        public static void ContinueToCreate (List<Element> planViews)
        {
            
        }
    }
}

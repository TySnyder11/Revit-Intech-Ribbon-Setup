﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Diagnostics;
namespace Intech
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TitleBlockSelector : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //Revit pre setup stuff
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            Transaction trans = new Transaction(doc);
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get element ID of selected
            Selection selected = uidoc.Selection;
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
            foreach (ElementId i in selectedIds) { Debug.WriteLine(i + " Selected"); }

            //Make sure you have something selected
            if (selectedIds.Count == 0)
            {
                return Result.Failed; ;
            }

            //Define title block list
            IList<ElementId> tb = new List<ElementId>();

            //filter out all elements that is not a sheet
            var sheet = new FilteredElementCollector(doc, selectedIds).OfCategory(BuiltInCategory.OST_Sheets).WhereElementIsNotElementType().ToElementIds();
            foreach (ElementId i in sheet) { Debug.WriteLine(i + " Sheet Id"); }

            //for each sheet get the title block inside and add to title block list
            foreach (ElementId i in sheet)
            {
                var title_block = new FilteredElementCollector(doc, i).OfCategory(BuiltInCategory.OST_TitleBlocks).WhereElementIsNotElementType().ToElementIds();
                foreach (ElementId f in title_block)
                {
                    Debug.WriteLine(f + " Title Block");
                    tb.Add(f);
                }
            }
            //Debugging
            Debug.WriteLine(tb);
            foreach (ElementId i in tb) { Debug.WriteLine(i + " List of Sheet Id"); }

            //Set selection to new element id and print on debug screen to make sure selected element Id has changes
            uidoc.Selection.SetElementIds(tb);
            ICollection<ElementId> newselectedIds = uidoc.Selection.GetElementIds();
            foreach (ElementId i in newselectedIds) { Debug.WriteLine(i + " New Selected"); }
            
            //Confirm change
            trans.Start("Select Title Block");
            trans.Commit();

            return Autodesk.Revit.UI.Result.Succeeded;
        }
    }
}

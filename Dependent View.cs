using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using TitleBlockSetup;
namespace Intech
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    //Settings
    public class DependentView : IExternalCommand
    {
        public Dictionary<string, (int, int)> sizes = new Dictionary<string, (int, int)>
        {
            { " 1/4\" " + "= 1\"" + "-0' ", (53979, 48) },
            { " 1/8\" " + "= 1\"" + "-0' ", (1547, 96) },
            { " 1/16\" " + "= 1\"" + "-0' ", (1103312, 192) },
            { " 3/4\" " + "= 1\"" + "-0' ", (54016, 16) },
            { " 3/8\" " + "= 1\"" + "-0' ", (476405, 32) },
            { " 3/16\" " + "= 1\"" + "-0' ", (1103309, 64) },
            { " 3/32\" " + "= 1\"" + "-0' ", (54457, 128) },
            { " 1\" " + " = 40\"" + "-0' ", (1547, 480) }
        };

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;

            List<Element> planViews = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewPlan))
                .WhereElementIsNotElementType()
                .ToElements() as List<Element>;
            List<Element> areaList = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_VolumeOfInterest).
                WhereElementIsNotElementType().
                ToElements() as List<Element>;

            DependentViewForm dependentViewForm = new DependentViewForm(planViews, areaList, sizes.Keys.ToList());

            var result = dependentViewForm.ShowDialog();

            if (dependentViewForm.PlanViewCheckBox.CheckedItems.Count == 0
                || (dependentViewForm.AreaCheckBox.CheckedItems.Count == 0
                && dependentViewForm.checkBox1.Checked == false))
                return Result.Cancelled;

            Debug.WriteLine(dependentViewForm.PlanViewCheckBox.CheckedItems.Count);

            List<Autodesk.Revit.DB.View> selectedViewPlans = new List<Autodesk.Revit.DB.View>();

            for (int x = 0; x < dependentViewForm.PlanViewCheckBox.CheckedItems.Count; x++)
            {
                foreach (Autodesk.Revit.DB.View w in planViews)
                {
                    //if schedule name is in the CheckedItems list, add schedule to selected list.
                    if (w.Name.Equals((dependentViewForm.PlanViewCheckBox.CheckedItems[x] as DataRowView).Row[0]))
                    {
                        selectedViewPlans.Add(w);
                        break;
                    }
                }
            }

            List<Element> selectedAreas = new List<Element>();

            for (int x = 0; x < dependentViewForm.AreaCheckBox.CheckedItems.Count; x++)
            {
                foreach (Element w in areaList)
                {
                    if (w.Name.Equals(dependentViewForm.AreaCheckBox.CheckedItems[x]))
                    {
                        
                        selectedAreas.Add(w);
                        break;
                    }
                }
            }

            List<Element> createdViews = new List<Element>();

            Transaction tranDependentView = new Transaction(doc, "create dependent views");
            tranDependentView.Start();
            foreach (Autodesk.Revit.DB.View i in selectedViewPlans)
            {
                //Standard dependent
                foreach (Element x in selectedAreas)
                {
                    try
                    {
                        ElementId NewViewID = i.Duplicate(ViewDuplicateOption.AsDependent);
                        Element NewView = doc.GetElement(NewViewID);
                        NewView.Name = i.Name + "- Area " + x.Name;
                        createdViews.Add(NewView);
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error", ex.ToString());
                        continue;
                    }
                }

                //Overall
                if (dependentViewForm.checkBox1.Checked == true)
                {
                    ElementId NewViewID = i.Duplicate(ViewDuplicateOption.Duplicate);
                    Element NewView = doc.GetElement(NewViewID);
                    NewView.Name = i.Name + " - OVERALL";
                    createdViews.Add(NewView);
                    tranDependentView.Commit();
                    tranDependentView.Start();
                    Debug.WriteLine(dependentViewForm.OverallView.SelectedItem.ToString());
                    (NewView as Autodesk.Revit.DB.View).Scale = sizes[dependentViewForm.OverallView.SelectedItem.ToString()].Item2;
                }
            }
            tranDependentView.Commit();
            if(dependentViewForm.CreateSheet.Checked)
                SheetCreate.ContinueToCreate(createdViews);

            return Result.Succeeded;
        }
    }
}

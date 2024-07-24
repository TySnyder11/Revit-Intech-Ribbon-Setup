using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
namespace Intech
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SheetCreateInit : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            Transaction trans = new Transaction(doc, "Create Sheet");
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            SheetCreateForm form = new SheetCreateForm(commandData);
            form.ShowDialog();

            if  (
                form.PlanViewCheckList.CheckedItems.Count == 0||
                string.IsNullOrEmpty(form.TradeAbriviation.Text)||
                string.IsNullOrEmpty(form.MiddleSheetNumber.Text) ||
                string.IsNullOrEmpty(form.SheetName.Text) ||
                string.IsNullOrEmpty(form.TitleBlockFamily.Text) ||
                string.IsNullOrEmpty(form.TitleBlockType.Text)
                )
                return Result.Cancelled;

            List<Element> AllViews = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewPlan))
                .WhereElementIsNotElementType()
                .ToElements() as List<Element>;

            List<Element> selectedViews = new List<Element>();

            foreach (DataRowView selectedName in form.PlanViewCheckList.CheckedItems)
                foreach (Element element in AllViews)
                {
                    if (element.Name.Equals(selectedName.Row[0]))
                        selectedViews.Add(element);
                }

            List<Viewport> viewports = new List<Viewport>();
            List<Parameter> levelParameter = new List<Parameter>();

            trans.Start();
            foreach (Element planview in selectedViews)
            {
                var alltitle_blocks = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_TitleBlocks)
                            .WhereElementIsElementType()
                            .ToElements();

                Element titleblock = null;
                foreach (FamilySymbol element in alltitle_blocks)
                    if (element.FamilyName.Contains(form.TitleBlockFamily.Text) && form.TitleBlockType.Text.Contains(element.Name))
                        titleblock = element;

                if (titleblock == null)
                {
                    TaskDialog.Show("Failed", "Could not find title block. If you are using your base select title block in settings make sure it is in this project.");
                    return Result.Failed;
                }

                ViewSheet newsheet = ViewSheet.Create(doc, titleblock.Id);
                
                newsheet.Name = form.SheetName.Text;

                var title_block = new FilteredElementCollector(doc, newsheet.Id)
                            .OfCategory(BuiltInCategory.OST_TitleBlocks)
                            .WhereElementIsNotElementType()
                            .ToElements();

                //Set selected parameters
                foreach (Parameter p in title_block[0].Parameters)
                {
                    if (form.ParameterCheckList.CheckedItems.Contains(p.Definition.Name))
                        p.Set(1);
                    else if (!p.IsReadOnly)
                        p.Set(0);
                }


                //Set area parameter
                if (form.AreaOverride.Checked)
                    title_block[0].LookupParameter(form.AreaOverrideComboBox.Text).Set(1);
                else
                {
                    string par = planview.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP).AsValueString();
                    string[] area = par.Split(' ');
                    foreach (Parameter p in title_block[0].Parameters)
                        if (p.Definition.Name.Contains(area[area.Count() - 1]))
                            p.Set(1);
                }

                //Set area parameter
                if (form.LevelOverride.Checked)
                    title_block[0].LookupParameter(form.LevelOverrideComboBox.Text).Set(1);
                else
                {
                    string par = planview.LookupParameter("Associated Level").AsValueString();
                    string[] level = par.Split(' ');
                    foreach (Parameter p in title_block[0].Parameters)
                        if (p.Definition.Name.Contains("Level"))
                            if (p.Definition.Name.Contains(level[level.Count() - 1]))
                                levelParameter.Add(p);
                                //title_block[0].LookupParameter(p.Definition.Name).Set(1);
                }

                XYZ xYZ = new XYZ();
                BoundingBoxXYZ boundingBoxXYZ = title_block[0].get_BoundingBox(newsheet);
                XYZ max = boundingBoxXYZ.Max;
                XYZ min = boundingBoxXYZ.Min;
                double X = (max.X+min.X) / 2;
                double Y = (max.Y + min.Y) / 2;

                XYZ viewportxyz = new XYZ(X,Y,0);

                Viewport viewport = Viewport.Create(doc, newsheet.Id, newsheet.Id, viewportxyz);
                viewports.Add(viewport);
            }
            trans.Commit();
            trans.Start();

            foreach (Parameter p in levelParameter)
                p.Set(1);

            foreach (Viewport viewport in viewports)
            {

                var scales = SettingsRead.Scale();
                
                ElementId sheetId = viewport.SheetId;
                string sheetscale = (doc.GetElement(sheetId) as View).Scale.ToString();

                foreach (var i in scales.Values)
                    if (i.Item2 == sheetscale)
                        viewport.ChangeTypeId(new ElementId(int.Parse(i.Item1)));
            }

            trans.Commit();
            
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    //Settings
    public class SheetSettingsMenu : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SheetSettings sheetSettings = new SheetSettings(commandData);
            sheetSettings.ShowDialog();
            return Result.Succeeded;
        }
    }
}

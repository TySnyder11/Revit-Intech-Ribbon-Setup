using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Intech;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;
namespace Intech.Tagging


{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    //Settings
    public class NumberTool : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;

            Revit.RevitUtils.init(doc);
            try
            {
                string filePath = Path.Combine(App.BasePath, "ReNumber.txt");
                SaveFileManager saveFileManager = new SaveFileManager(filePath, new TxtFormat());

                SaveFileSection sec = saveFileManager.GetSectionsByName("__General__").FirstOrDefault();
                if (sec == null)
                {
                    throw new InvalidOperationException("No section found for '__General__' in the save file. Please go to Numbering settings and make sure to add a row and click Confirm.");
                }
                List<Category> categories = new List<Category>();
                CategoryNameMap categoryMap = Intech.Revit.RevitUtils.GetAllCategories();
                foreach (string catNames in sec.GetColumn(0))
                {
                    categories.Add(categoryMap.get_Item(catNames));
                }
                // Create the selection filter
                ISelectionFilter filter = new CategoryObjectSelectionFilter(categories);

                // Prompt user to select multiple elements
                Reference selectedRef = uidoc.Selection.PickObject(ObjectType.Element, filter, "Select elements to number");
                Element selectedElement = uidoc.Document.GetElement(selectedRef);
                Category category = selectedElement.Category;
                string[] row = sec.lookUp(0, category.Name);
                string paramName = row[1];
                bool tag = string.Equals(row[2], "True");
                string prefix = row.Length > 3 ? row[3] : string.Empty;
                string num = row.Length > 4 ? row[4] : "1";
                string suffix = row.Length > 5 ? row[5] : string.Empty;
                string sep = row.Length > 6 ? row[6] : string.Empty;
                string whole = prefix + sep + num;
                if (!string.IsNullOrEmpty(suffix))
                {
                    whole += sep + suffix;
                }
                using (Transaction tran = new Transaction(doc))
                {
                    tran.Start("Number Parameter");
                    selectedElement.LookupParameter(paramName).Set(whole);
                    tran.Commit();
                }

                row[4] = (int.Parse(num) + 1).ToString();
                saveFileManager.AddOrUpdateSection(sec);
                var TagFam = tagtools.SaveInformation("Number");
                Intech.tagtools.tag
                    (
                    commandData,
                    TagFam.Category,
                    TagFam.Family,
                    TagFam.Path,
                    TagFam.TagFamily,
                    TagFam.Leader,
                    selectedElement.Id
                    );
                Execute(commandData, ref message, elements);
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"An error occurred: {ex.Message}");
                return Result.Failed;
            }
            return Result.Succeeded;
        }


        public class CategoryObjectSelectionFilter : ISelectionFilter
        {
            private readonly HashSet<ElementId> _allowedCategoryIds;

            public CategoryObjectSelectionFilter(IEnumerable<Category> categories)
            {
                _allowedCategoryIds = new HashSet<ElementId>(categories.Select(c => c.Id));
            }

            public bool AllowElement(Element elem)
            {
                return elem.Category != null && _allowedCategoryIds.Contains(elem.Category.Id);
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }
        }

    }
}

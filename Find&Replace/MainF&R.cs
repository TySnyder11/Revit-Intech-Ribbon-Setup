using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TitleBlockSetup.Find_Replace;

namespace Intech
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class MainFindandReplace : IExternalCommand
    {
        static Document doc = null;
        static UIDocument uidoc = null;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uidoc = commandData.Application.ActiveUIDocument;
            doc = uidoc.Document;

            FindAndReplaceUI findAndReplaceUI = new FindAndReplaceUI();
            findAndReplaceUI.ShowDialog();

            return Result.Succeeded;
        }

        public static List<Element> GetSelectedElements()
        {
            List<Reference> s = uidoc.Selection.GetReferences().ToList();

            List<Element> elems = new List<Element>();
            foreach (Reference r in s)
            {
                elems.Add(doc.GetElement(r));
            }
            return elems;
        }

        public static List<string> GetCommonTextParameters(List<Element> elements)
        {
            if (elements == null || elements.Count == 0)
                return new List<string>();

            // Start with the text parameters of the first element
            var commonParams = new HashSet<string>(
                elements[0].Parameters
                    .Cast<Parameter>()
                    .Where(p => p.StorageType == StorageType.String && p.HasValue)
                    .Select(p => p.Definition.Name)
            );

            // Intersect with the rest of the elements
            foreach (var elem in elements.Skip(1))
            {
                var currentParams = new HashSet<string>(
                    elem.Parameters
                        .Cast<Parameter>()
                        .Where(p => p.StorageType == StorageType.String && p.HasValue)
                        .Select(p => p.Definition.Name)
                );

                commonParams.IntersectWith(currentParams);
            }

            return commonParams.ToList();
        }

        public static bool Replace(List<Element> elements, string parameterName, string findText, string replaceText)
        {
            if (elements == null || elements.Count == 0 || string.IsNullOrEmpty(parameterName))
                return false;

            using (Transaction trans = new Transaction(doc, "Find and Replace, " + parameterName))
            {
                trans.Start();
                foreach (Element e in elements)
                {
                    Parameter p = e.LookupParameter(parameterName);
                    if (p != null)
                    {
                        string val = p.AsString();
                        string newVal = string.Empty;
                        if (string.IsNullOrEmpty(findText))
                        {
                            newVal = val + replaceText;
                        }
                        else
                        {
                            newVal = val.Replace(findText, replaceText);
                        }
                        p.Set(newVal);
                    }
                }
                trans.Commit();
            }

            return true;
        }
    }
}
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Intech;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech
{
    internal class tagtools
    {
        public static
            (
            List<String> Category,
            List<String> Path,
            List<String> TagFamily,
            List<bool> Leader
            )
        SaveInformation(string HangerType)
        {
            string path = Path.Combine(App.BasePath, "Settings.txt");


            //Get Txt information and take out header row
            Intech.SaveFileManager saveFileManager = new Intech.SaveFileManager(path, new TxtFormat());
            List<SaveFileSection> sections = saveFileManager.ReadAllSections();
            SaveFileSection section = saveFileManager.GetSectionsByName("Tag Settings").FirstOrDefault();

            //create lists
            List<string> Category = new List<string>();
            List<string> Family = new List<string>();
            List<string> Directory = new List<string>();
            List<string> TagFamily = new List<string>();
            List<bool> Leader = new List<bool>();
            List<string[]> data = section.lookUp(1, HangerType);
            foreach (var dataSection in data) {
                Category.Add(dataSection[2]);
                Directory.Add(dataSection[3]);
                TagFamily.Add(dataSection[4]);
                Leader.Add(bool.Parse(dataSection[5]));
            }


            return (Category, Directory, TagFamily, Leader);
        }
        public static (ElementId, int) Pickelement(ExternalCommandData commandData, List<string> Categories)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Selection selection = uidoc.Selection;



            //Select Element
            try
            {
                ReferenceArray ra = new ReferenceArray();
                ISelectionFilter selFilter = new SelectionFilter(Categories);
                ElementId selectedId = uidoc.Selection.PickObject(ObjectType.Element, selFilter, "Select Item to Tag").ElementId;

                //ElementId selectedId = uidoc.Selection.PickObject(ObjectType.Element).ElementId;
                return (selectedId, 1);
            }
            catch
            {
                ElementId cancel = new ElementId(0);
                return (cancel, 0);
            }
        }

        public static int tag
            (
            ExternalCommandData commandData,
            List<String> Category,
            List<String> Path,
            List<String> TagFamily,
            List<bool> Leader,
            ElementId ElementId
            )
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Selection selection = uidoc.Selection;


            //Get Elements information
            Element element = doc.GetElement(ElementId);
            string elemCategory = element.Category.Name;
            FabricationPart fab = element as FabricationPart;
            LocationCurve el = element.Location as LocationCurve;

            //get position of part
            XYZ xyz;
            if (element != null)
            {
                xyz = tagtools.getCenterByConnectors(element);
            }
            else
            {
                LocationPoint lp = element.Location as LocationPoint;
                xyz = lp.Point;
            }

            //Filters possible tags to get the correct one based off element information
            List<int> indexes = new List<int>();
            for (int i = 0; i < Category.Count(); i++)
            {
                if (Category[i].Contains(elemCategory))
                {
                    indexes.Add(i);
                }
            }
            if (indexes.Count() > 1)
            {
                //Create temp index to not override index till needed
                List<int> tempindexes = new List<int>();

                //See if issue is fixed
                if (!tempindexes.Any() || tempindexes.Count() > 1)
                {
                    TaskDialog.Show("Failed", "Multiple or duplicate tags under same conditions.");
                    return (0);
                }

                //Paste over old index with temp
                indexes = tempindexes;
            }
            //check for failsafe
            else if (indexes.Count() == 0)
            {
                TaskDialog.Show("Failed", "No matching tag for element. Category " + elemCategory);
                return (0);
            }
            else
            {
                Debug.WriteLine("Working");
            }
            int index = indexes[0];

            //Find tag family
            List<Family> Matching = new List<Family>();
            IList<Element> famsym = new FilteredElementCollector(doc).OfClass(typeof(Family)).ToElements();
            foreach (Family i in famsym)
            {
                if (i.Name.Contains(TagFamily[index]))
                {
                    Debug.WriteLine("Found tag family.");
                    Matching.Add(i);
                }

            }

            //If family not already in project load in
            if (!Matching.Any())
            {
                Family fam = null;
                Transaction loadtrans = new Transaction(doc, "Load Tag Family");

                loadtrans.Start();
                if (doc.LoadFamily(Path[index], out fam))
                {
                    Debug.WriteLine("Imported tag family.");
                    Matching.Add(fam);
                }
                else
                {
                    TaskDialog.Show("Failed to load family", "Manualy load tag into project or check path in Tag Settings." + elemCategory);
                }
                loadtrans.Commit();
            }

            //Get Tag Symbol Id
            ISet<ElementId> symbolId = Matching[0].GetFamilySymbolIds();

            Transaction tagtrans = new Transaction(doc, "Load Tag Family");

            //actually create tag
            tagtrans.Start();
            var newtag = IndependentTag.Create
                (
                doc, //doc
                symbolId.ElementAt(0), //tag symbol
                doc.ActiveView.Id, //Active view
                new Reference(element), //element to tag
                Leader[index], // Leader 
                TagOrientation.Horizontal, // orientating
                xyz //location
                );
            tagtrans.Commit();
            return (1);

        }

        //get center of fabricated part
        protected static XYZ getCenterByConnectors(Element part)
        {
            var connectors = GetConnectors(part).OfType<Connector>().ToList();

            XYZ average = XYZ.Zero;
            foreach (Connector con in connectors)
            {
                average += con.Origin;
            }
            average = average / connectors.Count;

            return average;
        }
        private static ConnectorSet GetConnectors(Element element)
        {
            if (element is FabricationPart fabPart)
            {
                return fabPart.ConnectorManager.Connectors;
            }
            if (element is FamilyInstance fi && fi.MEPModel != null)
            {
                return fi.MEPModel.ConnectorManager.Connectors;
            }
            if (element is Pipe pipe)
            {
                return pipe.ConnectorManager.Connectors;
            }
            if (element is Duct duct)
            {
                return duct.ConnectorManager.Connectors;
            }
            return null;
        }

        //filter what you are allowed to select
        public class SelectionFilter : ISelectionFilter
        {
            List<string> categories;
            public SelectionFilter(List<string> Category)
            {
                categories = Category;
            }
            public bool AllowElement(Element element)
            {
                if (element.Category == null)
                {
                    return false;
                }
                else if (categories.Contains(element.Category.Name))
                {
                    return true;
                }
                return false;
            }

            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }
    }
}

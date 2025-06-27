using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Intech.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech
{

    [Transaction(TransactionMode.Manual)]
    public class SleevePlace : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.UI.UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;
            Intech.Revit.RevitUtils.init(doc);

            List<Reference> elem = uidoc.Selection.GetReferences().ToList();
            List<RevitLinkInstance>  linked = Intech.Revit.RevitUtils.GetLinkedModels();
            string basePath = Path.Combine(App.BasePath, "Settings.txt");
            SaveFileManager saveFileManager = new SaveFileManager(basePath);
            SaveFileSection section = saveFileManager.GetSectionsByName("Sleeve Place", "linked Model");
            if (section.Rows.Count() == 0 && section.Rows[0].Count() == 0)
            {   
                return Result.Failed;
            }

            RevitLinkInstance structuralModel = linked.FirstOrDefault(l => l.Name == section.Rows[0][0]);
            List<Wall> walls = GetWallsFromLinkedModel(structuralModel);
            List<GeometryData> wallGeometry = ConvertWallsToGeometryData(walls);
            return Result.Succeeded;
        }


        public List<Wall> GetWallsFromLinkedModel(RevitLinkInstance structuralModel)
        {
            Document linkedDoc = structuralModel.GetLinkDocument();
            if (linkedDoc == null)
            {
                TaskDialog.Show("Error", "Linked document is not loaded.");
                return new List<Wall>();
            }

            FilteredElementCollector wallCollector = new FilteredElementCollector(linkedDoc);
            List<Wall> walls = wallCollector
            .OfClass(typeof(Wall))
            .Cast<Wall>()
            .ToList();

            return walls;
        }


        public List<GeometryData> ConvertWallsToGeometryData(List<Wall> walls)
        {
            List<GeometryData> geometryDataList = new List<GeometryData>();

            foreach (Wall wall in walls)
            {
                GeometryElement geomElement = wall.get_Geometry(new Options
                {
                    ComputeReferences = true,
                    IncludeNonVisibleObjects = false,
                    DetailLevel = ViewDetailLevel.Fine
                });

                if (geomElement == null) continue;

                foreach (GeometryObject geomObj in geomElement)
                {
                    Solid solid = geomObj as Solid;
                    if (solid != null && solid.Volume > 0)
                    {
                        geometryDataList.Add(new GeometryData
                        {
                            Solid = solid,
                            BoundingBox = wall.get_BoundingBox(null),
                            SourceElementId = wall.Id,
                            Role = GeometryRole.B
                        });
                    }
                }
            }

            return geometryDataList;
        }


    }
}

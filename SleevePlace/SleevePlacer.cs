using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Intech.Geometry;
using Intech.Geometry.Collision;
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

            List<GeometryData> mepGeometry = ElementToGeometryData.ConvertMEPToGeometryData(uidoc);
            if (mepGeometry.Count == 0)
            {
                return Result.Failed;
            }

            List<RevitLinkInstance> linked = Intech.Revit.RevitUtils.GetLinkedModels();
            string basePath = Path.Combine(App.BasePath, "Settings.txt");
            SaveFileManager saveFileManager = new SaveFileManager(basePath);
            SaveFileSection section = saveFileManager.GetSectionsByName("Sleeve Place", "linked Model");
            if (section.Rows.Count() == 0 && section.Rows[0].Count() == 0)
            {
                return Result.Failed;
            }

            RevitLinkInstance structuralModel = linked.FirstOrDefault(l => l.Name == section.Rows[0][0]);
            List<Wall> walls = GetWallsFromLinkedModel(structuralModel);
            List<GeometryData> wallGeometry = ConvertTransformedWallsToGeometryData(structuralModel, walls);
            if (wallGeometry.Count == 0)
            {
                return Result.Failed;
            }

            var engine = new Intech.Geometry.Collision.Engine.CollisionEngine();
            List<CollisionResult> test = new List<CollisionResult>();
            engine.Run(mepGeometry, wallGeometry, 15, 3, result =>
            {
                test.Add(result);
                PlaceSleeveAtCollision(result, doc);
            });
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

        public List<GeometryData> ConvertTransformedWallsToGeometryData(RevitLinkInstance linkInstance, List<Wall> walls)
        {
            List<GeometryData> geometryDataList = new List<GeometryData>();

            // Get the transform from the linked model to the host model
            Transform linkTransform = linkInstance.GetTransform();

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
                        // Apply the transform to the solid
                        Solid transformedSolid = SolidUtils.CreateTransformed(solid, linkTransform);

                        // Transform the bounding box
                        BoundingBoxXYZ originalBox = wall.get_BoundingBox(null);
                        BoundingBoxXYZ transformedBox = null;

                        if (originalBox != null)
                        {
                            transformedBox = new BoundingBoxXYZ
                            {
                                Min = linkTransform.OfPoint(originalBox.Min),
                                Max = linkTransform.OfPoint(originalBox.Max)
                            };
                        }

                        geometryDataList.Add(new GeometryData
                        {
                            Solid = transformedSolid,
                            BoundingBox = transformedBox,
                            SourceElementId = wall.Id,
                            Role = GeometryRole.B
                        });
                    }
                }
            }

            return geometryDataList;
        }

        public void PlaceSleeveAtCollision(CollisionResult collision, Document doc)
        {
            Solid intersection = collision.Intersection;
            if (intersection == null || intersection.Faces.Size == 0)
                return;

            XYZ center = ComputeCentroid(intersection);
            if (center == null)
                return;

            // Get linked model
            List<RevitLinkInstance> linked = Intech.Revit.RevitUtils.GetLinkedModels();
            string basePath = Path.Combine(App.BasePath, "Settings.txt");
            SaveFileManager saveFileManager = new SaveFileManager(basePath);
            SaveFileSection section = saveFileManager.GetSectionsByName("Sleeve Place", "linked Model");
            RevitLinkInstance structuralModel = linked.FirstOrDefault(l => l.Name == section.Rows[0][0]);

            if (structuralModel == null)
                return;

            Document linkedDoc = structuralModel.GetLinkDocument();
            if (linkedDoc == null)
                return;

            Element wallElement = linkedDoc.GetElement(collision.B.SourceElementId);
            if (!(wallElement is Wall wall))
                return;

            XYZ wallNormal = GetWallNormalFromElement(wall);
            if (wallNormal == null)
                return;

            // Determine if round or rectangular
            Element pipeElement = doc.GetElement(collision.A.SourceElementId);
            bool isRound = IsRound(pipeElement);

            // Get family and symbol names from settings
            SaveFileSection sec = saveFileManager.GetSectionsByName("Sleeve Place", "Sleeve Family");
            string familyName = isRound ? sec.Rows[0][0] : sec.Rows[0][1];
            string symbolName = isRound ? sec.Rows[1][0] : sec.Rows[1][1];

            // Find the correct FamilySymbol
            FamilySymbol sleeveSymbol = new FilteredElementCollector(doc)
             .OfClass(typeof(FamilySymbol))
             .OfType<FamilySymbol>()
             .FirstOrDefault(fs => fs.Family.Name == familyName && fs.Name == symbolName);

            if (sleeveSymbol == null)
            {
                TaskDialog.Show("Error", $"Sleeve family '{familyName}' with type '{symbolName}' not found.");
                return;
            }

            if (!sleeveSymbol.IsActive)
            {
                using (Transaction t = new Transaction(doc, "Activate Sleeve Symbol"))
                {
                    t.Start();
                    sleeveSymbol.Activate();
                    t.Commit();
                }
            }

            // Find nearest level in host document
            Level level = FindNearestLevel(doc, center);
            if (level == null)
                return;

            using (Transaction tx = new Transaction(doc, "Place Sleeve At Collision"))
            {
                tx.Start();

                if (SleeveExistsAt(center, doc, sleeveSymbol.Family.Name))
                    return;

                FamilyInstance sleeve = doc.Create.NewFamilyInstance(center, sleeveSymbol, level, StructuralType.NonStructural);

                // Rotate sleeve to align Z-axis with wall normal
                XYZ currentZ = GetSleeveAxis(sleeve);
                XYZ rotationAxis = currentZ.CrossProduct(wallNormal).Normalize();
                double angle = currentZ.AngleTo(wallNormal);

                if (!rotationAxis.IsZeroLength() && angle > 1e-6)
                {
                    Line axis = Line.CreateUnbound(center, rotationAxis);
                    ElementTransformUtils.RotateElement(doc, sleeve.Id, axis, angle);
                }

                // Set sleeve depth to wall thickness
                double thickness = GetWallThickness(wall);
                Parameter depthParam = sleeve.LookupParameter("Sleeve_Length");
                if (depthParam != null && !depthParam.IsReadOnly)
                {
                    depthParam.Set(thickness);
                }

                // Analyze intersection solid
                GetBoundingExtents(intersection, wallNormal, out double minZ, out double maxZ, out double minWall, out double maxWall);
                double height = maxZ - minZ;
                double width = maxWall - minWall;

                // Set sleeve dimensions
                SetSleeveDimensions(sleeve, isRound, width, height);

                tx.Commit();
            }
        }



        private XYZ ComputeCentroid(Solid solid)
        {
            XYZ sum = XYZ.Zero;
            int count = 0;

            foreach (Face face in solid.Faces)
            {
                Mesh mesh = face.Triangulate();
                foreach (XYZ vertex in mesh.Vertices)
                {
                    sum += vertex;
                    count++;
                }
            }

            return count > 0 ? sum / count : null;
        }

        private XYZ GetWallNormalFromElement(Wall wall)
        {
            LocationCurve locCurve = wall.Location as LocationCurve;
            if (locCurve != null)
            {
                Curve curve = locCurve.Curve;
                XYZ wallDirection = (curve.GetEndPoint(1) - curve.GetEndPoint(0)).Normalize();
                return wallDirection.CrossProduct(XYZ.BasisZ).Normalize(); // outward normal
            }
            return null;
        }

        private double GetWallThickness(Wall wall)
        {
            WallType wallType = wall.WallType;
            CompoundStructure structure = wallType.GetCompoundStructure();
            return structure != null ? structure.GetWidth() : 0;
        }

        private Level FindNearestLevel(Document doc, XYZ point)
        {
            return new FilteredElementCollector(doc)
            .OfClass(typeof(Level))
            .Cast<Level>()
            .OrderBy(lvl => Math.Abs(lvl.Elevation - point.Z))
            .FirstOrDefault();
        }

        private bool SleeveExistsAt(XYZ center, Document doc, string familyName, double tolerance = 0.05)
        {
            var existingSleeves = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilyInstance))
            .OfType<FamilyInstance>()
            .Where(fi => fi.Symbol.Family.Name == familyName);

            foreach (var sleeve in existingSleeves)
            {
                LocationPoint loc = sleeve.Location as LocationPoint;
                if (loc != null && loc.Point.DistanceTo(center) < tolerance)
                    return true;
            }

            return false;
        }

        private void GetBoundingExtents(Solid solid, XYZ wallNormal, out double minZ, out double maxZ, out double minWall, out double maxWall)
        {
            minZ = double.MaxValue;
            maxZ = double.MinValue;
            minWall = double.MaxValue;
            maxWall = double.MinValue;

            foreach (Face face in solid.Faces)
            {
                Mesh mesh = face.Triangulate();
                foreach (XYZ vertex in mesh.Vertices)
                {
                    double z = vertex.Z;
                    double wallCoord = vertex.DotProduct(wallNormal);

                    if (z < minZ) minZ = z;
                    if (z > maxZ) maxZ = z;
                    if (wallCoord < minWall) minWall = wallCoord;
                    if (wallCoord > maxWall) maxWall = wallCoord;
                }
            }
        }
        private bool IsRound(Element element)
        {
            if (element is Duct duct)
            {
                ConnectorSet connectors = duct.ConnectorManager.Connectors;
                foreach (Connector connector in connectors)
                {
                    if (connector.Shape == ConnectorProfileType.Round)
                        return true;
                    if (connector.Shape == ConnectorProfileType.Rectangular)
                        return false;
                }
            }

            if (element is Pipe)
                return true;

            return false;
        }



        private void SetSleeveDimensions(FamilyInstance sleeve, bool isRound, double width, double height)
        {
            if (isRound)
            {
                Parameter radiusParam = sleeve.LookupParameter("Sleeve_Nominal_Diameter");
                if (radiusParam != null && !radiusParam.IsReadOnly)
                    radiusParam.Set(Math.Max(width, height)); // assuming width = diameter
            }
            else
            {
                Parameter widthParam = sleeve.LookupParameter("Sleeve_Width");
                Parameter heightParam = sleeve.LookupParameter("Sleeve_Length");

                if (widthParam != null && !widthParam.IsReadOnly)
                    widthParam.Set(width);

                if (heightParam != null && !heightParam.IsReadOnly)
                    heightParam.Set(height);
            }
        }

        private XYZ GetSleeveAxis(FamilyInstance sleeve)
        {
            if (sleeve.MEPModel != null)
            {
                ConnectorSet connectors = sleeve.MEPModel.ConnectorManager.Connectors;
                var ordered = connectors.Cast<Connector>().OrderBy(c => c.Origin.Z).ToList();

                if (ordered.Count >= 2)
                {
                    XYZ dir = (ordered[1].Origin - ordered[0].Origin).Normalize();
                    return dir;
                }
            }

            return XYZ.BasisZ; // fallback
        }

    }
}

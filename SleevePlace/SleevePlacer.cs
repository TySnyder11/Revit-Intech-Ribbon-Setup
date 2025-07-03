using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
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
            List<Reference> references = null;
            try
            {
                references = uidoc.Selection.PickObjects(
                    ObjectType.Element,
                    new DuctPipeSelectionFilter(),
                    "Select ducts or pipes."
                    , uidoc.Selection.GetReferences()).ToList();
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }

            List<GeometryData> mepGeometry = ElementToGeometryData.ConvertMEPToGeometryData(references, doc);
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
            List<CollisionResult> res = new List<CollisionResult>();
            engine.Run(mepGeometry, wallGeometry, 15, 3, result =>
            {
                res.Add(result);
            });
            foreach ( CollisionResult result in res)
            {
                placeSleeveAtCollision(result, doc);
            }

            return Result.Succeeded;
        }

        public class DuctPipeSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return elem is Duct || elem is FabricationPart || (elem is FamilyInstance fi && fi.MEPModel != null) ||
                       (elem is FabricationPart) || elem is Pipe;
            }

            public bool AllowReference(Reference r, XYZ p)
            {
                return true;
            }
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

        string[] activeSettings = null;
        public void placeSleeveAtCollision(CollisionResult collision, Document doc)
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
            double insulationThickness = GetInsulationThickness(doc, pipeElement);


            // Get family and symbol names from settings
            SaveFileSection sec = null;
            if (isRound)
            {
                sec = saveFileManager.GetSectionsByName("Sleeve Place", "Round Sleeve");
            }
            else
            {
                sec = saveFileManager.GetSectionsByName("Sleeve Place", "Rectangular Sleeve");
            }
            activeSettings = sec.lookUp(0, "True").FirstOrDefault() ?? sec.Rows[0];

            string familyName = activeSettings[2];
            string symbolName = activeSettings[3];

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

                // Set sleeve lenght to wall thickness
                double lengthTolerance = double.Parse(activeSettings[6]) / 12;

                double rawThickness = GetWallThickness(wall);
                Parameter depthParam = sleeve.LookupParameter(activeSettings[4]);
                if (depthParam != null && !depthParam.IsReadOnly)
                {
                    double thickness = RoundUpToNearestIncrement(rawThickness, double.Parse(activeSettings[8]));
                    depthParam.Set(thickness);
                }

                // Analyze intersection solid
                double heightTolerance = 0;
                double widthTolerance = 0;
                if (isRound)
                {

                    heightTolerance = double.Parse(activeSettings[7])/12;
                    widthTolerance = double.Parse(activeSettings[7])/12;
                }
                else
                {
                    heightTolerance = 0;
                    widthTolerance = 0 / 12;
                }


                    GetBoundingExtents(intersection, wallNormal, out double minZ, out double maxZ, out double minWall, out double maxWall);
                double rawHeight = maxZ - minZ + 2 * insulationThickness + heightTolerance;
                double rawWidth = maxWall - minWall + 2 * insulationThickness + widthTolerance;

                double height = 0;
                double width = 0;
                if (isRound)
                {
                    height = RoundUpToNearestIncrement(rawHeight, double.Parse(activeSettings[9]));
                    width = RoundUpToNearestIncrement(rawWidth, double.Parse(activeSettings[9]));
                }
                else
                {
                    height = RoundUpToNearestIncrement(rawHeight, 0.5);
                    width = RoundUpToNearestIncrement(rawWidth, 0.5);
                }

                // Set sleeve dimensions
                SetSleeveDimensions(sleeve, isRound, width, height);
                MoveFamilyInstanceTo(sleeve, center);
                tx.Commit();
            }
        }

        public void MoveFamilyInstanceTo(FamilyInstance instance, XYZ targetPoint)
        {
            LocationPoint location = instance.Location as LocationPoint;
            if (location == null)
                throw new InvalidOperationException("FamilyInstance does not have a LocationPoint.");

            XYZ currentPoint = location.Point;
            XYZ translation = targetPoint - currentPoint;

            if (!translation.IsZeroLength())
            {
                ElementTransformUtils.MoveElement(instance.Document, instance.Id, translation);
            }
        }


        private double GetInsulationThickness(Document doc, Element pipe)
        {
            if (pipe == null)
                return 0;

            ElementId pipeId = pipe.Id;

            var insulation = new FilteredElementCollector(doc)
            .OfClass(typeof(PipeInsulation))
            .Cast<PipeInsulation>()
            .FirstOrDefault(ins => ins.HostElementId == pipeId);

            if (insulation != null)
            {

                Parameter thicknessParam = insulation.LookupParameter("Insulation Thickness");
                if (thicknessParam != null && thicknessParam.HasValue)
                    return thicknessParam.AsDouble(); // in feet
            }

            return 0;
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


        private double RoundUpToNearestIncrement(double valueInFeet, double incrementInInches)
        {
            if (incrementInInches != 0)
            {
                double incrementInFeet = incrementInInches / 12.0;
                double tolerance = 1e-6; // Arbitrarily small value in feet (~0.000012 inches)

                double remainder = valueInFeet % incrementInFeet;

                if (remainder < tolerance || Math.Abs(remainder - incrementInFeet) < tolerance)
                {
                    // Already close enough to an increment — return the rounded value
                    return Math.Round(valueInFeet / incrementInFeet) * incrementInFeet;
                }

                // Otherwise, round up
                return Math.Ceiling(valueInFeet / incrementInFeet) * incrementInFeet;
            }
            return valueInFeet;
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

        private bool SleeveExistsAt(XYZ center, Document doc, string familyName, double tolerance = 0.1)
        {
            // Create an Outline (not BoundingBoxXYZ)
            XYZ min = new XYZ(center.X - tolerance, center.Y - tolerance, center.Z - tolerance);
            XYZ max = new XYZ(center.X + tolerance, center.Y + tolerance, center.Z + tolerance);
            Outline outline = new Outline(min, max);

            BoundingBoxIntersectsFilter bboxFilter = new BoundingBoxIntersectsFilter(outline);

            var collector = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilyInstance))
            .WherePasses(bboxFilter)
            .Cast<FamilyInstance>()
            .Where(fi => fi.Symbol.Family.Name.Equals(familyName, StringComparison.OrdinalIgnoreCase));

            foreach (var sleeve in collector)
            {
                if (sleeve.Location is LocationPoint locPoint)
                {
                    if (locPoint.Point.DistanceTo(center) < tolerance)
                        return true;
                }
            }

            return false;
        }

        private void GetBoundingExtents(Solid solid, XYZ wallNormal, out double minZ, out double maxZ, out double minSection, out double maxSection)
        {
            minZ = double.MaxValue;
            maxZ = double.MinValue;

            minSection = double.MaxValue;
            maxSection = double.MinValue;


            XYZ reference = XYZ.BasisZ;
            if (wallNormal.IsAlmostEqualTo(XYZ.BasisZ))
                reference = XYZ.BasisX;

            XYZ sectionDirection = wallNormal.CrossProduct(reference).Normalize();

            foreach (Face face in solid.Faces)
            {
                Mesh mesh = face.Triangulate();
                foreach (XYZ vertex in mesh.Vertices)
                {
                    double z = vertex.Z;
                    double sectionCoord = vertex.DotProduct(sectionDirection);

                    if (z < minZ) minZ = z;
                    if (z > maxZ) maxZ = z;
                    if (sectionCoord < minSection) minSection = sectionCoord;
                    if (sectionCoord > maxSection) maxSection = sectionCoord;
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
                Parameter diameterParam = sleeve.LookupParameter(activeSettings[5]);
                if (diameterParam != null && !diameterParam.IsReadOnly)
                    diameterParam.Set(Math.Max(width, height));
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

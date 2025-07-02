using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinForms = System.Windows.Forms;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class RotateConnectedElements : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {

                // Get the current selection
                Selection sel = uidoc.Selection;
                ICollection<ElementId> selectedIds = sel.GetElementIds();

                List<Reference> ductPipeRefs = new List<Reference>();

                // Filter the current selection for ducts or pipes
                if (selectedIds.Count > 0)
                {
                    foreach (ElementId id in selectedIds)
                    {
                        Element elem = doc.GetElement(id);
                        if (new DuctPipeSelectionFilter().AllowElement(elem))
                        {
                            ductPipeRefs.Add(new Reference(elem));
                        }
                    }
                }


                ductPipeRefs = uidoc.Selection.PickObjects(
                ObjectType.Element,
                new DuctPipeSelectionFilter(),
                "Select ducts or pipes."
                ,ductPipeRefs
                ).ToList();

                List<ElementId> ductPipeIds = new List<ElementId>();
                foreach (Reference r in ductPipeRefs)
                {
                    ductPipeIds.Add(r.ElementId);
                }
                if (ductPipeIds.Count == 0)
                {
                    message = "No ducts or pipes selected.";
                    return Result.Failed;
                }

                // STEP 2: Select the fitting.
                Reference fittingRef = uidoc.Selection.PickObject(
                    ObjectType.Element,
                    new DuctPipeSelectionFilter(),
                    "Select the fitting to rotate."
                );
                Element fitting = doc.GetElement(fittingRef.ElementId);
                if (fitting == null)
                {
                    message = "Selected element is not a valid fitting.";
                    return Result.Failed;
                }
                // Optionally highlight the fitting.
                uidoc.Selection.SetElementIds(new List<ElementId> { fitting.Id });

                // STEP 3: Retrieve the connectors from the fitting.
                ConnectorSet connectors = GetConnectors(fitting);
                if (connectors == null || connectors.IsEmpty || connectors.Size < 2)
                {
                    message = "The selected fitting must have at least two connectors.";
                    return Result.Failed;
                }

                // STEP 4: Retrieve the fitting’s distance parameter.
                double close = 0;
                if (fitting is FamilyInstance fi)
                {
                    Parameter param = fitting.LookupParameter("Total Height");
                    if (param == null)
                    {
                        message = "Total Height parameter not found in the selected fitting.";
                        return Result.Failed;
                    }
                    close = param.AsDouble();
                }
                else if (fitting is FabricationPart)
                {
                    Parameter param = fitting.LookupParameter("eM_CenterlineLength");
                    if (param == null)
                    {
                        message = "eM_CenterlineLength parameter not found in the selected fitting.";
                        return Result.Failed;
                    }
                    close = param.AsDouble();
                }

                // STEP 5: Determine the connector pair using the provided distance "close".
                if (!GetMainAxisConnectorPair(connectors, out Connector connectorA, out Connector connectorB))
                {
                    message = "Unable to determine the main axis from the fitting's connectors.";
                    return Result.Failed;
                }
                XYZ pointA = connectorA.Origin;
                XYZ pointB = connectorB.Origin;
                XYZ axisDirection = (pointB - pointA).Normalize();
                // Use the midpoint of the two connectors as the base point of the axis.
                XYZ basePoint = (pointA + pointB) * 0.5;
                List<ElementId> tempBoxIds = new List<ElementId>();
                double radAngle = 0;
                using (Transaction tran = new Transaction(doc, "rotation visual"))
                {

                    tran.Start();
                    double offset = 2.0;
                    XYZ downPoint = basePoint + axisDirection.CrossProduct(XYZ.BasisZ).Normalize() * offset;
                    XYZ upPoint = basePoint - axisDirection.CrossProduct(XYZ.BasisZ).Normalize() * offset;

                    TextNoteType textType = new FilteredElementCollector(doc)
                     .OfClass(typeof(TextNoteType))
                     .Cast<TextNoteType>()
                     .FirstOrDefault();

                    if (textType == null)
                    {
                        message = "No TextNoteType found in the project.";
                        return Result.Failed;
                    }

                    TextNote upNote = TextNote.Create(doc, doc.ActiveView.Id, upPoint, "UP", textType.Id);
                    TextNote downNote = TextNote.Create(doc, doc.ActiveView.Id, downPoint, "DOWN", textType.Id);


                    OverrideGraphicSettings upColor = new OverrideGraphicSettings();
                    upColor.SetProjectionLineColor(new Color(0, 255, 0)); // Green
                    doc.ActiveView.SetElementOverrides(upNote.Id, upColor);

                    OverrideGraphicSettings downColor = new OverrideGraphicSettings();
                    downColor.SetProjectionLineColor(new Color(255, 0, 0)); // Red
                    doc.ActiveView.SetElementOverrides(downNote.Id, downColor);

                    tran.Commit();
                    // After creating each box:
                    tempBoxIds.Add(upNote.Id);
                    tempBoxIds.Add(downNote.Id);

                    string angleStr = MyInputBox.Show("Enter rotation angle in degrees:", "Rotation Angle", "");
                    DeleteTempBoxes(doc, tempBoxIds);
                    if (string.IsNullOrEmpty(angleStr))
                    {
                        message = "No rotation angle provided.";
                        return Result.Cancelled;
                    }
                    if (!double.TryParse(angleStr, out double degrees))
                    {
                        message = "Invalid rotation angle.";
                        return Result.Failed;
                    }
                    radAngle = degrees * Math.PI / 180.0; // Convert to radians
                }

                using (Transaction t = new Transaction(doc, "Rotate Fitting and Connected Elements"))
                {
                    t.Start();
                    // Create a rotation axis line through the basePoint in the computed direction.
                    Line rotationAxis = Line.CreateBound(basePoint, basePoint + axisDirection);
                    // Rotate the fitting.
                    
                    ElementTransformUtils.RotateElements(doc, ductPipeIds, rotationAxis, radAngle);
                    t.Commit();
                }

                TaskDialog.Show("Success", $"Rotated fitting and {ductPipeIds.Count} connected elements by {radAngle * 180 / Math.PI}°.");
                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
        Solid CreateBoxSolid(XYZ center, double size)
        {
            double half = size / 2;

            List<Curve> profile = new List<Curve>
            {
                Line.CreateBound(new XYZ(-half, -half, 0), new XYZ(half, -half, 0)),
                Line.CreateBound(new XYZ(half, -half, 0), new XYZ(half, half, 0)),
                Line.CreateBound(new XYZ(half, half, 0), new XYZ(-half, half, 0)),
                Line.CreateBound(new XYZ(-half, half, 0), new XYZ(-half, -half, 0))
            };

            CurveLoop loop = CurveLoop.Create(profile);
            Solid box = GeometryCreationUtilities.CreateExtrusionGeometry(new List<CurveLoop> { loop }, XYZ.BasisZ, size);

            Transform move = Transform.CreateTranslation(center - new XYZ(0, 0, size / 2));
            return SolidUtils.CreateTransformed(box, move);
        }

        /// <summary>
        /// Retrieves the connector set from the fitting.
        /// </summary>
        private ConnectorSet GetConnectors(Element element)
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

        private void DeleteTempBoxes(Document doc, List<ElementId> boxIds)
        {
            using (Transaction tx = new Transaction(doc, "Delete Temp Boxes"))
            {
                tx.Start();
                foreach (ElementId id in boxIds)
                {
                    if (doc.GetElement(id) != null)
                        doc.Delete(id);
                }
                tx.Commit();
            }
        }

        /// <summary>
        /// Finds the pair of connectors whose separation is closest to the given distance parameter.
        /// </summary>
        private bool GetMainAxisConnectorPair(ConnectorSet connectors, out Connector bestA, out Connector bestB)
        {
            bestA = null;
            bestB = null;
            foreach (Connector conn1 in connectors)
            {
                XYZ basis1 = conn1.CoordinateSystem.BasisZ;
                XYZ xyz1 = conn1.CoordinateSystem.Origin;
                foreach (Connector conn2 in connectors)
                {
                    if (conn1.Equals(conn2)) continue;
                    XYZ xyz2 = conn2.CoordinateSystem.Origin;
                    XYZ delta = xyz2 - xyz1;

                    // Check if delta is parallel to basis1
                    XYZ cross = delta.CrossProduct(basis1);
                    if (cross.GetLength() < 1e-9)
                    {
                        if(conn1.Origin.X < conn2.Origin.X || (conn1.Origin.X == conn2.Origin.X && conn1.Origin.Y < conn2.Origin.Y) ||
                         (conn1.Origin.X == conn2.Origin.X && conn1.Origin.Y == conn2.Origin.Y && conn1.Origin.Z < conn2.Origin.Z))
                        {
                            bestA = conn1;
                            bestB = conn2;
                        }
                        else
                        {
                            bestA = conn2;
                            bestB = conn1;
                        }
                    }
                }
            }
            return (bestA != null && bestB != null);
        }

        /// <summary>
        /// A selection filter that accepts only fittings – FamilyInstances that have a valid MEPModel or FabricationParts.
        /// </summary>
        public class FittingSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return (elem is FamilyInstance fi && fi.MEPModel != null) || (elem is FabricationPart);
            }

            public bool AllowReference(Reference r, XYZ p)
            {
                return true;
            }
        }

        /// <summary>
        /// A selection filter that accepts only ducts or pipes.
        /// </summary>
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
    }
}
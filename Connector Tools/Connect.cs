using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Finance.FinancialDayCount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ConnectElementsCommand : IExternalCommand
    {
        public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Selection selection = uidoc.Selection;

            try
            {
                // Select first element
                Reference ref1 = selection.PickObject(ObjectType.Element, "Select first duct or pipe");
                Element element1 = uidoc.Document.GetElement(ref1);

                // Determine filter based on first selection
                ISelectionFilter filter = null;
                if (element1 is FabricationPart)
                    filter = new FabricationDuctSelectionFilter();
                else if (element1 is Pipe)
                    filter = new ConnectorSelectionFilter();
                else if (element1 is Duct)
                    filter = new DuctworkSelectionFilter();
                else
                {
                    TaskDialog.Show("Error", "Selected element is neither a duct nor a pipe.");
                    return Result.Failed;
                }

                // Select second element with filtering
                Reference ref2 = selection.PickObject(ObjectType.Element, filter, "Select second element");
                Element element2 = uidoc.Document.GetElement(ref2);

                // Execute appropriate connection logic
                if (element1 is FabricationPart && element2 is FabricationPart)
                    ConnectFab(doc, element1, element2);
                else if (element1 is Pipe && (element2 is MEPCurve ||
                       (element2 is FamilyInstance fi && fi.MEPModel != null)))
                    ConnectPipes(doc, element1 as Pipe, element2);
                else if (element1 is Duct && (element2 is MEPCurve ||
                       (element2 is FamilyInstance fI && fI.MEPModel != null)))
                {
                    ConnectDuct(doc, element1 as Duct, element2);
                }
                else
                {
                    TaskDialog.Show("Error", "Selected elements are not compatible.");
                    return Result.Failed;
                }
                Execute(commandData,ref message, elements);
                return Result.Succeeded;
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
        }
        public class DuctworkSelectionFilter : ISelectionFilter
        {
            // Allow elements that are ducts
            public bool AllowElement(Element elem)
            {
                if (elem == null)
                    return false;

                // If the element's category is null, ignore it
                if (elem.Category == null)
                    return false;

                // Allow element if its category is either Duct Curves or Duct Fittings
                long categoryId = elem.Category.Id.Value;
                if (categoryId == (long)BuiltInCategory.OST_DuctCurves ||
                    categoryId == (long)BuiltInCategory.OST_DuctFitting)
                {
                    return true;
                }

                return false;
            }

            // Disallow references (not used in this case)
            public bool AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }
        public void ConnectPipes(Document doc, Pipe pipe, Element fitting)
        {
            // Get basic Revit objects.

            try
            {
                (Connector pipeConnector, Connector targetConnector) pair = GetClosestConnectorPair(pipe, fitting);
                if (pair.pipeConnector == null || pair.targetConnector == null)
                {
                    TaskDialog.Show("Error", "Unable to find a valid connector pair.");
                    return;
                }

                using (Transaction t = new Transaction(doc, "Connect Pipe"))
                {
                    t.Start();

                    // Re-retrieve the pipe connector now that the pipe has been repositioned.
                    Connector adjustedConnector = GetClosestConnectorByPosition(pipe, pair.targetConnector.Origin);
                    if (adjustedConnector != null)
                    {
                        if( adjustedConnector.IsConnectedTo(pair.targetConnector))
                        {
                            adjustedConnector.DisconnectFrom(pair.targetConnector);
                        }
                        AlignConnectorToConnector(adjustedConnector, pair.targetConnector, doc);
                        adjustedConnector.ConnectTo(pair.targetConnector);
                    }

                    t.Commit();
                }

                return;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return;
            }
        }

        public static void AlignConnectorToConnector(Connector fromConnector, Connector toConnector, Document doc)
        {
            Element element = fromConnector.Owner;

            XYZ fromOrigin = fromConnector.Origin;
            XYZ fromZ = fromConnector.CoordinateSystem.BasisZ;
            XYZ toOrigin = toConnector.Origin;
            XYZ toZ = toConnector.CoordinateSystem.BasisZ;

            // Project toOrigin onto the line defined by fromOrigin and fromZ
            XYZ vectorToTarget = toOrigin - fromOrigin;
            double projectionLength = vectorToTarget.DotProduct(fromZ);
            XYZ projectedPoint = fromOrigin + projectionLength * fromZ;

            // Compute translation vector
            XYZ translation = toOrigin - projectedPoint;

            // Apply translation
            if (!translation.IsZeroLength())
            {
                ElementTransformUtils.MoveElement(doc, element.Id, translation);
            }

            // Compute rotation
            XYZ rotationAxis = fromZ.CrossProduct(-toZ);
            double angle = fromZ.AngleTo(-toZ);

            if (!rotationAxis.IsZeroLength() && angle > 1e-6)
            {
                rotationAxis = rotationAxis.Normalize();
                Line rotationLine = Line.CreateUnbound(toOrigin, rotationAxis);
                ElementTransformUtils.RotateElement(doc, element.Id, rotationLine, angle);
            }

            if (element is Pipe pipe)
            {
                LocationCurve locationCurve = pipe.Location as LocationCurve;
                if (locationCurve?.Curve is Line line)
                {
                    XYZ start = line.GetEndPoint(0);
                    XYZ end = line.GetEndPoint(1);

                    // Find which end is closer to the toConnector
                    double distToStart = start.DistanceTo(toOrigin);
                    double distToEnd = end.DistanceTo(toOrigin);

                    Line newLine = distToStart < distToEnd
                    ? Line.CreateBound(toOrigin, end)
                    : Line.CreateBound(start, toOrigin);

                    locationCurve.Curve = newLine;
                }
            }

        }


        public void ConnectDuct(Document doc, Duct Duct1, Element Duct2)
        {
            // Get basic Revit objects.

            try
            {

                // STEP 3: Get the closest connector pair between the pipe and the target.
                (Connector pipeConnector, Connector targetConnector) pair = GetClosestConnectorPair(Duct1, Duct2);
                if (pair.pipeConnector == null || pair.targetConnector == null)
                {
                    TaskDialog.Show("Error", "Unable to find a valid connector pair.");
                    return;
                }

                // STEP 4: Modify the pipe's geometry without reversing its direction.
                // This version only rebuilds the pipe’s curve when the pipe connector is at an endpoint.
                using (Transaction t = new Transaction(doc, "Reposition Pipe"))
                {
                    t.Start();

                    LocationCurve locCurve = Duct1.Location as LocationCurve;
                    if (locCurve == null)
                    {
                        TaskDialog.Show("Error", "Pipe does not have a valid LocationCurve.");
                        return;
                    }

                    // Retrieve the current endpoints.
                    XYZ start = locCurve.Curve.GetEndPoint(0);
                    XYZ end = locCurve.Curve.GetEndPoint(1);
                    double tolerance = 0.001; // Adjust tolerance as needed in project units

                    Line newLine = null;
                    if (pair.pipeConnector.Origin.IsAlmostEqualTo(start, tolerance))
                    {
                        // The connector is at the start; keep the end fixed.
                        newLine = Line.CreateBound(pair.targetConnector.Origin, end);
                    }
                    else if (pair.pipeConnector.Origin.IsAlmostEqualTo(end, tolerance))
                    {
                        // The connector is at the end; keep the start fixed.
                        newLine = Line.CreateBound(start, pair.targetConnector.Origin);
                    }
                    else
                    {
                        TaskDialog.Show("Info", "Pipe connector is not at an endpoint. Consider using a translation approach instead.");
                        t.RollBack();
                        return;
                    }

                    // Update the pipe with the new line.
                    locCurve.Curve = newLine;
                    t.Commit();
                }

                // STEP 5: Connect the pipe's connector to the target connector.
                using (Transaction t = new Transaction(doc, "Connect Pipe"))
                {
                    t.Start();

                    // Re-retrieve the pipe connector now that the pipe has been repositioned.
                    Connector adjustedConnector = GetClosestConnectorByPosition(Duct1, pair.targetConnector.Origin);
                    if (adjustedConnector != null)
                    {   
                        adjustedConnector.ConnectTo(pair.targetConnector);
                    }
                    t.Commit();
                }
                return;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return;
            }
        }

        // Helper to get the union set of connectors for an element.
        private ConnectorSet GetConnectors(Element element)
        {
            ConnectorSet connectors = null;
            if (element is MEPCurve mepCurve)
                connectors = mepCurve.ConnectorManager.Connectors;
            else if (element is FamilyInstance fi && fi.MEPModel != null)
                connectors = fi.MEPModel.ConnectorManager.Connectors;
            return connectors;
        }

        // Iterates through all connectors on the pipe and target element to find the pair with the smallest distance.
        private (Connector, Connector) GetClosestConnectorPair(MEPCurve mepElement, Element target)
        {
            ConnectorSet mepConnectors = mepElement.ConnectorManager?.Connectors;
            ConnectorSet targetConnectors = GetConnectors(target);

            if (mepConnectors == null || targetConnectors == null)
                return (null, null);

            double bestDistance = double.MaxValue;
            Connector bestMEP = null;
            Connector bestTarget = null;

            foreach (Connector mc in mepConnectors)
            {
                foreach (Connector tc in targetConnectors)
                {
                    double d = mc.Origin.DistanceTo(tc.Origin);
                    if (d < bestDistance)
                    {
                        bestDistance = d;
                        bestMEP = mc;
                        bestTarget = tc;
                    }
                }
            }
            return (bestMEP, bestTarget);
        }

        // Finds the pipe connector that is closest to a specified position.
        private Connector GetClosestConnectorByPosition(MEPCurve curve, XYZ position)
        {
            ConnectorSet curveConnectors = curve.ConnectorManager.Connectors;
            Connector best = null;
            double bestDistance = double.MaxValue;
            foreach (Connector connector in curveConnectors)
            {
                double d = connector.Origin.DistanceTo(position);
                if (d < bestDistance)
                {
                    bestDistance = d;
                    best = connector;
                }
            }
            return best;
        }

        // A selection filter to permit only pipes.
        public class PipeSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem) => elem is Pipe;
            public bool AllowReference(Reference reference, XYZ position) => false;
        }

        // A selection filter for elements that contain connectors (MEPCurves or MEP FamilyInstances).
        public class ConnectorSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return (elem is MEPCurve) ||
                       (elem is FamilyInstance fi && fi.MEPModel != null);
            }
            public bool AllowReference(Reference reference, XYZ position) => false;
        }
        public ElementId ElementToSelect { get; set; }
        public string GetName()
        {
            return "Selection Update Handler";
        }
        public void ConnectFab(Document doc, Element duct1, Element duct2)
        {
            try
            {
                // Retrieve connectors from the first duct.
                ConnectorSet connectors1 = GetConnectorSet(duct1);
                if (connectors1 == null)
                {
                    TaskDialog.Show("Error", "The first selected duct does not have any connectors.");
                    return;
                }

                // Retrieve connectors from the second duct.
                ConnectorSet connectors2 = GetConnectorSet(duct2);
                if (connectors2 == null)
                {
                    TaskDialog.Show("Error", "The second selected duct does not have any connectors.");
                    return;
                }

                // Convert ConnectorSet to enumerable for iteration.
                var conns1 = connectors1.Cast<Connector>();
                var conns2 = connectors2.Cast<Connector>();

                // Find the closest pair of connector origins.
                Connector closestConnector1 = null;
                Connector closestConnector2 = null;
                double minDistance = double.MaxValue;

                foreach (var c1 in conns1)
                {
                    foreach (var c2 in conns2)
                    {
                        double dist = c1.Origin.DistanceTo(c2.Origin);
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            closestConnector1 = c1;
                            closestConnector2 = c2;
                        }
                    }
                }

                if (closestConnector1 == null || closestConnector2 == null)
                {
                    TaskDialog.Show("Error", "No valid connector pair could be found.");
                    return;
                }

                // Calculate the vector to move the first duct so its connector aligns with the second duct's connector.
                XYZ moveVector = closestConnector2.Origin - closestConnector1.Origin;

                using (Transaction tx = new Transaction(doc, "Move and Connect Duct"))
                {
                    tx.Start();
                    ElementTransformUtils.MoveElement(doc, duct1.Id, moveVector);
                    // Attempt to connect the two connectors.
                    closestConnector1.ConnectTo(closestConnector2);

                    tx.Commit();
                }
                return;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                // User canceled the operation.
                return;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
                return;
            }
        }

        /// <summary>
        /// Attempts to retrieve the connector set from an element.
        /// Checks whether the element is a FabricationPart or a FamilyInstance with a valid MEPModel.
        /// </summary>
        private ConnectorSet GetConnectorSet(Element e)
        {
            if (e == null)
                return null;
            if (e is FabricationPart fabPart)
            {
                return fabPart.ConnectorManager.Connectors;
            }
            else if (e is FamilyInstance fi && fi.MEPModel != null)
            {
                return fi.MEPModel.ConnectorManager?.Connectors;
            }
            return null;
        }
    }

    /// <summary>
    /// A selection filter that allows any element with a category of Duct Curves or Fabrication Ductwork,
    /// covering both standard and fabricated ducts (such as spiral ducts).
    /// </summary>
    public class FabricationDuctSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element element)
        {
            if (element == null || element.Category == null)
                return false;

            Int64 catId = element.Category.Id.Value;
            return (catId == (Int64)BuiltInCategory.OST_DuctCurves ||
                    catId == (Int64)BuiltInCategory.OST_FabricationDuctwork);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}

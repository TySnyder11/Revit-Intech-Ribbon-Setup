using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
[Transaction(TransactionMode.Manual)]
public class NumberRun : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uidoc = commandData.Application.ActiveUIDocument;
        Document doc = uidoc.Document;

        try
        {
            Reference startRef = uidoc.Selection.PickObject(ObjectType.Element, "Select start element");
            Element startElem = doc.GetElement(startRef);
            MEPSystem startSystem = GetSystem(startElem);

            if (startSystem == null)
            {
                TaskDialog.Show("Error", "Start element is not part of an MEP system.");
                return Result.Cancelled;
            }

            Reference endRef = uidoc.Selection.PickObject(ObjectType.Element, new ConnectedElementFilter(startSystem), "Select connected end element");
            Element endElem = doc.GetElement(endRef);

            Connector startConnector = GetPrimaryConnector(startElem);
            Connector endConnector = GetPrimaryConnector(endElem);

            if (startConnector == null || endConnector == null)
            {
                TaskDialog.Show("Error", "Could not find connectors.");
                return Result.Failed;
            }

            List<Connector> path = FindPathWithHeuristic(startConnector, endConnector);

            using (Transaction tx = new Transaction(doc, "Apply Dummy Numbering"))
            {
                tx.Start();

                int index = 1;
                foreach (Connector conn in path)
                {
                    Element owner = conn.Owner as Element;
                    if (owner != null)
                    {
                        Parameter param = owner.LookupParameter("DummyNumberingCode");
                        if (param != null && !param.IsReadOnly)
                        {
                            param.Set(string.Format("P-{0:D3}", index));
                            index++;
                        }
                    }
                }

                tx.Commit();
            }
            TaskDialog.Show("Success", string.Format("Numbering applied to {0} elements.", path.Count));
            Execute(commandData, ref message, elements);
            return Result.Succeeded;
        }
        catch (Autodesk.Revit.Exceptions.OperationCanceledException)
        {
            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return Result.Failed;
        }
    }

    private static MEPSystem GetSystem(Element element)
    {
        if (element is Pipe)
            return ((Pipe)element).MEPSystem;
        if (element is Duct)
            return ((Duct)element).MEPSystem;
        if (element is FlexDuct)
            return ((FlexDuct)element).MEPSystem;
        if (element is FlexPipe)
            return ((FlexPipe)element).MEPSystem;
        if (element is FamilyInstance fi && fi.MEPModel != null)
        {
            foreach (Connector c in fi.MEPModel.ConnectorManager.Connectors)
            {
                return c.MEPSystem;
            }
        }

        return null;
    }

    private Connector GetPrimaryConnector(Element element)
    {
        ConnectorSet connectors = null;

        if (element is Pipe)
            connectors = ((Pipe)element).ConnectorManager != null ? ((Pipe)element).ConnectorManager.Connectors : null;
        else if (element is Duct)
            connectors = ((Duct)element).ConnectorManager != null ? ((Duct)element).ConnectorManager.Connectors : null;
        else if (element is FlexDuct)
            connectors = ((FlexDuct)element).ConnectorManager != null ? ((FlexDuct)element).ConnectorManager.Connectors : null;
        else if (element is FlexPipe)
            connectors = ((FlexPipe)element).ConnectorManager != null ? ((FlexPipe)element).ConnectorManager.Connectors : null;
        else if (element is FamilyInstance)
        {
            FamilyInstance fi = (FamilyInstance)element;
            if (fi.MEPModel != null && fi.MEPModel.ConnectorManager != null)
                connectors = fi.MEPModel.ConnectorManager.Connectors;
        }

        if (connectors != null)
        {
            foreach (Connector c in connectors)
                return c;
        }

        return null;
    }

    private List<Connector> FindPathWithHeuristic(Connector start, Connector goal)
    {
        var openSet = new SortedList<double, Connector>();
        var cameFrom = new Dictionary<Connector, Connector>();
        var costSoFar = new Dictionary<Connector, double>();

        openSet.Add(0, start);
        costSoFar[start] = 0;

        while (openSet.Count > 0)
        {
            Connector current = openSet.Values[0];
            openSet.RemoveAt(0);

            if (current.Origin.IsAlmostEqualTo(goal.Origin))
                return ReconstructPath(cameFrom, current);

            foreach (Connector neighbor in GetConnectedConnectors(current))
            {
                double newCost = costSoFar[current] + current.Origin.DistanceTo(neighbor.Origin);
                if (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor])
                {
                    costSoFar[neighbor] = newCost;
                    double priority = newCost + EuclideanDistance(neighbor.Origin, goal.Origin);
                    openSet.Add(priority, neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return new List<Connector>(); // No path found
    }

    private List<Connector> ReconstructPath(Dictionary<Connector, Connector> cameFrom, Connector current)
    {
        var path = new List<Connector> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }

    private IEnumerable<Connector> GetConnectedConnectors(Connector connector)
    {
        var connected = new List<Connector>();
        foreach (Connector refConn in connector.AllRefs)
        {
            if (!refConn.IsConnected) continue;
            if (refConn.Owner.Id != connector.Owner.Id)
                connected.Add(refConn);
        }
        return connected;
    }

    private double EuclideanDistance(XYZ a, XYZ b)
    {
        return Math.Sqrt(
            Math.Pow(a.X - b.X, 2) +
            Math.Pow(a.Y - b.Y, 2) +
            Math.Pow(a.Z - b.Z, 2));
    }

    private class ConnectedElementFilter : ISelectionFilter
    {
        private readonly MEPSystem _system;

        public ConnectedElementFilter(MEPSystem system)
        {
            _system = system;
        }

        public bool AllowElement(Element elem)
        {
            MEPSystem other = GetSystem(elem);
            return other != null && other.Id == _system.Id;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}


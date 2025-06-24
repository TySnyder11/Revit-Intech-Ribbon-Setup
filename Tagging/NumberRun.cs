using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Intech.Tagging
{
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

                Reference endRef = uidoc.Selection.PickObject(ObjectType.Element, "Select end element");
                Element endElem = doc.GetElement(endRef);

                List<Element> path = FindElementPathWithHeuristic(doc, startElem, endElem);

                if (path.Count == 0)
                {
                    TaskDialog.Show("Path Not Found", "No path could be found between the selected elements.");
                    return Result.Failed;
                }

                using (Transaction tx = new Transaction(doc, "Apply Dummy Numbering"))
                {
                    tx.Start();

                    int index = 1;
                    foreach (Element elem in path)
                    {
                        Parameter param = elem.LookupParameter("Mark");
                        if (param != null && !param.IsReadOnly)
                        {
                            param.Set($"P-{index:D3}");
                            index++;
                        }
                    }

                    tx.Commit();
                }

                TaskDialog.Show("Success", $"Numbering applied to {path.Count} elements.");
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

        private List<Element> FindElementPathWithHeuristic(Document doc, Element startElem, Element goalElem)
        {
            var openSet = new SortedList<double, Element>();
            var cameFrom = new Dictionary<ElementId, ElementId>();
            var costSoFar = new Dictionary<ElementId, double>();
            var visited = new HashSet<ElementId>();

            XYZ goalPos = GetAverageConnectorPosition(goalElem);
            XYZ startPos = GetAverageConnectorPosition(startElem);

            openSet.Add(0, startElem);
            costSoFar[startElem.Id] = 0;

            while (openSet.Count > 0)
            {
                Element current = openSet.Values[0];
                openSet.RemoveAt(0);

                if (current.Id == goalElem.Id)
                    return ReconstructElementPath(cameFrom, current.Id, startElem.Id, doc);

                visited.Add(current.Id);

                foreach (Connector conn in GetConnectors(current))
                {
                    foreach (Connector refConn in conn.AllRefs)
                    {
                        if (!refConn.IsConnected) continue;

                        Element neighbor = refConn.Owner as Element;
                        if (neighbor == null || visited.Contains(neighbor.Id)) continue;

                        double newCost = costSoFar[current.Id] + conn.Origin.DistanceTo(refConn.Origin);

                        if (!costSoFar.ContainsKey(neighbor.Id) || newCost < costSoFar[neighbor.Id])
                        {
                            costSoFar[neighbor.Id] = newCost;

                            XYZ neighborPos = GetAverageConnectorPosition(neighbor);
                            double heuristic = neighborPos.DistanceTo(goalPos);
                            double priority = newCost + heuristic;

                            while (openSet.ContainsKey(priority)) priority += 0.0001;

                            openSet.Add(priority, neighbor);
                            cameFrom[neighbor.Id] = current.Id;
                        }
                    }
                }
            }

            return new List<Element>(); // No path found
        }

        private List<Element> ReconstructElementPath(Dictionary<ElementId, ElementId> cameFrom, ElementId currentId, ElementId startId, Document doc)
        {
            var path = new List<Element>();
            while (currentId != startId)
            {
                path.Insert(0, doc.GetElement(currentId));
                currentId = cameFrom[currentId];
            }
            path.Insert(0, doc.GetElement(startId));
            return path;
        }

        private IEnumerable<Connector> GetConnectors(Element element)
        {
            ConnectorSet connectors = null;

            if (element is MEPCurve mEP)
                connectors = mEP.ConnectorManager?.Connectors;
            else if (element is FamilyInstance fi)
                connectors = fi.MEPModel?.ConnectorManager?.Connectors;
            else if (element is FabricationPart fab)
                connectors = fab.ConnectorManager?.Connectors;

            return connectors?.Cast<Connector>() ?? Enumerable.Empty<Connector>();
        }

        private XYZ GetAverageConnectorPosition(Element element)
        {
            var connectors = GetConnectors(element).ToList();
            if (!connectors.Any()) return XYZ.Zero;

            XYZ sum = XYZ.Zero;
            foreach (var c in connectors)
                sum += c.Origin;

            return sum.Divide(connectors.Count);
        }

    }
}

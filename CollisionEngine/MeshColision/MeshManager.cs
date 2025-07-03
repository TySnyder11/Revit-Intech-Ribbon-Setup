using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Geometry
{
    using Autodesk.Revit.DB;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using static Intech.Geometry.Collision.MeshCollision;

    public static class MeshManager
    {
        public static ConnectionMesh FromRevitMesh(Mesh revitMesh)
        {
            var mesh = new ConnectionMesh();

            for (int i = 0; i < revitMesh.NumTriangles; i++)
            {
                MeshTriangle tri = revitMesh.get_Triangle(i);

                var v0 = mesh.GetOrCreateVertex(ToVector3(tri.get_Vertex(0)));
                var v1 = mesh.GetOrCreateVertex(ToVector3(tri.get_Vertex(1)));
                var v2 = mesh.GetOrCreateVertex(ToVector3(tri.get_Vertex(2)));

                var e0 = mesh.GetOrCreateEdge(v0, v1);
                var e1 = mesh.GetOrCreateEdge(v1, v2);
                var e2 = mesh.GetOrCreateEdge(v2, v0);

                var polygon = new Polygon();
                polygon.Edges.AddRange(new[] { e0, e1, e2 });

                AssignPolygonToEdge(e0, polygon);
                AssignPolygonToEdge(e1, polygon);
                AssignPolygonToEdge(e2, polygon);

                mesh.Polygons.Add(polygon);
            }

            return mesh;
        }

        public static ConnectionMesh Merge(ConnectionMesh a, ConnectionMesh b)
        {
            var merged = new ConnectionMesh();

            void AddPolygon(Polygon poly)
            {
                var newPoly = new Polygon();

                foreach (var edge in poly.Edges)
                {
                    var vStart = merged.GetOrCreateVertex(edge.Start.Position);
                    var vEnd = merged.GetOrCreateVertex(edge.End.Position);

                    var newEdge = merged.GetOrCreateEdge(vStart, vEnd);
                    newPoly.Edges.Add(newEdge);

                    if (newEdge.PolygonA == null)
                        newEdge.PolygonA = newPoly;
                    else if (newEdge.PolygonB == null)
                        newEdge.PolygonB = newPoly;
                    else
                        throw new InvalidOperationException("Edge already connected to two polygons.");
                }

                merged.Polygons.Add(newPoly);
            }

            foreach (var poly in a.Polygons)
                AddPolygon(poly);

            foreach (var poly in b.Polygons)
                AddPolygon(poly);

            return merged;
        }

        private static Vector3 ToVector3(XYZ xyz) =>
            new Vector3((float)xyz.X, (float)xyz.Y, (float)xyz.Z);

        private static void AssignPolygonToEdge(LineSegment edge, Polygon poly)
        {
            if (edge.PolygonA == null)
                edge.PolygonA = poly;
            else if (edge.PolygonB == null)
                edge.PolygonB = poly;
            else
                throw new InvalidOperationException("Edge already connected to two polygons.");
        }

        public static ConnectionMesh Intersect(ConnectionMesh meshA, ConnectionMesh meshB)
        {
            var grid = new SpatialGrid(0.1f);
            grid.InsertMesh(meshA);
            grid.InsertMesh(meshB);

            var intersections = new List<IntersectionResult>();
            foreach (var (polyA, polyB) in grid.GetPotentialCollisions())
            {
                var triA = GetTriangle(polyA);
                var triB = GetTriangle(polyB);

                if (TryIntersect(triA, triB, out var segments))
                {
                    foreach (var seg in segments)
                    {
                        var v0 = meshA.GetOrCreateVertex(seg.Item1);
                        var v1 = meshA.GetOrCreateVertex(seg.Item2);
                        var split = new LineSegment(v0, v1) { IsSplit = true };

                        intersections.Add(new IntersectionResult(polyA, polyB, split));
                    }
                }
            }

            var splitA = SplitMesh(meshA, intersections, true);
            var splitB = SplitMesh(meshB, intersections, false);

            GraphRepair.Repair(splitA);
            GraphRepair.Repair(splitB);

            var insideA = ContainmentTester.GetContainedPolygons(splitA, splitB);
            var insideB = ContainmentTester.GetContainedPolygons(splitB, splitA);

            return MeshMerger.MergePolygons(insideA, insideB);
        }

        private static Vector3[] GetTriangle(Polygon poly)
        {
            var verts = new List<Vector3>();
            foreach (var edge in poly.Edges)
                verts.Add(edge.Start.Position);

            return new[] { verts[0], verts[1], verts[2] };
        }

        public class IntersectionResult
        {
            public Polygon PolyA { get; }
            public Polygon PolyB { get; }
            public LineSegment Segment { get; }

            public IntersectionResult(Polygon polyA, Polygon polyB, LineSegment segment)
            {
                PolyA = polyA;
                PolyB = polyB;
                Segment = segment;
            }
        }

        private static ConnectionMesh SplitMesh(ConnectionMesh mesh, List<IntersectionResult> intersections, bool isMeshA)
        {
            var newMesh = new ConnectionMesh();

            foreach (var poly in mesh.Polygons)
            {
                var cuts = new List<LineSegment>();
                foreach (var inter in intersections)
                {
                    if ((isMeshA && inter.PolyA == poly) || (!isMeshA && inter.PolyB == poly))
                        cuts.Add(inter.Segment);
                }

                if (cuts.Count == 0)
                {
                    newMesh.Polygons.Add(poly);
                }
                else
                {

                    var newPolys = FaceReconstructor.SplitPolygonWithCuts(poly, cuts, newMesh);
                    newMesh.Polygons.AddRange(newPolys);
                }
            }

            return newMesh;
        }

        public static class FaceReconstructor
        {

            public static List<Polygon> SplitPolygonWithCuts(
             Polygon original,
             List<LineSegment> cuts,
             ConnectionMesh targetMesh)
            {
                // Step 1: Collect all edges (original + cuts)
                var allEdges = new List<(Vertex, Vertex)>();
                foreach (var edge in original.Edges)
                    allEdges.Add((edge.Start, edge.End));
                foreach (var cut in cuts)
                    allEdges.Add((cut.Start, cut.End));

                // Step 2: Project all vertices to 2D (XY plane)
                var projected = new Dictionary<Vertex, Vector2>();
                foreach (var (a, b) in allEdges)
                {
                    if (!projected.ContainsKey(a))
                        projected[a] = new Vector2(a.Position.X, a.Position.Y);
                    if (!projected.ContainsKey(b))
                        projected[b] = new Vector2(b.Position.X, b.Position.Y);
                }

                // Step 3: Build directed edge graph with angle sorting
                var edgeMap = new Dictionary<Vertex, List<Vertex>>();
                foreach (var (a, b) in allEdges)
                {
                    AddDirectedEdge(edgeMap, a, b);
                    AddDirectedEdge(edgeMap, b, a); // bidirectional
                }

                // Step 4: Sort outgoing edges by angle (CCW)
                var sortedEdges = new Dictionary<Vertex, List<Vertex>>();
                foreach (var kvp in edgeMap)
                {
                    var origin = kvp.Key;
                    var origin2D = projected[origin];
                    var neighbors = kvp.Value;

                    neighbors.Sort((v1, v2) =>
                    {
                        var a1 = AngleCCW(origin2D, projected[v1]);
                        var a2 = AngleCCW(origin2D, projected[v2]);
                        return a1.CompareTo(a2);
                    });

                    sortedEdges[origin] = neighbors;
                }

                // Step 5: Walk all faces
                var visited = new HashSet<string>();
                var faces = new List<List<Vertex>>();

                foreach (var from in sortedEdges.Keys)
                {
                    foreach (var to in sortedEdges[from])
                    {
                        string edgeKey = GetEdgeKey(from, to);
                        if (visited.Contains(edgeKey)) continue;

                        var face = WalkFace(from, to, sortedEdges, visited);
                        if (face != null && face.Count >= 3)
                            faces.Add(face);
                    }
                }

                // Step 6: Convert faces to Polygons
                var polygons = new List<Polygon>();
                var seenLoops = new HashSet<string>();

                foreach (var loop in faces)
                {
                    string loopKey = GetLoopKey(loop);
                    if (seenLoops.Contains(loopKey)) continue;
                    seenLoops.Add(loopKey);

                    var poly = new Polygon();
                    for (int i = 0; i < loop.Count; i++)
                    {
                        var a = loop[i];
                        var b = loop[(i + 1) % loop.Count];

                        var edge = targetMesh.GetOrCreateEdge(a, b);
                        poly.Edges.Add(edge);

                        if (edge.PolygonA == null) edge.PolygonA = poly;
                        else if (edge.PolygonB == null) edge.PolygonB = poly;
                    }

                    polygons.Add(poly);
                }

                return polygons;
            }

            private static void AddDirectedEdge(Dictionary<Vertex, List<Vertex>> map, Vertex from, Vertex to)
            {
                List<Vertex> list;
                if (!map.TryGetValue(from, out list))
                {
                    list = new List<Vertex>();
                    map[from] = list;
                }
                if (!list.Contains(to))
                    list.Add(to);
            }

            private static float AngleCCW(Vector2 origin, Vector2 target)
            {
                Vector2 dir = target - origin;
                return (float)Math.Atan2(dir.Y, dir.X);
            }

            private static string GetEdgeKey(Vertex a, Vertex b)
            {
                return a.GetHashCode() + "->" + b.GetHashCode();
            }

            private static string GetLoopKey(List<Vertex> loop)
            {
                var ids = new List<int>();
                foreach (var v in loop)
                    ids.Add(v.GetHashCode());

                ids.Sort();
                return string.Join("-", ids);
            }

            private static List<Vertex> WalkFace(
            Vertex start,
            Vertex next,
            Dictionary<Vertex, List<Vertex>> sortedEdges,
            HashSet<string> visited)
            {
                var face = new List<Vertex>();
                face.Add(start);
                face.Add(next);

                Vertex current = next;
                Vertex previous = start;

                while (true)
                {
                    visited.Add(GetEdgeKey(previous, current));

                    List<Vertex> neighbors = sortedEdges[current];
                    int index = neighbors.IndexOf(previous);
                    int nextIndex = (index + 1) % neighbors.Count;
                    Vertex nextVertex = neighbors[nextIndex];

                    if (nextVertex.Equals(start))
                        break;

                    if (face.Contains(nextVertex))
                        return null; // self-intersecting

                    face.Add(nextVertex);
                    previous = current;
                    current = nextVertex;
                }

                return face;
            }
        }
    }
}

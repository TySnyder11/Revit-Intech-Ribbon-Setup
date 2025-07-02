using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Intech.Geometry.Collision
{
    internal class MeshCollision
    {

        public static bool TryIntersect(
         Vector3[] triA, Vector3[] triB,
         out List<(Vector3, Vector3)> intersectionSegments)
        {
            intersectionSegments = new List<(Vector3, Vector3)>();

            if (AreCoplanar(triA, triB, out Vector3 normal))
            {
                return TryCoplanarOverlap(triA, triB, normal, out intersectionSegments);
            }

            for (int i = 0; i < 3; i++)
            {
                Vector3 p0 = triA[i];
                Vector3 p1 = triA[(i + 1) % 3];

                if (TrySegmentTriangleIntersect(p0, p1, triB, out Vector3 hit0, out Vector3 hit1))
                {
                    intersectionSegments.Add((hit0, hit1));
                }
            }

            return intersectionSegments.Count > 0;
        }

        private static bool TrySegmentTriangleIntersect(Vector3 p0, Vector3 p1, Vector3[] tri, out Vector3 hit0, out Vector3 hit1)
        {
            hit0 = hit1 = default;

            Vector3 dir = p1 - p0;
            float length = dir.Length();
            dir = Vector3.Normalize(dir);

            if (RayIntersectsTriangle(p0, dir, tri, out Vector3 hit, out float t) && t <= length)
            {
                hit0 = hit;
                return true;
            }

            return false;
        }

        private static bool RayIntersectsTriangle(Vector3 origin, Vector3 dir, Vector3[] tri, out Vector3 hit, out float t)
        {
            hit = default;
            t = 0;

            const float EPSILON = 1e-6f;
            Vector3 v0 = tri[0], v1 = tri[1], v2 = tri[2];
            Vector3 edge1 = v1 - v0;
            Vector3 edge2 = v2 - v0;

            Vector3 h = Vector3.Cross(dir, edge2);
            float a = Vector3.Dot(edge1, h);
            if (Math.Abs(a) < EPSILON) return false;

            float f = 1.0f / a;
            Vector3 s = origin - v0;
            float u = f * Vector3.Dot(s, h);
            if (u < 0.0 || u > 1.0) return false;

            Vector3 q = Vector3.Cross(s, edge1);
            float v = f * Vector3.Dot(dir, q);
            if (v < 0.0 || u + v > 1.0) return false;

            t = f * Vector3.Dot(edge2, q);
            if (t > EPSILON)
            {
                hit = origin + dir * t;
                return true;
            }

            return false;
        }

        private static bool AreCoplanar(Vector3[] triA, Vector3[] triB, out Vector3 normal)
        {
            normal = Vector3.Normalize(Vector3.Cross(triA[1] - triA[0], triA[2] - triA[0]));
            float d = -Vector3.Dot(normal, triA[0]);

            foreach (var v in triB)
            {
                if (Math.Abs(Vector3.Dot(normal, v) + d) > 1e-5f)
                    return false;
            }

            return true;
        }

        private static bool TryCoplanarOverlap(Vector3[] triA, Vector3[] triB, Vector3 normal, out List<(Vector3, Vector3)> segments)
        {
            segments = new List<(Vector3, Vector3)>();

            // Project to 2D plane
            Func<Vector3, Vector2> project = GetBest2DProjection(normal);

            var polyA = new List<Vector2> { project(triA[0]), project(triA[1]), project(triA[2]) };
            var polyB = new List<Vector2> { project(triB[0]), project(triB[1]), project(triB[2]) };

            var clipped = PolygonClipper.Clip(polyA, polyB); // You’ll need to implement or use a 2D polygon clipper

            foreach (var edge in clipped)
            {
                segments.Add((new Vector3(edge.Item1.X, edge.Item1.Y, 0), new Vector3(edge.Item2.X, edge.Item2.Y, 0)));
            }

            return segments.Count > 0;
        }

        private static Func<Vector3, Vector2> GetBest2DProjection(Vector3 normal)
        {
            normal = Vector3.Normalize(normal);
            if (Math.Abs(normal.Z) > Math.Abs(normal.X) && Math.Abs(normal.Z) > Math.Abs(normal.Y))
                return v => new Vector2(v.X, v.Y); // Project to XY
            else if (Math.Abs(normal.Y) > Math.Abs(normal.X))
                return v => new Vector2(v.X, v.Z); // Project to XZ
            else
                return v => new Vector2(v.Y, v.Z); // Project to YZ
        }

        public class SpatialGrid
        {
            private readonly float cellSize;
            private readonly Dictionary<(int, int, int), List<Polygon>> grid = new Dictionary<(int, int, int), List<Polygon>>();

            public SpatialGrid(float cellSize)
            {
                this.cellSize = cellSize;
            }

            public void InsertMesh(ConnectionMesh mesh)
            {
                foreach (var poly in mesh.Polygons)
                {
                    var bounds = GetBounds(poly);
                    var min = ToGrid(bounds.min);
                    var max = ToGrid(bounds.max);

                    for (int x = min.Item1; x <= max.Item1; x++)
                        for (int y = min.Item2; y <= max.Item2; y++)
                            for (int z = min.Item3; z <= max.Item3; z++)
                            {
                                var key = (x, y, z);
                                if (!grid.TryGetValue(key, out var list))
                                    grid[key] = list = new List<Polygon>();
                                list.Add(poly);
                            }
                }
            }

            public IEnumerable<(Polygon, Polygon)> GetPotentialCollisions()
            {
                var seen = new HashSet<(Polygon, Polygon)>();

                foreach (var cell in grid.Values)
                {
                    for (int i = 0; i < cell.Count; i++)
                        for (int j = i + 1; j < cell.Count; j++)
                        {
                            var a = cell[i];
                            var b = cell[j];
                            var pair = a.GetHashCode() < b.GetHashCode() ? (a, b) : (b, a);
                            if (seen.Add(pair))
                                yield return pair;
                        }
                }
            }

            private (Vector3 min, Vector3 max) GetBounds(Polygon poly)
            {
                Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                foreach (var edge in poly.Edges)
                {
                    min = Vector3.Min(min, edge.Start.Position);
                    min = Vector3.Min(min, edge.End.Position);
                    max = Vector3.Max(max, edge.Start.Position);
                    max = Vector3.Max(max, edge.End.Position);
                }
                return (min, max);
            }

            private (int, int, int) ToGrid(Vector3 pos)
            {
                return (
                    (int)Math.Floor(pos.X / cellSize),
                    (int)Math.Floor(pos.Y / cellSize),
                    (int)Math.Floor(pos.Z / cellSize)
                );
            }
        }

        public static class TriangleSplitter
        {
            public static List<Polygon> SplitPolygon(Polygon triangle, LineSegment split, ConnectionMesh mesh)
            {
                var verts = new List<Vertex>();
                foreach (var edge in triangle.Edges)
                {
                    verts.Add(edge.Start);
                }

                var v0 = split.Start;
                var v1 = split.End;

                // Create two new polygons
                var poly1 = new Polygon();
                var poly2 = new Polygon();

                // Build new edges and assign to polygons
                var e1 = mesh.GetOrCreateEdge(verts[0], v0);
                var e2 = mesh.GetOrCreateEdge(v0, v1);
                var e3 = mesh.GetOrCreateEdge(v1, verts[0]);

                poly1.Edges.AddRange(new[] { e1, e2, e3 });

                var e4 = mesh.GetOrCreateEdge(verts[1], v0);
                var e5 = mesh.GetOrCreateEdge(v0, verts[2]);
                var e6 = mesh.GetOrCreateEdge(verts[2], verts[1]);

                poly2.Edges.AddRange(new[] { e4, e5, e6 });

                // Assign polygons to edges
                foreach (var e in poly1.Edges)
                {
                    if (e.PolygonA == null) e.PolygonA = poly1;
                    else if (e.PolygonB == null) e.PolygonB = poly1;
                }

                foreach (var e in poly2.Edges)
                {
                    if (e.PolygonA == null) e.PolygonA = poly2;
                    else if (e.PolygonB == null) e.PolygonB = poly2;
                }

                return new List<Polygon> { poly1, poly2 };
            }
        }

        public static class GraphRepair
        {
            public static void Repair(ConnectionMesh mesh)
            {
                mesh.EdgeDict.Clear();

                foreach (var poly in mesh.Polygons)
                {
                    foreach (var edge in poly.Edges)
                    {
                        var key = edge.Start.GetHashCode() < edge.End.GetHashCode()
                        ? (edge.Start, edge.End)
                        : (edge.End, edge.Start);

                        if (!mesh.EdgeDict.ContainsKey(key))
                        {
                            mesh.EdgeDict[key] = edge;
                        }

                        if (edge.PolygonA == null)
                            edge.PolygonA = poly;
                        else if (edge.PolygonB == null)
                            edge.PolygonB = poly;
                    }
                }
            }
        }

        public static class ContainmentTester
        {
            public static List<Polygon> GetContainedPolygons(ConnectionMesh source, ConnectionMesh target)
            {
                var result = new List<Polygon>();
                var visited = new HashSet<Polygon>();

                foreach (var poly in source.Polygons)
                {
                    if (visited.Contains(poly)) continue;

                    var group = source.GetConnectedPolygons(poly);
                    visited.UnionWith(group);

                    var centroid = ComputeCentroid(group);
                    if (IsPointInsideMesh(centroid, target))
                    {
                        result.AddRange(group);
                    }
                }

                return result;
            }

            private static Vector3 ComputeCentroid(IEnumerable<Polygon> group)
            {
                Vector3 sum = Vector3.Zero;
                int count = 0;

                foreach (var poly in group)
                {
                    foreach (var edge in poly.Edges)
                    {
                        sum += edge.Start.Position;
                        sum += edge.End.Position;
                        count += 2;
                    }
                }

                return sum / count;
            }

            private static bool IsPointInsideMesh(Vector3 point, ConnectionMesh mesh)
            {
                Vector3 dir = new Vector3(1, 0.123f, 0.456f); // Arbitrary non-axis-aligned direction
                int hits = 0;

                foreach (var poly in mesh.Polygons)
                {
                    var verts = new List<Vector3>();
                    foreach (var edge in poly.Edges)
                        verts.Add(edge.Start.Position);

                    if (verts.Count < 3) continue;

                    var tri = new Vector3[] { verts[0], verts[1], verts[2] };
                    if (RayIntersectsTriangle(point, dir, tri, out _, out _))
                        hits++;
                }

                return hits % 2 == 1;
            }
        }

        public static class MeshMerger
        {
            public static ConnectionMesh MergePolygons(List<Polygon> polysA, List<Polygon> polysB)
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
                    }

                    merged.Polygons.Add(newPoly);
                }

                foreach (var poly in polysA) AddPolygon(poly);
                foreach (var poly in polysB) AddPolygon(poly);

                return merged;
            }
        }

    }
}

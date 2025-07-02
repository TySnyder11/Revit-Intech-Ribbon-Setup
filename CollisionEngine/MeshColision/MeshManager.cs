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
                var splits = new List<LineSegment>();
                foreach (var inter in intersections)
                {
                    if ((isMeshA && inter.PolyA == poly) || (!isMeshA && inter.PolyB == poly))
                        splits.Add(inter.Segment);
                }

                if (splits.Count == 0)
                {
                    newMesh.Polygons.Add(poly);
                }
                else
                {
                    foreach (var split in splits)
                    {
                        var newPolys = TriangleSplitter.SplitPolygon(poly, split, newMesh);
                        newMesh.Polygons.AddRange(newPolys);
                    }
                }
            }

            return newMesh;
        }

    }
}

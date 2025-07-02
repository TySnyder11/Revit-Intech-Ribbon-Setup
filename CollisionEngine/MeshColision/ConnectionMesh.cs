using Autodesk.Revit.DB.DirectContext3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Geometry
{
    public readonly struct Vertex
    {
        public readonly Vector3 Position;

        public Vertex(Vector3 position)
        {
            Position = position;
        }

        public override int GetHashCode() => Position.GetHashCode();
        public override bool Equals(object obj) => obj is Vertex other && Position.Equals(other.Position);
    }

    public class LineSegment
    {
        public Vertex Start { get; }
        public Vertex End { get; }

        public Polygon PolygonA { get; set; }
        public Polygon PolygonB { get; set; }

        public bool IsSplit { get; set; }

        public LineSegment(Vertex start, Vertex end)
        {
            Start = start;
            End = end;
        }

        public Polygon GetOtherPolygon(Polygon poly)
        {
            if (PolygonA == poly) return PolygonB;
            if (PolygonB == poly) return PolygonA;
            return null;
        }
    }

    public class Polygon
    {
        public List<LineSegment> Edges { get; } = new List<LineSegment>();
    }

    public class ConnectionMesh
    {
        public Dictionary<Vector3, Vertex> VertexHash = new Dictionary<Vector3, Vertex>();
        public Dictionary<(Vertex, Vertex), LineSegment> EdgeDict = new Dictionary<(Vertex, Vertex), LineSegment>();
        public List<Polygon> Polygons = new List<Polygon>();

        public Vertex GetOrCreateVertex(Vector3 pos)
        {
            if (!VertexHash.TryGetValue(pos, out var vertex))
            {
                vertex = new Vertex(pos);
                VertexHash[pos] = vertex;
            }
            return vertex;
        }

        public LineSegment GetOrCreateEdge(Vertex a, Vertex b)
        {
            var key = a.GetHashCode() < b.GetHashCode() ? (a, b) : (b, a);
            if (!EdgeDict.TryGetValue(key, out var edge))
            {
                edge = new LineSegment(a, b);
                EdgeDict[key] = edge;
            }
            return edge;
        }

        public HashSet<Polygon> GetConnectedPolygons(Polygon start)
        {
            var visited = new HashSet<Polygon>();
            var queue = new Queue<Polygon>();
            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var edge in current.Edges)
                {
                    if (edge.IsSplit) continue;

                    var neighbor = edge.GetOtherPolygon(current);
                    if (neighbor != null && !visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return visited;
        }

    }
}

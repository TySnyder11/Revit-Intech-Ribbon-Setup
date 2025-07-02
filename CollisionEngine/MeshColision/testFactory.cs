using Intech.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Intech.UnitTest
{
    public static class TestMeshFactory
    {
        public static ConnectionMesh CreateTriangle(Vector3 a, Vector3 b, Vector3 c)
        {
            var mesh = new ConnectionMesh();

            var va = mesh.GetOrCreateVertex(a);
            var vb = mesh.GetOrCreateVertex(b);
            var vc = mesh.GetOrCreateVertex(c);

            var e1 = mesh.GetOrCreateEdge(va, vb);
            var e2 = mesh.GetOrCreateEdge(vb, vc);
            var e3 = mesh.GetOrCreateEdge(vc, va);

            var poly = new Polygon();
            poly.Edges.AddRange(new[] { e1, e2, e3 });

            if (e1.PolygonA == null) e1.PolygonA = poly; else e1.PolygonB = poly;
            if (e2.PolygonA == null) e2.PolygonA = poly; else e2.PolygonB = poly;
            if (e3.PolygonA == null) e3.PolygonA = poly; else e3.PolygonB = poly;

            mesh.Polygons.Add(poly);
            return mesh;
        }
    }
}

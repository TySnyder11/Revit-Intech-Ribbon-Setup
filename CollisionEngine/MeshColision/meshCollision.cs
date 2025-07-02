using System;
using System.Collections.Generic;
using System.Numerics;
using Autodesk.Revit.DB;

namespace Intech.Geometry.Collision
{
    internal class MeshCollision
    {
        public class Triangle
        {
            public Vector3 A, B, C;

            public Triangle(Vector3 a, Vector3 b, Vector3 c)
            {
                A = a; B = b; C = c;
            }
        }

        public class LineSegment
        {
            public Vector3 Start, End;

            public LineSegment(Vector3 start, Vector3 end)
            {
                Start = start;
                End = end;
            }
        }

        public class Mesh
        {
            public Mesh()
            {
                Triangles = new List<Triangle>();
            }
            public Mesh(Autodesk.Revit.DB.Mesh mesh)
            {
                Triangles = new List<Triangle>();
                for (int i = 0; i < mesh.NumTriangles; i++)
                {
                    var tri = mesh.get_Triangle(i);
                    Triangles.Add(new Triangle(
                        ToVector3(tri.get_Vertex(0)),
                        ToVector3(tri.get_Vertex(1)),
                        ToVector3(tri.get_Vertex(2))
                    ));
                }
            }
            public Mesh(List<Triangle> triangles)
            {
                Triangles = triangles;
            }

            public List<Triangle> Triangles = new List<Triangle>();
        }

        public static Mesh GetIntersectionSegments(Autodesk.Revit.DB.Mesh meshA, Autodesk.Revit.DB.Mesh meshB)
        {
            var result = new List<Triangle>();

            for (int i = 0; i < meshA.NumTriangles; i++)
            {
                var triA = meshA.get_Triangle(i);
                var triangleA = new Triangle(
                    ToVector3(triA.get_Vertex(0)),
                    ToVector3(triA.get_Vertex(1)),
                    ToVector3(triA.get_Vertex(2))
                );

                for (int j = 0; j < meshB.NumTriangles; j++)
                {
                    var triB = meshB.get_Triangle(j);
                    var triangleB = new Triangle(
                        ToVector3(triB.get_Vertex(0)),
                        ToVector3(triB.get_Vertex(1)),
                        ToVector3(triB.get_Vertex(2))
                    );

                    if (TrianglesIntersect(triangleA, triangleB, out var segments))
                    {
                        
                    }
                }
            }

            return new Mesh(result);
        }

        private static Vector3 ToVector3(XYZ pt) =>
            new Vector3((float)pt.X, (float)pt.Y, (float)pt.Z);

        public static bool TrianglesIntersect(Triangle triA, Triangle triB, out List<LineSegment> intersectionSegments)
        {
            intersectionSegments = new List<LineSegment>();

            Vector3 nA = Vector3.Cross(triA.B - triA.A, triA.C - triA.A);
            Vector3 nB = Vector3.Cross(triB.B - triB.A, triB.C - triB.A);

            Vector3 D = Vector3.Cross(nA, nB);
            if (D.LengthSquared() < 1e-6f)
            {
                // Coplanar case — not handled here
                return false;
            }

            Vector3 linePoint = FindPlaneIntersectionPoint(triA.A, nA, triB.A, nB);
            if (linePoint == Vector3.Zero && D == Vector3.Zero)
            {
                return false;
            }

            float[] projA = ProjectTriangleOntoLine(triA, linePoint, D);
            float[] projB = ProjectTriangleOntoLine(triB, linePoint, D);

            float minA = (float)Math.Min(projA[0], Math.Min(projA[1], projA[2]));
            float maxA = (float)Math.Max(projA[0], Math.Max(projA[1], projA[2]));
            float minB = (float)Math.Min(projB[0], Math.Min(projB[1], projB[2]));
            float maxB = (float)Math.Max(projB[0], Math.Max(projB[1], projB[2]));

            float tMin = (float)Math.Max(minA, minB);
            float tMax = (float)Math.Min(maxA, maxB);

            if (tMin > tMax)
                return false;

            Vector3 p1 = linePoint + tMin * Vector3.Normalize(D);
            Vector3 p2 = linePoint + tMax * Vector3.Normalize(D);

            intersectionSegments.Add(new LineSegment(p1, p2));
            return true;
        }

        private static float[] ProjectTriangleOntoLine(Triangle tri, Vector3 linePoint, Vector3 lineDir)
        {
            Vector3 d = Vector3.Normalize(lineDir);
            return new float[]
            {
                Vector3.Dot(tri.A - linePoint, d),
                Vector3.Dot(tri.B - linePoint, d),
                Vector3.Dot(tri.C - linePoint, d)
            };
        }

        private static Vector3 FindPlaneIntersectionPoint(Vector3 p1, Vector3 n1, Vector3 p2, Vector3 n2)
        {
            Vector3 direction = Vector3.Cross(n1, n2);
            float denom = direction.LengthSquared();
            if (denom < 1e-6f)
                return Vector3.Zero;

            float d1 = -Vector3.Dot(n1, p1);
            float d2 = -Vector3.Dot(n2, p2);

            Vector3 point = ((d2 * Vector3.Cross(n1, direction)) - (d1 * Vector3.Cross(n2, direction))) / denom;
            return point;
        }
    }
}

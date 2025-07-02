using System.Collections.Generic;
using System.Numerics;
namespace Intech.Geometry.Collision
{
    public static class PolygonClipper
    {
        public static List<(Vector2, Vector2)> Clip(List<Vector2> subject, List<Vector2> clip)
        {
            var output = new List<Vector2>(subject);

            for (int i = 0; i < clip.Count; i++)
            {
                var input = new List<Vector2>(output);
                output.Clear();

                Vector2 A = clip[i];
                Vector2 B = clip[(i + 1) % clip.Count];

                for (int j = 0; j < input.Count; j++)
                {
                    Vector2 P = input[j];
                    Vector2 Q = input[(j + 1) % input.Count];

                    bool PInside = IsInside(A, B, P);
                    bool QInside = IsInside(A, B, Q);

                    if (PInside && QInside)
                    {
                        output.Add(Q);
                    }
                    else if (PInside && !QInside)
                    {
                        output.Add(Intersect(A, B, P, Q));
                    }
                    else if (!PInside && QInside)
                    {
                        output.Add(Intersect(A, B, P, Q));
                        output.Add(Q);
                    }
                }
            }

            var segments = new List<(Vector2, Vector2)>();
            for (int i = 0; i < output.Count; i++)
            {
                segments.Add((output[i], output[(i + 1) % output.Count]));
            }

            return segments;
        }

        private static bool IsInside(Vector2 A, Vector2 B, Vector2 P)
        {
            return (B.X - A.X) * (P.Y - A.Y) - (B.Y - A.Y) * (P.X - A.X) >= 0;
        }

        private static Vector2 Intersect(Vector2 A, Vector2 B, Vector2 P, Vector2 Q)
        {
            Vector2 AB = B - A;
            Vector2 PQ = Q - P;
            float cross = AB.X * PQ.Y - AB.Y * PQ.X;

            if (cross == 0) return P; // Parallel

            Vector2 AP = P - A;
            float t = (AP.X * PQ.Y - AP.Y * PQ.X) / cross;
            return A + t * AB;
        }
    }
}

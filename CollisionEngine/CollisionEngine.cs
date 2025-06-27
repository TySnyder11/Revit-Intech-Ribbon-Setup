using Autodesk.Revit.DB;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Geometry.Collision
{
    public class Engine
    {

        public class CollisionEngine
        {
            public delegate void CollisionHandler(CollisionResult result);

            public void Run(
            IEnumerable<GeometryData> groupA,
            IEnumerable<GeometryData> groupB,
            double cellSize,
            double overlap,
            CollisionHandler onCollision)
            {
                var partitions = new ConcurrentDictionary<(int x, int y, int z), Partition>();

                // Step 2: Assign geometry to partitions (multithreaded)
                Parallel.ForEach(groupA.Concat(groupB), record =>
                {
                    var inflated = InflateBoundingBox(record.BoundingBox, overlap);
                    var min = GetCellIndex(inflated.Min, cellSize);
                    var max = GetCellIndex(inflated.Max, cellSize);

                    for (int x = min.x; x <= max.x; x++)
                        for (int y = min.y; y <= max.y; y++)
                            for (int z = min.z; z <= max.z; z++)
                            {
                                var key = (x, y, z);
                                var partition = partitions.GetOrAdd(key, _ => new Partition());
                                lock (partition) partition.Add(record);
                            }
                });

                // Step 3–4: Process partitions (multithreaded)
                Parallel.ForEach(partitions.Values, partition =>
                {
                    if (!partition.ShouldProcess()) return;

                    foreach (var a in partition.GroupA)
                        foreach (var b in partition.GroupB)
                        {
                            if (!BoxesIntersect(a.BoundingBox,b.BoundingBox)) continue;

                            Solid intersection = null;
                            intersection = BooleanOperationsUtils.ExecuteBooleanOperation(
                                 a.Solid,
                                 b.Solid,
                                 BooleanOperationsType.Intersect
                                );
                            if (intersection != null && intersection.Volume > 1e-6)
                            {
                                var result = new CollisionResult { A = a, B = b, Intersection = intersection };
                                onCollision(result);
                            }
                        }
                });
            }

            bool BoxesIntersect(BoundingBoxXYZ a, BoundingBoxXYZ b)
            {
                return !(a.Max.X < b.Min.X || a.Min.X > b.Max.X ||
                a.Max.Y < b.Min.Y || a.Min.Y > b.Max.Y ||
                a.Max.Z < b.Min.Z || a.Min.Z > b.Max.Z);
            }

            public BoundingBoxXYZ InflateBoundingBox(BoundingBoxXYZ box, double overlap)
            {
                XYZ min = box.Min;
                XYZ max = box.Max;

                return new BoundingBoxXYZ
                {
                    Min = new XYZ(min.X - overlap, min.Y - overlap, min.Z - overlap),
                    Max = new XYZ(max.X + overlap, max.Y + overlap, max.Z + overlap)
                };
            }

            public (int x, int y, int z) GetCellIndex(XYZ point, double cellSize)
            {
                return (
                    (int)Math.Floor(point.X / cellSize),
                    (int)Math.Floor(point.Y / cellSize),
                    (int)Math.Floor(point.Z / cellSize)
                );
            }

        }
    }
}

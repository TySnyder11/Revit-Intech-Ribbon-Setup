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
                var processedPairs = new ConcurrentDictionary<(ElementId, ElementId), byte>();
                var results = new ConcurrentBag<CollisionResult>();

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
                                lock (partition)
                                    partition.Add(record);
                            }
                });

                Parallel.ForEach(partitions.Values, partition =>
                {
                    if (!partition.ShouldProcess()) return;

                    foreach (var a in partition.GroupA)
                    {
                        foreach (var b in partition.GroupB)
                        {
                            if (!BoxesIntersect(a.BoundingBox, b.BoundingBox)) continue;

                            var key = a.SourceElementId.Value < b.SourceElementId.Value
                            ? (a.SourceElementId, b.SourceElementId)
                            : (b.SourceElementId, a.SourceElementId);

                            if (!processedPairs.TryAdd(key, 0)) continue;

                            Solid intersection = null;
                            try
                            {
                                intersection = BooleanOperationsUtils.ExecuteBooleanOperation(
                                a.Solid,
                                b.Solid,
                                BooleanOperationsType.Intersect);
                            }
                            catch
                            {
                            }

                            if (intersection != null && intersection.Volume > 1e-6)
                            {
                                results.Add(new CollisionResult { A = a, B = b, Intersection = intersection });
                            }
                        }
                    }
                });

                foreach (var result in results)
                {
                    onCollision(result);
                }
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

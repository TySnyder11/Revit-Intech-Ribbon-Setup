using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Geometry
{
    internal class Partition
    {
        public List<GeometryData> GroupA { get; } = new List<GeometryData>();
        public List<GeometryData> GroupB { get; } = new List<GeometryData>();

        public void Add(GeometryData record)
        {
            if (record.Role == GeometryRole.A) GroupA.Add(record);
            else GroupB.Add(record);
        }

        public bool ShouldProcess() => GroupA.Count > 0 && GroupB.Count > 0;

    }
}

using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Geometry
{
    public class GeometryData
    {

        public Solid Solid { get; set; }
        public BoundingBoxXYZ BoundingBox { get; set; }
        public ElementId SourceElementId { get; set; }
        public GeometryRole Role { get; set; } // A or B
    }
    public enum GeometryRole { A, B }
}

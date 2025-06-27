using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Geometry.Collision
{
    public class CollisionResult
    {

        public GeometryData A { get; set; }
        public GeometryData B { get; set; }
        public Solid Intersection
        {
            get; set;

        }
    }
}

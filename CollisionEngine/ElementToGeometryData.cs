using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Intech.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech.Geometry
{
    internal class ElementToGeometryData
    {
        public static List<GeometryData> ConvertMEPToGeometryData(List<Reference> references, Document doc)
        {
            List<GeometryData> geometryDataList = new List<GeometryData>();

            foreach (Reference reference in references)
            {
                List<Solid> solids = new List<Solid>();
                Element element = doc.GetElement(reference);
                if (element == null)
                    continue;

                // Filter: Pipe, Duct, MEPCurve, FamilyInstance with MEPModel, or FabricationPart
                bool isValidMEP =
                    element is Pipe ||
                    element is Duct ||
                    element is MEPCurve ||
                    element is FabricationPart;

                if (!isValidMEP)
                    continue;

                if (element is FabricationPart fab)
                {
                    Options options = new Options
                    {
                        ComputeReferences = true,
                        IncludeNonVisibleObjects = true,
                        DetailLevel = ViewDetailLevel.Fine
                    };
                    Mesh test = null;
                    GeometryElement geomElement = element.get_Geometry(options);
                    foreach (GeometryObject obj in geomElement)
                    {
                        if (obj is Mesh mesh)
                        {
                            // Store mesh or convert to bounding volume
                            test = mesh;
                        }
                        else if (obj is GeometryInstance inst)
                        {
                            foreach (GeometryObject instObj in inst.GetInstanceGeometry())
                            {
                                if (instObj is Mesh instMesh)
                                {
                                    // Store mesh or convert to bounding volume
                                    test = instMesh;
                                }
                            }
                        }
                    }

                }
                else
                {
                    // Use standard get_Geometry for other MEP elements
                    Options options = new Options
                    {
                        ComputeReferences = true,
                        IncludeNonVisibleObjects = true,
                        DetailLevel = ViewDetailLevel.Fine
                    };

                    GeometryElement geomElement = element.get_Geometry(options);
                    if (geomElement != null)
                    {
                        foreach (GeometryObject obj in geomElement)
                        {
                            if (obj is Solid solid && solid.Volume > 0)
                                solids.Add(solid);
                            else if (obj is GeometryInstance inst)
                            {
                                foreach (GeometryObject instObj in inst.GetInstanceGeometry())
                                {
                                    if (instObj is Solid instSolid && instSolid.Volume > 0)
                                        solids.Add(instSolid);
                                }
                            }
                        }
                    }
                }
                foreach (Solid solid in solids)
                {
                    geometryDataList.Add(new GeometryData
                    {
                        Solid = solid,
                        BoundingBox = element.get_BoundingBox(null),
                        SourceElementId = element.Id,
                        Role = GeometryRole.A
                    });
                }
            }

            return geometryDataList;
        }
    }
}

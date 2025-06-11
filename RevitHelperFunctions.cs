using Autodesk.Revit.DB;
using OfficeOpenXml.Export.ToDataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Revit
{
    internal class RevitHelperFunctions
    {
        static Document doc;

        static public void init(Document document)
        {
            // Constructor logic if needed
            doc = document;
        }

        static public CategoryNameMap GetAllCategories()
        {
            // Access all categories in the document
            CategoryNameMap categories = doc.Settings.Categories;
            return categories;
        }

        //get parameters of element
        static public ParameterMap GetParameters(Element e)
        {
            ParameterMap pMap = e.ParametersMap;
            return pMap;
        }

        static public List<string> GetParameters(Category category)
        {

            var instances = new FilteredElementCollector(doc)
             .OfCategoryId(category.Id)
             .WhereElementIsNotElementType()
             .ToElements();

            // Step 2: Map from typeId to one instance
            Dictionary<ElementId, Element> typeToInstance = new Dictionary<ElementId, Element>();

            foreach (Element instance in instances)
            {
                ElementId typeId = instance.GetTypeId();
                if (!typeToInstance.ContainsKey(typeId))
                {
                    typeToInstance[typeId] = instance;
                }
            }

            // Step 3: Collect unique parameter names from one instance per type
            HashSet<string> paramNames = new HashSet<string>();

            foreach (Element instance in typeToInstance.Values)
            {
                foreach (Parameter p in instance.Parameters)
                {
                    paramNames.Add(p.Definition.Name);
                }
            }

            return paramNames.ToList();

        }

        static public Parameter GetParameters(Family family)
        {
            return null;
        }

        static public Parameter GetParameter(Category category, string paramName)
        {
            ElementCategoryFilter filter = new ElementCategoryFilter(category.Id);
            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .WherePasses(filter);
            if (collector.Count() == 0)
            {
                return null;
            }
            Element e = collector.First<Element>();
            Parameter param = null;
            foreach (Parameter def in GetParameters(e))
            {
                if (def.Definition.Name.ToLower().Equals(paramName.ToLower()))
                {
                    param = def;
                }
            }
            return param;
        }

        static public void GetUnit(Category category, string paramName, out string name, out ForgeTypeId unitID, out ForgeTypeId specTypeId)
        {
            unitID = null;
            name = null;
            Parameter param = GetParameter(category, paramName);
            specTypeId = param.Definition.GetDataType();
            if (param != null && param.StorageType == StorageType.Double)
            {
                unitID = param.GetUnitTypeId();
                name = LabelUtils.GetLabelForUnit(unitID);
            }
            return;
        }

        static public void GetUnit(Category category, string paramName, out string name, out ForgeTypeId unitID)
        {
            unitID = null;
            name = null;
            Parameter param = GetParameter(category, paramName);
            if (param != null && param.StorageType == StorageType.Double)
            {
                unitID = param.GetUnitTypeId();
                name = LabelUtils.GetLabelForUnit(unitID);
            }
            return;
        }

        static public ForgeTypeId GetUnit(string unitName)
        {

            var allUnits = UnitUtils.GetAllUnits();

            foreach (var unitId in allUnits)
            {
                string displayName = LabelUtils.GetLabelForUnit(unitId);

                if (string.Equals(displayName, unitName, StringComparison.OrdinalIgnoreCase))
                {
                    return unitId;
                }
            }
            return null;
        }


        static public List<Element> GetElementsOfCategory(Category category)
        {
            ElementCategoryFilter filter = new ElementCategoryFilter(category.Id);
            FilteredElementCollector collector = new FilteredElementCollector(doc)
            .WhereElementIsNotElementType()
            .WherePasses(filter);
            return collector.ToList();
        }


        static public List<Element> GetElementTypesOfCategory(Category category)
        {
            ElementCategoryFilter filter = new ElementCategoryFilter(category.Id);
            FilteredElementCollector collector = new FilteredElementCollector(doc)
            .WhereElementIsElementType()
            .WherePasses(filter);
            return collector.ToList();
        }


        static public string GetParameterValueAsString(Element element, string paramName)
        {
            Parameter param = element.LookupParameter(paramName);
            if (param == null) return null;

            switch (param.StorageType)
            {
                case StorageType.String:
                    return param.AsString();
                case StorageType.Double:
                    return param.AsValueString();
                case StorageType.Integer:
                    return param.AsInteger().ToString();
                case StorageType.ElementId:
                    ElementId id = param.AsElementId();
                    Element e = doc.GetElement(id);
                    return e?.Name ?? id.ToString();
                default:
                    return null;
            }
        }


    }
}

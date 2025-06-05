using Autodesk.Revit.DB;
using OfficeOpenXml.Export.ToDataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech
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
            ElementCategoryFilter filter = new ElementCategoryFilter(category.Id);
            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .WherePasses(filter);
            if (collector.Count() == 0)
            {
                return new List<string>();
            }
            Element e = collector.First<Element>();
            List<string> paramNames = new List<string>();
            foreach (Parameter def in GetParameters(e))
            {
                paramNames.Add(def.Definition.Name);
            }
            return paramNames;
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


    }
}

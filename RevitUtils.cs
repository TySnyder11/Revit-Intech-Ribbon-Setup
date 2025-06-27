using Autodesk.Private.Windows.ToolBars;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;
using OfficeOpenXml.Export.ToDataTable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech.Revit
{
    internal class RevitUtils
    {
        static Document _doc;
        static public void init(Document document)
        {
            // Constructor logic if needed
            _doc = document;
        }

        static public CategoryNameMap GetAllCategories()
        {
            // Access all categories in the document
            CategoryNameMap categories = _doc.Settings.Categories;
            return categories;
        }

        static public ParameterMap GetParameters(Element e)
        {
            ParameterMap pMap = e.ParametersMap;
            return pMap;
        }

        static public List<string> GetParameters(Category category)
        {

            var instances = new FilteredElementCollector(_doc)
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
            FilteredElementCollector collector = new FilteredElementCollector(_doc)
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
            FilteredElementCollector collector = new FilteredElementCollector(_doc)
            .WhereElementIsNotElementType()
            .WherePasses(filter);
            return collector.ToList();
        }

        static public List<Element> GetElementTypesOfCategory(Category category)
        {
            ElementCategoryFilter filter = new ElementCategoryFilter(category.Id);
            FilteredElementCollector collector = new FilteredElementCollector(_doc)
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
                    Element e = _doc.GetElement(id);
                    return e?.Name ?? id.ToString();
                default:
                    return null;
            }
        }

        public static List<Family> GetFamilies()
        {
            return new FilteredElementCollector(_doc)
            .OfClass(typeof(Family))
            .Cast<Family>()
            .ToList();
        }

        static public List<FamilySymbol> GetFamilySymbols(Family family)
        {
            if (family == null) return new List<FamilySymbol>();
            List<ElementId> fSd = family.GetFamilySymbolIds().ToList();
            List<FamilySymbol> symbols = new List<FamilySymbol>();
            foreach (ElementId familySymbolId in fSd)
            {
                FamilySymbol familySymbol = _doc.GetElement(familySymbolId) as FamilySymbol;
                symbols.Add(familySymbol);
            }
            return symbols;
        }

        static public Dictionary<Family, List<FamilySymbol>> GetFamilySymbolMap()
        {
            Dictionary<Family, List<FamilySymbol>> familySymbols = new Dictionary<Family, List<FamilySymbol>>();
            List<Family> families = GetFamilies();
            foreach (Family family in families)
            {
                List<FamilySymbol> symbols = GetFamilySymbols(family);
                if (symbols.Count > 0)
                {
                    familySymbols[family] = symbols;
                }
            }
            return familySymbols;
        }

        public static DefinitionGroups GetDefinitionGroups()
        {
            Autodesk.Revit.ApplicationServices.Application app = _doc.Application;
            // Ensure the shared parameter file is set
            string sharedParamFile = app.SharedParametersFilename;
            if (string.IsNullOrEmpty(sharedParamFile))
            {
                TaskDialog.Show("Error", "Shared parameter file path is not set.");
                return null;
            }

            // Open the shared parameter file
            DefinitionFile defFile = app.OpenSharedParameterFile();
            if (defFile == null)
            {
                TaskDialog.Show("Error", "Failed to open shared parameter file.");
                return null;
            }

            // Collect all definition groups
            DefinitionGroups groups = defFile.Groups;
            return groups;
        }

        public static List<Definition> GetSharedParameterDefinitions(DefinitionGroup group)
        {
            List<Definition> definitions = new List<Definition>();

            if (group == null)
                return definitions;

            foreach (Definition def in group.Definitions)
            {
                definitions.Add(def);
            }

            return definitions;
        }

        public void AddSharedParameterToFamily(Family family, Definition definition, ForgeTypeId group, bool isInstance)
        {
            Document famDoc = _doc.EditFamily(family);
            FamilyManager famMgr = famDoc.FamilyManager;

            using (Transaction t = new Transaction(famDoc, "Add Shared Parameter"))
            {
                t.Start();

                // Check if the parameter already exists
                bool exists = famMgr.Parameters.Cast<FamilyParameter>().Any(p => p.Definition.Name == definition.Name);
                if (!exists)
                {

                    ExternalDefinition extDef = definition as ExternalDefinition;
                    famMgr.AddParameter(extDef, group, isInstance);
                }

                t.Commit();
            }

            using (Transaction t = new Transaction(_doc, "Save family changes to project"))
            {
                t.Start();
                famDoc.LoadFamily(_doc);
                t.Commit();
            }
        }

        public static void AddSharedParametersToFamily(
            Document doc,
            Family family,
            List<Definition> definitions,
            ForgeTypeId group,
            bool isInstance,
            bool makeReporting)
        {
            Document famDoc = doc.EditFamily(family);
            FamilyManager famMgr = famDoc.FamilyManager;

            using (Transaction t = new Transaction(famDoc, "Add Shared Parameters"))
            {
                t.Start();

                foreach (Definition definition in definitions)
                {
                    bool exists = famMgr.Parameters.Cast<FamilyParameter>().Any(p => p.Definition.Name == definition.Name);
                    if (!exists)
                    {
                        ExternalDefinition extDef = definition as ExternalDefinition;
                        if (extDef == null)
                        {
                            TaskDialog.Show("Error", $"Definition '{definition.Name}' is not an ExternalDefinition.");
                            continue;
                        }

                        FamilyParameter param = famMgr.AddParameter(extDef, group, isInstance);

                        try
                        {
                            ForgeTypeId dataType = extDef.GetDataType();

                            if (makeReporting && dataType == SpecTypeId.Length && isInstance)
                            {
                                famMgr.MakeReporting(param);
                            }
                        }
                        catch (Exception ex)
                        {
                            TaskDialog.Show("Reporting Parameter Error", $"Could not make '{definition.Name}' a reporting parameter: {ex.Message}");
                        }
                    }
                }

                t.Commit();
            }

            using (Transaction t = new Transaction(doc, "Load Family into Project"))
            {
                t.Start();
                famDoc.LoadFamily(doc);
                t.Commit();
            }
        }

        public static void AddSharedParametersToFamilies(List<Family> families, List<Definition> definitions, ForgeTypeId group, bool isInstance)
        {
            List<Document> familyDocs = new List<Document>();
            string path = string.Empty;

            foreach (Family fam in families)
            {
                Document famDoc = _doc.EditFamily(fam);
                FamilyManager famMgr = famDoc.FamilyManager;

                using (Transaction t = new Transaction(famDoc, "Add Shared Parameter"))
                {
                    t.Start();
                    foreach (Definition definition in definitions)
                    {
                        if (definition is ExternalDefinition extDef)
                        {
                            bool exists = famMgr.Parameters.Cast<FamilyParameter>().Any(p => p.Definition.Name == extDef.Name);
                            if (!exists)
                            {
                                famMgr.AddParameter(extDef, group, isInstance);
                            }
                        }
                        else
                        {
                            TaskDialog.Show("Error", $"Definition {definition.Name} is not an ExternalDefinition.");
                        }
                    }
                    t.Commit();
                }

                string savePath = famDoc.PathName;
                if (string.IsNullOrEmpty(savePath))
                {
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        using (SaveFileDialog saveDialog = new SaveFileDialog())
                        {
                            saveDialog.Title = "Save Family As";
                            saveDialog.Filter = "Revit Family (*.rfa)|*.rfa";
                            saveDialog.FileName = famDoc.Title;
                            DialogResult result = saveDialog.ShowDialog();
                            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(saveDialog.FileName))
                            {
                                path = Path.GetDirectoryName(saveDialog.FileName);
                            }
                            else
                            {
                                TaskDialog.Show("Cancelled", "No file selected. Operation aborted.");
                                famDoc.Close(false);
                                return;
                            }
                        }
                    }
                    savePath = path;
                }
                else
                {
                    savePath = Path.GetDirectoryName(savePath);
                }

                    SaveAsOptions saveOptions = new SaveAsOptions { OverwriteExistingFile = true };
                famDoc.SaveAs(Path.Combine(savePath, famDoc.Title), saveOptions);

                familyDocs.Add(famDoc); // Keep open for reloading
            }

            // Now reload families into the project
            OverwriteFamilyLoadOptions loadOptions = new OverwriteFamilyLoadOptions();

            foreach (Document famDoc in familyDocs)
            {
                famDoc.LoadFamily(_doc, loadOptions);
                famDoc.Close(false);
            }
        }

        public static void AddSharedParametersToFamiliesNoSave(List<Family> families, List<Definition> definitions, ForgeTypeId group, bool isInstance)
        {
            List<Document> familyDocs = new List<Document>();
            string path = string.Empty;

            foreach (Family fam in families)
            {
                Document famDoc = _doc.EditFamily(fam);
                FamilyManager famMgr = famDoc.FamilyManager;

                using (Transaction t = new Transaction(famDoc, "Add Shared Parameter"))
                {
                    t.Start();
                    foreach (Definition definition in definitions)
                    {
                        if (definition is ExternalDefinition extDef)
                        {
                            bool exists = famMgr.Parameters.Cast<FamilyParameter>().Any(p => p.Definition.Name == extDef.Name);
                            if (!exists)
                            {
                                famMgr.AddParameter(extDef, group, isInstance);
                            }
                        }
                        else
                        {
                            TaskDialog.Show("Error", $"Definition {definition.Name} is not an ExternalDefinition.");
                        }
                    }
                    t.Commit();
                }

                familyDocs.Add(famDoc); // Keep open for reloading
            }

            // Now reload families into the project
            OverwriteFamilyLoadOptions loadOptions = new OverwriteFamilyLoadOptions();

            foreach (Document famDoc in familyDocs)
            {
                famDoc.LoadFamily(_doc, loadOptions);
                famDoc.Close(false);
            }
        }

        public class OverwriteFamilyLoadOptions : IFamilyLoadOptions
        {
            public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
            {
                overwriteParameterValues = true; // Overwrite existing parameter values
                return true; // Always overwrite the family
            }

            public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
            {
                source = FamilySource.Project;
                overwriteParameterValues = true;
                return true;
            }
        }

        public static List<ForgeTypeId> GetAllGroupTypeIds()
        {
            List<ForgeTypeId> groupTypeIds = new List<ForgeTypeId>();

            PropertyInfo[] properties = typeof(GroupTypeId).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(ForgeTypeId))
                {
                    ForgeTypeId id = prop.GetValue(null) as ForgeTypeId;
                    if (id != null && id.TypeId.StartsWith("autodesk.parameter.group"))
                    {
                        groupTypeIds.Add(id);
                    }
                }

            }

            return groupTypeIds;
        }

        public static List<ForgeTypeId> GetAllPossibleGroupTypeIds()
        {
            List<ForgeTypeId> groupTypeIds = new List<ForgeTypeId>();

            PropertyInfo[] properties = typeof(GroupTypeId).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo prop in properties)
            {
                if (prop.PropertyType == typeof(ForgeTypeId))
                {
                    ForgeTypeId id = prop.GetValue(null) as ForgeTypeId;

                    // Optional: filter to only those that look like parameter groups
                    if (id != null && id.TypeId.StartsWith("autodesk.parameter.group"))
                    {
                        groupTypeIds.Add(id);
                    }
                }
            }

            return groupTypeIds;
        }

        public static List<string> GetCommonParameters(List<Family> families)
        {
            if (families == null || families.Count == 0)
                return new List<string>();

            // Initialize with parameters from the first family
            HashSet<string> commonParams = GetParameterNamesFromFamily(families[0]);

            // Intersect with the rest
            for (int i = 1; i < families.Count; i++)
            {
                var currentParams = GetParameterNamesFromFamily(families[i]);
                commonParams.IntersectWith(currentParams);
            }

            return commonParams.ToList();
        }

        private static HashSet<string> GetParameterNamesFromFamily(Family family)
        {
            HashSet<string> paramNames = new HashSet<string>();

            // Get all symbols (types) of the family
            foreach (ElementId symbolId in family.GetFamilySymbolIds())
            {
                FamilySymbol symbol = _doc.GetElement(symbolId) as FamilySymbol;
                if (symbol == null) continue;

                foreach (Parameter param in symbol.Parameters)
                {
                    if (param != null)
                        paramNames.Add(param.Definition.Name);
                }
            }

            return paramNames;
        }

        public static List<string> GetCommonSharedParametersFromFamilies(List<Family> families)
        {
            HashSet<string> commonParams = null;

            foreach (Family family in families)
            {
                // Get one symbol from the family
                ElementId symbolId = family.GetFamilySymbolIds().FirstOrDefault();
                if (symbolId == null) continue;

                FamilySymbol symbol = family.Document.GetElement(symbolId) as FamilySymbol;
                if (symbol == null) continue;

                var sharedParamNames = symbol.Parameters
                .Cast<Parameter>()
                .Where(p => p.IsShared)
                .Select(p => p.Definition.Name)
                .ToHashSet();

                if (commonParams == null)
                {
                    commonParams = sharedParamNames;
                }
                else
                {
                    commonParams.IntersectWith(sharedParamNames);
                }
            }

            return commonParams?.ToList() ?? new List<string>();
        }

        public static HashSet<string> GetCommonFormulaUsableParameters(List<Family> families)
        {
            HashSet<string> commonParams = null;

            foreach (Family family in families)
            {
                using (Document famDoc = _doc.EditFamily(family))
                {
                    FamilyManager famMgr = famDoc.FamilyManager;
                    var usableParams = new HashSet<string>();

                    foreach (FamilyParameter param in famMgr.Parameters)
                    {
                        if (IsFormulaUsable(param))
                        {
                            usableParams.Add(param.Definition.Name);
                        }
                    }

                    if (commonParams == null)
                    {
                        commonParams = usableParams;
                    }
                    else
                    {
                        commonParams.IntersectWith(usableParams);
                    }

                    famDoc.Close(false); // Don't save changes
                }
            }

            return commonParams ?? new HashSet<string>();
        }

        public static bool IsFormulaUsable(FamilyParameter param)
        {
            if (param == null || param.Definition == null)
                return false;

            ForgeTypeId dataType = param.Definition.GetDataType();

            bool isDoubleBacked = param.StorageType == StorageType.Double && param.GetUnitTypeId() != null;
            bool isOtherUsableType =
            dataType == SpecTypeId.Boolean.YesNo ||
            dataType == SpecTypeId.Int.Integer ||
            dataType == SpecTypeId.String.Text;

            return isDoubleBacked || isOtherUsableType;
        }

        public static bool IsFormulaValid(Family family, string formula, out string errorMessage)
        {
            errorMessage = null;

            Document familyDoc = _doc.EditFamily(family);
            if (familyDoc == null || !familyDoc.IsFamilyDocument)
            {
                errorMessage = "Unable to open family document.";
                return false;
            }

            FamilyManager famMgr = familyDoc.FamilyManager;
            FamilyParameter tempParam = null;
            bool isValid = false;

            Transaction tx = new Transaction(familyDoc, "Validate Formula");

            try
            {
                tx.Start();

                ForgeTypeId groupId = GroupTypeId.Constraints;
                ForgeTypeId specId = SpecTypeId.Length;

                tempParam = famMgr.AddParameter("__FormulaTestParam", groupId, specId, true);
                famMgr.SetFormula(tempParam, formula);

                // If we got here, the formula is valid
                isValid = true;

                // Clean up
                famMgr.RemoveParameter(tempParam);
                tx.RollBack(); // Discard all changes
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                // If the parameter was added before the failure, try to remove it before rollback
                if (tx.HasStarted() && tempParam != null)
                {
                    try { famMgr.RemoveParameter(tempParam); } catch { /* ignore */ }
                }

                if (tx.HasStarted())
                {
                    tx.RollBack(); // Safely rollback if started
                }
            }
            finally
            {
                if (familyDoc.IsModifiable)
                {
                    // Just in case, ensure no lingering transaction
                    if (tx.GetStatus() == TransactionStatus.Started)
                        tx.RollBack();
                }

                familyDoc.Close(false); // Always close without saving
            }

            return isValid;
        }

        public static HashSet<string> GetCommonFormulaHostableParameters(List<Family> families)
        {
            HashSet<string> commonParams = null;

            foreach (Family family in families)
            {
                using (Document famDoc = _doc.EditFamily(family))
                {
                    FamilyManager famMgr = famDoc.FamilyManager;
                    var hostableParams = new HashSet<string>();

                    foreach (FamilyParameter param in famMgr.Parameters)
                    {
                        if (IsFormulaHostable(param))
                        {
                            hostableParams.Add(param.Definition.Name);
                        }
                    }

                    if (commonParams == null)
                    {
                        commonParams = hostableParams;
                    }
                    else
                    {
                        commonParams.IntersectWith(hostableParams);
                    }

                    famDoc.Close(false); // Don't save changes
                }
            }

            return commonParams ?? new HashSet<string>();
        }

        public static bool IsFormulaHostable(FamilyParameter param)
        {
            if (param == null || param.Definition == null)
                return false;

            if (param.IsReadOnly || param.IsReporting)
                return false;

            ForgeTypeId dataType = param.Definition.GetDataType();

            bool isDoubleBacked = param.StorageType == StorageType.Double && param.GetUnitTypeId() != null;
            bool isOtherHostableType =
            dataType == SpecTypeId.Boolean.YesNo ||
            dataType == SpecTypeId.Int.Integer ||
            dataType == SpecTypeId.String.Text;

            return isDoubleBacked || isOtherHostableType;
        }

        public static void SetFormulaForParameterInFamilies(
         List<Family> families,
         string parameterName,
         string formula)
        {
            foreach (Family family in families)
            {
                using (Document famDoc = _doc.EditFamily(family))
                {
                    FamilyManager famMgr = famDoc.FamilyManager;
                    FamilyParameter param = famMgr.get_Parameter(parameterName);

                    if (param == null)
                    {
                        TaskDialog.Show("Parameter Not Found", $"'{parameterName}' not found in family '{family.Name}'.");
                        continue;
                    }

                    if (!IsFormulaHostable(param))
                    {
                        TaskDialog.Show("Not Formula Hostable", $"Parameter '{parameterName}' in family '{family.Name}' is not formula-hostable.");
                        continue;
                    }

                    using (Transaction tx = new Transaction(famDoc, "Set Formula"))
                    {
                        tx.Start();
                        famMgr.SetFormula(param, formula);
                        tx.Commit();
                    }

                    famDoc.LoadFamily(_doc, new FamilyLoadOptions());
                    famDoc.Close(false); // Don't save to disk
                }
            }
        }

        public class FamilyLoadOptions : IFamilyLoadOptions
        {
            public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
            {
                overwriteParameterValues = true; // or false, depending on your needs
                return true; // reload the family
            }

            public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
            {
                source = FamilySource.Project;
                overwriteParameterValues = true;
                return true;
            }
        }

        public static string projectName()
        {
            return _doc.Title; 
        }


        public static List<Family> GetAllTitleBlockFamilies()
        {
            var collector = new FilteredElementCollector(_doc)
                .OfClass(typeof(Family));

            var titleBlockFamilies = new List<Family>();

            foreach (Family family in collector)
            {
                foreach (ElementId symbolId in family.GetFamilySymbolIds())
                {
                    FamilySymbol symbol = _doc.GetElement(symbolId) as FamilySymbol;
                    if (symbol != null && symbol.Category != null &&
                        symbol.Category.BuiltInCategory == BuiltInCategory.OST_TitleBlocks)
                    {
                        titleBlockFamilies.Add(family);
                        break; // No need to check more symbols in this family
                    }
                }
            }

            return titleBlockFamilies;
        }

        public static List<FamilySymbol> GetTitleBlockTypesFromFamily(Family family)
        {
            var titleBlockTypes = new List<FamilySymbol>();

            foreach (ElementId symbolId in family.GetFamilySymbolIds())
            {
                FamilySymbol symbol = _doc.GetElement(symbolId) as FamilySymbol;
                if (symbol != null && symbol.Category != null &&
                    symbol.Category.BuiltInCategory == BuiltInCategory.OST_TitleBlocks)
                {
                    titleBlockTypes.Add(symbol);
                }
            }

            return titleBlockTypes;
        }

        public static List<FamilySymbol> GetTitleBlockTypesFromFamily(string familyName)
        {
            // Find the Family by name
            Family family = new FilteredElementCollector(_doc)
         .OfClass(typeof(Family))
         .Cast<Family>()
         .FirstOrDefault(fam => fam.Name.Equals(familyName, StringComparison.OrdinalIgnoreCase));

            if (family == null)
            {
                // Optionally log or throw an exception
                return new List<FamilySymbol>();
            }

            return GetTitleBlockTypesFromFamily(family);
        }

        public static List<RevitLinkInstance> GetLinkedModels()
        {
            FilteredElementCollector collector = new FilteredElementCollector(_doc);
            ICollection<Element> linkInstances = collector
            .OfClass(typeof(RevitLinkInstance))
            .ToElements();

            List<RevitLinkInstance> linkedModels = new List<RevitLinkInstance>();

            foreach (Element e in linkInstances)
            {
                RevitLinkInstance linkInstance = e as RevitLinkInstance;
                if (linkInstance != null)
                {
                    linkedModels.Add(linkInstance);
                }
            }

            return linkedModels;
        }
    }
}

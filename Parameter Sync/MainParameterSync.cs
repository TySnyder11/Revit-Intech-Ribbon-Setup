using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ParameterSyncMenu : IExternalCommand
    {
        public static Document doc = null;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Revit.RevitHelperFunctions.init(commandData.Application.ActiveUIDocument.Document);
            SaveFileManager saveFileManager = new SaveFileManager("ParameterSync.txt", new TxtFormat());
            doc = commandData.Application.ActiveUIDocument.Document;
            Revit.RevitHelperFunctions.init(doc);
            ParameterSyncForm parameterSyncForm = new ParameterSyncForm();
            parameterSyncForm.ShowDialog();

            return Result.Succeeded;
        }

        public static string[] inputStringParse(string input)
        {

            var result = new List<string>();
            var pattern = @"(\[[^\]]+\](?:\{[^\}]+\})?)|([^\[\{]+)";
            var matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                if (!string.IsNullOrWhiteSpace(match.Value))
                {
                    result.Add(match.Value.Trim());
                }
            }

            return result.ToArray();
        }

        public static void compute(string input, string category, string outParameter)
        {
            string[] inputParse = inputStringParse(input);
            List<Parameter> parameters = new List<Parameter>();
            CategoryNameMap categories = Revit.RevitHelperFunctions.GetAllCategories();
            if (!categories.Contains(category))
            {
                throw new ArgumentException($"Category '{category}' does not exist in the document.");
            }
            Category cat = categories.get_Item(category);

            Dictionary<Element, string> paramValues = new Dictionary<Element, string>();

            List<Element> elem = Revit.RevitHelperFunctions.GetElementsOfCategory(cat);
            Transaction trans = new Transaction(doc, "Parameter Sync");
            trans.Start();
            try
            {
                if (elem.Count == 0)
                {
                    TaskDialog.Show("Error", $"No elements found in category '{category}'.");
                    return;
                }
                foreach (string parse in inputParse)
                {
                    if (parse.StartsWith("[") && parse.EndsWith("}"))
                    {

                        var match = Regex.Match(input, @"\(.*?)\?");
                        string paramName = string.Empty;
                        string unitName = string.Empty;
                        if (match.Success)
                        {
                            paramName = match.Groups[1].Value;
                            unitName = match.Groups[2].Success ? match.Groups[2].Value : "";
                        }
                        Parameter param = Revit.RevitHelperFunctions.GetParameter(cat, paramName);
                        ForgeTypeId outUnit = Revit.RevitHelperFunctions.GetUnit(unitName);
                        ForgeTypeId inUnit = param.GetUnitTypeId();
                        ForgeTypeId unitType = param.Definition.GetDataType();
                        Units units = doc.GetUnits();
                        FormatOptions origonal = units.GetFormatOptions(unitType);
                        FormatOptions temp = units.GetFormatOptions(unitType);
                        temp.SetUnitTypeId(outUnit);
                        units.SetFormatOptions(unitType, temp);
                        FilteredElementCollector collector = new FilteredElementCollector(doc)
                            .OfCategoryId(cat.Id)
                            .WhereElementIsNotElementType();
                        foreach (Element e in collector)
                        {
                            Parameter p = e.LookupParameter(paramName);
                            double paramVal = p.AsDouble();
                            paramVal = UnitUtils.Convert(paramVal, inUnit, outUnit);
                            String outputString = UnitFormatUtils.Format(units, unitType, paramVal, true);
                            paramValues.Add(e, (paramValues[e] ?? string.Empty) + outputString);
                        }
                        units.SetFormatOptions(unitType, origonal);
                        continue;
                    }
                    else if(parse.StartsWith("[") && parse.EndsWith("]"))
                    {
                        parse.Remove(0, 1);
                        parse.Remove(parse.Length - 1, 1);
                        Units units = doc.GetUnits();
                        foreach (Element e in elem) 
                        {
                            Parameter p = e.LookupParameter(parse);
                            double paramVal = p.AsDouble();
                            String outputString = UnitFormatUtils.Format(units, p.Definition.GetDataType(), paramVal, true);
                            paramValues.Add(e, (paramValues[e] ?? string.Empty) + outputString);
                        }
                    }
                }
                trans.Commit();
            }
            catch (FormatException ex)
            {
                trans.RollBack();
                TaskDialog.Show("Error", $"An error occurred while processing the input: {ex.Message}");
                return;
            }
        }
    }
}

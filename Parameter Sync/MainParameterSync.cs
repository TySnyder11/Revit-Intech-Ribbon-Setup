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

            List<Element> elems = new FilteredElementCollector(doc)
                            .OfCategoryId(cat.Id)
                            .WhereElementIsNotElementType().ToList();
            Transaction trans = new Transaction(doc, "Parameter Sync");
            trans.Start();
            try
            {
                if (elems.Count == 0)
                {
                    TaskDialog.Show("Error", $"No elements found in category '{category}'.");
                    return;
                }
                foreach (string parse in inputParse)
                {
                    if (parse.StartsWith("[") && parse.EndsWith("}"))
                    {


                        int startParam = parse.IndexOf('[') + 1;
                        int endParam = parse.IndexOf(']');
                        string paramName = parse.Substring(startParam, endParam - startParam);

                        int startUnit = parse.IndexOf('{') + 1;
                        int endUnit = parse.IndexOf('}');
                        string unitName = parse.Substring(startUnit, endUnit - startUnit);

                        Parameter param = Revit.RevitHelperFunctions.GetParameter(cat, paramName);
                        ForgeTypeId outUnit = Revit.RevitHelperFunctions.GetUnit(unitName);
                        ForgeTypeId inUnit = param.GetUnitTypeId();
                        ForgeTypeId unitType = param.Definition.GetDataType();
                        Units customUnits = new Units(UnitSystem.Imperial);

                        FormatOptions fo = new FormatOptions(outUnit);
                        if (!unitName.Equals("General"))
                        {
                            fo.Accuracy = 0.001;
                            if (fo.CanSuppressTrailingZeros())
                            {
                                fo.SuppressTrailingZeros = true;
                            }
                            if(LabelUtils.GetLabelForUnit(inUnit).Contains("Fraction"))
                            {
                                fo.Accuracy = (1.0 / 64.0);
                            }
                            fo.UseDefault = false;
                            customUnits.SetFormatOptions(unitType, fo);
                        }

                        List<Element> tempEls = new List<Element>(elems);
                        foreach (Element e in tempEls)
                        {
                            Parameter p = e.LookupParameter(paramName);
                            if (p == null)
                            {
                                foreach (Parameter par in e.Parameters)
                                {
                                    if (par.Definition.Name.ToLower().Equals(paramName.ToLower()))
                                        p = par;
                                }
                            }
                            if(p == null)
                            {
                                elems.Remove(e);
                                paramValues.Remove(e);
                                continue;
                            }
                            double paramVal = p.AsDouble();
                            String outputString = UnitFormatUtils.Format(customUnits, unitType, paramVal, false);
                            if (paramValues.ContainsKey(e))
                            {
                                paramValues[e] += outputString;
                            }
                            else
                            {
                                paramValues.Add(e, outputString);
                            }
                        }
                        continue;
                    }
                    else if(parse.StartsWith("[") && parse.EndsWith("]"))
                    {
                        string paramName = parse.Substring(1, parse.Length - 2);
                        Units units = doc.GetUnits();
                        List<Element> tempEls = new List<Element>(elems);
                        foreach (Element e in tempEls) 
                        {
                            Parameter p = e.LookupParameter(paramName);
                            if (p == null)
                            {
                                foreach (Parameter param in e.Parameters)
                                {
                                    if (param.Definition.Name.ToLower().Equals(paramName.ToLower()))
                                        p = param;
                                }
                            }
                            if (p == null)
                            {
                                elems.Remove(e);
                                paramValues.Remove(e);
                                continue;
                            }
                            if (p != null)
                            {
                                if (p.StorageType == StorageType.Double)
                                {
                                    double paramVal = p.AsDouble();
                                    String outputString = UnitFormatUtils.Format(units, p.Definition.GetDataType(), paramVal, true);

                                    if (paramValues.ContainsKey(e))
                                    {
                                        paramValues[e] += outputString;
                                    }
                                    else
                                    {
                                        paramValues.Add(e, outputString);
                                    }
                                    continue;
                                }
                                else
                                {
                                    string outputString = p.AsValueString();
                                    if (paramValues.ContainsKey(e))
                                    {
                                        paramValues[e] += outputString;
                                    }
                                    else
                                    {
                                        paramValues.Add(e, outputString);
                                    }
                                }
                            }
                        }
                        continue;
                    }
                    else
                    {
                        string outputString = parse;
                        foreach (Element e in elems)
                        {
                            if (paramValues.ContainsKey(e))
                            { 
                                paramValues[e] += outputString;
                            }
                            else
                            {
                                paramValues.Add(e, outputString);
                            }
                        }
                    }
                }
                foreach (Element elem in paramValues.Keys)
                {
                    elem.LookupParameter(outParameter).Set(paramValues[elem]);
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

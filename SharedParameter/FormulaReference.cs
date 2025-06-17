using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitleBlockSetup.SharedParameter
{
    public class RevitFormulaFunction
    {
        public string Name { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
    }

    public static class RevitFormulaFunctionLibrary
    {
        public static readonly List<RevitFormulaFunction> Functions = new List<RevitFormulaFunction>
        {
            new RevitFormulaFunction { Name = "abs", Format = "abs(x)", Description = "Returns the absolute value of x." },
            new RevitFormulaFunction { Name = "acos", Format = "acos(x)", Description = "Returns the arccosine of x (in radians)." },
            new RevitFormulaFunction { Name = "asin", Format = "asin(x)", Description = "Returns the arcsine of x (in radians)." },
            new RevitFormulaFunction { Name = "atan", Format = "atan(x)", Description = "Returns the arctangent of x (in radians)." },
            new RevitFormulaFunction { Name = "cos", Format = "cos(x)", Description = "Returns the cosine of x (in radians)." },
            new RevitFormulaFunction { Name = "exp", Format = "exp(x)", Description = "Returns e raised to the power of x." },
            new RevitFormulaFunction { Name = "if", Format = "if(condition, true_value, false_value)", Description = "Returns true_value if condition is true, otherwise false_value." },
            new RevitFormulaFunction { Name = "log", Format = "log(x)", Description = "Returns the natural logarithm (base e) of x." },
            new RevitFormulaFunction { Name = "log10", Format = "log10(x)", Description = "Returns the base-10 logarithm of x." },
            new RevitFormulaFunction { Name = "max", Format = "max(x, y)", Description = "Returns the greater of x and y." },
            new RevitFormulaFunction { Name = "min", Format = "min(x, y)", Description = "Returns the lesser of x and y." },
            new RevitFormulaFunction { Name = "round", Format = "round(x)", Description = "Rounds x to the nearest whole number." },
            new RevitFormulaFunction { Name = "sin", Format = "sin(x)", Description = "Returns the sine of x (in radians)." },
            new RevitFormulaFunction { Name = "sqrt", Format = "sqrt(x)", Description = "Returns the square root of x." },
            new RevitFormulaFunction { Name = "tan", Format = "tan(x)", Description = "Returns the tangent of x (in radians)." },
            new RevitFormulaFunction { Name = "roundup", Format = "roundup(x)", Description = "Rounds x up to the nearest whole number." },
            new RevitFormulaFunction { Name = "rounddown", Format = "rounddown(x)", Description = "Rounds x down to the nearest whole number." }
        };
    }
}

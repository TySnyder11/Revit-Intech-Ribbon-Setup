using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech.SharedParameter
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class FamilyTypeParameterAdd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication app = commandData.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Revit.RevitUtils.init(doc);
            SharedParameter.SharedParameterAdd sharedParameterForm = new SharedParameter.SharedParameterAdd(app);
            sharedParameterForm.ShowDialog();
            return Result.Succeeded;
        }
    }
}

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Intech;
using System.Windows;
using System.Windows.Forms;
namespace Intech.Tagging


{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    //Settings
    public class Renumber : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            

            return Result.Succeeded;
        }
    }
}

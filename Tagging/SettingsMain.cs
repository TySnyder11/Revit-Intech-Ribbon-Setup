using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Intech;
using System.Windows;
using System.Windows.Forms;
using TitleBlockSetup.Tagging;
namespace Intech.Tagging


{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    //Settings
    public class RenumberMain : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Revit.RevitUtils.init(doc);
            RenumberSettings settings = new RenumberSettings();
            settings.ShowDialog();

            return Result.Succeeded;
        }
    }
}
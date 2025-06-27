using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Sleeve
{

    [Transaction(TransactionMode.Manual)]
    public class SleeveSettingsMain : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.UI.UIApplication app = commandData.Application;
            UIDocument uidoc = app.ActiveUIDocument;
            Document doc = uidoc.Document;
            Intech.Revit.RevitUtils.init(doc);
            Intech.Sleeve.SleeveSettings settingsForm = new Intech.Sleeve.SleeveSettings();
            settingsForm.ShowDialog();

            return Result.Succeeded;
        }
    }
}


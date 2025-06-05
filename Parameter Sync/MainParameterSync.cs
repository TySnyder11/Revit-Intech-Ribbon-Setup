using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ParameterSyncMenu : IExternalCommand
    {
        public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Intech.RevitHelperFunctions.init(doc);
            
            Intech.ParameterSyncForm parameterSyncForm = new Intech.ParameterSyncForm();
            parameterSyncForm.ShowDialog();



            return Result.Succeeded;
        }
    }
}

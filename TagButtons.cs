using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace Intech


{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class TagSettings : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //Revit pre setup stuff
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            Transaction trans = new Transaction(doc);
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
           
            

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Size : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            ElementId SymId = new ElementId(4405058);
            //ElementId SymId=Elem.Symbol.Id;

            Tag.Family( SymId , commandData, elements);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Elevation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {




            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class Offset : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {




            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Renumber  : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {




            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Length : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {




            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Equipment : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {




            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Hanger : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Tag
    {

        public static void Family(ElementId SymbolId, ExternalCommandData commandData, ElementSet elements)
        {

            //Revit pre setup stuff
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            Transaction trans = new Transaction(doc);
            UIDocument uidoc = commandData.Application.ActiveUIDocument; 
            Transaction tag = new Transaction(doc);
            tag.Start("Tag");
            Element Elefamily = doc.GetElement(SymbolId);
            Family family = Elefamily as Family;
            Debug.WriteLine(family);

            //String name = family.Name;
            //Debug.WriteLine(family.Name);

            //ElementId FamSymId = family.GetFamilySymbolIds().First();
            //FamilySymbol FamSym = doc.GetElement(FamSymId) as FamilySymbol;
            //Debug.WriteLine(FamSym.Name);
            //Category cat = FamSym.Category;
            //Debug.WriteLine(cat.Name);

            tag.Commit();
        }
    }
}

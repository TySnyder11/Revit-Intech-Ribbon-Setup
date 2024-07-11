using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows.Forms;
namespace Intech


{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    
    //Settings
    public class TagSettings : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //Revit pre setup stuff
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            Transaction trans = new Transaction(doc);
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            
            //start settings form
            DialogResult form = System.Windows.Forms.DialogResult.None;
            using (TagSetting selectionForm = new TagSetting())
            {
                form = selectionForm.ShowDialog();
            }

            return Result.Succeeded;
        }
    }



    //Tags
    [Transaction(TransactionMode.Manual)]
    public class Size : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            //Loop until tagging is esc is pressed
            while (true)
            {

                //List of size tags
                var TagFam = tagtools.SaveInformation(this.GetType().Name);
                
                //Select Element
                var element = tagtools.Pickelement(commandData,TagFam.Category);
                if (element.Item2 == 0) break;

                //Tag Element
                if (tagtools.tag
                    (commandData,
                    TagFam.Category,
                    TagFam.Family,
                    TagFam.Path,
                    TagFam.TagFamily,
                    TagFam.Leader,
                    element.Item1)
                    == 0) break;

            }
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Elevation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Loop until tagging is esc is pressed
            while (true)
            {

                //List of size tags
                var TagFam = tagtools.SaveInformation(this.GetType().Name);

                //Select Element
                var element = tagtools.Pickelement(commandData, TagFam.Category);
                if (element.Item2 == 0) break;

                //Tag Element
                if (tagtools.tag
                    (commandData,
                    TagFam.Category,
                    TagFam.Family,
                    TagFam.Path,
                    TagFam.TagFamily,
                    TagFam.Leader,
                    element.Item1)
                    == 0) break;

            }

            return Result.Succeeded;
        }
    }
    [Transaction(TransactionMode.Manual)]
    public class Offset : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Loop until tagging is esc is pressed
            while (true)
            {

                //List of size tags
                var TagFam = tagtools.SaveInformation(this.GetType().Name);

                //Select Element
                var element = tagtools.Pickelement(commandData, TagFam.Category);
                if (element.Item2 == 0) break;

                //Tag Element
                if (tagtools.tag
                    (commandData,
                    TagFam.Category,
                    TagFam.Family,
                    TagFam.Path,
                    TagFam.TagFamily,
                    TagFam.Leader,
                    element.Item1)
                    == 0) break;

            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Number : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Loop until tagging is esc is pressed
            while (true)
            {

                //List of size tags
                var TagFam = tagtools.SaveInformation(this.GetType().Name);

                //Select Element
                var element = tagtools.Pickelement(commandData, TagFam.Category);
                if (element.Item2 == 0) break;

                //Tag Element
                if (tagtools.tag
                    (commandData,
                    TagFam.Category,
                    TagFam.Family,
                    TagFam.Path,
                    TagFam.TagFamily,
                    TagFam.Leader,
                    element.Item1)
                    == 0) break;

            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Length : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Loop until tagging is esc is pressed
            while (true)
            {

                //List of size tags
                var TagFam = tagtools.SaveInformation(this.GetType().Name);

                //Select Element
                var element = tagtools.Pickelement(commandData, TagFam.Category);
                if (element.Item2 == 0) break;

                //Tag Element
                if (tagtools.tag
                    (commandData,
                    TagFam.Category,
                    TagFam.Family,
                    TagFam.Path,
                    TagFam.TagFamily,
                    TagFam.Leader,
                    element.Item1)
                    == 0) break;

            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Equipment : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Loop until tagging is esc is pressed
            while (true)
            {

                //List of size tags
                var TagFam = tagtools.SaveInformation(this.GetType().Name);

                //Select Element
                var element = tagtools.Pickelement(commandData, TagFam.Category);
                if (element.Item2 == 0) break;

                //Tag Element
                if (tagtools.tag
                    (commandData,
                    TagFam.Category,
                    TagFam.Family,
                    TagFam.Path,
                    TagFam.TagFamily,
                    TagFam.Leader,
                    element.Item1)
                    == 0) break;

            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Hanger : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Loop until tagging is esc is pressed
            while (true)
            {

                //List of size tags
                var TagFam = tagtools.SaveInformation(this.GetType().Name);

                //Select Element
                var element = tagtools.Pickelement(commandData, TagFam.Category);
                if (element.Item2 == 0) break;

                //Tag Element
                if (tagtools.tag
                    (commandData,
                    TagFam.Category,
                    TagFam.Family,
                    TagFam.Path,
                    TagFam.TagFamily,
                    TagFam.Leader,
                    element.Item1)
                    == 0) break;

            }

            return Result.Succeeded;
        }
    }
}

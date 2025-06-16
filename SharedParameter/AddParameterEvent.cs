using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Revit
{

    public class AddSharedParametersHandler : IExternalEventHandler
    {
        public List<Family> Families { get; set; }
        public List<Definition> Definitions { get; set; }
        public ForgeTypeId Group { get; set; }
        public bool IsInstance { get; set; }

        public AddSharedParametersHandler(List<Family> families, List<Definition> definitions, ForgeTypeId group, bool isInstance)
        {
            Families = families;
            Definitions = definitions;
            Group = group;
            IsInstance = isInstance;
        }

        public void Execute(UIApplication app)
        {
            try
            {
                Intech.Revit.RevitHelperFunctions.AddSharedParametersToFamilies(Families, Definitions, Group, IsInstance);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        public string GetName() => "Add Shared Parameters to Families";
    }

}

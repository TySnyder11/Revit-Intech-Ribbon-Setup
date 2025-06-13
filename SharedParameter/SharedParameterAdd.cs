using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListBox;

namespace Intech.SharedParameter
{
    public partial class SharedParameterAdd : System.Windows.Forms.Form
    {
        List<Family> families = new List<Family>();
        DefinitionGroups defGroups = null;
        Definitions definitions = null;
        Dictionary<string, bool> defCheckStore = new Dictionary<string, bool>();
        Dictionary<string, bool> famCheckStore = new Dictionary<string, bool>();

        public SharedParameterAdd()
        {
            InitializeComponent();
            CenterToParent();
            families = Intech.Revit.RevitHelperFunctions.GetFamilies();
            defGroups = Intech.Revit.RevitHelperFunctions.GetDefinitionGroups();

            // Populate the FamilySelect TreeView with families and their symbols
            families = families.OrderBy(f => f.Name).ToList();
            families.ForEach(f => FamilySelect.Items.Add(f.Name));
            families.ForEach(f => famCheckStore.Add(f.Name, false));

            // Populate the definition groups into the ComboBox
            defGroupSelect.Items.Clear();
            foreach (DefinitionGroup group in defGroups)
            {
                defGroupSelect.Items.Add(group.Name);
            }
            if (defGroupSelect.Items.Count > 0)
            {
                defGroupSelect.SelectedIndex = 0; // Select the first group by default
            }

        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void Confirm_Click(object sender, EventArgs e)
        {
            // Logic to handle confirmation of shared parameter addition
            // This could include validation and saving the selected parameters
            // For now, we will just close the form
            this.Close();
        }

        private void defGroupSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the definitionSelect CheckedListBox based on the selected definition group
            definitionSelect.Items.Clear();
            defCheckStore.Clear();
            string selectedGroup = defGroupSelect.SelectedItem.ToString();
            DefinitionGroup group = defGroups.get_Item(selectedGroup);
            definitions = group.Definitions;
            foreach (Definition definition in definitions)
            {
                definitionSelect.Items.Add(definition.Name, false); // Add definitions with unchecked state
                defCheckStore.Add(definition.Name, false); // Store the unchecked state
            }
        }

        private void FamilySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedObjectCollection selected = FamilySelect.SelectedItems;
            if (selected.Count > 0)
            {
                string selectedFamily = selected[0].ToString();
                famCheckStore[selectedFamily] = !famCheckStore[selectedFamily]; // Toggle the check state
                FamilySelect.SetItemChecked(FamilySelect.SelectedIndex, famCheckStore[selectedFamily]);
                famCheckStore[selectedFamily] = FamilySelect.GetItemChecked(FamilySelect.SelectedIndex); // Update the store
            }
        }

        private void familySearch_TextChanged(object sender, EventArgs e)
        {
            FamilySelect.Items.Clear();
            //List<Family> filtered = families.Where(f => f.Name.ToLower().Contains(familySearch.Text.ToLower())).ToList();
        }
    }
}

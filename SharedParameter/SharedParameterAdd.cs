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

            FamilySelect.ItemCheck += FamilySelect_ItemCheck;

            // Populate the FamilySelect TreeView with families and their symbols
            families = families.OrderBy(f => f.Name).ToList();
            LoadItems(families);
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

        int lastCheckedIndex = -1;

        void LoadItems(List<Family> items)
        {
            foreach (Family family in items)
            {
                if (!famCheckStore.ContainsKey(family.Name))
                    famCheckStore[family.Name] = false;
            }

            UpdateFamilySelect(""); // Load all items initially
        }



        void UpdateFamilySelect(string filter)
        {
            FamilySelect.Items.Clear();

            var filtered = famCheckStore
            .Where(kvp => kvp.Key.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
            .ToList();

            foreach (var kvp in filtered)
            {
                FamilySelect.Items.Add(kvp.Key, kvp.Value);
            }
        }



        private void FamilySelect_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string currentItem = FamilySelect.Items[e.Index].ToString();

            if (ModifierKeys == Keys.Shift && lastCheckedIndex != -1)
            {
                int start = Math.Min(lastCheckedIndex, e.Index);
                int end = Math.Max(lastCheckedIndex, e.Index);
                bool newState = (e.NewValue == CheckState.Checked);

                // Temporarily detach the event handler to prevent recursion
                FamilySelect.ItemCheck -= FamilySelect_ItemCheck;

                for (int i = start; i <= end && i < FamilySelect.Items.Count; i++)
                {
                    string itemText = FamilySelect.Items[i].ToString();
                    famCheckStore[itemText] = newState;
                    FamilySelect.SetItemChecked(i, newState); // This would normally trigger ItemCheck
                }

                // Reattach the event handler
                FamilySelect.ItemCheck += FamilySelect_ItemCheck;

                // Cancel default behavior
                e.NewValue = FamilySelect.GetItemChecked(e.Index) ? CheckState.Checked : CheckState.Unchecked;
            }
            else
            {
                // Normal click: update backing store after event completes
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    famCheckStore[currentItem] = FamilySelect.GetItemChecked(e.Index);
                }));
            }

            lastCheckedIndex = e.Index;
        }




        private void familySearch_TextChanged(object sender, EventArgs e)
        {
            UpdateFamilySelect(familySearch.Text);
        }


    }
}

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
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
        Dictionary<string, bool> famCheckStore = new Dictionary<string, bool>();
        List<ForgeTypeId> groups = new List<ForgeTypeId>();
        UIApplication app = null;
        public SharedParameterAdd(UIApplication app)
        {
            this.app = app;
            InitializeComponent();
            CenterToParent();
            families = Intech.Revit.RevitUtils.GetFamilies();
            defGroups = Intech.Revit.RevitUtils.GetDefinitionGroups();

            FamilySelect.ItemCheck += FamilySelect_ItemCheck;
            FamilySelect.CheckOnClick = true;
            definitionSelect.Sorted = true;
            definitionSelect.CheckOnClick = true;
            InstanceSelect.Checked = true;

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


            groups = Revit.RevitUtils.GetAllGroupTypeIds();
            groups.Add(new ForgeTypeId(string.Empty));
            foreach (ForgeTypeId id in groups)
            {
                ParameterCategory.Items.Add(LabelUtils.GetLabelForGroup(id));
            }
            ParameterCategory.Sorted = true;
            if (ParameterCategory.Items.Count > 0)
            {
                ParameterCategory.SelectedIndex = 0; // Select the first group by default
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void defGroupSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update the definitionSelect CheckedListBox based on the selected definition group
            definitionSelect.Items.Clear();
            string selectedGroup = defGroupSelect.SelectedItem.ToString();
            DefinitionGroup group = defGroups.get_Item(selectedGroup);
            definitions = group.Definitions;
            foreach (Definition definition in definitions)
            {
                definitionSelect.Items.Add(definition.Name, false); // Add definitions with unchecked state
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

        private void Confirm_Click(object sender, EventArgs e)
        {
            List<Family> selectedFamilies = famCheckStore?
             .Where(kvp => kvp.Value)
             .Select(kvp => families?.FirstOrDefault(f => f?.Name == kvp.Key))
             .Where(f => f != null)
             .ToList() ?? new List<Family>();

            List<Definition> selectedDefinitions = definitionSelect?.CheckedItems?
             .Cast<string>()
             .Select(name => definitions?.get_Item(name))
             .Where(def => def != null)
             .ToList() ?? new List<Definition>();

            ForgeTypeId group = groups?.FirstOrDefault(g =>string.Equals(LabelUtils.GetLabelForGroup(g), 
                ParameterCategory.SelectedItem?.ToString(), StringComparison.OrdinalIgnoreCase));

            Intech.Revit.AddSharedParametersHandler test = new Intech.Revit.AddSharedParametersHandler(selectedFamilies, selectedDefinitions, group, InstanceSelect.Checked);
            test.Execute(app);
        }

        private void PushParameter_Click(object sender, EventArgs e)
        {
            //Open the push formula dialog
        }

        private void ParameterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void TypeSelect_CheckedChanged(object sender, EventArgs e)
        {
            InstanceSelect.CheckedChanged -= InstanceSelect_CheckedChanged;
            InstanceSelect.Checked = !TypeSelect.Checked;
            InstanceSelect.CheckedChanged += InstanceSelect_CheckedChanged;
        }

        private void InstanceSelect_CheckedChanged(object sender, EventArgs e)
        {
            TypeSelect.CheckedChanged -= TypeSelect_CheckedChanged;
            TypeSelect.Checked = !InstanceSelect.Checked;
            TypeSelect.CheckedChanged += TypeSelect_CheckedChanged;
        }
    }
}

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

namespace Intech.SharedParameter
{
    public partial class SharedParameterAdd : System.Windows.Forms.Form
    {
        Dictionary<Family, List<FamilySymbol>> famSymMap = new Dictionary<Family, List<FamilySymbol>>();
        DefinitionGroups defGroups = null;
        Definitions definitions = null;
        Dictionary<string, bool> defCheckStore = new Dictionary<string, bool>();

        public SharedParameterAdd()
        {
            InitializeComponent();
            CenterToParent();
            famSymMap = Intech.Revit.RevitHelperFunctions.GetFamilySymbolMap();
            defGroups = Intech.Revit.RevitHelperFunctions.GetDefinitionGroups();

            // Populate the FamilySelect TreeView with families and their symbols
            List<Family> fams = famSymMap.Keys.ToList();
            fams.Sort(new CompareFam());
            foreach (Family family in fams)
            {
                TreeNode familyNode = new TreeNode(family.Name);
                FamilySelect.Nodes.Add(familyNode);
                foreach (FamilySymbol symbol in famSymMap[family])
                {
                    TreeNode symbolNode = new TreeNode(symbol.Name);
                    familyNode.Nodes.Add(symbolNode);
                }
            }

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
        private class CompareFam : Comparer<Family>
        {
            public override int Compare(Family x, Family y)
            {
                return string.Compare(x.Name, y.Name);
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

        private void definitionSelect_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Update the defCheckStore dictionary when an item is checked or unchecked
            string definitionName = definitionSelect.Items[e.Index].ToString();
            defCheckStore[definitionName] = e.NewValue == CheckState.Checked;
        }
    }
}

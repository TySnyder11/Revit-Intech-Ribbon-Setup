using Autodesk.Revit.DB;
using Intech.Revit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech.SharedParameter
{
    public partial class FormulaAdd : System.Windows.Forms.Form
    {
        List<Family> familyList = new List<Family>();

        private bool suppressCopy = false;

        public FormulaAdd()
        {
            InitializeComponent();
            CenterToParent();
            InitializeCheckBox();
            FamilySelect.SelectedIndexChanged += FamilySelect_CheckedChanged;
            FormulaTextBox.TextChanged += FormulaTextBox_TextChanged;
            parameters.Sorted = true;
            FamilySelect.Sorted = true;
            SelectParameterComboBox.DefaultValue = string.Empty;
            SelectParameterComboBox.Sorted = true;
        }

        private void FormulaTextBox_TextChanged(object sender, EventArgs e)
        {
            string text = string.Empty;
            if (FormulaTextBox.Text.StartsWith("="))
                text = FormulaTextBox.Text.Substring(1);
            else 
                text = FormulaTextBox.Text;
            string parameterName =  SelectParameterComboBox.SelectedItem.ToString();
            SelectParameterComboBox.UpdateItem(parameterName, text);

        }

        private void InitializeCheckBox()
        {
            familyList = Revit.RevitUtils.GetFamilies();
            List<string> names = new List<string>();
            familyList.Where(f => f.IsEditable).ToList().ForEach(fam => names.Add(fam.Name));
            FamilySelect.AllItems = names;
        }

        private void famSearch_TextChanged(object sender, EventArgs e)
        {
            FamilySelect.Filter = famSearch.Text;
        }

        private void FamilySelect_CheckedChanged(object sender, EventArgs e)
        {
            List<string> checkList = FamilySelect.GetCheckedItems();
            List<Family> FamilyList = familyList.Where(t => checkList.Contains(t.Name)).ToList();
            List<string> comName = RevitUtils.GetCommonParameters(FamilyList);
            comName.Sort();

            suppressCopy = true;
            parameters.DataSource = comName;
            parameters.ClearSelected(); // Optional: clear selection
            suppressCopy = false;

            SelectParameterComboBox.SetItems(RevitUtils.GetCommonSharedParametersFromFamilies(FamilyList));

        }

        ToolTip copyToolTip = new ToolTip();

        private void parameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (parameters.SelectedItem != null)
            {

                if (suppressCopy)
                    return;

                // Copy the selected item to clipboard
                Clipboard.SetText(parameters.SelectedItem.ToString());

                // Get the rectangle of the selected item
                int index = parameters.SelectedIndex;
                System.Drawing.Rectangle itemRect = parameters.GetItemRectangle(index);

                // Convert to screen coordinates
                System.Drawing.Point screenPoint = parameters.PointToScreen(new System.Drawing.Point(itemRect.X, itemRect.Y));

                // Show tooltip near the selected item for 1 second
                copyToolTip.Show("Parameter Copied", parameters,
                 parameters.PointToClient(screenPoint),
                 1000);
            }
        }

        private void SelectParameterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormulaTextBox.TextChanged -= FormulaTextBox_TextChanged;
            FormulaTextBox.Text = "=" + SelectParameterComboBox.GetValue(SelectParameterComboBox.SelectedItem.ToString());
            FormulaTextBox.TextChanged += FormulaTextBox_TextChanged;
        }

        private void AllParamButton_Click(object sender, EventArgs e)
        {
            List<string> checkList = FamilySelect.GetCheckedItems();
            List<Family> FamilyList = familyList.Where(t => checkList.Contains(t.Name)).ToList();
            List<string> names = Revit.RevitUtils.GetCommonFormulaUsableParameters(FamilyList).ToList();
            suppressCopy = true;
            parameters.DataSource = names;
            parameters.ClearSelected(); // Optional: clear selection
            suppressCopy = false;
        }
    }
}

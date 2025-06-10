using Autodesk.Revit.DB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech
{
    public partial class EditParameterSync : System.Windows.Forms.Form
    {
        CategoryNameMap catagories = new CategoryNameMap();
        List<string> categoryNames = new List<string>();
        List<string> parameters = new List<string>();
        List<string> units = new List<string>();

        private List<string> currentSuggestions = new List<string>();
        private int suggestionIndex = 0;


        private Label ghostLabel;

        public EditParameterSync(string name, string category, string smartbox, string output)
        {
            InitializeComponent();
            CenterToParent();
            catagories = Intech.Revit.RevitHelperFunctions.GetAllCategories();
            // Populate the category combo box with category names
            foreach (Category cat in catagories)
            {
                categoryNames.Add(cat.Name);
            }
            categoryComboBox.DataSource = categoryNames;

            InitializeSmartParameterBox();
            nameTextBox.Text = name;
            if (catagories.Contains(category))
            {
                categoryComboBox.Text = category;
                Category cat = catagories.get_Item(category);
                parameters = Intech.Revit.RevitHelperFunctions.GetParameters(cat);
                parameters.Sort();
                parameterComboBox.DataSource = parameters;
            }
            else
            {
                categoryComboBox.Text = string.Empty;
            }
            smartParameterBox.Text = smartbox;
            if (parameters.Contains(output))
            {
                parameterComboBox.Text = output;
            }
            else
            {
                parameterComboBox.Text = string.Empty;
            }
        }

        private void InitializeSmartParameterBox()
        {
            smartParameterBox.AcceptsTab = true;
            smartParameterBox.TextChanged += smartParameterBox_TextChanged;
            smartParameterBox.KeyDown += smartParameterBox_KeyDown;

            ghostLabel = new Label
            {
                ForeColor = System.Drawing.Color.Gray,
                BackColor = System.Drawing.Color.Transparent,
                AutoSize = true,
                Visible = false
            };
            smartParameterBox.Controls.Add(ghostLabel);

        }


        private void category_SelectedIndexChanged(object sender, EventArgs e)
        {
            parameters.Clear();
            if (catagories.Contains(categoryComboBox.Text))
            {
                Category cat = catagories.get_Item(categoryComboBox.Text);
                parameters = Revit.RevitHelperFunctions.GetParameters(cat);
                parameters.Sort();
                if (!parameters.Contains(parameterComboBox.Text))
                {
                    parameterComboBox.Text = string.Empty;
                }
                parameterComboBox.DataSource = parameters;
            }
        }
        private void category_SelectedTextUpdate(object sender, EventArgs e)
        {
            string searchTerm = categoryComboBox.Text;
            var filteredItems = categoryNames.Where(item => item.ToLower().Contains(searchTerm.ToLower())).ToList();
            categoryComboBox.DataSource = filteredItems;

            if (String.IsNullOrWhiteSpace(searchTerm))
            {
                categoryComboBox.DataSource = categoryNames;
            }
            categoryComboBox.DroppedDown = true;
            categoryComboBox.SelectedIndex = -1;
            categoryComboBox.Text = searchTerm;
            categoryComboBox.SelectionStart = searchTerm.Length;
        }
        private void parameter_SelectedTextUpdate(object sender, EventArgs e)
        {
            string searchTerm = parameterComboBox.Text;
            var filteredItems = parameters.Where(item => item.ToLower().Contains(searchTerm.ToLower())).ToList();
            parameterComboBox.DataSource = filteredItems;
            if (String.IsNullOrWhiteSpace(searchTerm))
            {
                parameterComboBox.DataSource = categoryNames;
            }
            parameterComboBox.DroppedDown = true;
            parameterComboBox.SelectedIndex = -1;
            parameterComboBox.Text = searchTerm;
            parameterComboBox.SelectionStart = searchTerm.Length;
        }

        private void smartParameterBox_TextChanged(object sender, EventArgs e)
        {

            if (smartParameterBox.Text.Contains("\t"))
            {
                int cursor = smartParameterBox.SelectionStart;
                smartParameterBox.Text = smartParameterBox.Text.Replace("\t", "");
                smartParameterBox.SelectionStart = Math.Min(cursor, smartParameterBox.Text.Length);
            }

            int selectionStart = smartParameterBox.SelectionStart;
            int selectionLength = smartParameterBox.SelectionLength;

            smartParameterBox.TextChanged -= smartParameterBox_TextChanged;

            string text = smartParameterBox.Text;
            smartParameterBox.SelectAll();
            smartParameterBox.SelectionColor = System.Drawing.Color.Black;

            bool[] colored = new bool[text.Length];
            int i = 0;

            // First pass: color [ ... ] in red
            while (i < text.Length)
            {
                if (text[i] == '[')
                {
                    int start = i;
                    int end = text.IndexOf(']', start + 1);
                    if (end == -1) end = text.Length - 1;

                    for (int j = start; j <= end && j < text.Length; j++)
                    {
                        smartParameterBox.Select(j, 1);
                        smartParameterBox.SelectionColor = System.Drawing.Color.Red;
                        colored[j] = true;
                    }

                    i = end + 1;
                }
                else
                {
                    i++;
                }
            }

            // Second pass: color { ... } in purple, only if not already colored
            i = 0;
            while (i < text.Length)
            {
                if (text[i] == '{')
                {
                    int start = i;
                    int end = text.IndexOf('}', start + 1);
                    if (end == -1) end = text.Length - 1;

                    for (int j = start; j <= end && j < text.Length; j++)
                    {
                        if (!colored[j])
                        {
                            smartParameterBox.Select(j, 1);
                            smartParameterBox.SelectionColor = System.Drawing.Color.Purple;
                            colored[j] = true;
                        }
                    }

                    i = end + 1;
                }
                else
                {
                    i++;
                }
            }

            smartParameterBox.Select(selectionStart, selectionLength);
            smartParameterBox.SelectionColor = System.Drawing.Color.Black;

            smartParameterBox.TextChanged += smartParameterBox_TextChanged;

            ShowGhostSuggestion();
        }
        private void ShowGhostSuggestion()
        {
            int cursorPos = smartParameterBox.SelectionStart;
            string text = smartParameterBox.Text.Substring(0, cursorPos);
            int lastOpen = text.LastIndexOf('[');
            int lastClose = text.LastIndexOf(']');
            int lastOpenBrac = text.LastIndexOf('{');
            int lastCloseBrac = text.LastIndexOf('}');
            if (lastOpen > lastClose)
            {
                string partial = text.Substring(lastOpen + 1);
                currentSuggestions = parameters
                    .Where(p => p.StartsWith(partial, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (currentSuggestions.Count > 0)
                {
                    suggestionIndex = 0;
                    string match = currentSuggestions[suggestionIndex];
                    Revit.RevitHelperFunctions.GetUnit(catagories.get_Item(categoryComboBox.Text), match, out string unit, out ForgeTypeId unitID);
                    match += "]";
                    if (unit != null)
                    {
                        match += "{ " + unit + "}";
                    }
                    string suggestion = match.Substring(partial.Length);

                    System.Drawing.Point pos = smartParameterBox.GetPositionFromCharIndex(cursorPos);
                    ghostLabel.Text = suggestion;
                    ghostLabel.Location = new System.Drawing.Point(pos.X + 1, pos.Y);
                    ghostLabel.Visible = true;
                    return;
                }
            }
            else if (lastOpenBrac > lastCloseBrac && lastClose != -1 && lastOpen != -1)
            {
                string parameter = text.Substring(lastOpen + 1, lastClose - lastOpen - 1 ).Trim();
                Revit.RevitHelperFunctions.GetUnit(catagories.get_Item(categoryComboBox.Text), parameter, out string unit, out ForgeTypeId unitID, out ForgeTypeId specTypeId);

                if (UnitUtils.IsMeasurableSpec(specTypeId))
                {
                    List<ForgeTypeId> validUnits = UnitUtils.GetValidUnits(specTypeId) as List<ForgeTypeId>;
                    validUnits.Remove(unitID);
                    validUnits.Insert(0, unitID);
                    List<string> unitNames = validUnits
                        .Select(u => LabelUtils.GetLabelForUnit(u))
                        .Where(u => !string.IsNullOrEmpty(u))
                        .ToList();
                    string partial = text.Substring(lastOpenBrac + 1);
                    currentSuggestions = unitNames
                        .Where(p => p.StartsWith(partial, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (currentSuggestions.Count > 0)
                    {
                        suggestionIndex = 0;
                        string match = currentSuggestions[suggestionIndex];
                        match += "}";
                        string suggestion = match.Substring(partial.Length);

                        System.Drawing.Point pos = smartParameterBox.GetPositionFromCharIndex(cursorPos);
                        ghostLabel.Text = suggestion;
                        ghostLabel.Location = new System.Drawing.Point(pos.X + 1, pos.Y);
                        ghostLabel.Visible = true;
                        return;
                    }
                }
            }
            ghostLabel.Visible = false;
            currentSuggestions.Clear();
            suggestionIndex = 0;
        }


        private void smartParameterBox_KeyDown(object sender, KeyEventArgs e)
        {
            int cursorPos = smartParameterBox.SelectionStart;

            if (e.KeyCode == Keys.Tab && ghostLabel.Visible)
            {
                string text = smartParameterBox.Text.Substring(0, cursorPos);
                int lastBracket = text.LastIndexOf('[');
                int lastSquiglBracket = text.LastIndexOf('{');

                if (lastBracket != -1 && lastBracket > lastSquiglBracket)
                {
                    string insertion = ghostLabel.Text;
                    if (ghostLabel.Text.LastIndexOf("{") != -1)
                    {
                        insertion = ghostLabel.Text.Substring(0, ghostLabel.Text.LastIndexOf("{") + 1);
                    }
                    smartParameterBox.Text = smartParameterBox.Text.Insert(cursorPos, insertion);
                    smartParameterBox.SelectionStart = cursorPos + insertion.Length;
                }
                else if (lastSquiglBracket != -1)
                {
                    string insertion = ghostLabel.Text;
                    smartParameterBox.Text = smartParameterBox.Text.Insert(cursorPos, insertion);
                    smartParameterBox.SelectionStart = cursorPos + insertion.Length;
                }


                ghostLabel.Visible = false;
                e.SuppressKeyPress = true;
                ShowGhostSuggestion();

            }
            else if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.Up) && ghostLabel.Visible && currentSuggestions.Count > 1)
            {
                if (e.KeyCode == Keys.Down)
                {
                    suggestionIndex = (suggestionIndex + 1) % currentSuggestions.Count;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    suggestionIndex = (suggestionIndex - 1 + currentSuggestions.Count) % currentSuggestions.Count;
                }

                string text = smartParameterBox.Text.Substring(0, cursorPos);
                int lastBracket = text.LastIndexOf('[');
                int lastSquiglBracket = text.LastIndexOf('{');

                if (lastBracket != -1 && lastBracket > lastSquiglBracket)
                {
                    string partial = text.Substring(lastBracket + 1);
                    string match = currentSuggestions[suggestionIndex];
                    Revit.RevitHelperFunctions.GetUnit(catagories.get_Item(categoryComboBox.Text), match, out string unit, out ForgeTypeId unitID);
                    match += "]";
                    if( unit != null)
                    {
                        match += "{ " + unit + "}";
                    }
                    string suggestion = match.Substring(partial.Length);

                    System.Drawing.Point pos = smartParameterBox.GetPositionFromCharIndex(cursorPos);
                    ghostLabel.Text = suggestion;
                    ghostLabel.Location = new System.Drawing.Point(pos.X + 1, pos.Y);
                } 
                else if(lastSquiglBracket != -1)
                {
                    string partial = text.Substring(lastSquiglBracket + 1);
                    string match = currentSuggestions[suggestionIndex];
                    match += "}";
                    string suggestion = match.Substring(partial.Length);

                    System.Drawing.Point pos = smartParameterBox.GetPositionFromCharIndex(cursorPos);
                    ghostLabel.Text = suggestion;
                    ghostLabel.Location = new System.Drawing.Point(pos.X + 1, pos.Y);
                }

                e.SuppressKeyPress = true;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void saveAndLoad_Click(object sender, EventArgs e)
        {
            Intech.ParameterSyncMenu.compute(smartParameterBox.Text.Trim(), categoryComboBox.Text, parameterComboBox.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}

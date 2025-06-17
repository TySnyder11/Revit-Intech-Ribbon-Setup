using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech
{
    public partial class ParameterSyncForm : System.Windows.Forms.Form
    {
        CategoryNameMap catagories = new CategoryNameMap();
        public ParameterSyncForm()
        {
            InitializeComponent();
            CenterToParent();
            dataGridView1.Columns["NameColumn"].ReadOnly = true;
            catagories = Revit.RevitUtils.GetAllCategories();
            // Populate the combo box in the Element type row  on the dataGrid with category names
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
            foreach (Category category in catagories)
            {
                comboBoxCell.Items.Add(category.Name);
            }
            comboBoxCell.Sorted = true;
            dataGridView_Update();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Intech.NewParameterSync newParameterSync = new Intech.NewParameterSync();
            newParameterSync.ShowDialog();
            dataGridView_Update();
        }

        private void dataGridView_Update()
        {
            dataGridView1.Rows.Clear();
            SaveFileManager manager = new SaveFileManager(Path.Combine(Path.Combine(App.BasePath, "SaveFileManager"), "temp.txt"), new TxtFormat());
            List<Intech.SaveFileSection> sections = manager.GetSectionsByProject(Intech.ParameterSyncMenu.doc.Title);
            if (sections.Count > 0)
            {
                SaveFileSection section = null;
                foreach (SaveFileSection sec in sections)
                {
                    if (sec.SecondaryName == "ParameterSyncMenu")
                    {
                        section = sec;
                    }
                }
                if (section == null)
                {
                    MessageBox.Show("No Parameter Sync data found for this project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<string[]> saveRows = section.Rows;
                foreach(string[] row in saveRows)
                {
                    if(row.Length == 4)
                    {
                        int index = dataGridView1.Rows.Add();
                        dataGridView1.Rows[index].Cells["NameColumn"].Value = row[0];
                        dataGridView1.Rows[index].Cells["Element"].Value = row[1];
                        dataGridView1.Rows[index].Cells["baseParam"].Value = row[2];
                        dataGridView1.Rows[index].Cells["Output"].Value = row[3];
                    }
                }
            }
            dataGridView1.ClearSelection();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            SaveFileManager manager = new SaveFileManager(Path.Combine(Path.Combine(App.BasePath, "SaveFileManager"), "temp.txt"), new TxtFormat());
            List<Intech.SaveFileSection> sections = manager.GetSectionsByProject(Intech.ParameterSyncMenu.doc.Title);
            SaveFileSection section = null;
            foreach (SaveFileSection sec in sections)
            {
                if (sec.SecondaryName == "ParameterSyncMenu")
                {
                    section = sec;
                }
            }
            DataGridViewCell cell = dataGridView1.SelectedCells[0];
            section.Rows.RemoveAt(cell.RowIndex);
            manager.AddOrUpdateSection(section);
            dataGridView_Update();
        }

        private void reloadselect_Click(object sender, EventArgs e)
        {
            List<int> rows = new List<int>();
            DataGridViewSelectedCellCollection cells = dataGridView1.SelectedCells;
            foreach (DataGridViewCell cell in cells)
            {
                if (rows.Contains(cell.RowIndex))
                    continue; // Skip if the row is already added
                rows.Add(cell.RowIndex);
            }


            foreach (int row in rows)
            {

                string elementType = dataGridView1.Rows[row].Cells["Element"].Value.ToString();
                string baseParam = dataGridView1.Rows[row].Cells["baseParam"].Value.ToString();
                string outputParam = dataGridView1.Rows[row].Cells["Output"].Value.ToString();
                Intech.ParameterSyncMenu.compute(baseParam, elementType, outputParam);
            }
        }

        private void reloadAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue; // Skip the new row placeholder
                string elementType = row.Cells["Element"].Value.ToString();
                string baseParam = row.Cells["baseParam"].Value.ToString();
                string outputParam = row.Cells["Output"].Value.ToString();
                Intech.ParameterSyncMenu.compute(baseParam, elementType, outputParam);
            }
        }

        private void edit_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedCells.Count > 0)
            {
                DataGridViewCell cell = dataGridView1.SelectedCells[0];
                if (cell == null || cell.RowIndex < 0 || cell.ColumnIndex < 0)
                {
                    MessageBox.Show("Please select a valid cell to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string name = dataGridView1.Rows[cell.RowIndex].Cells["NameColumn"].Value.ToString();
                string elementType = dataGridView1.Rows[cell.RowIndex].Cells["Element"].Value.ToString();
                string baseParam = dataGridView1.Rows[cell.RowIndex].Cells["baseParam"].Value.ToString();
                string outputParam = dataGridView1.Rows[cell.RowIndex].Cells["Output"].Value.ToString();

                EditParameterSync editParameterSync = new EditParameterSync(name, elementType, baseParam, outputParam);
                if (editParameterSync.ShowDialog() == DialogResult.OK)
                {
                    // Save changes to the file
                    SaveFileManager manager = new SaveFileManager(Path.Combine(Path.Combine(App.BasePath, "SaveFileManager"), "temp.txt"), new TxtFormat());
                    List<Intech.SaveFileSection> sections = manager.GetSectionsByProject(Intech.ParameterSyncMenu.doc.Title);
                    SaveFileSection section = null;
                    foreach (SaveFileSection sec in sections)
                    {
                        if (sec.SecondaryName == "ParameterSyncMenu")
                        {
                            section = sec;
                        }
                    }
                    if (section != null)
                    {
                        section.Rows[cell.RowIndex] = new string[] { editParameterSync.nameTextBox.Text, 
                            editParameterSync.categoryComboBox.Text, editParameterSync.smartParameterBox.Text, editParameterSync.parameterComboBox.Text };
                        manager.AddOrUpdateSection(section);
                    }
                }
                dataGridView_Update();
            }
        }
    }
}

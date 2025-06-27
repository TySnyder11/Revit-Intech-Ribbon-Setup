using Autodesk.Revit.DB;
using Intech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TitleBlockSetup.Tagging
{
    public partial class RenumberSettings : System.Windows.Forms.Form
    {
        CategoryNameMap categories;
        public RenumberSettings()
        {
            InitializeComponent();
            CenterToParent();
            InitSectionGrid();

            Confirm.Click += Confirm_Click;
            Cancel.Click += Cancel_Click;
        }


        private void Cancel_Click(object sender, EventArgs e)
        {
            if (renumberMenu.HasChanges)
            {
                DialogResult result = MessageBox.Show(
                "You have unsaved changes. Are you sure you want to close?",
                "Confirm Close",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            this.Close();
        }


        private void Confirm_Click(object sender, EventArgs e)
        {
            renumberMenu.Confirm();
            this.Close();
        }

        private void InitSectionGrid()
        {
            renumberMenu.CellEdited += RenumberMenu_CellEdited;
            renumberMenu.RowAdded += RenumberMenu_RowAdded;

            categories = Intech.Revit.RevitUtils.GetAllCategories();

            string filePath = Path.Combine(App.BasePath, "Settings.txt");
            SaveFileManager saveFileManager = new SaveFileManager(filePath, new TxtFormat());

            SaveFileSection sec =  saveFileManager.GetSectionsByName("Number Settings", "Main") ?? 
                new SaveFileSection("Number Settings", "Main", "Category\tParameter\tTag\tPrefix\tCurrent Number\tSuffix\tSeperator");

            Dictionary<string, Intech.Windows.Forms.ColumnType > columnDictionary = new Dictionary<string, Intech.Windows.Forms.ColumnType> 
            {
                {"Category", Intech.Windows.Forms.ColumnType.ComboBox },
                {"Parameter", Intech.Windows.Forms.ColumnType.ComboBox },
                { "Tag" , Intech.Windows.Forms.ColumnType.CheckBox }
            };


            renumberMenu.ConfigureColumnTypes(columnDictionary);
            renumberMenu.Initialize(saveFileManager, sec);
            List<string> catData = new List<string>();
            foreach (Category cat in categories)
            {
                catData.Add(cat.Name);
            }
            renumberMenu.SetComboBoxItems("Category", catData);

            renumberMenu.SetDefaultColumnValue("Tag", true);
            renumberMenu.SetDefaultColumnValue("Current Number", "1");
            renumberMenu.SetDefaultColumnValue("Seperator", "-");

            renumberMenu.SetColumnWidth("Category", 200);

            renumberMenu.SetColumnWidth("Parameter", 150);
        }

        private void RenumberMenu_RowAdded(object sender, EventArgs e)
        {
            DataGridViewRowsAddedEventArgs rowEvent = e as DataGridViewRowsAddedEventArgs;
            if (renumberMenu.GetCellValue(0, rowEvent.RowIndex) is string categoryName && !string.IsNullOrWhiteSpace(categoryName))
            {
                Category categrory = categories.get_Item(categoryName);
                renumberMenu.SetComboBoxItems("Parameter", rowEvent.RowIndex, Intech.Revit.RevitUtils.GetParameters(categrory));
            }
        }

        private void RenumberMenu_CellEdited(object sender, EventArgs e)
        {
            DataGridViewCellEventArgs cellEvent = e as DataGridViewCellEventArgs;

            if (cellEvent.ColumnIndex == 0)
            {
                string categoryName = (string)renumberMenu.GetCellValue(0, cellEvent.RowIndex);
                Category categrory = categories.get_Item(categoryName);
                renumberMenu.SetComboBoxItems("Parameter", cellEvent.RowIndex, Intech.Revit.RevitUtils.GetParameters(categrory));
            } 
        }

        private void RenumberSettings_Load(object sender, EventArgs e)
        {

        }

        private void AdvancedSettings_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedCellCollection cells = renumberMenu.GetSelectedCell();
            HashSet<int> rows = new HashSet<int>();
            foreach (DataGridViewCell cell in cells)
            {
                rows.Add(cell.RowIndex);
            }
            foreach(int row in rows)
            {
                string catName = renumberMenu.GetCellValue(0, row) as String;
                Category category = categories.get_Item(catName);
                if (category != null)
                {
                    NumAdvancedSettings numAdvancedSettings = new NumAdvancedSettings(category);
                    numAdvancedSettings.ShowDialog();
                    renumberMenu.SetComboBoxItems("Parameter", row, Intech.Revit.RevitUtils.GetParameters(category));
                }
                else
                {
                    MessageBox.Show($"Category '{catName}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

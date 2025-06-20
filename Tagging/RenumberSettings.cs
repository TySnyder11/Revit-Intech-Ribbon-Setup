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

            }
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

            string filePath = Path.Combine(App.BasePath, "ReNumber.txt");
            SaveFileManager saveFileManager = new SaveFileManager(filePath, new TxtFormat());

            SaveFileSection sec =  saveFileManager.GetSectionsByProject("__General__").FirstOrDefault()?? 
                new SaveFileSection("__General__", "", "Category\tParameter\tTag\tPrefix\tCurrent Number\tSuffix\tSeperator");

            Dictionary<string, Intech.Windows.Forms.ColumnType> columnDictionary = new Dictionary<string, Intech.Windows.Forms.ColumnType> 
            {
                {"Category", Intech.Windows.Forms.ColumnType.ComboBox },
                {"Parameter", Intech.Windows.Forms.ColumnType.ComboBox },
                { "Tag" , Intech.Windows.Forms.ColumnType.CheckBox }
            };

            List<string> catData = new List<string>();
            foreach (Category cat in categories)
            {
                catData.Add(cat.Name);
            }
            renumberMenu.SetComboBoxItems("Category", catData);

            renumberMenu.ConfigureColumnTypes(columnDictionary);
            renumberMenu.Initialize(saveFileManager, sec);

            renumberMenu.SetDefaultColumnValue("Tag", true);
            renumberMenu.SetDefaultColumnValue("Current Number", "1");
            renumberMenu.SetDefaultColumnValue("Seperator", "-");

            renumberMenu.SetColumnWidth("Category", 200);

            renumberMenu.SetColumnWidth("Parameter", 150);
        }

        private void RenumberMenu_RowAdded(object sender, EventArgs e)
        {
            DataGridViewRowsAddedEventArgs rowEvent = e as DataGridViewRowsAddedEventArgs;
            if (renumberMenu.GetCellValue(0, rowEvent.RowIndex) is string categoryName)
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
    }
}

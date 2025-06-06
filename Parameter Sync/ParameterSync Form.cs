using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            catagories = Revit.RevitHelperFunctions.GetAllCategories();
            // Populate the combo box in the Element type row  on the dataGrid with category names
            DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
            foreach (Category category in catagories)
            {
                comboBoxCell.Items.Add(category.Name);
            }
            comboBoxCell.Sorted = true;
            dataGridView1.Rows[0].Cells["Element"] = comboBoxCell;
        }

        private void dataGridView1_CellChanged(object sender, DataGridViewCellEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;
            if (col == 1 && row >= 0)
            {
                
                string categoryName = dataGridView1.Rows[row].Cells[col].Value.ToString();
                List<string> parameterNames = Revit.RevitHelperFunctions.GetParameters(catagories.get_Item(categoryName));
                DataGridViewComboBoxCell comboBoxCell = new DataGridViewComboBoxCell();
                foreach (string name in parameterNames)
                {
                    comboBoxCell.Items.Add(name);
                }
                comboBoxCell.Sorted = true;
                dataGridView1.Rows[row].Cells["Output"] = comboBoxCell;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            Intech.NewParameterSync newParameterSync = new Intech.NewParameterSync();
            newParameterSync.ShowDialog();
        }
    }
}

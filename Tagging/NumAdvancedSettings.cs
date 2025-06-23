using Autodesk.Revit.DB;
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
    public partial class NumAdvancedSettings : System.Windows.Forms.Form
    {
        Category category = null;
        public NumAdvancedSettings(Category category)
        {
            InitializeComponent();
            this.category = category;
            List<string> p = Intech.Revit.RevitUtils.GetParameters(category);
            SmartCheckBox.Init(category.Name, p);
        }

        private void save_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(App.BasePath, "ReNumber.txt");
            string cat = category.Name;
            List<string> checkedItems = SmartCheckBox.GetCheckedItems();
            Intech.SaveFileManager saveFileManager = new Intech.SaveFileManager(filePath, new Intech.TxtFormat());
            Intech.SaveFileSection sec = new Intech.SaveFileSection(cat, "", "Matching Parameter");
            foreach (string item in checkedItems)
            {
                sec.Rows.Add(new string[]{item});
            }
            saveFileManager.AddOrUpdateSection(sec);
            this.Close();
        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Intech
{
    public partial class TagSetting : System.Windows.Forms.Form
    {
        CategoryNameMap categories;
        public TagSetting()
        {
            InitializeComponent();
            this.CenterToParent();

            //Get txt Path
            string path = Path.Combine(App.BasePath, "Settings.txt");
            Intech.SaveFileManager saveFileManager = new Intech.SaveFileManager(path, new TxtFormat());
            List<SaveFileSection> sections = saveFileManager.ReadAllSections();
            SaveFileSection section = saveFileManager.GetSectionsByName("Tag Settings").FirstOrDefault();
            TagSettings.ConfigureColumnTypes(new Dictionary<string, Intech.Windows.Forms.ColumnType> 
            {
                { "Tag Type", Intech.Windows.Forms.ColumnType.ComboBox },
                { "Category", Intech.Windows.Forms.ColumnType.ComboBox },
                { "Path", Intech.Windows.Forms.ColumnType.FilePicker },
                { "Leader", Windows.Forms.ColumnType.CheckBox }
            });
            TagSettings.Initialize(saveFileManager, section);

            categories = Intech.Revit.RevitUtils.GetAllCategories();
            List<string> catData = new List<string>();
            foreach (Category cat in categories)
            {
                catData.Add(cat.Name);
            }
            TagSettings.SetComboBoxItems("Category", catData);
            TagSettings.SetComboBoxItems("Tag Type", new List<string>
            {
                "Size",
                "Length",
                "Elevation",
                "Offset",
                "Number",
                "Hanger",
                "Tag1",
                "Tag2",
                "Tag3",
                "Tag4",
                "Tag5",
                "Tag6",
                "Tag7",
                "Tag8",
                "Tag9",
                "Tag10"
            });
        }

        private void dataGridView1_SelectionChange(object sender, EventArgs e)
        {
            
        }

        private void Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog Browser = new SaveFileDialog();
            Browser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            Browser.FilterIndex = 1; // Set the default filter to txt files
            Browser.DefaultExt = ".txt"; // Set the default extension

            if (Browser.ShowDialog(this) == DialogResult.OK)
            {
                string Folder = Browser.FileName;
                using (FileStream fs = File.Create(Folder))
                {

                }
                //System.IO.File.WriteAllText(Folder + @"\Tag Setting Export.txt", header + data);
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog Browser = new OpenFileDialog();

            if (Browser.ShowDialog(this) == DialogResult.OK)
            {
                string importpath = Browser.FileName;
                File.Copy(importpath, Path.Combine(App.BasePath, "Tag Settings.txt"), true);
            }
            this.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            TagSettings.Confirm();
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close(); //Closes Form
        }
    }
}

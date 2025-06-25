using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Net.Security;

namespace Intech
{
    public partial class TagSetting : Form
    {
        public TagSetting()
        {
            InitializeComponent();
            this.CenterToParent();

            //Get txt Path
            string path = Path.Combine(App.BasePath, "Settings.txt");
            Intech.SaveFileManager saveFileManager = new Intech.SaveFileManager(path, new TxtFormat());
            List<SaveFileSection> sections = saveFileManager.ReadAllSections();
            SaveFileSection section = saveFileManager.GetSectionsByName("Tag Settings").FirstOrDefault();
            TagSettings.Initialize(saveFileManager, section);
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

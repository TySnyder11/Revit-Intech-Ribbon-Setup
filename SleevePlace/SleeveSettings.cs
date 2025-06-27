using Autodesk.Revit.DB;
using Autodesk.Windows.ToolBars;
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

namespace Intech.Sleeve
{
    public partial class SleeveSettings : System.Windows.Forms.Form
    {
        SaveFileManager saveFileManager = null;
        public SleeveSettings()
        {
            InitializeComponent();
            CenterToParent();
            Save.Click += Save_Click;
            Cancel.Click += Cancel_Click;
            this.Text = "Sleeve Settings";
            List<RevitLinkInstance> links = Intech.Revit.RevitUtils.GetLinkedModels();
            List<string> linkNames = links.Select(x => x.Name).ToList();
            linkNames.Add("Current");
            linkNames.Sort();
            structCombo.DataSource = linkNames;

            string basePath = Path.Combine(App.BasePath, "Settings.txt");
            saveFileManager = new SaveFileManager(basePath);
            SaveFileSection section = saveFileManager.GetSectionsByName("Sleeve Place", "linked Model") ?? 
                new SaveFileSection("Sleeve Place", "linked Model", "Selected link name");
            if (section.Rows.Count() > 0 && section.Rows[0].Count() > 0 && linkNames.Contains(section.Rows[0][0]))
            {
                structCombo.Text = section.Rows[0][0];
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileSection section = new SaveFileSection("Sleeve Place", "linked Model", "Selected link name");
            section.Rows.Add(new string[] { structCombo.Text });
            saveFileManager.AddOrUpdateSection(section);
            this.Close();
        }
    }
}

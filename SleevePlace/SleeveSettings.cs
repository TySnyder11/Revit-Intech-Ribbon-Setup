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
        List<Family> fams = null;
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

            fams = Intech.Revit.RevitUtils.GetFamilies();
            List<string> famName = fams.Select(f => f.Name).ToList();
            famName.Sort();
            RoundSleeveFamilySelect.SetItems(famName);
            RectSleeveFamilySelect.SetItems(famName);

            string basePath = Path.Combine(App.BasePath, "Settings.txt");
            saveFileManager = new SaveFileManager(basePath);
            SaveFileSection section = saveFileManager.GetSectionsByName("Sleeve Place", "linked Model") ?? 
                new SaveFileSection("Sleeve Place", "linked Model", "Selected link name");
            if (section.Rows.Count() > 0 && section.Rows[0].Count() > 0 && linkNames.Contains(section.Rows[0][0]))
            {
                structCombo.Text = section.Rows[0][0];
            }

            SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sleeve Place", "Sleeve Family") ??
                new SaveFileSection("Sleeve Place", "Sleeve Family", "Selected family name");
            if (saveFileSection.Rows.Count() > 0 && saveFileSection.Rows[0].Count() > 0 && famName.Contains(saveFileSection.Rows[0][0]))
            {
                RoundSleeveFamilySelect.Text = saveFileSection.Rows[0][0];
                Family selectedFamily = fams.FirstOrDefault(f => f.Name == saveFileSection.Rows[0][0]);
                List<FamilySymbol> syms = Intech.Revit.RevitUtils.GetFamilySymbols(selectedFamily);
                List<string> symNames = syms.Select(s => s.Name).ToList();
                symNames.Sort();
                RoundTypeFamilySelect.SetItems(symNames);
            }
            if (saveFileSection.Rows.Count() > 1 && saveFileSection.Rows[0].Count() > 0 && linkNames.Contains(saveFileSection.Rows[1][0]))
            {
                RoundSleeveFamilySelect.SelectedValue = saveFileSection.Rows[1][0];
            }
            if (saveFileSection.Rows.Count() > 0 && saveFileSection.Rows[0].Count() > 1 && famName.Contains(saveFileSection.Rows[0][1]))
            {
                RectSleeveFamilySelect.Text = saveFileSection.Rows[0][1];
                Family selectedFamily = fams.FirstOrDefault(f => f.Name == saveFileSection.Rows[0][0]);
                List<FamilySymbol> syms = Intech.Revit.RevitUtils.GetFamilySymbols(selectedFamily);
                List<string> symNames = syms.Select(s => s.Name).ToList();
                symNames.Sort();
                RectTypeFamilySelect.SetItems(symNames);
            }
            if (saveFileSection.Rows.Count() > 1 && saveFileSection.Rows[0].Count() > 1 && linkNames.Contains(saveFileSection.Rows[1][1]))
            {
                RectSleeveFamilySelect.SelectedValue = saveFileSection.Rows[1][1];
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

            SaveFileSection saveFileSection = new SaveFileSection("Sleeve Place", "Sleeve Family", "Selected family name");
            saveFileSection.Rows.Add(new string[] { RoundSleeveFamilySelect.Text, RectSleeveFamilySelect.Text });
            saveFileSection.Rows.Add(new string[] { RoundTypeFamilySelect.Text, RectTypeFamilySelect.Text });
            saveFileManager.AddOrUpdateSection(saveFileSection);
            this.Close();
        }

        private void SleeveFamilySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string famName = RoundSleeveFamilySelect.Text;
            Family selectedFamily = fams.FirstOrDefault(f => f.Name == famName);
            List<FamilySymbol> syms = Intech.Revit.RevitUtils.GetFamilySymbols(selectedFamily);
            List<string> symNames = syms.Select(s => s.Name).ToList();
            symNames.Sort();
            RoundTypeFamilySelect.SetItems(symNames);
        }

        private void RectSleeveFamilySelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            string famName = RectSleeveFamilySelect.Text;
            Family selectedFamily = fams.FirstOrDefault(f => f.Name == famName);
            List<FamilySymbol> syms = Intech.Revit.RevitUtils.GetFamilySymbols(selectedFamily);
            List<string> symNames = syms.Select(s => s.Name).ToList();
            symNames.Sort();
            RectTypeFamilySelect.SetItems(symNames);
        }
    }
}

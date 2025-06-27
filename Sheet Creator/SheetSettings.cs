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
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Security.Principal;

namespace Intech
{
    public partial class SheetSettings : System.Windows.Forms.Form
    {
        private string projectName = string.Empty;

        SaveFileManager saveFileManager = null;

        public SheetSettings()
        {
            InitializeComponent();
            this.CenterToParent();
            projectName = Intech.Revit.RevitUtils.projectName();

            var titleBlocks = Intech.Revit.RevitUtils.GetAllTitleBlockFamilies();
            List<string> titleBlockNames = titleBlocks.ConvertAll(f => f.Name);
            TitleBlockFamily.SetItems(titleBlockNames);

            //Get txt Path
            string BasePath = Path.Combine(App.BasePath, "Settings.txt");
            saveFileManager = new SaveFileManager(BasePath);
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Sheet Creator Base Settings");

                TradeAbriviation.Text = saveFileSection.Rows[0][0];
                MiddleSheetNumber.Text = saveFileSection.Rows[0][1];

                List<string> TitleBlockTypes = new List<string>();

                string titleBlockFamilyValue = string.Empty;
                if (saveFileSection.Rows.Count > 0 && saveFileSection.Rows[0].Length > 2)
                {
                    titleBlockFamilyValue = saveFileSection.Rows[0][2] ?? string.Empty;
                }
                if (titleBlockNames.Contains(titleBlockFamilyValue))
                {
                    TitleBlockFamily.Text = titleBlockFamilyValue;
                    if (TitleBlockFamily.SelectedItem != null)
                    {
                        TitleBlockType.SetItems(Intech.Revit.RevitUtils.GetTitleBlockTypesFromFamily(titleBlockFamilyValue).ConvertAll(f => f.Name));
                    }
                }

                string titleBlockTypeValue = string.Empty;
                if (saveFileSection.Rows.Count > 0 && saveFileSection.Rows[0].Length > 3)
                {
                    titleBlockTypeValue = saveFileSection.Rows[0][3] ?? string.Empty;
                }
                if (TitleBlockType.Items.Contains(titleBlockTypeValue))
                {
                    TitleBlockType.Text = titleBlockTypeValue;
                }
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Nonstandard Level Info");
                LevelGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Nonstandard Scopebox Info");
                AreaGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Sheet Sub Discipline");
                SubDisciplineGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Sheet Discipline");
                DisciplineGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Scale");
                ScaleGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Sub Discipline check");
                SubDisciplineCheck.Checked = saveFileSection.Rows[0][0] == "True";
            }

        }

        private void Import_Click(object sender, EventArgs e)
        {

            var result = MessageBox.Show(
                 "Do you want to save your current settings before importing a new file?",
                 "Save Current Settings",
                 MessageBoxButtons.YesNoCancel,
                 MessageBoxIcon.Question);


            if (result == DialogResult.Cancel)
            {
                return;
            }
            else if (result == DialogResult.Yes)
            {
                Export_Click(sender, e);
            }


            OpenFileDialog Browser = new OpenFileDialog();
            Browser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            Browser.FilterIndex = 1;
            Browser.DefaultExt = ".txt";
            if (Browser.ShowDialog(this) == DialogResult.OK)
            {
                SaveFileManager import = new SaveFileManager(Browser.FileName);
                {
                    SaveFileSection saveFileSection = import.GetSectionsByName("Sheet Settings", "Sheet Creator Base Settings");

                    TradeAbriviation.Text = saveFileSection.Rows[0][0];
                    MiddleSheetNumber.Text = saveFileSection.Rows[0][1];
                    TitleBlockFamily.SelectedItem = saveFileSection.Rows[0][2];
                    TitleBlockType.SelectedItem = saveFileSection.Rows[0][3];
                }
                {
                    SaveFileSection saveFileSection = import.GetSectionsByName("Sheet Settings", "Nonstandard Level Info");
                    LevelGrid.Initialize(saveFileManager, saveFileSection);
                }
                {
                    SaveFileSection saveFileSection = import.GetSectionsByName("Sheet Settings", "Nonstandard Scopebox Info");
                    AreaGrid.Initialize(saveFileManager, saveFileSection);
                }
                {
                    SaveFileSection saveFileSection = import.GetSectionsByName("Sheet Settings", "Sheet Sub Discipline");
                    SubDisciplineGrid.Initialize(saveFileManager, saveFileSection);
                }
                {
                    SaveFileSection saveFileSection = import.GetSectionsByName("Sheet Settings", "Sheet Discipline");
                    DisciplineGrid.Initialize(saveFileManager, saveFileSection);
                }
                {
                    SaveFileSection saveFileSection = import.GetSectionsByName("Sheet Settings", "Scale");
                    ScaleGrid.Initialize(saveFileManager, saveFileSection);
                }
                {
                    SaveFileSection saveFileSection = import.GetSectionsByName("Sheet Settings", "Sub Discipline check");
                    SubDisciplineCheck.Checked = saveFileSection.Rows[0][0] == "True";
                }

            }
            this.Close();
        }

        private void Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog Browser = new SaveFileDialog();
            Browser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            Browser.FilterIndex = 1;
            Browser.DefaultExt = ".txt";
            if (Browser.ShowDialog(this) == DialogResult.OK)
            {
                SaveFileManager export = new SaveFileManager(Browser.FileName);
                export.AddOrUpdateSection(LevelGrid.GetSaveFileSection());
                export.AddOrUpdateSection(AreaGrid.GetSaveFileSection());
                export.AddOrUpdateSection(SubDisciplineGrid.GetSaveFileSection());
                export.AddOrUpdateSection(DisciplineGrid.GetSaveFileSection());
                export.AddOrUpdateSection(ScaleGrid.GetSaveFileSection());
                SaveFileSection saveFileSection = new SaveFileSection("Sheet Settings", "Sheet Creator Base Settings",
                    "Trade Abbreviation\tMiddle Sheet Number\tTitleBlockFamily\tTitleBlockType");
                saveFileSection.Rows.Add(new string[] { TradeAbriviation.Text, MiddleSheetNumber.Text, 
                    TitleBlockFamily.SelectedItem?.ToString() ?? string.Empty, TitleBlockType.SelectedItem?.ToString() ?? string.Empty });
                SaveFileSection subDisciplineCheck = new SaveFileSection("Sheet Settings", "Sub Discipline check", "bool");
                subDisciplineCheck.Rows.Add(new string[] { SubDisciplineCheck.Checked.ToString() });
                export.AddOrUpdateSection(saveFileSection);
                export.AddOrUpdateSection(subDisciplineCheck);
            }
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            {
                SaveFileSection saveFileSection = new SaveFileSection("Sheet Settings", "Sheet Creator Base Settings", 
                    "Trade Abbreviation\tMiddle Sheet Number\tTitleBlockFamily\tTitleBlockType");
                saveFileSection.Rows.Add(new string[] { TradeAbriviation.Text, MiddleSheetNumber.Text,
                    TitleBlockFamily.SelectedItem?.ToString() ?? string.Empty, TitleBlockType.SelectedItem?.ToString() ?? string.Empty });
                saveFileManager.AddOrUpdateSection(saveFileSection);
            }
            LevelGrid.Confirm();
            AreaGrid.Confirm();
            SubDisciplineGrid.Confirm();
            DisciplineGrid.Confirm();
            ScaleGrid.Confirm();
            {
                SaveFileSection saveFileSection = new SaveFileSection("Sheet Settings", "Sub Discipline check", "bool");
                saveFileSection.Rows.Add(new string[] { SubDisciplineCheck.Checked.ToString() });
                saveFileManager.AddOrUpdateSection(saveFileSection);
            }
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TitleBlockType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BaseControlTab_Click(object sender, EventArgs e)
        {

        }

        private void TitleBlockFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TitleBlockFamily.SelectedItem != null)
            {
                TitleBlockType.SetItems(Intech.Revit.RevitUtils.GetTitleBlockTypesFromFamily(TitleBlockFamily.SelectedItem.ToString()).ConvertAll(f => f.Name));
            }
            else
            {
                TitleBlockType.SetItems(new List<string>());
            }
        }

        private void ScaleGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

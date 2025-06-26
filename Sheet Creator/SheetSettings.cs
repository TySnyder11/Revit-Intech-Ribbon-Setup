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
            SaveFileManager saveFileManager = new SaveFileManager(BasePath);
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Sheet Creator Base Settings").FirstOrDefault();

                TradeAbriviation.Text = saveFileSection.Rows[0][0];
                MiddleSheetNumber.Text = saveFileSection.Rows[0][1];
                TitleBlockFamily.SelectedItem = saveFileSection.Rows[0][2];
                TitleBlockType.SelectedItem = saveFileSection.Rows[0][3];
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Nonstandard Level Info").FirstOrDefault();
                LevelGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Nonstandard Scopebox Info").FirstOrDefault();
                AreaGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Sheet Sub Discipline").FirstOrDefault();
                SubDisciplineGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Sheet Discipline").FirstOrDefault();
                DisciplineGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Scale").FirstOrDefault();
                ScaleGrid.Initialize(saveFileManager, saveFileSection);
            }
            {
                SaveFileSection saveFileSection = saveFileManager.GetSectionsByName("Sheet Settings", "Sub Discipline check").FirstOrDefault();
                SubDisciplineCheck.Checked = saveFileSection.Rows[0][0] == "True";
            }

        }

        private void Import_Click(object sender, EventArgs e)
        {
            OpenFileDialog Browser = new OpenFileDialog();
            Browser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            Browser.FilterIndex = 1; // Set the default filter to txt files
            Browser.DefaultExt = ".txt"; // Set the default extension
            if (Browser.ShowDialog(this) == DialogResult.OK)
            {
                
            }
            this.Close();
        }

        private void Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog Browser = new SaveFileDialog();
            Browser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            Browser.FilterIndex = 1; // Set the default filter to txt files
            Browser.DefaultExt = ".txt"; // Set the default extension
            if (Browser.ShowDialog(this) == DialogResult.OK)
            {
                
            }
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            
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

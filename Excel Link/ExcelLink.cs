using Autodesk.Revit.DB;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Autodesk.Revit.DB.SpecTypeId;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Excel_Link
{
    public partial class ExcelLinkUI : System.Windows.Forms.Form
    {
        Transaction t;
        string[][] data = null;
        public ExcelLinkUI(Autodesk.Revit.DB.Transaction t)
        {
            InitializeComponent();
            this.t = t;

            loadSaveFile();
        }

        private void create_Click(object sender, EventArgs e)
        {
            AddLink addLinkForm = new AddLink(t);
            addLinkForm.ShowDialog(this);
            InfoGrid.Rows.Clear();
            loadSaveFile();
        }
        private void loadSaveFile()
        {
            data = Intech.linkUI.readSave();
            for (int i = 0; i < data.Length; i++)
            {
                string[] instance = data[i];
                if (Intech.linkUI.doc.Title == instance[0])
                {
                    string excelPath = instance[1];
                    string fileName = Path.GetFileNameWithoutExtension(excelPath);
                    string sheet = instance[2];
                    string area = instance[3];
                    string viewSheet = instance[4];
                    string lastUpdate = instance[5];
                    string user = instance[6];
                    DateTime lastImport = DateTime.ParseExact(lastUpdate, "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
                    string time = lastImport.ToString("g");
                    if (File.Exists(excelPath))
                    {
                        DateTime lastModified = File.GetLastWriteTime(excelPath);
                        if (lastModified < lastImport)
                        {
                            //file is up to date
                            appendInfoGrid(sheet, "Up to Date", time, fileName);
                        }
                        else
                        {
                            //file is out of date
                            appendInfoGrid(sheet, "Out of Date", time, fileName);
                        }
                    }
                    else
                    {
                        appendInfoGrid(sheet, "File not found", time, fileName);
                    }
                }
            }
        }

        private void appendInfoGrid(string name, string status, string lastUpdate, string fileName)
        {
            InfoGrid.Rows.Add(name, status, lastUpdate, fileName);
        }

        private void InfoGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            string[] selected = data[row];
            string excelPath = selected[1];
            string sheet = selected[2];
            string area = selected[3];
            string viewSheet = selected[4];
            string lastUpdate = selected[5];
            string user = selected[6];

            FolderTextBox.Text = Path.GetDirectoryName(excelPath);
            FileTextBox.Text = Path.GetFileNameWithoutExtension(excelPath);
            ScheduleTextBox.Text = sheet;
            AreaTextBox.Text = area;
            workSheetTextBox.Text = sheet;

            //get schedule
            t.Start();
            t.Commit();
            Document doc = Intech.linkUI.doc;
            ViewSheet elm = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet))
                .WhereElementIsNotElementType()
                .ToElements()
                .Cast<ViewSheet>()
                .FirstOrDefault(vs => (vs.SheetNumber + " - " + vs.Name) == viewSheet);
            if (elm != null)
            {
                ViewTextBox.Text = elm.SheetNumber + " - " + elm.Name;
            }
            else
            {
                ViewTextBox.Text = "Schedule not sunk. You or " + user + " needs to sink to populate.";
            }


                //get status

                DateTime lastImport = DateTime.ParseExact(lastUpdate, "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
            string status = "";
            if (File.Exists(excelPath))
            {
                DateTime lastModified = File.GetLastWriteTime(excelPath);
                if (lastModified < lastImport)
                {
                    status = "Up to Date";
                }
                else
                {
                    status = "Out of Date";
                }
            }
            else
            {
                status = "File not found." + "Might be in local drive of " + user + ".";
            }

            StatusTextBox.Text = status;
        }

        private void Up_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(StatusTextBox.Text))
            {
                t.Start("Update Schedule");
                Intech.Excel.Update(FolderTextBox.Text + FileTextBox.Text, 
                    ScheduleTextBox.Text,workSheetTextBox.Text,AreaTextBox.Text);
            }
        }
    }
}

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

            loadSaveFile();
        }

        List<(string, string, string)> extraInfo = new List<(string, string, string)>();
        private void loadSaveFile()
        {
            data = Intech.linkUI.readSave();
            for (int i = 1; i < data.Length; i++)
            {
                string[] instance = data[i];
                if (Intech.linkUI.doc.Title == instance[0])
                {
                    string excelPath = instance[1];
                    string fileName = Path.GetFileNameWithoutExtension(excelPath);
                    string sheet = instance[2];
                    string area = instance[3];
                    string lastUpdate = instance[4];
                    string user = instance[5];
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
        }
    }
}

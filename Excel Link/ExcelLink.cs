using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

namespace TitleBlockSetup.Excel_Import
{
    public partial class ExcelLinkUI : Form
    {
        public ExcelLinkUI()
        {
            InitializeComponent();
        }

        private void updateScreenInfo()
        {

        }

        private void create_Click(object sender, EventArgs e)
        {
            string file = Intech.Excel.getExcelFile();
            if (file == null)
            {
                MessageBox.Show("No file selected.");
                return;
            }


        }
    }
}

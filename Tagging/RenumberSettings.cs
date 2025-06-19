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

namespace TitleBlockSetup.Tagging
{
    public partial class RenumberSettings : Form
    {
        public RenumberSettings()
        {
            InitializeComponent();
            InitSectionGrid();
        }

        private void InitSectionGrid()
        {
            string filePath = Path.Combine(App.BasePath, "ReNumber.txt");
            SaveFileManager saveFileManager = new SaveFileManager(filePath, new TxtFormat());

            SaveFileSection sec =  saveFileManager.GetSectionsByProject("__General__").FirstOrDefault()?? 
                new SaveFileSection("__General__", "", "Category\tTag\tprefix\tCurrent Number\tSuffix\tSeperator");

            Dictionary<string, Intech.Windows.Forms.ColumnType> columDictionary = new Dictionary<string, Intech.Windows.Forms.ColumnType> 
            {
                {"Category", Intech.Windows.Forms.ColumnType.ComboBox },
                { "Tag" , Intech.Windows.Forms.ColumnType.CheckBox }
            };
            renumberMenu.ConfigureColumnTypes(columDictionary);
            renumberMenu.Initialize(saveFileManager, sec);
        }
    }
}

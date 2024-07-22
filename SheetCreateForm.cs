using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Intech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech
{
    public partial class SheetCreateForm : System.Windows.Forms.Form
    {
        Dictionary<string, List<Element>> titleblockFamily = new Dictionary<string, List<Element>>();
        public SheetCreateForm(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;
            Transaction trans = new Transaction(doc);
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            InitializeComponent();
            this.CenterToParent();

            DataTable dt = new DataTable();

            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("Checked", typeof(bool));

            List<Element> planViews = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewPlan))
                .WhereElementIsNotElementType()
                .ToElements() as List<Element>;

            foreach (var item in planViews) dt.Rows.Add(item.Name, false);

            dt.AcceptChanges();

            PlanViewCheckList.DataSource = dt.DefaultView;
            PlanViewCheckList.DisplayMember = "Item";
            PlanViewCheckList.ValueMember = "Item";

            PlanViewCheckList.ItemCheck += PlanViewCheckList_ItemCheck;

            var baseInput = SettingsRead.BaseControls();

            if (baseInput.tradeAbbreviation != "") TradeAbriviation.Text = baseInput.tradeAbbreviation;
            if (baseInput.SheetNumber != "") MiddleSheetNumber.Text = baseInput.SheetNumber;
            if (baseInput.titleBlockFamily != "") TitleBlockFamily.Text = baseInput.titleBlockFamily;
            if (baseInput.titleBlockType != "") TitleBlockType.Text = baseInput.titleBlockType;

            List<Element> titleblocktypes = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .WhereElementIsElementType()
                .ToElements() as List<Element>;
            
            foreach (FamilySymbol i in titleblocktypes)
            {
                if (!titleblockFamily.Keys.Contains(i.FamilyName))
                {
                    titleblockFamily.Add(i.FamilyName, new List<Element>());
                    titleblockFamily[i.FamilyName].Add(i);
                }
                else if (titleblockFamily.Keys.Contains(i.FamilyName) && !titleblockFamily[i.FamilyName].Contains(i))
                {
                    titleblockFamily[i.FamilyName].Add(i);
                }
            }

            foreach (String i in titleblockFamily.Keys)
                TitleBlockFamily.Items.Add(i);


            if (TitleBlockFamily.Text != "")
                foreach (Element i in titleblockFamily[TitleBlockFamily.Text])
                {
                    TitleBlockType.Items.Add(i.Name);
                    if (TitleBlockType.Text.Contains(i.Name))
                        foreach (Parameter p in i.Parameters)
                        {
                            checkedListBox2.Items.Add(p.Element.Name);
                        }
                }
        }
        
        private void PlanViewCheckList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var dv = PlanViewCheckList.DataSource as DataView;
            var drv = dv[e.Index];
            drv["Checked"] = e.NewValue == CheckState.Checked ? true : false;
            
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            var dv = PlanViewCheckList.DataSource as DataView;
            string filter = "";
            if (SearchBox.Text.Trim().Length > 0)
            {
                //filter = $"Item LIKE '{textBox1.Text}*'";
                foreach (DataRow i in dv.Table.Rows)
                {
                    object[] Items = i.ItemArray;
                    string name = Items[0].ToString();
                    if (name.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        filter = filter + $"(Item LIKE '{name}*') OR ";
                }
                if (filter == "")
                {
                    filter = $"(Item LIKE 'doesnotexcist*')";
                }
                else
                {
                    int length = filter.Count() - 4;
                    filter = filter.Substring(0, length);
                }
            }
            else
            {
                filter = null;
            }

            dv.RowFilter = filter;

            for (var i = 0; i < PlanViewCheckList.Items.Count; i++)
            {
                var drv = PlanViewCheckList.Items[i] as DataRowView;
                var chk = Convert.ToBoolean(drv["Checked"]);
                PlanViewCheckList.SetItemChecked(i, chk);
            }
        }

        private void TitleBlockFamily_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

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
        Document doc;
        public SheetCreateForm(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            doc = uiapp.ActiveUIDocument.Document;
            Transaction trans = new Transaction(doc);
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            InitializeComponent();
            this.CenterToParent();

            DataTable dt = new DataTable();

            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("Checked", typeof(bool));

            var planViews = GetViewsNotOnSheets(doc);

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

            Transaction temp = new Transaction(doc, "Temp");
            if (TitleBlockFamily.Text != "")
                foreach (Element i in titleblockFamily[TitleBlockFamily.Text])
                {
                    TitleBlockType.Items.Add(i.Name);
                    if (TitleBlockType.Text.Contains(i.Name))
                    {
                        temp.Start();
                        ViewSheet sheet = ViewSheet.Create(doc, i.Id);

                        var title_block = new FilteredElementCollector(doc, sheet.Id)
                            .OfCategory(BuiltInCategory.OST_TitleBlocks)
                            .WhereElementIsNotElementType()
                            .ToElements();

                        foreach(Parameter p in title_block[0].GetOrderedParameters())
                            if (p.StorageType == StorageType.Integer)
                            ParameterCheckList.Items.Add(p.Definition.Name);

                        temp.RollBack();
                    }
                }

            var levels = new FilteredElementCollector(doc)
                            .OfClass(typeof(Level))
                            .WhereElementIsNotElementType()
                            .ToElements();
            foreach (Level level in levels)
                LevelOverrideComboBox.Items.Add(level.Name);

            var areas = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_VolumeOfInterest)
                            .WhereElementIsNotElementType()
                            .ToElements();
            foreach (Element area in areas)
                AreaOverrideComboBox.Items.Add(area.Name);
        }

        public static IEnumerable<Autodesk.Revit.DB.ViewPlan> GetViewsNotOnSheets(Document doc)
        {
            //  Get all sheets
            IEnumerable<ViewSheet> sheets = new FilteredElementCollector(doc)
                        .OfClass(typeof(ViewSheet))
                        .Cast<ViewSheet>()
                        .Where(x => !x.IsTemplate);

            //  Get all views placed on a sheet
            HashSet<ElementId> viewsOnSheets = new HashSet<ElementId>(sheets.SelectMany(x => x.GetAllPlacedViews()));

            //  Return the views that aren't placed on a sheet
            IEnumerable<Autodesk.Revit.DB.ViewPlan> views = new FilteredElementCollector(doc)
                        .OfClass(typeof(ViewPlan))
                        .Cast<ViewPlan>()
                        .Where(x => !x.IsTemplate);
            foreach (Autodesk.Revit.DB.ViewPlan view in views)
            {
                if (!viewsOnSheets.Contains(view.Id))
                    yield return view;
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
            TitleBlockType.Items.Clear();
            TitleBlockType.Text = "";
            ParameterCheckList.Items.Clear();
            Transaction temp = new Transaction(doc, "Temp");
            if (TitleBlockFamily.Text != "")
                foreach (Element i in titleblockFamily[TitleBlockFamily.Text])
                {
                    TitleBlockType.Items.Add(i.Name);
                    if (TitleBlockType.Text.Contains(i.Name))
                    {
                        temp.Start();
                        ViewSheet sheet = ViewSheet.Create(doc, i.Id);

                        var title_block = new FilteredElementCollector(doc, sheet.Id)
                            .OfCategory(BuiltInCategory.OST_TitleBlocks)
                            .WhereElementIsNotElementType()
                            .ToElements();

                        foreach (Parameter p in title_block[0].GetOrderedParameters())
                            ParameterCheckList.Items.Add(p.Definition.Name);

                        temp.RollBack();
                    }
                }
        }

        private void TitleBlockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParameterCheckList.Items.Clear();
            Transaction temp = new Transaction(doc, "Temp");
            if (TitleBlockFamily.Text != "")
                foreach (Element i in titleblockFamily[TitleBlockFamily.Text])
                    if (TitleBlockType.Text.Contains(i.Name))
                    {
                        temp.Start();
                        ViewSheet sheet = ViewSheet.Create(doc, i.Id);

                        var title_block = new FilteredElementCollector(doc, sheet.Id)
                            .OfCategory(BuiltInCategory.OST_TitleBlocks)
                            .WhereElementIsNotElementType()
                            .ToElements();

                        foreach (Parameter p in title_block[0].GetOrderedParameters())
                            ParameterCheckList.Items.Add(p.Definition.Name);

                        temp.RollBack();
                    }
        }

        private void LevelOverride_CheckedChanged(object sender, EventArgs e)
        {
            LevelOverrideComboBox.Visible = LevelOverride.Checked;
        }

        private void AreaOverride_CheckedChanged(object sender, EventArgs e)
        {
            AreaOverrideComboBox.Visible = AreaOverride.Checked;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            PlanViewCheckList.Items.Clear();
            this.Close();
        }

        private void Create_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SheetCreateForm_Load(object sender, EventArgs e)
        {

        }
    }
}

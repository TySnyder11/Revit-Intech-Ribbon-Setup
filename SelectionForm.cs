using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Data;


namespace Intech
{
    public partial class SelectionForm : System.Windows.Forms.Form
    {
        public DataTable dt = new DataTable();

        public SelectionForm(List<string> list)
        {
            InitializeComponent();
            this.CenterToParent();


            DataTable dt = new DataTable();

            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("Checked", typeof(bool));


            foreach (var item in list) dt.Rows.Add(item, false);

            dt.AcceptChanges();

            checkedListBox.DataSource = dt.DefaultView;
            checkedListBox.DisplayMember = "Item";
            checkedListBox.ValueMember = "Item";

            checkedListBox.ItemCheck += checkedListBox_ItemCheck;

        }
        protected void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = checkedListBox.SelectedIndex;
            checkedListBox.SetItemChecked(index, !checkedListBox.GetItemChecked(index));
        }

        protected void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var dv = checkedListBox.DataSource as DataView;
            var drv = dv[e.Index];
            drv["Checked"] = e.NewValue == CheckState.Checked ? true : false;
        }

        private void checkAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, true);
            }
        }

        private void uncheckAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, false);
            }
        }

        private void toggle_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, !checkedListBox.GetItemChecked(i));
            }
        }

        public void saveAS_Click(object sender, EventArgs e)//this is the save buttom
        {
            textBox1.Text = "";
            this.Close(); //just closing the form
        }

        public void SearchBar_TextChanged(object sender, EventArgs e)
        {
            var dv = checkedListBox.DataSource as DataView;
            string filter;
            Debug.WriteLine(textBox1.Text.Trim().Length);
            if (textBox1.Text.Trim().Length > 0)
            {
                filter = $"Item LIKE '{textBox1.Text}*'";
            }
            else
            {
                filter = null;
            }

            dv.RowFilter = filter;

            for (var i = 0; i < checkedListBox.Items.Count; i++)
            {
                var drv = checkedListBox.Items[i] as DataRowView;
                var chk = Convert.ToBoolean(drv["Checked"]);
                checkedListBox.SetItemChecked(i, chk);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

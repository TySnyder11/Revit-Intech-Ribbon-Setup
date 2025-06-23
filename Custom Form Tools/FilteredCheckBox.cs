using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Intech.Windows.Forms
{
    public partial class FilteredCheckBox : UserControl
    {

        public event EventHandler<string> SearchTextChanged;
        public event EventHandler<List<string>> CheckedItemsChanged;

        public FilteredCheckBox()
        {
            InitializeComponent();

            // Wire up events
            SearchBox.TextChanged += SearchBox_TextChanged;
            checkedListBox.ItemCheck += CheckedListBox_ItemCheck;
        }

        public void Init(string name, List<string> items)
        {
            NameTextBox.Text = name;
            checkedListBox.AllItems = items;
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            checkedListBox.Filter = SearchBox.Text;
            SearchTextChanged?.Invoke(this, SearchBox.Text);
        }

        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Delay event until after the check state has changed
            this.BeginInvoke(new Action(() =>
            {
                CheckedItemsChanged?.Invoke(this, checkedListBox.GetCheckedItems());
            }));
        }

        public List<string> GetCheckedItems()
        {
            return checkedListBox.GetCheckedItems();
        }

        public void SetCheckedItems(IEnumerable<string> items)
        {
            checkedListBox.SetCheckedItems(items);
        }
    }
}

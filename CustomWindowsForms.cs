using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech.Windows
{
    internal class CustomWindowsForms
    {
        public class FilterableTreeView : TreeView
        {
            private Dictionary<string, List<string>> _data = new Dictionary<string, List<string>>();
            private HashSet<string> _checkedItems = new HashSet<string>();

            public FilterableTreeView()
            {
                this.CheckBoxes = true;
                this.AfterCheck += FilterableTreeView_AfterCheck;
            }

            public void SetData(Dictionary<string, List<string>> data)
            {
                _data = data;
                RebuildTree();
            }

            public void ApplyFilter(string filter)
            {
                RebuildTree(filter);
            }

            private void RebuildTree(string filter = "")
            {
                this.BeginUpdate();
                this.Nodes.Clear();

                foreach (var kvp in _data)
                {
                    string root = kvp.Key;

                    if (!string.IsNullOrEmpty(filter) && !root.ToLower().Contains(filter.ToLower()))
                        continue;

                    TreeNode rootNode = new TreeNode(root)
                    {
                        Checked = _checkedItems.Contains(root)
                    };

                    foreach (var child in kvp.Value)
                    {
                        string fullKey = $"{root}|{child}";
                        TreeNode childNode = new TreeNode(child)
                        {
                            Checked = _checkedItems.Contains(fullKey)
                        };
                        rootNode.Nodes.Add(childNode);
                    }

                    this.Nodes.Add(rootNode);
                }

                this.EndUpdate();
            }


            private void FilterableTreeView_AfterCheck(object sender, TreeViewEventArgs e)
            {
                // Temporarily detach to prevent recursion
                this.AfterCheck -= FilterableTreeView_AfterCheck;

                // Your logic here (previously in OnAfterCheck)
                if (e.Node.Parent == null)
                {
                    string rootKey = e.Node.Text;

                    if (e.Node.Checked)
                        _checkedItems.Add(rootKey);
                    else
                        _checkedItems.Remove(rootKey);

                    foreach (TreeNode child in e.Node.Nodes)
                    {
                        child.Checked = e.Node.Checked;
                        string fullKey = $"{rootKey}|{child.Text}";
                        if (e.Node.Checked)
                            _checkedItems.Add(fullKey);
                        else
                            _checkedItems.Remove(fullKey);
                    }
                }
                else
                {
                    string rootKey = e.Node.Parent.Text;
                    string fullKey = $"{rootKey}|{e.Node.Text}";

                    if (e.Node.Checked)
                        _checkedItems.Add(fullKey);
                    else
                        _checkedItems.Remove(fullKey);

                    bool allChecked = e.Node.Parent.Nodes.Cast<TreeNode>().All(n => n.Checked);
                    e.Node.Parent.Checked = allChecked;

                    if (allChecked)
                        _checkedItems.Add(rootKey);
                    else
                        _checkedItems.Remove(rootKey);
                }

                this.AfterCheck += FilterableTreeView_AfterCheck;
            }

            public List<string> GetCheckedItems()
            {
                return _checkedItems.ToList();
            }

            private const int WM_LBUTTONDBLCLK = 0x0203;
            private const int WM_LBUTTONDOWN = 0x0201;

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_LBUTTONDBLCLK)
                {
                    // Convert double-click into two single clicks
                    Message singleClick = Message.Create(m.HWnd, WM_LBUTTONDOWN, m.WParam, m.LParam);
                    base.WndProc(ref singleClick);
                    return;
                }

                base.WndProc(ref m);
            }
        }


        public class FilterableCheckedListBox : CheckedListBox
        {
            private List<string> _allItems = new List<string>();
            private HashSet<string> _checkedItems = new HashSet<string>();
            private string _filter = "";
            private int _lastClickedIndex = -1;
            private bool _isShiftClick = false;

            [Category("Data")]
            [Description("The full list of items to display and filter.")]
            public List<string> AllItems
            {
                get => _allItems;
                set
                {
                    _allItems = value ?? new List<string>();
                    ApplyFilter();
                }
            }

            [Category("Behavior")]
            [Description("The current filter string used to filter visible items.")]
            public string Filter
            {
                get => _filter;
                set
                {
                    _filter = value ?? "";
                    ApplyFilter();
                }
            }

            public FilterableCheckedListBox()
            {
                this.CheckOnClick = true;
                this.SelectionMode = SelectionMode.One;
                this.ItemCheck += OnItemCheck;
                this.MouseDown += OnMouseDown;
            }

            private void OnMouseDown(object sender, MouseEventArgs e)
            {
                int index = this.IndexFromPoint(e.Location);
                if (index >= 0 && index < this.Items.Count)
                {
                    _isShiftClick = (ModifierKeys & Keys.Shift) == Keys.Shift;
                }
            }


            private void OnItemCheck(object sender, ItemCheckEventArgs e)
            {
                string item = this.Items[e.Index].ToString();

                if (_isShiftClick && _lastClickedIndex != -1 && _lastClickedIndex != e.Index)
                {
                    int start = Math.Min(_lastClickedIndex, e.Index);
                    int end = Math.Max(_lastClickedIndex, e.Index);
                    bool newState = e.NewValue == CheckState.Checked;

                    // Temporarily detach to avoid recursion
                    this.ItemCheck -= OnItemCheck;

                    for (int i = start; i <= end; i++)
                    {
                        string rangeItem = this.Items[i].ToString();
                        this.SetItemChecked(i, newState);

                        if (newState)
                            _checkedItems.Add(rangeItem);
                        else
                            _checkedItems.Remove(rangeItem);
                    }

                    this.ItemCheck += OnItemCheck;

                    // Cancel the default toggle for the current item
                    e.NewValue = this.GetItemChecked(e.Index) ? CheckState.Checked : CheckState.Unchecked;
                }
                else
                {
                    if (e.NewValue == CheckState.Checked)
                        _checkedItems.Add(item);
                    else
                        _checkedItems.Remove(item);
                }

                _lastClickedIndex = e.Index;
                _isShiftClick = false;
            }


            private void ApplyFilter()
            {
                var filtered = _allItems
                .Where(i => i.IndexOf(_filter, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

                this.Items.Clear();

                foreach (var item in filtered)
                {
                    this.Items.Add(item, _checkedItems.Contains(item));
                }
            }

            public List<string> GetCheckedItems() => _checkedItems.ToList();

            public void SetCheckedItems(IEnumerable<string> items)
            {
                _checkedItems = new HashSet<string>(items ?? Enumerable.Empty<string>());
                ApplyFilter();
            }
        }
    }
}

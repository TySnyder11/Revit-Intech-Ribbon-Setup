using Autodesk.Revit.DB;
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

        [DesignerCategory("Code")]
        public class FilteredComboBox : FilteredComboBox<object>
        {
            public FilteredComboBox() : base() { }
        }

        public class FilteredComboBox<T> : ComboBox
        {
            protected class ItemWrapper
            {
                public string Display { get; set; }
                public T Value { get; set; }

                public override string ToString() => Display;
            }

            protected List<ItemWrapper> _items = new List<ItemWrapper>();
            protected ItemWrapper _selectedItem;
            protected bool _suppressSelectionChange = false;

            public T DefaultValue { get; set; } = default;

            public FilteredComboBox()
            {
                this.DropDownStyle = ComboBoxStyle.DropDown;
                this.AutoCompleteMode = AutoCompleteMode.None;
                this.AutoCompleteSource = AutoCompleteSource.None;

                this.TextChanged += OnTextChanged;
                this.SelectedIndexChanged += OnSelectedIndexChanged;
            }

            public void SetItems(IEnumerable<(string display, T value)> items)
            {
                T currentValue = _selectedItem != null ? _selectedItem.Value : default;
                _items = items.Select(i => new ItemWrapper { Display = i.display, Value = i.value }).ToList();
                RefreshItems(currentValue);
            }

            public void SetItems(IList<string> displayStrings, IList<T> values)
            {
                if (displayStrings.Count != values.Count)
                    throw new ArgumentException("Display and value lists must be the same length.");

                T currentValue = _selectedItem != null ? _selectedItem.Value : default;
                _items = displayStrings.Select((d, i) => new ItemWrapper { Display = d, Value = values[i] }).ToList();
                RefreshItems(currentValue);
            }

            public void SetItems(IEnumerable<string> displayStrings)
            {
                var oldMap = _items.ToDictionary(i => i.Display, i => i.Value);
                T currentValue = _selectedItem != null ? _selectedItem.Value : default;

                _items = displayStrings
                .Select(display => new ItemWrapper
                {
                    Display = display,
                    Value = oldMap.TryGetValue(display, out var val) ? val : DefaultValue
                })
                .ToList();

                RefreshItems(currentValue);
            }

            public bool UpdateItem(string display, T newValue)
            {
                var item = _items.FirstOrDefault(i => string.Equals(i.Display, display));
                if (item != null)
                {
                    item.Value = newValue;
                    return true;
                }
                return false;
            }

            public bool UpdateItem(T newValue)
            {
                var obj = this.SelectedItem;
                if (obj == null)
                    return false;
                return UpdateItem(this.SelectedItem.ToString(),newValue);
            }

            public T SelectedValueTyped => _selectedItem != null ? _selectedItem.Value : default;

            public T GetValue(int index)
            {
                if (index >= 0 && index < _items.Count)
                    return _items[index].Value;

                throw new ArgumentOutOfRangeException(nameof(index));
            }

            public T GetValue(string display)
            {
                return GetValue(display, false);
            }
            public T GetValue() 
            {
                string text = this.SelectedText;
                if (string.IsNullOrEmpty(text))
                    return DefaultValue;
                return GetValue(this.SelectedText, true); 
            }

            public T GetValue(string display, bool ignoreCase)
            {
                var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
                var match = _items.FirstOrDefault(i => string.Equals(i.Display, display, comparison));
                if (match != null)
                    return match.Value;

                throw new KeyNotFoundException($"No item found with display string '{display}'.");
            }

            protected void RefreshItems(T currentValue)
            {
                _suppressSelectionChange = true;

                this.BeginUpdate();
                base.Items.Clear();
                base.Items.AddRange(_items.Cast<object>().ToArray());
                this.EndUpdate();

                var match = _items.FirstOrDefault(i => EqualityComparer<T>.Default.Equals(i.Value, currentValue));
                this.SelectedItem = match ?? null;

                _selectedItem = this.SelectedItem as ItemWrapper;
                _suppressSelectionChange = false;
            }

            protected void OnTextChanged(object sender, EventArgs e)
            {
                string typedText = this.Text;
                int selStart = this.SelectionStart;

                bool isExactMatch = _items.Any(item =>
                string.Equals(item.Display, typedText, StringComparison.OrdinalIgnoreCase));

                var filtered = isExactMatch
                ? _items
                : _items.Where(item =>
                item.Display.IndexOf(typedText, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

                this.BeginUpdate();
                base.Items.Clear();
                base.Items.AddRange(filtered.Cast<object>().ToArray());

                if (!this.DroppedDown && this.Focused)
                    this.DroppedDown = true;

                this.SelectionStart = selStart;
                this.SelectionLength = 0;
                this.EndUpdate();
            }

            protected void OnSelectedIndexChanged(object sender, EventArgs e)
            {
                if (_suppressSelectionChange)
                    return;

                _selectedItem = this.SelectedItem as ItemWrapper;
            }
        }
    }
}

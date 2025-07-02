using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Intech.Windows.CustomWindowsForms;
using static OfficeOpenXml.ExcelErrorValue;

namespace Intech.Windows.Forms
{

    public partial class SectionEditorControl : UserControl
    {
        private SaveFileManager _manager;
        private SaveFileSection _section;
        private readonly RowComparer _comparer = new RowComparer();
        private Dictionary<string, ColumnType> _columnTypes = new Dictionary<string, ColumnType>();
        private Stack<List<string[]>> _undoStack = new Stack<List<string[]>>();
        private Stack<List<string[]>> _redoStack = new Stack<List<string[]>>();

        private readonly Dictionary<string, object> _defaultColumnValues = new Dictionary<string, object>();

        private bool _hasChanges = false;

        public bool HasChanges => _hasChanges;

        public event EventHandler RowAdded;
        public event EventHandler RowRemoved;
        public event EventHandler SelectionChanged;
        public event EventHandler CellEdited;
        public event EventHandler Confirmed;

        public SectionEditorControl()
        {
            InitializeComponent();
            SetupEvents();
            dataGridView1.AllowUserToAddRows = false;
        }

        private void SetupEvents()
        {
            btnAdd.Click += (s, e) => AddRow();
            btnRemove.Click += (s, e) => RemoveSelectedRows();
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.SelectionChanged += (s, e) => SelectionChanged?.Invoke(this, e);
            dataGridView1.RowsAdded += (s, e) => RowAdded?.Invoke(this, e);
            dataGridView1.RowsRemoved += (s, e) => { UpdateSectionFromGrid(); RowRemoved?.Invoke(this, e); };
            dataGridView1.MouseDown += DataGridView1_MouseDown;
            dataGridView1.MouseMove += DataGridView1_MouseMove;
            dataGridView1.DragDrop += DataGridView1_DragDrop;
            dataGridView1.DragOver += (s, e) => e.Effect = DragDropEffects.Move;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
            dataGridView1.DataError += DataGridView1_DataError;

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSectionFromGrid();
            CellEdited?.Invoke(this, e);
        }
        

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string val = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            List<string> options = (List<string>)((DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex]).DataSource;
            if (options != null && options.Contains(val))
            {
                var cell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                cell.Value = val;
                return;
            }
            dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = val;
        }

        public void ConfigureColumnTypes(Dictionary<string, ColumnType> columnTypes)
        {
            _columnTypes = columnTypes;
        }


        public void Initialize(SaveFileManager manager, SaveFileSection section)
        {

            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _section = section ?? throw new ArgumentNullException(nameof(section));

            SetupGridColumns();
            LoadSectionToGrid();
        }



        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is FilterableComboBoxEditingControl combo &&
            dataGridView1.CurrentCell.OwningColumn is FilterableComboBoxColumn col)
            {
                combo.SetItems(col.ItemsSource);
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void SetupGridColumns()
        {
            dataGridView1.Columns.Clear();
            var headers = _section.Header.Split('\t');

            foreach (var header in headers)
            {
                var trimmed = header.Trim();
                if (_columnTypes.TryGetValue(trimmed, out var type))
                {
                    switch (type)
                    {
                        case ColumnType.ComboBox:
                            var comboColumn = new DataGridViewComboBoxColumn
                            {
                                Name = trimmed,
                                HeaderText = trimmed,
                            };
                            if (_defaultColumnValues.ContainsKey(trimmed))
                            {
                                SetComboBoxItems(trimmed, (List<string>)_defaultColumnValues[trimmed]);
                            }
                            dataGridView1.Columns.Add(comboColumn);
                            break;
                        case ColumnType.CheckBox:
                            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { Name = trimmed, HeaderText = trimmed });
                            break;

                        case ColumnType.FilePicker:
                            var filePickerColumn = new DataGridViewFilePickerColumn
                            {
                                Name = "File",
                                HeaderText = "Select File"
                            };
                            dataGridView1.Columns.Add(filePickerColumn);
                            DataGridViewFilePickerColumn.AttachTo(dataGridView1);

                            break;

                        default:
                            dataGridView1.Columns.Add(trimmed, trimmed);
                            break;
                    }
                }
                else
                {
                    dataGridView1.Columns.Add(trimmed, trimmed);
                }
            }
        }

        private void LoadSectionToGrid()
        {
            dataGridView1.Rows.Clear();
            foreach (var row in _section.Rows)
            {
                dataGridView1.Rows.Add(row);
            }
        }
        public void SetDefaultColumnValue(string columnName, object value)
        {
            _defaultColumnValues.Add(columnName, value);
        }

        public void SetColumnWidth(string columnName, int width)
        {
            dataGridView1.Columns[columnName].Width = width;
        }

        public SaveFileSection GetSaveFileSection()
        {
            return _section;
        }

        private void AddRow()
        {
            //DataGridViewRowsAddedEventArgs rowEvent = 
            dataGridView1.RowsAdded -= (s, e) => RowAdded?.Invoke(this, e);
            SaveUndoState();
            int rowIndex = dataGridView1.Rows.Add();
            var row = dataGridView1.Rows[rowIndex];
            foreach (var kvp in _defaultColumnValues)
            {
                if (dataGridView1.Columns.Contains(kvp.Key))
                {
                    row.Cells[kvp.Key].Value = kvp.Value;
                }
            }
            dataGridView1.RowsAdded += (s, e) => RowAdded?.Invoke(this, e);

            _hasChanges = true;
            DataGridViewRowsAddedEventArgs eventArg = new DataGridViewRowsAddedEventArgs(rowIndex, 1);
            RowAdded?.Invoke(this, eventArg);
        }


        private void RemoveSelectedRows()
        {
            SaveUndoState();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!row.IsNewRow)
                {
                    string[] values = GetRowValues(row);
                    _section.Rows.RemoveAll(r => _comparer.Equals(r, values));
                    dataGridView1.Rows.Remove(row);
                }
            }
            _hasChanges = true;
            RowRemoved?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateSectionFromGrid()
        {
            SaveUndoState();
            var updatedRows = new List<string[]>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string[] values = GetRowValues(row);
                    if (values.All(cell => cell != null))
                    {
                        updatedRows.Add(values);
                    }
                }
            }

            _section.Rows = new SaveFileSection.NoEmptyList();
            foreach (var row in updatedRows.Distinct(_comparer))
            {
                _section.Rows.Add(row);
            }

            _hasChanges = true;
        }

        private string[] GetRowValues(DataGridViewRow row)
        {
            return row.Cells.Cast<DataGridViewCell>()
            .Select(c => c.Value?.ToString() ?? string.Empty)
            .ToArray();
        }

        public int GetRowCount()
        {
            return _section.Rows.Count();
        }

        public void Confirm()
        {
            _manager.AddOrUpdateSection(_section);
            _hasChanges = false;
            Confirmed?.Invoke(this, EventArgs.Empty);
        }

        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                _redoStack.Push(CloneRows(_section.Rows));
                var previous = _undoStack.Pop();
                _section.Rows = new SaveFileSection.NoEmptyList();
                _section.Rows.AddRange(previous);
                LoadSectionToGrid();
                _hasChanges = true;
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                _undoStack.Push(CloneRows(_section.Rows));
                var next = _redoStack.Pop();
                _section.Rows = new SaveFileSection.NoEmptyList();
                _section.Rows.AddRange(next);
                LoadSectionToGrid();
                _hasChanges = true;
            }
        }

        private void SaveUndoState()
        {
            _undoStack.Push(CloneRows(_section.Rows));
            _redoStack.Clear();
        }

        private List<string[]> CloneRows(IEnumerable<string[]> rows)
        {
            return rows.Select(r => r.ToArray()).ToList();
        }

        private class RowComparer : IEqualityComparer<string[]>
        {
            public bool Equals(string[] x, string[] y) => x.SequenceEqual(y);
            public int GetHashCode(string[] obj) => string.Join("|", obj).GetHashCode();
        }

        // Drag-and-drop row reordering
        private int _dragRowIndex = -1;

        private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            _dragRowIndex = dataGridView1.HitTest(e.X, e.Y).RowIndex;
        }

        private void DataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left && _dragRowIndex >= 0)
            {
                dataGridView1.DoDragDrop(dataGridView1.Rows[_dragRowIndex], DragDropEffects.Move);
            }
        }

        private void DataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));
            int dropIndex = dataGridView1.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (dropIndex >= 0 && _dragRowIndex >= 0 && dropIndex != _dragRowIndex)
            {
                SaveUndoState();

                var row = _section.Rows[_dragRowIndex];
                _section.Rows.RemoveAt(_dragRowIndex);
                _section.Rows.Insert(dropIndex, row);

                LoadSectionToGrid();
                _hasChanges = true;
            }
        }

        public void SetComboBoxItems(string columnName, List<string> items)
        {
            if (dataGridView1.Columns[columnName] is DataGridViewComboBoxColumn comboCol)
            {
                items.Sort();
                comboCol.DataSource = items;
            }
        }

        public void SetComboBoxItems(string columnName , int row, List<string> items)
        {
            if (dataGridView1.Rows[row].Cells[columnName] is DataGridViewComboBoxCell comboCell)
            {
                items.Sort();
                comboCell.DataSource = items;
            }
        }

        public object GetCellValue(string columnName, int row)
        {
            return dataGridView1.Rows[row].Cells[columnName].Value;
        }
        public object GetCellValue(int column, int row)
        {
            return dataGridView1.Rows[row].Cells[column].Value;
        }

        public DataGridViewSelectedCellCollection GetSelectedCell()
        {
            return dataGridView1.SelectedCells;
        }

        public void SetCellValue(string columnName, int row, string Value)
        {
            dataGridView1.Rows[row].Cells[columnName].Value = Value;
        }
        public void SetCellValue(int column, int row, string Value)
        {
            dataGridView1.Rows[row].Cells[column].Value = Value;
        }
        public void SetCellValue(string columnName, int row, bool Value)
        {
            dataGridView1.Rows[row].Cells[columnName].Value = Value;
        }
        public void SetCellValue(int column, int row, bool Value)
        {
            dataGridView1.Rows[row].Cells[column].Value = Value;
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            var currentRow = dataGridView1.CurrentRow;
            int columnCount = dataGridView1.ColumnCount;

            string[] values = new string[columnCount];

            for (int i = 0; i < columnCount; i++)
            {
                values[i] = currentRow.Cells[i].Value?.ToString();
            }

            dataGridView1.Rows.Add(values);
        }

    }

    public enum ColumnType
    {
        Text,
        ComboBox,
        CheckBox,
        FilePicker
    }
}

using Autodesk.Revit.DB;
using Intech.Revit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Intech.SharedParameter
{
    public partial class FormulaAdd : System.Windows.Forms.Form
    {
        public FormulaAdd()
        {
            InitializeComponent();
            CenterToParent();
            InitializeTree();
        }

        private void InitializeTree()
        {
            parameterTypes.CheckBoxes = true;
            parameterTypes.AfterCheck += parameterTypes_AfterCheck;
            Dictionary<Family, List<FamilySymbol>> famMap = Revit.RevitHelperFunctions.GetFamilySymbolMap();
            Dictionary<string, List<string>> nameMap = new Dictionary<string, List<string>>();
            foreach (Family fam in famMap.Keys)
            {
                List<string> typeNames = new List<string>();
                foreach(FamilySymbol fSym in famMap[fam])
                {
                    typeNames.Add(fSym.Name);
                }
                nameMap.Add(fam.Name, typeNames);
            }

            parameterTypes.SetData(nameMap);
            parameterTypes.Sort();
        }

        private void paramSearch_TextChanged(object sender, EventArgs e)
        {
            parameterTypes.ApplyFilter(paramSearch.Text);
        }

        private void parameterTypes_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.Unknown)
                return;

            parameterTypes.AfterCheck -= parameterTypes_AfterCheck;
            parameterTypes.Enabled = false;

            if (e.Node.Parent == null)
            {
                // Root node was checked/unchecked
                foreach (TreeNode child in e.Node.Nodes)
                {
                    child.Checked = e.Node.Checked;
                }
            }
            else
            {
                // Child node was checked/unchecked
                TreeNode parent = e.Node.Parent;

                bool allChecked = true;
                foreach (TreeNode sibling in parent.Nodes)
                {
                    if (!sibling.Checked)
                    {
                        allChecked = false;
                        break;
                    }
                }

                parent.Checked = allChecked;
            }

            parameterTypes.Enabled = true;
            parameterTypes.AfterCheck += parameterTypes_AfterCheck;
        }
    }
}

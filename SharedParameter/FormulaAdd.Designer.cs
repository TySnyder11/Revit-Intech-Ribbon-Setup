using System.Windows.Forms;

namespace Intech.SharedParameter
{
    partial class FormulaAdd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormulaAdd));
            this.FormulaTextBox = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.CheckFormula = new System.Windows.Forms.Button();
            this.famSearch = new System.Windows.Forms.TextBox();
            this.parameters = new System.Windows.Forms.ListBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.FamilySelect = new Intech.Windows.CustomWindowsForms.FilterableCheckedListBox();
            this.SelectParameterComboBox = new System.Windows.Forms.ComboBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // FormulaTextBox
            // 
            this.FormulaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FormulaTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FormulaTextBox.Location = new System.Drawing.Point(12, 320);
            this.FormulaTextBox.Name = "FormulaTextBox";
            this.FormulaTextBox.Size = new System.Drawing.Size(360, 71);
            this.FormulaTextBox.TabIndex = 1;
            this.FormulaTextBox.Text = "=";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(12, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 13);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Family Types";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(13, 302);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 13);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "Formula";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(12, 421);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Confirm...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(456, 421);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // CheckFormula
            // 
            this.CheckFormula.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckFormula.Location = new System.Drawing.Point(108, 397);
            this.CheckFormula.Name = "CheckFormula";
            this.CheckFormula.Size = new System.Drawing.Size(323, 23);
            this.CheckFormula.TabIndex = 6;
            this.CheckFormula.Text = "Check Formula";
            this.CheckFormula.UseVisualStyleBackColor = true;
            // 
            // famSearch
            // 
            this.famSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.famSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.famSearch.Location = new System.Drawing.Point(12, 27);
            this.famSearch.Name = "famSearch";
            this.famSearch.Size = new System.Drawing.Size(360, 20);
            this.famSearch.TabIndex = 7;
            this.famSearch.TextChanged += new System.EventHandler(this.famSearch_TextChanged);
            // 
            // parameters
            // 
            this.parameters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameters.FormattingEnabled = true;
            this.parameters.IntegralHeight = false;
            this.parameters.Location = new System.Drawing.Point(388, 27);
            this.parameters.Name = "parameters";
            this.parameters.Size = new System.Drawing.Size(143, 318);
            this.parameters.TabIndex = 8;
            this.parameters.SelectedIndexChanged += new System.EventHandler(this.parameters_SelectedIndexChanged);
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(409, 8);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 13);
            this.textBox3.TabIndex = 9;
            this.textBox3.Text = "Available Parameters";
            // 
            // FamilySelect
            // 
            this.FamilySelect.AllItems = ((System.Collections.Generic.List<string>)(resources.GetObject("FamilySelect.AllItems")));
            this.FamilySelect.CheckOnClick = true;
            this.FamilySelect.Filter = "";
            this.FamilySelect.FormattingEnabled = true;
            this.FamilySelect.IntegralHeight = false;
            this.FamilySelect.Location = new System.Drawing.Point(12, 53);
            this.FamilySelect.Name = "FamilySelect";
            this.FamilySelect.Size = new System.Drawing.Size(360, 243);
            this.FamilySelect.TabIndex = 10;
            // 
            // SelectParameterComboBox
            // 
            this.SelectParameterComboBox.FormattingEnabled = true;
            this.SelectParameterComboBox.Location = new System.Drawing.Point(388, 370);
            this.SelectParameterComboBox.Name = "SelectParameterComboBox";
            this.SelectParameterComboBox.Size = new System.Drawing.Size(143, 21);
            this.SelectParameterComboBox.TabIndex = 11;
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox4.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(388, 351);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 13);
            this.textBox4.TabIndex = 12;
            this.textBox4.Text = "Active Parameter";
            // 
            // FormulaAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 451);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.SelectParameterComboBox);
            this.Controls.Add(this.FamilySelect);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.parameters);
            this.Controls.Add(this.famSearch);
            this.Controls.Add(this.CheckFormula);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.FormulaTextBox);
            this.Name = "FormulaAdd";
            this.Text = "FormulaAdd";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox FormulaTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button CheckFormula;
        private TextBox famSearch;
        private ListBox parameters;
        private TextBox textBox3;
        private Intech.Windows.CustomWindowsForms.FilterableCheckedListBox FamilySelect;
        private ComboBox SelectParameterComboBox;
        private TextBox textBox4;
    }
}
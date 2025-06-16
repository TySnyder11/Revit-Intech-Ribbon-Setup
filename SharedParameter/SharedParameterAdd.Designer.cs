namespace Intech.SharedParameter
{
    partial class SharedParameterAdd
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.familySearch = new System.Windows.Forms.TextBox();
            this.definitionSelect = new System.Windows.Forms.CheckedListBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.defGroupSelect = new System.Windows.Forms.ComboBox();
            this.cancel = new System.Windows.Forms.Button();
            this.Confirm = new System.Windows.Forms.Button();
            this.FamilySelect = new System.Windows.Forms.CheckedListBox();
            this.ParameterCategory = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.InstanceSelect = new System.Windows.Forms.RadioButton();
            this.TypeSelect = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(260, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(228, 13);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Family Select";
            // 
            // familySearch
            // 
            this.familySearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.familySearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.familySearch.Location = new System.Drawing.Point(260, 31);
            this.familySearch.Name = "familySearch";
            this.familySearch.Size = new System.Drawing.Size(228, 20);
            this.familySearch.TabIndex = 2;
            this.familySearch.TextChanged += new System.EventHandler(this.familySearch_TextChanged);
            // 
            // definitionSelect
            // 
            this.definitionSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.definitionSelect.FormattingEnabled = true;
            this.definitionSelect.IntegralHeight = false;
            this.definitionSelect.Location = new System.Drawing.Point(12, 54);
            this.definitionSelect.Name = "definitionSelect";
            this.definitionSelect.Size = new System.Drawing.Size(228, 337);
            this.definitionSelect.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(12, 12);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 13);
            this.textBox3.TabIndex = 4;
            this.textBox3.Text = "Shared Parameters";
            // 
            // defGroupSelect
            // 
            this.defGroupSelect.FormattingEnabled = true;
            this.defGroupSelect.Location = new System.Drawing.Point(12, 30);
            this.defGroupSelect.Name = "defGroupSelect";
            this.defGroupSelect.Size = new System.Drawing.Size(228, 21);
            this.defGroupSelect.TabIndex = 5;
            this.defGroupSelect.SelectedIndexChanged += new System.EventHandler(this.defGroupSelect_SelectedIndexChanged);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(413, 439);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 7;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // Confirm
            // 
            this.Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Confirm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Confirm.Location = new System.Drawing.Point(12, 439);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(75, 23);
            this.Confirm.TabIndex = 8;
            this.Confirm.Text = "Confirm...";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // FamilySelect
            // 
            this.FamilySelect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FamilySelect.FormattingEnabled = true;
            this.FamilySelect.IntegralHeight = false;
            this.FamilySelect.Location = new System.Drawing.Point(260, 54);
            this.FamilySelect.Name = "FamilySelect";
            this.FamilySelect.Size = new System.Drawing.Size(228, 337);
            this.FamilySelect.TabIndex = 9;
            // 
            // ParameterCategory
            // 
            this.ParameterCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ParameterCategory.FormattingEnabled = true;
            this.ParameterCategory.Location = new System.Drawing.Point(101, 408);
            this.ParameterCategory.Name = "ParameterCategory";
            this.ParameterCategory.Size = new System.Drawing.Size(118, 21);
            this.ParameterCategory.TabIndex = 11;
            this.ParameterCategory.SelectedIndexChanged += new System.EventHandler(this.ParameterCategory_SelectedIndexChanged);
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(12, 411);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(83, 13);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "Parameter Group";
            // 
            // InstanceSelect
            // 
            this.InstanceSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InstanceSelect.AutoSize = true;
            this.InstanceSelect.Location = new System.Drawing.Point(260, 398);
            this.InstanceSelect.Name = "InstanceSelect";
            this.InstanceSelect.Size = new System.Drawing.Size(66, 17);
            this.InstanceSelect.TabIndex = 13;
            this.InstanceSelect.TabStop = true;
            this.InstanceSelect.Text = "Instance";
            this.InstanceSelect.UseVisualStyleBackColor = true;
            this.InstanceSelect.CheckedChanged += new System.EventHandler(this.InstanceSelect_CheckedChanged);
            // 
            // TypeSelect
            // 
            this.TypeSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TypeSelect.AutoSize = true;
            this.TypeSelect.Location = new System.Drawing.Point(260, 421);
            this.TypeSelect.Name = "TypeSelect";
            this.TypeSelect.Size = new System.Drawing.Size(49, 17);
            this.TypeSelect.TabIndex = 14;
            this.TypeSelect.TabStop = true;
            this.TypeSelect.Text = "Type";
            this.TypeSelect.UseVisualStyleBackColor = true;
            this.TypeSelect.CheckedChanged += new System.EventHandler(this.TypeSelect_CheckedChanged);
            // 
            // SharedParameterAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(500, 465);
            this.Controls.Add(this.TypeSelect);
            this.Controls.Add(this.InstanceSelect);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.ParameterCategory);
            this.Controls.Add(this.FamilySelect);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.defGroupSelect);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.definitionSelect);
            this.Controls.Add(this.familySearch);
            this.Controls.Add(this.textBox1);
            this.Name = "SharedParameterAdd";
            this.Text = "`";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox familySearch;
        private System.Windows.Forms.CheckedListBox definitionSelect;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.ComboBox defGroupSelect;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.TextBox definitionSearch;
        private System.Windows.Forms.CheckedListBox FamilySelect;
        private System.Windows.Forms.ComboBox ParameterCategory;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.RadioButton InstanceSelect;
        private System.Windows.Forms.RadioButton TypeSelect;
        private System.Windows.Forms.CheckBox ReportingParameter;
    }
}
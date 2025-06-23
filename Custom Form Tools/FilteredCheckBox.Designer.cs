namespace Intech.Windows.Forms
{
    partial class FilteredCheckBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilteredCheckBox));
            this.checkedListBox = new Intech.Windows.CustomWindowsForms.FilterableCheckedListBox();
            this.SearchBox = new System.Windows.Forms.TextBox();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // checkedListBox
            // 
            this.checkedListBox.AllItems = ((System.Collections.Generic.List<string>)(resources.GetObject("checkedListBox.AllItems")));
            this.checkedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox.CheckOnClick = true;
            this.checkedListBox.Filter = "";
            this.checkedListBox.FormattingEnabled = true;
            this.checkedListBox.IntegralHeight = false;
            this.checkedListBox.Location = new System.Drawing.Point(3, 28);
            this.checkedListBox.Name = "checkedListBox";
            this.checkedListBox.Size = new System.Drawing.Size(255, 177);
            this.checkedListBox.TabIndex = 0;
            // 
            // SearchBox
            // 
            this.SearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchBox.Location = new System.Drawing.Point(142, 3);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(116, 20);
            this.SearchBox.TabIndex = 1;
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            // 
            // NameTextBox
            // 
            this.NameTextBox.BackColor = System.Drawing.SystemColors.Menu;
            this.NameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NameTextBox.Location = new System.Drawing.Point(3, 6);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(133, 13);
            this.NameTextBox.TabIndex = 2;
            // 
            // FilteredCheckBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.checkedListBox);
            this.Name = "FilteredCheckBox";
            this.Size = new System.Drawing.Size(261, 208);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Intech.Windows.CustomWindowsForms.FilterableCheckedListBox checkedListBox;
        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.TextBox NameTextBox;
    }
}

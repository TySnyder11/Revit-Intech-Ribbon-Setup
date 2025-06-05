namespace Intech
{
    partial class ParameterSyncForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Element = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.baseParam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Output = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reloadselect = new System.Windows.Forms.Button();
            this.reloadAll = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.settings = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.Element,
            this.baseParam,
            this.Output,
            this.Description});
            this.dataGridView1.Location = new System.Drawing.Point(12, 43);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(750, 243);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellChanged);
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            // 
            // Element
            // 
            this.Element.HeaderText = "ElementType";
            this.Element.Name = "Element";
            this.Element.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Element.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // baseParam
            // 
            this.baseParam.HeaderText = "Base Parameter";
            this.baseParam.Name = "baseParam";
            // 
            // Output
            // 
            this.Output.HeaderText = "Output Parameter";
            this.Output.Name = "Output";
            this.Output.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Output.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            // 
            // reloadselect
            // 
            this.reloadselect.Location = new System.Drawing.Point(13, 13);
            this.reloadselect.Name = "reloadselect";
            this.reloadselect.Size = new System.Drawing.Size(101, 23);
            this.reloadselect.TabIndex = 1;
            this.reloadselect.Text = "Reload Selected";
            this.reloadselect.UseVisualStyleBackColor = true;
            // 
            // reloadAll
            // 
            this.reloadAll.Location = new System.Drawing.Point(120, 13);
            this.reloadAll.Name = "reloadAll";
            this.reloadAll.Size = new System.Drawing.Size(75, 23);
            this.reloadAll.TabIndex = 2;
            this.reloadAll.Text = "Reload All";
            this.reloadAll.UseVisualStyleBackColor = true;
            // 
            // Close
            // 
            this.Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close.Location = new System.Drawing.Point(687, 421);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 3;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            // 
            // settings
            // 
            this.settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.settings.Location = new System.Drawing.Point(687, 392);
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(75, 23);
            this.settings.TabIndex = 4;
            this.settings.Text = "Settings";
            this.settings.UseVisualStyleBackColor = true;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(12, 411);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 7;
            this.addButton.Text = "New...";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // ParameterSyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Close;
            this.ClientSize = new System.Drawing.Size(774, 456);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.settings);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.reloadAll);
            this.Controls.Add(this.reloadselect);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ParameterSyncForm";
            this.Text = "Parameter Sync Form";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button reloadselect;
        private System.Windows.Forms.Button reloadAll;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.Button settings;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn Element;
        private System.Windows.Forms.DataGridViewTextBoxColumn baseParam;
        private System.Windows.Forms.DataGridViewComboBoxColumn Output;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.Button addButton;
    }
}
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
            this.Element = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.baseParam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Output = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reloadselect = new System.Windows.Forms.Button();
            this.reloadAll = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.Remove = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.Element,
            this.baseParam,
            this.Output});
            this.dataGridView1.Location = new System.Drawing.Point(12, 43);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(750, 243);
            this.dataGridView1.TabIndex = 0;
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
            // 
            // baseParam
            // 
            this.baseParam.HeaderText = "Base Parameter";
            this.baseParam.Name = "baseParam";
            this.baseParam.Width = 300;
            // 
            // Output
            // 
            this.Output.HeaderText = "Output Parameter";
            this.Output.Name = "Output";
            this.Output.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Output.Width = 200;
            // 
            // reloadselect
            // 
            this.reloadselect.Location = new System.Drawing.Point(13, 13);
            this.reloadselect.Name = "reloadselect";
            this.reloadselect.Size = new System.Drawing.Size(101, 23);
            this.reloadselect.TabIndex = 1;
            this.reloadselect.Text = "Reload Selected";
            this.reloadselect.UseVisualStyleBackColor = true;
            this.reloadselect.Click += new System.EventHandler(this.reloadselect_Click);
            // 
            // reloadAll
            // 
            this.reloadAll.Location = new System.Drawing.Point(120, 13);
            this.reloadAll.Name = "reloadAll";
            this.reloadAll.Size = new System.Drawing.Size(75, 23);
            this.reloadAll.TabIndex = 2;
            this.reloadAll.Text = "Reload All";
            this.reloadAll.UseVisualStyleBackColor = true;
            this.reloadAll.Click += new System.EventHandler(this.reloadAll_Click);
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
            // Remove
            // 
            this.Remove.Location = new System.Drawing.Point(93, 411);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(75, 23);
            this.Remove.TabIndex = 8;
            this.Remove.Text = "Remove";
            this.Remove.UseVisualStyleBackColor = true;
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // edit
            // 
            this.edit.Location = new System.Drawing.Point(174, 411);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(75, 23);
            this.edit.TabIndex = 9;
            this.edit.Text = "Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.edit_Click);
            // 
            // ParameterSyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Close;
            this.ClientSize = new System.Drawing.Size(774, 456);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.addButton);
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
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button Remove;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Element;
        private System.Windows.Forms.DataGridViewTextBoxColumn baseParam;
        private System.Windows.Forms.DataGridViewTextBoxColumn Output;
        private System.Windows.Forms.Button edit;
    }
}
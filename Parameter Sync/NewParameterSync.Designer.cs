namespace Intech 
{ 
    partial class NewParameterSync
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
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.smartParameterBox = new System.Windows.Forms.RichTextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.save = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.parameterComboBox = new System.Windows.Forms.ComboBox();
            this.saveAndLoad = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryComboBox.FormattingEnabled = true;
            this.categoryComboBox.Location = new System.Drawing.Point(288, 12);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(163, 21);
            this.categoryComboBox.TabIndex = 0;
            this.categoryComboBox.SelectedIndexChanged += new System.EventHandler(this.category_SelectedIndexChanged);
            this.categoryComboBox.TextUpdate += new System.EventHandler(this.category_SelectedTextUpdate);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(49, 14);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(159, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox1.Location = new System.Drawing.Point(12, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(31, 13);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Name";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox2.Location = new System.Drawing.Point(237, 17);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(45, 13);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "Category";
            // 
            // smartParameterBox
            // 
            this.smartParameterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smartParameterBox.Location = new System.Drawing.Point(96, 39);
            this.smartParameterBox.Name = "smartParameterBox";
            this.smartParameterBox.Size = new System.Drawing.Size(355, 54);
            this.smartParameterBox.TabIndex = 4;
            this.smartParameterBox.Text = "";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox3.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox3.Location = new System.Drawing.Point(5, 60);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(74, 13);
            this.textBox3.TabIndex = 5;
            this.textBox3.Text = "Paramerter Pull";
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox4.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textBox4.Location = new System.Drawing.Point(5, 103);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(85, 13);
            this.textBox4.TabIndex = 7;
            this.textBox4.Text = "Output Parameter";
            // 
            // save
            // 
            this.save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.save.Location = new System.Drawing.Point(4, 133);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(75, 23);
            this.save.TabIndex = 8;
            this.save.Text = "Save...";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // Close
            // 
            this.Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close.Location = new System.Drawing.Point(378, 133);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 9;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            // 
            // parameterComboBox
            // 
            this.parameterComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterComboBox.FormattingEnabled = true;
            this.parameterComboBox.Location = new System.Drawing.Point(96, 99);
            this.parameterComboBox.Name = "parameterComboBox";
            this.parameterComboBox.Size = new System.Drawing.Size(355, 21);
            this.parameterComboBox.TabIndex = 10;
            this.parameterComboBox.TextUpdate += new System.EventHandler(this.parameter_SelectedTextUpdate);
            // 
            // saveAndLoad
            // 
            this.saveAndLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveAndLoad.Location = new System.Drawing.Point(85, 133);
            this.saveAndLoad.Name = "saveAndLoad";
            this.saveAndLoad.Size = new System.Drawing.Size(102, 23);
            this.saveAndLoad.TabIndex = 11;
            this.saveAndLoad.Text = "Save and Load...";
            this.saveAndLoad.UseVisualStyleBackColor = true;
            this.saveAndLoad.Click += new System.EventHandler(this.saveAndLoad_Click);
            // 
            // NewParameterSync
            // 
            this.AcceptButton = this.save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 166);
            this.Controls.Add(this.saveAndLoad);
            this.Controls.Add(this.parameterComboBox);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.save);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.smartParameterBox);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.categoryComboBox);
            this.Name = "NewParameterSync";
            this.RightToLeftLayout = true;
            this.Text = "Create Parameter Link";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.RichTextBox smartParameterBox;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.ComboBox parameterComboBox;
        private System.Windows.Forms.Button saveAndLoad;
    }
}
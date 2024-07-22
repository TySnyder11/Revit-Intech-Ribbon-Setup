namespace Intech
{
    partial class SheetCreateForm
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
            this.PlanViewCheckList = new System.Windows.Forms.CheckedListBox();
            this.PlanViewLable = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SearchBox = new System.Windows.Forms.TextBox();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.Parameters = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.SheetNameLable = new System.Windows.Forms.Label();
            this.TitleBlockFamilyLable = new System.Windows.Forms.Label();
            this.TitleBlockFamily = new System.Windows.Forms.ComboBox();
            this.MiddleSheetNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TradeAbriviation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.Create = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.LevelOverride = new System.Windows.Forms.CheckBox();
            this.AreaOverride = new System.Windows.Forms.CheckBox();
            this.TitleBlockType = new System.Windows.Forms.ComboBox();
            this.Titleblock = new System.Windows.Forms.Label();
            this.AreaOverrideComboBox = new System.Windows.Forms.ComboBox();
            this.LevelOverrideComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PlanViewCheckList
            // 
            this.PlanViewCheckList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlanViewCheckList.FormattingEnabled = true;
            this.PlanViewCheckList.Location = new System.Drawing.Point(12, 28);
            this.PlanViewCheckList.Name = "PlanViewCheckList";
            this.PlanViewCheckList.Size = new System.Drawing.Size(264, 242);
            this.PlanViewCheckList.TabIndex = 0;
            // 
            // PlanViewLable
            // 
            this.PlanViewLable.AutoSize = true;
            this.PlanViewLable.Location = new System.Drawing.Point(9, 9);
            this.PlanViewLable.Name = "PlanViewLable";
            this.PlanViewLable.Size = new System.Drawing.Size(66, 16);
            this.PlanViewLable.TabIndex = 1;
            this.PlanViewLable.Text = "Plan View";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 302);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Search";
            // 
            // SearchBox
            // 
            this.SearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchBox.Location = new System.Drawing.Point(69, 298);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(207, 22);
            this.SearchBox.TabIndex = 6;
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.Location = new System.Drawing.Point(301, 224);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(304, 140);
            this.checkedListBox2.TabIndex = 8;
            // 
            // Parameters
            // 
            this.Parameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Parameters.AutoSize = true;
            this.Parameters.Location = new System.Drawing.Point(298, 205);
            this.Parameters.Name = "Parameters";
            this.Parameters.Size = new System.Drawing.Size(143, 16);
            this.Parameters.TabIndex = 9;
            this.Parameters.Text = "Title Block Parameters";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(302, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(303, 22);
            this.textBox1.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::TitleBlockSetup.Properties.Resources.IMC_Logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 392);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(85, 66);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // SheetNameLable
            // 
            this.SheetNameLable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SheetNameLable.AutoSize = true;
            this.SheetNameLable.Location = new System.Drawing.Point(299, 60);
            this.SheetNameLable.Name = "SheetNameLable";
            this.SheetNameLable.Size = new System.Drawing.Size(82, 16);
            this.SheetNameLable.TabIndex = 12;
            this.SheetNameLable.Text = "Sheet Name";
            // 
            // TitleBlockFamilyLable
            // 
            this.TitleBlockFamilyLable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TitleBlockFamilyLable.AutoSize = true;
            this.TitleBlockFamilyLable.Location = new System.Drawing.Point(299, 105);
            this.TitleBlockFamilyLable.Name = "TitleBlockFamilyLable";
            this.TitleBlockFamilyLable.Size = new System.Drawing.Size(113, 16);
            this.TitleBlockFamilyLable.TabIndex = 13;
            this.TitleBlockFamilyLable.Text = "Title Block Family";
            // 
            // TitleBlockFamily
            // 
            this.TitleBlockFamily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TitleBlockFamily.FormattingEnabled = true;
            this.TitleBlockFamily.Location = new System.Drawing.Point(301, 126);
            this.TitleBlockFamily.Name = "TitleBlockFamily";
            this.TitleBlockFamily.Size = new System.Drawing.Size(304, 24);
            this.TitleBlockFamily.TabIndex = 15;
            this.TitleBlockFamily.SelectedIndexChanged += new System.EventHandler(this.TitleBlockFamily_SelectedIndexChanged);
            // 
            // MiddleSheetNumber
            // 
            this.MiddleSheetNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MiddleSheetNumber.Location = new System.Drawing.Point(335, 34);
            this.MiddleSheetNumber.Name = "MiddleSheetNumber";
            this.MiddleSheetNumber.Size = new System.Drawing.Size(18, 22);
            this.MiddleSheetNumber.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(355, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "1A1";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(299, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 16);
            this.label4.TabIndex = 19;
            this.label4.Text = "Sheet Number";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(390, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "*Other characters auto fill";
            // 
            // TradeAbriviation
            // 
            this.TradeAbriviation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TradeAbriviation.Location = new System.Drawing.Point(298, 34);
            this.TradeAbriviation.Name = "TradeAbriviation";
            this.TradeAbriviation.Size = new System.Drawing.Size(18, 22);
            this.TradeAbriviation.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(319, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 16);
            this.label1.TabIndex = 22;
            this.label1.Text = "1";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(395, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 16);
            this.label6.TabIndex = 23;
            this.label6.Text = "Example M111A1";
            // 
            // Create
            // 
            this.Create.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Create.Location = new System.Drawing.Point(533, 410);
            this.Create.Name = "Create";
            this.Create.Size = new System.Drawing.Size(75, 23);
            this.Create.TabIndex = 24;
            this.Create.Text = "Create";
            this.Create.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(533, 439);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 25;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // LevelOverride
            // 
            this.LevelOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LevelOverride.AutoSize = true;
            this.LevelOverride.Location = new System.Drawing.Point(12, 330);
            this.LevelOverride.Name = "LevelOverride";
            this.LevelOverride.Size = new System.Drawing.Size(117, 20);
            this.LevelOverride.TabIndex = 26;
            this.LevelOverride.Text = "Level Override";
            this.LevelOverride.UseVisualStyleBackColor = true;
            // 
            // AreaOverride
            // 
            this.AreaOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AreaOverride.AutoSize = true;
            this.AreaOverride.Location = new System.Drawing.Point(12, 356);
            this.AreaOverride.Name = "AreaOverride";
            this.AreaOverride.Size = new System.Drawing.Size(113, 20);
            this.AreaOverride.TabIndex = 27;
            this.AreaOverride.Text = "Area Override";
            this.AreaOverride.UseVisualStyleBackColor = true;
            // 
            // TitleBlockType
            // 
            this.TitleBlockType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TitleBlockType.FormattingEnabled = true;
            this.TitleBlockType.Location = new System.Drawing.Point(301, 178);
            this.TitleBlockType.Name = "TitleBlockType";
            this.TitleBlockType.Size = new System.Drawing.Size(304, 24);
            this.TitleBlockType.TabIndex = 29;
            // 
            // Titleblock
            // 
            this.Titleblock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Titleblock.AutoSize = true;
            this.Titleblock.Location = new System.Drawing.Point(299, 157);
            this.Titleblock.Name = "Titleblock";
            this.Titleblock.Size = new System.Drawing.Size(131, 20);
            this.Titleblock.TabIndex = 28;
            this.Titleblock.Text = "Title Block Type";
            // 
            // AreaOverrideComboBox
            // 
            this.AreaOverrideComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AreaOverrideComboBox.FormattingEnabled = true;
            this.AreaOverrideComboBox.Location = new System.Drawing.Point(128, 352);
            this.AreaOverrideComboBox.Name = "AreaOverrideComboBox";
            this.AreaOverrideComboBox.Size = new System.Drawing.Size(148, 24);
            this.AreaOverrideComboBox.TabIndex = 30;
            this.AreaOverrideComboBox.Visible = false;
            // 
            // LevelOverrideComboBox
            // 
            this.LevelOverrideComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LevelOverrideComboBox.FormattingEnabled = true;
            this.LevelOverrideComboBox.Location = new System.Drawing.Point(128, 326);
            this.LevelOverrideComboBox.Name = "LevelOverrideComboBox";
            this.LevelOverrideComboBox.Size = new System.Drawing.Size(148, 24);
            this.LevelOverrideComboBox.TabIndex = 31;
            this.LevelOverrideComboBox.Visible = false;
            // 
            // SheetCreateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 470);
            this.Controls.Add(this.LevelOverrideComboBox);
            this.Controls.Add(this.AreaOverrideComboBox);
            this.Controls.Add(this.TitleBlockType);
            this.Controls.Add(this.Titleblock);
            this.Controls.Add(this.AreaOverride);
            this.Controls.Add(this.LevelOverride);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Create);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TradeAbriviation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MiddleSheetNumber);
            this.Controls.Add(this.TitleBlockFamily);
            this.Controls.Add(this.TitleBlockFamilyLable);
            this.Controls.Add(this.SheetNameLable);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Parameters);
            this.Controls.Add(this.checkedListBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.PlanViewLable);
            this.Controls.Add(this.PlanViewCheckList);
            this.MinimumSize = new System.Drawing.Size(580, 379);
            this.Name = "SheetCreateForm";
            this.Text = "SheetCreateForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox PlanViewCheckList;
        private System.Windows.Forms.Label PlanViewLable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Label Parameters;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label SheetNameLable;
        private System.Windows.Forms.Label TitleBlockFamilyLable;
        private System.Windows.Forms.ComboBox TitleBlockFamily;
        private System.Windows.Forms.TextBox MiddleSheetNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TradeAbriviation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button Create;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.CheckBox LevelOverride;
        private System.Windows.Forms.CheckBox AreaOverride;
        private System.Windows.Forms.ComboBox TitleBlockType;
        private System.Windows.Forms.Label Titleblock;
        private System.Windows.Forms.ComboBox AreaOverrideComboBox;
        private System.Windows.Forms.ComboBox LevelOverrideComboBox;
    }
}
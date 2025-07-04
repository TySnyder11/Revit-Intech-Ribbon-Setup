﻿using Autodesk.Revit.DB;

namespace Intech
{
    partial class SheetSettings
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
            this.SheetTabController = new System.Windows.Forms.TabControl();
            this.BaseControlTab = new System.Windows.Forms.TabPage();
            this.TitleBlockType = new Intech.Windows.CustomWindowsForms.FilteredComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TitleBlockFamily = new Intech.Windows.CustomWindowsForms.FilteredComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MiddleSheetNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TradeAbriviation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.ScaleTab = new System.Windows.Forms.TabPage();
            this.ScaleGrid = new Intech.Windows.Forms.SectionEditorControl();
            this.DisciplineTab = new System.Windows.Forms.TabPage();
            this.SubDisciplineGrid = new Intech.Windows.Forms.SectionEditorControl();
            this.SubDisciplineCheck = new System.Windows.Forms.CheckBox();
            this.DisciplineGrid = new Intech.Windows.Forms.SectionEditorControl();
            this.LevelTab = new System.Windows.Forms.TabPage();
            this.LevelGrid = new Intech.Windows.Forms.SectionEditorControl();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.AreaTab = new System.Windows.Forms.TabPage();
            this.AreaGrid = new Intech.Windows.Forms.SectionEditorControl();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.Confirm = new System.Windows.Forms.Button();
            this.Export = new System.Windows.Forms.Button();
            this.Import = new System.Windows.Forms.Button();
            this.SheetTabController.SuspendLayout();
            this.BaseControlTab.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.ScaleTab.SuspendLayout();
            this.DisciplineTab.SuspendLayout();
            this.LevelTab.SuspendLayout();
            this.AreaTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // SheetTabController
            // 
            this.SheetTabController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SheetTabController.Controls.Add(this.BaseControlTab);
            this.SheetTabController.Controls.Add(this.ScaleTab);
            this.SheetTabController.Controls.Add(this.DisciplineTab);
            this.SheetTabController.Controls.Add(this.LevelTab);
            this.SheetTabController.Controls.Add(this.AreaTab);
            this.SheetTabController.Location = new System.Drawing.Point(0, 2);
            this.SheetTabController.Margin = new System.Windows.Forms.Padding(2);
            this.SheetTabController.Name = "SheetTabController";
            this.SheetTabController.SelectedIndex = 0;
            this.SheetTabController.Size = new System.Drawing.Size(574, 298);
            this.SheetTabController.TabIndex = 0;
            // 
            // BaseControlTab
            // 
            this.BaseControlTab.Controls.Add(this.TitleBlockType);
            this.BaseControlTab.Controls.Add(this.label7);
            this.BaseControlTab.Controls.Add(this.label6);
            this.BaseControlTab.Controls.Add(this.TitleBlockFamily);
            this.BaseControlTab.Controls.Add(this.label5);
            this.BaseControlTab.Controls.Add(this.label4);
            this.BaseControlTab.Controls.Add(this.label3);
            this.BaseControlTab.Controls.Add(this.MiddleSheetNumber);
            this.BaseControlTab.Controls.Add(this.label2);
            this.BaseControlTab.Controls.Add(this.TradeAbriviation);
            this.BaseControlTab.Controls.Add(this.label1);
            this.BaseControlTab.Controls.Add(this.tabControl2);
            this.BaseControlTab.Location = new System.Drawing.Point(4, 22);
            this.BaseControlTab.Margin = new System.Windows.Forms.Padding(2);
            this.BaseControlTab.Name = "BaseControlTab";
            this.BaseControlTab.Padding = new System.Windows.Forms.Padding(2);
            this.BaseControlTab.Size = new System.Drawing.Size(566, 272);
            this.BaseControlTab.TabIndex = 0;
            this.BaseControlTab.Text = "Base Controls";
            this.BaseControlTab.UseVisualStyleBackColor = true;
            this.BaseControlTab.Click += new System.EventHandler(this.BaseControlTab_Click);
            // 
            // TitleBlockType
            // 
            this.TitleBlockType.DefaultValue = null;
            this.TitleBlockType.FormattingEnabled = true;
            this.TitleBlockType.Location = new System.Drawing.Point(8, 98);
            this.TitleBlockType.Margin = new System.Windows.Forms.Padding(2);
            this.TitleBlockType.Name = "TitleBlockType";
            this.TitleBlockType.Size = new System.Drawing.Size(114, 21);
            this.TitleBlockType.TabIndex = 25;
            this.TitleBlockType.SelectedIndexChanged += new System.EventHandler(this.TitleBlockType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 83);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Title Block Type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 45);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Title Block Family";
            // 
            // TitleBlockFamily
            // 
            this.TitleBlockFamily.DefaultValue = null;
            this.TitleBlockFamily.FormattingEnabled = true;
            this.TitleBlockFamily.Location = new System.Drawing.Point(8, 61);
            this.TitleBlockFamily.Margin = new System.Windows.Forms.Padding(2);
            this.TitleBlockFamily.Name = "TitleBlockFamily";
            this.TitleBlockFamily.Size = new System.Drawing.Size(114, 21);
            this.TitleBlockFamily.TabIndex = 22;
            this.TitleBlockFamily.SelectedIndexChanged += new System.EventHandler(this.TitleBlockFamily_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(82, 31);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Example M111A1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(80, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "*Other characters auto fill";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(54, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "1A1";
            // 
            // MiddleSheetNumber
            // 
            this.MiddleSheetNumber.Location = new System.Drawing.Point(36, 24);
            this.MiddleSheetNumber.Margin = new System.Windows.Forms.Padding(2);
            this.MiddleSheetNumber.Name = "MiddleSheetNumber";
            this.MiddleSheetNumber.Size = new System.Drawing.Size(16, 20);
            this.MiddleSheetNumber.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "1";
            // 
            // TradeAbriviation
            // 
            this.TradeAbriviation.Location = new System.Drawing.Point(9, 24);
            this.TradeAbriviation.Margin = new System.Windows.Forms.Padding(2);
            this.TradeAbriviation.Name = "TradeAbriviation";
            this.TradeAbriviation.Size = new System.Drawing.Size(16, 20);
            this.TradeAbriviation.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sheet Number";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(322, 300);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(6, 6);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(0, 0);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage4.Size = new System.Drawing.Size(0, 0);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // ScaleTab
            // 
            this.ScaleTab.Controls.Add(this.ScaleGrid);
            this.ScaleTab.Location = new System.Drawing.Point(4, 22);
            this.ScaleTab.Margin = new System.Windows.Forms.Padding(2);
            this.ScaleTab.Name = "ScaleTab";
            this.ScaleTab.Padding = new System.Windows.Forms.Padding(2);
            this.ScaleTab.Size = new System.Drawing.Size(566, 272);
            this.ScaleTab.TabIndex = 1;
            this.ScaleTab.Text = "Scale Tab";
            this.ScaleTab.UseVisualStyleBackColor = true;
            // 
            // ScaleGrid
            // 
            this.ScaleGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleGrid.Location = new System.Drawing.Point(6, 3);
            this.ScaleGrid.Margin = new System.Windows.Forms.Padding(2);
            this.ScaleGrid.Name = "ScaleGrid";
            this.ScaleGrid.Size = new System.Drawing.Size(529, 274);
            this.ScaleGrid.TabIndex = 0;
            // 
            // DisciplineTab
            // 
            this.DisciplineTab.Controls.Add(this.SubDisciplineGrid);
            this.DisciplineTab.Controls.Add(this.SubDisciplineCheck);
            this.DisciplineTab.Controls.Add(this.DisciplineGrid);
            this.DisciplineTab.Location = new System.Drawing.Point(4, 22);
            this.DisciplineTab.Margin = new System.Windows.Forms.Padding(2);
            this.DisciplineTab.Name = "DisciplineTab";
            this.DisciplineTab.Size = new System.Drawing.Size(566, 272);
            this.DisciplineTab.TabIndex = 2;
            this.DisciplineTab.Text = "Discipline Tab";
            this.DisciplineTab.UseVisualStyleBackColor = true;
            // 
            // SubDisciplineGrid
            // 
            this.SubDisciplineGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SubDisciplineGrid.Location = new System.Drawing.Point(368, 25);
            this.SubDisciplineGrid.Margin = new System.Windows.Forms.Padding(2);
            this.SubDisciplineGrid.Name = "SubDisciplineGrid";
            this.SubDisciplineGrid.Size = new System.Drawing.Size(191, 243);
            this.SubDisciplineGrid.TabIndex = 4;
            // 
            // SubDisciplineCheck
            // 
            this.SubDisciplineCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SubDisciplineCheck.AutoSize = true;
            this.SubDisciplineCheck.Checked = true;
            this.SubDisciplineCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SubDisciplineCheck.Location = new System.Drawing.Point(379, 4);
            this.SubDisciplineCheck.Margin = new System.Windows.Forms.Padding(2);
            this.SubDisciplineCheck.Name = "SubDisciplineCheck";
            this.SubDisciplineCheck.Size = new System.Drawing.Size(110, 17);
            this.SubDisciplineCheck.TabIndex = 3;
            this.SubDisciplineCheck.Text = "Has Sub Dicipline";
            this.SubDisciplineCheck.UseVisualStyleBackColor = true;
            // 
            // DisciplineGrid
            // 
            this.DisciplineGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisciplineGrid.Location = new System.Drawing.Point(6, 4);
            this.DisciplineGrid.Margin = new System.Windows.Forms.Padding(2);
            this.DisciplineGrid.Name = "DisciplineGrid";
            this.DisciplineGrid.Size = new System.Drawing.Size(358, 266);
            this.DisciplineGrid.TabIndex = 0;
            // 
            // LevelTab
            // 
            this.LevelTab.Controls.Add(this.LevelGrid);
            this.LevelTab.Controls.Add(this.label9);
            this.LevelTab.Controls.Add(this.label8);
            this.LevelTab.Location = new System.Drawing.Point(4, 22);
            this.LevelTab.Margin = new System.Windows.Forms.Padding(2);
            this.LevelTab.Name = "LevelTab";
            this.LevelTab.Size = new System.Drawing.Size(566, 272);
            this.LevelTab.TabIndex = 3;
            this.LevelTab.Text = "Nonstandard Level Tab";
            this.LevelTab.UseVisualStyleBackColor = true;
            // 
            // LevelGrid
            // 
            this.LevelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LevelGrid.Location = new System.Drawing.Point(8, 34);
            this.LevelGrid.Margin = new System.Windows.Forms.Padding(2);
            this.LevelGrid.Name = "LevelGrid";
            this.LevelGrid.Size = new System.Drawing.Size(522, 243);
            this.LevelGrid.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 19);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(221, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Standard TitleBlock Parameter Level: Level-1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 6);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(151, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Standard Level Name: Level 1";
            // 
            // AreaTab
            // 
            this.AreaTab.Controls.Add(this.AreaGrid);
            this.AreaTab.Controls.Add(this.label11);
            this.AreaTab.Controls.Add(this.label10);
            this.AreaTab.Location = new System.Drawing.Point(4, 22);
            this.AreaTab.Margin = new System.Windows.Forms.Padding(2);
            this.AreaTab.Name = "AreaTab";
            this.AreaTab.Size = new System.Drawing.Size(566, 272);
            this.AreaTab.TabIndex = 4;
            this.AreaTab.Text = "Nonstandard Area Tag";
            this.AreaTab.UseVisualStyleBackColor = true;
            // 
            // AreaGrid
            // 
            this.AreaGrid.Location = new System.Drawing.Point(3, 33);
            this.AreaGrid.Margin = new System.Windows.Forms.Padding(2);
            this.AreaGrid.Name = "AreaGrid";
            this.AreaGrid.Size = new System.Drawing.Size(512, 245);
            this.AreaGrid.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 18);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(233, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Standard TitleBlock Parameter Name: (word)-A1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 5);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(184, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Standard ScopeBox Name: (word) A1";
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(511, 304);
            this.Cancel.Margin = new System.Windows.Forms.Padding(2);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(56, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Confirm
            // 
            this.Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Confirm.Location = new System.Drawing.Point(450, 304);
            this.Confirm.Margin = new System.Windows.Forms.Padding(2);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(56, 23);
            this.Confirm.TabIndex = 3;
            this.Confirm.Text = "Confirm";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Export
            // 
            this.Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Export.Location = new System.Drawing.Point(9, 304);
            this.Export.Margin = new System.Windows.Forms.Padding(2);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(56, 23);
            this.Export.TabIndex = 4;
            this.Export.Text = "Export";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // Import
            // 
            this.Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Import.Location = new System.Drawing.Point(70, 304);
            this.Import.Margin = new System.Windows.Forms.Padding(2);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(56, 23);
            this.Import.TabIndex = 5;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // SheetSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 337);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.Import);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.SheetTabController);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SheetSettings";
            this.Text = "SheetSettings";
            this.SheetTabController.ResumeLayout(false);
            this.BaseControlTab.ResumeLayout(false);
            this.BaseControlTab.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.ScaleTab.ResumeLayout(false);
            this.DisciplineTab.ResumeLayout(false);
            this.DisciplineTab.PerformLayout();
            this.LevelTab.ResumeLayout(false);
            this.LevelTab.PerformLayout();
            this.AreaTab.ResumeLayout(false);
            this.AreaTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl SheetTabController;
        private System.Windows.Forms.TabPage BaseControlTab;
        private System.Windows.Forms.TabPage ScaleTab;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.TextBox TradeAbriviation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MiddleSheetNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Intech.Windows.CustomWindowsForms.FilteredComboBox TitleBlockFamily;
        private Intech.Windows.CustomWindowsForms.FilteredComboBox TitleBlockType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage DisciplineTab;
        private System.Windows.Forms.TabPage LevelTab;
        private Intech.Windows.Forms.SectionEditorControl DisciplineGrid;
        private System.Windows.Forms.CheckBox SubDisciplineCheck;
        private System.Windows.Forms.TabPage AreaTab;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private Intech.Windows.Forms.SectionEditorControl AreaGrid;
        private Intech.Windows.Forms.SectionEditorControl ScaleGrid;
        private Intech.Windows.Forms.SectionEditorControl LevelGrid;
        private Intech.Windows.Forms.SectionEditorControl SubDisciplineGrid;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.Button Import;
    }
}
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
            this.TitleBlockType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TitleBlockFamily = new System.Windows.Forms.ComboBox();
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
            this.ScaleGrid = new System.Windows.Forms.DataGridView();
            this.Scale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ViewportId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RevitScaleValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisciplineTab = new System.Windows.Forms.TabPage();
            this.SubDisciplineGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubDisciplineCheck = new System.Windows.Forms.CheckBox();
            this.DisciplineGrid = new System.Windows.Forms.DataGridView();
            this.DisciplineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DisciplineNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleBlockParameterDiscipline = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelTab = new System.Windows.Forms.TabPage();
            this.LevelGrid = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.AreaTab = new System.Windows.Forms.TabPage();
            this.AreaGrid = new System.Windows.Forms.DataGridView();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.Confirm = new System.Windows.Forms.Button();
            this.Export = new System.Windows.Forms.Button();
            this.Import = new System.Windows.Forms.Button();
            this.LevelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScopeBoxName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TitleBlockParameterArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SheetNumberNumberArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SheetTabController.SuspendLayout();
            this.BaseControlTab.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.ScaleTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleGrid)).BeginInit();
            this.DisciplineTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SubDisciplineGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisciplineGrid)).BeginInit();
            this.LevelTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LevelGrid)).BeginInit();
            this.AreaTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AreaGrid)).BeginInit();
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
            this.SheetTabController.Name = "SheetTabController";
            this.SheetTabController.SelectedIndex = 0;
            this.SheetTabController.Size = new System.Drawing.Size(699, 374);
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
            this.BaseControlTab.Location = new System.Drawing.Point(4, 25);
            this.BaseControlTab.Name = "BaseControlTab";
            this.BaseControlTab.Padding = new System.Windows.Forms.Padding(3);
            this.BaseControlTab.Size = new System.Drawing.Size(691, 345);
            this.BaseControlTab.TabIndex = 0;
            this.BaseControlTab.Text = "Base Controls";
            this.BaseControlTab.UseVisualStyleBackColor = true;
            this.BaseControlTab.Click += new System.EventHandler(this.BaseControlTab_Click);
            // 
            // TitleBlockType
            // 
            this.TitleBlockType.FormattingEnabled = true;
            this.TitleBlockType.Location = new System.Drawing.Point(11, 121);
            this.TitleBlockType.Name = "TitleBlockType";
            this.TitleBlockType.Size = new System.Drawing.Size(121, 24);
            this.TitleBlockType.TabIndex = 25;
            this.TitleBlockType.SelectedIndexChanged += new System.EventHandler(this.TitleBlockType_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 16);
            this.label7.TabIndex = 24;
            this.label7.Text = "Title Block Type";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 16);
            this.label6.TabIndex = 23;
            this.label6.Text = "Title Block Family";
            // 
            // TitleBlockFamily
            // 
            this.TitleBlockFamily.FormattingEnabled = true;
            this.TitleBlockFamily.Location = new System.Drawing.Point(11, 75);
            this.TitleBlockFamily.Name = "TitleBlockFamily";
            this.TitleBlockFamily.Size = new System.Drawing.Size(121, 24);
            this.TitleBlockFamily.TabIndex = 22;
            this.TitleBlockFamily.SelectedIndexChanged += new System.EventHandler(this.TitleBlockFamily_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(110, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 16);
            this.label5.TabIndex = 21;
            this.label5.Text = "Example M111A1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(106, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 16);
            this.label4.TabIndex = 20;
            this.label4.Text = "*Other characters auto fill";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 16);
            this.label3.TabIndex = 19;
            this.label3.Text = "1A1";
            // 
            // MiddleSheetNumber
            // 
            this.MiddleSheetNumber.Location = new System.Drawing.Point(48, 30);
            this.MiddleSheetNumber.Name = "MiddleSheetNumber";
            this.MiddleSheetNumber.Size = new System.Drawing.Size(20, 22);
            this.MiddleSheetNumber.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "1";
            // 
            // TradeAbriviation
            // 
            this.TradeAbriviation.Location = new System.Drawing.Point(12, 30);
            this.TradeAbriviation.Name = "TradeAbriviation";
            this.TradeAbriviation.Size = new System.Drawing.Size(20, 22);
            this.TradeAbriviation.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sheet Number";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(429, 369);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(8, 8);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(0, 0);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(0, 0);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // ScaleTab
            // 
            this.ScaleTab.Controls.Add(this.ScaleGrid);
            this.ScaleTab.Location = new System.Drawing.Point(4, 25);
            this.ScaleTab.Name = "ScaleTab";
            this.ScaleTab.Padding = new System.Windows.Forms.Padding(3);
            this.ScaleTab.Size = new System.Drawing.Size(691, 345);
            this.ScaleTab.TabIndex = 1;
            this.ScaleTab.Text = "Scale Tab";
            this.ScaleTab.UseVisualStyleBackColor = true;
            // 
            // ScaleGrid
            // 
            this.ScaleGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScaleGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Scale,
            this.ViewportId,
            this.RevitScaleValue});
            this.ScaleGrid.Location = new System.Drawing.Point(8, 4);
            this.ScaleGrid.Name = "ScaleGrid";
            this.ScaleGrid.RowHeadersWidth = 51;
            this.ScaleGrid.RowTemplate.Height = 24;
            this.ScaleGrid.Size = new System.Drawing.Size(639, 345);
            this.ScaleGrid.TabIndex = 0;
            // 
            // Scale
            // 
            this.Scale.HeaderText = "Scale";
            this.Scale.MinimumWidth = 6;
            this.Scale.Name = "Scale";
            this.Scale.Width = 125;
            // 
            // ViewportId
            // 
            this.ViewportId.HeaderText = "ViewportId";
            this.ViewportId.MinimumWidth = 6;
            this.ViewportId.Name = "ViewportId";
            this.ViewportId.Width = 125;
            // 
            // RevitScaleValue
            // 
            this.RevitScaleValue.HeaderText = "Revit Scale Value";
            this.RevitScaleValue.MinimumWidth = 6;
            this.RevitScaleValue.Name = "RevitScaleValue";
            this.RevitScaleValue.Width = 125;
            // 
            // DisciplineTab
            // 
            this.DisciplineTab.Controls.Add(this.SubDisciplineGrid);
            this.DisciplineTab.Controls.Add(this.SubDisciplineCheck);
            this.DisciplineTab.Controls.Add(this.DisciplineGrid);
            this.DisciplineTab.Location = new System.Drawing.Point(4, 25);
            this.DisciplineTab.Name = "DisciplineTab";
            this.DisciplineTab.Size = new System.Drawing.Size(691, 345);
            this.DisciplineTab.TabIndex = 2;
            this.DisciplineTab.Text = "Discipline Tab";
            this.DisciplineTab.UseVisualStyleBackColor = true;
            // 
            // SubDisciplineGrid
            // 
            this.SubDisciplineGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SubDisciplineGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SubDisciplineGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4});
            this.SubDisciplineGrid.Location = new System.Drawing.Point(471, 33);
            this.SubDisciplineGrid.Name = "SubDisciplineGrid";
            this.SubDisciplineGrid.RowHeadersWidth = 51;
            this.SubDisciplineGrid.RowTemplate.Height = 24;
            this.SubDisciplineGrid.Size = new System.Drawing.Size(211, 307);
            this.SubDisciplineGrid.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Sub Discipline";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 125;
            // 
            // SubDisciplineCheck
            // 
            this.SubDisciplineCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SubDisciplineCheck.AutoSize = true;
            this.SubDisciplineCheck.Checked = true;
            this.SubDisciplineCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SubDisciplineCheck.Location = new System.Drawing.Point(471, 7);
            this.SubDisciplineCheck.Name = "SubDisciplineCheck";
            this.SubDisciplineCheck.Size = new System.Drawing.Size(136, 20);
            this.SubDisciplineCheck.TabIndex = 3;
            this.SubDisciplineCheck.Text = "Has Sub Dicipline";
            this.SubDisciplineCheck.UseVisualStyleBackColor = true;
            // 
            // DisciplineGrid
            // 
            this.DisciplineGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisciplineGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DisciplineGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DisciplineName,
            this.DisciplineNumber,
            this.TitleBlockParameterDiscipline});
            this.DisciplineGrid.Location = new System.Drawing.Point(8, 5);
            this.DisciplineGrid.Name = "DisciplineGrid";
            this.DisciplineGrid.RowHeadersWidth = 51;
            this.DisciplineGrid.RowTemplate.Height = 24;
            this.DisciplineGrid.Size = new System.Drawing.Size(457, 335);
            this.DisciplineGrid.TabIndex = 0;
            // 
            // DisciplineName
            // 
            this.DisciplineName.HeaderText = "Discipline";
            this.DisciplineName.MinimumWidth = 6;
            this.DisciplineName.Name = "DisciplineName";
            this.DisciplineName.Width = 125;
            // 
            // DisciplineNumber
            // 
            this.DisciplineNumber.HeaderText = "Discipline Number (M__11A1)";
            this.DisciplineNumber.MinimumWidth = 6;
            this.DisciplineNumber.Name = "DisciplineNumber";
            this.DisciplineNumber.Width = 125;
            // 
            // TitleBlockParameterDiscipline
            // 
            this.TitleBlockParameterDiscipline.HeaderText = "Title Block Parameter";
            this.TitleBlockParameterDiscipline.MinimumWidth = 6;
            this.TitleBlockParameterDiscipline.Name = "TitleBlockParameterDiscipline";
            this.TitleBlockParameterDiscipline.Width = 125;
            // 
            // LevelTab
            // 
            this.LevelTab.Controls.Add(this.LevelGrid);
            this.LevelTab.Controls.Add(this.label9);
            this.LevelTab.Controls.Add(this.label8);
            this.LevelTab.Location = new System.Drawing.Point(4, 25);
            this.LevelTab.Name = "LevelTab";
            this.LevelTab.Size = new System.Drawing.Size(691, 345);
            this.LevelTab.TabIndex = 3;
            this.LevelTab.Text = "Nonstandard Level Tab";
            this.LevelTab.UseVisualStyleBackColor = true;
            // 
            // LevelGrid
            // 
            this.LevelGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LevelGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LevelGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LevelName,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.LevelGrid.Location = new System.Drawing.Point(11, 42);
            this.LevelGrid.Name = "LevelGrid";
            this.LevelGrid.RowHeadersWidth = 51;
            this.LevelGrid.RowTemplate.Height = 24;
            this.LevelGrid.Size = new System.Drawing.Size(630, 306);
            this.LevelGrid.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(277, 16);
            this.label9.TabIndex = 1;
            this.label9.Text = "Standard TitleBlock Parameter Level: Level-1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(187, 16);
            this.label8.TabIndex = 0;
            this.label8.Text = "Standard Level Name: Level 1";
            // 
            // AreaTab
            // 
            this.AreaTab.Controls.Add(this.AreaGrid);
            this.AreaTab.Controls.Add(this.label11);
            this.AreaTab.Controls.Add(this.label10);
            this.AreaTab.Location = new System.Drawing.Point(4, 25);
            this.AreaTab.Name = "AreaTab";
            this.AreaTab.Size = new System.Drawing.Size(691, 345);
            this.AreaTab.TabIndex = 4;
            this.AreaTab.Text = "Nonstandard Area Tag";
            this.AreaTab.UseVisualStyleBackColor = true;
            // 
            // AreaGrid
            // 
            this.AreaGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AreaGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ScopeBoxName,
            this.TitleBlockParameterArea,
            this.SheetNumberNumberArea});
            this.AreaGrid.Location = new System.Drawing.Point(4, 41);
            this.AreaGrid.Name = "AreaGrid";
            this.AreaGrid.RowHeadersWidth = 51;
            this.AreaGrid.RowTemplate.Height = 24;
            this.AreaGrid.Size = new System.Drawing.Size(682, 301);
            this.AreaGrid.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(294, 16);
            this.label11.TabIndex = 2;
            this.label11.Text = "Standard TitleBlock Parameter Name: (word)-A1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(230, 16);
            this.label10.TabIndex = 1;
            this.label10.Text = "Standard ScopeBox Name: (word) A1";
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(615, 387);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Confirm
            // 
            this.Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Confirm.Location = new System.Drawing.Point(534, 387);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(75, 23);
            this.Confirm.TabIndex = 3;
            this.Confirm.Text = "Confirm";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // Export
            // 
            this.Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Export.Location = new System.Drawing.Point(12, 387);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(75, 23);
            this.Export.TabIndex = 4;
            this.Export.Text = "Export";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // Import
            // 
            this.Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Import.Location = new System.Drawing.Point(93, 387);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(75, 23);
            this.Import.TabIndex = 5;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // LevelName
            // 
            this.LevelName.HeaderText = "Level Name";
            this.LevelName.MinimumWidth = 6;
            this.LevelName.Name = "LevelName";
            this.LevelName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LevelName.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Title Block Parameter";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Sheet Number Value (M11__A1)";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 125;
            // 
            // ScopeBoxName
            // 
            this.ScopeBoxName.HeaderText = "Scope Box Name";
            this.ScopeBoxName.MinimumWidth = 6;
            this.ScopeBoxName.Name = "ScopeBoxName";
            this.ScopeBoxName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ScopeBoxName.Width = 125;
            // 
            // TitleBlockParameterArea
            // 
            this.TitleBlockParameterArea.HeaderText = "Title Block Parameter";
            this.TitleBlockParameterArea.MinimumWidth = 6;
            this.TitleBlockParameterArea.Name = "TitleBlockParameterArea";
            this.TitleBlockParameterArea.Width = 125;
            // 
            // SheetNumberNumberArea
            // 
            this.SheetNumberNumberArea.HeaderText = "Sheet Number Area Value (M111__)";
            this.SheetNumberNumberArea.MinimumWidth = 6;
            this.SheetNumberNumberArea.Name = "SheetNumberNumberArea";
            this.SheetNumberNumberArea.Width = 125;
            // 
            // SheetSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 422);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.Import);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.SheetTabController);
            this.Name = "SheetSettings";
            this.Text = "SheetSettings";
            this.SheetTabController.ResumeLayout(false);
            this.BaseControlTab.ResumeLayout(false);
            this.BaseControlTab.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.ScaleTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScaleGrid)).EndInit();
            this.DisciplineTab.ResumeLayout(false);
            this.DisciplineTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SubDisciplineGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DisciplineGrid)).EndInit();
            this.LevelTab.ResumeLayout(false);
            this.LevelTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LevelGrid)).EndInit();
            this.AreaTab.ResumeLayout(false);
            this.AreaTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AreaGrid)).EndInit();
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
        private System.Windows.Forms.ComboBox TitleBlockFamily;
        private System.Windows.Forms.ComboBox TitleBlockType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage DisciplineTab;
        private System.Windows.Forms.TabPage LevelTab;
        private System.Windows.Forms.DataGridView DisciplineGrid;
        private System.Windows.Forms.CheckBox SubDisciplineCheck;
        private System.Windows.Forms.TabPage AreaTab;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView AreaGrid;
        private System.Windows.Forms.DataGridView ScaleGrid;
        private System.Windows.Forms.DataGridView LevelGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisciplineName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DisciplineNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scale;
        private System.Windows.Forms.DataGridViewTextBoxColumn ViewportId;
        private System.Windows.Forms.DataGridViewTextBoxColumn RevitScaleValue;
        private System.Windows.Forms.DataGridView SubDisciplineGrid;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleBlockParameterDiscipline;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScopeBoxName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleBlockParameterArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn SheetNumberNumberArea;
    }
}
namespace Intech.Sleeve
{
    partial class SleeveSettings
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
            this.structCombo = new System.Windows.Forms.ComboBox();
            this.Save = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.RoundSleeveFamilySelect = new Intech.Windows.CustomWindowsForms.FilteredComboBox();
            this.RoundTypeFamilySelect = new Intech.Windows.CustomWindowsForms.FilteredComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.RectTypeFamilySelect = new Intech.Windows.CustomWindowsForms.FilteredComboBox();
            this.RectSleeveFamilySelect = new Intech.Windows.CustomWindowsForms.FilteredComboBox();
            this.SuspendLayout();
            // 
            // structCombo
            // 
            this.structCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.structCombo.FormattingEnabled = true;
            this.structCombo.Location = new System.Drawing.Point(100, 9);
            this.structCombo.Name = "structCombo";
            this.structCombo.Size = new System.Drawing.Size(402, 21);
            this.structCombo.TabIndex = 0;
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.Location = new System.Drawing.Point(333, 447);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save...";
            this.Save.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(423, 447);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(82, 13);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Structural Model";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RoundSleeveFamilySelect
            // 
            this.RoundSleeveFamilySelect.DefaultValue = null;
            this.RoundSleeveFamilySelect.FormattingEnabled = true;
            this.RoundSleeveFamilySelect.Location = new System.Drawing.Point(100, 48);
            this.RoundSleeveFamilySelect.Name = "RoundSleeveFamilySelect";
            this.RoundSleeveFamilySelect.Size = new System.Drawing.Size(184, 21);
            this.RoundSleeveFamilySelect.TabIndex = 4;
            this.RoundSleeveFamilySelect.SelectedIndexChanged += new System.EventHandler(this.SleeveFamilySelect_SelectedIndexChanged);
            // 
            // RoundTypeFamilySelect
            // 
            this.RoundTypeFamilySelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RoundTypeFamilySelect.DefaultValue = null;
            this.RoundTypeFamilySelect.FormattingEnabled = true;
            this.RoundTypeFamilySelect.Location = new System.Drawing.Point(316, 48);
            this.RoundTypeFamilySelect.Name = "RoundTypeFamilySelect";
            this.RoundTypeFamilySelect.Size = new System.Drawing.Size(186, 21);
            this.RoundTypeFamilySelect.TabIndex = 5;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(12, 51);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(82, 13);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "Round Sleeve";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(100, 32);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(184, 13);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "Family";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(316, 32);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(186, 13);
            this.textBox4.TabIndex = 8;
            this.textBox4.Text = "Type";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Location = new System.Drawing.Point(316, 75);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(186, 13);
            this.textBox5.TabIndex = 13;
            this.textBox5.Text = "Type";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Location = new System.Drawing.Point(100, 75);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(184, 13);
            this.textBox6.TabIndex = 12;
            this.textBox6.Text = "Family";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox7.Location = new System.Drawing.Point(12, 94);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(82, 13);
            this.textBox7.TabIndex = 11;
            this.textBox7.Text = "Rect Sleeve";
            this.textBox7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // RectTypeFamilySelect
            // 
            this.RectTypeFamilySelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RectTypeFamilySelect.DefaultValue = null;
            this.RectTypeFamilySelect.FormattingEnabled = true;
            this.RectTypeFamilySelect.Location = new System.Drawing.Point(316, 91);
            this.RectTypeFamilySelect.Name = "RectTypeFamilySelect";
            this.RectTypeFamilySelect.Size = new System.Drawing.Size(186, 21);
            this.RectTypeFamilySelect.TabIndex = 10;
            // 
            // RectSleeveFamilySelect
            // 
            this.RectSleeveFamilySelect.DefaultValue = null;
            this.RectSleeveFamilySelect.FormattingEnabled = true;
            this.RectSleeveFamilySelect.Location = new System.Drawing.Point(100, 91);
            this.RectSleeveFamilySelect.Name = "RectSleeveFamilySelect";
            this.RectSleeveFamilySelect.Size = new System.Drawing.Size(184, 21);
            this.RectSleeveFamilySelect.TabIndex = 9;
            this.RectSleeveFamilySelect.SelectedIndexChanged += new System.EventHandler(this.RectSleeveFamilySelect_SelectedIndexChanged);
            // 
            // SleeveSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 482);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.RectTypeFamilySelect);
            this.Controls.Add(this.RectSleeveFamilySelect);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.RoundTypeFamilySelect);
            this.Controls.Add(this.RoundSleeveFamilySelect);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.structCombo);
            this.Name = "SleeveSettings";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox structCombo;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox textBox1;
        private Windows.CustomWindowsForms.FilteredComboBox RoundSleeveFamilySelect;
        private Windows.CustomWindowsForms.FilteredComboBox RoundTypeFamilySelect;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox7;
        private Windows.CustomWindowsForms.FilteredComboBox RectTypeFamilySelect;
        private Windows.CustomWindowsForms.FilteredComboBox RectSleeveFamilySelect;
    }
}
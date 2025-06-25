namespace TitleBlockSetup.Find_Replace
{
    partial class FindAndReplaceUI
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
            this.Confirm = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SelectionButton = new System.Windows.Forms.Button();
            this.parameterBox = new Intech.Windows.CustomWindowsForms.FilteredComboBox();
            this.find = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.replace = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Confirm
            // 
            this.Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Confirm.Location = new System.Drawing.Point(12, 148);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(75, 23);
            this.Confirm.TabIndex = 0;
            this.Confirm.Text = "Confirm...";
            this.Confirm.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(236, 148);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // SelectionButton
            // 
            this.SelectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectionButton.Location = new System.Drawing.Point(47, 9);
            this.SelectionButton.Name = "SelectionButton";
            this.SelectionButton.Size = new System.Drawing.Size(235, 23);
            this.SelectionButton.TabIndex = 2;
            this.SelectionButton.Text = "Change Selection";
            this.SelectionButton.UseVisualStyleBackColor = true;
            // 
            // parameterBox
            // 
            this.parameterBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterBox.DefaultValue = null;
            this.parameterBox.FormattingEnabled = true;
            this.parameterBox.Location = new System.Drawing.Point(79, 55);
            this.parameterBox.Name = "parameterBox";
            this.parameterBox.Size = new System.Drawing.Size(158, 21);
            this.parameterBox.TabIndex = 3;
            // 
            // find
            // 
            this.find.Location = new System.Drawing.Point(12, 112);
            this.find.Name = "find";
            this.find.Size = new System.Drawing.Size(143, 20);
            this.find.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Location = new System.Drawing.Point(79, 38);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(158, 13);
            this.textBox3.TabIndex = 6;
            this.textBox3.Text = "Parameter";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // replace
            // 
            this.replace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.replace.Location = new System.Drawing.Point(168, 112);
            this.replace.Name = "replace";
            this.replace.Size = new System.Drawing.Size(143, 20);
            this.replace.TabIndex = 7;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Location = new System.Drawing.Point(12, 93);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(143, 13);
            this.textBox4.TabIndex = 8;
            this.textBox4.Text = "Find";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            this.textBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox5.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Location = new System.Drawing.Point(168, 93);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(143, 13);
            this.textBox5.TabIndex = 9;
            this.textBox5.Text = "Replace";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FindAndReplaceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 183);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.replace);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.find);
            this.Controls.Add(this.parameterBox);
            this.Controls.Add(this.SelectionButton);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Confirm);
            this.Name = "FindAndReplaceUI";
            this.Text = "FindAndReplaceUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button SelectionButton;
        private Intech.Windows.CustomWindowsForms.FilteredComboBox parameterBox;
        private System.Windows.Forms.TextBox find;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox replace;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
    }
}
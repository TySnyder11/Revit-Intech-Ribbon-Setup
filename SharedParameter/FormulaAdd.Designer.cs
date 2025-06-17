using System.Windows.Forms;

namespace Intech.SharedParameter
{
    partial class FormulaAdd
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
            this.parameterTypes = new Intech.Windows.CustomWindowsForms.FilterableTreeView();
            this.FormulaTextBox = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.CheckFormula = new System.Windows.Forms.Button();
            this.paramSearch = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // parameterTypes
            // 
            this.parameterTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.parameterTypes.Location = new System.Drawing.Point(12, 53);
            this.parameterTypes.Name = "parameterTypes";
            this.parameterTypes.Size = new System.Drawing.Size(292, 257);
            this.parameterTypes.TabIndex = 0;
            // 
            // FormulaTextBox
            // 
            this.FormulaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FormulaTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FormulaTextBox.Location = new System.Drawing.Point(12, 332);
            this.FormulaTextBox.Name = "FormulaTextBox";
            this.FormulaTextBox.Size = new System.Drawing.Size(292, 71);
            this.FormulaTextBox.TabIndex = 1;
            this.FormulaTextBox.Text = "=";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(12, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 13);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Parameter Types";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(13, 314);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 13);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "Formula";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(12, 433);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Confirm...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(229, 433);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // CheckFormula
            // 
            this.CheckFormula.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckFormula.Location = new System.Drawing.Point(108, 409);
            this.CheckFormula.Name = "CheckFormula";
            this.CheckFormula.Size = new System.Drawing.Size(96, 23);
            this.CheckFormula.TabIndex = 6;
            this.CheckFormula.Text = "Check Formula";
            this.CheckFormula.UseVisualStyleBackColor = true;
            // 
            // paramSearch
            // 
            this.paramSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paramSearch.Location = new System.Drawing.Point(12, 27);
            this.paramSearch.Name = "paramSearch";
            this.paramSearch.Size = new System.Drawing.Size(292, 20);
            this.paramSearch.TabIndex = 7;
            this.paramSearch.TextChanged += new System.EventHandler(this.paramSearch_TextChanged);
            // 
            // FormulaAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 463);
            this.Controls.Add(this.paramSearch);
            this.Controls.Add(this.CheckFormula);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.FormulaTextBox);
            this.Controls.Add(this.parameterTypes);
            this.Name = "FormulaAdd";
            this.Text = "FormulaAdd";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Intech.Windows.CustomWindowsForms.FilterableTreeView parameterTypes;
        private System.Windows.Forms.RichTextBox FormulaTextBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button CheckFormula;
        private TextBox paramSearch;
    }
}
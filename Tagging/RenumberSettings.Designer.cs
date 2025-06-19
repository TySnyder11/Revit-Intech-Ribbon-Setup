namespace TitleBlockSetup.Tagging
{
    partial class RenumberSettings
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
            this.renumberPanel = new System.Windows.Forms.Panel();
            this.renumberMenu = new Intech.Windows.Forms.SectionEditorControl();
            this.renumberPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // renumberPanel
            // 
            this.renumberPanel.Controls.Add(this.renumberMenu);
            this.renumberPanel.Location = new System.Drawing.Point(39, 12);
            this.renumberPanel.Name = "renumberPanel";
            this.renumberPanel.Size = new System.Drawing.Size(704, 297);
            this.renumberPanel.TabIndex = 0;
            // 
            // renumberMenu
            // 
            this.renumberMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renumberMenu.Location = new System.Drawing.Point(0, 0);
            this.renumberMenu.Name = "renumberMenu";
            this.renumberMenu.Size = new System.Drawing.Size(704, 297);
            this.renumberMenu.TabIndex = 0;
            // 
            // RenumberSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 443);
            this.Controls.Add(this.renumberPanel);
            this.Name = "RenumberSettings";
            this.Text = "RenumberAdd";
            this.renumberPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel renumberPanel;
        private Intech.Windows.Forms.SectionEditorControl renumberMenu;
    }
}
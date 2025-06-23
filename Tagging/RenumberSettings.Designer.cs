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
            this.Confirm = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.AdvancedSettings = new System.Windows.Forms.Button();
            this.renumberPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // renumberPanel
            // 
            this.renumberPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renumberPanel.Controls.Add(this.renumberMenu);
            this.renumberPanel.Location = new System.Drawing.Point(12, 12);
            this.renumberPanel.Name = "renumberPanel";
            this.renumberPanel.Size = new System.Drawing.Size(834, 377);
            this.renumberPanel.TabIndex = 0;
            // 
            // renumberMenu
            // 
            this.renumberMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renumberMenu.Location = new System.Drawing.Point(0, 0);
            this.renumberMenu.Name = "renumberMenu";
            this.renumberMenu.Size = new System.Drawing.Size(834, 377);
            this.renumberMenu.TabIndex = 0;
            // 
            // Confirm
            // 
            this.Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Confirm.Location = new System.Drawing.Point(690, 393);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(75, 23);
            this.Confirm.TabIndex = 1;
            this.Confirm.Text = "Confirm...";
            this.Confirm.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(771, 395);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // AdvancedSettings
            // 
            this.AdvancedSettings.Location = new System.Drawing.Point(12, 395);
            this.AdvancedSettings.Name = "AdvancedSettings";
            this.AdvancedSettings.Size = new System.Drawing.Size(122, 23);
            this.AdvancedSettings.TabIndex = 3;
            this.AdvancedSettings.Text = "Advanced Settings";
            this.AdvancedSettings.UseVisualStyleBackColor = true;
            this.AdvancedSettings.Click += new System.EventHandler(this.AdvancedSettings_Click);
            // 
            // RenumberSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 428);
            this.Controls.Add(this.AdvancedSettings);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.renumberPanel);
            this.Name = "RenumberSettings";
            this.Text = "RenumberAdd";
            this.Load += new System.EventHandler(this.RenumberSettings_Load);
            this.renumberPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel renumberPanel;
        private Intech.Windows.Forms.SectionEditorControl renumberMenu;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button AdvancedSettings;
    }
}
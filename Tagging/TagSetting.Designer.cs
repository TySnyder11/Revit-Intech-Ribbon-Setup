using System;
using System.Reflection;

namespace Intech
{
    partial class TagSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TagSetting));
            this.Save = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.Export = new System.Windows.Forms.Button();
            this.Import = new System.Windows.Forms.Button();
            this.Icon = new System.Windows.Forms.PictureBox();
            this.TagSettings = new Intech.Windows.Forms.SectionEditorControl();
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).BeginInit();
            this.SuspendLayout();
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.Location = new System.Drawing.Point(461, 381);
            this.Save.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(56, 19);
            this.Save.TabIndex = 0;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.Location = new System.Drawing.Point(530, 381);
            this.Cancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(56, 19);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Export
            // 
            this.Export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Export.Location = new System.Drawing.Point(91, 374);
            this.Export.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Export.Name = "Export";
            this.Export.Size = new System.Drawing.Size(56, 26);
            this.Export.TabIndex = 2;
            this.Export.Text = "Export";
            this.Export.UseVisualStyleBackColor = true;
            this.Export.Click += new System.EventHandler(this.Export_Click);
            // 
            // Import
            // 
            this.Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Import.Location = new System.Drawing.Point(151, 373);
            this.Import.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(56, 28);
            this.Import.TabIndex = 3;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // Icon
            // 
            this.Icon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Icon.Image = ((System.Drawing.Image)(resources.GetObject("Icon.Image")));
            this.Icon.Location = new System.Drawing.Point(11, 337);
            this.Icon.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Icon.Name = "Icon";
            this.Icon.Size = new System.Drawing.Size(76, 67);
            this.Icon.TabIndex = 4;
            this.Icon.TabStop = false;
            // 
            // TagSettings
            // 
            this.TagSettings.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left |
                System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            this.TagSettings.Location = new System.Drawing.Point(23, 2);
            this.TagSettings.Name = "TagSettings";
            this.TagSettings.Size = new System.Drawing.Size(560, 330);
            this.TagSettings.TabIndex = 7;
            // 
            // TagSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 410);
            this.Controls.Add(this.TagSettings);
            this.Controls.Add(this.Icon);
            this.Controls.Add(this.Import);
            this.Controls.Add(this.Export);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Save);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TagSetting";
            this.Text = "Tag Settings";
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Export;
        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.PictureBox Icon;
        private Windows.Forms.SectionEditorControl TagSettings;
    }
}
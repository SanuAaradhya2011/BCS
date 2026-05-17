namespace CAB.UI
{
    partial class StandardImport
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
            this.groupBoxImportStandardData = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtBoxFileName = new System.Windows.Forms.TextBox();
            this.lblSelectFile = new System.Windows.Forms.Label();
            this.groupBoxImportStandardData.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxImportStandardData
            // 
            this.groupBoxImportStandardData.Controls.Add(this.btnCancel);
            this.groupBoxImportStandardData.Controls.Add(this.btnImport);
            this.groupBoxImportStandardData.Controls.Add(this.btnBrowse);
            this.groupBoxImportStandardData.Controls.Add(this.txtBoxFileName);
            this.groupBoxImportStandardData.Controls.Add(this.lblSelectFile);
            this.groupBoxImportStandardData.Location = new System.Drawing.Point(8, 8);
            this.groupBoxImportStandardData.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxImportStandardData.Name = "groupBoxImportStandardData";
            this.groupBoxImportStandardData.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxImportStandardData.Size = new System.Drawing.Size(556, 294);
            this.groupBoxImportStandardData.TabIndex = 1;
            this.groupBoxImportStandardData.TabStop = false;
            this.groupBoxImportStandardData.Text = "Import Standard Data";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(413, 218);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 32);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(316, 218);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(89, 32);
            this.btnImport.TabIndex = 18;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(457, 62);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(45, 28);
            this.btnBrowse.TabIndex = 17;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtBoxFileName
            // 
            this.txtBoxFileName.Location = new System.Drawing.Point(159, 64);
            this.txtBoxFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoxFileName.Name = "txtBoxFileName";
            this.txtBoxFileName.ReadOnly = true;
            this.txtBoxFileName.Size = new System.Drawing.Size(289, 22);
            this.txtBoxFileName.TabIndex = 16;
            // 
            // lblSelectFile
            // 
            this.lblSelectFile.AutoSize = true;
            this.lblSelectFile.Location = new System.Drawing.Point(32, 69);
            this.lblSelectFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectFile.Name = "lblSelectFile";
            this.lblSelectFile.Size = new System.Drawing.Size(109, 17);
            this.lblSelectFile.TabIndex = 14;
            this.lblSelectFile.Text = "Select *.EXP file";
            // 
            // StandardImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 312);
            this.Controls.Add(this.groupBoxImportStandardData);
            this.Name = "StandardImport";
            this.Text = "StandardImport";
            this.Load += new System.EventHandler(this.StandardImport_Load);
            this.groupBoxImportStandardData.ResumeLayout(false);
            this.groupBoxImportStandardData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxImportStandardData;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtBoxFileName;
        private System.Windows.Forms.Label lblSelectFile;
    }
}
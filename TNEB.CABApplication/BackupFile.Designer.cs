namespace CAB.UI
{
    partial class BackupFile
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
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.chkFileUploadList = new System.Windows.Forms.CheckedListBox();
			this.chkSelectAll = new System.Windows.Forms.CheckBox();
			this.btnBackup = new System.Windows.Forms.Button();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.btnCancel);
			this.groupBox4.Controls.Add(this.chkFileUploadList);
			this.groupBox4.Controls.Add(this.chkSelectAll);
			this.groupBox4.Controls.Add(this.btnBackup);
			this.groupBox4.Location = new System.Drawing.Point(20, 12);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(397, 365);
			this.groupBox4.TabIndex = 15;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Select CAB File For Backup";
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(276, 314);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// chkFileUploadList
			// 
			this.chkFileUploadList.BackColor = System.Drawing.SystemColors.Menu;
			this.chkFileUploadList.CheckOnClick = true;
			this.chkFileUploadList.FormattingEnabled = true;
			this.chkFileUploadList.Items.AddRange(new object[] {
            "22082008.CAB",
            "23082008.CAB",
            "24082008.CAB"});
			this.chkFileUploadList.Location = new System.Drawing.Point(45, 19);
			this.chkFileUploadList.Name = "chkFileUploadList";
			this.chkFileUploadList.ScrollAlwaysVisible = true;
			this.chkFileUploadList.Size = new System.Drawing.Size(306, 289);
			this.chkFileUploadList.TabIndex = 0;
			// 
			// chkSelectAll
			// 
			this.chkSelectAll.AutoSize = true;
			this.chkSelectAll.Location = new System.Drawing.Point(45, 318);
			this.chkSelectAll.Name = "chkSelectAll";
			this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
			this.chkSelectAll.TabIndex = 0;
			this.chkSelectAll.Text = "Select All";
			this.chkSelectAll.UseVisualStyleBackColor = true;
			this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
			// 
			// btnBackup
			// 
			this.btnBackup.Location = new System.Drawing.Point(195, 314);
			this.btnBackup.Name = "btnBackup";
			this.btnBackup.Size = new System.Drawing.Size(75, 23);
			this.btnBackup.TabIndex = 1;
			this.btnBackup.Text = " Backup";
			this.btnBackup.UseVisualStyleBackColor = true;
			this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
			// 
			// BackupFile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(437, 410);
			this.Controls.Add(this.groupBox4);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "BackupFile";
			this.StatusMessage = "";
			this.Text = "Backup File";
			this.Load += new System.EventHandler(this.BackupFile_Load);
			this.Activated += new System.EventHandler(this.BackupFile_Activated);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckedListBox chkFileUploadList;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.Button btnBackup;
    }
}
namespace CAB.UI
{
    partial class ASCIIExportSettings
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
            this.gbFormate = new System.Windows.Forms.GroupBox();
            this.treeViewAsciiExport = new System.Windows.Forms.TreeView();
            this.cmbDataSeparator = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDataSeparator = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblFormatName = new System.Windows.Forms.Label();
            this.lstFile = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbFormate.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFormate
            // 
            this.gbFormate.Controls.Add(this.treeViewAsciiExport);
            this.gbFormate.Controls.Add(this.cmbDataSeparator);
            this.gbFormate.Controls.Add(this.btnSave);
            this.gbFormate.Controls.Add(this.btnCancel);
            this.gbFormate.Controls.Add(this.lblDataSeparator);
            this.gbFormate.Controls.Add(this.txtFileName);
            this.gbFormate.Controls.Add(this.lblFormatName);
            this.gbFormate.Enabled = false;
            this.gbFormate.Location = new System.Drawing.Point(275, 57);
            this.gbFormate.Name = "gbFormate";
            this.gbFormate.Size = new System.Drawing.Size(398, 401);
            this.gbFormate.TabIndex = 1;
            this.gbFormate.TabStop = false;
            // 
            // treeViewAsciiExport
            // 
            this.treeViewAsciiExport.CheckBoxes = true;
            this.treeViewAsciiExport.Location = new System.Drawing.Point(35, 76);
            this.treeViewAsciiExport.Name = "treeViewAsciiExport";
            this.treeViewAsciiExport.Size = new System.Drawing.Size(325, 279);
            this.treeViewAsciiExport.TabIndex = 6;
            this.treeViewAsciiExport.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewAsciiExport_AfterCheck);
            // 
            // cmbDataSeparator
            // 
            this.cmbDataSeparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDataSeparator.FormattingEnabled = true;
            this.cmbDataSeparator.Items.AddRange(new object[] {
            ",",
            "/",
            "|",
            "\\",
            ":",
            ";"});
            this.cmbDataSeparator.Location = new System.Drawing.Point(118, 41);
            this.cmbDataSeparator.Name = "cmbDataSeparator";
            this.cmbDataSeparator.Size = new System.Drawing.Size(75, 21);
            this.cmbDataSeparator.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(190, 361);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 27);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(271, 361);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblDataSeparator
            // 
            this.lblDataSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.lblDataSeparator.Location = new System.Drawing.Point(45, 43);
            this.lblDataSeparator.Name = "lblDataSeparator";
            this.lblDataSeparator.Size = new System.Drawing.Size(53, 15);
            this.lblDataSeparator.TabIndex = 2;
            this.lblDataSeparator.Text = "Delimiter";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(118, 16);
            this.txtFileName.MaxLength = 20;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(184, 20);
            this.txtFileName.TabIndex = 0;
            // 
            // lblFormatName
            // 
            this.lblFormatName.AutoSize = true;
            this.lblFormatName.Location = new System.Drawing.Point(44, 19);
            this.lblFormatName.Name = "lblFormatName";
            this.lblFormatName.Size = new System.Drawing.Size(68, 13);
            this.lblFormatName.TabIndex = 0;
            this.lblFormatName.Text = "Format name";
            // 
            // lstFile
            // 
            this.lstFile.FormattingEnabled = true;
            this.lstFile.Location = new System.Drawing.Point(11, 64);
            this.lstFile.Margin = new System.Windows.Forms.Padding(2);
            this.lstFile.Name = "lstFile";
            this.lstFile.Size = new System.Drawing.Size(249, 394);
            this.lstFile.TabIndex = 2;
            this.lstFile.SelectedIndexChanged += new System.EventHandler(this.lstFile_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(10, 19);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 27);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(91, 19);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 27);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(172, 19);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 27);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(253, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 27);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ASCIIExportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 586);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lstFile);
            this.Controls.Add(this.gbFormate);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ASCIIExportSettings";
            this.StatusMessage = "";
            this.Text = "ASCII Export Settings";
            this.Load += new System.EventHandler(this.ASCIIExportSettings_Load);
            this.gbFormate.ResumeLayout(false);
            this.gbFormate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFormate;
        private System.Windows.Forms.ComboBox cmbDataSeparator;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDataSeparator;
        public System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblFormatName;
        private System.Windows.Forms.ListBox lstFile;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TreeView treeViewAsciiExport;
    }
}

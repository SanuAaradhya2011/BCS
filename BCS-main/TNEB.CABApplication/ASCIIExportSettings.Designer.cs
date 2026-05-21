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
            this.cmbDataSeparator = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBoxSelectParameters = new System.Windows.Forms.GroupBox();
            this.btnLoadSurveyDataParam = new System.Windows.Forms.Button();
            this.btnTamperDataParam = new System.Windows.Forms.Button();
            this.btnBillingDataParam = new System.Windows.Forms.Button();
            this.btnInstantDataParam = new System.Windows.Forms.Button();
            this.btnGeneralDataParam = new System.Windows.Forms.Button();
            this.chkBoxLoadSurveyData = new System.Windows.Forms.CheckBox();
            this.chkBoxGeneralData = new System.Windows.Forms.CheckBox();
            this.chkBoxBillingData = new System.Windows.Forms.CheckBox();
            this.chkBoxTamperData = new System.Windows.Forms.CheckBox();
            this.chkBoxInstantData = new System.Windows.Forms.CheckBox();
            this.lblDataSeparator = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lblFormatName = new System.Windows.Forms.Label();
            this.lstFile = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbFormate.SuspendLayout();
            this.groupBoxSelectParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFormate
            // 
            this.gbFormate.Controls.Add(this.cmbDataSeparator);
            this.gbFormate.Controls.Add(this.btnSave);
            this.gbFormate.Controls.Add(this.btnCancel);
            this.gbFormate.Controls.Add(this.groupBoxSelectParameters);
            this.gbFormate.Controls.Add(this.lblDataSeparator);
            this.gbFormate.Controls.Add(this.txtFileName);
            this.gbFormate.Controls.Add(this.lblFormatName);
            this.gbFormate.Enabled = false;
            this.gbFormate.Location = new System.Drawing.Point(338, 58);
            this.gbFormate.Name = "gbFormate";
            this.gbFormate.Size = new System.Drawing.Size(322, 372);
            this.gbFormate.TabIndex = 1;
            this.gbFormate.TabStop = false;
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
            this.cmbDataSeparator.Location = new System.Drawing.Point(118, 58);
            this.cmbDataSeparator.Name = "cmbDataSeparator";
            this.cmbDataSeparator.Size = new System.Drawing.Size(75, 21);
            this.cmbDataSeparator.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(153, 339);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 27);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(234, 339);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 27);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBoxSelectParameters
            // 
            this.groupBoxSelectParameters.Controls.Add(this.btnLoadSurveyDataParam);
            this.groupBoxSelectParameters.Controls.Add(this.btnTamperDataParam);
            this.groupBoxSelectParameters.Controls.Add(this.btnBillingDataParam);
            this.groupBoxSelectParameters.Controls.Add(this.btnInstantDataParam);
            this.groupBoxSelectParameters.Controls.Add(this.btnGeneralDataParam);
            this.groupBoxSelectParameters.Controls.Add(this.chkBoxLoadSurveyData);
            this.groupBoxSelectParameters.Controls.Add(this.chkBoxGeneralData);
            this.groupBoxSelectParameters.Controls.Add(this.chkBoxBillingData);
            this.groupBoxSelectParameters.Controls.Add(this.chkBoxTamperData);
            this.groupBoxSelectParameters.Controls.Add(this.chkBoxInstantData);
            this.groupBoxSelectParameters.Location = new System.Drawing.Point(47, 93);
            this.groupBoxSelectParameters.Name = "groupBoxSelectParameters";
            this.groupBoxSelectParameters.Size = new System.Drawing.Size(262, 230);
            this.groupBoxSelectParameters.TabIndex = 3;
            this.groupBoxSelectParameters.TabStop = false;
            this.groupBoxSelectParameters.Text = "Select parameters for exporting";
            // 
            // btnLoadSurveyDataParam
            // 
            this.btnLoadSurveyDataParam.Enabled = false;
            this.btnLoadSurveyDataParam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadSurveyDataParam.Location = new System.Drawing.Point(178, 193);
            this.btnLoadSurveyDataParam.Name = "btnLoadSurveyDataParam";
            this.btnLoadSurveyDataParam.Size = new System.Drawing.Size(36, 25);
            this.btnLoadSurveyDataParam.TabIndex = 9;
            this.btnLoadSurveyDataParam.Text = "...";
            this.btnLoadSurveyDataParam.UseVisualStyleBackColor = true;
            this.btnLoadSurveyDataParam.Click += new System.EventHandler(this.btnLoadSurveyDataParam_Click);
            // 
            // btnTamperDataParam
            // 
            this.btnTamperDataParam.Enabled = false;
            this.btnTamperDataParam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTamperDataParam.Location = new System.Drawing.Point(178, 155);
            this.btnTamperDataParam.Name = "btnTamperDataParam";
            this.btnTamperDataParam.Size = new System.Drawing.Size(36, 25);
            this.btnTamperDataParam.TabIndex = 7;
            this.btnTamperDataParam.Text = "...";
            this.btnTamperDataParam.UseVisualStyleBackColor = true;
            this.btnTamperDataParam.Click += new System.EventHandler(this.btnTamperDataParam_Click);
            // 
            // btnBillingDataParam
            // 
            this.btnBillingDataParam.Enabled = false;
            this.btnBillingDataParam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBillingDataParam.Location = new System.Drawing.Point(178, 79);
            this.btnBillingDataParam.Name = "btnBillingDataParam";
            this.btnBillingDataParam.Size = new System.Drawing.Size(36, 25);
            this.btnBillingDataParam.TabIndex = 3;
            this.btnBillingDataParam.Text = "...";
            this.btnBillingDataParam.UseVisualStyleBackColor = true;
            this.btnBillingDataParam.Click += new System.EventHandler(this.btnBillingDataParam_Click);
            // 
            // btnInstantDataParam
            // 
            this.btnInstantDataParam.Enabled = false;
            this.btnInstantDataParam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstantDataParam.Location = new System.Drawing.Point(178, 117);
            this.btnInstantDataParam.Name = "btnInstantDataParam";
            this.btnInstantDataParam.Size = new System.Drawing.Size(36, 25);
            this.btnInstantDataParam.TabIndex = 5;
            this.btnInstantDataParam.Text = "...";
            this.btnInstantDataParam.UseVisualStyleBackColor = true;
            this.btnInstantDataParam.Click += new System.EventHandler(this.btnInstantDataParam_Click);
            // 
            // btnGeneralDataParam
            // 
            this.btnGeneralDataParam.Enabled = false;
            this.btnGeneralDataParam.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGeneralDataParam.Location = new System.Drawing.Point(178, 41);
            this.btnGeneralDataParam.Name = "btnGeneralDataParam";
            this.btnGeneralDataParam.Size = new System.Drawing.Size(36, 25);
            this.btnGeneralDataParam.TabIndex = 1;
            this.btnGeneralDataParam.Text = "...";
            this.btnGeneralDataParam.UseVisualStyleBackColor = true;
            this.btnGeneralDataParam.Click += new System.EventHandler(this.btnGeneralDataParam_Click_1);
            // 
            // chkBoxLoadSurveyData
            // 
            this.chkBoxLoadSurveyData.AutoSize = true;
            this.chkBoxLoadSurveyData.Location = new System.Drawing.Point(27, 193);
            this.chkBoxLoadSurveyData.Name = "chkBoxLoadSurveyData";
            this.chkBoxLoadSurveyData.Size = new System.Drawing.Size(137, 17);
            this.chkBoxLoadSurveyData.TabIndex = 8;
            this.chkBoxLoadSurveyData.Text = "Load Survey Parameter";
            this.chkBoxLoadSurveyData.UseVisualStyleBackColor = true;
            this.chkBoxLoadSurveyData.CheckedChanged += new System.EventHandler(this.chkBoxLoadSurveyData_CheckedChanged);
            // 
            // chkBoxGeneralData
            // 
            this.chkBoxGeneralData.AutoSize = true;
            this.chkBoxGeneralData.Location = new System.Drawing.Point(27, 41);
            this.chkBoxGeneralData.Name = "chkBoxGeneralData";
            this.chkBoxGeneralData.Size = new System.Drawing.Size(114, 17);
            this.chkBoxGeneralData.TabIndex = 0;
            this.chkBoxGeneralData.Text = "General Parameter";
            this.chkBoxGeneralData.UseVisualStyleBackColor = true;
            this.chkBoxGeneralData.CheckedChanged += new System.EventHandler(this.chkBoxGeneralData_CheckedChanged);
            // 
            // chkBoxBillingData
            // 
            this.chkBoxBillingData.AutoSize = true;
            this.chkBoxBillingData.Location = new System.Drawing.Point(27, 79);
            this.chkBoxBillingData.Name = "chkBoxBillingData";
            this.chkBoxBillingData.Size = new System.Drawing.Size(104, 17);
            this.chkBoxBillingData.TabIndex = 2;
            this.chkBoxBillingData.Text = "Billing Parameter";
            this.chkBoxBillingData.UseVisualStyleBackColor = true;
            this.chkBoxBillingData.CheckedChanged += new System.EventHandler(this.chkBoxBillingData_CheckedChanged);
            // 
            // chkBoxTamperData
            // 
            this.chkBoxTamperData.AutoSize = true;
            this.chkBoxTamperData.Location = new System.Drawing.Point(27, 155);
            this.chkBoxTamperData.Name = "chkBoxTamperData";
            this.chkBoxTamperData.Size = new System.Drawing.Size(113, 17);
            this.chkBoxTamperData.TabIndex = 6;
            this.chkBoxTamperData.Text = "Tamper Parameter";
            this.chkBoxTamperData.UseVisualStyleBackColor = true;
            this.chkBoxTamperData.CheckedChanged += new System.EventHandler(this.chkBoxTamperData_CheckedChanged);
            // 
            // chkBoxInstantData
            // 
            this.chkBoxInstantData.AutoSize = true;
            this.chkBoxInstantData.Location = new System.Drawing.Point(27, 117);
            this.chkBoxInstantData.Name = "chkBoxInstantData";
            this.chkBoxInstantData.Size = new System.Drawing.Size(109, 17);
            this.chkBoxInstantData.TabIndex = 4;
            this.chkBoxInstantData.Text = "Instant Parameter";
            this.chkBoxInstantData.UseVisualStyleBackColor = true;
            this.chkBoxInstantData.CheckedChanged += new System.EventHandler(this.chkBoxInstantData_CheckedChanged);
            // 
            // lblDataSeparator
            // 
            this.lblDataSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.lblDataSeparator.Location = new System.Drawing.Point(45, 60);
            this.lblDataSeparator.Name = "lblDataSeparator";
            this.lblDataSeparator.Size = new System.Drawing.Size(53, 15);
            this.lblDataSeparator.TabIndex = 2;
            this.lblDataSeparator.Text = "Delimeter";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(118, 33);
            this.txtFileName.MaxLength = 20;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(184, 20);
            this.txtFileName.TabIndex = 0;
            // 
            // lblFormatName
            // 
            this.lblFormatName.AutoSize = true;
            this.lblFormatName.Location = new System.Drawing.Point(44, 36);
            this.lblFormatName.Name = "lblFormatName";
            this.lblFormatName.Size = new System.Drawing.Size(68, 13);
            this.lblFormatName.TabIndex = 0;
            this.lblFormatName.Text = "Format name";
            // 
            // lstFile
            // 
            this.lstFile.FormattingEnabled = true;
            this.lstFile.Location = new System.Drawing.Point(10, 64);
            this.lstFile.Margin = new System.Windows.Forms.Padding(2);
            this.lstFile.Name = "lstFile";
            this.lstFile.Size = new System.Drawing.Size(325, 368);
            this.lstFile.TabIndex = 2;
            this.lstFile.SelectedIndexChanged += new System.EventHandler(this.lstFile_SelectedIndexChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(10, 19);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 32);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(104, 19);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 32);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(197, 19);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 32);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(288, 19);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 32);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ASCIIExportSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(685, 479);
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
            this.groupBoxSelectParameters.ResumeLayout(false);
            this.groupBoxSelectParameters.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFormate;
        private System.Windows.Forms.ComboBox cmbDataSeparator;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBoxSelectParameters;
        private System.Windows.Forms.Button btnLoadSurveyDataParam;
        private System.Windows.Forms.Button btnTamperDataParam;
        private System.Windows.Forms.Button btnBillingDataParam;
        private System.Windows.Forms.Button btnInstantDataParam;
        private System.Windows.Forms.Button btnGeneralDataParam;
        private System.Windows.Forms.CheckBox chkBoxLoadSurveyData;
        private System.Windows.Forms.CheckBox chkBoxGeneralData;
        private System.Windows.Forms.CheckBox chkBoxBillingData;
        private System.Windows.Forms.CheckBox chkBoxTamperData;
        private System.Windows.Forms.CheckBox chkBoxInstantData;
        private System.Windows.Forms.Label lblDataSeparator;
        public System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label lblFormatName;
        private System.Windows.Forms.ListBox lstFile;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
    }
}
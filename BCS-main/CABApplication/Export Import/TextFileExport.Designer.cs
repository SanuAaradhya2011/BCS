namespace CAB.UI
{
    partial class TextFileExport
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
            this.grdFileList = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblErrorInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BSESFormat = new System.Windows.Forms.RadioButton();
            this.wbFormat = new System.Windows.Forms.RadioButton();
            this.lngFormat = new System.Windows.Forms.RadioButton();
            this.cmbExportType = new System.Windows.Forms.ComboBox();
            this.chkWithSeperator = new System.Windows.Forms.CheckBox();
            this.lblFilterEndDate = new System.Windows.Forms.Label();
            this.lblFilterStartDate = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.btnFilter = new System.Windows.Forms.Button();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.chkFilter = new System.Windows.Forms.CheckBox();
            this.rmcb = new System.Windows.Forms.ComboBox();
            this.cmbRelianceMumbaiDataType = new System.Windows.Forms.ComboBox();
            this.cmbBillHistNo = new System.Windows.Forms.ComboBox();
            this.lblBillHist = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdFileList)).BeginInit();
            this.panel1.SuspendLayout();
            this.grpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdFileList
            // 
            this.grdFileList.AllowUserToAddRows = false;
            this.grdFileList.AllowUserToDeleteRows = false;
            this.grdFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFileList.Location = new System.Drawing.Point(11, 147);
            this.grdFileList.Margin = new System.Windows.Forms.Padding(2);
            this.grdFileList.MultiSelect = false;
            this.grdFileList.Name = "grdFileList";
            this.grdFileList.RowTemplate.Height = 24;
            this.grdFileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdFileList.Size = new System.Drawing.Size(647, 289);
            this.grdFileList.TabIndex = 24;
            this.grdFileList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFileList_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 115);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "File List";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(15, 447);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 33;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(581, 441);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 32);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(495, 441);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 32);
            this.btnSave.TabIndex = 31;
            this.btnSave.Text = "Export";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblErrorInfo
            // 
            this.lblErrorInfo.AutoSize = true;
            this.lblErrorInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorInfo.ForeColor = System.Drawing.Color.Red;
            this.lblErrorInfo.Location = new System.Drawing.Point(265, 205);
            this.lblErrorInfo.Name = "lblErrorInfo";
            this.lblErrorInfo.Size = new System.Drawing.Size(138, 17);
            this.lblErrorInfo.TabIndex = 34;
            this.lblErrorInfo.Text = "No Files Available";
            this.lblErrorInfo.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "File Format :";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BSESFormat);
            this.panel1.Controls.Add(this.wbFormat);
            this.panel1.Controls.Add(this.lngFormat);
            this.panel1.Location = new System.Drawing.Point(106, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(552, 37);
            this.panel1.TabIndex = 36;
            // 
            // BSESFormat
            // 
            this.BSESFormat.AutoSize = true;
            this.BSESFormat.Location = new System.Drawing.Point(143, 9);
            this.BSESFormat.Name = "BSESFormat";
            this.BSESFormat.Size = new System.Drawing.Size(88, 17);
            this.BSESFormat.TabIndex = 3;
            this.BSESFormat.Text = "BSES Format";
            this.BSESFormat.UseVisualStyleBackColor = true;
            this.BSESFormat.Visible = false;
            // 
            // wbFormat
            // 
            this.wbFormat.AutoSize = true;
            this.wbFormat.Location = new System.Drawing.Point(89, 11);
            this.wbFormat.Name = "wbFormat";
            this.wbFormat.Size = new System.Drawing.Size(78, 17);
            this.wbFormat.TabIndex = 1;
            this.wbFormat.Text = "WB Format";
            this.wbFormat.UseVisualStyleBackColor = true;
            this.wbFormat.Visible = false;
            // 
            // lngFormat
            // 
            this.lngFormat.AutoSize = true;
            this.lngFormat.Checked = true;
            this.lngFormat.Location = new System.Drawing.Point(12, 10);
            this.lngFormat.Name = "lngFormat";
            this.lngFormat.Size = new System.Drawing.Size(82, 17);
            this.lngFormat.TabIndex = 0;
            this.lngFormat.TabStop = true;
            this.lngFormat.Text = "CAB Format";
            this.lngFormat.UseVisualStyleBackColor = true;
            this.lngFormat.Visible = false;
            // 
            // cmbExportType
            // 
            this.cmbExportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExportType.FormattingEnabled = true;
            this.cmbExportType.Items.AddRange(new object[] {
            "CAB Default Format",
            "WB Format",
            "BSES Format",
            "Puducherry Format",
            "Reliance Mumbai Format",
            "Torrent Ahmedabad Format",
            "Torrent Agra Format",
            "Tata Power Adani Format",
            "CSPDCL Format"});
            this.cmbExportType.Location = new System.Drawing.Point(106, 46);
            this.cmbExportType.Name = "cmbExportType";
            this.cmbExportType.Size = new System.Drawing.Size(167, 21);
            this.cmbExportType.TabIndex = 4;
            this.cmbExportType.SelectedIndexChanged += new System.EventHandler(this.cmbExportType_SelectedIndexChanged);
            // 
            // chkWithSeperator
            // 
            this.chkWithSeperator.AutoSize = true;
            this.chkWithSeperator.Location = new System.Drawing.Point(559, 48);
            this.chkWithSeperator.Name = "chkWithSeperator";
            this.chkWithSeperator.Size = new System.Drawing.Size(97, 17);
            this.chkWithSeperator.TabIndex = 2;
            this.chkWithSeperator.Text = "With Separator";
            this.chkWithSeperator.UseVisualStyleBackColor = true;
            // 
            // lblFilterEndDate
            // 
            this.lblFilterEndDate.AutoSize = true;
            this.lblFilterEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilterEndDate.Location = new System.Drawing.Point(230, 15);
            this.lblFilterEndDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFilterEndDate.Name = "lblFilterEndDate";
            this.lblFilterEndDate.Size = new System.Drawing.Size(20, 13);
            this.lblFilterEndDate.TabIndex = 44;
            this.lblFilterEndDate.Text = "To";
            // 
            // lblFilterStartDate
            // 
            this.lblFilterStartDate.AutoSize = true;
            this.lblFilterStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilterStartDate.Location = new System.Drawing.Point(8, 15);
            this.lblFilterStartDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFilterStartDate.Name = "lblFilterStartDate";
            this.lblFilterStartDate.Size = new System.Drawing.Size(30, 13);
            this.lblFilterStartDate.TabIndex = 43;
            this.lblFilterStartDate.Text = "From";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "dd/MM/yyyy";
            this.dtpEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(257, 11);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(150, 20);
            this.dtpEndDate.TabIndex = 42;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "dd/MM/yyyy";
            this.dtpStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(47, 11);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(150, 20);
            this.dtpStartDate.TabIndex = 41;
            // 
            // btnFilter
            // 
            this.btnFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFilter.Location = new System.Drawing.Point(425, 8);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(46, 23);
            this.btnFilter.TabIndex = 45;
            this.btnFilter.Text = "&Apply";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnFilter.ForeColor = System.Drawing.Color.White;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // grpFilter
            // 
            this.grpFilter.Controls.Add(this.dtpStartDate);
            this.grpFilter.Controls.Add(this.btnFilter);
            this.grpFilter.Controls.Add(this.lblFilterStartDate);
            this.grpFilter.Controls.Add(this.lblFilterEndDate);
            this.grpFilter.Controls.Add(this.dtpEndDate);
            this.grpFilter.Enabled = false;
            this.grpFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpFilter.Location = new System.Drawing.Point(106, 101);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(483, 36);
            this.grpFilter.TabIndex = 46;
            this.grpFilter.TabStop = false;
            // 
            // chkFilter
            // 
            this.chkFilter.AutoSize = true;
            this.chkFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFilter.Location = new System.Drawing.Point(11, 78);
            this.chkFilter.Name = "chkFilter";
            this.chkFilter.Size = new System.Drawing.Size(80, 17);
            this.chkFilter.TabIndex = 46;
            this.chkFilter.Text = "Use Filter";
            this.chkFilter.UseVisualStyleBackColor = true;
            this.chkFilter.CheckedChanged += new System.EventHandler(this.chkFilter_CheckedChanged);
            // 
            // rmcb
            // 
            this.rmcb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rmcb.FormattingEnabled = true;
            this.rmcb.Items.AddRange(new object[] {
            "Export Without Solar Format",
            "Export With Solar Format 1",
            "Export With Solar Format 2"});
            this.rmcb.Location = new System.Drawing.Point(394, 46);
            this.rmcb.Name = "rmcb";
            this.rmcb.Size = new System.Drawing.Size(160, 21);
            this.rmcb.TabIndex = 47;
            this.rmcb.Visible = false;
            this.rmcb.SelectedIndexChanged += new System.EventHandler(this.rmcb_SelectedIndexChanged);
            // 
            // cmbRelianceMumbaiDataType
            // 
            this.cmbRelianceMumbaiDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRelianceMumbaiDataType.FormattingEnabled = true;
            this.cmbRelianceMumbaiDataType.Items.AddRange(new object[] {
            "Billing",
            "LoadSurvey",
            "Instant",
            "Event"});
            this.cmbRelianceMumbaiDataType.Location = new System.Drawing.Point(279, 46);
            this.cmbRelianceMumbaiDataType.Name = "cmbRelianceMumbaiDataType";
            this.cmbRelianceMumbaiDataType.Size = new System.Drawing.Size(109, 21);
            this.cmbRelianceMumbaiDataType.TabIndex = 48;
            this.cmbRelianceMumbaiDataType.Visible = false;
            this.cmbRelianceMumbaiDataType.SelectedIndexChanged += new System.EventHandler(this.cmbRelianceMumbaiDataType_SelectedIndexChanged);
            // 
            // cmbBillHistNo
            // 
            this.cmbBillHistNo.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3"});
            this.cmbBillHistNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBillHistNo.FormattingEnabled = true;
            this.cmbBillHistNo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cmbBillHistNo.Location = new System.Drawing.Point(488, 73);
            this.cmbBillHistNo.Name = "cmbBillHistNo";
            this.cmbBillHistNo.Size = new System.Drawing.Size(66, 21);
            this.cmbBillHistNo.TabIndex = 47;
            this.cmbBillHistNo.Visible = false;
            // 
            // lblBillHist
            // 
            this.lblBillHist.AutoSize = true;
            this.lblBillHist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillHist.Location = new System.Drawing.Point(393, 76);
            this.lblBillHist.Name = "lblBillHist";
            this.lblBillHist.Size = new System.Drawing.Size(96, 13);
            this.lblBillHist.TabIndex = 35;
            this.lblBillHist.Text = "Billing History - ";
            // 
            // TextFileExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 487);
            this.Controls.Add(this.cmbRelianceMumbaiDataType);
            this.Controls.Add(this.cmbBillHistNo);
            this.Controls.Add(this.rmcb);
            this.Controls.Add(this.cmbExportType);
            this.Controls.Add(this.chkFilter);
            this.Controls.Add(this.chkWithSeperator);
            this.Controls.Add(this.grpFilter);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblBillHist);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblErrorInfo);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grdFileList);
            this.Name = "TextFileExport";
            this.StatusMessage = "";
            this.Text = "ASCII Export";
            this.Load += new System.EventHandler(this.TextFileExport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdFileList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grpFilter.ResumeLayout(false);
            this.grpFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdFileList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblErrorInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton wbFormat;
        private System.Windows.Forms.RadioButton lngFormat;
        private System.Windows.Forms.CheckBox chkWithSeperator;
        private System.Windows.Forms.RadioButton BSESFormat;
        private System.Windows.Forms.ComboBox cmbExportType;
        private System.Windows.Forms.Label lblFilterEndDate;
        private System.Windows.Forms.Label lblFilterStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.CheckBox chkFilter;
        private System.Windows.Forms.ComboBox rmcb;
        private System.Windows.Forms.ComboBox cmbRelianceMumbaiDataType;
        private System.Windows.Forms.ComboBox cmbBillHistNo;
        private System.Windows.Forms.Label lblBillHist;
    }
}


namespace CABApplication.Reports.Forms
{
    partial class TamperReport
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
            this.groupBoxTamperOccurence = new System.Windows.Forms.GroupBox();
            this.lngBtnView = new CAB.UI.Controls.CABButton();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.dtPickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtPickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.btnShowReport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbCheckBox = new System.Windows.Forms.GroupBox();
            this.lblNoData = new System.Windows.Forms.Label();
            this.dgvTamperOccurence = new System.Windows.Forms.DataGridView();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbComp5 = new System.Windows.Forms.RadioButton();
            this.rdbComp3 = new System.Windows.Forms.RadioButton();
            this.rdbComp2 = new System.Windows.Forms.RadioButton();
            this.rdbComp1 = new System.Windows.Forms.RadioButton();
            this.rdbAllTamper = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbSorting = new System.Windows.Forms.ComboBox();
            this.groupBoxTamperOccurence.SuspendLayout();
            this.gbCheckBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTamperOccurence)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTamperOccurence
            // 
            this.groupBoxTamperOccurence.Controls.Add(this.lngBtnView);
            this.groupBoxTamperOccurence.Controls.Add(this.lblDateFrom);
            this.groupBoxTamperOccurence.Controls.Add(this.dtPickerEndDate);
            this.groupBoxTamperOccurence.Controls.Add(this.dtPickerStartDate);
            this.groupBoxTamperOccurence.Controls.Add(this.lblDateTo);
            this.groupBoxTamperOccurence.Location = new System.Drawing.Point(12, 12);
            this.groupBoxTamperOccurence.Name = "groupBoxTamperOccurence";
            this.groupBoxTamperOccurence.Size = new System.Drawing.Size(402, 54);
            this.groupBoxTamperOccurence.TabIndex = 1;
            this.groupBoxTamperOccurence.TabStop = false;
            this.groupBoxTamperOccurence.Text = "Tamper Occurence Date";
            // 
            // lngBtnView
            // 
            this.lngBtnView.Location = new System.Drawing.Point(349, 6);
            this.lngBtnView.Name = "lngBtnView";
            this.lngBtnView.Size = new System.Drawing.Size(53, 13);
            this.lngBtnView.TabIndex = 9;
            this.lngBtnView.Text = "View";
            this.lngBtnView.TranslationKey = null;
            this.lngBtnView.UseVisualStyleBackColor = false;
            this.lngBtnView.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngBtnView.ForeColor = System.Drawing.Color.White;
            this.lngBtnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngBtnView.FlatAppearance.BorderSize = 0;
            this.lngBtnView.Visible = false;
            // 
            // lblDateFrom
            // 
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Location = new System.Drawing.Point(8, 24);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(55, 13);
            this.lblDateFrom.TabIndex = 7;
            this.lblDateFrom.Text = "Start Date";
            // 
            // dtPickerEndDate
            // 
            this.dtPickerEndDate.CustomFormat = "dd/MM/yyyy";
            this.dtPickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerEndDate.Location = new System.Drawing.Point(280, 22);
            this.dtPickerEndDate.Name = "dtPickerEndDate";
            this.dtPickerEndDate.Size = new System.Drawing.Size(98, 20);
            this.dtPickerEndDate.TabIndex = 2;
            this.dtPickerEndDate.DropDown += new System.EventHandler(this.dtPickerEndDate_DropDown);
            this.dtPickerEndDate.CloseUp += new System.EventHandler(this.dtPickerEndDate_CloseUp);
            // 
            // dtPickerStartDate
            // 
            this.dtPickerStartDate.CustomFormat = "dd/MM/yyyy";
            this.dtPickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPickerStartDate.Location = new System.Drawing.Point(69, 22);
            this.dtPickerStartDate.Name = "dtPickerStartDate";
            this.dtPickerStartDate.Size = new System.Drawing.Size(98, 20);
            this.dtPickerStartDate.TabIndex = 1;
            this.dtPickerStartDate.DropDown += new System.EventHandler(this.dtPickerStartDate_DropDown);
            this.dtPickerStartDate.CloseUp += new System.EventHandler(this.dtPickerStartDate_CloseUp);
            // 
            // lblDateTo
            // 
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Location = new System.Drawing.Point(222, 25);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(52, 13);
            this.lblDateTo.TabIndex = 8;
            this.lblDateTo.Text = "End Date";
            // 
            // btnShowReport
            // 
            this.btnShowReport.Location = new System.Drawing.Point(258, 466);
            this.btnShowReport.Name = "btnShowReport";
            this.btnShowReport.Size = new System.Drawing.Size(75, 23);
            this.btnShowReport.TabIndex = 19;
            this.btnShowReport.Text = "Show";
            this.btnShowReport.UseVisualStyleBackColor = false;
            this.btnShowReport.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShowReport.ForeColor = System.Drawing.Color.White;
            this.btnShowReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowReport.FlatAppearance.BorderSize = 0;
            this.btnShowReport.Click += new System.EventHandler(this.btnShowReport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(339, 466);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbCheckBox
            // 
            this.gbCheckBox.AutoScrollOffset = new System.Drawing.Point(28, 149);
            this.gbCheckBox.Controls.Add(this.lblNoData);
            this.gbCheckBox.Controls.Add(this.dgvTamperOccurence);
            this.gbCheckBox.Location = new System.Drawing.Point(9, 189);
            this.gbCheckBox.Name = "gbCheckBox";
            this.gbCheckBox.Size = new System.Drawing.Size(405, 271);
            this.gbCheckBox.TabIndex = 18;
            this.gbCheckBox.TabStop = false;
            this.gbCheckBox.Text = "Tamper Occurence";
            // 
            // lblNoData
            // 
            this.lblNoData.AutoSize = true;
            this.lblNoData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoData.ForeColor = System.Drawing.Color.Red;
            this.lblNoData.Location = new System.Drawing.Point(133, 127);
            this.lblNoData.Name = "lblNoData";
            this.lblNoData.Size = new System.Drawing.Size(138, 17);
            this.lblNoData.TabIndex = 1;
            this.lblNoData.Text = "No Data Available";
            this.lblNoData.Visible = false;
            // 
            // dgvTamperOccurence
            // 
            this.dgvTamperOccurence.AllowUserToAddRows = false;
            this.dgvTamperOccurence.AllowUserToDeleteRows = false;
            this.dgvTamperOccurence.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTamperOccurence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTamperOccurence.Location = new System.Drawing.Point(3, 16);
            this.dgvTamperOccurence.Name = "dgvTamperOccurence";
            this.dgvTamperOccurence.Size = new System.Drawing.Size(399, 252);
            this.dgvTamperOccurence.TabIndex = 0;
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(185, 470);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 21;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbComp5);
            this.groupBox1.Controls.Add(this.rdbComp3);
            this.groupBox1.Controls.Add(this.rdbComp2);
            this.groupBox1.Controls.Add(this.rdbComp1);
            this.groupBox1.Controls.Add(this.rdbAllTamper);
            this.groupBox1.Location = new System.Drawing.Point(12, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(404, 52);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // rdbComp5
            // 
            this.rdbComp5.AutoSize = true;
            this.rdbComp5.Location = new System.Drawing.Point(332, 18);
            this.rdbComp5.Name = "rdbComp5";
            this.rdbComp5.Size = new System.Drawing.Size(56, 17);
            this.rdbComp5.TabIndex = 32;
            this.rdbComp5.Text = "Others";
            this.rdbComp5.UseVisualStyleBackColor = true;
            this.rdbComp5.CheckedChanged += new System.EventHandler(this.rdbComp5_CheckedChanged);
            // 
            // rdbComp3
            // 
            this.rdbComp3.AutoSize = true;
            this.rdbComp3.Location = new System.Drawing.Point(253, 18);
            this.rdbComp3.Name = "rdbComp3";
            this.rdbComp3.Size = new System.Drawing.Size(55, 17);
            this.rdbComp3.TabIndex = 31;
            this.rdbComp3.Text = "Power";
            this.rdbComp3.UseVisualStyleBackColor = true;
            this.rdbComp3.CheckedChanged += new System.EventHandler(this.rdbComp3_CheckedChanged);
            // 
            // rdbComp2
            // 
            this.rdbComp2.AutoSize = true;
            this.rdbComp2.Location = new System.Drawing.Point(170, 19);
            this.rdbComp2.Name = "rdbComp2";
            this.rdbComp2.Size = new System.Drawing.Size(59, 17);
            this.rdbComp2.TabIndex = 30;
            this.rdbComp2.Text = "Current";
            this.rdbComp2.UseVisualStyleBackColor = true;
            this.rdbComp2.CheckedChanged += new System.EventHandler(this.rdbComp2_CheckedChanged);
            // 
            // rdbComp1
            // 
            this.rdbComp1.AutoSize = true;
            this.rdbComp1.Location = new System.Drawing.Point(85, 18);
            this.rdbComp1.Name = "rdbComp1";
            this.rdbComp1.Size = new System.Drawing.Size(61, 17);
            this.rdbComp1.TabIndex = 29;
            this.rdbComp1.Text = "Voltage";
            this.rdbComp1.UseVisualStyleBackColor = true;
            this.rdbComp1.CheckedChanged += new System.EventHandler(this.rdbComp1_CheckedChanged);
            // 
            // rdbAllTamper
            // 
            this.rdbAllTamper.AutoSize = true;
            this.rdbAllTamper.Checked = true;
            this.rdbAllTamper.Location = new System.Drawing.Point(25, 18);
            this.rdbAllTamper.Name = "rdbAllTamper";
            this.rdbAllTamper.Size = new System.Drawing.Size(36, 17);
            this.rdbAllTamper.TabIndex = 28;
            this.rdbAllTamper.TabStop = true;
            this.rdbAllTamper.Text = "All";
            this.rdbAllTamper.UseVisualStyleBackColor = true;
            this.rdbAllTamper.CheckedChanged += new System.EventHandler(this.rdbAllTamper_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbSorting);
            this.groupBox2.Location = new System.Drawing.Point(12, 130);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(404, 52);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sorting";
            // 
            // cmbSorting
            // 
            this.cmbSorting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSorting.FormattingEnabled = true;
            this.cmbSorting.Location = new System.Drawing.Point(11, 20);
            this.cmbSorting.Name = "cmbSorting";
            this.cmbSorting.Size = new System.Drawing.Size(387, 21);
            this.cmbSorting.TabIndex = 30;
            // 
            // TamperReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(428, 501);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkSelectAll);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnShowReport);
            this.Controls.Add(this.gbCheckBox);
            this.Controls.Add(this.groupBoxTamperOccurence);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TamperReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tamper Report";
            this.groupBoxTamperOccurence.ResumeLayout(false);
            this.groupBoxTamperOccurence.PerformLayout();
            this.gbCheckBox.ResumeLayout(false);
            this.gbCheckBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTamperOccurence)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTamperOccurence;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.DateTimePicker dtPickerEndDate;
        private System.Windows.Forms.DateTimePicker dtPickerStartDate;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.Button btnShowReport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbCheckBox;
        private System.Windows.Forms.DataGridView dgvTamperOccurence;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private CAB.UI.Controls.CABButton lngBtnView;
        private System.Windows.Forms.Label lblNoData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbComp5;
        private System.Windows.Forms.RadioButton rdbComp3;
        private System.Windows.Forms.RadioButton rdbComp2;
        private System.Windows.Forms.RadioButton rdbComp1;
        private System.Windows.Forms.RadioButton rdbAllTamper;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbSorting;
    }
}



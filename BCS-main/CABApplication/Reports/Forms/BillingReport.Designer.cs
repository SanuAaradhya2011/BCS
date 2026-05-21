namespace CABApplication.Reports.Forms
{
    partial class BillingReport
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
            this.chkBillAveragePF = new System.Windows.Forms.CheckBox();
            this.chkMDkW = new System.Windows.Forms.CheckBox();
            this.chkMDkWDateTime = new System.Windows.Forms.CheckBox();
            this.chkCumkvarhLag = new System.Windows.Forms.CheckBox();
            this.chkCumkvarhLead = new System.Windows.Forms.CheckBox();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbCheckBox = new System.Windows.Forms.GroupBox();
            this.chkMDkVADateTime = new System.Windows.Forms.CheckBox();
            this.chkMDKVA = new System.Windows.Forms.CheckBox();
            this.chkCumkVAh = new System.Windows.Forms.CheckBox();
            this.chkH1CumkWh = new System.Windows.Forms.CheckBox();
            this.chkBillingDate = new System.Windows.Forms.CheckBox();
            this.chkMeterNo = new System.Windows.Forms.CheckBox();
            this.chkConsumerName = new System.Windows.Forms.CheckBox();
            this.chkConsumerId = new System.Windows.Forms.CheckBox();
            this.rdbAllMeters = new System.Windows.Forms.RadioButton();
            this.rdbGroup = new System.Windows.Forms.RadioButton();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.grpSelect = new System.Windows.Forms.GroupBox();
            this.grpHistory = new System.Windows.Forms.GroupBox();
            this.cmbHistory = new System.Windows.Forms.ComboBox();
            this.lblHistory = new System.Windows.Forms.Label();
            this.gbCheckBox.SuspendLayout();
            this.grpSelect.SuspendLayout();
            this.grpHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkBillAveragePF
            // 
            this.chkBillAveragePF.AutoSize = true;
            this.chkBillAveragePF.Location = new System.Drawing.Point(166, 19);
            this.chkBillAveragePF.Name = "chkBillAveragePF";
            this.chkBillAveragePF.Size = new System.Drawing.Size(101, 17);
            this.chkBillAveragePF.TabIndex = 8;
            this.chkBillAveragePF.Text = "Bill Average P.F";
            this.chkBillAveragePF.UseVisualStyleBackColor = true;
            // 
            // chkMDkW
            // 
            this.chkMDkW.AutoSize = true;
            this.chkMDkW.Location = new System.Drawing.Point(166, 43);
            this.chkMDkW.Name = "chkMDkW";
            this.chkMDkW.Size = new System.Drawing.Size(63, 17);
            this.chkMDkW.TabIndex = 9;
            this.chkMDkW.Text = "MD kW";
            this.chkMDkW.UseVisualStyleBackColor = true;
            // 
            // chkMDkWDateTime
            // 
            this.chkMDkWDateTime.AutoSize = true;
            this.chkMDkWDateTime.Location = new System.Drawing.Point(166, 67);
            this.chkMDkWDateTime.Name = "chkMDkWDateTime";
            this.chkMDkWDateTime.Size = new System.Drawing.Size(112, 17);
            this.chkMDkWDateTime.TabIndex = 10;
            this.chkMDkWDateTime.Text = "MD kW DateTime";
            this.chkMDkWDateTime.UseVisualStyleBackColor = true;
            // 
            // chkCumkvarhLag
            // 
            this.chkCumkvarhLag.AutoSize = true;
            this.chkCumkvarhLag.Location = new System.Drawing.Point(166, 91);
            this.chkCumkvarhLag.Name = "chkCumkvarhLag";
            this.chkCumkvarhLag.Size = new System.Drawing.Size(104, 17);
            this.chkCumkvarhLag.TabIndex = 11;
            this.chkCumkvarhLag.Text = "Cum kvarh (Lag)";
            this.chkCumkvarhLag.UseVisualStyleBackColor = true;
            // 
            // chkCumkvarhLead
            // 
            this.chkCumkvarhLead.AutoSize = true;
            this.chkCumkvarhLead.Location = new System.Drawing.Point(166, 115);
            this.chkCumkvarhLead.Name = "chkCumkvarhLead";
            this.chkCumkvarhLead.Size = new System.Drawing.Size(110, 17);
            this.chkCumkvarhLead.TabIndex = 12;
            this.chkCumkvarhLead.Text = "Cum kvarh (Lead)";
            this.chkCumkvarhLead.UseVisualStyleBackColor = true;
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(173, 320);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 13;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShow.ForeColor = System.Drawing.Color.White;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.FlatAppearance.BorderSize = 0;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(255, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
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
            this.gbCheckBox.Controls.Add(this.chkMDkVADateTime);
            this.gbCheckBox.Controls.Add(this.chkMDKVA);
            this.gbCheckBox.Controls.Add(this.chkCumkVAh);
            this.gbCheckBox.Controls.Add(this.chkH1CumkWh);
            this.gbCheckBox.Controls.Add(this.chkBillingDate);
            this.gbCheckBox.Controls.Add(this.chkCumkvarhLead);
            this.gbCheckBox.Controls.Add(this.chkMeterNo);
            this.gbCheckBox.Controls.Add(this.chkCumkvarhLag);
            this.gbCheckBox.Controls.Add(this.chkConsumerName);
            this.gbCheckBox.Controls.Add(this.chkMDkWDateTime);
            this.gbCheckBox.Controls.Add(this.chkConsumerId);
            this.gbCheckBox.Controls.Add(this.chkMDkW);
            this.gbCheckBox.Controls.Add(this.chkBillAveragePF);
            this.gbCheckBox.Location = new System.Drawing.Point(7, 95);
            this.gbCheckBox.Name = "gbCheckBox";
            this.gbCheckBox.Size = new System.Drawing.Size(325, 215);
            this.gbCheckBox.TabIndex = 17;
            this.gbCheckBox.TabStop = false;
            // 
            // chkMDkVADateTime
            // 
            this.chkMDkVADateTime.AutoSize = true;
            this.chkMDkVADateTime.Checked = true;
            this.chkMDkVADateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMDkVADateTime.Enabled = false;
            this.chkMDkVADateTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkMDkVADateTime.Location = new System.Drawing.Point(20, 187);
            this.chkMDkVADateTime.Name = "chkMDkVADateTime";
            this.chkMDkVADateTime.Size = new System.Drawing.Size(115, 17);
            this.chkMDkVADateTime.TabIndex = 15;
            this.chkMDkVADateTime.Text = "MD kVA DateTime";
            this.chkMDkVADateTime.UseVisualStyleBackColor = true;
            // 
            // chkMDKVA
            // 
            this.chkMDKVA.AutoSize = true;
            this.chkMDKVA.Checked = true;
            this.chkMDKVA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMDKVA.Enabled = false;
            this.chkMDKVA.Location = new System.Drawing.Point(20, 163);
            this.chkMDKVA.Name = "chkMDKVA";
            this.chkMDKVA.Size = new System.Drawing.Size(70, 17);
            this.chkMDKVA.TabIndex = 14;
            this.chkMDKVA.Text = "MD  KVA";
            this.chkMDKVA.UseVisualStyleBackColor = true;
            // 
            // chkCumkVAh
            // 
            this.chkCumkVAh.AutoSize = true;
            this.chkCumkVAh.Checked = true;
            this.chkCumkVAh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCumkVAh.Enabled = false;
            this.chkCumkVAh.Location = new System.Drawing.Point(20, 139);
            this.chkCumkVAh.Name = "chkCumkVAh";
            this.chkCumkVAh.Size = new System.Drawing.Size(76, 17);
            this.chkCumkVAh.TabIndex = 13;
            this.chkCumkVAh.Text = "Cum kVAh";
            this.chkCumkVAh.UseVisualStyleBackColor = true;
            // 
            // chkH1CumkWh
            // 
            this.chkH1CumkWh.AutoSize = true;
            this.chkH1CumkWh.Checked = true;
            this.chkH1CumkWh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkH1CumkWh.Enabled = false;
            this.chkH1CumkWh.Location = new System.Drawing.Point(20, 115);
            this.chkH1CumkWh.Name = "chkH1CumkWh";
            this.chkH1CumkWh.Size = new System.Drawing.Size(73, 17);
            this.chkH1CumkWh.TabIndex = 12;
            this.chkH1CumkWh.Text = "Cum kWh";
            this.chkH1CumkWh.UseVisualStyleBackColor = true;
            // 
            // chkBillingDate
            // 
            this.chkBillingDate.AutoSize = true;
            this.chkBillingDate.Checked = true;
            this.chkBillingDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBillingDate.Enabled = false;
            this.chkBillingDate.Location = new System.Drawing.Point(20, 91);
            this.chkBillingDate.Name = "chkBillingDate";
            this.chkBillingDate.Size = new System.Drawing.Size(126, 17);
            this.chkBillingDate.TabIndex = 11;
            this.chkBillingDate.Text = "Billing Date and Time";
            this.chkBillingDate.UseVisualStyleBackColor = true;
            // 
            // chkMeterNo
            // 
            this.chkMeterNo.AutoSize = true;
            this.chkMeterNo.Checked = true;
            this.chkMeterNo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMeterNo.Enabled = false;
            this.chkMeterNo.Location = new System.Drawing.Point(20, 67);
            this.chkMeterNo.Name = "chkMeterNo";
            this.chkMeterNo.Size = new System.Drawing.Size(70, 17);
            this.chkMeterNo.TabIndex = 10;
            this.chkMeterNo.Text = "Meter No";
            this.chkMeterNo.UseVisualStyleBackColor = true;
            // 
            // chkConsumerName
            // 
            this.chkConsumerName.AutoSize = true;
            this.chkConsumerName.Checked = true;
            this.chkConsumerName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConsumerName.Enabled = false;
            this.chkConsumerName.Location = new System.Drawing.Point(20, 43);
            this.chkConsumerName.Name = "chkConsumerName";
            this.chkConsumerName.Size = new System.Drawing.Size(104, 17);
            this.chkConsumerName.TabIndex = 9;
            this.chkConsumerName.Text = "Consumer Name";
            this.chkConsumerName.UseVisualStyleBackColor = true;
            // 
            // chkConsumerId
            // 
            this.chkConsumerId.AutoSize = true;
            this.chkConsumerId.BackColor = System.Drawing.SystemColors.Control;
            this.chkConsumerId.Checked = true;
            this.chkConsumerId.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConsumerId.Enabled = false;
            this.chkConsumerId.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkConsumerId.Location = new System.Drawing.Point(20, 19);
            this.chkConsumerId.Name = "chkConsumerId";
            this.chkConsumerId.Size = new System.Drawing.Size(87, 17);
            this.chkConsumerId.TabIndex = 8;
            this.chkConsumerId.Text = "Consumer ID";
            this.chkConsumerId.UseVisualStyleBackColor = false;
            // 
            // rdbAllMeters
            // 
            this.rdbAllMeters.AutoSize = true;
            this.rdbAllMeters.Location = new System.Drawing.Point(6, 19);
            this.rdbAllMeters.Name = "rdbAllMeters";
            this.rdbAllMeters.Size = new System.Drawing.Size(71, 17);
            this.rdbAllMeters.TabIndex = 16;
            this.rdbAllMeters.TabStop = true;
            this.rdbAllMeters.Text = "All Meters";
            this.rdbAllMeters.UseVisualStyleBackColor = true;
            this.rdbAllMeters.CheckedChanged += new System.EventHandler(this.rdbAllMeters_CheckedChanged);
            // 
            // rdbGroup
            // 
            this.rdbGroup.AutoSize = true;
            this.rdbGroup.Location = new System.Drawing.Point(6, 42);
            this.rdbGroup.Name = "rdbGroup";
            this.rdbGroup.Size = new System.Drawing.Size(54, 17);
            this.rdbGroup.TabIndex = 15;
            this.rdbGroup.TabStop = true;
            this.rdbGroup.Text = "Group";
            this.rdbGroup.UseVisualStyleBackColor = true;
            this.rdbGroup.CheckedChanged += new System.EventHandler(this.rdbGroup_CheckedChanged);
            // 
            // cmbGroup
            // 
            this.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(73, 42);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(108, 21);
            this.cmbGroup.TabIndex = 18;
            // 
            // grpSelect
            // 
            this.grpSelect.Controls.Add(this.cmbGroup);
            this.grpSelect.Controls.Add(this.rdbGroup);
            this.grpSelect.Controls.Add(this.rdbAllMeters);
            this.grpSelect.Location = new System.Drawing.Point(8, 12);
            this.grpSelect.Name = "grpSelect";
            this.grpSelect.Size = new System.Drawing.Size(189, 79);
            this.grpSelect.TabIndex = 19;
            this.grpSelect.TabStop = false;
            this.grpSelect.Text = "Select";
            // 
            // grpHistory
            // 
            this.grpHistory.Controls.Add(this.cmbHistory);
            this.grpHistory.Controls.Add(this.lblHistory);
            this.grpHistory.Location = new System.Drawing.Point(205, 12);
            this.grpHistory.Name = "grpHistory";
            this.grpHistory.Size = new System.Drawing.Size(127, 79);
            this.grpHistory.TabIndex = 20;
            this.grpHistory.TabStop = false;
            this.grpHistory.Text = "Historywise";
            // 
            // cmbHistory
            // 
            this.cmbHistory.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHistory.FormattingEnabled = true;
            this.cmbHistory.Items.AddRange(new object[] {
            "0",
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
            this.cmbHistory.Location = new System.Drawing.Point(57, 16);
            this.cmbHistory.Name = "cmbHistory";
            this.cmbHistory.Size = new System.Drawing.Size(58, 21);
            this.cmbHistory.TabIndex = 1;
            // 
            // lblHistory
            // 
            this.lblHistory.AutoSize = true;
            this.lblHistory.Location = new System.Drawing.Point(7, 20);
            this.lblHistory.Name = "lblHistory";
            this.lblHistory.Size = new System.Drawing.Size(48, 13);
            this.lblHistory.TabIndex = 0;
            this.lblHistory.Text = "History : ";
            // 
            // BillingReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 354);
            this.Controls.Add(this.grpHistory);
            this.Controls.Add(this.grpSelect);
            this.Controls.Add(this.gbCheckBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "BillingReport";
            this.Text = "Billing Report";
            this.Load += new System.EventHandler(this.BillingReport_Load);
            this.gbCheckBox.ResumeLayout(false);
            this.gbCheckBox.PerformLayout();
            this.grpSelect.ResumeLayout(false);
            this.grpSelect.PerformLayout();
            this.grpHistory.ResumeLayout(false);
            this.grpHistory.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkBillAveragePF;
        private System.Windows.Forms.CheckBox chkMDkW;
        private System.Windows.Forms.CheckBox chkMDkWDateTime;
        private System.Windows.Forms.CheckBox chkCumkvarhLag;
        private System.Windows.Forms.CheckBox chkCumkvarhLead;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbCheckBox;
        private System.Windows.Forms.CheckBox chkMDkVADateTime;
        private System.Windows.Forms.CheckBox chkMDKVA;
        private System.Windows.Forms.CheckBox chkCumkVAh;
        private System.Windows.Forms.CheckBox chkH1CumkWh;
        private System.Windows.Forms.CheckBox chkBillingDate;
        private System.Windows.Forms.CheckBox chkMeterNo;
        private System.Windows.Forms.CheckBox chkConsumerName;
        private System.Windows.Forms.CheckBox chkConsumerId;
        private System.Windows.Forms.RadioButton rdbAllMeters;
        private System.Windows.Forms.RadioButton rdbGroup;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.GroupBox grpSelect;
        private System.Windows.Forms.GroupBox grpHistory;
        private System.Windows.Forms.ComboBox cmbHistory;
        private System.Windows.Forms.Label lblHistory;
    }
}


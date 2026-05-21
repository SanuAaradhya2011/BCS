namespace CAB.UI
{
    partial class SelectDialogTNEB
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblReports = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.SMD_btnCancel = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.chkProfileDemand = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.chkVIProfile = new System.Windows.Forms.CheckBox();
            this.chkProfileEnergy = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkPhasorReport = new System.Windows.Forms.CheckBox();
            this.chkInstantaneousData = new System.Windows.Forms.CheckBox();
            this.chkDailyMaximumData = new System.Windows.Forms.CheckBox();
            this.chkDailyEnergyConsumption = new System.Windows.Forms.CheckBox();
            this.chkCumulativeTamperEvents = new System.Windows.Forms.CheckBox();
            this.chkCumulativeEnergies = new System.Windows.Forms.CheckBox();
            this.SMD_SelectAll = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnDetailedCancel = new System.Windows.Forms.Button();
            this.btnDetailedShow = new System.Windows.Forms.Button();
            this.gbDetailedTamper = new System.Windows.Forms.GroupBox();
            this.chkBillingReport = new System.Windows.Forms.CheckBox();
            this.chkDetailedTamper = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.gbDetailedTamper.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-7, -22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 14);
            this.label1.TabIndex = 42;
            this.label1.Text = "Report Parameters : ";
            // 
            // lblReports
            // 
            this.lblReports.AutoSize = true;
            this.lblReports.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReports.Location = new System.Drawing.Point(12, 16);
            this.lblReports.Name = "lblReports";
            this.lblReports.Size = new System.Drawing.Size(109, 14);
            this.lblReports.TabIndex = 43;
            this.lblReports.Text = "Select Parameters";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(15, 33);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(433, 279);
            this.tabControl1.TabIndex = 52;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SMD_btnCancel);
            this.tabPage1.Controls.Add(this.btnShow);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.SMD_SelectAll);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(425, 253);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Detailed Report1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // SMD_btnCancel
            // 
            this.SMD_btnCancel.Location = new System.Drawing.Point(274, 197);
            this.SMD_btnCancel.Name = "SMD_btnCancel";
            this.SMD_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.SMD_btnCancel.TabIndex = 54;
            this.SMD_btnCancel.Text = "Cancel";
            this.SMD_btnCancel.UseVisualStyleBackColor = true;
            this.SMD_btnCancel.Click += new System.EventHandler(this.SMD_btnCancel_Click_1);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(191, 197);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 53;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click_1);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.fromDate);
            this.groupBox3.Controls.Add(this.chkProfileDemand);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.toDate);
            this.groupBox3.Controls.Add(this.chkVIProfile);
            this.groupBox3.Controls.Add(this.chkProfileEnergy);
            this.groupBox3.Location = new System.Drawing.Point(191, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(190, 185);
            this.groupBox3.TabIndex = 52;
            this.groupBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 53;
            this.label2.Text = "From";
            // 
            // fromDate
            // 
            this.fromDate.CustomFormat = "dd/MM/yyyy";
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromDate.Location = new System.Drawing.Point(71, 12);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(102, 20);
            this.fromDate.TabIndex = 50;
            // 
            // chkProfileDemand
            // 
            this.chkProfileDemand.AutoSize = true;
            this.chkProfileDemand.Location = new System.Drawing.Point(23, 107);
            this.chkProfileDemand.Name = "chkProfileDemand";
            this.chkProfileDemand.Size = new System.Drawing.Size(135, 17);
            this.chkProfileDemand.TabIndex = 48;
            this.chkProfileDemand.Text = "Load Survey (Demand)";
            this.chkProfileDemand.UseVisualStyleBackColor = true;
            this.chkProfileDemand.CheckedChanged += new System.EventHandler(this.chkProfileDemand_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 52;
            this.label3.Text = "To";
            // 
            // toDate
            // 
            this.toDate.CustomFormat = "dd-MMM-yyyy";
            this.toDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDate.Location = new System.Drawing.Point(71, 42);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(102, 20);
            this.toDate.TabIndex = 51;
            // 
            // chkVIProfile
            // 
            this.chkVIProfile.AutoSize = true;
            this.chkVIProfile.Location = new System.Drawing.Point(23, 84);
            this.chkVIProfile.Name = "chkVIProfile";
            this.chkVIProfile.Size = new System.Drawing.Size(68, 17);
            this.chkVIProfile.TabIndex = 47;
            this.chkVIProfile.Text = "VI Profile";
            this.chkVIProfile.UseVisualStyleBackColor = true;
            this.chkVIProfile.CheckedChanged += new System.EventHandler(this.chkVIProfile_CheckedChanged);
            // 
            // chkProfileEnergy
            // 
            this.chkProfileEnergy.AutoSize = true;
            this.chkProfileEnergy.Location = new System.Drawing.Point(23, 130);
            this.chkProfileEnergy.Name = "chkProfileEnergy";
            this.chkProfileEnergy.Size = new System.Drawing.Size(126, 17);
            this.chkProfileEnergy.TabIndex = 49;
            this.chkProfileEnergy.Text = "Load survey (Energy)";
            this.chkProfileEnergy.UseVisualStyleBackColor = true;
            this.chkProfileEnergy.CheckedChanged += new System.EventHandler(this.chkProfileEnergy_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkPhasorReport);
            this.groupBox2.Controls.Add(this.chkInstantaneousData);
            this.groupBox2.Controls.Add(this.chkDailyMaximumData);
            this.groupBox2.Controls.Add(this.chkDailyEnergyConsumption);
            this.groupBox2.Controls.Add(this.chkCumulativeTamperEvents);
            this.groupBox2.Controls.Add(this.chkCumulativeEnergies);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 185);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            // 
            // chkPhasorReport
            // 
            this.chkPhasorReport.AutoSize = true;
            this.chkPhasorReport.Location = new System.Drawing.Point(24, 157);
            this.chkPhasorReport.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.chkPhasorReport.Name = "chkPhasorReport";
            this.chkPhasorReport.Size = new System.Drawing.Size(85, 17);
            this.chkPhasorReport.TabIndex = 47;
            this.chkPhasorReport.Text = "Phasor Data";
            this.chkPhasorReport.UseVisualStyleBackColor = true;
            this.chkPhasorReport.CheckedChanged += new System.EventHandler(this.chkPhasorReport_CheckedChanged);
            // 
            // chkInstantaneousData
            // 
            this.chkInstantaneousData.AutoSize = true;
            this.chkInstantaneousData.Location = new System.Drawing.Point(24, 99);
            this.chkInstantaneousData.Name = "chkInstantaneousData";
            this.chkInstantaneousData.Size = new System.Drawing.Size(119, 17);
            this.chkInstantaneousData.TabIndex = 46;
            this.chkInstantaneousData.Text = "Instantaneous Data";
            this.chkInstantaneousData.UseVisualStyleBackColor = true;
            this.chkInstantaneousData.CheckedChanged += new System.EventHandler(this.chkInstantaneousData_CheckedChanged);
            // 
            // chkDailyMaximumData
            // 
            this.chkDailyMaximumData.AutoSize = true;
            this.chkDailyMaximumData.Location = new System.Drawing.Point(24, 73);
            this.chkDailyMaximumData.Name = "chkDailyMaximumData";
            this.chkDailyMaximumData.Size = new System.Drawing.Size(122, 17);
            this.chkDailyMaximumData.TabIndex = 42;
            this.chkDailyMaximumData.Text = "Daily Maximum Data";
            this.chkDailyMaximumData.UseVisualStyleBackColor = true;
            this.chkDailyMaximumData.CheckedChanged += new System.EventHandler(this.chkDailyMaximumData_CheckedChanged);
            // 
            // chkDailyEnergyConsumption
            // 
            this.chkDailyEnergyConsumption.AutoSize = true;
            this.chkDailyEnergyConsumption.Location = new System.Drawing.Point(24, 47);
            this.chkDailyEnergyConsumption.Name = "chkDailyEnergyConsumption";
            this.chkDailyEnergyConsumption.Size = new System.Drawing.Size(149, 17);
            this.chkDailyEnergyConsumption.TabIndex = 41;
            this.chkDailyEnergyConsumption.Text = "Daily Energy Consumption";
            this.chkDailyEnergyConsumption.UseVisualStyleBackColor = true;
            this.chkDailyEnergyConsumption.CheckedChanged += new System.EventHandler(this.chkDailyEnergyConsumption_CheckedChanged);
            // 
            // chkCumulativeTamperEvents
            // 
            this.chkCumulativeTamperEvents.AutoSize = true;
            this.chkCumulativeTamperEvents.Location = new System.Drawing.Point(24, 127);
            this.chkCumulativeTamperEvents.Name = "chkCumulativeTamperEvents";
            this.chkCumulativeTamperEvents.Size = new System.Drawing.Size(153, 17);
            this.chkCumulativeTamperEvents.TabIndex = 40;
            this.chkCumulativeTamperEvents.Text = "Cumulative Tamper Events";
            this.chkCumulativeTamperEvents.UseVisualStyleBackColor = true;
            this.chkCumulativeTamperEvents.CheckedChanged += new System.EventHandler(this.chkCumulativeTamperEvents_CheckedChanged);
            // 
            // chkCumulativeEnergies
            // 
            this.chkCumulativeEnergies.AutoSize = true;
            this.chkCumulativeEnergies.Location = new System.Drawing.Point(24, 19);
            this.chkCumulativeEnergies.Name = "chkCumulativeEnergies";
            this.chkCumulativeEnergies.Size = new System.Drawing.Size(122, 17);
            this.chkCumulativeEnergies.TabIndex = 39;
            this.chkCumulativeEnergies.Text = "Cumulative Energies";
            this.chkCumulativeEnergies.UseVisualStyleBackColor = true;
            this.chkCumulativeEnergies.CheckedChanged += new System.EventHandler(this.chkCumulativeEnergies_CheckedChanged);
            // 
            // SMD_SelectAll
            // 
            this.SMD_SelectAll.AutoSize = true;
            this.SMD_SelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SMD_SelectAll.Location = new System.Drawing.Point(30, 197);
            this.SMD_SelectAll.Name = "SMD_SelectAll";
            this.SMD_SelectAll.Size = new System.Drawing.Size(80, 17);
            this.SMD_SelectAll.TabIndex = 38;
            this.SMD_SelectAll.Text = "Select All";
            this.SMD_SelectAll.UseVisualStyleBackColor = true;
            this.SMD_SelectAll.CheckedChanged += new System.EventHandler(this.SMD_SelectAll_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnDetailedCancel);
            this.tabPage2.Controls.Add(this.btnDetailedShow);
            this.tabPage2.Controls.Add(this.gbDetailedTamper);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(425, 253);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Detailed Report2";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // btnDetailedCancel
            // 
            this.btnDetailedCancel.Location = new System.Drawing.Point(216, 120);
            this.btnDetailedCancel.Name = "btnDetailedCancel";
            this.btnDetailedCancel.Size = new System.Drawing.Size(75, 23);
            this.btnDetailedCancel.TabIndex = 3;
            this.btnDetailedCancel.Text = "Cancel";
            this.btnDetailedCancel.UseVisualStyleBackColor = true;
            this.btnDetailedCancel.Click += new System.EventHandler(this.btnDetailedCancel_Click);
            // 
            // btnDetailedShow
            // 
            this.btnDetailedShow.Location = new System.Drawing.Point(120, 120);
            this.btnDetailedShow.Name = "btnDetailedShow";
            this.btnDetailedShow.Size = new System.Drawing.Size(75, 23);
            this.btnDetailedShow.TabIndex = 2;
            this.btnDetailedShow.Text = "Show";
            this.btnDetailedShow.UseVisualStyleBackColor = true;
            this.btnDetailedShow.Click += new System.EventHandler(this.btnDetailedShow_Click);
            // 
            // gbDetailedTamper
            // 
            this.gbDetailedTamper.Controls.Add(this.chkBillingReport);
            this.gbDetailedTamper.Controls.Add(this.chkDetailedTamper);
            this.gbDetailedTamper.Location = new System.Drawing.Point(20, 16);
            this.gbDetailedTamper.Name = "gbDetailedTamper";
            this.gbDetailedTamper.Size = new System.Drawing.Size(274, 84);
            this.gbDetailedTamper.TabIndex = 1;
            this.gbDetailedTamper.TabStop = false;
            // 
            // chkBillingReport
            // 
            this.chkBillingReport.AutoSize = true;
            this.chkBillingReport.Location = new System.Drawing.Point(21, 53);
            this.chkBillingReport.Name = "chkBillingReport";
            this.chkBillingReport.Size = new System.Drawing.Size(99, 17);
            this.chkBillingReport.TabIndex = 1;
            this.chkBillingReport.Text = "Reset Backups";
            this.chkBillingReport.UseVisualStyleBackColor = true;
            // 
            // chkDetailedTamper
            // 
            this.chkDetailedTamper.AutoSize = true;
            this.chkDetailedTamper.Location = new System.Drawing.Point(21, 24);
            this.chkDetailedTamper.Name = "chkDetailedTamper";
            this.chkDetailedTamper.Size = new System.Drawing.Size(139, 17);
            this.chkDetailedTamper.TabIndex = 0;
            this.chkDetailedTamper.Text = "Detailed Tamper Report";
            this.chkDetailedTamper.UseVisualStyleBackColor = true;
            // 
            // SelectDialogTNEB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 356);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblReports);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SelectDialogTNEB";
            this.Text = "Detailed Report";
            this.Load += new System.EventHandler(this.SelectDialogTNEB_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectDialogTNEB_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.gbDetailedTamper.ResumeLayout(false);
            this.gbDetailedTamper.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblReports;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button SMD_btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.CheckBox chkProfileDemand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.CheckBox chkVIProfile;
        private System.Windows.Forms.CheckBox chkProfileEnergy;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkInstantaneousData;
        private System.Windows.Forms.CheckBox chkDailyMaximumData;
        private System.Windows.Forms.CheckBox chkDailyEnergyConsumption;
        private System.Windows.Forms.CheckBox chkCumulativeTamperEvents;
        private System.Windows.Forms.CheckBox chkCumulativeEnergies;
        private System.Windows.Forms.CheckBox SMD_SelectAll;
        private System.Windows.Forms.CheckBox chkDetailedTamper;
        private System.Windows.Forms.GroupBox gbDetailedTamper;
        private System.Windows.Forms.Button btnDetailedCancel;
        private System.Windows.Forms.Button btnDetailedShow;
        private System.Windows.Forms.CheckBox chkBillingReport;
        private System.Windows.Forms.CheckBox chkPhasorReport;
    }
}
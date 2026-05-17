namespace CAB.UI
{
    partial class SelectDialog
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
            this.ChkgroupBox = new System.Windows.Forms.GroupBox();
            this.groupBoxOthers = new System.Windows.Forms.GroupBox();
            this.chkMidnightEnergy = new System.Windows.Forms.CheckBox();
            this.chkDailyEnergyConsumption = new System.Windows.Forms.CheckBox();
            this.chkTransactions = new System.Windows.Forms.CheckBox();
            this.chkTamper = new System.Windows.Forms.CheckBox();
            this.groupBoxBilling = new System.Windows.Forms.GroupBox();
            this.chkPowerOffDuration = new System.Windows.Forms.CheckBox();
            this.chkTouConfiguration = new System.Windows.Forms.CheckBox();
            this.chkPhasor = new System.Windows.Forms.CheckBox();
            this.chkMiscelleneous = new System.Windows.Forms.CheckBox();
            this.chkMainEnergy = new System.Windows.Forms.CheckBox();
            this.chkBilling = new System.Windows.Forms.CheckBox();
            this.chkInstantReport = new System.Windows.Forms.CheckBox();
            this.chkGeneralReport = new System.Windows.Forms.CheckBox();
            this.chkEnergyConsumption = new System.Windows.Forms.CheckBox();
            this.chkTODEnergy = new System.Windows.Forms.CheckBox();
            this.chkPowerFactor = new System.Windows.Forms.CheckBox();
            this.chkTODConsumption = new System.Windows.Forms.CheckBox();
            this.chkMaximumDemand = new System.Windows.Forms.CheckBox();
            this.groupBoxLoadSurvey = new System.Windows.Forms.GroupBox();
            this.chkLoadSurvey = new System.Windows.Forms.CheckBox();
            this.SMD_rbtnLoadSurveyEnergy = new System.Windows.Forms.RadioButton();
            this.SMD_rbtnLoadSurveyDemand = new System.Windows.Forms.RadioButton();
            this.SMD_chkDTMLoadSurvey = new System.Windows.Forms.CheckBox();
            this.chkBillingTamperCounter = new System.Windows.Forms.CheckBox();
            this.chkBillingMechanism = new System.Windows.Forms.CheckBox();
            this.chkCTRatio = new System.Windows.Forms.CheckBox();
            this.chkLoadFactor = new System.Windows.Forms.CheckBox();
            this.chkPowerOnHours = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SMD_SelectAll = new System.Windows.Forms.CheckBox();
            this.SMD_btnCancel = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.lngGridViewReadControl1 = new CAB.UI.Controls.CABGridViewReadControl();
            this.gridShow = new System.Windows.Forms.Button();
            this.lblNoData = new System.Windows.Forms.Label();
            this.ChkgroupBox.SuspendLayout();
            this.groupBoxOthers.SuspendLayout();
            this.groupBoxBilling.SuspendLayout();
            this.groupBoxLoadSurvey.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChkgroupBox
            // 
            this.ChkgroupBox.Controls.Add(this.groupBoxOthers);
            this.ChkgroupBox.Controls.Add(this.groupBoxBilling);
            this.ChkgroupBox.Location = new System.Drawing.Point(12, 36);
            this.ChkgroupBox.Name = "ChkgroupBox";
            this.ChkgroupBox.Size = new System.Drawing.Size(108, 94);
            this.ChkgroupBox.TabIndex = 0;
            this.ChkgroupBox.TabStop = false;
            this.ChkgroupBox.Visible = false;
            // 
            // groupBoxOthers
            // 
            this.groupBoxOthers.Controls.Add(this.chkMidnightEnergy);
            this.groupBoxOthers.Controls.Add(this.chkDailyEnergyConsumption);
            this.groupBoxOthers.Controls.Add(this.chkTransactions);
            this.groupBoxOthers.Controls.Add(this.chkTamper);
            this.groupBoxOthers.Location = new System.Drawing.Point(6, 204);
            this.groupBoxOthers.Name = "groupBoxOthers";
            this.groupBoxOthers.Size = new System.Drawing.Size(351, 74);
            this.groupBoxOthers.TabIndex = 2;
            this.groupBoxOthers.TabStop = false;
            this.groupBoxOthers.Text = "Others";
            // 
            // chkMidnightEnergy
            // 
            this.chkMidnightEnergy.AutoSize = true;
            this.chkMidnightEnergy.Location = new System.Drawing.Point(211, 46);
            this.chkMidnightEnergy.Name = "chkMidnightEnergy";
            this.chkMidnightEnergy.Size = new System.Drawing.Size(110, 17);
            this.chkMidnightEnergy.TabIndex = 3;
            this.chkMidnightEnergy.Text = "Midnight Energies";
            this.chkMidnightEnergy.UseVisualStyleBackColor = true;
            // 
            // chkDailyEnergyConsumption
            // 
            this.chkDailyEnergyConsumption.AutoSize = true;
            this.chkDailyEnergyConsumption.Location = new System.Drawing.Point(26, 46);
            this.chkDailyEnergyConsumption.Name = "chkDailyEnergyConsumption";
            this.chkDailyEnergyConsumption.Size = new System.Drawing.Size(149, 17);
            this.chkDailyEnergyConsumption.TabIndex = 2;
            this.chkDailyEnergyConsumption.Text = "Daily Energy Consumption";
            this.chkDailyEnergyConsumption.UseVisualStyleBackColor = true;
            // 
            // chkTransactions
            // 
            this.chkTransactions.AutoSize = true;
            this.chkTransactions.Location = new System.Drawing.Point(211, 19);
            this.chkTransactions.Name = "chkTransactions";
            this.chkTransactions.Size = new System.Drawing.Size(87, 17);
            this.chkTransactions.TabIndex = 1;
            this.chkTransactions.Text = "Transactions";
            this.chkTransactions.UseVisualStyleBackColor = true;
            // 
            // chkTamper
            // 
            this.chkTamper.AutoSize = true;
            this.chkTamper.Location = new System.Drawing.Point(26, 19);
            this.chkTamper.Name = "chkTamper";
            this.chkTamper.Size = new System.Drawing.Size(76, 17);
            this.chkTamper.TabIndex = 0;
            this.chkTamper.Text = "All Tamper";
            this.chkTamper.UseVisualStyleBackColor = true;
            // 
            // groupBoxBilling
            // 
            this.groupBoxBilling.Controls.Add(this.chkPowerOffDuration);
            this.groupBoxBilling.Controls.Add(this.chkTouConfiguration);
            this.groupBoxBilling.Controls.Add(this.chkPhasor);
            this.groupBoxBilling.Controls.Add(this.chkMiscelleneous);
            this.groupBoxBilling.Controls.Add(this.chkMainEnergy);
            this.groupBoxBilling.Controls.Add(this.chkBilling);
            this.groupBoxBilling.Controls.Add(this.chkInstantReport);
            this.groupBoxBilling.Controls.Add(this.chkGeneralReport);
            this.groupBoxBilling.Controls.Add(this.chkEnergyConsumption);
            this.groupBoxBilling.Controls.Add(this.chkTODEnergy);
            this.groupBoxBilling.Controls.Add(this.chkPowerFactor);
            this.groupBoxBilling.Controls.Add(this.chkTODConsumption);
            this.groupBoxBilling.Controls.Add(this.chkMaximumDemand);
            this.groupBoxBilling.Location = new System.Drawing.Point(6, 8);
            this.groupBoxBilling.Name = "groupBoxBilling";
            this.groupBoxBilling.Size = new System.Drawing.Size(96, 86);
            this.groupBoxBilling.TabIndex = 0;
            this.groupBoxBilling.TabStop = false;
            this.groupBoxBilling.Text = "Billing";
            // 
            // chkPowerOffDuration
            // 
            this.chkPowerOffDuration.AutoSize = true;
            this.chkPowerOffDuration.Location = new System.Drawing.Point(211, 131);
            this.chkPowerOffDuration.Name = "chkPowerOffDuration";
            this.chkPowerOffDuration.Size = new System.Drawing.Size(116, 17);
            this.chkPowerOffDuration.TabIndex = 17;
            this.chkPowerOffDuration.Text = "Power Off Duration";
            this.chkPowerOffDuration.UseVisualStyleBackColor = true;
            // 
            // chkTouConfiguration
            // 
            this.chkTouConfiguration.AutoSize = true;
            this.chkTouConfiguration.Location = new System.Drawing.Point(26, 131);
            this.chkTouConfiguration.Name = "chkTouConfiguration";
            this.chkTouConfiguration.Size = new System.Drawing.Size(114, 17);
            this.chkTouConfiguration.TabIndex = 16;
            this.chkTouConfiguration.Text = "TOU Configuration";
            this.chkTouConfiguration.UseVisualStyleBackColor = true;
            // 
            // chkPhasor
            // 
            this.chkPhasor.AutoSize = true;
            this.chkPhasor.Location = new System.Drawing.Point(211, 108);
            this.chkPhasor.Name = "chkPhasor";
            this.chkPhasor.Size = new System.Drawing.Size(59, 17);
            this.chkPhasor.TabIndex = 15;
            this.chkPhasor.Text = "Phasor";
            this.chkPhasor.UseVisualStyleBackColor = true;
            this.chkPhasor.Visible = false;
            // 
            // chkMiscelleneous
            // 
            this.chkMiscelleneous.AutoSize = true;
            this.chkMiscelleneous.Location = new System.Drawing.Point(26, 108);
            this.chkMiscelleneous.Name = "chkMiscelleneous";
            this.chkMiscelleneous.Size = new System.Drawing.Size(93, 17);
            this.chkMiscelleneous.TabIndex = 14;
            this.chkMiscelleneous.Text = "Miscellaneous";
            this.chkMiscelleneous.UseVisualStyleBackColor = true;
            // 
            // chkMainEnergy
            // 
            this.chkMainEnergy.AutoSize = true;
            this.chkMainEnergy.Location = new System.Drawing.Point(26, 64);
            this.chkMainEnergy.Name = "chkMainEnergy";
            this.chkMainEnergy.Size = new System.Drawing.Size(85, 17);
            this.chkMainEnergy.TabIndex = 2;
            this.chkMainEnergy.Text = "Main Energy";
            this.chkMainEnergy.UseVisualStyleBackColor = true;
            // 
            // chkBilling
            // 
            this.chkBilling.AutoSize = true;
            this.chkBilling.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBilling.Location = new System.Drawing.Point(211, 159);
            this.chkBilling.Name = "chkBilling";
            this.chkBilling.Size = new System.Drawing.Size(80, 17);
            this.chkBilling.TabIndex = 13;
            this.chkBilling.Text = "Select All";
            this.chkBilling.UseVisualStyleBackColor = true;
            this.chkBilling.CheckedChanged += new System.EventHandler(this.chkBilling_CheckedChanged);
            // 
            // chkInstantReport
            // 
            this.chkInstantReport.AutoSize = true;
            this.chkInstantReport.Location = new System.Drawing.Point(26, 43);
            this.chkInstantReport.Name = "chkInstantReport";
            this.chkInstantReport.Size = new System.Drawing.Size(93, 17);
            this.chkInstantReport.TabIndex = 1;
            this.chkInstantReport.Text = "Instant Report";
            this.chkInstantReport.UseVisualStyleBackColor = true;
            // 
            // chkGeneralReport
            // 
            this.chkGeneralReport.AutoSize = true;
            this.chkGeneralReport.Location = new System.Drawing.Point(26, 22);
            this.chkGeneralReport.Name = "chkGeneralReport";
            this.chkGeneralReport.Size = new System.Drawing.Size(98, 17);
            this.chkGeneralReport.TabIndex = 0;
            this.chkGeneralReport.Text = "General Report";
            this.chkGeneralReport.UseVisualStyleBackColor = true;
            // 
            // chkEnergyConsumption
            // 
            this.chkEnergyConsumption.AutoSize = true;
            this.chkEnergyConsumption.Location = new System.Drawing.Point(26, 85);
            this.chkEnergyConsumption.Name = "chkEnergyConsumption";
            this.chkEnergyConsumption.Size = new System.Drawing.Size(149, 17);
            this.chkEnergyConsumption.TabIndex = 3;
            this.chkEnergyConsumption.Text = "Main Energy Consumption";
            this.chkEnergyConsumption.UseVisualStyleBackColor = true;
            // 
            // chkTODEnergy
            // 
            this.chkTODEnergy.AutoSize = true;
            this.chkTODEnergy.Location = new System.Drawing.Point(211, 23);
            this.chkTODEnergy.Name = "chkTODEnergy";
            this.chkTODEnergy.Size = new System.Drawing.Size(85, 17);
            this.chkTODEnergy.TabIndex = 4;
            this.chkTODEnergy.Text = "TOD Energy";
            this.chkTODEnergy.UseVisualStyleBackColor = true;
            // 
            // chkPowerFactor
            // 
            this.chkPowerFactor.AutoSize = true;
            this.chkPowerFactor.Location = new System.Drawing.Point(211, 87);
            this.chkPowerFactor.Name = "chkPowerFactor";
            this.chkPowerFactor.Size = new System.Drawing.Size(89, 17);
            this.chkPowerFactor.TabIndex = 8;
            this.chkPowerFactor.Text = "Power Factor";
            this.chkPowerFactor.UseVisualStyleBackColor = true;
            // 
            // chkTODConsumption
            // 
            this.chkTODConsumption.AutoSize = true;
            this.chkTODConsumption.Location = new System.Drawing.Point(211, 44);
            this.chkTODConsumption.Name = "chkTODConsumption";
            this.chkTODConsumption.Size = new System.Drawing.Size(113, 17);
            this.chkTODConsumption.TabIndex = 5;
            this.chkTODConsumption.Text = "TOD Consumption";
            this.chkTODConsumption.UseVisualStyleBackColor = true;
            // 
            // chkMaximumDemand
            // 
            this.chkMaximumDemand.AutoSize = true;
            this.chkMaximumDemand.Location = new System.Drawing.Point(211, 65);
            this.chkMaximumDemand.Name = "chkMaximumDemand";
            this.chkMaximumDemand.Size = new System.Drawing.Size(66, 17);
            this.chkMaximumDemand.TabIndex = 6;
            this.chkMaximumDemand.Text = "Demand";
            this.chkMaximumDemand.UseVisualStyleBackColor = true;
            // 
            // groupBoxLoadSurvey
            // 
            this.groupBoxLoadSurvey.Controls.Add(this.chkLoadSurvey);
            this.groupBoxLoadSurvey.Controls.Add(this.SMD_rbtnLoadSurveyEnergy);
            this.groupBoxLoadSurvey.Controls.Add(this.SMD_rbtnLoadSurveyDemand);
            this.groupBoxLoadSurvey.Location = new System.Drawing.Point(12, 3);
            this.groupBoxLoadSurvey.Name = "groupBoxLoadSurvey";
            this.groupBoxLoadSurvey.Size = new System.Drawing.Size(351, 14);
            this.groupBoxLoadSurvey.TabIndex = 1;
            this.groupBoxLoadSurvey.TabStop = false;
            this.groupBoxLoadSurvey.Text = "Load Survey";
            this.groupBoxLoadSurvey.Visible = false;
            // 
            // chkLoadSurvey
            // 
            this.chkLoadSurvey.AutoSize = true;
            this.chkLoadSurvey.Location = new System.Drawing.Point(26, 30);
            this.chkLoadSurvey.Name = "chkLoadSurvey";
            this.chkLoadSurvey.Size = new System.Drawing.Size(86, 17);
            this.chkLoadSurvey.TabIndex = 0;
            this.chkLoadSurvey.Text = "Load Survey";
            this.chkLoadSurvey.UseVisualStyleBackColor = true;
            this.chkLoadSurvey.CheckedChanged += new System.EventHandler(this.SMD_chkLoadSurvey_CheckedChanged);
            // 
            // SMD_rbtnLoadSurveyEnergy
            // 
            this.SMD_rbtnLoadSurveyEnergy.AutoSize = true;
            this.SMD_rbtnLoadSurveyEnergy.Location = new System.Drawing.Point(100, 53);
            this.SMD_rbtnLoadSurveyEnergy.Name = "SMD_rbtnLoadSurveyEnergy";
            this.SMD_rbtnLoadSurveyEnergy.Size = new System.Drawing.Size(58, 17);
            this.SMD_rbtnLoadSurveyEnergy.TabIndex = 2;
            this.SMD_rbtnLoadSurveyEnergy.Text = "Energy";
            this.SMD_rbtnLoadSurveyEnergy.UseVisualStyleBackColor = true;
            // 
            // SMD_rbtnLoadSurveyDemand
            // 
            this.SMD_rbtnLoadSurveyDemand.AutoSize = true;
            this.SMD_rbtnLoadSurveyDemand.Checked = true;
            this.SMD_rbtnLoadSurveyDemand.Location = new System.Drawing.Point(26, 53);
            this.SMD_rbtnLoadSurveyDemand.Name = "SMD_rbtnLoadSurveyDemand";
            this.SMD_rbtnLoadSurveyDemand.Size = new System.Drawing.Size(65, 17);
            this.SMD_rbtnLoadSurveyDemand.TabIndex = 1;
            this.SMD_rbtnLoadSurveyDemand.TabStop = true;
            this.SMD_rbtnLoadSurveyDemand.Text = "Demand";
            this.SMD_rbtnLoadSurveyDemand.UseVisualStyleBackColor = true;
            // 
            // SMD_chkDTMLoadSurvey
            // 
            this.SMD_chkDTMLoadSurvey.AutoSize = true;
            this.SMD_chkDTMLoadSurvey.Location = new System.Drawing.Point(142, 4);
            this.SMD_chkDTMLoadSurvey.Name = "SMD_chkDTMLoadSurvey";
            this.SMD_chkDTMLoadSurvey.Size = new System.Drawing.Size(113, 17);
            this.SMD_chkDTMLoadSurvey.TabIndex = 3;
            this.SMD_chkDTMLoadSurvey.Text = "DTM Load Survey";
            this.SMD_chkDTMLoadSurvey.UseVisualStyleBackColor = true;
            this.SMD_chkDTMLoadSurvey.Visible = false;
            this.SMD_chkDTMLoadSurvey.CheckedChanged += new System.EventHandler(this.SMD_chkDTMLoadSurvey_CheckedChanged);
            // 
            // chkBillingTamperCounter
            // 
            this.chkBillingTamperCounter.AutoSize = true;
            this.chkBillingTamperCounter.Location = new System.Drawing.Point(147, 6);
            this.chkBillingTamperCounter.Name = "chkBillingTamperCounter";
            this.chkBillingTamperCounter.Size = new System.Drawing.Size(132, 17);
            this.chkBillingTamperCounter.TabIndex = 10;
            this.chkBillingTamperCounter.Text = "Billing Tamper Counter";
            this.chkBillingTamperCounter.UseVisualStyleBackColor = true;
            this.chkBillingTamperCounter.Visible = false;
            // 
            // chkBillingMechanism
            // 
            this.chkBillingMechanism.AutoSize = true;
            this.chkBillingMechanism.Location = new System.Drawing.Point(144, 5);
            this.chkBillingMechanism.Name = "chkBillingMechanism";
            this.chkBillingMechanism.Size = new System.Drawing.Size(110, 17);
            this.chkBillingMechanism.TabIndex = 12;
            this.chkBillingMechanism.Text = "Billing Mechanism";
            this.chkBillingMechanism.UseVisualStyleBackColor = true;
            this.chkBillingMechanism.Visible = false;
            // 
            // chkCTRatio
            // 
            this.chkCTRatio.AutoSize = true;
            this.chkCTRatio.Location = new System.Drawing.Point(152, 5);
            this.chkCTRatio.Name = "chkCTRatio";
            this.chkCTRatio.Size = new System.Drawing.Size(68, 17);
            this.chkCTRatio.TabIndex = 11;
            this.chkCTRatio.Text = "CT Ratio";
            this.chkCTRatio.UseVisualStyleBackColor = true;
            this.chkCTRatio.Visible = false;
            // 
            // chkLoadFactor
            // 
            this.chkLoadFactor.AutoSize = true;
            this.chkLoadFactor.Location = new System.Drawing.Point(146, 5);
            this.chkLoadFactor.Name = "chkLoadFactor";
            this.chkLoadFactor.Size = new System.Drawing.Size(83, 17);
            this.chkLoadFactor.TabIndex = 9;
            this.chkLoadFactor.Text = "Load Factor";
            this.chkLoadFactor.UseVisualStyleBackColor = true;
            this.chkLoadFactor.Visible = false;
            // 
            // chkPowerOnHours
            // 
            this.chkPowerOnHours.AutoSize = true;
            this.chkPowerOnHours.Location = new System.Drawing.Point(145, 4);
            this.chkPowerOnHours.Name = "chkPowerOnHours";
            this.chkPowerOnHours.Size = new System.Drawing.Size(104, 17);
            this.chkPowerOnHours.TabIndex = 7;
            this.chkPowerOnHours.Text = "Power On Hours";
            this.chkPowerOnHours.UseVisualStyleBackColor = true;
            this.chkPowerOnHours.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 14);
            this.label1.TabIndex = 36;
            this.label1.Text = "Report Parameters : ";
            // 
            // SMD_SelectAll
            // 
            this.SMD_SelectAll.AutoSize = true;
            this.SMD_SelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SMD_SelectAll.Location = new System.Drawing.Point(40, 349);
            this.SMD_SelectAll.Name = "SMD_SelectAll";
            this.SMD_SelectAll.Size = new System.Drawing.Size(80, 17);
            this.SMD_SelectAll.TabIndex = 1;
            this.SMD_SelectAll.Text = "Select All";
            this.SMD_SelectAll.UseVisualStyleBackColor = true;
            this.SMD_SelectAll.Visible = false;
            this.SMD_SelectAll.CheckedChanged += new System.EventHandler(this.SMD_SelectAll_CheckedChanged);
            // 
            // SMD_btnCancel
            // 
            this.SMD_btnCancel.Location = new System.Drawing.Point(337, 349);
            this.SMD_btnCancel.Name = "SMD_btnCancel";
            this.SMD_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.SMD_btnCancel.TabIndex = 3;
            this.SMD_btnCancel.Text = "Cancel";
            this.SMD_btnCancel.UseVisualStyleBackColor = false;
            this.SMD_btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.SMD_btnCancel.ForeColor = System.Drawing.Color.White;
            this.SMD_btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SMD_btnCancel.FlatAppearance.BorderSize = 0;
            this.SMD_btnCancel.Click += new System.EventHandler(this.SMD_btnCancel_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(28, 372);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 2;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShow.ForeColor = System.Drawing.Color.White;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.FlatAppearance.BorderSize = 0;
            this.btnShow.Visible = false;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // lngGridViewReadControl1
            // 
            this.lngGridViewReadControl1.AutoScroll = true;
            this.lngGridViewReadControl1.Location = new System.Drawing.Point(18, 40);
            this.lngGridViewReadControl1.Name = "lngGridViewReadControl1";
            this.lngGridViewReadControl1.Size = new System.Drawing.Size(417, 303);
            this.lngGridViewReadControl1.TabIndex = 37;
            // 
            // gridShow
            // 
            this.gridShow.Location = new System.Drawing.Point(256, 350);
            this.gridShow.Name = "gridShow";
            this.gridShow.Size = new System.Drawing.Size(75, 23);
            this.gridShow.TabIndex = 38;
            this.gridShow.Text = "Show";
            this.gridShow.UseVisualStyleBackColor = false;
            this.gridShow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.gridShow.ForeColor = System.Drawing.Color.White;
            this.gridShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gridShow.FlatAppearance.BorderSize = 0;
            this.gridShow.Click += new System.EventHandler(this.gridShow_Click);
            // 
            // lblNoData
            // 
            this.lblNoData.AutoSize = true;
            this.lblNoData.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoData.ForeColor = System.Drawing.Color.Red;
            this.lblNoData.Location = new System.Drawing.Point(132, 129);
            this.lblNoData.Name = "lblNoData";
            this.lblNoData.Size = new System.Drawing.Size(109, 13);
            this.lblNoData.TabIndex = 39;
            this.lblNoData.Text = "No Data Available";
            this.lblNoData.Visible = false;
            // 
            // SelectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(441, 385);
            this.Controls.Add(this.lblNoData);
            this.Controls.Add(this.gridShow);
            this.Controls.Add(this.lngGridViewReadControl1);
            this.Controls.Add(this.SMD_chkDTMLoadSurvey);
            this.Controls.Add(this.groupBoxLoadSurvey);
            this.Controls.Add(this.chkBillingMechanism);
            this.Controls.Add(this.chkBillingTamperCounter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkCTRatio);
            this.Controls.Add(this.ChkgroupBox);
            this.Controls.Add(this.SMD_SelectAll);
            this.Controls.Add(this.SMD_btnCancel);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.chkPowerOnHours);
            this.Controls.Add(this.chkLoadFactor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Reports";
            this.Load += new System.EventHandler(this.SelectDialog_Load);
            this.ChkgroupBox.ResumeLayout(false);
            this.groupBoxOthers.ResumeLayout(false);
            this.groupBoxOthers.PerformLayout();
            this.groupBoxBilling.ResumeLayout(false);
            this.groupBoxBilling.PerformLayout();
            this.groupBoxLoadSurvey.ResumeLayout(false);
            this.groupBoxLoadSurvey.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ChkgroupBox;
        private System.Windows.Forms.CheckBox SMD_SelectAll;
        private System.Windows.Forms.Button SMD_btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkLoadFactor;
        private System.Windows.Forms.CheckBox chkTamper;
        private System.Windows.Forms.CheckBox chkPowerFactor;
        private System.Windows.Forms.CheckBox chkPowerOnHours;
        private System.Windows.Forms.CheckBox chkMaximumDemand;
        private System.Windows.Forms.CheckBox chkTODConsumption;
        private System.Windows.Forms.CheckBox chkTODEnergy;
        private System.Windows.Forms.CheckBox chkEnergyConsumption;
        private System.Windows.Forms.CheckBox chkGeneralReport;
        private System.Windows.Forms.CheckBox chkInstantReport;
        private System.Windows.Forms.GroupBox groupBoxLoadSurvey;
        private System.Windows.Forms.GroupBox groupBoxBilling;
        private System.Windows.Forms.CheckBox chkBilling;
        private System.Windows.Forms.CheckBox chkCTRatio;
        private System.Windows.Forms.CheckBox chkMainEnergy;
        private System.Windows.Forms.CheckBox chkBillingMechanism;
        private System.Windows.Forms.CheckBox chkBillingTamperCounter;
        private System.Windows.Forms.GroupBox groupBoxOthers;
        private System.Windows.Forms.RadioButton SMD_rbtnLoadSurveyEnergy;
        private System.Windows.Forms.RadioButton SMD_rbtnLoadSurveyDemand;
        private System.Windows.Forms.CheckBox chkLoadSurvey;
        private System.Windows.Forms.CheckBox chkTransactions;
        //private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer4;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lin_CbPh;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lin_CyPh;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lin_CrPh;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lin_Vbph;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lin_Vyph;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lin_Vrph;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lin_xAxis;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lin_yAxis;
        //private Microsoft.VisualBasic.PowerPacks.OvalShape CirclePhasordraw;
        private System.Windows.Forms.CheckBox SMD_chkDTMLoadSurvey;
        private System.Windows.Forms.CheckBox chkMiscelleneous;
        private System.Windows.Forms.CheckBox chkDailyEnergyConsumption;
        private System.Windows.Forms.CheckBox chkMidnightEnergy;
        private System.Windows.Forms.CheckBox chkPhasor;
        private System.Windows.Forms.CheckBox chkTouConfiguration;
        private System.Windows.Forms.CheckBox chkPowerOffDuration;
        private CAB.UI.Controls.CABGridViewReadControl lngGridViewReadControl1;
        private System.Windows.Forms.Button gridShow;
        private System.Windows.Forms.Label lblNoData;
    }
}


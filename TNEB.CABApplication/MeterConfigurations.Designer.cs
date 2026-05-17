namespace CAB.UI
{
    partial class MeterConfigurations
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
            this.components = new System.ComponentModel.Container();
            this.timer_RTC = new System.Windows.Forms.Timer(this.components);
            this.billingResetCtrl = new CABAppControl.Reset();
            this.tabDailyLog = new System.Windows.Forms.TabPage();
            this.dailyLog1 = new CABAppControl.DailyLog();
            this.tabReset = new System.Windows.Forms.TabPage();
            this.reset1 = new CABAppControl.Reset();
            this.tabBillingReset = new System.Windows.Forms.TabPage();
            this.billingReset1 = new CABAppControl.BillingReset();
            this.tabRTC = new System.Windows.Forms.TabPage();
            this.rtcCtrl = new CABAppControl.RTC();
            this.tabTOU = new System.Windows.Forms.TabPage();
            this.rdbFutureTOD = new System.Windows.Forms.RadioButton();
            this.rdbCurrentTOD = new System.Windows.Forms.RadioButton();
            this.touOperation1 = new CABAppControl.TOUOperation();
            this.tabDisplayParam = new System.Windows.Forms.TabPage();
            this.displayParameters = new CAB.UI.Controls.DisplayParamaters();
            this.tabkvarSelection = new System.Windows.Forms.TabPage();
            this.kvarSelection1 = new CABAppControl.kvarSelection();
            this.tabMDWithIP = new System.Windows.Forms.TabPage();
            this.grouBoxMDWithIP = new System.Windows.Forms.GroupBox();
            this.cmbDemandSubInterlavTime = new System.Windows.Forms.ComboBox();
            this.lblDemandSubIntervalTime = new System.Windows.Forms.Label();
            this.cmbDemandInterval = new System.Windows.Forms.ComboBox();
            this.lblTimeInterval = new System.Windows.Forms.Label();
            this.cmbDemandType = new System.Windows.Forms.ComboBox();
            this.lblDemandType = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.pnConfigOptions = new System.Windows.Forms.Panel();
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.btnCreateCfgFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.txterrorLog = new System.Windows.Forms.RichTextBox();
            this.grpCheckBoxes = new System.Windows.Forms.GroupBox();
            this.chkLockRS232 = new System.Windows.Forms.CheckBox();
            this.chkDailyLog = new System.Windows.Forms.CheckBox();
            this.chkReset = new System.Windows.Forms.CheckBox();
            this.chkBilingReset = new System.Windows.Forms.CheckBox();
            this.chkRTC = new System.Windows.Forms.CheckBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.chkTOD = new System.Windows.Forms.CheckBox();
            this.chkDisplayParam = new System.Windows.Forms.CheckBox();
            this.chkKVARSelcetion = new System.Windows.Forms.CheckBox();
            this.chkMDWithIP = new System.Windows.Forms.CheckBox();
            this.tabRS232LockUnlock = new System.Windows.Forms.TabControl();
            this.tabRS232 = new System.Windows.Forms.TabPage();
            this.chkLockRS232Port = new System.Windows.Forms.CheckBox();
            this.tabDailyLog.SuspendLayout();
            this.tabReset.SuspendLayout();
            this.tabBillingReset.SuspendLayout();
            this.tabRTC.SuspendLayout();
            this.tabTOU.SuspendLayout();
            this.tabDisplayParam.SuspendLayout();
            this.tabkvarSelection.SuspendLayout();
            this.tabMDWithIP.SuspendLayout();
            this.grouBoxMDWithIP.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.pnConfigOptions.SuspendLayout();
            this.grpCheckBoxes.SuspendLayout();
            this.tabRS232LockUnlock.SuspendLayout();
            this.tabRS232.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_RTC
            // 
            this.timer_RTC.Interval = 1000;
            this.timer_RTC.Tick += new System.EventHandler(this.timer_RTC_Tick);
            // 
            // billingResetCtrl
            // 
            this.billingResetCtrl.Location = new System.Drawing.Point(17, 16);
            this.billingResetCtrl.Name = "billingResetCtrl";
            this.billingResetCtrl.Size = new System.Drawing.Size(150, 59);
            this.billingResetCtrl.TabIndex = 0;
            // 
            // tabDailyLog
            // 
            this.tabDailyLog.Controls.Add(this.dailyLog1);
            this.tabDailyLog.Location = new System.Drawing.Point(4, 22);
            this.tabDailyLog.Name = "tabDailyLog";
            this.tabDailyLog.Size = new System.Drawing.Size(734, 449);
            this.tabDailyLog.TabIndex = 6;
            this.tabDailyLog.Text = "Daily Log";
            this.tabDailyLog.UseVisualStyleBackColor = true;
            // 
            // dailyLog1
            // 
            this.dailyLog1.Location = new System.Drawing.Point(16, 15);
            this.dailyLog1.Name = "dailyLog1";
            this.dailyLog1.Size = new System.Drawing.Size(311, 267);
            this.dailyLog1.TabIndex = 0;
            // 
            // tabReset
            // 
            this.tabReset.Controls.Add(this.reset1);
            this.tabReset.Location = new System.Drawing.Point(4, 22);
            this.tabReset.Name = "tabReset";
            this.tabReset.Size = new System.Drawing.Size(734, 449);
            this.tabReset.TabIndex = 7;
            this.tabReset.Text = "Billing Reset";
            this.tabReset.UseVisualStyleBackColor = true;
            // 
            // reset1
            // 
            this.reset1.Location = new System.Drawing.Point(3, 3);
            this.reset1.Name = "reset1";
            this.reset1.Size = new System.Drawing.Size(150, 150);
            this.reset1.TabIndex = 0;
            // 
            // tabBillingReset
            // 
            this.tabBillingReset.Controls.Add(this.billingReset1);
            this.tabBillingReset.Location = new System.Drawing.Point(4, 22);
            this.tabBillingReset.Name = "tabBillingReset";
            this.tabBillingReset.Size = new System.Drawing.Size(734, 449);
            this.tabBillingReset.TabIndex = 8;
            this.tabBillingReset.Text = "Billing Type";
            this.tabBillingReset.UseVisualStyleBackColor = true;
            // 
            // billingReset1
            // 
            this.billingReset1.Location = new System.Drawing.Point(3, 0);
            this.billingReset1.Name = "billingReset1";
            this.billingReset1.Size = new System.Drawing.Size(557, 403);
            this.billingReset1.TabIndex = 0;
            // 
            // tabRTC
            // 
            this.tabRTC.Controls.Add(this.rtcCtrl);
            this.tabRTC.Location = new System.Drawing.Point(4, 22);
            this.tabRTC.Name = "tabRTC";
            this.tabRTC.Size = new System.Drawing.Size(734, 449);
            this.tabRTC.TabIndex = 4;
            this.tabRTC.Text = "RTC";
            this.tabRTC.UseVisualStyleBackColor = true;
            // 
            // rtcCtrl
            // 
            this.rtcCtrl.Location = new System.Drawing.Point(91, 15);
            this.rtcCtrl.Name = "rtcCtrl";
            this.rtcCtrl.Size = new System.Drawing.Size(482, 384);
            this.rtcCtrl.TabIndex = 0;
            // 
            // tabTOU
            // 
            this.tabTOU.Controls.Add(this.rdbFutureTOD);
            this.tabTOU.Controls.Add(this.rdbCurrentTOD);
            this.tabTOU.Controls.Add(this.touOperation1);
            this.tabTOU.Location = new System.Drawing.Point(4, 22);
            this.tabTOU.Name = "tabTOU";
            this.tabTOU.Size = new System.Drawing.Size(734, 449);
            this.tabTOU.TabIndex = 5;
            this.tabTOU.Text = "TOD Operation";
            this.tabTOU.UseVisualStyleBackColor = true;
            // 
            // rdbFutureTOD
            // 
            this.rdbFutureTOD.AutoSize = true;
            this.rdbFutureTOD.Location = new System.Drawing.Point(555, 101);
            this.rdbFutureTOD.Name = "rdbFutureTOD";
            this.rdbFutureTOD.Size = new System.Drawing.Size(81, 17);
            this.rdbFutureTOD.TabIndex = 2;
            this.rdbFutureTOD.Text = "Future TOD";
            this.rdbFutureTOD.UseVisualStyleBackColor = true;
            this.rdbFutureTOD.CheckedChanged += new System.EventHandler(this.rdbFutureTOD_CheckedChanged);
            // 
            // rdbCurrentTOD
            // 
            this.rdbCurrentTOD.AutoSize = true;
            this.rdbCurrentTOD.Location = new System.Drawing.Point(555, 69);
            this.rdbCurrentTOD.Name = "rdbCurrentTOD";
            this.rdbCurrentTOD.Size = new System.Drawing.Size(85, 17);
            this.rdbCurrentTOD.TabIndex = 1;
            this.rdbCurrentTOD.Text = "Current TOD";
            this.rdbCurrentTOD.UseVisualStyleBackColor = true;
            this.rdbCurrentTOD.CheckedChanged += new System.EventHandler(this.rdbCurrentTOD_CheckedChanged);
            // 
            // touOperation1
            // 
            this.touOperation1.buttonclicked = false;
            this.touOperation1.Location = new System.Drawing.Point(67, 12);
            this.touOperation1.Name = "touOperation1";
            this.touOperation1.Size = new System.Drawing.Size(601, 395);
            this.touOperation1.TabIndex = 0;
            // 
            // tabDisplayParam
            // 
            this.tabDisplayParam.Controls.Add(this.displayParameters);
            this.tabDisplayParam.Location = new System.Drawing.Point(4, 22);
            this.tabDisplayParam.Name = "tabDisplayParam";
            this.tabDisplayParam.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplayParam.Size = new System.Drawing.Size(734, 449);
            this.tabDisplayParam.TabIndex = 3;
            this.tabDisplayParam.Text = "Display Parameters";
            this.tabDisplayParam.UseVisualStyleBackColor = true;
            // 
            // displayParameters
            // 
            this.displayParameters.AutoScroll = true;
            this.displayParameters.AutoSize = true;
            this.displayParameters.Location = new System.Drawing.Point(4, 4);
            this.displayParameters.Name = "displayParameters";
            this.displayParameters.Size = new System.Drawing.Size(727, 406);
            this.displayParameters.TabIndex = 0;
            this.displayParameters.Load += new System.EventHandler(this.displayParameters_Load);
            // 
            // tabkvarSelection
            // 
            this.tabkvarSelection.Controls.Add(this.kvarSelection1);
            this.tabkvarSelection.Location = new System.Drawing.Point(4, 22);
            this.tabkvarSelection.Name = "tabkvarSelection";
            this.tabkvarSelection.Padding = new System.Windows.Forms.Padding(3);
            this.tabkvarSelection.Size = new System.Drawing.Size(734, 449);
            this.tabkvarSelection.TabIndex = 2;
            this.tabkvarSelection.Text = "kvah Selection";
            this.tabkvarSelection.UseVisualStyleBackColor = true;
            // 
            // kvarSelection1
            // 
            this.kvarSelection1.Location = new System.Drawing.Point(22, 16);
            this.kvarSelection1.Name = "kvarSelection1";
            this.kvarSelection1.Size = new System.Drawing.Size(580, 90);
            this.kvarSelection1.TabIndex = 0;
            // 
            // tabMDWithIP
            // 
            this.tabMDWithIP.Controls.Add(this.grouBoxMDWithIP);
            this.tabMDWithIP.Location = new System.Drawing.Point(4, 22);
            this.tabMDWithIP.Name = "tabMDWithIP";
            this.tabMDWithIP.Padding = new System.Windows.Forms.Padding(3);
            this.tabMDWithIP.Size = new System.Drawing.Size(734, 449);
            this.tabMDWithIP.TabIndex = 1;
            this.tabMDWithIP.Text = "MD With IP";
            this.tabMDWithIP.UseVisualStyleBackColor = true;
            // 
            // grouBoxMDWithIP
            // 
            this.grouBoxMDWithIP.Controls.Add(this.cmbDemandSubInterlavTime);
            this.grouBoxMDWithIP.Controls.Add(this.lblDemandSubIntervalTime);
            this.grouBoxMDWithIP.Controls.Add(this.cmbDemandInterval);
            this.grouBoxMDWithIP.Controls.Add(this.lblTimeInterval);
            this.grouBoxMDWithIP.Controls.Add(this.cmbDemandType);
            this.grouBoxMDWithIP.Controls.Add(this.lblDemandType);
            this.grouBoxMDWithIP.Location = new System.Drawing.Point(10, 9);
            this.grouBoxMDWithIP.Name = "grouBoxMDWithIP";
            this.grouBoxMDWithIP.Size = new System.Drawing.Size(310, 184);
            this.grouBoxMDWithIP.TabIndex = 12;
            this.grouBoxMDWithIP.TabStop = false;
            this.grouBoxMDWithIP.Text = "MD With IP";
            // 
            // cmbDemandSubInterlavTime
            // 
            this.cmbDemandSubInterlavTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDemandSubInterlavTime.Enabled = false;
            this.cmbDemandSubInterlavTime.FormattingEnabled = true;
            this.cmbDemandSubInterlavTime.Items.AddRange(new object[] {
            "5",
            "10",
            "15"});
            this.cmbDemandSubInterlavTime.Location = new System.Drawing.Point(163, 118);
            this.cmbDemandSubInterlavTime.Name = "cmbDemandSubInterlavTime";
            this.cmbDemandSubInterlavTime.Size = new System.Drawing.Size(99, 21);
            this.cmbDemandSubInterlavTime.TabIndex = 14;
            // 
            // lblDemandSubIntervalTime
            // 
            this.lblDemandSubIntervalTime.AutoSize = true;
            this.lblDemandSubIntervalTime.Location = new System.Drawing.Point(54, 121);
            this.lblDemandSubIntervalTime.Name = "lblDemandSubIntervalTime";
            this.lblDemandSubIntervalTime.Size = new System.Drawing.Size(90, 13);
            this.lblDemandSubIntervalTime.TabIndex = 13;
            this.lblDemandSubIntervalTime.Text = "Sub Time Interval";
            // 
            // cmbDemandInterval
            // 
            this.cmbDemandInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDemandInterval.FormattingEnabled = true;
            this.cmbDemandInterval.Items.AddRange(new object[] {
            "15",
            "30",
            "60"});
            this.cmbDemandInterval.Location = new System.Drawing.Point(164, 82);
            this.cmbDemandInterval.Name = "cmbDemandInterval";
            this.cmbDemandInterval.Size = new System.Drawing.Size(99, 21);
            this.cmbDemandInterval.TabIndex = 12;
            this.cmbDemandInterval.SelectedIndexChanged += new System.EventHandler(this.cmbDemandInterval_SelectedIndexChanged);
            // 
            // lblTimeInterval
            // 
            this.lblTimeInterval.AutoSize = true;
            this.lblTimeInterval.Location = new System.Drawing.Point(54, 82);
            this.lblTimeInterval.Name = "lblTimeInterval";
            this.lblTimeInterval.Size = new System.Drawing.Size(68, 13);
            this.lblTimeInterval.TabIndex = 11;
            this.lblTimeInterval.Text = "Time Interval";
            // 
            // cmbDemandType
            // 
            this.cmbDemandType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDemandType.FormattingEnabled = true;
            this.cmbDemandType.Items.AddRange(new object[] {
            "Block Demand",
            "Sliding Demand"});
            this.cmbDemandType.Location = new System.Drawing.Point(163, 47);
            this.cmbDemandType.Name = "cmbDemandType";
            this.cmbDemandType.Size = new System.Drawing.Size(100, 21);
            this.cmbDemandType.TabIndex = 10;
            this.cmbDemandType.SelectedIndexChanged += new System.EventHandler(this.cmbDemandType_SelectedIndexChanged);
            // 
            // lblDemandType
            // 
            this.lblDemandType.AutoSize = true;
            this.lblDemandType.Location = new System.Drawing.Point(54, 47);
            this.lblDemandType.Name = "lblDemandType";
            this.lblDemandType.Size = new System.Drawing.Size(74, 13);
            this.lblDemandType.TabIndex = 9;
            this.lblDemandType.Text = "Demand Type";
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.pnConfigOptions);
            this.tabMain.Controls.Add(this.txterrorLog);
            this.tabMain.Controls.Add(this.grpCheckBoxes);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(734, 449);
            this.tabMain.TabIndex = 0;
            this.tabMain.Text = "Main";
            this.tabMain.UseVisualStyleBackColor = true;
            // 
            // pnConfigOptions
            // 
            this.pnConfigOptions.Controls.Add(this.btnUploadFile);
            this.pnConfigOptions.Controls.Add(this.btnCreateCfgFile);
            this.pnConfigOptions.Controls.Add(this.btnCancel);
            this.pnConfigOptions.Controls.Add(this.btnWrite);
            this.pnConfigOptions.Controls.Add(this.btnRead);
            this.pnConfigOptions.Location = new System.Drawing.Point(290, 174);
            this.pnConfigOptions.Name = "pnConfigOptions";
            this.pnConfigOptions.Size = new System.Drawing.Size(416, 34);
            this.pnConfigOptions.TabIndex = 7;
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.Location = new System.Drawing.Point(89, 0);
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(75, 23);
            this.btnUploadFile.TabIndex = 6;
            this.btnUploadFile.Text = "Upload File";
            this.btnUploadFile.UseVisualStyleBackColor = true;
            this.btnUploadFile.Click += new System.EventHandler(this.btnUploadFile_Click);
            // 
            // btnCreateCfgFile
            // 
            this.btnCreateCfgFile.Location = new System.Drawing.Point(12, 0);
            this.btnCreateCfgFile.Name = "btnCreateCfgFile";
            this.btnCreateCfgFile.Size = new System.Drawing.Size(71, 23);
            this.btnCreateCfgFile.TabIndex = 5;
            this.btnCreateCfgFile.Text = "Create File";
            this.btnCreateCfgFile.UseVisualStyleBackColor = true;
            this.btnCreateCfgFile.Click += new System.EventHandler(this.btnCreateCfgFile_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(331, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(250, 0);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 2;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(169, 0);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 1;
            this.btnRead.Text = "Read";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // txterrorLog
            // 
            this.txterrorLog.BackColor = System.Drawing.Color.White;
            this.txterrorLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txterrorLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txterrorLog.ForeColor = System.Drawing.Color.Red;
            this.txterrorLog.Location = new System.Drawing.Point(31, 203);
            this.txterrorLog.Name = "txterrorLog";
            this.txterrorLog.ReadOnly = true;
            this.txterrorLog.Size = new System.Drawing.Size(674, 184);
            this.txterrorLog.TabIndex = 4;
            this.txterrorLog.Text = "";
            this.txterrorLog.Visible = false;
            // 
            // grpCheckBoxes
            // 
            this.grpCheckBoxes.Controls.Add(this.chkLockRS232);
            this.grpCheckBoxes.Controls.Add(this.chkDailyLog);
            this.grpCheckBoxes.Controls.Add(this.chkReset);
            this.grpCheckBoxes.Controls.Add(this.chkBilingReset);
            this.grpCheckBoxes.Controls.Add(this.chkRTC);
            this.grpCheckBoxes.Controls.Add(this.chkSelectAll);
            this.grpCheckBoxes.Controls.Add(this.chkTOD);
            this.grpCheckBoxes.Controls.Add(this.chkDisplayParam);
            this.grpCheckBoxes.Controls.Add(this.chkKVARSelcetion);
            this.grpCheckBoxes.Controls.Add(this.chkMDWithIP);
            this.grpCheckBoxes.Location = new System.Drawing.Point(31, 6);
            this.grpCheckBoxes.Name = "grpCheckBoxes";
            this.grpCheckBoxes.Size = new System.Drawing.Size(665, 162);
            this.grpCheckBoxes.TabIndex = 0;
            this.grpCheckBoxes.TabStop = false;
            // 
            // chkLockRS232
            // 
            this.chkLockRS232.AutoSize = true;
            this.chkLockRS232.Location = new System.Drawing.Point(510, 83);
            this.chkLockRS232.Name = "chkLockRS232";
            this.chkLockRS232.Size = new System.Drawing.Size(125, 17);
            this.chkLockRS232.TabIndex = 9;
            this.chkLockRS232.Text = "Lock/Unlock RS232";
            this.chkLockRS232.UseVisualStyleBackColor = true;
            this.chkLockRS232.CheckedChanged += new System.EventHandler(this.chkLockRS232_CheckedChanged);
            // 
            // chkDailyLog
            // 
            this.chkDailyLog.AutoSize = true;
            this.chkDailyLog.Location = new System.Drawing.Point(510, 55);
            this.chkDailyLog.Name = "chkDailyLog";
            this.chkDailyLog.Size = new System.Drawing.Size(70, 17);
            this.chkDailyLog.TabIndex = 8;
            this.chkDailyLog.Text = "Daily Log";
            this.chkDailyLog.UseVisualStyleBackColor = true;
            this.chkDailyLog.CheckedChanged += new System.EventHandler(this.chkDailyLog_CheckedChanged);
            // 
            // chkReset
            // 
            this.chkReset.AutoSize = true;
            this.chkReset.Location = new System.Drawing.Point(510, 27);
            this.chkReset.Name = "chkReset";
            this.chkReset.Size = new System.Drawing.Size(84, 17);
            this.chkReset.TabIndex = 7;
            this.chkReset.Text = "Billing Reset";
            this.chkReset.UseVisualStyleBackColor = true;
            this.chkReset.CheckedChanged += new System.EventHandler(this.chkReset_CheckedChanged);
            // 
            // chkBilingReset
            // 
            this.chkBilingReset.AutoSize = true;
            this.chkBilingReset.Enabled = false;
            this.chkBilingReset.Location = new System.Drawing.Point(295, 83);
            this.chkBilingReset.Name = "chkBilingReset";
            this.chkBilingReset.Size = new System.Drawing.Size(80, 17);
            this.chkBilingReset.TabIndex = 6;
            this.chkBilingReset.Text = "Billing Type";
            this.chkBilingReset.UseVisualStyleBackColor = true;
            this.chkBilingReset.CheckedChanged += new System.EventHandler(this.chkBilingReset_CheckedChanged);
            // 
            // chkRTC
            // 
            this.chkRTC.AutoSize = true;
            this.chkRTC.Location = new System.Drawing.Point(295, 55);
            this.chkRTC.Name = "chkRTC";
            this.chkRTC.Size = new System.Drawing.Size(48, 17);
            this.chkRTC.TabIndex = 5;
            this.chkRTC.Text = "RTC";
            this.chkRTC.UseVisualStyleBackColor = true;
            this.chkRTC.CheckedChanged += new System.EventHandler(this.chkRTC_CheckedChanged);
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(28, 127);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 4;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // chkTOD
            // 
            this.chkTOD.AutoSize = true;
            this.chkTOD.Location = new System.Drawing.Point(294, 27);
            this.chkTOD.Name = "chkTOD";
            this.chkTOD.Size = new System.Drawing.Size(49, 17);
            this.chkTOD.TabIndex = 3;
            this.chkTOD.Text = "TOD";
            this.chkTOD.UseVisualStyleBackColor = true;
            this.chkTOD.CheckedChanged += new System.EventHandler(this.chkTOD_CheckedChanged);
            // 
            // chkDisplayParam
            // 
            this.chkDisplayParam.AutoSize = true;
            this.chkDisplayParam.Location = new System.Drawing.Point(28, 83);
            this.chkDisplayParam.Name = "chkDisplayParam";
            this.chkDisplayParam.Size = new System.Drawing.Size(116, 17);
            this.chkDisplayParam.TabIndex = 2;
            this.chkDisplayParam.Text = "Display Parameters";
            this.chkDisplayParam.UseVisualStyleBackColor = true;
            this.chkDisplayParam.CheckedChanged += new System.EventHandler(this.chkDisplayParam_CheckedChanged);
            // 
            // chkKVARSelcetion
            // 
            this.chkKVARSelcetion.AutoSize = true;
            this.chkKVARSelcetion.Location = new System.Drawing.Point(28, 55);
            this.chkKVARSelcetion.Name = "chkKVARSelcetion";
            this.chkKVARSelcetion.Size = new System.Drawing.Size(97, 17);
            this.chkKVARSelcetion.TabIndex = 1;
            this.chkKVARSelcetion.Text = "kvah Selection";
            this.chkKVARSelcetion.UseVisualStyleBackColor = true;
            this.chkKVARSelcetion.CheckedChanged += new System.EventHandler(this.chkKVARSelcetion_CheckedChanged);
            // 
            // chkMDWithIP
            // 
            this.chkMDWithIP.AutoSize = true;
            this.chkMDWithIP.Location = new System.Drawing.Point(28, 27);
            this.chkMDWithIP.Name = "chkMDWithIP";
            this.chkMDWithIP.Size = new System.Drawing.Size(78, 17);
            this.chkMDWithIP.TabIndex = 0;
            this.chkMDWithIP.Text = "MD with IP";
            this.chkMDWithIP.UseVisualStyleBackColor = true;
            this.chkMDWithIP.CheckedChanged += new System.EventHandler(this.chkMDWithIP_CheckedChanged);
            // 
            // tabRS232LockUnlock
            // 
            this.tabRS232LockUnlock.Controls.Add(this.tabMain);
            this.tabRS232LockUnlock.Controls.Add(this.tabMDWithIP);
            this.tabRS232LockUnlock.Controls.Add(this.tabkvarSelection);
            this.tabRS232LockUnlock.Controls.Add(this.tabDisplayParam);
            this.tabRS232LockUnlock.Controls.Add(this.tabTOU);
            this.tabRS232LockUnlock.Controls.Add(this.tabRTC);
            this.tabRS232LockUnlock.Controls.Add(this.tabBillingReset);
            this.tabRS232LockUnlock.Controls.Add(this.tabReset);
            this.tabRS232LockUnlock.Controls.Add(this.tabDailyLog);
            this.tabRS232LockUnlock.Controls.Add(this.tabRS232);
            this.tabRS232LockUnlock.Location = new System.Drawing.Point(17, 37);
            this.tabRS232LockUnlock.Name = "tabRS232LockUnlock";
            this.tabRS232LockUnlock.SelectedIndex = 0;
            this.tabRS232LockUnlock.Size = new System.Drawing.Size(742, 475);
            this.tabRS232LockUnlock.TabIndex = 0;
            this.tabRS232LockUnlock.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseClick);
            // 
            // tabRS232
            // 
            this.tabRS232.Controls.Add(this.chkLockRS232Port);
            this.tabRS232.Location = new System.Drawing.Point(4, 22);
            this.tabRS232.Name = "tabRS232";
            this.tabRS232.Size = new System.Drawing.Size(734, 449);
            this.tabRS232.TabIndex = 9;
            this.tabRS232.Text = "LockRS232";
            this.tabRS232.UseVisualStyleBackColor = true;
            // 
            // chkLockRS232Port
            // 
            this.chkLockRS232Port.AutoSize = true;
            this.chkLockRS232Port.Location = new System.Drawing.Point(29, 30);
            this.chkLockRS232Port.Name = "chkLockRS232Port";
            this.chkLockRS232Port.Size = new System.Drawing.Size(86, 17);
            this.chkLockRS232Port.TabIndex = 0;
            this.chkLockRS232Port.Text = "Lock RS232";
            this.chkLockRS232Port.UseVisualStyleBackColor = true;
            // 
            // MeterConfigurations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(771, 544);
            this.Controls.Add(this.tabRS232LockUnlock);
            this.Name = "MeterConfigurations";
            this.StatusMessage = "";
            this.Text = "WCM Meter Configurations";
            this.Load += new System.EventHandler(this.MeterConfigurations_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MeterConfigurations_FormClosing);
            this.tabDailyLog.ResumeLayout(false);
            this.tabReset.ResumeLayout(false);
            this.tabBillingReset.ResumeLayout(false);
            this.tabRTC.ResumeLayout(false);
            this.tabTOU.ResumeLayout(false);
            this.tabTOU.PerformLayout();
            this.tabDisplayParam.ResumeLayout(false);
            this.tabDisplayParam.PerformLayout();
            this.tabkvarSelection.ResumeLayout(false);
            this.tabMDWithIP.ResumeLayout(false);
            this.grouBoxMDWithIP.ResumeLayout(false);
            this.grouBoxMDWithIP.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.pnConfigOptions.ResumeLayout(false);
            this.grpCheckBoxes.ResumeLayout(false);
            this.grpCheckBoxes.PerformLayout();
            this.tabRS232LockUnlock.ResumeLayout(false);
            this.tabRS232.ResumeLayout(false);
            this.tabRS232.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer_RTC;
        private CABAppControl.Reset billingResetCtrl;
        private System.Windows.Forms.TabPage tabDailyLog;
        private CABAppControl.DailyLog dailyLog1;
        private System.Windows.Forms.TabPage tabReset;
        private CABAppControl.Reset reset1;
        private System.Windows.Forms.TabPage tabBillingReset;
        private CABAppControl.BillingReset billingReset1;
        private System.Windows.Forms.TabPage tabRTC;
        private CABAppControl.RTC rtcCtrl;
        private System.Windows.Forms.TabPage tabTOU;
        private System.Windows.Forms.RadioButton rdbFutureTOD;
        private System.Windows.Forms.RadioButton rdbCurrentTOD;
        private CABAppControl.TOUOperation touOperation1;
        private System.Windows.Forms.TabPage tabDisplayParam;
        private CAB.UI.Controls.DisplayParamaters displayParameters;
        private System.Windows.Forms.TabPage tabkvarSelection;
        private CABAppControl.kvarSelection kvarSelection1;
        private System.Windows.Forms.TabPage tabMDWithIP;
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.RichTextBox txterrorLog;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.GroupBox grpCheckBoxes;
        private System.Windows.Forms.CheckBox chkLockRS232;
        private System.Windows.Forms.CheckBox chkDailyLog;
        private System.Windows.Forms.CheckBox chkReset;
        private System.Windows.Forms.CheckBox chkBilingReset;
        private System.Windows.Forms.CheckBox chkRTC;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.CheckBox chkTOD;
        private System.Windows.Forms.CheckBox chkDisplayParam;
        private System.Windows.Forms.CheckBox chkKVARSelcetion;
        private System.Windows.Forms.CheckBox chkMDWithIP;
        private System.Windows.Forms.TabControl tabRS232LockUnlock;
        private System.Windows.Forms.TabPage tabRS232;
        private System.Windows.Forms.CheckBox chkLockRS232Port;
        private System.Windows.Forms.Button btnCreateCfgFile;
        private System.Windows.Forms.Button btnUploadFile;
        private System.Windows.Forms.Panel pnConfigOptions;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.GroupBox grouBoxMDWithIP;
        private System.Windows.Forms.ComboBox cmbDemandSubInterlavTime;
        private System.Windows.Forms.Label lblDemandSubIntervalTime;
        private System.Windows.Forms.ComboBox cmbDemandInterval;
        private System.Windows.Forms.Label lblTimeInterval;
        private System.Windows.Forms.ComboBox cmbDemandType;
        private System.Windows.Forms.Label lblDemandType;

    }
}
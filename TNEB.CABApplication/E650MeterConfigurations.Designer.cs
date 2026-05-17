namespace CAB.UI
{
    partial class E650MeterConfigurations
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
            this.tabReset = new System.Windows.Forms.TabPage();
            this.groupBoxMDReset = new System.Windows.Forms.GroupBox();
            this.chkMDReset = new System.Windows.Forms.CheckBox();
            this.tabBillingReset = new System.Windows.Forms.TabPage();
            this.gbManual = new System.Windows.Forms.GroupBox();
            this.cmbResetLockoutdays = new System.Windows.Forms.ComboBox();
            this.lblResetLockOutDays = new System.Windows.Forms.Label();
            this.groupBoxBillingTYpe = new System.Windows.Forms.GroupBox();
            this.cmbBoxBillingMinute = new System.Windows.Forms.ComboBox();
            this.cmbBoxBillingHour = new System.Windows.Forms.ComboBox();
            this.cmbBoxBillingDate = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.cmbBoxBillingPeriod = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.tabRTC = new System.Windows.Forms.TabPage();
            this.rtcCtrl = new CABAppControl.RTC();
            this.tabTOU = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdbFutureTOD = new System.Windows.Forms.RadioButton();
            this.rdbCurrentTOD = new System.Windows.Forms.RadioButton();
            this.btnResetTOUConfiguration = new System.Windows.Forms.Button();
            this.btnAutoFillTOUConfiguration = new System.Windows.Forms.Button();
            this.dtpFutureActivationDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dgvSeasonProfile = new System.Windows.Forms.DataGridView();
            this.dgvWeekProfile = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.dgvDayProfile = new System.Windows.Forms.DataGridView();
            this.tabDisplayParam = new System.Windows.Forms.TabPage();
            this.tlpDisplayParameter = new System.Windows.Forms.TableLayoutPanel();
            this.tabControlDisplayParams = new System.Windows.Forms.TabControl();
            this.tabPagePushButton = new System.Windows.Forms.TabPage();
            this.dGVPushDisplayParams = new System.Windows.Forms.DataGridView();
            this.tabPageScrollButton = new System.Windows.Forms.TabPage();
            this.dGVScrollDisplayParams = new System.Windows.Forms.DataGridView();
            this.tabPageHighResolution = new System.Windows.Forms.TabPage();
            this.dGVHighResolution = new System.Windows.Forms.DataGridView();
            this.tabPageDisplayTimeOut = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAutoScrollTime = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtScrollTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPushButtonTimeout = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtScrollResumeTime = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnUpScroll = new System.Windows.Forms.Button();
            this.chkDisplayParamSelectAll = new System.Windows.Forms.CheckBox();
            this.btnDownScroll = new System.Windows.Forms.Button();
            this.tabkvarSelection = new System.Windows.Forms.TabPage();
            this.groupBox59 = new System.Windows.Forms.GroupBox();
            this.rdbKVAhLagLead = new System.Windows.Forms.RadioButton();
            this.rdbKVAhLagOnly = new System.Windows.Forms.RadioButton();
            this.tabMDWithIP = new System.Windows.Forms.TabPage();
            this.grouBoxMDWithIP = new System.Windows.Forms.GroupBox();
            this.cmbDemandSubInterlavTime = new System.Windows.Forms.ComboBox();
            this.lblDemandSubIntervalTime = new System.Windows.Forms.Label();
            this.cmbDemandInterval = new System.Windows.Forms.ComboBox();
            this.lblTimeInterval = new System.Windows.Forms.Label();
            this.cmbDemandType = new System.Windows.Forms.ComboBox();
            this.lblDemandType = new System.Windows.Forms.Label();
            this.tabRS232LockUnlock = new System.Windows.Forms.TabControl();
            this.tabMain = new System.Windows.Forms.TabPage();
            this.pnConfigOptions = new System.Windows.Forms.Panel();
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.btnCreateCfgFile = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.txterrorLog = new System.Windows.Forms.RichTextBox();
            this.grpCheckBoxes = new System.Windows.Forms.GroupBox();
            this.chkAutoLock = new System.Windows.Forms.CheckBox();
            this.chkLockRS232 = new System.Windows.Forms.CheckBox();
            this.chkLSCapturePeriod = new System.Windows.Forms.CheckBox();
            this.chkBillingReset = new System.Windows.Forms.CheckBox();
            this.chkBilingType = new System.Windows.Forms.CheckBox();
            this.chkRTC = new System.Windows.Forms.CheckBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.chkTOD = new System.Windows.Forms.CheckBox();
            this.chkDisplayParam = new System.Windows.Forms.CheckBox();
            this.chkKVARSelcetion = new System.Windows.Forms.CheckBox();
            this.chkMDWithIP = new System.Windows.Forms.CheckBox();
            this.tabRS232 = new System.Windows.Forms.TabPage();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.rdbRS232Unlock = new System.Windows.Forms.RadioButton();
            this.rdbRS232Lock = new System.Windows.Forms.RadioButton();
            this.tabPageLSCapturePeriod = new System.Windows.Forms.TabPage();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.lblLoadSurveyCapturePeriod = new System.Windows.Forms.Label();
            this.cmbBoxLSCapturePeriod = new System.Windows.Forms.ComboBox();
            this.tabPageAutoLock = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rdbAutoUnlock = new System.Windows.Forms.RadioButton();
            this.rdbAutoLock = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnWriteDisplayParams = new System.Windows.Forms.Button();
            this.btnReadDisplayParams = new System.Windows.Forms.Button();
            this.chkBoxSelectAll = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPageSeason1 = new System.Windows.Forms.TabPage();
            this.grpDayTables = new System.Windows.Forms.GroupBox();
            this.lblDayTable6 = new System.Windows.Forms.Label();
            this.lblDayTable5 = new System.Windows.Forms.Label();
            this.lblDayTable4 = new System.Windows.Forms.Label();
            this.lblDayTable3 = new System.Windows.Forms.Label();
            this.lblDayTable2 = new System.Windows.Forms.Label();
            this.lblDayTable1 = new System.Windows.Forms.Label();
            this.tabPageSeason2 = new System.Windows.Forms.TabPage();
            this.groupBox26 = new System.Windows.Forms.GroupBox();
            this.label51 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.tabPageSeason3 = new System.Windows.Forms.TabPage();
            this.groupBox27 = new System.Windows.Forms.GroupBox();
            this.label57 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.tabPageSeason4 = new System.Windows.Forms.TabPage();
            this.groupBox28 = new System.Windows.Forms.GroupBox();
            this.label63 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.label191 = new System.Windows.Forms.Label();
            this.lblActivation = new System.Windows.Forms.Label();
            this.lblDayTable = new System.Windows.Forms.Label();
            this.tabReset.SuspendLayout();
            this.groupBoxMDReset.SuspendLayout();
            this.tabBillingReset.SuspendLayout();
            this.gbManual.SuspendLayout();
            this.groupBoxBillingTYpe.SuspendLayout();
            this.tabRTC.SuspendLayout();
            this.tabTOU.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeasonProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeekProfile)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayProfile)).BeginInit();
            this.tabDisplayParam.SuspendLayout();
            this.tlpDisplayParameter.SuspendLayout();
            this.tabControlDisplayParams.SuspendLayout();
            this.tabPagePushButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVPushDisplayParams)).BeginInit();
            this.tabPageScrollButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVScrollDisplayParams)).BeginInit();
            this.tabPageHighResolution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGVHighResolution)).BeginInit();
            this.tabPageDisplayTimeOut.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabkvarSelection.SuspendLayout();
            this.groupBox59.SuspendLayout();
            this.tabMDWithIP.SuspendLayout();
            this.grouBoxMDWithIP.SuspendLayout();
            this.tabRS232LockUnlock.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.pnConfigOptions.SuspendLayout();
            this.grpCheckBoxes.SuspendLayout();
            this.tabRS232.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.tabPageLSCapturePeriod.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.tabPageAutoLock.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox15.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel16.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPageSeason1.SuspendLayout();
            this.grpDayTables.SuspendLayout();
            this.tabPageSeason2.SuspendLayout();
            this.groupBox26.SuspendLayout();
            this.tabPageSeason3.SuspendLayout();
            this.groupBox27.SuspendLayout();
            this.tabPageSeason4.SuspendLayout();
            this.groupBox28.SuspendLayout();
            this.groupBox25.SuspendLayout();
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
            this.billingResetCtrl.Margin = new System.Windows.Forms.Padding(4);
            this.billingResetCtrl.Name = "billingResetCtrl";
            this.billingResetCtrl.Size = new System.Drawing.Size(150, 59);
            this.billingResetCtrl.TabIndex = 0;
            // 
            // tabReset
            // 
            this.tabReset.Controls.Add(this.groupBoxMDReset);
            this.tabReset.Location = new System.Drawing.Point(4, 22);
            this.tabReset.Name = "tabReset";
            this.tabReset.Size = new System.Drawing.Size(692, 424);
            this.tabReset.TabIndex = 7;
            this.tabReset.Text = "Billing Reset";
            this.tabReset.UseVisualStyleBackColor = true;
            // 
            // groupBoxMDReset
            // 
            this.groupBoxMDReset.Controls.Add(this.chkMDReset);
            this.groupBoxMDReset.Location = new System.Drawing.Point(10, 13);
            this.groupBoxMDReset.Name = "groupBoxMDReset";
            this.groupBoxMDReset.Size = new System.Drawing.Size(273, 125);
            this.groupBoxMDReset.TabIndex = 1;
            this.groupBoxMDReset.TabStop = false;
            this.groupBoxMDReset.Text = "Billing Reset";
            // 
            // chkMDReset
            // 
            this.chkMDReset.AutoSize = true;
            this.chkMDReset.Location = new System.Drawing.Point(6, 35);
            this.chkMDReset.Name = "chkMDReset";
            this.chkMDReset.Size = new System.Drawing.Size(84, 17);
            this.chkMDReset.TabIndex = 0;
            this.chkMDReset.Text = "Billing Reset";
            this.chkMDReset.UseVisualStyleBackColor = true;
            // 
            // tabBillingReset
            // 
            this.tabBillingReset.Controls.Add(this.gbManual);
            this.tabBillingReset.Controls.Add(this.groupBoxBillingTYpe);
            this.tabBillingReset.Location = new System.Drawing.Point(4, 22);
            this.tabBillingReset.Name = "tabBillingReset";
            this.tabBillingReset.Size = new System.Drawing.Size(692, 424);
            this.tabBillingReset.TabIndex = 8;
            this.tabBillingReset.Text = "Billing Type";
            this.tabBillingReset.UseVisualStyleBackColor = true;
            // 
            // gbManual
            // 
            this.gbManual.Controls.Add(this.cmbResetLockoutdays);
            this.gbManual.Controls.Add(this.lblResetLockOutDays);
            this.gbManual.Location = new System.Drawing.Point(13, 242);
            this.gbManual.Name = "gbManual";
            this.gbManual.Size = new System.Drawing.Size(402, 63);
            this.gbManual.TabIndex = 5;
            this.gbManual.TabStop = false;
            this.gbManual.Text = "Manual";
            // 
            // cmbResetLockoutdays
            // 
            this.cmbResetLockoutdays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResetLockoutdays.FormattingEnabled = true;
            this.cmbResetLockoutdays.Location = new System.Drawing.Point(134, 22);
            this.cmbResetLockoutdays.Name = "cmbResetLockoutdays";
            this.cmbResetLockoutdays.Size = new System.Drawing.Size(121, 21);
            this.cmbResetLockoutdays.TabIndex = 1;
            // 
            // lblResetLockOutDays
            // 
            this.lblResetLockOutDays.AutoSize = true;
            this.lblResetLockOutDays.Location = new System.Drawing.Point(13, 25);
            this.lblResetLockOutDays.Name = "lblResetLockOutDays";
            this.lblResetLockOutDays.Size = new System.Drawing.Size(107, 13);
            this.lblResetLockOutDays.TabIndex = 0;
            this.lblResetLockOutDays.Text = "Reset Lock out Days";
            // 
            // groupBoxBillingTYpe
            // 
            this.groupBoxBillingTYpe.Controls.Add(this.cmbBoxBillingMinute);
            this.groupBoxBillingTYpe.Controls.Add(this.cmbBoxBillingHour);
            this.groupBoxBillingTYpe.Controls.Add(this.cmbBoxBillingDate);
            this.groupBoxBillingTYpe.Controls.Add(this.label23);
            this.groupBoxBillingTYpe.Controls.Add(this.label24);
            this.groupBoxBillingTYpe.Controls.Add(this.label25);
            this.groupBoxBillingTYpe.Controls.Add(this.cmbBoxBillingPeriod);
            this.groupBoxBillingTYpe.Controls.Add(this.label26);
            this.groupBoxBillingTYpe.Location = new System.Drawing.Point(13, 12);
            this.groupBoxBillingTYpe.Name = "groupBoxBillingTYpe";
            this.groupBoxBillingTYpe.Size = new System.Drawing.Size(402, 210);
            this.groupBoxBillingTYpe.TabIndex = 2;
            this.groupBoxBillingTYpe.TabStop = false;
            this.groupBoxBillingTYpe.Text = "AutoMode";
            // 
            // cmbBoxBillingMinute
            // 
            this.cmbBoxBillingMinute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxBillingMinute.Enabled = false;
            this.cmbBoxBillingMinute.FormattingEnabled = true;
            this.cmbBoxBillingMinute.Location = new System.Drawing.Point(137, 157);
            this.cmbBoxBillingMinute.Name = "cmbBoxBillingMinute";
            this.cmbBoxBillingMinute.Size = new System.Drawing.Size(121, 21);
            this.cmbBoxBillingMinute.TabIndex = 3;
            // 
            // cmbBoxBillingHour
            // 
            this.cmbBoxBillingHour.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxBillingHour.Enabled = false;
            this.cmbBoxBillingHour.FormattingEnabled = true;
            this.cmbBoxBillingHour.Location = new System.Drawing.Point(137, 121);
            this.cmbBoxBillingHour.Name = "cmbBoxBillingHour";
            this.cmbBoxBillingHour.Size = new System.Drawing.Size(121, 21);
            this.cmbBoxBillingHour.TabIndex = 2;
            // 
            // cmbBoxBillingDate
            // 
            this.cmbBoxBillingDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxBillingDate.Enabled = false;
            this.cmbBoxBillingDate.FormattingEnabled = true;
            this.cmbBoxBillingDate.Location = new System.Drawing.Point(137, 85);
            this.cmbBoxBillingDate.Name = "cmbBoxBillingDate";
            this.cmbBoxBillingDate.Size = new System.Drawing.Size(121, 21);
            this.cmbBoxBillingDate.TabIndex = 1;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(20, 160);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(72, 13);
            this.label23.TabIndex = 11;
            this.label23.Text = "Select Minute";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(20, 124);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(63, 13);
            this.label24.TabIndex = 9;
            this.label24.Text = "Select Hour";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(20, 88);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(63, 13);
            this.label25.TabIndex = 10;
            this.label25.Text = "Select Date";
            // 
            // cmbBoxBillingPeriod
            // 
            this.cmbBoxBillingPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxBillingPeriod.FormattingEnabled = true;
            this.cmbBoxBillingPeriod.Items.AddRange(new object[] {
            "End of Month",
            "User Defined"});
            this.cmbBoxBillingPeriod.Location = new System.Drawing.Point(137, 51);
            this.cmbBoxBillingPeriod.Name = "cmbBoxBillingPeriod";
            this.cmbBoxBillingPeriod.Size = new System.Drawing.Size(121, 21);
            this.cmbBoxBillingPeriod.TabIndex = 0;
            this.cmbBoxBillingPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbBoxBillingPeriod_SelectedIndexChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(20, 59);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(100, 13);
            this.label26.TabIndex = 4;
            this.label26.Text = "Select Billing Period";
            // 
            // tabRTC
            // 
            this.tabRTC.Controls.Add(this.rtcCtrl);
            this.tabRTC.Location = new System.Drawing.Point(4, 22);
            this.tabRTC.Name = "tabRTC";
            this.tabRTC.Size = new System.Drawing.Size(692, 424);
            this.tabRTC.TabIndex = 4;
            this.tabRTC.Text = "RTC";
            this.tabRTC.UseVisualStyleBackColor = true;
            // 
            // rtcCtrl
            // 
            this.rtcCtrl.Location = new System.Drawing.Point(91, 15);
            this.rtcCtrl.Margin = new System.Windows.Forms.Padding(4);
            this.rtcCtrl.Name = "rtcCtrl";
            this.rtcCtrl.Size = new System.Drawing.Size(482, 384);
            this.rtcCtrl.TabIndex = 0;
            // 
            // tabTOU
            // 
            this.tabTOU.Controls.Add(this.groupBox4);
            this.tabTOU.Controls.Add(this.tabControl1);
            this.tabTOU.Location = new System.Drawing.Point(4, 22);
            this.tabTOU.Name = "tabTOU";
            this.tabTOU.Size = new System.Drawing.Size(692, 424);
            this.tabTOU.TabIndex = 5;
            this.tabTOU.Text = "TOD Operation";
            this.tabTOU.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdbFutureTOD);
            this.groupBox4.Controls.Add(this.rdbCurrentTOD);
            this.groupBox4.Controls.Add(this.btnResetTOUConfiguration);
            this.groupBox4.Controls.Add(this.btnAutoFillTOUConfiguration);
            this.groupBox4.Controls.Add(this.dtpFutureActivationDate);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.dgvSeasonProfile);
            this.groupBox4.Controls.Add(this.dgvWeekProfile);
            this.groupBox4.Location = new System.Drawing.Point(360, 29);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(314, 379);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // rdbFutureTOD
            // 
            this.rdbFutureTOD.AutoSize = true;
            this.rdbFutureTOD.Checked = true;
            this.rdbFutureTOD.Location = new System.Drawing.Point(195, 176);
            this.rdbFutureTOD.Margin = new System.Windows.Forms.Padding(2);
            this.rdbFutureTOD.Name = "rdbFutureTOD";
            this.rdbFutureTOD.Size = new System.Drawing.Size(81, 17);
            this.rdbFutureTOD.TabIndex = 52;
            this.rdbFutureTOD.TabStop = true;
            this.rdbFutureTOD.Text = "Future TOD";
            this.rdbFutureTOD.UseVisualStyleBackColor = true;
            this.rdbFutureTOD.CheckedChanged += new System.EventHandler(this.rdbFutureTOD_CheckedChanged);
            // 
            // rdbCurrentTOD
            // 
            this.rdbCurrentTOD.AutoSize = true;
            this.rdbCurrentTOD.Location = new System.Drawing.Point(195, 136);
            this.rdbCurrentTOD.Name = "rdbCurrentTOD";
            this.rdbCurrentTOD.Size = new System.Drawing.Size(85, 17);
            this.rdbCurrentTOD.TabIndex = 51;
            this.rdbCurrentTOD.Text = "Current TOD";
            this.rdbCurrentTOD.UseVisualStyleBackColor = true;
            this.rdbCurrentTOD.CheckedChanged += new System.EventHandler(this.rdbCurrentTOD_CheckedChanged);
            // 
            // btnResetTOUConfiguration
            // 
            this.btnResetTOUConfiguration.Location = new System.Drawing.Point(185, 217);
            this.btnResetTOUConfiguration.Name = "btnResetTOUConfiguration";
            this.btnResetTOUConfiguration.Size = new System.Drawing.Size(115, 27);
            this.btnResetTOUConfiguration.TabIndex = 50;
            this.btnResetTOUConfiguration.Text = "Reset All";
            this.btnResetTOUConfiguration.UseVisualStyleBackColor = true;
            this.btnResetTOUConfiguration.Click += new System.EventHandler(this.btnResetTOUConfiguration_Click);
            // 
            // btnAutoFillTOUConfiguration
            // 
            this.btnAutoFillTOUConfiguration.Location = new System.Drawing.Point(185, 263);
            this.btnAutoFillTOUConfiguration.Name = "btnAutoFillTOUConfiguration";
            this.btnAutoFillTOUConfiguration.Size = new System.Drawing.Size(115, 27);
            this.btnAutoFillTOUConfiguration.TabIndex = 49;
            this.btnAutoFillTOUConfiguration.Text = "Auto Fill";
            this.btnAutoFillTOUConfiguration.UseVisualStyleBackColor = true;
            this.btnAutoFillTOUConfiguration.Visible = false;
            this.btnAutoFillTOUConfiguration.Click += new System.EventHandler(this.btnAutoFillTOUConfiguration_Click);
            // 
            // dtpFutureActivationDate
            // 
            this.dtpFutureActivationDate.CustomFormat = "dd/MM/yyyy";
            this.dtpFutureActivationDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFutureActivationDate.Location = new System.Drawing.Point(172, 339);
            this.dtpFutureActivationDate.Name = "dtpFutureActivationDate";
            this.dtpFutureActivationDate.Size = new System.Drawing.Size(110, 20);
            this.dtpFutureActivationDate.TabIndex = 43;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(10, 341);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(142, 18);
            this.label11.TabIndex = 42;
            this.label11.Text = "Future TOU Activation Date";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(48, 161);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(75, 13);
            this.label12.TabIndex = 42;
            this.label12.Text = "Season Profile";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(136, 15);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(68, 13);
            this.label14.TabIndex = 40;
            this.label14.Text = "Week Profile";
            // 
            // dgvSeasonProfile
            // 
            this.dgvSeasonProfile.AllowDrop = true;
            this.dgvSeasonProfile.AllowUserToAddRows = false;
            this.dgvSeasonProfile.AllowUserToDeleteRows = false;
            this.dgvSeasonProfile.AllowUserToResizeColumns = false;
            this.dgvSeasonProfile.AllowUserToResizeRows = false;
            this.dgvSeasonProfile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSeasonProfile.Location = new System.Drawing.Point(13, 176);
            this.dgvSeasonProfile.Margin = new System.Windows.Forms.Padding(2);
            this.dgvSeasonProfile.Name = "dgvSeasonProfile";
            this.dgvSeasonProfile.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dgvSeasonProfile.RowTemplate.Height = 24;
            this.dgvSeasonProfile.Size = new System.Drawing.Size(135, 60);
            this.dgvSeasonProfile.TabIndex = 41;
            this.dgvSeasonProfile.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvSeasonProfile_CellValidating);
            this.dgvSeasonProfile.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSeasonProfile_CellClick);
            // 
            // dgvWeekProfile
            // 
            this.dgvWeekProfile.AllowUserToAddRows = false;
            this.dgvWeekProfile.AllowUserToDeleteRows = false;
            this.dgvWeekProfile.AllowUserToResizeColumns = false;
            this.dgvWeekProfile.AllowUserToResizeRows = false;
            this.dgvWeekProfile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWeekProfile.Location = new System.Drawing.Point(0, 31);
            this.dgvWeekProfile.Name = "dgvWeekProfile";
            this.dgvWeekProfile.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvWeekProfile.RowTemplate.Height = 24;
            this.dgvWeekProfile.Size = new System.Drawing.Size(313, 60);
            this.dgvWeekProfile.TabIndex = 39;
            this.dgvWeekProfile.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvWeekProfile_CellValidating);
            this.dgvWeekProfile.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWeekProfile_CellClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(10, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(336, 380);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(328, 354);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "TOD Detail";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.dgvDayProfile);
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(9, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(320, 320);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(60, 15);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(43, 13);
            this.label21.TabIndex = 4;
            this.label21.Text = "Season";
            // 
            // dgvDayProfile
            // 
            this.dgvDayProfile.AllowUserToAddRows = false;
            this.dgvDayProfile.AllowUserToDeleteRows = false;
            this.dgvDayProfile.AllowUserToResizeColumns = false;
            this.dgvDayProfile.AllowUserToResizeRows = false;
            this.dgvDayProfile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDayProfile.Location = new System.Drawing.Point(6, 31);
            this.dgvDayProfile.Name = "dgvDayProfile";
            this.dgvDayProfile.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvDayProfile.RowTemplate.Height = 24;
            this.dgvDayProfile.Size = new System.Drawing.Size(300, 282);
            this.dgvDayProfile.TabIndex = 3;
            this.dgvDayProfile.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvDayProfile_CellValidating);
            this.dgvDayProfile.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDayProfile_CellClick);
            // 
            // tabDisplayParam
            // 
            this.tabDisplayParam.AllowDrop = true;
            this.tabDisplayParam.Controls.Add(this.tlpDisplayParameter);
            this.tabDisplayParam.Location = new System.Drawing.Point(4, 22);
            this.tabDisplayParam.Name = "tabDisplayParam";
            this.tabDisplayParam.Padding = new System.Windows.Forms.Padding(3);
            this.tabDisplayParam.Size = new System.Drawing.Size(692, 424);
            this.tabDisplayParam.TabIndex = 3;
            this.tabDisplayParam.Text = "Display Parameters";
            this.tabDisplayParam.UseVisualStyleBackColor = true;
            // 
            // tlpDisplayParameter
            // 
            this.tlpDisplayParameter.ColumnCount = 4;
            this.tlpDisplayParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpDisplayParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 531F));
            this.tlpDisplayParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 223F));
            this.tlpDisplayParameter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 224F));
            this.tlpDisplayParameter.Controls.Add(this.tabControlDisplayParams, 1, 1);
            this.tlpDisplayParameter.Controls.Add(this.panel3, 2, 1);
            this.tlpDisplayParameter.Location = new System.Drawing.Point(6, 14);
            this.tlpDisplayParameter.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDisplayParameter.Name = "tlpDisplayParameter";
            this.tlpDisplayParameter.RowCount = 4;
            this.tlpDisplayParameter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tlpDisplayParameter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 395F));
            this.tlpDisplayParameter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tlpDisplayParameter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tlpDisplayParameter.Size = new System.Drawing.Size(654, 410);
            this.tlpDisplayParameter.TabIndex = 2;
            // 
            // tabControlDisplayParams
            // 
            this.tabControlDisplayParams.Controls.Add(this.tabPagePushButton);
            this.tabControlDisplayParams.Controls.Add(this.tabPageScrollButton);
            this.tabControlDisplayParams.Controls.Add(this.tabPageHighResolution);
            this.tabControlDisplayParams.Controls.Add(this.tabPageDisplayTimeOut);
            this.tabControlDisplayParams.Location = new System.Drawing.Point(20, 11);
            this.tabControlDisplayParams.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlDisplayParams.Name = "tabControlDisplayParams";
            this.tlpDisplayParameter.SetRowSpan(this.tabControlDisplayParams, 3);
            this.tabControlDisplayParams.SelectedIndex = 0;
            this.tabControlDisplayParams.Size = new System.Drawing.Size(531, 403);
            this.tabControlDisplayParams.TabIndex = 2;
            this.tabControlDisplayParams.SelectedIndexChanged += new System.EventHandler(this.tabControlDisplayParams_SelectedIndexChanged);
            // 
            // tabPagePushButton
            // 
            this.tabPagePushButton.Controls.Add(this.dGVPushDisplayParams);
            this.tabPagePushButton.Location = new System.Drawing.Point(4, 22);
            this.tabPagePushButton.Name = "tabPagePushButton";
            this.tabPagePushButton.Size = new System.Drawing.Size(523, 377);
            this.tabPagePushButton.TabIndex = 0;
            this.tabPagePushButton.Text = "Push Button";
            this.tabPagePushButton.UseVisualStyleBackColor = true;
            this.tabPagePushButton.Enter += new System.EventHandler(this.tabPagePushButton_Enter);
            // 
            // dGVPushDisplayParams
            // 
            this.dGVPushDisplayParams.AllowUserToAddRows = false;
            this.dGVPushDisplayParams.AllowUserToDeleteRows = false;
            this.dGVPushDisplayParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVPushDisplayParams.Location = new System.Drawing.Point(0, 0);
            this.dGVPushDisplayParams.Margin = new System.Windows.Forms.Padding(0);
            this.dGVPushDisplayParams.MultiSelect = false;
            this.dGVPushDisplayParams.Name = "dGVPushDisplayParams";
            this.dGVPushDisplayParams.RowTemplate.Height = 24;
            this.dGVPushDisplayParams.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dGVPushDisplayParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVPushDisplayParams.Size = new System.Drawing.Size(500, 380);
            this.dGVPushDisplayParams.TabIndex = 0;
            this.dGVPushDisplayParams.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVPushDisplayParams_CellContentClick);
            // 
            // tabPageScrollButton
            // 
            this.tabPageScrollButton.Controls.Add(this.dGVScrollDisplayParams);
            this.tabPageScrollButton.Location = new System.Drawing.Point(4, 22);
            this.tabPageScrollButton.Name = "tabPageScrollButton";
            this.tabPageScrollButton.Size = new System.Drawing.Size(523, 377);
            this.tabPageScrollButton.TabIndex = 1;
            this.tabPageScrollButton.Text = "Scroll Button";
            this.tabPageScrollButton.UseVisualStyleBackColor = true;
            this.tabPageScrollButton.Enter += new System.EventHandler(this.tabPageScrollButton_Enter);
            // 
            // dGVScrollDisplayParams
            // 
            this.dGVScrollDisplayParams.AllowUserToAddRows = false;
            this.dGVScrollDisplayParams.AllowUserToDeleteRows = false;
            this.dGVScrollDisplayParams.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVScrollDisplayParams.Location = new System.Drawing.Point(0, 0);
            this.dGVScrollDisplayParams.Margin = new System.Windows.Forms.Padding(0);
            this.dGVScrollDisplayParams.MultiSelect = false;
            this.dGVScrollDisplayParams.Name = "dGVScrollDisplayParams";
            this.dGVScrollDisplayParams.RowTemplate.Height = 24;
            this.dGVScrollDisplayParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVScrollDisplayParams.Size = new System.Drawing.Size(509, 380);
            this.dGVScrollDisplayParams.TabIndex = 1;
            this.dGVScrollDisplayParams.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVScrollDisplayParams_CellContentClick);
            // 
            // tabPageHighResolution
            // 
            this.tabPageHighResolution.Controls.Add(this.dGVHighResolution);
            this.tabPageHighResolution.Location = new System.Drawing.Point(4, 22);
            this.tabPageHighResolution.Name = "tabPageHighResolution";
            this.tabPageHighResolution.Size = new System.Drawing.Size(523, 377);
            this.tabPageHighResolution.TabIndex = 2;
            this.tabPageHighResolution.Text = "High Resolution";
            this.tabPageHighResolution.UseVisualStyleBackColor = true;
            this.tabPageHighResolution.Enter += new System.EventHandler(this.tabPageHighResolution_Enter);
            // 
            // dGVHighResolution
            // 
            this.dGVHighResolution.AllowUserToAddRows = false;
            this.dGVHighResolution.AllowUserToDeleteRows = false;
            this.dGVHighResolution.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGVHighResolution.Location = new System.Drawing.Point(0, 12);
            this.dGVHighResolution.Margin = new System.Windows.Forms.Padding(0);
            this.dGVHighResolution.MultiSelect = false;
            this.dGVHighResolution.Name = "dGVHighResolution";
            this.dGVHighResolution.RowTemplate.Height = 24;
            this.dGVHighResolution.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGVHighResolution.Size = new System.Drawing.Size(497, 369);
            this.dGVHighResolution.TabIndex = 1;
            this.dGVHighResolution.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGVHighResolution_CellContentClick);
            // 
            // tabPageDisplayTimeOut
            // 
            this.tabPageDisplayTimeOut.Controls.Add(this.groupBox2);
            this.tabPageDisplayTimeOut.Location = new System.Drawing.Point(4, 22);
            this.tabPageDisplayTimeOut.Name = "tabPageDisplayTimeOut";
            this.tabPageDisplayTimeOut.Size = new System.Drawing.Size(523, 377);
            this.tabPageDisplayTimeOut.TabIndex = 3;
            this.tabPageDisplayTimeOut.Text = "Display Timeout";
            this.tabPageDisplayTimeOut.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkAutoScrollTime);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtScrollTime);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtPushButtonTimeout);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtScrollResumeTime);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(3, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(512, 281);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Display Timeouts";
            // 
            // chkAutoScrollTime
            // 
            this.chkAutoScrollTime.AutoSize = true;
            this.chkAutoScrollTime.Location = new System.Drawing.Point(22, 104);
            this.chkAutoScrollTime.Name = "chkAutoScrollTime";
            this.chkAutoScrollTime.Size = new System.Drawing.Size(145, 17);
            this.chkAutoScrollTime.TabIndex = 22;
            this.chkAutoScrollTime.Text = "Auto Scroll Resume Time";
            this.chkAutoScrollTime.UseVisualStyleBackColor = true;
            this.chkAutoScrollTime.CheckedChanged += new System.EventHandler(this.chkAutoScrollTime_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(289, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "* Valid Range (3-300)";
            // 
            // txtScrollTime
            // 
            this.txtScrollTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtScrollTime.Location = new System.Drawing.Point(195, 27);
            this.txtScrollTime.MaxLength = 3;
            this.txtScrollTime.Name = "txtScrollTime";
            this.txtScrollTime.Size = new System.Drawing.Size(75, 20);
            this.txtScrollTime.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(289, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "* Valid Range (1-600)";
            // 
            // txtPushButtonTimeout
            // 
            this.txtPushButtonTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPushButtonTimeout.Location = new System.Drawing.Point(195, 66);
            this.txtPushButtonTimeout.MaxLength = 3;
            this.txtPushButtonTimeout.Name = "txtPushButtonTimeout";
            this.txtPushButtonTimeout.Size = new System.Drawing.Size(75, 20);
            this.txtPushButtonTimeout.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(289, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "* Valid Range (1-300)";
            // 
            // txtScrollResumeTime
            // 
            this.txtScrollResumeTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtScrollResumeTime.Enabled = false;
            this.txtScrollResumeTime.Location = new System.Drawing.Point(195, 104);
            this.txtScrollResumeTime.MaxLength = 3;
            this.txtScrollResumeTime.Name = "txtScrollResumeTime";
            this.txtScrollResumeTime.Size = new System.Drawing.Size(75, 20);
            this.txtScrollResumeTime.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Scroll Time Per Item";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Push Button Timeout";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnUpScroll);
            this.panel3.Controls.Add(this.chkDisplayParamSelectAll);
            this.panel3.Controls.Add(this.btnDownScroll);
            this.panel3.Location = new System.Drawing.Point(554, 14);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(85, 288);
            this.panel3.TabIndex = 17;
            // 
            // btnUpScroll
            // 
            this.btnUpScroll.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpScroll.Location = new System.Drawing.Point(13, 104);
            this.btnUpScroll.Name = "btnUpScroll";
            this.btnUpScroll.Size = new System.Drawing.Size(43, 41);
            this.btnUpScroll.TabIndex = 1;
            this.btnUpScroll.Text = "^";
            this.btnUpScroll.UseVisualStyleBackColor = true;
            this.btnUpScroll.Click += new System.EventHandler(this.btnUpScroll_Click);
            // 
            // chkDisplayParamSelectAll
            // 
            this.chkDisplayParamSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDisplayParamSelectAll.AutoSize = true;
            this.chkDisplayParamSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDisplayParamSelectAll.Location = new System.Drawing.Point(6, 211);
            this.chkDisplayParamSelectAll.Name = "chkDisplayParamSelectAll";
            this.chkDisplayParamSelectAll.Size = new System.Drawing.Size(80, 17);
            this.chkDisplayParamSelectAll.TabIndex = 3;
            this.chkDisplayParamSelectAll.Text = "Select All";
            this.chkDisplayParamSelectAll.UseVisualStyleBackColor = true;
            this.chkDisplayParamSelectAll.CheckedChanged += new System.EventHandler(this.chkDisplayParamSelectAll_CheckedChanged);
            // 
            // btnDownScroll
            // 
            this.btnDownScroll.Location = new System.Drawing.Point(13, 148);
            this.btnDownScroll.Name = "btnDownScroll";
            this.btnDownScroll.Size = new System.Drawing.Size(43, 41);
            this.btnDownScroll.TabIndex = 0;
            this.btnDownScroll.Text = "v";
            this.btnDownScroll.UseVisualStyleBackColor = true;
            this.btnDownScroll.Click += new System.EventHandler(this.btnDownScroll_Click);
            // 
            // tabkvarSelection
            // 
            this.tabkvarSelection.Controls.Add(this.groupBox59);
            this.tabkvarSelection.Location = new System.Drawing.Point(4, 22);
            this.tabkvarSelection.Name = "tabkvarSelection";
            this.tabkvarSelection.Padding = new System.Windows.Forms.Padding(3);
            this.tabkvarSelection.Size = new System.Drawing.Size(692, 424);
            this.tabkvarSelection.TabIndex = 2;
            this.tabkvarSelection.Text = "kvah Selection";
            this.tabkvarSelection.UseVisualStyleBackColor = true;
            // 
            // groupBox59
            // 
            this.groupBox59.Controls.Add(this.rdbKVAhLagLead);
            this.groupBox59.Controls.Add(this.rdbKVAhLagOnly);
            this.groupBox59.Location = new System.Drawing.Point(10, 13);
            this.groupBox59.Name = "groupBox59";
            this.groupBox59.Size = new System.Drawing.Size(310, 109);
            this.groupBox59.TabIndex = 18;
            this.groupBox59.TabStop = false;
            this.groupBox59.Text = "kvah Selection";
            // 
            // rdbKVAhLagLead
            // 
            this.rdbKVAhLagLead.AutoSize = true;
            this.rdbKVAhLagLead.Location = new System.Drawing.Point(161, 46);
            this.rdbKVAhLagLead.Name = "rdbKVAhLagLead";
            this.rdbKVAhLagLead.Size = new System.Drawing.Size(122, 17);
            this.rdbKVAhLagLead.TabIndex = 1;
            this.rdbKVAhLagLead.Text = "Lag + Lead (Unlock)";
            this.rdbKVAhLagLead.UseVisualStyleBackColor = true;
            // 
            // rdbKVAhLagOnly
            // 
            this.rdbKVAhLagOnly.AutoSize = true;
            this.rdbKVAhLagOnly.Location = new System.Drawing.Point(50, 46);
            this.rdbKVAhLagOnly.Name = "rdbKVAhLagOnly";
            this.rdbKVAhLagOnly.Size = new System.Drawing.Size(100, 17);
            this.rdbKVAhLagOnly.TabIndex = 0;
            this.rdbKVAhLagOnly.Text = "Lag Only (Lock)";
            this.rdbKVAhLagOnly.UseVisualStyleBackColor = true;
            // 
            // tabMDWithIP
            // 
            this.tabMDWithIP.Controls.Add(this.grouBoxMDWithIP);
            this.tabMDWithIP.Location = new System.Drawing.Point(4, 22);
            this.tabMDWithIP.Name = "tabMDWithIP";
            this.tabMDWithIP.Padding = new System.Windows.Forms.Padding(3);
            this.tabMDWithIP.Size = new System.Drawing.Size(692, 424);
            this.tabMDWithIP.TabIndex = 1;
            this.tabMDWithIP.Text = "MD IP/LSIP";
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
            this.grouBoxMDWithIP.TabIndex = 11;
            this.grouBoxMDWithIP.TabStop = false;
            this.grouBoxMDWithIP.Text = "MD IP/LSIP";
            // 
            // cmbDemandSubInterlavTime
            // 
            this.cmbDemandSubInterlavTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDemandSubInterlavTime.Enabled = false;
            this.cmbDemandSubInterlavTime.FormattingEnabled = true;
            this.cmbDemandSubInterlavTime.Items.AddRange(new object[] {
            "5",
            "10"});
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
            "30"});
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
            this.tabRS232LockUnlock.Controls.Add(this.tabRS232);
            this.tabRS232LockUnlock.Controls.Add(this.tabPageLSCapturePeriod);
            this.tabRS232LockUnlock.Controls.Add(this.tabPageAutoLock);
            this.tabRS232LockUnlock.Location = new System.Drawing.Point(17, 12);
            this.tabRS232LockUnlock.Name = "tabRS232LockUnlock";
            this.tabRS232LockUnlock.SelectedIndex = 0;
            this.tabRS232LockUnlock.Size = new System.Drawing.Size(700, 450);
            this.tabRS232LockUnlock.TabIndex = 0;
            this.tabRS232LockUnlock.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabRS232LockUnlock_MouseClick);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.pnConfigOptions);
            this.tabMain.Controls.Add(this.txterrorLog);
            this.tabMain.Controls.Add(this.grpCheckBoxes);
            this.tabMain.Location = new System.Drawing.Point(4, 22);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabMain.Size = new System.Drawing.Size(692, 424);
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
            this.pnConfigOptions.Location = new System.Drawing.Point(257, 174);
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
            this.txterrorLog.Location = new System.Drawing.Point(31, 216);
            this.txterrorLog.Name = "txterrorLog";
            this.txterrorLog.ReadOnly = true;
            this.txterrorLog.Size = new System.Drawing.Size(644, 184);
            this.txterrorLog.TabIndex = 4;
            this.txterrorLog.Text = "";
            this.txterrorLog.Visible = false;
            // 
            // grpCheckBoxes
            // 
            this.grpCheckBoxes.Controls.Add(this.chkAutoLock);
            this.grpCheckBoxes.Controls.Add(this.chkLockRS232);
            this.grpCheckBoxes.Controls.Add(this.chkLSCapturePeriod);
            this.grpCheckBoxes.Controls.Add(this.chkBillingReset);
            this.grpCheckBoxes.Controls.Add(this.chkBilingType);
            this.grpCheckBoxes.Controls.Add(this.chkRTC);
            this.grpCheckBoxes.Controls.Add(this.chkSelectAll);
            this.grpCheckBoxes.Controls.Add(this.chkTOD);
            this.grpCheckBoxes.Controls.Add(this.chkDisplayParam);
            this.grpCheckBoxes.Controls.Add(this.chkKVARSelcetion);
            this.grpCheckBoxes.Controls.Add(this.chkMDWithIP);
            this.grpCheckBoxes.Location = new System.Drawing.Point(31, 6);
            this.grpCheckBoxes.Name = "grpCheckBoxes";
            this.grpCheckBoxes.Size = new System.Drawing.Size(644, 162);
            this.grpCheckBoxes.TabIndex = 0;
            this.grpCheckBoxes.TabStop = false;
            // 
            // chkAutoLock
            // 
            this.chkAutoLock.AutoSize = true;
            this.chkAutoLock.Location = new System.Drawing.Point(510, 55);
            this.chkAutoLock.Name = "chkAutoLock";
            this.chkAutoLock.Size = new System.Drawing.Size(75, 17);
            this.chkAutoLock.TabIndex = 10;
            this.chkAutoLock.Text = "Auto Lock";
            this.chkAutoLock.UseVisualStyleBackColor = true;
            this.chkAutoLock.CheckedChanged += new System.EventHandler(this.chkAutoLock_CheckedChanged);
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
            // chkLSCapturePeriod
            // 
            this.chkLSCapturePeriod.AutoSize = true;
            this.chkLSCapturePeriod.Location = new System.Drawing.Point(510, 120);
            this.chkLSCapturePeriod.Name = "chkLSCapturePeriod";
            this.chkLSCapturePeriod.Size = new System.Drawing.Size(112, 17);
            this.chkLSCapturePeriod.TabIndex = 8;
            this.chkLSCapturePeriod.Text = "LS Capture Period";
            this.chkLSCapturePeriod.UseVisualStyleBackColor = true;
            this.chkLSCapturePeriod.Visible = false;
            this.chkLSCapturePeriod.CheckedChanged += new System.EventHandler(this.chkLSCapturePeriod_CheckedChanged);
            // 
            // chkBillingReset
            // 
            this.chkBillingReset.AutoSize = true;
            this.chkBillingReset.Location = new System.Drawing.Point(510, 27);
            this.chkBillingReset.Name = "chkBillingReset";
            this.chkBillingReset.Size = new System.Drawing.Size(84, 17);
            this.chkBillingReset.TabIndex = 7;
            this.chkBillingReset.Text = "Billing Reset";
            this.chkBillingReset.UseVisualStyleBackColor = true;
            this.chkBillingReset.CheckedChanged += new System.EventHandler(this.chkBillingReset_CheckedChanged);
            // 
            // chkBilingType
            // 
            this.chkBilingType.AutoSize = true;
            this.chkBilingType.Location = new System.Drawing.Point(295, 83);
            this.chkBilingType.Name = "chkBilingType";
            this.chkBilingType.Size = new System.Drawing.Size(80, 17);
            this.chkBilingType.TabIndex = 6;
            this.chkBilingType.Text = "Billing Type";
            this.chkBilingType.UseVisualStyleBackColor = true;
            this.chkBilingType.CheckedChanged += new System.EventHandler(this.chkBilingType_CheckedChanged);
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
            this.chkSelectAll.Location = new System.Drawing.Point(28, 129);
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
            // tabRS232
            // 
            this.tabRS232.Controls.Add(this.groupBox14);
            this.tabRS232.Location = new System.Drawing.Point(4, 22);
            this.tabRS232.Name = "tabRS232";
            this.tabRS232.Size = new System.Drawing.Size(692, 424);
            this.tabRS232.TabIndex = 9;
            this.tabRS232.Text = "LockRS232";
            this.tabRS232.UseVisualStyleBackColor = true;
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.rdbRS232Unlock);
            this.groupBox14.Controls.Add(this.rdbRS232Lock);
            this.groupBox14.Location = new System.Drawing.Point(10, 13);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(310, 109);
            this.groupBox14.TabIndex = 21;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "RS232 Lock/Unlock Selection";
            // 
            // rdbRS232Unlock
            // 
            this.rdbRS232Unlock.AutoSize = true;
            this.rdbRS232Unlock.Location = new System.Drawing.Point(161, 46);
            this.rdbRS232Unlock.Name = "rdbRS232Unlock";
            this.rdbRS232Unlock.Size = new System.Drawing.Size(59, 17);
            this.rdbRS232Unlock.TabIndex = 1;
            this.rdbRS232Unlock.Text = "Unlock";
            this.rdbRS232Unlock.UseVisualStyleBackColor = true;
            // 
            // rdbRS232Lock
            // 
            this.rdbRS232Lock.AutoSize = true;
            this.rdbRS232Lock.Location = new System.Drawing.Point(50, 46);
            this.rdbRS232Lock.Name = "rdbRS232Lock";
            this.rdbRS232Lock.Size = new System.Drawing.Size(49, 17);
            this.rdbRS232Lock.TabIndex = 0;
            this.rdbRS232Lock.Text = "Lock";
            this.rdbRS232Lock.UseVisualStyleBackColor = true;
            // 
            // tabPageLSCapturePeriod
            // 
            this.tabPageLSCapturePeriod.Controls.Add(this.groupBox18);
            this.tabPageLSCapturePeriod.Location = new System.Drawing.Point(4, 22);
            this.tabPageLSCapturePeriod.Name = "tabPageLSCapturePeriod";
            this.tabPageLSCapturePeriod.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLSCapturePeriod.Size = new System.Drawing.Size(692, 424);
            this.tabPageLSCapturePeriod.TabIndex = 10;
            this.tabPageLSCapturePeriod.Text = "LS Capture Period";
            this.tabPageLSCapturePeriod.UseVisualStyleBackColor = true;
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.lblSeconds);
            this.groupBox18.Controls.Add(this.lblLoadSurveyCapturePeriod);
            this.groupBox18.Controls.Add(this.cmbBoxLSCapturePeriod);
            this.groupBox18.Location = new System.Drawing.Point(15, 16);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Size = new System.Drawing.Size(417, 164);
            this.groupBox18.TabIndex = 2;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = "Load Survey Capture Period";
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(249, 49);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(55, 13);
            this.lblSeconds.TabIndex = 4;
            this.lblSeconds.Text = "Second(s)";
            // 
            // lblLoadSurveyCapturePeriod
            // 
            this.lblLoadSurveyCapturePeriod.AutoSize = true;
            this.lblLoadSurveyCapturePeriod.Location = new System.Drawing.Point(20, 49);
            this.lblLoadSurveyCapturePeriod.Name = "lblLoadSurveyCapturePeriod";
            this.lblLoadSurveyCapturePeriod.Size = new System.Drawing.Size(143, 13);
            this.lblLoadSurveyCapturePeriod.TabIndex = 3;
            this.lblLoadSurveyCapturePeriod.Text = "Load Survey Capture Period ";
            // 
            // cmbBoxLSCapturePeriod
            // 
            this.cmbBoxLSCapturePeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxLSCapturePeriod.FormattingEnabled = true;
            this.cmbBoxLSCapturePeriod.Items.AddRange(new object[] {
            "900",
            "1800",
            "3600"});
            this.cmbBoxLSCapturePeriod.Location = new System.Drawing.Point(168, 46);
            this.cmbBoxLSCapturePeriod.Name = "cmbBoxLSCapturePeriod";
            this.cmbBoxLSCapturePeriod.Size = new System.Drawing.Size(75, 21);
            this.cmbBoxLSCapturePeriod.TabIndex = 0;
            // 
            // tabPageAutoLock
            // 
            this.tabPageAutoLock.Controls.Add(this.groupBox5);
            this.tabPageAutoLock.Location = new System.Drawing.Point(4, 22);
            this.tabPageAutoLock.Name = "tabPageAutoLock";
            this.tabPageAutoLock.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAutoLock.Size = new System.Drawing.Size(692, 424);
            this.tabPageAutoLock.TabIndex = 11;
            this.tabPageAutoLock.Text = "Auto Lock";
            this.tabPageAutoLock.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rdbAutoUnlock);
            this.groupBox5.Controls.Add(this.rdbAutoLock);
            this.groupBox5.Location = new System.Drawing.Point(10, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(310, 109);
            this.groupBox5.TabIndex = 22;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Auto Billing Lock/Unlock Selection";
            // 
            // rdbAutoUnlock
            // 
            this.rdbAutoUnlock.AutoSize = true;
            this.rdbAutoUnlock.Location = new System.Drawing.Point(161, 46);
            this.rdbAutoUnlock.Name = "rdbAutoUnlock";
            this.rdbAutoUnlock.Size = new System.Drawing.Size(59, 17);
            this.rdbAutoUnlock.TabIndex = 1;
            this.rdbAutoUnlock.Text = "Unlock";
            this.rdbAutoUnlock.UseVisualStyleBackColor = true;
            // 
            // rdbAutoLock
            // 
            this.rdbAutoLock.AutoSize = true;
            this.rdbAutoLock.Location = new System.Drawing.Point(50, 46);
            this.rdbAutoLock.Name = "rdbAutoLock";
            this.rdbAutoLock.Size = new System.Drawing.Size(49, 17);
            this.rdbAutoLock.TabIndex = 0;
            this.rdbAutoLock.Text = "Lock";
            this.rdbAutoLock.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel13
            // 
            this.tableLayoutPanel13.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel13.Name = "tableLayoutPanel13";
            this.tableLayoutPanel13.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel13.TabIndex = 0;
            // 
            // groupBox15
            // 
            this.groupBox15.Controls.Add(this.label13);
            this.groupBox15.Controls.Add(this.label15);
            this.groupBox15.Controls.Add(this.label18);
            this.groupBox15.Controls.Add(this.label20);
            this.groupBox15.Controls.Add(this.label19);
            this.groupBox15.Location = new System.Drawing.Point(3, 16);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(599, 281);
            this.groupBox15.TabIndex = 21;
            this.groupBox15.TabStop = false;
            this.groupBox15.Text = "Display Timeouts";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(289, 105);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "* Valid Range (3-300)";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(289, 69);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(108, 13);
            this.label15.TabIndex = 19;
            this.label15.Text = "* Valid Range (1-600)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(289, 34);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(108, 13);
            this.label18.TabIndex = 20;
            this.label18.Text = "* Valid Range (1-300)";
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(19, 128);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(101, 13);
            this.label20.TabIndex = 11;
            this.label20.Text = "Scroll Time Per Item";
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(19, 160);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(106, 13);
            this.label19.TabIndex = 10;
            this.label19.Text = "Push Button Timeout";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnWriteDisplayParams);
            this.panel2.Controls.Add(this.btnReadDisplayParams);
            this.panel2.Controls.Add(this.chkBoxSelectAll);
            this.panel2.Location = new System.Drawing.Point(688, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(83, 288);
            this.panel2.TabIndex = 17;
            // 
            // btnWriteDisplayParams
            // 
            this.btnWriteDisplayParams.Location = new System.Drawing.Point(3, 262);
            this.btnWriteDisplayParams.Name = "btnWriteDisplayParams";
            this.btnWriteDisplayParams.Size = new System.Drawing.Size(75, 23);
            this.btnWriteDisplayParams.TabIndex = 1;
            this.btnWriteDisplayParams.Text = "Write";
            this.btnWriteDisplayParams.UseVisualStyleBackColor = true;
            // 
            // btnReadDisplayParams
            // 
            this.btnReadDisplayParams.Location = new System.Drawing.Point(3, 232);
            this.btnReadDisplayParams.Name = "btnReadDisplayParams";
            this.btnReadDisplayParams.Size = new System.Drawing.Size(75, 23);
            this.btnReadDisplayParams.TabIndex = 1;
            this.btnReadDisplayParams.Text = "Read";
            this.btnReadDisplayParams.UseVisualStyleBackColor = true;
            // 
            // chkBoxSelectAll
            // 
            this.chkBoxSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBoxSelectAll.AutoSize = true;
            this.chkBoxSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxSelectAll.Location = new System.Drawing.Point(6, 398);
            this.chkBoxSelectAll.Name = "chkBoxSelectAll";
            this.chkBoxSelectAll.Size = new System.Drawing.Size(80, 17);
            this.chkBoxSelectAll.TabIndex = 3;
            this.chkBoxSelectAll.Text = "Select All";
            this.chkBoxSelectAll.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(3, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(599, 281);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Display Timeouts";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(22, 104);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(145, 17);
            this.checkBox1.TabIndex = 22;
            this.checkBox1.Text = "Auto Scroll Resume Time";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(289, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "* Valid Range (3-300)";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox1.Location = new System.Drawing.Point(195, 27);
            this.textBox1.MaxLength = 3;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(75, 20);
            this.textBox1.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "* Valid Range (1-600)";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox2.Location = new System.Drawing.Point(195, 66);
            this.textBox2.MaxLength = 3;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(75, 20);
            this.textBox2.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(289, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "* Valid Range (1-300)";
            // 
            // textBox3
            // 
            this.textBox3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(195, 104);
            this.textBox3.MaxLength = 3;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(75, 20);
            this.textBox3.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Scroll Time Per Item";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Push Button Timeout";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Location = new System.Drawing.Point(688, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(83, 288);
            this.panel1.TabIndex = 17;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(13, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(43, 41);
            this.button1.TabIndex = 1;
            this.button1.Text = "^";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 262);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Write";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(3, 232);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "Read";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.Location = new System.Drawing.Point(6, 210);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(80, 17);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "Select All";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(13, 148);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(43, 41);
            this.button4.TabIndex = 0;
            this.button4.Text = "v";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel16
            // 
            this.tableLayoutPanel16.AutoScroll = true;
            this.tableLayoutPanel16.AutoSize = true;
            this.tableLayoutPanel16.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel16.ColumnCount = 3;
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 504F));
            this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 460F));
            this.tableLayoutPanel16.Controls.Add(this.tabControl2, 0, 1);
            this.tableLayoutPanel16.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel16.Name = "tableLayoutPanel16";
            this.tableLayoutPanel16.RowCount = 3;
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel16.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel16.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tableLayoutPanel16.SetColumnSpan(this.tabControl2, 2);
            this.tabControl2.Controls.Add(this.tabPageSeason1);
            this.tabControl2.Controls.Add(this.tabPageSeason2);
            this.tabControl2.Controls.Add(this.tabPageSeason3);
            this.tabControl2.Controls.Add(this.tabPageSeason4);
            this.tabControl2.Location = new System.Drawing.Point(3, 23);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(568, 14);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPageSeason1
            // 
            this.tabPageSeason1.Controls.Add(this.grpDayTables);
            this.tabPageSeason1.Location = new System.Drawing.Point(4, 22);
            this.tabPageSeason1.Name = "tabPageSeason1";
            this.tabPageSeason1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSeason1.Size = new System.Drawing.Size(560, 0);
            this.tabPageSeason1.TabIndex = 0;
            this.tabPageSeason1.Text = "Season 1";
            this.tabPageSeason1.UseVisualStyleBackColor = true;
            // 
            // grpDayTables
            // 
            this.grpDayTables.Controls.Add(this.lblDayTable6);
            this.grpDayTables.Controls.Add(this.lblDayTable5);
            this.grpDayTables.Controls.Add(this.lblDayTable4);
            this.grpDayTables.Controls.Add(this.lblDayTable3);
            this.grpDayTables.Controls.Add(this.lblDayTable2);
            this.grpDayTables.Controls.Add(this.lblDayTable1);
            this.grpDayTables.ForeColor = System.Drawing.Color.Black;
            this.grpDayTables.Location = new System.Drawing.Point(9, 5);
            this.grpDayTables.Name = "grpDayTables";
            this.grpDayTables.Size = new System.Drawing.Size(542, 407);
            this.grpDayTables.TabIndex = 4;
            this.grpDayTables.TabStop = false;
            // 
            // lblDayTable6
            // 
            this.lblDayTable6.AutoSize = true;
            this.lblDayTable6.Location = new System.Drawing.Point(416, 217);
            this.lblDayTable6.Name = "lblDayTable6";
            this.lblDayTable6.Size = new System.Drawing.Size(65, 13);
            this.lblDayTable6.TabIndex = 7;
            this.lblDayTable6.Text = "Day Table 6";
            // 
            // lblDayTable5
            // 
            this.lblDayTable5.AutoSize = true;
            this.lblDayTable5.Location = new System.Drawing.Point(238, 217);
            this.lblDayTable5.Name = "lblDayTable5";
            this.lblDayTable5.Size = new System.Drawing.Size(65, 13);
            this.lblDayTable5.TabIndex = 7;
            this.lblDayTable5.Text = "Day Table 5";
            // 
            // lblDayTable4
            // 
            this.lblDayTable4.AutoSize = true;
            this.lblDayTable4.Location = new System.Drawing.Point(60, 217);
            this.lblDayTable4.Name = "lblDayTable4";
            this.lblDayTable4.Size = new System.Drawing.Size(65, 13);
            this.lblDayTable4.TabIndex = 7;
            this.lblDayTable4.Text = "Day Table 4";
            // 
            // lblDayTable3
            // 
            this.lblDayTable3.AutoSize = true;
            this.lblDayTable3.Location = new System.Drawing.Point(416, 15);
            this.lblDayTable3.Name = "lblDayTable3";
            this.lblDayTable3.Size = new System.Drawing.Size(65, 13);
            this.lblDayTable3.TabIndex = 6;
            this.lblDayTable3.Text = "Day Table 3";
            // 
            // lblDayTable2
            // 
            this.lblDayTable2.AutoSize = true;
            this.lblDayTable2.Location = new System.Drawing.Point(238, 15);
            this.lblDayTable2.Name = "lblDayTable2";
            this.lblDayTable2.Size = new System.Drawing.Size(65, 13);
            this.lblDayTable2.TabIndex = 5;
            this.lblDayTable2.Text = "Day Table 2";
            // 
            // lblDayTable1
            // 
            this.lblDayTable1.AutoSize = true;
            this.lblDayTable1.Location = new System.Drawing.Point(60, 15);
            this.lblDayTable1.Name = "lblDayTable1";
            this.lblDayTable1.Size = new System.Drawing.Size(65, 13);
            this.lblDayTable1.TabIndex = 4;
            this.lblDayTable1.Text = "Day Table 1";
            // 
            // tabPageSeason2
            // 
            this.tabPageSeason2.Controls.Add(this.groupBox26);
            this.tabPageSeason2.Location = new System.Drawing.Point(4, 22);
            this.tabPageSeason2.Name = "tabPageSeason2";
            this.tabPageSeason2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSeason2.Size = new System.Drawing.Size(560, 0);
            this.tabPageSeason2.TabIndex = 1;
            this.tabPageSeason2.Text = "Season 2";
            this.tabPageSeason2.UseVisualStyleBackColor = true;
            // 
            // groupBox26
            // 
            this.groupBox26.Controls.Add(this.label51);
            this.groupBox26.Controls.Add(this.label52);
            this.groupBox26.Controls.Add(this.label53);
            this.groupBox26.Controls.Add(this.label54);
            this.groupBox26.Controls.Add(this.label55);
            this.groupBox26.Controls.Add(this.label56);
            this.groupBox26.ForeColor = System.Drawing.Color.Black;
            this.groupBox26.Location = new System.Drawing.Point(9, 5);
            this.groupBox26.Name = "groupBox26";
            this.groupBox26.Size = new System.Drawing.Size(545, 407);
            this.groupBox26.TabIndex = 5;
            this.groupBox26.TabStop = false;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(416, 217);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(65, 13);
            this.label51.TabIndex = 7;
            this.label51.Text = "Day Table 6";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(238, 217);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(65, 13);
            this.label52.TabIndex = 7;
            this.label52.Text = "Day Table 5";
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Location = new System.Drawing.Point(60, 217);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(65, 13);
            this.label53.TabIndex = 7;
            this.label53.Text = "Day Table 4";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(416, 15);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(65, 13);
            this.label54.TabIndex = 6;
            this.label54.Text = "Day Table 3";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Location = new System.Drawing.Point(238, 15);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(65, 13);
            this.label55.TabIndex = 5;
            this.label55.Text = "Day Table 2";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(60, 15);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(65, 13);
            this.label56.TabIndex = 4;
            this.label56.Text = "Day Table 1";
            // 
            // tabPageSeason3
            // 
            this.tabPageSeason3.Controls.Add(this.groupBox27);
            this.tabPageSeason3.Location = new System.Drawing.Point(4, 22);
            this.tabPageSeason3.Name = "tabPageSeason3";
            this.tabPageSeason3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSeason3.Size = new System.Drawing.Size(560, 0);
            this.tabPageSeason3.TabIndex = 2;
            this.tabPageSeason3.Text = "Season 3";
            this.tabPageSeason3.UseVisualStyleBackColor = true;
            // 
            // groupBox27
            // 
            this.groupBox27.Controls.Add(this.label57);
            this.groupBox27.Controls.Add(this.label58);
            this.groupBox27.Controls.Add(this.label59);
            this.groupBox27.Controls.Add(this.label60);
            this.groupBox27.Controls.Add(this.label61);
            this.groupBox27.Controls.Add(this.label62);
            this.groupBox27.ForeColor = System.Drawing.Color.Black;
            this.groupBox27.Location = new System.Drawing.Point(9, 5);
            this.groupBox27.Name = "groupBox27";
            this.groupBox27.Size = new System.Drawing.Size(548, 407);
            this.groupBox27.TabIndex = 5;
            this.groupBox27.TabStop = false;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Location = new System.Drawing.Point(416, 217);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(65, 13);
            this.label57.TabIndex = 7;
            this.label57.Text = "Day Table 6";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(238, 217);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(65, 13);
            this.label58.TabIndex = 7;
            this.label58.Text = "Day Table 5";
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.Location = new System.Drawing.Point(60, 217);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(65, 13);
            this.label59.TabIndex = 7;
            this.label59.Text = "Day Table 4";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(416, 15);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(65, 13);
            this.label60.TabIndex = 6;
            this.label60.Text = "Day Table 3";
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Location = new System.Drawing.Point(238, 15);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(65, 13);
            this.label61.TabIndex = 5;
            this.label61.Text = "Day Table 2";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(60, 15);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(0, 13);
            this.label62.TabIndex = 4;
            // 
            // tabPageSeason4
            // 
            this.tabPageSeason4.Controls.Add(this.groupBox28);
            this.tabPageSeason4.Location = new System.Drawing.Point(4, 22);
            this.tabPageSeason4.Name = "tabPageSeason4";
            this.tabPageSeason4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSeason4.Size = new System.Drawing.Size(560, 0);
            this.tabPageSeason4.TabIndex = 3;
            this.tabPageSeason4.Text = "Season 4";
            this.tabPageSeason4.UseVisualStyleBackColor = true;
            // 
            // groupBox28
            // 
            this.groupBox28.Controls.Add(this.label63);
            this.groupBox28.Controls.Add(this.label64);
            this.groupBox28.Controls.Add(this.label65);
            this.groupBox28.Controls.Add(this.label66);
            this.groupBox28.Controls.Add(this.label67);
            this.groupBox28.Controls.Add(this.label68);
            this.groupBox28.ForeColor = System.Drawing.Color.Black;
            this.groupBox28.Location = new System.Drawing.Point(9, 5);
            this.groupBox28.Name = "groupBox28";
            this.groupBox28.Size = new System.Drawing.Size(545, 407);
            this.groupBox28.TabIndex = 5;
            this.groupBox28.TabStop = false;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(416, 217);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(65, 13);
            this.label63.TabIndex = 7;
            this.label63.Text = "Day Table 6";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(238, 217);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(65, 13);
            this.label64.TabIndex = 7;
            this.label64.Text = "Day Table 5";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(60, 217);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(65, 13);
            this.label65.TabIndex = 7;
            this.label65.Text = "Day Table 4";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(416, 15);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(65, 13);
            this.label66.TabIndex = 6;
            this.label66.Text = "Day Table 3";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(238, 15);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(65, 13);
            this.label67.TabIndex = 5;
            this.label67.Text = "Day Table 2";
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(60, 15);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(65, 13);
            this.label68.TabIndex = 4;
            this.label68.Text = "Day Table 1";
            // 
            // groupBox25
            // 
            this.groupBox25.Controls.Add(this.label191);
            this.groupBox25.Controls.Add(this.lblActivation);
            this.groupBox25.Controls.Add(this.lblDayTable);
            this.groupBox25.Location = new System.Drawing.Point(577, 26);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(314, 443);
            this.groupBox25.TabIndex = 1;
            this.groupBox25.TabStop = false;
            // 
            // label191
            // 
            this.label191.Location = new System.Drawing.Point(10, 380);
            this.label191.Name = "label191";
            this.label191.Size = new System.Drawing.Size(142, 18);
            this.label191.TabIndex = 42;
            this.label191.Text = "Future TOU Activation Date";
            // 
            // lblActivation
            // 
            this.lblActivation.AutoSize = true;
            this.lblActivation.Location = new System.Drawing.Point(44, 196);
            this.lblActivation.Name = "lblActivation";
            this.lblActivation.Size = new System.Drawing.Size(75, 13);
            this.lblActivation.TabIndex = 42;
            this.lblActivation.Text = "Season Profile";
            // 
            // lblDayTable
            // 
            this.lblDayTable.AutoSize = true;
            this.lblDayTable.Location = new System.Drawing.Point(136, 15);
            this.lblDayTable.Name = "lblDayTable";
            this.lblDayTable.Size = new System.Drawing.Size(68, 13);
            this.lblDayTable.TabIndex = 40;
            this.lblDayTable.Text = "Week Profile";
            // 
            // E650MeterConfigurations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(720, 482);
            this.Controls.Add(this.tabRS232LockUnlock);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "E650MeterConfigurations";
            this.StatusMessage = "";
            this.Text = "LT Meter Configurations";
            this.Load += new System.EventHandler(this.E650MeterConfigurations_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.E650MeterConfigurations_FormClosing);
            this.tabReset.ResumeLayout(false);
            this.groupBoxMDReset.ResumeLayout(false);
            this.groupBoxMDReset.PerformLayout();
            this.tabBillingReset.ResumeLayout(false);
            this.gbManual.ResumeLayout(false);
            this.gbManual.PerformLayout();
            this.groupBoxBillingTYpe.ResumeLayout(false);
            this.groupBoxBillingTYpe.PerformLayout();
            this.tabRTC.ResumeLayout(false);
            this.tabTOU.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeasonProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWeekProfile)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDayProfile)).EndInit();
            this.tabDisplayParam.ResumeLayout(false);
            this.tlpDisplayParameter.ResumeLayout(false);
            this.tabControlDisplayParams.ResumeLayout(false);
            this.tabPagePushButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVPushDisplayParams)).EndInit();
            this.tabPageScrollButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVScrollDisplayParams)).EndInit();
            this.tabPageHighResolution.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGVHighResolution)).EndInit();
            this.tabPageDisplayTimeOut.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabkvarSelection.ResumeLayout(false);
            this.groupBox59.ResumeLayout(false);
            this.groupBox59.PerformLayout();
            this.tabMDWithIP.ResumeLayout(false);
            this.grouBoxMDWithIP.ResumeLayout(false);
            this.grouBoxMDWithIP.PerformLayout();
            this.tabRS232LockUnlock.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.pnConfigOptions.ResumeLayout(false);
            this.grpCheckBoxes.ResumeLayout(false);
            this.grpCheckBoxes.PerformLayout();
            this.tabRS232.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.tabPageLSCapturePeriod.ResumeLayout(false);
            this.groupBox18.ResumeLayout(false);
            this.groupBox18.PerformLayout();
            this.tabPageAutoLock.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox15.ResumeLayout(false);
            this.groupBox15.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel16.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPageSeason1.ResumeLayout(false);
            this.grpDayTables.ResumeLayout(false);
            this.grpDayTables.PerformLayout();
            this.tabPageSeason2.ResumeLayout(false);
            this.groupBox26.ResumeLayout(false);
            this.groupBox26.PerformLayout();
            this.tabPageSeason3.ResumeLayout(false);
            this.groupBox27.ResumeLayout(false);
            this.groupBox27.PerformLayout();
            this.tabPageSeason4.ResumeLayout(false);
            this.groupBox28.ResumeLayout(false);
            this.groupBox28.PerformLayout();
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer_RTC;
        private CABAppControl.Reset billingResetCtrl;
        private System.Windows.Forms.TabPage tabReset;
        private System.Windows.Forms.TabPage tabBillingReset;
        private System.Windows.Forms.TabPage tabRTC;
        private CABAppControl.RTC rtcCtrl;
        private System.Windows.Forms.TabPage tabTOU;
        private System.Windows.Forms.TabPage tabDisplayParam;
        private CAB.UI.Controls.DisplayParamaters displayParameters;
        private System.Windows.Forms.TabPage tabkvarSelection;
        private System.Windows.Forms.TabPage tabMDWithIP;
        private System.Windows.Forms.TabControl tabRS232LockUnlock;
        private System.Windows.Forms.TabPage tabRS232;
        private System.Windows.Forms.GroupBox groupBoxBillingTYpe;
        private System.Windows.Forms.ComboBox cmbBoxBillingMinute;
        private System.Windows.Forms.ComboBox cmbBoxBillingHour;
        private System.Windows.Forms.ComboBox cmbBoxBillingDate;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.ComboBox cmbBoxBillingPeriod;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.RadioButton rdbRS232Unlock;
        private System.Windows.Forms.RadioButton rdbRS232Lock;
        private System.Windows.Forms.TabPage tabPageLSCapturePeriod;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.Label lblSeconds;
        private System.Windows.Forms.Label lblLoadSurveyCapturePeriod;
        private System.Windows.Forms.ComboBox cmbBoxLSCapturePeriod;
        private System.Windows.Forms.GroupBox groupBox59;
        private System.Windows.Forms.RadioButton rdbKVAhLagLead;
        private System.Windows.Forms.RadioButton rdbKVAhLagOnly;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;             
        private System.Windows.Forms.GroupBox groupBox15;       
        private System.Windows.Forms.Label label13;       
        private System.Windows.Forms.Label label15;        
        private System.Windows.Forms.Label label18;        
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel panel2;        
        private System.Windows.Forms.Button btnWriteDisplayParams;
        private System.Windows.Forms.Button btnReadDisplayParams;
        private System.Windows.Forms.CheckBox chkBoxSelectAll;       
        private System.Windows.Forms.TabPage tabMain;
        private System.Windows.Forms.Panel pnConfigOptions;
        private System.Windows.Forms.Button btnUploadFile;
        private System.Windows.Forms.Button btnCreateCfgFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.RichTextBox txterrorLog;
        private System.Windows.Forms.GroupBox grpCheckBoxes;
        private System.Windows.Forms.CheckBox chkLockRS232;
        private System.Windows.Forms.CheckBox chkLSCapturePeriod;
        private System.Windows.Forms.CheckBox chkBillingReset;
        private System.Windows.Forms.CheckBox chkBilingType;
        private System.Windows.Forms.CheckBox chkRTC;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.CheckBox chkTOD;
        private System.Windows.Forms.CheckBox chkDisplayParam;
        private System.Windows.Forms.CheckBox chkKVARSelcetion;
        private System.Windows.Forms.CheckBox chkMDWithIP;                 
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TableLayoutPanel tlpDisplayParameter;
        private System.Windows.Forms.TabControl tabControlDisplayParams;
        private System.Windows.Forms.TabPage tabPagePushButton;
        private System.Windows.Forms.DataGridView dGVPushDisplayParams;
        private System.Windows.Forms.TabPage tabPageScrollButton;
        private System.Windows.Forms.DataGridView dGVScrollDisplayParams;
        private System.Windows.Forms.TabPage tabPageHighResolution;
        private System.Windows.Forms.DataGridView dGVHighResolution;
        private System.Windows.Forms.TabPage tabPageDisplayTimeOut;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkAutoScrollTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtScrollTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPushButtonTimeout;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtScrollResumeTime;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnUpScroll;
        private System.Windows.Forms.CheckBox chkDisplayParamSelectAll;
        private System.Windows.Forms.Button btnDownScroll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageSeason1;
        private System.Windows.Forms.GroupBox grpDayTables;
        
        private System.Windows.Forms.Label lblDayTable6;
        private System.Windows.Forms.Label lblDayTable5;
        private System.Windows.Forms.Label lblDayTable4;
        private System.Windows.Forms.Label lblDayTable3;
        private System.Windows.Forms.Label lblDayTable2;
        private System.Windows.Forms.Label lblDayTable1;
        
        private System.Windows.Forms.TabPage tabPageSeason2;
        private System.Windows.Forms.GroupBox groupBox26;
       
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label56;
        
        private System.Windows.Forms.TabPage tabPageSeason3;
        private System.Windows.Forms.GroupBox groupBox27;
        
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label62;
        
        private System.Windows.Forms.TabPage tabPageSeason4;
        private System.Windows.Forms.GroupBox groupBox28;
       
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label68;
       
        private System.Windows.Forms.GroupBox groupBox25;             
        private System.Windows.Forms.Label label191;
        private System.Windows.Forms.Label lblActivation;
        private System.Windows.Forms.Label lblDayTable;                   
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnResetTOUConfiguration;
        private System.Windows.Forms.Button btnAutoFillTOUConfiguration;
        private System.Windows.Forms.DateTimePicker dtpFutureActivationDate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView dgvSeasonProfile;
        private System.Windows.Forms.DataGridView dgvWeekProfile;
        private System.Windows.Forms.RadioButton rdbFutureTOD;
        private System.Windows.Forms.RadioButton rdbCurrentTOD;
        private System.Windows.Forms.GroupBox groupBoxMDReset;
        private System.Windows.Forms.CheckBox chkMDReset;
        private System.Windows.Forms.GroupBox grouBoxMDWithIP;
        private System.Windows.Forms.ComboBox cmbDemandType;
        private System.Windows.Forms.Label lblDemandType;
        private System.Windows.Forms.ComboBox cmbDemandInterval;
        private System.Windows.Forms.Label lblTimeInterval;
        private System.Windows.Forms.ComboBox cmbDemandSubInterlavTime;
        private System.Windows.Forms.Label lblDemandSubIntervalTime;
        private System.Windows.Forms.GroupBox gbManual;
        private System.Windows.Forms.ComboBox cmbResetLockoutdays;
        private System.Windows.Forms.Label lblResetLockOutDays;
        private System.Windows.Forms.TabPage tabPageAutoLock;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rdbAutoUnlock;
        private System.Windows.Forms.RadioButton rdbAutoLock;
        private System.Windows.Forms.CheckBox chkAutoLock;
        private System.Windows.Forms.DataGridView dgvDayProfile;

    }
}

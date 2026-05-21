namespace CAB.UI
{
    partial class GeneralConfiguration
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
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControlSystemConfiguration = new CAB.UI.Controls.PremiumTabControl();
            this.tbGeneral = new System.Windows.Forms.TabPage();
            this.lblPowerOnOfDurationFormat = new System.Windows.Forms.Label();
            this.ChkPowerOnOffDurationFormat = new System.Windows.Forms.CheckBox();
            this.chkBoxHideNameplate = new System.Windows.Forms.CheckBox();
            this.chkBoxNumPowFail = new System.Windows.Forms.CheckBox();
            this.grpRetries = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbNoOfRetries = new System.Windows.Forms.ComboBox();
            this.grpLoadSurvey = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbLoadSurveyDays = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboDate = new System.Windows.Forms.ComboBox();
            this.COMPortSet_lblCOMPort = new CAB.UI.Controls.CABLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtDefaultCABLocation = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.rbtnDefault = new System.Windows.Forms.RadioButton();
            this.rbtnCustom = new System.Windows.Forms.RadioButton();
            this.txtCustomCABLocation = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtDNWM = new System.Windows.Forms.RadioButton();
            this.rboFNC3 = new System.Windows.Forms.RadioButton();
            this.rboFNC2 = new System.Windows.Forms.RadioButton();
            this.rboFNC1 = new System.Windows.Forms.RadioButton();
            this.tbDashBoard = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.chklstDashBoard = new System.Windows.Forms.CheckedListBox();
            this.groupBox1.SuspendLayout();
            this.tabControlSystemConfiguration.SuspendLayout();
            this.tbGeneral.SuspendLayout();
            this.grpRetries.SuspendLayout();
            this.grpLoadSurvey.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tbDashBoard.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            this.lngbSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lngbSave.FlatAppearance.BorderSize = 0;
            this.lngbSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lngbSave.ForeColor = System.Drawing.Color.White;
            this.lngbSave.Location = new System.Drawing.Point(327, 420);
            this.lngbSave.Name = "lngbSave";
            this.lngbSave.Size = new System.Drawing.Size(75, 30);
            this.lngbSave.TabIndex = 4;
            this.lngbSave.Text = "Save";
            this.lngbSave.TranslationKey = null;
            this.lngbSave.UseVisualStyleBackColor = false;
            this.lngbSave.Click += new System.EventHandler(this.lngbSave_Click);
            // 
            this.lngbCancel.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lngbCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lngbCancel.FlatAppearance.BorderSize = 0;
            this.lngbCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lngbCancel.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lngbCancel.Location = new System.Drawing.Point(408, 420);
            this.lngbCancel.Name = "lngbCancel";
            this.lngbCancel.Size = new System.Drawing.Size(75, 30);
            this.lngbCancel.TabIndex = 5;
            this.lngbCancel.Text = "Cancel";
            this.lngbCancel.TranslationKey = null;
            this.lngbCancel.UseVisualStyleBackColor = false;
            this.lngbCancel.Click += new System.EventHandler(this.lngbCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.tabControlSystemConfiguration);
            this.groupBox1.Controls.Add(this.lngbCancel);
            this.groupBox1.Controls.Add(this.lngbSave);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(490, 460);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // tabControlSystemConfiguration
            // 
            this.tabControlSystemConfiguration.Controls.Add(this.tbGeneral);
            this.tabControlSystemConfiguration.Controls.Add(this.tbDashBoard);
            this.tabControlSystemConfiguration.Location = new System.Drawing.Point(6, 19);
            this.tabControlSystemConfiguration.Name = "tabControlSystemConfiguration";
            this.tabControlSystemConfiguration.SelectedIndex = 0;
            this.tabControlSystemConfiguration.Size = new System.Drawing.Size(459, 400);
            this.tabControlSystemConfiguration.TabIndex = 6;
            // 
            // tbGeneral
            // 
            this.tbGeneral.BackColor = System.Drawing.Color.White;
            this.tbGeneral.Controls.Add(this.lblPowerOnOfDurationFormat);
            this.tbGeneral.Controls.Add(this.ChkPowerOnOffDurationFormat);
            this.tbGeneral.Controls.Add(this.chkBoxHideNameplate);
            this.tbGeneral.Controls.Add(this.chkBoxNumPowFail);
            this.tbGeneral.Controls.Add(this.grpRetries);
            this.tbGeneral.Controls.Add(this.grpLoadSurvey);
            this.tbGeneral.Controls.Add(this.groupBox4);
            this.tbGeneral.Controls.Add(this.groupBox3);
            this.tbGeneral.Controls.Add(this.groupBox2);
            this.tbGeneral.Location = new System.Drawing.Point(4, 24);
            this.tbGeneral.Name = "tbGeneral";
            this.tbGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tbGeneral.Size = new System.Drawing.Size(451, 372);
            this.tbGeneral.TabIndex = 0;
            this.tbGeneral.Text = "General";
            // 
            // lblPowerOnOfDurationFormat
            // 
            this.lblPowerOnOfDurationFormat.AutoSize = true;
            this.lblPowerOnOfDurationFormat.ForeColor = System.Drawing.Color.Red;
            this.lblPowerOnOfDurationFormat.Location = new System.Drawing.Point(225, 302);
            this.lblPowerOnOfDurationFormat.Name = "lblPowerOnOfDurationFormat";
            this.lblPowerOnOfDurationFormat.Size = new System.Drawing.Size(138, 13);
            this.lblPowerOnOfDurationFormat.TabIndex = 36;
            this.lblPowerOnOfDurationFormat.Text = "Default format is (dd:hh:mm)";
            // 
            // ChkPowerOnOffDurationFormat
            // 
            this.ChkPowerOnOffDurationFormat.AutoSize = true;
            this.ChkPowerOnOffDurationFormat.Location = new System.Drawing.Point(9, 301);
            this.ChkPowerOnOffDurationFormat.Name = "ChkPowerOnOffDurationFormat";
            this.ChkPowerOnOffDurationFormat.Size = new System.Drawing.Size(218, 17);
            this.ChkPowerOnOffDurationFormat.TabIndex = 35;
            this.ChkPowerOnOffDurationFormat.Text = "Power On/Off Duration Format (dddd:hh)";
            this.ChkPowerOnOffDurationFormat.UseVisualStyleBackColor = true;
            // 
            // chkBoxHideNameplate
            // 
            this.chkBoxHideNameplate.AutoSize = true;
            this.chkBoxHideNameplate.Location = new System.Drawing.Point(9, 283);
            this.chkBoxHideNameplate.Name = "chkBoxHideNameplate";
            this.chkBoxHideNameplate.Size = new System.Drawing.Size(138, 17);
            this.chkBoxHideNameplate.TabIndex = 34;
            this.chkBoxHideNameplate.Text = "Hide NamePlate Details";
            this.chkBoxHideNameplate.UseVisualStyleBackColor = true;
            // 
            // chkBoxNumPowFail
            // 
            this.chkBoxNumPowFail.AutoSize = true;
            this.chkBoxNumPowFail.Location = new System.Drawing.Point(9, 265);
            this.chkBoxNumPowFail.Name = "chkBoxNumPowFail";
            this.chkBoxNumPowFail.Size = new System.Drawing.Size(172, 17);
            this.chkBoxNumPowFail.TabIndex = 33;
            this.chkBoxNumPowFail.Text = "Hide Number of Power-Failures";
            this.chkBoxNumPowFail.UseVisualStyleBackColor = true;
            // 
            // grpRetries
            // 
            this.grpRetries.BackColor = System.Drawing.Color.Transparent;
            this.grpRetries.Controls.Add(this.label1);
            this.grpRetries.Controls.Add(this.cmbNoOfRetries);
            this.grpRetries.Location = new System.Drawing.Point(225, 202);
            this.grpRetries.Margin = new System.Windows.Forms.Padding(2);
            this.grpRetries.Name = "grpRetries";
            this.grpRetries.Size = new System.Drawing.Size(205, 57);
            this.grpRetries.TabIndex = 32;
            this.grpRetries.TabStop = false;
            this.grpRetries.Text = "Retries";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "No. of Retries";
            // 
            // cmbNoOfRetries
            // 
            this.cmbNoOfRetries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNoOfRetries.FormatString = "N2";
            this.cmbNoOfRetries.FormattingEnabled = true;
            this.cmbNoOfRetries.Location = new System.Drawing.Point(115, 24);
            this.cmbNoOfRetries.MaxLength = 2;
            this.cmbNoOfRetries.Name = "cmbNoOfRetries";
            this.cmbNoOfRetries.Size = new System.Drawing.Size(47, 21);
            this.cmbNoOfRetries.TabIndex = 8;
            // 
            // grpLoadSurvey
            // 
            this.grpLoadSurvey.BackColor = System.Drawing.Color.Transparent;
            this.grpLoadSurvey.Controls.Add(this.label5);
            this.grpLoadSurvey.Controls.Add(this.cmbLoadSurveyDays);
            this.grpLoadSurvey.Location = new System.Drawing.Point(5, 202);
            this.grpLoadSurvey.Margin = new System.Windows.Forms.Padding(2);
            this.grpLoadSurvey.Name = "grpLoadSurvey";
            this.grpLoadSurvey.Size = new System.Drawing.Size(198, 57);
            this.grpLoadSurvey.TabIndex = 19;
            this.grpLoadSurvey.TabStop = false;
            this.grpLoadSurvey.Text = "Load Survey";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "No. of Days";
            // 
            // cmbLoadSurveyDays
            // 
            this.cmbLoadSurveyDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoadSurveyDays.FormatString = "N2";
            this.cmbLoadSurveyDays.FormattingEnabled = true;
            this.cmbLoadSurveyDays.Location = new System.Drawing.Point(108, 24);
            this.cmbLoadSurveyDays.MaxLength = 2;
            this.cmbLoadSurveyDays.Name = "cmbLoadSurveyDays";
            this.cmbLoadSurveyDays.Size = new System.Drawing.Size(47, 21);
            this.cmbLoadSurveyDays.TabIndex = 8;
            // 
            this.groupBox4.Controls.Add(this.cboDate);
            this.groupBox4.Controls.Add(this.COMPortSet_lblCOMPort);
            this.groupBox4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox4.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.groupBox4.Location = new System.Drawing.Point(5, 323);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(425, 43);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Application Date Format";
            this.groupBox4.Visible = false;
            // 
            // cboDate
            // 
            this.cboDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDate.FormattingEnabled = true;
            this.cboDate.Location = new System.Drawing.Point(131, 18);
            this.cboDate.Name = "cboDate";
            this.cboDate.Size = new System.Drawing.Size(121, 21);
            this.cboDate.TabIndex = 1;
            // 
            // COMPortSet_lblCOMPort
            // 
            this.COMPortSet_lblCOMPort.AutoSize = true;
            this.COMPortSet_lblCOMPort.Location = new System.Drawing.Point(55, 21);
            this.COMPortSet_lblCOMPort.Name = "COMPortSet_lblCOMPort";
            this.COMPortSet_lblCOMPort.Size = new System.Drawing.Size(65, 13);
            this.COMPortSet_lblCOMPort.TabIndex = 0;
            this.COMPortSet_lblCOMPort.Text = "Date Format";
            this.COMPortSet_lblCOMPort.TranslationKey = null;
            // 
            this.groupBox3.Controls.Add(this.txtDefaultCABLocation);
            this.groupBox3.Controls.Add(this.btnBrowse);
            this.groupBox3.Controls.Add(this.rbtnDefault);
            this.groupBox3.Controls.Add(this.rbtnCustom);
            this.groupBox3.Controls.Add(this.txtCustomCABLocation);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.groupBox3.Location = new System.Drawing.Point(9, 5);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(425, 78);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "File Location";
            // 
            // txtDefaultCABLocation
            // 
            this.txtDefaultCABLocation.BackColor = System.Drawing.Color.White;
            this.txtDefaultCABLocation.Location = new System.Drawing.Point(123, 18);
            this.txtDefaultCABLocation.Name = "txtDefaultCABLocation";
            this.txtDefaultCABLocation.ReadOnly = true;
            this.txtDefaultCABLocation.Size = new System.Drawing.Size(240, 20);
            this.txtDefaultCABLocation.TabIndex = 11;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(309, 41);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(56, 24);
            this.btnBrowse.TabIndex = 10;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnBrowse.ForeColor = System.Drawing.Color.White;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.FlatAppearance.BorderSize = 0;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // rbtnDefault
            // 
            this.rbtnDefault.AutoSize = true;
            this.rbtnDefault.Checked = true;
            this.rbtnDefault.Location = new System.Drawing.Point(53, 19);
            this.rbtnDefault.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnDefault.Name = "rbtnDefault";
            this.rbtnDefault.Size = new System.Drawing.Size(59, 17);
            this.rbtnDefault.TabIndex = 7;
            this.rbtnDefault.TabStop = true;
            this.rbtnDefault.Text = "Default";
            this.rbtnDefault.UseVisualStyleBackColor = true;
            this.rbtnDefault.CheckedChanged += new System.EventHandler(this.rbtnDefault_CheckedChanged);
            // 
            // rbtnCustom
            // 
            this.rbtnCustom.AutoSize = true;
            this.rbtnCustom.Location = new System.Drawing.Point(53, 43);
            this.rbtnCustom.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnCustom.Name = "rbtnCustom";
            this.rbtnCustom.Size = new System.Drawing.Size(60, 17);
            this.rbtnCustom.TabIndex = 8;
            this.rbtnCustom.Text = "Custom";
            this.rbtnCustom.UseVisualStyleBackColor = true;
            this.rbtnCustom.CheckedChanged += new System.EventHandler(this.rbtnCustom_CheckedChanged);
            // 
            // txtCustomCABLocation
            // 
            this.txtCustomCABLocation.BackColor = System.Drawing.Color.White;
            this.txtCustomCABLocation.Location = new System.Drawing.Point(123, 44);
            this.txtCustomCABLocation.Margin = new System.Windows.Forms.Padding(2);
            this.txtCustomCABLocation.Name = "txtCustomCABLocation";
            this.txtCustomCABLocation.ReadOnly = true;
            this.txtCustomCABLocation.Size = new System.Drawing.Size(182, 20);
            this.txtCustomCABLocation.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtDNWM);
            this.groupBox2.Controls.Add(this.rboFNC3);
            this.groupBox2.Controls.Add(this.rboFNC2);
            this.groupBox2.Controls.Add(this.rboFNC1);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.groupBox2.Location = new System.Drawing.Point(5, 87);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(425, 104);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Format";
            // 
            // rbtDNWM
            // 
            this.rbtDNWM.AutoSize = true;
            this.rbtDNWM.Checked = true;
            this.rbtDNWM.Location = new System.Drawing.Point(58, 16);
            this.rbtDNWM.Margin = new System.Windows.Forms.Padding(2);
            this.rbtDNWM.Name = "rbtDNWM";
            this.rbtDNWM.Size = new System.Drawing.Size(249, 17);
            this.rbtDNWM.TabIndex = 11;
            this.rbtDNWM.TabStop = true;
            this.rbtDNWM.Text = "System Generated Default Name With Meter ID";
            this.rbtDNWM.UseVisualStyleBackColor = true;
            // 
            // rboFNC3
            // 
            this.rboFNC3.AutoSize = true;
            this.rboFNC3.Location = new System.Drawing.Point(58, 81);
            this.rboFNC3.Margin = new System.Windows.Forms.Padding(2);
            this.rboFNC3.Name = "rboFNC3";
            this.rboFNC3.Size = new System.Drawing.Size(131, 17);
            this.rboFNC3.TabIndex = 10;
            this.rboFNC3.Text = "User Define File Name";
            this.rboFNC3.UseVisualStyleBackColor = true;
            // 
            // rboFNC2
            // 
            this.rboFNC2.AutoSize = true;
            this.rboFNC2.Location = new System.Drawing.Point(58, 60);
            this.rboFNC2.Margin = new System.Windows.Forms.Padding(2);
            this.rboFNC2.Name = "rboFNC2";
            this.rboFNC2.Size = new System.Drawing.Size(228, 17);
            this.rboFNC2.TabIndex = 9;
            this.rboFNC2.Text = "Prefix System Name with Default File Name";
            this.rboFNC2.UseVisualStyleBackColor = true;
            // 
            // rboFNC1
            // 
            this.rboFNC1.AutoSize = true;
            this.rboFNC1.Location = new System.Drawing.Point(58, 38);
            this.rboFNC1.Margin = new System.Windows.Forms.Padding(2);
            this.rboFNC1.Name = "rboFNC1";
            this.rboFNC1.Size = new System.Drawing.Size(180, 17);
            this.rboFNC1.TabIndex = 8;
            this.rboFNC1.Text = "System Generated Default Name";
            this.rboFNC1.UseVisualStyleBackColor = true;
            // 
            // tbDashBoard
            // 
            this.tbDashBoard.Controls.Add(this.groupBox6);
            this.tbDashBoard.Location = new System.Drawing.Point(4, 22);
            this.tbDashBoard.Name = "tbDashBoard";
            this.tbDashBoard.Padding = new System.Windows.Forms.Padding(3);
            this.tbDashBoard.Size = new System.Drawing.Size(451, 374);
            this.tbDashBoard.TabIndex = 1;
            this.tbDashBoard.Text = "Dash Board";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chkSelectAll);
            this.groupBox6.Controls.Add(this.chklstDashBoard);
            this.groupBox6.Location = new System.Drawing.Point(9, 4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(421, 374);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Dash Board Parameters";
            // 
            // chkSelectAll
            // 
            this.chkSelectAll.AutoSize = true;
            this.chkSelectAll.Location = new System.Drawing.Point(53, 338);
            this.chkSelectAll.Name = "chkSelectAll";
            this.chkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.chkSelectAll.TabIndex = 1;
            this.chkSelectAll.Text = "Select All";
            this.chkSelectAll.UseVisualStyleBackColor = true;
            this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
            // 
            // chklstDashBoard
            // 
            this.chklstDashBoard.FormattingEnabled = true;
            this.chklstDashBoard.Items.AddRange(new object[] {
            "Meter ID",
            "Meter Type",
            "Current Rating ",
            "Voltage Rating",
            "Meter constant ",
            "Manufacturing Date",
            "Type of Billing",
            "Power Factor Logic ",
            "Power Off Days",
            "MD Reset Counter",
            "Readout counter",
            "Total Power On Hours",
            "Number of Billings",
            "Last Billing Timestamp",
            "Load survey Days",
            "Number of Programming Updates",
            "Last Transaction",
            "Last Transaction Timestamp",
            "Number of RTC Updates",
            "Daily Profile Avaialble Days"});
            this.chklstDashBoard.Location = new System.Drawing.Point(50, 22);
            this.chklstDashBoard.Name = "chklstDashBoard";
            this.chklstDashBoard.Size = new System.Drawing.Size(321, 334);
            this.chklstDashBoard.TabIndex = 0;
            // 
            // GeneralConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.ClientSize = new System.Drawing.Size(510, 485);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GeneralConfiguration";
            this.StatusMessage = "";
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.GeneralConfiguration_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GeneralConfiguration_FormClosing);
            this.Activated += new System.EventHandler(this.rbtnCustom_CheckedChanged);
            this.groupBox1.ResumeLayout(false);
            this.tabControlSystemConfiguration.ResumeLayout(false);
            this.tbGeneral.ResumeLayout(false);
            this.tbGeneral.PerformLayout();
            this.grpRetries.ResumeLayout(false);
            this.grpRetries.PerformLayout();
            this.grpLoadSurvey.ResumeLayout(false);
            this.grpLoadSurvey.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tbDashBoard.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.CABButton lngbSave;
        private CAB.UI.Controls.CABButton lngbCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private CAB.UI.Controls.PremiumTabControl tabControlSystemConfiguration;
        private System.Windows.Forms.TabPage tbGeneral;
        private System.Windows.Forms.GroupBox groupBox4;
        internal System.Windows.Forms.ComboBox cboDate;
        private CAB.UI.Controls.CABLabel COMPortSet_lblCOMPort;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtDefaultCABLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.RadioButton rbtnDefault;
        private System.Windows.Forms.RadioButton rbtnCustom;
        private System.Windows.Forms.TextBox txtCustomCABLocation;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rboFNC3;
        private System.Windows.Forms.RadioButton rboFNC2;
        private System.Windows.Forms.RadioButton rboFNC1;
        private System.Windows.Forms.TabPage tbDashBoard;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckedListBox chklstDashBoard;
        private System.Windows.Forms.CheckBox chkSelectAll;
        private System.Windows.Forms.GroupBox grpLoadSurvey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbLoadSurveyDays;
        private System.Windows.Forms.GroupBox grpRetries;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbNoOfRetries;
        private System.Windows.Forms.CheckBox chkBoxNumPowFail;
        private System.Windows.Forms.RadioButton rbtDNWM;
        private System.Windows.Forms.CheckBox chkBoxHideNameplate;
        private System.Windows.Forms.CheckBox ChkPowerOnOffDurationFormat;
        private System.Windows.Forms.Label lblPowerOnOfDurationFormat;

    }
}



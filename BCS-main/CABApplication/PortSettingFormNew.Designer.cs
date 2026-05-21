namespace CAB.UI
{
    partial class PortSettingFormNew
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
            this.errpPortMapping = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtRemote = new System.Windows.Forms.RadioButton();
            this.rdDirect = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelRemote = new System.Windows.Forms.Panel();
            this.rbtTCP = new System.Windows.Forms.RadioButton();
            this.rbtGPRS = new System.Windows.Forms.RadioButton();
            this.rbtPSTN = new System.Windows.Forms.RadioButton();
            this.rbtGSM = new System.Windows.Forms.RadioButton();
            this.panelDirect = new System.Windows.Forms.Panel();
            this.rbtManual = new System.Windows.Forms.RadioButton();
            this.rbtAuto = new System.Windows.Forms.RadioButton();
            this.lblInitialBaudRate = new CAB.UI.Controls.CABLabel();
            this.cboInitialbaudRate = new System.Windows.Forms.ComboBox();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.COMPortSet_lblBaudRate = new CAB.UI.Controls.CABLabel();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.COMPortSet_lblCOMPort = new CAB.UI.Controls.CABLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chk96IEC = new System.Windows.Forms.CheckBox();
            this.chk300IEC = new System.Windows.Forms.CheckBox();
            this.chk96DLMS = new System.Windows.Forms.CheckBox();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.btnModemConfig = new CAB.UI.Controls.CABButton();
            this.btnModemInfo = new CAB.UI.Controls.CABButton();
            ((System.ComponentModel.ISupportInitialize)(this.errpPortMapping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panelRemote.SuspendLayout();
            this.panelDirect.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // errpPortMapping
            // 
            this.errpPortMapping.ContainerControl = this;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtRemote);
            this.groupBox2.Controls.Add(this.rdDirect);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(60)))), ((int)(((byte)(110)))));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(420, 55);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "🔌  Connection Mode";
            // 
            // rbtRemote
            // 
            this.rbtRemote.AutoSize = true;
            this.rbtRemote.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rbtRemote.Location = new System.Drawing.Point(220, 22);
            this.rbtRemote.Name = "rbtRemote";
            this.rbtRemote.Size = new System.Drawing.Size(202, 29);
            this.rbtRemote.TabIndex = 49;
            this.rbtRemote.Text = "Remote Connection";
            this.rbtRemote.UseVisualStyleBackColor = true;
            this.rbtRemote.Visible = false;
            this.rbtRemote.CheckedChanged += new System.EventHandler(this.rbtRemote_CheckedChanged);
            // 
            // rdDirect
            // 
            this.rdDirect.AutoSize = true;
            this.rdDirect.Checked = true;
            this.rdDirect.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.rdDirect.Location = new System.Drawing.Point(40, 22);
            this.rdDirect.Name = "rdDirect";
            this.rdDirect.Size = new System.Drawing.Size(189, 29);
            this.rdDirect.TabIndex = 48;
            this.rdDirect.TabStop = true;
            this.rdDirect.Text = "Direct Connection";
            this.rdDirect.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelRemote);
            this.groupBox1.Controls.Add(this.panelDirect);
            this.groupBox1.Controls.Add(this.lblInitialBaudRate);
            this.groupBox1.Controls.Add(this.cboInitialbaudRate);
            this.groupBox1.Controls.Add(this.cboBaudRate);
            this.groupBox1.Controls.Add(this.COMPortSet_lblBaudRate);
            this.groupBox1.Controls.Add(this.cboPort);
            this.groupBox1.Controls.Add(this.COMPortSet_lblCOMPort);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(60)))), ((int)(((byte)(110)))));
            this.groupBox1.Location = new System.Drawing.Point(15, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 320);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "⚙️  Port Configuration";
            // 
            // panelRemote
            // 
            this.panelRemote.Controls.Add(this.rbtTCP);
            this.panelRemote.Controls.Add(this.rbtGPRS);
            this.panelRemote.Controls.Add(this.rbtPSTN);
            this.panelRemote.Controls.Add(this.rbtGSM);
            this.panelRemote.Location = new System.Drawing.Point(8, 128);
            this.panelRemote.Name = "panelRemote";
            this.panelRemote.Size = new System.Drawing.Size(276, 39);
            this.panelRemote.TabIndex = 56;
            this.panelRemote.Visible = false;
            // 
            // rbtTCP
            // 
            this.rbtTCP.AutoSize = true;
            this.rbtTCP.Location = new System.Drawing.Point(139, 11);
            this.rbtTCP.Name = "rbtTCP";
            this.rbtTCP.Size = new System.Drawing.Size(101, 34);
            this.rbtTCP.TabIndex = 55;
            this.rbtTCP.Text = "TCP/IP";
            this.rbtTCP.UseVisualStyleBackColor = true;
            this.rbtTCP.CheckedChanged += new System.EventHandler(this.rbtTCP_CheckedChanged);
            // 
            // rbtGPRS
            // 
            this.rbtGPRS.AutoSize = true;
            this.rbtGPRS.Location = new System.Drawing.Point(207, 11);
            this.rbtGPRS.Name = "rbtGPRS";
            this.rbtGPRS.Size = new System.Drawing.Size(89, 34);
            this.rbtGPRS.TabIndex = 54;
            this.rbtGPRS.Text = "GPRS";
            this.rbtGPRS.UseVisualStyleBackColor = true;
            this.rbtGPRS.CheckedChanged += new System.EventHandler(this.rbtGPRS_CheckedChanged);
            // 
            // rbtPSTN
            // 
            this.rbtPSTN.AutoSize = true;
            this.rbtPSTN.Location = new System.Drawing.Point(70, 11);
            this.rbtPSTN.Name = "rbtPSTN";
            this.rbtPSTN.Size = new System.Drawing.Size(89, 34);
            this.rbtPSTN.TabIndex = 53;
            this.rbtPSTN.Text = "PSTN";
            this.rbtPSTN.UseVisualStyleBackColor = true;
            this.rbtPSTN.CheckedChanged += new System.EventHandler(this.rbtPSTN_CheckedChanged);
            // 
            // rbtGSM
            // 
            this.rbtGSM.AutoSize = true;
            this.rbtGSM.Checked = true;
            this.rbtGSM.Location = new System.Drawing.Point(11, 11);
            this.rbtGSM.Name = "rbtGSM";
            this.rbtGSM.Size = new System.Drawing.Size(83, 34);
            this.rbtGSM.TabIndex = 52;
            this.rbtGSM.TabStop = true;
            this.rbtGSM.Text = "GSM";
            this.rbtGSM.UseVisualStyleBackColor = true;
            this.rbtGSM.CheckedChanged += new System.EventHandler(this.rbtGSM_CheckedChanged);
            // 
            // panelDirect
            // 
            this.panelDirect.Controls.Add(this.rbtManual);
            this.panelDirect.Controls.Add(this.rbtAuto);
            this.panelDirect.Location = new System.Drawing.Point(8, 128);
            this.panelDirect.Name = "panelDirect";
            this.panelDirect.Size = new System.Drawing.Size(276, 39);
            this.panelDirect.TabIndex = 54;
            this.panelDirect.Visible = false;
            // 
            // rbtManual
            // 
            this.rbtManual.AutoSize = true;
            this.rbtManual.Location = new System.Drawing.Point(138, 11);
            this.rbtManual.Name = "rbtManual";
            this.rbtManual.Size = new System.Drawing.Size(108, 34);
            this.rbtManual.TabIndex = 53;
            this.rbtManual.Text = "Manual";
            this.rbtManual.UseVisualStyleBackColor = true;
            this.rbtManual.Visible = false;
            // 
            // rbtAuto
            // 
            this.rbtAuto.AutoSize = true;
            this.rbtAuto.Checked = true;
            this.rbtAuto.Location = new System.Drawing.Point(20, 11);
            this.rbtAuto.Name = "rbtAuto";
            this.rbtAuto.Size = new System.Drawing.Size(85, 34);
            this.rbtAuto.TabIndex = 52;
            this.rbtAuto.TabStop = true;
            this.rbtAuto.Text = "Auto";
            this.rbtAuto.UseVisualStyleBackColor = true;
            this.rbtAuto.CheckedChanged += new System.EventHandler(this.rbtAuto_CheckedChanged);
            // 
            // lblInitialBaudRate
            // 
            this.lblInitialBaudRate.AutoSize = true;
            this.lblInitialBaudRate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblInitialBaudRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.lblInitialBaudRate.Location = new System.Drawing.Point(25, 104);
            this.lblInitialBaudRate.Name = "lblInitialBaudRate";
            this.lblInitialBaudRate.Size = new System.Drawing.Size(153, 25);
            this.lblInitialBaudRate.TabIndex = 13;
            this.lblInitialBaudRate.Text = "Initial Baud Rate:";
            this.lblInitialBaudRate.TranslationKey = null;
            // 
            // cboInitialbaudRate
            // 
            this.cboInitialbaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInitialbaudRate.FormattingEnabled = true;
            this.cboInitialbaudRate.Items.AddRange(new object[] {
            "300",
            "9600",
            "34800"});
            this.cboInitialbaudRate.Location = new System.Drawing.Point(146, 101);
            this.cboInitialbaudRate.Name = "cboInitialbaudRate";
            this.cboInitialbaudRate.Size = new System.Drawing.Size(121, 38);
            this.cboInitialbaudRate.TabIndex = 12;
            this.cboInitialbaudRate.SelectedIndexChanged += new System.EventHandler(this.cboInitialbaudRate_SelectedIndexChanged);
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaudRate.FormattingEnabled = true;
            this.cboBaudRate.Items.AddRange(new object[] {
            "300",
            "9600",
            "19200",
            "38400"});
            this.cboBaudRate.Location = new System.Drawing.Point(146, 62);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(121, 38);
            this.cboBaudRate.TabIndex = 3;
            this.cboBaudRate.SelectedIndexChanged += new System.EventHandler(this.cboBaudRate_SelectedIndexChanged);
            // 
            // COMPortSet_lblBaudRate
            // 
            this.COMPortSet_lblBaudRate.AutoSize = true;
            this.COMPortSet_lblBaudRate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.COMPortSet_lblBaudRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.COMPortSet_lblBaudRate.Location = new System.Drawing.Point(25, 65);
            this.COMPortSet_lblBaudRate.Name = "COMPortSet_lblBaudRate";
            this.COMPortSet_lblBaudRate.Size = new System.Drawing.Size(101, 25);
            this.COMPortSet_lblBaudRate.TabIndex = 2;
            this.COMPortSet_lblBaudRate.Text = "Baud Rate:";
            this.COMPortSet_lblBaudRate.TranslationKey = null;
            // 
            // cboPort
            // 
            this.cboPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new System.Drawing.Point(146, 25);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(121, 38);
            this.cboPort.TabIndex = 1;
            // 
            // COMPortSet_lblCOMPort
            // 
            this.COMPortSet_lblCOMPort.AutoSize = true;
            this.COMPortSet_lblCOMPort.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.COMPortSet_lblCOMPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.COMPortSet_lblCOMPort.Location = new System.Drawing.Point(25, 28);
            this.COMPortSet_lblCOMPort.Name = "COMPortSet_lblCOMPort";
            this.COMPortSet_lblCOMPort.Size = new System.Drawing.Size(97, 25);
            this.COMPortSet_lblCOMPort.TabIndex = 0;
            this.COMPortSet_lblCOMPort.Text = "COM Port:";
            this.COMPortSet_lblCOMPort.TranslationKey = null;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chk96IEC);
            this.groupBox3.Controls.Add(this.chk300IEC);
            this.groupBox3.Controls.Add(this.chk96DLMS);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(60)))), ((int)(((byte)(110)))));
            this.groupBox3.Location = new System.Drawing.Point(300, 85);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 320);
            this.groupBox3.TabIndex = 52;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "📋  Protocol Profile";
            this.groupBox3.Visible = false;
            // 
            // chk96IEC
            // 
            this.chk96IEC.AutoSize = true;
            this.chk96IEC.Enabled = false;
            this.chk96IEC.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chk96IEC.Location = new System.Drawing.Point(20, 75);
            this.chk96IEC.Name = "chk96IEC";
            this.chk96IEC.Size = new System.Drawing.Size(243, 29);
            this.chk96IEC.TabIndex = 56;
            this.chk96IEC.Text = "IEC Profile (9600, 8, N, 1)";
            this.chk96IEC.UseVisualStyleBackColor = true;
            this.chk96IEC.CheckedChanged += new System.EventHandler(this.chk96IEC_CheckedChanged);
            // 
            // chk300IEC
            // 
            this.chk300IEC.AutoSize = true;
            this.chk300IEC.Enabled = false;
            this.chk300IEC.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chk300IEC.Location = new System.Drawing.Point(20, 50);
            this.chk300IEC.Name = "chk300IEC";
            this.chk300IEC.Size = new System.Drawing.Size(229, 29);
            this.chk300IEC.TabIndex = 55;
            this.chk300IEC.Text = "IEC Profile (300, 7, E, 1)";
            this.chk300IEC.UseVisualStyleBackColor = true;
            this.chk300IEC.CheckedChanged += new System.EventHandler(this.chk300IEC_CheckedChanged);
            // 
            // chk96DLMS
            // 
            this.chk96DLMS.AutoSize = true;
            this.chk96DLMS.Enabled = false;
            this.chk96DLMS.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chk96DLMS.Location = new System.Drawing.Point(20, 25);
            this.chk96DLMS.Name = "chk96DLMS";
            this.chk96DLMS.Size = new System.Drawing.Size(265, 29);
            this.chk96DLMS.TabIndex = 54;
            this.chk96DLMS.Text = "DLMS Profile (9600, 8, N, 1)";
            this.chk96DLMS.UseVisualStyleBackColor = true;
            this.chk96DLMS.CheckedChanged += new System.EventHandler(this.chk96DLMS_CheckedChanged);
            // 
            // lngbCancel
            // 
            this.lngbCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.lngbCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lngbCancel.FlatAppearance.BorderSize = 0;
            this.lngbCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbCancel.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lngbCancel.ForeColor = System.Drawing.Color.White;
            this.lngbCancel.Location = new System.Drawing.Point(140, 420);
            this.lngbCancel.Name = "lngbCancel";
            this.lngbCancel.Size = new System.Drawing.Size(100, 32);
            this.lngbCancel.TabIndex = 55;
            this.lngbCancel.Text = "✖️  Cancel";
            this.lngbCancel.TranslationKey = null;
            this.lngbCancel.UseVisualStyleBackColor = false;
            this.lngbCancel.Click += new System.EventHandler(this.lngbCancel_Click_1);
            // 
            // lngbSave
            // 
            this.lngbSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lngbSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lngbSave.FlatAppearance.BorderSize = 0;
            this.lngbSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbSave.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.lngbSave.ForeColor = System.Drawing.Color.White;
            this.lngbSave.Location = new System.Drawing.Point(24, 420);
            this.lngbSave.Name = "lngbSave";
            this.lngbSave.Size = new System.Drawing.Size(110, 32);
            this.lngbSave.TabIndex = 54;
            this.lngbSave.Text = "💾  Save Settings";
            this.lngbSave.TranslationKey = null;
            this.lngbSave.UseVisualStyleBackColor = false;
            this.lngbSave.Click += new System.EventHandler(this.lngbSave_Click);
            // 
            // btnModemConfig
            // 
            this.btnModemConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnModemConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModemConfig.FlatAppearance.BorderSize = 0;
            this.btnModemConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModemConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnModemConfig.ForeColor = System.Drawing.Color.White;
            this.btnModemConfig.Location = new System.Drawing.Point(347, 420);
            this.btnModemConfig.Name = "btnModemConfig";
            this.btnModemConfig.Size = new System.Drawing.Size(110, 32);
            this.btnModemConfig.TabIndex = 56;
            this.btnModemConfig.Text = "🛠️  Modem Settings";
            this.btnModemConfig.TranslationKey = null;
            this.btnModemConfig.UseVisualStyleBackColor = false;
            this.btnModemConfig.Visible = false;
            this.btnModemConfig.Click += new System.EventHandler(this.btnModemConfig_Click);
            // 
            // btnModemInfo
            // 
            this.btnModemInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnModemInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModemInfo.FlatAppearance.BorderSize = 0;
            this.btnModemInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModemInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnModemInfo.ForeColor = System.Drawing.Color.White;
            this.btnModemInfo.Location = new System.Drawing.Point(246, 420);
            this.btnModemInfo.Name = "btnModemInfo";
            this.btnModemInfo.Size = new System.Drawing.Size(95, 32);
            this.btnModemInfo.TabIndex = 57;
            this.btnModemInfo.Text = "ℹ️  Modem Info";
            this.btnModemInfo.TranslationKey = null;
            this.btnModemInfo.UseVisualStyleBackColor = false;
            this.btnModemInfo.Visible = false;
            this.btnModemInfo.Click += new System.EventHandler(this.btnModemInfo_Click);
            // 
            // PortSettingFormNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(580, 480);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnModemInfo);
            this.Controls.Add(this.btnModemConfig);
            this.Controls.Add(this.lngbCancel);
            this.Controls.Add(this.lngbSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "PortSettingFormNew";
            this.StatusMessage = "";
            this.Text = "Port Settings";
            this.Activated += new System.EventHandler(this.PortSettingForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortSettingForm_FormClosing);
            this.Load += new System.EventHandler(this.PortSettingFormNew_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errpPortMapping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelRemote.ResumeLayout(false);
            this.panelRemote.PerformLayout();
            this.panelDirect.ResumeLayout(false);
            this.panelDirect.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errpPortMapping;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtRemote;
        private System.Windows.Forms.RadioButton rdDirect;
        private System.Windows.Forms.GroupBox groupBox1;
        private CAB.UI.Controls.CABLabel lblInitialBaudRate;
        private System.Windows.Forms.ComboBox cboInitialbaudRate;
        internal System.Windows.Forms.ComboBox cboBaudRate;
        private CAB.UI.Controls.CABLabel COMPortSet_lblBaudRate;
        internal System.Windows.Forms.ComboBox cboPort;
        private CAB.UI.Controls.CABLabel COMPortSet_lblCOMPort;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbSave;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chk96IEC;
        private System.Windows.Forms.CheckBox chk300IEC;
        private System.Windows.Forms.CheckBox chk96DLMS;
        private System.Windows.Forms.Panel panelDirect;
        private System.Windows.Forms.RadioButton rbtManual;
        private System.Windows.Forms.RadioButton rbtAuto;
        private System.Windows.Forms.Panel panelRemote;
        private System.Windows.Forms.RadioButton rbtPSTN;
        private System.Windows.Forms.RadioButton rbtGSM;
        private System.Windows.Forms.RadioButton rbtGPRS;
        private CAB.UI.Controls.CABButton btnModemConfig;
        private CAB.UI.Controls.CABButton btnModemInfo;
        private System.Windows.Forms.RadioButton rbtTCP;

    }
}


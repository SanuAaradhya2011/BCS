namespace CAB.UI
{
    partial class PortSettingForm
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdGPRS = new System.Windows.Forms.RadioButton();
            this.rdPSTN = new System.Windows.Forms.RadioButton();
            this.rdGSM = new System.Windows.Forms.RadioButton();
            this.rdDirect = new System.Windows.Forms.RadioButton();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCommMode = new CAB.UI.Controls.CABLabel();
            this.cboCommMode = new System.Windows.Forms.ComboBox();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.COMPortSet_lblBaudRate = new CAB.UI.Controls.CABLabel();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.COMPortSet_lblCOMPort = new CAB.UI.Controls.CABLabel();
            this.errpPortMapping = new System.Windows.Forms.ErrorProvider(this.components);
            this.dgvPortUsageAssociation = new System.Windows.Forms.DataGridView();
            this.colPortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortUsageTypeModem = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colPortUsageTypeCMRI = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.panelMultiple = new System.Windows.Forms.Panel();
            this.chkIsNonDLMS = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errpPortMapping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortUsageAssociation)).BeginInit();
            this.panelMultiple.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdGPRS);
            this.groupBox2.Controls.Add(this.rdPSTN);
            this.groupBox2.Controls.Add(this.rdGSM);
            this.groupBox2.Controls.Add(this.rdDirect);
            this.groupBox2.Location = new System.Drawing.Point(15, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(293, 48);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Communication Type";
            // 
            // rdGPRS
            // 
            this.rdGPRS.AutoSize = true;
            this.rdGPRS.Location = new System.Drawing.Point(216, 18);
            this.rdGPRS.Name = "rdGPRS";
            this.rdGPRS.Size = new System.Drawing.Size(55, 17);
            this.rdGPRS.TabIndex = 51;
            this.rdGPRS.Text = "GPRS";
            this.rdGPRS.UseVisualStyleBackColor = true;
            this.rdGPRS.Visible = false;
            this.rdGPRS.CheckedChanged += new System.EventHandler(this.rdGPRS_CheckedChanged);
            // 
            // rdPSTN
            // 
            this.rdPSTN.AutoSize = true;
            this.rdPSTN.Location = new System.Drawing.Point(149, 18);
            this.rdPSTN.Name = "rdPSTN";
            this.rdPSTN.Size = new System.Drawing.Size(54, 17);
            this.rdPSTN.TabIndex = 50;
            this.rdPSTN.Text = "PSTN";
            this.rdPSTN.UseVisualStyleBackColor = true;
            this.rdPSTN.CheckedChanged += new System.EventHandler(this.rdPSTN_CheckedChanged);
            // 
            // rdGSM
            // 
            this.rdGSM.AutoSize = true;
            this.rdGSM.Location = new System.Drawing.Point(87, 18);
            this.rdGSM.Name = "rdGSM";
            this.rdGSM.Size = new System.Drawing.Size(49, 17);
            this.rdGSM.TabIndex = 49;
            this.rdGSM.Text = "GSM";
            this.rdGSM.UseVisualStyleBackColor = true;
            this.rdGSM.CheckedChanged += new System.EventHandler(this.rdGSM_CheckedChanged);
            // 
            // rdDirect
            // 
            this.rdDirect.AutoSize = true;
            this.rdDirect.Checked = true;
            this.rdDirect.Location = new System.Drawing.Point(21, 18);
            this.rdDirect.Name = "rdDirect";
            this.rdDirect.Size = new System.Drawing.Size(53, 17);
            this.rdDirect.TabIndex = 48;
            this.rdDirect.TabStop = true;
            this.rdDirect.Text = "Direct";
            this.rdDirect.UseVisualStyleBackColor = true;
            this.rdDirect.CheckedChanged += new System.EventHandler(this.rdDirect_CheckedChanged);
            // 
            // lngbCancel
            // 
            this.lngbCancel.Location = new System.Drawing.Point(255, 292);
            this.lngbCancel.Name = "lngbCancel";
            this.lngbCancel.Size = new System.Drawing.Size(55, 25);
            this.lngbCancel.TabIndex = 51;
            this.lngbCancel.Text = "Cancel";
            this.lngbCancel.TranslationKey = null;
            this.lngbCancel.UseVisualStyleBackColor = false;
            this.lngbCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbCancel.ForeColor = System.Drawing.Color.White;
            this.lngbCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbCancel.FlatAppearance.BorderSize = 0;
            this.lngbCancel.Click += new System.EventHandler(this.lngbCancel_Click);
            // 
            // lngbSave
            // 
            this.lngbSave.Location = new System.Drawing.Point(194, 292);
            this.lngbSave.Name = "lngbSave";
            this.lngbSave.Size = new System.Drawing.Size(55, 25);
            this.lngbSave.TabIndex = 50;
            this.lngbSave.Text = "Save";
            this.lngbSave.TranslationKey = null;
            this.lngbSave.UseVisualStyleBackColor = false;
            this.lngbSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.lngbSave.ForeColor = System.Drawing.Color.White;
            this.lngbSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lngbSave.FlatAppearance.BorderSize = 0;
            this.lngbSave.Click += new System.EventHandler(this.lngbSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCommMode);
            this.groupBox1.Controls.Add(this.cboCommMode);
            this.groupBox1.Controls.Add(this.cboBaudRate);
            this.groupBox1.Controls.Add(this.COMPortSet_lblBaudRate);
            this.groupBox1.Controls.Add(this.cboPort);
            this.groupBox1.Controls.Add(this.COMPortSet_lblCOMPort);
            this.groupBox1.Location = new System.Drawing.Point(15, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 197);
            this.groupBox1.TabIndex = 52;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Port Settings";
            // 
            // lblCommMode
            // 
            this.lblCommMode.AutoSize = true;
            this.lblCommMode.Location = new System.Drawing.Point(25, 136);
            this.lblCommMode.Name = "lblCommMode";
            this.lblCommMode.Size = new System.Drawing.Size(111, 13);
            this.lblCommMode.TabIndex = 13;
            this.lblCommMode.Text = "Communication  mode";
            this.lblCommMode.TranslationKey = null;
            // 
            // cboCommMode
            // 
            this.cboCommMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCommMode.FormattingEnabled = true;
            this.cboCommMode.Items.AddRange(new object[] {
            "RS 232",
            "Optical"});
            this.cboCommMode.Location = new System.Drawing.Point(146, 133);
            this.cboCommMode.Name = "cboCommMode";
            this.cboCommMode.Size = new System.Drawing.Size(121, 21);
            this.cboCommMode.TabIndex = 12;
            this.cboCommMode.SelectedIndexChanged += new System.EventHandler(this.cboCommMode_SelectedIndexChanged);
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaudRate.FormattingEnabled = true;
            this.cboBaudRate.Location = new System.Drawing.Point(146, 91);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(121, 21);
            this.cboBaudRate.TabIndex = 3;
            // 
            // COMPortSet_lblBaudRate
            // 
            this.COMPortSet_lblBaudRate.AutoSize = true;
            this.COMPortSet_lblBaudRate.Location = new System.Drawing.Point(25, 94);
            this.COMPortSet_lblBaudRate.Name = "COMPortSet_lblBaudRate";
            this.COMPortSet_lblBaudRate.Size = new System.Drawing.Size(58, 13);
            this.COMPortSet_lblBaudRate.TabIndex = 2;
            this.COMPortSet_lblBaudRate.Text = "Baud Rate";
            this.COMPortSet_lblBaudRate.TranslationKey = null;
            // 
            // cboPort
            // 
            this.cboPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPort.FormattingEnabled = true;
            this.cboPort.Location = new System.Drawing.Point(146, 49);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(121, 21);
            this.cboPort.TabIndex = 1;
            // 
            // COMPortSet_lblCOMPort
            // 
            this.COMPortSet_lblCOMPort.AutoSize = true;
            this.COMPortSet_lblCOMPort.Location = new System.Drawing.Point(25, 52);
            this.COMPortSet_lblCOMPort.Name = "COMPortSet_lblCOMPort";
            this.COMPortSet_lblCOMPort.Size = new System.Drawing.Size(53, 13);
            this.COMPortSet_lblCOMPort.TabIndex = 0;
            this.COMPortSet_lblCOMPort.Text = "COM Port";
            this.COMPortSet_lblCOMPort.TranslationKey = null;
            // 
            // errpPortMapping
            // 
            this.errpPortMapping.ContainerControl = this;
            // 
            // dgvPortUsageAssociation
            // 
            this.dgvPortUsageAssociation.AllowUserToAddRows = false;
            this.dgvPortUsageAssociation.AllowUserToDeleteRows = false;
            this.dgvPortUsageAssociation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortUsageAssociation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPortName,
            this.colPortUsageTypeModem,
            this.colPortUsageTypeCMRI});
            this.dgvPortUsageAssociation.Location = new System.Drawing.Point(17, 8);
            this.dgvPortUsageAssociation.Name = "dgvPortUsageAssociation";
            this.dgvPortUsageAssociation.RowHeadersWidth = 30;
            this.dgvPortUsageAssociation.Size = new System.Drawing.Size(258, 150);
            this.dgvPortUsageAssociation.TabIndex = 41;
            this.dgvPortUsageAssociation.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvPortUsageAssociation_CellBeginEdit);
            this.dgvPortUsageAssociation.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPortUsageAssociation_CellMouseLeave);
            this.dgvPortUsageAssociation.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPortUsageAssociation_CellEndEdit);
            // 
            // colPortName
            // 
            this.colPortName.HeaderText = "Port Name";
            this.colPortName.Name = "colPortName";
            this.colPortName.ReadOnly = true;
            this.colPortName.Width = 70;
            // 
            // colPortUsageTypeModem
            // 
            this.colPortUsageTypeModem.HeaderText = "Scheduling";
            this.colPortUsageTypeModem.Name = "colPortUsageTypeModem";
            this.colPortUsageTypeModem.Width = 70;
            // 
            // colPortUsageTypeCMRI
            // 
            this.colPortUsageTypeCMRI.HeaderText = "Direct";
            this.colPortUsageTypeCMRI.Name = "colPortUsageTypeCMRI";
            this.colPortUsageTypeCMRI.Width = 40;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(97, 169);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(99, 22);
            this.btnTestConnection.TabIndex = 42;
            this.btnTestConnection.Text = "Test Connections";
            this.btnTestConnection.UseVisualStyleBackColor = false;
            this.btnTestConnection.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnTestConnection.ForeColor = System.Drawing.Color.White;
            this.btnTestConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestConnection.FlatAppearance.BorderSize = 0;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // panelMultiple
            // 
            this.panelMultiple.Controls.Add(this.btnTestConnection);
            this.panelMultiple.Controls.Add(this.dgvPortUsageAssociation);
            this.panelMultiple.Location = new System.Drawing.Point(15, 63);
            this.panelMultiple.Name = "panelMultiple";
            this.panelMultiple.Size = new System.Drawing.Size(293, 198);
            this.panelMultiple.TabIndex = 50;
            this.panelMultiple.Visible = false;
            // 
            // chkIsNonDLMS
            // 
            this.chkIsNonDLMS.AutoSize = true;
            this.chkIsNonDLMS.Location = new System.Drawing.Point(15, 269);
            this.chkIsNonDLMS.Name = "chkIsNonDLMS";
            this.chkIsNonDLMS.Size = new System.Drawing.Size(161, 17);
            this.chkIsNonDLMS.TabIndex = 53;
            this.chkIsNonDLMS.Text = "For Single Phase IEC Supply";
            this.chkIsNonDLMS.UseVisualStyleBackColor = true;
            // 
            // PortSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(319, 332);
            this.Controls.Add(this.chkIsNonDLMS);
            this.Controls.Add(this.lngbCancel);
            this.Controls.Add(this.lngbSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelMultiple);
            this.Name = "PortSettingForm";
            this.StatusMessage = "";
            this.Text = "Port Settings";
            this.Load += new System.EventHandler(this.PortSettingForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortSettingForm_FormClosing);
            this.Activated += new System.EventHandler(this.PortSettingForm_Activated);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errpPortMapping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortUsageAssociation)).EndInit();
            this.panelMultiple.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdGPRS;
        private System.Windows.Forms.RadioButton rdPSTN;
        private System.Windows.Forms.RadioButton rdGSM;
        private System.Windows.Forms.RadioButton rdDirect;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private CAB.UI.Controls.CABLabel lblCommMode;
        private System.Windows.Forms.ComboBox cboCommMode;
        internal System.Windows.Forms.ComboBox cboBaudRate;
        private CAB.UI.Controls.CABLabel COMPortSet_lblBaudRate;
        internal System.Windows.Forms.ComboBox cboPort;
        private CAB.UI.Controls.CABLabel COMPortSet_lblCOMPort;
        private System.Windows.Forms.ErrorProvider errpPortMapping;
        private System.Windows.Forms.Panel panelMultiple;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.DataGridView dgvPortUsageAssociation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPortName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPortUsageTypeModem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colPortUsageTypeCMRI;
        private System.Windows.Forms.CheckBox chkIsNonDLMS;

    }
}


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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblCommMode = new CAB.UI.Controls.CABLabel();
            this.cboCommMode = new System.Windows.Forms.ComboBox();
            this.lngbCancel = new CAB.UI.Controls.CABButton();
            this.lngbSave = new CAB.UI.Controls.CABButton();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.COMPortSet_lblBaudRate = new CAB.UI.Controls.CABLabel();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.COMPortSet_lblCOMPort = new CAB.UI.Controls.CABLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblCommMode);
            this.groupBox1.Controls.Add(this.cboCommMode);
            this.groupBox1.Controls.Add(this.lngbCancel);
            this.groupBox1.Controls.Add(this.lngbSave);
            this.groupBox1.Controls.Add(this.cboBaudRate);
            this.groupBox1.Controls.Add(this.COMPortSet_lblBaudRate);
            this.groupBox1.Controls.Add(this.cboPort);
            this.groupBox1.Controls.Add(this.COMPortSet_lblCOMPort);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 242);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "COM Port Settings";
            // 
            // lblCommMode
            // 
            this.lblCommMode.AutoSize = true;
            this.lblCommMode.Location = new System.Drawing.Point(67, 130);
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
            this.cboCommMode.Location = new System.Drawing.Point(188, 127);
            this.cboCommMode.Name = "cboCommMode";
            this.cboCommMode.Size = new System.Drawing.Size(121, 21);
            this.cboCommMode.TabIndex = 12;
            this.cboCommMode.SelectedIndexChanged += new System.EventHandler(this.cboCommMode_SelectedIndexChanged);
            // 
            // lngbCancel
            // 
            this.lngbCancel.Location = new System.Drawing.Point(254, 191);
            this.lngbCancel.Name = "lngbCancel";
            this.lngbCancel.Size = new System.Drawing.Size(55, 25);
            this.lngbCancel.TabIndex = 5;
            this.lngbCancel.Text = "Cancel";
            this.lngbCancel.TranslationKey = null;
            this.lngbCancel.UseVisualStyleBackColor = true;
            this.lngbCancel.Click += new System.EventHandler(this.lngbCancel_Click);
            // 
            // lngbSave
            // 
            this.lngbSave.Location = new System.Drawing.Point(193, 191);
            this.lngbSave.Name = "lngbSave";
            this.lngbSave.Size = new System.Drawing.Size(55, 25);
            this.lngbSave.TabIndex = 4;
            this.lngbSave.Text = "Save";
            this.lngbSave.TranslationKey = null;
            this.lngbSave.UseVisualStyleBackColor = true;
            this.lngbSave.Click += new System.EventHandler(this.lngbSave_Click);
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaudRate.FormattingEnabled = true;
            this.cboBaudRate.Location = new System.Drawing.Point(188, 85);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(121, 21);
            this.cboBaudRate.TabIndex = 3;
            // 
            // COMPortSet_lblBaudRate
            // 
            this.COMPortSet_lblBaudRate.AutoSize = true;
            this.COMPortSet_lblBaudRate.Location = new System.Drawing.Point(67, 88);
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
            this.cboPort.Location = new System.Drawing.Point(188, 43);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(121, 21);
            this.cboPort.TabIndex = 1;
            // 
            // COMPortSet_lblCOMPort
            // 
            this.COMPortSet_lblCOMPort.AutoSize = true;
            this.COMPortSet_lblCOMPort.Location = new System.Drawing.Point(67, 46);
            this.COMPortSet_lblCOMPort.Name = "COMPortSet_lblCOMPort";
            this.COMPortSet_lblCOMPort.Size = new System.Drawing.Size(53, 13);
            this.COMPortSet_lblCOMPort.TabIndex = 0;
            this.COMPortSet_lblCOMPort.Text = "COM Port";
            this.COMPortSet_lblCOMPort.TranslationKey = null;
            // 
            // PortSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(404, 262);
            this.Controls.Add(this.groupBox1);
            this.Name = "PortSettingForm";
            this.StatusMessage = "";
            this.Text = "COM Port Settings";
            this.Load += new System.EventHandler(this.PortSettingForm_Load);
            this.Activated += new System.EventHandler(this.PortSettingForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PortSettingForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private CAB.UI.Controls.CABLabel lblCommMode;
        private System.Windows.Forms.ComboBox cboCommMode;
        private CAB.UI.Controls.CABButton lngbCancel;
        private CAB.UI.Controls.CABButton lngbSave;
        internal System.Windows.Forms.ComboBox cboBaudRate;
        private CAB.UI.Controls.CABLabel COMPortSet_lblBaudRate;
        internal System.Windows.Forms.ComboBox cboPort;
        private CAB.UI.Controls.CABLabel COMPortSet_lblCOMPort;
    }
}
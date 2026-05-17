namespace DLMS_Final
{
    partial class frmBaudRateSelector
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblCOMPort = new System.Windows.Forms.Label();
            this.cmbCOMPorts = new System.Windows.Forms.ComboBox();
            this.cmbInitialBaudRate = new System.Windows.Forms.ComboBox();
            this.lblSelectBaudRate = new System.Windows.Forms.Label();
            this.pnlCOMPort = new System.Windows.Forms.Panel();
            this.gbModem = new System.Windows.Forms.GroupBox();
            this.pnlCOMPort.SuspendLayout();
            this.gbModem.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(163, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(54, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblCOMPort
            // 
            this.lblCOMPort.AutoSize = true;
            this.lblCOMPort.Location = new System.Drawing.Point(19, 23);
            this.lblCOMPort.Name = "lblCOMPort";
            this.lblCOMPort.Size = new System.Drawing.Size(53, 13);
            this.lblCOMPort.TabIndex = 6;
            this.lblCOMPort.Text = "COM Port";
            // 
            // cmbCOMPorts
            // 
            this.cmbCOMPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCOMPorts.FormattingEnabled = true;
            this.cmbCOMPorts.Location = new System.Drawing.Point(97, 20);
            this.cmbCOMPorts.Name = "cmbCOMPorts";
            this.cmbCOMPorts.Size = new System.Drawing.Size(83, 21);
            this.cmbCOMPorts.TabIndex = 5;
            // 
            // cmbInitialBaudRate
            // 
            this.cmbInitialBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInitialBaudRate.FormattingEnabled = true;
            this.cmbInitialBaudRate.Items.AddRange(new object[] {
            "300",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cmbInitialBaudRate.Location = new System.Drawing.Point(96, 21);
            this.cmbInitialBaudRate.Name = "cmbInitialBaudRate";
            this.cmbInitialBaudRate.Size = new System.Drawing.Size(121, 21);
            this.cmbInitialBaudRate.TabIndex = 1;
            // 
            // lblSelectBaudRate
            // 
            this.lblSelectBaudRate.AutoSize = true;
            this.lblSelectBaudRate.Location = new System.Drawing.Point(20, 24);
            this.lblSelectBaudRate.Name = "lblSelectBaudRate";
            this.lblSelectBaudRate.Size = new System.Drawing.Size(58, 13);
            this.lblSelectBaudRate.TabIndex = 4;
            this.lblSelectBaudRate.Text = "Baud Rate";
            // 
            // pnlCOMPort
            // 
            this.pnlCOMPort.Controls.Add(this.lblCOMPort);
            this.pnlCOMPort.Controls.Add(this.cmbCOMPorts);
            this.pnlCOMPort.Location = new System.Drawing.Point(6, 53);
            this.pnlCOMPort.Name = "pnlCOMPort";
            this.pnlCOMPort.Size = new System.Drawing.Size(212, 55);
            this.pnlCOMPort.TabIndex = 5;
            this.pnlCOMPort.Visible = false;
            // 
            // gbModem
            // 
            this.gbModem.Controls.Add(this.lblSelectBaudRate);
            this.gbModem.Controls.Add(this.pnlCOMPort);
            this.gbModem.Controls.Add(this.cmbInitialBaudRate);
            this.gbModem.Location = new System.Drawing.Point(31, 12);
            this.gbModem.Name = "gbModem";
            this.gbModem.Size = new System.Drawing.Size(229, 121);
            this.gbModem.TabIndex = 6;
            this.gbModem.TabStop = false;
            this.gbModem.Text = "Modem";
            // 
            // frmBaudRateSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 175);
            this.ControlBox = false;
            this.Controls.Add(this.gbModem);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "frmBaudRateSelector";
            this.Text = "Modem Baud Rate";
            this.Load += new System.EventHandler(this.frmBaudRateSelector_Load);
            this.pnlCOMPort.ResumeLayout(false);
            this.pnlCOMPort.PerformLayout();
            this.gbModem.ResumeLayout(false);
            this.gbModem.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblCOMPort;
        private System.Windows.Forms.ComboBox cmbCOMPorts;
        private System.Windows.Forms.Label lblSelectBaudRate;
        private System.Windows.Forms.ComboBox cmbInitialBaudRate;
        private System.Windows.Forms.Panel pnlCOMPort;
        private System.Windows.Forms.GroupBox gbModem;
    }
}
namespace DLMS_Final
{
    partial class SerialSettings
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdBtnRJPort = new System.Windows.Forms.RadioButton();
            this.rdBtnOpticalPort = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdBtnModeE = new System.Windows.Forms.RadioButton();
            this.rdBtnDirectHDLC = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbAvailableSerialPort = new System.Windows.Forms.ComboBox();
            this.COMPortSet_lblCOMPort = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.66253F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.33747F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.318681F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.68132F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(453, 241);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.cmbAvailableSerialPort);
            this.groupBox1.Controls.Add(this.COMPortSet_lblCOMPort);
            this.groupBox1.Location = new System.Drawing.Point(49, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 196);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "COM Port Settings";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdBtnRJPort);
            this.groupBox2.Controls.Add(this.rdBtnOpticalPort);
            this.groupBox2.Location = new System.Drawing.Point(77, 290);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 62);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Communication Port";
            this.groupBox2.Visible = false;
            // 
            // rdBtnRJPort
            // 
            this.rdBtnRJPort.AutoSize = true;
            this.rdBtnRJPort.Checked = true;
            this.rdBtnRJPort.Location = new System.Drawing.Point(28, 29);
            this.rdBtnRJPort.Name = "rdBtnRJPort";
            this.rdBtnRJPort.Size = new System.Drawing.Size(61, 17);
            this.rdBtnRJPort.TabIndex = 6;
            this.rdBtnRJPort.TabStop = true;
            this.rdBtnRJPort.Text = "RS-232";
            this.rdBtnRJPort.UseVisualStyleBackColor = true;
            // 
            // rdBtnOpticalPort
            // 
            this.rdBtnOpticalPort.AutoSize = true;
            this.rdBtnOpticalPort.Location = new System.Drawing.Point(129, 29);
            this.rdBtnOpticalPort.Name = "rdBtnOpticalPort";
            this.rdBtnOpticalPort.Size = new System.Drawing.Size(80, 17);
            this.rdBtnOpticalPort.TabIndex = 7;
            this.rdBtnOpticalPort.Text = "Optical Port";
            this.rdBtnOpticalPort.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdBtnModeE);
            this.groupBox3.Controls.Add(this.rdBtnDirectHDLC);
            this.groupBox3.Location = new System.Drawing.Point(89, 200);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(259, 63);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Communication Mode";
            this.groupBox3.Visible = false;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // rdBtnModeE
            // 
            this.rdBtnModeE.AutoSize = true;
            this.rdBtnModeE.Location = new System.Drawing.Point(30, 29);
            this.rdBtnModeE.Name = "rdBtnModeE";
            this.rdBtnModeE.Size = new System.Drawing.Size(62, 17);
            this.rdBtnModeE.TabIndex = 10;
            this.rdBtnModeE.Text = "Mode-E";
            this.rdBtnModeE.UseVisualStyleBackColor = true;
            // 
            // rdBtnDirectHDLC
            // 
            this.rdBtnDirectHDLC.AutoSize = true;
            this.rdBtnDirectHDLC.Checked = true;
            this.rdBtnDirectHDLC.Location = new System.Drawing.Point(129, 29);
            this.rdBtnDirectHDLC.Name = "rdBtnDirectHDLC";
            this.rdBtnDirectHDLC.Size = new System.Drawing.Size(85, 17);
            this.rdBtnDirectHDLC.TabIndex = 9;
            this.rdBtnDirectHDLC.TabStop = true;
            this.rdBtnDirectHDLC.Text = "Direct-HDLC";
            this.rdBtnDirectHDLC.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(173, 132);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 28);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(89, 132);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 28);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbAvailableSerialPort
            // 
            this.cmbAvailableSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAvailableSerialPort.FormattingEnabled = true;
            this.cmbAvailableSerialPort.Location = new System.Drawing.Point(150, 69);
            this.cmbAvailableSerialPort.Name = "cmbAvailableSerialPort";
            this.cmbAvailableSerialPort.Size = new System.Drawing.Size(139, 21);
            this.cmbAvailableSerialPort.TabIndex = 1;
            this.cmbAvailableSerialPort.SelectedIndexChanged += new System.EventHandler(this.cmbAvailableSerialPort_SelectedIndexChanged);
            // 
            // COMPortSet_lblCOMPort
            // 
            this.COMPortSet_lblCOMPort.AutoSize = true;
            this.COMPortSet_lblCOMPort.Location = new System.Drawing.Point(57, 69);
            this.COMPortSet_lblCOMPort.Name = "COMPortSet_lblCOMPort";
            this.COMPortSet_lblCOMPort.Size = new System.Drawing.Size(55, 13);
            this.COMPortSet_lblCOMPort.TabIndex = 0;
            this.COMPortSet_lblCOMPort.Text = "Serial Port";
            // 
            // SerialSettings
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(453, 241);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SerialSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "COM Port Settings";
            this.Load += new System.EventHandler(this.SerialSettings_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.ComboBox cmbAvailableSerialPort;
        private System.Windows.Forms.Label COMPortSet_lblCOMPort;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdBtnDirectHDLC;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdBtnRJPort;
        private System.Windows.Forms.RadioButton rdBtnOpticalPort;
        private System.Windows.Forms.RadioButton rdBtnModeE;
    }
}
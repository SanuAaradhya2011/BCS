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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbGeneral = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnTOUBrowse = new System.Windows.Forms.Button();
            this.rbtnTOUDefault = new System.Windows.Forms.RadioButton();
            this.rbtnTOUCustom = new System.Windows.Forms.RadioButton();
            this.txtDefaultTOULocation = new System.Windows.Forms.TextBox();
            this.txtCustomTOULocation = new System.Windows.Forms.TextBox();
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
            this.rboFNC3 = new System.Windows.Forms.RadioButton();
            this.rboFNC2 = new System.Windows.Forms.RadioButton();
            this.rboFNC1 = new System.Windows.Forms.RadioButton();
            this.tbDashBoard = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chkSelectAll = new System.Windows.Forms.CheckBox();
            this.chklstDashBoard = new System.Windows.Forms.CheckedListBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbGeneral.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tbDashBoard.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // lngbSave
            // 
            this.lngbSave.Location = new System.Drawing.Point(326, 439);
            this.lngbSave.Name = "lngbSave";
            this.lngbSave.Size = new System.Drawing.Size(55, 25);
            this.lngbSave.TabIndex = 4;
            this.lngbSave.Text = "Save";
            this.lngbSave.TranslationKey = null;
            this.lngbSave.UseVisualStyleBackColor = true;
            this.lngbSave.Click += new System.EventHandler(this.lngbSave_Click);
            // 
            // lngbCancel
            // 
            this.lngbCancel.Location = new System.Drawing.Point(387, 439);
            this.lngbCancel.Name = "lngbCancel";
            this.lngbCancel.Size = new System.Drawing.Size(55, 25);
            this.lngbCancel.TabIndex = 5;
            this.lngbCancel.Text = "Cancel";
            this.lngbCancel.TranslationKey = null;
            this.lngbCancel.UseVisualStyleBackColor = true;
            this.lngbCancel.Click += new System.EventHandler(this.lngbCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.lngbCancel);
            this.groupBox1.Controls.Add(this.lngbSave);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 470);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbGeneral);
            this.tabControl1.Controls.Add(this.tbDashBoard);
            this.tabControl1.Location = new System.Drawing.Point(6, 19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(447, 414);
            this.tabControl1.TabIndex = 6;
            // 
            // tbGeneral
            // 
            this.tbGeneral.Controls.Add(this.groupBox5);
            this.tbGeneral.Controls.Add(this.groupBox4);
            this.tbGeneral.Controls.Add(this.groupBox3);
            this.tbGeneral.Controls.Add(this.groupBox2);
            this.tbGeneral.Location = new System.Drawing.Point(4, 22);
            this.tbGeneral.Name = "tbGeneral";
            this.tbGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tbGeneral.Size = new System.Drawing.Size(439, 388);
            this.tbGeneral.TabIndex = 0;
            this.tbGeneral.Text = "General";
            this.tbGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnTOUBrowse);
            this.groupBox5.Controls.Add(this.rbtnTOUDefault);
            this.groupBox5.Controls.Add(this.rbtnTOUCustom);
            this.groupBox5.Controls.Add(this.txtDefaultTOULocation);
            this.groupBox5.Controls.Add(this.txtCustomTOULocation);
            this.groupBox5.Location = new System.Drawing.Point(7, 174);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(425, 80);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "TOU  File Location";
            // 
            // btnTOUBrowse
            // 
            this.btnTOUBrowse.Location = new System.Drawing.Point(307, 39);
            this.btnTOUBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnTOUBrowse.Name = "btnTOUBrowse";
            this.btnTOUBrowse.Size = new System.Drawing.Size(56, 24);
            this.btnTOUBrowse.TabIndex = 10;
            this.btnTOUBrowse.Text = "Browse";
            this.btnTOUBrowse.UseVisualStyleBackColor = true;
            this.btnTOUBrowse.Click += new System.EventHandler(this.btnTOUBrowse_Click);
            // 
            // rbtnTOUDefault
            // 
            this.rbtnTOUDefault.AutoSize = true;
            this.rbtnTOUDefault.Checked = true;
            this.rbtnTOUDefault.Location = new System.Drawing.Point(51, 19);
            this.rbtnTOUDefault.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnTOUDefault.Name = "rbtnTOUDefault";
            this.rbtnTOUDefault.Size = new System.Drawing.Size(59, 17);
            this.rbtnTOUDefault.TabIndex = 7;
            this.rbtnTOUDefault.TabStop = true;
            this.rbtnTOUDefault.Text = "Default";
            this.rbtnTOUDefault.UseVisualStyleBackColor = true;
            this.rbtnTOUDefault.CheckedChanged += new System.EventHandler(this.rbtnTOUDefault_CheckedChanged);
            // 
            // rbtnTOUCustom
            // 
            this.rbtnTOUCustom.AutoSize = true;
            this.rbtnTOUCustom.Location = new System.Drawing.Point(51, 41);
            this.rbtnTOUCustom.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnTOUCustom.Name = "rbtnTOUCustom";
            this.rbtnTOUCustom.Size = new System.Drawing.Size(60, 17);
            this.rbtnTOUCustom.TabIndex = 8;
            this.rbtnTOUCustom.Text = "Custom";
            this.rbtnTOUCustom.UseVisualStyleBackColor = true;
            this.rbtnTOUCustom.CheckedChanged += new System.EventHandler(this.rbtnTOUCustom_CheckedChanged);
            // 
            // txtDefaultTOULocation
            // 
            this.txtDefaultTOULocation.BackColor = System.Drawing.Color.White;
            this.txtDefaultTOULocation.Location = new System.Drawing.Point(121, 18);
            this.txtDefaultTOULocation.Margin = new System.Windows.Forms.Padding(2);
            this.txtDefaultTOULocation.Name = "txtDefaultTOULocation";
            this.txtDefaultTOULocation.ReadOnly = true;
            this.txtDefaultTOULocation.Size = new System.Drawing.Size(240, 20);
            this.txtDefaultTOULocation.TabIndex = 9;
            // 
            // txtCustomTOULocation
            // 
            this.txtCustomTOULocation.BackColor = System.Drawing.Color.White;
            this.txtCustomTOULocation.Location = new System.Drawing.Point(121, 42);
            this.txtCustomTOULocation.Margin = new System.Windows.Forms.Padding(2);
            this.txtCustomTOULocation.Name = "txtCustomTOULocation";
            this.txtCustomTOULocation.ReadOnly = true;
            this.txtCustomTOULocation.Size = new System.Drawing.Size(182, 20);
            this.txtCustomTOULocation.TabIndex = 9;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cboDate);
            this.groupBox4.Controls.Add(this.COMPortSet_lblCOMPort);
            this.groupBox4.Location = new System.Drawing.Point(7, 14);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(425, 54);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Application Date Format";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // cboDate
            // 
            this.cboDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDate.FormattingEnabled = true;
            this.cboDate.Location = new System.Drawing.Point(131, 18);
            this.cboDate.Name = "cboDate";
            this.cboDate.Size = new System.Drawing.Size(121, 21);
            this.cboDate.TabIndex = 1;
            this.cboDate.SelectedIndexChanged += new System.EventHandler(this.cboDate_SelectedIndexChanged);
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtDefaultCABLocation);
            this.groupBox3.Controls.Add(this.btnBrowse);
            this.groupBox3.Controls.Add(this.rbtnDefault);
            this.groupBox3.Controls.Add(this.rbtnCustom);
            this.groupBox3.Controls.Add(this.txtCustomCABLocation);
            this.groupBox3.Location = new System.Drawing.Point(7, 82);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(425, 78);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CAB File Location";
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
            this.btnBrowse.UseVisualStyleBackColor = true;
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
            this.groupBox2.Controls.Add(this.rboFNC3);
            this.groupBox2.Controls.Add(this.rboFNC2);
            this.groupBox2.Controls.Add(this.rboFNC1);
            this.groupBox2.Location = new System.Drawing.Point(7, 267);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(425, 104);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Format";
            // 
            // rboFNC3
            // 
            this.rboFNC3.AutoSize = true;
            this.rboFNC3.Location = new System.Drawing.Point(47, 73);
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
            this.rboFNC2.Location = new System.Drawing.Point(47, 47);
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
            this.rboFNC1.Checked = true;
            this.rboFNC1.Location = new System.Drawing.Point(47, 21);
            this.rboFNC1.Margin = new System.Windows.Forms.Padding(2);
            this.rboFNC1.Name = "rboFNC1";
            this.rboFNC1.Size = new System.Drawing.Size(180, 17);
            this.rboFNC1.TabIndex = 8;
            this.rboFNC1.TabStop = true;
            this.rboFNC1.Text = "System Generated Default Name";
            this.rboFNC1.UseVisualStyleBackColor = true;
            // 
            // tbDashBoard
            // 
            this.tbDashBoard.Controls.Add(this.groupBox6);
            this.tbDashBoard.Location = new System.Drawing.Point(4, 22);
            this.tbDashBoard.Name = "tbDashBoard";
            this.tbDashBoard.Padding = new System.Windows.Forms.Padding(3);
            this.tbDashBoard.Size = new System.Drawing.Size(439, 388);
            this.tbDashBoard.TabIndex = 1;
            this.tbDashBoard.Text = "Dash Board";
            this.tbDashBoard.UseVisualStyleBackColor = true;
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(473, 490);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GeneralConfiguration";
            this.StatusMessage = "";
            this.Text = "System Configuration";
            this.Load += new System.EventHandler(this.PortSettingForm_Load);
            this.Activated += new System.EventHandler(this.GeneralConfiguration_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GeneralConfiguration_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbGeneral.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbGeneral;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnTOUBrowse;
        private System.Windows.Forms.RadioButton rbtnTOUDefault;
        private System.Windows.Forms.RadioButton rbtnTOUCustom;
        private System.Windows.Forms.TextBox txtDefaultTOULocation;
        private System.Windows.Forms.TextBox txtCustomTOULocation;
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

    }
}
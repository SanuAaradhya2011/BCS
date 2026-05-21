namespace CAB.UI
{
    partial class GSMScheduleInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbtnMonthly = new System.Windows.Forms.RadioButton();
            this.rbtnWeekly = new System.Windows.Forms.RadioButton();
            this.rbtnDaily = new System.Windows.Forms.RadioButton();
            this.cboWeekDay = new System.Windows.Forms.ComboBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.rboInactive = new System.Windows.Forms.RadioButton();
            this.rboActive = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.clbParameter = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtScheduleName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboHr = new System.Windows.Forms.ComboBox();
            this.cboMi = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(299, 406);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 31);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cboMi);
            this.groupBox1.Controls.Add(this.cboHr);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.cboWeekDay);
            this.groupBox1.Controls.Add(this.lblPeriod);
            this.groupBox1.Controls.Add(this.rboInactive);
            this.groupBox1.Controls.Add(this.rboActive);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.clbParameter);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpDate);
            this.groupBox1.Controls.Add(this.txtScheduleName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(408, 397);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Schedule Information";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbtnMonthly);
            this.panel1.Controls.Add(this.rbtnWeekly);
            this.panel1.Controls.Add(this.rbtnDaily);
            this.panel1.Location = new System.Drawing.Point(133, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 32);
            this.panel1.TabIndex = 22;
            // 
            // rbtnMonthly
            // 
            this.rbtnMonthly.AutoSize = true;
            this.rbtnMonthly.Location = new System.Drawing.Point(173, 8);
            this.rbtnMonthly.Name = "rbtnMonthly";
            this.rbtnMonthly.Size = new System.Drawing.Size(78, 21);
            this.rbtnMonthly.TabIndex = 20;
            this.rbtnMonthly.Text = "Monthly";
            this.rbtnMonthly.UseVisualStyleBackColor = true;
            this.rbtnMonthly.CheckedChanged += new System.EventHandler(this.rbtnMonthly_CheckedChanged);
            // 
            // rbtnWeekly
            // 
            this.rbtnWeekly.AutoSize = true;
            this.rbtnWeekly.Location = new System.Drawing.Point(92, 6);
            this.rbtnWeekly.Name = "rbtnWeekly";
            this.rbtnWeekly.Size = new System.Drawing.Size(75, 21);
            this.rbtnWeekly.TabIndex = 19;
            this.rbtnWeekly.Text = "Weekly";
            this.rbtnWeekly.UseVisualStyleBackColor = true;
            this.rbtnWeekly.CheckedChanged += new System.EventHandler(this.rbtnWeekly_CheckedChanged);
            // 
            // rbtnDaily
            // 
            this.rbtnDaily.AutoSize = true;
            this.rbtnDaily.Checked = true;
            this.rbtnDaily.Location = new System.Drawing.Point(20, 8);
            this.rbtnDaily.Name = "rbtnDaily";
            this.rbtnDaily.Size = new System.Drawing.Size(60, 21);
            this.rbtnDaily.TabIndex = 18;
            this.rbtnDaily.TabStop = true;
            this.rbtnDaily.Text = "Daily";
            this.rbtnDaily.UseVisualStyleBackColor = true;
            this.rbtnDaily.CheckedChanged += new System.EventHandler(this.rbtnDaily_CheckedChanged);
            // 
            // cboWeekDay
            // 
            this.cboWeekDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWeekDay.FormattingEnabled = true;
            this.cboWeekDay.Location = new System.Drawing.Point(153, 104);
            this.cboWeekDay.Margin = new System.Windows.Forms.Padding(4);
            this.cboWeekDay.Name = "cboWeekDay";
            this.cboWeekDay.Size = new System.Drawing.Size(131, 24);
            this.cboWeekDay.TabIndex = 17;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(6, 104);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(81, 17);
            this.lblPeriod.TabIndex = 16;
            this.lblPeriod.Text = "Days Name";
            // 
            // rboInactive
            // 
            this.rboInactive.AutoSize = true;
            this.rboInactive.Location = new System.Drawing.Point(223, 367);
            this.rboInactive.Name = "rboInactive";
            this.rboInactive.Size = new System.Drawing.Size(77, 21);
            this.rboInactive.TabIndex = 12;
            this.rboInactive.Text = "Inactive";
            this.rboInactive.UseVisualStyleBackColor = true;
            // 
            // rboActive
            // 
            this.rboActive.AutoSize = true;
            this.rboActive.Checked = true;
            this.rboActive.Location = new System.Drawing.Point(154, 367);
            this.rboActive.Name = "rboActive";
            this.rboActive.Size = new System.Drawing.Size(67, 21);
            this.rboActive.TabIndex = 11;
            this.rboActive.TabStop = true;
            this.rboActive.Text = "Active";
            this.rboActive.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 17);
            this.label4.TabIndex = 15;
            this.label4.Text = "Reading Parameters";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 369);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "Schedule Status";
            // 
            // clbParameter
            // 
            this.clbParameter.ColumnWidth = 1000;
            this.clbParameter.HorizontalExtent = 1000;
            this.clbParameter.Items.AddRange(new object[] {
            "General and Billing",
            "Tamper",
            "Load survey",
            "Transaction",
            "Phasor",
            "Fraud Energy",
            "Daily Load Profile",
            "DTM Load Survey"});
            this.clbParameter.Location = new System.Drawing.Point(153, 211);
            this.clbParameter.Margin = new System.Windows.Forms.Padding(0);
            this.clbParameter.Name = "clbParameter";
            this.clbParameter.Size = new System.Drawing.Size(209, 140);
            this.clbParameter.TabIndex = 14;
            this.clbParameter.ThreeDCheckBoxes = true;
            this.clbParameter.UseCompatibleTextRendering = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Period";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(154, 140);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(128, 22);
            this.dtpDate.TabIndex = 11;
            this.dtpDate.Value = new System.DateTime(2010, 8, 7, 0, 0, 0, 0);
            // 
            // txtScheduleName
            // 
            this.txtScheduleName.Location = new System.Drawing.Point(156, 36);
            this.txtScheduleName.Name = "txtScheduleName";
            this.txtScheduleName.Size = new System.Drawing.Size(206, 22);
            this.txtScheduleName.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "Schedule Time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Schedule Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Schedule Date";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(176, 406);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(112, 31);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboHr
            // 
            this.cboHr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHr.FormattingEnabled = true;
            this.cboHr.Location = new System.Drawing.Point(153, 176);
            this.cboHr.Margin = new System.Windows.Forms.Padding(4);
            this.cboHr.Name = "cboHr";
            this.cboHr.Size = new System.Drawing.Size(60, 24);
            this.cboHr.TabIndex = 23;
            // 
            // cboMi
            // 
            this.cboMi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMi.FormattingEnabled = true;
            this.cboMi.Location = new System.Drawing.Point(263, 176);
            this.cboMi.Margin = new System.Windows.Forms.Padding(4);
            this.cboMi.Name = "cboMi";
            this.cboMi.Size = new System.Drawing.Size(60, 24);
            this.cboMi.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(220, 179);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 17);
            this.label7.TabIndex = 25;
            this.label7.Text = "(hh)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(330, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 17);
            this.label8.TabIndex = 26;
            this.label8.Text = "(mm)";
            // 
            // GSMScheduleInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Name = "GSMScheduleInfo";
            this.Size = new System.Drawing.Size(427, 451);
            this.Load += new System.EventHandler(this.GSMScheduleInfo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbtnMonthly;
        private System.Windows.Forms.RadioButton rbtnWeekly;
        private System.Windows.Forms.RadioButton rbtnDaily;
        internal System.Windows.Forms.ComboBox cboWeekDay;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.RadioButton rboInactive;
        private System.Windows.Forms.RadioButton rboActive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckedListBox clbParameter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox txtScheduleName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.ComboBox cboMi;
        internal System.Windows.Forms.ComboBox cboHr;

    }
}

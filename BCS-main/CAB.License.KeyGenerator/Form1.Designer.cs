namespace CAB.License.KeyGenerator
{
    partial class KeyGenerator
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
            this.label3 = new System.Windows.Forms.Label();
            this.cmbBCSVersion = new System.Windows.Forms.ComboBox();
            this.grbUserRights = new System.Windows.Forms.GroupBox();
            this.chkAdvanceProgramming = new System.Windows.Forms.CheckBox();
            this.chkDataExportImport = new System.Windows.Forms.CheckBox();
            this.chkDataReadout = new System.Windows.Forms.CheckBox();
            this.chkSchedule = new System.Windows.Forms.CheckBox();
            this.chkProgramming = new System.Windows.Forms.CheckBox();
            this.chkDataArchive = new System.Windows.Forms.CheckBox();
            this.chkReportsView = new System.Windows.Forms.CheckBox();
            this.chkUserAdministrator = new System.Windows.Forms.CheckBox();
            this.chkDefinition = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.grbUserRights.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbBCSVersion);
            this.groupBox1.Controls.Add(this.grbUserRights);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(5, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(645, 275);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Browse File and Generate Key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "BCS Version";
            // 
            // cmbBCSVersion
            // 
            this.cmbBCSVersion.FormattingEnabled = true;
            this.cmbBCSVersion.Items.AddRange(new object[] {
            "BCS Released Version",
            "BCS New Releases"});
            this.cmbBCSVersion.Location = new System.Drawing.Point(105, 82);
            this.cmbBCSVersion.Name = "cmbBCSVersion";
            this.cmbBCSVersion.Size = new System.Drawing.Size(218, 21);
            this.cmbBCSVersion.TabIndex = 19;
            this.cmbBCSVersion.SelectedIndexChanged += new System.EventHandler(this.cmbBCSVersion_SelectedIndexChanged);
            // 
            // grbUserRights
            // 
            this.grbUserRights.Controls.Add(this.chkAdvanceProgramming);
            this.grbUserRights.Controls.Add(this.chkDataExportImport);
            this.grbUserRights.Controls.Add(this.chkDataReadout);
            this.grbUserRights.Controls.Add(this.chkSchedule);
            this.grbUserRights.Controls.Add(this.chkProgramming);
            this.grbUserRights.Controls.Add(this.chkDataArchive);
            this.grbUserRights.Controls.Add(this.chkReportsView);
            this.grbUserRights.Controls.Add(this.chkUserAdministrator);
            this.grbUserRights.Controls.Add(this.chkDefinition);
            this.grbUserRights.Location = new System.Drawing.Point(29, 115);
            this.grbUserRights.Name = "grbUserRights";
            this.grbUserRights.Size = new System.Drawing.Size(606, 120);
            this.grbUserRights.TabIndex = 18;
            this.grbUserRights.TabStop = false;
            this.grbUserRights.Text = "User rights";
            // 
            // chkAdvanceProgramming
            // 
            this.chkAdvanceProgramming.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkAdvanceProgramming.AutoSize = true;
            this.chkAdvanceProgramming.Location = new System.Drawing.Point(266, 76);
            this.chkAdvanceProgramming.Name = "chkAdvanceProgramming";
            this.chkAdvanceProgramming.Size = new System.Drawing.Size(230, 17);
            this.chkAdvanceProgramming.TabIndex = 9;
            this.chkAdvanceProgramming.Text = "Advance Programming(Tamper Icon Reset)";
            this.chkAdvanceProgramming.UseVisualStyleBackColor = true;
            // 
            // chkDataExportImport
            // 
            this.chkDataExportImport.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkDataExportImport.AutoSize = true;
            this.chkDataExportImport.Checked = true;
            this.chkDataExportImport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDataExportImport.Location = new System.Drawing.Point(143, 76);
            this.chkDataExportImport.Name = "chkDataExportImport";
            this.chkDataExportImport.Size = new System.Drawing.Size(116, 17);
            this.chkDataExportImport.TabIndex = 8;
            this.chkDataExportImport.Text = "Data Export/Import";
            this.chkDataExportImport.UseVisualStyleBackColor = true;
            // 
            // chkDataReadout
            // 
            this.chkDataReadout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkDataReadout.AutoSize = true;
            this.chkDataReadout.Checked = true;
            this.chkDataReadout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDataReadout.Location = new System.Drawing.Point(266, 28);
            this.chkDataReadout.Name = "chkDataReadout";
            this.chkDataReadout.Size = new System.Drawing.Size(93, 17);
            this.chkDataReadout.TabIndex = 5;
            this.chkDataReadout.Text = "Data Readout";
            this.chkDataReadout.UseVisualStyleBackColor = true;
            // 
            // chkSchedule
            // 
            this.chkSchedule.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkSchedule.AutoSize = true;
            this.chkSchedule.Location = new System.Drawing.Point(14, 52);
            this.chkSchedule.Name = "chkSchedule";
            this.chkSchedule.Size = new System.Drawing.Size(71, 17);
            this.chkSchedule.TabIndex = 2;
            this.chkSchedule.Text = "Schedule";
            this.chkSchedule.UseVisualStyleBackColor = true;
            // 
            // chkProgramming
            // 
            this.chkProgramming.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkProgramming.AutoSize = true;
            this.chkProgramming.Location = new System.Drawing.Point(266, 53);
            this.chkProgramming.Name = "chkProgramming";
            this.chkProgramming.Size = new System.Drawing.Size(87, 17);
            this.chkProgramming.TabIndex = 1;
            this.chkProgramming.Text = "Programming";
            this.chkProgramming.UseVisualStyleBackColor = true;
            // 
            // chkDataArchive
            // 
            this.chkDataArchive.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkDataArchive.AutoSize = true;
            this.chkDataArchive.Checked = true;
            this.chkDataArchive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDataArchive.Location = new System.Drawing.Point(143, 53);
            this.chkDataArchive.Name = "chkDataArchive";
            this.chkDataArchive.Size = new System.Drawing.Size(88, 17);
            this.chkDataArchive.TabIndex = 4;
            this.chkDataArchive.Text = "Data Archive";
            this.chkDataArchive.UseVisualStyleBackColor = true;
            // 
            // chkReportsView
            // 
            this.chkReportsView.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkReportsView.AutoSize = true;
            this.chkReportsView.Checked = true;
            this.chkReportsView.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReportsView.Location = new System.Drawing.Point(13, 75);
            this.chkReportsView.Name = "chkReportsView";
            this.chkReportsView.Size = new System.Drawing.Size(89, 17);
            this.chkReportsView.TabIndex = 7;
            this.chkReportsView.Text = "Reports View";
            this.chkReportsView.UseVisualStyleBackColor = true;
            // 
            // chkUserAdministrator
            // 
            this.chkUserAdministrator.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkUserAdministrator.AutoSize = true;
            this.chkUserAdministrator.Location = new System.Drawing.Point(14, 29);
            this.chkUserAdministrator.Name = "chkUserAdministrator";
            this.chkUserAdministrator.Size = new System.Drawing.Size(111, 17);
            this.chkUserAdministrator.TabIndex = 0;
            this.chkUserAdministrator.Text = "User Administrator";
            this.chkUserAdministrator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkUserAdministrator.UseVisualStyleBackColor = true;
            // 
            // chkDefinition
            // 
            this.chkDefinition.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkDefinition.AutoSize = true;
            this.chkDefinition.Checked = true;
            this.chkDefinition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDefinition.Location = new System.Drawing.Point(143, 28);
            this.chkDefinition.Name = "chkDefinition";
            this.chkDefinition.Size = new System.Drawing.Size(70, 17);
            this.chkDefinition.TabIndex = 6;
            this.chkDefinition.Text = "Definition";
            this.chkDefinition.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Key";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox2.Location = new System.Drawing.Point(105, 51);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(536, 20);
            this.textBox2.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(105, 241);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(218, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Generate Key";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(353, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 25);
            this.button1.TabIndex = 2;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select File";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(105, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(218, 20);
            this.textBox1.TabIndex = 0;
            // 
            // KeyGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 298);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "KeyGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Key Generator";
            this.Load += new System.EventHandler(this.KeyGenerator_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbUserRights.ResumeLayout(false);
            this.grbUserRights.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox grbUserRights;
        private System.Windows.Forms.CheckBox chkDataExportImport;
        private System.Windows.Forms.CheckBox chkDataReadout;
        private System.Windows.Forms.CheckBox chkSchedule;
        private System.Windows.Forms.CheckBox chkProgramming;
        private System.Windows.Forms.CheckBox chkDataArchive;
        private System.Windows.Forms.CheckBox chkReportsView;
        private System.Windows.Forms.CheckBox chkUserAdministrator;
        private System.Windows.Forms.CheckBox chkDefinition;
        private System.Windows.Forms.CheckBox chkAdvanceProgramming;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbBCSVersion;


    }
}


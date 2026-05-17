namespace CABAppControl
{
    partial class MDWithIP
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
            this.label1 = new System.Windows.Forms.Label();
            this.grpMDWithIP = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbkVADemandSubInt = new System.Windows.Forms.ComboBox();
            this.cmbkVADemandInt = new System.Windows.Forms.ComboBox();
            this.cmbkVADemandType = new System.Windows.Forms.ComboBox();
            this.cmbkWDemandSubInt = new System.Windows.Forms.ComboBox();
            this.cmbkWDemandInt = new System.Windows.Forms.ComboBox();
            this.cmbkWDemandType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpMDWithIP.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Demand Parameter :\r\n";
            // 
            // grpMDWithIP
            // 
            this.grpMDWithIP.Controls.Add(this.label10);
            this.grpMDWithIP.Controls.Add(this.label9);
            this.grpMDWithIP.Controls.Add(this.cmbkVADemandSubInt);
            this.grpMDWithIP.Controls.Add(this.cmbkVADemandInt);
            this.grpMDWithIP.Controls.Add(this.cmbkVADemandType);
            this.grpMDWithIP.Controls.Add(this.cmbkWDemandSubInt);
            this.grpMDWithIP.Controls.Add(this.cmbkWDemandInt);
            this.grpMDWithIP.Controls.Add(this.cmbkWDemandType);
            this.grpMDWithIP.Controls.Add(this.label5);
            this.grpMDWithIP.Controls.Add(this.label6);
            this.grpMDWithIP.Controls.Add(this.label7);
            this.grpMDWithIP.Controls.Add(this.label8);
            this.grpMDWithIP.Controls.Add(this.label4);
            this.grpMDWithIP.Controls.Add(this.label3);
            this.grpMDWithIP.Controls.Add(this.label2);
            this.grpMDWithIP.Controls.Add(this.label1);
            this.grpMDWithIP.Location = new System.Drawing.Point(7, -1);
            this.grpMDWithIP.Name = "grpMDWithIP";
            this.grpMDWithIP.Size = new System.Drawing.Size(605, 250);
            this.grpMDWithIP.TabIndex = 1;
            this.grpMDWithIP.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Location = new System.Drawing.Point(492, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 15);
            this.label10.TabIndex = 15;
            this.label10.Text = "kVA";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(189, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 15);
            this.label9.TabIndex = 14;
            this.label9.Text = "kW";
            // 
            // cmbkVADemandSubInt
            // 
            this.cmbkVADemandSubInt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbkVADemandSubInt.FormattingEnabled = true;
            this.cmbkVADemandSubInt.Items.AddRange(new object[] {
            "5",
            "10",
            "15"});
            this.cmbkVADemandSubInt.Location = new System.Drawing.Point(460, 175);
            this.cmbkVADemandSubInt.Name = "cmbkVADemandSubInt";
            this.cmbkVADemandSubInt.Size = new System.Drawing.Size(99, 21);
            this.cmbkVADemandSubInt.TabIndex = 13;
            // 
            // cmbkVADemandInt
            // 
            this.cmbkVADemandInt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbkVADemandInt.FormattingEnabled = true;
            this.cmbkVADemandInt.Items.AddRange(new object[] {
            "15",
            "30",
            "60"});
            this.cmbkVADemandInt.Location = new System.Drawing.Point(460, 136);
            this.cmbkVADemandInt.Name = "cmbkVADemandInt";
            this.cmbkVADemandInt.Size = new System.Drawing.Size(99, 21);
            this.cmbkVADemandInt.TabIndex = 12;
            this.cmbkVADemandInt.SelectedIndexChanged += new System.EventHandler(this.cmbkVADemandInt_SelectedIndexChanged);
            // 
            // cmbkVADemandType
            // 
            this.cmbkVADemandType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbkVADemandType.FormattingEnabled = true;
            this.cmbkVADemandType.Items.AddRange(new object[] {
            "Block Demand",
            "Sliding Demand"});
            this.cmbkVADemandType.Location = new System.Drawing.Point(460, 97);
            this.cmbkVADemandType.Name = "cmbkVADemandType";
            this.cmbkVADemandType.Size = new System.Drawing.Size(100, 21);
            this.cmbkVADemandType.TabIndex = 11;
            this.cmbkVADemandType.SelectedIndexChanged += new System.EventHandler(this.cmbkVADemandType_SelectedIndexChanged);
            // 
            // cmbkWDemandSubInt
            // 
            this.cmbkWDemandSubInt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbkWDemandSubInt.FormattingEnabled = true;
            this.cmbkWDemandSubInt.Items.AddRange(new object[] {
            "5",
            "10",
            "15"});
            this.cmbkWDemandSubInt.Location = new System.Drawing.Point(156, 175);
            this.cmbkWDemandSubInt.Name = "cmbkWDemandSubInt";
            this.cmbkWDemandSubInt.Size = new System.Drawing.Size(99, 21);
            this.cmbkWDemandSubInt.TabIndex = 10;
            // 
            // cmbkWDemandInt
            // 
            this.cmbkWDemandInt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbkWDemandInt.FormattingEnabled = true;
            this.cmbkWDemandInt.Items.AddRange(new object[] {
            "15",
            "30",
            "60"});
            this.cmbkWDemandInt.Location = new System.Drawing.Point(156, 136);
            this.cmbkWDemandInt.Name = "cmbkWDemandInt";
            this.cmbkWDemandInt.Size = new System.Drawing.Size(99, 21);
            this.cmbkWDemandInt.TabIndex = 9;
            this.cmbkWDemandInt.SelectedIndexChanged += new System.EventHandler(this.cmbkWDemandInt_SelectedIndexChanged);
            // 
            // cmbkWDemandType
            // 
            this.cmbkWDemandType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbkWDemandType.FormattingEnabled = true;
            this.cmbkWDemandType.Items.AddRange(new object[] {
            "Block Demand",
            "Sliding Demand"});
            this.cmbkWDemandType.Location = new System.Drawing.Point(156, 97);
            this.cmbkWDemandType.Name = "cmbkWDemandType";
            this.cmbkWDemandType.Size = new System.Drawing.Size(100, 21);
            this.cmbkWDemandType.TabIndex = 8;
            this.cmbkWDemandType.SelectedIndexChanged += new System.EventHandler(this.cmbkWDemandType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(344, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Sub Time Interval\r\n";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(344, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Time Interval";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(344, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Demand Type";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(344, 61);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Demand Parameter  :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Sub Time Interval";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Time Interval";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Demand Type";
            // 
            // MDWithIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpMDWithIP);
            this.Name = "MDWithIP";
            this.Size = new System.Drawing.Size(618, 254);
            this.grpMDWithIP.ResumeLayout(false);
            this.grpMDWithIP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpMDWithIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbkVADemandSubInt;
        private System.Windows.Forms.ComboBox cmbkVADemandInt;
        private System.Windows.Forms.ComboBox cmbkVADemandType;
        private System.Windows.Forms.ComboBox cmbkWDemandSubInt;
        private System.Windows.Forms.ComboBox cmbkWDemandInt;
        private System.Windows.Forms.ComboBox cmbkWDemandType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}

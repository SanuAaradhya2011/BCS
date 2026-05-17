namespace CAB.UI
{
    partial class DaySpecificLoadSurvey
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
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.rdbtnEnergy = new System.Windows.Forms.RadioButton();
            this.rdbtnDemand = new System.Windows.Forms.RadioButton();
            this.btnShowReport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.dtpToDate);
            this.groupBox1.Controls.Add(this.dtpFromDate);
            this.groupBox1.Controls.Add(this.rdbtnEnergy);
            this.groupBox1.Controls.Add(this.rdbtnDemand);
            this.groupBox1.Controls.Add(this.btnShowReport);
            this.groupBox1.Location = new System.Drawing.Point(6, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 161);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "From";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(188, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "To";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(231, 27);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(102, 20);
            this.dtpToDate.TabIndex = 8;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dtpToDate_ValueChanged);
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd-MMM-yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(72, 27);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(102, 20);
            this.dtpFromDate.TabIndex = 7;
            // 
            // rdbtnEnergy
            // 
            this.rdbtnEnergy.AutoSize = true;
            this.rdbtnEnergy.Location = new System.Drawing.Point(112, 78);
            this.rdbtnEnergy.Name = "rdbtnEnergy";
            this.rdbtnEnergy.Size = new System.Drawing.Size(58, 17);
            this.rdbtnEnergy.TabIndex = 2;
            this.rdbtnEnergy.TabStop = true;
            this.rdbtnEnergy.Text = "Energy";
            this.rdbtnEnergy.UseVisualStyleBackColor = true;
            // 
            // rdbtnDemand
            // 
            this.rdbtnDemand.AutoSize = true;
            this.rdbtnDemand.Location = new System.Drawing.Point(29, 77);
            this.rdbtnDemand.Name = "rdbtnDemand";
            this.rdbtnDemand.Size = new System.Drawing.Size(65, 17);
            this.rdbtnDemand.TabIndex = 1;
            this.rdbtnDemand.TabStop = true;
            this.rdbtnDemand.Text = "Demand";
            this.rdbtnDemand.UseVisualStyleBackColor = true;
            // 
            // btnShowReport
            // 
            this.btnShowReport.Location = new System.Drawing.Point(211, 117);
            this.btnShowReport.Name = "btnShowReport";
            this.btnShowReport.Size = new System.Drawing.Size(60, 30);
            this.btnShowReport.TabIndex = 0;
            this.btnShowReport.Text = "Show";
            this.btnShowReport.UseVisualStyleBackColor = true;
            this.btnShowReport.Click += new System.EventHandler(this.btnShowReport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(274, 117);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(60, 30);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DaySpecificLoadSurvey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 171);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DaySpecificLoadSurvey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Day Specific Load Survey";
            this.Load += new System.EventHandler(this.DaySpecificLoadSurvey_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnShowReport;
        private System.Windows.Forms.RadioButton rdbtnEnergy;
        private System.Windows.Forms.RadioButton rdbtnDemand;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Button btnClose;
    }
}
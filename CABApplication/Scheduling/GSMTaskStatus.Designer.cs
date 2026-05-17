namespace CABApplication
{
    partial class GSMTaskStatus
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
            this.GboxTaskDetails = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblNextRunTime = new System.Windows.Forms.Label();
            this.lblNextRunDate = new System.Windows.Forms.Label();
            this.lblTaskType = new System.Windows.Forms.Label();
            this.lblGroupName = new System.Windows.Forms.Label();
            this.lblTaskName = new System.Windows.Forms.Label();
            this.gboxTaskStatus = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MeterID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeterGeneralData = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MeterInstData = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MeterBillingData = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MidnightCompleted = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LoadSurveyCompleted = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TamperCompleted = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MeterDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeterStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeterRetries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GboxTaskDetails.SuspendLayout();
            this.gboxTaskStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GboxTaskDetails
            // 
            this.GboxTaskDetails.Controls.Add(this.lblStatus);
            this.GboxTaskDetails.Controls.Add(this.lblNextRunTime);
            this.GboxTaskDetails.Controls.Add(this.lblNextRunDate);
            this.GboxTaskDetails.Controls.Add(this.lblTaskType);
            this.GboxTaskDetails.Controls.Add(this.lblGroupName);
            this.GboxTaskDetails.Controls.Add(this.lblTaskName);
            this.GboxTaskDetails.Location = new System.Drawing.Point(12, 12);
            this.GboxTaskDetails.Name = "GboxTaskDetails";
            this.GboxTaskDetails.Size = new System.Drawing.Size(769, 100);
            this.GboxTaskDetails.TabIndex = 0;
            this.GboxTaskDetails.TabStop = false;
            this.GboxTaskDetails.Text = "Schedule Details";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(433, 16);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(167, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Status :";
            // 
            // lblNextRunTime
            // 
            this.lblNextRunTime.Location = new System.Drawing.Point(350, 74);
            this.lblNextRunTime.Name = "lblNextRunTime";
            this.lblNextRunTime.Size = new System.Drawing.Size(250, 13);
            this.lblNextRunTime.TabIndex = 4;
            this.lblNextRunTime.Text = "Next Run Time :";
            // 
            // lblNextRunDate
            // 
            this.lblNextRunDate.Location = new System.Drawing.Point(350, 48);
            this.lblNextRunDate.Name = "lblNextRunDate";
            this.lblNextRunDate.Size = new System.Drawing.Size(250, 13);
            this.lblNextRunDate.TabIndex = 3;
            this.lblNextRunDate.Text = "Next Run Date :";
            // 
            // lblTaskType
            // 
            this.lblTaskType.Location = new System.Drawing.Point(17, 74);
            this.lblTaskType.Name = "lblTaskType";
            this.lblTaskType.Size = new System.Drawing.Size(275, 13);
            this.lblTaskType.TabIndex = 2;
            this.lblTaskType.Text = "Schedule Type  :";
            // 
            // lblGroupName
            // 
            this.lblGroupName.Location = new System.Drawing.Point(18, 48);
            this.lblGroupName.Name = "lblGroupName";
            this.lblGroupName.Size = new System.Drawing.Size(341, 13);
            this.lblGroupName.TabIndex = 1;
            this.lblGroupName.Text = "Group Name      :";
            // 
            // lblTaskName
            // 
            this.lblTaskName.Location = new System.Drawing.Point(17, 24);
            this.lblTaskName.Name = "lblTaskName";
            this.lblTaskName.Size = new System.Drawing.Size(410, 13);
            this.lblTaskName.TabIndex = 0;
            this.lblTaskName.Text = "Schedule Name : ";
            // 
            // gboxTaskStatus
            // 
            this.gboxTaskStatus.Controls.Add(this.dataGridView1);
            this.gboxTaskStatus.Location = new System.Drawing.Point(12, 118);
            this.gboxTaskStatus.Name = "gboxTaskStatus";
            this.gboxTaskStatus.Size = new System.Drawing.Size(982, 352);
            this.gboxTaskStatus.TabIndex = 1;
            this.gboxTaskStatus.TabStop = false;
            this.gboxTaskStatus.Text = "Schedule Status";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MeterID,
            this.MeterGeneralData,
            this.MeterInstData,
            this.MeterBillingData,
            this.MidnightCompleted,
            this.LoadSurveyCompleted,
            this.TamperCompleted,
            this.MeterDate,
            this.MeterStatus,
            this.MeterRetries,
            this.errorMessage});
            this.dataGridView1.Location = new System.Drawing.Point(8, 23);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(968, 317);
            this.dataGridView1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 30000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(787, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 98);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "NC : Meter read not completed.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "NS : Meter read not started.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "C : Meter read is complete.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP : Meter read is in progress.";
            // 
            // MeterID
            // 
            this.MeterID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.MeterID.HeaderText = "Meter ID";
            this.MeterID.Name = "MeterID";
            this.MeterID.ReadOnly = true;
            this.MeterID.Width = 73;
            // 
            // MeterGeneralData
            // 
            this.MeterGeneralData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MeterGeneralData.HeaderText = "General";
            this.MeterGeneralData.Name = "MeterGeneralData";
            this.MeterGeneralData.ReadOnly = true;
            this.MeterGeneralData.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MeterGeneralData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.MeterGeneralData.Width = 69;
            // 
            // MeterInstData
            // 
            this.MeterInstData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MeterInstData.HeaderText = "Instantaneous";
            this.MeterInstData.Name = "MeterInstData";
            this.MeterInstData.ReadOnly = true;
            this.MeterInstData.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MeterInstData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.MeterInstData.Width = 99;
            // 
            // MeterBillingData
            // 
            this.MeterBillingData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MeterBillingData.HeaderText = "Billing";
            this.MeterBillingData.Name = "MeterBillingData";
            this.MeterBillingData.ReadOnly = true;
            this.MeterBillingData.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MeterBillingData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.MeterBillingData.Width = 59;
            // 
            // MidnightCompleted
            // 
            this.MidnightCompleted.HeaderText = "Daily Load";
            this.MidnightCompleted.Name = "MidnightCompleted";
            this.MidnightCompleted.ReadOnly = true;
            // 
            // LoadSurveyCompleted
            // 
            this.LoadSurveyCompleted.HeaderText = "Load Survey";
            this.LoadSurveyCompleted.Name = "LoadSurveyCompleted";
            this.LoadSurveyCompleted.ReadOnly = true;
            // 
            // TamperCompleted
            // 
            this.TamperCompleted.HeaderText = "Tamper";
            this.TamperCompleted.Name = "TamperCompleted";
            this.TamperCompleted.ReadOnly = true;
            // 
            // MeterDate
            // 
            this.MeterDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.MeterDate.HeaderText = "Date";
            this.MeterDate.Name = "MeterDate";
            this.MeterDate.ReadOnly = true;
            this.MeterDate.Width = 55;
            // 
            // MeterStatus
            // 
            this.MeterStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MeterStatus.HeaderText = "Status";
            this.MeterStatus.Name = "MeterStatus";
            this.MeterStatus.ReadOnly = true;
            this.MeterStatus.Width = 62;
            // 
            // MeterRetries
            // 
            this.MeterRetries.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.MeterRetries.HeaderText = "Tries";
            this.MeterRetries.Name = "MeterRetries";
            this.MeterRetries.ReadOnly = true;
            this.MeterRetries.Width = 55;
            // 
            // errorMessage
            // 
            this.errorMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.errorMessage.HeaderText = "Message";
            this.errorMessage.Name = "errorMessage";
            this.errorMessage.ReadOnly = true;
            this.errorMessage.Width = 75;
            // 
            // GSMTaskStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 484);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gboxTaskStatus);
            this.Controls.Add(this.GboxTaskDetails);
            this.Name = "GSMTaskStatus";
            this.StatusMessage = "";
            this.Text = "Scheduled Task Status";
            this.Load += new System.EventHandler(this.GSMTaskStatus_Load);
            this.GboxTaskDetails.ResumeLayout(false);
            this.gboxTaskStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GboxTaskDetails;
        private System.Windows.Forms.GroupBox gboxTaskStatus;
        private System.Windows.Forms.Label lblTaskName;
        private System.Windows.Forms.Label lblGroupName;
        private System.Windows.Forms.Label lblTaskType;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblNextRunTime;
        private System.Windows.Forms.Label lblNextRunDate;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeterID;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MeterGeneralData;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MeterInstData;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MeterBillingData;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MidnightCompleted;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LoadSurveyCompleted;
        private System.Windows.Forms.DataGridViewCheckBoxColumn TamperCompleted;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeterDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeterStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeterRetries;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorMessage;
    }
}

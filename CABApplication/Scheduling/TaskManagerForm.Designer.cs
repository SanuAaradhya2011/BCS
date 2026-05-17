namespace CABApplication.Scheduling
{
    partial class TaskManagerForm
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
            this.dgvScheduledTasks = new System.Windows.Forms.DataGridView();
            this.taskId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkTaskManager = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.taskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taskType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.startTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlSelection = new System.Windows.Forms.Panel();
            this.rdbInactive = new System.Windows.Forms.RadioButton();
            this.rdbCompleted = new System.Windows.Forms.RadioButton();
            this.rdbAll = new System.Windows.Forms.RadioButton();
            this.rdbInprogress = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.gbSchedule = new System.Windows.Forms.GroupBox();
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnInactive = new System.Windows.Forms.Button();
            this.btnNewTask = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.spcTaskManager = new System.Windows.Forms.SplitContainer();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.btnConfigureParameters = new System.Windows.Forms.Button();
            this.btnShowRunning = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduledTasks)).BeginInit();
            this.pnlSelection.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbSchedule.SuspendLayout();
            this.spcTaskManager.Panel1.SuspendLayout();
            this.spcTaskManager.Panel2.SuspendLayout();
            this.spcTaskManager.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvScheduledTasks
            // 
            this.dgvScheduledTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScheduledTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.taskId,
            this.chkTaskManager,
            this.taskName,
            this.groupId,
            this.taskType,
            this.startDate,
            this.startTime,
            this.Status,
            this.groupName});
            this.dgvScheduledTasks.Location = new System.Drawing.Point(0, 0);
            this.dgvScheduledTasks.MultiSelect = false;
            this.dgvScheduledTasks.Name = "dgvScheduledTasks";
            this.dgvScheduledTasks.Size = new System.Drawing.Size(1072, 593);
            this.dgvScheduledTasks.TabIndex = 0;
            this.dgvScheduledTasks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvScheduledTasks_CellDoubleClick);
            // 
            // taskId
            // 
            this.taskId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.taskId.DataPropertyName = "taskId";
            this.taskId.HeaderText = "Schedule Id";
            this.taskId.Name = "taskId";
            this.taskId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.taskId.Visible = false;
            // 
            // chkTaskManager
            // 
            this.chkTaskManager.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.chkTaskManager.DataPropertyName = "chkTaskManager";
            this.chkTaskManager.HeaderText = "Select";
            this.chkTaskManager.Name = "chkTaskManager";
            this.chkTaskManager.Width = 43;
            // 
            // taskName
            // 
            this.taskName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.taskName.DataPropertyName = "taskName";
            this.taskName.FillWeight = 120.1172F;
            this.taskName.HeaderText = "Schedule Name";
            this.taskName.Name = "taskName";
            this.taskName.ReadOnly = true;
            this.taskName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupId
            // 
            this.groupId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupId.DataPropertyName = "groupId";
            this.groupId.HeaderText = "Group Id";
            this.groupId.Name = "groupId";
            this.groupId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.groupId.Visible = false;
            // 
            // taskType
            // 
            this.taskType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.taskType.DataPropertyName = "taskType";
            this.taskType.FillWeight = 93.05698F;
            this.taskType.HeaderText = "Schedule Type";
            this.taskType.Name = "taskType";
            this.taskType.ReadOnly = true;
            this.taskType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // startDate
            // 
            this.startDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.startDate.DataPropertyName = "startDate";
            this.startDate.FillWeight = 143.0129F;
            this.startDate.HeaderText = "Next Run Date";
            this.startDate.Name = "startDate";
            this.startDate.ReadOnly = true;
            this.startDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // startTime
            // 
            this.startTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.startTime.DataPropertyName = "startTime";
            this.startTime.FillWeight = 69.15031F;
            this.startTime.HeaderText = "Next Run Time";
            this.startTime.Name = "startTime";
            this.startTime.ReadOnly = true;
            this.startTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.DataPropertyName = "Status";
            this.Status.FillWeight = 54.54546F;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // groupName
            // 
            this.groupName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.groupName.DataPropertyName = "groupName";
            this.groupName.FillWeight = 120.1172F;
            this.groupName.HeaderText = "Group Name";
            this.groupName.Name = "groupName";
            this.groupName.ReadOnly = true;
            this.groupName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // pnlSelection
            // 
            this.pnlSelection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSelection.Controls.Add(this.rdbInactive);
            this.pnlSelection.Controls.Add(this.rdbCompleted);
            this.pnlSelection.Controls.Add(this.rdbAll);
            this.pnlSelection.Controls.Add(this.rdbInprogress);
            this.pnlSelection.Location = new System.Drawing.Point(37, 25);
            this.pnlSelection.Name = "pnlSelection";
            this.pnlSelection.Size = new System.Drawing.Size(213, 49);
            this.pnlSelection.TabIndex = 9;
            // 
            // rdbInactive
            // 
            this.rdbInactive.AutoSize = true;
            this.rdbInactive.Location = new System.Drawing.Point(139, 26);
            this.rdbInactive.Name = "rdbInactive";
            this.rdbInactive.Size = new System.Drawing.Size(63, 17);
            this.rdbInactive.TabIndex = 4;
            this.rdbInactive.TabStop = true;
            this.rdbInactive.Text = "Inactive";
            this.rdbInactive.UseVisualStyleBackColor = true;
            this.rdbInactive.CheckedChanged += new System.EventHandler(this.rdbInactive_CheckedChanged);
            // 
            // rdbCompleted
            // 
            this.rdbCompleted.AutoSize = true;
            this.rdbCompleted.Location = new System.Drawing.Point(5, 27);
            this.rdbCompleted.Name = "rdbCompleted";
            this.rdbCompleted.Size = new System.Drawing.Size(75, 17);
            this.rdbCompleted.TabIndex = 3;
            this.rdbCompleted.TabStop = true;
            this.rdbCompleted.Text = "Completed";
            this.rdbCompleted.UseVisualStyleBackColor = true;
            this.rdbCompleted.CheckedChanged += new System.EventHandler(this.rdbCompleted_CheckedChanged);
            // 
            // rdbAll
            // 
            this.rdbAll.AutoSize = true;
            this.rdbAll.Location = new System.Drawing.Point(139, 3);
            this.rdbAll.Name = "rdbAll";
            this.rdbAll.Size = new System.Drawing.Size(44, 17);
            this.rdbAll.TabIndex = 2;
            this.rdbAll.TabStop = true;
            this.rdbAll.Text = "ALL";
            this.rdbAll.UseVisualStyleBackColor = true;
            this.rdbAll.CheckedChanged += new System.EventHandler(this.rdbAll_CheckedChanged);
            // 
            // rdbInprogress
            // 
            this.rdbInprogress.AutoSize = true;
            this.rdbInprogress.Location = new System.Drawing.Point(5, 3);
            this.rdbInprogress.Name = "rdbInprogress";
            this.rdbInprogress.Size = new System.Drawing.Size(118, 17);
            this.rdbInprogress.TabIndex = 0;
            this.rdbInprogress.TabStop = true;
            this.rdbInprogress.Text = "Inprogress/Inqueue";
            this.rdbInprogress.UseVisualStyleBackColor = true;
            this.rdbInprogress.CheckedChanged += new System.EventHandler(this.rdbInprogress_CheckedChanged);
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
            this.groupBox1.Location = new System.Drawing.Point(730, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 78);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Inactive :    Task is paused/aborted.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(165, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Completed : Task has completed.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Inprogress : Scheduled task is running.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Inqueue :    Task is scheduled to run.";
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.Color.White;
            this.txtStatus.Location = new System.Drawing.Point(2, 14);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(1075, 57);
            this.txtStatus.TabIndex = 11;
            this.txtStatus.Text = "";
            this.txtStatus.Visible = false;
            // 
            // gbSchedule
            // 
            this.gbSchedule.Controls.Add(this.btnActivate);
            this.gbSchedule.Controls.Add(this.btnInactive);
            this.gbSchedule.Controls.Add(this.btnNewTask);
            this.gbSchedule.Controls.Add(this.btnDelete);
            this.gbSchedule.Location = new System.Drawing.Point(256, 21);
            this.gbSchedule.Name = "gbSchedule";
            this.gbSchedule.Size = new System.Drawing.Size(333, 54);
            this.gbSchedule.TabIndex = 12;
            this.gbSchedule.TabStop = false;
            this.gbSchedule.Text = "Schedule";
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(249, 18);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(75, 25);
            this.btnActivate.TabIndex = 11;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = false;
            this.btnActivate.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnActivate.ForeColor = System.Drawing.Color.White;
            this.btnActivate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivate.FlatAppearance.BorderSize = 0;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnInactive
            // 
            this.btnInactive.Location = new System.Drawing.Point(168, 18);
            this.btnInactive.Name = "btnInactive";
            this.btnInactive.Size = new System.Drawing.Size(75, 25);
            this.btnInactive.TabIndex = 10;
            this.btnInactive.Text = "Inactive";
            this.btnInactive.UseVisualStyleBackColor = false;
            this.btnInactive.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnInactive.ForeColor = System.Drawing.Color.White;
            this.btnInactive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInactive.FlatAppearance.BorderSize = 0;
            this.btnInactive.Click += new System.EventHandler(this.btnInactive_Click);
            // 
            // btnNewTask
            // 
            this.btnNewTask.Location = new System.Drawing.Point(9, 18);
            this.btnNewTask.Name = "btnNewTask";
            this.btnNewTask.Size = new System.Drawing.Size(75, 25);
            this.btnNewTask.TabIndex = 8;
            this.btnNewTask.Text = "Create New";
            this.btnNewTask.UseVisualStyleBackColor = false;
            this.btnNewTask.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnNewTask.ForeColor = System.Drawing.Color.White;
            this.btnNewTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewTask.FlatAppearance.BorderSize = 0;
            this.btnNewTask.Click += new System.EventHandler(this.btnNewTask_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(88, 18);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 25);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // spcTaskManager
            // 
            this.spcTaskManager.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.spcTaskManager.Location = new System.Drawing.Point(37, 90);
            this.spcTaskManager.Name = "spcTaskManager";
            this.spcTaskManager.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcTaskManager.Panel1
            // 
            this.spcTaskManager.Panel1.Controls.Add(this.dgvScheduledTasks);
            this.spcTaskManager.Panel1MinSize = 50;
            // 
            // spcTaskManager.Panel2
            // 
            this.spcTaskManager.Panel2.AutoScroll = true;
            this.spcTaskManager.Panel2.Controls.Add(this.txtStatus);
            this.spcTaskManager.Panel2Collapsed = true;
            this.spcTaskManager.Panel2MinSize = 10;
            this.spcTaskManager.Size = new System.Drawing.Size(1079, 600);
            this.spcTaskManager.SplitterDistance = 213;
            this.spcTaskManager.TabIndex = 13;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(604, 16);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 23);
            this.btnClearLog.TabIndex = 14;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnClearLog.ForeColor = System.Drawing.Color.White;
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.FlatAppearance.BorderSize = 0;
            this.btnClearLog.Visible = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Location = new System.Drawing.Point(603, 41);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 23);
            this.btnAbort.TabIndex = 15;
            this.btnAbort.Text = "&Abort";
            this.btnAbort.UseVisualStyleBackColor = false;
            this.btnAbort.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnAbort.ForeColor = System.Drawing.Color.White;
            this.btnAbort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbort.FlatAppearance.BorderSize = 0;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // btnConfigureParameters
            // 
            this.btnConfigureParameters.Location = new System.Drawing.Point(955, 16);
            this.btnConfigureParameters.Name = "btnConfigureParameters";
            this.btnConfigureParameters.Size = new System.Drawing.Size(159, 23);
            this.btnConfigureParameters.TabIndex = 16;
            this.btnConfigureParameters.Text = "Configure Report Parameters";
            this.btnConfigureParameters.UseVisualStyleBackColor = false;
            this.btnConfigureParameters.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnConfigureParameters.ForeColor = System.Drawing.Color.White;
            this.btnConfigureParameters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfigureParameters.FlatAppearance.BorderSize = 0;
            this.btnConfigureParameters.Click += new System.EventHandler(this.btnConfigureParameters_Click);
            // 
            // btnShowRunning
            // 
            this.btnShowRunning.Location = new System.Drawing.Point(955, 54);
            this.btnShowRunning.Name = "btnShowRunning";
            this.btnShowRunning.Size = new System.Drawing.Size(159, 23);
            this.btnShowRunning.TabIndex = 17;
            this.btnShowRunning.Text = "Show Running  Scheduled";
            this.btnShowRunning.UseVisualStyleBackColor = false;
            this.btnShowRunning.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShowRunning.ForeColor = System.Drawing.Color.White;
            this.btnShowRunning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowRunning.FlatAppearance.BorderSize = 0;
            this.btnShowRunning.Click += new System.EventHandler(this.btnShowRunning_Click);
            // 
            // TaskManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1128, 706);
            this.Controls.Add(this.btnShowRunning);
            this.Controls.Add(this.btnConfigureParameters);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.spcTaskManager);
            this.Controls.Add(this.gbSchedule);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pnlSelection);
            this.Name = "TaskManagerForm";
            this.StatusMessage = "";
            this.Text = "Task Manager";
            this.Load += new System.EventHandler(this.TaskManagerForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskManagerForm_FormClosing);
            this.Activated += new System.EventHandler(this.TaskManagerForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduledTasks)).EndInit();
            this.pnlSelection.ResumeLayout(false);
            this.pnlSelection.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbSchedule.ResumeLayout(false);
            this.spcTaskManager.Panel1.ResumeLayout(false);
            this.spcTaskManager.Panel2.ResumeLayout(false);
            this.spcTaskManager.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvScheduledTasks;
        private System.Windows.Forms.Panel pnlSelection;
        private System.Windows.Forms.RadioButton rdbAll;
        private System.Windows.Forms.RadioButton rdbInprogress;
        private System.Windows.Forms.RadioButton rdbCompleted;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox txtStatus;
        private System.Windows.Forms.GroupBox gbSchedule;
        private System.Windows.Forms.Button btnNewTask;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Button btnInactive;
        private System.Windows.Forms.RadioButton rdbInactive;
        private System.Windows.Forms.SplitContainer spcTaskManager;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskId;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkTaskManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupId;
        private System.Windows.Forms.DataGridViewTextBoxColumn taskType;
        private System.Windows.Forms.DataGridViewTextBoxColumn startDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn groupName;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.Button btnConfigureParameters;
        private System.Windows.Forms.Button btnShowRunning;



    }
}


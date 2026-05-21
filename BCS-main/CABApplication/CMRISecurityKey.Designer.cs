namespace CAB.UI
{
    partial class CMRISecurityKey
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
            this.CmriAuth_grpAuth = new System.Windows.Forms.GroupBox();
            this.lngLabel2 = new CAB.UI.Controls.CABLabel();
            this.lngLabel1 = new CAB.UI.Controls.CABLabel();
            this.btnPushMove = new System.Windows.Forms.Button();
            this.btnScheduleCancel = new System.Windows.Forms.Button();
            this.btnPushRemove = new System.Windows.Forms.Button();
            this.btnCMRISchedule = new System.Windows.Forms.Button();
            this.btnPushRemoveAll = new System.Windows.Forms.Button();
            this.btnPushMoveAll = new System.Windows.Forms.Button();
            this.listBoxSelectedMeters = new System.Windows.Forms.ListBox();
            this.listBoxAvailableMeters = new System.Windows.Forms.ListBox();
            this.CmriAuth_grpAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmriAuth_grpAuth
            // 
            this.CmriAuth_grpAuth.Controls.Add(this.lngLabel2);
            this.CmriAuth_grpAuth.Controls.Add(this.lngLabel1);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushMove);
            this.CmriAuth_grpAuth.Controls.Add(this.btnScheduleCancel);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushRemove);
            this.CmriAuth_grpAuth.Controls.Add(this.btnCMRISchedule);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushRemoveAll);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushMoveAll);
            this.CmriAuth_grpAuth.Controls.Add(this.listBoxSelectedMeters);
            this.CmriAuth_grpAuth.Controls.Add(this.listBoxAvailableMeters);
            this.CmriAuth_grpAuth.Location = new System.Drawing.Point(13, 10);
            this.CmriAuth_grpAuth.Name = "CmriAuth_grpAuth";
            this.CmriAuth_grpAuth.Size = new System.Drawing.Size(591, 368);
            this.CmriAuth_grpAuth.TabIndex = 1;
            this.CmriAuth_grpAuth.TabStop = false;
            this.CmriAuth_grpAuth.Text = "CMRI Schedule File";
            // 
            // lngLabel2
            // 
            this.lngLabel2.AutoSize = true;
            this.lngLabel2.Location = new System.Drawing.Point(353, 20);
            this.lngLabel2.Name = "lngLabel2";
            this.lngLabel2.Size = new System.Drawing.Size(84, 13);
            this.lngLabel2.TabIndex = 48;
            this.lngLabel2.Text = "Selected Meters";
            this.lngLabel2.TranslationKey = null;
            // 
            // lngLabel1
            // 
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Location = new System.Drawing.Point(46, 20);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(85, 13);
            this.lngLabel1.TabIndex = 47;
            this.lngLabel1.Text = "Available Meters";
            this.lngLabel1.TranslationKey = null;
            // 
            // btnPushMove
            // 
            this.btnPushMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPushMove.Location = new System.Drawing.Point(272, 86);
            this.btnPushMove.Name = "btnPushMove";
            this.btnPushMove.Size = new System.Drawing.Size(50, 28);
            this.btnPushMove.TabIndex = 1;
            this.btnPushMove.Text = ">";
            this.btnPushMove.UseVisualStyleBackColor = false;
            this.btnPushMove.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnPushMove.ForeColor = System.Drawing.Color.White;
            this.btnPushMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushMove.FlatAppearance.BorderSize = 0;
            this.btnPushMove.Click += new System.EventHandler(this.btnPushMove_Click);
            // 
            // btnScheduleCancel
            // 
            this.btnScheduleCancel.Location = new System.Drawing.Point(313, 317);
            this.btnScheduleCancel.Name = "btnScheduleCancel";
            this.btnScheduleCancel.Size = new System.Drawing.Size(75, 28);
            this.btnScheduleCancel.TabIndex = 48;
            this.btnScheduleCancel.Text = "Cancel";
            this.btnScheduleCancel.UseVisualStyleBackColor = false;
            this.btnScheduleCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnScheduleCancel.ForeColor = System.Drawing.Color.White;
            this.btnScheduleCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleCancel.FlatAppearance.BorderSize = 0;
            this.btnScheduleCancel.Click += new System.EventHandler(this.btnScheduleCancel_Click);
            // 
            // btnPushRemove
            // 
            this.btnPushRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPushRemove.Location = new System.Drawing.Point(272, 154);
            this.btnPushRemove.Name = "btnPushRemove";
            this.btnPushRemove.Size = new System.Drawing.Size(50, 28);
            this.btnPushRemove.TabIndex = 3;
            this.btnPushRemove.Text = "<";
            this.btnPushRemove.UseVisualStyleBackColor = false;
            this.btnPushRemove.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnPushRemove.ForeColor = System.Drawing.Color.White;
            this.btnPushRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushRemove.FlatAppearance.BorderSize = 0;
            this.btnPushRemove.Click += new System.EventHandler(this.btnPushRemove_Click);
            // 
            // btnCMRISchedule
            // 
            this.btnCMRISchedule.Location = new System.Drawing.Point(203, 316);
            this.btnCMRISchedule.Name = "btnCMRISchedule";
            this.btnCMRISchedule.Size = new System.Drawing.Size(104, 28);
            this.btnCMRISchedule.TabIndex = 47;
            this.btnCMRISchedule.Text = "Get Security Keys";
            this.btnCMRISchedule.UseVisualStyleBackColor = false;
            this.btnCMRISchedule.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCMRISchedule.ForeColor = System.Drawing.Color.White;
            this.btnCMRISchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCMRISchedule.FlatAppearance.BorderSize = 0;
            this.btnCMRISchedule.Click += new System.EventHandler(this.btnCMRISchedule_Click);
            // 
            // btnPushRemoveAll
            // 
            this.btnPushRemoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPushRemoveAll.Location = new System.Drawing.Point(271, 188);
            this.btnPushRemoveAll.Name = "btnPushRemoveAll";
            this.btnPushRemoveAll.Size = new System.Drawing.Size(50, 28);
            this.btnPushRemoveAll.TabIndex = 4;
            this.btnPushRemoveAll.Text = "<<";
            this.btnPushRemoveAll.UseVisualStyleBackColor = false;
            this.btnPushRemoveAll.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnPushRemoveAll.ForeColor = System.Drawing.Color.White;
            this.btnPushRemoveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushRemoveAll.FlatAppearance.BorderSize = 0;
            this.btnPushRemoveAll.Click += new System.EventHandler(this.btnPushRemoveAll_Click);
            // 
            // btnPushMoveAll
            // 
            this.btnPushMoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPushMoveAll.Location = new System.Drawing.Point(272, 120);
            this.btnPushMoveAll.Name = "btnPushMoveAll";
            this.btnPushMoveAll.Size = new System.Drawing.Size(50, 28);
            this.btnPushMoveAll.TabIndex = 2;
            this.btnPushMoveAll.Text = ">>";
            this.btnPushMoveAll.UseVisualStyleBackColor = false;
            this.btnPushMoveAll.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnPushMoveAll.ForeColor = System.Drawing.Color.White;
            this.btnPushMoveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushMoveAll.FlatAppearance.BorderSize = 0;
            this.btnPushMoveAll.Click += new System.EventHandler(this.btnPushMoveAll_Click);
            // 
            // listBoxSelectedMeters
            // 
            this.listBoxSelectedMeters.FormattingEnabled = true;
            this.listBoxSelectedMeters.Location = new System.Drawing.Point(356, 46);
            this.listBoxSelectedMeters.Name = "listBoxSelectedMeters";
            this.listBoxSelectedMeters.Size = new System.Drawing.Size(187, 251);
            this.listBoxSelectedMeters.TabIndex = 5;
            // 
            // listBoxAvailableMeters
            // 
            this.listBoxAvailableMeters.FormattingEnabled = true;
            this.listBoxAvailableMeters.Location = new System.Drawing.Point(49, 46);
            this.listBoxAvailableMeters.Name = "listBoxAvailableMeters";
            this.listBoxAvailableMeters.Size = new System.Drawing.Size(187, 251);
            this.listBoxAvailableMeters.TabIndex = 0;
            // 
            // CMRISecurityKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(613, 395);
            this.Controls.Add(this.CmriAuth_grpAuth);
            this.Name = "CMRISecurityKey";
            this.StatusMessage = "";
            this.Text = "CMRI Schedule File";
            this.Load += new System.EventHandler(this.CMRIScheduleFile_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CMRIScheduleFile_FormClosing);
            this.Activated += new System.EventHandler(this.CMRIScheduleFile_Activated);
            this.CmriAuth_grpAuth.ResumeLayout(false);
            this.CmriAuth_grpAuth.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox CmriAuth_grpAuth;
		private System.Windows.Forms.Button btnPushMove;
		private System.Windows.Forms.Button btnPushRemove;
		private System.Windows.Forms.Button btnPushRemoveAll;
		private System.Windows.Forms.Button btnPushMoveAll;
		private System.Windows.Forms.ListBox listBoxSelectedMeters;
        private System.Windows.Forms.ListBox listBoxAvailableMeters;
		private CAB.UI.Controls.CABLabel lngLabel2;
        private CAB.UI.Controls.CABLabel lngLabel1;
        private System.Windows.Forms.Button btnScheduleCancel;
        private System.Windows.Forms.Button btnCMRISchedule;
	}
}

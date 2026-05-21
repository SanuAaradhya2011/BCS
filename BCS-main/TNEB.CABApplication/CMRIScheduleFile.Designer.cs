namespace CAB.UI
{
	partial class CMRIScheduleFile
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
            this.lngLabel3 = new CAB.UI.Controls.CABLabel();
            this.lblcfgfile = new System.Windows.Forms.TextBox();
            this.txtClearCMRI = new System.Windows.Forms.TextBox();
            this.txtLSDays = new System.Windows.Forms.TextBox();
            this.lblLSDays = new CAB.UI.Controls.CABLabel();
            this.lblClearCMRI = new CAB.UI.Controls.CABLabel();
            this.btnSelectcfgfile = new System.Windows.Forms.Button();
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
            this.CmriAuth_grpAuth.Controls.Add(this.lngLabel3);
            this.CmriAuth_grpAuth.Controls.Add(this.lblcfgfile);
            this.CmriAuth_grpAuth.Controls.Add(this.txtClearCMRI);
            this.CmriAuth_grpAuth.Controls.Add(this.txtLSDays);
            this.CmriAuth_grpAuth.Controls.Add(this.lblLSDays);
            this.CmriAuth_grpAuth.Controls.Add(this.lblClearCMRI);
            this.CmriAuth_grpAuth.Controls.Add(this.btnSelectcfgfile);
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
            this.CmriAuth_grpAuth.Size = new System.Drawing.Size(591, 464);
            this.CmriAuth_grpAuth.TabIndex = 1;
            this.CmriAuth_grpAuth.TabStop = false;
            this.CmriAuth_grpAuth.Text = "CMRI Schedule File";
            // 
            // lngLabel3
            // 
            this.lngLabel3.AutoSize = true;
            this.lngLabel3.Location = new System.Drawing.Point(222, 320);
            this.lngLabel3.Name = "lngLabel3";
            this.lngLabel3.Size = new System.Drawing.Size(40, 13);
            this.lngLabel3.TabIndex = 59;
            this.lngLabel3.Text = "(1 - 90)\r\n";
            this.lngLabel3.TranslationKey = null;
            // 
            // lblcfgfile
            // 
            this.lblcfgfile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblcfgfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcfgfile.Location = new System.Drawing.Point(6, 430);
            this.lblcfgfile.Multiline = true;
            this.lblcfgfile.Name = "lblcfgfile";
            this.lblcfgfile.ReadOnly = true;
            this.lblcfgfile.Size = new System.Drawing.Size(417, 26);
            this.lblcfgfile.TabIndex = 58;
            this.lblcfgfile.Text = "cfg File Path";
            // 
            // txtClearCMRI
            // 
            this.txtClearCMRI.Location = new System.Drawing.Point(269, 354);
            this.txtClearCMRI.MaxLength = 5;
            this.txtClearCMRI.Name = "txtClearCMRI";
            this.txtClearCMRI.PasswordChar = '*';
            this.txtClearCMRI.Size = new System.Drawing.Size(82, 20);
            this.txtClearCMRI.TabIndex = 57;
            this.txtClearCMRI.Text = "12345";
            this.txtClearCMRI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtClearCMRI_KeyPress);
            // 
            // txtLSDays
            // 
            this.txtLSDays.Location = new System.Drawing.Point(269, 317);
            this.txtLSDays.MaxLength = 2;
            this.txtLSDays.Name = "txtLSDays";
            this.txtLSDays.Size = new System.Drawing.Size(82, 20);
            this.txtLSDays.TabIndex = 56;
            this.txtLSDays.Text = "90";
            this.txtLSDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLSDays_KeyPress);
            // 
            // lblLSDays
            // 
            this.lblLSDays.AutoSize = true;
            this.lblLSDays.Location = new System.Drawing.Point(49, 320);
            this.lblLSDays.Name = "lblLSDays";
            this.lblLSDays.Size = new System.Drawing.Size(176, 13);
            this.lblLSDays.TabIndex = 54;
            this.lblLSDays.Text = "No. of days for Load survey readout";
            this.lblLSDays.TranslationKey = null;
            // 
            // lblClearCMRI
            // 
            this.lblClearCMRI.AutoSize = true;
            this.lblClearCMRI.Location = new System.Drawing.Point(49, 357);
            this.lblClearCMRI.Name = "lblClearCMRI";
            this.lblClearCMRI.Size = new System.Drawing.Size(110, 13);
            this.lblClearCMRI.TabIndex = 55;
            this.lblClearCMRI.Text = "Clear CMRI Password";
            this.lblClearCMRI.TranslationKey = null;
            // 
            // btnSelectcfgfile
            // 
            this.btnSelectcfgfile.Location = new System.Drawing.Point(52, 389);
            this.btnSelectcfgfile.Name = "btnSelectcfgfile";
            this.btnSelectcfgfile.Size = new System.Drawing.Size(95, 28);
            this.btnSelectcfgfile.TabIndex = 53;
            this.btnSelectcfgfile.Text = "Browse cfg file...";
            this.btnSelectcfgfile.UseVisualStyleBackColor = true;
            this.btnSelectcfgfile.Click += new System.EventHandler(this.btnSelectcfgfile_Click);
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
            this.btnPushMove.UseVisualStyleBackColor = true;
            this.btnPushMove.Click += new System.EventHandler(this.btnPushMove_Click);
            // 
            // btnScheduleCancel
            // 
            this.btnScheduleCancel.Location = new System.Drawing.Point(510, 428);
            this.btnScheduleCancel.Name = "btnScheduleCancel";
            this.btnScheduleCancel.Size = new System.Drawing.Size(75, 28);
            this.btnScheduleCancel.TabIndex = 48;
            this.btnScheduleCancel.Text = "Cancel";
            this.btnScheduleCancel.UseVisualStyleBackColor = true;
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
            this.btnPushRemove.UseVisualStyleBackColor = true;
            this.btnPushRemove.Click += new System.EventHandler(this.btnPushRemove_Click);
            // 
            // btnCMRISchedule
            // 
            this.btnCMRISchedule.Location = new System.Drawing.Point(429, 429);
            this.btnCMRISchedule.Name = "btnCMRISchedule";
            this.btnCMRISchedule.Size = new System.Drawing.Size(75, 28);
            this.btnCMRISchedule.TabIndex = 47;
            this.btnCMRISchedule.Text = "OK";
            this.btnCMRISchedule.UseVisualStyleBackColor = true;
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
            this.btnPushRemoveAll.UseVisualStyleBackColor = true;
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
            this.btnPushMoveAll.UseVisualStyleBackColor = true;
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
            // CMRIScheduleFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(613, 479);
            this.Controls.Add(this.CmriAuth_grpAuth);
            this.Name = "CMRIScheduleFile";
            this.StatusMessage = "";
            this.Text = "CMRI Schedule File";
            this.Load += new System.EventHandler(this.CMRIScheduleFile_Load);
            this.Activated += new System.EventHandler(this.CMRIScheduleFile_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CMRIScheduleFile_FormClosing);
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
        private System.Windows.Forms.Button btnSelectcfgfile;
        private System.Windows.Forms.TextBox txtClearCMRI;
        private System.Windows.Forms.TextBox txtLSDays;
        private CAB.UI.Controls.CABLabel lblLSDays;
        private CAB.UI.Controls.CABLabel lblClearCMRI;
        private System.Windows.Forms.TextBox lblcfgfile;
        private CAB.UI.Controls.CABLabel lngLabel3;
	}
}
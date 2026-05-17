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
            this.securityfilepanel = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnsecuritykey = new System.Windows.Forms.Button();
            this.txtpwd = new System.Windows.Forms.TextBox();
            this.txtusername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbusername = new System.Windows.Forms.Label();
            this.btnsecurityfile = new System.Windows.Forms.Button();
            this.btnclose = new System.Windows.Forms.Button();
            this.scdfileoperationpanel = new System.Windows.Forms.Panel();
            this.chkisLPRSchedule = new System.Windows.Forms.CheckBox();
            this.btnCMRISchedule = new System.Windows.Forms.Button();
            this.lngLabel3 = new CAB.UI.Controls.CABLabel();
            this.btnScheduleCancel = new System.Windows.Forms.Button();
            this.lblcfgfile = new System.Windows.Forms.TextBox();
            this.btnSelectcfgfile = new System.Windows.Forms.Button();
            this.txtClearCMRI = new System.Windows.Forms.TextBox();
            this.lblClearCMRI = new CAB.UI.Controls.CABLabel();
            this.txtLSDays = new System.Windows.Forms.TextBox();
            this.lblLSDays = new CAB.UI.Controls.CABLabel();
            this.lngLabel2 = new CAB.UI.Controls.CABLabel();
            this.lngLabel1 = new CAB.UI.Controls.CABLabel();
            this.btnPushMove = new System.Windows.Forms.Button();
            this.btnPushRemove = new System.Windows.Forms.Button();
            this.btnPushRemoveAll = new System.Windows.Forms.Button();
            this.btnPushMoveAll = new System.Windows.Forms.Button();
            this.listBoxSelectedMeters = new System.Windows.Forms.ListBox();
            this.listBoxAvailableMeters = new System.Windows.Forms.ListBox();
            this.CmriAuth_grpAuth.SuspendLayout();
            this.securityfilepanel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.scdfileoperationpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmriAuth_grpAuth
            // 
            this.CmriAuth_grpAuth.Controls.Add(this.securityfilepanel);
            this.CmriAuth_grpAuth.Controls.Add(this.scdfileoperationpanel);
            this.CmriAuth_grpAuth.Controls.Add(this.lngLabel2);
            this.CmriAuth_grpAuth.Controls.Add(this.lngLabel1);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushMove);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushRemove);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushRemoveAll);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushMoveAll);
            this.CmriAuth_grpAuth.Controls.Add(this.listBoxSelectedMeters);
            this.CmriAuth_grpAuth.Controls.Add(this.listBoxAvailableMeters);
            this.CmriAuth_grpAuth.Location = new System.Drawing.Point(20, 20);
            this.CmriAuth_grpAuth.Name = "CmriAuth_grpAuth";
            this.CmriAuth_grpAuth.Size = new System.Drawing.Size(740, 540);
            this.CmriAuth_grpAuth.TabIndex = 1;
            this.CmriAuth_grpAuth.TabStop = false;
            this.CmriAuth_grpAuth.Text = "  CMRI Configuration  ";
            this.CmriAuth_grpAuth.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.CmriAuth_grpAuth.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            this.CmriAuth_grpAuth.Padding = new System.Windows.Forms.Padding(12);
            // 
            // securityfilepanel
            // 
            this.securityfilepanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.securityfilepanel.Controls.Add(this.statusStrip1);
            this.securityfilepanel.Controls.Add(this.btnsecuritykey);
            this.securityfilepanel.Controls.Add(this.txtpwd);
            this.securityfilepanel.Controls.Add(this.txtusername);
            this.securityfilepanel.Controls.Add(this.label1);
            this.securityfilepanel.Controls.Add(this.lbusername);
            this.securityfilepanel.Controls.Add(this.btnsecurityfile);
            this.securityfilepanel.Controls.Add(this.btnclose);
            this.securityfilepanel.Location = new System.Drawing.Point(12, 370);
            this.securityfilepanel.Name = "securityfilepanel";
            this.securityfilepanel.Size = new System.Drawing.Size(716, 160);
            this.securityfilepanel.TabIndex = 62;
            this.securityfilepanel.BackColor = System.Drawing.Color.FromArgb(248, 250, 253);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbstatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 131);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(579, 22);
            this.statusStrip1.TabIndex = 68;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbstatus
            // 
            this.lbstatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbstatus.Name = "lbstatus";
            this.lbstatus.Size = new System.Drawing.Size(0, 17);
            // 
            // btnsecuritykey
            // 
            this.btnsecuritykey.Location = new System.Drawing.Point(400, 20);
            this.btnsecuritykey.Name = "btnsecuritykey";
            this.btnsecuritykey.Size = new System.Drawing.Size(170, 32);
            this.btnsecuritykey.TabIndex = 67;
            this.btnsecuritykey.Text = "🔑  Generate Security Key";
            this.btnsecuritykey.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnsecuritykey.UseVisualStyleBackColor = false;
            this.btnsecuritykey.BackColor = System.Drawing.Color.FromArgb(156, 39, 176);
            this.btnsecuritykey.ForeColor = System.Drawing.Color.White;
            this.btnsecuritykey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsecuritykey.FlatAppearance.BorderSize = 0;
            this.btnsecuritykey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(130, 30, 150);
            this.btnsecuritykey.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsecuritykey.Click += new System.EventHandler(this.btnsecuritykey_Click);
            // 
            // txtpwd
            // 
            this.txtpwd.Location = new System.Drawing.Point(130, 56);
            this.txtpwd.MaxLength = 50;
            this.txtpwd.Name = "txtpwd";
            this.txtpwd.PasswordChar = '*';
            this.txtpwd.Size = new System.Drawing.Size(180, 25);
            this.txtpwd.TabIndex = 66;
            this.txtpwd.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtpwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtpwd.BackColor = System.Drawing.Color.White;
            // 
            // txtusername
            // 
            this.txtusername.Location = new System.Drawing.Point(130, 24);
            this.txtusername.MaxLength = 50;
            this.txtusername.Name = "txtusername";
            this.txtusername.Size = new System.Drawing.Size(180, 25);
            this.txtusername.TabIndex = 65;
            this.txtusername.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtusername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtusername.BackColor = System.Drawing.Color.White;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 64;
            this.label1.Text = "🔒  Password :";
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // lbusername
            // 
            this.lbusername.AutoSize = true;
            this.lbusername.Location = new System.Drawing.Point(20, 27);
            this.lbusername.Name = "lbusername";
            this.lbusername.Size = new System.Drawing.Size(90, 17);
            this.lbusername.TabIndex = 63;
            this.lbusername.Text = "👤  User Name :";
            this.lbusername.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lbusername.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            // 
            // btnsecurityfile
            // 
            this.btnsecurityfile.Location = new System.Drawing.Point(400, 56);
            this.btnsecurityfile.Name = "btnsecurityfile";
            this.btnsecurityfile.Size = new System.Drawing.Size(170, 32);
            this.btnsecurityfile.TabIndex = 61;
            this.btnsecurityfile.Text = "📄  Generate HHU File";
            this.btnsecurityfile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnsecurityfile.UseVisualStyleBackColor = false;
            this.btnsecurityfile.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnsecurityfile.ForeColor = System.Drawing.Color.White;
            this.btnsecurityfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsecurityfile.FlatAppearance.BorderSize = 0;
            this.btnsecurityfile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 100, 190);
            this.btnsecurityfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsecurityfile.Click += new System.EventHandler(this.btnsecurityfile_Click);
            // 
            // btnclose
            // 
            this.btnclose.Location = new System.Drawing.Point(600, 20);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(100, 32);
            this.btnclose.TabIndex = 62;
            this.btnclose.Text = "Cancel";
            this.btnclose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnclose.UseVisualStyleBackColor = false;
            this.btnclose.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnclose.ForeColor = System.Drawing.Color.White;
            this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnclose.FlatAppearance.BorderSize = 0;
            this.btnclose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // scdfileoperationpanel
            // 
            this.scdfileoperationpanel.Controls.Add(this.chkisLPRSchedule);
            this.scdfileoperationpanel.Controls.Add(this.btnCMRISchedule);
            this.scdfileoperationpanel.Controls.Add(this.lngLabel3);
            this.scdfileoperationpanel.Controls.Add(this.btnScheduleCancel);
            this.scdfileoperationpanel.Controls.Add(this.lblcfgfile);
            this.scdfileoperationpanel.Controls.Add(this.btnSelectcfgfile);
            this.scdfileoperationpanel.Controls.Add(this.txtClearCMRI);
            this.scdfileoperationpanel.Controls.Add(this.lblClearCMRI);
            this.scdfileoperationpanel.Controls.Add(this.txtLSDays);
            this.scdfileoperationpanel.Controls.Add(this.lblLSDays);
            this.scdfileoperationpanel.Location = new System.Drawing.Point(12, 370);
            this.scdfileoperationpanel.Name = "scdfileoperationpanel";
            this.scdfileoperationpanel.Size = new System.Drawing.Size(716, 160);
            this.scdfileoperationpanel.TabIndex = 61;
            this.scdfileoperationpanel.BackColor = System.Drawing.Color.FromArgb(248, 250, 253);
            // 
            // chkisLPRSchedule
            // 
            this.chkisLPRSchedule.AutoSize = true;
            this.chkisLPRSchedule.Location = new System.Drawing.Point(20, 8);
            this.chkisLPRSchedule.Name = "chkisLPRSchedule";
            this.chkisLPRSchedule.Size = new System.Drawing.Size(110, 20);
            this.chkisLPRSchedule.TabIndex = 60;
            this.chkisLPRSchedule.Text = "  LPR Schedule";
            this.chkisLPRSchedule.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.chkisLPRSchedule.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            this.chkisLPRSchedule.UseVisualStyleBackColor = true;
            this.chkisLPRSchedule.CheckedChanged += new System.EventHandler(this.chkisLPRSchedule_CheckedChanged);
            // 
            // btnCMRISchedule
            // 
            this.btnCMRISchedule.Location = new System.Drawing.Point(530, 120);
            this.btnCMRISchedule.Name = "btnCMRISchedule";
            this.btnCMRISchedule.Size = new System.Drawing.Size(120, 32);
            this.btnCMRISchedule.TabIndex = 47;
            this.btnCMRISchedule.Text = "✔  Generate";
            this.btnCMRISchedule.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.btnCMRISchedule.UseVisualStyleBackColor = false;
            this.btnCMRISchedule.BackColor = System.Drawing.Color.FromArgb(16, 137, 62);
            this.btnCMRISchedule.ForeColor = System.Drawing.Color.White;
            this.btnCMRISchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCMRISchedule.FlatAppearance.BorderSize = 0;
            this.btnCMRISchedule.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(14, 115, 52);
            this.btnCMRISchedule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCMRISchedule.Click += new System.EventHandler(this.btnCMRISchedule_Click);
            // 
            // lngLabel3
            // 
            this.lngLabel3.AutoSize = true;
            this.lngLabel3.Location = new System.Drawing.Point(395, 31);
            this.lngLabel3.Name = "lngLabel3";
            this.lngLabel3.Size = new System.Drawing.Size(40, 17);
            this.lngLabel3.TabIndex = 59;
            this.lngLabel3.Text = "(1 - 90)";
            this.lngLabel3.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lngLabel3.ForeColor = System.Drawing.Color.FromArgb(140, 140, 160);
            this.lngLabel3.TranslationKey = null;
            // 
            // btnScheduleCancel
            // 
            this.btnScheduleCancel.Location = new System.Drawing.Point(655, 120);
            this.btnScheduleCancel.Name = "btnScheduleCancel";
            this.btnScheduleCancel.Size = new System.Drawing.Size(50, 32);
            this.btnScheduleCancel.TabIndex = 48;
            this.btnScheduleCancel.Text = "Close";
            this.btnScheduleCancel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnScheduleCancel.UseVisualStyleBackColor = false;
            this.btnScheduleCancel.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
            this.btnScheduleCancel.ForeColor = System.Drawing.Color.White;
            this.btnScheduleCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScheduleCancel.FlatAppearance.BorderSize = 0;
            this.btnScheduleCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnScheduleCancel.Click += new System.EventHandler(this.btnScheduleCancel_Click);
            // 
            // lblcfgfile
            // 
            this.lblcfgfile.BackColor = System.Drawing.Color.White;
            this.lblcfgfile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblcfgfile.Location = new System.Drawing.Point(20, 120);
            this.lblcfgfile.Multiline = true;
            this.lblcfgfile.Name = "lblcfgfile";
            this.lblcfgfile.ReadOnly = true;
            this.lblcfgfile.Size = new System.Drawing.Size(500, 30);
            this.lblcfgfile.TabIndex = 58;
            this.lblcfgfile.Text = "cfg File Path";
            this.lblcfgfile.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            this.lblcfgfile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // btnSelectcfgfile
            // 
            this.btnSelectcfgfile.Location = new System.Drawing.Point(20, 80);
            this.btnSelectcfgfile.Name = "btnSelectcfgfile";
            this.btnSelectcfgfile.Size = new System.Drawing.Size(130, 32);
            this.btnSelectcfgfile.TabIndex = 53;
            this.btnSelectcfgfile.Text = "📂  Browse cfg";
            this.btnSelectcfgfile.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSelectcfgfile.UseVisualStyleBackColor = false;
            this.btnSelectcfgfile.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSelectcfgfile.ForeColor = System.Drawing.Color.White;
            this.btnSelectcfgfile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectcfgfile.FlatAppearance.BorderSize = 0;
            this.btnSelectcfgfile.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 100, 190);
            this.btnSelectcfgfile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectcfgfile.Click += new System.EventHandler(this.btnSelectcfgfile_Click);
            // 
            // txtClearCMRI
            // 
            this.txtClearCMRI.Location = new System.Drawing.Point(270, 50);
            this.txtClearCMRI.MaxLength = 5;
            this.txtClearCMRI.Name = "txtClearCMRI";
            this.txtClearCMRI.PasswordChar = '*';
            this.txtClearCMRI.Size = new System.Drawing.Size(120, 25);
            this.txtClearCMRI.TabIndex = 57;
            this.txtClearCMRI.Text = "12345";
            this.txtClearCMRI.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtClearCMRI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClearCMRI.BackColor = System.Drawing.Color.White;
            this.txtClearCMRI.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtClearCMRI_KeyPress);
            // 
            // lblClearCMRI
            // 
            this.lblClearCMRI.AutoSize = true;
            this.lblClearCMRI.Location = new System.Drawing.Point(20, 53);
            this.lblClearCMRI.Name = "lblClearCMRI";
            this.lblClearCMRI.Size = new System.Drawing.Size(130, 17);
            this.lblClearCMRI.TabIndex = 55;
            this.lblClearCMRI.Text = "🔒  Clear CMRI Password";
            this.lblClearCMRI.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblClearCMRI.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.lblClearCMRI.TranslationKey = null;
            // 
            // txtLSDays
            // 
            this.txtLSDays.Location = new System.Drawing.Point(270, 28);
            this.txtLSDays.MaxLength = 2;
            this.txtLSDays.Name = "txtLSDays";
            this.txtLSDays.Size = new System.Drawing.Size(120, 25);
            this.txtLSDays.TabIndex = 56;
            this.txtLSDays.Text = "90";
            this.txtLSDays.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtLSDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLSDays.BackColor = System.Drawing.Color.White;
            this.txtLSDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLSDays_KeyPress);
            // 
            // lblLSDays
            // 
            this.lblLSDays.AutoSize = true;
            this.lblLSDays.Location = new System.Drawing.Point(20, 31);
            this.lblLSDays.Name = "lblLSDays";
            this.lblLSDays.Size = new System.Drawing.Size(200, 17);
            this.lblLSDays.TabIndex = 54;
            this.lblLSDays.Text = "📅  Load Survey Readout Days";
            this.lblLSDays.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblLSDays.ForeColor = System.Drawing.Color.FromArgb(60, 60, 80);
            this.lblLSDays.TranslationKey = null;
            // 
            // lngLabel2
            // 
            this.lngLabel2.AutoSize = true;
            this.lngLabel2.Location = new System.Drawing.Point(460, 32);
            this.lngLabel2.Name = "lngLabel2";
            this.lngLabel2.Size = new System.Drawing.Size(100, 17);
            this.lngLabel2.TabIndex = 48;
            this.lngLabel2.Text = "✅  Selected Meters";
            this.lngLabel2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lngLabel2.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            this.lngLabel2.TranslationKey = null;
            // 
            // lngLabel1
            // 
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Location = new System.Drawing.Point(30, 32);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(100, 17);
            this.lngLabel1.TabIndex = 47;
            this.lngLabel1.Text = "📋  Available Meters";
            this.lngLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.lngLabel1.ForeColor = System.Drawing.Color.FromArgb(30, 60, 110);
            this.lngLabel1.TranslationKey = null;
            // 
            // btnPushMove
            // 
            this.btnPushMove.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPushMove.Location = new System.Drawing.Point(310, 100);
            this.btnPushMove.Name = "btnPushMove";
            this.btnPushMove.Size = new System.Drawing.Size(110, 34);
            this.btnPushMove.TabIndex = 1;
            this.btnPushMove.Text = "  ▶  Add";
            this.btnPushMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPushMove.UseVisualStyleBackColor = false;
            this.btnPushMove.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnPushMove.ForeColor = System.Drawing.Color.White;
            this.btnPushMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushMove.FlatAppearance.BorderSize = 0;
            this.btnPushMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 100, 190);
            this.btnPushMove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPushMove.Click += new System.EventHandler(this.btnPushMove_Click);
            // 
            // btnPushRemove
            // 
            this.btnPushRemove.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPushRemove.Location = new System.Drawing.Point(310, 180);
            this.btnPushRemove.Name = "btnPushRemove";
            this.btnPushRemove.Size = new System.Drawing.Size(110, 34);
            this.btnPushRemove.TabIndex = 3;
            this.btnPushRemove.Text = "◀  Remove";
            this.btnPushRemove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPushRemove.UseVisualStyleBackColor = false;
            this.btnPushRemove.BackColor = System.Drawing.Color.FromArgb(220, 80, 80);
            this.btnPushRemove.ForeColor = System.Drawing.Color.White;
            this.btnPushRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushRemove.FlatAppearance.BorderSize = 0;
            this.btnPushRemove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(195, 60, 60);
            this.btnPushRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPushRemove.Click += new System.EventHandler(this.btnPushRemove_Click);
            // 
            // btnPushRemoveAll
            // 
            this.btnPushRemoveAll.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPushRemoveAll.Location = new System.Drawing.Point(310, 220);
            this.btnPushRemoveAll.Name = "btnPushRemoveAll";
            this.btnPushRemoveAll.Size = new System.Drawing.Size(110, 34);
            this.btnPushRemoveAll.TabIndex = 4;
            this.btnPushRemoveAll.Text = "◀◀  All";
            this.btnPushRemoveAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPushRemoveAll.UseVisualStyleBackColor = false;
            this.btnPushRemoveAll.BackColor = System.Drawing.Color.FromArgb(220, 80, 80);
            this.btnPushRemoveAll.ForeColor = System.Drawing.Color.White;
            this.btnPushRemoveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushRemoveAll.FlatAppearance.BorderSize = 0;
            this.btnPushRemoveAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(195, 60, 60);
            this.btnPushRemoveAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPushRemoveAll.Click += new System.EventHandler(this.btnPushRemoveAll_Click);
            // 
            // btnPushMoveAll
            // 
            this.btnPushMoveAll.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPushMoveAll.Location = new System.Drawing.Point(310, 140);
            this.btnPushMoveAll.Name = "btnPushMoveAll";
            this.btnPushMoveAll.Size = new System.Drawing.Size(110, 34);
            this.btnPushMoveAll.TabIndex = 2;
            this.btnPushMoveAll.Text = "▶▶  All";
            this.btnPushMoveAll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPushMoveAll.UseVisualStyleBackColor = false;
            this.btnPushMoveAll.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnPushMoveAll.ForeColor = System.Drawing.Color.White;
            this.btnPushMoveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushMoveAll.FlatAppearance.BorderSize = 0;
            this.btnPushMoveAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(0, 100, 190);
            this.btnPushMoveAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPushMoveAll.Click += new System.EventHandler(this.btnPushMoveAll_Click);
            // 
            // listBoxSelectedMeters
            // 
            this.listBoxSelectedMeters.FormattingEnabled = true;
            this.listBoxSelectedMeters.Location = new System.Drawing.Point(460, 56);
            this.listBoxSelectedMeters.Name = "listBoxSelectedMeters";
            this.listBoxSelectedMeters.Size = new System.Drawing.Size(240, 290);
            this.listBoxSelectedMeters.TabIndex = 5;
            this.listBoxSelectedMeters.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.listBoxSelectedMeters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxSelectedMeters.BackColor = System.Drawing.Color.White;
            // 
            // listBoxAvailableMeters
            // 
            this.listBoxAvailableMeters.FormattingEnabled = true;
            this.listBoxAvailableMeters.Location = new System.Drawing.Point(30, 56);
            this.listBoxAvailableMeters.Name = "listBoxAvailableMeters";
            this.listBoxAvailableMeters.Size = new System.Drawing.Size(240, 290);
            this.listBoxAvailableMeters.TabIndex = 0;
            this.listBoxAvailableMeters.Font = new System.Drawing.Font("Consolas", 9.5F);
            this.listBoxAvailableMeters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxAvailableMeters.BackColor = System.Drawing.Color.White;
            // 
            // CMRIScheduleFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(235, 240, 248);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(780, 585);
            this.Controls.Add(this.CmriAuth_grpAuth);
            this.Name = "CMRIScheduleFile";
            this.Text = "CMRI Schedule File";
            this.Load += new System.EventHandler(this.CMRIScheduleFile_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CMRIScheduleFile_FormClosing);
            this.Activated += new System.EventHandler(this.CMRIScheduleFile_Activated);
            this.CmriAuth_grpAuth.ResumeLayout(false);
            this.CmriAuth_grpAuth.PerformLayout();
            this.securityfilepanel.ResumeLayout(false);
            this.securityfilepanel.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.scdfileoperationpanel.ResumeLayout(false);
            this.scdfileoperationpanel.PerformLayout();
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
        private System.Windows.Forms.CheckBox chkisLPRSchedule;
        private System.Windows.Forms.Panel scdfileoperationpanel;
        private System.Windows.Forms.Panel securityfilepanel;
        private System.Windows.Forms.Button btnsecurityfile;
        private System.Windows.Forms.Button btnclose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbusername;
        private System.Windows.Forms.TextBox txtusername;
        private System.Windows.Forms.TextBox txtpwd;
        private System.Windows.Forms.Button btnsecuritykey;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbstatus;
	}
}


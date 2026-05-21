namespace CAB.UI
{
    partial class CMRIScheduleFileNonTneb
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
            this.btnPushRemove = new System.Windows.Forms.Button();
            this.btnPushRemoveAll = new System.Windows.Forms.Button();
            this.btnPushMoveAll = new System.Windows.Forms.Button();
            this.listBoxSelectedMeters = new System.Windows.Forms.ListBox();
            this.listBoxAvailableMeters = new System.Windows.Forms.ListBox();
            this.CmriAuth_grpProgrammoption = new System.Windows.Forms.GroupBox();
            this.lblschdfile = new System.Windows.Forms.TextBox();
            this.CmriAuth_btnCancel = new System.Windows.Forms.Button();
            this.CmriAuth_btnOK = new System.Windows.Forms.Button();
            this.chk_LPRPara = new System.Windows.Forms.CheckBox();
            this.GrpLPRPara = new System.Windows.Forms.GroupBox();
            this.chk_Default = new System.Windows.Forms.CheckBox();
            this.chk_MD2 = new System.Windows.Forms.CheckBox();
            this.chk_MD1 = new System.Windows.Forms.CheckBox();
            this.chk_Kvah = new System.Windows.Forms.CheckBox();
            this.chk_KvarhLead = new System.Windows.Forms.CheckBox();
            this.chk_KvarhLag = new System.Windows.Forms.CheckBox();
            this.chk_Kwh = new System.Windows.Forms.CheckBox();
            this.chk_BillingReset = new System.Windows.Forms.CheckBox();
            this.Btn_SelectTouFile = new System.Windows.Forms.Button();
            this.chk_SetTOU = new System.Windows.Forms.CheckBox();
            this.chk_SetTamperIcon = new System.Windows.Forms.CheckBox();
            this.chk_SetRTC = new System.Windows.Forms.CheckBox();
            this.chk_DailyLog = new System.Windows.Forms.CheckBox();
            this.GRPDailyLog = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Highload = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_Rating = new System.Windows.Forms.TextBox();
            this.txt_LowLoad = new System.Windows.Forms.TextBox();
            this.chk_CumFundamentalkWh = new System.Windows.Forms.CheckBox();
            this.chk_MinI = new System.Windows.Forms.CheckBox();
            this.chk_MaxI = new System.Windows.Forms.CheckBox();
            this.chk_MinV = new System.Windows.Forms.CheckBox();
            this.chk_MaxV = new System.Windows.Forms.CheckBox();
            this.chk_MD3 = new System.Windows.Forms.CheckBox();
            this.txt_CTratio = new System.Windows.Forms.TextBox();
            this.chk_CTRatio = new System.Windows.Forms.CheckBox();
            this.CmriAuth_grpAuth.SuspendLayout();
            this.CmriAuth_grpProgrammoption.SuspendLayout();
            this.GrpLPRPara.SuspendLayout();
            this.GRPDailyLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmriAuth_grpAuth
            // 
            this.CmriAuth_grpAuth.Controls.Add(this.lngLabel2);
            this.CmriAuth_grpAuth.Controls.Add(this.lngLabel1);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushMove);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushRemove);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushRemoveAll);
            this.CmriAuth_grpAuth.Controls.Add(this.btnPushMoveAll);
            this.CmriAuth_grpAuth.Controls.Add(this.listBoxSelectedMeters);
            this.CmriAuth_grpAuth.Controls.Add(this.listBoxAvailableMeters);
            this.CmriAuth_grpAuth.Controls.Add(this.CmriAuth_grpProgrammoption);
            this.CmriAuth_grpAuth.Location = new System.Drawing.Point(28, 10);
            this.CmriAuth_grpAuth.Name = "CmriAuth_grpAuth";
            this.CmriAuth_grpAuth.Size = new System.Drawing.Size(653, 503);
            this.CmriAuth_grpAuth.TabIndex = 1;
            this.CmriAuth_grpAuth.TabStop = false;
            this.CmriAuth_grpAuth.Text = "CMRI Schedule File";
            this.CmriAuth_grpAuth.Enter += new System.EventHandler(this.CmriAuth_grpAuth_Enter);
            // 
            // lngLabel2
            // 
            this.lngLabel2.AutoSize = true;
            this.lngLabel2.Location = new System.Drawing.Point(385, 20);
            this.lngLabel2.Name = "lngLabel2";
            this.lngLabel2.Size = new System.Drawing.Size(84, 13);
            this.lngLabel2.TabIndex = 48;
            this.lngLabel2.Text = "Selected Meters";
            this.lngLabel2.TranslationKey = null;
            // 
            // lngLabel1
            // 
            this.lngLabel1.AutoSize = true;
            this.lngLabel1.Location = new System.Drawing.Point(78, 20);
            this.lngLabel1.Name = "lngLabel1";
            this.lngLabel1.Size = new System.Drawing.Size(85, 13);
            this.lngLabel1.TabIndex = 47;
            this.lngLabel1.Text = "Available Meters";
            this.lngLabel1.TranslationKey = null;
            // 
            // btnPushMove
            // 
            this.btnPushMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPushMove.Location = new System.Drawing.Point(304, 86);
            this.btnPushMove.Name = "btnPushMove";
            this.btnPushMove.Size = new System.Drawing.Size(50, 28);
            this.btnPushMove.TabIndex = 1;
            this.btnPushMove.Text = ">";
            this.btnPushMove.UseVisualStyleBackColor = true;
            this.btnPushMove.Click += new System.EventHandler(this.btnPushMove_Click);
            // 
            // btnPushRemove
            // 
            this.btnPushRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPushRemove.Location = new System.Drawing.Point(304, 154);
            this.btnPushRemove.Name = "btnPushRemove";
            this.btnPushRemove.Size = new System.Drawing.Size(50, 28);
            this.btnPushRemove.TabIndex = 3;
            this.btnPushRemove.Text = "<";
            this.btnPushRemove.UseVisualStyleBackColor = true;
            this.btnPushRemove.Click += new System.EventHandler(this.btnPushRemove_Click);
            // 
            // btnPushRemoveAll
            // 
            this.btnPushRemoveAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPushRemoveAll.Location = new System.Drawing.Point(303, 188);
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
            this.btnPushMoveAll.Location = new System.Drawing.Point(304, 120);
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
            this.listBoxSelectedMeters.Location = new System.Drawing.Point(388, 46);
            this.listBoxSelectedMeters.Name = "listBoxSelectedMeters";
            this.listBoxSelectedMeters.Size = new System.Drawing.Size(187, 251);
            this.listBoxSelectedMeters.TabIndex = 5;
            // 
            // listBoxAvailableMeters
            // 
            this.listBoxAvailableMeters.FormattingEnabled = true;
            this.listBoxAvailableMeters.Location = new System.Drawing.Point(81, 46);
            this.listBoxAvailableMeters.Name = "listBoxAvailableMeters";
            this.listBoxAvailableMeters.Size = new System.Drawing.Size(187, 251);
            this.listBoxAvailableMeters.TabIndex = 0;
            // 
            // CmriAuth_grpProgrammoption
            // 
            this.CmriAuth_grpProgrammoption.Controls.Add(this.lblschdfile);
            this.CmriAuth_grpProgrammoption.Controls.Add(this.CmriAuth_btnCancel);
            this.CmriAuth_grpProgrammoption.Controls.Add(this.CmriAuth_btnOK);
            this.CmriAuth_grpProgrammoption.Controls.Add(this.chk_LPRPara);
            this.CmriAuth_grpProgrammoption.Controls.Add(this.GrpLPRPara);
            this.CmriAuth_grpProgrammoption.Controls.Add(this.chk_BillingReset);
            this.CmriAuth_grpProgrammoption.Controls.Add(this.Btn_SelectTouFile);
            this.CmriAuth_grpProgrammoption.Controls.Add(this.chk_SetTOU);
            this.CmriAuth_grpProgrammoption.Controls.Add(this.chk_SetTamperIcon);
            this.CmriAuth_grpProgrammoption.Controls.Add(this.chk_SetRTC);
            this.CmriAuth_grpProgrammoption.Location = new System.Drawing.Point(9, 303);
            this.CmriAuth_grpProgrammoption.Name = "CmriAuth_grpProgrammoption";
            this.CmriAuth_grpProgrammoption.Size = new System.Drawing.Size(638, 197);
            this.CmriAuth_grpProgrammoption.TabIndex = 20;
            this.CmriAuth_grpProgrammoption.TabStop = false;
            // 
            // lblschdfile
            // 
            this.lblschdfile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblschdfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblschdfile.Location = new System.Drawing.Point(2, 166);
            this.lblschdfile.Multiline = true;
            this.lblschdfile.Name = "lblschdfile";
            this.lblschdfile.ReadOnly = true;
            this.lblschdfile.Size = new System.Drawing.Size(472, 26);
            this.lblschdfile.TabIndex = 49;
            this.lblschdfile.Text = "TOU File Path";
            // 
            // CmriAuth_btnCancel
            // 
            this.CmriAuth_btnCancel.Location = new System.Drawing.Point(561, 166);
            this.CmriAuth_btnCancel.Name = "CmriAuth_btnCancel";
            this.CmriAuth_btnCancel.Size = new System.Drawing.Size(75, 28);
            this.CmriAuth_btnCancel.TabIndex = 48;
            this.CmriAuth_btnCancel.Text = "Cancel";
            this.CmriAuth_btnCancel.UseVisualStyleBackColor = true;
            this.CmriAuth_btnCancel.Click += new System.EventHandler(this.CmriAuth_btnCancel_Click);
            // 
            // CmriAuth_btnOK
            // 
            this.CmriAuth_btnOK.Location = new System.Drawing.Point(480, 166);
            this.CmriAuth_btnOK.Name = "CmriAuth_btnOK";
            this.CmriAuth_btnOK.Size = new System.Drawing.Size(75, 28);
            this.CmriAuth_btnOK.TabIndex = 47;
            this.CmriAuth_btnOK.Text = "OK";
            this.CmriAuth_btnOK.UseVisualStyleBackColor = true;
            this.CmriAuth_btnOK.Click += new System.EventHandler(this.CmriAuth_btnOK_Click);
            // 
            // chk_LPRPara
            // 
            this.chk_LPRPara.AutoSize = true;
            this.chk_LPRPara.Location = new System.Drawing.Point(29, 124);
            this.chk_LPRPara.Name = "chk_LPRPara";
            this.chk_LPRPara.Size = new System.Drawing.Size(70, 17);
            this.chk_LPRPara.TabIndex = 7;
            this.chk_LPRPara.Text = "Daily Log";
            this.chk_LPRPara.UseVisualStyleBackColor = true;
            this.chk_LPRPara.CheckedChanged += new System.EventHandler(this.chk_LPRPara_CheckedChanged);
            // 
            // GrpLPRPara
            // 
            this.GrpLPRPara.Controls.Add(this.chk_Default);
            this.GrpLPRPara.Controls.Add(this.chk_MD2);
            this.GrpLPRPara.Controls.Add(this.chk_MD1);
            this.GrpLPRPara.Controls.Add(this.chk_Kvah);
            this.GrpLPRPara.Controls.Add(this.chk_KvarhLead);
            this.GrpLPRPara.Controls.Add(this.chk_KvarhLag);
            this.GrpLPRPara.Controls.Add(this.chk_Kwh);
            this.GrpLPRPara.Enabled = false;
            this.GrpLPRPara.Location = new System.Drawing.Point(297, 10);
            this.GrpLPRPara.Name = "GrpLPRPara";
            this.GrpLPRPara.Size = new System.Drawing.Size(332, 144);
            this.GrpLPRPara.TabIndex = 0;
            this.GrpLPRPara.TabStop = false;
            this.GrpLPRPara.Text = "Daily Log";
            // 
            // chk_Default
            // 
            this.chk_Default.AutoSize = true;
            this.chk_Default.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_Default.ForeColor = System.Drawing.Color.Black;
            this.chk_Default.Location = new System.Drawing.Point(22, 123);
            this.chk_Default.Name = "chk_Default";
            this.chk_Default.Size = new System.Drawing.Size(80, 17);
            this.chk_Default.TabIndex = 11;
            this.chk_Default.Text = "Select All";
            this.chk_Default.UseVisualStyleBackColor = true;
            this.chk_Default.CheckedChanged += new System.EventHandler(this.chk_Default_CheckedChanged);
            // 
            // chk_MD2
            // 
            this.chk_MD2.AutoSize = true;
            this.chk_MD2.Location = new System.Drawing.Point(177, 28);
            this.chk_MD2.Name = "chk_MD2";
            this.chk_MD2.Size = new System.Drawing.Size(75, 17);
            this.chk_MD2.TabIndex = 1;
            this.chk_MD2.Text = "Daily MD2";
            this.chk_MD2.UseVisualStyleBackColor = true;
            // 
            // chk_MD1
            // 
            this.chk_MD1.AutoSize = true;
            this.chk_MD1.Location = new System.Drawing.Point(176, 51);
            this.chk_MD1.Name = "chk_MD1";
            this.chk_MD1.Size = new System.Drawing.Size(75, 17);
            this.chk_MD1.TabIndex = 4;
            this.chk_MD1.Text = "Daily MD1";
            this.chk_MD1.UseVisualStyleBackColor = true;
            // 
            // chk_Kvah
            // 
            this.chk_Kvah.AutoSize = true;
            this.chk_Kvah.Location = new System.Drawing.Point(23, 91);
            this.chk_Kvah.Name = "chk_Kvah";
            this.chk_Kvah.Size = new System.Drawing.Size(108, 17);
            this.chk_Kvah.TabIndex = 9;
            this.chk_Kvah.Text = "Cumulative KVAh";
            this.chk_Kvah.UseVisualStyleBackColor = true;
            // 
            // chk_KvarhLead
            // 
            this.chk_KvarhLead.AutoSize = true;
            this.chk_KvarhLead.Location = new System.Drawing.Point(23, 70);
            this.chk_KvarhLead.Name = "chk_KvarhLead";
            this.chk_KvarhLead.Size = new System.Drawing.Size(146, 17);
            this.chk_KvarhLead.TabIndex = 6;
            this.chk_KvarhLead.Text = "Cumulative KVARh(Lead)";
            this.chk_KvarhLead.UseVisualStyleBackColor = true;
            // 
            // chk_KvarhLag
            // 
            this.chk_KvarhLag.AutoSize = true;
            this.chk_KvarhLag.Location = new System.Drawing.Point(23, 49);
            this.chk_KvarhLag.Name = "chk_KvarhLag";
            this.chk_KvarhLag.Size = new System.Drawing.Size(140, 17);
            this.chk_KvarhLag.TabIndex = 3;
            this.chk_KvarhLag.Text = "Cumulative KVARh(Lag)";
            this.chk_KvarhLag.UseVisualStyleBackColor = true;
            // 
            // chk_Kwh
            // 
            this.chk_Kwh.AutoSize = true;
            this.chk_Kwh.Location = new System.Drawing.Point(23, 28);
            this.chk_Kwh.Name = "chk_Kwh";
            this.chk_Kwh.Size = new System.Drawing.Size(105, 17);
            this.chk_Kwh.TabIndex = 0;
            this.chk_Kwh.Text = "Cumulative KWh";
            this.chk_Kwh.UseVisualStyleBackColor = true;
            // 
            // chk_BillingReset
            // 
            this.chk_BillingReset.AutoSize = true;
            this.chk_BillingReset.Location = new System.Drawing.Point(29, 99);
            this.chk_BillingReset.Name = "chk_BillingReset";
            this.chk_BillingReset.Size = new System.Drawing.Size(84, 17);
            this.chk_BillingReset.TabIndex = 6;
            this.chk_BillingReset.Text = "Billing Reset";
            this.chk_BillingReset.UseVisualStyleBackColor = true;
            this.chk_BillingReset.CheckedChanged += new System.EventHandler(this.chk_BillingReset_CheckedChanged);
            // 
            // Btn_SelectTouFile
            // 
            this.Btn_SelectTouFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_SelectTouFile.Location = new System.Drawing.Point(122, 21);
            this.Btn_SelectTouFile.Name = "Btn_SelectTouFile";
            this.Btn_SelectTouFile.Size = new System.Drawing.Size(63, 22);
            this.Btn_SelectTouFile.TabIndex = 3;
            this.Btn_SelectTouFile.Text = "Browse..";
            this.Btn_SelectTouFile.UseVisualStyleBackColor = true;
            this.Btn_SelectTouFile.Click += new System.EventHandler(this.Btn_SelectTouFile_Click);
            // 
            // chk_SetTOU
            // 
            this.chk_SetTOU.AutoSize = true;
            this.chk_SetTOU.Location = new System.Drawing.Point(29, 24);
            this.chk_SetTOU.Name = "chk_SetTOU";
            this.chk_SetTOU.Size = new System.Drawing.Size(64, 17);
            this.chk_SetTOU.TabIndex = 2;
            this.chk_SetTOU.Text = "Set Tou";
            this.chk_SetTOU.UseVisualStyleBackColor = true;
            this.chk_SetTOU.CheckedChanged += new System.EventHandler(this.chk_SetTOU_CheckedChanged);
            // 
            // chk_SetTamperIcon
            // 
            this.chk_SetTamperIcon.AutoSize = true;
            this.chk_SetTamperIcon.Location = new System.Drawing.Point(29, 74);
            this.chk_SetTamperIcon.Name = "chk_SetTamperIcon";
            this.chk_SetTamperIcon.Size = new System.Drawing.Size(140, 17);
            this.chk_SetTamperIcon.TabIndex = 5;
            this.chk_SetTamperIcon.Text = "Mgt. Tamper icon Reset";
            this.chk_SetTamperIcon.UseVisualStyleBackColor = true;
            this.chk_SetTamperIcon.CheckedChanged += new System.EventHandler(this.chk_SetTamperIcon_CheckedChanged);
            // 
            // chk_SetRTC
            // 
            this.chk_SetRTC.AutoSize = true;
            this.chk_SetRTC.Location = new System.Drawing.Point(29, 49);
            this.chk_SetRTC.Name = "chk_SetRTC";
            this.chk_SetRTC.Size = new System.Drawing.Size(97, 17);
            this.chk_SetRTC.TabIndex = 4;
            this.chk_SetRTC.Text = "Set Meter RTC";
            this.chk_SetRTC.UseVisualStyleBackColor = true;
            this.chk_SetRTC.CheckedChanged += new System.EventHandler(this.chk_SetRTC_CheckedChanged);
            // 
            // chk_DailyLog
            // 
            this.chk_DailyLog.AutoSize = true;
            this.chk_DailyLog.Enabled = false;
            this.chk_DailyLog.Location = new System.Drawing.Point(705, 133);
            this.chk_DailyLog.Name = "chk_DailyLog";
            this.chk_DailyLog.Size = new System.Drawing.Size(204, 17);
            this.chk_DailyLog.TabIndex = 8;
            this.chk_DailyLog.Text = "Transformer Loading Event Threshold";
            this.chk_DailyLog.UseVisualStyleBackColor = true;
            this.chk_DailyLog.Visible = false;
            this.chk_DailyLog.CheckedChanged += new System.EventHandler(this.chk_DailyLog_CheckedChanged);
            // 
            // GRPDailyLog
            // 
            this.GRPDailyLog.Controls.Add(this.label3);
            this.GRPDailyLog.Controls.Add(this.label2);
            this.GRPDailyLog.Controls.Add(this.label4);
            this.GRPDailyLog.Controls.Add(this.txt_Highload);
            this.GRPDailyLog.Controls.Add(this.label5);
            this.GRPDailyLog.Controls.Add(this.label6);
            this.GRPDailyLog.Controls.Add(this.label7);
            this.GRPDailyLog.Controls.Add(this.txt_Rating);
            this.GRPDailyLog.Controls.Add(this.txt_LowLoad);
            this.GRPDailyLog.Enabled = false;
            this.GRPDailyLog.Location = new System.Drawing.Point(706, 41);
            this.GRPDailyLog.Name = "GRPDailyLog";
            this.GRPDailyLog.Size = new System.Drawing.Size(151, 81);
            this.GRPDailyLog.TabIndex = 32;
            this.GRPDailyLog.TabStop = false;
            this.GRPDailyLog.Text = "Transformer Loading Event Threshold";
            this.GRPDailyLog.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(277, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Transformer Rating";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Low load Threshold";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "High load Threshold";
            // 
            // txt_Highload
            // 
            this.txt_Highload.Location = new System.Drawing.Point(20, 49);
            this.txt_Highload.MaxLength = 3;
            this.txt_Highload.Name = "txt_Highload";
            this.txt_Highload.Size = new System.Drawing.Size(63, 20);
            this.txt_Highload.TabIndex = 0;
            this.txt_Highload.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Highload_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(349, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "KVA";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(89, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "%";
            // 
            // txt_Rating
            // 
            this.txt_Rating.Location = new System.Drawing.Point(280, 49);
            this.txt_Rating.MaxLength = 3;
            this.txt_Rating.Name = "txt_Rating";
            this.txt_Rating.Size = new System.Drawing.Size(63, 20);
            this.txt_Rating.TabIndex = 2;
            this.txt_Rating.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Rating_KeyDown);
            // 
            // txt_LowLoad
            // 
            this.txt_LowLoad.Location = new System.Drawing.Point(157, 49);
            this.txt_LowLoad.MaxLength = 3;
            this.txt_LowLoad.Name = "txt_LowLoad";
            this.txt_LowLoad.Size = new System.Drawing.Size(63, 20);
            this.txt_LowLoad.TabIndex = 1;
            this.txt_LowLoad.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_LowLoad_KeyDown);
            // 
            // chk_CumFundamentalkWh
            // 
            this.chk_CumFundamentalkWh.AutoSize = true;
            this.chk_CumFundamentalkWh.Location = new System.Drawing.Point(714, 234);
            this.chk_CumFundamentalkWh.Name = "chk_CumFundamentalkWh";
            this.chk_CumFundamentalkWh.Size = new System.Drawing.Size(169, 17);
            this.chk_CumFundamentalkWh.TabIndex = 12;
            this.chk_CumFundamentalkWh.Text = "Cumulative Fundamental KWh";
            this.chk_CumFundamentalkWh.UseVisualStyleBackColor = true;
            this.chk_CumFundamentalkWh.Visible = false;
            // 
            // chk_MinI
            // 
            this.chk_MinI.AutoSize = true;
            this.chk_MinI.Location = new System.Drawing.Point(716, 348);
            this.chk_MinI.Name = "chk_MinI";
            this.chk_MinI.Size = new System.Drawing.Size(74, 17);
            this.chk_MinI.TabIndex = 8;
            this.chk_MinI.Text = "Min(Avg I)";
            this.chk_MinI.UseVisualStyleBackColor = true;
            this.chk_MinI.Visible = false;
            // 
            // chk_MaxI
            // 
            this.chk_MaxI.AutoSize = true;
            this.chk_MaxI.Location = new System.Drawing.Point(717, 282);
            this.chk_MaxI.Name = "chk_MaxI";
            this.chk_MaxI.Size = new System.Drawing.Size(77, 17);
            this.chk_MaxI.TabIndex = 10;
            this.chk_MaxI.Text = "Max(Avg I)";
            this.chk_MaxI.UseVisualStyleBackColor = true;
            this.chk_MaxI.Visible = false;
            // 
            // chk_MinV
            // 
            this.chk_MinV.AutoSize = true;
            this.chk_MinV.Location = new System.Drawing.Point(716, 325);
            this.chk_MinV.Name = "chk_MinV";
            this.chk_MinV.Size = new System.Drawing.Size(81, 17);
            this.chk_MinV.TabIndex = 5;
            this.chk_MinV.Text = "Min (Avg V)";
            this.chk_MinV.UseVisualStyleBackColor = true;
            this.chk_MinV.Visible = false;
            // 
            // chk_MaxV
            // 
            this.chk_MaxV.AutoSize = true;
            this.chk_MaxV.Location = new System.Drawing.Point(717, 259);
            this.chk_MaxV.Name = "chk_MaxV";
            this.chk_MaxV.Size = new System.Drawing.Size(84, 17);
            this.chk_MaxV.TabIndex = 7;
            this.chk_MaxV.Text = "Max (Avg V)";
            this.chk_MaxV.UseVisualStyleBackColor = true;
            this.chk_MaxV.Visible = false;
            // 
            // chk_MD3
            // 
            this.chk_MD3.AutoSize = true;
            this.chk_MD3.Location = new System.Drawing.Point(716, 301);
            this.chk_MD3.Name = "chk_MD3";
            this.chk_MD3.Size = new System.Drawing.Size(75, 17);
            this.chk_MD3.TabIndex = 2;
            this.chk_MD3.Text = "Daily MD3";
            this.chk_MD3.UseVisualStyleBackColor = true;
            this.chk_MD3.Visible = false;
            // 
            // txt_CTratio
            // 
            this.txt_CTratio.Location = new System.Drawing.Point(798, 156);
            this.txt_CTratio.MaxLength = 3;
            this.txt_CTratio.Name = "txt_CTratio";
            this.txt_CTratio.Size = new System.Drawing.Size(63, 20);
            this.txt_CTratio.TabIndex = 1;
            this.txt_CTratio.Visible = false;
            // 
            // chk_CTRatio
            // 
            this.chk_CTRatio.AutoSize = true;
            this.chk_CTRatio.Enabled = false;
            this.chk_CTRatio.Location = new System.Drawing.Point(705, 159);
            this.chk_CTRatio.Name = "chk_CTRatio";
            this.chk_CTRatio.Size = new System.Drawing.Size(87, 17);
            this.chk_CTRatio.TabIndex = 0;
            this.chk_CTRatio.Text = "Set CT Ratio";
            this.chk_CTRatio.UseVisualStyleBackColor = true;
            this.chk_CTRatio.Visible = false;
            this.chk_CTRatio.CheckedChanged += new System.EventHandler(this.chk_CTRatio_CheckedChanged);
            // 
            // CMRIScheduleFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(705, 563);
            this.Controls.Add(this.chk_CumFundamentalkWh);
            this.Controls.Add(this.chk_DailyLog);
            this.Controls.Add(this.chk_MinI);
            this.Controls.Add(this.CmriAuth_grpAuth);
            this.Controls.Add(this.chk_MaxI);
            this.Controls.Add(this.GRPDailyLog);
            this.Controls.Add(this.chk_MinV);
            this.Controls.Add(this.chk_MaxV);
            this.Controls.Add(this.txt_CTratio);
            this.Controls.Add(this.chk_MD3);
            this.Controls.Add(this.chk_CTRatio);
            this.Name = "CMRIScheduleFile";
            this.StatusMessage = "";
            this.Text = "CMRI Schedule File";
            this.Load += new System.EventHandler(this.CMRIScheduleFile_Load);
            this.Activated += new System.EventHandler(this.CMRIScheduleFile_Activated);
            this.CmriAuth_grpAuth.ResumeLayout(false);
            this.CmriAuth_grpAuth.PerformLayout();
            this.CmriAuth_grpProgrammoption.ResumeLayout(false);
            this.CmriAuth_grpProgrammoption.PerformLayout();
            this.GrpLPRPara.ResumeLayout(false);
            this.GrpLPRPara.PerformLayout();
            this.GRPDailyLog.ResumeLayout(false);
            this.GRPDailyLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.GroupBox CmriAuth_grpAuth;
		private System.Windows.Forms.Button btnPushMove;
		private System.Windows.Forms.Button btnPushRemove;
		private System.Windows.Forms.Button btnPushRemoveAll;
		private System.Windows.Forms.Button btnPushMoveAll;
		private System.Windows.Forms.ListBox listBoxSelectedMeters;
		private System.Windows.Forms.ListBox listBoxAvailableMeters;
		private System.Windows.Forms.GroupBox CmriAuth_grpProgrammoption;
		private System.Windows.Forms.CheckBox chk_DailyLog;
		private System.Windows.Forms.CheckBox chk_LPRPara;
		private System.Windows.Forms.GroupBox GRPDailyLog;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txt_Highload;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txt_Rating;
		private System.Windows.Forms.TextBox txt_LowLoad;
		private System.Windows.Forms.GroupBox GrpLPRPara;
		private System.Windows.Forms.CheckBox chk_Default;
		private System.Windows.Forms.CheckBox chk_MinI;
		private System.Windows.Forms.CheckBox chk_MaxI;
		private System.Windows.Forms.CheckBox chk_MinV;
		private System.Windows.Forms.CheckBox chk_MaxV;
		private System.Windows.Forms.CheckBox chk_MD3;
		private System.Windows.Forms.CheckBox chk_MD2;
		private System.Windows.Forms.CheckBox chk_MD1;
		private System.Windows.Forms.CheckBox chk_Kvah;
		private System.Windows.Forms.CheckBox chk_KvarhLead;
		private System.Windows.Forms.CheckBox chk_KvarhLag;
		private System.Windows.Forms.CheckBox chk_Kwh;
		private System.Windows.Forms.TextBox txt_CTratio;
		private System.Windows.Forms.CheckBox chk_BillingReset;
		private System.Windows.Forms.CheckBox chk_CTRatio;
		private System.Windows.Forms.Button Btn_SelectTouFile;
		private System.Windows.Forms.CheckBox chk_SetTOU;
		private System.Windows.Forms.CheckBox chk_SetTamperIcon;
        private System.Windows.Forms.CheckBox chk_SetRTC;
		private CAB.UI.Controls.CABLabel lngLabel2;
		private CAB.UI.Controls.CABLabel lngLabel1;
		private System.Windows.Forms.CheckBox chk_CumFundamentalkWh;
        private System.Windows.Forms.TextBox lblschdfile;
        private System.Windows.Forms.Button CmriAuth_btnCancel;
        private System.Windows.Forms.Button CmriAuth_btnOK;
	}
}
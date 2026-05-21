namespace CAB.UI
{
	partial class UserInformation
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lngLabel_CU_Note = new CAB.UI.Controls.CABLabel();
            this.cmbDesignation = new System.Windows.Forms.ComboBox();
            this.lngLabel_CU_LoginID = new CAB.UI.Controls.CABLabel();
            this.txtLoginID = new System.Windows.Forms.TextBox();
            this.lngLabel_CU_Designation = new CAB.UI.Controls.CABLabel();
            this.lngLabel_CU_ConfirmPassword = new CAB.UI.Controls.CABLabel();
            this.lngLabel_CU_Password = new CAB.UI.Controls.CABLabel();
            this.lngLabel_CU_Category = new CAB.UI.Controls.CABLabel();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.lblUserName = new CAB.UI.Controls.CABLabel();
            this.CU_btnCancel = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.CU_btnSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CU_chkdatacollect = new System.Windows.Forms.CheckBox();
            this.CU_chkMRI = new System.Windows.Forms.CheckBox();
            this.CU_chkProgramming = new System.Windows.Forms.CheckBox();
            this.CU_chkArchival = new System.Windows.Forms.CheckBox();
            this.CU_chkview = new System.Windows.Forms.CheckBox();
            this.CU_chkadmin = new System.Windows.Forms.CheckBox();
            this.CU_chkdatastore = new System.Windows.Forms.CheckBox();
            this.CU_chkConMgmt = new System.Windows.Forms.CheckBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(577, 476);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lngLabel_CU_Note);
            this.groupBox1.Controls.Add(this.cmbDesignation);
            this.groupBox1.Controls.Add(this.lngLabel_CU_LoginID);
            this.groupBox1.Controls.Add(this.txtLoginID);
            this.groupBox1.Controls.Add(this.lngLabel_CU_Designation);
            this.groupBox1.Controls.Add(this.lngLabel_CU_ConfirmPassword);
            this.groupBox1.Controls.Add(this.lngLabel_CU_Password);
            this.groupBox1.Controls.Add(this.lngLabel_CU_Category);
            this.groupBox1.Controls.Add(this.txtConfirmPassword);
            this.groupBox1.Controls.Add(this.lblUserName);
            this.groupBox1.Controls.Add(this.CU_btnCancel);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.CU_btnSave);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.cmbCategory);
            this.groupBox1.Location = new System.Drawing.Point(25, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(526, 442);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create User";
            // 
            // lngLabel_CU_Note
            // 
            this.lngLabel_CU_Note.AutoSize = true;
            this.lngLabel_CU_Note.Location = new System.Drawing.Point(47, 25);
            this.lngLabel_CU_Note.Name = "lngLabel_CU_Note";
            this.lngLabel_CU_Note.Size = new System.Drawing.Size(244, 13);
            this.lngLabel_CU_Note.TabIndex = 24;
            this.lngLabel_CU_Note.Text = "Note: Star marked (  * ) Field is mandatory to enter.";
            this.lngLabel_CU_Note.TranslationKey = "L000009";
            // 
            // cmbDesignation
            // 
            this.cmbDesignation.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbDesignation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDesignation.FormattingEnabled = true;
            this.cmbDesignation.Items.AddRange(new object[] {
            "Administrator",
            "Master",
            "Utility",
            "Reader",
            "Data store administrator"});
            this.cmbDesignation.Location = new System.Drawing.Point(181, 123);
            this.cmbDesignation.Name = "cmbDesignation";
            this.cmbDesignation.Size = new System.Drawing.Size(146, 21);
            this.cmbDesignation.TabIndex = 23;
            // 
            // lngLabel_CU_LoginID
            // 
            this.lngLabel_CU_LoginID.AutoSize = true;
            this.lngLabel_CU_LoginID.Location = new System.Drawing.Point(41, 160);
            this.lngLabel_CU_LoginID.Name = "lngLabel_CU_LoginID";
            this.lngLabel_CU_LoginID.Size = new System.Drawing.Size(54, 13);
            this.lngLabel_CU_LoginID.TabIndex = 22;
            this.lngLabel_CU_LoginID.Text = "* Login ID";
            this.lngLabel_CU_LoginID.TranslationKey = "L000005";
            // 
            // txtLoginID
            // 
            this.txtLoginID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLoginID.Location = new System.Drawing.Point(181, 159);
            this.txtLoginID.MaxLength = 10;
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.PasswordChar = '*';
            this.txtLoginID.Size = new System.Drawing.Size(146, 20);
            this.txtLoginID.TabIndex = 21;
            this.txtLoginID.UseSystemPasswordChar = true;
            // 
            // lngLabel_CU_Designation
            // 
            this.lngLabel_CU_Designation.AutoSize = true;
            this.lngLabel_CU_Designation.Location = new System.Drawing.Point(47, 125);
            this.lngLabel_CU_Designation.Name = "lngLabel_CU_Designation";
            this.lngLabel_CU_Designation.Size = new System.Drawing.Size(63, 13);
            this.lngLabel_CU_Designation.TabIndex = 20;
            this.lngLabel_CU_Designation.Text = "Designation";
            this.lngLabel_CU_Designation.TranslationKey = null;
            // 
            // lngLabel_CU_ConfirmPassword
            // 
            this.lngLabel_CU_ConfirmPassword.AutoSize = true;
            this.lngLabel_CU_ConfirmPassword.Location = new System.Drawing.Point(41, 230);
            this.lngLabel_CU_ConfirmPassword.Name = "lngLabel_CU_ConfirmPassword";
            this.lngLabel_CU_ConfirmPassword.Size = new System.Drawing.Size(98, 13);
            this.lngLabel_CU_ConfirmPassword.TabIndex = 19;
            this.lngLabel_CU_ConfirmPassword.Text = "* Confirm Password";
            this.lngLabel_CU_ConfirmPassword.TranslationKey = "L000008";
            // 
            // lngLabel_CU_Password
            // 
            this.lngLabel_CU_Password.AutoSize = true;
            this.lngLabel_CU_Password.Location = new System.Drawing.Point(41, 195);
            this.lngLabel_CU_Password.Name = "lngLabel_CU_Password";
            this.lngLabel_CU_Password.Size = new System.Drawing.Size(60, 13);
            this.lngLabel_CU_Password.TabIndex = 18;
            this.lngLabel_CU_Password.Text = "* Password";
            this.lngLabel_CU_Password.TranslationKey = "L000007";
            // 
            // lngLabel_CU_Category
            // 
            this.lngLabel_CU_Category.AutoSize = true;
            this.lngLabel_CU_Category.Location = new System.Drawing.Point(46, 90);
            this.lngLabel_CU_Category.Name = "lngLabel_CU_Category";
            this.lngLabel_CU_Category.Size = new System.Drawing.Size(49, 13);
            this.lngLabel_CU_Category.TabIndex = 17;
            this.lngLabel_CU_Category.Text = "Category";
            this.lngLabel_CU_Category.TranslationKey = "L000004";
            // 
            // txtConfirmPassword
            // 
            this.txtConfirmPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtConfirmPassword.Location = new System.Drawing.Point(181, 229);
            this.txtConfirmPassword.MaxLength = 10;
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(146, 20);
            this.txtConfirmPassword.TabIndex = 3;
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(41, 55);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(67, 13);
            this.lblUserName.TabIndex = 16;
            this.lblUserName.Text = "* User Name";
            this.lblUserName.TranslationKey = "L000006";
            // 
            // CU_btnCancel
            // 
            this.CU_btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CU_btnCancel.Location = new System.Drawing.Point(445, 409);
            this.CU_btnCancel.Name = "CU_btnCancel";
            this.CU_btnCancel.Size = new System.Drawing.Size(75, 27);
            this.CU_btnCancel.TabIndex = 15;
            this.CU_btnCancel.Text = "Cancel";
            this.CU_btnCancel.UseVisualStyleBackColor = true;
            this.CU_btnCancel.Click += new System.EventHandler(this.CU_btnCancel_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPassword.Location = new System.Drawing.Point(181, 194);
            this.txtPassword.MaxLength = 10;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(146, 20);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // CU_btnSave
            // 
            this.CU_btnSave.Location = new System.Drawing.Point(365, 409);
            this.CU_btnSave.Name = "CU_btnSave";
            this.CU_btnSave.Size = new System.Drawing.Size(75, 27);
            this.CU_btnSave.TabIndex = 14;
            this.CU_btnSave.Text = "Save";
            this.CU_btnSave.UseVisualStyleBackColor = true;
            this.CU_btnSave.Click += new System.EventHandler(this.CU_btnSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CU_chkdatacollect);
            this.groupBox2.Controls.Add(this.CU_chkMRI);
            this.groupBox2.Controls.Add(this.CU_chkProgramming);
            this.groupBox2.Controls.Add(this.CU_chkArchival);
            this.groupBox2.Controls.Add(this.CU_chkview);
            this.groupBox2.Controls.Add(this.CU_chkadmin);
            this.groupBox2.Controls.Add(this.CU_chkdatastore);
            this.groupBox2.Controls.Add(this.CU_chkConMgmt);
            this.groupBox2.Location = new System.Drawing.Point(22, 271);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(483, 116);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User rights";
            // 
            // CU_chkdatacollect
            // 
            this.CU_chkdatacollect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CU_chkdatacollect.AutoSize = true;
            this.CU_chkdatacollect.Location = new System.Drawing.Point(357, 50);
            this.CU_chkdatacollect.Name = "CU_chkdatacollect";
            this.CU_chkdatacollect.Size = new System.Drawing.Size(97, 17);
            this.CU_chkdatacollect.TabIndex = 11;
            this.CU_chkdatacollect.Text = "Data collection";
            this.CU_chkdatacollect.UseVisualStyleBackColor = true;
            // 
            // CU_chkMRI
            // 
            this.CU_chkMRI.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CU_chkMRI.AutoSize = true;
            this.CU_chkMRI.Location = new System.Drawing.Point(357, 18);
            this.CU_chkMRI.Name = "CU_chkMRI";
            this.CU_chkMRI.Size = new System.Drawing.Size(95, 17);
            this.CU_chkMRI.TabIndex = 7;
            this.CU_chkMRI.Text = "MRI download";
            this.CU_chkMRI.UseVisualStyleBackColor = true;
            // 
            // CU_chkProgramming
            // 
            this.CU_chkProgramming.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CU_chkProgramming.AutoSize = true;
            this.CU_chkProgramming.Location = new System.Drawing.Point(198, 18);
            this.CU_chkProgramming.Name = "CU_chkProgramming";
            this.CU_chkProgramming.Size = new System.Drawing.Size(87, 17);
            this.CU_chkProgramming.TabIndex = 6;
            this.CU_chkProgramming.Text = "Programming";
            this.CU_chkProgramming.UseVisualStyleBackColor = true;
            // 
            // CU_chkArchival
            // 
            this.CU_chkArchival.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CU_chkArchival.AutoSize = true;
            this.CU_chkArchival.Location = new System.Drawing.Point(197, 50);
            this.CU_chkArchival.Name = "CU_chkArchival";
            this.CU_chkArchival.Size = new System.Drawing.Size(88, 17);
            this.CU_chkArchival.TabIndex = 10;
            this.CU_chkArchival.Text = "Data Archive";
            this.CU_chkArchival.UseVisualStyleBackColor = true;
            // 
            // CU_chkview
            // 
            this.CU_chkview.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CU_chkview.AutoSize = true;
            this.CU_chkview.Location = new System.Drawing.Point(197, 82);
            this.CU_chkview.Name = "CU_chkview";
            this.CU_chkview.Size = new System.Drawing.Size(89, 17);
            this.CU_chkview.TabIndex = 13;
            this.CU_chkview.Text = "Reports View";
            this.CU_chkview.UseVisualStyleBackColor = true;
            // 
            // CU_chkadmin
            // 
            this.CU_chkadmin.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CU_chkadmin.AutoSize = true;
            this.CU_chkadmin.Enabled = false;
            this.CU_chkadmin.Location = new System.Drawing.Point(18, 18);
            this.CU_chkadmin.Name = "CU_chkadmin";
            this.CU_chkadmin.Size = new System.Drawing.Size(110, 17);
            this.CU_chkadmin.TabIndex = 5;
            this.CU_chkadmin.Text = "User administrator";
            this.CU_chkadmin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CU_chkadmin.UseVisualStyleBackColor = true;
            // 
            // CU_chkdatastore
            // 
            this.CU_chkdatastore.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CU_chkdatastore.AutoSize = true;
            this.CU_chkdatastore.Location = new System.Drawing.Point(18, 50);
            this.CU_chkdatastore.Name = "CU_chkdatastore";
            this.CU_chkdatastore.Size = new System.Drawing.Size(106, 17);
            this.CU_chkdatastore.TabIndex = 9;
            this.CU_chkdatastore.Text = "Data store admin";
            this.CU_chkdatastore.UseVisualStyleBackColor = true;
            // 
            // CU_chkConMgmt
            // 
            this.CU_chkConMgmt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CU_chkConMgmt.AutoSize = true;
            this.CU_chkConMgmt.Location = new System.Drawing.Point(18, 82);
            this.CU_chkConMgmt.Name = "CU_chkConMgmt";
            this.CU_chkConMgmt.Size = new System.Drawing.Size(70, 17);
            this.CU_chkConMgmt.TabIndex = 12;
            this.CU_chkConMgmt.Text = "Definition";
            this.CU_chkConMgmt.UseVisualStyleBackColor = true;
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtUserName.Location = new System.Drawing.Point(181, 52);
            this.txtUserName.MaxLength = 20;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(146, 20);
            this.txtUserName.TabIndex = 0;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "Administrator",
            "Master",
            "Utility",
            "Reader",
            "Data store administrator"});
            this.cmbCategory.Location = new System.Drawing.Point(181, 87);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(146, 21);
            this.cmbCategory.TabIndex = 1;
            // 
            // UserInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.CU_btnCancel;
            this.ClientSize = new System.Drawing.Size(577, 476);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Name = "UserInformation";
            this.Text = "Create User";
            this.Load += new System.EventHandler(this.CreateUser_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CreateUser_FormClosed);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button CU_btnCancel;
		private System.Windows.Forms.Button CU_btnSave;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox CU_chkdatacollect;
		private System.Windows.Forms.CheckBox CU_chkMRI;
		private System.Windows.Forms.CheckBox CU_chkProgramming;
		private System.Windows.Forms.CheckBox CU_chkArchival;
		private System.Windows.Forms.CheckBox CU_chkview;
		private System.Windows.Forms.CheckBox CU_chkadmin;
		private System.Windows.Forms.CheckBox CU_chkdatastore;
		private System.Windows.Forms.CheckBox CU_chkConMgmt;
		private CAB.UI.Controls.CABLabel lngLabel_CU_Designation;
		private CAB.UI.Controls.CABLabel lngLabel_CU_ConfirmPassword;
		private CAB.UI.Controls.CABLabel lngLabel_CU_Password;
		private CAB.UI.Controls.CABLabel lngLabel_CU_Category;
		private System.Windows.Forms.TextBox txtConfirmPassword;
		private CAB.UI.Controls.CABLabel lblUserName;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.ComboBox cmbCategory;
		private CAB.UI.Controls.CABLabel lngLabel_CU_LoginID;
		private System.Windows.Forms.TextBox txtLoginID;
		private System.Windows.Forms.ComboBox cmbDesignation;
		private CAB.UI.Controls.CABLabel lngLabel_CU_Note;



	}
}
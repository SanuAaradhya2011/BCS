namespace CAB.UI
{
    partial class LoginForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnLogin = new CAB.UI.Controls.CABButton();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.btnCancel = new CAB.UI.Controls.CABButton();
			this.lngLabel1 = new CAB.UI.Controls.CABLabel();
			this.lngLabel2 = new CAB.UI.Controls.CABLabel();
			this.linklblRegister = new System.Windows.Forms.LinkLabel();
			this.lbl_ShowDemo = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(408, 231);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// btnLogin
			// 
			this.btnLogin.Location = new System.Drawing.Point(258, 183);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(56, 25);
			this.btnLogin.TabIndex = 2;
			this.btnLogin.Text = "&Login";
			this.btnLogin.TranslationKey = "B000001";
			this.btnLogin.UseVisualStyleBackColor = true;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// txtPassword
			// 
			this.txtPassword.BackColor = System.Drawing.Color.White;
			this.txtPassword.Location = new System.Drawing.Point(232, 143);
			this.txtPassword.MaxLength = 10;
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(144, 20);
			this.txtPassword.TabIndex = 1;
			this.txtPassword.UseSystemPasswordChar = true;
			// 
			// txtUserName
			// 
			this.txtUserName.BackColor = System.Drawing.Color.White;
			this.txtUserName.Location = new System.Drawing.Point(232, 109);
			this.txtUserName.MaxLength = 20;
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(144, 20);
			this.txtUserName.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(320, 183);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(56, 25);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.TranslationKey = "B000002";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lngLabel1
			// 
			this.lngLabel1.AutoSize = true;
			this.lngLabel1.Location = new System.Drawing.Point(163, 113);
			this.lngLabel1.Name = "lngLabel1";
			this.lngLabel1.Size = new System.Drawing.Size(45, 13);
			this.lngLabel1.TabIndex = 22;
			this.lngLabel1.Text = "Login Id";
			this.lngLabel1.TranslationKey = "L000001";
			// 
			// lngLabel2
			// 
			this.lngLabel2.AutoSize = true;
			this.lngLabel2.Location = new System.Drawing.Point(163, 147);
			this.lngLabel2.Name = "lngLabel2";
			this.lngLabel2.Size = new System.Drawing.Size(53, 13);
			this.lngLabel2.TabIndex = 23;
			this.lngLabel2.Text = "Password";
			this.lngLabel2.TranslationKey = "L000002";
			// 
			// linklblRegister
			// 
			this.linklblRegister.AutoSize = true;
			this.linklblRegister.Location = new System.Drawing.Point(13, 206);
			this.linklblRegister.Name = "linklblRegister";
			this.linklblRegister.Size = new System.Drawing.Size(55, 13);
			this.linklblRegister.TabIndex = 24;
			this.linklblRegister.TabStop = true;
			this.linklblRegister.Text = "linkLabel1";
			this.linklblRegister.Visible = false;
			this.linklblRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linklblRegister_LinkClicked);
			// 
			// lbl_ShowDemo
			// 
			this.lbl_ShowDemo.AutoSize = true;
			this.lbl_ShowDemo.Location = new System.Drawing.Point(13, 190);
			this.lbl_ShowDemo.Name = "lbl_ShowDemo";
			this.lbl_ShowDemo.Size = new System.Drawing.Size(35, 13);
			this.lbl_ShowDemo.TabIndex = 25;
			this.lbl_ShowDemo.Text = "label1";
			// 
			// LoginForm
			// 
			this.AcceptButton = this.btnLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(408, 231);
			this.Controls.Add(this.lbl_ShowDemo);
			this.Controls.Add(this.linklblRegister);
			this.Controls.Add(this.lngLabel2);
			this.Controls.Add(this.lngLabel1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtUserName);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "User Login";
			this.TranslationKey = "C000001";
			this.Load += new System.EventHandler(this.LoginForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private CAB.UI.Controls.CABButton btnLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private CAB.UI.Controls.CABButton btnCancel;
        private CAB.UI.Controls.CABLabel lngLabel1;
		private CAB.UI.Controls.CABLabel lngLabel2;
		public System.Windows.Forms.LinkLabel linklblRegister;
		public System.Windows.Forms.Label lbl_ShowDemo;
    }
}
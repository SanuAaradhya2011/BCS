namespace CABApplication
{
    partial class FTPSettings
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtftpDirectoryName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtftpPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtuserID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFtpIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkAutoUpload = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkAutoUpload);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.txtftpDirectoryName);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtftpPassword);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtuserID);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtFtpIP);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 165);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;         
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(171, 119);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(59, 32);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtftpDirectoryName
            // 
            this.txtftpDirectoryName.BackColor = System.Drawing.SystemColors.Window;
            this.txtftpDirectoryName.Location = new System.Drawing.Point(87, 93);
            this.txtftpDirectoryName.Name = "txtftpDirectoryName";
            this.txtftpDirectoryName.Size = new System.Drawing.Size(143, 20);
            this.txtftpDirectoryName.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "FTP Directory";
            // 
            // txtftpPassword
            // 
            this.txtftpPassword.BackColor = System.Drawing.SystemColors.Window;
            this.txtftpPassword.Location = new System.Drawing.Point(87, 67);
            this.txtftpPassword.Name = "txtftpPassword";
            this.txtftpPassword.Size = new System.Drawing.Size(143, 20);
            this.txtftpPassword.TabIndex = 5;
            this.txtftpPassword.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Password";
            // 
            // txtuserID
            // 
            this.txtuserID.BackColor = System.Drawing.SystemColors.Window;
            this.txtuserID.Location = new System.Drawing.Point(87, 41);
            this.txtuserID.Name = "txtuserID";
            this.txtuserID.Size = new System.Drawing.Size(143, 20);
            this.txtuserID.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Login ID";
            // 
            // txtFtpIP
            // 
            this.txtFtpIP.BackColor = System.Drawing.SystemColors.Window;
            this.txtFtpIP.Location = new System.Drawing.Point(87, 15);
            this.txtFtpIP.Name = "txtFtpIP";
            this.txtFtpIP.Size = new System.Drawing.Size(143, 20);
            this.txtFtpIP.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "FTP IP";
            // 
            // chkAutoUpload
            // 
            this.chkAutoUpload.AutoSize = true;
            this.chkAutoUpload.Location = new System.Drawing.Point(13, 122);
            this.chkAutoUpload.Name = "chkAutoUpload";
            this.chkAutoUpload.Size = new System.Drawing.Size(90, 17);
            this.chkAutoUpload.TabIndex = 10;
            this.chkAutoUpload.Text = "Auto-Upload";
            this.chkAutoUpload.UseVisualStyleBackColor = true;
            // 
            // FTPSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 186);
            this.Controls.Add(this.groupBox2);
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "FTPSettings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Text = "FTP Server Settings";
            this.Load += new System.EventHandler(this.FTPSettings_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtftpDirectoryName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtftpPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtuserID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFtpIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkAutoUpload;
    }
}


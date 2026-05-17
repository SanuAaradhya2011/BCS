namespace CAB.UI
{
    partial class E650MeterPassword
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
            this.grpPwd = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnWriteMeterPassword = new System.Windows.Forms.Button();
            this.grpPwd.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPwd
            // 
            this.grpPwd.Controls.Add(this.btnClose);
            this.grpPwd.Controls.Add(this.btnWriteMeterPassword);
            this.grpPwd.Location = new System.Drawing.Point(4, 4);
            this.grpPwd.Name = "grpPwd";
            this.grpPwd.Size = new System.Drawing.Size(368, 116);
            this.grpPwd.TabIndex = 3;
            this.grpPwd.TabStop = false;
            this.grpPwd.Text = "Change Meter Password";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(197, 36);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(91, 45);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnWriteMeterPassword
            // 
            this.btnWriteMeterPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWriteMeterPassword.Location = new System.Drawing.Point(80, 36);
            this.btnWriteMeterPassword.Name = "btnWriteMeterPassword";
            this.btnWriteMeterPassword.Size = new System.Drawing.Size(111, 45);
            this.btnWriteMeterPassword.TabIndex = 6;
            this.btnWriteMeterPassword.Text = "Set Password";
            this.btnWriteMeterPassword.UseVisualStyleBackColor = false;
            this.btnWriteMeterPassword.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnWriteMeterPassword.ForeColor = System.Drawing.Color.White;
            this.btnWriteMeterPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWriteMeterPassword.FlatAppearance.BorderSize = 0;
            this.btnWriteMeterPassword.Click += new System.EventHandler(this.btnWriteMeterPassword_Click);
            // 
            // E650MeterPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(389, 135);
            this.Controls.Add(this.grpPwd);
            this.Name = "E650MeterPassword";
            this.Text = "Meter Password";
            this.grpPwd.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPwd;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnWriteMeterPassword;

    }
}

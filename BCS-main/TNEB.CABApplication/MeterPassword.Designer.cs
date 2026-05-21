namespace CAB.UI
{
    partial class MeterPassword
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
            this.lblCTRatioValidRange = new System.Windows.Forms.Label();
            this.txtCTRatio = new System.Windows.Forms.TextBox();
            this.lblCTRatio = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.MPP_lblpass = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.CmriAuth_grpAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmriAuth_grpAuth
            // 
            this.CmriAuth_grpAuth.Controls.Add(this.lblCTRatioValidRange);
            this.CmriAuth_grpAuth.Controls.Add(this.txtCTRatio);
            this.CmriAuth_grpAuth.Controls.Add(this.lblCTRatio);
            this.CmriAuth_grpAuth.Controls.Add(this.txtPassword);
            this.CmriAuth_grpAuth.Controls.Add(this.MPP_lblpass);
            this.CmriAuth_grpAuth.Controls.Add(this.btnCancel);
            this.CmriAuth_grpAuth.Controls.Add(this.btnOK);
            this.CmriAuth_grpAuth.Location = new System.Drawing.Point(21, 12);
            this.CmriAuth_grpAuth.Name = "CmriAuth_grpAuth";
            this.CmriAuth_grpAuth.Size = new System.Drawing.Size(367, 166);
            this.CmriAuth_grpAuth.TabIndex = 1;
            this.CmriAuth_grpAuth.TabStop = false;
            this.CmriAuth_grpAuth.Text = "Meter Password";
            // 
            // lblCTRatioValidRange
            // 
            this.lblCTRatioValidRange.AutoSize = true;
            this.lblCTRatioValidRange.Location = new System.Drawing.Point(260, 73);
            this.lblCTRatioValidRange.Name = "lblCTRatioValidRange";
            this.lblCTRatioValidRange.Size = new System.Drawing.Size(101, 13);
            this.lblCTRatioValidRange.TabIndex = 14;
            this.lblCTRatioValidRange.Text = "Valid Range (1-240)";
            // 
            // txtCTRatio
            // 
            this.txtCTRatio.Location = new System.Drawing.Point(130, 70);
            this.txtCTRatio.MaxLength = 3;
            this.txtCTRatio.Name = "txtCTRatio";
            this.txtCTRatio.Size = new System.Drawing.Size(130, 20);
            this.txtCTRatio.TabIndex = 1;
            // 
            // lblCTRatio
            // 
            this.lblCTRatio.AutoSize = true;
            this.lblCTRatio.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCTRatio.Location = new System.Drawing.Point(46, 73);
            this.lblCTRatio.Name = "lblCTRatio";
            this.lblCTRatio.Size = new System.Drawing.Size(49, 13);
            this.lblCTRatio.TabIndex = 13;
            this.lblCTRatio.Text = "CT Ratio";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(130, 35);
            this.txtPassword.MaxLength = 8;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(130, 20);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // MPP_lblpass
            // 
            this.MPP_lblpass.AutoSize = true;
            this.MPP_lblpass.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MPP_lblpass.Location = new System.Drawing.Point(46, 38);
            this.MPP_lblpass.Name = "MPP_lblpass";
            this.MPP_lblpass.Size = new System.Drawing.Size(53, 13);
            this.MPP_lblpass.TabIndex = 12;
            this.MPP_lblpass.Text = "Password";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(196, 123);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(130, 123);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 28);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // MeterPassword
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(400, 194);
            this.Controls.Add(this.CmriAuth_grpAuth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MeterPassword";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Meter Password";
            this.Load += new System.EventHandler(this.MeterPassword_Load);
            this.CmriAuth_grpAuth.ResumeLayout(false);
            this.CmriAuth_grpAuth.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox CmriAuth_grpAuth;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.TextBox txtCTRatio;
        private System.Windows.Forms.Label lblCTRatio;
        public System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label MPP_lblpass;
        private System.Windows.Forms.Label lblCTRatioValidRange;
    }
}
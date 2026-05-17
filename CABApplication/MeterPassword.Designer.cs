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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeterPassword));
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
            this.CmriAuth_grpAuth.Location = new System.Drawing.Point(30, 20);
            this.CmriAuth_grpAuth.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CmriAuth_grpAuth.Name = "CmriAuth_grpAuth";
            this.CmriAuth_grpAuth.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CmriAuth_grpAuth.Size = new System.Drawing.Size(524, 277);
            this.CmriAuth_grpAuth.TabIndex = 1;
            this.CmriAuth_grpAuth.TabStop = false;
            this.CmriAuth_grpAuth.Text = "Meter Password";
            // 
            // lblCTRatioValidRange
            // 
            this.lblCTRatioValidRange.AutoSize = true;
            this.lblCTRatioValidRange.Location = new System.Drawing.Point(371, 122);
            this.lblCTRatioValidRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCTRatioValidRange.Name = "lblCTRatioValidRange";
            this.lblCTRatioValidRange.Size = new System.Drawing.Size(167, 25);
            this.lblCTRatioValidRange.TabIndex = 14;
            this.lblCTRatioValidRange.Text = "Valid Range (1-240)";
            // 
            // txtCTRatio
            // 
            this.txtCTRatio.Location = new System.Drawing.Point(186, 117);
            this.txtCTRatio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCTRatio.MaxLength = 3;
            this.txtCTRatio.Name = "txtCTRatio";
            this.txtCTRatio.Size = new System.Drawing.Size(184, 31);
            this.txtCTRatio.TabIndex = 1;
            // 
            // lblCTRatio
            // 
            this.lblCTRatio.AutoSize = true;
            this.lblCTRatio.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCTRatio.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCTRatio.Location = new System.Drawing.Point(66, 122);
            this.lblCTRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCTRatio.Name = "lblCTRatio";
            this.lblCTRatio.Size = new System.Drawing.Size(84, 25);
            this.lblCTRatio.TabIndex = 13;
            this.lblCTRatio.Text = "CT Ratio";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(186, 58);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPassword.MaxLength = 8;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(184, 31);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // MPP_lblpass
            // 
            this.MPP_lblpass.AutoSize = true;
            this.MPP_lblpass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MPP_lblpass.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MPP_lblpass.Location = new System.Drawing.Point(66, 63);
            this.MPP_lblpass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MPP_lblpass.Name = "MPP_lblpass";
            this.MPP_lblpass.Size = new System.Drawing.Size(92, 25);
            this.MPP_lblpass.TabIndex = 12;
            this.MPP_lblpass.Text = "Password";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(280, 205);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 47);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(186, 205);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(86, 47);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = " OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // MeterPassword
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(571, 323);
            this.Controls.Add(this.CmriAuth_grpAuth);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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

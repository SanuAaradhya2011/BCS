namespace CABApplication
{
    partial class Utility
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
            this.lblUtilityName = new System.Windows.Forms.Label();
            this.lblUtilityPassword = new System.Windows.Forms.Label();
            this.txtUtilityName = new System.Windows.Forms.TextBox();
            this.txtUtilityPassword = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblUtilityName
            // 
            this.lblUtilityName.AutoSize = true;
            this.lblUtilityName.Location = new System.Drawing.Point(13, 24);
            this.lblUtilityName.Name = "lblUtilityName";
            this.lblUtilityName.Size = new System.Drawing.Size(63, 13);
            this.lblUtilityName.TabIndex = 0;
            this.lblUtilityName.Text = "Utility Name";
            // 
            // lblUtilityPassword
            // 
            this.lblUtilityPassword.AutoSize = true;
            this.lblUtilityPassword.Location = new System.Drawing.Point(13, 56);
            this.lblUtilityPassword.Name = "lblUtilityPassword";
            this.lblUtilityPassword.Size = new System.Drawing.Size(81, 13);
            this.lblUtilityPassword.TabIndex = 1;
            this.lblUtilityPassword.Text = "Utility Password";
            // 
            // txtUtilityName
            // 
            this.txtUtilityName.Location = new System.Drawing.Point(99, 24);
            this.txtUtilityName.Name = "txtUtilityName";
            this.txtUtilityName.Size = new System.Drawing.Size(120, 20);
            this.txtUtilityName.TabIndex = 0;
            // 
            // txtUtilityPassword
            // 
            this.txtUtilityPassword.Location = new System.Drawing.Point(99, 56);
            this.txtUtilityPassword.MaxLength = 8;
            this.txtUtilityPassword.Name = "txtUtilityPassword";
            this.txtUtilityPassword.PasswordChar = '*';
            this.txtUtilityPassword.Size = new System.Drawing.Size(120, 20);
            this.txtUtilityPassword.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(99, 90);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // Utility
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 125);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtUtilityPassword);
            this.Controls.Add(this.txtUtilityName);
            this.Controls.Add(this.lblUtilityPassword);
            this.Controls.Add(this.lblUtilityName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Utility";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Utility Selection";
            this.Load += new System.EventHandler(this.Utility_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Utility_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUtilityName;
        private System.Windows.Forms.Label lblUtilityPassword;
        private System.Windows.Forms.TextBox txtUtilityName;
        private System.Windows.Forms.TextBox txtUtilityPassword;
        private System.Windows.Forms.Button btnSave;
    }
}
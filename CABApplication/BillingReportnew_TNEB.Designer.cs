namespace CABApplication.Reports.Forms
{
    partial class BillingReportnew_TNEB
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
            this.chkCumkWh = new System.Windows.Forms.CheckBox();
            this.gbCheckBox = new System.Windows.Forms.GroupBox();
            this.chkKwMd = new System.Windows.Forms.CheckBox();
            this.chkResetDateTime = new System.Windows.Forms.CheckBox();
            this.chkResetMode = new System.Windows.Forms.CheckBox();
            this.chkResetNo = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.gbCheckBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkCumkWh
            // 
            this.chkCumkWh.AutoSize = true;
            this.chkCumkWh.Checked = true;
            this.chkCumkWh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCumkWh.Enabled = false;
            this.chkCumkWh.Location = new System.Drawing.Point(20, 90);
            this.chkCumkWh.Name = "chkCumkWh";
            this.chkCumkWh.Size = new System.Drawing.Size(73, 17);
            this.chkCumkWh.TabIndex = 12;
            this.chkCumkWh.Text = "Cum kWh";
            this.chkCumkWh.UseVisualStyleBackColor = true;
            // 
            // gbCheckBox
            // 
            this.gbCheckBox.Controls.Add(this.chkKwMd);
            this.gbCheckBox.Controls.Add(this.chkCumkWh);
            this.gbCheckBox.Controls.Add(this.chkResetDateTime);
            this.gbCheckBox.Controls.Add(this.chkResetMode);
            this.gbCheckBox.Controls.Add(this.chkResetNo);
            this.gbCheckBox.Location = new System.Drawing.Point(6, 12);
            this.gbCheckBox.Name = "gbCheckBox";
            this.gbCheckBox.Size = new System.Drawing.Size(325, 164);
            this.gbCheckBox.TabIndex = 23;
            this.gbCheckBox.TabStop = false;
            // 
            // chkKwMd
            // 
            this.chkKwMd.AutoSize = true;
            this.chkKwMd.Checked = true;
            this.chkKwMd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKwMd.Enabled = false;
            this.chkKwMd.Location = new System.Drawing.Point(20, 114);
            this.chkKwMd.Name = "chkKwMd";
            this.chkKwMd.Size = new System.Drawing.Size(156, 17);
            this.chkKwMd.TabIndex = 13;
            this.chkKwMd.Text = "kW (MD) Occ. Date && Time";
            this.chkKwMd.UseVisualStyleBackColor = true;
            // 
            // chkResetDateTime
            // 
            this.chkResetDateTime.AutoSize = true;
            this.chkResetDateTime.Checked = true;
            this.chkResetDateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResetDateTime.Enabled = false;
            this.chkResetDateTime.Location = new System.Drawing.Point(20, 67);
            this.chkResetDateTime.Name = "chkResetDateTime";
            this.chkResetDateTime.Size = new System.Drawing.Size(115, 17);
            this.chkResetDateTime.TabIndex = 10;
            this.chkResetDateTime.Text = "Reset Date && Time";
            this.chkResetDateTime.UseVisualStyleBackColor = true;
            // 
            // chkResetMode
            // 
            this.chkResetMode.AutoSize = true;
            this.chkResetMode.Checked = true;
            this.chkResetMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResetMode.Enabled = false;
            this.chkResetMode.Location = new System.Drawing.Point(20, 43);
            this.chkResetMode.Name = "chkResetMode";
            this.chkResetMode.Size = new System.Drawing.Size(84, 17);
            this.chkResetMode.TabIndex = 9;
            this.chkResetMode.Text = "Reset Mode";
            this.chkResetMode.UseVisualStyleBackColor = true;
            // 
            // chkResetNo
            // 
            this.chkResetNo.AutoSize = true;
            this.chkResetNo.BackColor = System.Drawing.SystemColors.Control;
            this.chkResetNo.Checked = true;
            this.chkResetNo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResetNo.Enabled = false;
            this.chkResetNo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkResetNo.Location = new System.Drawing.Point(20, 19);
            this.chkResetNo.Name = "chkResetNo";
            this.chkResetNo.Size = new System.Drawing.Size(118, 17);
            this.chkResetNo.TabIndex = 8;
            this.chkResetNo.Text = "Reset No && Avg.PF";
            this.chkResetNo.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(256, 182);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(179, 182);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 21;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShow.ForeColor = System.Drawing.Color.White;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.FlatAppearance.BorderSize = 0;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // BillingReport_TNEB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 219);
            this.Controls.Add(this.gbCheckBox);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BillingReport_TNEB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Billing Report";
            this.Load += new System.EventHandler(this.BillingReport_TNEB_Load);
            this.gbCheckBox.ResumeLayout(false);
            this.gbCheckBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkCumkWh;
        private System.Windows.Forms.GroupBox gbCheckBox;
        private System.Windows.Forms.CheckBox chkResetDateTime;
        private System.Windows.Forms.CheckBox chkResetMode;
        private System.Windows.Forms.CheckBox chkResetNo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.CheckBox chkKwMd;

    }
}


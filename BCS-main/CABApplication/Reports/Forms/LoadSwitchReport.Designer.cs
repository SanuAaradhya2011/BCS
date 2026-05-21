namespace CAB.UI
{
    partial class LoadSwitchReport
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
            System.Windows.Forms.GroupBox groupLoadSurvey;
            this.lblNoDataFound = new System.Windows.Forms.Label();
            this.chkListLoadSwitchParameters = new System.Windows.Forms.CheckedListBox();
            this.SMD_btnCancel = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            groupLoadSurvey = new System.Windows.Forms.GroupBox();
            groupLoadSurvey.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupLoadSurvey
            // 
            groupLoadSurvey.Controls.Add(this.lblNoDataFound);
            groupLoadSurvey.Controls.Add(this.chkListLoadSwitchParameters);
            groupLoadSurvey.Location = new System.Drawing.Point(12, 10);
            groupLoadSurvey.Name = "groupLoadSurvey";
            groupLoadSurvey.Size = new System.Drawing.Size(301, 285);
            groupLoadSurvey.TabIndex = 8;
            groupLoadSurvey.TabStop = false;
            groupLoadSurvey.Text = "Load Switch Parameters";
            // 
            // lblNoDataFound
            // 
            this.lblNoDataFound.AutoSize = true;
            this.lblNoDataFound.BackColor = System.Drawing.Color.White;
            this.lblNoDataFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoDataFound.ForeColor = System.Drawing.Color.Red;
            this.lblNoDataFound.Location = new System.Drawing.Point(13, 21);
            this.lblNoDataFound.Name = "lblNoDataFound";
            this.lblNoDataFound.Size = new System.Drawing.Size(98, 13);
            this.lblNoDataFound.TabIndex = 1;
            this.lblNoDataFound.Text = "lblNoDataFound";
            // 
            // chkListLoadSwitchParameters
            // 
            this.chkListLoadSwitchParameters.BackColor = System.Drawing.Color.White;
            this.chkListLoadSwitchParameters.CheckOnClick = true;
            this.chkListLoadSwitchParameters.FormattingEnabled = true;
            this.chkListLoadSwitchParameters.Location = new System.Drawing.Point(7, 17);
            this.chkListLoadSwitchParameters.Name = "chkListLoadSwitchParameters";
            this.chkListLoadSwitchParameters.Size = new System.Drawing.Size(287, 259);
            this.chkListLoadSwitchParameters.TabIndex = 0;
            // 
            // SMD_btnCancel
            // 
            this.SMD_btnCancel.Location = new System.Drawing.Point(234, 301);
            this.SMD_btnCancel.Name = "SMD_btnCancel";
            this.SMD_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.SMD_btnCancel.TabIndex = 7;
            this.SMD_btnCancel.Text = "Cancel";
            this.SMD_btnCancel.UseVisualStyleBackColor = false;
            this.SMD_btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.SMD_btnCancel.ForeColor = System.Drawing.Color.White;
            this.SMD_btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SMD_btnCancel.FlatAppearance.BorderSize = 0;
            this.SMD_btnCancel.Click += new System.EventHandler(this.SMD_btnCancel_Click);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(150, 301);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 6;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShow.ForeColor = System.Drawing.Color.White;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.FlatAppearance.BorderSize = 0;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // LoadSwitchReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 334);
            this.Controls.Add(groupLoadSurvey);
            this.Controls.Add(this.SMD_btnCancel);
            this.Controls.Add(this.btnShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadSwitchReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Switch";
            this.Load += new System.EventHandler(this.LoadSurveyReport_Load);
            groupLoadSurvey.ResumeLayout(false);
            groupLoadSurvey.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SMD_btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.CheckedListBox chkListLoadSwitchParameters;
        private System.Windows.Forms.Label lblNoDataFound;
    }
}

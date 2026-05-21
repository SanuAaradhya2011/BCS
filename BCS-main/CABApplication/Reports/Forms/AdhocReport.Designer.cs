namespace CABApplication.Reports.Forms
{
    partial class AdhocReport
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.lblNoDataFound = new System.Windows.Forms.Label();
            this.chkListadhocParameters = new System.Windows.Forms.CheckedListBox();
            groupLoadSurvey = new System.Windows.Forms.GroupBox();
            groupLoadSurvey.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(247, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 23);
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
            this.btnShow.Location = new System.Drawing.Point(180, 292);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(63, 23);
            this.btnShow.TabIndex = 21;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShow.ForeColor = System.Drawing.Color.White;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.FlatAppearance.BorderSize = 0;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // groupLoadSurvey
            // 
            groupLoadSurvey.Controls.Add(this.lblNoDataFound);
            groupLoadSurvey.Controls.Add(this.chkListadhocParameters);
            groupLoadSurvey.Location = new System.Drawing.Point(10, 2);
            groupLoadSurvey.Name = "groupLoadSurvey";
            groupLoadSurvey.Size = new System.Drawing.Size(301, 285);
            groupLoadSurvey.TabIndex = 23;
            groupLoadSurvey.TabStop = false;
            groupLoadSurvey.Text = "Ad Hoc Parameters";
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
            // chkListadhocParameters
            // 
            this.chkListadhocParameters.BackColor = System.Drawing.Color.White;
            this.chkListadhocParameters.CheckOnClick = true;
            this.chkListadhocParameters.FormattingEnabled = true;
            this.chkListadhocParameters.Location = new System.Drawing.Point(7, 15);
            this.chkListadhocParameters.Name = "chkListadhocParameters";
            this.chkListadhocParameters.Size = new System.Drawing.Size(287, 259);
            this.chkListadhocParameters.TabIndex = 0;
            // 
            // AdhocReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 322);
            this.Controls.Add(groupLoadSurvey);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdhocReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ad hoc Report";
            this.Load += new System.EventHandler(this.AdhocReport_Load);
            groupLoadSurvey.ResumeLayout(false);
            groupLoadSurvey.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Label lblNoDataFound;
        private System.Windows.Forms.CheckedListBox chkListadhocParameters;
    }
}

namespace CAB.UI
{
    partial class MidNightReportFileWise
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
            this.btnShow = new System.Windows.Forms.Button();
            this.SMD_btnCancel = new System.Windows.Forms.Button();
            this.lblNoDataFound = new System.Windows.Forms.Label();
            this.chkListMidnightParameters = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(111, 229);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(75, 23);
            this.btnShow.TabIndex = 7;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = false;
            this.btnShow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnShow.ForeColor = System.Drawing.Color.White;
            this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShow.FlatAppearance.BorderSize = 0;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // SMD_btnCancel
            // 
            this.SMD_btnCancel.Location = new System.Drawing.Point(199, 229);
            this.SMD_btnCancel.Name = "SMD_btnCancel";
            this.SMD_btnCancel.Size = new System.Drawing.Size(75, 23);
            this.SMD_btnCancel.TabIndex = 8;
            this.SMD_btnCancel.Text = "Cancel";
            this.SMD_btnCancel.UseVisualStyleBackColor = false;
            this.SMD_btnCancel.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.SMD_btnCancel.ForeColor = System.Drawing.Color.White;
            this.SMD_btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SMD_btnCancel.FlatAppearance.BorderSize = 0;
            this.SMD_btnCancel.Click += new System.EventHandler(this.SMD_btnCancel_Click);
            // 
            // lblNoDataFound
            // 
            this.lblNoDataFound.AutoSize = true;
            this.lblNoDataFound.BackColor = System.Drawing.Color.White;
            this.lblNoDataFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoDataFound.ForeColor = System.Drawing.Color.Red;
            this.lblNoDataFound.Location = new System.Drawing.Point(13, 16);
            this.lblNoDataFound.Name = "lblNoDataFound";
            this.lblNoDataFound.Size = new System.Drawing.Size(98, 13);
            this.lblNoDataFound.TabIndex = 9;
            this.lblNoDataFound.Text = "lblNoDataFound";
            // 
            // chkListMidnightParameters
            // 
            this.chkListMidnightParameters.CheckOnClick = true;
            this.chkListMidnightParameters.FormattingEnabled = true;
            this.chkListMidnightParameters.HorizontalScrollbar = true;
            this.chkListMidnightParameters.Location = new System.Drawing.Point(12, 12);
            this.chkListMidnightParameters.Name = "chkListMidnightParameters";
            this.chkListMidnightParameters.Size = new System.Drawing.Size(320, 199);
            this.chkListMidnightParameters.TabIndex = 10;
            // 
            // MidNightReportFileWise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(348, 277);
            this.Controls.Add(this.lblNoDataFound);
            this.Controls.Add(this.SMD_btnCancel);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.chkListMidnightParameters);
            this.Name = "MidNightReportFileWise";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Midnight Report";
            this.Load += new System.EventHandler(this.MidNightReportFileWise_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button SMD_btnCancel;
        private System.Windows.Forms.Label lblNoDataFound;
        private System.Windows.Forms.CheckedListBox chkListMidnightParameters;
    }
}

namespace CABApplication.Scheduling
{
    partial class GPRSSchedulingReport
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
            this.components = new System.ComponentModel.Container();
            this.tabReports = new CAB.UI.Controls.PremiumTabControl();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.buttonDownload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tabReports
            // 
            this.tabReports.Location = new System.Drawing.Point(0, 2);
            this.tabReports.Name = "tabReports";
            this.tabReports.SelectedIndex = 0;
            this.tabReports.Size = new System.Drawing.Size(1324, 560);
            this.tabReports.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabReports.TabIndex = 0;
            this.tabReports.SelectedIndexChanged += new System.EventHandler(this.tabReports_SelectedIndexChanged);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 20000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(1249, 568);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(75, 23);
            this.buttonDownload.TabIndex = 1;
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = false;
            this.buttonDownload.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.buttonDownload.ForeColor = System.Drawing.Color.White;
            this.buttonDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDownload.FlatAppearance.BorderSize = 0;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // GPRSSchedulingReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1327, 611);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.tabReports);
            this.Name = "GPRSSchedulingReport";
            this.Text = "GPRS Scheduling Report";
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.PremiumTabControl tabReports;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button buttonDownload;
    }
}


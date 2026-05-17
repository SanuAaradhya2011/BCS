namespace CABApplication.Scheduling
{
    partial class SchedulingReportConfiguration
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
            this.tabProfiles = new CAB.UI.Controls.PremiumTabControl();
            this.SuspendLayout();
            // 
            // tabProfiles
            // 
            this.tabProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabProfiles.Location = new System.Drawing.Point(0, 0);
            this.tabProfiles.Name = "tabProfiles";
            this.tabProfiles.SelectedIndex = 0;
            this.tabProfiles.Size = new System.Drawing.Size(862, 626);
            this.tabProfiles.TabIndex = 0;
            
            // 
            // SchedulingReportConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 626);
            this.Controls.Add(this.tabProfiles);
            this.Name = "SchedulingReportConfiguration";
            this.Text = "Scheduling Report Configuration";
            this.ResumeLayout(false);

        }

        #endregion

        private CAB.UI.Controls.PremiumTabControl tabProfiles;
    }
}

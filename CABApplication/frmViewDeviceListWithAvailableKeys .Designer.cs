namespace CABApplication
{
    partial class frmViewDeviceListWithAvailableKeys
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
            this.listBoxAvailableMeters = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxAvailableMeters
            // 
            this.listBoxAvailableMeters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAvailableMeters.Enabled = false;
            this.listBoxAvailableMeters.FormattingEnabled = true;
            this.listBoxAvailableMeters.Location = new System.Drawing.Point(0, 0);
            this.listBoxAvailableMeters.Name = "listBoxAvailableMeters";
            this.listBoxAvailableMeters.Size = new System.Drawing.Size(259, 315);
            this.listBoxAvailableMeters.TabIndex = 1;
            // 
            // frmViewDeviceListWithAvailableKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 315);
            this.Controls.Add(this.listBoxAvailableMeters);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViewDeviceListWithAvailableKeys";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device List With Retrieved Keys";
            this.Load += new System.EventHandler(this.FrmViewDeviceListWithAvailableKeys_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxAvailableMeters;
    }
}
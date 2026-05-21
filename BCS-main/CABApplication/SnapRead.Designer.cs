namespace CABApplication
{
    partial class SnapRead
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
            this.tabControlInstant = new CAB.UI.Controls.PremiumTabControl();
            this.tabPageReading = new System.Windows.Forms.TabPage();
            this.btnReadSnap = new System.Windows.Forms.Button();
            this.btnHoldSnap = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelSnap = new System.Windows.Forms.Button();
            this.lngInstant = new CAB.UI.Controls.CABGridControl();
            this.progressBarTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tabControlInstant.SuspendLayout();
            this.tabPageReading.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlInstant
            // 
            this.tabControlInstant.Controls.Add(this.tabPageReading);
            this.tabControlInstant.Location = new System.Drawing.Point(12, 12);
            this.tabControlInstant.Name = "tabControlInstant";
            this.tabControlInstant.SelectedIndex = 0;
            this.tabControlInstant.Size = new System.Drawing.Size(966, 475);
            this.tabControlInstant.TabIndex = 1;
            // 
            // tabPageReading
            // 
            this.tabPageReading.AutoScroll = true;
            this.tabPageReading.Controls.Add(this.btnReadSnap);
            this.tabPageReading.Controls.Add(this.btnHoldSnap);
            this.tabPageReading.Controls.Add(this.label1);
            this.tabPageReading.Controls.Add(this.btnCancelSnap);
            this.tabPageReading.Controls.Add(this.lngInstant);
            this.tabPageReading.Location = new System.Drawing.Point(4, 22);
            this.tabPageReading.Name = "tabPageReading";
            this.tabPageReading.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReading.Size = new System.Drawing.Size(958, 449);
            this.tabPageReading.TabIndex = 0;
            this.tabPageReading.Text = "Snap Read";
            // 
            // btnReadSnap
            // 
            this.btnReadSnap.Location = new System.Drawing.Point(618, 414);
            this.btnReadSnap.Name = "btnReadSnap";
            this.btnReadSnap.Size = new System.Drawing.Size(83, 29);
            this.btnReadSnap.TabIndex = 17;
            this.btnReadSnap.Text = "Read Data";
            this.btnReadSnap.UseVisualStyleBackColor = false;
            this.btnReadSnap.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnReadSnap.ForeColor = System.Drawing.Color.White;
            this.btnReadSnap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReadSnap.FlatAppearance.BorderSize = 0;
            this.btnReadSnap.Click += new System.EventHandler(this.btnReadSnapRead_Click);
            // 
            // btnHoldSnap
            // 
            this.btnHoldSnap.Enabled = false;
            this.btnHoldSnap.Location = new System.Drawing.Point(707, 414);
            this.btnHoldSnap.Name = "btnHoldSnap";
            this.btnHoldSnap.Size = new System.Drawing.Size(83, 29);
            this.btnHoldSnap.TabIndex = 19;
            this.btnHoldSnap.Text = "Hold";
            this.btnHoldSnap.UseVisualStyleBackColor = false;
            this.btnHoldSnap.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnHoldSnap.ForeColor = System.Drawing.Color.White;
            this.btnHoldSnap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHoldSnap.FlatAppearance.BorderSize = 0;
            this.btnHoldSnap.Click += new System.EventHandler(this.btnHoldSnapRead_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(273, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "To Start Snap Read Click on Read Data.";
            // 
            // btnCancelSnap
            // 
            this.btnCancelSnap.Location = new System.Drawing.Point(796, 414);
            this.btnCancelSnap.Name = "btnCancelSnap";
            this.btnCancelSnap.Size = new System.Drawing.Size(83, 29);
            this.btnCancelSnap.TabIndex = 18;
            this.btnCancelSnap.Text = "Cancel";
            this.btnCancelSnap.UseVisualStyleBackColor = false;
            this.btnCancelSnap.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnCancelSnap.ForeColor = System.Drawing.Color.White;
            this.btnCancelSnap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelSnap.FlatAppearance.BorderSize = 0;
            this.btnCancelSnap.Click += new System.EventHandler(this.btnCancelSnapRead_Click);
            // 
            // lngInstant
            // 
            this.lngInstant.AutoScroll = true;
            this.lngInstant.Data = null;
            this.lngInstant.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lngInstant.HiddenColumn = null;
            this.lngInstant.HiddenColumns = null;
            this.lngInstant.IsEqual = false;
            this.lngInstant.IsFullRow = false;
            this.lngInstant.IsSorting = false;
            this.lngInstant.Location = new System.Drawing.Point(7, 7);
            this.lngInstant.Name = "lngInstant";
            this.lngInstant.SelectedIndex = 0;
            this.lngInstant.SelectedRowId = "";
            this.lngInstant.Size = new System.Drawing.Size(945, 407);
            this.lngInstant.TabIndex = 0;
            this.lngInstant.ValueColumn = null;
            // 
            // progressBarTimer
            // 
            this.progressBarTimer.Tick += new System.EventHandler(this.progressBarTimer_Tick);
            // 
            // statusStrip
            // 
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 612);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(980, 22);
            this.statusStrip.TabIndex = 58;
            this.statusStrip.Text = "statusStrip1";
            this.statusStrip.Visible = false;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // SnapRead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(980, 634);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControlInstant);
            this.Name = "SnapRead";
            this.StatusMessage = "";
            this.Text = "Dynamic Snap Read";
            this.Load += new System.EventHandler(this.SnapRead_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SnapRead_FormClosing);
            this.tabControlInstant.ResumeLayout(false);
            this.tabPageReading.ResumeLayout(false);
            this.tabPageReading.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CAB.UI.Controls.PremiumTabControl tabControlInstant;
        private System.Windows.Forms.Button btnHoldSnap;
        private System.Windows.Forms.Button btnCancelSnap;
        private System.Windows.Forms.Button btnReadSnap;
        private System.Windows.Forms.TabPage tabPageReading;
        private System.Windows.Forms.Label label1;
        private CAB.UI.Controls.CABGridControl lngInstant;
        private System.Windows.Forms.Timer progressBarTimer;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;

    }
}

